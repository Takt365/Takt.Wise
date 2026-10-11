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

-- 源表 / 目标表：同名 + 列与实体一致（TaktSalesConditionRecord / Item / ScaleQuantity / ScaleValue）
-- {{SourceDatabase}}.dbo.takt_logistics_sales_condition_record* → 当前租户库同名表
-- 业务唯一键：CompanyCode + PlantCode + SalesConditionRecordCode（与租户库唯一索引对齐）
-- company_code / plant_code 取自源表（主表/明细/数量等级/价值等级各自表均有 plant_code）；tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数
-- 源/目标币种列与实体一致：scale_currency_code / condition_currency_code

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#sq') IS NOT NULL DROP TABLE #sq;
IF OBJECT_ID('tempdb..#sv') IS NOT NULL DROP TABLE #sv;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#sq_delta') IS NOT NULL DROP TABLE #sq_delta;
IF OBJECT_ID('tempdb..#sv_delta') IS NOT NULL DROP TABLE #sv_delta;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT,
  [company_code] NVARCHAR(4),
  [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [plant_code] NVARCHAR(4), [sales_condition_record_code] NVARCHAR(20), [price_type] NVARCHAR(4),
  [customer_code] NVARCHAR(40), [material_code] NVARCHAR(40), [material_description] NVARCHAR(40),
  [sales_group] NVARCHAR(3), [tax_code] NVARCHAR(4),
  [gr_based_invoice_inspection] INT, [pricing_date_control] INT,
  [valid_from] DATETIME, [valid_to] DATETIME,
  [sales_quotation_id] BIGINT, [sales_quotation_code] NVARCHAR(40), [variable_key] NVARCHAR(40),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT
);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [sales_condition_record_id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [sales_condition_record_code] NVARCHAR(20), [sales_condition_record_seq] INT, [price_type] NVARCHAR(4),
  [scale_type] NVARCHAR(1), [scale_basis] NVARCHAR(1),
  [scale_quantity] DECIMAL(18,4), [scale_unit] NVARCHAR(5),
  [scale_value] DECIMAL(18,5), [scale_currency_code] NVARCHAR(3),
  [calculation_type] NVARCHAR(1), [price] DECIMAL(18,5),
  [untaxed_price] DECIMAL(18,5), [tax_included_price] DECIMAL(18,5), [tax_amount] DECIMAL(18,5),
  [condition_currency_code] NVARCHAR(3), [price_unit] INT, [unit_of_measure] NVARCHAR(5),
  [min_order_quantity] INT, [rounding_value] INT, [planned_delivery_time_days] INT, [is_obsolete] INT,
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT
);

CREATE TABLE #sq (
  [rn] INT, [id] BIGINT, [sales_condition_record_item_id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [sales_condition_record_code] NVARCHAR(20), [sales_condition_record_seq] INT, [sales_scale_seq] INT,
  [scale_quantity] DECIMAL(18,4), [price] DECIMAL(18,5),
  [untaxed_price] DECIMAL(18,5), [tax_included_price] DECIMAL(18,5), [tax_amount] DECIMAL(18,5), [is_obsolete] INT,
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT
);

CREATE TABLE #sv (
  [rn] INT, [id] BIGINT, [sales_condition_record_item_id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [sales_condition_record_code] NVARCHAR(20), [sales_condition_record_seq] INT, [sales_scale_seq] INT,
  [scale_value] DECIMAL(18,5), [price] DECIMAL(18,5),
  [untaxed_price] DECIMAL(18,5), [tax_included_price] DECIMAL(18,5), [tax_amount] DECIMAL(18,5), [is_obsolete] INT,
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT
);

CREATE TABLE #hdr_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, plant_code NVARCHAR(4), price_code NVARCHAR(20));
CREATE TABLE #item_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, price_code NVARCHAR(20), price_seq INT);
CREATE TABLE #sq_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, price_code NVARCHAR(20), price_seq INT, scale_seq INT);
CREATE TABLE #sv_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, price_code NVARCHAR(20), price_seq INT, scale_seq INT);

