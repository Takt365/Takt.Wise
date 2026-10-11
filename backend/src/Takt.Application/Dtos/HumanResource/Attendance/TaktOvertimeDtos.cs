// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktOvertimeDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：Overtime 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktOvertime 生成，请按需审阅）
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
// Overtime 响应 DTO
// ========================================

/// <summary>
/// 加班申请（时长与状态由业务维护，可与工作流扩展对接）
/// 对应前端 TaktOvertimeDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktOvertimeDto : TaktApprovalDtoBase
{
    /// <summary>
    /// OvertimeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OvertimeId { get; set; }

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 加班归属日期
    /// </summary>
    public DateTime OvertimeDate { get; set; }

    /// <summary>
    /// 计划加班开始时间
    /// </summary>
    public DateTime PlannedStartTime { get; set; }

    /// <summary>
    /// 计划加班结束时间
    /// </summary>
    public DateTime PlannedEndTime { get; set; }

    /// <summary>
    /// 加班总人数
    /// </summary>
    public int TotalEmployees { get; set; } = 0;

    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    public decimal TotalPlannedHours { get; set; }

    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    public decimal TotalActualHours { get; set; }

    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    public int OvertimeType { get; set; } = 0;

    /// <summary>
    /// 加班原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HandlingBy { get; set; }

    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? HandlingByName { get; set; } = string.Empty;

    /// <summary>
    /// 经办时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 经办备注
    /// </summary>
    public string? HandlingComment { get; set; } = string.Empty;

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int OvertimeStatus { get; set; } = 0;

    /// <summary>
    /// 加班明细列表
    /// （子表：TaktOvertimeItem）
    /// </summary>
    public List<TaktOvertimeItemDto>? Items { get; set; }

}

// ========================================
// Overtime 查询 DTO
// ========================================

