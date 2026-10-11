// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeDelegation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeDelegation 实体国际化翻译种子（键前缀 entity.employeedelegation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeDelegationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeDelegation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeedelegation 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeDelegationTranslations())
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

        TaktLogger.Information("TaktEmployeeDelegation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeDelegation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeedelegation._self / entity.employeedelegation.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeDelegationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeedelegation._self
            new TranslationSeedItem("entity.employeedelegation._self", "en-US", "Employee Delegation Information_us", "实体名称"),
            // entity.employeedelegation._self
            new TranslationSeedItem("entity.employeedelegation._self", "ja-JP", "员工代理关系信息_jp", "实体名称"),
            // entity.employeedelegation._self
            new TranslationSeedItem("entity.employeedelegation._self", "zh-CN", "员工代理关系信息", "实体名称"),
            // entity.employeedelegation._self
            new TranslationSeedItem("entity.employeedelegation._self", "zh-HK", "员工代理关系信息_hk", "实体名称"),

            // entity.employeedelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeedelegation.proxyemployeeid", "en-US", "代理人ID_us", "代理人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeedelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeedelegation.proxyemployeeid", "ja-JP", "代理人ID_jp", "代理人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeedelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeedelegation.proxyemployeeid", "zh-CN", "代理人ID", "代理人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeedelegation.proxyemployeeid
            new TranslationSeedItem("entity.employeedelegation.proxyemployeeid", "zh-HK", "代理人ID_hk", "代理人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeedelegation.proxyemployeecode
            new TranslationSeedItem("entity.employeedelegation.proxyemployeecode", "en-US", "代理人编码_us", "代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeedelegation.proxyemployeecode
            new TranslationSeedItem("entity.employeedelegation.proxyemployeecode", "ja-JP", "代理人编码_jp", "代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeedelegation.proxyemployeecode
            new TranslationSeedItem("entity.employeedelegation.proxyemployeecode", "zh-CN", "代理人编码", "代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeedelegation.proxyemployeecode
            new TranslationSeedItem("entity.employeedelegation.proxyemployeecode", "zh-HK", "代理人编码_hk", "代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeedelegation.proxyemployeename
            new TranslationSeedItem("entity.employeedelegation.proxyemployeename", "en-US", "代理人姓名_us", "代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeedelegation.proxyemployeename
            new TranslationSeedItem("entity.employeedelegation.proxyemployeename", "ja-JP", "代理人姓名_jp", "代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeedelegation.proxyemployeename
            new TranslationSeedItem("entity.employeedelegation.proxyemployeename", "zh-CN", "代理人姓名", "代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeedelegation.proxyemployeename
            new TranslationSeedItem("entity.employeedelegation.proxyemployeename", "zh-HK", "代理人姓名_hk", "代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeedelegation.originalemployeeid
            new TranslationSeedItem("entity.employeedelegation.originalemployeeid", "en-US", "被代理人ID_us", "被代理人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeedelegation.originalemployeeid
            new TranslationSeedItem("entity.employeedelegation.originalemployeeid", "ja-JP", "被代理人ID_jp", "被代理人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeedelegation.originalemployeeid
            new TranslationSeedItem("entity.employeedelegation.originalemployeeid", "zh-CN", "被代理人ID", "被代理人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeedelegation.originalemployeeid
            new TranslationSeedItem("entity.employeedelegation.originalemployeeid", "zh-HK", "被代理人ID_hk", "被代理人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeedelegation.originalemployeecode
            new TranslationSeedItem("entity.employeedelegation.originalemployeecode", "en-US", "被代理人编码_us", "被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeedelegation.originalemployeecode
            new TranslationSeedItem("entity.employeedelegation.originalemployeecode", "ja-JP", "被代理人编码_jp", "被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeedelegation.originalemployeecode
            new TranslationSeedItem("entity.employeedelegation.originalemployeecode", "zh-CN", "被代理人编码", "被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeedelegation.originalemployeecode
            new TranslationSeedItem("entity.employeedelegation.originalemployeecode", "zh-HK", "被代理人编码_hk", "被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeedelegation.originalemployeename
            new TranslationSeedItem("entity.employeedelegation.originalemployeename", "en-US", "被代理人姓名_us", "被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeedelegation.originalemployeename
            new TranslationSeedItem("entity.employeedelegation.originalemployeename", "ja-JP", "被代理人姓名_jp", "被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeedelegation.originalemployeename
            new TranslationSeedItem("entity.employeedelegation.originalemployeename", "zh-CN", "被代理人姓名", "被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeedelegation.originalemployeename
            new TranslationSeedItem("entity.employeedelegation.originalemployeename", "zh-HK", "被代理人姓名_hk", "被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeedelegation.delegationtype
            new TranslationSeedItem("entity.employeedelegation.delegationtype", "en-US", "代理类型_us", "代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）"),
            // entity.employeedelegation.delegationtype
            new TranslationSeedItem("entity.employeedelegation.delegationtype", "ja-JP", "代理类型_jp", "代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）"),
            // entity.employeedelegation.delegationtype
            new TranslationSeedItem("entity.employeedelegation.delegationtype", "zh-CN", "代理类型", "代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）"),
            // entity.employeedelegation.delegationtype
            new TranslationSeedItem("entity.employeedelegation.delegationtype", "zh-HK", "代理类型_hk", "代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）"),

            // entity.employeedelegation.scopetype
            new TranslationSeedItem("entity.employeedelegation.scopetype", "en-US", "代理范围类型_us", "代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）"),
            // entity.employeedelegation.scopetype
            new TranslationSeedItem("entity.employeedelegation.scopetype", "ja-JP", "代理范围类型_jp", "代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）"),
            // entity.employeedelegation.scopetype
            new TranslationSeedItem("entity.employeedelegation.scopetype", "zh-CN", "代理范围类型", "代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）"),
            // entity.employeedelegation.scopetype
            new TranslationSeedItem("entity.employeedelegation.scopetype", "zh-HK", "代理范围类型_hk", "代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）"),

            // entity.employeedelegation.scopeid
            new TranslationSeedItem("entity.employeedelegation.scopeid", "en-US", "代理范围ID_us", "代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）"),
            // entity.employeedelegation.scopeid
            new TranslationSeedItem("entity.employeedelegation.scopeid", "ja-JP", "代理范围ID_jp", "代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）"),
            // entity.employeedelegation.scopeid
            new TranslationSeedItem("entity.employeedelegation.scopeid", "zh-CN", "代理范围ID", "代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）"),
            // entity.employeedelegation.scopeid
            new TranslationSeedItem("entity.employeedelegation.scopeid", "zh-HK", "代理范围ID_hk", "代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）"),

            // entity.employeedelegation.reason
            new TranslationSeedItem("entity.employeedelegation.reason", "en-US", "代理原因_us", "代理原因（如休假、出差、培训、岗位空缺、病假等）"),
            // entity.employeedelegation.reason
            new TranslationSeedItem("entity.employeedelegation.reason", "ja-JP", "代理原因_jp", "代理原因（如休假、出差、培训、岗位空缺、病假等）"),
            // entity.employeedelegation.reason
            new TranslationSeedItem("entity.employeedelegation.reason", "zh-CN", "代理原因", "代理原因（如休假、出差、培训、岗位空缺、病假等）"),
            // entity.employeedelegation.reason
            new TranslationSeedItem("entity.employeedelegation.reason", "zh-HK", "代理原因_hk", "代理原因（如休假、出差、培训、岗位空缺、病假等）"),

            // entity.employeedelegation.startdate
            new TranslationSeedItem("entity.employeedelegation.startdate", "en-US", "代理开始时间_us", "代理开始时间"),
            // entity.employeedelegation.startdate
            new TranslationSeedItem("entity.employeedelegation.startdate", "ja-JP", "代理开始时间_jp", "代理开始时间"),
            // entity.employeedelegation.startdate
            new TranslationSeedItem("entity.employeedelegation.startdate", "zh-CN", "代理开始时间", "代理开始时间"),
            // entity.employeedelegation.startdate
            new TranslationSeedItem("entity.employeedelegation.startdate", "zh-HK", "代理开始时间_hk", "代理开始时间"),

            // entity.employeedelegation.enddate
            new TranslationSeedItem("entity.employeedelegation.enddate", "en-US", "代理结束时间_us", "代理结束时间（null=长期有效，直到手动删除）"),
            // entity.employeedelegation.enddate
            new TranslationSeedItem("entity.employeedelegation.enddate", "ja-JP", "代理结束时间_jp", "代理结束时间（null=长期有效，直到手动删除）"),
            // entity.employeedelegation.enddate
            new TranslationSeedItem("entity.employeedelegation.enddate", "zh-CN", "代理结束时间", "代理结束时间（null=长期有效，直到手动删除）"),
            // entity.employeedelegation.enddate
            new TranslationSeedItem("entity.employeedelegation.enddate", "zh-HK", "代理结束时间_hk", "代理结束时间（null=长期有效，直到手动删除）"),

            // entity.employeedelegation.originalemployee
            new TranslationSeedItem("entity.employeedelegation.originalemployee", "en-US", "被代理人_us", "被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId）"),
            // entity.employeedelegation.originalemployee
            new TranslationSeedItem("entity.employeedelegation.originalemployee", "ja-JP", "被代理人_jp", "被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId）"),
            // entity.employeedelegation.originalemployee
            new TranslationSeedItem("entity.employeedelegation.originalemployee", "zh-CN", "被代理人", "被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId）"),
            // entity.employeedelegation.originalemployee
            new TranslationSeedItem("entity.employeedelegation.originalemployee", "zh-HK", "被代理人_hk", "被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId）"),

            // entity.employeedelegation.proxyemployee
            new TranslationSeedItem("entity.employeedelegation.proxyemployee", "en-US", "代理人_us", "代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId）"),
            // entity.employeedelegation.proxyemployee
            new TranslationSeedItem("entity.employeedelegation.proxyemployee", "ja-JP", "代理人_jp", "代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId）"),
            // entity.employeedelegation.proxyemployee
            new TranslationSeedItem("entity.employeedelegation.proxyemployee", "zh-CN", "代理人", "代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId）"),
            // entity.employeedelegation.proxyemployee
            new TranslationSeedItem("entity.employeedelegation.proxyemployee", "zh-HK", "代理人_hk", "代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId）"),
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