INSERT INTO #hdr
SELECT S.rn, @base_id + S.rn,
  S.[company_code],
  S.[tenant_code], S.[culture_code],
  S.[plant_code], S.[sales_condition_record_code], S.[price_type], S.[customer_code], S.[material_code], S.[material_description],
  S.[sales_group], S.[tax_code],
  S.[gr_based_invoice_inspection], S.[pricing_date_control],
  S.[valid_from], S.[valid_to], S.[sales_quotation_id], S.[sales_quotation_code], S.[variable_key], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[company_code], N.[plant_code], N.[sales_condition_record_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      LTRIM(RTRIM(R.[sales_condition_record_code])) AS [sales_condition_record_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[price_type])), N''), N'') AS [price_type],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[customer_code])), N''), N'') AS [customer_code],
      CASE
        WHEN LEN(LTRIM(RTRIM(R.[material_code]))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE LTRIM(RTRIM(R.[material_code]))
      END AS [material_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[material_description], N''))), 40) AS [material_description],
      NULLIF(LTRIM(RTRIM(R.[sales_group])), N'') AS [sales_group],
      NULLIF(LTRIM(RTRIM(R.[tax_code])), N'') AS [tax_code],
      COALESCE(TRY_CAST(R.[gr_based_invoice_inspection] AS INT), 0) AS [gr_based_invoice_inspection],
      COALESCE(TRY_CAST(R.[pricing_date_control] AS INT), 1) AS [pricing_date_control],
      COALESCE(TRY_CAST(R.[valid_from] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS [valid_from],
      COALESCE(TRY_CAST(R.[valid_to] AS DATETIME), CAST('9999-12-31 23:59:59' AS DATETIME)) AS [valid_to],
      TRY_CAST(R.[sales_quotation_id] AS BIGINT) AS [sales_quotation_id],
      NULLIF(LTRIM(RTRIM(R.[sales_quotation_code])), N'') AS [sales_quotation_code],
      NULLIF(LTRIM(RTRIM(R.[variable_key])), N'') AS [variable_key],
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
          LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
          LTRIM(RTRIM(R.[plant_code])),
          LTRIM(RTRIM(R.[sales_condition_record_code]))
        ORDER BY LEN(ISNULL(LTRIM(RTRIM(R.[material_code])), N'')) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record] R
    WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;


CREATE CLUSTERED INDEX ix_hdr_rn ON #hdr ([rn]);
CREATE UNIQUE INDEX ix_hdr_company_plant_code ON #hdr ([company_code], [plant_code], [sales_condition_record_code]);
CREATE INDEX ix_hdr_company_code ON #hdr ([company_code], [sales_condition_record_code]);

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @hdr_source AS [to_rn],
  @hdr_source AS [max_rn],
  @hdr_source AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @hdr_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record] R
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
);
IF @hdr_source <> @hdr_sap_raw
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(N'主表装入不一致: sap=', @hdr_sap_raw, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;
IF EXISTS (SELECT 1 FROM #hdr GROUP BY [company_code], [plant_code], [sales_condition_record_code] HAVING COUNT(*) > 1)
BEGIN
  DECLARE @hdr_dup NVARCHAR(400);
  SELECT TOP 1 @hdr_dup = CONCAT([company_code], N' / ', [plant_code], N' / ', [sales_condition_record_code], N' x', COUNT(*))
  FROM #hdr GROUP BY [company_code], [plant_code], [sales_condition_record_code] HAVING COUNT(*) > 1;
  THROW 50001, @hdr_dup, 1;
END;

INSERT INTO #item
SELECT S.rn, @base_id + 1000000000 + S.rn, 0,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code],
  S.[sales_condition_record_code], S.[sales_condition_record_seq], S.[price_type],
  S.[scale_type], S.[scale_basis], S.[scale_quantity], S.[scale_unit],
  S.[scale_value], S.[scale_currency_code], S.[calculation_type], S.[price],
  S.[untaxed_price], S.[tax_included_price], S.[tax_amount], S.[condition_currency_code], S.[price_unit], S.[unit_of_measure],
  S.[min_order_quantity], S.[rounding_value], S.[planned_delivery_time_days], S.[is_obsolete], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
    LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
    LTRIM(RTRIM(R.[sales_condition_record_code])) AS [sales_condition_record_code],
    COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10) AS [sales_condition_record_seq],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[price_type])), N''), N'') AS [price_type],
    NULLIF(LTRIM(RTRIM(R.[scale_type])), N'') AS [scale_type],
    NULLIF(LTRIM(RTRIM(R.[scale_basis])), N'') AS [scale_basis],
    ROUND(COALESCE(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,8)), 0), 4) AS [scale_quantity],
    NULLIF(LTRIM(RTRIM(R.[scale_unit])), N'') AS [scale_unit],
    ROUND(COALESCE(TRY_CAST(R.[scale_value] AS DECIMAL(18,8)), 0), 5) AS [scale_value],
    NULLIF(LTRIM(RTRIM(R.[scale_currency_code])), N'') AS [scale_currency_code],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[calculation_type])), N''), N'') AS [calculation_type],
    ROUND(COALESCE(TRY_CAST(R.[price] AS DECIMAL(18,8)), 0), 5) AS [price],
    ROUND(COALESCE(TRY_CAST(R.[untaxed_price] AS DECIMAL(18,8)), 0), 5) AS [untaxed_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_included_price] AS DECIMAL(18,8)), 0), 5) AS [tax_included_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_amount],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[condition_currency_code])), N''), N'') AS [condition_currency_code],
    COALESCE(TRY_CAST(R.[price_unit] AS INT), 0) AS [price_unit],
    ISNULL(NULLIF(LTRIM(RTRIM(R.[unit_of_measure])), N''), N'') AS [unit_of_measure],
    COALESCE(TRY_CAST(R.[min_order_quantity] AS INT), 0) AS [min_order_quantity],
    COALESCE(TRY_CAST(R.[rounding_value] AS INT), 0) AS [rounding_value],
    COALESCE(TRY_CAST(R.[planned_delivery_time_days] AS INT), 0) AS [planned_delivery_time_days],
    COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
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
      ORDER BY
        LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
        LTRIM(RTRIM(R.[plant_code])),
        LTRIM(RTRIM(R.[sales_condition_record_code])),
        COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10)
    ) AS rn
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record_item] R
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #hdr H
      WHERE H.[sales_condition_record_code] = LTRIM(RTRIM(R.[sales_condition_record_code]))
        AND H.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
        AND H.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4)
    )
) S;

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @item_source AS [to_rn],
  @item_source AS [max_rn],
  @item_source AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @item_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record_item] R
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #hdr H
      WHERE H.[sales_condition_record_code] = LTRIM(RTRIM(R.[sales_condition_record_code]))
        AND H.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
        AND H.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4)
    )
);
IF @item_source <> @item_sap_raw
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(N'明细装入不一致: sap=', @item_sap_raw, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;
IF EXISTS (SELECT 1 FROM #item GROUP BY [company_code], [plant_code], [sales_condition_record_code], [sales_condition_record_seq] HAVING COUNT(*) > 1)
BEGIN
  DECLARE @item_dup NVARCHAR(400);
  SELECT TOP 1 @item_dup = CONCAT([company_code], N' / ', [plant_code], N' / ', [sales_condition_record_code], N' / ', [sales_condition_record_seq], N' x', COUNT(*))
  FROM #item GROUP BY [company_code], [plant_code], [sales_condition_record_code], [sales_condition_record_seq] HAVING COUNT(*) > 1;
  THROW 50001, @item_dup, 1;
END;

INSERT INTO #sq
SELECT S.rn, @base_id + 2000000000 + S.rn, 0,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code],
  S.[sales_condition_record_code], S.[sales_condition_record_seq], S.[sales_scale_seq],
  S.[scale_quantity], S.[price], S.[untaxed_price], S.[tax_included_price], S.[tax_amount], S.[is_obsolete], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
    LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
    LTRIM(RTRIM(R.[sales_condition_record_code])) AS [sales_condition_record_code],
    COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10) AS [sales_condition_record_seq],
    COALESCE(TRY_CAST(R.[sales_scale_seq] AS INT), 10) AS [sales_scale_seq],
    ROUND(COALESCE(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,8)), 0), 4) AS [scale_quantity],
    ROUND(COALESCE(TRY_CAST(R.[price] AS DECIMAL(18,8)), 0), 5) AS [price],
    ROUND(COALESCE(TRY_CAST(R.[untaxed_price] AS DECIMAL(18,8)), 0), 5) AS [untaxed_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_included_price] AS DECIMAL(18,8)), 0), 5) AS [tax_included_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_amount],
    COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
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
      ORDER BY
        LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
        LTRIM(RTRIM(R.[plant_code])),
        LTRIM(RTRIM(R.[sales_condition_record_code])),
        COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10),
        COALESCE(TRY_CAST(R.[sales_scale_seq] AS INT), 10),
        COALESCE(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,8)), 0)
    ) AS rn
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record_scale_quantity] R
  INNER JOIN #item I
    ON I.[sales_condition_record_code] = LTRIM(RTRIM(R.[sales_condition_record_code]))
   AND I.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
   AND I.[sales_condition_record_seq] = COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10)
   AND I.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4)
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
) S;

