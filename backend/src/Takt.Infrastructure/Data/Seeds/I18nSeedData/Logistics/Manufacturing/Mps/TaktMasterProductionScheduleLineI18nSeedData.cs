// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleLineI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMasterProductionScheduleLine 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mps;

/// <summary>
/// TaktMasterProductionScheduleLine 实体国际化翻译种子（键前缀 entity.masterproductionscheduleline.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMasterProductionScheduleLineI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMasterProductionScheduleLine 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 masterproductionscheduleline 实体翻译...", tenantCode);

        foreach (var item in GetMasterProductionScheduleLineTranslations())
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

        TaktLogger.Information("TaktMasterProductionScheduleLine 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMasterProductionScheduleLine 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.masterproductionscheduleline._self / entity.masterproductionscheduleline.{{field}}；ResourceGroup=Mps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMasterProductionScheduleLineTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.masterproductionscheduleline._self
            new TranslationSeedItem("entity.masterproductionscheduleline._self", "en-US", "Master Production Schedule Line Information_us", "实体名称"),
            // entity.masterproductionscheduleline._self
            new TranslationSeedItem("entity.masterproductionscheduleline._self", "ja-JP", "主生产计划 MPS 行信息_jp", "实体名称"),
            // entity.masterproductionscheduleline._self
            new TranslationSeedItem("entity.masterproductionscheduleline._self", "zh-CN", "主生产计划 MPS 行信息", "实体名称"),
            // entity.masterproductionscheduleline._self
            new TranslationSeedItem("entity.masterproductionscheduleline._self", "zh-HK", "主生产计划 MPS 行信息_hk", "实体名称"),

            // entity.masterproductionscheduleline.masterproductionscheduleid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterproductionscheduleid", "en-US", "MPS头表ID_us", "MPS 头表 ID（主子表关系）"),
            // entity.masterproductionscheduleline.masterproductionscheduleid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterproductionscheduleid", "ja-JP", "MPS头表ID_jp", "MPS 头表 ID（主子表关系）"),
            // entity.masterproductionscheduleline.masterproductionscheduleid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterproductionscheduleid", "zh-CN", "MPS头表ID", "MPS 头表 ID（主子表关系）"),
            // entity.masterproductionscheduleline.masterproductionscheduleid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterproductionscheduleid", "zh-HK", "MPS头表ID_hk", "MPS 头表 ID（主子表关系）"),

            // entity.masterproductionscheduleline.mpscode
            new TranslationSeedItem("entity.masterproductionscheduleline.mpscode", "en-US", "MPS编码_us", "MPS 编码（冗余）"),
            // entity.masterproductionscheduleline.mpscode
            new TranslationSeedItem("entity.masterproductionscheduleline.mpscode", "ja-JP", "MPS编码_jp", "MPS 编码（冗余）"),
            // entity.masterproductionscheduleline.mpscode
            new TranslationSeedItem("entity.masterproductionscheduleline.mpscode", "zh-CN", "MPS编码", "MPS 编码（冗余）"),
            // entity.masterproductionscheduleline.mpscode
            new TranslationSeedItem("entity.masterproductionscheduleline.mpscode", "zh-HK", "MPS编码_hk", "MPS 编码（冗余）"),

            // entity.masterproductionscheduleline.linenumber
            new TranslationSeedItem("entity.masterproductionscheduleline.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.masterproductionscheduleline.linenumber
            new TranslationSeedItem("entity.masterproductionscheduleline.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.masterproductionscheduleline.linenumber
            new TranslationSeedItem("entity.masterproductionscheduleline.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.masterproductionscheduleline.linenumber
            new TranslationSeedItem("entity.masterproductionscheduleline.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.masterproductionscheduleline.masterdemandschedulelineid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterdemandschedulelineid", "en-US", "来源MDS行ID_us", "来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）"),
            // entity.masterproductionscheduleline.masterdemandschedulelineid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterdemandschedulelineid", "ja-JP", "来源MDS行ID_jp", "来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）"),
            // entity.masterproductionscheduleline.masterdemandschedulelineid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterdemandschedulelineid", "zh-CN", "来源MDS行ID", "来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）"),
            // entity.masterproductionscheduleline.masterdemandschedulelineid
            new TranslationSeedItem("entity.masterproductionscheduleline.masterdemandschedulelineid", "zh-HK", "来源MDS行ID_hk", "来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）"),

            // entity.masterproductionscheduleline.materialcode
            new TranslationSeedItem("entity.masterproductionscheduleline.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.masterproductionscheduleline.materialcode
            new TranslationSeedItem("entity.masterproductionscheduleline.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.masterproductionscheduleline.materialcode
            new TranslationSeedItem("entity.masterproductionscheduleline.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.masterproductionscheduleline.materialcode
            new TranslationSeedItem("entity.masterproductionscheduleline.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.masterproductionscheduleline.bucketstart
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketstart", "en-US", "时间桶开始_us", "时间桶开始"),
            // entity.masterproductionscheduleline.bucketstart
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketstart", "ja-JP", "时间桶开始_jp", "时间桶开始"),
            // entity.masterproductionscheduleline.bucketstart
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketstart", "zh-CN", "时间桶开始", "时间桶开始"),
            // entity.masterproductionscheduleline.bucketstart
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketstart", "zh-HK", "时间桶开始_hk", "时间桶开始"),

            // entity.masterproductionscheduleline.bucketend
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketend", "en-US", "时间桶结束_us", "时间桶结束"),
            // entity.masterproductionscheduleline.bucketend
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketend", "ja-JP", "时间桶结束_jp", "时间桶结束"),
            // entity.masterproductionscheduleline.bucketend
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketend", "zh-CN", "时间桶结束", "时间桶结束"),
            // entity.masterproductionscheduleline.bucketend
            new TranslationSeedItem("entity.masterproductionscheduleline.bucketend", "zh-HK", "时间桶结束_hk", "时间桶结束"),

            // entity.masterproductionscheduleline.grossrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.grossrequirement", "en-US", "毛需求数量_us", "毛需求数量"),
            // entity.masterproductionscheduleline.grossrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.grossrequirement", "ja-JP", "毛需求数量_jp", "毛需求数量"),
            // entity.masterproductionscheduleline.grossrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.grossrequirement", "zh-CN", "毛需求数量", "毛需求数量"),
            // entity.masterproductionscheduleline.grossrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.grossrequirement", "zh-HK", "毛需求数量_hk", "毛需求数量"),

            // entity.masterproductionscheduleline.scheduledreceipts
            new TranslationSeedItem("entity.masterproductionscheduleline.scheduledreceipts", "en-US", "预计入库_us", "预计入库（计划接收）"),
            // entity.masterproductionscheduleline.scheduledreceipts
            new TranslationSeedItem("entity.masterproductionscheduleline.scheduledreceipts", "ja-JP", "预计入库_jp", "预计入库（计划接收）"),
            // entity.masterproductionscheduleline.scheduledreceipts
            new TranslationSeedItem("entity.masterproductionscheduleline.scheduledreceipts", "zh-CN", "预计入库", "预计入库（计划接收）"),
            // entity.masterproductionscheduleline.scheduledreceipts
            new TranslationSeedItem("entity.masterproductionscheduleline.scheduledreceipts", "zh-HK", "预计入库_hk", "预计入库（计划接收）"),

            // entity.masterproductionscheduleline.projectedonhand
            new TranslationSeedItem("entity.masterproductionscheduleline.projectedonhand", "en-US", "预计可用库存_us", "预计可用库存（期初预计库存）"),
            // entity.masterproductionscheduleline.projectedonhand
            new TranslationSeedItem("entity.masterproductionscheduleline.projectedonhand", "ja-JP", "预计可用库存_jp", "预计可用库存（期初预计库存）"),
            // entity.masterproductionscheduleline.projectedonhand
            new TranslationSeedItem("entity.masterproductionscheduleline.projectedonhand", "zh-CN", "预计可用库存", "预计可用库存（期初预计库存）"),
            // entity.masterproductionscheduleline.projectedonhand
            new TranslationSeedItem("entity.masterproductionscheduleline.projectedonhand", "zh-HK", "预计可用库存_hk", "预计可用库存（期初预计库存）"),

            // entity.masterproductionscheduleline.netrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.netrequirement", "en-US", "净需求数量_us", "净需求数量"),
            // entity.masterproductionscheduleline.netrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.netrequirement", "ja-JP", "净需求数量_jp", "净需求数量"),
            // entity.masterproductionscheduleline.netrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.netrequirement", "zh-CN", "净需求数量", "净需求数量"),
            // entity.masterproductionscheduleline.netrequirement
            new TranslationSeedItem("entity.masterproductionscheduleline.netrequirement", "zh-HK", "净需求数量_hk", "净需求数量"),

            // entity.masterproductionscheduleline.plannedorderquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.plannedorderquantity", "en-US", "计划订单数量_us", "计划订单数量（MPS 产出，供 MRP 展开）"),
            // entity.masterproductionscheduleline.plannedorderquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.plannedorderquantity", "ja-JP", "计划订单数量_jp", "计划订单数量（MPS 产出，供 MRP 展开）"),
            // entity.masterproductionscheduleline.plannedorderquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.plannedorderquantity", "zh-CN", "计划订单数量", "计划订单数量（MPS 产出，供 MRP 展开）"),
            // entity.masterproductionscheduleline.plannedorderquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.plannedorderquantity", "zh-HK", "计划订单数量_hk", "计划订单数量（MPS 产出，供 MRP 展开）"),

            // entity.masterproductionscheduleline.atpquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.atpquantity", "en-US", "可承诺量ATP_us", "可承诺量 ATP"),
            // entity.masterproductionscheduleline.atpquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.atpquantity", "ja-JP", "可承诺量ATP_jp", "可承诺量 ATP"),
            // entity.masterproductionscheduleline.atpquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.atpquantity", "zh-CN", "可承诺量ATP", "可承诺量 ATP"),
            // entity.masterproductionscheduleline.atpquantity
            new TranslationSeedItem("entity.masterproductionscheduleline.atpquantity", "zh-HK", "可承诺量ATP_hk", "可承诺量 ATP"),

            // entity.masterproductionscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterproductionscheduleline.unitofmeasure", "en-US", "计量单位_us", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.masterproductionscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterproductionscheduleline.unitofmeasure", "ja-JP", "计量单位_jp", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.masterproductionscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterproductionscheduleline.unitofmeasure", "zh-CN", "计量单位", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.masterproductionscheduleline.unitofmeasure
            new TranslationSeedItem("entity.masterproductionscheduleline.unitofmeasure", "zh-HK", "计量单位_hk", "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.masterproductionscheduleline.isobsolete
            new TranslationSeedItem("entity.masterproductionscheduleline.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.masterproductionscheduleline.isobsolete
            new TranslationSeedItem("entity.masterproductionscheduleline.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.masterproductionscheduleline.isobsolete
            new TranslationSeedItem("entity.masterproductionscheduleline.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.masterproductionscheduleline.isobsolete
            new TranslationSeedItem("entity.masterproductionscheduleline.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mps";
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
