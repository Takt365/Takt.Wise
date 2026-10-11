// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzStartupHostedService.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：应用启动后加载各租户正常状态 Quartz 任务
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 启动加载服务
/// </summary>
public sealed class TaktQuartzStartupHostedService : IHostedService
{
    private readonly TaktQuartzOptions _options;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceScopeFactory">作用域工厂</param>
    /// <param name="options">Quartz 配置</param>
    public TaktQuartzStartupHostedService(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<TaktQuartzOptions> options)
    {
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        ArgumentNullException.ThrowIfNull(options);
        _serviceScopeFactory = serviceScopeFactory;
        _options = options.Value;
    }

    /// <summary>
    /// 启动后加载任务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_options.Enabled || !_options.LoadTasksOnStartup)
        {
            return;
        }
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var schedulerManager = scope.ServiceProvider.GetRequiredService<ITaktQuartzSchedulerManager>();
            await schedulerManager.LoadAllQuartzTasksAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[Quartz] 启动加载任务失败，应用继续运行");
        }
    }

    /// <summary>
    /// 停止宿主
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
