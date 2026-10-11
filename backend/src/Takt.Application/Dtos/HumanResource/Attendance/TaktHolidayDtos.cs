// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktHolidayDtos.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：Holiday 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktHoliday 生成，请按需审阅）
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
// Holiday 响应 DTO
// ========================================

/// <summary>
/// 假日实体 假日条目，用于考勤日历、排班与薪资计算；字典 humanresource_attendance_holiday_category、humanresource_attendance_holiday_working_day_type 与字段取值一致 公司级实体：按 TenantCode + CompanyCode 隔离；同一公司内以开始日期+结束日期+假日类型唯一
/// 对应前端 TaktHolidayDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktHolidayDto : TaktCompanyDtoBase
{
    /// <summary>
    /// HolidayID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HolidayId { get; set; }

    /// <summary>
    /// 假日名称
    /// </summary>
    public string HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int HolidayType { get; set; } = 0;

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 假期天数（含起止日；由 StartDate～EndDate 派生）
    /// </summary>
    public int DaysCount { get; set; } = 1;

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；多对分号分隔，如 2026-09-20=&gt;2026-10-06;2026-10-10=&gt;2026-10-07；无可空串）
    /// </summary>
    public string CompensatoryWorkDates { get; set; } = string.Empty;

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPaid { get; set; } = 1;

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string HolidayTheme { get; set; } = string.Empty;

}

// ========================================
// Holiday 查询 DTO
// ========================================

/// <summary>
/// Holiday 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktHolidayQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 假日名称
    /// </summary>
    public string? HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int? HolidayType { get; set; }

    /// <summary>
    /// 假日开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 假日开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 假日结束日期（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 假日结束日期（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

    /// <summary>
    /// 假期天数（含起止日）
    /// </summary>
    public int? DaysCount { get; set; }

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；模糊查询）
    /// </summary>
    public string? CompensatoryWorkDates { get; set; }

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsPaid { get; set; }

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string? HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string? HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string? HolidayTheme { get; set; } = string.Empty;

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
// 创建Holiday DTO
// ========================================

/// <summary>
/// 创建Holiday DTO
/// </summary>
public class TaktHolidayCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 假日名称
    /// </summary>
    [Required(ErrorMessage = "假日名称不能为空")]
    public string HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int HolidayType { get; set; } = 0;

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 假期天数（含起止日；由 StartDate～EndDate 派生）
    /// </summary>
    public int DaysCount { get; set; } = 1;

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；多对分号分隔，如 2026-09-20=&gt;2026-10-06;2026-10-10=&gt;2026-10-07；无可空串）
    /// </summary>
    public string CompensatoryWorkDates { get; set; } = string.Empty;

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPaid { get; set; } = 1;

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    [Required(ErrorMessage = "假日问候语（简短，用于界面问候展示）不能为空")]
    public string HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    [Required(ErrorMessage = "假日引用/诗句（用于引用区展示）不能为空")]
    public string HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    [Required(ErrorMessage = "假日主题（对应前端主题色 key，用于日历等非工作日展示）不能为空")]
    public string HolidayTheme { get; set; } = string.Empty;

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
// 更新Holiday DTO
// ========================================

/// <summary>
/// 更新Holiday DTO
/// 继承 TaktHolidayCreateDto，添加 HolidayId 字段
/// </summary>
public class TaktHolidayUpdateDto : TaktHolidayCreateDto
{
    /// <summary>
    /// HolidayID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HolidayId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Holiday 导入模板行 DTO
/// </summary>
public class TaktHolidayTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 假日名称
    /// </summary>
    public string? HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int? HolidayType { get; set; }

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 假期天数（含起止日；由 StartDate～EndDate 派生）
    /// </summary>
    public int? DaysCount { get; set; }

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；多对分号分隔，如 2026-09-20=&gt;2026-10-06;2026-10-10=&gt;2026-10-07；无可空串）
    /// </summary>
    public string CompensatoryWorkDates { get; set; } = string.Empty;

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsPaid { get; set; }

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string? HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string? HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string? HolidayTheme { get; set; } = string.Empty;

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
/// Holiday 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktHolidayImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 假日名称
    /// </summary>
    public string? HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int? HolidayType { get; set; }

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 假期天数（含起止日；由 StartDate～EndDate 派生）
    /// </summary>
    public int? DaysCount { get; set; }

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；多对分号分隔，如 2026-09-20=&gt;2026-10-06;2026-10-10=&gt;2026-10-07；无可空串）
    /// </summary>
    public string CompensatoryWorkDates { get; set; } = string.Empty;

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsPaid { get; set; }

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string? HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string? HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string? HolidayTheme { get; set; } = string.Empty;

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
/// Holiday 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktHolidayExportDto
{
    /// <summary>
    /// HolidayID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HolidayId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 假日名称
    /// </summary>
    public string HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int HolidayType { get; set; } = 0;

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 假期天数（含起止日；由 StartDate～EndDate 派生）
    /// </summary>
    public int DaysCount { get; set; } = 1;

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；多对分号分隔，如 2026-09-20=&gt;2026-10-06;2026-10-10=&gt;2026-10-07；无可空串）
    /// </summary>
    public string CompensatoryWorkDates { get; set; } = string.Empty;

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPaid { get; set; } = 1;

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string HolidayTheme { get; set; } = string.Empty;

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


// ========================================
// 假日主题（登录前预览）
// ========================================

/// <summary>
/// 服务器当日、用户默认登录公司下的假日主题响应 DTO
/// 对应前端 TaktHolidayThemeDto；业务字段与 TaktHoliday 实体一致，并追加 IsHolidayToday
/// </summary>
public class TaktHolidayThemeDto
{
    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 假日名称
    /// </summary>
    public string HolidayName { get; set; } = string.Empty;

    /// <summary>
    /// 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
    /// </summary>
    public int HolidayType { get; set; } = 0;

    /// <summary>
    /// 假日开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 假日结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 假期天数（含起止日；由 StartDate～EndDate 派生）
    /// </summary>
    public int DaysCount { get; set; } = 1;

    /// <summary>
    /// 调休对应（上班日=&gt;所补放假日；多对分号分隔，如 2026-09-20=&gt;2026-10-06;2026-10-10=&gt;2026-10-07；无可空串）
    /// </summary>
    public string CompensatoryWorkDates { get; set; } = string.Empty;

    /// <summary>
    /// 是否带薪假（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPaid { get; set; } = 1;

    /// <summary>
    /// 假日问候语（简短，用于界面问候展示）
    /// </summary>
    public string HolidayGreeting { get; set; } = string.Empty;

    /// <summary>
    /// 假日引用/诗句（用于引用区展示）
    /// </summary>
    public string HolidayQuote { get; set; } = string.Empty;

    /// <summary>
    /// 假日主题（对应前端主题色 key，用于日历等非工作日展示）
    /// </summary>
    public string HolidayTheme { get; set; } = string.Empty;

    /// <summary>
    /// 服务器当日是否处于假日区间且为非工作日（用于问候/引用区；无匹配记录时为 false）
    /// </summary>
    public bool IsHolidayToday { get; set; }
}
