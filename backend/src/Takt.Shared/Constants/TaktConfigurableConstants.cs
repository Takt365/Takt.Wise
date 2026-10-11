// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktConfigurableConstants.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表查询/导出行数默认与上限常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 定制报表行数限制常量（查询与导出共用）
/// </summary>
public static class TaktConfigurableConstants
{
    /// <summary>
    /// 单次查询/导出默认行数
    /// </summary>
    public const int DefaultRowLimit = 500;

    /// <summary>
    /// 单次查询/导出允许的最大行数
    /// </summary>
    public const int MaxRowLimit = 50000;
}
