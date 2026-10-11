// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：order.types.ts
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
 * 服务订单实体
 * 对应前端 TaktCustomerServiceOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerServiceOrder
 * @description 对应后端 TaktCustomerServiceOrderDto
 */
export interface CustomerServiceOrder extends CompanyDtoBase {
  /**
   * CustomerServiceOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerServiceOrderId: string;

  /**
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode: string;

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
   * 订单日期
   */
  orderDate: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

  /**
   * 订单总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 订单实付金额
   */
  actualAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（选项 TaktEmployees/options；DictValue=Id）
   */
  serviceEmployeeId?: string;

  /**
   * 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  serviceEmployeeName?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 关联服务合同 （主表：TaktCustomerServiceContract）
   */
  customerServiceContract?: CustomerServiceContract;

  /**
   * 关联服务请求 （主表：TaktCustomerServiceRequest）
   */
  customerServiceRequest?: CustomerServiceRequest;

}


/**
 * CustomerServiceOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerServiceOrderQuery
 * @description 对应后端 TaktCustomerServiceOrderQueryDto
 */
export interface CustomerServiceOrderQuery extends TaktPagedQuery {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期（范围查询-开始）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围查询-结束）
   */
  orderDateEnd?: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType?: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus?: number;

  /**
   * 订单总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 订单实付金额
   */
  actualAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 计划开始日期（范围查询-开始）
   */
  plannedStartDateStart?: string;

  /**
   * 计划开始日期（范围查询-结束）
   */
  plannedStartDateEnd?: string;

  /**
   * 计划结束日期（范围查询-开始）
   */
  plannedEndDateStart?: string;

  /**
   * 计划结束日期（范围查询-结束）
   */
  plannedEndDateEnd?: string;

  /**
   * 实际开始日期（范围查询-开始）
   */
  actualStartDateStart?: string;

  /**
   * 实际开始日期（范围查询-结束）
   */
  actualStartDateEnd?: string;

  /**
   * 实际结束日期（范围查询-开始）
   */
  actualEndDateStart?: string;

  /**
   * 实际结束日期（范围查询-结束）
   */
  actualEndDateEnd?: string;

  /**
   * 服务负责人（选项 TaktEmployees/options；DictValue=Id）
   */
  serviceEmployeeId?: string;

  /**
   * 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  serviceEmployeeName?: string;

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
 * 创建CustomerServiceOrder DTO
 * 对应前端 CustomerServiceOrderCreate
 * @description 对应后端 TaktCustomerServiceOrderCreateDto
 */
export interface CustomerServiceOrderCreate {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

  /**
   * 订单总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 订单实付金额
   */
  actualAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（选项 TaktEmployees/options；DictValue=Id）
   */
  serviceEmployeeId?: string;

  /**
   * 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  serviceEmployeeName?: string;

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
 * 更新CustomerServiceOrder DTO
 * 继承 TaktCustomerServiceOrderCreateDto，添加 CustomerServiceOrderId 字段
 * 对应前端 CustomerServiceOrderUpdate
 * @description 对应后端 TaktCustomerServiceOrderUpdateDto
 */
export interface CustomerServiceOrderUpdate extends CustomerServiceOrderCreate {
  /**
   * CustomerServiceOrderID（标识要更新的实体）
   */
  customerServiceOrderId: string;

}


/**
 * CustomerServiceOrder 状态更新 DTO
 * 对应前端 CustomerServiceOrderStatus
 * @description 对应后端 TaktCustomerServiceOrderStatusDto
 */
export interface CustomerServiceOrderStatus {
  /**
   * CustomerServiceOrderID
   */
  customerServiceOrderId: string;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

}


/**
 * CustomerServiceOrder 排序更新 DTO
 * 对应前端 CustomerServiceOrderSort
 * @description 对应后端 TaktCustomerServiceOrderSortDto
 */
export interface CustomerServiceOrderSort {
  /**
   * CustomerServiceOrderID
   */
  customerServiceOrderId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerServiceOrder 导入模板行 DTO
 * 对应前端 CustomerServiceOrderTemplate
 * @description 对应后端 TaktCustomerServiceOrderTemplateDto
 */
export interface CustomerServiceOrderTemplate {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType?: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus?: number;

  /**
   * 订单总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 订单实付金额
   */
  actualAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（选项 TaktEmployees/options；DictValue=Id）
   */
  serviceEmployeeId?: string;

  /**
   * 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  serviceEmployeeName?: string;

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
 * CustomerServiceOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerServiceOrderImport
 * @description 对应后端 TaktCustomerServiceOrderImportDto
 */
export interface CustomerServiceOrderImport {
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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode?: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate?: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType?: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus?: number;

  /**
   * 订单总金额
   */
  totalAmount?: number;

  /**
   * 折扣金额
   */
  discountAmount?: number;

  /**
   * 税费
   */
  taxAmount?: number;

  /**
   * 订单实付金额
   */
  actualAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（选项 TaktEmployees/options；DictValue=Id）
   */
  serviceEmployeeId?: string;

  /**
   * 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  serviceEmployeeName?: string;

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
 * CustomerServiceOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerServiceOrderExport
 * @description 对应后端 TaktCustomerServiceOrderExportDto
 */
export interface CustomerServiceOrderExport {
  /**
   * CustomerServiceOrderID
   */
  customerServiceOrderId: string;

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
   * 服务订单编码（组合唯一索引）
   */
  serviceOrderCode: string;

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
   * 关联服务请求ID（序列化为string以避免Javascript精度问题）
   */
  serviceRequestId?: string;

  /**
   * 关联服务请求单号（冗余：按对应 Id 取主数据名称联动）
   */
  serviceRequestCode?: string;

  /**
   * 订单日期
   */
  orderDate: string;

  /**
   * 订单类型（0=现场服务，1=远程支持，2=备件更换，3=安装调试，4=其他）
   */
  orderType: number;

  /**
   * 订单状态（0=草稿，1=已确认，2=执行中，3=已完成，4=已结算，5=已取消）
   */
  orderStatus: number;

  /**
   * 订单总金额
   */
  totalAmount: number;

  /**
   * 折扣金额
   */
  discountAmount: number;

  /**
   * 税费
   */
  taxAmount: number;

  /**
   * 订单实付金额
   */
  actualAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 计划开始日期
   */
  plannedStartDate?: string;

  /**
   * 计划结束日期
   */
  plannedEndDate?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际结束日期
   */
  actualEndDate?: string;

  /**
   * 服务负责人（选项 TaktEmployees/options；DictValue=Id）
   */
  serviceEmployeeId?: string;

  /**
   * 服务负责人名称（冗余：按 ServiceEmployeeId 取 TaktEmployee.EmployeeName 联动）
   */
  serviceEmployeeName?: string;

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

