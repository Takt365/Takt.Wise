// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktExchangeRateI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktExchangeRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktExchangeRate 实体国际化翻译种子（键前缀 entity.exchangerate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktExchangeRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktExchangeRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 exchangerate 实体翻译...", tenantCode);

        foreach (var item in GetExchangeRateTranslations())
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

        TaktLogger.Information("TaktExchangeRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktExchangeRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.exchangerate._self / entity.exchangerate.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetExchangeRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.exchangerate._self
            new TranslationSeedItem("entity.exchangerate._self", "en-US", "Exchange Rate Information_us", "实体名称"),
            // entity.exchangerate._self
            new TranslationSeedItem("entity.exchangerate._self", "ja-JP", "汇率信息_jp", "实体名称"),
            // entity.exchangerate._self
            new TranslationSeedItem("entity.exchangerate._self", "zh-CN", "汇率信息", "实体名称"),
            // entity.exchangerate._self
            new TranslationSeedItem("entity.exchangerate._self", "zh-HK", "汇率信息_hk", "实体名称"),

            // entity.exchangerate.fromcurrencycode
            new TranslationSeedItem("entity.exchangerate.fromcurrencycode", "en-US", "源币种_us", "源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）"),
            // entity.exchangerate.fromcurrencycode
            new TranslationSeedItem("entity.exchangerate.fromcurrencycode", "ja-JP", "源币种_jp", "源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）"),
            // entity.exchangerate.fromcurrencycode
            new TranslationSeedItem("entity.exchangerate.fromcurrencycode", "zh-CN", "源币种", "源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）"),
            // entity.exchangerate.fromcurrencycode
            new TranslationSeedItem("entity.exchangerate.fromcurrencycode", "zh-HK", "源币种_hk", "源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）"),

            // entity.exchangerate.tocurrencycode
            new TranslationSeedItem("entity.exchangerate.tocurrencycode", "en-US", "目标币种_us", "目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）"),
            // entity.exchangerate.tocurrencycode
            new TranslationSeedItem("entity.exchangerate.tocurrencycode", "ja-JP", "目标币种_jp", "目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）"),
            // entity.exchangerate.tocurrencycode
            new TranslationSeedItem("entity.exchangerate.tocurrencycode", "zh-CN", "目标币种", "目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）"),
            // entity.exchangerate.tocurrencycode
            new TranslationSeedItem("entity.exchangerate.tocurrencycode", "zh-HK", "目标币种_hk", "目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）"),

            // entity.exchangerate.type
            new TranslationSeedItem("entity.exchangerate.type", "en-US", "汇率类型_us", "汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）"),
            // entity.exchangerate.type
            new TranslationSeedItem("entity.exchangerate.type", "ja-JP", "汇率类型_jp", "汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）"),
            // entity.exchangerate.type
            new TranslationSeedItem("entity.exchangerate.type", "zh-CN", "汇率类型", "汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）"),
            // entity.exchangerate.type
            new TranslationSeedItem("entity.exchangerate.type", "zh-HK", "汇率类型_hk", "汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）"),

            // entity.exchangerate.exchangerate
            new TranslationSeedItem("entity.exchangerate.exchangerate", "en-US", "汇率_us", "汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）"),
            // entity.exchangerate.exchangerate
            new TranslationSeedItem("entity.exchangerate.exchangerate", "ja-JP", "汇率_jp", "汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）"),
            // entity.exchangerate.exchangerate
            new TranslationSeedItem("entity.exchangerate.exchangerate", "zh-CN", "汇率", "汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）"),
            // entity.exchangerate.exchangerate
            new TranslationSeedItem("entity.exchangerate.exchangerate", "zh-HK", "汇率_hk", "汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）"),

            // entity.exchangerate.ratiofrom
            new TranslationSeedItem("entity.exchangerate.ratiofrom", "en-US", "源币种基数_us", "源币种换算基数（配合 RatioTo 支持间接标价；默认 1）"),
            // entity.exchangerate.ratiofrom
            new TranslationSeedItem("entity.exchangerate.ratiofrom", "ja-JP", "源币种基数_jp", "源币种换算基数（配合 RatioTo 支持间接标价；默认 1）"),
            // entity.exchangerate.ratiofrom
            new TranslationSeedItem("entity.exchangerate.ratiofrom", "zh-CN", "源币种基数", "源币种换算基数（配合 RatioTo 支持间接标价；默认 1）"),
            // entity.exchangerate.ratiofrom
            new TranslationSeedItem("entity.exchangerate.ratiofrom", "zh-HK", "源币种基数_hk", "源币种换算基数（配合 RatioTo 支持间接标价；默认 1）"),

            // entity.exchangerate.ratioto
            new TranslationSeedItem("entity.exchangerate.ratioto", "en-US", "目标币种基数_us", "目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）"),
            // entity.exchangerate.ratioto
            new TranslationSeedItem("entity.exchangerate.ratioto", "ja-JP", "目标币种基数_jp", "目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）"),
            // entity.exchangerate.ratioto
            new TranslationSeedItem("entity.exchangerate.ratioto", "zh-CN", "目标币种基数", "目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）"),
            // entity.exchangerate.ratioto
            new TranslationSeedItem("entity.exchangerate.ratioto", "zh-HK", "目标币种基数_hk", "目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）"),

            // entity.exchangerate.validfrom
            new TranslationSeedItem("entity.exchangerate.validfrom", "en-US", "生效日期_us", "生效日期"),
            // entity.exchangerate.validfrom
            new TranslationSeedItem("entity.exchangerate.validfrom", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.exchangerate.validfrom
            new TranslationSeedItem("entity.exchangerate.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.exchangerate.validfrom
            new TranslationSeedItem("entity.exchangerate.validfrom", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.exchangerate.validto
            new TranslationSeedItem("entity.exchangerate.validto", "en-US", "失效日期_us", "失效日期"),
            // entity.exchangerate.validto
            new TranslationSeedItem("entity.exchangerate.validto", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.exchangerate.validto
            new TranslationSeedItem("entity.exchangerate.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.exchangerate.validto
            new TranslationSeedItem("entity.exchangerate.validto", "zh-HK", "失效日期_hk", "失效日期"),

            // entity.exchangerate.status
            new TranslationSeedItem("entity.exchangerate.status", "en-US", "汇率状态_us", "汇率状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.exchangerate.status
            new TranslationSeedItem("entity.exchangerate.status", "ja-JP", "汇率状态_jp", "汇率状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.exchangerate.status
            new TranslationSeedItem("entity.exchangerate.status", "zh-CN", "汇率状态", "汇率状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.exchangerate.status
            new TranslationSeedItem("entity.exchangerate.status", "zh-HK", "汇率状态_hk", "汇率状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
