// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.QuickQuery
// 文件名称：TaktConfigurableSourceI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurableSource 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktConfigurableSource 实体国际化翻译种子（键前缀 entity.configurablesource.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableSourceI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurableSource 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurablesource 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableSourceTranslations())
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

        TaktLogger.Information("TaktConfigurableSource 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurableSource 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurablesource._self / entity.configurablesource.{{field}}；ResourceGroup=QuickQuery；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableSourceTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurablesource._self
            new TranslationSeedItem("entity.configurablesource._self", "en-US", "Configurable Source Information_us", "实体名称"),
            // entity.configurablesource._self
            new TranslationSeedItem("entity.configurablesource._self", "ja-JP", "定制报表数据源信息_jp", "实体名称"),
            // entity.configurablesource._self
            new TranslationSeedItem("entity.configurablesource._self", "zh-CN", "定制报表数据源信息", "实体名称"),
            // entity.configurablesource._self
            new TranslationSeedItem("entity.configurablesource._self", "zh-HK", "定制报表数据源信息_hk", "实体名称"),

            // entity.configurablesource.configurableid
            new TranslationSeedItem("entity.configurablesource.configurableid", "en-US", "定制报表主表ID_us", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablesource.configurableid
            new TranslationSeedItem("entity.configurablesource.configurableid", "ja-JP", "定制报表主表ID_jp", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablesource.configurableid
            new TranslationSeedItem("entity.configurablesource.configurableid", "zh-CN", "定制报表主表ID", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),
            // entity.configurablesource.configurableid
            new TranslationSeedItem("entity.configurablesource.configurableid", "zh-HK", "定制报表主表ID_hk", "关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）"),

            // entity.configurablesource.sourcealias
            new TranslationSeedItem("entity.configurablesource.sourcealias", "en-US", "数据源别名_us", "数据源别名（如 A、B、C，用于 JOIN 与字段引用）"),
            // entity.configurablesource.sourcealias
            new TranslationSeedItem("entity.configurablesource.sourcealias", "ja-JP", "数据源别名_jp", "数据源别名（如 A、B、C，用于 JOIN 与字段引用）"),
            // entity.configurablesource.sourcealias
            new TranslationSeedItem("entity.configurablesource.sourcealias", "zh-CN", "数据源别名", "数据源别名（如 A、B、C，用于 JOIN 与字段引用）"),
            // entity.configurablesource.sourcealias
            new TranslationSeedItem("entity.configurablesource.sourcealias", "zh-HK", "数据源别名_hk", "数据源别名（如 A、B、C，用于 JOIN 与字段引用）"),

            // entity.configurablesource.tablename
            new TranslationSeedItem("entity.configurablesource.tablename", "en-US", "物理表名_us", "物理表名（选项 TaktDatabaseInfos/tables；DictValue=TableName；运行时 takt_ 前缀白名单校验）"),
            // entity.configurablesource.tablename
            new TranslationSeedItem("entity.configurablesource.tablename", "ja-JP", "物理表名_jp", "物理表名（选项 TaktDatabaseInfos/tables；DictValue=TableName；运行时 takt_ 前缀白名单校验）"),
            // entity.configurablesource.tablename
            new TranslationSeedItem("entity.configurablesource.tablename", "zh-CN", "物理表名", "物理表名（选项 TaktDatabaseInfos/tables；DictValue=TableName；运行时 takt_ 前缀白名单校验）"),
            // entity.configurablesource.tablename
            new TranslationSeedItem("entity.configurablesource.tablename", "zh-HK", "物理表名_hk", "物理表名（选项 TaktDatabaseInfos/tables；DictValue=TableName；运行时 takt_ 前缀白名单校验）"),

            // entity.configurablesource.isprimary
            new TranslationSeedItem("entity.configurablesource.isprimary", "en-US", "是否主表_us", "是否主表（字典 sys_yes_no；0=否 1=是；驱动 FROM 的第一张表）"),
            // entity.configurablesource.isprimary
            new TranslationSeedItem("entity.configurablesource.isprimary", "ja-JP", "是否主表_jp", "是否主表（字典 sys_yes_no；0=否 1=是；驱动 FROM 的第一张表）"),
            // entity.configurablesource.isprimary
            new TranslationSeedItem("entity.configurablesource.isprimary", "zh-CN", "是否主表", "是否主表（字典 sys_yes_no；0=否 1=是；驱动 FROM 的第一张表）"),
            // entity.configurablesource.isprimary
            new TranslationSeedItem("entity.configurablesource.isprimary", "zh-HK", "是否主表_hk", "是否主表（字典 sys_yes_no；0=否 1=是；驱动 FROM 的第一张表）"),

            // entity.configurablesource.sortorder
            new TranslationSeedItem("entity.configurablesource.sortorder", "en-US", "排序号_us", "排序号（回填）（多表 FROM 顺序）"),
            // entity.configurablesource.sortorder
            new TranslationSeedItem("entity.configurablesource.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（多表 FROM 顺序）"),
            // entity.configurablesource.sortorder
            new TranslationSeedItem("entity.configurablesource.sortorder", "zh-CN", "排序号", "排序号（回填）（多表 FROM 顺序）"),
            // entity.configurablesource.sortorder
            new TranslationSeedItem("entity.configurablesource.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（多表 FROM 顺序）"),

            // entity.configurablesource.configurable
            new TranslationSeedItem("entity.configurablesource.configurable", "en-US", "关联的定制报表主表_us", "关联的定制报表主表"),
            // entity.configurablesource.configurable
            new TranslationSeedItem("entity.configurablesource.configurable", "ja-JP", "关联的定制报表主表_jp", "关联的定制报表主表"),
            // entity.configurablesource.configurable
            new TranslationSeedItem("entity.configurablesource.configurable", "zh-CN", "关联的定制报表主表", "关联的定制报表主表"),
            // entity.configurablesource.configurable
            new TranslationSeedItem("entity.configurablesource.configurable", "zh-HK", "关联的定制报表主表_hk", "关联的定制报表主表"),
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
