// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktTicketCategoryAssign.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：工单分类默认处理人配置实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 工单分类默认处理人（按 CategoryCode 自动分配处理人）
/// </summary>
[SugarTable("takt_routine_help_desk_ticket_category_assign", "工单分类默认处理人表")]
[SugarIndex("ix_ticket_category_assign_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_category_assign_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_category_assign_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CategoryCode), OrderByType.Asc, false)]
public class TaktTicketCategoryAssign : TaktCompanyEntityBase
{
    /// <summary>
    /// 分类编码（业务编码；与 TaktTicket.CategoryCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "category_code", ColumnDescription = "分类编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 默认处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "assignee_id", ColumnDescription = "默认处理人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeId { get; set; }

    /// <summary>
    /// 默认处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "assignee_name", ColumnDescription = "默认处理人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? AssigneeName { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
}
