// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesConditionRecordItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesConditionRecordItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSalesConditionRecordItem 实体国际化翻译种子（键前缀 entity.salesconditionrecorditem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesConditionRecordItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesConditionRecordItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesconditionrecorditem 实体翻译...", tenantCode);

        foreach (var item in GetSalesConditionRecordItemTranslations())
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

        TaktLogger.Information("TaktSalesConditionRecordItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesConditionRecordItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesconditionrecorditem._self / entity.salesconditionrecorditem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesConditionRecordItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesconditionrecorditem._self
            new TranslationSeedItem("entity.salesconditionrecorditem._self", "en-US", "Sales Condition Record Item Information_us", "实体名称"),
            // entity.salesconditionrecorditem._self
            new TranslationSeedItem("entity.salesconditionrecorditem._self", "ja-JP", "Takt销售条件记录行信息_jp", "实体名称"),
            // entity.salesconditionrecorditem._self
            new TranslationSeedItem("entity.salesconditionrecorditem._self", "zh-CN", "Takt销售条件记录行信息", "实体名称"),
            // entity.salesconditionrecorditem._self
            new TranslationSeedItem("entity.salesconditionrecorditem._self", "zh-HK", "Takt销售条件记录行信息_hk", "实体名称"),

            // entity.salesconditionrecorditem.salesconditionrecordid
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordid", "en-US", "销售价格ID_us", "销售价格 ID（主子表关系；选项 TaktSalesConditionRecords/options，DictValue=Id）"),
            // entity.salesconditionrecorditem.salesconditionrecordid
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordid", "ja-JP", "销售价格ID_jp", "销售价格 ID（主子表关系；选项 TaktSalesConditionRecords/options，DictValue=Id）"),
            // entity.salesconditionrecorditem.salesconditionrecordid
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordid", "zh-CN", "销售价格ID", "销售价格 ID（主子表关系；选项 TaktSalesConditionRecords/options，DictValue=Id）"),
            // entity.salesconditionrecorditem.salesconditionrecordid
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordid", "zh-HK", "销售价格ID_hk", "销售价格 ID（主子表关系；选项 TaktSalesConditionRecords/options，DictValue=Id）"),

            // entity.salesconditionrecorditem.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordcode", "en-US", "定价记录号_us", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesconditionrecorditem.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordcode", "ja-JP", "定价记录号_jp", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesconditionrecorditem.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordcode", "zh-CN", "定价记录号", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesconditionrecorditem.salesconditionrecordcode
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordcode", "zh-HK", "定价记录号_hk", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.salesconditionrecorditem.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordseq", "en-US", "定价序号_us", "定价序号（项号/序号，固定步长=10）"),
            // entity.salesconditionrecorditem.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordseq", "ja-JP", "定价序号_jp", "定价序号（项号/序号，固定步长=10）"),
            // entity.salesconditionrecorditem.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordseq", "zh-CN", "定价序号", "定价序号（项号/序号，固定步长=10）"),
            // entity.salesconditionrecorditem.salesconditionrecordseq
            new TranslationSeedItem("entity.salesconditionrecorditem.salesconditionrecordseq", "zh-HK", "定价序号_hk", "定价序号（项号/序号，固定步长=10）"),

            // entity.salesconditionrecorditem.pricetype
            new TranslationSeedItem("entity.salesconditionrecorditem.pricetype", "en-US", "条件类型_us", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salesconditionrecorditem.pricetype
            new TranslationSeedItem("entity.salesconditionrecorditem.pricetype", "ja-JP", "条件类型_jp", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salesconditionrecorditem.pricetype
            new TranslationSeedItem("entity.salesconditionrecorditem.pricetype", "zh-CN", "条件类型", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salesconditionrecorditem.pricetype
            new TranslationSeedItem("entity.salesconditionrecorditem.pricetype", "zh-HK", "条件类型_hk", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),

            // entity.salesconditionrecorditem.scaletype
            new TranslationSeedItem("entity.salesconditionrecorditem.scaletype", "en-US", "等级类型_us", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salesconditionrecorditem.scaletype
            new TranslationSeedItem("entity.salesconditionrecorditem.scaletype", "ja-JP", "等级类型_jp", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salesconditionrecorditem.scaletype
            new TranslationSeedItem("entity.salesconditionrecorditem.scaletype", "zh-CN", "等级类型", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salesconditionrecorditem.scaletype
            new TranslationSeedItem("entity.salesconditionrecorditem.scaletype", "zh-HK", "等级类型_hk", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),

            // entity.salesconditionrecorditem.scalebasis
            new TranslationSeedItem("entity.salesconditionrecorditem.scalebasis", "en-US", "等级基础_us", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salesconditionrecorditem.scalebasis
            new TranslationSeedItem("entity.salesconditionrecorditem.scalebasis", "ja-JP", "等级基础_jp", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salesconditionrecorditem.scalebasis
            new TranslationSeedItem("entity.salesconditionrecorditem.scalebasis", "zh-CN", "等级基础", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salesconditionrecorditem.scalebasis
            new TranslationSeedItem("entity.salesconditionrecorditem.scalebasis", "zh-HK", "等级基础_hk", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),

            // entity.salesconditionrecorditem.scalequantity
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantity", "en-US", "等级数量_us", "等级数量"),
            // entity.salesconditionrecorditem.scalequantity
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantity", "ja-JP", "等级数量_jp", "等级数量"),
            // entity.salesconditionrecorditem.scalequantity
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantity", "zh-CN", "等级数量", "等级数量"),
            // entity.salesconditionrecorditem.scalequantity
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantity", "zh-HK", "等级数量_hk", "等级数量"),

            // entity.salesconditionrecorditem.scaleunit
            new TranslationSeedItem("entity.salesconditionrecorditem.scaleunit", "en-US", "等级单位_us", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salesconditionrecorditem.scaleunit
            new TranslationSeedItem("entity.salesconditionrecorditem.scaleunit", "ja-JP", "等级单位_jp", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salesconditionrecorditem.scaleunit
            new TranslationSeedItem("entity.salesconditionrecorditem.scaleunit", "zh-CN", "等级单位", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salesconditionrecorditem.scaleunit
            new TranslationSeedItem("entity.salesconditionrecorditem.scaleunit", "zh-HK", "等级单位_hk", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),

            // entity.salesconditionrecorditem.scalevalue
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalue", "en-US", "等级值_us", "等级值"),
            // entity.salesconditionrecorditem.scalevalue
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalue", "ja-JP", "等级值_jp", "等级值"),
            // entity.salesconditionrecorditem.scalevalue
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalue", "zh-CN", "等级值", "等级值"),
            // entity.salesconditionrecorditem.scalevalue
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalue", "zh-HK", "等级值_hk", "等级值"),

            // entity.salesconditionrecorditem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.scalecurrencycode", "en-US", "等级货币_us", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.salesconditionrecorditem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.scalecurrencycode", "ja-JP", "等级货币_jp", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.salesconditionrecorditem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.scalecurrencycode", "zh-CN", "等级货币", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.salesconditionrecorditem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.scalecurrencycode", "zh-HK", "等级货币_hk", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.salesconditionrecorditem.calculationtype
            new TranslationSeedItem("entity.salesconditionrecorditem.calculationtype", "en-US", "计算类型_us", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.salesconditionrecorditem.calculationtype
            new TranslationSeedItem("entity.salesconditionrecorditem.calculationtype", "ja-JP", "计算类型_jp", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.salesconditionrecorditem.calculationtype
            new TranslationSeedItem("entity.salesconditionrecorditem.calculationtype", "zh-CN", "计算类型", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.salesconditionrecorditem.calculationtype
            new TranslationSeedItem("entity.salesconditionrecorditem.calculationtype", "zh-HK", "计算类型_hk", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),

            // entity.salesconditionrecorditem.price
            new TranslationSeedItem("entity.salesconditionrecorditem.price", "en-US", "价格_us", "价格"),
            // entity.salesconditionrecorditem.price
            new TranslationSeedItem("entity.salesconditionrecorditem.price", "ja-JP", "价格_jp", "价格"),
            // entity.salesconditionrecorditem.price
            new TranslationSeedItem("entity.salesconditionrecorditem.price", "zh-CN", "价格", "价格"),
            // entity.salesconditionrecorditem.price
            new TranslationSeedItem("entity.salesconditionrecorditem.price", "zh-HK", "价格_hk", "价格"),

            // entity.salesconditionrecorditem.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.untaxedprice", "en-US", "未税价格_us", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecorditem.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.untaxedprice", "ja-JP", "未税价格_jp", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecorditem.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.untaxedprice", "zh-CN", "未税价格", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecorditem.untaxedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.untaxedprice", "zh-HK", "未税价格_hk", "未税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salesconditionrecorditem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.taxincludedprice", "en-US", "含税价格_us", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecorditem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.taxincludedprice", "ja-JP", "含税价格_jp", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecorditem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.taxincludedprice", "zh-CN", "含税价格", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionrecorditem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionrecorditem.taxincludedprice", "zh-HK", "含税价格_hk", "含税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salesconditionrecorditem.taxamount
            new TranslationSeedItem("entity.salesconditionrecorditem.taxamount", "en-US", "税费_us", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionrecorditem.taxamount
            new TranslationSeedItem("entity.salesconditionrecorditem.taxamount", "ja-JP", "税费_jp", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionrecorditem.taxamount
            new TranslationSeedItem("entity.salesconditionrecorditem.taxamount", "zh-CN", "税费", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionrecorditem.taxamount
            new TranslationSeedItem("entity.salesconditionrecorditem.taxamount", "zh-HK", "税费_hk", "税费（冗余；含税−未税，打印用）"),

            // entity.salesconditionrecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.conditioncurrencycode", "en-US", "条件货币_us", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salesconditionrecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.conditioncurrencycode", "ja-JP", "条件货币_jp", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salesconditionrecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.conditioncurrencycode", "zh-CN", "条件货币", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salesconditionrecorditem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionrecorditem.conditioncurrencycode", "zh-HK", "条件货币_hk", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),

            // entity.salesconditionrecorditem.priceunit
            new TranslationSeedItem("entity.salesconditionrecorditem.priceunit", "en-US", "定价单位_us", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesconditionrecorditem.priceunit
            new TranslationSeedItem("entity.salesconditionrecorditem.priceunit", "ja-JP", "定价单位_jp", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesconditionrecorditem.priceunit
            new TranslationSeedItem("entity.salesconditionrecorditem.priceunit", "zh-CN", "定价单位", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesconditionrecorditem.priceunit
            new TranslationSeedItem("entity.salesconditionrecorditem.priceunit", "zh-HK", "定价单位_hk", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.salesconditionrecorditem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionrecorditem.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesconditionrecorditem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionrecorditem.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesconditionrecorditem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionrecorditem.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesconditionrecorditem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionrecorditem.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.salesconditionrecorditem.minorderquantity
            new TranslationSeedItem("entity.salesconditionrecorditem.minorderquantity", "en-US", "最小起订量_us", "最小起订量（计量单位数量，整数）"),
            // entity.salesconditionrecorditem.minorderquantity
            new TranslationSeedItem("entity.salesconditionrecorditem.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（计量单位数量，整数）"),
            // entity.salesconditionrecorditem.minorderquantity
            new TranslationSeedItem("entity.salesconditionrecorditem.minorderquantity", "zh-CN", "最小起订量", "最小起订量（计量单位数量，整数）"),
            // entity.salesconditionrecorditem.minorderquantity
            new TranslationSeedItem("entity.salesconditionrecorditem.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（计量单位数量，整数）"),

            // entity.salesconditionrecorditem.roundingvalue
            new TranslationSeedItem("entity.salesconditionrecorditem.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salesconditionrecorditem.roundingvalue
            new TranslationSeedItem("entity.salesconditionrecorditem.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salesconditionrecorditem.roundingvalue
            new TranslationSeedItem("entity.salesconditionrecorditem.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salesconditionrecorditem.roundingvalue
            new TranslationSeedItem("entity.salesconditionrecorditem.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入，整数）"),

            // entity.salesconditionrecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionrecorditem.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数，整数）"),
            // entity.salesconditionrecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionrecorditem.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数，整数）"),
            // entity.salesconditionrecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionrecorditem.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数，整数）"),
            // entity.salesconditionrecorditem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionrecorditem.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数，整数）"),

            // entity.salesconditionrecorditem.isobsolete
            new TranslationSeedItem("entity.salesconditionrecorditem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionrecorditem.isobsolete
            new TranslationSeedItem("entity.salesconditionrecorditem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionrecorditem.isobsolete
            new TranslationSeedItem("entity.salesconditionrecorditem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionrecorditem.isobsolete
            new TranslationSeedItem("entity.salesconditionrecorditem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salesconditionrecorditem.scalequantities
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantities", "en-US", "数量等级行列表_us", "数量等级行列表（主子表关系）"),
            // entity.salesconditionrecorditem.scalequantities
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantities", "ja-JP", "数量等级行列表_jp", "数量等级行列表（主子表关系）"),
            // entity.salesconditionrecorditem.scalequantities
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantities", "zh-CN", "数量等级行列表", "数量等级行列表（主子表关系）"),
            // entity.salesconditionrecorditem.scalequantities
            new TranslationSeedItem("entity.salesconditionrecorditem.scalequantities", "zh-HK", "数量等级行列表_hk", "数量等级行列表（主子表关系）"),

            // entity.salesconditionrecorditem.scalevalues
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalues", "en-US", "价值等级行列表_us", "价值等级行列表（主子表关系）"),
            // entity.salesconditionrecorditem.scalevalues
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalues", "ja-JP", "价值等级行列表_jp", "价值等级行列表（主子表关系）"),
            // entity.salesconditionrecorditem.scalevalues
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalues", "zh-CN", "价值等级行列表", "价值等级行列表（主子表关系）"),
            // entity.salesconditionrecorditem.scalevalues
            new TranslationSeedItem("entity.salesconditionrecorditem.scalevalues", "zh-HK", "价值等级行列表_hk", "价值等级行列表（主子表关系）"),
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
        translation.ResourceGroup = "Sales";
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
