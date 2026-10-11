// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktTicketPriorityHelper.cs
// 功能描述：ITSM 紧急度×影响范围 → 优先级矩阵（与 sys_priority_level 字典对齐）
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 工单优先级矩阵：依据 ITSM 惯例由紧急度与影响范围推导优先级。
/// </summary>
public static class TaktTicketPriorityHelper
{
    private static readonly int[,] PriorityMatrix =
    {
        { 1, 2, 3 },
        { 2, 3, 4 },
        { 2, 3, 4 },
    };

    /// <summary>
    /// 将紧急度/影响范围规范为 1～3；非法或 0 视为 3（低）。
    /// </summary>
    /// <param name="level">原始等级</param>
    /// <returns>1、2 或 3</returns>
    public static int NormalizeLevel(int level) =>
        level is >= 1 and <= 3 ? level : 3;

    /// <summary>
    /// 根据 ITSM 3×3 矩阵计算优先级（字典 sys_priority_level）。
    /// </summary>
    /// <param name="urgency">紧急度 1～3</param>
    /// <param name="impact">影响范围 1～3</param>
    /// <returns>优先级 1～4</returns>
    public static int ResolvePriority(int urgency, int impact)
    {
        var u = NormalizeLevel(urgency);
        var i = NormalizeLevel(impact);
        return PriorityMatrix[u - 1, i - 1];
    }
}
