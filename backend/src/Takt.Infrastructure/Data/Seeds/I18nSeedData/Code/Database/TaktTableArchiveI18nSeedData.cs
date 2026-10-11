// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Database
// 文件名称：TaktTableArchiveI18nSeedData.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTableArchive 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Database;

/// <summary>
/// TaktTableArchive 实体国际化翻译种子（键前缀 entity.tablearchive.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTableArchiveI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTableArchive 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 tablearchive 实体翻译...", tenantCode);

        foreach (var item in GetTableArchiveTranslations())
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

        TaktLogger.Information("TaktTableArchive 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTableArchive 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.tablearchive._self / entity.tablearchive.{{field}}；ResourceGroup=Database；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTableArchiveTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.tablearchive._self
            new TranslationSeedItem("entity.tablearchive._self", "en-US", "Table Archive", "实体名称"),
            // entity.tablearchive._self
            new TranslationSeedItem("entity.tablearchive._self", "ja-JP", "テーブルアーカイブ", "实体名称"),
            // entity.tablearchive._self
            new TranslationSeedItem("entity.tablearchive._self", "zh-CN", "数据表归档", "实体名称"),
            // entity.tablearchive._self
            new TranslationSeedItem("entity.tablearchive._self", "zh-HK", "資料表歸檔", "实体名称"),

            // entity.tablearchive.targettenantcode
            new TranslationSeedItem("entity.tablearchive.targettenantcode", "en-US", "目标租户_us", "目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）"),
            // entity.tablearchive.targettenantcode
            new TranslationSeedItem("entity.tablearchive.targettenantcode", "ja-JP", "目标租户_jp", "目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）"),
            // entity.tablearchive.targettenantcode
            new TranslationSeedItem("entity.tablearchive.targettenantcode", "zh-CN", "目标租户", "目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）"),
            // entity.tablearchive.targettenantcode
            new TranslationSeedItem("entity.tablearchive.targettenantcode", "zh-HK", "目标租户_hk", "目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）"),

            // entity.tablearchive.targetdatabasename
            new TranslationSeedItem("entity.tablearchive.targetdatabasename", "en-US", "目标数据库_us", "目标数据库展示名（与 DatabaseInfos DisplayName 一致）"),
            // entity.tablearchive.targetdatabasename
            new TranslationSeedItem("entity.tablearchive.targetdatabasename", "ja-JP", "目标数据库_jp", "目标数据库展示名（与 DatabaseInfos DisplayName 一致）"),
            // entity.tablearchive.targetdatabasename
            new TranslationSeedItem("entity.tablearchive.targetdatabasename", "zh-CN", "目标数据库", "目标数据库展示名（与 DatabaseInfos DisplayName 一致）"),
            // entity.tablearchive.targetdatabasename
            new TranslationSeedItem("entity.tablearchive.targetdatabasename", "zh-HK", "目标数据库_hk", "目标数据库展示名（与 DatabaseInfos DisplayName 一致）"),

            // entity.tablearchive.tablename
            new TranslationSeedItem("entity.tablearchive.tablename", "en-US", "物理表名_us", "物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）"),
            // entity.tablearchive.tablename
            new TranslationSeedItem("entity.tablearchive.tablename", "ja-JP", "物理表名_jp", "物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）"),
            // entity.tablearchive.tablename
            new TranslationSeedItem("entity.tablearchive.tablename", "zh-CN", "物理表名", "物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）"),
            // entity.tablearchive.tablename
            new TranslationSeedItem("entity.tablearchive.tablename", "zh-HK", "物理表名_hk", "物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）"),

            // entity.tablearchive.archivekeycolumn
            new TranslationSeedItem("entity.tablearchive.archivekeycolumn", "en-US", "归档键列_us", "归档键列名（如 costing_date；小写蛇形，与物理列一致）"),
            // entity.tablearchive.archivekeycolumn
            new TranslationSeedItem("entity.tablearchive.archivekeycolumn", "ja-JP", "归档键列_jp", "归档键列名（如 costing_date；小写蛇形，与物理列一致）"),
            // entity.tablearchive.archivekeycolumn
            new TranslationSeedItem("entity.tablearchive.archivekeycolumn", "zh-CN", "归档键列", "归档键列名（如 costing_date；小写蛇形，与物理列一致）"),
            // entity.tablearchive.archivekeycolumn
            new TranslationSeedItem("entity.tablearchive.archivekeycolumn", "zh-HK", "归档键列_hk", "归档键列名（如 costing_date；小写蛇形，与物理列一致）"),

            // entity.tablearchive.archivekeykind
            new TranslationSeedItem("entity.tablearchive.archivekeykind", "en-US", "Archive Key Kind", "Archive key kind (dict sys_archive_key_kind; yyyyMMddHHmmss/yyyyMM/yyyy etc.)"),
            // entity.tablearchive.archivekeykind
            new TranslationSeedItem("entity.tablearchive.archivekeykind", "ja-JP", "アーカイブキータイプ", "アーカイブキータイプ（辞書 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）"),
            // entity.tablearchive.archivekeykind
            new TranslationSeedItem("entity.tablearchive.archivekeykind", "zh-CN", "归档键类型", "归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）"),
            // entity.tablearchive.archivekeykind
            new TranslationSeedItem("entity.tablearchive.archivekeykind", "zh-HK", "歸檔鍵類型", "歸檔鍵類型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）"),

            // entity.tablearchive.retainhotyears
            new TranslationSeedItem("entity.tablearchive.retainhotyears", "en-US", "Hot Retain Years", "Fixed to 1; only years ≤ currentYear−1 may be archived (e.g. in 2026 only ≤2025)"),
            // entity.tablearchive.retainhotyears
            new TranslationSeedItem("entity.tablearchive.retainhotyears", "ja-JP", "ホット保持年数", "固定 1；当年−1 以前のみアーカイブ可（例：2026年なら ≤2025）"),
            // entity.tablearchive.retainhotyears
            new TranslationSeedItem("entity.tablearchive.retainhotyears", "zh-CN", "热库保留年数", "固定为 1；仅允许归档当前年减 1 及更早（例：2026 只能归档≤2025）"),
            // entity.tablearchive.retainhotyears
            new TranslationSeedItem("entity.tablearchive.retainhotyears", "zh-HK", "熱庫保留年數", "固定為 1；僅允許歸檔當年減 1 及更早（例：2026 只能歸檔≤2025）"),

            // entity.tablearchive.archivename
            new TranslationSeedItem("entity.tablearchive.archivename", "en-US", "Archive Name", "Auto: {table}_{yyyyMMddHHmmss|yyyyMM|yyyy}"),
            // entity.tablearchive.archivename
            new TranslationSeedItem("entity.tablearchive.archivename", "ja-JP", "アーカイブ名", "自動：{表}_{yyyyMMddHHmmss|yyyyMM|yyyy}"),
            // entity.tablearchive.archivename
            new TranslationSeedItem("entity.tablearchive.archivename", "zh-CN", "归档名称", "自动生成：物理表名_格式码（如 takt_xxx_yyyy）"),
            // entity.tablearchive.archivename
            new TranslationSeedItem("entity.tablearchive.archivename", "zh-HK", "歸檔名稱", "自動產生：物理表名_格式碼（如 takt_xxx_yyyy）"),

            // entity.tablearchive.sortorder
            new TranslationSeedItem("entity.tablearchive.sortorder", "en-US", "排序号_us", "排序号"),
            // entity.tablearchive.sortorder
            new TranslationSeedItem("entity.tablearchive.sortorder", "ja-JP", "排序号_jp", "排序号"),
            // entity.tablearchive.sortorder
            new TranslationSeedItem("entity.tablearchive.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.tablearchive.sortorder
            new TranslationSeedItem("entity.tablearchive.sortorder", "zh-HK", "排序号_hk", "排序号"),

            // entity.tablearchive.archivestatus
            new TranslationSeedItem("entity.tablearchive.archivestatus", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.tablearchive.archivestatus
            new TranslationSeedItem("entity.tablearchive.archivestatus", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.tablearchive.archivestatus
            new TranslationSeedItem("entity.tablearchive.archivestatus", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
            // entity.tablearchive.archivestatus
            new TranslationSeedItem("entity.tablearchive.archivestatus", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用 0=禁用）"),
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
        translation.ResourceGroup = "Database";
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
