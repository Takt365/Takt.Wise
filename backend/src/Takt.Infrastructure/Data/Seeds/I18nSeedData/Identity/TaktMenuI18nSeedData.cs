// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktMenuI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktMenu 实体字段国际化种子（已对齐前端 locales：src/locales/identity/menu）
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
/// TaktMenu 实体国际化翻译种子（键前缀 entity.menu.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktMenuI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktMenu 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 menu 实体翻译...", tenantCode);

        foreach (var item in GetMenuTranslations())
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

        TaktLogger.Information("TaktMenu 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktMenu 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.menu._self / entity.menu.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetMenuTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.menu._self
            new TranslationSeedItem("entity.menu._self", "en-US", "Menu Information_us", "实体名称"),
            // entity.menu._self
            new TranslationSeedItem("entity.menu._self", "ja-JP", "菜单信息_jp", "实体名称"),
            // entity.menu._self
            new TranslationSeedItem("entity.menu._self", "zh-CN", "菜单信息", "实体名称"),
            // entity.menu._self
            new TranslationSeedItem("entity.menu._self", "zh-HK", "菜单信息_hk", "实体名称"),

            // entity.menu.code
            new TranslationSeedItem("entity.menu.code", "en-US", "菜单编码_us", "菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique；前端表单选择编码规则后自动通过 TaktNumbering 菜单编码规则生成并展示，非手输；单据类型菜单：菜单管理）"),
            // entity.menu.code
            new TranslationSeedItem("entity.menu.code", "ja-JP", "菜单编码_jp", "菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique；前端表单选择编码规则后自动通过 TaktNumbering 菜单编码规则生成并展示，非手输；单据类型菜单：菜单管理）"),
            // entity.menu.code
            new TranslationSeedItem("entity.menu.code", "zh-CN", "菜单编码", "菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique；前端表单选择编码规则后自动通过 TaktNumbering 菜单编码规则生成并展示，非手输；单据类型菜单：菜单管理）"),
            // entity.menu.code
            new TranslationSeedItem("entity.menu.code", "zh-HK", "菜单编码_hk", "菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique；前端表单选择编码规则后自动通过 TaktNumbering 菜单编码规则生成并展示，非手输；单据类型菜单：菜单管理）"),

            // entity.menu.name
            new TranslationSeedItem("entity.menu.name", "en-US", "菜单名称_us", "菜单名称"),
            // entity.menu.name
            new TranslationSeedItem("entity.menu.name", "ja-JP", "菜单名称_jp", "菜单名称"),
            // entity.menu.name
            new TranslationSeedItem("entity.menu.name", "zh-CN", "菜单名称", "菜单名称"),
            // entity.menu.name
            new TranslationSeedItem("entity.menu.name", "zh-HK", "菜单名称_hk", "菜单名称"),

            // entity.menu.l10nkey
            new TranslationSeedItem("entity.menu.l10nkey", "en-US", "国际化键_us", "国际化键（用于多语言支持；varchar Length=140）"),
            // entity.menu.l10nkey
            new TranslationSeedItem("entity.menu.l10nkey", "ja-JP", "国际化键_jp", "国际化键（用于多语言支持；varchar Length=140）"),
            // entity.menu.l10nkey
            new TranslationSeedItem("entity.menu.l10nkey", "zh-CN", "国际化键", "国际化键（用于多语言支持；varchar Length=140）"),
            // entity.menu.l10nkey
            new TranslationSeedItem("entity.menu.l10nkey", "zh-HK", "国际化键_hk", "国际化键（用于多语言支持；varchar Length=140）"),

            // entity.menu.icon
            new TranslationSeedItem("entity.menu.icon", "en-US", "菜单图标_us", "菜单图标"),
            // entity.menu.icon
            new TranslationSeedItem("entity.menu.icon", "ja-JP", "菜单图标_jp", "菜单图标"),
            // entity.menu.icon
            new TranslationSeedItem("entity.menu.icon", "zh-CN", "菜单图标", "菜单图标"),
            // entity.menu.icon
            new TranslationSeedItem("entity.menu.icon", "zh-HK", "菜单图标_hk", "菜单图标"),

            // entity.menu.parentid
            new TranslationSeedItem("entity.menu.parentid", "en-US", "父菜单ID_us", "父菜单ID（0表示根菜单）"),
            // entity.menu.parentid
            new TranslationSeedItem("entity.menu.parentid", "ja-JP", "父菜单ID_jp", "父菜单ID（0表示根菜单）"),
            // entity.menu.parentid
            new TranslationSeedItem("entity.menu.parentid", "zh-CN", "父菜单ID", "父菜单ID（0表示根菜单）"),
            // entity.menu.parentid
            new TranslationSeedItem("entity.menu.parentid", "zh-HK", "父菜单ID_hk", "父菜单ID（0表示根菜单）"),

            // entity.menu.level
            new TranslationSeedItem("entity.menu.level", "en-US", "层级_us", "层级（1=一级菜单，2=二级菜单，以此类推）"),
            // entity.menu.level
            new TranslationSeedItem("entity.menu.level", "ja-JP", "层级_jp", "层级（1=一级菜单，2=二级菜单，以此类推）"),
            // entity.menu.level
            new TranslationSeedItem("entity.menu.level", "zh-CN", "层级", "层级（1=一级菜单，2=二级菜单，以此类推）"),
            // entity.menu.level
            new TranslationSeedItem("entity.menu.level", "zh-HK", "层级_hk", "层级（1=一级菜单，2=二级菜单，以此类推）"),

            // entity.menu.path
            new TranslationSeedItem("entity.menu.path", "en-US", "菜单路径_us", "菜单路径（如：/100/1000/1001/，用于快速查询子菜单）"),
            // entity.menu.path
            new TranslationSeedItem("entity.menu.path", "ja-JP", "菜单路径_jp", "菜单路径（如：/100/1000/1001/，用于快速查询子菜单）"),
            // entity.menu.path
            new TranslationSeedItem("entity.menu.path", "zh-CN", "菜单路径", "菜单路径（如：/100/1000/1001/，用于快速查询子菜单）"),
            // entity.menu.path
            new TranslationSeedItem("entity.menu.path", "zh-HK", "菜单路径_hk", "菜单路径（如：/100/1000/1001/，用于快速查询子菜单）"),

            // entity.menu.isleaf
            new TranslationSeedItem("entity.menu.isleaf", "en-US", "是否叶子节点_us", "是否叶子节点（字典 sys_yes_no；0=否，1=是）"),
            // entity.menu.isleaf
            new TranslationSeedItem("entity.menu.isleaf", "ja-JP", "是否叶子节点_jp", "是否叶子节点（字典 sys_yes_no；0=否，1=是）"),
            // entity.menu.isleaf
            new TranslationSeedItem("entity.menu.isleaf", "zh-CN", "是否叶子节点", "是否叶子节点（字典 sys_yes_no；0=否，1=是）"),
            // entity.menu.isleaf
            new TranslationSeedItem("entity.menu.isleaf", "zh-HK", "是否叶子节点_hk", "是否叶子节点（字典 sys_yes_no；0=否，1=是）"),

            // entity.menu.type
            new TranslationSeedItem("entity.menu.type", "en-US", "菜单类型_us", "菜单类型（字典 sys_menu_type；0=目录，1=菜单，2=按钮）"),
            // entity.menu.type
            new TranslationSeedItem("entity.menu.type", "ja-JP", "菜单类型_jp", "菜单类型（字典 sys_menu_type；0=目录，1=菜单，2=按钮）"),
            // entity.menu.type
            new TranslationSeedItem("entity.menu.type", "zh-CN", "菜单类型", "菜单类型（字典 sys_menu_type；0=目录，1=菜单，2=按钮）"),
            // entity.menu.type
            new TranslationSeedItem("entity.menu.type", "zh-HK", "菜单类型_hk", "菜单类型（字典 sys_menu_type；0=目录，1=菜单，2=按钮）"),

            // entity.menu.permission
            new TranslationSeedItem("entity.menu.permission", "en-US", "权限标识_us", "权限标识（格式：module:resource:action）"),
            // entity.menu.permission
            new TranslationSeedItem("entity.menu.permission", "ja-JP", "权限标识_jp", "权限标识（格式：module:resource:action）"),
            // entity.menu.permission
            new TranslationSeedItem("entity.menu.permission", "zh-CN", "权限标识", "权限标识（格式：module:resource:action）"),
            // entity.menu.permission
            new TranslationSeedItem("entity.menu.permission", "zh-HK", "权限标识_hk", "权限标识（格式：module:resource:action）"),

            // entity.menu.routepath
            new TranslationSeedItem("entity.menu.routepath", "en-US", "路由地址_us", "路由地址（前端路由）"),
            // entity.menu.routepath
            new TranslationSeedItem("entity.menu.routepath", "ja-JP", "路由地址_jp", "路由地址（前端路由）"),
            // entity.menu.routepath
            new TranslationSeedItem("entity.menu.routepath", "zh-CN", "路由地址", "路由地址（前端路由）"),
            // entity.menu.routepath
            new TranslationSeedItem("entity.menu.routepath", "zh-HK", "路由地址_hk", "路由地址（前端路由）"),

            // entity.menu.component
            new TranslationSeedItem("entity.menu.component", "en-US", "组件路径_us", "组件路径（前端组件路径）"),
            // entity.menu.component
            new TranslationSeedItem("entity.menu.component", "ja-JP", "组件路径_jp", "组件路径（前端组件路径）"),
            // entity.menu.component
            new TranslationSeedItem("entity.menu.component", "zh-CN", "组件路径", "组件路径（前端组件路径）"),
            // entity.menu.component
            new TranslationSeedItem("entity.menu.component", "zh-HK", "组件路径_hk", "组件路径（前端组件路径）"),

            // entity.menu.isexternal
            new TranslationSeedItem("entity.menu.isexternal", "en-US", "是否外部链接_us", "是否外部链接（字典 sys_yes_no）"),
            // entity.menu.isexternal
            new TranslationSeedItem("entity.menu.isexternal", "ja-JP", "是否外部链接_jp", "是否外部链接（字典 sys_yes_no）"),
            // entity.menu.isexternal
            new TranslationSeedItem("entity.menu.isexternal", "zh-CN", "是否外部链接", "是否外部链接（字典 sys_yes_no）"),
            // entity.menu.isexternal
            new TranslationSeedItem("entity.menu.isexternal", "zh-HK", "是否外部链接_hk", "是否外部链接（字典 sys_yes_no）"),

            // entity.menu.linkurl
            new TranslationSeedItem("entity.menu.linkurl", "en-US", "外部链接地址_us", "外部链接地址"),
            // entity.menu.linkurl
            new TranslationSeedItem("entity.menu.linkurl", "ja-JP", "外部链接地址_jp", "外部链接地址"),
            // entity.menu.linkurl
            new TranslationSeedItem("entity.menu.linkurl", "zh-CN", "外部链接地址", "外部链接地址"),
            // entity.menu.linkurl
            new TranslationSeedItem("entity.menu.linkurl", "zh-HK", "外部链接地址_hk", "外部链接地址"),

            // entity.menu.iscached
            new TranslationSeedItem("entity.menu.iscached", "en-US", "是否缓存_us", "是否缓存（字典 sys_yes_no；前端 keep-alive）"),
            // entity.menu.iscached
            new TranslationSeedItem("entity.menu.iscached", "ja-JP", "是否缓存_jp", "是否缓存（字典 sys_yes_no；前端 keep-alive）"),
            // entity.menu.iscached
            new TranslationSeedItem("entity.menu.iscached", "zh-CN", "是否缓存", "是否缓存（字典 sys_yes_no；前端 keep-alive）"),
            // entity.menu.iscached
            new TranslationSeedItem("entity.menu.iscached", "zh-HK", "是否缓存_hk", "是否缓存（字典 sys_yes_no；前端 keep-alive）"),

            // entity.menu.isvisible
            new TranslationSeedItem("entity.menu.isvisible", "en-US", "是否显示_us", "是否显示（字典 sys_yes_no；0=隐藏，1=显示）"),
            // entity.menu.isvisible
            new TranslationSeedItem("entity.menu.isvisible", "ja-JP", "是否显示_jp", "是否显示（字典 sys_yes_no；0=隐藏，1=显示）"),
            // entity.menu.isvisible
            new TranslationSeedItem("entity.menu.isvisible", "zh-CN", "是否显示", "是否显示（字典 sys_yes_no；0=隐藏，1=显示）"),
            // entity.menu.isvisible
            new TranslationSeedItem("entity.menu.isvisible", "zh-HK", "是否显示_hk", "是否显示（字典 sys_yes_no；0=隐藏，1=显示）"),

            // entity.menu.isbuiltin
            new TranslationSeedItem("entity.menu.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；种子菜单为内置，不允许删除）"),
            // entity.menu.isbuiltin
            new TranslationSeedItem("entity.menu.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；种子菜单为内置，不允许删除）"),
            // entity.menu.isbuiltin
            new TranslationSeedItem("entity.menu.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；种子菜单为内置，不允许删除）"),
            // entity.menu.isbuiltin
            new TranslationSeedItem("entity.menu.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；种子菜单为内置，不允许删除）"),

            // entity.menu.description
            new TranslationSeedItem("entity.menu.description", "en-US", "菜单描述_us", "菜单描述"),
            // entity.menu.description
            new TranslationSeedItem("entity.menu.description", "ja-JP", "菜单描述_jp", "菜单描述"),
            // entity.menu.description
            new TranslationSeedItem("entity.menu.description", "zh-CN", "菜单描述", "菜单描述"),
            // entity.menu.description
            new TranslationSeedItem("entity.menu.description", "zh-HK", "菜单描述_hk", "菜单描述"),

            // entity.menu.sortorder
            new TranslationSeedItem("entity.menu.sortorder", "en-US", "排序号_us", "排序号（回填）（同级菜单排序）"),
            // entity.menu.sortorder
            new TranslationSeedItem("entity.menu.sortorder", "ja-JP", "排序号_jp", "排序号（回填）（同级菜单排序）"),
            // entity.menu.sortorder
            new TranslationSeedItem("entity.menu.sortorder", "zh-CN", "排序号", "排序号（回填）（同级菜单排序）"),
            // entity.menu.sortorder
            new TranslationSeedItem("entity.menu.sortorder", "zh-HK", "排序号_hk", "排序号（回填）（同级菜单排序）"),

            // entity.menu.status
            new TranslationSeedItem("entity.menu.status", "en-US", "状态_us", "状态（字典 sys_normal_disable）"),
            // entity.menu.status
            new TranslationSeedItem("entity.menu.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable）"),
            // entity.menu.status
            new TranslationSeedItem("entity.menu.status", "zh-CN", "状态", "状态（字典 sys_normal_disable）"),
            // entity.menu.status
            new TranslationSeedItem("entity.menu.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable）"),

            // entity.menu.rolemenus
            new TranslationSeedItem("entity.menu.rolemenus", "en-US", "拥有该菜单权限的角色关联_us", "拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu）"),
            // entity.menu.rolemenus
            new TranslationSeedItem("entity.menu.rolemenus", "ja-JP", "拥有该菜单权限的角色关联_jp", "拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu）"),
            // entity.menu.rolemenus
            new TranslationSeedItem("entity.menu.rolemenus", "zh-CN", "拥有该菜单权限的角色关联", "拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu）"),
            // entity.menu.rolemenus
            new TranslationSeedItem("entity.menu.rolemenus", "zh-HK", "拥有该菜单权限的角色关联_hk", "拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu）"),
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
