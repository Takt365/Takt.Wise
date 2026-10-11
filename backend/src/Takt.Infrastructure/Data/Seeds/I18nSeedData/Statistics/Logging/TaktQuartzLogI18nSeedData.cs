// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktQuartzLogI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQuartzLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQuartzLog 实体国际化翻译种子（键前缀 entity.quartzlog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQuartzLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQuartzLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 quartzlog 实体翻译...", tenantCode);

        foreach (var item in GetQuartzLogTranslations())
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

        TaktLogger.Information("TaktQuartzLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQuartzLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.quartzlog._self / entity.quartzlog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQuartzLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.quartzlog._self
            new TranslationSeedItem("entity.quartzlog._self", "en-US", "Quartz Log Information_us", "实体名称"),
            // entity.quartzlog._self
            new TranslationSeedItem("entity.quartzlog._self", "ja-JP", "Quartz 任务执行日志信息_jp", "实体名称"),
            // entity.quartzlog._self
            new TranslationSeedItem("entity.quartzlog._self", "zh-CN", "Quartz 任务执行日志信息", "实体名称"),
            // entity.quartzlog._self
            new TranslationSeedItem("entity.quartzlog._self", "zh-HK", "Quartz 任务执行日志信息_hk", "实体名称"),

            // entity.quartzlog.quartztaskid
            new TranslationSeedItem("entity.quartzlog.quartztaskid", "en-US", "定时任务ID_us", "关联定时任务 ID（选项 TaktQuartzTasks/options；DictValue=Id）"),
            // entity.quartzlog.quartztaskid
            new TranslationSeedItem("entity.quartzlog.quartztaskid", "ja-JP", "定时任务ID_jp", "关联定时任务 ID（选项 TaktQuartzTasks/options；DictValue=Id）"),
            // entity.quartzlog.quartztaskid
            new TranslationSeedItem("entity.quartzlog.quartztaskid", "zh-CN", "定时任务ID", "关联定时任务 ID（选项 TaktQuartzTasks/options；DictValue=Id）"),
            // entity.quartzlog.quartztaskid
            new TranslationSeedItem("entity.quartzlog.quartztaskid", "zh-HK", "定时任务ID_hk", "关联定时任务 ID（选项 TaktQuartzTasks/options；DictValue=Id）"),

            // entity.quartzlog.taskname
            new TranslationSeedItem("entity.quartzlog.taskname", "en-US", "任务名称_us", "任务名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.quartzlog.taskname
            new TranslationSeedItem("entity.quartzlog.taskname", "ja-JP", "任务名称_jp", "任务名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.quartzlog.taskname
            new TranslationSeedItem("entity.quartzlog.taskname", "zh-CN", "任务名称", "任务名称（冗余：按对应 Id 取主数据名称联动）"),
            // entity.quartzlog.taskname
            new TranslationSeedItem("entity.quartzlog.taskname", "zh-HK", "任务名称_hk", "任务名称（冗余：按对应 Id 取主数据名称联动）"),

            // entity.quartzlog.jobgroup
            new TranslationSeedItem("entity.quartzlog.jobgroup", "en-US", "任务组名_us", "任务组名（字典 sys_quartz_job_group；DictValue=编码）"),
            // entity.quartzlog.jobgroup
            new TranslationSeedItem("entity.quartzlog.jobgroup", "ja-JP", "任务组名_jp", "任务组名（字典 sys_quartz_job_group；DictValue=编码）"),
            // entity.quartzlog.jobgroup
            new TranslationSeedItem("entity.quartzlog.jobgroup", "zh-CN", "任务组名", "任务组名（字典 sys_quartz_job_group；DictValue=编码）"),
            // entity.quartzlog.jobgroup
            new TranslationSeedItem("entity.quartzlog.jobgroup", "zh-HK", "任务组名_hk", "任务组名（字典 sys_quartz_job_group；DictValue=编码）"),

            // entity.quartzlog.tasktype
            new TranslationSeedItem("entity.quartzlog.tasktype", "en-US", "任务类型_us", "任务类型（字典 sys_quartz_task_type；DictValue=assembly/http/sql）"),
            // entity.quartzlog.tasktype
            new TranslationSeedItem("entity.quartzlog.tasktype", "ja-JP", "任务类型_jp", "任务类型（字典 sys_quartz_task_type；DictValue=assembly/http/sql）"),
            // entity.quartzlog.tasktype
            new TranslationSeedItem("entity.quartzlog.tasktype", "zh-CN", "任务类型", "任务类型（字典 sys_quartz_task_type；DictValue=assembly/http/sql）"),
            // entity.quartzlog.tasktype
            new TranslationSeedItem("entity.quartzlog.tasktype", "zh-HK", "任务类型_hk", "任务类型（字典 sys_quartz_task_type；DictValue=assembly/http/sql）"),

            // entity.quartzlog.executetime
            new TranslationSeedItem("entity.quartzlog.executetime", "en-US", "执行时间_us", "执行时间"),
            // entity.quartzlog.executetime
            new TranslationSeedItem("entity.quartzlog.executetime", "ja-JP", "执行时间_jp", "执行时间"),
            // entity.quartzlog.executetime
            new TranslationSeedItem("entity.quartzlog.executetime", "zh-CN", "执行时间", "执行时间"),
            // entity.quartzlog.executetime
            new TranslationSeedItem("entity.quartzlog.executetime", "zh-HK", "执行时间_hk", "执行时间"),

            // entity.quartzlog.executeduration
            new TranslationSeedItem("entity.quartzlog.executeduration", "en-US", "执行耗时（毫秒）_us", "执行耗时（毫秒）"),
            // entity.quartzlog.executeduration
            new TranslationSeedItem("entity.quartzlog.executeduration", "ja-JP", "执行耗时（毫秒）_jp", "执行耗时（毫秒）"),
            // entity.quartzlog.executeduration
            new TranslationSeedItem("entity.quartzlog.executeduration", "zh-CN", "执行耗时（毫秒）", "执行耗时（毫秒）"),
            // entity.quartzlog.executeduration
            new TranslationSeedItem("entity.quartzlog.executeduration", "zh-HK", "执行耗时（毫秒）_hk", "执行耗时（毫秒）"),

            // entity.quartzlog.executeparams
            new TranslationSeedItem("entity.quartzlog.executeparams", "en-US", "执行参数_us", "执行参数（无参数为空串）"),
            // entity.quartzlog.executeparams
            new TranslationSeedItem("entity.quartzlog.executeparams", "ja-JP", "执行参数_jp", "执行参数（无参数为空串）"),
            // entity.quartzlog.executeparams
            new TranslationSeedItem("entity.quartzlog.executeparams", "zh-CN", "执行参数", "执行参数（无参数为空串）"),
            // entity.quartzlog.executeparams
            new TranslationSeedItem("entity.quartzlog.executeparams", "zh-HK", "执行参数_hk", "执行参数（无参数为空串）"),

            // entity.quartzlog.executemessage
            new TranslationSeedItem("entity.quartzlog.executemessage", "en-US", "执行消息_us", "执行消息（无消息为空串）"),
            // entity.quartzlog.executemessage
            new TranslationSeedItem("entity.quartzlog.executemessage", "ja-JP", "执行消息_jp", "执行消息（无消息为空串）"),
            // entity.quartzlog.executemessage
            new TranslationSeedItem("entity.quartzlog.executemessage", "zh-CN", "执行消息", "执行消息（无消息为空串）"),
            // entity.quartzlog.executemessage
            new TranslationSeedItem("entity.quartzlog.executemessage", "zh-HK", "执行消息_hk", "执行消息（无消息为空串）"),

            // entity.quartzlog.errorinfo
            new TranslationSeedItem("entity.quartzlog.errorinfo", "en-US", "错误信息_us", "错误信息（成功为空串）"),
            // entity.quartzlog.errorinfo
            new TranslationSeedItem("entity.quartzlog.errorinfo", "ja-JP", "错误信息_jp", "错误信息（成功为空串）"),
            // entity.quartzlog.errorinfo
            new TranslationSeedItem("entity.quartzlog.errorinfo", "zh-CN", "错误信息", "错误信息（成功为空串）"),
            // entity.quartzlog.errorinfo
            new TranslationSeedItem("entity.quartzlog.errorinfo", "zh-HK", "错误信息_hk", "错误信息（成功为空串）"),

            // entity.quartzlog.executeip
            new TranslationSeedItem("entity.quartzlog.executeip", "en-US", "执行机器IP_us", "执行机器 IP"),
            // entity.quartzlog.executeip
            new TranslationSeedItem("entity.quartzlog.executeip", "ja-JP", "执行机器IP_jp", "执行机器 IP"),
            // entity.quartzlog.executeip
            new TranslationSeedItem("entity.quartzlog.executeip", "zh-CN", "执行机器IP", "执行机器 IP"),
            // entity.quartzlog.executeip
            new TranslationSeedItem("entity.quartzlog.executeip", "zh-HK", "执行机器IP_hk", "执行机器 IP"),

            // entity.quartzlog.executehost
            new TranslationSeedItem("entity.quartzlog.executehost", "en-US", "执行机器名_us", "执行机器名"),
            // entity.quartzlog.executehost
            new TranslationSeedItem("entity.quartzlog.executehost", "ja-JP", "执行机器名_jp", "执行机器名"),
            // entity.quartzlog.executehost
            new TranslationSeedItem("entity.quartzlog.executehost", "zh-CN", "执行机器名", "执行机器名"),
            // entity.quartzlog.executehost
            new TranslationSeedItem("entity.quartzlog.executehost", "zh-HK", "执行机器名_hk", "执行机器名"),

            // entity.quartzlog.executestatus
            new TranslationSeedItem("entity.quartzlog.executestatus", "en-US", "执行状态_us", "执行状态（字典 sys_success_fail_status；0=失败 1=成功）"),
            // entity.quartzlog.executestatus
            new TranslationSeedItem("entity.quartzlog.executestatus", "ja-JP", "执行状态_jp", "执行状态（字典 sys_success_fail_status；0=失败 1=成功）"),
            // entity.quartzlog.executestatus
            new TranslationSeedItem("entity.quartzlog.executestatus", "zh-CN", "执行状态", "执行状态（字典 sys_success_fail_status；0=失败 1=成功）"),
            // entity.quartzlog.executestatus
            new TranslationSeedItem("entity.quartzlog.executestatus", "zh-HK", "执行状态_hk", "执行状态（字典 sys_success_fail_status；0=失败 1=成功）"),

            // entity.quartzlog.quartztask
            new TranslationSeedItem("entity.quartzlog.quartztask", "en-US", "关联的定时任务_us", "关联的定时任务"),
            // entity.quartzlog.quartztask
            new TranslationSeedItem("entity.quartzlog.quartztask", "ja-JP", "关联的定时任务_jp", "关联的定时任务"),
            // entity.quartzlog.quartztask
            new TranslationSeedItem("entity.quartzlog.quartztask", "zh-CN", "关联的定时任务", "关联的定时任务"),
            // entity.quartzlog.quartztask
            new TranslationSeedItem("entity.quartzlog.quartztask", "zh-HK", "关联的定时任务_hk", "关联的定时任务"),
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
