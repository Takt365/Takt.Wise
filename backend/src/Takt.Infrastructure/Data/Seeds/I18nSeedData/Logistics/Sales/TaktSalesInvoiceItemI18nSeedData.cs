// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesInvoiceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesInvoiceItem 实体国际化翻译种子（键前缀 entity.salesinvoiceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesInvoiceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesInvoiceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesinvoiceitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesInvoiceItemTranslations())
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

        TaktLogger.Information("TaktSalesInvoiceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesInvoiceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesinvoiceitem._self / entity.salesinvoiceitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "en-US", "Sales Invoice Item Information_us", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "ja-JP", "Takt销售发票明细信息_jp", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "zh-CN", "Takt销售发票明细信息", "实体名称"),
            // entity.salesinvoiceitem._self
            new TranslationSeedItem("entity.salesinvoiceitem._self", "zh-HK", "Takt销售发票明细信息_hk", "实体名称"),

            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "en-US", "销售发票ID_us", "销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "ja-JP", "销售发票ID_jp", "销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "zh-CN", "销售发票ID", "销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）"),
            // entity.salesinvoiceitem.salesinvoiceid
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoiceid", "zh-HK", "销售发票ID_hk", "销售发票ID（选项 TaktSalesInvoices/options；DictValue=Id）"),

            // entity.salesinvoiceitem.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.billingdocumentcode", "en-US", "开票凭证_us", "开票凭证（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesinvoiceitem.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.billingdocumentcode", "ja-JP", "开票凭证_jp", "开票凭证（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesinvoiceitem.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.billingdocumentcode", "zh-CN", "开票凭证", "开票凭证（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesinvoiceitem.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.billingdocumentcode", "zh-HK", "开票凭证_hk", "开票凭证（冗余：按对应 Id 取主数据名称联动）"),

            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "en-US", "项目_us", "项目（开票凭证项目；行号步长生成器用 int，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "ja-JP", "项目_jp", "项目（开票凭证项目；行号步长生成器用 int，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "zh-CN", "项目", "项目（开票凭证项目；行号步长生成器用 int，固定步长=10）"),
            // entity.salesinvoiceitem.linenumber
            new TranslationSeedItem("entity.salesinvoiceitem.linenumber", "zh-HK", "项目_hk", "项目（开票凭证项目；行号步长生成器用 int，固定步长=10）"),

            // entity.salesinvoiceitem.billingquantity
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantity", "en-US", "已出发票数量_us", "已出发票数量"),
            // entity.salesinvoiceitem.billingquantity
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantity", "ja-JP", "已出发票数量_jp", "已出发票数量"),
            // entity.salesinvoiceitem.billingquantity
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantity", "zh-CN", "已出发票数量", "已出发票数量"),
            // entity.salesinvoiceitem.billingquantity
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantity", "zh-HK", "已出发票数量_hk", "已出发票数量"),

            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "en-US", "销售单位_us", "销售单位"),
            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "ja-JP", "销售单位_jp", "销售单位"),
            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "zh-CN", "销售单位", "销售单位"),
            // entity.salesinvoiceitem.salesunit
            new TranslationSeedItem("entity.salesinvoiceitem.salesunit", "zh-HK", "销售单位_hk", "销售单位"),

            // entity.salesinvoiceitem.baseunit
            new TranslationSeedItem("entity.salesinvoiceitem.baseunit", "en-US", "基本计量单位_us", "基本计量单位"),
            // entity.salesinvoiceitem.baseunit
            new TranslationSeedItem("entity.salesinvoiceitem.baseunit", "ja-JP", "基本计量单位_jp", "基本计量单位"),
            // entity.salesinvoiceitem.baseunit
            new TranslationSeedItem("entity.salesinvoiceitem.baseunit", "zh-CN", "基本计量单位", "基本计量单位"),
            // entity.salesinvoiceitem.baseunit
            new TranslationSeedItem("entity.salesinvoiceitem.baseunit", "zh-HK", "基本计量单位_hk", "基本计量单位"),

            // entity.salesinvoiceitem.scalequantity
            new TranslationSeedItem("entity.salesinvoiceitem.scalequantity", "en-US", "等级数量_us", "等级数量"),
            // entity.salesinvoiceitem.scalequantity
            new TranslationSeedItem("entity.salesinvoiceitem.scalequantity", "ja-JP", "等级数量_jp", "等级数量"),
            // entity.salesinvoiceitem.scalequantity
            new TranslationSeedItem("entity.salesinvoiceitem.scalequantity", "zh-CN", "等级数量", "等级数量"),
            // entity.salesinvoiceitem.scalequantity
            new TranslationSeedItem("entity.salesinvoiceitem.scalequantity", "zh-HK", "等级数量_hk", "等级数量"),

            // entity.salesinvoiceitem.billingquantitysku
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantitysku", "en-US", "库存单位开票数量_us", "库存单位开票数量"),
            // entity.salesinvoiceitem.billingquantitysku
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantitysku", "ja-JP", "库存单位开票数量_jp", "库存单位开票数量"),
            // entity.salesinvoiceitem.billingquantitysku
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantitysku", "zh-CN", "库存单位开票数量", "库存单位开票数量"),
            // entity.salesinvoiceitem.billingquantitysku
            new TranslationSeedItem("entity.salesinvoiceitem.billingquantitysku", "zh-HK", "库存单位开票数量_hk", "库存单位开票数量"),

            // entity.salesinvoiceitem.netweight
            new TranslationSeedItem("entity.salesinvoiceitem.netweight", "en-US", "净重量_us", "净重量"),
            // entity.salesinvoiceitem.netweight
            new TranslationSeedItem("entity.salesinvoiceitem.netweight", "ja-JP", "净重量_jp", "净重量"),
            // entity.salesinvoiceitem.netweight
            new TranslationSeedItem("entity.salesinvoiceitem.netweight", "zh-CN", "净重量", "净重量"),
            // entity.salesinvoiceitem.netweight
            new TranslationSeedItem("entity.salesinvoiceitem.netweight", "zh-HK", "净重量_hk", "净重量"),

            // entity.salesinvoiceitem.grossweight
            new TranslationSeedItem("entity.salesinvoiceitem.grossweight", "en-US", "毛重_us", "毛重"),
            // entity.salesinvoiceitem.grossweight
            new TranslationSeedItem("entity.salesinvoiceitem.grossweight", "ja-JP", "毛重_jp", "毛重"),
            // entity.salesinvoiceitem.grossweight
            new TranslationSeedItem("entity.salesinvoiceitem.grossweight", "zh-CN", "毛重", "毛重"),
            // entity.salesinvoiceitem.grossweight
            new TranslationSeedItem("entity.salesinvoiceitem.grossweight", "zh-HK", "毛重_hk", "毛重"),

            // entity.salesinvoiceitem.weightunit
            new TranslationSeedItem("entity.salesinvoiceitem.weightunit", "en-US", "重量单位_us", "重量单位"),
            // entity.salesinvoiceitem.weightunit
            new TranslationSeedItem("entity.salesinvoiceitem.weightunit", "ja-JP", "重量单位_jp", "重量单位"),
            // entity.salesinvoiceitem.weightunit
            new TranslationSeedItem("entity.salesinvoiceitem.weightunit", "zh-CN", "重量单位", "重量单位"),
            // entity.salesinvoiceitem.weightunit
            new TranslationSeedItem("entity.salesinvoiceitem.weightunit", "zh-HK", "重量单位_hk", "重量单位"),

            // entity.salesinvoiceitem.businessareacode
            new TranslationSeedItem("entity.salesinvoiceitem.businessareacode", "en-US", "业务范围_us", "业务范围"),
            // entity.salesinvoiceitem.businessareacode
            new TranslationSeedItem("entity.salesinvoiceitem.businessareacode", "ja-JP", "业务范围_jp", "业务范围"),
            // entity.salesinvoiceitem.businessareacode
            new TranslationSeedItem("entity.salesinvoiceitem.businessareacode", "zh-CN", "业务范围", "业务范围"),
            // entity.salesinvoiceitem.businessareacode
            new TranslationSeedItem("entity.salesinvoiceitem.businessareacode", "zh-HK", "业务范围_hk", "业务范围"),

            // entity.salesinvoiceitem.pricingdate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingdate", "en-US", "定价日期_us", "定价日期"),
            // entity.salesinvoiceitem.pricingdate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingdate", "ja-JP", "定价日期_jp", "定价日期"),
            // entity.salesinvoiceitem.pricingdate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingdate", "zh-CN", "定价日期", "定价日期"),
            // entity.salesinvoiceitem.pricingdate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingdate", "zh-HK", "定价日期_hk", "定价日期"),

            // entity.salesinvoiceitem.servicerendereddate
            new TranslationSeedItem("entity.salesinvoiceitem.servicerendereddate", "en-US", "提供服务日期_us", "提供服务日期"),
            // entity.salesinvoiceitem.servicerendereddate
            new TranslationSeedItem("entity.salesinvoiceitem.servicerendereddate", "ja-JP", "提供服务日期_jp", "提供服务日期"),
            // entity.salesinvoiceitem.servicerendereddate
            new TranslationSeedItem("entity.salesinvoiceitem.servicerendereddate", "zh-CN", "提供服务日期", "提供服务日期"),
            // entity.salesinvoiceitem.servicerendereddate
            new TranslationSeedItem("entity.salesinvoiceitem.servicerendereddate", "zh-HK", "提供服务日期_hk", "提供服务日期"),

            // entity.salesinvoiceitem.pricingexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingexchangerate", "en-US", "汇率_us", "汇率"),
            // entity.salesinvoiceitem.pricingexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingexchangerate", "ja-JP", "汇率_jp", "汇率"),
            // entity.salesinvoiceitem.pricingexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingexchangerate", "zh-CN", "汇率", "汇率"),
            // entity.salesinvoiceitem.pricingexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.pricingexchangerate", "zh-HK", "汇率_hk", "汇率"),

            // entity.salesinvoiceitem.netamount
            new TranslationSeedItem("entity.salesinvoiceitem.netamount", "en-US", "净价值_us", "净价值"),
            // entity.salesinvoiceitem.netamount
            new TranslationSeedItem("entity.salesinvoiceitem.netamount", "ja-JP", "净价值_jp", "净价值"),
            // entity.salesinvoiceitem.netamount
            new TranslationSeedItem("entity.salesinvoiceitem.netamount", "zh-CN", "净价值", "净价值"),
            // entity.salesinvoiceitem.netamount
            new TranslationSeedItem("entity.salesinvoiceitem.netamount", "zh-HK", "净价值_hk", "净价值"),

            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "en-US", "参考凭证_us", "参考凭证"),
            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "ja-JP", "参考凭证_jp", "参考凭证"),
            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "zh-CN", "参考凭证", "参考凭证"),
            // entity.salesinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcode", "zh-HK", "参考凭证_hk", "参考凭证"),

            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "en-US", "参考项目_us", "参考项目"),
            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "ja-JP", "参考项目_jp", "参考项目"),
            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "zh-CN", "参考项目", "参考项目"),
            // entity.salesinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentitem", "zh-HK", "参考项目_hk", "参考项目"),

            // entity.salesinvoiceitem.referencedocumentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcategory", "en-US", "先前凭证类别_us", "先前凭证类别"),
            // entity.salesinvoiceitem.referencedocumentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcategory", "ja-JP", "先前凭证类别_jp", "先前凭证类别"),
            // entity.salesinvoiceitem.referencedocumentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcategory", "zh-CN", "先前凭证类别", "先前凭证类别"),
            // entity.salesinvoiceitem.referencedocumentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.referencedocumentcategory", "zh-HK", "先前凭证类别_hk", "先前凭证类别"),

            // entity.salesinvoiceitem.salesdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentcode", "en-US", "销售凭证_us", "销售凭证"),
            // entity.salesinvoiceitem.salesdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentcode", "ja-JP", "销售凭证_jp", "销售凭证"),
            // entity.salesinvoiceitem.salesdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentcode", "zh-CN", "销售凭证", "销售凭证"),
            // entity.salesinvoiceitem.salesdocumentcode
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentcode", "zh-HK", "销售凭证_hk", "销售凭证"),

            // entity.salesinvoiceitem.salesdocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentitem", "en-US", "销售凭证项目_us", "销售凭证项目"),
            // entity.salesinvoiceitem.salesdocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentitem", "ja-JP", "销售凭证项目_jp", "销售凭证项目"),
            // entity.salesinvoiceitem.salesdocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentitem", "zh-CN", "销售凭证项目", "销售凭证项目"),
            // entity.salesinvoiceitem.salesdocumentitem
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentitem", "zh-HK", "销售凭证项目_hk", "销售凭证项目"),

            // entity.salesinvoiceitem.salesdocumentreferenceflag
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentreferenceflag", "en-US", "销售凭证参考_us", "销售凭证参考"),
            // entity.salesinvoiceitem.salesdocumentreferenceflag
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentreferenceflag", "ja-JP", "销售凭证参考_jp", "销售凭证参考"),
            // entity.salesinvoiceitem.salesdocumentreferenceflag
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentreferenceflag", "zh-CN", "销售凭证参考", "销售凭证参考"),
            // entity.salesinvoiceitem.salesdocumentreferenceflag
            new TranslationSeedItem("entity.salesinvoiceitem.salesdocumentreferenceflag", "zh-HK", "销售凭证参考_hk", "销售凭证参考"),

            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "en-US", "物料_us", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "ja-JP", "物料_jp", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "zh-CN", "物料", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesinvoiceitem.materialcode
            new TranslationSeedItem("entity.salesinvoiceitem.materialcode", "zh-HK", "物料_hk", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.salesinvoiceitem.materialdescription
            new TranslationSeedItem("entity.salesinvoiceitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesinvoiceitem.materialdescription
            new TranslationSeedItem("entity.salesinvoiceitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesinvoiceitem.materialdescription
            new TranslationSeedItem("entity.salesinvoiceitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesinvoiceitem.materialdescription
            new TranslationSeedItem("entity.salesinvoiceitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.salesinvoiceitem.pricingreferencematerialcode
            new TranslationSeedItem("entity.salesinvoiceitem.pricingreferencematerialcode", "en-US", "定价参考物料_us", "定价参考物料"),
            // entity.salesinvoiceitem.pricingreferencematerialcode
            new TranslationSeedItem("entity.salesinvoiceitem.pricingreferencematerialcode", "ja-JP", "定价参考物料_jp", "定价参考物料"),
            // entity.salesinvoiceitem.pricingreferencematerialcode
            new TranslationSeedItem("entity.salesinvoiceitem.pricingreferencematerialcode", "zh-CN", "定价参考物料", "定价参考物料"),
            // entity.salesinvoiceitem.pricingreferencematerialcode
            new TranslationSeedItem("entity.salesinvoiceitem.pricingreferencematerialcode", "zh-HK", "定价参考物料_hk", "定价参考物料"),

            // entity.salesinvoiceitem.batchcode
            new TranslationSeedItem("entity.salesinvoiceitem.batchcode", "en-US", "批次_us", "批次"),
            // entity.salesinvoiceitem.batchcode
            new TranslationSeedItem("entity.salesinvoiceitem.batchcode", "ja-JP", "批次_jp", "批次"),
            // entity.salesinvoiceitem.batchcode
            new TranslationSeedItem("entity.salesinvoiceitem.batchcode", "zh-CN", "批次", "批次"),
            // entity.salesinvoiceitem.batchcode
            new TranslationSeedItem("entity.salesinvoiceitem.batchcode", "zh-HK", "批次_hk", "批次"),

            // entity.salesinvoiceitem.materialgroup
            new TranslationSeedItem("entity.salesinvoiceitem.materialgroup", "en-US", "物料组_us", "物料组"),
            // entity.salesinvoiceitem.materialgroup
            new TranslationSeedItem("entity.salesinvoiceitem.materialgroup", "ja-JP", "物料组_jp", "物料组"),
            // entity.salesinvoiceitem.materialgroup
            new TranslationSeedItem("entity.salesinvoiceitem.materialgroup", "zh-CN", "物料组", "物料组"),
            // entity.salesinvoiceitem.materialgroup
            new TranslationSeedItem("entity.salesinvoiceitem.materialgroup", "zh-HK", "物料组_hk", "物料组"),

            // entity.salesinvoiceitem.salesitemcategory
            new TranslationSeedItem("entity.salesinvoiceitem.salesitemcategory", "en-US", "项目类别_us", "项目类别"),
            // entity.salesinvoiceitem.salesitemcategory
            new TranslationSeedItem("entity.salesinvoiceitem.salesitemcategory", "ja-JP", "项目类别_jp", "项目类别"),
            // entity.salesinvoiceitem.salesitemcategory
            new TranslationSeedItem("entity.salesinvoiceitem.salesitemcategory", "zh-CN", "项目类别", "项目类别"),
            // entity.salesinvoiceitem.salesitemcategory
            new TranslationSeedItem("entity.salesinvoiceitem.salesitemcategory", "zh-HK", "项目类别_hk", "项目类别"),

            // entity.salesinvoiceitem.producthierarchy
            new TranslationSeedItem("entity.salesinvoiceitem.producthierarchy", "en-US", "产品层次_us", "产品层次（最长 18，故 Length=18）"),
            // entity.salesinvoiceitem.producthierarchy
            new TranslationSeedItem("entity.salesinvoiceitem.producthierarchy", "ja-JP", "产品层次_jp", "产品层次（最长 18，故 Length=18）"),
            // entity.salesinvoiceitem.producthierarchy
            new TranslationSeedItem("entity.salesinvoiceitem.producthierarchy", "zh-CN", "产品层次", "产品层次（最长 18，故 Length=18）"),
            // entity.salesinvoiceitem.producthierarchy
            new TranslationSeedItem("entity.salesinvoiceitem.producthierarchy", "zh-HK", "产品层次_hk", "产品层次（最长 18，故 Length=18）"),

            // entity.salesinvoiceitem.shippingpoint
            new TranslationSeedItem("entity.salesinvoiceitem.shippingpoint", "en-US", "装运点/接收点_us", "装运点/接收点"),
            // entity.salesinvoiceitem.shippingpoint
            new TranslationSeedItem("entity.salesinvoiceitem.shippingpoint", "ja-JP", "装运点/接收点_jp", "装运点/接收点"),
            // entity.salesinvoiceitem.shippingpoint
            new TranslationSeedItem("entity.salesinvoiceitem.shippingpoint", "zh-CN", "装运点/接收点", "装运点/接收点"),
            // entity.salesinvoiceitem.shippingpoint
            new TranslationSeedItem("entity.salesinvoiceitem.shippingpoint", "zh-HK", "装运点/接收点_hk", "装运点/接收点"),

            // entity.salesinvoiceitem.division
            new TranslationSeedItem("entity.salesinvoiceitem.division", "en-US", "产品组_us", "产品组"),
            // entity.salesinvoiceitem.division
            new TranslationSeedItem("entity.salesinvoiceitem.division", "ja-JP", "产品组_jp", "产品组"),
            // entity.salesinvoiceitem.division
            new TranslationSeedItem("entity.salesinvoiceitem.division", "zh-CN", "产品组", "产品组"),
            // entity.salesinvoiceitem.division
            new TranslationSeedItem("entity.salesinvoiceitem.division", "zh-HK", "产品组_hk", "产品组"),

            // entity.salesinvoiceitem.partneritem
            new TranslationSeedItem("entity.salesinvoiceitem.partneritem", "en-US", "合作伙伴项目_us", "合作伙伴项目"),
            // entity.salesinvoiceitem.partneritem
            new TranslationSeedItem("entity.salesinvoiceitem.partneritem", "ja-JP", "合作伙伴项目_jp", "合作伙伴项目"),
            // entity.salesinvoiceitem.partneritem
            new TranslationSeedItem("entity.salesinvoiceitem.partneritem", "zh-CN", "合作伙伴项目", "合作伙伴项目"),
            // entity.salesinvoiceitem.partneritem
            new TranslationSeedItem("entity.salesinvoiceitem.partneritem", "zh-HK", "合作伙伴项目_hk", "合作伙伴项目"),

            // entity.salesinvoiceitem.departurecountry
            new TranslationSeedItem("entity.salesinvoiceitem.departurecountry", "en-US", "国家_us", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoiceitem.departurecountry
            new TranslationSeedItem("entity.salesinvoiceitem.departurecountry", "ja-JP", "国家_jp", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoiceitem.departurecountry
            new TranslationSeedItem("entity.salesinvoiceitem.departurecountry", "zh-CN", "国家", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoiceitem.departurecountry
            new TranslationSeedItem("entity.salesinvoiceitem.departurecountry", "zh-HK", "国家_hk", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.salesinvoiceitem.plantregion
            new TranslationSeedItem("entity.salesinvoiceitem.plantregion", "en-US", "交货工厂地区_us", "交货工厂地区"),
            // entity.salesinvoiceitem.plantregion
            new TranslationSeedItem("entity.salesinvoiceitem.plantregion", "ja-JP", "交货工厂地区_jp", "交货工厂地区"),
            // entity.salesinvoiceitem.plantregion
            new TranslationSeedItem("entity.salesinvoiceitem.plantregion", "zh-CN", "交货工厂地区", "交货工厂地区"),
            // entity.salesinvoiceitem.plantregion
            new TranslationSeedItem("entity.salesinvoiceitem.plantregion", "zh-HK", "交货工厂地区_hk", "交货工厂地区"),

            // entity.salesinvoiceitem.pricingflag
            new TranslationSeedItem("entity.salesinvoiceitem.pricingflag", "en-US", "定价_us", "定价"),
            // entity.salesinvoiceitem.pricingflag
            new TranslationSeedItem("entity.salesinvoiceitem.pricingflag", "ja-JP", "定价_jp", "定价"),
            // entity.salesinvoiceitem.pricingflag
            new TranslationSeedItem("entity.salesinvoiceitem.pricingflag", "zh-CN", "定价", "定价"),
            // entity.salesinvoiceitem.pricingflag
            new TranslationSeedItem("entity.salesinvoiceitem.pricingflag", "zh-HK", "定价_hk", "定价"),

            // entity.salesinvoiceitem.warehousecode
            new TranslationSeedItem("entity.salesinvoiceitem.warehousecode", "en-US", "库存地点_us", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.salesinvoiceitem.warehousecode
            new TranslationSeedItem("entity.salesinvoiceitem.warehousecode", "ja-JP", "库存地点_jp", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.salesinvoiceitem.warehousecode
            new TranslationSeedItem("entity.salesinvoiceitem.warehousecode", "zh-CN", "库存地点", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.salesinvoiceitem.warehousecode
            new TranslationSeedItem("entity.salesinvoiceitem.warehousecode", "zh-HK", "库存地点_hk", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),

            // entity.salesinvoiceitem.costamount
            new TranslationSeedItem("entity.salesinvoiceitem.costamount", "en-US", "成本_us", "成本"),
            // entity.salesinvoiceitem.costamount
            new TranslationSeedItem("entity.salesinvoiceitem.costamount", "ja-JP", "成本_jp", "成本"),
            // entity.salesinvoiceitem.costamount
            new TranslationSeedItem("entity.salesinvoiceitem.costamount", "zh-CN", "成本", "成本"),
            // entity.salesinvoiceitem.costamount
            new TranslationSeedItem("entity.salesinvoiceitem.costamount", "zh-HK", "成本_hk", "成本"),

            // entity.salesinvoiceitem.subtotal1
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal1", "en-US", "小计1_us", "小计1"),
            // entity.salesinvoiceitem.subtotal1
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal1", "ja-JP", "小计1_jp", "小计1"),
            // entity.salesinvoiceitem.subtotal1
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal1", "zh-CN", "小计1", "小计1"),
            // entity.salesinvoiceitem.subtotal1
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal1", "zh-HK", "小计1_hk", "小计1"),

            // entity.salesinvoiceitem.subtotal2
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal2", "en-US", "小计2_us", "小计2"),
            // entity.salesinvoiceitem.subtotal2
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal2", "ja-JP", "小计2_jp", "小计2"),
            // entity.salesinvoiceitem.subtotal2
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal2", "zh-CN", "小计2", "小计2"),
            // entity.salesinvoiceitem.subtotal2
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal2", "zh-HK", "小计2_hk", "小计2"),

            // entity.salesinvoiceitem.subtotal3
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal3", "en-US", "小计3_us", "小计3"),
            // entity.salesinvoiceitem.subtotal3
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal3", "ja-JP", "小计3_jp", "小计3"),
            // entity.salesinvoiceitem.subtotal3
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal3", "zh-CN", "小计3", "小计3"),
            // entity.salesinvoiceitem.subtotal3
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal3", "zh-HK", "小计3_hk", "小计3"),

            // entity.salesinvoiceitem.subtotal4
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal4", "en-US", "小计4_us", "小计4"),
            // entity.salesinvoiceitem.subtotal4
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal4", "ja-JP", "小计4_jp", "小计4"),
            // entity.salesinvoiceitem.subtotal4
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal4", "zh-CN", "小计4", "小计4"),
            // entity.salesinvoiceitem.subtotal4
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal4", "zh-HK", "小计4_hk", "小计4"),

            // entity.salesinvoiceitem.subtotal5
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal5", "en-US", "小计5_us", "小计5"),
            // entity.salesinvoiceitem.subtotal5
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal5", "ja-JP", "小计5_jp", "小计5"),
            // entity.salesinvoiceitem.subtotal5
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal5", "zh-CN", "小计5", "小计5"),
            // entity.salesinvoiceitem.subtotal5
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal5", "zh-HK", "小计5_hk", "小计5"),

            // entity.salesinvoiceitem.subtotal6
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal6", "en-US", "小计6_us", "小计6"),
            // entity.salesinvoiceitem.subtotal6
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal6", "ja-JP", "小计6_jp", "小计6"),
            // entity.salesinvoiceitem.subtotal6
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal6", "zh-CN", "小计6", "小计6"),
            // entity.salesinvoiceitem.subtotal6
            new TranslationSeedItem("entity.salesinvoiceitem.subtotal6", "zh-HK", "小计6_hk", "小计6"),

            // entity.salesinvoiceitem.statisticsexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.statisticsexchangerate", "en-US", "汇率统计_us", "汇率统计"),
            // entity.salesinvoiceitem.statisticsexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.statisticsexchangerate", "ja-JP", "汇率统计_jp", "汇率统计"),
            // entity.salesinvoiceitem.statisticsexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.statisticsexchangerate", "zh-CN", "汇率统计", "汇率统计"),
            // entity.salesinvoiceitem.statisticsexchangerate
            new TranslationSeedItem("entity.salesinvoiceitem.statisticsexchangerate", "zh-HK", "汇率统计_hk", "汇率统计"),

            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesinvoiceitem.profitcentercode
            new TranslationSeedItem("entity.salesinvoiceitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.salesinvoiceitem.creditprice
            new TranslationSeedItem("entity.salesinvoiceitem.creditprice", "en-US", "信贷价格_us", "信贷价格"),
            // entity.salesinvoiceitem.creditprice
            new TranslationSeedItem("entity.salesinvoiceitem.creditprice", "ja-JP", "信贷价格_jp", "信贷价格"),
            // entity.salesinvoiceitem.creditprice
            new TranslationSeedItem("entity.salesinvoiceitem.creditprice", "zh-CN", "信贷价格", "信贷价格"),
            // entity.salesinvoiceitem.creditprice
            new TranslationSeedItem("entity.salesinvoiceitem.creditprice", "zh-HK", "信贷价格_hk", "信贷价格"),

            // entity.salesinvoiceitem.customergroupsalesorder
            new TranslationSeedItem("entity.salesinvoiceitem.customergroupsalesorder", "en-US", "客户组销售订单_us", "客户组销售订单"),
            // entity.salesinvoiceitem.customergroupsalesorder
            new TranslationSeedItem("entity.salesinvoiceitem.customergroupsalesorder", "ja-JP", "客户组销售订单_jp", "客户组销售订单"),
            // entity.salesinvoiceitem.customergroupsalesorder
            new TranslationSeedItem("entity.salesinvoiceitem.customergroupsalesorder", "zh-CN", "客户组销售订单", "客户组销售订单"),
            // entity.salesinvoiceitem.customergroupsalesorder
            new TranslationSeedItem("entity.salesinvoiceitem.customergroupsalesorder", "zh-HK", "客户组销售订单_hk", "客户组销售订单"),

            // entity.salesinvoiceitem.destinationcountryorder
            new TranslationSeedItem("entity.salesinvoiceitem.destinationcountryorder", "en-US", "订单目的地国家_us", "订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoiceitem.destinationcountryorder
            new TranslationSeedItem("entity.salesinvoiceitem.destinationcountryorder", "ja-JP", "订单目的地国家_jp", "订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoiceitem.destinationcountryorder
            new TranslationSeedItem("entity.salesinvoiceitem.destinationcountryorder", "zh-CN", "订单目的地国家", "订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoiceitem.destinationcountryorder
            new TranslationSeedItem("entity.salesinvoiceitem.destinationcountryorder", "zh-HK", "订单目的地国家_hk", "订单目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.salesinvoiceitem.regionorder
            new TranslationSeedItem("entity.salesinvoiceitem.regionorder", "en-US", "地区订单_us", "地区订单"),
            // entity.salesinvoiceitem.regionorder
            new TranslationSeedItem("entity.salesinvoiceitem.regionorder", "ja-JP", "地区订单_jp", "地区订单"),
            // entity.salesinvoiceitem.regionorder
            new TranslationSeedItem("entity.salesinvoiceitem.regionorder", "zh-CN", "地区订单", "地区订单"),
            // entity.salesinvoiceitem.regionorder
            new TranslationSeedItem("entity.salesinvoiceitem.regionorder", "zh-HK", "地区订单_hk", "地区订单"),

            // entity.salesinvoiceitem.salesorganizationorder
            new TranslationSeedItem("entity.salesinvoiceitem.salesorganizationorder", "en-US", "订单的销售机构_us", "订单的销售机构"),
            // entity.salesinvoiceitem.salesorganizationorder
            new TranslationSeedItem("entity.salesinvoiceitem.salesorganizationorder", "ja-JP", "订单的销售机构_jp", "订单的销售机构"),
            // entity.salesinvoiceitem.salesorganizationorder
            new TranslationSeedItem("entity.salesinvoiceitem.salesorganizationorder", "zh-CN", "订单的销售机构", "订单的销售机构"),
            // entity.salesinvoiceitem.salesorganizationorder
            new TranslationSeedItem("entity.salesinvoiceitem.salesorganizationorder", "zh-HK", "订单的销售机构_hk", "订单的销售机构"),

            // entity.salesinvoiceitem.distributionchannelorder
            new TranslationSeedItem("entity.salesinvoiceitem.distributionchannelorder", "en-US", "订单分销渠道_us", "订单分销渠道"),
            // entity.salesinvoiceitem.distributionchannelorder
            new TranslationSeedItem("entity.salesinvoiceitem.distributionchannelorder", "ja-JP", "订单分销渠道_jp", "订单分销渠道"),
            // entity.salesinvoiceitem.distributionchannelorder
            new TranslationSeedItem("entity.salesinvoiceitem.distributionchannelorder", "zh-CN", "订单分销渠道", "订单分销渠道"),
            // entity.salesinvoiceitem.distributionchannelorder
            new TranslationSeedItem("entity.salesinvoiceitem.distributionchannelorder", "zh-HK", "订单分销渠道_hk", "订单分销渠道"),

            // entity.salesinvoiceitem.documentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.documentcategory", "en-US", "SD 凭证类别_us", "SD 凭证类别"),
            // entity.salesinvoiceitem.documentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.documentcategory", "ja-JP", "SD 凭证类别_jp", "SD 凭证类别"),
            // entity.salesinvoiceitem.documentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.documentcategory", "zh-CN", "SD 凭证类别", "SD 凭证类别"),
            // entity.salesinvoiceitem.documentcategory
            new TranslationSeedItem("entity.salesinvoiceitem.documentcategory", "zh-HK", "SD 凭证类别_hk", "SD 凭证类别"),

            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "en-US", "税额_us", "税额"),
            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "ja-JP", "税额_jp", "税额"),
            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "zh-CN", "税额", "税额"),
            // entity.salesinvoiceitem.taxamount
            new TranslationSeedItem("entity.salesinvoiceitem.taxamount", "zh-HK", "税额_hk", "税额"),

            // entity.salesinvoiceitem.grossamount
            new TranslationSeedItem("entity.salesinvoiceitem.grossamount", "en-US", "总值_us", "总值"),
            // entity.salesinvoiceitem.grossamount
            new TranslationSeedItem("entity.salesinvoiceitem.grossamount", "ja-JP", "总值_jp", "总值"),
            // entity.salesinvoiceitem.grossamount
            new TranslationSeedItem("entity.salesinvoiceitem.grossamount", "zh-CN", "总值", "总值"),
            // entity.salesinvoiceitem.grossamount
            new TranslationSeedItem("entity.salesinvoiceitem.grossamount", "zh-HK", "总值_hk", "总值"),

            // entity.salesinvoiceitem.exchangeratedate
            new TranslationSeedItem("entity.salesinvoiceitem.exchangeratedate", "en-US", "换算日期_us", "换算日期"),
            // entity.salesinvoiceitem.exchangeratedate
            new TranslationSeedItem("entity.salesinvoiceitem.exchangeratedate", "ja-JP", "换算日期_jp", "换算日期"),
            // entity.salesinvoiceitem.exchangeratedate
            new TranslationSeedItem("entity.salesinvoiceitem.exchangeratedate", "zh-CN", "换算日期", "换算日期"),
            // entity.salesinvoiceitem.exchangeratedate
            new TranslationSeedItem("entity.salesinvoiceitem.exchangeratedate", "zh-HK", "换算日期_hk", "换算日期"),

            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesinvoiceitem.isobsolete
            new TranslationSeedItem("entity.salesinvoiceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "en-US", "销售发票主表_us", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "ja-JP", "销售发票主表_jp", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "zh-CN", "销售发票主表", "销售发票主表"),
            // entity.salesinvoiceitem.salesinvoice
            new TranslationSeedItem("entity.salesinvoiceitem.salesinvoice", "zh-HK", "销售发票主表_hk", "销售发票主表"),
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
