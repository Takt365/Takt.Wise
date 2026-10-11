// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktTrackingLogI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTrackingLog 实体字段国际化种子（已对齐前端 locales：src/locales/statistics/logging/tracking-log）
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
/// TaktTrackingLog 实体国际化翻译种子（键前缀 entity.trackinglog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTrackingLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTrackingLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 trackinglog 实体翻译...", tenantCode);

        foreach (var item in GetTrackingLogTranslations())
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

        TaktLogger.Information("TaktTrackingLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTrackingLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.trackinglog._self / entity.trackinglog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTrackingLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.trackinglog._self
            new TranslationSeedItem("entity.trackinglog._self", "en-US", "Tracking Log Information_us", "实体名称"),
            // entity.trackinglog._self
            new TranslationSeedItem("entity.trackinglog._self", "ja-JP", "前端交互日志信息_jp", "实体名称"),
            // entity.trackinglog._self
            new TranslationSeedItem("entity.trackinglog._self", "zh-CN", "前端交互日志信息", "实体名称"),
            // entity.trackinglog._self
            new TranslationSeedItem("entity.trackinglog._self", "zh-HK", "前端交互日志信息_hk", "实体名称"),

            // entity.trackinglog.userid
            new TranslationSeedItem("entity.trackinglog.userid", "en-US", "用户ID_us", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.trackinglog.userid
            new TranslationSeedItem("entity.trackinglog.userid", "ja-JP", "用户ID_jp", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.trackinglog.userid
            new TranslationSeedItem("entity.trackinglog.userid", "zh-CN", "用户ID", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.trackinglog.userid
            new TranslationSeedItem("entity.trackinglog.userid", "zh-HK", "用户ID_hk", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.trackinglog.username
            new TranslationSeedItem("entity.trackinglog.username", "en-US", "用户名_us", "用户名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.trackinglog.username
            new TranslationSeedItem("entity.trackinglog.username", "ja-JP", "用户名_jp", "用户名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.trackinglog.username
            new TranslationSeedItem("entity.trackinglog.username", "zh-CN", "用户名", "用户名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.trackinglog.username
            new TranslationSeedItem("entity.trackinglog.username", "zh-HK", "用户名_hk", "用户名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.trackinglog.eventtrackingtype
            new TranslationSeedItem("entity.trackinglog.eventtrackingtype", "en-US", "事件类型_us", "事件类型（如 longtask）"),
            // entity.trackinglog.eventtrackingtype
            new TranslationSeedItem("entity.trackinglog.eventtrackingtype", "ja-JP", "事件类型_jp", "事件类型（如 longtask）"),
            // entity.trackinglog.eventtrackingtype
            new TranslationSeedItem("entity.trackinglog.eventtrackingtype", "zh-CN", "事件类型", "事件类型（如 longtask）"),
            // entity.trackinglog.eventtrackingtype
            new TranslationSeedItem("entity.trackinglog.eventtrackingtype", "zh-HK", "事件类型_hk", "事件类型（如 longtask）"),

            // entity.trackinglog.eventtrackingcategory
            new TranslationSeedItem("entity.trackinglog.eventtrackingcategory", "en-US", "事件分类_us", "事件分类（如 performance）"),
            // entity.trackinglog.eventtrackingcategory
            new TranslationSeedItem("entity.trackinglog.eventtrackingcategory", "ja-JP", "事件分类_jp", "事件分类（如 performance）"),
            // entity.trackinglog.eventtrackingcategory
            new TranslationSeedItem("entity.trackinglog.eventtrackingcategory", "zh-CN", "事件分类", "事件分类（如 performance）"),
            // entity.trackinglog.eventtrackingcategory
            new TranslationSeedItem("entity.trackinglog.eventtrackingcategory", "zh-HK", "事件分类_hk", "事件分类（如 performance）"),

            // entity.trackinglog.eventtime
            new TranslationSeedItem("entity.trackinglog.eventtime", "en-US", "事件发生时间_us", "事件发生时间（客户端 UTC）"),
            // entity.trackinglog.eventtime
            new TranslationSeedItem("entity.trackinglog.eventtime", "ja-JP", "事件发生时间_jp", "事件发生时间（客户端 UTC）"),
            // entity.trackinglog.eventtime
            new TranslationSeedItem("entity.trackinglog.eventtime", "zh-CN", "事件发生时间", "事件发生时间（客户端 UTC）"),
            // entity.trackinglog.eventtime
            new TranslationSeedItem("entity.trackinglog.eventtime", "zh-HK", "事件发生时间_hk", "事件发生时间（客户端 UTC）"),

            // entity.trackinglog.durationms
            new TranslationSeedItem("entity.trackinglog.durationms", "en-US", "阻塞时长毫秒_us", "长任务阻塞时长（毫秒）"),
            // entity.trackinglog.durationms
            new TranslationSeedItem("entity.trackinglog.durationms", "ja-JP", "阻塞时长毫秒_jp", "长任务阻塞时长（毫秒）"),
            // entity.trackinglog.durationms
            new TranslationSeedItem("entity.trackinglog.durationms", "zh-CN", "阻塞时长毫秒", "长任务阻塞时长（毫秒）"),
            // entity.trackinglog.durationms
            new TranslationSeedItem("entity.trackinglog.durationms", "zh-HK", "阻塞时长毫秒_hk", "长任务阻塞时长（毫秒）"),

            // entity.trackinglog.performancestartms
            new TranslationSeedItem("entity.trackinglog.performancestartms", "en-US", "Performance开始毫秒_us", "PerformanceEntry.startTime（毫秒，相对页面导航起点）"),
            // entity.trackinglog.performancestartms
            new TranslationSeedItem("entity.trackinglog.performancestartms", "ja-JP", "Performance开始毫秒_jp", "PerformanceEntry.startTime（毫秒，相对页面导航起点）"),
            // entity.trackinglog.performancestartms
            new TranslationSeedItem("entity.trackinglog.performancestartms", "zh-CN", "Performance开始毫秒", "PerformanceEntry.startTime（毫秒，相对页面导航起点）"),
            // entity.trackinglog.performancestartms
            new TranslationSeedItem("entity.trackinglog.performancestartms", "zh-HK", "Performance开始毫秒_hk", "PerformanceEntry.startTime（毫秒，相对页面导航起点）"),

            // entity.trackinglog.entryname
            new TranslationSeedItem("entity.trackinglog.entryname", "en-US", "Performance条目名_us", "PerformanceEntry.name"),
            // entity.trackinglog.entryname
            new TranslationSeedItem("entity.trackinglog.entryname", "ja-JP", "Performance条目名_jp", "PerformanceEntry.name"),
            // entity.trackinglog.entryname
            new TranslationSeedItem("entity.trackinglog.entryname", "zh-CN", "Performance条目名", "PerformanceEntry.name"),
            // entity.trackinglog.entryname
            new TranslationSeedItem("entity.trackinglog.entryname", "zh-HK", "Performance条目名_hk", "PerformanceEntry.name"),

            // entity.trackinglog.trackinglevel
            new TranslationSeedItem("entity.trackinglog.trackinglevel", "en-US", "追踪级别_us", "追踪级别（字典 sys_tracking_level；1=warn 2=error）"),
            // entity.trackinglog.trackinglevel
            new TranslationSeedItem("entity.trackinglog.trackinglevel", "ja-JP", "追踪级别_jp", "追踪级别（字典 sys_tracking_level；1=warn 2=error）"),
            // entity.trackinglog.trackinglevel
            new TranslationSeedItem("entity.trackinglog.trackinglevel", "zh-CN", "追踪级别", "追踪级别（字典 sys_tracking_level；1=warn 2=error）"),
            // entity.trackinglog.trackinglevel
            new TranslationSeedItem("entity.trackinglog.trackinglevel", "zh-HK", "追踪级别_hk", "追踪级别（字典 sys_tracking_level；1=warn 2=error）"),

            // entity.trackinglog.routepath
            new TranslationSeedItem("entity.trackinglog.routepath", "en-US", "路由路径_us", "SPA 路由路径"),
            // entity.trackinglog.routepath
            new TranslationSeedItem("entity.trackinglog.routepath", "ja-JP", "路由路径_jp", "SPA 路由路径"),
            // entity.trackinglog.routepath
            new TranslationSeedItem("entity.trackinglog.routepath", "zh-CN", "路由路径", "SPA 路由路径"),
            // entity.trackinglog.routepath
            new TranslationSeedItem("entity.trackinglog.routepath", "zh-HK", "路由路径_hk", "SPA 路由路径"),

            // entity.trackinglog.pageurl
            new TranslationSeedItem("entity.trackinglog.pageurl", "en-US", "页面URL_us", "页面完整 URL"),
            // entity.trackinglog.pageurl
            new TranslationSeedItem("entity.trackinglog.pageurl", "ja-JP", "页面URL_jp", "页面完整 URL"),
            // entity.trackinglog.pageurl
            new TranslationSeedItem("entity.trackinglog.pageurl", "zh-CN", "页面URL", "页面完整 URL"),
            // entity.trackinglog.pageurl
            new TranslationSeedItem("entity.trackinglog.pageurl", "zh-HK", "页面URL_hk", "页面完整 URL"),

            // entity.trackinglog.containertype
            new TranslationSeedItem("entity.trackinglog.containertype", "en-US", "归因容器类型_us", "TaskAttribution.containerType"),
            // entity.trackinglog.containertype
            new TranslationSeedItem("entity.trackinglog.containertype", "ja-JP", "归因容器类型_jp", "TaskAttribution.containerType"),
            // entity.trackinglog.containertype
            new TranslationSeedItem("entity.trackinglog.containertype", "zh-CN", "归因容器类型", "TaskAttribution.containerType"),
            // entity.trackinglog.containertype
            new TranslationSeedItem("entity.trackinglog.containertype", "zh-HK", "归因容器类型_hk", "TaskAttribution.containerType"),

            // entity.trackinglog.containername
            new TranslationSeedItem("entity.trackinglog.containername", "en-US", "归因容器名称_us", "TaskAttribution.containerName"),
            // entity.trackinglog.containername
            new TranslationSeedItem("entity.trackinglog.containername", "ja-JP", "归因容器名称_jp", "TaskAttribution.containerName"),
            // entity.trackinglog.containername
            new TranslationSeedItem("entity.trackinglog.containername", "zh-CN", "归因容器名称", "TaskAttribution.containerName"),
            // entity.trackinglog.containername
            new TranslationSeedItem("entity.trackinglog.containername", "zh-HK", "归因容器名称_hk", "TaskAttribution.containerName"),

            // entity.trackinglog.containersrc
            new TranslationSeedItem("entity.trackinglog.containersrc", "en-US", "归因脚本来源_us", "TaskAttribution.containerSrc"),
            // entity.trackinglog.containersrc
            new TranslationSeedItem("entity.trackinglog.containersrc", "ja-JP", "归因脚本来源_jp", "TaskAttribution.containerSrc"),
            // entity.trackinglog.containersrc
            new TranslationSeedItem("entity.trackinglog.containersrc", "zh-CN", "归因脚本来源", "TaskAttribution.containerSrc"),
            // entity.trackinglog.containersrc
            new TranslationSeedItem("entity.trackinglog.containersrc", "zh-HK", "归因脚本来源_hk", "TaskAttribution.containerSrc"),

            // entity.trackinglog.containerid
            new TranslationSeedItem("entity.trackinglog.containerid", "en-US", "归因容器ID_us", "TaskAttribution.containerId"),
            // entity.trackinglog.containerid
            new TranslationSeedItem("entity.trackinglog.containerid", "ja-JP", "归因容器ID_jp", "TaskAttribution.containerId"),
            // entity.trackinglog.containerid
            new TranslationSeedItem("entity.trackinglog.containerid", "zh-CN", "归因容器ID", "TaskAttribution.containerId"),
            // entity.trackinglog.containerid
            new TranslationSeedItem("entity.trackinglog.containerid", "zh-HK", "归因容器ID_hk", "TaskAttribution.containerId"),

            // entity.trackinglog.attributionjson
            new TranslationSeedItem("entity.trackinglog.attributionjson", "en-US", "归因JSON_us", "完整 attribution JSON 数组"),
            // entity.trackinglog.attributionjson
            new TranslationSeedItem("entity.trackinglog.attributionjson", "ja-JP", "归因JSON_jp", "完整 attribution JSON 数组"),
            // entity.trackinglog.attributionjson
            new TranslationSeedItem("entity.trackinglog.attributionjson", "zh-CN", "归因JSON", "完整 attribution JSON 数组"),
            // entity.trackinglog.attributionjson
            new TranslationSeedItem("entity.trackinglog.attributionjson", "zh-HK", "归因JSON_hk", "完整 attribution JSON 数组"),

            // entity.trackinglog.useragent
            new TranslationSeedItem("entity.trackinglog.useragent", "en-US", "用户代理_us", "用户代理（User-Agent）"),
            // entity.trackinglog.useragent
            new TranslationSeedItem("entity.trackinglog.useragent", "ja-JP", "用户代理_jp", "用户代理（User-Agent）"),
            // entity.trackinglog.useragent
            new TranslationSeedItem("entity.trackinglog.useragent", "zh-CN", "用户代理", "用户代理（User-Agent）"),
            // entity.trackinglog.useragent
            new TranslationSeedItem("entity.trackinglog.useragent", "zh-HK", "用户代理_hk", "用户代理（User-Agent）"),

            // entity.trackinglog.clientip
            new TranslationSeedItem("entity.trackinglog.clientip", "en-US", "客户端IP_us", "客户端 IP"),
            // entity.trackinglog.clientip
            new TranslationSeedItem("entity.trackinglog.clientip", "ja-JP", "客户端IP_jp", "客户端 IP"),
            // entity.trackinglog.clientip
            new TranslationSeedItem("entity.trackinglog.clientip", "zh-CN", "客户端IP", "客户端 IP"),
            // entity.trackinglog.clientip
            new TranslationSeedItem("entity.trackinglog.clientip", "zh-HK", "客户端IP_hk", "客户端 IP"),
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
