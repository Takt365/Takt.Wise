// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktApsSchedule.cs
// 功能描述：APS排程主表，基于 MRP 计划订单做周/日/班次、设备级排程
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程主表（高级计划与排程）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_schedule", "APS排程主表")]
[SugarIndex("ix_aps_schedule_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_aps_schedule_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ScheduleCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_plan_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_mrp", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialRequirementsPlanningId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ScheduleStatus), OrderByType.Asc, false)]
public class TaktApsSchedule : TaktCompanyEntityBase
{

    /// <summary>
    /// 来源 MRP 头表 ID（Planning 层上游，关联 TaktMaterialRequirementsPlanning.Id）
    /// </summary>
    [SugarColumn(ColumnName = "material_requirements_planning_id", ColumnDescription = "来源MRP头表ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "material_requirements_planning_code", ColumnDescription = "来源MRP编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? MaterialRequirementsPlanningCode { get; set; }

    /// <summary>
    /// 排程编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 排程编码规则生成并展示，非手输；单据类型菜单：高级排程）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_code", ColumnDescription = "排程编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// 排程名称
    /// </summary>
    [SugarColumn(ColumnName = "schedule_name", ColumnDescription = "排程名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// 排程类型（0=主生产计划，1=车间作业计划，2=紧急插单，3=计划调整）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_type", ColumnDescription = "排程类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ScheduleType { get; set; } = 0;

    /// <summary>
    /// 计划日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_date", ColumnDescription = "计划日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [SugarColumn(ColumnName = "plan_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [SugarColumn(ColumnName = "plan_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 计划周期（0=日计划，1=周计划，2=月计划）
    /// </summary>
    [SugarColumn(ColumnName = "plan_cycle", ColumnDescription = "计划周期", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlanCycle { get; set; } = 0;

    /// <summary>
    /// 车间编码
    /// </summary>
    [SugarColumn(ColumnName = "workshop_code", ColumnDescription = "车间编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? WorkshopCode { get; set; }

    /// <summary>
    /// 车间名称
    /// </summary>
    [SugarColumn(ColumnName = "workshop_name", ColumnDescription = "车间名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? WorkshopName { get; set; }

    /// <summary>
    /// 生产班组编码
    /// </summary>
    [SugarColumn(ColumnName = "production_line_code", ColumnDescription = "生产线编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? ProductionLineCode { get; set; }

    /// <summary>
    /// 生产班组名称
    /// </summary>
    [SugarColumn(ColumnName = "production_line_name", ColumnDescription = "生产班组名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ProductionLineName { get; set; }

    /// <summary>
    /// 排程策略（0=按订单排程，1=按库存排程，2=混合排程）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_strategy", ColumnDescription = "排程策略", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ScheduleStrategy { get; set; } = 0;

    /// <summary>
    /// 排程算法（0=正向排程，1=逆向排程，2=双向排程）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_algorithm", ColumnDescription = "排程算法", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ScheduleAlgorithm { get; set; } = 0;

    /// <summary>
    /// 优化目标（0=交期优先，1=产能优先，2=成本优先，3=均衡生产）
    /// </summary>
    [SugarColumn(ColumnName = "optimization_objective", ColumnDescription = "优化目标", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OptimizationObjective { get; set; } = 0;

    /// <summary>
    /// 排程状态（0=草稿，1=计算中，2=已计算，3=已发布，4=执行中，5=已完成，6=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_status", ColumnDescription = "排程状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ScheduleStatus { get; set; } = 0;

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
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人（选项 TaktApsGroups/options?apsGroupCategory=2；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "publish_user_id", ColumnDescription = "发布人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublishUserId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "publish_user_name", ColumnDescription = "发布人姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? PublishUserName { get; set; }

    /// <summary>
    /// 排程说明
    /// </summary>
    [SugarColumn(ColumnName = "schedule_description", ColumnDescription = "排程说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ScheduleDescription { get; set; }

    /// <summary>
    /// APS 排程订单列表（排程批次关联的订单）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktApsOrder.ApsScheduleId))]
    public List<TaktApsOrder>? Orders { get; set; }

    /// <summary>
    /// 排程明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktApsScheduleItem.ApsScheduleId))]
    public List<TaktApsScheduleItem>? Items { get; set; }
}
