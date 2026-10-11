// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopContent.types.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 多语言正文实体
 * 对应前端 TaktSopContentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopContent
 * @description 对应后端 TaktSopContentDto
 */
export interface SopContent extends CompanyDtoBase {
  /**
   * SopContentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopContentId: string;

  /**
   * 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId: string;

  /**
   * 版本 名称（填充字段）
   */
  revisionName?: string;

  /**
   * SOP 主档 ID（冗余，序列化为 string 以避免 Javascript 精度问题）
   */
  sopId: string;

  /**
   * SOP 主档 名称（填充字段）
   */
  sopName?: string;

  /**
   * 正文语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  contentLang: string;

  /**
   * 正文标题
   */
  contentTitle?: string;

  /**
   * 版本 （主表：TaktSopRevision）
   */
  revision?: SopRevision;

  /**
   * 工步列表 （子表：TaktSopStep）
   */
  steps?: SopStep[];

}


/**
 * SopContent 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopContentQuery
 * @description 对应后端 TaktSopContentQueryDto
 */
export interface SopContentQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId?: string;

  /**
   * SOP 主档 ID（冗余，序列化为 string 以避免 Javascript 精度问题）
   */
  sopId?: string;

  /**
   * 正文语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  contentLang?: string;

  /**
   * 正文标题
   */
  contentTitle?: string;

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
 * 创建SopContent DTO
 * 对应前端 SopContentCreate
 * @description 对应后端 TaktSopContentCreateDto
 */
export interface SopContentCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId: string;

  /**
   * SOP 主档 ID（冗余，序列化为 string 以避免 Javascript 精度问题）
   */
  sopId: string;

  /**
   * 正文语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  contentLang: string;

  /**
   * 正文标题
   */
  contentTitle?: string;

  /**
   * 工步列表（子表，级联保存）
   */
  steps?: SopStepCreate[];

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
 * 更新SopContent DTO
 * 继承 TaktSopContentCreateDto，添加 SopContentId 字段
 * 对应前端 SopContentUpdate
 * @description 对应后端 TaktSopContentUpdateDto
 */
export interface SopContentUpdate extends SopContentCreate {
  /**
   * SopContentID（标识要更新的实体）
   */
  sopContentId: string;

}


/**
 * SopContent 导入模板行 DTO
 * 对应前端 SopContentTemplate
 * @description 对应后端 TaktSopContentTemplateDto
 */
export interface SopContentTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId?: string;

  /**
   * SOP 主档 ID（冗余，序列化为 string 以避免 Javascript 精度问题）
   */
  sopId?: string;

  /**
   * 正文语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  contentLang?: string;

  /**
   * 正文标题
   */
  contentTitle?: string;

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
 * SopContent 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopContentImport
 * @description 对应后端 TaktSopContentImportDto
 */
export interface SopContentImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId?: string;

  /**
   * SOP 主档 ID（冗余，序列化为 string 以避免 Javascript 精度问题）
   */
  sopId?: string;

  /**
   * 正文语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  contentLang?: string;

  /**
   * 正文标题
   */
  contentTitle?: string;

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
 * SopContent 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopContentExport
 * @description 对应后端 TaktSopContentExportDto
 */
export interface SopContentExport {
  /**
   * SopContentID
   */
  sopContentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId: string;

  /**
   * SOP 主档 ID（冗余，序列化为 string 以避免 Javascript 精度问题）
   */
  sopId: string;

  /**
   * 正文语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  contentLang: string;

  /**
   * 正文标题
   */
  contentTitle?: string;

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

