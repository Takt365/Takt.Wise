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
DECLARE @dml_n INT;
DECLARE @item_updated INT = 0;

-- =============================================================================
-- QT_SYNC_PPH_BK：按工厂物料回填空物料描述（❌ 仅空字段，禁止覆盖已有值）
-- 写入：
--   takt_logistics_procurement_purchase_order_item.material_description
-- 来源只读：takt_logistics_materials_material_plant（TaktMaterialPlant；按租户+公司+工厂+物料）
-- ❌ 不改写：material_code、数量金额及其它业务列（仅顺带写 updated_by/updated_at）
-- 规则：仅当目标 material_description 为空时写入；已有描述一律保留
-- 本次写入合并到 ext_field：$._bk.pph = { at, material_description }
-- 物料码匹配：trim + 18 位纯数字截末 10（对齐 TaktStringHelper.NormalizeSapNumericMaterialCode）
-- 建议：QT_SYNC_MATPLT / QT_SYNC_PO 之后
-- =============================================================================

IF OBJECT_ID('tempdb..#mat_plant_desc') IS NOT NULL DROP TABLE #mat_plant_desc;

;WITH src AS (
  SELECT
    LEFT(LTRIM(RTRIM(p.[plant_code])), 4) AS [plant_code],
    CASE
      WHEN LEN(LTRIM(RTRIM(p.[material_code]))) = 18
        AND LTRIM(RTRIM(p.[material_code])) NOT LIKE '%[^0-9]%'
      THEN RIGHT(LTRIM(RTRIM(p.[material_code])), 10)
      ELSE LTRIM(RTRIM(p.[material_code]))
    END AS [mat_key],
    LEFT(LTRIM(RTRIM(p.[material_description])), 40) AS [material_description],
    ROW_NUMBER() OVER (
      PARTITION BY
        LEFT(LTRIM(RTRIM(p.[plant_code])), 4),
        CASE
          WHEN LEN(LTRIM(RTRIM(p.[material_code]))) = 18
            AND LTRIM(RTRIM(p.[material_code])) NOT LIKE '%[^0-9]%'
          THEN RIGHT(LTRIM(RTRIM(p.[material_code])), 10)
          ELSE LTRIM(RTRIM(p.[material_code]))
        END
      ORDER BY p.[id]
    ) AS rn
  FROM [dbo].[takt_logistics_materials_material_plant] AS p
  WHERE p.[tenant_code] = @tenant_code
    AND p.[company_code] = @company_code
    AND p.[is_deleted] = 0
    AND LTRIM(RTRIM(ISNULL(p.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(p.[material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(p.[material_description], N''))) <> N''
)
SELECT [plant_code], [mat_key], [material_description]
INTO #mat_plant_desc
FROM src
WHERE rn = 1
  AND [mat_key] IS NOT NULL AND [mat_key] <> N'';

-- ---------- ① 采购订单明细：仅空 material_description ----------
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) t
SET
  t.[material_description] = m.[material_description],
  t.[ext_field] = LEFT(x.[new_ext], 4000),
  t.[updated_by] = @sync_user_id,
  t.[updated_at] = @now
FROM [dbo].[takt_logistics_procurement_purchase_order_item] AS t
INNER JOIN #mat_plant_desc AS m
  ON m.[plant_code] = LEFT(LTRIM(RTRIM(ISNULL(t.[plant_code], N''))), 4)
 AND m.[mat_key] = CASE
    WHEN LEN(LTRIM(RTRIM(t.[material_code]))) = 18
      AND LTRIM(RTRIM(t.[material_code])) NOT LIKE '%[^0-9]%'
    THEN RIGHT(LTRIM(RTRIM(t.[material_code])), 10)
    ELSE LTRIM(RTRIM(t.[material_code]))
  END
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
      e1.[with_bk],
      N'lax $._bk.pph',
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
  AND (t.[material_description] IS NULL OR LTRIM(RTRIM(t.[material_description])) = N'');
  SET @dml_n = @@ROWCOUNT;
  SET @item_updated = @item_updated + @dml_n;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    CASE WHEN @item_updated - @dml_n + 1 < 1 THEN 0 ELSE @item_updated - @dml_n + 1 END AS [from_rn],
    @item_updated AS [to_rn],
    @item_updated AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((CASE WHEN @item_updated - @dml_n + 1 < 1 THEN 0 ELSE @item_updated - @dml_n + 1 END) AS NVARCHAR(20)), N'|',
    CAST((@item_updated) AS NVARCHAR(20)), N'|',
    CAST((@item_updated) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
END

DROP TABLE #mat_plant_desc;

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'pph_bk' AS NVARCHAR(40)) AS [scope],
  @tenant_code AS [tenant_code],
  @company_code AS [company_code],
  @item_updated AS [purchase_order_item_updated],
  @item_updated AS [total_updated_rows];
