// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsFavoriteI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsFavorite 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter;

/// <summary>
/// TaktNewsFavorite 实体国际化翻译种子（键前缀 entity.newsfavorite.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsFavoriteI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsFavorite 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newsfavorite 实体翻译...", tenantCode);

        foreach (var item in GetNewsFavoriteTranslations())
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

        TaktLogger.Information("TaktNewsFavorite 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsFavorite 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newsfavorite._self / entity.newsfavorite.{{field}}；ResourceGroup=NewsCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNewsFavoriteTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newsfavorite._self
            new TranslationSeedItem("entity.newsfavorite._self", "en-US", "News Favorite Information_us", "实体名称"),
            // entity.newsfavorite._self
            new TranslationSeedItem("entity.newsfavorite._self", "ja-JP", "新闻中心收藏记录信息_jp", "实体名称"),
            // entity.newsfavorite._self
            new TranslationSeedItem("entity.newsfavorite._self", "zh-CN", "新闻中心收藏记录信息", "实体名称"),
            // entity.newsfavorite._self
            new TranslationSeedItem("entity.newsfavorite._self", "zh-HK", "新闻中心收藏记录信息_hk", "实体名称"),

            // entity.newsfavorite.newsid
            new TranslationSeedItem("entity.newsfavorite.newsid", "en-US", "新闻ID_us", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newsfavorite.newsid
            new TranslationSeedItem("entity.newsfavorite.newsid", "ja-JP", "新闻ID_jp", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newsfavorite.newsid
            new TranslationSeedItem("entity.newsfavorite.newsid", "zh-CN", "新闻ID", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newsfavorite.newsid
            new TranslationSeedItem("entity.newsfavorite.newsid", "zh-HK", "新闻ID_hk", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),

            // entity.newsfavorite.linenumber
            new TranslationSeedItem("entity.newsfavorite.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.newsfavorite.linenumber
            new TranslationSeedItem("entity.newsfavorite.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.newsfavorite.linenumber
            new TranslationSeedItem("entity.newsfavorite.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.newsfavorite.linenumber
            new TranslationSeedItem("entity.newsfavorite.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.newsfavorite.userid
            new TranslationSeedItem("entity.newsfavorite.userid", "en-US", "用户ID_us", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newsfavorite.userid
            new TranslationSeedItem("entity.newsfavorite.userid", "ja-JP", "用户ID_jp", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newsfavorite.userid
            new TranslationSeedItem("entity.newsfavorite.userid", "zh-CN", "用户ID", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newsfavorite.userid
            new TranslationSeedItem("entity.newsfavorite.userid", "zh-HK", "用户ID_hk", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.newsfavorite.username
            new TranslationSeedItem("entity.newsfavorite.username", "en-US", "用户姓名_us", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newsfavorite.username
            new TranslationSeedItem("entity.newsfavorite.username", "ja-JP", "用户姓名_jp", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newsfavorite.username
            new TranslationSeedItem("entity.newsfavorite.username", "zh-CN", "用户姓名", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newsfavorite.username
            new TranslationSeedItem("entity.newsfavorite.username", "zh-HK", "用户姓名_hk", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.newsfavorite.favoritetime
            new TranslationSeedItem("entity.newsfavorite.favoritetime", "en-US", "收藏时间_us", "收藏时间"),
            // entity.newsfavorite.favoritetime
            new TranslationSeedItem("entity.newsfavorite.favoritetime", "ja-JP", "收藏时间_jp", "收藏时间"),
            // entity.newsfavorite.favoritetime
            new TranslationSeedItem("entity.newsfavorite.favoritetime", "zh-CN", "收藏时间", "收藏时间"),
            // entity.newsfavorite.favoritetime
            new TranslationSeedItem("entity.newsfavorite.favoritetime", "zh-HK", "收藏时间_hk", "收藏时间"),

            // entity.newsfavorite.isobsolete
            new TranslationSeedItem("entity.newsfavorite.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newsfavorite.isobsolete
            new TranslationSeedItem("entity.newsfavorite.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newsfavorite.isobsolete
            new TranslationSeedItem("entity.newsfavorite.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newsfavorite.isobsolete
            new TranslationSeedItem("entity.newsfavorite.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.newsfavorite.news
            new TranslationSeedItem("entity.newsfavorite.news", "en-US", "新闻_us", "新闻（主表）"),
            // entity.newsfavorite.news
            new TranslationSeedItem("entity.newsfavorite.news", "ja-JP", "新闻_jp", "新闻（主表）"),
            // entity.newsfavorite.news
            new TranslationSeedItem("entity.newsfavorite.news", "zh-CN", "新闻", "新闻（主表）"),
            // entity.newsfavorite.news
            new TranslationSeedItem("entity.newsfavorite.news", "zh-HK", "新闻_hk", "新闻（主表）"),
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
        translation.ResourceGroup = "NewsCenter";
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
