// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItemI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialRequirementsPlanningItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialRequirementsPlanningItem 实体国际化翻译种子（键前缀 entity.materialrequirementsplanningitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialRequirementsPlanningItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialRequirementsPlanningItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialrequirementsplanningitem 实体翻译...", tenantCode);

        foreach (var item in GetMaterialRequirementsPlanningItemTranslations())
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

        TaktLogger.Information("TaktMaterialRequirementsPlanningItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialRequirementsPlanningItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialrequirementsplanningitem._self / entity.materialrequirementsplanningitem.{{field}}；ResourceGroup=Mrp；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialRequirementsPlanningItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialrequirementsplanningitem._self
            new TranslationSeedItem("entity.materialrequirementsplanningitem._self", "en-US", "Material Requirements Planning Item Information_us", "实体名称"),
            // entity.materialrequirementsplanningitem._self
            new TranslationSeedItem("entity.materialrequirementsplanningitem._self", "ja-JP", "物料需求计划 MRP 明细行信息_jp", "实体名称"),
            // entity.materialrequirementsplanningitem._self
            new TranslationSeedItem("entity.materialrequirementsplanningitem._self", "zh-CN", "物料需求计划 MRP 明细行信息", "实体名称"),
            // entity.materialrequirementsplanningitem._self
            new TranslationSeedItem("entity.materialrequirementsplanningitem._self", "zh-HK", "物料需求计划 MRP 明细行信息_hk", "实体名称"),

            // entity.materialrequirementsplanningitem.materialrequirementsplanningid
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningid", "en-US", "MRP头表ID_us", "MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.materialrequirementsplanningitem.materialrequirementsplanningid
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningid", "ja-JP", "MRP头表ID_jp", "MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.materialrequirementsplanningitem.materialrequirementsplanningid
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningid", "zh-CN", "MRP头表ID", "MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.materialrequirementsplanningitem.materialrequirementsplanningid
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningid", "zh-HK", "MRP头表ID_hk", "MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.materialrequirementsplanningitem.materialrequirementsplanningcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningcode", "en-US", "MRP编码_us", "MRP 编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.materialrequirementsplanningitem.materialrequirementsplanningcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningcode", "ja-JP", "MRP编码_jp", "MRP 编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.materialrequirementsplanningitem.materialrequirementsplanningcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningcode", "zh-CN", "MRP编码", "MRP 编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.materialrequirementsplanningitem.materialrequirementsplanningcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialrequirementsplanningcode", "zh-HK", "MRP编码_hk", "MRP 编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.materialrequirementsplanningitem.linenumber
            new TranslationSeedItem("entity.materialrequirementsplanningitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.materialrequirementsplanningitem.linenumber
            new TranslationSeedItem("entity.materialrequirementsplanningitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.materialrequirementsplanningitem.linenumber
            new TranslationSeedItem("entity.materialrequirementsplanningitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.materialrequirementsplanningitem.linenumber
            new TranslationSeedItem("entity.materialrequirementsplanningitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.materialrequirementsplanningitem.materialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialrequirementsplanningitem.materialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialrequirementsplanningitem.materialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.materialrequirementsplanningitem.materialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.materialrequirementsplanningitem.materialdescription
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.materialrequirementsplanningitem.materialdescription
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.materialrequirementsplanningitem.materialdescription
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.materialrequirementsplanningitem.materialdescription
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.materialrequirementsplanningitem.materialspecification
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialspecification", "en-US", "物料规格_us", "物料规格（回填：随物料）"),
            // entity.materialrequirementsplanningitem.materialspecification
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialspecification", "ja-JP", "物料规格_jp", "物料规格（回填：随物料）"),
            // entity.materialrequirementsplanningitem.materialspecification
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialspecification", "zh-CN", "物料规格", "物料规格（回填：随物料）"),
            // entity.materialrequirementsplanningitem.materialspecification
            new TranslationSeedItem("entity.materialrequirementsplanningitem.materialspecification", "zh-HK", "物料规格_hk", "物料规格（回填：随物料）"),

            // entity.materialrequirementsplanningitem.modelcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelcode", "en-US", "机种编码_us", "机种编码（关联 TaktModelDestination.ModelCode，可选）"),
            // entity.materialrequirementsplanningitem.modelcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelcode", "ja-JP", "机种编码_jp", "机种编码（关联 TaktModelDestination.ModelCode，可选）"),
            // entity.materialrequirementsplanningitem.modelcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelcode", "zh-CN", "机种编码", "机种编码（关联 TaktModelDestination.ModelCode，可选）"),
            // entity.materialrequirementsplanningitem.modelcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelcode", "zh-HK", "机种编码_hk", "机种编码（关联 TaktModelDestination.ModelCode，可选）"),

            // entity.materialrequirementsplanningitem.modelname
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelname", "en-US", "机种名称_us", "机种名称（冗余）"),
            // entity.materialrequirementsplanningitem.modelname
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelname", "ja-JP", "机种名称_jp", "机种名称（冗余）"),
            // entity.materialrequirementsplanningitem.modelname
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelname", "zh-CN", "机种名称", "机种名称（冗余）"),
            // entity.materialrequirementsplanningitem.modelname
            new TranslationSeedItem("entity.materialrequirementsplanningitem.modelname", "zh-HK", "机种名称_hk", "机种名称（冗余）"),

            // entity.materialrequirementsplanningitem.parentmaterialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.parentmaterialcode", "en-US", "父项物料编码_us", "父项物料编码（BOM 展开上级，可选）"),
            // entity.materialrequirementsplanningitem.parentmaterialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.parentmaterialcode", "ja-JP", "父项物料编码_jp", "父项物料编码（BOM 展开上级，可选）"),
            // entity.materialrequirementsplanningitem.parentmaterialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.parentmaterialcode", "zh-CN", "父项物料编码", "父项物料编码（BOM 展开上级，可选）"),
            // entity.materialrequirementsplanningitem.parentmaterialcode
            new TranslationSeedItem("entity.materialrequirementsplanningitem.parentmaterialcode", "zh-HK", "父项物料编码_hk", "父项物料编码（BOM 展开上级，可选）"),

            // entity.materialrequirementsplanningitem.bomlevel
            new TranslationSeedItem("entity.materialrequirementsplanningitem.bomlevel", "en-US", "BOM层级_us", "BOM 层级（1=顶层成品）"),
            // entity.materialrequirementsplanningitem.bomlevel
            new TranslationSeedItem("entity.materialrequirementsplanningitem.bomlevel", "ja-JP", "BOM层级_jp", "BOM 层级（1=顶层成品）"),
            // entity.materialrequirementsplanningitem.bomlevel
            new TranslationSeedItem("entity.materialrequirementsplanningitem.bomlevel", "zh-CN", "BOM层级", "BOM 层级（1=顶层成品）"),
            // entity.materialrequirementsplanningitem.bomlevel
            new TranslationSeedItem("entity.materialrequirementsplanningitem.bomlevel", "zh-HK", "BOM层级_hk", "BOM 层级（1=顶层成品）"),

            // entity.materialrequirementsplanningitem.requirementdate
            new TranslationSeedItem("entity.materialrequirementsplanningitem.requirementdate", "en-US", "需求日期_us", "需求日期"),
            // entity.materialrequirementsplanningitem.requirementdate
            new TranslationSeedItem("entity.materialrequirementsplanningitem.requirementdate", "ja-JP", "需求日期_jp", "需求日期"),
            // entity.materialrequirementsplanningitem.requirementdate
            new TranslationSeedItem("entity.materialrequirementsplanningitem.requirementdate", "zh-CN", "需求日期", "需求日期"),
            // entity.materialrequirementsplanningitem.requirementdate
            new TranslationSeedItem("entity.materialrequirementsplanningitem.requirementdate", "zh-HK", "需求日期_hk", "需求日期"),

            // entity.materialrequirementsplanningitem.planunit
            new TranslationSeedItem("entity.materialrequirementsplanningitem.planunit", "en-US", "计划单位_us", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.materialrequirementsplanningitem.planunit
            new TranslationSeedItem("entity.materialrequirementsplanningitem.planunit", "ja-JP", "计划单位_jp", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.materialrequirementsplanningitem.planunit
            new TranslationSeedItem("entity.materialrequirementsplanningitem.planunit", "zh-CN", "计划单位", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.materialrequirementsplanningitem.planunit
            new TranslationSeedItem("entity.materialrequirementsplanningitem.planunit", "zh-HK", "计划单位_hk", "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.materialrequirementsplanningitem.grossrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.grossrequirement", "en-US", "毛需求数量_us", "毛需求数量（基本单位数量）"),
            // entity.materialrequirementsplanningitem.grossrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.grossrequirement", "ja-JP", "毛需求数量_jp", "毛需求数量（基本单位数量）"),
            // entity.materialrequirementsplanningitem.grossrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.grossrequirement", "zh-CN", "毛需求数量", "毛需求数量（基本单位数量）"),
            // entity.materialrequirementsplanningitem.grossrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.grossrequirement", "zh-HK", "毛需求数量_hk", "毛需求数量（基本单位数量）"),

            // entity.materialrequirementsplanningitem.scheduledreceipts
            new TranslationSeedItem("entity.materialrequirementsplanningitem.scheduledreceipts", "en-US", "计划接收数量_us", "计划接收数量（在途/已订未收等，运算快照）"),
            // entity.materialrequirementsplanningitem.scheduledreceipts
            new TranslationSeedItem("entity.materialrequirementsplanningitem.scheduledreceipts", "ja-JP", "计划接收数量_jp", "计划接收数量（在途/已订未收等，运算快照）"),
            // entity.materialrequirementsplanningitem.scheduledreceipts
            new TranslationSeedItem("entity.materialrequirementsplanningitem.scheduledreceipts", "zh-CN", "计划接收数量", "计划接收数量（在途/已订未收等，运算快照）"),
            // entity.materialrequirementsplanningitem.scheduledreceipts
            new TranslationSeedItem("entity.materialrequirementsplanningitem.scheduledreceipts", "zh-HK", "计划接收数量_hk", "计划接收数量（在途/已订未收等，运算快照）"),

            // entity.materialrequirementsplanningitem.onhandquantity
            new TranslationSeedItem("entity.materialrequirementsplanningitem.onhandquantity", "en-US", "现有库存数量_us", "现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）"),
            // entity.materialrequirementsplanningitem.onhandquantity
            new TranslationSeedItem("entity.materialrequirementsplanningitem.onhandquantity", "ja-JP", "现有库存数量_jp", "现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）"),
            // entity.materialrequirementsplanningitem.onhandquantity
            new TranslationSeedItem("entity.materialrequirementsplanningitem.onhandquantity", "zh-CN", "现有库存数量", "现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）"),
            // entity.materialrequirementsplanningitem.onhandquantity
            new TranslationSeedItem("entity.materialrequirementsplanningitem.onhandquantity", "zh-HK", "现有库存数量_hk", "现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）"),

            // entity.materialrequirementsplanningitem.projectedonhand
            new TranslationSeedItem("entity.materialrequirementsplanningitem.projectedonhand", "en-US", "预计可用库存_us", "预计可用库存（运算后 POH 快照）"),
            // entity.materialrequirementsplanningitem.projectedonhand
            new TranslationSeedItem("entity.materialrequirementsplanningitem.projectedonhand", "ja-JP", "预计可用库存_jp", "预计可用库存（运算后 POH 快照）"),
            // entity.materialrequirementsplanningitem.projectedonhand
            new TranslationSeedItem("entity.materialrequirementsplanningitem.projectedonhand", "zh-CN", "预计可用库存", "预计可用库存（运算后 POH 快照）"),
            // entity.materialrequirementsplanningitem.projectedonhand
            new TranslationSeedItem("entity.materialrequirementsplanningitem.projectedonhand", "zh-HK", "预计可用库存_hk", "预计可用库存（运算后 POH 快照）"),

            // entity.materialrequirementsplanningitem.netrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.netrequirement", "en-US", "净需求数量_us", "净需求数量（基本单位数量）"),
            // entity.materialrequirementsplanningitem.netrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.netrequirement", "ja-JP", "净需求数量_jp", "净需求数量（基本单位数量）"),
            // entity.materialrequirementsplanningitem.netrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.netrequirement", "zh-CN", "净需求数量", "净需求数量（基本单位数量）"),
            // entity.materialrequirementsplanningitem.netrequirement
            new TranslationSeedItem("entity.materialrequirementsplanningitem.netrequirement", "zh-HK", "净需求数量_hk", "净需求数量（基本单位数量）"),

            // entity.materialrequirementsplanningitem.procurementtype
            new TranslationSeedItem("entity.materialrequirementsplanningitem.procurementtype", "en-US", "供应类型_us", "供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）"),
            // entity.materialrequirementsplanningitem.procurementtype
            new TranslationSeedItem("entity.materialrequirementsplanningitem.procurementtype", "ja-JP", "供应类型_jp", "供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）"),
            // entity.materialrequirementsplanningitem.procurementtype
            new TranslationSeedItem("entity.materialrequirementsplanningitem.procurementtype", "zh-CN", "供应类型", "供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）"),
            // entity.materialrequirementsplanningitem.procurementtype
            new TranslationSeedItem("entity.materialrequirementsplanningitem.procurementtype", "zh-HK", "供应类型_hk", "供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）"),

            // entity.materialrequirementsplanningitem.isobsolete
            new TranslationSeedItem("entity.materialrequirementsplanningitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是）"),
            // entity.materialrequirementsplanningitem.isobsolete
            new TranslationSeedItem("entity.materialrequirementsplanningitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是）"),
            // entity.materialrequirementsplanningitem.isobsolete
            new TranslationSeedItem("entity.materialrequirementsplanningitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是）"),
            // entity.materialrequirementsplanningitem.isobsolete
            new TranslationSeedItem("entity.materialrequirementsplanningitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是）"),
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
