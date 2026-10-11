// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktQuartzEnums.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 调度与任务执行日志专用枚举（Statistics.Logging 域强类型，非字典项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// Quartz 任务类型（与 TaktQuartzTask.TaskType 数值对齐）
/// </summary>
public enum TaktQuartzTaskType
{
    /// <summary>
    /// 程序集
    /// </summary>
    [Display(Name = "程序集")]
    Assembly = 1,
    /// <summary>
    /// 网络请求
    /// </summary>
    [Display(Name = "网络请求")]
    Http = 2,
    /// <summary>
    /// SQL 语句
    /// </summary>
    [Display(Name = "SQL语句")]
    Sql = 3,
}

/// <summary>
/// Quartz 执行日志 JobGroup 快照（由 TaktQuartzTask.JobGroup 字符串映射）
/// </summary>
public enum TaktQuartzLogJobGroup
{
    /// <summary>
    /// 未知
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0,
    /// <summary>
    /// 默认（DEFAULT）
    /// </summary>
    [Display(Name = "默认")]
    Default = 1,
}
