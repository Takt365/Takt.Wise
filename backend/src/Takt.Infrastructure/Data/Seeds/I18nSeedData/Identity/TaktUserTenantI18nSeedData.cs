// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktUserTenantI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktUserTenant 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity;

/// <summary>
/// TaktUserTenant 实体国际化翻译种子（键前缀 entity.usertenant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktUserTenantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktUserTenant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 usertenant 实体翻译...", tenantCode);

        foreach (var item in GetUserTenantTranslations())
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

        TaktLogger.Information("TaktUserTenant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktUserTenant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.usertenant._self / entity.usertenant.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetUserTenantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.usertenant._self
            new TranslationSeedItem("entity.usertenant._self", "en-US", "User Tenant Information_us", "实体名称"),
            // entity.usertenant._self
            new TranslationSeedItem("entity.usertenant._self", "ja-JP", "用户-租户关联信息_jp", "实体名称"),
            // entity.usertenant._self
            new TranslationSeedItem("entity.usertenant._self", "zh-CN", "用户-租户关联信息", "实体名称"),
            // entity.usertenant._self
            new TranslationSeedItem("entity.usertenant._self", "zh-HK", "用户-租户关联信息_hk", "实体名称"),

            // entity.usertenant.userid
            new TranslationSeedItem("entity.usertenant.userid", "en-US", "用户ID_us", "用户ID"),
            // entity.usertenant.userid
            new TranslationSeedItem("entity.usertenant.userid", "ja-JP", "用户ID_jp", "用户ID"),
            // entity.usertenant.userid
            new TranslationSeedItem("entity.usertenant.userid", "zh-CN", "用户ID", "用户ID"),
            // entity.usertenant.userid
            new TranslationSeedItem("entity.usertenant.userid", "zh-HK", "用户ID_hk", "用户ID"),

            // entity.usertenant.isdefault
            new TranslationSeedItem("entity.usertenant.isdefault", "en-US", "是否默认租户_us", "是否默认登录租户（字典 sys_yes_no；同一用户在同一公司下仅应有一条为是）"),
            // entity.usertenant.isdefault
            new TranslationSeedItem("entity.usertenant.isdefault", "ja-JP", "是否默认租户_jp", "是否默认登录租户（字典 sys_yes_no；同一用户在同一公司下仅应有一条为是）"),
            // entity.usertenant.isdefault
            new TranslationSeedItem("entity.usertenant.isdefault", "zh-CN", "是否默认租户", "是否默认登录租户（字典 sys_yes_no；同一用户在同一公司下仅应有一条为是）"),
            // entity.usertenant.isdefault
            new TranslationSeedItem("entity.usertenant.isdefault", "zh-HK", "是否默认租户_hk", "是否默认登录租户（字典 sys_yes_no；同一用户在同一公司下仅应有一条为是）"),

            // entity.usertenant.user
            new TranslationSeedItem("entity.usertenant.user", "en-US", "用户_us", "用户（多对一）"),
            // entity.usertenant.user
            new TranslationSeedItem("entity.usertenant.user", "ja-JP", "用户_jp", "用户（多对一）"),
            // entity.usertenant.user
            new TranslationSeedItem("entity.usertenant.user", "zh-CN", "用户", "用户（多对一）"),
            // entity.usertenant.user
            new TranslationSeedItem("entity.usertenant.user", "zh-HK", "用户_hk", "用户（多对一）"),

            // entity.usertenant.tenant
            new TranslationSeedItem("entity.usertenant.tenant", "en-US", "可访问租户_us", "可访问租户（多对一，按 TenantCode 关联）"),
            // entity.usertenant.tenant
            new TranslationSeedItem("entity.usertenant.tenant", "ja-JP", "可访问租户_jp", "可访问租户（多对一，按 TenantCode 关联）"),
            // entity.usertenant.tenant
            new TranslationSeedItem("entity.usertenant.tenant", "zh-CN", "可访问租户", "可访问租户（多对一，按 TenantCode 关联）"),
            // entity.usertenant.tenant
            new TranslationSeedItem("entity.usertenant.tenant", "zh-HK", "可访问租户_hk", "可访问租户（多对一，按 TenantCode 关联）"),
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
        translation.ResourceGroup = "Identity";
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
