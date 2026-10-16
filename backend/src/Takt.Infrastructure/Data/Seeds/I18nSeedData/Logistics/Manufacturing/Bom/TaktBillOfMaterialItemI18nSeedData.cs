// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBillOfMaterialItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom;

/// <summary>
/// TaktBillOfMaterialItem 实体国际化翻译种子（键前缀 entity.billofmaterialitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBillOfMaterialItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBillOfMaterialItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 billofmaterialitem 实体翻译...", tenantCode);

        foreach (var item in GetBillOfMaterialItemTranslations())
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

        TaktLogger.Information("TaktBillOfMaterialItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBillOfMaterialItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.billofmaterialitem._self / entity.billofmaterialitem.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBillOfMaterialItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "en-US", "Bill Of Material Item Information_us", "实体名称"),
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "ja-JP", "Takt物料清单明细信息_jp", "实体名称"),
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "zh-CN", "Takt物料清单明细信息", "实体名称"),
            // entity.billofmaterialitem._self
            new TranslationSeedItem("entity.billofmaterialitem._self", "zh-HK", "Takt物料清单明细信息_hk", "实体名称"),

            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "en-US", "物料清单ID_us", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "ja-JP", "物料清单ID_jp", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "zh-CN", "物料清单ID", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialitem.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialitem.billofmaterialid", "zh-HK", "物料清单ID_hk", "物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）"),

            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "en-US", "BOM编码_us", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "ja-JP", "BOM编码_jp", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "zh-CN", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialitem.bomcode
            new TranslationSeedItem("entity.billofmaterialitem.bomcode", "zh-HK", "BOM编码_hk", "BOM编码（冗余，便于查询）"),

            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "en-US", "行号_us", "行号（项号，步长10：10/20/30…）"),
            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "ja-JP", "行号_jp", "行号（项号，步长10：10/20/30…）"),
            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "zh-CN", "行号", "行号（项号，步长10：10/20/30…）"),
            // entity.billofmaterialitem.linenumber
            new TranslationSeedItem("entity.billofmaterialitem.linenumber", "zh-HK", "行号_hk", "行号（项号，步长10：10/20/30…）"),

            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "en-US", "子项物料编码_us", "子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "ja-JP", "子项物料编码_jp", "子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "zh-CN", "子项物料编码", "子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.billofmaterialitem.materialcode
            new TranslationSeedItem("entity.billofmaterialitem.materialcode", "zh-HK", "子项物料编码_hk", "子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.billofmaterialitem.materialdescription
            new TranslationSeedItem("entity.billofmaterialitem.materialdescription", "en-US", "子项物料描述_us", "子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.billofmaterialitem.materialdescription
            new TranslationSeedItem("entity.billofmaterialitem.materialdescription", "ja-JP", "子项物料描述_jp", "子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.billofmaterialitem.materialdescription
            new TranslationSeedItem("entity.billofmaterialitem.materialdescription", "zh-CN", "子项物料描述", "子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.billofmaterialitem.materialdescription
            new TranslationSeedItem("entity.billofmaterialitem.materialdescription", "zh-HK", "子项物料描述_hk", "子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "en-US", "用量_us", "用量（quantity）"),
            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "ja-JP", "用量_jp", "用量（quantity）"),
            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "zh-CN", "用量", "用量（quantity）"),
            // entity.billofmaterialitem.usagequantity
            new TranslationSeedItem("entity.billofmaterialitem.usagequantity", "zh-HK", "用量_hk", "用量（quantity）"),

            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "en-US", "单位_us", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "ja-JP", "单位_jp", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "zh-CN", "单位", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterialitem.materialunit
            new TranslationSeedItem("entity.billofmaterialitem.materialunit", "zh-HK", "单位_hk", "单位（字典 logistics_materials_unit_of_measure_code）"),

            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "en-US", "损耗率_us", "损耗率（0-100，scrap_rate）"),
            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "ja-JP", "损耗率_jp", "损耗率（0-100，scrap_rate）"),
            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "zh-CN", "损耗率", "损耗率（0-100，scrap_rate）"),
            // entity.billofmaterialitem.scraprate
            new TranslationSeedItem("entity.billofmaterialitem.scraprate", "zh-HK", "损耗率_hk", "损耗率（0-100，scrap_rate）"),

            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "en-US", "实际用量_us", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "ja-JP", "实际用量_jp", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "zh-CN", "实际用量", "实际用量（用量 × (1 + 损耗率/100)）"),
            // entity.billofmaterialitem.actualusagequantity
            new TranslationSeedItem("entity.billofmaterialitem.actualusagequantity", "zh-HK", "实际用量_hk", "实际用量（用量 × (1 + 损耗率/100)）"),

            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "en-US", "工序号_us", "工序号（operation_seq）"),
            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "ja-JP", "工序号_jp", "工序号（operation_seq）"),
            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "zh-CN", "工序号", "工序号（operation_seq）"),
            // entity.billofmaterialitem.operationseq
            new TranslationSeedItem("entity.billofmaterialitem.operationseq", "zh-HK", "工序号_hk", "工序号（operation_seq）"),

            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "en-US", "工作中心_us", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "ja-JP", "工作中心_jp", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "zh-CN", "工作中心", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.billofmaterialitem.workcenter
            new TranslationSeedItem("entity.billofmaterialitem.workcenter", "zh-HK", "工作中心_hk", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),

            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "en-US", "位号_us", "位号（position，PCB位号等）"),
            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "ja-JP", "位号_jp", "位号（position，PCB位号等）"),
            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "zh-CN", "位号", "位号（position，PCB位号等）"),
            // entity.billofmaterialitem.position
            new TranslationSeedItem("entity.billofmaterialitem.position", "zh-HK", "位号_hk", "位号（position，PCB位号等）"),

            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "en-US", "替代组号_us", "替代组号（substitute_group）"),
            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "ja-JP", "替代组号_jp", "替代组号（substitute_group）"),
            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "zh-CN", "替代组号", "替代组号（substitute_group）"),
            // entity.billofmaterialitem.substitutegroup
            new TranslationSeedItem("entity.billofmaterialitem.substitutegroup", "zh-HK", "替代组号_hk", "替代组号（substitute_group）"),

            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "en-US", "替代优先级_us", "替代优先级（组内越小越优先）"),
            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "ja-JP", "替代优先级_jp", "替代优先级（组内越小越优先）"),
            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "zh-CN", "替代优先级", "替代优先级（组内越小越优先）"),
            // entity.billofmaterialitem.substitutepriority
            new TranslationSeedItem("entity.billofmaterialitem.substitutepriority", "zh-HK", "替代优先级_hk", "替代优先级（组内越小越优先）"),

            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "en-US", "是否可选件_us", "是否可选件（字典 sys_yes_no；0=否，1=是）"),
            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "ja-JP", "是否可选件_jp", "是否可选件（字典 sys_yes_no；0=否，1=是）"),
            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "zh-CN", "是否可选件", "是否可选件（字典 sys_yes_no；0=否，1=是）"),
            // entity.billofmaterialitem.isoptional
            new TranslationSeedItem("entity.billofmaterialitem.isoptional", "zh-HK", "是否可选件_hk", "是否可选件（字典 sys_yes_no；0=否，1=是）"),

            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "en-US", "是否虚拟件_us", "是否虚拟件（字典 sys_yes_no；0=否，1=是）"),
            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "ja-JP", "是否虚拟件_jp", "是否虚拟件（字典 sys_yes_no；0=否，1=是）"),
            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "zh-CN", "是否虚拟件", "是否虚拟件（字典 sys_yes_no；0=否，1=是）"),
            // entity.billofmaterialitem.isphantom
            new TranslationSeedItem("entity.billofmaterialitem.isphantom", "zh-HK", "是否虚拟件_hk", "是否虚拟件（字典 sys_yes_no；0=否，1=是）"),

            // entity.billofmaterialitem.isobsolete
            new TranslationSeedItem("entity.billofmaterialitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.billofmaterialitem.isobsolete
            new TranslationSeedItem("entity.billofmaterialitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.billofmaterialitem.isobsolete
            new TranslationSeedItem("entity.billofmaterialitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.billofmaterialitem.isobsolete
            new TranslationSeedItem("entity.billofmaterialitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "en-US", "物料清单_us", "物料清单（BOM头）"),
            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "ja-JP", "物料清单_jp", "物料清单（BOM头）"),
            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "zh-CN", "物料清单", "物料清单（BOM头）"),
            // entity.billofmaterialitem.bom
            new TranslationSeedItem("entity.billofmaterialitem.bom", "zh-HK", "物料清单_hk", "物料清单（BOM头）"),

            // entity.billofmaterialitem.substitutes
            new TranslationSeedItem("entity.billofmaterialitem.substitutes", "en-US", "替代料明细_us", "替代料明细（一行主件可维护多条替代物料）"),
            // entity.billofmaterialitem.substitutes
            new TranslationSeedItem("entity.billofmaterialitem.substitutes", "ja-JP", "替代料明细_jp", "替代料明细（一行主件可维护多条替代物料）"),
            // entity.billofmaterialitem.substitutes
            new TranslationSeedItem("entity.billofmaterialitem.substitutes", "zh-CN", "替代料明细", "替代料明细（一行主件可维护多条替代物料）"),
            // entity.billofmaterialitem.substitutes
            new TranslationSeedItem("entity.billofmaterialitem.substitutes", "zh-HK", "替代料明细_hk", "替代料明细（一行主件可维护多条替代物料）"),
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
        translation.ResourceGroup = "Bom";
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
