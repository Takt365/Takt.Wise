// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktFqcOrderItem.cs
// 功能描述：FQC出货检验单明细实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// FQC出货检验单明细实体
/// </summary>
[SugarTable("takt_logistics_quality_fqc_order_item", "出货检验单明细表")]
[SugarIndex("ix_fqc_order_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_fqc_order_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_fqc_order_item_order_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FqcOrderId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_fqc_order_item_fqc_order_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FqcOrderId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_fqc_order_item_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktFqcOrderItem : TaktCompanyEntityBase
{    /// <summary>
    /// FQC检验单 ID（选项 TaktFqcOrders/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "fqc_order_id", ColumnDescription = "FQC检验单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }
    /// <summary>
    /// FQC检验单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "fqc_order_code", ColumnDescription = "FQC检验单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string FqcOrderCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;
    /// <summary>
    /// 批次号
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? BatchCode { get; set; }
    /// <summary>
    /// 入库数量
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_quantity", ColumnDescription = "入库数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal WarehouseQuantity { get; set; } = 0;
    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
    /// </summary>
    [SugarColumn(ColumnName = "standard_code", ColumnDescription = "检验标准编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string StandardCode { get; set; } = string.Empty;
    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_code", ColumnDescription = "抽样方案编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SamplingSchemeCode { get; set; } = string.Empty;
    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_method", ColumnDescription = "检验方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int InspectionMethod { get; set; } = 2;
    /// <summary>
    /// 抽样数量
    /// </summary>
    [SugarColumn(ColumnName = "sample_quantity", ColumnDescription = "抽样数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SampleQuantity { get; set; } = 0;
    /// <summary>
    /// 合格数量
    /// </summary>
    [SugarColumn(ColumnName = "qualified_quantity", ColumnDescription = "合格数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QualifiedQuantity { get; set; } = 0;
    /// <summary>
    /// 不合格数量
    /// </summary>
    [SugarColumn(ColumnName = "unqualified_quantity", ColumnDescription = "不合格数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int UnqualifiedQuantity { get; set; } = 0;
    /// <summary>
    /// 验退数量
    /// </summary>
    [SugarColumn(ColumnName = "inspection_return_quantity", ColumnDescription = "验退数量", ColumnDataType = "decimal", Length = 16, DecimalDigits = 6, IsNullable = false, DefaultValue = "0")]
    public decimal InspectionReturnQuantity { get; set; } = 0;
    /// <summary>
    /// 抽检序列号
    /// </summary>
    [SugarColumn(ColumnName = "sample_serial_code", ColumnDescription = "抽检序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SampleSerialCode { get; set; }
    /// <summary>
    /// 检验说明
    /// </summary>
    [SugarColumn(ColumnName = "inspection_description", ColumnDescription = "检验说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? InspectionDescription { get; set; }
    /// <summary>
    /// 检验员（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inspector_id", ColumnDescription = "检验员ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InspectorId { get; set; }
    /// <summary>
    /// 检验员名称（冗余：按 InspectorId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "inspector_name", ColumnDescription = "检验员名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? InspectorName { get; set; }
    /// <summary>
    /// 检验日期
    /// </summary>
    [SugarColumn(ColumnName = "inspection_date", ColumnDescription = "检验日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime InspectionDate { get; set; }
    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    [SugarColumn(ColumnName = "judge_status", ColumnDescription = "判定状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// FQC检验单（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(FqcOrderId))]
    public TaktFqcOrder? Order { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFqcDefectHandling.FqcOrderItemId))]
    public List<TaktFqcDefectHandling>? DefectHandlings { get; set; }
}
