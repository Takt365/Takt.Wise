// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktPerfCycleI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPerfCycle 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPerfCycle 实体国际化翻译种子（键前缀 entity.perfcycle.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPerfCycleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPerfCycle 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 perfcycle 实体翻译...", tenantCode);

        foreach (var item in GetPerfCycleTranslations())
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

        TaktLogger.Information("TaktPerfCycle 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPerfCycle 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.perfcycle._self / entity.perfcycle.{{field}}；ResourceGroup=Performance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPerfCycleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.perfcycle._self
            new TranslationSeedItem("entity.perfcycle._self", "en-US", "Perf Cycle Information_us", "实体名称"),
            // entity.perfcycle._self
            new TranslationSeedItem("entity.perfcycle._self", "ja-JP", "绩效考核周期日程安排信息_jp", "实体名称"),
            // entity.perfcycle._self
            new TranslationSeedItem("entity.perfcycle._self", "zh-CN", "绩效考核周期日程安排信息", "实体名称"),
            // entity.perfcycle._self
            new TranslationSeedItem("entity.perfcycle._self", "zh-HK", "绩效考核周期日程安排信息_hk", "实体名称"),

            // entity.perfcycle.cyclecode
            new TranslationSeedItem("entity.perfcycle.cyclecode", "en-US", "周期编码_us", "周期编码（租户+公司内唯一）"),
            // entity.perfcycle.cyclecode
            new TranslationSeedItem("entity.perfcycle.cyclecode", "ja-JP", "周期编码_jp", "周期编码（租户+公司内唯一）"),
            // entity.perfcycle.cyclecode
            new TranslationSeedItem("entity.perfcycle.cyclecode", "zh-CN", "周期编码", "周期编码（租户+公司内唯一）"),
            // entity.perfcycle.cyclecode
            new TranslationSeedItem("entity.perfcycle.cyclecode", "zh-HK", "周期编码_hk", "周期编码（租户+公司内唯一）"),

            // entity.perfcycle.cyclename
            new TranslationSeedItem("entity.perfcycle.cyclename", "en-US", "周期名称_us", "周期名称"),
            // entity.perfcycle.cyclename
            new TranslationSeedItem("entity.perfcycle.cyclename", "ja-JP", "周期名称_jp", "周期名称"),
            // entity.perfcycle.cyclename
            new TranslationSeedItem("entity.perfcycle.cyclename", "zh-CN", "周期名称", "周期名称"),
            // entity.perfcycle.cyclename
            new TranslationSeedItem("entity.perfcycle.cyclename", "zh-HK", "周期名称_hk", "周期名称"),

            // entity.perfcycle.cycletype
            new TranslationSeedItem("entity.perfcycle.cycletype", "en-US", "周期类型_us", "周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),
            // entity.perfcycle.cycletype
            new TranslationSeedItem("entity.perfcycle.cycletype", "ja-JP", "周期类型_jp", "周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),
            // entity.perfcycle.cycletype
            new TranslationSeedItem("entity.perfcycle.cycletype", "zh-CN", "周期类型", "周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),
            // entity.perfcycle.cycletype
            new TranslationSeedItem("entity.perfcycle.cycletype", "zh-HK", "周期类型_hk", "周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),

            // entity.perfcycle.cycleyear
            new TranslationSeedItem("entity.perfcycle.cycleyear", "en-US", "周期年度_us", "周期年度"),
            // entity.perfcycle.cycleyear
            new TranslationSeedItem("entity.perfcycle.cycleyear", "ja-JP", "周期年度_jp", "周期年度"),
            // entity.perfcycle.cycleyear
            new TranslationSeedItem("entity.perfcycle.cycleyear", "zh-CN", "周期年度", "周期年度"),
            // entity.perfcycle.cycleyear
            new TranslationSeedItem("entity.perfcycle.cycleyear", "zh-HK", "周期年度_hk", "周期年度"),

            // entity.perfcycle.cyclesequence
            new TranslationSeedItem("entity.perfcycle.cyclesequence", "en-US", "周期序号_us", "周期序号"),
            // entity.perfcycle.cyclesequence
            new TranslationSeedItem("entity.perfcycle.cyclesequence", "ja-JP", "周期序号_jp", "周期序号"),
            // entity.perfcycle.cyclesequence
            new TranslationSeedItem("entity.perfcycle.cyclesequence", "zh-CN", "周期序号", "周期序号"),
            // entity.perfcycle.cyclesequence
            new TranslationSeedItem("entity.perfcycle.cyclesequence", "zh-HK", "周期序号_hk", "周期序号"),

            // entity.perfcycle.startdate
            new TranslationSeedItem("entity.perfcycle.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.perfcycle.startdate
            new TranslationSeedItem("entity.perfcycle.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.perfcycle.startdate
            new TranslationSeedItem("entity.perfcycle.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.perfcycle.startdate
            new TranslationSeedItem("entity.perfcycle.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.perfcycle.enddate
            new TranslationSeedItem("entity.perfcycle.enddate", "en-US", "结束日期_us", "结束日期"),
            // entity.perfcycle.enddate
            new TranslationSeedItem("entity.perfcycle.enddate", "ja-JP", "结束日期_jp", "结束日期"),
            // entity.perfcycle.enddate
            new TranslationSeedItem("entity.perfcycle.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.perfcycle.enddate
            new TranslationSeedItem("entity.perfcycle.enddate", "zh-HK", "结束日期_hk", "结束日期"),

            // entity.perfcycle.goalsettingduedate
            new TranslationSeedItem("entity.perfcycle.goalsettingduedate", "en-US", "目标设定截止日期_us", "目标设定截止日期"),
            // entity.perfcycle.goalsettingduedate
            new TranslationSeedItem("entity.perfcycle.goalsettingduedate", "ja-JP", "目标设定截止日期_jp", "目标设定截止日期"),
            // entity.perfcycle.goalsettingduedate
            new TranslationSeedItem("entity.perfcycle.goalsettingduedate", "zh-CN", "目标设定截止日期", "目标设定截止日期"),
            // entity.perfcycle.goalsettingduedate
            new TranslationSeedItem("entity.perfcycle.goalsettingduedate", "zh-HK", "目标设定截止日期_hk", "目标设定截止日期"),

            // entity.perfcycle.selfevaluationduedate
            new TranslationSeedItem("entity.perfcycle.selfevaluationduedate", "en-US", "自评截止日期_us", "自评截止日期"),
            // entity.perfcycle.selfevaluationduedate
            new TranslationSeedItem("entity.perfcycle.selfevaluationduedate", "ja-JP", "自评截止日期_jp", "自评截止日期"),
            // entity.perfcycle.selfevaluationduedate
            new TranslationSeedItem("entity.perfcycle.selfevaluationduedate", "zh-CN", "自评截止日期", "自评截止日期"),
            // entity.perfcycle.selfevaluationduedate
            new TranslationSeedItem("entity.perfcycle.selfevaluationduedate", "zh-HK", "自评截止日期_hk", "自评截止日期"),

            // entity.perfcycle.supervisorreviewduedate
            new TranslationSeedItem("entity.perfcycle.supervisorreviewduedate", "en-US", "主管评审截止日期_us", "主管评审截止日期"),
            // entity.perfcycle.supervisorreviewduedate
            new TranslationSeedItem("entity.perfcycle.supervisorreviewduedate", "ja-JP", "主管评审截止日期_jp", "主管评审截止日期"),
            // entity.perfcycle.supervisorreviewduedate
            new TranslationSeedItem("entity.perfcycle.supervisorreviewduedate", "zh-CN", "主管评审截止日期", "主管评审截止日期"),
            // entity.perfcycle.supervisorreviewduedate
            new TranslationSeedItem("entity.perfcycle.supervisorreviewduedate", "zh-HK", "主管评审截止日期_hk", "主管评审截止日期"),

            // entity.perfcycle.interviewduedate
            new TranslationSeedItem("entity.perfcycle.interviewduedate", "en-US", "面谈截止日期_us", "面谈截止日期"),
            // entity.perfcycle.interviewduedate
            new TranslationSeedItem("entity.perfcycle.interviewduedate", "ja-JP", "面谈截止日期_jp", "面谈截止日期"),
            // entity.perfcycle.interviewduedate
            new TranslationSeedItem("entity.perfcycle.interviewduedate", "zh-CN", "面谈截止日期", "面谈截止日期"),
            // entity.perfcycle.interviewduedate
            new TranslationSeedItem("entity.perfcycle.interviewduedate", "zh-HK", "面谈截止日期_hk", "面谈截止日期"),

            // entity.perfcycle.resultconfirmationduedate
            new TranslationSeedItem("entity.perfcycle.resultconfirmationduedate", "en-US", "结果确认截止日期_us", "结果确认截止日期"),
            // entity.perfcycle.resultconfirmationduedate
            new TranslationSeedItem("entity.perfcycle.resultconfirmationduedate", "ja-JP", "结果确认截止日期_jp", "结果确认截止日期"),
            // entity.perfcycle.resultconfirmationduedate
            new TranslationSeedItem("entity.perfcycle.resultconfirmationduedate", "zh-CN", "结果确认截止日期", "结果确认截止日期"),
            // entity.perfcycle.resultconfirmationduedate
            new TranslationSeedItem("entity.perfcycle.resultconfirmationduedate", "zh-HK", "结果确认截止日期_hk", "结果确认截止日期"),

            // entity.perfcycle.applicabledepartment
            new TranslationSeedItem("entity.perfcycle.applicabledepartment", "en-US", "适用部门_us", "适用部门"),
            // entity.perfcycle.applicabledepartment
            new TranslationSeedItem("entity.perfcycle.applicabledepartment", "ja-JP", "适用部门_jp", "适用部门"),
            // entity.perfcycle.applicabledepartment
            new TranslationSeedItem("entity.perfcycle.applicabledepartment", "zh-CN", "适用部门", "适用部门"),
            // entity.perfcycle.applicabledepartment
            new TranslationSeedItem("entity.perfcycle.applicabledepartment", "zh-HK", "适用部门_hk", "适用部门"),

            // entity.perfcycle.description
            new TranslationSeedItem("entity.perfcycle.description", "en-US", "周期说明_us", "周期说明"),
            // entity.perfcycle.description
            new TranslationSeedItem("entity.perfcycle.description", "ja-JP", "周期说明_jp", "周期说明"),
            // entity.perfcycle.description
            new TranslationSeedItem("entity.perfcycle.description", "zh-CN", "周期说明", "周期说明"),
            // entity.perfcycle.description
            new TranslationSeedItem("entity.perfcycle.description", "zh-HK", "周期说明_hk", "周期说明"),

            // entity.perfcycle.cycleschedulestatus
            new TranslationSeedItem("entity.perfcycle.cycleschedulestatus", "en-US", "状态_us", "状态（字典 humanresource_performance_cycle_schedule_status；0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
            // entity.perfcycle.cycleschedulestatus
            new TranslationSeedItem("entity.perfcycle.cycleschedulestatus", "ja-JP", "状态_jp", "状态（字典 humanresource_performance_cycle_schedule_status；0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
            // entity.perfcycle.cycleschedulestatus
            new TranslationSeedItem("entity.perfcycle.cycleschedulestatus", "zh-CN", "状态", "状态（字典 humanresource_performance_cycle_schedule_status；0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
            // entity.perfcycle.cycleschedulestatus
            new TranslationSeedItem("entity.perfcycle.cycleschedulestatus", "zh-HK", "状态_hk", "状态（字典 humanresource_performance_cycle_schedule_status；0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）"),
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
