// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyBatchDefect.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立批量不良统计实体，按生产类别+批次汇总 TaktAssyDefect 生实实绩与不良数量；批次工单总数量=累计生实实绩时批次状态为已完成
// 计算公式：不良数量 = 累计生实实绩 - 累计无不良数量；不良率(%) = 不良数量 ÷ 累计生实实绩 × 100%；直行率(%) = 累计无不良数量 ÷ 累计生实实绩 × 100%
// 数据来源：组立不良日报 TaktAssyDefect 保存/删除时由 TaktAssyDefectStatSyncHelper 刷新（批次为空时跳过）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立批量不良统计实体（统计维度：生产类别+批次）
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_assy_batch", "组立批量不良统计表")]
[SugarIndex("ix_assy_batch_defect_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_assy_batch_defect_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_batch_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(BatchCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_batch_batch_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BatchCode), OrderByType.Asc, false)]
public class TaktAssyBatchDefect : TaktCompanyEntityBase
{
    /// <summary>
    /// 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    [SugarColumn(ColumnName = "prod_category", ColumnDescription = "生产类别", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdCategory { get; set; } = string.Empty;
    /// <summary>
    /// 批次（统计维度）
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string BatchCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
    /// </summary>
    [SugarColumn(ColumnName = "prod_date_group", ColumnDescription = "生产日期组", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdDateGroup { get; set; }
    /// <summary>
    /// 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_group", ColumnDescription = "生产工单组", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdOrderGroup { get; set; }
    /// <summary>
    /// 机种（取最近日报）
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "生产物料组", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? MaterialGroup { get; set; }
    /// <summary>
    /// 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
    /// </summary>
    [SugarColumn(ColumnName = "batch_order_qty", ColumnDescription = "批次工单总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal BatchOrderQty { get; set; } = 0;
    /// <summary>
    /// 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty_group", ColumnDescription = "订单数量组", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdOrderQtyGroup { get; set; }
    /// <summary>
    /// 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
    /// </summary>
    [SugarColumn(ColumnName = "prod_actual_qty", ColumnDescription = "累计生实实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdActualQty { get; set; } = 0;
    /// <summary>
    /// 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
    /// </summary>
    [SugarColumn(ColumnName = "good_quantity", ColumnDescription = "累计无不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal GoodQuantity { get; set; } = 0;
    /// <summary>
    /// 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
    /// </summary>
    [SugarColumn(ColumnName = "defect_qty", ColumnDescription = "累计不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal DefectQty { get; set; } = 0;
    /// <summary>
    /// 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    [SugarColumn(ColumnName = "defect_rate_percent", ColumnDescription = "不良率", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DefectRatePercent { get; set; } = 0;
    /// <summary>
    /// 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
    /// </summary>
    [SugarColumn(ColumnName = "yield_rate_percent", ColumnDescription = "直行率", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal YieldRatePercent { get; set; } = 0;
    /// <summary>
    /// 最近生产日期（关联日报最大 ProdDate）
    /// </summary>
    [SugarColumn(ColumnName = "last_prod_date", ColumnDescription = "最近生产日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? LastProdDate { get; set; }
    /// <summary>
    /// 关联组立不良日报笔数
    /// </summary>
    [SugarColumn(ColumnName = "report_count", ColumnDescription = "日报笔数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReportCount { get; set; } = 0;
    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
    /// </summary>
    [SugarColumn(ColumnName = "batch_status", ColumnDescription = "批次状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BatchStatus { get; set; } = 1;
}
