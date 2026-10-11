// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktPurchaseSalesInventoryDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseSalesInventory 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseSalesInventory 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// PurchaseSalesInventory 响应 DTO
// ========================================

/// <summary>
/// 进销存表实体（存货数量金额账；CAS《存货》成本流转 / IAS 2 inventory movement） 勾稽：期末数量/成本 = 期初 + 采购入库 + 生产入库 + 其他入库调整 − 出库成本结转； 销售收入单独列示，不计入存货成本等式（避免收入与成本混淆）。 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别
/// 对应前端 TaktPurchaseSalesInventoryDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseSalesInventoryDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseSalesInventoryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseSalesInventoryId { get; set; }


    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitCode { get; set; } = string.Empty;

    /// <summary>
    /// 期初数量
    /// </summary>
    public decimal OpeningQty { get; set; }

    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    public decimal OpeningAmount { get; set; }

    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    public decimal PurchaseQty { get; set; }

    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    public decimal PurchaseAmount { get; set; }

    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    public decimal ProductionQty { get; set; }

    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    public decimal ProductionAmount { get; set; }

    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    public decimal IssueQty { get; set; }

    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    public decimal IssueCostAmount { get; set; }

    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    public decimal SalesRevenueAmount { get; set; }

    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    public decimal AdjustQty { get; set; }

    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    public decimal AdjustAmount { get; set; }

    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    public decimal ClosingQty { get; set; }

    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    public decimal ClosingAmount { get; set; }

    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    public decimal ClosingUnitCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int PsiStatus { get; set; } = 0;

}

// ========================================
// PurchaseSalesInventory 查询 DTO
// ========================================

/// <summary>
/// PurchaseSalesInventory 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseSalesInventoryQueryDto : TaktPagedQuery
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitCode { get; set; } = string.Empty;

    /// <summary>
    /// 期初数量
    /// </summary>
    public decimal? OpeningQty { get; set; }

    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    public decimal? OpeningAmount { get; set; }

    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    public decimal? PurchaseQty { get; set; }

    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    public decimal? PurchaseAmount { get; set; }

    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    public decimal? ProductionQty { get; set; }

    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    public decimal? ProductionAmount { get; set; }

    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    public decimal? IssueQty { get; set; }

    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    public decimal? IssueCostAmount { get; set; }

    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    public decimal? SalesRevenueAmount { get; set; }

    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    public decimal? AdjustQty { get; set; }

    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    public decimal? AdjustAmount { get; set; }

    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    public decimal? ClosingQty { get; set; }

    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    public decimal? ClosingAmount { get; set; }

    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    public decimal? ClosingUnitCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? PsiStatus { get; set; }

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

    /// <summary>
    /// 物料描述（冗余）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;
}

// ========================================
// 创建PurchaseSalesInventory DTO
// ========================================

