// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionPlanItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp;

/// <summary>
/// TaktProductionPlanItem 实体国际化翻译种子（键前缀 entity.productionplanitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionPlanItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionPlanItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionplanitem 实体翻译...", tenantCode);

        foreach (var item in GetProductionPlanItemTranslations())
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

        TaktLogger.Information("TaktProductionPlanItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionPlanItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionplanitem._self / entity.productionplanitem.{{field}}；ResourceGroup=Mrp；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionPlanItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "en-US", "Production Plan Item Information_us", "实体名称"),
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "ja-JP", "Takt生产计划明细信息_jp", "实体名称"),
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "zh-CN", "Takt生产计划明细信息", "实体名称"),
            // entity.productionplanitem._self
            new TranslationSeedItem("entity.productionplanitem._self", "zh-HK", "Takt生产计划明细信息_hk", "实体名称"),

            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "en-US", "生产计划ID_us", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "ja-JP", "生产计划ID_jp", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "zh-CN", "生产计划ID", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.productionplanid
            new TranslationSeedItem("entity.productionplanitem.productionplanid", "zh-HK", "生产计划ID_hk", "生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "en-US", "生产计划编码_us", "生产计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "ja-JP", "生产计划编码_jp", "生产计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "zh-CN", "生产计划编码", "生产计划编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.productionplanitem.productionplancode
            new TranslationSeedItem("entity.productionplanitem.productionplancode", "zh-HK", "生产计划编码_hk", "生产计划编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.productionplanitem.linenumber
            new TranslationSeedItem("entity.productionplanitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.productionplanitem.salesforecastid
            new TranslationSeedItem("entity.productionplanitem.salesforecastid", "en-US", "来源销售计划ID_us", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.salesforecastid
            new TranslationSeedItem("entity.productionplanitem.salesforecastid", "ja-JP", "来源销售计划ID_jp", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.salesforecastid
            new TranslationSeedItem("entity.productionplanitem.salesforecastid", "zh-CN", "来源销售计划ID", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.productionplanitem.salesforecastid
            new TranslationSeedItem("entity.productionplanitem.salesforecastid", "zh-HK", "来源销售计划ID_hk", "来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.productionplanitem.salesforecastcode
            new TranslationSeedItem("entity.productionplanitem.salesforecastcode", "en-US", "来源销售计划编码_us", "来源销售计划编码"),
            // entity.productionplanitem.salesforecastcode
            new TranslationSeedItem("entity.productionplanitem.salesforecastcode", "ja-JP", "来源销售计划编码_jp", "来源销售计划编码"),
            // entity.productionplanitem.salesforecastcode
            new TranslationSeedItem("entity.productionplanitem.salesforecastcode", "zh-CN", "来源销售计划编码", "来源销售计划编码"),
            // entity.productionplanitem.salesforecastcode
            new TranslationSeedItem("entity.productionplanitem.salesforecastcode", "zh-HK", "来源销售计划编码_hk", "来源销售计划编码"),

            // entity.productionplanitem.salesforecastlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesforecastlinenumber", "en-US", "来源销售计划行号_us", "来源销售计划行号"),
            // entity.productionplanitem.salesforecastlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesforecastlinenumber", "ja-JP", "来源销售计划行号_jp", "来源销售计划行号"),
            // entity.productionplanitem.salesforecastlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesforecastlinenumber", "zh-CN", "来源销售计划行号", "来源销售计划行号"),
            // entity.productionplanitem.salesforecastlinenumber
            new TranslationSeedItem("entity.productionplanitem.salesforecastlinenumber", "zh-HK", "来源销售计划行号_hk", "来源销售计划行号"),

            // entity.productionplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.productionplanitem.materialrequirementsplanningitemid", "en-US", "来源MRP明细ID_us", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),
            // entity.productionplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.productionplanitem.materialrequirementsplanningitemid", "ja-JP", "来源MRP明细ID_jp", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),
            // entity.productionplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.productionplanitem.materialrequirementsplanningitemid", "zh-CN", "来源MRP明细ID", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),
            // entity.productionplanitem.materialrequirementsplanningitemid
            new TranslationSeedItem("entity.productionplanitem.materialrequirementsplanningitemid", "zh-HK", "来源MRP明细ID_hk", "来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）"),

            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.productionplanitem.materialcode
            new TranslationSeedItem("entity.productionplanitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.productionplanitem.materialdescription
            new TranslationSeedItem("entity.productionplanitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.productionplanitem.materialdescription
            new TranslationSeedItem("entity.productionplanitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.productionplanitem.materialdescription
            new TranslationSeedItem("entity.productionplanitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.productionplanitem.materialdescription
            new TranslationSeedItem("entity.productionplanitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.productionplanitem.materialspecification
            new TranslationSeedItem("entity.productionplanitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.productionplanitem.modelcode
            new TranslationSeedItem("entity.productionplanitem.modelcode", "en-US", "机种编码_us", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),
            // entity.productionplanitem.modelcode
            new TranslationSeedItem("entity.productionplanitem.modelcode", "ja-JP", "机种编码_jp", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),
            // entity.productionplanitem.modelcode
            new TranslationSeedItem("entity.productionplanitem.modelcode", "zh-CN", "机种编码", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),
            // entity.productionplanitem.modelcode
            new TranslationSeedItem("entity.productionplanitem.modelcode", "zh-HK", "机种编码_hk", "机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）"),

            // entity.productionplanitem.modelname
            new TranslationSeedItem("entity.productionplanitem.modelname", "en-US", "机种名称_us", "机种名称（冗余字段，便于查询展示）"),
            // entity.productionplanitem.modelname
            new TranslationSeedItem("entity.productionplanitem.modelname", "ja-JP", "机种名称_jp", "机种名称（冗余字段，便于查询展示）"),
            // entity.productionplanitem.modelname
            new TranslationSeedItem("entity.productionplanitem.modelname", "zh-CN", "机种名称", "机种名称（冗余字段，便于查询展示）"),
            // entity.productionplanitem.modelname
            new TranslationSeedItem("entity.productionplanitem.modelname", "zh-HK", "机种名称_hk", "机种名称（冗余字段，便于查询展示）"),

            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "en-US", "计划单位_us", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "ja-JP", "计划单位_jp", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "zh-CN", "计划单位", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.productionplanitem.planunit
            new TranslationSeedItem("entity.productionplanitem.planunit", "zh-HK", "计划单位_hk", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "en-US", "计划数量_us", "计划数量（基本单位数量）"),
            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "ja-JP", "计划数量_jp", "计划数量（基本单位数量）"),
            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "zh-CN", "计划数量", "计划数量（基本单位数量）"),
            // entity.productionplanitem.planquantity
            new TranslationSeedItem("entity.productionplanitem.planquantity", "zh-HK", "计划数量_hk", "计划数量（基本单位数量）"),

            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "en-US", "计划开工日期_us", "计划开工日期"),
            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "ja-JP", "计划开工日期_jp", "计划开工日期"),
            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "zh-CN", "计划开工日期", "计划开工日期"),
            // entity.productionplanitem.plannedstartdate
            new TranslationSeedItem("entity.productionplanitem.plannedstartdate", "zh-HK", "计划开工日期_hk", "计划开工日期"),

            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "en-US", "计划完工日期_us", "计划完工日期"),
            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "ja-JP", "计划完工日期_jp", "计划完工日期"),
            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "zh-CN", "计划完工日期", "计划完工日期"),
            // entity.productionplanitem.plannedenddate
            new TranslationSeedItem("entity.productionplanitem.plannedenddate", "zh-HK", "计划完工日期_hk", "计划完工日期"),

            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "en-US", "已转工单采购数量_us", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "ja-JP", "已转工单采购数量_jp", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "zh-CN", "已转工单采购数量", "已转工单/采购数量（基本单位数量）"),
            // entity.productionplanitem.convertedquantity
            new TranslationSeedItem("entity.productionplanitem.convertedquantity", "zh-HK", "已转工单采购数量_hk", "已转工单/采购数量（基本单位数量）"),

            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "en-US", "预计单位成本_us", "预计单位成本"),
            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "ja-JP", "预计单位成本_jp", "预计单位成本"),
            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "zh-CN", "预计单位成本", "预计单位成本"),
            // entity.productionplanitem.estimatedunitcost
            new TranslationSeedItem("entity.productionplanitem.estimatedunitcost", "zh-HK", "预计单位成本_hk", "预计单位成本"),

            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.productionplanitem.estimatedamount
            new TranslationSeedItem("entity.productionplanitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),

            // entity.productionplanitem.isobsolete
            new TranslationSeedItem("entity.productionplanitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.productionplanitem.isobsolete
            new TranslationSeedItem("entity.productionplanitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.productionplanitem.isobsolete
            new TranslationSeedItem("entity.productionplanitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.productionplanitem.isobsolete
            new TranslationSeedItem("entity.productionplanitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mrp";
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
