// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Training
// 文件名称：TaktTrainingPlanI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrainingPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTrainingPlan 实体国际化翻译种子（键前缀 entity.trainingplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrainingPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrainingPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trainingplan 实体翻译...", tenantCode);

        foreach (var item in GetTrainingPlanTranslations())
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

        TaktLogger.Information("TaktTrainingPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrainingPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trainingplan._self / entity.trainingplan.{{field}}；ResourceGroup=Training；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrainingPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trainingplan._self
            new TranslationSeedItem("entity.trainingplan._self", "en-US", "Training Plan Information_us", "实体名称"),
            // entity.trainingplan._self
            new TranslationSeedItem("entity.trainingplan._self", "ja-JP", "培训计划信息_jp", "实体名称"),
            // entity.trainingplan._self
            new TranslationSeedItem("entity.trainingplan._self", "zh-CN", "培训计划信息", "实体名称"),
            // entity.trainingplan._self
            new TranslationSeedItem("entity.trainingplan._self", "zh-HK", "培训计划信息_hk", "实体名称"),

            // entity.trainingplan.plancode
            new TranslationSeedItem("entity.trainingplan.plancode", "en-US", "计划编码_us", "计划编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 计划编码规则生成并展示，非手输；单据类型菜单：年度计划）"),
            // entity.trainingplan.plancode
            new TranslationSeedItem("entity.trainingplan.plancode", "ja-JP", "计划编码_jp", "计划编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 计划编码规则生成并展示，非手输；单据类型菜单：年度计划）"),
            // entity.trainingplan.plancode
            new TranslationSeedItem("entity.trainingplan.plancode", "zh-CN", "计划编码", "计划编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 计划编码规则生成并展示，非手输；单据类型菜单：年度计划）"),
            // entity.trainingplan.plancode
            new TranslationSeedItem("entity.trainingplan.plancode", "zh-HK", "计划编码_hk", "计划编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 计划编码规则生成并展示，非手输；单据类型菜单：年度计划）"),

            // entity.trainingplan.planname
            new TranslationSeedItem("entity.trainingplan.planname", "en-US", "计划名称_us", "计划名称"),
            // entity.trainingplan.planname
            new TranslationSeedItem("entity.trainingplan.planname", "ja-JP", "计划名称_jp", "计划名称"),
            // entity.trainingplan.planname
            new TranslationSeedItem("entity.trainingplan.planname", "zh-CN", "计划名称", "计划名称"),
            // entity.trainingplan.planname
            new TranslationSeedItem("entity.trainingplan.planname", "zh-HK", "计划名称_hk", "计划名称"),

            // entity.trainingplan.planyear
            new TranslationSeedItem("entity.trainingplan.planyear", "en-US", "计划年度_us", "计划年度"),
            // entity.trainingplan.planyear
            new TranslationSeedItem("entity.trainingplan.planyear", "ja-JP", "计划年度_jp", "计划年度"),
            // entity.trainingplan.planyear
            new TranslationSeedItem("entity.trainingplan.planyear", "zh-CN", "计划年度", "计划年度"),
            // entity.trainingplan.planyear
            new TranslationSeedItem("entity.trainingplan.planyear", "zh-HK", "计划年度_hk", "计划年度"),

            // entity.trainingplan.plantype
            new TranslationSeedItem("entity.trainingplan.plantype", "en-US", "计划类型_us", "计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）"),
            // entity.trainingplan.plantype
            new TranslationSeedItem("entity.trainingplan.plantype", "ja-JP", "计划类型_jp", "计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）"),
            // entity.trainingplan.plantype
            new TranslationSeedItem("entity.trainingplan.plantype", "zh-CN", "计划类型", "计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）"),
            // entity.trainingplan.plantype
            new TranslationSeedItem("entity.trainingplan.plantype", "zh-HK", "计划类型_hk", "计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）"),

            // entity.trainingplan.applicabledepartment
            new TranslationSeedItem("entity.trainingplan.applicabledepartment", "en-US", "适用部门_us", "适用部门"),
            // entity.trainingplan.applicabledepartment
            new TranslationSeedItem("entity.trainingplan.applicabledepartment", "ja-JP", "适用部门_jp", "适用部门"),
            // entity.trainingplan.applicabledepartment
            new TranslationSeedItem("entity.trainingplan.applicabledepartment", "zh-CN", "适用部门", "适用部门"),
            // entity.trainingplan.applicabledepartment
            new TranslationSeedItem("entity.trainingplan.applicabledepartment", "zh-HK", "适用部门_hk", "适用部门"),

            // entity.trainingplan.startdate
            new TranslationSeedItem("entity.trainingplan.startdate", "en-US", "计划开始日期_us", "计划开始日期"),
            // entity.trainingplan.startdate
            new TranslationSeedItem("entity.trainingplan.startdate", "ja-JP", "计划开始日期_jp", "计划开始日期"),
            // entity.trainingplan.startdate
            new TranslationSeedItem("entity.trainingplan.startdate", "zh-CN", "计划开始日期", "计划开始日期"),
            // entity.trainingplan.startdate
            new TranslationSeedItem("entity.trainingplan.startdate", "zh-HK", "计划开始日期_hk", "计划开始日期"),

            // entity.trainingplan.enddate
            new TranslationSeedItem("entity.trainingplan.enddate", "en-US", "计划结束日期_us", "计划结束日期"),
            // entity.trainingplan.enddate
            new TranslationSeedItem("entity.trainingplan.enddate", "ja-JP", "计划结束日期_jp", "计划结束日期"),
            // entity.trainingplan.enddate
            new TranslationSeedItem("entity.trainingplan.enddate", "zh-CN", "计划结束日期", "计划结束日期"),
            // entity.trainingplan.enddate
            new TranslationSeedItem("entity.trainingplan.enddate", "zh-HK", "计划结束日期_hk", "计划结束日期"),

            // entity.trainingplan.trainingobjectives
            new TranslationSeedItem("entity.trainingplan.trainingobjectives", "en-US", "培训目标_us", "培训目标"),
            // entity.trainingplan.trainingobjectives
            new TranslationSeedItem("entity.trainingplan.trainingobjectives", "ja-JP", "培训目标_jp", "培训目标"),
            // entity.trainingplan.trainingobjectives
            new TranslationSeedItem("entity.trainingplan.trainingobjectives", "zh-CN", "培训目标", "培训目标"),
            // entity.trainingplan.trainingobjectives
            new TranslationSeedItem("entity.trainingplan.trainingobjectives", "zh-HK", "培训目标_hk", "培训目标"),

            // entity.trainingplan.plannedheadcount
            new TranslationSeedItem("entity.trainingplan.plannedheadcount", "en-US", "计划培训人数_us", "计划培训人数"),
            // entity.trainingplan.plannedheadcount
            new TranslationSeedItem("entity.trainingplan.plannedheadcount", "ja-JP", "计划培训人数_jp", "计划培训人数"),
            // entity.trainingplan.plannedheadcount
            new TranslationSeedItem("entity.trainingplan.plannedheadcount", "zh-CN", "计划培训人数", "计划培训人数"),
            // entity.trainingplan.plannedheadcount
            new TranslationSeedItem("entity.trainingplan.plannedheadcount", "zh-HK", "计划培训人数_hk", "计划培训人数"),

            // entity.trainingplan.trainingbudget
            new TranslationSeedItem("entity.trainingplan.trainingbudget", "en-US", "培训预算_us", "培训预算（元）"),
            // entity.trainingplan.trainingbudget
            new TranslationSeedItem("entity.trainingplan.trainingbudget", "ja-JP", "培训预算_jp", "培训预算（元）"),
            // entity.trainingplan.trainingbudget
            new TranslationSeedItem("entity.trainingplan.trainingbudget", "zh-CN", "培训预算", "培训预算（元）"),
            // entity.trainingplan.trainingbudget
            new TranslationSeedItem("entity.trainingplan.trainingbudget", "zh-HK", "培训预算_hk", "培训预算（元）"),

            // entity.trainingplan.description
            new TranslationSeedItem("entity.trainingplan.description", "en-US", "计划说明_us", "计划说明"),
            // entity.trainingplan.description
            new TranslationSeedItem("entity.trainingplan.description", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.trainingplan.description
            new TranslationSeedItem("entity.trainingplan.description", "zh-CN", "计划说明", "计划说明"),
            // entity.trainingplan.description
            new TranslationSeedItem("entity.trainingplan.description", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.trainingplan.status
            new TranslationSeedItem("entity.trainingplan.status", "en-US", "业务状态_us", "计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.trainingplan.status
            new TranslationSeedItem("entity.trainingplan.status", "ja-JP", "业务状态_jp", "计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.trainingplan.status
            new TranslationSeedItem("entity.trainingplan.status", "zh-CN", "业务状态", "计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.trainingplan.status
            new TranslationSeedItem("entity.trainingplan.status", "zh-HK", "业务状态_hk", "计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）"),
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
