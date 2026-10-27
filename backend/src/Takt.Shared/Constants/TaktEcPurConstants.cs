// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcPurConstants.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变采购课执行常量（非外部采购自动完成文案）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变采购课（TaktEcPur）执行常量
/// </summary>
public static class TaktEcPurConstants
{
    /// <summary>
    /// 新采购类型非 F 时自动写入的执行内容（英/日/中）
    /// </summary>
    public const string NotPurchasingRelatedExecContent = "Not purchasing related(購買関連なし,采购无关)";
}
