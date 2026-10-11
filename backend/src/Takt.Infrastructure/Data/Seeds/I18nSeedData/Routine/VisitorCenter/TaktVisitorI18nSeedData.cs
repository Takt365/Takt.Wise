// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.VisitorCenter
// 文件名称：TaktVisitorI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktVisitor 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.VisitorCenter;

/// <summary>
/// TaktVisitor 实体国际化翻译种子（键前缀 entity.visitor.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktVisitorI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktVisitor 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 visitor 实体翻译...", tenantCode);

        foreach (var item in GetVisitorTranslations())
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

        TaktLogger.Information("TaktVisitor 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktVisitor 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.visitor._self / entity.visitor.{{field}}；ResourceGroup=VisitorCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetVisitorTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.visitor._self
            new TranslationSeedItem("entity.visitor._self", "en-US", "Visitor Information_us", "实体名称"),
            // entity.visitor._self
            new TranslationSeedItem("entity.visitor._self", "ja-JP", "来访接待主信息_jp", "实体名称"),
            // entity.visitor._self
            new TranslationSeedItem("entity.visitor._self", "zh-CN", "来访接待主信息", "实体名称"),
            // entity.visitor._self
            new TranslationSeedItem("entity.visitor._self", "zh-HK", "来访接待主信息_hk", "实体名称"),

            // entity.visitor.companyname
            new TranslationSeedItem("entity.visitor.companyname", "en-US", "来访公司名称_us", "来访公司名称"),
            // entity.visitor.companyname
            new TranslationSeedItem("entity.visitor.companyname", "ja-JP", "来访公司名称_jp", "来访公司名称"),
            // entity.visitor.companyname
            new TranslationSeedItem("entity.visitor.companyname", "zh-CN", "来访公司名称", "来访公司名称"),
            // entity.visitor.companyname
            new TranslationSeedItem("entity.visitor.companyname", "zh-HK", "来访公司名称_hk", "来访公司名称"),

            // entity.visitor.visitstarttime
            new TranslationSeedItem("entity.visitor.visitstarttime", "en-US", "参访开始时间_us", "参访开始时间"),
            // entity.visitor.visitstarttime
            new TranslationSeedItem("entity.visitor.visitstarttime", "ja-JP", "参访开始时间_jp", "参访开始时间"),
            // entity.visitor.visitstarttime
            new TranslationSeedItem("entity.visitor.visitstarttime", "zh-CN", "参访开始时间", "参访开始时间"),
            // entity.visitor.visitstarttime
            new TranslationSeedItem("entity.visitor.visitstarttime", "zh-HK", "参访开始时间_hk", "参访开始时间"),

            // entity.visitor.visitendtime
            new TranslationSeedItem("entity.visitor.visitendtime", "en-US", "参访结束时间_us", "参访结束时间"),
            // entity.visitor.visitendtime
            new TranslationSeedItem("entity.visitor.visitendtime", "ja-JP", "参访结束时间_jp", "参访结束时间"),
            // entity.visitor.visitendtime
            new TranslationSeedItem("entity.visitor.visitendtime", "zh-CN", "参访结束时间", "参访结束时间"),
            // entity.visitor.visitendtime
            new TranslationSeedItem("entity.visitor.visitendtime", "zh-HK", "参访结束时间_hk", "参访结束时间"),

            // entity.visitor.companions
            new TranslationSeedItem("entity.visitor.companions", "en-US", "来访人员列表_us", "来访人员列表"),
            // entity.visitor.companions
            new TranslationSeedItem("entity.visitor.companions", "ja-JP", "来访人员列表_jp", "来访人员列表"),
            // entity.visitor.companions
            new TranslationSeedItem("entity.visitor.companions", "zh-CN", "来访人员列表", "来访人员列表"),
            // entity.visitor.companions
            new TranslationSeedItem("entity.visitor.companions", "zh-HK", "来访人员列表_hk", "来访人员列表"),
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
        translation.ResourceGroup = "VisitorCenter";
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
