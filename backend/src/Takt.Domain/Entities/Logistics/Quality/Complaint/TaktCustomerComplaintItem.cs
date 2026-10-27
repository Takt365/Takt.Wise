// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItem.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉明细实体，记录客诉的不良项目详情
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 客诉明细实体
/// </summary>
[SugarTable("takt_logistics_quality_customer_complaint_item", "客诉明细表")]
[SugarIndex("ix_customer_complaint_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_complaint_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ComplaintId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_item_complaint_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ComplaintId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_item_customer_complaint_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerComplaintCode), OrderByType.Asc, false)]
public class TaktCustomerComplaintItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_id", ColumnDescription = "客诉ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ComplaintId { get; set; }
    /// <summary>
    /// 客诉单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "customer_complaint_code", ColumnDescription = "客诉单号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string CustomerComplaintCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "product_code", ColumnDescription = "产品编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ProductCode { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    [SugarColumn(ColumnName = "product_name", ColumnDescription = "产品名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ProductName { get; set; }
    /// <summary>
    /// 批次号
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次号", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? BatchCode { get; set; }
    /// <summary>
    /// 不良项目类型（字典 logistics_quality_complaint_item_type）
    /// </summary>
    [SugarColumn(ColumnName = "item_type", ColumnDescription = "不良项目类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ItemType { get; set; } = 0;
    /// <summary>
    /// 不良现象描述
    /// </summary>
    [SugarColumn(ColumnName = "defect_description", ColumnDescription = "不良现象描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string DefectDescription { get; set; } = string.Empty;
    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
    /// </summary>
    [SugarColumn(ColumnName = "defect_level", ColumnDescription = "缺点等级", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string DefectLevel { get; set; } = string.Empty;
    /// <summary>
    /// 不良数量
    /// </summary>
    [SugarColumn(ColumnName = "defect_quantity", ColumnDescription = "不良数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DefectQuantity { get; set; } = 0;
    /// <summary>
    /// 不良率（%）
    /// </summary>
    [SugarColumn(ColumnName = "defect_rate", ColumnDescription = "不良率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = true)]
    public decimal? DefectRate { get; set; }
    /// <summary>
    /// 原因分析
    /// </summary>
    [SugarColumn(ColumnName = "cause_analysis", ColumnDescription = "原因分析", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? CauseAnalysis { get; set; }
    /// <summary>
    /// 改善对策
    /// </summary>
    [SugarColumn(ColumnName = "improvement_action", ColumnDescription = "改善对策", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ImprovementAction { get; set; }
    /// <summary>
    /// 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "improvement_responsible_group_code", ColumnDescription = "改善组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? ImprovementResponsibleGroupCode { get; set; }
    /// <summary>
    /// 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "improvement_responsible_group_name", ColumnDescription = "改善组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ImprovementResponsibleGroupName { get; set; }
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
    /// 改善状态（字典 logistics_quality_improvement_status）
    /// </summary>
    [SugarColumn(ColumnName = "improvement_status", ColumnDescription = "改善状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ImprovementStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 客诉主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ComplaintId))]
    public TaktCustomerComplaint? Complaint { get; set; }
}