/// <summary>
/// Overtime 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktOvertimeQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 加班归属日期（范围查询-开始）
    /// </summary>
    public DateTime? OvertimeDateStart { get; set; }

    /// <summary>
    /// 加班归属日期（范围查询-结束）
    /// </summary>
    public DateTime? OvertimeDateEnd { get; set; }

    /// <summary>
    /// 计划加班开始时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartTimeStart { get; set; }

    /// <summary>
    /// 计划加班开始时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartTimeEnd { get; set; }

    /// <summary>
    /// 计划加班结束时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndTimeStart { get; set; }

    /// <summary>
    /// 计划加班结束时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndTimeEnd { get; set; }

    /// <summary>
    /// 加班总人数
    /// </summary>
    public int? TotalEmployees { get; set; }

    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    public decimal? TotalPlannedHours { get; set; }

    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    public decimal? TotalActualHours { get; set; }

    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    public int? OvertimeType { get; set; }

    /// <summary>
    /// 加班原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HandlingBy { get; set; }

    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? HandlingByName { get; set; } = string.Empty;

    /// <summary>
    /// 经办时间（范围查询-开始）
    /// </summary>
    public DateTime? HandlingAtStart { get; set; }

    /// <summary>
    /// 经办时间（范围查询-结束）
    /// </summary>
    public DateTime? HandlingAtEnd { get; set; }

    /// <summary>
    /// 经办备注
    /// </summary>
    public string? HandlingComment { get; set; } = string.Empty;

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int? OvertimeStatus { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建Overtime DTO
// ========================================

/// <summary>
/// 创建Overtime DTO
/// </summary>
public class TaktOvertimeCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 加班归属日期
    /// </summary>
    public DateTime OvertimeDate { get; set; }

    /// <summary>
    /// 计划加班开始时间
    /// </summary>
    public DateTime PlannedStartTime { get; set; }

    /// <summary>
    /// 计划加班结束时间
    /// </summary>
    public DateTime PlannedEndTime { get; set; }

    /// <summary>
    /// 加班总人数
    /// </summary>
    public int TotalEmployees { get; set; } = 0;

    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    public decimal TotalPlannedHours { get; set; }

    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    public decimal TotalActualHours { get; set; }

    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    public int OvertimeType { get; set; } = 0;

    /// <summary>
    /// 加班原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HandlingBy { get; set; }

    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? HandlingByName { get; set; } = string.Empty;

    /// <summary>
    /// 经办时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 经办备注
    /// </summary>
    public string? HandlingComment { get; set; } = string.Empty;

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int OvertimeStatus { get; set; } = 0;

    /// <summary>
    /// 加班明细列表（子表，级联保存）
    /// </summary>
    public List<TaktOvertimeItemCreateDto>? Items { get; set; }

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
// 更新Overtime DTO
// ========================================

/// <summary>
/// 更新Overtime DTO
/// 继承 TaktOvertimeCreateDto，添加 OvertimeId 字段
/// </summary>
public class TaktOvertimeUpdateDto : TaktOvertimeCreateDto
{
    /// <summary>
    /// OvertimeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OvertimeId { get; set; }

    /// <summary>
    /// 加班明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktOvertimeItemUpdateDto>? Items { get; set; }

}

// ========================================
// Overtime 状态 DTO
// ========================================

/// <summary>
/// Overtime 状态更新 DTO
/// </summary>
public class TaktOvertimeStatusDto
{
    /// <summary>
    /// OvertimeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OvertimeId { get; set; }

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    [Required(ErrorMessage = "加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）不能为空")]
    public int OvertimeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Overtime 导入模板行 DTO
/// </summary>
public class TaktOvertimeTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 加班归属日期
    /// </summary>
    public DateTime? OvertimeDate { get; set; }

    /// <summary>
    /// 计划加班开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划加班结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 加班总人数
    /// </summary>
    public int? TotalEmployees { get; set; }

    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    public decimal? TotalPlannedHours { get; set; }

    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    public decimal? TotalActualHours { get; set; }

    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    public int? OvertimeType { get; set; }

    /// <summary>
    /// 加班原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HandlingBy { get; set; }

    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? HandlingByName { get; set; } = string.Empty;

    /// <summary>
    /// 经办时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 经办备注
    /// </summary>
    public string? HandlingComment { get; set; } = string.Empty;

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int? OvertimeStatus { get; set; }

    /// <summary>
    /// 加班明细列表（子表，级联保存）
    /// </summary>
    public List<TaktOvertimeItemCreateDto>? Items { get; set; }

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
/// Overtime 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktOvertimeImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 加班归属日期
    /// </summary>
    public DateTime? OvertimeDate { get; set; }

    /// <summary>
    /// 计划加班开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划加班结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 加班总人数
    /// </summary>
    public int? TotalEmployees { get; set; }

    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    public decimal? TotalPlannedHours { get; set; }

    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    public decimal? TotalActualHours { get; set; }

    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    public int? OvertimeType { get; set; }

    /// <summary>
    /// 加班原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HandlingBy { get; set; }

    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? HandlingByName { get; set; } = string.Empty;

    /// <summary>
    /// 经办时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 经办备注
    /// </summary>
    public string? HandlingComment { get; set; } = string.Empty;

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int? OvertimeStatus { get; set; }

    /// <summary>
    /// 加班明细列表（子表，级联保存）
    /// </summary>
    public List<TaktOvertimeItemCreateDto>? Items { get; set; }

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
/// Overtime 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktOvertimeExportDto
{
    /// <summary>
    /// OvertimeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OvertimeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 加班归属日期
    /// </summary>
    public DateTime OvertimeDate { get; set; }

    /// <summary>
    /// 计划加班开始时间
    /// </summary>
    public DateTime PlannedStartTime { get; set; }

    /// <summary>
    /// 计划加班结束时间
    /// </summary>
    public DateTime PlannedEndTime { get; set; }

    /// <summary>
    /// 加班总人数
    /// </summary>
    public int TotalEmployees { get; set; } = 0;

    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    public decimal TotalPlannedHours { get; set; }

    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    public decimal TotalActualHours { get; set; }

    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    public int OvertimeType { get; set; } = 0;

    /// <summary>
    /// 加班原因
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HandlingBy { get; set; }

    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? HandlingByName { get; set; } = string.Empty;

    /// <summary>
    /// 经办时间
    /// </summary>
    public DateTime? HandlingAt { get; set; }

    /// <summary>
    /// 经办备注
    /// </summary>
    public string? HandlingComment { get; set; } = string.Empty;

    /// <summary>
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int OvertimeStatus { get; set; } = 0;

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
