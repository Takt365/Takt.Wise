// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery
// 文件名称：TaktConfigurableSelectionI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurableSelection 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery;

/// <summary>
/// TaktConfigurableSelection 实体国际化翻译种子（键前缀 entity.configurableselection.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableSelectionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurableSelection 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurableselection 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableSelectionTranslations())
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

        TaktLogger.Information("TaktConfigurableSelection 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurableSelection 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurableselection._self / entity.configurableselection.{{field}}；ResourceGroup=QuickQuery；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableSelectionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurableselection._self
            new TranslationSeedItem("entity.configurableselection._self", "en-US", "Configurable Selection Information_us", "实体名称"),
            // entity.configurableselection._self
            new TranslationSeedItem("entity.configurableselection._self", "ja-JP", "定制报表筛选条件信息_jp", "实体名称"),
            // entity.configurableselection._self
            new TranslationSeedItem("entity.configurableselection._self", "zh-CN", "定制报表筛选条件信息", "实体名称"),
            // entity.configurableselection._self
            new TranslationSeedItem("entity.configurableselection._self", "zh-HK", "定制报表筛选条件信息_hk", "实体名称"),

            // entity.configurableselection.configurableid
            new TranslationSeedItem("entity.configurableselection.configurableid", "en-US", "定制报表主表ID_us", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurableselection.configurableid
            new TranslationSeedItem("entity.configurableselection.configurableid", "ja-JP", "定制报表主表ID_jp", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurableselection.configurableid
            new TranslationSeedItem("entity.configurableselection.configurableid", "zh-CN", "定制报表主表ID", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurableselection.configurableid
            new TranslationSeedItem("entity.configurableselection.configurableid", "zh-HK", "定制报表主表ID_hk", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),

            // entity.configurableselection.sourcealias
            new TranslationSeedItem("entity.configurableselection.sourcealias", "en-US", "数据源别名_us", "数据源别名"),
            // entity.configurableselection.sourcealias
            new TranslationSeedItem("entity.configurableselection.sourcealias", "ja-JP", "数据源别名_jp", "数据源别名"),
            // entity.configurableselection.sourcealias
            new TranslationSeedItem("entity.configurableselection.sourcealias", "zh-CN", "数据源别名", "数据源别名"),
            // entity.configurableselection.sourcealias
            new TranslationSeedItem("entity.configurableselection.sourcealias", "zh-HK", "数据源别名_hk", "数据源别名"),

            // entity.configurableselection.columnname
            new TranslationSeedItem("entity.configurableselection.columnname", "en-US", "列名_us", "列名"),
            // entity.configurableselection.columnname
            new TranslationSeedItem("entity.configurableselection.columnname", "ja-JP", "列名_jp", "列名"),
            // entity.configurableselection.columnname
            new TranslationSeedItem("entity.configurableselection.columnname", "zh-CN", "列名", "列名"),
            // entity.configurableselection.columnname
            new TranslationSeedItem("entity.configurableselection.columnname", "zh-HK", "列名_hk", "列名"),

            // entity.configurableselection.displayname
            new TranslationSeedItem("entity.configurableselection.displayname", "en-US", "显示名称_us", "显示名称（筛选项标签）"),
            // entity.configurableselection.displayname
            new TranslationSeedItem("entity.configurableselection.displayname", "ja-JP", "显示名称_jp", "显示名称（筛选项标签）"),
            // entity.configurableselection.displayname
            new TranslationSeedItem("entity.configurableselection.displayname", "zh-CN", "显示名称", "显示名称（筛选项标签）"),
            // entity.configurableselection.displayname
            new TranslationSeedItem("entity.configurableselection.displayname", "zh-HK", "显示名称_hk", "显示名称（筛选项标签）"),

            // entity.configurableselection.filteroperator
            new TranslationSeedItem("entity.configurableselection.filteroperator", "en-US", "比较运算符_us", "比较运算符（字典 code_generator_query_type；1=等于 2=不等于 3=大于 4=大于等于 5=小于 6=小于等于 7=模糊 8=区间）"),
            // entity.configurableselection.filteroperator
            new TranslationSeedItem("entity.configurableselection.filteroperator", "ja-JP", "比较运算符_jp", "比较运算符（字典 code_generator_query_type；1=等于 2=不等于 3=大于 4=大于等于 5=小于 6=小于等于 7=模糊 8=区间）"),
            // entity.configurableselection.filteroperator
            new TranslationSeedItem("entity.configurableselection.filteroperator", "zh-CN", "比较运算符", "比较运算符（字典 code_generator_query_type；1=等于 2=不等于 3=大于 4=大于等于 5=小于 6=小于等于 7=模糊 8=区间）"),
            // entity.configurableselection.filteroperator
            new TranslationSeedItem("entity.configurableselection.filteroperator", "zh-HK", "比较运算符_hk", "比较运算符（字典 code_generator_query_type；1=等于 2=不等于 3=大于 4=大于等于 5=小于 6=小于等于 7=模糊 8=区间）"),

            // entity.configurableselection.defaultvalue
            new TranslationSeedItem("entity.configurableselection.defaultvalue", "en-US", "默认值_us", "默认值（单值或 IN 列表逗号分隔）"),
            // entity.configurableselection.defaultvalue
            new TranslationSeedItem("entity.configurableselection.defaultvalue", "ja-JP", "默认值_jp", "默认值（单值或 IN 列表逗号分隔）"),
            // entity.configurableselection.defaultvalue
            new TranslationSeedItem("entity.configurableselection.defaultvalue", "zh-CN", "默认值", "默认值（单值或 IN 列表逗号分隔）"),
            // entity.configurableselection.defaultvalue
            new TranslationSeedItem("entity.configurableselection.defaultvalue", "zh-HK", "默认值_hk", "默认值（单值或 IN 列表逗号分隔）"),

            // entity.configurableselection.defaultvalueto
            new TranslationSeedItem("entity.configurableselection.defaultvalueto", "en-US", "区间结束值_us", "区间结束值（BETWEEN 时使用）"),
            // entity.configurableselection.defaultvalueto
            new TranslationSeedItem("entity.configurableselection.defaultvalueto", "ja-JP", "区间结束值_jp", "区间结束值（BETWEEN 时使用）"),
            // entity.configurableselection.defaultvalueto
            new TranslationSeedItem("entity.configurableselection.defaultvalueto", "zh-CN", "区间结束值", "区间结束值（BETWEEN 时使用）"),
            // entity.configurableselection.defaultvalueto
            new TranslationSeedItem("entity.configurableselection.defaultvalueto", "zh-HK", "区间结束值_hk", "区间结束值（BETWEEN 时使用）"),

            // entity.configurableselection.isrequired
            new TranslationSeedItem("entity.configurableselection.isrequired", "en-US", "是否必填_us", "是否必填（字典 sys_yes_no；0=否 1=是）"),
            // entity.configurableselection.isrequired
            new TranslationSeedItem("entity.configurableselection.isrequired", "ja-JP", "是否必填_jp", "是否必填（字典 sys_yes_no；0=否 1=是）"),
            // entity.configurableselection.isrequired
            new TranslationSeedItem("entity.configurableselection.isrequired", "zh-CN", "是否必填", "是否必填（字典 sys_yes_no；0=否 1=是）"),
            // entity.configurableselection.isrequired
            new TranslationSeedItem("entity.configurableselection.isrequired", "zh-HK", "是否必填_hk", "是否必填（字典 sys_yes_no；0=否 1=是）"),

            // entity.configurableselection.sortorder
            new TranslationSeedItem("entity.configurableselection.sortorder", "en-US", "排序号_us", "排序号（回填）（筛选项展示顺序）"),
            // entity.configurableselection.sortorder
            new TranslationSeedItem("entity.configurableselection.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（筛选项展示顺序）"),
            // entity.configurableselection.sortorder
            new TranslationSeedItem("entity.configurableselection.sortorder", "zh-CN", "排序号", "排序号（回填）（筛选项展示顺序）"),
            // entity.configurableselection.sortorder
            new TranslationSeedItem("entity.configurableselection.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（筛选项展示顺序）"),

            // entity.configurableselection.configurable
            new TranslationSeedItem("entity.configurableselection.configurable", "en-US", "关联的定制报表主表_us", "关联的定制报表主表"),
            // entity.configurableselection.configurable
            new TranslationSeedItem("entity.configurableselection.configurable", "ja-JP", "关联的定制报表主表_jp", "关联的定制报表主表"),
            // entity.configurableselection.configurable
            new TranslationSeedItem("entity.configurableselection.configurable", "zh-CN", "关联的定制报表主表", "关联的定制报表主表"),
            // entity.configurableselection.configurable
            new TranslationSeedItem("entity.configurableselection.configurable", "zh-HK", "关联的定制报表主表_hk", "关联的定制报表主表"),
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
        translation.ResourceGroup = "QuickQuery";
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
