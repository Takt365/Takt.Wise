// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：ticket.types.ts
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
 * 服务工单实体
 * 对应前端 TaktCustomerServiceTicketDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerServiceTicket
 * @description 对应后端 TaktCustomerServiceTicketDto
 */
export interface CustomerServiceTicket extends CompanyDtoBase {
  /**
   * CustomerServiceTicketID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerServiceTicketId: string;

  /**
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求名称（填充字段）
   */
  serviceRequestName?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单名称（填充字段）
   */
  serviceOrderName?: string;

  /**
   * 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceOrderCode?: string;

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
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

  /**
   * 工单主题
   */
  ticketSubject: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人（选项 TaktEmployees/options；DictValue=Id）
   */
  acceptedByEmployeeId?: string;

  /**
   * 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  acceptedByEmployeeName?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 关联服务请求 （主表：TaktCustomerServiceRequest）
   */
  customerServiceRequest?: CustomerServiceRequest;

  /**
   * 关联服务订单 （主表：TaktCustomerServiceOrder）
   */
  customerServiceOrder?: CustomerServiceOrder;

  /**
   * 关联服务合同 （主表：TaktCustomerServiceContract）
   */
  customerServiceContract?: CustomerServiceContract;

}


/**
 * CustomerServiceTicket 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerServiceTicketQuery
 * @description 对应后端 TaktCustomerServiceTicketQueryDto
 */
export interface CustomerServiceTicketQuery extends TaktPagedQuery {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType?: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority?: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus?: number;

  /**
   * 工单主题
   */
  ticketSubject?: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间（范围查询-开始）
   */
  scheduledStartTimeStart?: string;

  /**
   * 计划开始时间（范围查询-结束）
   */
  scheduledStartTimeEnd?: string;

  /**
   * 计划结束时间（范围查询-开始）
   */
  scheduledEndTimeStart?: string;

  /**
   * 计划结束时间（范围查询-结束）
   */
  scheduledEndTimeEnd?: string;

  /**
   * 实际开始时间（范围查询-开始）
   */
  actualStartTimeStart?: string;

  /**
   * 实际开始时间（范围查询-结束）
   */
  actualStartTimeEnd?: string;

  /**
   * 实际结束时间（范围查询-开始）
   */
  actualEndTimeStart?: string;

  /**
   * 实际结束时间（范围查询-结束）
   */
  actualEndTimeEnd?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人（选项 TaktEmployees/options；DictValue=Id）
   */
  acceptedByEmployeeId?: string;

  /**
   * 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  acceptedByEmployeeName?: string;

  /**
   * 验收时间（范围查询-开始）
   */
  acceptedAtStart?: string;

  /**
   * 验收时间（范围查询-结束）
   */
  acceptedAtEnd?: string;

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
 * 创建CustomerServiceTicket DTO
 * 对应前端 CustomerServiceTicketCreate
 * @description 对应后端 TaktCustomerServiceTicketCreateDto
 */
export interface CustomerServiceTicketCreate {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

  /**
   * 工单主题
   */
  ticketSubject: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人（选项 TaktEmployees/options；DictValue=Id）
   */
  acceptedByEmployeeId?: string;

  /**
   * 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  acceptedByEmployeeName?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

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
 * 更新CustomerServiceTicket DTO
 * 继承 TaktCustomerServiceTicketCreateDto，添加 CustomerServiceTicketId 字段
 * 对应前端 CustomerServiceTicketUpdate
 * @description 对应后端 TaktCustomerServiceTicketUpdateDto
 */
export interface CustomerServiceTicketUpdate extends CustomerServiceTicketCreate {
  /**
   * CustomerServiceTicketID（标识要更新的实体）
   */
  customerServiceTicketId: string;

}


/**
 * CustomerServiceTicket 状态更新 DTO
 * 对应前端 CustomerServiceTicketStatus
 * @description 对应后端 TaktCustomerServiceTicketStatusDto
 */
export interface CustomerServiceTicketStatus {
  /**
   * CustomerServiceTicketID
   */
  customerServiceTicketId: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

}


/**
 * CustomerServiceTicket 排序更新 DTO
 * 对应前端 CustomerServiceTicketSort
 * @description 对应后端 TaktCustomerServiceTicketSortDto
 */
export interface CustomerServiceTicketSort {
  /**
   * CustomerServiceTicketID
   */
  customerServiceTicketId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerServiceTicket 导入模板行 DTO
 * 对应前端 CustomerServiceTicketTemplate
 * @description 对应后端 TaktCustomerServiceTicketTemplateDto
 */
export interface CustomerServiceTicketTemplate {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType?: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority?: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus?: number;

  /**
   * 工单主题
   */
  ticketSubject?: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人（选项 TaktEmployees/options；DictValue=Id）
   */
  acceptedByEmployeeId?: string;

  /**
   * 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  acceptedByEmployeeName?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

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
 * CustomerServiceTicket 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerServiceTicketImport
 * @description 对应后端 TaktCustomerServiceTicketImportDto
 */
export interface CustomerServiceTicketImport {
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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType?: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority?: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus?: number;

  /**
   * 工单主题
   */
  ticketSubject?: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人（选项 TaktEmployees/options；DictValue=Id）
   */
  acceptedByEmployeeId?: string;

  /**
   * 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  acceptedByEmployeeName?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

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
 * CustomerServiceTicket 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerServiceTicketExport
 * @description 对应后端 TaktCustomerServiceTicketExportDto
 */
export interface CustomerServiceTicketExport {
  /**
   * CustomerServiceTicketID
   */
  customerServiceTicketId: string;

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
   * 服务工单编码（组合唯一索引）
   */
  serviceTicketCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 关联服务订单ID（序列化为string以避免Javascript精度问题）
   */
  serviceOrderId?: string;

  /**
   * 关联服务订单编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceOrderCode?: string;

  /**
   * 关联服务合同ID（序列化为string以避免Javascript精度问题）
   */
  serviceContractId?: string;

  /**
   * 关联服务合同编码（冗余：按对应 Id 取主数据名称联动）
   */
  serviceContractCode?: string;

  /**
   * 工单类型（0=维修，1=巡检，2=安装，3=升级，4=其他）
   */
  ticketType: number;

  /**
   * 优先级（字典 sys_priority_level）
   */
  priority: number;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
   */
  ticketStatus: number;

  /**
   * 工单主题
   */
  ticketSubject: string;

  /**
   * 故障/问题描述
   */
  faultDescription?: string;

  /**
   * 处理方案/解决说明
   */
  solutionDescription?: string;

  /**
   * 服务地点
   */
  serviceLocation?: string;

  /**
   * 指派服务人员工ID（序列化为string以避免Javascript精度问题）
   */
  assignedEmployeeId?: string;

  /**
   * 指派服务人员姓名
   */
  assignedEmployeeName?: string;

  /**
   * 计划开始时间
   */
  scheduledStartTime?: string;

  /**
   * 计划结束时间
   */
  scheduledEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 验收结果（0=不合格，1=合格，2=部分合格）
   */
  acceptanceResult?: number;

  /**
   * 验收人（选项 TaktEmployees/options；DictValue=Id）
   */
  acceptedByEmployeeId?: string;

  /**
   * 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  acceptedByEmployeeName?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

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

