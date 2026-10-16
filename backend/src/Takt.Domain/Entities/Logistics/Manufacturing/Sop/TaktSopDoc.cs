#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopDoc.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 文档头（产品+工序+工位+版本）；审批单，FlowInstanceId 由业务发起流程后写入
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 文档头实体。FlowInstanceId 由业务在发起流程后写入；审批状态见 ApprovalStatus。
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_doc", "SOP文档头表")]
[SugarIndex("ix_sop_doc_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_doc_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_doc_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SopCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_doc_product_step", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(RoutingItemId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_doc_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktSopDoc : TaktApprovalEntityBase
{

    /// <summary>
    /// SOP 编码（前端表单选择编码规则后自动通过 TaktNumbering SOP编码规则生成并展示，非手输；单据类型菜单：SOP文档）
    /// </summary>
    [SugarColumn(ColumnName = "sop_code", ColumnDescription = "SOP编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SopCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP 名称
    /// </summary>
    [SugarColumn(ColumnName = "sop_name", ColumnDescription = "SOP名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string SopName { get; set; } = string.Empty;

    /// <summary>
    /// 产品/物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线明细 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "routing_item_id", ColumnDescription = "工序ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_id", ColumnDescription = "工位ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 当前生效版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "current_revision_id", ColumnDescription = "当前版本ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentRevisionId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "sop_status", ColumnDescription = "文档状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SopStatus { get; set; } = 1;

    /// <summary>
    /// 工序
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoutingItemId))]
    public TaktRoutingItem? RoutingItem { get; set; }

    /// <summary>
    /// 工位
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(WorkstationId))]
    public TaktSopWorkstation? Workstation { get; set; }

    /// <summary>
    /// 版本列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopRevision.SopId))]
    public List<TaktSopRevision>? Revisions { get; set; }
}
