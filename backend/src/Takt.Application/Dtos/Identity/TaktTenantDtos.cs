// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktTenantDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Tenant 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTenant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Identity;

// ========================================
// Tenant 响应 DTO
// ========================================

/// <summary>
/// 租户实体 代表系统中的独立租户（第一层数据隔离）
/// 对应前端 TaktTenantDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktTenantDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// TenantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间
    /// </summary>
    public DateTime SubscriptionStartTime { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）
    /// </summary>
    public DateTime SubscriptionEndTime { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int TenantStatus { get; set; } = 0;

}

// ========================================
// Tenant 查询 DTO
// ========================================

/// <summary>
/// Tenant 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTenantQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 租户名称
    /// </summary>
    public string? TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间（范围查询-开始）
    /// </summary>
    public DateTime? SubscriptionStartTimeStart { get; set; }

    /// <summary>
    /// 订阅开始时间（范围查询-结束）
    /// </summary>
    public DateTime? SubscriptionStartTimeEnd { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）（范围查询-开始）
    /// </summary>
    public DateTime? SubscriptionEndTimeStart { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）（范围查询-结束）
    /// </summary>
    public DateTime? SubscriptionEndTimeEnd { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? TenantStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Tenant DTO
// ========================================

/// <summary>
/// 创建Tenant DTO
/// </summary>
public class TaktTenantCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 租户名称
    /// </summary>
    [Required(ErrorMessage = "租户名称不能为空")]
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间
    /// </summary>
    public DateTime SubscriptionStartTime { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）
    /// </summary>
    public DateTime SubscriptionEndTime { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [Required(ErrorMessage = "联系邮箱不能为空")]
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int TenantStatus { get; set; } = 0;

    /// <summary>
    /// 可访问该租户的用户 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
    /// </summary>
    public long[]? UserIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Tenant DTO
// ========================================

/// <summary>
/// 更新Tenant DTO
/// 继承 TaktTenantCreateDto，添加 TenantId 字段
/// </summary>
public class TaktTenantUpdateDto : TaktTenantCreateDto
{
    /// <summary>
    /// TenantID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TenantId { get; set; }

}

// ========================================
// Tenant 状态 DTO
// ========================================

/// <summary>
/// Tenant 状态更新 DTO
/// </summary>
public class TaktTenantStatusDto
{
    /// <summary>
    /// TenantID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TenantId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int TenantStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Tenant 导入模板行 DTO
/// </summary>
public class TaktTenantTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 租户名称
    /// </summary>
    public string? TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间
    /// </summary>
    public DateTime? SubscriptionStartTime { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）
    /// </summary>
    public DateTime? SubscriptionEndTime { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? TenantStatus { get; set; }

    /// <summary>
    /// 可访问该租户的用户 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
    /// </summary>
    public long[]? UserIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Tenant 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTenantImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 租户名称
    /// </summary>
    public string? TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间
    /// </summary>
    public DateTime? SubscriptionStartTime { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）
    /// </summary>
    public DateTime? SubscriptionEndTime { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? TenantStatus { get; set; }

    /// <summary>
    /// 可访问该租户的用户 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
    /// </summary>
    public long[]? UserIds { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Tenant 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTenantExportDto
{
    /// <summary>
    /// TenantID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// 订阅开始时间
    /// </summary>
    public DateTime SubscriptionStartTime { get; set; }

    /// <summary>
    /// 订阅结束时间（9999/12/31 23:59:59表示长期有效）
    /// </summary>
    public DateTime SubscriptionEndTime { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    public string? ContactName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int TenantStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
