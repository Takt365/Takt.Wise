// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcImplementationStatusConstants.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施路径状态常量（品管课完成视为正式完成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变实施路径状态（按 TaktEcExec 汇总；正式完成以品管课 Qa 全部明细已实施为准）
/// </summary>
public static class TaktEcImplementationStatusConstants
{
    /// <summary>
    /// 未开始（各部门均未实施）
    /// </summary>
    public const int NotStarted = 0;
    /// <summary>
    /// 实施中（路径上存在未完成部门，且品管课尚未全部完成）
    /// </summary>
    public const int InProgress = 1;
    /// <summary>
    /// 正式完成（品管课全部明细已实施；制技 Te 可未完成）
    /// </summary>
    public const int OfficiallyCompleted = 2;
    /// <summary>
    /// 全部完成（含制技 Te 全部明细已实施）
    /// </summary>
    public const int FullyCompleted = 3;
}
