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

-- 源表 / 目标表：同名 + 列与实体 TaktMaterialMovingPrice 一致
-- {{SourceDatabase}}.dbo.takt_logistics_materials_material_moving_price → 当前租户库同名表
-- 业务唯一键：Plant+MaterialCode+ValuationPeriod（Valuation 仅业务字段，不参与匹配）
-- tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [valuation_period] NVARCHAR(7),
  [material_code] NVARCHAR(20),
  [valuation] NVARCHAR(4),
  [stock_quantity] DECIMAL(18,4),
  [stock_amount] DECIMAL(18,2),
  [price_control] NVARCHAR(1),
  [moving_price] DECIMAL(18,5),
  [price_unit] INT,
  [currency_code] NVARCHAR(3),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

-- 源表装入：
-- 1) 物料码含全角括号（　）的不参与同步
-- 2) 18 位纯数字物料截末 10 位
-- 3) 按唯一键 Plant+Material+ValuationPeriod 去重（库存金额→移动价→数量 较大者优先）
INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[valuation_period],
  S.[material_code],
  S.[valuation],
  S.[stock_quantity],
  S.[stock_amount],
  S.[price_control],
  S.[moving_price],
  S.[price_unit],
  S.[currency_code],
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
      ORDER BY N.[plant_code], N.[material_code], N.[valuation_period]
    ) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7) AS [valuation_period],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
      LTRIM(RTRIM(R.[valuation])) AS [valuation],
      COALESCE(TRY_CAST(R.[stock_quantity] AS DECIMAL(18,4)), 0) AS [stock_quantity],
      COALESCE(TRY_CAST(R.[stock_amount] AS DECIMAL(18,2)), 0) AS [stock_amount],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[price_control])), ''), '') AS [price_control],
      COALESCE(TRY_CAST(R.[moving_price] AS DECIMAL(18,5)), 0) AS [moving_price],
      COALESCE(TRY_CAST(R.[price_unit] AS INT), 0) AS [price_unit],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), ''), '') AS [currency_code],
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
          LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7),
          CASE
            WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
              AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
            ELSE LTRIM(RTRIM(R.[material_code]))
          END
        ORDER BY
          COALESCE(TRY_CAST(R.[stock_amount] AS DECIMAL(18,2)), 0) DESC,
          COALESCE(TRY_CAST(R.[moving_price] AS DECIMAL(18,5)), 0) DESC,
          COALESCE(TRY_CAST(R.[stock_quantity] AS DECIMAL(18,4)), 0) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
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
DECLARE @sap_raw_count INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price]);
DECLARE @sap_excluded_paren INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
  WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%（%'
     OR LTRIM(RTRIM(ISNULL(R.[material_code], N''))) LIKE N'%）%'
);
DECLARE @sap_eligible_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
  WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
    AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))) <> N''
);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7) AS [valuation_period],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_moving_price] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%（%'
      AND LTRIM(RTRIM(ISNULL(R.[material_code], N''))) NOT LIKE N'%）%'
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))) <> N''
    GROUP BY
      LTRIM(RTRIM(R.[plant_code])),
      LEFT(LTRIM(RTRIM(ISNULL(R.[valuation_period], N''))), 7),
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END
  ) K
);
DECLARE @dedupe_dropped INT = @sap_eligible_count - @sap_key_count;

-- 装入行数须等于「排除全角括号后」业务键去重行数
IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', excluded_paren=', @sap_excluded_paren);
  THROW 50003, @src_msg, 1;
END;

