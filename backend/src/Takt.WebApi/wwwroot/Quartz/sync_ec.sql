SET NOCOUNT ON;
DECLARE @progress_msg NVARCHAR(400);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'start' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows],
  N'' AS [scope];
SET @progress_msg = N'QUARTZ_SYNC_PROGRESS|start|0|0|0|0|';
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @plant_code NVARCHAR(4) = N'{{PlantCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @now DATETIME = GETDATE();
DECLARE @apply_chunk INT = 100000;
DECLARE @merge_from_rn INT;
DECLARE @merge_to_rn INT;
DECLARE @merge_max_rn INT;
DECLARE @dml_n INT;
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;
-- 更新履历：主表 ext_field._sync.ec[]、子表 _sync.ecd[] 追加 { at, 变更列:{o,n} }；仅差异；超 nvarchar(4000) 保留原 JSON

IF OBJECT_ID('tempdb..#source_main') IS NOT NULL DROP TABLE #source_main;
IF OBJECT_ID('tempdb..#source_detail') IS NOT NULL DROP TABLE #source_detail;
IF OBJECT_ID('tempdb..#main_delta') IS NOT NULL DROP TABLE #main_delta;
IF OBJECT_ID('tempdb..#detail_delta') IS NOT NULL DROP TABLE #detail_delta;
IF OBJECT_ID('tempdb..#detail_idmap') IS NOT NULL DROP TABLE #detail_idmap;

CREATE TABLE #source_main (
  [rn] INT,
  [id] BIGINT,
  [source_ec_code] NVARCHAR(100),
  [source_model] NVARCHAR(100),
  [source_title] NVARCHAR(200),
  [source_status] NVARCHAR(100),
  [source_issue_date] DATE,
  [source_tcj_owner] NVARCHAR(MAX),
  [source_tcj_dependency] NVARCHAR(MAX),
  [source_ec_meeting] NVARCHAR(MAX),
  [source_pp_code] NVARCHAR(MAX),
  [source_technical_notice_code] NVARCHAR(MAX),
  [source_implementation] NVARCHAR(MAX),
  [source_main_change_reason] NVARCHAR(MAX),
  [source_secondary_change_reason] NVARCHAR(MAX),
  [source_safety_regulation] NVARCHAR(MAX),
  [source_progress_status] NVARCHAR(MAX),
  [source_serial_number_control] NVARCHAR(MAX),
  [source_customer_approval] NVARCHAR(MAX),
  [source_service_manual_revision] NVARCHAR(MAX),
  [source_user_manual_revision] NVARCHAR(MAX),
  [source_promotion_manual_revision] NVARCHAR(MAX),
  [source_standard_document_revision] NVARCHAR(MAX),
  [source_information_release] NVARCHAR(MAX),
  [source_cost_change] NVARCHAR(MAX),
  [source_unit_cost] DECIMAL(18,2),
  [source_mold_modification_cost] DECIMAL(18,2),
  [source_related_drawing] NVARCHAR(MAX),
  [source_ec_content] NVARCHAR(MAX),
  [created_at] DATETIME
);

CREATE TABLE #source_detail (
  [rn] INT,
  [id] BIGINT,
  [source_ec_id] BIGINT NULL,
  [source_ec_code] NVARCHAR(100),
  [line_number] INT NOT NULL DEFAULT 10,
  [source_old_material_code] NVARCHAR(100),
  [source_root_material_code] NVARCHAR(500),
  [source_parent_material_code] NVARCHAR(500),
  [source_old_material_description] NVARCHAR(MAX),
  [source_old_usage_quantity] NVARCHAR(MAX),
  [source_old_item_position] NVARCHAR(MAX),
  [source_new_material_code] NVARCHAR(MAX),
  [source_new_material_description] NVARCHAR(MAX),
  [source_new_usage_quantity] NVARCHAR(MAX),
  [source_new_item_position] NVARCHAR(MAX),
  [source_bom_code] NVARCHAR(MAX),
  [source_compatibility] NVARCHAR(MAX),
  [source_2nd_vendor] NVARCHAR(MAX),
  [source_instruction] NVARCHAR(MAX),
  [source_old_part_disposition] NVARCHAR(MAX),
  [source_bom_effective_date] DATE,
  [created_at] DATETIME
);

CREATE TABLE #main_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  source_ec_code NVARCHAR(100)
);

CREATE TABLE #detail_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  source_ec_id BIGINT,
  source_old_material_code NVARCHAR(100)
);

-- 主表源：PP_SapEcn 原样全量（空设变号除外）；created_at ← CreateTime
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'loadstart' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows],
  N'main' AS [scope];
SET @progress_msg = N'QUARTZ_SYNC_PROGRESS|loadstart|0|0|0|0|main';
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
INSERT INTO #source_main
SELECT
  S.rn,
  @base_id + S.rn,
  S.source_ec_code,
  ISNULL(S.[D_SAP_ZPABD_Z002], ''),
  ISNULL(S.[D_SAP_ZPABD_Z003], ''),
  ISNULL(S.[D_SAP_ZPABD_Z004], ''),
  COALESCE(TRY_CONVERT(DATE, NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPABD_Z005])), ''), 23), CAST('1900-01-01' AS DATE)),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z006] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z007] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z008] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z009] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z010] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z011] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z012] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z013] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z014] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z015] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z016] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z017] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z018] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z019] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z020] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z021] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z022] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z023] AS NVARCHAR(MAX)), ''),
  ISNULL(TRY_CAST(S.[D_SAP_ZPABD_Z024] AS DECIMAL(18,2)), 0),
  ISNULL(TRY_CAST(S.[D_SAP_ZPABD_Z025] AS DECIMAL(18,2)), 0),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z026] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_Z027] AS NVARCHAR(MAX)), N''),
  COALESCE(TRY_CAST(S.[CreateTime] AS DATETIME), @now)
FROM (
  SELECT *,
    LTRIM(RTRIM([D_SAP_ZPABD_Z001])) AS source_ec_code,
    ROW_NUMBER() OVER (ORDER BY LTRIM(RTRIM([D_SAP_ZPABD_Z001]))) AS rn
  FROM [Sap_Data].[dbo].[PP_SapEcn]
  WHERE LTRIM(RTRIM([D_SAP_ZPABD_Z001])) <> ''
) S;

