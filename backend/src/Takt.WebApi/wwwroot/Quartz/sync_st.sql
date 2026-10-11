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
-- 生效日期：当前-10天（与组立日报默认生产日期对齐，保证 by-material 有效期命中）
DECLARE @effective_date DATE = DATEADD(DAY, -10, CAST(@now AS DATE));
-- 更新履历：ext_field._sync.st[] 追加 { at, 变更列:{o,n} }；仅差异；超 nvarchar(4000) 保留原 JSON
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [material_code] NVARCHAR(100),
  [work_center] NVARCHAR(100),
  [operation_desc] NVARCHAR(200),
  [standard_minutes] DECIMAL(18,2),
  [time_unit] NVARCHAR(20),
  [standard_shorts] INT,
  [points_unit] NVARCHAR(20),
  [points_to_minutes_rate] DECIMAL(18,3),
  [converted_minutes] DECIMAL(18,2),
  [tenant_code] NVARCHAR(20),
  [company_code] NVARCHAR(20),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [updated_by] BIGINT
);

-- 源表原样全量装入（行数 = PP_SapManhour；禁止擅自去重改行数）
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z001])),
  LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z002])),
  LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z003])),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z004])), ''), ''),
  COALESCE(TRY_CAST(S.[D_SAP_ZPBLD_Z007] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z008])), ''), ''),
  COALESCE(TRY_CAST(S.[D_SAP_ZPBLD_Z005] AS INT), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z006])), ''), ''),
  -- 精度与实体列一致：rate=3 位、converted_minutes=2 位（误用 4 位会导致永远「有差异」）
  CAST(ROUND(
    CASE
      WHEN S.[D_SAP_ZPBLD_Z004] LIKE '%SMT%' THEN CAST(0.028 AS DECIMAL(18,3))
      WHEN S.[D_SAP_ZPBLD_Z004] LIKE N'%自插%' THEN CAST(0.045 AS DECIMAL(18,3))
      ELSE CAST(1.000 AS DECIMAL(18,3))
    END
  , 3) AS DECIMAL(18,3)) AS [points_to_minutes_rate],
  CAST(ROUND(
    COALESCE(TRY_CAST(S.[D_SAP_ZPBLD_Z005] AS DECIMAL(18,4)), 0) *
    CASE
      WHEN S.[D_SAP_ZPBLD_Z004] LIKE '%SMT%' THEN CAST(0.028 AS DECIMAL(18,3))
      WHEN S.[D_SAP_ZPBLD_Z004] LIKE N'%自插%' THEN CAST(0.045 AS DECIMAL(18,3))
      ELSE CAST(1.000 AS DECIMAL(18,3))
    END
  , 2) AS DECIMAL(18,2)) AS [converted_minutes],
  @tenant_code,
  @company_code,
  '{}',
  '',
  @sync_user_id
FROM (
  SELECT *,
    ROW_NUMBER() OVER (
      ORDER BY [D_SAP_ZPBLD_Z001],[D_SAP_ZPBLD_Z002],[D_SAP_ZPBLD_Z003],[D_SAP_ZPBLD_Z004]
    ) AS rn
  FROM [Sap_Data].[dbo].[PP_SapManhour]
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
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapManhour]);

-- 全量同步：临时表行数必须等于源表行数
IF @source_count <> @sap_raw_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'源行数与装入不一致: source=', @sap_raw_count, N', loaded=', @source_count);
  THROW 50003, @src_msg, 1;
END;

-- 唯一键 = Plant+Material+WorkCenter；源内重复则无法 1:1 落目标表（与唯一索引一致）
IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [material_code], [work_center]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT([plant_code], N' / ', [material_code], N' / ', [work_center], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [material_code], [work_center]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  material_code NVARCHAR(100),
  tenant_code NVARCHAR(20),
  company_code NVARCHAR(20),
  change_by BIGINT,
  operation_desc_old NVARCHAR(200),
  operation_desc_new NVARCHAR(200),
  standard_minutes_old DECIMAL(18,2),
  standard_minutes_new DECIMAL(18,2),
  time_unit_old NVARCHAR(20),
  time_unit_new NVARCHAR(20),
  standard_shorts_old INT,
  standard_shorts_new INT,
  points_unit_old NVARCHAR(20),
  points_unit_new NVARCHAR(20),
  points_to_minutes_rate_old DECIMAL(18,3),
  points_to_minutes_rate_new DECIMAL(18,3),
  converted_minutes_old DECIMAL(18,2),
  converted_minutes_new DECIMAL(18,2),
  is_deleted_old INT,
  is_deleted_new INT,
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

DECLARE @merge_actions TABLE ([action] NVARCHAR(10));

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_standard_operation_time]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);

