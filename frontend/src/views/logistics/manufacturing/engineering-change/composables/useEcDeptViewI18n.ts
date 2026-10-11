// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：useEcDeptViewI18n.ts
// 功能描述：执行部门表单 i18n：明细字段 entity.ecdetail.*，公共执行字段 entity.ecexec.*，课别字段 entity.{deptSlug}.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { COMMON_ENTITY_FIELD_I18N_KEYS } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n'

/** 与 TaktEcDetailI18nSeedData 对齐的明细/隔离字段 */
const DETAIL_OR_COMMON_FIELDS = new Set([
  'ecCode',
  'lineNumber',
  'ecModelCode',
  'ecRootMaterialCode',
  'ecRootMaterialDescription',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'discontinuedStatus',
  'ecOldMaterialCode',
  'ecNewMaterialCode',
  'ecOldPurchaseType',
  'ecOldRequiresInspection',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewPurchaseType',
  'ecNewRequiresInspection',
  'ecOldPartDisposition',
  'ecScope',
  'isObsolete',
  ...Object.keys(COMMON_ENTITY_FIELD_I18N_KEYS),
])

/** 与 TaktEcExecI18nSeedData 对齐的各部门公共执行字段 */
const EXEC_SHARED_FIELDS = new Set(['isImplemented', 'execContent', 'ecDetailId', 'deptCode', 'deptName'])

/** 生管课共享冗余字段（预定日期/预定批次） */
const SEIKAN_SHARED_FIELDS = new Set(['scheduledDate', 'scheduledBatch'])

/**
 * 执行部门表单字段 i18n
 * @param deptSlug 部门实体 slug（eckoubai / ecseikan / …，与 TaktEcXxxI18nSeedData 一致）
 */
export function useEcDeptViewI18n(deptSlug: string) {
  const detailI18n = useEntityFieldI18n('ecdetail')
  const execI18n = useEntityFieldI18n('ecexec')
  const deptI18n = useEntityFieldI18n(deptSlug)
  const seikanI18n = useEntityFieldI18n('ecseikan')

  /**
   * 按字段来源选择解析器
   * @param field DTO 属性 camelCase
   */
  function resolverFor(field: string) {
    if (EXEC_SHARED_FIELDS.has(field)) {
      return execI18n
    }
    if (SEIKAN_SHARED_FIELDS.has(field)) {
      return seikanI18n
    }
    if (DETAIL_OR_COMMON_FIELDS.has(field)) {
      return detailI18n
    }
    return deptI18n
  }

  /**
   * 业务字段标签
   * @param field DTO 属性 camelCase
   * @returns {string} 翻译文案
   */
  function label(field: string): string {
    // 执行行主键（ecSeikanId / ecKoubaiId …）无独立 entity 种子时用通用「主键ID」
    if (field !== 'ecDetailId' && /^ec[A-Za-z]+Id$/.test(field)) {
      return deptI18n.t('common.page.entity.id')
    }
    return resolverFor(field).label(field)
  }

  return {
    t: deptI18n.t,
    label,
    self: deptI18n.self,
  }
}
