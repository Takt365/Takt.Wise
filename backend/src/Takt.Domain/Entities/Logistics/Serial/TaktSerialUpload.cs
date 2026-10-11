// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Serial
// 文件名称：TaktSerialUpload.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号上传送货明细实体（工厂/出库日/发货单号/序号/物料/数量/序列号/装箱/运输/文本）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Serial;

/// <summary>
/// 序列号上传（公司级；发货票维度的送货/装箱明细行）
/// </summary>
[SugarTable("takt_logistics_serial_upload", "序列号上传表")]
[SugarIndex("ix_serial_upload_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_serial_upload_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_upload_invoice_seq_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ShippingInvoiceCode), OrderByType.Asc, nameof(SequenceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_serial_upload_serial_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SerialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_upload_outbound_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OutboundDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_upload_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktSerialUpload : TaktCompanyEntityBase
{

    /// <summary>
    /// 出库日期
    /// </summary>
    [SugarColumn(ColumnName = "outbound_date", ColumnDescription = "出库日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OutboundDate { get; set; } = DateTime.Today;

    /// <summary>
    /// 发货单号（固定 9 位）
    /// </summary>
    [SugarColumn(ColumnName = "shipping_invoice_code", ColumnDescription = "发货单号", ColumnDataType = "nvarchar", Length = 9, IsNullable = false)]
    public string ShippingInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（同一工厂+发货单号内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "sequence_code", ColumnDescription = "序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SequenceCode { get; set; }

    /// <summary>
    /// 产品物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "产品物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 合计数量
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "合计数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalQuantity { get; set; }

    /// <summary>
    /// 序列号（固定 7 位）
    /// </summary>
    [SugarColumn(ColumnName = "serial_code", ColumnDescription = "序列号", ColumnDataType = "nvarchar", Length = 7, IsNullable = false)]
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 装箱数量
    /// </summary>
    [SugarColumn(ColumnName = "packing_quantity", ColumnDescription = "装箱数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PackingQuantity { get; set; }

    /// <summary>
    /// 运输方式（最长 20）
    /// </summary>
    [SugarColumn(ColumnName = "transport_mode", ColumnDescription = "运输方式", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "")]
    public string TransportMode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_text", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string MaterialText { get; set; } = string.Empty;
}
