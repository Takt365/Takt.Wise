// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktOvertimeI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktOvertime 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktOvertime 实体国际化翻译种子（键前缀 entity.overtime.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktOvertimeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktOvertime 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 overtime 实体翻译...", tenantCode);

        foreach (var item in GetOvertimeTranslations())
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

        TaktLogger.Information("TaktOvertime 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktOvertime 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.overtime._self / entity.overtime.{{field}}；ResourceGroup=Attendance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetOvertimeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.overtime._self
            new TranslationSeedItem("entity.overtime._self", "en-US", "Overtime Information_us", "实体名称"),
            // entity.overtime._self
            new TranslationSeedItem("entity.overtime._self", "ja-JP", "加班申请信息_jp", "实体名称"),
            // entity.overtime._self
            new TranslationSeedItem("entity.overtime._self", "zh-CN", "加班申请信息", "实体名称"),
            // entity.overtime._self
            new TranslationSeedItem("entity.overtime._self", "zh-HK", "加班申请信息_hk", "实体名称"),

            // entity.overtime.deptid
            new TranslationSeedItem("entity.overtime.deptid", "en-US", "部门ID_us", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.overtime.deptid
            new TranslationSeedItem("entity.overtime.deptid", "ja-JP", "部门ID_jp", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.overtime.deptid
            new TranslationSeedItem("entity.overtime.deptid", "zh-CN", "部门ID", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),
            // entity.overtime.deptid
            new TranslationSeedItem("entity.overtime.deptid", "zh-HK", "部门ID_hk", "部门（选项 TaktDepts/tree-options；DictValue=Id）"),

            // entity.overtime.deptname
            new TranslationSeedItem("entity.overtime.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.overtime.deptname
            new TranslationSeedItem("entity.overtime.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.overtime.deptname
            new TranslationSeedItem("entity.overtime.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.overtime.deptname
            new TranslationSeedItem("entity.overtime.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),

            // entity.overtime.date
            new TranslationSeedItem("entity.overtime.date", "en-US", "加班日期_us", "加班归属日期"),
            // entity.overtime.date
            new TranslationSeedItem("entity.overtime.date", "ja-JP", "加班日期_jp", "加班归属日期"),
            // entity.overtime.date
            new TranslationSeedItem("entity.overtime.date", "zh-CN", "加班日期", "加班归属日期"),
            // entity.overtime.date
            new TranslationSeedItem("entity.overtime.date", "zh-HK", "加班日期_hk", "加班归属日期"),

            // entity.overtime.plannedstarttime
            new TranslationSeedItem("entity.overtime.plannedstarttime", "en-US", "计划开始时间_us", "计划加班开始时间"),
            // entity.overtime.plannedstarttime
            new TranslationSeedItem("entity.overtime.plannedstarttime", "ja-JP", "计划开始时间_jp", "计划加班开始时间"),
            // entity.overtime.plannedstarttime
            new TranslationSeedItem("entity.overtime.plannedstarttime", "zh-CN", "计划开始时间", "计划加班开始时间"),
            // entity.overtime.plannedstarttime
            new TranslationSeedItem("entity.overtime.plannedstarttime", "zh-HK", "计划开始时间_hk", "计划加班开始时间"),

            // entity.overtime.plannedendtime
            new TranslationSeedItem("entity.overtime.plannedendtime", "en-US", "计划结束时间_us", "计划加班结束时间"),
            // entity.overtime.plannedendtime
            new TranslationSeedItem("entity.overtime.plannedendtime", "ja-JP", "计划结束时间_jp", "计划加班结束时间"),
            // entity.overtime.plannedendtime
            new TranslationSeedItem("entity.overtime.plannedendtime", "zh-CN", "计划结束时间", "计划加班结束时间"),
            // entity.overtime.plannedendtime
            new TranslationSeedItem("entity.overtime.plannedendtime", "zh-HK", "计划结束时间_hk", "计划加班结束时间"),

            // entity.overtime.totalemployees
            new TranslationSeedItem("entity.overtime.totalemployees", "en-US", "加班总人数_us", "加班总人数"),
            // entity.overtime.totalemployees
            new TranslationSeedItem("entity.overtime.totalemployees", "ja-JP", "加班总人数_jp", "加班总人数"),
            // entity.overtime.totalemployees
            new TranslationSeedItem("entity.overtime.totalemployees", "zh-CN", "加班总人数", "加班总人数"),
            // entity.overtime.totalemployees
            new TranslationSeedItem("entity.overtime.totalemployees", "zh-HK", "加班总人数_hk", "加班总人数"),

            // entity.overtime.totalplannedhours
            new TranslationSeedItem("entity.overtime.totalplannedhours", "en-US", "计划总小时数_us", "计划加班总小时数"),
            // entity.overtime.totalplannedhours
            new TranslationSeedItem("entity.overtime.totalplannedhours", "ja-JP", "计划总小时数_jp", "计划加班总小时数"),
            // entity.overtime.totalplannedhours
            new TranslationSeedItem("entity.overtime.totalplannedhours", "zh-CN", "计划总小时数", "计划加班总小时数"),
            // entity.overtime.totalplannedhours
            new TranslationSeedItem("entity.overtime.totalplannedhours", "zh-HK", "计划总小时数_hk", "计划加班总小时数"),

            // entity.overtime.totalactualhours
            new TranslationSeedItem("entity.overtime.totalactualhours", "en-US", "实际总小时数_us", "实际加班总小时数"),
            // entity.overtime.totalactualhours
            new TranslationSeedItem("entity.overtime.totalactualhours", "ja-JP", "实际总小时数_jp", "实际加班总小时数"),
            // entity.overtime.totalactualhours
            new TranslationSeedItem("entity.overtime.totalactualhours", "zh-CN", "实际总小时数", "实际加班总小时数"),
            // entity.overtime.totalactualhours
            new TranslationSeedItem("entity.overtime.totalactualhours", "zh-HK", "实际总小时数_hk", "实际加班总小时数"),

            // entity.overtime.type
            new TranslationSeedItem("entity.overtime.type", "en-US", "加班类型_us", "加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）"),
            // entity.overtime.type
            new TranslationSeedItem("entity.overtime.type", "ja-JP", "加班类型_jp", "加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）"),
            // entity.overtime.type
            new TranslationSeedItem("entity.overtime.type", "zh-CN", "加班类型", "加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）"),
            // entity.overtime.type
            new TranslationSeedItem("entity.overtime.type", "zh-HK", "加班类型_hk", "加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）"),

            // entity.overtime.reason
            new TranslationSeedItem("entity.overtime.reason", "en-US", "加班原因_us", "加班原因"),
            // entity.overtime.reason
            new TranslationSeedItem("entity.overtime.reason", "ja-JP", "加班原因_jp", "加班原因"),
            // entity.overtime.reason
            new TranslationSeedItem("entity.overtime.reason", "zh-CN", "加班原因", "加班原因"),
            // entity.overtime.reason
            new TranslationSeedItem("entity.overtime.reason", "zh-HK", "加班原因_hk", "加班原因"),

            // entity.overtime.handlingby
            new TranslationSeedItem("entity.overtime.handlingby", "en-US", "经办人ID_us", "经办人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.overtime.handlingby
            new TranslationSeedItem("entity.overtime.handlingby", "ja-JP", "经办人ID_jp", "经办人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.overtime.handlingby
            new TranslationSeedItem("entity.overtime.handlingby", "zh-CN", "经办人ID", "经办人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.overtime.handlingby
            new TranslationSeedItem("entity.overtime.handlingby", "zh-HK", "经办人ID_hk", "经办人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.overtime.handlingbyname
            new TranslationSeedItem("entity.overtime.handlingbyname", "en-US", "经办人名称_us", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.overtime.handlingbyname
            new TranslationSeedItem("entity.overtime.handlingbyname", "ja-JP", "经办人名称_jp", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.overtime.handlingbyname
            new TranslationSeedItem("entity.overtime.handlingbyname", "zh-CN", "经办人名称", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.overtime.handlingbyname
            new TranslationSeedItem("entity.overtime.handlingbyname", "zh-HK", "经办人名称_hk", "经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.overtime.handlingat
            new TranslationSeedItem("entity.overtime.handlingat", "en-US", "经办时间_us", "经办时间"),
            // entity.overtime.handlingat
            new TranslationSeedItem("entity.overtime.handlingat", "ja-JP", "经办时间_jp", "经办时间"),
            // entity.overtime.handlingat
            new TranslationSeedItem("entity.overtime.handlingat", "zh-CN", "经办时间", "经办时间"),
            // entity.overtime.handlingat
            new TranslationSeedItem("entity.overtime.handlingat", "zh-HK", "经办时间_hk", "经办时间"),

            // entity.overtime.handlingcomment
            new TranslationSeedItem("entity.overtime.handlingcomment", "en-US", "经办备注_us", "经办备注"),
            // entity.overtime.handlingcomment
            new TranslationSeedItem("entity.overtime.handlingcomment", "ja-JP", "经办备注_jp", "经办备注"),
            // entity.overtime.handlingcomment
            new TranslationSeedItem("entity.overtime.handlingcomment", "zh-CN", "经办备注", "经办备注"),
            // entity.overtime.handlingcomment
            new TranslationSeedItem("entity.overtime.handlingcomment", "zh-HK", "经办备注_hk", "经办备注"),

            // entity.overtime.status
            new TranslationSeedItem("entity.overtime.status", "en-US", "加班状态_us", "加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
            // entity.overtime.status
            new TranslationSeedItem("entity.overtime.status", "ja-JP", "加班状态_jp", "加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
            // entity.overtime.status
            new TranslationSeedItem("entity.overtime.status", "zh-CN", "加班状态", "加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),
            // entity.overtime.status
            new TranslationSeedItem("entity.overtime.status", "zh-HK", "加班状态_hk", "加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）"),

            // entity.overtime.items
            new TranslationSeedItem("entity.overtime.items", "en-US", "加班明细列表_us", "加班明细列表"),
            // entity.overtime.items
            new TranslationSeedItem("entity.overtime.items", "ja-JP", "加班明细列表_jp", "加班明细列表"),
            // entity.overtime.items
            new TranslationSeedItem("entity.overtime.items", "zh-CN", "加班明细列表", "加班明细列表"),
            // entity.overtime.items
            new TranslationSeedItem("entity.overtime.items", "zh-HK", "加班明细列表_hk", "加班明细列表"),
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
