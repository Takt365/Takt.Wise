// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowScheme.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程定义实体，存储流程键、版本、设计 JSON 及关联表单
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程定义实体（前端流程方案 FlowScheme）
/// </summary>
[SugarTable("takt_workflow_scheme", "流程定义表")]
[SugarIndex("ix_flow_scheme_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_scheme_key_version_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProcessKey), OrderByType.Asc, nameof(DefinitionVersion), OrderByType.Asc, true)]
[SugarIndex("ix_flow_scheme_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktFlowScheme : TaktCompanyEntityBase
{
    /// <summary>
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    [SugarColumn(ColumnName = "process_key", ColumnDescription = "流程键", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string ProcessKey { get; set; } = string.Empty;
    /// <summary>
    /// 流程名称
    /// </summary>
    [SugarColumn(ColumnName = "process_name", ColumnDescription = "流程名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ProcessName { get; set; } = string.Empty;
    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    [SugarColumn(ColumnName = "definition_version", ColumnDescription = "定义版本号", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DefinitionVersion { get; set; } = 1;
    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    [SugarColumn(ColumnName = "process_version", ColumnDescription = "版本标签", ColumnDataType = "varchar", Length = 32, IsNullable = false, DefaultValue = "v1.0.0")]
    public string ProcessVersion { get; set; } = "v1.0.0";
    /// <summary>
    /// 是否当前最新版（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_latest", ColumnDescription = "是否最新版", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsLatest { get; set; } = 1;
    /// <summary>
    /// 流程分类（字典 sys_flow_category；0=通用流程 1=业务流程 2=系统流程）
    /// </summary>
    [SugarColumn(ColumnName = "process_category", ColumnDescription = "流程分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProcessCategory { get; set; }
    /// <summary>
    /// 流程说明
    /// </summary>
    [SugarColumn(ColumnName = "process_description", ColumnDescription = "流程说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ProcessDescription { get; set; }
    /// <summary>
    /// 挂起状态（字典 sys_flow_suspension_state；1=激活 2=挂起）
    /// </summary>
    [SugarColumn(ColumnName = "suspension_state", ColumnDescription = "挂起状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SuspensionState { get; set; } = 1;
    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    [SugarColumn(ColumnName = "process_content", ColumnDescription = "流程设计", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ProcessContent { get; set; }
    /// <summary>
    /// 部署批次号（引擎发布时生成）
    /// </summary>
    [SugarColumn(ColumnName = "deployment_id", ColumnDescription = "部署批次号", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? DeploymentId { get; set; }
    /// <summary>
    /// 关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "form_id", ColumnDescription = "关联表单ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? FormId { get; set; }
    /// <summary>
    /// 关联表单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "form_code", ColumnDescription = "关联表单编码", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? FormCode { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 发布状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）
    /// </summary>
    [SugarColumn(ColumnName = "process_status", ColumnDescription = "发布状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProcessStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 关联表单
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(FormId))]
    public TaktFlowForm? Form { get; set; }
}
