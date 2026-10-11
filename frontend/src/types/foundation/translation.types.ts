// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：translation.types.ts
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
 * 指定文化下前端翻译键值包（对应后端 TaktTranslationMessagesDto）
 * @description GET TaktTranslations/messages
 */
export interface TranslationMessages {
  /**
   * 区域文化编码（BCP47，如 zh-CN、en-US）
   */
  cultureCode: string;

  /**
   * 扁平翻译：I18nKey → TranslationText（仅 resource_type=frontend）
   */
  messages: Record<string, string>;
}

/**
 * 翻译实体 存储系统界面的多语言翻译文本 租户级实体：翻译数据在租户内共享，不需要公司隔离
 * 对应前端 TaktTranslationDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 Translation
 * @description 对应后端 TaktTranslationDto
 */
export interface Translation extends TenantCoreDtoBase {
  /**
   * TranslationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  translationId: string;

  /**
   * 区域文化（选项 TaktCultures/options；DictValue=Id）
   */
  cultureId: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 文化名称（填充字段）
   */
  cultureName?: string;

  /**
   * 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
   */
  i18nKey: string;

  /**
   * 翻译文本（该语言下的显示文本）
   */
  translationText: string;

  /**
   * 资源分组（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  resourceGroup: string;

  /**
   * 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  resourceType: string;

  /**
   * 上下文注释（帮助翻译人员理解使用场景）
   */
  contextNote?: string;

  /**
   * 文化（多对一关联） （主表：TaktCulture）
   */
  culture?: Culture;

}


/**
 * Translation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TranslationQuery
 * @description 对应后端 TaktTranslationQueryDto
 */
export interface TranslationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 文化ID（关联 TaktCulture.Id）
   */
  cultureId?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
   */
  i18nKey?: string;

  /**
   * 翻译文本（该语言下的显示文本）
   */
  translationText?: string;

  /**
   * 资源分组（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  resourceGroup?: string;

  /**
   * 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  resourceType?: string;

  /**
   * 上下文注释（帮助翻译人员理解使用场景）
   */
  contextNote?: string;

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
 * 创建Translation DTO
 * 对应前端 TranslationCreate
 * @description 对应后端 TaktTranslationCreateDto
 */
export interface TranslationCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 文化ID（关联 TaktCulture.Id）
   */
  cultureId: string;

  /**
   * 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
   */
  i18nKey: string;

  /**
   * 翻译文本（该语言下的显示文本）
   */
  translationText: string;

  /**
   * 资源分组（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  resourceGroup: string;

  /**
   * 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  resourceType: string;

  /**
   * 上下文注释（帮助翻译人员理解使用场景）
   */
  contextNote?: string;

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
 * 更新Translation DTO
 * 继承 TaktTranslationCreateDto，添加 TranslationId 字段
 * 对应前端 TranslationUpdate
 * @description 对应后端 TaktTranslationUpdateDto
 */
export interface TranslationUpdate extends TranslationCreate {
  /**
   * TranslationID（标识要更新的实体）
   */
  translationId: string;

}


/**
 * Translation 导入模板行 DTO
 * 对应前端 TranslationTemplate
 * @description 对应后端 TaktTranslationTemplateDto
 */
export interface TranslationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 文化ID（关联 TaktCulture.Id）
   */
  cultureId?: string;

  /**
   * 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
   */
  i18nKey?: string;

  /**
   * 翻译文本（该语言下的显示文本）
   */
  translationText?: string;

  /**
   * 资源分组（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  resourceGroup?: string;

  /**
   * 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  resourceType?: string;

  /**
   * 上下文注释（帮助翻译人员理解使用场景）
   */
  contextNote?: string;

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
 * Translation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TranslationImport
 * @description 对应后端 TaktTranslationImportDto
 */
export interface TranslationImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 文化ID（关联 TaktCulture.Id）
   */
  cultureId?: string;

  /**
   * 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
   */
  i18nKey?: string;

  /**
   * 翻译文本（该语言下的显示文本）
   */
  translationText?: string;

  /**
   * 资源分组（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  resourceGroup?: string;

  /**
   * 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  resourceType?: string;

  /**
   * 上下文注释（帮助翻译人员理解使用场景）
   */
  contextNote?: string;

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
 * Translation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TranslationExport
 * @description 对应后端 TaktTranslationExportDto
 */
export interface TranslationExport {
  /**
   * TranslationID
   */
  translationId: string;

  /**
   * 区域文化（选项 TaktCultures/options；DictValue=Id）
   */
  cultureId: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；如 common.confirm）
   */
  i18nKey: string;

  /**
   * 翻译文本（该语言下的显示文本）
   */
  translationText: string;

  /**
   * 资源分组（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  resourceGroup: string;

  /**
   * 资源类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  resourceType: string;

  /**
   * 上下文注释（帮助翻译人员理解使用场景）
   */
  contextNote?: string;

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
 * Translation转置行 DTO
 * 对应前端 TranslationTransposed
 * @description 对应后端 TaktTranslationTransposedDto
 */
export interface TranslationTransposed {
  /**
   * 翻译ID（分组内首条记录 Id，新建为 0）
   */
  translationId: string;

  /**
   * 翻译键（转置行键）
   */
  i18nKey: string;

  /**
   * 资源分组（关联 TaktMenu.Id）
   */
  resourceGroup: string;

  /**
   * 资源类别（字典 sys_resource_type）
   */
  resourceType: string;

  /**
   * 上下文注释
   */
  contextNote?: string;

  /**
   * 各语言文本；键为 CultureCode（如 zh-CN、en-US），值对应该语言下的显示文本
   */
  translations: Record<string, string>;

}


/**
 * Translation转置分页查询 DTO
 * 对应前端 TranslationTransposedQuery
 * @description 对应后端 TaktTranslationTransposedQueryDto
 */
export interface TranslationTransposedQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 文化ID
   */
  cultureId?: string;

  /**
   * 文化编码
   */
  cultureCode?: string;

  /**
   * 翻译键
   */
  i18nKey?: string;

  /**
   * 翻译文本
   */
  translationText?: string;

  /**
   * 资源分组
   */
  resourceGroup?: string;

  /**
   * 资源类别
   */
  resourceType?: string;

  /**
   * 上下文注释
   */
  contextNote?: string;

}


/**
 * Translation转置分页结果 DTO（含语言列顺序）
 * 对应前端 TranslationTransposedResult
 * @description 对应后端 TaktTranslationTransposedResultDto
 */
export interface TranslationTransposedResult {
  /**
   * 分页数据
   */
  paged: number;

  /**
   * 语言列顺序（表头从左到右），如 zh-CN、en-US 等
   */
  cultureCodeOrder: string[];

}


/**
 * Translation转置批量保存 DTO
 * 对应前端 TranslationTransposedBatch
 * @description 对应后端 TaktTranslationTransposedBatchDto
 */
export interface TranslationTransposedBatch {
  /**
   * 转置行数据
   */
  rows: TranslationTransposed[];

}

