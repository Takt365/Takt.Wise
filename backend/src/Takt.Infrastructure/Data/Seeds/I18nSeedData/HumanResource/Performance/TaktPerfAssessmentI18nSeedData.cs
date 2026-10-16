// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktPerfAssessmentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPerfAssessment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPerfAssessment 实体国际化翻译种子（键前缀 entity.perfassessment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPerfAssessmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPerfAssessment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 perfassessment 实体翻译...", tenantCode);

        foreach (var item in GetPerfAssessmentTranslations())
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

        TaktLogger.Information("TaktPerfAssessment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPerfAssessment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.perfassessment._self / entity.perfassessment.{{field}}；ResourceGroup=Performance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPerfAssessmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.perfassessment._self
            new TranslationSeedItem("entity.perfassessment._self", "en-US", "Perf Assessment Information_us", "实体名称"),
            // entity.perfassessment._self
            new TranslationSeedItem("entity.perfassessment._self", "ja-JP", "员工绩效考核信息_jp", "实体名称"),
            // entity.perfassessment._self
            new TranslationSeedItem("entity.perfassessment._self", "zh-CN", "员工绩效考核信息", "实体名称"),
            // entity.perfassessment._self
            new TranslationSeedItem("entity.perfassessment._self", "zh-HK", "员工绩效考核信息_hk", "实体名称"),

            // entity.perfassessment.employeeid
            new TranslationSeedItem("entity.perfassessment.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfassessment.employeeid
            new TranslationSeedItem("entity.perfassessment.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfassessment.employeeid
            new TranslationSeedItem("entity.perfassessment.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfassessment.employeeid
            new TranslationSeedItem("entity.perfassessment.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.perfassessment.employeename
            new TranslationSeedItem("entity.perfassessment.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.perfassessment.employeename
            new TranslationSeedItem("entity.perfassessment.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.perfassessment.employeename
            new TranslationSeedItem("entity.perfassessment.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.perfassessment.employeename
            new TranslationSeedItem("entity.perfassessment.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.perfassessment.assessmentperiod
            new TranslationSeedItem("entity.perfassessment.assessmentperiod", "en-US", "考核周期_us", "考核周期（如 2026-Q1、2026-Annual）"),
            // entity.perfassessment.assessmentperiod
            new TranslationSeedItem("entity.perfassessment.assessmentperiod", "ja-JP", "考核周期_jp", "考核周期（如 2026-Q1、2026-Annual）"),
            // entity.perfassessment.assessmentperiod
            new TranslationSeedItem("entity.perfassessment.assessmentperiod", "zh-CN", "考核周期", "考核周期（如 2026-Q1、2026-Annual）"),
            // entity.perfassessment.assessmentperiod
            new TranslationSeedItem("entity.perfassessment.assessmentperiod", "zh-HK", "考核周期_hk", "考核周期（如 2026-Q1、2026-Annual）"),

            // entity.perfassessment.assessmentdate
            new TranslationSeedItem("entity.perfassessment.assessmentdate", "en-US", "考核日期_us", "考核日期"),
            // entity.perfassessment.assessmentdate
            new TranslationSeedItem("entity.perfassessment.assessmentdate", "ja-JP", "考核日期_jp", "考核日期"),
            // entity.perfassessment.assessmentdate
            new TranslationSeedItem("entity.perfassessment.assessmentdate", "zh-CN", "考核日期", "考核日期"),
            // entity.perfassessment.assessmentdate
            new TranslationSeedItem("entity.perfassessment.assessmentdate", "zh-HK", "考核日期_hk", "考核日期"),

            // entity.perfassessment.schememetricid
            new TranslationSeedItem("entity.perfassessment.schememetricid", "en-US", "方案指标ID_us", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),
            // entity.perfassessment.schememetricid
            new TranslationSeedItem("entity.perfassessment.schememetricid", "ja-JP", "方案指标ID_jp", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),
            // entity.perfassessment.schememetricid
            new TranslationSeedItem("entity.perfassessment.schememetricid", "zh-CN", "方案指标ID", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),
            // entity.perfassessment.schememetricid
            new TranslationSeedItem("entity.perfassessment.schememetricid", "zh-HK", "方案指标ID_hk", "方案指标（选项 TaktPerfSchemes/options；DictValue=Id）"),

            // entity.perfassessment.selfscore
            new TranslationSeedItem("entity.perfassessment.selfscore", "en-US", "自评分数_us", "自评分数"),
            // entity.perfassessment.selfscore
            new TranslationSeedItem("entity.perfassessment.selfscore", "ja-JP", "自评分数_jp", "自评分数"),
            // entity.perfassessment.selfscore
            new TranslationSeedItem("entity.perfassessment.selfscore", "zh-CN", "自评分数", "自评分数"),
            // entity.perfassessment.selfscore
            new TranslationSeedItem("entity.perfassessment.selfscore", "zh-HK", "自评分数_hk", "自评分数"),

            // entity.perfassessment.selfevaluationnotes
            new TranslationSeedItem("entity.perfassessment.selfevaluationnotes", "en-US", "自评说明_us", "自评说明"),
            // entity.perfassessment.selfevaluationnotes
            new TranslationSeedItem("entity.perfassessment.selfevaluationnotes", "ja-JP", "自评说明_jp", "自评说明"),
            // entity.perfassessment.selfevaluationnotes
            new TranslationSeedItem("entity.perfassessment.selfevaluationnotes", "zh-CN", "自评说明", "自评说明"),
            // entity.perfassessment.selfevaluationnotes
            new TranslationSeedItem("entity.perfassessment.selfevaluationnotes", "zh-HK", "自评说明_hk", "自评说明"),

            // entity.perfassessment.supervisorscore
            new TranslationSeedItem("entity.perfassessment.supervisorscore", "en-US", "主管评分_us", "主管评分"),
            // entity.perfassessment.supervisorscore
            new TranslationSeedItem("entity.perfassessment.supervisorscore", "ja-JP", "主管评分_jp", "主管评分"),
            // entity.perfassessment.supervisorscore
            new TranslationSeedItem("entity.perfassessment.supervisorscore", "zh-CN", "主管评分", "主管评分"),
            // entity.perfassessment.supervisorscore
            new TranslationSeedItem("entity.perfassessment.supervisorscore", "zh-HK", "主管评分_hk", "主管评分"),

            // entity.perfassessment.supervisorcomments
            new TranslationSeedItem("entity.perfassessment.supervisorcomments", "en-US", "主管评语_us", "主管评语"),
            // entity.perfassessment.supervisorcomments
            new TranslationSeedItem("entity.perfassessment.supervisorcomments", "ja-JP", "主管评语_jp", "主管评语"),
            // entity.perfassessment.supervisorcomments
            new TranslationSeedItem("entity.perfassessment.supervisorcomments", "zh-CN", "主管评语", "主管评语"),
            // entity.perfassessment.supervisorcomments
            new TranslationSeedItem("entity.perfassessment.supervisorcomments", "zh-HK", "主管评语_hk", "主管评语"),

            // entity.perfassessment.finalscore
            new TranslationSeedItem("entity.perfassessment.finalscore", "en-US", "综合得分_us", "综合得分"),
            // entity.perfassessment.finalscore
            new TranslationSeedItem("entity.perfassessment.finalscore", "ja-JP", "综合得分_jp", "综合得分"),
            // entity.perfassessment.finalscore
            new TranslationSeedItem("entity.perfassessment.finalscore", "zh-CN", "综合得分", "综合得分"),
            // entity.perfassessment.finalscore
            new TranslationSeedItem("entity.perfassessment.finalscore", "zh-HK", "综合得分_hk", "综合得分"),

            // entity.perfassessment.performancegrade
            new TranslationSeedItem("entity.perfassessment.performancegrade", "en-US", "绩效等级_us", "绩效等级（字典 humanresource_performance_grade；列存 DictValue：A/B/C/D/E）"),
            // entity.perfassessment.performancegrade
            new TranslationSeedItem("entity.perfassessment.performancegrade", "ja-JP", "绩效等级_jp", "绩效等级（字典 humanresource_performance_grade；列存 DictValue：A/B/C/D/E）"),
            // entity.perfassessment.performancegrade
            new TranslationSeedItem("entity.perfassessment.performancegrade", "zh-CN", "绩效等级", "绩效等级（字典 humanresource_performance_grade；列存 DictValue：A/B/C/D/E）"),
            // entity.perfassessment.performancegrade
            new TranslationSeedItem("entity.perfassessment.performancegrade", "zh-HK", "绩效等级_hk", "绩效等级（字典 humanresource_performance_grade；列存 DictValue：A/B/C/D/E）"),

            // entity.perfassessment.reviewerid
            new TranslationSeedItem("entity.perfassessment.reviewerid", "en-US", "评审人ID_us", "评审人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfassessment.reviewerid
            new TranslationSeedItem("entity.perfassessment.reviewerid", "ja-JP", "评审人ID_jp", "评审人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfassessment.reviewerid
            new TranslationSeedItem("entity.perfassessment.reviewerid", "zh-CN", "评审人ID", "评审人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.perfassessment.reviewerid
            new TranslationSeedItem("entity.perfassessment.reviewerid", "zh-HK", "评审人ID_hk", "评审人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.perfassessment.reviewername
            new TranslationSeedItem("entity.perfassessment.reviewername", "en-US", "评审人名称_us", "评审人名称（冗余：按 ReviewerId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.perfassessment.reviewername
            new TranslationSeedItem("entity.perfassessment.reviewername", "ja-JP", "评审人名称_jp", "评审人名称（冗余：按 ReviewerId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.perfassessment.reviewername
            new TranslationSeedItem("entity.perfassessment.reviewername", "zh-CN", "评审人名称", "评审人名称（冗余：按 ReviewerId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.perfassessment.reviewername
            new TranslationSeedItem("entity.perfassessment.reviewername", "zh-HK", "评审人名称_hk", "评审人名称（冗余：按 ReviewerId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.perfassessment.interviewdate
            new TranslationSeedItem("entity.perfassessment.interviewdate", "en-US", "面谈日期_us", "面谈日期"),
            // entity.perfassessment.interviewdate
            new TranslationSeedItem("entity.perfassessment.interviewdate", "ja-JP", "面谈日期_jp", "面谈日期"),
            // entity.perfassessment.interviewdate
            new TranslationSeedItem("entity.perfassessment.interviewdate", "zh-CN", "面谈日期", "面谈日期"),
            // entity.perfassessment.interviewdate
            new TranslationSeedItem("entity.perfassessment.interviewdate", "zh-HK", "面谈日期_hk", "面谈日期"),

            // entity.perfassessment.interviewnotes
            new TranslationSeedItem("entity.perfassessment.interviewnotes", "en-US", "面谈记录_us", "面谈记录"),
            // entity.perfassessment.interviewnotes
            new TranslationSeedItem("entity.perfassessment.interviewnotes", "ja-JP", "面谈记录_jp", "面谈记录"),
            // entity.perfassessment.interviewnotes
            new TranslationSeedItem("entity.perfassessment.interviewnotes", "zh-CN", "面谈记录", "面谈记录"),
            // entity.perfassessment.interviewnotes
            new TranslationSeedItem("entity.perfassessment.interviewnotes", "zh-HK", "面谈记录_hk", "面谈记录"),

            // entity.perfassessment.assessmentstatus
            new TranslationSeedItem("entity.perfassessment.assessmentstatus", "en-US", "状态_us", "状态（字典 humanresource_performance_assessment_status；0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
            // entity.perfassessment.assessmentstatus
            new TranslationSeedItem("entity.perfassessment.assessmentstatus", "ja-JP", "状态_jp", "状态（字典 humanresource_performance_assessment_status；0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
            // entity.perfassessment.assessmentstatus
            new TranslationSeedItem("entity.perfassessment.assessmentstatus", "zh-CN", "状态", "状态（字典 humanresource_performance_assessment_status；0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
            // entity.perfassessment.assessmentstatus
            new TranslationSeedItem("entity.perfassessment.assessmentstatus", "zh-HK", "状态_hk", "状态（字典 humanresource_performance_assessment_status；0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）"),
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
