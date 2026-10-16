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
-- 源表 / 目标表：同名 + 列与实体 TaktMaterialGroup 一致（组合 4：仅租户）
-- {{SourceDatabase}}.dbo.takt_logistics_materials_material_group → 当前租户库同名表
-- 业务唯一键：TenantCode + MaterialGroupCode
IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [material_group_code] VARCHAR(6),
  [material_group_name] NVARCHAR(80),
  [sort_order] INT,
  [material_group_description] NVARCHAR(2000),
  [tenant_code] NVARCHAR(3),
  [ext_field] NVARCHAR(4000),
  [remark] NVARCHAR(500),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[material_group_code],
  S.[material_group_name],
  S.[sort_order],
  S.[material_group_description],
  S.[tenant_code],
  S.[ext_field],
  S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[material_group_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[material_group_code], N''))), 6) AS [material_group_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_group_name], N''))), 80), N''), LEFT(LTRIM(RTRIM(ISNULL(R.[material_group_code], N''))), 6)) AS [material_group_name],
      COALESCE(TRY_CAST(R.[sort_order] AS INT), 0) AS [sort_order],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_group_description], N''))), 2000), N'') AS [material_group_description],
      LEFT(ISNULL(R.[ext_field], N'{}'), 4000) AS [ext_field],
      LEFT(ISNULL(R.[remark], N''), 500) AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY LEFT(LTRIM(RTRIM(ISNULL(R.[material_group_code], N''))), 6)
        ORDER BY ISNULL(R.[is_deleted], 0), ISNULL(R.[updated_at], CAST('1900-01-01' AS DATETIME)) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_group] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_group_code], N''))) <> N''
      AND LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) = @tenant_code
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
IF EXISTS (
  SELECT 1 FROM #st_source
  GROUP BY [material_group_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(200);
  SELECT TOP 1 @dup_key = CONCAT([material_group_code], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [material_group_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;
UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_logistics_materials_material_group] T
  ON T.[tenant_code] = S.[tenant_code]
 AND LTRIM(RTRIM(T.[material_group_code])) = S.[material_group_code];
IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT
);
DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_group]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
  MERGE INTO [takt_logistics_materials_material_group] AS T
  USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
  ON T.[tenant_code] = S.[tenant_code]
  AND LTRIM(RTRIM(T.[material_group_code])) = S.[material_group_code]
  WHEN MATCHED AND (
    ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
    OR LTRIM(RTRIM(ISNULL(T.[material_group_name], N''))) <> S.[material_group_name]
    OR T.[sort_order] <> S.[sort_order]
    OR LTRIM(RTRIM(ISNULL(T.[material_group_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_group_description], N'')))
    OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
    OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')
  ) THEN
    UPDATE SET
    T.[material_group_name]=S.[material_group_name],
    T.[sort_order]=S.[sort_order],
    T.[material_group_description]=S.[material_group_description],
    T.[ext_field]=S.[ext_field],
    T.[remark]=S.[remark],
    T.[updated_by]=S.[updated_by],
    T.[updated_at]=S.[updated_at],
    T.[is_deleted]=S.[is_deleted],
    T.[deleted_by]=S.[deleted_by],
    T.[deleted_at]=S.[deleted_at]
  WHEN NOT MATCHED THEN
    INSERT (
      [id],[material_group_code],[material_group_name],[sort_order],[material_group_description],
      [tenant_code],[ext_field],[remark],
      [created_by],[created_at],[updated_by],[updated_at],
      [is_deleted],[deleted_by],[deleted_at]
    )
    VALUES (
      S.[id],S.[material_group_code],S.[material_group_name],S.[sort_order],S.[material_group_description],
      S.[tenant_code],S.[ext_field],S.[remark],
      COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
      S.[is_deleted],S.[deleted_by],S.[deleted_at]
    )
  OUTPUT S.rn, $action, INSERTED.[id]
  INTO #delta(rn, oper_type, id);
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
  FROM [takt_logistics_materials_material_group] T
  WHERE T.[tenant_code] = @tenant_code
    AND T.[is_deleted] = 0
    AND NOT EXISTS (
      SELECT 1 FROM #st_source S
      WHERE S.[material_group_code] = LTRIM(RTRIM(T.[material_group_code]))
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
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_group]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @source_active_count INT = (SELECT COUNT(*) FROM #st_source WHERE [is_deleted] = 0);
IF @target_count <> @source_active_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(N'有效行数不一致: source=', @source_active_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;
DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"source":' + CAST(@source_count AS NVARCHAR)
  + N',"target_before":' + CAST(@target_before AS NVARCHAR)
  + N',"target_after":' + CAST(@target_count AS NVARCHAR)
  + N',"insert":' + CAST(@insert_count AS NVARCHAR)
  + N',"update":' + CAST(@update_count AS NVARCHAR)
  + N',"unchanged":' + CAST(@unchanged_count AS NVARCHAR)
  + N',"soft_delete_this_run":' + CAST(@delete_count AS NVARCHAR) + N'}';
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
  N'takt_logistics_materials_material_group',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE MaterialGroup Sync (summary)',
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
  N'物料组',
  N'exec_sql_merge',
  'SQL',
  N'/sync/material-group',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);
SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @source_count AS [source_raw_count],
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count];
