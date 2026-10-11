// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Middleware
// 文件名称：TaktExceptionMiddleware.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一异常处理中间件（采集异常并返回标准响应）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Models.Logging;

namespace Takt.Infrastructure.Middleware;

/// <summary>
/// 统一异常处理中间件
/// </summary>
public class TaktExceptionMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">下一个中间件</param>
    public TaktExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 执行中间件
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>异步任务</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var logContext = BuildExceptionContext(context);
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "INTERNAL_ERROR";
        var message = Localize(context, "common.system.internal.error");

        switch (exception)
        {
            case TaktBusinessException businessEx:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = businessEx.ErrorCode ?? errorCode;
                message = businessEx.Message;
                logContext.Action = "business";
                TaktLogger.Warning(
                    logContext,
                    "{HttpPrefix}业务异常: ErrorCode={ErrorCode}, Message={Message}",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    errorCode,
                    message);
                break;

            case UnauthorizedAccessException authEx:
                statusCode = HttpStatusCode.Unauthorized;
                errorCode = "UNAUTHORIZED";
                message = Localize(context, "common.permission.unauthorized");
                logContext.Action = "unauthorized";
                TaktLogger.Warning(
                    logContext,
                    "{HttpPrefix}未授权访问: {Message}",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    authEx.Message);
                break;

            case KeyNotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                errorCode = "NOT_FOUND";
                message = notFoundEx.Message;
                logContext.Action = "not_found";
                TaktLogger.Warning(
                    logContext,
                    "{HttpPrefix}资源未找到: {Message}",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    notFoundEx.Message);
                break;

            case ArgumentException argEx:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "INVALID_ARGUMENT";
                message = argEx.Message;
                logContext.Action = "invalid_argument";
                TaktLogger.Warning(
                    logContext,
                    "{HttpPrefix}参数异常: {Message}",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    argEx.Message);
                break;

            default:
                if (TaktTenantDatabaseHelper.IsInfrastructureFailure(exception))
                {
                    statusCode = HttpStatusCode.BadRequest;
                    var configuration = context.RequestServices.GetService<IConfiguration>();
                    var tenantCode = TaktUserContext.TryResolveTenantCode(context)?.Trim() ?? "000";
                    var tenantDbEx = TaktTenantDatabaseHelper.CreateBusinessException(exception, configuration, tenantCode);
                    errorCode = tenantDbEx.ErrorCode ?? "error.tenant.database.connection";
                    message = tenantDbEx.Message;
                    logContext.Action = "tenant_database";
                    TaktLogger.Warning(
                        logContext,
                        "{HttpPrefix}租户业务库不可用: Tenant={TenantCode}, Message={Message}",
                        TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                        tenantCode,
                        message);
                    break;
                }

                logContext.Action = "unhandled";
                TaktLogger.Error(
                    exception,
                    logContext,
                    "{HttpPrefix}未处理的异常: {ExceptionType}, {Message}",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    exception.GetType().Name,
                    exception.Message);
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var result = TaktApiResult<object>.Fail(message);
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result, jsonOptions));
    }

    private static string Localize(HttpContext context, string key, params object[] args)
    {
        var localizationService = context.RequestServices.GetService<ITaktLocalizationService>();
        if (localizationService == null)
        {
            return key;
        }

        try
        {
            return localizationService.Translate(key, args: args);
        }
        catch
        {
            return key;
        }
    }

    private static TaktLogContext BuildExceptionContext(HttpContext context)
    {
        var principal = TaktUserContext.ResolvePrincipal(context);
        return new TaktLogContext
        {
            Module = "exception",
            RequestId = context.Items["RequestId"]?.ToString(),
            Route = context.Request.Path.Value,
            ClientIp = context.Connection.RemoteIpAddress?.ToString(),
            TenantCode = TaktUserContext.TryResolveTenantCode(context),
            CompanyCode = TaktUserContext.TryResolveCompanyCode(context),
            UserId = TaktUserContext.TryResolveUserId(principal)?.ToString(),
            UserName = TaktUserContext.TryResolveUserName(principal)
        };
    }
}
