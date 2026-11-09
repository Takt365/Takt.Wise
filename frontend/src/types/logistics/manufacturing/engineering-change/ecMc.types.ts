// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecMc.types.ts
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
 * 设变部管课（部门 D0430）执行表。采购类型 F、仓库非 C003 且新物料有效才落行。按设变单号去重一行；机种/根物料/新物料写入编码组。内部/技术实施范围仍可落行并按设变聚合。
 * 对应前端 TaktEcMcDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcMc
 * @description 对应后端 TaktEcMcDto
 */
export interface EcMc extends CompanyDtoBase {
  /**
   * EcMcID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecMcId: string;

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
   * 机种编码组（同设变下可见明细的机种编码，分号分隔；ntext）
   */
  ecModelCodeGroup: string;

  /**
   * 根物料编码组（同设变下可见明细的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 新物料编码组（同设变下采购类型 F、仓库非 C003 的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 新采购类型（冗余：来自保留明细；落行多为 F）
   */
  ecNewPurchaseType: string;

  /**
   * 新品仓库（冗余：来自保留明细；非 C003）
   */
  ecNewWarehouse: string;

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
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 执行内容（各部门通用；ntext）
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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
   */
  ecControlScope: number;

  /**
   * 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
   */
  ecGrainKey: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
   */
  isObsolete: number;

  /**
   * 设变明细列表（视图主从：执行表为主；一对多；业务键 EcCode + EcModelCode + EcRootMaterialCode）
   */
  ecDetails?: EcDetail[];

}


/**
 * EcMc 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcMcQuery
 * @description 对应后端 TaktEcMcQueryDto
 */
export interface EcMcQuery extends TaktPagedQuery {
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
   * 机种编码组（同设变下可见明细的机种编码，分号分隔；ntext）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变下可见明细的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 新物料编码组（同设变下采购类型 F、仓库非 C003 的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 新采购类型（冗余：来自保留明细；落行多为 F）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品仓库（冗余：来自保留明细；非 C003）
   */
  ecNewWarehouse?: string;

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
   * 出库日期（范围查询-开始）
   */
  outboundDateStart?: string;

  /**
   * 出库日期（范围查询-结束）
   */
  outboundDateEnd?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 执行内容（各部门通用；ntext）
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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
   */
  ecControlScope?: number;

  /**
   * 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
   */
  ecGrainKey?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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
 * 创建EcMc DTO
 * 对应前端 EcMcCreate
 * @description 对应后端 TaktEcMcCreateDto
 */
export interface EcMcCreate {
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
   * 机种编码组（同设变下可见明细的机种编码，分号分隔；ntext）
   */
  ecModelCodeGroup: string;

  /**
   * 根物料编码组（同设变下可见明细的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 新物料编码组（同设变下采购类型 F、仓库非 C003 的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 新采购类型（冗余：来自保留明细；落行多为 F）
   */
  ecNewPurchaseType: string;

  /**
   * 新品仓库（冗余：来自保留明细；非 C003）
   */
  ecNewWarehouse: string;

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
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 执行内容（各部门通用；ntext）
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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
   */
  ecControlScope: number;

  /**
   * 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
   */
  ecGrainKey: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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
 * 更新EcMc DTO
 * 继承 TaktEcMcCreateDto，添加 EcMcId 字段
 * 对应前端 EcMcUpdate
 * @description 对应后端 TaktEcMcUpdateDto
 */
export interface EcMcUpdate extends EcMcCreate {
  /**
   * EcMcID（标识要更新的实体）
   */
  ecMcId: string;

}


/**
 * EcMc 停产状态更新 DTO
 * 对应前端 EcMcDiscontinuedStatus
 * @description 对应后端 TaktEcMcDiscontinuedStatusDto
 */
export interface EcMcDiscontinuedStatus {
  /**
   * EcMcID
   */
  ecMcId: string;

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
 * EcMc 作废/撤销作废 DTO
 * 对应前端 EcMcObsolete
 * @description 对应后端 TaktEcMcObsoleteDto
 */
export interface EcMcObsolete {
  /**
   * EcMcID
   */
  ecMcId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcMc 导入模板行 DTO
 * 对应前端 EcMcTemplate
 * @description 对应后端 TaktEcMcTemplateDto
 */
export interface EcMcTemplate {
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
   * 机种编码组（同设变下可见明细的机种编码，分号分隔；ntext）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变下可见明细的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 新物料编码组（同设变下采购类型 F、仓库非 C003 的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 新采购类型（冗余：来自保留明细；落行多为 F）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品仓库（冗余：来自保留明细；非 C003）
   */
  ecNewWarehouse?: string;

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
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 执行内容（各部门通用；ntext）
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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
   */
  ecControlScope?: number;

  /**
   * 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
   */
  ecGrainKey?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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
 * EcMc 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcMcImport
 * @description 对应后端 TaktEcMcImportDto
 */
export interface EcMcImport {
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
   * 机种编码组（同设变下可见明细的机种编码，分号分隔；ntext）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变下可见明细的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup?: string;

  /**
   * 新物料编码组（同设变下采购类型 F、仓库非 C003 的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup?: string;

  /**
   * 新采购类型（冗余：来自保留明细；落行多为 F）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品仓库（冗余：来自保留明细；非 C003）
   */
  ecNewWarehouse?: string;

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
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 执行内容（各部门通用；ntext）
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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
   */
  ecControlScope?: number;

  /**
   * 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
   */
  ecGrainKey?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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
 * EcMc 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcMcExport
 * @description 对应后端 TaktEcMcExportDto
 */
export interface EcMcExport {
  /**
   * EcMcID
   */
  ecMcId: string;

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
   * 机种编码组（同设变下可见明细的机种编码，分号分隔；ntext）
   */
  ecModelCodeGroup: string;

  /**
   * 根物料编码组（同设变下可见明细的根物料编码，分号分隔；ntext）
   */
  ecRootMaterialCodeGroup: string;

  /**
   * 新物料编码组（同设变下采购类型 F、仓库非 C003 的新物料编码，分号分隔；ntext）
   */
  ecNewMaterialCodeGroup: string;

  /**
   * 新采购类型（冗余：来自保留明细；落行多为 F）
   */
  ecNewPurchaseType: string;

  /**
   * 新品仓库（冗余：来自保留明细；非 C003）
   */
  ecNewWarehouse: string;

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
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 执行内容（各部门通用；ntext）
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
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品 2=按机种 3=按设变；跟随主表选中值）
   */
  ecControlScope: number;

  /**
   * 执行行去重键（按控制范围：按成品/按机种/按设变；同一键的明细共用本行。ntext）
   */
  ecGrainKey: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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

