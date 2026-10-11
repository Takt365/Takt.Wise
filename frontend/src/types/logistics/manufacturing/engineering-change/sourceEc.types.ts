// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：sourceEc.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 设变来源明细列表
 * 对应前端 TaktSourceEcDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SourceEc
 * @description 对应后端 TaktSourceEcDto
 */
export interface SourceEc extends CompanyDtoBase {
  /**
   * SourceEcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sourceEcId: string;

  /**
   * 设变号码
   */
  sourceEcCode: string;

  /**
   * 机种
   */
  sourceModel: string;

  /**
   * 标题
   */
  sourceTitle: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus: string;

  /**
   * 发行日期
   */
  sourceIssueDate: string;

  /**
   * TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
   */
  sourceTcjOwner?: string;

  /**
   * TCJ依赖
   */
  sourceTcjDependency?: string;

  /**
   * 设变会议
   */
  sourceEcMeeting?: string;

  /**
   * PP番号
   */
  sourcePpCode?: string;

  /**
   * 技联书
   */
  sourceTechnicalNoticeCode?: string;

  /**
   * 实施
   */
  sourceImplementation?: string;

  /**
   * 主变更理由
   */
  sourceMainChangeReason?: string;

  /**
   * 次变更理由
   */
  sourceSecondaryChangeReason?: string;

  /**
   * 安规
   */
  sourceSafetyRegulation?: string;

  /**
   * 进行状况
   */
  sourceProgressStatus?: string;

  /**
   * 机番管理
   */
  sourceSerialNumberControl?: string;

  /**
   * 客户承认
   */
  sourceCustomerApproval?: string;

  /**
   * 服务手册订正
   */
  sourceServiceManualRevision?: string;

  /**
   * 用户手册订正
   */
  sourceUserManualRevision?: string;

  /**
   * 宣传手册订正
   */
  sourcePromotionManualRevision?: string;

  /**
   * 标准书订正
   */
  sourceStandardDocumentRevision?: string;

  /**
   * 情报发行
   */
  sourceInformationRelease?: string;

  /**
   * 成本变动
   */
  sourceCostChange?: string;

  /**
   * 单位成本
   */
  sourceUnitCost: number;

  /**
   * 模具改修费
   */
  sourceMoldModificationCost: number;

  /**
   * 相关图纸
   */
  sourceRelatedDrawing?: string;

  /**
   * 设变内容
   */
  sourceEcContent: string;

  /**
   * 设变来源明细列表 （子表：TaktSourceEcDetail）
   */
  sourceEcDetails?: SourceEcDetail[];

}


/**
 * SourceEc 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SourceEcQuery
 * @description 对应后端 TaktSourceEcQueryDto
 */
export interface SourceEcQuery extends TaktPagedQuery {
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
   * 设变号码
   */
  sourceEcCode?: string;

  /**
   * 机种
   */
  sourceModel?: string;

  /**
   * 标题
   */
  sourceTitle?: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus?: string;

  /**
   * 发行日期（范围查询-开始）
   */
  sourceIssueDateStart?: string;

  /**
   * 发行日期（范围查询-结束）
   */
  sourceIssueDateEnd?: string;

  /**
   * TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
   */
  sourceTcjOwner?: string;

  /**
   * TCJ依赖
   */
  sourceTcjDependency?: string;

  /**
   * 设变会议
   */
  sourceEcMeeting?: string;

  /**
   * PP番号
   */
  sourcePpCode?: string;

  /**
   * 技联书
   */
  sourceTechnicalNoticeCode?: string;

  /**
   * 实施
   */
  sourceImplementation?: string;

  /**
   * 主变更理由
   */
  sourceMainChangeReason?: string;

  /**
   * 次变更理由
   */
  sourceSecondaryChangeReason?: string;

  /**
   * 安规
   */
  sourceSafetyRegulation?: string;

  /**
   * 进行状况
   */
  sourceProgressStatus?: string;

  /**
   * 机番管理
   */
  sourceSerialNumberControl?: string;

  /**
   * 客户承认
   */
  sourceCustomerApproval?: string;

  /**
   * 服务手册订正
   */
  sourceServiceManualRevision?: string;

  /**
   * 用户手册订正
   */
  sourceUserManualRevision?: string;

  /**
   * 宣传手册订正
   */
  sourcePromotionManualRevision?: string;

  /**
   * 标准书订正
   */
  sourceStandardDocumentRevision?: string;

  /**
   * 情报发行
   */
  sourceInformationRelease?: string;

  /**
   * 成本变动
   */
  sourceCostChange?: string;

  /**
   * 单位成本
   */
  sourceUnitCost?: number;

  /**
   * 模具改修费
   */
  sourceMoldModificationCost?: number;

  /**
   * 相关图纸
   */
  sourceRelatedDrawing?: string;

