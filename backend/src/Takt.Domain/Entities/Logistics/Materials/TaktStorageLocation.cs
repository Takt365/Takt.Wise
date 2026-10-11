// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktStorageLocation.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt库位主数据实体，定义仓库下存储库位（与序列号入出库 LocationCode 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt库位主数据实体（公司级；从属于 TaktWarehouse）
/// </summary>
[SugarTable("takt_logistics_materials_storage_location", "库位主数据表")]
[SugarIndex("ix_storage_location_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_storage_location_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_storage_location_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WarehouseCode), OrderByType.Asc, nameof(LocationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_storage_location_warehouse_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(WarehouseId), OrderByType.Asc, false)]
public class TaktStorageLocation : TaktCompanyEntityBase
{
    /// <summary>
    /// 仓库 ID（选项 TaktWarehouses/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_id", ColumnDescription = "仓库ID", ColumnDataType = "bigint", IsNullable = false)]
    public long WarehouseId { get; set; }
    /// <summary>
    /// 仓库编码（冗余；关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options，DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "存货地点编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string WarehouseCode { get; set; } = string.Empty;
    /// <summary>
    /// 库位编码（40，租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
    /// </summary>
    [SugarColumn(ColumnName = "location_code", ColumnDescription = "库位编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string LocationCode { get; set; } = string.Empty;
    /// <summary>
    /// 库位名称
    /// </summary>
    [SugarColumn(ColumnName = "location_name", ColumnDescription = "库位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string LocationName { get; set; } = string.Empty;
    /// <summary>
    /// 库位类型（字典 logistics_materials_storage_location_type）
    /// </summary>
    [SugarColumn(ColumnName = "location_type", ColumnDescription = "库位类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LocationType { get; set; } = 0;
    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 库位状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "location_status", ColumnDescription = "库位状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int LocationStatus { get; set; } = 1;

    /// <summary>
    /// 所属仓库（主子表关系）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(WarehouseId))]
    public TaktWarehouse? Warehouse { get; set; }
}
