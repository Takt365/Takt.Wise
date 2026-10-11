// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Serial
// 文件名称：TaktSerialOutbound.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号出库主表实体，记录序列号出库的基本信息
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Serial;

/// <summary>
/// 序列号出库主表实体
/// </summary>
[SugarTable("takt_logistics_serial_outbound", "序列号出库表")]
[SugarIndex("ix_serial_outbound_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_serial_outbound_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_outbound_outbound_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(OutboundCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_serial_outbound_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_outbound_outbound_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OutboundDate), OrderByType.Asc, false)]
public class TaktSerialOutbound : TaktCompanyEntityBase
{
    /// <summary>
    /// 出库单号（租户+公司+工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_code", ColumnDescription = "出库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string OutboundCode { get; set; } = string.Empty;
    /// <summary>
    /// 发货单号
    /// </summary>
    [SugarColumn(ColumnName = "shipping_invoice_code", ColumnDescription = "发货单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string ShippingInvoiceCode { get; set; } = string.Empty;
    /// <summary>
    /// 装车日期
    /// </summary>
    [SugarColumn(ColumnName = "outbound_date", ColumnDescription = "装车日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OutboundDate { get; set; } = DateTime.Today;
    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options；DictValue=DestinationCode）
    /// </summary>
    [SugarColumn(ColumnName = "destination", ColumnDescription = "仕向地", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string Destination { get; set; } = string.Empty;
    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
    /// </summary>
    [SugarColumn(ColumnName = "destination_port", ColumnDescription = "目的地港", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string DestinationPort { get; set; } = string.Empty;
    /// <summary>
    /// 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_type", ColumnDescription = "出库类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "5")]
    public int OutboundType { get; set; } = 5;
    /// <summary>
    /// 仓库编码（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "仓库编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "C008")]
    public string WarehouseCode { get; set; } = "C008";
    /// <summary>
    /// 库位编码（选项 TaktStorageLocations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "location_code", ColumnDescription = "库位编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "1F-2")]
    public string LocationCode { get; set; } = "1F-2";
    /// <summary>
    /// 总数量
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "总数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalQuantity { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 序列号出库明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSerialOutboundItem.OutboundId))]
    public List<TaktSerialOutboundItem>? Items { get; set; }
}
