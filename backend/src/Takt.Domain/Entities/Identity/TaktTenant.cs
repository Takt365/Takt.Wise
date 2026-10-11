// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktTenant.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：租户实体，代表系统中的独立租户（第一层数据隔离）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 租户实体
/// 代表系统中的独立租户（第一层数据隔离）
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_identity_tenant", "租户表")]
[SugarIndex("ix_tenant_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_tenant_code_unique", nameof(TenantCode), OrderByType.Asc, true)]
public class TaktTenant : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 租户名称
    /// </summary>
    [SugarColumn(ColumnName = "tenant_name", ColumnDescription = "租户名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间
    /// </summary>
    [SugarColumn(ColumnName = "subscription_start_time", ColumnDescription = "订阅开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime SubscriptionStartTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）
    /// </summary>
    [SugarColumn(ColumnName = "subscription_end_time", ColumnDescription = "订阅结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime SubscriptionEndTime { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [SugarColumn(ColumnName = "contact_name", ColumnDescription = "联系人姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactName { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "tenant_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TenantStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 可访问该租户的用户关联（RBAC，表 takt_identity_user_tenant）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserTenant.TenantCode))]
    public List<TaktUserTenant>? UserTenants { get; set; }

}
