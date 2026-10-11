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
DECLARE @ddl NVARCHAR(500);

-- 源表 / 目标表：同名 + 列与实体 TaktSalesInvoice / TaktSalesInvoiceItem 一致
-- {{SourceDatabase}}.dbo.takt_logistics_sales_invoice[_item] → 当前租户库同名表
-- 主表唯一键：Tenant+Company+billing_document_code
-- 明细唯一键：sales_invoice_id+line_number
-- 源明细 FK：先按 billing_document_code 回填 sales_invoice_id，再 SH.id=R.sales_invoice_id 装入
-- tenant/company/plant/culture：优先源表本列；为空时回退任务参数 {{TenantCode}}/{{CompanyCode}}/{{PlantCode}}/{{CultureCode}}
-- （SAP 暂存常见主表隔离列空、明细有值，不回退会导致主表装入 0、明细 keys 巨大）

IF COL_LENGTH(N'dbo.takt_logistics_sales_invoice', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_invoice] ADD [posted_by] NVARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_invoice', N'posted_by') IS NULL
BEGIN
  SET @ddl = N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] ADD [posted_by] NVARCHAR(6) NULL';
  EXEC sys.sp_executesql @ddl;
END

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [billing_document_code] NVARCHAR(10),
  [billing_type] NVARCHAR(4),
  [billing_category] NVARCHAR(1),
  [document_category] NVARCHAR(1),
  [currency_code] NVARCHAR(3),
  [sales_organization] NVARCHAR(4),
  [distribution_channel] NVARCHAR(2),
  [pricing_procedure] NVARCHAR(6),
  [condition_code] NVARCHAR(10),
  [shipping_conditions] NVARCHAR(2),
  [billing_date] DATETIME,
  [customer_group] NVARCHAR(2),
  [incoterms1] NVARCHAR(3),
  [incoterms2] NVARCHAR(28),
  [posting_status] NVARCHAR(1),
  [accounting_exchange_rate] DECIMAL(18,5),
  [payment_terms] NVARCHAR(4),
  [account_assignment_group] NVARCHAR(2),
  [country_code] NVARCHAR(3),
  [net_amount] DECIMAL(18,2),
  [payer_code] NVARCHAR(10),
  [customer_code] NVARCHAR(10),
  [statistics_currency_code] NVARCHAR(3),
  [foreign_trade_code] NVARCHAR(10),
  [cancelled_billing_document] NVARCHAR(10),
  [invoice_list_type] NVARCHAR(4),
  [division] NVARCHAR(2),
  [hierarchy_type_pricing] NVARCHAR(1),
  [trading_partner] NVARCHAR(6),
  [tax_departure_country] NVARCHAR(3),
  [organization_sales_tax_number] NVARCHAR(20),
  [country_sales_tax_number] NVARCHAR(20),
  [reference_code] NVARCHAR(16),
  [cancelled_flag] NVARCHAR(1),
  [exchange_rate_date] DATETIME,
  [payment_reference] NVARCHAR(30),
  [reversal_reason] NVARCHAR(2),
  [posted_by] NVARCHAR(6),
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #item (
  [rn] INT,
  [id] BIGINT,
  [sales_invoice_id] BIGINT,
  [plant_code] NVARCHAR(4),
  [billing_document_code] NVARCHAR(10),
  [line_number] INT,
  [billing_quantity] DECIMAL(18,3),
  [sales_unit] NVARCHAR(3),
  [base_unit] NVARCHAR(3),
  [scale_quantity] DECIMAL(18,3),
  [billing_quantity_sku] DECIMAL(18,3),
  [net_weight] DECIMAL(18,3),
  [gross_weight] DECIMAL(18,3),
  [weight_unit] NVARCHAR(3),
  [business_area_code] NVARCHAR(4),
  [pricing_date] DATETIME,
  [service_rendered_date] DATETIME,
  [pricing_exchange_rate] DECIMAL(18,5),
  [net_amount] DECIMAL(18,2),
  [reference_document_code] NVARCHAR(10),
  [reference_document_item] INT,
  [reference_document_category] NVARCHAR(1),
  [sales_document_code] NVARCHAR(20),
  [sales_document_item] INT,
  [sales_document_reference_flag] NVARCHAR(1),
  [material_code] NVARCHAR(20),
  [material_description] NVARCHAR(40),
  [pricing_reference_material_code] NVARCHAR(20),
  [batch_code] NVARCHAR(10),
  [material_group] NVARCHAR(9),
  [sales_item_category] NVARCHAR(4),
  [product_hierarchy] NVARCHAR(18),
  [shipping_point] NVARCHAR(4),
  [division] NVARCHAR(2),
  [partner_item] INT,
  [departure_country] NVARCHAR(3),
  [plant_region] NVARCHAR(3),
  [pricing_flag] NVARCHAR(1),
  [warehouse_code] NVARCHAR(4),
  [cost_amount] DECIMAL(18,2),
  [subtotal1] DECIMAL(18,2),
  [subtotal2] DECIMAL(18,2),
  [subtotal3] DECIMAL(18,2),
  [subtotal4] DECIMAL(18,2),
  [subtotal5] DECIMAL(18,2),
  [subtotal6] DECIMAL(18,2),
  [statistics_exchange_rate] DECIMAL(18,5),
  [profit_center_code] NVARCHAR(10),
  [credit_price] DECIMAL(18,2),
  [customer_group_sales_order] NVARCHAR(2),
  [destination_country_order] NVARCHAR(3),
  [region_order] NVARCHAR(3),
  [sales_organization_order] NVARCHAR(4),
  [distribution_channel_order] NVARCHAR(2),
  [document_category] NVARCHAR(1),
  [tax_amount] DECIMAL(18,2),
  [gross_amount] DECIMAL(18,2),
  [exchange_rate_date] DATETIME,
  [is_obsolete] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [billing_document_code] NVARCHAR(10)
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [billing_document_code] NVARCHAR(10), [line_number] INT
);
CREATE TABLE #hdr_soft (
  [id] BIGINT,
  [billing_document_code] NVARCHAR(10)
);
CREATE TABLE #item_soft (
  [id] BIGINT,
  [billing_document_code] NVARCHAR(10),
  [line_number] INT
);

