// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktWarehouse.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt仓库主数据实体，定义工厂下仓储地点（与序列号入出库 WarehouseCode 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt仓库主数据实体（公司级；按工厂划分仓储地点）
/// </summary>
[SugarTable("takt_logistics_materials_warehouse", "仓库主数据表")]
[SugarIndex("ix_warehouse_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_warehouse_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_warehouse_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WarehouseCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_warehouse_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktWarehouse : TaktCompanyEntityBase
{
    /// <summary>
    /// 存货地点编码（4位，租户+公司+工厂内唯一；业务表冗余存此编码）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "存货地点编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string WarehouseCode { get; set; } = string.Empty;
    /// <summary>
    /// 仓库名称
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_name", ColumnDescription = "仓库名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string WarehouseName { get; set; } = string.Empty;
    /// <summary>
    /// 仓库简称
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_short_name", ColumnDescription = "仓库简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? WarehouseShortName { get; set; }
    /// <summary>
    /// 仓库地址（address）
    /// </summary>
    [SugarColumn(ColumnName = "address", ColumnDescription = "仓库地址", ColumnDataType = "varchar", Length = 255, IsNullable = true)]
    public string? Address { get; set; }
    /// <summary>
    /// 联系人（contact_person）
    /// </summary>
    [SugarColumn(ColumnName = "contact_person", ColumnDescription = "联系人", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 联系电话（contact_phone）
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "varchar", Length = 30, IsNullable = true)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 仓库负责人（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "manager_user_id", ColumnDescription = "仓库负责人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ManagerUserId { get; set; }
    /// <summary>
    /// 仓库负责人姓名（冗余：按 ManagerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "manager_user_name", ColumnDescription = "仓库负责人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? ManagerUserName { get; set; }
    /// <summary>
    /// 虚拟仓（is_virtual；字典 sys_yes_no；0=实体仓，1=虚拟仓）
    /// </summary>
    [SugarColumn(ColumnName = "is_virtual", ColumnDescription = "虚拟仓", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsVirtual { get; set; } = 0;
    /// <summary>
    /// 仓库类型（字典 logistics_materials_warehouse_type；0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_type", ColumnDescription = "仓库类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int WarehouseType { get; set; } = 2;
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
    /// 仓库状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_status", ColumnDescription = "仓库状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int WarehouseStatus { get; set; } = 1;

    /// <summary>
    /// 库位列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktStorageLocation.WarehouseId))]
    public List<TaktStorageLocation>? StorageLocations { get; set; }
}
