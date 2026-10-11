// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：productionEquipment.types.ts
// 创建时间：2026-07-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 生产设备主数据（排程资源；粗能力 StdEquipHourlyCapacity=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate；多穴=(60÷StdMinutesPerCycle)×CavityCount×AvailabilityRate）
 * 对应前端 TaktProductionEquipmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionEquipment
 * @description 对应后端 TaktProductionEquipmentDto
 */
export interface ProductionEquipment extends CompanyDtoBase {

  /**
   * 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
   */
  equipCategory?: number;

  /**
   * 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
   */
  prodEquipCode?: string;

  /**
   * 生产设备名称（列表展示名）
   */
  prodEquipName?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 设备品牌（铭牌 Brand）
   */
  equipBrand?: string;

  /**
   * 机型名称（铭牌 Machine Type，如 SP18P-L）
   */
  machineType?: string;

  /**
   * 型号（铭牌 Model No，如 NM-EJP1A）
   */
  modelCode?: string;

  /**
   * 序列号（铭牌 Serial No，如 1P8V0336）
   */
  serialCode?: string;

  /**
   * 出厂日期（Manufacturing Date）
   */
  manufacturingDate?: string;

  /**
   * 设备规格
   */
  equipSpecification?: string;

  /**
   * 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
   */
  stdCycleTimeSeconds?: number;

  /**
   * 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
   */
  stdMinutesPerUnit?: number;

  /**
   * 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
   */
  stdMinutesPerCycle?: number;

  /**
   * 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
   */
  theoreticalSpm?: number;

  /**
   * 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
   */
  theoreticalCycleTimeSeconds?: number;

  /**
   * 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
   */
  stdEquipHourlyCapacity?: number;

  /**
   * 设备时间稼动率（AvailabilityRate，0–1）
   */
  availabilityRate?: number;

  /**
   * 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
   */
  performanceRate?: number;

  /**
   * 准备时间（分钟；通用调试）
   */
  setupMinutes?: number;

  /**
   * 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
   */
  moldChangeMinutes?: number;

  /**
   * 换料时间（分钟；注塑料筒清洗等）
   */
  materialChangeMinutes?: number;

  /**
   * 平均无故障时间 MTBF（小时）
   */
  mtbfHours?: number;

  /**
   * 平均修复时间 MTTR（小时）
   */
  mttrHours?: number;

  /**
   * 重复定位精度（mm；滑块/模板 ±0.01~0.05）
   */
  repeatabilityAccuracy?: number;

  /**
   * 闭合高度精度（mm；冲压）
   */
  shutHeightAccuracy?: number;

  /**
   * 注射精度（%；注塑计量 ±0.5%）
   */
  injectionAccuracy?: number;

  /**
   * 温控精度（℃；注塑 ±1℃）
   */
  temperatureControlAccuracy?: number;

  /**
   * 压力控制精度（%；冲压/注塑 ±1–2%）
   */
  pressureControlAccuracy?: number;

  /**
   * 工艺能力 Cpk（关键尺寸）
   */
  processCapabilityCpk?: number;

  /**
   * 最大成型公差（mm）
   */
  maxDimensionalTolerance?: number;

  /**
   * 最大模具尺寸（L×W×H）
   */
  maxMoldDimension?: string;

  /**
   * 最小模具尺寸（L×W×H）
   */
  minMoldDimension?: string;

  /**
   * 模具重量上限（ton）
   */
  maxMoldWeightTon?: number;

  /**
   * 模具厚度范围（冲压闭合高度/注塑模板间距）
   */
  moldHeightRange?: string;

  /**
   * 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
   */
  ejectionType?: number;

  /**
   * 顶出行程（mm）
   */
  ejectionStrokeMm?: number;

  /**
   * 工位数/穴数（CavityCount；一出几，产能折算关键）
   */
  cavityCount?: number;

  /**
   * 快速换模（字典 sys_yes_no）
   */
  quickMoldChange?: number;

  /**
   * 模具编码（模具主数据关联）
   */
  moldCode?: string;

  /**
   * 额定吨位（ton；冲压）
   */
  ratedTonnage?: number;

  /**
   * 锁模力（kN；注塑）
   */
  clampingForceKn?: number;

  /**
   * 最大行程（mm）
   */
  maxStrokeMm?: number;

  /**
   * 开模行程（mm；注塑）
   */
  openStrokeMm?: number;

  /**
   * 模板尺寸（mm）
   */
  platenSize?: string;

  /**
   * 使用电压（V）
   */
  ratedVoltage?: number;

