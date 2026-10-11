// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktDictTypeI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDictType 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktDictType 实体国际化翻译种子（键前缀 entity.dicttype.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDictTypeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDictType 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dicttype 实体翻译...", tenantCode);

        foreach (var item in GetDictTypeTranslations())
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

        TaktLogger.Information("TaktDictType 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDictType 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.dicttype._self / entity.dicttype.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDictTypeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.dicttype._self
            new TranslationSeedItem("entity.dicttype._self", "en-US", "Dict Type Information_us", "实体名称"),
            // entity.dicttype._self
            new TranslationSeedItem("entity.dicttype._self", "ja-JP", "字典类型信息_jp", "实体名称"),
            // entity.dicttype._self
            new TranslationSeedItem("entity.dicttype._self", "zh-CN", "字典类型信息", "实体名称"),
            // entity.dicttype._self
            new TranslationSeedItem("entity.dicttype._self", "zh-HK", "字典类型信息_hk", "实体名称"),

            // entity.dicttype.code
            new TranslationSeedItem("entity.dicttype.code", "en-US", "字典类型编码_us", "字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）"),
            // entity.dicttype.code
            new TranslationSeedItem("entity.dicttype.code", "ja-JP", "字典类型编码_jp", "字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）"),
            // entity.dicttype.code
            new TranslationSeedItem("entity.dicttype.code", "zh-CN", "字典类型编码", "字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）"),
            // entity.dicttype.code
            new TranslationSeedItem("entity.dicttype.code", "zh-HK", "字典类型编码_hk", "字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）"),

            // entity.dicttype.name
            new TranslationSeedItem("entity.dicttype.name", "en-US", "字典类型名称_us", "字典类型名称（如：订单状态、用户类型）"),
            // entity.dicttype.name
            new TranslationSeedItem("entity.dicttype.name", "ja-JP", "字典类型名称_jp", "字典类型名称（如：订单状态、用户类型）"),
            // entity.dicttype.name
            new TranslationSeedItem("entity.dicttype.name", "zh-CN", "字典类型名称", "字典类型名称（如：订单状态、用户类型）"),
            // entity.dicttype.name
            new TranslationSeedItem("entity.dicttype.name", "zh-HK", "字典类型名称_hk", "字典类型名称（如：订单状态、用户类型）"),

            // entity.dicttype.datasource
            new TranslationSeedItem("entity.dicttype.datasource", "en-US", "数据源_us", "数据源（字典 sys_data_source；0=系统表 1=SQL查询）"),
            // entity.dicttype.datasource
            new TranslationSeedItem("entity.dicttype.datasource", "ja-JP", "数据源_jp", "数据源（字典 sys_data_source；0=系统表 1=SQL查询）"),
            // entity.dicttype.datasource
            new TranslationSeedItem("entity.dicttype.datasource", "zh-CN", "数据源", "数据源（字典 sys_data_source；0=系统表 1=SQL查询）"),
            // entity.dicttype.datasource
            new TranslationSeedItem("entity.dicttype.datasource", "zh-HK", "数据源_hk", "数据源（字典 sys_data_source；0=系统表 1=SQL查询）"),

            // entity.dicttype.dictscript
            new TranslationSeedItem("entity.dicttype.dictscript", "en-US", "SQL脚本_us", "SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),
            // entity.dicttype.dictscript
            new TranslationSeedItem("entity.dicttype.dictscript", "ja-JP", "SQL脚本_jp", "SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),
            // entity.dicttype.dictscript
            new TranslationSeedItem("entity.dicttype.dictscript", "zh-CN", "SQL脚本", "SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),
            // entity.dicttype.dictscript
            new TranslationSeedItem("entity.dicttype.dictscript", "zh-HK", "SQL脚本_hk", "SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),

            // entity.dicttype.isbuiltin
            new TranslationSeedItem("entity.dicttype.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.dicttype.isbuiltin
            new TranslationSeedItem("entity.dicttype.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.dicttype.isbuiltin
            new TranslationSeedItem("entity.dicttype.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；0=否 1=是）"),
            // entity.dicttype.isbuiltin
            new TranslationSeedItem("entity.dicttype.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；0=否 1=是）"),

            // entity.dicttype.sortorder
            new TranslationSeedItem("entity.dicttype.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.dicttype.sortorder
            new TranslationSeedItem("entity.dicttype.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.dicttype.sortorder
            new TranslationSeedItem("entity.dicttype.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.dicttype.sortorder
            new TranslationSeedItem("entity.dicttype.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.dicttype.dictstatus
            new TranslationSeedItem("entity.dicttype.dictstatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.dicttype.dictstatus
            new TranslationSeedItem("entity.dicttype.dictstatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.dicttype.dictstatus
            new TranslationSeedItem("entity.dicttype.dictstatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.dicttype.dictstatus
            new TranslationSeedItem("entity.dicttype.dictstatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),

            // entity.dicttype.dictdatalist
            new TranslationSeedItem("entity.dicttype.dictdatalist", "en-US", "字典数据列表_us", "字典数据列表（一对多关联）"),
            // entity.dicttype.dictdatalist
            new TranslationSeedItem("entity.dicttype.dictdatalist", "ja-JP", "字典数据列表_jp", "字典数据列表（一对多关联）"),
            // entity.dicttype.dictdatalist
            new TranslationSeedItem("entity.dicttype.dictdatalist", "zh-CN", "字典数据列表", "字典数据列表（一对多关联）"),
            // entity.dicttype.dictdatalist
            new TranslationSeedItem("entity.dicttype.dictdatalist", "zh-HK", "字典数据列表_hk", "字典数据列表（一对多关联）"),
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
        translation.ResourceGroup = "Foundation";
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
