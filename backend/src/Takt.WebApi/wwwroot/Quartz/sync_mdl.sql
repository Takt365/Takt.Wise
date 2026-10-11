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
DECLARE @base_epoch BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', GETUTCDATE()) * 1000;
-- 更新履历：ext_field._sync.mdl[] 追加 { at, 变更列:{o,n} }；仅差异；超 nvarchar(4000) 保留原 JSON

IF OBJECT_ID('tempdb..#cjs_source') IS NOT NULL DROP TABLE #cjs_source;
CREATE TABLE #cjs_source (
  [rn] INT,
  [material_code] NVARCHAR(100),
  [model_code] NVARCHAR(100),
  [destination_code] NVARCHAR(100)
);

-- 源表装入后按业务唯一键 Tenant+Material+Model 去重（Destination 仅业务字段）
-- 描述不在此阶段匹配：先按 Material+Model 同步，MERGE 后再回填
INSERT INTO #cjs_source ([rn], [material_code], [model_code], [destination_code])
SELECT
  S.rn,
  S.[material_code],
  S.[model_code],
  S.[destination_code]
FROM (
  SELECT
    N.[material_code],
    N.[model_code],
    N.[destination_code],
    ROW_NUMBER() OVER (
      ORDER BY N.[material_code], N.[model_code]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[D_SAP_DEST_Z001])) AS [material_code],
      LTRIM(RTRIM(R.[D_SAP_DEST_Z002])) AS [model_code],
      LTRIM(RTRIM(R.[D_SAP_DEST_Z003])) AS [destination_code],
      ROW_NUMBER() OVER (
        PARTITION BY
          LTRIM(RTRIM(R.[D_SAP_DEST_Z001])),
          LTRIM(RTRIM(R.[D_SAP_DEST_Z002]))
        ORDER BY
          CASE WHEN LTRIM(RTRIM(ISNULL(R.[D_SAP_DEST_Z003], N''))) = N'' THEN 1 ELSE 0 END,
          LTRIM(RTRIM(R.[D_SAP_DEST_Z003]))
      ) AS dup_rn
    FROM [Sap_Data].[dbo].[PP_SapModelDest] R
    WHERE LTRIM(RTRIM(ISNULL(R.[D_SAP_DEST_Z001], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[D_SAP_DEST_Z002], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #cjs_source);
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
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapModelDest]);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[D_SAP_DEST_Z001])) AS [material_code],
      LTRIM(RTRIM(R.[D_SAP_DEST_Z002])) AS [model_code]
    FROM [Sap_Data].[dbo].[PP_SapModelDest] R
    WHERE LTRIM(RTRIM(ISNULL(R.[D_SAP_DEST_Z001], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[D_SAP_DEST_Z002], N''))) <> N''
    GROUP BY
      LTRIM(RTRIM(R.[D_SAP_DEST_Z001])),
      LTRIM(RTRIM(R.[D_SAP_DEST_Z002]))
  ) K
);

IF @batch_size = 0 AND @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count);
  THROW 50003, @src_msg, 1;
END;

