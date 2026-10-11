// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktSourceOfSupplyI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSourceOfSupply 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktSourceOfSupply 实体国际化翻译种子（键前缀 entity.sourceofsupply.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSourceOfSupplyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSourceOfSupply 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sourceofsupply 实体翻译...", tenantCode);

        foreach (var item in GetSourceOfSupplyTranslations())
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

        TaktLogger.Information("TaktSourceOfSupply 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSourceOfSupply 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sourceofsupply._self / entity.sourceofsupply.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSourceOfSupplyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sourceofsupply._self
            new TranslationSeedItem("entity.sourceofsupply._self", "en-US", "Source Of Supply Information_us", "实体名称"),
            // entity.sourceofsupply._self
            new TranslationSeedItem("entity.sourceofsupply._self", "ja-JP", "Takt货源清单信息_jp", "实体名称"),
            // entity.sourceofsupply._self
            new TranslationSeedItem("entity.sourceofsupply._self", "zh-CN", "Takt货源清单信息", "实体名称"),
            // entity.sourceofsupply._self
            new TranslationSeedItem("entity.sourceofsupply._self", "zh-HK", "Takt货源清单信息_hk", "实体名称"),

            // entity.sourceofsupply.code
            new TranslationSeedItem("entity.sourceofsupply.code", "en-US", "货源清单编码_us", "货源清单编码（租户+公司内唯一；业务单据号）"),
            // entity.sourceofsupply.code
            new TranslationSeedItem("entity.sourceofsupply.code", "ja-JP", "货源清单编码_jp", "货源清单编码（租户+公司内唯一；业务单据号）"),
            // entity.sourceofsupply.code
            new TranslationSeedItem("entity.sourceofsupply.code", "zh-CN", "货源清单编码", "货源清单编码（租户+公司内唯一；业务单据号）"),
            // entity.sourceofsupply.code
            new TranslationSeedItem("entity.sourceofsupply.code", "zh-HK", "货源清单编码_hk", "货源清单编码（租户+公司内唯一；业务单据号）"),

            // entity.sourceofsupply.materialcode
            new TranslationSeedItem("entity.sourceofsupply.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sourceofsupply.materialcode
            new TranslationSeedItem("entity.sourceofsupply.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sourceofsupply.materialcode
            new TranslationSeedItem("entity.sourceofsupply.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sourceofsupply.materialcode
            new TranslationSeedItem("entity.sourceofsupply.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.sourceofsupply.suppliercode
            new TranslationSeedItem("entity.sourceofsupply.suppliercode", "en-US", "供货商编码_us", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.sourceofsupply.suppliercode
            new TranslationSeedItem("entity.sourceofsupply.suppliercode", "ja-JP", "供货商编码_jp", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.sourceofsupply.suppliercode
            new TranslationSeedItem("entity.sourceofsupply.suppliercode", "zh-CN", "供货商编码", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),
            // entity.sourceofsupply.suppliercode
            new TranslationSeedItem("entity.sourceofsupply.suppliercode", "zh-HK", "供货商编码_hk", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）"),

            // entity.sourceofsupply.purchasegroup
            new TranslationSeedItem("entity.sourceofsupply.purchasegroup", "en-US", "采购组_us", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.sourceofsupply.purchasegroup
            new TranslationSeedItem("entity.sourceofsupply.purchasegroup", "ja-JP", "采购组_jp", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.sourceofsupply.purchasegroup
            new TranslationSeedItem("entity.sourceofsupply.purchasegroup", "zh-CN", "采购组", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),
            // entity.sourceofsupply.purchasegroup
            new TranslationSeedItem("entity.sourceofsupply.purchasegroup", "zh-HK", "采购组_hk", "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）"),

            // entity.sourceofsupply.isfixed
            new TranslationSeedItem("entity.sourceofsupply.isfixed", "en-US", "固定_us", "固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）"),
            // entity.sourceofsupply.isfixed
            new TranslationSeedItem("entity.sourceofsupply.isfixed", "ja-JP", "固定_jp", "固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）"),
            // entity.sourceofsupply.isfixed
            new TranslationSeedItem("entity.sourceofsupply.isfixed", "zh-CN", "固定", "固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）"),
            // entity.sourceofsupply.isfixed
            new TranslationSeedItem("entity.sourceofsupply.isfixed", "zh-HK", "固定_hk", "固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）"),

            // entity.sourceofsupply.isblocked
            new TranslationSeedItem("entity.sourceofsupply.isblocked", "en-US", "冻结_us", "冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）"),
            // entity.sourceofsupply.isblocked
            new TranslationSeedItem("entity.sourceofsupply.isblocked", "ja-JP", "冻结_jp", "冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）"),
            // entity.sourceofsupply.isblocked
            new TranslationSeedItem("entity.sourceofsupply.isblocked", "zh-CN", "冻结", "冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）"),
            // entity.sourceofsupply.isblocked
            new TranslationSeedItem("entity.sourceofsupply.isblocked", "zh-HK", "冻结_hk", "冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）"),

            // entity.sourceofsupply.purchaseunit
            new TranslationSeedItem("entity.sourceofsupply.purchaseunit", "en-US", "采购单位_us", "采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.sourceofsupply.purchaseunit
            new TranslationSeedItem("entity.sourceofsupply.purchaseunit", "ja-JP", "采购单位_jp", "采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.sourceofsupply.purchaseunit
            new TranslationSeedItem("entity.sourceofsupply.purchaseunit", "zh-CN", "采购单位", "采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),
            // entity.sourceofsupply.purchaseunit
            new TranslationSeedItem("entity.sourceofsupply.purchaseunit", "zh-HK", "采购单位_hk", "采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）"),

            // entity.sourceofsupply.minorderquantity
            new TranslationSeedItem("entity.sourceofsupply.minorderquantity", "en-US", "最小起订量_us", "最小起订量（采购单位数量，整数）"),
            // entity.sourceofsupply.minorderquantity
            new TranslationSeedItem("entity.sourceofsupply.minorderquantity", "ja-JP", "最小起订量_jp", "最小起订量（采购单位数量，整数）"),
            // entity.sourceofsupply.minorderquantity
            new TranslationSeedItem("entity.sourceofsupply.minorderquantity", "zh-CN", "最小起订量", "最小起订量（采购单位数量，整数）"),
            // entity.sourceofsupply.minorderquantity
            new TranslationSeedItem("entity.sourceofsupply.minorderquantity", "zh-HK", "最小起订量_hk", "最小起订量（采购单位数量，整数）"),

            // entity.sourceofsupply.roundingvalue
            new TranslationSeedItem("entity.sourceofsupply.roundingvalue", "en-US", "舍入值_us", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.sourceofsupply.roundingvalue
            new TranslationSeedItem("entity.sourceofsupply.roundingvalue", "ja-JP", "舍入值_jp", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.sourceofsupply.roundingvalue
            new TranslationSeedItem("entity.sourceofsupply.roundingvalue", "zh-CN", "舍入值", "舍入值（基本单位数量，用于数量舍入，整数）"),
            // entity.sourceofsupply.roundingvalue
            new TranslationSeedItem("entity.sourceofsupply.roundingvalue", "zh-HK", "舍入值_hk", "舍入值（基本单位数量，用于数量舍入，整数）"),

            // entity.sourceofsupply.planneddeliverytimedays
            new TranslationSeedItem("entity.sourceofsupply.planneddeliverytimedays", "en-US", "计划交货时间_us", "计划交货时间（天数，整数）"),
            // entity.sourceofsupply.planneddeliverytimedays
            new TranslationSeedItem("entity.sourceofsupply.planneddeliverytimedays", "ja-JP", "计划交货时间_jp", "计划交货时间（天数，整数）"),
            // entity.sourceofsupply.planneddeliverytimedays
            new TranslationSeedItem("entity.sourceofsupply.planneddeliverytimedays", "zh-CN", "计划交货时间", "计划交货时间（天数，整数）"),
            // entity.sourceofsupply.planneddeliverytimedays
            new TranslationSeedItem("entity.sourceofsupply.planneddeliverytimedays", "zh-HK", "计划交货时间_hk", "计划交货时间（天数，整数）"),

            // entity.sourceofsupply.agreementnumber
            new TranslationSeedItem("entity.sourceofsupply.agreementnumber", "en-US", "框架协议号_us", "框架协议号（采购合同/协议编码，可选）"),
            // entity.sourceofsupply.agreementnumber
            new TranslationSeedItem("entity.sourceofsupply.agreementnumber", "ja-JP", "框架协议号_jp", "框架协议号（采购合同/协议编码，可选）"),
            // entity.sourceofsupply.agreementnumber
            new TranslationSeedItem("entity.sourceofsupply.agreementnumber", "zh-CN", "框架协议号", "框架协议号（采购合同/协议编码，可选）"),
            // entity.sourceofsupply.agreementnumber
            new TranslationSeedItem("entity.sourceofsupply.agreementnumber", "zh-HK", "框架协议号_hk", "框架协议号（采购合同/协议编码，可选）"),

            // entity.sourceofsupply.agreementlinenumber
            new TranslationSeedItem("entity.sourceofsupply.agreementlinenumber", "en-US", "协议行号_us", "协议行号"),
            // entity.sourceofsupply.agreementlinenumber
            new TranslationSeedItem("entity.sourceofsupply.agreementlinenumber", "ja-JP", "协议行号_jp", "协议行号"),
            // entity.sourceofsupply.agreementlinenumber
            new TranslationSeedItem("entity.sourceofsupply.agreementlinenumber", "zh-CN", "协议行号", "协议行号"),
            // entity.sourceofsupply.agreementlinenumber
            new TranslationSeedItem("entity.sourceofsupply.agreementlinenumber", "zh-HK", "协议行号_hk", "协议行号"),

            // entity.sourceofsupply.validfrom
            new TranslationSeedItem("entity.sourceofsupply.validfrom", "en-US", "生效日期_us", "生效日期"),
            // entity.sourceofsupply.validfrom
            new TranslationSeedItem("entity.sourceofsupply.validfrom", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.sourceofsupply.validfrom
            new TranslationSeedItem("entity.sourceofsupply.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.sourceofsupply.validfrom
            new TranslationSeedItem("entity.sourceofsupply.validfrom", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.sourceofsupply.validto
            new TranslationSeedItem("entity.sourceofsupply.validto", "en-US", "失效日期_us", "失效日期"),
            // entity.sourceofsupply.validto
            new TranslationSeedItem("entity.sourceofsupply.validto", "ja-JP", "失效日期_jp", "失效日期"),
            // entity.sourceofsupply.validto
            new TranslationSeedItem("entity.sourceofsupply.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.sourceofsupply.validto
            new TranslationSeedItem("entity.sourceofsupply.validto", "zh-HK", "失效日期_hk", "失效日期"),

            // entity.sourceofsupply.sortorder
            new TranslationSeedItem("entity.sourceofsupply.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前；同物料多货源清单时的优先级）"),
            // entity.sourceofsupply.sortorder
            new TranslationSeedItem("entity.sourceofsupply.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前；同物料多货源清单时的优先级）"),
            // entity.sourceofsupply.sortorder
            new TranslationSeedItem("entity.sourceofsupply.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前；同物料多货源清单时的优先级）"),
            // entity.sourceofsupply.sortorder
            new TranslationSeedItem("entity.sourceofsupply.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前；同物料多货源清单时的优先级）"),

            // entity.sourceofsupply.sourcestatus
            new TranslationSeedItem("entity.sourceofsupply.sourcestatus", "en-US", "货源清单状态_us", "货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.sourceofsupply.sourcestatus
            new TranslationSeedItem("entity.sourceofsupply.sourcestatus", "ja-JP", "货源清单状态_jp", "货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.sourceofsupply.sourcestatus
            new TranslationSeedItem("entity.sourceofsupply.sourcestatus", "zh-CN", "货源清单状态", "货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.sourceofsupply.sourcestatus
            new TranslationSeedItem("entity.sourceofsupply.sourcestatus", "zh-HK", "货源清单状态_hk", "货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Procurement";
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
