// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktWorkflowStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：工作流统计 DTO（数据看板 todo-stat / instance-stat / my-stat / processed-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Workflow;

/// <summary>
/// 工作流待办统计 DTO（当前用户 Pending 任务 + Running 实例）
/// </summary>
public class TaktWorkflowTodoStatDto
{
    /// <summary>
    /// 待办数量
    /// </summary>
    public int PendingTodoCount { get; set; }
}

/// <summary>
/// 流程实例统计查询 DTO（按开始/创建时间区间）
/// </summary>
public class TaktWorkflowInstanceStatQueryDto
{
    /// <summary>
    /// 开始时间（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }

    /// <summary>
    /// 开始时间（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }
}

/// <summary>
/// 流程实例统计 DTO（租户+公司维度）
/// </summary>
public class TaktWorkflowInstanceStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月实例数量
    /// </summary>
    public int MonthInstanceCount { get; set; }

    /// <summary>
    /// 月运行中实例数量
    /// </summary>
    public int MonthRunningCount { get; set; }

    /// <summary>
    /// 月已完成实例数量
    /// </summary>
    public int MonthCompletedCount { get; set; }

    /// <summary>
    /// 月已驳回实例数量
    /// </summary>
    public int MonthRejectedCount { get; set; }

    /// <summary>
    /// 月已终止实例数量
    /// </summary>
    public int MonthTerminatedCount { get; set; }
}

/// <summary>
/// 我发起的流程统计查询 DTO
/// </summary>
public class TaktWorkflowMyInstanceStatQueryDto
{
    /// <summary>
    /// 开始时间（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }

    /// <summary>
    /// 开始时间（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }
}

/// <summary>
/// 我发起的流程统计 DTO
/// </summary>
public class TaktWorkflowMyInstanceStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月发起实例数量
    /// </summary>
    public int MonthMyInstanceCount { get; set; }

    /// <summary>
    /// 月运行中数量
    /// </summary>
    public int MonthMyRunningCount { get; set; }

    /// <summary>
    /// 月已完成数量
    /// </summary>
    public int MonthMyCompletedCount { get; set; }
}

/// <summary>
/// 已办任务统计查询 DTO（按办结时间区间）
/// </summary>
public class TaktWorkflowProcessedStatQueryDto
{
    /// <summary>
    /// 办结时间（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? CompletedAtStart { get; set; }

    /// <summary>
    /// 办结时间（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? CompletedAtEnd { get; set; }
}

/// <summary>
/// 已办任务统计 DTO（当前用户 Completed 任务）
/// </summary>
public class TaktWorkflowProcessedStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月已办任务数量
    /// </summary>
    public int MonthProcessedTaskCount { get; set; }
}