-- 存在则仅在业务字段变化或需恢复软删时 UPDATE；无变化不写入 #delta（避免「更新=全量」）
SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_manufacturing_bom_standard_operation_time] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
AND LTRIM(RTRIM(T.[work_center])) = S.[work_center]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[operation_desc], N''))) <> LTRIM(RTRIM(ISNULL(S.[operation_desc], N'')))
  OR ROUND(T.[standard_minutes], 2) <> ROUND(S.[standard_minutes], 2)
  OR LTRIM(RTRIM(ISNULL(T.[time_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[time_unit], N'')))
  OR T.[standard_shorts] <> S.[standard_shorts]
  OR LTRIM(RTRIM(ISNULL(T.[points_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[points_unit], N'')))
  OR ROUND(T.[points_to_minutes_rate], 3) <> ROUND(S.[points_to_minutes_rate], 3)
  OR ROUND(T.[converted_minutes], 2) <> ROUND(S.[converted_minutes], 2)
) THEN
  UPDATE SET
  T.[operation_desc]=S.[operation_desc],
  T.[standard_minutes]=S.[standard_minutes],
  T.[time_unit]=S.[time_unit],
  T.[standard_shorts]=S.[standard_shorts],
  T.[points_unit]=S.[points_unit],
  T.[points_to_minutes_rate]=S.[points_to_minutes_rate],
  T.[converted_minutes]=S.[converted_minutes],
  T.[effective_date]=@effective_date,
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=@now,
  T.[approved_by]=T.[created_by],
  T.[approved_at]=T.[created_at],
  T.[approval_status]=2,
  T.[culture_code]=@culture_code,
  T.[is_deleted]=0,
  T.[ext_field]=(
    SELECT CASE WHEN LEN(x.[new_ext]) <= 4000 THEN x.[new_ext] ELSE e0.[base_ext] END
    FROM (SELECT CASE WHEN ISJSON(NULLIF(LTRIM(RTRIM(ISNULL(T.[ext_field], N''))), N'')) = 1 THEN LTRIM(RTRIM(T.[ext_field])) ELSE N'{}' END AS [base_ext]) e0
    CROSS APPLY (SELECT CASE WHEN JSON_QUERY(e0.[base_ext], N'$._sync') IS NULL THEN JSON_MODIFY(e0.[base_ext], N'lax $._sync', JSON_QUERY(N'{}')) ELSE e0.[base_ext] END AS [with_root]) e1
    CROSS APPLY (SELECT CASE
      WHEN JSON_QUERY(e1.[with_root], N'$._sync.st') IS NULL THEN JSON_MODIFY(e1.[with_root], N'lax $._sync.st', JSON_QUERY(N'[]'))
      WHEN LEFT(LTRIM(ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.st'), N'')), 1) = N'[' THEN e1.[with_root]
      ELSE JSON_MODIFY(e1.[with_root], N'lax $._sync.st', JSON_QUERY(N'[' + ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.st'), N'{}') + N']'))
    END AS [with_arr]) e2
    CROSS APPLY (SELECT JSON_MODIFY(e2.[with_arr], N'append $._sync.st', JSON_QUERY(CONCAT(
      N'{"at":"', CONVERT(VARCHAR(19), @now, 126), N'"',
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[operation_desc], N''))) <> LTRIM(RTRIM(ISNULL(S.[operation_desc], N''))) THEN CONCAT(N',"operation_desc":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[operation_desc], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[operation_desc], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ROUND(T.[standard_minutes], 2) <> ROUND(S.[standard_minutes], 2) THEN CONCAT(N',"standard_minutes":{"o":', CONVERT(VARCHAR(40), ROUND(T.[standard_minutes], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[standard_minutes], 2)), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[time_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[time_unit], N''))) THEN CONCAT(N',"time_unit":{"o":"', REPLACE(ISNULL(T.[time_unit], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[time_unit], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN T.[standard_shorts] <> S.[standard_shorts] THEN CONCAT(N',"standard_shorts":{"o":', CONVERT(VARCHAR(20), T.[standard_shorts]), N',"n":', CONVERT(VARCHAR(20), S.[standard_shorts]), N'}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[points_unit], N''))) <> LTRIM(RTRIM(ISNULL(S.[points_unit], N''))) THEN CONCAT(N',"points_unit":{"o":"', REPLACE(ISNULL(T.[points_unit], N''), N'"', N'\"'), N'","n":"', REPLACE(ISNULL(S.[points_unit], N''), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN ROUND(T.[points_to_minutes_rate], 3) <> ROUND(S.[points_to_minutes_rate], 3) THEN CONCAT(N',"points_to_minutes_rate":{"o":', CONVERT(VARCHAR(40), ROUND(T.[points_to_minutes_rate], 3)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[points_to_minutes_rate], 3)), N'}') ELSE N'' END,
      CASE WHEN ROUND(T.[converted_minutes], 2) <> ROUND(S.[converted_minutes], 2) THEN CONCAT(N',"converted_minutes":{"o":', CONVERT(VARCHAR(40), ROUND(T.[converted_minutes], 2)), N',"n":', CONVERT(VARCHAR(40), ROUND(S.[converted_minutes], 2)), N'}') ELSE N'' END,
      CASE WHEN ISNULL(T.[is_deleted], 0) <> 0 THEN N',"is_deleted":{"o":1,"n":0}' ELSE N'' END,
      N'}'))) AS [new_ext]) x
  )
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[material_code],[work_center],[operation_desc],
    [standard_minutes],[time_unit],[standard_shorts],[points_unit],
    [points_to_minutes_rate],[converted_minutes],
    [effective_date],[expiry_date],
    [tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [approved_by],[approved_at],[approval_status],
    [is_deleted]
  )
  VALUES (
    S.[id],S.[plant_code],S.[material_code],S.[work_center],S.[operation_desc],
    S.[standard_minutes],S.[time_unit],S.[standard_shorts],S.[points_unit],
    S.[points_to_minutes_rate],S.[converted_minutes],
    @effective_date,'9999-12-31',
    S.[tenant_code],S.[company_code],@culture_code,S.[ext_field],S.[remark],
    S.[updated_by],@now,S.[updated_by],@now,
    S.[updated_by],@now,2,
    0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[material_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[operation_desc], INSERTED.[operation_desc],
  DELETED.[standard_minutes], INSERTED.[standard_minutes],
  DELETED.[time_unit], INSERTED.[time_unit],
  DELETED.[standard_shorts], INSERTED.[standard_shorts],
  DELETED.[points_unit], INSERTED.[points_unit],
  DELETED.[points_to_minutes_rate], INSERTED.[points_to_minutes_rate],
  DELETED.[converted_minutes], INSERTED.[converted_minutes],
  DELETED.[is_deleted], INSERTED.[is_deleted],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #delta(
  rn, oper_type, id, material_code, tenant_code, company_code, change_by,
  operation_desc_old, operation_desc_new,
  standard_minutes_old, standard_minutes_new,
  time_unit_old, time_unit_new,
  standard_shorts_old, standard_shorts_new,
  points_unit_old, points_unit_new,
  points_to_minutes_rate_old, points_to_minutes_rate_new,
  converted_minutes_old, converted_minutes_new,
  is_deleted_old, is_deleted_new,
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
  [material_code] NVARCHAR(100),
  [work_center] NVARCHAR(100)
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
  INSERTED.[material_code],
  INSERTED.[work_center]
INTO #soft_deleted_rows ([id], [plant_code], [material_code], [work_center])
FROM [takt_logistics_manufacturing_bom_standard_operation_time] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[company_code] = @company_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[material_code] = LTRIM(RTRIM(T.[material_code]))
      AND S.[work_center] = LTRIM(RTRIM(T.[work_center]))
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
    ISNULL([material_code], N''), N'/',
    ISNULL([work_center], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_standard_operation_time]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_standard_operation_time]
  WHERE [tenant_code] = @tenant_code
    AND [company_code] = @company_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_standard_operation_time]
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
-- 未变 = 源行 - 真正新增 - 真正更新（无变化的 MATCHED 未进入 #delta）
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
  N'takt_logistics_manufacturing_bom_standard_operation_time',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE Manhour Sync (summary)',
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
  N'工时管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/manhour',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

INSERT INTO @merge_actions SELECT oper_type FROM #delta;

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
