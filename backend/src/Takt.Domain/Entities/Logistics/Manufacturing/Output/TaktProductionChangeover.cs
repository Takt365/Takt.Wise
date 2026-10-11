// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeover.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：生产切换记录实体，记录工厂/类别/日期/班组、切换前后工单与机种、切换时长、仪设/SOP/学习时间及总时间、人数等切换分析维度
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// 生产切换记录实体
/// <para>业务唯一键：TenantCode+CompanyCode+ProdCategory+ChangeoverCategory+ProdDate+TeamCode+CurrentProdOrderCode+ChangeoverProdOrderCode。</para>
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_production_changeover", "生产切换记录表")]
[SugarIndex("ix_production_changeover_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_changeover_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_production_changeover_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ChangeoverCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(CurrentProdOrderCode), OrderByType.Asc, nameof(ChangeoverProdOrderCode), OrderByType.Asc, true)]
public class TaktProductionChangeover : TaktCompanyEntityBase
{

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP；与 ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "prod_category", ColumnDescription = "生产类别", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA；与 ProdCategory、ProdDate、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_category", ColumnDescription = "切换类别", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期（与 ProdCategory、ChangeoverCategory、TeamCode、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "prod_date", ColumnDescription = "生产日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤；与 ProdCategory、ChangeoverCategory、ProdDate、CurrentProdOrderCode、ChangeoverProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", Length = 8, ColumnDataType = "nvarchar", IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、ChangeoverProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "current_prod_order_code", ColumnDescription = "当前工单", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "current_model_code", ColumnDescription = "当前机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤；与 ProdCategory、ChangeoverCategory、ProdDate、TeamCode、CurrentProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_prod_order_code", ColumnDescription = "切换后工单", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_model_code", ColumnDescription = "切换后机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    [SugarColumn(ColumnName = "changeover_count", ColumnDescription = "切换次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_time", ColumnDescription = "切换时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "instrument_setup_time", ColumnDescription = "仪设时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InstrumentSetupTime { get; set; } = 0;

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "total_changeover_time", ColumnDescription = "切换总时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "read_sop_time", ColumnDescription = "读取SOP时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReadSopTime { get; set; } = 0;

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "learning_time", ColumnDescription = "学习时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LearningTime { get; set; } = 0;

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    [SugarColumn(ColumnName = "person_count", ColumnDescription = "人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "total_learning_time", ColumnDescription = "学习总时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalLearningTime { get; set; } = 0;

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    [SugarColumn(ColumnName = "total_sop_time", ColumnDescription = "SOP总时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalSopTime { get; set; } = 0;
}
