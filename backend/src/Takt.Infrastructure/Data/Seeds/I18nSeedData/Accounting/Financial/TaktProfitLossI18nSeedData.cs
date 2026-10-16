// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktProfitLossI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProfitLoss 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktProfitLoss 实体国际化翻译种子（键前缀 entity.profitloss.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProfitLossI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProfitLoss 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 profitloss 实体翻译...", tenantCode);

        foreach (var item in GetProfitLossTranslations())
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

        TaktLogger.Information("TaktProfitLoss 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProfitLoss 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.profitloss._self / entity.profitloss.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProfitLossTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.profitloss._self
            new TranslationSeedItem("entity.profitloss._self", "en-US", "Profit Loss Information_us", "实体名称"),
            // entity.profitloss._self
            new TranslationSeedItem("entity.profitloss._self", "ja-JP", "利润表信息_jp", "实体名称"),
            // entity.profitloss._self
            new TranslationSeedItem("entity.profitloss._self", "zh-CN", "利润表信息", "实体名称"),
            // entity.profitloss._self
            new TranslationSeedItem("entity.profitloss._self", "zh-HK", "利润表信息_hk", "实体名称"),

            // entity.profitloss.periodcode
            new TranslationSeedItem("entity.profitloss.periodcode", "en-US", "会计期间_us", "会计期间编码（YYYYMM；利润表报告期）"),
            // entity.profitloss.periodcode
            new TranslationSeedItem("entity.profitloss.periodcode", "ja-JP", "会计期间_jp", "会计期间编码（YYYYMM；利润表报告期）"),
            // entity.profitloss.periodcode
            new TranslationSeedItem("entity.profitloss.periodcode", "zh-CN", "会计期间", "会计期间编码（YYYYMM；利润表报告期）"),
            // entity.profitloss.periodcode
            new TranslationSeedItem("entity.profitloss.periodcode", "zh-HK", "会计期间_hk", "会计期间编码（YYYYMM；利润表报告期）"),

            // entity.profitloss.statementlinecode
            new TranslationSeedItem("entity.profitloss.statementlinecode", "en-US", "报表项目编码_us", "报表项目编码（利润表/综合收益表行项目）"),
            // entity.profitloss.statementlinecode
            new TranslationSeedItem("entity.profitloss.statementlinecode", "ja-JP", "报表项目编码_jp", "报表项目编码（利润表/综合收益表行项目）"),
            // entity.profitloss.statementlinecode
            new TranslationSeedItem("entity.profitloss.statementlinecode", "zh-CN", "报表项目编码", "报表项目编码（利润表/综合收益表行项目）"),
            // entity.profitloss.statementlinecode
            new TranslationSeedItem("entity.profitloss.statementlinecode", "zh-HK", "报表项目编码_hk", "报表项目编码（利润表/综合收益表行项目）"),

            // entity.profitloss.statementlinename
            new TranslationSeedItem("entity.profitloss.statementlinename", "en-US", "报表项目名称_us", "报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）"),
            // entity.profitloss.statementlinename
            new TranslationSeedItem("entity.profitloss.statementlinename", "ja-JP", "报表项目名称_jp", "报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）"),
            // entity.profitloss.statementlinename
            new TranslationSeedItem("entity.profitloss.statementlinename", "zh-CN", "报表项目名称", "报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）"),
            // entity.profitloss.statementlinename
            new TranslationSeedItem("entity.profitloss.statementlinename", "zh-HK", "报表项目名称_hk", "报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）"),

            // entity.profitloss.accounttitlecode
            new TranslationSeedItem("entity.profitloss.accounttitlecode", "en-US", "会计科目编码_us", "会计科目编码（可选；选项 TaktAccountTitles/options）"),
            // entity.profitloss.accounttitlecode
            new TranslationSeedItem("entity.profitloss.accounttitlecode", "ja-JP", "会计科目编码_jp", "会计科目编码（可选；选项 TaktAccountTitles/options）"),
            // entity.profitloss.accounttitlecode
            new TranslationSeedItem("entity.profitloss.accounttitlecode", "zh-CN", "会计科目编码", "会计科目编码（可选；选项 TaktAccountTitles/options）"),
            // entity.profitloss.accounttitlecode
            new TranslationSeedItem("entity.profitloss.accounttitlecode", "zh-HK", "会计科目编码_hk", "会计科目编码（可选；选项 TaktAccountTitles/options）"),

            // entity.profitloss.accounttitlename
            new TranslationSeedItem("entity.profitloss.accounttitlename", "en-US", "会计科目名称_us", "会计科目名称（冗余）"),
            // entity.profitloss.accounttitlename
            new TranslationSeedItem("entity.profitloss.accounttitlename", "ja-JP", "会计科目名称_jp", "会计科目名称（冗余）"),
            // entity.profitloss.accounttitlename
            new TranslationSeedItem("entity.profitloss.accounttitlename", "zh-CN", "会计科目名称", "会计科目名称（冗余）"),
            // entity.profitloss.accounttitlename
            new TranslationSeedItem("entity.profitloss.accounttitlename", "zh-HK", "会计科目名称_hk", "会计科目名称（冗余）"),

            // entity.profitloss.linecategory
            new TranslationSeedItem("entity.profitloss.linecategory", "en-US", "行类别_us", "行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）"),
            // entity.profitloss.linecategory
            new TranslationSeedItem("entity.profitloss.linecategory", "ja-JP", "行类别_jp", "行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）"),
            // entity.profitloss.linecategory
            new TranslationSeedItem("entity.profitloss.linecategory", "zh-CN", "行类别", "行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）"),
            // entity.profitloss.linecategory
            new TranslationSeedItem("entity.profitloss.linecategory", "zh-HK", "行类别_hk", "行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）"),

            // entity.profitloss.istotalline
            new TranslationSeedItem("entity.profitloss.istotalline", "en-US", "是否合计行_us", "是否合计/小计行（字典 sys_yes_no；1=是，0=否）"),
            // entity.profitloss.istotalline
            new TranslationSeedItem("entity.profitloss.istotalline", "ja-JP", "是否合计行_jp", "是否合计/小计行（字典 sys_yes_no；1=是，0=否）"),
            // entity.profitloss.istotalline
            new TranslationSeedItem("entity.profitloss.istotalline", "zh-CN", "是否合计行", "是否合计/小计行（字典 sys_yes_no；1=是，0=否）"),
            // entity.profitloss.istotalline
            new TranslationSeedItem("entity.profitloss.istotalline", "zh-HK", "是否合计行_hk", "是否合计/小计行（字典 sys_yes_no；1=是，0=否）"),

            // entity.profitloss.periodamount
            new TranslationSeedItem("entity.profitloss.periodamount", "en-US", "本期金额_us", "本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）"),
            // entity.profitloss.periodamount
            new TranslationSeedItem("entity.profitloss.periodamount", "ja-JP", "本期金额_jp", "本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）"),
            // entity.profitloss.periodamount
            new TranslationSeedItem("entity.profitloss.periodamount", "zh-CN", "本期金额", "本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）"),
            // entity.profitloss.periodamount
            new TranslationSeedItem("entity.profitloss.periodamount", "zh-HK", "本期金额_hk", "本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）"),

            // entity.profitloss.priorperiodamount
            new TranslationSeedItem("entity.profitloss.priorperiodamount", "en-US", "上期金额_us", "上期金额（比较信息；CAS/IAS 1）"),
            // entity.profitloss.priorperiodamount
            new TranslationSeedItem("entity.profitloss.priorperiodamount", "ja-JP", "上期金额_jp", "上期金额（比较信息；CAS/IAS 1）"),
            // entity.profitloss.priorperiodamount
            new TranslationSeedItem("entity.profitloss.priorperiodamount", "zh-CN", "上期金额", "上期金额（比较信息；CAS/IAS 1）"),
            // entity.profitloss.priorperiodamount
            new TranslationSeedItem("entity.profitloss.priorperiodamount", "zh-HK", "上期金额_hk", "上期金额（比较信息；CAS/IAS 1）"),

            // entity.profitloss.yeartodateamount
            new TranslationSeedItem("entity.profitloss.yeartodateamount", "en-US", "本年累计金额_us", "本年累计金额（中国利润表常见列；自财年期初至本期末）"),
            // entity.profitloss.yeartodateamount
            new TranslationSeedItem("entity.profitloss.yeartodateamount", "ja-JP", "本年累计金额_jp", "本年累计金额（中国利润表常见列；自财年期初至本期末）"),
            // entity.profitloss.yeartodateamount
            new TranslationSeedItem("entity.profitloss.yeartodateamount", "zh-CN", "本年累计金额", "本年累计金额（中国利润表常见列；自财年期初至本期末）"),
            // entity.profitloss.yeartodateamount
            new TranslationSeedItem("entity.profitloss.yeartodateamount", "zh-HK", "本年累计金额_hk", "本年累计金额（中国利润表常见列；自财年期初至本期末）"),

            // entity.profitloss.isexpense
            new TranslationSeedItem("entity.profitloss.isexpense", "en-US", "是否费用性质_us", "是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）"),
            // entity.profitloss.isexpense
            new TranslationSeedItem("entity.profitloss.isexpense", "ja-JP", "是否费用性质_jp", "是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）"),
            // entity.profitloss.isexpense
            new TranslationSeedItem("entity.profitloss.isexpense", "zh-CN", "是否费用性质", "是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）"),
            // entity.profitloss.isexpense
            new TranslationSeedItem("entity.profitloss.isexpense", "zh-HK", "是否费用性质_hk", "是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）"),

            // entity.profitloss.currencycode
            new TranslationSeedItem("entity.profitloss.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code）"),
            // entity.profitloss.currencycode
            new TranslationSeedItem("entity.profitloss.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code）"),
            // entity.profitloss.currencycode
            new TranslationSeedItem("entity.profitloss.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code）"),
            // entity.profitloss.currencycode
            new TranslationSeedItem("entity.profitloss.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code）"),

            // entity.profitloss.sortorder
            new TranslationSeedItem("entity.profitloss.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.profitloss.sortorder
            new TranslationSeedItem("entity.profitloss.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.profitloss.sortorder
            new TranslationSeedItem("entity.profitloss.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.profitloss.sortorder
            new TranslationSeedItem("entity.profitloss.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.profitloss.status
            new TranslationSeedItem("entity.profitloss.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.profitloss.status
            new TranslationSeedItem("entity.profitloss.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.profitloss.status
            new TranslationSeedItem("entity.profitloss.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.profitloss.status
            new TranslationSeedItem("entity.profitloss.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
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