INSERT INTO #hdr
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[billing_document_code],
  S.[billing_type],
  S.[billing_category],
  S.[document_category],
  S.[currency_code],
  S.[sales_organization],
  S.[distribution_channel],
  S.[pricing_procedure],
  S.[condition_code],
  S.[shipping_conditions],
  S.[billing_date],
  S.[customer_group],
  S.[incoterms1],
  S.[incoterms2],
  S.[posting_status],
  S.[accounting_exchange_rate],
  S.[payment_terms],
  S.[account_assignment_group],
  S.[country_code],
  S.[net_amount],
  S.[payer_code],
  S.[customer_code],
  S.[statistics_currency_code],
  S.[foreign_trade_code],
  S.[cancelled_billing_document],
  S.[invoice_list_type],
  S.[division],
  S.[hierarchy_type_pricing],
  S.[trading_partner],
  S.[tax_departure_country],
  S.[organization_sales_tax_number],
  S.[country_sales_tax_number],
  S.[reference_code],
  S.[cancelled_flag],
  S.[exchange_rate_date],
  S.[payment_reference],
  S.[reversal_reason],
  S.[posted_by],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[billing_document_code]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'') AS [billing_document_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4), N''), @plant_code) AS [plant_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3), N''), @tenant_code) AS [tenant_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4), N''), @company_code) AS [company_code],
      COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(R.[culture_code], N''))), N''), @culture_code) AS [culture_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[billing_type])), 4), N'') AS [billing_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[billing_category])), 1), N'') AS [billing_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_category])), 1), N'') AS [document_category],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'') AS [currency_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_organization])), 4), N'') AS [sales_organization],
      NULLIF(LEFT(LTRIM(RTRIM(R.[distribution_channel])), 2), N'') AS [distribution_channel],
      NULLIF(LEFT(LTRIM(RTRIM(R.[pricing_procedure])), 6), N'') AS [pricing_procedure],
      NULLIF(LEFT(LTRIM(RTRIM(R.[condition_code])), 10), N'') AS [condition_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[shipping_conditions])), 2), N'') AS [shipping_conditions],
      ISNULL(TRY_CAST(R.[billing_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [billing_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_group])), 2), N'') AS [customer_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[incoterms1])), 3), N'') AS [incoterms1],
      NULLIF(LEFT(LTRIM(RTRIM(R.[incoterms2])), 28), N'') AS [incoterms2],
      NULLIF(LEFT(LTRIM(RTRIM(R.[posting_status])), 1), N'') AS [posting_status],
      ROUND(TRY_CAST(R.[accounting_exchange_rate] AS DECIMAL(18,5)), 5) AS [accounting_exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_terms])), 4), N'') AS [payment_terms],
      NULLIF(LEFT(LTRIM(RTRIM(R.[account_assignment_group])), 2), N'') AS [account_assignment_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[country_code])), 3), N'') AS [country_code],
      ISNULL(ROUND(TRY_CAST(R.[net_amount] AS DECIMAL(18,2)), 2), 0) AS [net_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payer_code])), 10), N'') AS [payer_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[customer_code])), 10), N''), N'') AS [customer_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[statistics_currency_code])), 3), N'') AS [statistics_currency_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[foreign_trade_code])), 10), N'') AS [foreign_trade_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cancelled_billing_document])), 10), N'') AS [cancelled_billing_document],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_list_type])), 4), N'') AS [invoice_list_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[division])), 2), N'') AS [division],
      NULLIF(LEFT(LTRIM(RTRIM(R.[hierarchy_type_pricing])), 1), N'') AS [hierarchy_type_pricing],
      NULLIF(LEFT(LTRIM(RTRIM(R.[trading_partner])), 6), N'') AS [trading_partner],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_departure_country])), 3), N'') AS [tax_departure_country],
      NULLIF(LEFT(LTRIM(RTRIM(R.[organization_sales_tax_number])), 20), N'') AS [organization_sales_tax_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[country_sales_tax_number])), 20), N'') AS [country_sales_tax_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cancelled_flag])), 1), N'') AS [cancelled_flag],
      TRY_CAST(R.[exchange_rate_date] AS DATETIME) AS [exchange_rate_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[payment_reference])), 30), N'') AS [payment_reference],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reversal_reason])), 2), N'') AS [reversal_reason],
      -- 源主表旧暂存可能无 posted_by；同批 ALTER 不可见，装入恒 NULL（MERGE 不回写覆盖）
      CAST(NULL AS NVARCHAR(6)) AS [posted_by],
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
          COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3), N''), @tenant_code),
          COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4), N''), @company_code),
          LTRIM(RTRIM(R.[billing_document_code]))
        ORDER BY R.[id]
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[billing_document_code], N''))) <> N''
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3), N''), @tenant_code) <> N''
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4), N''), @company_code) <> N''
      AND COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(R.[culture_code], N''))), N''), @culture_code) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

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
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3), N''), @tenant_code) AS [tenant_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4), N''), @company_code) AS [company_code],
      LTRIM(RTRIM(R.[billing_document_code])) AS [billing_document_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[billing_document_code], N''))) <> N''
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3), N''), @tenant_code) <> N''
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4), N''), @company_code) <> N''
      AND COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(R.[culture_code], N''))), N''), @culture_code) <> N''
    GROUP BY
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3), N''), @tenant_code),
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4), N''), @company_code),
      LTRIM(RTRIM(R.[billing_document_code]))
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(
    N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #hdr), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_sales_invoice] AS T
USING (SELECT * FROM #hdr WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code]
 AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[billing_document_code])) = S.[billing_document_code]