DECLARE @main_source_count INT = (SELECT COUNT(*) FROM #source_main);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @main_source_count AS [to_rn],
  @main_source_count AS [max_rn],
  @main_source_count AS [batch_rows],
  N'main' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|load|1|',
  CAST(@main_source_count AS NVARCHAR(20)), N'|',
  CAST(@main_source_count AS NVARCHAR(20)), N'|',
  CAST(@main_source_count AS NVARCHAR(20)), N'|main');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @main_sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [Sap_Data].[dbo].[PP_SapEcn]
  WHERE LTRIM(RTRIM([D_SAP_ZPABD_Z001])) <> ''
);

IF @main_source_count <> @main_sap_raw_count
BEGIN
  DECLARE @main_src_msg NVARCHAR(200) = CONCAT(
    N'主表源行数与装入不一致: source=', @main_sap_raw_count, N', loaded=', @main_source_count);
  THROW 50003, @main_src_msg, 1;
END;

IF EXISTS (
  SELECT 1 FROM #source_main GROUP BY [source_ec_code] HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @main_dup NVARCHAR(400);
  SELECT TOP 1 @main_dup = CONCAT([source_ec_code], N' x', COUNT(*))
  FROM #source_main GROUP BY [source_ec_code] HAVING COUNT(*) > 1;
  THROW 50001, @main_dup, 1;
END;

-- 子表源：仅按设变号关联主表装入；created_at ← PP_SapEcnSub.CreateTime
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'loadstart' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = N'QUARTZ_SYNC_PROGRESS|loadstart|0|0|0|0|detail';
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
INSERT INTO #source_detail (
  [rn],[id],[source_ec_id],[source_ec_code],[source_old_material_code],
  [source_root_material_code],[source_parent_material_code],[source_old_material_description],
  [source_old_usage_quantity],[source_old_item_position],
  [source_new_material_code],[source_new_material_description],[source_new_usage_quantity],
  [source_new_item_position],[source_bom_code],
  [source_compatibility],[source_2nd_vendor],[source_instruction],
  [source_old_part_disposition],[source_bom_effective_date],[created_at]
)
SELECT
  S.rn,
  @base_id + 1000000000 + S.rn,
  NULL,
  S.source_ec_code,
  S.source_old_material_code,
  S.source_root_material_code,
  S.source_parent_material_code,
  S.source_old_material_description,
  S.source_old_usage_quantity,
  S.source_old_item_position,
  S.source_new_material_code,
  S.source_new_material_description,
  S.source_new_usage_quantity,
  S.source_new_item_position,
  S.source_bom_code,
  S.source_compatibility,
  S.source_2nd_vendor,
  S.source_instruction,
  S.source_old_part_disposition,
  S.source_bom_effective_date,
  S.created_at
FROM (
  SELECT
    LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])) AS source_ec_code,
    ISNULL(Sub.[D_SAP_ZPABD_S004], N'') AS source_old_material_code,
    ISNULL(Sub.[D_SAP_ZPABD_S002], N'') AS source_root_material_code,
    ISNULL(Sub.[D_SAP_ZPABD_S003], N'') AS source_parent_material_code,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S005] AS NVARCHAR(MAX)), N'') AS source_old_material_description,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S006] AS NVARCHAR(MAX)), N'') AS source_old_usage_quantity,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S007] AS NVARCHAR(MAX)), N'') AS source_old_item_position,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S008] AS NVARCHAR(MAX)), N'') AS source_new_material_code,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S009] AS NVARCHAR(MAX)), N'') AS source_new_material_description,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S010] AS NVARCHAR(MAX)), N'') AS source_new_usage_quantity,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S011] AS NVARCHAR(MAX)), N'') AS source_new_item_position,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S012] AS NVARCHAR(MAX)), N'') AS source_bom_code,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S013] AS NVARCHAR(MAX)), N'') AS source_compatibility,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S014] AS NVARCHAR(MAX)), N'') AS source_2nd_vendor,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S015] AS NVARCHAR(MAX)), N'') AS source_instruction,
    ISNULL(CAST(Sub.[D_SAP_ZPABD_S016] AS NVARCHAR(MAX)), N'') AS source_old_part_disposition,
    TRY_CONVERT(DATE, NULLIF(LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S017])), N''), 23) AS source_bom_effective_date,
    COALESCE(TRY_CAST(Sub.[CreateTime] AS DATETIME), @now) AS created_at,
    ROW_NUMBER() OVER (
      ORDER BY
        LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])),
        ISNULL(Sub.[D_SAP_ZPABD_S002], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S003], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S004], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S008], N''),
        ISNULL(Sub.[D_SAP_ZPABD_S012], N'')
    ) AS rn
  FROM [Sap_Data].[dbo].[PP_SapEcnSub] Sub
  INNER JOIN #source_main M
    ON M.[source_ec_code] = LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001]))
  WHERE LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])) <> N''
) S;

DECLARE @detail_source_count INT = (SELECT COUNT(*) FROM #source_detail);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @detail_source_count AS [to_rn],
  @detail_source_count AS [max_rn],
  @detail_source_count AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|load|1|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|detail');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @detail_sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [Sap_Data].[dbo].[PP_SapEcnSub] Sub
  WHERE LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001])) <> ''
    AND EXISTS (
      SELECT 1
      FROM [Sap_Data].[dbo].[PP_SapEcn] E
      WHERE LTRIM(RTRIM(E.[D_SAP_ZPABD_Z001])) <> ''
        AND LTRIM(RTRIM(E.[D_SAP_ZPABD_Z001])) = LTRIM(RTRIM(Sub.[D_SAP_ZPABD_S001]))
    )
);

IF @detail_source_count <> @detail_sap_raw_count
BEGIN
  DECLARE @detail_src_msg NVARCHAR(200) = CONCAT(
    N'子表源行数与装入不一致: source=', @detail_sap_raw_count, N', loaded=', @detail_source_count);
  THROW 50003, @detail_src_msg, 1;
END;

DECLARE @main_target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [plant_code] = @plant_code
    AND [is_deleted] = 0
);

-- 回填本司历史空工厂码，再按 Tenant+Company+Plant+SourceEcCode 对齐唯一索引做 MERGE
UPDATE [takt_logistics_manufacturing_ec_source]
SET [plant_code] = @plant_code
WHERE [tenant_code] = @tenant_code
  AND [company_code] = @company_code
  AND NULLIF(LTRIM(RTRIM([plant_code])), N'') IS NULL;

