// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktVendorI18nSeedData.cs
// 创建时间：2026-10-27
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
            new TranslationSeedItem("entity.vendor.code", "en-US", "经销商_us", "经销商（与租户、公司、采购组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 经销商规则生成并展示，非手输；单据类型菜单：经销商）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "ja-JP", "经销商_jp", "经销商（与租户、公司、采购组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 经销商规则生成并展示，非手输；单据类型菜单：经销商）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "zh-CN", "经销商", "经销商（与租户、公司、采购组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 经销商规则生成并展示，非手输；单据类型菜单：经销商）"),
            // entity.vendor.code
            new TranslationSeedItem("entity.vendor.code", "zh-HK", "经销商_hk", "经销商（与租户、公司、采购组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 经销商规则生成并展示，非手输；单据类型菜单：经销商）"),

            // entity.vendor.accountgroup
            new TranslationSeedItem("entity.vendor.accountgroup", "en-US", "账户组_us", "账户组"),
            // entity.vendor.accountgroup
            new TranslationSeedItem("entity.vendor.accountgroup", "ja-JP", "账户组_jp", "账户组"),
            // entity.vendor.accountgroup
            new TranslationSeedItem("entity.vendor.accountgroup", "zh-CN", "账户组", "账户组"),
            // entity.vendor.accountgroup
            new TranslationSeedItem("entity.vendor.accountgroup", "zh-HK", "账户组_hk", "账户组"),

            // entity.vendor.title
            new TranslationSeedItem("entity.vendor.title", "en-US", "称谓_us", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),
            // entity.vendor.title
            new TranslationSeedItem("entity.vendor.title", "ja-JP", "称谓_jp", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),
            // entity.vendor.title
            new TranslationSeedItem("entity.vendor.title", "zh-CN", "称谓", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),
            // entity.vendor.title
            new TranslationSeedItem("entity.vendor.title", "zh-HK", "称谓_hk", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),

            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "en-US", "名称 1_us", "名称 1"),
            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "ja-JP", "名称 1_jp", "名称 1"),
            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "zh-CN", "名称 1", "名称 1"),
            // entity.vendor.name1
            new TranslationSeedItem("entity.vendor.name1", "zh-HK", "名称 1_hk", "名称 1"),

            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "en-US", "名称 2_us", "名称 2"),
            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "ja-JP", "名称 2_jp", "名称 2"),
            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "zh-CN", "名称 2", "名称 2"),
            // entity.vendor.name2
            new TranslationSeedItem("entity.vendor.name2", "zh-HK", "名称 2_hk", "名称 2"),

            // entity.vendor.name3
            new TranslationSeedItem("entity.vendor.name3", "en-US", "名称 3_us", "名称 3"),
            // entity.vendor.name3
            new TranslationSeedItem("entity.vendor.name3", "ja-JP", "名称 3_jp", "名称 3"),
            // entity.vendor.name3
            new TranslationSeedItem("entity.vendor.name3", "zh-CN", "名称 3", "名称 3"),
            // entity.vendor.name3
            new TranslationSeedItem("entity.vendor.name3", "zh-HK", "名称 3_hk", "名称 3"),

            // entity.vendor.name4
            new TranslationSeedItem("entity.vendor.name4", "en-US", "名称 4_us", "名称 4"),
            // entity.vendor.name4
            new TranslationSeedItem("entity.vendor.name4", "ja-JP", "名称 4_jp", "名称 4"),
            // entity.vendor.name4
            new TranslationSeedItem("entity.vendor.name4", "zh-CN", "名称 4", "名称 4"),
            // entity.vendor.name4
            new TranslationSeedItem("entity.vendor.name4", "zh-HK", "名称 4_hk", "名称 4"),

            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "en-US", "简称_us", "简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "ja-JP", "简称_jp", "简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "zh-CN", "简称", "简称"),
            // entity.vendor.shortname
            new TranslationSeedItem("entity.vendor.shortname", "zh-HK", "简称_hk", "简称"),

            // entity.vendor.countrycode
            new TranslationSeedItem("entity.vendor.countrycode", "en-US", "国家_us", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.countrycode
            new TranslationSeedItem("entity.vendor.countrycode", "ja-JP", "国家_jp", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.countrycode
            new TranslationSeedItem("entity.vendor.countrycode", "zh-CN", "国家", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.countrycode
            new TranslationSeedItem("entity.vendor.countrycode", "zh-HK", "国家_hk", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.vendor.regioncode
            new TranslationSeedItem("entity.vendor.regioncode", "en-US", "地区_us", "地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.vendor.regioncode
            new TranslationSeedItem("entity.vendor.regioncode", "ja-JP", "地区_jp", "地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.vendor.regioncode
            new TranslationSeedItem("entity.vendor.regioncode", "zh-CN", "地区", "地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.vendor.regioncode
            new TranslationSeedItem("entity.vendor.regioncode", "zh-HK", "地区_hk", "地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.vendor.citycode
            new TranslationSeedItem("entity.vendor.citycode", "en-US", "城市_us", "城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.vendor.citycode
            new TranslationSeedItem("entity.vendor.citycode", "ja-JP", "城市_jp", "城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.vendor.citycode
            new TranslationSeedItem("entity.vendor.citycode", "zh-CN", "城市", "城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.vendor.citycode
            new TranslationSeedItem("entity.vendor.citycode", "zh-HK", "城市_hk", "城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.vendor.addr1
            new TranslationSeedItem("entity.vendor.addr1", "en-US", "地址1_us", "地址1"),
            // entity.vendor.addr1
            new TranslationSeedItem("entity.vendor.addr1", "ja-JP", "地址1_jp", "地址1"),
            // entity.vendor.addr1
            new TranslationSeedItem("entity.vendor.addr1", "zh-CN", "地址1", "地址1"),
            // entity.vendor.addr1
            new TranslationSeedItem("entity.vendor.addr1", "zh-HK", "地址1_hk", "地址1"),

            // entity.vendor.addr2
            new TranslationSeedItem("entity.vendor.addr2", "en-US", "地址2_us", "地址2（含门牌，短字符串 40 不够）"),
            // entity.vendor.addr2
            new TranslationSeedItem("entity.vendor.addr2", "ja-JP", "地址2_jp", "地址2（含门牌，短字符串 40 不够）"),
            // entity.vendor.addr2
            new TranslationSeedItem("entity.vendor.addr2", "zh-CN", "地址2", "地址2（含门牌，短字符串 40 不够）"),
            // entity.vendor.addr2
            new TranslationSeedItem("entity.vendor.addr2", "zh-HK", "地址2_hk", "地址2（含门牌，短字符串 40 不够）"),

            // entity.vendor.postalcode
            new TranslationSeedItem("entity.vendor.postalcode", "en-US", "邮政编码_us", "邮政编码"),
            // entity.vendor.postalcode
            new TranslationSeedItem("entity.vendor.postalcode", "ja-JP", "邮政编码_jp", "邮政编码"),
            // entity.vendor.postalcode
            new TranslationSeedItem("entity.vendor.postalcode", "zh-CN", "邮政编码", "邮政编码"),
            // entity.vendor.postalcode
            new TranslationSeedItem("entity.vendor.postalcode", "zh-HK", "邮政编码_hk", "邮政编码"),

            // entity.vendor.phone1
            new TranslationSeedItem("entity.vendor.phone1", "en-US", "电话号码1_us", "电话号码1"),
            // entity.vendor.phone1
            new TranslationSeedItem("entity.vendor.phone1", "ja-JP", "电话号码1_jp", "电话号码1"),
            // entity.vendor.phone1
            new TranslationSeedItem("entity.vendor.phone1", "zh-CN", "电话号码1", "电话号码1"),
            // entity.vendor.phone1
            new TranslationSeedItem("entity.vendor.phone1", "zh-HK", "电话号码1_hk", "电话号码1"),

            // entity.vendor.phone2
            new TranslationSeedItem("entity.vendor.phone2", "en-US", "电话号码2_us", "电话号码2"),
            // entity.vendor.phone2
            new TranslationSeedItem("entity.vendor.phone2", "ja-JP", "电话号码2_jp", "电话号码2"),
            // entity.vendor.phone2
            new TranslationSeedItem("entity.vendor.phone2", "zh-CN", "电话号码2", "电话号码2"),
            // entity.vendor.phone2
            new TranslationSeedItem("entity.vendor.phone2", "zh-HK", "电话号码2_hk", "电话号码2"),

            // entity.vendor.faxnumber
            new TranslationSeedItem("entity.vendor.faxnumber", "en-US", "传真号码_us", "传真号码"),
            // entity.vendor.faxnumber
            new TranslationSeedItem("entity.vendor.faxnumber", "ja-JP", "传真号码_jp", "传真号码"),
            // entity.vendor.faxnumber
            new TranslationSeedItem("entity.vendor.faxnumber", "zh-CN", "传真号码", "传真号码"),
            // entity.vendor.faxnumber
            new TranslationSeedItem("entity.vendor.faxnumber", "zh-HK", "传真号码_hk", "传真号码"),

            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "en-US", "客户_us", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "ja-JP", "客户_jp", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "zh-CN", "客户", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),
            // entity.vendor.customercode
            new TranslationSeedItem("entity.vendor.customercode", "zh-HK", "客户_hk", "客户（选项 TaktCustomers/options；DictValue=CustomerCode）"),

            // entity.vendor.tradingpartner
            new TranslationSeedItem("entity.vendor.tradingpartner", "en-US", "贸易伙伴_us", "贸易伙伴"),
            // entity.vendor.tradingpartner
            new TranslationSeedItem("entity.vendor.tradingpartner", "ja-JP", "贸易伙伴_jp", "贸易伙伴"),
            // entity.vendor.tradingpartner
            new TranslationSeedItem("entity.vendor.tradingpartner", "zh-CN", "贸易伙伴", "贸易伙伴"),
            // entity.vendor.tradingpartner
            new TranslationSeedItem("entity.vendor.tradingpartner", "zh-HK", "贸易伙伴_hk", "贸易伙伴"),

            // entity.vendor.authorizationgroup
            new TranslationSeedItem("entity.vendor.authorizationgroup", "en-US", "权限组_us", "权限组"),
            // entity.vendor.authorizationgroup
            new TranslationSeedItem("entity.vendor.authorizationgroup", "ja-JP", "权限组_jp", "权限组"),
            // entity.vendor.authorizationgroup
            new TranslationSeedItem("entity.vendor.authorizationgroup", "zh-CN", "权限组", "权限组"),
            // entity.vendor.authorizationgroup
            new TranslationSeedItem("entity.vendor.authorizationgroup", "zh-HK", "权限组_hk", "权限组"),

            // entity.vendor.groupkey
            new TranslationSeedItem("entity.vendor.groupkey", "en-US", "组代码_us", "组代码"),
            // entity.vendor.groupkey
            new TranslationSeedItem("entity.vendor.groupkey", "ja-JP", "组代码_jp", "组代码"),
            // entity.vendor.groupkey
            new TranslationSeedItem("entity.vendor.groupkey", "zh-CN", "组代码", "组代码"),
            // entity.vendor.groupkey
            new TranslationSeedItem("entity.vendor.groupkey", "zh-HK", "组代码_hk", "组代码"),

            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "en-US", "经销商类型_us", "经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "ja-JP", "经销商类型_jp", "经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "zh-CN", "经销商类型", "经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.vendor.type
            new TranslationSeedItem("entity.vendor.type", "zh-HK", "经销商类型_hk", "经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),

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

            // entity.vendor.creditinfonumber
            new TranslationSeedItem("entity.vendor.creditinfonumber", "en-US", "信用信息号_us", "信用信息号"),
            // entity.vendor.creditinfonumber
            new TranslationSeedItem("entity.vendor.creditinfonumber", "ja-JP", "信用信息号_jp", "信用信息号"),
            // entity.vendor.creditinfonumber
            new TranslationSeedItem("entity.vendor.creditinfonumber", "zh-CN", "信用信息号", "信用信息号"),
            // entity.vendor.creditinfonumber
            new TranslationSeedItem("entity.vendor.creditinfonumber", "zh-HK", "信用信息号_hk", "信用信息号"),

            // entity.vendor.bankcountrycode
            new TranslationSeedItem("entity.vendor.bankcountrycode", "en-US", "银行国家代码_us", "银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.bankcountrycode
            new TranslationSeedItem("entity.vendor.bankcountrycode", "ja-JP", "银行国家代码_jp", "银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.bankcountrycode
            new TranslationSeedItem("entity.vendor.bankcountrycode", "zh-CN", "银行国家代码", "银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.bankcountrycode
            new TranslationSeedItem("entity.vendor.bankcountrycode", "zh-HK", "银行国家代码_hk", "银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "en-US", "银行代码_us", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "ja-JP", "银行代码_jp", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "zh-CN", "银行代码", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.bankcode
            new TranslationSeedItem("entity.vendor.bankcode", "zh-HK", "银行代码_hk", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "en-US", "银行账户_us", "银行账户"),
            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "ja-JP", "银行账户_jp", "银行账户"),
            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "zh-CN", "银行账户", "银行账户"),
            // entity.vendor.bankaccount
            new TranslationSeedItem("entity.vendor.bankaccount", "zh-HK", "银行账户_hk", "银行账户"),

            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "en-US", "科目持有者_us", "科目持有者"),
            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "ja-JP", "科目持有者_jp", "科目持有者"),
            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "zh-CN", "科目持有者", "科目持有者"),
            // entity.vendor.accountholder
            new TranslationSeedItem("entity.vendor.accountholder", "zh-HK", "科目持有者_hk", "科目持有者"),

            // entity.vendor.bankchargebearercode
            new TranslationSeedItem("entity.vendor.bankchargebearercode", "en-US", "指令码_us", "指令码"),
            // entity.vendor.bankchargebearercode
            new TranslationSeedItem("entity.vendor.bankchargebearercode", "ja-JP", "指令码_jp", "指令码"),
            // entity.vendor.bankchargebearercode
            new TranslationSeedItem("entity.vendor.bankchargebearercode", "zh-CN", "指令码", "指令码"),
            // entity.vendor.bankchargebearercode
            new TranslationSeedItem("entity.vendor.bankchargebearercode", "zh-HK", "指令码_hk", "指令码"),

            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "en-US", "统驭科目_us", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "ja-JP", "统驭科目_jp", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "zh-CN", "统驭科目", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),
            // entity.vendor.reconciliationaccount
            new TranslationSeedItem("entity.vendor.reconciliationaccount", "zh-HK", "统驭科目_hk", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）"),

            // entity.vendor.headofficeaccountcode
            new TranslationSeedItem("entity.vendor.headofficeaccountcode", "en-US", "总部_us", "总部"),
            // entity.vendor.headofficeaccountcode
            new TranslationSeedItem("entity.vendor.headofficeaccountcode", "ja-JP", "总部_jp", "总部"),
            // entity.vendor.headofficeaccountcode
            new TranslationSeedItem("entity.vendor.headofficeaccountcode", "zh-CN", "总部", "总部"),
            // entity.vendor.headofficeaccountcode
            new TranslationSeedItem("entity.vendor.headofficeaccountcode", "zh-HK", "总部_hk", "总部"),

            // entity.vendor.planninggroup
            new TranslationSeedItem("entity.vendor.planninggroup", "en-US", "计划组_us", "计划组"),
            // entity.vendor.planninggroup
            new TranslationSeedItem("entity.vendor.planninggroup", "ja-JP", "计划组_jp", "计划组"),
            // entity.vendor.planninggroup
            new TranslationSeedItem("entity.vendor.planninggroup", "zh-CN", "计划组", "计划组"),
            // entity.vendor.planninggroup
            new TranslationSeedItem("entity.vendor.planninggroup", "zh-HK", "计划组_hk", "计划组"),

            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),
            // entity.vendor.paymentterms
            new TranslationSeedItem("entity.vendor.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),

            // entity.vendor.doubleinvoicecheck
            new TranslationSeedItem("entity.vendor.doubleinvoicecheck", "en-US", "检查双重发票_us", "检查双重发票（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.doubleinvoicecheck
            new TranslationSeedItem("entity.vendor.doubleinvoicecheck", "ja-JP", "检查双重发票_jp", "检查双重发票（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.doubleinvoicecheck
            new TranslationSeedItem("entity.vendor.doubleinvoicecheck", "zh-CN", "检查双重发票", "检查双重发票（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.doubleinvoicecheck
            new TranslationSeedItem("entity.vendor.doubleinvoicecheck", "zh-HK", "检查双重发票_hk", "检查双重发票（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),
            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),
            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),
            // entity.vendor.paymentmethod
            new TranslationSeedItem("entity.vendor.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),

            // entity.vendor.paymentblock
            new TranslationSeedItem("entity.vendor.paymentblock", "en-US", "付款冻结_us", "付款冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.paymentblock
            new TranslationSeedItem("entity.vendor.paymentblock", "ja-JP", "付款冻结_jp", "付款冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.paymentblock
            new TranslationSeedItem("entity.vendor.paymentblock", "zh-CN", "付款冻结", "付款冻结（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.paymentblock
            new TranslationSeedItem("entity.vendor.paymentblock", "zh-HK", "付款冻结_hk", "付款冻结（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.housebank
            new TranslationSeedItem("entity.vendor.housebank", "en-US", "开户行_us", "开户行（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.housebank
            new TranslationSeedItem("entity.vendor.housebank", "ja-JP", "开户行_jp", "开户行（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.housebank
            new TranslationSeedItem("entity.vendor.housebank", "zh-CN", "开户行", "开户行（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.vendor.housebank
            new TranslationSeedItem("entity.vendor.housebank", "zh-HK", "开户行_hk", "开户行（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "en-US", "客商对冲_us", "在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "ja-JP", "客商对冲_jp", "在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "zh-CN", "客商对冲", "在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.clearingwithcustomer
            new TranslationSeedItem("entity.vendor.clearingwithcustomer", "zh-HK", "客商对冲_hk", "在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.accountingclerk
            new TranslationSeedItem("entity.vendor.accountingclerk", "en-US", "会计职员_us", "会计职员"),
            // entity.vendor.accountingclerk
            new TranslationSeedItem("entity.vendor.accountingclerk", "ja-JP", "会计职员_jp", "会计职员"),
            // entity.vendor.accountingclerk
            new TranslationSeedItem("entity.vendor.accountingclerk", "zh-CN", "会计职员", "会计职员"),
            // entity.vendor.accountingclerk
            new TranslationSeedItem("entity.vendor.accountingclerk", "zh-HK", "会计职员_hk", "会计职员"),

            // entity.vendor.ouraccountatvendor
            new TranslationSeedItem("entity.vendor.ouraccountatvendor", "en-US", "社内经销商_us", "我们带有经销商的帐目号"),
            // entity.vendor.ouraccountatvendor
            new TranslationSeedItem("entity.vendor.ouraccountatvendor", "ja-JP", "社内经销商_jp", "我们带有经销商的帐目号"),
            // entity.vendor.ouraccountatvendor
            new TranslationSeedItem("entity.vendor.ouraccountatvendor", "zh-CN", "社内经销商", "我们带有经销商的帐目号"),
            // entity.vendor.ouraccountatvendor
            new TranslationSeedItem("entity.vendor.ouraccountatvendor", "zh-HK", "社内经销商_hk", "我们带有经销商的帐目号"),

            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "en-US", "经销商联系人_us", "经销商联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "ja-JP", "经销商联系人_jp", "经销商联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "zh-CN", "经销商联系人", "经销商联系人"),
            // entity.vendor.contactperson
            new TranslationSeedItem("entity.vendor.contactperson", "zh-HK", "经销商联系人_hk", "经销商联系人"),

            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "en-US", "经销商联系电话_us", "经销商联系电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "ja-JP", "经销商联系电话_jp", "经销商联系电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "zh-CN", "经销商联系电话", "经销商联系电话"),
            // entity.vendor.contactphone
            new TranslationSeedItem("entity.vendor.contactphone", "zh-HK", "经销商联系电话_hk", "经销商联系电话"),

            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "en-US", "经销商联系邮件_us", "经销商联系邮件（邮箱地址短字符串 40 不够）"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "ja-JP", "经销商联系邮件_jp", "经销商联系邮件（邮箱地址短字符串 40 不够）"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "zh-CN", "经销商联系邮件", "经销商联系邮件（邮箱地址短字符串 40 不够）"),
            // entity.vendor.contactemail
            new TranslationSeedItem("entity.vendor.contactemail", "zh-HK", "经销商联系邮件_hk", "经销商联系邮件（邮箱地址短字符串 40 不够）"),

            // entity.vendor.withholdingtaxcountry
            new TranslationSeedItem("entity.vendor.withholdingtaxcountry", "en-US", "代扣所得税国家_us", "代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.withholdingtaxcountry
            new TranslationSeedItem("entity.vendor.withholdingtaxcountry", "ja-JP", "代扣所得税国家_jp", "代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.withholdingtaxcountry
            new TranslationSeedItem("entity.vendor.withholdingtaxcountry", "zh-CN", "代扣所得税国家", "代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.vendor.withholdingtaxcountry
            new TranslationSeedItem("entity.vendor.withholdingtaxcountry", "zh-HK", "代扣所得税国家_hk", "代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.vendor.withholdingtaxtype
            new TranslationSeedItem("entity.vendor.withholdingtaxtype", "en-US", "预扣税类型_us", "预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）"),
            // entity.vendor.withholdingtaxtype
            new TranslationSeedItem("entity.vendor.withholdingtaxtype", "ja-JP", "预扣税类型_jp", "预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）"),
            // entity.vendor.withholdingtaxtype
            new TranslationSeedItem("entity.vendor.withholdingtaxtype", "zh-CN", "预扣税类型", "预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）"),
            // entity.vendor.withholdingtaxtype
            new TranslationSeedItem("entity.vendor.withholdingtaxtype", "zh-HK", "预扣税类型_hk", "预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）"),

            // entity.vendor.withholdingtaxcode
            new TranslationSeedItem("entity.vendor.withholdingtaxcode", "en-US", "预扣税代码_us", "预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.vendor.withholdingtaxcode
            new TranslationSeedItem("entity.vendor.withholdingtaxcode", "ja-JP", "预扣税代码_jp", "预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.vendor.withholdingtaxcode
            new TranslationSeedItem("entity.vendor.withholdingtaxcode", "zh-CN", "预扣税代码", "预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.vendor.withholdingtaxcode
            new TranslationSeedItem("entity.vendor.withholdingtaxcode", "zh-HK", "预扣税代码_hk", "预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "en-US", "采购组织_us", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "ja-JP", "采购组织_jp", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "zh-CN", "采购组织", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.vendor.purchasingorganization
            new TranslationSeedItem("entity.vendor.purchasingorganization", "zh-HK", "采购组织_hk", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.vendor.ordercurrencycode
            new TranslationSeedItem("entity.vendor.ordercurrencycode", "en-US", "订单货币_us", "订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.vendor.ordercurrencycode
            new TranslationSeedItem("entity.vendor.ordercurrencycode", "ja-JP", "订单货币_jp", "订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.vendor.ordercurrencycode
            new TranslationSeedItem("entity.vendor.ordercurrencycode", "zh-CN", "订单货币", "订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.vendor.ordercurrencycode
            new TranslationSeedItem("entity.vendor.ordercurrencycode", "zh-HK", "订单货币_hk", "订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "en-US", "国际贸易条款1_us", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "ja-JP", "国际贸易条款1_jp", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "zh-CN", "国际贸易条款1", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.vendor.incoterms1
            new TranslationSeedItem("entity.vendor.incoterms1", "zh-HK", "国际贸易条款1_hk", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),

            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "en-US", "国际贸易条款2_us", "国际贸易条款2（地点说明）"),
            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "ja-JP", "国际贸易条款2_jp", "国际贸易条款2（地点说明）"),
            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "zh-CN", "国际贸易条款2", "国际贸易条款2（地点说明）"),
            // entity.vendor.incoterms2
            new TranslationSeedItem("entity.vendor.incoterms2", "zh-HK", "国际贸易条款2_hk", "国际贸易条款2（地点说明）"),

            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "en-US", "定价日期控制_us", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "ja-JP", "定价日期控制_jp", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "zh-CN", "定价日期控制", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),
            // entity.vendor.pricingdatecontrol
            new TranslationSeedItem("entity.vendor.pricingdatecontrol", "zh-HK", "定价日期控制_hk", "定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）"),

            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "en-US", "基于收货的发票校验_us", "基于收货的发票校验（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "ja-JP", "基于收货的发票校验_jp", "基于收货的发票校验（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "zh-CN", "基于收货的发票校验", "基于收货的发票校验（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.grbasedinvoiceinspection
            new TranslationSeedItem("entity.vendor.grbasedinvoiceinspection", "zh-HK", "基于收货的发票校验_hk", "基于收货的发票校验（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "en-US", "自动评估GRR结算交货_us", "自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "ja-JP", "自动评估GRR结算交货_jp", "自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "zh-CN", "自动评估GRR结算交货", "自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.evaluatedreceiptsettlement
            new TranslationSeedItem("entity.vendor.evaluatedreceiptsettlement", "zh-HK", "自动评估GRR结算交货_hk", "自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.returnersflag
            new TranslationSeedItem("entity.vendor.returnersflag", "en-US", "自动评估GR结算保留_us", "自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.returnersflag
            new TranslationSeedItem("entity.vendor.returnersflag", "ja-JP", "自动评估GR结算保留_jp", "自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.returnersflag
            new TranslationSeedItem("entity.vendor.returnersflag", "zh-CN", "自动评估GR结算保留", "自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.returnersflag
            new TranslationSeedItem("entity.vendor.returnersflag", "zh-HK", "自动评估GR结算保留_hk", "自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "en-US", "自动建立采购订单_us", "自动建立采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "ja-JP", "自动建立采购订单_jp", "自动建立采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "zh-CN", "自动建立采购订单", "自动建立采购订单（字典 sys_yes_no；0=否 1=是）"),
            // entity.vendor.automaticpurchaseorder
            new TranslationSeedItem("entity.vendor.automaticpurchaseorder", "zh-HK", "自动建立采购订单_hk", "自动建立采购订单（字典 sys_yes_no；0=否 1=是）"),

            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.vendor.purchasegroup
            new TranslationSeedItem("entity.vendor.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.vendor.taxnumber1
            new TranslationSeedItem("entity.vendor.taxnumber1", "en-US", "税号1_us", "税号1"),
            // entity.vendor.taxnumber1
            new TranslationSeedItem("entity.vendor.taxnumber1", "ja-JP", "税号1_jp", "税号1"),
            // entity.vendor.taxnumber1
            new TranslationSeedItem("entity.vendor.taxnumber1", "zh-CN", "税号1", "税号1"),
            // entity.vendor.taxnumber1
            new TranslationSeedItem("entity.vendor.taxnumber1", "zh-HK", "税号1_hk", "税号1"),

            // entity.vendor.taxnumber2
            new TranslationSeedItem("entity.vendor.taxnumber2", "en-US", "税号2_us", "税号2"),
            // entity.vendor.taxnumber2
            new TranslationSeedItem("entity.vendor.taxnumber2", "ja-JP", "税号2_jp", "税号2"),
            // entity.vendor.taxnumber2
            new TranslationSeedItem("entity.vendor.taxnumber2", "zh-CN", "税号2", "税号2"),
            // entity.vendor.taxnumber2
            new TranslationSeedItem("entity.vendor.taxnumber2", "zh-HK", "税号2_hk", "税号2"),

            // entity.vendor.taxnumber3
            new TranslationSeedItem("entity.vendor.taxnumber3", "en-US", "税号3_us", "税号3（来源主数据最长 60）"),
            // entity.vendor.taxnumber3
            new TranslationSeedItem("entity.vendor.taxnumber3", "ja-JP", "税号3_jp", "税号3（来源主数据最长 60）"),
            // entity.vendor.taxnumber3
            new TranslationSeedItem("entity.vendor.taxnumber3", "zh-CN", "税号3", "税号3（来源主数据最长 60）"),
            // entity.vendor.taxnumber3
            new TranslationSeedItem("entity.vendor.taxnumber3", "zh-HK", "税号3_hk", "税号3（来源主数据最长 60）"),

            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "en-US", "信用等级_us", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "ja-JP", "信用等级_jp", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "zh-CN", "信用等级", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.vendor.creditlevel
            new TranslationSeedItem("entity.vendor.creditlevel", "zh-HK", "信用等级_hk", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),

            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "en-US", "经销商等级_us", "经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "ja-JP", "经销商等级_jp", "经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "zh-CN", "经销商等级", "经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.vendor.level
            new TranslationSeedItem("entity.vendor.level", "zh-HK", "经销商等级_hk", "经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),

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