WHEN MATCHED AND (
  ISNULL(T.[billing_type], N'') <> ISNULL(S.[billing_type], N'')
  OR ISNULL(T.[billing_category], N'') <> ISNULL(S.[billing_category], N'')
  OR ISNULL(T.[document_category], N'') <> ISNULL(S.[document_category], N'')
  OR ISNULL(T.[currency_code], N'') <> ISNULL(S.[currency_code], N'')
  OR ISNULL(T.[sales_organization], N'') <> ISNULL(S.[sales_organization], N'')
  OR ISNULL(T.[distribution_channel], N'') <> ISNULL(S.[distribution_channel], N'')
  OR ISNULL(T.[pricing_procedure], N'') <> ISNULL(S.[pricing_procedure], N'')
  OR ISNULL(T.[condition_code], N'') <> ISNULL(S.[condition_code], N'')
  OR ISNULL(T.[shipping_conditions], N'') <> ISNULL(S.[shipping_conditions], N'')
  OR ISNULL(T.[billing_date], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[billing_date], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[customer_group], N'') <> ISNULL(S.[customer_group], N'')
  OR ISNULL(T.[incoterms1], N'') <> ISNULL(S.[incoterms1], N'')
  OR ISNULL(T.[incoterms2], N'') <> ISNULL(S.[incoterms2], N'')
  OR ISNULL(T.[posting_status], N'') <> ISNULL(S.[posting_status], N'')
  OR ISNULL(T.[accounting_exchange_rate], -1) <> ISNULL(S.[accounting_exchange_rate], -1)
  OR ISNULL(T.[payment_terms], N'') <> ISNULL(S.[payment_terms], N'')
  OR ISNULL(T.[account_assignment_group], N'') <> ISNULL(S.[account_assignment_group], N'')
  OR ISNULL(T.[country_code], N'') <> ISNULL(S.[country_code], N'')
  OR ISNULL(T.[net_amount], -1) <> ISNULL(S.[net_amount], -1)
  OR ISNULL(T.[payer_code], N'') <> ISNULL(S.[payer_code], N'')
  OR ISNULL(T.[customer_code], N'') <> ISNULL(S.[customer_code], N'')
  OR ISNULL(T.[statistics_currency_code], N'') <> ISNULL(S.[statistics_currency_code], N'')
  OR ISNULL(T.[foreign_trade_code], N'') <> ISNULL(S.[foreign_trade_code], N'')
  OR ISNULL(T.[cancelled_billing_document], N'') <> ISNULL(S.[cancelled_billing_document], N'')
  OR ISNULL(T.[invoice_list_type], N'') <> ISNULL(S.[invoice_list_type], N'')
  OR ISNULL(T.[division], N'') <> ISNULL(S.[division], N'')
  OR ISNULL(T.[hierarchy_type_pricing], N'') <> ISNULL(S.[hierarchy_type_pricing], N'')
  OR ISNULL(T.[trading_partner], N'') <> ISNULL(S.[trading_partner], N'')
  OR ISNULL(T.[tax_departure_country], N'') <> ISNULL(S.[tax_departure_country], N'')
  OR ISNULL(T.[organization_sales_tax_number], N'') <> ISNULL(S.[organization_sales_tax_number], N'')
  OR ISNULL(T.[country_sales_tax_number], N'') <> ISNULL(S.[country_sales_tax_number], N'')
  OR ISNULL(T.[reference_code], N'') <> ISNULL(S.[reference_code], N'')
  OR ISNULL(T.[cancelled_flag], N'') <> ISNULL(S.[cancelled_flag], N'')
  OR ISNULL(T.[exchange_rate_date], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[exchange_rate_date], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[payment_reference], N'') <> ISNULL(S.[payment_reference], N'')
  OR ISNULL(T.[reversal_reason], N'') <> ISNULL(S.[reversal_reason], N'')
  OR ISNULL(T.[plant_code], N'') <> ISNULL(S.[plant_code], N'')
  OR ISNULL(T.[culture_code], N'') <> ISNULL(S.[culture_code], N'')
  OR T.[is_deleted] <> S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[billing_type]=S.[billing_type],
  T.[billing_category]=S.[billing_category],
  T.[document_category]=S.[document_category],
  T.[currency_code]=S.[currency_code],
  T.[sales_organization]=S.[sales_organization],
  T.[distribution_channel]=S.[distribution_channel],
  T.[pricing_procedure]=S.[pricing_procedure],
  T.[condition_code]=S.[condition_code],
  T.[shipping_conditions]=S.[shipping_conditions],
  T.[billing_date]=S.[billing_date],
  T.[customer_group]=S.[customer_group],
  T.[incoterms1]=S.[incoterms1],
  T.[incoterms2]=S.[incoterms2],
  T.[posting_status]=S.[posting_status],
  T.[accounting_exchange_rate]=S.[accounting_exchange_rate],
  T.[payment_terms]=S.[payment_terms],
  T.[account_assignment_group]=S.[account_assignment_group],
  T.[country_code]=S.[country_code],
  T.[net_amount]=S.[net_amount],
  T.[payer_code]=S.[payer_code],
  T.[customer_code]=S.[customer_code],
  T.[statistics_currency_code]=S.[statistics_currency_code],
  T.[foreign_trade_code]=S.[foreign_trade_code],
  T.[cancelled_billing_document]=S.[cancelled_billing_document],
  T.[invoice_list_type]=S.[invoice_list_type],
  T.[division]=S.[division],
  T.[hierarchy_type_pricing]=S.[hierarchy_type_pricing],
  T.[trading_partner]=S.[trading_partner],
  T.[tax_departure_country]=S.[tax_departure_country],
  T.[organization_sales_tax_number]=S.[organization_sales_tax_number],
  T.[country_sales_tax_number]=S.[country_sales_tax_number],
  T.[reference_code]=S.[reference_code],
  T.[cancelled_flag]=S.[cancelled_flag],
  T.[exchange_rate_date]=S.[exchange_rate_date],
  T.[payment_reference]=S.[payment_reference],
  T.[reversal_reason]=S.[reversal_reason],
  T.[plant_code]=S.[plant_code],
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
  INSERT ([id],[plant_code],[billing_document_code],[billing_type],[billing_category],[document_category],[currency_code],[sales_organization],[distribution_channel],[pricing_procedure],[condition_code],[shipping_conditions],[billing_date],[customer_group],[incoterms1],[incoterms2],[posting_status],[accounting_exchange_rate],[payment_terms],[account_assignment_group],[country_code],[net_amount],[payer_code],[customer_code],[statistics_currency_code],[foreign_trade_code],[cancelled_billing_document],[invoice_list_type],[division],[hierarchy_type_pricing],[trading_partner],[tax_departure_country],[organization_sales_tax_number],[country_sales_tax_number],[reference_code],[cancelled_flag],[exchange_rate_date],[payment_reference],[reversal_reason],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[plant_code],S.[billing_document_code],S.[billing_type],S.[billing_category],S.[document_category],S.[currency_code],S.[sales_organization],S.[distribution_channel],S.[pricing_procedure],S.[condition_code],S.[shipping_conditions],S.[billing_date],S.[customer_group],S.[incoterms1],S.[incoterms2],S.[posting_status],S.[accounting_exchange_rate],S.[payment_terms],S.[account_assignment_group],S.[country_code],S.[net_amount],S.[payer_code],S.[customer_code],S.[statistics_currency_code],S.[foreign_trade_code],S.[cancelled_billing_document],S.[invoice_list_type],S.[division],S.[hierarchy_type_pricing],S.[trading_partner],S.[tax_departure_country],S.[organization_sales_tax_number],S.[country_sales_tax_number],S.[reference_code],S.[cancelled_flag],S.[exchange_rate_date],S.[payment_reference],S.[reversal_reason],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[billing_document_code]
INTO #hdr_delta (rn, oper_type, id, [billing_document_code]);
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

UPDATE S SET S.[id] = T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_sales_invoice] T
  ON T.[tenant_code]=S.[tenant_code]
 AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[billing_document_code])) = S.[billing_document_code];


