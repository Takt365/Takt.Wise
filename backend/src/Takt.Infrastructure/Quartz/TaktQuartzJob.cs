// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJob.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz IJob 入口（并发/非并发两种 Job 类型）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 任务基类
/// </summary>
public abstract class TaktQuartzJobBase : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceScopeFactory">作用域工厂</param>
    protected TaktQuartzJobBase(IServiceScopeFactory serviceScopeFactory)
    {
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// Quartz 调度入口
    /// </summary>
    /// <param name="context">执行上下文</param>
    /// <returns>任务</returns>
    public async Task Execute(IJobExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        using var scope = _serviceScopeFactory.CreateScope();
        // 必须先注入租户/公司，再解析 Scoped 服务；否则 ITaktUserContext 会落成无公司的 SeedUserContext
        var data = context.MergedJobDataMap;
        TaktQuartzAmbientHttpContext.Configure(
            scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>(),
            scope.ServiceProvider.GetRequiredService<IOptions<TaktTenantContextOptions>>().Value,
            TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.TenantCode),
            TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.CompanyCode),
            TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.UserName));
        var executor = scope.ServiceProvider.GetRequiredService<TaktQuartzJobExecutor>();
        await executor.ExecuteAsync(context, context.CancellationToken);
    }
}

/// <summary>
/// 禁止同一 Job 并发执行
/// </summary>
[DisallowConcurrentExecution]
public sealed class TaktQuartzSequentialJob : TaktQuartzJobBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceScopeFactory">作用域工厂</param>
    public TaktQuartzSequentialJob(IServiceScopeFactory serviceScopeFactory)
        : base(serviceScopeFactory)
    {
    }
}

/// <summary>
/// 允许同一 Job 并发执行
/// </summary>
public sealed class TaktQuartzConcurrentJob : TaktQuartzJobBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceScopeFactory">作用域工厂</param>
    public TaktQuartzConcurrentJob(IServiceScopeFactory serviceScopeFactory)
        : base(serviceScopeFactory)
    {
    }
}
