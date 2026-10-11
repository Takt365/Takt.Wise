// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Middleware
// 文件名称：TaktLoggingMiddleware.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一 HTTP 请求日志中间件（采集、格式化、上报）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Services;
using Takt.Infrastructure.SignalR;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logging;

namespace Takt.Infrastructure.Middleware;

/// <summary>
/// 统一 HTTP 请求日志中间件
/// </summary>
/// <remarks>
/// <para>在每个 HTTP 请求生命周期内完成以下工作：</para>
/// <list type="number">
/// <item><description>生成短 <c>RequestId</c> 并写入 HttpContext.Items，供下游关联日志。</description></item>
/// <item><description>基于租户/公司请求头、用户 Claims、客户端 IP 等构建 TaktLogContext，并通过 TaktLogger.BeginScope 注入 Serilog 作用域。</description></item>
/// <item><description>记录请求开始日志；SignalR Hub 协商/WebSocket 等走 TaktSignalRLogging 专用格式。</description></item>
/// <item><description>在 <c>finally</c> 中统计耗时，按状态码与阈值分级输出（5xx Error、4xx Warning、&gt;1s 慢请求 Warning、Hub 完成日志、其余 Debug）。</description></item>
/// <item><description>调用 TaktSqlSugarAuditAop.TryPersistOperLog 将符合条件的写操作 API 持久化到 <c>TaktOperLog</c>（Statistics.Logging 模块 API 与日志表 CUD 由审计 AOP 动态排除）。</description></item>
/// </list>
/// </remarks>
public class TaktLoggingMiddleware
{
    /// <summary>
    /// 慢请求告警阈值（毫秒）；超过该值记录 Warning 级别日志
    /// </summary>
    private const int SlowRequestThresholdMs = 1000;

    /// <summary>
    /// 操作日志请求体最大采样字节数（超出部分截断，防止大 POST 占满内存）
    /// </summary>
    private const int MaxOperLogBodyBytes = 64 * 1024;

