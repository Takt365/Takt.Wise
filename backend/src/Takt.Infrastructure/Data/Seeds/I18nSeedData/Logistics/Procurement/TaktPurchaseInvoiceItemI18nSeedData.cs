// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInvoiceItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInvoiceItem 实体国际化翻译种子（键前缀 entity.purchaseinvoiceitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInvoiceItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInvoiceItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinvoiceitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInvoiceItemTranslations())
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

        TaktLogger.Information("TaktPurchaseInvoiceItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInvoiceItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinvoiceitem._self / entity.purchaseinvoiceitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInvoiceItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "en-US", "Purchase Invoice Item Information_us", "实体名称"),
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "ja-JP", "Takt采购发票明细信息_jp", "实体名称"),
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "zh-CN", "Takt采购发票明细信息", "实体名称"),
            // entity.purchaseinvoiceitem._self
            new TranslationSeedItem("entity.purchaseinvoiceitem._self", "zh-HK", "Takt采购发票明细信息_hk", "实体名称"),

            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "en-US", "采购发票ID_us", "采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）"),
            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "ja-JP", "采购发票ID_jp", "采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）"),
            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "zh-CN", "采购发票ID", "采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）"),
            // entity.purchaseinvoiceitem.purchaseinvoiceid
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoiceid", "zh-HK", "采购发票ID_hk", "采购发票ID（选项 TaktPurchaseInvoices/options；DictValue=Id）"),

            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "en-US", "凭证编号_us", "凭证编号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "ja-JP", "凭证编号_jp", "凭证编号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "zh-CN", "凭证编号", "凭证编号（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseinvoiceitem.purchaseinvoicecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoicecode", "zh-HK", "凭证编号_hk", "凭证编号（冗余：按对应 Id 取主数据名称联动）"),

            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "en-US", "凭证项目_us", "发票项目（发票行项目；行号步长生成器用 int，固定步长=10）"),
            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "ja-JP", "凭证项目_jp", "发票项目（发票行项目；行号步长生成器用 int，固定步长=10）"),
            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "zh-CN", "凭证项目", "发票项目（发票行项目；行号步长生成器用 int，固定步长=10）"),
            // entity.purchaseinvoiceitem.linenumber
            new TranslationSeedItem("entity.purchaseinvoiceitem.linenumber", "zh-HK", "凭证项目_hk", "发票项目（发票行项目；行号步长生成器用 int，固定步长=10）"),

            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "en-US", "采购凭证_us", "采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),
            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "ja-JP", "采购凭证_jp", "采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),
            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "zh-CN", "采购凭证", "采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),
            // entity.purchaseinvoiceitem.purchaseordercode
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseordercode", "zh-HK", "采购凭证_hk", "采购凭证（选项 TaktPurchaseOrders/options；DictValue=PurchaseOrderCode）"),

            // entity.purchaseinvoiceitem.purchaseorderitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderitem", "en-US", "项目_us", "项目（采购凭证项目）"),
            // entity.purchaseinvoiceitem.purchaseorderitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderitem", "ja-JP", "项目_jp", "项目（采购凭证项目）"),
            // entity.purchaseinvoiceitem.purchaseorderitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderitem", "zh-CN", "项目", "项目（采购凭证项目）"),
            // entity.purchaseinvoiceitem.purchaseorderitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseorderitem", "zh-HK", "项目_hk", "项目（采购凭证项目）"),

            // entity.purchaseinvoiceitem.accountassignmentseq
            new TranslationSeedItem("entity.purchaseinvoiceitem.accountassignmentseq", "en-US", "科目分配序号_us", "科目分配序号"),
            // entity.purchaseinvoiceitem.accountassignmentseq
            new TranslationSeedItem("entity.purchaseinvoiceitem.accountassignmentseq", "ja-JP", "科目分配序号_jp", "科目分配序号"),
            // entity.purchaseinvoiceitem.accountassignmentseq
            new TranslationSeedItem("entity.purchaseinvoiceitem.accountassignmentseq", "zh-CN", "科目分配序号", "科目分配序号"),
            // entity.purchaseinvoiceitem.accountassignmentseq
            new TranslationSeedItem("entity.purchaseinvoiceitem.accountassignmentseq", "zh-HK", "科目分配序号_hk", "科目分配序号"),

            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "en-US", "物料_us", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "ja-JP", "物料_jp", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "zh-CN", "物料", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.purchaseinvoiceitem.materialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialcode", "zh-HK", "物料_hk", "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.purchaseinvoiceitem.valuationarea
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationarea", "en-US", "评估范围_us", "评估范围"),
            // entity.purchaseinvoiceitem.valuationarea
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationarea", "ja-JP", "评估范围_jp", "评估范围"),
            // entity.purchaseinvoiceitem.valuationarea
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationarea", "zh-CN", "评估范围", "评估范围"),
            // entity.purchaseinvoiceitem.valuationarea
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationarea", "zh-HK", "评估范围_hk", "评估范围"),

            // entity.purchaseinvoiceitem.amount
            new TranslationSeedItem("entity.purchaseinvoiceitem.amount", "en-US", "金额_us", "金额"),
            // entity.purchaseinvoiceitem.amount
            new TranslationSeedItem("entity.purchaseinvoiceitem.amount", "ja-JP", "金额_jp", "金额"),
            // entity.purchaseinvoiceitem.amount
            new TranslationSeedItem("entity.purchaseinvoiceitem.amount", "zh-CN", "金额", "金额"),
            // entity.purchaseinvoiceitem.amount
            new TranslationSeedItem("entity.purchaseinvoiceitem.amount", "zh-HK", "金额_hk", "金额"),

            // entity.purchaseinvoiceitem.debitcreditindicator
            new TranslationSeedItem("entity.purchaseinvoiceitem.debitcreditindicator", "en-US", "借/贷标识_us", "借/贷标识"),
            // entity.purchaseinvoiceitem.debitcreditindicator
            new TranslationSeedItem("entity.purchaseinvoiceitem.debitcreditindicator", "ja-JP", "借/贷标识_jp", "借/贷标识"),
            // entity.purchaseinvoiceitem.debitcreditindicator
            new TranslationSeedItem("entity.purchaseinvoiceitem.debitcreditindicator", "zh-CN", "借/贷标识", "借/贷标识"),
            // entity.purchaseinvoiceitem.debitcreditindicator
            new TranslationSeedItem("entity.purchaseinvoiceitem.debitcreditindicator", "zh-HK", "借/贷标识_hk", "借/贷标识"),

            // entity.purchaseinvoiceitem.taxcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxcode", "en-US", "税码_us", "税码"),
            // entity.purchaseinvoiceitem.taxcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxcode", "ja-JP", "税码_jp", "税码"),
            // entity.purchaseinvoiceitem.taxcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxcode", "zh-CN", "税码", "税码"),
            // entity.purchaseinvoiceitem.taxcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.taxcode", "zh-HK", "税码_hk", "税码"),

            // entity.purchaseinvoiceitem.quantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.quantity", "en-US", "数量_us", "数量"),
            // entity.purchaseinvoiceitem.quantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.quantity", "ja-JP", "数量_jp", "数量"),
            // entity.purchaseinvoiceitem.quantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.quantity", "zh-CN", "数量", "数量"),
            // entity.purchaseinvoiceitem.quantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.quantity", "zh-HK", "数量_hk", "数量"),

            // entity.purchaseinvoiceitem.orderunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.orderunit", "en-US", "订单单位_us", "订单单位"),
            // entity.purchaseinvoiceitem.orderunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.orderunit", "ja-JP", "订单单位_jp", "订单单位"),
            // entity.purchaseinvoiceitem.orderunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.orderunit", "zh-CN", "订单单位", "订单单位"),
            // entity.purchaseinvoiceitem.orderunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.orderunit", "zh-HK", "订单单位_hk", "订单单位"),

            // entity.purchaseinvoiceitem.popricequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.popricequantity", "en-US", "订单价格单位数量_us", "订单价格单位数量"),
            // entity.purchaseinvoiceitem.popricequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.popricequantity", "ja-JP", "订单价格单位数量_jp", "订单价格单位数量"),
            // entity.purchaseinvoiceitem.popricequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.popricequantity", "zh-CN", "订单价格单位数量", "订单价格单位数量"),
            // entity.purchaseinvoiceitem.popricequantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.popricequantity", "zh-HK", "订单价格单位数量_hk", "订单价格单位数量"),

            // entity.purchaseinvoiceitem.popriceunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.popriceunit", "en-US", "订单价格单位_us", "订单价格单位"),
            // entity.purchaseinvoiceitem.popriceunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.popriceunit", "ja-JP", "订单价格单位_jp", "订单价格单位"),
            // entity.purchaseinvoiceitem.popriceunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.popriceunit", "zh-CN", "订单价格单位", "订单价格单位"),
            // entity.purchaseinvoiceitem.popriceunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.popriceunit", "zh-HK", "订单价格单位_hk", "订单价格单位"),

            // entity.purchaseinvoiceitem.valuatedstockquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuatedstockquantity", "en-US", "总库存_us", "总库存"),
            // entity.purchaseinvoiceitem.valuatedstockquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuatedstockquantity", "ja-JP", "总库存_jp", "总库存"),
            // entity.purchaseinvoiceitem.valuatedstockquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuatedstockquantity", "zh-CN", "总库存", "总库存"),
            // entity.purchaseinvoiceitem.valuatedstockquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuatedstockquantity", "zh-HK", "总库存_hk", "总库存"),

            // entity.purchaseinvoiceitem.previousperiodstock
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodstock", "en-US", "上一过账期间库存_us", "上一过账期间库存"),
            // entity.purchaseinvoiceitem.previousperiodstock
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodstock", "ja-JP", "上一过账期间库存_jp", "上一过账期间库存"),
            // entity.purchaseinvoiceitem.previousperiodstock
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodstock", "zh-CN", "上一过账期间库存", "上一过账期间库存"),
            // entity.purchaseinvoiceitem.previousperiodstock
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodstock", "zh-HK", "上一过账期间库存_hk", "上一过账期间库存"),

            // entity.purchaseinvoiceitem.baseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.baseunit", "en-US", "基本计量单位_us", "基本计量单位"),
            // entity.purchaseinvoiceitem.baseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.baseunit", "ja-JP", "基本计量单位_jp", "基本计量单位"),
            // entity.purchaseinvoiceitem.baseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.baseunit", "zh-CN", "基本计量单位", "基本计量单位"),
            // entity.purchaseinvoiceitem.baseunit
            new TranslationSeedItem("entity.purchaseinvoiceitem.baseunit", "zh-HK", "基本计量单位_hk", "基本计量单位"),

            // entity.purchaseinvoiceitem.valuationclass
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationclass", "en-US", "评估类_us", "评估类"),
            // entity.purchaseinvoiceitem.valuationclass
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationclass", "ja-JP", "评估类_jp", "评估类"),
            // entity.purchaseinvoiceitem.valuationclass
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationclass", "zh-CN", "评估类", "评估类"),
            // entity.purchaseinvoiceitem.valuationclass
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuationclass", "zh-HK", "评估类_hk", "评估类"),

            // entity.purchaseinvoiceitem.updatepohistoryflag
            new TranslationSeedItem("entity.purchaseinvoiceitem.updatepohistoryflag", "en-US", "标识: 更新采购订单历史_us", "标识: 更新采购订单历史"),
            // entity.purchaseinvoiceitem.updatepohistoryflag
            new TranslationSeedItem("entity.purchaseinvoiceitem.updatepohistoryflag", "ja-JP", "标识: 更新采购订单历史_jp", "标识: 更新采购订单历史"),
            // entity.purchaseinvoiceitem.updatepohistoryflag
            new TranslationSeedItem("entity.purchaseinvoiceitem.updatepohistoryflag", "zh-CN", "标识: 更新采购订单历史", "标识: 更新采购订单历史"),
            // entity.purchaseinvoiceitem.updatepohistoryflag
            new TranslationSeedItem("entity.purchaseinvoiceitem.updatepohistoryflag", "zh-HK", "标识: 更新采购订单历史_hk", "标识: 更新采购订单历史"),

            // entity.purchaseinvoiceitem.subsequentdebitcredit
            new TranslationSeedItem("entity.purchaseinvoiceitem.subsequentdebitcredit", "en-US", "后续借/贷_us", "后续借/贷"),
            // entity.purchaseinvoiceitem.subsequentdebitcredit
            new TranslationSeedItem("entity.purchaseinvoiceitem.subsequentdebitcredit", "ja-JP", "后续借/贷_jp", "后续借/贷"),
            // entity.purchaseinvoiceitem.subsequentdebitcredit
            new TranslationSeedItem("entity.purchaseinvoiceitem.subsequentdebitcredit", "zh-CN", "后续借/贷", "后续借/贷"),
            // entity.purchaseinvoiceitem.subsequentdebitcredit
            new TranslationSeedItem("entity.purchaseinvoiceitem.subsequentdebitcredit", "zh-HK", "后续借/贷_hk", "后续借/贷"),

            // entity.purchaseinvoiceitem.blockreasonprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonprice", "en-US", "价格冻结原因_us", "价格冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonprice", "ja-JP", "价格冻结原因_jp", "价格冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonprice", "zh-CN", "价格冻结原因", "价格冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonprice
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonprice", "zh-HK", "价格冻结原因_hk", "价格冻结原因"),

            // entity.purchaseinvoiceitem.blockreasonquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquantity", "en-US", "数量冻结原因_us", "数量冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquantity", "ja-JP", "数量冻结原因_jp", "数量冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquantity", "zh-CN", "数量冻结原因", "数量冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonquantity
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquantity", "zh-HK", "数量冻结原因_hk", "数量冻结原因"),

            // entity.purchaseinvoiceitem.blockreasonquality
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquality", "en-US", "质量冻结原因_us", "质量冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonquality
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquality", "ja-JP", "质量冻结原因_jp", "质量冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonquality
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquality", "zh-CN", "质量冻结原因", "质量冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonquality
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonquality", "zh-HK", "质量冻结原因_hk", "质量冻结原因"),

            // entity.purchaseinvoiceitem.blockreasonenhanced
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonenhanced", "en-US", "增强冻结原因_us", "增强冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonenhanced
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonenhanced", "ja-JP", "增强冻结原因_jp", "增强冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonenhanced
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonenhanced", "zh-CN", "增强冻结原因", "增强冻结原因"),
            // entity.purchaseinvoiceitem.blockreasonenhanced
            new TranslationSeedItem("entity.purchaseinvoiceitem.blockreasonenhanced", "zh-HK", "增强冻结原因_hk", "增强冻结原因"),

            // entity.purchaseinvoiceitem.valuestring
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuestring", "en-US", "价值串_us", "价值串"),
            // entity.purchaseinvoiceitem.valuestring
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuestring", "ja-JP", "价值串_jp", "价值串"),
            // entity.purchaseinvoiceitem.valuestring
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuestring", "zh-CN", "价值串", "价值串"),
            // entity.purchaseinvoiceitem.valuestring
            new TranslationSeedItem("entity.purchaseinvoiceitem.valuestring", "zh-HK", "价值串_hk", "价值串"),

            // entity.purchaseinvoiceitem.referencecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencecode", "en-US", "参照_us", "参照"),
            // entity.purchaseinvoiceitem.referencecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencecode", "ja-JP", "参照_jp", "参照"),
            // entity.purchaseinvoiceitem.referencecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencecode", "zh-CN", "参照", "参照"),
            // entity.purchaseinvoiceitem.referencecode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencecode", "zh-HK", "参照_hk", "参照"),

            // entity.purchaseinvoiceitem.conditiontype
            new TranslationSeedItem("entity.purchaseinvoiceitem.conditiontype", "en-US", "条件类型_us", "条件类型"),
            // entity.purchaseinvoiceitem.conditiontype
            new TranslationSeedItem("entity.purchaseinvoiceitem.conditiontype", "ja-JP", "条件类型_jp", "条件类型"),
            // entity.purchaseinvoiceitem.conditiontype
            new TranslationSeedItem("entity.purchaseinvoiceitem.conditiontype", "zh-CN", "条件类型", "条件类型"),
            // entity.purchaseinvoiceitem.conditiontype
            new TranslationSeedItem("entity.purchaseinvoiceitem.conditiontype", "zh-HK", "条件类型_hk", "条件类型"),

            // entity.purchaseinvoiceitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.totalvaluatedstockvalue", "en-US", "总价值_us", "总价值"),
            // entity.purchaseinvoiceitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.totalvaluatedstockvalue", "ja-JP", "总价值_jp", "总价值"),
            // entity.purchaseinvoiceitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.totalvaluatedstockvalue", "zh-CN", "总价值", "总价值"),
            // entity.purchaseinvoiceitem.totalvaluatedstockvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.totalvaluatedstockvalue", "zh-HK", "总价值_hk", "总价值"),

            // entity.purchaseinvoiceitem.previousperiodvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodvalue", "en-US", "前期总值_us", "前期总值"),
            // entity.purchaseinvoiceitem.previousperiodvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodvalue", "ja-JP", "前期总值_jp", "前期总值"),
            // entity.purchaseinvoiceitem.previousperiodvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodvalue", "zh-CN", "前期总值", "前期总值"),
            // entity.purchaseinvoiceitem.previousperiodvalue
            new TranslationSeedItem("entity.purchaseinvoiceitem.previousperiodvalue", "zh-HK", "前期总值_hk", "前期总值"),

            // entity.purchaseinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentcode", "en-US", "参考凭证_us", "参考凭证"),
            // entity.purchaseinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentcode", "ja-JP", "参考凭证_jp", "参考凭证"),
            // entity.purchaseinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentcode", "zh-CN", "参考凭证", "参考凭证"),
            // entity.purchaseinvoiceitem.referencedocumentcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentcode", "zh-HK", "参考凭证_hk", "参考凭证"),

            // entity.purchaseinvoiceitem.referencedocumentyear
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentyear", "en-US", "当前期间年_us", "当前期间年"),
            // entity.purchaseinvoiceitem.referencedocumentyear
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentyear", "ja-JP", "当前期间年_jp", "当前期间年"),
            // entity.purchaseinvoiceitem.referencedocumentyear
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentyear", "zh-CN", "当前期间年", "当前期间年"),
            // entity.purchaseinvoiceitem.referencedocumentyear
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentyear", "zh-HK", "当前期间年_hk", "当前期间年"),

            // entity.purchaseinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentitem", "en-US", "参考凭证项目_us", "参考凭证项目"),
            // entity.purchaseinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentitem", "ja-JP", "参考凭证项目_jp", "参考凭证项目"),
            // entity.purchaseinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentitem", "zh-CN", "参考凭证项目", "参考凭证项目"),
            // entity.purchaseinvoiceitem.referencedocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.referencedocumentitem", "zh-HK", "参考凭证项目_hk", "参考凭证项目"),

            // entity.purchaseinvoiceitem.stockmanagedmaterialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.stockmanagedmaterialcode", "en-US", "库存物料_us", "库存物料"),
            // entity.purchaseinvoiceitem.stockmanagedmaterialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.stockmanagedmaterialcode", "ja-JP", "库存物料_jp", "库存物料"),
            // entity.purchaseinvoiceitem.stockmanagedmaterialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.stockmanagedmaterialcode", "zh-CN", "库存物料", "库存物料"),
            // entity.purchaseinvoiceitem.stockmanagedmaterialcode
            new TranslationSeedItem("entity.purchaseinvoiceitem.stockmanagedmaterialcode", "zh-HK", "库存物料_hk", "库存物料"),

            // entity.purchaseinvoiceitem.itemtext
            new TranslationSeedItem("entity.purchaseinvoiceitem.itemtext", "en-US", "文本_us", "文本"),
            // entity.purchaseinvoiceitem.itemtext
            new TranslationSeedItem("entity.purchaseinvoiceitem.itemtext", "ja-JP", "文本_jp", "文本"),
            // entity.purchaseinvoiceitem.itemtext
            new TranslationSeedItem("entity.purchaseinvoiceitem.itemtext", "zh-CN", "文本", "文本"),
            // entity.purchaseinvoiceitem.itemtext
            new TranslationSeedItem("entity.purchaseinvoiceitem.itemtext", "zh-HK", "文本_hk", "文本"),

            // entity.purchaseinvoiceitem.materialdocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialdocumentitem", "en-US", "来自到达的发票的存货过帐行_us", "来自到达的发票的存货过帐行"),
            // entity.purchaseinvoiceitem.materialdocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialdocumentitem", "ja-JP", "来自到达的发票的存货过帐行_jp", "来自到达的发票的存货过帐行"),
            // entity.purchaseinvoiceitem.materialdocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialdocumentitem", "zh-CN", "来自到达的发票的存货过帐行", "来自到达的发票的存货过帐行"),
            // entity.purchaseinvoiceitem.materialdocumentitem
            new TranslationSeedItem("entity.purchaseinvoiceitem.materialdocumentitem", "zh-HK", "来自到达的发票的存货过帐行_hk", "来自到达的发票的存货过帐行"),

            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseinvoiceitem.isobsolete
            new TranslationSeedItem("entity.purchaseinvoiceitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.purchaseinvoiceitem.purchaseinvoice
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoice", "en-US", "采购发票主表_us", "采购发票主表"),
            // entity.purchaseinvoiceitem.purchaseinvoice
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoice", "ja-JP", "采购发票主表_jp", "采购发票主表"),
            // entity.purchaseinvoiceitem.purchaseinvoice
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoice", "zh-CN", "采购发票主表", "采购发票主表"),
            // entity.purchaseinvoiceitem.purchaseinvoice
            new TranslationSeedItem("entity.purchaseinvoiceitem.purchaseinvoice", "zh-HK", "采购发票主表_hk", "采购发票主表"),
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
