// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialDocumentItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialDocumentItem 实体国际化翻译种子（键前缀 entity.materialdocumentitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialDocumentItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialDocumentItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialdocumentitem 实体翻译...", tenantCode);

        foreach (var item in GetMaterialDocumentItemTranslations())
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

        TaktLogger.Information("TaktMaterialDocumentItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialDocumentItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialdocumentitem._self / entity.materialdocumentitem.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialDocumentItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "en-US", "Material Document Item Information_us", "实体名称"),
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "ja-JP", "Takt物料凭证行项目信息_jp", "实体名称"),
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "zh-CN", "Takt物料凭证行项目信息", "实体名称"),
            // entity.materialdocumentitem._self
            new TranslationSeedItem("entity.materialdocumentitem._self", "zh-HK", "Takt物料凭证行项目信息_hk", "实体名称"),

            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "en-US", "物料凭证ID_us", "物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）"),
            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "ja-JP", "物料凭证ID_jp", "物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）"),
            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "zh-CN", "物料凭证ID", "物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）"),
            // entity.materialdocumentitem.materialdocumentid
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentid", "zh-HK", "物料凭证ID_hk", "物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）"),

            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "en-US", "物料凭证_us", "物料凭证（冗余：按对应 Id 取主数据名称联动）"),
            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "ja-JP", "物料凭证_jp", "物料凭证（冗余：按对应 Id 取主数据名称联动）"),
            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "zh-CN", "物料凭证", "物料凭证（冗余：按对应 Id 取主数据名称联动）"),
            // entity.materialdocumentitem.materialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.materialdocumentcode", "zh-HK", "物料凭证_hk", "物料凭证（冗余：按对应 Id 取主数据名称联动）"),

            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "en-US", "物料凭证项目_us", "物料凭证项目（行号步长生成器用 int，固定步长=10）"),
            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "ja-JP", "物料凭证项目_jp", "物料凭证项目（行号步长生成器用 int，固定步长=10）"),
            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "zh-CN", "物料凭证项目", "物料凭证项目（行号步长生成器用 int，固定步长=10）"),
            // entity.materialdocumentitem.linenumber
            new TranslationSeedItem("entity.materialdocumentitem.linenumber", "zh-HK", "物料凭证项目_hk", "物料凭证项目（行号步长生成器用 int，固定步长=10）"),

            // entity.materialdocumentitem.lineid
            new TranslationSeedItem("entity.materialdocumentitem.lineid", "en-US", "行标识_us", "行标识"),
            // entity.materialdocumentitem.lineid
            new TranslationSeedItem("entity.materialdocumentitem.lineid", "ja-JP", "行标识_jp", "行标识"),
            // entity.materialdocumentitem.lineid
            new TranslationSeedItem("entity.materialdocumentitem.lineid", "zh-CN", "行标识", "行标识"),
            // entity.materialdocumentitem.lineid
            new TranslationSeedItem("entity.materialdocumentitem.lineid", "zh-HK", "行标识_hk", "行标识"),

            // entity.materialdocumentitem.parentlineid
            new TranslationSeedItem("entity.materialdocumentitem.parentlineid", "en-US", "上级行 ID_us", "上级行 ID"),
            // entity.materialdocumentitem.parentlineid
            new TranslationSeedItem("entity.materialdocumentitem.parentlineid", "ja-JP", "上级行 ID_jp", "上级行 ID"),
            // entity.materialdocumentitem.parentlineid
            new TranslationSeedItem("entity.materialdocumentitem.parentlineid", "zh-CN", "上级行 ID", "上级行 ID"),
            // entity.materialdocumentitem.parentlineid
            new TranslationSeedItem("entity.materialdocumentitem.parentlineid", "zh-HK", "上级行 ID_hk", "上级行 ID"),

            // entity.materialdocumentitem.linedepth
            new TranslationSeedItem("entity.materialdocumentitem.linedepth", "en-US", "层次结构级别_us", "层次结构级别"),
            // entity.materialdocumentitem.linedepth
            new TranslationSeedItem("entity.materialdocumentitem.linedepth", "ja-JP", "层次结构级别_jp", "层次结构级别"),
            // entity.materialdocumentitem.linedepth
            new TranslationSeedItem("entity.materialdocumentitem.linedepth", "zh-CN", "层次结构级别", "层次结构级别"),
            // entity.materialdocumentitem.linedepth
            new TranslationSeedItem("entity.materialdocumentitem.linedepth", "zh-HK", "层次结构级别_hk", "层次结构级别"),

            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "en-US", "移动类型_us", "移动类型（字典 logistics_materials_movement_type）"),
            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "ja-JP", "移动类型_jp", "移动类型（字典 logistics_materials_movement_type）"),
            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "zh-CN", "移动类型", "移动类型（字典 logistics_materials_movement_type）"),
            // entity.materialdocumentitem.movementtype
            new TranslationSeedItem("entity.materialdocumentitem.movementtype", "zh-HK", "移动类型_hk", "移动类型（字典 logistics_materials_movement_type）"),

            // entity.materialdocumentitem.autocreatedflag
            new TranslationSeedItem("entity.materialdocumentitem.autocreatedflag", "en-US", "项目自动创建_us", "项目自动创建"),
            // entity.materialdocumentitem.autocreatedflag
            new TranslationSeedItem("entity.materialdocumentitem.autocreatedflag", "ja-JP", "项目自动创建_jp", "项目自动创建"),
            // entity.materialdocumentitem.autocreatedflag
            new TranslationSeedItem("entity.materialdocumentitem.autocreatedflag", "zh-CN", "项目自动创建", "项目自动创建"),
            // entity.materialdocumentitem.autocreatedflag
            new TranslationSeedItem("entity.materialdocumentitem.autocreatedflag", "zh-HK", "项目自动创建_hk", "项目自动创建"),

            // entity.materialdocumentitem.materialcode
            new TranslationSeedItem("entity.materialdocumentitem.materialcode", "en-US", "物料_us", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialdocumentitem.materialcode
            new TranslationSeedItem("entity.materialdocumentitem.materialcode", "ja-JP", "物料_jp", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialdocumentitem.materialcode
            new TranslationSeedItem("entity.materialdocumentitem.materialcode", "zh-CN", "物料", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialdocumentitem.materialcode
            new TranslationSeedItem("entity.materialdocumentitem.materialcode", "zh-HK", "物料_hk", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "en-US", "库存地点_us", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "ja-JP", "库存地点_jp", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "zh-CN", "库存地点", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),
            // entity.materialdocumentitem.warehousecode
            new TranslationSeedItem("entity.materialdocumentitem.warehousecode", "zh-HK", "库存地点_hk", "库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）"),

            // entity.materialdocumentitem.batchcode
            new TranslationSeedItem("entity.materialdocumentitem.batchcode", "en-US", "批次_us", "批次"),
            // entity.materialdocumentitem.batchcode
            new TranslationSeedItem("entity.materialdocumentitem.batchcode", "ja-JP", "批次_jp", "批次"),
            // entity.materialdocumentitem.batchcode
            new TranslationSeedItem("entity.materialdocumentitem.batchcode", "zh-CN", "批次", "批次"),
            // entity.materialdocumentitem.batchcode
            new TranslationSeedItem("entity.materialdocumentitem.batchcode", "zh-HK", "批次_hk", "批次"),

            // entity.materialdocumentitem.stocktype
            new TranslationSeedItem("entity.materialdocumentitem.stocktype", "en-US", "库存类型_us", "库存类型（字典 logistics_stock_type）"),
            // entity.materialdocumentitem.stocktype
            new TranslationSeedItem("entity.materialdocumentitem.stocktype", "ja-JP", "库存类型_jp", "库存类型（字典 logistics_stock_type）"),
            // entity.materialdocumentitem.stocktype
            new TranslationSeedItem("entity.materialdocumentitem.stocktype", "zh-CN", "库存类型", "库存类型（字典 logistics_stock_type）"),
            // entity.materialdocumentitem.stocktype
            new TranslationSeedItem("entity.materialdocumentitem.stocktype", "zh-HK", "库存类型_hk", "库存类型（字典 logistics_stock_type）"),

            // entity.materialdocumentitem.restrictedstockflag
            new TranslationSeedItem("entity.materialdocumentitem.restrictedstockflag", "en-US", "批次限制_us", "批次限制"),
            // entity.materialdocumentitem.restrictedstockflag
            new TranslationSeedItem("entity.materialdocumentitem.restrictedstockflag", "ja-JP", "批次限制_jp", "批次限制"),
            // entity.materialdocumentitem.restrictedstockflag
            new TranslationSeedItem("entity.materialdocumentitem.restrictedstockflag", "zh-CN", "批次限制", "批次限制"),
            // entity.materialdocumentitem.restrictedstockflag
            new TranslationSeedItem("entity.materialdocumentitem.restrictedstockflag", "zh-HK", "批次限制_hk", "批次限制"),

            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "en-US", "特殊库存_us", "特殊库存（字典 logistics_materials_special_stock_type）"),
            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "ja-JP", "特殊库存_jp", "特殊库存（字典 logistics_materials_special_stock_type）"),
            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "zh-CN", "特殊库存", "特殊库存（字典 logistics_materials_special_stock_type）"),
            // entity.materialdocumentitem.specialstock
            new TranslationSeedItem("entity.materialdocumentitem.specialstock", "zh-HK", "特殊库存_hk", "特殊库存（字典 logistics_materials_special_stock_type）"),

            // entity.materialdocumentitem.suppliercode
            new TranslationSeedItem("entity.materialdocumentitem.suppliercode", "en-US", "供应商_us", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.materialdocumentitem.suppliercode
            new TranslationSeedItem("entity.materialdocumentitem.suppliercode", "ja-JP", "供应商_jp", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.materialdocumentitem.suppliercode
            new TranslationSeedItem("entity.materialdocumentitem.suppliercode", "zh-CN", "供应商", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.materialdocumentitem.suppliercode
            new TranslationSeedItem("entity.materialdocumentitem.suppliercode", "zh-HK", "供应商_hk", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "en-US", "客户_us", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "ja-JP", "客户_jp", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "zh-CN", "客户", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.materialdocumentitem.customercode
            new TranslationSeedItem("entity.materialdocumentitem.customercode", "zh-HK", "客户_hk", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.materialdocumentitem.debitcreditindicator
            new TranslationSeedItem("entity.materialdocumentitem.debitcreditindicator", "en-US", "借/贷标识_us", "借/贷标识（S=借方入库/增加，H=贷方冲销退货/减少；入库实绩统计按此加减，不以移动类型取反）"),
            // entity.materialdocumentitem.debitcreditindicator
            new TranslationSeedItem("entity.materialdocumentitem.debitcreditindicator", "ja-JP", "借/贷标识_jp", "借/贷标识（S=借方入库/增加，H=贷方冲销退货/减少；入库实绩统计按此加减，不以移动类型取反）"),
            // entity.materialdocumentitem.debitcreditindicator
            new TranslationSeedItem("entity.materialdocumentitem.debitcreditindicator", "zh-CN", "借/贷标识", "借/贷标识（S=借方入库/增加，H=贷方冲销退货/减少；入库实绩统计按此加减，不以移动类型取反）"),
            // entity.materialdocumentitem.debitcreditindicator
            new TranslationSeedItem("entity.materialdocumentitem.debitcreditindicator", "zh-HK", "借/贷标识_hk", "借/贷标识（S=借方入库/增加，H=贷方冲销退货/减少；入库实绩统计按此加减，不以移动类型取反）"),

            // entity.materialdocumentitem.currencycode
            new TranslationSeedItem("entity.materialdocumentitem.currencycode", "en-US", "货币_us", "货币（字典 accounting_financial_currency_code）"),
            // entity.materialdocumentitem.currencycode
            new TranslationSeedItem("entity.materialdocumentitem.currencycode", "ja-JP", "货币_jp", "货币（字典 accounting_financial_currency_code）"),
            // entity.materialdocumentitem.currencycode
            new TranslationSeedItem("entity.materialdocumentitem.currencycode", "zh-CN", "货币", "货币（字典 accounting_financial_currency_code）"),
            // entity.materialdocumentitem.currencycode
            new TranslationSeedItem("entity.materialdocumentitem.currencycode", "zh-HK", "货币_hk", "货币（字典 accounting_financial_currency_code）"),

            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "en-US", "本位币金额_us", "本位币金额"),
            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "ja-JP", "本位币金额_jp", "本位币金额"),
            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "zh-CN", "本位币金额", "本位币金额"),
            // entity.materialdocumentitem.localcurrencyamount
            new TranslationSeedItem("entity.materialdocumentitem.localcurrencyamount", "zh-HK", "本位币金额_hk", "本位币金额"),

            // entity.materialdocumentitem.alternativeamount
            new TranslationSeedItem("entity.materialdocumentitem.alternativeamount", "en-US", "金额_us", "金额"),
            // entity.materialdocumentitem.alternativeamount
            new TranslationSeedItem("entity.materialdocumentitem.alternativeamount", "ja-JP", "金额_jp", "金额"),
            // entity.materialdocumentitem.alternativeamount
            new TranslationSeedItem("entity.materialdocumentitem.alternativeamount", "zh-CN", "金额", "金额"),
            // entity.materialdocumentitem.alternativeamount
            new TranslationSeedItem("entity.materialdocumentitem.alternativeamount", "zh-HK", "金额_hk", "金额"),

            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "en-US", "数量_us", "数量（基本计量单位）"),
            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "ja-JP", "数量_jp", "数量（基本计量单位）"),
            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "zh-CN", "数量", "数量（基本计量单位）"),
            // entity.materialdocumentitem.quantity
            new TranslationSeedItem("entity.materialdocumentitem.quantity", "zh-HK", "数量_hk", "数量（基本计量单位）"),

            // entity.materialdocumentitem.baseunit
            new TranslationSeedItem("entity.materialdocumentitem.baseunit", "en-US", "基本计量单位_us", "基本计量单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.materialdocumentitem.baseunit
            new TranslationSeedItem("entity.materialdocumentitem.baseunit", "ja-JP", "基本计量单位_jp", "基本计量单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.materialdocumentitem.baseunit
            new TranslationSeedItem("entity.materialdocumentitem.baseunit", "zh-CN", "基本计量单位", "基本计量单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.materialdocumentitem.baseunit
            new TranslationSeedItem("entity.materialdocumentitem.baseunit", "zh-HK", "基本计量单位_hk", "基本计量单位（字典 logistics_materials_unit_of_measure_code）"),

            // entity.materialdocumentitem.entryquantity
            new TranslationSeedItem("entity.materialdocumentitem.entryquantity", "en-US", "输入单位数量_us", "输入单位数量"),
            // entity.materialdocumentitem.entryquantity
            new TranslationSeedItem("entity.materialdocumentitem.entryquantity", "ja-JP", "输入单位数量_jp", "输入单位数量"),
            // entity.materialdocumentitem.entryquantity
            new TranslationSeedItem("entity.materialdocumentitem.entryquantity", "zh-CN", "输入单位数量", "输入单位数量"),
            // entity.materialdocumentitem.entryquantity
            new TranslationSeedItem("entity.materialdocumentitem.entryquantity", "zh-HK", "输入单位数量_hk", "输入单位数量"),

            // entity.materialdocumentitem.entryunit
            new TranslationSeedItem("entity.materialdocumentitem.entryunit", "en-US", "条目单位_us", "条目单位"),
            // entity.materialdocumentitem.entryunit
            new TranslationSeedItem("entity.materialdocumentitem.entryunit", "ja-JP", "条目单位_jp", "条目单位"),
            // entity.materialdocumentitem.entryunit
            new TranslationSeedItem("entity.materialdocumentitem.entryunit", "zh-CN", "条目单位", "条目单位"),
            // entity.materialdocumentitem.entryunit
            new TranslationSeedItem("entity.materialdocumentitem.entryunit", "zh-HK", "条目单位_hk", "条目单位"),

            // entity.materialdocumentitem.popricequantity
            new TranslationSeedItem("entity.materialdocumentitem.popricequantity", "en-US", "订单价格单位数量_us", "订单价格单位数量"),
            // entity.materialdocumentitem.popricequantity
            new TranslationSeedItem("entity.materialdocumentitem.popricequantity", "ja-JP", "订单价格单位数量_jp", "订单价格单位数量"),
            // entity.materialdocumentitem.popricequantity
            new TranslationSeedItem("entity.materialdocumentitem.popricequantity", "zh-CN", "订单价格单位数量", "订单价格单位数量"),
            // entity.materialdocumentitem.popricequantity
            new TranslationSeedItem("entity.materialdocumentitem.popricequantity", "zh-HK", "订单价格单位数量_hk", "订单价格单位数量"),

            // entity.materialdocumentitem.popriceunit
            new TranslationSeedItem("entity.materialdocumentitem.popriceunit", "en-US", "订单价格单位_us", "订单价格单位"),
            // entity.materialdocumentitem.popriceunit
            new TranslationSeedItem("entity.materialdocumentitem.popriceunit", "ja-JP", "订单价格单位_jp", "订单价格单位"),
            // entity.materialdocumentitem.popriceunit
            new TranslationSeedItem("entity.materialdocumentitem.popriceunit", "zh-CN", "订单价格单位", "订单价格单位"),
            // entity.materialdocumentitem.popriceunit
            new TranslationSeedItem("entity.materialdocumentitem.popriceunit", "zh-HK", "订单价格单位_hk", "订单价格单位"),

            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "en-US", "采购订单_us", "采购订单"),
            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "ja-JP", "采购订单_jp", "采购订单"),
            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "zh-CN", "采购订单", "采购订单"),
            // entity.materialdocumentitem.purchaseordercode
            new TranslationSeedItem("entity.materialdocumentitem.purchaseordercode", "zh-HK", "采购订单_hk", "采购订单"),

            // entity.materialdocumentitem.purchaseorderitem
            new TranslationSeedItem("entity.materialdocumentitem.purchaseorderitem", "en-US", "项目_us", "采购订单项目"),
            // entity.materialdocumentitem.purchaseorderitem
            new TranslationSeedItem("entity.materialdocumentitem.purchaseorderitem", "ja-JP", "项目_jp", "采购订单项目"),
            // entity.materialdocumentitem.purchaseorderitem
            new TranslationSeedItem("entity.materialdocumentitem.purchaseorderitem", "zh-CN", "项目", "采购订单项目"),
            // entity.materialdocumentitem.purchaseorderitem
            new TranslationSeedItem("entity.materialdocumentitem.purchaseorderitem", "zh-HK", "项目_hk", "采购订单项目"),

            // entity.materialdocumentitem.referencedocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentyear", "en-US", "参考凭证会计年度_us", "参考凭证会计年度"),
            // entity.materialdocumentitem.referencedocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentyear", "ja-JP", "参考凭证会计年度_jp", "参考凭证会计年度"),
            // entity.materialdocumentitem.referencedocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentyear", "zh-CN", "参考凭证会计年度", "参考凭证会计年度"),
            // entity.materialdocumentitem.referencedocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentyear", "zh-HK", "参考凭证会计年度_hk", "参考凭证会计年度"),

            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "en-US", "参考凭证_us", "参考凭证"),
            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "ja-JP", "参考凭证_jp", "参考凭证"),
            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "zh-CN", "参考凭证", "参考凭证"),
            // entity.materialdocumentitem.referencedocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentcode", "zh-HK", "参考凭证_hk", "参考凭证"),

            // entity.materialdocumentitem.referencedocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentitem", "en-US", "参考凭证项目_us", "参考凭证项目"),
            // entity.materialdocumentitem.referencedocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentitem", "ja-JP", "参考凭证项目_jp", "参考凭证项目"),
            // entity.materialdocumentitem.referencedocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentitem", "zh-CN", "参考凭证项目", "参考凭证项目"),
            // entity.materialdocumentitem.referencedocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.referencedocumentitem", "zh-HK", "参考凭证项目_hk", "参考凭证项目"),

            // entity.materialdocumentitem.originalmaterialdocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentyear", "en-US", "物料凭证的年份_us", "冲销物料凭证的年份"),
            // entity.materialdocumentitem.originalmaterialdocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentyear", "ja-JP", "物料凭证的年份_jp", "冲销物料凭证的年份"),
            // entity.materialdocumentitem.originalmaterialdocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentyear", "zh-CN", "物料凭证的年份", "冲销物料凭证的年份"),
            // entity.materialdocumentitem.originalmaterialdocumentyear
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentyear", "zh-HK", "物料凭证的年份_hk", "冲销物料凭证的年份"),

            // entity.materialdocumentitem.originalmaterialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentcode", "en-US", "物料凭证_us", "冲销物料凭证"),
            // entity.materialdocumentitem.originalmaterialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentcode", "ja-JP", "物料凭证_jp", "冲销物料凭证"),
            // entity.materialdocumentitem.originalmaterialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentcode", "zh-CN", "物料凭证", "冲销物料凭证"),
            // entity.materialdocumentitem.originalmaterialdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.originalmaterialdocumentcode", "zh-HK", "物料凭证_hk", "冲销物料凭证"),

            // entity.materialdocumentitem.originallinenumber
            new TranslationSeedItem("entity.materialdocumentitem.originallinenumber", "en-US", "物料凭证项目_us", "冲销物料凭证项目"),
            // entity.materialdocumentitem.originallinenumber
            new TranslationSeedItem("entity.materialdocumentitem.originallinenumber", "ja-JP", "物料凭证项目_jp", "冲销物料凭证项目"),
            // entity.materialdocumentitem.originallinenumber
            new TranslationSeedItem("entity.materialdocumentitem.originallinenumber", "zh-CN", "物料凭证项目", "冲销物料凭证项目"),
            // entity.materialdocumentitem.originallinenumber
            new TranslationSeedItem("entity.materialdocumentitem.originallinenumber", "zh-HK", "物料凭证项目_hk", "冲销物料凭证项目"),

            // entity.materialdocumentitem.deliverycompletedflag
            new TranslationSeedItem("entity.materialdocumentitem.deliverycompletedflag", "en-US", "交货已完成_us", "交货已完成"),
            // entity.materialdocumentitem.deliverycompletedflag
            new TranslationSeedItem("entity.materialdocumentitem.deliverycompletedflag", "ja-JP", "交货已完成_jp", "交货已完成"),
            // entity.materialdocumentitem.deliverycompletedflag
            new TranslationSeedItem("entity.materialdocumentitem.deliverycompletedflag", "zh-CN", "交货已完成", "交货已完成"),
            // entity.materialdocumentitem.deliverycompletedflag
            new TranslationSeedItem("entity.materialdocumentitem.deliverycompletedflag", "zh-HK", "交货已完成_hk", "交货已完成"),

            // entity.materialdocumentitem.itemtext
            new TranslationSeedItem("entity.materialdocumentitem.itemtext", "en-US", "文本_us", "文本（项目文本最长 50，故 Length=50）"),
            // entity.materialdocumentitem.itemtext
            new TranslationSeedItem("entity.materialdocumentitem.itemtext", "ja-JP", "文本_jp", "文本（项目文本最长 50，故 Length=50）"),
            // entity.materialdocumentitem.itemtext
            new TranslationSeedItem("entity.materialdocumentitem.itemtext", "zh-CN", "文本", "文本（项目文本最长 50，故 Length=50）"),
            // entity.materialdocumentitem.itemtext
            new TranslationSeedItem("entity.materialdocumentitem.itemtext", "zh-HK", "文本_hk", "文本（项目文本最长 50，故 Length=50）"),

            // entity.materialdocumentitem.equipmentcode
            new TranslationSeedItem("entity.materialdocumentitem.equipmentcode", "en-US", "设备_us", "设备"),
            // entity.materialdocumentitem.equipmentcode
            new TranslationSeedItem("entity.materialdocumentitem.equipmentcode", "ja-JP", "设备_jp", "设备"),
            // entity.materialdocumentitem.equipmentcode
            new TranslationSeedItem("entity.materialdocumentitem.equipmentcode", "zh-CN", "设备", "设备"),
            // entity.materialdocumentitem.equipmentcode
            new TranslationSeedItem("entity.materialdocumentitem.equipmentcode", "zh-HK", "设备_hk", "设备"),

            // entity.materialdocumentitem.goodsrecipient
            new TranslationSeedItem("entity.materialdocumentitem.goodsrecipient", "en-US", "收货方_us", "收货方（最长 12，故 Length=12）"),
            // entity.materialdocumentitem.goodsrecipient
            new TranslationSeedItem("entity.materialdocumentitem.goodsrecipient", "ja-JP", "收货方_jp", "收货方（最长 12，故 Length=12）"),
            // entity.materialdocumentitem.goodsrecipient
            new TranslationSeedItem("entity.materialdocumentitem.goodsrecipient", "zh-CN", "收货方", "收货方（最长 12，故 Length=12）"),
            // entity.materialdocumentitem.goodsrecipient
            new TranslationSeedItem("entity.materialdocumentitem.goodsrecipient", "zh-HK", "收货方_hk", "收货方（最长 12，故 Length=12）"),

            // entity.materialdocumentitem.unloadingpoint
            new TranslationSeedItem("entity.materialdocumentitem.unloadingpoint", "en-US", "卸货点_us", "卸货点（最长 25，故 Length=25）"),
            // entity.materialdocumentitem.unloadingpoint
            new TranslationSeedItem("entity.materialdocumentitem.unloadingpoint", "ja-JP", "卸货点_jp", "卸货点（最长 25，故 Length=25）"),
            // entity.materialdocumentitem.unloadingpoint
            new TranslationSeedItem("entity.materialdocumentitem.unloadingpoint", "zh-CN", "卸货点", "卸货点（最长 25，故 Length=25）"),
            // entity.materialdocumentitem.unloadingpoint
            new TranslationSeedItem("entity.materialdocumentitem.unloadingpoint", "zh-HK", "卸货点_hk", "卸货点（最长 25，故 Length=25）"),

            // entity.materialdocumentitem.businessareacode
            new TranslationSeedItem("entity.materialdocumentitem.businessareacode", "en-US", "业务范围_us", "业务范围"),
            // entity.materialdocumentitem.businessareacode
            new TranslationSeedItem("entity.materialdocumentitem.businessareacode", "ja-JP", "业务范围_jp", "业务范围"),
            // entity.materialdocumentitem.businessareacode
            new TranslationSeedItem("entity.materialdocumentitem.businessareacode", "zh-CN", "业务范围", "业务范围"),
            // entity.materialdocumentitem.businessareacode
            new TranslationSeedItem("entity.materialdocumentitem.businessareacode", "zh-HK", "业务范围_hk", "业务范围"),

            // entity.materialdocumentitem.controllingareacode
            new TranslationSeedItem("entity.materialdocumentitem.controllingareacode", "en-US", "成本控制域_us", "成本控制域"),
            // entity.materialdocumentitem.controllingareacode
            new TranslationSeedItem("entity.materialdocumentitem.controllingareacode", "ja-JP", "成本控制域_jp", "成本控制域"),
            // entity.materialdocumentitem.controllingareacode
            new TranslationSeedItem("entity.materialdocumentitem.controllingareacode", "zh-CN", "成本控制域", "成本控制域"),
            // entity.materialdocumentitem.controllingareacode
            new TranslationSeedItem("entity.materialdocumentitem.controllingareacode", "zh-HK", "成本控制域_hk", "成本控制域"),

            // entity.materialdocumentitem.tradingpartnerbusinessarea
            new TranslationSeedItem("entity.materialdocumentitem.tradingpartnerbusinessarea", "en-US", "伙伴业务范围_us", "伙伴业务范围"),
            // entity.materialdocumentitem.tradingpartnerbusinessarea
            new TranslationSeedItem("entity.materialdocumentitem.tradingpartnerbusinessarea", "ja-JP", "伙伴业务范围_jp", "伙伴业务范围"),
            // entity.materialdocumentitem.tradingpartnerbusinessarea
            new TranslationSeedItem("entity.materialdocumentitem.tradingpartnerbusinessarea", "zh-CN", "伙伴业务范围", "伙伴业务范围"),
            // entity.materialdocumentitem.tradingpartnerbusinessarea
            new TranslationSeedItem("entity.materialdocumentitem.tradingpartnerbusinessarea", "zh-HK", "伙伴业务范围_hk", "伙伴业务范围"),

            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "en-US", "订单_us", "订单"),
            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "ja-JP", "订单_jp", "订单"),
            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "zh-CN", "订单", "订单"),
            // entity.materialdocumentitem.productionordercode
            new TranslationSeedItem("entity.materialdocumentitem.productionordercode", "zh-HK", "订单_hk", "订单"),

            // entity.materialdocumentitem.assetcode
            new TranslationSeedItem("entity.materialdocumentitem.assetcode", "en-US", "资产_us", "资产"),
            // entity.materialdocumentitem.assetcode
            new TranslationSeedItem("entity.materialdocumentitem.assetcode", "ja-JP", "资产_jp", "资产"),
            // entity.materialdocumentitem.assetcode
            new TranslationSeedItem("entity.materialdocumentitem.assetcode", "zh-CN", "资产", "资产"),
            // entity.materialdocumentitem.assetcode
            new TranslationSeedItem("entity.materialdocumentitem.assetcode", "zh-HK", "资产_hk", "资产"),

            // entity.materialdocumentitem.assetsubcode
            new TranslationSeedItem("entity.materialdocumentitem.assetsubcode", "en-US", "次级编号_us", "次级编号"),
            // entity.materialdocumentitem.assetsubcode
            new TranslationSeedItem("entity.materialdocumentitem.assetsubcode", "ja-JP", "次级编号_jp", "次级编号"),
            // entity.materialdocumentitem.assetsubcode
            new TranslationSeedItem("entity.materialdocumentitem.assetsubcode", "zh-CN", "次级编号", "次级编号"),
            // entity.materialdocumentitem.assetsubcode
            new TranslationSeedItem("entity.materialdocumentitem.assetsubcode", "zh-HK", "次级编号_hk", "次级编号"),

            // entity.materialdocumentitem.fiscalyear
            new TranslationSeedItem("entity.materialdocumentitem.fiscalyear", "en-US", "会计年度_us", "会计年度"),
            // entity.materialdocumentitem.fiscalyear
            new TranslationSeedItem("entity.materialdocumentitem.fiscalyear", "ja-JP", "会计年度_jp", "会计年度"),
            // entity.materialdocumentitem.fiscalyear
            new TranslationSeedItem("entity.materialdocumentitem.fiscalyear", "zh-CN", "会计年度", "会计年度"),
            // entity.materialdocumentitem.fiscalyear
            new TranslationSeedItem("entity.materialdocumentitem.fiscalyear", "zh-HK", "会计年度_hk", "会计年度"),

            // entity.materialdocumentitem.posttopreviousperiodflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousperiodflag", "en-US", "允许前期记帐_us", "允许前期记帐"),
            // entity.materialdocumentitem.posttopreviousperiodflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousperiodflag", "ja-JP", "允许前期记帐_jp", "允许前期记帐"),
            // entity.materialdocumentitem.posttopreviousperiodflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousperiodflag", "zh-CN", "允许前期记帐", "允许前期记帐"),
            // entity.materialdocumentitem.posttopreviousperiodflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousperiodflag", "zh-HK", "允许前期记帐_hk", "允许前期记帐"),

            // entity.materialdocumentitem.posttopreviousyearflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousyearflag", "en-US", "上年度记帐_us", "上年度记帐"),
            // entity.materialdocumentitem.posttopreviousyearflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousyearflag", "ja-JP", "上年度记帐_jp", "上年度记帐"),
            // entity.materialdocumentitem.posttopreviousyearflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousyearflag", "zh-CN", "上年度记帐", "上年度记帐"),
            // entity.materialdocumentitem.posttopreviousyearflag
            new TranslationSeedItem("entity.materialdocumentitem.posttopreviousyearflag", "zh-HK", "上年度记帐_hk", "上年度记帐"),

            // entity.materialdocumentitem.accountingdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentcode", "en-US", "凭证编号_us", "会计凭证编号"),
            // entity.materialdocumentitem.accountingdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentcode", "ja-JP", "凭证编号_jp", "会计凭证编号"),
            // entity.materialdocumentitem.accountingdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentcode", "zh-CN", "凭证编号", "会计凭证编号"),
            // entity.materialdocumentitem.accountingdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentcode", "zh-HK", "凭证编号_hk", "会计凭证编号"),

            // entity.materialdocumentitem.accountingdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentitem", "en-US", "行项目_us", "会计凭证行项目"),
            // entity.materialdocumentitem.accountingdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentitem", "ja-JP", "行项目_jp", "会计凭证行项目"),
            // entity.materialdocumentitem.accountingdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentitem", "zh-CN", "行项目", "会计凭证行项目"),
            // entity.materialdocumentitem.accountingdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.accountingdocumentitem", "zh-HK", "行项目_hk", "会计凭证行项目"),

            // entity.materialdocumentitem.revaluationdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentcode", "en-US", "凭证编号_us", "再评估凭证编号"),
            // entity.materialdocumentitem.revaluationdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentcode", "ja-JP", "凭证编号_jp", "再评估凭证编号"),
            // entity.materialdocumentitem.revaluationdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentcode", "zh-CN", "凭证编号", "再评估凭证编号"),
            // entity.materialdocumentitem.revaluationdocumentcode
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentcode", "zh-HK", "凭证编号_hk", "再评估凭证编号"),

            // entity.materialdocumentitem.revaluationdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentitem", "en-US", "行项目_us", "再评估凭证行项目"),
            // entity.materialdocumentitem.revaluationdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentitem", "ja-JP", "行项目_jp", "再评估凭证行项目"),
            // entity.materialdocumentitem.revaluationdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentitem", "zh-CN", "行项目", "再评估凭证行项目"),
            // entity.materialdocumentitem.revaluationdocumentitem
            new TranslationSeedItem("entity.materialdocumentitem.revaluationdocumentitem", "zh-HK", "行项目_hk", "再评估凭证行项目"),

            // entity.materialdocumentitem.reservationcode
            new TranslationSeedItem("entity.materialdocumentitem.reservationcode", "en-US", "预留编号_us", "预留编号"),
            // entity.materialdocumentitem.reservationcode
            new TranslationSeedItem("entity.materialdocumentitem.reservationcode", "ja-JP", "预留编号_jp", "预留编号"),
            // entity.materialdocumentitem.reservationcode
            new TranslationSeedItem("entity.materialdocumentitem.reservationcode", "zh-CN", "预留编号", "预留编号"),
            // entity.materialdocumentitem.reservationcode
            new TranslationSeedItem("entity.materialdocumentitem.reservationcode", "zh-HK", "预留编号_hk", "预留编号"),

            // entity.materialdocumentitem.reservationitem
            new TranslationSeedItem("entity.materialdocumentitem.reservationitem", "en-US", "项目编号库存转储预留_us", "项目编号库存转储预留"),
            // entity.materialdocumentitem.reservationitem
            new TranslationSeedItem("entity.materialdocumentitem.reservationitem", "ja-JP", "项目编号库存转储预留_jp", "项目编号库存转储预留"),
            // entity.materialdocumentitem.reservationitem
            new TranslationSeedItem("entity.materialdocumentitem.reservationitem", "zh-CN", "项目编号库存转储预留", "项目编号库存转储预留"),
            // entity.materialdocumentitem.reservationitem
            new TranslationSeedItem("entity.materialdocumentitem.reservationitem", "zh-HK", "项目编号库存转储预留_hk", "项目编号库存转储预留"),

            // entity.materialdocumentitem.finalissueflag
            new TranslationSeedItem("entity.materialdocumentitem.finalissueflag", "en-US", "最终发货标识_us", "最终发货标识"),
            // entity.materialdocumentitem.finalissueflag
            new TranslationSeedItem("entity.materialdocumentitem.finalissueflag", "ja-JP", "最终发货标识_jp", "最终发货标识"),
            // entity.materialdocumentitem.finalissueflag
            new TranslationSeedItem("entity.materialdocumentitem.finalissueflag", "zh-CN", "最终发货标识", "最终发货标识"),
            // entity.materialdocumentitem.finalissueflag
            new TranslationSeedItem("entity.materialdocumentitem.finalissueflag", "zh-HK", "最终发货标识_hk", "最终发货标识"),

            // entity.materialdocumentitem.reservationquantity
            new TranslationSeedItem("entity.materialdocumentitem.reservationquantity", "en-US", "预留已处理数量_us", "预留已处理数量"),
            // entity.materialdocumentitem.reservationquantity
            new TranslationSeedItem("entity.materialdocumentitem.reservationquantity", "ja-JP", "预留已处理数量_jp", "预留已处理数量"),
            // entity.materialdocumentitem.reservationquantity
            new TranslationSeedItem("entity.materialdocumentitem.reservationquantity", "zh-CN", "预留已处理数量", "预留已处理数量"),
            // entity.materialdocumentitem.reservationquantity
            new TranslationSeedItem("entity.materialdocumentitem.reservationquantity", "zh-HK", "预留已处理数量_hk", "预留已处理数量"),

            // entity.materialdocumentitem.receivingmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingmaterialcode", "en-US", "接收物料_us", "接收物料"),
            // entity.materialdocumentitem.receivingmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingmaterialcode", "ja-JP", "接收物料_jp", "接收物料"),
            // entity.materialdocumentitem.receivingmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingmaterialcode", "zh-CN", "接收物料", "接收物料"),
            // entity.materialdocumentitem.receivingmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingmaterialcode", "zh-HK", "接收物料_hk", "接收物料"),

            // entity.materialdocumentitem.receivingplantcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingplantcode", "en-US", "收货工厂_us", "收货工厂"),
            // entity.materialdocumentitem.receivingplantcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingplantcode", "ja-JP", "收货工厂_jp", "收货工厂"),
            // entity.materialdocumentitem.receivingplantcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingplantcode", "zh-CN", "收货工厂", "收货工厂"),
            // entity.materialdocumentitem.receivingplantcode
            new TranslationSeedItem("entity.materialdocumentitem.receivingplantcode", "zh-HK", "收货工厂_hk", "收货工厂"),

            // entity.materialdocumentitem.receivingwarehousecode
            new TranslationSeedItem("entity.materialdocumentitem.receivingwarehousecode", "en-US", "收货库存地点_us", "收货库存地点"),
            // entity.materialdocumentitem.receivingwarehousecode
            new TranslationSeedItem("entity.materialdocumentitem.receivingwarehousecode", "ja-JP", "收货库存地点_jp", "收货库存地点"),
            // entity.materialdocumentitem.receivingwarehousecode
            new TranslationSeedItem("entity.materialdocumentitem.receivingwarehousecode", "zh-CN", "收货库存地点", "收货库存地点"),
            // entity.materialdocumentitem.receivingwarehousecode
            new TranslationSeedItem("entity.materialdocumentitem.receivingwarehousecode", "zh-HK", "收货库存地点_hk", "收货库存地点"),

            // entity.materialdocumentitem.profitcentercode
            new TranslationSeedItem("entity.materialdocumentitem.profitcentercode", "en-US", "利润中心_us", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.materialdocumentitem.profitcentercode
            new TranslationSeedItem("entity.materialdocumentitem.profitcentercode", "ja-JP", "利润中心_jp", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.materialdocumentitem.profitcentercode
            new TranslationSeedItem("entity.materialdocumentitem.profitcentercode", "zh-CN", "利润中心", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),
            // entity.materialdocumentitem.profitcentercode
            new TranslationSeedItem("entity.materialdocumentitem.profitcentercode", "zh-HK", "利润中心_hk", "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）"),

            // entity.materialdocumentitem.valuatedstockquantity
            new TranslationSeedItem("entity.materialdocumentitem.valuatedstockquantity", "en-US", "过帐前总计估价库存_us", "过帐前总计估价库存"),
            // entity.materialdocumentitem.valuatedstockquantity
            new TranslationSeedItem("entity.materialdocumentitem.valuatedstockquantity", "ja-JP", "过帐前总计估价库存_jp", "过帐前总计估价库存"),
            // entity.materialdocumentitem.valuatedstockquantity
            new TranslationSeedItem("entity.materialdocumentitem.valuatedstockquantity", "zh-CN", "过帐前总计估价库存", "过帐前总计估价库存"),
            // entity.materialdocumentitem.valuatedstockquantity
            new TranslationSeedItem("entity.materialdocumentitem.valuatedstockquantity", "zh-HK", "过帐前总计估价库存_hk", "过帐前总计估价库存"),

            // entity.materialdocumentitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.materialdocumentitem.totalvaluatedstockvalue", "en-US", "过帐前总计评估的库存的价值_us", "过帐前总计评估的库存的价值"),
            // entity.materialdocumentitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.materialdocumentitem.totalvaluatedstockvalue", "ja-JP", "过帐前总计评估的库存的价值_jp", "过帐前总计评估的库存的价值"),
            // entity.materialdocumentitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.materialdocumentitem.totalvaluatedstockvalue", "zh-CN", "过帐前总计评估的库存的价值", "过帐前总计评估的库存的价值"),
            // entity.materialdocumentitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.materialdocumentitem.totalvaluatedstockvalue", "zh-HK", "过帐前总计评估的库存的价值_hk", "过帐前总计评估的库存的价值"),

            // entity.materialdocumentitem.pricecontrol
            new TranslationSeedItem("entity.materialdocumentitem.pricecontrol", "en-US", "价格控制_us", "价格控制"),
            // entity.materialdocumentitem.pricecontrol
            new TranslationSeedItem("entity.materialdocumentitem.pricecontrol", "ja-JP", "价格控制_jp", "价格控制"),
            // entity.materialdocumentitem.pricecontrol
            new TranslationSeedItem("entity.materialdocumentitem.pricecontrol", "zh-CN", "价格控制", "价格控制"),
            // entity.materialdocumentitem.pricecontrol
            new TranslationSeedItem("entity.materialdocumentitem.pricecontrol", "zh-HK", "价格控制_hk", "价格控制"),

            // entity.materialdocumentitem.manufacturerpartmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.manufacturerpartmaterialcode", "en-US", "制造商物料编码_us", "制造商物料编码"),
            // entity.materialdocumentitem.manufacturerpartmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.manufacturerpartmaterialcode", "ja-JP", "制造商物料编码_jp", "制造商物料编码"),
            // entity.materialdocumentitem.manufacturerpartmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.manufacturerpartmaterialcode", "zh-CN", "制造商物料编码", "制造商物料编码"),
            // entity.materialdocumentitem.manufacturerpartmaterialcode
            new TranslationSeedItem("entity.materialdocumentitem.manufacturerpartmaterialcode", "zh-HK", "制造商物料编码_hk", "制造商物料编码"),

            // entity.materialdocumentitem.mkpfreferencecode
            new TranslationSeedItem("entity.materialdocumentitem.mkpfreferencecode", "en-US", "参考_us", "参考（最长 32，故 Length=32）"),
            // entity.materialdocumentitem.mkpfreferencecode
            new TranslationSeedItem("entity.materialdocumentitem.mkpfreferencecode", "ja-JP", "参考_jp", "参考（最长 32，故 Length=32）"),
            // entity.materialdocumentitem.mkpfreferencecode
            new TranslationSeedItem("entity.materialdocumentitem.mkpfreferencecode", "zh-CN", "参考", "参考（最长 32，故 Length=32）"),
            // entity.materialdocumentitem.mkpfreferencecode
            new TranslationSeedItem("entity.materialdocumentitem.mkpfreferencecode", "zh-HK", "参考_hk", "参考（最长 32，故 Length=32）"),

            // entity.materialdocumentitem.imdeliverycode
            new TranslationSeedItem("entity.materialdocumentitem.imdeliverycode", "en-US", "交货_us", "交货"),
            // entity.materialdocumentitem.imdeliverycode
            new TranslationSeedItem("entity.materialdocumentitem.imdeliverycode", "ja-JP", "交货_jp", "交货"),
            // entity.materialdocumentitem.imdeliverycode
            new TranslationSeedItem("entity.materialdocumentitem.imdeliverycode", "zh-CN", "交货", "交货"),
            // entity.materialdocumentitem.imdeliverycode
            new TranslationSeedItem("entity.materialdocumentitem.imdeliverycode", "zh-HK", "交货_hk", "交货"),

            // entity.materialdocumentitem.imdeliveryitem
            new TranslationSeedItem("entity.materialdocumentitem.imdeliveryitem", "en-US", "交货项目_us", "交货项目"),
            // entity.materialdocumentitem.imdeliveryitem
            new TranslationSeedItem("entity.materialdocumentitem.imdeliveryitem", "ja-JP", "交货项目_jp", "交货项目"),
            // entity.materialdocumentitem.imdeliveryitem
            new TranslationSeedItem("entity.materialdocumentitem.imdeliveryitem", "zh-CN", "交货项目", "交货项目"),
            // entity.materialdocumentitem.imdeliveryitem
            new TranslationSeedItem("entity.materialdocumentitem.imdeliveryitem", "zh-HK", "交货项目_hk", "交货项目"),

            // entity.materialdocumentitem.postedby
            new TranslationSeedItem("entity.materialdocumentitem.postedby", "en-US", "过账人_us", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.materialdocumentitem.postedby
            new TranslationSeedItem("entity.materialdocumentitem.postedby", "ja-JP", "过账人_jp", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.materialdocumentitem.postedby
            new TranslationSeedItem("entity.materialdocumentitem.postedby", "zh-CN", "过账人", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.materialdocumentitem.postedby
            new TranslationSeedItem("entity.materialdocumentitem.postedby", "zh-HK", "过账人_hk", "过账人（当前登录用户对应的 EmployeeCode）"),

            // entity.materialdocumentitem.isobsolete
            new TranslationSeedItem("entity.materialdocumentitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.materialdocumentitem.isobsolete
            new TranslationSeedItem("entity.materialdocumentitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.materialdocumentitem.isobsolete
            new TranslationSeedItem("entity.materialdocumentitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.materialdocumentitem.isobsolete
            new TranslationSeedItem("entity.materialdocumentitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.materialdocumentitem.materialdocument
            new TranslationSeedItem("entity.materialdocumentitem.materialdocument", "en-US", "物料凭证主表_us", "物料凭证主表"),
            // entity.materialdocumentitem.materialdocument
            new TranslationSeedItem("entity.materialdocumentitem.materialdocument", "ja-JP", "物料凭证主表_jp", "物料凭证主表"),
            // entity.materialdocumentitem.materialdocument
            new TranslationSeedItem("entity.materialdocumentitem.materialdocument", "zh-CN", "物料凭证主表", "物料凭证主表"),
            // entity.materialdocumentitem.materialdocument
            new TranslationSeedItem("entity.materialdocumentitem.materialdocument", "zh-HK", "物料凭证主表_hk", "物料凭证主表"),
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
