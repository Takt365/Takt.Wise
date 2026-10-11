// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：culture.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * 区域文化实体 定义系统支持的多语言区域文化，如：zh-CN（简体中文）、en-US（美式英文）、ja-JP（日文）等 租户级实体：区域文化定义在租户内共享，不需要公司隔离
 * 对应前端 TaktCultureDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 Culture
 * @description 对应后端 TaktCultureDto
 */
export interface Culture extends TenantCoreDtoBase {
  /**
   * CultureID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  cultureId: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 本地化名称（用该语言显示的自身名称，如：中文、English）
   */
  nativeName: string;

  /**
   * 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
   */
  icon?: string;

  /**
   * 默认语言（字典 sys_yes_no；1=是 0=否）
   */
  isDefault: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 翻译列表（一对多关联） （子表：TaktTranslation）
   */
  translationList?: Translation[];

}


/**
 * Culture 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CultureQuery
 * @description 对应后端 TaktCultureQueryDto
 */
export interface CultureQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 本地化名称（用该语言显示的自身名称，如：中文、English）
   */
  nativeName?: string;

  /**
   * 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
   */
  icon?: string;

  /**
   * 默认语言（字典 sys_yes_no；1=是 0=否）
   */
  isDefault?: number;

  /**
   * 排序号
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
 * 创建Culture DTO
 * 对应前端 CultureCreate
 * @description 对应后端 TaktCultureCreateDto
 */
export interface CultureCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 本地化名称（用该语言显示的自身名称，如：中文、English）
   */
  nativeName: string;

  /**
   * 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
   */
  icon?: string;

  /**
   * 默认语言（字典 sys_yes_no；1=是 0=否）
   */
  isDefault: number;

  /**
   * 翻译列表（一对多关联）（子表，级联保存）
   */
  translationList?: TranslationCreate[];

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
 * 更新Culture DTO
 * 继承 TaktCultureCreateDto，添加 CultureId 字段
 * 对应前端 CultureUpdate
 * @description 对应后端 TaktCultureUpdateDto
 */
export interface CultureUpdate extends CultureCreate {
  /**
   * CultureID（标识要更新的实体）
   */
  cultureId: string;

  /**
   * 翻译列表（一对多关联）（子表，级联保存）
   */
  translationList?: any;

}


/**
 * Culture 排序更新 DTO
 * 对应前端 CultureSort
 * @description 对应后端 TaktCultureSortDto
 */
export interface CultureSort {
  /**
   * CultureID
   */
  cultureId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Culture 导入模板行 DTO
 * 对应前端 CultureTemplate
 * @description 对应后端 TaktCultureTemplateDto
 */
export interface CultureTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 本地化名称（用该语言显示的自身名称，如：中文、English）
   */
  nativeName?: string;

  /**
   * 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
   */
  icon?: string;

  /**
   * 默认语言（字典 sys_yes_no；1=是 0=否）
   */
  isDefault?: number;

  /**
   * 翻译列表（一对多关联）（子表，级联保存）
   */
  translationList?: TranslationCreate[];

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
 * Culture 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CultureImport
 * @description 对应后端 TaktCultureImportDto
 */
export interface CultureImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 本地化名称（用该语言显示的自身名称，如：中文、English）
   */
  nativeName?: string;

  /**
   * 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
   */
  icon?: string;

  /**
   * 默认语言（字典 sys_yes_no；1=是 0=否）
   */
  isDefault?: number;

  /**
   * 翻译列表（一对多关联）（子表，级联保存）
   */
  translationList?: TranslationCreate[];

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
 * Culture 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CultureExport
 * @description 对应后端 TaktCultureExportDto
 */
export interface CultureExport {
  /**
   * CultureID
   */
  cultureId: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 本地化名称（用该语言显示的自身名称，如：中文、English）
   */
  nativeName: string;

  /**
   * 语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）
   */
  icon?: string;

  /**
   * 默认语言（字典 sys_yes_no；1=是 0=否）
   */
  isDefault: number;

  /**
   * 排序号
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

