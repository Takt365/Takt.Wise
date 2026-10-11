// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细（产出子表）实体，按生产时段记录实际产量、停线、达成率等
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报明细（产出子表）实体（按组立日报ID + 生产时段 + 行号唯一）
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_assy_detail", "组立日报明细表")]
[SugarIndex("ix_assy_output_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_assy_output_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_detail_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AssyOutputId), OrderByType.Asc, nameof(TimePeriod), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktAssyOutputDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 组立日报ID（主表主键；与 TimePeriod、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "assy_output_id", ColumnDescription = "组立日报ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10；与 AssyOutputId、TimePeriod 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段（固定值；与 AssyOutputId、LineNumber 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "time_period", ColumnDescription = "生产时段", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
    /// </summary>
    [SugarColumn(ColumnName = "std_capacity", ColumnDescription = "标准产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StdCapacity { get; set; } = 0;

    /// <summary>
    /// 实际生产数量
    /// </summary>
    [SugarColumn(ColumnName = "prod_actual_qty", ColumnDescription = "实际生产数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = false, DefaultValue = "0")]
    public decimal ProdActualQty { get; set; } = 0;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "downtime_minutes", ColumnDescription = "停线时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）
    /// </summary>
    [SugarColumn(ColumnName = "downtime_reason", ColumnDescription = "停线原因", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DowntimeReason { get; set; }

    /// <summary>
    /// 停线说明（停线原因选定后的补充详细说明）
    /// </summary>
    [SugarColumn(ColumnName = "downtime_description", ColumnDescription = "停线说明", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DowntimeDescription { get; set; }

    /// <summary>
    /// 未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）
    /// </summary>
    [SugarColumn(ColumnName = "unachieved_reason", ColumnDescription = "未达成原因", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? UnachievedReason { get; set; }

    /// <summary>
    /// 未达成说明（未达成原因选定后的补充详细说明）
    /// </summary>
    [SugarColumn(ColumnName = "unachieved_description", ColumnDescription = "未达成说明", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? UnachievedDescription { get; set; }

    /// <summary>
    /// 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
    /// </summary>
    [SugarColumn(ColumnName = "input_minutes", ColumnDescription = "投入工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal InputMinutes { get; set; } = 0;

    /// <summary>
    /// 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
    /// </summary>
    [SugarColumn(ColumnName = "actual_minutes", ColumnDescription = "实际工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualMinutes { get; set; } = 0;

    /// <summary>
    /// 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
    /// </summary>
    [SugarColumn(ColumnName = "indirect_minutes", ColumnDescription = "间接工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectMinutes { get; set; } = 0;

    /// <summary>
    /// 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
    /// </summary>
    [SugarColumn(ColumnName = "confirm_minutes", ColumnDescription = "报工工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConfirmMinutes { get; set; } = 0;

    /// <summary>
    /// 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
    /// </summary>
    [SugarColumn(ColumnName = "mixed_prod", ColumnDescription = "混合生产", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
    /// </summary>
    [SugarColumn(ColumnName = "achievement_rate", ColumnDescription = "达成率", ColumnDataType = "decimal", Length = 7, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AchievementRate { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 组立日报（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(AssyOutputId))]
    public TaktAssyOutput? AssyOutput { get; set; }
}
