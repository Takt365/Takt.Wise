// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowAddSign.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程加签记录实体，对接加签/减签能力
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程加签记录实体
/// </summary>
[SugarTable("takt_workflow_add_sign", "流程加签记录表")]
[SugarIndex("ix_flow_add_sign_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_add_sign_instance", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_flow_add_sign_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktFlowAddSign : TaktCompanyEntityBase
{
    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = false)]
    public long InstanceId { get; set; }
    /// <summary>
    /// 加签节点 ID（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    [SugarColumn(ColumnName = "node_id", ColumnDescription = "节点ID", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string NodeId { get; set; } = string.Empty;
    /// <summary>
    /// 加签人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sign_user_id", ColumnDescription = "加签人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long SignUserId { get; set; }
    /// <summary>
    /// 加签人姓名（冗余：按 SignUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "sign_user_name", ColumnDescription = "加签人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? SignUserName { get; set; }
    /// <summary>
    /// 加签方式（字典 sys_flow_add_sign_type；DictValue=sequential、all、one）
    /// </summary>
    [SugarColumn(ColumnName = "sign_type", ColumnDescription = "加签方式", ColumnDataType = "varchar", Length = 32, IsNullable = false, DefaultValue = "sequential")]
    public string SignType { get; set; } = "sequential";
    /// <summary>
    /// 完成后是否回到加签节点（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "return_to_sign_node", ColumnDescription = "回到加签节点", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReturnToSignNode { get; set; }
    /// <summary>
    /// 加签原因
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "加签原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }
    /// <summary>
    /// 是否已处理（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_handled", ColumnDescription = "是否已处理", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsHandled { get; set; }
    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 所属流程实例
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
    public TaktFlowInstance? Instance { get; set; }
}
