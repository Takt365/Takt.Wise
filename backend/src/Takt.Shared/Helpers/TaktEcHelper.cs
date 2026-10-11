// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEcHelper.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更 SignalR 分组名（部门组 dept_xxx、任务组 task_xxx）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 工程变更 SignalR 组名编解码
/// </summary>
public static class TaktEcHelper
{
    /// <summary>
    /// 部门通知组（对应 Clients.Group("dept_xxx") 语义，含公司隔离前缀）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="deptCode">设变责任部门编码（TaktDept.DeptCode，如 D0710、D0810）</param>
    /// <returns>组名</returns>
    public static string DeptGroup(string companyCode, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return $"Company_{companyCode.Trim()}_Dept_{deptCode.Trim()}";
    }

    /// <summary>
    /// 执行任务进度组
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="taskId">任务主键</param>
    /// <returns>组名</returns>
    public static string TaskGroup(string companyCode, long taskId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (taskId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(taskId));
        }
        return $"Company_{companyCode.Trim()}_Task_{taskId}";
    }
}
