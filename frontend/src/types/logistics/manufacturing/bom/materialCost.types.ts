// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialCost.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * BOM 物料成本汇总表（由明细按工厂+机种+产品+核算月聚合写入；UI 机种组→产品行→Item，实体不拆分，无对明细导航）
 * 对应前端 TaktBomMaterialCostDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BomMaterialCost
 * @description 对应后端 TaktBomMaterialCostDto
 */
export interface BomMaterialCost extends CompanyDtoBase {
  /**
   * BomMaterialCostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bomMaterialCostId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
   */
  modelMonthlyAverageCost: number;

  /**
   * 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
   */
  materialType: string;

  /**
   * 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
   */
  productCode: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
 * 产品月成本
   */
  productMonthlyCost: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
   */
  productMonthlyCalculation: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
   */
  costingDate: string;

}


/**
 * BomMaterialCost 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BomMaterialCostQuery
 * @description 对应后端 TaktBomMaterialCostQueryDto
 */
export interface BomMaterialCostQuery extends TaktPagedQuery {
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
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
   */
  modelMonthlyAverageCost?: number;

  /**
   * 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
   */
  materialType?: string;

  /**
   * 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
   */
  productCode?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
 * 产品月成本
   */
  productMonthlyCost?: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
   */
  productMonthlyCalculation?: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）（范围查询-开始）
   */
  costingDateStart?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）（范围查询-结束）
   */
  costingDateEnd?: string;

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
 * 创建BomMaterialCost DTO
 * 对应前端 BomMaterialCostCreate
 * @description 对应后端 TaktBomMaterialCostCreateDto
 */
export interface BomMaterialCostCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
   */
  modelMonthlyAverageCost: number;

  /**
   * 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
   */
  materialType: string;

  /**
   * 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
   */
  productCode: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
 * 产品月成本
   */
  productMonthlyCost: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
   */
  productMonthlyCalculation: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
   */
  costingDate: string;

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
 * 更新BomMaterialCost DTO
 * 继承 TaktBomMaterialCostCreateDto，添加 BomMaterialCostId 字段
 * 对应前端 BomMaterialCostUpdate
 * @description 对应后端 TaktBomMaterialCostUpdateDto
 */
export interface BomMaterialCostUpdate extends BomMaterialCostCreate {
  /**
   * BomMaterialCostID（标识要更新的实体）
   */
  bomMaterialCostId: string;

}


/**
 * BomMaterialCost 导入模板行 DTO
 * 对应前端 BomMaterialCostTemplate
 * @description 对应后端 TaktBomMaterialCostTemplateDto
 */
export interface BomMaterialCostTemplate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
   */
  modelMonthlyAverageCost?: number;

  /**
   * 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
   */
  materialType?: string;

  /**
   * 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
   */
  productCode?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
 * 产品月成本
   */
  productMonthlyCost?: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
   */
  productMonthlyCalculation?: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
   */
  costingDate?: string;

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
 * BomMaterialCost 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BomMaterialCostImport
 * @description 对应后端 TaktBomMaterialCostImportDto
 */
export interface BomMaterialCostImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
   */
  modelMonthlyAverageCost?: number;

  /**
   * 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
   */
  materialType?: string;

  /**
   * 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
   */
  productCode?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
 * 产品月成本
   */
  productMonthlyCost?: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
   */
  productMonthlyCalculation?: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
   */
  costingDate?: string;

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
 * BomMaterialCost 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BomMaterialCostExport
 * @description 对应后端 TaktBomMaterialCostExportDto
 */
export interface BomMaterialCostExport {
  /**
   * BomMaterialCostID
   */
  bomMaterialCostId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
   */
  modelMonthlyAverageCost: number;

  /**
   * 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
   */
  materialType: string;

  /**
   * 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
   */
  productCode: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
 * 产品月成本
   */
  productMonthlyCost: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
   */
  productMonthlyCalculation: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
   */
  costingDate: string;

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
 * BOM 物料成本机种维度展示行（工厂+物料类型+机种+核算期间聚合）
 * @description 对应后端 TaktBomMaterialCostModelGroupDto（分析/三层浏览用；非 CRUD 实体）
 */
export interface BomMaterialCostModelGroup {
  /** 分组键（plant|materialType|model|period） */
  groupKey: string;
  plantCode: string;
  materialType: string;
  modelCode: string;
  modelMonthlyAverageCost: number;
  currencyCode: string;
  costingPeriod: string;
  costingDate: string;
  productRowCount: number;
}

