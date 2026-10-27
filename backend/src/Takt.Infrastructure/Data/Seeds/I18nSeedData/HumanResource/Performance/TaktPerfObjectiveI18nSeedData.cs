// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktPerfObjectiveI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPerfObjective 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance;

/// <summary>
/// TaktPerfObjective 实体国际化翻译种子（键前缀 entity.perfobjective.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPerfObjectiveI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPerfObjective 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 perfobjective 实体翻译...", tenantCode);

        foreach (var item in GetPerfObjectiveTranslations())
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

        TaktLogger.Information("TaktPerfObjective 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPerfObjective 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.perfobjective._self / entity.perfobjective.{{field}}；ResourceGroup=Performance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPerfObjectiveTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.perfobjective._self
            new TranslationSeedItem("entity.perfobjective._self", "en-US", "Perf Objective Information_us", "实体名称"),
            // entity.perfobjective._self
            new TranslationSeedItem("entity.perfobjective._self", "ja-JP", "员工绩效目标信息_jp", "实体名称"),
            // entity.perfobjective._self
            new TranslationSeedItem("entity.perfobjective._self", "zh-CN", "员工绩效目标信息", "实体名称"),
            // entity.perfobjective._self
            new TranslationSeedItem("entity.perfobjective._self", "zh-HK", "员工绩效目标信息_hk", "实体名称"),

            // entity.perfobjective.employeeid
            new TranslationSeedItem("entity.perfobjective.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfobjective.employeeid
            new TranslationSeedItem("entity.perfobjective.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfobjective.employeeid
            new TranslationSeedItem("entity.perfobjective.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfobjective.employeeid
            new TranslationSeedItem("entity.perfobjective.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.perfobjective.employeename
            new TranslationSeedItem("entity.perfobjective.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.perfobjective.employeename
            new TranslationSeedItem("entity.perfobjective.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.perfobjective.employeename
            new TranslationSeedItem("entity.perfobjective.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.perfobjective.employeename
            new TranslationSeedItem("entity.perfobjective.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.perfobjective.schememetricid
            new TranslationSeedItem("entity.perfobjective.schememetricid", "en-US", "方案指标ID_us", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),
            // entity.perfobjective.schememetricid
            new TranslationSeedItem("entity.perfobjective.schememetricid", "ja-JP", "方案指标ID_jp", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),
            // entity.perfobjective.schememetricid
            new TranslationSeedItem("entity.perfobjective.schememetricid", "zh-CN", "方案指标ID", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),
            // entity.perfobjective.schememetricid
            new TranslationSeedItem("entity.perfobjective.schememetricid", "zh-HK", "方案指标ID_hk", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),

            // entity.perfobjective.objectiveperiod
            new TranslationSeedItem("entity.perfobjective.objectiveperiod", "en-US", "目标周期_us", "目标周期（如 2026-Q1、2026-Annual）"),
            // entity.perfobjective.objectiveperiod
            new TranslationSeedItem("entity.perfobjective.objectiveperiod", "ja-JP", "目标周期_jp", "目标周期（如 2026-Q1、2026-Annual）"),
            // entity.perfobjective.objectiveperiod
            new TranslationSeedItem("entity.perfobjective.objectiveperiod", "zh-CN", "目标周期", "目标周期（如 2026-Q1、2026-Annual）"),
            // entity.perfobjective.objectiveperiod
            new TranslationSeedItem("entity.perfobjective.objectiveperiod", "zh-HK", "目标周期_hk", "目标周期（如 2026-Q1、2026-Annual）"),

            // entity.perfobjective.objectivedescription
            new TranslationSeedItem("entity.perfobjective.objectivedescription", "en-US", "目标描述_us", "目标描述"),
            // entity.perfobjective.objectivedescription
            new TranslationSeedItem("entity.perfobjective.objectivedescription", "ja-JP", "目标描述_jp", "目标描述"),
            // entity.perfobjective.objectivedescription
            new TranslationSeedItem("entity.perfobjective.objectivedescription", "zh-CN", "目标描述", "目标描述"),
            // entity.perfobjective.objectivedescription
            new TranslationSeedItem("entity.perfobjective.objectivedescription", "zh-HK", "目标描述_hk", "目标描述"),

            // entity.perfobjective.targetvalue
            new TranslationSeedItem("entity.perfobjective.targetvalue", "en-US", "目标值_us", "目标值"),
            // entity.perfobjective.targetvalue
            new TranslationSeedItem("entity.perfobjective.targetvalue", "ja-JP", "目标值_jp", "目标值"),
            // entity.perfobjective.targetvalue
            new TranslationSeedItem("entity.perfobjective.targetvalue", "zh-CN", "目标值", "目标值"),
            // entity.perfobjective.targetvalue
            new TranslationSeedItem("entity.perfobjective.targetvalue", "zh-HK", "目标值_hk", "目标值"),

            // entity.perfobjective.actualvalue
            new TranslationSeedItem("entity.perfobjective.actualvalue", "en-US", "实际完成值_us", "实际完成值"),
            // entity.perfobjective.actualvalue
            new TranslationSeedItem("entity.perfobjective.actualvalue", "ja-JP", "实际完成值_jp", "实际完成值"),
            // entity.perfobjective.actualvalue
            new TranslationSeedItem("entity.perfobjective.actualvalue", "zh-CN", "实际完成值", "实际完成值"),
            // entity.perfobjective.actualvalue
            new TranslationSeedItem("entity.perfobjective.actualvalue", "zh-HK", "实际完成值_hk", "实际完成值"),

            // entity.perfobjective.completionpercentage
            new TranslationSeedItem("entity.perfobjective.completionpercentage", "en-US", "完成百分比_us", "完成百分比（%）"),
            // entity.perfobjective.completionpercentage
            new TranslationSeedItem("entity.perfobjective.completionpercentage", "ja-JP", "完成百分比_jp", "完成百分比（%）"),
            // entity.perfobjective.completionpercentage
            new TranslationSeedItem("entity.perfobjective.completionpercentage", "zh-CN", "完成百分比", "完成百分比（%）"),
            // entity.perfobjective.completionpercentage
            new TranslationSeedItem("entity.perfobjective.completionpercentage", "zh-HK", "完成百分比_hk", "完成百分比（%）"),

            // entity.perfobjective.objectiveweight
            new TranslationSeedItem("entity.perfobjective.objectiveweight", "en-US", "目标权重_us", "目标权重（%）"),
            // entity.perfobjective.objectiveweight
            new TranslationSeedItem("entity.perfobjective.objectiveweight", "ja-JP", "目标权重_jp", "目标权重（%）"),
            // entity.perfobjective.objectiveweight
            new TranslationSeedItem("entity.perfobjective.objectiveweight", "zh-CN", "目标权重", "目标权重（%）"),
            // entity.perfobjective.objectiveweight
            new TranslationSeedItem("entity.perfobjective.objectiveweight", "zh-HK", "目标权重_hk", "目标权重（%）"),

            // entity.perfobjective.startdate
            new TranslationSeedItem("entity.perfobjective.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.perfobjective.startdate
            new TranslationSeedItem("entity.perfobjective.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.perfobjective.startdate
            new TranslationSeedItem("entity.perfobjective.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.perfobjective.startdate
            new TranslationSeedItem("entity.perfobjective.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.perfobjective.duedate
            new TranslationSeedItem("entity.perfobjective.duedate", "en-US", "截止日期_us", "截止日期"),
            // entity.perfobjective.duedate
            new TranslationSeedItem("entity.perfobjective.duedate", "ja-JP", "截止日期_jp", "截止日期"),
            // entity.perfobjective.duedate
            new TranslationSeedItem("entity.perfobjective.duedate", "zh-CN", "截止日期", "截止日期"),
            // entity.perfobjective.duedate
            new TranslationSeedItem("entity.perfobjective.duedate", "zh-HK", "截止日期_hk", "截止日期"),

            // entity.perfobjective.achievementnotes
            new TranslationSeedItem("entity.perfobjective.achievementnotes", "en-US", "目标达成说明_us", "目标达成说明"),
            // entity.perfobjective.achievementnotes
            new TranslationSeedItem("entity.perfobjective.achievementnotes", "ja-JP", "目标达成说明_jp", "目标达成说明"),
            // entity.perfobjective.achievementnotes
            new TranslationSeedItem("entity.perfobjective.achievementnotes", "zh-CN", "目标达成说明", "目标达成说明"),
            // entity.perfobjective.achievementnotes
            new TranslationSeedItem("entity.perfobjective.achievementnotes", "zh-HK", "目标达成说明_hk", "目标达成说明"),

            // entity.perfobjective.objectivestatus
            new TranslationSeedItem("entity.perfobjective.objectivestatus", "en-US", "业务状态_us", "业务状态（字典 humanresource_performance_objective_status；0=待确认 1=进行中 2=已完成）"),
            // entity.perfobjective.objectivestatus
            new TranslationSeedItem("entity.perfobjective.objectivestatus", "ja-JP", "业务状态_jp", "业务状态（字典 humanresource_performance_objective_status；0=待确认 1=进行中 2=已完成）"),
            // entity.perfobjective.objectivestatus
            new TranslationSeedItem("entity.perfobjective.objectivestatus", "zh-CN", "业务状态", "业务状态（字典 humanresource_performance_objective_status；0=待确认 1=进行中 2=已完成）"),
            // entity.perfobjective.objectivestatus
            new TranslationSeedItem("entity.perfobjective.objectivestatus", "zh-HK", "业务状态_hk", "业务状态（字典 humanresource_performance_objective_status；0=待确认 1=进行中 2=已完成）"),
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
        translation.ResourceGroup = "Performance";
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
