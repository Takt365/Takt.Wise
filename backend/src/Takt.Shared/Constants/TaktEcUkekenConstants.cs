// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcUkekenConstants.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变受检课执行常量（新品无需检验时自动完成文案）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变受检课（TaktEcUkeken）执行常量
/// </summary>
public static class TaktEcUkekenConstants
{
    /// <summary>
    /// 新品无需检验时自动写入的执行内容（英/日/中）
    /// </summary>
    public const string NotRelatedToIqcExecContent = "Not related to IQC（IQC 関連なし，跟IQC无关）";
}