DECLARE @sq_source INT = (SELECT COUNT(*) FROM #sq);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @sq_source AS [to_rn],
  @sq_source AS [max_rn],
  @sq_source AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@sq_source) AS NVARCHAR(20)), N'|',
  CAST((@sq_source) AS NVARCHAR(20)), N'|',
  CAST((@sq_source) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @sq_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record_scale_quantity] R
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #item I
      WHERE I.[sales_condition_record_code] = LTRIM(RTRIM(R.[sales_condition_record_code]))
        AND I.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
        AND I.[sales_condition_record_seq] = COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10)
        AND I.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4)
    )
);
IF @sq_source <> @sq_sap_raw
BEGIN
  DECLARE @sq_src_msg NVARCHAR(200) = CONCAT(N'数量等级装入不一致: sap=', @sq_sap_raw, N', loaded=', @sq_source);
  THROW 50003, @sq_src_msg, 1;
END;

INSERT INTO #sv
SELECT S.rn, @base_id + 3000000000 + S.rn, 0,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code],
  S.[sales_condition_record_code], S.[sales_condition_record_seq], S.[sales_scale_seq],
  S.[scale_value], S.[price], S.[untaxed_price], S.[tax_included_price], S.[tax_amount], S.[is_obsolete], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
    LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
    LTRIM(RTRIM(R.[sales_condition_record_code])) AS [sales_condition_record_code],
    COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10) AS [sales_condition_record_seq],
    COALESCE(TRY_CAST(R.[sales_scale_seq] AS INT), 10) AS [sales_scale_seq],
    ROUND(COALESCE(TRY_CAST(R.[scale_value] AS DECIMAL(18,8)), 0), 5) AS [scale_value],
    ROUND(COALESCE(TRY_CAST(R.[price] AS DECIMAL(18,8)), 0), 5) AS [price],
    ROUND(COALESCE(TRY_CAST(R.[untaxed_price] AS DECIMAL(18,8)), 0), 5) AS [untaxed_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_included_price] AS DECIMAL(18,8)), 0), 5) AS [tax_included_price],
    ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_amount],
    COALESCE(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
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
      ORDER BY
        LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
        LTRIM(RTRIM(R.[plant_code])),
        LTRIM(RTRIM(R.[sales_condition_record_code])),
        COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10),
        COALESCE(TRY_CAST(R.[sales_scale_seq] AS INT), 10),
        COALESCE(TRY_CAST(R.[scale_value] AS DECIMAL(18,8)), 0)
    ) AS rn
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record_scale_value] R
  INNER JOIN #item I
    ON I.[sales_condition_record_code] = LTRIM(RTRIM(R.[sales_condition_record_code]))
   AND I.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
   AND I.[sales_condition_record_seq] = COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10)
   AND I.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4)
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
) S;

