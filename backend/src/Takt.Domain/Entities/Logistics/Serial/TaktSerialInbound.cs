// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Serial
// 文件名称：TaktSerialInbound.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号入库主表实体，记录序列号入库的基本信息
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Serial;

/// <summary>
/// 序列号入库主表实体
/// </summary>
[SugarTable("takt_logistics_serial_inbound", "序列号入库表")]
[SugarIndex("ix_serial_inbound_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_serial_inbound_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_inbound_inbound_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(InboundCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_serial_inbound_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_serial_inbound_inbound_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InboundDate), OrderByType.Asc, false)]
public class TaktSerialInbound : TaktCompanyEntityBase
{
    /// <summary>
    /// 入库单号（租户+公司+工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "inbound_code", ColumnDescription = "入库单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string InboundCode { get; set; } = string.Empty;
    /// <summary>
    /// 入库日期
    /// </summary>
    [SugarColumn(ColumnName = "inbound_date", ColumnDescription = "入库日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime InboundDate { get; set; } = DateTime.Today;
    /// <summary>
    /// 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "inbound_type", ColumnDescription = "入库类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "4")]
    public int InboundType { get; set; } = 4;
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
    /// 序列号入库明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSerialInboundItem.InboundId))]
    public List<TaktSerialInboundItem>? Items { get; set; }
}