DECLARE @hdr_delete INT = 0;
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
OUTPUT INSERTED.[id], INSERTED.[billing_document_code]
INTO #hdr_soft ([id], [billing_document_code])
FROM [takt_logistics_sales_invoice] T
WHERE T.[is_deleted] = 0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #hdr S WHERE S.[id] = T.[id]);
  SET @dml_n = @@ROWCOUNT;
  SET @hdr_delete = @hdr_delete + @dml_n;
END

-- 源库回填：明细 sales_invoice_id → 主表雪花 id（业务键 billing_document_code）
UPDATE I
SET I.[sales_invoice_id] = H.[id]
FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice_item] AS I
INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] AS H
  ON LTRIM(RTRIM(H.[billing_document_code])) = LTRIM(RTRIM(I.[billing_document_code]))
WHERE I.[sales_invoice_id] IS NULL OR I.[sales_invoice_id] <> H.[id];

INSERT INTO #item
SELECT
  S.rn,
  @base_id + 1000000000 + S.rn,
  0,
  S.[plant_code],
  S.[billing_document_code],
  S.[line_number],
  S.[billing_quantity],
  S.[sales_unit],
  S.[base_unit],
  S.[scale_quantity],
  S.[billing_quantity_sku],
  S.[net_weight],
  S.[gross_weight],
  S.[weight_unit],
  S.[business_area_code],
  S.[pricing_date],
  S.[service_rendered_date],
  S.[pricing_exchange_rate],
  S.[net_amount],
  S.[reference_document_code],
  S.[reference_document_item],
  S.[reference_document_category],
  S.[sales_document_code],
  S.[sales_document_item],
  S.[sales_document_reference_flag],
  S.[material_code],
  S.[material_description],
  S.[pricing_reference_material_code],
  S.[batch_code],
  S.[material_group],
  S.[sales_item_category],
  S.[product_hierarchy],
  S.[shipping_point],
  S.[division],
  S.[partner_item],
  S.[departure_country],
  S.[plant_region],
  S.[pricing_flag],
  S.[warehouse_code],
  S.[cost_amount],
  S.[subtotal1],
  S.[subtotal2],
  S.[subtotal3],
  S.[subtotal4],
  S.[subtotal5],
  S.[subtotal6],
  S.[statistics_exchange_rate],
  S.[profit_center_code],
  S.[credit_price],
  S.[customer_group_sales_order],
  S.[destination_country_order],
  S.[region_order],
  S.[sales_organization_order],
  S.[distribution_channel_order],
  S.[document_category],
  S.[tax_amount],
  S.[gross_amount],
  S.[exchange_rate_date],
  S.[is_obsolete],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[billing_document_code], N.[line_number]) AS rn
  FROM (
    SELECT
      -- 与 #hdr 对齐：单据码/租户/公司取自源主表 SH（空则任务参数）；勿优先明细行隔离列，否则 JOIN #hdr 失败
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[billing_document_code])), 10), N''),
        ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'')) AS [billing_document_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      COALESCE(
        NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4), N''),
        NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[plant_code], N''))), 4), N''),
        @plant_code
      ) AS [plant_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[tenant_code], N''))), 3), N''), @tenant_code) AS [tenant_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[company_code], N''))), 4), N''), @company_code) AS [company_code],
      COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(SH.[culture_code], N''))), N''), @culture_code) AS [culture_code],
      ROUND(TRY_CAST(R.[billing_quantity] AS DECIMAL(18,3)), 3) AS [billing_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_unit])), 3), N'') AS [sales_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N'') AS [base_unit],
      ROUND(TRY_CAST(R.[scale_quantity] AS DECIMAL(18,3)), 3) AS [scale_quantity],
      ROUND(TRY_CAST(R.[billing_quantity_sku] AS DECIMAL(18,3)), 3) AS [billing_quantity_sku],
      ROUND(TRY_CAST(R.[net_weight] AS DECIMAL(18,3)), 3) AS [net_weight],
      ROUND(TRY_CAST(R.[gross_weight] AS DECIMAL(18,3)), 3) AS [gross_weight],
      NULLIF(LEFT(LTRIM(RTRIM(R.[weight_unit])), 3), N'') AS [weight_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[business_area_code])), 4), N'') AS [business_area_code],
      TRY_CAST(R.[pricing_date] AS DATETIME) AS [pricing_date],
      TRY_CAST(R.[service_rendered_date] AS DATETIME) AS [service_rendered_date],
      ROUND(TRY_CAST(R.[pricing_exchange_rate] AS DECIMAL(18,5)), 5) AS [pricing_exchange_rate],
      ROUND(TRY_CAST(R.[net_amount] AS DECIMAL(18,2)), 2) AS [net_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_code])), 10), N'') AS [reference_document_code],
      TRY_CAST(R.[reference_document_item] AS INT) AS [reference_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_category])), 1), N'') AS [reference_document_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_document_code])), 20), N'') AS [sales_document_code],
      TRY_CAST(R.[sales_document_item] AS INT) AS [sales_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_document_reference_flag])), 1), N'') AS [sales_document_reference_flag],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N'')))) = 18
          AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'')
      END AS [material_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[material_description], N''))), 40) AS [material_description],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[pricing_reference_material_code], N'')))) = 18
          AND LTRIM(RTRIM(R.[pricing_reference_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[pricing_reference_material_code])), 10)
        ELSE NULLIF(LEFT(LTRIM(RTRIM(R.[pricing_reference_material_code])), 20), N'')
      END AS [pricing_reference_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[batch_code])), 10), N'') AS [batch_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_group])), 9), N'') AS [material_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_item_category])), 4), N'') AS [sales_item_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[product_hierarchy])), 18), N'') AS [product_hierarchy],
      NULLIF(LEFT(LTRIM(RTRIM(R.[shipping_point])), 4), N'') AS [shipping_point],
      NULLIF(LEFT(LTRIM(RTRIM(R.[division])), 2), N'') AS [division],
      TRY_CAST(R.[partner_item] AS INT) AS [partner_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[departure_country])), 3), N'') AS [departure_country],
      NULLIF(LEFT(LTRIM(RTRIM(R.[plant_region])), 3), N'') AS [plant_region],
      NULLIF(LEFT(LTRIM(RTRIM(R.[pricing_flag])), 1), N'') AS [pricing_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[warehouse_code])), 4), N'') AS [warehouse_code],
      ROUND(TRY_CAST(R.[cost_amount] AS DECIMAL(18,2)), 2) AS [cost_amount],
      ROUND(TRY_CAST(R.[subtotal1] AS DECIMAL(18,2)), 2) AS [subtotal1],
      ROUND(TRY_CAST(R.[subtotal2] AS DECIMAL(18,2)), 2) AS [subtotal2],
      ROUND(TRY_CAST(R.[subtotal3] AS DECIMAL(18,2)), 2) AS [subtotal3],
      ROUND(TRY_CAST(R.[subtotal4] AS DECIMAL(18,2)), 2) AS [subtotal4],
      ROUND(TRY_CAST(R.[subtotal5] AS DECIMAL(18,2)), 2) AS [subtotal5],
      ROUND(TRY_CAST(R.[subtotal6] AS DECIMAL(18,2)), 2) AS [subtotal6],
      ROUND(TRY_CAST(R.[statistics_exchange_rate] AS DECIMAL(18,5)), 5) AS [statistics_exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[profit_center_code])), 10), N'') AS [profit_center_code],
      ROUND(TRY_CAST(R.[credit_price] AS DECIMAL(18,2)), 2) AS [credit_price],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_group_sales_order])), 2), N'') AS [customer_group_sales_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[destination_country_order])), 3), N'') AS [destination_country_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[region_order])), 3), N'') AS [region_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[sales_organization_order])), 4), N'') AS [sales_organization_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[distribution_channel_order])), 2), N'') AS [distribution_channel_order],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_category])), 1), N'') AS [document_category],
      ROUND(TRY_CAST(R.[tax_amount] AS DECIMAL(18,2)), 2) AS [tax_amount],
      ROUND(TRY_CAST(R.[gross_amount] AS DECIMAL(18,2)), 2) AS [gross_amount],
      TRY_CAST(R.[exchange_rate_date] AS DATETIME) AS [exchange_rate_date],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
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
          COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[tenant_code], N''))), 3), N''), @tenant_code),
          COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[company_code], N''))), 4), N''), @company_code),
          ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[billing_document_code])), 10), N''),
            ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'')),
          COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY R.[id]
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] SH
      ON SH.[id] = R.[sales_invoice_id]
    WHERE COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
      AND ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[billing_document_code])), 10), N''),
            NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N'')) IS NOT NULL
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[tenant_code], N''))), 3), N''), @tenant_code) <> N''
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[company_code], N''))), 4), N''), @company_code) <> N''
      AND COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(SH.[culture_code], N''))), N''), @culture_code) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

