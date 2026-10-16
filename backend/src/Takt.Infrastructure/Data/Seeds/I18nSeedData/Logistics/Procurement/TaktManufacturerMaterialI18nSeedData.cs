// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktManufacturerMaterialI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktManufacturerMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktManufacturerMaterial 实体国际化翻译种子（键前缀 entity.manufacturermaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktManufacturerMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktManufacturerMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 manufacturermaterial 实体翻译...", tenantCode);

        foreach (var item in GetManufacturerMaterialTranslations())
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

        TaktLogger.Information("TaktManufacturerMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktManufacturerMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.manufacturermaterial._self / entity.manufacturermaterial.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetManufacturerMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "en-US", "Manufacturer Material Information_us", "实体名称"),
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "ja-JP", "Takt制造商物料信息_jp", "实体名称"),
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "zh-CN", "Takt制造商物料信息", "实体名称"),
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "zh-HK", "Takt制造商物料信息_hk", "实体名称"),

            // entity.manufacturermaterial.vendorcode
            new TranslationSeedItem("entity.manufacturermaterial.vendorcode", "en-US", "经销商编码_us", "经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）"),
            // entity.manufacturermaterial.vendorcode
            new TranslationSeedItem("entity.manufacturermaterial.vendorcode", "ja-JP", "经销商编码_jp", "经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）"),
            // entity.manufacturermaterial.vendorcode
            new TranslationSeedItem("entity.manufacturermaterial.vendorcode", "zh-CN", "经销商编码", "经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）"),
            // entity.manufacturermaterial.vendorcode
            new TranslationSeedItem("entity.manufacturermaterial.vendorcode", "zh-HK", "经销商编码_hk", "经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）"),

            // entity.manufacturermaterial.vendorshortname
            new TranslationSeedItem("entity.manufacturermaterial.vendorshortname", "en-US", "经销商简称_us", "经销商简称（冗余）"),
            // entity.manufacturermaterial.vendorshortname
            new TranslationSeedItem("entity.manufacturermaterial.vendorshortname", "ja-JP", "经销商简称_jp", "经销商简称（冗余）"),
            // entity.manufacturermaterial.vendorshortname
            new TranslationSeedItem("entity.manufacturermaterial.vendorshortname", "zh-CN", "经销商简称", "经销商简称（冗余）"),
            // entity.manufacturermaterial.vendorshortname
            new TranslationSeedItem("entity.manufacturermaterial.vendorshortname", "zh-HK", "经销商简称_hk", "经销商简称（冗余）"),

            // entity.manufacturermaterial.suppliercode
            new TranslationSeedItem("entity.manufacturermaterial.suppliercode", "en-US", "供货商编码_us", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）"),
            // entity.manufacturermaterial.suppliercode
            new TranslationSeedItem("entity.manufacturermaterial.suppliercode", "ja-JP", "供货商编码_jp", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）"),
            // entity.manufacturermaterial.suppliercode
            new TranslationSeedItem("entity.manufacturermaterial.suppliercode", "zh-CN", "供货商编码", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）"),
            // entity.manufacturermaterial.suppliercode
            new TranslationSeedItem("entity.manufacturermaterial.suppliercode", "zh-HK", "供货商编码_hk", "供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）"),

            // entity.manufacturermaterial.suppliershortname
            new TranslationSeedItem("entity.manufacturermaterial.suppliershortname", "en-US", "供货商简称_us", "供货商简称（冗余）"),
            // entity.manufacturermaterial.suppliershortname
            new TranslationSeedItem("entity.manufacturermaterial.suppliershortname", "ja-JP", "供货商简称_jp", "供货商简称（冗余）"),
            // entity.manufacturermaterial.suppliershortname
            new TranslationSeedItem("entity.manufacturermaterial.suppliershortname", "zh-CN", "供货商简称", "供货商简称（冗余）"),
            // entity.manufacturermaterial.suppliershortname
            new TranslationSeedItem("entity.manufacturermaterial.suppliershortname", "zh-HK", "供货商简称_hk", "供货商简称（冗余）"),

            // entity.manufacturermaterial.materialtype
            new TranslationSeedItem("entity.manufacturermaterial.materialtype", "en-US", "物料类型_us", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),
            // entity.manufacturermaterial.materialtype
            new TranslationSeedItem("entity.manufacturermaterial.materialtype", "ja-JP", "物料类型_jp", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),
            // entity.manufacturermaterial.materialtype
            new TranslationSeedItem("entity.manufacturermaterial.materialtype", "zh-CN", "物料类型", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),
            // entity.manufacturermaterial.materialtype
            new TranslationSeedItem("entity.manufacturermaterial.materialtype", "zh-HK", "物料类型_hk", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),

            // entity.manufacturermaterial.materialgroup
            new TranslationSeedItem("entity.manufacturermaterial.materialgroup", "en-US", "物料组_us", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.manufacturermaterial.materialgroup
            new TranslationSeedItem("entity.manufacturermaterial.materialgroup", "ja-JP", "物料组_jp", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.manufacturermaterial.materialgroup
            new TranslationSeedItem("entity.manufacturermaterial.materialgroup", "zh-CN", "物料组", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.manufacturermaterial.materialgroup
            new TranslationSeedItem("entity.manufacturermaterial.materialgroup", "zh-HK", "物料组_hk", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),

            // entity.manufacturermaterial.internalmaterialcode
            new TranslationSeedItem("entity.manufacturermaterial.internalmaterialcode", "en-US", "内部物料编码_us", "内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）"),
            // entity.manufacturermaterial.internalmaterialcode
            new TranslationSeedItem("entity.manufacturermaterial.internalmaterialcode", "ja-JP", "内部物料编码_jp", "内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）"),
            // entity.manufacturermaterial.internalmaterialcode
            new TranslationSeedItem("entity.manufacturermaterial.internalmaterialcode", "zh-CN", "内部物料编码", "内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）"),
            // entity.manufacturermaterial.internalmaterialcode
            new TranslationSeedItem("entity.manufacturermaterial.internalmaterialcode", "zh-HK", "内部物料编码_hk", "内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）"),

            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "en-US", "制造商物料编码_us", "制造商物料编码（制造商内部的物料编码；前端表单选择编码规则后自动通过 TaktNumbering 制造商物料编码规则生成并展示，非手输；单据类型菜单：制造商物料）"),
            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "ja-JP", "制造商物料编码_jp", "制造商物料编码（制造商内部的物料编码；前端表单选择编码规则后自动通过 TaktNumbering 制造商物料编码规则生成并展示，非手输；单据类型菜单：制造商物料）"),
            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "zh-CN", "制造商物料编码", "制造商物料编码（制造商内部的物料编码；前端表单选择编码规则后自动通过 TaktNumbering 制造商物料编码规则生成并展示，非手输；单据类型菜单：制造商物料）"),
            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "zh-HK", "制造商物料编码_hk", "制造商物料编码（制造商内部的物料编码；前端表单选择编码规则后自动通过 TaktNumbering 制造商物料编码规则生成并展示，非手输；单据类型菜单：制造商物料）"),

            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "en-US", "制造商物料描述_us", "制造商物料描述"),
            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "ja-JP", "制造商物料描述_jp", "制造商物料描述"),
            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "zh-CN", "制造商物料描述", "制造商物料描述"),
            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "zh-HK", "制造商物料描述_hk", "制造商物料描述"),

            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "en-US", "制造商物料规格_us", "制造商物料规格"),
            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "ja-JP", "制造商物料规格_jp", "制造商物料规格"),
            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "zh-CN", "制造商物料规格", "制造商物料规格"),
            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "zh-HK", "制造商物料规格_hk", "制造商物料规格"),
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
