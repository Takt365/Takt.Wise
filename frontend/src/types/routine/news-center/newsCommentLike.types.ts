// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：newsCommentLike.types.ts
// 创建时间：2026-08-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 新闻中心评论点赞记录实体
 * 对应前端 TaktNewsCommentLikeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsCommentLike
 * @description 对应后端 TaktNewsCommentLikeDto
 */
export interface NewsCommentLike extends CompanyDtoBase {
  /**
   * NewsCommentLikeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  newsCommentLikeId: string;

  /**
   * 评论 ID（选项 TaktNewsComments/options；DictValue=Id）
   */
  commentId: string;

  /**
   * 评论 名称（填充字段）
   */
  commentName?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName: string;

  /**
   * 点赞时间
   */
  likeTime: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 评论（主表） （主表：TaktNewsComment）
   */
  comment?: NewsComment;

}


/**
 * NewsCommentLike 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsCommentLikeQuery
 * @description 对应后端 TaktNewsCommentLikeQueryDto
 */
export interface NewsCommentLikeQuery extends TaktPagedQuery {
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
   * 评论 ID（选项 TaktNewsComments/options；DictValue=Id）
   */
  commentId?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName?: string;

  /**
   * 点赞时间（范围查询-开始）
   */
  likeTimeStart?: string;

  /**
   * 点赞时间（范围查询-结束）
   */
  likeTimeEnd?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * 创建NewsCommentLike DTO
 * 对应前端 NewsCommentLikeCreate
 * @description 对应后端 TaktNewsCommentLikeCreateDto
 */
export interface NewsCommentLikeCreate {
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
   * 评论 ID（选项 TaktNewsComments/options；DictValue=Id）
   */
  commentId: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName: string;

  /**
   * 点赞时间
   */
  likeTime: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * 更新NewsCommentLike DTO
 * 继承 TaktNewsCommentLikeCreateDto，添加 NewsCommentLikeId 字段
 * 对应前端 NewsCommentLikeUpdate
 * @description 对应后端 TaktNewsCommentLikeUpdateDto
 */
export interface NewsCommentLikeUpdate extends NewsCommentLikeCreate {
  /**
   * NewsCommentLikeID（标识要更新的实体）
   */
  newsCommentLikeId: string;

}


/**
 * NewsCommentLike 作废/撤销作废 DTO
 * 对应前端 NewsCommentLikeObsolete
 * @description 对应后端 TaktNewsCommentLikeObsoleteDto
 */
export interface NewsCommentLikeObsolete {
  /**
   * NewsCommentLikeID
   */
  newsCommentLikeId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * NewsCommentLike 导入模板行 DTO
 * 对应前端 NewsCommentLikeTemplate
 * @description 对应后端 TaktNewsCommentLikeTemplateDto
 */
export interface NewsCommentLikeTemplate {
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
   * 评论 ID（选项 TaktNewsComments/options；DictValue=Id）
   */
  commentId?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName?: string;

  /**
   * 点赞时间
   */
  likeTime?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * NewsCommentLike 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsCommentLikeImport
 * @description 对应后端 TaktNewsCommentLikeImportDto
 */
export interface NewsCommentLikeImport {
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
   * 评论 ID（选项 TaktNewsComments/options；DictValue=Id）
   */
  commentId?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName?: string;

  /**
   * 点赞时间
   */
  likeTime?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * NewsCommentLike 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsCommentLikeExport
 * @description 对应后端 TaktNewsCommentLikeExportDto
 */
export interface NewsCommentLikeExport {
  /**
   * NewsCommentLikeID
   */
  newsCommentLikeId: string;

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
   * 评论 ID（选项 TaktNewsComments/options；DictValue=Id）
   */
  commentId: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName: string;

  /**
   * 点赞时间
   */
  likeTime: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