UPDATE [takt_logistics_manufacturing_ec_source_detail]
SET [plant_code] = @plant_code
WHERE [tenant_code] = @tenant_code
  AND [company_code] = @company_code
  AND NULLIF(LTRIM(RTRIM([plant_code])), N'') IS NULL;

-- 主表：存在则更新（有变化或恢复软删），不存在则插入；唯一键 Tenant+Company+Plant+SourceEcCode
SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #source_main), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_manufacturing_ec_source] AS T
USING (SELECT * FROM #source_main WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = @tenant_code
AND T.[company_code] = @company_code
AND T.[plant_code] = @plant_code
AND LTRIM(RTRIM(T.[source_ec_code])) = S.[source_ec_code]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[source_model], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_model], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_title], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_title], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_status], N'')))
  OR ISNULL(T.[source_issue_date], '1900-01-01') <> ISNULL(S.[source_issue_date], '1900-01-01')
  OR LTRIM(RTRIM(ISNULL(T.[source_tcj_owner], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_tcj_owner], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_tcj_dependency], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_tcj_dependency], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_ec_meeting], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_ec_meeting], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_pp_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_pp_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_technical_notice_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_technical_notice_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_implementation], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_implementation], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_main_change_reason], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_main_change_reason], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_secondary_change_reason], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_secondary_change_reason], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_safety_regulation], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_safety_regulation], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_progress_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_progress_status], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_serial_number_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_serial_number_control], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_customer_approval], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_customer_approval], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_service_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_service_manual_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_user_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_user_manual_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_promotion_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_promotion_manual_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_standard_document_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_standard_document_revision], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_information_release], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_information_release], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_cost_change], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_cost_change], N'')))
  OR ROUND(T.[source_unit_cost], 2) <> ROUND(S.[source_unit_cost], 2)
  OR ROUND(T.[source_mold_modification_cost], 2) <> ROUND(S.[source_mold_modification_cost], 2)
  OR LTRIM(RTRIM(ISNULL(T.[source_related_drawing], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_related_drawing], N'')))
  OR ISNULL(CAST(T.[source_ec_content] AS NVARCHAR(MAX)), N'') <> ISNULL(S.[source_ec_content], N'')
  OR ISNULL(T.[created_at], @now) <> ISNULL(S.[created_at], @now)
) THEN
  UPDATE SET
  T.[source_model]=S.[source_model],
  T.[source_title]=S.[source_title],
  T.[source_status]=S.[source_status],
  T.[source_issue_date]=S.[source_issue_date],
  T.[source_tcj_owner]=S.[source_tcj_owner],
  T.[source_tcj_dependency]=S.[source_tcj_dependency],
  T.[source_ec_meeting]=S.[source_ec_meeting],
  T.[source_pp_code]=S.[source_pp_code],
  T.[source_technical_notice_code]=S.[source_technical_notice_code],
  T.[source_implementation]=S.[source_implementation],
  T.[source_main_change_reason]=S.[source_main_change_reason],
  T.[source_secondary_change_reason]=S.[source_secondary_change_reason],
  T.[source_safety_regulation]=S.[source_safety_regulation],
  T.[source_progress_status]=S.[source_progress_status],
  T.[source_serial_number_control]=S.[source_serial_number_control],
  T.[source_customer_approval]=S.[source_customer_approval],
  T.[source_service_manual_revision]=S.[source_service_manual_revision],
  T.[source_user_manual_revision]=S.[source_user_manual_revision],
  T.[source_promotion_manual_revision]=S.[source_promotion_manual_revision],
  T.[source_standard_document_revision]=S.[source_standard_document_revision],
  T.[source_information_release]=S.[source_information_release],
  T.[source_cost_change]=S.[source_cost_change],
  T.[source_unit_cost]=S.[source_unit_cost],
  T.[source_mold_modification_cost]=S.[source_mold_modification_cost],
  T.[source_related_drawing]=S.[source_related_drawing],
  T.[source_ec_content]=S.[source_ec_content],
  T.[created_at]=COALESCE(S.[created_at], T.[created_at]),
  T.[updated_by]=@sync_user_id,
  T.[updated_at]=@now,
  T.[plant_code]=@plant_code,
  T.[culture_code]=@culture_code,
  T.[is_deleted]=0,
  T.[deleted_by]=NULL,
  T.[deleted_at]=NULL,
  T.[ext_field]=(
    SELECT CASE WHEN LEN(x.[new_ext]) <= 4000 THEN x.[new_ext] ELSE e0.[base_ext] END
    FROM (SELECT CASE WHEN ISJSON(NULLIF(LTRIM(RTRIM(CAST(ISNULL(T.[ext_field], N'') AS nvarchar(max)))), N'')) = 1 THEN LTRIM(RTRIM(CAST(T.[ext_field] AS nvarchar(max)))) ELSE N'{}' END AS [base_ext]) e0
    CROSS APPLY (SELECT CASE WHEN JSON_QUERY(e0.[base_ext], N'$._sync') IS NULL THEN JSON_MODIFY(e0.[base_ext], N'lax $._sync', JSON_QUERY(N'{}')) ELSE e0.[base_ext] END AS [with_root]) e1
    CROSS APPLY (SELECT CASE
      WHEN JSON_QUERY(e1.[with_root], N'$._sync.ec') IS NULL THEN JSON_MODIFY(e1.[with_root], N'lax $._sync.ec', JSON_QUERY(N'[]'))
      WHEN LEFT(LTRIM(ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.ec'), N'')), 1) = N'[' THEN e1.[with_root]
      ELSE JSON_MODIFY(e1.[with_root], N'lax $._sync.ec', JSON_QUERY(N'[' + ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.ec'), N'{}') + N']'))
    END AS [with_arr]) e2
    CROSS APPLY (SELECT JSON_MODIFY(e2.[with_arr], N'append $._sync.ec', JSON_QUERY(CONCAT(
      N'{"at":"', CONVERT(VARCHAR(19), @now, 126), N'"',
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_model], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_model], N''))) THEN CONCAT(N',"source_model":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_model], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_model], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_title], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_title], N''))) THEN CONCAT(N',"source_title":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_title], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_title], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_status], N''))) THEN CONCAT(N',"source_status":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[source_issue_date], '1900-01-01') <> ISNULL(S.[source_issue_date], '1900-01-01') THEN CONCAT(N',"source_issue_date":{"o":"', ISNULL(CONVERT(VARCHAR(10), T.[source_issue_date], 23), N''), N'","n":"', ISNULL(CONVERT(VARCHAR(10), S.[source_issue_date], 23), N''), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_tcj_owner], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_tcj_owner], N''))) THEN CONCAT(N',"source_tcj_owner":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_tcj_owner], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_tcj_owner], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_tcj_dependency], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_tcj_dependency], N''))) THEN CONCAT(N',"source_tcj_dependency":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_tcj_dependency], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_tcj_dependency], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_ec_meeting], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_ec_meeting], N''))) THEN CONCAT(N',"source_ec_meeting":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_ec_meeting], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_ec_meeting], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_pp_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_pp_code], N''))) THEN CONCAT(N',"source_pp_code":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_pp_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_pp_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_technical_notice_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_technical_notice_code], N''))) THEN CONCAT(N',"source_technical_notice_code":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_technical_notice_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_technical_notice_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_implementation], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_implementation], N''))) THEN CONCAT(N',"source_implementation":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_implementation], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_implementation], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_main_change_reason], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_main_change_reason], N''))) THEN CONCAT(N',"source_main_change_reason":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_main_change_reason], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_main_change_reason], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_secondary_change_reason], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_secondary_change_reason], N''))) THEN CONCAT(N',"source_secondary_change_reason":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_secondary_change_reason], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_secondary_change_reason], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_safety_regulation], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_safety_regulation], N''))) THEN CONCAT(N',"source_safety_regulation":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_safety_regulation], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_safety_regulation], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_progress_status], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_progress_status], N''))) THEN CONCAT(N',"source_progress_status":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_progress_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_progress_status], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_serial_number_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_serial_number_control], N''))) THEN CONCAT(N',"source_serial_number_control":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_serial_number_control], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_serial_number_control], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_customer_approval], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_customer_approval], N''))) THEN CONCAT(N',"source_customer_approval":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_customer_approval], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_customer_approval], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_service_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_service_manual_revision], N''))) THEN CONCAT(N',"source_service_manual_revision":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_service_manual_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_service_manual_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_user_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_user_manual_revision], N''))) THEN CONCAT(N',"source_user_manual_revision":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_user_manual_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_user_manual_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_promotion_manual_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_promotion_manual_revision], N''))) THEN CONCAT(N',"source_promotion_manual_revision":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_promotion_manual_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_promotion_manual_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_standard_document_revision], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_standard_document_revision], N''))) THEN CONCAT(N',"source_standard_document_revision":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_standard_document_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_standard_document_revision], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_information_release], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_information_release], N''))) THEN CONCAT(N',"source_information_release":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_information_release], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_information_release], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_cost_change], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_cost_change], N''))) THEN CONCAT(N',"source_cost_change":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_cost_change], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_cost_change], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ROUND(T.[source_unit_cost], 2) <> ROUND(S.[source_unit_cost], 2) THEN CONCAT(N',"source_unit_cost":{"o":', CONVERT(VARCHAR(40), ROUND(T.[source_unit_cost], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[source_unit_cost], 2)), N'}') ELSE N'' END,
      CASE WHEN ROUND(T.[source_mold_modification_cost], 2) <> ROUND(S.[source_mold_modification_cost], 2) THEN CONCAT(N',"source_mold_modification_cost":{"o":', CONVERT(VARCHAR(40), ROUND(T.[source_mold_modification_cost], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[source_mold_modification_cost], 2)), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_related_drawing], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_related_drawing], N''))) THEN CONCAT(N',"source_related_drawing":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_related_drawing], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_related_drawing], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(CAST(T.[source_ec_content] AS NVARCHAR(MAX)), N'') <> ISNULL(S.[source_ec_content], N'') THEN CONCAT(N',"source_ec_content":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(CAST(T.[source_ec_content] AS NVARCHAR(MAX)), N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_ec_content], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[created_at], @now) <> ISNULL(S.[created_at], @now) THEN CONCAT(N',"created_at":{"o":"', ISNULL(CONVERT(VARCHAR(19), T.[created_at], 126), N''), N'","n":"', ISNULL(CONVERT(VARCHAR(19), S.[created_at], 126), N''), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[is_deleted], 0) <> 0 THEN N',"is_deleted":{"o":1,"n":0}' ELSE N'' END,
      N'}'))) AS [new_ext]) x
  )
