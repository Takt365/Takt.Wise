// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderI18nSeedData.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionOrder 实体国际化翻译种子（键前缀 entity.productionorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionorder 实体翻译...", tenantCode);

        foreach (var item in GetProductionOrderTranslations())
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

        TaktLogger.Information("TaktProductionOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionorder._self / entity.productionorder.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionorder._self
            new TranslationSeedItem("entity.productionorder._self", "en-US", "Production Order Information_us", "实体名称"),
            // entity.productionorder._self
            new TranslationSeedItem("entity.productionorder._self", "ja-JP", "生产工单信息_jp", "实体名称"),
            // entity.productionorder._self
            new TranslationSeedItem("entity.productionorder._self", "zh-CN", "生产工单信息", "实体名称"),
            // entity.productionorder._self
            new TranslationSeedItem("entity.productionorder._self", "zh-HK", "生产工单信息_hk", "实体名称"),

            // entity.productionorder.plantcode
            new TranslationSeedItem("entity.productionorder.plantcode", "en-US", "工厂代码_us", "工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.productionorder.plantcode
            new TranslationSeedItem("entity.productionorder.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.productionorder.plantcode
            new TranslationSeedItem("entity.productionorder.plantcode", "zh-CN", "工厂代码", "工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),
            // entity.productionorder.plantcode
            new TranslationSeedItem("entity.productionorder.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）"),

            // entity.productionorder.prodordertype
            new TranslationSeedItem("entity.productionorder.prodordertype", "en-US", "工单类别_us", "工单类别（字典 logistics_manufacturing_prod_order_type，存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）"),
            // entity.productionorder.prodordertype
            new TranslationSeedItem("entity.productionorder.prodordertype", "ja-JP", "工单类别_jp", "工单类别（字典 logistics_manufacturing_prod_order_type，存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）"),
            // entity.productionorder.prodordertype
            new TranslationSeedItem("entity.productionorder.prodordertype", "zh-CN", "工单类别", "工单类别（字典 logistics_manufacturing_prod_order_type，存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）"),
            // entity.productionorder.prodordertype
            new TranslationSeedItem("entity.productionorder.prodordertype", "zh-HK", "工单类别_hk", "工单类别（字典 logistics_manufacturing_prod_order_type，存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）"),

            // entity.productionorder.prodordercode
            new TranslationSeedItem("entity.productionorder.prodordercode", "en-US", "工单号_us", "工单号"),
            // entity.productionorder.prodordercode
            new TranslationSeedItem("entity.productionorder.prodordercode", "ja-JP", "工单号_jp", "工单号"),
            // entity.productionorder.prodordercode
            new TranslationSeedItem("entity.productionorder.prodordercode", "zh-CN", "工单号", "工单号"),
            // entity.productionorder.prodordercode
            new TranslationSeedItem("entity.productionorder.prodordercode", "zh-HK", "工单号_hk", "工单号"),

            // entity.productionorder.materialcode
            new TranslationSeedItem("entity.productionorder.materialcode", "en-US", "物料编码_us", "物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）"),
            // entity.productionorder.materialcode
            new TranslationSeedItem("entity.productionorder.materialcode", "ja-JP", "物料编码_jp", "物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）"),
            // entity.productionorder.materialcode
            new TranslationSeedItem("entity.productionorder.materialcode", "zh-CN", "物料编码", "物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）"),
            // entity.productionorder.materialcode
            new TranslationSeedItem("entity.productionorder.materialcode", "zh-HK", "物料编码_hk", "物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）"),

            // entity.productionorder.prodorderqty
            new TranslationSeedItem("entity.productionorder.prodorderqty", "en-US", "工单数量_us", "工单数量"),
            // entity.productionorder.prodorderqty
            new TranslationSeedItem("entity.productionorder.prodorderqty", "ja-JP", "工单数量_jp", "工单数量"),
            // entity.productionorder.prodorderqty
            new TranslationSeedItem("entity.productionorder.prodorderqty", "zh-CN", "工单数量", "工单数量"),
            // entity.productionorder.prodorderqty
            new TranslationSeedItem("entity.productionorder.prodorderqty", "zh-HK", "工单数量_hk", "工单数量"),

            // entity.productionorder.producedqty
            new TranslationSeedItem("entity.productionorder.producedqty", "en-US", "已生产数量_us", "已生产数量"),
            // entity.productionorder.producedqty
            new TranslationSeedItem("entity.productionorder.producedqty", "ja-JP", "已生产数量_jp", "已生产数量"),
            // entity.productionorder.producedqty
            new TranslationSeedItem("entity.productionorder.producedqty", "zh-CN", "已生产数量", "已生产数量"),
            // entity.productionorder.producedqty
            new TranslationSeedItem("entity.productionorder.producedqty", "zh-HK", "已生产数量_hk", "已生产数量"),

            // entity.productionorder.unitofmeasure
            new TranslationSeedItem("entity.productionorder.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code，存 DictValue）"),
            // entity.productionorder.unitofmeasure
            new TranslationSeedItem("entity.productionorder.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code，存 DictValue）"),
            // entity.productionorder.unitofmeasure
            new TranslationSeedItem("entity.productionorder.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code，存 DictValue）"),
            // entity.productionorder.unitofmeasure
            new TranslationSeedItem("entity.productionorder.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code，存 DictValue）"),

            // entity.productionorder.actualstartdate
            new TranslationSeedItem("entity.productionorder.actualstartdate", "en-US", "实际开始日期_us", "实际开始日期"),
            // entity.productionorder.actualstartdate
            new TranslationSeedItem("entity.productionorder.actualstartdate", "ja-JP", "实际开始日期_jp", "实际开始日期"),
            // entity.productionorder.actualstartdate
            new TranslationSeedItem("entity.productionorder.actualstartdate", "zh-CN", "实际开始日期", "实际开始日期"),
            // entity.productionorder.actualstartdate
            new TranslationSeedItem("entity.productionorder.actualstartdate", "zh-HK", "实际开始日期_hk", "实际开始日期"),

            // entity.productionorder.actualenddate
            new TranslationSeedItem("entity.productionorder.actualenddate", "en-US", "实际完成日期_us", "实际完成日期"),
            // entity.productionorder.actualenddate
            new TranslationSeedItem("entity.productionorder.actualenddate", "ja-JP", "实际完成日期_jp", "实际完成日期"),
            // entity.productionorder.actualenddate
            new TranslationSeedItem("entity.productionorder.actualenddate", "zh-CN", "实际完成日期", "实际完成日期"),
            // entity.productionorder.actualenddate
            new TranslationSeedItem("entity.productionorder.actualenddate", "zh-HK", "实际完成日期_hk", "实际完成日期"),

            // entity.productionorder.priority
            new TranslationSeedItem("entity.productionorder.priority", "en-US", "优先级_us", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),
            // entity.productionorder.priority
            new TranslationSeedItem("entity.productionorder.priority", "ja-JP", "优先级_jp", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),
            // entity.productionorder.priority
            new TranslationSeedItem("entity.productionorder.priority", "zh-CN", "优先级", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),
            // entity.productionorder.priority
            new TranslationSeedItem("entity.productionorder.priority", "zh-HK", "优先级_hk", "优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）"),

            // entity.productionorder.workcenter
            new TranslationSeedItem("entity.productionorder.workcenter", "en-US", "工作中心_us", "工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.workcenter
            new TranslationSeedItem("entity.productionorder.workcenter", "ja-JP", "工作中心_jp", "工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.workcenter
            new TranslationSeedItem("entity.productionorder.workcenter", "zh-CN", "工作中心", "工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.workcenter
            new TranslationSeedItem("entity.productionorder.workcenter", "zh-HK", "工作中心_hk", "工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）"),

            // entity.productionorder.prodbatch
            new TranslationSeedItem("entity.productionorder.prodbatch", "en-US", "生产批次_us", "生产批次"),
            // entity.productionorder.prodbatch
            new TranslationSeedItem("entity.productionorder.prodbatch", "ja-JP", "生产批次_jp", "生产批次"),
            // entity.productionorder.prodbatch
            new TranslationSeedItem("entity.productionorder.prodbatch", "zh-CN", "生产批次", "生产批次"),
            // entity.productionorder.prodbatch
            new TranslationSeedItem("entity.productionorder.prodbatch", "zh-HK", "生产批次_hk", "生产批次"),

            // entity.productionorder.serialCode
            new TranslationSeedItem("entity.productionorder.serialCode", "en-US", "序列号_us", "序列号"),
            // entity.productionorder.serialCode
            new TranslationSeedItem("entity.productionorder.serialCode", "ja-JP", "序列号_jp", "序列号"),
            // entity.productionorder.serialCode
            new TranslationSeedItem("entity.productionorder.serialCode", "zh-CN", "序列号", "序列号"),
            // entity.productionorder.serialCode
            new TranslationSeedItem("entity.productionorder.serialCode", "zh-HK", "序列号_hk", "序列号"),

            // entity.productionorder.routingcode
            new TranslationSeedItem("entity.productionorder.routingcode", "en-US", "工艺路线编码_us", "工艺路线编码"),
            // entity.productionorder.routingcode
            new TranslationSeedItem("entity.productionorder.routingcode", "ja-JP", "工艺路线编码_jp", "工艺路线编码"),
            // entity.productionorder.routingcode
            new TranslationSeedItem("entity.productionorder.routingcode", "zh-CN", "工艺路线编码", "工艺路线编码"),
            // entity.productionorder.routingcode
            new TranslationSeedItem("entity.productionorder.routingcode", "zh-HK", "工艺路线编码_hk", "工艺路线编码"),

            // entity.productionorder.plannedorderid
            new TranslationSeedItem("entity.productionorder.plannedorderid", "en-US", "来源计划订单ID_us", "来源计划订单 ID（关联 TaktPlannedOrder.Id，选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.plannedorderid
            new TranslationSeedItem("entity.productionorder.plannedorderid", "ja-JP", "来源计划订单ID_jp", "来源计划订单 ID（关联 TaktPlannedOrder.Id，选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.plannedorderid
            new TranslationSeedItem("entity.productionorder.plannedorderid", "zh-CN", "来源计划订单ID", "来源计划订单 ID（关联 TaktPlannedOrder.Id，选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.plannedorderid
            new TranslationSeedItem("entity.productionorder.plannedorderid", "zh-HK", "来源计划订单ID_hk", "来源计划订单 ID（关联 TaktPlannedOrder.Id，选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤）"),

            // entity.productionorder.apsorderid
            new TranslationSeedItem("entity.productionorder.apsorderid", "en-US", "来源APS订单ID_us", "来源 APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.apsorderid
            new TranslationSeedItem("entity.productionorder.apsorderid", "ja-JP", "来源APS订单ID_jp", "来源 APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.apsorderid
            new TranslationSeedItem("entity.productionorder.apsorderid", "zh-CN", "来源APS订单ID", "来源 APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options，ExtValue=PlantCode 过滤）"),
            // entity.productionorder.apsorderid
            new TranslationSeedItem("entity.productionorder.apsorderid", "zh-HK", "来源APS订单ID_hk", "来源 APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options，ExtValue=PlantCode 过滤）"),

            // entity.productionorder.plannedstarttime
            new TranslationSeedItem("entity.productionorder.plannedstarttime", "en-US", "计划开工时间_us", "计划开工时间"),
            // entity.productionorder.plannedstarttime
            new TranslationSeedItem("entity.productionorder.plannedstarttime", "ja-JP", "计划开工时间_jp", "计划开工时间"),
            // entity.productionorder.plannedstarttime
            new TranslationSeedItem("entity.productionorder.plannedstarttime", "zh-CN", "计划开工时间", "计划开工时间"),
            // entity.productionorder.plannedstarttime
            new TranslationSeedItem("entity.productionorder.plannedstarttime", "zh-HK", "计划开工时间_hk", "计划开工时间"),

            // entity.productionorder.plannedendtime
            new TranslationSeedItem("entity.productionorder.plannedendtime", "en-US", "计划完工时间_us", "计划完工时间"),
            // entity.productionorder.plannedendtime
            new TranslationSeedItem("entity.productionorder.plannedendtime", "ja-JP", "计划完工时间_jp", "计划完工时间"),
            // entity.productionorder.plannedendtime
            new TranslationSeedItem("entity.productionorder.plannedendtime", "zh-CN", "计划完工时间", "计划完工时间"),
            // entity.productionorder.plannedendtime
            new TranslationSeedItem("entity.productionorder.plannedendtime", "zh-HK", "计划完工时间_hk", "计划完工时间"),

            // entity.productionorder.orderstatus
            new TranslationSeedItem("entity.productionorder.orderstatus", "en-US", "状态_us", "状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）"),
            // entity.productionorder.orderstatus
            new TranslationSeedItem("entity.productionorder.orderstatus", "ja-JP", "状态_jp", "状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）"),
            // entity.productionorder.orderstatus
            new TranslationSeedItem("entity.productionorder.orderstatus", "zh-CN", "状态", "状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）"),
            // entity.productionorder.orderstatus
            new TranslationSeedItem("entity.productionorder.orderstatus", "zh-HK", "状态_hk", "状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）"),
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
        translation.ResourceGroup = "Output";
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
