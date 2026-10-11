// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Serial
// 文件名称：TaktSerialSummary.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号汇总实体（入库与出库对照一行）；区分扫描原号与计算序号
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Serial;

/// <summary>
/// 序列号汇总（公司级；一行对应一笔入库序列及其可选出库对照）
/// </summary>
/// <remarks>
/// 产品入库/出库序列号为原始扫描号；入库/出库序列号为业务计算后的序号。
/// 未出库时出库侧字段可为空串或空日期。
/// </remarks>
[SugarTable("takt_logistics_serial_summary", "序列号汇总表")]
[SugarIndex("ix_serial_summary_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_serial_summary_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_summary_inbound_serial_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(InboundSerialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_serial_summary_product_inbound_serial", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProductInboundSerialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_summary_inbound_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InboundCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_summary_outbound_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OutboundCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_summary_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_summary_inbound_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InboundDate), OrderByType.Asc, false)]
public class TaktSerialSummary : TaktCompanyEntityBase
{

    /// <summary>
    /// 入库单号
    /// </summary>
    [SugarColumn(ColumnName = "inbound_code", ColumnDescription = "入库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string InboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    [SugarColumn(ColumnName = "inbound_date", ColumnDescription = "入库日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime InboundDate { get; set; } = DateTime.Today;

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "产品物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "inbound_serial_code", ColumnDescription = "入库序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string InboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    [SugarColumn(ColumnName = "inbound_quantity", ColumnDescription = "入库数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InboundQuantity { get; set; }

    /// <summary>
    /// 产品入库序列号（原始扫描号码）
    /// </summary>
    [SugarColumn(ColumnName = "product_inbound_serial_code", ColumnDescription = "产品入库序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ProductInboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号（未出库时为空）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_code", ColumnDescription = "出库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "")]
    public string OutboundCode { get; set; } = string.Empty;

    /// <summary>
    /// 发货单号（未出库时为空）
    /// </summary>
    [SugarColumn(ColumnName = "shipping_invoice_code", ColumnDescription = "发货单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 装车日期（未装车时为空）
    /// </summary>
    [SugarColumn(ColumnName = "loading_date", ColumnDescription = "装车日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LoadingDate { get; set; }

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
    /// 出库日期（未出库时为空）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_date", ColumnDescription = "出库日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 出库序列号（计算后的业务序号；未出库时为空）
    /// </summary>
    [SugarColumn(ColumnName = "outbound_serial_code", ColumnDescription = "出库序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string OutboundSerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库数量
    /// </summary>
    [SugarColumn(ColumnName = "outbound_quantity", ColumnDescription = "出库数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OutboundQuantity { get; set; }

    /// <summary>
    /// 产品出库序列号（原始扫描号码；未出库时为空）
    /// </summary>
    [SugarColumn(ColumnName = "product_outbound_serial_code", ColumnDescription = "产品出库序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string ProductOutboundSerialCode { get; set; } = string.Empty;
}
