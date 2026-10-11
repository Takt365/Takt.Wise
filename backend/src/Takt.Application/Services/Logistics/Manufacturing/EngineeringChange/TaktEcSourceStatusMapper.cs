// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSourceStatusMapper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：来源设变 SourceStatus（PLM 英文状态）映射为 ChangeStatus（字典 logistics_manufacturing_ec_status 1～7）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 来源设变 PLM 英文状态 → 设变变更状态（logistics_manufacturing_ec_status）映射
/// </summary>
public static class TaktEcSourceStatusMapper
{
    /// <summary>
    /// 来源状态关键字与 ChangeStatus 对照（包含匹配，不区分大小写；顺序与来源系统 1～7 一致）
    /// </summary>
    private static readonly (int ChangeStatus, string Keyword)[] SourceKeywordToChangeStatusMap =
    [
        (1, "Work"),
        (2, "Cancel"),
        (3, "Issued"),
        (4, "Change"),
        (5, "Fixed"),
        (6, "Pending"),
        (7, "Rejected"),
    ];

    /// <summary>
    /// 尝试将来源设变 SourceStatus 映射为 ChangeStatus（字典 logistics_manufacturing_ec_status；1～7）
    /// </summary>
    /// <param name="sourceStatus">来源 PLM 状态文本（如 Work in Process、Issued、Fixed）</param>
    /// <param name="changeStatus">映射后的变更状态</param>
    /// <returns>是否识别并映射成功</returns>
    public static bool TryMapToChangeStatus(string? sourceStatus, out int changeStatus)
    {
        changeStatus = 0;
        if (string.IsNullOrWhiteSpace(sourceStatus))
        {
            return false;
        }
        var trimmed = sourceStatus.Trim();
        if (int.TryParse(trimmed, out var numeric) && numeric is >= 1 and <= 7)
        {
            changeStatus = numeric;
            return true;
        }
        foreach (var (status, keyword) in SourceKeywordToChangeStatusMap)
        {
            if (trimmed.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                changeStatus = status;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 将来源设变 SourceStatus 映射为 ChangeStatus；无法识别时抛出异常（导入场景）
    /// </summary>
    /// <param name="sourceStatus">来源 PLM 状态文本</param>
    /// <returns>变更状态 1～7</returns>
    /// <exception cref="InvalidOperationException">状态无法识别</exception>
    public static int MapToChangeStatusOrThrow(string? sourceStatus)
    {
        if (TryMapToChangeStatus(sourceStatus, out var changeStatus))
        {
            return changeStatus;
        }
        throw new InvalidOperationException($"来源设变状态无法映射为变更状态: [{sourceStatus ?? string.Empty}]");
    }
}
