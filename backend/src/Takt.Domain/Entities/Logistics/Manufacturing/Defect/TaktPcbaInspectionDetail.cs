// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查明细实体，记录板别、目视/AOI线别、实装日期、检查员、检查数量等
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查明细实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_pcba_inspection_detail", "PCBA检查明细表")]
[SugarIndex("ix_pcba_inspection_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_inspection_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PcbaInspectionId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktPcbaInspectionDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_inspection_id", ColumnDescription = "PCBA检查ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaInspectionId { get; set; }
    
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
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "visual_inspection_line", ColumnDescription = "目视线别", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? VisualInspectionLine { get; set; }

    /// <summary>
    /// AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "aoi_line", ColumnDescription = "AOI线别", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? AoiLine { get; set; }

    /// <summary>
    /// B面实装日期
    /// </summary>
    [SugarColumn(ColumnName = "b_side_assembly_date", ColumnDescription = "B面实装日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T面实装日期
    /// </summary>
    [SugarColumn(ColumnName = "t_side_assembly_date", ColumnDescription = "T面实装日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 检查员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inspector_id", ColumnDescription = "检查员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InspectorId { get; set; }
    /// <summary>
    /// 检查员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "inspector_name", ColumnDescription = "检查员名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? InspectorName { get; set; }

    /// <summary>
    /// 当日完成数量
    /// </summary>
    [SugarColumn(ColumnName = "daily_completed_qty", ColumnDescription = "当日完成数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal DailyCompletedQty { get; set; } = 0;

    /// <summary>
    /// 检查数量
    /// </summary>
    [SugarColumn(ColumnName = "inspection_qty", ColumnDescription = "检查数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal InspectionQty { get; set; } = 0;

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_status", ColumnDescription = "检查状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InspectionStatus { get; set; } = 1;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", Length = 8, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? TeamCode { get; set; }

    /// <summary>
    /// 检查工数
    /// </summary>
    [SugarColumn(ColumnName = "inspection_work_hours", ColumnDescription = "检查工数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal InspectionWorkHours { get; set; } = 0;

    /// <summary>
    /// AOI工数
    /// </summary>
    [SugarColumn(ColumnName = "aoi_work_hours", ColumnDescription = "AOI工数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AoiWorkHours { get; set; } = 0;

    /// <summary>
    /// 不良数量
    /// </summary>
    [SugarColumn(ColumnName = "defect_qty", ColumnDescription = "不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal DefectQty { get; set; } = 0;

    /// <summary>
    /// 手贴
    /// </summary>
    [SugarColumn(ColumnName = "hand_placement", ColumnDescription = "手贴", Length = 100, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? HandPlacement { get; set; }

    /// <summary>
    /// 流水号
    /// </summary>
    [SugarColumn(ColumnName = "serial_number", ColumnDescription = "流水号", Length = 50, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [SugarColumn(ColumnName = "content", ColumnDescription = "内容", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? Content { get; set; }

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "defect_location", ColumnDescription = "不良个所", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DefectLocation { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// PCBA检查日报（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PcbaInspectionId))]
    public TaktPcbaInspection? PcbaInspection { get; set; }
}
