// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktAccountTitle.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 会计科目实体
/// </summary>
[SugarTable("takt_accounting_financial_account_title", "会计科目表")]
[SugarIndex("ix_account_title_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_account_title_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_account_title_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AccountTitleCode), OrderByType.Asc, true)]
[SugarIndex("ix_account_title_parent", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
public class TaktAccountTitle : TaktCompanyEntityBase
{    /// <summary>
    /// 科目编码（前端表单选择编码规则后自动通过 TaktNumbering 科目编码规则生成并展示，非手输；单据类型菜单：会计科目）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_code", ColumnDescription = "科目编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string AccountTitleCode { get; set; } = string.Empty;
    /// <summary>
    /// 科目名称
    /// </summary>
    [SugarColumn(ColumnName = "account_title_name", ColumnDescription = "科目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string AccountTitleName { get; set; } = string.Empty;
    /// <summary>
    /// 父级 ID
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentId { get; set; }
    /// <summary>
    /// 科目类型（字典 accounting_financial_account_title_type=资产负债表科目，P=初级成本或收入，S=次级成本，N=非经营性收支，C=现金/银行账户）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_type", ColumnDescription = "科目类型", ColumnDataType = "varchar", Length = 1, IsNullable = false)]
    public string AccountTitleType { get; set; } = string.Empty;
    /// <summary>
    /// 余额方向（0=借方，1=贷方）
    /// </summary>
    [SugarColumn(ColumnName = "balance_direction", ColumnDescription = "余额方向", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BalanceDirection { get; set; }
    /// <summary>
    /// 科目层级
    /// </summary>
    [SugarColumn(ColumnName = "account_title_level", ColumnDescription = "科目层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int AccountTitleLevel { get; set; } = 1;
    /// <summary>
    /// 末级科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_leaf", ColumnDescription = "末级科目", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsLeaf { get; set; } = 1;
    /// <summary>
    /// 辅助核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_auxiliary", ColumnDescription = "辅助核算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsAuxiliary { get; set; }
    /// <summary>
    /// 辅助核算类型 / 统驭标识（字典 accounting_financial_auxiliary_type；D=客户，K=供应商，A=资产，S=总账无辅助，M=物料）
    /// </summary>
    [SugarColumn(ColumnName = "auxiliary_type", ColumnDescription = "辅助核算类型", ColumnDataType = "varchar", Length = 1, IsNullable = false)]
    public string AuxiliaryType { get; set; } = string.Empty;
    /// <summary>
    /// 数量核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_quantity", ColumnDescription = "数量核算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsQuantity { get; set; }
    /// <summary>
    /// 外币核算（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_currency", ColumnDescription = "外币核算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsCurrency { get; set; }
    /// <summary>
    /// 现金科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_cash", ColumnDescription = "现金科目", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsCash { get; set; }
    /// <summary>
    /// 银行科目（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_bank", ColumnDescription = "银行科目", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBank { get; set; }
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
    /// 科目状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_status", ColumnDescription = "科目状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int AccountTitleStatus { get; set; } = 1;
}
