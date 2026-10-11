SET NOCOUNT ON;
DECLARE @progress_msg NVARCHAR(400);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'start' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'start', N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};
DECLARE @now DATETIME = GETDATE();
DECLARE @apply_chunk INT = 20000;
DECLARE @updated INT = 0;
DECLARE @processed INT = 0;
DECLARE @chunk_n INT = 0;
DECLARE @chunk_updated INT = 0;
DECLARE @costing_period_raw NVARCHAR(40) = LTRIM(RTRIM(N'{{CostingPeriod}}'));
DECLARE @costing_y INT = TRY_CONVERT(INT, LEFT(@costing_period_raw, 4));
DECLARE @costing_m INT = TRY_CONVERT(INT, SUBSTRING(@costing_period_raw, 6, 2));
IF @costing_period_raw IS NULL
  OR @costing_period_raw = N''
  OR SUBSTRING(@costing_period_raw, 5, 1) <> N'-'
  OR @costing_y IS NULL
  OR @costing_m IS NULL
  OR @costing_m < 1
  OR @costing_m > 12
BEGIN
  SET @costing_y = YEAR(GETDATE());
  SET @costing_m = MONTH(GETDATE());
END
DECLARE @period_start DATE = DATEFROMPARTS(@costing_y, @costing_m, 1);
DECLARE @period_end_exclusive DATE = DATEADD(MONTH, 1, @period_start);
DECLARE @costing_period NVARCHAR(7) = CONVERT(CHAR(7), @period_start, 23);

-- =============================================================================
-- QT_SYNC_BC_BK：BOM 物料成本明细采购价回填（对齐 TaktBomCalculatePurchaseInfoRecordHelper）
-- 大数据：采购价主/条件/等级一次性装入临时表；BOM 明细按核算月 + TOP(@apply_chunk=20000) 循环
-- 须选业务库（如 zTakt_000_Dev），勿选暂存库 zTakt_900_Dev
-- 核算月：ExecuteParams.costingPeriod（yyyy-MM）；空则当月
-- 唯一写入目标：takt_logistics_manufacturing_bom_material_cost_item
--   purchase_organization ← 采购信息记录主表 plant_code（采购组织=工厂码）
--   purchase_group        ← 主表 purchase_group
--   supplier_code         ← 主表 supplier_code
--   net_purchase_price    ← 条件行/数量等级/价值等级解析净价（5 位）
--   purchase_price_unit   ← 条件行 price_unit（≤0 则 1）
--   purchase_currency_code← 条件行 condition_currency_code
-- 匹配：BOM 组件 material_key ↔ 采购信息记录 material_key（不按工厂过滤）
-- 选价：仅 ValidFrom≤核算日取最晚（例：6/1 与 7/1 → 核算 6/30 用 6/1，核算 7/31 用 7/1）
-- ❌ 不用未来 ValidFrom；无 ≤核算日 价格则跳过（不回填 0）
-- 仅空回填：已有采购组织/组/供应商/货币/净价≠0/价格单位>1 一律保留
-- 本次写入合并到 ext_field：$._bk.bc（含 price_info=定价记录号：供应商：有效起始日：价格）
-- 同序：PB00 / Id 大优先
-- 条件行：未作废；PB00 优先，再按 purchase_info_record_seq / id
-- 净价：scale_basis=C 或（非 B 且有数量等级）→ 数量等级；否则有价值等级 → 价值等级；否则条件行 price
-- 建议：在 QT_SYNC_BC / QT_SYNC_PUP 之后执行
-- =============================================================================

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#pp_item') IS NOT NULL DROP TABLE #pp_item;
IF OBJECT_ID('tempdb..#qty_scale') IS NOT NULL DROP TABLE #qty_scale;
IF OBJECT_ID('tempdb..#val_scale') IS NOT NULL DROP TABLE #val_scale;
IF OBJECT_ID('tempdb..#done') IS NOT NULL DROP TABLE #done;
IF OBJECT_ID('tempdb..#chunk') IS NOT NULL DROP TABLE #chunk;
IF OBJECT_ID('tempdb..#hdr_pick') IS NOT NULL DROP TABLE #hdr_pick;
IF OBJECT_ID('tempdb..#item_pick') IS NOT NULL DROP TABLE #item_pick;
IF OBJECT_ID('tempdb..#fill') IS NOT NULL DROP TABLE #fill;

