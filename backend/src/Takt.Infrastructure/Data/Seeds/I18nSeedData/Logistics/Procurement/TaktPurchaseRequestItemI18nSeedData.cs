// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseRequestItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseRequestItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseRequestItem 实体国际化翻译种子（键前缀 entity.purchaserequestitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseRequestItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseRequestItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaserequestitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseRequestItemTranslations())
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

        TaktLogger.Information("TaktPurchaseRequestItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseRequestItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaserequestitem._self / entity.purchaserequestitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseRequestItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaserequestitem._self
            new TranslationSeedItem("entity.purchaserequestitem._self", "en-US", "Purchase Request Item Information_us", "实体名称"),
            // entity.purchaserequestitem._self
            new TranslationSeedItem("entity.purchaserequestitem._self", "ja-JP", "Takt采购申请明细信息_jp", "实体名称"),
            // entity.purchaserequestitem._self
            new TranslationSeedItem("entity.purchaserequestitem._self", "zh-CN", "Takt采购申请明细信息", "实体名称"),
            // entity.purchaserequestitem._self
            new TranslationSeedItem("entity.purchaserequestitem._self", "zh-HK", "Takt采购申请明细信息_hk", "实体名称"),

            // entity.purchaserequestitem.purchaserequestid
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestid", "en-US", "采购申请ID_us", "采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),
            // entity.purchaserequestitem.purchaserequestid
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestid", "ja-JP", "采购申请ID_jp", "采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),
            // entity.purchaserequestitem.purchaserequestid
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestid", "zh-CN", "采购申请ID", "采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),
            // entity.purchaserequestitem.purchaserequestid
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestid", "zh-HK", "采购申请ID_hk", "采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),

            // entity.purchaserequestitem.purchaserequestcode
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestcode", "en-US", "采购申请编码_us", "采购申请编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaserequestitem.purchaserequestcode
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestcode", "ja-JP", "采购申请编码_jp", "采购申请编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaserequestitem.purchaserequestcode
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestcode", "zh-CN", "采购申请编码", "采购申请编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaserequestitem.purchaserequestcode
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestcode", "zh-HK", "采购申请编码_hk", "采购申请编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.purchaserequestitem.purchaseplanitemid
            new TranslationSeedItem("entity.purchaserequestitem.purchaseplanitemid", "en-US", "来源采购计划明细ID_us", "来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）"),
            // entity.purchaserequestitem.purchaseplanitemid
            new TranslationSeedItem("entity.purchaserequestitem.purchaseplanitemid", "ja-JP", "来源采购计划明细ID_jp", "来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）"),
            // entity.purchaserequestitem.purchaseplanitemid
            new TranslationSeedItem("entity.purchaserequestitem.purchaseplanitemid", "zh-CN", "来源采购计划明细ID", "来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）"),
            // entity.purchaserequestitem.purchaseplanitemid
            new TranslationSeedItem("entity.purchaserequestitem.purchaseplanitemid", "zh-HK", "来源采购计划明细ID_hk", "来源采购计划明细 ID（MRP 追溯，关联 TaktPurchasePlanItem.Id）"),

            // entity.purchaserequestitem.linenumber
            new TranslationSeedItem("entity.purchaserequestitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.purchaserequestitem.linenumber
            new TranslationSeedItem("entity.purchaserequestitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.purchaserequestitem.linenumber
            new TranslationSeedItem("entity.purchaserequestitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.purchaserequestitem.linenumber
            new TranslationSeedItem("entity.purchaserequestitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.purchaserequestitem.allocationcategory
            new TranslationSeedItem("entity.purchaserequestitem.allocationcategory", "en-US", "分配类别_us", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),
            // entity.purchaserequestitem.allocationcategory
            new TranslationSeedItem("entity.purchaserequestitem.allocationcategory", "ja-JP", "分配类别_jp", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),
            // entity.purchaserequestitem.allocationcategory
            new TranslationSeedItem("entity.purchaserequestitem.allocationcategory", "zh-CN", "分配类别", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),
            // entity.purchaserequestitem.allocationcategory
            new TranslationSeedItem("entity.purchaserequestitem.allocationcategory", "zh-HK", "分配类别_hk", "分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）"),

            // entity.purchaserequestitem.materialcode
            new TranslationSeedItem("entity.purchaserequestitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaserequestitem.materialcode
            new TranslationSeedItem("entity.purchaserequestitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaserequestitem.materialcode
            new TranslationSeedItem("entity.purchaserequestitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaserequestitem.materialcode
            new TranslationSeedItem("entity.purchaserequestitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.purchaserequestitem.materialdescription
            new TranslationSeedItem("entity.purchaserequestitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaserequestitem.materialdescription
            new TranslationSeedItem("entity.purchaserequestitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaserequestitem.materialdescription
            new TranslationSeedItem("entity.purchaserequestitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.purchaserequestitem.materialdescription
            new TranslationSeedItem("entity.purchaserequestitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.purchaserequestitem.materialspecification
            new TranslationSeedItem("entity.purchaserequestitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.purchaserequestitem.materialspecification
            new TranslationSeedItem("entity.purchaserequestitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.purchaserequestitem.materialspecification
            new TranslationSeedItem("entity.purchaserequestitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.purchaserequestitem.materialspecification
            new TranslationSeedItem("entity.purchaserequestitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.purchaserequestitem.requestunit
            new TranslationSeedItem("entity.purchaserequestitem.requestunit", "en-US", "申请单位_us", "申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaserequestitem.requestunit
            new TranslationSeedItem("entity.purchaserequestitem.requestunit", "ja-JP", "申请单位_jp", "申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaserequestitem.requestunit
            new TranslationSeedItem("entity.purchaserequestitem.requestunit", "zh-CN", "申请单位", "申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.purchaserequestitem.requestunit
            new TranslationSeedItem("entity.purchaserequestitem.requestunit", "zh-HK", "申请单位_hk", "申请单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.purchaserequestitem.requestquantity
            new TranslationSeedItem("entity.purchaserequestitem.requestquantity", "en-US", "申请数量_us", "申请数量（基本单位数量）"),
            // entity.purchaserequestitem.requestquantity
            new TranslationSeedItem("entity.purchaserequestitem.requestquantity", "ja-JP", "申请数量_jp", "申请数量（基本单位数量）"),
            // entity.purchaserequestitem.requestquantity
            new TranslationSeedItem("entity.purchaserequestitem.requestquantity", "zh-CN", "申请数量", "申请数量（基本单位数量）"),
            // entity.purchaserequestitem.requestquantity
            new TranslationSeedItem("entity.purchaserequestitem.requestquantity", "zh-HK", "申请数量_hk", "申请数量（基本单位数量）"),

            // entity.purchaserequestitem.convertedquantity
            new TranslationSeedItem("entity.purchaserequestitem.convertedquantity", "en-US", "已转订单数量_us", "已转订单数量（基本单位数量）"),
            // entity.purchaserequestitem.convertedquantity
            new TranslationSeedItem("entity.purchaserequestitem.convertedquantity", "ja-JP", "已转订单数量_jp", "已转订单数量（基本单位数量）"),
            // entity.purchaserequestitem.convertedquantity
            new TranslationSeedItem("entity.purchaserequestitem.convertedquantity", "zh-CN", "已转订单数量", "已转订单数量（基本单位数量）"),
            // entity.purchaserequestitem.convertedquantity
            new TranslationSeedItem("entity.purchaserequestitem.convertedquantity", "zh-HK", "已转订单数量_hk", "已转订单数量（基本单位数量）"),

            // entity.purchaserequestitem.purchaseperunit
            new TranslationSeedItem("entity.purchaserequestitem.purchaseperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaserequestitem.purchaseperunit
            new TranslationSeedItem("entity.purchaserequestitem.purchaseperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaserequestitem.purchaseperunit
            new TranslationSeedItem("entity.purchaserequestitem.purchaseperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),
            // entity.purchaserequestitem.purchaseperunit
            new TranslationSeedItem("entity.purchaserequestitem.purchaseperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_materials_price_unit_param：1/100/1000/10000；默认 1000）"),

            // entity.purchaserequestitem.purchaserequestunitprice
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestunitprice", "en-US", "请购单价_us", "请购单价"),
            // entity.purchaserequestitem.purchaserequestunitprice
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestunitprice", "ja-JP", "请购单价_jp", "请购单价"),
            // entity.purchaserequestitem.purchaserequestunitprice
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestunitprice", "zh-CN", "请购单价", "请购单价"),
            // entity.purchaserequestitem.purchaserequestunitprice
            new TranslationSeedItem("entity.purchaserequestitem.purchaserequestunitprice", "zh-HK", "请购单价_hk", "请购单价"),

            // entity.purchaserequestitem.taxcode
            new TranslationSeedItem("entity.purchaserequestitem.taxcode", "en-US", "税码_us", "税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.purchaserequestitem.taxcode
            new TranslationSeedItem("entity.purchaserequestitem.taxcode", "ja-JP", "税码_jp", "税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.purchaserequestitem.taxcode
            new TranslationSeedItem("entity.purchaserequestitem.taxcode", "zh-CN", "税码", "税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.purchaserequestitem.taxcode
            new TranslationSeedItem("entity.purchaserequestitem.taxcode", "zh-HK", "税码_hk", "税码（冗余：按 PurchaseRequestId 取 TaktPurchaseRequest.TaxCode 联动；字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),

            // entity.purchaserequestitem.taxincludedamount
            new TranslationSeedItem("entity.purchaserequestitem.taxincludedamount", "en-US", "含税金额_us", "含税金额"),
            // entity.purchaserequestitem.taxincludedamount
            new TranslationSeedItem("entity.purchaserequestitem.taxincludedamount", "ja-JP", "含税金额_jp", "含税金额"),
            // entity.purchaserequestitem.taxincludedamount
            new TranslationSeedItem("entity.purchaserequestitem.taxincludedamount", "zh-CN", "含税金额", "含税金额"),
            // entity.purchaserequestitem.taxincludedamount
            new TranslationSeedItem("entity.purchaserequestitem.taxincludedamount", "zh-HK", "含税金额_hk", "含税金额"),

            // entity.purchaserequestitem.untaxedamount
            new TranslationSeedItem("entity.purchaserequestitem.untaxedamount", "en-US", "未税金额_us", "未税金额"),
            // entity.purchaserequestitem.untaxedamount
            new TranslationSeedItem("entity.purchaserequestitem.untaxedamount", "ja-JP", "未税金额_jp", "未税金额"),
            // entity.purchaserequestitem.untaxedamount
            new TranslationSeedItem("entity.purchaserequestitem.untaxedamount", "zh-CN", "未税金额", "未税金额"),
            // entity.purchaserequestitem.untaxedamount
            new TranslationSeedItem("entity.purchaserequestitem.untaxedamount", "zh-HK", "未税金额_hk", "未税金额"),

            // entity.purchaserequestitem.taxamount
            new TranslationSeedItem("entity.purchaserequestitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.purchaserequestitem.taxamount
            new TranslationSeedItem("entity.purchaserequestitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.purchaserequestitem.taxamount
            new TranslationSeedItem("entity.purchaserequestitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.purchaserequestitem.taxamount
            new TranslationSeedItem("entity.purchaserequestitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.purchaserequestitem.requestamount
            new TranslationSeedItem("entity.purchaserequestitem.requestamount", "en-US", "请购金额_us", "请购金额"),
            // entity.purchaserequestitem.requestamount
            new TranslationSeedItem("entity.purchaserequestitem.requestamount", "ja-JP", "请购金额_jp", "请购金额"),
            // entity.purchaserequestitem.requestamount
            new TranslationSeedItem("entity.purchaserequestitem.requestamount", "zh-CN", "请购金额", "请购金额"),
            // entity.purchaserequestitem.requestamount
            new TranslationSeedItem("entity.purchaserequestitem.requestamount", "zh-HK", "请购金额_hk", "请购金额"),

            // entity.purchaserequestitem.pricingdate
            new TranslationSeedItem("entity.purchaserequestitem.pricingdate", "en-US", "价格日期_us", "价格日期"),
            // entity.purchaserequestitem.pricingdate
            new TranslationSeedItem("entity.purchaserequestitem.pricingdate", "ja-JP", "价格日期_jp", "价格日期"),
            // entity.purchaserequestitem.pricingdate
            new TranslationSeedItem("entity.purchaserequestitem.pricingdate", "zh-CN", "价格日期", "价格日期"),
            // entity.purchaserequestitem.pricingdate
            new TranslationSeedItem("entity.purchaserequestitem.pricingdate", "zh-HK", "价格日期_hk", "价格日期"),

            // entity.purchaserequestitem.grossweight
            new TranslationSeedItem("entity.purchaserequestitem.grossweight", "en-US", "毛重_us", "毛重"),
            // entity.purchaserequestitem.grossweight
            new TranslationSeedItem("entity.purchaserequestitem.grossweight", "ja-JP", "毛重_jp", "毛重"),
            // entity.purchaserequestitem.grossweight
            new TranslationSeedItem("entity.purchaserequestitem.grossweight", "zh-CN", "毛重", "毛重"),
            // entity.purchaserequestitem.grossweight
            new TranslationSeedItem("entity.purchaserequestitem.grossweight", "zh-HK", "毛重_hk", "毛重"),

            // entity.purchaserequestitem.netweight
            new TranslationSeedItem("entity.purchaserequestitem.netweight", "en-US", "净重_us", "净重"),
            // entity.purchaserequestitem.netweight
            new TranslationSeedItem("entity.purchaserequestitem.netweight", "ja-JP", "净重_jp", "净重"),
            // entity.purchaserequestitem.netweight
            new TranslationSeedItem("entity.purchaserequestitem.netweight", "zh-CN", "净重", "净重"),
            // entity.purchaserequestitem.netweight
            new TranslationSeedItem("entity.purchaserequestitem.netweight", "zh-HK", "净重_hk", "净重"),

            // entity.purchaserequestitem.weightunit
            new TranslationSeedItem("entity.purchaserequestitem.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.purchaserequestitem.weightunit
            new TranslationSeedItem("entity.purchaserequestitem.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.purchaserequestitem.weightunit
            new TranslationSeedItem("entity.purchaserequestitem.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.purchaserequestitem.weightunit
            new TranslationSeedItem("entity.purchaserequestitem.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),

            // entity.purchaserequestitem.volume
            new TranslationSeedItem("entity.purchaserequestitem.volume", "en-US", "体积_us", "体积"),
            // entity.purchaserequestitem.volume
            new TranslationSeedItem("entity.purchaserequestitem.volume", "ja-JP", "体积_jp", "体积"),
            // entity.purchaserequestitem.volume
            new TranslationSeedItem("entity.purchaserequestitem.volume", "zh-CN", "体积", "体积"),
            // entity.purchaserequestitem.volume
            new TranslationSeedItem("entity.purchaserequestitem.volume", "zh-HK", "体积_hk", "体积"),

            // entity.purchaserequestitem.volumeunit
            new TranslationSeedItem("entity.purchaserequestitem.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.purchaserequestitem.volumeunit
            new TranslationSeedItem("entity.purchaserequestitem.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.purchaserequestitem.volumeunit
            new TranslationSeedItem("entity.purchaserequestitem.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.purchaserequestitem.volumeunit
            new TranslationSeedItem("entity.purchaserequestitem.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),

            // entity.purchaserequestitem.profitcentercode
            new TranslationSeedItem("entity.purchaserequestitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.purchaserequestitem.profitcentercode
            new TranslationSeedItem("entity.purchaserequestitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.purchaserequestitem.profitcentercode
            new TranslationSeedItem("entity.purchaserequestitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.purchaserequestitem.profitcentercode
            new TranslationSeedItem("entity.purchaserequestitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.purchaserequestitem.isobsolete
            new TranslationSeedItem("entity.purchaserequestitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaserequestitem.isobsolete
            new TranslationSeedItem("entity.purchaserequestitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaserequestitem.isobsolete
            new TranslationSeedItem("entity.purchaserequestitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaserequestitem.isobsolete
            new TranslationSeedItem("entity.purchaserequestitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
