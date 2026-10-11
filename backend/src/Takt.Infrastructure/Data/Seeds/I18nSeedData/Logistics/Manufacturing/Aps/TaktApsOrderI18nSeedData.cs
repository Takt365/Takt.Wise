// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOrderI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktApsOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktApsOrder 实体国际化翻译种子（键前缀 entity.apsorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktApsOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktApsOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 apsorder 实体翻译...", tenantCode);

        foreach (var item in GetApsOrderTranslations())
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

        TaktLogger.Information("TaktApsOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktApsOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.apsorder._self / entity.apsorder.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetApsOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.apsorder._self
            new TranslationSeedItem("entity.apsorder._self", "en-US", "Aps Order Information_us", "实体名称"),
            // entity.apsorder._self
            new TranslationSeedItem("entity.apsorder._self", "ja-JP", "APS 排程订单信息_jp", "实体名称"),
            // entity.apsorder._self
            new TranslationSeedItem("entity.apsorder._self", "zh-CN", "APS 排程订单信息", "实体名称"),
            // entity.apsorder._self
            new TranslationSeedItem("entity.apsorder._self", "zh-HK", "APS 排程订单信息_hk", "实体名称"),

            // entity.apsorder.code
            new TranslationSeedItem("entity.apsorder.code", "en-US", "APS订单编码_us", "APS 订单编码"),
            // entity.apsorder.code
            new TranslationSeedItem("entity.apsorder.code", "ja-JP", "APS订单编码_jp", "APS 订单编码"),
            // entity.apsorder.code
            new TranslationSeedItem("entity.apsorder.code", "zh-CN", "APS订单编码", "APS 订单编码"),
            // entity.apsorder.code
            new TranslationSeedItem("entity.apsorder.code", "zh-HK", "APS订单编码_hk", "APS 订单编码"),

            // entity.apsorder.plannedorderid
            new TranslationSeedItem("entity.apsorder.plannedorderid", "en-US", "来源计划订单ID_us", "来源计划订单 ID"),
            // entity.apsorder.plannedorderid
            new TranslationSeedItem("entity.apsorder.plannedorderid", "ja-JP", "来源计划订单ID_jp", "来源计划订单 ID"),
            // entity.apsorder.plannedorderid
            new TranslationSeedItem("entity.apsorder.plannedorderid", "zh-CN", "来源计划订单ID", "来源计划订单 ID"),
            // entity.apsorder.plannedorderid
            new TranslationSeedItem("entity.apsorder.plannedorderid", "zh-HK", "来源计划订单ID_hk", "来源计划订单 ID"),

            // entity.apsorder.plannedordercode
            new TranslationSeedItem("entity.apsorder.plannedordercode", "en-US", "来源计划订单编码_us", "来源计划订单编码（冗余）"),
            // entity.apsorder.plannedordercode
            new TranslationSeedItem("entity.apsorder.plannedordercode", "ja-JP", "来源计划订单编码_jp", "来源计划订单编码（冗余）"),
            // entity.apsorder.plannedordercode
            new TranslationSeedItem("entity.apsorder.plannedordercode", "zh-CN", "来源计划订单编码", "来源计划订单编码（冗余）"),
            // entity.apsorder.plannedordercode
            new TranslationSeedItem("entity.apsorder.plannedordercode", "zh-HK", "来源计划订单编码_hk", "来源计划订单编码（冗余）"),

            // entity.apsorder.materialcode
            new TranslationSeedItem("entity.apsorder.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.apsorder.materialcode
            new TranslationSeedItem("entity.apsorder.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.apsorder.materialcode
            new TranslationSeedItem("entity.apsorder.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.apsorder.materialcode
            new TranslationSeedItem("entity.apsorder.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.apsorder.orderquantity
            new TranslationSeedItem("entity.apsorder.orderquantity", "en-US", "订单数量_us", "订单数量"),
            // entity.apsorder.orderquantity
            new TranslationSeedItem("entity.apsorder.orderquantity", "ja-JP", "订单数量_jp", "订单数量"),
            // entity.apsorder.orderquantity
            new TranslationSeedItem("entity.apsorder.orderquantity", "zh-CN", "订单数量", "订单数量"),
            // entity.apsorder.orderquantity
            new TranslationSeedItem("entity.apsorder.orderquantity", "zh-HK", "订单数量_hk", "订单数量"),

            // entity.apsorder.unitofmeasure
            new TranslationSeedItem("entity.apsorder.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.apsorder.unitofmeasure
            new TranslationSeedItem("entity.apsorder.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.apsorder.unitofmeasure
            new TranslationSeedItem("entity.apsorder.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.apsorder.unitofmeasure
            new TranslationSeedItem("entity.apsorder.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.apsorder.routingcode
            new TranslationSeedItem("entity.apsorder.routingcode", "en-US", "工艺路线编码_us", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),
            // entity.apsorder.routingcode
            new TranslationSeedItem("entity.apsorder.routingcode", "ja-JP", "工艺路线编码_jp", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),
            // entity.apsorder.routingcode
            new TranslationSeedItem("entity.apsorder.routingcode", "zh-CN", "工艺路线编码", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),
            // entity.apsorder.routingcode
            new TranslationSeedItem("entity.apsorder.routingcode", "zh-HK", "工艺路线编码_hk", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),

            // entity.apsorder.plannedstarttime
            new TranslationSeedItem("entity.apsorder.plannedstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.apsorder.plannedstarttime
            new TranslationSeedItem("entity.apsorder.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.apsorder.plannedstarttime
            new TranslationSeedItem("entity.apsorder.plannedstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.apsorder.plannedstarttime
            new TranslationSeedItem("entity.apsorder.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.apsorder.plannedendtime
            new TranslationSeedItem("entity.apsorder.plannedendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.apsorder.plannedendtime
            new TranslationSeedItem("entity.apsorder.plannedendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.apsorder.plannedendtime
            new TranslationSeedItem("entity.apsorder.plannedendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.apsorder.plannedendtime
            new TranslationSeedItem("entity.apsorder.plannedendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.apsorder.orderstatus
            new TranslationSeedItem("entity.apsorder.orderstatus", "en-US", "APS订单状态_us", "APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）"),
            // entity.apsorder.orderstatus
            new TranslationSeedItem("entity.apsorder.orderstatus", "ja-JP", "APS订单状态_jp", "APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）"),
            // entity.apsorder.orderstatus
            new TranslationSeedItem("entity.apsorder.orderstatus", "zh-CN", "APS订单状态", "APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）"),
            // entity.apsorder.orderstatus
            new TranslationSeedItem("entity.apsorder.orderstatus", "zh-HK", "APS订单状态_hk", "APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）"),

            // entity.apsorder.apsscheduleid
            new TranslationSeedItem("entity.apsorder.apsscheduleid", "en-US", "APS排程批次ID_us", "关联 APS 排程批次 ID（可选）"),
            // entity.apsorder.apsscheduleid
            new TranslationSeedItem("entity.apsorder.apsscheduleid", "ja-JP", "APS排程批次ID_jp", "关联 APS 排程批次 ID（可选）"),
            // entity.apsorder.apsscheduleid
            new TranslationSeedItem("entity.apsorder.apsscheduleid", "zh-CN", "APS排程批次ID", "关联 APS 排程批次 ID（可选）"),
            // entity.apsorder.apsscheduleid
            new TranslationSeedItem("entity.apsorder.apsscheduleid", "zh-HK", "APS排程批次ID_hk", "关联 APS 排程批次 ID（可选）"),

            // entity.apsorder.operations
            new TranslationSeedItem("entity.apsorder.operations", "en-US", "APS 工序排程列表_us", "APS 工序排程列表"),
            // entity.apsorder.operations
            new TranslationSeedItem("entity.apsorder.operations", "ja-JP", "APS 工序排程列表_jp", "APS 工序排程列表"),
            // entity.apsorder.operations
            new TranslationSeedItem("entity.apsorder.operations", "zh-CN", "APS 工序排程列表", "APS 工序排程列表"),
            // entity.apsorder.operations
            new TranslationSeedItem("entity.apsorder.operations", "zh-HK", "APS 工序排程列表_hk", "APS 工序排程列表"),
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
