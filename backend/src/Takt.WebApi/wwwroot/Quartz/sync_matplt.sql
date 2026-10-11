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
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @plant_code NVARCHAR(4) = N'{{PlantCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @apply_chunk INT = 20000;
DECLARE @merge_from_rn INT;
DECLARE @merge_to_rn INT;
DECLARE @merge_max_rn INT;
DECLARE @dml_n INT;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;
-- 更新履历：ext_field._sync.matplt[] 追加 { at, 变更列:{o,n} }；仅差异；超 nvarchar(4000) 保留原 JSON

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [material_code] NVARCHAR(100),
  [material_description] NVARCHAR(40),
  [material_specification] NVARCHAR(70),
  [industry_sector] NVARCHAR(100),
  [material_hierarchy] NVARCHAR(100),
  [material_group] NVARCHAR(100),
  [material_type] NVARCHAR(100),
  [base_unit] NVARCHAR(50),
  [purchase_group] NVARCHAR(100),
  [purchase_type] NVARCHAR(100),
  [special_procurement] NVARCHAR(100),
  [is_bulk] INT,
  [min_order_quantity] DECIMAL(18,2),
  [rounding_value] DECIMAL(18,2),
  [planned_delivery_time_days] DECIMAL(18,2),
  [in_house_production_days] DECIMAL(18,2),
  [manufacturer] NVARCHAR(200),
  [manufacturer_material_code] NVARCHAR(200),
  [currency] NVARCHAR(20),
  [price_control] NVARCHAR(100),
  [price_unit] NVARCHAR(100),
  [valuation] NVARCHAR(100),
  [moving_price] DECIMAL(18,2),
  [difference_code] NVARCHAR(100),
  [profit_center] NVARCHAR(100),
  [current_stock] DECIMAL(18,2),
  [production_location] NVARCHAR(100),
  [purchasing_location] NVARCHAR(100),
  [storage_location] NVARCHAR(100),
  [is_inspection] INT,
  [is_batch] INT,
  [discontinued_status] NVARCHAR(20),
  [material_status] NVARCHAR(100),
  [tenant_code] NVARCHAR(50),
  [company_code] NVARCHAR(50),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [updated_by] BIGINT
);

-- 源表原样全量装入（行数 = PP_SapMaterial；禁止擅自去重改行数）
-- 目标：takt_logistics_materials_material_plant（公司级；唯一键 Tenant+Company+Plant+Material）
-- 前置：建议先执行 sync_mat.sql（全局物料 + 描述）
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  LTRIM(RTRIM(S.[plant_code])),
  LTRIM(RTRIM(S.[material_code])),
  LEFT(ISNULL(NULLIF(LTRIM(RTRIM(S.[material_description])), ''), ''), 40),
  CAST(N'' AS NVARCHAR(70)),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[industry_sector])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[material_hierarchy])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[material_group])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[material_type])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[base_unit])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[purchase_group])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[purchase_type])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[special_procurement])), ''), ''),
  CASE WHEN LTRIM(RTRIM(S.[is_bulk])) = 'X' THEN 1 ELSE 0 END,
  COALESCE(TRY_CAST(S.[min_order_quantity] AS DECIMAL(18,2)), 0),
  COALESCE(TRY_CAST(S.[rounding_value] AS DECIMAL(18,2)), 0),
  COALESCE(TRY_CAST(S.[planned_delivery_time_days] AS DECIMAL(18,2)), 0),
  COALESCE(TRY_CAST(S.[in_house_production_days] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[manufacturer])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[manufacturer_material_code])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[currency])), ''), 'CNY'),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[price_control])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[price_unit])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[valuation])), ''), ''),
  COALESCE(TRY_CAST(S.[moving_price] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[difference_code])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(LEFT(S.[profit_center], 4))), ''), ''),
  COALESCE(TRY_CAST(S.[current_stock] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[production_location])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[purchasing_location])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[storage_location])), ''), ''),
  CASE WHEN LTRIM(RTRIM(S.[is_inspection])) = 'X' THEN 1 ELSE 0 END,
  CASE WHEN LTRIM(RTRIM(S.[is_batch])) = 'X' THEN 1 ELSE 0 END,
  COALESCE(NULLIF(LTRIM(RTRIM(S.[discontinued_status])), ''), 'Z0'),
  '1',
  @tenant_code,
  @company_code,
  '{}',
  '',
  @sync_user_id