/// <summary>
/// 创建PurchaseSalesInventory DTO
/// </summary>
public class TaktPurchaseSalesInventoryCreateDto
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "关联工厂（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    [Required(ErrorMessage = "会计期间编码（YYYYMM）不能为空")]
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余）
    /// </summary>
    [Required(ErrorMessage = "物料名称（冗余）不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [Required(ErrorMessage = "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）不能为空")]
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitCode { get; set; } = string.Empty;

    /// <summary>
    /// 期初数量
    /// </summary>
    public decimal OpeningQty { get; set; }

    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    public decimal OpeningAmount { get; set; }

    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    public decimal PurchaseQty { get; set; }

    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    public decimal PurchaseAmount { get; set; }

    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    public decimal ProductionQty { get; set; }

    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    public decimal ProductionAmount { get; set; }

    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    public decimal IssueQty { get; set; }

    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    public decimal IssueCostAmount { get; set; }

    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    public decimal SalesRevenueAmount { get; set; }

    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    public decimal AdjustQty { get; set; }

    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    public decimal AdjustAmount { get; set; }

    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    public decimal ClosingQty { get; set; }

    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    public decimal ClosingAmount { get; set; }

    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    public decimal ClosingUnitCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int PsiStatus { get; set; } = 0;

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
// 更新PurchaseSalesInventory DTO
// ========================================

/// <summary>
/// 更新PurchaseSalesInventory DTO
/// 继承 TaktPurchaseSalesInventoryCreateDto，添加 PurchaseSalesInventoryId 字段
/// </summary>
public class TaktPurchaseSalesInventoryUpdateDto : TaktPurchaseSalesInventoryCreateDto
{
    /// <summary>
    /// PurchaseSalesInventoryID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseSalesInventoryId { get; set; }

}

// ========================================
// PurchaseSalesInventory 状态 DTO
// ========================================

/// <summary>
/// PurchaseSalesInventory 状态更新 DTO
/// </summary>
public class TaktPurchaseSalesInventoryStatusDto
{
    /// <summary>
    /// PurchaseSalesInventoryID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseSalesInventoryId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=停用）不能为空")]
    public int PsiStatus { get; set; } = 0;
}

// ========================================
// PurchaseSalesInventory 排序 DTO
// ========================================

/// <summary>
/// PurchaseSalesInventory 排序更新 DTO
/// </summary>
public class TaktPurchaseSalesInventorySortDto
{
    /// <summary>
    /// PurchaseSalesInventoryID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseSalesInventoryId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseSalesInventory 导入模板行 DTO
/// </summary>
public class TaktPurchaseSalesInventoryTemplateDto
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitCode { get; set; } = string.Empty;

    /// <summary>
    /// 期初数量
    /// </summary>
    public decimal? OpeningQty { get; set; }

    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    public decimal? OpeningAmount { get; set; }

    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    public decimal? PurchaseQty { get; set; }

    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    public decimal? PurchaseAmount { get; set; }

    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    public decimal? ProductionQty { get; set; }

    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    public decimal? ProductionAmount { get; set; }

    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    public decimal? IssueQty { get; set; }

    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    public decimal? IssueCostAmount { get; set; }

    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    public decimal? SalesRevenueAmount { get; set; }

    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    public decimal? AdjustQty { get; set; }

    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    public decimal? AdjustAmount { get; set; }

    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    public decimal? ClosingQty { get; set; }

    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    public decimal? ClosingAmount { get; set; }

    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    public decimal? ClosingUnitCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? PsiStatus { get; set; }

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
/// PurchaseSalesInventory 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseSalesInventoryImportDto
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitCode { get; set; } = string.Empty;

    /// <summary>
    /// 期初数量
    /// </summary>
    public decimal? OpeningQty { get; set; }

    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    public decimal? OpeningAmount { get; set; }

    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    public decimal? PurchaseQty { get; set; }

    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    public decimal? PurchaseAmount { get; set; }

    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    public decimal? ProductionQty { get; set; }

    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    public decimal? ProductionAmount { get; set; }

    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    public decimal? IssueQty { get; set; }

    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    public decimal? IssueCostAmount { get; set; }

    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    public decimal? SalesRevenueAmount { get; set; }

    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    public decimal? AdjustQty { get; set; }

    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    public decimal? AdjustAmount { get; set; }

    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    public decimal? ClosingQty { get; set; }

    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    public decimal? ClosingAmount { get; set; }

    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    public decimal? ClosingUnitCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? PsiStatus { get; set; }

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
/// PurchaseSalesInventory 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseSalesInventoryExportDto
{
    /// <summary>
    /// PurchaseSalesInventoryID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseSalesInventoryId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options 或 TaktMaterialPlants/options，DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位
    /// </summary>
    public string? UnitCode { get; set; } = string.Empty;

    /// <summary>
    /// 期初数量
    /// </summary>
    public decimal OpeningQty { get; set; }

    /// <summary>
    /// 期初存货成本金额（IAS 2 / CAS 成本口径）
    /// </summary>
    public decimal OpeningAmount { get; set; }

    /// <summary>
    /// 本期采购入库数量
    /// </summary>
    public decimal PurchaseQty { get; set; }

    /// <summary>
    /// 本期采购入库成本金额
    /// </summary>
    public decimal PurchaseAmount { get; set; }

    /// <summary>
    /// 本期生产入库数量（自制半成品/产成品）
    /// </summary>
    public decimal ProductionQty { get; set; }

    /// <summary>
    /// 本期生产入库成本金额
    /// </summary>
    public decimal ProductionAmount { get; set; }

    /// <summary>
    /// 本期出库数量（销售/领用等发出）
    /// </summary>
    public decimal IssueQty { get; set; }

    /// <summary>
    /// 本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）
    /// </summary>
    public decimal IssueCostAmount { get; set; }

    /// <summary>
    /// 本期销售收入金额（利润表口径；不参与存货成本等式）
    /// </summary>
    public decimal SalesRevenueAmount { get; set; }

    /// <summary>
    /// 本期调整数量（盘盈盘亏、报废等，可正可负）
    /// </summary>
    public decimal AdjustQty { get; set; }

    /// <summary>
    /// 本期调整成本金额
    /// </summary>
    public decimal AdjustAmount { get; set; }

    /// <summary>
    /// 期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）
    /// </summary>
    public decimal ClosingQty { get; set; }

    /// <summary>
    /// 期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）
    /// </summary>
    public decimal ClosingAmount { get; set; }

    /// <summary>
    /// 期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）
    /// </summary>
    public decimal ClosingUnitCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int PsiStatus { get; set; } = 0;

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
