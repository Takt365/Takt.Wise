// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcScopeExecApplyResult.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施范围派生各部门执行行结果（含部门写入条数摘要）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 单部门执行行写入统计
/// </summary>
public sealed class TaktEcDeptExecCountItem
{
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; init; } = string.Empty;

    /// <summary>
    /// 部门显示名
    /// </summary>
    public string DeptName { get; init; } = string.Empty;

    /// <summary>
    /// 本次写入/更新条数
    /// </summary>
    public int SavedCount { get; init; }
}

/// <summary>
/// 设变实施范围 → 部门执行行编排结果
/// </summary>
public sealed class TaktEcScopeExecApplyResult
{
    /// <summary>
    /// 空结果
    /// </summary>
    public static TaktEcScopeExecApplyResult Empty { get; } = new()
    {
        Items = Array.Empty<TaktEcDeptExecCountItem>(),
    };

    /// <summary>
    /// 各部门写入统计（仅 SavedCount&gt;0）
    /// </summary>
    public IReadOnlyList<TaktEcDeptExecCountItem> Items { get; init; } = Array.Empty<TaktEcDeptExecCountItem>();

    /// <summary>
    /// 涉及的部门编码
    /// </summary>
    public IReadOnlyList<string> DeptCodes => Items.Select(x => x.DeptCode).ToList();

    /// <summary>
    /// 写入总条数
    /// </summary>
    public int TotalSavedCount => Items.Sum(x => x.SavedCount);

    /// <summary>
    /// 格式化摘要（采购课 12 条；部管课 10 条）
    /// </summary>
    /// <returns>摘要文案；无写入时为空串</returns>
    public string FormatSummary()
    {
        if (Items.Count == 0)
        {
            return string.Empty;
        }
        return string.Join("；", Items.Select(x => $"{x.DeptName} {x.SavedCount} 条"));
    }

    /// <summary>
    /// 由部门编码与条数构建结果
    /// </summary>
    /// <param name="counts">部门编码 → 条数</param>
    /// <returns>结果</returns>
    public static TaktEcScopeExecApplyResult FromCounts(IReadOnlyDictionary<string, int> counts)
    {
        ArgumentNullException.ThrowIfNull(counts);
        var items = counts
            .Where(kv => kv.Value > 0)
            .Select(kv => new TaktEcDeptExecCountItem
            {
                DeptCode = kv.Key,
                DeptName = TaktEcDeptCodes.GetDisplayName(kv.Key),
                SavedCount = kv.Value,
            })
            .ToList();
        return new TaktEcScopeExecApplyResult { Items = items };
    }
}
