// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：BomMaterialCost 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBomMaterialCost 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// BomMaterialCost 响应 DTO
// ========================================

/// <summary>
/// BOM 物料成本汇总表（由明细按工厂+机种+产品+核算月聚合写入；UI 机种组→产品行→Item，实体不拆分，无对明细导航）
/// 对应前端 TaktBomMaterialCostDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBomMaterialCostDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BomMaterialCostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostId { get; set; }


    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
 /// 产品月成本
    /// </summary>
    public decimal ProductMonthlyCost { get; set; }

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// </summary>
    public decimal ProductMonthlyCalculation { get; set; }

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    public decimal LatestPurchaseCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
    /// </summary>
    public DateTime CostingDate { get; set; }

}

// ========================================
// BomMaterialCost 查询 DTO
// ========================================

/// <summary>
/// BomMaterialCost 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBomMaterialCostQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
    /// </summary>
    public decimal? ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
 /// 产品月成本
    /// </summary>
    public decimal? ProductMonthlyCost { get; set; }

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// </summary>
    public decimal? ProductMonthlyCalculation { get; set; }

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    public decimal? LatestPurchaseCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string? CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）（范围查询-开始）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）（范围查询-结束）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建BomMaterialCost DTO
// ========================================

/// <summary>
/// 创建BomMaterialCost DTO
/// </summary>
public class TaktBomMaterialCostCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    [Required(ErrorMessage = "机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    [Required(ErrorMessage = "物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    [Required(ErrorMessage = "产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>不能为空")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    [Required(ErrorMessage = "产品描述不能为空")]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
 /// 产品月成本
    /// </summary>
    public decimal ProductMonthlyCost { get; set; }

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// </summary>
    public decimal ProductMonthlyCalculation { get; set; }

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    public decimal LatestPurchaseCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；如 CNY/USD）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    [Required(ErrorMessage = "核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）不能为空")]
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
    /// </summary>
    public DateTime CostingDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新BomMaterialCost DTO
// ========================================

/// <summary>
/// 更新BomMaterialCost DTO
/// 继承 TaktBomMaterialCostCreateDto，添加 BomMaterialCostId 字段
/// </summary>
public class TaktBomMaterialCostUpdateDto : TaktBomMaterialCostCreateDto
{
    /// <summary>
    /// BomMaterialCostID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BomMaterialCost 导入模板行 DTO
/// </summary>
public class TaktBomMaterialCostTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
    /// </summary>
    public decimal? ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
 /// 产品月成本
    /// </summary>
    public decimal? ProductMonthlyCost { get; set; }

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// </summary>
    public decimal? ProductMonthlyCalculation { get; set; }

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    public decimal? LatestPurchaseCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string? CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
    /// </summary>
    public DateTime? CostingDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// BomMaterialCost 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBomMaterialCostImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
    /// </summary>
    public decimal? ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
 /// 产品月成本
    /// </summary>
    public decimal? ProductMonthlyCost { get; set; }

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// </summary>
    public decimal? ProductMonthlyCalculation { get; set; }

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    public decimal? LatestPurchaseCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string? CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
    /// </summary>
    public DateTime? CostingDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// BomMaterialCost 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBomMaterialCostExportDto
{
    /// <summary>
    /// BomMaterialCostID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（选项 TaktModelDestinations/model-options；DictValue=ModelCode） <para>分析/成本推移查询栏「机种」下拉：须用 TaktBomCostOptions/model-options（本表 ModelCode 去重，可按 PlantCode/MaterialType 过滤），❌ 勿用 TaktModelDestinations/model-options。</para>
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+物料类型+机种+核算月份下各产品月成本算术平均）
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 物料类型（存 ROH/HALB/FERT 等码） <para>CRUD 表单：字典 logistics_materials_material_type。</para> <para>分析/推移查询栏：本表 MaterialType 去重 options（TaktBomCostOptions/material-type-options，含全部类型），❌ 勿与 CRUD 字典下拉混用；查询栏可空=不过滤。</para>
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码；本表业务主键之一） <para>分析/成本推移查询栏「物料」下拉：须用 TaktBomCostOptions/product-options（本表 ProductCode 去重，可按 PlantCode/MaterialType/ModelCode 过滤），❌ 勿用 TaktMaterialPlants/options 或字典 logistics_materials_material_type。</para> <para>导入时 18 位纯数字自动归一化为后 10 位。</para>
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
 /// 产品月成本
    /// </summary>
    public decimal ProductMonthlyCost { get; set; }

    /// <summary>
    /// 产品月计算（本系统按明细合计：生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// </summary>
    public decimal ProductMonthlyCalculation { get; set; }

    /// <summary>
    /// 最近采购成本（与产品月计算同一快照口径；行金额=组件数量×(净价÷采购价格单位)）
    /// </summary>
    public decimal LatestPurchaseCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（必须与本次成本合计/重算所用明细 TaktBomMaterialCostItem.CostingDate 一致；同月多日时取最后核算日）
    /// </summary>
    public DateTime CostingDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