  /**
   * 设变内容
   */
  sourceEcContent?: string;

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
 * 创建SourceEc DTO
 * 对应前端 SourceEcCreate
 * @description 对应后端 TaktSourceEcCreateDto
 */
export interface SourceEcCreate {
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
   * 设变号码
   */
  sourceEcCode: string;

  /**
   * 机种
   */
  sourceModel: string;

  /**
   * 标题
   */
  sourceTitle: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus: string;

  /**
   * 发行日期
   */
  sourceIssueDate: string;

  /**
   * TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
   */
  sourceTcjOwner?: string;

  /**
   * TCJ依赖
   */
  sourceTcjDependency?: string;

  /**
   * 设变会议
   */
  sourceEcMeeting?: string;

  /**
   * PP番号
   */
  sourcePpCode?: string;

  /**
   * 技联书
   */
  sourceTechnicalNoticeCode?: string;

  /**
   * 实施
   */
  sourceImplementation?: string;

  /**
   * 主变更理由
   */
  sourceMainChangeReason?: string;

  /**
   * 次变更理由
   */
  sourceSecondaryChangeReason?: string;

  /**
   * 安规
   */
  sourceSafetyRegulation?: string;

  /**
   * 进行状况
   */
  sourceProgressStatus?: string;

  /**
   * 机番管理
   */
  sourceSerialNumberControl?: string;

  /**
   * 客户承认
   */
  sourceCustomerApproval?: string;

  /**
   * 服务手册订正
   */
  sourceServiceManualRevision?: string;

  /**
   * 用户手册订正
   */
  sourceUserManualRevision?: string;

  /**
   * 宣传手册订正
   */
  sourcePromotionManualRevision?: string;

  /**
   * 标准书订正
   */
  sourceStandardDocumentRevision?: string;

  /**
   * 情报发行
   */
  sourceInformationRelease?: string;

  /**
   * 成本变动
   */
  sourceCostChange?: string;

  /**
   * 单位成本
   */
  sourceUnitCost: number;

  /**
   * 模具改修费
   */
  sourceMoldModificationCost: number;

  /**
   * 相关图纸
   */
  sourceRelatedDrawing?: string;

  /**
   * 设变内容
   */
  sourceEcContent: string;

  /**
   * 设变来源明细列表（子表，级联保存）
   */
  sourceEcDetails?: SourceEcDetailCreate[];

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
 * 更新SourceEc DTO
 * 继承 TaktSourceEcCreateDto，添加 SourceEcId 字段
 * 对应前端 SourceEcUpdate
 * @description 对应后端 TaktSourceEcUpdateDto
 */
export interface SourceEcUpdate extends SourceEcCreate {
  /**
   * SourceEcID（标识要更新的实体）
   */
  sourceEcId: string;

  /**
   * 设变来源明细列表（子表，级联保存）
   */
  sourceEcDetails?: any;

}


/**
 * SourceEc 状态更新 DTO
 * 对应前端 SourceEcStatus
 * @description 对应后端 TaktSourceEcStatusDto
 */
export interface SourceEcStatus {
  /**
   * SourceEcID
   */
  sourceEcId: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus: string;

}


/**
 * SourceEc 导入模板行 DTO
 * 对应前端 SourceEcTemplate
 * @description 对应后端 TaktSourceEcTemplateDto
 */
export interface SourceEcTemplate {
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
   * 设变号码
   */
  sourceEcCode?: string;

  /**
   * 机种
   */
  sourceModel?: string;

  /**
   * 标题
   */
  sourceTitle?: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus?: string;

  /**
   * 发行日期
   */
  sourceIssueDate?: string;

  /**
   * TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
   */
  sourceTcjOwner?: string;

  /**
   * TCJ依赖
   */
  sourceTcjDependency?: string;

  /**
   * 设变会议
   */
  sourceEcMeeting?: string;

  /**
   * PP番号
   */
  sourcePpCode?: string;

  /**
   * 技联书
   */
  sourceTechnicalNoticeCode?: string;

  /**
   * 实施
   */
  sourceImplementation?: string;

  /**
   * 主变更理由
   */
  sourceMainChangeReason?: string;

  /**
   * 次变更理由
   */
  sourceSecondaryChangeReason?: string;

  /**
   * 安规
   */
  sourceSafetyRegulation?: string;

  /**
   * 进行状况
   */
  sourceProgressStatus?: string;

  /**
   * 机番管理
   */
  sourceSerialNumberControl?: string;

  /**
   * 客户承认
   */
  sourceCustomerApproval?: string;

  /**
   * 服务手册订正
   */
  sourceServiceManualRevision?: string;

  /**
   * 用户手册订正
   */
  sourceUserManualRevision?: string;

  /**
   * 宣传手册订正
   */
  sourcePromotionManualRevision?: string;

  /**
   * 标准书订正
   */
  sourceStandardDocumentRevision?: string;

  /**
   * 情报发行
   */
  sourceInformationRelease?: string;

  /**
   * 成本变动
   */
  sourceCostChange?: string;

