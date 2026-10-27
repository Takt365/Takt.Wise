// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktBackupLogI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBackupLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBackupLog 实体国际化翻译种子（键前缀 entity.backuplog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBackupLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBackupLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 backuplog 实体翻译...", tenantCode);

        foreach (var item in GetBackupLogTranslations())
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

        TaktLogger.Information("TaktBackupLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBackupLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.backuplog._self / entity.backuplog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBackupLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.backuplog._self
            new TranslationSeedItem("entity.backuplog._self", "en-US", "Backup Log Information_us", "实体名称"),
            // entity.backuplog._self
            new TranslationSeedItem("entity.backuplog._self", "ja-JP", "备份日志_jp", "实体名称"),
            // entity.backuplog._self
            new TranslationSeedItem("entity.backuplog._self", "zh-CN", "备份日志", "实体名称"),
            // entity.backuplog._self
            new TranslationSeedItem("entity.backuplog._self", "zh-HK", "备份日志_hk", "实体名称"),

            // entity.backuplog.backupkind
            new TranslationSeedItem("entity.backuplog.backupkind", "en-US", "备份种类_us", "备份种类（小写，如 database / file / config）"),
            // entity.backuplog.backupkind
            new TranslationSeedItem("entity.backuplog.backupkind", "ja-JP", "备份种类_jp", "备份种类（小写，如 database / file / config）"),
            // entity.backuplog.backupkind
            new TranslationSeedItem("entity.backuplog.backupkind", "zh-CN", "备份种类", "备份种类（小写，如 database / file / config）"),
            // entity.backuplog.backupkind
            new TranslationSeedItem("entity.backuplog.backupkind", "zh-HK", "备份种类_hk", "备份种类（小写，如 database / file / config）"),

            // entity.backuplog.sourceid
            new TranslationSeedItem("entity.backuplog.sourceid", "en-US", "来源业务键_us", "来源业务键（备份配置 Id、任务号等，统一字符串）"),
            // entity.backuplog.sourceid
            new TranslationSeedItem("entity.backuplog.sourceid", "ja-JP", "来源业务键_jp", "来源业务键（备份配置 Id、任务号等，统一字符串）"),
            // entity.backuplog.sourceid
            new TranslationSeedItem("entity.backuplog.sourceid", "zh-CN", "来源业务键", "来源业务键（备份配置 Id、任务号等，统一字符串）"),
            // entity.backuplog.sourceid
            new TranslationSeedItem("entity.backuplog.sourceid", "zh-HK", "来源业务键_hk", "来源业务键（备份配置 Id、任务号等，统一字符串）"),

            // entity.backuplog.sourcecode
            new TranslationSeedItem("entity.backuplog.sourcecode", "en-US", "来源编码_us", "来源编码快照（冗余：按对应 Id 取主数据名称联动）"),
            // entity.backuplog.sourcecode
            new TranslationSeedItem("entity.backuplog.sourcecode", "ja-JP", "来源编码_jp", "来源编码快照（冗余：按对应 Id 取主数据名称联动）"),
            // entity.backuplog.sourcecode
            new TranslationSeedItem("entity.backuplog.sourcecode", "zh-CN", "来源编码", "来源编码快照（冗余：按对应 Id 取主数据名称联动）"),
            // entity.backuplog.sourcecode
            new TranslationSeedItem("entity.backuplog.sourcecode", "zh-HK", "来源编码_hk", "来源编码快照（冗余：按对应 Id 取主数据名称联动）"),

            // entity.backuplog.targetname
            new TranslationSeedItem("entity.backuplog.targetname", "en-US", "目标名称_us", "目标名称（库展示名、目标标签等）"),
            // entity.backuplog.targetname
            new TranslationSeedItem("entity.backuplog.targetname", "ja-JP", "目标名称_jp", "目标名称（库展示名、目标标签等）"),
            // entity.backuplog.targetname
            new TranslationSeedItem("entity.backuplog.targetname", "zh-CN", "目标名称", "目标名称（库展示名、目标标签等）"),
            // entity.backuplog.targetname
            new TranslationSeedItem("entity.backuplog.targetname", "zh-HK", "目标名称_hk", "目标名称（库展示名、目标标签等）"),

            // entity.backuplog.targetscope
            new TranslationSeedItem("entity.backuplog.targetscope", "en-US", "目标范围_us", "目标范围（可选；如租户码、公司码、路径根等）"),
            // entity.backuplog.targetscope
            new TranslationSeedItem("entity.backuplog.targetscope", "ja-JP", "目标范围_jp", "目标范围（可选；如租户码、公司码、路径根等）"),
            // entity.backuplog.targetscope
            new TranslationSeedItem("entity.backuplog.targetscope", "zh-CN", "目标范围", "目标范围（可选；如租户码、公司码、路径根等）"),
            // entity.backuplog.targetscope
            new TranslationSeedItem("entity.backuplog.targetscope", "zh-HK", "目标范围_hk", "目标范围（可选；如租户码、公司码、路径根等）"),

            // entity.backuplog.syncmode
            new TranslationSeedItem("entity.backuplog.syncmode", "en-US", "同步模式_us", "同步模式快照（字典 sys_backup_sync_mode；1=完整 2=增量）"),
            // entity.backuplog.syncmode
            new TranslationSeedItem("entity.backuplog.syncmode", "ja-JP", "同步模式_jp", "同步模式快照（字典 sys_backup_sync_mode；1=完整 2=增量）"),
            // entity.backuplog.syncmode
            new TranslationSeedItem("entity.backuplog.syncmode", "zh-CN", "同步模式", "同步模式快照（字典 sys_backup_sync_mode；1=完整 2=增量）"),
            // entity.backuplog.syncmode
            new TranslationSeedItem("entity.backuplog.syncmode", "zh-HK", "同步模式_hk", "同步模式快照（字典 sys_backup_sync_mode；1=完整 2=增量）"),

            // entity.backuplog.executemode
            new TranslationSeedItem("entity.backuplog.executemode", "en-US", "执行方式_us", "执行方式快照（字典 sys_backup_execute_mode；1=立即 2=后台）"),
            // entity.backuplog.executemode
            new TranslationSeedItem("entity.backuplog.executemode", "ja-JP", "执行方式_jp", "执行方式快照（字典 sys_backup_execute_mode；1=立即 2=后台）"),
            // entity.backuplog.executemode
            new TranslationSeedItem("entity.backuplog.executemode", "zh-CN", "执行方式", "执行方式快照（字典 sys_backup_execute_mode；1=立即 2=后台）"),
            // entity.backuplog.executemode
            new TranslationSeedItem("entity.backuplog.executemode", "zh-HK", "执行方式_hk", "执行方式快照（字典 sys_backup_execute_mode；1=立即 2=后台）"),

            // entity.backuplog.pathtype
            new TranslationSeedItem("entity.backuplog.pathtype", "en-US", "路径类型_us", "路径类型快照（字典 sys_backup_path_type；0=无 1=本地 2=网络 3=FTP）"),
            // entity.backuplog.pathtype
            new TranslationSeedItem("entity.backuplog.pathtype", "ja-JP", "路径类型_jp", "路径类型快照（字典 sys_backup_path_type；0=无 1=本地 2=网络 3=FTP）"),
            // entity.backuplog.pathtype
            new TranslationSeedItem("entity.backuplog.pathtype", "zh-CN", "路径类型", "路径类型快照（字典 sys_backup_path_type；0=无 1=本地 2=网络 3=FTP）"),
            // entity.backuplog.pathtype
            new TranslationSeedItem("entity.backuplog.pathtype", "zh-HK", "路径类型_hk", "路径类型快照（字典 sys_backup_path_type；0=无 1=本地 2=网络 3=FTP）"),

            // entity.backuplog.resultpath
            new TranslationSeedItem("entity.backuplog.resultpath", "en-US", "结果路径_us", "执行后结果路径"),
            // entity.backuplog.resultpath
            new TranslationSeedItem("entity.backuplog.resultpath", "ja-JP", "结果路径_jp", "执行后结果路径"),
            // entity.backuplog.resultpath
            new TranslationSeedItem("entity.backuplog.resultpath", "zh-CN", "结果路径", "执行后结果路径"),
            // entity.backuplog.resultpath
            new TranslationSeedItem("entity.backuplog.resultpath", "zh-HK", "结果路径_hk", "执行后结果路径"),

            // entity.backuplog.filesizebytes
            new TranslationSeedItem("entity.backuplog.filesizebytes", "en-US", "文件大小字节_us", "结果大小（字节）"),
            // entity.backuplog.filesizebytes
            new TranslationSeedItem("entity.backuplog.filesizebytes", "ja-JP", "文件大小字节_jp", "结果大小（字节）"),
            // entity.backuplog.filesizebytes
            new TranslationSeedItem("entity.backuplog.filesizebytes", "zh-CN", "文件大小字节", "结果大小（字节）"),
            // entity.backuplog.filesizebytes
            new TranslationSeedItem("entity.backuplog.filesizebytes", "zh-HK", "文件大小字节_hk", "结果大小（字节）"),

            // entity.backuplog.runstatus
            new TranslationSeedItem("entity.backuplog.runstatus", "en-US", "运行状态_us", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),
            // entity.backuplog.runstatus
            new TranslationSeedItem("entity.backuplog.runstatus", "ja-JP", "运行状态_jp", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),
            // entity.backuplog.runstatus
            new TranslationSeedItem("entity.backuplog.runstatus", "zh-CN", "运行状态", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),
            // entity.backuplog.runstatus
            new TranslationSeedItem("entity.backuplog.runstatus", "zh-HK", "运行状态_hk", "运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）"),

            // entity.backuplog.errormessage
            new TranslationSeedItem("entity.backuplog.errormessage", "en-US", "错误信息_us", "失败错误信息"),
            // entity.backuplog.errormessage
            new TranslationSeedItem("entity.backuplog.errormessage", "ja-JP", "错误信息_jp", "失败错误信息"),
            // entity.backuplog.errormessage
            new TranslationSeedItem("entity.backuplog.errormessage", "zh-CN", "错误信息", "失败错误信息"),
            // entity.backuplog.errormessage
            new TranslationSeedItem("entity.backuplog.errormessage", "zh-HK", "错误信息_hk", "失败错误信息"),

            // entity.backuplog.startedat
            new TranslationSeedItem("entity.backuplog.startedat", "en-US", "开始时间_us", "开始时间"),
            // entity.backuplog.startedat
            new TranslationSeedItem("entity.backuplog.startedat", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.backuplog.startedat
            new TranslationSeedItem("entity.backuplog.startedat", "zh-CN", "开始时间", "开始时间"),
            // entity.backuplog.startedat
            new TranslationSeedItem("entity.backuplog.startedat", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.backuplog.finishedat
            new TranslationSeedItem("entity.backuplog.finishedat", "en-US", "结束时间_us", "结束时间"),
            // entity.backuplog.finishedat
            new TranslationSeedItem("entity.backuplog.finishedat", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.backuplog.finishedat
            new TranslationSeedItem("entity.backuplog.finishedat", "zh-CN", "结束时间", "结束时间"),
            // entity.backuplog.finishedat
            new TranslationSeedItem("entity.backuplog.finishedat", "zh-HK", "结束时间_hk", "结束时间"),
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