DECLARE @sv_source INT = (SELECT COUNT(*) FROM #sv);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @sv_source AS [to_rn],
  @sv_source AS [max_rn],
  @sv_source AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@sv_source) AS NVARCHAR(20)), N'|',
  CAST((@sv_source) AS NVARCHAR(20)), N'|',
  CAST((@sv_source) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @sv_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_condition_record_scale_value] R
  WHERE LTRIM(RTRIM(ISNULL(R.[sales_condition_record_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    AND EXISTS (
      SELECT 1 FROM #item I
      WHERE I.[sales_condition_record_code] = LTRIM(RTRIM(R.[sales_condition_record_code]))
        AND I.[plant_code] = LTRIM(RTRIM(R.[plant_code]))
        AND I.[sales_condition_record_seq] = COALESCE(TRY_CAST(R.[sales_condition_record_seq] AS INT), 10)
        AND I.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4)
    )
);
IF @sv_source <> @sv_sap_raw
BEGIN
  DECLARE @sv_src_msg NVARCHAR(200) = CONCAT(N'价值等级装入不一致: sap=', @sv_sap_raw, N', loaded=', @sv_source);
  THROW 50003, @sv_src_msg, 1;
END;

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record] T
  WHERE T.[tenant_code]=@tenant_code AND T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = T.[company_code])
);
DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record_item] I
  WHERE I.[tenant_code]=@tenant_code AND I.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = I.[company_code])
);
DECLARE @sq_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record_scale_quantity] Q
  WHERE Q.[tenant_code]=@tenant_code AND Q.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = Q.[company_code])
);
DECLARE @sv_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record_scale_value] V
  WHERE V.[tenant_code]=@tenant_code AND V.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = V.[company_code])
);

UPDATE S SET S.[id] = COALESCE(T.[id], S.[id])
FROM #hdr S
LEFT JOIN [takt_logistics_sales_condition_record] T
  ON T.[tenant_code]=S.[tenant_code]
 AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code])) = S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_condition_record_code])) = S.[sales_condition_record_code];

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #hdr), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_sales_condition_record] AS T
USING (SELECT * FROM #hdr WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[sales_condition_record_code]))=S.[sales_condition_record_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted],0)<>S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[price_type],N'')))<>S.[price_type]
  OR LTRIM(RTRIM(ISNULL(T.[customer_code],N'')))<>S.[customer_code]
  OR LTRIM(RTRIM(ISNULL(T.[material_code],N'')))<>S.[material_code]
  OR LTRIM(RTRIM(ISNULL(T.[material_description],N'')))<>LTRIM(RTRIM(ISNULL(S.[material_description],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[sales_group],N'')))<>LTRIM(RTRIM(ISNULL(S.[sales_group],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_code],N'')))
  OR T.[gr_based_invoice_inspection]<>S.[gr_based_invoice_inspection]
  OR T.[pricing_date_control]<>S.[pricing_date_control]
  OR T.[valid_from]<>S.[valid_from] OR T.[valid_to]<>S.[valid_to]
  OR ISNULL(T.[sales_quotation_id],0)<>ISNULL(S.[sales_quotation_id],0)
  OR LTRIM(RTRIM(ISNULL(T.[sales_quotation_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[sales_quotation_code],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[variable_key],N'')))<>LTRIM(RTRIM(ISNULL(S.[variable_key],N'')))
  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[price_type]=S.[price_type],
  T.[customer_code]=S.[customer_code],
  T.[material_code]=S.[material_code],
  T.[material_description]=S.[material_description],
  T.[sales_group]=S.[sales_group],
  T.[tax_code]=S.[tax_code],
  T.[gr_based_invoice_inspection]=S.[gr_based_invoice_inspection],
  T.[pricing_date_control]=S.[pricing_date_control],
  T.[valid_from]=S.[valid_from],
  T.[valid_to]=S.[valid_to],
  T.[sales_quotation_id]=S.[sales_quotation_id],
  T.[sales_quotation_code]=S.[sales_quotation_code],
  T.[variable_key]=S.[variable_key],
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
WHEN NOT MATCHED THEN INSERT (
  [id],[plant_code],[sales_condition_record_code],[price_type],[customer_code],[material_code],[material_description],
  [sales_group],[tax_code],[gr_based_invoice_inspection],[pricing_date_control],
  [valid_from],[valid_to],[sales_quotation_id],[sales_quotation_code],[variable_key],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
) VALUES (
  S.[id],S.[plant_code],S.[sales_condition_record_code],S.[price_type],S.[customer_code],S.[material_code],S.[material_description],
  S.[sales_group],S.[tax_code],S.[gr_based_invoice_inspection],S.[pricing_date_control],
  S.[valid_from],S.[valid_to],S.[sales_quotation_id],S.[sales_quotation_code],S.[variable_key],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
  COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at]
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[plant_code], INSERTED.[sales_condition_record_code]
INTO #hdr_delta(rn, oper_type, id, plant_code, price_code);
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

UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_sales_condition_record] T
  ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code] AND T.[is_deleted]=0
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code] AND LTRIM(RTRIM(T.[sales_condition_record_code]))=S.[sales_condition_record_code];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_sales_condition_record] T
WHERE T.[tenant_code]=@tenant_code AND T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[company_code] = T.[company_code])
  AND NOT EXISTS (
    SELECT 1 FROM #hdr S
    WHERE S.[company_code] = T.[company_code]
      AND S.[plant_code] = LTRIM(RTRIM(T.[plant_code]))
      AND S.[sales_condition_record_code] = LTRIM(RTRIM(T.[sales_condition_record_code]))
  );
