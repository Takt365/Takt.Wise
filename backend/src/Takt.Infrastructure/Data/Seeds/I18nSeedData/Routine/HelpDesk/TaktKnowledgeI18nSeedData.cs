// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktKnowledgeI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktKnowledge 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktKnowledge 实体国际化翻译种子（键前缀 entity.knowledge.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktKnowledgeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktKnowledge 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 knowledge 实体翻译...", tenantCode);

        foreach (var item in GetKnowledgeTranslations())
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

        TaktLogger.Information("TaktKnowledge 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktKnowledge 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.knowledge._self / entity.knowledge.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetKnowledgeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.knowledge._self
            new TranslationSeedItem("entity.knowledge._self", "en-US", "Knowledge Information_us", "实体名称"),
            // entity.knowledge._self
            new TranslationSeedItem("entity.knowledge._self", "ja-JP", "服务台知识库信息_jp", "实体名称"),
            // entity.knowledge._self
            new TranslationSeedItem("entity.knowledge._self", "zh-CN", "服务台知识库信息", "实体名称"),
            // entity.knowledge._self
            new TranslationSeedItem("entity.knowledge._self", "zh-HK", "服务台知识库信息_hk", "实体名称"),

            // entity.knowledge.title
            new TranslationSeedItem("entity.knowledge.title", "en-US", "知识标题_us", "知识标题"),
            // entity.knowledge.title
            new TranslationSeedItem("entity.knowledge.title", "ja-JP", "知识标题_jp", "知识标题"),
            // entity.knowledge.title
            new TranslationSeedItem("entity.knowledge.title", "zh-CN", "知识标题", "知识标题"),
            // entity.knowledge.title
            new TranslationSeedItem("entity.knowledge.title", "zh-HK", "知识标题_hk", "知识标题"),

            // entity.knowledge.content
            new TranslationSeedItem("entity.knowledge.content", "en-US", "知识内容_us", "知识内容（富文本/HTML）"),
            // entity.knowledge.content
            new TranslationSeedItem("entity.knowledge.content", "ja-JP", "知识内容_jp", "知识内容（富文本/HTML）"),
            // entity.knowledge.content
            new TranslationSeedItem("entity.knowledge.content", "zh-CN", "知识内容", "知识内容（富文本/HTML）"),
            // entity.knowledge.content
            new TranslationSeedItem("entity.knowledge.content", "zh-HK", "知识内容_hk", "知识内容（富文本/HTML）"),

            // entity.knowledge.summary
            new TranslationSeedItem("entity.knowledge.summary", "en-US", "知识摘要_us", "知识摘要（简短描述，列表/搜索展示）"),
            // entity.knowledge.summary
            new TranslationSeedItem("entity.knowledge.summary", "ja-JP", "知识摘要_jp", "知识摘要（简短描述，列表/搜索展示）"),
            // entity.knowledge.summary
            new TranslationSeedItem("entity.knowledge.summary", "zh-CN", "知识摘要", "知识摘要（简短描述，列表/搜索展示）"),
            // entity.knowledge.summary
            new TranslationSeedItem("entity.knowledge.summary", "zh-HK", "知识摘要_hk", "知识摘要（简短描述，列表/搜索展示）"),

            // entity.knowledge.categorycode
            new TranslationSeedItem("entity.knowledge.categorycode", "en-US", "分类编码_us", "分类编码（业务编码；如 faq/guide）"),
            // entity.knowledge.categorycode
            new TranslationSeedItem("entity.knowledge.categorycode", "ja-JP", "分类编码_jp", "分类编码（业务编码；如 faq/guide）"),
            // entity.knowledge.categorycode
            new TranslationSeedItem("entity.knowledge.categorycode", "zh-CN", "分类编码", "分类编码（业务编码；如 faq/guide）"),
            // entity.knowledge.categorycode
            new TranslationSeedItem("entity.knowledge.categorycode", "zh-HK", "分类编码_hk", "分类编码（业务编码；如 faq/guide）"),

            // entity.knowledge.tags
            new TranslationSeedItem("entity.knowledge.tags", "en-US", "标签_us", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.knowledge.tags
            new TranslationSeedItem("entity.knowledge.tags", "ja-JP", "标签_jp", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.knowledge.tags
            new TranslationSeedItem("entity.knowledge.tags", "zh-CN", "标签", "标签（逗号分隔或 JSON 数组存储）"),
            // entity.knowledge.tags
            new TranslationSeedItem("entity.knowledge.tags", "zh-HK", "标签_hk", "标签（逗号分隔或 JSON 数组存储）"),

            // entity.knowledge.viewcount
            new TranslationSeedItem("entity.knowledge.viewcount", "en-US", "浏览次数_us", "浏览次数"),
            // entity.knowledge.viewcount
            new TranslationSeedItem("entity.knowledge.viewcount", "ja-JP", "浏览次数_jp", "浏览次数"),
            // entity.knowledge.viewcount
            new TranslationSeedItem("entity.knowledge.viewcount", "zh-CN", "浏览次数", "浏览次数"),
            // entity.knowledge.viewcount
            new TranslationSeedItem("entity.knowledge.viewcount", "zh-HK", "浏览次数_hk", "浏览次数"),

            // entity.knowledge.helpfulcount
            new TranslationSeedItem("entity.knowledge.helpfulcount", "en-US", "有用评价数_us", "有用评价数"),
            // entity.knowledge.helpfulcount
            new TranslationSeedItem("entity.knowledge.helpfulcount", "ja-JP", "有用评价数_jp", "有用评价数"),
            // entity.knowledge.helpfulcount
            new TranslationSeedItem("entity.knowledge.helpfulcount", "zh-CN", "有用评价数", "有用评价数"),
            // entity.knowledge.helpfulcount
            new TranslationSeedItem("entity.knowledge.helpfulcount", "zh-HK", "有用评价数_hk", "有用评价数"),

            // entity.knowledge.unhelpfulcount
            new TranslationSeedItem("entity.knowledge.unhelpfulcount", "en-US", "无帮助评价数_us", "无帮助评价数"),
            // entity.knowledge.unhelpfulcount
            new TranslationSeedItem("entity.knowledge.unhelpfulcount", "ja-JP", "无帮助评价数_jp", "无帮助评价数"),
            // entity.knowledge.unhelpfulcount
            new TranslationSeedItem("entity.knowledge.unhelpfulcount", "zh-CN", "无帮助评价数", "无帮助评价数"),
            // entity.knowledge.unhelpfulcount
            new TranslationSeedItem("entity.knowledge.unhelpfulcount", "zh-HK", "无帮助评价数_hk", "无帮助评价数"),

            // entity.knowledge.ispublished
            new TranslationSeedItem("entity.knowledge.ispublished", "en-US", "是否已发布_us", "是否已发布（字典 sys_yes_no；0=否 1=是）"),
            // entity.knowledge.ispublished
            new TranslationSeedItem("entity.knowledge.ispublished", "ja-JP", "是否已发布_jp", "是否已发布（字典 sys_yes_no；0=否 1=是）"),
            // entity.knowledge.ispublished
            new TranslationSeedItem("entity.knowledge.ispublished", "zh-CN", "是否已发布", "是否已发布（字典 sys_yes_no；0=否 1=是）"),
            // entity.knowledge.ispublished
            new TranslationSeedItem("entity.knowledge.ispublished", "zh-HK", "是否已发布_hk", "是否已发布（字典 sys_yes_no；0=否 1=是）"),

            // entity.knowledge.version
            new TranslationSeedItem("entity.knowledge.version", "en-US", "版本号_us", "版本号"),
            // entity.knowledge.version
            new TranslationSeedItem("entity.knowledge.version", "ja-JP", "版本号_jp", "版本号"),
            // entity.knowledge.version
            new TranslationSeedItem("entity.knowledge.version", "zh-CN", "版本号", "版本号"),
            // entity.knowledge.version
            new TranslationSeedItem("entity.knowledge.version", "zh-HK", "版本号_hk", "版本号"),

            // entity.knowledge.publishedat
            new TranslationSeedItem("entity.knowledge.publishedat", "en-US", "发布时间_us", "发布时间"),
            // entity.knowledge.publishedat
            new TranslationSeedItem("entity.knowledge.publishedat", "ja-JP", "发布时间_jp", "发布时间"),
            // entity.knowledge.publishedat
            new TranslationSeedItem("entity.knowledge.publishedat", "zh-CN", "发布时间", "发布时间"),
            // entity.knowledge.publishedat
            new TranslationSeedItem("entity.knowledge.publishedat", "zh-HK", "发布时间_hk", "发布时间"),

            // entity.knowledge.revisedat
            new TranslationSeedItem("entity.knowledge.revisedat", "en-US", "最后修订时间_us", "最后修订时间"),
            // entity.knowledge.revisedat
            new TranslationSeedItem("entity.knowledge.revisedat", "ja-JP", "最后修订时间_jp", "最后修订时间"),
            // entity.knowledge.revisedat
            new TranslationSeedItem("entity.knowledge.revisedat", "zh-CN", "最后修订时间", "最后修订时间"),
            // entity.knowledge.revisedat
            new TranslationSeedItem("entity.knowledge.revisedat", "zh-HK", "最后修订时间_hk", "最后修订时间"),

            // entity.knowledge.sortorder
            new TranslationSeedItem("entity.knowledge.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.knowledge.sortorder
            new TranslationSeedItem("entity.knowledge.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.knowledge.sortorder
            new TranslationSeedItem("entity.knowledge.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.knowledge.sortorder
            new TranslationSeedItem("entity.knowledge.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.knowledge.status
            new TranslationSeedItem("entity.knowledge.status", "en-US", "知识状态_us", "知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）"),
            // entity.knowledge.status
            new TranslationSeedItem("entity.knowledge.status", "ja-JP", "知识状态_jp", "知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）"),
            // entity.knowledge.status
            new TranslationSeedItem("entity.knowledge.status", "zh-CN", "知识状态", "知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）"),
            // entity.knowledge.status
            new TranslationSeedItem("entity.knowledge.status", "zh-HK", "知识状态_hk", "知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）"),

            // entity.knowledge.attachments
            new TranslationSeedItem("entity.knowledge.attachments", "en-US", "附件JSON_us", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.knowledge.attachments
            new TranslationSeedItem("entity.knowledge.attachments", "ja-JP", "附件JSON_jp", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.knowledge.attachments
            new TranslationSeedItem("entity.knowledge.attachments", "zh-CN", "附件JSON", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
            // entity.knowledge.attachments
            new TranslationSeedItem("entity.knowledge.attachments", "zh-HK", "附件JSON_hk", "附件（JSON 列表形式，由 TaktFile 统一上传到服务器）"),
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
        translation.ResourceGroup = "HelpDesk";
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
