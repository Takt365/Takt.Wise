// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseOrderI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseOrder 实体国际化翻译种子（键前缀 entity.purchaseorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseorder 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseOrderTranslations())
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

        TaktLogger.Information("TaktPurchaseOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseorder._self / entity.purchaseorder.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseorder._self
            new TranslationSeedItem("entity.purchaseorder._self", "en-US", "Purchase Order Information_us", "实体名称"),
            // entity.purchaseorder._self
            new TranslationSeedItem("entity.purchaseorder._self", "ja-JP", "Takt采购订单信息_jp", "实体名称"),
            // entity.purchaseorder._self
            new TranslationSeedItem("entity.purchaseorder._self", "zh-CN", "Takt采购订单信息", "实体名称"),
            // entity.purchaseorder._self
            new TranslationSeedItem("entity.purchaseorder._self", "zh-HK", "Takt采购订单信息_hk", "实体名称"),

            // entity.purchaseorder.code
            new TranslationSeedItem("entity.purchaseorder.code", "en-US", "采购订单编码_us", "采购订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）"),
            // entity.purchaseorder.code
            new TranslationSeedItem("entity.purchaseorder.code", "ja-JP", "采购订单编码_jp", "采购订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）"),
            // entity.purchaseorder.code
            new TranslationSeedItem("entity.purchaseorder.code", "zh-CN", "采购订单编码", "采购订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）"),
            // entity.purchaseorder.code
            new TranslationSeedItem("entity.purchaseorder.code", "zh-HK", "采购订单编码_hk", "采购订单编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购订单编码规则生成并展示，非手输；单据类型菜单：采购订单）"),

            // entity.purchaseorder.purchaserequestid
            new TranslationSeedItem("entity.purchaseorder.purchaserequestid", "en-US", "来源采购申请ID_us", "来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),
            // entity.purchaseorder.purchaserequestid
            new TranslationSeedItem("entity.purchaseorder.purchaserequestid", "ja-JP", "来源采购申请ID_jp", "来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),
            // entity.purchaseorder.purchaserequestid
            new TranslationSeedItem("entity.purchaseorder.purchaserequestid", "zh-CN", "来源采购申请ID", "来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),
            // entity.purchaseorder.purchaserequestid
            new TranslationSeedItem("entity.purchaseorder.purchaserequestid", "zh-HK", "来源采购申请ID_hk", "来源采购申请 ID（选项 TaktPurchaseRequests/options；DictValue=Id）"),

            // entity.purchaseorder.purchaserequestcode
            new TranslationSeedItem("entity.purchaseorder.purchaserequestcode", "en-US", "来源采购申请编码_us", "来源采购申请编码（冗余）"),
            // entity.purchaseorder.purchaserequestcode
            new TranslationSeedItem("entity.purchaseorder.purchaserequestcode", "ja-JP", "来源采购申请编码_jp", "来源采购申请编码（冗余）"),
            // entity.purchaseorder.purchaserequestcode
            new TranslationSeedItem("entity.purchaseorder.purchaserequestcode", "zh-CN", "来源采购申请编码", "来源采购申请编码（冗余）"),
            // entity.purchaseorder.purchaserequestcode
            new TranslationSeedItem("entity.purchaseorder.purchaserequestcode", "zh-HK", "来源采购申请编码_hk", "来源采购申请编码（冗余）"),

            // entity.purchaseorder.suppliercode
            new TranslationSeedItem("entity.purchaseorder.suppliercode", "en-US", "供应商编码_us", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseorder.suppliercode
            new TranslationSeedItem("entity.purchaseorder.suppliercode", "ja-JP", "供应商编码_jp", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseorder.suppliercode
            new TranslationSeedItem("entity.purchaseorder.suppliercode", "zh-CN", "供应商编码", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.purchaseorder.suppliercode
            new TranslationSeedItem("entity.purchaseorder.suppliercode", "zh-HK", "供应商编码_hk", "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.purchaseorder.suppliername1
            new TranslationSeedItem("entity.purchaseorder.suppliername1", "en-US", "供应商名称1_us", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseorder.suppliername1
            new TranslationSeedItem("entity.purchaseorder.suppliername1", "ja-JP", "供应商名称1_jp", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseorder.suppliername1
            new TranslationSeedItem("entity.purchaseorder.suppliername1", "zh-CN", "供应商名称1", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseorder.suppliername1
            new TranslationSeedItem("entity.purchaseorder.suppliername1", "zh-HK", "供应商名称1_hk", "供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),

            // entity.purchaseorder.orderdate
            new TranslationSeedItem("entity.purchaseorder.orderdate", "en-US", "订单日期_us", "订单日期"),
            // entity.purchaseorder.orderdate
            new TranslationSeedItem("entity.purchaseorder.orderdate", "ja-JP", "订单日期_jp", "订单日期"),
            // entity.purchaseorder.orderdate
            new TranslationSeedItem("entity.purchaseorder.orderdate", "zh-CN", "订单日期", "订单日期"),
            // entity.purchaseorder.orderdate
            new TranslationSeedItem("entity.purchaseorder.orderdate", "zh-HK", "订单日期_hk", "订单日期"),

            // entity.purchaseorder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseorder.requiredarrivaldate", "en-US", "要求到货日期_us", "要求到货日期"),
            // entity.purchaseorder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseorder.requiredarrivaldate", "ja-JP", "要求到货日期_jp", "要求到货日期"),
            // entity.purchaseorder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseorder.requiredarrivaldate", "zh-CN", "要求到货日期", "要求到货日期"),
            // entity.purchaseorder.requiredarrivaldate
            new TranslationSeedItem("entity.purchaseorder.requiredarrivaldate", "zh-HK", "要求到货日期_hk", "要求到货日期"),

            // entity.purchaseorder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseorder.actualarrivaldate", "en-US", "实际到货日期_us", "实际到货日期"),
            // entity.purchaseorder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseorder.actualarrivaldate", "ja-JP", "实际到货日期_jp", "实际到货日期"),
            // entity.purchaseorder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseorder.actualarrivaldate", "zh-CN", "实际到货日期", "实际到货日期"),
            // entity.purchaseorder.actualarrivaldate
            new TranslationSeedItem("entity.purchaseorder.actualarrivaldate", "zh-HK", "实际到货日期_hk", "实际到货日期"),

            // entity.purchaseorder.purchasegroup
            new TranslationSeedItem("entity.purchaseorder.purchasegroup", "en-US", "采购组代码_us", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseorder.purchasegroup
            new TranslationSeedItem("entity.purchaseorder.purchasegroup", "ja-JP", "采购组代码_jp", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseorder.purchasegroup
            new TranslationSeedItem("entity.purchaseorder.purchasegroup", "zh-CN", "采购组代码", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.purchaseorder.purchasegroup
            new TranslationSeedItem("entity.purchaseorder.purchasegroup", "zh-HK", "采购组代码_hk", "采购组编码（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.purchaseorder.type
            new TranslationSeedItem("entity.purchaseorder.type", "en-US", "采购订单类型_us", "采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）"),
            // entity.purchaseorder.type
            new TranslationSeedItem("entity.purchaseorder.type", "ja-JP", "采购订单类型_jp", "采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）"),
            // entity.purchaseorder.type
            new TranslationSeedItem("entity.purchaseorder.type", "zh-CN", "采购订单类型", "采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）"),
            // entity.purchaseorder.type
            new TranslationSeedItem("entity.purchaseorder.type", "zh-HK", "采购订单类型_hk", "采购订单类型（字典 logistics_procurement_purchase_order_type；与采购申请/询价共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议）"),

            // entity.purchaseorder.paymentterms
            new TranslationSeedItem("entity.purchaseorder.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.purchaseorder.paymentterms
            new TranslationSeedItem("entity.purchaseorder.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.purchaseorder.paymentterms
            new TranslationSeedItem("entity.purchaseorder.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.purchaseorder.paymentterms
            new TranslationSeedItem("entity.purchaseorder.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),

            // entity.purchaseorder.pricingprocedure
            new TranslationSeedItem("entity.purchaseorder.pricingprocedure", "en-US", "定价过程_us", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaseorder.pricingprocedure
            new TranslationSeedItem("entity.purchaseorder.pricingprocedure", "ja-JP", "定价过程_jp", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaseorder.pricingprocedure
            new TranslationSeedItem("entity.purchaseorder.pricingprocedure", "zh-CN", "定价过程", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaseorder.pricingprocedure
            new TranslationSeedItem("entity.purchaseorder.pricingprocedure", "zh-HK", "定价过程_hk", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),

            // entity.purchaseorder.pricingconditioncode
            new TranslationSeedItem("entity.purchaseorder.pricingconditioncode", "en-US", "定价条件编码_us", "定价条件编码"),
            // entity.purchaseorder.pricingconditioncode
            new TranslationSeedItem("entity.purchaseorder.pricingconditioncode", "ja-JP", "定价条件编码_jp", "定价条件编码"),
            // entity.purchaseorder.pricingconditioncode
            new TranslationSeedItem("entity.purchaseorder.pricingconditioncode", "zh-CN", "定价条件编码", "定价条件编码"),
            // entity.purchaseorder.pricingconditioncode
            new TranslationSeedItem("entity.purchaseorder.pricingconditioncode", "zh-HK", "定价条件编码_hk", "定价条件编码"),

            // entity.purchaseorder.totalquantity
            new TranslationSeedItem("entity.purchaseorder.totalquantity", "en-US", "订单总数量_us", "订单总数量（基本单位数量）"),
            // entity.purchaseorder.totalquantity
            new TranslationSeedItem("entity.purchaseorder.totalquantity", "ja-JP", "订单总数量_jp", "订单总数量（基本单位数量）"),
            // entity.purchaseorder.totalquantity
            new TranslationSeedItem("entity.purchaseorder.totalquantity", "zh-CN", "订单总数量", "订单总数量（基本单位数量）"),
            // entity.purchaseorder.totalquantity
            new TranslationSeedItem("entity.purchaseorder.totalquantity", "zh-HK", "订单总数量_hk", "订单总数量（基本单位数量）"),

            // entity.purchaseorder.totalamount
            new TranslationSeedItem("entity.purchaseorder.totalamount", "en-US", "订单总金额_us", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.totalamount
            new TranslationSeedItem("entity.purchaseorder.totalamount", "ja-JP", "订单总金额_jp", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.totalamount
            new TranslationSeedItem("entity.purchaseorder.totalamount", "zh-CN", "订单总金额", "订单总金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.totalamount
            new TranslationSeedItem("entity.purchaseorder.totalamount", "zh-HK", "订单总金额_hk", "订单总金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorder.discountamount
            new TranslationSeedItem("entity.purchaseorder.discountamount", "en-US", "折扣金额_us", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.discountamount
            new TranslationSeedItem("entity.purchaseorder.discountamount", "ja-JP", "折扣金额_jp", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.discountamount
            new TranslationSeedItem("entity.purchaseorder.discountamount", "zh-CN", "折扣金额", "折扣金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.discountamount
            new TranslationSeedItem("entity.purchaseorder.discountamount", "zh-HK", "折扣金额_hk", "折扣金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorder.currencycode
            new TranslationSeedItem("entity.purchaseorder.currencycode", "en-US", "结算币种_us", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseorder.currencycode
            new TranslationSeedItem("entity.purchaseorder.currencycode", "ja-JP", "结算币种_jp", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseorder.currencycode
            new TranslationSeedItem("entity.purchaseorder.currencycode", "zh-CN", "结算币种", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseorder.currencycode
            new TranslationSeedItem("entity.purchaseorder.currencycode", "zh-HK", "结算币种_hk", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),

            // entity.purchaseorder.exchangerate
            new TranslationSeedItem("entity.purchaseorder.exchangerate", "en-US", "汇率_us", "汇率"),
            // entity.purchaseorder.exchangerate
            new TranslationSeedItem("entity.purchaseorder.exchangerate", "ja-JP", "汇率_jp", "汇率"),
            // entity.purchaseorder.exchangerate
            new TranslationSeedItem("entity.purchaseorder.exchangerate", "zh-CN", "汇率", "汇率"),
            // entity.purchaseorder.exchangerate
            new TranslationSeedItem("entity.purchaseorder.exchangerate", "zh-HK", "汇率_hk", "汇率"),

            // entity.purchaseorder.taxcode
            new TranslationSeedItem("entity.purchaseorder.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseorder.taxcode
            new TranslationSeedItem("entity.purchaseorder.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseorder.taxcode
            new TranslationSeedItem("entity.purchaseorder.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseorder.taxcode
            new TranslationSeedItem("entity.purchaseorder.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.purchaseorder.taxrate
            new TranslationSeedItem("entity.purchaseorder.taxrate", "en-US", "税率_us", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaseorder.taxrate
            new TranslationSeedItem("entity.purchaseorder.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaseorder.taxrate
            new TranslationSeedItem("entity.purchaseorder.taxrate", "zh-CN", "税率", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaseorder.taxrate
            new TranslationSeedItem("entity.purchaseorder.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),

            // entity.purchaseorder.taxamount
            new TranslationSeedItem("entity.purchaseorder.taxamount", "en-US", "税费_us", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.taxamount
            new TranslationSeedItem("entity.purchaseorder.taxamount", "ja-JP", "税费_jp", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.taxamount
            new TranslationSeedItem("entity.purchaseorder.taxamount", "zh-CN", "税费", "税费（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.taxamount
            new TranslationSeedItem("entity.purchaseorder.taxamount", "zh-HK", "税费_hk", "税费（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorder.actualamount
            new TranslationSeedItem("entity.purchaseorder.actualamount", "en-US", "订单实付金额_us", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.actualamount
            new TranslationSeedItem("entity.purchaseorder.actualamount", "ja-JP", "订单实付金额_jp", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.actualamount
            new TranslationSeedItem("entity.purchaseorder.actualamount", "zh-CN", "订单实付金额", "订单实付金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.actualamount
            new TranslationSeedItem("entity.purchaseorder.actualamount", "zh-HK", "订单实付金额_hk", "订单实付金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorder.receivedquantity
            new TranslationSeedItem("entity.purchaseorder.receivedquantity", "en-US", "已入库数量_us", "已入库数量（基本单位数量）"),
            // entity.purchaseorder.receivedquantity
            new TranslationSeedItem("entity.purchaseorder.receivedquantity", "ja-JP", "已入库数量_jp", "已入库数量（基本单位数量）"),
            // entity.purchaseorder.receivedquantity
            new TranslationSeedItem("entity.purchaseorder.receivedquantity", "zh-CN", "已入库数量", "已入库数量（基本单位数量）"),
            // entity.purchaseorder.receivedquantity
            new TranslationSeedItem("entity.purchaseorder.receivedquantity", "zh-HK", "已入库数量_hk", "已入库数量（基本单位数量）"),

            // entity.purchaseorder.receivedamount
            new TranslationSeedItem("entity.purchaseorder.receivedamount", "en-US", "已入库金额_us", "已入库金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.receivedamount
            new TranslationSeedItem("entity.purchaseorder.receivedamount", "ja-JP", "已入库金额_jp", "已入库金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.receivedamount
            new TranslationSeedItem("entity.purchaseorder.receivedamount", "zh-CN", "已入库金额", "已入库金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.receivedamount
            new TranslationSeedItem("entity.purchaseorder.receivedamount", "zh-HK", "已入库金额_hk", "已入库金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorder.paidamount
            new TranslationSeedItem("entity.purchaseorder.paidamount", "en-US", "已付款金额_us", "已付款金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.paidamount
            new TranslationSeedItem("entity.purchaseorder.paidamount", "ja-JP", "已付款金额_jp", "已付款金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.paidamount
            new TranslationSeedItem("entity.purchaseorder.paidamount", "zh-CN", "已付款金额", "已付款金额（精确到分，存储为整数，单位为分）"),
            // entity.purchaseorder.paidamount
            new TranslationSeedItem("entity.purchaseorder.paidamount", "zh-HK", "已付款金额_hk", "已付款金额（精确到分，存储为整数，单位为分）"),

            // entity.purchaseorder.paymentmethod
            new TranslationSeedItem("entity.purchaseorder.paymentmethod", "en-US", "支付方式_us", "支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseorder.paymentmethod
            new TranslationSeedItem("entity.purchaseorder.paymentmethod", "ja-JP", "支付方式_jp", "支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseorder.paymentmethod
            new TranslationSeedItem("entity.purchaseorder.paymentmethod", "zh-CN", "支付方式", "支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.purchaseorder.paymentmethod
            new TranslationSeedItem("entity.purchaseorder.paymentmethod", "zh-HK", "支付方式_hk", "支付方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.purchaseorder.deliverymethod
            new TranslationSeedItem("entity.purchaseorder.deliverymethod", "en-US", "交货方式_us", "交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）"),
            // entity.purchaseorder.deliverymethod
            new TranslationSeedItem("entity.purchaseorder.deliverymethod", "ja-JP", "交货方式_jp", "交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）"),
            // entity.purchaseorder.deliverymethod
            new TranslationSeedItem("entity.purchaseorder.deliverymethod", "zh-CN", "交货方式", "交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）"),
            // entity.purchaseorder.deliverymethod
            new TranslationSeedItem("entity.purchaseorder.deliverymethod", "zh-HK", "交货方式_hk", "交货方式（字典 logistics_sales_delivery_method；0=自提，1=送货上门（采购为供应商送货），2=物流配送，3=快递）"),

            // entity.purchaseorder.deliveryaddress
            new TranslationSeedItem("entity.purchaseorder.deliveryaddress", "en-US", "交货地址_us", "交货地址"),
            // entity.purchaseorder.deliveryaddress
            new TranslationSeedItem("entity.purchaseorder.deliveryaddress", "ja-JP", "交货地址_jp", "交货地址"),
            // entity.purchaseorder.deliveryaddress
            new TranslationSeedItem("entity.purchaseorder.deliveryaddress", "zh-CN", "交货地址", "交货地址"),
            // entity.purchaseorder.deliveryaddress
            new TranslationSeedItem("entity.purchaseorder.deliveryaddress", "zh-HK", "交货地址_hk", "交货地址"),

            // entity.purchaseorder.orderstatus
            new TranslationSeedItem("entity.purchaseorder.orderstatus", "en-US", "订单状态_us", "订单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchaseorder.orderstatus
            new TranslationSeedItem("entity.purchaseorder.orderstatus", "ja-JP", "订单状态_jp", "订单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchaseorder.orderstatus
            new TranslationSeedItem("entity.purchaseorder.orderstatus", "zh-CN", "订单状态", "订单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchaseorder.orderstatus
            new TranslationSeedItem("entity.purchaseorder.orderstatus", "zh-HK", "订单状态_hk", "订单状态（字典 sys_normal_disable；1=启用，0=禁用）"),

            // entity.purchaseorder.deliverystatus
            new TranslationSeedItem("entity.purchaseorder.deliverystatus", "en-US", "交货状态_us", "交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseorder.deliverystatus
            new TranslationSeedItem("entity.purchaseorder.deliverystatus", "ja-JP", "交货状态_jp", "交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseorder.deliverystatus
            new TranslationSeedItem("entity.purchaseorder.deliverystatus", "zh-CN", "交货状态", "交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）"),
            // entity.purchaseorder.deliverystatus
            new TranslationSeedItem("entity.purchaseorder.deliverystatus", "zh-HK", "交货状态_hk", "交货状态（字典 logistics_sales_delivery_status；0=未交货，1=部分交货，2=全部交货）"),

            // entity.purchaseorder.items
            new TranslationSeedItem("entity.purchaseorder.items", "en-US", "订单明细列表_us", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.purchaseorder.items
            new TranslationSeedItem("entity.purchaseorder.items", "ja-JP", "订单明细列表_jp", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.purchaseorder.items
            new TranslationSeedItem("entity.purchaseorder.items", "zh-CN", "订单明细列表", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
            // entity.purchaseorder.items
            new TranslationSeedItem("entity.purchaseorder.items", "zh-HK", "订单明细列表_hk", "订单明细列表（主子表关系，一个订单可以有多个明细）"),
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
