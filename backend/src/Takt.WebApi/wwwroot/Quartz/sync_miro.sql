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
DECLARE @apply_chunk INT = 100000;
DECLARE @merge_from_rn INT;
DECLARE @merge_to_rn INT;
DECLARE @merge_max_rn INT;
DECLARE @dml_n INT;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;
DECLARE @ddl NVARCHAR(500);

-- 源/目标列与实体 TaktPurchaseInvoice / Item 一致
-- plant_code 取自各源表本列（主表 R.plant_code、明细 R.plant_code）；空 plant 丢弃，不回退 @plant_code
-- tenant/company/plant/culture 取自各源表本列；空值丢弃，不回退任务参数
-- 主表唯一键：Tenant+Company+fiscal_year+purchase_invoice_code
-- 明细唯一键：purchase_invoice_id+line_number
-- 源明细 FK：先按 purchase_invoice_code 回填 purchase_invoice_id=主表雪花 id，再 SH.id=R.purchase_invoice_id 装入
-- 源侧同凭证码跨年重复时回填会歧义；当前源库同码无多年（已核对）

IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_invoice', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_invoice] ADD [posted_by] NVARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_purchase_invoice', N'posted_by') IS NULL
BEGIN
  SET @ddl = N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] ADD [posted_by] NVARCHAR(6) NULL';
  EXEC sys.sp_executesql @ddl;
