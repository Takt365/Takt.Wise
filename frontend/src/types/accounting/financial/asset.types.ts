// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：asset.types.ts
// 创建时间：2026-08-30
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 资产实体
 * 对应前端 TaktAssetDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Asset
 * @description 对应后端 TaktAssetDto
 */
export interface Asset extends CompanyDtoBase {
  /**
   * AssetID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assetId: string;

  /**
   * 资产代码
   */
  assetCode: string;

  /**
   * 资产名称
   */
  assetName: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
   */
  assetType: string;

  /**
   * 资产原值
   */
  assetOriginalValue: number;

  /**
   * 资产净值
   */
  assetNetValue: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 报废日期
   */
  scrapDate?: string;

  /**
   * 处置日期
   */
  disposalDate?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus: number;

}


/**
 * Asset 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssetQuery
 * @description 对应后端 TaktAssetQueryDto
 */
export interface AssetQuery extends TaktPagedQuery {
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
   * 资产代码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory?: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
   */
  assetType?: string;

  /**
   * 资产原值
   */
  assetOriginalValue?: number;

  /**
   * 资产净值
   */
  assetNetValue?: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation?: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期（范围查询-开始）
   */
  purchaseDateStart?: string;

  /**
   * 购买日期（范围查询-结束）
   */
  purchaseDateEnd?: string;

  /**
   * 启用日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 启用日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 报废日期（范围查询-开始）
   */
  scrapDateStart?: string;

  /**
   * 报废日期（范围查询-结束）
   */
  scrapDateEnd?: string;

  /**
   * 处置日期（范围查询-开始）
   */
  disposalDateStart?: string;

  /**
   * 处置日期（范围查询-结束）
   */
  disposalDateEnd?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths?: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
   */
  depreciationMethod?: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation?: number;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus?: number;

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
 * 创建Asset DTO
 * 对应前端 AssetCreate
 * @description 对应后端 TaktAssetCreateDto
 */
export interface AssetCreate {
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
   * 资产代码
   */
  assetCode: string;

  /**
   * 资产名称
   */
  assetName: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
   */
  assetType: string;

  /**
   * 资产原值
   */
  assetOriginalValue: number;

  /**
   * 资产净值
   */
  assetNetValue: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 报废日期
   */
  scrapDate?: string;

  /**
   * 处置日期
   */
  disposalDate?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus: number;

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
 * 更新Asset DTO
 * 继承 TaktAssetCreateDto，添加 AssetId 字段
 * 对应前端 AssetUpdate
 * @description 对应后端 TaktAssetUpdateDto
 */
export interface AssetUpdate extends AssetCreate {
  /**
   * AssetID（标识要更新的实体）
   */
  assetId: string;

}


/**
 * Asset 状态更新 DTO
 * 对应前端 AssetStatus
 * @description 对应后端 TaktAssetStatusDto
 */
export interface AssetStatus {
  /**
   * AssetID
   */
  assetId: string;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus: number;

}


/**
 * Asset 导入模板行 DTO
 * 对应前端 AssetTemplate
 * @description 对应后端 TaktAssetTemplateDto
 */
export interface AssetTemplate {
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
   * 资产代码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory?: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
   */
  assetType?: string;

  /**
   * 资产原值
   */
  assetOriginalValue?: number;

  /**
   * 资产净值
   */
  assetNetValue?: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation?: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 报废日期
   */
  scrapDate?: string;

  /**
   * 处置日期
   */
  disposalDate?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths?: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
   */
  depreciationMethod?: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation?: number;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus?: number;

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
 * Asset 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssetImport
 * @description 对应后端 TaktAssetImportDto
 */
export interface AssetImport {
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
   * 资产代码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory?: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
   */
  assetType?: string;

  /**
   * 资产原值
   */
  assetOriginalValue?: number;

  /**
   * 资产净值
   */
  assetNetValue?: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation?: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 报废日期
   */
  scrapDate?: string;

  /**
   * 处置日期
   */
  disposalDate?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths?: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
   */
  depreciationMethod?: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation?: number;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus?: number;

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
 * Asset 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssetExport
 * @description 对应后端 TaktAssetExportDto
 */
export interface AssetExport {
  /**
   * AssetID
   */
  assetId: string;

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
   * 资产代码
   */
  assetCode: string;

  /**
   * 资产名称
   */
  assetName: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
   */
  assetType: string;

  /**
   * 资产原值
   */
  assetOriginalValue: number;

  /**
   * 资产净值
   */
  assetNetValue: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 报废日期
   */
  scrapDate?: string;

  /**
   * 处置日期
   */
  disposalDate?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
   */
  assetStatus: number;

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

