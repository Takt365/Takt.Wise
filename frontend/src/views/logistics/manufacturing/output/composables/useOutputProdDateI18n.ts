// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/composables
// 文件名称：useOutputProdDateI18n.ts
// 功能描述：制造产出模块生产日期锁定/可选范围静态文案；引用键 logistics.manufacturing.output.prod-date.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useI18n } from 'vue-i18n'
import { OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY } from './taktOutputProdDateEditLock'

/** 生产日期锁定提示 */
export const OUTPUT_PROD_DATE_LOCKED_I18N_KEY = 'logistics.manufacturing.output.prod-date.page.proddatelocked'

/** 生产日期超出可选范围提示 */
export const OUTPUT_PROD_DATE_OUT_OF_RANGE_I18N_KEY = 'logistics.manufacturing.output.prod-date.page.proddateoutofrange'

/**
 * 制造产出生产日期 i18n（组立/PCBA/换型等共用）
 */
export function useOutputProdDateI18n() {
  const { t: localeT } = useI18n()

  /** 生产日期锁定提示 */
  function prodDateLockedMessage(prodDate: string, cutoffDay: number = OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY): string {
    return localeT(OUTPUT_PROD_DATE_LOCKED_I18N_KEY, { prodDate, cutoffDay })
  }

  /** 生产日期超出可选范围提示 */
  function prodDateOutOfRangeMessage(cutoffDay: number = OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY): string {
    return localeT(OUTPUT_PROD_DATE_OUT_OF_RANGE_I18N_KEY, { cutoffDay })
  }

  return {
    prodDateLockedMessage,
    prodDateOutOfRangeMessage,
  }
}
