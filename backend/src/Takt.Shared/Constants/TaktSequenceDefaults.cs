// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktSequenceDefaults.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：行号/排序号生成默认步长与起始值（仅 int const，非枚举）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 行号、排序号序列生成默认常量
/// </summary>
public static class TaktSequenceDefaults
{
    /// <summary>
    /// 明细行号默认起始值（currentMax 为 0 时）
    /// </summary>
    public const int LineNumberDefaultStart = 10;

    /// <summary>
    /// 明细行号递增步长
    /// </summary>
    public const int LineNumberStep = 10;

    /// <summary>
    /// 排序号（回填）默认起始值（currentMax 为 0 时）
    /// </summary>
    public const int SortOrderDefaultStart = 1;

    /// <summary>
    /// 排序号（回填）递增步长
    /// </summary>
    public const int SortOrderStep = 1;
}
