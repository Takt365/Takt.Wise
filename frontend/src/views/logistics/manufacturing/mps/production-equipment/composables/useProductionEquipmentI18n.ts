// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mps/production-equipment/composables
// 文件名称：useProductionEquipmentI18n.ts
// 功能描述：生产设备主数据字段清单 + useProductionEquipmentI18n（字段名映射一次，文案由 entity.productionequipment.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProductionEquipmentQuery } from '@/types/logistics/manufacturing/mps/productionEquipment.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktProductionEquipmentI18nSeedData 一致的实体 slug */
export const PRODUCTIONEQUIPMENT_ENTITY_SLUG = 'productionequipment'

/** entity.productionequipment._self 静态属性（导入组件 entity-i18n-key 等） */
export const PRODUCTIONEQUIPMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(PRODUCTIONEQUIPMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PRODUCTIONEQUIPMENT_LIST_FIELDS = [
  'plantCode',
  'equipCategory',
  'prodEquipCode',
  'prodEquipName',
  'manufacturer',
  'equipBrand',
  'machineType',
  'modelCode',
  'serialCode',
  'manufacturingDate',
  'equipSpecification',
  'stdCycleTimeSeconds',
  'stdMinutesPerUnit',
  'stdMinutesPerCycle',
  'theoreticalSpm',
  'theoreticalCycleTimeSeconds',
  'stdEquipHourlyCapacity',
  'availabilityRate',
  'performanceRate',
  'setupMinutes',
  'moldChangeMinutes',
  'materialChangeMinutes',
  'mtbfHours',
  'mttrHours',
  'repeatabilityAccuracy',
  'shutHeightAccuracy',
  'injectionAccuracy',
  'temperatureControlAccuracy',
  'pressureControlAccuracy',
  'processCapabilityCpk',
  'maxDimensionalTolerance',
  'maxMoldDimension',
  'minMoldDimension',
  'maxMoldWeightTon',
  'moldHeightRange',
  'ejectionType',
  'ejectionStrokeMm',
  'cavityCount',
  'quickMoldChange',
  'moldCode',
  'ratedTonnage',
  'clampingForceKn',
  'maxStrokeMm',
  'openStrokeMm',
  'platenSize',
  'ratedVoltage',
  'ratedPowerKw',
  'airConsumptionLpm',
  'coolingWaterFlowLpm',
  'operatorCount',
  'isCriticalResource',
  'parallelCapacity',
  'allowRushOrder',
  'warmupMinutes',
  'operatingTempRange',
  'operatingHumidityRange',
  'noiseLevelDb',
  'equipmentRunStatus',
  'maintenanceIntervalHours',
  'cumulativeRunHours',
  'interfaceType',
  'commissioningDate',
  'decommissioningDate',
  'scrapDate',
  'storageLocation',
  'equipAdministrator',
  'prodEquipStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PRODUCTIONEQUIPMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  equipCategory: 'select',
  prodEquipCode: 'required',
  prodEquipName: 'required',
  manufacturer: 'optional',
  equipBrand: 'optional',
  machineType: 'required',
  modelCode: 'optional',
  serialCode: 'optional',
  manufacturingDate: 'optional',
  equipSpecification: 'optional',
  stdCycleTimeSeconds: 'select',
  stdMinutesPerUnit: 'select',
  stdMinutesPerCycle: 'select',
  theoreticalSpm: 'select',
  theoreticalCycleTimeSeconds: 'select',
  stdEquipHourlyCapacity: 'select',
  availabilityRate: 'select',
  performanceRate: 'select',
  setupMinutes: 'select',
  moldChangeMinutes: 'select',
  materialChangeMinutes: 'select',
  mtbfHours: 'select',
  mttrHours: 'select',
  repeatabilityAccuracy: 'optional',
  shutHeightAccuracy: 'optional',
  injectionAccuracy: 'optional',
  temperatureControlAccuracy: 'optional',
  pressureControlAccuracy: 'optional',
  processCapabilityCpk: 'optional',
  maxDimensionalTolerance: 'optional',
  maxMoldDimension: 'optional',
  minMoldDimension: 'optional',
  maxMoldWeightTon: 'optional',
  moldHeightRange: 'optional',
  ejectionType: 'optional',
  ejectionStrokeMm: 'optional',
  cavityCount: 'select',
  quickMoldChange: 'select',
  moldCode: 'optional',
  ratedTonnage: 'optional',
  clampingForceKn: 'optional',
  maxStrokeMm: 'optional',
  openStrokeMm: 'optional',
  platenSize: 'optional',
  ratedVoltage: 'optional',
  ratedPowerKw: 'optional',
  airConsumptionLpm: 'optional',
  coolingWaterFlowLpm: 'optional',
  operatorCount: 'select',
  isCriticalResource: 'select',
  parallelCapacity: 'select',
  allowRushOrder: 'select',
  warmupMinutes: 'select',
  operatingTempRange: 'optional',
  operatingHumidityRange: 'optional',
  noiseLevelDb: 'optional',
  equipmentRunStatus: 'select',
  maintenanceIntervalHours: 'select',
  cumulativeRunHours: 'select',
  interfaceType: 'optional',
  commissioningDate: 'optional',
  decommissioningDate: 'optional',
  scrapDate: 'optional',
  storageLocation: 'required',
  equipAdministrator: 'optional',
  prodEquipStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProductionEquipmentField = keyof typeof PRODUCTIONEQUIPMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PRODUCTIONEQUIPMENT_QUERY_STRING_FIELDS = [
  'prodEquipCode',
  'prodEquipName',
  'manufacturer',
  'equipBrand',
  'machineType',
  'modelCode',
  'serialCode',
  'manufacturingDateStart',
  'manufacturingDateEnd',
  'equipSpecification',
  'maxMoldDimension',
  'minMoldDimension',
  'moldHeightRange',
  'moldCode',
  'platenSize',
  'operatingTempRange',
  'operatingHumidityRange',
  'interfaceType',
  'commissioningDateStart',
  'commissioningDateEnd',
  'decommissioningDateStart',
  'decommissioningDateEnd',
  'scrapDateStart',
  'scrapDateEnd',
  'storageLocation',
  'equipAdministrator',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProductionEquipmentQuery)[]

export type ProductionEquipmentQueryField =
  | (typeof PRODUCTIONEQUIPMENT_QUERY_STRING_FIELDS)[number]
  | 'equipCategory' | 'stdCycleTimeSeconds' | 'stdMinutesPerUnit' | 'stdMinutesPerCycle' | 'theoreticalSpm' | 'theoreticalCycleTimeSeconds' | 'stdEquipHourlyCapacity' | 'availabilityRate' | 'performanceRate' | 'setupMinutes' | 'moldChangeMinutes' | 'materialChangeMinutes' | 'mtbfHours' | 'mttrHours' | 'repeatabilityAccuracy' | 'shutHeightAccuracy' | 'injectionAccuracy' | 'temperatureControlAccuracy' | 'pressureControlAccuracy' | 'processCapabilityCpk' | 'maxDimensionalTolerance' | 'maxMoldWeightTon' | 'ejectionType' | 'ejectionStrokeMm' | 'cavityCount' | 'quickMoldChange' | 'ratedTonnage' | 'clampingForceKn' | 'maxStrokeMm' | 'openStrokeMm' | 'ratedVoltage' | 'ratedPowerKw' | 'airConsumptionLpm' | 'coolingWaterFlowLpm' | 'operatorCount' | 'isCriticalResource' | 'parallelCapacity' | 'allowRushOrder' | 'warmupMinutes' | 'noiseLevelDb' | 'equipmentRunStatus' | 'maintenanceIntervalHours' | 'cumulativeRunHours' | 'prodEquipStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PRODUCTIONEQUIPMENT_QUERY_FIELDS: readonly ProductionEquipmentQueryField[] = [
  ...PRODUCTIONEQUIPMENT_QUERY_STRING_FIELDS,
  'equipCategory',
  'stdCycleTimeSeconds',
  'stdMinutesPerUnit',
  'stdMinutesPerCycle',
  'theoreticalSpm',
  'theoreticalCycleTimeSeconds',
  'stdEquipHourlyCapacity',
  'availabilityRate',
  'performanceRate',
  'setupMinutes',
  'moldChangeMinutes',
  'materialChangeMinutes',
  'mtbfHours',
  'mttrHours',
  'repeatabilityAccuracy',
  'shutHeightAccuracy',
  'injectionAccuracy',
  'temperatureControlAccuracy',
  'pressureControlAccuracy',
  'processCapabilityCpk',
  'maxDimensionalTolerance',
  'maxMoldWeightTon',
  'ejectionType',
  'ejectionStrokeMm',
  'cavityCount',
  'quickMoldChange',
  'ratedTonnage',
  'clampingForceKn',
  'maxStrokeMm',
  'openStrokeMm',
  'ratedVoltage',
  'ratedPowerKw',
  'airConsumptionLpm',
  'coolingWaterFlowLpm',
  'operatorCount',
  'isCriticalResource',
  'parallelCapacity',
  'allowRushOrder',
  'warmupMinutes',
  'noiseLevelDb',
  'equipmentRunStatus',
  'maintenanceIntervalHours',
  'cumulativeRunHours',
  'prodEquipStatus',
]

/**
 * 生产设备主数据字段 i18n：index / production-equipment-form 统一入口
 */
export function useProductionEquipmentI18n() {
  const ef = useEntityFieldI18n(PRODUCTIONEQUIPMENT_ENTITY_SLUG)

  function ph(field: ProductionEquipmentField): string {
    return ef.placeholder(field, PRODUCTIONEQUIPMENT_PLACEHOLDER[field])
  }

  function queryPh(field: ProductionEquipmentQueryField, kind: EntityFieldPlaceholderKind): string {
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
