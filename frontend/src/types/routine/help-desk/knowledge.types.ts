// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：knowledge.types.ts
// 创建时间：2026-07-09
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
 * 服务台知识库实体
 * 对应前端 TaktKnowledgeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Knowledge
 * @description 对应后端 TaktKnowledgeDto
 */
export interface Knowledge extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 知识标题
   */
  knowledgeTitle?: string;

  /**
   * 知识内容（富文本/HTML）
   */
  knowledgeContent?: string;

  /**
   * 知识摘要（简短描述，列表/搜索展示）
   */
  knowledgeSummary?: string;

  /**
   * 分类编码（如 faq/guide 等）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  knowledgeTags?: string;

  /**
   * 浏览次数
   */
  knowledgeViewCount?: number;

  /**
   * 有用评价数
   */
  helpfulCount?: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount?: number;

  /**
   * 是否已发布（0=否，1=是）
   */
  knowledgeIsPublished?: number;

  /**
   * 版本号
   */
  version?: number;

  /**
   * 发布时间
   */
  publishedAt?: string;

  /**
   * 最后修订时间
   */
  revisedAt?: string;

  /**
   * 知识状态（0=草稿，1=已发布，2=已下架）
   */
  knowledgeStatus?: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

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
 * Knowledge 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 KnowledgeExport
 * @description 对应后端 TaktKnowledgeExportDto
 */
export interface KnowledgeExport {
  /**
   * KnowledgeID
   */
  knowledgeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 知识标题
   */
  knowledgeTitle: string;

  /**
   * 知识内容（富文本/HTML）
   */
  knowledgeContent?: string;

  /**
   * 知识摘要（简短描述，列表/搜索展示）
   */
  knowledgeSummary?: string;

  /**
   * 分类编码（如 faq/guide 等）
   */
  categoryCode?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  knowledgeTags?: string;

  /**
   * 浏览次数
   */
  knowledgeViewCount: number;

  /**
   * 有用评价数
   */
  helpfulCount: number;

  /**
   * 无帮助评价数
   */
  unhelpfulCount: number;

  /**
   * 是否已发布（0=否，1=是）
   */
  knowledgeIsPublished: number;

  /**
   * 版本号
   */
  version: number;

  /**
   * 发布时间
   */
  publishedAt?: string;

  /**
   * 最后修订时间
   */
  revisedAt?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 知识状态（0=草稿，1=已发布，2=已下架）
   */
  knowledgeStatus: number;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

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

