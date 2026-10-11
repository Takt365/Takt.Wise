// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Middleware
// 文件名称：TaktSignalRTokenMiddleware.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 连接上下文中间件：WebSocket 无法携带自定义 Header，将查询参数映射为请求头
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logging;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Middleware;

/// <summary>
/// SignalR 连接上下文中间件
/// WebSocket 无法使用 Authorization / 租户公司自定义 Header，需通过查询参数传递并由本中间件写入 Request.Headers
/// </summary>
public class TaktSignalRTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TaktTenantContextOptions _tenantOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">下一个中间件</param>
    /// <param name="tenantOptions">租户上下文配置（请求头名称）</param>
    public TaktSignalRTokenMiddleware(RequestDelegate next, IOptions<TaktTenantContextOptions> tenantOptions)
    {
        _next = next;
        _tenantOptions = tenantOptions.Value;
    }

    /// <summary>
    /// 执行中间件
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>任务</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (IsSignalRRequest(path))
        {
            PromoteQueryToHeader(context, "access_token", "Authorization", static v => $"Bearer {v}");
            PromoteQueryToHeader(context, "tenant_code", _tenantOptions.TenantHeaderName);
            PromoteQueryToHeader(context, "company_code", _tenantOptions.CompanyHeaderName);
            PromoteQueryToHeader(context, TaktHttpQueryNames.ClientUserAgent, TaktHttpHeaderNames.ClientUserAgent);
            PromoteQueryToHeader(context, TaktHttpQueryNames.ClientBrowser, TaktHttpHeaderNames.ClientBrowser);
            PromoteQueryToHeader(context, TaktHttpQueryNames.ClientOperatingSystem, TaktHttpHeaderNames.ClientOperatingSystem);
            PromoteQueryToHeader(context, TaktHttpQueryNames.ClientDeviceType, TaktHttpHeaderNames.ClientDeviceType);

            if (context.Request.Query.ContainsKey("access_token")
                || context.Request.Query.ContainsKey("tenant_code")
                || context.Request.Query.ContainsKey("company_code")
                || context.Request.Query.ContainsKey(TaktHttpQueryNames.ClientUserAgent)
                || context.Request.Query.ContainsKey(TaktHttpQueryNames.ClientBrowser)
                || context.Request.Query.ContainsKey(TaktHttpQueryNames.ClientOperatingSystem)
                || context.Request.Query.ContainsKey(TaktHttpQueryNames.ClientDeviceType))
            {
                TaktLogger.Information(
                    new TaktLogContext { Module = "signalr", Action = "context", Route = path },
                    "SignalR 上下文中间件: 已从查询参数映射 Authorization / {TenantHeader} / {CompanyHeader}，路径: {Path}",
                    _tenantOptions.TenantHeaderName,
                    _tenantOptions.CompanyHeaderName,
                    path);
            }
        }

        await _next(context);
    }

    /// <summary>
    /// 是否为 SignalR Hub 或 negotiate 请求
    /// </summary>
    /// <param name="path">请求路径</param>
    /// <returns>是 SignalR 相关请求时为 true</returns>
    private static bool IsSignalRRequest(string path) =>
        path.Contains("/hubs/", StringComparison.OrdinalIgnoreCase)
        || path.Contains("/negotiate", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// 当请求头未设置时，将查询参数值写入对应请求头
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="queryKey">查询参数名</param>
    /// <param name="headerName">目标请求头名</param>
    /// <param name="transform">可选值转换（如 Bearer 前缀）</param>
    private static void PromoteQueryToHeader(
        HttpContext context,
        string queryKey,
        string headerName,
        Func<string, string>? transform = null)
    {
        if (!string.IsNullOrEmpty(context.Request.Headers[headerName].ToString()))
        {
            return;
        }

        if (!context.Request.Query.TryGetValue(queryKey, out var values)
            || values.Count == 0
            || string.IsNullOrWhiteSpace(values[0]))
        {
            return;
        }

        var raw = values[0]!.Trim();
        context.Request.Headers[headerName] = transform != null ? transform(raw) : raw;
    }
}
