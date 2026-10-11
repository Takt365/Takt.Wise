// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktRoleI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRole 实体字段国际化种子（已对齐前端 locales：src/locales/identity/role）
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
/// TaktRole 实体国际化翻译种子（键前缀 entity.role.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRole 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 role 实体翻译...", tenantCode);

        foreach (var item in GetRoleTranslations())
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

        TaktLogger.Information("TaktRole 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRole 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.role._self / entity.role.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetRoleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.role._self
            new TranslationSeedItem("entity.role._self", "en-US", "Role Information_us", "实体名称"),
            // entity.role._self
            new TranslationSeedItem("entity.role._self", "ja-JP", "角色信息_jp", "实体名称"),
            // entity.role._self
            new TranslationSeedItem("entity.role._self", "zh-CN", "角色信息", "实体名称"),
            // entity.role._self
            new TranslationSeedItem("entity.role._self", "zh-HK", "角色信息_hk", "实体名称"),

            // entity.role.code
            new TranslationSeedItem("entity.role.code", "en-US", "角色编码_us", "角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）"),
            // entity.role.code
            new TranslationSeedItem("entity.role.code", "ja-JP", "角色编码_jp", "角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）"),
            // entity.role.code
            new TranslationSeedItem("entity.role.code", "zh-CN", "角色编码", "角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）"),
            // entity.role.code
            new TranslationSeedItem("entity.role.code", "zh-HK", "角色编码_hk", "角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）"),

            // entity.role.name
            new TranslationSeedItem("entity.role.name", "en-US", "角色名称_us", "角色名称"),
            // entity.role.name
            new TranslationSeedItem("entity.role.name", "ja-JP", "角色名称_jp", "角色名称"),
            // entity.role.name
            new TranslationSeedItem("entity.role.name", "zh-CN", "角色名称", "角色名称"),
            // entity.role.name
            new TranslationSeedItem("entity.role.name", "zh-HK", "角色名称_hk", "角色名称"),

            // entity.role.datascope
            new TranslationSeedItem("entity.role.datascope", "en-US", "数据权限范围_us", "数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）"),
            // entity.role.datascope
            new TranslationSeedItem("entity.role.datascope", "ja-JP", "数据权限范围_jp", "数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）"),
            // entity.role.datascope
            new TranslationSeedItem("entity.role.datascope", "zh-CN", "数据权限范围", "数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）"),
            // entity.role.datascope
            new TranslationSeedItem("entity.role.datascope", "zh-HK", "数据权限范围_hk", "数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）"),

            // entity.role.isbuiltin
            new TranslationSeedItem("entity.role.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；种子角色为内置，不允许删除）"),
            // entity.role.isbuiltin
            new TranslationSeedItem("entity.role.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；种子角色为内置，不允许删除）"),
            // entity.role.isbuiltin
            new TranslationSeedItem("entity.role.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；种子角色为内置，不允许删除）"),
            // entity.role.isbuiltin
            new TranslationSeedItem("entity.role.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；种子角色为内置，不允许删除）"),

            // entity.role.description
            new TranslationSeedItem("entity.role.description", "en-US", "角色描述_us", "角色描述"),
            // entity.role.description
            new TranslationSeedItem("entity.role.description", "ja-JP", "角色描述_jp", "角色描述"),
            // entity.role.description
            new TranslationSeedItem("entity.role.description", "zh-CN", "角色描述", "角色描述"),
            // entity.role.description
            new TranslationSeedItem("entity.role.description", "zh-HK", "角色描述_hk", "角色描述"),

            // entity.role.sortorder
            new TranslationSeedItem("entity.role.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.role.sortorder
            new TranslationSeedItem("entity.role.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.role.sortorder
            new TranslationSeedItem("entity.role.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.role.sortorder
            new TranslationSeedItem("entity.role.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.role.status
            new TranslationSeedItem("entity.role.status", "en-US", "状态_us", "状态（字典 sys_normal_disable）"),
            // entity.role.status
            new TranslationSeedItem("entity.role.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable）"),
            // entity.role.status
            new TranslationSeedItem("entity.role.status", "zh-CN", "状态", "状态（字典 sys_normal_disable）"),
            // entity.role.status
            new TranslationSeedItem("entity.role.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable）"),

            // entity.role.menus
            new TranslationSeedItem("entity.role.menus", "en-US", "角色菜单权限关联_us", "角色菜单权限关联（RBAC，表 takt_identity_role_menu）"),
            // entity.role.menus
            new TranslationSeedItem("entity.role.menus", "ja-JP", "角色菜单权限关联_jp", "角色菜单权限关联（RBAC，表 takt_identity_role_menu）"),
            // entity.role.menus
            new TranslationSeedItem("entity.role.menus", "zh-CN", "角色菜单权限关联", "角色菜单权限关联（RBAC，表 takt_identity_role_menu）"),
            // entity.role.menus
            new TranslationSeedItem("entity.role.menus", "zh-HK", "角色菜单权限关联_hk", "角色菜单权限关联（RBAC，表 takt_identity_role_menu）"),

            // entity.role.companies
            new TranslationSeedItem("entity.role.companies", "en-US", "角色可访问公司关联_us", "角色可访问公司关联（RBAC，表 takt_identity_role_company）"),
            // entity.role.companies
            new TranslationSeedItem("entity.role.companies", "ja-JP", "角色可访问公司关联_jp", "角色可访问公司关联（RBAC，表 takt_identity_role_company）"),
            // entity.role.companies
            new TranslationSeedItem("entity.role.companies", "zh-CN", "角色可访问公司关联", "角色可访问公司关联（RBAC，表 takt_identity_role_company）"),
            // entity.role.companies
            new TranslationSeedItem("entity.role.companies", "zh-HK", "角色可访问公司关联_hk", "角色可访问公司关联（RBAC，表 takt_identity_role_company）"),

            // entity.role.depts
            new TranslationSeedItem("entity.role.depts", "en-US", "自定义数据权限关联部门_us", "自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept）"),
            // entity.role.depts
            new TranslationSeedItem("entity.role.depts", "ja-JP", "自定义数据权限关联部门_jp", "自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept）"),
            // entity.role.depts
            new TranslationSeedItem("entity.role.depts", "zh-CN", "自定义数据权限关联部门", "自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept）"),
            // entity.role.depts
            new TranslationSeedItem("entity.role.depts", "zh-HK", "自定义数据权限关联部门_hk", "自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept）"),

            // entity.role.userroles
            new TranslationSeedItem("entity.role.userroles", "en-US", "拥有该角色的用户关联_us", "拥有该角色的用户关联（RBAC，表 takt_identity_user_role）"),
            // entity.role.userroles
            new TranslationSeedItem("entity.role.userroles", "ja-JP", "拥有该角色的用户关联_jp", "拥有该角色的用户关联（RBAC，表 takt_identity_user_role）"),
            // entity.role.userroles
            new TranslationSeedItem("entity.role.userroles", "zh-CN", "拥有该角色的用户关联", "拥有该角色的用户关联（RBAC，表 takt_identity_user_role）"),
            // entity.role.userroles
            new TranslationSeedItem("entity.role.userroles", "zh-HK", "拥有该角色的用户关联_hk", "拥有该角色的用户关联（RBAC，表 takt_identity_user_role）"),
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