WHEN NOT MATCHED THEN
  INSERT (
    [id],[source_ec_code],[source_model],[source_title],[source_status],
    [source_issue_date],[source_tcj_owner],[source_tcj_dependency],
    [source_ec_meeting],[source_pp_code],[source_technical_notice_code],
    [source_implementation],[source_main_change_reason],
    [source_secondary_change_reason],[source_safety_regulation],
    [source_progress_status],[source_serial_number_control],
    [source_customer_approval],[source_service_manual_revision],
    [source_user_manual_revision],[source_promotion_manual_revision],
    [source_standard_document_revision],[source_information_release],
    [source_cost_change],[source_unit_cost],
    [source_mold_modification_cost],[source_related_drawing],
    [source_ec_content],[tenant_code],[company_code],[plant_code],[culture_code],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
  )
  VALUES (
    S.[id],S.[source_ec_code],S.[source_model],S.[source_title],S.[source_status],
    S.[source_issue_date],S.[source_tcj_owner],S.[source_tcj_dependency],
    S.[source_ec_meeting],S.[source_pp_code],S.[source_technical_notice_code],
    S.[source_implementation],S.[source_main_change_reason],
    S.[source_secondary_change_reason],S.[source_safety_regulation],
    S.[source_progress_status],S.[source_serial_number_control],
    S.[source_customer_approval],S.[source_service_manual_revision],
    S.[source_user_manual_revision],S.[source_promotion_manual_revision],
    S.[source_standard_document_revision],S.[source_information_release],
    S.[source_cost_change],S.[source_unit_cost],
    S.[source_mold_modification_cost],S.[source_related_drawing],
    S.[source_ec_content],@tenant_code,@company_code,@plant_code,@culture_code,
    @sync_user_id,COALESCE(S.[created_at], @now),@sync_user_id,@now,0
  )
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[source_ec_code]
INTO #main_delta(rn, oper_type, id, source_ec_code);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows],
    N'main' AS [scope];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'main');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END