    /// <summary>
    /// 管道中的下一个中间件委托
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// 初始化 TaktLoggingMiddleware 实例
    /// </summary>
    /// <param name="next">ASP.NET Core 请求管道中的下一个中间件</param>
    public TaktLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 处理 HTTP 请求：建立日志作用域、记录开始/结束日志，并在请求结束时尝试写入操作日志
    /// </summary>
    /// <param name="context">当前 HTTP 请求上下文</param>
    /// <param name="dbContext">当前租户 SqlSugar 上下文，用于将操作日志写入 <c>takt_statistics_logging_oper_log</c></param>
    /// <returns>表示中间件异步执行的任务</returns>
    public async Task InvokeAsync(HttpContext context, TaktSqlSugarContext dbContext)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];
        context.Items["RequestId"] = requestId;

        var logContext = BuildRequestContext(context, requestId);
        using var scope = TaktLogger.BeginScope(logContext);

        var request = context.Request;
        var pathValue = request.Path.Value ?? string.Empty;
        var isSignalRHubRequest = IsSignalRHubRequest(pathValue);
        var requestBody = await ReadRequestBodyAsync(context);

        if (isSignalRHubRequest)
        {
            TaktLogger.Information(
                logContext,
                "{HttpPrefix}SignalR HTTP 请求开始: {Method} {Path}",
                TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                request.Method,
                request.Path);
        }
        else
        {
            TaktLogger.Information(
                logContext,
                "{HttpPrefix}HTTP 请求开始: {Method} {Path}",
                TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                request.Method,
                request.Path);
        }

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var response = context.Response;
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            logContext.Extra = new Dictionary<string, object?>
            {
                ["StatusCode"] = response.StatusCode,
                ["ElapsedMs"] = elapsedMs
            };

            if (response.StatusCode >= 500)
            {
                TaktLogger.Error(
                    logContext,
                    "{HttpPrefix}HTTP 请求失败: {Method} {Path}, StatusCode={StatusCode}, Elapsed={ElapsedMs}ms",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    request.Method,
                    request.Path,
                    response.StatusCode,
                    elapsedMs);
            }
            else if (response.StatusCode >= 400)
            {
                TaktLogger.Warning(
                    logContext,
                    "{HttpPrefix}HTTP 请求警告: {Method} {Path}, StatusCode={StatusCode}, Elapsed={ElapsedMs}ms",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    request.Method,
                    request.Path,
                    response.StatusCode,
                    elapsedMs);
            }
            else if (elapsedMs > SlowRequestThresholdMs)
            {
                TaktLogger.Warning(
                    logContext,
                    "{HttpPrefix}慢请求: {Method} {Path}, StatusCode={StatusCode}, Elapsed={ElapsedMs}ms",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    request.Method,
                    request.Path,
                    response.StatusCode,
                    elapsedMs);
            }
            else if (isSignalRHubRequest)
            {
                TaktSignalRLogging.LogHubHttpCompleted(
                    logContext,
                    request.Method,
                    pathValue,
                    response.StatusCode,
                    elapsedMs);
            }
            else
            {
                TaktLogger.Debug(
                    logContext,
                    "{HttpPrefix}HTTP 请求完成: {Method} {Path}, StatusCode={StatusCode}, Elapsed={ElapsedMs}ms",
                    TaktLogFormatter.FormatHttpRequestPrefix(logContext),
                    request.Method,
                    request.Path,
                    response.StatusCode,
                    elapsedMs);
            }

            TaktSqlSugarAuditAop.TryPersistOperLog(
                dbContext.Db,
                context,
                elapsedMs,
                response.StatusCode,
                requestBody);
        }
    }

    /// <summary>
    /// 读取可重复使用的请求体文本，供操作日志 <c>RequestParam</c> 字段使用
    /// </summary>
    /// <param name="context">当前 HTTP 请求上下文</param>
    /// <returns>
    /// POST/PUT/PATCH 且存在请求体时的 UTF-8 文本；无正文或非写方法时返回 <see langword="null"/>。
    /// 读取后会将 HttpRequest.Body 位置重置为 0，不影响后续模型绑定。
    /// </returns>
    private static async Task<string?> ReadRequestBodyAsync(HttpContext context)
    {
        var request = context.Request;
        if (request.ContentLength is null or 0)
        {
            return null;
        }

        if (request.Method is not ("POST" or "PUT" or "PATCH"))
        {
            return null;
        }

        // 超大 Body（如设变 10 万+ 明细）禁止读入操作日志，也不触碰 Body（避免与 Kestrel 限流交互）
        if (request.ContentLength > MaxOperLogBodyBytes)
        {
            return $"(body omitted, ContentLength={request.ContentLength})";
        }

        request.EnableBuffering();
        request.Body.Position = 0;
        var buffer = new char[MaxOperLogBodyBytes];
        using var reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 4096,
            leaveOpen: true);
        var readCount = await reader.ReadBlockAsync(buffer, 0, MaxOperLogBodyBytes);
        request.Body.Position = 0;
        if (readCount <= 0)
        {
            return null;
        }

        return new string(buffer, 0, readCount);
    }

    /// <summary>
    /// 判断当前请求是否为 SignalR Hub 相关 HTTP 流量（协商、WebSocket 升级等）
    /// </summary>
    /// <param name="path">请求路径（通常为 PathString.Value）</param>
    /// <returns>路径包含 <c>/hubs/</c>（不区分大小写）时为 <see langword="true"/></returns>
    private static bool IsSignalRHubRequest(string path)
    {
        return path.Contains("/hubs/", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 根据 HTTP 上下文构建单次请求的结构化日志上下文
    /// </summary>
    /// <param name="context">当前 HTTP 请求上下文</param>
    /// <param name="requestId">本次请求关联 ID（8 位十六进制，写入 TaktLogContext.RequestId）</param>
    /// <returns>
    /// 包含模块 <c>request</c>、动作 <c>http</c>、路由、客户端 IP、
    /// <c>X-Tenant-Code</c> / <c>X-Company-Code</c> 请求头及当前用户标识的 TaktLogContext
    /// </returns>
    private static TaktLogContext BuildRequestContext(HttpContext context, string requestId)
    {
        var principal = TaktUserContext.ResolvePrincipal(context);
        return new TaktLogContext
        {
            Module = "request",
            Action = "http",
            RequestId = requestId,
            TraceId = Activity.Current?.TraceId.ToString(),
            Route = context.Request.Path.Value,
            ClientIp = context.Connection.RemoteIpAddress?.ToString(),
            TenantCode = TaktUserContext.TryResolveTenantCode(context),
            CompanyCode = TaktUserContext.TryResolveCompanyCode(context),
            UserId = TaktUserContext.TryResolveUserId(principal)?.ToString(),
            UserName = TaktUserContext.TryResolveUserName(principal)
        };
    }
}
