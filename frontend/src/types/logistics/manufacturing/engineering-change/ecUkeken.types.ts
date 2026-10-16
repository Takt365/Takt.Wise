// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecUkeken.types.ts
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
 * 设变受检课（D0810）部门执行表
 * 对应前端 TaktEcUkekenDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcUkeken
 * @description 对应后端 TaktEcUkekenDto
 */
export interface EcUkeken extends CompanyDtoBase {
  /**
   * EcUkekenID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecUkekenId: string;

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
   * 机种编码组（同设变单号+新品检验+新物料，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变单号+新品检验+新物料，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;
  /** 根物料描述（仅全仕向且按成品时有值） */
  ecRootMaterialDescription?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
   */
  ecNewMaterialDescription?: string;

  /**
   * 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是；冗余：来自 TaktEcDetail.EcNewRequiresInspection）
   */
  ecNewRequiresInspection: number;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 受检单号
   */
  iqcOrderCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 设变明细列表（视图主从：执行表为主；一对多；业务键 EcCode + EcNewMaterialCode）
   */
  ecDetails?: EcDetail[];

}


/**
 * EcUkeken 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcUkekenQuery
 * @description 对应后端 TaktEcUkekenQueryDto
 */
export interface EcUkekenQuery extends TaktPagedQuery {
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
   * 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
   */
  ecNewMaterialDescription?: string;

  /**
   * 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是；冗余：来自 TaktEcDetail.EcNewRequiresInspection）
   */
  ecNewRequiresInspection?: number;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode?: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 受检单号
   */
  iqcOrderCode?: string;

  /**
   * 检验日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

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
 * 创建EcUkeken DTO
 * 对应前端 EcUkekenCreate
 * @description 对应后端 TaktEcUkekenCreateDto
 */
export interface EcUkekenCreate {
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
   * 机种编码组（同设变单号+新品检验+新物料，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变单号+新品检验+新物料，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;
  /** 根物料描述（仅全仕向且按成品时有值） */
  ecRootMaterialDescription?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;

  /**
   * 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
   */
  ecNewMaterialDescription?: string;

  /**
   * 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是；冗余：来自 TaktEcDetail.EcNewRequiresInspection）
   */
  ecNewRequiresInspection: number;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
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
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 受检单号
   */
  iqcOrderCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

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
 * 更新EcUkeken DTO
 * 继承 TaktEcUkekenCreateDto，添加 EcUkekenId 字段
 * 对应前端 EcUkekenUpdate
 * @description 对应后端 TaktEcUkekenUpdateDto
 */
export interface EcUkekenUpdate extends EcUkekenCreate {
  /**
   * EcUkekenID（标识要更新的实体）
   */
  ecUkekenId: string;

}


/**
 * 对应前端 EcUkekenDiscontinuedStatus
 * @description 对应后端 TaktEcUkekenDiscontinuedStatusDto
 */
export interface EcUkekenDiscontinuedStatus {
  /**
   * EcUkekenID
   */
  ecUkekenId: string;
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
 * EcUkeken 作废/撤销作废 DTO
 * 对应前端 EcUkekenObsolete
 * @description 对应后端 TaktEcUkekenObsoleteDto
 */
export interface EcUkekenObsolete {
  /**
   * EcUkekenID
   */
  ecUkekenId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcUkeken 导入模板行 DTO
 * 对应前端 EcUkekenTemplate
 * @description 对应后端 TaktEcUkekenTemplateDto
 */
export interface EcUkekenTemplate {
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
   * 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
   */
  ecNewMaterialDescription?: string;

  /**
   * 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是；冗余：来自 TaktEcDetail.EcNewRequiresInspection）
   */
  ecNewRequiresInspection?: number;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode?: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 受检单号
   */
  iqcOrderCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

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
 * EcUkeken 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcUkekenImport
 * @description 对应后端 TaktEcUkekenImportDto
 */
export interface EcUkekenImport {
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
   * 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
   */
  ecNewMaterialDescription?: string;

  /**
   * 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是；冗余：来自 TaktEcDetail.EcNewRequiresInspection）
   */
  ecNewRequiresInspection?: number;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
   */
  deptCode?: string;

  /**
   * 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 是否实施（0=否 1=是，字典 sys_yes_no）
   */
  isImplemented?: number;

  /**
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 受检单号
   */
  iqcOrderCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

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
 * EcUkeken 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcUkekenExport
 * @description 对应后端 TaktEcUkekenExportDto
 */
export interface EcUkekenExport {
  /**
   * EcUkekenID
   */
  ecUkekenId: string;

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
   * 机种编码组（同设变单号+新品检验+新物料，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变单号+新品检验+新物料，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;
  /** 根物料描述（仅全仕向且按成品时有值） */
  ecRootMaterialDescription?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
   */
  discontinuedStatus: string;

  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
  /** 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category） */
  ecDeptScope?: number;

  /**
   * 新物料编码（冗余：来自 TaktEcDetail.EcNewMaterialCode）
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述（冗余：来自 TaktEcDetail.EcNewMaterialDescription）
   */
  ecNewMaterialDescription?: string;

  /**
   * 新品仓库（选项 TaktWarehouses/options；DictValue=WarehouseCode；冗余：来自 TaktEcDetail.EcNewWarehouse）
   */
  ecNewWarehouse?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是；冗余：来自 TaktEcDetail.EcNewRequiresInspection）
   */
  ecNewRequiresInspection: number;

  /**
   * 部门编码（TaktDept.DeptCode；本表固定课别）
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
   * 执行内容（各部门通用）
   */
  execContent?: string;

  /**
   * 受检单号
   */
  iqcOrderCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

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

