// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOperationI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsOperation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps;

/// <summary>
/// TaktApsOperation 实体国际化翻译种子（键前缀 entity.apsoperation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsOperationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsOperation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsoperation 实体翻译...", tenantCode);

        foreach (var item in GetApsOperationTranslations())
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

        TaktLogger.Information("TaktApsOperation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsOperation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsoperation._self / entity.apsoperation.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsOperationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsoperation._self
            new TranslationSeedItem("entity.apsoperation._self", "en-US", "Aps Operation Information_us", "实体名称"),
            // entity.apsoperation._self
            new TranslationSeedItem("entity.apsoperation._self", "ja-JP", "APS 工序排程信息_jp", "实体名称"),
            // entity.apsoperation._self
            new TranslationSeedItem("entity.apsoperation._self", "zh-CN", "APS 工序排程信息", "实体名称"),
            // entity.apsoperation._self
            new TranslationSeedItem("entity.apsoperation._self", "zh-HK", "APS 工序排程信息_hk", "实体名称"),

            // entity.apsoperation.apsorderid
            new TranslationSeedItem("entity.apsoperation.apsorderid", "en-US", "APS订单ID_us", "APS 订单 ID（主子表关系）"),
            // entity.apsoperation.apsorderid
            new TranslationSeedItem("entity.apsoperation.apsorderid", "ja-JP", "APS订单ID_jp", "APS 订单 ID（主子表关系）"),
            // entity.apsoperation.apsorderid
            new TranslationSeedItem("entity.apsoperation.apsorderid", "zh-CN", "APS订单ID", "APS 订单 ID（主子表关系）"),
            // entity.apsoperation.apsorderid
            new TranslationSeedItem("entity.apsoperation.apsorderid", "zh-HK", "APS订单ID_hk", "APS 订单 ID（主子表关系）"),

            // entity.apsoperation.apsordercode
            new TranslationSeedItem("entity.apsoperation.apsordercode", "en-US", "APS订单编码_us", "APS 订单编码（冗余）"),
            // entity.apsoperation.apsordercode
            new TranslationSeedItem("entity.apsoperation.apsordercode", "ja-JP", "APS订单编码_jp", "APS 订单编码（冗余）"),
            // entity.apsoperation.apsordercode
            new TranslationSeedItem("entity.apsoperation.apsordercode", "zh-CN", "APS订单编码", "APS 订单编码（冗余）"),
            // entity.apsoperation.apsordercode
            new TranslationSeedItem("entity.apsoperation.apsordercode", "zh-HK", "APS订单编码_hk", "APS 订单编码（冗余）"),

            // entity.apsoperation.linenumber
            new TranslationSeedItem("entity.apsoperation.linenumber", "en-US", "行号_us", "行号（工序序号）"),
            // entity.apsoperation.linenumber
            new TranslationSeedItem("entity.apsoperation.linenumber", "ja-JP", "行号_jp", "行号（工序序号）"),
            // entity.apsoperation.linenumber
            new TranslationSeedItem("entity.apsoperation.linenumber", "zh-CN", "行号", "行号（工序序号）"),
            // entity.apsoperation.linenumber
            new TranslationSeedItem("entity.apsoperation.linenumber", "zh-HK", "行号_hk", "行号（工序序号）"),

            // entity.apsoperation.routingitemid
            new TranslationSeedItem("entity.apsoperation.routingitemid", "en-US", "工艺路线工序ID_us", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.apsoperation.routingitemid
            new TranslationSeedItem("entity.apsoperation.routingitemid", "ja-JP", "工艺路线工序ID_jp", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.apsoperation.routingitemid
            new TranslationSeedItem("entity.apsoperation.routingitemid", "zh-CN", "工艺路线工序ID", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),
            // entity.apsoperation.routingitemid
            new TranslationSeedItem("entity.apsoperation.routingitemid", "zh-HK", "工艺路线工序ID_hk", "工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）"),

            // entity.apsoperation.processcode
            new TranslationSeedItem("entity.apsoperation.processcode", "en-US", "工序编码_us", "工序编码"),
            // entity.apsoperation.processcode
            new TranslationSeedItem("entity.apsoperation.processcode", "ja-JP", "工序编码_jp", "工序编码"),
            // entity.apsoperation.processcode
            new TranslationSeedItem("entity.apsoperation.processcode", "zh-CN", "工序编码", "工序编码"),
            // entity.apsoperation.processcode
            new TranslationSeedItem("entity.apsoperation.processcode", "zh-HK", "工序编码_hk", "工序编码"),

            // entity.apsoperation.processname
            new TranslationSeedItem("entity.apsoperation.processname", "en-US", "工序名称_us", "工序名称"),
            // entity.apsoperation.processname
            new TranslationSeedItem("entity.apsoperation.processname", "ja-JP", "工序名称_jp", "工序名称"),
            // entity.apsoperation.processname
            new TranslationSeedItem("entity.apsoperation.processname", "zh-CN", "工序名称", "工序名称"),
            // entity.apsoperation.processname
            new TranslationSeedItem("entity.apsoperation.processname", "zh-HK", "工序名称_hk", "工序名称"),

            // entity.apsoperation.workcentercode
            new TranslationSeedItem("entity.apsoperation.workcentercode", "en-US", "工作中心编码_us", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.apsoperation.workcentercode
            new TranslationSeedItem("entity.apsoperation.workcentercode", "ja-JP", "工作中心编码_jp", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.apsoperation.workcentercode
            new TranslationSeedItem("entity.apsoperation.workcentercode", "zh-CN", "工作中心编码", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.apsoperation.workcentercode
            new TranslationSeedItem("entity.apsoperation.workcentercode", "zh-HK", "工作中心编码_hk", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),

            // entity.apsoperation.workcenterresourceid
            new TranslationSeedItem("entity.apsoperation.workcenterresourceid", "en-US", "工作中心资源ID_us", "工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）"),
            // entity.apsoperation.workcenterresourceid
            new TranslationSeedItem("entity.apsoperation.workcenterresourceid", "ja-JP", "工作中心资源ID_jp", "工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）"),
            // entity.apsoperation.workcenterresourceid
            new TranslationSeedItem("entity.apsoperation.workcenterresourceid", "zh-CN", "工作中心资源ID", "工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）"),
            // entity.apsoperation.workcenterresourceid
            new TranslationSeedItem("entity.apsoperation.workcenterresourceid", "zh-HK", "工作中心资源ID_hk", "工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）"),

            // entity.apsoperation.plannedstarttime
            new TranslationSeedItem("entity.apsoperation.plannedstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.apsoperation.plannedstarttime
            new TranslationSeedItem("entity.apsoperation.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.apsoperation.plannedstarttime
            new TranslationSeedItem("entity.apsoperation.plannedstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.apsoperation.plannedstarttime
            new TranslationSeedItem("entity.apsoperation.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.apsoperation.plannedendtime
            new TranslationSeedItem("entity.apsoperation.plannedendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.apsoperation.plannedendtime
            new TranslationSeedItem("entity.apsoperation.plannedendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.apsoperation.plannedendtime
            new TranslationSeedItem("entity.apsoperation.plannedendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.apsoperation.plannedendtime
            new TranslationSeedItem("entity.apsoperation.plannedendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.apsoperation.planneddurationminutes
            new TranslationSeedItem("entity.apsoperation.planneddurationminutes", "en-US", "计划工时分钟_us", "计划工时（分钟）"),
            // entity.apsoperation.planneddurationminutes
            new TranslationSeedItem("entity.apsoperation.planneddurationminutes", "ja-JP", "计划工时分钟_jp", "计划工时（分钟）"),
            // entity.apsoperation.planneddurationminutes
            new TranslationSeedItem("entity.apsoperation.planneddurationminutes", "zh-CN", "计划工时分钟", "计划工时（分钟）"),
            // entity.apsoperation.planneddurationminutes
            new TranslationSeedItem("entity.apsoperation.planneddurationminutes", "zh-HK", "计划工时分钟_hk", "计划工时（分钟）"),

            // entity.apsoperation.changeoverminutes
            new TranslationSeedItem("entity.apsoperation.changeoverminutes", "en-US", "换型时间分钟_us", "换型时间（分钟）"),
            // entity.apsoperation.changeoverminutes
            new TranslationSeedItem("entity.apsoperation.changeoverminutes", "ja-JP", "换型时间分钟_jp", "换型时间（分钟）"),
            // entity.apsoperation.changeoverminutes
            new TranslationSeedItem("entity.apsoperation.changeoverminutes", "zh-CN", "换型时间分钟", "换型时间（分钟）"),
            // entity.apsoperation.changeoverminutes
            new TranslationSeedItem("entity.apsoperation.changeoverminutes", "zh-HK", "换型时间分钟_hk", "换型时间（分钟）"),

            // entity.apsoperation.operationstatus
            new TranslationSeedItem("entity.apsoperation.operationstatus", "en-US", "工序状态_us", "工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）"),
            // entity.apsoperation.operationstatus
            new TranslationSeedItem("entity.apsoperation.operationstatus", "ja-JP", "工序状态_jp", "工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）"),
            // entity.apsoperation.operationstatus
            new TranslationSeedItem("entity.apsoperation.operationstatus", "zh-CN", "工序状态", "工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）"),
            // entity.apsoperation.operationstatus
            new TranslationSeedItem("entity.apsoperation.operationstatus", "zh-HK", "工序状态_hk", "工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）"),

            // entity.apsoperation.isobsolete
            new TranslationSeedItem("entity.apsoperation.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.apsoperation.isobsolete
            new TranslationSeedItem("entity.apsoperation.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.apsoperation.isobsolete
            new TranslationSeedItem("entity.apsoperation.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.apsoperation.isobsolete
            new TranslationSeedItem("entity.apsoperation.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Aps";
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
