// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktClientI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktClient 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktClient 实体国际化翻译种子（键前缀 entity.client.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktClientI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktClient 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 client 实体翻译...", tenantCode);

        foreach (var item in GetClientTranslations())
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

        TaktLogger.Information("TaktClient 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktClient 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.client._self / entity.client.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetClientTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "en-US", "Client Information_us", "实体名称"),
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "ja-JP", "Takt客户端信息信息_jp", "实体名称"),
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "zh-CN", "Takt客户端信息信息", "实体名称"),
            // entity.client._self
            new TranslationSeedItem("entity.client._self", "zh-HK", "Takt客户端信息信息_hk", "实体名称"),

            // entity.client.code
            new TranslationSeedItem("entity.client.code", "en-US", "客户端编码_us", "客户端编码（唯一索引）"),
            // entity.client.code
            new TranslationSeedItem("entity.client.code", "ja-JP", "客户端编码_jp", "客户端编码（唯一索引）"),
            // entity.client.code
            new TranslationSeedItem("entity.client.code", "zh-CN", "客户端编码", "客户端编码（唯一索引）"),
            // entity.client.code
            new TranslationSeedItem("entity.client.code", "zh-HK", "客户端编码_hk", "客户端编码（唯一索引）"),

            // entity.client.name1
            new TranslationSeedItem("entity.client.name1", "en-US", "客户端名称1_us", "客户端名称1"),
            // entity.client.name1
            new TranslationSeedItem("entity.client.name1", "ja-JP", "客户端名称1_jp", "客户端名称1"),
            // entity.client.name1
            new TranslationSeedItem("entity.client.name1", "zh-CN", "客户端名称1", "客户端名称1"),
            // entity.client.name1
            new TranslationSeedItem("entity.client.name1", "zh-HK", "客户端名称1_hk", "客户端名称1"),

            // entity.client.name2
            new TranslationSeedItem("entity.client.name2", "en-US", "客户端名称2_us", "客户端名称2"),
            // entity.client.name2
            new TranslationSeedItem("entity.client.name2", "ja-JP", "客户端名称2_jp", "客户端名称2"),
            // entity.client.name2
            new TranslationSeedItem("entity.client.name2", "zh-CN", "客户端名称2", "客户端名称2"),
            // entity.client.name2
            new TranslationSeedItem("entity.client.name2", "zh-HK", "客户端名称2_hk", "客户端名称2"),

            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "en-US", "客户端简称_us", "客户端简称"),
            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "ja-JP", "客户端简称_jp", "客户端简称"),
            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "zh-CN", "客户端简称", "客户端简称"),
            // entity.client.shortname
            new TranslationSeedItem("entity.client.shortname", "zh-HK", "客户端简称_hk", "客户端简称"),

            // entity.client.type
            new TranslationSeedItem("entity.client.type", "en-US", "客户端类型_us", "客户端类型（字典 logistics_sales_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）"),
            // entity.client.type
            new TranslationSeedItem("entity.client.type", "ja-JP", "客户端类型_jp", "客户端类型（字典 logistics_sales_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）"),
            // entity.client.type
            new TranslationSeedItem("entity.client.type", "zh-CN", "客户端类型", "客户端类型（字典 logistics_sales_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）"),
            // entity.client.type
            new TranslationSeedItem("entity.client.type", "zh-HK", "客户端类型_hk", "客户端类型（字典 logistics_sales_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）"),

            // entity.client.enterprisenature
            new TranslationSeedItem("entity.client.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature）"),
            // entity.client.enterprisenature
            new TranslationSeedItem("entity.client.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature）"),
            // entity.client.enterprisenature
            new TranslationSeedItem("entity.client.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature）"),
            // entity.client.enterprisenature
            new TranslationSeedItem("entity.client.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature）"),

            // entity.client.industryattribute
            new TranslationSeedItem("entity.client.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute）"),
            // entity.client.industryattribute
            new TranslationSeedItem("entity.client.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute）"),
            // entity.client.industryattribute
            new TranslationSeedItem("entity.client.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute）"),
            // entity.client.industryattribute
            new TranslationSeedItem("entity.client.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute）"),

            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "en-US", "客户端标识_us", "客户端标识（税务登记证号/统一社会信用代码）"),
            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "ja-JP", "客户端标识_jp", "客户端标识（税务登记证号/统一社会信用代码）"),
            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "zh-CN", "客户端标识", "客户端标识（税务登记证号/统一社会信用代码）"),
            // entity.client.taxnumber
            new TranslationSeedItem("entity.client.taxnumber", "zh-HK", "客户端标识_hk", "客户端标识（税务登记证号/统一社会信用代码）"),

            // entity.client.taxcode
            new TranslationSeedItem("entity.client.taxcode", "en-US", "税码_us", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.client.taxcode
            new TranslationSeedItem("entity.client.taxcode", "ja-JP", "税码_jp", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.client.taxcode
            new TranslationSeedItem("entity.client.taxcode", "zh-CN", "税码", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),
            // entity.client.taxcode
            new TranslationSeedItem("entity.client.taxcode", "zh-HK", "税码_hk", "税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）"),

            // entity.client.taxrate
            new TranslationSeedItem("entity.client.taxrate", "en-US", "税率_us", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.client.taxrate
            new TranslationSeedItem("entity.client.taxrate", "ja-JP", "税率_jp", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.client.taxrate
            new TranslationSeedItem("entity.client.taxrate", "zh-CN", "税率", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),
            // entity.client.taxrate
            new TranslationSeedItem("entity.client.taxrate", "zh-HK", "税率_hk", "税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）"),

            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.client.registrationcountry
            new TranslationSeedItem("entity.client.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.client.registrationprovince
            new TranslationSeedItem("entity.client.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.client.registrationprovince
            new TranslationSeedItem("entity.client.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.client.registrationprovince
            new TranslationSeedItem("entity.client.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.client.registrationprovince
            new TranslationSeedItem("entity.client.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.client.registrationcity
            new TranslationSeedItem("entity.client.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.client.registrationcity
            new TranslationSeedItem("entity.client.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.client.registrationcity
            new TranslationSeedItem("entity.client.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.client.registrationcity
            new TranslationSeedItem("entity.client.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.client.registrationaddress1
            new TranslationSeedItem("entity.client.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.client.registrationaddress2
            new TranslationSeedItem("entity.client.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "en-US", "客户端电话_us", "客户端电话"),
            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "ja-JP", "客户端电话_jp", "客户端电话"),
            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "zh-CN", "客户端电话", "客户端电话"),
            // entity.client.phone
            new TranslationSeedItem("entity.client.phone", "zh-HK", "客户端电话_hk", "客户端电话"),

            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "en-US", "客户端传真_us", "客户端传真"),
            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "ja-JP", "客户端传真_jp", "客户端传真"),
            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "zh-CN", "客户端传真", "客户端传真"),
            // entity.client.fax
            new TranslationSeedItem("entity.client.fax", "zh-HK", "客户端传真_hk", "客户端传真"),

            // entity.client.email
            new TranslationSeedItem("entity.client.email", "en-US", "客户端邮箱_us", "客户端邮箱"),
            // entity.client.email
            new TranslationSeedItem("entity.client.email", "ja-JP", "客户端邮箱_jp", "客户端邮箱"),
            // entity.client.email
            new TranslationSeedItem("entity.client.email", "zh-CN", "客户端邮箱", "客户端邮箱"),
            // entity.client.email
            new TranslationSeedItem("entity.client.email", "zh-HK", "客户端邮箱_hk", "客户端邮箱"),

            // entity.client.website
            new TranslationSeedItem("entity.client.website", "en-US", "客户端网站_us", "客户端网站"),
            // entity.client.website
            new TranslationSeedItem("entity.client.website", "ja-JP", "客户端网站_jp", "客户端网站"),
            // entity.client.website
            new TranslationSeedItem("entity.client.website", "zh-CN", "客户端网站", "客户端网站"),
            // entity.client.website
            new TranslationSeedItem("entity.client.website", "zh-HK", "客户端网站_hk", "客户端网站"),

            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.client.contactperson
            new TranslationSeedItem("entity.client.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "en-US", "联系人电话_us", "联系人电话"),
            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "ja-JP", "联系人电话_jp", "联系人电话"),
            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.client.contactphone
            new TranslationSeedItem("entity.client.contactphone", "zh-HK", "联系人电话_hk", "联系人电话"),

            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "en-US", "联系人邮箱_us", "联系人邮箱"),
            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "ja-JP", "联系人邮箱_jp", "联系人邮箱"),
            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.client.contactemail
            new TranslationSeedItem("entity.client.contactemail", "zh-HK", "联系人邮箱_hk", "联系人邮箱"),

            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "en-US", "结算币种代码_us", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "ja-JP", "结算币种代码_jp", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "zh-CN", "结算币种代码", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.client.currencycode
            new TranslationSeedItem("entity.client.currencycode", "zh-HK", "结算币种代码_hk", "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.client.salesorganization
            new TranslationSeedItem("entity.client.salesorganization", "en-US", "销售组织_us", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.client.salesorganization
            new TranslationSeedItem("entity.client.salesorganization", "ja-JP", "销售组织_jp", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.client.salesorganization
            new TranslationSeedItem("entity.client.salesorganization", "zh-CN", "销售组织", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.client.salesorganization
            new TranslationSeedItem("entity.client.salesorganization", "zh-HK", "销售组织_hk", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),

            // entity.client.distributionchannel
            new TranslationSeedItem("entity.client.distributionchannel", "en-US", "分销渠道_us", "分销渠道"),
            // entity.client.distributionchannel
            new TranslationSeedItem("entity.client.distributionchannel", "ja-JP", "分销渠道_jp", "分销渠道"),
            // entity.client.distributionchannel
            new TranslationSeedItem("entity.client.distributionchannel", "zh-CN", "分销渠道", "分销渠道"),
            // entity.client.distributionchannel
            new TranslationSeedItem("entity.client.distributionchannel", "zh-HK", "分销渠道_hk", "分销渠道"),

            // entity.client.productgroup
            new TranslationSeedItem("entity.client.productgroup", "en-US", "产品组_us", "产品组"),
            // entity.client.productgroup
            new TranslationSeedItem("entity.client.productgroup", "ja-JP", "产品组_jp", "产品组"),
            // entity.client.productgroup
            new TranslationSeedItem("entity.client.productgroup", "zh-CN", "产品组", "产品组"),
            // entity.client.productgroup
            new TranslationSeedItem("entity.client.productgroup", "zh-HK", "产品组_hk", "产品组"),

            // entity.client.customergroup
            new TranslationSeedItem("entity.client.customergroup", "en-US", "客户组_us", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),
            // entity.client.customergroup
            new TranslationSeedItem("entity.client.customergroup", "ja-JP", "客户组_jp", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),
            // entity.client.customergroup
            new TranslationSeedItem("entity.client.customergroup", "zh-CN", "客户组", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),
            // entity.client.customergroup
            new TranslationSeedItem("entity.client.customergroup", "zh-HK", "客户组_hk", "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）"),

            // entity.client.tradingpartner
            new TranslationSeedItem("entity.client.tradingpartner", "en-US", "贸易伙伴_us", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.client.tradingpartner
            new TranslationSeedItem("entity.client.tradingpartner", "ja-JP", "贸易伙伴_jp", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.client.tradingpartner
            new TranslationSeedItem("entity.client.tradingpartner", "zh-CN", "贸易伙伴", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.client.tradingpartner
            new TranslationSeedItem("entity.client.tradingpartner", "zh-HK", "贸易伙伴_hk", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.client.accountassignmentgroup
            new TranslationSeedItem("entity.client.accountassignmentgroup", "en-US", "帐户分配组_us", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.client.accountassignmentgroup
            new TranslationSeedItem("entity.client.accountassignmentgroup", "ja-JP", "帐户分配组_jp", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.client.accountassignmentgroup
            new TranslationSeedItem("entity.client.accountassignmentgroup", "zh-CN", "帐户分配组", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.client.accountassignmentgroup
            new TranslationSeedItem("entity.client.accountassignmentgroup", "zh-HK", "帐户分配组_hk", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),

            // entity.client.suppliercode
            new TranslationSeedItem("entity.client.suppliercode", "en-US", "供应商_us", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.client.suppliercode
            new TranslationSeedItem("entity.client.suppliercode", "ja-JP", "供应商_jp", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.client.suppliercode
            new TranslationSeedItem("entity.client.suppliercode", "zh-CN", "供应商", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.client.suppliercode
            new TranslationSeedItem("entity.client.suppliercode", "zh-HK", "供应商_hk", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.client.nielsenindicator
            new TranslationSeedItem("entity.client.nielsenindicator", "en-US", "尼尔森标识_us", "尼尔森标识"),
            // entity.client.nielsenindicator
            new TranslationSeedItem("entity.client.nielsenindicator", "ja-JP", "尼尔森标识_jp", "尼尔森标识"),
            // entity.client.nielsenindicator
            new TranslationSeedItem("entity.client.nielsenindicator", "zh-CN", "尼尔森标识", "尼尔森标识"),
            // entity.client.nielsenindicator
            new TranslationSeedItem("entity.client.nielsenindicator", "zh-HK", "尼尔森标识_hk", "尼尔森标识"),

            // entity.client.centralpostingblock
            new TranslationSeedItem("entity.client.centralpostingblock", "en-US", "中心记帐冻结_us", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.client.centralpostingblock
            new TranslationSeedItem("entity.client.centralpostingblock", "ja-JP", "中心记帐冻结_jp", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.client.centralpostingblock
            new TranslationSeedItem("entity.client.centralpostingblock", "zh-CN", "中心记帐冻结", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.client.centralpostingblock
            new TranslationSeedItem("entity.client.centralpostingblock", "zh-HK", "中心记帐冻结_hk", "中心记帐冻结（字典 sys_yes_no；0=否 1=是）"),

            // entity.client.reconciliationaccount
            new TranslationSeedItem("entity.client.reconciliationaccount", "en-US", "统驭科目_us", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.client.reconciliationaccount
            new TranslationSeedItem("entity.client.reconciliationaccount", "ja-JP", "统驭科目_jp", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.client.reconciliationaccount
            new TranslationSeedItem("entity.client.reconciliationaccount", "zh-CN", "统驭科目", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.client.reconciliationaccount
            new TranslationSeedItem("entity.client.reconciliationaccount", "zh-HK", "统驭科目_hk", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),

            // entity.client.headquarters
            new TranslationSeedItem("entity.client.headquarters", "en-US", "总部_us", "总部（选项 TaktClients/options；DictValue=ClientCode）"),
            // entity.client.headquarters
            new TranslationSeedItem("entity.client.headquarters", "ja-JP", "总部_jp", "总部（选项 TaktClients/options；DictValue=ClientCode）"),
            // entity.client.headquarters
            new TranslationSeedItem("entity.client.headquarters", "zh-CN", "总部", "总部（选项 TaktClients/options；DictValue=ClientCode）"),
            // entity.client.headquarters
            new TranslationSeedItem("entity.client.headquarters", "zh-HK", "总部_hk", "总部（选项 TaktClients/options；DictValue=ClientCode）"),

            // entity.client.clearingwithvendor
            new TranslationSeedItem("entity.client.clearingwithvendor", "en-US", "具有供应商的清算_us", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.client.clearingwithvendor
            new TranslationSeedItem("entity.client.clearingwithvendor", "ja-JP", "具有供应商的清算_jp", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.client.clearingwithvendor
            new TranslationSeedItem("entity.client.clearingwithvendor", "zh-CN", "具有供应商的清算", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.client.clearingwithvendor
            new TranslationSeedItem("entity.client.clearingwithvendor", "zh-HK", "具有供应商的清算_hk", "具有供应商的清算（字典 sys_yes_no；0=否 1=是）"),

            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),
            // entity.client.paymentterms
            new TranslationSeedItem("entity.client.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）"),

            // entity.client.paymentmethod
            new TranslationSeedItem("entity.client.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.client.paymentmethod
            new TranslationSeedItem("entity.client.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.client.paymentmethod
            new TranslationSeedItem("entity.client.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),
            // entity.client.paymentmethod
            new TranslationSeedItem("entity.client.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）"),

            // entity.client.deliveringplant
            new TranslationSeedItem("entity.client.deliveringplant", "en-US", "交货工厂_us", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.client.deliveringplant
            new TranslationSeedItem("entity.client.deliveringplant", "ja-JP", "交货工厂_jp", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.client.deliveringplant
            new TranslationSeedItem("entity.client.deliveringplant", "zh-CN", "交货工厂", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.client.deliveringplant
            new TranslationSeedItem("entity.client.deliveringplant", "zh-HK", "交货工厂_hk", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.client.incoterms1
            new TranslationSeedItem("entity.client.incoterms1", "en-US", "国际贸易条件1_us", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.client.incoterms1
            new TranslationSeedItem("entity.client.incoterms1", "ja-JP", "国际贸易条件1_jp", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.client.incoterms1
            new TranslationSeedItem("entity.client.incoterms1", "zh-CN", "国际贸易条件1", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.client.incoterms1
            new TranslationSeedItem("entity.client.incoterms1", "zh-HK", "国际贸易条件1_hk", "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),

            // entity.client.incoterms2
            new TranslationSeedItem("entity.client.incoterms2", "en-US", "国际贸易条件2_us", "国际贸易条件2（地点说明）"),
            // entity.client.incoterms2
            new TranslationSeedItem("entity.client.incoterms2", "ja-JP", "国际贸易条件2_jp", "国际贸易条件2（地点说明）"),
            // entity.client.incoterms2
            new TranslationSeedItem("entity.client.incoterms2", "zh-CN", "国际贸易条件2", "国际贸易条件2（地点说明）"),
            // entity.client.incoterms2
            new TranslationSeedItem("entity.client.incoterms2", "zh-HK", "国际贸易条件2_hk", "国际贸易条件2（地点说明）"),

            // entity.client.shippingconditions
            new TranslationSeedItem("entity.client.shippingconditions", "en-US", "装运条件_us", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.client.shippingconditions
            new TranslationSeedItem("entity.client.shippingconditions", "ja-JP", "装运条件_jp", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.client.shippingconditions
            new TranslationSeedItem("entity.client.shippingconditions", "zh-CN", "装运条件", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.client.shippingconditions
            new TranslationSeedItem("entity.client.shippingconditions", "zh-HK", "装运条件_hk", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),

            // entity.client.customerpricingprocedure
            new TranslationSeedItem("entity.client.customerpricingprocedure", "en-US", "客户定价过程_us", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.client.customerpricingprocedure
            new TranslationSeedItem("entity.client.customerpricingprocedure", "ja-JP", "客户定价过程_jp", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.client.customerpricingprocedure
            new TranslationSeedItem("entity.client.customerpricingprocedure", "zh-CN", "客户定价过程", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.client.customerpricingprocedure
            new TranslationSeedItem("entity.client.customerpricingprocedure", "zh-HK", "客户定价过程_hk", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),

            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "en-US", "销售渠道_us", "销售渠道（字典 logistics_sales_channel；0=直销 1=经销 2=代销 3=电商 4=其他）"),
            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "ja-JP", "销售渠道_jp", "销售渠道（字典 logistics_sales_channel；0=直销 1=经销 2=代销 3=电商 4=其他）"),
            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "zh-CN", "销售渠道", "销售渠道（字典 logistics_sales_channel；0=直销 1=经销 2=代销 3=电商 4=其他）"),
            // entity.client.saleschannel
            new TranslationSeedItem("entity.client.saleschannel", "zh-HK", "销售渠道_hk", "销售渠道（字典 logistics_sales_channel；0=直销 1=经销 2=代销 3=电商 4=其他）"),

            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "en-US", "平台名称_us", "平台名称（电商平台名称）"),
            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "ja-JP", "平台名称_jp", "平台名称（电商平台名称）"),
            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "zh-CN", "平台名称", "平台名称（电商平台名称）"),
            // entity.client.platformname
            new TranslationSeedItem("entity.client.platformname", "zh-HK", "平台名称_hk", "平台名称（电商平台名称）"),

            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "en-US", "店铺名称_us", "店铺名称"),
            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "ja-JP", "店铺名称_jp", "店铺名称"),
            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "zh-CN", "店铺名称", "店铺名称"),
            // entity.client.storename
            new TranslationSeedItem("entity.client.storename", "zh-HK", "店铺名称_hk", "店铺名称"),

            // entity.client.level
            new TranslationSeedItem("entity.client.level", "en-US", "客户端等级_us", "客户端等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.client.level
            new TranslationSeedItem("entity.client.level", "ja-JP", "客户端等级_jp", "客户端等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.client.level
            new TranslationSeedItem("entity.client.level", "zh-CN", "客户端等级", "客户端等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.client.level
            new TranslationSeedItem("entity.client.level", "zh-HK", "客户端等级_hk", "客户端等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),

            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "en-US", "评价分数_us", "评价分数（0-100分）"),
            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "ja-JP", "评价分数_jp", "评价分数（0-100分）"),
            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.client.evaluationscore
            new TranslationSeedItem("entity.client.evaluationscore", "zh-HK", "评价分数_hk", "评价分数（0-100分）"),

            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.client.sortorder
            new TranslationSeedItem("entity.client.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.client.status
            new TranslationSeedItem("entity.client.status", "en-US", "客户端状态_us", "客户端状态（字典 sys_normal_disable）"),
            // entity.client.status
            new TranslationSeedItem("entity.client.status", "ja-JP", "客户端状态_jp", "客户端状态（字典 sys_normal_disable）"),
            // entity.client.status
            new TranslationSeedItem("entity.client.status", "zh-CN", "客户端状态", "客户端状态（字典 sys_normal_disable）"),
            // entity.client.status
            new TranslationSeedItem("entity.client.status", "zh-HK", "客户端状态_hk", "客户端状态（字典 sys_normal_disable）"),
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
