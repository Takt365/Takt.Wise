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
DECLARE @apply_chunk INT = 100000;
DECLARE @merge_from_rn INT;
DECLARE @merge_to_rn INT;
DECLARE @merge_max_rn INT;
DECLARE @dml_n INT;
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
-- QT_SYNC_BC_PCB_SECT_BK：BOM 明细 PCB SECT 整树回填 pcb_sect_indicator
-- 对齐 TaktBomMaterialCostItemLineCostHelper.CollectPcbSectHierarchyRows（集合版，禁止逐行 WHILE）
-- 树根：component_description 含「PCB SECT」，或已标 pcb_sect_indicator=X
-- 子孙：同工厂+规范化产品码+核算日树上，根节点 rn 起至下一同级/更浅节点之前
-- 核算月：ExecuteParams.costingPeriod（yyyy-MM）；空则当月
-- 仅目标库；须选业务库（如 zTakt_000_Dev），勿选暂存库 zTakt_900_Dev
-- 本次写入合并到 ext_field：$._bk.pcb_sect[] 追加 { at, pcb_sect_indicator, costing_period }（不覆盖历史）
-- =============================================================================

IF OBJECT_ID('tempdb..#bc_pcb_src') IS NOT NULL DROP TABLE #bc_pcb_src;
IF OBJECT_ID('tempdb..#bc_pcb_mark') IS NOT NULL DROP TABLE #bc_pcb_mark;

CREATE TABLE #bc_pcb_src (
  [rn] INT NOT NULL,
  [id] BIGINT NOT NULL,
  [plant_code] NVARCHAR(4) NOT NULL,
  [product_norm] NVARCHAR(20) NOT NULL,
  [cost_day] DATE NOT NULL,
  [bom_depth] INT NOT NULL,
  [is_pcb_node] BIT NOT NULL,
  [already_marked] BIT NOT NULL,
  CONSTRAINT [pk_bc_pcb_src] PRIMARY KEY CLUSTERED ([rn])
);

CREATE TABLE #bc_pcb_mark (
  [id] BIGINT NOT NULL PRIMARY KEY
);

INSERT INTO #bc_pcb_src (
  [rn], [id], [plant_code], [product_norm], [cost_day], [bom_depth], [is_pcb_node], [already_marked]
)
SELECT
  ROW_NUMBER() OVER (
    ORDER BY
      x.[plant_code],
      x.[product_norm],
      x.[cost_day],
      x.[product_code],
      x.[line_number],
      x.[bom_depth],
      x.[bom_level_num],
      x.[bom_level],
      x.[id]
  ) AS [rn],
  x.[id],
  x.[plant_code],
  x.[product_norm],
  x.[cost_day],
  x.[bom_depth],
  x.[is_pcb_node],
  x.[already_marked]
FROM (
  SELECT
    t.[id],
    LTRIM(RTRIM(ISNULL(t.[plant_code], N''))) AS [plant_code],
    CASE
      WHEN LEN(LTRIM(RTRIM(ISNULL(t.[product_code], N'')))) = 18
        AND LTRIM(RTRIM(t.[product_code])) NOT LIKE N'%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[product_code])), 10)
      ELSE LTRIM(RTRIM(ISNULL(t.[product_code], N'')))
    END AS [product_norm],
    CAST(t.[costing_date] AS DATE) AS [cost_day],
    LTRIM(RTRIM(ISNULL(t.[product_code], N''))) AS [product_code],
    t.[line_number],
    bl.[bom_level],
    bl.[bom_depth],
    bl.[bom_level_num],
    CASE
      WHEN CHARINDEX(N'PCB SECT', UPPER(LTRIM(RTRIM(ISNULL(t.[component_description], N''))))) > 0
        OR UPPER(LTRIM(RTRIM(ISNULL(t.[pcb_sect_indicator], N'')))) = N'X'
      THEN CAST(1 AS BIT)
      ELSE CAST(0 AS BIT)
    END AS [is_pcb_node],
    CASE
      WHEN UPPER(LTRIM(RTRIM(ISNULL(t.[pcb_sect_indicator], N'')))) = N'X'
      THEN CAST(1 AS BIT)
      ELSE CAST(0 AS BIT)
    END AS [already_marked]
  FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
  CROSS APPLY (
    SELECT LTRIM(RTRIM(ISNULL(t.[bom_level], N''))) AS [bom_level]
  ) AS b0
  CROSS APPLY (
    SELECT
      b0.[bom_level],
      CASE
        WHEN b0.[bom_level] = N'' THEN 2147483647
        WHEN PATINDEX(N'%[^.]%', b0.[bom_level]) = 0 THEN LEN(b0.[bom_level])
        ELSE PATINDEX(N'%[^.]%', b0.[bom_level]) - 1
      END AS [bom_depth],
      CASE
        WHEN b0.[bom_level] = N'' THEN 2147483647
        ELSE ISNULL(
          TRY_CAST(
            CASE
              WHEN PATINDEX(N'%[^.]%', b0.[bom_level]) = 0 THEN N''
              WHEN PATINDEX(
                N'%[^0-9]%',
                SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40)
              ) = 0
              THEN SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40)
              ELSE LEFT(
                SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40),
                PATINDEX(
                  N'%[^0-9]%',
                  SUBSTRING(b0.[bom_level], PATINDEX(N'%[^.]%', b0.[bom_level]), 40)
                ) - 1)
            END AS INT),
          2147483647)
      END AS [bom_level_num]
  ) AS bl
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND t.[costing_date] IS NOT NULL
    AND t.[costing_date] >= @period_start
    AND t.[costing_date] < @period_end_exclusive
    AND LTRIM(RTRIM(ISNULL(t.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(t.[product_code], N''))) <> N''
) AS x;

