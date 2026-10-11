// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionSchedule.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划 MPS 头表，承接 MDS 下推；成品级排产与粗产能校验
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 主生产计划 MPS 头表（公司级；MDS 下推，成品级何时做多少、粗产能校验）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_master_production_schedule", "主生产计划MPS头表")]
[SugarIndex("ix_mps_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mps_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_master_production_schedule_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MpsCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_master_production_schedule_mds", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterDemandScheduleId), OrderByType.Asc, false)]
public class TaktMasterProductionSchedule : TaktApprovalEntityBase
{

    /// <summary>
    /// MPS 编码
    /// </summary>
    [SugarColumn(ColumnName = "mps_code", ColumnDescription = "MPS编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [SugarColumn(ColumnName = "master_demand_schedule_id", ColumnDescription = "来源MDS头表ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "mds_code", ColumnDescription = "来源MDS编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MdsCode { get; set; }

    /// <summary>
    /// 计划周期开始
    /// </summary>
    [SugarColumn(ColumnName = "plan_period_start", ColumnDescription = "计划周期开始", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束
    /// </summary>
    [SugarColumn(ColumnName = "plan_period_end", ColumnDescription = "计划周期结束", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    [SugarColumn(ColumnName = "bucket_type", ColumnDescription = "时间桶粒度", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BucketType { get; set; } = 1;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_status", ColumnDescription = "计划状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ScheduleStatus { get; set; } = 1;

    /// <summary>
    /// MPS 明细行
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMasterProductionScheduleLine.MasterProductionScheduleId))]
    public List<TaktMasterProductionScheduleLine>? Lines { get; set; }
}
