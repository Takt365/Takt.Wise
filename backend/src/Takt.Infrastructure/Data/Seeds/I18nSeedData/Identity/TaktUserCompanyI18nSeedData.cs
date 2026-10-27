// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktUserCompanyI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktUserCompany 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktUserCompany 实体国际化翻译种子（键前缀 entity.usercompany.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktUserCompanyI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktUserCompany 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 usercompany 实体翻译...", tenantCode);

        foreach (var item in GetUserCompanyTranslations())
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

        TaktLogger.Information("TaktUserCompany 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktUserCompany 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.usercompany._self / entity.usercompany.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetUserCompanyTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.usercompany._self
            new TranslationSeedItem("entity.usercompany._self", "en-US", "User Company Information_us", "实体名称"),
            // entity.usercompany._self
            new TranslationSeedItem("entity.usercompany._self", "ja-JP", "用户公司关联信息_jp", "实体名称"),
            // entity.usercompany._self
            new TranslationSeedItem("entity.usercompany._self", "zh-CN", "用户公司关联信息", "实体名称"),
            // entity.usercompany._self
            new TranslationSeedItem("entity.usercompany._self", "zh-HK", "用户公司关联信息_hk", "实体名称"),

            // entity.usercompany.userid
            new TranslationSeedItem("entity.usercompany.userid", "en-US", "用户ID_us", "用户ID"),
            // entity.usercompany.userid
            new TranslationSeedItem("entity.usercompany.userid", "ja-JP", "用户ID_jp", "用户ID"),
            // entity.usercompany.userid
            new TranslationSeedItem("entity.usercompany.userid", "zh-CN", "用户ID", "用户ID"),
            // entity.usercompany.userid
            new TranslationSeedItem("entity.usercompany.userid", "zh-HK", "用户ID_hk", "用户ID"),

            // entity.usercompany.isdefault
            new TranslationSeedItem("entity.usercompany.isdefault", "en-US", "默认公司_us", "是否默认登录公司（字典 sys_yes_no；1=是 0=否） 同一用户在同一租户下仅应有一条为 1；登录时由 TaktAuthService 按 IsDefault=1 解析默认公司 演示种子 TaktUserCompanySeedData 为所有用户关联全部公司，默认登录公司为 <c>2300</c>"),
            // entity.usercompany.isdefault
            new TranslationSeedItem("entity.usercompany.isdefault", "ja-JP", "默认公司_jp", "是否默认登录公司（字典 sys_yes_no；1=是 0=否） 同一用户在同一租户下仅应有一条为 1；登录时由 TaktAuthService 按 IsDefault=1 解析默认公司 演示种子 TaktUserCompanySeedData 为所有用户关联全部公司，默认登录公司为 <c>2300</c>"),
            // entity.usercompany.isdefault
            new TranslationSeedItem("entity.usercompany.isdefault", "zh-CN", "默认公司", "是否默认登录公司（字典 sys_yes_no；1=是 0=否） 同一用户在同一租户下仅应有一条为 1；登录时由 TaktAuthService 按 IsDefault=1 解析默认公司 演示种子 TaktUserCompanySeedData 为所有用户关联全部公司，默认登录公司为 <c>2300</c>"),
            // entity.usercompany.isdefault
            new TranslationSeedItem("entity.usercompany.isdefault", "zh-HK", "默认公司_hk", "是否默认登录公司（字典 sys_yes_no；1=是 0=否） 同一用户在同一租户下仅应有一条为 1；登录时由 TaktAuthService 按 IsDefault=1 解析默认公司 演示种子 TaktUserCompanySeedData 为所有用户关联全部公司，默认登录公司为 <c>2300</c>"),

            // entity.usercompany.user
            new TranslationSeedItem("entity.usercompany.user", "en-US", "用户_us", "用户（多对一）"),
            // entity.usercompany.user
            new TranslationSeedItem("entity.usercompany.user", "ja-JP", "用户_jp", "用户（多对一）"),
            // entity.usercompany.user
            new TranslationSeedItem("entity.usercompany.user", "zh-CN", "用户", "用户（多对一）"),
            // entity.usercompany.user
            new TranslationSeedItem("entity.usercompany.user", "zh-HK", "用户_hk", "用户（多对一）"),

            // entity.usercompany.company
            new TranslationSeedItem("entity.usercompany.company", "en-US", "可访问公司_us", "可访问公司（多对一，按 CompanyCode 关联）"),
            // entity.usercompany.company
            new TranslationSeedItem("entity.usercompany.company", "ja-JP", "可访问公司_jp", "可访问公司（多对一，按 CompanyCode 关联）"),
            // entity.usercompany.company
            new TranslationSeedItem("entity.usercompany.company", "zh-CN", "可访问公司", "可访问公司（多对一，按 CompanyCode 关联）"),
            // entity.usercompany.company
            new TranslationSeedItem("entity.usercompany.company", "zh-HK", "可访问公司_hk", "可访问公司（多对一，按 CompanyCode 关联）"),
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