END

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT, [plant_code] NVARCHAR(4),
  [purchase_invoice_code] NVARCHAR(10), [fiscal_year] NVARCHAR(4),
  [document_type] NVARCHAR(2), [document_date] DATETIME, [posting_date] DATETIME,
  [transaction_event_type] NVARCHAR(2), [reference_code] NVARCHAR(16),
  [supplier_code] NVARCHAR(10), [currency_code] NVARCHAR(3), [exchange_rate] DECIMAL(18,5),
  [gross_amount] DECIMAL(18,2), [vat_amount] DECIMAL(18,2),
  [tax_jurisdiction_code] NVARCHAR(15), [cash_discount_days1] INT,
  [invoice_flag] NVARCHAR(1), [header_text] NVARCHAR(25),
  [reversal_document_code] NVARCHAR(10), [reversal_fiscal_year] NVARCHAR(4),
  [tax_code] NVARCHAR(2), [supplying_country] NVARCHAR(3), [tax_exchange_rate] DECIMAL(18,5),
  [baseline_date] DATETIME, [exchange_rate_date] DATETIME,
  [transaction_code] NVARCHAR(40), [posted_by] NVARCHAR(6),
  [tenant_code] NVARCHAR(3), [company_code] NVARCHAR(4), [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [purchase_invoice_id] BIGINT,
  [fiscal_year] NVARCHAR(4), [purchase_invoice_code] NVARCHAR(10), [line_number] INT,
  [plant_code] NVARCHAR(4),
  [purchase_order_code] NVARCHAR(20), [purchase_order_item] INT, [account_assignment_seq] NVARCHAR(2),
  [material_code] NVARCHAR(20), [valuation_area] NVARCHAR(4), [amount] DECIMAL(18,2),
  [debit_credit_indicator] NVARCHAR(1), [tax_code] NVARCHAR(2), [quantity] DECIMAL(18,3),
  [order_unit] NVARCHAR(3), [po_price_quantity] DECIMAL(18,3), [po_price_unit] NVARCHAR(3),
  [valuated_stock_quantity] DECIMAL(18,3), [previous_period_stock] DECIMAL(18,3), [base_unit] NVARCHAR(3),
  [valuation_class] NVARCHAR(4), [update_po_history_flag] NVARCHAR(1), [subsequent_debit_credit] NVARCHAR(1),
  [block_reason_price] NVARCHAR(1), [block_reason_quantity] NVARCHAR(1),
  [block_reason_quality] NVARCHAR(1), [block_reason_enhanced] NVARCHAR(1),
  [value_string] NVARCHAR(4), [reference_code] NVARCHAR(16), [condition_type] NVARCHAR(4),
  [total_valuated_stock_value] DECIMAL(18,2), [previous_period_value] DECIMAL(18,2),
  [reference_document_code] NVARCHAR(10), [reference_document_year] NVARCHAR(4), [reference_document_item] INT,
  [stock_managed_material_code] NVARCHAR(20), [item_text] NVARCHAR(40), [material_document_item] INT,
  [is_obsolete] INT, [tenant_code] NVARCHAR(3), [company_code] NVARCHAR(4), [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #hdr_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, [fiscal_year] NVARCHAR(4), [purchase_invoice_code] NVARCHAR(10));
CREATE TABLE #item_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, [purchase_invoice_code] NVARCHAR(10), [line_number] INT);
CREATE TABLE #hdr_soft ([id] BIGINT, [fiscal_year] NVARCHAR(4), [purchase_invoice_code] NVARCHAR(10));
CREATE TABLE #item_soft ([id] BIGINT, [purchase_invoice_code] NVARCHAR(10), [line_number] INT);

INSERT INTO #hdr
SELECT S.rn, @base_id + S.rn, S.[plant_code],
  S.[purchase_invoice_code], S.[fiscal_year], S.[document_type], S.[document_date], S.[posting_date],
  S.[transaction_event_type], S.[reference_code], S.[supplier_code], S.[currency_code], S.[exchange_rate],
  S.[gross_amount], S.[vat_amount], S.[tax_jurisdiction_code], S.[cash_discount_days1],
  S.[invoice_flag], S.[header_text], S.[reversal_document_code], S.[reversal_fiscal_year],
  S.[tax_code], S.[supplying_country], S.[tax_exchange_rate], S.[baseline_date],
  S.[exchange_rate_date], S.[transaction_code], S.[posted_by],
  S.[tenant_code], S.[company_code], S.[culture_code], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT N.*, ROW_NUMBER() OVER (ORDER BY N.[fiscal_year], N.[purchase_invoice_code]) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_invoice_code])), 10), N''), N'') AS [purchase_invoice_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[fiscal_year])), 4), N''), N'') AS [fiscal_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_type])), 2), N'') AS [document_type],
      ISNULL(TRY_CAST(R.[document_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [document_date],
      ISNULL(TRY_CAST(R.[posting_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [posting_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_event_type])), 2), N'') AS [transaction_event_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[supplier_code])), 10), N''), N'') AS [supplier_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'') AS [currency_code],
      ROUND(TRY_CAST(R.[exchange_rate] AS DECIMAL(18,5)), 5) AS [exchange_rate],
      ISNULL(ROUND(TRY_CAST(R.[gross_amount] AS DECIMAL(18,2)), 2), 0) AS [gross_amount],
      ROUND(TRY_CAST(R.[vat_amount] AS DECIMAL(18,2)), 2) AS [vat_amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_jurisdiction_code])), 15), N'') AS [tax_jurisdiction_code],
      TRY_CAST(R.[cash_discount_days1] AS INT) AS [cash_discount_days1],
      NULLIF(LEFT(LTRIM(RTRIM(R.[invoice_flag])), 1), N'') AS [invoice_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[header_text])), 25), N'') AS [header_text],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reversal_document_code])), 10), N'') AS [reversal_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reversal_fiscal_year])), 4), N'') AS [reversal_fiscal_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_code])), 2), N'') AS [tax_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[supplying_country])), 3), N'') AS [supplying_country],
      ROUND(TRY_CAST(R.[tax_exchange_rate] AS DECIMAL(18,5)), 5) AS [tax_exchange_rate],
      TRY_CAST(R.[baseline_date] AS DATETIME) AS [baseline_date],
      TRY_CAST(R.[exchange_rate_date] AS DATETIME) AS [exchange_rate_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_code])), 40), N'') AS [transaction_code],
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
        PARTITION BY LTRIM(RTRIM(R.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code]))
        ORDER BY LTRIM(RTRIM(R.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
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
  SELECT COUNT(*) FROM (
    SELECT LTRIM(RTRIM(R.[fiscal_year])) AS [fiscal_year], LTRIM(RTRIM(R.[purchase_invoice_code])) AS [purchase_invoice_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] R
    WHERE LTRIM(RTRIM(ISNULL(R.[fiscal_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code]))
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #hdr), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_procurement_purchase_invoice] AS T
USING (SELECT * FROM #hdr WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[fiscal_year]))=S.[fiscal_year]
 AND LTRIM(RTRIM(T.[purchase_invoice_code]))=S.[purchase_invoice_code]
WHEN MATCHED AND (
  ISNULL(T.[document_type],N'')<>ISNULL(S.[document_type],N'')
  OR ISNULL(T.[document_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[document_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[posting_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[posting_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[transaction_event_type],N'')<>ISNULL(S.[transaction_event_type],N'')
  OR ISNULL(T.[reference_code],N'')<>ISNULL(S.[reference_code],N'')
  OR ISNULL(T.[supplier_code],N'')<>ISNULL(S.[supplier_code],N'')
  OR ISNULL(T.[currency_code],N'')<>ISNULL(S.[currency_code],N'')
  OR ISNULL(T.[exchange_rate],-1)<>ISNULL(S.[exchange_rate],-1)
  OR ISNULL(T.[gross_amount],-1)<>ISNULL(S.[gross_amount],-1)
  OR ISNULL(T.[vat_amount],-1)<>ISNULL(S.[vat_amount],-1)
  OR ISNULL(T.[tax_jurisdiction_code],N'')<>ISNULL(S.[tax_jurisdiction_code],N'')
  OR ISNULL(T.[cash_discount_days1],-1)<>ISNULL(S.[cash_discount_days1],-1)
  OR ISNULL(T.[invoice_flag],N'')<>ISNULL(S.[invoice_flag],N'')
  OR ISNULL(T.[header_text],N'')<>ISNULL(S.[header_text],N'')
  OR ISNULL(T.[reversal_document_code],N'')<>ISNULL(S.[reversal_document_code],N'')
  OR ISNULL(T.[reversal_fiscal_year],N'')<>ISNULL(S.[reversal_fiscal_year],N'')
  OR ISNULL(T.[tax_code],N'')<>ISNULL(S.[tax_code],N'')
  OR ISNULL(T.[supplying_country],N'')<>ISNULL(S.[supplying_country],N'')
  OR ISNULL(T.[tax_exchange_rate],-1)<>ISNULL(S.[tax_exchange_rate],-1)
  OR ISNULL(T.[baseline_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[baseline_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[exchange_rate_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[exchange_rate_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[transaction_code],N'')<>ISNULL(S.[transaction_code],N'')
  OR ISNULL(T.[plant_code],N'')<>ISNULL(S.[plant_code],N'')
  OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'')
  OR T.[is_deleted]<>S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[document_type]=S.[document_type],
  T.[document_date]=S.[document_date],
  T.[posting_date]=S.[posting_date],
  T.[transaction_event_type]=S.[transaction_event_type],
  T.[reference_code]=S.[reference_code],
  T.[supplier_code]=S.[supplier_code],
  T.[currency_code]=S.[currency_code],
  T.[exchange_rate]=S.[exchange_rate],
  T.[gross_amount]=S.[gross_amount],
  T.[vat_amount]=S.[vat_amount],
  T.[tax_jurisdiction_code]=S.[tax_jurisdiction_code],
  T.[cash_discount_days1]=S.[cash_discount_days1],
  T.[invoice_flag]=S.[invoice_flag],
  T.[header_text]=S.[header_text],
  T.[reversal_document_code]=S.[reversal_document_code],
  T.[reversal_fiscal_year]=S.[reversal_fiscal_year],
  T.[tax_code]=S.[tax_code],
  T.[supplying_country]=S.[supplying_country],
  T.[tax_exchange_rate]=S.[tax_exchange_rate],
  T.[baseline_date]=S.[baseline_date],
  T.[exchange_rate_date]=S.[exchange_rate_date],
  T.[transaction_code]=S.[transaction_code],
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
  INSERT ([id],[plant_code],[purchase_invoice_code],[fiscal_year],[document_type],[document_date],[posting_date],[transaction_event_type],[reference_code],[supplier_code],[currency_code],[exchange_rate],[gross_amount],[vat_amount],[tax_jurisdiction_code],[cash_discount_days1],[invoice_flag],[header_text],[reversal_document_code],[reversal_fiscal_year],[tax_code],[supplying_country],[tax_exchange_rate],[baseline_date],[exchange_rate_date],[transaction_code],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[plant_code],S.[purchase_invoice_code],S.[fiscal_year],S.[document_type],S.[document_date],S.[posting_date],S.[transaction_event_type],S.[reference_code],S.[supplier_code],S.[currency_code],S.[exchange_rate],S.[gross_amount],S.[vat_amount],S.[tax_jurisdiction_code],S.[cash_discount_days1],S.[invoice_flag],S.[header_text],S.[reversal_document_code],S.[reversal_fiscal_year],S.[tax_code],S.[supplying_country],S.[tax_exchange_rate],S.[baseline_date],S.[exchange_rate_date],S.[transaction_code],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[fiscal_year], INSERTED.[purchase_invoice_code]
INTO #hdr_delta (rn, oper_type, id, [fiscal_year], [purchase_invoice_code]);
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

-- 先回填真实 id，再按 id 软删孤儿（禁止仅靠业务键 EXISTS，避免 collation/不可见字符导致刚装入行被误删）
UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_procurement_purchase_invoice] T
  ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[fiscal_year]))=S.[fiscal_year]
 AND LTRIM(RTRIM(T.[purchase_invoice_code]))=S.[purchase_invoice_code];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[fiscal_year], INSERTED.[purchase_invoice_code]
INTO #hdr_soft ([id], [fiscal_year], [purchase_invoice_code])
FROM [takt_logistics_procurement_purchase_invoice] T
WHERE T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #hdr S WHERE S.[id]=T.[id]);
DECLARE @hdr_delete INT = @@ROWCOUNT;

-- 源库回填：明细 purchase_invoice_id → 主表雪花 id（业务键 purchase_invoice_code；源同码无多年）
UPDATE I
SET I.[purchase_invoice_id] = H.[id]
FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice_item] AS I
INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] AS H
  ON LTRIM(RTRIM(H.[purchase_invoice_code])) = LTRIM(RTRIM(I.[purchase_invoice_code]))
WHERE I.[purchase_invoice_id] IS NULL OR I.[purchase_invoice_id] <> H.[id];

INSERT INTO #item
SELECT S.rn, @base_id+1000000000+S.rn, 0,
  S.[fiscal_year], S.[purchase_invoice_code], S.[line_number], S.[plant_code],
  S.[purchase_order_code], S.[purchase_order_item], S.[account_assignment_seq],
  S.[material_code], S.[valuation_area], S.[amount], S.[debit_credit_indicator], S.[tax_code],
  S.[quantity], S.[order_unit], S.[po_price_quantity], S.[po_price_unit],
  S.[valuated_stock_quantity], S.[previous_period_stock], S.[base_unit],
  S.[valuation_class], S.[update_po_history_flag], S.[subsequent_debit_credit],
  S.[block_reason_price], S.[block_reason_quantity], S.[block_reason_quality], S.[block_reason_enhanced],
  S.[value_string], S.[reference_code], S.[condition_type],
  S.[total_valuated_stock_value], S.[previous_period_value],
  S.[reference_document_code], S.[reference_document_year], S.[reference_document_item],
  S.[stock_managed_material_code], S.[item_text], S.[material_document_item],
  S.[is_obsolete], S.[tenant_code], S.[company_code], S.[culture_code], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT N.*, ROW_NUMBER() OVER (ORDER BY N.[fiscal_year], N.[purchase_invoice_code], N.[line_number]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[fiscal_year])), 4), N''), N'') AS [fiscal_year],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_invoice_code])), 10), N''), N'') AS [purchase_invoice_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_order_code])), 20), N'') AS [purchase_order_code],
      TRY_CAST(R.[purchase_order_item] AS INT) AS [purchase_order_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[account_assignment_seq])), 2), N'') AS [account_assignment_seq],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N'')
      END AS [material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[valuation_area])), 4), N'') AS [valuation_area],
      ROUND(TRY_CAST(R.[amount] AS DECIMAL(18,2)), 2) AS [amount],
      NULLIF(LEFT(LTRIM(RTRIM(R.[debit_credit_indicator])), 1), N'') AS [debit_credit_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_code])), 2), N'') AS [tax_code],
      ROUND(TRY_CAST(R.[quantity] AS DECIMAL(18,3)), 3) AS [quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[order_unit])), 3), N'') AS [order_unit],
      ROUND(TRY_CAST(R.[po_price_quantity] AS DECIMAL(18,3)), 3) AS [po_price_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[po_price_unit])), 3), N'') AS [po_price_unit],
      ROUND(TRY_CAST(R.[valuated_stock_quantity] AS DECIMAL(18,3)), 3) AS [valuated_stock_quantity],
      ROUND(TRY_CAST(R.[previous_period_stock] AS DECIMAL(18,3)), 3) AS [previous_period_stock],
      NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N'') AS [base_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[valuation_class])), 4), N'') AS [valuation_class],
      NULLIF(LEFT(LTRIM(RTRIM(R.[update_po_history_flag])), 1), N'') AS [update_po_history_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[subsequent_debit_credit])), 1), N'') AS [subsequent_debit_credit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[block_reason_price])), 1), N'') AS [block_reason_price],
      NULLIF(LEFT(LTRIM(RTRIM(R.[block_reason_quantity])), 1), N'') AS [block_reason_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[block_reason_quality])), 1), N'') AS [block_reason_quality],
      NULLIF(LEFT(LTRIM(RTRIM(R.[block_reason_enhanced])), 1), N'') AS [block_reason_enhanced],
      NULLIF(LEFT(LTRIM(RTRIM(R.[value_string])), 4), N'') AS [value_string],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[condition_type])), 4), N'') AS [condition_type],
      ROUND(TRY_CAST(R.[total_valuated_stock_value] AS DECIMAL(18,2)), 2) AS [total_valuated_stock_value],
      ROUND(TRY_CAST(R.[previous_period_value] AS DECIMAL(18,2)), 2) AS [previous_period_value],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_code])), 10), N'') AS [reference_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_year])), 4), N'') AS [reference_document_year],
      TRY_CAST(R.[reference_document_item] AS INT) AS [reference_document_item],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[stock_managed_material_code], N''))))=18 AND LTRIM(RTRIM(R.[stock_managed_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[stock_managed_material_code])), 10)
        ELSE NULLIF(LEFT(LTRIM(RTRIM(R.[stock_managed_material_code])), 20), N'')
      END AS [stock_managed_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[item_text])), 40), N'') AS [item_text],
      TRY_CAST(R.[material_document_item] AS INT) AS [material_document_item],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0)=0 THEN 0 ELSE 1 END AS [is_deleted],
            ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(SH.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code])), COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] SH ON SH.[id]=R.[purchase_invoice_id]
    WHERE LTRIM(RTRIM(ISNULL(SH.[fiscal_year], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0)>0
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  ) N
  WHERE N.dup_rn=1
) S
WHERE @batch_size=0 OR S.rn<=@batch_size;

