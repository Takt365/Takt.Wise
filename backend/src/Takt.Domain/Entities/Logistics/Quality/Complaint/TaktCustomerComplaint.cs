// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaint.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉主表实体，记录客户投诉基本信息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 客诉主表实体
/// </summary>
[SugarTable("takt_logistics_quality_customer_complaint", "客诉主表")]
[SugarIndex("ix_customer_complaint_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_complaint_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_complaint_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(CustomerComplaintCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_related_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_complaint_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ComplaintStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_complaint_customer_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerId), OrderByType.Asc, false)]
public class TaktCustomerComplaint : TaktCompanyEntityBase
{
    /// <summary>
    /// 客诉单号（组合唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "customer_complaint_code", ColumnDescription = "客诉单号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string CustomerComplaintCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "customer_id", ColumnDescription = "客户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }
    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "客户名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string CustomerName1 { get; set; } = string.Empty;
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 投诉日期
    /// </summary>
    [SugarColumn(ColumnName = "complaint_date", ColumnDescription = "投诉日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ComplaintDate { get; set; } = DateTime.Today;
    /// <summary>
    /// 投诉方式（字典 logistics_quality_complaint_method；0=电话，1=邮件，2=传真，3=现场，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_method", ColumnDescription = "投诉方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ComplaintMethod { get; set; } = 0;
    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_type", ColumnDescription = "投诉类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ComplaintType { get; set; } = 0;
    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_level", ColumnDescription = "投诉等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ComplaintLevel { get; set; } = 0;
    /// <summary>
    /// 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_dept_id", ColumnDescription = "责任部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleDeptId { get; set; }
    /// <summary>
    /// 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_dept_name", ColumnDescription = "责任部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
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
    /// 要求回复日期
    /// </summary>
    [SugarColumn(ColumnName = "required_reply_date", ColumnDescription = "要求回复日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RequiredReplyDate { get; set; }
    /// <summary>
    /// 实际回复日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_reply_date", ColumnDescription = "实际回复日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualReplyDate { get; set; }
    /// <summary>
    /// 客诉描述
    /// </summary>
    [SugarColumn(ColumnName = "complaint_description", ColumnDescription = "客诉描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string ComplaintDescription { get; set; } = string.Empty;
    /// <summary>
    /// 处理结果/回复内容
    /// </summary>
    [SugarColumn(ColumnName = "handling_result", ColumnDescription = "处理结果", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? HandlingResult { get; set; }
    /// <summary>
    /// 客户满意度（字典 logistics_quality_customer_satisfaction）
    /// </summary>
    [SugarColumn(ColumnName = "customer_satisfaction", ColumnDescription = "客户满意度", ColumnDataType = "int", IsNullable = true)]
    public int? CustomerSatisfaction { get; set; }
    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 客诉状态（字典 logistics_quality_complaint_status）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_status", ColumnDescription = "客诉状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ComplaintStatus { get; set; } = 0;

    /// <summary>
    /// 客诉明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerComplaintItem.ComplaintId))]
    public List<TaktCustomerComplaintItem>? Items { get; set; }
}
