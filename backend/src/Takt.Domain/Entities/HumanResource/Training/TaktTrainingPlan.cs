// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Training
// 文件名称：TaktTrainingPlan.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：培训计划实体，对应菜单 training/plan
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Training;

/// <summary>
/// 培训计划（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// </summary>
[SugarTable("takt_human_resource_training_plan", "培训计划表")]
[SugarIndex("ix_training_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_training_plan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_training_plan_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlanCode), OrderByType.Asc, true)]
public class TaktTrainingPlan : TaktApprovalEntityBase
{
    /// <summary>
    /// 计划编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "plan_code", ColumnDescription = "计划编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlanCode { get; set; } = string.Empty;
    /// <summary>
    /// 计划名称
    /// </summary>
    [SugarColumn(ColumnName = "plan_name", ColumnDescription = "计划名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlanName { get; set; } = string.Empty;
    /// <summary>
    /// 计划年度
    /// </summary>
    [SugarColumn(ColumnName = "plan_year", ColumnDescription = "计划年度", ColumnDataType = "int", IsNullable = false)]
    public int PlanYear { get; set; } = 0;
    /// <summary>
    /// 计划类型（字典 humanresource_training_plan_type；列存 DictValue：YEAR/QUARTER/MONTH/SPECIAL）
    /// </summary>
    [SugarColumn(ColumnName = "plan_type", ColumnDescription = "计划类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PlanType { get; set; } = string.Empty;
    /// <summary>
    /// 适用部门
    /// </summary>
    [SugarColumn(ColumnName = "applicable_department", ColumnDescription = "适用部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ApplicableDepartment { get; set; } = string.Empty;
    /// <summary>
    /// 计划开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "计划开始日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 计划结束日期
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "计划结束日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 培训目标
    /// </summary>
    [SugarColumn(ColumnName = "training_objectives", ColumnDescription = "培训目标", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string TrainingObjectives { get; set; } = string.Empty;
    /// <summary>
    /// 计划培训人数
    /// </summary>
    [SugarColumn(ColumnName = "planned_headcount", ColumnDescription = "计划培训人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlannedHeadcount { get; set; } = 0;
    /// <summary>
    /// 培训预算（元）
    /// </summary>
    [SugarColumn(ColumnName = "training_budget", ColumnDescription = "培训预算", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TrainingBudget { get; set; } = 0m;
    /// <summary>
    /// 计划说明
    /// </summary>
    [SugarColumn(ColumnName = "training_plan_description", ColumnDescription = "计划说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string TrainingPlanDescription { get; set; } = string.Empty;
    /// <summary>
    /// 计划业务状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "training_plan_status", ColumnDescription = "业务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TrainingPlanStatus { get; set; } = 1;
}
