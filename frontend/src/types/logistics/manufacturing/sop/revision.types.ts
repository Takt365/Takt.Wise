// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：revision.types.ts
// 创建时间：2026-08-12
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
 * SOP 版本实体
 * 对应前端 TaktSopRevisionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopRevision
 * @description 对应后端 TaktSopRevisionDto
 */
export interface SopRevision extends CompanyDtoBase {
  /**
   * SopRevisionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopRevisionId: string;

  /**
   * SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId: string;

  /**
   * SOP 文档头 名称（填充字段）
   */
  sopName?: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
   */
  ecnId?: string;

  /**
   * 关联 ECN 主表 名称（填充字段）
   */
  ecnName?: string;

  /**
   * 是否锁定（字典 sys_yes_no；0=否，1=是）
   */
  isLocked: number;

  /**
   * 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
   */
  forceLeaderAck: number;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus: number;

  /**
   * 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
   */
  effectiveRule: number;

  /**
   * SOP 文档头 （主表：TaktSopDoc）
   */
  sopDoc?: SopDoc;

  /**
   * 多语言正文 （子表：TaktSopContent）
   */
  contents?: SopContent[];

}


/**
 * SopRevision 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopRevisionQuery
 * @description 对应后端 TaktSopRevisionQueryDto
 */
export interface SopRevisionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
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
   * SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId?: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision?: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
   */
  ecnId?: string;

  /**
   * 是否锁定（字典 sys_yes_no；0=否，1=是）
   */
  isLocked?: number;

  /**
   * 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
   */
  forceLeaderAck?: number;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus?: number;

  /**
   * 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
   */
  effectiveRule?: number;

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
 * 创建SopRevision DTO
 * 对应前端 SopRevisionCreate
 * @description 对应后端 TaktSopRevisionCreateDto
 */
export interface SopRevisionCreate {
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
   * SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
   */
  ecnId?: string;

  /**
   * 是否锁定（字典 sys_yes_no；0=否，1=是）
   */
  isLocked: number;

  /**
   * 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
   */
  forceLeaderAck: number;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus: number;

  /**
   * 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
   */
  effectiveRule: number;

  /**
   * 多语言正文（子表，级联保存）
   */
  contents?: SopContentCreate[];

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
 * 更新SopRevision DTO
 * 继承 TaktSopRevisionCreateDto，添加 SopRevisionId 字段
 * 对应前端 SopRevisionUpdate
 * @description 对应后端 TaktSopRevisionUpdateDto
 */
export interface SopRevisionUpdate extends SopRevisionCreate {
  /**
   * SopRevisionID（标识要更新的实体）
   */
  sopRevisionId: string;

  /**
   * 多语言正文（子表，级联保存）
   */
  contents?: any;

}


/**
 * SopRevision 状态更新 DTO
 * 对应前端 SopRevisionStatus
 * @description 对应后端 TaktSopRevisionStatusDto
 */
export interface SopRevisionStatus {
  /**
   * SopRevisionID
   */
  sopRevisionId: string;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus: number;

}


/**
 * SopRevision 导入模板行 DTO
 * 对应前端 SopRevisionTemplate
 * @description 对应后端 TaktSopRevisionTemplateDto
 */
export interface SopRevisionTemplate {
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
   * SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId?: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision?: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
   */
  ecnId?: string;

  /**
   * 是否锁定（字典 sys_yes_no；0=否，1=是）
   */
  isLocked?: number;

  /**
   * 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
   */
  forceLeaderAck?: number;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus?: number;

  /**
   * 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
   */
  effectiveRule?: number;

  /**
   * 多语言正文（子表，级联保存）
   */
  contents?: SopContentCreate[];

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
 * SopRevision 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopRevisionImport
 * @description 对应后端 TaktSopRevisionImportDto
 */
export interface SopRevisionImport {
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
   * SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId?: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision?: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
   */
  ecnId?: string;

  /**
   * 是否锁定（字典 sys_yes_no；0=否，1=是）
   */
  isLocked?: number;

  /**
   * 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
   */
  forceLeaderAck?: number;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus?: number;

  /**
   * 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
   */
  effectiveRule?: number;

  /**
   * 多语言正文（子表，级联保存）
   */
  contents?: SopContentCreate[];

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
 * SopRevision 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopRevisionExport
 * @description 对应后端 TaktSopRevisionExportDto
 */
export interface SopRevisionExport {
  /**
   * SopRevisionID
   */
  sopRevisionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
   */
  sopId: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
   */
  ecnId?: string;

  /**
   * 是否锁定（字典 sys_yes_no；0=否，1=是）
   */
  isLocked: number;

  /**
   * 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
   */
  forceLeaderAck: number;

  /**
   * 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
   */
  revisionStatus: number;

  /**
   * 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
   */
  effectiveRule: number;

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

