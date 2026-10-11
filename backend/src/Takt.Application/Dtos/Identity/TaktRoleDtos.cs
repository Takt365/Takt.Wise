// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktRoleDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Role 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktRole 生成，请按需审阅）
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
// Role 响应 DTO
// ========================================

/// <summary>
/// 角色实体 代表系统角色（RBAC权限模型）
/// 对应前端 TaktRoleDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktRoleDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// RoleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    public int DataScope { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int RoleStatus { get; set; } = 0;

    /// <summary>
    /// 角色菜单权限关联（RBAC，表 takt_identity_role_menu）
    /// （子表：TaktRoleMenu）
    /// </summary>
    public List<TaktRoleMenuDto>? RoleMenus { get; set; }

    /// <summary>
    /// 角色可访问公司关联（RBAC，表 takt_identity_role_company）
    /// （子表：TaktRoleCompany）
    /// </summary>
    public List<TaktRoleCompanyDto>? RoleCompanies { get; set; }

    /// <summary>
    /// 自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept）
    /// （子表：TaktRoleDept）
    /// </summary>
    public List<TaktRoleDeptDto>? RoleDepts { get; set; }

    /// <summary>
    /// 拥有该角色的用户关联（RBAC，表 takt_identity_user_role）
    /// （子表：TaktUserRole）
    /// </summary>
    public List<TaktUserRoleDto>? UserRoles { get; set; }

}

// ========================================
// Role 查询 DTO
// ========================================

/// <summary>
/// Role 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktRoleQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    public string? RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string? RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    public int? DataScope { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? RoleStatus { get; set; }

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
// 创建Role DTO
// ========================================

/// <summary>
/// 创建Role DTO
/// </summary>
public class TaktRoleCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    [Required(ErrorMessage = "角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）不能为空")]
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    [Required(ErrorMessage = "角色名称不能为空")]
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    public int DataScope { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int RoleStatus { get; set; } = 0;

    /// <summary>
    /// 角色菜单权限关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleMenuIds { get; set; }

    /// <summary>
    /// 角色可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? RoleCompanyCodes { get; set; }

    /// <summary>
    /// 自定义数据权限关联部门（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleDeptIds { get; set; }

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
// 更新Role DTO
// ========================================

/// <summary>
/// 更新Role DTO
/// 继承 TaktRoleCreateDto，添加 RoleId 字段
/// </summary>
public class TaktRoleUpdateDto : TaktRoleCreateDto
{
    /// <summary>
    /// RoleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

}

// ========================================
// Role 状态 DTO
// ========================================

/// <summary>
/// Role 状态更新 DTO
/// </summary>
public class TaktRoleStatusDto
{
    /// <summary>
    /// RoleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int RoleStatus { get; set; } = 0;
}

// ========================================
// Role 内置 DTO
// ========================================

/// <summary>
/// Role 内置更新 DTO
/// </summary>
public class TaktRoleBuiltInDto
{
    /// <summary>
    /// RoleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [Required(ErrorMessage = "内置不能为空")]
    public int IsBuiltIn { get; set; } = 0;
}

// ========================================
// Role 排序 DTO
// ========================================

/// <summary>
/// Role 排序更新 DTO
/// </summary>
public class TaktRoleSortDto
{
    /// <summary>
    /// RoleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Role 导入模板行 DTO
/// </summary>
public class TaktRoleTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    public string? RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string? RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    public int? DataScope { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? RoleStatus { get; set; }

    /// <summary>
    /// 角色菜单权限关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleMenuIds { get; set; }

    /// <summary>
    /// 角色可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? RoleCompanyCodes { get; set; }

    /// <summary>
    /// 自定义数据权限关联部门（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleDeptIds { get; set; }

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
/// Role 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktRoleImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    public string? RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string? RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    public int? DataScope { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? RoleStatus { get; set; }

    /// <summary>
    /// 角色菜单权限关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleMenuIds { get; set; }

    /// <summary>
    /// 角色可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? RoleCompanyCodes { get; set; }

    /// <summary>
    /// 自定义数据权限关联部门（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleDeptIds { get; set; }

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
/// Role 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktRoleExportDto
{
    /// <summary>
    /// RoleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
    /// </summary>
    public int DataScope { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? RoleDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int RoleStatus { get; set; } = 0;

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
