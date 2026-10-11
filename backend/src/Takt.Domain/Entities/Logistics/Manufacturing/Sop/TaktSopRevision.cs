#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopRevision.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 版本（ECN 锁定、班组长确认、受控 PDF）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 版本实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_revision", "SOP版本表")]
[SugarIndex("ix_sop_revision_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_revision_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_revision_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SopId), OrderByType.Asc, nameof(Revision), OrderByType.Asc, true)]
public class TaktSopRevision : TaktCompanyEntityBase
{
    /// <summary>
    /// SOP 文档头 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sop_id", ColumnDescription = "SOP文档头ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// 版本号（主版本.次版本，如 1.0、A.01）
    /// </summary>
    [SugarColumn(ColumnName = "revision", ColumnDescription = "版本号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 受控 PDF URL
    /// </summary>
    [SugarColumn(ColumnName = "file_url", ColumnDescription = "受控PDF URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? FileUrl { get; set; }

    /// <summary>
    /// 变更说明
    /// </summary>
    [SugarColumn(ColumnName = "change_desc", ColumnDescription = "变更说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ChangeDesc { get; set; }

    /// <summary>
    /// 关联 ECN 主表 ID（选项 TaktEcs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "ecn_id", ColumnDescription = "ECN主表ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnId { get; set; }

    /// <summary>
    /// 是否锁定（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_locked", ColumnDescription = "是否锁定", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsLocked { get; set; } = 0;

    /// <summary>
    /// 是否强制班组长确认（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "force_leader_ack", ColumnDescription = "是否强制班组长确认", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ForceLeaderAck { get; set; } = 0;

    /// <summary>
    /// 版本状态（字典 sys_lifecycle_status；1=编制中，2=审核中，3=已生效，4=已废止）
    /// </summary>
    [SugarColumn(ColumnName = "revision_status", ColumnDescription = "版本状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RevisionStatus { get; set; } = 1;

    /// <summary>
    /// 生效规则（字典 logistics_sop_effective_rule；1=立即生效，2=按工单生效）
    /// </summary>
    [SugarColumn(ColumnName = "effective_rule", ColumnDescription = "生效规则", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EffectiveRule { get; set; } = 2;

    /// <summary>
    /// SOP 文档头
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SopId))]
    public TaktSopDoc? SopDoc { get; set; }

    /// <summary>
    /// 多语言正文
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopContent.RevisionId))]
    public List<TaktSopContent>? Contents { get; set; }
}
