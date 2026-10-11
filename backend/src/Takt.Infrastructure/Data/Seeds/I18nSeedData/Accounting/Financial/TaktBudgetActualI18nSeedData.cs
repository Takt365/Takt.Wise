// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktBudgetActualI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBudgetActual 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBudgetActual 实体国际化翻译种子（键前缀 entity.budgetactual.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBudgetActualI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBudgetActual 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 budgetactual 实体翻译...", tenantCode);

        foreach (var item in GetBudgetActualTranslations())
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

        TaktLogger.Information("TaktBudgetActual 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBudgetActual 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.budgetactual._self / entity.budgetactual.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBudgetActualTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.budgetactual._self
            new TranslationSeedItem("entity.budgetactual._self", "en-US", "Budget Actual Information_us", "实体名称"),
            // entity.budgetactual._self
            new TranslationSeedItem("entity.budgetactual._self", "ja-JP", "预算实绩信息_jp", "实体名称"),
            // entity.budgetactual._self
            new TranslationSeedItem("entity.budgetactual._self", "zh-CN", "预算实绩信息", "实体名称"),
            // entity.budgetactual._self
            new TranslationSeedItem("entity.budgetactual._self", "zh-HK", "预算实绩信息_hk", "实体名称"),

            // entity.budgetactual.periodcode
            new TranslationSeedItem("entity.budgetactual.periodcode", "en-US", "会计期间_us", "会计期间编码（YYYYMM）"),
            // entity.budgetactual.periodcode
            new TranslationSeedItem("entity.budgetactual.periodcode", "ja-JP", "会计期间_jp", "会计期间编码（YYYYMM）"),
            // entity.budgetactual.periodcode
            new TranslationSeedItem("entity.budgetactual.periodcode", "zh-CN", "会计期间", "会计期间编码（YYYYMM）"),
            // entity.budgetactual.periodcode
            new TranslationSeedItem("entity.budgetactual.periodcode", "zh-HK", "会计期间_hk", "会计期间编码（YYYYMM）"),

            // entity.budgetactual.costcentercode
            new TranslationSeedItem("entity.budgetactual.costcentercode", "en-US", "成本中心编码_us", "成本中心编码（选项 TaktCostCenters/options；空串表示公司级）"),
            // entity.budgetactual.costcentercode
            new TranslationSeedItem("entity.budgetactual.costcentercode", "ja-JP", "成本中心编码_jp", "成本中心编码（选项 TaktCostCenters/options；空串表示公司级）"),
            // entity.budgetactual.costcentercode
            new TranslationSeedItem("entity.budgetactual.costcentercode", "zh-CN", "成本中心编码", "成本中心编码（选项 TaktCostCenters/options；空串表示公司级）"),
            // entity.budgetactual.costcentercode
            new TranslationSeedItem("entity.budgetactual.costcentercode", "zh-HK", "成本中心编码_hk", "成本中心编码（选项 TaktCostCenters/options；空串表示公司级）"),

            // entity.budgetactual.costcentername
            new TranslationSeedItem("entity.budgetactual.costcentername", "en-US", "成本中心名称_us", "成本中心名称（冗余）"),
            // entity.budgetactual.costcentername
            new TranslationSeedItem("entity.budgetactual.costcentername", "ja-JP", "成本中心名称_jp", "成本中心名称（冗余）"),
            // entity.budgetactual.costcentername
            new TranslationSeedItem("entity.budgetactual.costcentername", "zh-CN", "成本中心名称", "成本中心名称（冗余）"),
            // entity.budgetactual.costcentername
            new TranslationSeedItem("entity.budgetactual.costcentername", "zh-HK", "成本中心名称_hk", "成本中心名称（冗余）"),

            // entity.budgetactual.budgetitemcode
            new TranslationSeedItem("entity.budgetactual.budgetitemcode", "en-US", "预算项编码_us", "预算项编码"),
            // entity.budgetactual.budgetitemcode
            new TranslationSeedItem("entity.budgetactual.budgetitemcode", "ja-JP", "预算项编码_jp", "预算项编码"),
            // entity.budgetactual.budgetitemcode
            new TranslationSeedItem("entity.budgetactual.budgetitemcode", "zh-CN", "预算项编码", "预算项编码"),
            // entity.budgetactual.budgetitemcode
            new TranslationSeedItem("entity.budgetactual.budgetitemcode", "zh-HK", "预算项编码_hk", "预算项编码"),

            // entity.budgetactual.budgetitemname
            new TranslationSeedItem("entity.budgetactual.budgetitemname", "en-US", "预算项名称_us", "预算项名称"),
            // entity.budgetactual.budgetitemname
            new TranslationSeedItem("entity.budgetactual.budgetitemname", "ja-JP", "预算项名称_jp", "预算项名称"),
            // entity.budgetactual.budgetitemname
            new TranslationSeedItem("entity.budgetactual.budgetitemname", "zh-CN", "预算项名称", "预算项名称"),
            // entity.budgetactual.budgetitemname
            new TranslationSeedItem("entity.budgetactual.budgetitemname", "zh-HK", "预算项名称_hk", "预算项名称"),

            // entity.budgetactual.accounttitlecode
            new TranslationSeedItem("entity.budgetactual.accounttitlecode", "en-US", "会计科目编码_us", "会计科目编码（可选；选项 TaktAccountTitles/options）"),
            // entity.budgetactual.accounttitlecode
            new TranslationSeedItem("entity.budgetactual.accounttitlecode", "ja-JP", "会计科目编码_jp", "会计科目编码（可选；选项 TaktAccountTitles/options）"),
            // entity.budgetactual.accounttitlecode
            new TranslationSeedItem("entity.budgetactual.accounttitlecode", "zh-CN", "会计科目编码", "会计科目编码（可选；选项 TaktAccountTitles/options）"),
            // entity.budgetactual.accounttitlecode
            new TranslationSeedItem("entity.budgetactual.accounttitlecode", "zh-HK", "会计科目编码_hk", "会计科目编码（可选；选项 TaktAccountTitles/options）"),

            // entity.budgetactual.budgettype
            new TranslationSeedItem("entity.budgetactual.budgettype", "en-US", "预算类型_us", "预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）"),
            // entity.budgetactual.budgettype
            new TranslationSeedItem("entity.budgetactual.budgettype", "ja-JP", "预算类型_jp", "预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）"),
            // entity.budgetactual.budgettype
            new TranslationSeedItem("entity.budgetactual.budgettype", "zh-CN", "预算类型", "预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）"),
            // entity.budgetactual.budgettype
            new TranslationSeedItem("entity.budgetactual.budgettype", "zh-HK", "预算类型_hk", "预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）"),

            // entity.budgetactual.measuretype
            new TranslationSeedItem("entity.budgetactual.measuretype", "en-US", "计量类型_us", "计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）"),
            // entity.budgetactual.measuretype
            new TranslationSeedItem("entity.budgetactual.measuretype", "ja-JP", "计量类型_jp", "计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）"),
            // entity.budgetactual.measuretype
            new TranslationSeedItem("entity.budgetactual.measuretype", "zh-CN", "计量类型", "计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）"),
            // entity.budgetactual.measuretype
            new TranslationSeedItem("entity.budgetactual.measuretype", "zh-HK", "计量类型_hk", "计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）"),

            // entity.budgetactual.budgetamount
            new TranslationSeedItem("entity.budgetactual.budgetamount", "en-US", "本期预算_us", "本期预算金额（或数量，视 MeasureType）"),
            // entity.budgetactual.budgetamount
            new TranslationSeedItem("entity.budgetactual.budgetamount", "ja-JP", "本期预算_jp", "本期预算金额（或数量，视 MeasureType）"),
            // entity.budgetactual.budgetamount
            new TranslationSeedItem("entity.budgetactual.budgetamount", "zh-CN", "本期预算", "本期预算金额（或数量，视 MeasureType）"),
            // entity.budgetactual.budgetamount
            new TranslationSeedItem("entity.budgetactual.budgetamount", "zh-HK", "本期预算_hk", "本期预算金额（或数量，视 MeasureType）"),

            // entity.budgetactual.actualamount
            new TranslationSeedItem("entity.budgetactual.actualamount", "en-US", "本期实绩_us", "本期实绩金额（或数量）"),
            // entity.budgetactual.actualamount
            new TranslationSeedItem("entity.budgetactual.actualamount", "ja-JP", "本期实绩_jp", "本期实绩金额（或数量）"),
            // entity.budgetactual.actualamount
            new TranslationSeedItem("entity.budgetactual.actualamount", "zh-CN", "本期实绩", "本期实绩金额（或数量）"),
            // entity.budgetactual.actualamount
            new TranslationSeedItem("entity.budgetactual.actualamount", "zh-HK", "本期实绩_hk", "本期实绩金额（或数量）"),

            // entity.budgetactual.varianceamount
            new TranslationSeedItem("entity.budgetactual.varianceamount", "en-US", "本期差异_us", "本期差异金额（= 实绩 − 预算）"),
            // entity.budgetactual.varianceamount
            new TranslationSeedItem("entity.budgetactual.varianceamount", "ja-JP", "本期差异_jp", "本期差异金额（= 实绩 − 预算）"),
            // entity.budgetactual.varianceamount
            new TranslationSeedItem("entity.budgetactual.varianceamount", "zh-CN", "本期差异", "本期差异金额（= 实绩 − 预算）"),
            // entity.budgetactual.varianceamount
            new TranslationSeedItem("entity.budgetactual.varianceamount", "zh-HK", "本期差异_hk", "本期差异金额（= 实绩 − 预算）"),

            // entity.budgetactual.variancepercent
            new TranslationSeedItem("entity.budgetactual.variancepercent", "en-US", "本期差异率_us", "本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）"),
            // entity.budgetactual.variancepercent
            new TranslationSeedItem("entity.budgetactual.variancepercent", "ja-JP", "本期差异率_jp", "本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）"),
            // entity.budgetactual.variancepercent
            new TranslationSeedItem("entity.budgetactual.variancepercent", "zh-CN", "本期差异率", "本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）"),
            // entity.budgetactual.variancepercent
            new TranslationSeedItem("entity.budgetactual.variancepercent", "zh-HK", "本期差异率_hk", "本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）"),

            // entity.budgetactual.priorperiodactual
            new TranslationSeedItem("entity.budgetactual.priorperiodactual", "en-US", "上年同期实绩_us", "上年同期实绩（比较分析）"),
            // entity.budgetactual.priorperiodactual
            new TranslationSeedItem("entity.budgetactual.priorperiodactual", "ja-JP", "上年同期实绩_jp", "上年同期实绩（比较分析）"),
            // entity.budgetactual.priorperiodactual
            new TranslationSeedItem("entity.budgetactual.priorperiodactual", "zh-CN", "上年同期实绩", "上年同期实绩（比较分析）"),
            // entity.budgetactual.priorperiodactual
            new TranslationSeedItem("entity.budgetactual.priorperiodactual", "zh-HK", "上年同期实绩_hk", "上年同期实绩（比较分析）"),

            // entity.budgetactual.ytdbudgetamount
            new TranslationSeedItem("entity.budgetactual.ytdbudgetamount", "en-US", "本年累计预算_us", "本年累计预算"),
            // entity.budgetactual.ytdbudgetamount
            new TranslationSeedItem("entity.budgetactual.ytdbudgetamount", "ja-JP", "本年累计预算_jp", "本年累计预算"),
            // entity.budgetactual.ytdbudgetamount
            new TranslationSeedItem("entity.budgetactual.ytdbudgetamount", "zh-CN", "本年累计预算", "本年累计预算"),
            // entity.budgetactual.ytdbudgetamount
            new TranslationSeedItem("entity.budgetactual.ytdbudgetamount", "zh-HK", "本年累计预算_hk", "本年累计预算"),

            // entity.budgetactual.ytdactualamount
            new TranslationSeedItem("entity.budgetactual.ytdactualamount", "en-US", "本年累计实绩_us", "本年累计实绩"),
            // entity.budgetactual.ytdactualamount
            new TranslationSeedItem("entity.budgetactual.ytdactualamount", "ja-JP", "本年累计实绩_jp", "本年累计实绩"),
            // entity.budgetactual.ytdactualamount
            new TranslationSeedItem("entity.budgetactual.ytdactualamount", "zh-CN", "本年累计实绩", "本年累计实绩"),
            // entity.budgetactual.ytdactualamount
            new TranslationSeedItem("entity.budgetactual.ytdactualamount", "zh-HK", "本年累计实绩_hk", "本年累计实绩"),

            // entity.budgetactual.ytdvarianceamount
            new TranslationSeedItem("entity.budgetactual.ytdvarianceamount", "en-US", "本年累计差异_us", "本年累计差异（= 本年累计实绩 − 本年累计预算）"),
            // entity.budgetactual.ytdvarianceamount
            new TranslationSeedItem("entity.budgetactual.ytdvarianceamount", "ja-JP", "本年累计差异_jp", "本年累计差异（= 本年累计实绩 − 本年累计预算）"),
            // entity.budgetactual.ytdvarianceamount
            new TranslationSeedItem("entity.budgetactual.ytdvarianceamount", "zh-CN", "本年累计差异", "本年累计差异（= 本年累计实绩 − 本年累计预算）"),
            // entity.budgetactual.ytdvarianceamount
            new TranslationSeedItem("entity.budgetactual.ytdvarianceamount", "zh-HK", "本年累计差异_hk", "本年累计差异（= 本年累计实绩 − 本年累计预算）"),

            // entity.budgetactual.currencycode
            new TranslationSeedItem("entity.budgetactual.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）"),
            // entity.budgetactual.currencycode
            new TranslationSeedItem("entity.budgetactual.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）"),
            // entity.budgetactual.currencycode
            new TranslationSeedItem("entity.budgetactual.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）"),
            // entity.budgetactual.currencycode
            new TranslationSeedItem("entity.budgetactual.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）"),

            // entity.budgetactual.sortorder
            new TranslationSeedItem("entity.budgetactual.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.budgetactual.sortorder
            new TranslationSeedItem("entity.budgetactual.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.budgetactual.sortorder
            new TranslationSeedItem("entity.budgetactual.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.budgetactual.sortorder
            new TranslationSeedItem("entity.budgetactual.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.budgetactual.status
            new TranslationSeedItem("entity.budgetactual.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.budgetactual.status
            new TranslationSeedItem("entity.budgetactual.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.budgetactual.status
            new TranslationSeedItem("entity.budgetactual.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.budgetactual.status
            new TranslationSeedItem("entity.budgetactual.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
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
