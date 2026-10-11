// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktStandardWageRateI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktStandardWageRate 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktStandardWageRate 实体国际化翻译种子（键前缀 entity.standardwagerate.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktStandardWageRateI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktStandardWageRate 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 standardwagerate 实体翻译...", tenantCode);

        foreach (var item in GetStandardWageRateTranslations())
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

        TaktLogger.Information("TaktStandardWageRate 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktStandardWageRate 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.standardwagerate._self / entity.standardwagerate.{{field}}；ResourceGroup=Controlling；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetStandardWageRateTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.standardwagerate._self
            new TranslationSeedItem("entity.standardwagerate._self", "en-US", "Standard Wage Rate Information_us", "实体名称"),
            // entity.standardwagerate._self
            new TranslationSeedItem("entity.standardwagerate._self", "ja-JP", "标准工资率信息_jp", "实体名称"),
            // entity.standardwagerate._self
            new TranslationSeedItem("entity.standardwagerate._self", "zh-CN", "标准工资率信息", "实体名称"),
            // entity.standardwagerate._self
            new TranslationSeedItem("entity.standardwagerate._self", "zh-HK", "标准工资率信息_hk", "实体名称"),

            // entity.standardwagerate.yearmonth
            new TranslationSeedItem("entity.standardwagerate.yearmonth", "en-US", "年月_us", "年月（yyyyMM）"),
            // entity.standardwagerate.yearmonth
            new TranslationSeedItem("entity.standardwagerate.yearmonth", "ja-JP", "年月_jp", "年月（yyyyMM）"),
            // entity.standardwagerate.yearmonth
            new TranslationSeedItem("entity.standardwagerate.yearmonth", "zh-CN", "年月", "年月（yyyyMM）"),
            // entity.standardwagerate.yearmonth
            new TranslationSeedItem("entity.standardwagerate.yearmonth", "zh-HK", "年月_hk", "年月（yyyyMM）"),

            // entity.standardwagerate.workingdays
            new TranslationSeedItem("entity.standardwagerate.workingdays", "en-US", "工作天数_us", "工作天数"),
            // entity.standardwagerate.workingdays
            new TranslationSeedItem("entity.standardwagerate.workingdays", "ja-JP", "工作天数_jp", "工作天数"),
            // entity.standardwagerate.workingdays
            new TranslationSeedItem("entity.standardwagerate.workingdays", "zh-CN", "工作天数", "工作天数"),
            // entity.standardwagerate.workingdays
            new TranslationSeedItem("entity.standardwagerate.workingdays", "zh-HK", "工作天数_hk", "工作天数"),

            // entity.standardwagerate.salesamount
            new TranslationSeedItem("entity.standardwagerate.salesamount", "en-US", "销售额_us", "销售额"),
            // entity.standardwagerate.salesamount
            new TranslationSeedItem("entity.standardwagerate.salesamount", "ja-JP", "销售额_jp", "销售额"),
            // entity.standardwagerate.salesamount
            new TranslationSeedItem("entity.standardwagerate.salesamount", "zh-CN", "销售额", "销售额"),
            // entity.standardwagerate.salesamount
            new TranslationSeedItem("entity.standardwagerate.salesamount", "zh-HK", "销售额_hk", "销售额"),

            // entity.standardwagerate.directlaborcount
            new TranslationSeedItem("entity.standardwagerate.directlaborcount", "en-US", "直接人数_us", "直接人数"),
            // entity.standardwagerate.directlaborcount
            new TranslationSeedItem("entity.standardwagerate.directlaborcount", "ja-JP", "直接人数_jp", "直接人数"),
            // entity.standardwagerate.directlaborcount
            new TranslationSeedItem("entity.standardwagerate.directlaborcount", "zh-CN", "直接人数", "直接人数"),
            // entity.standardwagerate.directlaborcount
            new TranslationSeedItem("entity.standardwagerate.directlaborcount", "zh-HK", "直接人数_hk", "直接人数"),

            // entity.standardwagerate.directlaborwage
            new TranslationSeedItem("entity.standardwagerate.directlaborwage", "en-US", "直接工资_us", "直接工资"),
            // entity.standardwagerate.directlaborwage
            new TranslationSeedItem("entity.standardwagerate.directlaborwage", "ja-JP", "直接工资_jp", "直接工资"),
            // entity.standardwagerate.directlaborwage
            new TranslationSeedItem("entity.standardwagerate.directlaborwage", "zh-CN", "直接工资", "直接工资"),
            // entity.standardwagerate.directlaborwage
            new TranslationSeedItem("entity.standardwagerate.directlaborwage", "zh-HK", "直接工资_hk", "直接工资"),

            // entity.standardwagerate.directovertimehours
            new TranslationSeedItem("entity.standardwagerate.directovertimehours", "en-US", "直接加班小时_us", "直接加班小时"),
            // entity.standardwagerate.directovertimehours
            new TranslationSeedItem("entity.standardwagerate.directovertimehours", "ja-JP", "直接加班小时_jp", "直接加班小时"),
            // entity.standardwagerate.directovertimehours
            new TranslationSeedItem("entity.standardwagerate.directovertimehours", "zh-CN", "直接加班小时", "直接加班小时"),
            // entity.standardwagerate.directovertimehours
            new TranslationSeedItem("entity.standardwagerate.directovertimehours", "zh-HK", "直接加班小时_hk", "直接加班小时"),

            // entity.standardwagerate.directovertimetotal
            new TranslationSeedItem("entity.standardwagerate.directovertimetotal", "en-US", "直接加班总额_us", "直接加班总额"),
            // entity.standardwagerate.directovertimetotal
            new TranslationSeedItem("entity.standardwagerate.directovertimetotal", "ja-JP", "直接加班总额_jp", "直接加班总额"),
            // entity.standardwagerate.directovertimetotal
            new TranslationSeedItem("entity.standardwagerate.directovertimetotal", "zh-CN", "直接加班总额", "直接加班总额"),
            // entity.standardwagerate.directovertimetotal
            new TranslationSeedItem("entity.standardwagerate.directovertimetotal", "zh-HK", "直接加班总额_hk", "直接加班总额"),

            // entity.standardwagerate.directwagerate
            new TranslationSeedItem("entity.standardwagerate.directwagerate", "en-US", "直接工资率_us", "直接工资率"),
            // entity.standardwagerate.directwagerate
            new TranslationSeedItem("entity.standardwagerate.directwagerate", "ja-JP", "直接工资率_jp", "直接工资率"),
            // entity.standardwagerate.directwagerate
            new TranslationSeedItem("entity.standardwagerate.directwagerate", "zh-CN", "直接工资率", "直接工资率"),
            // entity.standardwagerate.directwagerate
            new TranslationSeedItem("entity.standardwagerate.directwagerate", "zh-HK", "直接工资率_hk", "直接工资率"),

            // entity.standardwagerate.indirectlaborcount
            new TranslationSeedItem("entity.standardwagerate.indirectlaborcount", "en-US", "间接人数_us", "间接人数"),
            // entity.standardwagerate.indirectlaborcount
            new TranslationSeedItem("entity.standardwagerate.indirectlaborcount", "ja-JP", "间接人数_jp", "间接人数"),
            // entity.standardwagerate.indirectlaborcount
            new TranslationSeedItem("entity.standardwagerate.indirectlaborcount", "zh-CN", "间接人数", "间接人数"),
            // entity.standardwagerate.indirectlaborcount
            new TranslationSeedItem("entity.standardwagerate.indirectlaborcount", "zh-HK", "间接人数_hk", "间接人数"),

            // entity.standardwagerate.indirectlaborwage
            new TranslationSeedItem("entity.standardwagerate.indirectlaborwage", "en-US", "间接工资_us", "间接工资"),
            // entity.standardwagerate.indirectlaborwage
            new TranslationSeedItem("entity.standardwagerate.indirectlaborwage", "ja-JP", "间接工资_jp", "间接工资"),
            // entity.standardwagerate.indirectlaborwage
            new TranslationSeedItem("entity.standardwagerate.indirectlaborwage", "zh-CN", "间接工资", "间接工资"),
            // entity.standardwagerate.indirectlaborwage
            new TranslationSeedItem("entity.standardwagerate.indirectlaborwage", "zh-HK", "间接工资_hk", "间接工资"),

            // entity.standardwagerate.indirectovertimehours
            new TranslationSeedItem("entity.standardwagerate.indirectovertimehours", "en-US", "间接加班小时_us", "间接加班小时"),
            // entity.standardwagerate.indirectovertimehours
            new TranslationSeedItem("entity.standardwagerate.indirectovertimehours", "ja-JP", "间接加班小时_jp", "间接加班小时"),
            // entity.standardwagerate.indirectovertimehours
            new TranslationSeedItem("entity.standardwagerate.indirectovertimehours", "zh-CN", "间接加班小时", "间接加班小时"),
            // entity.standardwagerate.indirectovertimehours
            new TranslationSeedItem("entity.standardwagerate.indirectovertimehours", "zh-HK", "间接加班小时_hk", "间接加班小时"),

            // entity.standardwagerate.indirectovertimetotal
            new TranslationSeedItem("entity.standardwagerate.indirectovertimetotal", "en-US", "间接加班总额_us", "间接加班总额"),
            // entity.standardwagerate.indirectovertimetotal
            new TranslationSeedItem("entity.standardwagerate.indirectovertimetotal", "ja-JP", "间接加班总额_jp", "间接加班总额"),
            // entity.standardwagerate.indirectovertimetotal
            new TranslationSeedItem("entity.standardwagerate.indirectovertimetotal", "zh-CN", "间接加班总额", "间接加班总额"),
            // entity.standardwagerate.indirectovertimetotal
            new TranslationSeedItem("entity.standardwagerate.indirectovertimetotal", "zh-HK", "间接加班总额_hk", "间接加班总额"),

            // entity.standardwagerate.indirectwagerate
            new TranslationSeedItem("entity.standardwagerate.indirectwagerate", "en-US", "间接工资率_us", "间接工资率"),
            // entity.standardwagerate.indirectwagerate
            new TranslationSeedItem("entity.standardwagerate.indirectwagerate", "ja-JP", "间接工资率_jp", "间接工资率"),
            // entity.standardwagerate.indirectwagerate
            new TranslationSeedItem("entity.standardwagerate.indirectwagerate", "zh-CN", "间接工资率", "间接工资率"),
            // entity.standardwagerate.indirectwagerate
            new TranslationSeedItem("entity.standardwagerate.indirectwagerate", "zh-HK", "间接工资率_hk", "间接工资率"),
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
        translation.ResourceGroup = "Controlling";
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
