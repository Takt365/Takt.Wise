// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales
// 文件名称：TaktSellerMaterialI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSellerMaterial 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Sales;

/// <summary>
/// TaktSellerMaterial 实体国际化翻译种子（键前缀 entity.sellermaterial.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSellerMaterialI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSellerMaterial 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sellermaterial 实体翻译...", tenantCode);

        foreach (var item in GetSellerMaterialTranslations())
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

        TaktLogger.Information("TaktSellerMaterial 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSellerMaterial 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sellermaterial._self / entity.sellermaterial.{{field}}；ResourceGroup=Sales；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSellerMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sellermaterial._self
            new TranslationSeedItem("entity.sellermaterial._self", "en-US", "Seller Material Information_us", "实体名称"),
            // entity.sellermaterial._self
            new TranslationSeedItem("entity.sellermaterial._self", "ja-JP", "Takt销售商物料信息_jp", "实体名称"),
            // entity.sellermaterial._self
            new TranslationSeedItem("entity.sellermaterial._self", "zh-CN", "Takt销售商物料信息", "实体名称"),
            // entity.sellermaterial._self
            new TranslationSeedItem("entity.sellermaterial._self", "zh-HK", "Takt销售商物料信息_hk", "实体名称"),

            // entity.sellermaterial.customercode
            new TranslationSeedItem("entity.sellermaterial.customercode", "en-US", "客户编码_us", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）"),
            // entity.sellermaterial.customercode
            new TranslationSeedItem("entity.sellermaterial.customercode", "ja-JP", "客户编码_jp", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）"),
            // entity.sellermaterial.customercode
            new TranslationSeedItem("entity.sellermaterial.customercode", "zh-CN", "客户编码", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）"),
            // entity.sellermaterial.customercode
            new TranslationSeedItem("entity.sellermaterial.customercode", "zh-HK", "客户编码_hk", "客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）"),

            // entity.sellermaterial.customershortname
            new TranslationSeedItem("entity.sellermaterial.customershortname", "en-US", "客户简称_us", "客户简称（冗余）"),
            // entity.sellermaterial.customershortname
            new TranslationSeedItem("entity.sellermaterial.customershortname", "ja-JP", "客户简称_jp", "客户简称（冗余）"),
            // entity.sellermaterial.customershortname
            new TranslationSeedItem("entity.sellermaterial.customershortname", "zh-CN", "客户简称", "客户简称（冗余）"),
            // entity.sellermaterial.customershortname
            new TranslationSeedItem("entity.sellermaterial.customershortname", "zh-HK", "客户简称_hk", "客户简称（冗余）"),

            // entity.sellermaterial.clientcode
            new TranslationSeedItem("entity.sellermaterial.clientcode", "en-US", "客户端编码_us", "客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）"),
            // entity.sellermaterial.clientcode
            new TranslationSeedItem("entity.sellermaterial.clientcode", "ja-JP", "客户端编码_jp", "客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）"),
            // entity.sellermaterial.clientcode
            new TranslationSeedItem("entity.sellermaterial.clientcode", "zh-CN", "客户端编码", "客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）"),
            // entity.sellermaterial.clientcode
            new TranslationSeedItem("entity.sellermaterial.clientcode", "zh-HK", "客户端编码_hk", "客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）"),

            // entity.sellermaterial.clientshortname
            new TranslationSeedItem("entity.sellermaterial.clientshortname", "en-US", "客户端简称_us", "客户端简称（冗余）"),
            // entity.sellermaterial.clientshortname
            new TranslationSeedItem("entity.sellermaterial.clientshortname", "ja-JP", "客户端简称_jp", "客户端简称（冗余）"),
            // entity.sellermaterial.clientshortname
            new TranslationSeedItem("entity.sellermaterial.clientshortname", "zh-CN", "客户端简称", "客户端简称（冗余）"),
            // entity.sellermaterial.clientshortname
            new TranslationSeedItem("entity.sellermaterial.clientshortname", "zh-HK", "客户端简称_hk", "客户端简称（冗余）"),

            // entity.sellermaterial.materialtype
            new TranslationSeedItem("entity.sellermaterial.materialtype", "en-US", "物料类型_us", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),
            // entity.sellermaterial.materialtype
            new TranslationSeedItem("entity.sellermaterial.materialtype", "ja-JP", "物料类型_jp", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),
            // entity.sellermaterial.materialtype
            new TranslationSeedItem("entity.sellermaterial.materialtype", "zh-CN", "物料类型", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),
            // entity.sellermaterial.materialtype
            new TranslationSeedItem("entity.sellermaterial.materialtype", "zh-HK", "物料类型_hk", "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）"),

            // entity.sellermaterial.materialgroup
            new TranslationSeedItem("entity.sellermaterial.materialgroup", "en-US", "物料组_us", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.sellermaterial.materialgroup
            new TranslationSeedItem("entity.sellermaterial.materialgroup", "ja-JP", "物料组_jp", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.sellermaterial.materialgroup
            new TranslationSeedItem("entity.sellermaterial.materialgroup", "zh-CN", "物料组", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),
            // entity.sellermaterial.materialgroup
            new TranslationSeedItem("entity.sellermaterial.materialgroup", "zh-HK", "物料组_hk", "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）"),

            // entity.sellermaterial.internalmaterialcode
            new TranslationSeedItem("entity.sellermaterial.internalmaterialcode", "en-US", "内部物料编码_us", "内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）"),
            // entity.sellermaterial.internalmaterialcode
            new TranslationSeedItem("entity.sellermaterial.internalmaterialcode", "ja-JP", "内部物料编码_jp", "内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）"),
            // entity.sellermaterial.internalmaterialcode
            new TranslationSeedItem("entity.sellermaterial.internalmaterialcode", "zh-CN", "内部物料编码", "内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）"),
            // entity.sellermaterial.internalmaterialcode
            new TranslationSeedItem("entity.sellermaterial.internalmaterialcode", "zh-HK", "内部物料编码_hk", "内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）"),

            // entity.sellermaterial.materialcode
            new TranslationSeedItem("entity.sellermaterial.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sellermaterial.materialcode
            new TranslationSeedItem("entity.sellermaterial.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sellermaterial.materialcode
            new TranslationSeedItem("entity.sellermaterial.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.sellermaterial.materialcode
            new TranslationSeedItem("entity.sellermaterial.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.sellermaterial.materialdescription
            new TranslationSeedItem("entity.sellermaterial.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.sellermaterial.materialdescription
            new TranslationSeedItem("entity.sellermaterial.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.sellermaterial.materialdescription
            new TranslationSeedItem("entity.sellermaterial.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.sellermaterial.materialdescription
            new TranslationSeedItem("entity.sellermaterial.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.sellermaterial.code
            new TranslationSeedItem("entity.sellermaterial.code", "en-US", "销售商物料编码_us", "销售商物料编码（销售商内部的物料编码）"),
            // entity.sellermaterial.code
            new TranslationSeedItem("entity.sellermaterial.code", "ja-JP", "销售商物料编码_jp", "销售商物料编码（销售商内部的物料编码）"),
            // entity.sellermaterial.code
            new TranslationSeedItem("entity.sellermaterial.code", "zh-CN", "销售商物料编码", "销售商物料编码（销售商内部的物料编码）"),
            // entity.sellermaterial.code
            new TranslationSeedItem("entity.sellermaterial.code", "zh-HK", "销售商物料编码_hk", "销售商物料编码（销售商内部的物料编码）"),

            // entity.sellermaterial.description
            new TranslationSeedItem("entity.sellermaterial.description", "en-US", "销售商物料描述_us", "销售商物料描述"),
            // entity.sellermaterial.description
            new TranslationSeedItem("entity.sellermaterial.description", "ja-JP", "销售商物料描述_jp", "销售商物料描述"),
            // entity.sellermaterial.description
            new TranslationSeedItem("entity.sellermaterial.description", "zh-CN", "销售商物料描述", "销售商物料描述"),
            // entity.sellermaterial.description
            new TranslationSeedItem("entity.sellermaterial.description", "zh-HK", "销售商物料描述_hk", "销售商物料描述"),

            // entity.sellermaterial.specification
            new TranslationSeedItem("entity.sellermaterial.specification", "en-US", "销售商物料规格_us", "销售商物料规格"),
            // entity.sellermaterial.specification
            new TranslationSeedItem("entity.sellermaterial.specification", "ja-JP", "销售商物料规格_jp", "销售商物料规格"),
            // entity.sellermaterial.specification
            new TranslationSeedItem("entity.sellermaterial.specification", "zh-CN", "销售商物料规格", "销售商物料规格"),
            // entity.sellermaterial.specification
            new TranslationSeedItem("entity.sellermaterial.specification", "zh-HK", "销售商物料规格_hk", "销售商物料规格"),
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
        translation.ResourceGroup = "Sales";
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
