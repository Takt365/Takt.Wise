// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.LaborHour
// 文件名称：TaktPcbaAiLaborHours.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA自插工数统计实体，按生产日期、班组、班次汇总 TaktPcbaOutput / TaktPcbaOutputDetail（自插工作中心）工时与产量
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.LaborHour;

/// <summary>
/// PCBA自插工数统计实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_labor_hour_pcba_ai", "PCBA自插工数统计表")]
[SugarIndex("ix_pcba_ai_labor_hours_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_ai_labor_hours_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(ShiftNo), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_labor_hour_pcba_ai_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_labor_hour_pcba_ai_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_labor_hour_pcba_ai_team_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, false)]
public class TaktPcbaAiLaborHour : TaktCompanyEntityBase
{

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
    /// 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
    /// </summary>
    [SugarColumn(ColumnName = "std_capacity", ColumnDescription = "标准产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal StdCapacity { get; set; } = 0;

    /// <summary>
    /// 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
    /// </summary>
    [SugarColumn(ColumnName = "prod_actual_qty", ColumnDescription = "实际生产数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = false, DefaultValue = "0")]
    public decimal ProdActualQty { get; set; } = 0;

    /// <summary>
    /// 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
    /// </summary>
    [SugarColumn(ColumnName = "input_minutes", ColumnDescription = "投入工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal InputMinutes { get; set; } = 0;

    /// <summary>
    /// 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
    /// </summary>
    [SugarColumn(ColumnName = "downtime_minutes", ColumnDescription = "停线损失工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
    /// </summary>
    [SugarColumn(ColumnName = "confirm_minutes", ColumnDescription = "报工工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConfirmMinutes { get; set; } = 0;

    /// <summary>
    /// 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
    /// </summary>
    [SugarColumn(ColumnName = "actual_minutes", ColumnDescription = "实际工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualMinutes { get; set; } = 0;
}