CREATE TABLE #hdr (
  [header_id] BIGINT NOT NULL,
  [mat_key] NVARCHAR(20) NOT NULL,
  [purchase_organization] NVARCHAR(4) NOT NULL,
  [purchase_group] NVARCHAR(3) NOT NULL,
  [supplier_code] NVARCHAR(10) NOT NULL,
  [purchase_info_record_code] NVARCHAR(20) NOT NULL,
  [valid_from_d] DATE NOT NULL,
  [pb00_rank] INT NOT NULL
);

CREATE TABLE #pp_item (
  [header_id] BIGINT NOT NULL PRIMARY KEY,
  [item_id] BIGINT NOT NULL,
  [scale_basis] NVARCHAR(4) NOT NULL,
  [item_price] DECIMAL(18,5) NOT NULL,
  [purchase_price_unit] INT NOT NULL,
  [purchase_currency_code] NVARCHAR(3) NOT NULL
);

CREATE TABLE #qty_scale (
  [item_id] BIGINT NOT NULL,
  [scale_quantity] DECIMAL(18,5) NOT NULL,
  [price] DECIMAL(18,5) NOT NULL,
  [id] BIGINT NOT NULL
);

CREATE TABLE #val_scale (
  [item_id] BIGINT NOT NULL,
  [scale_value] DECIMAL(18,5) NOT NULL,
  [price] DECIMAL(18,5) NOT NULL,
  [id] BIGINT NOT NULL
);

CREATE TABLE #done (
  [id] BIGINT NOT NULL PRIMARY KEY
);

INSERT INTO #hdr (
  [header_id], [mat_key], [purchase_organization], [purchase_group],
  [supplier_code], [purchase_info_record_code], [valid_from_d], [pb00_rank]
)
SELECT
  h.[id],
  CASE
    WHEN LEN(LTRIM(RTRIM(h.[material_code]))) = 18
      AND LTRIM(RTRIM(h.[material_code])) NOT LIKE N'%[^0-9]%'
    THEN RIGHT(LTRIM(RTRIM(h.[material_code])), 10)
    ELSE LTRIM(RTRIM(h.[material_code]))
  END,
  LEFT(LTRIM(RTRIM(ISNULL(h.[plant_code], N''))), 4),
  LEFT(LTRIM(RTRIM(ISNULL(h.[purchase_group], N''))), 3),
  LEFT(LTRIM(RTRIM(ISNULL(h.[supplier_code], N''))), 10),
  LEFT(LTRIM(RTRIM(ISNULL(h.[purchase_info_record_code], N''))), 20),
  CAST(h.[valid_from] AS DATE),
  CASE WHEN UPPER(LTRIM(RTRIM(h.[price_type]))) = N'PB00' THEN 0 ELSE 1 END
FROM [dbo].[takt_logistics_procurement_purchase_info_record] AS h
WHERE h.[tenant_code] = @tenant_code
  AND h.[company_code] = @company_code
  AND h.[is_deleted] = 0
  AND LTRIM(RTRIM(ISNULL(h.[material_code], N''))) <> N'';

CREATE NONCLUSTERED INDEX [ix_hdr_mat] ON #hdr ([mat_key], [valid_from_d]);

;WITH items AS (
  SELECT
    i.[id] AS [item_id],
    i.[purchase_info_record_id] AS [header_id],
    LTRIM(RTRIM(ISNULL(i.[scale_basis], N''))) AS [scale_basis],
    CAST(i.[price] AS DECIMAL(18,5)) AS [item_price],
    CASE WHEN i.[price_unit] <= 0 THEN 1 ELSE i.[price_unit] END AS [purchase_price_unit],
    LEFT(LTRIM(RTRIM(ISNULL(i.[condition_currency_code], N''))), 3) AS [purchase_currency_code],
    ROW_NUMBER() OVER (
      PARTITION BY i.[purchase_info_record_id]
      ORDER BY
        CASE WHEN UPPER(LTRIM(RTRIM(i.[price_type]))) = N'PB00' THEN 0 ELSE 1 END,
        i.[purchase_info_record_seq] ASC,
        i.[id] ASC
    ) AS [rn]
  FROM [dbo].[takt_logistics_procurement_purchase_info_record_item] AS i
  WHERE i.[tenant_code] = @tenant_code
    AND i.[company_code] = @company_code
    AND i.[is_deleted] = 0
    AND i.[is_obsolete] = 0
)
INSERT INTO #pp_item (
  [header_id], [item_id], [scale_basis], [item_price], [purchase_price_unit], [purchase_currency_code]
)
SELECT
  [header_id], [item_id], [scale_basis], [item_price], [purchase_price_unit], [purchase_currency_code]
