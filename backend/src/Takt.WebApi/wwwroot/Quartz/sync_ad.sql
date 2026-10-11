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

-- 源表 / 目标表：同名 + 列与实体 TaktAdminDivision 业务字段一致（含 id/parent_id 树关系）
-- {{SourceDatabase}}.dbo.takt_foundation_admin_division → 当前租户库同名表
-- 业务唯一键：Tenant+DivisionCode（租户级，无 company_code）
-- 同步后按 DivisionCode 重映射 parent_id，并按 Level 重建 division_path / is_leaf

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT NOT NULL,
  [provisional_id] BIGINT NOT NULL,
  [id] BIGINT NOT NULL,
  [source_id] BIGINT NOT NULL,
  [source_parent_id] BIGINT NOT NULL,
  [parent_division_code] NVARCHAR(40) NOT NULL,
  [country_code] NVARCHAR(2) NOT NULL,
  [division_code] NVARCHAR(40) NOT NULL,
  [division_name] NVARCHAR(200) NOT NULL,
  [parent_id] BIGINT NOT NULL,
  [level] INT NOT NULL,
  [division_path] NVARCHAR(500) NOT NULL,
  [is_leaf] INT NOT NULL,
  [postal_code] NVARCHAR(20) NULL,
  [currency_code] VARCHAR(3) NOT NULL,
  [phone_code] VARCHAR(16) NOT NULL,
  [is_built_in] INT NOT NULL,
  [sort_order] INT NOT NULL,
  [division_status] INT NOT NULL,
  [tenant_code] NVARCHAR(3) NOT NULL,
  [ext_field] NVARCHAR(4000) NULL,
  [remark] NVARCHAR(500) NULL,
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT NOT NULL);

INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  @base_id + S.rn,
  S.[source_id],
  S.[source_parent_id],
  ISNULL(S.[parent_division_code], N''),
  S.[country_code],
  S.[division_code],
  S.[division_name],
  0,
  S.[level],
  N'',
  S.[is_leaf],
  S.[postal_code],
  S.[currency_code],
  S.[phone_code],
  S.[is_built_in],
  S.[sort_order],
  S.[division_status],
  S.[tenant_code],
  S.[ext_field],
  S.[remark],
    S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY N.[level], N.[sort_order], N.[division_code]
    ) AS rn
  FROM (
    SELECT
      COALESCE(TRY_CAST(R.[id] AS BIGINT), 0) AS [source_id],
      COALESCE(TRY_CAST(R.[parent_id] AS BIGINT), 0) AS [source_parent_id],
      ISNULL(NULLIF(LTRIM(RTRIM(P.[division_code])), N''), N'') AS [parent_division_code],
      UPPER(LTRIM(RTRIM(ISNULL(R.[country_code], N'')))) AS [country_code],
      LTRIM(RTRIM(ISNULL(R.[division_code], N''))) AS [division_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[division_name])), N''), N'') AS [division_name],
      COALESCE(TRY_CAST(R.[level] AS INT), 1) AS [level],
      COALESCE(TRY_CAST(R.[is_leaf] AS INT), 0) AS [is_leaf],
      NULLIF(LTRIM(RTRIM(R.[postal_code])), N'') AS [postal_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), N''), N'') AS [currency_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[phone_code])), N''), N'') AS [phone_code],
      COALESCE(TRY_CAST(R.[is_built_in] AS INT), 0) AS [is_built_in],
      COALESCE(TRY_CAST(R.[sort_order] AS INT), 0) AS [sort_order],
      COALESCE(TRY_CAST(R.[division_status] AS INT), 1) AS [division_status],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted]
    FROM [{{SourceDatabase}}].[dbo].[takt_foundation_admin_division] R
    LEFT JOIN [{{SourceDatabase}}].[dbo].[takt_foundation_admin_division] P
      ON P.[id] = R.[parent_id]
     AND COALESCE(TRY_CAST(R.[parent_id] AS BIGINT), 0) <> 0
    WHERE LTRIM(RTRIM(ISNULL(R.[division_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
  ) N
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
DECLARE @sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_foundation_admin_division] R
  WHERE LTRIM(RTRIM(ISNULL(R.[division_code], N''))) <> N''
);

IF @source_count <> @sap_raw_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'源行数与装入不一致: source=', @sap_raw_count, N', loaded=', @source_count);
  THROW 50003, @src_msg, 1;
