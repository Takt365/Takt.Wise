// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialDescriptionI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialDescription 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialDescription 实体国际化翻译种子（键前缀 entity.materialdescription.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialDescriptionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialDescription 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialdescription 实体翻译...", tenantCode);

        foreach (var item in GetMaterialDescriptionTranslations())
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

        TaktLogger.Information("TaktMaterialDescription 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialDescription 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialdescription._self / entity.materialdescription.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialDescriptionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialdescription._self
            new TranslationSeedItem("entity.materialdescription._self", "en-US", "Material Description Information_us", "实体名称"),
            // entity.materialdescription._self
            new TranslationSeedItem("entity.materialdescription._self", "ja-JP", "Takt物料多语言描述信息_jp", "实体名称"),
            // entity.materialdescription._self
            new TranslationSeedItem("entity.materialdescription._self", "zh-CN", "Takt物料多语言描述信息", "实体名称"),
            // entity.materialdescription._self
            new TranslationSeedItem("entity.materialdescription._self", "zh-HK", "Takt物料多语言描述信息_hk", "实体名称"),

            // entity.materialdescription.materialcode
            new TranslationSeedItem("entity.materialdescription.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),
            // entity.materialdescription.materialcode
            new TranslationSeedItem("entity.materialdescription.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),
            // entity.materialdescription.materialcode
            new TranslationSeedItem("entity.materialdescription.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),
            // entity.materialdescription.materialcode
            new TranslationSeedItem("entity.materialdescription.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）"),

            // entity.materialdescription.materialdescription
            new TranslationSeedItem("entity.materialdescription.materialdescription", "en-US", "物料描述_us", "物料描述"),
            // entity.materialdescription.materialdescription
            new TranslationSeedItem("entity.materialdescription.materialdescription", "ja-JP", "物料描述_jp", "物料描述"),
            // entity.materialdescription.materialdescription
            new TranslationSeedItem("entity.materialdescription.materialdescription", "zh-CN", "物料描述", "物料描述"),
            // entity.materialdescription.materialdescription
            new TranslationSeedItem("entity.materialdescription.materialdescription", "zh-HK", "物料描述_hk", "物料描述"),

            // entity.materialdescription.materialspecification
            new TranslationSeedItem("entity.materialdescription.materialspecification", "en-US", "物料规格_us", "物料规格"),
            // entity.materialdescription.materialspecification
            new TranslationSeedItem("entity.materialdescription.materialspecification", "ja-JP", "物料规格_jp", "物料规格"),
            // entity.materialdescription.materialspecification
            new TranslationSeedItem("entity.materialdescription.materialspecification", "zh-CN", "物料规格", "物料规格"),
            // entity.materialdescription.materialspecification
            new TranslationSeedItem("entity.materialdescription.materialspecification", "zh-HK", "物料规格_hk", "物料规格"),

            // entity.materialdescription.materialmodel
            new TranslationSeedItem("entity.materialdescription.materialmodel", "en-US", "物料型号_us", "物料型号"),
            // entity.materialdescription.materialmodel
            new TranslationSeedItem("entity.materialdescription.materialmodel", "ja-JP", "物料型号_jp", "物料型号"),
            // entity.materialdescription.materialmodel
            new TranslationSeedItem("entity.materialdescription.materialmodel", "zh-CN", "物料型号", "物料型号"),
            // entity.materialdescription.materialmodel
            new TranslationSeedItem("entity.materialdescription.materialmodel", "zh-HK", "物料型号_hk", "物料型号"),

            // entity.materialdescription.materiallongdescription
            new TranslationSeedItem("entity.materialdescription.materiallongdescription", "en-US", "物料长描述_us", "物料长描述"),
            // entity.materialdescription.materiallongdescription
            new TranslationSeedItem("entity.materialdescription.materiallongdescription", "ja-JP", "物料长描述_jp", "物料长描述"),
            // entity.materialdescription.materiallongdescription
            new TranslationSeedItem("entity.materialdescription.materiallongdescription", "zh-CN", "物料长描述", "物料长描述"),
            // entity.materialdescription.materiallongdescription
            new TranslationSeedItem("entity.materialdescription.materiallongdescription", "zh-HK", "物料长描述_hk", "物料长描述"),
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
