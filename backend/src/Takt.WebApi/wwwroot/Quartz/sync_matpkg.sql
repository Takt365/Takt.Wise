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

-- 源表 / 目标表：同名 + 列与实体 TaktPackagingMaterial 一致
-- {{SourceDatabase}}.dbo.takt_logistics_materials_packaging_material → 当前租户库同名表
-- 业务唯一键：Plant+PackagingMaterialCode（与租户库唯一索引 Tenant+Company+Plant+PackagingMaterialCode 对齐）
-- tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [packaging_material_code] NVARCHAR(20),
  [material_code] NVARCHAR(20),
  [material_description] NVARCHAR(40),
  [hs_code] NVARCHAR(20),
  [hs_name] NVARCHAR(500),
  [additional_code] NVARCHAR(20),
  [origin_country_region_code] NVARCHAR(2),
  [origin_country_region_name] NVARCHAR(100),
  [destination_country_region_code] NVARCHAR(2),
  [destination_country_region_name] NVARCHAR(100),
  [regulatory_condition_code] NVARCHAR(40),
  [tariff_rate_type] NVARCHAR(40),
  [gross_weight] DECIMAL(18,10),
  [net_weight] DECIMAL(18,10),
  [weight_unit] NVARCHAR(10),
  [business_volume] DECIMAL(18,6),
  [volume_unit] NVARCHAR(10),
  [size_dimension] NVARCHAR(40),
  [packaging_type] NVARCHAR(40),
  [packing_unit] NVARCHAR(20),
  [quantity_per_packing] DECIMAL(18,2),
  [packaging_spec] NVARCHAR(200),
  [packaging_description] NVARCHAR(500),
  [sort_order] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

-- 源表装入：
-- 1) 物料码/包装码含全角括号（　）的不参与同步
-- 2) 18 位纯数字物料/包装码截末 10 位
-- 3) 按唯一键 Plant+PackagingMaterialCode 去重
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[packaging_material_code],
  S.[material_code],
  S.[material_description],
  S.[hs_code],
  S.[hs_name],
  S.[additional_code],
  S.[origin_country_region_code],
  S.[origin_country_region_name],
  S.[destination_country_region_code],
  S.[destination_country_region_name],
  S.[regulatory_condition_code],
  S.[tariff_rate_type],
  S.[gross_weight],
  S.[net_weight],
  S.[weight_unit],
  S.[business_volume],
  S.[volume_unit],
  S.[size_dimension],
  S.[packaging_type],
  S.[packing_unit],
  S.[quantity_per_packing],
  S.[packaging_spec],
  S.[packaging_description],
  S.[sort_order],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  '{}',
  '',
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY N.[plant_code], N.[packaging_material_code]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[packaging_material_code]))) = 18
          AND LTRIM(RTRIM(R.[packaging_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[packaging_material_code])), 10)
        ELSE LTRIM(RTRIM(R.[packaging_material_code]))
      END AS [packaging_material_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_description], N''))), 40), N''), N'') AS [material_description],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[hs_code], N''))), 20), N'') AS [hs_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[hs_name], N''))), 500), N'') AS [hs_name],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[additional_code], N''))), 20), N'') AS [additional_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[origin_country_region_code], N''))), 2), N'') AS [origin_country_region_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[origin_country_region_name], N''))), 100), N'') AS [origin_country_region_name],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[destination_country_region_code], N''))), 2), N'') AS [destination_country_region_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[destination_country_region_name], N''))), 100), N'') AS [destination_country_region_name],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[regulatory_condition_code], N''))), 40), N'') AS [regulatory_condition_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tariff_rate_type], N''))), 40), N'') AS [tariff_rate_type],
      TRY_CAST(R.[gross_weight] AS DECIMAL(18,10)) AS [gross_weight],
      TRY_CAST(R.[net_weight] AS DECIMAL(18,10)) AS [net_weight],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[weight_unit], N''))), 10), N''), N'') AS [weight_unit],
      TRY_CAST(R.[business_volume] AS DECIMAL(18,6)) AS [business_volume],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[volume_unit], N''))), 10), N''), N'') AS [volume_unit],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[size_dimension], N''))), 40), N'') AS [size_dimension],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[packaging_type], N''))), 40), N''), N'') AS [packaging_type],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[packing_unit], N''))), 20), N''), N'') AS [packing_unit],
      TRY_CAST(R.[quantity_per_packing] AS DECIMAL(18,2)) AS [quantity_per_packing],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[packaging_spec], N''))), 200), N'') AS [packaging_spec],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[packaging_description], N''))), 500), N'') AS [packaging_description],
      COALESCE(TRY_CAST(R.[sort_order] AS INT), 0) AS [sort_order],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
            ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[plant_code])),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[packaging_material_code]))) = 18
              AND LTRIM(RTRIM(R.[packaging_material_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[packaging_material_code])), 10)
            ELSE LTRIM(RTRIM(R.[packaging_material_code]))
          END
        ORDER BY
          LEN(ISNULL(LTRIM(RTRIM(R.[material_description])), N'')) DESC,
          LEN(ISNULL(LTRIM(RTRIM(R.[material_code])), N'')) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_packaging_material] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
  ) N
  WHERE N.dup_rn = 1
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
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_packaging_material]);
DECLARE @sap_excluded_paren INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_packaging_material] R
  WHERE LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) LIKE N'%（%'
     OR LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) LIKE N'%）%'
     OR LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%（%'
     OR LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%）%'
);
DECLARE @sap_eligible_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_packaging_material] R
  WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) NOT LIKE N'%（%'
    AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) NOT LIKE N'%）%'
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[packaging_material_code]))) = 18
          AND LTRIM(RTRIM(R.[packaging_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[packaging_material_code])), 10)
        ELSE LTRIM(RTRIM(R.[packaging_material_code]))
      END AS [packaging_material_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_packaging_material] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[packaging_material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[packaging_material_code]))) = 18
          AND LTRIM(RTRIM(R.[packaging_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[packaging_material_code])), 10)
        ELSE LTRIM(RTRIM(R.[packaging_material_code]))
      END
  ) K
);
DECLARE @dedupe_dropped INT = @sap_eligible_count - @sap_key_count;

IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', excluded_paren=', @sap_excluded_paren);
  THROW 50003, @src_msg, 1;
END;

IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [packaging_material_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ',
      [packaging_material_code], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [packaging_material_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  packaging_material_code NVARCHAR(20),
  material_code NVARCHAR(20),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  material_description_old NVARCHAR(40),
  material_description_new NVARCHAR(40),
  quantity_per_packing_old DECIMAL(18,2),
  quantity_per_packing_new DECIMAL(18,2),
  packaging_type_old NVARCHAR(40),
  packaging_type_new NVARCHAR(40)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_packaging_material] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_materials_packaging_material] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[packaging_material_code])) = S.[packaging_material_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[material_code], N''))) <> S.[material_code]
  OR LTRIM(RTRIM(ISNULL(T.[material_description], N''))) <> S.[material_description]
  OR LTRIM(RTRIM(ISNULL(T.[hs_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[hs_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[hs_name], N''))) <> LTRIM(RTRIM(ISNULL(S.[hs_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[additional_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[additional_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[origin_country_region_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[origin_country_region_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[origin_country_region_name], N''))) <> LTRIM(RTRIM(ISNULL(S.[origin_country_region_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[destination_country_region_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[destination_country_region_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[destination_country_region_name], N''))) <> LTRIM(RTRIM(ISNULL(S.[destination_country_region_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[regulatory_condition_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[regulatory_condition_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tariff_rate_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[tariff_rate_type], N'')))
  OR ISNULL(ROUND(T.[gross_weight], 10), -1) <> ISNULL(ROUND(S.[gross_weight], 10), -1)
  OR ISNULL(ROUND(T.[net_weight], 10), -1) <> ISNULL(ROUND(S.[net_weight], 10), -1)
  OR LTRIM(RTRIM(ISNULL(T.[weight_unit], N''))) <> S.[weight_unit]
  OR ISNULL(ROUND(T.[business_volume], 6), -1) <> ISNULL(ROUND(S.[business_volume], 6), -1)
  OR LTRIM(RTRIM(ISNULL(T.[volume_unit], N''))) <> S.[volume_unit]
  OR LTRIM(RTRIM(ISNULL(T.[size_dimension], N''))) <> LTRIM(RTRIM(ISNULL(S.[size_dimension], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[packaging_type], N''))) <> S.[packaging_type]
  OR LTRIM(RTRIM(ISNULL(T.[packing_unit], N''))) <> S.[packing_unit]
  OR ISNULL(ROUND(T.[quantity_per_packing], 2), -1) <> ISNULL(ROUND(S.[quantity_per_packing], 2), -1)
  OR LTRIM(RTRIM(ISNULL(T.[packaging_spec], N''))) <> LTRIM(RTRIM(ISNULL(S.[packaging_spec], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[packaging_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[packaging_description], N'')))
  OR T.[sort_order] <> S.[sort_order]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[material_code]=S.[material_code],
  T.[material_description]=S.[material_description],
  T.[hs_code]=S.[hs_code],
  T.[hs_name]=S.[hs_name],
  T.[additional_code]=S.[additional_code],
  T.[origin_country_region_code]=S.[origin_country_region_code],
  T.[origin_country_region_name]=S.[origin_country_region_name],
  T.[destination_country_region_code]=S.[destination_country_region_code],
  T.[destination_country_region_name]=S.[destination_country_region_name],
  T.[regulatory_condition_code]=S.[regulatory_condition_code],
  T.[tariff_rate_type]=S.[tariff_rate_type],
  T.[gross_weight]=S.[gross_weight],
  T.[net_weight]=S.[net_weight],
  T.[weight_unit]=S.[weight_unit],
  T.[business_volume]=S.[business_volume],
  T.[volume_unit]=S.[volume_unit],
  T.[size_dimension]=S.[size_dimension],
  T.[packaging_type]=S.[packaging_type],
  T.[packing_unit]=S.[packing_unit],
  T.[quantity_per_packing]=S.[quantity_per_packing],
  T.[packaging_spec]=S.[packaging_spec],
  T.[packaging_description]=S.[packaging_description],
  T.[sort_order]=S.[sort_order],
  T.[culture_code]=S.[culture_code],
  T.[ext_field]=S.[ext_field],
  T.[remark]=S.[remark],
  T.[created_by]=S.[created_by],
  T.[created_at]=S.[created_at],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[packaging_material_code],[material_code],[material_description],
    [hs_code],[hs_name],[additional_code],
    [origin_country_region_code],[origin_country_region_name],
    [destination_country_region_code],[destination_country_region_name],
    [regulatory_condition_code],[tariff_rate_type],
    [gross_weight],[net_weight],[weight_unit],[business_volume],[volume_unit],
    [size_dimension],[packaging_type],[packing_unit],[quantity_per_packing],
    [packaging_spec],[packaging_description],[sort_order],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[packaging_material_code],S.[material_code],S.[material_description],
    S.[hs_code],S.[hs_name],S.[additional_code],
    S.[origin_country_region_code],S.[origin_country_region_name],
    S.[destination_country_region_code],S.[destination_country_region_name],
    S.[regulatory_condition_code],S.[tariff_rate_type],
    S.[gross_weight],S.[net_weight],S.[weight_unit],S.[business_volume],S.[volume_unit],
    S.[size_dimension],S.[packaging_type],S.[packing_unit],S.[quantity_per_packing],
    S.[packaging_spec],S.[packaging_description],S.[sort_order],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
    S.[is_deleted],
    S.[deleted_by],S.[deleted_at]
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[packaging_material_code],
  INSERTED.[material_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[material_description], INSERTED.[material_description],
  DELETED.[quantity_per_packing], INSERTED.[quantity_per_packing],
  DELETED.[packaging_type], INSERTED.[packaging_type]
INTO #delta(
  rn, oper_type, id, plant_code, packaging_material_code, material_code,
  tenant_code, company_code, change_by,
  material_description_old, material_description_new,
  quantity_per_packing_old, quantity_per_packing_new,
  packaging_type_old, packaging_type_new
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

IF OBJECT_ID('tempdb..#soft_deleted_rows') IS NOT NULL DROP TABLE #soft_deleted_rows;
CREATE TABLE #soft_deleted_rows (
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [packaging_material_code] NVARCHAR(20)
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
  INSERTED.[packaging_material_code]
INTO #soft_deleted_rows ([id], [plant_code], [packaging_material_code])
FROM [takt_logistics_materials_packaging_material] T
WHERE T.[is_deleted] = 0
  AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[packaging_material_code] = LTRIM(RTRIM(T.[packaging_material_code]))
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
    ISNULL([packaging_material_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
IF LEN(@soft_deleted_keys) > 2000
BEGIN
  SET @soft_deleted_keys = LEFT(@soft_deleted_keys, 2000) + N'...(+more)';
END;
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_packaging_material] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @source_active_count INT = (
  SELECT COUNT(*)
  FROM #st_source
  WHERE [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_packaging_material] T
  WHERE EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_packaging_material] T
  WHERE T.[is_deleted]=1
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

IF @target_count <> @source_active_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(
    N'有效行数不一致: source=', @source_active_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;


/* per-row delta_log removed */


DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"sap_raw":' + CAST(@sap_raw_count AS NVARCHAR)
  + N',"excluded_paren":' + CAST(@sap_excluded_paren AS NVARCHAR)
  + N',"sap_eligible":' + CAST(@sap_eligible_count AS NVARCHAR)
  + N',"sap_keys":' + CAST(@sap_key_count AS NVARCHAR)
  + N',"dedupe_dropped":' + CAST(@dedupe_dropped AS NVARCHAR)
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
  N'takt_logistics_materials_packaging_material',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE PackagingMaterial Sync (summary)',
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
  N'包装物料',
  N'exec_sql_merge',
  'SQL',
  N'/sync/packaging-material',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @sap_raw_count AS [source_raw_count],
  @sap_excluded_paren AS [excluded_paren],
  @sap_eligible_count AS [sap_eligible_count],
  @sap_key_count AS [sap_key_count],
  @dedupe_dropped AS [dedupe_dropped],
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
