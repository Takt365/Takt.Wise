// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery
// 文件名称：TaktConfigurableFieldI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurableField 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktConfigurableField 实体国际化翻译种子（键前缀 entity.configurablefield.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableFieldI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurableField 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurablefield 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableFieldTranslations())
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

        TaktLogger.Information("TaktConfigurableField 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurableField 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurablefield._self / entity.configurablefield.{{field}}；ResourceGroup=QuickQuery；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableFieldTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurablefield._self
            new TranslationSeedItem("entity.configurablefield._self", "en-US", "Configurable Field Information_us", "实体名称"),
            // entity.configurablefield._self
            new TranslationSeedItem("entity.configurablefield._self", "ja-JP", "定制报表输出字段定义信息_jp", "实体名称"),
            // entity.configurablefield._self
            new TranslationSeedItem("entity.configurablefield._self", "zh-CN", "定制报表输出字段定义信息", "实体名称"),
            // entity.configurablefield._self
            new TranslationSeedItem("entity.configurablefield._self", "zh-HK", "定制报表输出字段定义信息_hk", "实体名称"),

            // entity.configurablefield.configurableid
            new TranslationSeedItem("entity.configurablefield.configurableid", "en-US", "定制报表主表ID_us", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablefield.configurableid
            new TranslationSeedItem("entity.configurablefield.configurableid", "ja-JP", "定制报表主表ID_jp", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablefield.configurableid
            new TranslationSeedItem("entity.configurablefield.configurableid", "zh-CN", "定制报表主表ID", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablefield.configurableid
            new TranslationSeedItem("entity.configurablefield.configurableid", "zh-HK", "定制报表主表ID_hk", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),

            // entity.configurablefield.sourcealias
            new TranslationSeedItem("entity.configurablefield.sourcealias", "en-US", "数据源别名_us", "数据源别名"),
            // entity.configurablefield.sourcealias
            new TranslationSeedItem("entity.configurablefield.sourcealias", "ja-JP", "数据源别名_jp", "数据源别名"),
            // entity.configurablefield.sourcealias
            new TranslationSeedItem("entity.configurablefield.sourcealias", "zh-CN", "数据源别名", "数据源别名"),
            // entity.configurablefield.sourcealias
            new TranslationSeedItem("entity.configurablefield.sourcealias", "zh-HK", "数据源别名_hk", "数据源别名"),

            // entity.configurablefield.columnname
            new TranslationSeedItem("entity.configurablefield.columnname", "en-US", "列名_us", "列名"),
            // entity.configurablefield.columnname
            new TranslationSeedItem("entity.configurablefield.columnname", "ja-JP", "列名_jp", "列名"),
            // entity.configurablefield.columnname
            new TranslationSeedItem("entity.configurablefield.columnname", "zh-CN", "列名", "列名"),
            // entity.configurablefield.columnname
            new TranslationSeedItem("entity.configurablefield.columnname", "zh-HK", "列名_hk", "列名"),

            // entity.configurablefield.displayname
            new TranslationSeedItem("entity.configurablefield.displayname", "en-US", "显示名称_us", "显示名称（表头/Excel 列标题）"),
            // entity.configurablefield.displayname
            new TranslationSeedItem("entity.configurablefield.displayname", "ja-JP", "显示名称_jp", "显示名称（表头/Excel 列标题）"),
            // entity.configurablefield.displayname
            new TranslationSeedItem("entity.configurablefield.displayname", "zh-CN", "显示名称", "显示名称（表头/Excel 列标题）"),
            // entity.configurablefield.displayname
            new TranslationSeedItem("entity.configurablefield.displayname", "zh-HK", "显示名称_hk", "显示名称（表头/Excel 列标题）"),

            // entity.configurablefield.outputalias
            new TranslationSeedItem("entity.configurablefield.outputalias", "en-US", "输出别名_us", "输出别名（SELECT AS，为空时使用 display_name）"),
            // entity.configurablefield.outputalias
            new TranslationSeedItem("entity.configurablefield.outputalias", "ja-JP", "输出别名_jp", "输出别名（SELECT AS，为空时使用 display_name）"),
            // entity.configurablefield.outputalias
            new TranslationSeedItem("entity.configurablefield.outputalias", "zh-CN", "输出别名", "输出别名（SELECT AS，为空时使用 display_name）"),
            // entity.configurablefield.outputalias
            new TranslationSeedItem("entity.configurablefield.outputalias", "zh-HK", "输出别名_hk", "输出别名（SELECT AS，为空时使用 display_name）"),

            // entity.configurablefield.aggregatefunc
            new TranslationSeedItem("entity.configurablefield.aggregatefunc", "en-US", "聚合函数_us", "聚合函数（字典 sys_configurable_aggregate_func；0=无 1=COUNT 2=SUM 3=AVG 4=MIN 5=MAX）"),
            // entity.configurablefield.aggregatefunc
            new TranslationSeedItem("entity.configurablefield.aggregatefunc", "ja-JP", "聚合函数_jp", "聚合函数（字典 sys_configurable_aggregate_func；0=无 1=COUNT 2=SUM 3=AVG 4=MIN 5=MAX）"),
            // entity.configurablefield.aggregatefunc
            new TranslationSeedItem("entity.configurablefield.aggregatefunc", "zh-CN", "聚合函数", "聚合函数（字典 sys_configurable_aggregate_func；0=无 1=COUNT 2=SUM 3=AVG 4=MIN 5=MAX）"),
            // entity.configurablefield.aggregatefunc
            new TranslationSeedItem("entity.configurablefield.aggregatefunc", "zh-HK", "聚合函数_hk", "聚合函数（字典 sys_configurable_aggregate_func；0=无 1=COUNT 2=SUM 3=AVG 4=MIN 5=MAX）"),

            // entity.configurablefield.isvisible
            new TranslationSeedItem("entity.configurablefield.isvisible", "en-US", "是否输出_us", "是否输出（字典 sys_yes_no；0=否 1=是）"),
            // entity.configurablefield.isvisible
            new TranslationSeedItem("entity.configurablefield.isvisible", "ja-JP", "是否输出_jp", "是否输出（字典 sys_yes_no；0=否 1=是）"),
            // entity.configurablefield.isvisible
            new TranslationSeedItem("entity.configurablefield.isvisible", "zh-CN", "是否输出", "是否输出（字典 sys_yes_no；0=否 1=是）"),
            // entity.configurablefield.isvisible
            new TranslationSeedItem("entity.configurablefield.isvisible", "zh-HK", "是否输出_hk", "是否输出（字典 sys_yes_no；0=否 1=是）"),

            // entity.configurablefield.sortorder
            new TranslationSeedItem("entity.configurablefield.sortorder", "en-US", "排序号_us", "排序号（回填）（SELECT 列顺序）"),
            // entity.configurablefield.sortorder
            new TranslationSeedItem("entity.configurablefield.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（SELECT 列顺序）"),
            // entity.configurablefield.sortorder
            new TranslationSeedItem("entity.configurablefield.sortorder", "zh-CN", "排序号", "排序号（回填）（SELECT 列顺序）"),
            // entity.configurablefield.sortorder
            new TranslationSeedItem("entity.configurablefield.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（SELECT 列顺序）"),

            // entity.configurablefield.configurable
            new TranslationSeedItem("entity.configurablefield.configurable", "en-US", "关联的定制报表主表_us", "关联的定制报表主表"),
            // entity.configurablefield.configurable
            new TranslationSeedItem("entity.configurablefield.configurable", "ja-JP", "关联的定制报表主表_jp", "关联的定制报表主表"),
            // entity.configurablefield.configurable
            new TranslationSeedItem("entity.configurablefield.configurable", "zh-CN", "关联的定制报表主表", "关联的定制报表主表"),
            // entity.configurablefield.configurable
            new TranslationSeedItem("entity.configurablefield.configurable", "zh-HK", "关联的定制报表主表_hk", "关联的定制报表主表"),
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
