// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：purchasePlan.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt采购计划实体（公司级；TaktMaterialRequirementsPlanning 运算产出，可转采购申请或订单）
 * 对应前端 TaktPurchasePlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PurchasePlan
 * @description 对应后端 TaktPurchasePlanDto
 */
export interface PurchasePlan extends ApprovalDtoBase {
  /**
   * PurchasePlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePlanId: string;

  /**
   * 采购计划编码（租户+公司+工厂内业务唯一）
   */
  purchasePlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源物料需求计划 名称（填充字段）
   */
  materialRequirementsPlanningName?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划名称（填充字段）
   */
  productionPlanName?: string;

  /**
   * 来源生产计划编码（冗余：按对应 Id 取主数据名称联动）
   */
  productionPlanCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroupCode?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转申请/订单金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 采购计划明细列表（主子表关系，一个计划可有多个明细行） （子表：TaktPurchasePlanItem）
   */
  items?: PurchasePlanItem[];

}


/**
 * PurchasePlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchasePlanQuery
 * @description 对应后端 TaktPurchasePlanQueryDto
 */
export interface PurchasePlanQuery extends TaktPagedQuery {
  /**
   * 租户编码
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 采购计划编码（租户+公司+工厂内业务唯一）
   */
  purchasePlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码（冗余：按对应 Id 取主数据名称联动）
   */
  productionPlanCode?: string;

  /**
   * 计划编制日期（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划编制日期（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 计划周期开始日期（范围查询-开始）
   */
  planPeriodStartStart?: string;

  /**
   * 计划周期开始日期（范围查询-结束）
   */
  planPeriodStartEnd?: string;

  /**
   * 计划周期结束日期（范围查询-开始）
   */
  planPeriodEndStart?: string;

  /**
   * 计划周期结束日期（范围查询-结束）
   */
  planPeriodEndEnd?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroupCode?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转申请/订单金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建PurchasePlan DTO
 * 对应前端 PurchasePlanCreate
 * @description 对应后端 TaktPurchasePlanCreateDto
 */
export interface PurchasePlanCreate {
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
   * 采购计划编码（租户+公司+工厂内业务唯一）
   */
  purchasePlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码（冗余：按对应 Id 取主数据名称联动）
   */
  productionPlanCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroupCode?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转申请/订单金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 采购计划明细列表（主子表关系，一个计划可有多个明细行）（子表，级联保存）
   */
  items?: PurchasePlanItemCreate[];

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
 * 更新PurchasePlan DTO
 * 继承 TaktPurchasePlanCreateDto，添加 PurchasePlanId 字段
 * 对应前端 PurchasePlanUpdate
 * @description 对应后端 TaktPurchasePlanUpdateDto
 */
export interface PurchasePlanUpdate extends PurchasePlanCreate {
  /**
   * PurchasePlanID（标识要更新的实体）
   */
  purchasePlanId: string;

  /**
   * 采购计划明细列表（主子表关系，一个计划可有多个明细行）（子表，级联保存）
   */
  items?: any;

}


/**
 * PurchasePlan 状态更新 DTO
 * 对应前端 PurchasePlanStatus
 * @description 对应后端 TaktPurchasePlanStatusDto
 */
export interface PurchasePlanStatus {
  /**
   * PurchasePlanID
   */
  purchasePlanId: string;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

}


/**
 * PurchasePlan 导入模板行 DTO
 * 对应前端 PurchasePlanTemplate
 * @description 对应后端 TaktPurchasePlanTemplateDto
 */
export interface PurchasePlanTemplate {
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
   * 采购计划编码（租户+公司+工厂内业务唯一）
   */
  purchasePlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码（冗余：按对应 Id 取主数据名称联动）
   */
  productionPlanCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroupCode?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转申请/订单金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 采购计划明细列表（主子表关系，一个计划可有多个明细行）（子表，级联保存）
   */
  items?: PurchasePlanItemCreate[];

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
 * PurchasePlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchasePlanImport
 * @description 对应后端 TaktPurchasePlanImportDto
 */
export interface PurchasePlanImport {
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
   * 采购计划编码（租户+公司+工厂内业务唯一）
   */
  purchasePlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码（冗余：按对应 Id 取主数据名称联动）
   */
  productionPlanCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroupCode?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转申请/订单金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 采购计划明细列表（主子表关系，一个计划可有多个明细行）（子表，级联保存）
   */
  items?: PurchasePlanItemCreate[];

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
 * PurchasePlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePlanExport
 * @description 对应后端 TaktPurchasePlanExportDto
 */
export interface PurchasePlanExport {
  /**
   * PurchasePlanID
   */
  purchasePlanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 采购计划编码（租户+公司+工厂内业务唯一）
   */
  purchasePlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源生产计划ID（产出追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 来源生产计划编码（冗余：按对应 Id 取主数据名称联动）
   */
  productionPlanCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroupCode?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转申请/订单数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转申请/订单金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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

