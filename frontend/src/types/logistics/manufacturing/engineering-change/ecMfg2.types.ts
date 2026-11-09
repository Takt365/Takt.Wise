// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecMfg2.types.ts
// 创建时间：2026-10-29
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
 * 设变制造2课（部门 D0620）执行表。未作废明细均落行。按成品去重键为设变单号+根物料（机种写入编码组）；按机种为设变单号+机种（根物料写入编码组）；按设变为仅设变单号（机种与根物料写入编码组）
 * 对应前端 TaktEcMfg2Dto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcMfg2
 * @description 对应后端 TaktEcMfg2Dto
 */
export interface EcMfg2 extends CompanyDtoBase {
  /**
   * EcMfg2ID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecMfg2Id: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 设变单号（冗余：来自 TaktEcDetail.EcCode）
   */
  ecCode: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 机种编码组（单个或多个机种，分号分隔；ntext）
   */
  ecModelCodeGroup: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup: string;

  /**
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 生产班组（多选；选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode 过滤；分号分隔；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/电子物料/制技固定设变+机种（DictValue=2）
   */
  ecControlScope: number;

  /**
   * 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
   */
  ecGrainKey: string;

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
 * EcMfg2 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcMfg2Query
 * @description 对应后端 TaktEcMfg2QueryDto
 */
export interface EcMfg2Query extends TaktPagedQuery {
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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 设变单号（冗余：来自 TaktEcDetail.EcCode）
   */
  ecCode?: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;

  /**
   * 机种编码组（单个或多个机种，分号分隔；ntext）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup?: string;

  /**
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）（范围查询-开始）
   */
  scheduledDateStart?: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）（范围查询-结束）
   */
  scheduledDateEnd?: string;

  /**
   * 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 生产班组（多选；选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode 过滤；分号分隔；Length=200）
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
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode?: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope?: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/电子物料/制技固定设变+机种（DictValue=2）
   */
  ecControlScope?: number;

  /**
   * 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
   */
  ecGrainKey?: string;

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
 * 创建EcMfg2 DTO
 * 对应前端 EcMfg2Create
 * @description 对应后端 TaktEcMfg2CreateDto
 */
export interface EcMfg2Create {
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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 设变单号（冗余：来自 TaktEcDetail.EcCode）
   */
  ecCode: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 机种编码组（单个或多个机种，分号分隔；ntext）
   */
  ecModelCodeGroup: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup: string;

  /**
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 生产班组（多选；选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode 过滤；分号分隔；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/电子物料/制技固定设变+机种（DictValue=2）
   */
  ecControlScope: number;

  /**
   * 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
   */
  ecGrainKey: string;

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
 * 更新EcMfg2 DTO
 * 继承 TaktEcMfg2CreateDto，添加 EcMfg2Id 字段
 * 对应前端 EcMfg2Update
 * @description 对应后端 TaktEcMfg2UpdateDto
 */
export interface EcMfg2Update extends EcMfg2Create {
  /**
   * EcMfg2ID（标识要更新的实体）
   */
  ecMfg2Id: string;

}


/**
 * EcMfg2 停产状态更新 DTO
 * 对应前端 EcMfg2DiscontinuedStatus
 * @description 对应后端 TaktEcMfg2DiscontinuedStatusDto
 */
export interface EcMfg2DiscontinuedStatus {
  /**
   * EcMfg2ID
   */
  ecMfg2Id: string;

  /**
   * 根物料停产状态（字典 logistics_materials_material_discontinued_status；Z0=在产；停产按钮默认 ZQ）
   */
  discontinuedStatus: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category）
   */
  ecControlScope: number;

}


/**
 * EcMfg2 作废/撤销作废 DTO
 * 对应前端 EcMfg2Obsolete
 * @description 对应后端 TaktEcMfg2ObsoleteDto
 */
export interface EcMfg2Obsolete {
  /**
   * EcMfg2ID
   */
  ecMfg2Id: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcMfg2 导入模板行 DTO
 * 对应前端 EcMfg2Template
 * @description 对应后端 TaktEcMfg2TemplateDto
 */
export interface EcMfg2Template {
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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 设变单号（冗余：来自 TaktEcDetail.EcCode）
   */
  ecCode?: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;

  /**
   * 机种编码组（单个或多个机种，分号分隔；ntext）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup?: string;

  /**
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 生产班组（多选；选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode 过滤；分号分隔；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode?: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope?: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/电子物料/制技固定设变+机种（DictValue=2）
   */
  ecControlScope?: number;

  /**
   * 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
   */
  ecGrainKey?: string;

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
 * EcMfg2 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcMfg2Import
 * @description 对应后端 TaktEcMfg2ImportDto
 */
export interface EcMfg2Import {
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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 设变单号（冗余：来自 TaktEcDetail.EcCode）
   */
  ecCode?: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus?: string;

  /**
   * 机种编码组（单个或多个机种，分号分隔；ntext）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription?: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup?: string;

  /**
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 生产班组（多选；选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode 过滤；分号分隔；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode?: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope?: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/电子物料/制技固定设变+机种（DictValue=2）
   */
  ecControlScope?: number;

  /**
   * 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
   */
  ecGrainKey?: string;

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
 * EcMfg2 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcMfg2Export
 * @description 对应后端 TaktEcMfg2ExportDto
 */
export interface EcMfg2Export {
  /**
   * EcMfg2ID
   */
  ecMfg2Id: string;

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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 设变单号（冗余：来自 TaktEcDetail.EcCode）
   */
  ecCode: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 机种编码组（单个或多个机种，分号分隔；ntext）
   */
  ecModelCodeGroup: string;

  /**
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext，按机种时同机种根物料可能很多）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 上阶物料编码（冗余：来自 TaktEcDetail.EcParentMaterialCode）
   */
  ecParentMaterialCode: string;

  /**
   * 上阶物料描述（冗余：来自 TaktEcDetail.EcParentMaterialDescription）
   */
  ecParentMaterialDescription: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup: string;

  /**
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 预定日期（冗余：来自 TaktEcPc.ScheduledDate）
   */
  scheduledDate?: string;

  /**
   * 预定批次（冗余：来自 TaktEcPc.ScheduledBatch）
   */
  scheduledBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 生产班组（多选；选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode 过滤；分号分隔；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/电子物料/制技固定设变+机种（DictValue=2）
   */
  ecControlScope: number;

  /**
   * 执行行去重键（设变单号、机种或根物料或新物料、分道；同一键的明细共用本行。ntext）
   */
  ecGrainKey: string;

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