  /**
   * 单位成本
   */
  sourceUnitCost?: number;

  /**
   * 模具改修费
   */
  sourceMoldModificationCost?: number;

  /**
   * 相关图纸
   */
  sourceRelatedDrawing?: string;

  /**
   * 设变内容
   */
  sourceEcContent?: string;

  /**
   * 设变来源明细列表（子表，级联保存）
   */
  sourceEcDetails?: SourceEcDetailCreate[];

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
 * SourceEc 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SourceEcImport
 * @description 对应后端 TaktSourceEcImportDto
 */
export interface SourceEcImport {
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
   * 设变号码
   */
  sourceEcCode?: string;

  /**
   * 机种
   */
  sourceModel?: string;

  /**
   * 标题
   */
  sourceTitle?: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus?: string;

  /**
   * 发行日期
   */
  sourceIssueDate?: string;

  /**
   * TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
   */
  sourceTcjOwner?: string;

  /**
   * TCJ依赖
   */
  sourceTcjDependency?: string;

  /**
   * 设变会议
   */
  sourceEcMeeting?: string;

  /**
   * PP番号
   */
  sourcePpCode?: string;

  /**
   * 技联书
   */
  sourceTechnicalNoticeCode?: string;

  /**
   * 实施
   */
  sourceImplementation?: string;

  /**
   * 主变更理由
   */
  sourceMainChangeReason?: string;

  /**
   * 次变更理由
   */
  sourceSecondaryChangeReason?: string;

  /**
   * 安规
   */
  sourceSafetyRegulation?: string;

  /**
   * 进行状况
   */
  sourceProgressStatus?: string;

  /**
   * 机番管理
   */
  sourceSerialNumberControl?: string;

  /**
   * 客户承认
   */
  sourceCustomerApproval?: string;

  /**
   * 服务手册订正
   */
  sourceServiceManualRevision?: string;

  /**
   * 用户手册订正
   */
  sourceUserManualRevision?: string;

  /**
   * 宣传手册订正
   */
  sourcePromotionManualRevision?: string;

  /**
   * 标准书订正
   */
  sourceStandardDocumentRevision?: string;

  /**
   * 情报发行
   */
  sourceInformationRelease?: string;

  /**
   * 成本变动
   */
  sourceCostChange?: string;

  /**
   * 单位成本
   */
  sourceUnitCost?: number;

  /**
   * 模具改修费
   */
  sourceMoldModificationCost?: number;

  /**
   * 相关图纸
   */
  sourceRelatedDrawing?: string;

  /**
   * 设变内容
   */
  sourceEcContent?: string;

  /**
   * 设变来源明细列表（子表，级联保存）
   */
  sourceEcDetails?: SourceEcDetailCreate[];

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
 * SourceEc 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SourceEcExport
 * @description 对应后端 TaktSourceEcExportDto
 */
export interface SourceEcExport {
  /**
   * SourceEcID
   */
  sourceEcId: string;

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
   * 设变号码
   */
  sourceEcCode: string;

  /**
   * 机种
   */
  sourceModel: string;

  /**
   * 标题
   */
  sourceTitle: string;

  /**
   * 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
   */
  sourceStatus: string;

  /**
   * 发行日期
   */
  sourceIssueDate: string;

  /**
   * TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
   */
  sourceTcjOwner?: string;

  /**
   * TCJ依赖
   */
  sourceTcjDependency?: string;

  /**
   * 设变会议
   */
  sourceEcMeeting?: string;

  /**
   * PP番号
   */
  sourcePpCode?: string;

  /**
   * 技联书
   */
  sourceTechnicalNoticeCode?: string;

  /**
   * 实施
   */
  sourceImplementation?: string;

  /**
   * 主变更理由
   */
  sourceMainChangeReason?: string;

  /**
   * 次变更理由
   */
  sourceSecondaryChangeReason?: string;

  /**
   * 安规
   */
  sourceSafetyRegulation?: string;

  /**
   * 进行状况
   */
  sourceProgressStatus?: string;

  /**
   * 机番管理
   */
  sourceSerialNumberControl?: string;

  /**
   * 客户承认
   */
  sourceCustomerApproval?: string;

  /**
   * 服务手册订正
   */
  sourceServiceManualRevision?: string;

  /**
   * 用户手册订正
   */
  sourceUserManualRevision?: string;

  /**
   * 宣传手册订正
   */
  sourcePromotionManualRevision?: string;

  /**
   * 标准书订正
   */
  sourceStandardDocumentRevision?: string;

  /**
   * 情报发行
   */
  sourceInformationRelease?: string;

  /**
   * 成本变动
   */
  sourceCostChange?: string;

  /**
   * 单位成本
   */
  sourceUnitCost: number;

  /**
   * 模具改修费
   */
  sourceMoldModificationCost: number;

  /**
   * 相关图纸
   */
  sourceRelatedDrawing?: string;

  /**
   * 设变内容
   */
  sourceEcContent: string;

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

