// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktShiftScheduleI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktShiftSchedule 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktShiftSchedule 实体国际化翻译种子（键前缀 entity.shiftschedule.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktShiftScheduleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktShiftSchedule 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 shiftschedule 实体翻译...", tenantCode);

        foreach (var item in GetShiftScheduleTranslations())
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

        TaktLogger.Information("TaktShiftSchedule 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktShiftSchedule 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.shiftschedule._self / entity.shiftschedule.{{field}}；ResourceGroup=Attendance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetShiftScheduleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "en-US", "Shift Schedule Information_us", "实体名称"),
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "ja-JP", "排班计划信息_jp", "实体名称"),
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "zh-CN", "排班计划信息", "实体名称"),
            // entity.shiftschedule._self
            new TranslationSeedItem("entity.shiftschedule._self", "zh-HK", "排班计划信息_hk", "实体名称"),

            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "en-US", "排班类别_us", "排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）"),
            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "ja-JP", "排班类别_jp", "排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）"),
            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "zh-CN", "排班类别", "排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）"),
            // entity.shiftschedule.scheduletype
            new TranslationSeedItem("entity.shiftschedule.scheduletype", "zh-HK", "排班类别_hk", "排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）"),

            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "en-US", "部门ID_us", "部门（选项 TaktDepts/tree-options；DictValue=Id；ScheduleType=0 时必填）"),
            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "ja-JP", "部门ID_jp", "部门（选项 TaktDepts/tree-options；DictValue=Id；ScheduleType=0 时必填）"),
            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "zh-CN", "部门ID", "部门（选项 TaktDepts/tree-options；DictValue=Id；ScheduleType=0 时必填）"),
            // entity.shiftschedule.deptid
            new TranslationSeedItem("entity.shiftschedule.deptid", "zh-HK", "部门ID_hk", "部门（选项 TaktDepts/tree-options；DictValue=Id；ScheduleType=0 时必填）"),

            // entity.shiftschedule.deptname
            new TranslationSeedItem("entity.shiftschedule.deptname", "en-US", "部门名称_us", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.shiftschedule.deptname
            new TranslationSeedItem("entity.shiftschedule.deptname", "ja-JP", "部门名称_jp", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.shiftschedule.deptname
            new TranslationSeedItem("entity.shiftschedule.deptname", "zh-CN", "部门名称", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),
            // entity.shiftschedule.deptname
            new TranslationSeedItem("entity.shiftschedule.deptname", "zh-HK", "部门名称_hk", "部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）"),

            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id；ScheduleType=1 时必填）"),
            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id；ScheduleType=1 时必填）"),
            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id；ScheduleType=1 时必填）"),
            // entity.shiftschedule.employeeid
            new TranslationSeedItem("entity.shiftschedule.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id；ScheduleType=1 时必填）"),

            // entity.shiftschedule.employeename
            new TranslationSeedItem("entity.shiftschedule.employeename", "en-US", "员工名称_us", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.shiftschedule.employeename
            new TranslationSeedItem("entity.shiftschedule.employeename", "ja-JP", "员工名称_jp", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.shiftschedule.employeename
            new TranslationSeedItem("entity.shiftschedule.employeename", "zh-CN", "员工名称", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.shiftschedule.employeename
            new TranslationSeedItem("entity.shiftschedule.employeename", "zh-HK", "员工名称_hk", "员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "en-US", "排班日期_us", "排班日期"),
            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "ja-JP", "排班日期_jp", "排班日期"),
            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "zh-CN", "排班日期", "排班日期"),
            // entity.shiftschedule.scheduledate
            new TranslationSeedItem("entity.shiftschedule.scheduledate", "zh-HK", "排班日期_hk", "排班日期"),

            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "en-US", "班次ID_us", "班次（选项 TaktWorkShifts/options；DictValue=Id）"),
            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "ja-JP", "班次ID_jp", "班次（选项 TaktWorkShifts/options；DictValue=Id）"),
            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "zh-CN", "班次ID", "班次（选项 TaktWorkShifts/options；DictValue=Id）"),
            // entity.shiftschedule.shiftid
            new TranslationSeedItem("entity.shiftschedule.shiftid", "zh-HK", "班次ID_hk", "班次（选项 TaktWorkShifts/options；DictValue=Id）"),
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
