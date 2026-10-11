// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktGreetingsI18nSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：问候语国际化翻译种子数据初始化（5个时段 × 4种语言）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 问候语国际化翻译种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// 5个时段：早晨、上午、中午、下午、晚上（与前端 common.page.greeting.* 一致）
/// 4种语言：简体中文、英文、日文、香港繁体
/// </summary>
public class TaktGreetingsI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在菜单翻译之后）
    /// </summary>
    public int Order => 48;

    /// <summary>
    /// 初始化问候语国际化翻译种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化问候语国际化翻译种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过问候语国际化翻译种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化问候语国际化翻译数据...", tenantCode);

        foreach (var item in GetStandardGreetingsTranslations())
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

        TaktLogger.Information("问候语国际化翻译种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准问候语翻译列表
    /// 5个时段 × 4种语言 = 20条翻译记录
    /// 时段划分（与 WelcomeModule.vue 一致）：
    /// - 早晨 (00:00-08:59): common.page.greeting.morning
    /// - 上午 (09:00-11:59): common.page.greeting.forenoon
    /// - 中午 (12:00-13:59): common.page.greeting.noon
    /// - 下午 (14:00-17:59): common.page.greeting.afternoon
    /// - 晚上 (18:00-23:59): common.page.greeting.night
    /// </summary>
    private static List<TranslationSeedItem> GetStandardGreetingsTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // ========================================
            // 早晨问候 (00:00-08:59)
            // ========================================
            new TranslationSeedItem("common.page.greeting.morning", "zh-CN", "早上好！", "早晨时段问候语"),
            new TranslationSeedItem("common.page.greeting.morning", "en-US", "Good morning!", "Morning greeting"),
            new TranslationSeedItem("common.page.greeting.morning", "ja-JP", "おはようございます！", "朝の挨拶"),
            new TranslationSeedItem("common.page.greeting.morning", "zh-HK", "早上好！", "早晨时段问候语"),

            // ========================================
            // 上午问候 (09:00-11:59)
            // ========================================
            new TranslationSeedItem("common.page.greeting.forenoon", "zh-CN", "上午好！", "上午时段问候语"),
            new TranslationSeedItem("common.page.greeting.forenoon", "en-US", "Good morning!", "Forenoon greeting"),
            new TranslationSeedItem("common.page.greeting.forenoon", "ja-JP", "おはようございます！", "午前の挨拶"),
            new TranslationSeedItem("common.page.greeting.forenoon", "zh-HK", "上午好！", "上午时段问候语"),

            // ========================================
            // 中午问候 (12:00-13:59)
            // ========================================
            new TranslationSeedItem("common.page.greeting.noon", "zh-CN", "中午好！", "中午时段问候语"),
            new TranslationSeedItem("common.page.greeting.noon", "en-US", "Good afternoon!", "Noon greeting"),
            new TranslationSeedItem("common.page.greeting.noon", "ja-JP", "こんにちは！", "昼の挨拶"),
            new TranslationSeedItem("common.page.greeting.noon", "zh-HK", "中午好！", "中午时段问候语"),

            // ========================================
            // 下午问候 (14:00-17:59)
            // ========================================
            new TranslationSeedItem("common.page.greeting.afternoon", "zh-CN", "下午好！", "下午时段问候语"),
            new TranslationSeedItem("common.page.greeting.afternoon", "en-US", "Good afternoon!", "Afternoon greeting"),
            new TranslationSeedItem("common.page.greeting.afternoon", "ja-JP", "こんにちは！", "午後の挨拶"),
            new TranslationSeedItem("common.page.greeting.afternoon", "zh-HK", "下午好！", "下午时段问候语"),

            // ========================================
            // 晚上问候 (18:00-23:59)
            // ========================================
            new TranslationSeedItem("common.page.greeting.night", "zh-CN", "晚上好！", "晚上时段问候语"),
            new TranslationSeedItem("common.page.greeting.night", "en-US", "Good evening!", "Night greeting"),
            new TranslationSeedItem("common.page.greeting.night", "ja-JP", "こんばんは！", "夜の挨拶"),
            new TranslationSeedItem("common.page.greeting.night", "zh-HK", "晚上好！", "晚上时段问候语"),
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
        translation.ResourceGroup = "Foundation";
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