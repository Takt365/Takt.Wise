// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Identity
// 文件名称：TaktUserI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktUser 实体字段国际化种子（已对齐前端 locales：src/locales/identity/user）
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
/// TaktUser 实体国际化翻译种子（键前缀 entity.user.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktUserI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktUser 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 user 实体翻译...", tenantCode);

        foreach (var item in GetUserTranslations())
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

        TaktLogger.Information("TaktUser 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktUser 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.user._self / entity.user.{{field}}；ResourceGroup=Identity；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetUserTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.user._self
            new TranslationSeedItem("entity.user._self", "en-US", "User Information_us", "实体名称"),
            // entity.user._self
            new TranslationSeedItem("entity.user._self", "ja-JP", "用户信息_jp", "实体名称"),
            // entity.user._self
            new TranslationSeedItem("entity.user._self", "zh-CN", "用户信息", "实体名称"),
            // entity.user._self
            new TranslationSeedItem("entity.user._self", "zh-HK", "用户信息_hk", "实体名称"),

            // entity.user.name
            new TranslationSeedItem("entity.user.name", "en-US", "用户名_us", "用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）"),
            // entity.user.name
            new TranslationSeedItem("entity.user.name", "ja-JP", "用户名_jp", "用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）"),
            // entity.user.name
            new TranslationSeedItem("entity.user.name", "zh-CN", "用户名", "用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）"),
            // entity.user.name
            new TranslationSeedItem("entity.user.name", "zh-HK", "用户名_hk", "用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）"),

            // entity.user.nickname
            new TranslationSeedItem("entity.user.nickname", "en-US", "昵称_us", "昵称（显示名称，2–20 位）"),
            // entity.user.nickname
            new TranslationSeedItem("entity.user.nickname", "ja-JP", "昵称_jp", "昵称（显示名称，2–20 位）"),
            // entity.user.nickname
            new TranslationSeedItem("entity.user.nickname", "zh-CN", "昵称", "昵称（显示名称，2–20 位）"),
            // entity.user.nickname
            new TranslationSeedItem("entity.user.nickname", "zh-HK", "昵称_hk", "昵称（显示名称，2–20 位）"),

            // entity.user.type
            new TranslationSeedItem("entity.user.type", "en-US", "用户类型_us", "用户类型（字典 sys_user_type）"),
            // entity.user.type
            new TranslationSeedItem("entity.user.type", "ja-JP", "用户类型_jp", "用户类型（字典 sys_user_type）"),
            // entity.user.type
            new TranslationSeedItem("entity.user.type", "zh-CN", "用户类型", "用户类型（字典 sys_user_type）"),
            // entity.user.type
            new TranslationSeedItem("entity.user.type", "zh-HK", "用户类型_hk", "用户类型（字典 sys_user_type）"),

            // entity.user.password
            new TranslationSeedItem("entity.user.password", "en-US", "密码哈希_us", "密码哈希值（bcrypt加密）"),
            // entity.user.password
            new TranslationSeedItem("entity.user.password", "ja-JP", "密码哈希_jp", "密码哈希值（bcrypt加密）"),
            // entity.user.password
            new TranslationSeedItem("entity.user.password", "zh-CN", "密码哈希", "密码哈希值（bcrypt加密）"),
            // entity.user.password
            new TranslationSeedItem("entity.user.password", "zh-HK", "密码哈希_hk", "密码哈希值（bcrypt加密）"),

            // entity.user.employeeid
            new TranslationSeedItem("entity.user.employeeid", "en-US", "员工ID_us", "关联的员工ID（（选项 TaktUsers/options；DictValue=Id,必须关联人事档案）"),
            // entity.user.employeeid
            new TranslationSeedItem("entity.user.employeeid", "ja-JP", "员工ID_jp", "关联的员工ID（（选项 TaktUsers/options；DictValue=Id,必须关联人事档案）"),
            // entity.user.employeeid
            new TranslationSeedItem("entity.user.employeeid", "zh-CN", "员工ID", "关联的员工ID（（选项 TaktUsers/options；DictValue=Id,必须关联人事档案）"),
            // entity.user.employeeid
            new TranslationSeedItem("entity.user.employeeid", "zh-HK", "员工ID_hk", "关联的员工ID（（选项 TaktUsers/options；DictValue=Id,必须关联人事档案）"),

            // entity.user.isbuiltin
            new TranslationSeedItem("entity.user.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）"),
            // entity.user.isbuiltin
            new TranslationSeedItem("entity.user.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）"),
            // entity.user.isbuiltin
            new TranslationSeedItem("entity.user.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）"),
            // entity.user.isbuiltin
            new TranslationSeedItem("entity.user.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）"),

            // entity.user.lastloginat
            new TranslationSeedItem("entity.user.lastloginat", "en-US", "最后登录时间_us", "最后登录时间（登录成功时 RecordUserLastLoginAsync 写入；登出不修改本字段）"),
            // entity.user.lastloginat
            new TranslationSeedItem("entity.user.lastloginat", "ja-JP", "最后登录时间_jp", "最后登录时间（登录成功时 RecordUserLastLoginAsync 写入；登出不修改本字段）"),
            // entity.user.lastloginat
            new TranslationSeedItem("entity.user.lastloginat", "zh-CN", "最后登录时间", "最后登录时间（登录成功时 RecordUserLastLoginAsync 写入；登出不修改本字段）"),
            // entity.user.lastloginat
            new TranslationSeedItem("entity.user.lastloginat", "zh-HK", "最后登录时间_hk", "最后登录时间（登录成功时 RecordUserLastLoginAsync 写入；登出不修改本字段）"),

            // entity.user.lastloginip
            new TranslationSeedItem("entity.user.lastloginip", "en-US", "最后登录IP_us", "最后登录IP"),
            // entity.user.lastloginip
            new TranslationSeedItem("entity.user.lastloginip", "ja-JP", "最后登录IP_jp", "最后登录IP"),
            // entity.user.lastloginip
            new TranslationSeedItem("entity.user.lastloginip", "zh-CN", "最后登录IP", "最后登录IP"),
            // entity.user.lastloginip
            new TranslationSeedItem("entity.user.lastloginip", "zh-HK", "最后登录IP_hk", "最后登录IP"),

            // entity.user.logincount
            new TranslationSeedItem("entity.user.logincount", "en-US", "登录次数_us", "登录次数"),
            // entity.user.logincount
            new TranslationSeedItem("entity.user.logincount", "ja-JP", "登录次数_jp", "登录次数"),
            // entity.user.logincount
            new TranslationSeedItem("entity.user.logincount", "zh-CN", "登录次数", "登录次数"),
            // entity.user.logincount
            new TranslationSeedItem("entity.user.logincount", "zh-HK", "登录次数_hk", "登录次数"),

            // entity.user.passwordexpiredays
            new TranslationSeedItem("entity.user.passwordexpiredays", "en-US", "密码过期天数_us", "密码过期天数（0=永不过期，30=30天后过期）"),
            // entity.user.passwordexpiredays
            new TranslationSeedItem("entity.user.passwordexpiredays", "ja-JP", "密码过期天数_jp", "密码过期天数（0=永不过期，30=30天后过期）"),
            // entity.user.passwordexpiredays
            new TranslationSeedItem("entity.user.passwordexpiredays", "zh-CN", "密码过期天数", "密码过期天数（0=永不过期，30=30天后过期）"),
            // entity.user.passwordexpiredays
            new TranslationSeedItem("entity.user.passwordexpiredays", "zh-HK", "密码过期天数_hk", "密码过期天数（0=永不过期，30=30天后过期）"),

            // entity.user.loginfailcount
            new TranslationSeedItem("entity.user.loginfailcount", "en-US", "失败登录次数_us", "失败登录次数"),
            // entity.user.loginfailcount
            new TranslationSeedItem("entity.user.loginfailcount", "ja-JP", "失败登录次数_jp", "失败登录次数"),
            // entity.user.loginfailcount
            new TranslationSeedItem("entity.user.loginfailcount", "zh-CN", "失败登录次数", "失败登录次数"),
            // entity.user.loginfailcount
            new TranslationSeedItem("entity.user.loginfailcount", "zh-HK", "失败登录次数_hk", "失败登录次数"),

            // entity.user.lockeduntil
            new TranslationSeedItem("entity.user.lockeduntil", "en-US", "锁定时间_us", "锁定时间（登录失败过多时锁定）"),
            // entity.user.lockeduntil
            new TranslationSeedItem("entity.user.lockeduntil", "ja-JP", "锁定时间_jp", "锁定时间（登录失败过多时锁定）"),
            // entity.user.lockeduntil
            new TranslationSeedItem("entity.user.lockeduntil", "zh-CN", "锁定时间", "锁定时间（登录失败过多时锁定）"),
            // entity.user.lockeduntil
            new TranslationSeedItem("entity.user.lockeduntil", "zh-HK", "锁定时间_hk", "锁定时间（登录失败过多时锁定）"),

            // entity.user.status
            new TranslationSeedItem("entity.user.status", "en-US", "状态_us", "状态（字典 sys_normal_disable）"),
            // entity.user.status
            new TranslationSeedItem("entity.user.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable）"),
            // entity.user.status
            new TranslationSeedItem("entity.user.status", "zh-CN", "状态", "状态（字典 sys_normal_disable）"),
            // entity.user.status
            new TranslationSeedItem("entity.user.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable）"),

            // entity.user.roles
            new TranslationSeedItem("entity.user.roles", "en-US", "用户角色关联_us", "用户角色关联（RBAC，表 takt_identity_user_role）"),
            // entity.user.roles
            new TranslationSeedItem("entity.user.roles", "ja-JP", "用户角色关联_jp", "用户角色关联（RBAC，表 takt_identity_user_role）"),
            // entity.user.roles
            new TranslationSeedItem("entity.user.roles", "zh-CN", "用户角色关联", "用户角色关联（RBAC，表 takt_identity_user_role）"),
            // entity.user.roles
            new TranslationSeedItem("entity.user.roles", "zh-HK", "用户角色关联_hk", "用户角色关联（RBAC，表 takt_identity_user_role）"),

            // entity.user.tenants
            new TranslationSeedItem("entity.user.tenants", "en-US", "用户可访问租户关联_us", "用户可访问租户关联（RBAC，表 takt_identity_user_tenant）"),
            // entity.user.tenants
            new TranslationSeedItem("entity.user.tenants", "ja-JP", "用户可访问租户关联_jp", "用户可访问租户关联（RBAC，表 takt_identity_user_tenant）"),
            // entity.user.tenants
            new TranslationSeedItem("entity.user.tenants", "zh-CN", "用户可访问租户关联", "用户可访问租户关联（RBAC，表 takt_identity_user_tenant）"),
            // entity.user.tenants
            new TranslationSeedItem("entity.user.tenants", "zh-HK", "用户可访问租户关联_hk", "用户可访问租户关联（RBAC，表 takt_identity_user_tenant）"),

            // entity.user.companies
            new TranslationSeedItem("entity.user.companies", "en-US", "用户可访问公司关联_us", "用户可访问公司关联（RBAC，表 takt_identity_user_company）"),
            // entity.user.companies
            new TranslationSeedItem("entity.user.companies", "ja-JP", "用户可访问公司关联_jp", "用户可访问公司关联（RBAC，表 takt_identity_user_company）"),
            // entity.user.companies
            new TranslationSeedItem("entity.user.companies", "zh-CN", "用户可访问公司关联", "用户可访问公司关联（RBAC，表 takt_identity_user_company）"),
            // entity.user.companies
            new TranslationSeedItem("entity.user.companies", "zh-HK", "用户可访问公司关联_hk", "用户可访问公司关联（RBAC，表 takt_identity_user_company）"),
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
