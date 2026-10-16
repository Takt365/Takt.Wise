// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLineI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMasterDemandScheduleLine 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds;

/// <summary>
/// TaktMasterDemandScheduleLine 实体国际化翻译种子（键前缀 entity.masterdemandscheduleline.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMasterDemandScheduleLineI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMasterDemandScheduleLine 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 masterdemandscheduleline 实体翻译...", tenantCode);

        foreach (var item in GetMasterDemandScheduleLineTranslations())
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

        TaktLogger.Information("TaktMasterDemandScheduleLine 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMasterDemandScheduleLine 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.masterdemandscheduleline._self / entity.masterdemandscheduleline.{{field}}；ResourceGroup=Mds；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMasterDemandScheduleLineTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.masterdemandscheduleline._self
            new TranslationSeedItem("entity.masterdemandscheduleline._self", "en-US", "Master Demand Schedule Line Information_us", "实体名称"),
            // entity.masterdemandscheduleline._self
            new TranslationSeedItem("entity.masterdemandscheduleline._self", "ja-JP", "主需求计划 MDS 行信息_jp", "实体名称"),
            // entity.masterdemandscheduleline._self
            new TranslationSeedItem("entity.masterdemandscheduleline._self", "zh-CN", "主需求计划 MDS 行信息", "实体名称"),
            // entity.masterdemandscheduleline._self
            new TranslationSeedItem("entity.masterdemandscheduleline._self", "zh-HK", "主需求计划 MDS 行信息_hk", "实体名称"),

            // entity.masterdemandscheduleline.masterdemandscheduleid
            new TranslationSeedItem("entity.masterdemandscheduleline.masterdemandscheduleid", "en-US", "MDS头表ID_us", "MDS 头表 ID（主子表关系）"),
            // entity.masterdemandscheduleline.masterdemandscheduleid
            new TranslationSeedItem("entity.masterdemandscheduleline.masterdemandscheduleid", "ja-JP", "MDS头表ID_jp", "MDS 头表 ID（主子表关系）"),
            // entity.masterdemandscheduleline.masterdemandscheduleid
            new TranslationSeedItem("entity.masterdemandscheduleline.masterdemandscheduleid", "zh-CN", "MDS头表ID", "MDS 头表 ID（主子表关系）"),
            // entity.masterdemandscheduleline.masterdemandscheduleid
            new TranslationSeedItem("entity.masterdemandscheduleline.masterdemandscheduleid", "zh-HK", "MDS头表ID_hk", "MDS 头表 ID（主子表关系）"),

            // entity.masterdemandscheduleline.mdscode
            new TranslationSeedItem("entity.masterdemandscheduleline.mdscode", "en-US", "MDS编码_us", "MDS 编码（冗余）"),
            // entity.masterdemandscheduleline.mdscode
            new TranslationSeedItem("entity.masterdemandscheduleline.mdscode", "ja-JP", "MDS编码_jp", "MDS 编码（冗余）"),
            // entity.masterdemandscheduleline.mdscode
            new TranslationSeedItem("entity.masterdemandscheduleline.mdscode", "zh-CN", "MDS编码", "MDS 编码（冗余）"),
            // entity.masterdemandscheduleline.mdscode
            new TranslationSeedItem("entity.masterdemandscheduleline.mdscode", "zh-HK", "MDS编码_hk", "MDS 编码（冗余）"),

            // entity.masterdemandscheduleline.linenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.masterdemandscheduleline.linenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.masterdemandscheduleline.linenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.masterdemandscheduleline.linenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.masterdemandscheduleline.demandsourcetype
            new TranslationSeedItem("entity.masterdemandscheduleline.demandsourcetype", "en-US", "需求来源_us", "需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）"),
            // entity.masterdemandscheduleline.demandsourcetype
            new TranslationSeedItem("entity.masterdemandscheduleline.demandsourcetype", "ja-JP", "需求来源_jp", "需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）"),
            // entity.masterdemandscheduleline.demandsourcetype
            new TranslationSeedItem("entity.masterdemandscheduleline.demandsourcetype", "zh-CN", "需求来源", "需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）"),
            // entity.masterdemandscheduleline.demandsourcetype
            new TranslationSeedItem("entity.masterdemandscheduleline.demandsourcetype", "zh-HK", "需求来源_hk", "需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）"),

            // entity.masterdemandscheduleline.salesorderid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderid", "en-US", "来源销售订单ID_us", "来源销售订单 ID（可选）"),
            // entity.masterdemandscheduleline.salesorderid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderid", "ja-JP", "来源销售订单ID_jp", "来源销售订单 ID（可选）"),
            // entity.masterdemandscheduleline.salesorderid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderid", "zh-CN", "来源销售订单ID", "来源销售订单 ID（可选）"),
            // entity.masterdemandscheduleline.salesorderid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderid", "zh-HK", "来源销售订单ID_hk", "来源销售订单 ID（可选）"),

            // entity.masterdemandscheduleline.salesorderlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderlinenumber", "en-US", "来源销售订单行号_us", "来源销售订单行号（可选；与 SalesOrderId 成对）"),
            // entity.masterdemandscheduleline.salesorderlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderlinenumber", "ja-JP", "来源销售订单行号_jp", "来源销售订单行号（可选；与 SalesOrderId 成对）"),
            // entity.masterdemandscheduleline.salesorderlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderlinenumber", "zh-CN", "来源销售订单行号", "来源销售订单行号（可选；与 SalesOrderId 成对）"),
            // entity.masterdemandscheduleline.salesorderlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesorderlinenumber", "zh-HK", "来源销售订单行号_hk", "来源销售订单行号（可选；与 SalesOrderId 成对）"),

            // entity.masterdemandscheduleline.salesforecastid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastid", "en-US", "来源销售预测ID_us", "来源销售预测 ID（可选；预测/计划类需求）"),
            // entity.masterdemandscheduleline.salesforecastid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastid", "ja-JP", "来源销售预测ID_jp", "来源销售预测 ID（可选；预测/计划类需求）"),
            // entity.masterdemandscheduleline.salesforecastid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastid", "zh-CN", "来源销售预测ID", "来源销售预测 ID（可选；预测/计划类需求）"),
            // entity.masterdemandscheduleline.salesforecastid
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastid", "zh-HK", "来源销售预测ID_hk", "来源销售预测 ID（可选；预测/计划类需求）"),

            // entity.masterdemandscheduleline.salesforecastlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastlinenumber", "en-US", "来源销售预测行号_us", "来源销售预测行号（可选；与 SalesForecastId 成对）"),
            // entity.masterdemandscheduleline.salesforecastlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastlinenumber", "ja-JP", "来源销售预测行号_jp", "来源销售预测行号（可选；与 SalesForecastId 成对）"),
            // entity.masterdemandscheduleline.salesforecastlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastlinenumber", "zh-CN", "来源销售预测行号", "来源销售预测行号（可选；与 SalesForecastId 成对）"),
            // entity.masterdemandscheduleline.salesforecastlinenumber
            new TranslationSeedItem("entity.masterdemandscheduleline.salesforecastlinenumber", "zh-HK", "来源销售预测行号_hk", "来源销售预测行号（可选；与 SalesForecastId 成对）"),

            // entity.masterdemandscheduleline.materialcode
            new TranslationSeedItem("entity.masterdemandscheduleline.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.masterdemandscheduleline.materialcode
            new TranslationSeedItem("entity.masterdemandscheduleline.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.masterdemandscheduleline.materialcode
            new TranslationSeedItem("entity.masterdemandscheduleline.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.masterdemandscheduleline.materialcode
            new TranslationSeedItem("entity.masterdemandscheduleline.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.masterdemandscheduleline.bucketstart
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketstart", "en-US", "时间桶开始_us", "时间桶开始"),
            // entity.masterdemandscheduleline.bucketstart
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketstart", "ja-JP", "时间桶开始_jp", "时间桶开始"),
            // entity.masterdemandscheduleline.bucketstart
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketstart", "zh-CN", "时间桶开始", "时间桶开始"),
            // entity.masterdemandscheduleline.bucketstart
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketstart", "zh-HK", "时间桶开始_hk", "时间桶开始"),

            // entity.masterdemandscheduleline.bucketend
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketend", "en-US", "时间桶结束_us", "时间桶结束"),
            // entity.masterdemandscheduleline.bucketend
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketend", "ja-JP", "时间桶结束_jp", "时间桶结束"),
            // entity.masterdemandscheduleline.bucketend
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketend", "zh-CN", "时间桶结束", "时间桶结束"),
            // entity.masterdemandscheduleline.bucketend
            new TranslationSeedItem("entity.masterdemandscheduleline.bucketend", "zh-HK", "时间桶结束_hk", "时间桶结束"),

            // entity.masterdemandscheduleline.demandquantity
            new TranslationSeedItem("entity.masterdemandscheduleline.demandquantity", "en-US", "需求数量_us", "需求数量（基本单位）"),
            // entity.masterdemandscheduleline.demandquantity
            new TranslationSeedItem("entity.masterdemandscheduleline.demandquantity", "ja-JP", "需求数量_jp", "需求数量（基本单位）"),
            // entity.masterdemandscheduleline.demandquantity
            new TranslationSeedItem("entity.masterdemandscheduleline.demandquantity", "zh-CN", "需求数量", "需求数量（基本单位）"),
            // entity.masterdemandscheduleline.demandquantity
            new TranslationSeedItem("entity.masterdemandscheduleline.demandquantity", "zh-HK", "需求数量_hk", "需求数量（基本单位）"),

            // entity.masterdemandscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterdemandscheduleline.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.masterdemandscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterdemandscheduleline.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.masterdemandscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterdemandscheduleline.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.masterdemandscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterdemandscheduleline.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.masterdemandscheduleline.isobsolete
            new TranslationSeedItem("entity.masterdemandscheduleline.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.masterdemandscheduleline.isobsolete
            new TranslationSeedItem("entity.masterdemandscheduleline.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.masterdemandscheduleline.isobsolete
            new TranslationSeedItem("entity.masterdemandscheduleline.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.masterdemandscheduleline.isobsolete
            new TranslationSeedItem("entity.masterdemandscheduleline.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mds";
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
