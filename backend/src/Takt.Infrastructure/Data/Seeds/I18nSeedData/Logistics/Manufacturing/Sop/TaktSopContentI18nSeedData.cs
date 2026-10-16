// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopContentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopContent 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopContent 实体国际化翻译种子（键前缀 entity.sopcontent.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopContentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopContent 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopcontent 实体翻译...", tenantCode);

        foreach (var item in GetSopContentTranslations())
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

        TaktLogger.Information("TaktSopContent 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopContent 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopcontent._self / entity.sopcontent.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopContentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopcontent._self
            new TranslationSeedItem("entity.sopcontent._self", "en-US", "Sop Content Information_us", "实体名称"),
            // entity.sopcontent._self
            new TranslationSeedItem("entity.sopcontent._self", "ja-JP", "SOP 多语言正文信息_jp", "实体名称"),
            // entity.sopcontent._self
            new TranslationSeedItem("entity.sopcontent._self", "zh-CN", "SOP 多语言正文信息", "实体名称"),
            // entity.sopcontent._self
            new TranslationSeedItem("entity.sopcontent._self", "zh-HK", "SOP 多语言正文信息_hk", "实体名称"),

            // entity.sopcontent.revisionid
            new TranslationSeedItem("entity.sopcontent.revisionid", "en-US", "版本ID_us", "版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopcontent.revisionid
            new TranslationSeedItem("entity.sopcontent.revisionid", "ja-JP", "版本ID_jp", "版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopcontent.revisionid
            new TranslationSeedItem("entity.sopcontent.revisionid", "zh-CN", "版本ID", "版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),
            // entity.sopcontent.revisionid
            new TranslationSeedItem("entity.sopcontent.revisionid", "zh-HK", "版本ID_hk", "版本 ID（选项 TaktSopRevisions/options；DictValue=Id）"),

            // entity.sopcontent.sopid
            new TranslationSeedItem("entity.sopcontent.sopid", "en-US", "SOP主档ID_us", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopcontent.sopid
            new TranslationSeedItem("entity.sopcontent.sopid", "ja-JP", "SOP主档ID_jp", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopcontent.sopid
            new TranslationSeedItem("entity.sopcontent.sopid", "zh-CN", "SOP主档ID", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),
            // entity.sopcontent.sopid
            new TranslationSeedItem("entity.sopcontent.sopid", "zh-HK", "SOP主档ID_hk", "SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）"),

            // entity.sopcontent.contenttitle
            new TranslationSeedItem("entity.sopcontent.contenttitle", "en-US", "正文标题_us", "正文标题"),
            // entity.sopcontent.contenttitle
            new TranslationSeedItem("entity.sopcontent.contenttitle", "ja-JP", "正文标题_jp", "正文标题"),
            // entity.sopcontent.contenttitle
            new TranslationSeedItem("entity.sopcontent.contenttitle", "zh-CN", "正文标题", "正文标题"),
            // entity.sopcontent.contenttitle
            new TranslationSeedItem("entity.sopcontent.contenttitle", "zh-HK", "正文标题_hk", "正文标题"),

            // entity.sopcontent.revision
            new TranslationSeedItem("entity.sopcontent.revision", "en-US", "版本_us", "版本"),
            // entity.sopcontent.revision
            new TranslationSeedItem("entity.sopcontent.revision", "ja-JP", "版本_jp", "版本"),
            // entity.sopcontent.revision
            new TranslationSeedItem("entity.sopcontent.revision", "zh-CN", "版本", "版本"),
            // entity.sopcontent.revision
            new TranslationSeedItem("entity.sopcontent.revision", "zh-HK", "版本_hk", "版本"),

            // entity.sopcontent.steps
            new TranslationSeedItem("entity.sopcontent.steps", "en-US", "工步列表_us", "工步列表"),
            // entity.sopcontent.steps
            new TranslationSeedItem("entity.sopcontent.steps", "ja-JP", "工步列表_jp", "工步列表"),
            // entity.sopcontent.steps
            new TranslationSeedItem("entity.sopcontent.steps", "zh-CN", "工步列表", "工步列表"),
            // entity.sopcontent.steps
            new TranslationSeedItem("entity.sopcontent.steps", "zh-HK", "工步列表_hk", "工步列表"),
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
        translation.ResourceGroup = "Sop";
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