FROM (
  SELECT
    [D_SAP_ZCA1D_Z001] AS plant_code,
    [D_SAP_ZCA1D_Z002] AS material_code,
    [D_SAP_ZCA1D_Z003] AS industry_sector,
    [D_SAP_ZCA1D_Z004] AS material_type,
    [D_SAP_ZCA1D_Z005] AS material_description,
    [D_SAP_ZCA1D_Z006] AS base_unit,
    [D_SAP_ZCA1D_Z007] AS material_hierarchy,
    [D_SAP_ZCA1D_Z008] AS material_group,
    [D_SAP_ZCA1D_Z009] AS purchase_group,
    [D_SAP_ZCA1D_Z010] AS purchase_type,
    [D_SAP_ZCA1D_Z011] AS special_procurement,
    [D_SAP_ZCA1D_Z012] AS is_bulk,
    [D_SAP_ZCA1D_Z013] AS min_order_quantity,
    [D_SAP_ZCA1D_Z015] AS rounding_value,
    [D_SAP_ZCA1D_Z017] AS planned_delivery_time_days,
    [D_SAP_ZCA1D_Z018] AS in_house_production_days,
    [D_SAP_ZCA1D_Z019] AS is_inspection,
    [D_SAP_ZCA1D_Z020] AS profit_center,
    [D_SAP_ZCA1D_Z021] AS difference_code,
    [D_SAP_ZCA1D_Z022] AS is_batch,
    [D_SAP_ZCA1D_Z023] AS manufacturer,
    [D_SAP_ZCA1D_Z024] AS manufacturer_material_code,
    [D_SAP_ZCA1D_Z025] AS valuation,
    [D_SAP_ZCA1D_Z026] AS moving_price,
    [D_SAP_ZCA1D_Z027] AS currency,
    [D_SAP_ZCA1D_Z028] AS price_control,
    [D_SAP_ZCA1D_Z029] AS price_unit,
    [D_SAP_ZCA1D_Z030] AS production_location,
    [D_SAP_ZCA1D_Z031] AS purchasing_location,
    [D_SAP_ZCA1D_Z032] AS storage_location,
    [D_SAP_ZCA1D_Z033] AS current_stock,
    [D_SAP_ZCA1D_Z034] AS discontinued_status,
    ROW_NUMBER() OVER (ORDER BY [D_SAP_ZCA1D_Z001], [D_SAP_ZCA1D_Z002]) AS rn
  FROM [Sap_Data].[dbo].[PP_SapMaterial]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @source_count AS [to_rn],
  @source_count AS [max_rn],
  @source_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@source_count) AS NVARCHAR(20)), N'|',
  CAST((@source_count) AS NVARCHAR(20)), N'|',
  CAST((@source_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapMaterial]);

-- 全量同步：临时表行数必须等于源表行数
IF @source_count <> @sap_raw_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'源行数与装入不一致: source=', @sap_raw_count, N', loaded=', @source_count);
  THROW 50003, @src_msg, 1;
END;

