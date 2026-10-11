// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSalesInvoiceI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesInvoice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSalesInvoice 实体国际化翻译种子（键前缀 entity.salesinvoice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesInvoiceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesInvoice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesinvoice 实体翻译...", tenantCode);

        foreach (var item in GetSalesInvoiceTranslations())
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

        TaktLogger.Information("TaktSalesInvoice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesInvoice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesinvoice._self / entity.salesinvoice.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesInvoiceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "en-US", "Sales Invoice Information_us", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "ja-JP", "Takt销售发票主表信息_jp", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "zh-CN", "Takt销售发票主表信息", "实体名称"),
            // entity.salesinvoice._self
            new TranslationSeedItem("entity.salesinvoice._self", "zh-HK", "Takt销售发票主表信息_hk", "实体名称"),

            // entity.salesinvoice.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.billingdocumentcode", "en-US", "开票凭证_us", "开票凭证"),
            // entity.salesinvoice.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.billingdocumentcode", "ja-JP", "开票凭证_jp", "开票凭证"),
            // entity.salesinvoice.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.billingdocumentcode", "zh-CN", "开票凭证", "开票凭证"),
            // entity.salesinvoice.billingdocumentcode
            new TranslationSeedItem("entity.salesinvoice.billingdocumentcode", "zh-HK", "开票凭证_hk", "开票凭证"),

            // entity.salesinvoice.billingtype
            new TranslationSeedItem("entity.salesinvoice.billingtype", "en-US", "开票类型_us", "开票类型"),
            // entity.salesinvoice.billingtype
            new TranslationSeedItem("entity.salesinvoice.billingtype", "ja-JP", "开票类型_jp", "开票类型"),
            // entity.salesinvoice.billingtype
            new TranslationSeedItem("entity.salesinvoice.billingtype", "zh-CN", "开票类型", "开票类型"),
            // entity.salesinvoice.billingtype
            new TranslationSeedItem("entity.salesinvoice.billingtype", "zh-HK", "开票类型_hk", "开票类型"),

            // entity.salesinvoice.billingcategory
            new TranslationSeedItem("entity.salesinvoice.billingcategory", "en-US", "出具发票类别_us", "出具发票类别"),
            // entity.salesinvoice.billingcategory
            new TranslationSeedItem("entity.salesinvoice.billingcategory", "ja-JP", "出具发票类别_jp", "出具发票类别"),
            // entity.salesinvoice.billingcategory
            new TranslationSeedItem("entity.salesinvoice.billingcategory", "zh-CN", "出具发票类别", "出具发票类别"),
            // entity.salesinvoice.billingcategory
            new TranslationSeedItem("entity.salesinvoice.billingcategory", "zh-HK", "出具发票类别_hk", "出具发票类别"),

            // entity.salesinvoice.documentcategory
            new TranslationSeedItem("entity.salesinvoice.documentcategory", "en-US", "SD 凭证类别_us", "SD 凭证类别"),
            // entity.salesinvoice.documentcategory
            new TranslationSeedItem("entity.salesinvoice.documentcategory", "ja-JP", "SD 凭证类别_jp", "SD 凭证类别"),
            // entity.salesinvoice.documentcategory
            new TranslationSeedItem("entity.salesinvoice.documentcategory", "zh-CN", "SD 凭证类别", "SD 凭证类别"),
            // entity.salesinvoice.documentcategory
            new TranslationSeedItem("entity.salesinvoice.documentcategory", "zh-HK", "SD 凭证类别_hk", "SD 凭证类别"),

            // entity.salesinvoice.currencycode
            new TranslationSeedItem("entity.salesinvoice.currencycode", "en-US", "凭证货币_us", "凭证货币（字典 accounting_financial_currency_code）"),
            // entity.salesinvoice.currencycode
            new TranslationSeedItem("entity.salesinvoice.currencycode", "ja-JP", "凭证货币_jp", "凭证货币（字典 accounting_financial_currency_code）"),
            // entity.salesinvoice.currencycode
            new TranslationSeedItem("entity.salesinvoice.currencycode", "zh-CN", "凭证货币", "凭证货币（字典 accounting_financial_currency_code）"),
            // entity.salesinvoice.currencycode
            new TranslationSeedItem("entity.salesinvoice.currencycode", "zh-HK", "凭证货币_hk", "凭证货币（字典 accounting_financial_currency_code）"),

            // entity.salesinvoice.salesorganization
            new TranslationSeedItem("entity.salesinvoice.salesorganization", "en-US", "销售组织_us", "销售组织"),
            // entity.salesinvoice.salesorganization
            new TranslationSeedItem("entity.salesinvoice.salesorganization", "ja-JP", "销售组织_jp", "销售组织"),
            // entity.salesinvoice.salesorganization
            new TranslationSeedItem("entity.salesinvoice.salesorganization", "zh-CN", "销售组织", "销售组织"),
            // entity.salesinvoice.salesorganization
            new TranslationSeedItem("entity.salesinvoice.salesorganization", "zh-HK", "销售组织_hk", "销售组织"),

            // entity.salesinvoice.distributionchannel
            new TranslationSeedItem("entity.salesinvoice.distributionchannel", "en-US", "分销渠道_us", "分销渠道"),
            // entity.salesinvoice.distributionchannel
            new TranslationSeedItem("entity.salesinvoice.distributionchannel", "ja-JP", "分销渠道_jp", "分销渠道"),
            // entity.salesinvoice.distributionchannel
            new TranslationSeedItem("entity.salesinvoice.distributionchannel", "zh-CN", "分销渠道", "分销渠道"),
            // entity.salesinvoice.distributionchannel
            new TranslationSeedItem("entity.salesinvoice.distributionchannel", "zh-HK", "分销渠道_hk", "分销渠道"),

            // entity.salesinvoice.pricingprocedure
            new TranslationSeedItem("entity.salesinvoice.pricingprocedure", "en-US", "定价过程_us", "定价过程"),
            // entity.salesinvoice.pricingprocedure
            new TranslationSeedItem("entity.salesinvoice.pricingprocedure", "ja-JP", "定价过程_jp", "定价过程"),
            // entity.salesinvoice.pricingprocedure
            new TranslationSeedItem("entity.salesinvoice.pricingprocedure", "zh-CN", "定价过程", "定价过程"),
            // entity.salesinvoice.pricingprocedure
            new TranslationSeedItem("entity.salesinvoice.pricingprocedure", "zh-HK", "定价过程_hk", "定价过程"),

            // entity.salesinvoice.conditioncode
            new TranslationSeedItem("entity.salesinvoice.conditioncode", "en-US", "单据条件号_us", "单据条件号"),
            // entity.salesinvoice.conditioncode
            new TranslationSeedItem("entity.salesinvoice.conditioncode", "ja-JP", "单据条件号_jp", "单据条件号"),
            // entity.salesinvoice.conditioncode
            new TranslationSeedItem("entity.salesinvoice.conditioncode", "zh-CN", "单据条件号", "单据条件号"),
            // entity.salesinvoice.conditioncode
            new TranslationSeedItem("entity.salesinvoice.conditioncode", "zh-HK", "单据条件号_hk", "单据条件号"),

            // entity.salesinvoice.shippingconditions
            new TranslationSeedItem("entity.salesinvoice.shippingconditions", "en-US", "装运条件_us", "装运条件（字典 logistics_sales_shipping_conditions）"),
            // entity.salesinvoice.shippingconditions
            new TranslationSeedItem("entity.salesinvoice.shippingconditions", "ja-JP", "装运条件_jp", "装运条件（字典 logistics_sales_shipping_conditions）"),
            // entity.salesinvoice.shippingconditions
            new TranslationSeedItem("entity.salesinvoice.shippingconditions", "zh-CN", "装运条件", "装运条件（字典 logistics_sales_shipping_conditions）"),
            // entity.salesinvoice.shippingconditions
            new TranslationSeedItem("entity.salesinvoice.shippingconditions", "zh-HK", "装运条件_hk", "装运条件（字典 logistics_sales_shipping_conditions）"),

            // entity.salesinvoice.billingdate
            new TranslationSeedItem("entity.salesinvoice.billingdate", "en-US", "出具发票日期_us", "出具发票日期"),
            // entity.salesinvoice.billingdate
            new TranslationSeedItem("entity.salesinvoice.billingdate", "ja-JP", "出具发票日期_jp", "出具发票日期"),
            // entity.salesinvoice.billingdate
            new TranslationSeedItem("entity.salesinvoice.billingdate", "zh-CN", "出具发票日期", "出具发票日期"),
            // entity.salesinvoice.billingdate
            new TranslationSeedItem("entity.salesinvoice.billingdate", "zh-HK", "出具发票日期_hk", "出具发票日期"),

            // entity.salesinvoice.customergroup
            new TranslationSeedItem("entity.salesinvoice.customergroup", "en-US", "客户组_us", "客户组"),
            // entity.salesinvoice.customergroup
            new TranslationSeedItem("entity.salesinvoice.customergroup", "ja-JP", "客户组_jp", "客户组"),
            // entity.salesinvoice.customergroup
            new TranslationSeedItem("entity.salesinvoice.customergroup", "zh-CN", "客户组", "客户组"),
            // entity.salesinvoice.customergroup
            new TranslationSeedItem("entity.salesinvoice.customergroup", "zh-HK", "客户组_hk", "客户组"),

            // entity.salesinvoice.incoterms1
            new TranslationSeedItem("entity.salesinvoice.incoterms1", "en-US", "国际贸易条件_us", "国际贸易条件"),
            // entity.salesinvoice.incoterms1
            new TranslationSeedItem("entity.salesinvoice.incoterms1", "ja-JP", "国际贸易条件_jp", "国际贸易条件"),
            // entity.salesinvoice.incoterms1
            new TranslationSeedItem("entity.salesinvoice.incoterms1", "zh-CN", "国际贸易条件", "国际贸易条件"),
            // entity.salesinvoice.incoterms1
            new TranslationSeedItem("entity.salesinvoice.incoterms1", "zh-HK", "国际贸易条件_hk", "国际贸易条件"),

            // entity.salesinvoice.incoterms2
            new TranslationSeedItem("entity.salesinvoice.incoterms2", "en-US", "国际贸易条件(部分2)_us", "国际贸易条件(部分2)（最长 28，故 Length=28）"),
            // entity.salesinvoice.incoterms2
            new TranslationSeedItem("entity.salesinvoice.incoterms2", "ja-JP", "国际贸易条件(部分2)_jp", "国际贸易条件(部分2)（最长 28，故 Length=28）"),
            // entity.salesinvoice.incoterms2
            new TranslationSeedItem("entity.salesinvoice.incoterms2", "zh-CN", "国际贸易条件(部分2)", "国际贸易条件(部分2)（最长 28，故 Length=28）"),
            // entity.salesinvoice.incoterms2
            new TranslationSeedItem("entity.salesinvoice.incoterms2", "zh-HK", "国际贸易条件(部分2)_hk", "国际贸易条件(部分2)（最长 28，故 Length=28）"),

            // entity.salesinvoice.postingstatus
            new TranslationSeedItem("entity.salesinvoice.postingstatus", "en-US", "过账状态_us", "过账状态"),
            // entity.salesinvoice.postingstatus
            new TranslationSeedItem("entity.salesinvoice.postingstatus", "ja-JP", "过账状态_jp", "过账状态"),
            // entity.salesinvoice.postingstatus
            new TranslationSeedItem("entity.salesinvoice.postingstatus", "zh-CN", "过账状态", "过账状态"),
            // entity.salesinvoice.postingstatus
            new TranslationSeedItem("entity.salesinvoice.postingstatus", "zh-HK", "过账状态_hk", "过账状态"),

            // entity.salesinvoice.accountingexchangerate
            new TranslationSeedItem("entity.salesinvoice.accountingexchangerate", "en-US", "会计汇率_us", "会计汇率"),
            // entity.salesinvoice.accountingexchangerate
            new TranslationSeedItem("entity.salesinvoice.accountingexchangerate", "ja-JP", "会计汇率_jp", "会计汇率"),
            // entity.salesinvoice.accountingexchangerate
            new TranslationSeedItem("entity.salesinvoice.accountingexchangerate", "zh-CN", "会计汇率", "会计汇率"),
            // entity.salesinvoice.accountingexchangerate
            new TranslationSeedItem("entity.salesinvoice.accountingexchangerate", "zh-HK", "会计汇率_hk", "会计汇率"),

            // entity.salesinvoice.paymentterms
            new TranslationSeedItem("entity.salesinvoice.paymentterms", "en-US", "付款条件_us", "付款条件"),
            // entity.salesinvoice.paymentterms
            new TranslationSeedItem("entity.salesinvoice.paymentterms", "ja-JP", "付款条件_jp", "付款条件"),
            // entity.salesinvoice.paymentterms
            new TranslationSeedItem("entity.salesinvoice.paymentterms", "zh-CN", "付款条件", "付款条件"),
            // entity.salesinvoice.paymentterms
            new TranslationSeedItem("entity.salesinvoice.paymentterms", "zh-HK", "付款条件_hk", "付款条件"),

            // entity.salesinvoice.accountassignmentgroup
            new TranslationSeedItem("entity.salesinvoice.accountassignmentgroup", "en-US", "客户分配帐户组别_us", "客户分配帐户组别"),
            // entity.salesinvoice.accountassignmentgroup
            new TranslationSeedItem("entity.salesinvoice.accountassignmentgroup", "ja-JP", "客户分配帐户组别_jp", "客户分配帐户组别"),
            // entity.salesinvoice.accountassignmentgroup
            new TranslationSeedItem("entity.salesinvoice.accountassignmentgroup", "zh-CN", "客户分配帐户组别", "客户分配帐户组别"),
            // entity.salesinvoice.accountassignmentgroup
            new TranslationSeedItem("entity.salesinvoice.accountassignmentgroup", "zh-HK", "客户分配帐户组别_hk", "客户分配帐户组别"),

            // entity.salesinvoice.countrycode
            new TranslationSeedItem("entity.salesinvoice.countrycode", "en-US", "目的地国家_us", "目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoice.countrycode
            new TranslationSeedItem("entity.salesinvoice.countrycode", "ja-JP", "目的地国家_jp", "目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoice.countrycode
            new TranslationSeedItem("entity.salesinvoice.countrycode", "zh-CN", "目的地国家", "目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoice.countrycode
            new TranslationSeedItem("entity.salesinvoice.countrycode", "zh-HK", "目的地国家_hk", "目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.salesinvoice.netamount
            new TranslationSeedItem("entity.salesinvoice.netamount", "en-US", "净价值_us", "净价值"),
            // entity.salesinvoice.netamount
            new TranslationSeedItem("entity.salesinvoice.netamount", "ja-JP", "净价值_jp", "净价值"),
            // entity.salesinvoice.netamount
            new TranslationSeedItem("entity.salesinvoice.netamount", "zh-CN", "净价值", "净价值"),
            // entity.salesinvoice.netamount
            new TranslationSeedItem("entity.salesinvoice.netamount", "zh-HK", "净价值_hk", "净价值"),

            // entity.salesinvoice.payercode
            new TranslationSeedItem("entity.salesinvoice.payercode", "en-US", "付款方_us", "付款方（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesinvoice.payercode
            new TranslationSeedItem("entity.salesinvoice.payercode", "ja-JP", "付款方_jp", "付款方（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesinvoice.payercode
            new TranslationSeedItem("entity.salesinvoice.payercode", "zh-CN", "付款方", "付款方（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesinvoice.payercode
            new TranslationSeedItem("entity.salesinvoice.payercode", "zh-HK", "付款方_hk", "付款方（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "en-US", "售达方_us", "售达方（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "ja-JP", "售达方_jp", "售达方（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "zh-CN", "售达方", "售达方（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.salesinvoice.customercode
            new TranslationSeedItem("entity.salesinvoice.customercode", "zh-HK", "售达方_hk", "售达方（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.salesinvoice.statisticscurrencycode
            new TranslationSeedItem("entity.salesinvoice.statisticscurrencycode", "en-US", "统计货币_us", "统计货币（字典 accounting_financial_currency_code）"),
            // entity.salesinvoice.statisticscurrencycode
            new TranslationSeedItem("entity.salesinvoice.statisticscurrencycode", "ja-JP", "统计货币_jp", "统计货币（字典 accounting_financial_currency_code）"),
            // entity.salesinvoice.statisticscurrencycode
            new TranslationSeedItem("entity.salesinvoice.statisticscurrencycode", "zh-CN", "统计货币", "统计货币（字典 accounting_financial_currency_code）"),
            // entity.salesinvoice.statisticscurrencycode
            new TranslationSeedItem("entity.salesinvoice.statisticscurrencycode", "zh-HK", "统计货币_hk", "统计货币（字典 accounting_financial_currency_code）"),

            // entity.salesinvoice.foreigntradecode
            new TranslationSeedItem("entity.salesinvoice.foreigntradecode", "en-US", "外贸数据编号_us", "外贸数据编号"),
            // entity.salesinvoice.foreigntradecode
            new TranslationSeedItem("entity.salesinvoice.foreigntradecode", "ja-JP", "外贸数据编号_jp", "外贸数据编号"),
            // entity.salesinvoice.foreigntradecode
            new TranslationSeedItem("entity.salesinvoice.foreigntradecode", "zh-CN", "外贸数据编号", "外贸数据编号"),
            // entity.salesinvoice.foreigntradecode
            new TranslationSeedItem("entity.salesinvoice.foreigntradecode", "zh-HK", "外贸数据编号_hk", "外贸数据编号"),

            // entity.salesinvoice.cancelledbillingdocument
            new TranslationSeedItem("entity.salesinvoice.cancelledbillingdocument", "en-US", "已取消的开票凭证_us", "已取消的开票凭证"),
            // entity.salesinvoice.cancelledbillingdocument
            new TranslationSeedItem("entity.salesinvoice.cancelledbillingdocument", "ja-JP", "已取消的开票凭证_jp", "已取消的开票凭证"),
            // entity.salesinvoice.cancelledbillingdocument
            new TranslationSeedItem("entity.salesinvoice.cancelledbillingdocument", "zh-CN", "已取消的开票凭证", "已取消的开票凭证"),
            // entity.salesinvoice.cancelledbillingdocument
            new TranslationSeedItem("entity.salesinvoice.cancelledbillingdocument", "zh-HK", "已取消的开票凭证_hk", "已取消的开票凭证"),

            // entity.salesinvoice.invoicelisttype
            new TranslationSeedItem("entity.salesinvoice.invoicelisttype", "en-US", "发票清单类型_us", "发票清单类型"),
            // entity.salesinvoice.invoicelisttype
            new TranslationSeedItem("entity.salesinvoice.invoicelisttype", "ja-JP", "发票清单类型_jp", "发票清单类型"),
            // entity.salesinvoice.invoicelisttype
            new TranslationSeedItem("entity.salesinvoice.invoicelisttype", "zh-CN", "发票清单类型", "发票清单类型"),
            // entity.salesinvoice.invoicelisttype
            new TranslationSeedItem("entity.salesinvoice.invoicelisttype", "zh-HK", "发票清单类型_hk", "发票清单类型"),

            // entity.salesinvoice.division
            new TranslationSeedItem("entity.salesinvoice.division", "en-US", "产品组_us", "产品组"),
            // entity.salesinvoice.division
            new TranslationSeedItem("entity.salesinvoice.division", "ja-JP", "产品组_jp", "产品组"),
            // entity.salesinvoice.division
            new TranslationSeedItem("entity.salesinvoice.division", "zh-CN", "产品组", "产品组"),
            // entity.salesinvoice.division
            new TranslationSeedItem("entity.salesinvoice.division", "zh-HK", "产品组_hk", "产品组"),

            // entity.salesinvoice.hierarchytypepricing
            new TranslationSeedItem("entity.salesinvoice.hierarchytypepricing", "en-US", "定价的层次类型_us", "定价的层次类型"),
            // entity.salesinvoice.hierarchytypepricing
            new TranslationSeedItem("entity.salesinvoice.hierarchytypepricing", "ja-JP", "定价的层次类型_jp", "定价的层次类型"),
            // entity.salesinvoice.hierarchytypepricing
            new TranslationSeedItem("entity.salesinvoice.hierarchytypepricing", "zh-CN", "定价的层次类型", "定价的层次类型"),
            // entity.salesinvoice.hierarchytypepricing
            new TranslationSeedItem("entity.salesinvoice.hierarchytypepricing", "zh-HK", "定价的层次类型_hk", "定价的层次类型"),

            // entity.salesinvoice.tradingpartner
            new TranslationSeedItem("entity.salesinvoice.tradingpartner", "en-US", "贸易伙伴_us", "贸易伙伴"),
            // entity.salesinvoice.tradingpartner
            new TranslationSeedItem("entity.salesinvoice.tradingpartner", "ja-JP", "贸易伙伴_jp", "贸易伙伴"),
            // entity.salesinvoice.tradingpartner
            new TranslationSeedItem("entity.salesinvoice.tradingpartner", "zh-CN", "贸易伙伴", "贸易伙伴"),
            // entity.salesinvoice.tradingpartner
            new TranslationSeedItem("entity.salesinvoice.tradingpartner", "zh-HK", "贸易伙伴_hk", "贸易伙伴"),

            // entity.salesinvoice.taxdeparturecountry
            new TranslationSeedItem("entity.salesinvoice.taxdeparturecountry", "en-US", "征税国家_us", "征税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoice.taxdeparturecountry
            new TranslationSeedItem("entity.salesinvoice.taxdeparturecountry", "ja-JP", "征税国家_jp", "征税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoice.taxdeparturecountry
            new TranslationSeedItem("entity.salesinvoice.taxdeparturecountry", "zh-CN", "征税国家", "征税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.salesinvoice.taxdeparturecountry
            new TranslationSeedItem("entity.salesinvoice.taxdeparturecountry", "zh-HK", "征税国家_hk", "征税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.salesinvoice.organizationsalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.organizationsalestaxnumber", "en-US", "组织销售税编号_us", "组织销售税编号"),
            // entity.salesinvoice.organizationsalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.organizationsalestaxnumber", "ja-JP", "组织销售税编号_jp", "组织销售税编号"),
            // entity.salesinvoice.organizationsalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.organizationsalestaxnumber", "zh-CN", "组织销售税编号", "组织销售税编号"),
            // entity.salesinvoice.organizationsalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.organizationsalestaxnumber", "zh-HK", "组织销售税编号_hk", "组织销售税编号"),

            // entity.salesinvoice.countrysalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.countrysalestaxnumber", "en-US", "国家销售税编号_us", "国家销售税编号"),
            // entity.salesinvoice.countrysalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.countrysalestaxnumber", "ja-JP", "国家销售税编号_jp", "国家销售税编号"),
            // entity.salesinvoice.countrysalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.countrysalestaxnumber", "zh-CN", "国家销售税编号", "国家销售税编号"),
            // entity.salesinvoice.countrysalestaxnumber
            new TranslationSeedItem("entity.salesinvoice.countrysalestaxnumber", "zh-HK", "国家销售税编号_hk", "国家销售税编号"),

            // entity.salesinvoice.referencecode
            new TranslationSeedItem("entity.salesinvoice.referencecode", "en-US", "参考_us", "参考（最长 16，故 Length=16）"),
            // entity.salesinvoice.referencecode
            new TranslationSeedItem("entity.salesinvoice.referencecode", "ja-JP", "参考_jp", "参考（最长 16，故 Length=16）"),
            // entity.salesinvoice.referencecode
            new TranslationSeedItem("entity.salesinvoice.referencecode", "zh-CN", "参考", "参考（最长 16，故 Length=16）"),
            // entity.salesinvoice.referencecode
            new TranslationSeedItem("entity.salesinvoice.referencecode", "zh-HK", "参考_hk", "参考（最长 16，故 Length=16）"),

            // entity.salesinvoice.cancelledflag
            new TranslationSeedItem("entity.salesinvoice.cancelledflag", "en-US", "已被取消_us", "已被取消"),
            // entity.salesinvoice.cancelledflag
            new TranslationSeedItem("entity.salesinvoice.cancelledflag", "ja-JP", "已被取消_jp", "已被取消"),
            // entity.salesinvoice.cancelledflag
            new TranslationSeedItem("entity.salesinvoice.cancelledflag", "zh-CN", "已被取消", "已被取消"),
            // entity.salesinvoice.cancelledflag
            new TranslationSeedItem("entity.salesinvoice.cancelledflag", "zh-HK", "已被取消_hk", "已被取消"),

            // entity.salesinvoice.exchangeratedate
            new TranslationSeedItem("entity.salesinvoice.exchangeratedate", "en-US", "换算日期_us", "换算日期"),
            // entity.salesinvoice.exchangeratedate
            new TranslationSeedItem("entity.salesinvoice.exchangeratedate", "ja-JP", "换算日期_jp", "换算日期"),
            // entity.salesinvoice.exchangeratedate
            new TranslationSeedItem("entity.salesinvoice.exchangeratedate", "zh-CN", "换算日期", "换算日期"),
            // entity.salesinvoice.exchangeratedate
            new TranslationSeedItem("entity.salesinvoice.exchangeratedate", "zh-HK", "换算日期_hk", "换算日期"),

            // entity.salesinvoice.paymentreference
            new TranslationSeedItem("entity.salesinvoice.paymentreference", "en-US", "付款参考_us", "付款参考（最长 30，故 Length=30）"),
            // entity.salesinvoice.paymentreference
            new TranslationSeedItem("entity.salesinvoice.paymentreference", "ja-JP", "付款参考_jp", "付款参考（最长 30，故 Length=30）"),
            // entity.salesinvoice.paymentreference
            new TranslationSeedItem("entity.salesinvoice.paymentreference", "zh-CN", "付款参考", "付款参考（最长 30，故 Length=30）"),
            // entity.salesinvoice.paymentreference
            new TranslationSeedItem("entity.salesinvoice.paymentreference", "zh-HK", "付款参考_hk", "付款参考（最长 30，故 Length=30）"),

            // entity.salesinvoice.reversalreason
            new TranslationSeedItem("entity.salesinvoice.reversalreason", "en-US", "冲销原因_us", "冲销原因"),
            // entity.salesinvoice.reversalreason
            new TranslationSeedItem("entity.salesinvoice.reversalreason", "ja-JP", "冲销原因_jp", "冲销原因"),
            // entity.salesinvoice.reversalreason
            new TranslationSeedItem("entity.salesinvoice.reversalreason", "zh-CN", "冲销原因", "冲销原因"),
            // entity.salesinvoice.reversalreason
            new TranslationSeedItem("entity.salesinvoice.reversalreason", "zh-HK", "冲销原因_hk", "冲销原因"),

            // entity.salesinvoice.postedby
            new TranslationSeedItem("entity.salesinvoice.postedby", "en-US", "过账人_us", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.salesinvoice.postedby
            new TranslationSeedItem("entity.salesinvoice.postedby", "ja-JP", "过账人_jp", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.salesinvoice.postedby
            new TranslationSeedItem("entity.salesinvoice.postedby", "zh-CN", "过账人", "过账人（当前登录用户对应的 EmployeeCode）"),
            // entity.salesinvoice.postedby
            new TranslationSeedItem("entity.salesinvoice.postedby", "zh-HK", "过账人_hk", "过账人（当前登录用户对应的 EmployeeCode）"),

            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "en-US", "销售发票明细列表_us", "销售发票明细列表（主子表关系）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "ja-JP", "销售发票明细列表_jp", "销售发票明细列表（主子表关系）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "zh-CN", "销售发票明细列表", "销售发票明细列表（主子表关系）"),
            // entity.salesinvoice.items
            new TranslationSeedItem("entity.salesinvoice.items", "zh-HK", "销售发票明细列表_hk", "销售发票明细列表（主子表关系）"),
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
