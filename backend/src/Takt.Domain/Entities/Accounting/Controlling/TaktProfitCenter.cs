// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Controlling
// 文件名称：TaktProfitCenter.cs
// 创建时间：2025-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：利润中心实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Controlling;

/// <summary>
/// 利润中心实体
/// </summary>
[SugarTable("takt_accounting_controlling_profit_center", "利润中心表")]
[SugarIndex("ix_profit_center_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_profit_center_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_profit_center_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProfitCenterCode), OrderByType.Asc, true)]
[SugarIndex("ix_profit_center_parent", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
public class TaktProfitCenter : TaktCompanyEntityBase
{    /// <summary>
    /// 利润中心编码（4位，租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心编码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string ProfitCenterCode { get; set; } = string.Empty;
    /// <summary>
    /// 利润中心名称
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_name", ColumnDescription = "利润中心名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ProfitCenterName { get; set; } = string.Empty;
    /// <summary>
    /// 父级 ID
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentId { get; set; }
    /// <summary>
    /// 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "manager_id", ColumnDescription = "负责人ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? ManagerId { get; set; }
    /// <summary>
    /// 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
    /// </summary>
    [SugarColumn(ColumnName = "manager_name", ColumnDescription = "负责人姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ManagerName { get; set; }
    /// <summary>
    /// 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "所属部门ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? DeptId { get; set; }
    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "所属部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 利润中心层级
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_level", ColumnDescription = "利润中心层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProfitCenterLevel { get; set; } = 1;
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_from", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidFrom { get; set; } = DateTime.Today;
    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_to", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidTo { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_status", ColumnDescription = "利润中心状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProfitCenterStatus { get; set; } = 1;
}