DECLARE @hdr_del INT = @@ROWCOUNT;

UPDATE I SET I.[sales_condition_record_id]=H.[id]
FROM #item I
INNER JOIN #hdr H
  ON H.[company_code]=I.[company_code]
 AND H.[plant_code]=I.[plant_code]
 AND H.[sales_condition_record_code]=I.[sales_condition_record_code];
DELETE FROM #item WHERE [sales_condition_record_id]=0 OR [sales_condition_record_id] IS NULL;

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #item), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_sales_condition_record_item] AS T
USING (SELECT * FROM #item WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[sales_condition_record_id]=S.[sales_condition_record_id] AND T.[sales_condition_record_seq]=S.[sales_condition_record_seq]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted],0)<>S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[plant_code],N'')))<>S.[plant_code]
  OR LTRIM(RTRIM(ISNULL(T.[sales_condition_record_code],N'')))<>S.[sales_condition_record_code]
  OR LTRIM(RTRIM(ISNULL(T.[price_type],N'')))<>S.[price_type]
  OR LTRIM(RTRIM(ISNULL(T.[scale_type],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_type],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[scale_basis],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_basis],N'')))
  OR ROUND(T.[scale_quantity],4)<>ROUND(S.[scale_quantity],4)
  OR LTRIM(RTRIM(ISNULL(T.[scale_unit],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_unit],N'')))
  OR ROUND(T.[scale_value],5)<>ROUND(S.[scale_value],5)
  OR LTRIM(RTRIM(ISNULL(T.[scale_currency_code],N'')))<>LTRIM(RTRIM(ISNULL(S.[scale_currency_code],N'')))
  OR LTRIM(RTRIM(ISNULL(T.[calculation_type],N'')))<>S.[calculation_type]
  OR ROUND(T.[price],5)<>ROUND(S.[price],5)
  OR ROUND(T.[untaxed_price],5)<>ROUND(S.[untaxed_price],5)
  OR ROUND(T.[tax_included_price],5)<>ROUND(S.[tax_included_price],5)
  OR ROUND(T.[tax_amount],5)<>ROUND(S.[tax_amount],5)
  OR LTRIM(RTRIM(ISNULL(T.[condition_currency_code],N'')))<>S.[condition_currency_code]
  OR T.[price_unit]<>S.[price_unit]
  OR LTRIM(RTRIM(ISNULL(T.[unit_of_measure],N'')))<>S.[unit_of_measure]
  OR T.[min_order_quantity]<>S.[min_order_quantity]
  OR T.[rounding_value]<>S.[rounding_value]
  OR T.[planned_delivery_time_days]<>S.[planned_delivery_time_days]
  OR T.[is_obsolete]<>S.[is_obsolete]
  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[sales_condition_record_code]=S.[sales_condition_record_code],
  T.[price_type]=S.[price_type],
  T.[scale_type]=S.[scale_type],
  T.[scale_basis]=S.[scale_basis],
  T.[scale_quantity]=S.[scale_quantity],
  T.[scale_unit]=S.[scale_unit],
  T.[scale_value]=S.[scale_value],
  T.[scale_currency_code]=S.[scale_currency_code],
  T.[calculation_type]=S.[calculation_type],
  T.[price]=S.[price],
  T.[untaxed_price]=S.[untaxed_price],
  T.[tax_included_price]=S.[tax_included_price],
  T.[tax_amount]=S.[tax_amount],
  T.[condition_currency_code]=S.[condition_currency_code],
  T.[price_unit]=S.[price_unit],
  T.[unit_of_measure]=S.[unit_of_measure],
  T.[min_order_quantity]=S.[min_order_quantity],
  T.[rounding_value]=S.[rounding_value],
  T.[planned_delivery_time_days]=S.[planned_delivery_time_days],
  T.[is_obsolete]=S.[is_obsolete],
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
WHEN NOT MATCHED THEN INSERT (
  [id],[sales_condition_record_id],[plant_code],[sales_condition_record_code],[sales_condition_record_seq],[price_type],
  [scale_type],[scale_basis],[scale_quantity],[scale_unit],[scale_value],[scale_currency_code],
  [calculation_type],[price],[untaxed_price],[tax_included_price],[tax_amount],
  [condition_currency_code],[price_unit],[unit_of_measure],
  [min_order_quantity],[rounding_value],[planned_delivery_time_days],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
) VALUES (
  S.[id],S.[sales_condition_record_id],S.[plant_code],S.[sales_condition_record_code],S.[sales_condition_record_seq],S.[price_type],
  S.[scale_type],S.[scale_basis],S.[scale_quantity],S.[scale_unit],S.[scale_value],S.[scale_currency_code],
  S.[calculation_type],S.[price],S.[untaxed_price],S.[tax_included_price],S.[tax_amount],
  S.[condition_currency_code],S.[price_unit],S.[unit_of_measure],
  S.[min_order_quantity],S.[rounding_value],S.[planned_delivery_time_days],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
  COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at]
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[sales_condition_record_code], INSERTED.[sales_condition_record_seq]
INTO #item_delta(rn, oper_type, id, price_code, price_seq);
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

