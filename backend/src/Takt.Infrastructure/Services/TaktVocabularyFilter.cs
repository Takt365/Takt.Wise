// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktVocabularyFilter.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词过滤实现（租户词库缓存 + 替换/检测）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Models;

namespace Takt.Infrastructure.Services;

/// <summary>
/// 敏感词过滤实现（ITaktVocabularyFilter）。
/// 按租户缓存启用词条，优先匹配较长词；ReplaceText 默认为 *Takt*，为空时回退为等长 *。
/// </summary>
public class TaktVocabularyFilter : ITaktVocabularyFilter
{
    private const string CacheKeyPrefix = "takt:vocabulary:filter:";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(30);

    private readonly ITaktTenantRepository<TaktVocabulary> _vocabularyRepository;
    private readonly ITaktCacheService _cacheService;
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// 初始化敏感词过滤器
    /// </summary>
    /// <param name="vocabularyRepository">敏感词仓储</param>
    /// <param name="cacheService">缓存服务</param>
    /// <param name="userContext">用户上下文（解析租户编码）</param>
    public TaktVocabularyFilter(
        ITaktTenantRepository<TaktVocabulary> vocabularyRepository,
        ITaktCacheService cacheService,
        ITaktUserContext userContext)
    {
        _vocabularyRepository = vocabularyRepository;
        _cacheService = cacheService;
        _userContext = userContext;
    }

    /// <summary>
    /// 检测文本是否包含敏感词
    /// 从当前租户缓存词库加载启用词条，按 minFilterLevel 筛选后大小写不敏感子串匹配
    /// </summary>
    /// <param name="text">待检测文本；空或 null 视为未命中</param>
    /// <param name="minFilterLevel">最低过滤等级（1=低，2=中，3=高）；为空时匹配全部启用词条</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否命中至少一个敏感词</returns>
    public async Task<bool> ContainsSensitiveWordAsync(
        string? text,
        int? minFilterLevel = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        var entries = await GetActiveEntriesAsync(cancellationToken);
        var filteredEntries = FilterByLevel(entries, minFilterLevel);
        foreach (var entry in filteredEntries)
        {
            if (text.Contains(entry.WordText, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 过滤文本：命中敏感词时按词条 ReplaceText 替换，默认为 *Takt*，为空时回退为等长 *
    /// 优先匹配较长词（词库按词长降序），返回原文、替换后文本及命中词列表
    /// </summary>
    /// <param name="text">待过滤文本</param>
    /// <param name="minFilterLevel">最低过滤等级；为空时匹配全部启用词条</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>过滤结果（含原文、替换后文本、是否命中及 MatchedWords）</returns>
    public async Task<TaktVocabularyFilterResult> FilterAsync(
        string? text,
        int? minFilterLevel = null,
        CancellationToken cancellationToken = default)
    {
        var originalText = text ?? string.Empty;
        if (string.IsNullOrEmpty(originalText))
        {
            return new TaktVocabularyFilterResult
            {
                OriginalText = originalText,
                FilteredText = originalText,
                HasSensitiveWord = false,
                MatchedWords = Array.Empty<string>(),
            };
        }
        var entries = await GetActiveEntriesAsync(cancellationToken);
        var filteredEntries = FilterByLevel(entries, minFilterLevel);
        var matchedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var filteredText = originalText;
        foreach (var entry in filteredEntries)
        {
            if (!filteredText.Contains(entry.WordText, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            matchedWords.Add(entry.WordText);
            var replacement = ResolveReplacement(entry);
            filteredText = ReplaceIgnoreCase(filteredText, entry.WordText, replacement);
        }
        return new TaktVocabularyFilterResult
        {
            OriginalText = originalText,
            FilteredText = filteredText,
            HasSensitiveWord = matchedWords.Count > 0,
            MatchedWords = matchedWords.ToList(),
        };
    }

    /// <summary>
    /// 清除当前租户敏感词缓存（词库增删改后调用）
    /// 缓存键格式 takt:vocabulary:filter:{tenantCode}；无租户上下文时直接返回
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示异步清除完成的任务</returns>
    public async Task InvalidateCacheAsync(CancellationToken cancellationToken = default)
    {
        var tenantCode = ResolveTenantCode();
        if (string.IsNullOrEmpty(tenantCode))
        {
            return;
        }
        await _cacheService.RemoveAsync(BuildCacheKey(tenantCode), cancellationToken);
    }

    /// <summary>
    /// 获取当前租户启用词条（带缓存，按词长降序）
    /// </summary>
    private async Task<IReadOnlyList<VocabularyEntry>> GetActiveEntriesAsync(CancellationToken cancellationToken)
    {
        var tenantCode = ResolveTenantCode();
        if (string.IsNullOrEmpty(tenantCode))
        {
            return Array.Empty<VocabularyEntry>();
        }
        return await _cacheService.GetOrCreateAsync(
            BuildCacheKey(tenantCode),
            () => LoadActiveEntriesAsync(cancellationToken),
            CacheExpiration,
            cancellationToken);
    }

    /// <summary>
    /// 从数据库加载启用词条
    /// </summary>
    private async Task<IReadOnlyList<VocabularyEntry>> LoadActiveEntriesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var list = await _vocabularyRepository.GetListAsync(x => x.VocabularyStatus == 1);
        return list
            .Where(x => !string.IsNullOrWhiteSpace(x.WordText))
            .Select(x => new VocabularyEntry(x.WordText, x.FilterLevel, x.ReplaceText))
            .OrderByDescending(x => x.WordText.Length)
            .ThenBy(x => x.WordText, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 按最低过滤等级筛选词条
    /// </summary>
    private static IReadOnlyList<VocabularyEntry> FilterByLevel(
        IReadOnlyList<VocabularyEntry> entries,
        int? minFilterLevel)
    {
        if (!minFilterLevel.HasValue)
        {
            return entries;
        }
        return entries.Where(x => x.FilterLevel >= minFilterLevel.Value).ToList();
    }

    /// <summary>
    /// 解析替换文本
    /// </summary>
    private static string ResolveReplacement(VocabularyEntry entry)
    {
        if (!string.IsNullOrEmpty(entry.ReplaceText))
        {
            return entry.ReplaceText;
        }
        return new string('*', entry.WordText.Length);
    }

    /// <summary>
    /// 大小写不敏感替换全部匹配项
    /// </summary>
    private static string ReplaceIgnoreCase(string input, string oldValue, string newValue)
    {
        if (string.IsNullOrEmpty(oldValue))
        {
            return input;
        }
        var builder = new StringBuilder(input.Length);
        var index = 0;
        while (index < input.Length)
        {
            var found = input.IndexOf(oldValue, index, StringComparison.OrdinalIgnoreCase);
            if (found < 0)
            {
                builder.Append(input, index, input.Length - index);
                break;
            }
            builder.Append(input, index, found - index);
            builder.Append(newValue);
            index = found + oldValue.Length;
        }
        return builder.ToString();
    }

    /// <summary>
    /// 解析当前租户编码
    /// </summary>
    private string ResolveTenantCode()
    {
        return _userContext.TenantCode?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 构建租户词库缓存键
    /// </summary>
    private static string BuildCacheKey(string tenantCode) => $"{CacheKeyPrefix}{tenantCode}";

    /// <summary>
    /// 敏感词词条快照
    /// </summary>
    private sealed record VocabularyEntry(string WordText, int FilterLevel, string? ReplaceText);
}
