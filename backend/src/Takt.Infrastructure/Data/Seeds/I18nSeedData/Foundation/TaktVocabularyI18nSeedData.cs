// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktVocabularyI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktVocabulary 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktVocabulary 实体国际化翻译种子（键前缀 entity.vocabulary.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktVocabularyI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktVocabulary 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 vocabulary 实体翻译...", tenantCode);

        foreach (var item in GetVocabularyTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktVocabulary 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktVocabulary 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.vocabulary._self / entity.vocabulary.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetVocabularyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.vocabulary._self
            new TranslationSeedItem("entity.vocabulary._self", "en-US", "Vocabulary Information_us", "实体名称"),
            // entity.vocabulary._self
            new TranslationSeedItem("entity.vocabulary._self", "ja-JP", "敏感词信息_jp", "实体名称"),
            // entity.vocabulary._self
            new TranslationSeedItem("entity.vocabulary._self", "zh-CN", "敏感词信息", "实体名称"),
            // entity.vocabulary._self
            new TranslationSeedItem("entity.vocabulary._self", "zh-HK", "敏感词信息_hk", "实体名称"),

            // entity.vocabulary.wordtext
            new TranslationSeedItem("entity.vocabulary.wordtext", "en-US", "敏感词文本_us", "敏感词文本（租户内唯一）"),
            // entity.vocabulary.wordtext
            new TranslationSeedItem("entity.vocabulary.wordtext", "ja-JP", "敏感词文本_jp", "敏感词文本（租户内唯一）"),
            // entity.vocabulary.wordtext
            new TranslationSeedItem("entity.vocabulary.wordtext", "zh-CN", "敏感词文本", "敏感词文本（租户内唯一）"),
            // entity.vocabulary.wordtext
            new TranslationSeedItem("entity.vocabulary.wordtext", "zh-HK", "敏感词文本_hk", "敏感词文本（租户内唯一）"),

            // entity.vocabulary.wordcategory
            new TranslationSeedItem("entity.vocabulary.wordcategory", "en-US", "词性类别_us", "词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）"),
            // entity.vocabulary.wordcategory
            new TranslationSeedItem("entity.vocabulary.wordcategory", "ja-JP", "词性类别_jp", "词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）"),
            // entity.vocabulary.wordcategory
            new TranslationSeedItem("entity.vocabulary.wordcategory", "zh-CN", "词性类别", "词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）"),
            // entity.vocabulary.wordcategory
            new TranslationSeedItem("entity.vocabulary.wordcategory", "zh-HK", "词性类别_hk", "词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）"),

            // entity.vocabulary.filterlevel
            new TranslationSeedItem("entity.vocabulary.filterlevel", "en-US", "过滤等级_us", "过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）"),
            // entity.vocabulary.filterlevel
            new TranslationSeedItem("entity.vocabulary.filterlevel", "ja-JP", "过滤等级_jp", "过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）"),
            // entity.vocabulary.filterlevel
            new TranslationSeedItem("entity.vocabulary.filterlevel", "zh-CN", "过滤等级", "过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）"),
            // entity.vocabulary.filterlevel
            new TranslationSeedItem("entity.vocabulary.filterlevel", "zh-HK", "过滤等级_hk", "过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）"),

            // entity.vocabulary.replacetext
            new TranslationSeedItem("entity.vocabulary.replacetext", "en-US", "替换文本_us", "替换文本（默认 *Takt*）"),
            // entity.vocabulary.replacetext
            new TranslationSeedItem("entity.vocabulary.replacetext", "ja-JP", "替换文本_jp", "替换文本（默认 *Takt*）"),
            // entity.vocabulary.replacetext
            new TranslationSeedItem("entity.vocabulary.replacetext", "zh-CN", "替换文本", "替换文本（默认 *Takt*）"),
            // entity.vocabulary.replacetext
            new TranslationSeedItem("entity.vocabulary.replacetext", "zh-HK", "替换文本_hk", "替换文本（默认 *Takt*）"),

            // entity.vocabulary.status
            new TranslationSeedItem("entity.vocabulary.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.vocabulary.status
            new TranslationSeedItem("entity.vocabulary.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.vocabulary.status
            new TranslationSeedItem("entity.vocabulary.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.vocabulary.status
            new TranslationSeedItem("entity.vocabulary.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Foundation";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
