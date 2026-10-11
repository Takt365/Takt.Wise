// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandling.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉处理记录实体，记录客诉的处理过程和结果
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 客诉处理记录实体
/// </summary>
[SugarTable("takt_logistics_quality_customer_complaint_handling", "客诉处理记录表")]
[SugarIndex("ix_customer_complaint_handling_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_complaint_handling_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_handling_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ComplaintHandlingCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_handling_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_handling_complaint_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ComplaintId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_handling_handler_group_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(HandlerGroupCode), OrderByType.Asc, false)]
public class TaktCustomerComplaintHandling : TaktCompanyEntityBase
{

    /// <summary>
    /// 客诉处理记录编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_handling_code", ColumnDescription = "客诉处理记录编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ComplaintHandlingCode { get; set; } = string.Empty;
    /// <summary>
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_id", ColumnDescription = "客诉ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }
    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_code", ColumnDescription = "客诉单号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ComplaintCode { get; set; } = string.Empty;
    /// <summary>
    /// 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_item_id", ColumnDescription = "客诉明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ComplaintItemId { get; set; }
    /// <summary>
    /// 处理阶段（字典 logistics_quality_complaint_handling_stage）
    /// </summary>
    [SugarColumn(ColumnName = "handling_stage", ColumnDescription = "处理阶段", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingStage { get; set; } = 0;
    /// <summary>
    /// 处理方式（字典 logistics_quality_complaint_handling_method）
    /// </summary>
    [SugarColumn(ColumnName = "handling_method", ColumnDescription = "处理方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingMethod { get; set; } = 0;
    /// <summary>
    /// 处理说明
    /// </summary>
    [SugarColumn(ColumnName = "handling_description", ColumnDescription = "处理说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string HandlingDescription { get; set; } = string.Empty;
    /// <summary>
    /// 原因分析
    /// </summary>
    [SugarColumn(ColumnName = "cause_analysis", ColumnDescription = "原因分析", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? CauseAnalysis { get; set; }
    /// <summary>
    /// 改善对策/纠正措施
    /// </summary>
    [SugarColumn(ColumnName = "corrective_action", ColumnDescription = "纠正措施", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? CorrectiveAction { get; set; }
    /// <summary>
    /// 预防措施
    /// </summary>
    [SugarColumn(ColumnName = "preventive_action", ColumnDescription = "预防措施", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? PreventiveAction { get; set; }
    /// <summary>
    /// 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_dept_id", ColumnDescription = "责任部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }
    /// <summary>
    /// 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_dept_name", ColumnDescription = "责任部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ResponsibleDeptName { get; set; }
    /// <summary>
    /// 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_group_code", ColumnDescription = "责任组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? ResponsibleGroupCode { get; set; }
    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_group_name", ColumnDescription = "责任组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ResponsibleGroupName { get; set; }
    /// <summary>
    /// 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "handler_group_code", ColumnDescription = "处理组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? HandlerGroupCode { get; set; }
    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "handler_group_name", ColumnDescription = "处理组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? HandlerGroupName { get; set; }
    /// <summary>
    /// 处理时间
    /// </summary>
    [SugarColumn(ColumnName = "handling_at", ColumnDescription = "处理时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? HandlingAt { get; set; }
    /// <summary>
    /// 计划完成日期
    /// </summary>
    [SugarColumn(ColumnName = "planned_completion_date", ColumnDescription = "计划完成日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedCompletionDate { get; set; }
    /// <summary>
    /// 实际完成日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_completion_date", ColumnDescription = "实际完成日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualCompletionDate { get; set; }
    /// <summary>
    /// 处理成本/损失金额
    /// </summary>
    [SugarColumn(ColumnName = "handling_cost", ColumnDescription = "处理成本", ColumnDataType = "decimal", Length = 18, DecimalDigits =  2, IsNullable = true)]
    public decimal? HandlingCost { get; set; }
    /// <summary>
    /// 客户反馈
    /// </summary>
    [SugarColumn(ColumnName = "customer_feedback", ColumnDescription = "客户反馈", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? CustomerFeedback { get; set; }
    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    [SugarColumn(ColumnName = "customer_satisfaction", ColumnDescription = "客户满意度", ColumnDataType = "int", IsNullable = true)]
    public int? CustomerSatisfaction { get; set; }
    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FileName { get; set; }
    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? AccessUrl { get; set; }
    /// <summary>
    /// 处理状态（字典 logistics_quality_complaint_handling_status）
    /// </summary>
    [SugarColumn(ColumnName = "handling_status", ColumnDescription = "处理状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 客诉主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ComplaintId))]
    public TaktCustomerComplaint? Complaint { get; set; }
}
