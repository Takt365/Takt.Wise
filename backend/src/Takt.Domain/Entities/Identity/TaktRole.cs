// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktRole.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色实体，代表系统角色（RBAC权限模型）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.HumanResource.Organization;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 角色实体
/// 代表系统角色（RBAC权限模型）
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_identity_role", "角色表")]
[SugarIndex("ix_role_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_role_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_role_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(RoleCode), OrderByType.Asc, true)]
public class TaktRole : TaktTenantCoreEntityBase
{    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    [SugarColumn(ColumnName = "role_code", ColumnDescription = "角色编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string RoleCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色名称
    /// </summary>
    [SugarColumn(ColumnName = "role_name", ColumnDescription = "角色名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string RoleName { get; set; } = string.Empty;
    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    [SugarColumn(ColumnName = "data_scope", ColumnDescription = "数据权限范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "4")]
    public int DataScope { get; set; } = 4;
    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 角色描述
    /// </summary>
    [SugarColumn(ColumnName = "role_description", ColumnDescription = "角色描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? RoleDescription { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "role_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RoleStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 角色菜单权限关联（RBAC，表 takt_identity_role_menu）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleMenu.RoleId))]
    public List<TaktRoleMenu>? RoleMenus { get; set; }

    /// <summary>
    /// 角色可访问公司关联（RBAC，表 takt_identity_role_company）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleCompany.RoleId))]
    public List<TaktRoleCompany>? RoleCompanies { get; set; }

    /// <summary>
    /// 自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleDept.RoleId))]
    public List<TaktRoleDept>? RoleDepts { get; set; }

    /// <summary>
    /// 拥有该角色的用户关联（RBAC，表 takt_identity_user_role）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserRole.RoleId))]
    public List<TaktUserRole>? UserRoles { get; set; }

}