FROM items
WHERE [rn] = 1;

INSERT INTO #qty_scale ([item_id], [scale_quantity], [price], [id])
SELECT
  q.[purchase_info_record_item_id],
  CAST(q.[scale_quantity] AS DECIMAL(18,5)),
  CAST(q.[price] AS DECIMAL(18,5)),
  q.[id]
FROM [dbo].[takt_logistics_procurement_purchase_info_record_scale_quantity] AS q
INNER JOIN #pp_item AS p ON p.[item_id] = q.[purchase_info_record_item_id]
WHERE q.[tenant_code] = @tenant_code
  AND q.[company_code] = @company_code
  AND q.[is_deleted] = 0
  AND q.[is_obsolete] = 0;

CREATE NONCLUSTERED INDEX [ix_qty_item] ON #qty_scale ([item_id], [scale_quantity], [id]);

INSERT INTO #val_scale ([item_id], [scale_value], [price], [id])
SELECT
  v.[purchase_info_record_item_id],
  CAST(v.[scale_value] AS DECIMAL(18,5)),
  CAST(v.[price] AS DECIMAL(18,5)),
  v.[id]
FROM [dbo].[takt_logistics_procurement_purchase_info_record_scale_value] AS v
INNER JOIN #pp_item AS p ON p.[item_id] = v.[purchase_info_record_item_id]
WHERE v.[tenant_code] = @tenant_code
  AND v.[company_code] = @company_code
  AND v.[is_deleted] = 0
  AND v.[is_obsolete] = 0;

CREATE NONCLUSTERED INDEX [ix_val_item] ON #val_scale ([item_id], [scale_value], [id]);

