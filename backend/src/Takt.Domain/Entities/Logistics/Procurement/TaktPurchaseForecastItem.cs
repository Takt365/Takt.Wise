// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseForecastItem.cs
// 创建时间：2026-07-29
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购预测明细（结构对齐销售预测明细：财年×月 001/002/增减与已转采购数量）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购预测明细（一行 = 主表物料在某财年某月的 001/002 计划量；产品/类别/利润中心/机种/物料在主表）
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_forecast_item", "采购预测明细表")]
[SugarIndex("ix_purchase_forecast_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_forecast_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_forecast_item_month_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseForecastId), OrderByType.Asc, nameof(FiscalYear), OrderByType.Asc, nameof(PlanMonth), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_forecast_item_plan_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseForecastCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_forecast_item_fiscal_year", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FiscalYear), OrderByType.Asc, false)]
public class TaktPurchaseForecastItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_forecast_id", ColumnDescription = "采购预测ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_forecast_code", ColumnDescription = "采购预测编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
    /// </summary>
    [SugarColumn(ColumnName = "fiscal_year", ColumnDescription = "财年", ColumnDataType = "nvarchar", Length = 6, IsNullable = false)]
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 计划月份（1～12）
    /// </summary>
    [SugarColumn(ColumnName = "plan_month", ColumnDescription = "计划月份", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlanMonth { get; set; } = 1;

    /// <summary>
    /// 计划数量版本001
    /// </summary>
    [SugarColumn(ColumnName = "plan_quantity_001", ColumnDescription = "计划数量版本001", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlanQuantity001 { get; set; } = 0;

    /// <summary>
    /// 计划数量版本002
    /// </summary>
    [SugarColumn(ColumnName = "plan_quantity_002", ColumnDescription = "计划数量版本002", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlanQuantity002 { get; set; } = 0;

    /// <summary>
    /// 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
    /// </summary>
    [SugarColumn(ColumnName = "plan_quantity_delta", ColumnDescription = "计划增减", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlanQuantityDelta { get; set; } = 0;

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转采购数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 预计单价
    /// </summary>
    [SugarColumn(ColumnName = "estimated_unit_price", ColumnDescription = "预计单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedUnitPrice { get; set; } = 0;

    /// <summary>
    /// 预计金额
    /// </summary>
    [SugarColumn(ColumnName = "estimated_amount", ColumnDescription = "预计金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedAmount { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
