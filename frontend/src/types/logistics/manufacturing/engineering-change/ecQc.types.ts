// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecQc.types.ts
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
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）
   */
  productionDate?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;


/**
 * 设变品管课（部门 D0820）执行表。未作废明细均落行。按成品去重键为设变单号+根物料（机种写入编码组）；按机种为设变单号+机种（根物料写入编码组）；按设变为仅设变单号（机种与根物料写入编码组）
 * 对应前端 TaktEcQcDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcQc
 * @description 对应后端 TaktEcQcDto
 */
export interface EcQc extends CompanyDtoBase {
  /**
   * EcQcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecQcId: string;

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
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）
   */
  productionDate?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 检验日期（品管自填）
   */
  inspectionDate?: string;

  /**
   * 检验批次（品管自填）
   */
  inspectionBatch?: string;

  /**
   * 执行内容（各部门通用；抽样号码写入本字段，不单列）
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

}


/**
 * EcQc 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcQcQuery
 * @description 对应后端 TaktEcQcQueryDto
 */
export interface EcQcQuery extends TaktPagedQuery {
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
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup?: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 检验日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

  /**
   * 检验批次（品管自填）
   */
  inspectionBatch?: string;

  /**
   * 执行内容（各部门通用；抽样号码写入本字段，不单列）
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
 * 创建EcQc DTO
 * 对应前端 EcQcCreate
 * @description 对应后端 TaktEcQcCreateDto
 */
export interface EcQcCreate {
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
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）
   */
  productionDate?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 检验日期（品管自填）
   */
  inspectionDate?: string;

  /**
   * 检验批次（品管自填）
   */
  inspectionBatch?: string;

  /**
   * 执行内容（各部门通用；抽样号码写入本字段，不单列）
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
 * 更新EcQc DTO
 * 继承 TaktEcQcCreateDto，添加 EcQcId 字段
 * 对应前端 EcQcUpdate
 * @description 对应后端 TaktEcQcUpdateDto
 */
export interface EcQcUpdate extends EcQcCreate {
  /**
   * EcQcID（标识要更新的实体）
   */
  ecQcId: string;

}


/**
 * EcQc 状态更新 DTO
 * 对应前端 EcQcStatus
 * @description 对应后端 TaktEcQcStatusDto
 */
export interface EcQcStatus {
  /**
   * EcQcID
   */
  ecQcId: string;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

}


/**
 * EcQc 作废/撤销作废 DTO
 * 对应前端 EcQcObsolete
 * @description 对应后端 TaktEcQcObsoleteDto
 */
export interface EcQcObsolete {
  /**
   * EcQcID
   */
  ecQcId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcQc 导入模板行 DTO
 * 对应前端 EcQcTemplate
 * @description 对应后端 TaktEcQcTemplateDto
 */
export interface EcQcTemplate {
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
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup?: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）
   */
  productionDate?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 检验日期（品管自填）
   */
  inspectionDate?: string;

  /**
   * 检验批次（品管自填）
   */
  inspectionBatch?: string;

  /**
   * 执行内容（各部门通用；抽样号码写入本字段，不单列）
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
 * EcQc 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcQcImport
 * @description 对应后端 TaktEcQcImportDto
 */
export interface EcQcImport {
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
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup?: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）
   */
  productionDate?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 检验日期（品管自填）
   */
  inspectionDate?: string;

  /**
   * 检验批次（品管自填）
   */
  inspectionBatch?: string;

  /**
   * 执行内容（各部门通用；抽样号码写入本字段，不单列）
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
 * EcQc 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcQcExport
 * @description 对应后端 TaktEcQcExportDto
 */
export interface EcQcExport {
  /**
   * EcQcID
   */
  ecQcId: string;

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
   * 新物料编码组（同去重键下的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 旧物料编码组（同去重键下的旧物料编码，分号分隔；ntext）
   */
  ecOldMaterialCodeGroup: string;

  /**
   * 生产班组（冗余：来自制一 TaktEcMfg1.ProductionTeam；多选 TeamCode 分号分隔；只读；Length=200）
   */
  productionTeam?: string;

  /**
   * 生产日期（冗余：来自制一 TaktEcMfg1.ProductionDate；只读；空则空）
   */
  productionDate?: string;

  /**
   * 生产批次（冗余：来自制一 TaktEcMfg1.ProductionBatch；只读）
   */
  productionBatch?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 检验日期（品管自填）
   */
  inspectionDate?: string;

  /**
   * 检验批次（品管自填）
   */
  inspectionBatch?: string;

  /**
   * 执行内容（各部门通用；抽样号码写入本字段，不单列）
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

