// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：workOrderMaterial.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 维护工单领料明细实体（主子表：挂载于维护工单）
 * 对应前端 TaktMaintenanceWorkOrderMaterialDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaintenanceWorkOrderMaterial
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialDto
 */
export interface MaintenanceWorkOrderMaterial extends CompanyDtoBase {
  /**
   * MaintenanceWorkOrderMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceWorkOrderMaterialId: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单名称（填充字段）
   */
  maintenanceWorkOrderName?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  materialId: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 需求数量
   */
  requiredQuantity: number;

  /**
   * 已领数量
   */
  issuedQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 金额
   */
  amount: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位
   */
  storageLocation?: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus: number;

  /**
   * 领料时间
   */
  issueTime?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 维护工单（主表） （主表：TaktMaintenanceWorkOrder）
   */
  maintenanceWorkOrder?: MaintenanceWorkOrder;

  /**
   * 物料（工厂物料主数据） （主表：TaktMaterialPlant）
   */
  materialPlant?: MaterialPlant;

}


/**
 * MaintenanceWorkOrderMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceWorkOrderMaterialQuery
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialQueryDto
 */
export interface MaintenanceWorkOrderMaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  materialId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 需求数量
   */
  requiredQuantity?: number;

  /**
   * 已领数量
   */
  issuedQuantity?: number;

  /**
   * 单位
   */
  materialUnit?: string;

  /**
   * 单价
   */
  unitPrice?: number;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位
   */
  storageLocation?: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus?: number;

  /**
   * 领料时间（范围查询-开始）
   */
  issueTimeStart?: string;

  /**
   * 领料时间（范围查询-结束）
   */
  issueTimeEnd?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建MaintenanceWorkOrderMaterial DTO
 * 对应前端 MaintenanceWorkOrderMaterialCreate
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialCreateDto
 */
export interface MaintenanceWorkOrderMaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  materialId: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 需求数量
   */
  requiredQuantity: number;

  /**
   * 已领数量
   */
  issuedQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 金额
   */
  amount: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位
   */
  storageLocation?: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus: number;

  /**
   * 领料时间
   */
  issueTime?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新MaintenanceWorkOrderMaterial DTO
 * 继承 TaktMaintenanceWorkOrderMaterialCreateDto，添加 MaintenanceWorkOrderMaterialId 字段
 * 对应前端 MaintenanceWorkOrderMaterialUpdate
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialUpdateDto
 */
export interface MaintenanceWorkOrderMaterialUpdate extends MaintenanceWorkOrderMaterialCreate {
  /**
   * MaintenanceWorkOrderMaterialID（标识要更新的实体）
   */
  maintenanceWorkOrderMaterialId: string;

}


/**
 * MaintenanceWorkOrderMaterial 状态更新 DTO
 * 对应前端 MaintenanceWorkOrderMaterialStatus
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialStatusDto
 */
export interface MaintenanceWorkOrderMaterialStatus {
  /**
   * MaintenanceWorkOrderMaterialID
   */
  maintenanceWorkOrderMaterialId: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus: number;

}


/**
 * MaintenanceWorkOrderMaterial 作废/撤销作废 DTO
 * 对应前端 MaintenanceWorkOrderMaterialObsolete
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialObsoleteDto
 */
export interface MaintenanceWorkOrderMaterialObsolete {
  /**
   * MaintenanceWorkOrderMaterialID
   */
  maintenanceWorkOrderMaterialId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * MaintenanceWorkOrderMaterial 导入模板行 DTO
 * 对应前端 MaintenanceWorkOrderMaterialTemplate
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialTemplateDto
 */
export interface MaintenanceWorkOrderMaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  materialId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 需求数量
   */
  requiredQuantity?: number;

  /**
   * 已领数量
   */
  issuedQuantity?: number;

  /**
   * 单位
   */
  materialUnit?: string;

  /**
   * 单价
   */
  unitPrice?: number;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位
   */
  storageLocation?: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus?: number;

  /**
   * 领料时间
   */
  issueTime?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * MaintenanceWorkOrderMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceWorkOrderMaterialImport
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialImportDto
 */
export interface MaintenanceWorkOrderMaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  materialId?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 需求数量
   */
  requiredQuantity?: number;

  /**
   * 已领数量
   */
  issuedQuantity?: number;

  /**
   * 单位
   */
  materialUnit?: string;

  /**
   * 单价
   */
  unitPrice?: number;

  /**
   * 金额
   */
  amount?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位
   */
  storageLocation?: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus?: number;

  /**
   * 领料时间
   */
  issueTime?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * MaintenanceWorkOrderMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceWorkOrderMaterialExport
 * @description 对应后端 TaktMaintenanceWorkOrderMaterialExportDto
 */
export interface MaintenanceWorkOrderMaterialExport {
  /**
   * MaintenanceWorkOrderMaterialID
   */
  maintenanceWorkOrderMaterialId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  materialId: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 需求数量
   */
  requiredQuantity: number;

  /**
   * 已领数量
   */
  issuedQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 单价
   */
  unitPrice: number;

  /**
   * 金额
   */
  amount: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位
   */
  storageLocation?: string;

  /**
   * 领料状态（0=待领料，1=部分领料，2=已领料）
   */
  issueStatus: number;

  /**
   * 领料时间
   */
  issueTime?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

