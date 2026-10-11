// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：mdl_sync.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：机种目的地同步（源库 PP_SapModelDest → takt_logistics_materials_model_destination；含 delta/oper 日志）
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
 * 生成机种-目的地 MERGE 同步 SQL（含 #cjs_source、delta/oper 日志）
 * @returns {string}
 */
function makeCjsSyncSql() {
  const batchSizeValue = resolveBatchSizeSqlValue();
  return `
SET NOCOUNT ON;

DECLARE @batch_size INT = ${batchSizeValue};
DECLARE @now DATETIME = GETDATE();
DECLARE @base_epoch BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', GETUTCDATE()) * 1000;

/* ========== 1. 源数据 ========== */
IF OBJECT_ID('tempdb..#cjs_source') IS NOT NULL DROP TABLE #cjs_source;
CREATE TABLE #cjs_source (
  [rn] INT IDENTITY(1,1) PRIMARY KEY,
  [material_code] NVARCHAR(100),
  [model_code] NVARCHAR(100),
  [destination_code] NVARCHAR(100)
);

INSERT INTO #cjs_source ([material_code], [model_code], [destination_code])
SELECT
  LTRIM(RTRIM([D_SAP_DEST_Z001])),
  LTRIM(RTRIM([D_SAP_DEST_Z002])),
  LTRIM(RTRIM([D_SAP_DEST_Z003]))
FROM (
  SELECT *, ROW_NUMBER() OVER (ORDER BY [D_SAP_DEST_Z001], [D_SAP_DEST_Z002], [D_SAP_DEST_Z003]) AS rn
  FROM [Sap_Data].[dbo].[PP_SapModelDest]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

/* ========== 2. Delta 表 ========== */
IF OBJECT_ID('tempdb..#cjs_delta') IS NOT NULL DROP TABLE #cjs_delta;
CREATE TABLE #cjs_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  tenant_code NVARCHAR(20),
  company_code NVARCHAR(20),
  sort_order_old INT,
  sort_order_new INT,
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

/* ========== 3. MERGE（✅ rn 从 #cjs_source 取出） ========== */
MERGE INTO [takt_logistics_materials_model_destination] AS T
USING (
  SELECT
    @base_epoch + S.[rn] AS [id],
    S.[rn],
    S.[material_code],
    S.[model_code],
    S.[destination_code]
  FROM #cjs_source S
) AS S
ON  T.[material_code]    = S.[material_code]
AND T.[model_code]       = S.[model_code]
AND T.[destination_code] = S.[destination_code]
WHEN MATCHED THEN
  UPDATE SET
    T.[sort_order]   = 0,
    T.[tenant_code]  = '000',
    T.[ext_field]    = '{}',
    T.[remark]       = '幂等更新',
    T.[updated_by]   = '900001',
    T.[updated_at]   = @now
WHEN NOT MATCHED THEN
  INSERT (
    [id],
    [material_code],
    [material_description],
    [model_code],
    [model_name],
    [destination_code],
    [destination_name],
    [sort_order],
    [tenant_code],
    [ext_field],
    [remark],
    [created_by],
    [created_at],
    [updated_by],
    [updated_at],
    [is_deleted],
    [deleted_by],
    [deleted_at]
  )
  VALUES (
    S.[id],
    S.[material_code],
    '',
    S.[model_code],
    '',
    S.[destination_code],
    '',
    0,
    '000',
    '{}',
    '幂等更新',
    '900001',
    @now,
    '900001',
    @now,
    0,
    NULL,
    NULL
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  '000',
  '2300',
  DELETED.[sort_order], INSERTED.[sort_order],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #cjs_delta(
  rn, oper_type, id,
  tenant_code, company_code,
  sort_order_old, sort_order_new,
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
  @base_epoch + d.rn,
  d.oper_type,
  N'takt_logistics_materials_model_destination',
  d.id,
  CASE WHEN d.oper_type = 'UPDATE' THEN
    (
      SELECT
        d.sort_order_old AS sort_order,
        d.ext_field_old AS ext_field,
        d.remark_old AS remark
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    )
  ELSE '{}' END,
  (
    SELECT
      d.sort_order_new AS sort_order,
      d.ext_field_new AS ext_field,
      d.remark_new AS remark
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  CASE WHEN d.oper_type = 'UPDATE' THEN
    (
      SELECT
        ISNULL(CAST(d.sort_order_old AS NVARCHAR), 'null') AS [sort_order.old],
        ISNULL(CAST(d.sort_order_new AS NVARCHAR), 'null') AS [sort_order.new],
        ISNULL(d.remark_old, 'null') AS [remark.old],
        ISNULL(d.remark_new, 'null') AS [remark.new]
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    )
  ELSE '{}' END,
  N'MERGE Model Destination Sync',
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
FROM #cjs_delta d;

/* ========== 5. oper_log ========== */
DECLARE @ins INT = (SELECT COUNT(*) FROM #cjs_delta WHERE oper_type = 'INSERT');
DECLARE @upd INT = (SELECT COUNT(*) FROM #cjs_delta WHERE oper_type = 'UPDATE');
DECLARE @json NVARCHAR(MAX) = N'{"insert":' + ISNULL(CAST(@ins AS NVARCHAR),'0') + N',"update":' + ISNULL(CAST(@upd AS NVARCHAR),'0') + N'}';

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
  @base_epoch + 1,
  N'SYSTEM_SAP_SYNC',
  N'SAP_SYNC',
  N'机种-目的地管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/sap/model_destination',
  CONCAT('batch_size=', @batch_size),
  @json,
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

/* ========== 6. 汇总日志 ========== */
SELECT 'INS', @ins;
SELECT 'UPD', @upd;
`;
}

// ========================================
// 主流程
// ========================================

(async () => {
  console.log('==========================================');
  console.log('  机种目的地同步（mdl）');
  console.log(`  BATCH_SIZE: ${formatBatchSizeLabel()}`);
  console.log('==========================================');

  const cjsTotal = Number(execSQLValue(
    `SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapModelDest]`,
    { filePrefix: 'mdl_val' }
  ));
  console.log(`\n📋 机种目的地源表数据：${cjsTotal} 条`);

  const cjsResult = await execSQL(makeCjsSyncSql(), '机种目的地同步', { filePrefix: 'mdl_sync' });
  const cjsCounts = parseInsUpdSummaryCounts(cjsResult);

  console.log('\n==========================================');
  console.log('📊 最终汇总日志');
  console.log('------------------------------------------');
  console.log('【机种目的地】');
  console.log(`   插入：${cjsCounts.mainInsert}`);
  console.log(`   更新：${cjsCounts.mainUpdate}`);
  console.log(`   目标表总记录数：${execSQLValue(
    'SELECT COUNT(*) FROM [takt_logistics_materials_model_destination]',
    { filePrefix: 'mdl_val' }
  )}`);
  console.log('==========================================');
})();