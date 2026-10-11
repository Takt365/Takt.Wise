// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzSchedulerManager.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 调度管理器实现（注册/暂停/立即执行/启动加载）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 调度管理器实现
/// </summary>
public sealed class TaktQuartzSchedulerManager : ITaktQuartzSchedulerManager
{
    private const int TaskStatusNormal = 0;
    private const int TaskStatusPaused = 1;
    private const int TriggerTypeSimple = 0;
    private const int TriggerTypeCron = 1;
    private const int MisfireIgnore = 1;
    private const int MisfireFireAndProceed = 2;
    private const int MisfireDoNothing = 3;
    private readonly IConfiguration _configuration;
    private readonly TaktQuartzOptions _options;
    private readonly ISchedulerFactory _schedulerFactory;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="schedulerFactory">Quartz 调度器工厂</param>
    /// <param name="configuration">应用配置</param>
    /// <param name="options">Quartz 配置</param>
    public TaktQuartzSchedulerManager(
        ISchedulerFactory schedulerFactory,
        IConfiguration configuration,
        IOptions<TaktQuartzOptions> options)
    {
        ArgumentNullException.ThrowIfNull(schedulerFactory);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(options);
        _schedulerFactory = schedulerFactory;
        _configuration = configuration;
        _options = options.Value;
    }

    /// <summary>
    /// 调度定时任务（新增或覆盖）
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="UserName">触发/创建用户（写入 JobDataMap）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task ScheduleQuartzTaskAsync(
        TaktQuartzTask task,
        string? UserName = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        EnsureEnabled();
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        var jobKey = BuildJobKey(task);
        var triggerKey = BuildTriggerKey(task);
        var jobDetail = BuildJobDetail(task, UserName);
        var trigger = BuildTrigger(task, triggerKey);
        if (await scheduler.CheckExists(jobKey, cancellationToken))
        {
            await scheduler.DeleteJob(jobKey, cancellationToken);
        }
        await scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        if (task.TaskStatus == TaskStatusPaused)
        {
            await scheduler.PauseJob(jobKey, cancellationToken);
        }
        TaktLogger.Information("[Quartz] 已调度任务 {TaskCode} Job={JobKey}", task.TaskCode, jobKey);
    }

    /// <summary>
    /// 从调度器移除定时任务
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task RemoveQuartzTaskAsync(TaktQuartzTask task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (!_options.Enabled)
        {
            return;
        }
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        var jobKey = BuildJobKey(task);
        if (await scheduler.CheckExists(jobKey, cancellationToken))
        {
            await scheduler.DeleteJob(jobKey, cancellationToken);
            TaktLogger.Information("[Quartz] 已移除任务 {TaskCode} Job={JobKey}", task.TaskCode, jobKey);
        }
    }

    /// <summary>
    /// 启动（恢复）定时任务调度
    /// </summary>
    /// <remarks>
    /// 必须重新 Schedule，不能仅 Resume：应用启动时 LoadAll 只装载 TaskStatus=正常 的任务；
    /// 原先「暂停」任务不在内存调度器中，若只 Resume 会出现「库中已是正常、调度器却无 Job」而永不触发。
    /// </remarks>
    /// <param name="task">定时任务实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task StartQuartzTaskAsync(TaktQuartzTask task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        EnsureEnabled();
        await ScheduleQuartzTaskAsync(task, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 暂停定时任务调度
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task PauseQuartzTaskAsync(TaktQuartzTask task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (!_options.Enabled)
        {
            return;
        }
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        var jobKey = BuildJobKey(task);
        if (await scheduler.CheckExists(jobKey, cancellationToken))
        {
            await scheduler.PauseJob(jobKey, cancellationToken);
        }
    }

    /// <summary>
    /// 立即执行一次定时任务
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="executeParams">本次触发执行参数（非空则覆盖任务配置 ExecuteParams）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task RunQuartzTaskNowAsync(
        TaktQuartzTask task,
        string? UserName = null,
        string? executeParams = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        EnsureEnabled();
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        var jobKey = BuildJobKey(task);
        if (!await scheduler.CheckExists(jobKey, cancellationToken))
        {
            await ScheduleQuartzTaskAsync(task, cancellationToken: cancellationToken);
        }
        var effectiveParams = !string.IsNullOrWhiteSpace(executeParams)
            ? executeParams.Trim()
            : (task.ExecuteParams ?? string.Empty);
        var data = new JobDataMap
        {
            { TaktQuartzJobDataKeys.UserName, UserName ?? string.Empty },
            { TaktQuartzJobDataKeys.ManualTrigger, 1 },
            { TaktQuartzJobDataKeys.ExecuteParams, effectiveParams },
        };
        TaktQuartzJobExecutionLogger.LogManualTrigger(task, UserName, jobKey.ToString());
        await scheduler.TriggerJob(jobKey, data, cancellationToken);
    }

    /// <summary>
    /// 启动时加载所有租户的正常状态任务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task LoadAllQuartzTasksAsync(CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled || !_options.LoadTasksOnStartup)
        {
            return;
        }
        var tenantConnections = _configuration.GetTenantConnections();
        var loadedCount = 0;
        foreach (var (tenantCode, _) in tenantConnections)
        {
            using var seedContext = new TaktSeedContext(_configuration, tenantCode);
            var tasks = await seedContext.Db.Queryable<TaktQuartzTask>()
                .Where(x => x.IsDeleted == 0 && x.TaskStatus == TaskStatusNormal)
                .ToListAsync(cancellationToken);
            foreach (var task in tasks)
            {
                await ScheduleQuartzTaskAsync(task, cancellationToken: cancellationToken);
                loadedCount++;
            }
        }
        TaktLogger.Information("[Quartz] 启动加载完成，共调度 {Count} 个正常状态任务", loadedCount);
    }

