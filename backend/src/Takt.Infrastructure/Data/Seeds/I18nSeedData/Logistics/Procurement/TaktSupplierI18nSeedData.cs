// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktSupplierI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSupplier 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSupplier 实体国际化翻译种子（键前缀 entity.supplier.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSupplierI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSupplier 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 supplier 实体翻译...", tenantCode);

        foreach (var item in GetSupplierTranslations())
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

        TaktLogger.Information("TaktSupplier 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSupplier 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.supplier._self / entity.supplier.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSupplierTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "en-US", "Supplier Information_us", "实体名称"),
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "ja-JP", "Takt供货商信息_jp", "实体名称"),
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "zh-CN", "Takt供货商信息", "实体名称"),
            // entity.supplier._self
            new TranslationSeedItem("entity.supplier._self", "zh-HK", "Takt供货商信息_hk", "实体名称"),

            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "en-US", "供货商编码_us", "供货商编码（唯一索引）"),
            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "ja-JP", "供货商编码_jp", "供货商编码（唯一索引）"),
            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "zh-CN", "供货商编码", "供货商编码（唯一索引）"),
            // entity.supplier.code
            new TranslationSeedItem("entity.supplier.code", "zh-HK", "供货商编码_hk", "供货商编码（唯一索引）"),

            // entity.supplier.name1
            new TranslationSeedItem("entity.supplier.name1", "en-US", "供货商名称1_us", "供货商名称1"),
            // entity.supplier.name1
            new TranslationSeedItem("entity.supplier.name1", "ja-JP", "供货商名称1_jp", "供货商名称1"),
            // entity.supplier.name1
            new TranslationSeedItem("entity.supplier.name1", "zh-CN", "供货商名称1", "供货商名称1"),
            // entity.supplier.name1
            new TranslationSeedItem("entity.supplier.name1", "zh-HK", "供货商名称1_hk", "供货商名称1"),

            // entity.supplier.name2
            new TranslationSeedItem("entity.supplier.name2", "en-US", "供货商名称2_us", "供货商名称2"),
            // entity.supplier.name2
            new TranslationSeedItem("entity.supplier.name2", "ja-JP", "供货商名称2_jp", "供货商名称2"),
            // entity.supplier.name2
            new TranslationSeedItem("entity.supplier.name2", "zh-CN", "供货商名称2", "供货商名称2"),
            // entity.supplier.name2
            new TranslationSeedItem("entity.supplier.name2", "zh-HK", "供货商名称2_hk", "供货商名称2"),

            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "en-US", "供货商简称_us", "供货商简称"),
            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "ja-JP", "供货商简称_jp", "供货商简称"),
            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "zh-CN", "供货商简称", "供货商简称"),
            // entity.supplier.shortname
            new TranslationSeedItem("entity.supplier.shortname", "zh-HK", "供货商简称_hk", "供货商简称"),

            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "en-US", "供货商类型_us", "供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),
            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "ja-JP", "供货商类型_jp", "供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),
            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "zh-CN", "供货商类型", "供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),
            // entity.supplier.type
            new TranslationSeedItem("entity.supplier.type", "zh-HK", "供货商类型_hk", "供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）"),

            // entity.supplier.enterprisenature
            new TranslationSeedItem("entity.supplier.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature）"),
            // entity.supplier.enterprisenature
            new TranslationSeedItem("entity.supplier.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature）"),
            // entity.supplier.enterprisenature
            new TranslationSeedItem("entity.supplier.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature）"),
            // entity.supplier.enterprisenature
            new TranslationSeedItem("entity.supplier.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature）"),

            // entity.supplier.industryattribute
            new TranslationSeedItem("entity.supplier.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute）"),
            // entity.supplier.industryattribute
            new TranslationSeedItem("entity.supplier.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute）"),
            // entity.supplier.industryattribute
            new TranslationSeedItem("entity.supplier.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute）"),
            // entity.supplier.industryattribute
            new TranslationSeedItem("entity.supplier.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute）"),

            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "en-US", "供货商标识_us", "供货商标识（税务登记证号/统一社会信用代码）"),
            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "ja-JP", "供货商标识_jp", "供货商标识（税务登记证号/统一社会信用代码）"),
            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "zh-CN", "供货商标识", "供货商标识（税务登记证号/统一社会信用代码）"),
            // entity.supplier.taxnumber
            new TranslationSeedItem("entity.supplier.taxnumber", "zh-HK", "供货商标识_hk", "供货商标识（税务登记证号/统一社会信用代码）"),

            // entity.supplier.taxcode
            new TranslationSeedItem("entity.supplier.taxcode", "en-US", "税码_us", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.supplier.taxcode
            new TranslationSeedItem("entity.supplier.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.supplier.taxcode
            new TranslationSeedItem("entity.supplier.taxcode", "zh-CN", "税码", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.supplier.taxcode
            new TranslationSeedItem("entity.supplier.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),

            // entity.supplier.taxrate
            new TranslationSeedItem("entity.supplier.taxrate", "en-US", "税率_us", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.supplier.taxrate
            new TranslationSeedItem("entity.supplier.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.supplier.taxrate
            new TranslationSeedItem("entity.supplier.taxrate", "zh-CN", "税率", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.supplier.taxrate
            new TranslationSeedItem("entity.supplier.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),

            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.supplier.registrationcountry
            new TranslationSeedItem("entity.supplier.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.supplier.registrationprovince
            new TranslationSeedItem("entity.supplier.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.supplier.registrationprovince
            new TranslationSeedItem("entity.supplier.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.supplier.registrationprovince
            new TranslationSeedItem("entity.supplier.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.supplier.registrationprovince
            new TranslationSeedItem("entity.supplier.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.supplier.registrationcity
            new TranslationSeedItem("entity.supplier.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.supplier.registrationcity
            new TranslationSeedItem("entity.supplier.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.supplier.registrationcity
            new TranslationSeedItem("entity.supplier.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.supplier.registrationcity
            new TranslationSeedItem("entity.supplier.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.supplier.registrationaddress1
            new TranslationSeedItem("entity.supplier.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.supplier.registrationaddress2
            new TranslationSeedItem("entity.supplier.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "en-US", "供货商电话_us", "供货商电话"),
            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "ja-JP", "供货商电话_jp", "供货商电话"),
            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "zh-CN", "供货商电话", "供货商电话"),
            // entity.supplier.phone
            new TranslationSeedItem("entity.supplier.phone", "zh-HK", "供货商电话_hk", "供货商电话"),

            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "en-US", "供货商传真_us", "供货商传真"),
            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "ja-JP", "供货商传真_jp", "供货商传真"),
            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "zh-CN", "供货商传真", "供货商传真"),
            // entity.supplier.fax
            new TranslationSeedItem("entity.supplier.fax", "zh-HK", "供货商传真_hk", "供货商传真"),

            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "en-US", "供货商邮箱_us", "供货商邮箱"),
            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "ja-JP", "供货商邮箱_jp", "供货商邮箱"),
            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "zh-CN", "供货商邮箱", "供货商邮箱"),
            // entity.supplier.email
            new TranslationSeedItem("entity.supplier.email", "zh-HK", "供货商邮箱_hk", "供货商邮箱"),

            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "en-US", "供货商网站_us", "供货商网站"),
            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "ja-JP", "供货商网站_jp", "供货商网站"),
            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "zh-CN", "供货商网站", "供货商网站"),
            // entity.supplier.website
            new TranslationSeedItem("entity.supplier.website", "zh-HK", "供货商网站_hk", "供货商网站"),

            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.supplier.contactperson
            new TranslationSeedItem("entity.supplier.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "en-US", "联系人电话_us", "联系人电话"),
            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "ja-JP", "联系人电话_jp", "联系人电话"),
            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.supplier.contactphone
            new TranslationSeedItem("entity.supplier.contactphone", "zh-HK", "联系人电话_hk", "联系人电话"),

            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "en-US", "联系人邮箱_us", "联系人邮箱"),
            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "ja-JP", "联系人邮箱_jp", "联系人邮箱"),
            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.supplier.contactemail
            new TranslationSeedItem("entity.supplier.contactemail", "zh-HK", "联系人邮箱_hk", "联系人邮箱"),

            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "en-US", "结算币种代码_us", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "zh-CN", "结算币种代码", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.supplier.currencycode
            new TranslationSeedItem("entity.supplier.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.supplier.reconciliationaccount
            new TranslationSeedItem("entity.supplier.reconciliationaccount", "en-US", "统驭科目_us", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.supplier.reconciliationaccount
            new TranslationSeedItem("entity.supplier.reconciliationaccount", "ja-JP", "统驭科目_jp", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.supplier.reconciliationaccount
            new TranslationSeedItem("entity.supplier.reconciliationaccount", "zh-CN", "统驭科目", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.supplier.reconciliationaccount
            new TranslationSeedItem("entity.supplier.reconciliationaccount", "zh-HK", "统驭科目_hk", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),

            // entity.supplier.customercode
            new TranslationSeedItem("entity.supplier.customercode", "en-US", "客户_us", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.supplier.customercode
            new TranslationSeedItem("entity.supplier.customercode", "ja-JP", "客户_jp", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.supplier.customercode
            new TranslationSeedItem("entity.supplier.customercode", "zh-CN", "客户", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.supplier.customercode
            new TranslationSeedItem("entity.supplier.customercode", "zh-HK", "客户_hk", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.supplier.clearingwithcustomer
            new TranslationSeedItem("entity.supplier.clearingwithcustomer", "en-US", "具有客户的清算_us", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.clearingwithcustomer
            new TranslationSeedItem("entity.supplier.clearingwithcustomer", "ja-JP", "具有客户的清算_jp", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.clearingwithcustomer
            new TranslationSeedItem("entity.supplier.clearingwithcustomer", "zh-CN", "具有客户的清算", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.clearingwithcustomer
            new TranslationSeedItem("entity.supplier.clearingwithcustomer", "zh-HK", "具有客户的清算_hk", "具有客户的清算（字典 sys_yes_no；0=否 1=是）"),

            // entity.supplier.paymentmethod
            new TranslationSeedItem("entity.supplier.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.supplier.paymentmethod
            new TranslationSeedItem("entity.supplier.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.supplier.paymentmethod
            new TranslationSeedItem("entity.supplier.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.supplier.paymentmethod
            new TranslationSeedItem("entity.supplier.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.supplier.paymentterms
            new TranslationSeedItem("entity.supplier.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),

            // entity.supplier.bankcode
            new TranslationSeedItem("entity.supplier.bankcode", "en-US", "银行代码_us", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.supplier.bankcode
            new TranslationSeedItem("entity.supplier.bankcode", "ja-JP", "银行代码_jp", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.supplier.bankcode
            new TranslationSeedItem("entity.supplier.bankcode", "zh-CN", "银行代码", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.supplier.bankcode
            new TranslationSeedItem("entity.supplier.bankcode", "zh-HK", "银行代码_hk", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.supplier.bankaccount
            new TranslationSeedItem("entity.supplier.bankaccount", "en-US", "银行帐号_us", "银行帐号"),
            // entity.supplier.bankaccount
            new TranslationSeedItem("entity.supplier.bankaccount", "ja-JP", "银行帐号_jp", "银行帐号"),
            // entity.supplier.bankaccount
            new TranslationSeedItem("entity.supplier.bankaccount", "zh-CN", "银行帐号", "银行帐号"),
            // entity.supplier.bankaccount
            new TranslationSeedItem("entity.supplier.bankaccount", "zh-HK", "银行帐号_hk", "银行帐号"),

            // entity.supplier.accountholder
            new TranslationSeedItem("entity.supplier.accountholder", "en-US", "帐户持有人_us", "帐户持有人"),
            // entity.supplier.accountholder
            new TranslationSeedItem("entity.supplier.accountholder", "ja-JP", "帐户持有人_jp", "帐户持有人"),
            // entity.supplier.accountholder
            new TranslationSeedItem("entity.supplier.accountholder", "zh-CN", "帐户持有人", "帐户持有人"),
            // entity.supplier.accountholder
            new TranslationSeedItem("entity.supplier.accountholder", "zh-HK", "帐户持有人_hk", "帐户持有人"),

            // entity.supplier.grbasedinvoiceinspection
            new TranslationSeedItem("entity.supplier.grbasedinvoiceinspection", "en-US", "基于收货的发票验证_us", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.grbasedinvoiceinspection
            new TranslationSeedItem("entity.supplier.grbasedinvoiceinspection", "ja-JP", "基于收货的发票验证_jp", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.grbasedinvoiceinspection
            new TranslationSeedItem("entity.supplier.grbasedinvoiceinspection", "zh-CN", "基于收货的发票验证", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.grbasedinvoiceinspection
            new TranslationSeedItem("entity.supplier.grbasedinvoiceinspection", "zh-HK", "基于收货的发票验证_hk", "基于收货的发票验证（字典 sys_yes_no；0=否 1=是）"),

            // entity.supplier.incoterms1
            new TranslationSeedItem("entity.supplier.incoterms1", "en-US", "国际贸易条件1_us", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.supplier.incoterms1
            new TranslationSeedItem("entity.supplier.incoterms1", "ja-JP", "国际贸易条件1_jp", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.supplier.incoterms1
            new TranslationSeedItem("entity.supplier.incoterms1", "zh-CN", "国际贸易条件1", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.supplier.incoterms1
            new TranslationSeedItem("entity.supplier.incoterms1", "zh-HK", "国际贸易条件1_hk", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),

            // entity.supplier.incoterms2
            new TranslationSeedItem("entity.supplier.incoterms2", "en-US", "国际贸易条件2_us", "国际贸易条件2（地点说明）"),
            // entity.supplier.incoterms2
            new TranslationSeedItem("entity.supplier.incoterms2", "ja-JP", "国际贸易条件2_jp", "国际贸易条件2（地点说明）"),
            // entity.supplier.incoterms2
            new TranslationSeedItem("entity.supplier.incoterms2", "zh-CN", "国际贸易条件2", "国际贸易条件2（地点说明）"),
            // entity.supplier.incoterms2
            new TranslationSeedItem("entity.supplier.incoterms2", "zh-HK", "国际贸易条件2_hk", "国际贸易条件2（地点说明）"),

            // entity.supplier.automaticpurchaseorder
            new TranslationSeedItem("entity.supplier.automaticpurchaseorder", "en-US", "自动产生的采购订单_us", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.automaticpurchaseorder
            new TranslationSeedItem("entity.supplier.automaticpurchaseorder", "ja-JP", "自动产生的采购订单_jp", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.automaticpurchaseorder
            new TranslationSeedItem("entity.supplier.automaticpurchaseorder", "zh-CN", "自动产生的采购订单", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.automaticpurchaseorder
            new TranslationSeedItem("entity.supplier.automaticpurchaseorder", "zh-HK", "自动产生的采购订单_hk", "自动产生的采购订单（字典 sys_yes_no；0=否 1=是）"),

            // entity.supplier.pricingdatecontrol
            new TranslationSeedItem("entity.supplier.pricingdatecontrol", "en-US", "定价日期控制_us", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.supplier.pricingdatecontrol
            new TranslationSeedItem("entity.supplier.pricingdatecontrol", "ja-JP", "定价日期控制_jp", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.supplier.pricingdatecontrol
            new TranslationSeedItem("entity.supplier.pricingdatecontrol", "zh-CN", "定价日期控制", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.supplier.pricingdatecontrol
            new TranslationSeedItem("entity.supplier.pricingdatecontrol", "zh-HK", "定价日期控制_hk", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),

            // entity.supplier.purchasegroup
            new TranslationSeedItem("entity.supplier.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.supplier.purchasegroup
            new TranslationSeedItem("entity.supplier.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.supplier.purchasegroup
            new TranslationSeedItem("entity.supplier.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.supplier.purchasegroup
            new TranslationSeedItem("entity.supplier.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.supplier.planneddeliverytimedays
            new TranslationSeedItem("entity.supplier.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天）"),
            // entity.supplier.planneddeliverytimedays
            new TranslationSeedItem("entity.supplier.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天）"),
            // entity.supplier.planneddeliverytimedays
            new TranslationSeedItem("entity.supplier.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天）"),
            // entity.supplier.planneddeliverytimedays
            new TranslationSeedItem("entity.supplier.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天）"),

            // entity.supplier.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.supplier.evaluatedreceiptsettlement", "en-US", "评估收据结算_us", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.supplier.evaluatedreceiptsettlement", "ja-JP", "评估收据结算_jp", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.supplier.evaluatedreceiptsettlement", "zh-CN", "评估收据结算", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),
            // entity.supplier.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.supplier.evaluatedreceiptsettlement", "zh-HK", "评估收据结算_hk", "评估收据结算（字典 sys_yes_no；0=否 1=是）"),

            // entity.supplier.purchasingorganization
            new TranslationSeedItem("entity.supplier.purchasingorganization", "en-US", "采购组织_us", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.supplier.purchasingorganization
            new TranslationSeedItem("entity.supplier.purchasingorganization", "ja-JP", "采购组织_jp", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.supplier.purchasingorganization
            new TranslationSeedItem("entity.supplier.purchasingorganization", "zh-CN", "采购组织", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.supplier.purchasingorganization
            new TranslationSeedItem("entity.supplier.purchasingorganization", "zh-HK", "采购组织_hk", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "en-US", "供货商等级_us", "供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),
            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "ja-JP", "供货商等级_jp", "供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),
            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "zh-CN", "供货商等级", "供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),
            // entity.supplier.level
            new TranslationSeedItem("entity.supplier.level", "zh-HK", "供货商等级_hk", "供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),

            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "en-US", "评价分数_us", "评价分数（0-100分）"),
            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "ja-JP", "评价分数_jp", "评价分数（0-100分）"),
            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.supplier.evaluationscore
            new TranslationSeedItem("entity.supplier.evaluationscore", "zh-HK", "评价分数_hk", "评价分数（0-100分）"),

            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.supplier.sortorder
            new TranslationSeedItem("entity.supplier.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "en-US", "供货商状态_us", "供货商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "ja-JP", "供货商状态_jp", "供货商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "zh-CN", "供货商状态", "供货商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.supplier.status
            new TranslationSeedItem("entity.supplier.status", "zh-HK", "供货商状态_hk", "供货商状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
