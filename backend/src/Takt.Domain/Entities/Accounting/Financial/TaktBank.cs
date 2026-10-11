// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktBank.cs
// 创建时间：2026-07-22
// 创建人：Takt365(Cursor AI)
// 功能描述：银行信息实体（公司级；字段顺序与长度对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 银行信息实体（公司级；租户+公司隔离；按国家地区 + 银行代码唯一）
/// </summary>
[SugarTable("takt_accounting_financial_bank", "银行信息表")]
[SugarIndex("ix_bank_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bank_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_bank_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CountryRegion), OrderByType.Asc, nameof(BankCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_bank_swift", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SwiftBic), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_bank_group", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BankGroup), OrderByType.Asc, false)]
public class TaktBank : TaktCompanyEntityBase
{
    /// <summary>
    /// 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_region", ColumnDescription = "国家地区", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string CountryRegion { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（；CHAR 15；与国家地区组成业务唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "bank_code", ColumnDescription = "银行代码", ColumnDataType = "nvarchar", Length = 15, IsNullable = false)]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称1
    /// </summary>
    [SugarColumn(ColumnName = "bank_name1", ColumnDescription = "银行名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string BankName1 { get; set; } = string.Empty;

    /// <summary>
    /// 银行名称2
    /// </summary>
    [SugarColumn(ColumnName = "bank_name2", ColumnDescription = "银行名称2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? BankName2 { get; set; }

    /// <summary>
    /// 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "province", ColumnDescription = "州省", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? Province { get; set; }

    /// <summary>
    /// 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "prefecture", ColumnDescription = "地市", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? Prefecture { get; set; }

    /// <summary>
    /// 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
    /// </summary>
    [SugarColumn(ColumnName = "district", ColumnDescription = "区县", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? District { get; set; }

    /// <summary>
    /// 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
    /// </summary>
    [SugarColumn(ColumnName = "township", ColumnDescription = "乡镇街道", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? Township { get; set; }

    /// <summary>
    /// 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
    /// </summary>
    [SugarColumn(ColumnName = "village", ColumnDescription = "行政村", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? Village { get; set; }

    /// <summary>
    /// 地址1（详细地址行1）
    /// </summary>
    [SugarColumn(ColumnName = "address1", ColumnDescription = "地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Address1 { get; set; }

    /// <summary>
    /// 地址2（详细地址行2）
    /// </summary>
    [SugarColumn(ColumnName = "address2", ColumnDescription = "地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Address2 { get; set; }

    /// <summary>
    /// 银行识别码（SWIFT/BIC；CHAR 11）
    /// </summary>
    [SugarColumn(ColumnName = "swift_bic", ColumnDescription = "银行识别码", ColumnDataType = "nvarchar", Length = 11, IsNullable = true)]
    public string? SwiftBic { get; set; }

    /// <summary>
    /// 银行组（；CHAR 2）
    /// </summary>
    [SugarColumn(ColumnName = "bank_group", ColumnDescription = "银行组", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? BankGroup { get; set; }

    /// <summary>
    /// 邮政银行往来账户（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "pobk_cur_ac", ColumnDescription = "邮政银行往来账户", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PobkCurAc { get; set; } = 0;

    /// <summary>
    /// 银行编码（；CHAR 15）
    /// </summary>
    [SugarColumn(ColumnName = "bank_number", ColumnDescription = "银行编码", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? BankNumber { get; set; }

    /// <summary>
    /// 邮政银行（；CHAR 16）
    /// </summary>
    [SugarColumn(ColumnName = "postal_bank", ColumnDescription = "邮政银行", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? PostalBank { get; set; }

    /// <summary>
    /// 地址号（；CHAR 10）
    /// </summary>
    [SugarColumn(ColumnName = "address_number", ColumnDescription = "地址号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? AddressNumber { get; set; }

    /// <summary>
    /// 分行（；CHAR 40）
    /// </summary>
    [SugarColumn(ColumnName = "branch", ColumnDescription = "分行", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Branch { get; set; }

    /// <summary>
    /// 方法（CHAR 4）
    /// </summary>
    [SugarColumn(ColumnName = "bank_method", ColumnDescription = "方法", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? BankMethod { get; set; }

    /// <summary>
    /// 格式（含银行数据文件的格式；CHAR 3）
    /// </summary>
    [SugarColumn(ColumnName = "bank_format", ColumnDescription = "格式", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? BankFormat { get; set; }

    /// <summary>
    /// IBAN 规则（CHAR 6）
    /// </summary>
    [SugarColumn(ColumnName = "iban_rule", ColumnDescription = "IBAN规则", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? IbanRule { get; set; }

    /// <summary>
    /// 企业间（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "sdd_b2b", ColumnDescription = "企业间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SddB2b { get; set; } = 0;

    /// <summary>
    /// 核心个人（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "sdd_core", ColumnDescription = "核心个人", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SddCore { get; set; } = 0;

    /// <summary>
    /// SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
    /// </summary>
    [SugarColumn(ColumnName = "sdd_rtrans", ColumnDescription = "SEPA拒付交易支持标识", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SddRtrans { get; set; } = 0;

    /// <summary>
    /// BIC+ 编码（CHAR 12）
    /// </summary>
    [SugarColumn(ColumnName = "bic_plus_number", ColumnDescription = "BIC+编码", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? BicPlusNumber { get; set; }

    /// <summary>
    /// 路径代码（CHAR 15）
    /// </summary>
    [SugarColumn(ColumnName = "path_code", ColumnDescription = "路径代码", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? PathCode { get; set; }
}
