// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：vocabulary.types.ts
// 创建时间：2026-06-24
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
 * 敏感词实体（租户内共享，供新闻、公告评论等模块引用）
 * 对应前端 TaktVocabularyDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 Vocabulary
 * @description 对应后端 TaktVocabularyDto
 */
export interface Vocabulary extends TenantCoreDtoBase {
  /**
   * VocabularyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  vocabularyId: string;

  /**
   * 敏感词文本（租户内唯一）
   */
  wordText: string;

  /**
   * 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
   */
  wordCategory: number;

  /**
   * 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
   */
  filterLevel: number;

  /**
   * 替换文本（默认 *Takt*）
   */
  replaceText: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus: number;

}

/**
 * Vocabulary 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 VocabularyQuery
 * @description 对应后端 TaktVocabularyQueryDto
 */
export interface VocabularyQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 敏感词文本（租户内唯一）
   */
  wordText?: string;

  /**
   * 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
   */
  wordCategory?: number;

  /**
   * 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
   */
  filterLevel?: number;

  /**
   * 替换文本（默认 *Takt*）
   */
  replaceText?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus?: number;

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
 * 创建Vocabulary DTO
 * 对应前端 VocabularyCreate
 * @description 对应后端 TaktVocabularyCreateDto
 */
export interface VocabularyCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 敏感词文本（租户内唯一）
   */
  wordText: string;

  /**
   * 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
   */
  wordCategory: number;

  /**
   * 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
   */
  filterLevel: number;

  /**
   * 替换文本（默认 *Takt*）
   */
  replaceText: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus: number;

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
 * 更新Vocabulary DTO
 * 继承 TaktVocabularyCreateDto，添加 VocabularyId 字段
 * 对应前端 VocabularyUpdate
 * @description 对应后端 TaktVocabularyUpdateDto
 */
export interface VocabularyUpdate extends VocabularyCreate {
  /**
   * VocabularyID（标识要更新的实体）
   */
  vocabularyId: string;

}

/**
 * Vocabulary 状态更新 DTO
 * 对应前端 VocabularyStatus
 * @description 对应后端 TaktVocabularyStatusDto
 */
export interface VocabularyStatus {
  /**
   * VocabularyID
   */
  vocabularyId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus: number;

}

/**
 * Vocabulary 导入模板行 DTO
 * 对应前端 VocabularyTemplate
 * @description 对应后端 TaktVocabularyTemplateDto
 */
export interface VocabularyTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 敏感词文本（租户内唯一）
   */
  wordText?: string;

  /**
   * 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
   */
  wordCategory?: number;

  /**
   * 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
   */
  filterLevel?: number;

  /**
   * 替换文本（默认 *Takt*）
   */
  replaceText?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus?: number;

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
 * Vocabulary 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 VocabularyImport
 * @description 对应后端 TaktVocabularyImportDto
 */
export interface VocabularyImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 敏感词文本（租户内唯一）
   */
  wordText?: string;

  /**
   * 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
   */
  wordCategory?: number;

  /**
   * 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
   */
  filterLevel?: number;

  /**
   * 替换文本（默认 *Takt*）
   */
  replaceText?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus?: number;

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
 * Vocabulary 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VocabularyExport
 * @description 对应后端 TaktVocabularyExportDto
 */
export interface VocabularyExport {
  /**
   * VocabularyID
   */
  vocabularyId: string;

  /**
   * 敏感词文本（租户内唯一）
   */
  wordText: string;

  /**
   * 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
   */
  wordCategory: number;

  /**
   * 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
   */
  filterLevel: number;

  /**
   * 替换文本（默认 *Takt*）
   */
  replaceText: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  vocabularyStatus: number;

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

