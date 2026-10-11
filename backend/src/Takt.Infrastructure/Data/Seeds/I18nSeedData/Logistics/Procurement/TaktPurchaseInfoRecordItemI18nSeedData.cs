// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInfoRecordItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchaseInfoRecordItem 实体国际化翻译种子（键前缀 entity.purchaseinforecorditem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInfoRecordItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInfoRecordItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinforecorditem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInfoRecordItemTranslations())
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

        TaktLogger.Information("TaktPurchaseInfoRecordItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInfoRecordItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinforecorditem._self / entity.purchaseinforecorditem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInfoRecordItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinforecorditem._self
            new TranslationSeedItem("entity.purchaseinforecorditem._self", "en-US", "Purchase Info Record Item Information_us", "实体名称"),
            // entity.purchaseinforecorditem._self
            new TranslationSeedItem("entity.purchaseinforecorditem._self", "ja-JP", "Takt采购信息记录行信息_jp", "实体名称"),
            // entity.purchaseinforecorditem._self
            new TranslationSeedItem("entity.purchaseinforecorditem._self", "zh-CN", "Takt采购信息记录行信息", "实体名称"),
            // entity.purchaseinforecorditem._self
            new TranslationSeedItem("entity.purchaseinforecorditem._self", "zh-HK", "Takt采购信息记录行信息_hk", "实体名称"),

            // entity.purchaseinforecorditem.purchaseinforecordid
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordid", "en-US", "采购价格ID_us", "采购价格 ID（主子表关系；选项 TaktPurchaseInfoRecords/options，DictValue=Id）"),
            // entity.purchaseinforecorditem.purchaseinforecordid
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordid", "ja-JP", "采购价格ID_jp", "采购价格 ID（主子表关系；选项 TaktPurchaseInfoRecords/options，DictValue=Id）"),
            // entity.purchaseinforecorditem.purchaseinforecordid
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordid", "zh-CN", "采购价格ID", "采购价格 ID（主子表关系；选项 TaktPurchaseInfoRecords/options，DictValue=Id）"),
            // entity.purchaseinforecorditem.purchaseinforecordid
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordid", "zh-HK", "采购价格ID_hk", "采购价格 ID（主子表关系；选项 TaktPurchaseInfoRecords/options，DictValue=Id）"),

            // entity.purchaseinforecorditem.purchaseinforecordcode
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordcode", "en-US", "定价记录号_us", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseinforecorditem.purchaseinforecordcode
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordcode", "ja-JP", "定价记录号_jp", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseinforecorditem.purchaseinforecordcode
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordcode", "zh-CN", "定价记录号", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseinforecorditem.purchaseinforecordcode
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordcode", "zh-HK", "定价记录号_hk", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.purchaseinforecorditem.purchaseinforecordseq
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordseq", "en-US", "定价序号_us", "定价序号（项号/序号，固定步长=10）"),
            // entity.purchaseinforecorditem.purchaseinforecordseq
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordseq", "ja-JP", "定价序号_jp", "定价序号（项号/序号，固定步长=10）"),
            // entity.purchaseinforecorditem.purchaseinforecordseq
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordseq", "zh-CN", "定价序号", "定价序号（项号/序号，固定步长=10）"),
            // entity.purchaseinforecorditem.purchaseinforecordseq
            new TranslationSeedItem("entity.purchaseinforecorditem.purchaseinforecordseq", "zh-HK", "定价序号_hk", "定价序号（项号/序号，固定步长=10）"),

            // entity.purchaseinforecorditem.pricetype
            new TranslationSeedItem("entity.purchaseinforecorditem.pricetype", "en-US", "条件类型_us", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.purchaseinforecorditem.pricetype
            new TranslationSeedItem("entity.purchaseinforecorditem.pricetype", "ja-JP", "条件类型_jp", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.purchaseinforecorditem.pricetype
            new TranslationSeedItem("entity.purchaseinforecorditem.pricetype", "zh-CN", "条件类型", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.purchaseinforecorditem.pricetype
            new TranslationSeedItem("entity.purchaseinforecorditem.pricetype", "zh-HK", "条件类型_hk", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),

            // entity.purchaseinforecorditem.scaletype
            new TranslationSeedItem("entity.purchaseinforecorditem.scaletype", "en-US", "等级类型_us", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.purchaseinforecorditem.scaletype
            new TranslationSeedItem("entity.purchaseinforecorditem.scaletype", "ja-JP", "等级类型_jp", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.purchaseinforecorditem.scaletype
            new TranslationSeedItem("entity.purchaseinforecorditem.scaletype", "zh-CN", "等级类型", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.purchaseinforecorditem.scaletype
            new TranslationSeedItem("entity.purchaseinforecorditem.scaletype", "zh-HK", "等级类型_hk", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),

            // entity.purchaseinforecorditem.scalebasis
            new TranslationSeedItem("entity.purchaseinforecorditem.scalebasis", "en-US", "等级基础_us", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.purchaseinforecorditem.scalebasis
            new TranslationSeedItem("entity.purchaseinforecorditem.scalebasis", "ja-JP", "等级基础_jp", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.purchaseinforecorditem.scalebasis
            new TranslationSeedItem("entity.purchaseinforecorditem.scalebasis", "zh-CN", "等级基础", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.purchaseinforecorditem.scalebasis
            new TranslationSeedItem("entity.purchaseinforecorditem.scalebasis", "zh-HK", "等级基础_hk", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),

            // entity.purchaseinforecorditem.scalequantity
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantity", "en-US", "等级数量_us", "等级数量"),
            // entity.purchaseinforecorditem.scalequantity
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantity", "ja-JP", "等级数量_jp", "等级数量"),
            // entity.purchaseinforecorditem.scalequantity
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantity", "zh-CN", "等级数量", "等级数量"),
            // entity.purchaseinforecorditem.scalequantity
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantity", "zh-HK", "等级数量_hk", "等级数量"),

            // entity.purchaseinforecorditem.scaleunit
            new TranslationSeedItem("entity.purchaseinforecorditem.scaleunit", "en-US", "等级单位_us", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.purchaseinforecorditem.scaleunit
            new TranslationSeedItem("entity.purchaseinforecorditem.scaleunit", "ja-JP", "等级单位_jp", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.purchaseinforecorditem.scaleunit
            new TranslationSeedItem("entity.purchaseinforecorditem.scaleunit", "zh-CN", "等级单位", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.purchaseinforecorditem.scaleunit
            new TranslationSeedItem("entity.purchaseinforecorditem.scaleunit", "zh-HK", "等级单位_hk", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),

            // entity.purchaseinforecorditem.scalevalue
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalue", "en-US", "等级值_us", "等级值"),
            // entity.purchaseinforecorditem.scalevalue
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalue", "ja-JP", "等级值_jp", "等级值"),
            // entity.purchaseinforecorditem.scalevalue
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalue", "zh-CN", "等级值", "等级值"),
            // entity.purchaseinforecorditem.scalevalue
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalue", "zh-HK", "等级值_hk", "等级值"),

            // entity.purchaseinforecorditem.scalecurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.scalecurrencycode", "en-US", "等级货币_us", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.purchaseinforecorditem.scalecurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.scalecurrencycode", "ja-JP", "等级货币_jp", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.purchaseinforecorditem.scalecurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.scalecurrencycode", "zh-CN", "等级货币", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.purchaseinforecorditem.scalecurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.scalecurrencycode", "zh-HK", "等级货币_hk", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.purchaseinforecorditem.calculationtype
            new TranslationSeedItem("entity.purchaseinforecorditem.calculationtype", "en-US", "计算类型_us", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.purchaseinforecorditem.calculationtype
            new TranslationSeedItem("entity.purchaseinforecorditem.calculationtype", "ja-JP", "计算类型_jp", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.purchaseinforecorditem.calculationtype
            new TranslationSeedItem("entity.purchaseinforecorditem.calculationtype", "zh-CN", "计算类型", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.purchaseinforecorditem.calculationtype
            new TranslationSeedItem("entity.purchaseinforecorditem.calculationtype", "zh-HK", "计算类型_hk", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),

            // entity.purchaseinforecorditem.price
            new TranslationSeedItem("entity.purchaseinforecorditem.price", "en-US", "价格_us", "价格"),
            // entity.purchaseinforecorditem.price
            new TranslationSeedItem("entity.purchaseinforecorditem.price", "ja-JP", "价格_jp", "价格"),
            // entity.purchaseinforecorditem.price
            new TranslationSeedItem("entity.purchaseinforecorditem.price", "zh-CN", "价格", "价格"),
            // entity.purchaseinforecorditem.price
            new TranslationSeedItem("entity.purchaseinforecorditem.price", "zh-HK", "价格_hk", "价格"),

            // entity.purchaseinforecorditem.untaxedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.untaxedprice", "en-US", "未税价格_us", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.purchaseinforecorditem.untaxedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.untaxedprice", "ja-JP", "未税价格_jp", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.purchaseinforecorditem.untaxedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.untaxedprice", "zh-CN", "未税价格", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.purchaseinforecorditem.untaxedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.untaxedprice", "zh-HK", "未税价格_hk", "未税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.purchaseinforecorditem.taxincludedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.taxincludedprice", "en-US", "含税价格_us", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.purchaseinforecorditem.taxincludedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.taxincludedprice", "ja-JP", "含税价格_jp", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.purchaseinforecorditem.taxincludedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.taxincludedprice", "zh-CN", "含税价格", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.purchaseinforecorditem.taxincludedprice
            new TranslationSeedItem("entity.purchaseinforecorditem.taxincludedprice", "zh-HK", "含税价格_hk", "含税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.purchaseinforecorditem.taxamount
            new TranslationSeedItem("entity.purchaseinforecorditem.taxamount", "en-US", "税费_us", "税费（冗余；含税−未税，打印用）"),
            // entity.purchaseinforecorditem.taxamount
            new TranslationSeedItem("entity.purchaseinforecorditem.taxamount", "ja-JP", "税费_jp", "税费（冗余；含税−未税，打印用）"),
            // entity.purchaseinforecorditem.taxamount
            new TranslationSeedItem("entity.purchaseinforecorditem.taxamount", "zh-CN", "税费", "税费（冗余；含税−未税，打印用）"),
            // entity.purchaseinforecorditem.taxamount
            new TranslationSeedItem("entity.purchaseinforecorditem.taxamount", "zh-HK", "税费_hk", "税费（冗余；含税−未税，打印用）"),

            // entity.purchaseinforecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.conditioncurrencycode", "en-US", "条件货币_us", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.purchaseinforecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.conditioncurrencycode", "ja-JP", "条件货币_jp", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.purchaseinforecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.conditioncurrencycode", "zh-CN", "条件货币", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.purchaseinforecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.purchaseinforecorditem.conditioncurrencycode", "zh-HK", "条件货币_hk", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),

            // entity.purchaseinforecorditem.priceunit
            new TranslationSeedItem("entity.purchaseinforecorditem.priceunit", "en-US", "定价单位_us", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.purchaseinforecorditem.priceunit
            new TranslationSeedItem("entity.purchaseinforecorditem.priceunit", "ja-JP", "定价单位_jp", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.purchaseinforecorditem.priceunit
            new TranslationSeedItem("entity.purchaseinforecorditem.priceunit", "zh-CN", "定价单位", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.purchaseinforecorditem.priceunit
            new TranslationSeedItem("entity.purchaseinforecorditem.priceunit", "zh-HK", "定价单位_hk", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.purchaseinforecorditem.unitofmeasure
            new TranslationSeedItem("entity.purchaseinforecorditem.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseinforecorditem.unitofmeasure
            new TranslationSeedItem("entity.purchaseinforecorditem.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseinforecorditem.unitofmeasure
            new TranslationSeedItem("entity.purchaseinforecorditem.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseinforecorditem.unitofmeasure
            new TranslationSeedItem("entity.purchaseinforecorditem.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.purchaseinforecorditem.minorderquantity
            new TranslationSeedItem("entity.purchaseinforecorditem.minorderquantity", "en-US", "最小起订量_us", "最小起订量（计量单位数量，整数）"),
            // entity.purchaseinforecorditem.minorderquantity
            new TranslationSeedItem("entity.purchaseinforecorditem.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（计量单位数量，整数）"),
            // entity.purchaseinforecorditem.minorderquantity
            new TranslationSeedItem("entity.purchaseinforecorditem.minorderquantity", "zh-CN", "最小起订量", "最小起订量（计量单位数量，整数）"),
            // entity.purchaseinforecorditem.minorderquantity
            new TranslationSeedItem("entity.purchaseinforecorditem.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（计量单位数量，整数）"),

            // entity.purchaseinforecorditem.roundingvalue
            new TranslationSeedItem("entity.purchaseinforecorditem.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.purchaseinforecorditem.roundingvalue
            new TranslationSeedItem("entity.purchaseinforecorditem.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.purchaseinforecorditem.roundingvalue
            new TranslationSeedItem("entity.purchaseinforecorditem.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.purchaseinforecorditem.roundingvalue
            new TranslationSeedItem("entity.purchaseinforecorditem.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入，整数）"),

            // entity.purchaseinforecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.purchaseinforecorditem.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数，整数）"),
            // entity.purchaseinforecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.purchaseinforecorditem.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数，整数）"),
            // entity.purchaseinforecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.purchaseinforecorditem.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数，整数）"),
            // entity.purchaseinforecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.purchaseinforecorditem.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数，整数）"),

            // entity.purchaseinforecorditem.isobsolete
            new TranslationSeedItem("entity.purchaseinforecorditem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinforecorditem.isobsolete
            new TranslationSeedItem("entity.purchaseinforecorditem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinforecorditem.isobsolete
            new TranslationSeedItem("entity.purchaseinforecorditem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinforecorditem.isobsolete
            new TranslationSeedItem("entity.purchaseinforecorditem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.purchaseinforecorditem.scalequantities
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantities", "en-US", "数量等级行列表_us", "数量等级行列表（主子表关系）"),
            // entity.purchaseinforecorditem.scalequantities
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantities", "ja-JP", "数量等级行列表_jp", "数量等级行列表（主子表关系）"),
            // entity.purchaseinforecorditem.scalequantities
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantities", "zh-CN", "数量等级行列表", "数量等级行列表（主子表关系）"),
            // entity.purchaseinforecorditem.scalequantities
            new TranslationSeedItem("entity.purchaseinforecorditem.scalequantities", "zh-HK", "数量等级行列表_hk", "数量等级行列表（主子表关系）"),

            // entity.purchaseinforecorditem.scalevalues
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalues", "en-US", "价值等级行列表_us", "价值等级行列表（主子表关系）"),
            // entity.purchaseinforecorditem.scalevalues
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalues", "ja-JP", "价值等级行列表_jp", "价值等级行列表（主子表关系）"),
            // entity.purchaseinforecorditem.scalevalues
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalues", "zh-CN", "价值等级行列表", "价值等级行列表（主子表关系）"),
            // entity.purchaseinforecorditem.scalevalues
            new TranslationSeedItem("entity.purchaseinforecorditem.scalevalues", "zh-HK", "价值等级行列表_hk", "价值等级行列表（主子表关系）"),
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
        translation.ResourceGroup = "Procurement";
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
