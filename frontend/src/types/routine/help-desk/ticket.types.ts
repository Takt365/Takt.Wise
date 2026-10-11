// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticket.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 服务台工单实体
 * 对应前端 TaktTicketDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Ticket
 * @description 对应后端 TaktTicketDto
 */
export interface Ticket extends CompanyDtoBase {
  /**
   * TicketID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ticketId: string;

  /**
   * 工单编码（唯一）
   */
  ticketCode: string;

  /**
   * 工单标题
   */
  ticketTitle: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
   */
  urgency: number;

  /**
   * 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
   */
  impact: number;

  /**
   * 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
   */
  categoryCode?: string;

  /**
   * 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
   */
  ticketSource: number;

  /**
   * 提交人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  submitterId: string;

  /**
   * 提交人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  submitterName?: string;

  /**
   * 处理人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  assigneeId?: string;

  /**
   * 处理人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  assigneeName?: string;

  /**
   * 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
   */
  knowledgeId?: string;

  /**
   * 关联知识 名称（填充字段）
   */
  knowledgeName?: string;

  /**
   * 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
   */
  parentTicketId?: string;

  /**
   * 父工单 名称（填充字段）
   */
  parentTicketName?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
   */
  itAssetId?: string;

  /**
   * IT 设备保修扩展 名称（填充字段）
   */
  itAssetName?: string;

  /**
   * 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称（冗余：按对应 Id 取主数据名称联动）
   */
  applicantDeptName?: string;

  /**
   * 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
   */
  applicantName?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId） （子表：TaktTicket）
   */
  childTickets?: Ticket[];

}


/**
 * Ticket 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TicketQuery
 * @description 对应后端 TaktTicketQueryDto
 */
export interface TicketQuery extends TaktPagedQuery {
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
   * 工单编码（唯一）
   */
  ticketCode?: string;

  /**
   * 工单标题
   */
  ticketTitle?: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
   */
  urgency?: number;

  /**
   * 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
   */
  impact?: number;

  /**
   * 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
   */
  categoryCode?: string;

  /**
   * 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
   */
  ticketSource?: number;

  /**
   * 提交人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  submitterId?: string;

  /**
   * 提交人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  submitterName?: string;

  /**
   * 处理人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  assigneeId?: string;

  /**
   * 处理人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  assigneeName?: string;

  /**
   * 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）（范围查询-开始）
   */
  firstResponseAtStart?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）（范围查询-结束）
   */
  firstResponseAtEnd?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）（范围查询-开始）
   */
  firstResponseDueByStart?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）（范围查询-结束）
   */
  firstResponseDueByEnd?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）（范围查询-开始）
   */
  resolvedAtStart?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）（范围查询-结束）
   */
  resolvedAtEnd?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）（范围查询-开始）
   */
  resolutionDueByStart?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）（范围查询-结束）
   */
  resolutionDueByEnd?: string;

  /**
   * 关闭时间（工单最终关闭的时间）（范围查询-开始）
   */
  closedAtStart?: string;

  /**
   * 关闭时间（工单最终关闭的时间）（范围查询-结束）
   */
  closedAtEnd?: string;

  /**
   * IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称（冗余：按对应 Id 取主数据名称联动）
   */
  applicantDeptName?: string;

  /**
   * 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
   */
  applicantBy?: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
   */
  applicantName?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus?: number;

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
 * 创建Ticket DTO
 * 对应前端 TicketCreate
 * @description 对应后端 TaktTicketCreateDto
 */
export interface TicketCreate {
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
   * 工单编码（唯一）
   */
  ticketCode: string;

  /**
   * 工单标题
   */
  ticketTitle: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
   */
  urgency: number;

  /**
   * 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
   */
  impact: number;

  /**
   * 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
   */
  categoryCode?: string;

  /**
   * 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
   */
  ticketSource: number;

  /**
   * 提交人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  submitterId: string;

  /**
   * 提交人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  submitterName?: string;

  /**
   * 处理人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  assigneeId?: string;

  /**
   * 处理人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  assigneeName?: string;

  /**
   * 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称（冗余：按对应 Id 取主数据名称联动）
   */
  applicantDeptName?: string;

