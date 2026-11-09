// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecMe.types.ts
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
import type { EcDetail } from './ecDetail.types';

/**
 * 设变制造技术课（部门 D0630）执行表。未作废明细均落行。任何范围去重键为设变单号+机种，根物料写入编码组
 * 对应前端 TaktEcMeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcMe
 * @description 对应后端 TaktEcMeDto
 */
export interface EcMe extends CompanyDtoBase {
  /**
   * EcMeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecMeId: string;

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
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
   */
  isSopUpdated: number;

  /**
   * SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  sopLeader?: string;

  /**
   * SOP日期
   */
  sopDate?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
   */
  isImplemented: number;

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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；制技固定设变+机种 DictValue=2，无按成品）
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
   * 关联设变明细（按 EcCode + 机种编码组/根物料编码组匹配，详情填充）
   */
  ecDetails?: EcDetail[];

}


/**
 * EcMe 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcMeQuery
 * @description 对应后端 TaktEcMeQueryDto
 */
export interface EcMeQuery extends TaktPagedQuery {
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
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
   */
  isSopUpdated?: number;

  /**
   * SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  sopLeader?: string;

  /**
   * SOP日期（范围查询-开始）
   */
  sopDateStart?: string;

  /**
   * SOP日期（范围查询-结束）
   */
  sopDateEnd?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
   */
  isImplemented?: number;

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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；制技固定设变+机种 DictValue=2，无按成品）
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
 * 创建EcMe DTO
 * 对应前端 EcMeCreate
 * @description 对应后端 TaktEcMeCreateDto
 */
export interface EcMeCreate {
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
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
   */
  isSopUpdated: number;

  /**
   * SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  sopLeader?: string;

  /**
   * SOP日期
   */
  sopDate?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
   */
  isImplemented: number;

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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；制技固定设变+机种 DictValue=2，无按成品）
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
 * 更新EcMe DTO
 * 继承 TaktEcMeCreateDto，添加 EcMeId 字段
 * 对应前端 EcMeUpdate
 * @description 对应后端 TaktEcMeUpdateDto
 */
export interface EcMeUpdate extends EcMeCreate {
  /**
   * EcMeID（标识要更新的实体）
   */
  ecMeId: string;

}


/**
 * EcMe 停产状态更新 DTO
 * 对应前端 EcMeDiscontinuedStatus
 * @description 对应后端 TaktEcMeDiscontinuedStatusDto
 */
export interface EcMeDiscontinuedStatus {
  /**
   * EcMeID
   */
  ecMeId: string;

  /**
   * 根物料停产状态（字典 logistics_materials_material_discontinued_status；Z0=在产；停产按钮默认 ZQ）
   */
  discontinuedStatus: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcImplScope）
   */
  ecImplScope?: number;

  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种
   */
  ecControlScope?: number;

}


/**
 * EcMe 作废/撤销作废 DTO
 * 对应前端 EcMeObsolete
 * @description 对应后端 TaktEcMeObsoleteDto
 */
export interface EcMeObsolete {
  /**
   * EcMeID
   */
  ecMeId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcMe 导入模板行 DTO
 * 对应前端 EcMeTemplate
 * @description 对应后端 TaktEcMeTemplateDto
 */
export interface EcMeTemplate {
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
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
   */
  isSopUpdated?: number;

  /**
   * SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  sopLeader?: string;

  /**
   * SOP日期
   */
  sopDate?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
   */
  isImplemented?: number;

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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；制技固定设变+机种 DictValue=2，无按成品）
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
 * EcMe 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcMeImport
 * @description 对应后端 TaktEcMeImportDto
 */
export interface EcMeImport {
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
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
   */
  isSopUpdated?: number;

  /**
   * SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  sopLeader?: string;

  /**
   * SOP日期
   */
  sopDate?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
   */
  isImplemented?: number;

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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；制技固定设变+机种 DictValue=2，无按成品）
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
 * EcMe 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcMeExport
 * @description 对应后端 TaktEcMeExportDto
 */
export interface EcMeExport {
  /**
   * EcMeID
   */
  ecMeId: string;

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
   * 根物料编码组（同去重键下的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 是否更新 SOP（字典 sys_yes_no；0=否 1=是）
   */
  isSopUpdated: number;

  /**
   * SOP担当（选项 TaktEcGroups/options?ecGroupCategory=2；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  sopLeader?: string;

  /**
   * SOP日期
   */
  sopDate?: string;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 是否实施（制技不落库，不参与页签。页签按是否更新 IsSopUpdated：0=未更新，1=已更新）
   */
  isImplemented: number;

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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；制技固定设变+机种 DictValue=2，无按成品）
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

