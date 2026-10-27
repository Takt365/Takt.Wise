// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Training
// 文件名称：TaktTrainingAttendeeI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrainingAttendee 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Training;

/// <summary>
/// TaktTrainingAttendee 实体国际化翻译种子（键前缀 entity.trainingattendee.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrainingAttendeeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrainingAttendee 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trainingattendee 实体翻译...", tenantCode);

        foreach (var item in GetTrainingAttendeeTranslations())
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

        TaktLogger.Information("TaktTrainingAttendee 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrainingAttendee 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trainingattendee._self / entity.trainingattendee.{{field}}；ResourceGroup=Training；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrainingAttendeeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trainingattendee._self
            new TranslationSeedItem("entity.trainingattendee._self", "en-US", "Training Attendee Information_us", "实体名称"),
            // entity.trainingattendee._self
            new TranslationSeedItem("entity.trainingattendee._self", "ja-JP", "员工培训结果记录信息_jp", "实体名称"),
            // entity.trainingattendee._self
            new TranslationSeedItem("entity.trainingattendee._self", "zh-CN", "员工培训结果记录信息", "实体名称"),
            // entity.trainingattendee._self
            new TranslationSeedItem("entity.trainingattendee._self", "zh-HK", "员工培训结果记录信息_hk", "实体名称"),

            // entity.trainingattendee.employeeid
            new TranslationSeedItem("entity.trainingattendee.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.trainingattendee.employeeid
            new TranslationSeedItem("entity.trainingattendee.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.trainingattendee.employeeid
            new TranslationSeedItem("entity.trainingattendee.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.trainingattendee.employeeid
            new TranslationSeedItem("entity.trainingattendee.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.trainingattendee.employeename
            new TranslationSeedItem("entity.trainingattendee.employeename", "en-US", "员工姓名_us", "员工姓名"),
            // entity.trainingattendee.employeename
            new TranslationSeedItem("entity.trainingattendee.employeename", "ja-JP", "员工姓名_jp", "员工姓名"),
            // entity.trainingattendee.employeename
            new TranslationSeedItem("entity.trainingattendee.employeename", "zh-CN", "员工姓名", "员工姓名"),
            // entity.trainingattendee.employeename
            new TranslationSeedItem("entity.trainingattendee.employeename", "zh-HK", "员工姓名_hk", "员工姓名"),

            // entity.trainingattendee.trainingcourseid
            new TranslationSeedItem("entity.trainingattendee.trainingcourseid", "en-US", "培训课程ID_us", "培训课程（选项 TaktTrainingCourses/options；DictValue=Id）"),
            // entity.trainingattendee.trainingcourseid
            new TranslationSeedItem("entity.trainingattendee.trainingcourseid", "ja-JP", "培训课程ID_jp", "培训课程（选项 TaktTrainingCourses/options；DictValue=Id）"),
            // entity.trainingattendee.trainingcourseid
            new TranslationSeedItem("entity.trainingattendee.trainingcourseid", "zh-CN", "培训课程ID", "培训课程（选项 TaktTrainingCourses/options；DictValue=Id）"),
            // entity.trainingattendee.trainingcourseid
            new TranslationSeedItem("entity.trainingattendee.trainingcourseid", "zh-HK", "培训课程ID_hk", "培训课程（选项 TaktTrainingCourses/options；DictValue=Id）"),

            // entity.trainingattendee.coursename
            new TranslationSeedItem("entity.trainingattendee.coursename", "en-US", "培训课程名称_us", "培训课程名称"),
            // entity.trainingattendee.coursename
            new TranslationSeedItem("entity.trainingattendee.coursename", "ja-JP", "培训课程名称_jp", "培训课程名称"),
            // entity.trainingattendee.coursename
            new TranslationSeedItem("entity.trainingattendee.coursename", "zh-CN", "培训课程名称", "培训课程名称"),
            // entity.trainingattendee.coursename
            new TranslationSeedItem("entity.trainingattendee.coursename", "zh-HK", "培训课程名称_hk", "培训课程名称"),

            // entity.trainingattendee.trainingtype
            new TranslationSeedItem("entity.trainingattendee.trainingtype", "en-US", "培训类型_us", "培训类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),
            // entity.trainingattendee.trainingtype
            new TranslationSeedItem("entity.trainingattendee.trainingtype", "ja-JP", "培训类型_jp", "培训类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),
            // entity.trainingattendee.trainingtype
            new TranslationSeedItem("entity.trainingattendee.trainingtype", "zh-CN", "培训类型", "培训类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),
            // entity.trainingattendee.trainingtype
            new TranslationSeedItem("entity.trainingattendee.trainingtype", "zh-HK", "培训类型_hk", "培训类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),

            // entity.trainingattendee.instructor
            new TranslationSeedItem("entity.trainingattendee.instructor", "en-US", "培训讲师_us", "培训讲师"),
            // entity.trainingattendee.instructor
            new TranslationSeedItem("entity.trainingattendee.instructor", "ja-JP", "培训讲师_jp", "培训讲师"),
            // entity.trainingattendee.instructor
            new TranslationSeedItem("entity.trainingattendee.instructor", "zh-CN", "培训讲师", "培训讲师"),
            // entity.trainingattendee.instructor
            new TranslationSeedItem("entity.trainingattendee.instructor", "zh-HK", "培训讲师_hk", "培训讲师"),

            // entity.trainingattendee.trainingstartdate
            new TranslationSeedItem("entity.trainingattendee.trainingstartdate", "en-US", "培训开始日期_us", "培训开始日期"),
            // entity.trainingattendee.trainingstartdate
            new TranslationSeedItem("entity.trainingattendee.trainingstartdate", "ja-JP", "培训开始日期_jp", "培训开始日期"),
            // entity.trainingattendee.trainingstartdate
            new TranslationSeedItem("entity.trainingattendee.trainingstartdate", "zh-CN", "培训开始日期", "培训开始日期"),
            // entity.trainingattendee.trainingstartdate
            new TranslationSeedItem("entity.trainingattendee.trainingstartdate", "zh-HK", "培训开始日期_hk", "培训开始日期"),

            // entity.trainingattendee.trainingenddate
            new TranslationSeedItem("entity.trainingattendee.trainingenddate", "en-US", "培训结束日期_us", "培训结束日期"),
            // entity.trainingattendee.trainingenddate
            new TranslationSeedItem("entity.trainingattendee.trainingenddate", "ja-JP", "培训结束日期_jp", "培训结束日期"),
            // entity.trainingattendee.trainingenddate
            new TranslationSeedItem("entity.trainingattendee.trainingenddate", "zh-CN", "培训结束日期", "培训结束日期"),
            // entity.trainingattendee.trainingenddate
            new TranslationSeedItem("entity.trainingattendee.trainingenddate", "zh-HK", "培训结束日期_hk", "培训结束日期"),

            // entity.trainingattendee.trainingdate
            new TranslationSeedItem("entity.trainingattendee.trainingdate", "en-US", "培训日期_us", "培训日期"),
            // entity.trainingattendee.trainingdate
            new TranslationSeedItem("entity.trainingattendee.trainingdate", "ja-JP", "培训日期_jp", "培训日期"),
            // entity.trainingattendee.trainingdate
            new TranslationSeedItem("entity.trainingattendee.trainingdate", "zh-CN", "培训日期", "培训日期"),
            // entity.trainingattendee.trainingdate
            new TranslationSeedItem("entity.trainingattendee.trainingdate", "zh-HK", "培训日期_hk", "培训日期"),

            // entity.trainingattendee.traininghours
            new TranslationSeedItem("entity.trainingattendee.traininghours", "en-US", "培训时长_us", "培训时长（小时）"),
            // entity.trainingattendee.traininghours
            new TranslationSeedItem("entity.trainingattendee.traininghours", "ja-JP", "培训时长_jp", "培训时长（小时）"),
            // entity.trainingattendee.traininghours
            new TranslationSeedItem("entity.trainingattendee.traininghours", "zh-CN", "培训时长", "培训时长（小时）"),
            // entity.trainingattendee.traininghours
            new TranslationSeedItem("entity.trainingattendee.traininghours", "zh-HK", "培训时长_hk", "培训时长（小时）"),

            // entity.trainingattendee.trainingscore
            new TranslationSeedItem("entity.trainingattendee.trainingscore", "en-US", "培训成绩_us", "培训成绩"),
            // entity.trainingattendee.trainingscore
            new TranslationSeedItem("entity.trainingattendee.trainingscore", "ja-JP", "培训成绩_jp", "培训成绩"),
            // entity.trainingattendee.trainingscore
            new TranslationSeedItem("entity.trainingattendee.trainingscore", "zh-CN", "培训成绩", "培训成绩"),
            // entity.trainingattendee.trainingscore
            new TranslationSeedItem("entity.trainingattendee.trainingscore", "zh-HK", "培训成绩_hk", "培训成绩"),

            // entity.trainingattendee.ispassed
            new TranslationSeedItem("entity.trainingattendee.ispassed", "en-US", "是否通过_us", "是否通过（字典 sys_yes_no；0=否 1=是）"),
            // entity.trainingattendee.ispassed
            new TranslationSeedItem("entity.trainingattendee.ispassed", "ja-JP", "是否通过_jp", "是否通过（字典 sys_yes_no；0=否 1=是）"),
            // entity.trainingattendee.ispassed
            new TranslationSeedItem("entity.trainingattendee.ispassed", "zh-CN", "是否通过", "是否通过（字典 sys_yes_no；0=否 1=是）"),
            // entity.trainingattendee.ispassed
            new TranslationSeedItem("entity.trainingattendee.ispassed", "zh-HK", "是否通过_hk", "是否通过（字典 sys_yes_no；0=否 1=是）"),

            // entity.trainingattendee.certificatecode
            new TranslationSeedItem("entity.trainingattendee.certificatecode", "en-US", "证书编码_us", "证书编码"),
            // entity.trainingattendee.certificatecode
            new TranslationSeedItem("entity.trainingattendee.certificatecode", "ja-JP", "证书编码_jp", "证书编码"),
            // entity.trainingattendee.certificatecode
            new TranslationSeedItem("entity.trainingattendee.certificatecode", "zh-CN", "证书编码", "证书编码"),
            // entity.trainingattendee.certificatecode
            new TranslationSeedItem("entity.trainingattendee.certificatecode", "zh-HK", "证书编码_hk", "证书编码"),

            // entity.trainingattendee.trainingevaluation
            new TranslationSeedItem("entity.trainingattendee.trainingevaluation", "en-US", "培训评价_us", "培训评价"),
            // entity.trainingattendee.trainingevaluation
            new TranslationSeedItem("entity.trainingattendee.trainingevaluation", "ja-JP", "培训评价_jp", "培训评价"),
            // entity.trainingattendee.trainingevaluation
            new TranslationSeedItem("entity.trainingattendee.trainingevaluation", "zh-CN", "培训评价", "培训评价"),
            // entity.trainingattendee.trainingevaluation
            new TranslationSeedItem("entity.trainingattendee.trainingevaluation", "zh-HK", "培训评价_hk", "培训评价"),

            // entity.trainingattendee.trainingresultstatus
            new TranslationSeedItem("entity.trainingattendee.trainingresultstatus", "en-US", "状态_us", "参训记录状态（字典 sys_normal_disable；1=有效 0=无效）"),
            // entity.trainingattendee.trainingresultstatus
            new TranslationSeedItem("entity.trainingattendee.trainingresultstatus", "ja-JP", "状态_jp", "参训记录状态（字典 sys_normal_disable；1=有效 0=无效）"),
            // entity.trainingattendee.trainingresultstatus
            new TranslationSeedItem("entity.trainingattendee.trainingresultstatus", "zh-CN", "状态", "参训记录状态（字典 sys_normal_disable；1=有效 0=无效）"),
            // entity.trainingattendee.trainingresultstatus
            new TranslationSeedItem("entity.trainingattendee.trainingresultstatus", "zh-HK", "状态_hk", "参训记录状态（字典 sys_normal_disable；1=有效 0=无效）"),
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
        translation.ResourceGroup = "Training";
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
