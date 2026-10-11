// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktResultEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：API 结果代码枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// API 结果代码枚举
/// 用于统一 API 响应状态码
/// </summary>
public enum TaktResultCode
{
    /// <summary>
    /// 成功（200 OK）
    /// </summary>
    [Display(Name = "成功")]
    Success = 200,

    /// <summary>
    /// 错误请求（400 Bad Request）
    /// </summary>
    [Display(Name = "错误请求")]
    BadRequest = 400,

    /// <summary>
    /// 未授权（401 Unauthorized）
    /// </summary>
    [Display(Name = "未授权")]
    Unauthorized = 401,

    /// <summary>
    /// 禁止访问（403 Forbidden）
    /// </summary>
    [Display(Name = "禁止访问")]
    Forbidden = 403,

    /// <summary>
    /// 未找到（404 Not Found）
    /// </summary>
    [Display(Name = "未找到")]
    NotFound = 404,

    /// <summary>
    /// 服务器内部错误（500 Internal Server Error）
    /// </summary>
    [Display(Name = "服务器内部错误")]
    InternalServerError = 500,

    /// <summary>
    /// 请求超时（408 Request Timeout）
    /// </summary>
    [Display(Name = "请求超时")]
    RequestTimeout = 408,

    /// <summary>
    /// 请求过于频繁（429 Too Many Requests）
    /// </summary>
    [Display(Name = "请求过于频繁")]
    TooManyRequests = 429,

    // ========================================
    // 业务错误代码（1000+）
    // ========================================

    /// <summary>
    /// 业务错误 - 租户不存在
    /// </summary>
    [Display(Name = "租户不存在")]
    TenantNotFound = 1001,

    /// <summary>
    /// 业务错误 - 公司不存在
    /// </summary>
    [Display(Name = "公司不存在")]
    CompanyNotFound = 1002,

    /// <summary>
    /// 业务错误 - 用户不存在
    /// </summary>
    [Display(Name = "用户不存在")]
    UserNotFound = 1003,

    /// <summary>
    /// 业务错误 - 用户名或密码错误
    /// </summary>
    [Display(Name = "用户名或密码错误")]
    InvalidCredentials = 1004,

    /// <summary>
    /// 业务错误 - 用户已锁定
    /// </summary>
    [Display(Name = "用户已锁定")]
    UserLocked = 1005,

    /// <summary>
    /// 业务错误 - 账号已禁用
    /// </summary>
    [Display(Name = "账号已禁用")]
    AccountDisabled = 1006,

    /// <summary>
    /// 业务错误 - 数据不存在
    /// </summary>
    [Display(Name = "数据不存在")]
    DataNotFound = 1007,

    /// <summary>
    /// 业务错误 - 数据已存在
    /// </summary>
    [Display(Name = "数据已存在")]
    DataAlreadyExists = 1008,

    /// <summary>
    /// 业务错误 - 操作失败
    /// </summary>
    [Display(Name = "操作失败")]
    OperationFailed = 1009,

    /// <summary>
    /// 业务错误 - 权限不足
    /// </summary>
    [Display(Name = "权限不足")]
    InsufficientPermission = 1010,

    /// <summary>
    /// 业务错误 - 验证码错误
    /// </summary>
    [Display(Name = "验证码错误")]
    InvalidCaptcha = 1011,

    /// <summary>
    /// 业务错误 - 验证码已过期
    /// </summary>
    [Display(Name = "验证码已过期")]
    CaptchaExpired = 1012
}
