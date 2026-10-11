#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopCall.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工位安灯呼叫（班长/维修/品质，记录响应时长）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 安灯呼叫实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_call", "SOP安灯呼叫表")]
[SugarIndex("ix_sop_call_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_call_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_call_workstation", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WorkstationId), OrderByType.Asc, false)]
public class TaktSopCall : TaktCompanyEntityBase
{

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_id", ColumnDescription = "工位ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_id", ColumnDescription = "执行追溯ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    [SugarColumn(ColumnName = "call_type", ColumnDescription = "呼叫类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CallType { get; set; } = 1;

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "caller_id", ColumnDescription = "呼叫人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CallerId { get; set; }
    /// <summary>
    /// 呼叫人名称（冗余：按 CallerId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "caller_name", ColumnDescription = "呼叫人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? CallerName { get; set; }

    /// <summary>
    /// 呼叫时间
    /// </summary>
    [SugarColumn(ColumnName = "called_at", ColumnDescription = "呼叫时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CalledAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "responded_by", ColumnDescription = "响应人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }
    /// <summary>
    /// 响应人名称（冗余：按 RespondedBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "responded_by_name", ColumnDescription = "响应人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RespondedByName { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    [SugarColumn(ColumnName = "responded_at", ColumnDescription = "响应时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    [SugarColumn(ColumnName = "response_seconds", ColumnDescription = "响应时长秒", ColumnDataType = "int", IsNullable = true)]
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    [SugarColumn(ColumnName = "call_status", ColumnDescription = "呼叫状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CallStatus { get; set; } = 1;

    /// <summary>
    /// 工位
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(WorkstationId))]
    public TaktSopWorkstation? Workstation { get; set; }
}
