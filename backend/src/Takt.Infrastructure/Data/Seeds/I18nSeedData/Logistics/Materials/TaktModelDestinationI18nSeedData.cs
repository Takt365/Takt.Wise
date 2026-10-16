// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktModelDestinationI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktModelDestination 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktModelDestination 实体国际化翻译种子（键前缀 entity.modeldestination.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktModelDestinationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktModelDestination 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 modeldestination 实体翻译...", tenantCode);

        foreach (var item in GetModelDestinationTranslations())
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

        TaktLogger.Information("TaktModelDestination 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktModelDestination 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.modeldestination._self / entity.modeldestination.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetModelDestinationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.modeldestination._self
            new TranslationSeedItem("entity.modeldestination._self", "en-US", "Model Destination Information_us", "实体名称"),
            // entity.modeldestination._self
            new TranslationSeedItem("entity.modeldestination._self", "ja-JP", "Takt型号目的地信息_jp", "实体名称"),
            // entity.modeldestination._self
            new TranslationSeedItem("entity.modeldestination._self", "zh-CN", "Takt型号目的地信息", "实体名称"),
            // entity.modeldestination._self
            new TranslationSeedItem("entity.modeldestination._self", "zh-HK", "Takt型号目的地信息_hk", "实体名称"),

            // entity.modeldestination.materialcode
            new TranslationSeedItem("entity.modeldestination.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.modeldestination.materialcode
            new TranslationSeedItem("entity.modeldestination.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.modeldestination.materialcode
            new TranslationSeedItem("entity.modeldestination.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.modeldestination.materialcode
            new TranslationSeedItem("entity.modeldestination.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.modeldestination.materialdescription
            new TranslationSeedItem("entity.modeldestination.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.modeldestination.materialdescription
            new TranslationSeedItem("entity.modeldestination.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.modeldestination.materialdescription
            new TranslationSeedItem("entity.modeldestination.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.modeldestination.materialdescription
            new TranslationSeedItem("entity.modeldestination.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.modeldestination.modelcode
            new TranslationSeedItem("entity.modeldestination.modelcode", "en-US", "机种编码_us", "机种编码（40；前端表单选择编码规则后自动通过 TaktNumbering 机种编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),
            // entity.modeldestination.modelcode
            new TranslationSeedItem("entity.modeldestination.modelcode", "ja-JP", "机种编码_jp", "机种编码（40；前端表单选择编码规则后自动通过 TaktNumbering 机种编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),
            // entity.modeldestination.modelcode
            new TranslationSeedItem("entity.modeldestination.modelcode", "zh-CN", "机种编码", "机种编码（40；前端表单选择编码规则后自动通过 TaktNumbering 机种编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),
            // entity.modeldestination.modelcode
            new TranslationSeedItem("entity.modeldestination.modelcode", "zh-HK", "机种编码_hk", "机种编码（40；前端表单选择编码规则后自动通过 TaktNumbering 机种编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),

            // entity.modeldestination.modelname
            new TranslationSeedItem("entity.modeldestination.modelname", "en-US", "机种名称_us", "机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）"),
            // entity.modeldestination.modelname
            new TranslationSeedItem("entity.modeldestination.modelname", "ja-JP", "机种名称_jp", "机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）"),
            // entity.modeldestination.modelname
            new TranslationSeedItem("entity.modeldestination.modelname", "zh-CN", "机种名称", "机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）"),
            // entity.modeldestination.modelname
            new TranslationSeedItem("entity.modeldestination.modelname", "zh-HK", "机种名称_hk", "机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）"),

            // entity.modeldestination.destinationcode
            new TranslationSeedItem("entity.modeldestination.destinationcode", "en-US", "仕向地编码_us", "仕向地编码（40；前端表单选择编码规则后自动通过 TaktNumbering 仕向地编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),
            // entity.modeldestination.destinationcode
            new TranslationSeedItem("entity.modeldestination.destinationcode", "ja-JP", "仕向地编码_jp", "仕向地编码（40；前端表单选择编码规则后自动通过 TaktNumbering 仕向地编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),
            // entity.modeldestination.destinationcode
            new TranslationSeedItem("entity.modeldestination.destinationcode", "zh-CN", "仕向地编码", "仕向地编码（40；前端表单选择编码规则后自动通过 TaktNumbering 仕向地编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),
            // entity.modeldestination.destinationcode
            new TranslationSeedItem("entity.modeldestination.destinationcode", "zh-HK", "仕向地编码_hk", "仕向地编码（40；前端表单选择编码规则后自动通过 TaktNumbering 仕向地编码规则生成并展示，非手输；单据类型菜单：机种仕向）"),

            // entity.modeldestination.destinationname
            new TranslationSeedItem("entity.modeldestination.destinationname", "en-US", "仕向地名称_us", "仕向地名称（80）"),
            // entity.modeldestination.destinationname
            new TranslationSeedItem("entity.modeldestination.destinationname", "ja-JP", "仕向地名称_jp", "仕向地名称（80）"),
            // entity.modeldestination.destinationname
            new TranslationSeedItem("entity.modeldestination.destinationname", "zh-CN", "仕向地名称", "仕向地名称（80）"),
            // entity.modeldestination.destinationname
            new TranslationSeedItem("entity.modeldestination.destinationname", "zh-HK", "仕向地名称_hk", "仕向地名称（80）"),

            // entity.modeldestination.sortorder
            new TranslationSeedItem("entity.modeldestination.sortorder", "en-US", "排序号_us", "排序号（回填）（越小越靠前）"),
            // entity.modeldestination.sortorder
            new TranslationSeedItem("entity.modeldestination.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（越小越靠前）"),
            // entity.modeldestination.sortorder
            new TranslationSeedItem("entity.modeldestination.sortorder", "zh-CN", "排序号", "排序号（回填）（越小越靠前）"),
            // entity.modeldestination.sortorder
            new TranslationSeedItem("entity.modeldestination.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（越小越靠前）"),
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
