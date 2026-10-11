// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Training
// 文件名称：TaktTrainingCourseDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingCourse 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTrainingCourse 生成，请按需审阅）
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
// TrainingCourse 响应 DTO
// ========================================

/// <summary>
/// 培训课程定义
/// 对应前端 TaktTrainingCourseDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTrainingCourseDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TrainingCourseID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }


    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    public int TrainingCourseStatus { get; set; } = 0;

}

// ========================================
// TrainingCourse 查询 DTO
// ========================================

/// <summary>
/// TrainingCourse 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTrainingCourseQueryDto : TaktPagedQuery
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
    /// 课程编码（租户+公司内唯一）
    /// </summary>
    public string? CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// 课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
    /// </summary>
    public string? CourseType { get; set; } = string.Empty;

    /// <summary>
    /// 课程级别（初级/中级/高级/专家）
    /// </summary>
    public string? CourseLevel { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    public string? CourseDescription { get; set; } = string.Empty;

    /// <summary>
    /// 课程目标
    /// </summary>
    public string? CourseObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 主讲讲师
    /// </summary>
    public string? MainInstructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训方式（线下/线上/混合）
    /// </summary>
    public string? TrainingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 考核方式（考试/实操/作业/无）
    /// </summary>
    public string? AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 及格分数线
    /// </summary>
    public decimal? PassingScore { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    public int? TrainingCourseStatus { get; set; }

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
// 创建TrainingCourse DTO
// ========================================

/// <summary>
/// 创建TrainingCourse DTO
/// </summary>
public class TaktTrainingCourseCreateDto
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
    /// 课程编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "课程编码（租户+公司内唯一）不能为空")]
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// 课程名称
    /// </summary>
    [Required(ErrorMessage = "课程名称不能为空")]
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
    /// </summary>
    [Required(ErrorMessage = "课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）不能为空")]
    public string CourseType { get; set; } = string.Empty;

    /// <summary>
    /// 课程级别（初级/中级/高级/专家）
    /// </summary>
    [Required(ErrorMessage = "课程级别（初级/中级/高级/专家）不能为空")]
    public string CourseLevel { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    [Required(ErrorMessage = "课程描述不能为空")]
    public string CourseDescription { get; set; } = string.Empty;

    /// <summary>
    /// 课程目标
    /// </summary>
    [Required(ErrorMessage = "课程目标不能为空")]
    public string CourseObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 主讲讲师
    /// </summary>
    [Required(ErrorMessage = "主讲讲师不能为空")]
    public string MainInstructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训方式（线下/线上/混合）
    /// </summary>
    [Required(ErrorMessage = "培训方式（线下/线上/混合）不能为空")]
    public string TrainingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 考核方式（考试/实操/作业/无）
    /// </summary>
    [Required(ErrorMessage = "考核方式（考试/实操/作业/无）不能为空")]
    public string AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 及格分数线
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    public int TrainingCourseStatus { get; set; } = 0;

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
// 更新TrainingCourse DTO
// ========================================

/// <summary>
/// 更新TrainingCourse DTO
/// 继承 TaktTrainingCourseCreateDto，添加 TrainingCourseId 字段
/// </summary>
public class TaktTrainingCourseUpdateDto : TaktTrainingCourseCreateDto
{
    /// <summary>
    /// TrainingCourseID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

}

// ========================================
// TrainingCourse 状态 DTO
// ========================================

/// <summary>
/// TrainingCourse 状态更新 DTO
/// </summary>
public class TaktTrainingCourseStatusDto
{
    /// <summary>
    /// TrainingCourseID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（1=启用 0=禁用）不能为空")]
    public int TrainingCourseStatus { get; set; } = 0;
}

// ========================================
// TrainingCourse 排序 DTO
// ========================================

/// <summary>
/// TrainingCourse 排序更新 DTO
/// </summary>
public class TaktTrainingCourseSortDto
{
    /// <summary>
    /// TrainingCourseID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TrainingCourse 导入模板行 DTO
/// </summary>
public class TaktTrainingCourseTemplateDto
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
    /// 课程编码（租户+公司内唯一）
    /// </summary>
    public string? CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// 课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
    /// </summary>
    public string? CourseType { get; set; } = string.Empty;

    /// <summary>
    /// 课程级别（初级/中级/高级/专家）
    /// </summary>
    public string? CourseLevel { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    public string? CourseDescription { get; set; } = string.Empty;

    /// <summary>
    /// 课程目标
    /// </summary>
    public string? CourseObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 主讲讲师
    /// </summary>
    public string? MainInstructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训方式（线下/线上/混合）
    /// </summary>
    public string? TrainingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 考核方式（考试/实操/作业/无）
    /// </summary>
    public string? AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 及格分数线
    /// </summary>
    public decimal? PassingScore { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    public int? TrainingCourseStatus { get; set; }

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
/// TrainingCourse 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTrainingCourseImportDto
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
    /// 课程编码（租户+公司内唯一）
    /// </summary>
    public string? CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// 课程名称
    /// </summary>
    public string? CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
    /// </summary>
    public string? CourseType { get; set; } = string.Empty;

    /// <summary>
    /// 课程级别（初级/中级/高级/专家）
    /// </summary>
    public string? CourseLevel { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    public string? CourseDescription { get; set; } = string.Empty;

    /// <summary>
    /// 课程目标
    /// </summary>
    public string? CourseObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal? TrainingHours { get; set; }

    /// <summary>
    /// 主讲讲师
    /// </summary>
    public string? MainInstructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训方式（线下/线上/混合）
    /// </summary>
    public string? TrainingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 考核方式（考试/实操/作业/无）
    /// </summary>
    public string? AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 及格分数线
    /// </summary>
    public decimal? PassingScore { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    public int? TrainingCourseStatus { get; set; }

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
/// TrainingCourse 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTrainingCourseExportDto
{
    /// <summary>
    /// TrainingCourseID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 课程编码（租户+公司内唯一）
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// 课程名称
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
    /// </summary>
    public string CourseType { get; set; } = string.Empty;

    /// <summary>
    /// 课程级别（初级/中级/高级/专家）
    /// </summary>
    public string CourseLevel { get; set; } = string.Empty;

    /// <summary>
    /// 课程描述
    /// </summary>
    public string CourseDescription { get; set; } = string.Empty;

    /// <summary>
    /// 课程目标
    /// </summary>
    public string CourseObjectives { get; set; } = string.Empty;

    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    public decimal TrainingHours { get; set; }

    /// <summary>
    /// 主讲讲师
    /// </summary>
    public string MainInstructor { get; set; } = string.Empty;

    /// <summary>
    /// 培训方式（线下/线上/混合）
    /// </summary>
    public string TrainingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 考核方式（考试/实操/作业/无）
    /// </summary>
    public string AssessmentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 及格分数线
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（1=启用 0=禁用）
    /// </summary>
    public int TrainingCourseStatus { get; set; } = 0;

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
