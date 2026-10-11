#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecScan.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 物料扫码防错记录（BOM 比对，NG 禁止下一步）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 物料扫码记录实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_exec_scan", "SOP物料扫码记录表")]
[SugarIndex("ix_sop_exec_scan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_exec_scan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_exec_scan_exec", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecId), OrderByType.Asc, false)]
public class TaktSopExecScan : TaktCompanyEntityBase
{
    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_id", ColumnDescription = "执行追溯ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_step_id", ColumnDescription = "工步执行明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "step_id", ColumnDescription = "工步ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 扫描条码
    /// </summary>
    [SugarColumn(ColumnName = "scanned_barcode", ColumnDescription = "扫描条码", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ScannedBarcode { get; set; } = string.Empty;

    /// <summary>
    /// 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "expected_material_code", ColumnDescription = "期望物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ExpectedMaterialCode { get; set; }

    /// <summary>
    /// 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
    /// </summary>
    [SugarColumn(ColumnName = "scan_result", ColumnDescription = "扫码结果", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ScanResult { get; set; } = 1;

    /// <summary>
    /// 比对说明
    /// </summary>
    [SugarColumn(ColumnName = "match_message", ColumnDescription = "比对说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? MatchMessage { get; set; }

    /// <summary>
    /// 扫描时间
    /// </summary>
    [SugarColumn(ColumnName = "scanned_at", ColumnDescription = "扫描时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ScannedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 执行追溯
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ExecId))]
    public TaktSopExec? Exec { get; set; }
}
