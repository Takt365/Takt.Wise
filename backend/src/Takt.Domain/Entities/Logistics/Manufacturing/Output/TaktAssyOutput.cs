// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutput.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报（产出）主表实体，按工厂、生产日期、生产线等记录生产订单与标准产能
// 计算公式：达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%（见 TaktProductionStatHelper.CalculateAchievementRatePercent）
// 标准产能：小时产能 = DirectLabor×60÷StdMinutes×标准生产稼动率(%)；StdMinutes 取自标准工序时间；稼动率取自 TaktStandardOperationRate
// 明细参考：明细标准产能无产量无报工为 0；有产量无报工为表头小时产能；有报工为报工工时÷标准工时×稼动率；明细达成率(%) = 实际生产数量 ÷ 明细标准产能 × 100%
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报（产出）主表实体
/// <para>业务唯一键：TenantCode+CompanyCode+TeamCode+ProdCategory+ProdDate+ProdOrderCode。</para>
/// 达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_assy", "组立日报表")]
[SugarIndex("ix_assy_output_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_assy_output_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_team_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_prod_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, false)]
public class TaktAssyOutput : TaktCompanyEntityBase
{

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP；与租户、TeamCode、ProdDate、ProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "prod_category", ColumnDescription = "生产类别", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期（与 TeamCode、ProdCategory、ProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "prod_date", ColumnDescription = "生产日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options?teamCategory=A；DictValue=TeamCode，ExtValue=PlantCode；与租户、ProdCategory、ProdDate、ProdOrderCode 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", Length = 8, ColumnDataType = "nvarchar", IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    [SugarColumn(ColumnName = "direct_labor", ColumnDescription = "直接人员", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    [SugarColumn(ColumnName = "indirect_labor", ColumnDescription = "间接人员", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IndirectLabor { get; set; } = 0;

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
    /// 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode；与租户、TeamCode、ProdCategory、ProdDate 组成唯一键）
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
    /// 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
    /// </summary>
    [SugarColumn(ColumnName = "std_minutes", ColumnDescription = "标准工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StdMinutes { get; set; } = 0;

    /// <summary>
    /// 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
    /// </summary>
    [SugarColumn(ColumnName = "std_capacity", ColumnDescription = "标准产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StdCapacity { get; set; } = 0;

    /// <summary>
    /// 组立日报明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktAssyOutputDetail.AssyOutputId))]
    public List<TaktAssyOutputDetail>? AssyOutputDetails { get; set; }
}
