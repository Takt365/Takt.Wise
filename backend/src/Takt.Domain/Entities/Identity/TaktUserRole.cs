// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktUserRole.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户-角色关联实体，定义用户拥有哪些角色（公司隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 用户-角色关联实体
/// 定义用户拥有哪些角色（RBAC 核心关联表；租户+公司隔离）
/// </summary>
[SugarTable("takt_identity_user_role", "用户-角色关联表")]
[SugarIndex("ix_user_role_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_user_role_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_user_role_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, true)]
[SugarIndex("ix_user_role_role", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, false)]
[SugarIndex("ix_user_role_user", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
public class TaktUserRole : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    public long UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", ColumnDataType = "bigint", IsNullable = false)]
    public long RoleId { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 用户（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(UserId), nameof(TaktUser.Id))]
    public TaktUser User { get; set; } = null!;

    /// <summary>
    /// 角色（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoleId), nameof(TaktRole.Id))]
    public TaktRole Role { get; set; } = null!;
}
