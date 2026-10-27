// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktTenantI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTenant 实体字段国际化种子（已对齐前端 locales：src/locales/identity/tenant）
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
/// TaktTenant 实体国际化翻译种子（键前缀 entity.tenant.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTenantI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTenant 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 tenant 实体翻译...", tenantCode);

        foreach (var item in GetTenantTranslations())
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

        TaktLogger.Information("TaktTenant 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTenant 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.tenant._self / entity.tenant.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTenantTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.tenant._self
            new TranslationSeedItem("entity.tenant._self", "en-US", "Tenant Information_us", "实体名称"),
            // entity.tenant._self
            new TranslationSeedItem("entity.tenant._self", "ja-JP", "租户信息_jp", "实体名称"),
            // entity.tenant._self
            new TranslationSeedItem("entity.tenant._self", "zh-CN", "租户信息", "实体名称"),
            // entity.tenant._self
            new TranslationSeedItem("entity.tenant._self", "zh-HK", "租户信息_hk", "实体名称"),

            // entity.tenant.name
            new TranslationSeedItem("entity.tenant.name", "en-US", "租户名称_us", "租户名称"),
            // entity.tenant.name
            new TranslationSeedItem("entity.tenant.name", "ja-JP", "租户名称_jp", "租户名称"),
            // entity.tenant.name
            new TranslationSeedItem("entity.tenant.name", "zh-CN", "租户名称", "租户名称"),
            // entity.tenant.name
            new TranslationSeedItem("entity.tenant.name", "zh-HK", "租户名称_hk", "租户名称"),

            // entity.tenant.subscriptionstarttime
            new TranslationSeedItem("entity.tenant.subscriptionstarttime", "en-US", "订阅开始时间_us", "订阅开始时间"),
            // entity.tenant.subscriptionstarttime
            new TranslationSeedItem("entity.tenant.subscriptionstarttime", "ja-JP", "订阅开始时间_jp", "订阅开始时间"),
            // entity.tenant.subscriptionstarttime
            new TranslationSeedItem("entity.tenant.subscriptionstarttime", "zh-CN", "订阅开始时间", "订阅开始时间"),
            // entity.tenant.subscriptionstarttime
            new TranslationSeedItem("entity.tenant.subscriptionstarttime", "zh-HK", "订阅开始时间_hk", "订阅开始时间"),

            // entity.tenant.subscriptionendtime
            new TranslationSeedItem("entity.tenant.subscriptionendtime", "en-US", "订阅结束时间_us", "订阅结束时间（9999/12/31 23:59:59表示长期有效）"),
            // entity.tenant.subscriptionendtime
            new TranslationSeedItem("entity.tenant.subscriptionendtime", "ja-JP", "订阅结束时间_jp", "订阅结束时间（9999/12/31 23:59:59表示长期有效）"),
            // entity.tenant.subscriptionendtime
            new TranslationSeedItem("entity.tenant.subscriptionendtime", "zh-CN", "订阅结束时间", "订阅结束时间（9999/12/31 23:59:59表示长期有效）"),
            // entity.tenant.subscriptionendtime
            new TranslationSeedItem("entity.tenant.subscriptionendtime", "zh-HK", "订阅结束时间_hk", "订阅结束时间（9999/12/31 23:59:59表示长期有效）"),

            // entity.tenant.contactname
            new TranslationSeedItem("entity.tenant.contactname", "en-US", "联系人姓名_us", "联系人姓名"),
            // entity.tenant.contactname
            new TranslationSeedItem("entity.tenant.contactname", "ja-JP", "联系人姓名_jp", "联系人姓名"),
            // entity.tenant.contactname
            new TranslationSeedItem("entity.tenant.contactname", "zh-CN", "联系人姓名", "联系人姓名"),
            // entity.tenant.contactname
            new TranslationSeedItem("entity.tenant.contactname", "zh-HK", "联系人姓名_hk", "联系人姓名"),

            // entity.tenant.contactphone
            new TranslationSeedItem("entity.tenant.contactphone", "en-US", "联系电话_us", "联系电话"),
            // entity.tenant.contactphone
            new TranslationSeedItem("entity.tenant.contactphone", "ja-JP", "联系电话_jp", "联系电话"),
            // entity.tenant.contactphone
            new TranslationSeedItem("entity.tenant.contactphone", "zh-CN", "联系电话", "联系电话"),
            // entity.tenant.contactphone
            new TranslationSeedItem("entity.tenant.contactphone", "zh-HK", "联系电话_hk", "联系电话"),

            // entity.tenant.contactemail
            new TranslationSeedItem("entity.tenant.contactemail", "en-US", "联系邮箱_us", "联系邮箱"),
            // entity.tenant.contactemail
            new TranslationSeedItem("entity.tenant.contactemail", "ja-JP", "联系邮箱_jp", "联系邮箱"),
            // entity.tenant.contactemail
            new TranslationSeedItem("entity.tenant.contactemail", "zh-CN", "联系邮箱", "联系邮箱"),
            // entity.tenant.contactemail
            new TranslationSeedItem("entity.tenant.contactemail", "zh-HK", "联系邮箱_hk", "联系邮箱"),

            // entity.tenant.isbuiltin
            new TranslationSeedItem("entity.tenant.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）"),
            // entity.tenant.isbuiltin
            new TranslationSeedItem("entity.tenant.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）"),
            // entity.tenant.isbuiltin
            new TranslationSeedItem("entity.tenant.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）"),
            // entity.tenant.isbuiltin
            new TranslationSeedItem("entity.tenant.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；种子租户 000/500/100 为内置，不允许删除）"),

            // entity.tenant.status
            new TranslationSeedItem("entity.tenant.status", "en-US", "状态_us", "状态（字典 sys_normal_disable）"),
            // entity.tenant.status
            new TranslationSeedItem("entity.tenant.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable）"),
            // entity.tenant.status
            new TranslationSeedItem("entity.tenant.status", "zh-CN", "状态", "状态（字典 sys_normal_disable）"),
            // entity.tenant.status
            new TranslationSeedItem("entity.tenant.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable）"),

            // entity.tenant.usertenants
            new TranslationSeedItem("entity.tenant.usertenants", "en-US", "可访问该租户的用户关联_us", "可访问该租户的用户关联（RBAC，表 takt_identity_user_tenant）"),
            // entity.tenant.usertenants
            new TranslationSeedItem("entity.tenant.usertenants", "ja-JP", "可访问该租户的用户关联_jp", "可访问该租户的用户关联（RBAC，表 takt_identity_user_tenant）"),
            // entity.tenant.usertenants
            new TranslationSeedItem("entity.tenant.usertenants", "zh-CN", "可访问该租户的用户关联", "可访问该租户的用户关联（RBAC，表 takt_identity_user_tenant）"),
            // entity.tenant.usertenants
            new TranslationSeedItem("entity.tenant.usertenants", "zh-HK", "可访问该租户的用户关联_hk", "可访问该租户的用户关联（RBAC，表 takt_identity_user_tenant）"),
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
