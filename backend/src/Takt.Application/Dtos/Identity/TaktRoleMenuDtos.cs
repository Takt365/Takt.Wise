// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktRoleMenuDtos.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktRoleMenu RBAC 关联 DTO（仅列表，分配走 TaktRbacsController）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Takt.Shared.Helpers;

namespace Takt.Application.Dtos.Identity;

/// <summary>
/// 角色-菜单关联列表 DTO（对应 TaktRoleMenu）
/// </summary>
public class TaktRoleMenuDto : TaktCompanyDtoBase
{
    /// <summary>
    /// 关联主键（适配实体 Id）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleMenuId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色名称（填充字段）
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// 菜单ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MenuId { get; set; }

    /// <summary>
    /// 菜单名称（填充字段）
    /// </summary>
    public string? MenuName { get; set; }

    /// <summary>
    /// 菜单父级 Id（填充：TaktMenu.ParentId；0=根；分配弹窗构已分配树用）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单类型（填充：TaktMenu.MenuType；0=目录 1=页面 2=按钮）
    /// </summary>
    public int MenuType { get; set; }

    /// <summary>
    /// 菜单物化路径（填充：TaktMenu.MenuPath，如 /1/3/5/）
    /// </summary>
    public string? MenuPath { get; set; }

    /// <summary>
    /// 菜单排序（填充：TaktMenu.SortOrder；分配弹窗左右树同序）
    /// </summary>
    public int SortOrder { get; set; }
}
