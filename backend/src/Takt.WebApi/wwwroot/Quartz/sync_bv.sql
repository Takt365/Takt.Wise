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

-- 源表 / 目标表：同名 + 业务字段源表原样写入（❌ 禁止他表回填、禁止默认 FERT、禁止发明机种月均）
-- {{SourceDatabase}}.dbo.takt_logistics_manufacturing_bom_material_cost → 当前租户库同名表
-- 业务唯一键：Plant+Product+CostingDate（日；ModelCode/CostingPeriod 仅业务字段，不参与匹配）
-- 流程：①源表装入（含 is_deleted 原样）→ ②唯一键去重 → MERGE（源有什么写什么）→ ③目标有源无则软删

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [model_code] NVARCHAR(40),
  [model_monthly_average_cost] DECIMAL(18,5),
  [material_type] NVARCHAR(4),
  [product_code] NVARCHAR(20),
  [product_description] NVARCHAR(40),
  [product_monthly_cost] DECIMAL(18,5),
  [product_monthly_calculation] DECIMAL(18,5),
  [latest_purchase_cost] DECIMAL(18,5),
  [currency_code] NVARCHAR(3),
  [costing_period] NVARCHAR(7),
  [costing_date] DATETIME,
  [is_deleted] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] BIGINT,
  [created_at] DATETIME,
  [updated_by] BIGINT,
  [updated_at] DATETIME,
  [deleted_by] BIGINT,
  [deleted_at] DATETIME
);

IF OBJECT_ID('tempdb..#st_raw') IS NOT NULL DROP TABLE #st_raw;
CREATE TABLE #st_raw (
  [plant_code] NVARCHAR(4),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [model_code] NVARCHAR(40),
  [model_monthly_average_cost] DECIMAL(18,5),
  [material_type] NVARCHAR(4),
  [product_code] NVARCHAR(20),
  [product_description] NVARCHAR(40),
  [product_monthly_cost] DECIMAL(18,5),
  [product_monthly_calculation] DECIMAL(18,5),
  [latest_purchase_cost] DECIMAL(18,5),
  [currency_code] NVARCHAR(3),
  [costing_period] NVARCHAR(7),
  [costing_date] DATETIME,
  [is_deleted] INT,
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] BIGINT,
  [created_at] DATETIME,
  [updated_by] BIGINT,
  [updated_at] DATETIME,
  [deleted_by] BIGINT,
  [deleted_at] DATETIME
);

-- 源表原样装入（仅 trim / 18 位纯数字产品码截末 10 / 小数位对齐；❌ 禁止他表回填与默认码）
INSERT INTO #st_raw (
  [plant_code],[tenant_code],[company_code],[culture_code],
  [model_code],[model_monthly_average_cost],[material_type],[product_code],[product_description],
  [product_monthly_cost],[product_monthly_calculation],[latest_purchase_cost],[currency_code],[costing_period],[costing_date],
  [is_deleted],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[deleted_by],[deleted_at]
)
SELECT
  LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
  LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
  LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
  LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
  ISNULL(NULLIF(LTRIM(RTRIM(R.[model_code])), N''), N'') AS [model_code],
  ROUND(COALESCE(TRY_CAST(R.[model_monthly_average_cost] AS DECIMAL(18,8)), 0), 5) AS [model_monthly_average_cost],
  ISNULL(NULLIF(LTRIM(RTRIM(R.[material_type])), N''), N'') AS [material_type],
  CASE
    WHEN LEN(LTRIM(RTRIM(R.[product_code]))) = 18
      AND LTRIM(RTRIM(R.[product_code])) NOT LIKE '%[^0-9]%'
    THEN RIGHT(LTRIM(RTRIM(R.[product_code])), 10)
    ELSE LTRIM(RTRIM(R.[product_code]))
  END AS [product_code],
  ISNULL(NULLIF(LTRIM(RTRIM(R.[product_description])), N''), N'') AS [product_description],
  ROUND(COALESCE(TRY_CAST(R.[product_monthly_cost] AS DECIMAL(18,8)), 0), 5) AS [product_monthly_cost],
  ROUND(COALESCE(TRY_CAST(R.[product_monthly_calculation] AS DECIMAL(18,8)), 0), 5) AS [product_monthly_calculation],
  ROUND(COALESCE(TRY_CAST(R.[latest_purchase_cost] AS DECIMAL(18,8)), 0), 5) AS [latest_purchase_cost],
  ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), N''), N'') AS [currency_code],
  LTRIM(RTRIM(R.[costing_period])) AS [costing_period],
  CAST(CAST(TRY_CAST(R.[costing_date] AS DATETIME) AS DATE) AS DATETIME) AS [costing_date],
  CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
  ISNULL(R.[ext_field], N'{}') AS [ext_field],
  ISNULL(R.[remark], N'') AS [remark],
  COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
  R.[created_at] AS [created_at],
  TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
  R.[updated_at] AS [updated_at],
  TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
  R.[deleted_at] AS [deleted_at]
FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost] R
WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  AND LTRIM(RTRIM(ISNULL(R.[product_code], N''))) <> N''
  AND LTRIM(RTRIM(ISNULL(R.[costing_period], N''))) <> N''
  AND TRY_CAST(R.[costing_date] AS DATETIME) IS NOT NULL;

INSERT INTO #st_source (
  [rn],[id],[plant_code],[model_code],[model_monthly_average_cost],[material_type],
  [product_code],[product_description],[product_monthly_cost],[product_monthly_calculation],[latest_purchase_cost],
  [currency_code],[costing_period],[costing_date],[is_deleted],
  [tenant_code],[company_code],[culture_code],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[deleted_by],[deleted_at]
)
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[model_code],
  S.[model_monthly_average_cost],
  S.[material_type],
  S.[product_code],
  S.[product_description],
  S.[product_monthly_cost],
  S.[product_monthly_calculation],
  S.[latest_purchase_cost],
  S.[currency_code],
  S.[costing_period],
  S.[costing_date],
  S.[is_deleted],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  S.[ext_field],
  S.[remark],
  S.[created_by],
  S.[created_at],
  S.[updated_by],
  S.[updated_at],
  S.[deleted_by],
  S.[deleted_at]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (
      ORDER BY
        N.[plant_code], N.[product_code], N.[costing_date]
    ) AS rn
  FROM (
    SELECT
      R.[plant_code],
      R.[tenant_code],
      R.[company_code],
      R.[culture_code],
      R.[model_code],
      R.[model_monthly_average_cost],
      R.[material_type],
      R.[product_code],
      R.[product_description],
      R.[product_monthly_cost],
      R.[product_monthly_calculation],
      R.[latest_purchase_cost],
      R.[currency_code],
      R.[costing_period],
      R.[costing_date],
      R.[is_deleted],
      R.[ext_field],
      R.[remark],
      R.[created_by],
      R.[created_at],
      R.[updated_by],
      R.[updated_at],
      R.[deleted_by],
      R.[deleted_at],
      ROW_NUMBER() OVER (
        PARTITION BY
          R.[plant_code],
          R.[product_code],
          R.[costing_date]
        ORDER BY
          R.[is_deleted] ASC,
          R.[updated_at] DESC
      ) AS dup_rn
    FROM #st_raw R
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
DECLARE @table_total INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_manufacturing_bom_material_cost]
);
DECLARE @source_deleted_count INT = (
  SELECT COUNT(*)
  FROM #st_source
  WHERE [is_deleted] = 1
);
DECLARE @raw_count INT = (SELECT COUNT(*) FROM #st_raw);
DECLARE @skipped_empty INT = @table_total - @raw_count;
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      R.[plant_code],
      R.[product_code],
      R.[costing_date]
    FROM #st_raw R
    GROUP BY
      R.[plant_code],
      R.[product_code],
      R.[costing_date]
  ) K
);
DECLARE @dedupe_dropped INT = @raw_count - @sap_key_count;

IF @batch_size = 0 AND @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', raw=', @raw_count, N', dedupe_dropped=', @dedupe_dropped);
  THROW 50003, @src_msg, 1;
END;

IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [product_code], [costing_date]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(800);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ', [product_code], N' / ',
      CONVERT(NVARCHAR(23), [costing_date], 121), N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [product_code], [costing_date]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  model_code NVARCHAR(40),
  product_code NVARCHAR(20),
  costing_period NVARCHAR(7),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  model_monthly_average_cost_old DECIMAL(18,5),
  model_monthly_average_cost_new DECIMAL(18,5),
  product_monthly_cost_old DECIMAL(18,5),
  product_monthly_cost_new DECIMAL(18,5),
  product_monthly_calculation_old DECIMAL(18,5),
  product_monthly_calculation_new DECIMAL(18,5),
  latest_purchase_cost_old DECIMAL(18,5),
  latest_purchase_cost_new DECIMAL(18,5),
  currency_code_old NVARCHAR(3),
  currency_code_new NVARCHAR(3),
  costing_date_old DATETIME,
  costing_date_new DATETIME
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

-- 目标 costing_date 统一为日（与源装入一致，便于业务键匹配）
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) T
SET T.[costing_date] = CAST(CAST(T.[costing_date] AS DATE) AS DATETIME)
FROM [takt_logistics_manufacturing_bom_material_cost] T
WHERE EXISTS (
  SELECT 1 FROM #st_source S
  WHERE S.[tenant_code] = T.[tenant_code] AND S.[company_code] = T.[company_code]
)
AND T.[costing_date] <> CAST(CAST(T.[costing_date] AS DATE) AS DATETIME);
  SET @dml_n = @@ROWCOUNT;
END

-- 业务键已存在于目标：沿用目标 id（目标 id 本地唯一，与源 id 无关）
UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_logistics_manufacturing_bom_material_cost] T
  ON T.[tenant_code] = S.[tenant_code]
 AND T.[company_code] = S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
 AND LTRIM(RTRIM(T.[product_code])) = S.[product_code]
 AND CAST(CAST(T.[costing_date] AS DATE) AS DATETIME) = S.[costing_date];

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_manufacturing_bom_material_cost] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[product_code])) = S.[product_code]
AND CAST(CAST(T.[costing_date] AS DATE) AS DATETIME) = S.[costing_date]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[model_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[model_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[material_type], N''))) <> LTRIM(RTRIM(ISNULL(S.[material_type], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[product_description], N''))) <> LTRIM(RTRIM(ISNULL(S.[product_description], N'')))
  OR ROUND(T.[model_monthly_average_cost], 5) <> ROUND(S.[model_monthly_average_cost], 5)
  OR ROUND(T.[product_monthly_cost], 5) <> ROUND(S.[product_monthly_cost], 5)
  OR ROUND(T.[product_monthly_calculation], 5) <> ROUND(S.[product_monthly_calculation], 5)
  OR ROUND(T.[latest_purchase_cost], 5) <> ROUND(S.[latest_purchase_cost], 5)
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[costing_period], N''))) <> LTRIM(RTRIM(ISNULL(S.[costing_period], N'')))
  OR T.[costing_date] <> S.[costing_date]
  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[model_code]=S.[model_code],
  T.[material_type]=S.[material_type],
  T.[product_description]=S.[product_description],
  T.[model_monthly_average_cost]=S.[model_monthly_average_cost],
  T.[product_monthly_cost]=S.[product_monthly_cost],
  T.[product_monthly_calculation]=S.[product_monthly_calculation],
  T.[latest_purchase_cost]=S.[latest_purchase_cost],
  T.[currency_code]=S.[currency_code],
  T.[costing_period]=S.[costing_period],
  T.[costing_date]=S.[costing_date],
  T.[culture_code]=S.[culture_code],
  T.[remark]=S.[remark],
  T.[created_by]=S.[created_by],
  T.[created_at]=S.[created_at],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at],
  T.[ext_field]=S.[ext_field]
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[model_code],[model_monthly_average_cost],[material_type],
    [product_code],[product_description],[product_monthly_cost],[product_monthly_calculation],[latest_purchase_cost],
    [currency_code],[costing_period],[costing_date],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[model_code],S.[model_monthly_average_cost],S.[material_type],
    S.[product_code],S.[product_description],S.[product_monthly_cost],S.[product_monthly_calculation],S.[latest_purchase_cost],
    S.[currency_code],S.[costing_period],S.[costing_date],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
    S.[is_deleted],
    S.[deleted_by],S.[deleted_at]
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[model_code],
  INSERTED.[product_code],
  INSERTED.[costing_period],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  COALESCE(INSERTED.[updated_by], INSERTED.[created_by], @sync_user_id),
  DELETED.[model_monthly_average_cost], INSERTED.[model_monthly_average_cost],
  DELETED.[product_monthly_cost], INSERTED.[product_monthly_cost],
  DELETED.[product_monthly_calculation], INSERTED.[product_monthly_calculation],
  DELETED.[latest_purchase_cost], INSERTED.[latest_purchase_cost],
  DELETED.[currency_code], INSERTED.[currency_code],
  DELETED.[costing_date], INSERTED.[costing_date]
