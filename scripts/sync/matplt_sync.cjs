// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：matplt_sync.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂物料同步（源库 PP_SapMaterial → takt_logistics_materials_material_plant；含 delta/oper 日志）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const {
  BATCH_SIZE,
  execSQL,
  execSQLValue,
  formatBatchSizeLabel,
  parseInsUpdSummaryCounts,
  resolveBatchSizeSqlValue,
} = require('./common_sync.cjs');

/**
 * 生成物料主数据 MERGE 同步 SQL（含 #delta、oper_log、delta_log）
 * @returns {string}
 */
function makeSyncSql() {
  const batchSizeValue = resolveBatchSizeSqlValue();
  return `
SET NOCOUNT ON;

DECLARE @batch_size INT = ${batchSizeValue};
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

/* ========== 1. 源数据 ========== */
IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [material_code] NVARCHAR(100),
  [material_description] NVARCHAR(40),
  [material_specification] NVARCHAR(70),
  [industry_sector] NVARCHAR(100),
  [material_hierarchy] NVARCHAR(100),
  [material_group] NVARCHAR(100),
  [material_type] NVARCHAR(100),
  [base_unit] NVARCHAR(50),
  [purchase_group] NVARCHAR(100),
  [purchase_type] NVARCHAR(100),
  [special_procurement] NVARCHAR(100),
  [is_bulk] INT,
  [min_order_quantity] DECIMAL(18,2),
  [rounding_value] DECIMAL(18,2),
  [planned_delivery_time_days] DECIMAL(18,2),
  [in_house_production_days] DECIMAL(18,2),
  [manufacturer] NVARCHAR(200),
  [manufacturer_material_code] NVARCHAR(200),
  [currency] NVARCHAR(20),
  [price_control] NVARCHAR(100),
  [price_unit] NVARCHAR(100),
  [valuation] NVARCHAR(100),
  [moving_price] DECIMAL(18,2),
  [difference_code] NVARCHAR(100),
  [profit_center] NVARCHAR(100),
  [current_stock] DECIMAL(18,2),
  [production_location] NVARCHAR(100),
  [purchasing_location] NVARCHAR(100),
  [storage_location] NVARCHAR(100),
  [is_inspection] INT,
  [is_batch] INT,
  [discontinued_status] NVARCHAR(20),
  [material_status] NVARCHAR(100),
  [tenant_code] NVARCHAR(50),
  [company_code] NVARCHAR(50),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] NVARCHAR(50),
  [created_at] DATETIME2,
  [updated_by] NVARCHAR(50),
  [updated_at] DATETIME2,
  [is_deleted] BIT,
  [deleted_by] NVARCHAR(50),
  [deleted_at] DATETIME2
);

INSERT INTO #st_source
SELECT
  S.rn,
  CAST((DATEDIFF_BIG(MICROSECOND, '1970-01-01', GETDATE()) * 1000 + S.rn) AS BIGINT),
  LTRIM(RTRIM(S.[plant_code])),
  LTRIM(RTRIM(S.[material_code])),
  LEFT(ISNULL(NULLIF(LTRIM(RTRIM(S.[material_description])), ''), ''), 40),
  CAST(N'' AS NVARCHAR(70)),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[industry_sector])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[material_hierarchy])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[material_group])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[material_type])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[base_unit])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[purchase_group])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[purchase_type])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[special_procurement])), ''), ''),
  CASE WHEN LTRIM(RTRIM(S.[is_bulk])) = 'X' THEN 1 ELSE 0 END,
  COALESCE(TRY_CAST(S.[min_order_quantity] AS DECIMAL(18,2)), 0),
  COALESCE(TRY_CAST(S.[rounding_value] AS DECIMAL(18,2)), 0),
  COALESCE(TRY_CAST(S.[planned_delivery_time_days] AS DECIMAL(18,2)), 0),
  COALESCE(TRY_CAST(S.[in_house_production_days] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[manufacturer])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[manufacturer_material_code])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[currency])), ''), 'CNY'),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[price_control])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[price_unit])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[valuation])), ''), ''),
  COALESCE(TRY_CAST(S.[moving_price] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[difference_code])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(LEFT(S.[profit_center], 4))), ''), ''),
  COALESCE(TRY_CAST(S.[current_stock] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[production_location])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[purchasing_location])), ''), ''),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[storage_location])), ''), ''),
  CASE WHEN LTRIM(RTRIM(S.[is_inspection])) = 'X' THEN 1 ELSE 0 END,
  CASE WHEN LTRIM(RTRIM(S.[is_batch])) = 'X' THEN 1 ELSE 0 END,
  COALESCE(NULLIF(LTRIM(RTRIM(S.[discontinued_status])), ''), 'Z0'),
  '1',
  '000',
  '2300',
  '{}',
  '',
  '900001',
  GETDATE(),
  '900001',
  GETDATE(),
  0,
  NULL,
  NULL
FROM (
  SELECT
    [D_SAP_ZCA1D_Z001] AS plant_code,
    [D_SAP_ZCA1D_Z002] AS material_code,
    [D_SAP_ZCA1D_Z003] AS industry_sector,
    [D_SAP_ZCA1D_Z004] AS material_type,
    [D_SAP_ZCA1D_Z005] AS material_description,
    [D_SAP_ZCA1D_Z006] AS base_unit,
    [D_SAP_ZCA1D_Z007] AS material_hierarchy,
    [D_SAP_ZCA1D_Z008] AS material_group,
    [D_SAP_ZCA1D_Z009] AS purchase_group,
    [D_SAP_ZCA1D_Z010] AS purchase_type,
    [D_SAP_ZCA1D_Z011] AS special_procurement,
    [D_SAP_ZCA1D_Z012] AS is_bulk,
    [D_SAP_ZCA1D_Z013] AS min_order_quantity,
    [D_SAP_ZCA1D_Z015] AS rounding_value,
    [D_SAP_ZCA1D_Z017] AS planned_delivery_time_days,
    [D_SAP_ZCA1D_Z018] AS in_house_production_days,
    [D_SAP_ZCA1D_Z019] AS is_inspection,
    [D_SAP_ZCA1D_Z020] AS profit_center,
    [D_SAP_ZCA1D_Z021] AS difference_code,
    [D_SAP_ZCA1D_Z022] AS is_batch,
    [D_SAP_ZCA1D_Z023] AS manufacturer,
    [D_SAP_ZCA1D_Z024] AS manufacturer_material_code,
    [D_SAP_ZCA1D_Z025] AS valuation,
    [D_SAP_ZCA1D_Z026] AS moving_price,
    [D_SAP_ZCA1D_Z027] AS currency,
    [D_SAP_ZCA1D_Z028] AS price_control,
    [D_SAP_ZCA1D_Z029] AS price_unit,
    [D_SAP_ZCA1D_Z030] AS production_location,
    [D_SAP_ZCA1D_Z031] AS purchasing_location,
    [D_SAP_ZCA1D_Z032] AS storage_location,
    [D_SAP_ZCA1D_Z033] AS current_stock,
    [D_SAP_ZCA1D_Z034] AS discontinued_status,
    ROW_NUMBER() OVER (ORDER BY [D_SAP_ZCA1D_Z001], [D_SAP_ZCA1D_Z002]) AS rn
  FROM [Sap_Data].[dbo].[PP_SapMaterial]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

/* ========== 2. Delta 表 ========== */
IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(100),
  material_code NVARCHAR(100),
  tenant_code NVARCHAR(50),
  company_code NVARCHAR(50),
  material_description_old NVARCHAR(40),
  material_description_new NVARCHAR(40),
  material_type_old NVARCHAR(100),
  material_type_new NVARCHAR(100),
  base_unit_old NVARCHAR(50),
  base_unit_new NVARCHAR(50),
  material_group_old NVARCHAR(100),
  material_group_new NVARCHAR(100),
  current_stock_old DECIMAL(18,2),
  current_stock_new DECIMAL(18,2),
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

/* ========== 3. MERGE（目标列对齐 TaktMaterialPlant） ========== */
MERGE INTO [takt_logistics_materials_material_plant] AS T
USING #st_source AS S
ON T.[plant_code] = S.[plant_code]
AND T.[material_code] = S.[material_code]
WHEN MATCHED THEN
  UPDATE SET
    T.[material_description] = S.[material_description],
    T.[material_specification] = S.[material_specification],
    T.[industry_sector] = S.[industry_sector],
    T.[material_hierarchy] = S.[material_hierarchy],
    T.[material_group] = S.[material_group],
    T.[material_type] = S.[material_type],
    T.[base_unit] = S.[base_unit],
    T.[purchase_group] = S.[purchase_group],
    T.[purchase_type] = S.[purchase_type],
    T.[special_procurement] = S.[special_procurement],
    T.[is_bulk] = S.[is_bulk],
    T.[min_order_quantity] = S.[min_order_quantity],
    T.[rounding_value] = S.[rounding_value],
    T.[planned_delivery_time_days] = S.[planned_delivery_time_days],
    T.[in_house_production_days] = S.[in_house_production_days],
    T.[manufacturer] = S.[manufacturer],
    T.[manufacturer_material_code] = S.[manufacturer_material_code],
    T.[currency_code] = S.[currency],
    T.[price_control] = S.[price_control],
    T.[price_unit] = S.[price_unit],
    T.[valuation] = S.[valuation],
    T.[moving_price] = S.[moving_price],
    T.[difference_code] = S.[difference_code],
    T.[profit_center] = S.[profit_center],
    T.[current_stock] = S.[current_stock],
    T.[production_location] = S.[production_location],
    T.[purchasing_location] = S.[purchasing_location],
    T.[storage_location] = S.[storage_location],
    T.[requires_inspection] = S.[is_inspection],
    T.[is_batch] = S.[is_batch],
    T.[discontinued_status] = S.[discontinued_status],
    T.[material_status] = S.[material_status],
    T.[tenant_code] = S.[tenant_code],
    T.[company_code] = S.[company_code],
    T.[ext_field] = S.[ext_field],
    T.[remark] = S.[remark],
    T.[updated_by] = S.[updated_by],
    T.[updated_at] = S.[updated_at],
    T.[is_deleted] = S.[is_deleted],
    T.[deleted_by] = S.[deleted_by],
    T.[deleted_at] = S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[material_code],[material_description],[material_specification],
    [industry_sector],[material_hierarchy],[material_group],[material_type],[base_unit],
    [purchase_group],[purchase_type],[special_procurement],[is_bulk],[min_order_quantity],
    [rounding_value],[planned_delivery_time_days],[in_house_production_days],[manufacturer],
    [manufacturer_material_code],[currency_code],[price_control],[price_unit],[valuation],
    [moving_price],[difference_code],[profit_center],[current_stock],[production_location],
    [purchasing_location],[storage_location],[requires_inspection],[is_batch],[discontinued_status],
    [material_status],[tenant_code],[company_code],[ext_field],[remark],[created_by],
    [created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[material_code],S.[material_description],S.[material_specification],
    S.[industry_sector],S.[material_hierarchy],S.[material_group],S.[material_type],S.[base_unit],
    S.[purchase_group],S.[purchase_type],S.[special_procurement],S.[is_bulk],S.[min_order_quantity],
    S.[rounding_value],S.[planned_delivery_time_days],S.[in_house_production_days],S.[manufacturer],
    S.[manufacturer_material_code],S.[currency],S.[price_control],S.[price_unit],S.[valuation],
    S.[moving_price],S.[difference_code],S.[profit_center],S.[current_stock],S.[production_location],
    S.[purchasing_location],S.[storage_location],S.[is_inspection],S.[is_batch],S.[discontinued_status],
    S.[material_status],S.[tenant_code],S.[company_code],S.[ext_field],S.[remark],S.[created_by],
    S.[created_at],S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at]
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[material_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  DELETED.[material_description], INSERTED.[material_description],
  DELETED.[material_type], INSERTED.[material_type],
  DELETED.[base_unit], INSERTED.[base_unit],
  DELETED.[material_group], INSERTED.[material_group],
  DELETED.[current_stock], INSERTED.[current_stock],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #delta(
  rn, oper_type, id, plant_code, material_code,
  tenant_code, company_code,
  material_description_old, material_description_new,
  material_type_old, material_type_new,
  base_unit_old, base_unit_new,
  material_group_old, material_group_new,
  current_stock_old, current_stock_new,
  ext_field_old, ext_field_new,
  remark_old, remark_new
);

/* ========== 4. delta_log ========== */
INSERT INTO [takt_statistics_logging_delta_log] (
  [id],
  [oper_type],
  [table_name],
  [primary_key_id],
  [before_data],
  [after_data],
  [diff_data],
  [sql_statement],
  [oper_ip],
  [oper_location],
  [user_agent],
  [browser],
  [os],
  [device_type],
  [oper_time],
  [elapsed_time],
  [tenant_code],
  [company_code],
  [ext_field],
  [remark],
  [created_by],
  [created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_materials_material_plant',
  d.id,
  CASE WHEN d.oper_type = 'UPDATE' THEN
    (
      SELECT
        d.material_description_old AS material_description,
        d.material_type_old AS material_type,
        d.base_unit_old AS base_unit,
        d.material_group_old AS material_group,
        d.current_stock_old AS current_stock
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    )
  ELSE '{}' END,
  (
    SELECT
      d.material_description_new AS material_description,
      d.material_type_new AS material_type,
      d.base_unit_new AS base_unit,
      d.material_group_new AS material_group,
      d.current_stock_new AS current_stock
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  CASE WHEN d.oper_type = 'UPDATE' THEN
    (
      SELECT
        ISNULL(d.material_description_old, 'null') AS [material_description.old],
        ISNULL(d.material_description_new, 'null') AS [material_description.new],
        ISNULL(CAST(d.current_stock_old AS NVARCHAR), 'null') AS [current_stock.old],
        ISNULL(CAST(d.current_stock_new AS NVARCHAR), 'null') AS [current_stock.new]
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    )
  ELSE '{}' END,
  N'MERGE GeneralMaterial Sync',
  '127.0.0.1',
  'Server',
  'SQLCMD',
  'Server',
  'Windows',
  'Server',
  @now,
  0,
  d.tenant_code,
  d.company_code,
  '{}',
  N'SAP_SYNC',
  '900001',
  @now
FROM #delta d;

/* ========== 5. oper_log ========== */
DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @json_result NVARCHAR(MAX) = N'{"insert":' + ISNULL(CAST(@insert_count AS NVARCHAR),'0') + N',"update":' + ISNULL(CAST(@update_count AS NVARCHAR),'0') + N'}';

INSERT INTO [takt_statistics_logging_oper_log] (
  [id],
  [user_name],
  [oper_type],
  [oper_module],
  [oper_method],
  [request_method],
  [oper_url],
  [request_param],
  [json_result],
  [oper_ip],
  [oper_location],
  [user_agent],
  [browser],
  [os],
  [device_type],
  [oper_time],
  [elapsed_time],
  [oper_status],
  [error_msg],
  [tenant_code],
  [company_code],
  [created_by],
  [created_at]
)
SELECT
  @base_id + 1,
  N'SYSTEM_SAP_SYNC',
  N'SAP_SYNC',
  N'物料主数据管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/sap/material',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1',
  'Server',
  'SQLCMD',
  'Server',
  'Windows',
  'Server',
  @now,
  DATEDIFF(MILLISECOND, @now, GETDATE()),
  1,
  '',
  '000',
  '2300',
  '900001',
  @now;

/* ========== 6. 汇总 ========== */
SELECT 'INS', @insert_count;
SELECT 'UPD', @update_count;
`;
}

// ========================================
// 主流程
// ========================================

(async () => {
  console.log('==========================================');
  console.log('  工厂物料同步（matplt）');
  console.log(`  BATCH_SIZE: ${formatBatchSizeLabel()}`);
  console.log('==========================================');

  const totalSourceRows = Number(execSQLValue(
    `SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapMaterial]`,
    { filePrefix: 'matplt_val' }
  ));
  console.log(`\n📋 发现源表数据 ${totalSourceRows} 条，${BATCH_SIZE === 0 ? '执行全量同步' : '仅处理前 ' + BATCH_SIZE + ' 条'}`);

  const syncSql = makeSyncSql();
  const syncResult = await execSQL(syncSql, '工厂物料同步', { filePrefix: 'matplt_sync' });
  const summaryCounts = parseInsUpdSummaryCounts(syncResult);

  console.log('\n==========================================');
  console.log('📊 最终日志:');
  console.log('  插入：' + summaryCounts.mainInsert);
  console.log('  更新：' + summaryCounts.mainUpdate);
  console.log('  目标表总记录数：' + execSQLValue(
    'SELECT COUNT(*) FROM [takt_logistics_materials_material_plant]',
    { filePrefix: 'matplt_val' }
  ));
  console.log('==========================================');
})();