  /**
   * 额定功率（kW）
   */
  ratedPowerKw?: number;

  /**
   * 耗气量（L/min）
   */
  airConsumptionLpm?: number;

  /**
   * 冷却水流量（L/min）
   */
  coolingWaterFlowLpm?: number;

  /**
   * 操作人员数（标准配人）
   */
  operatorCount?: number;

  /**
   * 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
   */
  isCriticalResource?: number;

  /**
   * 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
   */
  parallelCapacity?: number;

  /**
   * 是否允许插单（字典 sys_yes_no）
   */
  allowRushOrder?: number;

  /**
   * 开机预热时间（分钟；注塑螺杆预热）
   */
  warmupMinutes?: number;

  /**
   * 工作温度范围（℃）
   */
  operatingTempRange?: string;

  /**
   * 湿度范围（%RH）
   */
  operatingHumidityRange?: string;

  /**
   * 噪音水平（dB）
   */
  noiseLevelDb?: number;

  /**
   * 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
   */
  equipmentRunStatus?: number;

  /**
   * 保养周期（小时）
   */
  maintenanceIntervalHours?: number;

  /**
   * 累计运行时间（小时；寿命/PM）
   */
  cumulativeRunHours?: number;

  /**
   * 车间集成接口（SMEMA/PLC 等）
   */
  interfaceType?: string;

  /**
   * 投产日期（Commissioning Date；设备正式投产日期）
   */
  commissioningDate?: string;

  /**
   * 停产日期（Decommissioning Date；设备停止生产日期）
   */
  decommissioningDate?: string;

  /**
   * 报废日期（资产注销 / Scrap Date）
   */
  scrapDate?: string;

  /**
   * 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
   */
  storageLocation?: string;

  /**
   * 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipAdministrator?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  prodEquipStatus?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * ProductionEquipment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionEquipmentExport
 * @description 对应后端 TaktProductionEquipmentExportDto
 */
export interface ProductionEquipmentExport {
  /**
   * ProductionEquipmentID
   */
  productionEquipmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 设备类别（字典 logistics_maintenance_equip_category；Press/Injection/DieCasting/SMT/Assembly 等）
   */
  equipCategory: number;

  /**
   * 生产设备编码（同一工厂+存放位置内不可重复；EquipCode / 资产MES编码）
   */
  prodEquipCode: string;

  /**
   * 生产设备名称（列表展示名）
   */
  prodEquipName: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 设备品牌（铭牌 Brand）
   */
  equipBrand?: string;

  /**
   * 机型名称（铭牌 Machine Type，如 SP18P-L）
   */
  machineType: string;

  /**
   * 型号（铭牌 Model No，如 NM-EJP1A）
   */
  modelCode?: string;

  /**
   * 序列号（铭牌 Serial No，如 1P8V0336）
   */
  serialCode?: string;

  /**
   * 出厂日期（Manufacturing Date）
   */
  manufacturingDate?: string;

  /**
   * 设备规格
   */
  equipSpecification?: string;

  /**
   * 理论周期时间（秒/模次；StdCycleTime，SPM 倒数）
   */
  stdCycleTimeSeconds: number;

  /**
   * 标准分钟数/件（StdMinutesPerUnit；StdCycleTime÷60×件数/模次）
   */
  stdMinutesPerUnit: number;

  /**
   * 标准分钟/周期（StdMinutesPerCycle；多穴产能折算用）
   */
  stdMinutesPerCycle: number;

  /**
   * 理论模次/小时（TheoreticalSPM，冲压 Strokes Per Minute×60）
   */
  theoreticalSpm: number;

  /**
   * 理论射出/成型周期（秒；注塑注射+保压+冷却+开合模）
   */
  theoreticalCycleTimeSeconds: number;

  /**
   * 设备标准小时产能（件/小时；=(60÷StdMinutesPerUnit)×AvailabilityRate×PerformanceRate）
   */
  stdEquipHourlyCapacity: number;

  /**
   * 设备时间稼动率（AvailabilityRate，0–1）
   */
  availabilityRate: number;

  /**
   * 性能稼动率（PerformanceRate，0–1；实际可达产能乘数）
   */
  performanceRate: number;

  /**
   * 准备时间（分钟；通用调试）
   */
  setupMinutes: number;

  /**
   * 换模时间（分钟；SMED，冲压 10–30min，注塑 15–60min）
   */
  moldChangeMinutes: number;

  /**
   * 换料时间（分钟；注塑料筒清洗等）
   */
  materialChangeMinutes: number;

