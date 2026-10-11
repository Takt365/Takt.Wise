// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Performance
// 文件名称：TaktPerfSchemeI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPerfScheme 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPerfScheme 实体国际化翻译种子（键前缀 entity.perfscheme.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPerfSchemeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPerfScheme 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 perfscheme 实体翻译...", tenantCode);

        foreach (var item in GetPerfSchemeTranslations())
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

        TaktLogger.Information("TaktPerfScheme 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPerfScheme 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.perfscheme._self / entity.perfscheme.{{field}}；ResourceGroup=Performance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPerfSchemeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.perfscheme._self
            new TranslationSeedItem("entity.perfscheme._self", "en-US", "Perf Scheme Information_us", "实体名称"),
            // entity.perfscheme._self
            new TranslationSeedItem("entity.perfscheme._self", "ja-JP", "绩效方案指标信息_jp", "实体名称"),
            // entity.perfscheme._self
            new TranslationSeedItem("entity.perfscheme._self", "zh-CN", "绩效方案指标信息", "实体名称"),
            // entity.perfscheme._self
            new TranslationSeedItem("entity.perfscheme._self", "zh-HK", "绩效方案指标信息_hk", "实体名称"),

            // entity.perfscheme.schemecode
            new TranslationSeedItem("entity.perfscheme.schemecode", "en-US", "方案编码_us", "方案编码"),
            // entity.perfscheme.schemecode
            new TranslationSeedItem("entity.perfscheme.schemecode", "ja-JP", "方案编码_jp", "方案编码"),
            // entity.perfscheme.schemecode
            new TranslationSeedItem("entity.perfscheme.schemecode", "zh-CN", "方案编码", "方案编码"),
            // entity.perfscheme.schemecode
            new TranslationSeedItem("entity.perfscheme.schemecode", "zh-HK", "方案编码_hk", "方案编码"),

            // entity.perfscheme.schemename
            new TranslationSeedItem("entity.perfscheme.schemename", "en-US", "方案名称_us", "方案名称"),
            // entity.perfscheme.schemename
            new TranslationSeedItem("entity.perfscheme.schemename", "ja-JP", "方案名称_jp", "方案名称"),
            // entity.perfscheme.schemename
            new TranslationSeedItem("entity.perfscheme.schemename", "zh-CN", "方案名称", "方案名称"),
            // entity.perfscheme.schemename
            new TranslationSeedItem("entity.perfscheme.schemename", "zh-HK", "方案名称_hk", "方案名称"),

            // entity.perfscheme.applicabledepartment
            new TranslationSeedItem("entity.perfscheme.applicabledepartment", "en-US", "适用部门_us", "适用部门"),
            // entity.perfscheme.applicabledepartment
            new TranslationSeedItem("entity.perfscheme.applicabledepartment", "ja-JP", "适用部门_jp", "适用部门"),
            // entity.perfscheme.applicabledepartment
            new TranslationSeedItem("entity.perfscheme.applicabledepartment", "zh-CN", "适用部门", "适用部门"),
            // entity.perfscheme.applicabledepartment
            new TranslationSeedItem("entity.perfscheme.applicabledepartment", "zh-HK", "适用部门_hk", "适用部门"),

            // entity.perfscheme.cycletype
            new TranslationSeedItem("entity.perfscheme.cycletype", "en-US", "考核周期类型_us", "考核周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),
            // entity.perfscheme.cycletype
            new TranslationSeedItem("entity.perfscheme.cycletype", "ja-JP", "考核周期类型_jp", "考核周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),
            // entity.perfscheme.cycletype
            new TranslationSeedItem("entity.perfscheme.cycletype", "zh-CN", "考核周期类型", "考核周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),
            // entity.perfscheme.cycletype
            new TranslationSeedItem("entity.perfscheme.cycletype", "zh-HK", "考核周期类型_hk", "考核周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）"),

            // entity.perfscheme.scoringstandard
            new TranslationSeedItem("entity.perfscheme.scoringstandard", "en-US", "评分标准_us", "评分标准（字典 humanresource_performance_scoring_standard；列存 DictValue：PERCENT/FIVE/GRADE）"),
            // entity.perfscheme.scoringstandard
            new TranslationSeedItem("entity.perfscheme.scoringstandard", "ja-JP", "评分标准_jp", "评分标准（字典 humanresource_performance_scoring_standard；列存 DictValue：PERCENT/FIVE/GRADE）"),
            // entity.perfscheme.scoringstandard
            new TranslationSeedItem("entity.perfscheme.scoringstandard", "zh-CN", "评分标准", "评分标准（字典 humanresource_performance_scoring_standard；列存 DictValue：PERCENT/FIVE/GRADE）"),
            // entity.perfscheme.scoringstandard
            new TranslationSeedItem("entity.perfscheme.scoringstandard", "zh-HK", "评分标准_hk", "评分标准（字典 humanresource_performance_scoring_standard；列存 DictValue：PERCENT/FIVE/GRADE）"),

            // entity.perfscheme.selfevaluationweight
            new TranslationSeedItem("entity.perfscheme.selfevaluationweight", "en-US", "自评权重_us", "自评权重（%）"),
            // entity.perfscheme.selfevaluationweight
            new TranslationSeedItem("entity.perfscheme.selfevaluationweight", "ja-JP", "自评权重_jp", "自评权重（%）"),
            // entity.perfscheme.selfevaluationweight
            new TranslationSeedItem("entity.perfscheme.selfevaluationweight", "zh-CN", "自评权重", "自评权重（%）"),
            // entity.perfscheme.selfevaluationweight
            new TranslationSeedItem("entity.perfscheme.selfevaluationweight", "zh-HK", "自评权重_hk", "自评权重（%）"),

            // entity.perfscheme.supervisorweight
            new TranslationSeedItem("entity.perfscheme.supervisorweight", "en-US", "主管评分权重_us", "主管评分权重（%）"),
            // entity.perfscheme.supervisorweight
            new TranslationSeedItem("entity.perfscheme.supervisorweight", "ja-JP", "主管评分权重_jp", "主管评分权重（%）"),
            // entity.perfscheme.supervisorweight
            new TranslationSeedItem("entity.perfscheme.supervisorweight", "zh-CN", "主管评分权重", "主管评分权重（%）"),
            // entity.perfscheme.supervisorweight
            new TranslationSeedItem("entity.perfscheme.supervisorweight", "zh-HK", "主管评分权重_hk", "主管评分权重（%）"),

            // entity.perfscheme.metriccode
            new TranslationSeedItem("entity.perfscheme.metriccode", "en-US", "指标编码_us", "指标编码"),
            // entity.perfscheme.metriccode
            new TranslationSeedItem("entity.perfscheme.metriccode", "ja-JP", "指标编码_jp", "指标编码"),
            // entity.perfscheme.metriccode
            new TranslationSeedItem("entity.perfscheme.metriccode", "zh-CN", "指标编码", "指标编码"),
            // entity.perfscheme.metriccode
            new TranslationSeedItem("entity.perfscheme.metriccode", "zh-HK", "指标编码_hk", "指标编码"),

            // entity.perfscheme.metricname
            new TranslationSeedItem("entity.perfscheme.metricname", "en-US", "指标名称_us", "指标名称"),
            // entity.perfscheme.metricname
            new TranslationSeedItem("entity.perfscheme.metricname", "ja-JP", "指标名称_jp", "指标名称"),
            // entity.perfscheme.metricname
            new TranslationSeedItem("entity.perfscheme.metricname", "zh-CN", "指标名称", "指标名称"),
            // entity.perfscheme.metricname
            new TranslationSeedItem("entity.perfscheme.metricname", "zh-HK", "指标名称_hk", "指标名称"),

            // entity.perfscheme.category
            new TranslationSeedItem("entity.perfscheme.category", "en-US", "指标类别_us", "指标类别（字典 humanresource_performance_metric_category；列存 DictValue：PERF/CAPABILITY/ATTITUDE/MANAGEMENT/INNOVATION/QUALITY/EFFICIENCY/SAFETY）"),
            // entity.perfscheme.category
            new TranslationSeedItem("entity.perfscheme.category", "ja-JP", "指标类别_jp", "指标类别（字典 humanresource_performance_metric_category；列存 DictValue：PERF/CAPABILITY/ATTITUDE/MANAGEMENT/INNOVATION/QUALITY/EFFICIENCY/SAFETY）"),
            // entity.perfscheme.category
            new TranslationSeedItem("entity.perfscheme.category", "zh-CN", "指标类别", "指标类别（字典 humanresource_performance_metric_category；列存 DictValue：PERF/CAPABILITY/ATTITUDE/MANAGEMENT/INNOVATION/QUALITY/EFFICIENCY/SAFETY）"),
            // entity.perfscheme.category
            new TranslationSeedItem("entity.perfscheme.category", "zh-HK", "指标类别_hk", "指标类别（字典 humanresource_performance_metric_category；列存 DictValue：PERF/CAPABILITY/ATTITUDE/MANAGEMENT/INNOVATION/QUALITY/EFFICIENCY/SAFETY）"),

            // entity.perfscheme.metrictype
            new TranslationSeedItem("entity.perfscheme.metrictype", "en-US", "指标类型_us", "指标类型（字典 humanresource_performance_metric_type；列存 DictValue：QUANT/QUAL）"),
            // entity.perfscheme.metrictype
            new TranslationSeedItem("entity.perfscheme.metrictype", "ja-JP", "指标类型_jp", "指标类型（字典 humanresource_performance_metric_type；列存 DictValue：QUANT/QUAL）"),
            // entity.perfscheme.metrictype
            new TranslationSeedItem("entity.perfscheme.metrictype", "zh-CN", "指标类型", "指标类型（字典 humanresource_performance_metric_type；列存 DictValue：QUANT/QUAL）"),
            // entity.perfscheme.metrictype
            new TranslationSeedItem("entity.perfscheme.metrictype", "zh-HK", "指标类型_hk", "指标类型（字典 humanresource_performance_metric_type；列存 DictValue：QUANT/QUAL）"),

            // entity.perfscheme.scoringcriteria
            new TranslationSeedItem("entity.perfscheme.scoringcriteria", "en-US", "评分标准说明_us", "评分标准说明"),
            // entity.perfscheme.scoringcriteria
            new TranslationSeedItem("entity.perfscheme.scoringcriteria", "ja-JP", "评分标准说明_jp", "评分标准说明"),
            // entity.perfscheme.scoringcriteria
            new TranslationSeedItem("entity.perfscheme.scoringcriteria", "zh-CN", "评分标准说明", "评分标准说明"),
            // entity.perfscheme.scoringcriteria
            new TranslationSeedItem("entity.perfscheme.scoringcriteria", "zh-HK", "评分标准说明_hk", "评分标准说明"),

            // entity.perfscheme.standardweight
            new TranslationSeedItem("entity.perfscheme.standardweight", "en-US", "标准权重_us", "标准权重（%）"),
            // entity.perfscheme.standardweight
            new TranslationSeedItem("entity.perfscheme.standardweight", "ja-JP", "标准权重_jp", "标准权重（%）"),
            // entity.perfscheme.standardweight
            new TranslationSeedItem("entity.perfscheme.standardweight", "zh-CN", "标准权重", "标准权重（%）"),
            // entity.perfscheme.standardweight
            new TranslationSeedItem("entity.perfscheme.standardweight", "zh-HK", "标准权重_hk", "标准权重（%）"),

            // entity.perfscheme.sortorder
            new TranslationSeedItem("entity.perfscheme.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.perfscheme.sortorder
            new TranslationSeedItem("entity.perfscheme.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.perfscheme.sortorder
            new TranslationSeedItem("entity.perfscheme.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.perfscheme.sortorder
            new TranslationSeedItem("entity.perfscheme.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.perfscheme.schememetricstatus
            new TranslationSeedItem("entity.perfscheme.schememetricstatus", "en-US", "状态_us", "状态（字典 humanresource_performance_scheme_metric_status；0=启用 1=停用）"),
            // entity.perfscheme.schememetricstatus
            new TranslationSeedItem("entity.perfscheme.schememetricstatus", "ja-JP", "状态_jp", "状态（字典 humanresource_performance_scheme_metric_status；0=启用 1=停用）"),
            // entity.perfscheme.schememetricstatus
            new TranslationSeedItem("entity.perfscheme.schememetricstatus", "zh-CN", "状态", "状态（字典 humanresource_performance_scheme_metric_status；0=启用 1=停用）"),
            // entity.perfscheme.schememetricstatus
            new TranslationSeedItem("entity.perfscheme.schememetricstatus", "zh-HK", "状态_hk", "状态（字典 humanresource_performance_scheme_metric_status；0=启用 1=停用）"),
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
