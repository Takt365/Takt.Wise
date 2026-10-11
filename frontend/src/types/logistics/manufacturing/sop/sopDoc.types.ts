// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopDoc.types.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 文档头实体。FlowInstanceId 由业务在发起流程后写入；审批状态见 ApprovalStatus。
 * 对应前端 TaktSopDocDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 SopDoc
 * @description 对应后端 TaktSopDocDto
 */
export interface SopDoc extends ApprovalDtoBase {
  /**
   * SopDocID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopDocId: string;

  /**
   * SOP 编码
   */
  sopCode: string;

  /**
   * SOP 名称
   */
  sopName: string;

  /**
   * 产品/物料编码
   */
  materialCode: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 工艺路线明细 名称（填充字段）
   */
  routingItemName?: string;

  /**
   * 工位 ID（可选，工位级 ESOP；序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 工位 名称（填充字段）
   */
  workstationName?: string;

  /**
   * 当前生效版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  currentRevisionId?: string;

  /**
   * 当前生效版本 名称（填充字段）
   */
  currentRevisionName?: string;

  /**
   * 默认语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  defaultLang: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus: number;

  /**
   * 工序 （主表：TaktRoutingItem）
   */
  routingItem?: RoutingItem;

  /**
   * 工位 （主表：TaktSopWorkstation）
   */
  workstation?: SopWorkstation;

  /**
   * 版本列表 （子表：TaktSopRevision）
   */
  revisions?: SopRevision[];

}


/**
 * SopDoc 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopDocQuery
 * @description 对应后端 TaktSopDocQueryDto
 */
export interface SopDocQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * SOP 编码
   */
  sopCode?: string;

  /**
   * SOP 名称
   */
  sopName?: string;

  /**
   * 产品/物料编码
   */
  materialCode?: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId?: string;

  /**
   * 工位 ID（可选，工位级 ESOP；序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 当前生效版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  currentRevisionId?: string;

  /**
   * 默认语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  defaultLang?: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus?: number;

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
 * 创建SopDoc DTO
 * 对应前端 SopDocCreate
 * @description 对应后端 TaktSopDocCreateDto
 */
export interface SopDocCreate {
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
   * SOP 编码
   */
  sopCode: string;

  /**
   * SOP 名称
   */
  sopName: string;

  /**
   * 产品/物料编码
   */
  materialCode: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 工位 ID（可选，工位级 ESOP；序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 当前生效版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  currentRevisionId?: string;

  /**
   * 默认语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  defaultLang: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus: number;

  /**
   * 版本列表（子表，级联保存）
   */
  revisions?: SopRevisionCreate[];

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
 * 更新SopDoc DTO
 * 继承 TaktSopDocCreateDto，添加 SopDocId 字段
 * 对应前端 SopDocUpdate
 * @description 对应后端 TaktSopDocUpdateDto
 */
export interface SopDocUpdate extends SopDocCreate {
  /**
   * SopDocID（标识要更新的实体）
   */
  sopDocId: string;

}


/**
 * SopDoc 状态更新 DTO
 * 对应前端 SopDocStatus
 * @description 对应后端 TaktSopDocStatusDto
 */
export interface SopDocStatus {
  /**
   * SopDocID
   */
  sopDocId: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus: number;

}


/**
 * SopDoc 导入模板行 DTO
 * 对应前端 SopDocTemplate
 * @description 对应后端 TaktSopDocTemplateDto
 */
export interface SopDocTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * SOP 编码
   */
  sopCode?: string;

  /**
   * SOP 名称
   */
  sopName?: string;

  /**
   * 产品/物料编码
   */
  materialCode?: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId?: string;

  /**
   * 工位 ID（可选，工位级 ESOP；序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 当前生效版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  currentRevisionId?: string;

  /**
   * 默认语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  defaultLang?: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus?: number;

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
 * SopDoc 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopDocImport
 * @description 对应后端 TaktSopDocImportDto
 */
export interface SopDocImport {
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
   * SOP 编码
   */
  sopCode?: string;

  /**
   * SOP 名称
   */
  sopName?: string;

  /**
   * 产品/物料编码
   */
  materialCode?: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId?: string;

  /**
   * 工位 ID（可选，工位级 ESOP；序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 当前生效版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  currentRevisionId?: string;

  /**
   * 默认语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  defaultLang?: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus?: number;

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
 * SopDoc 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopDocExport
 * @description 对应后端 TaktSopDocExportDto
 */
export interface SopDocExport {
  /**
   * SopDocID
   */
  sopDocId: string;

  /**
   * SOP 编码
   */
  sopCode: string;

  /**
   * SOP 名称
   */
  sopName: string;

  /**
   * 产品/物料编码
   */
  materialCode: string;

  /**
   * 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemId: string;

  /**
   * 工位 ID（可选，工位级 ESOP；序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 当前生效版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  currentRevisionId?: string;

  /**
   * 默认语言（zh-CN 简体 / en-US 英文 / ja-JP 日文 / zh-HK 香港繁体；与 TaktCulture.CultureCode 一致）
   */
  defaultLang: string;

  /**
   * 状态（字典 sys_normal_disable，0=停用，1=正常）
   */
  sopStatus: number;

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