-- 主表孤儿软删：目标有而源没有（仅 is_deleted=0）
IF OBJECT_ID('tempdb..#main_soft_deleted_rows') IS NOT NULL DROP TABLE #main_soft_deleted_rows;
CREATE TABLE #main_soft_deleted_rows (
  [id] BIGINT,
  [source_ec_code] NVARCHAR(100)
);


DECLARE @main_delete_count INT = 0;
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
OUTPUT INSERTED.[id], INSERTED.[source_ec_code]
INTO #main_soft_deleted_rows ([id], [source_ec_code])
FROM [takt_logistics_manufacturing_ec_source] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[plant_code] = @plant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #source_main S
    WHERE S.[source_ec_code] = LTRIM(RTRIM(T.[source_ec_code]))
  );
  SET @dml_n = @@ROWCOUNT;
  SET @main_delete_count = @main_delete_count + @dml_n;
END
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'soft' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  @main_delete_count AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  @main_delete_count AS [batch_rows],
  N'main' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|soft|0|',
  CAST(@main_delete_count AS NVARCHAR(20)), N'|0|',
  CAST(@main_delete_count AS NVARCHAR(20)), N'|main');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @main_soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @main_soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([source_ec_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #main_soft_deleted_rows;
SET @main_soft_deleted_keys = ISNULL(@main_soft_deleted_keys, N'');

DECLARE @main_target_count INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [plant_code] = @plant_code AND [is_deleted] = 0
);
DECLARE @main_target_physical INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [plant_code] = @plant_code
);
DECLARE @main_soft_deleted INT = (
  SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]
  WHERE [tenant_code] = @tenant_code AND [company_code] = @company_code AND [plant_code] = @plant_code AND [is_deleted] = 1
);

DECLARE @detail_target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [plant_code] = @plant_code
    AND [is_deleted] = 0
);

UPDATE [takt_logistics_manufacturing_ec_source_detail]
SET [plant_code] = @plant_code
WHERE [tenant_code] = @tenant_code
  AND [company_code] = @company_code
  AND NULLIF(LTRIM(RTRIM([plant_code])), N'') IS NULL;

-- 子表回填目标主表 id（MERGE 后主表 id）
UPDATE D
SET D.[source_ec_id] = M.[id]
FROM #source_detail D
INNER JOIN [takt_logistics_manufacturing_ec_source] M
  ON M.[tenant_code] = @tenant_code
 AND M.[company_code] = @company_code
 AND M.[plant_code] = @plant_code
 AND M.[source_ec_code] = D.[source_ec_code]
 AND M.[is_deleted] = 0;

IF EXISTS (SELECT 1 FROM #source_detail WHERE [source_ec_id] IS NULL)
BEGIN
  DECLARE @ec_map_n INT = (SELECT COUNT(*) FROM #source_detail WHERE [source_ec_id] IS NULL);
  DECLARE @ec_map_sample NVARCHAR(80) = N'';
  SELECT TOP 1 @ec_map_sample = [source_ec_code] FROM #source_detail WHERE [source_ec_id] IS NULL;
  DECLARE @ec_map_msg NVARCHAR(200) = CONCAT(N'孤儿明细: n=', @ec_map_n, N', sample=', @ec_map_sample);
  THROW 50004, @ec_map_msg, 1;
END;

-- 业务键：主表id+完成品+上阶+旧件+新件+BOM+安装位置+生效日（禁止仅按旧件料号丢行）
-- SAP 源偶发完全同键重复行：保留 rn 最小一行，计入 dedupe_dropped（不再 THROW 中断日链）
DECLARE @detail_before_dedupe INT = (SELECT COUNT(*) FROM #source_detail);
;WITH detail_bk AS (
  SELECT
    [id],
    ROW_NUMBER() OVER (
      PARTITION BY
        [source_ec_id],
        LTRIM(RTRIM(ISNULL([source_root_material_code], N''))),
        LTRIM(RTRIM(ISNULL([source_parent_material_code], N''))),
        LTRIM(RTRIM(ISNULL([source_old_material_code], N''))),
        LTRIM(RTRIM(ISNULL([source_new_material_code], N''))),
        LTRIM(RTRIM(ISNULL([source_bom_code], N''))),
        LTRIM(RTRIM(ISNULL([source_old_item_position], N''))),
        LTRIM(RTRIM(ISNULL([source_new_item_position], N''))),
        ISNULL([source_bom_effective_date], CAST('1900-01-01' AS DATE))
      ORDER BY [rn]
    ) AS [bk_rn]
  FROM #source_detail
)
DELETE D
FROM #source_detail D
INNER JOIN detail_bk B ON B.[id] = D.[id]
WHERE B.[bk_rn] > 1;

DECLARE @detail_dedupe_dropped INT = @detail_before_dedupe - (SELECT COUNT(*) FROM #source_detail);
SET @detail_source_count = (SELECT COUNT(*) FROM #source_detail);

-- 业务键对齐：禁止对 50 万行 OUTER APPLY 相关子查询（更新路径会卡死数十分钟）
-- 键列含 NVARCHAR(MAX)/500，禁止直接建索引；用 SHA2_256 业务键哈希 + source_ec_id 哈希 JOIN
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'idmap' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  @detail_source_count AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|idmap|0|0|0|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|detail');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

;WITH target_bk AS (
  SELECT
    X.[id],
    X.[line_number],
    X.[source_ec_id],
    LTRIM(RTRIM(ISNULL(X.[source_root_material_code], N''))) AS [bk_root_material_code],
    HASHBYTES(
      N'SHA2_256',
      CONCAT(
        CAST(X.[source_ec_id] AS NVARCHAR(30)), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_root_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_parent_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_old_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_new_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_bom_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_old_item_position], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(X.[source_new_item_position], N''))), 400), N'|',
        CONVERT(VARCHAR(10), ISNULL(X.[source_bom_effective_date], CAST('1900-01-01' AS DATE)), 23)
      )
    ) AS [bk_hash],
    ROW_NUMBER() OVER (
      PARTITION BY
        X.[source_ec_id],
        HASHBYTES(
          N'SHA2_256',
          CONCAT(
            CAST(X.[source_ec_id] AS NVARCHAR(30)), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_root_material_code], N''))), 400), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_parent_material_code], N''))), 400), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_old_material_code], N''))), 400), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_new_material_code], N''))), 400), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_bom_code], N''))), 400), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_old_item_position], N''))), 400), N'|',
            LEFT(LTRIM(RTRIM(ISNULL(X.[source_new_item_position], N''))), 400), N'|',
            CONVERT(VARCHAR(10), ISNULL(X.[source_bom_effective_date], CAST('1900-01-01' AS DATE)), 23)
          )
        )
      ORDER BY X.[is_deleted] ASC, X.[id] ASC
    ) AS [bk_rn]
  FROM [takt_logistics_manufacturing_ec_source_detail] X
  WHERE X.[tenant_code] = @tenant_code
    AND X.[company_code] = @company_code
    AND X.[plant_code] = @plant_code
)
SELECT
  [id],
  [line_number],
  [source_ec_id],
  [bk_root_material_code],
  [bk_hash]
