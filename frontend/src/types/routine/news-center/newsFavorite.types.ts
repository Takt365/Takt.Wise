// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/news-center
// 文件名称：newsFavorite.types.ts
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
 * 新闻中心收藏记录实体
 * 对应前端 TaktNewsFavoriteDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 NewsFavorite
 * @description 对应后端 TaktNewsFavoriteDto
 */
export interface NewsFavorite extends CompanyDtoBase {
  /**
   * NewsFavoriteID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  newsFavoriteId: string;

  /**
   * 新闻 ID（选项 TaktNews/options；DictValue=Id）
   */
  newsId: string;

  /**
   * 新闻 名称（填充字段）
   */
  newsName?: string;

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
   * 收藏时间
   */
  favoriteTime: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 新闻（主表） （主表：TaktNews）
   */
  news?: News;

}


/**
 * NewsFavorite 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NewsFavoriteQuery
 * @description 对应后端 TaktNewsFavoriteQueryDto
 */
export interface NewsFavoriteQuery extends TaktPagedQuery {
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
   * 新闻 ID（选项 TaktNews/options；DictValue=Id）
   */
  newsId?: string;

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
   * 收藏时间（范围查询-开始）
   */
  favoriteTimeStart?: string;

  /**
   * 收藏时间（范围查询-结束）
   */
  favoriteTimeEnd?: string;

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
 * 创建NewsFavorite DTO
 * 对应前端 NewsFavoriteCreate
 * @description 对应后端 TaktNewsFavoriteCreateDto
 */
export interface NewsFavoriteCreate {
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
   * 新闻 ID（选项 TaktNews/options；DictValue=Id）
   */
  newsId: string;

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
   * 收藏时间
   */
  favoriteTime: string;

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
 * 更新NewsFavorite DTO
 * 继承 TaktNewsFavoriteCreateDto，添加 NewsFavoriteId 字段
 * 对应前端 NewsFavoriteUpdate
 * @description 对应后端 TaktNewsFavoriteUpdateDto
 */
export interface NewsFavoriteUpdate extends NewsFavoriteCreate {
  /**
   * NewsFavoriteID（标识要更新的实体）
   */
  newsFavoriteId: string;

}


/**
 * NewsFavorite 作废/撤销作废 DTO
 * 对应前端 NewsFavoriteObsolete
 * @description 对应后端 TaktNewsFavoriteObsoleteDto
 */
export interface NewsFavoriteObsolete {
  /**
   * NewsFavoriteID
   */
  newsFavoriteId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * NewsFavorite 导入模板行 DTO
 * 对应前端 NewsFavoriteTemplate
 * @description 对应后端 TaktNewsFavoriteTemplateDto
 */
export interface NewsFavoriteTemplate {
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
   * 新闻 ID（选项 TaktNews/options；DictValue=Id）
   */
  newsId?: string;

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
   * 收藏时间
   */
  favoriteTime?: string;

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
 * NewsFavorite 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NewsFavoriteImport
 * @description 对应后端 TaktNewsFavoriteImportDto
 */
export interface NewsFavoriteImport {
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
   * 新闻 ID（选项 TaktNews/options；DictValue=Id）
   */
  newsId?: string;

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
   * 收藏时间
   */
  favoriteTime?: string;

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
 * NewsFavorite 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NewsFavoriteExport
 * @description 对应后端 TaktNewsFavoriteExportDto
 */
export interface NewsFavoriteExport {
  /**
   * NewsFavoriteID
   */
  newsFavoriteId: string;

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
   * 新闻 ID（选项 TaktNews/options；DictValue=Id）
   */
  newsId: string;

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
   * 收藏时间
   */
  favoriteTime: string;

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