UPDATE I SET I.[purchase_invoice_id]=H.[id], I.[id]=COALESCE(T.[id], I.[id])
FROM #item I
INNER JOIN #hdr H ON H.[fiscal_year]=I.[fiscal_year] AND H.[purchase_invoice_code]=I.[purchase_invoice_code]
LEFT JOIN [takt_logistics_procurement_purchase_invoice_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[purchase_invoice_id]=H.[id] AND T.[line_number]=I.[line_number];

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [purchase_invoice_id]<>0);
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
  SELECT COUNT(*) FROM (
    SELECT LTRIM(RTRIM(SH.[fiscal_year])) AS [fiscal_year], LTRIM(RTRIM(R.[purchase_invoice_code])) AS [purchase_invoice_code], COALESCE(TRY_CAST(R.[line_number] AS INT),0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice] SH ON SH.[id]=R.[purchase_invoice_id]
    WHERE LTRIM(RTRIM(ISNULL(SH.[fiscal_year], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[purchase_invoice_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT),0)>0
    GROUP BY LTRIM(RTRIM(SH.[fiscal_year])), LTRIM(RTRIM(R.[purchase_invoice_code])), COALESCE(TRY_CAST(R.[line_number] AS INT),0)
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;
DELETE FROM #item WHERE [purchase_invoice_id]=0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

-- 先回填目标明细外键（凭证号在主表唯一），再同步
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
  UPDATE TOP (@apply_chunk) I
  SET I.[purchase_invoice_id] = U.[id], I.[updated_by] = @sync_user_id, I.[updated_at] = @now
  FROM [takt_logistics_procurement_purchase_invoice_item] AS I
  INNER JOIN (
    SELECT
      H.[tenant_code], H.[company_code],
      LTRIM(RTRIM(H.[purchase_invoice_code])) AS [purchase_invoice_code],
      MIN(H.[id]) AS [id]
    FROM [takt_logistics_procurement_purchase_invoice] AS H
    WHERE H.[is_deleted] = 0
    GROUP BY H.[tenant_code], H.[company_code], LTRIM(RTRIM(H.[purchase_invoice_code]))
    HAVING COUNT(*) = 1
  ) AS U
    ON U.[tenant_code] = I.[tenant_code]
   AND U.[company_code] = I.[company_code]
   AND U.[purchase_invoice_code] = LTRIM(RTRIM(I.[purchase_invoice_code]))
  WHERE I.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code] = I.[tenant_code] AND S.[company_code] = I.[company_code])
    AND (I.[purchase_invoice_id] IS NULL OR I.[purchase_invoice_id] <> U.[id]);
  SET @dml_n = @@ROWCOUNT;
END

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #item), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_procurement_purchase_invoice_item] AS T
USING (SELECT * FROM #item WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[purchase_invoice_id]=S.[purchase_invoice_id] AND T.[line_number]=S.[line_number]
WHEN MATCHED AND (
  ISNULL(T.[plant_code],N'')<>ISNULL(S.[plant_code],N'')
  OR ISNULL(T.[purchase_invoice_code],N'')<>ISNULL(S.[purchase_invoice_code],N'')
  OR ISNULL(T.[purchase_order_code],N'')<>ISNULL(S.[purchase_order_code],N'')
  OR ISNULL(T.[purchase_order_item],-1)<>ISNULL(S.[purchase_order_item],-1)
  OR ISNULL(T.[account_assignment_seq],N'')<>ISNULL(S.[account_assignment_seq],N'')
  OR ISNULL(T.[material_code],N'')<>ISNULL(S.[material_code],N'')
  OR ISNULL(T.[valuation_area],N'')<>ISNULL(S.[valuation_area],N'')
  OR ISNULL(T.[amount],-1)<>ISNULL(S.[amount],-1)
  OR ISNULL(T.[debit_credit_indicator],N'')<>ISNULL(S.[debit_credit_indicator],N'')
  OR ISNULL(T.[tax_code],N'')<>ISNULL(S.[tax_code],N'')
  OR ISNULL(T.[quantity],-1)<>ISNULL(S.[quantity],-1)
  OR ISNULL(T.[order_unit],N'')<>ISNULL(S.[order_unit],N'')
  OR ISNULL(T.[po_price_quantity],-1)<>ISNULL(S.[po_price_quantity],-1)
  OR ISNULL(T.[po_price_unit],N'')<>ISNULL(S.[po_price_unit],N'')
  OR ISNULL(T.[valuated_stock_quantity],-1)<>ISNULL(S.[valuated_stock_quantity],-1)
  OR ISNULL(T.[previous_period_stock],-1)<>ISNULL(S.[previous_period_stock],-1)
  OR ISNULL(T.[base_unit],N'')<>ISNULL(S.[base_unit],N'')
  OR ISNULL(T.[valuation_class],N'')<>ISNULL(S.[valuation_class],N'')
  OR ISNULL(T.[update_po_history_flag],N'')<>ISNULL(S.[update_po_history_flag],N'')
  OR ISNULL(T.[subsequent_debit_credit],N'')<>ISNULL(S.[subsequent_debit_credit],N'')
  OR ISNULL(T.[block_reason_price],N'')<>ISNULL(S.[block_reason_price],N'')
  OR ISNULL(T.[block_reason_quantity],N'')<>ISNULL(S.[block_reason_quantity],N'')
  OR ISNULL(T.[block_reason_quality],N'')<>ISNULL(S.[block_reason_quality],N'')
  OR ISNULL(T.[block_reason_enhanced],N'')<>ISNULL(S.[block_reason_enhanced],N'')
  OR ISNULL(T.[value_string],N'')<>ISNULL(S.[value_string],N'')
  OR ISNULL(T.[reference_code],N'')<>ISNULL(S.[reference_code],N'')
  OR ISNULL(T.[condition_type],N'')<>ISNULL(S.[condition_type],N'')
  OR ISNULL(T.[total_valuated_stock_value],-1)<>ISNULL(S.[total_valuated_stock_value],-1)
  OR ISNULL(T.[previous_period_value],-1)<>ISNULL(S.[previous_period_value],-1)
  OR ISNULL(T.[reference_document_code],N'')<>ISNULL(S.[reference_document_code],N'')
  OR ISNULL(T.[reference_document_year],N'')<>ISNULL(S.[reference_document_year],N'')
  OR ISNULL(T.[reference_document_item],-1)<>ISNULL(S.[reference_document_item],-1)
  OR ISNULL(T.[stock_managed_material_code],N'')<>ISNULL(S.[stock_managed_material_code],N'')
  OR ISNULL(T.[item_text],N'')<>ISNULL(S.[item_text],N'')
  OR ISNULL(T.[material_document_item],-1)<>ISNULL(S.[material_document_item],-1)
  OR ISNULL(T.[is_obsolete],-1)<>ISNULL(S.[is_obsolete],-1)
  OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'')
  OR T.[is_deleted]<>S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[purchase_invoice_code]=S.[purchase_invoice_code],
  T.[purchase_order_code]=S.[purchase_order_code],
  T.[purchase_order_item]=S.[purchase_order_item],
  T.[account_assignment_seq]=S.[account_assignment_seq],
  T.[material_code]=S.[material_code],
  T.[valuation_area]=S.[valuation_area],
  T.[amount]=S.[amount],
  T.[debit_credit_indicator]=S.[debit_credit_indicator],
  T.[tax_code]=S.[tax_code],
  T.[quantity]=S.[quantity],
  T.[order_unit]=S.[order_unit],
  T.[po_price_quantity]=S.[po_price_quantity],
  T.[po_price_unit]=S.[po_price_unit],
  T.[valuated_stock_quantity]=S.[valuated_stock_quantity],
  T.[previous_period_stock]=S.[previous_period_stock],
  T.[base_unit]=S.[base_unit],
  T.[valuation_class]=S.[valuation_class],
  T.[update_po_history_flag]=S.[update_po_history_flag],
  T.[subsequent_debit_credit]=S.[subsequent_debit_credit],
  T.[block_reason_price]=S.[block_reason_price],
  T.[block_reason_quantity]=S.[block_reason_quantity],
  T.[block_reason_quality]=S.[block_reason_quality],
  T.[block_reason_enhanced]=S.[block_reason_enhanced],
  T.[value_string]=S.[value_string],
  T.[reference_code]=S.[reference_code],
  T.[condition_type]=S.[condition_type],
  T.[total_valuated_stock_value]=S.[total_valuated_stock_value],
  T.[previous_period_value]=S.[previous_period_value],
  T.[reference_document_code]=S.[reference_document_code],
  T.[reference_document_year]=S.[reference_document_year],
  T.[reference_document_item]=S.[reference_document_item],
  T.[stock_managed_material_code]=S.[stock_managed_material_code],
  T.[item_text]=S.[item_text],
  T.[material_document_item]=S.[material_document_item],
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
  INSERT ([id],[purchase_invoice_id],[plant_code],[purchase_invoice_code],[line_number],[purchase_order_code],[purchase_order_item],[account_assignment_seq],[material_code],[valuation_area],[amount],[debit_credit_indicator],[tax_code],[quantity],[order_unit],[po_price_quantity],[po_price_unit],[valuated_stock_quantity],[previous_period_stock],[base_unit],[valuation_class],[update_po_history_flag],[subsequent_debit_credit],[block_reason_price],[block_reason_quantity],[block_reason_quality],[block_reason_enhanced],[value_string],[reference_code],[condition_type],[total_valuated_stock_value],[previous_period_value],[reference_document_code],[reference_document_year],[reference_document_item],[stock_managed_material_code],[item_text],[material_document_item],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[purchase_invoice_id],S.[plant_code],S.[purchase_invoice_code],S.[line_number],S.[purchase_order_code],S.[purchase_order_item],S.[account_assignment_seq],S.[material_code],S.[valuation_area],S.[amount],S.[debit_credit_indicator],S.[tax_code],S.[quantity],S.[order_unit],S.[po_price_quantity],S.[po_price_unit],S.[valuated_stock_quantity],S.[previous_period_stock],S.[base_unit],S.[valuation_class],S.[update_po_history_flag],S.[subsequent_debit_credit],S.[block_reason_price],S.[block_reason_quantity],S.[block_reason_quality],S.[block_reason_enhanced],S.[value_string],S.[reference_code],S.[condition_type],S.[total_valuated_stock_value],S.[previous_period_value],S.[reference_document_code],S.[reference_document_year],S.[reference_document_item],S.[stock_managed_material_code],S.[item_text],S.[material_document_item],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[purchase_invoice_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [purchase_invoice_code], [line_number]);
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

UPDATE I SET I.[id]=T.[id]
FROM #item I
INNER JOIN [takt_logistics_procurement_purchase_invoice_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[purchase_invoice_id]=I.[purchase_invoice_id] AND T.[line_number]=I.[line_number];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[purchase_invoice_code], INSERTED.[line_number]
INTO #item_soft ([id], [purchase_invoice_code], [line_number])
FROM [takt_logistics_procurement_purchase_invoice_item] T
WHERE T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #item S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[id]=T.[id]);
DECLARE @item_delete INT = @@ROWCOUNT;

-- 有效行校验：目标 is_deleted=0 必须等于装入中 is_deleted=0（源软删行会 MERGE 为删除态，不能与全量装入比）
DECLARE @hdr_source_active INT = (SELECT COUNT(*) FROM #hdr WHERE [is_deleted]=0);
DECLARE @item_source_active INT = (SELECT COUNT(*) FROM #item WHERE [is_deleted]=0);
DECLARE @hdr_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_purchase_invoice] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_purchase_invoice_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
IF @hdr_after <> @hdr_source_active BEGIN DECLARE @hdr_cnt NVARCHAR(200)=CONCAT(N'主表有效行数不一致: source=',@hdr_source_active,N', active=',@hdr_after); THROW 50002,@hdr_cnt,1; END;
IF @item_after <> @item_source_active BEGIN DECLARE @item_cnt NVARCHAR(200)=CONCAT(N'明细有效行数不一致: source=',@item_source_active,N', active=',@item_after); THROW 50002,@item_cnt,1; END;

DECLARE @hdr_ins INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'INSERT');
DECLARE @hdr_upd INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'UPDATE');
DECLARE @hdr_unchanged INT=@hdr_source-@hdr_ins-@hdr_upd;
DECLARE @item_ins INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'INSERT');
DECLARE @item_upd INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'UPDATE');
DECLARE @item_unchanged INT=@item_source-@item_ins-@item_upd;

DECLARE @hdr_soft_keys NVARCHAR(MAX)=N'';
SELECT @hdr_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([fiscal_year],N''),N'/',ISNULL([purchase_invoice_code],N'')) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #hdr_soft ORDER BY [id]) SoftSample;
SET @hdr_soft_keys=ISNULL(@hdr_soft_keys,N'');
IF @hdr_delete>100 SET @hdr_soft_keys=CONCAT(@hdr_soft_keys,N'; ...(+',CAST(@hdr_delete-100 AS NVARCHAR(20)),N')');

DECLARE @item_soft_keys NVARCHAR(MAX)=N'';
SELECT @item_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([purchase_invoice_code],N''),N'/',CAST([line_number] AS NVARCHAR(20))) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys=ISNULL(@item_soft_keys,N'');
IF @item_delete>100 SET @item_soft_keys=CONCAT(@item_soft_keys,N'; ...(+',CAST(@item_delete-100 AS NVARCHAR(20)),N')');

DECLARE @hdr_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice]
);
DECLARE @item_sap_raw INT = (
  SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_invoice_item]
);
DECLARE @hdr_physical INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice] T
  WHERE EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_physical INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice_item] T
  WHERE EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @hdr_soft_total INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice] T
  WHERE T.[is_deleted]=1
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_soft_total INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_invoice_item] T
  WHERE T.[is_deleted]=1
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
