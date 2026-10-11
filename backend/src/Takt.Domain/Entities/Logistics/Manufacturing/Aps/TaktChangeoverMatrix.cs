// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktChangeoverMatrix.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：换型矩阵，定义工作中心上产品切换的换型时间
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// 换型矩阵（工作中心 + 前产品 → 后产品的换型时间）
/// </summary>
[SugarTable("takt_logistics_manufacturing_changeover_matrix", "换型矩阵表")]
[SugarIndex("ix_changeover_matrix_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_changeover_matrix_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_changeover_matrix_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WorkCenterCode), OrderByType.Asc, nameof(FromMaterialCode), OrderByType.Asc, nameof(ToMaterialCode), OrderByType.Asc, true)]
public class TaktChangeoverMatrix : TaktCompanyEntityBase
{

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_code", ColumnDescription = "工作中心编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型前物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "from_material_code", ColumnDescription = "换型前物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string FromMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型后物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "to_material_code", ColumnDescription = "换型后物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ToMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_minutes", ColumnDescription = "换型时间分钟", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ChangeoverMinutes { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "matrix_status", ColumnDescription = "矩阵状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MatrixStatus { get; set; } = 1;
}
