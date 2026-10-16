// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktCustomerI18nSeedData.cs
// 创建时间：2026-10-14
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
            new TranslationSeedItem("entity.customer.code", "en-US", "客户编码_us", "客户编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户编码规则生成并展示，非手输；单据类型菜单：客户信息）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "ja-JP", "客户编码_jp", "客户编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户编码规则生成并展示，非手输；单据类型菜单：客户信息）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "zh-CN", "客户编码", "客户编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户编码规则生成并展示，非手输；单据类型菜单：客户信息）"),
            // entity.customer.code
            new TranslationSeedItem("entity.customer.code", "zh-HK", "客户编码_hk", "客户编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户编码规则生成并展示，非手输；单据类型菜单：客户信息）"),

            // entity.customer.accountgroup
            new TranslationSeedItem("entity.customer.accountgroup", "en-US", "科目组_us", "科目组"),
            // entity.customer.accountgroup
            new TranslationSeedItem("entity.customer.accountgroup", "ja-JP", "科目组_jp", "科目组"),
            // entity.customer.accountgroup
            new TranslationSeedItem("entity.customer.accountgroup", "zh-CN", "科目组", "科目组"),
            // entity.customer.accountgroup
            new TranslationSeedItem("entity.customer.accountgroup", "zh-HK", "科目组_hk", "科目组"),

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

            // entity.customer.title
            new TranslationSeedItem("entity.customer.title", "en-US", "称谓_us", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),
            // entity.customer.title
            new TranslationSeedItem("entity.customer.title", "ja-JP", "称谓_jp", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),
            // entity.customer.title
            new TranslationSeedItem("entity.customer.title", "zh-CN", "称谓", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),
            // entity.customer.title
            new TranslationSeedItem("entity.customer.title", "zh-HK", "称谓_hk", "称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）"),

            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "en-US", "名称1_us", "名称1"),
            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "ja-JP", "名称1_jp", "名称1"),
            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "zh-CN", "名称1", "名称1"),
            // entity.customer.name1
            new TranslationSeedItem("entity.customer.name1", "zh-HK", "名称1_hk", "名称1"),

            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "en-US", "名称2_us", "名称2"),
            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "ja-JP", "名称2_jp", "名称2"),
            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "zh-CN", "名称2", "名称2"),
            // entity.customer.name2
            new TranslationSeedItem("entity.customer.name2", "zh-HK", "名称2_hk", "名称2"),

            // entity.customer.name3
            new TranslationSeedItem("entity.customer.name3", "en-US", "名称3_us", "名称3"),
            // entity.customer.name3
            new TranslationSeedItem("entity.customer.name3", "ja-JP", "名称3_jp", "名称3"),
            // entity.customer.name3
            new TranslationSeedItem("entity.customer.name3", "zh-CN", "名称3", "名称3"),
            // entity.customer.name3
            new TranslationSeedItem("entity.customer.name3", "zh-HK", "名称3_hk", "名称3"),

            // entity.customer.name4
            new TranslationSeedItem("entity.customer.name4", "en-US", "名称4_us", "名称4"),
            // entity.customer.name4
            new TranslationSeedItem("entity.customer.name4", "ja-JP", "名称4_jp", "名称4"),
            // entity.customer.name4
            new TranslationSeedItem("entity.customer.name4", "zh-CN", "名称4", "名称4"),
            // entity.customer.name4
            new TranslationSeedItem("entity.customer.name4", "zh-HK", "名称4_hk", "名称4"),

            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "en-US", "简称_us", "简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "ja-JP", "简称_jp", "简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "zh-CN", "简称", "简称"),
            // entity.customer.shortname
            new TranslationSeedItem("entity.customer.shortname", "zh-HK", "简称_hk", "简称"),

            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.registrationcountry
            new TranslationSeedItem("entity.customer.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.customer.registrationregion
            new TranslationSeedItem("entity.customer.registrationregion", "en-US", "注册地区_us", "注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.customer.registrationregion
            new TranslationSeedItem("entity.customer.registrationregion", "ja-JP", "注册地区_jp", "注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.customer.registrationregion
            new TranslationSeedItem("entity.customer.registrationregion", "zh-CN", "注册地区", "注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.customer.registrationregion
            new TranslationSeedItem("entity.customer.registrationregion", "zh-HK", "注册地区_hk", "注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "en-US", "注册城市_us", "注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "ja-JP", "注册城市_jp", "注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "zh-CN", "注册城市", "注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.customer.registrationcity
            new TranslationSeedItem("entity.customer.registrationcity", "zh-HK", "注册城市_hk", "注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.customer.addr1
            new TranslationSeedItem("entity.customer.addr1", "en-US", "地址1_us", "地址1（含门牌，短字符串 40 不够）"),
            // entity.customer.addr1
            new TranslationSeedItem("entity.customer.addr1", "ja-JP", "地址1_jp", "地址1（含门牌，短字符串 40 不够）"),
            // entity.customer.addr1
            new TranslationSeedItem("entity.customer.addr1", "zh-CN", "地址1", "地址1（含门牌，短字符串 40 不够）"),
            // entity.customer.addr1
            new TranslationSeedItem("entity.customer.addr1", "zh-HK", "地址1_hk", "地址1（含门牌，短字符串 40 不够）"),

            // entity.customer.addr2
            new TranslationSeedItem("entity.customer.addr2", "en-US", "地址2_us", "地址2（含门牌，短字符串 40 不够）"),
            // entity.customer.addr2
            new TranslationSeedItem("entity.customer.addr2", "ja-JP", "地址2_jp", "地址2（含门牌，短字符串 40 不够）"),
            // entity.customer.addr2
            new TranslationSeedItem("entity.customer.addr2", "zh-CN", "地址2", "地址2（含门牌，短字符串 40 不够）"),
            // entity.customer.addr2
            new TranslationSeedItem("entity.customer.addr2", "zh-HK", "地址2_hk", "地址2（含门牌，短字符串 40 不够）"),

            // entity.customer.addr3
            new TranslationSeedItem("entity.customer.addr3", "en-US", "地址3_us", "地址3（含门牌，短字符串 40 不够）"),
            // entity.customer.addr3
            new TranslationSeedItem("entity.customer.addr3", "ja-JP", "地址3_jp", "地址3（含门牌，短字符串 40 不够）"),
            // entity.customer.addr3
            new TranslationSeedItem("entity.customer.addr3", "zh-CN", "地址3", "地址3（含门牌，短字符串 40 不够）"),
            // entity.customer.addr3
            new TranslationSeedItem("entity.customer.addr3", "zh-HK", "地址3_hk", "地址3（含门牌，短字符串 40 不够）"),

            // entity.customer.addr4
            new TranslationSeedItem("entity.customer.addr4", "en-US", "地址4_us", "地址4（含门牌，短字符串 40 不够）"),
            // entity.customer.addr4
            new TranslationSeedItem("entity.customer.addr4", "ja-JP", "地址4_jp", "地址4（含门牌，短字符串 40 不够）"),
            // entity.customer.addr4
            new TranslationSeedItem("entity.customer.addr4", "zh-CN", "地址4", "地址4（含门牌，短字符串 40 不够）"),
            // entity.customer.addr4
            new TranslationSeedItem("entity.customer.addr4", "zh-HK", "地址4_hk", "地址4（含门牌，短字符串 40 不够）"),

            // entity.customer.addr5
            new TranslationSeedItem("entity.customer.addr5", "en-US", "地址5_us", "地址5（含门牌，短字符串 40 不够）"),
            // entity.customer.addr5
            new TranslationSeedItem("entity.customer.addr5", "ja-JP", "地址5_jp", "地址5（含门牌，短字符串 40 不够）"),
            // entity.customer.addr5
            new TranslationSeedItem("entity.customer.addr5", "zh-CN", "地址5", "地址5（含门牌，短字符串 40 不够）"),
            // entity.customer.addr5
            new TranslationSeedItem("entity.customer.addr5", "zh-HK", "地址5_hk", "地址5（含门牌，短字符串 40 不够）"),

            // entity.customer.postalcode
            new TranslationSeedItem("entity.customer.postalcode", "en-US", "邮政编码_us", "邮政编码"),
            // entity.customer.postalcode
            new TranslationSeedItem("entity.customer.postalcode", "ja-JP", "邮政编码_jp", "邮政编码"),
            // entity.customer.postalcode
            new TranslationSeedItem("entity.customer.postalcode", "zh-CN", "邮政编码", "邮政编码"),
            // entity.customer.postalcode
            new TranslationSeedItem("entity.customer.postalcode", "zh-HK", "邮政编码_hk", "邮政编码"),

            // entity.customer.phone1
            new TranslationSeedItem("entity.customer.phone1", "en-US", "电话1_us", "电话1"),
            // entity.customer.phone1
            new TranslationSeedItem("entity.customer.phone1", "ja-JP", "电话1_jp", "电话1"),
            // entity.customer.phone1
            new TranslationSeedItem("entity.customer.phone1", "zh-CN", "电话1", "电话1"),
            // entity.customer.phone1
            new TranslationSeedItem("entity.customer.phone1", "zh-HK", "电话1_hk", "电话1"),

            // entity.customer.phone2
            new TranslationSeedItem("entity.customer.phone2", "en-US", "电话2_us", "电话2"),
            // entity.customer.phone2
            new TranslationSeedItem("entity.customer.phone2", "ja-JP", "电话2_jp", "电话2"),
            // entity.customer.phone2
            new TranslationSeedItem("entity.customer.phone2", "zh-CN", "电话2", "电话2"),
            // entity.customer.phone2
            new TranslationSeedItem("entity.customer.phone2", "zh-HK", "电话2_hk", "电话2"),

            // entity.customer.faxnumber
            new TranslationSeedItem("entity.customer.faxnumber", "en-US", "传真_us", "传真"),
            // entity.customer.faxnumber
            new TranslationSeedItem("entity.customer.faxnumber", "ja-JP", "传真_jp", "传真"),
            // entity.customer.faxnumber
            new TranslationSeedItem("entity.customer.faxnumber", "zh-CN", "传真", "传真"),
            // entity.customer.faxnumber
            new TranslationSeedItem("entity.customer.faxnumber", "zh-HK", "传真_hk", "传真"),

            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "en-US", "供应商_us", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "ja-JP", "供应商_jp", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "zh-CN", "供应商", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.customer.suppliercode
            new TranslationSeedItem("entity.customer.suppliercode", "zh-HK", "供应商_hk", "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "en-US", "贸易伙伴_us", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "ja-JP", "贸易伙伴_jp", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "zh-CN", "贸易伙伴", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.tradingpartner
            new TranslationSeedItem("entity.customer.tradingpartner", "zh-HK", "贸易伙伴_hk", "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.customer.taxnumber1
            new TranslationSeedItem("entity.customer.taxnumber1", "en-US", "税号1_us", "税号1"),
            // entity.customer.taxnumber1
            new TranslationSeedItem("entity.customer.taxnumber1", "ja-JP", "税号1_jp", "税号1"),
            // entity.customer.taxnumber1
            new TranslationSeedItem("entity.customer.taxnumber1", "zh-CN", "税号1", "税号1"),
            // entity.customer.taxnumber1
            new TranslationSeedItem("entity.customer.taxnumber1", "zh-HK", "税号1_hk", "税号1"),

            // entity.customer.taxnumber2
            new TranslationSeedItem("entity.customer.taxnumber2", "en-US", "税号2_us", "税号2"),
            // entity.customer.taxnumber2
            new TranslationSeedItem("entity.customer.taxnumber2", "ja-JP", "税号2_jp", "税号2"),
            // entity.customer.taxnumber2
            new TranslationSeedItem("entity.customer.taxnumber2", "zh-CN", "税号2", "税号2"),
            // entity.customer.taxnumber2
            new TranslationSeedItem("entity.customer.taxnumber2", "zh-HK", "税号2_hk", "税号2"),

            // entity.customer.taxnumber3
            new TranslationSeedItem("entity.customer.taxnumber3", "en-US", "税号3_us", "税号3（来源主数据最长 60）"),
            // entity.customer.taxnumber3
            new TranslationSeedItem("entity.customer.taxnumber3", "ja-JP", "税号3_jp", "税号3（来源主数据最长 60）"),
            // entity.customer.taxnumber3
            new TranslationSeedItem("entity.customer.taxnumber3", "zh-CN", "税号3", "税号3（来源主数据最长 60）"),
            // entity.customer.taxnumber3
            new TranslationSeedItem("entity.customer.taxnumber3", "zh-HK", "税号3_hk", "税号3（来源主数据最长 60）"),

            // entity.customer.bankcountrycode
            new TranslationSeedItem("entity.customer.bankcountrycode", "en-US", "银行国家_us", "银行国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.bankcountrycode
            new TranslationSeedItem("entity.customer.bankcountrycode", "ja-JP", "银行国家_jp", "银行国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.bankcountrycode
            new TranslationSeedItem("entity.customer.bankcountrycode", "zh-CN", "银行国家", "银行国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.customer.bankcountrycode
            new TranslationSeedItem("entity.customer.bankcountrycode", "zh-HK", "银行国家_hk", "银行国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.customer.bankcode
            new TranslationSeedItem("entity.customer.bankcode", "en-US", "银行代码_us", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.customer.bankcode
            new TranslationSeedItem("entity.customer.bankcode", "ja-JP", "银行代码_jp", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.customer.bankcode
            new TranslationSeedItem("entity.customer.bankcode", "zh-CN", "银行代码", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.customer.bankcode
            new TranslationSeedItem("entity.customer.bankcode", "zh-HK", "银行代码_hk", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.customer.bankaccount
            new TranslationSeedItem("entity.customer.bankaccount", "en-US", "银行账户_us", "银行账户"),
            // entity.customer.bankaccount
            new TranslationSeedItem("entity.customer.bankaccount", "ja-JP", "银行账户_jp", "银行账户"),
            // entity.customer.bankaccount
            new TranslationSeedItem("entity.customer.bankaccount", "zh-CN", "银行账户", "银行账户"),
            // entity.customer.bankaccount
            new TranslationSeedItem("entity.customer.bankaccount", "zh-HK", "银行账户_hk", "银行账户"),

            // entity.customer.bankaccountholder
            new TranslationSeedItem("entity.customer.bankaccountholder", "en-US", "账户持有人_us", "账户持有人"),
            // entity.customer.bankaccountholder
            new TranslationSeedItem("entity.customer.bankaccountholder", "ja-JP", "账户持有人_jp", "账户持有人"),
            // entity.customer.bankaccountholder
            new TranslationSeedItem("entity.customer.bankaccountholder", "zh-CN", "账户持有人", "账户持有人"),
            // entity.customer.bankaccountholder
            new TranslationSeedItem("entity.customer.bankaccountholder", "zh-HK", "账户持有人_hk", "账户持有人"),

            // entity.customer.category
            new TranslationSeedItem("entity.customer.category", "en-US", "客户分类_us", "客户分类（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.customer.category
            new TranslationSeedItem("entity.customer.category", "ja-JP", "客户分类_jp", "客户分类（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.customer.category
            new TranslationSeedItem("entity.customer.category", "zh-CN", "客户分类", "客户分类（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),
            // entity.customer.category
            new TranslationSeedItem("entity.customer.category", "zh-HK", "客户分类_hk", "客户分类（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）"),

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

            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "en-US", "统驭科目_us", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "ja-JP", "统驭科目_jp", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "zh-CN", "统驭科目", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),
            // entity.customer.reconciliationaccount
            new TranslationSeedItem("entity.customer.reconciliationaccount", "zh-HK", "统驭科目_hk", "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）"),

            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "en-US", "付款条件_us", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "ja-JP", "付款条件_jp", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "zh-CN", "付款条件", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),
            // entity.customer.paymentterms
            new TranslationSeedItem("entity.customer.paymentterms", "zh-HK", "付款条件_hk", "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）"),

            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "en-US", "付款方式_us", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),
            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "ja-JP", "付款方式_jp", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),
            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "zh-CN", "付款方式", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),
            // entity.customer.paymentmethod
            new TranslationSeedItem("entity.customer.paymentmethod", "zh-HK", "付款方式_hk", "付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）"),

            // entity.customer.salesdistrict
            new TranslationSeedItem("entity.customer.salesdistrict", "en-US", "销售地区_us", "销售地区"),
            // entity.customer.salesdistrict
            new TranslationSeedItem("entity.customer.salesdistrict", "ja-JP", "销售地区_jp", "销售地区"),
            // entity.customer.salesdistrict
            new TranslationSeedItem("entity.customer.salesdistrict", "zh-CN", "销售地区", "销售地区"),
            // entity.customer.salesdistrict
            new TranslationSeedItem("entity.customer.salesdistrict", "zh-HK", "销售地区_hk", "销售地区"),

            // entity.customer.salesdepartment
            new TranslationSeedItem("entity.customer.salesdepartment", "en-US", "销售部门_us", "销售部门（选项 TaktDepts/options；DictValue=DeptCode）"),
            // entity.customer.salesdepartment
            new TranslationSeedItem("entity.customer.salesdepartment", "ja-JP", "销售部门_jp", "销售部门（选项 TaktDepts/options；DictValue=DeptCode）"),
            // entity.customer.salesdepartment
            new TranslationSeedItem("entity.customer.salesdepartment", "zh-CN", "销售部门", "销售部门（选项 TaktDepts/options；DictValue=DeptCode）"),
            // entity.customer.salesdepartment
            new TranslationSeedItem("entity.customer.salesdepartment", "zh-HK", "销售部门_hk", "销售部门（选项 TaktDepts/options；DictValue=DeptCode）"),

            // entity.customer.salesgroup
            new TranslationSeedItem("entity.customer.salesgroup", "en-US", "销售组_us", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.customer.salesgroup
            new TranslationSeedItem("entity.customer.salesgroup", "ja-JP", "销售组_jp", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.customer.salesgroup
            new TranslationSeedItem("entity.customer.salesgroup", "zh-CN", "销售组", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),
            // entity.customer.salesgroup
            new TranslationSeedItem("entity.customer.salesgroup", "zh-HK", "销售组_hk", "销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）"),

            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "en-US", "货币_us", "货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "ja-JP", "货币_jp", "货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "zh-CN", "货币", "货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),
            // entity.customer.currencycode
            new TranslationSeedItem("entity.customer.currencycode", "zh-HK", "货币_hk", "货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）"),

            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "en-US", "客户定价过程_us", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "ja-JP", "客户定价过程_jp", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "zh-CN", "客户定价过程", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),
            // entity.customer.pricingprocedure
            new TranslationSeedItem("entity.customer.pricingprocedure", "zh-HK", "客户定价过程_hk", "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）"),

            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "en-US", "装运条件_us", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "ja-JP", "装运条件_jp", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "zh-CN", "装运条件", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),
            // entity.customer.shippingconditions
            new TranslationSeedItem("entity.customer.shippingconditions", "zh-HK", "装运条件_hk", "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）"),

            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "en-US", "交货工厂_us", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "ja-JP", "交货工厂_jp", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "zh-CN", "交货工厂", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.customer.deliveringplant
            new TranslationSeedItem("entity.customer.deliveringplant", "zh-HK", "交货工厂_hk", "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "en-US", "国际贸易条款1_us", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "ja-JP", "国际贸易条款1_jp", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "zh-CN", "国际贸易条款1", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),
            // entity.customer.incoterms1
            new TranslationSeedItem("entity.customer.incoterms1", "zh-HK", "国际贸易条款1_hk", "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）"),

            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "en-US", "国际贸易条款2_us", "国际贸易条款2（地点说明）"),
            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "ja-JP", "国际贸易条款2_jp", "国际贸易条款2（地点说明）"),
            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "zh-CN", "国际贸易条款2", "国际贸易条款2（地点说明）"),
            // entity.customer.incoterms2
            new TranslationSeedItem("entity.customer.incoterms2", "zh-HK", "国际贸易条款2_hk", "国际贸易条款2（地点说明）"),

            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "en-US", "帐户分配组_us", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "ja-JP", "帐户分配组_jp", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "zh-CN", "帐户分配组", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),
            // entity.customer.accountassignmentgroup
            new TranslationSeedItem("entity.customer.accountassignmentgroup", "zh-HK", "帐户分配组_hk", "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）"),

            // entity.customer.taxclassification
            new TranslationSeedItem("entity.customer.taxclassification", "en-US", "税分类_us", "税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.customer.taxclassification
            new TranslationSeedItem("entity.customer.taxclassification", "ja-JP", "税分类_jp", "税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.customer.taxclassification
            new TranslationSeedItem("entity.customer.taxclassification", "zh-CN", "税分类", "税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.customer.taxclassification
            new TranslationSeedItem("entity.customer.taxclassification", "zh-HK", "税分类_hk", "税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.customer.contact
            new TranslationSeedItem("entity.customer.contact", "en-US", "客户联系人_us", "客户联系人"),
            // entity.customer.contact
            new TranslationSeedItem("entity.customer.contact", "ja-JP", "客户联系人_jp", "客户联系人"),
            // entity.customer.contact
            new TranslationSeedItem("entity.customer.contact", "zh-CN", "客户联系人", "客户联系人"),
            // entity.customer.contact
            new TranslationSeedItem("entity.customer.contact", "zh-HK", "客户联系人_hk", "客户联系人"),

            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "en-US", "客户联系电话_us", "客户联系电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "ja-JP", "客户联系电话_jp", "客户联系电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "zh-CN", "客户联系电话", "客户联系电话"),
            // entity.customer.phone
            new TranslationSeedItem("entity.customer.phone", "zh-HK", "客户联系电话_hk", "客户联系电话"),

            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "en-US", "客户联系邮件_us", "客户联系邮件（邮箱地址短字符串 40 不够）"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "ja-JP", "客户联系邮件_jp", "客户联系邮件（邮箱地址短字符串 40 不够）"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "zh-CN", "客户联系邮件", "客户联系邮件（邮箱地址短字符串 40 不够）"),
            // entity.customer.email
            new TranslationSeedItem("entity.customer.email", "zh-HK", "客户联系邮件_hk", "客户联系邮件（邮箱地址短字符串 40 不够）"),

            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "en-US", "信用等级_us", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "ja-JP", "信用等级_jp", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "zh-CN", "信用等级", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),
            // entity.customer.creditlevel
            new TranslationSeedItem("entity.customer.creditlevel", "zh-HK", "信用等级_hk", "信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）"),

            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "en-US", "客户等级_us", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "ja-JP", "客户等级_jp", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "zh-CN", "客户等级", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),
            // entity.customer.level
            new TranslationSeedItem("entity.customer.level", "zh-HK", "客户等级_hk", "客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）"),

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