-- 唯一键 = Plant+MaterialCode+ValuationPeriod（去重后仍重复则失败）
IF EXISTS (
  SELECT 1
  FROM #st_source
  GROUP BY [plant_code], [material_code], [valuation_period]
  HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT(
      [plant_code], N' / ',
      [material_code], N' / ',
      [valuation_period], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [plant_code], [material_code], [valuation_period]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  material_code NVARCHAR(20),
  valuation NVARCHAR(4),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  stock_quantity_old DECIMAL(18,4),
  stock_quantity_new DECIMAL(18,4),
  stock_amount_old DECIMAL(18,2),
  stock_amount_new DECIMAL(18,2),
  moving_price_old DECIMAL(18,5),
  moving_price_new DECIMAL(18,5),
  price_unit_old INT,
  price_unit_new INT,
  valuation_period_old NVARCHAR(7),
  valuation_period_new NVARCHAR(7)
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

-- 业务键已存在于目标：沿用目标 id（目标 id 本地唯一，与源 id 无关）
UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_logistics_materials_material_moving_price] T
  ON T.[tenant_code] = S.[tenant_code]
 AND T.[company_code] = S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
 AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
 AND T.[valuation_period] = S.[valuation_period];

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_materials_material_moving_price] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
AND T.[valuation_period] = S.[valuation_period]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[valuation], N''))) <> LTRIM(RTRIM(ISNULL(S.[valuation], N'')))
  OR ROUND(T.[stock_quantity], 4) <> ROUND(S.[stock_quantity], 4)
  OR ROUND(T.[stock_amount], 2) <> ROUND(S.[stock_amount], 2)
  OR LTRIM(RTRIM(ISNULL(T.[price_control], N''))) <> LTRIM(RTRIM(ISNULL(S.[price_control], N'')))
  OR ROUND(T.[moving_price], 5) <> ROUND(S.[moving_price], 5)
  OR T.[price_unit] <> S.[price_unit]
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[currency_code], N'')))
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
  T.[valuation]=S.[valuation],
  T.[stock_quantity]=S.[stock_quantity],
  T.[stock_amount]=S.[stock_amount],
  T.[price_control]=S.[price_control],
  T.[moving_price]=S.[moving_price],
  T.[price_unit]=S.[price_unit],
  T.[currency_code]=S.[currency_code],
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
    [id],[plant_code],[valuation_period],[material_code],[valuation],
    [stock_quantity],[stock_amount],[price_control],[moving_price],[price_unit],[currency_code],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[valuation_period],S.[material_code],S.[valuation],
    S.[stock_quantity],S.[stock_amount],S.[price_control],S.[moving_price],S.[price_unit],S.[currency_code],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
    S.[is_deleted],
    S.[deleted_by],S.[deleted_at]
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[material_code],
  INSERTED.[valuation],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[stock_quantity], INSERTED.[stock_quantity],
  DELETED.[stock_amount], INSERTED.[stock_amount],
  DELETED.[moving_price], INSERTED.[moving_price],
  DELETED.[price_unit], INSERTED.[price_unit],
  DELETED.[valuation_period], INSERTED.[valuation_period]
INTO #delta(
  rn, oper_type, id, plant_code, material_code, valuation,
  tenant_code, company_code, change_by,
  stock_quantity_old, stock_quantity_new,
  stock_amount_old, stock_amount_new,
  moving_price_old, moving_price_new,
  price_unit_old, price_unit_new,
  valuation_period_old, valuation_period_new
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
  [material_code] NVARCHAR(20),
  [valuation_period] NVARCHAR(7)
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
  INSERTED.[valuation_period]
INTO #soft_deleted_rows ([id], [plant_code], [material_code], [valuation_period])
FROM [takt_logistics_materials_material_moving_price] T
WHERE T.[is_deleted] = 0
  AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[tenant_code] = T.[tenant_code]
      AND S.[company_code] = T.[company_code]
      AND S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[material_code] = LTRIM(RTRIM(T.[material_code]))
      AND S.[valuation_period] = T.[valuation_period]
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
    ISNULL([valuation_period], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price] T
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
  FROM [takt_logistics_materials_material_moving_price] T
  WHERE EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_moving_price] T
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
  N'takt_logistics_materials_material_moving_price',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE MaterialMovingPrice Sync (summary)',
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
  N'移动价格',
  N'exec_sql_merge',
  'SQL',
  N'/sync/material-moving-price',
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
