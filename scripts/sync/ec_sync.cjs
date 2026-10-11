// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：ec_sync.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更同步（PP_SapEcn/PP_SapEcnSub → ec_source + ec_source_detail；增量 INSERT；含 delta/oper 日志）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const {
  BATCH_SIZE,
  execSQL,
  execSQLValue,
  formatBatchSizeLabel,
} = require('./common_sync.cjs');

/**
 * 从 sqlcmd 输出解析主表/明细 INSERT 汇总行
 * @param {string} text sqlcmd stdout
 * @returns {{ mainInsert: number, detailInsert: number }}
 */
function parseEcSummaryCounts(text) {
  const result = { mainInsert: 0, detailInsert: 0 };
  const m1 = text.match(/MAIN_INS\s+(\d+)/i);
  const m2 = text.match(/DETAIL_INS\s+(\d+)/i);
  result.mainInsert = m1 ? Number(m1[1]) : 0;
  result.detailInsert = m2 ? Number(m2[1]) : 0;
  return result;
}

// ========================================
// 主流程（主表批次内联 SQL；明细随主表 EC 号关联插入）
// ========================================

(async () => {
  console.log('==========================================');
  console.log('  EC main + detail sync');
  console.log('  BATCH_SIZE: ' + formatBatchSizeLabel());
  console.log('==========================================');

  const totalMainRows = Number(execSQLValue(
    "SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapEcn] WHERE LTRIM(RTRIM([D_SAP_ZPABD_Z001])) <> ''",
    { filePrefix: 'ec_val' }
  ));
  console.log('');
  console.log('Main source rows: ' + totalMainRows);
  if (!Number.isFinite(totalMainRows) || totalMainRows <= 0) {
    console.log('No main source data found, skip sync.');
    return;
  }

  const batchSize = BATCH_SIZE === 0 ? totalMainRows : Math.min(BATCH_SIZE, totalMainRows);
  console.log('Syncing ' + batchSize + ' main rows (full: ' + (BATCH_SIZE === 0) + ')');

  const syncSQL = `
SET NOCOUNT ON;

IF OBJECT_ID('tempdb..#source_main') IS NOT NULL DROP TABLE #source_main;
IF OBJECT_ID('tempdb..#source_detail') IS NOT NULL DROP TABLE #source_detail;
IF OBJECT_ID('tempdb..#main_inserted') IS NOT NULL DROP TABLE #main_inserted;
IF OBJECT_ID('tempdb..#detail_inserted') IS NOT NULL DROP TABLE #detail_inserted;

CREATE TABLE #source_main (source_ec_no NVARCHAR(100));
CREATE TABLE #source_detail (
  source_ec_no NVARCHAR(100),
  legacy_part_no NVARCHAR(100),
  source_root_material_code NVARCHAR(500),
  source_parent_material_code NVARCHAR(500),
  source_old_material_description NVARCHAR(MAX),
  source_old_usage_quantity NVARCHAR(MAX),
  source_old_item_position NVARCHAR(MAX),
  source_new_material_code NVARCHAR(MAX),
  source_new_material_description NVARCHAR(MAX),
  source_new_usage_quantity NVARCHAR(MAX),
  source_new_item_position NVARCHAR(MAX),
  source_bom_code NVARCHAR(MAX),
  source_compatibility NVARCHAR(MAX),
  source_2nd_vendor NVARCHAR(MAX),
  source_instruction NVARCHAR(MAX),
  source_old_part_disposition NVARCHAR(MAX),
  source_bom_effective_date DATE,
  created_at DATETIME
);
CREATE TABLE #main_inserted (id BIGINT, source_ec_no NVARCHAR(100));
CREATE TABLE #detail_inserted (id BIGINT, source_ec_id BIGINT, legacy_part_no NVARCHAR(100));

INSERT INTO #source_main (source_ec_no)
SELECT source_ec_no FROM (
  SELECT LTRIM(RTRIM([D_SAP_ZPABD_Z001])) AS source_ec_no,
    ROW_NUMBER() OVER (ORDER BY LTRIM(RTRIM([D_SAP_ZPABD_Z001]))) AS rn
  FROM [Sap_Data].[dbo].[PP_SapEcn]
  WHERE LTRIM(RTRIM([D_SAP_ZPABD_Z001])) <> ''
) X
WHERE rn BETWEEN 1 AND ${batchSize};

INSERT INTO [takt_logistics_manufacturing_ec_source]
([id],[source_ec_no],[source_model],[source_title],[source_status],
 [source_issue_date],[source_tcj_owner],[source_tcj_dependency],
 [source_ec_meeting],[source_pp_no],[source_technical_notice_no],
 [source_implementation],[source_main_change_reason],
 [source_secondary_change_reason],[source_safety_regulation],
 [source_progress_status],[source_serial_number_control],
 [source_customer_approval],[source_service_manual_revision],
 [source_user_manual_revision],[source_promotion_manual_revision],
 [source_standard_document_revision],[source_information_release],
 [source_cost_change],[source_unit_cost],
 [source_mold_modification_cost],[source_related_drawing],
 [source_ec_content],[tenant_code],[company_code],
 [created_by],[created_at],[updated_by],[updated_at],[is_deleted])
OUTPUT INSERTED.[id], INSERTED.[source_ec_no] INTO #main_inserted
SELECT
  CAST((DATEDIFF_BIG(MICROSECOND, '1970-01-01', GETDATE()) * 1000 + ROW_NUMBER() OVER (ORDER BY S.source_ec_no)) AS BIGINT),
  S.source_ec_no,
  ISNULL(S1.[D_SAP_ZPABD_Z002], ''),
  ISNULL(S1.[D_SAP_ZPABD_Z003], ''),
  ISNULL(S1.[D_SAP_ZPABD_Z004], ''),
  COALESCE(TRY_CONVERT(DATE, NULLIF(LTRIM(RTRIM(S1.[D_SAP_ZPABD_Z005])), ''), 23), CAST('1900-01-01' AS DATE)),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z006] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z007] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z008] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z009] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z010] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z011] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z012] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z013] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z014] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z015] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z016] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z017] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z018] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z019] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z020] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z021] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z022] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z023] AS NVARCHAR(MAX)), ''),
  ISNULL(TRY_CAST(S1.[D_SAP_ZPABD_Z024] AS DECIMAL(18,2)), 0),
  ISNULL(TRY_CAST(S1.[D_SAP_ZPABD_Z025] AS DECIMAL(18,2)), 0),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z026] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S1.[D_SAP_ZPABD_Z027] AS NVARCHAR(MAX)), N''),
  '000', '2300', '900001', COALESCE(TRY_CAST(S1.[CreateTime] AS DATETIME), GETDATE()), '900001', GETDATE(), 0
FROM #source_main S
INNER JOIN [Sap_Data].[dbo].[PP_SapEcn] S1
  ON LTRIM(RTRIM(S1.[D_SAP_ZPABD_Z001])) = S.source_ec_no
WHERE NOT EXISTS (
  SELECT 1 FROM [takt_logistics_manufacturing_ec_source] T
  WHERE T.[source_ec_no] = S.source_ec_no
);

INSERT INTO #source_detail (
  source_ec_no, legacy_part_no,
  source_root_material_code, source_parent_material_code,
  source_old_material_description, source_old_usage_quantity,
  source_old_item_position, source_new_material_code,
  source_new_material_description, source_new_usage_quantity,
  source_new_item_position, source_bom_code,
  source_compatibility, source_2nd_vendor,
  source_instruction, source_old_part_disposition,
  source_bom_effective_date, created_at
)
SELECT
  LTRIM(RTRIM(S.[D_SAP_ZPABD_S001])),
  ISNULL(S.[D_SAP_ZPABD_S004], ''),
  ISNULL(S.[D_SAP_ZPABD_S002], ''),
  ISNULL(S.[D_SAP_ZPABD_S003], ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S005] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S006] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S007] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S008] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S009] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S010] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S011] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S012] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S013] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S014] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S015] AS NVARCHAR(MAX)), ''),
  ISNULL(CAST(S.[D_SAP_ZPABD_S016] AS NVARCHAR(MAX)), ''),
  TRY_CONVERT(DATE, NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPABD_S017])), ''), 23),
  COALESCE(TRY_CAST(S.[CreateTime] AS DATETIME), GETDATE())
FROM [Sap_Data].[dbo].[PP_SapEcnSub] S
INNER JOIN #source_main SM
  ON SM.source_ec_no = LTRIM(RTRIM(S.[D_SAP_ZPABD_S001]))
WHERE LTRIM(RTRIM(S.[D_SAP_ZPABD_S001])) <> '';

INSERT INTO [takt_logistics_manufacturing_ec_source_detail]
([id],[source_ec_id],[source_root_material_code],[source_parent_material_code],
 [source_old_material_code],[source_old_material_description],[source_old_usage_quantity],
 [source_old_item_position],[source_new_material_code],
 [source_new_material_description],[source_new_usage_quantity],
 [source_new_item_position],[source_bom_code],
 [source_compatibility],[source_2nd_vendor],
 [source_instruction],[source_old_part_disposition],
 [source_bom_effective_date],[tenant_code],[company_code],
 [created_by],[created_at],[updated_by],[updated_at],[is_deleted])
OUTPUT INSERTED.[id], INSERTED.[source_ec_id], INSERTED.[source_old_material_code] INTO #detail_inserted
SELECT
  CAST((DATEDIFF_BIG(MICROSECOND, '1970-01-01', GETDATE()) * 1000 + ROW_NUMBER() OVER (ORDER BY D.source_ec_no, D.legacy_part_no)) AS BIGINT),
  M.[id],
  D.source_root_material_code,
  D.source_parent_material_code,
  D.legacy_part_no,
  D.source_old_material_description,
  D.source_old_usage_quantity,
  D.source_old_item_position,
  D.source_new_material_code,
  D.source_new_material_description,
  D.source_new_usage_quantity,
  D.source_new_item_position,
  D.source_bom_code,
  D.source_compatibility,
  D.source_2nd_vendor,
  D.source_instruction,
  D.source_old_part_disposition,
  D.source_bom_effective_date,
  '000', '2300', '900001', COALESCE(D.created_at, GETDATE()), '900001', GETDATE(), 0
FROM #source_detail D
INNER JOIN [takt_logistics_manufacturing_ec_source] M
  ON M.[source_ec_no] = D.source_ec_no
WHERE NOT EXISTS (
  SELECT 1 FROM [takt_logistics_manufacturing_ec_source_detail] T
  WHERE T.[source_ec_id] = M.[id]
    AND T.[source_old_material_code] = D.legacy_part_no
);

-- ========== 以下为新增日志部分 ==========

-- delta_log: main
INSERT INTO [takt_statistics_logging_delta_log] (
  [id], [oper_type], [table_name], [primary_key_id],
  [before_data], [after_data], [diff_data],
  [sql_statement], [oper_ip], [oper_location],
  [user_agent], [browser], [os], [device_type],
  [oper_time], [elapsed_time],
  [tenant_code], [company_code],
  [ext_field], [remark],
  [created_by], [created_at]
)
SELECT
  mi.[id],
  'INSERT',
  N'takt_logistics_manufacturing_ec_source',
  mi.[id],
  '{}',
  (SELECT mi.source_ec_no AS source_ec_no FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
  '{}',
  N'INSERT EC Main',
  '127.0.0.1', 'Server',
  'SQLCMD', 'Server', 'Windows', 'Server',
  GETDATE(), 0,
  '000', '2300',
  '{}', N'SAP_SYNC',
  '900001', GETDATE()
FROM #main_inserted mi;

-- delta_log: detail
INSERT INTO [takt_statistics_logging_delta_log] (
  [id], [oper_type], [table_name], [primary_key_id],
  [before_data], [after_data], [diff_data],
  [sql_statement], [oper_ip], [oper_location],
  [user_agent], [browser], [os], [device_type],
  [oper_time], [elapsed_time],
  [tenant_code], [company_code],
  [ext_field], [remark],
  [created_by], [created_at]
)
SELECT
  di.[id],
  'INSERT',
  N'takt_logistics_manufacturing_ec_source_detail',
  di.[id],
  '{}',
  (SELECT CAST(di.source_ec_id AS NVARCHAR) AS source_ec_id, di.legacy_part_no AS legacy_part_no FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
  '{}',
  N'INSERT EC Detail',
  '127.0.0.1', 'Server',
  'SQLCMD', 'Server', 'Windows', 'Server',
  GETDATE(), 0,
  '000', '2300',
  '{}', N'SAP_SYNC',
  '900001', GETDATE()
FROM #detail_inserted di;

-- oper_log
DECLARE @main_cnt INT = (SELECT COUNT(*) FROM #main_inserted);
DECLARE @detail_cnt INT = (SELECT COUNT(*) FROM #detail_inserted);
DECLARE @json_result NVARCHAR(MAX) = N'{"main_insert":' + ISNULL(CAST(@main_cnt AS NVARCHAR),'0') + N',"detail_insert":' + ISNULL(CAST(@detail_cnt AS NVARCHAR),'0') + N'}';

INSERT INTO [takt_statistics_logging_oper_log] (
  [id], [user_name], [oper_type], [oper_module],
  [oper_method], [request_method], [oper_url],
  [request_param], [json_result],
  [oper_ip], [oper_location],
  [user_agent], [browser], [os], [device_type],
  [oper_time], [elapsed_time],
  [oper_status], [error_msg],
  [tenant_code], [company_code],
  [created_by], [created_at]
)
SELECT
  (SELECT ISNULL(MAX([id]),0) FROM [takt_statistics_logging_oper_log]) + 1,
  N'SYSTEM_SAP_SYNC', N'SAP_SYNC', N'EC management',
  N'exec_sql_insert', 'SQL', N'/sync/sap/ec',
  CONCAT('batch_size=', ${batchSize}),
  @json_result,
  '127.0.0.1', 'Server',
  'SQLCMD', 'Server', 'Windows', 'Server',
  GETDATE(), 0,
  1, '',
  '000', '2300',
  '900001', GETDATE();

-- summary
SELECT 'MAIN_INS', @main_cnt;
SELECT 'DETAIL_INS', @detail_cnt;
`;

  const syncResult = await execSQL(syncSQL, 'EC main+detail sync', { filePrefix: 'ec_sync' });
  const counts = parseEcSummaryCounts(syncResult);

  console.log('');
  console.log('==========================================');
  console.log('Result:');
  console.log('  Main table: inserted ' + counts.mainInsert);
  console.log('  Detail table: inserted ' + counts.detailInsert);
  console.log('  Main total: ' + execSQLValue(
    'SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source]',
    { filePrefix: 'ec_val' }
  ));
  console.log('  Detail total: ' + execSQLValue(
    'SELECT COUNT(*) FROM [takt_logistics_manufacturing_ec_source_detail]',
    { filePrefix: 'ec_val' }
  ));
  console.log('==========================================');
})();