CREATE NONCLUSTERED INDEX [ix_bc_pcb_src_tree]
  ON #bc_pcb_src ([plant_code], [product_norm], [cost_day], [rn])
  INCLUDE ([bom_depth], [id], [is_pcb_node], [already_marked]);

DECLARE @scanned INT = (SELECT COUNT(*) FROM #bc_pcb_src);

;WITH [pcb_roots] AS (
  SELECT
    s.[rn],
    s.[plant_code],
    s.[product_norm],
    s.[cost_day],
    s.[bom_depth]
  FROM #bc_pcb_src AS s
  WHERE s.[is_pcb_node] = 1
),
[pcb_ranges] AS (
  SELECT
    r.[rn] AS [start_rn],
    r.[plant_code],
    r.[product_norm],
    r.[cost_day],
    ISNULL((
      SELECT MIN(n.[rn])
      FROM #bc_pcb_src AS n
      WHERE n.[plant_code] = r.[plant_code]
        AND n.[product_norm] = r.[product_norm]
        AND n.[cost_day] = r.[cost_day]
        AND n.[rn] > r.[rn]
        AND n.[bom_depth] <= r.[bom_depth]
    ), 2147483647) AS [end_rn_exclusive]
  FROM [pcb_roots] AS r
)
INSERT INTO #bc_pcb_mark ([id])
SELECT DISTINCT s.[id]
FROM #bc_pcb_src AS s
INNER JOIN [pcb_ranges] AS r
  ON s.[plant_code] = r.[plant_code]
 AND s.[product_norm] = r.[product_norm]
 AND s.[cost_day] = r.[cost_day]
 AND s.[rn] >= r.[start_rn]
 AND s.[rn] < r.[end_rn_exclusive];

DECLARE @pcb_sect_count INT = (SELECT COUNT(*) FROM #bc_pcb_mark);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @pcb_sect_count AS [to_rn],
  @pcb_sect_count AS [max_rn],
  @pcb_sect_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@pcb_sect_count) AS NVARCHAR(20)), N'|',
  CAST((@pcb_sect_count) AS NVARCHAR(20)), N'|',
  CAST((@pcb_sect_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @unchanged INT = (
  SELECT COUNT(*)
  FROM #bc_pcb_mark AS m
  INNER JOIN #bc_pcb_src AS s ON s.[id] = m.[id]
  WHERE s.[already_marked] = 1
);

DECLARE @updated INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) t
SET
  t.[pcb_sect_indicator] = N'X',
  t.[ext_field] = LEFT(x.[new_ext], 4000),
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_manufacturing_bom_material_cost_item] AS t
INNER JOIN #bc_pcb_mark AS m ON m.[id] = t.[id]
CROSS APPLY (
  SELECT
    CASE
      WHEN ISJSON(NULLIF(LTRIM(RTRIM(CAST(ISNULL(t.[ext_field], N'') AS nvarchar(max)))), N'')) = 1
      THEN LTRIM(RTRIM(CAST(t.[ext_field] AS nvarchar(max))))
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
    CASE
      WHEN JSON_QUERY(e1.[with_bk], N'$._bk.pcb_sect') IS NULL
      THEN JSON_MODIFY(e1.[with_bk], N'lax $._bk.pcb_sect', JSON_QUERY(N'[]'))
      WHEN LEFT(LTRIM(ISNULL(JSON_QUERY(e1.[with_bk], N'$._bk.pcb_sect'), N'')), 1) = N'['
      THEN e1.[with_bk]
      ELSE JSON_MODIFY(
        e1.[with_bk],
        N'lax $._bk.pcb_sect',
        JSON_QUERY(N'[' + ISNULL(JSON_QUERY(e1.[with_bk], N'$._bk.pcb_sect'), N'{}') + N']'))
    END AS [with_arr]
) AS e2
CROSS APPLY (
  SELECT
    JSON_MODIFY(
      e2.[with_arr],
      N'append $._bk.pcb_sect',
      JSON_QUERY(
        JSON_MODIFY(
          JSON_MODIFY(
            JSON_MODIFY(N'{}', N'$.at', CONVERT(VARCHAR(19), @now, 126)),
            N'$.pcb_sect_indicator',
            N'X'),
          N'$.costing_period',
          @costing_period)))
    AS [new_ext]
) AS x
WHERE UPPER(LTRIM(RTRIM(ISNULL(t.[pcb_sect_indicator], N'')))) <> N'X';
  SET @dml_n = @@ROWCOUNT;
  SET @updated = @updated + @dml_n;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    CASE WHEN @updated - @dml_n + 1 < 1 THEN 0 ELSE @updated - @dml_n + 1 END AS [from_rn],
    @updated AS [to_rn],
    @pcb_sect_count AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((CASE WHEN @updated - @dml_n + 1 < 1 THEN 0 ELSE @updated - @dml_n + 1 END) AS NVARCHAR(20)), N'|',
    CAST((@updated) AS NVARCHAR(20)), N'|',
    CAST((@pcb_sect_count) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
END

DROP TABLE #bc_pcb_src;
DROP TABLE #bc_pcb_mark;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'bc_pcb_sect_bk' AS NVARCHAR(40)) AS [scope],
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @scanned AS [source_count],
  @pcb_sect_count AS [target_after],
  @updated AS [update_count],
  @unchanged AS [unchanged_count],
  0 AS [insert_count],
  0 AS [delete_count],
  0 AS [target_before],
  0 AS [target_physical],
  0 AS [soft_deleted];
