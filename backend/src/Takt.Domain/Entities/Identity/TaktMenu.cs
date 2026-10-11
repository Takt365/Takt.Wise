// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktMenu.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单实体，代表系统菜单和权限（树形结构）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 菜单实体
/// 代表系统菜单和权限（树形结构）
/// 支持目录、菜单、按钮三种类型
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_identity_menu", "菜单表")]
[SugarIndex("ix_menu_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_menu_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_menu_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(MenuCode), OrderByType.Asc, true)]
[SugarIndex("ix_menu_parent", nameof(TenantCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
public class TaktMenu : TaktTenantCoreEntityBase
{    /// <summary>
    /// 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
    /// </summary>
    [SugarColumn(ColumnName = "menu_code", ColumnDescription = "菜单编码", ColumnDataType = "varchar", Length = 120, IsNullable = false)]
    public string MenuCode { get; set; } = string.Empty;
    /// <summary>
    /// 菜单名称
    /// </summary>
    [SugarColumn(ColumnName = "menu_name", ColumnDescription = "菜单名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string MenuName { get; set; } = string.Empty;
    /// <summary>
    /// 国际化键（用于多语言支持；varchar Length=140）
    /// </summary>
    [SugarColumn(ColumnName = "i18n_key", ColumnDescription = "国际化键", ColumnDataType = "varchar", Length = 140, IsNullable = false, DefaultValue = "")]
    public string I18nKey { get; set; } = string.Empty;
    /// <summary>
    /// 菜单图标
    /// </summary>
    [SugarColumn(ColumnName = "icon", ColumnDescription = "菜单图标", ColumnDataType = "varchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 父菜单ID（0表示根菜单）
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父菜单ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentId { get; set; } = 0;
    /// <summary>
    /// 层级（1=一级菜单，2=二级菜单，以此类推）
    /// </summary>
    [SugarColumn(ColumnName = "level", ColumnDescription = "层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Level { get; set; } = 1;
    /// <summary>
    /// 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
    /// </summary>
    [SugarColumn(ColumnName = "menu_path", ColumnDescription = "菜单路径", ColumnDataType = "varchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string MenuPath { get; set; } = string.Empty;
    /// <summary>
    /// 是否叶子节点（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_leaf", ColumnDescription = "是否叶子节点", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsLeaf { get; set; } = 0;
    /// <summary>
    /// 菜单类型（字典 sys_menu_type；0=目录，1=菜单，2=按钮）
    /// </summary>
    [SugarColumn(ColumnName = "menu_type", ColumnDescription = "菜单类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MenuType { get; set; } = 1;
    /// <summary>
    /// 权限标识（格式：module:resource:action）
    /// </summary>
    [SugarColumn(ColumnName = "permission", ColumnDescription = "权限标识", ColumnDataType = "varchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string Permission { get; set; } = string.Empty;
    /// <summary>
    /// 路由地址（前端路由）
    /// </summary>
    [SugarColumn(ColumnName = "route_path", ColumnDescription = "路由地址", ColumnDataType = "varchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string RoutePath { get; set; } = string.Empty;
    /// <summary>
    /// 组件路径（前端组件路径）
    /// </summary>
    [SugarColumn(ColumnName = "component_path", ColumnDescription = "组件路径", ColumnDataType = "varchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string ComponentPath { get; set; } = string.Empty;
    /// <summary>
    /// 是否外部链接（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_external", ColumnDescription = "是否外部链接", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsExternal { get; set; } = 0;
    /// <summary>
    /// 外部链接地址
    /// </summary>
    [SugarColumn(ColumnName = "external_url", ColumnDescription = "外部链接地址", ColumnDataType = "varchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string ExternalUrl { get; set; } = string.Empty;
    /// <summary>
    /// 是否缓存（字典 sys_yes_no；前端 keep-alive）
    /// </summary>
    [SugarColumn(ColumnName = "is_cached", ColumnDescription = "是否缓存", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsCached { get; set; } = 0;
    /// <summary>
    /// 是否显示（字典 sys_yes_no；0=隐藏，1=显示）
    /// </summary>
    [SugarColumn(ColumnName = "is_visible", ColumnDescription = "是否显示", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsVisible { get; set; } = 1;
    /// <summary>
    /// 内置（字典 sys_yes_no；种子菜单为内置，不允许删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 菜单描述
    /// </summary>
    [SugarColumn(ColumnName = "menu_description", ColumnDescription = "菜单描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string MenuDescription { get; set; } = string.Empty;
    /// <summary>
    /// 排序号（回填）（同级菜单排序）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "menu_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MenuStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleMenu.MenuId))]
    public List<TaktRoleMenu>? RoleMenus { get; set; }

}
