// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktTaxCode.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：税收代码实体（公司级；租户 + 公司 + 过程 + 税码 + 税类别唯一）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 税收代码实体（公司级；租户内公司 + 过程 + 税码 + 税类别唯一）
/// </summary>
[SugarTable("takt_accounting_financial_tax_code", "税收代码表")]
[SugarIndex("ix_tax_code_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_tax_code_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_tax_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TaxProcedureCode), OrderByType.Asc, nameof(TaxCode), OrderByType.Asc, nameof(TaxCategory), OrderByType.Asc, true)]
public class TaktTaxCode : TaktCompanyEntityBase
{
    /// <summary>
    /// 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
    /// </summary>
    [SugarColumn(ColumnName = "tax_procedure_code", ColumnDescription = "过程", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string TaxProcedureCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（长度 2；同一租户、公司、过程、税类别下唯一；前端表单选择编码规则后自动通过 TaktNumbering 税码规则生成并展示，非手输；单据类型菜单：税收代码）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "varchar", Length = 2, IsNullable = false)]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
    /// </summary>
    [SugarColumn(ColumnName = "tax_category", ColumnDescription = "税类别", ColumnDataType = "varchar", Length = 1, IsNullable = false)]
    public string TaxCategory { get; set; } = string.Empty;

    /// <summary>
    /// 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "描述", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }

    /// <summary>
    /// 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code_status", ColumnDescription = "税收代码状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TaxCodeStatus { get; set; } = 1;
}
