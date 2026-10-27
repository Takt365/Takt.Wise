// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInquiryItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInquiryItem 实体国际化翻译种子（键前缀 entity.purchaseinquiryitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInquiryItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInquiryItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinquiryitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInquiryItemTranslations())
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

        TaktLogger.Information("TaktPurchaseInquiryItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInquiryItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinquiryitem._self / entity.purchaseinquiryitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInquiryItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "en-US", "Purchase Inquiry Item Information_us", "实体名称"),
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "ja-JP", "采购询价明细信息_jp", "实体名称"),
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "zh-CN", "采购询价明细信息", "实体名称"),
            // entity.purchaseinquiryitem._self
            new TranslationSeedItem("entity.purchaseinquiryitem._self", "zh-HK", "采购询价明细信息_hk", "实体名称"),

            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "en-US", "采购询价ID_us", "采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "ja-JP", "采购询价ID_jp", "采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "zh-CN", "采购询价ID", "采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),
            // entity.purchaseinquiryitem.purchaseinquiryid
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquiryid", "zh-HK", "采购询价ID_hk", "采购询价 ID（选项 TaktPurchaseInquirys/options；DictValue=Id）"),

            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "en-US", "采购询价编码_us", "采购询价编码（冗余，便于查询）"),
            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "ja-JP", "采购询价编码_jp", "采购询价编码（冗余，便于查询）"),
            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "zh-CN", "采购询价编码", "采购询价编码（冗余，便于查询）"),
            // entity.purchaseinquiryitem.purchaseinquirycode
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseinquirycode", "zh-HK", "采购询价编码_hk", "采购询价编码（冗余，便于查询）"),

            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaseinquiryitem.linenumber
            new TranslationSeedItem("entity.purchaseinquiryitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "en-US", "分配类别_us", "分配类别（字典 logistics_sales_allocation_category；A=资产，K=成本中心，F=订单）"),
            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "ja-JP", "分配类别_jp", "分配类别（字典 logistics_sales_allocation_category；A=资产，K=成本中心，F=订单）"),
            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "zh-CN", "分配类别", "分配类别（字典 logistics_sales_allocation_category；A=资产，K=成本中心，F=订单）"),
            // entity.purchaseinquiryitem.allocationcategory
            new TranslationSeedItem("entity.purchaseinquiryitem.allocationcategory", "zh-HK", "分配类别_hk", "分配类别（字典 logistics_sales_allocation_category；A=资产，K=成本中心，F=订单）"),

            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseinquiryitem.materialcode
            new TranslationSeedItem("entity.purchaseinquiryitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.purchaseinquiryitem.materialdescription
            new TranslationSeedItem("entity.purchaseinquiryitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseinquiryitem.materialdescription
            new TranslationSeedItem("entity.purchaseinquiryitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseinquiryitem.materialdescription
            new TranslationSeedItem("entity.purchaseinquiryitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaseinquiryitem.materialdescription
            new TranslationSeedItem("entity.purchaseinquiryitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.purchaseinquiryitem.materialspecification
            new TranslationSeedItem("entity.purchaseinquiryitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "en-US", "询价单位_us", "询价单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "ja-JP", "询价单位_jp", "询价单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "zh-CN", "询价单位", "询价单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaseinquiryitem.inquiryunit
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryunit", "zh-HK", "询价单位_hk", "询价单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "en-US", "询价数量_us", "询价数量（基本单位数量，decimal(18,5)）"),
            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "ja-JP", "询价数量_jp", "询价数量（基本单位数量，decimal(18,5)）"),
            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "zh-CN", "询价数量", "询价数量（基本单位数量，decimal(18,5)）"),
            // entity.purchaseinquiryitem.inquiryquantity
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryquantity", "zh-HK", "询价数量_hk", "询价数量（基本单位数量，decimal(18,5)）"),

            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaseinquiryitem.purchaseperunit
            new TranslationSeedItem("entity.purchaseinquiryitem.purchaseperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),

            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "en-US", "报价单价_us", "报价单价"),
            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "ja-JP", "报价单价_jp", "报价单价"),
            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "zh-CN", "报价单价", "报价单价"),
            // entity.purchaseinquiryitem.quotedunitprice
            new TranslationSeedItem("entity.purchaseinquiryitem.quotedunitprice", "zh-HK", "报价单价_hk", "报价单价"),

            // entity.purchaseinquiryitem.taxcode
            new TranslationSeedItem("entity.purchaseinquiryitem.taxcode", "en-US", "税码_us", "税码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinquiryitem.taxcode
            new TranslationSeedItem("entity.purchaseinquiryitem.taxcode", "ja-JP", "税码_jp", "税码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinquiryitem.taxcode
            new TranslationSeedItem("entity.purchaseinquiryitem.taxcode", "zh-CN", "税码", "税码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinquiryitem.taxcode
            new TranslationSeedItem("entity.purchaseinquiryitem.taxcode", "zh-HK", "税码_hk", "税码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.TaxCode 联动；选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.purchaseinquiryitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxincludedamount", "en-US", "含税金额_us", "含税金额"),
            // entity.purchaseinquiryitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxincludedamount", "ja-JP", "含税金额_jp", "含税金额"),
            // entity.purchaseinquiryitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxincludedamount", "zh-CN", "含税金额", "含税金额"),
            // entity.purchaseinquiryitem.taxincludedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxincludedamount", "zh-HK", "含税金额_hk", "含税金额"),

            // entity.purchaseinquiryitem.untaxedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.untaxedamount", "en-US", "未税金额_us", "未税金额"),
            // entity.purchaseinquiryitem.untaxedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.untaxedamount", "ja-JP", "未税金额_jp", "未税金额"),
            // entity.purchaseinquiryitem.untaxedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.untaxedamount", "zh-CN", "未税金额", "未税金额"),
            // entity.purchaseinquiryitem.untaxedamount
            new TranslationSeedItem("entity.purchaseinquiryitem.untaxedamount", "zh-HK", "未税金额_hk", "未税金额"),

            // entity.purchaseinquiryitem.taxamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.purchaseinquiryitem.taxamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.purchaseinquiryitem.taxamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.purchaseinquiryitem.taxamount
            new TranslationSeedItem("entity.purchaseinquiryitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.purchaseinquiryitem.inquiryamount
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryamount", "en-US", "询价金额_us", "询价金额"),
            // entity.purchaseinquiryitem.inquiryamount
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryamount", "ja-JP", "询价金额_jp", "询价金额"),
            // entity.purchaseinquiryitem.inquiryamount
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryamount", "zh-CN", "询价金额", "询价金额"),
            // entity.purchaseinquiryitem.inquiryamount
            new TranslationSeedItem("entity.purchaseinquiryitem.inquiryamount", "zh-HK", "询价金额_hk", "询价金额"),

            // entity.purchaseinquiryitem.pricingdate
            new TranslationSeedItem("entity.purchaseinquiryitem.pricingdate", "en-US", "价格日期_us", "价格日期"),
            // entity.purchaseinquiryitem.pricingdate
            new TranslationSeedItem("entity.purchaseinquiryitem.pricingdate", "ja-JP", "价格日期_jp", "价格日期"),
            // entity.purchaseinquiryitem.pricingdate
            new TranslationSeedItem("entity.purchaseinquiryitem.pricingdate", "zh-CN", "价格日期", "价格日期"),
            // entity.purchaseinquiryitem.pricingdate
            new TranslationSeedItem("entity.purchaseinquiryitem.pricingdate", "zh-HK", "价格日期_hk", "价格日期"),

            // entity.purchaseinquiryitem.grossweight
            new TranslationSeedItem("entity.purchaseinquiryitem.grossweight", "en-US", "毛重_us", "毛重"),
            // entity.purchaseinquiryitem.grossweight
            new TranslationSeedItem("entity.purchaseinquiryitem.grossweight", "ja-JP", "毛重_jp", "毛重"),
            // entity.purchaseinquiryitem.grossweight
            new TranslationSeedItem("entity.purchaseinquiryitem.grossweight", "zh-CN", "毛重", "毛重"),
            // entity.purchaseinquiryitem.grossweight
            new TranslationSeedItem("entity.purchaseinquiryitem.grossweight", "zh-HK", "毛重_hk", "毛重"),

            // entity.purchaseinquiryitem.netweight
            new TranslationSeedItem("entity.purchaseinquiryitem.netweight", "en-US", "净重_us", "净重"),
            // entity.purchaseinquiryitem.netweight
            new TranslationSeedItem("entity.purchaseinquiryitem.netweight", "ja-JP", "净重_jp", "净重"),
            // entity.purchaseinquiryitem.netweight
            new TranslationSeedItem("entity.purchaseinquiryitem.netweight", "zh-CN", "净重", "净重"),
            // entity.purchaseinquiryitem.netweight
            new TranslationSeedItem("entity.purchaseinquiryitem.netweight", "zh-HK", "净重_hk", "净重"),

            // entity.purchaseinquiryitem.weightunit
            new TranslationSeedItem("entity.purchaseinquiryitem.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.purchaseinquiryitem.weightunit
            new TranslationSeedItem("entity.purchaseinquiryitem.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.purchaseinquiryitem.weightunit
            new TranslationSeedItem("entity.purchaseinquiryitem.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.purchaseinquiryitem.weightunit
            new TranslationSeedItem("entity.purchaseinquiryitem.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),

            // entity.purchaseinquiryitem.volume
            new TranslationSeedItem("entity.purchaseinquiryitem.volume", "en-US", "体积_us", "体积"),
            // entity.purchaseinquiryitem.volume
            new TranslationSeedItem("entity.purchaseinquiryitem.volume", "ja-JP", "体积_jp", "体积"),
            // entity.purchaseinquiryitem.volume
            new TranslationSeedItem("entity.purchaseinquiryitem.volume", "zh-CN", "体积", "体积"),
            // entity.purchaseinquiryitem.volume
            new TranslationSeedItem("entity.purchaseinquiryitem.volume", "zh-HK", "体积_hk", "体积"),

            // entity.purchaseinquiryitem.volumeunit
            new TranslationSeedItem("entity.purchaseinquiryitem.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.purchaseinquiryitem.volumeunit
            new TranslationSeedItem("entity.purchaseinquiryitem.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.purchaseinquiryitem.volumeunit
            new TranslationSeedItem("entity.purchaseinquiryitem.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.purchaseinquiryitem.volumeunit
            new TranslationSeedItem("entity.purchaseinquiryitem.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),

            // entity.purchaseinquiryitem.profitcentercode
            new TranslationSeedItem("entity.purchaseinquiryitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.purchaseinquiryitem.profitcentercode
            new TranslationSeedItem("entity.purchaseinquiryitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.purchaseinquiryitem.profitcentercode
            new TranslationSeedItem("entity.purchaseinquiryitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.purchaseinquiryitem.profitcentercode
            new TranslationSeedItem("entity.purchaseinquiryitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.purchaseinquiryitem.isobsolete
            new TranslationSeedItem("entity.purchaseinquiryitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinquiryitem.isobsolete
            new TranslationSeedItem("entity.purchaseinquiryitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinquiryitem.isobsolete
            new TranslationSeedItem("entity.purchaseinquiryitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinquiryitem.isobsolete
            new TranslationSeedItem("entity.purchaseinquiryitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