WHILE 1 = 1
BEGIN
  IF OBJECT_ID('tempdb..#chunk') IS NOT NULL DROP TABLE #chunk;
  IF OBJECT_ID('tempdb..#hdr_pick') IS NOT NULL DROP TABLE #hdr_pick;
  IF OBJECT_ID('tempdb..#item_pick') IS NOT NULL DROP TABLE #item_pick;
  IF OBJECT_ID('tempdb..#fill') IS NOT NULL DROP TABLE #fill;

  SELECT TOP (@apply_chunk)
    t.[id] AS [bom_id],
    CASE
      WHEN LEN(LTRIM(RTRIM(t.[component_code]))) = 18
        AND LTRIM(RTRIM(t.[component_code])) NOT LIKE N'%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[component_code])), 10)
      ELSE LTRIM(RTRIM(t.[component_code]))
    END AS [mat_key],
    CAST(t.[component_quantity] AS DECIMAL(18,5)) AS [component_quantity],
    CAST(t.[costing_date] AS DATE) AS [cost_day]
  INTO #chunk
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND t.[costing_date] >= @period_start
    AND t.[costing_date] < @period_end_exclusive
    AND LTRIM(RTRIM(ISNULL(t.[component_code], N''))) <> N''
    AND (
      LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N''
      OR LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N''
      OR LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N''
      OR t.[net_purchase_price] = 0
      OR t.[purchase_price_unit] <= 1
      OR LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N''
    )
    AND NOT EXISTS (SELECT 1 FROM #done AS d WHERE d.[id] = t.[id])
  ORDER BY t.[id];

  SET @chunk_n = @@ROWCOUNT;
  IF @chunk_n = 0
    BREAK;

  ALTER TABLE #chunk ADD CONSTRAINT [pk_bc_bk_chunk] PRIMARY KEY CLUSTERED ([bom_id]);
  CREATE NONCLUSTERED INDEX [ix_chunk_mat] ON #chunk ([mat_key], [cost_day]);

  INSERT INTO #done ([id])
  SELECT [bom_id] FROM #chunk;
  SET @processed = @processed + @chunk_n;

  ;WITH joined AS (
    SELECT
      c.[bom_id],
      c.[component_quantity],
      h.[header_id],
      h.[purchase_organization],
      h.[purchase_group],
      h.[supplier_code],
      h.[purchase_info_record_code],
      h.[valid_from_d],
      h.[pb00_rank]
    FROM #chunk AS c
    INNER JOIN #hdr AS h
      ON h.[mat_key] = c.[mat_key]
     AND h.[valid_from_d] <= c.[cost_day]
     AND c.[mat_key] <> N''
  ),
  ranked AS (
    SELECT
      j.*,
      ROW_NUMBER() OVER (
        PARTITION BY j.[bom_id]
        ORDER BY
          j.[valid_from_d] DESC,
          j.[pb00_rank] ASC,
          j.[header_id] DESC
      ) AS [rn]
    FROM joined AS j
  )
  SELECT
    [bom_id],
    [component_quantity],
    [header_id],
    [purchase_organization],
    [purchase_group],
    [supplier_code],
    [purchase_info_record_code],
    [valid_from_d]
  INTO #hdr_pick
  FROM ranked
  WHERE [rn] = 1;

  SELECT
    h.[bom_id],
    h.[component_quantity],
    h.[header_id],
    h.[purchase_organization],
    h.[purchase_group],
    h.[supplier_code],
    h.[purchase_info_record_code],
    h.[valid_from_d],
    i.[item_id],
    i.[scale_basis],
    i.[item_price],
    i.[purchase_price_unit],
    i.[purchase_currency_code]
  INTO #item_pick
  FROM #hdr_pick AS h
  INNER JOIN #pp_item AS i
    ON i.[header_id] = h.[header_id];

  SELECT
    p.[bom_id],
    LEFT(p.[purchase_organization], 4) AS [purchase_organization],
    LEFT(p.[purchase_group], 3) AS [purchase_group],
    LEFT(p.[supplier_code], 10) AS [supplier_code],
    LEFT(p.[purchase_info_record_code], 20) AS [purchase_info_record_code],
    p.[valid_from_d],
    ROUND(
      CASE
        WHEN (
          UPPER(p.[scale_basis]) = N'C'
          OR (UPPER(p.[scale_basis]) <> N'B' AND qp_any.[has_row] = 1)
        ) AND qp_any.[has_row] = 1
        THEN ISNULL(qp_match.[scale_price], ISNULL(qp_min.[scale_price], p.[item_price]))
        WHEN vp_any.[has_row] = 1
        THEN ISNULL(vp_match.[scale_price], ISNULL(vp_min.[scale_price], p.[item_price]))
        ELSE p.[item_price]
      END,
      5
    ) AS [net_purchase_price],
    p.[purchase_price_unit],
    LEFT(ISNULL(p.[purchase_currency_code], N''), 3) AS [purchase_currency_code]
  INTO #fill
  FROM #item_pick AS p
  OUTER APPLY (
    SELECT CASE WHEN EXISTS (SELECT 1 FROM #qty_scale AS q WHERE q.[item_id] = p.[item_id]) THEN 1 ELSE 0 END AS [has_row]
  ) AS qp_any
  OUTER APPLY (
    SELECT CASE WHEN EXISTS (SELECT 1 FROM #val_scale AS v WHERE v.[item_id] = p.[item_id]) THEN 1 ELSE 0 END AS [has_row]
  ) AS vp_any
  OUTER APPLY (
    SELECT TOP (1) CAST(q.[price] AS DECIMAL(18,5)) AS [scale_price]
    FROM #qty_scale AS q
    WHERE q.[item_id] = p.[item_id]
      AND q.[scale_quantity] <= p.[component_quantity]
    ORDER BY q.[scale_quantity] DESC, q.[id] DESC
  ) AS qp_match
  OUTER APPLY (
    SELECT TOP (1) CAST(q.[price] AS DECIMAL(18,5)) AS [scale_price]
    FROM #qty_scale AS q
    WHERE q.[item_id] = p.[item_id]
    ORDER BY q.[scale_quantity] ASC, q.[id] ASC
  ) AS qp_min
  OUTER APPLY (
    SELECT TOP (1) CAST(v.[price] AS DECIMAL(18,5)) AS [scale_price]
    FROM #val_scale AS v
    WHERE v.[item_id] = p.[item_id]
      AND v.[scale_value] <= p.[component_quantity]
    ORDER BY v.[scale_value] DESC, v.[id] DESC
  ) AS vp_match
  OUTER APPLY (
    SELECT TOP (1) CAST(v.[price] AS DECIMAL(18,5)) AS [scale_price]
    FROM #val_scale AS v
    WHERE v.[item_id] = p.[item_id]
    ORDER BY v.[scale_value] ASC, v.[id] ASC
  ) AS vp_min;

  UPDATE t
  SET
    t.[purchase_organization] = CASE
      WHEN LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N''
        AND LTRIM(RTRIM(ISNULL(f.[purchase_organization], N''))) <> N''
      THEN f.[purchase_organization] ELSE t.[purchase_organization] END,
    t.[purchase_group] = CASE
      WHEN LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N''
        AND LTRIM(RTRIM(ISNULL(f.[purchase_group], N''))) <> N''
      THEN f.[purchase_group] ELSE t.[purchase_group] END,
    t.[supplier_code] = CASE
      WHEN LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N''
        AND LTRIM(RTRIM(ISNULL(f.[supplier_code], N''))) <> N''
      THEN f.[supplier_code] ELSE t.[supplier_code] END,
    t.[net_purchase_price] = CASE
      WHEN t.[net_purchase_price] = 0 AND f.[net_purchase_price] <> 0
      THEN f.[net_purchase_price] ELSE t.[net_purchase_price] END,
    t.[purchase_price_unit] = CASE
      WHEN t.[purchase_price_unit] <= 1 AND f.[purchase_price_unit] > 1
      THEN f.[purchase_price_unit] ELSE t.[purchase_price_unit] END,
    t.[purchase_currency_code] = CASE
      WHEN LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N''
        AND LTRIM(RTRIM(ISNULL(f.[purchase_currency_code], N''))) <> N''
      THEN f.[purchase_currency_code] ELSE t.[purchase_currency_code] END,
    t.[ext_field] = LEFT(x.[new_ext], 4000),
    t.[updated_by] = @sync_user_id,
    t.[updated_at] = @now
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
  INNER JOIN #fill AS f ON f.[bom_id] = t.[id]
  CROSS APPLY (
    SELECT
      CASE
        WHEN ISJSON(NULLIF(LTRIM(RTRIM(ISNULL(t.[ext_field], N''))), N'')) = 1
        THEN LTRIM(RTRIM(t.[ext_field]))
        ELSE N'{}'
      END AS [base_ext]
  ) AS e0
  CROSS APPLY (
    SELECT
      CASE
        WHEN JSON_QUERY(e0.[base_ext], N'$._bk') IS NULL
        THEN JSON_MODIFY(e0.[base_ext], N'lax $._bk', JSON_QUERY(N'{}'))
        ELSE e0.[base_ext]
      END AS [with_bk]
  ) AS e1
  CROSS APPLY (
    SELECT
      JSON_MODIFY(
        JSON_MODIFY(
          JSON_MODIFY(
            JSON_MODIFY(
              JSON_MODIFY(
                JSON_MODIFY(
                  JSON_MODIFY(
                    JSON_MODIFY(
                      JSON_MODIFY(
                        JSON_MODIFY(N'{}', N'$.at', CONVERT(VARCHAR(19), @now, 126)),
                        N'$.price_info',
                        CONCAT(
                          N'定价记录号：', ISNULL(f.[purchase_info_record_code], N''),
                          N'：供应商：', ISNULL(f.[supplier_code], N''),
                          N'：有效起始日：', CONVERT(VARCHAR(10), f.[valid_from_d], 23),
                          N'：价格：', CONVERT(VARCHAR(40), f.[net_purchase_price]))),
                      N'$.purchase_info_record_code', f.[purchase_info_record_code]),
                    N'$.valid_from', CONVERT(VARCHAR(10), f.[valid_from_d], 23)),
                  N'$.price', f.[net_purchase_price]),
                N'$.purchase_organization',
                CASE
                  WHEN LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N''
                    AND LTRIM(RTRIM(ISNULL(f.[purchase_organization], N''))) <> N''
                  THEN f.[purchase_organization] END),
              N'$.purchase_group',
              CASE
                WHEN LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N''
                  AND LTRIM(RTRIM(ISNULL(f.[purchase_group], N''))) <> N''
                THEN f.[purchase_group] END),
            N'$.supplier_code',
            CASE
              WHEN LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N''
                AND LTRIM(RTRIM(ISNULL(f.[supplier_code], N''))) <> N''
              THEN f.[supplier_code] END),
          N'$.net_purchase_price',
          CASE
            WHEN t.[net_purchase_price] = 0 AND f.[net_purchase_price] <> 0
            THEN f.[net_purchase_price] END),
        N'$.purchase_price_unit',
        CASE
          WHEN t.[purchase_price_unit] <= 1 AND f.[purchase_price_unit] > 1
          THEN f.[purchase_price_unit] END)
      AS [frag_pre]
  ) AS e2a
  CROSS APPLY (
    SELECT
      JSON_MODIFY(
        e2a.[frag_pre],
        N'$.purchase_currency_code',
        CASE
          WHEN LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N''
            AND LTRIM(RTRIM(ISNULL(f.[purchase_currency_code], N''))) <> N''
          THEN f.[purchase_currency_code] END)
      AS [frag]
  ) AS e2
  CROSS APPLY (
    SELECT JSON_MODIFY(e1.[with_bk], N'lax $._bk.bc', JSON_QUERY(e2.[frag])) AS [new_ext]
  ) AS x
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND (
      (LTRIM(RTRIM(ISNULL(t.[purchase_organization], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[purchase_organization], N''))) <> N'')
      OR (LTRIM(RTRIM(ISNULL(t.[purchase_group], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[purchase_group], N''))) <> N'')
      OR (LTRIM(RTRIM(ISNULL(t.[supplier_code], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[supplier_code], N''))) <> N'')
      OR (t.[net_purchase_price] = 0 AND f.[net_purchase_price] <> 0)
      OR (t.[purchase_price_unit] <= 1 AND f.[purchase_price_unit] > 1)
      OR (LTRIM(RTRIM(ISNULL(t.[purchase_currency_code], N''))) = N'' AND LTRIM(RTRIM(ISNULL(f.[purchase_currency_code], N''))) <> N'')
    );

  SET @chunk_updated = @@ROWCOUNT;
  SET @updated = @updated + @chunk_updated;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @processed - @chunk_n + 1 AS [from_rn],
    @processed AS [to_rn],
    @processed AS [max_rn],
    @chunk_updated AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@processed - @chunk_n + 1) AS NVARCHAR(20)), N'|',
    CAST((@processed) AS NVARCHAR(20)), N'|',
    CAST((@processed) AS NVARCHAR(20)), N'|',
    CAST((@chunk_updated) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
END;

DROP TABLE #hdr;
DROP TABLE #pp_item;
DROP TABLE #qty_scale;
DROP TABLE #val_scale;
DROP TABLE #done;
IF OBJECT_ID('tempdb..#chunk') IS NOT NULL DROP TABLE #chunk;
IF OBJECT_ID('tempdb..#hdr_pick') IS NOT NULL DROP TABLE #hdr_pick;
IF OBJECT_ID('tempdb..#item_pick') IS NOT NULL DROP TABLE #item_pick;
IF OBJECT_ID('tempdb..#fill') IS NOT NULL DROP TABLE #fill;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'bc_bk' AS NVARCHAR(40)) AS [scope],
  @processed AS [source_count],
  @updated AS [update_count],
  0 AS [insert_count],
  0 AS [delete_count],
  0 AS [target_before],
  0 AS [target_after],
  0 AS [target_physical],
  0 AS [soft_deleted],
  CASE WHEN @processed > @updated THEN @processed - @updated ELSE 0 END AS [unchanged_count];
