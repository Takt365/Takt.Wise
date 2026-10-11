// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBillOfMaterial 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/bom/bill-of-material）
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
/// TaktBillOfMaterial 实体国际化翻译种子（键前缀 entity.billofmaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBillOfMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBillOfMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 billofmaterial 实体翻译...", tenantCode);

        foreach (var item in GetBillOfMaterialTranslations())
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

        TaktLogger.Information("TaktBillOfMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBillOfMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.billofmaterial._self / entity.billofmaterial.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBillOfMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.billofmaterial._self
            new TranslationSeedItem("entity.billofmaterial._self", "en-US", "Bill Of Material Information_us", "实体名称"),
            // entity.billofmaterial._self
            new TranslationSeedItem("entity.billofmaterial._self", "ja-JP", "Takt物料清单信息_jp", "实体名称"),
            // entity.billofmaterial._self
            new TranslationSeedItem("entity.billofmaterial._self", "zh-CN", "Takt物料清单信息", "实体名称"),
            // entity.billofmaterial._self
            new TranslationSeedItem("entity.billofmaterial._self", "zh-HK", "Takt物料清单信息_hk", "实体名称"),

            // entity.billofmaterial.bomcode
            new TranslationSeedItem("entity.billofmaterial.bomcode", "en-US", "BOM编码_us", "BOM编码（业务单据号，便于检索，非唯一键）"),
            // entity.billofmaterial.bomcode
            new TranslationSeedItem("entity.billofmaterial.bomcode", "ja-JP", "BOM编码_jp", "BOM编码（业务单据号，便于检索，非唯一键）"),
            // entity.billofmaterial.bomcode
            new TranslationSeedItem("entity.billofmaterial.bomcode", "zh-CN", "BOM编码", "BOM编码（业务单据号，便于检索，非唯一键）"),
            // entity.billofmaterial.bomcode
            new TranslationSeedItem("entity.billofmaterial.bomcode", "zh-HK", "BOM编码_hk", "BOM编码（业务单据号，便于检索，非唯一键）"),

            // entity.billofmaterial.bomname
            new TranslationSeedItem("entity.billofmaterial.bomname", "en-US", "BOM名称_us", "BOM名称"),
            // entity.billofmaterial.bomname
            new TranslationSeedItem("entity.billofmaterial.bomname", "ja-JP", "BOM名称_jp", "BOM名称"),
            // entity.billofmaterial.bomname
            new TranslationSeedItem("entity.billofmaterial.bomname", "zh-CN", "BOM名称", "BOM名称"),
            // entity.billofmaterial.bomname
            new TranslationSeedItem("entity.billofmaterial.bomname", "zh-HK", "BOM名称_hk", "BOM名称"),

            // entity.billofmaterial.parentmaterialcode
            new TranslationSeedItem("entity.billofmaterial.parentmaterialcode", "en-US", "父物料编码_us", "父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.billofmaterial.parentmaterialcode
            new TranslationSeedItem("entity.billofmaterial.parentmaterialcode", "ja-JP", "父物料编码_jp", "父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.billofmaterial.parentmaterialcode
            new TranslationSeedItem("entity.billofmaterial.parentmaterialcode", "zh-CN", "父物料编码", "父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.billofmaterial.parentmaterialcode
            new TranslationSeedItem("entity.billofmaterial.parentmaterialcode", "zh-HK", "父物料编码_hk", "父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.billofmaterial.parentmaterialdescription
            new TranslationSeedItem("entity.billofmaterial.parentmaterialdescription", "en-US", "父物料描述_us", "父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.billofmaterial.parentmaterialdescription
            new TranslationSeedItem("entity.billofmaterial.parentmaterialdescription", "ja-JP", "父物料描述_jp", "父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.billofmaterial.parentmaterialdescription
            new TranslationSeedItem("entity.billofmaterial.parentmaterialdescription", "zh-CN", "父物料描述", "父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.billofmaterial.parentmaterialdescription
            new TranslationSeedItem("entity.billofmaterial.parentmaterialdescription", "zh-HK", "父物料描述_hk", "父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.billofmaterial.bomversion
            new TranslationSeedItem("entity.billofmaterial.bomversion", "en-US", "BOM版本号_us", "BOM版本号"),
            // entity.billofmaterial.bomversion
            new TranslationSeedItem("entity.billofmaterial.bomversion", "ja-JP", "BOM版本号_jp", "BOM版本号"),
            // entity.billofmaterial.bomversion
            new TranslationSeedItem("entity.billofmaterial.bomversion", "zh-CN", "BOM版本号", "BOM版本号"),
            // entity.billofmaterial.bomversion
            new TranslationSeedItem("entity.billofmaterial.bomversion", "zh-HK", "BOM版本号_hk", "BOM版本号"),

            // entity.billofmaterial.bomtype
            new TranslationSeedItem("entity.billofmaterial.bomtype", "en-US", "BOM类型_us", "BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）"),
            // entity.billofmaterial.bomtype
            new TranslationSeedItem("entity.billofmaterial.bomtype", "ja-JP", "BOM类型_jp", "BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）"),
            // entity.billofmaterial.bomtype
            new TranslationSeedItem("entity.billofmaterial.bomtype", "zh-CN", "BOM类型", "BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）"),
            // entity.billofmaterial.bomtype
            new TranslationSeedItem("entity.billofmaterial.bomtype", "zh-HK", "BOM类型_hk", "BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）"),

            // entity.billofmaterial.alternativebomnumber
            new TranslationSeedItem("entity.billofmaterial.alternativebomnumber", "en-US", "备选BOM编码_us", "备选BOM编码（对应，如01/02）"),
            // entity.billofmaterial.alternativebomnumber
            new TranslationSeedItem("entity.billofmaterial.alternativebomnumber", "ja-JP", "备选BOM编码_jp", "备选BOM编码（对应，如01/02）"),
            // entity.billofmaterial.alternativebomnumber
            new TranslationSeedItem("entity.billofmaterial.alternativebomnumber", "zh-CN", "备选BOM编码", "备选BOM编码（对应，如01/02）"),
            // entity.billofmaterial.alternativebomnumber
            new TranslationSeedItem("entity.billofmaterial.alternativebomnumber", "zh-HK", "备选BOM编码_hk", "备选BOM编码（对应，如01/02）"),

            // entity.billofmaterial.effectivedate
            new TranslationSeedItem("entity.billofmaterial.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.billofmaterial.effectivedate
            new TranslationSeedItem("entity.billofmaterial.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.billofmaterial.effectivedate
            new TranslationSeedItem("entity.billofmaterial.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.billofmaterial.effectivedate
            new TranslationSeedItem("entity.billofmaterial.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.billofmaterial.expirydate
            new TranslationSeedItem("entity.billofmaterial.expirydate", "en-US", "失效日期_us", "失效日期（为空表示永久有效）"),
            // entity.billofmaterial.expirydate
            new TranslationSeedItem("entity.billofmaterial.expirydate", "ja-JP", "失效日期_jp", "失效日期（为空表示永久有效）"),
            // entity.billofmaterial.expirydate
            new TranslationSeedItem("entity.billofmaterial.expirydate", "zh-CN", "失效日期", "失效日期（为空表示永久有效）"),
            // entity.billofmaterial.expirydate
            new TranslationSeedItem("entity.billofmaterial.expirydate", "zh-HK", "失效日期_hk", "失效日期（为空表示永久有效）"),

            // entity.billofmaterial.parentmaterialunit
            new TranslationSeedItem("entity.billofmaterial.parentmaterialunit", "en-US", "父物料单位_us", "父物料单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterial.parentmaterialunit
            new TranslationSeedItem("entity.billofmaterial.parentmaterialunit", "ja-JP", "父物料单位_jp", "父物料单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterial.parentmaterialunit
            new TranslationSeedItem("entity.billofmaterial.parentmaterialunit", "zh-CN", "父物料单位", "父物料单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterial.parentmaterialunit
            new TranslationSeedItem("entity.billofmaterial.parentmaterialunit", "zh-HK", "父物料单位_hk", "父物料单位（字典 logistics_materials_unit_of_measure_code）"),

            // entity.billofmaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billofmaterial.parentmaterialquantity", "en-US", "基本数量_us", "基本数量（BOM基数，对应）"),
            // entity.billofmaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billofmaterial.parentmaterialquantity", "ja-JP", "基本数量_jp", "基本数量（BOM基数，对应）"),
            // entity.billofmaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billofmaterial.parentmaterialquantity", "zh-CN", "基本数量", "基本数量（BOM基数，对应）"),
            // entity.billofmaterial.parentmaterialquantity
            new TranslationSeedItem("entity.billofmaterial.parentmaterialquantity", "zh-HK", "基本数量_hk", "基本数量（BOM基数，对应）"),

            // entity.billofmaterial.bomdescription
            new TranslationSeedItem("entity.billofmaterial.bomdescription", "en-US", "BOM描述_us", "BOM描述"),
            // entity.billofmaterial.bomdescription
            new TranslationSeedItem("entity.billofmaterial.bomdescription", "ja-JP", "BOM描述_jp", "BOM描述"),
            // entity.billofmaterial.bomdescription
            new TranslationSeedItem("entity.billofmaterial.bomdescription", "zh-CN", "BOM描述", "BOM描述"),
            // entity.billofmaterial.bomdescription
            new TranslationSeedItem("entity.billofmaterial.bomdescription", "zh-HK", "BOM描述_hk", "BOM描述"),

            // entity.billofmaterial.bomgroupcode
            new TranslationSeedItem("entity.billofmaterial.bomgroupcode", "en-US", "BOM组编码_us", "BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）"),
            // entity.billofmaterial.bomgroupcode
            new TranslationSeedItem("entity.billofmaterial.bomgroupcode", "ja-JP", "BOM组编码_jp", "BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）"),
            // entity.billofmaterial.bomgroupcode
            new TranslationSeedItem("entity.billofmaterial.bomgroupcode", "zh-CN", "BOM组编码", "BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）"),
            // entity.billofmaterial.bomgroupcode
            new TranslationSeedItem("entity.billofmaterial.bomgroupcode", "zh-HK", "BOM组编码_hk", "BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）"),

            // entity.billofmaterial.bomgroupname
            new TranslationSeedItem("entity.billofmaterial.bomgroupname", "en-US", "BOM组名称_us", "BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）"),
            // entity.billofmaterial.bomgroupname
            new TranslationSeedItem("entity.billofmaterial.bomgroupname", "ja-JP", "BOM组名称_jp", "BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）"),
            // entity.billofmaterial.bomgroupname
            new TranslationSeedItem("entity.billofmaterial.bomgroupname", "zh-CN", "BOM组名称", "BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）"),
            // entity.billofmaterial.bomgroupname
            new TranslationSeedItem("entity.billofmaterial.bomgroupname", "zh-HK", "BOM组名称_hk", "BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）"),

            // entity.billofmaterial.sortorder
            new TranslationSeedItem("entity.billofmaterial.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.billofmaterial.sortorder
            new TranslationSeedItem("entity.billofmaterial.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.billofmaterial.sortorder
            new TranslationSeedItem("entity.billofmaterial.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.billofmaterial.sortorder
            new TranslationSeedItem("entity.billofmaterial.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),

            // entity.billofmaterial.bomstatus
            new TranslationSeedItem("entity.billofmaterial.bomstatus", "en-US", "BOM状态_us", "BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）"),
            // entity.billofmaterial.bomstatus
            new TranslationSeedItem("entity.billofmaterial.bomstatus", "ja-JP", "BOM状态_jp", "BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）"),
            // entity.billofmaterial.bomstatus
            new TranslationSeedItem("entity.billofmaterial.bomstatus", "zh-CN", "BOM状态", "BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）"),
            // entity.billofmaterial.bomstatus
            new TranslationSeedItem("entity.billofmaterial.bomstatus", "zh-HK", "BOM状态_hk", "BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）"),

            // entity.billofmaterial.items
            new TranslationSeedItem("entity.billofmaterial.items", "en-US", "BOM组成件明细_us", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
            // entity.billofmaterial.items
            new TranslationSeedItem("entity.billofmaterial.items", "ja-JP", "BOM组成件明细_jp", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
            // entity.billofmaterial.items
            new TranslationSeedItem("entity.billofmaterial.items", "zh-CN", "BOM组成件明细", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
            // entity.billofmaterial.items
            new TranslationSeedItem("entity.billofmaterial.items", "zh-HK", "BOM组成件明细_hk", "BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）"),
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
