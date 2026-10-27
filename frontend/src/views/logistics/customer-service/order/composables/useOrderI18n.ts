// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/customer-service/order/composables
// 文件名称：useOrderI18n.ts
// 功能描述：服务订单实体字段清单 + useCustomerServiceOrderI18n（字段名映射一次，文案由 entity.customerserviceorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerServiceOrderQuery } from '@/types/logistics/customer-service/order.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerServiceOrderI18nSeedData 一致的实体 slug */
export const CUSTOMERSERVICEORDER_ENTITY_SLUG = 'customerserviceorder'

/** entity.customerserviceorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSERVICEORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSERVICEORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSERVICEORDER_LIST_FIELDS = [
  'serviceOrderCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'serviceRequestId',
  'serviceRequestCode',
  'orderDate',
  'orderType',
  'orderStatus',
  'totalAmount',
  'discountAmount',
  'taxAmount',
  'actualAmount',
  'currencyCode',
  'plannedStartDate',
  'plannedEndDate',
  'actualStartDate',
  'actualEndDate',
  'serviceEmployeeId',
  'serviceEmployeeName',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSERVICEORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  serviceOrderCode: 'required',
  clientId: 'required',
  clientCode: 'optional',
  clientName1: 'optional',
  serviceContractId: 'optional',
  serviceContractCode: 'optional',
  serviceRequestId: 'optional',
  serviceRequestCode: 'optional',
  orderDate: 'select',
  orderType: 'select',
  orderStatus: 'select',
  totalAmount: 'select',
  discountAmount: 'select',
  taxAmount: 'select',
  actualAmount: 'select',
  currencyCode: 'required',
  plannedStartDate: 'optional',
  plannedEndDate: 'optional',
  actualStartDate: 'optional',
  actualEndDate: 'optional',
  serviceEmployeeId: 'optional',
  serviceEmployeeName: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerServiceOrderField = keyof typeof CUSTOMERSERVICEORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSERVICEORDER_QUERY_STRING_FIELDS = [
  'cultureCode',
  'serviceOrderCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'serviceRequestId',
  'serviceRequestCode',
  'orderDateStart',
  'orderDateEnd',
  'currencyCode',
  'plannedStartDateStart',
  'plannedStartDateEnd',
  'plannedEndDateStart',
  'plannedEndDateEnd',
  'actualStartDateStart',
  'actualStartDateEnd',
  'actualEndDateStart',
  'actualEndDateEnd',
  'serviceEmployeeId',
  'serviceEmployeeName',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerServiceOrderQuery)[]

export type CustomerServiceOrderQueryField =
  | (typeof CUSTOMERSERVICEORDER_QUERY_STRING_FIELDS)[number]
  | 'orderType' | 'orderStatus' | 'totalAmount' | 'discountAmount' | 'taxAmount' | 'actualAmount'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSERVICEORDER_QUERY_FIELDS: readonly CustomerServiceOrderQueryField[] = [
  ...CUSTOMERSERVICEORDER_QUERY_STRING_FIELDS,
  'orderType',
  'orderStatus',
  'totalAmount',
  'discountAmount',
  'taxAmount',
  'actualAmount',
]

/**
 * 服务订单实体字段 i18n：index / order-form 统一入口
 */
export function useCustomerServiceOrderI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSERVICEORDER_ENTITY_SLUG)

  function ph(field: CustomerServiceOrderField): string {
    return ef.placeholder(field, CUSTOMERSERVICEORDER_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerServiceOrderQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
