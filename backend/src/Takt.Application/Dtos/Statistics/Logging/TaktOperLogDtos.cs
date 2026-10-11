// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktOperLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：OperLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktOperLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Statistics.Logging;

// ========================================
// OperLog 响应 DTO
// ========================================

/// <summary>
/// 操作日志实体
/// 对应前端 TaktOperLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktOperLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// OperLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OperLogId { get; set; }

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作模块（如：用户管理、部门管理）
    /// </summary>
    public string OperModule { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，默认 unknown）
    /// </summary>
    public string OperType { get; set; } = string.Empty;

    /// <summary>
    /// 操作方法（如：TaktUserService.CreateUserAsync）
    /// </summary>
    public string OperMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求方式（GET、POST、PUT、DELETE 等）
    /// </summary>
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 操作 URL（含查询字符串）
    /// </summary>
    public string OperUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
    /// </summary>
    public string RequestParam { get; set; } = string.Empty;

    /// <summary>
    /// 返回结果 JSON（当前操作出参/响应摘要）
    /// </summary>
    public string JsonResult { get; set; } = string.Empty;

    /// <summary>
    /// 错误消息（失败时；成功为空串）
    /// </summary>
    public string ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（业务操作发生时刻）
    /// </summary>
    public DateTime OperTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ElapsedTime { get; set; } = 0;

    /// <summary>
    /// 操作状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus OperStatus { get; set; }

}

// ========================================
// OperLog 查询 DTO
// ========================================

/// <summary>
/// OperLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktOperLogQueryDto : TaktPagedQuery
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
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作模块（如：用户管理、部门管理）
    /// </summary>
    public string? OperModule { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，默认 unknown）
    /// </summary>
    public string? OperType { get; set; } = string.Empty;

    /// <summary>
    /// 操作方法（如：TaktUserService.CreateUserAsync）
    /// </summary>
    public string? OperMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求方式（GET、POST、PUT、DELETE 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 操作 URL（含查询字符串）
    /// </summary>
    public string? OperUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
    /// </summary>
    public string? RequestParam { get; set; } = string.Empty;

    /// <summary>
    /// 返回结果 JSON（当前操作出参/响应摘要）
    /// </summary>
    public string? JsonResult { get; set; } = string.Empty;

    /// <summary>
    /// 错误消息（失败时；成功为空串）
    /// </summary>
    public string? ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string? OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    public string? OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string? Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string? Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    public string? DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（业务操作发生时刻）（范围查询-开始）
    /// </summary>
    public DateTime? OperTimeStart { get; set; }

    /// <summary>
    /// 操作时间（业务操作发生时刻）（范围查询-结束）
    /// </summary>
    public DateTime? OperTimeEnd { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int? ElapsedTime { get; set; }

    /// <summary>
    /// 操作状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus? OperStatus { get; set; }

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
// 创建OperLog DTO
// ========================================

/// <summary>
/// 创建OperLog DTO
/// </summary>
public class TaktOperLogCreateDto
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
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    [Required(ErrorMessage = "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作模块（如：用户管理、部门管理）
    /// </summary>
    [Required(ErrorMessage = "操作模块（如：用户管理、部门管理）不能为空")]
    public string OperModule { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "操作类型（TaktConstants.OperType，默认 unknown）不能为空")]
    public string OperType { get; set; } = string.Empty;

    /// <summary>
    /// 操作方法（如：TaktUserService.CreateUserAsync）
    /// </summary>
    [Required(ErrorMessage = "操作方法（如：TaktUserService.CreateUserAsync）不能为空")]
    public string OperMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求方式（GET、POST、PUT、DELETE 等）
    /// </summary>
    [Required(ErrorMessage = "请求方式（GET、POST、PUT、DELETE 等）不能为空")]
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 操作 URL（含查询字符串）
    /// </summary>
    [Required(ErrorMessage = "操作 URL（含查询字符串）不能为空")]
    public string OperUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
    /// </summary>
    [Required(ErrorMessage = "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）不能为空")]
    public string RequestParam { get; set; } = string.Empty;

    /// <summary>
    /// 返回结果 JSON（当前操作出参/响应摘要）
    /// </summary>
    [Required(ErrorMessage = "返回结果 JSON（当前操作出参/响应摘要）不能为空")]
    public string JsonResult { get; set; } = string.Empty;

    /// <summary>
    /// 错误消息（失败时；成功为空串）
    /// </summary>
    [Required(ErrorMessage = "错误消息（失败时；成功为空串）不能为空")]
    public string ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    [Required(ErrorMessage = "操作 IP不能为空")]
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    [Required(ErrorMessage = "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）不能为空")]
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [Required(ErrorMessage = "用户代理（User-Agent）不能为空")]
    public string UserAgent { get; set; } = string.Empty;

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
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "登录设备（TaktConstants.DeviceType，默认 unknown）不能为空")]
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（业务操作发生时刻）
    /// </summary>
    public DateTime OperTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ElapsedTime { get; set; } = 0;

    /// <summary>
    /// 操作状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus OperStatus { get; set; }

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
// 更新OperLog DTO
// ========================================

/// <summary>
/// 更新OperLog DTO
/// 继承 TaktOperLogCreateDto，添加 OperLogId 字段
/// </summary>
public class TaktOperLogUpdateDto : TaktOperLogCreateDto
{
    /// <summary>
    /// OperLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OperLogId { get; set; }

}

// ========================================
// OperLog 状态 DTO
// ========================================

/// <summary>
/// OperLog 状态更新 DTO
/// </summary>
public class TaktOperLogStatusDto
{
    /// <summary>
    /// OperLogID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OperLogId { get; set; }

    /// <summary>
    /// 操作状态（0=失败，1=成功）
    /// </summary>
    [Required(ErrorMessage = "操作状态（0=失败，1=成功）不能为空")]
    public TaktExecuteStatus OperStatus { get; set; }
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// OperLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktOperLogExportDto
{
    /// <summary>
    /// OperLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OperLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作模块（如：用户管理、部门管理）
    /// </summary>
    public string OperModule { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，默认 unknown）
    /// </summary>
    public string OperType { get; set; } = string.Empty;

    /// <summary>
    /// 操作方法（如：TaktUserService.CreateUserAsync）
    /// </summary>
    public string OperMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求方式（GET、POST、PUT、DELETE 等）
    /// </summary>
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 操作 URL（含查询字符串）
    /// </summary>
    public string OperUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
    /// </summary>
    public string RequestParam { get; set; } = string.Empty;

    /// <summary>
    /// 返回结果 JSON（当前操作出参/响应摘要）
    /// </summary>
    public string JsonResult { get; set; } = string.Empty;

    /// <summary>
    /// 错误消息（失败时；成功为空串）
    /// </summary>
    public string ErrorMsg { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（业务操作发生时刻）
    /// </summary>
    public DateTime OperTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ElapsedTime { get; set; } = 0;

    /// <summary>
    /// 操作状态（0=失败，1=成功）
    /// </summary>
    public TaktExecuteStatus OperStatus { get; set; }

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
