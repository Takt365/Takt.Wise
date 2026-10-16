// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktIpqcDefectHandling.cs
// 功能描述：IPQC制程检验不良处理记录实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// IPQC制程检验不良处理记录实体
/// </summary>
[SugarTable("takt_logistics_quality_ipqc_defect_handling", "制程检验不良处理记录表")]
[SugarIndex("ix_ipqc_defect_handling_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ipqc_defect_handling_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_ipqc_defect_handling_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IpqcOrderItemId), OrderByType.Asc, nameof(DefectCode), OrderByType.Asc, nameof(HandlingMethod), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_ipqc_defect_handling_defect_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DefectType), OrderByType.Asc, false)]
public class TaktIpqcDefectHandling : TaktCompanyEntityBase
{    /// <summary>
    /// IPQC不良处理编码（前端表单选择编码规则后自动通过 TaktNumbering IPQC不良处理编码规则生成并展示，非手输；单据类型菜单：制程检验）
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_defect_handling_code", ColumnDescription = "IPQC不良处理编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string IpqcDefectHandlingCode { get; set; } = string.Empty;
    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_order_item_id", ColumnDescription = "IPQC检验单明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderItemId { get; set; }
    /// <summary>
    /// IPQC检验单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_order_code", ColumnDescription = "IPQC检验单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string IpqcOrderCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    [SugarColumn(ColumnName = "defect_type", ColumnDescription = "不良类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DefectType { get; set; } = 0;
    /// <summary>
    /// 不良现象编码
    /// </summary>
    [SugarColumn(ColumnName = "defect_code", ColumnDescription = "不良现象编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string DefectCode { get; set; } = string.Empty;
    /// <summary>
    /// 不良现象描述
    /// </summary>
    [SugarColumn(ColumnName = "defect_description", ColumnDescription = "不良现象描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string DefectDescription { get; set; } = string.Empty;
    /// <summary>
    /// 不良数量
    /// </summary>
    [SugarColumn(ColumnName = "defect_quantity", ColumnDescription = "不良数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DefectQuantity { get; set; } = 0;
    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    [SugarColumn(ColumnName = "handling_method", ColumnDescription = "处理方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingMethod { get; set; } = 0;
    /// <summary>
    /// 处理说明
    /// </summary>
    [SugarColumn(ColumnName = "handling_description", ColumnDescription = "处理说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? HandlingDescription { get; set; }
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
    /// 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_group_code", ColumnDescription = "责任组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? ResponsibleGroupCode { get; set; }
    /// <summary>
    /// 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_group_name", ColumnDescription = "责任组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ResponsibleGroupName { get; set; }
    /// <summary>
    /// 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "handler_group_code", ColumnDescription = "处理组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? HandlerGroupCode { get; set; }
    /// <summary>
    /// 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "handler_group_name", ColumnDescription = "处理组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? HandlerGroupName { get; set; }
    /// <summary>
    /// 处理时间
    /// </summary>
    [SugarColumn(ColumnName = "handling_at", ColumnDescription = "处理时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? HandlingAt { get; set; }
    /// <summary>
    /// 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "verify_dept_id", ColumnDescription = "验证部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? VerifyDeptId { get; set; }
    /// <summary>
    /// 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "verify_dept_name", ColumnDescription = "验证部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? VerifyDeptName { get; set; }
    /// <summary>
    /// 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "quality_owner_group_code", ColumnDescription = "质量负责人组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? QualityOwnerGroupCode { get; set; }
    /// <summary>
    /// 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "quality_owner_group_name", ColumnDescription = "质量负责人组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? QualityOwnerGroupName { get; set; }
    /// <summary>
    /// 验证时间
    /// </summary>
    [SugarColumn(ColumnName = "verify_at", ColumnDescription = "验证时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? VerifyAt { get; set; }
    /// <summary>
    /// 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
    /// </summary>
    [SugarColumn(ColumnName = "verify_status", ColumnDescription = "验证结果", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VerifyStatus { get; set; } = 0;
    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    [SugarColumn(ColumnName = "corrective_action", ColumnDescription = "纠正措施", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? CorrectiveAction { get; set; }
    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    [SugarColumn(ColumnName = "defect_images", ColumnDescription = "不良图片", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? DefectImages { get; set; }
    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    [SugarColumn(ColumnName = "handling_status", ColumnDescription = "处理状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// IPQC检验单明细（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(IpqcOrderItemId))]
    public TaktIpqcOrderItem? OrderItem { get; set; }
}
