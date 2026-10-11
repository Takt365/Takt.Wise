// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktQuartzSignalRNotifier.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务 SignalR 推送编排（定义变更；执行完成见 Infrastructure/Quartz）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Shared.Models.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// Quartz SignalR 推送编排接口
/// </summary>
public interface ITaktQuartzSignalRNotifier
{
    /// <summary>
    /// 推送定时任务定义变更
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="task">定时任务实体</param>
    /// <param name="changeType">变更类型</param>
    /// <param name="operatorUserName">操作人用户名</param>
    /// <returns>任务</returns>
    Task NotifyTaskChangedAsync(
        string tenantCode,
        string companyCode,
        TaktQuartzTask task,
        string changeType,
        string? operatorUserName);
}

/// <summary>
/// Quartz SignalR 推送编排实现
/// </summary>
public class TaktQuartzSignalRNotifier : ITaktQuartzSignalRNotifier
{
    private readonly ITaktSignalRDispatchService _signalRDispatchService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="signalRDispatchService">SignalR 调度服务</param>
    public TaktQuartzSignalRNotifier(ITaktSignalRDispatchService signalRDispatchService)
    {
        _signalRDispatchService = signalRDispatchService;
    }

    /// <summary>
    /// 推送定时任务定义变更
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="task">定时任务实体</param>
    /// <param name="changeType">变更类型</param>
    /// <param name="operatorUserName">操作人用户名</param>
    /// <returns>任务</returns>
    public async Task NotifyTaskChangedAsync(
        string tenantCode,
        string companyCode,
        TaktQuartzTask task,
        string changeType,
        string? operatorUserName)
    {
        ArgumentNullException.ThrowIfNull(task);
        var push = new TaktSignalRQuartzTaskChangedPush
        {
            TenantCode = tenantCode.Trim(),
            CompanyCode = companyCode.Trim(),
            QuartzTaskId = task.Id,
            TaskCode = task.TaskCode ?? string.Empty,
            TaskName = task.TaskName ?? string.Empty,
            ChangeType = changeType.Trim(),
            OperatorUserName = operatorUserName?.Trim(),
            ChangedAt = DateTime.Now,
        };
        await _signalRDispatchService.PushQuartzTaskChangedAsync(push);
    }
}
