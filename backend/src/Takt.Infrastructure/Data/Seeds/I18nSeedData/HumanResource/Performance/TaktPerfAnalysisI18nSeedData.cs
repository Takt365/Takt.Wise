// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktPerfAnalysisI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPerfAnalysis 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPerfAnalysis 实体国际化翻译种子（键前缀 entity.perfanalysis.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPerfAnalysisI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPerfAnalysis 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 perfanalysis 实体翻译...", tenantCode);

        foreach (var item in GetPerfAnalysisTranslations())
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

        TaktLogger.Information("TaktPerfAnalysis 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPerfAnalysis 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.perfanalysis._self / entity.perfanalysis.{{field}}；ResourceGroup=Performance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPerfAnalysisTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.perfanalysis._self
            new TranslationSeedItem("entity.perfanalysis._self", "en-US", "Perf Analysis Information_us", "实体名称"),
            // entity.perfanalysis._self
            new TranslationSeedItem("entity.perfanalysis._self", "ja-JP", "分析改进信息_jp", "实体名称"),
            // entity.perfanalysis._self
            new TranslationSeedItem("entity.perfanalysis._self", "zh-CN", "分析改进信息", "实体名称"),
            // entity.perfanalysis._self
            new TranslationSeedItem("entity.perfanalysis._self", "zh-HK", "分析改进信息_hk", "实体名称"),

            // entity.perfanalysis.employeeid
            new TranslationSeedItem("entity.perfanalysis.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfanalysis.employeeid
            new TranslationSeedItem("entity.perfanalysis.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfanalysis.employeeid
            new TranslationSeedItem("entity.perfanalysis.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfanalysis.employeeid
            new TranslationSeedItem("entity.perfanalysis.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.perfanalysis.employeename
            new TranslationSeedItem("entity.perfanalysis.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.perfanalysis.employeename
            new TranslationSeedItem("entity.perfanalysis.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.perfanalysis.employeename
            new TranslationSeedItem("entity.perfanalysis.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.perfanalysis.employeename
            new TranslationSeedItem("entity.perfanalysis.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.perfanalysis.assessmentid
            new TranslationSeedItem("entity.perfanalysis.assessmentid", "en-US", "考核评估ID_us", "考核评估（选项 TaktPerfAssessments/options；DictValue=Id）"),
            // entity.perfanalysis.assessmentid
            new TranslationSeedItem("entity.perfanalysis.assessmentid", "ja-JP", "考核评估ID_jp", "考核评估（选项 TaktPerfAssessments/options；DictValue=Id）"),
            // entity.perfanalysis.assessmentid
            new TranslationSeedItem("entity.perfanalysis.assessmentid", "zh-CN", "考核评估ID", "考核评估（选项 TaktPerfAssessments/options；DictValue=Id）"),
            // entity.perfanalysis.assessmentid
            new TranslationSeedItem("entity.perfanalysis.assessmentid", "zh-HK", "考核评估ID_hk", "考核评估（选项 TaktPerfAssessments/options；DictValue=Id）"),

            // entity.perfanalysis.plantitle
            new TranslationSeedItem("entity.perfanalysis.plantitle", "en-US", "改进计划标题_us", "改进计划标题"),
            // entity.perfanalysis.plantitle
            new TranslationSeedItem("entity.perfanalysis.plantitle", "ja-JP", "改进计划标题_jp", "改进计划标题"),
            // entity.perfanalysis.plantitle
            new TranslationSeedItem("entity.perfanalysis.plantitle", "zh-CN", "改进计划标题", "改进计划标题"),
            // entity.perfanalysis.plantitle
            new TranslationSeedItem("entity.perfanalysis.plantitle", "zh-HK", "改进计划标题_hk", "改进计划标题"),

            // entity.perfanalysis.improvementarea
            new TranslationSeedItem("entity.perfanalysis.improvementarea", "en-US", "改进领域_us", "改进领域"),
            // entity.perfanalysis.improvementarea
            new TranslationSeedItem("entity.perfanalysis.improvementarea", "ja-JP", "改进领域_jp", "改进领域"),
            // entity.perfanalysis.improvementarea
            new TranslationSeedItem("entity.perfanalysis.improvementarea", "zh-CN", "改进领域", "改进领域"),
            // entity.perfanalysis.improvementarea
            new TranslationSeedItem("entity.perfanalysis.improvementarea", "zh-HK", "改进领域_hk", "改进领域"),

            // entity.perfanalysis.currentsituation
            new TranslationSeedItem("entity.perfanalysis.currentsituation", "en-US", "当前状况描述_us", "当前状况描述"),
            // entity.perfanalysis.currentsituation
            new TranslationSeedItem("entity.perfanalysis.currentsituation", "ja-JP", "当前状况描述_jp", "当前状况描述"),
            // entity.perfanalysis.currentsituation
            new TranslationSeedItem("entity.perfanalysis.currentsituation", "zh-CN", "当前状况描述", "当前状况描述"),
            // entity.perfanalysis.currentsituation
            new TranslationSeedItem("entity.perfanalysis.currentsituation", "zh-HK", "当前状况描述_hk", "当前状况描述"),

            // entity.perfanalysis.improvementgoal
            new TranslationSeedItem("entity.perfanalysis.improvementgoal", "en-US", "改进目标_us", "改进目标"),
            // entity.perfanalysis.improvementgoal
            new TranslationSeedItem("entity.perfanalysis.improvementgoal", "ja-JP", "改进目标_jp", "改进目标"),
            // entity.perfanalysis.improvementgoal
            new TranslationSeedItem("entity.perfanalysis.improvementgoal", "zh-CN", "改进目标", "改进目标"),
            // entity.perfanalysis.improvementgoal
            new TranslationSeedItem("entity.perfanalysis.improvementgoal", "zh-HK", "改进目标_hk", "改进目标"),

            // entity.perfanalysis.improvementactions
            new TranslationSeedItem("entity.perfanalysis.improvementactions", "en-US", "改进措施_us", "改进措施"),
            // entity.perfanalysis.improvementactions
            new TranslationSeedItem("entity.perfanalysis.improvementactions", "ja-JP", "改进措施_jp", "改进措施"),
            // entity.perfanalysis.improvementactions
            new TranslationSeedItem("entity.perfanalysis.improvementactions", "zh-CN", "改进措施", "改进措施"),
            // entity.perfanalysis.improvementactions
            new TranslationSeedItem("entity.perfanalysis.improvementactions", "zh-HK", "改进措施_hk", "改进措施"),

            // entity.perfanalysis.plandate
            new TranslationSeedItem("entity.perfanalysis.plandate", "en-US", "计划制定日期_us", "计划制定日期"),
            // entity.perfanalysis.plandate
            new TranslationSeedItem("entity.perfanalysis.plandate", "ja-JP", "计划制定日期_jp", "计划制定日期"),
            // entity.perfanalysis.plandate
            new TranslationSeedItem("entity.perfanalysis.plandate", "zh-CN", "计划制定日期", "计划制定日期"),
            // entity.perfanalysis.plandate
            new TranslationSeedItem("entity.perfanalysis.plandate", "zh-HK", "计划制定日期_hk", "计划制定日期"),

            // entity.perfanalysis.targetcompletiondate
            new TranslationSeedItem("entity.perfanalysis.targetcompletiondate", "en-US", "目标完成日期_us", "目标完成日期"),
            // entity.perfanalysis.targetcompletiondate
            new TranslationSeedItem("entity.perfanalysis.targetcompletiondate", "ja-JP", "目标完成日期_jp", "目标完成日期"),
            // entity.perfanalysis.targetcompletiondate
            new TranslationSeedItem("entity.perfanalysis.targetcompletiondate", "zh-CN", "目标完成日期", "目标完成日期"),
            // entity.perfanalysis.targetcompletiondate
            new TranslationSeedItem("entity.perfanalysis.targetcompletiondate", "zh-HK", "目标完成日期_hk", "目标完成日期"),

            // entity.perfanalysis.progresspercentage
            new TranslationSeedItem("entity.perfanalysis.progresspercentage", "en-US", "进度百分比_us", "进度百分比（%）"),
            // entity.perfanalysis.progresspercentage
            new TranslationSeedItem("entity.perfanalysis.progresspercentage", "ja-JP", "进度百分比_jp", "进度百分比（%）"),
            // entity.perfanalysis.progresspercentage
            new TranslationSeedItem("entity.perfanalysis.progresspercentage", "zh-CN", "进度百分比", "进度百分比（%）"),
            // entity.perfanalysis.progresspercentage
            new TranslationSeedItem("entity.perfanalysis.progresspercentage", "zh-HK", "进度百分比_hk", "进度百分比（%）"),

            // entity.perfanalysis.resultdescription
            new TranslationSeedItem("entity.perfanalysis.resultdescription", "en-US", "改进结果说明_us", "改进结果说明"),
            // entity.perfanalysis.resultdescription
            new TranslationSeedItem("entity.perfanalysis.resultdescription", "ja-JP", "改进结果说明_jp", "改进结果说明"),
            // entity.perfanalysis.resultdescription
            new TranslationSeedItem("entity.perfanalysis.resultdescription", "zh-CN", "改进结果说明", "改进结果说明"),
            // entity.perfanalysis.resultdescription
            new TranslationSeedItem("entity.perfanalysis.resultdescription", "zh-HK", "改进结果说明_hk", "改进结果说明"),

            // entity.perfanalysis.mentorid
            new TranslationSeedItem("entity.perfanalysis.mentorid", "en-US", "指导老师ID_us", "指导老师（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfanalysis.mentorid
            new TranslationSeedItem("entity.perfanalysis.mentorid", "ja-JP", "指导老师ID_jp", "指导老师（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfanalysis.mentorid
            new TranslationSeedItem("entity.perfanalysis.mentorid", "zh-CN", "指导老师ID", "指导老师（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfanalysis.mentorid
            new TranslationSeedItem("entity.perfanalysis.mentorid", "zh-HK", "指导老师ID_hk", "指导老师（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.perfanalysis.mentorname
            new TranslationSeedItem("entity.perfanalysis.mentorname", "en-US", "指导老师名称_us", "指导老师名称（冗余：按 MentorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.perfanalysis.mentorname
            new TranslationSeedItem("entity.perfanalysis.mentorname", "ja-JP", "指导老师名称_jp", "指导老师名称（冗余：按 MentorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.perfanalysis.mentorname
            new TranslationSeedItem("entity.perfanalysis.mentorname", "zh-CN", "指导老师名称", "指导老师名称（冗余：按 MentorId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.perfanalysis.mentorname
            new TranslationSeedItem("entity.perfanalysis.mentorname", "zh-HK", "指导老师名称_hk", "指导老师名称（冗余：按 MentorId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.perfanalysis.improvementstatus
            new TranslationSeedItem("entity.perfanalysis.improvementstatus", "en-US", "业务状态_us", "业务状态（字典 humanresource_performance_improvement_status；0=待审批 1=进行中 2=已完成 3=已关闭）"),
            // entity.perfanalysis.improvementstatus
            new TranslationSeedItem("entity.perfanalysis.improvementstatus", "ja-JP", "业务状态_jp", "业务状态（字典 humanresource_performance_improvement_status；0=待审批 1=进行中 2=已完成 3=已关闭）"),
            // entity.perfanalysis.improvementstatus
            new TranslationSeedItem("entity.perfanalysis.improvementstatus", "zh-CN", "业务状态", "业务状态（字典 humanresource_performance_improvement_status；0=待审批 1=进行中 2=已完成 3=已关闭）"),
            // entity.perfanalysis.improvementstatus
            new TranslationSeedItem("entity.perfanalysis.improvementstatus", "zh-HK", "业务状态_hk", "业务状态（字典 humanresource_performance_improvement_status；0=待审批 1=进行中 2=已完成 3=已关闭）"),
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
