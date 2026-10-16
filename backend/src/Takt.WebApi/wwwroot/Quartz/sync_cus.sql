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

-- 源表 / 目标表：同名 + 列与实体 TaktCustomer 一致
-- {{SourceDatabase}}.dbo.takt_logistics_sales_customer → 当前租户库同名表
-- 业务唯一键：TenantCode + CompanyCode + SalesOrganization + CustomerCode
-- tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数

IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'account_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [account_group] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'account_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [account_group] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'sales_organization') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [sales_organization] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'sales_organization') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [sales_organization] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'distribution_channel') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [distribution_channel] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'distribution_channel') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [distribution_channel] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'product_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [product_group] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'product_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [product_group] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_title') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_title] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_title') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_title] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_name1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_name1] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_name1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_name1] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_name2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_name2] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_name2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_name2] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_name3') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_name3] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_name3') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_name3] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_name4') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_name4] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_name4') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_name4] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_short_name') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_short_name] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_short_name') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_short_name] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'registration_country') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [registration_country] NVARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'registration_country') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [registration_country] NVARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'registration_region') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [registration_region] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'registration_region') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [registration_region] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'registration_city') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [registration_city] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'registration_city') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [registration_city] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'addr1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [addr1] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'addr1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [addr1] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'addr2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [addr2] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'addr2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [addr2] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'addr3') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [addr3] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'addr3') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [addr3] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'addr4') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [addr4] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'addr4') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [addr4] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'addr5') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [addr5] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'addr5') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [addr5] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'postal_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [postal_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'postal_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [postal_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'phone1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [phone1] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'phone1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [phone1] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'phone2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [phone2] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'phone2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [phone2] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'fax_number') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [fax_number] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'fax_number') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [fax_number] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'supplier_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [supplier_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'supplier_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [supplier_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'trading_partner') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [trading_partner] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'trading_partner') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [trading_partner] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'tax_number1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [tax_number1] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'tax_number1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [tax_number1] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'tax_number2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [tax_number2] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'tax_number2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [tax_number2] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'tax_number3') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [tax_number3] NVARCHAR(60) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'tax_number3') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [tax_number3] NVARCHAR(60) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'bank_country_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [bank_country_code] NVARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'bank_country_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [bank_country_code] NVARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'bank_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [bank_code] NVARCHAR(15) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'bank_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [bank_code] NVARCHAR(15) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'bank_account') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [bank_account] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'bank_account') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [bank_account] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'bank_account_holder') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [bank_account_holder] NVARCHAR(80) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'bank_account_holder') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [bank_account_holder] NVARCHAR(80) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_category') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_category] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_category') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_category] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'enterprise_nature') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [enterprise_nature] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'enterprise_nature') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [enterprise_nature] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'industry_attribute') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [industry_attribute] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'industry_attribute') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [industry_attribute] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'reconciliation_account') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [reconciliation_account] VARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'reconciliation_account') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [reconciliation_account] VARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'payment_terms') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [payment_terms] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'payment_terms') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [payment_terms] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'payment_method') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [payment_method] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'payment_method') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [payment_method] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'sales_district') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [sales_district] VARCHAR(6) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'sales_district') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [sales_district] VARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'sales_department') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [sales_department] VARCHAR(6) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'sales_department') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [sales_department] VARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'sales_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [sales_group] VARCHAR(6) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'sales_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [sales_group] VARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'currency_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [currency_code] NVARCHAR(3) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'currency_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [currency_code] NVARCHAR(3) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_pricing_procedure') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_pricing_procedure] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_pricing_procedure') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_pricing_procedure] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'shipping_conditions') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [shipping_conditions] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'shipping_conditions') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [shipping_conditions] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'delivering_plant') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [delivering_plant] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'delivering_plant') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [delivering_plant] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'incoterms1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [incoterms1] VARCHAR(3) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'incoterms1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [incoterms1] VARCHAR(3) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'incoterms2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [incoterms2] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'incoterms2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [incoterms2] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'account_assignment_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [account_assignment_group] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'account_assignment_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [account_assignment_group] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'tax_classification') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [tax_classification] NVARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'tax_classification') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [tax_classification] NVARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_contact') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_contact] NVARCHAR(80) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_contact') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_contact] NVARCHAR(80) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_phone') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_phone] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_phone') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_phone] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_email') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_email] NVARCHAR(100) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_email') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_email] NVARCHAR(100) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'credit_level') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [credit_level] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'credit_level') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [credit_level] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_level') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_level] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_level') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_level] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'sort_order') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [sort_order] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'sort_order') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [sort_order] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'customer_status') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] ADD [customer_status] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'customer_status') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [customer_status] INT NULL;

IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'sales_organization') IS NOT NULL
  EXEC(N'UPDATE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer]
    SET [sales_organization] = LEFT(LTRIM(RTRIM([company_code])), 4)
    WHERE NULLIF(LTRIM(RTRIM([sales_organization])), N'''') IS NULL
      AND NULLIF(LTRIM(RTRIM([company_code])), N'''') IS NOT NULL');
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'tax_classification') IS NOT NULL
  AND COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_sales_customer', N'tax_code') IS NOT NULL
  EXEC(N'UPDATE [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer]
    SET [tax_classification] = LEFT(LTRIM(RTRIM([tax_code])), 4)
    WHERE NULLIF(LTRIM(RTRIM([tax_classification])), N'''') IS NULL
      AND NULLIF(LTRIM(RTRIM([tax_code])), N'''') IS NOT NULL');
IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [customer_code] NVARCHAR(10),
  [account_group] VARCHAR(4),
  [sales_organization] VARCHAR(4),
  [distribution_channel] VARCHAR(2),
  [product_group] VARCHAR(2),
  [customer_title] INT,
  [customer_name1] NVARCHAR(140),
  [customer_name2] NVARCHAR(140),
  [customer_name3] NVARCHAR(140),
  [customer_name4] NVARCHAR(140),
  [customer_short_name] NVARCHAR(40),
  [registration_country] NVARCHAR(2),
  [registration_region] NVARCHAR(40),
  [registration_city] NVARCHAR(40),
  [addr1] NVARCHAR(140),
  [addr2] NVARCHAR(140),
  [addr3] NVARCHAR(140),
  [addr4] NVARCHAR(140),
  [addr5] NVARCHAR(140),
  [postal_code] NVARCHAR(10),
  [phone1] NVARCHAR(40),
  [phone2] NVARCHAR(40),
  [fax_number] NVARCHAR(40),
  [supplier_code] NVARCHAR(10),
  [trading_partner] VARCHAR(4),
  [tax_number1] NVARCHAR(40),
  [tax_number2] NVARCHAR(40),
  [tax_number3] NVARCHAR(60),
  [bank_country_code] NVARCHAR(2),
  [bank_code] NVARCHAR(15),
  [bank_account] NVARCHAR(40),
  [bank_account_holder] NVARCHAR(80),
  [customer_category] INT,
  [enterprise_nature] VARCHAR(4),
  [industry_attribute] VARCHAR(4),
  [reconciliation_account] VARCHAR(40),
  [payment_terms] NVARCHAR(40),
  [payment_method] INT,
  [sales_district] VARCHAR(6),
  [sales_department] VARCHAR(6),
  [sales_group] VARCHAR(6),
  [currency_code] NVARCHAR(3),
  [customer_pricing_procedure] VARCHAR(2),
  [shipping_conditions] VARCHAR(2),
  [delivering_plant] VARCHAR(4),
  [incoterms1] VARCHAR(3),
  [incoterms2] NVARCHAR(40),
  [account_assignment_group] VARCHAR(2),
  [tax_classification] NVARCHAR(4),
  [customer_contact] NVARCHAR(80),
  [customer_phone] NVARCHAR(40),
  [customer_email] NVARCHAR(100),
  [credit_level] INT,
  [customer_level] INT,
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
  S.[account_group],
  S.[sales_organization],
  S.[distribution_channel],
  S.[product_group],
  S.[customer_title],
  S.[customer_name1],
  S.[customer_name2],
  S.[customer_name3],
  S.[customer_name4],
  S.[customer_short_name],
  S.[registration_country],
  S.[registration_region],
  S.[registration_city],
  S.[addr1],
  S.[addr2],
  S.[addr3],
  S.[addr4],
  S.[addr5],
  S.[postal_code],
  S.[phone1],
  S.[phone2],
  S.[fax_number],
  S.[supplier_code],
  S.[trading_partner],
  S.[tax_number1],
  S.[tax_number2],
  S.[tax_number3],
  S.[bank_country_code],
  S.[bank_code],
  S.[bank_account],
  S.[bank_account_holder],
  S.[customer_category],
  S.[enterprise_nature],
  S.[industry_attribute],
  S.[reconciliation_account],
  S.[payment_terms],
  S.[payment_method],
  S.[sales_district],
  S.[sales_department],
  S.[sales_group],
  S.[currency_code],
  S.[customer_pricing_procedure],
  S.[shipping_conditions],
  S.[delivering_plant],
  S.[incoterms1],
  S.[incoterms2],
  S.[account_assignment_group],
  S.[tax_classification],
  S.[customer_contact],
  S.[customer_phone],
  S.[customer_email],
  S.[credit_level],
  S.[customer_level],
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
    ROW_NUMBER() OVER (ORDER BY N.[company_code], N.[sales_organization], N.[customer_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_code], N''))), 10), N''), N'') AS [customer_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[account_group], N''))), 4), N''), N'') AS [account_group],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[sales_organization], N''))), 4), N''), N'') AS [sales_organization],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[distribution_channel], N''))), 2), N''), N'') AS [distribution_channel],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[product_group], N''))), 2), N''), N'') AS [product_group],
      COALESCE(TRY_CAST(R.[customer_title] AS INT), 1) AS [customer_title],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_name1], N''))), 140), N''), N'') AS [customer_name1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_name2], N''))), 140), N'') AS [customer_name2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_name3], N''))), 140), N'') AS [customer_name3],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_name4], N''))), 140), N'') AS [customer_name4],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_short_name], N''))), 40), N'') AS [customer_short_name],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[registration_country], N''))), 2), N'') AS [registration_country],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[registration_region], N''))), 40), N'') AS [registration_region],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[registration_city], N''))), 40), N'') AS [registration_city],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr1], N''))), 140), N'') AS [addr1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr2], N''))), 140), N'') AS [addr2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr3], N''))), 140), N'') AS [addr3],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr4], N''))), 140), N'') AS [addr4],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr5], N''))), 140), N'') AS [addr5],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[postal_code], N''))), 10), N'') AS [postal_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[phone1], N''))), 40), N'') AS [phone1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[phone2], N''))), 40), N'') AS [phone2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[fax_number], N''))), 40), N'') AS [fax_number],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_code], N''))), 10), N''), N'') AS [supplier_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[trading_partner], N''))), 4), N''), N'') AS [trading_partner],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_number1], N''))), 40), N'') AS [tax_number1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_number2], N''))), 40), N'') AS [tax_number2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_number3], N''))), 60), N'') AS [tax_number3],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_country_code], N''))), 2), N'') AS [bank_country_code],
      CAST(NULL AS NVARCHAR(15)) AS [bank_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_account], N''))), 40), N'') AS [bank_account],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_account_holder], N''))), 80), N'') AS [bank_account_holder],
      COALESCE(TRY_CAST(R.[customer_category] AS INT), 0) AS [customer_category],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[enterprise_nature], N''))), 4), N''), N'150') AS [enterprise_nature],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[industry_attribute], N''))), 4), N''), N'C') AS [industry_attribute],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[reconciliation_account], N''))), 40), N''), N'') AS [reconciliation_account],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[payment_terms], N''))), 40), N''), N'PREPAYSHIP') AS [payment_terms],
      COALESCE(TRY_CAST(R.[payment_method] AS INT), 0) AS [payment_method],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[sales_district], N''))), 6), N'') AS [sales_district],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[sales_department], N''))), 6), N'') AS [sales_department],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[sales_group], N''))), 6), N'') AS [sales_group],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[currency_code], N''))), 3), N'') AS [currency_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_pricing_procedure], N''))), 2), N''), N'1') AS [customer_pricing_procedure],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[shipping_conditions], N''))), 2), N''), N'') AS [shipping_conditions],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[delivering_plant], N''))), 4), N''), N'') AS [delivering_plant],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[incoterms1], N''))), 3), N''), N'FOB') AS [incoterms1],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[incoterms2], N''))), 40), N''), N'') AS [incoterms2],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[account_assignment_group], N''))), 2), N''), N'') AS [account_assignment_group],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_classification], N''))), 4), N'') AS [tax_classification],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_contact], N''))), 80), N'') AS [customer_contact],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_phone], N''))), 40), N'') AS [customer_phone],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_email], N''))), 100), N'') AS [customer_email],
      COALESCE(TRY_CAST(R.[credit_level] AS INT), 0) AS [credit_level],
      COALESCE(TRY_CAST(R.[customer_level] AS INT), 0) AS [customer_level],
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
          LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3),
          LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
          LTRIM(RTRIM(ISNULL(R.[sales_organization], N''))),
          LTRIM(RTRIM(ISNULL(R.[customer_code], N'')))
        ORDER BY ISNULL(R.[is_deleted], 0), ISNULL(R.[updated_at], CAST('1900-01-01' AS DATETIME)) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[sales_organization], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[customer_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

EXEC(N'
UPDATE S
SET S.[bank_code] = B.[bank_code]
FROM #st_source S
INNER JOIN (
  SELECT
    LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''''))), 3) AS [tenant_code],
    LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''''))), 4) AS [company_code],
    LTRIM(RTRIM(ISNULL(R.[sales_organization], N''''))) AS [sales_organization],
    LTRIM(RTRIM(ISNULL(R.[customer_code], N''''))) AS [customer_code],
    NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_code], N''''))), 15), N'''') AS [bank_code],
    ROW_NUMBER() OVER (
      PARTITION BY
        LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''''))), 3),
        LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''''))), 4),
        LTRIM(RTRIM(ISNULL(R.[sales_organization], N''''))),
        LTRIM(RTRIM(ISNULL(R.[customer_code], N'''')))
      ORDER BY ISNULL(R.[is_deleted], 0), ISNULL(R.[updated_at], CAST(''1900-01-01'' AS DATETIME)) DESC
    ) AS dup_rn
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_sales_customer] R
) B
  ON B.[dup_rn] = 1
 AND B.[tenant_code] = S.[tenant_code]
 AND B.[company_code] = S.[company_code]
 AND B.[sales_organization] = S.[sales_organization]
 AND B.[customer_code] = S.[customer_code];
');

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
  GROUP BY [tenant_code], [company_code], [sales_organization], [customer_code]
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
  FROM [takt_logistics_sales_customer] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_logistics_sales_customer] T
  ON T.[tenant_code] = S.[tenant_code]
 AND T.[company_code] = S.[company_code]
 AND LTRIM(RTRIM(T.[sales_organization]))=S.[sales_organization]
  AND LTRIM(RTRIM(T.[customer_code]))=S.[customer_code];

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_sales_customer] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[sales_organization]))=S.[sales_organization]
 AND LTRIM(RTRIM(T.[customer_code]))=S.[customer_code]
WHEN MATCHED AND (
  LTRIM(RTRIM(ISNULL(T.[plant_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[plant_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[account_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[account_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[sales_organization], N'')))<>LTRIM(RTRIM(ISNULL(S.[sales_organization], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[distribution_channel], N'')))<>LTRIM(RTRIM(ISNULL(S.[distribution_channel], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[product_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[product_group], N'')))
  OR T.[customer_title]<>S.[customer_title]
  OR LTRIM(RTRIM(ISNULL(T.[customer_name1], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_name1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_name2], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_name2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_name3], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_name3], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_name4], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_name4], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_short_name], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_short_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_country], N'')))<>LTRIM(RTRIM(ISNULL(S.[registration_country], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_region], N'')))<>LTRIM(RTRIM(ISNULL(S.[registration_region], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[registration_city], N'')))<>LTRIM(RTRIM(ISNULL(S.[registration_city], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr1], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr2], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr3], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr3], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr4], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr4], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr5], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr5], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[postal_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[postal_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[phone1], N'')))<>LTRIM(RTRIM(ISNULL(S.[phone1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[phone2], N'')))<>LTRIM(RTRIM(ISNULL(S.[phone2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[fax_number], N'')))<>LTRIM(RTRIM(ISNULL(S.[fax_number], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[trading_partner], N'')))<>LTRIM(RTRIM(ISNULL(S.[trading_partner], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_number1], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_number1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_number2], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_number2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_number3], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_number3], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_country_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_country_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_account], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_account], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_account_holder], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_account_holder], N'')))
  OR T.[customer_category]<>S.[customer_category]
  OR LTRIM(RTRIM(ISNULL(T.[enterprise_nature], N'')))<>LTRIM(RTRIM(ISNULL(S.[enterprise_nature], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[industry_attribute], N'')))<>LTRIM(RTRIM(ISNULL(S.[industry_attribute], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[reconciliation_account], N'')))<>LTRIM(RTRIM(ISNULL(S.[reconciliation_account], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[payment_terms], N'')))<>LTRIM(RTRIM(ISNULL(S.[payment_terms], N'')))
  OR T.[payment_method]<>S.[payment_method]
  OR LTRIM(RTRIM(ISNULL(T.[sales_district], N'')))<>LTRIM(RTRIM(ISNULL(S.[sales_district], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[sales_department], N'')))<>LTRIM(RTRIM(ISNULL(S.[sales_department], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[sales_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[sales_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[currency_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[currency_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_pricing_procedure], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_pricing_procedure], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[shipping_conditions], N'')))<>LTRIM(RTRIM(ISNULL(S.[shipping_conditions], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[delivering_plant], N'')))<>LTRIM(RTRIM(ISNULL(S.[delivering_plant], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[incoterms1], N'')))<>LTRIM(RTRIM(ISNULL(S.[incoterms1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[incoterms2], N'')))<>LTRIM(RTRIM(ISNULL(S.[incoterms2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[account_assignment_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[account_assignment_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_classification], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_classification], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_contact], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_contact], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_phone], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_phone], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_email], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_email], N'')))
  OR T.[credit_level]<>S.[credit_level]
  OR T.[customer_level]<>S.[customer_level]
  OR T.[sort_order]<>S.[sort_order]
  OR T.[customer_status]<>S.[customer_status]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))
  OR ISNULL(T.[is_deleted], 0)<>S.[is_deleted]
  OR ISNULL(T.[ext_field], N'')<>ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'')<>ISNULL(S.[remark], N'')
) THEN
  UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[customer_code]=S.[customer_code],
  T.[account_group]=S.[account_group],
  T.[sales_organization]=S.[sales_organization],
  T.[distribution_channel]=S.[distribution_channel],
  T.[product_group]=S.[product_group],
  T.[customer_title]=S.[customer_title],
  T.[customer_name1]=S.[customer_name1],
  T.[customer_name2]=S.[customer_name2],
  T.[customer_name3]=S.[customer_name3],
  T.[customer_name4]=S.[customer_name4],
  T.[customer_short_name]=S.[customer_short_name],
  T.[registration_country]=S.[registration_country],
  T.[registration_region]=S.[registration_region],
  T.[registration_city]=S.[registration_city],
  T.[addr1]=S.[addr1],
  T.[addr2]=S.[addr2],
  T.[addr3]=S.[addr3],
  T.[addr4]=S.[addr4],
  T.[addr5]=S.[addr5],
  T.[postal_code]=S.[postal_code],
  T.[phone1]=S.[phone1],
  T.[phone2]=S.[phone2],
  T.[fax_number]=S.[fax_number],
  T.[supplier_code]=S.[supplier_code],
  T.[trading_partner]=S.[trading_partner],
  T.[tax_number1]=S.[tax_number1],
  T.[tax_number2]=S.[tax_number2],
  T.[tax_number3]=S.[tax_number3],
  T.[bank_country_code]=S.[bank_country_code],
  T.[bank_account]=S.[bank_account],
  T.[bank_account_holder]=S.[bank_account_holder],
  T.[customer_category]=S.[customer_category],
  T.[enterprise_nature]=S.[enterprise_nature],
  T.[industry_attribute]=S.[industry_attribute],
  T.[reconciliation_account]=S.[reconciliation_account],
  T.[payment_terms]=S.[payment_terms],
  T.[payment_method]=S.[payment_method],
  T.[sales_district]=S.[sales_district],
  T.[sales_department]=S.[sales_department],
  T.[sales_group]=S.[sales_group],
  T.[currency_code]=S.[currency_code],
  T.[customer_pricing_procedure]=S.[customer_pricing_procedure],
  T.[shipping_conditions]=S.[shipping_conditions],
  T.[delivering_plant]=S.[delivering_plant],
  T.[incoterms1]=S.[incoterms1],
  T.[incoterms2]=S.[incoterms2],
  T.[account_assignment_group]=S.[account_assignment_group],
  T.[tax_classification]=S.[tax_classification],
  T.[customer_contact]=S.[customer_contact],
  T.[customer_phone]=S.[customer_phone],
  T.[customer_email]=S.[customer_email],
  T.[credit_level]=S.[credit_level],
  T.[customer_level]=S.[customer_level],
  T.[sort_order]=S.[sort_order],
  T.[customer_status]=S.[customer_status],
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
    [id],[plant_code],[customer_code],[account_group],[sales_organization],[distribution_channel],[product_group],[customer_title],[customer_name1],[customer_name2],[customer_name3],[customer_name4],[customer_short_name],[registration_country],[registration_region],[registration_city],[addr1],[addr2],[addr3],[addr4],[addr5],[postal_code],[phone1],[phone2],[fax_number],[supplier_code],[trading_partner],[tax_number1],[tax_number2],[tax_number3],[bank_country_code],[bank_account],[bank_account_holder],[customer_category],[enterprise_nature],[industry_attribute],[reconciliation_account],[payment_terms],[payment_method],[sales_district],[sales_department],[sales_group],[currency_code],[customer_pricing_procedure],[shipping_conditions],[delivering_plant],[incoterms1],[incoterms2],[account_assignment_group],[tax_classification],[customer_contact],[customer_phone],[customer_email],[credit_level],[customer_level],[sort_order],[customer_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[customer_code],S.[account_group],S.[sales_organization],S.[distribution_channel],S.[product_group],S.[customer_title],S.[customer_name1],S.[customer_name2],S.[customer_name3],S.[customer_name4],S.[customer_short_name],S.[registration_country],S.[registration_region],S.[registration_city],S.[addr1],S.[addr2],S.[addr3],S.[addr4],S.[addr5],S.[postal_code],S.[phone1],S.[phone2],S.[fax_number],S.[supplier_code],S.[trading_partner],S.[tax_number1],S.[tax_number2],S.[tax_number3],S.[bank_country_code],S.[bank_account],S.[bank_account_holder],S.[customer_category],S.[enterprise_nature],S.[industry_attribute],S.[reconciliation_account],S.[payment_terms],S.[payment_method],S.[sales_district],S.[sales_department],S.[sales_group],S.[currency_code],S.[customer_pricing_procedure],S.[shipping_conditions],S.[delivering_plant],S.[incoterms1],S.[incoterms2],S.[account_assignment_group],S.[tax_classification],S.[customer_contact],S.[customer_phone],S.[customer_email],S.[credit_level],S.[customer_level],S.[sort_order],S.[customer_status],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
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

EXEC(N'
UPDATE T
SET T.[bank_code] = S.[bank_code]
FROM [dbo].[takt_logistics_sales_customer] T
INNER JOIN #st_source S
  ON T.[tenant_code] = S.[tenant_code]
 AND T.[company_code] = S.[company_code]
 AND LTRIM(RTRIM(T.[sales_organization])) = S.[sales_organization]
 AND LTRIM(RTRIM(T.[customer_code])) = S.[customer_code]
WHERE LTRIM(RTRIM(ISNULL(T.[bank_code], N''''))) <> LTRIM(RTRIM(ISNULL(S.[bank_code], N'''')));
');

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
  FROM [takt_logistics_sales_customer] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
    AND NOT EXISTS (
      SELECT 1 FROM #st_source S
      WHERE S.[tenant_code] = T.[tenant_code]
        AND S.[company_code] = T.[company_code]
        AND S.[sales_organization] = LTRIM(RTRIM(T.[sales_organization]))
        AND S.[customer_code] = LTRIM(RTRIM(T.[customer_code]))
    );
  SET @dml_n = @@ROWCOUNT;
  SET @delete_count = @delete_count + @dml_n;
END

DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_sales_customer] T
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
  N'takt_logistics_sales_customer',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE TaktCustomer Sync (summary)',
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
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count];
