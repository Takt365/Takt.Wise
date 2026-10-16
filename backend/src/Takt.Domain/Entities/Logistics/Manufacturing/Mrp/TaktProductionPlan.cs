// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlan.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt生产计划实体，MRP 运算产出（自制件汇总计划，供 APS 排程参考）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mrp;

/// <summary>
/// Takt生产计划实体（公司级；MRP 运算产出自制件计划，非 MPS 上游）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mrp_production_plan", "生产计划表")]
[SugarIndex("ix_production_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_plan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_production_plan_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProductionPlanCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_production_plan_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_production_plan_plan_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_production_plan_sales_plan", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesForecastId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_production_plan_mrp", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialRequirementsPlanningId), OrderByType.Asc, false)]
public class TaktProductionPlan : TaktApprovalEntityBase
{

    /// <summary>
    /// 生产计划编码（租户+公司+工厂内业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 生产计划编码规则生成并展示，非手输；单据类型菜单：生产计划）
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_code", ColumnDescription = "生产计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
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
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "sales_plan_id", ColumnDescription = "来源销售预测ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "sales_plan_code", ColumnDescription = "来源销售预测编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SalesForecastCode { get; set; }

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
    /// 计划总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "计划总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 计划总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "计划总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转工单采购数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 已转工单/采购金额
    /// </summary>
    [SugarColumn(ColumnName = "converted_amount", ColumnDescription = "已转工单采购金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedAmount { get; set; } = 0;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "plan_status", ColumnDescription = "计划状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlanStatus { get; set; } = 1;

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    [SugarColumn(ColumnName = "converted_status", ColumnDescription = "转单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    [SugarColumn(ColumnName = "plan_description", ColumnDescription = "计划说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? PlanDescription { get; set; }

    /// <summary>
    /// 生产计划明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktProductionPlanItem.ProductionPlanId))]
    public List<TaktProductionPlanItem>? Items { get; set; }
}