INTO #detail_idmap
FROM target_bk
WHERE [bk_rn] = 1;

CREATE UNIQUE CLUSTERED INDEX [ix_detail_idmap_bk]
ON #detail_idmap ([source_ec_id], [bk_hash]);

CREATE NONCLUSTERED INDEX [ix_detail_idmap_fg_line]
ON #detail_idmap ([source_ec_id], [bk_root_material_code], [line_number]);

UPDATE S
SET S.[id] = COALESCE(M.[id], S.[id]),
    S.[line_number] = COALESCE(M.[line_number], S.[line_number])
FROM #source_detail S
LEFT JOIN #detail_idmap M
  ON M.[source_ec_id] = S.[source_ec_id]
 AND M.[bk_hash] = HASHBYTES(
      N'SHA2_256',
      CONCAT(
        CAST(S.[source_ec_id] AS NVARCHAR(30)), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_root_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_parent_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_old_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_new_material_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_bom_code], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_old_item_position], N''))), 400), N'|',
        LEFT(LTRIM(RTRIM(ISNULL(S.[source_new_item_position], N''))), 400), N'|',
        CONVERT(VARCHAR(10), ISNULL(S.[source_bom_effective_date], CAST('1900-01-01' AS DATE)), 23)
      )
    );

-- 未命中：按唯一键 Tenant+Company+SourceEcId+SourceRootMaterialCode 取 MAX(line_number) 后步长 10
;WITH occupied AS (
  SELECT
    [source_ec_id],
    [bk_root_material_code],
    ISNULL(MAX([line_number]), 0) AS [max_line]
  FROM #detail_idmap
  GROUP BY [source_ec_id], [bk_root_material_code]
),
fresh AS (
  SELECT
    S.[id],
    ISNULL(O.[max_line], 0) + 10 * ROW_NUMBER() OVER (
      PARTITION BY S.[source_ec_id], LTRIM(RTRIM(ISNULL(S.[source_root_material_code], N'')))
      ORDER BY S.[rn]
    ) AS [new_line]
  FROM #source_detail S
  LEFT JOIN occupied O
    ON O.[source_ec_id] = S.[source_ec_id]
   AND O.[bk_root_material_code] = LTRIM(RTRIM(ISNULL(S.[source_root_material_code], N'')))
  WHERE NOT EXISTS (
    SELECT 1
    FROM #detail_idmap M
    WHERE M.[id] = S.[id]
  )
)
UPDATE D
SET D.[line_number] = F.[new_line]
FROM #source_detail D
INNER JOIN fresh F ON F.[id] = D.[id];

SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'idmap' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @detail_source_count AS [to_rn],
  @detail_source_count AS [max_rn],
  @detail_source_count AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|idmap|1|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|',
  CAST(@detail_source_count AS NVARCHAR(20)), N'|detail');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

IF OBJECT_ID('tempdb..#detail_soft_deleted_rows') IS NOT NULL DROP TABLE #detail_soft_deleted_rows;
CREATE TABLE #detail_soft_deleted_rows (
  [id] BIGINT,
  [source_ec_id] BIGINT,
  [source_old_material_code] NVARCHAR(100)
);

-- 先回填目标子表主外键，再按 id 同步
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
  UPDATE TOP (@apply_chunk) D
  SET D.[source_ec_id] = M.[id], D.[updated_by] = @sync_user_id, D.[updated_at] = @now
  FROM [takt_logistics_manufacturing_ec_source_detail] AS D
  INNER JOIN [takt_logistics_manufacturing_ec_source] AS M
    ON M.[tenant_code] = D.[tenant_code]
   AND M.[company_code] = D.[company_code]
   AND M.[plant_code] = D.[plant_code]
   AND M.[source_ec_code] = D.[source_ec_code]
   AND M.[is_deleted] = 0
  WHERE D.[tenant_code] = @tenant_code
    AND D.[company_code] = @company_code
    AND D.[plant_code] = @plant_code
    AND D.[is_deleted] = 0
    AND (D.[source_ec_id] IS NULL OR D.[source_ec_id] <> M.[id]);
  SET @dml_n = @@ROWCOUNT;
END

