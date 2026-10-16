// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktBalanceSheetI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBalanceSheet 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBalanceSheet 实体国际化翻译种子（键前缀 entity.balancesheet.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBalanceSheetI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBalanceSheet 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 balancesheet 实体翻译...", tenantCode);

        foreach (var item in GetBalanceSheetTranslations())
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

        TaktLogger.Information("TaktBalanceSheet 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBalanceSheet 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.balancesheet._self / entity.balancesheet.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBalanceSheetTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.balancesheet._self
            new TranslationSeedItem("entity.balancesheet._self", "en-US", "Balance Sheet Information_us", "实体名称"),
            // entity.balancesheet._self
            new TranslationSeedItem("entity.balancesheet._self", "ja-JP", "资产负债表行信息_jp", "实体名称"),
            // entity.balancesheet._self
            new TranslationSeedItem("entity.balancesheet._self", "zh-CN", "资产负债表行信息", "实体名称"),
            // entity.balancesheet._self
            new TranslationSeedItem("entity.balancesheet._self", "zh-HK", "资产负债表行信息_hk", "实体名称"),

            // entity.balancesheet.periodcode
            new TranslationSeedItem("entity.balancesheet.periodcode", "en-US", "会计期间_us", "会计期间编码（YYYYMM；资产负债表日所属报告期）"),
            // entity.balancesheet.periodcode
            new TranslationSeedItem("entity.balancesheet.periodcode", "ja-JP", "会计期间_jp", "会计期间编码（YYYYMM；资产负债表日所属报告期）"),
            // entity.balancesheet.periodcode
            new TranslationSeedItem("entity.balancesheet.periodcode", "zh-CN", "会计期间", "会计期间编码（YYYYMM；资产负债表日所属报告期）"),
            // entity.balancesheet.periodcode
            new TranslationSeedItem("entity.balancesheet.periodcode", "zh-HK", "会计期间_hk", "会计期间编码（YYYYMM；资产负债表日所属报告期）"),

            // entity.balancesheet.statementlinecode
            new TranslationSeedItem("entity.balancesheet.statementlinecode", "en-US", "报表项目编码_us", "报表项目编码（资产负债表行项目；可与总账科目多对一映射）"),
            // entity.balancesheet.statementlinecode
            new TranslationSeedItem("entity.balancesheet.statementlinecode", "ja-JP", "报表项目编码_jp", "报表项目编码（资产负债表行项目；可与总账科目多对一映射）"),
            // entity.balancesheet.statementlinecode
            new TranslationSeedItem("entity.balancesheet.statementlinecode", "zh-CN", "报表项目编码", "报表项目编码（资产负债表行项目；可与总账科目多对一映射）"),
            // entity.balancesheet.statementlinecode
            new TranslationSeedItem("entity.balancesheet.statementlinecode", "zh-HK", "报表项目编码_hk", "报表项目编码（资产负债表行项目；可与总账科目多对一映射）"),

            // entity.balancesheet.statementlinename
            new TranslationSeedItem("entity.balancesheet.statementlinename", "en-US", "报表项目名称_us", "报表项目名称（如「货币资金」「应付账款」「未分配利润」）"),
            // entity.balancesheet.statementlinename
            new TranslationSeedItem("entity.balancesheet.statementlinename", "ja-JP", "报表项目名称_jp", "报表项目名称（如「货币资金」「应付账款」「未分配利润」）"),
            // entity.balancesheet.statementlinename
            new TranslationSeedItem("entity.balancesheet.statementlinename", "zh-CN", "报表项目名称", "报表项目名称（如「货币资金」「应付账款」「未分配利润」）"),
            // entity.balancesheet.statementlinename
            new TranslationSeedItem("entity.balancesheet.statementlinename", "zh-HK", "报表项目名称_hk", "报表项目名称（如「货币资金」「应付账款」「未分配利润」）"),

            // entity.balancesheet.accounttitlecode
            new TranslationSeedItem("entity.balancesheet.accounttitlecode", "en-US", "会计科目编码_us", "会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）"),
            // entity.balancesheet.accounttitlecode
            new TranslationSeedItem("entity.balancesheet.accounttitlecode", "ja-JP", "会计科目编码_jp", "会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）"),
            // entity.balancesheet.accounttitlecode
            new TranslationSeedItem("entity.balancesheet.accounttitlecode", "zh-CN", "会计科目编码", "会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）"),
            // entity.balancesheet.accounttitlecode
            new TranslationSeedItem("entity.balancesheet.accounttitlecode", "zh-HK", "会计科目编码_hk", "会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）"),

            // entity.balancesheet.accounttitlename
            new TranslationSeedItem("entity.balancesheet.accounttitlename", "en-US", "会计科目名称_us", "会计科目名称（冗余）"),
            // entity.balancesheet.accounttitlename
            new TranslationSeedItem("entity.balancesheet.accounttitlename", "ja-JP", "会计科目名称_jp", "会计科目名称（冗余）"),
            // entity.balancesheet.accounttitlename
            new TranslationSeedItem("entity.balancesheet.accounttitlename", "zh-CN", "会计科目名称", "会计科目名称（冗余）"),
            // entity.balancesheet.accounttitlename
            new TranslationSeedItem("entity.balancesheet.accounttitlename", "zh-HK", "会计科目名称_hk", "会计科目名称（冗余）"),

            // entity.balancesheet.linecategory
            new TranslationSeedItem("entity.balancesheet.linecategory", "en-US", "行类别_us", "行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）"),
            // entity.balancesheet.linecategory
            new TranslationSeedItem("entity.balancesheet.linecategory", "ja-JP", "行类别_jp", "行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）"),
            // entity.balancesheet.linecategory
            new TranslationSeedItem("entity.balancesheet.linecategory", "zh-CN", "行类别", "行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）"),
            // entity.balancesheet.linecategory
            new TranslationSeedItem("entity.balancesheet.linecategory", "zh-HK", "行类别_hk", "行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）"),

            // entity.balancesheet.balancedirection
            new TranslationSeedItem("entity.balancesheet.balancedirection", "en-US", "余额方向_us", "余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）"),
            // entity.balancesheet.balancedirection
            new TranslationSeedItem("entity.balancesheet.balancedirection", "ja-JP", "余额方向_jp", "余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）"),
            // entity.balancesheet.balancedirection
            new TranslationSeedItem("entity.balancesheet.balancedirection", "zh-CN", "余额方向", "余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）"),
            // entity.balancesheet.balancedirection
            new TranslationSeedItem("entity.balancesheet.balancedirection", "zh-HK", "余额方向_hk", "余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）"),

            // entity.balancesheet.istotalline
            new TranslationSeedItem("entity.balancesheet.istotalline", "en-US", "是否合计行_us", "是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）"),
            // entity.balancesheet.istotalline
            new TranslationSeedItem("entity.balancesheet.istotalline", "ja-JP", "是否合计行_jp", "是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）"),
            // entity.balancesheet.istotalline
            new TranslationSeedItem("entity.balancesheet.istotalline", "zh-CN", "是否合计行", "是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）"),
            // entity.balancesheet.istotalline
            new TranslationSeedItem("entity.balancesheet.istotalline", "zh-HK", "是否合计行_hk", "是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）"),

            // entity.balancesheet.openingbalance
            new TranslationSeedItem("entity.balancesheet.openingbalance", "en-US", "期初余额_us", "期初余额（总账口径）"),
            // entity.balancesheet.openingbalance
            new TranslationSeedItem("entity.balancesheet.openingbalance", "ja-JP", "期初余额_jp", "期初余额（总账口径）"),
            // entity.balancesheet.openingbalance
            new TranslationSeedItem("entity.balancesheet.openingbalance", "zh-CN", "期初余额", "期初余额（总账口径）"),
            // entity.balancesheet.openingbalance
            new TranslationSeedItem("entity.balancesheet.openingbalance", "zh-HK", "期初余额_hk", "期初余额（总账口径）"),

            // entity.balancesheet.debitamount
            new TranslationSeedItem("entity.balancesheet.debitamount", "en-US", "借方发生额_us", "本期借方发生额"),
            // entity.balancesheet.debitamount
            new TranslationSeedItem("entity.balancesheet.debitamount", "ja-JP", "借方发生额_jp", "本期借方发生额"),
            // entity.balancesheet.debitamount
            new TranslationSeedItem("entity.balancesheet.debitamount", "zh-CN", "借方发生额", "本期借方发生额"),
            // entity.balancesheet.debitamount
            new TranslationSeedItem("entity.balancesheet.debitamount", "zh-HK", "借方发生额_hk", "本期借方发生额"),

            // entity.balancesheet.creditamount
            new TranslationSeedItem("entity.balancesheet.creditamount", "en-US", "贷方发生额_us", "本期贷方发生额"),
            // entity.balancesheet.creditamount
            new TranslationSeedItem("entity.balancesheet.creditamount", "ja-JP", "贷方发生额_jp", "本期贷方发生额"),
            // entity.balancesheet.creditamount
            new TranslationSeedItem("entity.balancesheet.creditamount", "zh-CN", "贷方发生额", "本期贷方发生额"),
            // entity.balancesheet.creditamount
            new TranslationSeedItem("entity.balancesheet.creditamount", "zh-HK", "贷方发生额_hk", "本期贷方发生额"),

            // entity.balancesheet.closingbalance
            new TranslationSeedItem("entity.balancesheet.closingbalance", "en-US", "期末余额_us", "期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）"),
            // entity.balancesheet.closingbalance
            new TranslationSeedItem("entity.balancesheet.closingbalance", "ja-JP", "期末余额_jp", "期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）"),
            // entity.balancesheet.closingbalance
            new TranslationSeedItem("entity.balancesheet.closingbalance", "zh-CN", "期末余额", "期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）"),
            // entity.balancesheet.closingbalance
            new TranslationSeedItem("entity.balancesheet.closingbalance", "zh-HK", "期末余额_hk", "期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）"),

            // entity.balancesheet.presentationamount
            new TranslationSeedItem("entity.balancesheet.presentationamount", "en-US", "期末列报金额_us", "期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）"),
            // entity.balancesheet.presentationamount
            new TranslationSeedItem("entity.balancesheet.presentationamount", "ja-JP", "期末列报金额_jp", "期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）"),
            // entity.balancesheet.presentationamount
            new TranslationSeedItem("entity.balancesheet.presentationamount", "zh-CN", "期末列报金额", "期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）"),
            // entity.balancesheet.presentationamount
            new TranslationSeedItem("entity.balancesheet.presentationamount", "zh-HK", "期末列报金额_hk", "期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）"),

            // entity.balancesheet.priorperiodamount
            new TranslationSeedItem("entity.balancesheet.priorperiodamount", "en-US", "上期列报金额_us", "上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）"),
            // entity.balancesheet.priorperiodamount
            new TranslationSeedItem("entity.balancesheet.priorperiodamount", "ja-JP", "上期列报金额_jp", "上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）"),
            // entity.balancesheet.priorperiodamount
            new TranslationSeedItem("entity.balancesheet.priorperiodamount", "zh-CN", "上期列报金额", "上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）"),
            // entity.balancesheet.priorperiodamount
            new TranslationSeedItem("entity.balancesheet.priorperiodamount", "zh-HK", "上期列报金额_hk", "上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）"),

            // entity.balancesheet.currencycode
            new TranslationSeedItem("entity.balancesheet.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；报告货币）"),
            // entity.balancesheet.currencycode
            new TranslationSeedItem("entity.balancesheet.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；报告货币）"),
            // entity.balancesheet.currencycode
            new TranslationSeedItem("entity.balancesheet.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；报告货币）"),
            // entity.balancesheet.currencycode
            new TranslationSeedItem("entity.balancesheet.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；报告货币）"),

            // entity.balancesheet.sortorder
            new TranslationSeedItem("entity.balancesheet.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前；应与报表印刷顺序一致）"),
            // entity.balancesheet.sortorder
            new TranslationSeedItem("entity.balancesheet.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前；应与报表印刷顺序一致）"),
            // entity.balancesheet.sortorder
            new TranslationSeedItem("entity.balancesheet.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前；应与报表印刷顺序一致）"),
            // entity.balancesheet.sortorder
            new TranslationSeedItem("entity.balancesheet.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前；应与报表印刷顺序一致）"),

            // entity.balancesheet.status
            new TranslationSeedItem("entity.balancesheet.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.balancesheet.status
            new TranslationSeedItem("entity.balancesheet.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.balancesheet.status
            new TranslationSeedItem("entity.balancesheet.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.balancesheet.status
            new TranslationSeedItem("entity.balancesheet.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
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