-- 唯一键 = Plant+Material；源内重复则无法 1:1 落目标表（与唯一索引 Tenant+Company+Plant+Material 一致）
IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [material_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT([plant_code], N' / ', [material_code], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [material_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(100),
  material_code NVARCHAR(100),
  tenant_code NVARCHAR(50),
  company_code NVARCHAR(50),
  change_by BIGINT,
  material_description_old NVARCHAR(40),
  material_description_new NVARCHAR(40),
  material_type_old NVARCHAR(100),
  material_type_new NVARCHAR(100),
  base_unit_old NVARCHAR(50),
  base_unit_new NVARCHAR(50),
  material_group_old NVARCHAR(100),
  material_group_new NVARCHAR(100),
  current_stock_old DECIMAL(18,2),
  current_stock_new DECIMAL(18,2),
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_plant]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

-- 存在则仅在业务字段变化或需恢复软删时 UPDATE；无变化不写入 #delta
SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_materials_material_plant] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_description], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_specification], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_specification], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[industry_sector], N''))) <> LTRIM(RTRIM(ISNULL(S.[industry_sector], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_hierarchy], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_hierarchy], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_group], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_type], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[base_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[base_unit], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[purchase_group], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[purchase_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_type], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[special_procurement], N''))) <> LTRIM(RTRIM(ISNULL(S.[special_procurement], N'')))
  OR T.[is_bulk] <> S.[is_bulk]
  OR ROUND(T.[min_order_quantity], 2) <> ROUND(S.[min_order_quantity], 2)
  OR ROUND(T.[rounding_value], 2) <> ROUND(S.[rounding_value], 2)
  OR ROUND(T.[planned_delivery_time_days], 2) <> ROUND(S.[planned_delivery_time_days], 2)
  OR ROUND(T.[in_house_production_days], 2) <> ROUND(S.[in_house_production_days], 2)
  OR LTRIM(RTRIM(ISNULL(T.[manufacturer], N''))) <> LTRIM(RTRIM(ISNULL(S.[manufacturer], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[manufacturer_material_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[manufacturer_material_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[price_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[price_control], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[price_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[price_unit], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[valuation], N''))) <> LTRIM(RTRIM(ISNULL(S.[valuation], N'')))
  OR ROUND(T.[moving_price], 2) <> ROUND(S.[moving_price], 2)
  OR LTRIM(RTRIM(ISNULL(T.[difference_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[difference_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[profit_center], N''))) <> LTRIM(RTRIM(ISNULL(S.[profit_center], N'')))
  OR ROUND(T.[current_stock], 2) <> ROUND(S.[current_stock], 2)
  OR LTRIM(RTRIM(ISNULL(T.[production_location], N''))) <> LTRIM(RTRIM(ISNULL(S.[production_location], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[purchasing_location], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchasing_location], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[storage_location], N''))) <> LTRIM(RTRIM(ISNULL(S.[storage_location], N'')))
  OR T.[requires_inspection] <> S.[is_inspection]
  OR T.[is_batch] <> S.[is_batch]
  OR LTRIM(RTRIM(ISNULL(T.[discontinued_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[discontinued_status], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_status], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[remark], N''))) <> LTRIM(RTRIM(ISNULL(S.[remark], N'')))
) THEN
  UPDATE SET
  T.[material_description]=S.[material_description],
  T.[material_specification]=S.[material_specification],
  T.[industry_sector]=S.[industry_sector],
  T.[material_hierarchy]=S.[material_hierarchy],
  T.[material_group]=S.[material_group],
  T.[material_type]=S.[material_type],
  T.[base_unit]=S.[base_unit],
  T.[purchase_group]=S.[purchase_group],
  T.[purchase_type]=S.[purchase_type],
  T.[special_procurement]=S.[special_procurement],
  T.[is_bulk]=S.[is_bulk],
  T.[min_order_quantity]=S.[min_order_quantity],
  T.[rounding_value]=S.[rounding_value],
  T.[planned_delivery_time_days]=S.[planned_delivery_time_days],
  T.[in_house_production_days]=S.[in_house_production_days],
  T.[manufacturer]=S.[manufacturer],
  T.[manufacturer_material_code]=S.[manufacturer_material_code],
  T.[currency_code]=S.[currency],
  T.[price_control]=S.[price_control],
  T.[price_unit]=S.[price_unit],
  T.[valuation]=S.[valuation],
  T.[moving_price]=S.[moving_price],
  T.[difference_code]=S.[difference_code],
  T.[profit_center]=S.[profit_center],
  T.[current_stock]=S.[current_stock],
  T.[production_location]=S.[production_location],
  T.[purchasing_location]=S.[purchasing_location],
  T.[storage_location]=S.[storage_location],
  T.[requires_inspection]=S.[is_inspection],
  T.[is_batch]=S.[is_batch],
  T.[discontinued_status]=S.[discontinued_status],
  T.[material_status]=S.[material_status],
  T.[remark]=S.[remark],
  T.[culture_code]=@culture_code,
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=@now,
  T.[is_deleted]=0,
  T.[ext_field]=(
    SELECT CASE WHEN LEN(x.[new_ext]) <= 4000 THEN x.[new_ext] ELSE e0.[base_ext] END
    FROM (SELECT CASE WHEN ISJSON(NULLIF(LTRIM(RTRIM(ISNULL(T.[ext_field], N''))), N'')) = 1 THEN LTRIM(RTRIM(T.[ext_field])) ELSE N'{}' END AS [base_ext]) e0
    CROSS APPLY (SELECT CASE WHEN JSON_QUERY(e0.[base_ext], N'$._sync') IS NULL THEN JSON_MODIFY(e0.[base_ext], N'lax $._sync', JSON_QUERY(N'{}')) ELSE e0.[base_ext] END AS [with_root]) e1
    CROSS APPLY (SELECT CASE
      WHEN JSON_QUERY(e1.[with_root], N'$._sync.matplt') IS NULL THEN JSON_MODIFY(e1.[with_root], N'lax $._sync.matplt', JSON_QUERY(N'[]'))
      WHEN LEFT(LTRIM(ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.matplt'), N'')), 1) = N'[' THEN e1.[with_root]
      ELSE JSON_MODIFY(e1.[with_root], N'lax $._sync.matplt', JSON_QUERY(N'[' + ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.matplt'), N'{}') + N']'))
    END AS [with_arr]) e2
    CROSS APPLY (SELECT JSON_MODIFY(e2.[with_arr], N'append $._sync.matplt', JSON_QUERY(CONCAT(
      N'{"at":"', CONVERT(VARCHAR(19), @now, 126), N'"',
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_description], N''))) THEN CONCAT(N',"material_description":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[material_description], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[material_description], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[material_specification], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_specification], N''))) THEN CONCAT(N',"material_specification":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[material_specification], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[material_specification], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[industry_sector], N''))) <> LTRIM(RTRIM(ISNULL(S.[industry_sector], N''))) THEN CONCAT(N',"industry_sector":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[industry_sector], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[industry_sector], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[material_hierarchy], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_hierarchy], N''))) THEN CONCAT(N',"material_hierarchy":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[material_hierarchy], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[material_hierarchy], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[material_group], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_group], N''))) THEN CONCAT(N',"material_group":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[material_group], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[material_group], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[material_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_type], N''))) THEN CONCAT(N',"material_type":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[material_type], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[material_type], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[base_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[base_unit], N''))) THEN CONCAT(N',"base_unit":{"o":"', REPLACE(ISNULL(T.[base_unit], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[base_unit], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[purchase_group], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_group], N''))) THEN CONCAT(N',"purchase_group":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[purchase_group], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[purchase_group], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[purchase_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchase_type], N''))) THEN CONCAT(N',"purchase_type":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[purchase_type], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[purchase_type], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[special_procurement], N''))) <> LTRIM(RTRIM(ISNULL(S.[special_procurement], N''))) THEN CONCAT(N',"special_procurement":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[special_procurement], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[special_procurement], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN T.[is_bulk] <> S.[is_bulk] THEN CONCAT(N',"is_bulk":{"o":', CONVERT(VARCHAR(20), T.[is_bulk]), N',"n":', CONVERT(VARCHAR(20), S.[is_bulk]), N'}') ELSE N'' END,
      CASE WHEN ROUND(T.[min_order_quantity], 2) <> ROUND(S.[min_order_quantity], 2) THEN CONCAT(N',"min_order_quantity":{"o":', CONVERT(VARCHAR(40), ROUND(T.[min_order_quantity], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[min_order_quantity], 2)), N'}') ELSE N'' END,
      CASE WHEN ROUND(T.[rounding_value], 2) <> ROUND(S.[rounding_value], 2) THEN CONCAT(N',"rounding_value":{"o":', CONVERT(VARCHAR(40), ROUND(T.[rounding_value], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[rounding_value], 2)), N'}') ELSE N'' END,
      CASE WHEN ROUND(T.[planned_delivery_time_days], 2) <> ROUND(S.[planned_delivery_time_days], 2) THEN CONCAT(N',"planned_delivery_time_days":{"o":', CONVERT(VARCHAR(40), ROUND(T.[planned_delivery_time_days], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[planned_delivery_time_days], 2)), N'}') ELSE N'' END,
      CASE WHEN ROUND(T.[in_house_production_days], 2) <> ROUND(S.[in_house_production_days], 2) THEN CONCAT(N',"in_house_production_days":{"o":', CONVERT(VARCHAR(40), ROUND(T.[in_house_production_days], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[in_house_production_days], 2)), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[manufacturer], N''))) <> LTRIM(RTRIM(ISNULL(S.[manufacturer], N''))) THEN CONCAT(N',"manufacturer":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[manufacturer], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[manufacturer], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[manufacturer_material_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[manufacturer_material_code], N''))) THEN CONCAT(N',"manufacturer_material_code":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[manufacturer_material_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[manufacturer_material_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency], N''))) THEN CONCAT(N',"currency_code":{"o":"', REPLACE(ISNULL(T.[currency_code], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[currency], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[price_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[price_control], N''))) THEN CONCAT(N',"price_control":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[price_control], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[price_control], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[price_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[price_unit], N''))) THEN CONCAT(N',"price_unit":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[price_unit], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[price_unit], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[valuation], N''))) <> LTRIM(RTRIM(ISNULL(S.[valuation], N''))) THEN CONCAT(N',"valuation":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[valuation], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[valuation], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ROUND(T.[moving_price], 2) <> ROUND(S.[moving_price], 2) THEN CONCAT(N',"moving_price":{"o":', CONVERT(VARCHAR(40), ROUND(T.[moving_price], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[moving_price], 2)), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[difference_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[difference_code], N''))) THEN CONCAT(N',"difference_code":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[difference_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[difference_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[profit_center], N''))) <> LTRIM(RTRIM(ISNULL(S.[profit_center], N''))) THEN CONCAT(N',"profit_center":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[profit_center], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[profit_center], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ROUND(T.[current_stock], 2) <> ROUND(S.[current_stock], 2) THEN CONCAT(N',"current_stock":{"o":', CONVERT(VARCHAR(40), ROUND(T.[current_stock], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[current_stock], 2)), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[production_location], N''))) <> LTRIM(RTRIM(ISNULL(S.[production_location], N''))) THEN CONCAT(N',"production_location":{"o":"', REPLACE(ISNULL(T.[production_location], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[production_location], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[purchasing_location], N''))) <> LTRIM(RTRIM(ISNULL(S.[purchasing_location], N''))) THEN CONCAT(N',"purchasing_location":{"o":"', REPLACE(ISNULL(T.[purchasing_location], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[purchasing_location], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[storage_location], N''))) <> LTRIM(RTRIM(ISNULL(S.[storage_location], N''))) THEN CONCAT(N',"storage_location":{"o":"', REPLACE(ISNULL(T.[storage_location], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[storage_location], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN T.[requires_inspection] <> S.[is_inspection] THEN CONCAT(N',"requires_inspection":{"o":', CONVERT(VARCHAR(20), T.[requires_inspection]), N',"n":', CONVERT(VARCHAR(20), S.[is_inspection]), N'}') ELSE N'' END,
      CASE WHEN T.[is_batch] <> S.[is_batch] THEN CONCAT(N',"is_batch":{"o":', CONVERT(VARCHAR(20), T.[is_batch]), N',"n":', CONVERT(VARCHAR(20), S.[is_batch]), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[discontinued_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[discontinued_status], N''))) THEN CONCAT(N',"discontinued_status":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[discontinued_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[discontinued_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[material_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_status], N''))) THEN CONCAT(N',"material_status":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[material_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[material_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[remark], N''))) <> LTRIM(RTRIM(ISNULL(S.[remark], N''))) THEN CONCAT(N',"remark":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[remark], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[remark], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[is_deleted], 0) <> 0 THEN N',"is_deleted":{"o":1,"n":0}' ELSE N'' END,
      N'}'))) AS [new_ext]) x
  )
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[material_code],[material_description],[material_specification],
    [industry_sector],[material_hierarchy],[material_group],[material_type],[base_unit],
    [purchase_group],[purchase_type],[special_procurement],[is_bulk],[min_order_quantity],
    [rounding_value],[planned_delivery_time_days],[in_house_production_days],[manufacturer],
    [manufacturer_material_code],[currency_code],[price_control],[price_unit],[valuation],
    [moving_price],[difference_code],[profit_center],[current_stock],[production_location],
    [purchasing_location],[storage_location],[requires_inspection],[is_batch],[discontinued_status],
    [material_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],
    [created_at],[updated_by],[updated_at],[is_deleted]
  )
  VALUES (
    S.[id],S.[plant_code],S.[material_code],S.[material_description],S.[material_specification],
    S.[industry_sector],S.[material_hierarchy],S.[material_group],S.[material_type],S.[base_unit],
    S.[purchase_group],S.[purchase_type],S.[special_procurement],S.[is_bulk],S.[min_order_quantity],
    S.[rounding_value],S.[planned_delivery_time_days],S.[in_house_production_days],S.[manufacturer],
    S.[manufacturer_material_code],S.[currency],S.[price_control],S.[price_unit],S.[valuation],
    S.[moving_price],S.[difference_code],S.[profit_center],S.[current_stock],S.[production_location],
    S.[purchasing_location],S.[storage_location],S.[is_inspection],S.[is_batch],S.[discontinued_status],
    S.[material_status],S.[tenant_code],S.[company_code],@culture_code,S.[ext_field],S.[remark],S.[updated_by],@now,S.[updated_by],@now,0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[material_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[material_description], INSERTED.[material_description],
  DELETED.[material_type], INSERTED.[material_type],
  DELETED.[base_unit], INSERTED.[base_unit],
  DELETED.[material_group], INSERTED.[material_group],
  DELETED.[current_stock], INSERTED.[current_stock],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #delta(
  rn, oper_type, id, plant_code, material_code,
  tenant_code, company_code, change_by,
  material_description_old, material_description_new,
  material_type_old, material_type_new,
  base_unit_old, base_unit_new,
  material_group_old, material_group_new,
  current_stock_old, current_stock_new,
  ext_field_old, ext_field_new,
  remark_old, remark_new
);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END

-- 孤儿软删：目标有而源没有时才软删；存在更新/不存在插入已由 MERGE 完成
IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [material_code] NVARCHAR(100)
);


