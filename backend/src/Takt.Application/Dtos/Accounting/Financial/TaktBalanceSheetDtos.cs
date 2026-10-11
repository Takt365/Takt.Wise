// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktBalanceSheetDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：BalanceSheet 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBalanceSheet 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// BalanceSheet 响应 DTO
// ========================================

/// <summary>
/// 资产负债表行实体（CAS 财务报表列报 / IAS 1 Statement of Financial Position） 列报原则：资产与负债按流动/非流动分类；所有者权益单独列示；期末列报金额参与「资产=负债+权益」勾稽。 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
/// 对应前端 TaktBalanceSheetDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBalanceSheetDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BalanceSheetID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BalanceSheetId { get; set; }


    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    public string StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    public string StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    public int LineCategory { get; set; } = 0;

    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    public int BalanceDirection { get; set; } = 0;

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    public int IsTotalLine { get; set; } = 0;

    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// 本期借方发生额
    /// </summary>
    public decimal DebitAmount { get; set; }

    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    public decimal ClosingBalance { get; set; }

    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    public decimal PresentationAmount { get; set; }

    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    public decimal PriorPeriodAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前；应与报表印刷顺序一致）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int BalanceSheetStatus { get; set; } = 0;

}

// ========================================
// BalanceSheet 查询 DTO
// ========================================

/// <summary>
/// BalanceSheet 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBalanceSheetQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    public string? StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    public string? StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    public int? LineCategory { get; set; }

    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    public int? BalanceDirection { get; set; }

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    public int? IsTotalLine { get; set; }

    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    public decimal? OpeningBalance { get; set; }

    /// <summary>
    /// 本期借方发生额
    /// </summary>
    public decimal? DebitAmount { get; set; }

    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    public decimal? ClosingBalance { get; set; }

    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    public decimal? PresentationAmount { get; set; }

    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    public decimal? PriorPeriodAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前；应与报表印刷顺序一致）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? BalanceSheetStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建BalanceSheet DTO
// ========================================

/// <summary>
/// 创建BalanceSheet DTO
/// </summary>
public class TaktBalanceSheetCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    [Required(ErrorMessage = "关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    [Required(ErrorMessage = "会计期间编码（YYYYMM；资产负债表日所属报告期）不能为空")]
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    [Required(ErrorMessage = "报表项目编码（资产负债表行项目；可与总账科目多对一映射）不能为空")]
    public string StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    [Required(ErrorMessage = "报表项目名称（如「货币资金」「应付账款」「未分配利润」）不能为空")]
    public string StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    public int LineCategory { get; set; } = 0;

    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    public int BalanceDirection { get; set; } = 0;

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    public int IsTotalLine { get; set; } = 0;

    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// 本期借方发生额
    /// </summary>
    public decimal DebitAmount { get; set; }

    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    public decimal ClosingBalance { get; set; }

    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    public decimal PresentationAmount { get; set; }

    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    public decimal PriorPeriodAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；报告货币）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int BalanceSheetStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新BalanceSheet DTO
// ========================================

/// <summary>
/// 更新BalanceSheet DTO
/// 继承 TaktBalanceSheetCreateDto，添加 BalanceSheetId 字段
/// </summary>
public class TaktBalanceSheetUpdateDto : TaktBalanceSheetCreateDto
{
    /// <summary>
    /// BalanceSheetID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BalanceSheetId { get; set; }

}

// ========================================
// BalanceSheet 状态 DTO
// ========================================

/// <summary>
/// BalanceSheet 状态更新 DTO
/// </summary>
public class TaktBalanceSheetStatusDto
{
    /// <summary>
    /// BalanceSheetID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BalanceSheetId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=停用）不能为空")]
    public int BalanceSheetStatus { get; set; } = 0;
}

// ========================================
// BalanceSheet 排序 DTO
// ========================================

/// <summary>
/// BalanceSheet 排序更新 DTO
/// </summary>
public class TaktBalanceSheetSortDto
{
    /// <summary>
    /// BalanceSheetID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BalanceSheetId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前；应与报表印刷顺序一致）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前；应与报表印刷顺序一致）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BalanceSheet 导入模板行 DTO
/// </summary>
public class TaktBalanceSheetTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    public string? StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    public string? StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    public int? LineCategory { get; set; }

    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    public int? BalanceDirection { get; set; }

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    public int? IsTotalLine { get; set; }

    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    public decimal? OpeningBalance { get; set; }

    /// <summary>
    /// 本期借方发生额
    /// </summary>
    public decimal? DebitAmount { get; set; }

    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    public decimal? ClosingBalance { get; set; }

    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    public decimal? PresentationAmount { get; set; }

    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    public decimal? PriorPeriodAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? BalanceSheetStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// BalanceSheet 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBalanceSheetImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    public string? StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    public string? StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    public int? LineCategory { get; set; }

    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    public int? BalanceDirection { get; set; }

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    public int? IsTotalLine { get; set; }

    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    public decimal? OpeningBalance { get; set; }

    /// <summary>
    /// 本期借方发生额
    /// </summary>
    public decimal? DebitAmount { get; set; }

    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    public decimal? ClosingBalance { get; set; }

    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    public decimal? PresentationAmount { get; set; }

    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    public decimal? PriorPeriodAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? BalanceSheetStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// BalanceSheet 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBalanceSheetExportDto
{
    /// <summary>
    /// BalanceSheetID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BalanceSheetId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    public string StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    public string StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    public int LineCategory { get; set; } = 0;

    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    public int BalanceDirection { get; set; } = 0;

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    public int IsTotalLine { get; set; } = 0;

    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// 本期借方发生额
    /// </summary>
    public decimal DebitAmount { get; set; }

    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    public decimal ClosingBalance { get; set; }

    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    public decimal PresentationAmount { get; set; }

    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    public decimal PriorPeriodAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前；应与报表印刷顺序一致）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int BalanceSheetStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