    /// <summary>
    /// 校验 Quartz 已启用
    /// </summary>
    private void EnsureEnabled()
    {
        if (!_options.Enabled)
        {
            throw new InvalidOperationException("Quartz 调度未启用，请在配置 Quartz:Enabled 后重试");
        }
    }

    /// <summary>
    /// 构建 JobKey（租户+公司+分组隔离）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <returns>JobKey</returns>
    private static JobKey BuildJobKey(TaktQuartzTask task)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(task.JobName);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.JobGroup);
        var group = $"{task.TenantCode}_{task.CompanyCode}_{task.JobGroup}";
        return new JobKey(task.JobName, group);
    }

    /// <summary>
    /// 构建 TriggerKey
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <returns>TriggerKey</returns>
    private static TriggerKey BuildTriggerKey(TaktQuartzTask task)
    {
        var jobKey = BuildJobKey(task);
        return new TriggerKey($"{jobKey.Name}_trigger", jobKey.Group);
    }

    /// <summary>
    /// 构建 JobDetail
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <returns>JobDetail</returns>
    private static IJobDetail BuildJobDetail(TaktQuartzTask task, string? UserName = null)
    {
        var jobType = task.Concurrent == 1 ? typeof(TaktQuartzConcurrentJob) : typeof(TaktQuartzSequentialJob);
        return JobBuilder.Create(jobType)
            .WithIdentity(BuildJobKey(task))
            .UsingJobData(TaktQuartzJobDataKeys.TenantCode, task.TenantCode ?? string.Empty)
            .UsingJobData(TaktQuartzJobDataKeys.CompanyCode, task.CompanyCode ?? string.Empty)
            .UsingJobData(TaktQuartzJobDataKeys.QuartzTaskId, task.Id)
            .UsingJobData(TaktQuartzJobDataKeys.UserName, UserName ?? string.Empty)
            .UsingJobData(TaktQuartzJobDataKeys.ExecuteParams, task.ExecuteParams ?? string.Empty)
            .Build();
    }

    /// <summary>
    /// 构建 Trigger
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="triggerKey">触发器键</param>
    /// <returns>Trigger</returns>
    private static ITrigger BuildTrigger(TaktQuartzTask task, TriggerKey triggerKey)
    {
        var builder = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(BuildJobKey(task));
        if (task.FirstRunAt.HasValue && task.FirstRunAt.Value > DateTime.Now)
        {
            builder = builder.StartAt(task.FirstRunAt.Value);
        }
        else
        {
            builder = builder.StartNow();
        }
        if (task.TriggerType == TriggerTypeCron)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(task.CronExpression);
            if (!CronExpression.IsValidExpression(task.CronExpression))
            {
                throw new ArgumentException($"Cron 表达式无效：{task.CronExpression}");
            }
            var cronBuilder = CronScheduleBuilder.CronSchedule(task.CronExpression);
            cronBuilder = ApplyCronMisfirePolicy(cronBuilder, task.MisfirePolicy);
            return builder.WithSchedule(cronBuilder).Build();
        }
        // IntervalSeconds<=0：一次性触发（用于后台备份等按 FirstRunAt 单次执行）
        if (task.IntervalSeconds <= 0)
        {
            var oneShotBuilder = SimpleScheduleBuilder.Create().WithRepeatCount(0);
            oneShotBuilder = ApplySimpleMisfirePolicy(oneShotBuilder, task.MisfirePolicy);
            return builder.WithSchedule(oneShotBuilder).Build();
        }
        var interval = Math.Max(1, task.IntervalSeconds);
        var simpleBuilder = SimpleScheduleBuilder.Create()
            .WithIntervalInSeconds(interval)
            .RepeatForever();
        simpleBuilder = ApplySimpleMisfirePolicy(simpleBuilder, task.MisfirePolicy);
        return builder.WithSchedule(simpleBuilder).Build();
    }

    /// <summary>
    /// 应用 Cron Misfire 策略
    /// </summary>
    /// <param name="builder">Cron 调度构建器</param>
    /// <param name="misfirePolicy">Misfire 策略</param>
    /// <returns>构建器</returns>
    private static CronScheduleBuilder ApplyCronMisfirePolicy(CronScheduleBuilder builder, int misfirePolicy)
    {
        return misfirePolicy switch
        {
            MisfireIgnore => builder.WithMisfireHandlingInstructionIgnoreMisfires(),
            MisfireFireAndProceed => builder.WithMisfireHandlingInstructionFireAndProceed(),
            MisfireDoNothing => builder.WithMisfireHandlingInstructionDoNothing(),
            _ => builder,
        };
    }

    /// <summary>
    /// 应用 Simple Misfire 策略
    /// </summary>
    /// <param name="builder">Simple 调度构建器</param>
    /// <param name="misfirePolicy">Misfire 策略</param>
    /// <returns>构建器</returns>
    private static SimpleScheduleBuilder ApplySimpleMisfirePolicy(SimpleScheduleBuilder builder, int misfirePolicy)
    {
        return misfirePolicy switch
        {
            MisfireIgnore => builder.WithMisfireHandlingInstructionIgnoreMisfires(),
            MisfireFireAndProceed => builder.WithMisfireHandlingInstructionNowWithExistingCount(),
            MisfireDoNothing => builder.WithMisfireHandlingInstructionNextWithExistingCount(),
            _ => builder,
        };
    }
}
