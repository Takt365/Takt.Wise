// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktPlantI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPlant 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

/// <summary>
/// TaktPlant 实体国际化翻译种子（键前缀 entity.plant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPlantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPlant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 plant 实体翻译...", tenantCode);

        foreach (var item in GetPlantTranslations())
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

        TaktLogger.Information("TaktPlant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPlant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.plant._self / entity.plant.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPlantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "en-US", "Plant Information_us", "实体名称"),
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "ja-JP", "Takt工厂信息_jp", "实体名称"),
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "zh-CN", "Takt工厂信息", "实体名称"),
            // entity.plant._self
            new TranslationSeedItem("entity.plant._self", "zh-HK", "Takt工厂信息_hk", "实体名称"),

            // entity.plant.name1
            new TranslationSeedItem("entity.plant.name1", "en-US", "工厂名称1_us", "工厂名称1"),
            // entity.plant.name1
            new TranslationSeedItem("entity.plant.name1", "ja-JP", "工厂名称1_jp", "工厂名称1"),
            // entity.plant.name1
            new TranslationSeedItem("entity.plant.name1", "zh-CN", "工厂名称1", "工厂名称1"),
            // entity.plant.name1
            new TranslationSeedItem("entity.plant.name1", "zh-HK", "工厂名称1_hk", "工厂名称1"),

            // entity.plant.name2
            new TranslationSeedItem("entity.plant.name2", "en-US", "工厂名称2_us", "工厂名称2"),
            // entity.plant.name2
            new TranslationSeedItem("entity.plant.name2", "ja-JP", "工厂名称2_jp", "工厂名称2"),
            // entity.plant.name2
            new TranslationSeedItem("entity.plant.name2", "zh-CN", "工厂名称2", "工厂名称2"),
            // entity.plant.name2
            new TranslationSeedItem("entity.plant.name2", "zh-HK", "工厂名称2_hk", "工厂名称2"),

            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "en-US", "工厂简称_us", "工厂简称"),
            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "ja-JP", "工厂简称_jp", "工厂简称"),
            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "zh-CN", "工厂简称", "工厂简称"),
            // entity.plant.shortname
            new TranslationSeedItem("entity.plant.shortname", "zh-HK", "工厂简称_hk", "工厂简称"),

            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "en-US", "编码代号_us", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "ja-JP", "编码代号_jp", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "zh-CN", "编码代号", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),
            // entity.plant.codealias
            new TranslationSeedItem("entity.plant.codealias", "zh-HK", "编码代号_hk", "编码代号（如 TKC、TCJ、DTA；前端字典录入）"),

            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "en-US", "企业性质_us", "企业性质（字典 sys_enterprise_nature；DictValue=150 等）"),
            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "ja-JP", "企业性质_jp", "企业性质（字典 sys_enterprise_nature；DictValue=150 等）"),
            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "zh-CN", "企业性质", "企业性质（字典 sys_enterprise_nature；DictValue=150 等）"),
            // entity.plant.enterprisenature
            new TranslationSeedItem("entity.plant.enterprisenature", "zh-HK", "企业性质_hk", "企业性质（字典 sys_enterprise_nature；DictValue=150 等）"),

            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "en-US", "行业属性_us", "行业属性（字典 sys_industry_attribute；DictValue=C 等）"),
            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "ja-JP", "行业属性_jp", "行业属性（字典 sys_industry_attribute；DictValue=C 等）"),
            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "zh-CN", "行业属性", "行业属性（字典 sys_industry_attribute；DictValue=C 等）"),
            // entity.plant.industryattribute
            new TranslationSeedItem("entity.plant.industryattribute", "zh-HK", "行业属性_hk", "行业属性（字典 sys_industry_attribute；DictValue=C 等）"),

            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "en-US", "企业规模_us", "企业规模（字典 sys_enterprise_scale；DictValue=M 等）"),
            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "ja-JP", "企业规模_jp", "企业规模（字典 sys_enterprise_scale；DictValue=M 等）"),
            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "zh-CN", "企业规模", "企业规模（字典 sys_enterprise_scale；DictValue=M 等）"),
            // entity.plant.enterprisescale
            new TranslationSeedItem("entity.plant.enterprisescale", "zh-HK", "企业规模_hk", "企业规模（字典 sys_enterprise_scale；DictValue=M 等）"),

            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "en-US", "经营范围_us", "经营范围"),
            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "ja-JP", "经营范围_jp", "经营范围"),
            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "zh-CN", "经营范围", "经营范围"),
            // entity.plant.businessscope
            new TranslationSeedItem("entity.plant.businessscope", "zh-HK", "经营范围_hk", "经营范围"),

            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.plant.registrationaddress1
            new TranslationSeedItem("entity.plant.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.plant.registrationaddress2
            new TranslationSeedItem("entity.plant.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.plant.registrationregion
            new TranslationSeedItem("entity.plant.registrationregion", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.plant.registrationprovince
            new TranslationSeedItem("entity.plant.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.plant.registrationcity
            new TranslationSeedItem("entity.plant.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "en-US", "经营国家_us", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "ja-JP", "经营国家_jp", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "zh-CN", "经营国家", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.plant.businessregion
            new TranslationSeedItem("entity.plant.businessregion", "zh-HK", "经营国家_hk", "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "en-US", "经营地区-省_us", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "ja-JP", "经营地区-省_jp", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "zh-CN", "经营地区-省", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.plant.businessprovince
            new TranslationSeedItem("entity.plant.businessprovince", "zh-HK", "经营地区-省_hk", "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "en-US", "经营地区-市_us", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "ja-JP", "经营地区-市_jp", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "zh-CN", "经营地区-市", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.plant.businesscity
            new TranslationSeedItem("entity.plant.businesscity", "zh-HK", "经营地区-市_hk", "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "en-US", "经营地址1_us", "经营地址1"),
            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "ja-JP", "经营地址1_jp", "经营地址1"),
            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "zh-CN", "经营地址1", "经营地址1"),
            // entity.plant.businessaddress1
            new TranslationSeedItem("entity.plant.businessaddress1", "zh-HK", "经营地址1_hk", "经营地址1"),

            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "en-US", "经营地址2_us", "经营地址2"),
            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "ja-JP", "经营地址2_jp", "经营地址2"),
            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "zh-CN", "经营地址2", "经营地址2"),
            // entity.plant.businessaddress2
            new TranslationSeedItem("entity.plant.businessaddress2", "zh-HK", "经营地址2_hk", "经营地址2"),

            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "en-US", "工厂地址1_us", "工厂地址1"),
            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "ja-JP", "工厂地址1_jp", "工厂地址1"),
            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "zh-CN", "工厂地址1", "工厂地址1"),
            // entity.plant.address1
            new TranslationSeedItem("entity.plant.address1", "zh-HK", "工厂地址1_hk", "工厂地址1"),

            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "en-US", "工厂地址2_us", "工厂地址2"),
            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "ja-JP", "工厂地址2_jp", "工厂地址2"),
            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "zh-CN", "工厂地址2", "工厂地址2"),
            // entity.plant.address2
            new TranslationSeedItem("entity.plant.address2", "zh-HK", "工厂地址2_hk", "工厂地址2"),

            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "en-US", "工厂电话_us", "工厂电话"),
            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "ja-JP", "工厂电话_jp", "工厂电话"),
            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "zh-CN", "工厂电话", "工厂电话"),
            // entity.plant.phone
            new TranslationSeedItem("entity.plant.phone", "zh-HK", "工厂电话_hk", "工厂电话"),

            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "en-US", "工厂邮箱_us", "工厂邮箱"),
            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "ja-JP", "工厂邮箱_jp", "工厂邮箱"),
            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "zh-CN", "工厂邮箱", "工厂邮箱"),
            // entity.plant.email
            new TranslationSeedItem("entity.plant.email", "zh-HK", "工厂邮箱_hk", "工厂邮箱"),

            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "en-US", "工厂传真_us", "工厂传真"),
            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "ja-JP", "工厂传真_jp", "工厂传真"),
            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "zh-CN", "工厂传真", "工厂传真"),
            // entity.plant.fax
            new TranslationSeedItem("entity.plant.fax", "zh-HK", "工厂传真_hk", "工厂传真"),

            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "en-US", "工厂网站_us", "工厂网站"),
            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "ja-JP", "工厂网站_jp", "工厂网站"),
            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "zh-CN", "工厂网站", "工厂网站"),
            // entity.plant.website
            new TranslationSeedItem("entity.plant.website", "zh-HK", "工厂网站_hk", "工厂网站"),

            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "en-US", "统一社会信用代码_us", "统一社会信用代码"),
            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "ja-JP", "统一社会信用代码_jp", "统一社会信用代码"),
            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "zh-CN", "统一社会信用代码", "统一社会信用代码"),
            // entity.plant.unifiedsocialcreditcode
            new TranslationSeedItem("entity.plant.unifiedsocialcreditcode", "zh-HK", "统一社会信用代码_hk", "统一社会信用代码"),

            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "en-US", "税务登记号_us", "税务登记号"),
            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "ja-JP", "税务登记号_jp", "税务登记号"),
            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "zh-CN", "税务登记号", "税务登记号"),
            // entity.plant.taxregistrationnumber
            new TranslationSeedItem("entity.plant.taxregistrationnumber", "zh-HK", "税务登记号_hk", "税务登记号"),

            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "en-US", "法定代表人_us", "法定代表人"),
            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "ja-JP", "法定代表人_jp", "法定代表人"),
            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "zh-CN", "法定代表人", "法定代表人"),
            // entity.plant.legalrepresentative
            new TranslationSeedItem("entity.plant.legalrepresentative", "zh-HK", "法定代表人_hk", "法定代表人"),

            // entity.plant.manageruserid
            new TranslationSeedItem("entity.plant.manageruserid", "en-US", "工厂负责人用户ID_us", "工厂负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.plant.manageruserid
            new TranslationSeedItem("entity.plant.manageruserid", "ja-JP", "工厂负责人用户ID_jp", "工厂负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.plant.manageruserid
            new TranslationSeedItem("entity.plant.manageruserid", "zh-CN", "工厂负责人用户ID", "工厂负责人（选项 TaktUsers/options；DictValue=Id）"),
            // entity.plant.manageruserid
            new TranslationSeedItem("entity.plant.manageruserid", "zh-HK", "工厂负责人用户ID_hk", "工厂负责人（选项 TaktUsers/options；DictValue=Id）"),

            // entity.plant.managerusername
            new TranslationSeedItem("entity.plant.managerusername", "en-US", "工厂负责人名称_us", "工厂负责人名称（冗余：按 PlantManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.plant.managerusername
            new TranslationSeedItem("entity.plant.managerusername", "ja-JP", "工厂负责人名称_jp", "工厂负责人名称（冗余：按 PlantManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.plant.managerusername
            new TranslationSeedItem("entity.plant.managerusername", "zh-CN", "工厂负责人名称", "工厂负责人名称（冗余：按 PlantManagerUserId 取 TaktUser.UserName 联动）"),
            // entity.plant.managerusername
            new TranslationSeedItem("entity.plant.managerusername", "zh-HK", "工厂负责人名称_hk", "工厂负责人名称（冗余：按 PlantManagerUserId 取 TaktUser.UserName 联动）"),

            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "en-US", "注册资本_us", "注册资本（万元）"),
            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "ja-JP", "注册资本_jp", "注册资本（万元）"),
            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "zh-CN", "注册资本", "注册资本（万元）"),
            // entity.plant.registeredcapital
            new TranslationSeedItem("entity.plant.registeredcapital", "zh-HK", "注册资本_hk", "注册资本（万元）"),

            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "en-US", "成立日期_us", "成立日期"),
            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "ja-JP", "成立日期_jp", "成立日期"),
            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "zh-CN", "成立日期", "成立日期"),
            // entity.plant.establishmentdate
            new TranslationSeedItem("entity.plant.establishmentdate", "zh-HK", "成立日期_hk", "成立日期"),

            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "en-US", "关闭日期_us", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "ja-JP", "关闭日期_jp", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "zh-CN", "关闭日期", "关闭日期（注销/停业；未关闭则为 null）"),
            // entity.plant.closingdate
            new TranslationSeedItem("entity.plant.closingdate", "zh-HK", "关闭日期_hk", "关闭日期（注销/停业；未关闭则为 null）"),

            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "en-US", "存续状态_us", "存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）"),
            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "ja-JP", "存续状态_jp", "存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）"),
            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "zh-CN", "存续状态", "存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）"),
            // entity.plant.existence
            new TranslationSeedItem("entity.plant.existence", "zh-HK", "存续状态_hk", "存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）"),

            // entity.plant.bankcode
            new TranslationSeedItem("entity.plant.bankcode", "en-US", "银行代码_us", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.plant.bankcode
            new TranslationSeedItem("entity.plant.bankcode", "ja-JP", "银行代码_jp", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.plant.bankcode
            new TranslationSeedItem("entity.plant.bankcode", "zh-CN", "银行代码", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),
            // entity.plant.bankcode
            new TranslationSeedItem("entity.plant.bankcode", "zh-HK", "银行代码_hk", "银行代码（选项 TaktBanks/options；DictValue=BankCode）"),

            // entity.plant.bankaccount
            new TranslationSeedItem("entity.plant.bankaccount", "en-US", "银行帐号_us", "银行帐号"),
            // entity.plant.bankaccount
            new TranslationSeedItem("entity.plant.bankaccount", "ja-JP", "银行帐号_jp", "银行帐号"),
            // entity.plant.bankaccount
            new TranslationSeedItem("entity.plant.bankaccount", "zh-CN", "银行帐号", "银行帐号"),
            // entity.plant.bankaccount
            new TranslationSeedItem("entity.plant.bankaccount", "zh-HK", "银行帐号_hk", "银行帐号"),

            // entity.plant.accountholder
            new TranslationSeedItem("entity.plant.accountholder", "en-US", "帐户持有人_us", "帐户持有人"),
            // entity.plant.accountholder
            new TranslationSeedItem("entity.plant.accountholder", "ja-JP", "帐户持有人_jp", "帐户持有人"),
            // entity.plant.accountholder
            new TranslationSeedItem("entity.plant.accountholder", "zh-CN", "帐户持有人", "帐户持有人"),
            // entity.plant.accountholder
            new TranslationSeedItem("entity.plant.accountholder", "zh-HK", "帐户持有人_hk", "帐户持有人"),

            // entity.plant.purchasingorganization
            new TranslationSeedItem("entity.plant.purchasingorganization", "en-US", "采购组织_us", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.plant.purchasingorganization
            new TranslationSeedItem("entity.plant.purchasingorganization", "ja-JP", "采购组织_jp", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.plant.purchasingorganization
            new TranslationSeedItem("entity.plant.purchasingorganization", "zh-CN", "采购组织", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),
            // entity.plant.purchasingorganization
            new TranslationSeedItem("entity.plant.purchasingorganization", "zh-HK", "采购组织_hk", "采购组织（选项 TaktPlants/options；DictValue=PlantCode）"),

            // entity.plant.salesorganization
            new TranslationSeedItem("entity.plant.salesorganization", "en-US", "销售组织_us", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.plant.salesorganization
            new TranslationSeedItem("entity.plant.salesorganization", "ja-JP", "销售组织_jp", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.plant.salesorganization
            new TranslationSeedItem("entity.plant.salesorganization", "zh-CN", "销售组织", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.plant.salesorganization
            new TranslationSeedItem("entity.plant.salesorganization", "zh-HK", "销售组织_hk", "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）"),

            // entity.plant.materialrequirementsplanning
            new TranslationSeedItem("entity.plant.materialrequirementsplanning", "en-US", "物料需求计划_us", "物料需求计划（MRP 范围/控制；对齐）"),
            // entity.plant.materialrequirementsplanning
            new TranslationSeedItem("entity.plant.materialrequirementsplanning", "ja-JP", "物料需求计划_jp", "物料需求计划（MRP 范围/控制；对齐）"),
            // entity.plant.materialrequirementsplanning
            new TranslationSeedItem("entity.plant.materialrequirementsplanning", "zh-CN", "物料需求计划", "物料需求计划（MRP 范围/控制；对齐）"),
            // entity.plant.materialrequirementsplanning
            new TranslationSeedItem("entity.plant.materialrequirementsplanning", "zh-HK", "物料需求计划_hk", "物料需求计划（MRP 范围/控制；对齐）"),

            // entity.plant.distributionchannel
            new TranslationSeedItem("entity.plant.distributionchannel", "en-US", "分销渠道_us", "分销渠道"),
            // entity.plant.distributionchannel
            new TranslationSeedItem("entity.plant.distributionchannel", "ja-JP", "分销渠道_jp", "分销渠道"),
            // entity.plant.distributionchannel
            new TranslationSeedItem("entity.plant.distributionchannel", "zh-CN", "分销渠道", "分销渠道"),
            // entity.plant.distributionchannel
            new TranslationSeedItem("entity.plant.distributionchannel", "zh-HK", "分销渠道_hk", "分销渠道"),

            // entity.plant.intercompanybillingproductgroup
            new TranslationSeedItem("entity.plant.intercompanybillingproductgroup", "en-US", "公司间出具发票产品组_us", "公司间出具发票产品组（产品组/Division）"),
            // entity.plant.intercompanybillingproductgroup
            new TranslationSeedItem("entity.plant.intercompanybillingproductgroup", "ja-JP", "公司间出具发票产品组_jp", "公司间出具发票产品组（产品组/Division）"),
            // entity.plant.intercompanybillingproductgroup
            new TranslationSeedItem("entity.plant.intercompanybillingproductgroup", "zh-CN", "公司间出具发票产品组", "公司间出具发票产品组（产品组/Division）"),
            // entity.plant.intercompanybillingproductgroup
            new TranslationSeedItem("entity.plant.intercompanybillingproductgroup", "zh-HK", "公司间出具发票产品组_hk", "公司间出具发票产品组（产品组/Division）"),

            // entity.plant.taxindicator
            new TranslationSeedItem("entity.plant.taxindicator", "en-US", "税收标识_us", "税收标识"),
            // entity.plant.taxindicator
            new TranslationSeedItem("entity.plant.taxindicator", "ja-JP", "税收标识_jp", "税收标识"),
            // entity.plant.taxindicator
            new TranslationSeedItem("entity.plant.taxindicator", "zh-CN", "税收标识", "税收标识"),
            // entity.plant.taxindicator
            new TranslationSeedItem("entity.plant.taxindicator", "zh-HK", "税收标识_hk", "税收标识"),

            // entity.plant.valuationarea
            new TranslationSeedItem("entity.plant.valuationarea", "en-US", "评估范围_us", "评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）"),
            // entity.plant.valuationarea
            new TranslationSeedItem("entity.plant.valuationarea", "ja-JP", "评估范围_jp", "评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）"),
            // entity.plant.valuationarea
            new TranslationSeedItem("entity.plant.valuationarea", "zh-CN", "评估范围", "评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）"),
            // entity.plant.valuationarea
            new TranslationSeedItem("entity.plant.valuationarea", "zh-HK", "评估范围_hk", "评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）"),

            // entity.plant.vendornumber
            new TranslationSeedItem("entity.plant.vendornumber", "en-US", "工厂供应商号码_us", "工厂供应商号码（工厂作为供应商）"),
            // entity.plant.vendornumber
            new TranslationSeedItem("entity.plant.vendornumber", "ja-JP", "工厂供应商号码_jp", "工厂供应商号码（工厂作为供应商）"),
            // entity.plant.vendornumber
            new TranslationSeedItem("entity.plant.vendornumber", "zh-CN", "工厂供应商号码", "工厂供应商号码（工厂作为供应商）"),
            // entity.plant.vendornumber
            new TranslationSeedItem("entity.plant.vendornumber", "zh-HK", "工厂供应商号码_hk", "工厂供应商号码（工厂作为供应商）"),

            // entity.plant.customernumber
            new TranslationSeedItem("entity.plant.customernumber", "en-US", "客户编码-工厂_us", "客户编码-工厂（工厂作为客户）"),
            // entity.plant.customernumber
            new TranslationSeedItem("entity.plant.customernumber", "ja-JP", "客户编码-工厂_jp", "客户编码-工厂（工厂作为客户）"),
            // entity.plant.customernumber
            new TranslationSeedItem("entity.plant.customernumber", "zh-CN", "客户编码-工厂", "客户编码-工厂（工厂作为客户）"),
            // entity.plant.customernumber
            new TranslationSeedItem("entity.plant.customernumber", "zh-HK", "客户编码-工厂_hk", "客户编码-工厂（工厂作为客户）"),

            // entity.plant.factorycalendar
            new TranslationSeedItem("entity.plant.factorycalendar", "en-US", "工厂日历_us", "工厂日历"),
            // entity.plant.factorycalendar
            new TranslationSeedItem("entity.plant.factorycalendar", "ja-JP", "工厂日历_jp", "工厂日历"),
            // entity.plant.factorycalendar
            new TranslationSeedItem("entity.plant.factorycalendar", "zh-CN", "工厂日历", "工厂日历"),
            // entity.plant.factorycalendar
            new TranslationSeedItem("entity.plant.factorycalendar", "zh-HK", "工厂日历_hk", "工厂日历"),

            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "en-US", "关联公司_us", "关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "ja-JP", "关联公司_jp", "关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "zh-CN", "关联公司", "关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）"),
            // entity.plant.relatedcompany
            new TranslationSeedItem("entity.plant.relatedcompany", "zh-HK", "关联公司_hk", "关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）"),

            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.plant.sortorder
            new TranslationSeedItem("entity.plant.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "en-US", "工厂状态_us", "工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "ja-JP", "工厂状态_jp", "工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "zh-CN", "工厂状态", "工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.plant.status
            new TranslationSeedItem("entity.plant.status", "zh-HK", "工厂状态_hk", "工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
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
        translation.ResourceGroup = "Materials";
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
