// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Middleware
// 文件名称：TaktCsrfProtectionMiddleware.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：CSRF 防护中间件（Bearer 令牌请求跳过；Cookie 会话校验防伪令牌）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Takt.Infrastructure.Extensions;
using Takt.Shared.Enums;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Middleware;

/// <summary>
/// CSRF 防护中间件
/// </summary>
public class TaktCsrfProtectionMiddleware
{
    /// <summary>
    /// 管道中的下一个中间件委托
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">下一个中间件</param>
    public TaktCsrfProtectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 执行中间件
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="antiforgery">防伪令牌服务</param>
    /// <param name="securityOptions">安全配置</param>
    /// <returns>异步任务</returns>
    public async Task InvokeAsync(
        HttpContext context,
        IAntiforgery antiforgery,
        IOptions<TaktSecurityOptions> securityOptions)
    {
        var options = securityOptions.Value;

        if (!options.CsrfProtection.Enabled
            || ShouldSkipCsrfValidation(context))
        {
            await _next(context);
            return;
        }

        try
        {
            await antiforgery.ValidateRequestAsync(context);
        }
        catch (AntiforgeryValidationException)
        {
            await WriteJsonErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                "CSRF_TOKEN_INVALID",
                "CSRF 校验失败，请刷新页面后重试",
                TaktResultCode.BadRequest);
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// 是否跳过 CSRF 校验（Bearer、安全方法、白名单路径等）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>应跳过时为 true</returns>
    private static bool ShouldSkipCsrfValidation(HttpContext context)
    {
        if (HasBearerAuthorization(context))
        {
            return true;
        }

        var method = context.Request.Method;
        if (HttpMethods.IsGet(method)
            || HttpMethods.IsHead(method)
            || HttpMethods.IsOptions(method)
            || HttpMethods.IsTrace(method))
        {
            return true;
        }

        var path = context.Request.Path.Value ?? string.Empty;
        if (path.StartsWith("/connect", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/hubs", StringComparison.OrdinalIgnoreCase)
            || TaktObservabilityCollectionExtensions.IsObservabilityProbePath(context.Request.Path)
            || path.StartsWith("/scalar", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/api/security/csrf-token", StringComparison.OrdinalIgnoreCase)
            || IsAnonymousSessionAuthPath(path))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 是否为匿名 Cookie 会话认证接口（登录流程中尚未持有 Bearer，且不适用 CSRF 令牌）
    /// </summary>
    /// <param name="path">请求路径</param>
    /// <returns>属于会话登录/登出/验密接口时为 true</returns>
    private static bool IsAnonymousSessionAuthPath(string path)
    {
        return path.Equals("/api/TaktAuths/session/signin", StringComparison.OrdinalIgnoreCase)
            || path.Equals("/api/TaktAuths/session/signout", StringComparison.OrdinalIgnoreCase)
            || path.Equals("/api/TaktAuths/session/verify-password", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否携带 Bearer 授权头（API 令牌模式无需 CSRF）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>Authorization 以 Bearer 开头时为 true</returns>
    private static bool HasBearerAuthorization(HttpContext context)
    {
        var authorization = context.Request.Headers.Authorization.ToString();
        return authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 写入统一 JSON 错误响应
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="statusCode">HTTP 状态码</param>
    /// <param name="errorCode">业务错误码</param>
    /// <param name="message">错误消息</param>
    /// <param name="resultCode">结果枚举码</param>
    /// <returns>异步任务</returns>
    private static async Task WriteJsonErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string errorCode,
        string message,
        TaktResultCode resultCode)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var result = TaktApiResult<object>.Fail(message, resultCode);
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}
