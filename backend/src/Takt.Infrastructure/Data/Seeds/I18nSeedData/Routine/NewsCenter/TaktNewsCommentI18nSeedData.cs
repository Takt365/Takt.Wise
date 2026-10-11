// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.NewsCenter
// 文件名称：TaktNewsCommentI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsComment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktNewsComment 实体国际化翻译种子（键前缀 entity.newscomment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsCommentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsComment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newscomment 实体翻译...", tenantCode);

        foreach (var item in GetNewsCommentTranslations())
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

        TaktLogger.Information("TaktNewsComment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsComment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newscomment._self / entity.newscomment.{{field}}；ResourceGroup=NewsCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNewsCommentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "en-US", "News Comment Information_us", "实体名称"),
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "ja-JP", "新闻中心评论信息_jp", "实体名称"),
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "zh-CN", "新闻中心评论信息", "实体名称"),
            // entity.newscomment._self
            new TranslationSeedItem("entity.newscomment._self", "zh-HK", "新闻中心评论信息_hk", "实体名称"),

            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "en-US", "新闻ID_us", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "ja-JP", "新闻ID_jp", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "zh-CN", "新闻ID", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),
            // entity.newscomment.newsid
            new TranslationSeedItem("entity.newscomment.newsid", "zh-HK", "新闻ID_hk", "新闻 ID（选项 TaktNews/options；DictValue=Id）"),

            // entity.newscomment.linenumber
            new TranslationSeedItem("entity.newscomment.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.newscomment.linenumber
            new TranslationSeedItem("entity.newscomment.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.newscomment.linenumber
            new TranslationSeedItem("entity.newscomment.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.newscomment.linenumber
            new TranslationSeedItem("entity.newscomment.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "en-US", "父评论ID_us", "父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）"),
            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "ja-JP", "父评论ID_jp", "父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）"),
            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "zh-CN", "父评论ID", "父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）"),
            // entity.newscomment.parentid
            new TranslationSeedItem("entity.newscomment.parentid", "zh-HK", "父评论ID_hk", "父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）"),

            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "en-US", "评论人ID_us", "评论人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "ja-JP", "评论人ID_jp", "评论人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "zh-CN", "评论人ID", "评论人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscomment.userid
            new TranslationSeedItem("entity.newscomment.userid", "zh-HK", "评论人ID_hk", "评论人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "en-US", "评论人姓名_us", "评论人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "ja-JP", "评论人姓名_jp", "评论人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "zh-CN", "评论人姓名", "评论人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.username
            new TranslationSeedItem("entity.newscomment.username", "zh-HK", "评论人姓名_hk", "评论人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "en-US", "评论人头像URL_us", "评论人头像 URL（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "ja-JP", "评论人头像URL_jp", "评论人头像 URL（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "zh-CN", "评论人头像URL", "评论人头像 URL（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.useravatar
            new TranslationSeedItem("entity.newscomment.useravatar", "zh-HK", "评论人头像URL_hk", "评论人头像 URL（冗余：按对应 Id 取主数据名称联动）"),

            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "en-US", "被回复人ID_us", "被回复人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "ja-JP", "被回复人ID_jp", "被回复人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "zh-CN", "被回复人ID", "被回复人 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.newscomment.replytouserid
            new TranslationSeedItem("entity.newscomment.replytouserid", "zh-HK", "被回复人ID_hk", "被回复人 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "en-US", "被回复人姓名_us", "被回复人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "ja-JP", "被回复人姓名_jp", "被回复人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "zh-CN", "被回复人姓名", "被回复人姓名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.newscomment.replytousername
            new TranslationSeedItem("entity.newscomment.replytousername", "zh-HK", "被回复人姓名_hk", "被回复人姓名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "en-US", "评论内容_us", "评论内容"),
            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "ja-JP", "评论内容_jp", "评论内容"),
            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "zh-CN", "评论内容", "评论内容"),
            // entity.newscomment.commentcontent
            new TranslationSeedItem("entity.newscomment.commentcontent", "zh-HK", "评论内容_hk", "评论内容"),

            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "en-US", "评论时间_us", "评论时间"),
            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "ja-JP", "评论时间_jp", "评论时间"),
            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "zh-CN", "评论时间", "评论时间"),
            // entity.newscomment.commenttime
            new TranslationSeedItem("entity.newscomment.commenttime", "zh-HK", "评论时间_hk", "评论时间"),

            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "en-US", "点赞次数_us", "点赞次数"),
            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "ja-JP", "点赞次数_jp", "点赞次数"),
            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "zh-CN", "点赞次数", "点赞次数"),
            // entity.newscomment.likecount
            new TranslationSeedItem("entity.newscomment.likecount", "zh-HK", "点赞次数_hk", "点赞次数"),

            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "en-US", "回复次数_us", "回复次数（子评论数量）"),
            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "ja-JP", "回复次数_jp", "回复次数（子评论数量）"),
            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "zh-CN", "回复次数", "回复次数（子评论数量）"),
            // entity.newscomment.replycount
            new TranslationSeedItem("entity.newscomment.replycount", "zh-HK", "回复次数_hk", "回复次数（子评论数量）"),

            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "en-US", "评论层级_us", "评论层级（0=顶级，最多 3 级）"),
            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "ja-JP", "评论层级_jp", "评论层级（0=顶级，最多 3 级）"),
            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "zh-CN", "评论层级", "评论层级（0=顶级，最多 3 级）"),
            // entity.newscomment.commentlevel
            new TranslationSeedItem("entity.newscomment.commentlevel", "zh-HK", "评论层级_hk", "评论层级（0=顶级，最多 3 级）"),

            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "en-US", "评论状态_us", "评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）"),
            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "ja-JP", "评论状态_jp", "评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）"),
            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "zh-CN", "评论状态", "评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）"),
            // entity.newscomment.commentstatus
            new TranslationSeedItem("entity.newscomment.commentstatus", "zh-HK", "评论状态_hk", "评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）"),

            // entity.newscomment.isobsolete
            new TranslationSeedItem("entity.newscomment.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newscomment.isobsolete
            new TranslationSeedItem("entity.newscomment.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newscomment.isobsolete
            new TranslationSeedItem("entity.newscomment.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.newscomment.isobsolete
            new TranslationSeedItem("entity.newscomment.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "en-US", "新闻_us", "新闻（主表）"),
            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "ja-JP", "新闻_jp", "新闻（主表）"),
            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "zh-CN", "新闻", "新闻（主表）"),
            // entity.newscomment.news
            new TranslationSeedItem("entity.newscomment.news", "zh-HK", "新闻_hk", "新闻（主表）"),

            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "en-US", "评论点赞记录列表_us", "评论点赞记录列表（主子表关系）"),
            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "ja-JP", "评论点赞记录列表_jp", "评论点赞记录列表（主子表关系）"),
            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "zh-CN", "评论点赞记录列表", "评论点赞记录列表（主子表关系）"),
            // entity.newscomment.likes
            new TranslationSeedItem("entity.newscomment.likes", "zh-HK", "评论点赞记录列表_hk", "评论点赞记录列表（主子表关系）"),
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
