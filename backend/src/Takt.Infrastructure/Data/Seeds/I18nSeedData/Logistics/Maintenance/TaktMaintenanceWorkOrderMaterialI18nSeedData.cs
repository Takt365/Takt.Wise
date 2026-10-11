// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderMaterialI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaintenanceWorkOrderMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Maintenance;

/// <summary>
/// TaktMaintenanceWorkOrderMaterial 实体国际化翻译种子（键前缀 entity.maintenanceworkordermaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaintenanceWorkOrderMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaintenanceWorkOrderMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 maintenanceworkordermaterial 实体翻译...", tenantCode);

        foreach (var item in GetMaintenanceWorkOrderMaterialTranslations())
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

        TaktLogger.Information("TaktMaintenanceWorkOrderMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaintenanceWorkOrderMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.maintenanceworkordermaterial._self / entity.maintenanceworkordermaterial.{{field}}；ResourceGroup=Maintenance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaintenanceWorkOrderMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.maintenanceworkordermaterial._self
            new TranslationSeedItem("entity.maintenanceworkordermaterial._self", "en-US", "Maintenance Work Order Material Information_us", "实体名称"),
            // entity.maintenanceworkordermaterial._self
            new TranslationSeedItem("entity.maintenanceworkordermaterial._self", "ja-JP", "维护工单领料明细信息_jp", "实体名称"),
            // entity.maintenanceworkordermaterial._self
            new TranslationSeedItem("entity.maintenanceworkordermaterial._self", "zh-CN", "维护工单领料明细信息", "实体名称"),
            // entity.maintenanceworkordermaterial._self
            new TranslationSeedItem("entity.maintenanceworkordermaterial._self", "zh-HK", "维护工单领料明细信息_hk", "实体名称"),

            // entity.maintenanceworkordermaterial.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorderid", "en-US", "维护工单ID_us", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkordermaterial.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorderid", "ja-JP", "维护工单ID_jp", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkordermaterial.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorderid", "zh-CN", "维护工单ID", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkordermaterial.maintenanceworkorderid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorderid", "zh-HK", "维护工单ID_hk", "维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkordermaterial.workordercode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.workordercode", "en-US", "维护工单号_us", "维护工单号（冗余）"),
            // entity.maintenanceworkordermaterial.workordercode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.workordercode", "ja-JP", "维护工单号_jp", "维护工单号（冗余）"),
            // entity.maintenanceworkordermaterial.workordercode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.workordercode", "zh-CN", "维护工单号", "维护工单号（冗余）"),
            // entity.maintenanceworkordermaterial.workordercode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.workordercode", "zh-HK", "维护工单号_hk", "维护工单号（冗余）"),

            // entity.maintenanceworkordermaterial.linenumber
            new TranslationSeedItem("entity.maintenanceworkordermaterial.linenumber", "en-US", "行号_us", "行号（步长10：10/20/30…）"),
            // entity.maintenanceworkordermaterial.linenumber
            new TranslationSeedItem("entity.maintenanceworkordermaterial.linenumber", "ja-JP", "行号_jp", "行号（步长10：10/20/30…）"),
            // entity.maintenanceworkordermaterial.linenumber
            new TranslationSeedItem("entity.maintenanceworkordermaterial.linenumber", "zh-CN", "行号", "行号（步长10：10/20/30…）"),
            // entity.maintenanceworkordermaterial.linenumber
            new TranslationSeedItem("entity.maintenanceworkordermaterial.linenumber", "zh-HK", "行号_hk", "行号（步长10：10/20/30…）"),

            // entity.maintenanceworkordermaterial.materialid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialid", "en-US", "物料ID_us", "物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkordermaterial.materialid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialid", "ja-JP", "物料ID_jp", "物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkordermaterial.materialid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialid", "zh-CN", "物料ID", "物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),
            // entity.maintenanceworkordermaterial.materialid
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialid", "zh-HK", "物料ID_hk", "物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）"),

            // entity.maintenanceworkordermaterial.materialcode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.maintenanceworkordermaterial.materialcode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.maintenanceworkordermaterial.materialcode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.maintenanceworkordermaterial.materialcode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.maintenanceworkordermaterial.materialdescription
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.maintenanceworkordermaterial.materialdescription
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.maintenanceworkordermaterial.materialdescription
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.maintenanceworkordermaterial.materialdescription
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.maintenanceworkordermaterial.requiredquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.requiredquantity", "en-US", "需求数量_us", "需求数量"),
            // entity.maintenanceworkordermaterial.requiredquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.requiredquantity", "ja-JP", "需求数量_jp", "需求数量"),
            // entity.maintenanceworkordermaterial.requiredquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.requiredquantity", "zh-CN", "需求数量", "需求数量"),
            // entity.maintenanceworkordermaterial.requiredquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.requiredquantity", "zh-HK", "需求数量_hk", "需求数量"),

            // entity.maintenanceworkordermaterial.issuedquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuedquantity", "en-US", "已领数量_us", "已领数量"),
            // entity.maintenanceworkordermaterial.issuedquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuedquantity", "ja-JP", "已领数量_jp", "已领数量"),
            // entity.maintenanceworkordermaterial.issuedquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuedquantity", "zh-CN", "已领数量", "已领数量"),
            // entity.maintenanceworkordermaterial.issuedquantity
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuedquantity", "zh-HK", "已领数量_hk", "已领数量"),

            // entity.maintenanceworkordermaterial.materialunit
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialunit", "en-US", "单位_us", "单位"),
            // entity.maintenanceworkordermaterial.materialunit
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialunit", "ja-JP", "单位_jp", "单位"),
            // entity.maintenanceworkordermaterial.materialunit
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialunit", "zh-CN", "单位", "单位"),
            // entity.maintenanceworkordermaterial.materialunit
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialunit", "zh-HK", "单位_hk", "单位"),

            // entity.maintenanceworkordermaterial.unitprice
            new TranslationSeedItem("entity.maintenanceworkordermaterial.unitprice", "en-US", "单价_us", "单价"),
            // entity.maintenanceworkordermaterial.unitprice
            new TranslationSeedItem("entity.maintenanceworkordermaterial.unitprice", "ja-JP", "单价_jp", "单价"),
            // entity.maintenanceworkordermaterial.unitprice
            new TranslationSeedItem("entity.maintenanceworkordermaterial.unitprice", "zh-CN", "单价", "单价"),
            // entity.maintenanceworkordermaterial.unitprice
            new TranslationSeedItem("entity.maintenanceworkordermaterial.unitprice", "zh-HK", "单价_hk", "单价"),

            // entity.maintenanceworkordermaterial.amount
            new TranslationSeedItem("entity.maintenanceworkordermaterial.amount", "en-US", "金额_us", "金额"),
            // entity.maintenanceworkordermaterial.amount
            new TranslationSeedItem("entity.maintenanceworkordermaterial.amount", "ja-JP", "金额_jp", "金额"),
            // entity.maintenanceworkordermaterial.amount
            new TranslationSeedItem("entity.maintenanceworkordermaterial.amount", "zh-CN", "金额", "金额"),
            // entity.maintenanceworkordermaterial.amount
            new TranslationSeedItem("entity.maintenanceworkordermaterial.amount", "zh-HK", "金额_hk", "金额"),

            // entity.maintenanceworkordermaterial.warehousecode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.warehousecode", "en-US", "仓库编码_us", "仓库编码"),
            // entity.maintenanceworkordermaterial.warehousecode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.warehousecode", "ja-JP", "仓库编码_jp", "仓库编码"),
            // entity.maintenanceworkordermaterial.warehousecode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.warehousecode", "zh-CN", "仓库编码", "仓库编码"),
            // entity.maintenanceworkordermaterial.warehousecode
            new TranslationSeedItem("entity.maintenanceworkordermaterial.warehousecode", "zh-HK", "仓库编码_hk", "仓库编码"),

            // entity.maintenanceworkordermaterial.storagelocation
            new TranslationSeedItem("entity.maintenanceworkordermaterial.storagelocation", "en-US", "库位_us", "库位"),
            // entity.maintenanceworkordermaterial.storagelocation
            new TranslationSeedItem("entity.maintenanceworkordermaterial.storagelocation", "ja-JP", "库位_jp", "库位"),
            // entity.maintenanceworkordermaterial.storagelocation
            new TranslationSeedItem("entity.maintenanceworkordermaterial.storagelocation", "zh-CN", "库位", "库位"),
            // entity.maintenanceworkordermaterial.storagelocation
            new TranslationSeedItem("entity.maintenanceworkordermaterial.storagelocation", "zh-HK", "库位_hk", "库位"),

            // entity.maintenanceworkordermaterial.issuestatus
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuestatus", "en-US", "领料状态_us", "领料状态（0=待领料，1=部分领料，2=已领料）"),
            // entity.maintenanceworkordermaterial.issuestatus
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuestatus", "ja-JP", "领料状态_jp", "领料状态（0=待领料，1=部分领料，2=已领料）"),
            // entity.maintenanceworkordermaterial.issuestatus
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuestatus", "zh-CN", "领料状态", "领料状态（0=待领料，1=部分领料，2=已领料）"),
            // entity.maintenanceworkordermaterial.issuestatus
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuestatus", "zh-HK", "领料状态_hk", "领料状态（0=待领料，1=部分领料，2=已领料）"),

            // entity.maintenanceworkordermaterial.issuetime
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuetime", "en-US", "领料时间_us", "领料时间"),
            // entity.maintenanceworkordermaterial.issuetime
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuetime", "ja-JP", "领料时间_jp", "领料时间"),
            // entity.maintenanceworkordermaterial.issuetime
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuetime", "zh-CN", "领料时间", "领料时间"),
            // entity.maintenanceworkordermaterial.issuetime
            new TranslationSeedItem("entity.maintenanceworkordermaterial.issuetime", "zh-HK", "领料时间_hk", "领料时间"),

            // entity.maintenanceworkordermaterial.isobsolete
            new TranslationSeedItem("entity.maintenanceworkordermaterial.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.maintenanceworkordermaterial.isobsolete
            new TranslationSeedItem("entity.maintenanceworkordermaterial.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.maintenanceworkordermaterial.isobsolete
            new TranslationSeedItem("entity.maintenanceworkordermaterial.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.maintenanceworkordermaterial.isobsolete
            new TranslationSeedItem("entity.maintenanceworkordermaterial.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.maintenanceworkordermaterial.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorder", "en-US", "维护工单_us", "维护工单（主表）"),
            // entity.maintenanceworkordermaterial.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorder", "ja-JP", "维护工单_jp", "维护工单（主表）"),
            // entity.maintenanceworkordermaterial.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorder", "zh-CN", "维护工单", "维护工单（主表）"),
            // entity.maintenanceworkordermaterial.maintenanceworkorder
            new TranslationSeedItem("entity.maintenanceworkordermaterial.maintenanceworkorder", "zh-HK", "维护工单_hk", "维护工单（主表）"),

            // entity.maintenanceworkordermaterial.materialplant
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialplant", "en-US", "物料_us", "物料（工厂物料主数据）"),
            // entity.maintenanceworkordermaterial.materialplant
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialplant", "ja-JP", "物料_jp", "物料（工厂物料主数据）"),
            // entity.maintenanceworkordermaterial.materialplant
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialplant", "zh-CN", "物料", "物料（工厂物料主数据）"),
            // entity.maintenanceworkordermaterial.materialplant
            new TranslationSeedItem("entity.maintenanceworkordermaterial.materialplant", "zh-HK", "物料_hk", "物料（工厂物料主数据）"),
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
        translation.ResourceGroup = "Maintenance";
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
