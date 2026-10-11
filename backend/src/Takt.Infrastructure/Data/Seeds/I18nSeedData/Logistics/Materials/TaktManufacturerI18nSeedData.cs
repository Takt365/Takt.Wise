// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktManufacturerI18nSeedData.cs
// 创建时间：2026-08-04
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktManufacturer 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktManufacturer 实体国际化翻译种子（键前缀 entity.manufacturer.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktManufacturerI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktManufacturer 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 manufacturer 实体翻译...", tenantCode);

        foreach (var item in GetManufacturerTranslations())
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

        TaktLogger.Information("TaktManufacturer 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktManufacturer 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.manufacturer._self / entity.manufacturer.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetManufacturerTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.manufacturer._self
            new TranslationSeedItem("entity.manufacturer._self", "en-US", "Manufacturer Information_us", "实体名称"),
            // entity.manufacturer._self
            new TranslationSeedItem("entity.manufacturer._self", "ja-JP", "Takt制造商信息_jp", "实体名称"),
            // entity.manufacturer._self
            new TranslationSeedItem("entity.manufacturer._self", "zh-CN", "Takt制造商信息", "实体名称"),
            // entity.manufacturer._self
            new TranslationSeedItem("entity.manufacturer._self", "zh-HK", "Takt制造商信息_hk", "实体名称"),

            // entity.manufacturer.code
            new TranslationSeedItem("entity.manufacturer.code", "en-US", "制造商编码_us", "制造商编码（唯一索引）"),
            // entity.manufacturer.code
            new TranslationSeedItem("entity.manufacturer.code", "ja-JP", "制造商编码_jp", "制造商编码（唯一索引）"),
            // entity.manufacturer.code
            new TranslationSeedItem("entity.manufacturer.code", "zh-CN", "制造商编码", "制造商编码（唯一索引）"),
            // entity.manufacturer.code
            new TranslationSeedItem("entity.manufacturer.code", "zh-HK", "制造商编码_hk", "制造商编码（唯一索引）"),

            // entity.manufacturer.name1
            new TranslationSeedItem("entity.manufacturer.name1", "en-US", "制造商名称1_us", "制造商名称1"),
            // entity.manufacturer.name1
            new TranslationSeedItem("entity.manufacturer.name1", "ja-JP", "制造商名称1_jp", "制造商名称1"),
            // entity.manufacturer.name1
            new TranslationSeedItem("entity.manufacturer.name1", "zh-CN", "制造商名称1", "制造商名称1"),
            // entity.manufacturer.name1
            new TranslationSeedItem("entity.manufacturer.name1", "zh-HK", "制造商名称1_hk", "制造商名称1"),

            // entity.manufacturer.name2
            new TranslationSeedItem("entity.manufacturer.name2", "en-US", "制造商名称2_us", "制造商名称2"),
            // entity.manufacturer.name2
            new TranslationSeedItem("entity.manufacturer.name2", "ja-JP", "制造商名称2_jp", "制造商名称2"),
            // entity.manufacturer.name2
            new TranslationSeedItem("entity.manufacturer.name2", "zh-CN", "制造商名称2", "制造商名称2"),
            // entity.manufacturer.name2
            new TranslationSeedItem("entity.manufacturer.name2", "zh-HK", "制造商名称2_hk", "制造商名称2"),

            // entity.manufacturer.shortname
            new TranslationSeedItem("entity.manufacturer.shortname", "en-US", "制造商简称_us", "制造商简称"),
            // entity.manufacturer.shortname
            new TranslationSeedItem("entity.manufacturer.shortname", "ja-JP", "制造商简称_jp", "制造商简称"),
            // entity.manufacturer.shortname
            new TranslationSeedItem("entity.manufacturer.shortname", "zh-CN", "制造商简称", "制造商简称"),
            // entity.manufacturer.shortname
            new TranslationSeedItem("entity.manufacturer.shortname", "zh-HK", "制造商简称_hk", "制造商简称"),

            // entity.manufacturer.type
            new TranslationSeedItem("entity.manufacturer.type", "en-US", "制造商类型_us", "制造商类型（字典 logistics_procurement_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）"),
            // entity.manufacturer.type
            new TranslationSeedItem("entity.manufacturer.type", "ja-JP", "制造商类型_jp", "制造商类型（字典 logistics_procurement_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）"),
            // entity.manufacturer.type
            new TranslationSeedItem("entity.manufacturer.type", "zh-CN", "制造商类型", "制造商类型（字典 logistics_procurement_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）"),
            // entity.manufacturer.type
            new TranslationSeedItem("entity.manufacturer.type", "zh-HK", "制造商类型_hk", "制造商类型（字典 logistics_procurement_manufacturer_type；0=OEM，1=ODM，2=CM，3=品牌制造商，4=其他）"),

            // entity.manufacturer.industrysector
            new TranslationSeedItem("entity.manufacturer.industrysector", "en-US", "行业领域_us", "行业领域（字典 logistics_materials_industry_sector；DictValue=A/C/M/P）"),
            // entity.manufacturer.industrysector
            new TranslationSeedItem("entity.manufacturer.industrysector", "ja-JP", "行业领域_jp", "行业领域（字典 logistics_materials_industry_sector；DictValue=A/C/M/P）"),
            // entity.manufacturer.industrysector
            new TranslationSeedItem("entity.manufacturer.industrysector", "zh-CN", "行业领域", "行业领域（字典 logistics_materials_industry_sector；DictValue=A/C/M/P）"),
            // entity.manufacturer.industrysector
            new TranslationSeedItem("entity.manufacturer.industrysector", "zh-HK", "行业领域_hk", "行业领域（字典 logistics_materials_industry_sector；DictValue=A/C/M/P）"),

            // entity.manufacturer.taxnumber
            new TranslationSeedItem("entity.manufacturer.taxnumber", "en-US", "制造商标识_us", "制造商标识（税务登记证号/统一社会信用代码）"),
            // entity.manufacturer.taxnumber
            new TranslationSeedItem("entity.manufacturer.taxnumber", "ja-JP", "制造商标识_jp", "制造商标识（税务登记证号/统一社会信用代码）"),
            // entity.manufacturer.taxnumber
            new TranslationSeedItem("entity.manufacturer.taxnumber", "zh-CN", "制造商标识", "制造商标识（税务登记证号/统一社会信用代码）"),
            // entity.manufacturer.taxnumber
            new TranslationSeedItem("entity.manufacturer.taxnumber", "zh-HK", "制造商标识_hk", "制造商标识（税务登记证号/统一社会信用代码）"),

            // entity.manufacturer.registrationcountry
            new TranslationSeedItem("entity.manufacturer.registrationcountry", "en-US", "注册国家_us", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.manufacturer.registrationcountry
            new TranslationSeedItem("entity.manufacturer.registrationcountry", "ja-JP", "注册国家_jp", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.manufacturer.registrationcountry
            new TranslationSeedItem("entity.manufacturer.registrationcountry", "zh-CN", "注册国家", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.manufacturer.registrationcountry
            new TranslationSeedItem("entity.manufacturer.registrationcountry", "zh-HK", "注册国家_hk", "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.manufacturer.registrationprovince
            new TranslationSeedItem("entity.manufacturer.registrationprovince", "en-US", "注册省_us", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.manufacturer.registrationprovince
            new TranslationSeedItem("entity.manufacturer.registrationprovince", "ja-JP", "注册省_jp", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.manufacturer.registrationprovince
            new TranslationSeedItem("entity.manufacturer.registrationprovince", "zh-CN", "注册省", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),
            // entity.manufacturer.registrationprovince
            new TranslationSeedItem("entity.manufacturer.registrationprovince", "zh-HK", "注册省_hk", "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）"),

            // entity.manufacturer.registrationcity
            new TranslationSeedItem("entity.manufacturer.registrationcity", "en-US", "注册市_us", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.manufacturer.registrationcity
            new TranslationSeedItem("entity.manufacturer.registrationcity", "ja-JP", "注册市_jp", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.manufacturer.registrationcity
            new TranslationSeedItem("entity.manufacturer.registrationcity", "zh-CN", "注册市", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),
            // entity.manufacturer.registrationcity
            new TranslationSeedItem("entity.manufacturer.registrationcity", "zh-HK", "注册市_hk", "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）"),

            // entity.manufacturer.registrationaddress1
            new TranslationSeedItem("entity.manufacturer.registrationaddress1", "en-US", "注册地址1_us", "注册地址1"),
            // entity.manufacturer.registrationaddress1
            new TranslationSeedItem("entity.manufacturer.registrationaddress1", "ja-JP", "注册地址1_jp", "注册地址1"),
            // entity.manufacturer.registrationaddress1
            new TranslationSeedItem("entity.manufacturer.registrationaddress1", "zh-CN", "注册地址1", "注册地址1"),
            // entity.manufacturer.registrationaddress1
            new TranslationSeedItem("entity.manufacturer.registrationaddress1", "zh-HK", "注册地址1_hk", "注册地址1"),

            // entity.manufacturer.registrationaddress2
            new TranslationSeedItem("entity.manufacturer.registrationaddress2", "en-US", "注册地址2_us", "注册地址2"),
            // entity.manufacturer.registrationaddress2
            new TranslationSeedItem("entity.manufacturer.registrationaddress2", "ja-JP", "注册地址2_jp", "注册地址2"),
            // entity.manufacturer.registrationaddress2
            new TranslationSeedItem("entity.manufacturer.registrationaddress2", "zh-CN", "注册地址2", "注册地址2"),
            // entity.manufacturer.registrationaddress2
            new TranslationSeedItem("entity.manufacturer.registrationaddress2", "zh-HK", "注册地址2_hk", "注册地址2"),

            // entity.manufacturer.phone
            new TranslationSeedItem("entity.manufacturer.phone", "en-US", "制造商电话_us", "制造商电话"),
            // entity.manufacturer.phone
            new TranslationSeedItem("entity.manufacturer.phone", "ja-JP", "制造商电话_jp", "制造商电话"),
            // entity.manufacturer.phone
            new TranslationSeedItem("entity.manufacturer.phone", "zh-CN", "制造商电话", "制造商电话"),
            // entity.manufacturer.phone
            new TranslationSeedItem("entity.manufacturer.phone", "zh-HK", "制造商电话_hk", "制造商电话"),

            // entity.manufacturer.fax
            new TranslationSeedItem("entity.manufacturer.fax", "en-US", "制造商传真_us", "制造商传真"),
            // entity.manufacturer.fax
            new TranslationSeedItem("entity.manufacturer.fax", "ja-JP", "制造商传真_jp", "制造商传真"),
            // entity.manufacturer.fax
            new TranslationSeedItem("entity.manufacturer.fax", "zh-CN", "制造商传真", "制造商传真"),
            // entity.manufacturer.fax
            new TranslationSeedItem("entity.manufacturer.fax", "zh-HK", "制造商传真_hk", "制造商传真"),

            // entity.manufacturer.email
            new TranslationSeedItem("entity.manufacturer.email", "en-US", "制造商邮箱_us", "制造商邮箱"),
            // entity.manufacturer.email
            new TranslationSeedItem("entity.manufacturer.email", "ja-JP", "制造商邮箱_jp", "制造商邮箱"),
            // entity.manufacturer.email
            new TranslationSeedItem("entity.manufacturer.email", "zh-CN", "制造商邮箱", "制造商邮箱"),
            // entity.manufacturer.email
            new TranslationSeedItem("entity.manufacturer.email", "zh-HK", "制造商邮箱_hk", "制造商邮箱"),

            // entity.manufacturer.website
            new TranslationSeedItem("entity.manufacturer.website", "en-US", "制造商网站_us", "制造商网站"),
            // entity.manufacturer.website
            new TranslationSeedItem("entity.manufacturer.website", "ja-JP", "制造商网站_jp", "制造商网站"),
            // entity.manufacturer.website
            new TranslationSeedItem("entity.manufacturer.website", "zh-CN", "制造商网站", "制造商网站"),
            // entity.manufacturer.website
            new TranslationSeedItem("entity.manufacturer.website", "zh-HK", "制造商网站_hk", "制造商网站"),

            // entity.manufacturer.contactperson
            new TranslationSeedItem("entity.manufacturer.contactperson", "en-US", "联系人_us", "联系人"),
            // entity.manufacturer.contactperson
            new TranslationSeedItem("entity.manufacturer.contactperson", "ja-JP", "联系人_jp", "联系人"),
            // entity.manufacturer.contactperson
            new TranslationSeedItem("entity.manufacturer.contactperson", "zh-CN", "联系人", "联系人"),
            // entity.manufacturer.contactperson
            new TranslationSeedItem("entity.manufacturer.contactperson", "zh-HK", "联系人_hk", "联系人"),

            // entity.manufacturer.contactphone
            new TranslationSeedItem("entity.manufacturer.contactphone", "en-US", "联系人电话_us", "联系人电话"),
            // entity.manufacturer.contactphone
            new TranslationSeedItem("entity.manufacturer.contactphone", "ja-JP", "联系人电话_jp", "联系人电话"),
            // entity.manufacturer.contactphone
            new TranslationSeedItem("entity.manufacturer.contactphone", "zh-CN", "联系人电话", "联系人电话"),
            // entity.manufacturer.contactphone
            new TranslationSeedItem("entity.manufacturer.contactphone", "zh-HK", "联系人电话_hk", "联系人电话"),

            // entity.manufacturer.contactemail
            new TranslationSeedItem("entity.manufacturer.contactemail", "en-US", "联系人邮箱_us", "联系人邮箱"),
            // entity.manufacturer.contactemail
            new TranslationSeedItem("entity.manufacturer.contactemail", "ja-JP", "联系人邮箱_jp", "联系人邮箱"),
            // entity.manufacturer.contactemail
            new TranslationSeedItem("entity.manufacturer.contactemail", "zh-CN", "联系人邮箱", "联系人邮箱"),
            // entity.manufacturer.contactemail
            new TranslationSeedItem("entity.manufacturer.contactemail", "zh-HK", "联系人邮箱_hk", "联系人邮箱"),

            // entity.manufacturer.level
            new TranslationSeedItem("entity.manufacturer.level", "en-US", "制造商等级_us", "制造商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),
            // entity.manufacturer.level
            new TranslationSeedItem("entity.manufacturer.level", "ja-JP", "制造商等级_jp", "制造商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),
            // entity.manufacturer.level
            new TranslationSeedItem("entity.manufacturer.level", "zh-CN", "制造商等级", "制造商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),
            // entity.manufacturer.level
            new TranslationSeedItem("entity.manufacturer.level", "zh-HK", "制造商等级_hk", "制造商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）"),

            // entity.manufacturer.qualitycertification
            new TranslationSeedItem("entity.manufacturer.qualitycertification", "en-US", "质量认证_us", "质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）"),
            // entity.manufacturer.qualitycertification
            new TranslationSeedItem("entity.manufacturer.qualitycertification", "ja-JP", "质量认证_jp", "质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）"),
            // entity.manufacturer.qualitycertification
            new TranslationSeedItem("entity.manufacturer.qualitycertification", "zh-CN", "质量认证", "质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）"),
            // entity.manufacturer.qualitycertification
            new TranslationSeedItem("entity.manufacturer.qualitycertification", "zh-HK", "质量认证_hk", "质量认证（字典 logistics_quality_certification；0=无，1=ISO 9001，2=ISO 14001，3=ISO 45001，4=ISO 22000，5=ISO 27001，6=ISO 20000，7=ISO 50001，8=ISO 13485，9=IATF 16949，10=ISO/IEC 17025，11=GB/T 50430）"),

            // entity.manufacturer.evaluationscore
            new TranslationSeedItem("entity.manufacturer.evaluationscore", "en-US", "评价分数_us", "评价分数（0-100分）"),
            // entity.manufacturer.evaluationscore
            new TranslationSeedItem("entity.manufacturer.evaluationscore", "ja-JP", "评价分数_jp", "评价分数（0-100分）"),
            // entity.manufacturer.evaluationscore
            new TranslationSeedItem("entity.manufacturer.evaluationscore", "zh-CN", "评价分数", "评价分数（0-100分）"),
            // entity.manufacturer.evaluationscore
            new TranslationSeedItem("entity.manufacturer.evaluationscore", "zh-HK", "评价分数_hk", "评价分数（0-100分）"),

            // entity.manufacturer.sortorder
            new TranslationSeedItem("entity.manufacturer.sortorder", "en-US", "排序号_us", "排序号（越小越靠前）"),
            // entity.manufacturer.sortorder
            new TranslationSeedItem("entity.manufacturer.sortorder", "ja-JP", "排序号_jp", "排序号（越小越靠前）"),
            // entity.manufacturer.sortorder
            new TranslationSeedItem("entity.manufacturer.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.manufacturer.sortorder
            new TranslationSeedItem("entity.manufacturer.sortorder", "zh-HK", "排序号_hk", "排序号（越小越靠前）"),

            // entity.manufacturer.status
            new TranslationSeedItem("entity.manufacturer.status", "en-US", "制造商状态_us", "制造商状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.manufacturer.status
            new TranslationSeedItem("entity.manufacturer.status", "ja-JP", "制造商状态_jp", "制造商状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.manufacturer.status
            new TranslationSeedItem("entity.manufacturer.status", "zh-CN", "制造商状态", "制造商状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.manufacturer.status
            new TranslationSeedItem("entity.manufacturer.status", "zh-HK", "制造商状态_hk", "制造商状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),

            // entity.manufacturer.materials
            new TranslationSeedItem("entity.manufacturer.materials", "en-US", "导航属性：制造商物料列表_us", "导航属性：制造商物料列表"),
            // entity.manufacturer.materials
            new TranslationSeedItem("entity.manufacturer.materials", "ja-JP", "导航属性：制造商物料列表_jp", "导航属性：制造商物料列表"),
            // entity.manufacturer.materials
            new TranslationSeedItem("entity.manufacturer.materials", "zh-CN", "导航属性：制造商物料列表", "导航属性：制造商物料列表"),
            // entity.manufacturer.materials
            new TranslationSeedItem("entity.manufacturer.materials", "zh-HK", "导航属性：制造商物料列表_hk", "导航属性：制造商物料列表"),
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
