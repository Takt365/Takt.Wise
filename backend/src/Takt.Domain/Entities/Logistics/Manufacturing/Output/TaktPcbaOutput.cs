// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutput.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报实体，按工厂、生产日期、工单等记录PCBA生产订单；班组/人员/设备/标准工时产能等字段在子表明细；新增时子表明细按物料查 TaktStandardOperationTime 工作中心自动生成；明细含 SMT/修正 工作中心时由 TaktPcbaOutputService 级联生成检查/改修日报
// 计算公式：达成率(%) = 明细当日完成数量合计 ÷ 明细人员标准产能合计 × 100%（见 TaktProductionStatHelper.CalculateAchievementRatePercent）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报实体
/// 达成率(%) = 明细当日完成数量合计 ÷ 明细人员标准产能合计 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_pcba", "PCBA日报表")]
[SugarIndex("ix_pcba_output_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_output_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_prod_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, false)]
public class TaktPcbaOutput : TaktCompanyEntityBase
{

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    [SugarColumn(ColumnName = "prod_category", ColumnDescription = "生产类别", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    [SugarColumn(ColumnName = "prod_date", ColumnDescription = "生产日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 工单类别（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_type", ColumnDescription = "工单类别", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdOrderType { get; set; }

    /// <summary>
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", Length = 12, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? BatchCode { get; set; }

    /// <summary>
    /// 工单数量（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty", ColumnDescription = "工单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdOrderQty { get; set; } = 0;

    /// <summary>
    /// 序列号（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "serial_code", ColumnDescription = "序列号", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? SerialCode { get; set; }

    /// <summary>
    /// PCBA明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPcbaOutputDetail.PcbaOutputId))]
    public List<TaktPcbaOutputDetail>? PcbaOutputDetails { get; set; }
}
