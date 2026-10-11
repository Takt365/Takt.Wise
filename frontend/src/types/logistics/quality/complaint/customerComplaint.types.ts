// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerComplaint.types.ts
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 客诉主表实体
 * 对应前端 TaktCustomerComplaintDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaint
 * @description 对应后端 TaktCustomerComplaintDto
 */
export interface CustomerComplaint extends CompanyDtoBase {
  /**
   * CustomerComplaintID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerComplaintId: string;

  /**
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户 名称（填充字段）
   */
  customerName?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel: number;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus: number;

  /**
   * 客诉明细列表（主子表关系） （子表：TaktCustomerComplaintItem）
   */
  items?: CustomerComplaintItem[];

}


/**
 * CustomerComplaint 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerComplaintQuery
 * @description 对应后端 TaktCustomerComplaintQueryDto
 */
export interface CustomerComplaintQuery extends TaktPagedQuery {
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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode?: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期（范围查询-开始）
   */
  complaintDateStart?: string;

  /**
   * 投诉日期（范围查询-结束）
   */
  complaintDateEnd?: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod?: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType?: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel?: number;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 要求回复日期（范围查询-开始）
   */
  requiredReplyDateStart?: string;

  /**
   * 要求回复日期（范围查询-结束）
   */
  requiredReplyDateEnd?: string;

  /**
   * 实际回复日期（范围查询-开始）
   */
  actualReplyDateStart?: string;

  /**
   * 实际回复日期（范围查询-结束）
   */
  actualReplyDateEnd?: string;

  /**
   * 客诉描述
   */
  complaintDescription?: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus?: number;

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
 * 创建CustomerComplaint DTO
 * 对应前端 CustomerComplaintCreate
 * @description 对应后端 TaktCustomerComplaintCreateDto
 */
export interface CustomerComplaintCreate {
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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel: number;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus: number;

  /**
   * 客诉明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerComplaintItemCreate[];

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
 * 更新CustomerComplaint DTO
 * 继承 TaktCustomerComplaintCreateDto，添加 CustomerComplaintId 字段
 * 对应前端 CustomerComplaintUpdate
 * @description 对应后端 TaktCustomerComplaintUpdateDto
 */
export interface CustomerComplaintUpdate extends CustomerComplaintCreate {
  /**
   * CustomerComplaintID（标识要更新的实体）
   */
  customerComplaintId: string;

  /**
   * 客诉明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * CustomerComplaint 状态更新 DTO
 * 对应前端 CustomerComplaintStatus
 * @description 对应后端 TaktCustomerComplaintStatusDto
 */
export interface CustomerComplaintStatus {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus: number;

}


/**
 * CustomerComplaint 排序更新 DTO
 * 对应前端 CustomerComplaintSort
 * @description 对应后端 TaktCustomerComplaintSortDto
 */
export interface CustomerComplaintSort {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerComplaint 导入模板行 DTO
 * 对应前端 CustomerComplaintTemplate
 * @description 对应后端 TaktCustomerComplaintTemplateDto
 */
export interface CustomerComplaintTemplate {
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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode?: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate?: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod?: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType?: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel?: number;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉描述
   */
  complaintDescription?: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus?: number;

  /**
   * 客诉明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerComplaintItemCreate[];

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
 * CustomerComplaint 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerComplaintImport
 * @description 对应后端 TaktCustomerComplaintImportDto
 */
export interface CustomerComplaintImport {
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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode?: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate?: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod?: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType?: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel?: number;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉描述
   */
  complaintDescription?: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus?: number;

  /**
   * 客诉明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerComplaintItemCreate[];

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
 * CustomerComplaint 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintExport
 * @description 对应后端 TaktCustomerComplaintExportDto
 */
export interface CustomerComplaintExport {
  /**
   * CustomerComplaintID
   */
  customerComplaintId: string;

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
   * 客诉单号（组合唯一索引）
   */
  customerComplaintCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 投诉日期
   */
  complaintDate: string;

  /**
   * 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
   */
  complaintMethod: number;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type）
   */
  complaintType: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level）
   */
  complaintLevel: number;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 要求回复日期
   */
  requiredReplyDate?: string;

  /**
   * 实际回复日期
   */
  actualReplyDate?: string;

  /**
   * 客诉描述
   */
  complaintDescription: string;

  /**
   * 处理结果/回复内容
   */
  handlingResult?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客诉状态（字典 logistics_quality_complaint_status）
   */
  complaintStatus: number;

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

