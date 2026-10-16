// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktLeaveI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktLeave 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance;

/// <summary>
/// TaktLeave 实体国际化翻译种子（键前缀 entity.leave.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktLeaveI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktLeave 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 leave 实体翻译...", tenantCode);

        foreach (var item in GetLeaveTranslations())
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

        TaktLogger.Information("TaktLeave 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktLeave 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.leave._self / entity.leave.{{field}}；ResourceGroup=Attendance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetLeaveTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.leave._self
            new TranslationSeedItem("entity.leave._self", "en-US", "Leave Information_us", "实体名称"),
            // entity.leave._self
            new TranslationSeedItem("entity.leave._self", "ja-JP", "请假信息_jp", "实体名称"),
            // entity.leave._self
            new TranslationSeedItem("entity.leave._self", "zh-CN", "请假信息", "实体名称"),
            // entity.leave._self
            new TranslationSeedItem("entity.leave._self", "zh-HK", "请假信息_hk", "实体名称"),

            // entity.leave.employeeid
            new TranslationSeedItem("entity.leave.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.leave.employeeid
            new TranslationSeedItem("entity.leave.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.leave.employeeid
            new TranslationSeedItem("entity.leave.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.leave.employeeid
            new TranslationSeedItem("entity.leave.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.leave.employeename
            new TranslationSeedItem("entity.leave.employeename", "en-US", "员工姓名_us", "员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.leave.employeename
            new TranslationSeedItem("entity.leave.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.leave.employeename
            new TranslationSeedItem("entity.leave.employeename", "zh-CN", "员工姓名", "员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.leave.employeename
            new TranslationSeedItem("entity.leave.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.leave.deptid
            new TranslationSeedItem("entity.leave.deptid", "en-US", "部门ID_us", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.leave.deptid
            new TranslationSeedItem("entity.leave.deptid", "ja-JP", "部门ID_jp", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.leave.deptid
            new TranslationSeedItem("entity.leave.deptid", "zh-CN", "部门ID", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.leave.deptid
            new TranslationSeedItem("entity.leave.deptid", "zh-HK", "部门ID_hk", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.leave.deptname
            new TranslationSeedItem("entity.leave.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.leave.deptname
            new TranslationSeedItem("entity.leave.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.leave.deptname
            new TranslationSeedItem("entity.leave.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.leave.deptname
            new TranslationSeedItem("entity.leave.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),

            // entity.leave.type
            new TranslationSeedItem("entity.leave.type", "en-US", "请假类型_us", "请假类型（字典 sys_leave_type；列存 DictValue）"),
            // entity.leave.type
            new TranslationSeedItem("entity.leave.type", "ja-JP", "请假类型_jp", "请假类型（字典 sys_leave_type；列存 DictValue）"),
            // entity.leave.type
            new TranslationSeedItem("entity.leave.type", "zh-CN", "请假类型", "请假类型（字典 sys_leave_type；列存 DictValue）"),
            // entity.leave.type
            new TranslationSeedItem("entity.leave.type", "zh-HK", "请假类型_hk", "请假类型（字典 sys_leave_type；列存 DictValue）"),

            // entity.leave.startdate
            new TranslationSeedItem("entity.leave.startdate", "en-US", "开始日期_us", "开始日期"),
            // entity.leave.startdate
            new TranslationSeedItem("entity.leave.startdate", "ja-JP", "开始日期_jp", "开始日期"),
            // entity.leave.startdate
            new TranslationSeedItem("entity.leave.startdate", "zh-CN", "开始日期", "开始日期"),
            // entity.leave.startdate
            new TranslationSeedItem("entity.leave.startdate", "zh-HK", "开始日期_hk", "开始日期"),

            // entity.leave.enddate
            new TranslationSeedItem("entity.leave.enddate", "en-US", "结束日期_us", "结束日期"),
            // entity.leave.enddate
            new TranslationSeedItem("entity.leave.enddate", "ja-JP", "结束日期_jp", "结束日期"),
            // entity.leave.enddate
            new TranslationSeedItem("entity.leave.enddate", "zh-CN", "结束日期", "结束日期"),
            // entity.leave.enddate
            new TranslationSeedItem("entity.leave.enddate", "zh-HK", "结束日期_hk", "结束日期"),

            // entity.leave.reason
            new TranslationSeedItem("entity.leave.reason", "en-US", "请假事由_us", "请假事由"),
            // entity.leave.reason
            new TranslationSeedItem("entity.leave.reason", "ja-JP", "请假事由_jp", "请假事由"),
            // entity.leave.reason
            new TranslationSeedItem("entity.leave.reason", "zh-CN", "请假事由", "请假事由"),
            // entity.leave.reason
            new TranslationSeedItem("entity.leave.reason", "zh-HK", "请假事由_hk", "请假事由"),

            // entity.leave.attachments
            new TranslationSeedItem("entity.leave.attachments", "en-US", "证明附件JSON_us", "证明附件（JSON 列表，由 TaktFile 统一上传）"),
            // entity.leave.attachments
            new TranslationSeedItem("entity.leave.attachments", "ja-JP", "证明附件JSON_jp", "证明附件（JSON 列表，由 TaktFile 统一上传）"),
            // entity.leave.attachments
            new TranslationSeedItem("entity.leave.attachments", "zh-CN", "证明附件JSON", "证明附件（JSON 列表，由 TaktFile 统一上传）"),
            // entity.leave.attachments
            new TranslationSeedItem("entity.leave.attachments", "zh-HK", "证明附件JSON_hk", "证明附件（JSON 列表，由 TaktFile 统一上传）"),

            // entity.leave.handlingby
            new TranslationSeedItem("entity.leave.handlingby", "en-US", "经办人ID_us", "经办人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.leave.handlingby
            new TranslationSeedItem("entity.leave.handlingby", "ja-JP", "经办人ID_jp", "经办人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.leave.handlingby
            new TranslationSeedItem("entity.leave.handlingby", "zh-CN", "经办人ID", "经办人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.leave.handlingby
            new TranslationSeedItem("entity.leave.handlingby", "zh-HK", "经办人ID_hk", "经办人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.leave.handlingbyname
            new TranslationSeedItem("entity.leave.handlingbyname", "en-US", "经办人名称_us", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.leave.handlingbyname
            new TranslationSeedItem("entity.leave.handlingbyname", "ja-JP", "经办人名称_jp", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.leave.handlingbyname
            new TranslationSeedItem("entity.leave.handlingbyname", "zh-CN", "经办人名称", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.leave.handlingbyname
            new TranslationSeedItem("entity.leave.handlingbyname", "zh-HK", "经办人名称_hk", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.leave.handlingat
            new TranslationSeedItem("entity.leave.handlingat", "en-US", "经办时间_us", "经办时间"),
            // entity.leave.handlingat
            new TranslationSeedItem("entity.leave.handlingat", "ja-JP", "经办时间_jp", "经办时间"),
            // entity.leave.handlingat
            new TranslationSeedItem("entity.leave.handlingat", "zh-CN", "经办时间", "经办时间"),
            // entity.leave.handlingat
            new TranslationSeedItem("entity.leave.handlingat", "zh-HK", "经办时间_hk", "经办时间"),

            // entity.leave.handlingcomment
            new TranslationSeedItem("entity.leave.handlingcomment", "en-US", "经办备注_us", "经办备注"),
            // entity.leave.handlingcomment
            new TranslationSeedItem("entity.leave.handlingcomment", "ja-JP", "经办备注_jp", "经办备注"),
            // entity.leave.handlingcomment
            new TranslationSeedItem("entity.leave.handlingcomment", "zh-CN", "经办备注", "经办备注"),
            // entity.leave.handlingcomment
            new TranslationSeedItem("entity.leave.handlingcomment", "zh-HK", "经办备注_hk", "经办备注"),

            // entity.leave.status
            new TranslationSeedItem("entity.leave.status", "en-US", "请假状态_us", "请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
            // entity.leave.status
            new TranslationSeedItem("entity.leave.status", "ja-JP", "请假状态_jp", "请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
            // entity.leave.status
            new TranslationSeedItem("entity.leave.status", "zh-CN", "请假状态", "请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
            // entity.leave.status
            new TranslationSeedItem("entity.leave.status", "zh-HK", "请假状态_hk", "请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
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
        translation.ResourceGroup = "Attendance";
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
