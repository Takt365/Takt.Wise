// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：request.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 服务请求实体
 * 对应前端 TaktCustomerServiceRequestDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerServiceRequest
 * @description 对应后端 TaktCustomerServiceRequestDto
 */
export interface CustomerServiceRequest extends CompanyDtoBase {
  /**
   * CustomerServiceRequestID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerServiceRequestId: string;

  /**
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端名称（填充字段）
   */
  clientName?: string;

  /**
   * 客户端编码（冗余：按对应 Id 取主数据名称联动）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余：按对应 Id 取主数据名称联动）
   */
  clientName1: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同名称（填充字段）
   */
  serviceContractName?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 请求日期
   */
  requestDate: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

  /**
   * 请求主题
   */
  requestSubject: string;

  /**
   * 请求描述
   */
  requestDescription: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 关联服务合同 （主表：TaktCustomerServiceContract）
   */
  customerServiceContract?: CustomerServiceContract;

}


/**
 * CustomerServiceRequest 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerServiceRequestQuery
 * @description 对应后端 TaktCustomerServiceRequestQueryDto
 */
export interface CustomerServiceRequestQuery extends TaktPagedQuery {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余：按对应 Id 取主数据名称联动）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余：按对应 Id 取主数据名称联动）
   */
  clientName1?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 请求日期（范围查询-开始）
   */
  requestDateStart?: string;

  /**
   * 请求日期（范围查询-结束）
   */
  requestDateEnd?: string;

  /**
   * 期望服务日期（范围查询-开始）
   */
  expectedServiceDateStart?: string;

  /**
   * 期望服务日期（范围查询-结束）
   */
  expectedServiceDateEnd?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType?: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel?: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority?: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus?: number;

  /**
   * 请求主题
   */
  requestSubject?: string;

  /**
   * 请求描述
   */
  requestDescription?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间（范围查询-开始）
   */
  assignedAtStart?: string;

  /**
   * 受理时间（范围查询-结束）
   */
  assignedAtEnd?: string;

  /**
   * 关闭时间（范围查询-开始）
   */
  closedAtStart?: string;

  /**
   * 关闭时间（范围查询-结束）
   */
  closedAtEnd?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

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
 * 创建CustomerServiceRequest DTO
 * 对应前端 CustomerServiceRequestCreate
 * @description 对应后端 TaktCustomerServiceRequestCreateDto
 */
export interface CustomerServiceRequestCreate {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余：按对应 Id 取主数据名称联动）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余：按对应 Id 取主数据名称联动）
   */
  clientName1: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 请求日期
   */
  requestDate: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

  /**
   * 请求主题
   */
  requestSubject: string;

  /**
   * 请求描述
   */
  requestDescription: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

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
 * 更新CustomerServiceRequest DTO
 * 继承 TaktCustomerServiceRequestCreateDto，添加 CustomerServiceRequestId 字段
 * 对应前端 CustomerServiceRequestUpdate
 * @description 对应后端 TaktCustomerServiceRequestUpdateDto
 */
export interface CustomerServiceRequestUpdate extends CustomerServiceRequestCreate {
  /**
   * CustomerServiceRequestID（标识要更新的实体）
   */
  customerServiceRequestId: string;

}


/**
 * CustomerServiceRequest 状态更新 DTO
 * 对应前端 CustomerServiceRequestStatus
 * @description 对应后端 TaktCustomerServiceRequestStatusDto
 */
export interface CustomerServiceRequestStatus {
  /**
   * CustomerServiceRequestID
   */
  customerServiceRequestId: string;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

}


/**
 * CustomerServiceRequest 排序更新 DTO
 * 对应前端 CustomerServiceRequestSort
 * @description 对应后端 TaktCustomerServiceRequestSortDto
 */
export interface CustomerServiceRequestSort {
  /**
   * CustomerServiceRequestID
   */
  customerServiceRequestId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerServiceRequest 导入模板行 DTO
 * 对应前端 CustomerServiceRequestTemplate
 * @description 对应后端 TaktCustomerServiceRequestTemplateDto
 */
export interface CustomerServiceRequestTemplate {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余：按对应 Id 取主数据名称联动）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余：按对应 Id 取主数据名称联动）
   */
  clientName1?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 请求日期
   */
  requestDate?: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType?: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel?: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority?: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus?: number;

  /**
   * 请求主题
   */
  requestSubject?: string;

  /**
   * 请求描述
   */
  requestDescription?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

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
 * CustomerServiceRequest 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerServiceRequestImport
 * @description 对应后端 TaktCustomerServiceRequestImportDto
 */
export interface CustomerServiceRequestImport {
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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余：按对应 Id 取主数据名称联动）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余：按对应 Id 取主数据名称联动）
   */
  clientName1?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 请求日期
   */
  requestDate?: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType?: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel?: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority?: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus?: number;

  /**
   * 请求主题
   */
  requestSubject?: string;

  /**
   * 请求描述
   */
  requestDescription?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

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
 * CustomerServiceRequest 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerServiceRequestExport
 * @description 对应后端 TaktCustomerServiceRequestExportDto
 */
export interface CustomerServiceRequestExport {
  /**
   * CustomerServiceRequestID
   */
  customerServiceRequestId: string;

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
   * 服务请求单号（组合唯一索引）
   */
  serviceRequestCode: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余：按对应 Id 取主数据名称联动）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余：按对应 Id 取主数据名称联动）
   */
  clientName1: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 请求日期
   */
  requestDate: string;

  /**
   * 期望服务日期
   */
  expectedServiceDate?: string;

  /**
   * 请求类型（0=咨询，1=报修，2=投诉，3=安装，4=巡检，5=其他）
   */
  requestType: number;

  /**
   * 请求来源（0=电话，1=邮件，2=门户，3=现场，4=其他）
   */
  sourceChannel: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority: number;

  /**
   * 请求状态（0=草稿，1=已提交，2=处理中，3=已完成，4=已关闭，5=已取消）
   */
  requestStatus: number;

  /**
   * 请求主题
   */
  requestSubject: string;

  /**
   * 请求描述
   */
  requestDescription: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 服务地址
   */
  serviceAddress?: string;

  /**
   * 受理人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 受理人姓名
   */
  assignedEmployeeName?: string;

  /**
   * 受理时间
   */
  assignedAt?: string;

  /**
   * 关闭时间
   */
  closedAt?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

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

