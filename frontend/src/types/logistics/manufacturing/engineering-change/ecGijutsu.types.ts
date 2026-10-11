// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecGijutsu.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：设变技术部门页面类型（对应后端 TaktEcGijutsu / TaktEcGijutsuDto；主键字段 ecGijutsuId）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';
import type { EcAttachment, EcAttachmentCreate } from './ec-attachment';
import type { EcDetail, EcDetailCreate } from './ec-detail';

/**
 * 设变技术课（ECN）主表实体，记录设变单号、工厂、发行/录入日期、标题、详情、设变担当、设变状态等；联络等文档见附件表 Attachments。
 * 对应前端 TaktEcGijutsuDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcGijutsu
 * @description 对应后端 TaktEcGijutsuDto
 */
export interface EcGijutsu extends CompanyDtoBase {

  /**
   * 设变单号（唯一）
   */
  ecCode: string;

  /**
   * 发行日期
   */
  ecIssueDate: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

  /**
   * 设变标题
   */
  ecTitle: string;

  /**
   * 设变内容
   */
  ecContent: string;

  /**
   * 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecLeader: string;

  /**
   * 损失金额
   */
  ecLossAmount: number;

  /**
   * 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
   */
  ecScope: number;

  /**
   * 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
   */
  ecDeptScope: number;

  /**
   * 录入日期
   */
  ecEntryDate: string;

  /**
   * 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
   */
  ecStatus: number;

  /**
   * 设变明细列表 （子表：TaktEcDetail）
   */
  ecDetails?: EcDetail[];

  /**
   * 设变附件列表（一个设变可对应多个附件） （子表：TaktEcAttachment）
   */
  attachments?: EcAttachment[];

}


/**
 * 设变主表弹窗 formData（编辑态子表可为 API 响应行；提交时由 ec-form getValues 补齐隔离字段）
 */
export type EcGijutsuFormData = Omit<
  Partial<EcGijutsuCreate & { ecGijutsuId?: string }>,
  'ecDetails' | 'attachments'
> & {
  ecDetails?: Array<Partial<EcDetailCreate & { ecDetailId?: string }>>;
  attachments?: Array<Partial<EcAttachmentCreate & { ecAttachmentId?: string }>>;
};


/**
 * EcGijutsu 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcQuery
 * @description 对应后端 TaktEcGijutsuQueryDto
 */
export interface EcGijutsuQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 设变单号（唯一）
   */
  ecCode?: string;

  /**
   * 发行日期（范围查询-开始）
   */
  ecIssueDateStart?: string;

  /**
   * 发行日期（范围查询-结束）
   */
  ecIssueDateEnd?: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus?: number;

  /**
   * 设变标题
   */
  ecTitle?: string;

  /**
   * 设变内容
   */
  ecContent?: string;

  /**
   * 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecLeader?: string;

  /**
   * 损失金额
   */
  ecLossAmount?: number;

  /**
   * 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
   */
  ecScope?: number;

  /**
   * 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
   */
  ecDeptScope?: number;

  /**
   * 录入日期（范围查询-开始）
   */
  ecEntryDateStart?: string;

  /**
   * 录入日期（范围查询-结束）
   */
  ecEntryDateEnd?: string;

  /**
   * 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
   */
  ecStatus?: number;

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
 * 创建EcGijutsu DTO
 * 对应前端 EcCreate
 * @description 对应后端 TaktEcGijutsuCreateDto
 */
export interface EcGijutsuCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 设变单号（唯一）
   */
  ecCode: string;

  /**
   * 发行日期
   */
  ecIssueDate: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

  /**
   * 设变标题
   */
  ecTitle: string;

  /**
   * 设变内容
   */
  ecContent: string;

  /**
   * 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecLeader: string;

  /**
   * 损失金额
   */
  ecLossAmount: number;

  /**
   * 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
   */
  ecScope: number;

  /**
   * 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
   */
  ecDeptScope: number;

  /**
   * 录入日期
   */
  ecEntryDate: string;

  /**
   * 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
   */
  ecStatus: number;

  /**
   * 设变明细列表（子表，级联保存）
   */
  ecDetails?: EcDetailCreate[];

  /**
   * 来源设变主表 ID；大明细延后由服务端物化
   */
  sourceEcId?: string;

  /**
   * 草稿是否省略明细
   */
  detailsDeferred?: boolean;

  /**
   * 省略时的明细行数
   */
  deferredDetailCount?: number;

  /**
   * 设变附件列表（一个设变可对应多个附件）（子表，级联保存）
   */
  attachments?: EcAttachmentCreate[];

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
 * 更新EcGijutsu DTO
 * 继承 TaktEcGijutsuCreateDto，添加 ecGijutsuId 字段
 * 对应前端 EcGijutsuUpdate
 * @description 对应后端 TaktEcGijutsuUpdateDto
 */
