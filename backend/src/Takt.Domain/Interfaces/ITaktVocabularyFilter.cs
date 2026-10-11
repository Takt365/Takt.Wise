// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktVocabularyFilter.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词过滤服务接口（基于租户词库 TaktVocabulary）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 敏感词过滤服务。从当前租户 TaktVocabulary 词库加载启用词条，
/// 供新闻评论、公告等 UGC 场景检测或替换敏感词。
/// </summary>
public interface ITaktVocabularyFilter
{
    /// <summary>
    /// 检测文本是否包含敏感词
    /// </summary>
    /// <param name="text">待检测文本</param>
    /// <param name="minFilterLevel">最低过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）；为空时匹配全部启用词条</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否命中敏感词</returns>
    Task<bool> ContainsSensitiveWordAsync(
        string? text,
        int? minFilterLevel = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 过滤文本：命中敏感词时按词条 ReplaceText 替换，默认为 *Takt*，为空时回退为等长 *
    /// </summary>
    /// <param name="text">待过滤文本</param>
    /// <param name="minFilterLevel">最低过滤等级；为空时匹配全部启用词条</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>过滤结果（含原文、替换后文本及命中词）</returns>
    Task<TaktVocabularyFilterResult> FilterAsync(
        string? text,
        int? minFilterLevel = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 清除当前租户敏感词缓存（词库增删改后调用）
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    Task InvalidateCacheAsync(CancellationToken cancellationToken = default);
}
