// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecKoubai.types.ts
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
 * 设变采购课（D0510）部门执行表
 * 对应前端 TaktEcKoubaiDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcKoubai
 * @description 对应后端 TaktEcKoubaiDto
 */
export interface EcKoubai extends CompanyDtoBase {
  /**
   * EcKoubaiID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecKoubaiId: string;

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
   * 机种编码组（同设变单号+新采购类型+新物料，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变单号+新采购类型+新物料，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

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
   * 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
   */
  ecNewPurchaseType?: string;

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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * EcKoubai 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcKoubaiQuery
 * @description 对应后端 TaktEcKoubaiQueryDto
 */
export interface EcKoubaiQuery extends TaktPagedQuery {
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
   * 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
   */
  ecNewPurchaseType?: string;

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
   * 采购订单发行日期（范围查询-开始）
   */
  purchaseOrderIssueDateStart?: string;

  /**
   * 采购订单发行日期（范围查询-结束）
   */
  purchaseOrderIssueDateEnd?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * 创建EcKoubai DTO
 * 对应前端 EcKoubaiCreate
 * @description 对应后端 TaktEcKoubaiCreateDto
 */
export interface EcKoubaiCreate {
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
   * 机种编码组（同设变单号+新采购类型+新物料，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变单号+新采购类型+新物料，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

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
   * 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
   */
  ecNewPurchaseType?: string;

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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * 更新EcKoubai DTO
 * 继承 TaktEcKoubaiCreateDto，添加 EcKoubaiId 字段
 * 对应前端 EcKoubaiUpdate
 * @description 对应后端 TaktEcKoubaiUpdateDto
 */
export interface EcKoubaiUpdate extends EcKoubaiCreate {
  /**
   * EcKoubaiID（标识要更新的实体）
   */
  ecKoubaiId: string;

}


/**
 * 对应前端 EcKoubaiDiscontinuedStatus
 * @description 对应后端 TaktEcKoubaiDiscontinuedStatusDto
 */
export interface EcKoubaiDiscontinuedStatus {
  /**
   * EcKoubaiID
   */
  ecKoubaiId: string;
  /**
   * 根物料停产状态（字典 logistics_materials_material_discontinued_status；Z0=在产；停产按钮默认 ZQ）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcDetail.EcScope）
   */
  ecScope: number;
}

/**
 * EcKoubai 作废/撤销作废 DTO
 * 对应前端 EcKoubaiObsolete
 * @description 对应后端 TaktEcKoubaiObsoleteDto
 */
export interface EcKoubaiObsolete {
  /**
   * EcKoubaiID
   */
  ecKoubaiId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcKoubai 导入模板行 DTO
 * 对应前端 EcKoubaiTemplate
 * @description 对应后端 TaktEcKoubaiTemplateDto
 */
export interface EcKoubaiTemplate {
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
   * 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
   */
  ecNewPurchaseType?: string;

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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * EcKoubai 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcKoubaiImport
 * @description 对应后端 TaktEcKoubaiImportDto
 */
export interface EcKoubaiImport {
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
   * 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
   */
  ecNewPurchaseType?: string;

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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * EcKoubai 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcKoubaiExport
 * @description 对应后端 TaktEcKoubaiExportDto
 */
export interface EcKoubaiExport {
  /**
   * EcKoubaiID
   */
  ecKoubaiId: string;

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
   * 机种编码组（同设变单号+新采购类型+新物料，分号分隔）
   */
  ecModelCodeGroup?: string;

  /**
   * 根物料编码组（同设变单号+新采购类型+新物料，分号分隔）
   */
  ecRootMaterialCodeGroup?: string;

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
   * 新采购类型（F=外部采购，E=自制生产；冗余：来自 TaktEcDetail.EcNewPurchaseType）
   */
  ecNewPurchaseType?: string;

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
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderCode?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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

