// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktInventoryReserveI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktInventoryReserve 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktInventoryReserve 实体国际化翻译种子（键前缀 entity.inventoryreserve.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktInventoryReserveI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktInventoryReserve 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 inventoryreserve 实体翻译...", tenantCode);

        foreach (var item in GetInventoryReserveTranslations())
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

        TaktLogger.Information("TaktInventoryReserve 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktInventoryReserve 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.inventoryreserve._self / entity.inventoryreserve.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetInventoryReserveTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.inventoryreserve._self
            new TranslationSeedItem("entity.inventoryreserve._self", "en-US", "Inventory Reserve Information_us", "实体名称"),
            // entity.inventoryreserve._self
            new TranslationSeedItem("entity.inventoryreserve._self", "ja-JP", "存货跌价准备信息_jp", "实体名称"),
            // entity.inventoryreserve._self
            new TranslationSeedItem("entity.inventoryreserve._self", "zh-CN", "存货跌价准备信息", "实体名称"),
            // entity.inventoryreserve._self
            new TranslationSeedItem("entity.inventoryreserve._self", "zh-HK", "存货跌价准备信息_hk", "实体名称"),

            // entity.inventoryreserve.perioddate
            new TranslationSeedItem("entity.inventoryreserve.perioddate", "en-US", "会计期间_us", "期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）"),
            // entity.inventoryreserve.perioddate
            new TranslationSeedItem("entity.inventoryreserve.perioddate", "ja-JP", "会计期间_jp", "期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）"),
            // entity.inventoryreserve.perioddate
            new TranslationSeedItem("entity.inventoryreserve.perioddate", "zh-CN", "会计期间", "期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）"),
            // entity.inventoryreserve.perioddate
            new TranslationSeedItem("entity.inventoryreserve.perioddate", "zh-HK", "会计期间_hk", "期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）"),

            // entity.inventoryreserve.materialcode
            new TranslationSeedItem("entity.inventoryreserve.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.inventoryreserve.materialcode
            new TranslationSeedItem("entity.inventoryreserve.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.inventoryreserve.materialcode
            new TranslationSeedItem("entity.inventoryreserve.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.inventoryreserve.materialcode
            new TranslationSeedItem("entity.inventoryreserve.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.inventoryreserve.materialdescription
            new TranslationSeedItem("entity.inventoryreserve.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.inventoryreserve.materialdescription
            new TranslationSeedItem("entity.inventoryreserve.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.inventoryreserve.materialdescription
            new TranslationSeedItem("entity.inventoryreserve.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.inventoryreserve.materialdescription
            new TranslationSeedItem("entity.inventoryreserve.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.inventoryreserve.valuation
            new TranslationSeedItem("entity.inventoryreserve.valuation", "en-US", "评估类别_us", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.inventoryreserve.valuation
            new TranslationSeedItem("entity.inventoryreserve.valuation", "ja-JP", "评估类别_jp", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.inventoryreserve.valuation
            new TranslationSeedItem("entity.inventoryreserve.valuation", "zh-CN", "评估类别", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.inventoryreserve.valuation
            new TranslationSeedItem("entity.inventoryreserve.valuation", "zh-HK", "评估类别_hk", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),

            // entity.inventoryreserve.provisionscope
            new TranslationSeedItem("entity.inventoryreserve.provisionscope", "en-US", "计提范围_us", "计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）"),
            // entity.inventoryreserve.provisionscope
            new TranslationSeedItem("entity.inventoryreserve.provisionscope", "ja-JP", "计提范围_jp", "计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）"),
            // entity.inventoryreserve.provisionscope
            new TranslationSeedItem("entity.inventoryreserve.provisionscope", "zh-CN", "计提范围", "计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）"),
            // entity.inventoryreserve.provisionscope
            new TranslationSeedItem("entity.inventoryreserve.provisionscope", "zh-HK", "计提范围_hk", "计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）"),

            // entity.inventoryreserve.stockquantity
            new TranslationSeedItem("entity.inventoryreserve.stockquantity", "en-US", "库存数量_us", "库存数量（基本单位，4 位小数）"),
            // entity.inventoryreserve.stockquantity
            new TranslationSeedItem("entity.inventoryreserve.stockquantity", "ja-JP", "库存数量_jp", "库存数量（基本单位，4 位小数）"),
            // entity.inventoryreserve.stockquantity
            new TranslationSeedItem("entity.inventoryreserve.stockquantity", "zh-CN", "库存数量", "库存数量（基本单位，4 位小数）"),
            // entity.inventoryreserve.stockquantity
            new TranslationSeedItem("entity.inventoryreserve.stockquantity", "zh-HK", "库存数量_hk", "库存数量（基本单位，4 位小数）"),

            // entity.inventoryreserve.unitcost
            new TranslationSeedItem("entity.inventoryreserve.unitcost", "en-US", "单位成本_us", "单位成本（存货取得/结存单位成本；与币种一致，5 位小数）"),
            // entity.inventoryreserve.unitcost
            new TranslationSeedItem("entity.inventoryreserve.unitcost", "ja-JP", "单位成本_jp", "单位成本（存货取得/结存单位成本；与币种一致，5 位小数）"),
            // entity.inventoryreserve.unitcost
            new TranslationSeedItem("entity.inventoryreserve.unitcost", "zh-CN", "单位成本", "单位成本（存货取得/结存单位成本；与币种一致，5 位小数）"),
            // entity.inventoryreserve.unitcost
            new TranslationSeedItem("entity.inventoryreserve.unitcost", "zh-HK", "单位成本_hk", "单位成本（存货取得/结存单位成本；与币种一致，5 位小数）"),

            // entity.inventoryreserve.inventorycost
            new TranslationSeedItem("entity.inventoryreserve.inventorycost", "en-US", "存货成本_us", "存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）"),
            // entity.inventoryreserve.inventorycost
            new TranslationSeedItem("entity.inventoryreserve.inventorycost", "ja-JP", "存货成本_jp", "存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）"),
            // entity.inventoryreserve.inventorycost
            new TranslationSeedItem("entity.inventoryreserve.inventorycost", "zh-CN", "存货成本", "存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）"),
            // entity.inventoryreserve.inventorycost
            new TranslationSeedItem("entity.inventoryreserve.inventorycost", "zh-HK", "存货成本_hk", "存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）"),

            // entity.inventoryreserve.estimatedsellingprice
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingprice", "en-US", "估计售价_us", "估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）"),
            // entity.inventoryreserve.estimatedsellingprice
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingprice", "ja-JP", "估计售价_jp", "估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）"),
            // entity.inventoryreserve.estimatedsellingprice
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingprice", "zh-CN", "估计售价", "估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）"),
            // entity.inventoryreserve.estimatedsellingprice
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingprice", "zh-HK", "估计售价_hk", "估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）"),

            // entity.inventoryreserve.estimatedcompletioncost
            new TranslationSeedItem("entity.inventoryreserve.estimatedcompletioncost", "en-US", "至完工估计成本_us", "至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）"),
            // entity.inventoryreserve.estimatedcompletioncost
            new TranslationSeedItem("entity.inventoryreserve.estimatedcompletioncost", "ja-JP", "至完工估计成本_jp", "至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）"),
            // entity.inventoryreserve.estimatedcompletioncost
            new TranslationSeedItem("entity.inventoryreserve.estimatedcompletioncost", "zh-CN", "至完工估计成本", "至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）"),
            // entity.inventoryreserve.estimatedcompletioncost
            new TranslationSeedItem("entity.inventoryreserve.estimatedcompletioncost", "zh-HK", "至完工估计成本_hk", "至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）"),

            // entity.inventoryreserve.estimatedsellingcost
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingcost", "en-US", "估计销售费用_us", "销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）"),
            // entity.inventoryreserve.estimatedsellingcost
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingcost", "ja-JP", "估计销售费用_jp", "销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）"),
            // entity.inventoryreserve.estimatedsellingcost
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingcost", "zh-CN", "估计销售费用", "销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）"),
            // entity.inventoryreserve.estimatedsellingcost
            new TranslationSeedItem("entity.inventoryreserve.estimatedsellingcost", "zh-HK", "估计销售费用_hk", "销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）"),

            // entity.inventoryreserve.netrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.netrealizablevalue", "en-US", "可变现净值_us", "可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）"),
            // entity.inventoryreserve.netrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.netrealizablevalue", "ja-JP", "可变现净值_jp", "可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）"),
            // entity.inventoryreserve.netrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.netrealizablevalue", "zh-CN", "可变现净值", "可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）"),
            // entity.inventoryreserve.netrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.netrealizablevalue", "zh-HK", "可变现净值_hk", "可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）"),

            // entity.inventoryreserve.unitnetrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.unitnetrealizablevalue", "en-US", "单位可变现净值_us", "单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）"),
            // entity.inventoryreserve.unitnetrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.unitnetrealizablevalue", "ja-JP", "单位可变现净值_jp", "单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）"),
            // entity.inventoryreserve.unitnetrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.unitnetrealizablevalue", "zh-CN", "单位可变现净值", "单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）"),
            // entity.inventoryreserve.unitnetrealizablevalue
            new TranslationSeedItem("entity.inventoryreserve.unitnetrealizablevalue", "zh-HK", "单位可变现净值_hk", "单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）"),

            // entity.inventoryreserve.openingprovision
            new TranslationSeedItem("entity.inventoryreserve.openingprovision", "en-US", "期初跌价准备_us", "期初跌价准备余额（存货跌价准备科目期初贷方余额）"),
            // entity.inventoryreserve.openingprovision
            new TranslationSeedItem("entity.inventoryreserve.openingprovision", "ja-JP", "期初跌价准备_jp", "期初跌价准备余额（存货跌价准备科目期初贷方余额）"),
            // entity.inventoryreserve.openingprovision
            new TranslationSeedItem("entity.inventoryreserve.openingprovision", "zh-CN", "期初跌价准备", "期初跌价准备余额（存货跌价准备科目期初贷方余额）"),
            // entity.inventoryreserve.openingprovision
            new TranslationSeedItem("entity.inventoryreserve.openingprovision", "zh-HK", "期初跌价准备_hk", "期初跌价准备余额（存货跌价准备科目期初贷方余额）"),

            // entity.inventoryreserve.provisionamount
            new TranslationSeedItem("entity.inventoryreserve.provisionamount", "en-US", "本期计提金额_us", "本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）"),
            // entity.inventoryreserve.provisionamount
            new TranslationSeedItem("entity.inventoryreserve.provisionamount", "ja-JP", "本期计提金额_jp", "本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）"),
            // entity.inventoryreserve.provisionamount
            new TranslationSeedItem("entity.inventoryreserve.provisionamount", "zh-CN", "本期计提金额", "本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）"),
            // entity.inventoryreserve.provisionamount
            new TranslationSeedItem("entity.inventoryreserve.provisionamount", "zh-HK", "本期计提金额_hk", "本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）"),

            // entity.inventoryreserve.reversalamount
            new TranslationSeedItem("entity.inventoryreserve.reversalamount", "en-US", "本期转回金额_us", "本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）"),
            // entity.inventoryreserve.reversalamount
            new TranslationSeedItem("entity.inventoryreserve.reversalamount", "ja-JP", "本期转回金额_jp", "本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）"),
            // entity.inventoryreserve.reversalamount
            new TranslationSeedItem("entity.inventoryreserve.reversalamount", "zh-CN", "本期转回金额", "本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）"),
            // entity.inventoryreserve.reversalamount
            new TranslationSeedItem("entity.inventoryreserve.reversalamount", "zh-HK", "本期转回金额_hk", "本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）"),

            // entity.inventoryreserve.closingprovision
            new TranslationSeedItem("entity.inventoryreserve.closingprovision", "en-US", "期末跌价准备_us", "期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）"),
            // entity.inventoryreserve.closingprovision
            new TranslationSeedItem("entity.inventoryreserve.closingprovision", "ja-JP", "期末跌价准备_jp", "期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）"),
            // entity.inventoryreserve.closingprovision
            new TranslationSeedItem("entity.inventoryreserve.closingprovision", "zh-CN", "期末跌价准备", "期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）"),
            // entity.inventoryreserve.closingprovision
            new TranslationSeedItem("entity.inventoryreserve.closingprovision", "zh-HK", "期末跌价准备_hk", "期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）"),

            // entity.inventoryreserve.impairmentloss
            new TranslationSeedItem("entity.inventoryreserve.impairmentloss", "en-US", "本期净损益影响_us", "本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）"),
            // entity.inventoryreserve.impairmentloss
            new TranslationSeedItem("entity.inventoryreserve.impairmentloss", "ja-JP", "本期净损益影响_jp", "本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）"),
            // entity.inventoryreserve.impairmentloss
            new TranslationSeedItem("entity.inventoryreserve.impairmentloss", "zh-CN", "本期净损益影响", "本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）"),
            // entity.inventoryreserve.impairmentloss
            new TranslationSeedItem("entity.inventoryreserve.impairmentloss", "zh-HK", "本期净损益影响_hk", "本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）"),

            // entity.inventoryreserve.carryingamount
            new TranslationSeedItem("entity.inventoryreserve.carryingamount", "en-US", "账面价值_us", "账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）"),
            // entity.inventoryreserve.carryingamount
            new TranslationSeedItem("entity.inventoryreserve.carryingamount", "ja-JP", "账面价值_jp", "账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）"),
            // entity.inventoryreserve.carryingamount
            new TranslationSeedItem("entity.inventoryreserve.carryingamount", "zh-CN", "账面价值", "账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）"),
            // entity.inventoryreserve.carryingamount
            new TranslationSeedItem("entity.inventoryreserve.carryingamount", "zh-HK", "账面价值_hk", "账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）"),

            // entity.inventoryreserve.currencycode
            new TranslationSeedItem("entity.inventoryreserve.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.inventoryreserve.currencycode
            new TranslationSeedItem("entity.inventoryreserve.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.inventoryreserve.currencycode
            new TranslationSeedItem("entity.inventoryreserve.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.inventoryreserve.currencycode
            new TranslationSeedItem("entity.inventoryreserve.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.inventoryreserve.impairmentreason
            new TranslationSeedItem("entity.inventoryreserve.impairmentreason", "en-US", "跌价原因_us", "跌价原因说明（业务备注：滞销、毁损、市价下跌等）"),
            // entity.inventoryreserve.impairmentreason
            new TranslationSeedItem("entity.inventoryreserve.impairmentreason", "ja-JP", "跌价原因_jp", "跌价原因说明（业务备注：滞销、毁损、市价下跌等）"),
            // entity.inventoryreserve.impairmentreason
            new TranslationSeedItem("entity.inventoryreserve.impairmentreason", "zh-CN", "跌价原因", "跌价原因说明（业务备注：滞销、毁损、市价下跌等）"),
            // entity.inventoryreserve.impairmentreason
            new TranslationSeedItem("entity.inventoryreserve.impairmentreason", "zh-HK", "跌价原因_hk", "跌价原因说明（业务备注：滞销、毁损、市价下跌等）"),

            // entity.inventoryreserve.sortorder
            new TranslationSeedItem("entity.inventoryreserve.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.inventoryreserve.sortorder
            new TranslationSeedItem("entity.inventoryreserve.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.inventoryreserve.sortorder
            new TranslationSeedItem("entity.inventoryreserve.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.inventoryreserve.sortorder
            new TranslationSeedItem("entity.inventoryreserve.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.inventoryreserve.provisionstatus
            new TranslationSeedItem("entity.inventoryreserve.provisionstatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.inventoryreserve.provisionstatus
            new TranslationSeedItem("entity.inventoryreserve.provisionstatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.inventoryreserve.provisionstatus
            new TranslationSeedItem("entity.inventoryreserve.provisionstatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
            // entity.inventoryreserve.provisionstatus
            new TranslationSeedItem("entity.inventoryreserve.provisionstatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用，0=停用）"),
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
        translation.ResourceGroup = "Materials";
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
