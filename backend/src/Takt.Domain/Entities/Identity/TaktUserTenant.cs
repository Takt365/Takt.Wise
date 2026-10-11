// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktUserTenant.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：用户-租户关联实体，用户可访问多个租户（公司隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 用户-租户关联实体
/// 一个用户可关联多个租户；行内 TenantCode 为可访问租户编码；CompanyCode 为公司隔离（RBAC 关联按公司）。
/// </summary>
[SugarTable("takt_identity_user_tenant", "用户-租户关联表")]
[SugarIndex("ix_user_tenant_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_user_tenant_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_user_tenant_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("ix_user_tenant_user", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
[SugarIndex("ix_user_tenant_user_default", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, nameof(IsDefault), OrderByType.Asc, false)]
public class TaktUserTenant : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    public long UserId { get; set; }

    /// <summary>
    /// 是否默认登录租户（字典 sys_yes_no；同一用户在同一公司下仅应有一条为是）
    /// </summary>
    [SugarColumn(ColumnName = "is_default", ColumnDescription = "是否默认租户", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDefault { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 用户（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(UserId), nameof(TaktUser.Id))]
    public TaktUser User { get; set; } = null!;

    /// <summary>
    /// 可访问租户（多对一，按 TenantCode 关联）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(TenantCode), nameof(TaktTenant.TenantCode))]
    public TaktTenant Tenant { get; set; } = null!;
}
