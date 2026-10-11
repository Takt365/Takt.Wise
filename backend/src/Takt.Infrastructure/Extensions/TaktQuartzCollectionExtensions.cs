// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktQuartzCollectionExtensions.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 调度服务注册扩展
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Quartz;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// Quartz 调度注册扩展
/// </summary>
public static class TaktQuartzCollectionExtensions
{
    /// <summary>
    /// 注册 Quartz 调度器、Job 与启动加载服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">应用配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        var quartzOptions = configuration.RequireOptions<TaktQuartzOptions>(TaktQuartzOptions.SectionName);
        quartzOptions.Validate();
        services.Configure<TaktQuartzOptions>(configuration.GetSection(TaktQuartzOptions.SectionName));
        services.AddScoped<TaktQuartzJobExecutor>();
        services.AddScoped<ITaktQuartzJobSignalRPushService, TaktQuartzJobSignalRPushService>();
        services.AddTransient<TaktQuartzSequentialJob>();
        services.AddTransient<TaktQuartzConcurrentJob>();
        services.AddHttpClient(TaktQuartzConstants.HttpClientName, client =>
        {
            client.Timeout = TimeSpan.FromMinutes(5);
        });
        if (quartzOptions.Enabled)
        {
            services.AddQuartz(quartz =>
            {
                quartz.SchedulerName = quartzOptions.SchedulerName;
                quartz.SchedulerId = quartzOptions.SchedulerId;
            });
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            services.AddHostedService<TaktQuartzStartupHostedService>();
        }
        return services;
    }
}
