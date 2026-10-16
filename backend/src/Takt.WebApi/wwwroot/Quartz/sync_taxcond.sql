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

-- 源表 / 目标表：同名 + 列与实体 TaktTaxCondition 一致
-- {{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition → 当前租户库同名表
-- 业务唯一键：TenantCode + CompanyCode + ApplicationCode + CountryCode + TaxCode + ConditionRecordCode
-- tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数

IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'application_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [application_code] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'application_code') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [application_code] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'condition_type_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [condition_type_code] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'condition_type_code') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [condition_type_code] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'country_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [country_code] NVARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'country_code') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [country_code] NVARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'tax_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [tax_code] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'tax_code') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [tax_code] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'condition_record_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [condition_record_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'condition_record_code') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [condition_record_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'sort_order') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [sort_order] INT NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'sort_order') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [sort_order] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_accounting_financial_tax_condition', N'tax_condition_status') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] ADD [tax_condition_status] INT NULL');
IF COL_LENGTH(N'dbo.takt_accounting_financial_tax_condition', N'tax_condition_status') IS NULL
  ALTER TABLE [dbo].[takt_accounting_financial_tax_condition] ADD [tax_condition_status] INT NULL;
IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [application_code] VARCHAR(2),
  [condition_type_code] VARCHAR(4),
  [country_code] NVARCHAR(2),
  [tax_code] VARCHAR(2),
  [condition_record_code] NVARCHAR(10),
  [sort_order] INT,
  [tax_condition_status] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[application_code],
  S.[condition_type_code],
  S.[country_code],
  S.[tax_code],
  S.[condition_record_code],
  S.[sort_order],
  S.[tax_condition_status],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  S.[ext_field],
  S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[company_code], N.[application_code], N.[country_code], N.[tax_code], N.[condition_record_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[application_code], N''))), 2), N''), N'') AS [application_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[condition_type_code], N''))), 4), N''), N'') AS [condition_type_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[country_code], N''))), 2), N''), N'') AS [country_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_code], N''))), 2), N''), N'') AS [tax_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[condition_record_code], N''))), 10), N''), N'') AS [condition_record_code],
      COALESCE(TRY_CAST(R.[sort_order] AS INT), 0) AS [sort_order],
      COALESCE(TRY_CAST(R.[tax_condition_status] AS INT), 1) AS [tax_condition_status],
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
          LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3),
          LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
          LTRIM(RTRIM(ISNULL(R.[application_code], N''))),
          LTRIM(RTRIM(ISNULL(R.[country_code], N''))),
          LTRIM(RTRIM(ISNULL(R.[tax_code], N''))),
          LTRIM(RTRIM(ISNULL(R.[condition_record_code], N'')))
        ORDER BY ISNULL(R.[is_deleted], 0), ISNULL(R.[updated_at], CAST('1900-01-01' AS DATETIME)) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_accounting_financial_tax_condition] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[application_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[country_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tax_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[condition_record_code], N''))) <> N''
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
  GROUP BY [tenant_code], [company_code], [application_code], [country_code], [tax_code], [condition_record_code]
  HAVING COUNT(*) > 1
)
BEGIN
  THROW 50001, N'业务键重复', 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_accounting_financial_tax_condition] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_accounting_financial_tax_condition] T
  ON T.[tenant_code] = S.[tenant_code]
 AND T.[company_code] = S.[company_code]
 AND LTRIM(RTRIM(T.[application_code]))=S.[application_code]
  AND LTRIM(RTRIM(T.[country_code]))=S.[country_code]
  AND LTRIM(RTRIM(T.[tax_code]))=S.[tax_code]
  AND LTRIM(RTRIM(T.[condition_record_code]))=S.[condition_record_code];

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_accounting_financial_tax_condition] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[application_code]))=S.[application_code]
 AND LTRIM(RTRIM(T.[country_code]))=S.[country_code]
 AND LTRIM(RTRIM(T.[tax_code]))=S.[tax_code]
 AND LTRIM(RTRIM(T.[condition_record_code]))=S.[condition_record_code]
WHEN MATCHED AND (
  LTRIM(RTRIM(ISNULL(T.[plant_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[plant_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[application_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[application_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[condition_type_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[condition_type_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[country_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[country_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[condition_record_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[condition_record_code], N'')))
  OR T.[sort_order]<>S.[sort_order]
  OR T.[tax_condition_status]<>S.[tax_condition_status]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))
  OR ISNULL(T.[is_deleted], 0)<>S.[is_deleted]
  OR ISNULL(T.[ext_field], N'')<>ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'')<>ISNULL(S.[remark], N'')
) THEN
  UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[application_code]=S.[application_code],
  T.[condition_type_code]=S.[condition_type_code],
  T.[country_code]=S.[country_code],
  T.[tax_code]=S.[tax_code],
  T.[condition_record_code]=S.[condition_record_code],
  T.[sort_order]=S.[sort_order],
  T.[tax_condition_status]=S.[tax_condition_status],
  T.[culture_code]=S.[culture_code],
  T.[ext_field]=S.[ext_field],
  T.[remark]=S.[remark],
  T.[created_by]=S.[created_by],
  T.[created_at]=COALESCE(S.[created_at], T.[created_at], @now),
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[application_code],[condition_type_code],[country_code],[tax_code],[condition_record_code],[sort_order],[tax_condition_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[application_code],S.[condition_type_code],S.[country_code],S.[tax_code],S.[condition_record_code],S.[sort_order],S.[tax_condition_status],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
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
  FROM [takt_accounting_financial_tax_condition] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
    AND NOT EXISTS (
      SELECT 1 FROM #st_source S
      WHERE S.[tenant_code] = T.[tenant_code]
        AND S.[company_code] = T.[company_code]
        AND S.[application_code] = LTRIM(RTRIM(T.[application_code]))
        AND S.[country_code] = LTRIM(RTRIM(T.[country_code]))
        AND S.[tax_code] = LTRIM(RTRIM(T.[tax_code]))
        AND S.[condition_record_code] = LTRIM(RTRIM(T.[condition_record_code]))
    );
  SET @dml_n = @@ROWCOUNT;
  SET @delete_count = @delete_count + @dml_n;
END

DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_accounting_financial_tax_condition] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
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
  N'takt_accounting_financial_tax_condition',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE TaktTaxCondition Sync (summary)',
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
  N'税务条件',
  N'exec_sql_merge',
  'SQL',
  N'/sync/tax-condition',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count];
