// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialCost.types.ts
// 创建时间：2026-11-02
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
 * BOM 物料成本汇总表（由明细按工厂+产品+核算日聚合写入；UI 机种组→产品行→Item，实体不拆分，无对明细导航） <para>业务唯一键（新增/更新匹配）：TenantCode+CompanyCode+PlantCode+ProductCode+CostingDate；ModelCode/CostingPeriod 为业务字段，不参与唯一匹配。</para> <para>软删除：本表 IsDeleted 可为 0/1；列表读隔离不显示已删；合计/重算/机种平均须 includeSoftDeleted 含已删主表。</para>
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
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
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
   * 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
   */
  productMonthlyCost: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
   */
  productMonthlyCalculation: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
   */
  costingPeriod: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）
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
   * 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
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
   * 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
   */
  productMonthlyCost?: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
   */
  productMonthlyCalculation?: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
   */
  costingPeriod?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）（范围查询-开始）
   */
  costingDateStart?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）（范围查询-结束）
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
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
   * 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
   */
  productMonthlyCost: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
   */
  productMonthlyCalculation: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
   */
  costingPeriod: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
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
   * 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
   */
  productMonthlyCost?: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
   */
  productMonthlyCalculation?: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
   */
  costingPeriod?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
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
   * 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
   */
  productMonthlyCost?: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
   */
  productMonthlyCalculation?: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
   */
  costingPeriod?: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）
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
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+核算月+机种+物料类型下各产品金额算术平均； 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才更新并记 ExtField 前后值）
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
   * 产品月成本（外部系统计算后的月成本；合计/重算/零价回填不得覆盖）
   */
  productMonthlyCost: number;

  /**
   * 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数）
   */
  productMonthlyCalculation: number;

  /**
   * 最近采购成本（与产品月计算同一快照口径：生产相关=X、PCB SECT 标识为空、采购类型=F、用量 &gt; 0.001；行金额=组件数量×(净价÷采购价格单位)）
   */
  latestPurchaseCost: number;

  /**
   * 币种（字典 accounting_financial_currency_code；如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；展示/筛选用，不参与唯一匹配）
   */
  costingPeriod: string;

  /**
   * 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；与 ProductCode 构成业务唯一键之一）
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

