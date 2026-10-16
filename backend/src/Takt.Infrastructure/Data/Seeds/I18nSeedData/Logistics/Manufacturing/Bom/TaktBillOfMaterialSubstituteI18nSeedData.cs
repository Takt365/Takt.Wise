// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialSubstituteI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktBillOfMaterialSubstitute 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktBillOfMaterialSubstitute 实体国际化翻译种子（键前缀 entity.billofmaterialsubstitute.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktBillOfMaterialSubstituteI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktBillOfMaterialSubstitute 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 billofmaterialsubstitute 实体翻译...", tenantCode);

        foreach (var item in GetBillOfMaterialSubstituteTranslations())
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

        TaktLogger.Information("TaktBillOfMaterialSubstitute 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktBillOfMaterialSubstitute 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.billofmaterialsubstitute._self / entity.billofmaterialsubstitute.{{field}}；ResourceGroup=Bom；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetBillOfMaterialSubstituteTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.billofmaterialsubstitute._self
            new TranslationSeedItem("entity.billofmaterialsubstitute._self", "en-US", "Bill Of Material Substitute Information_us", "实体名称"),
            // entity.billofmaterialsubstitute._self
            new TranslationSeedItem("entity.billofmaterialsubstitute._self", "ja-JP", "BOM替代料信息_jp", "实体名称"),
            // entity.billofmaterialsubstitute._self
            new TranslationSeedItem("entity.billofmaterialsubstitute._self", "zh-CN", "BOM替代料信息", "实体名称"),
            // entity.billofmaterialsubstitute._self
            new TranslationSeedItem("entity.billofmaterialsubstitute._self", "zh-HK", "BOM替代料信息_hk", "实体名称"),

            // entity.billofmaterialsubstitute.billofmaterialitemid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitemid", "en-US", "物料清单明细ID_us", "物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialsubstitute.billofmaterialitemid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitemid", "ja-JP", "物料清单明细ID_jp", "物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialsubstitute.billofmaterialitemid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitemid", "zh-CN", "物料清单明细ID", "物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialsubstitute.billofmaterialitemid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitemid", "zh-HK", "物料清单明细ID_hk", "物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.billofmaterialsubstitute.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialid", "en-US", "物料清单ID_us", "物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialsubstitute.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialid", "ja-JP", "物料清单ID_jp", "物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialsubstitute.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialid", "zh-CN", "物料清单ID", "物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）"),
            // entity.billofmaterialsubstitute.billofmaterialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialid", "zh-HK", "物料清单ID_hk", "物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）"),

            // entity.billofmaterialsubstitute.bomcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.bomcode", "en-US", "BOM编码_us", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialsubstitute.bomcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.bomcode", "ja-JP", "BOM编码_jp", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialsubstitute.bomcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.bomcode", "zh-CN", "BOM编码", "BOM编码（冗余，便于查询）"),
            // entity.billofmaterialsubstitute.bomcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.bomcode", "zh-HK", "BOM编码_hk", "BOM编码（冗余，便于查询）"),

            // entity.billofmaterialsubstitute.primarymaterialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.primarymaterialcode", "en-US", "主件物料编码_us", "主件物料编码（冗余，对应明细行子项物料编码）"),
            // entity.billofmaterialsubstitute.primarymaterialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.primarymaterialcode", "ja-JP", "主件物料编码_jp", "主件物料编码（冗余，对应明细行子项物料编码）"),
            // entity.billofmaterialsubstitute.primarymaterialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.primarymaterialcode", "zh-CN", "主件物料编码", "主件物料编码（冗余，对应明细行子项物料编码）"),
            // entity.billofmaterialsubstitute.primarymaterialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.primarymaterialcode", "zh-HK", "主件物料编码_hk", "主件物料编码（冗余，对应明细行子项物料编码）"),

            // entity.billofmaterialsubstitute.linenumber
            new TranslationSeedItem("entity.billofmaterialsubstitute.linenumber", "en-US", "替代行号_us", "替代行号（步长10：10/20/30…）"),
            // entity.billofmaterialsubstitute.linenumber
            new TranslationSeedItem("entity.billofmaterialsubstitute.linenumber", "ja-JP", "替代行号_jp", "替代行号（步长10：10/20/30…）"),
            // entity.billofmaterialsubstitute.linenumber
            new TranslationSeedItem("entity.billofmaterialsubstitute.linenumber", "zh-CN", "替代行号", "替代行号（步长10：10/20/30…）"),
            // entity.billofmaterialsubstitute.linenumber
            new TranslationSeedItem("entity.billofmaterialsubstitute.linenumber", "zh-HK", "替代行号_hk", "替代行号（步长10：10/20/30…）"),

            // entity.billofmaterialsubstitute.substitutematerialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialid", "en-US", "替代物料ID_us", "替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）"),
            // entity.billofmaterialsubstitute.substitutematerialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialid", "ja-JP", "替代物料ID_jp", "替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）"),
            // entity.billofmaterialsubstitute.substitutematerialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialid", "zh-CN", "替代物料ID", "替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）"),
            // entity.billofmaterialsubstitute.substitutematerialid
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialid", "zh-HK", "替代物料ID_hk", "替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）"),

            // entity.billofmaterialsubstitute.substitutematerialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialcode", "en-US", "替代物料编码_us", "替代物料编码（冗余）"),
            // entity.billofmaterialsubstitute.substitutematerialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialcode", "ja-JP", "替代物料编码_jp", "替代物料编码（冗余）"),
            // entity.billofmaterialsubstitute.substitutematerialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialcode", "zh-CN", "替代物料编码", "替代物料编码（冗余）"),
            // entity.billofmaterialsubstitute.substitutematerialcode
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialcode", "zh-HK", "替代物料编码_hk", "替代物料编码（冗余）"),

            // entity.billofmaterialsubstitute.substitutegroup
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutegroup", "en-US", "替代组号_us", "替代组号（与明细行 substitute_group 对齐，便于组内检索）"),
            // entity.billofmaterialsubstitute.substitutegroup
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutegroup", "ja-JP", "替代组号_jp", "替代组号（与明细行 substitute_group 对齐，便于组内检索）"),
            // entity.billofmaterialsubstitute.substitutegroup
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutegroup", "zh-CN", "替代组号", "替代组号（与明细行 substitute_group 对齐，便于组内检索）"),
            // entity.billofmaterialsubstitute.substitutegroup
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutegroup", "zh-HK", "替代组号_hk", "替代组号（与明细行 substitute_group 对齐，便于组内检索）"),

            // entity.billofmaterialsubstitute.substitutepriority
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutepriority", "en-US", "替代优先级_us", "替代优先级（越小越优先）"),
            // entity.billofmaterialsubstitute.substitutepriority
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutepriority", "ja-JP", "替代优先级_jp", "替代优先级（越小越优先）"),
            // entity.billofmaterialsubstitute.substitutepriority
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutepriority", "zh-CN", "替代优先级", "替代优先级（越小越优先）"),
            // entity.billofmaterialsubstitute.substitutepriority
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutepriority", "zh-HK", "替代优先级_hk", "替代优先级（越小越优先）"),

            // entity.billofmaterialsubstitute.usagequantity
            new TranslationSeedItem("entity.billofmaterialsubstitute.usagequantity", "en-US", "替代用量_us", "替代用量"),
            // entity.billofmaterialsubstitute.usagequantity
            new TranslationSeedItem("entity.billofmaterialsubstitute.usagequantity", "ja-JP", "替代用量_jp", "替代用量"),
            // entity.billofmaterialsubstitute.usagequantity
            new TranslationSeedItem("entity.billofmaterialsubstitute.usagequantity", "zh-CN", "替代用量", "替代用量"),
            // entity.billofmaterialsubstitute.usagequantity
            new TranslationSeedItem("entity.billofmaterialsubstitute.usagequantity", "zh-HK", "替代用量_hk", "替代用量"),

            // entity.billofmaterialsubstitute.materialunit
            new TranslationSeedItem("entity.billofmaterialsubstitute.materialunit", "en-US", "单位_us", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterialsubstitute.materialunit
            new TranslationSeedItem("entity.billofmaterialsubstitute.materialunit", "ja-JP", "单位_jp", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterialsubstitute.materialunit
            new TranslationSeedItem("entity.billofmaterialsubstitute.materialunit", "zh-CN", "单位", "单位（字典 logistics_materials_unit_of_measure_code）"),
            // entity.billofmaterialsubstitute.materialunit
            new TranslationSeedItem("entity.billofmaterialsubstitute.materialunit", "zh-HK", "单位_hk", "单位（字典 logistics_materials_unit_of_measure_code）"),

            // entity.billofmaterialsubstitute.usageratio
            new TranslationSeedItem("entity.billofmaterialsubstitute.usageratio", "en-US", "替代比例_us", "替代比例（相对主件用量，默认1表示等量替代）"),
            // entity.billofmaterialsubstitute.usageratio
            new TranslationSeedItem("entity.billofmaterialsubstitute.usageratio", "ja-JP", "替代比例_jp", "替代比例（相对主件用量，默认1表示等量替代）"),
            // entity.billofmaterialsubstitute.usageratio
            new TranslationSeedItem("entity.billofmaterialsubstitute.usageratio", "zh-CN", "替代比例", "替代比例（相对主件用量，默认1表示等量替代）"),
            // entity.billofmaterialsubstitute.usageratio
            new TranslationSeedItem("entity.billofmaterialsubstitute.usageratio", "zh-HK", "替代比例_hk", "替代比例（相对主件用量，默认1表示等量替代）"),

            // entity.billofmaterialsubstitute.isenabled
            new TranslationSeedItem("entity.billofmaterialsubstitute.isenabled", "en-US", "是否启用_us", "是否启用（0=否，1=是，字典 sys_yes_no）"),
            // entity.billofmaterialsubstitute.isenabled
            new TranslationSeedItem("entity.billofmaterialsubstitute.isenabled", "ja-JP", "是否启用_jp", "是否启用（0=否，1=是，字典 sys_yes_no）"),
            // entity.billofmaterialsubstitute.isenabled
            new TranslationSeedItem("entity.billofmaterialsubstitute.isenabled", "zh-CN", "是否启用", "是否启用（0=否，1=是，字典 sys_yes_no）"),
            // entity.billofmaterialsubstitute.isenabled
            new TranslationSeedItem("entity.billofmaterialsubstitute.isenabled", "zh-HK", "是否启用_hk", "是否启用（0=否，1=是，字典 sys_yes_no）"),

            // entity.billofmaterialsubstitute.effectivedate
            new TranslationSeedItem("entity.billofmaterialsubstitute.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.billofmaterialsubstitute.effectivedate
            new TranslationSeedItem("entity.billofmaterialsubstitute.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.billofmaterialsubstitute.effectivedate
            new TranslationSeedItem("entity.billofmaterialsubstitute.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.billofmaterialsubstitute.effectivedate
            new TranslationSeedItem("entity.billofmaterialsubstitute.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.billofmaterialsubstitute.expirydate
            new TranslationSeedItem("entity.billofmaterialsubstitute.expirydate", "en-US", "失效日期_us", "失效日期（为空表示永久有效）"),
            // entity.billofmaterialsubstitute.expirydate
            new TranslationSeedItem("entity.billofmaterialsubstitute.expirydate", "ja-JP", "失效日期_jp", "失效日期（为空表示永久有效）"),
            // entity.billofmaterialsubstitute.expirydate
            new TranslationSeedItem("entity.billofmaterialsubstitute.expirydate", "zh-CN", "失效日期", "失效日期（为空表示永久有效）"),
            // entity.billofmaterialsubstitute.expirydate
            new TranslationSeedItem("entity.billofmaterialsubstitute.expirydate", "zh-HK", "失效日期_hk", "失效日期（为空表示永久有效）"),

            // entity.billofmaterialsubstitute.isobsolete
            new TranslationSeedItem("entity.billofmaterialsubstitute.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.billofmaterialsubstitute.isobsolete
            new TranslationSeedItem("entity.billofmaterialsubstitute.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.billofmaterialsubstitute.isobsolete
            new TranslationSeedItem("entity.billofmaterialsubstitute.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.billofmaterialsubstitute.isobsolete
            new TranslationSeedItem("entity.billofmaterialsubstitute.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.billofmaterialsubstitute.billofmaterialitem
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitem", "en-US", "物料清单明细_us", "物料清单明细（主表）"),
            // entity.billofmaterialsubstitute.billofmaterialitem
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitem", "ja-JP", "物料清单明细_jp", "物料清单明细（主表）"),
            // entity.billofmaterialsubstitute.billofmaterialitem
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitem", "zh-CN", "物料清单明细", "物料清单明细（主表）"),
            // entity.billofmaterialsubstitute.billofmaterialitem
            new TranslationSeedItem("entity.billofmaterialsubstitute.billofmaterialitem", "zh-HK", "物料清单明细_hk", "物料清单明细（主表）"),

            // entity.billofmaterialsubstitute.substitutematerialplant
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialplant", "en-US", "替代物料_us", "替代物料（工厂物料主数据）"),
            // entity.billofmaterialsubstitute.substitutematerialplant
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialplant", "ja-JP", "替代物料_jp", "替代物料（工厂物料主数据）"),
            // entity.billofmaterialsubstitute.substitutematerialplant
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialplant", "zh-CN", "替代物料", "替代物料（工厂物料主数据）"),
            // entity.billofmaterialsubstitute.substitutematerialplant
            new TranslationSeedItem("entity.billofmaterialsubstitute.substitutematerialplant", "zh-HK", "替代物料_hk", "替代物料（工厂物料主数据）"),
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
