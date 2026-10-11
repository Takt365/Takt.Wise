// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：ack.types.ts
// 创建时间：2026-06-30
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
 * SOP 确认实体
 * 对应前端 TaktSopAckDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopAck
 * @description 对应后端 TaktSopAckDto
 */
export interface SopAck extends CompanyDtoBase {
  /**
   * SopAckID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopAckId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * SOP 主档 ID（关联 TaktSopDoc.Id，选项 TaktSopDocs/options）
   */
  sopId: string;

  /**
   * SOP 主档 名称（填充字段）
   */
  sopName?: string;

  /**
   * SOP 版本 ID（关联 TaktSopRevision.Id，选项 TaktSopRevisions/options）
   */
  revisionId: string;

  /**
   * SOP 版本 名称（填充字段）
   */
  revisionName?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 工位 名称（填充字段）
   */
  workstationName?: string;

  /**
   * 确认人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  acknowledgedBy: string;

  /**
   * 确认时间
   */
  acknowledgedAt: string;

  /**
   * 确认意见
   */
  ackComment?: string;

  /**
   * SOP 主档 （主表：TaktSopDoc）
   */
  sopDoc?: SopDoc;

  /**
   * SOP 版本 （主表：TaktSopRevision）
   */
  revision?: SopRevision;

  /**
   * 工位 （主表：TaktSopWorkstation）
   */
  workstation?: SopWorkstation;

}


/**
 * SopAck 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopAckQuery
 * @description 对应后端 TaktSopAckQueryDto
 */
export interface SopAckQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * SOP 主档 ID（关联 TaktSopDoc.Id，选项 TaktSopDocs/options）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（关联 TaktSopRevision.Id，选项 TaktSopRevisions/options）
   */
  revisionId?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 确认人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  acknowledgedBy?: string;

  /**
   * 确认时间（范围查询-开始）
   */
  acknowledgedAtStart?: string;

  /**
   * 确认时间（范围查询-结束）
   */
  acknowledgedAtEnd?: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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
 * 创建SopAck DTO
 * 对应前端 SopAckCreate
 * @description 对应后端 TaktSopAckCreateDto
 */
export interface SopAckCreate {
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
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * SOP 主档 ID（关联 TaktSopDoc.Id，选项 TaktSopDocs/options）
   */
  sopId: string;

  /**
   * SOP 版本 ID（关联 TaktSopRevision.Id，选项 TaktSopRevisions/options）
   */
  revisionId: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 确认人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  acknowledgedBy: string;

  /**
   * 确认时间
   */
  acknowledgedAt: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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
 * 更新SopAck DTO
 * 继承 TaktSopAckCreateDto，添加 SopAckId 字段
 * 对应前端 SopAckUpdate
 * @description 对应后端 TaktSopAckUpdateDto
 */
export interface SopAckUpdate extends SopAckCreate {
  /**
   * SopAckID（标识要更新的实体）
   */
  sopAckId: string;

}


/**
 * SopAck 导入模板行 DTO
 * 对应前端 SopAckTemplate
 * @description 对应后端 TaktSopAckTemplateDto
 */
export interface SopAckTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * SOP 主档 ID（关联 TaktSopDoc.Id，选项 TaktSopDocs/options）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（关联 TaktSopRevision.Id，选项 TaktSopRevisions/options）
   */
  revisionId?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 确认人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  acknowledgedBy?: string;

  /**
   * 确认时间
   */
  acknowledgedAt?: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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
 * SopAck 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopAckImport
 * @description 对应后端 TaktSopAckImportDto
 */
export interface SopAckImport {
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
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * SOP 主档 ID（关联 TaktSopDoc.Id，选项 TaktSopDocs/options）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（关联 TaktSopRevision.Id，选项 TaktSopRevisions/options）
   */
  revisionId?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 确认人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  acknowledgedBy?: string;

  /**
   * 确认时间
   */
  acknowledgedAt?: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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
 * SopAck 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopAckExport
 * @description 对应后端 TaktSopAckExportDto
 */
export interface SopAckExport {
  /**
   * SopAckID
   */
  sopAckId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * SOP 主档 ID（关联 TaktSopDoc.Id，选项 TaktSopDocs/options）
   */
  sopId: string;

  /**
   * SOP 版本 ID（关联 TaktSopRevision.Id，选项 TaktSopRevisions/options）
   */
  revisionId: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 确认人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  acknowledgedBy: string;

  /**
   * 确认时间
   */
  acknowledgedAt: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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

