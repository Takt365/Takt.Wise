// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecSeizouikka.types.ts
// 创建时间：2026-09-08
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
 * 设变制造1课（D0610）部门执行表
 * 对应前端 TaktEcSeizouikkaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcSeizouikka
 * @description 对应后端 TaktEcSeizouikkaDto
 */
export interface EcSeizouikka extends CompanyDtoBase {
  /**
   * EcSeizouikkaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecSeizouikkaId: string;

  /**
   * 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
   */
  ecDetailId: string;

  /**
   * 设变明细 名称（填充字段）
   */
  ecDetailName?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 机种编码组（按设变时同设变单号下的机种编码，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
   */
  ecRootMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 设变明细列表（视图主从：执行表为主；一对多；业务键 EcCode + EcModelCode + EcRootMaterialCode）
   */
  ecDetails?: EcDetail[];

}


/**
 * EcSeizouikka 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcSeizouikkaQuery
 * @description 对应后端 TaktEcSeizouikkaQueryDto
 */
export interface EcSeizouikkaQuery extends TaktPagedQuery {
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
   * 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
   */
  ecDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
   */
  ecRootMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建EcSeizouikka DTO
 * 对应前端 EcSeizouikkaCreate
 * @description 对应后端 TaktEcSeizouikkaCreateDto
 */
export interface EcSeizouikkaCreate {
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
   * 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
   */
  ecDetailId: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 机种编码组（按设变时同设变单号下的机种编码，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
   */
  ecRootMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新EcSeizouikka DTO
 * 继承 TaktEcSeizouikkaCreateDto，添加 EcSeizouikkaId 字段
 * 对应前端 EcSeizouikkaUpdate
 * @description 对应后端 TaktEcSeizouikkaUpdateDto
 */
export interface EcSeizouikkaUpdate extends EcSeizouikkaCreate {
  /**
   * EcSeizouikkaID（标识要更新的实体）
   */
  ecSeizouikkaId: string;

}


/**
 * 对应前端 EcSeizouikkaDiscontinuedStatus
 * @description 对应后端 TaktEcSeizouikkaDiscontinuedStatusDto
 */
export interface EcSeizouikkaDiscontinuedStatus {
  /**
   * EcSeizouikkaID
   */
  ecSeizouikkaId: string;
  /**
   * 根物料停产状态（字典 logistics_materials_material_discontinued_status；Z0=在产；停产按钮默认 ZQ）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;
}

/**
 * EcSeizouikka 作废/撤销作废 DTO
 * 对应前端 EcSeizouikkaObsolete
 * @description 对应后端 TaktEcSeizouikkaObsoleteDto
 */
export interface EcSeizouikkaObsolete {
  /**
   * EcSeizouikkaID
   */
  ecSeizouikkaId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcSeizouikka 导入模板行 DTO
 * 对应前端 EcSeizouikkaTemplate
 * @description 对应后端 TaktEcSeizouikkaTemplateDto
 */
export interface EcSeizouikkaTemplate {
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
   * 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
   */
  ecDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
   */
  ecRootMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * EcSeizouikka 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcSeizouikkaImport
 * @description 对应后端 TaktEcSeizouikkaImportDto
 */
export interface EcSeizouikkaImport {
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
   * 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
   */
  ecDetailId?: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
   */
  ecRootMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * EcSeizouikka 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcSeizouikkaExport
 * @description 对应后端 TaktEcSeizouikkaExportDto
 */
export interface EcSeizouikkaExport {
  /**
   * EcSeizouikkaID
   */
  ecSeizouikkaId: string;

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
   * 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
   */
  ecDetailId: string;

  /**
   * 设变单号（冗余，便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 机种编码组（按设变时同设变单号下的机种编码，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 根物料描述（冗余：来自 TaktEcDetail.EcRootMaterialDescription）
   */
  ecRootMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料；冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;

  /**
   * 部门编码（TaktDept.DeptCode，5 位，如 D0610）
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 预定日期（冗余：来自 TaktEcSeikan.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcSeikan.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 实施批次
   */
  implementationBatch?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

