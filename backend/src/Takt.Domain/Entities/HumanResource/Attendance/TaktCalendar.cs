// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktCalendar.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂日历实体，按公司/工厂维度定义工作日、休息日与调休
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 工厂日历（公司级；按 PlantCode 区分工厂维度）
/// </summary>
[SugarTable("takt_human_resource_attendance_calendar", "工厂日历表")]
[SugarIndex("ix_calendar_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_calendar_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_calendar_plant_date_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(CalendarDate), OrderByType.Asc, true)]
[SugarIndex("ix_calendar_calendar_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CalendarDate), OrderByType.Asc, false)]
[SugarIndex("ix_calendar_is_working_day", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsWorkingDay), OrderByType.Asc, false)]
public class TaktCalendar : TaktCompanyEntityBase
{
    /// <summary>
    /// 日历日期
    /// </summary>
    [SugarColumn(ColumnName = "calendar_date", ColumnDescription = "日历日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime CalendarDate { get; set; }
    /// <summary>
    /// 月内第几天（1～31；由 CalendarDate 派生）
    /// </summary>
    [SugarColumn(ColumnName = "day_of_month", ColumnDescription = "月内第几天", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DayOfMonth { get; set; }
    /// <summary>
    /// 星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）
    /// </summary>
    [SugarColumn(ColumnName = "weekday", ColumnDescription = "星期", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Weekday { get; set; }
    /// <summary>
    /// 年内第几周（ISO 8601；1～53；由 CalendarDate 派生）
    /// </summary>
    [SugarColumn(ColumnName = "week_of_year", ColumnDescription = "年内第几周", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int WeekOfYear { get; set; }
    /// <summary>
    /// 季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）
    /// </summary>
    [SugarColumn(ColumnName = "quarter", ColumnDescription = "季度", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Quarter { get; set; }
    /// <summary>
    /// 季内第几天（1～92；自然年季度；由 CalendarDate 派生）
    /// </summary>
    [SugarColumn(ColumnName = "day_of_quarter", ColumnDescription = "季内第几天", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DayOfQuarter { get; set; }
    /// <summary>
    /// 年内第几天（1～366；由 CalendarDate 派生）
    /// </summary>
    [SugarColumn(ColumnName = "day_of_year", ColumnDescription = "年内第几天", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DayOfYear { get; set; }
    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    [SugarColumn(ColumnName = "is_working_day", ColumnDescription = "是否工作日", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsWorkingDay { get; set; }
    /// <summary>
    /// 关联假日（选项 TaktHolidays/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "holiday_id", ColumnDescription = "关联假日ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }
    /// <summary>
    /// 关联班次（选项 TaktWorkShifts/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "shift_id", ColumnDescription = "关联班次ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }
}
