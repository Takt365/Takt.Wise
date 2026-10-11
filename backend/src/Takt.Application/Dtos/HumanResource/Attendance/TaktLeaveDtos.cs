// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktLeaveDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：Leave 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktLeave 生成，请按需审阅）
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
// Leave 响应 DTO
// ========================================

/// <summary>
/// 请假实体。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与请假模块对接。
/// 对应前端 TaktLeaveDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktLeaveDto : TaktApprovalDtoBase
{
    /// <summary>
    /// LeaveID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LeaveId { get; set; }

    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

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
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 请假事由
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int LeaveStatus { get; set; } = 0;

}

// ========================================
// Leave 查询 DTO
// ========================================

/// <summary>
/// Leave 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktLeaveQueryDto : TaktPagedQuery
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

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
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    public string? LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期（范围查询-开始）
    /// </summary>
    public DateTime? StartDateStart { get; set; }

    /// <summary>
    /// 开始日期（范围查询-结束）
    /// </summary>
    public DateTime? StartDateEnd { get; set; }

    /// <summary>
    /// 结束日期（范围查询-开始）
    /// </summary>
    public DateTime? EndDateStart { get; set; }

    /// <summary>
    /// 结束日期（范围查询-结束）
    /// </summary>
    public DateTime? EndDateEnd { get; set; }

    /// <summary>
    /// 请假事由
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int? LeaveStatus { get; set; }

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
// 创建Leave DTO
// ========================================

/// <summary>
/// 创建Leave DTO
/// </summary>
public class TaktLeaveCreateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

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
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    [Required(ErrorMessage = "请假类型（字典 sys_leave_type；列存 DictValue）不能为空")]
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 请假事由
    /// </summary>
    [Required(ErrorMessage = "请假事由不能为空")]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int LeaveStatus { get; set; } = 0;

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
// 更新Leave DTO
// ========================================

/// <summary>
/// 更新Leave DTO
/// 继承 TaktLeaveCreateDto，添加 LeaveId 字段
/// </summary>
public class TaktLeaveUpdateDto : TaktLeaveCreateDto
{
    /// <summary>
    /// LeaveID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LeaveId { get; set; }

}

// ========================================
// Leave 状态 DTO
// ========================================

/// <summary>
/// Leave 状态更新 DTO
/// </summary>
public class TaktLeaveStatusDto
{
    /// <summary>
    /// LeaveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LeaveId { get; set; }

    /// <summary>
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    [Required(ErrorMessage = "请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）不能为空")]
    public int LeaveStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Leave 导入模板行 DTO
/// </summary>
public class TaktLeaveTemplateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

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
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    public string? LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 请假事由
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int? LeaveStatus { get; set; }

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
/// Leave 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktLeaveImportDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

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
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    public string? LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 请假事由
    /// </summary>
    public string? Reason { get; set; } = string.Empty;

    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int? LeaveStatus { get; set; }

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
/// Leave 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktLeaveExportDto
{
    /// <summary>
    /// LeaveID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long LeaveId { get; set; }

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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

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
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// 开始日期
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 请假事由
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    public int LeaveStatus { get; set; } = 0;

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
