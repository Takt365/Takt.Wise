// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionDispatchI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionDispatch 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktProductionDispatch 实体国际化翻译种子（键前缀 entity.productiondispatch.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionDispatchI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionDispatch 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productiondispatch 实体翻译...", tenantCode);

        foreach (var item in GetProductionDispatchTranslations())
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

        TaktLogger.Information("TaktProductionDispatch 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionDispatch 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productiondispatch._self / entity.productiondispatch.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionDispatchTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productiondispatch._self
            new TranslationSeedItem("entity.productiondispatch._self", "en-US", "Production Dispatch Information_us", "实体名称"),
            // entity.productiondispatch._self
            new TranslationSeedItem("entity.productiondispatch._self", "ja-JP", "生产派工单信息_jp", "实体名称"),
            // entity.productiondispatch._self
            new TranslationSeedItem("entity.productiondispatch._self", "zh-CN", "生产派工单信息", "实体名称"),
            // entity.productiondispatch._self
            new TranslationSeedItem("entity.productiondispatch._self", "zh-HK", "生产派工单信息_hk", "实体名称"),

            // entity.productiondispatch.dispatchcode
            new TranslationSeedItem("entity.productiondispatch.dispatchcode", "en-US", "派工单编码_us", "派工单编码（前端表单选择编码规则后自动通过 TaktNumbering 派工单编码规则生成并展示，非手输；单据类型菜单：生产派工）"),
            // entity.productiondispatch.dispatchcode
            new TranslationSeedItem("entity.productiondispatch.dispatchcode", "ja-JP", "派工单编码_jp", "派工单编码（前端表单选择编码规则后自动通过 TaktNumbering 派工单编码规则生成并展示，非手输；单据类型菜单：生产派工）"),
            // entity.productiondispatch.dispatchcode
            new TranslationSeedItem("entity.productiondispatch.dispatchcode", "zh-CN", "派工单编码", "派工单编码（前端表单选择编码规则后自动通过 TaktNumbering 派工单编码规则生成并展示，非手输；单据类型菜单：生产派工）"),
            // entity.productiondispatch.dispatchcode
            new TranslationSeedItem("entity.productiondispatch.dispatchcode", "zh-HK", "派工单编码_hk", "派工单编码（前端表单选择编码规则后自动通过 TaktNumbering 派工单编码规则生成并展示，非手输；单据类型菜单：生产派工）"),

            // entity.productiondispatch.productionorderid
            new TranslationSeedItem("entity.productiondispatch.productionorderid", "en-US", "生产工单ID_us", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),
            // entity.productiondispatch.productionorderid
            new TranslationSeedItem("entity.productiondispatch.productionorderid", "ja-JP", "生产工单ID_jp", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),
            // entity.productiondispatch.productionorderid
            new TranslationSeedItem("entity.productiondispatch.productionorderid", "zh-CN", "生产工单ID", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),
            // entity.productiondispatch.productionorderid
            new TranslationSeedItem("entity.productiondispatch.productionorderid", "zh-HK", "生产工单ID_hk", "生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）"),

            // entity.productiondispatch.prodordercode
            new TranslationSeedItem("entity.productiondispatch.prodordercode", "en-US", "工单号_us", "工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）"),
            // entity.productiondispatch.prodordercode
            new TranslationSeedItem("entity.productiondispatch.prodordercode", "ja-JP", "工单号_jp", "工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）"),
            // entity.productiondispatch.prodordercode
            new TranslationSeedItem("entity.productiondispatch.prodordercode", "zh-CN", "工单号", "工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）"),
            // entity.productiondispatch.prodordercode
            new TranslationSeedItem("entity.productiondispatch.prodordercode", "zh-HK", "工单号_hk", "工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）"),

            // entity.productiondispatch.apsoperationid
            new TranslationSeedItem("entity.productiondispatch.apsoperationid", "en-US", "APS工序排程ID_us", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),
            // entity.productiondispatch.apsoperationid
            new TranslationSeedItem("entity.productiondispatch.apsoperationid", "ja-JP", "APS工序排程ID_jp", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),
            // entity.productiondispatch.apsoperationid
            new TranslationSeedItem("entity.productiondispatch.apsoperationid", "zh-CN", "APS工序排程ID", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),
            // entity.productiondispatch.apsoperationid
            new TranslationSeedItem("entity.productiondispatch.apsoperationid", "zh-HK", "APS工序排程ID_hk", "APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）"),

            // entity.productiondispatch.workcentercode
            new TranslationSeedItem("entity.productiondispatch.workcentercode", "en-US", "工作中心编码_us", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.productiondispatch.workcentercode
            new TranslationSeedItem("entity.productiondispatch.workcentercode", "ja-JP", "工作中心编码_jp", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.productiondispatch.workcentercode
            new TranslationSeedItem("entity.productiondispatch.workcentercode", "zh-CN", "工作中心编码", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.productiondispatch.workcentercode
            new TranslationSeedItem("entity.productiondispatch.workcentercode", "zh-HK", "工作中心编码_hk", "工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),

            // entity.productiondispatch.processcode
            new TranslationSeedItem("entity.productiondispatch.processcode", "en-US", "工序编码_us", "工序编码"),
            // entity.productiondispatch.processcode
            new TranslationSeedItem("entity.productiondispatch.processcode", "ja-JP", "工序编码_jp", "工序编码"),
            // entity.productiondispatch.processcode
            new TranslationSeedItem("entity.productiondispatch.processcode", "zh-CN", "工序编码", "工序编码"),
            // entity.productiondispatch.processcode
            new TranslationSeedItem("entity.productiondispatch.processcode", "zh-HK", "工序编码_hk", "工序编码"),

            // entity.productiondispatch.dispatchquantity
            new TranslationSeedItem("entity.productiondispatch.dispatchquantity", "en-US", "派工数量_us", "派工数量"),
            // entity.productiondispatch.dispatchquantity
            new TranslationSeedItem("entity.productiondispatch.dispatchquantity", "ja-JP", "派工数量_jp", "派工数量"),
            // entity.productiondispatch.dispatchquantity
            new TranslationSeedItem("entity.productiondispatch.dispatchquantity", "zh-CN", "派工数量", "派工数量"),
            // entity.productiondispatch.dispatchquantity
            new TranslationSeedItem("entity.productiondispatch.dispatchquantity", "zh-HK", "派工数量_hk", "派工数量"),

            // entity.productiondispatch.plannedstarttime
            new TranslationSeedItem("entity.productiondispatch.plannedstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.productiondispatch.plannedstarttime
            new TranslationSeedItem("entity.productiondispatch.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.productiondispatch.plannedstarttime
            new TranslationSeedItem("entity.productiondispatch.plannedstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.productiondispatch.plannedstarttime
            new TranslationSeedItem("entity.productiondispatch.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.productiondispatch.plannedendtime
            new TranslationSeedItem("entity.productiondispatch.plannedendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.productiondispatch.plannedendtime
            new TranslationSeedItem("entity.productiondispatch.plannedendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.productiondispatch.plannedendtime
            new TranslationSeedItem("entity.productiondispatch.plannedendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.productiondispatch.plannedendtime
            new TranslationSeedItem("entity.productiondispatch.plannedendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.productiondispatch.dispatchstatus
            new TranslationSeedItem("entity.productiondispatch.dispatchstatus", "en-US", "派工状态_us", "派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）"),
            // entity.productiondispatch.dispatchstatus
            new TranslationSeedItem("entity.productiondispatch.dispatchstatus", "ja-JP", "派工状态_jp", "派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）"),
            // entity.productiondispatch.dispatchstatus
            new TranslationSeedItem("entity.productiondispatch.dispatchstatus", "zh-CN", "派工状态", "派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）"),
            // entity.productiondispatch.dispatchstatus
            new TranslationSeedItem("entity.productiondispatch.dispatchstatus", "zh-HK", "派工状态_hk", "派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）"),
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
