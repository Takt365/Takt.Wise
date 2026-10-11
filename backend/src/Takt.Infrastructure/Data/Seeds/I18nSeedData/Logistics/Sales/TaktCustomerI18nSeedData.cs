// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktCustomerI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomer 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCustomer 实体国际化翻译种子（键前缀 entity.customer.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomer 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customer 实体翻译...", tenantCode);

        foreach (var item in GetCustomerTranslations())
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

        TaktLogger.Information("TaktCustomer 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomer 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customer._self / entity.customer.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "en-US", "Customer Information_us", "实体名称"),
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "ja-JP", "Takt客户信息信息_jp", "实体名称"),
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "zh-CN", "Takt客户信息信息", "实体名称"),
            // entity.customer._self
            new TranslationSeedItem("entity.customer._self", "zh-HK", "Takt客户信息信息_hk", "实体名称"),

            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "en-US", "客户编码_us", "客户编码（唯一索引）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "ja-JP", "客户编码_jp", "客户编码（唯一索引）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "zh-CN", "客户编码", "客户编码（唯一索引）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "zh-HK", "客户编码_hk", "客户编码（唯一索引）"),

            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "en-US", "客户名称1_us", "客户名称1"),
            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "ja-JP", "客户名称1_jp", "客户名称1"),
            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "zh-CN", "客户名称1", "客户名称1"),
            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "zh-HK", "客户名称1_hk", "客户名称1"),

            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "en-US", "客户名称2_us", "客户名称2"),
            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "ja-JP", "客户名称2_jp", "客户名称2"),
            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "zh-CN", "客户名称2", "客户名称2"),
            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "zh-HK", "客户名称2_hk", "客户名称2"),

            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "en-US", "客户简称_us", "客户简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "ja-JP", "客户简称_jp", "客户简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "zh-CN", "客户简称", "客户简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "zh-HK", "客户简称_hk", "客户简称"),

            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "en-US", "客户类型_us", "客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "ja-JP", "客户类型_jp", "客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "zh-CN", "客户类型", "客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.customer.type
            new TranslationSeedItem("entity.customer.type", "zh-HK", "客户类型_hk", "客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),

            // entity.customer.enterprisenature
            new TranslationSeedItem("entity.customer.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature）"),
            // entity.customer.enterprisenature
            new TranslationSeedItem("entity.customer.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature）"),
            // entity.customer.enterprisenature
            new TranslationSeedItem("entity.customer.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature）"),
            // entity.customer.enterprisenature
            new TranslationSeedItem("entity.customer.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature）"),

            // entity.customer.industryattribute
            new TranslationSeedItem("entity.customer.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute）"),
            // entity.customer.industryattribute
            new TranslationSeedItem("entity.customer.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute）"),
            // entity.customer.industryattribute
            new TranslationSeedItem("entity.customer.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute）"),
            // entity.customer.industryattribute
            new TranslationSeedItem("entity.customer.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute）"),

            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "en-US", "客户标识_us", "客户标识（税务登记证号/统一社会信用代码）"),
            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "ja-JP", "客户标识_jp", "客户标识（税务登记证号/统一社会信用代码）"),
            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "zh-CN", "客户标识", "客户标识（税务登记证号/统一社会信用代码）"),
            // entity.customer.taxnumber
            new TranslationSeedItem("entity.customer.taxnumber", "zh-HK", "客户标识_hk", "客户标识（税务登记证号/统一社会信用代码）"),

            // entity.customer.taxcode
            new TranslationSeedItem("entity.customer.taxcode", "en-US", "税码_us", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.customer.taxcode
            new TranslationSeedItem("entity.customer.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.customer.taxcode
            new TranslationSeedItem("entity.customer.taxcode", "zh-CN", "税码", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.customer.taxcode
            new TranslationSeedItem("entity.customer.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),

            // entity.customer.taxrate
            new TranslationSeedItem("entity.customer.taxrate", "en-US", "税率_us", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.customer.taxrate
            new TranslationSeedItem("entity.customer.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.customer.taxrate
            new TranslationSeedItem("entity.customer.taxrate", "zh-CN", "税率", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.customer.taxrate
            new TranslationSeedItem("entity.customer.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),

            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.customer.registrationprovince
            new TranslationSeedItem("entity.customer.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.customer.registrationprovince
            new TranslationSeedItem("entity.customer.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.customer.registrationprovince
            new TranslationSeedItem("entity.customer.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.customer.registrationprovince
            new TranslationSeedItem("entity.customer.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.customer.registrationaddress1
            new TranslationSeedItem("entity.customer.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.customer.registrationaddress2
            new TranslationSeedItem("entity.customer.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "en-US", "客户电话_us", "客户电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "ja-JP", "客户电话_jp", "客户电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "zh-CN", "客户电话", "客户电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "zh-HK", "客户电话_hk", "客户电话"),

            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "en-US", "客户传真_us", "客户传真"),
            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "ja-JP", "客户传真_jp", "客户传真"),
            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "zh-CN", "客户传真", "客户传真"),
            // entity.customer.fax
            new TranslationSeedItem("entity.customer.fax", "zh-HK", "客户传真_hk", "客户传真"),

            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "en-US", "客户邮箱_us", "客户邮箱"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "ja-JP", "客户邮箱_jp", "客户邮箱"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "zh-CN", "客户邮箱", "客户邮箱"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "zh-HK", "客户邮箱_hk", "客户邮箱"),

            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "en-US", "客户网站_us", "客户网站"),
            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "ja-JP", "客户网站_jp", "客户网站"),
            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "zh-CN", "客户网站", "客户网站"),
            // entity.customer.website
            new TranslationSeedItem("entity.customer.website", "zh-HK", "客户网站_hk", "客户网站"),

            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.customer.contactperson
            new TranslationSeedItem("entity.customer.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "en-US", "联系人电话_us", "联系人电话"),
            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "ja-JP", "联系人电话_jp", "联系人电话"),
            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.customer.contactphone
            new TranslationSeedItem("entity.customer.contactphone", "zh-HK", "联系人电话_hk", "联系人电话"),

            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "en-US", "联系人邮箱_us", "联系人邮箱"),
            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "ja-JP", "联系人邮箱_jp", "联系人邮箱"),
            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.customer.contactemail
            new TranslationSeedItem("entity.customer.contactemail", "zh-HK", "联系人邮箱_hk", "联系人邮箱"),

            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "en-US", "结算币种代码_us", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "zh-CN", "结算币种代码", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.customer.salesorganization
            new TranslationSeedItem("entity.customer.salesorganization", "en-US", "销售组织_us", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.customer.salesorganization
            new TranslationSeedItem("entity.customer.salesorganization", "ja-JP", "销售组织_jp", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.customer.salesorganization
            new TranslationSeedItem("entity.customer.salesorganization", "zh-CN", "销售组织", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.customer.salesorganization
            new TranslationSeedItem("entity.customer.salesorganization", "zh-HK", "销售组织_hk", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),

            // entity.customer.distributionchannel
            new TranslationSeedItem("entity.customer.distributionchannel", "en-US", "分销渠道_us", "分销渠道"),
            // entity.customer.distributionchannel
            new TranslationSeedItem("entity.customer.distributionchannel", "ja-JP", "分销渠道_jp", "分销渠道"),
            // entity.customer.distributionchannel
            new TranslationSeedItem("entity.customer.distributionchannel", "zh-CN", "分销渠道", "分销渠道"),
            // entity.customer.distributionchannel
            new TranslationSeedItem("entity.customer.distributionchannel", "zh-HK", "分销渠道_hk", "分销渠道"),

            // entity.customer.productgroup
            new TranslationSeedItem("entity.customer.productgroup", "en-US", "产品组_us", "产品组"),
            // entity.customer.productgroup
            new TranslationSeedItem("entity.customer.productgroup", "ja-JP", "产品组_jp", "产品组"),
            // entity.customer.productgroup
            new TranslationSeedItem("entity.customer.productgroup", "zh-CN", "产品组", "产品组"),
            // entity.customer.productgroup
            new TranslationSeedItem("entity.customer.productgroup", "zh-HK", "产品组_hk", "产品组"),

            // entity.customer.group
            new TranslationSeedItem("entity.customer.group", "en-US", "客户组_us", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),
            // entity.customer.group
            new TranslationSeedItem("entity.customer.group", "ja-JP", "客户组_jp", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),
            // entity.customer.group
            new TranslationSeedItem("entity.customer.group", "zh-CN", "客户组", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),
            // entity.customer.group
            new TranslationSeedItem("entity.customer.group", "zh-HK", "客户组_hk", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),

            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "en-US", "贸易伙伴_us", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "ja-JP", "贸易伙伴_jp", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "zh-CN", "贸易伙伴", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "zh-HK", "贸易伙伴_hk", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "en-US", "帐户分配组_us", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "ja-JP", "帐户分配组_jp", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "zh-CN", "帐户分配组", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "zh-HK", "帐户分配组_hk", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),

            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "en-US", "供应商_us", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "ja-JP", "供应商_jp", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "zh-CN", "供应商", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "zh-HK", "供应商_hk", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.customer.nielsenindicator
            new TranslationSeedItem("entity.customer.nielsenindicator", "en-US", "尼尔森标识_us", "尼尔森标识"),
            // entity.customer.nielsenindicator
            new TranslationSeedItem("entity.customer.nielsenindicator", "ja-JP", "尼尔森标识_jp", "尼尔森标识"),
            // entity.customer.nielsenindicator
            new TranslationSeedItem("entity.customer.nielsenindicator", "zh-CN", "尼尔森标识", "尼尔森标识"),
            // entity.customer.nielsenindicator
            new TranslationSeedItem("entity.customer.nielsenindicator", "zh-HK", "尼尔森标识_hk", "尼尔森标识"),

            // entity.customer.centralpostingblock
            new TranslationSeedItem("entity.customer.centralpostingblock", "en-US", "中心记帐冻结_us", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.customer.centralpostingblock
            new TranslationSeedItem("entity.customer.centralpostingblock", "ja-JP", "中心记帐冻结_jp", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.customer.centralpostingblock
            new TranslationSeedItem("entity.customer.centralpostingblock", "zh-CN", "中心记帐冻结", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.customer.centralpostingblock
            new TranslationSeedItem("entity.customer.centralpostingblock", "zh-HK", "中心记帐冻结_hk", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),

            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "en-US", "统驭科目_us", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "ja-JP", "统驭科目_jp", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "zh-CN", "统驭科目", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "zh-HK", "统驭科目_hk", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),

            // entity.customer.headquarters
            new TranslationSeedItem("entity.customer.headquarters", "en-US", "总部_us", "总部（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.customer.headquarters
            new TranslationSeedItem("entity.customer.headquarters", "ja-JP", "总部_jp", "总部（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.customer.headquarters
            new TranslationSeedItem("entity.customer.headquarters", "zh-CN", "总部", "总部（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.customer.headquarters
            new TranslationSeedItem("entity.customer.headquarters", "zh-HK", "总部_hk", "总部（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.customer.clearingwithvendor
            new TranslationSeedItem("entity.customer.clearingwithvendor", "en-US", "具有供应商的清算_us", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.customer.clearingwithvendor
            new TranslationSeedItem("entity.customer.clearingwithvendor", "ja-JP", "具有供应商的清算_jp", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.customer.clearingwithvendor
            new TranslationSeedItem("entity.customer.clearingwithvendor", "zh-CN", "具有供应商的清算", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.customer.clearingwithvendor
            new TranslationSeedItem("entity.customer.clearingwithvendor", "zh-HK", "具有供应商的清算_hk", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),

            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),

            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "en-US", "交货工厂_us", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "ja-JP", "交货工厂_jp", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "zh-CN", "交货工厂", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "zh-HK", "交货工厂_hk", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "en-US", "国际贸易条件1_us", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "ja-JP", "国际贸易条件1_jp", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "zh-CN", "国际贸易条件1", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "zh-HK", "国际贸易条件1_hk", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),

            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "en-US", "国际贸易条件2_us", "国际贸易条件2（地点说明）"),
            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "ja-JP", "国际贸易条件2_jp", "国际贸易条件2（地点说明）"),
            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "zh-CN", "国际贸易条件2", "国际贸易条件2（地点说明）"),
            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "zh-HK", "国际贸易条件2_hk", "国际贸易条件2（地点说明）"),

            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "en-US", "装运条件_us", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "ja-JP", "装运条件_jp", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "zh-CN", "装运条件", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "zh-HK", "装运条件_hk", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),

            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "en-US", "客户定价过程_us", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "ja-JP", "客户定价过程_jp", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "zh-CN", "客户定价过程", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "zh-HK", "客户定价过程_hk", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),

            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "en-US", "信用等级_us", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "ja-JP", "信用等级_jp", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "zh-CN", "信用等级", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "zh-HK", "信用等级_hk", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),

            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "en-US", "信用额度_us", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "ja-JP", "信用额度_jp", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "zh-CN", "信用额度", "信用额度（精确到分，存储为整数，单位为分）"),
            // entity.customer.creditamount
            new TranslationSeedItem("entity.customer.creditamount", "zh-HK", "信用额度_hk", "信用额度（精确到分，存储为整数，单位为分）"),

            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "en-US", "折扣率_us", "折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）"),
            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "ja-JP", "折扣率_jp", "折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）"),
            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "zh-CN", "折扣率", "折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）"),
            // entity.customer.discountrate
            new TranslationSeedItem("entity.customer.discountrate", "zh-HK", "折扣率_hk", "折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）"),

            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "en-US", "客户等级_us", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "ja-JP", "客户等级_jp", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "zh-CN", "客户等级", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "zh-HK", "客户等级_hk", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),

            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "en-US", "评价分数_us", "评价分数（0-100分）"),
            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "ja-JP", "评价分数_jp", "评价分数（0-100分）"),
            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.customer.evaluationscore
            new TranslationSeedItem("entity.customer.evaluationscore", "zh-HK", "评价分数_hk", "评价分数（0-100分）"),

            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.customer.sortorder
            new TranslationSeedItem("entity.customer.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "en-US", "客户状态_us", "客户状态（字典 sys_normal_disable）"),
            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "ja-JP", "客户状态_jp", "客户状态（字典 sys_normal_disable）"),
            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "zh-CN", "客户状态", "客户状态（字典 sys_normal_disable）"),
            // entity.customer.status
            new TranslationSeedItem("entity.customer.status", "zh-HK", "客户状态_hk", "客户状态（字典 sys_normal_disable）"),
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
