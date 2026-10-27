// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Training
// 文件名称：TaktTrainingCourseI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrainingCourse 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTrainingCourse 实体国际化翻译种子（键前缀 entity.trainingcourse.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrainingCourseI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrainingCourse 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trainingcourse 实体翻译...", tenantCode);

        foreach (var item in GetTrainingCourseTranslations())
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

        TaktLogger.Information("TaktTrainingCourse 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrainingCourse 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trainingcourse._self / entity.trainingcourse.{{field}}；ResourceGroup=Training；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrainingCourseTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trainingcourse._self
            new TranslationSeedItem("entity.trainingcourse._self", "en-US", "Training Course Information_us", "实体名称"),
            // entity.trainingcourse._self
            new TranslationSeedItem("entity.trainingcourse._self", "ja-JP", "培训课程定义信息_jp", "实体名称"),
            // entity.trainingcourse._self
            new TranslationSeedItem("entity.trainingcourse._self", "zh-CN", "培训课程定义信息", "实体名称"),
            // entity.trainingcourse._self
            new TranslationSeedItem("entity.trainingcourse._self", "zh-HK", "培训课程定义信息_hk", "实体名称"),

            // entity.trainingcourse.coursecode
            new TranslationSeedItem("entity.trainingcourse.coursecode", "en-US", "课程编码_us", "课程编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 课程编码规则生成并展示，非手输；单据类型菜单：培训课程）"),
            // entity.trainingcourse.coursecode
            new TranslationSeedItem("entity.trainingcourse.coursecode", "ja-JP", "课程编码_jp", "课程编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 课程编码规则生成并展示，非手输；单据类型菜单：培训课程）"),
            // entity.trainingcourse.coursecode
            new TranslationSeedItem("entity.trainingcourse.coursecode", "zh-CN", "课程编码", "课程编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 课程编码规则生成并展示，非手输；单据类型菜单：培训课程）"),
            // entity.trainingcourse.coursecode
            new TranslationSeedItem("entity.trainingcourse.coursecode", "zh-HK", "课程编码_hk", "课程编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 课程编码规则生成并展示，非手输；单据类型菜单：培训课程）"),

            // entity.trainingcourse.coursename
            new TranslationSeedItem("entity.trainingcourse.coursename", "en-US", "课程名称_us", "课程名称"),
            // entity.trainingcourse.coursename
            new TranslationSeedItem("entity.trainingcourse.coursename", "ja-JP", "课程名称_jp", "课程名称"),
            // entity.trainingcourse.coursename
            new TranslationSeedItem("entity.trainingcourse.coursename", "zh-CN", "课程名称", "课程名称"),
            // entity.trainingcourse.coursename
            new TranslationSeedItem("entity.trainingcourse.coursename", "zh-HK", "课程名称_hk", "课程名称"),

            // entity.trainingcourse.coursetype
            new TranslationSeedItem("entity.trainingcourse.coursetype", "en-US", "课程类型_us", "课程类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),
            // entity.trainingcourse.coursetype
            new TranslationSeedItem("entity.trainingcourse.coursetype", "ja-JP", "课程类型_jp", "课程类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),
            // entity.trainingcourse.coursetype
            new TranslationSeedItem("entity.trainingcourse.coursetype", "zh-CN", "课程类型", "课程类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),
            // entity.trainingcourse.coursetype
            new TranslationSeedItem("entity.trainingcourse.coursetype", "zh-HK", "课程类型_hk", "课程类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）"),

            // entity.trainingcourse.courselevel
            new TranslationSeedItem("entity.trainingcourse.courselevel", "en-US", "课程级别_us", "课程级别（字典 humanresource_training_course_level；列存 DictValue：BEGINNER/INTERMEDIATE/ADVANCED/EXPERT）"),
            // entity.trainingcourse.courselevel
            new TranslationSeedItem("entity.trainingcourse.courselevel", "ja-JP", "课程级别_jp", "课程级别（字典 humanresource_training_course_level；列存 DictValue：BEGINNER/INTERMEDIATE/ADVANCED/EXPERT）"),
            // entity.trainingcourse.courselevel
            new TranslationSeedItem("entity.trainingcourse.courselevel", "zh-CN", "课程级别", "课程级别（字典 humanresource_training_course_level；列存 DictValue：BEGINNER/INTERMEDIATE/ADVANCED/EXPERT）"),
            // entity.trainingcourse.courselevel
            new TranslationSeedItem("entity.trainingcourse.courselevel", "zh-HK", "课程级别_hk", "课程级别（字典 humanresource_training_course_level；列存 DictValue：BEGINNER/INTERMEDIATE/ADVANCED/EXPERT）"),

            // entity.trainingcourse.coursedescription
            new TranslationSeedItem("entity.trainingcourse.coursedescription", "en-US", "课程描述_us", "课程描述"),
            // entity.trainingcourse.coursedescription
            new TranslationSeedItem("entity.trainingcourse.coursedescription", "ja-JP", "课程描述_jp", "课程描述"),
            // entity.trainingcourse.coursedescription
            new TranslationSeedItem("entity.trainingcourse.coursedescription", "zh-CN", "课程描述", "课程描述"),
            // entity.trainingcourse.coursedescription
            new TranslationSeedItem("entity.trainingcourse.coursedescription", "zh-HK", "课程描述_hk", "课程描述"),

            // entity.trainingcourse.courseobjectives
            new TranslationSeedItem("entity.trainingcourse.courseobjectives", "en-US", "课程目标_us", "课程目标"),
            // entity.trainingcourse.courseobjectives
            new TranslationSeedItem("entity.trainingcourse.courseobjectives", "ja-JP", "课程目标_jp", "课程目标"),
            // entity.trainingcourse.courseobjectives
            new TranslationSeedItem("entity.trainingcourse.courseobjectives", "zh-CN", "课程目标", "课程目标"),
            // entity.trainingcourse.courseobjectives
            new TranslationSeedItem("entity.trainingcourse.courseobjectives", "zh-HK", "课程目标_hk", "课程目标"),

            // entity.trainingcourse.traininghours
            new TranslationSeedItem("entity.trainingcourse.traininghours", "en-US", "培训时长_us", "培训时长（小时）"),
            // entity.trainingcourse.traininghours
            new TranslationSeedItem("entity.trainingcourse.traininghours", "ja-JP", "培训时长_jp", "培训时长（小时）"),
            // entity.trainingcourse.traininghours
            new TranslationSeedItem("entity.trainingcourse.traininghours", "zh-CN", "培训时长", "培训时长（小时）"),
            // entity.trainingcourse.traininghours
            new TranslationSeedItem("entity.trainingcourse.traininghours", "zh-HK", "培训时长_hk", "培训时长（小时）"),

            // entity.trainingcourse.maininstructor
            new TranslationSeedItem("entity.trainingcourse.maininstructor", "en-US", "主讲讲师_us", "主讲讲师"),
            // entity.trainingcourse.maininstructor
            new TranslationSeedItem("entity.trainingcourse.maininstructor", "ja-JP", "主讲讲师_jp", "主讲讲师"),
            // entity.trainingcourse.maininstructor
            new TranslationSeedItem("entity.trainingcourse.maininstructor", "zh-CN", "主讲讲师", "主讲讲师"),
            // entity.trainingcourse.maininstructor
            new TranslationSeedItem("entity.trainingcourse.maininstructor", "zh-HK", "主讲讲师_hk", "主讲讲师"),

            // entity.trainingcourse.trainingmethod
            new TranslationSeedItem("entity.trainingcourse.trainingmethod", "en-US", "培训方式_us", "培训方式（字典 humanresource_training_method；列存 DictValue：OFFLINE/ONLINE/HYBRID）"),
            // entity.trainingcourse.trainingmethod
            new TranslationSeedItem("entity.trainingcourse.trainingmethod", "ja-JP", "培训方式_jp", "培训方式（字典 humanresource_training_method；列存 DictValue：OFFLINE/ONLINE/HYBRID）"),
            // entity.trainingcourse.trainingmethod
            new TranslationSeedItem("entity.trainingcourse.trainingmethod", "zh-CN", "培训方式", "培训方式（字典 humanresource_training_method；列存 DictValue：OFFLINE/ONLINE/HYBRID）"),
            // entity.trainingcourse.trainingmethod
            new TranslationSeedItem("entity.trainingcourse.trainingmethod", "zh-HK", "培训方式_hk", "培训方式（字典 humanresource_training_method；列存 DictValue：OFFLINE/ONLINE/HYBRID）"),

            // entity.trainingcourse.assessmentmethod
            new TranslationSeedItem("entity.trainingcourse.assessmentmethod", "en-US", "考核方式_us", "考核方式（字典 humanresource_training_assessment_method；列存 DictValue：EXAM/PRACTICAL/ASSIGNMENT/NONE）"),
            // entity.trainingcourse.assessmentmethod
            new TranslationSeedItem("entity.trainingcourse.assessmentmethod", "ja-JP", "考核方式_jp", "考核方式（字典 humanresource_training_assessment_method；列存 DictValue：EXAM/PRACTICAL/ASSIGNMENT/NONE）"),
            // entity.trainingcourse.assessmentmethod
            new TranslationSeedItem("entity.trainingcourse.assessmentmethod", "zh-CN", "考核方式", "考核方式（字典 humanresource_training_assessment_method；列存 DictValue：EXAM/PRACTICAL/ASSIGNMENT/NONE）"),
            // entity.trainingcourse.assessmentmethod
            new TranslationSeedItem("entity.trainingcourse.assessmentmethod", "zh-HK", "考核方式_hk", "考核方式（字典 humanresource_training_assessment_method；列存 DictValue：EXAM/PRACTICAL/ASSIGNMENT/NONE）"),

            // entity.trainingcourse.passingscore
            new TranslationSeedItem("entity.trainingcourse.passingscore", "en-US", "及格分数线_us", "及格分数线"),
            // entity.trainingcourse.passingscore
            new TranslationSeedItem("entity.trainingcourse.passingscore", "ja-JP", "及格分数线_jp", "及格分数线"),
            // entity.trainingcourse.passingscore
            new TranslationSeedItem("entity.trainingcourse.passingscore", "zh-CN", "及格分数线", "及格分数线"),
            // entity.trainingcourse.passingscore
            new TranslationSeedItem("entity.trainingcourse.passingscore", "zh-HK", "及格分数线_hk", "及格分数线"),

            // entity.trainingcourse.sortorder
            new TranslationSeedItem("entity.trainingcourse.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.trainingcourse.sortorder
            new TranslationSeedItem("entity.trainingcourse.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.trainingcourse.sortorder
            new TranslationSeedItem("entity.trainingcourse.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.trainingcourse.sortorder
            new TranslationSeedItem("entity.trainingcourse.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.trainingcourse.status
            new TranslationSeedItem("entity.trainingcourse.status", "en-US", "状态_us", "课程状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.trainingcourse.status
            new TranslationSeedItem("entity.trainingcourse.status", "ja-JP", "状态_jp", "课程状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.trainingcourse.status
            new TranslationSeedItem("entity.trainingcourse.status", "zh-CN", "状态", "课程状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.trainingcourse.status
            new TranslationSeedItem("entity.trainingcourse.status", "zh-HK", "状态_hk", "课程状态（字典 sys_normal_disable；1=启用 0=禁用）"),
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
