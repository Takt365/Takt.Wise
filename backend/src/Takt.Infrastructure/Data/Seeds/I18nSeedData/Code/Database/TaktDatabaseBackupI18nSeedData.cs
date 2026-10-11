// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Database
// 文件名称：TaktDatabaseBackupI18nSeedData.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDatabaseBackup 实体字段国际化种子（已对齐前端 locales：src/locales/code/database/database-backup）
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
/// TaktDatabaseBackup 实体国际化翻译种子（键前缀 entity.databasebackup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDatabaseBackupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDatabaseBackup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 databasebackup 实体翻译...", tenantCode);

        foreach (var item in GetDatabaseBackupTranslations())
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

        TaktLogger.Information("TaktDatabaseBackup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDatabaseBackup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.databasebackup._self / entity.databasebackup.{{field}}；ResourceGroup=Database；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDatabaseBackupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.databasebackup._self
            new TranslationSeedItem("entity.databasebackup._self", "en-US", "Database Backup Information_us", "实体名称"),
            // entity.databasebackup._self
            new TranslationSeedItem("entity.databasebackup._self", "ja-JP", "数据库备份记录信息_jp", "实体名称"),
            // entity.databasebackup._self
            new TranslationSeedItem("entity.databasebackup._self", "zh-CN", "数据库备份记录信息", "实体名称"),
            // entity.databasebackup._self
            new TranslationSeedItem("entity.databasebackup._self", "zh-HK", "数据库备份记录信息_hk", "实体名称"),

            // entity.databasebackup.backupcode
            new TranslationSeedItem("entity.databasebackup.backupcode", "en-US", "备份编码_us", "备份编码（租户+公司内唯一）"),
            // entity.databasebackup.backupcode
            new TranslationSeedItem("entity.databasebackup.backupcode", "ja-JP", "备份编码_jp", "备份编码（租户+公司内唯一）"),
            // entity.databasebackup.backupcode
            new TranslationSeedItem("entity.databasebackup.backupcode", "zh-CN", "备份编码", "备份编码（租户+公司内唯一）"),
            // entity.databasebackup.backupcode
            new TranslationSeedItem("entity.databasebackup.backupcode", "zh-HK", "备份编码_hk", "备份编码（租户+公司内唯一）"),

            // entity.databasebackup.targettenantcode
            new TranslationSeedItem("entity.databasebackup.targettenantcode", "en-US", "目标租户_us", "目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）"),
            // entity.databasebackup.targettenantcode
            new TranslationSeedItem("entity.databasebackup.targettenantcode", "ja-JP", "目标租户_jp", "目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）"),
            // entity.databasebackup.targettenantcode
            new TranslationSeedItem("entity.databasebackup.targettenantcode", "zh-CN", "目标租户", "目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）"),
            // entity.databasebackup.targettenantcode
            new TranslationSeedItem("entity.databasebackup.targettenantcode", "zh-HK", "目标租户_hk", "目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）"),

            // entity.databasebackup.targetdatabasename
            new TranslationSeedItem("entity.databasebackup.targetdatabasename", "en-US", "目标数据库_us", "目标数据库展示名"),
            // entity.databasebackup.targetdatabasename
            new TranslationSeedItem("entity.databasebackup.targetdatabasename", "ja-JP", "目标数据库_jp", "目标数据库展示名"),
            // entity.databasebackup.targetdatabasename
            new TranslationSeedItem("entity.databasebackup.targetdatabasename", "zh-CN", "目标数据库", "目标数据库展示名"),
            // entity.databasebackup.targetdatabasename
            new TranslationSeedItem("entity.databasebackup.targetdatabasename", "zh-HK", "目标数据库_hk", "目标数据库展示名"),

            // entity.databasebackup.backuptype
            new TranslationSeedItem("entity.databasebackup.backuptype", "en-US", "备份类型_us", "备份类型（1=Full Sync 2=Delta Sync）"),
            // entity.databasebackup.backuptype
            new TranslationSeedItem("entity.databasebackup.backuptype", "ja-JP", "备份类型_jp", "备份类型（1=Full Sync 2=Delta Sync）"),
            // entity.databasebackup.backuptype
            new TranslationSeedItem("entity.databasebackup.backuptype", "zh-CN", "备份类型", "备份类型（1=Full Sync 2=Delta Sync）"),
            // entity.databasebackup.backuptype
            new TranslationSeedItem("entity.databasebackup.backuptype", "zh-HK", "备份类型_hk", "备份类型（1=Full Sync 2=Delta Sync）"),

            // entity.databasebackup.executemode
            new TranslationSeedItem("entity.databasebackup.executemode", "en-US", "执行方式_us", "执行方式（1=立即 2=后台）"),
            // entity.databasebackup.executemode
            new TranslationSeedItem("entity.databasebackup.executemode", "ja-JP", "执行方式_jp", "执行方式（1=立即 2=后台）"),
            // entity.databasebackup.executemode
            new TranslationSeedItem("entity.databasebackup.executemode", "zh-CN", "执行方式", "执行方式（1=立即 2=后台）"),
            // entity.databasebackup.executemode
            new TranslationSeedItem("entity.databasebackup.executemode", "zh-HK", "执行方式_hk", "执行方式（1=立即 2=后台）"),

            // entity.databasebackup.backuppath
            new TranslationSeedItem("entity.databasebackup.backuppath", "en-US", "备份目录_us", "目标备份目录"),
            // entity.databasebackup.backuppath
            new TranslationSeedItem("entity.databasebackup.backuppath", "ja-JP", "备份目录_jp", "目标备份目录"),
            // entity.databasebackup.backuppath
            new TranslationSeedItem("entity.databasebackup.backuppath", "zh-CN", "备份目录", "目标备份目录"),
            // entity.databasebackup.backuppath
            new TranslationSeedItem("entity.databasebackup.backuppath", "zh-HK", "备份目录_hk", "目标备份目录"),

            // entity.databasebackup.backuppathtype
            new TranslationSeedItem("entity.databasebackup.backuppathtype", "en-US", "路径类型_us", "1=本地(服务器) 2=文件服务器 3=FTP 4=客户端"),
            new TranslationSeedItem("entity.databasebackup.backuppathtype", "ja-JP", "路径类型_jp", "1=本地(服务器) 2=文件服务器 3=FTP 4=客户端"),
            new TranslationSeedItem("entity.databasebackup.backuppathtype", "zh-CN", "路径类型", "1=本地(服务器) 2=文件服务器 3=FTP 4=客户端"),
            new TranslationSeedItem("entity.databasebackup.backuppathtype", "zh-HK", "路径类型_hk", "1=本地(服务器) 2=文件服务器 3=FTP 4=客户端"),

            // entity.databasebackup.backuphost
            new TranslationSeedItem("entity.databasebackup.backuphost", "en-US", "服务器名称_us", "网络主机或 FTP 服务器"),
            new TranslationSeedItem("entity.databasebackup.backuphost", "ja-JP", "服务器名称_jp", "网络主机或 FTP 服务器"),
            new TranslationSeedItem("entity.databasebackup.backuphost", "zh-CN", "服务器名称", "网络主机或 FTP 服务器"),
            new TranslationSeedItem("entity.databasebackup.backuphost", "zh-HK", "服务器名称_hk", "网络主机或 FTP 服务器"),

            // entity.databasebackup.backupport
            new TranslationSeedItem("entity.databasebackup.backupport", "en-US", "端口_us", "FTP 端口"),
            new TranslationSeedItem("entity.databasebackup.backupport", "ja-JP", "端口_jp", "FTP 端口"),
            new TranslationSeedItem("entity.databasebackup.backupport", "zh-CN", "端口", "FTP 端口"),
            new TranslationSeedItem("entity.databasebackup.backupport", "zh-HK", "端口_hk", "FTP 端口"),

            // entity.databasebackup.backupUserName
            new TranslationSeedItem("entity.databasebackup.backupUserName", "en-US", "用户名_us", "网络/FTP 用户名"),
            new TranslationSeedItem("entity.databasebackup.backupUserName", "ja-JP", "用户名_jp", "网络/FTP 用户名"),
            new TranslationSeedItem("entity.databasebackup.backupUserName", "zh-CN", "用户名", "网络/FTP 用户名"),
            new TranslationSeedItem("entity.databasebackup.backupUserName", "zh-HK", "用户名_hk", "网络/FTP 用户名"),

            // entity.databasebackup.backuppassword
            new TranslationSeedItem("entity.databasebackup.backuppassword", "en-US", "密码_us", "网络/FTP 密码"),
            new TranslationSeedItem("entity.databasebackup.backuppassword", "ja-JP", "密码_jp", "网络/FTP 密码"),
            new TranslationSeedItem("entity.databasebackup.backuppassword", "zh-CN", "密码", "网络/FTP 密码"),
            new TranslationSeedItem("entity.databasebackup.backuppassword", "zh-HK", "密码_hk", "网络/FTP 密码"),

            // entity.databasebackup.backupfilename
            new TranslationSeedItem("entity.databasebackup.backupfilename", "en-US", "备份文件名_us", "备份文件名（含 .bak）"),
            new TranslationSeedItem("entity.databasebackup.backupfilename", "ja-JP", "备份文件名_jp", "备份文件名（含 .bak）"),
            new TranslationSeedItem("entity.databasebackup.backupfilename", "zh-CN", "备份文件名", "备份文件名（含 .bak）"),
            new TranslationSeedItem("entity.databasebackup.backupfilename", "zh-HK", "备份文件名_hk", "备份文件名（含 .bak）"),

            // entity.databasebackup.scheduledat
            new TranslationSeedItem("entity.databasebackup.scheduledat", "en-US", "计划执行时间_us", "计划执行时间（后台调度）"),
            new TranslationSeedItem("entity.databasebackup.scheduledat", "ja-JP", "计划执行时间_jp", "计划执行时间（后台调度）"),
            new TranslationSeedItem("entity.databasebackup.scheduledat", "zh-CN", "计划执行时间", "计划执行时间（后台调度）"),
            new TranslationSeedItem("entity.databasebackup.scheduledat", "zh-HK", "计划执行时间_hk", "计划执行时间（后台调度）"),

            // entity.databasebackup.lastrunat
            new TranslationSeedItem("entity.databasebackup.lastrunat", "en-US", "最近执行时间_us", "最近一次执行时间（摘要；明细见备份日志）"),
            new TranslationSeedItem("entity.databasebackup.lastrunat", "ja-JP", "最近执行时间_jp", "最近一次执行时间（摘要；明细见备份日志）"),
            new TranslationSeedItem("entity.databasebackup.lastrunat", "zh-CN", "最近执行时间", "最近一次执行时间（摘要；明细见备份日志）"),
            new TranslationSeedItem("entity.databasebackup.lastrunat", "zh-HK", "最近执行时间_hk", "最近一次执行时间（摘要；明细见备份日志）"),

            // entity.databasebackup.quartztaskid
            new TranslationSeedItem("entity.databasebackup.quartztaskid", "en-US", "Quartz任务Id_us", "关联 Quartz 任务主键（后台执行时）"),
            new TranslationSeedItem("entity.databasebackup.quartztaskid", "ja-JP", "Quartz任务Id_jp", "关联 Quartz 任务主键（后台执行时）"),
            new TranslationSeedItem("entity.databasebackup.quartztaskid", "zh-CN", "Quartz任务Id", "关联 Quartz 任务主键（后台执行时）"),
            new TranslationSeedItem("entity.databasebackup.quartztaskid", "zh-HK", "Quartz任务Id_hk", "关联 Quartz 任务主键（后台执行时）"),

            // entity.databasebackup.backupstatus
            new TranslationSeedItem("entity.databasebackup.backupstatus", "en-US", "备份状态_us", "备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）"),
            new TranslationSeedItem("entity.databasebackup.backupstatus", "ja-JP", "备份状态_jp", "备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）"),
            new TranslationSeedItem("entity.databasebackup.backupstatus", "zh-CN", "备份状态", "备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）"),
            new TranslationSeedItem("entity.databasebackup.backupstatus", "zh-HK", "备份状态_hk", "备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）"),
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
