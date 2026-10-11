// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktRoleCompany.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色公司关联实体，定义角色可访问的公司范围
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Accounting.Financial;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 角色公司关联实体
/// 定义角色可访问的公司范围（数据权限控制）
/// 例如：角色"财务经理"可访问公司1000、2300、2400
/// </summary>
[SugarTable("takt_identity_role_company", "角色公司关联表")]
[SugarIndex("ix_role_company_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_role_company_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_role_company_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, true)]
[SugarIndex("ix_role_company_role", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, false)]
public class TaktRoleCompany : TaktCompanyEntityBase
{
    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", ColumnDataType = "bigint", IsNullable = false)]
    public long RoleId { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 角色（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoleId), nameof(TaktRole.Id))]
    public TaktRole Role { get; set; } = null!;

    /// <summary>
    /// 可访问公司（多对一，按 CompanyCode 关联）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(CompanyCode), nameof(TaktCompany.CompanyCode))]
    public TaktCompany Company { get; set; } = null!;
}
