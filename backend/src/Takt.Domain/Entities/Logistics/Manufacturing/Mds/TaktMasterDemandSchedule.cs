// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandSchedule.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主需求计划 MDS 头表，汇总销售订单与预测需求（Sales Order / Forecast）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mds;

/// <summary>
/// 主需求计划 MDS 头表（公司级；承接销售订单与预测，下推 MPS）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mds_master_demand_schedule", "主需求计划MDS头表")]
[SugarIndex("ix_mds_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mds_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_master_demand_schedule_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MdsCode), OrderByType.Asc, true)]
public class TaktMasterDemandSchedule : TaktApprovalEntityBase
{

    /// <summary>
    /// MDS 编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering MDS编码规则生成并展示，非手输；单据类型菜单：主需求计划）
    /// </summary>
    [SugarColumn(ColumnName = "mds_code", ColumnDescription = "MDS编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MdsCode { get; set; } = string.Empty;

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
    /// MDS 明细行（按物料与时间桶）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMasterDemandScheduleLine.MasterDemandScheduleId))]
    public List<TaktMasterDemandScheduleLine>? Lines { get; set; }
}
