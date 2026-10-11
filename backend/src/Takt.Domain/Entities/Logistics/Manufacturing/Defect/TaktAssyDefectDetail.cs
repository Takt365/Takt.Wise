// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良明细实体，记录不良区分、数量、随机卡号、发生工程、测试步骤、症状、个所、原因、修理员等
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立不良明细实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_assy_detail", "组立不良明细表")]
[SugarIndex("ix_assy_defect_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_assy_defect_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AssyDefectId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktAssyDefectDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "assy_defect_id", ColumnDescription = "组立不良ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssyDefectId { get; set; }
    
    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩（冗余字段,便于统计/查询）
    /// </summary>
    [SugarColumn(ColumnName = "prod_actual_qty", ColumnDescription = "生实实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdActualQty { get; set; } = 0;

    /// <summary>
    /// 无不良数量（冗余字段,便于统计/查询）
    /// </summary>
    [SugarColumn(ColumnName = "good_quantity", ColumnDescription = "无不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal GoodQuantity { get; set; } = 0;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 不良区分（字典 logistics_manufacturing_defect_category；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "defect_category", ColumnDescription = "不良区分", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectCategory { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    [SugarColumn(ColumnName = "defect_qty", ColumnDescription = "不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal DefectQty { get; set; } = 0;

    /// <summary>
    /// 累计不良
    /// </summary>
    [SugarColumn(ColumnName = "cumulative_defect_qty", ColumnDescription = "累计不良", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal CumulativeDefectQty { get; set; } = 0;

    /// <summary>
    /// 随机卡号
    /// </summary>
    [SugarColumn(ColumnName = "random_card_code", ColumnDescription = "随机卡号", Length = 50, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? RandomCardCode { get; set; }

    /// <summary>
    /// 发生工程
    /// </summary>
    [SugarColumn(ColumnName = "occurrence_engineering", ColumnDescription = "发生工程", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? OccurrenceEngineering { get; set; }

    /// <summary>
    /// 测试步骤
    /// </summary>
    [SugarColumn(ColumnName = "test_step", ColumnDescription = "测试步骤", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? TestStep { get; set; }

    /// <summary>
    /// 不良症状
    /// </summary>
    [SugarColumn(ColumnName = "defect_symptom", ColumnDescription = "不良症状", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectSymptom { get; set; }

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_assy_location_category；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "defect_location", ColumnDescription = "不良个所", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectLocation { get; set; }

    /// <summary>
    /// 不良原因
    /// </summary>
    [SugarColumn(ColumnName = "defect_reason", ColumnDescription = "不良原因", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectReason { get; set; }

    /// <summary>
    /// 修理员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "repair_operator_id", ColumnDescription = "修理员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RepairOperatorId { get; set; }
    /// <summary>
    /// 修理员名称（冗余：按 RepairOperatorId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "repair_operator_name", ColumnDescription = "修理员名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RepairOperatorName { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 组立不良日报（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(AssyDefectId))]
    public TaktAssyDefect? AssyDefect { get; set; }
}
