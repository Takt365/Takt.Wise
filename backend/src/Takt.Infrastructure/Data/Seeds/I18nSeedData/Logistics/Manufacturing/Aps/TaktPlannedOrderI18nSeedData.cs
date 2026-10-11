// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktPlannedOrderI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPlannedOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktPlannedOrder 实体国际化翻译种子（键前缀 entity.plannedorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPlannedOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPlannedOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 plannedorder 实体翻译...", tenantCode);

        foreach (var item in GetPlannedOrderTranslations())
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

        TaktLogger.Information("TaktPlannedOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPlannedOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.plannedorder._self / entity.plannedorder.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPlannedOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.plannedorder._self
            new TranslationSeedItem("entity.plannedorder._self", "en-US", "Planned Order Information_us", "实体名称"),
            // entity.plannedorder._self
            new TranslationSeedItem("entity.plannedorder._self", "ja-JP", "计划订单信息_jp", "实体名称"),
            // entity.plannedorder._self
            new TranslationSeedItem("entity.plannedorder._self", "zh-CN", "计划订单信息", "实体名称"),
            // entity.plannedorder._self
            new TranslationSeedItem("entity.plannedorder._self", "zh-HK", "计划订单信息_hk", "实体名称"),

            // entity.plannedorder.code
            new TranslationSeedItem("entity.plannedorder.code", "en-US", "计划订单编码_us", "计划订单编码"),
            // entity.plannedorder.code
            new TranslationSeedItem("entity.plannedorder.code", "ja-JP", "计划订单编码_jp", "计划订单编码"),
            // entity.plannedorder.code
            new TranslationSeedItem("entity.plannedorder.code", "zh-CN", "计划订单编码", "计划订单编码"),
            // entity.plannedorder.code
            new TranslationSeedItem("entity.plannedorder.code", "zh-HK", "计划订单编码_hk", "计划订单编码"),

            // entity.plannedorder.materialrequirementsplanningid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningid", "en-US", "来源MRP头表ID_us", "来源 MRP 头表 ID"),
            // entity.plannedorder.materialrequirementsplanningid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningid", "ja-JP", "来源MRP头表ID_jp", "来源 MRP 头表 ID"),
            // entity.plannedorder.materialrequirementsplanningid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningid", "zh-CN", "来源MRP头表ID", "来源 MRP 头表 ID"),
            // entity.plannedorder.materialrequirementsplanningid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningid", "zh-HK", "来源MRP头表ID_hk", "来源 MRP 头表 ID"),

            // entity.plannedorder.materialrequirementsplanningcode
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningcode", "en-US", "来源MRP编码_us", "来源 MRP 编码（冗余）"),
            // entity.plannedorder.materialrequirementsplanningcode
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningcode", "ja-JP", "来源MRP编码_jp", "来源 MRP 编码（冗余）"),
            // entity.plannedorder.materialrequirementsplanningcode
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningcode", "zh-CN", "来源MRP编码", "来源 MRP 编码（冗余）"),
            // entity.plannedorder.materialrequirementsplanningcode
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningcode", "zh-HK", "来源MRP编码_hk", "来源 MRP 编码（冗余）"),

            // entity.plannedorder.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningitemid", "en-US", "来源MRP明细行ID_us", "来源 MRP 明细行 ID"),
            // entity.plannedorder.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningitemid", "ja-JP", "来源MRP明细行ID_jp", "来源 MRP 明细行 ID"),
            // entity.plannedorder.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningitemid", "zh-CN", "来源MRP明细行ID", "来源 MRP 明细行 ID"),
            // entity.plannedorder.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.plannedorder.materialrequirementsplanningitemid", "zh-HK", "来源MRP明细行ID_hk", "来源 MRP 明细行 ID"),

            // entity.plannedorder.materialcode
            new TranslationSeedItem("entity.plannedorder.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.plannedorder.materialcode
            new TranslationSeedItem("entity.plannedorder.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.plannedorder.materialcode
            new TranslationSeedItem("entity.plannedorder.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.plannedorder.materialcode
            new TranslationSeedItem("entity.plannedorder.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.plannedorder.plannedquantity
            new TranslationSeedItem("entity.plannedorder.plannedquantity", "en-US", "计划数量_us", "计划数量"),
            // entity.plannedorder.plannedquantity
            new TranslationSeedItem("entity.plannedorder.plannedquantity", "ja-JP", "计划数量_jp", "计划数量"),
            // entity.plannedorder.plannedquantity
            new TranslationSeedItem("entity.plannedorder.plannedquantity", "zh-CN", "计划数量", "计划数量"),
            // entity.plannedorder.plannedquantity
            new TranslationSeedItem("entity.plannedorder.plannedquantity", "zh-HK", "计划数量_hk", "计划数量"),

            // entity.plannedorder.unitofmeasure
            new TranslationSeedItem("entity.plannedorder.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.plannedorder.unitofmeasure
            new TranslationSeedItem("entity.plannedorder.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.plannedorder.unitofmeasure
            new TranslationSeedItem("entity.plannedorder.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.plannedorder.unitofmeasure
            new TranslationSeedItem("entity.plannedorder.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.plannedorder.plannedstarttime
            new TranslationSeedItem("entity.plannedorder.plannedstarttime", "en-US", "计划开始时间_us", "计划开始时间"),
            // entity.plannedorder.plannedstarttime
            new TranslationSeedItem("entity.plannedorder.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划开始时间"),
            // entity.plannedorder.plannedstarttime
            new TranslationSeedItem("entity.plannedorder.plannedstarttime", "zh-CN", "计划开始时间", "计划开始时间"),
            // entity.plannedorder.plannedstarttime
            new TranslationSeedItem("entity.plannedorder.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划开始时间"),

            // entity.plannedorder.plannedendtime
            new TranslationSeedItem("entity.plannedorder.plannedendtime", "en-US", "计划结束时间_us", "计划结束时间"),
            // entity.plannedorder.plannedendtime
            new TranslationSeedItem("entity.plannedorder.plannedendtime", "ja-JP", "计划结束时间_jp", "计划结束时间"),
            // entity.plannedorder.plannedendtime
            new TranslationSeedItem("entity.plannedorder.plannedendtime", "zh-CN", "计划结束时间", "计划结束时间"),
            // entity.plannedorder.plannedendtime
            new TranslationSeedItem("entity.plannedorder.plannedendtime", "zh-HK", "计划结束时间_hk", "计划结束时间"),

            // entity.plannedorder.routingcode
            new TranslationSeedItem("entity.plannedorder.routingcode", "en-US", "工艺路线编码_us", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),
            // entity.plannedorder.routingcode
            new TranslationSeedItem("entity.plannedorder.routingcode", "ja-JP", "工艺路线编码_jp", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),
            // entity.plannedorder.routingcode
            new TranslationSeedItem("entity.plannedorder.routingcode", "zh-CN", "工艺路线编码", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),
            // entity.plannedorder.routingcode
            new TranslationSeedItem("entity.plannedorder.routingcode", "zh-HK", "工艺路线编码_hk", "工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）"),

            // entity.plannedorder.orderstatus
            new TranslationSeedItem("entity.plannedorder.orderstatus", "en-US", "计划订单状态_us", "计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）"),
            // entity.plannedorder.orderstatus
            new TranslationSeedItem("entity.plannedorder.orderstatus", "ja-JP", "计划订单状态_jp", "计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）"),
            // entity.plannedorder.orderstatus
            new TranslationSeedItem("entity.plannedorder.orderstatus", "zh-CN", "计划订单状态", "计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）"),
            // entity.plannedorder.orderstatus
            new TranslationSeedItem("entity.plannedorder.orderstatus", "zh-HK", "计划订单状态_hk", "计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）"),
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
