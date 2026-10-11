// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：taktAssyOutputProdDateEditLock.ts
// 功能描述：组立日报生产日期锁定（转发至 output 共用 composable，保留旧导出名兼容）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export {
  OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY as ASSY_OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY,
  parseOutputProdDateYmd as parseAssyOutputProdDateYmd,
  resolveOutputProdDateEditDeadline as resolveAssyOutputProdDateEditDeadline,
  isOutputProdDateLocked as isAssyOutputProdDateLocked,
  resolveOutputSelectableProdDateRange as resolveAssyOutputSelectableProdDateRange,
  isOutputProdDateSelectable as isAssyOutputProdDateSelectable,
  getOutputProdDateYmdFromRecord as getAssyOutputProdDateYmdFromRecord,
  outputProdDatePickerDisabledDate as assyOutputProdDatePickerDisabledDate,
  resolveDefaultOutputProdDateYmd,
  formatOutputProdDateYmd,
} from '../../composables/taktOutputProdDateEditLock'
