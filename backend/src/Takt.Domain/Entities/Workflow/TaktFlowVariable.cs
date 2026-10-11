// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowVariable.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程变量实体，支撑条件分支与业务回写
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程变量实体
/// </summary>
[SugarTable("takt_workflow_variable", "流程变量表")]
[SugarIndex("ix_flow_variable_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_variable_instance_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InstanceId), OrderByType.Asc, nameof(VariableName), OrderByType.Asc, true)]
[SugarIndex("ix_flow_variable_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktFlowVariable : TaktCompanyEntityBase
{
    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = false)]
    public long InstanceId { get; set; }
    /// <summary>
    /// 任务 ID（选项 TaktFlowTasks/options；DictValue=Id；任务级变量时填写）
    /// </summary>
    [SugarColumn(ColumnName = "task_id", ColumnDescription = "任务ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? TaskId { get; set; }
    /// <summary>
    /// 变量名
    /// </summary>
    [SugarColumn(ColumnName = "variable_name", ColumnDescription = "变量名", ColumnDataType = "varchar", Length = 128, IsNullable = false)]
    public string VariableName { get; set; } = string.Empty;
    /// <summary>
    /// 变量类型（字典 sys_flow_variable_type；0=字符串 1=长整型 2=双精度 3=布尔 4=JSON）
    /// </summary>
    [SugarColumn(ColumnName = "variable_type", ColumnDescription = "变量类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VariableType { get; set; } = 0;
    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    [SugarColumn(ColumnName = "text_value", ColumnDescription = "文本值", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? TextValue { get; set; }
    /// <summary>
    /// 长整型值
    /// </summary>
    [SugarColumn(ColumnName = "long_value", ColumnDescription = "长整型值", ColumnDataType = "bigint", IsNullable = true)]
    public long? LongValue { get; set; }
    /// <summary>
    /// 双精度值
    /// </summary>
    [SugarColumn(ColumnName = "double_value", ColumnDescription = "双精度值", ColumnDataType = "float", IsNullable = true)]
    public double? DoubleValue { get; set; }
    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 所属流程实例
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
    public TaktFlowInstance? Instance { get; set; }
}
