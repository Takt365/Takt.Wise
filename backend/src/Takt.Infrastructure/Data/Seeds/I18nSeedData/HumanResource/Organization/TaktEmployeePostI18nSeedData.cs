// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Organization
// 文件名称：TaktEmployeePostI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeePost 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeePost 实体国际化翻译种子（键前缀 entity.employeepost.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeePostI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeePost 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeepost 实体翻译...", tenantCode);

        foreach (var item in GetEmployeePostTranslations())
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

        TaktLogger.Information("TaktEmployeePost 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeePost 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeepost._self / entity.employeepost.{{field}}；ResourceGroup=Organization；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeePostTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeepost._self
            new TranslationSeedItem("entity.employeepost._self", "en-US", "Employee Post Information_us", "实体名称"),
            // entity.employeepost._self
            new TranslationSeedItem("entity.employeepost._self", "ja-JP", "员工-岗位关联信息_jp", "实体名称"),
            // entity.employeepost._self
            new TranslationSeedItem("entity.employeepost._self", "zh-CN", "员工-岗位关联信息", "实体名称"),
            // entity.employeepost._self
            new TranslationSeedItem("entity.employeepost._self", "zh-HK", "员工-岗位关联信息_hk", "实体名称"),

            // entity.employeepost.employeeid
            new TranslationSeedItem("entity.employeepost.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeepost.employeeid
            new TranslationSeedItem("entity.employeepost.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeepost.employeeid
            new TranslationSeedItem("entity.employeepost.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeepost.employeeid
            new TranslationSeedItem("entity.employeepost.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeepost.postid
            new TranslationSeedItem("entity.employeepost.postid", "en-US", "岗位ID_us", "岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeepost.postid
            new TranslationSeedItem("entity.employeepost.postid", "ja-JP", "岗位ID_jp", "岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeepost.postid
            new TranslationSeedItem("entity.employeepost.postid", "zh-CN", "岗位ID", "岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeepost.postid
            new TranslationSeedItem("entity.employeepost.postid", "zh-HK", "岗位ID_hk", "岗位（选项 TaktPosts/options；DictValue=Id）"),

            // entity.employeepost.employee
            new TranslationSeedItem("entity.employeepost.employee", "en-US", "员工_us", "员工（多对一）"),
            // entity.employeepost.employee
            new TranslationSeedItem("entity.employeepost.employee", "ja-JP", "员工_jp", "员工（多对一）"),
            // entity.employeepost.employee
            new TranslationSeedItem("entity.employeepost.employee", "zh-CN", "员工", "员工（多对一）"),
            // entity.employeepost.employee
            new TranslationSeedItem("entity.employeepost.employee", "zh-HK", "员工_hk", "员工（多对一）"),

            // entity.employeepost.post
            new TranslationSeedItem("entity.employeepost.post", "en-US", "岗位_us", "岗位（多对一）"),
            // entity.employeepost.post
            new TranslationSeedItem("entity.employeepost.post", "ja-JP", "岗位_jp", "岗位（多对一）"),
            // entity.employeepost.post
            new TranslationSeedItem("entity.employeepost.post", "zh-CN", "岗位", "岗位（多对一）"),
            // entity.employeepost.post
            new TranslationSeedItem("entity.employeepost.post", "zh-HK", "岗位_hk", "岗位（多对一）"),
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
