// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderLaborDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceWorkOrderLabor 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaintenanceWorkOrderLabor 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Maintenance;

// ========================================
// MaintenanceWorkOrderLabor 响应 DTO
// ========================================

/// <summary>
/// 维护工单报工明细实体（主子表：挂载于维护工单）
/// 对应前端 TaktMaintenanceWorkOrderLaborDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaintenanceWorkOrderLaborDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaintenanceWorkOrderLaborID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderLaborId { get; set; }

    /// <summary>
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单名称（填充字段）
    /// </summary>
    public string? MaintenanceWorkOrderName { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 报工日期
    /// </summary>
    public DateTime WorkDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    public decimal WorkHours { get; set; }

    /// <summary>
    /// 小时费率
    /// </summary>
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal LaborCost { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    public string? OperationDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    public int ConfirmationStatus { get; set; } = 0;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 维护工单（主表）
    /// （主表：TaktMaintenanceWorkOrder）
    /// </summary>
    public TaktMaintenanceWorkOrderDto? MaintenanceWorkOrder { get; set; }

}

// ========================================
// MaintenanceWorkOrderLabor 查询 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderLabor 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaintenanceWorkOrderLaborQueryDto : TaktPagedQuery
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 报工日期（范围查询-开始）
    /// </summary>
    public DateTime? WorkDateStart { get; set; }

    /// <summary>
    /// 报工日期（范围查询-结束）
    /// </summary>
    public DateTime? WorkDateEnd { get; set; }

    /// <summary>
    /// 开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }

    /// <summary>
    /// 开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }

    /// <summary>
    /// 结束时间（范围查询-开始）
    /// </summary>
    public DateTime? EndTimeStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? EndTimeEnd { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    public decimal? WorkHours { get; set; }

    /// <summary>
    /// 小时费率
    /// </summary>
    public decimal? HourlyRate { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal? LaborCost { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    public string? OperationDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    public int? ConfirmationStatus { get; set; }

    /// <summary>
    /// 确认时间（范围查询-开始）
    /// </summary>
    public DateTime? ConfirmedAtStart { get; set; }

    /// <summary>
    /// 确认时间（范围查询-结束）
    /// </summary>
    public DateTime? ConfirmedAtEnd { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建MaintenanceWorkOrderLabor DTO
// ========================================

/// <summary>
/// 创建MaintenanceWorkOrderLabor DTO
/// </summary>
public class TaktMaintenanceWorkOrderLaborCreateDto
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    [Required(ErrorMessage = "维护工单号（冗余）不能为空")]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 报工日期
    /// </summary>
    public DateTime WorkDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    public decimal WorkHours { get; set; }

    /// <summary>
    /// 小时费率
    /// </summary>
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal LaborCost { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    public string? OperationDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    public int ConfirmationStatus { get; set; } = 0;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新MaintenanceWorkOrderLabor DTO
// ========================================

/// <summary>
/// 更新MaintenanceWorkOrderLabor DTO
/// 继承 TaktMaintenanceWorkOrderLaborCreateDto，添加 MaintenanceWorkOrderLaborId 字段
/// </summary>
public class TaktMaintenanceWorkOrderLaborUpdateDto : TaktMaintenanceWorkOrderLaborCreateDto
{
    /// <summary>
    /// MaintenanceWorkOrderLaborID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderLaborId { get; set; }

}

// ========================================
// MaintenanceWorkOrderLabor 状态 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderLabor 状态更新 DTO
/// </summary>
public class TaktMaintenanceWorkOrderLaborStatusDto
{
    /// <summary>
    /// MaintenanceWorkOrderLaborID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderLaborId { get; set; }

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    [Required(ErrorMessage = "报工确认状态（0=待确认，1=已确认）不能为空")]
    public int ConfirmationStatus { get; set; } = 0;
}

// ========================================
// MaintenanceWorkOrderLabor 作废 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderLabor 作废/撤销作废 DTO
/// </summary>
public class TaktMaintenanceWorkOrderLaborObsoleteDto
{
    /// <summary>
    /// MaintenanceWorkOrderLaborID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderLaborId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrderLabor 导入模板行 DTO
/// </summary>
public class TaktMaintenanceWorkOrderLaborTemplateDto
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 报工日期
    /// </summary>
    public DateTime? WorkDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    public decimal? WorkHours { get; set; }

    /// <summary>
    /// 小时费率
    /// </summary>
    public decimal? HourlyRate { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal? LaborCost { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    public string? OperationDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    public int? ConfirmationStatus { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// MaintenanceWorkOrderLabor 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaintenanceWorkOrderLaborImportDto
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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 报工日期
    /// </summary>
    public DateTime? WorkDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    public decimal? WorkHours { get; set; }

    /// <summary>
    /// 小时费率
    /// </summary>
    public decimal? HourlyRate { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal? LaborCost { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    public string? OperationDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    public int? ConfirmationStatus { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// MaintenanceWorkOrderLabor 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaintenanceWorkOrderLaborExportDto
{
    /// <summary>
    /// MaintenanceWorkOrderLaborID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderLaborId { get; set; }

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
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 报工日期
    /// </summary>
    public DateTime WorkDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    public decimal WorkHours { get; set; }

    /// <summary>
    /// 小时费率
    /// </summary>
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// 人工成本
    /// </summary>
    public decimal LaborCost { get; set; }

    /// <summary>
    /// 作业描述
    /// </summary>
    public string? OperationDescription { get; set; } = string.Empty;

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    public int ConfirmationStatus { get; set; } = 0;

    /// <summary>
    /// 确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
