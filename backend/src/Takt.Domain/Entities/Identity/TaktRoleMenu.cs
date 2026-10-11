// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktRoleMenu.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色-菜单关联实体，定义角色拥有哪些菜单/权限（公司隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 角色-菜单关联实体
/// 定义角色拥有哪些菜单/权限（RBAC 核心关联表；租户+公司隔离）
/// </summary>
[SugarTable("takt_identity_role_menu", "角色-菜单关联表")]
[SugarIndex("ix_role_menu_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_role_menu_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_role_menu_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, nameof(MenuId), OrderByType.Asc, true)]
[SugarIndex("ix_role_menu_menu", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MenuId), OrderByType.Asc, false)]
[SugarIndex("ix_role_menu_role", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, false)]
public class TaktRoleMenu : TaktCompanyEntityBase
{
    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", ColumnDataType = "bigint", IsNullable = false)]
    public long RoleId { get; set; }

    /// <summary>
    /// 菜单ID
    /// </summary>
    [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID", ColumnDataType = "bigint", IsNullable = false)]
    public long MenuId { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 角色（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoleId), nameof(TaktRole.Id))]
    public TaktRole Role { get; set; } = null!;

    /// <summary>
    /// 菜单（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MenuId), nameof(TaktMenu.Id))]
    public TaktMenu Menu { get; set; } = null!;
}
