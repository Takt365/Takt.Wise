// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Serial
// 文件名称：TaktSerialOutboundItem.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号出库明细实体，记录每个序列号的出库信息
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Serial;

/// <summary>
/// 序列号出库明细实体
/// </summary>
[SugarTable("takt_logistics_serial_outbound_item", "序列号出库明细表")]
[SugarIndex("ix_serial_outbound_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_serial_outbound_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_outbound_item_outbound_serial_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OutboundSerialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_serial_outbound_item_outbound_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OutboundId), OrderByType.Asc, false)]
public class TaktSerialOutboundItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 出库主表 ID（选项 TaktSerialOutbounds/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_id", ColumnDescription = "出库ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OutboundId { get; set; }
    /// <summary>
    /// 出库单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_code", ColumnDescription = "出库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string OutboundCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 出库序列号（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_serial_code", ColumnDescription = "出库序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string OutboundSerialCode { get; set; } = string.Empty;
    /// <summary>
    /// 关联入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "reference_inbound_id", ColumnDescription = "关联入库ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReferenceInboundId { get; set; }
    /// <summary>
    /// 关联入库单号（选项 TaktSerialInbounds/options；DictValue=InboundCode）
    /// </summary>
    [SugarColumn(ColumnName = "reference_inbound_code", ColumnDescription = "关联入库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "")]
    public string ReferenceInboundCode { get; set; } = string.Empty;
    /// <summary>
    /// 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
    /// </summary>
    [SugarColumn(ColumnName = "reference_inbound_line_number", ColumnDescription = "关联入库行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReferenceInboundLineNumber { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;


// ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 出库主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(OutboundId))]
    public TaktSerialOutbound? Outbound { get; set; }
}
