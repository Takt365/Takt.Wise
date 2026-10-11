// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修明细实体，记录板别、生产实绩、不良症状、检出工程、责任归属等
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA改修明细实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_pcba_repair_detail", "PCBA改修明细表")]
[SugarIndex("ix_pcba_repair_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_repair_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PcbaRepairId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktPcbaRepairDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_repair_id", ColumnDescription = "PCBA改修ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaRepairId { get; set; }
    
    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;
    
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA板别（字典 logistics_manufacturing_pcba_function；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_board_type", ColumnDescription = "PCBA板别", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? PcbaBoardType { get; set; }

    /// <summary>
    /// 生产实绩
    /// </summary>
    [SugarColumn(ColumnName = "prod_actual_qty", ColumnDescription = "生产实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdActualQty { get; set; } = 0;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", Length = 8, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? TeamCode { get; set; }

    /// <summary>
    /// 卡号
    /// </summary>
    [SugarColumn(ColumnName = "card_code", ColumnDescription = "卡号", Length = 50, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? CardCode { get; set; }

    /// <summary>
    /// 不良症状
    /// </summary>
    [SugarColumn(ColumnName = "defect_symptom", ColumnDescription = "不良症状", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectSymptom { get; set; }

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category；存 DictValue，与组立不良区分共用）
    /// </summary>
    [SugarColumn(ColumnName = "defect_engineering", ColumnDescription = "检出工程", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectEngineering { get; set; }

    /// <summary>
    /// 不良原因
    /// </summary>
    [SugarColumn(ColumnName = "defect_reason", ColumnDescription = "不良原因", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectReason { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    [SugarColumn(ColumnName = "defect_qty", ColumnDescription = "不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal DefectQty { get; set; } = 0;

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "defect_responsibility", ColumnDescription = "责任归属", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectResponsibility { get; set; }

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "defect_nature", ColumnDescription = "不良性质", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectNature { get; set; }

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
    /// PCBA改修日报（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PcbaRepairId))]
    public TaktPcbaRepair? PcbaRepair { get; set; }
}
