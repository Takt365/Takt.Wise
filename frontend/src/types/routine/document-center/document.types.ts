// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/document-center
// 文件名称：document.types.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制；需审批通过后发布（草稿→审批→发布） 审批态见基类 ApprovalStatus，字典 sys_approval_status
 * 对应前端 TaktDocumentDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Document
 * @description 对应后端 TaktDocumentDto
 */
export interface Document extends ApprovalDtoBase {
  /**
   * DocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  documentId: string;

  /**
   * 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
   */
  documentCode: string;

  /**
   * 文档标题
   */
  documentTitle: string;

  /**
   * 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
   */
  documentCategory: number;

  /**
   * 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
   */
  confidentialLevel: number;

  /**
   * 当前版本号
   */
  version: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName: string;

  /**
   * 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 归属部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  documentIsTop: number;

  /**
   * 浏览次数
   */
  documentViewCount: number;

  /**
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
   */
  targetUsers?: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus: number;

  /**
   * 版本历史列表（主子表关系） （子表：TaktDocumentVersion）
   */
  versions?: DocumentVersion[];

}


/**
 * Document 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DocumentQuery
 * @description 对应后端 TaktDocumentQueryDto
 */
export interface DocumentQuery extends TaktPagedQuery {
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
   * 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  documentTitle?: string;

  /**
   * 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
   */
  documentCategory?: number;

  /**
   * 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
   */
  confidentialLevel?: number;

  /**
   * 当前版本号
   */
  version?: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 生效时间（范围查询-开始）
   */
  documentEffectiveTimeStart?: string;

  /**
   * 生效时间（范围查询-结束）
   */
  documentEffectiveTimeEnd?: string;

  /**
   * 失效时间（范围查询-开始）
   */
  documentExpireTimeStart?: string;

  /**
   * 失效时间（范围查询-结束）
   */
  documentExpireTimeEnd?: string;

  /**
   * 发布时间（范围查询-开始）
   */
  documentPublishTimeStart?: string;

  /**
   * 发布时间（范围查询-结束）
   */
  documentPublishTimeEnd?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId?: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName?: string;

  /**
   * 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 归属部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  documentIsTop?: number;

  /**
   * 浏览次数
   */
  documentViewCount?: number;

  /**
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope?: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
   */
  targetUsers?: string;

  /**
   * 排序号（回填）
   */
  sortOrder?: number;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus?: number;

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
 * 创建Document DTO
 * 对应前端 DocumentCreate
 * @description 对应后端 TaktDocumentCreateDto
 */
export interface DocumentCreate {
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
   * 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
   */
  documentCode: string;

  /**
   * 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 文档标题
   */
  documentTitle: string;

  /**
   * 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
   */
  documentCategory: number;

  /**
   * 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
   */
  confidentialLevel: number;

  /**
   * 当前版本号
   */
  version: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName: string;

  /**
   * 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 归属部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  documentIsTop: number;

  /**
   * 浏览次数
   */
  documentViewCount: number;

  /**
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
   */
  targetUsers?: string;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus: number;

  /**
   * 版本历史列表（主子表关系）（子表，级联保存）
   */
  versions?: DocumentVersionCreate[];

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
 * 更新Document DTO
 * 继承 TaktDocumentCreateDto，添加 DocumentId 字段
 * 对应前端 DocumentUpdate
 * @description 对应后端 TaktDocumentUpdateDto
 */
export interface DocumentUpdate extends DocumentCreate {
  /**
   * DocumentID（标识要更新的实体）
   */
  documentId: string;

  /**
   * 版本历史列表（主子表关系）（子表，级联保存）
   */
  versions?: any;

}


/**
 * Document 状态更新 DTO
 * 对应前端 DocumentStatus
 * @description 对应后端 TaktDocumentStatusDto
 */
export interface DocumentStatus {
  /**
   * DocumentID
   */
  documentId: string;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus: number;

}


/**
 * Document 排序更新 DTO
 * 对应前端 DocumentSort
 * @description 对应后端 TaktDocumentSortDto
 */
export interface DocumentSort {
  /**
   * DocumentID
   */
  documentId: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

}


/**
 * Document 导入模板行 DTO
 * 对应前端 DocumentTemplate
 * @description 对应后端 TaktDocumentTemplateDto
 */
export interface DocumentTemplate {
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
   * 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  documentTitle?: string;

  /**
   * 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
   */
  documentCategory?: number;

  /**
   * 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
   */
  confidentialLevel?: number;

  /**
   * 当前版本号
   */
  version?: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId?: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName?: string;

  /**
   * 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 归属部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  documentIsTop?: number;

  /**
   * 浏览次数
   */
  documentViewCount?: number;

  /**
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope?: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
   */
  targetUsers?: string;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus?: number;

  /**
   * 版本历史列表（主子表关系）（子表，级联保存）
   */
  versions?: DocumentVersionCreate[];

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
 * Document 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DocumentImport
 * @description 对应后端 TaktDocumentImportDto
 */
export interface DocumentImport {
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
   * 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
   */
  documentCode?: string;

  /**
   * 文档标题
   */
  documentTitle?: string;

  /**
   * 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
   */
  documentCategory?: number;

  /**
   * 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
   */
  confidentialLevel?: number;

  /**
   * 当前版本号
   */
  version?: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId?: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName?: string;

  /**
   * 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 归属部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  documentIsTop?: number;

  /**
   * 浏览次数
   */
  documentViewCount?: number;

  /**
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope?: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
   */
  targetUsers?: string;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus?: number;

  /**
   * 版本历史列表（主子表关系）（子表，级联保存）
   */
  versions?: DocumentVersionCreate[];

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
 * Document 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DocumentExport
 * @description 对应后端 TaktDocumentExportDto
 */
export interface DocumentExport {
  /**
   * DocumentID
   */
  documentId: string;

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
   * 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
   */
  documentCode: string;

  /**
   * 文档标题
   */
  documentTitle: string;

  /**
   * 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
   */
  documentCategory: number;

  /**
   * 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
   */
  confidentialLevel: number;

  /**
   * 当前版本号
   */
  version: number;

  /**
   * 文档内容（富文本 HTML）
   */
  documentContent?: string;

  /**
   * 文档摘要（用于列表展示）
   */
  documentSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  documentTags?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 生效时间
   */
  documentEffectiveTime?: string;

  /**
   * 失效时间
   */
  documentExpireTime?: string;

  /**
   * 发布时间
   */
  documentPublishTime?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName: string;

  /**
   * 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 归属部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  documentIsTop: number;

  /**
   * 浏览次数
   */
  documentViewCount: number;

  /**
   * 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
   */
  targetScope: number;

  /**
   * 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
   */
  targetDepartments?: string;

  /**
   * 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
   */
  targetUsers?: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  documentStatus: number;

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

