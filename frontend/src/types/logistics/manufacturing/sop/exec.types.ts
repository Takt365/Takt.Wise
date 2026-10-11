// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：exec.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 工位执行追溯实体
 * 对应前端 TaktSopExecDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopExec
 * @description 对应后端 TaktSopExecDto
 */
export interface SopExec extends CompanyDtoBase {
  /**
   * SopExecID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopExecId: string;

  /**
   * 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
   */
  productionOrderId?: string;

  /**
   * 生产工单 名称（填充字段）
   */
  productionOrderName?: string;

  /**
   * MES 工单号（冗余，便于追溯查询）
   */
  workOrderCode: string;

  /**
   * 产品序列号 SN
   */
  serialNumber?: string;

  /**
   * 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId: string;

  /**
   * 工序 名称（填充字段）
   */
  routingItemName?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType: number;

  /**
   * 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
   */
  workstationId: string;

  /**
   * 工位 名称（填充字段）
   */
  workstationName?: string;

  /**
   * 员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工 名称（填充字段）
   */
  employeeName?: string;

  /**
   * SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId: string;

  /**
   * SOP 主档 名称（填充字段）
   */
  sopName?: string;

  /**
   * SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
   */
  revisionId: string;

  /**
   * SOP 版本 名称（填充字段）
   */
  revisionName?: string;

  /**
   * 版本号快照
   */
  revision: string;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  selfCheckResult?: number;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus: number;

  /**
   * 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  currentStepId?: string;

  /**
   * 当前工步 名称（填充字段）
   */
  currentStepName?: string;

  /**
   * 工位 （主表：TaktSopWorkstation）
   */
  workstation?: SopWorkstation;

  /**
   * 工步执行明细 （子表：TaktSopExecStep）
   */
  steps?: SopExecStep[];

  /**
   * 扫码记录 （子表：TaktSopExecScan）
   */
  scans?: SopExecScan[];

  /**
   * 作业参数 （子表：TaktSopArgument）
   */
  arguments?: SopArgument[];

}


/**
 * SopExec 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopExecQuery
 * @description 对应后端 TaktSopExecQueryDto
 */
export interface SopExecQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
   */
  productionOrderId?: string;

  /**
   * MES 工单号（冗余，便于追溯查询）
   */
  workOrderCode?: string;

  /**
   * 产品序列号 SN
   */
  serialNumber?: string;

  /**
   * 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType?: number;

  /**
   * 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
   */
  workstationId?: string;

  /**
   * 员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
   */
  revisionId?: string;

  /**
   * 版本号快照
   */
  revision?: string;

  /**
   * 开始时间（范围查询-开始）
   */
  startedAtStart?: string;

  /**
   * 开始时间（范围查询-结束）
   */
  startedAtEnd?: string;

  /**
   * 结束时间（范围查询-开始）
   */
  endedAtStart?: string;

  /**
   * 结束时间（范围查询-结束）
   */
  endedAtEnd?: string;

  /**
   * 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  selfCheckResult?: number;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus?: number;

  /**
   * 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  currentStepId?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SopExec DTO
 * 对应前端 SopExecCreate
 * @description 对应后端 TaktSopExecCreateDto
 */
export interface SopExecCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
   */
  productionOrderId?: string;

  /**
   * MES 工单号（冗余，便于追溯查询）
   */
  workOrderCode: string;

  /**
   * 产品序列号 SN
   */
  serialNumber?: string;

  /**
   * 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType: number;

  /**
   * 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
   */
  workstationId: string;

  /**
   * 员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId: string;

  /**
   * SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
   */
  revisionId: string;

  /**
   * 版本号快照
   */
  revision: string;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  selfCheckResult?: number;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus: number;

  /**
   * 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  currentStepId?: string;

  /**
   * 工步执行明细（子表，级联保存）
   */
  steps?: SopExecStepCreate[];

  /**
   * 扫码记录（子表，级联保存）
   */
  scans?: SopExecScanCreate[];

  /**
   * 作业参数（子表，级联保存）
   */
  arguments?: SopArgumentCreate[];

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
 * 更新SopExec DTO
 * 继承 TaktSopExecCreateDto，添加 SopExecId 字段
 * 对应前端 SopExecUpdate
 * @description 对应后端 TaktSopExecUpdateDto
 */