END;

IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [division_code]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT([division_code], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [division_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF EXISTS (
  SELECT 1
  FROM #st_source S
  WHERE S.[level] > 1
    AND (
      S.[parent_division_code] = N''
      OR NOT EXISTS (
        SELECT 1 FROM #st_source P WHERE P.[division_code] = S.[parent_division_code]
      )
    )
)
BEGIN
  DECLARE @orphan NVARCHAR(400);
  SELECT TOP 1
    @orphan = CONCAT(S.[division_code], N' parent=', ISNULL(S.[parent_division_code], N''), N' level=', S.[level])
  FROM #st_source S
  WHERE S.[level] > 1
    AND (
      S.[parent_division_code] = N''
      OR NOT EXISTS (
        SELECT 1 FROM #st_source P WHERE P.[division_code] = S.[parent_division_code]
      )
    );
  THROW 50004, @orphan, 1;
END;

-- 复用租户库已有 Id（含软删行），新行用 provisional_id
UPDATE S
SET S.[id] = COALESCE(T.[id], S.[provisional_id])
FROM #st_source S
LEFT JOIN [takt_foundation_admin_division] T
  ON T.[tenant_code] = @tenant_code
 AND LTRIM(RTRIM(T.[division_code])) = S.[division_code];

-- 父级 Id：根=0；其余按 parent_division_code 映射到同步集内 Id
UPDATE S
SET S.[parent_id] = CASE
  WHEN S.[level] <= 1 OR S.[parent_division_code] = N'' THEN 0
  ELSE COALESCE(P.[id], 0)
END
FROM #st_source S
LEFT JOIN #st_source P
  ON P.[division_code] = S.[parent_division_code];

-- 按 Level 重建 division_path（依赖父节点 path 已就绪）
DECLARE @lv INT = 1;
DECLARE @max_lv INT = (SELECT ISNULL(MAX([level]), 1) FROM #st_source);
WHILE @lv <= @max_lv
BEGIN
  IF @lv = 1
  BEGIN
    UPDATE S
    SET S.[division_path] = CONCAT(N'/', CAST(S.[id] AS NVARCHAR(30)), N'/')
    FROM #st_source S
    WHERE S.[level] = 1;
  END
  ELSE
  BEGIN
    UPDATE S
    SET S.[division_path] = CONCAT(P.[division_path], CAST(S.[id] AS NVARCHAR(30)), N'/')
    FROM #st_source S
    INNER JOIN #st_source P ON P.[id] = S.[parent_id]
    WHERE S.[level] = @lv;
  END
  SET @lv = @lv + 1;
END;

UPDATE S
SET S.[is_leaf] = CASE
  WHEN EXISTS (SELECT 1 FROM #st_source C WHERE C.[parent_id] = S.[id]) THEN 0
  ELSE 1
END
FROM #st_source S;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  division_code NVARCHAR(40),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  country_code_old NVARCHAR(2),
  country_code_new NVARCHAR(2),
  division_name_old NVARCHAR(200),
  division_name_new NVARCHAR(200),
  parent_id_old BIGINT,
  parent_id_new BIGINT,
  level_old INT,
  level_new INT,
  division_path_old NVARCHAR(500),
  division_path_new NVARCHAR(500),
  is_leaf_old INT,
  is_leaf_new INT,
  postal_code_old NVARCHAR(20),
  postal_code_new NVARCHAR(20),
  currency_code_old VARCHAR(3),
  currency_code_new VARCHAR(3),
  phone_code_old VARCHAR(16),
  phone_code_new VARCHAR(16),
  is_built_in_old INT,
  is_built_in_new INT,
  sort_order_old INT,
  sort_order_new INT,
  division_status_old INT,
  division_status_new INT
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_foundation_admin_division]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_foundation_admin_division] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND LTRIM(RTRIM(T.[division_code])) = S.[division_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[country_code], N''))) <> S.[country_code]
  OR LTRIM(RTRIM(ISNULL(T.[division_name], N''))) <> S.[division_name]
  OR T.[parent_id] <> S.[parent_id]
  OR T.[level] <> S.[level]
  OR LTRIM(RTRIM(ISNULL(T.[division_path], N''))) <> S.[division_path]
  OR T.[is_leaf] <> S.[is_leaf]
  OR LTRIM(RTRIM(ISNULL(T.[postal_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[postal_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> S.[currency_code]
  OR LTRIM(RTRIM(ISNULL(T.[phone_code], N''))) <> S.[phone_code]
  OR T.[is_built_in] <> S.[is_built_in]
  OR T.[sort_order] <> S.[sort_order]
  OR T.[division_status] <> S.[division_status]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[country_code]=S.[country_code],
  T.[division_name]=S.[division_name],
  T.[parent_id]=S.[parent_id],
  T.[level]=S.[level],
  T.[division_path]=S.[division_path],
  T.[is_leaf]=S.[is_leaf],
  T.[postal_code]=S.[postal_code],
  T.[currency_code]=S.[currency_code],
  T.[phone_code]=S.[phone_code],
  T.[is_built_in]=S.[is_built_in],
  T.[sort_order]=S.[sort_order],
  T.[division_status]=S.[division_status],
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
    [id],[country_code],[division_code],[division_name],[parent_id],[level],
    [division_path],[is_leaf],[postal_code],[currency_code],[phone_code],
    [is_built_in],[sort_order],[division_status],
    [tenant_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[country_code],S.[division_code],S.[division_name],S.[parent_id],S.[level],
    S.[division_path],S.[is_leaf],S.[postal_code],S.[currency_code],S.[phone_code],
    S.[is_built_in],S.[sort_order],S.[division_status],S.[tenant_code],S.[ext_field],S.[remark],
    COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
    S.[is_deleted],
    S.[deleted_by],S.[deleted_at]
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[division_code],
  INSERTED.[tenant_code],
  @company_code,
  INSERTED.[updated_by],
  DELETED.[country_code], INSERTED.[country_code],
  DELETED.[division_name], INSERTED.[division_name],
  DELETED.[parent_id], INSERTED.[parent_id],
  DELETED.[level], INSERTED.[level],
  DELETED.[division_path], INSERTED.[division_path],
  DELETED.[is_leaf], INSERTED.[is_leaf],
  DELETED.[postal_code], INSERTED.[postal_code],
  DELETED.[currency_code], INSERTED.[currency_code],
  DELETED.[phone_code], INSERTED.[phone_code],
  DELETED.[is_built_in], INSERTED.[is_built_in],
  DELETED.[sort_order], INSERTED.[sort_order],
  DELETED.[division_status], INSERTED.[division_status]
INTO #delta(
  rn, oper_type, id, division_code, tenant_code, company_code, change_by,
  country_code_old, country_code_new,
  division_name_old, division_name_new,
  parent_id_old, parent_id_new,
  level_old, level_new,
  division_path_old, division_path_new,
  is_leaf_old, is_leaf_new,
  postal_code_old, postal_code_new,
  currency_code_old, currency_code_new,
  phone_code_old, phone_code_new,
  is_built_in_old, is_built_in_new,
  sort_order_old, sort_order_new,
  division_status_old, division_status_new
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
  [division_code] NVARCHAR(40)
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
  INSERTED.[division_code]
INTO #soft_deleted_rows ([id], [division_code])
FROM [takt_foundation_admin_division] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[division_code] = LTRIM(RTRIM(T.[division_code]))
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
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([division_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');

DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_foundation_admin_division]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @source_active_count INT = (
  SELECT COUNT(*)
  FROM #st_source
  WHERE [is_deleted] = 0
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_foundation_admin_division]
  WHERE [tenant_code] = @tenant_code
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_foundation_admin_division]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 1
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
  N'takt_foundation_admin_division',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE AdminDivision Sync (summary)',
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
  N'行政区划',
  N'exec_sql_merge',
  'SQL',
  N'/sync/admin-division',
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
