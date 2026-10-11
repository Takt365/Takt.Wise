// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktQuartzTaskI18nSeedData.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQuartzTask 实体字段国际化种子（已对齐前端 locales：src/locales/foundation/quartz-task）
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
/// TaktQuartzTask 实体国际化翻译种子（键前缀 entity.quartztask.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQuartzTaskI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQuartzTask 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 quartztask 实体翻译...", tenantCode);

        foreach (var item in GetQuartzTaskTranslations())
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

        TaktLogger.Information("TaktQuartzTask 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQuartzTask 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.quartztask._self / entity.quartztask.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQuartzTaskTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.quartztask._self
            new TranslationSeedItem("entity.quartztask._self", "en-US", "Quartz Task Information_us", "实体名称"),
            // entity.quartztask._self
            new TranslationSeedItem("entity.quartztask._self", "ja-JP", "Quartz 定时任务信息_jp", "实体名称"),
            // entity.quartztask._self
            new TranslationSeedItem("entity.quartztask._self", "zh-CN", "Quartz 定时任务信息", "实体名称"),
            // entity.quartztask._self
            new TranslationSeedItem("entity.quartztask._self", "zh-HK", "Quartz 定时任务信息_hk", "实体名称"),

            // entity.quartztask.taskcode
            new TranslationSeedItem("entity.quartztask.taskcode", "en-US", "任务编码_us", "任务编码（租户+公司内唯一）"),
            // entity.quartztask.taskcode
            new TranslationSeedItem("entity.quartztask.taskcode", "ja-JP", "任务编码_jp", "任务编码（租户+公司内唯一）"),
            // entity.quartztask.taskcode
            new TranslationSeedItem("entity.quartztask.taskcode", "zh-CN", "任务编码", "任务编码（租户+公司内唯一）"),
            // entity.quartztask.taskcode
            new TranslationSeedItem("entity.quartztask.taskcode", "zh-HK", "任务编码_hk", "任务编码（租户+公司内唯一）"),

            // entity.quartztask.taskname
            new TranslationSeedItem("entity.quartztask.taskname", "en-US", "任务名称_us", "任务名称"),
            // entity.quartztask.taskname
            new TranslationSeedItem("entity.quartztask.taskname", "ja-JP", "任务名称_jp", "任务名称"),
            // entity.quartztask.taskname
            new TranslationSeedItem("entity.quartztask.taskname", "zh-CN", "任务名称", "任务名称"),
            // entity.quartztask.taskname
            new TranslationSeedItem("entity.quartztask.taskname", "zh-HK", "任务名称_hk", "任务名称"),

            // entity.quartztask.jobname
            new TranslationSeedItem("entity.quartztask.jobname", "en-US", "Job名称_us", "Quartz Job 名称"),
            // entity.quartztask.jobname
            new TranslationSeedItem("entity.quartztask.jobname", "ja-JP", "Job名称_jp", "Quartz Job 名称"),
            // entity.quartztask.jobname
            new TranslationSeedItem("entity.quartztask.jobname", "zh-CN", "Job名称", "Quartz Job 名称"),
            // entity.quartztask.jobname
            new TranslationSeedItem("entity.quartztask.jobname", "zh-HK", "Job名称_hk", "Quartz Job 名称"),

            // entity.quartztask.jobgroup
            new TranslationSeedItem("entity.quartztask.jobgroup", "en-US", "Job分组_us", "Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）"),
            // entity.quartztask.jobgroup
            new TranslationSeedItem("entity.quartztask.jobgroup", "ja-JP", "Job分组_jp", "Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）"),
            // entity.quartztask.jobgroup
            new TranslationSeedItem("entity.quartztask.jobgroup", "zh-CN", "Job分组", "Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）"),
            // entity.quartztask.jobgroup
            new TranslationSeedItem("entity.quartztask.jobgroup", "zh-HK", "Job分组_hk", "Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）"),

            // entity.quartztask.tasktype
            new TranslationSeedItem("entity.quartztask.tasktype", "en-US", "任务类型_us", "任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）"),
            // entity.quartztask.tasktype
            new TranslationSeedItem("entity.quartztask.tasktype", "ja-JP", "任务类型_jp", "任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）"),
            // entity.quartztask.tasktype
            new TranslationSeedItem("entity.quartztask.tasktype", "zh-CN", "任务类型", "任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）"),
            // entity.quartztask.tasktype
            new TranslationSeedItem("entity.quartztask.tasktype", "zh-HK", "任务类型_hk", "任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）"),

            // entity.quartztask.assemblyname
            new TranslationSeedItem("entity.quartztask.assemblyname", "en-US", "程序集名称_us", "程序集名称（任务类型为程序集时使用）"),
            // entity.quartztask.assemblyname
            new TranslationSeedItem("entity.quartztask.assemblyname", "ja-JP", "程序集名称_jp", "程序集名称（任务类型为程序集时使用）"),
            // entity.quartztask.assemblyname
            new TranslationSeedItem("entity.quartztask.assemblyname", "zh-CN", "程序集名称", "程序集名称（任务类型为程序集时使用）"),
            // entity.quartztask.assemblyname
            new TranslationSeedItem("entity.quartztask.assemblyname", "zh-HK", "程序集名称_hk", "程序集名称（任务类型为程序集时使用）"),

            // entity.quartztask.classname
            new TranslationSeedItem("entity.quartztask.classname", "en-US", "任务类名_us", "任务类名（任务类型为程序集时使用）"),
            // entity.quartztask.classname
            new TranslationSeedItem("entity.quartztask.classname", "ja-JP", "任务类名_jp", "任务类名（任务类型为程序集时使用）"),
            // entity.quartztask.classname
            new TranslationSeedItem("entity.quartztask.classname", "zh-CN", "任务类名", "任务类名（任务类型为程序集时使用）"),
            // entity.quartztask.classname
            new TranslationSeedItem("entity.quartztask.classname", "zh-HK", "任务类名_hk", "任务类名（任务类型为程序集时使用）"),

            // entity.quartztask.apiurl
            new TranslationSeedItem("entity.quartztask.apiurl", "en-US", "API执行地址_us", "API 执行地址（任务类型为网络请求时使用）"),
            // entity.quartztask.apiurl
            new TranslationSeedItem("entity.quartztask.apiurl", "ja-JP", "API执行地址_jp", "API 执行地址（任务类型为网络请求时使用）"),
            // entity.quartztask.apiurl
            new TranslationSeedItem("entity.quartztask.apiurl", "zh-CN", "API执行地址", "API 执行地址（任务类型为网络请求时使用）"),
            // entity.quartztask.apiurl
            new TranslationSeedItem("entity.quartztask.apiurl", "zh-HK", "API执行地址_hk", "API 执行地址（任务类型为网络请求时使用）"),

            // entity.quartztask.requestmethod
            new TranslationSeedItem("entity.quartztask.requestmethod", "en-US", "网络请求方式_us", "网络请求方式（GET/POST 等）"),
            // entity.quartztask.requestmethod
            new TranslationSeedItem("entity.quartztask.requestmethod", "ja-JP", "网络请求方式_jp", "网络请求方式（GET/POST 等）"),
            // entity.quartztask.requestmethod
            new TranslationSeedItem("entity.quartztask.requestmethod", "zh-CN", "网络请求方式", "网络请求方式（GET/POST 等）"),
            // entity.quartztask.requestmethod
            new TranslationSeedItem("entity.quartztask.requestmethod", "zh-HK", "网络请求方式_hk", "网络请求方式（GET/POST 等）"),

            // entity.quartztask.sqlscript
            new TranslationSeedItem("entity.quartztask.sqlscript", "en-US", "SQL脚本路径_us", "SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）"),
            // entity.quartztask.sqlscript
            new TranslationSeedItem("entity.quartztask.sqlscript", "ja-JP", "SQL脚本路径_jp", "SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）"),
            // entity.quartztask.sqlscript
            new TranslationSeedItem("entity.quartztask.sqlscript", "zh-CN", "SQL脚本路径", "SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）"),
            // entity.quartztask.sqlscript
            new TranslationSeedItem("entity.quartztask.sqlscript", "zh-HK", "SQL脚本路径_hk", "SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）"),

            // entity.quartztask.triggertype
            new TranslationSeedItem("entity.quartztask.triggertype", "en-US", "触发器类型_us", "触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）"),
            // entity.quartztask.triggertype
            new TranslationSeedItem("entity.quartztask.triggertype", "ja-JP", "触发器类型_jp", "触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）"),
            // entity.quartztask.triggertype
            new TranslationSeedItem("entity.quartztask.triggertype", "zh-CN", "触发器类型", "触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）"),
            // entity.quartztask.triggertype
            new TranslationSeedItem("entity.quartztask.triggertype", "zh-HK", "触发器类型_hk", "触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）"),

            // entity.quartztask.cronexpression
            new TranslationSeedItem("entity.quartztask.cronexpression", "en-US", "Cron表达式_us", "Cron 表达式（触发器类型为 Cron 时使用）"),
            // entity.quartztask.cronexpression
            new TranslationSeedItem("entity.quartztask.cronexpression", "ja-JP", "Cron表达式_jp", "Cron 表达式（触发器类型为 Cron 时使用）"),
            // entity.quartztask.cronexpression
            new TranslationSeedItem("entity.quartztask.cronexpression", "zh-CN", "Cron表达式", "Cron 表达式（触发器类型为 Cron 时使用）"),
            // entity.quartztask.cronexpression
            new TranslationSeedItem("entity.quartztask.cronexpression", "zh-HK", "Cron表达式_hk", "Cron 表达式（触发器类型为 Cron 时使用）"),

            // entity.quartztask.intervalseconds
            new TranslationSeedItem("entity.quartztask.intervalseconds", "en-US", "执行间隔时间（秒）_us", "执行间隔时间（秒，触发器类型为 Simple 时使用）"),
            // entity.quartztask.intervalseconds
            new TranslationSeedItem("entity.quartztask.intervalseconds", "ja-JP", "执行间隔时间（秒）_jp", "执行间隔时间（秒，触发器类型为 Simple 时使用）"),
            // entity.quartztask.intervalseconds
            new TranslationSeedItem("entity.quartztask.intervalseconds", "zh-CN", "执行间隔时间（秒）", "执行间隔时间（秒，触发器类型为 Simple 时使用）"),
            // entity.quartztask.intervalseconds
            new TranslationSeedItem("entity.quartztask.intervalseconds", "zh-HK", "执行间隔时间（秒）_hk", "执行间隔时间（秒，触发器类型为 Simple 时使用）"),

            // entity.quartztask.executeparams
            new TranslationSeedItem("entity.quartztask.executeparams", "en-US", "执行参数_us", "执行参数"),
            // entity.quartztask.executeparams
            new TranslationSeedItem("entity.quartztask.executeparams", "ja-JP", "执行参数_jp", "执行参数"),
            // entity.quartztask.executeparams
            new TranslationSeedItem("entity.quartztask.executeparams", "zh-CN", "执行参数", "执行参数"),
            // entity.quartztask.executeparams
            new TranslationSeedItem("entity.quartztask.executeparams", "zh-HK", "执行参数_hk", "执行参数"),

            // entity.quartztask.concurrent
            new TranslationSeedItem("entity.quartztask.concurrent", "en-US", "并发_us", "是否允许并发执行（字典 sys_yes_no；0=否 1=是）"),
            // entity.quartztask.concurrent
            new TranslationSeedItem("entity.quartztask.concurrent", "ja-JP", "并发_jp", "是否允许并发执行（字典 sys_yes_no；0=否 1=是）"),
            // entity.quartztask.concurrent
            new TranslationSeedItem("entity.quartztask.concurrent", "zh-CN", "并发", "是否允许并发执行（字典 sys_yes_no；0=否 1=是）"),
            // entity.quartztask.concurrent
            new TranslationSeedItem("entity.quartztask.concurrent", "zh-HK", "并发_hk", "是否允许并发执行（字典 sys_yes_no；0=否 1=是）"),

            // entity.quartztask.misfirepolicy
            new TranslationSeedItem("entity.quartztask.misfirepolicy", "en-US", "Misfire策略_us", "Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）"),
            // entity.quartztask.misfirepolicy
            new TranslationSeedItem("entity.quartztask.misfirepolicy", "ja-JP", "Misfire策略_jp", "Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）"),
            // entity.quartztask.misfirepolicy
            new TranslationSeedItem("entity.quartztask.misfirepolicy", "zh-CN", "Misfire策略", "Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）"),
            // entity.quartztask.misfirepolicy
            new TranslationSeedItem("entity.quartztask.misfirepolicy", "zh-HK", "Misfire策略_hk", "Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）"),

            // entity.quartztask.firstrunat
            new TranslationSeedItem("entity.quartztask.firstrunat", "en-US", "首次执行_us", "首次执行（调度生效开始时间）"),
            // entity.quartztask.firstrunat
            new TranslationSeedItem("entity.quartztask.firstrunat", "ja-JP", "首次执行_jp", "首次执行（调度生效开始时间）"),
            // entity.quartztask.firstrunat
            new TranslationSeedItem("entity.quartztask.firstrunat", "zh-CN", "首次执行", "首次执行（调度生效开始时间）"),
            // entity.quartztask.firstrunat
            new TranslationSeedItem("entity.quartztask.firstrunat", "zh-HK", "首次执行_hk", "首次执行（调度生效开始时间）"),

            // entity.quartztask.executecount
            new TranslationSeedItem("entity.quartztask.executecount", "en-US", "执行次数_us", "执行次数"),
            // entity.quartztask.executecount
            new TranslationSeedItem("entity.quartztask.executecount", "ja-JP", "执行次数_jp", "执行次数"),
            // entity.quartztask.executecount
            new TranslationSeedItem("entity.quartztask.executecount", "zh-CN", "执行次数", "执行次数"),
            // entity.quartztask.executecount
            new TranslationSeedItem("entity.quartztask.executecount", "zh-HK", "执行次数_hk", "执行次数"),

            // entity.quartztask.lastrunat
            new TranslationSeedItem("entity.quartztask.lastrunat", "en-US", "上次执行_us", "上次执行"),
            // entity.quartztask.lastrunat
            new TranslationSeedItem("entity.quartztask.lastrunat", "ja-JP", "上次执行_jp", "上次执行"),
            // entity.quartztask.lastrunat
            new TranslationSeedItem("entity.quartztask.lastrunat", "zh-CN", "上次执行", "上次执行"),
            // entity.quartztask.lastrunat
            new TranslationSeedItem("entity.quartztask.lastrunat", "zh-HK", "上次执行_hk", "上次执行"),

            // entity.quartztask.nextrunat
            new TranslationSeedItem("entity.quartztask.nextrunat", "en-US", "下次执行_us", "下次执行"),
            // entity.quartztask.nextrunat
            new TranslationSeedItem("entity.quartztask.nextrunat", "ja-JP", "下次执行_jp", "下次执行"),
            // entity.quartztask.nextrunat
            new TranslationSeedItem("entity.quartztask.nextrunat", "zh-CN", "下次执行", "下次执行"),
            // entity.quartztask.nextrunat
            new TranslationSeedItem("entity.quartztask.nextrunat", "zh-HK", "下次执行_hk", "下次执行"),

            // entity.quartztask.taskdescription
            new TranslationSeedItem("entity.quartztask.taskdescription", "en-US", "任务描述_us", "任务描述"),
            // entity.quartztask.taskdescription
            new TranslationSeedItem("entity.quartztask.taskdescription", "ja-JP", "任务描述_jp", "任务描述"),
            // entity.quartztask.taskdescription
            new TranslationSeedItem("entity.quartztask.taskdescription", "zh-CN", "任务描述", "任务描述"),
            // entity.quartztask.taskdescription
            new TranslationSeedItem("entity.quartztask.taskdescription", "zh-HK", "任务描述_hk", "任务描述"),

            // entity.quartztask.taskstatus
            new TranslationSeedItem("entity.quartztask.taskstatus", "en-US", "任务状态_us", "任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）"),
            // entity.quartztask.taskstatus
            new TranslationSeedItem("entity.quartztask.taskstatus", "ja-JP", "任务状态_jp", "任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）"),
            // entity.quartztask.taskstatus
            new TranslationSeedItem("entity.quartztask.taskstatus", "zh-CN", "任务状态", "任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）"),
            // entity.quartztask.taskstatus
            new TranslationSeedItem("entity.quartztask.taskstatus", "zh-HK", "任务状态_hk", "任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）"),

            // entity.quartztask.quartzlogs
            new TranslationSeedItem("entity.quartztask.quartzlogs", "en-US", "关联的任务执行日志列表_us", "关联的任务执行日志列表（主子表关系：QuartzTaskId）"),
            // entity.quartztask.quartzlogs
            new TranslationSeedItem("entity.quartztask.quartzlogs", "ja-JP", "关联的任务执行日志列表_jp", "关联的任务执行日志列表（主子表关系：QuartzTaskId）"),
            // entity.quartztask.quartzlogs
            new TranslationSeedItem("entity.quartztask.quartzlogs", "zh-CN", "关联的任务执行日志列表", "关联的任务执行日志列表（主子表关系：QuartzTaskId）"),
            // entity.quartztask.quartzlogs
            new TranslationSeedItem("entity.quartztask.quartzlogs", "zh-HK", "关联的任务执行日志列表_hk", "关联的任务执行日志列表（主子表关系：QuartzTaskId）"),
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