export interface SopExecUpdate extends SopExecCreate {
  /**
   * SopExecID（标识要更新的实体）
   */
  sopExecId: string;

  /**
   * 工步执行明细（子表，级联保存）
   */
  steps?: any;

  /**
   * 扫码记录（子表，级联保存）
   */
  scans?: any;

  /**
   * 作业参数（子表，级联保存）
   */
  arguments?: any;

}


/**
 * SopExec 状态更新 DTO
 * 对应前端 SopExecStatus
 * @description 对应后端 TaktSopExecStatusDto
 */
export interface SopExecStatus {
  /**
   * SopExecID
   */
  sopExecId: string;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus: number;

}


/**
 * SopExec 导入模板行 DTO
 * 对应前端 SopExecTemplate
 * @description 对应后端 TaktSopExecTemplateDto
 */
export interface SopExecTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
   */
  productionOrderId?: string;

  /**
   * MES 工单号（冗余，便于追溯查询）
   */
  workOrderCode?: string;

  /**
   * 产品序列号 SN
   */
  serialNumber?: string;

  /**
   * 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType?: number;

  /**
   * 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
   */
  workstationId?: string;

  /**
   * 员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
   */
  revisionId?: string;

  /**
   * 版本号快照
   */
  revision?: string;

  /**
   * 开始时间
   */
  startedAt?: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  selfCheckResult?: number;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus?: number;

  /**
   * 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  currentStepId?: string;

  /**
   * 工步执行明细（子表，级联保存）
   */
  steps?: SopExecStepCreate[];

  /**
   * 扫码记录（子表，级联保存）
   */
  scans?: SopExecScanCreate[];

  /**
   * 作业参数（子表，级联保存）
   */
  arguments?: SopArgumentCreate[];

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
 * SopExec 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopExecImport
 * @description 对应后端 TaktSopExecImportDto
 */
export interface SopExecImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
   */
  productionOrderId?: string;

  /**
   * MES 工单号（冗余，便于追溯查询）
   */
  workOrderCode?: string;

  /**
   * 产品序列号 SN
   */
  serialNumber?: string;

  /**
   * 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType?: number;

  /**
   * 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
   */
  workstationId?: string;

  /**
   * 员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
   */
  revisionId?: string;

  /**
   * 版本号快照
   */
  revision?: string;

  /**
   * 开始时间
   */
  startedAt?: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  selfCheckResult?: number;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus?: number;

  /**
   * 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  currentStepId?: string;

  /**
   * 工步执行明细（子表，级联保存）
   */
  steps?: SopExecStepCreate[];

  /**
   * 扫码记录（子表，级联保存）
   */
  scans?: SopExecScanCreate[];

  /**
   * 作业参数（子表，级联保存）
   */
  arguments?: SopArgumentCreate[];

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
 * SopExec 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopExecExport
 * @description 对应后端 TaktSopExecExportDto
 */
export interface SopExecExport {
  /**
   * SopExecID
   */
  sopExecId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
   */
  productionOrderId?: string;

  /**
   * MES 工单号（冗余，便于追溯查询）
   */
  workOrderCode: string;

  /**
   * 产品序列号 SN
   */
  serialNumber?: string;

  /**
   * 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId: string;

  /**
   * 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
   */
  processSegmentType: number;

  /**
   * 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
   */
  workstationId: string;

  /**
   * 员工 ID（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId: string;

  /**
   * SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
   */
  revisionId: string;

  /**
   * 版本号快照
   */
  revision: string;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  endedAt?: string;

  /**
   * 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
   */
  selfCheckResult?: number;

  /**
   * 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
   */
  execStatus: number;

  /**
   * 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  currentStepId?: string;

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

