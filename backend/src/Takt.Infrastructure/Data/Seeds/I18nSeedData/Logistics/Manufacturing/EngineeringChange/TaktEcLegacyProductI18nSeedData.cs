// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcLegacyProductI18nSeedData.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：旧品管制视图翻译种子（_self + 生管旧品处理）；其余业务字段复用 entity.ecdetail.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 旧品管制视图国际化翻译种子（键前缀 entity.eclegacyproduct.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcLegacyProductI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化旧品管制视图自称翻译
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入与更新条数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktEcLegacyProduct 实体国际化翻译种子...");
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
        foreach (var item in GetEcLegacyProductTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }
        TaktLogger.Information("TaktEcLegacyProduct 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 旧品管制自称 + 生管旧品处理（与明细字典旧品处理区分；其余字段走 ecdetail 种子）
    /// </summary>
    private static List<TranslationSeedItem> GetEcLegacyProductTranslations()
    {
        return new List<TranslationSeedItem>
        {
            new TranslationSeedItem("entity.eclegacyproduct._self", "en-US", "旧品管制_us", "实体名称"),
            new TranslationSeedItem("entity.eclegacyproduct._self", "ja-JP", "旧品管制_jp", "实体名称"),
            new TranslationSeedItem("entity.eclegacyproduct._self", "zh-CN", "旧品管制", "实体名称"),
            new TranslationSeedItem("entity.eclegacyproduct._self", "zh-HK", "旧品管制_hk", "实体名称"),
            new TranslationSeedItem("entity.eclegacyproduct.oldproducthandling", "en-US", "生管旧品处理_us", "生管课执行行旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition）"),
            new TranslationSeedItem("entity.eclegacyproduct.oldproducthandling", "ja-JP", "生管旧品处理_jp", "生管课执行行旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition）"),
            new TranslationSeedItem("entity.eclegacyproduct.oldproducthandling", "zh-CN", "生管旧品处理", "生管课执行行旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition）"),
            new TranslationSeedItem("entity.eclegacyproduct.oldproducthandling", "zh-HK", "生管旧品处理_hk", "生管课执行行旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段
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
        translation.ResourceGroup = "EngineeringChange";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    /// <summary>
    /// 按 I18nKey + CultureCode 幂等写入翻译
    /// </summary>
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
    /// 翻译种子项
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