DECLARE @delete_count INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[material_code]
INTO #soft_deleted_rows ([id], [plant_code], [material_code])
FROM [takt_logistics_materials_material_plant] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[material_code] = LTRIM(RTRIM(T.[material_code]))
  );
  SET @dml_n = @@ROWCOUNT;
  SET @delete_count = @delete_count + @dml_n;
END
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'soft' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  @delete_count AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  @delete_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'soft', N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@delete_count) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@delete_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    ISNULL([plant_code], N''), N'/',
    ISNULL([material_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_plant]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_plant]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_plant]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 1
);

IF @target_count <> @source_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(
    N'有效行数不一致: source=', @source_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;


/* per-row delta_log removed */


DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"source":' + CAST(@source_count AS NVARCHAR)
  + N',"target_before":' + CAST(@target_before AS NVARCHAR)
  + N',"target_after":' + CAST(@target_count AS NVARCHAR)
  + N',"target_physical":' + CAST(@target_physical AS NVARCHAR)
  + N',"soft_deleted":' + CAST(@soft_deleted AS NVARCHAR)
  + N',"insert":' + CAST(@insert_count AS NVARCHAR)
  + N',"update":' + CAST(@update_count AS NVARCHAR)
  + N',"unchanged":' + CAST(@unchanged_count AS NVARCHAR)
  + N',"soft_delete_this_run":' + CAST(@delete_count AS NVARCHAR)
  + N',"soft_delete_keys":"' + REPLACE(@soft_deleted_keys, N'"', N'''') + N'"}';

-- 批量同步：每次执行仅写 1 条摘要差异日志（非逐行）；明细变更见业务操作差异日志
INSERT INTO [takt_statistics_logging_delta_log] (
  [id],[oper_type],[table_name],[primary_key_id],
  [before_data],[after_data],[diff_data],[sql_statement],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[tenant_code],[company_code],[plant_code],[culture_code],
  [ext_field],[remark],[created_by],[created_at]
)
VALUES (
  @base_id,
  N'SYNC',
  N'takt_logistics_materials_material_plant',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE MaterialPlant Sync (summary)',
  N'127.0.0.1',N'Server',N'SQLCMD',N'Server',N'Windows',N'Server',
  @now,0,
  @tenant_code,@company_code,@plant_code,@culture_code,
  N'{}',N'SYNC batch summary',@sync_user_id,@now
);





INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[plant_code],[culture_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'工厂物料管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/material-plant',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

-- Quartz 执行器读取此结果集写入 ExecuteMessage / quartz-.log
SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [source_raw_count],
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
