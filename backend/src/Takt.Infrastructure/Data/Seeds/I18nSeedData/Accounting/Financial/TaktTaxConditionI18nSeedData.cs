// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktTaxConditionI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTaxCondition 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTaxCondition 实体国际化翻译种子（键前缀 entity.taxcondition.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTaxConditionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTaxCondition 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 taxcondition 实体翻译...", tenantCode);

        foreach (var item in GetTaxConditionTranslations())
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

        TaktLogger.Information("TaktTaxCondition 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTaxCondition 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.taxcondition._self / entity.taxcondition.{{field}}；ResourceGroup=Financial；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTaxConditionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.taxcondition._self
            new TranslationSeedItem("entity.taxcondition._self", "en-US", "Tax Condition Information_us", "实体名称"),
            // entity.taxcondition._self
            new TranslationSeedItem("entity.taxcondition._self", "ja-JP", "税务条件信息_jp", "实体名称"),
            // entity.taxcondition._self
            new TranslationSeedItem("entity.taxcondition._self", "zh-CN", "税务条件信息", "实体名称"),
            // entity.taxcondition._self
            new TranslationSeedItem("entity.taxcondition._self", "zh-HK", "税务条件信息_hk", "实体名称"),

            // entity.taxcondition.applicationcode
            new TranslationSeedItem("entity.taxcondition.applicationcode", "en-US", "应用程序_us", "应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）"),
            // entity.taxcondition.applicationcode
            new TranslationSeedItem("entity.taxcondition.applicationcode", "ja-JP", "应用程序_jp", "应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）"),
            // entity.taxcondition.applicationcode
            new TranslationSeedItem("entity.taxcondition.applicationcode", "zh-CN", "应用程序", "应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）"),
            // entity.taxcondition.applicationcode
            new TranslationSeedItem("entity.taxcondition.applicationcode", "zh-HK", "应用程序_hk", "应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）"),

            // entity.taxcondition.conditiontypecode
            new TranslationSeedItem("entity.taxcondition.conditiontypecode", "en-US", "条件类型_us", "条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）"),
            // entity.taxcondition.conditiontypecode
            new TranslationSeedItem("entity.taxcondition.conditiontypecode", "ja-JP", "条件类型_jp", "条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）"),
            // entity.taxcondition.conditiontypecode
            new TranslationSeedItem("entity.taxcondition.conditiontypecode", "zh-CN", "条件类型", "条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）"),
            // entity.taxcondition.conditiontypecode
            new TranslationSeedItem("entity.taxcondition.conditiontypecode", "zh-HK", "条件类型_hk", "条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）"),

            // entity.taxcondition.countrycode
            new TranslationSeedItem("entity.taxcondition.countrycode", "en-US", "国家_us", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.taxcondition.countrycode
            new TranslationSeedItem("entity.taxcondition.countrycode", "ja-JP", "国家_jp", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.taxcondition.countrycode
            new TranslationSeedItem("entity.taxcondition.countrycode", "zh-CN", "国家", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),
            // entity.taxcondition.countrycode
            new TranslationSeedItem("entity.taxcondition.countrycode", "zh-HK", "国家_hk", "国家（字典 sys_country_code；DictValue=ISO alpha-2）"),

            // entity.taxcondition.taxcode
            new TranslationSeedItem("entity.taxcondition.taxcode", "en-US", "税码_us", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.taxcondition.taxcode
            new TranslationSeedItem("entity.taxcondition.taxcode", "ja-JP", "税码_jp", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.taxcondition.taxcode
            new TranslationSeedItem("entity.taxcondition.taxcode", "zh-CN", "税码", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),
            // entity.taxcondition.taxcode
            new TranslationSeedItem("entity.taxcondition.taxcode", "zh-HK", "税码_hk", "税码（选项 TaktTaxCodes/options；DictValue=TaxCode）"),

            // entity.taxcondition.conditionrecordcode
            new TranslationSeedItem("entity.taxcondition.conditionrecordcode", "en-US", "条件记录号_us", "条件记录号（长度 10；前端表单选择编码规则后自动通过 TaktNumbering 条件记录号规则生成并展示，非手输；单据类型菜单：税务条件）"),
            // entity.taxcondition.conditionrecordcode
            new TranslationSeedItem("entity.taxcondition.conditionrecordcode", "ja-JP", "条件记录号_jp", "条件记录号（长度 10；前端表单选择编码规则后自动通过 TaktNumbering 条件记录号规则生成并展示，非手输；单据类型菜单：税务条件）"),
            // entity.taxcondition.conditionrecordcode
            new TranslationSeedItem("entity.taxcondition.conditionrecordcode", "zh-CN", "条件记录号", "条件记录号（长度 10；前端表单选择编码规则后自动通过 TaktNumbering 条件记录号规则生成并展示，非手输；单据类型菜单：税务条件）"),
            // entity.taxcondition.conditionrecordcode
            new TranslationSeedItem("entity.taxcondition.conditionrecordcode", "zh-HK", "条件记录号_hk", "条件记录号（长度 10；前端表单选择编码规则后自动通过 TaktNumbering 条件记录号规则生成并展示，非手输；单据类型菜单：税务条件）"),

            // entity.taxcondition.sortorder
            new TranslationSeedItem("entity.taxcondition.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.taxcondition.sortorder
            new TranslationSeedItem("entity.taxcondition.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.taxcondition.sortorder
            new TranslationSeedItem("entity.taxcondition.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.taxcondition.sortorder
            new TranslationSeedItem("entity.taxcondition.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.taxcondition.status
            new TranslationSeedItem("entity.taxcondition.status", "en-US", "税务条件状态_us", "税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.taxcondition.status
            new TranslationSeedItem("entity.taxcondition.status", "ja-JP", "税务条件状态_jp", "税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.taxcondition.status
            new TranslationSeedItem("entity.taxcondition.status", "zh-CN", "税务条件状态", "税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.taxcondition.status
            new TranslationSeedItem("entity.taxcondition.status", "zh-HK", "税务条件状态_hk", "税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
