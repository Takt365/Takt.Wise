// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktWorkCenter 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Aps;

/// <summary>
/// TaktWorkCenter 实体国际化翻译种子（键前缀 entity.workcenter.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktWorkCenterI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktWorkCenter 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 workcenter 实体翻译...", tenantCode);

        foreach (var item in GetWorkCenterTranslations())
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

        TaktLogger.Information("TaktWorkCenter 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktWorkCenter 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.workcenter._self / entity.workcenter.{{field}}；ResourceGroup=Aps；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetWorkCenterTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.workcenter._self
            new TranslationSeedItem("entity.workcenter._self", "en-US", "Work Center Information_us", "实体名称"),
            // entity.workcenter._self
            new TranslationSeedItem("entity.workcenter._self", "ja-JP", "工作中心信息_jp", "实体名称"),
            // entity.workcenter._self
            new TranslationSeedItem("entity.workcenter._self", "zh-CN", "工作中心信息", "实体名称"),
            // entity.workcenter._self
            new TranslationSeedItem("entity.workcenter._self", "zh-HK", "工作中心信息_hk", "实体名称"),

            // entity.workcenter.code
            new TranslationSeedItem("entity.workcenter.code", "en-US", "工作中心编码_us", "工作中心编码"),
            // entity.workcenter.code
            new TranslationSeedItem("entity.workcenter.code", "ja-JP", "工作中心编码_jp", "工作中心编码"),
            // entity.workcenter.code
            new TranslationSeedItem("entity.workcenter.code", "zh-CN", "工作中心编码", "工作中心编码"),
            // entity.workcenter.code
            new TranslationSeedItem("entity.workcenter.code", "zh-HK", "工作中心编码_hk", "工作中心编码"),

            // entity.workcenter.description
            new TranslationSeedItem("entity.workcenter.description", "en-US", "工作中心描述_us", "工作中心描述"),
            // entity.workcenter.description
            new TranslationSeedItem("entity.workcenter.description", "ja-JP", "工作中心描述_jp", "工作中心描述"),
            // entity.workcenter.description
            new TranslationSeedItem("entity.workcenter.description", "zh-CN", "工作中心描述", "工作中心描述"),
            // entity.workcenter.description
            new TranslationSeedItem("entity.workcenter.description", "zh-HK", "工作中心描述_hk", "工作中心描述"),

            // entity.workcenter.status
            new TranslationSeedItem("entity.workcenter.status", "en-US", "工作中心状态_us", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.workcenter.status
            new TranslationSeedItem("entity.workcenter.status", "ja-JP", "工作中心状态_jp", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.workcenter.status
            new TranslationSeedItem("entity.workcenter.status", "zh-CN", "工作中心状态", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.workcenter.status
            new TranslationSeedItem("entity.workcenter.status", "zh-HK", "工作中心状态_hk", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),

            // entity.workcenter.resources
            new TranslationSeedItem("entity.workcenter.resources", "en-US", "工作中心资源列表_us", "工作中心资源列表"),
            // entity.workcenter.resources
            new TranslationSeedItem("entity.workcenter.resources", "ja-JP", "工作中心资源列表_jp", "工作中心资源列表"),
            // entity.workcenter.resources
            new TranslationSeedItem("entity.workcenter.resources", "zh-CN", "工作中心资源列表", "工作中心资源列表"),
            // entity.workcenter.resources
            new TranslationSeedItem("entity.workcenter.resources", "zh-HK", "工作中心资源列表_hk", "工作中心资源列表"),
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
        translation.ResourceGroup = "Aps";
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
