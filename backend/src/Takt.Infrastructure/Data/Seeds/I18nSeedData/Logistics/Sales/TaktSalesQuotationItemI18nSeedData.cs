// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesQuotationItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesQuotationItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesQuotationItem 实体国际化翻译种子（键前缀 entity.salesquotationitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesQuotationItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesQuotationItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesquotationitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesQuotationItemTranslations())
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

        TaktLogger.Information("TaktSalesQuotationItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesQuotationItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesquotationitem._self / entity.salesquotationitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesQuotationItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "en-US", "Sales Quotation Item Information_us", "实体名称"),
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "ja-JP", "Takt销售报价明细信息_jp", "实体名称"),
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "zh-CN", "Takt销售报价明细信息", "实体名称"),
            // entity.salesquotationitem._self
            new TranslationSeedItem("entity.salesquotationitem._self", "zh-HK", "Takt销售报价明细信息_hk", "实体名称"),

            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "en-US", "销售报价ID_us", "销售报价（选项 TaktSalesQuotations/options；DictValue=Id）"),
            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "ja-JP", "销售报价ID_jp", "销售报价（选项 TaktSalesQuotations/options；DictValue=Id）"),
            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "zh-CN", "销售报价ID", "销售报价（选项 TaktSalesQuotations/options；DictValue=Id）"),
            // entity.salesquotationitem.salesquotationid
            new TranslationSeedItem("entity.salesquotationitem.salesquotationid", "zh-HK", "销售报价ID_hk", "销售报价（选项 TaktSalesQuotations/options；DictValue=Id）"),

            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "en-US", "销售报价编码_us", "销售报价编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "ja-JP", "销售报价编码_jp", "销售报价编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "zh-CN", "销售报价编码", "销售报价编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesquotationitem.salesquotationcode
            new TranslationSeedItem("entity.salesquotationitem.salesquotationcode", "zh-HK", "销售报价编码_hk", "销售报价编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesquotationitem.linenumber
            new TranslationSeedItem("entity.salesquotationitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesquotationitem.materialcode
            new TranslationSeedItem("entity.salesquotationitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.salesquotationitem.materialdescription
            new TranslationSeedItem("entity.salesquotationitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesquotationitem.materialdescription
            new TranslationSeedItem("entity.salesquotationitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesquotationitem.materialdescription
            new TranslationSeedItem("entity.salesquotationitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesquotationitem.materialdescription
            new TranslationSeedItem("entity.salesquotationitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.salesquotationitem.materialspecification
            new TranslationSeedItem("entity.salesquotationitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "en-US", "销售单位_us", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "ja-JP", "销售单位_jp", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "zh-CN", "销售单位", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesquotationitem.salesunit
            new TranslationSeedItem("entity.salesquotationitem.salesunit", "zh-HK", "销售单位_hk", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "en-US", "报价数量_us", "报价数量（基本单位数量）"),
            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "ja-JP", "报价数量_jp", "报价数量（基本单位数量）"),
            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "zh-CN", "报价数量", "报价数量（基本单位数量）"),
            // entity.salesquotationitem.quotationquantity
            new TranslationSeedItem("entity.salesquotationitem.quotationquantity", "zh-HK", "报价数量_hk", "报价数量（基本单位数量）"),

            // entity.salesquotationitem.salesperunit
            new TranslationSeedItem("entity.salesquotationitem.salesperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesquotationitem.salesperunit
            new TranslationSeedItem("entity.salesquotationitem.salesperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesquotationitem.salesperunit
            new TranslationSeedItem("entity.salesquotationitem.salesperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesquotationitem.salesperunit
            new TranslationSeedItem("entity.salesquotationitem.salesperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.salesquotationitem.quotationunitprice
            new TranslationSeedItem("entity.salesquotationitem.quotationunitprice", "en-US", "报价单价_us", "报价单价"),
            // entity.salesquotationitem.quotationunitprice
            new TranslationSeedItem("entity.salesquotationitem.quotationunitprice", "ja-JP", "报价单价_jp", "报价单价"),
            // entity.salesquotationitem.quotationunitprice
            new TranslationSeedItem("entity.salesquotationitem.quotationunitprice", "zh-CN", "报价单价", "报价单价"),
            // entity.salesquotationitem.quotationunitprice
            new TranslationSeedItem("entity.salesquotationitem.quotationunitprice", "zh-HK", "报价单价_hk", "报价单价"),

            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "en-US", "折扣率_us", "折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "ja-JP", "折扣率_jp", "折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "zh-CN", "折扣率", "折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),
            // entity.salesquotationitem.discountrate
            new TranslationSeedItem("entity.salesquotationitem.discountrate", "zh-HK", "折扣率_hk", "折扣率（字典 logistics_sales_discount_rate_param 预设或手输；0-100，表示折扣百分比）"),

            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "en-US", "折扣金额_us", "折扣金额"),
            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "ja-JP", "折扣金额_jp", "折扣金额"),
            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "zh-CN", "折扣金额", "折扣金额"),
            // entity.salesquotationitem.discountamount
            new TranslationSeedItem("entity.salesquotationitem.discountamount", "zh-HK", "折扣金额_hk", "折扣金额"),

            // entity.salesquotationitem.taxcode
            new TranslationSeedItem("entity.salesquotationitem.taxcode", "en-US", "税码_us", "税码（冗余：按 SalesQuotationId 取 TaktSalesQuotation.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salesquotationitem.taxcode
            new TranslationSeedItem("entity.salesquotationitem.taxcode", "ja-JP", "税码_jp", "税码（冗余：按 SalesQuotationId 取 TaktSalesQuotation.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salesquotationitem.taxcode
            new TranslationSeedItem("entity.salesquotationitem.taxcode", "zh-CN", "税码", "税码（冗余：按 SalesQuotationId 取 TaktSalesQuotation.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.salesquotationitem.taxcode
            new TranslationSeedItem("entity.salesquotationitem.taxcode", "zh-HK", "税码_hk", "税码（冗余：按 SalesQuotationId 取 TaktSalesQuotation.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.salesquotationitem.taxincludedamount
            new TranslationSeedItem("entity.salesquotationitem.taxincludedamount", "en-US", "含税金额_us", "含税金额"),
            // entity.salesquotationitem.taxincludedamount
            new TranslationSeedItem("entity.salesquotationitem.taxincludedamount", "ja-JP", "含税金额_jp", "含税金额"),
            // entity.salesquotationitem.taxincludedamount
            new TranslationSeedItem("entity.salesquotationitem.taxincludedamount", "zh-CN", "含税金额", "含税金额"),
            // entity.salesquotationitem.taxincludedamount
            new TranslationSeedItem("entity.salesquotationitem.taxincludedamount", "zh-HK", "含税金额_hk", "含税金额"),

            // entity.salesquotationitem.untaxedamount
            new TranslationSeedItem("entity.salesquotationitem.untaxedamount", "en-US", "未税金额_us", "未税金额"),
            // entity.salesquotationitem.untaxedamount
            new TranslationSeedItem("entity.salesquotationitem.untaxedamount", "ja-JP", "未税金额_jp", "未税金额"),
            // entity.salesquotationitem.untaxedamount
            new TranslationSeedItem("entity.salesquotationitem.untaxedamount", "zh-CN", "未税金额", "未税金额"),
            // entity.salesquotationitem.untaxedamount
            new TranslationSeedItem("entity.salesquotationitem.untaxedamount", "zh-HK", "未税金额_hk", "未税金额"),

            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesquotationitem.taxamount
            new TranslationSeedItem("entity.salesquotationitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.salesquotationitem.quotationamount
            new TranslationSeedItem("entity.salesquotationitem.quotationamount", "en-US", "报价金额_us", "报价金额"),
            // entity.salesquotationitem.quotationamount
            new TranslationSeedItem("entity.salesquotationitem.quotationamount", "ja-JP", "报价金额_jp", "报价金额"),
            // entity.salesquotationitem.quotationamount
            new TranslationSeedItem("entity.salesquotationitem.quotationamount", "zh-CN", "报价金额", "报价金额"),
            // entity.salesquotationitem.quotationamount
            new TranslationSeedItem("entity.salesquotationitem.quotationamount", "zh-HK", "报价金额_hk", "报价金额"),

            // entity.salesquotationitem.pricingdate
            new TranslationSeedItem("entity.salesquotationitem.pricingdate", "en-US", "价格日期_us", "价格日期"),
            // entity.salesquotationitem.pricingdate
            new TranslationSeedItem("entity.salesquotationitem.pricingdate", "ja-JP", "价格日期_jp", "价格日期"),
            // entity.salesquotationitem.pricingdate
            new TranslationSeedItem("entity.salesquotationitem.pricingdate", "zh-CN", "价格日期", "价格日期"),
            // entity.salesquotationitem.pricingdate
            new TranslationSeedItem("entity.salesquotationitem.pricingdate", "zh-HK", "价格日期_hk", "价格日期"),

            // entity.salesquotationitem.grossweight
            new TranslationSeedItem("entity.salesquotationitem.grossweight", "en-US", "毛重_us", "毛重"),
            // entity.salesquotationitem.grossweight
            new TranslationSeedItem("entity.salesquotationitem.grossweight", "ja-JP", "毛重_jp", "毛重"),
            // entity.salesquotationitem.grossweight
            new TranslationSeedItem("entity.salesquotationitem.grossweight", "zh-CN", "毛重", "毛重"),
            // entity.salesquotationitem.grossweight
            new TranslationSeedItem("entity.salesquotationitem.grossweight", "zh-HK", "毛重_hk", "毛重"),

            // entity.salesquotationitem.netweight
            new TranslationSeedItem("entity.salesquotationitem.netweight", "en-US", "净重_us", "净重"),
            // entity.salesquotationitem.netweight
            new TranslationSeedItem("entity.salesquotationitem.netweight", "ja-JP", "净重_jp", "净重"),
            // entity.salesquotationitem.netweight
            new TranslationSeedItem("entity.salesquotationitem.netweight", "zh-CN", "净重", "净重"),
            // entity.salesquotationitem.netweight
            new TranslationSeedItem("entity.salesquotationitem.netweight", "zh-HK", "净重_hk", "净重"),

            // entity.salesquotationitem.weightunit
            new TranslationSeedItem("entity.salesquotationitem.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.salesquotationitem.weightunit
            new TranslationSeedItem("entity.salesquotationitem.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.salesquotationitem.weightunit
            new TranslationSeedItem("entity.salesquotationitem.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.salesquotationitem.weightunit
            new TranslationSeedItem("entity.salesquotationitem.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),

            // entity.salesquotationitem.volume
            new TranslationSeedItem("entity.salesquotationitem.volume", "en-US", "体积_us", "体积"),
            // entity.salesquotationitem.volume
            new TranslationSeedItem("entity.salesquotationitem.volume", "ja-JP", "体积_jp", "体积"),
            // entity.salesquotationitem.volume
            new TranslationSeedItem("entity.salesquotationitem.volume", "zh-CN", "体积", "体积"),
            // entity.salesquotationitem.volume
            new TranslationSeedItem("entity.salesquotationitem.volume", "zh-HK", "体积_hk", "体积"),

            // entity.salesquotationitem.volumeunit
            new TranslationSeedItem("entity.salesquotationitem.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.salesquotationitem.volumeunit
            new TranslationSeedItem("entity.salesquotationitem.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.salesquotationitem.volumeunit
            new TranslationSeedItem("entity.salesquotationitem.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.salesquotationitem.volumeunit
            new TranslationSeedItem("entity.salesquotationitem.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),

            // entity.salesquotationitem.profitcentercode
            new TranslationSeedItem("entity.salesquotationitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesquotationitem.profitcentercode
            new TranslationSeedItem("entity.salesquotationitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesquotationitem.profitcentercode
            new TranslationSeedItem("entity.salesquotationitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesquotationitem.profitcentercode
            new TranslationSeedItem("entity.salesquotationitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.salesquotationitem.isobsolete
            new TranslationSeedItem("entity.salesquotationitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesquotationitem.isobsolete
            new TranslationSeedItem("entity.salesquotationitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesquotationitem.isobsolete
            new TranslationSeedItem("entity.salesquotationitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesquotationitem.isobsolete
            new TranslationSeedItem("entity.salesquotationitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salesquotationitem.salesquotation
            new TranslationSeedItem("entity.salesquotationitem.salesquotation", "en-US", "销售报价主表_us", "销售报价主表"),
            // entity.salesquotationitem.salesquotation
            new TranslationSeedItem("entity.salesquotationitem.salesquotation", "ja-JP", "销售报价主表_jp", "销售报价主表"),
            // entity.salesquotationitem.salesquotation
            new TranslationSeedItem("entity.salesquotationitem.salesquotation", "zh-CN", "销售报价主表", "销售报价主表"),
            // entity.salesquotationitem.salesquotation
            new TranslationSeedItem("entity.salesquotationitem.salesquotation", "zh-HK", "销售报价主表_hk", "销售报价主表"),
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
