// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktFinancialPeriodI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFinancialPeriod 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktFinancialPeriod 实体国际化翻译种子（键前缀 entity.financialperiod.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFinancialPeriodI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFinancialPeriod 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 financialperiod 实体翻译...", tenantCode);

        foreach (var item in GetFinancialPeriodTranslations())
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

        TaktLogger.Information("TaktFinancialPeriod 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFinancialPeriod 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.financialperiod._self / entity.financialperiod.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFinancialPeriodTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.financialperiod._self
            new TranslationSeedItem("entity.financialperiod._self", "en-US", "Financial Period Information_us", "实体名称"),
            // entity.financialperiod._self
            new TranslationSeedItem("entity.financialperiod._self", "ja-JP", "财务期间信息_jp", "实体名称"),
            // entity.financialperiod._self
            new TranslationSeedItem("entity.financialperiod._self", "zh-CN", "财务期间信息", "实体名称"),
            // entity.financialperiod._self
            new TranslationSeedItem("entity.financialperiod._self", "zh-HK", "财务期间信息_hk", "实体名称"),

            // entity.financialperiod.countrycode
            new TranslationSeedItem("entity.financialperiod.countrycode", "en-US", "国家代码_us", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.financialperiod.countrycode
            new TranslationSeedItem("entity.financialperiod.countrycode", "ja-JP", "国家代码_jp", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.financialperiod.countrycode
            new TranslationSeedItem("entity.financialperiod.countrycode", "zh-CN", "国家代码", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.financialperiod.countrycode
            new TranslationSeedItem("entity.financialperiod.countrycode", "zh-HK", "国家代码_hk", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.financialperiod.financialyearcode
            new TranslationSeedItem("entity.financialperiod.financialyearcode", "en-US", "财务年度编码_us", "财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）"),
            // entity.financialperiod.financialyearcode
            new TranslationSeedItem("entity.financialperiod.financialyearcode", "ja-JP", "财务年度编码_jp", "财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）"),
            // entity.financialperiod.financialyearcode
            new TranslationSeedItem("entity.financialperiod.financialyearcode", "zh-CN", "财务年度编码", "财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）"),
            // entity.financialperiod.financialyearcode
            new TranslationSeedItem("entity.financialperiod.financialyearcode", "zh-HK", "财务年度编码_hk", "财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）"),

            // entity.financialperiod.periodcode
            new TranslationSeedItem("entity.financialperiod.periodcode", "en-US", "会计期间编码_us", "会计期间编码（YYYYMM，如 201101、202704）"),
            // entity.financialperiod.periodcode
            new TranslationSeedItem("entity.financialperiod.periodcode", "ja-JP", "会计期间编码_jp", "会计期间编码（YYYYMM，如 201101、202704）"),
            // entity.financialperiod.periodcode
            new TranslationSeedItem("entity.financialperiod.periodcode", "zh-CN", "会计期间编码", "会计期间编码（YYYYMM，如 201101、202704）"),
            // entity.financialperiod.periodcode
            new TranslationSeedItem("entity.financialperiod.periodcode", "zh-HK", "会计期间编码_hk", "会计期间编码（YYYYMM，如 201101、202704）"),

            // entity.financialperiod.calendaryear
            new TranslationSeedItem("entity.financialperiod.calendaryear", "en-US", "自然年_us", "自然年（日历年份）"),
            // entity.financialperiod.calendaryear
            new TranslationSeedItem("entity.financialperiod.calendaryear", "ja-JP", "自然年_jp", "自然年（日历年份）"),
            // entity.financialperiod.calendaryear
            new TranslationSeedItem("entity.financialperiod.calendaryear", "zh-CN", "自然年", "自然年（日历年份）"),
            // entity.financialperiod.calendaryear
            new TranslationSeedItem("entity.financialperiod.calendaryear", "zh-HK", "自然年_hk", "自然年（日历年份）"),

            // entity.financialperiod.calendarmonth
            new TranslationSeedItem("entity.financialperiod.calendarmonth", "en-US", "自然月_us", "自然月（1～12）"),
            // entity.financialperiod.calendarmonth
            new TranslationSeedItem("entity.financialperiod.calendarmonth", "ja-JP", "自然月_jp", "自然月（1～12）"),
            // entity.financialperiod.calendarmonth
            new TranslationSeedItem("entity.financialperiod.calendarmonth", "zh-CN", "自然月", "自然月（1～12）"),
            // entity.financialperiod.calendarmonth
            new TranslationSeedItem("entity.financialperiod.calendarmonth", "zh-HK", "自然月_hk", "自然月（1～12）"),

            // entity.financialperiod.financialquartercode
            new TranslationSeedItem("entity.financialperiod.financialquartercode", "en-US", "财季编码_us", "财季编码（随国家财年规则变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）"),
            // entity.financialperiod.financialquartercode
            new TranslationSeedItem("entity.financialperiod.financialquartercode", "ja-JP", "财季编码_jp", "财季编码（随国家财年规则变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）"),
            // entity.financialperiod.financialquartercode
            new TranslationSeedItem("entity.financialperiod.financialquartercode", "zh-CN", "财季编码", "财季编码（随国家财年规则变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）"),
            // entity.financialperiod.financialquartercode
            new TranslationSeedItem("entity.financialperiod.financialquartercode", "zh-HK", "财季编码_hk", "财季编码（随国家财年规则变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）"),

            // entity.financialperiod.isbuiltin
            new TranslationSeedItem("entity.financialperiod.isbuiltin", "en-US", "是否内置_us", "是否内置（字典 sys_yes_no；1=是，0=否）"),
            // entity.financialperiod.isbuiltin
            new TranslationSeedItem("entity.financialperiod.isbuiltin", "ja-JP", "是否内置_jp", "是否内置（字典 sys_yes_no；1=是，0=否）"),
            // entity.financialperiod.isbuiltin
            new TranslationSeedItem("entity.financialperiod.isbuiltin", "zh-CN", "是否内置", "是否内置（字典 sys_yes_no；1=是，0=否）"),
            // entity.financialperiod.isbuiltin
            new TranslationSeedItem("entity.financialperiod.isbuiltin", "zh-HK", "是否内置_hk", "是否内置（字典 sys_yes_no；1=是，0=否）"),
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
        translation.ResourceGroup = "Financial";
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
