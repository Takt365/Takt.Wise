// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktPurchaseSalesInventoryI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseSalesInventory 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseSalesInventory 实体国际化翻译种子（键前缀 entity.purchasesalesinventory.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseSalesInventoryI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseSalesInventory 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchasesalesinventory 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseSalesInventoryTranslations())
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

        TaktLogger.Information("TaktPurchaseSalesInventory 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseSalesInventory 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchasesalesinventory._self / entity.purchasesalesinventory.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseSalesInventoryTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchasesalesinventory._self
            new TranslationSeedItem("entity.purchasesalesinventory._self", "en-US", "Purchase Sales Inventory Information_us", "实体名称"),
            // entity.purchasesalesinventory._self
            new TranslationSeedItem("entity.purchasesalesinventory._self", "ja-JP", "进销存表信息_jp", "实体名称"),
            // entity.purchasesalesinventory._self
            new TranslationSeedItem("entity.purchasesalesinventory._self", "zh-CN", "进销存表信息", "实体名称"),
            // entity.purchasesalesinventory._self
            new TranslationSeedItem("entity.purchasesalesinventory._self", "zh-HK", "进销存表信息_hk", "实体名称"),

            // entity.purchasesalesinventory.periodcode
            new TranslationSeedItem("entity.purchasesalesinventory.periodcode", "en-US", "会计期间_us", "会计期间编码（YYYYMM）"),
            // entity.purchasesalesinventory.periodcode
            new TranslationSeedItem("entity.purchasesalesinventory.periodcode", "ja-JP", "会计期间_jp", "会计期间编码（YYYYMM）"),
            // entity.purchasesalesinventory.periodcode
            new TranslationSeedItem("entity.purchasesalesinventory.periodcode", "zh-CN", "会计期间", "会计期间编码（YYYYMM）"),
            // entity.purchasesalesinventory.periodcode
            new TranslationSeedItem("entity.purchasesalesinventory.periodcode", "zh-HK", "会计期间_hk", "会计期间编码（YYYYMM）"),

            // entity.purchasesalesinventory.materialcode
            new TranslationSeedItem("entity.purchasesalesinventory.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktGeneralMaterials/options 或 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.purchasesalesinventory.materialcode
            new TranslationSeedItem("entity.purchasesalesinventory.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktGeneralMaterials/options 或 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.purchasesalesinventory.materialcode
            new TranslationSeedItem("entity.purchasesalesinventory.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktGeneralMaterials/options 或 TaktMaterialPlants/options；DictValue=MaterialCode）"),
            // entity.purchasesalesinventory.materialcode
            new TranslationSeedItem("entity.purchasesalesinventory.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktGeneralMaterials/options 或 TaktMaterialPlants/options；DictValue=MaterialCode）"),

            // entity.purchasesalesinventory.materialdescription
            new TranslationSeedItem("entity.purchasesalesinventory.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchasesalesinventory.materialdescription
            new TranslationSeedItem("entity.purchasesalesinventory.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchasesalesinventory.materialdescription
            new TranslationSeedItem("entity.purchasesalesinventory.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchasesalesinventory.materialdescription
            new TranslationSeedItem("entity.purchasesalesinventory.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.purchasesalesinventory.valuation
            new TranslationSeedItem("entity.purchasesalesinventory.valuation", "en-US", "评估类别_us", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.purchasesalesinventory.valuation
            new TranslationSeedItem("entity.purchasesalesinventory.valuation", "ja-JP", "评估类别_jp", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.purchasesalesinventory.valuation
            new TranslationSeedItem("entity.purchasesalesinventory.valuation", "zh-CN", "评估类别", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.purchasesalesinventory.valuation
            new TranslationSeedItem("entity.purchasesalesinventory.valuation", "zh-HK", "评估类别_hk", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),

            // entity.purchasesalesinventory.unitcode
            new TranslationSeedItem("entity.purchasesalesinventory.unitcode", "en-US", "计量单位_us", "计量单位"),
            // entity.purchasesalesinventory.unitcode
            new TranslationSeedItem("entity.purchasesalesinventory.unitcode", "ja-JP", "计量单位_jp", "计量单位"),
            // entity.purchasesalesinventory.unitcode
            new TranslationSeedItem("entity.purchasesalesinventory.unitcode", "zh-CN", "计量单位", "计量单位"),
            // entity.purchasesalesinventory.unitcode
            new TranslationSeedItem("entity.purchasesalesinventory.unitcode", "zh-HK", "计量单位_hk", "计量单位"),

            // entity.purchasesalesinventory.openingqty
            new TranslationSeedItem("entity.purchasesalesinventory.openingqty", "en-US", "期初数量_us", "期初数量"),
            // entity.purchasesalesinventory.openingqty
            new TranslationSeedItem("entity.purchasesalesinventory.openingqty", "ja-JP", "期初数量_jp", "期初数量"),
            // entity.purchasesalesinventory.openingqty
            new TranslationSeedItem("entity.purchasesalesinventory.openingqty", "zh-CN", "期初数量", "期初数量"),
            // entity.purchasesalesinventory.openingqty
            new TranslationSeedItem("entity.purchasesalesinventory.openingqty", "zh-HK", "期初数量_hk", "期初数量"),

            // entity.purchasesalesinventory.openingamount
            new TranslationSeedItem("entity.purchasesalesinventory.openingamount", "en-US", "期初成本金额_us", "期初存货成本金额（IAS 2 / CAS 成本口径）"),
            // entity.purchasesalesinventory.openingamount
            new TranslationSeedItem("entity.purchasesalesinventory.openingamount", "ja-JP", "期初成本金额_jp", "期初存货成本金额（IAS 2 / CAS 成本口径）"),
            // entity.purchasesalesinventory.openingamount
            new TranslationSeedItem("entity.purchasesalesinventory.openingamount", "zh-CN", "期初成本金额", "期初存货成本金额（IAS 2 / CAS 成本口径）"),
            // entity.purchasesalesinventory.openingamount
            new TranslationSeedItem("entity.purchasesalesinventory.openingamount", "zh-HK", "期初成本金额_hk", "期初存货成本金额（IAS 2 / CAS 成本口径）"),

            // entity.purchasesalesinventory.purchaseqty
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseqty", "en-US", "采购入库数量_us", "本期采购入库数量"),
            // entity.purchasesalesinventory.purchaseqty
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseqty", "ja-JP", "采购入库数量_jp", "本期采购入库数量"),
            // entity.purchasesalesinventory.purchaseqty
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseqty", "zh-CN", "采购入库数量", "本期采购入库数量"),
            // entity.purchasesalesinventory.purchaseqty
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseqty", "zh-HK", "采购入库数量_hk", "本期采购入库数量"),

            // entity.purchasesalesinventory.purchaseamount
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseamount", "en-US", "采购入库成本_us", "本期采购入库成本金额"),
            // entity.purchasesalesinventory.purchaseamount
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseamount", "ja-JP", "采购入库成本_jp", "本期采购入库成本金额"),
            // entity.purchasesalesinventory.purchaseamount
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseamount", "zh-CN", "采购入库成本", "本期采购入库成本金额"),
            // entity.purchasesalesinventory.purchaseamount
            new TranslationSeedItem("entity.purchasesalesinventory.purchaseamount", "zh-HK", "采购入库成本_hk", "本期采购入库成本金额"),

            // entity.purchasesalesinventory.productionqty
            new TranslationSeedItem("entity.purchasesalesinventory.productionqty", "en-US", "生产入库数量_us", "本期生产入库数量（自制半成品/产成品）"),
            // entity.purchasesalesinventory.productionqty
            new TranslationSeedItem("entity.purchasesalesinventory.productionqty", "ja-JP", "生产入库数量_jp", "本期生产入库数量（自制半成品/产成品）"),
            // entity.purchasesalesinventory.productionqty
            new TranslationSeedItem("entity.purchasesalesinventory.productionqty", "zh-CN", "生产入库数量", "本期生产入库数量（自制半成品/产成品）"),
            // entity.purchasesalesinventory.productionqty
            new TranslationSeedItem("entity.purchasesalesinventory.productionqty", "zh-HK", "生产入库数量_hk", "本期生产入库数量（自制半成品/产成品）"),

            // entity.purchasesalesinventory.productionamount
            new TranslationSeedItem("entity.purchasesalesinventory.productionamount", "en-US", "生产入库成本_us", "本期生产入库成本金额"),
            // entity.purchasesalesinventory.productionamount
            new TranslationSeedItem("entity.purchasesalesinventory.productionamount", "ja-JP", "生产入库成本_jp", "本期生产入库成本金额"),
            // entity.purchasesalesinventory.productionamount
            new TranslationSeedItem("entity.purchasesalesinventory.productionamount", "zh-CN", "生产入库成本", "本期生产入库成本金额"),
            // entity.purchasesalesinventory.productionamount
            new TranslationSeedItem("entity.purchasesalesinventory.productionamount", "zh-HK", "生产入库成本_hk", "本期生产入库成本金额"),

            // entity.purchasesalesinventory.issueqty
            new TranslationSeedItem("entity.purchasesalesinventory.issueqty", "en-US", "出库数量_us", "本期出库数量（销售/领用等发出）"),
            // entity.purchasesalesinventory.issueqty
            new TranslationSeedItem("entity.purchasesalesinventory.issueqty", "ja-JP", "出库数量_jp", "本期出库数量（销售/领用等发出）"),
            // entity.purchasesalesinventory.issueqty
            new TranslationSeedItem("entity.purchasesalesinventory.issueqty", "zh-CN", "出库数量", "本期出库数量（销售/领用等发出）"),
            // entity.purchasesalesinventory.issueqty
            new TranslationSeedItem("entity.purchasesalesinventory.issueqty", "zh-HK", "出库数量_hk", "本期出库数量（销售/领用等发出）"),

            // entity.purchasesalesinventory.issuecostamount
            new TranslationSeedItem("entity.purchasesalesinventory.issuecostamount", "en-US", "出库结转成本_us", "本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）"),
            // entity.purchasesalesinventory.issuecostamount
            new TranslationSeedItem("entity.purchasesalesinventory.issuecostamount", "ja-JP", "出库结转成本_jp", "本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）"),
            // entity.purchasesalesinventory.issuecostamount
            new TranslationSeedItem("entity.purchasesalesinventory.issuecostamount", "zh-CN", "出库结转成本", "本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）"),
            // entity.purchasesalesinventory.issuecostamount
            new TranslationSeedItem("entity.purchasesalesinventory.issuecostamount", "zh-HK", "出库结转成本_hk", "本期出库结转成本（营业成本/领用成本；计入存货成本等式减项）"),

            // entity.purchasesalesinventory.salesrevenueamount
            new TranslationSeedItem("entity.purchasesalesinventory.salesrevenueamount", "en-US", "销售收入_us", "本期销售收入金额（利润表口径；不参与存货成本等式）"),
            // entity.purchasesalesinventory.salesrevenueamount
            new TranslationSeedItem("entity.purchasesalesinventory.salesrevenueamount", "ja-JP", "销售收入_jp", "本期销售收入金额（利润表口径；不参与存货成本等式）"),
            // entity.purchasesalesinventory.salesrevenueamount
            new TranslationSeedItem("entity.purchasesalesinventory.salesrevenueamount", "zh-CN", "销售收入", "本期销售收入金额（利润表口径；不参与存货成本等式）"),
            // entity.purchasesalesinventory.salesrevenueamount
            new TranslationSeedItem("entity.purchasesalesinventory.salesrevenueamount", "zh-HK", "销售收入_hk", "本期销售收入金额（利润表口径；不参与存货成本等式）"),

            // entity.purchasesalesinventory.adjustqty
            new TranslationSeedItem("entity.purchasesalesinventory.adjustqty", "en-US", "调整数量_us", "本期调整数量（盘盈盘亏、报废等，可正可负）"),
            // entity.purchasesalesinventory.adjustqty
            new TranslationSeedItem("entity.purchasesalesinventory.adjustqty", "ja-JP", "调整数量_jp", "本期调整数量（盘盈盘亏、报废等，可正可负）"),
            // entity.purchasesalesinventory.adjustqty
            new TranslationSeedItem("entity.purchasesalesinventory.adjustqty", "zh-CN", "调整数量", "本期调整数量（盘盈盘亏、报废等，可正可负）"),
            // entity.purchasesalesinventory.adjustqty
            new TranslationSeedItem("entity.purchasesalesinventory.adjustqty", "zh-HK", "调整数量_hk", "本期调整数量（盘盈盘亏、报废等，可正可负）"),

            // entity.purchasesalesinventory.adjustamount
            new TranslationSeedItem("entity.purchasesalesinventory.adjustamount", "en-US", "调整成本金额_us", "本期调整成本金额"),
            // entity.purchasesalesinventory.adjustamount
            new TranslationSeedItem("entity.purchasesalesinventory.adjustamount", "ja-JP", "调整成本金额_jp", "本期调整成本金额"),
            // entity.purchasesalesinventory.adjustamount
            new TranslationSeedItem("entity.purchasesalesinventory.adjustamount", "zh-CN", "调整成本金额", "本期调整成本金额"),
            // entity.purchasesalesinventory.adjustamount
            new TranslationSeedItem("entity.purchasesalesinventory.adjustamount", "zh-HK", "调整成本金额_hk", "本期调整成本金额"),

            // entity.purchasesalesinventory.closingqty
            new TranslationSeedItem("entity.purchasesalesinventory.closingqty", "en-US", "期末数量_us", "期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）"),
            // entity.purchasesalesinventory.closingqty
            new TranslationSeedItem("entity.purchasesalesinventory.closingqty", "ja-JP", "期末数量_jp", "期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）"),
            // entity.purchasesalesinventory.closingqty
            new TranslationSeedItem("entity.purchasesalesinventory.closingqty", "zh-CN", "期末数量", "期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）"),
            // entity.purchasesalesinventory.closingqty
            new TranslationSeedItem("entity.purchasesalesinventory.closingqty", "zh-HK", "期末数量_hk", "期末数量（= 期初 + 采购 + 生产 + 调整 − 出库）"),

            // entity.purchasesalesinventory.closingamount
            new TranslationSeedItem("entity.purchasesalesinventory.closingamount", "en-US", "期末成本金额_us", "期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）"),
            // entity.purchasesalesinventory.closingamount
            new TranslationSeedItem("entity.purchasesalesinventory.closingamount", "ja-JP", "期末成本金额_jp", "期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）"),
            // entity.purchasesalesinventory.closingamount
            new TranslationSeedItem("entity.purchasesalesinventory.closingamount", "zh-CN", "期末成本金额", "期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）"),
            // entity.purchasesalesinventory.closingamount
            new TranslationSeedItem("entity.purchasesalesinventory.closingamount", "zh-HK", "期末成本金额_hk", "期末存货成本金额（= 期初 + 采购 + 生产 + 调整 − 出库结转成本）"),

            // entity.purchasesalesinventory.closingunitcost
            new TranslationSeedItem("entity.purchasesalesinventory.closingunitcost", "en-US", "期末单位成本_us", "期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）"),
            // entity.purchasesalesinventory.closingunitcost
            new TranslationSeedItem("entity.purchasesalesinventory.closingunitcost", "ja-JP", "期末单位成本_jp", "期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）"),
            // entity.purchasesalesinventory.closingunitcost
            new TranslationSeedItem("entity.purchasesalesinventory.closingunitcost", "zh-CN", "期末单位成本", "期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）"),
            // entity.purchasesalesinventory.closingunitcost
            new TranslationSeedItem("entity.purchasesalesinventory.closingunitcost", "zh-HK", "期末单位成本_hk", "期末单位成本（期末数量&gt;0 时 = 期末成本/期末数量）"),

            // entity.purchasesalesinventory.currencycode
            new TranslationSeedItem("entity.purchasesalesinventory.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code）"),
            // entity.purchasesalesinventory.currencycode
            new TranslationSeedItem("entity.purchasesalesinventory.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code）"),
            // entity.purchasesalesinventory.currencycode
            new TranslationSeedItem("entity.purchasesalesinventory.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code）"),
            // entity.purchasesalesinventory.currencycode
            new TranslationSeedItem("entity.purchasesalesinventory.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code）"),

            // entity.purchasesalesinventory.sortorder
            new TranslationSeedItem("entity.purchasesalesinventory.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.purchasesalesinventory.sortorder
            new TranslationSeedItem("entity.purchasesalesinventory.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.purchasesalesinventory.sortorder
            new TranslationSeedItem("entity.purchasesalesinventory.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.purchasesalesinventory.sortorder
            new TranslationSeedItem("entity.purchasesalesinventory.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.purchasesalesinventory.psistatus
            new TranslationSeedItem("entity.purchasesalesinventory.psistatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.purchasesalesinventory.psistatus
            new TranslationSeedItem("entity.purchasesalesinventory.psistatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.purchasesalesinventory.psistatus
            new TranslationSeedItem("entity.purchasesalesinventory.psistatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.purchasesalesinventory.psistatus
            new TranslationSeedItem("entity.purchasesalesinventory.psistatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
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
