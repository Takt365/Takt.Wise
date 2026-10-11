// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanning.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划 MRP 头表，承接 MPS 下推；展开 BOM/库存/在途，产出自制计划订单与采购计划
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mrp;

/// <summary>
/// 物料需求计划 MRP 头表（公司级；MPS 下推，产出 TaktPlannedOrder / TaktProductionPlan / TaktPurchasePlan）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mrp_material_requirements_planning", "物料需求计划MRP头表")]
[SugarIndex("ix_mrp_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mrp_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialRequirementsPlanningCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_material_requirements_planning_mps", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterProductionScheduleId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_material_requirements_planning_mds", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterDemandScheduleId), OrderByType.Asc, false)]
public class TaktMaterialRequirementsPlanning : TaktApprovalEntityBase
{

    /// <summary>
    /// MRP 编码（租户+公司+工厂内业务唯一）
    /// </summary>
    [SugarColumn(ColumnName = "material_requirements_planning_code", ColumnDescription = "MRP编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
    /// </summary>
    [SugarColumn(ColumnName = "master_production_schedule_id", ColumnDescription = "来源MPS头表ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 来源 MPS 编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "mps_code", ColumnDescription = "来源MPS编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MpsCode { get; set; }

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层追溯，可选）
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
    /// 计划编制日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_date", ColumnDescription = "计划编制日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_period_start", ColumnDescription = "计划周期开始日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanPeriodStart { get; set; } = DateTime.Now;

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_period_end", ColumnDescription = "计划周期结束日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanPeriodEnd { get; set; } = DateTime.Now;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "planner_group_code", ColumnDescription = "计划组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "planner_group_name", ColumnDescription = "计划组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? PlannerGroupName { get; set; }

    /// <summary>
    /// 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
    /// </summary>
    [SugarColumn(ColumnName = "run_status", ColumnDescription = "运算状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RunStatus { get; set; } = 0;

    /// <summary>
    /// 产出生产计划 ID（运算完成后回写）
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_id", ColumnDescription = "产出生产计划ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 产出生产计划编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_code", ColumnDescription = "产出生产计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ProductionPlanCode { get; set; }

    /// <summary>
    /// 产出采购计划 ID（运算完成后回写）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_id", ColumnDescription = "产出采购计划ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePlanId { get; set; }

    /// <summary>
    /// 产出采购计划编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_code", ColumnDescription = "产出采购计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PurchasePlanCode { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    [SugarColumn(ColumnName = "plan_description", ColumnDescription = "计划说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? PlanDescription { get; set; }

    /// <summary>
    /// MRP 需求明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaterialRequirementsPlanningItem.MaterialRequirementsPlanningId))]
    public List<TaktMaterialRequirementsPlanningItem>? Items { get; set; }
}
