// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：news.types.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计；正文为富文本 HTML；需审批通过后发布（草稿→审批→发布） 审批态见基类 ApprovalStatus，字典 sys_approval_status
 * 对应前端 TaktNewsDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 News
 * @description 对应后端 TaktNewsDto
 */
export interface News extends ApprovalDtoBase {
  /**
   * NewsID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  newsId: string;

  /**
   * 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
   */
  newsCode: string;

  /**
   * 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
   */
  newsCategory: number;

  /**
   * 新闻标题
   */
  newsTitle: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  newsIsTop: number;

  /**
   * 推荐（字典 sys_yes_no；0=否 1=是）
   */
  newsIsRecommended: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount: number;

  /**
   * 点赞次数
   */
  newsLikeCount: number;

  /**
   * 评论次数
   */
  newsCommentCount: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount: number;

  /**
   * 分享次数
   */
  newsShareCount: number;

  /**
   * 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 发布部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

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
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus: number;

}


/**
 * News 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsQuery
 * @description 对应后端 TaktNewsQueryDto
 */
export interface NewsQuery extends TaktPagedQuery {
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
   * 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
   */
  newsCode?: string;

  /**
   * 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
   */
  newsCategory?: number;

  /**
   * 新闻标题
   */
  newsTitle?: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  newsIsTop?: number;

  /**
   * 推荐（字典 sys_yes_no；0=否 1=是）
   */
  newsIsRecommended?: number;

  /**
   * 生效时间（范围查询-开始）
   */
  newsEffectiveTimeStart?: string;

  /**
   * 生效时间（范围查询-结束）
   */
  newsEffectiveTimeEnd?: string;

  /**
   * 失效时间（范围查询-开始）
   */
  newsExpireTimeStart?: string;

  /**
   * 失效时间（范围查询-结束）
   */
  newsExpireTimeEnd?: string;

  /**
   * 阅读次数
   */
  newsReadCount?: number;

  /**
   * 点赞次数
   */
  newsLikeCount?: number;

  /**
   * 评论次数
   */
  newsCommentCount?: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount?: number;

  /**
   * 分享次数
   */
  newsShareCount?: number;

  /**
   * 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 发布部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId?: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName?: string;

  /**
   * 发布时间（范围查询-开始）
   */
  newsPublishTimeStart?: string;

  /**
   * 发布时间（范围查询-结束）
   */
  newsPublishTimeEnd?: string;

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
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus?: number;

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
 * 创建News DTO
 * 对应前端 NewsCreate
 * @description 对应后端 TaktNewsCreateDto
 */
export interface NewsCreate {
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
   * 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
   */
  newsCode: string;

  /**
   * 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
   */
  newsCategory: number;

  /**
   * 新闻标题
   */
  newsTitle: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  newsIsTop: number;

  /**
   * 推荐（字典 sys_yes_no；0=否 1=是）
   */
  newsIsRecommended: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount: number;

  /**
   * 点赞次数
   */
  newsLikeCount: number;

  /**
   * 评论次数
   */
  newsCommentCount: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount: number;

  /**
   * 分享次数
   */
  newsShareCount: number;

  /**
   * 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 发布部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

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
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus: number;

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
 * 更新News DTO
 * 继承 TaktNewsCreateDto，添加 NewsId 字段
 * 对应前端 NewsUpdate
 * @description 对应后端 TaktNewsUpdateDto
 */
export interface NewsUpdate extends NewsCreate {
  /**
   * NewsID（标识要更新的实体）
   */
  newsId: string;

}


/**
 * News 状态更新 DTO
 * 对应前端 NewsStatus
 * @description 对应后端 TaktNewsStatusDto
 */
export interface NewsStatus {
  /**
   * NewsID
   */
  newsId: string;

  /**
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus: number;

}


/**
 * News 排序更新 DTO
 * 对应前端 NewsSort
 * @description 对应后端 TaktNewsSortDto
 */
export interface NewsSort {
  /**
   * NewsID
   */
  newsId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * News 导入模板行 DTO
 * 对应前端 NewsTemplate
 * @description 对应后端 TaktNewsTemplateDto
 */
export interface NewsTemplate {
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
   * 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
   */
  newsCode?: string;

  /**
   * 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
   */
  newsCategory?: number;

  /**
   * 新闻标题
   */
  newsTitle?: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  newsIsTop?: number;

  /**
   * 推荐（字典 sys_yes_no；0=否 1=是）
   */
  newsIsRecommended?: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount?: number;

  /**
   * 点赞次数
   */
  newsLikeCount?: number;

  /**
   * 评论次数
   */
  newsCommentCount?: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount?: number;

  /**
   * 分享次数
   */
  newsShareCount?: number;

  /**
   * 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 发布部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId?: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName?: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

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
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus?: number;

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
 * News 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsImport
 * @description 对应后端 TaktNewsImportDto
 */
export interface NewsImport {
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
   * 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
   */
  newsCode?: string;

  /**
   * 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
   */
  newsCategory?: number;

  /**
   * 新闻标题
   */
  newsTitle?: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
   */
  newsContent?: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  newsIsTop?: number;

  /**
   * 推荐（字典 sys_yes_no；0=否 1=是）
   */
  newsIsRecommended?: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount?: number;

  /**
   * 点赞次数
   */
  newsLikeCount?: number;

  /**
   * 评论次数
   */
  newsCommentCount?: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount?: number;

  /**
   * 分享次数
   */
  newsShareCount?: number;

  /**
   * 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 发布部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId?: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName?: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

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
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus?: number;

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
 * News 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsExport
 * @description 对应后端 TaktNewsExportDto
 */
export interface NewsExport {
  /**
   * NewsID
   */
  newsId: string;

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
   * 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
   */
  newsCode: string;

  /**
   * 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
   */
  newsCategory: number;

  /**
   * 新闻标题
   */
  newsTitle: string;

  /**
   * 新闻摘要（用于列表展示）
   */
  newsSummary?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  newsTags?: string;

  /**
   * 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
   */
  newsContent: string;

  /**
   * 新闻封面图片 URL
   */
  newsCoverImage?: string;

  /**
   * 置顶（字典 sys_yes_no；0=否 1=是）
   */
  newsIsTop: number;

  /**
   * 推荐（字典 sys_yes_no；0=否 1=是）
   */
  newsIsRecommended: number;

  /**
   * 生效时间
   */
  newsEffectiveTime?: string;

  /**
   * 失效时间
   */
  newsExpireTime?: string;

  /**
   * 阅读次数
   */
  newsReadCount: number;

  /**
   * 点赞次数
   */
  newsLikeCount: number;

  /**
   * 评论次数
   */
  newsCommentCount: number;

  /**
   * 收藏次数
   */
  newsFavoriteCount: number;

  /**
   * 分享次数
   */
  newsShareCount: number;

  /**
   * 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 发布部门名称（冗余字段，便于查询）
   */
  deptName?: string;

  /**
   * 发布人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  publisherId: string;

  /**
   * 发布人姓名（冗余字段，便于查询）
   */
  publisherName: string;

  /**
   * 发布时间
   */
  newsPublishTime?: string;

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
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
   */
  newsStatus: number;

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

