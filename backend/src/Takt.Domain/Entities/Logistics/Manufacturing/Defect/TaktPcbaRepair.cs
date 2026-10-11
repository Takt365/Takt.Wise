// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepair.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修日报实体，按工厂、生产日期、生产线等记录改修主数据
// 计算公式：不良率(%) = 明细不良数量合计 ÷ 明细生产实绩合计 × 100%（分母为 0 时取 0）
// 直行率(%) = (明细生产实绩合计 - 明细不良数量合计) ÷ 明细生产实绩合计 × 100%（数量取自 TaktPcbaRepairDetail）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA改修日报实体
/// 不良率(%) = 明细不良数量合计 ÷ 明细生产实绩合计 × 100%；直行率(%) = (生产实绩 - 不良数量) ÷ 生产实绩 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_pcba_repair", "PCBA改修日报表")]
[SugarIndex("ix_pcba_repair_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_repair_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_repair_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_repair_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_repair_team_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_repair_prod_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, false)]
public class TaktPcbaRepair : TaktCompanyEntityBase
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
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", Length = 8, ColumnDataType = "nvarchar", IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

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
    /// 工单数量
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty", ColumnDescription = "工单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdOrderQty { get; set; } = 0;

    /// <summary>
    /// 机种
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? BatchCode { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// PCBA改修明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPcbaRepairDetail.PcbaRepairId))]
    public List<TaktPcbaRepairDetail>? PcbaRepairDetails { get; set; }
}
