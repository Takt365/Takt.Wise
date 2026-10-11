// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization
// 文件名称：TaktRoleDeptI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktRoleDept 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization;

/// <summary>
/// TaktRoleDept 实体国际化翻译种子（键前缀 entity.roledept.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktRoleDeptI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktRoleDept 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 roledept 实体翻译...", tenantCode);

        foreach (var item in GetRoleDeptTranslations())
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

        TaktLogger.Information("TaktRoleDept 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktRoleDept 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.roledept._self / entity.roledept.{{field}}；ResourceGroup=Organization；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetRoleDeptTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.roledept._self
            new TranslationSeedItem("entity.roledept._self", "en-US", "Role Dept Information_us", "实体名称"),
            // entity.roledept._self
            new TranslationSeedItem("entity.roledept._self", "ja-JP", "角色-部门关联信息_jp", "实体名称"),
            // entity.roledept._self
            new TranslationSeedItem("entity.roledept._self", "zh-CN", "角色-部门关联信息", "实体名称"),
            // entity.roledept._self
            new TranslationSeedItem("entity.roledept._self", "zh-HK", "角色-部门关联信息_hk", "实体名称"),

            // entity.roledept.roleid
            new TranslationSeedItem("entity.roledept.roleid", "en-US", "角色ID_us", "角色（选项 TaktRoles/options；DictValue=Id）"),
            // entity.roledept.roleid
            new TranslationSeedItem("entity.roledept.roleid", "ja-JP", "角色ID_jp", "角色（选项 TaktRoles/options；DictValue=Id）"),
            // entity.roledept.roleid
            new TranslationSeedItem("entity.roledept.roleid", "zh-CN", "角色ID", "角色（选项 TaktRoles/options；DictValue=Id）"),
            // entity.roledept.roleid
            new TranslationSeedItem("entity.roledept.roleid", "zh-HK", "角色ID_hk", "角色（选项 TaktRoles/options；DictValue=Id）"),

            // entity.roledept.deptid
            new TranslationSeedItem("entity.roledept.deptid", "en-US", "部门ID_us", "部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.roledept.deptid
            new TranslationSeedItem("entity.roledept.deptid", "ja-JP", "部门ID_jp", "部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.roledept.deptid
            new TranslationSeedItem("entity.roledept.deptid", "zh-CN", "部门ID", "部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.roledept.deptid
            new TranslationSeedItem("entity.roledept.deptid", "zh-HK", "部门ID_hk", "部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),

            // entity.roledept.role
            new TranslationSeedItem("entity.roledept.role", "en-US", "角色_us", "角色（多对一）"),
            // entity.roledept.role
            new TranslationSeedItem("entity.roledept.role", "ja-JP", "角色_jp", "角色（多对一）"),
            // entity.roledept.role
            new TranslationSeedItem("entity.roledept.role", "zh-CN", "角色", "角色（多对一）"),
            // entity.roledept.role
            new TranslationSeedItem("entity.roledept.role", "zh-HK", "角色_hk", "角色（多对一）"),

            // entity.roledept.dept
            new TranslationSeedItem("entity.roledept.dept", "en-US", "部门_us", "部门（多对一）"),
            // entity.roledept.dept
            new TranslationSeedItem("entity.roledept.dept", "ja-JP", "部门_jp", "部门（多对一）"),
            // entity.roledept.dept
            new TranslationSeedItem("entity.roledept.dept", "zh-CN", "部门", "部门（多对一）"),
            // entity.roledept.dept
            new TranslationSeedItem("entity.roledept.dept", "zh-HK", "部门_hk", "部门（多对一）"),
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
        translation.ResourceGroup = "Organization";
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
