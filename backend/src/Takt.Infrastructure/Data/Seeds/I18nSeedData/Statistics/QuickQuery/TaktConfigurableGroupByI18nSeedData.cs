// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery
// 文件名称：TaktConfigurableGroupByI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurableGroupBy 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktConfigurableGroupBy 实体国际化翻译种子（键前缀 entity.configurablegroupby.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableGroupByI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurableGroupBy 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurablegroupby 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableGroupByTranslations())
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

        TaktLogger.Information("TaktConfigurableGroupBy 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurableGroupBy 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurablegroupby._self / entity.configurablegroupby.{{field}}；ResourceGroup=QuickQuery；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableGroupByTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurablegroupby._self
            new TranslationSeedItem("entity.configurablegroupby._self", "en-US", "Configurable Group By Information_us", "实体名称"),
            // entity.configurablegroupby._self
            new TranslationSeedItem("entity.configurablegroupby._self", "ja-JP", "定制报表分组字段定义信息_jp", "实体名称"),
            // entity.configurablegroupby._self
            new TranslationSeedItem("entity.configurablegroupby._self", "zh-CN", "定制报表分组字段定义信息", "实体名称"),
            // entity.configurablegroupby._self
            new TranslationSeedItem("entity.configurablegroupby._self", "zh-HK", "定制报表分组字段定义信息_hk", "实体名称"),

            // entity.configurablegroupby.configurableid
            new TranslationSeedItem("entity.configurablegroupby.configurableid", "en-US", "定制报表主表ID_us", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablegroupby.configurableid
            new TranslationSeedItem("entity.configurablegroupby.configurableid", "ja-JP", "定制报表主表ID_jp", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablegroupby.configurableid
            new TranslationSeedItem("entity.configurablegroupby.configurableid", "zh-CN", "定制报表主表ID", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablegroupby.configurableid
            new TranslationSeedItem("entity.configurablegroupby.configurableid", "zh-HK", "定制报表主表ID_hk", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),

            // entity.configurablegroupby.sourcealias
            new TranslationSeedItem("entity.configurablegroupby.sourcealias", "en-US", "数据源别名_us", "数据源别名"),
            // entity.configurablegroupby.sourcealias
            new TranslationSeedItem("entity.configurablegroupby.sourcealias", "ja-JP", "数据源别名_jp", "数据源别名"),
            // entity.configurablegroupby.sourcealias
            new TranslationSeedItem("entity.configurablegroupby.sourcealias", "zh-CN", "数据源别名", "数据源别名"),
            // entity.configurablegroupby.sourcealias
            new TranslationSeedItem("entity.configurablegroupby.sourcealias", "zh-HK", "数据源别名_hk", "数据源别名"),

            // entity.configurablegroupby.columnname
            new TranslationSeedItem("entity.configurablegroupby.columnname", "en-US", "列名_us", "列名"),
            // entity.configurablegroupby.columnname
            new TranslationSeedItem("entity.configurablegroupby.columnname", "ja-JP", "列名_jp", "列名"),
            // entity.configurablegroupby.columnname
            new TranslationSeedItem("entity.configurablegroupby.columnname", "zh-CN", "列名", "列名"),
            // entity.configurablegroupby.columnname
            new TranslationSeedItem("entity.configurablegroupby.columnname", "zh-HK", "列名_hk", "列名"),

            // entity.configurablegroupby.sortorder
            new TranslationSeedItem("entity.configurablegroupby.sortorder", "en-US", "排序号_us", "排序号（回填）（GROUP BY 列顺序）"),
            // entity.configurablegroupby.sortorder
            new TranslationSeedItem("entity.configurablegroupby.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（GROUP BY 列顺序）"),
            // entity.configurablegroupby.sortorder
            new TranslationSeedItem("entity.configurablegroupby.sortorder", "zh-CN", "排序号", "排序号（回填）（GROUP BY 列顺序）"),
            // entity.configurablegroupby.sortorder
            new TranslationSeedItem("entity.configurablegroupby.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（GROUP BY 列顺序）"),

            // entity.configurablegroupby.configurable
            new TranslationSeedItem("entity.configurablegroupby.configurable", "en-US", "关联的定制报表主表_us", "关联的定制报表主表"),
            // entity.configurablegroupby.configurable
            new TranslationSeedItem("entity.configurablegroupby.configurable", "ja-JP", "关联的定制报表主表_jp", "关联的定制报表主表"),
            // entity.configurablegroupby.configurable
            new TranslationSeedItem("entity.configurablegroupby.configurable", "zh-CN", "关联的定制报表主表", "关联的定制报表主表"),
            // entity.configurablegroupby.configurable
            new TranslationSeedItem("entity.configurablegroupby.configurable", "zh-HK", "关联的定制报表主表_hk", "关联的定制报表主表"),
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
