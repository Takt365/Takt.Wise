// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktLoginLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：LoginLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktLoginLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Statistics.Logging;

// ========================================
// LoginLog 响应 DTO
// ========================================

/// <summary>
/// 登录日志实体
/// 对应前端 TaktLoginLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktLoginLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// LoginLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LoginLogId { get; set; }

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录方式（TaktConstants.LoginType，如 password=账号密码、refreshtoken=刷新令牌）
    /// </summary>
    public string LoginType { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果（TaktConstants.LoginResult，如 success=成功、passworderror=密码错误）
    /// </summary>
    public string LoginResult { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果消息
    /// </summary>
    public string LoginMessage { get; set; } = string.Empty;

    /// <summary>
    /// 登录IP地址
    /// </summary>
    public string LoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录地点（IP解析，如：中国-广东省-深圳市）
    /// </summary>
    public string LoginLocation { get; set; } = string.Empty;

    /// <summary>
    /// 登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）
    /// </summary>
    public DateTime? LogoutAt { get; set; }

}

// ========================================
// LoginLog 查询 DTO
// ========================================

/// <summary>
/// LoginLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktLoginLogQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录方式（TaktConstants.LoginType，如 password=账号密码、refreshtoken=刷新令牌）
    /// </summary>
    public string? LoginType { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string? Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string? Os { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果（TaktConstants.LoginResult，如 success=成功、passworderror=密码错误）
    /// </summary>
    public string? LoginResult { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果消息
    /// </summary>
    public string? LoginMessage { get; set; } = string.Empty;

    /// <summary>
    /// 登录IP地址
    /// </summary>
    public string? LoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录地点（IP解析，如：中国-广东省-深圳市）
    /// </summary>
    public string? LoginLocation { get; set; } = string.Empty;

    /// <summary>
    /// 登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）（范围查询-开始）
    /// </summary>
    public DateTime? LogoutAtStart { get; set; }

    /// <summary>
    /// 登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）（范围查询-结束）
    /// </summary>
    public DateTime? LogoutAtEnd { get; set; }

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
// 创建LoginLog DTO
// ========================================

/// <summary>
/// 创建LoginLog DTO
/// </summary>
public class TaktLoginLogCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    [Required(ErrorMessage = "用户名（登录账号）不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录方式（TaktConstants.LoginType，如 password=账号密码、refreshtoken=刷新令牌）
    /// </summary>
    [Required(ErrorMessage = "登录方式（TaktConstants.LoginType，如 password=账号密码、refreshtoken=刷新令牌）不能为空")]
    public string LoginType { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "浏览器（TaktConstants.BrowserType，默认 unknown）不能为空")]
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "操作系统（TaktConstants.OperatingSystem，默认 unknown）不能为空")]
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [Required(ErrorMessage = "用户代理（User-Agent）不能为空")]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果（TaktConstants.LoginResult，如 success=成功、passworderror=密码错误）
    /// </summary>
    [Required(ErrorMessage = "登录结果（TaktConstants.LoginResult，如 success=成功、passworderror=密码错误）不能为空")]
    public string LoginResult { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果消息
    /// </summary>
    [Required(ErrorMessage = "登录结果消息不能为空")]
    public string LoginMessage { get; set; } = string.Empty;

    /// <summary>
    /// 登录IP地址
    /// </summary>
    [Required(ErrorMessage = "登录IP地址不能为空")]
    public string LoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录地点（IP解析，如：中国-广东省-深圳市）
    /// </summary>
    [Required(ErrorMessage = "登录地点（IP解析，如：中国-广东省-深圳市）不能为空")]
    public string LoginLocation { get; set; } = string.Empty;

    /// <summary>
    /// 登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）
    /// </summary>
    public DateTime? LogoutAt { get; set; }

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
// 更新LoginLog DTO
// ========================================

/// <summary>
/// 更新LoginLog DTO
/// 继承 TaktLoginLogCreateDto，添加 LoginLogId 字段
/// </summary>
public class TaktLoginLogUpdateDto : TaktLoginLogCreateDto
{
    /// <summary>
    /// LoginLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LoginLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// LoginLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktLoginLogExportDto
{
    /// <summary>
    /// LoginLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LoginLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录方式（TaktConstants.LoginType，如 password=账号密码、refreshtoken=刷新令牌）
    /// </summary>
    public string LoginType { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果（TaktConstants.LoginResult，如 success=成功、passworderror=密码错误）
    /// </summary>
    public string LoginResult { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果消息
    /// </summary>
    public string LoginMessage { get; set; } = string.Empty;

    /// <summary>
    /// 登录IP地址
    /// </summary>
    public string LoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录地点（IP解析，如：中国-广东省-深圳市）
    /// </summary>
    public string LoginLocation { get; set; } = string.Empty;

    /// <summary>
    /// 登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）
    /// </summary>
    public DateTime? LogoutAt { get; set; }

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
