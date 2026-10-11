// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktShiftScheduleDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：ShiftSchedule 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktShiftSchedule 生成，请按需审阅）
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
// ShiftSchedule 响应 DTO
// ========================================

/// <summary>
/// 排班计划（ScheduleType=0 部门排班时 DeptId 必填；ScheduleType=1 人员排班时 EmployeeId 必填）
/// 对应前端 TaktShiftScheduleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktShiftScheduleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ShiftScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ShiftScheduleId { get; set; }


}

// ========================================
// ShiftSchedule 查询 DTO
// ========================================

/// <summary>
/// ShiftSchedule 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktShiftScheduleQueryDto : TaktPagedQuery
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
    /// 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
    /// </summary>
    public int? ScheduleType { get; set; }

    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 排班日期（范围查询-开始）
    /// </summary>
    public DateTime? ScheduleDateStart { get; set; }

    /// <summary>
    /// 排班日期（范围查询-结束）
    /// </summary>
    public DateTime? ScheduleDateEnd { get; set; }

    /// <summary>
    /// 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
// 创建ShiftSchedule DTO
// ========================================

/// <summary>
/// 创建ShiftSchedule DTO
/// </summary>
public class TaktShiftScheduleCreateDto
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
    /// 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
    /// </summary>
    public int ScheduleType { get; set; } = 0;

    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 排班日期
    /// </summary>
    public DateTime ScheduleDate { get; set; }

    /// <summary>
    /// 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ShiftId { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
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
// 更新ShiftSchedule DTO
// ========================================

/// <summary>
/// 更新ShiftSchedule DTO
/// 继承 TaktShiftScheduleCreateDto，添加 ShiftScheduleId 字段
/// </summary>
public class TaktShiftScheduleUpdateDto : TaktShiftScheduleCreateDto
{
    /// <summary>
    /// ShiftScheduleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ShiftScheduleId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ShiftSchedule 导入模板行 DTO
/// </summary>
public class TaktShiftScheduleTemplateDto
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
    /// 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
    /// </summary>
    public int? ScheduleType { get; set; }

    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 排班日期
    /// </summary>
    public DateTime? ScheduleDate { get; set; }

    /// <summary>
    /// 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
/// ShiftSchedule 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktShiftScheduleImportDto
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
    /// 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
    /// </summary>
    public int? ScheduleType { get; set; }

    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 排班日期
    /// </summary>
    public DateTime? ScheduleDate { get; set; }

    /// <summary>
    /// 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
/// ShiftSchedule 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktShiftScheduleExportDto
{
    /// <summary>
    /// ShiftScheduleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ShiftScheduleId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
    /// </summary>
    public int ScheduleType { get; set; } = 0;

    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 排班日期
    /// </summary>
    public DateTime ScheduleDate { get; set; }

    /// <summary>
    /// 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ShiftId { get; set; }

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
