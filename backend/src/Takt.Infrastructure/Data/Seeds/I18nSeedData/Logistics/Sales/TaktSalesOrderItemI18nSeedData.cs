// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesOrderItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesOrderItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesOrderItem 实体国际化翻译种子（键前缀 entity.salesorderitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesOrderItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesOrderItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesorderitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesOrderItemTranslations())
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

        TaktLogger.Information("TaktSalesOrderItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesOrderItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesorderitem._self / entity.salesorderitem.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesOrderItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "en-US", "Sales Order Item Information_us", "实体名称"),
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "ja-JP", "Takt销售订单明细信息_jp", "实体名称"),
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "zh-CN", "Takt销售订单明细信息", "实体名称"),
            // entity.salesorderitem._self
            new TranslationSeedItem("entity.salesorderitem._self", "zh-HK", "Takt销售订单明细信息_hk", "实体名称"),

            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "en-US", "销售订单ID_us", "销售订单（选项 TaktSalesOrders/options；DictValue=Id）"),
            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "ja-JP", "销售订单ID_jp", "销售订单（选项 TaktSalesOrders/options；DictValue=Id）"),
            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "zh-CN", "销售订单ID", "销售订单（选项 TaktSalesOrders/options；DictValue=Id）"),
            // entity.salesorderitem.salesorderid
            new TranslationSeedItem("entity.salesorderitem.salesorderid", "zh-HK", "销售订单ID_hk", "销售订单（选项 TaktSalesOrders/options；DictValue=Id）"),

            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "en-US", "销售订单编码_us", "销售订单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "ja-JP", "销售订单编码_jp", "销售订单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "zh-CN", "销售订单编码", "销售订单编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesorderitem.salesordercode
            new TranslationSeedItem("entity.salesorderitem.salesordercode", "zh-HK", "销售订单编码_hk", "销售订单编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.salesorderitem.linenumber
            new TranslationSeedItem("entity.salesorderitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.salesorderitem.materialcode
            new TranslationSeedItem("entity.salesorderitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.salesorderitem.materialdescription
            new TranslationSeedItem("entity.salesorderitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesorderitem.materialdescription
            new TranslationSeedItem("entity.salesorderitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesorderitem.materialdescription
            new TranslationSeedItem("entity.salesorderitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.salesorderitem.materialdescription
            new TranslationSeedItem("entity.salesorderitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.salesorderitem.materialspecification
            new TranslationSeedItem("entity.salesorderitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "en-US", "销售单位_us", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "ja-JP", "销售单位_jp", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "zh-CN", "销售单位", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.salesorderitem.salesunit
            new TranslationSeedItem("entity.salesorderitem.salesunit", "zh-HK", "销售单位_hk", "销售单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "en-US", "订购数量_us", "订购数量（基本单位数量）"),
            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "ja-JP", "订购数量_jp", "订购数量（基本单位数量）"),
            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "zh-CN", "订购数量", "订购数量（基本单位数量）"),
            // entity.salesorderitem.orderquantity
            new TranslationSeedItem("entity.salesorderitem.orderquantity", "zh-HK", "订购数量_hk", "订购数量（基本单位数量）"),

            // entity.salesorderitem.targetquantity
            new TranslationSeedItem("entity.salesorderitem.targetquantity", "en-US", "目标数量_us", "目标数量（基本单位数量）"),
            // entity.salesorderitem.targetquantity
            new TranslationSeedItem("entity.salesorderitem.targetquantity", "ja-JP", "目标数量_jp", "目标数量（基本单位数量）"),
            // entity.salesorderitem.targetquantity
            new TranslationSeedItem("entity.salesorderitem.targetquantity", "zh-CN", "目标数量", "目标数量（基本单位数量）"),
            // entity.salesorderitem.targetquantity
            new TranslationSeedItem("entity.salesorderitem.targetquantity", "zh-HK", "目标数量_hk", "目标数量（基本单位数量）"),

            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "en-US", "已发货数量_us", "已发货数量（基本单位数量）"),
            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "ja-JP", "已发货数量_jp", "已发货数量（基本单位数量）"),
            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "zh-CN", "已发货数量", "已发货数量（基本单位数量）"),
            // entity.salesorderitem.shippedquantity
            new TranslationSeedItem("entity.salesorderitem.shippedquantity", "zh-HK", "已发货数量_hk", "已发货数量（基本单位数量）"),

            // entity.salesorderitem.salesperunit
            new TranslationSeedItem("entity.salesorderitem.salesperunit", "en-US", "价格单位_us", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesorderitem.salesperunit
            new TranslationSeedItem("entity.salesorderitem.salesperunit", "ja-JP", "价格单位_jp", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesorderitem.salesperunit
            new TranslationSeedItem("entity.salesorderitem.salesperunit", "zh-CN", "价格单位", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),
            // entity.salesorderitem.salesperunit
            new TranslationSeedItem("entity.salesorderitem.salesperunit", "zh-HK", "价格单位_hk", "价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）"),

            // entity.salesorderitem.salesunitprice
            new TranslationSeedItem("entity.salesorderitem.salesunitprice", "en-US", "销售单价_us", "销售单价"),
            // entity.salesorderitem.salesunitprice
            new TranslationSeedItem("entity.salesorderitem.salesunitprice", "ja-JP", "销售单价_jp", "销售单价"),
            // entity.salesorderitem.salesunitprice
            new TranslationSeedItem("entity.salesorderitem.salesunitprice", "zh-CN", "销售单价", "销售单价"),
            // entity.salesorderitem.salesunitprice
            new TranslationSeedItem("entity.salesorderitem.salesunitprice", "zh-HK", "销售单价_hk", "销售单价"),

            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "en-US", "折扣率_us", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "ja-JP", "折扣率_jp", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "zh-CN", "折扣率", "折扣率（0-100，表示折扣百分比）"),
            // entity.salesorderitem.discountrate
            new TranslationSeedItem("entity.salesorderitem.discountrate", "zh-HK", "折扣率_hk", "折扣率（0-100，表示折扣百分比）"),

            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "en-US", "折扣金额_us", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "ja-JP", "折扣金额_jp", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.salesorderitem.discountamount
            new TranslationSeedItem("entity.salesorderitem.discountamount", "zh-HK", "折扣金额_hk", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.salesorderitem.taxincludedamount
            new TranslationSeedItem("entity.salesorderitem.taxincludedamount", "en-US", "含税金额_us", "含税金额"),
            // entity.salesorderitem.taxincludedamount
            new TranslationSeedItem("entity.salesorderitem.taxincludedamount", "ja-JP", "含税金额_jp", "含税金额"),
            // entity.salesorderitem.taxincludedamount
            new TranslationSeedItem("entity.salesorderitem.taxincludedamount", "zh-CN", "含税金额", "含税金额"),
            // entity.salesorderitem.taxincludedamount
            new TranslationSeedItem("entity.salesorderitem.taxincludedamount", "zh-HK", "含税金额_hk", "含税金额"),

            // entity.salesorderitem.untaxedamount
            new TranslationSeedItem("entity.salesorderitem.untaxedamount", "en-US", "未税金额_us", "未税金额"),
            // entity.salesorderitem.untaxedamount
            new TranslationSeedItem("entity.salesorderitem.untaxedamount", "ja-JP", "未税金额_jp", "未税金额"),
            // entity.salesorderitem.untaxedamount
            new TranslationSeedItem("entity.salesorderitem.untaxedamount", "zh-CN", "未税金额", "未税金额"),
            // entity.salesorderitem.untaxedamount
            new TranslationSeedItem("entity.salesorderitem.untaxedamount", "zh-HK", "未税金额_hk", "未税金额"),

            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "en-US", "税费_us", "税费"),
            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "zh-CN", "税费", "税费"),
            // entity.salesorderitem.taxamount
            new TranslationSeedItem("entity.salesorderitem.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.salesorderitem.salesamount
            new TranslationSeedItem("entity.salesorderitem.salesamount", "en-US", "销售金额_us", "销售金额"),
            // entity.salesorderitem.salesamount
            new TranslationSeedItem("entity.salesorderitem.salesamount", "ja-JP", "销售金额_jp", "销售金额"),
            // entity.salesorderitem.salesamount
            new TranslationSeedItem("entity.salesorderitem.salesamount", "zh-CN", "销售金额", "销售金额"),
            // entity.salesorderitem.salesamount
            new TranslationSeedItem("entity.salesorderitem.salesamount", "zh-HK", "销售金额_hk", "销售金额"),

            // entity.salesorderitem.grossweight
            new TranslationSeedItem("entity.salesorderitem.grossweight", "en-US", "毛重_us", "毛重"),
            // entity.salesorderitem.grossweight
            new TranslationSeedItem("entity.salesorderitem.grossweight", "ja-JP", "毛重_jp", "毛重"),
            // entity.salesorderitem.grossweight
            new TranslationSeedItem("entity.salesorderitem.grossweight", "zh-CN", "毛重", "毛重"),
            // entity.salesorderitem.grossweight
            new TranslationSeedItem("entity.salesorderitem.grossweight", "zh-HK", "毛重_hk", "毛重"),

            // entity.salesorderitem.netweight
            new TranslationSeedItem("entity.salesorderitem.netweight", "en-US", "净重_us", "净重"),
            // entity.salesorderitem.netweight
            new TranslationSeedItem("entity.salesorderitem.netweight", "ja-JP", "净重_jp", "净重"),
            // entity.salesorderitem.netweight
            new TranslationSeedItem("entity.salesorderitem.netweight", "zh-CN", "净重", "净重"),
            // entity.salesorderitem.netweight
            new TranslationSeedItem("entity.salesorderitem.netweight", "zh-HK", "净重_hk", "净重"),

            // entity.salesorderitem.weightunit
            new TranslationSeedItem("entity.salesorderitem.weightunit", "en-US", "重量单位_us", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.salesorderitem.weightunit
            new TranslationSeedItem("entity.salesorderitem.weightunit", "ja-JP", "重量单位_jp", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.salesorderitem.weightunit
            new TranslationSeedItem("entity.salesorderitem.weightunit", "zh-CN", "重量单位", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),
            // entity.salesorderitem.weightunit
            new TranslationSeedItem("entity.salesorderitem.weightunit", "zh-HK", "重量单位_hk", "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）"),

            // entity.salesorderitem.volume
            new TranslationSeedItem("entity.salesorderitem.volume", "en-US", "体积_us", "体积"),
            // entity.salesorderitem.volume
            new TranslationSeedItem("entity.salesorderitem.volume", "ja-JP", "体积_jp", "体积"),
            // entity.salesorderitem.volume
            new TranslationSeedItem("entity.salesorderitem.volume", "zh-CN", "体积", "体积"),
            // entity.salesorderitem.volume
            new TranslationSeedItem("entity.salesorderitem.volume", "zh-HK", "体积_hk", "体积"),

            // entity.salesorderitem.volumeunit
            new TranslationSeedItem("entity.salesorderitem.volumeunit", "en-US", "体积单位_us", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.salesorderitem.volumeunit
            new TranslationSeedItem("entity.salesorderitem.volumeunit", "ja-JP", "体积单位_jp", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.salesorderitem.volumeunit
            new TranslationSeedItem("entity.salesorderitem.volumeunit", "zh-CN", "体积单位", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),
            // entity.salesorderitem.volumeunit
            new TranslationSeedItem("entity.salesorderitem.volumeunit", "zh-HK", "体积单位_hk", "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）"),

            // entity.salesorderitem.profitcentercode
            new TranslationSeedItem("entity.salesorderitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesorderitem.profitcentercode
            new TranslationSeedItem("entity.salesorderitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesorderitem.profitcentercode
            new TranslationSeedItem("entity.salesorderitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.salesorderitem.profitcentercode
            new TranslationSeedItem("entity.salesorderitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "en-US", "行交货状态_us", "行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）"),
            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "ja-JP", "行交货状态_jp", "行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）"),
            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "zh-CN", "行交货状态", "行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）"),
            // entity.salesorderitem.deliverystatus
            new TranslationSeedItem("entity.salesorderitem.deliverystatus", "zh-HK", "行交货状态_hk", "行交货状态（字典 logistics_sales_delivery_status；0=未交货 1=部分交货 2=全部交货）"),

            // entity.salesorderitem.isobsolete
            new TranslationSeedItem("entity.salesorderitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesorderitem.isobsolete
            new TranslationSeedItem("entity.salesorderitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesorderitem.isobsolete
            new TranslationSeedItem("entity.salesorderitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesorderitem.isobsolete
            new TranslationSeedItem("entity.salesorderitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.salesorderitem.salesorder
            new TranslationSeedItem("entity.salesorderitem.salesorder", "en-US", "销售订单主表_us", "销售订单主表"),
            // entity.salesorderitem.salesorder
            new TranslationSeedItem("entity.salesorderitem.salesorder", "ja-JP", "销售订单主表_jp", "销售订单主表"),
            // entity.salesorderitem.salesorder
            new TranslationSeedItem("entity.salesorderitem.salesorder", "zh-CN", "销售订单主表", "销售订单主表"),
            // entity.salesorderitem.salesorder
            new TranslationSeedItem("entity.salesorderitem.salesorder", "zh-HK", "销售订单主表_hk", "销售订单主表"),
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
