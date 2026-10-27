// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseInquiry 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPurchaseInquiry 实体国际化翻译种子（键前缀 entity.purchaseinquiry.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseInquiryI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseInquiry 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseinquiry 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseInquiryTranslations())
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

        TaktLogger.Information("TaktPurchaseInquiry 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseInquiry 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseinquiry._self / entity.purchaseinquiry.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseInquiryTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseinquiry._self
            new TranslationSeedItem("entity.purchaseinquiry._self", "en-US", "Purchase Inquiry Information_us", "实体名称"),
            // entity.purchaseinquiry._self
            new TranslationSeedItem("entity.purchaseinquiry._self", "ja-JP", "采购询价信息_jp", "实体名称"),
            // entity.purchaseinquiry._self
            new TranslationSeedItem("entity.purchaseinquiry._self", "zh-CN", "采购询价信息", "实体名称"),
            // entity.purchaseinquiry._self
            new TranslationSeedItem("entity.purchaseinquiry._self", "zh-HK", "采购询价信息_hk", "实体名称"),

            // entity.purchaseinquiry.code
            new TranslationSeedItem("entity.purchaseinquiry.code", "en-US", "采购询价编码_us", "采购询价编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购询价编码规则生成并展示，非手输；单据类型菜单：采购询价）"),
            // entity.purchaseinquiry.code
            new TranslationSeedItem("entity.purchaseinquiry.code", "ja-JP", "采购询价编码_jp", "采购询价编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购询价编码规则生成并展示，非手输；单据类型菜单：采购询价）"),
            // entity.purchaseinquiry.code
            new TranslationSeedItem("entity.purchaseinquiry.code", "zh-CN", "采购询价编码", "采购询价编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购询价编码规则生成并展示，非手输；单据类型菜单：采购询价）"),
            // entity.purchaseinquiry.code
            new TranslationSeedItem("entity.purchaseinquiry.code", "zh-HK", "采购询价编码_hk", "采购询价编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购询价编码规则生成并展示，非手输；单据类型菜单：采购询价）"),

            // entity.purchaseinquiry.inquirydate
            new TranslationSeedItem("entity.purchaseinquiry.inquirydate", "en-US", "询价日期_us", "询价日期"),
            // entity.purchaseinquiry.inquirydate
            new TranslationSeedItem("entity.purchaseinquiry.inquirydate", "ja-JP", "询价日期_jp", "询价日期"),
            // entity.purchaseinquiry.inquirydate
            new TranslationSeedItem("entity.purchaseinquiry.inquirydate", "zh-CN", "询价日期", "询价日期"),
            // entity.purchaseinquiry.inquirydate
            new TranslationSeedItem("entity.purchaseinquiry.inquirydate", "zh-HK", "询价日期_hk", "询价日期"),

            // entity.purchaseinquiry.quotedeadlinedate
            new TranslationSeedItem("entity.purchaseinquiry.quotedeadlinedate", "en-US", "报价截止日期_us", "报价截止日期"),
            // entity.purchaseinquiry.quotedeadlinedate
            new TranslationSeedItem("entity.purchaseinquiry.quotedeadlinedate", "ja-JP", "报价截止日期_jp", "报价截止日期"),
            // entity.purchaseinquiry.quotedeadlinedate
            new TranslationSeedItem("entity.purchaseinquiry.quotedeadlinedate", "zh-CN", "报价截止日期", "报价截止日期"),
            // entity.purchaseinquiry.quotedeadlinedate
            new TranslationSeedItem("entity.purchaseinquiry.quotedeadlinedate", "zh-HK", "报价截止日期_hk", "报价截止日期"),

            // entity.purchaseinquiry.inquiryemployeeid
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeeid", "en-US", "询价人员工ID_us", "询价人员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.purchaseinquiry.inquiryemployeeid
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeeid", "ja-JP", "询价人员工ID_jp", "询价人员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.purchaseinquiry.inquiryemployeeid
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeeid", "zh-CN", "询价人员工ID", "询价人员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.purchaseinquiry.inquiryemployeeid
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeeid", "zh-HK", "询价人员工ID_hk", "询价人员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.purchaseinquiry.inquiryemployeename
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeename", "en-US", "询价人名称_us", "询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.purchaseinquiry.inquiryemployeename
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeename", "ja-JP", "询价人名称_jp", "询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.purchaseinquiry.inquiryemployeename
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeename", "zh-CN", "询价人名称", "询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.purchaseinquiry.inquiryemployeename
            new TranslationSeedItem("entity.purchaseinquiry.inquiryemployeename", "zh-HK", "询价人名称_hk", "询价人名称（冗余：按 InquiryEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.purchaseinquiry.suppliercode
            new TranslationSeedItem("entity.purchaseinquiry.suppliercode", "en-US", "询价供应商编码_us", "询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),
            // entity.purchaseinquiry.suppliercode
            new TranslationSeedItem("entity.purchaseinquiry.suppliercode", "ja-JP", "询价供应商编码_jp", "询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),
            // entity.purchaseinquiry.suppliercode
            new TranslationSeedItem("entity.purchaseinquiry.suppliercode", "zh-CN", "询价供应商编码", "询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),
            // entity.purchaseinquiry.suppliercode
            new TranslationSeedItem("entity.purchaseinquiry.suppliercode", "zh-HK", "询价供应商编码_hk", "询价供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；一单一供应商，明细禁止再挂供应商）"),

            // entity.purchaseinquiry.suppliername1
            new TranslationSeedItem("entity.purchaseinquiry.suppliername1", "en-US", "询价供应商名称1_us", "询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseinquiry.suppliername1
            new TranslationSeedItem("entity.purchaseinquiry.suppliername1", "ja-JP", "询价供应商名称1_jp", "询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseinquiry.suppliername1
            new TranslationSeedItem("entity.purchaseinquiry.suppliername1", "zh-CN", "询价供应商名称1", "询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),
            // entity.purchaseinquiry.suppliername1
            new TranslationSeedItem("entity.purchaseinquiry.suppliername1", "zh-HK", "询价供应商名称1_hk", "询价供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）"),

            // entity.purchaseinquiry.type
            new TranslationSeedItem("entity.purchaseinquiry.type", "en-US", "采购询价类型_us", "采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）"),
            // entity.purchaseinquiry.type
            new TranslationSeedItem("entity.purchaseinquiry.type", "ja-JP", "采购询价类型_jp", "采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）"),
            // entity.purchaseinquiry.type
            new TranslationSeedItem("entity.purchaseinquiry.type", "zh-CN", "采购询价类型", "采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）"),
            // entity.purchaseinquiry.type
            new TranslationSeedItem("entity.purchaseinquiry.type", "zh-HK", "采购询价类型_hk", "采购询价类型（字典 logistics_procurement_purchase_order_type；与采购订单/申请共用；DictValue=A-AB/A-AN/B-FO/B-NB/B-RV/F-DB/F-EUB/F-FO/F-NB/F-UB/K-MK/K-WK/L-LP/L-LPA/L-LU；ExtLabel=凭证类别 A询价/B申请/F订单/K合同/L计划协议；询价默认 A-AN）"),

            // entity.purchaseinquiry.pricingprocedure
            new TranslationSeedItem("entity.purchaseinquiry.pricingprocedure", "en-US", "定价过程_us", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaseinquiry.pricingprocedure
            new TranslationSeedItem("entity.purchaseinquiry.pricingprocedure", "ja-JP", "定价过程_jp", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaseinquiry.pricingprocedure
            new TranslationSeedItem("entity.purchaseinquiry.pricingprocedure", "zh-CN", "定价过程", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),
            // entity.purchaseinquiry.pricingprocedure
            new TranslationSeedItem("entity.purchaseinquiry.pricingprocedure", "zh-HK", "定价过程_hk", "定价过程（字典 logistics_procurement_pricing_procedure；DictValue=ZRM001/ZRM002/RM0000～RMREGU；ExtLabel=A；ExtValue=M；默认 ZRM001）"),

            // entity.purchaseinquiry.pricingconditioncode
            new TranslationSeedItem("entity.purchaseinquiry.pricingconditioncode", "en-US", "定价条件编码_us", "定价条件编码"),
            // entity.purchaseinquiry.pricingconditioncode
            new TranslationSeedItem("entity.purchaseinquiry.pricingconditioncode", "ja-JP", "定价条件编码_jp", "定价条件编码"),
            // entity.purchaseinquiry.pricingconditioncode
            new TranslationSeedItem("entity.purchaseinquiry.pricingconditioncode", "zh-CN", "定价条件编码", "定价条件编码"),
            // entity.purchaseinquiry.pricingconditioncode
            new TranslationSeedItem("entity.purchaseinquiry.pricingconditioncode", "zh-HK", "定价条件编码_hk", "定价条件编码"),

            // entity.purchaseinquiry.currencycode
            new TranslationSeedItem("entity.purchaseinquiry.currencycode", "en-US", "结算币种_us", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseinquiry.currencycode
            new TranslationSeedItem("entity.purchaseinquiry.currencycode", "ja-JP", "结算币种_jp", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseinquiry.currencycode
            new TranslationSeedItem("entity.purchaseinquiry.currencycode", "zh-CN", "结算币种", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),
            // entity.purchaseinquiry.currencycode
            new TranslationSeedItem("entity.purchaseinquiry.currencycode", "zh-HK", "结算币种_hk", "结算币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；一单一币种）"),

            // entity.purchaseinquiry.taxcode
            new TranslationSeedItem("entity.purchaseinquiry.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinquiry.taxcode
            new TranslationSeedItem("entity.purchaseinquiry.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinquiry.taxcode
            new TranslationSeedItem("entity.purchaseinquiry.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.purchaseinquiry.taxcode
            new TranslationSeedItem("entity.purchaseinquiry.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.purchaseinquiry.taxrate
            new TranslationSeedItem("entity.purchaseinquiry.taxrate", "en-US", "税率_us", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaseinquiry.taxrate
            new TranslationSeedItem("entity.purchaseinquiry.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaseinquiry.taxrate
            new TranslationSeedItem("entity.purchaseinquiry.taxrate", "zh-CN", "税率", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),
            // entity.purchaseinquiry.taxrate
            new TranslationSeedItem("entity.purchaseinquiry.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；一单一税率；由税码 TaxCode 回填，如 J2→13）"),

            // entity.purchaseinquiry.taxamount
            new TranslationSeedItem("entity.purchaseinquiry.taxamount", "en-US", "税费_us", "税费"),
            // entity.purchaseinquiry.taxamount
            new TranslationSeedItem("entity.purchaseinquiry.taxamount", "ja-JP", "税费_jp", "税费"),
            // entity.purchaseinquiry.taxamount
            new TranslationSeedItem("entity.purchaseinquiry.taxamount", "zh-CN", "税费", "税费"),
            // entity.purchaseinquiry.taxamount
            new TranslationSeedItem("entity.purchaseinquiry.taxamount", "zh-HK", "税费_hk", "税费"),

            // entity.purchaseinquiry.paymentmode
            new TranslationSeedItem("entity.purchaseinquiry.paymentmode", "en-US", "付款方式_us", "付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）"),
            // entity.purchaseinquiry.paymentmode
            new TranslationSeedItem("entity.purchaseinquiry.paymentmode", "ja-JP", "付款方式_jp", "付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）"),
            // entity.purchaseinquiry.paymentmode
            new TranslationSeedItem("entity.purchaseinquiry.paymentmode", "zh-CN", "付款方式", "付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）"),
            // entity.purchaseinquiry.paymentmode
            new TranslationSeedItem("entity.purchaseinquiry.paymentmode", "zh-HK", "付款方式_hk", "付款方式（字典 logistics_procurement_payment_mode：vendorpay=供应商付款，employeereimburse=员工报销）"),

            // entity.purchaseinquiry.chainscheme
            new TranslationSeedItem("entity.purchaseinquiry.chainscheme", "en-US", "采购链路方案_us", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）"),
            // entity.purchaseinquiry.chainscheme
            new TranslationSeedItem("entity.purchaseinquiry.chainscheme", "ja-JP", "采购链路方案_jp", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）"),
            // entity.purchaseinquiry.chainscheme
            new TranslationSeedItem("entity.purchaseinquiry.chainscheme", "zh-CN", "采购链路方案", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）"),
            // entity.purchaseinquiry.chainscheme
            new TranslationSeedItem("entity.purchaseinquiry.chainscheme", "zh-HK", "采购链路方案_hk", "采购链路方案（字典 logistics_procurement_chain_scheme；1=方案一含报销，2=方案二仅 PO）"),

            // entity.purchaseinquiry.totalquantity
            new TranslationSeedItem("entity.purchaseinquiry.totalquantity", "en-US", "询价总数量_us", "询价总数量（基本单位数量）"),
            // entity.purchaseinquiry.totalquantity
            new TranslationSeedItem("entity.purchaseinquiry.totalquantity", "ja-JP", "询价总数量_jp", "询价总数量（基本单位数量）"),
            // entity.purchaseinquiry.totalquantity
            new TranslationSeedItem("entity.purchaseinquiry.totalquantity", "zh-CN", "询价总数量", "询价总数量（基本单位数量）"),
            // entity.purchaseinquiry.totalquantity
            new TranslationSeedItem("entity.purchaseinquiry.totalquantity", "zh-HK", "询价总数量_hk", "询价总数量（基本单位数量）"),

            // entity.purchaseinquiry.totalamount
            new TranslationSeedItem("entity.purchaseinquiry.totalamount", "en-US", "询价总金额_us", "询价总金额"),
            // entity.purchaseinquiry.totalamount
            new TranslationSeedItem("entity.purchaseinquiry.totalamount", "ja-JP", "询价总金额_jp", "询价总金额"),
            // entity.purchaseinquiry.totalamount
            new TranslationSeedItem("entity.purchaseinquiry.totalamount", "zh-CN", "询价总金额", "询价总金额"),
            // entity.purchaseinquiry.totalamount
            new TranslationSeedItem("entity.purchaseinquiry.totalamount", "zh-HK", "询价总金额_hk", "询价总金额"),

            // entity.purchaseinquiry.convertedquantity
            new TranslationSeedItem("entity.purchaseinquiry.convertedquantity", "en-US", "已转价格数量_us", "已转价格数量（基本单位数量）"),
            // entity.purchaseinquiry.convertedquantity
            new TranslationSeedItem("entity.purchaseinquiry.convertedquantity", "ja-JP", "已转价格数量_jp", "已转价格数量（基本单位数量）"),
            // entity.purchaseinquiry.convertedquantity
            new TranslationSeedItem("entity.purchaseinquiry.convertedquantity", "zh-CN", "已转价格数量", "已转价格数量（基本单位数量）"),
            // entity.purchaseinquiry.convertedquantity
            new TranslationSeedItem("entity.purchaseinquiry.convertedquantity", "zh-HK", "已转价格数量_hk", "已转价格数量（基本单位数量）"),

            // entity.purchaseinquiry.convertedamount
            new TranslationSeedItem("entity.purchaseinquiry.convertedamount", "en-US", "已转价格金额_us", "已转价格金额"),
            // entity.purchaseinquiry.convertedamount
            new TranslationSeedItem("entity.purchaseinquiry.convertedamount", "ja-JP", "已转价格金额_jp", "已转价格金额"),
            // entity.purchaseinquiry.convertedamount
            new TranslationSeedItem("entity.purchaseinquiry.convertedamount", "zh-CN", "已转价格金额", "已转价格金额"),
            // entity.purchaseinquiry.convertedamount
            new TranslationSeedItem("entity.purchaseinquiry.convertedamount", "zh-HK", "已转价格金额_hk", "已转价格金额"),

            // entity.purchaseinquiry.inquiryreason
            new TranslationSeedItem("entity.purchaseinquiry.inquiryreason", "en-US", "询价原因_us", "询价原因"),
            // entity.purchaseinquiry.inquiryreason
            new TranslationSeedItem("entity.purchaseinquiry.inquiryreason", "ja-JP", "询价原因_jp", "询价原因"),
            // entity.purchaseinquiry.inquiryreason
            new TranslationSeedItem("entity.purchaseinquiry.inquiryreason", "zh-CN", "询价原因", "询价原因"),
            // entity.purchaseinquiry.inquiryreason
            new TranslationSeedItem("entity.purchaseinquiry.inquiryreason", "zh-HK", "询价原因_hk", "询价原因"),

            // entity.purchaseinquiry.inquirystatus
            new TranslationSeedItem("entity.purchaseinquiry.inquirystatus", "en-US", "询价状态_us", "询价状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchaseinquiry.inquirystatus
            new TranslationSeedItem("entity.purchaseinquiry.inquirystatus", "ja-JP", "询价状态_jp", "询价状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchaseinquiry.inquirystatus
            new TranslationSeedItem("entity.purchaseinquiry.inquirystatus", "zh-CN", "询价状态", "询价状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.purchaseinquiry.inquirystatus
            new TranslationSeedItem("entity.purchaseinquiry.inquirystatus", "zh-HK", "询价状态_hk", "询价状态（字典 sys_normal_disable；1=启用，0=禁用）"),

            // entity.purchaseinquiry.convertedstatus
            new TranslationSeedItem("entity.purchaseinquiry.convertedstatus", "en-US", "转价格状态_us", "转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseinquiry.convertedstatus
            new TranslationSeedItem("entity.purchaseinquiry.convertedstatus", "ja-JP", "转价格状态_jp", "转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseinquiry.convertedstatus
            new TranslationSeedItem("entity.purchaseinquiry.convertedstatus", "zh-CN", "转价格状态", "转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),
            // entity.purchaseinquiry.convertedstatus
            new TranslationSeedItem("entity.purchaseinquiry.convertedstatus", "zh-HK", "转价格状态_hk", "转价格状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）"),

            // entity.purchaseinquiry.items
            new TranslationSeedItem("entity.purchaseinquiry.items", "en-US", "采购询价明细列表_us", "采购询价明细列表（主子表关系）"),
            // entity.purchaseinquiry.items
            new TranslationSeedItem("entity.purchaseinquiry.items", "ja-JP", "采购询价明细列表_jp", "采购询价明细列表（主子表关系）"),
            // entity.purchaseinquiry.items
            new TranslationSeedItem("entity.purchaseinquiry.items", "zh-CN", "采购询价明细列表", "采购询价明细列表（主子表关系）"),
            // entity.purchaseinquiry.items
            new TranslationSeedItem("entity.purchaseinquiry.items", "zh-HK", "采购询价明细列表_hk", "采购询价明细列表（主子表关系）"),
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
