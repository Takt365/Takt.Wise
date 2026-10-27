// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：useEcDeptViewI18n.ts
// 功能描述：执行部门列表/表单字段标签，一律走该部门 entity.{slug}.* 种子
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n'

/**
 * 执行部门表单字段 i18n（列标题与表单标签一律走该部门 entity.{slug}.* 种子）
 * @param deptSlug 部门实体 slug（ecpur / ecpc / …，与 TaktEcXxxI18nSeedData 一致）
 */
export function useEcDeptViewI18n(deptSlug: string) {
  const deptI18n = useEntityFieldI18n(deptSlug)

  /**
   * 业务字段标签
   * @param field DTO 属性 camelCase
   * @returns {string} 翻译文案
   */
  function label(field: string): string {
    // 执行行主键（ecPcId / ecPurId …）无独立 entity 种子时用通用「主键ID」
    if (field !== 'ecDetailId' && /^ec[A-Za-z]+Id$/.test(field)) {
      return deptI18n.t('common.page.entity.id')
    }
    return deptI18n.label(field)
  }

  return {
    t: deptI18n.t,
    label,
    self: deptI18n.self,
  }
}
