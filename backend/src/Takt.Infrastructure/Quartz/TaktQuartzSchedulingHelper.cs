// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzSchedulingHelper.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务执行/日志与 TaktQuartzTask、TaktQuartzLog 实体字段对齐的纯函数辅助
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Quartz;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 调度辅助（任务校验、日志构建；无状态纯函数）
/// </summary>
public static class TaktQuartzSchedulingHelper
{
    /// <summary>
    /// 任务状态：正常（字典 sys_quartz_task_status；0=正常）
    /// </summary>
    public const int TaskStatusNormal = 0;

    /// <summary>
    /// 任务状态：暂停（字典 sys_quartz_task_status；1=暂停）
    /// </summary>
    public const int TaskStatusPaused = 1;

    /// <summary>
    /// 系统调度写入日志时的创建人 ID（与 TaktConstants.SystemAuditUser.Id 一致）
    /// </summary>
    public const long SystemCreatedBy = TaktConstants.SystemAuditUser.Id;

    /// <summary>
    /// 校验定时任务是否可执行（按 TaktQuartzTask 字段语义）
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="manualTrigger">是否手动立即触发（手动触发允许暂停态任务）</param>
    /// <exception cref="InvalidOperationException">任务不可执行时抛出</exception>
    public static void ValidateQuartzTaskForExecution(TaktQuartzTask task, bool manualTrigger)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (task.IsDeleted != 0)
        {
            throw new InvalidOperationException($"定时任务已删除：{task.TaskCode}");
        }
        if (!manualTrigger && task.TaskStatus == TaskStatusPaused)
        {
            throw new InvalidOperationException($"定时任务已暂停：{task.TaskCode}");
        }
        switch (task.TaskType.Trim().ToLowerInvariant())
        {
            case "assembly":
                ArgumentException.ThrowIfNullOrWhiteSpace(task.ClassName);
                break;
            case "http":
                ArgumentException.ThrowIfNullOrWhiteSpace(task.ApiUrl);
                break;
            case "sql":
                if (!TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(task.SqlScript))
                {
                    throw new InvalidOperationException(
                        "SqlScript 只可填相对 wwwroot 的 .sql 路径（如 Quartz/sync_mat.sql），不允许填写具体 SQL 语句");
                }
                break;
            default:
                throw new InvalidOperationException($"不支持的任务类型：{task.TaskType}");
        }
    }

    /// <summary>
    /// 解析有效执行参数（触发覆盖优先，否则取任务配置）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="triggerExecuteParams">触发时传入的执行参数</param>
    /// <returns>有效执行参数</returns>
    public static string? ResolveEffectiveExecuteParams(TaktQuartzTask task, string? triggerExecuteParams)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (!string.IsNullOrWhiteSpace(triggerExecuteParams))
        {
            return triggerExecuteParams;
        }
        return task.ExecuteParams;
    }

    /// <summary>
    /// 根据任务实体与执行结果构建 TaktQuartzLog（字段与实体定义一致）
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="executeTime">执行时间</param>
    /// <param name="durationMs">耗时毫秒</param>
    /// <param name="executeParams">执行参数快照</param>
    /// <param name="executeMessage">执行消息</param>
    /// <param name="errorInfo">错误信息</param>
    /// <param name="executeStatus">执行状态</param>
    /// <param name="executeIp">执行机器 IP</param>
    /// <param name="executeHost">执行机器名</param>
    /// <returns>任务执行日志实体</returns>
    public static TaktQuartzLog BuildQuartzLog(
        TaktQuartzTask task,
        DateTime executeTime,
        long durationMs,
        string? executeParams,
        string? executeMessage,
        string? errorInfo,
        TaktExecuteStatus executeStatus,
        string? executeIp,
        string? executeHost)
    {
        ArgumentNullException.ThrowIfNull(task);
        var now = DateTime.Now;
        var log = new TaktQuartzLog
        {
            TenantCode = task.TenantCode,
            CompanyCode = task.CompanyCode,
            QuartzTaskId = task.Id,
            TaskName = Truncate(task.TaskName, 100),
            JobGroup = Truncate(task.JobGroup?.Trim(), TaktQuartzConstants.MaxJobGroupLength),
            TaskType = task.TaskType?.Trim() ?? string.Empty,
            ExecuteTime = executeTime,
            ExecuteDuration = durationMs,
            ExecuteParams = Truncate(executeParams, 1000),
            ExecuteMessage = Truncate(executeMessage, 2000),
            ErrorInfo = Truncate(errorInfo, TaktQuartzJobExecutionLogger.MaxErrorInfoLength),
            ExecuteIp = Truncate(executeIp, 50),
            ExecuteHost = Truncate(executeHost, 100),
            ExecuteStatus = (int)executeStatus,
            ExtField = task.ExtField,
            Remark = task.Remark,
        };
        log.ApplyCreate(TaktConstants.SystemAuditUser.Id, now);
        return log;
    }

    /// <summary>
    /// 从 Quartz 触发器解析任务下次执行
    /// </summary>
    /// <param name="context">Quartz 执行上下文</param>
    /// <returns>下次执行（本地时间），无则 null</returns>
    public static DateTime? ResolveNextRunAt(IJobExecutionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Trigger?.GetNextFireTimeUtc().HasValue != true)
        {
            return null;
        }
        return context.Trigger.GetNextFireTimeUtc()!.Value.LocalDateTime;
    }

    /// <summary>
    /// 校验程序集任务处理器与任务 AssemblyName 配置一致
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="handler">任务处理器实例</param>
    /// <exception cref="InvalidOperationException">程序集名称不匹配时抛出</exception>
    public static void EnsureAssemblyNameMatchesHandler(TaktQuartzTask task, ITaktQuartzJobHandler handler)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(handler);
        if (string.IsNullOrWhiteSpace(task.AssemblyName))
        {
            return;
        }
        var handlerAssemblyName = handler.GetType().Assembly.GetName().Name ?? string.Empty;
        if (!string.Equals(task.AssemblyName.Trim(), handlerAssemblyName, StringComparison.OrdinalIgnoreCase)
            && !handlerAssemblyName.Contains(task.AssemblyName.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"程序集名称不匹配：任务={task.AssemblyName}，处理器={handlerAssemblyName}");
        }
    }

    /// <summary>
    /// 截断字符串至实体列最大长度
    /// </summary>
    /// <param name="value">原始值</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>截断后的字符串；null/空输入返回空串</returns>
    public static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxLength);
        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
