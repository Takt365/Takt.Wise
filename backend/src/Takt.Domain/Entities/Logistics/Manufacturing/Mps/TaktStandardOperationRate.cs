// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktStandardOperationRate.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率实体，定义工厂/年度/稼动率类型下的标准稼动率
// 计算公式：OperationRate 为标准对标目标值（人工维护，非本表字段推导）；按 租户 + 公司 + 工厂 + 财务年度 + 稼动率类型 唯一维护
// 对比参考：达成率(%) = 实际稼动率 ÷ 标准稼动率 × 100%（实际值取自人员/设备稼动率实绩）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 标准生产稼动率实体
/// OperationRate 为标准对标目标值；对比参考：达成率(%) = 实际稼动率 ÷ 标准稼动率 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_standard_operation_rate", "标准生产稼动率表")]
[SugarIndex("ix_standard_operation_rate_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_standard_operation_rate_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_standard_operation_rate_sor_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(FinancialYear), OrderByType.Asc, nameof(OperationType), OrderByType.Asc, true)]
public class TaktStandardOperationRate : TaktCompanyEntityBase
{

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
    /// </summary>
    [SugarColumn(ColumnName = "financial_year", ColumnDescription = "财务年度", ColumnDataType = "nvarchar", Length = 6, IsNullable = false)]
    public string FinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "operation_type", ColumnDescription = "稼动率类型", ColumnDataType = "int", IsNullable = false)]
    public int OperationType { get; set; } = 1;

    /// <summary>
    /// 稼动率（比例，如 0.85 表示 85%）
    /// </summary>
    [SugarColumn(ColumnName = "operation_rate", ColumnDescription = "稼动率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OperationRate { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    [SugarColumn(ColumnName = "rate_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RateStatus { get; set; } = 1;
}
