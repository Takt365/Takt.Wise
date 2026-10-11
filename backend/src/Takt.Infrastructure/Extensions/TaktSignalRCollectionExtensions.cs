// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktSignalRCollectionExtensions.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 实时通信服务注册与 Hub 映射扩展
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Infrastructure.Services;
using Takt.Infrastructure.SignalR;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// SignalR 配置扩展方法
/// </summary>
public static class TaktSignalRCollectionExtensions
{
    /// <summary>
    /// 添加 SignalR 服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>SignalR 服务构建器</returns>
    public static ISignalRServerBuilder AddTaktSignalR(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration;
        services.AddSingleton<IUserIdProvider, TaktSignalRUserIdProvider>();

        return services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaximumReceiveMessageSize = 32 * 1024;
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            options.AddFilter<TaktHubUserPrincipalFilter>();
        })
        .AddJsonProtocol(protocolOptions =>
        {
            protocolOptions.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            protocolOptions.PayloadSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });
    }

    /// <summary>
    /// 映射指定 SignalR Hub
    /// </summary>
    /// <typeparam name="THub">Hub 类型</typeparam>
    /// <param name="app">Web 应用程序</param>
    /// <param name="pattern">Hub 路由</param>
    /// <returns>Web 应用程序</returns>
    public static WebApplication MapTaktSignalRHub<THub>(this WebApplication app, string pattern)
        where THub : Hub
    {
        app.MapHub<THub>(pattern);
        return app;
    }
}

/// <summary>
/// Hub 客户端调用时将 HubCallerContext.User 挂到 TaktUserContext.HubInvocationPrincipal
/// </summary>
internal sealed class TaktHubUserPrincipalFilter : IHubFilter
{
    /// <summary>
    /// Hub 方法调用前挂载当前连接的 ClaimsPrincipal，供 TaktUserContext 解析用户
    /// </summary>
    /// <param name="invocationContext">Hub 调用上下文</param>
    /// <param name="next">下一个过滤器或 Hub 方法委托</param>
    /// <returns>Hub 方法返回值</returns>
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        var previous = TaktUserContext.HubInvocationPrincipal;
        try
        {
            TaktUserContext.HubInvocationPrincipal = invocationContext.Context.User;
            return await next(invocationContext);
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = previous;
        }
    }
}
