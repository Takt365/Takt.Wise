// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktArchiveLogI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktArchiveLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging;

/// <summary>
/// TaktArchiveLog 实体国际化翻译种子（键前缀 entity.archivelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktArchiveLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktArchiveLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 archivelog 实体翻译...", tenantCode);

        foreach (var item in GetArchiveLogTranslations())
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

        TaktLogger.Information("TaktArchiveLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktArchiveLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.archivelog._self / entity.archivelog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetArchiveLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.archivelog._self
            new TranslationSeedItem("entity.archivelog._self", "en-US", "Archive Log Information_us", "实体名称"),
            // entity.archivelog._self
            new TranslationSeedItem("entity.archivelog._self", "ja-JP", "归档日志_jp", "实体名称"),
            // entity.archivelog._self
            new TranslationSeedItem("entity.archivelog._self", "zh-CN", "归档日志", "实体名称"),
            // entity.archivelog._self
            new TranslationSeedItem("entity.archivelog._self", "zh-HK", "归档日志_hk", "实体名称"),

            // entity.archivelog.archivekind
            new TranslationSeedItem("entity.archivelog.archivekind", "en-US", "归档种类_us", "归档种类（小写点号分段，如 table.year / file / attachment）"),
            // entity.archivelog.archivekind
            new TranslationSeedItem("entity.archivelog.archivekind", "ja-JP", "归档种类_jp", "归档种类（小写点号分段，如 table.year / file / attachment）"),
            // entity.archivelog.archivekind
            new TranslationSeedItem("entity.archivelog.archivekind", "zh-CN", "归档种类", "归档种类（小写点号分段，如 table.year / file / attachment）"),
            // entity.archivelog.archivekind
            new TranslationSeedItem("entity.archivelog.archivekind", "zh-HK", "归档种类_hk", "归档种类（小写点号分段，如 table.year / file / attachment）"),

            // entity.archivelog.sourceid
            new TranslationSeedItem("entity.archivelog.sourceid", "en-US", "来源业务键_us", "来源业务键（策略 Id、单据号等，统一字符串）"),
            // entity.archivelog.sourceid
            new TranslationSeedItem("entity.archivelog.sourceid", "ja-JP", "来源业务键_jp", "来源业务键（策略 Id、单据号等，统一字符串）"),
            // entity.archivelog.sourceid
            new TranslationSeedItem("entity.archivelog.sourceid", "zh-CN", "来源业务键", "来源业务键（策略 Id、单据号等，统一字符串）"),
            // entity.archivelog.sourceid
            new TranslationSeedItem("entity.archivelog.sourceid", "zh-HK", "来源业务键_hk", "来源业务键（策略 Id、单据号等，统一字符串）"),

            // entity.archivelog.sourcename
            new TranslationSeedItem("entity.archivelog.sourcename", "en-US", "来源名称_us", "来源名称（表名、路径、资源名等）"),
            // entity.archivelog.sourcename
            new TranslationSeedItem("entity.archivelog.sourcename", "ja-JP", "来源名称_jp", "来源名称（表名、路径、资源名等）"),
            // entity.archivelog.sourcename
            new TranslationSeedItem("entity.archivelog.sourcename", "zh-CN", "来源名称", "来源名称（表名、路径、资源名等）"),
            // entity.archivelog.sourcename
            new TranslationSeedItem("entity.archivelog.sourcename", "zh-HK", "来源名称_hk", "来源名称（表名、路径、资源名等）"),

            // entity.archivelog.targetname
            new TranslationSeedItem("entity.archivelog.targetname", "en-US", "目标名称_us", "归档目标名称（年分表名、归档路径等）"),
            // entity.archivelog.targetname
            new TranslationSeedItem("entity.archivelog.targetname", "ja-JP", "目标名称_jp", "归档目标名称（年分表名、归档路径等）"),
            // entity.archivelog.targetname
            new TranslationSeedItem("entity.archivelog.targetname", "zh-CN", "目标名称", "归档目标名称（年分表名、归档路径等）"),
            // entity.archivelog.targetname
            new TranslationSeedItem("entity.archivelog.targetname", "zh-HK", "目标名称_hk", "归档目标名称（年分表名、归档路径等）"),

            // entity.archivelog.archiveyear
            new TranslationSeedItem("entity.archivelog.archiveyear", "en-US", "归档年份_us", "归档年份（按年归档时填写；其它场景可空）"),
            // entity.archivelog.archiveyear
            new TranslationSeedItem("entity.archivelog.archiveyear", "ja-JP", "归档年份_jp", "归档年份（按年归档时填写；其它场景可空）"),
            // entity.archivelog.archiveyear
            new TranslationSeedItem("entity.archivelog.archiveyear", "zh-CN", "归档年份", "归档年份（按年归档时填写；其它场景可空）"),
            // entity.archivelog.archiveyear
            new TranslationSeedItem("entity.archivelog.archiveyear", "zh-HK", "归档年份_hk", "归档年份（按年归档时填写；其它场景可空）"),

            // entity.archivelog.sourcecount
            new TranslationSeedItem("entity.archivelog.sourcecount", "en-US", "源匹配数量_us", "归档前匹配数量（行/文件/对象）"),
            // entity.archivelog.sourcecount
            new TranslationSeedItem("entity.archivelog.sourcecount", "ja-JP", "源匹配数量_jp", "归档前匹配数量（行/文件/对象）"),
            // entity.archivelog.sourcecount
            new TranslationSeedItem("entity.archivelog.sourcecount", "zh-CN", "源匹配数量", "归档前匹配数量（行/文件/对象）"),
            // entity.archivelog.sourcecount
            new TranslationSeedItem("entity.archivelog.sourcecount", "zh-HK", "源匹配数量_hk", "归档前匹配数量（行/文件/对象）"),

            // entity.archivelog.archivedcount
            new TranslationSeedItem("entity.archivelog.archivedcount", "en-US", "归档数量_us", "实际归档数量"),
            // entity.archivelog.archivedcount
            new TranslationSeedItem("entity.archivelog.archivedcount", "ja-JP", "归档数量_jp", "实际归档数量"),
            // entity.archivelog.archivedcount
            new TranslationSeedItem("entity.archivelog.archivedcount", "zh-CN", "归档数量", "实际归档数量"),
            // entity.archivelog.archivedcount
            new TranslationSeedItem("entity.archivelog.archivedcount", "zh-HK", "归档数量_hk", "实际归档数量"),

            // entity.archivelog.deletedcount
            new TranslationSeedItem("entity.archivelog.deletedcount", "en-US", "删除数量_us", "源侧删除数量（热区清理等；无删除则为 0）"),
            // entity.archivelog.deletedcount
            new TranslationSeedItem("entity.archivelog.deletedcount", "ja-JP", "删除数量_jp", "源侧删除数量（热区清理等；无删除则为 0）"),
            // entity.archivelog.deletedcount
            new TranslationSeedItem("entity.archivelog.deletedcount", "zh-CN", "删除数量", "源侧删除数量（热区清理等；无删除则为 0）"),
            // entity.archivelog.deletedcount
            new TranslationSeedItem("entity.archivelog.deletedcount", "zh-HK", "删除数量_hk", "源侧删除数量（热区清理等；无删除则为 0）"),

            // entity.archivelog.runstatus
            new TranslationSeedItem("entity.archivelog.runstatus", "en-US", "运行状态_us", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),
            // entity.archivelog.runstatus
            new TranslationSeedItem("entity.archivelog.runstatus", "ja-JP", "运行状态_jp", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),
            // entity.archivelog.runstatus
            new TranslationSeedItem("entity.archivelog.runstatus", "zh-CN", "运行状态", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),
            // entity.archivelog.runstatus
            new TranslationSeedItem("entity.archivelog.runstatus", "zh-HK", "运行状态_hk", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),

            // entity.archivelog.errormessage
            new TranslationSeedItem("entity.archivelog.errormessage", "en-US", "错误信息_us", "失败错误信息"),
            // entity.archivelog.errormessage
            new TranslationSeedItem("entity.archivelog.errormessage", "ja-JP", "错误信息_jp", "失败错误信息"),
            // entity.archivelog.errormessage
            new TranslationSeedItem("entity.archivelog.errormessage", "zh-CN", "错误信息", "失败错误信息"),
            // entity.archivelog.errormessage
            new TranslationSeedItem("entity.archivelog.errormessage", "zh-HK", "错误信息_hk", "失败错误信息"),

            // entity.archivelog.startedat
            new TranslationSeedItem("entity.archivelog.startedat", "en-US", "开始时间_us", "开始时间"),
            // entity.archivelog.startedat
            new TranslationSeedItem("entity.archivelog.startedat", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.archivelog.startedat
            new TranslationSeedItem("entity.archivelog.startedat", "zh-CN", "开始时间", "开始时间"),
            // entity.archivelog.startedat
            new TranslationSeedItem("entity.archivelog.startedat", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.archivelog.finishedat
            new TranslationSeedItem("entity.archivelog.finishedat", "en-US", "结束时间_us", "结束时间"),
            // entity.archivelog.finishedat
            new TranslationSeedItem("entity.archivelog.finishedat", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.archivelog.finishedat
            new TranslationSeedItem("entity.archivelog.finishedat", "zh-CN", "结束时间", "结束时间"),
            // entity.archivelog.finishedat
            new TranslationSeedItem("entity.archivelog.finishedat", "zh-HK", "结束时间_hk", "结束时间"),
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
        translation.ResourceGroup = "Logging";
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