UPDATE S SET S.[id]=T.[id]
FROM #item S
INNER JOIN [takt_logistics_sales_condition_record_item] T
  ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code] AND T.[is_deleted]=0
 AND T.[sales_condition_record_id]=S.[sales_condition_record_id] AND T.[sales_condition_record_seq]=S.[sales_condition_record_seq];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_sales_condition_record_item] T
WHERE T.[tenant_code]=@tenant_code AND T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[company_code] = T.[company_code])
  AND EXISTS (
    SELECT 1 FROM [takt_logistics_sales_condition_record] H
    WHERE H.[id]=T.[sales_condition_record_id] AND H.[tenant_code]=@tenant_code
      AND H.[company_code]=T.[company_code]
  )
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[sales_condition_record_id]=T.[sales_condition_record_id] AND S.[sales_condition_record_seq]=T.[sales_condition_record_seq]);
DECLARE @item_del INT = @@ROWCOUNT;

UPDATE Q SET Q.[sales_condition_record_item_id]=I.[id]
FROM #sq Q
INNER JOIN #item I
  ON I.[company_code]=Q.[company_code]
 AND I.[plant_code]=Q.[plant_code]
 AND I.[sales_condition_record_code]=Q.[sales_condition_record_code]
 AND I.[sales_condition_record_seq]=Q.[sales_condition_record_seq];
DELETE FROM #sq WHERE [sales_condition_record_item_id]=0 OR [sales_condition_record_item_id] IS NULL;

UPDATE V SET V.[sales_condition_record_item_id]=I.[id]
FROM #sv V
INNER JOIN #item I
  ON I.[company_code]=V.[company_code]
 AND I.[plant_code]=V.[plant_code]
 AND I.[sales_condition_record_code]=V.[sales_condition_record_code]
 AND I.[sales_condition_record_seq]=V.[sales_condition_record_seq];
DELETE FROM #sv WHERE [sales_condition_record_item_id]=0 OR [sales_condition_record_item_id] IS NULL;

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #sq), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_sales_condition_record_scale_quantity] AS T
USING (SELECT * FROM #sq WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[sales_condition_record_item_id]=S.[sales_condition_record_item_id] AND T.[sales_condition_record_seq]=S.[sales_condition_record_seq]
 AND T.[sales_scale_seq]=S.[sales_scale_seq] AND ROUND(T.[scale_quantity],4)=ROUND(S.[scale_quantity],4)
WHEN MATCHED AND (
  ISNULL(T.[is_deleted],0)<>S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[plant_code],N'')))<>S.[plant_code]
  OR LTRIM(RTRIM(ISNULL(T.[sales_condition_record_code],N'')))<>S.[sales_condition_record_code]
  OR ROUND(T.[price],5)<>ROUND(S.[price],5) OR ROUND(T.[untaxed_price],5)<>ROUND(S.[untaxed_price],5)
  OR ROUND(T.[tax_included_price],5)<>ROUND(S.[tax_included_price],5) OR ROUND(T.[tax_amount],5)<>ROUND(S.[tax_amount],5)
  OR T.[is_obsolete]<>S.[is_obsolete]
  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[sales_condition_record_code]=S.[sales_condition_record_code],
  T.[price]=S.[price],
  T.[untaxed_price]=S.[untaxed_price],
  T.[tax_included_price]=S.[tax_included_price],
  T.[tax_amount]=S.[tax_amount],
  T.[is_obsolete]=S.[is_obsolete],
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
WHEN NOT MATCHED THEN INSERT (
  [id],[sales_condition_record_item_id],[plant_code],[sales_condition_record_code],[sales_condition_record_seq],[sales_scale_seq],
  [scale_quantity],[price],[untaxed_price],[tax_included_price],[tax_amount],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
) VALUES (
  S.[id],S.[sales_condition_record_item_id],S.[plant_code],S.[sales_condition_record_code],S.[sales_condition_record_seq],S.[sales_scale_seq],
  S.[scale_quantity],S.[price],S.[untaxed_price],S.[tax_included_price],S.[tax_amount],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
  COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at]
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[sales_condition_record_code], INSERTED.[sales_condition_record_seq], INSERTED.[sales_scale_seq]
INTO #sq_delta(rn, oper_type, id, price_code, price_seq, scale_seq);
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

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_sales_condition_record_scale_quantity] T
WHERE T.[tenant_code]=@tenant_code AND T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[company_code] = T.[company_code])
  AND EXISTS (
    SELECT 1 FROM [takt_logistics_sales_condition_record_item] I
    INNER JOIN [takt_logistics_sales_condition_record] H ON H.[id]=I.[sales_condition_record_id]
    WHERE I.[id]=T.[sales_condition_record_item_id] AND I.[tenant_code]=@tenant_code
      AND I.[company_code]=T.[company_code] AND H.[tenant_code]=@tenant_code
      AND H.[company_code]=I.[company_code]
  )
  AND NOT EXISTS (
    SELECT 1 FROM #sq S
    WHERE S.[sales_condition_record_item_id]=T.[sales_condition_record_item_id] AND S.[sales_condition_record_seq]=T.[sales_condition_record_seq]
      AND S.[sales_scale_seq]=T.[sales_scale_seq] AND ROUND(S.[scale_quantity],4)=ROUND(T.[scale_quantity],4)
  );