export interface EcGijutsuUpdate extends EcGijutsuCreate {
  /**
   * ecGijutsuId（标识要更新的实体）
   */
  ecGijutsuId: string;

}


/**
 * EcGijutsu 状态更新 DTO
 * 对应前端 EcGijutsuStatus
 * @description 对应后端 TaktEcGijutsuStatusDto
 */
export interface EcGijutsuStatus {
  /**
   * ecGijutsuId
   */
  ecGijutsuId: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

}


/**
 * EcGijutsu 导入模板行 DTO
 * 对应前端 EcTemplate
 * @description 对应后端 TaktEcGijutsuTemplateDto
 */
export interface EcGijutsuTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 设变单号（唯一）
   */
  ecCode?: string;

  /**
   * 发行日期
   */
  ecIssueDate?: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus?: number;

  /**
   * 设变标题
   */
  ecTitle?: string;

  /**
   * 设变内容
   */
  ecContent?: string;

  /**
   * 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecLeader?: string;

  /**
   * 损失金额
   */
  ecLossAmount?: number;

  /**
   * 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
   */
  ecScope?: number;

  /**
   * 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
   */
  ecDeptScope?: number;

  /**
   * 录入日期
   */
  ecEntryDate?: string;

  /**
   * 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
   */
  ecStatus?: number;

  /**
   * 设变明细列表（子表，级联保存）
   */
  ecDetails?: EcDetailCreate[];

  /**
   * 设变附件列表（一个设变可对应多个附件）（子表，级联保存）
   */
  attachments?: EcAttachmentCreate[];

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
 * EcGijutsu 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcImport
 * @description 对应后端 TaktEcGijutsuImportDto
 */
export interface EcGijutsuImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 设变单号（唯一）
   */
  ecCode?: string;

  /**
   * 发行日期
   */
  ecIssueDate?: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus?: number;

  /**
   * 设变标题
   */
  ecTitle?: string;

  /**
   * 设变内容
   */
  ecContent?: string;

  /**
   * 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecLeader?: string;

  /**
   * 损失金额
   */
  ecLossAmount?: number;

  /**
   * 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
   */
  ecScope?: number;

  /**
   * 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
   */
  ecDeptScope?: number;

  /**
   * 录入日期
   */
  ecEntryDate?: string;

  /**
   * 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
   */
  ecStatus?: number;

  /**
   * 设变明细列表（子表，级联保存）
   */
  ecDetails?: EcDetailCreate[];

  /**
   * 设变附件列表（一个设变可对应多个附件）（子表，级联保存）
   */
  attachments?: EcAttachmentCreate[];

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
 * EcGijutsu 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcExport
 * @description 对应后端 TaktEcGijutsuExportDto
 */
export interface EcGijutsuExport {
  /**
   * EcID
   */
  ecGijutsuId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 设变单号（唯一）
   */
  ecCode: string;

  /**
   * 发行日期
   */
  ecIssueDate: string;

  /**
   * 变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)
   */
  changeStatus: number;

  /**
   * 设变标题
   */
  ecTitle: string;

  /**
   * 设变内容
   */
  ecContent: string;

  /**
   * 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecLeader: string;

  /**
   * 损失金额
   */
  ecLossAmount: number;

  /**
   * 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
   */
  ecScope: number;

  /**
   * 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
   */
  ecDeptScope: number;

  /**
   * 录入日期
   */
  ecEntryDate: string;

  /**
   * 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
   */
  ecStatus: number;

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
 * 设变技术课主表统计（主表设变单数 + 子表明细行数）
 */
export interface EcGijutsuStat {
  /** 统计月份 yyyy-MM */
  statMonth: string;
  /** 设变主表数量 */
  ecCount: number;
  /** 设变明细子表记录数 */
  ecDetailCount: number;
  /** 工厂代码 */
  plantCode?: string;
}

/**
 * 设变技术课主表统计查询
 */
export interface EcGijutsuStatQuery {
  /** 录入日期-开始 */
  ecEntryDateStart?: string;
  /** 录入日期-结束 */
  ecEntryDateEnd?: string;
  /** 工厂代码 */
  plantCode?: string;
}

/**
 * 设变技术课主表后台保存已提交回执（Create/Update 入队后立即返回）
 * @description 对应后端 TaktEcGijutsuSubmittedDto
 */
export interface EcGijutsuSubmitted {
  /** 工厂代码 */
  plantCode: string;
  /** 设变单号 */
  ecCode: string;
  /** 是否为更新 */
  isUpdate: boolean;
  /** 明细行数 */
  detailCount: number;
  /** 更新时的主表主键（新增为 "0"） */
  ecGijutsuId: string;
}

