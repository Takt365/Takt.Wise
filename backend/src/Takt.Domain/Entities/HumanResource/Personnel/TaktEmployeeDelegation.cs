// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegation.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工代理关系实体，独立记录所有代理场景
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工代理关系实体
/// 独立记录所有代理场景（部门代理、岗位代理、审批代理等）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_delegation", "员工代理关系表")]
[SugarIndex("ix_employee_delegation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_delegation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_employee_delegation_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OriginalEmployeeId), OrderByType.Asc, nameof(ProxyEmployeeId), OrderByType.Asc, nameof(DelegationType), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, true)]
public class TaktEmployeeDelegation : TaktCompanyEntityBase
{
    /// <summary>
    /// 代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "proxy_employee_id", ColumnDescription = "代理人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long ProxyEmployeeId { get; set; }
    /// <summary>
    /// 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "proxy_employee_code", ColumnDescription = "代理人编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string ProxyEmployeeCode { get; set; } = string.Empty;
    /// <summary>
    /// 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "proxy_employee_name", ColumnDescription = "代理人姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ProxyEmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 被代理人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "original_employee_id", ColumnDescription = "被代理人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long OriginalEmployeeId { get; set; }
    /// <summary>
    /// 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "original_employee_code", ColumnDescription = "被代理人编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string OriginalEmployeeCode { get; set; } = string.Empty;
    /// <summary>
    /// 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "original_employee_name", ColumnDescription = "被代理人姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string OriginalEmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
    /// </summary>
    [SugarColumn(ColumnName = "delegation_type", ColumnDescription = "代理类型", ColumnDataType = "int", IsNullable = false)]
    public int DelegationType { get; set; }
    /// <summary>
    /// 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
    /// </summary>
    [SugarColumn(ColumnName = "scope_type", ColumnDescription = "代理范围类型", ColumnDataType = "int", IsNullable = false)]
    public int ScopeType { get; set; }
    /// <summary>
    /// 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
    /// </summary>
    [SugarColumn(ColumnName = "scope_id", ColumnDescription = "代理范围ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? ScopeId { get; set; }
    /// <summary>
    /// 代理原因（如休假、出差、培训、岗位空缺、病假等）
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "代理原因", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// 代理开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "代理开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 代理结束时间（null=长期有效，直到手动删除）
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "代理结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndDate { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(OriginalEmployeeId))]
    public TaktEmployee? OriginalEmployee { get; set; }

    /// <summary>
    /// 代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ProxyEmployeeId))]
    public TaktEmployee? ProxyEmployee { get; set; }
}
