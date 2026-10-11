// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobExecutionLogger.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务执行独立日志（Serilog 通道 TaktLogChannel=QuartzJob → logs/quartz-/quartz-.log；记录开始/过程/结果/异常）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logging;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 任务执行独立日志（调试可靠性：每次执行起止、摘要、异常均写入专属通道）
/// </summary>
public static class TaktQuartzJobExecutionLogger
{
    /// <summary>
    /// 落库 ErrorInfo 最大长度（与实体列截断策略一致）
    /// </summary>
    public const int MaxErrorInfoLength = 2000;

    /// <summary>
    /// 开启单次执行日志作用域（租户/公司/任务码/通道属性）
    /// </summary>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <param name="taskId">任务 Id</param>
    /// <param name="taskCode">任务编码</param>
    /// <param name="taskType">任务类型</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="manualTrigger">是否手动触发</param>
    /// <returns>可释放作用域</returns>
    public static IDisposable BeginExecutionScope(
        string tenantCode,
        string companyCode,
        long taskId,
        string? taskCode,
        string? taskType,
        string? UserName,
        bool manualTrigger)
    {
        var context = new TaktLogContext
        {
            Module = TaktQuartzConstants.LogModuleName,
            Action = "execute",
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            UserName = UserName,
            Extra = new Dictionary<string, object?>
            {
                [TaktQuartzConstants.LogChannelPropertyName] = TaktQuartzConstants.LogChannelValue,
                ["QuartzTaskId"] = taskId.ToString(),
                ["TaskCode"] = taskCode ?? string.Empty,
                ["TaskType"] = taskType ?? string.Empty,
                ["ManualTrigger"] = manualTrigger,
            },
        };
        return TaktLogger.BeginScope(context);
    }

    /// <summary>
    /// 记录调度侧「已触发立即执行」（Job 尚未开始）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="jobKey">JobKey 文本</param>
    public static void LogManualTrigger(TaktQuartzTask task, string? UserName, string jobKey)
    {
        ArgumentNullException.ThrowIfNull(task);
        using (BeginExecutionScope(
            task.TenantCode,
            task.CompanyCode,
            task.Id,
            task.TaskCode,
            task.TaskType,
            UserName,
            manualTrigger: true))
        {
            TaktLogger.Information(
                "[Quartz] 手动触发立即执行 TaskId={TaskId}, Code={TaskCode}, Job={JobKey}, User={User}",
                task.Id,
                task.TaskCode,
                jobKey,
                UserName ?? string.Empty);
        }
    }

    /// <summary>
    /// 记录 Job 开始执行
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="manualTrigger">是否手动</param>
    /// <param name="UserName">触发用户</param>
    public static void LogStarted(TaktQuartzTask task, bool manualTrigger, string? UserName)
    {
        ArgumentNullException.ThrowIfNull(task);
        TaktLogger.Information(
            "[Quartz] 开始执行 TaskId={TaskId}, Code={TaskCode}, Type={TaskType}, Manual={Manual}, Tenant={Tenant}, Company={Company}, User={User}, SqlOrUrl={Target}",
            task.Id,
            task.TaskCode,
            task.TaskType,
            manualTrigger,
            task.TenantCode,
            task.CompanyCode,
            UserName ?? string.Empty,
            ResolveExecutionTarget(task));
    }

    /// <summary>
    /// 记录执行过程（SQL/HTTP/程序集中间步骤）
    /// </summary>
    /// <param name="message">过程摘要</param>
    /// <param name="taskId">任务 Id</param>
    public static void LogProgress(string message, long taskId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        TaktLogger.Information("[Quartz] {Message} TaskId={TaskId}", message, taskId);
    }

    /// <summary>
    /// 记录执行完成（成功或已落库的失败摘要；完整异常另见 LogFailed）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="status">执行状态</param>
    /// <param name="durationMs">耗时毫秒</param>
    /// <param name="logId">执行日志表 Id</param>
    /// <param name="executeMessage">执行消息</param>
    /// <param name="errorInfo">错误摘要（无则空）</param>
    public static void LogCompleted(
        TaktQuartzTask task,
        TaktExecuteStatus status,
        long durationMs,
        long logId,
        string? executeMessage,
        string? errorInfo)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (status == TaktExecuteStatus.Success)
        {
            TaktLogger.Information(
                "[Quartz] 执行成功 TaskId={TaskId}, Code={TaskCode}, DurationMs={DurationMs}, LogId={LogId}, Message={Message}",
                task.Id,
                task.TaskCode,
                durationMs,
                logId,
                executeMessage ?? string.Empty);
            return;
        }
        TaktLogger.Warning(
            "[Quartz] 执行失败已落库 TaskId={TaskId}, Code={TaskCode}, DurationMs={DurationMs}, LogId={LogId}, Message={Message}, Error={Error}",
            task.Id,
            task.TaskCode,
            durationMs,
            logId,
            executeMessage ?? string.Empty,
            string.IsNullOrWhiteSpace(errorInfo) ? "-" : errorInfo);
    }

    /// <summary>
    /// 记录执行异常（含完整 Exception，写入专属 quartz 文件）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="task">定时任务</param>
    /// <param name="durationMs">已耗时毫秒</param>
    public static void LogFailed(Exception exception, TaktQuartzTask task, long durationMs)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentNullException.ThrowIfNull(task);
        TaktLogger.Error(
            exception,
            "[Quartz] 任务执行异常 TaskId={TaskId}, Code={TaskCode}, Type={TaskType}, DurationMs={DurationMs}, Target={Target}",
            task.Id,
            task.TaskCode,
            task.TaskType,
            durationMs,
            ResolveExecutionTarget(task));
    }

    /// <summary>
    /// 记录基础设施级失败（JobDataMap 缺字段、任务不存在等，尚无任务实体）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <param name="taskId">任务 Id（可能为 0）</param>
    public static void LogInfrastructureFailure(
        Exception exception,
        string? tenantCode,
        string? companyCode,
        long taskId)
    {
        ArgumentNullException.ThrowIfNull(exception);
        using (BeginExecutionScope(
            tenantCode ?? string.Empty,
            companyCode ?? string.Empty,
            taskId,
            taskCode: null,
            taskType: null,
            UserName: null,
            manualTrigger: false))
        {
            TaktLogger.Error(
                exception,
                "[Quartz] 任务执行基础设施失败 TaskId={TaskId}, Tenant={Tenant}, Company={Company}",
                taskId,
                tenantCode ?? string.Empty,
                companyCode ?? string.Empty);
        }
    }

    /// <summary>
    /// 生成可落库的错误信息（消息 + 堆栈截断，供 TaktQuartzLog.ErrorInfo）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <returns>截断后的错误文本</returns>
    public static string FormatErrorInfoForPersist(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        var text = exception.ToString();
        if (text.Length <= MaxErrorInfoLength)
        {
            return text;
        }
        return text[..MaxErrorInfoLength];
    }

    /// <summary>
    /// 解析执行目标摘要（SQL 路径 / URL / 程序集类名）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <returns>目标摘要</returns>
    private static string ResolveExecutionTarget(TaktQuartzTask task)
    {
        var type = (task.TaskType ?? string.Empty).Trim().ToLowerInvariant();
        return type switch
        {
            "sql" => task.SqlScript?.Trim() ?? string.Empty,
            "http" => task.ApiUrl?.Trim() ?? string.Empty,
            "assembly" => $"{task.AssemblyName}/{task.ClassName}",
            _ => task.TaskType ?? string.Empty,
        };
    }
}
