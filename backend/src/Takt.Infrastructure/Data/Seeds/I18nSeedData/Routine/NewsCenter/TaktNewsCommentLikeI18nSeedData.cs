// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsCommentLikeI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsCommentLike 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktNewsCommentLike 实体国际化翻译种子（键前缀 entity.newscommentlike.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsCommentLikeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsCommentLike 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newscommentlike 实体翻译...", tenantCode);

        foreach (var item in GetNewsCommentLikeTranslations())
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

        TaktLogger.Information("TaktNewsCommentLike 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsCommentLike 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newscommentlike._self / entity.newscommentlike.{{field}}；ResourceGroup=NewsCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNewsCommentLikeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newscommentlike._self
            new TranslationSeedItem("entity.newscommentlike._self", "en-US", "News Comment Like Information_us", "实体名称"),
            // entity.newscommentlike._self
            new TranslationSeedItem("entity.newscommentlike._self", "ja-JP", "新闻中心评论点赞记录信息_jp", "实体名称"),
            // entity.newscommentlike._self
            new TranslationSeedItem("entity.newscommentlike._self", "zh-CN", "新闻中心评论点赞记录信息", "实体名称"),
            // entity.newscommentlike._self
            new TranslationSeedItem("entity.newscommentlike._self", "zh-HK", "新闻中心评论点赞记录信息_hk", "实体名称"),

            // entity.newscommentlike.commentid
            new TranslationSeedItem("entity.newscommentlike.commentid", "en-US", "评论ID_us", "评论 ID（选项 TaktNewsComments/options；DictValue=Id）"),
            // entity.newscommentlike.commentid
            new TranslationSeedItem("entity.newscommentlike.commentid", "ja-JP", "评论ID_jp", "评论 ID（选项 TaktNewsComments/options；DictValue=Id）"),
            // entity.newscommentlike.commentid
            new TranslationSeedItem("entity.newscommentlike.commentid", "zh-CN", "评论ID", "评论 ID（选项 TaktNewsComments/options；DictValue=Id）"),
            // entity.newscommentlike.commentid
            new TranslationSeedItem("entity.newscommentlike.commentid", "zh-HK", "评论ID_hk", "评论 ID（选项 TaktNewsComments/options；DictValue=Id）"),

            // entity.newscommentlike.linenumber
            new TranslationSeedItem("entity.newscommentlike.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.newscommentlike.linenumber
            new TranslationSeedItem("entity.newscommentlike.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.newscommentlike.linenumber
            new TranslationSeedItem("entity.newscommentlike.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.newscommentlike.linenumber
            new TranslationSeedItem("entity.newscommentlike.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.newscommentlike.userid
            new TranslationSeedItem("entity.newscommentlike.userid", "en-US", "用户ID_us", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscommentlike.userid
            new TranslationSeedItem("entity.newscommentlike.userid", "ja-JP", "用户ID_jp", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscommentlike.userid
            new TranslationSeedItem("entity.newscommentlike.userid", "zh-CN", "用户ID", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscommentlike.userid
            new TranslationSeedItem("entity.newscommentlike.userid", "zh-HK", "用户ID_hk", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.newscommentlike.username
            new TranslationSeedItem("entity.newscommentlike.username", "en-US", "用户姓名_us", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscommentlike.username
            new TranslationSeedItem("entity.newscommentlike.username", "ja-JP", "用户姓名_jp", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscommentlike.username
            new TranslationSeedItem("entity.newscommentlike.username", "zh-CN", "用户姓名", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscommentlike.username
            new TranslationSeedItem("entity.newscommentlike.username", "zh-HK", "用户姓名_hk", "用户姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.newscommentlike.liketime
            new TranslationSeedItem("entity.newscommentlike.liketime", "en-US", "点赞时间_us", "点赞时间"),
            // entity.newscommentlike.liketime
            new TranslationSeedItem("entity.newscommentlike.liketime", "ja-JP", "点赞时间_jp", "点赞时间"),
            // entity.newscommentlike.liketime
            new TranslationSeedItem("entity.newscommentlike.liketime", "zh-CN", "点赞时间", "点赞时间"),
            // entity.newscommentlike.liketime
            new TranslationSeedItem("entity.newscommentlike.liketime", "zh-HK", "点赞时间_hk", "点赞时间"),

            // entity.newscommentlike.isobsolete
            new TranslationSeedItem("entity.newscommentlike.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newscommentlike.isobsolete
            new TranslationSeedItem("entity.newscommentlike.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newscommentlike.isobsolete
            new TranslationSeedItem("entity.newscommentlike.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newscommentlike.isobsolete
            new TranslationSeedItem("entity.newscommentlike.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.newscommentlike.comment
            new TranslationSeedItem("entity.newscommentlike.comment", "en-US", "评论_us", "评论（主表）"),
            // entity.newscommentlike.comment
            new TranslationSeedItem("entity.newscommentlike.comment", "ja-JP", "评论_jp", "评论（主表）"),
            // entity.newscommentlike.comment
            new TranslationSeedItem("entity.newscommentlike.comment", "zh-CN", "评论", "评论（主表）"),
            // entity.newscommentlike.comment
            new TranslationSeedItem("entity.newscommentlike.comment", "zh-HK", "评论_hk", "评论（主表）"),
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
