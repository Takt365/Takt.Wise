#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopAck.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 新版本班组长确认（ECN 变更后强制弹窗）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 确认实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_ack", "SOP确认表")]
[SugarIndex("ix_sop_ack_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_ack_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_ack_revision", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RevisionId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_ack_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSopAck : TaktCompanyEntityBase
{

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sop_id", ColumnDescription = "SOP主档ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "revision_id", ColumnDescription = "SOP版本ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_id", ColumnDescription = "工位ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "acknowledged_by", ColumnDescription = "确认人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AcknowledgedBy { get; set; }
    /// <summary>
    /// 确认人名称（冗余：按 AcknowledgedBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "acknowledged_by_name", ColumnDescription = "确认人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AcknowledgedByName { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    [SugarColumn(ColumnName = "acknowledged_at", ColumnDescription = "确认时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime AcknowledgedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 确认意见
    /// </summary>
    [SugarColumn(ColumnName = "ack_comment", ColumnDescription = "确认意见", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? AckComment { get; set; }

    /// <summary>
    /// SOP 主档
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SopId))]
    public TaktSopDoc? SopDoc { get; set; }

    /// <summary>
    /// SOP 版本
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RevisionId))]
    public TaktSopRevision? Revision { get; set; }

    /// <summary>
    /// 工位
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(WorkstationId))]
    public TaktSopWorkstation? Workstation { get; set; }
}
