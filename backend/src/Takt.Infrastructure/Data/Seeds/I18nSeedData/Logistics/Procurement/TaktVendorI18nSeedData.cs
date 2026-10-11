// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktVendorI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktVendor 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktVendor 实体国际化翻译种子（键前缀 entity.vendor.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktVendorI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktVendor 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 vendor 实体翻译...", tenantCode);

        foreach (var item in GetVendorTranslations())
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

        TaktLogger.Information("TaktVendor 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktVendor 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.vendor._self / entity.vendor.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetVendorTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "en-US", "Vendor Information_us", "实体名称"),
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "ja-JP", "Takt经销商信息_jp", "实体名称"),
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "zh-CN", "Takt经销商信息", "实体名称"),
            // entity.vendor._self
            new TranslationSeedItem("entity.vendor._self", "zh-HK", "Takt经销商信息_hk", "实体名称"),

            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "en-US", "经销商编码_us", "经销商编码（唯一索引）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "ja-JP", "经销商编码_jp", "经销商编码（唯一索引）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "zh-CN", "经销商编码", "经销商编码（唯一索引）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "zh-HK", "经销商编码_hk", "经销商编码（唯一索引）"),

            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "en-US", "经销商名称1_us", "经销商名称1"),
            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "ja-JP", "经销商名称1_jp", "经销商名称1"),
            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "zh-CN", "经销商名称1", "经销商名称1"),
            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "zh-HK", "经销商名称1_hk", "经销商名称1"),

            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "en-US", "经销商名称2_us", "经销商名称2"),
            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "ja-JP", "经销商名称2_jp", "经销商名称2"),
            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "zh-CN", "经销商名称2", "经销商名称2"),
            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "zh-HK", "经销商名称2_hk", "经销商名称2"),

            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "en-US", "经销商简称_us", "经销商简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "ja-JP", "经销商简称_jp", "经销商简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "zh-CN", "经销商简称", "经销商简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "zh-HK", "经销商简称_hk", "经销商简称"),

            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "en-US", "经销商类型_us", "经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "ja-JP", "经销商类型_jp", "经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "zh-CN", "经销商类型", "经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "zh-HK", "经销商类型_hk", "经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）"),

            // entity.vendor.enterprisenature
            new TranslationSeedItem("entity.vendor.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature）"),
            // entity.vendor.enterprisenature
            new TranslationSeedItem("entity.vendor.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature）"),
            // entity.vendor.enterprisenature
            new TranslationSeedItem("entity.vendor.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature）"),
            // entity.vendor.enterprisenature
            new TranslationSeedItem("entity.vendor.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature）"),

            // entity.vendor.industryattribute
            new TranslationSeedItem("entity.vendor.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute）"),
            // entity.vendor.industryattribute
            new TranslationSeedItem("entity.vendor.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute）"),
            // entity.vendor.industryattribute
            new TranslationSeedItem("entity.vendor.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute）"),
            // entity.vendor.industryattribute
            new TranslationSeedItem("entity.vendor.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute）"),

            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "en-US", "经销商标识_us", "经销商标识（税务登记证号/统一社会信用代码）"),
            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "ja-JP", "经销商标识_jp", "经销商标识（税务登记证号/统一社会信用代码）"),
            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "zh-CN", "经销商标识", "经销商标识（税务登记证号/统一社会信用代码）"),
            // entity.vendor.taxnumber
            new TranslationSeedItem("entity.vendor.taxnumber", "zh-HK", "经销商标识_hk", "经销商标识（税务登记证号/统一社会信用代码）"),

            // entity.vendor.taxcode
            new TranslationSeedItem("entity.vendor.taxcode", "en-US", "税码_us", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.vendor.taxcode
            new TranslationSeedItem("entity.vendor.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.vendor.taxcode
            new TranslationSeedItem("entity.vendor.taxcode", "zh-CN", "税码", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.vendor.taxcode
            new TranslationSeedItem("entity.vendor.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),

            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "en-US", "税率_us", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "zh-CN", "税率", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.vendor.taxrate
            new TranslationSeedItem("entity.vendor.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),

            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.registrationcountry
            new TranslationSeedItem("entity.vendor.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.vendor.registrationprovince
            new TranslationSeedItem("entity.vendor.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.vendor.registrationprovince
            new TranslationSeedItem("entity.vendor.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.vendor.registrationprovince
            new TranslationSeedItem("entity.vendor.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.vendor.registrationprovince
            new TranslationSeedItem("entity.vendor.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.vendor.registrationcity
            new TranslationSeedItem("entity.vendor.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.vendor.registrationcity
            new TranslationSeedItem("entity.vendor.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.vendor.registrationcity
            new TranslationSeedItem("entity.vendor.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.vendor.registrationcity
            new TranslationSeedItem("entity.vendor.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.vendor.registrationaddress1
            new TranslationSeedItem("entity.vendor.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.vendor.registrationaddress2
            new TranslationSeedItem("entity.vendor.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "en-US", "经销商电话_us", "经销商电话"),
            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "ja-JP", "经销商电话_jp", "经销商电话"),
            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "zh-CN", "经销商电话", "经销商电话"),
            // entity.vendor.phone
            new TranslationSeedItem("entity.vendor.phone", "zh-HK", "经销商电话_hk", "经销商电话"),

            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "en-US", "经销商传真_us", "经销商传真"),
            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "ja-JP", "经销商传真_jp", "经销商传真"),
            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "zh-CN", "经销商传真", "经销商传真"),
            // entity.vendor.fax
            new TranslationSeedItem("entity.vendor.fax", "zh-HK", "经销商传真_hk", "经销商传真"),

            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "en-US", "经销商邮箱_us", "经销商邮箱"),
            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "ja-JP", "经销商邮箱_jp", "经销商邮箱"),
            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "zh-CN", "经销商邮箱", "经销商邮箱"),
            // entity.vendor.email
            new TranslationSeedItem("entity.vendor.email", "zh-HK", "经销商邮箱_hk", "经销商邮箱"),

            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "en-US", "经销商网站_us", "经销商网站"),
            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "ja-JP", "经销商网站_jp", "经销商网站"),
            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "zh-CN", "经销商网站", "经销商网站"),
            // entity.vendor.website
            new TranslationSeedItem("entity.vendor.website", "zh-HK", "经销商网站_hk", "经销商网站"),

            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "en-US", "联系人电话_us", "联系人电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "ja-JP", "联系人电话_jp", "联系人电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "zh-HK", "联系人电话_hk", "联系人电话"),

            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "en-US", "联系人邮箱_us", "联系人邮箱"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "ja-JP", "联系人邮箱_jp", "联系人邮箱"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "zh-HK", "联系人邮箱_hk", "联系人邮箱"),

            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "en-US", "结算币种代码_us", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "zh-CN", "结算币种代码", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.vendor.currencycode
            new TranslationSeedItem("entity.vendor.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "en-US", "统驭科目_us", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "ja-JP", "统驭科目_jp", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "zh-CN", "统驭科目", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "zh-HK", "统驭科目_hk", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),

            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "en-US", "客户_us", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "ja-JP", "客户_jp", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "zh-CN", "客户", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "zh-HK", "客户_hk", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "en-US", "具有客户的清算_us", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "ja-JP", "具有客户的清算_jp", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "zh-CN", "具有客户的清算", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "zh-HK", "具有客户的清算_hk", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),

            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "en-US", "银行代码_us", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "ja-JP", "银行代码_jp", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "zh-CN", "银行代码", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "zh-HK", "银行代码_hk", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "en-US", "银行帐号_us", "银行帐号"),
            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "ja-JP", "银行帐号_jp", "银行帐号"),
            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "zh-CN", "银行帐号", "银行帐号"),
            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "zh-HK", "银行帐号_hk", "银行帐号"),

            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "en-US", "帐户持有人_us", "帐户持有人"),
            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "ja-JP", "帐户持有人_jp", "帐户持有人"),
            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "zh-CN", "帐户持有人", "帐户持有人"),
            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "zh-HK", "帐户持有人_hk", "帐户持有人"),

            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "en-US", "基于收货的发票验证_us", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "ja-JP", "基于收货的发票验证_jp", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "zh-CN", "基于收货的发票验证", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "zh-HK", "基于收货的发票验证_hk", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "en-US", "国际贸易条件1_us", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "ja-JP", "国际贸易条件1_jp", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "zh-CN", "国际贸易条件1", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "zh-HK", "国际贸易条件1_hk", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),

            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "en-US", "国际贸易条件2_us", "国际贸易条件2（地点说明）"),
            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "ja-JP", "国际贸易条件2_jp", "国际贸易条件2（地点说明）"),
            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "zh-CN", "国际贸易条件2", "国际贸易条件2（地点说明）"),
            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "zh-HK", "国际贸易条件2_hk", "国际贸易条件2（地点说明）"),

            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "en-US", "自动产生的采购订单_us", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "ja-JP", "自动产生的采购订单_jp", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "zh-CN", "自动产生的采购订单", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "zh-HK", "自动产生的采购订单_hk", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "en-US", "定价日期控制_us", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "ja-JP", "定价日期控制_jp", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "zh-CN", "定价日期控制", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "zh-HK", "定价日期控制_hk", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),

            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.vendor.planneddeliverytimedays
            new TranslationSeedItem("entity.vendor.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天）"),
            // entity.vendor.planneddeliverytimedays
            new TranslationSeedItem("entity.vendor.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天）"),
            // entity.vendor.planneddeliverytimedays
            new TranslationSeedItem("entity.vendor.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天）"),
            // entity.vendor.planneddeliverytimedays
            new TranslationSeedItem("entity.vendor.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天）"),

            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "en-US", "评估收据结算_us", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "ja-JP", "评估收据结算_jp", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "zh-CN", "评估收据结算", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "zh-HK", "评估收据结算_hk", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "en-US", "采购组织_us", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "ja-JP", "采购组织_jp", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "zh-CN", "采购组织", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "zh-HK", "采购组织_hk", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "en-US", "信用等级_us", "信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "ja-JP", "信用等级_jp", "信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "zh-CN", "信用等级", "信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "zh-HK", "信用等级_hk", "信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）"),

            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "en-US", "信用额度_us", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "ja-JP", "信用额度_jp", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "zh-CN", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.vendor.creditamount
            new TranslationSeedItem("entity.vendor.creditamount", "zh-HK", "信用额度_hk", "信用额度（精确到分，存储为整数，单位为分）"),

            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "en-US", "授权品牌_us", "授权品牌"),
            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "ja-JP", "授权品牌_jp", "授权品牌"),
            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "zh-CN", "授权品牌", "授权品牌"),
            // entity.vendor.authorizedbrand
            new TranslationSeedItem("entity.vendor.authorizedbrand", "zh-HK", "授权品牌_hk", "授权品牌"),

            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "en-US", "代理区域_us", "代理区域"),
            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "ja-JP", "代理区域_jp", "代理区域"),
            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "zh-CN", "代理区域", "代理区域"),
            // entity.vendor.agentregion
            new TranslationSeedItem("entity.vendor.agentregion", "zh-HK", "代理区域_hk", "代理区域"),

            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "en-US", "经销商等级_us", "经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "ja-JP", "经销商等级_jp", "经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "zh-CN", "经销商等级", "经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "zh-HK", "经销商等级_hk", "经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）"),

            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "en-US", "评价分数_us", "评价分数（0-100分）"),
            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "ja-JP", "评价分数_jp", "评价分数（0-100分）"),
            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.vendor.evaluationscore
            new TranslationSeedItem("entity.vendor.evaluationscore", "zh-HK", "评价分数_hk", "评价分数（0-100分）"),

            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.vendor.sortorder
            new TranslationSeedItem("entity.vendor.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "en-US", "经销商状态_us", "经销商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "ja-JP", "经销商状态_jp", "经销商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "zh-CN", "经销商状态", "经销商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.vendor.status
            new TranslationSeedItem("entity.vendor.status", "zh-HK", "经销商状态_hk", "经销商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
