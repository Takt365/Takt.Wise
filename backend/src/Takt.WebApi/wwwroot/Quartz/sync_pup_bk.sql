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
DECLARE @desc_updated INT = 0;

-- =============================================================================
-- QT_SYNC_PUP_BK：采购条件主表物料描述回填（❌ 仅空字段，禁止全局覆盖）
-- 顺序：先回填子表外键，再回填主表空物料描述
--   1) purchase_condition_item.purchase_condition_id
--   2) purchase_condition_scale_quantity / scale_value.purchase_condition_item_id
--   3) purchase_condition.material_description（Length=40；仅空）
-- 来源只读：takt_logistics_materials_material_description（culture_code 固定 ja-JP）
-- ❌ 不改写：material_code、price_type、供应商及其它业务列（仅顺带写 updated_by/updated_at）
-- 规则：仅当目标 material_description 为空时写入；已有描述一律保留
-- 本次写入合并到 ext_field：$._bk.pup = { at, material_description }
-- 物料码匹配：trim + 18 位纯数字截末 10
-- 建议：在 QT_SYNC_PUP / 物料描述就绪之后执行
-- =============================================================================

-- ---------- ① 子表外键（业务键对齐后再写描述） ----------
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
  UPDATE TOP (@apply_chunk) I
  SET I.[purchase_condition_id] = H.[id], I.[updated_by] = @sync_user_id, I.[updated_at] = @now
  FROM [dbo].[takt_logistics_procurement_purchase_condition_item] AS I
  INNER JOIN [dbo].[takt_logistics_procurement_purchase_condition] AS H
    ON H.[tenant_code] = I.[tenant_code]
   AND H.[company_code] = I.[company_code]
   AND LTRIM(RTRIM(H.[plant_code])) = LTRIM(RTRIM(I.[plant_code]))
   AND LTRIM(RTRIM(H.[purchase_condition_code])) = LTRIM(RTRIM(I.[purchase_condition_code]))
   AND H.[is_deleted] = 0
  WHERE I.[tenant_code] = @tenant_code
    AND I.[company_code] = @company_code
    AND I.[is_deleted] = 0
    AND (I.[purchase_condition_id] IS NULL OR I.[purchase_condition_id] <> H.[id]);
  SET @dml_n = @@ROWCOUNT;
END
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
  UPDATE TOP (@apply_chunk) Q
  SET Q.[purchase_condition_item_id] = I.[id], Q.[updated_by] = @sync_user_id, Q.[updated_at] = @now
  FROM [dbo].[takt_logistics_procurement_purchase_condition_scale_quantity] AS Q
  INNER JOIN [dbo].[takt_logistics_procurement_purchase_condition_item] AS I
    ON I.[tenant_code] = Q.[tenant_code]
   AND I.[company_code] = Q.[company_code]
   AND LTRIM(RTRIM(I.[plant_code])) = LTRIM(RTRIM(Q.[plant_code]))
   AND LTRIM(RTRIM(I.[purchase_condition_code])) = LTRIM(RTRIM(Q.[purchase_condition_code]))
   AND I.[purchase_condition_seq] = Q.[purchase_condition_seq]
   AND I.[is_deleted] = 0
  WHERE Q.[tenant_code] = @tenant_code
    AND Q.[company_code] = @company_code
    AND Q.[is_deleted] = 0
    AND (Q.[purchase_condition_item_id] IS NULL OR Q.[purchase_condition_item_id] <> I.[id]);
  SET @dml_n = @@ROWCOUNT;
END
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
  UPDATE TOP (@apply_chunk) V
  SET V.[purchase_condition_item_id] = I.[id], V.[updated_by] = @sync_user_id, V.[updated_at] = @now
  FROM [dbo].[takt_logistics_procurement_purchase_condition_scale_value] AS V
  INNER JOIN [dbo].[takt_logistics_procurement_purchase_condition_item] AS I
    ON I.[tenant_code] = V.[tenant_code]
   AND I.[company_code] = V.[company_code]
   AND LTRIM(RTRIM(I.[plant_code])) = LTRIM(RTRIM(V.[plant_code]))
   AND LTRIM(RTRIM(I.[purchase_condition_code])) = LTRIM(RTRIM(V.[purchase_condition_code]))
   AND I.[purchase_condition_seq] = V.[purchase_condition_seq]
   AND I.[is_deleted] = 0
  WHERE V.[tenant_code] = @tenant_code
    AND V.[company_code] = @company_code
    AND V.[is_deleted] = 0
    AND (V.[purchase_condition_item_id] IS NULL OR V.[purchase_condition_item_id] <> I.[id]);
  SET @dml_n = @@ROWCOUNT;
END

