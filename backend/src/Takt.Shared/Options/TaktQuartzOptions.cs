// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktQuartzOptions.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务调度配置；appsettings 覆盖本类默认值
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// Quartz 调度器配置
/// </summary>
public class TaktQuartzOptions
{
    /// <summary>
    /// 配置节名称
    /// </summary>
    public const string SectionName = "Quartz";

    /// <summary>
    /// 是否启用 Quartz 调度
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 调度器名称
    /// </summary>
    public string SchedulerName { get; set; } = "TaktQuartzScheduler";

    /// <summary>
    /// 调度器实例 ID
    /// </summary>
    public string SchedulerId { get; set; } = "TaktQuartzSchedulerInstance";

    /// <summary>
    /// 启动时是否自动加载所有租户任务
    /// </summary>
    public bool LoadTasksOnStartup { get; set; } = true;

    /// <summary>
    /// 校验 Quartz 配置
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(SchedulerName))
        {
            throw new InvalidOperationException($"{SectionName}:SchedulerName 不能为空");
        }

        if (string.IsNullOrWhiteSpace(SchedulerId))
        {
            throw new InvalidOperationException($"{SectionName}:SchedulerId 不能为空");
        }
    }
}
