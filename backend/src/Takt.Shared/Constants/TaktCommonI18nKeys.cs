// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktCommonI18nKeys.cs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：通用 i18n 键常量（菜单按钮与页面工具栏共用 common.page.button.*）。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 通用翻译键常量（与 TaktCommonI18nSeedData 中 common.page.button.* 种子一致）。
/// </summary>
public static class TaktCommonI18nKeys
{
    /// <summary>
    /// 通用按钮文案键前缀（菜单按钮种子、工具栏 t() 共用）。
    /// </summary>
    public const string PageButtonPrefix = "common.page.button.";

    /// <summary>
    /// 按操作后缀生成菜单按钮 I18nKey（小写后缀）。
    /// </summary>
    /// <param name="actionSuffix">权限末段或操作后缀，如 create、export。</param>
    /// <returns>完整键，如 common.page.button.create。</returns>
    /// <exception cref="ArgumentException">actionSuffix 为空。</exception>
    public static string MenuButton(string actionSuffix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(actionSuffix);
        return PageButtonPrefix + actionSuffix.Trim().ToLowerInvariant();
    }
}
