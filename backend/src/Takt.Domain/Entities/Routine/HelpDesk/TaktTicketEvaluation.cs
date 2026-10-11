// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktTicketEvaluation.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：工单服务评价实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 工单服务评价（一个工单对应一条评价）
/// </summary>
[SugarTable("takt_routine_help_desk_ticket_evaluation", "工单服务评价表")]
[SugarIndex("ix_ticket_evaluation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_evaluation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_evaluation_ticket_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketId), OrderByType.Asc, true)]
public class TaktTicketEvaluation : TaktCompanyEntityBase
{
    /// <summary>
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_id", ColumnDescription = "工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 综合评分
    /// </summary>
    [SugarColumn(ColumnName = "score", ColumnDescription = "综合评分", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Score { get; set; } = 0;

    /// <summary>
    /// 评价内容
    /// </summary>
    [SugarColumn(ColumnName = "comment", ColumnDescription = "评价内容", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? Comment { get; set; }

    /// <summary>
    /// 评价人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "evaluator_id", ColumnDescription = "评价人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluatorId { get; set; }

    /// <summary>
    /// 评价人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "evaluator_name", ColumnDescription = "评价人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? EvaluatorName { get; set; }

    /// <summary>
    /// 评价时间
    /// </summary>
    [SugarColumn(ColumnName = "evaluated_at", ColumnDescription = "评价时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EvaluatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 工单（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(TicketId))]
    public TaktTicket? Ticket { get; set; }
}
