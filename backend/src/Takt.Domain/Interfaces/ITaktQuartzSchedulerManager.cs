// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktQuartzSchedulerManager.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 调度管理器接口（Infrastructure 由 TaktQuartzSchedulerManager 实现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;

namespace Takt.Domain.Interfaces;

/// <summary>
/// Quartz 调度管理器接口
/// </summary>
public interface ITaktQuartzSchedulerManager
{
    /// <summary>
    /// 调度定时任务（新增或覆盖）
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="UserName">触发/创建用户（写入 JobDataMap，StartNow 立即触发时可用）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task ScheduleQuartzTaskAsync(TaktQuartzTask task, string? UserName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 从调度器移除定时任务
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task RemoveQuartzTaskAsync(TaktQuartzTask task, CancellationToken cancellationToken = default);

    /// <summary>
    /// 启动（恢复）定时任务调度
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task StartQuartzTaskAsync(TaktQuartzTask task, CancellationToken cancellationToken = default);

    /// <summary>
    /// 暂停定时任务调度
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task PauseQuartzTaskAsync(TaktQuartzTask task, CancellationToken cancellationToken = default);

    /// <summary>
    /// 立即执行一次定时任务
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="executeParams">本次触发执行参数（非空则覆盖任务配置 ExecuteParams）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task RunQuartzTaskNowAsync(
        TaktQuartzTask task,
        string? UserName = null,
        string? executeParams = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 启动时加载所有租户的正常状态任务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task LoadAllQuartzTasksAsync(CancellationToken cancellationToken = default);
}