DECLARE @sq_del INT = @@ROWCOUNT;

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #sv), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_sales_condition_record_scale_value] AS T
USING (SELECT * FROM #sv WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[sales_condition_record_item_id]=S.[sales_condition_record_item_id] AND T.[sales_condition_record_seq]=S.[sales_condition_record_seq]
 AND T.[sales_scale_seq]=S.[sales_scale_seq] AND ROUND(T.[scale_value],5)=ROUND(S.[scale_value],5)
WHEN MATCHED AND (
  ISNULL(T.[is_deleted],0)<>S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[plant_code],N'')))<>S.[plant_code]
  OR LTRIM(RTRIM(ISNULL(T.[sales_condition_record_code],N'')))<>S.[sales_condition_record_code]
  OR ROUND(T.[price],5)<>ROUND(S.[price],5) OR ROUND(T.[untaxed_price],5)<>ROUND(S.[untaxed_price],5)
  OR ROUND(T.[tax_included_price],5)<>ROUND(S.[tax_included_price],5) OR ROUND(T.[tax_amount],5)<>ROUND(S.[tax_amount],5)
  OR T.[is_obsolete]<>S.[is_obsolete]
  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[sales_condition_record_code]=S.[sales_condition_record_code],
  T.[price]=S.[price],
  T.[untaxed_price]=S.[untaxed_price],
  T.[tax_included_price]=S.[tax_included_price],
  T.[tax_amount]=S.[tax_amount],
  T.[is_obsolete]=S.[is_obsolete],
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
WHEN NOT MATCHED THEN INSERT (
  [id],[sales_condition_record_item_id],[plant_code],[sales_condition_record_code],[sales_condition_record_seq],[sales_scale_seq],
  [scale_value],[price],[untaxed_price],[tax_included_price],[tax_amount],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
  [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
) VALUES (
  S.[id],S.[sales_condition_record_item_id],S.[plant_code],S.[sales_condition_record_code],S.[sales_condition_record_seq],S.[sales_scale_seq],
  S.[scale_value],S.[price],S.[untaxed_price],S.[tax_included_price],S.[tax_amount],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
  COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at]
)
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[sales_condition_record_code], INSERTED.[sales_condition_record_seq], INSERTED.[sales_scale_seq]
INTO #sv_delta(rn, oper_type, id, price_code, price_seq, scale_seq);
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

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
FROM [takt_logistics_sales_condition_record_scale_value] T
WHERE T.[tenant_code]=@tenant_code AND T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[company_code] = T.[company_code])
  AND EXISTS (
    SELECT 1 FROM [takt_logistics_sales_condition_record_item] I
    INNER JOIN [takt_logistics_sales_condition_record] H ON H.[id]=I.[sales_condition_record_id]
    WHERE I.[id]=T.[sales_condition_record_item_id] AND I.[tenant_code]=@tenant_code
      AND I.[company_code]=T.[company_code] AND H.[tenant_code]=@tenant_code
      AND H.[company_code]=I.[company_code]
  )
  AND NOT EXISTS (
    SELECT 1 FROM #sv S
    WHERE S.[sales_condition_record_item_id]=T.[sales_condition_record_item_id] AND S.[sales_condition_record_seq]=T.[sales_condition_record_seq]
      AND S.[sales_scale_seq]=T.[sales_scale_seq] AND ROUND(S.[scale_value],5)=ROUND(T.[scale_value],5)
  );
DECLARE @sv_del INT = @@ROWCOUNT;

DECLARE @hdr_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record] T
  WHERE T.[tenant_code]=@tenant_code AND T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = T.[company_code])
);
DECLARE @item_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record_item] I
  WHERE I.[tenant_code]=@tenant_code AND I.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = I.[company_code])
);
DECLARE @sq_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record_scale_quantity] Q
  WHERE Q.[tenant_code]=@tenant_code AND Q.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = Q.[company_code])
);
DECLARE @sv_after INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_condition_record_scale_value] V
  WHERE V.[tenant_code]=@tenant_code AND V.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr H WHERE H.[company_code] = V.[company_code])
);

