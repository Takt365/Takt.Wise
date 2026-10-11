// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Materials
// 文件名称：TaktMaterialGroupI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMaterialGroup 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktMaterialGroup 实体国际化翻译种子（键前缀 entity.materialgroup.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMaterialGroupI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMaterialGroup 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 materialgroup 实体翻译...", tenantCode);

        foreach (var item in GetMaterialGroupTranslations())
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

        TaktLogger.Information("TaktMaterialGroup 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMaterialGroup 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.materialgroup._self / entity.materialgroup.{{field}}；ResourceGroup=Materials；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMaterialGroupTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.materialgroup._self
            new TranslationSeedItem("entity.materialgroup._self", "en-US", "Material Group Information_us", "实体名称"),
            // entity.materialgroup._self
            new TranslationSeedItem("entity.materialgroup._self", "ja-JP", "Takt物料组主数据信息_jp", "实体名称"),
            // entity.materialgroup._self
            new TranslationSeedItem("entity.materialgroup._self", "zh-CN", "Takt物料组主数据信息", "实体名称"),
            // entity.materialgroup._self
            new TranslationSeedItem("entity.materialgroup._self", "zh-HK", "Takt物料组主数据信息_hk", "实体名称"),

            // entity.materialgroup.code
            new TranslationSeedItem("entity.materialgroup.code", "en-US", "物料组编码_us", "物料组编码（与员工编码同长 6；租户内唯一）"),
            // entity.materialgroup.code
            new TranslationSeedItem("entity.materialgroup.code", "ja-JP", "物料组编码_jp", "物料组编码（与员工编码同长 6；租户内唯一）"),
            // entity.materialgroup.code
            new TranslationSeedItem("entity.materialgroup.code", "zh-CN", "物料组编码", "物料组编码（与员工编码同长 6；租户内唯一）"),
            // entity.materialgroup.code
            new TranslationSeedItem("entity.materialgroup.code", "zh-HK", "物料组编码_hk", "物料组编码（与员工编码同长 6；租户内唯一）"),

            // entity.materialgroup.name
            new TranslationSeedItem("entity.materialgroup.name", "en-US", "物料组名称_us", "物料组名称（group_name）"),
            // entity.materialgroup.name
            new TranslationSeedItem("entity.materialgroup.name", "ja-JP", "物料组名称_jp", "物料组名称（group_name）"),
            // entity.materialgroup.name
            new TranslationSeedItem("entity.materialgroup.name", "zh-CN", "物料组名称", "物料组名称（group_name）"),
            // entity.materialgroup.name
            new TranslationSeedItem("entity.materialgroup.name", "zh-HK", "物料组名称_hk", "物料组名称（group_name）"),

            // entity.materialgroup.sortorder
            new TranslationSeedItem("entity.materialgroup.sortorder", "en-US", "排序号_us", "排序号（回填）（sort；越小越靠前）"),
            // entity.materialgroup.sortorder
            new TranslationSeedItem("entity.materialgroup.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（sort；越小越靠前）"),
            // entity.materialgroup.sortorder
            new TranslationSeedItem("entity.materialgroup.sortorder", "zh-CN", "排序号", "排序号（回填）（sort；越小越靠前）"),
            // entity.materialgroup.sortorder
            new TranslationSeedItem("entity.materialgroup.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（sort；越小越靠前）"),

            // entity.materialgroup.description
            new TranslationSeedItem("entity.materialgroup.description", "en-US", "物料组描述_us", "物料组描述（description）"),
            // entity.materialgroup.description
            new TranslationSeedItem("entity.materialgroup.description", "ja-JP", "物料组描述_jp", "物料组描述（description）"),
            // entity.materialgroup.description
            new TranslationSeedItem("entity.materialgroup.description", "zh-CN", "物料组描述", "物料组描述（description）"),
            // entity.materialgroup.description
            new TranslationSeedItem("entity.materialgroup.description", "zh-HK", "物料组描述_hk", "物料组描述（description）"),
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
