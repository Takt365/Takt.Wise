// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktCompanyI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCompany 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktCompany 实体国际化翻译种子（键前缀 entity.company.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCompanyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCompany 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 company 实体翻译...", tenantCode);

        foreach (var item in GetCompanyTranslations())
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

        TaktLogger.Information("TaktCompany 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCompany 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.company._self / entity.company.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCompanyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "en-US", "Company Information_us", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "ja-JP", "公司信息_jp", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "zh-CN", "公司信息", "实体名称"),
            // entity.company._self
            new TranslationSeedItem("entity.company._self", "zh-HK", "公司信息_hk", "实体名称"),

            // entity.company.name1
            new TranslationSeedItem("entity.company.name1", "en-US", "公司名称1_us", "公司名称1"),
            // entity.company.name1
            new TranslationSeedItem("entity.company.name1", "ja-JP", "公司名称1_jp", "公司名称1"),
            // entity.company.name1
            new TranslationSeedItem("entity.company.name1", "zh-CN", "公司名称1", "公司名称1"),
            // entity.company.name1
            new TranslationSeedItem("entity.company.name1", "zh-HK", "公司名称1_hk", "公司名称1"),

            // entity.company.name2
            new TranslationSeedItem("entity.company.name2", "en-US", "公司名称2_us", "公司名称2"),
            // entity.company.name2
            new TranslationSeedItem("entity.company.name2", "ja-JP", "公司名称2_jp", "公司名称2"),
            // entity.company.name2
            new TranslationSeedItem("entity.company.name2", "zh-CN", "公司名称2", "公司名称2"),
            // entity.company.name2
            new TranslationSeedItem("entity.company.name2", "zh-HK", "公司名称2_hk", "公司名称2"),

            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "en-US", "公司简称_us", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "ja-JP", "公司简称_jp", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "zh-CN", "公司简称", "公司简称"),
            // entity.company.shortname
            new TranslationSeedItem("entity.company.shortname", "zh-HK", "公司简称_hk", "公司简称"),

            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature）"),
            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature）"),
            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature）"),
            // entity.company.enterprisenature
            new TranslationSeedItem("entity.company.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature）"),

            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute）"),
            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute）"),
            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute）"),
            // entity.company.industryattribute
            new TranslationSeedItem("entity.company.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute）"),

            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "en-US", "企业规模_us", "企业规模（字典 sys_enterprise_scale）"),
            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "ja-JP", "企业规模_jp", "企业规模（字典 sys_enterprise_scale）"),
            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "zh-CN", "企业规模", "企业规模（字典 sys_enterprise_scale）"),
            // entity.company.enterprisescale
            new TranslationSeedItem("entity.company.enterprisescale", "zh-HK", "企业规模_hk", "企业规模（字典 sys_enterprise_scale）"),

            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "en-US", "经营范围_us", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "ja-JP", "经营范围_jp", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "zh-CN", "经营范围", "经营范围"),
            // entity.company.businessscope
            new TranslationSeedItem("entity.company.businessscope", "zh-HK", "经营范围_hk", "经营范围"),

            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.company.registrationaddress1
            new TranslationSeedItem("entity.company.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.company.registrationaddress2
            new TranslationSeedItem("entity.company.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.company.registrationregion
            new TranslationSeedItem("entity.company.registrationregion", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.company.registrationprovince
            new TranslationSeedItem("entity.company.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.company.registrationcity
            new TranslationSeedItem("entity.company.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "en-US", "经营国家_us", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "ja-JP", "经营国家_jp", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "zh-CN", "经营国家", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.company.businessregion
            new TranslationSeedItem("entity.company.businessregion", "zh-HK", "经营国家_hk", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "en-US", "经营地区-省_us", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "ja-JP", "经营地区-省_jp", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "zh-CN", "经营地区-省", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.company.businessprovince
            new TranslationSeedItem("entity.company.businessprovince", "zh-HK", "经营地区-省_hk", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "en-US", "经营地区-市_us", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "ja-JP", "经营地区-市_jp", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "zh-CN", "经营地区-市", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.company.businesscity
            new TranslationSeedItem("entity.company.businesscity", "zh-HK", "经营地区-市_hk", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "en-US", "经营地址1_us", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "ja-JP", "经营地址1_jp", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "zh-CN", "经营地址1", "经营地址1"),
            // entity.company.businessaddress1
            new TranslationSeedItem("entity.company.businessaddress1", "zh-HK", "经营地址1_hk", "经营地址1"),

            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "en-US", "经营地址2_us", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "ja-JP", "经营地址2_jp", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "zh-CN", "经营地址2", "经营地址2"),
            // entity.company.businessaddress2
            new TranslationSeedItem("entity.company.businessaddress2", "zh-HK", "经营地址2_hk", "经营地址2"),

            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "en-US", "公司电话_us", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "ja-JP", "公司电话_jp", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "zh-CN", "公司电话", "公司电话"),
            // entity.company.phone
            new TranslationSeedItem("entity.company.phone", "zh-HK", "公司电话_hk", "公司电话"),

            // entity.company.email
            new TranslationSeedItem("entity.company.email", "en-US", "公司邮箱_us", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "ja-JP", "公司邮箱_jp", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "zh-CN", "公司邮箱", "公司邮箱"),
            // entity.company.email
            new TranslationSeedItem("entity.company.email", "zh-HK", "公司邮箱_hk", "公司邮箱"),

            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "en-US", "公司传真_us", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "ja-JP", "公司传真_jp", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "zh-CN", "公司传真", "公司传真"),
            // entity.company.fax
            new TranslationSeedItem("entity.company.fax", "zh-HK", "公司传真_hk", "公司传真"),

            // entity.company.website
            new TranslationSeedItem("entity.company.website", "en-US", "公司网站_us", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "ja-JP", "公司网站_jp", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "zh-CN", "公司网站", "公司网站"),
            // entity.company.website
            new TranslationSeedItem("entity.company.website", "zh-HK", "公司网站_hk", "公司网站"),

            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "en-US", "统一社会信用代码_us", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "ja-JP", "统一社会信用代码_jp", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "zh-CN", "统一社会信用代码", "统一社会信用代码"),
            // entity.company.unifiedsocialcreditcode
            new TranslationSeedItem("entity.company.unifiedsocialcreditcode", "zh-HK", "统一社会信用代码_hk", "统一社会信用代码"),

            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "en-US", "税务登记号_us", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "ja-JP", "税务登记号_jp", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "zh-CN", "税务登记号", "税务登记号"),
            // entity.company.taxregistrationnumber
            new TranslationSeedItem("entity.company.taxregistrationnumber", "zh-HK", "税务登记号_hk", "税务登记号"),

            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "en-US", "法定代表人_us", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "ja-JP", "法定代表人_jp", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "zh-CN", "法定代表人", "法定代表人"),
            // entity.company.legalrepresentative
            new TranslationSeedItem("entity.company.legalrepresentative", "zh-HK", "法定代表人_hk", "法定代表人"),

            // entity.company.manageruserid
            new TranslationSeedItem("entity.company.manageruserid", "en-US", "公司负责人用户ID_us", "公司负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.company.manageruserid
            new TranslationSeedItem("entity.company.manageruserid", "ja-JP", "公司负责人用户ID_jp", "公司负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.company.manageruserid
            new TranslationSeedItem("entity.company.manageruserid", "zh-CN", "公司负责人用户ID", "公司负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.company.manageruserid
            new TranslationSeedItem("entity.company.manageruserid", "zh-HK", "公司负责人用户ID_hk", "公司负责人（选项 TaktUsers/options；DictValue=Id）"),

            // entity.company.managerusername
            new TranslationSeedItem("entity.company.managerusername", "en-US", "公司负责人名称_us", "公司负责人名称（冗余：按 CompanyManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.company.managerusername
            new TranslationSeedItem("entity.company.managerusername", "ja-JP", "公司负责人名称_jp", "公司负责人名称（冗余：按 CompanyManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.company.managerusername
            new TranslationSeedItem("entity.company.managerusername", "zh-CN", "公司负责人名称", "公司负责人名称（冗余：按 CompanyManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.company.managerusername
            new TranslationSeedItem("entity.company.managerusername", "zh-HK", "公司负责人名称_hk", "公司负责人名称（冗余：按 CompanyManagerUserId 取 TaktUser.UserName 联动）"),

            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "en-US", "注册资本_us", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "ja-JP", "注册资本_jp", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "zh-CN", "注册资本", "注册资本（万元）"),
            // entity.company.registeredcapital
            new TranslationSeedItem("entity.company.registeredcapital", "zh-HK", "注册资本_hk", "注册资本（万元）"),

            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "en-US", "成立日期_us", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "ja-JP", "成立日期_jp", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "zh-CN", "成立日期", "成立日期"),
            // entity.company.establishmentdate
            new TranslationSeedItem("entity.company.establishmentdate", "zh-HK", "成立日期_hk", "成立日期"),

            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "en-US", "关闭日期_us", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "ja-JP", "关闭日期_jp", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "zh-CN", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.company.closingdate
            new TranslationSeedItem("entity.company.closingdate", "zh-HK", "关闭日期_hk", "关闭日期（注销/停业；未关闭则为 null）"),

            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "en-US", "存续状态_us", "存续状态（字典 sys_entity_existence_status）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "ja-JP", "存续状态_jp", "存续状态（字典 sys_entity_existence_status）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "zh-CN", "存续状态", "存续状态（字典 sys_entity_existence_status）"),
            // entity.company.existence
            new TranslationSeedItem("entity.company.existence", "zh-HK", "存续状态_hk", "存续状态（字典 sys_entity_existence_status）"),

            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "en-US", "编码代号_us", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "ja-JP", "编码代号_jp", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "zh-CN", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.company.codealias
            new TranslationSeedItem("entity.company.codealias", "zh-HK", "编码代号_hk", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),

            // entity.company.bankcode
            new TranslationSeedItem("entity.company.bankcode", "en-US", "银行代码_us", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.company.bankcode
            new TranslationSeedItem("entity.company.bankcode", "ja-JP", "银行代码_jp", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.company.bankcode
            new TranslationSeedItem("entity.company.bankcode", "zh-CN", "银行代码", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.company.bankcode
            new TranslationSeedItem("entity.company.bankcode", "zh-HK", "银行代码_hk", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.company.bankaccount
            new TranslationSeedItem("entity.company.bankaccount", "en-US", "银行帐号_us", "银行帐号"),
            // entity.company.bankaccount
            new TranslationSeedItem("entity.company.bankaccount", "ja-JP", "银行帐号_jp", "银行帐号"),
            // entity.company.bankaccount
            new TranslationSeedItem("entity.company.bankaccount", "zh-CN", "银行帐号", "银行帐号"),
            // entity.company.bankaccount
            new TranslationSeedItem("entity.company.bankaccount", "zh-HK", "银行帐号_hk", "银行帐号"),

            // entity.company.accountholder
            new TranslationSeedItem("entity.company.accountholder", "en-US", "帐户持有人_us", "帐户持有人"),
            // entity.company.accountholder
            new TranslationSeedItem("entity.company.accountholder", "ja-JP", "帐户持有人_jp", "帐户持有人"),
            // entity.company.accountholder
            new TranslationSeedItem("entity.company.accountholder", "zh-CN", "帐户持有人", "帐户持有人"),
            // entity.company.accountholder
            new TranslationSeedItem("entity.company.accountholder", "zh-HK", "帐户持有人_hk", "帐户持有人"),

            // entity.company.currencycode
            new TranslationSeedItem("entity.company.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),
            // entity.company.currencycode
            new TranslationSeedItem("entity.company.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),
            // entity.company.currencycode
            new TranslationSeedItem("entity.company.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),
            // entity.company.currencycode
            new TranslationSeedItem("entity.company.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),

            // entity.company.chartofaccounts
            new TranslationSeedItem("entity.company.chartofaccounts", "en-US", "帐目表_us", "帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）"),
            // entity.company.chartofaccounts
            new TranslationSeedItem("entity.company.chartofaccounts", "ja-JP", "帐目表_jp", "帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）"),
            // entity.company.chartofaccounts
            new TranslationSeedItem("entity.company.chartofaccounts", "zh-CN", "帐目表", "帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）"),
            // entity.company.chartofaccounts
            new TranslationSeedItem("entity.company.chartofaccounts", "zh-HK", "帐目表_hk", "帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）"),

            // entity.company.inputtaxcode
            new TranslationSeedItem("entity.company.inputtaxcode", "en-US", "进项税码_us", "进项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.company.inputtaxcode
            new TranslationSeedItem("entity.company.inputtaxcode", "ja-JP", "进项税码_jp", "进项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.company.inputtaxcode
            new TranslationSeedItem("entity.company.inputtaxcode", "zh-CN", "进项税码", "进项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.company.inputtaxcode
            new TranslationSeedItem("entity.company.inputtaxcode", "zh-HK", "进项税码_hk", "进项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.company.outputtaxcode
            new TranslationSeedItem("entity.company.outputtaxcode", "en-US", "销项税码_us", "销项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.company.outputtaxcode
            new TranslationSeedItem("entity.company.outputtaxcode", "ja-JP", "销项税码_jp", "销项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.company.outputtaxcode
            new TranslationSeedItem("entity.company.outputtaxcode", "zh-CN", "销项税码", "销项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.company.outputtaxcode
            new TranslationSeedItem("entity.company.outputtaxcode", "zh-HK", "销项税码_hk", "销项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.company.businessplace
            new TranslationSeedItem("entity.company.businessplace", "en-US", "营业税主体_us", "营业税主体（营业场所）"),
            // entity.company.businessplace
            new TranslationSeedItem("entity.company.businessplace", "ja-JP", "营业税主体_jp", "营业税主体（营业场所）"),
            // entity.company.businessplace
            new TranslationSeedItem("entity.company.businessplace", "zh-CN", "营业税主体", "营业税主体（营业场所）"),
            // entity.company.businessplace
            new TranslationSeedItem("entity.company.businessplace", "zh-HK", "营业税主体_hk", "营业税主体（营业场所）"),

            // entity.company.postingperiodvariant
            new TranslationSeedItem("entity.company.postingperiodvariant", "en-US", "记帐期间变式_us", "记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）"),
            // entity.company.postingperiodvariant
            new TranslationSeedItem("entity.company.postingperiodvariant", "ja-JP", "记帐期间变式_jp", "记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）"),
            // entity.company.postingperiodvariant
            new TranslationSeedItem("entity.company.postingperiodvariant", "zh-CN", "记帐期间变式", "记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）"),
            // entity.company.postingperiodvariant
            new TranslationSeedItem("entity.company.postingperiodvariant", "zh-HK", "记帐期间变式_hk", "记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）"),

            // entity.company.fiscalyearvariant
            new TranslationSeedItem("entity.company.fiscalyearvariant", "en-US", "会计年度变式_us", "会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）"),
            // entity.company.fiscalyearvariant
            new TranslationSeedItem("entity.company.fiscalyearvariant", "ja-JP", "会计年度变式_jp", "会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）"),
            // entity.company.fiscalyearvariant
            new TranslationSeedItem("entity.company.fiscalyearvariant", "zh-CN", "会计年度变式", "会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）"),
            // entity.company.fiscalyearvariant
            new TranslationSeedItem("entity.company.fiscalyearvariant", "zh-HK", "会计年度变式_hk", "会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）"),

            // entity.company.creditcontrolarea
            new TranslationSeedItem("entity.company.creditcontrolarea", "en-US", "贷方控制范围_us", "贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.company.creditcontrolarea
            new TranslationSeedItem("entity.company.creditcontrolarea", "ja-JP", "贷方控制范围_jp", "贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.company.creditcontrolarea
            new TranslationSeedItem("entity.company.creditcontrolarea", "zh-CN", "贷方控制范围", "贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.company.creditcontrolarea
            new TranslationSeedItem("entity.company.creditcontrolarea", "zh-HK", "贷方控制范围_hk", "贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),

            // entity.company.financialmanagementarea
            new TranslationSeedItem("entity.company.financialmanagementarea", "en-US", "财务管理范围_us", "财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.company.financialmanagementarea
            new TranslationSeedItem("entity.company.financialmanagementarea", "ja-JP", "财务管理范围_jp", "财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.company.financialmanagementarea
            new TranslationSeedItem("entity.company.financialmanagementarea", "zh-CN", "财务管理范围", "财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.company.financialmanagementarea
            new TranslationSeedItem("entity.company.financialmanagementarea", "zh-HK", "财务管理范围_hk", "财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）"),

            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.company.sortorder
            new TranslationSeedItem("entity.company.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.company.status
            new TranslationSeedItem("entity.company.status", "en-US", "公司状态_us", "公司状态（字典 sys_normal_disable）"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "ja-JP", "公司状态_jp", "公司状态（字典 sys_normal_disable）"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "zh-CN", "公司状态", "公司状态（字典 sys_normal_disable）"),
            // entity.company.status
            new TranslationSeedItem("entity.company.status", "zh-HK", "公司状态_hk", "公司状态（字典 sys_normal_disable）"),

            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "en-US", "可访问该公司的角色关联_us", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "ja-JP", "可访问该公司的角色关联_jp", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "zh-CN", "可访问该公司的角色关联", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),
            // entity.company.rolecompanies
            new TranslationSeedItem("entity.company.rolecompanies", "zh-HK", "可访问该公司的角色关联_hk", "可访问该公司的角色关联（RBAC，表 takt_identity_role_company）"),

            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "en-US", "可访问该公司的用户关联_us", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "ja-JP", "可访问该公司的用户关联_jp", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "zh-CN", "可访问该公司的用户关联", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
            // entity.company.usercompanies
            new TranslationSeedItem("entity.company.usercompanies", "zh-HK", "可访问该公司的用户关联_hk", "可访问该公司的用户关联（RBAC，表 takt_identity_user_company）"),
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
        translation.ResourceGroup = "Financial";
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