  /**
   * 平均无故障时间 MTBF（小时）
   */
  mtbfHours: number;

  /**
   * 平均修复时间 MTTR（小时）
   */
  mttrHours: number;

  /**
   * 重复定位精度（mm；滑块/模板 ±0.01~0.05）
   */
  repeatabilityAccuracy?: number;

  /**
   * 闭合高度精度（mm；冲压）
   */
  shutHeightAccuracy?: number;

  /**
   * 注射精度（%；注塑计量 ±0.5%）
   */
  injectionAccuracy?: number;

  /**
   * 温控精度（℃；注塑 ±1℃）
   */
  temperatureControlAccuracy?: number;

  /**
   * 压力控制精度（%；冲压/注塑 ±1–2%）
   */
  pressureControlAccuracy?: number;

  /**
   * 工艺能力 Cpk（关键尺寸）
   */
  processCapabilityCpk?: number;

  /**
   * 最大成型公差（mm）
   */
  maxDimensionalTolerance?: number;

  /**
   * 最大模具尺寸（L×W×H）
   */
  maxMoldDimension?: string;

  /**
   * 最小模具尺寸（L×W×H）
   */
  minMoldDimension?: string;

  /**
   * 模具重量上限（ton）
   */
  maxMoldWeightTon?: number;

  /**
   * 模具厚度范围（冲压闭合高度/注塑模板间距）
   */
  moldHeightRange?: string;

  /**
   * 顶出方式（字典 logistics_ejection_type；0=机械 1=液压 2=气动）
   */
  ejectionType?: number;

  /**
   * 顶出行程（mm）
   */
  ejectionStrokeMm?: number;

  /**
   * 工位数/穴数（CavityCount；一出几，产能折算关键）
   */
  cavityCount: number;

  /**
   * 快速换模（字典 sys_yes_no）
   */
  quickMoldChange: number;

  /**
   * 模具编码（模具主数据关联）
   */
  moldCode?: string;

  /**
   * 额定吨位（ton；冲压）
   */
  ratedTonnage?: number;

  /**
   * 锁模力（kN；注塑）
   */
  clampingForceKn?: number;

  /**
   * 最大行程（mm）
   */
  maxStrokeMm?: number;

  /**
   * 开模行程（mm；注塑）
   */
  openStrokeMm?: number;

  /**
   * 模板尺寸（mm）
   */
  platenSize?: string;

  /**
   * 使用电压（V）
   */
  ratedVoltage?: number;

  /**
   * 额定功率（kW）
   */
  ratedPowerKw?: number;

  /**
   * 耗气量（L/min）
   */
  airConsumptionLpm?: number;

  /**
   * 冷却水流量（L/min）
   */
  coolingWaterFlowLpm?: number;

  /**
   * 操作人员数（标准配人）
   */
  operatorCount: number;

  /**
   * 是否关键设备（字典 sys_yes_no；RCCP/粗能力）
   */
  isCriticalResource: number;

  /**
   * 并行限制（MaxParallelJobs；可同时加工任务数，APS 有限产能）
   */
  parallelCapacity: number;

  /**
   * 是否允许插单（字典 sys_yes_no）
   */
  allowRushOrder: number;

  /**
   * 开机预热时间（分钟；注塑螺杆预热）
   */
  warmupMinutes: number;

  /**
   * 工作温度范围（℃）
   */
  operatingTempRange?: string;

  /**
   * 湿度范围（%RH）
   */
  operatingHumidityRange?: string;

  /**
   * 噪音水平（dB）
   */
  noiseLevelDb?: number;

  /**
   * 设备运行状态（字典 logistics_equipment_run_status；0=RUN 1=IDLE 2=DOWN 3=SETUP）
   */
  equipmentRunStatus: number;

  /**
   * 保养周期（小时）
   */
  maintenanceIntervalHours: number;

  /**
   * 累计运行时间（小时；寿命/PM）
   */
  cumulativeRunHours: number;

  /**
   * 车间集成接口（SMEMA/PLC 等）
   */
  interfaceType?: string;

  /**
   * 投产日期（Commissioning Date；设备正式投产日期）
   */
  commissioningDate?: string;

  /**
   * 停产日期（Decommissioning Date；设备停止生产日期）
   */
  decommissioningDate?: string;

  /**
   * 报废日期（资产注销 / Scrap Date）
   */
  scrapDate?: string;

  /**
   * 存放位置（车间/线体/工位等物理位置；与设备编码组合唯一）
   */
  storageLocation: string;

  /**
   * 设备管理员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipAdministrator?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  prodEquipStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