DECLARE @hdr_ins INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type='INSERT');
DECLARE @hdr_upd INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type='UPDATE');
DECLARE @item_ins INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type='INSERT');
DECLARE @item_upd INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type='UPDATE');
DECLARE @sq_ins INT = (SELECT COUNT(*) FROM #sq_delta WHERE oper_type='INSERT');
DECLARE @sq_upd INT = (SELECT COUNT(*) FROM #sq_delta WHERE oper_type='UPDATE');
DECLARE @sv_ins INT = (SELECT COUNT(*) FROM #sv_delta WHERE oper_type='INSERT');
DECLARE @sv_upd INT = (SELECT COUNT(*) FROM #sv_delta WHERE oper_type='UPDATE');

IF @hdr_after <> @hdr_source
BEGIN
  DECLARE @hdr_cnt_msg NVARCHAR(200) = CONCAT(N'主表有效行不一致: source=', @hdr_source, N', active=', @hdr_after);
  THROW 50002, @hdr_cnt_msg, 1;
END;

DECLARE @json_result NVARCHAR(MAX) =
  N'{"hdr_sap_raw":' + CAST(@hdr_sap_raw AS NVARCHAR)
  + N',"hdr_source":' + CAST(@hdr_source AS NVARCHAR)
  + N',"hdr_before":' + CAST(@hdr_before AS NVARCHAR)
  + N',"hdr_after":' + CAST(@hdr_after AS NVARCHAR)
  + N',"hdr_insert":' + CAST(@hdr_ins AS NVARCHAR)
  + N',"hdr_update":' + CAST(@hdr_upd AS NVARCHAR)
  + N',"hdr_soft_delete":' + CAST(@hdr_del AS NVARCHAR)
  + N',"item_sap_raw":' + CAST(@item_sap_raw AS NVARCHAR)
  + N',"item_source":' + CAST(@item_source AS NVARCHAR)
  + N',"item_before":' + CAST(@item_before AS NVARCHAR)
  + N',"item_after":' + CAST(@item_after AS NVARCHAR)
  + N',"item_insert":' + CAST(@item_ins AS NVARCHAR)
  + N',"item_update":' + CAST(@item_upd AS NVARCHAR)
  + N',"item_soft_delete":' + CAST(@item_del AS NVARCHAR)
  + N',"sq_sap_raw":' + CAST(@sq_sap_raw AS NVARCHAR)
  + N',"sq_source":' + CAST(@sq_source AS NVARCHAR)
  + N',"sq_before":' + CAST(@sq_before AS NVARCHAR)
  + N',"sq_after":' + CAST(@sq_after AS NVARCHAR)
  + N',"sq_insert":' + CAST(@sq_ins AS NVARCHAR)
  + N',"sq_update":' + CAST(@sq_upd AS NVARCHAR)
  + N',"sq_soft_delete":' + CAST(@sq_del AS NVARCHAR)
  + N',"sv_sap_raw":' + CAST(@sv_sap_raw AS NVARCHAR)
  + N',"sv_source":' + CAST(@sv_source AS NVARCHAR)
  + N',"sv_before":' + CAST(@sv_before AS NVARCHAR)
  + N',"sv_after":' + CAST(@sv_after AS NVARCHAR)
  + N',"sv_insert":' + CAST(@sv_ins AS NVARCHAR)
  + N',"sv_update":' + CAST(@sv_upd AS NVARCHAR)
  + N',"sv_soft_delete":' + CAST(@sv_del AS NVARCHAR) + N'}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[plant_code],[culture_code],[created_by],[created_at]
)
SELECT
  @base_id + 2000000000 + ROW_NUMBER() OVER (ORDER BY P.[company_code], P.[plant_code], P.[culture_code]),
  N'SYSTEM_SYNC', N'SYNC', N'销售条件记录',
  N'exec_sql_merge', 'SQL', N'/sync/sales-condition-record', CONCAT('batch_size=', @batch_size),
  @json_result, '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now, DATEDIFF(MILLISECOND,@now,GETDATE()), 1, '',
  @tenant_code, P.[company_code], P.[plant_code], P.[culture_code], @sync_user_id, @now
FROM (
  SELECT DISTINCT [company_code], [plant_code], [culture_code]
  FROM #hdr
  WHERE LTRIM(RTRIM(ISNULL([plant_code], N''))) <> N''
) P;

SELECT N'QUARTZ_SYNC_SUMMARY' AS [summary_tag], CAST(N'hdr' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_raw AS [source_raw_count], @hdr_source AS [source_count], @hdr_before AS [target_before],
  @hdr_after AS [target_after], @hdr_ins AS [insert_count], @hdr_upd AS [update_count], @hdr_del AS [delete_count]
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'item', @item_sap_raw, @item_source, @item_before, @item_after, @item_ins, @item_upd, @item_del
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'scale_quantity', @sq_sap_raw, @sq_source, @sq_before, @sq_after, @sq_ins, @sq_upd, @sq_del
UNION ALL SELECT N'QUARTZ_SYNC_SUMMARY', N'scale_value', @sv_sap_raw, @sv_source, @sv_before, @sv_after, @sv_ins, @sv_upd, @sv_del;
