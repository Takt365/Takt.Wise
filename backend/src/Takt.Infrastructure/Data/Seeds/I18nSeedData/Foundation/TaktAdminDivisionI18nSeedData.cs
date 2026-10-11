// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktAdminDivisionI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAdminDivision 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktAdminDivision 实体国际化翻译种子（键前缀 entity.admindivision.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAdminDivisionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAdminDivision 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 admindivision 实体翻译...", tenantCode);

        foreach (var item in GetAdminDivisionTranslations())
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

        TaktLogger.Information("TaktAdminDivision 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAdminDivision 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.admindivision._self / entity.admindivision.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAdminDivisionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.admindivision._self
            new TranslationSeedItem("entity.admindivision._self", "en-US", "Admin Division Information_us", "实体名称"),
            // entity.admindivision._self
            new TranslationSeedItem("entity.admindivision._self", "ja-JP", "行政区划信息_jp", "实体名称"),
            // entity.admindivision._self
            new TranslationSeedItem("entity.admindivision._self", "zh-CN", "行政区划信息", "实体名称"),
            // entity.admindivision._self
            new TranslationSeedItem("entity.admindivision._self", "zh-HK", "行政区划信息_hk", "实体名称"),

            // entity.admindivision.countrycode
            new TranslationSeedItem("entity.admindivision.countrycode", "en-US", "国家代码_us", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.admindivision.countrycode
            new TranslationSeedItem("entity.admindivision.countrycode", "ja-JP", "国家代码_jp", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.admindivision.countrycode
            new TranslationSeedItem("entity.admindivision.countrycode", "zh-CN", "国家代码", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.admindivision.countrycode
            new TranslationSeedItem("entity.admindivision.countrycode", "zh-HK", "国家代码_hk", "国家代码（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.admindivision.divisioncode
            new TranslationSeedItem("entity.admindivision.divisioncode", "en-US", "区划编码_us", "区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）"),
            // entity.admindivision.divisioncode
            new TranslationSeedItem("entity.admindivision.divisioncode", "ja-JP", "区划编码_jp", "区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）"),
            // entity.admindivision.divisioncode
            new TranslationSeedItem("entity.admindivision.divisioncode", "zh-CN", "区划编码", "区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）"),
            // entity.admindivision.divisioncode
            new TranslationSeedItem("entity.admindivision.divisioncode", "zh-HK", "区划编码_hk", "区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）"),

            // entity.admindivision.divisionname
            new TranslationSeedItem("entity.admindivision.divisionname", "en-US", "区划名称_us", "区划名称（本国语言官方/本地显示名）"),
            // entity.admindivision.divisionname
            new TranslationSeedItem("entity.admindivision.divisionname", "ja-JP", "区划名称_jp", "区划名称（本国语言官方/本地显示名）"),
            // entity.admindivision.divisionname
            new TranslationSeedItem("entity.admindivision.divisionname", "zh-CN", "区划名称", "区划名称（本国语言官方/本地显示名）"),
            // entity.admindivision.divisionname
            new TranslationSeedItem("entity.admindivision.divisionname", "zh-HK", "区划名称_hk", "区划名称（本国语言官方/本地显示名）"),

            // entity.admindivision.parentid
            new TranslationSeedItem("entity.admindivision.parentid", "en-US", "父级区划ID_us", "父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）"),
            // entity.admindivision.parentid
            new TranslationSeedItem("entity.admindivision.parentid", "ja-JP", "父级区划ID_jp", "父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）"),
            // entity.admindivision.parentid
            new TranslationSeedItem("entity.admindivision.parentid", "zh-CN", "父级区划ID", "父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）"),
            // entity.admindivision.parentid
            new TranslationSeedItem("entity.admindivision.parentid", "zh-HK", "父级区划ID_hk", "父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）"),

            // entity.admindivision.level
            new TranslationSeedItem("entity.admindivision.level", "en-US", "层级_us", "层级（字典 sys_admin_division_level；1～6）"),
            // entity.admindivision.level
            new TranslationSeedItem("entity.admindivision.level", "ja-JP", "层级_jp", "层级（字典 sys_admin_division_level；1～6）"),
            // entity.admindivision.level
            new TranslationSeedItem("entity.admindivision.level", "zh-CN", "层级", "层级（字典 sys_admin_division_level；1～6）"),
            // entity.admindivision.level
            new TranslationSeedItem("entity.admindivision.level", "zh-HK", "层级_hk", "层级（字典 sys_admin_division_level；1～6）"),

            // entity.admindivision.divisionpath
            new TranslationSeedItem("entity.admindivision.divisionpath", "en-US", "区划路径_us", "区划路径（如 /1/3/5/，用于快速查询子孙）"),
            // entity.admindivision.divisionpath
            new TranslationSeedItem("entity.admindivision.divisionpath", "ja-JP", "区划路径_jp", "区划路径（如 /1/3/5/，用于快速查询子孙）"),
            // entity.admindivision.divisionpath
            new TranslationSeedItem("entity.admindivision.divisionpath", "zh-CN", "区划路径", "区划路径（如 /1/3/5/，用于快速查询子孙）"),
            // entity.admindivision.divisionpath
            new TranslationSeedItem("entity.admindivision.divisionpath", "zh-HK", "区划路径_hk", "区划路径（如 /1/3/5/，用于快速查询子孙）"),

            // entity.admindivision.isleaf
            new TranslationSeedItem("entity.admindivision.isleaf", "en-US", "是否叶子节点_us", "是否叶子节点（字典 sys_yes_no）"),
            // entity.admindivision.isleaf
            new TranslationSeedItem("entity.admindivision.isleaf", "ja-JP", "是否叶子节点_jp", "是否叶子节点（字典 sys_yes_no）"),
            // entity.admindivision.isleaf
            new TranslationSeedItem("entity.admindivision.isleaf", "zh-CN", "是否叶子节点", "是否叶子节点（字典 sys_yes_no）"),
            // entity.admindivision.isleaf
            new TranslationSeedItem("entity.admindivision.isleaf", "zh-HK", "是否叶子节点_hk", "是否叶子节点（字典 sys_yes_no）"),

            // entity.admindivision.postalcode
            new TranslationSeedItem("entity.admindivision.postalcode", "en-US", "邮政编码_us", "邮政编码（可选；部分国家区划关联邮编）"),
            // entity.admindivision.postalcode
            new TranslationSeedItem("entity.admindivision.postalcode", "ja-JP", "邮政编码_jp", "邮政编码（可选；部分国家区划关联邮编）"),
            // entity.admindivision.postalcode
            new TranslationSeedItem("entity.admindivision.postalcode", "zh-CN", "邮政编码", "邮政编码（可选；部分国家区划关联邮编）"),
            // entity.admindivision.postalcode
            new TranslationSeedItem("entity.admindivision.postalcode", "zh-HK", "邮政编码_hk", "邮政编码（可选；部分国家区划关联邮编）"),

            // entity.admindivision.currencycode
            new TranslationSeedItem("entity.admindivision.currencycode", "en-US", "币种_us", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),
            // entity.admindivision.currencycode
            new TranslationSeedItem("entity.admindivision.currencycode", "ja-JP", "币种_jp", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),
            // entity.admindivision.currencycode
            new TranslationSeedItem("entity.admindivision.currencycode", "zh-CN", "币种", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),
            // entity.admindivision.currencycode
            new TranslationSeedItem("entity.admindivision.currencycode", "zh-HK", "币种_hk", "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）"),

            // entity.admindivision.phonecode
            new TranslationSeedItem("entity.admindivision.phonecode", "en-US", "电话区号_us", "电话区号（国际电话区号，如 +86、+81）"),
            // entity.admindivision.phonecode
            new TranslationSeedItem("entity.admindivision.phonecode", "ja-JP", "电话区号_jp", "电话区号（国际电话区号，如 +86、+81）"),
            // entity.admindivision.phonecode
            new TranslationSeedItem("entity.admindivision.phonecode", "zh-CN", "电话区号", "电话区号（国际电话区号，如 +86、+81）"),
            // entity.admindivision.phonecode
            new TranslationSeedItem("entity.admindivision.phonecode", "zh-HK", "电话区号_hk", "电话区号（国际电话区号，如 +86、+81）"),

            // entity.admindivision.isbuiltin
            new TranslationSeedItem("entity.admindivision.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；内置项禁止删除）"),
            // entity.admindivision.isbuiltin
            new TranslationSeedItem("entity.admindivision.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；内置项禁止删除）"),
            // entity.admindivision.isbuiltin
            new TranslationSeedItem("entity.admindivision.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；内置项禁止删除）"),
            // entity.admindivision.isbuiltin
            new TranslationSeedItem("entity.admindivision.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；内置项禁止删除）"),

            // entity.admindivision.sortorder
            new TranslationSeedItem("entity.admindivision.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.admindivision.sortorder
            new TranslationSeedItem("entity.admindivision.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.admindivision.sortorder
            new TranslationSeedItem("entity.admindivision.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.admindivision.sortorder
            new TranslationSeedItem("entity.admindivision.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.admindivision.divisionstatus
            new TranslationSeedItem("entity.admindivision.divisionstatus", "en-US", "区划状态_us", "区划状态（字典 sys_normal_disable）"),
            // entity.admindivision.divisionstatus
            new TranslationSeedItem("entity.admindivision.divisionstatus", "ja-JP", "区划状态_jp", "区划状态（字典 sys_normal_disable）"),
            // entity.admindivision.divisionstatus
            new TranslationSeedItem("entity.admindivision.divisionstatus", "zh-CN", "区划状态", "区划状态（字典 sys_normal_disable）"),
            // entity.admindivision.divisionstatus
            new TranslationSeedItem("entity.admindivision.divisionstatus", "zh-HK", "区划状态_hk", "区划状态（字典 sys_normal_disable）"),
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
        translation.ResourceGroup = "Foundation";
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
