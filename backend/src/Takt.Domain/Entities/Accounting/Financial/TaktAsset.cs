// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktAsset.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：资产实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 资产实体
/// </summary>
[SugarTable("takt_accounting_financial_asset", "资产")]
[SugarIndex("ix_asset_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_asset_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_asset_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AssetCode), OrderByType.Asc, true)]
public class TaktAsset : TaktCompanyEntityBase
{
    /// <summary>
    /// 资产代码
    /// </summary>
    [SugarColumn(ColumnName = "asset_code", ColumnDescription = "资产代码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string AssetCode { get; set; } = string.Empty;
    /// <summary>
    /// 资产名称
    /// </summary>
    [SugarColumn(ColumnName = "asset_name", ColumnDescription = "资产名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string AssetName { get; set; } = string.Empty;
    /// <summary>
    /// 资产分类（字典 accounting_financial_asset_category）
    /// </summary>
    [SugarColumn(ColumnName = "asset_category", ColumnDescription = "资产分类", ColumnDataType = "varchar", Length = 8, IsNullable = false)]
    public string AssetCategory { get; set; } = string.Empty;
    /// <summary>
    /// 资产类型（字典 accounting_financial_asset_type；NORM=普通资产）
    /// </summary>
    [SugarColumn(ColumnName = "asset_type", ColumnDescription = "资产类型", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "NORM")]
    public string AssetType { get; set; } = "NORM";
    /// <summary>
    /// 资产原值
    /// </summary>
    [SugarColumn(ColumnName = "asset_original_value", ColumnDescription = "资产原值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AssetOriginalValue { get; set; }
    /// <summary>
    /// 资产净值
    /// </summary>
    [SugarColumn(ColumnName = "asset_net_value", ColumnDescription = "资产净值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AssetNetValue { get; set; }
    /// <summary>
    /// 累计折旧
    /// </summary>
    [SugarColumn(ColumnName = "accumulated_depreciation", ColumnDescription = "累计折旧", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AccumulatedDepreciation { get; set; }
    /// <summary>
    /// 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_id", ColumnDescription = "成本中心ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? CostCenterId { get; set; }
    /// <summary>
    /// 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_name", ColumnDescription = "成本中心名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? CostCenterName { get; set; }
    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? DeptId { get; set; }
    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 使用者（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "使用者ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? UserId { get; set; }
    /// <summary>
    /// 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "使用者名称", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? UserName { get; set; }
    /// <summary>
    /// 资产位置
    /// </summary>
    [SugarColumn(ColumnName = "asset_location", ColumnDescription = "资产位置", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? AssetLocation { get; set; }
    /// <summary>
    /// 购买日期
    /// </summary>
    [SugarColumn(ColumnName = "purchase_date", ColumnDescription = "购买日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PurchaseDate { get; set; }
    /// <summary>
    /// 启用日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "启用日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 报废日期
    /// </summary>
    [SugarColumn(ColumnName = "scrap_date", ColumnDescription = "报废日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ScrapDate { get; set; }
    /// <summary>
    /// 处置日期
    /// </summary>
    [SugarColumn(ColumnName = "disposal_date", ColumnDescription = "处置日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DisposalDate { get; set; }
    /// <summary>
    /// 预计使用月数
    /// </summary>
    [SugarColumn(ColumnName = "expected_life_months", ColumnDescription = "预计使用月数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ExpectedLifeMonths { get; set; }
    /// <summary>
    /// 折旧方法（字典 accounting_financial_depreciation_method：0=不自动计提，1=直线法，2=双倍余额递减，3=年数总和，4=产量法，5=手工，6=剩余年限直线）
    /// </summary>
    [SugarColumn(ColumnName = "depreciation_method", ColumnDescription = "折旧方法", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DepreciationMethod { get; set; }
    /// <summary>
    /// 每月折旧金额
    /// </summary>
    [SugarColumn(ColumnName = "monthly_depreciation", ColumnDescription = "每月折旧金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MonthlyDepreciation { get; set; }
    /// <summary>
    /// 资产状态（字典 accounting_financial_asset_status：0=未使用，1=使用中，2=报废，3=处置，4=实物不存在）
    /// </summary>
    [SugarColumn(ColumnName = "asset_status", ColumnDescription = "资产状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int AssetStatus { get; set; } = 1;
}
