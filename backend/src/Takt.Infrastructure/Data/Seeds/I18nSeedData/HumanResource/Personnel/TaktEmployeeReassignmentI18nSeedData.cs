// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeReassignmentI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeReassignment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel;

/// <summary>
/// TaktEmployeeReassignment 实体国际化翻译种子（键前缀 entity.employeereassignment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeReassignmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeReassignment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeereassignment 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeReassignmentTranslations())
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

        TaktLogger.Information("TaktEmployeeReassignment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeReassignment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeereassignment._self / entity.employeereassignment.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeReassignmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeereassignment._self
            new TranslationSeedItem("entity.employeereassignment._self", "en-US", "Employee Reassignment Information_us", "实体名称"),
            // entity.employeereassignment._self
            new TranslationSeedItem("entity.employeereassignment._self", "ja-JP", "员工调动记录信息_jp", "实体名称"),
            // entity.employeereassignment._self
            new TranslationSeedItem("entity.employeereassignment._self", "zh-CN", "员工调动记录信息", "实体名称"),
            // entity.employeereassignment._self
            new TranslationSeedItem("entity.employeereassignment._self", "zh-HK", "员工调动记录信息_hk", "实体名称"),

            // entity.employeereassignment.employeeid
            new TranslationSeedItem("entity.employeereassignment.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeereassignment.employeeid
            new TranslationSeedItem("entity.employeereassignment.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeereassignment.employeeid
            new TranslationSeedItem("entity.employeereassignment.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeereassignment.employeeid
            new TranslationSeedItem("entity.employeereassignment.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeereassignment.employeecode
            new TranslationSeedItem("entity.employeereassignment.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeereassignment.employeecode
            new TranslationSeedItem("entity.employeereassignment.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeereassignment.employeecode
            new TranslationSeedItem("entity.employeereassignment.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeereassignment.employeecode
            new TranslationSeedItem("entity.employeereassignment.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeereassignment.employeename
            new TranslationSeedItem("entity.employeereassignment.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeereassignment.employeename
            new TranslationSeedItem("entity.employeereassignment.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeereassignment.employeename
            new TranslationSeedItem("entity.employeereassignment.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeereassignment.employeename
            new TranslationSeedItem("entity.employeereassignment.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeereassignment.reassignmenttype
            new TranslationSeedItem("entity.employeereassignment.reassignmenttype", "en-US", "调动类型_us", "调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）"),
            // entity.employeereassignment.reassignmenttype
            new TranslationSeedItem("entity.employeereassignment.reassignmenttype", "ja-JP", "调动类型_jp", "调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）"),
            // entity.employeereassignment.reassignmenttype
            new TranslationSeedItem("entity.employeereassignment.reassignmenttype", "zh-CN", "调动类型", "调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）"),
            // entity.employeereassignment.reassignmenttype
            new TranslationSeedItem("entity.employeereassignment.reassignmenttype", "zh-HK", "调动类型_hk", "调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）"),

            // entity.employeereassignment.fromdeptid
            new TranslationSeedItem("entity.employeereassignment.fromdeptid", "en-US", "调出部门ID_us", "调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.employeereassignment.fromdeptid
            new TranslationSeedItem("entity.employeereassignment.fromdeptid", "ja-JP", "调出部门ID_jp", "调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.employeereassignment.fromdeptid
            new TranslationSeedItem("entity.employeereassignment.fromdeptid", "zh-CN", "调出部门ID", "调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.employeereassignment.fromdeptid
            new TranslationSeedItem("entity.employeereassignment.fromdeptid", "zh-HK", "调出部门ID_hk", "调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),

            // entity.employeereassignment.fromdeptname
            new TranslationSeedItem("entity.employeereassignment.fromdeptname", "en-US", "调出部门名称_us", "调出部门名称"),
            // entity.employeereassignment.fromdeptname
            new TranslationSeedItem("entity.employeereassignment.fromdeptname", "ja-JP", "调出部门名称_jp", "调出部门名称"),
            // entity.employeereassignment.fromdeptname
            new TranslationSeedItem("entity.employeereassignment.fromdeptname", "zh-CN", "调出部门名称", "调出部门名称"),
            // entity.employeereassignment.fromdeptname
            new TranslationSeedItem("entity.employeereassignment.fromdeptname", "zh-HK", "调出部门名称_hk", "调出部门名称"),

            // entity.employeereassignment.frompostid
            new TranslationSeedItem("entity.employeereassignment.frompostid", "en-US", "调出岗位ID_us", "调出岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeereassignment.frompostid
            new TranslationSeedItem("entity.employeereassignment.frompostid", "ja-JP", "调出岗位ID_jp", "调出岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeereassignment.frompostid
            new TranslationSeedItem("entity.employeereassignment.frompostid", "zh-CN", "调出岗位ID", "调出岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeereassignment.frompostid
            new TranslationSeedItem("entity.employeereassignment.frompostid", "zh-HK", "调出岗位ID_hk", "调出岗位（选项 TaktPosts/options；DictValue=Id）"),

            // entity.employeereassignment.frompostname
            new TranslationSeedItem("entity.employeereassignment.frompostname", "en-US", "调出岗位名称_us", "调出岗位名称"),
            // entity.employeereassignment.frompostname
            new TranslationSeedItem("entity.employeereassignment.frompostname", "ja-JP", "调出岗位名称_jp", "调出岗位名称"),
            // entity.employeereassignment.frompostname
            new TranslationSeedItem("entity.employeereassignment.frompostname", "zh-CN", "调出岗位名称", "调出岗位名称"),
            // entity.employeereassignment.frompostname
            new TranslationSeedItem("entity.employeereassignment.frompostname", "zh-HK", "调出岗位名称_hk", "调出岗位名称"),

            // entity.employeereassignment.todeptid
            new TranslationSeedItem("entity.employeereassignment.todeptid", "en-US", "调入部门ID_us", "调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.employeereassignment.todeptid
            new TranslationSeedItem("entity.employeereassignment.todeptid", "ja-JP", "调入部门ID_jp", "调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.employeereassignment.todeptid
            new TranslationSeedItem("entity.employeereassignment.todeptid", "zh-CN", "调入部门ID", "调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),
            // entity.employeereassignment.todeptid
            new TranslationSeedItem("entity.employeereassignment.todeptid", "zh-HK", "调入部门ID_hk", "调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）"),

            // entity.employeereassignment.todeptname
            new TranslationSeedItem("entity.employeereassignment.todeptname", "en-US", "调入部门名称_us", "调入部门名称"),
            // entity.employeereassignment.todeptname
            new TranslationSeedItem("entity.employeereassignment.todeptname", "ja-JP", "调入部门名称_jp", "调入部门名称"),
            // entity.employeereassignment.todeptname
            new TranslationSeedItem("entity.employeereassignment.todeptname", "zh-CN", "调入部门名称", "调入部门名称"),
            // entity.employeereassignment.todeptname
            new TranslationSeedItem("entity.employeereassignment.todeptname", "zh-HK", "调入部门名称_hk", "调入部门名称"),

            // entity.employeereassignment.topostid
            new TranslationSeedItem("entity.employeereassignment.topostid", "en-US", "调入岗位ID_us", "调入岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeereassignment.topostid
            new TranslationSeedItem("entity.employeereassignment.topostid", "ja-JP", "调入岗位ID_jp", "调入岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeereassignment.topostid
            new TranslationSeedItem("entity.employeereassignment.topostid", "zh-CN", "调入岗位ID", "调入岗位（选项 TaktPosts/options；DictValue=Id）"),
            // entity.employeereassignment.topostid
            new TranslationSeedItem("entity.employeereassignment.topostid", "zh-HK", "调入岗位ID_hk", "调入岗位（选项 TaktPosts/options；DictValue=Id）"),

            // entity.employeereassignment.topostname
            new TranslationSeedItem("entity.employeereassignment.topostname", "en-US", "调入岗位名称_us", "调入岗位名称"),
            // entity.employeereassignment.topostname
            new TranslationSeedItem("entity.employeereassignment.topostname", "ja-JP", "调入岗位名称_jp", "调入岗位名称"),
            // entity.employeereassignment.topostname
            new TranslationSeedItem("entity.employeereassignment.topostname", "zh-CN", "调入岗位名称", "调入岗位名称"),
            // entity.employeereassignment.topostname
            new TranslationSeedItem("entity.employeereassignment.topostname", "zh-HK", "调入岗位名称_hk", "调入岗位名称"),

            // entity.employeereassignment.effectivedate
            new TranslationSeedItem("entity.employeereassignment.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.employeereassignment.effectivedate
            new TranslationSeedItem("entity.employeereassignment.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.employeereassignment.effectivedate
            new TranslationSeedItem("entity.employeereassignment.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.employeereassignment.effectivedate
            new TranslationSeedItem("entity.employeereassignment.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.employeereassignment.reason
            new TranslationSeedItem("entity.employeereassignment.reason", "en-US", "调动原因_us", "调动原因"),
            // entity.employeereassignment.reason
            new TranslationSeedItem("entity.employeereassignment.reason", "ja-JP", "调动原因_jp", "调动原因"),
            // entity.employeereassignment.reason
            new TranslationSeedItem("entity.employeereassignment.reason", "zh-CN", "调动原因", "调动原因"),
            // entity.employeereassignment.reason
            new TranslationSeedItem("entity.employeereassignment.reason", "zh-HK", "调动原因_hk", "调动原因"),

            // entity.employeereassignment.employee
            new TranslationSeedItem("entity.employeereassignment.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeereassignment.employee
            new TranslationSeedItem("entity.employeereassignment.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeereassignment.employee
            new TranslationSeedItem("entity.employeereassignment.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeereassignment.employee
            new TranslationSeedItem("entity.employeereassignment.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
        translation.ResourceGroup = "Personnel";
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
