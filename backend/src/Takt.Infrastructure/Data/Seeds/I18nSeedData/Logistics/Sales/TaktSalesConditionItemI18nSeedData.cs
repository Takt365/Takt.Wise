// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesConditionItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesConditionItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesConditionItem 实体国际化翻译种子（键前缀 entity.salesconditionitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesConditionItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesConditionItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesconditionitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesConditionItemTranslations())
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

        TaktLogger.Information("TaktSalesConditionItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesConditionItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesconditionitem._self / entity.salesconditionitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesConditionItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesconditionitem._self
            new TranslationSeedItem("entity.salesconditionitem._self", "en-US", "Sales Condition Item Information_us", "实体名称"),
            // entity.salesconditionitem._self
            new TranslationSeedItem("entity.salesconditionitem._self", "ja-JP", "Takt销售条件行信息_jp", "实体名称"),
            // entity.salesconditionitem._self
            new TranslationSeedItem("entity.salesconditionitem._self", "zh-CN", "Takt销售条件行信息", "实体名称"),
            // entity.salesconditionitem._self
            new TranslationSeedItem("entity.salesconditionitem._self", "zh-HK", "Takt销售条件行信息_hk", "实体名称"),

            // entity.salesconditionitem.salesconditionid
            new TranslationSeedItem("entity.salesconditionitem.salesconditionid", "en-US", "销售价格ID_us", "销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）"),
            // entity.salesconditionitem.salesconditionid
            new TranslationSeedItem("entity.salesconditionitem.salesconditionid", "ja-JP", "销售价格ID_jp", "销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）"),
            // entity.salesconditionitem.salesconditionid
            new TranslationSeedItem("entity.salesconditionitem.salesconditionid", "zh-CN", "销售价格ID", "销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）"),
            // entity.salesconditionitem.salesconditionid
            new TranslationSeedItem("entity.salesconditionitem.salesconditionid", "zh-HK", "销售价格ID_hk", "销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）"),

            // entity.salesconditionitem.salesconditioncode
            new TranslationSeedItem("entity.salesconditionitem.salesconditioncode", "en-US", "定价记录号_us", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesconditionitem.salesconditioncode
            new TranslationSeedItem("entity.salesconditionitem.salesconditioncode", "ja-JP", "定价记录号_jp", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesconditionitem.salesconditioncode
            new TranslationSeedItem("entity.salesconditionitem.salesconditioncode", "zh-CN", "定价记录号", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesconditionitem.salesconditioncode
            new TranslationSeedItem("entity.salesconditionitem.salesconditioncode", "zh-HK", "定价记录号_hk", "定价记录号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.salesconditionitem.salesconditionseq
            new TranslationSeedItem("entity.salesconditionitem.salesconditionseq", "en-US", "定价序号_us", "定价序号（项号/序号，固定步长=10）"),
            // entity.salesconditionitem.salesconditionseq
            new TranslationSeedItem("entity.salesconditionitem.salesconditionseq", "ja-JP", "定价序号_jp", "定价序号（项号/序号，固定步长=10）"),
            // entity.salesconditionitem.salesconditionseq
            new TranslationSeedItem("entity.salesconditionitem.salesconditionseq", "zh-CN", "定价序号", "定价序号（项号/序号，固定步长=10）"),
            // entity.salesconditionitem.salesconditionseq
            new TranslationSeedItem("entity.salesconditionitem.salesconditionseq", "zh-HK", "定价序号_hk", "定价序号（项号/序号，固定步长=10）"),

            // entity.salesconditionitem.pricetype
            new TranslationSeedItem("entity.salesconditionitem.pricetype", "en-US", "条件类型_us", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salesconditionitem.pricetype
            new TranslationSeedItem("entity.salesconditionitem.pricetype", "ja-JP", "条件类型_jp", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salesconditionitem.pricetype
            new TranslationSeedItem("entity.salesconditionitem.pricetype", "zh-CN", "条件类型", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),
            // entity.salesconditionitem.pricetype
            new TranslationSeedItem("entity.salesconditionitem.pricetype", "zh-HK", "条件类型_hk", "条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）"),

            // entity.salesconditionitem.scaletype
            new TranslationSeedItem("entity.salesconditionitem.scaletype", "en-US", "等级类型_us", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salesconditionitem.scaletype
            new TranslationSeedItem("entity.salesconditionitem.scaletype", "ja-JP", "等级类型_jp", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salesconditionitem.scaletype
            new TranslationSeedItem("entity.salesconditionitem.scaletype", "zh-CN", "等级类型", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),
            // entity.salesconditionitem.scaletype
            new TranslationSeedItem("entity.salesconditionitem.scaletype", "zh-HK", "等级类型_hk", "等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）"),

            // entity.salesconditionitem.scalebasis
            new TranslationSeedItem("entity.salesconditionitem.scalebasis", "en-US", "等级基础_us", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salesconditionitem.scalebasis
            new TranslationSeedItem("entity.salesconditionitem.scalebasis", "ja-JP", "等级基础_jp", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salesconditionitem.scalebasis
            new TranslationSeedItem("entity.salesconditionitem.scalebasis", "zh-CN", "等级基础", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),
            // entity.salesconditionitem.scalebasis
            new TranslationSeedItem("entity.salesconditionitem.scalebasis", "zh-HK", "等级基础_hk", "等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）"),

            // entity.salesconditionitem.scalequantity
            new TranslationSeedItem("entity.salesconditionitem.scalequantity", "en-US", "等级数量_us", "等级数量"),
            // entity.salesconditionitem.scalequantity
            new TranslationSeedItem("entity.salesconditionitem.scalequantity", "ja-JP", "等级数量_jp", "等级数量"),
            // entity.salesconditionitem.scalequantity
            new TranslationSeedItem("entity.salesconditionitem.scalequantity", "zh-CN", "等级数量", "等级数量"),
            // entity.salesconditionitem.scalequantity
            new TranslationSeedItem("entity.salesconditionitem.scalequantity", "zh-HK", "等级数量_hk", "等级数量"),

            // entity.salesconditionitem.scaleunit
            new TranslationSeedItem("entity.salesconditionitem.scaleunit", "en-US", "等级单位_us", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salesconditionitem.scaleunit
            new TranslationSeedItem("entity.salesconditionitem.scaleunit", "ja-JP", "等级单位_jp", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salesconditionitem.scaleunit
            new TranslationSeedItem("entity.salesconditionitem.scaleunit", "zh-CN", "等级单位", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),
            // entity.salesconditionitem.scaleunit
            new TranslationSeedItem("entity.salesconditionitem.scaleunit", "zh-HK", "等级单位_hk", "等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）"),

            // entity.salesconditionitem.scalevalue
            new TranslationSeedItem("entity.salesconditionitem.scalevalue", "en-US", "等级值_us", "等级值"),
            // entity.salesconditionitem.scalevalue
            new TranslationSeedItem("entity.salesconditionitem.scalevalue", "ja-JP", "等级值_jp", "等级值"),
            // entity.salesconditionitem.scalevalue
            new TranslationSeedItem("entity.salesconditionitem.scalevalue", "zh-CN", "等级值", "等级值"),
            // entity.salesconditionitem.scalevalue
            new TranslationSeedItem("entity.salesconditionitem.scalevalue", "zh-HK", "等级值_hk", "等级值"),

            // entity.salesconditionitem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionitem.scalecurrencycode", "en-US", "等级货币_us", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.salesconditionitem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionitem.scalecurrencycode", "ja-JP", "等级货币_jp", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.salesconditionitem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionitem.scalecurrencycode", "zh-CN", "等级货币", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.salesconditionitem.scalecurrencycode
            new TranslationSeedItem("entity.salesconditionitem.scalecurrencycode", "zh-HK", "等级货币_hk", "等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.salesconditionitem.calculationtype
            new TranslationSeedItem("entity.salesconditionitem.calculationtype", "en-US", "计算类型_us", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.salesconditionitem.calculationtype
            new TranslationSeedItem("entity.salesconditionitem.calculationtype", "ja-JP", "计算类型_jp", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.salesconditionitem.calculationtype
            new TranslationSeedItem("entity.salesconditionitem.calculationtype", "zh-CN", "计算类型", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),
            // entity.salesconditionitem.calculationtype
            new TranslationSeedItem("entity.salesconditionitem.calculationtype", "zh-HK", "计算类型_hk", "计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）"),

            // entity.salesconditionitem.price
            new TranslationSeedItem("entity.salesconditionitem.price", "en-US", "价格_us", "价格"),
            // entity.salesconditionitem.price
            new TranslationSeedItem("entity.salesconditionitem.price", "ja-JP", "价格_jp", "价格"),
            // entity.salesconditionitem.price
            new TranslationSeedItem("entity.salesconditionitem.price", "zh-CN", "价格", "价格"),
            // entity.salesconditionitem.price
            new TranslationSeedItem("entity.salesconditionitem.price", "zh-HK", "价格_hk", "价格"),

            // entity.salesconditionitem.untaxedprice
            new TranslationSeedItem("entity.salesconditionitem.untaxedprice", "en-US", "未税价格_us", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionitem.untaxedprice
            new TranslationSeedItem("entity.salesconditionitem.untaxedprice", "ja-JP", "未税价格_jp", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionitem.untaxedprice
            new TranslationSeedItem("entity.salesconditionitem.untaxedprice", "zh-CN", "未税价格", "未税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionitem.untaxedprice
            new TranslationSeedItem("entity.salesconditionitem.untaxedprice", "zh-HK", "未税价格_hk", "未税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salesconditionitem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionitem.taxincludedprice", "en-US", "含税价格_us", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionitem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionitem.taxincludedprice", "ja-JP", "含税价格_jp", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionitem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionitem.taxincludedprice", "zh-CN", "含税价格", "含税价格（冗余；可由 Price 与税码推算后回写）"),
            // entity.salesconditionitem.taxincludedprice
            new TranslationSeedItem("entity.salesconditionitem.taxincludedprice", "zh-HK", "含税价格_hk", "含税价格（冗余；可由 Price 与税码推算后回写）"),

            // entity.salesconditionitem.taxamount
            new TranslationSeedItem("entity.salesconditionitem.taxamount", "en-US", "税费_us", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionitem.taxamount
            new TranslationSeedItem("entity.salesconditionitem.taxamount", "ja-JP", "税费_jp", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionitem.taxamount
            new TranslationSeedItem("entity.salesconditionitem.taxamount", "zh-CN", "税费", "税费（冗余；含税−未税，打印用）"),
            // entity.salesconditionitem.taxamount
            new TranslationSeedItem("entity.salesconditionitem.taxamount", "zh-HK", "税费_hk", "税费（冗余；含税−未税，打印用）"),

            // entity.salesconditionitem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionitem.conditioncurrencycode", "en-US", "条件货币_us", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salesconditionitem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionitem.conditioncurrencycode", "ja-JP", "条件货币_jp", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salesconditionitem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionitem.conditioncurrencycode", "zh-CN", "条件货币", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),
            // entity.salesconditionitem.conditioncurrencycode
            new TranslationSeedItem("entity.salesconditionitem.conditioncurrencycode", "zh-HK", "条件货币_hk", "条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）"),

            // entity.salesconditionitem.priceunit
            new TranslationSeedItem("entity.salesconditionitem.priceunit", "en-US", "定价单位_us", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesconditionitem.priceunit
            new TranslationSeedItem("entity.salesconditionitem.priceunit", "ja-JP", "定价单位_jp", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesconditionitem.priceunit
            new TranslationSeedItem("entity.salesconditionitem.priceunit", "zh-CN", "定价单位", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesconditionitem.priceunit
            new TranslationSeedItem("entity.salesconditionitem.priceunit", "zh-HK", "定价单位_hk", "定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.salesconditionitem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionitem.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesconditionitem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionitem.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesconditionitem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionitem.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesconditionitem.unitofmeasure
            new TranslationSeedItem("entity.salesconditionitem.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.salesconditionitem.minorderquantity
            new TranslationSeedItem("entity.salesconditionitem.minorderquantity", "en-US", "最小起订量_us", "最小起订量（计量单位数量，整数）"),
            // entity.salesconditionitem.minorderquantity
            new TranslationSeedItem("entity.salesconditionitem.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（计量单位数量，整数）"),
            // entity.salesconditionitem.minorderquantity
            new TranslationSeedItem("entity.salesconditionitem.minorderquantity", "zh-CN", "最小起订量", "最小起订量（计量单位数量，整数）"),
            // entity.salesconditionitem.minorderquantity
            new TranslationSeedItem("entity.salesconditionitem.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（计量单位数量，整数）"),

            // entity.salesconditionitem.roundingvalue
            new TranslationSeedItem("entity.salesconditionitem.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salesconditionitem.roundingvalue
            new TranslationSeedItem("entity.salesconditionitem.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salesconditionitem.roundingvalue
            new TranslationSeedItem("entity.salesconditionitem.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.salesconditionitem.roundingvalue
            new TranslationSeedItem("entity.salesconditionitem.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入，整数）"),

            // entity.salesconditionitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionitem.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数，整数）"),
            // entity.salesconditionitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionitem.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数，整数）"),
            // entity.salesconditionitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionitem.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数，整数）"),
            // entity.salesconditionitem.planneddeliverytimedays
            new TranslationSeedItem("entity.salesconditionitem.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数，整数）"),

            // entity.salesconditionitem.isobsolete
            new TranslationSeedItem("entity.salesconditionitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionitem.isobsolete
            new TranslationSeedItem("entity.salesconditionitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionitem.isobsolete
            new TranslationSeedItem("entity.salesconditionitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesconditionitem.isobsolete
            new TranslationSeedItem("entity.salesconditionitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salesconditionitem.scalequantities
            new TranslationSeedItem("entity.salesconditionitem.scalequantities", "en-US", "数量等级行列表_us", "数量等级行列表（主子表关系）"),
            // entity.salesconditionitem.scalequantities
            new TranslationSeedItem("entity.salesconditionitem.scalequantities", "ja-JP", "数量等级行列表_jp", "数量等级行列表（主子表关系）"),
            // entity.salesconditionitem.scalequantities
            new TranslationSeedItem("entity.salesconditionitem.scalequantities", "zh-CN", "数量等级行列表", "数量等级行列表（主子表关系）"),
            // entity.salesconditionitem.scalequantities
            new TranslationSeedItem("entity.salesconditionitem.scalequantities", "zh-HK", "数量等级行列表_hk", "数量等级行列表（主子表关系）"),

            // entity.salesconditionitem.scalevalues
            new TranslationSeedItem("entity.salesconditionitem.scalevalues", "en-US", "价值等级行列表_us", "价值等级行列表（主子表关系）"),
            // entity.salesconditionitem.scalevalues
            new TranslationSeedItem("entity.salesconditionitem.scalevalues", "ja-JP", "价值等级行列表_jp", "价值等级行列表（主子表关系）"),
            // entity.salesconditionitem.scalevalues
            new TranslationSeedItem("entity.salesconditionitem.scalevalues", "zh-CN", "价值等级行列表", "价值等级行列表（主子表关系）"),
            // entity.salesconditionitem.scalevalues
            new TranslationSeedItem("entity.salesconditionitem.scalevalues", "zh-HK", "价值等级行列表_hk", "价值等级行列表（主子表关系）"),
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