  /**
   * 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
   */
  applicantName?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: TicketCreate[];

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
 * 更新Ticket DTO
 * 继承 TaktTicketCreateDto，添加 TicketId 字段
 * 对应前端 TicketUpdate
 * @description 对应后端 TaktTicketUpdateDto
 */
export interface TicketUpdate extends TicketCreate {
  /**
   * TicketID（标识要更新的实体）
   */
  ticketId: string;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: any;

}


/**
 * Ticket 状态更新 DTO
 * 对应前端 TicketStatus
 * @description 对应后端 TaktTicketStatusDto
 */
export interface TicketStatus {
  /**
   * TicketID
   */
  ticketId: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus: number;

}


/**
 * Ticket 导入模板行 DTO
 * 对应前端 TicketTemplate
 * @description 对应后端 TaktTicketTemplateDto
 */
export interface TicketTemplate {
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
   * 工单编码（唯一）
   */
  ticketCode?: string;

  /**
   * 工单标题
   */
  ticketTitle?: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
   */
  urgency?: number;

  /**
   * 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
   */
  impact?: number;

  /**
   * 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
   */
  categoryCode?: string;

  /**
   * 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
   */
  ticketSource?: number;

  /**
   * 提交人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  submitterId?: string;

  /**
   * 提交人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  submitterName?: string;

  /**
   * 处理人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  assigneeId?: string;

  /**
   * 处理人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  assigneeName?: string;

  /**
   * 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称（冗余：按对应 Id 取主数据名称联动）
   */
  applicantDeptName?: string;

  /**
   * 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
   */
  applicantBy?: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
   */
  applicantName?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus?: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: TicketCreate[];

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
 * Ticket 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TicketImport
 * @description 对应后端 TaktTicketImportDto
 */
export interface TicketImport {
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
   * 工单编码（唯一）
   */
  ticketCode?: string;

  /**
   * 工单标题
   */
  ticketTitle?: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
   */
  urgency?: number;

  /**
   * 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
   */
  impact?: number;

  /**
   * 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
   */
  categoryCode?: string;

  /**
   * 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
   */
  ticketSource?: number;

  /**
   * 提交人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  submitterId?: string;

  /**
   * 提交人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  submitterName?: string;

  /**
   * 处理人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  assigneeId?: string;

  /**
   * 处理人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  assigneeName?: string;

  /**
   * 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称（冗余：按对应 Id 取主数据名称联动）
   */
  applicantDeptName?: string;

  /**
   * 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
   */
  applicantBy?: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
   */
  applicantName?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus?: number;

  /**
   * 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
   */
  childTickets?: TicketCreate[];

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
 * Ticket 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketExport
 * @description 对应后端 TaktTicketExportDto
 */
export interface TicketExport {
  /**
   * TicketID
   */
  ticketId: string;

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
   * 工单编码（唯一）
   */
  ticketCode: string;

  /**
   * 工单标题
   */
  ticketTitle: string;

  /**
   * 工单内容描述
   */
  ticketContent?: string;

  /**
   * 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
   */
  urgency: number;

  /**
   * 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
   */
  impact: number;

  /**
   * 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
   */
  categoryCode?: string;

  /**
   * 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
   */
  ticketSource: number;

  /**
   * 提交人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  submitterId: string;

  /**
   * 提交人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  submitterName?: string;

  /**
   * 处理人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  assigneeId?: string;

  /**
   * 处理人姓名（冗余：按对应 Id 取主数据名称联动）
   */
  assigneeName?: string;

  /**
   * 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
   */
  knowledgeId?: string;

  /**
   * 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
   */
  parentTicketId?: string;

  /**
   * 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
   */
  firstResponseAt?: string;

  /**
   * 首次响应期限（根据 SLA 计算出的首次响应截止时间）
   */
  firstResponseDueBy?: string;

  /**
   * 解决时间（问题被标记为已解决的时间）
   */
  resolvedAt?: string;

  /**
   * 解决期限（根据 SLA 计算出的解决截止时间）
   */
  resolutionDueBy?: string;

  /**
   * 关闭时间（工单最终关闭的时间）
   */
  closedAt?: string;

  /**
   * IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
   */
  assetCode?: string;

  /**
   * 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicantDeptId?: string;

  /**
   * 申请部门名称（冗余：按对应 Id 取主数据名称联动）
   */
  applicantDeptName?: string;

  /**
   * 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
   */
  applicantName?: string;

  /**
   * 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
   */
  ticketStatus: number;

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

/**
 * 当前用户工单关联资产汇总行（门户「我的资产」）
 */
export interface TicketMyAsset {
  /**
   * 资产编码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 关联工单数
   */
  ticketCount?: number;

  /**
   * 最近工单时间
   */
  lastTicketAt?: string;
}

