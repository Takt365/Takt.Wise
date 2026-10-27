// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktVisitLogI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktVisitLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging;

/// <summary>
/// TaktVisitLog 实体国际化翻译种子（键前缀 entity.visitlog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktVisitLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktVisitLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 visitlog 实体翻译...", tenantCode);

        foreach (var item in GetVisitLogTranslations())
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

        TaktLogger.Information("TaktVisitLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktVisitLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.visitlog._self / entity.visitlog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetVisitLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.visitlog._self
            new TranslationSeedItem("entity.visitlog._self", "en-US", "Visit Log Information_us", "实体名称"),
            // entity.visitlog._self
            new TranslationSeedItem("entity.visitlog._self", "ja-JP", "用户日访问量统计信息_jp", "实体名称"),
            // entity.visitlog._self
            new TranslationSeedItem("entity.visitlog._self", "zh-CN", "用户日访问量统计信息", "实体名称"),
            // entity.visitlog._self
            new TranslationSeedItem("entity.visitlog._self", "zh-HK", "用户日访问量统计信息_hk", "实体名称"),

            // entity.visitlog.userid
            new TranslationSeedItem("entity.visitlog.userid", "en-US", "用户ID_us", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.visitlog.userid
            new TranslationSeedItem("entity.visitlog.userid", "ja-JP", "用户ID_jp", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.visitlog.userid
            new TranslationSeedItem("entity.visitlog.userid", "zh-CN", "用户ID", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),
            // entity.visitlog.userid
            new TranslationSeedItem("entity.visitlog.userid", "zh-HK", "用户ID_hk", "用户 ID（选项 TaktUsers/options；DictValue=Id）"),

            // entity.visitlog.username
            new TranslationSeedItem("entity.visitlog.username", "en-US", "用户名_us", "用户名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.visitlog.username
            new TranslationSeedItem("entity.visitlog.username", "ja-JP", "用户名_jp", "用户名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.visitlog.username
            new TranslationSeedItem("entity.visitlog.username", "zh-CN", "用户名", "用户名（冗余：按对应 Id 取主数据名称联动）"),
            // entity.visitlog.username
            new TranslationSeedItem("entity.visitlog.username", "zh-HK", "用户名_hk", "用户名（冗余：按对应 Id 取主数据名称联动）"),

            // entity.visitlog.statdate
            new TranslationSeedItem("entity.visitlog.statdate", "en-US", "统计日期_us", "统计日期（自然日，不含时分秒）"),
            // entity.visitlog.statdate
            new TranslationSeedItem("entity.visitlog.statdate", "ja-JP", "统计日期_jp", "统计日期（自然日，不含时分秒）"),
            // entity.visitlog.statdate
            new TranslationSeedItem("entity.visitlog.statdate", "zh-CN", "统计日期", "统计日期（自然日，不含时分秒）"),
            // entity.visitlog.statdate
            new TranslationSeedItem("entity.visitlog.statdate", "zh-HK", "统计日期_hk", "统计日期（自然日，不含时分秒）"),

            // entity.visitlog.visitcount
            new TranslationSeedItem("entity.visitlog.visitcount", "en-US", "当日访问次数_us", "当日访问次数（成功登录/进入系统次数）"),
            // entity.visitlog.visitcount
            new TranslationSeedItem("entity.visitlog.visitcount", "ja-JP", "当日访问次数_jp", "当日访问次数（成功登录/进入系统次数）"),
            // entity.visitlog.visitcount
            new TranslationSeedItem("entity.visitlog.visitcount", "zh-CN", "当日访问次数", "当日访问次数（成功登录/进入系统次数）"),
            // entity.visitlog.visitcount
            new TranslationSeedItem("entity.visitlog.visitcount", "zh-HK", "当日访问次数_hk", "当日访问次数（成功登录/进入系统次数）"),
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
        translation.ResourceGroup = "Logging";
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
