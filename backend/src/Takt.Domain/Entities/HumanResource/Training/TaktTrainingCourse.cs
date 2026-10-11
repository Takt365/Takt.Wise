// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Training
// 文件名称：TaktTrainingCourse.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：培训课程实体，对应菜单 training/course
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Training;

/// <summary>
/// 培训课程定义
/// </summary>
[SugarTable("takt_human_resource_training_course", "培训课程表")]
[SugarIndex("ix_training_course_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_training_course_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_training_course_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CourseCode), OrderByType.Asc, true)]
public class TaktTrainingCourse : TaktCompanyEntityBase
{
    /// <summary>
    /// 课程编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "course_code", ColumnDescription = "课程编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string CourseCode { get; set; } = string.Empty;
    /// <summary>
    /// 课程名称
    /// </summary>
    [SugarColumn(ColumnName = "course_name", ColumnDescription = "课程名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string CourseName { get; set; } = string.Empty;
    /// <summary>
    /// 课程类型（字典 humanresource_training_course_type；列存 DictValue：ONBOARD/SKILL/MANAGEMENT/SAFETY/PROFESSIONAL）
    /// </summary>
    [SugarColumn(ColumnName = "course_type", ColumnDescription = "课程类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CourseType { get; set; } = string.Empty;
    /// <summary>
    /// 课程级别（字典 humanresource_training_course_level；列存 DictValue：BEGINNER/INTERMEDIATE/ADVANCED/EXPERT）
    /// </summary>
    [SugarColumn(ColumnName = "course_level", ColumnDescription = "课程级别", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CourseLevel { get; set; } = string.Empty;
    /// <summary>
    /// 课程描述
    /// </summary>
    [SugarColumn(ColumnName = "course_description", ColumnDescription = "课程描述", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string CourseDescription { get; set; } = string.Empty;
    /// <summary>
    /// 课程目标
    /// </summary>
    [SugarColumn(ColumnName = "course_objectives", ColumnDescription = "课程目标", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string CourseObjectives { get; set; } = string.Empty;
    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    [SugarColumn(ColumnName = "training_hours", ColumnDescription = "培训时长", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TrainingHours { get; set; } = 0m;
    /// <summary>
    /// 主讲讲师
    /// </summary>
    [SugarColumn(ColumnName = "main_instructor", ColumnDescription = "主讲讲师", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string MainInstructor { get; set; } = string.Empty;
    /// <summary>
    /// 培训方式（字典 humanresource_training_method；列存 DictValue：OFFLINE/ONLINE/HYBRID）
    /// </summary>
    [SugarColumn(ColumnName = "training_method", ColumnDescription = "培训方式", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string TrainingMethod { get; set; } = string.Empty;
    /// <summary>
    /// 考核方式（字典 humanresource_training_assessment_method；列存 DictValue：EXAM/PRACTICAL/ASSIGNMENT/NONE）
    /// </summary>
    [SugarColumn(ColumnName = "assessment_method", ColumnDescription = "考核方式", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string AssessmentMethod { get; set; } = string.Empty;
    /// <summary>
    /// 及格分数线
    /// </summary>
    [SugarColumn(ColumnName = "passing_score", ColumnDescription = "及格分数线", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PassingScore { get; set; } = 0m;
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 课程状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "training_course_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TrainingCourseStatus { get; set; } = 1;
}
