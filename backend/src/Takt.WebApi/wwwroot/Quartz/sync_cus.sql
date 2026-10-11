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

-- 源表 / 目标表：同名 + 列与实体 TaktCustomer 一致（源/目标结构相同，以目标实体为准）
-- {{SourceDatabase}}.dbo.takt_logistics_sales_customer → 当前租户库同名表
-- 业务唯一键：CompanyCode + CustomerCode（与租户库唯一索引对齐；PlantCode 为业务字段，不参与匹配）
-- company_code / plant_code 取自源表；tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数；全量同步（无公司白名单）

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [customer_code] NVARCHAR(10),
  [customer_name1] NVARCHAR(140),
  [customer_name2] NVARCHAR(140),
  [customer_short_name] NVARCHAR(40),
  [customer_type] INT,
  [enterprise_nature] VARCHAR(4),
  [industry_attribute] VARCHAR(4),
  [customer_tax_number] NVARCHAR(50),
  [tax_code] NVARCHAR(4),
  [tax_rate] INT,
  [registration_country] NVARCHAR(2),
  [registration_province] NVARCHAR(70),
  [registration_city] NVARCHAR(70),
  [registration_address1] NVARCHAR(140),
  [registration_address2] NVARCHAR(140),
  [customer_phone] NVARCHAR(50),
  [customer_fax] NVARCHAR(50),
  [customer_email] NVARCHAR(100),
  [customer_website] NVARCHAR(200),
  [contact_person] NVARCHAR(50),
  [contact_phone] NVARCHAR(50),
  [contact_email] NVARCHAR(100),
  [currency_code] NVARCHAR(3),
  [sales_organization] VARCHAR(4),
  [distribution_channel] VARCHAR(2),
  [product_group] VARCHAR(2),
  [customer_group] VARCHAR(2),
  [trading_partner] VARCHAR(4),
  [account_assignment_group] VARCHAR(2),
  [supplier_code] NVARCHAR(10),
  [nielsen_indicator] VARCHAR(2),
  [central_posting_block] INT,
  [reconciliation_account] VARCHAR(40),
  [headquarters] NVARCHAR(20),
  [clearing_with_vendor] INT,
  [payment_terms] NVARCHAR(40),
  [payment_method] INT,
  [delivering_plant] VARCHAR(4),
  [incoterms1] VARCHAR(3),
  [incoterms2] NVARCHAR(40),
  [shipping_conditions] VARCHAR(2),
  [customer_pricing_procedure] VARCHAR(2),
  [credit_level] INT,
  [credit_amount] DECIMAL(18,2),
  [discount_rate] DECIMAL(5,2),
  [customer_level] INT,
  [evaluation_score] DECIMAL(5,2),
  [sort_order] INT,
  [customer_status] INT,
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
  S.[customer_code],
  S.[customer_name1],
  S.[customer_name2],
  S.[customer_short_name],
  S.[customer_type],
  S.[enterprise_nature],
  S.[industry_attribute],
  S.[customer_tax_number],
  S.[tax_code],
  S.[tax_rate],
  S.[registration_country],
  S.[registration_province],
  S.[registration_city],
  S.[registration_address1],
  S.[registration_address2],
  S.[customer_phone],
  S.[customer_fax],
  S.[customer_email],
  S.[customer_website],
  S.[contact_person],
  S.[contact_phone],
  S.[contact_email],
  S.[currency_code],
  S.[sales_organization],
  S.[distribution_channel],
  S.[product_group],
  S.[customer_group],
  S.[trading_partner],
  S.[account_assignment_group],
  S.[supplier_code],
  S.[nielsen_indicator],
  S.[central_posting_block],
  S.[reconciliation_account],
  S.[headquarters],
  S.[clearing_with_vendor],
  S.[payment_terms],
  S.[payment_method],
  S.[delivering_plant],
  S.[incoterms1],
  S.[incoterms2],
  S.[shipping_conditions],
  S.[customer_pricing_procedure],
  S.[credit_level],
  S.[credit_amount],
  S.[discount_rate],
  S.[customer_level],
  S.[evaluation_score],
  S.[sort_order],
  S.[customer_status],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  S.[ext_field],
  S.[remark],
    S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[company_code], N.[customer_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(R.[customer_code])) AS [customer_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[customer_name1])), N''), N'') AS [customer_name1],
      NULLIF(LTRIM(RTRIM(R.[customer_name2])), N'') AS [customer_name2],
      NULLIF(LTRIM(RTRIM(R.[customer_short_name])), N'') AS [customer_short_name],
      COALESCE(TRY_CAST(R.[customer_type] AS INT), 0) AS [customer_type],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[enterprise_nature])), N''), N'') AS [enterprise_nature],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[industry_attribute])), N''), N'') AS [industry_attribute],
      NULLIF(LTRIM(RTRIM(R.[customer_tax_number])), N'') AS [customer_tax_number],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_code], N''))), 4), N'') AS [tax_code],
      COALESCE(TRY_CAST(R.[tax_rate] AS INT), 0) AS [tax_rate],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      NULLIF(LTRIM(RTRIM(R.[registration_country])), N'') AS [registration_country],
      NULLIF(LTRIM(RTRIM(R.[registration_province])), N'') AS [registration_province],
      NULLIF(LTRIM(RTRIM(R.[registration_city])), N'') AS [registration_city],
      NULLIF(LTRIM(RTRIM(R.[registration_address1])), N'') AS [registration_address1],
      NULLIF(LTRIM(RTRIM(R.[registration_address2])), N'') AS [registration_address2],
      NULLIF(LTRIM(RTRIM(R.[customer_phone])), N'') AS [customer_phone],
      NULLIF(LTRIM(RTRIM(R.[customer_fax])), N'') AS [customer_fax],
      NULLIF(LTRIM(RTRIM(R.[customer_email])), N'') AS [customer_email],
      NULLIF(LTRIM(RTRIM(R.[customer_website])), N'') AS [customer_website],
      NULLIF(LTRIM(RTRIM(R.[contact_person])), N'') AS [contact_person],
      NULLIF(LTRIM(RTRIM(R.[contact_phone])), N'') AS [contact_phone],
      NULLIF(LTRIM(RTRIM(R.[contact_email])), N'') AS [contact_email],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[currency_code])), N''), N'') AS [currency_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[sales_organization])), N''), N'') AS [sales_organization],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[distribution_channel])), N''), N'') AS [distribution_channel],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[product_group])), N''), N'') AS [product_group],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[customer_group])), N''), N'') AS [customer_group],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[trading_partner])), N''), N'') AS [trading_partner],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[account_assignment_group])), N''), N'') AS [account_assignment_group],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[supplier_code])), N''), N'') AS [supplier_code],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[nielsen_indicator])), N''), N'') AS [nielsen_indicator],
      COALESCE(TRY_CAST(R.[central_posting_block] AS INT), 0) AS [central_posting_block],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[reconciliation_account])), N''), N'') AS [reconciliation_account],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[headquarters])), N''), N'') AS [headquarters],
      COALESCE(TRY_CAST(R.[clearing_with_vendor] AS INT), 0) AS [clearing_with_vendor],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[payment_terms])), N''), N'') AS [payment_terms],
      COALESCE(TRY_CAST(R.[payment_method] AS INT), 1) AS [payment_method],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[delivering_plant])), N''), N'') AS [delivering_plant],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[incoterms1])), N''), N'') AS [incoterms1],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[incoterms2])), N''), N'') AS [incoterms2],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[shipping_conditions])), N''), N'') AS [shipping_conditions],
      ISNULL(NULLIF(LTRIM(RTRIM(R.[customer_pricing_procedure])), N''), N'') AS [customer_pricing_procedure],
      COALESCE(TRY_CAST(R.[credit_level] AS INT), 0) AS [credit_level],
      ROUND(COALESCE(TRY_CAST(R.[credit_amount] AS DECIMAL(18,8)), 0), 2) AS [credit_amount],
      ROUND(COALESCE(TRY_CAST(R.[discount_rate] AS DECIMAL(18,8)), 0), 2) AS [discount_rate],
      COALESCE(TRY_CAST(R.[customer_level] AS INT), 0) AS [customer_level],
      ROUND(COALESCE(TRY_CAST(R.[evaluation_score] AS DECIMAL(18,8)), 0), 2) AS [evaluation_score],
      COALESCE(TRY_CAST(R.[sort_order] AS INT), 0) AS [sort_order],
      COALESCE(TRY_CAST(R.[customer_status] AS INT), 1) AS [customer_status],
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
          LTRIM(RTRIM(R.[customer_code]))
        ORDER BY
          LEN(ISNULL(LTRIM(RTRIM(R.[customer_name1])), N'')) DESC,
          LEN(ISNULL(LTRIM(RTRIM(R.[plant_code])), N'')) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] R
    WHERE LTRIM(RTRIM(ISNULL(R.[customer_code], N''))) <> N''
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
DECLARE @sap_raw_count INT = (
  SELECT COUNT(*)
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] R
  WHERE LTRIM(RTRIM(ISNULL(R.[customer_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
);
DECLARE @sap_key_count INT = (
  SELECT COUNT(*)
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(R.[customer_code])) AS [customer_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] R
    WHERE LTRIM(RTRIM(ISNULL(R.[customer_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
    GROUP BY
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
      LTRIM(RTRIM(R.[customer_code]))
  ) K
);
DECLARE @dedupe_dropped INT = @sap_raw_count - @sap_key_count;

IF @source_count <> @sap_key_count
BEGIN
  DECLARE @src_msg NVARCHAR(200) = CONCAT(
    N'业务键装入不一致: keys=', @sap_key_count, N', loaded=', @source_count,
    N', sap_raw=', @sap_raw_count, N', dedupe_dropped=', @dedupe_dropped);
  THROW 50003, @src_msg, 1;
END;

IF EXISTS (
  SELECT 1 FROM #st_source GROUP BY [company_code], [customer_code] HAVING COUNT(*) > 1
)
BEGIN
  DECLARE @dup_key NVARCHAR(400);
  SELECT TOP 1
    @dup_key = CONCAT([company_code], N' / ', [customer_code], N' x', COUNT(*))
  FROM #st_source
  GROUP BY [company_code], [customer_code]
  HAVING COUNT(*) > 1;
  THROW 50001, @dup_key, 1;
END;

-- 复用租户库已有 Id（含软删行；按公司+客户码对齐唯一索引）
UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_logistics_sales_customer] T
  ON T.[tenant_code]=S.[tenant_code]
 AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[customer_code])) = S.[customer_code];

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(4),
  customer_code NVARCHAR(10),
  tenant_code NVARCHAR(3),
  company_code NVARCHAR(4),
  change_by BIGINT,
  customer_name1_old NVARCHAR(140),
  customer_name1_new NVARCHAR(140),
  customer_status_old INT,
  customer_status_new INT
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_customer] T
  WHERE T.[tenant_code] = @tenant_code
    AND T.[is_deleted] = 0
    AND EXISTS (
      SELECT 1 FROM #st_source S WHERE S.[company_code] = T.[company_code]
    )
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_sales_customer] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[customer_code])) = S.[customer_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR LTRIM(RTRIM(ISNULL(T.[plant_code], N''))) <> S.[plant_code]
  OR LTRIM(RTRIM(ISNULL(T.[customer_name1], N''))) <> S.[customer_name1]
  OR LTRIM(RTRIM(ISNULL(T.[customer_name2], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_name2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_short_name], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_short_name], N'')))
  OR T.[customer_type] <> S.[customer_type]
  OR LTRIM(RTRIM(ISNULL(T.[enterprise_nature], N''))) <> S.[enterprise_nature]
  OR LTRIM(RTRIM(ISNULL(T.[industry_attribute], N''))) <> S.[industry_attribute]
  OR LTRIM(RTRIM(ISNULL(T.[customer_tax_number], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_tax_number], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_code], N''))) <> LTRIM(RTRIM(ISNULL(S.[tax_code], N'')))
  OR T.[tax_rate] <> S.[tax_rate]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N''))) <> S.[culture_code]
  OR LTRIM(RTRIM(ISNULL(T.[registration_country], N''))) <> LTRIM(RTRIM(ISNULL(S.[registration_country], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_province], N''))) <> LTRIM(RTRIM(ISNULL(S.[registration_province], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_city], N''))) <> LTRIM(RTRIM(ISNULL(S.[registration_city], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_address1], N''))) <> LTRIM(RTRIM(ISNULL(S.[registration_address1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_address2], N''))) <> LTRIM(RTRIM(ISNULL(S.[registration_address2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_phone], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_phone], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_fax], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_fax], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_email], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_email], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_website], N''))) <> LTRIM(RTRIM(ISNULL(S.[customer_website], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[contact_person], N''))) <> LTRIM(RTRIM(ISNULL(S.[contact_person], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[contact_phone], N''))) <> LTRIM(RTRIM(ISNULL(S.[contact_phone], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[contact_email], N''))) <> LTRIM(RTRIM(ISNULL(S.[contact_email], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N''))) <> S.[currency_code]
  OR LTRIM(RTRIM(ISNULL(T.[sales_organization], N''))) <> S.[sales_organization]
  OR LTRIM(RTRIM(ISNULL(T.[distribution_channel], N''))) <> S.[distribution_channel]
  OR LTRIM(RTRIM(ISNULL(T.[product_group], N''))) <> S.[product_group]
  OR LTRIM(RTRIM(ISNULL(T.[customer_group], N''))) <> S.[customer_group]
  OR LTRIM(RTRIM(ISNULL(T.[trading_partner], N''))) <> S.[trading_partner]
  OR LTRIM(RTRIM(ISNULL(T.[account_assignment_group], N''))) <> S.[account_assignment_group]
  OR LTRIM(RTRIM(ISNULL(T.[supplier_code], N''))) <> S.[supplier_code]
  OR LTRIM(RTRIM(ISNULL(T.[nielsen_indicator], N''))) <> S.[nielsen_indicator]
  OR T.[central_posting_block] <> S.[central_posting_block]
  OR LTRIM(RTRIM(ISNULL(T.[reconciliation_account], N''))) <> S.[reconciliation_account]
  OR LTRIM(RTRIM(ISNULL(T.[headquarters], N''))) <> S.[headquarters]
  OR T.[clearing_with_vendor] <> S.[clearing_with_vendor]
  OR LTRIM(RTRIM(ISNULL(T.[payment_terms], N''))) <> S.[payment_terms]
  OR T.[payment_method] <> S.[payment_method]
  OR LTRIM(RTRIM(ISNULL(T.[delivering_plant], N''))) <> S.[delivering_plant]
  OR LTRIM(RTRIM(ISNULL(T.[incoterms1], N''))) <> S.[incoterms1]
  OR LTRIM(RTRIM(ISNULL(T.[incoterms2], N''))) <> S.[incoterms2]
  OR LTRIM(RTRIM(ISNULL(T.[shipping_conditions], N''))) <> S.[shipping_conditions]
  OR LTRIM(RTRIM(ISNULL(T.[customer_pricing_procedure], N''))) <> S.[customer_pricing_procedure]
  OR T.[credit_level] <> S.[credit_level]
  OR ROUND(T.[credit_amount], 2) <> ROUND(S.[credit_amount], 2)
  OR ROUND(T.[discount_rate], 2) <> ROUND(S.[discount_rate], 2)
  OR T.[customer_level] <> S.[customer_level]
  OR ROUND(T.[evaluation_score], 2) <> ROUND(S.[evaluation_score], 2)
  OR T.[sort_order] <> S.[sort_order]
  OR T.[customer_status] <> S.[customer_status]

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
  T.[customer_name1]=S.[customer_name1],
  T.[customer_name2]=S.[customer_name2],
  T.[customer_short_name]=S.[customer_short_name],
  T.[customer_type]=S.[customer_type],
  T.[enterprise_nature]=S.[enterprise_nature],
  T.[industry_attribute]=S.[industry_attribute],
  T.[customer_tax_number]=S.[customer_tax_number],
  T.[tax_code]=S.[tax_code],
  T.[tax_rate]=S.[tax_rate],
  T.[culture_code]=S.[culture_code],
  T.[registration_country]=S.[registration_country],
  T.[registration_province]=S.[registration_province],
  T.[registration_city]=S.[registration_city],
  T.[registration_address1]=S.[registration_address1],
  T.[registration_address2]=S.[registration_address2],
  T.[customer_phone]=S.[customer_phone],
  T.[customer_fax]=S.[customer_fax],
  T.[customer_email]=S.[customer_email],
  T.[customer_website]=S.[customer_website],
  T.[contact_person]=S.[contact_person],
  T.[contact_phone]=S.[contact_phone],
  T.[contact_email]=S.[contact_email],
  T.[currency_code]=S.[currency_code],
  T.[sales_organization]=S.[sales_organization],
  T.[distribution_channel]=S.[distribution_channel],
  T.[product_group]=S.[product_group],
  T.[customer_group]=S.[customer_group],
  T.[trading_partner]=S.[trading_partner],
  T.[account_assignment_group]=S.[account_assignment_group],
  T.[supplier_code]=S.[supplier_code],
  T.[nielsen_indicator]=S.[nielsen_indicator],
  T.[central_posting_block]=S.[central_posting_block],
  T.[reconciliation_account]=S.[reconciliation_account],
  T.[headquarters]=S.[headquarters],
  T.[clearing_with_vendor]=S.[clearing_with_vendor],
  T.[payment_terms]=S.[payment_terms],
  T.[payment_method]=S.[payment_method],
  T.[delivering_plant]=S.[delivering_plant],
  T.[incoterms1]=S.[incoterms1],
  T.[incoterms2]=S.[incoterms2],
  T.[shipping_conditions]=S.[shipping_conditions],
  T.[customer_pricing_procedure]=S.[customer_pricing_procedure],
  T.[credit_level]=S.[credit_level],
  T.[credit_amount]=S.[credit_amount],
  T.[discount_rate]=S.[discount_rate],
  T.[customer_level]=S.[customer_level],
  T.[evaluation_score]=S.[evaluation_score],
  T.[sort_order]=S.[sort_order],
  T.[customer_status]=S.[customer_status],
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
    [id],[plant_code],[customer_code],[customer_name1],[customer_name2],[customer_short_name],
    [customer_type],[enterprise_nature],[industry_attribute],
    [customer_tax_number],[tax_code],[tax_rate],[registration_country],[registration_province],[registration_city],
    [registration_address1],[registration_address2],[customer_phone],[customer_fax],[customer_email],
    [customer_website],[contact_person],[contact_phone],[contact_email],[currency_code],
    [sales_organization],[distribution_channel],[product_group],[customer_group],[trading_partner],
    [account_assignment_group],[supplier_code],[nielsen_indicator],[central_posting_block],
    [reconciliation_account],[headquarters],[clearing_with_vendor],[payment_terms],[payment_method],
    [delivering_plant],[incoterms1],[incoterms2],[shipping_conditions],[customer_pricing_procedure],
    [credit_level],[credit_amount],[discount_rate],[customer_level],[evaluation_score],
    [sort_order],[customer_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[customer_code],S.[customer_name1],S.[customer_name2],S.[customer_short_name],
    S.[customer_type],S.[enterprise_nature],S.[industry_attribute],
    S.[customer_tax_number],S.[tax_code],S.[tax_rate],S.[registration_country],S.[registration_province],S.[registration_city],
    S.[registration_address1],S.[registration_address2],S.[customer_phone],S.[customer_fax],S.[customer_email],
    S.[customer_website],S.[contact_person],S.[contact_phone],S.[contact_email],S.[currency_code],
    S.[sales_organization],S.[distribution_channel],S.[product_group],S.[customer_group],S.[trading_partner],
    S.[account_assignment_group],S.[supplier_code],S.[nielsen_indicator],S.[central_posting_block],
    S.[reconciliation_account],S.[headquarters],S.[clearing_with_vendor],S.[payment_terms],S.[payment_method],
    S.[delivering_plant],S.[incoterms1],S.[incoterms2],S.[shipping_conditions],S.[customer_pricing_procedure],
    S.[credit_level],S.[credit_amount],S.[discount_rate],S.[customer_level],S.[evaluation_score],
    S.[sort_order],S.[customer_status],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
    S.[is_deleted],
    S.[deleted_by],S.[deleted_at]
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[customer_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[customer_name1], INSERTED.[customer_name1],
  DELETED.[customer_status], INSERTED.[customer_status]
INTO #delta(
  rn, oper_type, id, plant_code, customer_code,
  tenant_code, company_code, change_by,
  customer_name1_old, customer_name1_new,
  customer_status_old, customer_status_new
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
  [customer_code] NVARCHAR(10)
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
  INSERTED.[customer_code]
INTO #soft_deleted_rows ([id], [plant_code], [customer_code])
FROM [takt_logistics_sales_customer] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND EXISTS (
    SELECT 1 FROM #st_source S0 WHERE S0.[company_code] = T.[company_code]
  )
  AND NOT EXISTS (
    SELECT 1
    FROM #st_source S
    WHERE S.[company_code] = T.[company_code]
      AND S.[customer_code] = LTRIM(RTRIM(T.[customer_code]))
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
    ISNULL([customer_code], N'')
  )
  AS NVARCHAR(MAX)),
  N'; '
)
FROM #soft_deleted_rows;
SET @soft_deleted_keys = ISNULL(@soft_deleted_keys, N'');
DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_customer] T
  WHERE T.[tenant_code] = @tenant_code
    AND T.[is_deleted] = 0
    AND EXISTS (
      SELECT 1 FROM #st_source S WHERE S.[company_code] = T.[company_code]
    )
);
DECLARE @target_physical INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_customer] T
  WHERE T.[tenant_code] = @tenant_code
    AND EXISTS (
      SELECT 1 FROM #st_source S WHERE S.[company_code] = T.[company_code]
    )
);
DECLARE @soft_deleted INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_customer] T
  WHERE T.[tenant_code] = @tenant_code
    AND T.[is_deleted] = 1
    AND EXISTS (
      SELECT 1 FROM #st_source S WHERE S.[company_code] = T.[company_code]
    )
);
DECLARE @source_active_count INT = (
  SELECT COUNT(*)
  FROM #st_source
  WHERE [is_deleted] = 0
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
  N'takt_logistics_sales_customer',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE Customer Sync (summary)',
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
  N'客户信息',
  N'exec_sql_merge',
  'SQL',
  N'/sync/customer',
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
  @sap_key_count AS [sap_key_count],
  @dedupe_dropped AS [dedupe_dropped],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @target_physical AS [target_physical],
  @soft_deleted AS [soft_deleted],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count],
  @soft_deleted_keys AS [soft_deleted_keys];
