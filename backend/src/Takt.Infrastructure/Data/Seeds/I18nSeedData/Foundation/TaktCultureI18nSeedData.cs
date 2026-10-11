// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktCultureI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCulture 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCulture 实体国际化翻译种子（键前缀 entity.culture.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCultureI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCulture 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 culture 实体翻译...", tenantCode);

        foreach (var item in GetCultureTranslations())
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

        TaktLogger.Information("TaktCulture 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCulture 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.culture._self / entity.culture.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCultureTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.culture._self
            new TranslationSeedItem("entity.culture._self", "en-US", "Culture Information_us", "实体名称"),
            // entity.culture._self
            new TranslationSeedItem("entity.culture._self", "ja-JP", "区域文化信息_jp", "实体名称"),
            // entity.culture._self
            new TranslationSeedItem("entity.culture._self", "zh-CN", "区域文化信息", "实体名称"),
            // entity.culture._self
            new TranslationSeedItem("entity.culture._self", "zh-HK", "区域文化信息_hk", "实体名称"),

            // entity.culture.code
            new TranslationSeedItem("entity.culture.code", "en-US", "区域文化_us", "区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）"),
            // entity.culture.code
            new TranslationSeedItem("entity.culture.code", "ja-JP", "区域文化_jp", "区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）"),
            // entity.culture.code
            new TranslationSeedItem("entity.culture.code", "zh-CN", "区域文化", "区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）"),
            // entity.culture.code
            new TranslationSeedItem("entity.culture.code", "zh-HK", "区域文化_hk", "区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）"),

            // entity.culture.nativename
            new TranslationSeedItem("entity.culture.nativename", "en-US", "本地化名称_us", "本地化名称（用该语言显示的自身名称，如：中文、English；nvarchar Length=40）"),
            // entity.culture.nativename
            new TranslationSeedItem("entity.culture.nativename", "ja-JP", "本地化名称_jp", "本地化名称（用该语言显示的自身名称，如：中文、English；nvarchar Length=40）"),
            // entity.culture.nativename
            new TranslationSeedItem("entity.culture.nativename", "zh-CN", "本地化名称", "本地化名称（用该语言显示的自身名称，如：中文、English；nvarchar Length=40）"),
            // entity.culture.nativename
            new TranslationSeedItem("entity.culture.nativename", "zh-HK", "本地化名称_hk", "本地化名称（用该语言显示的自身名称，如：中文、English；nvarchar Length=40）"),

            // entity.culture.icon
            new TranslationSeedItem("entity.culture.icon", "en-US", "语言图标_us", "语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）"),
            // entity.culture.icon
            new TranslationSeedItem("entity.culture.icon", "ja-JP", "语言图标_jp", "语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）"),
            // entity.culture.icon
            new TranslationSeedItem("entity.culture.icon", "zh-CN", "语言图标", "语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）"),
            // entity.culture.icon
            new TranslationSeedItem("entity.culture.icon", "zh-HK", "语言图标_hk", "语言图标（flag-icons：fi-cn / fi-us / fi-jp，前端解析为 fi fi-xx）"),

            // entity.culture.isdefault
            new TranslationSeedItem("entity.culture.isdefault", "en-US", "默认语言_us", "默认语言（字典 sys_yes_no；1=是 0=否）"),
            // entity.culture.isdefault
            new TranslationSeedItem("entity.culture.isdefault", "ja-JP", "默认语言_jp", "默认语言（字典 sys_yes_no；1=是 0=否）"),
            // entity.culture.isdefault
            new TranslationSeedItem("entity.culture.isdefault", "zh-CN", "默认语言", "默认语言（字典 sys_yes_no；1=是 0=否）"),
            // entity.culture.isdefault
            new TranslationSeedItem("entity.culture.isdefault", "zh-HK", "默认语言_hk", "默认语言（字典 sys_yes_no；1=是 0=否）"),

            // entity.culture.sortorder
            new TranslationSeedItem("entity.culture.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.culture.sortorder
            new TranslationSeedItem("entity.culture.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.culture.sortorder
            new TranslationSeedItem("entity.culture.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.culture.sortorder
            new TranslationSeedItem("entity.culture.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.culture.translationlist
            new TranslationSeedItem("entity.culture.translationlist", "en-US", "翻译列表_us", "翻译列表（一对多关联）"),
            // entity.culture.translationlist
            new TranslationSeedItem("entity.culture.translationlist", "ja-JP", "翻译列表_jp", "翻译列表（一对多关联）"),
            // entity.culture.translationlist
            new TranslationSeedItem("entity.culture.translationlist", "zh-CN", "翻译列表", "翻译列表（一对多关联）"),
            // entity.culture.translationlist
            new TranslationSeedItem("entity.culture.translationlist", "zh-HK", "翻译列表_hk", "翻译列表（一对多关联）"),
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
