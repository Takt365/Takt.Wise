// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.VisitorCenter
// 文件名称：TaktVisitor.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：来访接待主实体，记录来访公司及参访时段
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.VisitorCenter;

/// <summary>
/// 来访接待主实体（来访公司及参访起止时间）
/// </summary>
[SugarTable("takt_routine_visitor_center", "来访接待表")]
[SugarIndex("ix_visitor_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_visitor_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_visitor_company_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VisitorCompanyName), OrderByType.Asc, false)]
[SugarIndex("ix_visitor_start_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VisitStartTime), OrderByType.Asc, false)]
[SugarIndex("ix_visitor_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VisitorCompanyName), OrderByType.Asc, nameof(VisitStartTime), OrderByType.Asc, true)]
public class TaktVisitor : TaktCompanyEntityBase
{
    /// <summary>
    /// 来访公司名称
    /// </summary>
    [SugarColumn(ColumnName = "visitor_company_name", ColumnDescription = "来访公司名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string VisitorCompanyName { get; set; } = string.Empty;
    /// <summary>
    /// 参访开始时间
    /// </summary>
    [SugarColumn(ColumnName = "visit_start_time", ColumnDescription = "参访开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime VisitStartTime { get; set; }
    /// <summary>
    /// 参访结束时间
    /// </summary>
    [SugarColumn(ColumnName = "visit_end_time", ColumnDescription = "参访结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime VisitEndTime { get; set; }
    /// <summary>
    /// 来访人员列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktVisitorCompanion.VisitorId), nameof(Id))]
    public List<TaktVisitorCompanion>? Companions { get; set; }
}
