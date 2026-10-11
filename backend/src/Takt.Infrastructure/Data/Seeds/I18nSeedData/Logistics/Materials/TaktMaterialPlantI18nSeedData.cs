// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialPlantI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialPlant 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialPlant 实体国际化翻译种子（键前缀 entity.materialplant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialPlantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialPlant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialplant 实体翻译...", tenantCode);

        foreach (var item in GetMaterialPlantTranslations())
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

        TaktLogger.Information("TaktMaterialPlant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialPlant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialplant._self / entity.materialplant.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialPlantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "en-US", "Material Plant Information_us", "实体名称"),
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "ja-JP", "Takt工厂物料信息_jp", "实体名称"),
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "zh-CN", "Takt工厂物料信息", "实体名称"),
            // entity.materialplant._self
            new TranslationSeedItem("entity.materialplant._self", "zh-HK", "Takt工厂物料信息_hk", "实体名称"),

            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),
            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),
            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),
            // entity.materialplant.materialcode
            new TranslationSeedItem("entity.materialplant.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),

            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）"),
            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）"),
            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）"),
            // entity.materialplant.materialdescription
            new TranslationSeedItem("entity.materialplant.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）"),

            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.materialplant.materialspecification
            new TranslationSeedItem("entity.materialplant.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "en-US", "行业领域_us", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "ja-JP", "行业领域_jp", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "zh-CN", "行业领域", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),
            // entity.materialplant.industrysector
            new TranslationSeedItem("entity.materialplant.industrysector", "zh-HK", "行业领域_hk", "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）"),

            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "en-US", "物料层级_us", "物料层级"),
            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "ja-JP", "物料层级_jp", "物料层级"),
            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "zh-CN", "物料层级", "物料层级"),
            // entity.materialplant.materialhierarchy
            new TranslationSeedItem("entity.materialplant.materialhierarchy", "zh-HK", "物料层级_hk", "物料层级"),

            // entity.materialplant.materialgroup
            new TranslationSeedItem("entity.materialplant.materialgroup", "en-US", "物料组_us", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.materialplant.materialgroup
            new TranslationSeedItem("entity.materialplant.materialgroup", "ja-JP", "物料组_jp", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.materialplant.materialgroup
            new TranslationSeedItem("entity.materialplant.materialgroup", "zh-CN", "物料组", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.materialplant.materialgroup
            new TranslationSeedItem("entity.materialplant.materialgroup", "zh-HK", "物料组_hk", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),

            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "en-US", "物料类型_us", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "ja-JP", "物料类型_jp", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "zh-CN", "物料类型", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),
            // entity.materialplant.materialtype
            new TranslationSeedItem("entity.materialplant.materialtype", "zh-HK", "物料类型_hk", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）"),

            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "en-US", "基本单位_us", "基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "ja-JP", "基本单位_jp", "基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "zh-CN", "基本单位", "基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.materialplant.baseunit
            new TranslationSeedItem("entity.materialplant.baseunit", "zh-HK", "基本单位_hk", "基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.materialplant.purchasegroup
            new TranslationSeedItem("entity.materialplant.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "en-US", "采购类型_us", "采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）"),
            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "ja-JP", "采购类型_jp", "采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）"),
            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "zh-CN", "采购类型", "采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）"),
            // entity.materialplant.purchasetype
            new TranslationSeedItem("entity.materialplant.purchasetype", "zh-HK", "采购类型_hk", "采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）"),

            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "en-US", "特殊采购_us", "特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）"),
            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "ja-JP", "特殊采购_jp", "特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）"),
            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "zh-CN", "特殊采购", "特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）"),
            // entity.materialplant.specialprocurement
            new TranslationSeedItem("entity.materialplant.specialprocurement", "zh-HK", "特殊采购_hk", "特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）"),

            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "en-US", "是否散装_us", "是否散装（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "ja-JP", "是否散装_jp", "是否散装（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "zh-CN", "是否散装", "是否散装（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.isbulk
            new TranslationSeedItem("entity.materialplant.isbulk", "zh-HK", "是否散装_hk", "是否散装（字典 sys_yes_no；0=否，1=是）"),

            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "en-US", "最小起订量_us", "最小起订量（基本单位数量，整数）"),
            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（基本单位数量，整数）"),
            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "zh-CN", "最小起订量", "最小起订量（基本单位数量，整数）"),
            // entity.materialplant.minorderquantity
            new TranslationSeedItem("entity.materialplant.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（基本单位数量，整数）"),

            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.materialplant.roundingvalue
            new TranslationSeedItem("entity.materialplant.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入，整数）"),

            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数，整数）"),
            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数，整数）"),
            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数，整数）"),
            // entity.materialplant.planneddeliverytimedays
            new TranslationSeedItem("entity.materialplant.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数，整数）"),

            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "en-US", "自制生产天数_us", "自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）"),
            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "ja-JP", "自制生产天数_jp", "自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）"),
            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "zh-CN", "自制生产天数", "自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）"),
            // entity.materialplant.inhouseproductiondays
            new TranslationSeedItem("entity.materialplant.inhouseproductiondays", "zh-HK", "自制生产天数_hk", "自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）"),

            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "en-US", "制造商_us", "制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "ja-JP", "制造商_jp", "制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "zh-CN", "制造商", "制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.materialplant.manufacturer
            new TranslationSeedItem("entity.materialplant.manufacturer", "zh-HK", "制造商_hk", "制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.materialplant.manufacturermaterialcode
            new TranslationSeedItem("entity.materialplant.manufacturermaterialcode", "en-US", "制造商物料编码_us", "制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),
            // entity.materialplant.manufacturermaterialcode
            new TranslationSeedItem("entity.materialplant.manufacturermaterialcode", "ja-JP", "制造商物料编码_jp", "制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),
            // entity.materialplant.manufacturermaterialcode
            new TranslationSeedItem("entity.materialplant.manufacturermaterialcode", "zh-CN", "制造商物料编码", "制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),
            // entity.materialplant.manufacturermaterialcode
            new TranslationSeedItem("entity.materialplant.manufacturermaterialcode", "zh-HK", "制造商物料编码_hk", "制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）"),

            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.materialplant.currencycode
            new TranslationSeedItem("entity.materialplant.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "en-US", "价格控制_us", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),
            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "ja-JP", "价格控制_jp", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),
            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "zh-CN", "价格控制", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),
            // entity.materialplant.pricecontrol
            new TranslationSeedItem("entity.materialplant.pricecontrol", "zh-HK", "价格控制_hk", "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）"),

            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "en-US", "价格单位_us", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "zh-CN", "价格单位", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.materialplant.priceunit
            new TranslationSeedItem("entity.materialplant.priceunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.materialplant.valuation
            new TranslationSeedItem("entity.materialplant.valuation", "en-US", "评估类别_us", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.materialplant.valuation
            new TranslationSeedItem("entity.materialplant.valuation", "ja-JP", "评估类别_jp", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.materialplant.valuation
            new TranslationSeedItem("entity.materialplant.valuation", "zh-CN", "评估类别", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),
            // entity.materialplant.valuation
            new TranslationSeedItem("entity.materialplant.valuation", "zh-HK", "评估类别_hk", "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）"),

            // entity.materialplant.movingprice
            new TranslationSeedItem("entity.materialplant.movingprice", "en-US", "移动价格_us", "移动价格（decimal，4 位小数）"),
            // entity.materialplant.movingprice
            new TranslationSeedItem("entity.materialplant.movingprice", "ja-JP", "移动价格_jp", "移动价格（decimal，4 位小数）"),
            // entity.materialplant.movingprice
            new TranslationSeedItem("entity.materialplant.movingprice", "zh-CN", "移动价格", "移动价格（decimal，4 位小数）"),
            // entity.materialplant.movingprice
            new TranslationSeedItem("entity.materialplant.movingprice", "zh-HK", "移动价格_hk", "移动价格（decimal，4 位小数）"),

            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "en-US", "差异码_us", "差异码（6）"),
            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "ja-JP", "差异码_jp", "差异码（6）"),
            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "zh-CN", "差异码", "差异码（6）"),
            // entity.materialplant.differencecode
            new TranslationSeedItem("entity.materialplant.differencecode", "zh-HK", "差异码_hk", "差异码（6）"),

            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.materialplant.profitcenter
            new TranslationSeedItem("entity.materialplant.profitcenter", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "en-US", "当前库存_us", "当前库存（基本单位数量，decimal，4 位小数）"),
            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "ja-JP", "当前库存_jp", "当前库存（基本单位数量，decimal，4 位小数）"),
            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "zh-CN", "当前库存", "当前库存（基本单位数量，decimal，4 位小数）"),
            // entity.materialplant.currentstock
            new TranslationSeedItem("entity.materialplant.currentstock", "zh-HK", "当前库存_hk", "当前库存（基本单位数量，decimal，4 位小数）"),

            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "en-US", "生产仓储_us", "生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "ja-JP", "生产仓储_jp", "生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "zh-CN", "生产仓储", "生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialplant.productionlocation
            new TranslationSeedItem("entity.materialplant.productionlocation", "zh-HK", "生产仓储_hk", "生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),

            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "en-US", "采购仓储_us", "采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "ja-JP", "采购仓储_jp", "采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "zh-CN", "采购仓储", "采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialplant.purchasinglocation
            new TranslationSeedItem("entity.materialplant.purchasinglocation", "zh-HK", "采购仓储_hk", "采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),

            // entity.materialplant.storagelocation
            new TranslationSeedItem("entity.materialplant.storagelocation", "en-US", "库位_us", "库位（选项 TaktStorageLocations/options；DictValue=LocationCode）"),
            // entity.materialplant.storagelocation
            new TranslationSeedItem("entity.materialplant.storagelocation", "ja-JP", "库位_jp", "库位（选项 TaktStorageLocations/options；DictValue=LocationCode）"),
            // entity.materialplant.storagelocation
            new TranslationSeedItem("entity.materialplant.storagelocation", "zh-CN", "库位", "库位（选项 TaktStorageLocations/options；DictValue=LocationCode）"),
            // entity.materialplant.storagelocation
            new TranslationSeedItem("entity.materialplant.storagelocation", "zh-HK", "库位_hk", "库位（选项 TaktStorageLocations/options；DictValue=LocationCode）"),

            // entity.materialplant.requiresinspection
            new TranslationSeedItem("entity.materialplant.requiresinspection", "en-US", "是否需检验_us", "是否需检验（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.requiresinspection
            new TranslationSeedItem("entity.materialplant.requiresinspection", "ja-JP", "是否需检验_jp", "是否需检验（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.requiresinspection
            new TranslationSeedItem("entity.materialplant.requiresinspection", "zh-CN", "是否需检验", "是否需检验（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.requiresinspection
            new TranslationSeedItem("entity.materialplant.requiresinspection", "zh-HK", "是否需检验_hk", "是否需检验（字典 sys_yes_no；0=否，1=是）"),

            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "en-US", "批次标识_us", "批次标识（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "ja-JP", "批次标识_jp", "批次标识（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "zh-CN", "批次标识", "批次标识（字典 sys_yes_no；0=否，1=是）"),
            // entity.materialplant.isbatch
            new TranslationSeedItem("entity.materialplant.isbatch", "zh-HK", "批次标识_hk", "批次标识（字典 sys_yes_no；0=否，1=是）"),

            // entity.materialplant.discontinuedstatus
            new TranslationSeedItem("entity.materialplant.discontinuedstatus", "en-US", "停产状态_us", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）"),
            // entity.materialplant.discontinuedstatus
            new TranslationSeedItem("entity.materialplant.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）"),
            // entity.materialplant.discontinuedstatus
            new TranslationSeedItem("entity.materialplant.discontinuedstatus", "zh-CN", "停产状态", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）"),
            // entity.materialplant.discontinuedstatus
            new TranslationSeedItem("entity.materialplant.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）"),

            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "en-US", "物料状态_us", "物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "ja-JP", "物料状态_jp", "物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "zh-CN", "物料状态", "物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.materialplant.materialstatus
            new TranslationSeedItem("entity.materialplant.materialstatus", "zh-HK", "物料状态_hk", "物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
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
