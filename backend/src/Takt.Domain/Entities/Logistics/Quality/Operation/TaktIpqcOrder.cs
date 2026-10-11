// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktIpqcOrder.cs
// 功能描述：IPQC制程检验单实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// IPQC制程检验单实体
/// </summary>
[SugarTable("takt_logistics_quality_ipqc_order", "制程检验单表")]
[SugarIndex("ix_ipqc_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ipqc_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_ipqc_order_ipqc_order_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(IpqcOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_ipqc_order_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_ipqc_order_process_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProcessCode), OrderByType.Asc, false)]
public class TaktIpqcOrder : TaktCompanyEntityBase
{
    /// <summary>
    /// 来源单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "source_code", ColumnDescription = "来源单号", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SourceCode { get; set; } = string.Empty;
    /// <summary>
    /// 检验日期
    /// </summary>
    [SugarColumn(ColumnName = "inspection_date", ColumnDescription = "检验日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? InspectionDate { get; set; }
    /// <summary>
    /// IPQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_order_code", ColumnDescription = "IPQC检验单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string IpqcOrderCode { get; set; } = string.Empty;
    /// <summary>
    /// 工序编码
    /// </summary>
    [SugarColumn(ColumnName = "process_code", ColumnDescription = "工序编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProcessCode { get; set; } = string.Empty;
    /// <summary>
    /// 工序名称
    /// </summary>
    [SugarColumn(ColumnName = "process_name", ColumnDescription = "工序名称", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string ProcessName { get; set; } = string.Empty;
    /// <summary>
    /// 生产总数
    /// </summary>
    [SugarColumn(ColumnName = "total_production_quantity", ColumnDescription = "生产总数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalProductionQuantity { get; set; } = 0;
    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_sample_quantity", ColumnDescription = "总抽样数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalSampleQuantity { get; set; } = 0;
    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_qualified_quantity", ColumnDescription = "总合格数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalQualifiedQuantity { get; set; } = 0;
    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_unqualified_quantity", ColumnDescription = "总不合格数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalUnqualifiedQuantity { get; set; } = 0;
    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_inspection_return_quantity", ColumnDescription = "总验退数量", ColumnDataType = "decimal", Length = 16, DecimalDigits = 6, IsNullable = false, DefaultValue = "0")]
    public decimal TotalInspectionReturnQuantity { get; set; } = 0;
    /// <summary>
    /// 判定人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "judge_by_employee_id", ColumnDescription = "判定人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? JudgeByEmployeeId { get; set; }
    /// <summary>
    /// 判定人名称（冗余：按 JudgeByEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "judge_by_employee_name", ColumnDescription = "判定人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? JudgeByEmployeeName { get; set; }
    /// <summary>
    /// 判定日期
    /// </summary>
    [SugarColumn(ColumnName = "judge_date", ColumnDescription = "判定日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? JudgeDate { get; set; }
    /// <summary>
    /// 判定说明
    /// </summary>
    [SugarColumn(ColumnName = "judge_description", ColumnDescription = "判定说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? JudgeDescription { get; set; }
    /// <summary>
    /// 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
    /// </summary>
    [SugarColumn(ColumnName = "judge_status", ColumnDescription = "判定状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// IPQC检验单明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktIpqcOrderItem.IpqcOrderId))]
    public List<TaktIpqcOrderItem>? Items { get; set; }
}