-- 子表：已映射 id → UPDATE；新 id → INSERT；源无键 → 软删
SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #source_detail), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_manufacturing_ec_source_detail] AS T
USING (SELECT * FROM #source_detail WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = @tenant_code
AND T.[company_code] = @company_code
AND T.[plant_code] = @plant_code
AND T.[id] = S.[id]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[source_old_material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_old_material_description], N'')))
  OR ISNULL(T.[source_old_usage_quantity], -1) <> ISNULL(TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_old_usage_quantity] AS NVARCHAR(40)))), N'')), -1)
  OR LTRIM(RTRIM(ISNULL(T.[source_new_material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_new_material_description], N'')))
  OR ISNULL(T.[source_new_usage_quantity], -1) <> ISNULL(TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_new_usage_quantity] AS NVARCHAR(40)))), N'')), -1)
  OR LTRIM(RTRIM(ISNULL(T.[source_compatibility], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_compatibility], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_2nd_vendor], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_2nd_vendor], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_instruction], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_instruction], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[source_old_part_disposition], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_old_part_disposition], N'')))
  OR ISNULL(T.[created_at], @now) <> ISNULL(S.[created_at], @now)
) THEN
  UPDATE SET
  T.[source_old_material_description]=S.[source_old_material_description],
  T.[source_old_usage_quantity]=TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_old_usage_quantity] AS NVARCHAR(40)))), N'')),
  T.[source_new_material_description]=S.[source_new_material_description],
  T.[source_new_usage_quantity]=TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_new_usage_quantity] AS NVARCHAR(40)))), N'')),
  T.[source_compatibility]=S.[source_compatibility],
  T.[source_2nd_vendor]=S.[source_2nd_vendor],
  T.[source_instruction]=S.[source_instruction],
  T.[source_old_part_disposition]=S.[source_old_part_disposition],
  T.[created_at]=COALESCE(S.[created_at], T.[created_at]),
  T.[updated_by]=@sync_user_id,
  T.[updated_at]=@now,
  T.[culture_code]=@culture_code,
  T.[is_deleted]=0,
  T.[deleted_by]=NULL,
  T.[deleted_at]=NULL,
  T.[ext_field]=(
    SELECT CASE WHEN LEN(x.[new_ext]) <= 4000 THEN x.[new_ext] ELSE e0.[base_ext] END
    FROM (SELECT CASE WHEN ISJSON(NULLIF(LTRIM(RTRIM(CAST(ISNULL(T.[ext_field], N'') AS nvarchar(max)))), N'')) = 1 THEN LTRIM(RTRIM(CAST(T.[ext_field] AS nvarchar(max)))) ELSE N'{}' END AS [base_ext]) e0
    CROSS APPLY (SELECT CASE WHEN JSON_QUERY(e0.[base_ext], N'$._sync') IS NULL THEN JSON_MODIFY(e0.[base_ext], N'lax $._sync', JSON_QUERY(N'{}')) ELSE e0.[base_ext] END AS [with_root]) e1
    CROSS APPLY (SELECT CASE
      WHEN JSON_QUERY(e1.[with_root], N'$._sync.ecd') IS NULL THEN JSON_MODIFY(e1.[with_root], N'lax $._sync.ecd', JSON_QUERY(N'[]'))
      WHEN LEFT(LTRIM(ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.ecd'), N'')), 1) = N'[' THEN e1.[with_root]
      ELSE JSON_MODIFY(e1.[with_root], N'lax $._sync.ecd', JSON_QUERY(N'[' + ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.ecd'), N'{}') + N']'))
    END AS [with_arr]) e2
    CROSS APPLY (SELECT JSON_MODIFY(e2.[with_arr], N'append $._sync.ecd', JSON_QUERY(CONCAT(
      N'{"at":"', CONVERT(VARCHAR(19), @now, 126), N'"',
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_old_material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_old_material_description], N''))) THEN CONCAT(N',"source_old_material_description":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_old_material_description], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_old_material_description], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[source_old_usage_quantity], -1) <> ISNULL(TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_old_usage_quantity] AS NVARCHAR(40)))), N'')), -1) THEN CONCAT(N',"source_old_usage_quantity":{"o":', ISNULL(CONVERT(VARCHAR(40), T.[source_old_usage_quantity]), N'null'), N',"n":', ISNULL(CONVERT(VARCHAR(40), TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_old_usage_quantity] AS NVARCHAR(40)))), N''))), N'null'), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_new_material_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_new_material_description], N''))) THEN CONCAT(N',"source_new_material_description":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_new_material_description], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_new_material_description], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[source_new_usage_quantity], -1) <> ISNULL(TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_new_usage_quantity] AS NVARCHAR(40)))), N'')), -1) THEN CONCAT(N',"source_new_usage_quantity":{"o":', ISNULL(CONVERT(VARCHAR(40), T.[source_new_usage_quantity]), N'null'), N',"n":', ISNULL(CONVERT(VARCHAR(40), TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_new_usage_quantity] AS NVARCHAR(40)))), N''))), N'null'), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_compatibility], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_compatibility], N''))) THEN CONCAT(N',"source_compatibility":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_compatibility], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_compatibility], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_2nd_vendor], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_2nd_vendor], N''))) THEN CONCAT(N',"source_2nd_vendor":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_2nd_vendor], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_2nd_vendor], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_instruction], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_instruction], N''))) THEN CONCAT(N',"source_instruction":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_instruction], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_instruction], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[source_old_part_disposition], N''))) <> LTRIM(RTRIM(ISNULL(S.[source_old_part_disposition], N''))) THEN CONCAT(N',"source_old_part_disposition":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[source_old_part_disposition], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[source_old_part_disposition], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[created_at], @now) <> ISNULL(S.[created_at], @now) THEN CONCAT(N',"created_at":{"o":"', ISNULL(CONVERT(VARCHAR(19), T.[created_at], 126), N''), N'","n":"', ISNULL(CONVERT(VARCHAR(19), S.[created_at], 126), N''), N'"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[is_deleted], 0) <> 0 THEN N',"is_deleted":{"o":1,"n":0}' ELSE N'' END,
      N'}'))) AS [new_ext]) x
  )
WHEN NOT MATCHED THEN
  INSERT (
    [id],[source_ec_id],[source_ec_code],[line_number],
    [source_root_material_code],[source_parent_material_code],
    [source_old_material_code],[source_old_material_description],[source_old_usage_quantity],
    [source_old_item_position],[source_new_material_code],
    [source_new_material_description],[source_new_usage_quantity],
    [source_new_item_position],[source_bom_code],
    [source_compatibility],[source_2nd_vendor],
    [source_instruction],[source_old_part_disposition],
    [source_bom_effective_date],[tenant_code],[company_code],[plant_code],[culture_code],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted]
  )
  VALUES (
    S.[id],S.[source_ec_id],S.[source_ec_code],S.[line_number],
    S.[source_root_material_code],S.[source_parent_material_code],
    S.[source_old_material_code],S.[source_old_material_description],
    TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_old_usage_quantity] AS NVARCHAR(40)))), N'')),
    S.[source_old_item_position],S.[source_new_material_code],
    S.[source_new_material_description],
    TRY_CONVERT(DECIMAL(18,5), NULLIF(LTRIM(RTRIM(CAST(S.[source_new_usage_quantity] AS NVARCHAR(40)))), N'')),
    S.[source_new_item_position],S.[source_bom_code],
    S.[source_compatibility],S.[source_2nd_vendor],
    S.[source_instruction],S.[source_old_part_disposition],
    S.[source_bom_effective_date],@tenant_code,@company_code,@plant_code,@culture_code,
    @sync_user_id,COALESCE(S.[created_at], @now),@sync_user_id,@now,0
  )
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[source_ec_id], INSERTED.[source_old_material_code]
INTO #detail_delta(rn, oper_type, id, source_ec_id, source_old_material_code);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows],
    N'detail' AS [scope];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'detail');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END


