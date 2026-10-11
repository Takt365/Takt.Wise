// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktManufacturerMaterialI18nSeedData.cs
// 创建时间：2026-08-04
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials;

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
    /// I18nKey：entity.manufacturermaterial._self / entity.manufacturermaterial.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetManufacturerMaterialTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "en-US", "Manufacturer GeneralMaterial Information_us", "实体名称"),
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "ja-JP", "Takt制造商物料信息_jp", "实体名称"),
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "zh-CN", "Takt制造商物料信息", "实体名称"),
            // entity.manufacturermaterial._self
            new TranslationSeedItem("entity.manufacturermaterial._self", "zh-HK", "Takt制造商物料信息_hk", "实体名称"),

            // entity.manufacturermaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturermaterial.manufacturerid", "en-US", "制造商ID_us", "制造商 ID（选项 TaktManufacturers/options；DictValue=Id）"),
            // entity.manufacturermaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturermaterial.manufacturerid", "ja-JP", "制造商ID_jp", "制造商 ID（选项 TaktManufacturers/options；DictValue=Id）"),
            // entity.manufacturermaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturermaterial.manufacturerid", "zh-CN", "制造商ID", "制造商 ID（选项 TaktManufacturers/options；DictValue=Id）"),
            // entity.manufacturermaterial.manufacturerid
            new TranslationSeedItem("entity.manufacturermaterial.manufacturerid", "zh-HK", "制造商ID_hk", "制造商 ID（选项 TaktManufacturers/options；DictValue=Id）"),

            // entity.manufacturermaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturermaterial.manufacturercode", "en-US", "制造商编码_us", "制造商编码（冗余）"),
            // entity.manufacturermaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturermaterial.manufacturercode", "ja-JP", "制造商编码_jp", "制造商编码（冗余）"),
            // entity.manufacturermaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturermaterial.manufacturercode", "zh-CN", "制造商编码", "制造商编码（冗余）"),
            // entity.manufacturermaterial.manufacturercode
            new TranslationSeedItem("entity.manufacturermaterial.manufacturercode", "zh-HK", "制造商编码_hk", "制造商编码（冗余）"),

            // entity.manufacturermaterial.manufacturershortname
            new TranslationSeedItem("entity.manufacturermaterial.manufacturershortname", "en-US", "制造商简称_us", "制造商简称（冗余）"),
            // entity.manufacturermaterial.manufacturershortname
            new TranslationSeedItem("entity.manufacturermaterial.manufacturershortname", "ja-JP", "制造商简称_jp", "制造商简称（冗余）"),
            // entity.manufacturermaterial.manufacturershortname
            new TranslationSeedItem("entity.manufacturermaterial.manufacturershortname", "zh-CN", "制造商简称", "制造商简称（冗余）"),
            // entity.manufacturermaterial.manufacturershortname
            new TranslationSeedItem("entity.manufacturermaterial.manufacturershortname", "zh-HK", "制造商简称_hk", "制造商简称（冗余）"),

            // entity.manufacturermaterial.linenumber
            new TranslationSeedItem("entity.manufacturermaterial.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.manufacturermaterial.linenumber
            new TranslationSeedItem("entity.manufacturermaterial.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.manufacturermaterial.linenumber
            new TranslationSeedItem("entity.manufacturermaterial.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.manufacturermaterial.linenumber
            new TranslationSeedItem("entity.manufacturermaterial.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "en-US", "制造商物料编码_us", "制造商物料编码（制造商内部的物料编码）"),
            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "ja-JP", "制造商物料编码_jp", "制造商物料编码（制造商内部的物料编码）"),
            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "zh-CN", "制造商物料编码", "制造商物料编码（制造商内部的物料编码）"),
            // entity.manufacturermaterial.code
            new TranslationSeedItem("entity.manufacturermaterial.code", "zh-HK", "制造商物料编码_hk", "制造商物料编码（制造商内部的物料编码）"),

            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "en-US", "制造商物料描述_us", "制造商物料描述（制造商内部的物料描述）"),
            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "ja-JP", "制造商物料描述_jp", "制造商物料描述（制造商内部的物料描述）"),
            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "zh-CN", "制造商物料描述", "制造商物料描述（制造商内部的物料描述）"),
            // entity.manufacturermaterial.description
            new TranslationSeedItem("entity.manufacturermaterial.description", "zh-HK", "制造商物料描述_hk", "制造商物料描述（制造商内部的物料描述）"),

            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "en-US", "制造商物料规格_us", "制造商物料规格"),
            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "ja-JP", "制造商物料规格_jp", "制造商物料规格"),
            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "zh-CN", "制造商物料规格", "制造商物料规格"),
            // entity.manufacturermaterial.specification
            new TranslationSeedItem("entity.manufacturermaterial.specification", "zh-HK", "制造商物料规格_hk", "制造商物料规格"),

            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.manufacturermaterial.materialcode
            new TranslationSeedItem("entity.manufacturermaterial.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "en-US", "物料描述_us", "物料描述（回填：随物料）"),
            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "ja-JP", "物料描述_jp", "物料描述（回填：随物料）"),
            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "zh-CN", "物料描述", "物料描述（回填：随物料）"),
            // entity.manufacturermaterial.materialdescription
            new TranslationSeedItem("entity.manufacturermaterial.materialdescription", "zh-HK", "物料描述_hk", "物料描述（回填：随物料）"),

            // entity.manufacturermaterial.isobsolete
            new TranslationSeedItem("entity.manufacturermaterial.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.manufacturermaterial.isobsolete
            new TranslationSeedItem("entity.manufacturermaterial.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.manufacturermaterial.isobsolete
            new TranslationSeedItem("entity.manufacturermaterial.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.manufacturermaterial.isobsolete
            new TranslationSeedItem("entity.manufacturermaterial.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.manufacturermaterial.manufacturer
            new TranslationSeedItem("entity.manufacturermaterial.manufacturer", "en-US", "导航属性：关联的制造商_us", "导航属性：关联的制造商（主子表关系；多对一）"),
            // entity.manufacturermaterial.manufacturer
            new TranslationSeedItem("entity.manufacturermaterial.manufacturer", "ja-JP", "导航属性：关联的制造商_jp", "导航属性：关联的制造商（主子表关系；多对一）"),
            // entity.manufacturermaterial.manufacturer
            new TranslationSeedItem("entity.manufacturermaterial.manufacturer", "zh-CN", "导航属性：关联的制造商", "导航属性：关联的制造商（主子表关系；多对一）"),
            // entity.manufacturermaterial.manufacturer
            new TranslationSeedItem("entity.manufacturermaterial.manufacturer", "zh-HK", "导航属性：关联的制造商_hk", "导航属性：关联的制造商（主子表关系；多对一）"),
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
        translation.ResourceGroup = "Materials";
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