INTO #delta(
  rn, oper_type, id, plant_code, model_code, product_code, costing_period,
  tenant_code, company_code, change_by,
  model_monthly_average_cost_old, model_monthly_average_cost_new,
  product_monthly_cost_old, product_monthly_cost_new,
  product_monthly_calculation_old, product_monthly_calculation_new,
  latest_purchase_cost_old, latest_purchase_cost_new,
  currency_code_old, currency_code_new,
  costing_date_old, costing_date_new
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
  [product_code] NVARCHAR(20),
  [costing_date] DATETIME
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
  INSERTED.[product_code],
  INSERTED.[costing_date]
INTO #soft_deleted_rows ([id], [plant_code], [product_code], [costing_date])
FROM [takt_logistics_manufacturing_bom_material_cost] T
WHERE T.[is_deleted] = 0
  AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[tenant_code] = T.[tenant_code]
      AND S.[company_code] = T.[company_code]
      AND S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[product_code] = LTRIM(RTRIM(T.[product_code]))
      AND S.[costing_date] = CAST(CAST(T.[costing_date] AS DATE) AS DATETIME)
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
    ISNULL([product_code], N''), N'/',
    CONVERT(NVARCHAR(23), [costing_date], 121)
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost] T
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
  FROM [takt_logistics_manufacturing_bom_material_cost] T
  WHERE EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_manufacturing_bom_material_cost] T
  WHERE T.[is_deleted]=1
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

-- 有效行：源 is_deleted=0 ↔ 目标 is_deleted=0（❌ 勿用含软删的 @source_count 对比 active）
IF @target_count <> @source_active_count
BEGIN
  DECLARE @count_msg NVARCHAR(300) = CONCAT(
    N'有效行数不一致: source_active=', @source_active_count,
    N', active=', @target_count,
    N', source_total=', @source_count,
    N', source_deleted=', @source_deleted_count);
  THROW 50002, @count_msg, 1;
END;


/* per-row delta_log removed */


DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"table_total":' + CAST(@table_total AS NVARCHAR)
  + N',"sap_raw":' + CAST(@raw_count AS NVARCHAR)
  + N',"skipped_empty":' + CAST(@skipped_empty AS NVARCHAR)
  + N',"source_deleted":' + CAST(@source_deleted_count AS NVARCHAR)
  + N',"source":' + CAST(@source_count AS NVARCHAR)
  + N',"source_active":' + CAST(@source_active_count AS NVARCHAR)
  + N',"sap_keys":' + CAST(@sap_key_count AS NVARCHAR)
  + N',"dedupe_dropped":' + CAST(@dedupe_dropped AS NVARCHAR)
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
  N'takt_logistics_manufacturing_bom_material_cost',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE BomMaterialCost Sync (summary)',
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
  N'材料成本汇总',
  N'exec_sql_merge',
  'SQL',
  N'/sync/bom-material-cost',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'' AS NVARCHAR(40)) AS [scope],
  @table_total AS [source_raw_count],
  @source_count AS [source_count],
  @source_active_count AS [source_active_count],
  @skipped_empty AS [skipped_empty_count],
  @source_deleted_count AS [source_deleted_count],
  @dedupe_dropped AS [dedupe_dropped],
  @raw_count AS [sap_raw_count],
  @sap_key_count AS [sap_key_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