IF OBJECT_ID('tempdb..#mat_key') IS NOT NULL DROP TABLE #mat_key;
IF OBJECT_ID('tempdb..#mat_desc_ja') IS NOT NULL DROP TABLE #mat_desc_ja;

-- 仅收集「描述为空」的采购条件物料键（非全表）
;WITH keys AS (
  SELECT DISTINCT
    CASE
      WHEN LEN(LTRIM(RTRIM(t.[material_code]))) = 18
        AND LTRIM(RTRIM(t.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(t.[material_code])), 10)
      ELSE LTRIM(RTRIM(t.[material_code]))
    END AS [mat_key]
  FROM [dbo].[takt_logistics_procurement_purchase_condition] AS t
  WHERE t.[tenant_code] = @tenant_code
    AND t.[company_code] = @company_code
    AND t.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(t.[material_code], N''))) <> N''
    AND (t.[material_description] IS NULL OR LTRIM(RTRIM(t.[material_description])) = N'')
)
SELECT [mat_key]
INTO #mat_key
FROM keys
WHERE [mat_key] IS NOT NULL AND [mat_key] <> N'';

;WITH src AS (
  SELECT
    CASE
      WHEN LEN(LTRIM(RTRIM(d.[material_code]))) = 18
        AND LTRIM(RTRIM(d.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(d.[material_code])), 10)
      ELSE LTRIM(RTRIM(d.[material_code]))
    END AS [mat_key],
    LEFT(LTRIM(RTRIM(d.[material_description])), 40) AS [material_description],
    ROW_NUMBER() OVER (
      PARTITION BY
        CASE
          WHEN LEN(LTRIM(RTRIM(d.[material_code]))) = 18
            AND LTRIM(RTRIM(d.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(d.[material_code])), 10)
          ELSE LTRIM(RTRIM(d.[material_code]))
        END
      ORDER BY d.[id]
    ) AS rn
  FROM [dbo].[takt_logistics_materials_material_description] AS d
  WHERE d.[tenant_code] = @tenant_code
    AND d.[is_deleted] = 0
    AND d.[culture_code] = N'ja-JP'
    AND LTRIM(RTRIM(ISNULL(d.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(d.[material_description], N''))) <> N''
)
SELECT [mat_key], [material_description]
INTO #mat_desc_ja
FROM src
WHERE rn = 1
  AND EXISTS (SELECT 1 FROM #mat_key k WHERE k.[mat_key] = src.[mat_key]);

-- ---------- 仅 UPDATE 主表空 material_description（无其它业务列） ----------
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) t
SET
  t.[material_description] = m.[material_description],
  t.[ext_field] = LEFT(x.[new_ext], 4000),
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_procurement_purchase_condition] AS t
INNER JOIN #mat_desc_ja AS m
  ON m.[mat_key] = CASE
    WHEN LEN(LTRIM(RTRIM(t.[material_code]))) = 18
      AND LTRIM(RTRIM(t.[material_code])) NOT LIKE '%[^0-9]%'
    THEN RIGHT(LTRIM(RTRIM(t.[material_code])), 10)
    ELSE LTRIM(RTRIM(t.[material_code]))
  END
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
    JSON_MODIFY(
      e1.[with_bk],
      N'lax $._bk.pup',
      JSON_QUERY(
        JSON_MODIFY(
          JSON_MODIFY(N'{}', N'$.at', CONVERT(VARCHAR(19), @now, 126)),
          N'$.material_description',
          m.[material_description])))
    AS [new_ext]
) AS x
WHERE t.[tenant_code] = @tenant_code
  AND t.[company_code] = @company_code
  AND t.[is_deleted] = 0
  AND LTRIM(RTRIM(ISNULL(t.[material_code], N''))) <> N''
  -- 仅空：禁止覆盖已有 material_description
  AND (t.[material_description] IS NULL OR LTRIM(RTRIM(t.[material_description])) = N'');
  SET @dml_n = @@ROWCOUNT;
  SET @desc_updated = @desc_updated + @dml_n;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    CASE WHEN @desc_updated - @dml_n + 1 < 1 THEN 0 ELSE @desc_updated - @dml_n + 1 END AS [from_rn],
    @desc_updated AS [to_rn],
    @desc_updated AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((CASE WHEN @desc_updated - @dml_n + 1 < 1 THEN 0 ELSE @desc_updated - @dml_n + 1 END) AS NVARCHAR(20)), N'|',
    CAST((@desc_updated) AS NVARCHAR(20)), N'|',
    CAST((@desc_updated) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
END

DROP TABLE #mat_key;
DROP TABLE #mat_desc_ja;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'pup_bk' AS NVARCHAR(40)) AS [scope],
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @desc_updated AS [material_description_updated],
  @desc_updated AS [total_updated_rows];
