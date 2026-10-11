// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktCalendarDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Calendar 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCalendar 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Attendance;

// ========================================
// Calendar 响应 DTO
// ========================================

/// <summary>
/// 工厂日历（公司级；按 PlantCode 区分工厂维度）
/// 对应前端 TaktCalendarDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCalendarDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CalendarID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CalendarId { get; set; }

    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime CalendarDate { get; set; }

    /// <summary>
    /// 月内第几天（1～31；由 CalendarDate 派生）
    /// </summary>
    public int DayOfMonth { get; set; }

    /// <summary>
    /// 星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）
    /// </summary>
    public int Weekday { get; set; }

    /// <summary>
    /// 年内第几周（ISO 8601；1～53；由 CalendarDate 派生）
    /// </summary>
    public int WeekOfYear { get; set; }

    /// <summary>
    /// 季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）
    /// </summary>
    public int Quarter { get; set; }

    /// <summary>
    /// 季内第几天（1～92；自然年季度；由 CalendarDate 派生）
    /// </summary>
    public int DayOfQuarter { get; set; }

    /// <summary>
    /// 年内第几天（1～366；由 CalendarDate 派生）
    /// </summary>
    public int DayOfYear { get; set; }

    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }
}

// ========================================
// Calendar 查询 DTO
// ========================================

/// <summary>
/// Calendar 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCalendarQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 日历日期（范围查询-开始）
    /// </summary>
    public DateTime? CalendarDateStart { get; set; }

    /// <summary>
    /// 日历日期（范围查询-结束）
    /// </summary>
    public DateTime? CalendarDateEnd { get; set; }

    /// <summary>
    /// 月内第几天（1～31）
    /// </summary>
    public int? DayOfMonth { get; set; }

    /// <summary>
    /// 星期（1=周一 … 7=周日）
    /// </summary>
    public int? Weekday { get; set; }

    /// <summary>
    /// 年内第几周（ISO 8601；1～53）
    /// </summary>
    public int? WeekOfYear { get; set; }

    /// <summary>
    /// 季度（1～4）
    /// </summary>
    public int? Quarter { get; set; }

    /// <summary>
    /// 季内第几天（1～92）
    /// </summary>
    public int? DayOfQuarter { get; set; }

    /// <summary>
    /// 年内第几天（1～366）
    /// </summary>
    public int? DayOfYear { get; set; }

    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    public int? IsWorkingDay { get; set; }

    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Calendar DTO
// ========================================

/// <summary>
/// 创建Calendar DTO
/// </summary>
public class TaktCalendarCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime CalendarDate { get; set; }

    /// <summary>
    /// 月内第几天（1～31；由 CalendarDate 派生）
    /// </summary>
    public int DayOfMonth { get; set; }

    /// <summary>
    /// 星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）
    /// </summary>
    public int Weekday { get; set; }

    /// <summary>
    /// 年内第几周（ISO 8601；1～53；由 CalendarDate 派生）
    /// </summary>
    public int WeekOfYear { get; set; }

    /// <summary>
    /// 季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）
    /// </summary>
    public int Quarter { get; set; }

    /// <summary>
    /// 季内第几天（1～92；自然年季度；由 CalendarDate 派生）
    /// </summary>
    public int DayOfQuarter { get; set; }

    /// <summary>
    /// 年内第几天（1～366；由 CalendarDate 派生）
    /// </summary>
    public int DayOfYear { get; set; }

    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [Required(ErrorMessage = "关联工厂（为空表示公司级通用日历）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Calendar DTO
// ========================================

/// <summary>
/// 更新Calendar DTO
/// 继承 TaktCalendarCreateDto，添加 CalendarId 字段
/// </summary>
public class TaktCalendarUpdateDto : TaktCalendarCreateDto
{
    /// <summary>
    /// CalendarID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CalendarId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Calendar 导入模板行 DTO
/// </summary>
public class TaktCalendarTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime? CalendarDate { get; set; }

    /// <summary>
    /// 月内第几天（1～31；由 CalendarDate 派生）
    /// </summary>
    public int? DayOfMonth { get; set; }

    /// <summary>
    /// 星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）
    /// </summary>
    public int? Weekday { get; set; }

    /// <summary>
    /// 年内第几周（ISO 8601；1～53；由 CalendarDate 派生）
    /// </summary>
    public int? WeekOfYear { get; set; }

    /// <summary>
    /// 季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）
    /// </summary>
    public int? Quarter { get; set; }

    /// <summary>
    /// 季内第几天（1～92；自然年季度；由 CalendarDate 派生）
    /// </summary>
    public int? DayOfQuarter { get; set; }

    /// <summary>
    /// 年内第几天（1～366；由 CalendarDate 派生）
    /// </summary>
    public int? DayOfYear { get; set; }

    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    public int? IsWorkingDay { get; set; }

    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Calendar 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCalendarImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime? CalendarDate { get; set; }

    /// <summary>
    /// 月内第几天（1～31；由 CalendarDate 派生）
    /// </summary>
    public int? DayOfMonth { get; set; }

    /// <summary>
    /// 星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）
    /// </summary>
    public int? Weekday { get; set; }

    /// <summary>
    /// 年内第几周（ISO 8601；1～53；由 CalendarDate 派生）
    /// </summary>
    public int? WeekOfYear { get; set; }

    /// <summary>
    /// 季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）
    /// </summary>
    public int? Quarter { get; set; }

    /// <summary>
    /// 季内第几天（1～92；自然年季度；由 CalendarDate 派生）
    /// </summary>
    public int? DayOfQuarter { get; set; }

    /// <summary>
    /// 年内第几天（1～366；由 CalendarDate 派生）
    /// </summary>
    public int? DayOfYear { get; set; }

    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    public int? IsWorkingDay { get; set; }

    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Calendar 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCalendarExportDto
{
    /// <summary>
    /// CalendarID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CalendarId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 日历日期
    /// </summary>
    public DateTime CalendarDate { get; set; }

    /// <summary>
    /// 月内第几天（1～31；由 CalendarDate 派生）
    /// </summary>
    public int DayOfMonth { get; set; }

    /// <summary>
    /// 星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）
    /// </summary>
    public int Weekday { get; set; }

    /// <summary>
    /// 年内第几周（ISO 8601；1～53；由 CalendarDate 派生）
    /// </summary>
    public int WeekOfYear { get; set; }

    /// <summary>
    /// 季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）
    /// </summary>
    public int Quarter { get; set; }

    /// <summary>
    /// 季内第几天（1～92；自然年季度；由 CalendarDate 派生）
    /// </summary>
    public int DayOfQuarter { get; set; }

    /// <summary>
    /// 年内第几天（1～366；由 CalendarDate 派生）
    /// </summary>
    public int DayOfYear { get; set; }

    /// <summary>
    /// 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
    /// </summary>
    public int IsWorkingDay { get; set; } = 0;

    /// <summary>
    /// 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HolidayId { get; set; }

    /// <summary>
    /// 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
