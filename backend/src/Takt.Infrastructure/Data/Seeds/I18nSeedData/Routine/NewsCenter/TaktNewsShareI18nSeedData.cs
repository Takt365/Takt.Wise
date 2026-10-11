// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsShareI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsShare 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktNewsShare 实体国际化翻译种子（键前缀 entity.newsshare.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsShareI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsShare 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newsshare 实体翻译...", tenantCode);

        foreach (var item in GetNewsShareTranslations())
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

        TaktLogger.Information("TaktNewsShare 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsShare 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newsshare._self / entity.newsshare.{{field}}；ResourceGroup=NewsCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNewsShareTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newsshare._self
            new TranslationSeedItem("entity.newsshare._self", "en-US", "News Share Information_us", "实体名称"),
            // entity.newsshare._self
            new TranslationSeedItem("entity.newsshare._self", "ja-JP", "新闻中心分享记录信息_jp", "实体名称"),
            // entity.newsshare._self
            new TranslationSeedItem("entity.newsshare._self", "zh-CN", "新闻中心分享记录信息", "实体名称"),
            // entity.newsshare._self
            new TranslationSeedItem("entity.newsshare._self", "zh-HK", "新闻中心分享记录信息_hk", "实体名称"),

            // entity.newsshare.newsid
            new TranslationSeedItem("entity.newsshare.newsid", "en-US", "新闻ID_us", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newsshare.newsid
            new TranslationSeedItem("entity.newsshare.newsid", "ja-JP", "新闻ID_jp", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newsshare.newsid
            new TranslationSeedItem("entity.newsshare.newsid", "zh-CN", "新闻ID", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newsshare.newsid
            new TranslationSeedItem("entity.newsshare.newsid", "zh-HK", "新闻ID_hk", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),

            // entity.newsshare.linenumber
            new TranslationSeedItem("entity.newsshare.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.newsshare.linenumber
            new TranslationSeedItem("entity.newsshare.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.newsshare.linenumber
            new TranslationSeedItem("entity.newsshare.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.newsshare.linenumber
            new TranslationSeedItem("entity.newsshare.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.newsshare.userid
            new TranslationSeedItem("entity.newsshare.userid", "en-US", "分享人ID_us", "分享人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newsshare.userid
            new TranslationSeedItem("entity.newsshare.userid", "ja-JP", "分享人ID_jp", "分享人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newsshare.userid
            new TranslationSeedItem("entity.newsshare.userid", "zh-CN", "分享人ID", "分享人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newsshare.userid
            new TranslationSeedItem("entity.newsshare.userid", "zh-HK", "分享人ID_hk", "分享人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.newsshare.username
            new TranslationSeedItem("entity.newsshare.username", "en-US", "分享人姓名_us", "分享人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newsshare.username
            new TranslationSeedItem("entity.newsshare.username", "ja-JP", "分享人姓名_jp", "分享人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newsshare.username
            new TranslationSeedItem("entity.newsshare.username", "zh-CN", "分享人姓名", "分享人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newsshare.username
            new TranslationSeedItem("entity.newsshare.username", "zh-HK", "分享人姓名_hk", "分享人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.newsshare.sharechannel
            new TranslationSeedItem("entity.newsshare.sharechannel", "en-US", "分享渠道_us", "分享渠道（如 wechat、link 等）"),
            // entity.newsshare.sharechannel
            new TranslationSeedItem("entity.newsshare.sharechannel", "ja-JP", "分享渠道_jp", "分享渠道（如 wechat、link 等）"),
            // entity.newsshare.sharechannel
            new TranslationSeedItem("entity.newsshare.sharechannel", "zh-CN", "分享渠道", "分享渠道（如 wechat、link 等）"),
            // entity.newsshare.sharechannel
            new TranslationSeedItem("entity.newsshare.sharechannel", "zh-HK", "分享渠道_hk", "分享渠道（如 wechat、link 等）"),

            // entity.newsshare.sharetime
            new TranslationSeedItem("entity.newsshare.sharetime", "en-US", "分享时间_us", "分享时间"),
            // entity.newsshare.sharetime
            new TranslationSeedItem("entity.newsshare.sharetime", "ja-JP", "分享时间_jp", "分享时间"),
            // entity.newsshare.sharetime
            new TranslationSeedItem("entity.newsshare.sharetime", "zh-CN", "分享时间", "分享时间"),
            // entity.newsshare.sharetime
            new TranslationSeedItem("entity.newsshare.sharetime", "zh-HK", "分享时间_hk", "分享时间"),

            // entity.newsshare.isobsolete
            new TranslationSeedItem("entity.newsshare.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newsshare.isobsolete
            new TranslationSeedItem("entity.newsshare.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newsshare.isobsolete
            new TranslationSeedItem("entity.newsshare.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newsshare.isobsolete
            new TranslationSeedItem("entity.newsshare.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.newsshare.news
            new TranslationSeedItem("entity.newsshare.news", "en-US", "新闻_us", "新闻（主表）"),
            // entity.newsshare.news
            new TranslationSeedItem("entity.newsshare.news", "ja-JP", "新闻_jp", "新闻（主表）"),
            // entity.newsshare.news
            new TranslationSeedItem("entity.newsshare.news", "zh-CN", "新闻", "新闻（主表）"),
            // entity.newsshare.news
            new TranslationSeedItem("entity.newsshare.news", "zh-HK", "新闻_hk", "新闻（主表）"),
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
