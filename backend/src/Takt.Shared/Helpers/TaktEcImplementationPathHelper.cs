// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEcImplementationPathHelper.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施执行路径解析（当前卡点部门、品管课正式完成判定）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// 设变实施路径解析结果
/// </summary>
public sealed class TaktEcImplementationPathResult
{
    /// <summary>
    /// 当前待实施部门编码（路径上首个未全部完成的部门；全部完成时为 null）
    /// </summary>
    public string? CurrentDeptCode { get; init; }
    /// <summary>
    /// 当前部门待实施明细数
    /// </summary>
    public int PendingAtCurrentDeptCount { get; init; }
    /// <summary>
    /// 实施路径状态
    /// </summary>
    public int ImplementationStatus { get; init; }
    /// <summary>
    /// 品管课是否已全部实施（正式完成）
    /// </summary>
    public bool IsOfficiallyCompleted { get; init; }
}

/// <summary>
/// 设变实施执行路径辅助（顺序与 TaktEcDeptCodes.KanbanOrder 一致）
/// </summary>
public static class TaktEcImplementationPathHelper
{
    /// <summary>
    /// 根据各部门汇总阶段解析当前卡点与正式完成状态
    /// </summary>
    /// <param name="detailCount">设变明细行数</param>
    /// <param name="stages">各部门实施汇总（DeptCode + ImplementedCount + TotalCount）</param>
    /// <returns>路径解析结果</returns>
    /// <exception cref="ArgumentNullException">stages 为 null</exception>
    public static TaktEcImplementationPathResult Resolve(
        int detailCount,
        IReadOnlyList<TaktEcImplementationStageSnapshot> stages)
    {
        ArgumentNullException.ThrowIfNull(stages);
        if (detailCount <= 0)
        {
            return new TaktEcImplementationPathResult
            {
                ImplementationStatus = TaktEcImplementationStatusConstants.NotStarted,
            };
        }
        var stageMap = stages.ToDictionary(
            s => s.DeptCode,
            s => s,
            StringComparer.Ordinal);
        string? currentDept = null;
        var pendingAtCurrent = 0;
        foreach (var code in TaktEcDeptCodes.KanbanOrder)
        {
            if (!stageMap.TryGetValue(code, out var stage))
            {
                continue;
            }
            if (stage.ImplementedCount < stage.TotalCount)
            {
                currentDept = code;
                pendingAtCurrent = stage.TotalCount - stage.ImplementedCount;
                break;
            }
        }
        var qaStage = GetStage(stageMap, TaktEcDeptCodes.Qc);
        var isOfficiallyCompleted = qaStage.TotalCount > 0
            && qaStage.ImplementedCount >= qaStage.TotalCount;
        var anyProgress = stages.Any(s => s.ImplementedCount > 0);
        int status;
        if (isOfficiallyCompleted)
        {
            status = currentDept == null
                ? TaktEcImplementationStatusConstants.FullyCompleted
                : TaktEcImplementationStatusConstants.OfficiallyCompleted;
        }
        else if (anyProgress)
        {
            status = TaktEcImplementationStatusConstants.InProgress;
        }
        else
        {
            status = TaktEcImplementationStatusConstants.NotStarted;
        }
        return new TaktEcImplementationPathResult
        {
            CurrentDeptCode = currentDept,
            PendingAtCurrentDeptCount = pendingAtCurrent,
            ImplementationStatus = status,
            IsOfficiallyCompleted = isOfficiallyCompleted,
        };
    }

    /// <summary>
    /// 读取部门阶段快照
    /// </summary>
    /// <param name="stageMap">部门编码映射</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>阶段快照</returns>
    private static TaktEcImplementationStageSnapshot GetStage(
        IReadOnlyDictionary<string, TaktEcImplementationStageSnapshot> stageMap,
        string deptCode)
    {
        return stageMap.TryGetValue(deptCode, out var stage)
            ? stage
            : new TaktEcImplementationStageSnapshot(deptCode, 0, 0);
    }
}

/// <summary>
/// 设变部门实施阶段快照（供路径解析使用）
/// </summary>
/// <param name="DeptCode">部门编码</param>
/// <param name="ImplementedCount">已实施明细数</param>
/// <param name="TotalCount">明细总数</param>
public sealed record TaktEcImplementationStageSnapshot(
    string DeptCode,
    int ImplementedCount,
    int TotalCount);