UPDATE I
SET I.[sales_invoice_id] = H.[id],
    I.[id] = COALESCE(T.[id], I.[id])
FROM #item I
INNER JOIN #hdr H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND H.[billing_document_code] = I.[billing_document_code]
LEFT JOIN [takt_logistics_sales_invoice_item] T
  ON T.[tenant_code]=I.[tenant_code]
 AND T.[company_code]=I.[company_code]
 AND T.[sales_invoice_id] = H.[id]
 AND T.[line_number] = I.[line_number];

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [sales_invoice_id] <> 0);
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
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[tenant_code], N''))), 3), N''), @tenant_code) AS [tenant_code],
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[company_code], N''))), 4), N''), @company_code) AS [company_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[billing_document_code])), 10), N''),
        ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'')) AS [billing_document_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice] SH
      ON SH.[id] = R.[sales_invoice_id]
    WHERE COALESCE(TRY_CAST(R.[line_number] AS INT), 0) > 0
      AND ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[billing_document_code])), 10), N''),
            NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N'')) IS NOT NULL
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[tenant_code], N''))), 3), N''), @tenant_code) <> N''
      AND COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[company_code], N''))), 4), N''), @company_code) <> N''
      AND COALESCE(NULLIF(LTRIM(RTRIM(ISNULL(SH.[culture_code], N''))), N''), @culture_code) <> N''
    GROUP BY
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[tenant_code], N''))), 3), N''), @tenant_code),
      COALESCE(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(SH.[company_code], N''))), 4), N''), @company_code),
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[billing_document_code])), 10), N''),
        ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[billing_document_code])), 10), N''), N'')),
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(
    N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;

DELETE FROM #item WHERE [sales_invoice_id] = 0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #item), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_sales_invoice_item] AS T
USING (SELECT * FROM #item WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code]
 AND T.[company_code]=S.[company_code]
 AND T.[sales_invoice_id] = S.[sales_invoice_id]
 AND T.[line_number] = S.[line_number]
WHEN MATCHED AND (
  ISNULL(T.[plant_code], N'') <> ISNULL(S.[plant_code], N'')
  OR ISNULL(T.[billing_document_code], N'') <> ISNULL(S.[billing_document_code], N'')
  OR ISNULL(T.[billing_quantity], -1) <> ISNULL(S.[billing_quantity], -1)
  OR ISNULL(T.[sales_unit], N'') <> ISNULL(S.[sales_unit], N'')
  OR ISNULL(T.[base_unit], N'') <> ISNULL(S.[base_unit], N'')
  OR ISNULL(T.[scale_quantity], -1) <> ISNULL(S.[scale_quantity], -1)
  OR ISNULL(T.[billing_quantity_sku], -1) <> ISNULL(S.[billing_quantity_sku], -1)
  OR ISNULL(T.[net_weight], -1) <> ISNULL(S.[net_weight], -1)
  OR ISNULL(T.[gross_weight], -1) <> ISNULL(S.[gross_weight], -1)
  OR ISNULL(T.[weight_unit], N'') <> ISNULL(S.[weight_unit], N'')
  OR ISNULL(T.[business_area_code], N'') <> ISNULL(S.[business_area_code], N'')
  OR ISNULL(T.[pricing_date], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[pricing_date], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[service_rendered_date], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[service_rendered_date], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[pricing_exchange_rate], -1) <> ISNULL(S.[pricing_exchange_rate], -1)
  OR ISNULL(T.[net_amount], -1) <> ISNULL(S.[net_amount], -1)
  OR ISNULL(T.[reference_document_code], N'') <> ISNULL(S.[reference_document_code], N'')
  OR ISNULL(T.[reference_document_item], -1) <> ISNULL(S.[reference_document_item], -1)
  OR ISNULL(T.[reference_document_category], N'') <> ISNULL(S.[reference_document_category], N'')
  OR ISNULL(T.[sales_document_code], N'') <> ISNULL(S.[sales_document_code], N'')
  OR ISNULL(T.[sales_document_item], -1) <> ISNULL(S.[sales_document_item], -1)
  OR ISNULL(T.[sales_document_reference_flag], N'') <> ISNULL(S.[sales_document_reference_flag], N'')
  OR ISNULL(T.[material_code], N'') <> ISNULL(S.[material_code], N'')
  OR ISNULL(T.[material_description], N'') <> ISNULL(S.[material_description], N'')
  OR ISNULL(T.[pricing_reference_material_code], N'') <> ISNULL(S.[pricing_reference_material_code], N'')
  OR ISNULL(T.[batch_code], N'') <> ISNULL(S.[batch_code], N'')
  OR ISNULL(T.[material_group], N'') <> ISNULL(S.[material_group], N'')
  OR ISNULL(T.[sales_item_category], N'') <> ISNULL(S.[sales_item_category], N'')
  OR ISNULL(T.[product_hierarchy], N'') <> ISNULL(S.[product_hierarchy], N'')
  OR ISNULL(T.[shipping_point], N'') <> ISNULL(S.[shipping_point], N'')
  OR ISNULL(T.[division], N'') <> ISNULL(S.[division], N'')
  OR ISNULL(T.[partner_item], -1) <> ISNULL(S.[partner_item], -1)
  OR ISNULL(T.[departure_country], N'') <> ISNULL(S.[departure_country], N'')
  OR ISNULL(T.[plant_region], N'') <> ISNULL(S.[plant_region], N'')
  OR ISNULL(T.[pricing_flag], N'') <> ISNULL(S.[pricing_flag], N'')
  OR ISNULL(T.[warehouse_code], N'') <> ISNULL(S.[warehouse_code], N'')
  OR ISNULL(T.[cost_amount], -1) <> ISNULL(S.[cost_amount], -1)
  OR ISNULL(T.[subtotal1], -1) <> ISNULL(S.[subtotal1], -1)
  OR ISNULL(T.[subtotal2], -1) <> ISNULL(S.[subtotal2], -1)
  OR ISNULL(T.[subtotal3], -1) <> ISNULL(S.[subtotal3], -1)
  OR ISNULL(T.[subtotal4], -1) <> ISNULL(S.[subtotal4], -1)
  OR ISNULL(T.[subtotal5], -1) <> ISNULL(S.[subtotal5], -1)
  OR ISNULL(T.[subtotal6], -1) <> ISNULL(S.[subtotal6], -1)
  OR ISNULL(T.[statistics_exchange_rate], -1) <> ISNULL(S.[statistics_exchange_rate], -1)
  OR ISNULL(T.[profit_center_code], N'') <> ISNULL(S.[profit_center_code], N'')
  OR ISNULL(T.[credit_price], -1) <> ISNULL(S.[credit_price], -1)
  OR ISNULL(T.[customer_group_sales_order], N'') <> ISNULL(S.[customer_group_sales_order], N'')
  OR ISNULL(T.[destination_country_order], N'') <> ISNULL(S.[destination_country_order], N'')
  OR ISNULL(T.[region_order], N'') <> ISNULL(S.[region_order], N'')
  OR ISNULL(T.[sales_organization_order], N'') <> ISNULL(S.[sales_organization_order], N'')
  OR ISNULL(T.[distribution_channel_order], N'') <> ISNULL(S.[distribution_channel_order], N'')
  OR ISNULL(T.[document_category], N'') <> ISNULL(S.[document_category], N'')
  OR ISNULL(T.[tax_amount], -1) <> ISNULL(S.[tax_amount], -1)
  OR ISNULL(T.[gross_amount], -1) <> ISNULL(S.[gross_amount], -1)
  OR ISNULL(T.[exchange_rate_date], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[exchange_rate_date], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[is_obsolete], -1) <> ISNULL(S.[is_obsolete], -1)
  OR ISNULL(T.[culture_code], N'') <> ISNULL(S.[culture_code], N'')
  OR T.[is_deleted] <> S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[billing_document_code]=S.[billing_document_code],
  T.[billing_quantity]=S.[billing_quantity],
  T.[sales_unit]=S.[sales_unit],
  T.[base_unit]=S.[base_unit],
  T.[scale_quantity]=S.[scale_quantity],
  T.[billing_quantity_sku]=S.[billing_quantity_sku],
  T.[net_weight]=S.[net_weight],
  T.[gross_weight]=S.[gross_weight],
  T.[weight_unit]=S.[weight_unit],
  T.[business_area_code]=S.[business_area_code],
  T.[pricing_date]=S.[pricing_date],
  T.[service_rendered_date]=S.[service_rendered_date],
  T.[pricing_exchange_rate]=S.[pricing_exchange_rate],
  T.[net_amount]=S.[net_amount],
  T.[reference_document_code]=S.[reference_document_code],
  T.[reference_document_item]=S.[reference_document_item],
  T.[reference_document_category]=S.[reference_document_category],
  T.[sales_document_code]=S.[sales_document_code],
  T.[sales_document_item]=S.[sales_document_item],
  T.[sales_document_reference_flag]=S.[sales_document_reference_flag],
  T.[material_code]=S.[material_code],
  T.[material_description]=S.[material_description],
  T.[pricing_reference_material_code]=S.[pricing_reference_material_code],
  T.[batch_code]=S.[batch_code],
  T.[material_group]=S.[material_group],
  T.[sales_item_category]=S.[sales_item_category],
  T.[product_hierarchy]=S.[product_hierarchy],
  T.[shipping_point]=S.[shipping_point],
  T.[division]=S.[division],
  T.[partner_item]=S.[partner_item],
  T.[departure_country]=S.[departure_country],
  T.[plant_region]=S.[plant_region],
  T.[pricing_flag]=S.[pricing_flag],
  T.[warehouse_code]=S.[warehouse_code],
  T.[cost_amount]=S.[cost_amount],
  T.[subtotal1]=S.[subtotal1],
  T.[subtotal2]=S.[subtotal2],
  T.[subtotal3]=S.[subtotal3],
  T.[subtotal4]=S.[subtotal4],
  T.[subtotal5]=S.[subtotal5],
  T.[subtotal6]=S.[subtotal6],
  T.[statistics_exchange_rate]=S.[statistics_exchange_rate],
  T.[profit_center_code]=S.[profit_center_code],
  T.[credit_price]=S.[credit_price],
  T.[customer_group_sales_order]=S.[customer_group_sales_order],
  T.[destination_country_order]=S.[destination_country_order],
  T.[region_order]=S.[region_order],
  T.[sales_organization_order]=S.[sales_organization_order],
  T.[distribution_channel_order]=S.[distribution_channel_order],
  T.[document_category]=S.[document_category],
  T.[tax_amount]=S.[tax_amount],
  T.[gross_amount]=S.[gross_amount],
  T.[exchange_rate_date]=S.[exchange_rate_date],
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
WHEN NOT MATCHED THEN
  INSERT ([id],[sales_invoice_id],[plant_code],[billing_document_code],[line_number],[billing_quantity],[sales_unit],[base_unit],[scale_quantity],[billing_quantity_sku],[net_weight],[gross_weight],[weight_unit],[business_area_code],[pricing_date],[service_rendered_date],[pricing_exchange_rate],[net_amount],[reference_document_code],[reference_document_item],[reference_document_category],[sales_document_code],[sales_document_item],[sales_document_reference_flag],[material_code],[material_description],[pricing_reference_material_code],[batch_code],[material_group],[sales_item_category],[product_hierarchy],[shipping_point],[division],[partner_item],[departure_country],[plant_region],[pricing_flag],[warehouse_code],[cost_amount],[subtotal1],[subtotal2],[subtotal3],[subtotal4],[subtotal5],[subtotal6],[statistics_exchange_rate],[profit_center_code],[credit_price],[customer_group_sales_order],[destination_country_order],[region_order],[sales_organization_order],[distribution_channel_order],[document_category],[tax_amount],[gross_amount],[exchange_rate_date],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[sales_invoice_id],S.[plant_code],S.[billing_document_code],S.[line_number],S.[billing_quantity],S.[sales_unit],S.[base_unit],S.[scale_quantity],S.[billing_quantity_sku],S.[net_weight],S.[gross_weight],S.[weight_unit],S.[business_area_code],S.[pricing_date],S.[service_rendered_date],S.[pricing_exchange_rate],S.[net_amount],S.[reference_document_code],S.[reference_document_item],S.[reference_document_category],S.[sales_document_code],S.[sales_document_item],S.[sales_document_reference_flag],S.[material_code],S.[material_description],S.[pricing_reference_material_code],S.[batch_code],S.[material_group],S.[sales_item_category],S.[product_hierarchy],S.[shipping_point],S.[division],S.[partner_item],S.[departure_country],S.[plant_region],S.[pricing_flag],S.[warehouse_code],S.[cost_amount],S.[subtotal1],S.[subtotal2],S.[subtotal3],S.[subtotal4],S.[subtotal5],S.[subtotal6],S.[statistics_exchange_rate],S.[profit_center_code],S.[credit_price],S.[customer_group_sales_order],S.[destination_country_order],S.[region_order],S.[sales_organization_order],S.[distribution_channel_order],S.[document_category],S.[tax_amount],S.[gross_amount],S.[exchange_rate_date],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[billing_document_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [billing_document_code], [line_number]);
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

UPDATE I SET I.[id] = T.[id]
FROM #item I
INNER JOIN [takt_logistics_sales_invoice_item] T
  ON T.[tenant_code]=I.[tenant_code]
 AND T.[company_code]=I.[company_code]
 AND T.[sales_invoice_id] = I.[sales_invoice_id]
 AND T.[line_number] = I.[line_number];


DECLARE @item_delete INT = 0;
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
OUTPUT INSERTED.[id], INSERTED.[billing_document_code], INSERTED.[line_number]
INTO #item_soft ([id], [billing_document_code], [line_number])
FROM [takt_logistics_sales_invoice_item] T
WHERE T.[is_deleted] = 0
  AND EXISTS (SELECT 1 FROM #item S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[id] = T.[id]);
  SET @dml_n = @@ROWCOUNT;
  SET @item_delete = @item_delete + @dml_n;
END

DECLARE @hdr_source_active INT = (SELECT COUNT(*) FROM #hdr WHERE [is_deleted]=0);
DECLARE @item_source_active INT = (SELECT COUNT(*) FROM #item WHERE [is_deleted]=0);
DECLARE @hdr_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_invoice] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_invoice_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

IF @hdr_after <> @hdr_source_active
BEGIN
  DECLARE @hdr_cnt NVARCHAR(200) = CONCAT(
    N'主表有效行数不一致: source=',@hdr_source_active, N', active=', @hdr_after);
  THROW 50002, @hdr_cnt, 1;
END;
IF @item_after <> @item_source_active
BEGIN
  DECLARE @item_cnt NVARCHAR(200) = CONCAT(
    N'明细有效行数不一致: source=', @item_source_active, N', active=', @item_after);
  THROW 50002, @item_cnt, 1;
END;

DECLARE @hdr_ins INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type = N'INSERT');
DECLARE @hdr_upd INT = (SELECT COUNT(*) FROM #hdr_delta WHERE oper_type = N'UPDATE');
DECLARE @hdr_unchanged INT = @hdr_source - @hdr_ins - @hdr_upd;
DECLARE @item_ins INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type = N'INSERT');
DECLARE @item_upd INT = (SELECT COUNT(*) FROM #item_delta WHERE oper_type = N'UPDATE');
DECLARE @item_unchanged INT = @item_source - @item_ins - @item_upd;

DECLARE @hdr_soft_keys NVARCHAR(MAX) = N'';
SELECT @hdr_soft_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([billing_document_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (SELECT TOP (100) * FROM #hdr_soft ORDER BY [id]) SoftSample;
SET @hdr_soft_keys = ISNULL(@hdr_soft_keys, N'');
IF @hdr_delete > 100
  SET @hdr_soft_keys = CONCAT(@hdr_soft_keys, N'; ...(+', CAST(@hdr_delete - 100 AS NVARCHAR(20)), N')');

DECLARE @item_soft_keys NVARCHAR(MAX) = N'';
SELECT @item_soft_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([billing_document_code], N''), N'/', CAST([line_number] AS NVARCHAR(20)))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys = ISNULL(@item_soft_keys, N'');
IF @item_delete > 100
  SET @item_soft_keys = CONCAT(@item_soft_keys, N'; ...(+', CAST(@item_delete - 100 AS NVARCHAR(20)), N')');

DECLARE @hdr_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice]
);
DECLARE @item_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_invoice_item]
);
DECLARE @hdr_physical INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice] T
  WHERE EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_physical INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice_item] T
  WHERE EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @hdr_soft_total INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice] T
  WHERE T.[is_deleted] = 1
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_soft_total INT = (
  SELECT COUNT(*) FROM [takt_logistics_sales_invoice_item] T
  WHERE T.[is_deleted] = 1
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

-- 列名须与 TaktQuartzSqlResultReader 一致（scope/source_count/target_*），宽表 hdr_source 无法解析会误报装入=0
SELECT N'QUARTZ_SYNC_SUMMARY' AS [summary_tag], CAST(N'main' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_raw AS [source_raw_count], @hdr_source AS [source_count],
  @hdr_before AS [target_before], @hdr_after AS [target_after],
  @hdr_physical AS [target_physical], @hdr_soft_total AS [soft_deleted],
  @hdr_ins AS [insert_count], @hdr_upd AS [update_count], @hdr_unchanged AS [unchanged_count],
  @hdr_delete AS [delete_count], @hdr_soft_keys AS [soft_deleted_keys]
UNION ALL
SELECT N'QUARTZ_SYNC_SUMMARY', CAST(N'detail' AS NVARCHAR(40)),
  @item_sap_raw, @item_source,
  @item_before, @item_after,
  @item_physical, @item_soft_total,
  @item_ins, @item_upd, @item_unchanged,
  @item_delete, @item_soft_keys;
