// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktTicketReply.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：工单回复/会话实体，支撑等待用户回复与处理中沟通
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 工单回复实体（用户与客服会话）
/// </summary>
[SugarTable("takt_routine_help_desk_ticket_reply", "工单回复表")]
[SugarIndex("ix_ticket_reply_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_reply_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_reply_ticket_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketId), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_reply_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
public class TaktTicketReply : TaktCompanyEntityBase
{
    /// <summary>
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_id", ColumnDescription = "工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    [SugarColumn(ColumnName = "author_type", ColumnDescription = "作者类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AuthorType { get; set; } = 0;

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "author_id", ColumnDescription = "作者用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AuthorId { get; set; }

    /// <summary>
    /// 作者姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "author_name", ColumnDescription = "作者姓名", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? AuthorName { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    [SugarColumn(ColumnName = "ticket_reply_content", ColumnDescription = "回复内容", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；0=否 1=是；仅客服可见）
    /// </summary>
    [SugarColumn(ColumnName = "is_internal", ColumnDescription = "是否内部备注", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsInternal { get; set; } = 0;

    /// <summary>
    /// 工单（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(TicketId))]
    public TaktTicket? Ticket { get; set; }
}
