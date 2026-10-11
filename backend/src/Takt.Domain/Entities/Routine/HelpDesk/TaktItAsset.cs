// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktItAsset.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台 IT 设备保修扩展实体（基本信息见财务 TaktAsset，本表仅存储保修/维保专责信息）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 服务台 IT 设备保修扩展实体（与财务 TaktAsset 按 AssetCode 一对一扩展）
/// </summary>
[SugarTable("takt_routine_help_desk_it_asset", "IT设备保修扩展表")]
[SugarIndex("ix_it_asset_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_it_asset_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_it_asset_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AssetCode), OrderByType.Asc, true)]
[SugarIndex("ix_it_asset_warranty_expiry", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(WarrantyExpiryDate), OrderByType.Asc, false)]
public class TaktItAsset : TaktCompanyEntityBase
{
    /// <summary>
    /// 资产号码（选项 TaktAssets/options；DictValue=AssetCode）
    /// </summary>
    [SugarColumn(ColumnName = "asset_code", ColumnDescription = "资产号码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 保修类型（字典 sys_warranty_type；0=原厂保修 1=延长保修 2=上门保修 3=寄修保修 4=维保合同 5=付费保养）
    /// </summary>
    [SugarColumn(ColumnName = "warranty_type", ColumnDescription = "保修类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int WarrantyType { get; set; } = 0;

    /// <summary>
    /// 保修开始日期
    /// </summary>
    [SugarColumn(ColumnName = "warranty_start_date", ColumnDescription = "保修开始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 保修到期日
    /// </summary>
    [SugarColumn(ColumnName = "warranty_expiry_date", ColumnDescription = "保修到期日", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? WarrantyExpiryDate { get; set; }

    /// <summary>
    /// 保修服务商/厂商
    /// </summary>
    [SugarColumn(ColumnName = "warranty_provider", ColumnDescription = "保修服务商", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? WarrantyProvider { get; set; }

    /// <summary>
    /// 保修合同编码
    /// </summary>
    [SugarColumn(ColumnName = "warranty_contract_code", ColumnDescription = "保修合同编码", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? WarrantyContractCode { get; set; }

    /// <summary>
    /// 服务电话
    /// </summary>
    [SugarColumn(ColumnName = "service_hotline", ColumnDescription = "服务电话", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? ServiceHotline { get; set; }

    /// <summary>
    /// 服务邮箱
    /// </summary>
    [SugarColumn(ColumnName = "service_email", ColumnDescription = "服务邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? ServiceEmail { get; set; }

    /// <summary>
    /// 维保到期日
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_expiry_date", ColumnDescription = "维保到期日", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? MaintenanceExpiryDate { get; set; }

    /// <summary>
    /// 上次维保日期
    /// </summary>
    [SugarColumn(ColumnName = "last_maintenance_date", ColumnDescription = "上次维保日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// 下次维保日期
    /// </summary>
    [SugarColumn(ColumnName = "next_maintenance_date", ColumnDescription = "下次维保日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 保修/维保说明
    /// </summary>
    [SugarColumn(ColumnName = "warranty_remark", ColumnDescription = "保修说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? WarrantyRemark { get; set; }
}
