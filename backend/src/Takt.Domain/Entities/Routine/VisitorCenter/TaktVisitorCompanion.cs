// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.VisitorCenter
// 文件名称：TaktVisitorCompanion.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：来访人员子实体，记录部门、职称与姓名
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.VisitorCenter;

/// <summary>
/// 来访人员子实体（部门、职称、姓名）
/// </summary>
[SugarTable("takt_routine_visitor_center_companion", "来访人员表")]
[SugarIndex("ix_visitor_companion_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_visitor_companion_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_visitor_companion_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VisitorId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_visitor_companion_visitor_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VisitorId), OrderByType.Asc, false)]
public class TaktVisitorCompanion : TaktCompanyEntityBase
{
    /// <summary>
    /// 来访记录 ID（选项 TaktVisitors/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "visitor_id", ColumnDescription = "来访记录ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 部门
    /// </summary>
    [SugarColumn(ColumnName = "department", ColumnDescription = "部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// 职称
    /// </summary>
    [SugarColumn(ColumnName = "job_title", ColumnDescription = "职称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// 来访人员姓名
    /// </summary>
    [SugarColumn(ColumnName = "companion_name", ColumnDescription = "来访人员姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CompanionName { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 来访记录（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(VisitorId), nameof(TaktVisitor.Id))]
    public TaktVisitor? Visitor { get; set; }
}