-- 业务唯一键 = Material+Model（与 Tenant+Material+Model 一致）
IF EXISTS (
  SELECT 1
  FROM #cjs_source
  GROUP BY [material_code], [model_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT([material_code], N' / ', [model_code], N' x', COUNT(*))
  FROM #cjs_source
  GROUP BY [material_code], [model_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#cjs_delta') IS NOT NULL DROP TABLE #cjs_delta;
CREATE TABLE #cjs_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  tenant_code NVARCHAR(20),
  company_code NVARCHAR(20),
  change_by BIGINT,
  sort_order_old INT,
  sort_order_new INT,
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);

-- 存在则仅在仕向地/软删/默认字段变化时 UPDATE；描述不在 MERGE 内处理（见后置回填）
SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #cjs_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_materials_model_destination] AS T
USING (
  SELECT
    @base_epoch + S.[rn] AS [id],
    S.[rn],
    S.[material_code],
    S.[model_code],
    S.[destination_code]
  FROM #cjs_source S
  WHERE S.[rn] >= @merge_from_rn AND S.[rn] <= @merge_to_rn
) AS S
ON T.[tenant_code] = @tenant_code
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
AND LTRIM(RTRIM(T.[model_code])) = S.[model_code]
WHEN MATCHED AND (
  T.[is_deleted] <> 0
  OR T.[sort_order] <> 0
  OR LTRIM(RTRIM(ISNULL(T.[destination_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[destination_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[remark], N''))) <> N'幂等更新'
) THEN
  UPDATE SET
  T.[destination_code]=S.[destination_code],
  T.[sort_order]=0,
  T.[remark]=N'幂等更新',
  T.[updated_by]=@sync_user_id,
  T.[updated_at]=@now,
  T.[is_deleted]=0,
  T.[ext_field]=(
    SELECT CASE WHEN LEN(x.[new_ext]) <= 4000 THEN x.[new_ext] ELSE e0.[base_ext] END
    FROM (SELECT CASE WHEN ISJSON(NULLIF(LTRIM(RTRIM(ISNULL(T.[ext_field], N''))), N'')) = 1 THEN LTRIM(RTRIM(T.[ext_field])) ELSE N'{}' END AS [base_ext]) e0
    CROSS APPLY (SELECT CASE WHEN JSON_QUERY(e0.[base_ext], N'$._sync') IS NULL THEN JSON_MODIFY(e0.[base_ext], N'lax $._sync', JSON_QUERY(N'{}')) ELSE e0.[base_ext] END AS [with_root]) e1
    CROSS APPLY (SELECT CASE
      WHEN JSON_QUERY(e1.[with_root], N'$._sync.mdl') IS NULL THEN JSON_MODIFY(e1.[with_root], N'lax $._sync.mdl', JSON_QUERY(N'[]'))
      WHEN LEFT(LTRIM(ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.mdl'), N'')), 1) = N'[' THEN e1.[with_root]
      ELSE JSON_MODIFY(e1.[with_root], N'lax $._sync.mdl', JSON_QUERY(N'[' + ISNULL(JSON_QUERY(e1.[with_root], N'$._sync.mdl'), N'{}') + N']'))
    END AS [with_arr]) e2
    CROSS APPLY (SELECT JSON_MODIFY(e2.[with_arr], N'append $._sync.mdl', JSON_QUERY(CONCAT(
      N'{"at":"', CONVERT(VARCHAR(19), @now, 126), N'"',
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[destination_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[destination_code], N''))) THEN CONCAT(N',"destination_code":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[destination_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(S.[destination_code], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'"}') ELSE N'' END,
      CASE WHEN T.[sort_order] <> 0 THEN CONCAT(N',"sort_order":{"o":', CONVERT(VARCHAR(20), T.[sort_order]), N',"n":0}') ELSE N'' END,
      CASE WHEN LTRIM(RTRIM(ISNULL(T.[remark], N''))) <> N'幂等更新' THEN CONCAT(N',"remark":{"o":"', REPLACE(REPLACE(ISNULL(LEFT(LTRIM(RTRIM(ISNULL(T.[remark], N''))), 80), N''), N'\', N'\\'), N'"', N'\"'), N'","n":"幂等更新"}') ELSE N'' END,
      CASE WHEN ISNULL(T.[is_deleted], 0) <> 0 THEN N',"is_deleted":{"o":1,"n":0}' ELSE N'' END,
      N'}'))) AS [new_ext]) x
  )
WHEN NOT MATCHED THEN
  INSERT (
    [id],
    [material_code],
    [material_description],
    [model_code],
    [model_name],
    [destination_code],
    [destination_name],
    [sort_order],
    [tenant_code],
    [ext_field],
    [remark],
    [created_by],
    [created_at],
    [updated_by],
    [updated_at],
    [is_deleted]
  )
  VALUES (
    S.[id],
    S.[material_code],
    N'',
    S.[model_code],
    '',
    S.[destination_code],
    '',
    0,
    @tenant_code,
    '{}',
    N'幂等更新',
    @sync_user_id,
    @now,
    @sync_user_id,
    @now,
    0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  @tenant_code,
  @company_code,
  @sync_user_id,
  DELETED.[sort_order], INSERTED.[sort_order],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #cjs_delta(
  rn, oper_type, id,
  tenant_code, company_code, change_by,
  sort_order_old, sort_order_new,
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
  [material_code] NVARCHAR(100),
  [model_code] NVARCHAR(100)
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
  INSERTED.[material_code],
  INSERTED.[model_code]
INTO #soft_deleted_rows ([id], [material_code], [model_code])
FROM [takt_logistics_materials_model_destination] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #cjs_source S
    WHERE S.[material_code] = LTRIM(RTRIM(T.[material_code]))
      AND S.[model_code] = LTRIM(RTRIM(T.[model_code]))
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
    ISNULL([material_code], N''), N'/',
    ISNULL([model_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');

-- 同步完成后回填（两字段语言切勿混用）：
--   MaterialDescription ← material_code + culture_code=ja-JP（Length=40）
--   ModelName ← model_code + culture_code=Z1（Length=80）
DECLARE @desc_backfill_count INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) T
SET
  T.[material_description] = LEFT(LTRIM(RTRIM(D.[material_description])), 40),
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
FROM [takt_logistics_materials_model_destination] T
INNER JOIN [takt_logistics_materials_material_description] D
  ON D.[tenant_code] = @tenant_code
  AND D.[is_deleted] = 0
  AND LTRIM(RTRIM(D.[material_code])) = LTRIM(RTRIM(T.[material_code]))
  AND D.[culture_code] = N'ja-JP'
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND LTRIM(RTRIM(ISNULL(D.[material_description], N''))) <> N''
  AND LTRIM(RTRIM(ISNULL(T.[material_description], N'')))
    <> LEFT(LTRIM(RTRIM(D.[material_description])), 40);
  SET @dml_n = @@ROWCOUNT;
  SET @desc_backfill_count = @desc_backfill_count + @dml_n;
END

DECLARE @model_name_backfill_count INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) T
SET
  T.[model_name] = LEFT(LTRIM(RTRIM(D.[material_description])), 80),
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
FROM [takt_logistics_materials_model_destination] T
INNER JOIN [takt_logistics_materials_material_description] D
  ON D.[tenant_code] = @tenant_code
  AND D.[is_deleted] = 0
  AND LTRIM(RTRIM(D.[material_code])) = LTRIM(RTRIM(T.[model_code]))
  AND D.[culture_code] = N'Z1'
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND LTRIM(RTRIM(ISNULL(D.[material_description], N''))) <> N''
  AND LTRIM(RTRIM(ISNULL(T.[model_name], N'')))
    <> LEFT(LTRIM(RTRIM(D.[material_description])), 80);
  SET @dml_n = @@ROWCOUNT;
  SET @model_name_backfill_count = @model_name_backfill_count + @dml_n;
END

DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_model_destination]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 1
);

IF @target_count <> @source_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(
    N'有效行数不一致: source=', @source_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;


/* per-row delta_log removed */


DECLARE @insert_count INT = (SELECT COUNT(*) FROM #cjs_delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #cjs_delta WHERE oper_type = 'UPDATE');
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
  + N',"desc_backfill":' + CAST(@desc_backfill_count AS NVARCHAR)
  + N',"model_name_backfill":' + CAST(@model_name_backfill_count AS NVARCHAR)
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
  @base_epoch,
  N'SYNC',
  N'takt_logistics_materials_model_destination',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE Model Destination Sync (summary)',
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
  @base_epoch + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'机种-目的地管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/model_destination',
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
  @desc_backfill_count AS [desc_backfill_count],
  @model_name_backfill_count AS [model_name_backfill_count],
  @soft_deleted_keys AS [soft_deleted_keys];