-- 对齐后源行已持有目标 id：孤儿 = 目标有效行 id 不在 #source_detail（禁止再按业务键 LTRIM 全表对比）
DECLARE @detail_delete_count INT = 0;
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
OUTPUT INSERTED.[id], INSERTED.[source_ec_id], INSERTED.[source_old_material_code]
INTO #detail_soft_deleted_rows ([id], [source_ec_id], [source_old_material_code])
FROM [takt_logistics_manufacturing_ec_source_detail] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[plant_code] = @plant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #source_detail S
    WHERE S.[id] = T.[id]
  );
  SET @dml_n = @@ROWCOUNT;
  SET @detail_delete_count = @detail_delete_count + @dml_n;
END
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'soft' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  @detail_delete_count AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  @detail_delete_count AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|soft|0|',
  CAST(@detail_delete_count AS NVARCHAR(20)), N'|0|',
  CAST(@detail_delete_count AS NVARCHAR(20)), N'|detail');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

DECLARE @detail_soft_deleted_keys NVARCHAR(MAX) = N'';
SELECT @detail_soft_deleted_keys = STRING_AGG(
  CAST(
    CONCAT(
    CAST([id] AS NVARCHAR(30)), N'|',
    CAST([source_ec_id] AS NVARCHAR(30)), N'/',
    ISNULL([source_old_material_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (
  SELECT TOP (200) [id], [source_ec_id], [source_old_material_code]
  FROM #detail_soft_deleted_rows
  ORDER BY [id]
) K;
SET @detail_soft_deleted_keys = ISNULL(@detail_soft_deleted_keys, N'');

DECLARE @detail_target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail] T
  INNER JOIN [takt_logistics_manufacturing_ec_source] M ON M.[id] = T.[source_ec_id]
  WHERE T.[tenant_code] = @tenant_code
    AND T.[company_code] = @company_code
    AND T.[plant_code] = @plant_code
    AND T.[is_deleted] = 0
    AND M.[tenant_code] = @tenant_code
    AND M.[company_code] = @company_code
    AND M.[plant_code] = @plant_code
    AND M.[is_deleted] = 0
);
DECLARE @detail_target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [plant_code] = @plant_code
);
DECLARE @detail_soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_ec_source_detail]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [plant_code] = @plant_code
    AND [is_deleted] = 1
);

DECLARE @main_ins INT = (SELECT COUNT(*) FROM #main_delta WHERE oper_type = 'INSERT');
DECLARE @main_upd INT = (SELECT COUNT(*) FROM #main_delta WHERE oper_type = 'UPDATE');
DECLARE @main_unchanged_count INT = @main_source_count - @main_ins - @main_upd;
DECLARE @detail_ins INT = (SELECT COUNT(*) FROM #detail_delta WHERE oper_type = 'INSERT');
DECLARE @detail_upd INT = (SELECT COUNT(*) FROM #detail_delta WHERE oper_type = 'UPDATE');
DECLARE @detail_unchanged_count INT = @detail_source_count - @detail_ins - @detail_upd;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"main_sap_raw":' + CAST(@main_sap_raw_count AS NVARCHAR)
  + N',"main_source":' + CAST(@main_source_count AS NVARCHAR)
  + N',"main_target_before":' + CAST(@main_target_before AS NVARCHAR)
  + N',"main_target_after":' + CAST(@main_target_count AS NVARCHAR)
  + N',"main_target_physical":' + CAST(@main_target_physical AS NVARCHAR)
  + N',"main_soft_deleted":' + CAST(@main_soft_deleted AS NVARCHAR)
  + N',"main_insert":' + CAST(@main_ins AS NVARCHAR)
  + N',"main_update":' + CAST(@main_upd AS NVARCHAR)
  + N',"main_unchanged":' + CAST(@main_unchanged_count AS NVARCHAR)
  + N',"main_soft_delete_this_run":' + CAST(@main_delete_count AS NVARCHAR)
  + N',"main_soft_delete_keys":"' + REPLACE(@main_soft_deleted_keys, N'"', N'''') + N'"'
  + N',"detail_sap_raw":' + CAST(@detail_sap_raw_count AS NVARCHAR)
  + N',"detail_source":' + CAST(@detail_source_count AS NVARCHAR)
  + N',"detail_dedupe_dropped":' + CAST(@detail_dedupe_dropped AS NVARCHAR)
  + N',"detail_target_before":' + CAST(@detail_target_before AS NVARCHAR)
  + N',"detail_target_after":' + CAST(@detail_target_count AS NVARCHAR)
  + N',"detail_target_physical":' + CAST(@detail_target_physical AS NVARCHAR)
  + N',"detail_soft_deleted":' + CAST(@detail_soft_deleted AS NVARCHAR)
  + N',"detail_insert":' + CAST(@detail_ins AS NVARCHAR)
  + N',"detail_update":' + CAST(@detail_upd AS NVARCHAR)
  + N',"detail_unchanged":' + CAST(@detail_unchanged_count AS NVARCHAR)
  + N',"detail_soft_delete_this_run":' + CAST(@detail_delete_count AS NVARCHAR)
  + N',"detail_soft_delete_keys":"' + REPLACE(@detail_soft_deleted_keys, N'"', N'''') + N'"}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[plant_code],[culture_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',N'SYNC',N'EC management',
  N'exec_sql_merge','SQL',N'/sync/ec','',
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

-- Quartz 执行器读取此结果集写入 ExecuteMessage / quartz-.log
SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'main' AS NVARCHAR(40)) AS [scope],
  @main_sap_raw_count AS [source_raw_count],
  @main_source_count AS [source_count],
  CAST(0 AS INT) AS [dedupe_dropped],
  @main_target_before AS [target_before],
  @main_target_count AS [target_after],
  @main_target_physical AS [target_physical],
  @main_soft_deleted AS [soft_deleted],
  @main_ins AS [insert_count],
  @main_upd AS [update_count],
  @main_unchanged_count AS [unchanged_count],
  @main_delete_count AS [delete_count],
  @main_soft_deleted_keys AS [soft_deleted_keys]
UNION ALL
SELECT
  N'QUARTZ_SYNC_SUMMARY',
  CAST(N'detail' AS NVARCHAR(40)),
  @detail_sap_raw_count,
  @detail_source_count,
  @detail_dedupe_dropped,
  @detail_target_before,
  @detail_target_count,
  @detail_target_physical,
  @detail_soft_deleted,
  @detail_ins,
  @detail_upd,
  @detail_unchanged_count,
  @detail_delete_count,
  @detail_soft_deleted_keys;
