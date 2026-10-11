// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/materials/plant/composables
// 文件名称：usePlantI18n.ts
// 功能描述：Takt工厂实体 代表租户下的独立工厂主档 与公司种子对称 组合 2：无关联工厂、有语言字段清单 + usePlantI18n（字段名映射一次，文案由 entity.plant.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PlantQuery } from '@/types/logistics/materials/plant.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPlantI18nSeedData 一致的实体 slug */
export const PLANT_ENTITY_SLUG = 'plant'

/** entity.plant._self 静态属性（导入组件 entity-i18n-key 等） */
export const PLANT_SELF_I18N_KEY = buildEntitySelfI18nKey(PLANT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PLANT_LIST_FIELDS = [
  'plantName1',
  'plantName2',
  'plantShortName',
  'codeAlias',
  'enterpriseNature',
  'industryAttribute',
  'enterpriseScale',
  'businessScope',
  'registrationAddress1',
  'registrationAddress2',
  'registrationRegion',
  'registrationProvince',
  'registrationCity',
  'businessRegion',
  'businessProvince',
  'businessCity',
  'businessAddress1',
  'businessAddress2',
  'plantAddress1',
  'plantAddress2',
  'plantPhone',
  'plantEmail',
  'plantFax',
  'plantWebsite',
  'unifiedSocialCreditCode',
  'taxRegistrationNumber',
  'legalRepresentative',
  'plantManager',
  'registeredCapital',
  'establishmentDate',
  'closingDate',
  'plantExistence',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'purchasingOrganization',
  'salesOrganization',
  'materialRequirementsPlanning',
  'distributionChannel',
  'intercompanyBillingProductGroup',
  'taxIndicator',
  'valuationArea',
  'plantVendorNumber',
  'plantCustomerNumber',
  'factoryCalendar',
  'relatedCompany',
  'plantStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PLANT_PLACEHOLDER = {
  tenantCode: 'optional',
  cultureCode: 'optional',
  plantName1: 'required',
  plantName2: 'optional',
  plantShortName: 'required',
  codeAlias: 'required',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  enterpriseScale: 'select',
  businessScope: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  registrationRegion: 'select',
  registrationProvince: 'select',
  registrationCity: 'select',
  businessRegion: 'select',
  businessProvince: 'select',
  businessCity: 'select',
  businessAddress1: 'optional',
  businessAddress2: 'optional',
  plantAddress1: 'optional',
  plantAddress2: 'optional',
  plantPhone: 'required',
  plantEmail: 'required',
  plantFax: 'required',
  plantWebsite: 'required',
  unifiedSocialCreditCode: 'required',
  taxRegistrationNumber: 'required',
  legalRepresentative: 'required',
  plantManager: 'required',
  registeredCapital: 'select',
  establishmentDate: 'select',
  closingDate: 'optional',
  plantExistence: 'select',
  bankCode: 'select',
  bankAccount: 'required',
  accountHolder: 'required',
  purchasingOrganization: 'select',
  salesOrganization: 'select',
  materialRequirementsPlanning: 'required',
  distributionChannel: 'required',
  intercompanyBillingProductGroup: 'required',
  taxIndicator: 'required',
  valuationArea: 'select',
  plantVendorNumber: 'required',
  plantCustomerNumber: 'required',
  factoryCalendar: 'required',
  relatedCompany: 'select',
  plantStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PlantField = keyof typeof PLANT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PLANT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantName1',
  'plantName2',
  'plantShortName',
  'codeAlias',
  'enterpriseNature',
  'industryAttribute',
  'enterpriseScale',
  'businessScope',
  'registrationAddress1',
  'registrationAddress2',
  'registrationRegion',
  'registrationProvince',
  'registrationCity',
  'businessRegion',
  'businessProvince',
  'businessCity',
  'businessAddress1',
  'businessAddress2',
  'plantAddress1',
  'plantAddress2',
  'plantPhone',
  'plantEmail',
  'plantFax',
  'plantWebsite',
  'unifiedSocialCreditCode',
  'taxRegistrationNumber',
  'legalRepresentative',
  'plantManager',
  'establishmentDateStart',
  'establishmentDateEnd',
  'closingDateStart',
  'closingDateEnd',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'purchasingOrganization',
  'salesOrganization',
  'materialRequirementsPlanning',
  'distributionChannel',
  'intercompanyBillingProductGroup',
  'taxIndicator',
  'valuationArea',
  'plantVendorNumber',
  'plantCustomerNumber',
  'factoryCalendar',
  'relatedCompany',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PlantQuery)[]

export type PlantQueryField =
  | (typeof PLANT_QUERY_STRING_FIELDS)[number]
  | 'registeredCapital' | 'plantExistence' | 'plantStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PLANT_QUERY_FIELDS: readonly PlantQueryField[] = [
  ...PLANT_QUERY_STRING_FIELDS,
  'registeredCapital',
  'plantExistence',
  'plantStatus',
]

/**
 * Takt工厂实体 代表租户下的独立工厂主档 与公司种子对称 组合 2：无关联工厂、有语言字段 i18n：index / plant-form 统一入口
 */
export function usePlantI18n() {
  const ef = useEntityFieldI18n(PLANT_ENTITY_SLUG)

  function ph(field: PlantField): string {
    return ef.placeholder(field, PLANT_PLACEHOLDER[field])
  }

  function queryPh(field: PlantQueryField, kind: EntityFieldPlaceholderKind): string {
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
