// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Training
// 文件名称：TaktTrainingAttendeeDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingAttendee 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTrainingAttendee 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Training;

// ========================================
// TrainingAttendee 响应 DTO
// ========================================

/// <summary>
/// 员工培训结果记录
/// 对应前端 TaktTrainingAttendeeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTrainingAttendeeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TrainingAttendeeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingAttendeeId { get; set; }


    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public int TrainingResultStatus { get; set; } = 0;

}

// ========================================
// TrainingAttendee 查询 DTO
// ========================================

/// <summary>
/// TrainingAttendee 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTrainingAttendeeQueryDto : TaktPagedQuery
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string? TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string? Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期（范围查询-开始）
    /// </summary>
    public DateTime? TrainingStartDateStart { get; set; }

    /// <summary>
    /// 培训开始日期（范围查询-结束）
    /// </summary>
    public DateTime? TrainingStartDateEnd { get; set; }

    /// <summary>
    /// 培训结束日期（范围查询-开始）
    /// </summary>
    public DateTime? TrainingEndDateStart { get; set; }

    /// <summary>
    /// 培训结束日期（范围查询-结束）
    /// </summary>
    public DateTime? TrainingEndDateEnd { get; set; }

    /// <summary>
    /// 培训日期（范围查询-开始）
    /// </summary>
    public DateTime? TrainingDateStart { get; set; }

    /// <summary>
    /// 培训日期（范围查询-结束）
    /// </summary>
    public DateTime? TrainingDateEnd { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal? TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int? IsPassed { get; set; }

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string? TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public int? TrainingResultStatus { get; set; }

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
// 创建TrainingAttendee DTO
// ========================================

/// <summary>
/// 创建TrainingAttendee DTO
/// </summary>
public class TaktTrainingAttendeeCreateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    [Required(ErrorMessage = "员工姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    [Required(ErrorMessage = "培训课程名称不能为空")]
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    [Required(ErrorMessage = "培训类型不能为空")]
    public string TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    [Required(ErrorMessage = "培训讲师不能为空")]
    public string Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int IsPassed { get; set; } = 0;

    /// <summary>
    /// 证书编码
    /// </summary>
    [Required(ErrorMessage = "证书编码不能为空")]
    public string CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    [Required(ErrorMessage = "培训评价不能为空")]
    public string TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public int TrainingResultStatus { get; set; } = 0;

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
// 更新TrainingAttendee DTO
// ========================================

/// <summary>
/// 更新TrainingAttendee DTO
/// 继承 TaktTrainingAttendeeCreateDto，添加 TrainingAttendeeId 字段
/// </summary>
public class TaktTrainingAttendeeUpdateDto : TaktTrainingAttendeeCreateDto
{
    /// <summary>
    /// TrainingAttendeeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingAttendeeId { get; set; }

}

// ========================================
// TrainingAttendee 状态 DTO
// ========================================

/// <summary>
/// TrainingAttendee 状态更新 DTO
/// </summary>
public class TaktTrainingAttendeeStatusDto
{
    /// <summary>
    /// TrainingAttendeeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingAttendeeId { get; set; }

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    [Required(ErrorMessage = "状态（1=有效 0=无效）不能为空")]
    public int TrainingResultStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TrainingAttendee 导入模板行 DTO
/// </summary>
public class TaktTrainingAttendeeTemplateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string? TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string? Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime? TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime? TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime? TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal? TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int? IsPassed { get; set; }

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string? TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public int? TrainingResultStatus { get; set; }

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
/// TrainingAttendee 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTrainingAttendeeImportDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string? TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string? Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime? TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime? TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime? TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal? TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int? IsPassed { get; set; }

    /// <summary>
    /// 证书编码
    /// </summary>
    public string? CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string? TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public int? TrainingResultStatus { get; set; }

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
/// TrainingAttendee 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTrainingAttendeeExportDto
{
    /// <summary>
    /// TrainingAttendeeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingAttendeeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 培训课程 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 培训课程名称
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 培训类型
    /// </summary>
    public string TrainingType { get; set; } = string.Empty;

    /// <summary>
    /// 培训讲师
    /// </summary>
    public string Instructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训开始日期
    /// </summary>
    public DateTime TrainingStartDate { get; set; }

    /// <summary>
    /// 培训结束日期
    /// </summary>
    public DateTime TrainingEndDate { get; set; }

    /// <summary>
    /// 培训日期
    /// </summary>
    public DateTime TrainingDate { get; set; }

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 培训成绩
    /// </summary>
    public decimal TrainingScore { get; set; }

    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    public int IsPassed { get; set; } = 0;

    /// <summary>
    /// 证书编码
    /// </summary>
    public string CertificateCode { get; set; } = string.Empty;

    /// <summary>
    /// 培训评价
    /// </summary>
    public string TrainingEvaluation { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    public int TrainingResultStatus { get; set; } = 0;

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
