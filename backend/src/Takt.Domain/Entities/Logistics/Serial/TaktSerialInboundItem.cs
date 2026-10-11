// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Serial
// 文件名称：TaktSerialInboundItem.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号入库明细实体，记录每个序列号的入库信息
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Serial;

/// <summary>
/// 序列号入库明细实体
/// </summary>
[SugarTable("takt_logistics_serial_inbound_item", "序列号入库明细表")]
[SugarIndex("ix_serial_inbound_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_serial_inbound_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_inbound_item_inbound_serial_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InboundSerialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_serial_inbound_item_inbound_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InboundId), OrderByType.Asc, false)]
public class TaktSerialInboundItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 入库主表 ID（选项 TaktSerialInbounds/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inbound_id", ColumnDescription = "入库ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InboundId { get; set; }
    /// <summary>
    /// 入库单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "inbound_code", ColumnDescription = "入库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string InboundCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 入库序列号（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "inbound_serial_code", ColumnDescription = "入库序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;


// ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 入库主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(InboundId))]
    public TaktSerialInbound? Inbound { get; set; }
}
