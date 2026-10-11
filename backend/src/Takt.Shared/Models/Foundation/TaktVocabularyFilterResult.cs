// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models
// 文件名称：TaktVocabularyFilterResult.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词过滤结果（原文、替换后文本及命中词列表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models;

/// <summary>
/// 敏感词过滤结果
/// </summary>
public class TaktVocabularyFilterResult
{
    /// <summary>
    /// 原始文本
    /// </summary>
    public string OriginalText { get; set; } = string.Empty;

    /// <summary>
    /// 过滤后的文本（未命中敏感词时与原文相同）
    /// </summary>
    public string FilteredText { get; set; } = string.Empty;

    /// <summary>
    /// 是否命中敏感词
    /// </summary>
    public bool HasSensitiveWord { get; set; }

    /// <summary>
    /// 命中的敏感词文本（去重，大小写不敏感）
    /// </summary>
    public IReadOnlyList<string> MatchedWords { get; set; } = Array.Empty<string>();
}
