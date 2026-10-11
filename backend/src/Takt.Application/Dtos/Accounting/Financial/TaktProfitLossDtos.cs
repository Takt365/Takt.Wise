// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktProfitLossDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：ProfitLoss 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProfitLoss 生成，请按需审阅）
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
// ProfitLoss 响应 DTO
// ========================================

/// <summary>
/// 利润表（及综合收益）行实体（CAS 利润表列报 / IAS 1 Statement of Profit or Loss and OCI） 列报层次：收入→成本费用→营业利润→利润总额→所得税→净利润→其他综合收益→综合收益总额。 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
/// 对应前端 TaktProfitLossDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProfitLossDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProfitLossID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitLossId { get; set; }


    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    public string StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    public string StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    public int LineCategory { get; set; } = 0;

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsTotalLine { get; set; } = 0;

    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    public decimal PeriodAmount { get; set; }

    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    public decimal PriorPeriodAmount { get; set; }

    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    public decimal YearToDateAmount { get; set; }

    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    public int IsExpense { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int ProfitLossStatus { get; set; } = 0;

}

// ========================================
// ProfitLoss 查询 DTO
// ========================================

/// <summary>
/// ProfitLoss 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProfitLossQueryDto : TaktPagedQuery
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    public string? StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    public string? StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    public int? LineCategory { get; set; }

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsTotalLine { get; set; }

    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    public decimal? PeriodAmount { get; set; }

    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    public decimal? PriorPeriodAmount { get; set; }

    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    public decimal? YearToDateAmount { get; set; }

    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    public int? IsExpense { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? ProfitLossStatus { get; set; }

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
// 创建ProfitLoss DTO
// ========================================

/// <summary>
/// 创建ProfitLoss DTO
/// </summary>
public class TaktProfitLossCreateDto
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "关联工厂（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    [Required(ErrorMessage = "会计期间编码（YYYYMM；利润表报告期）不能为空")]
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    [Required(ErrorMessage = "报表项目编码（利润表/综合收益表行项目）不能为空")]
    public string StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    [Required(ErrorMessage = "报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）不能为空")]
    public string StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    public int LineCategory { get; set; } = 0;

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsTotalLine { get; set; } = 0;

    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    public decimal PeriodAmount { get; set; }

    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    public decimal PriorPeriodAmount { get; set; }

    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    public decimal YearToDateAmount { get; set; }

    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    public int IsExpense { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int ProfitLossStatus { get; set; } = 0;

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
// 更新ProfitLoss DTO
// ========================================

/// <summary>
/// 更新ProfitLoss DTO
/// 继承 TaktProfitLossCreateDto，添加 ProfitLossId 字段
/// </summary>
public class TaktProfitLossUpdateDto : TaktProfitLossCreateDto
{
    /// <summary>
    /// ProfitLossID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitLossId { get; set; }

}

// ========================================
// ProfitLoss 状态 DTO
// ========================================

/// <summary>
/// ProfitLoss 状态更新 DTO
/// </summary>
public class TaktProfitLossStatusDto
{
    /// <summary>
    /// ProfitLossID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitLossId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=停用）不能为空")]
    public int ProfitLossStatus { get; set; } = 0;
}

// ========================================
// ProfitLoss 排序 DTO
// ========================================

/// <summary>
/// ProfitLoss 排序更新 DTO
/// </summary>
public class TaktProfitLossSortDto
{
    /// <summary>
    /// ProfitLossID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitLossId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProfitLoss 导入模板行 DTO
/// </summary>
public class TaktProfitLossTemplateDto
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    public string? StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    public string? StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    public int? LineCategory { get; set; }

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsTotalLine { get; set; }

    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    public decimal? PeriodAmount { get; set; }

    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    public decimal? PriorPeriodAmount { get; set; }

    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    public decimal? YearToDateAmount { get; set; }

    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    public int? IsExpense { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? ProfitLossStatus { get; set; }

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
/// ProfitLoss 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProfitLossImportDto
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
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    public string? StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    public string? StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    public int? LineCategory { get; set; }

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsTotalLine { get; set; }

    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    public decimal? PeriodAmount { get; set; }

    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    public decimal? PriorPeriodAmount { get; set; }

    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    public decimal? YearToDateAmount { get; set; }

    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    public int? IsExpense { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? ProfitLossStatus { get; set; }

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
/// ProfitLoss 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProfitLossExportDto
{
    /// <summary>
    /// ProfitLossID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProfitLossId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    public string StatementLineCode { get; set; } = string.Empty;

    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    public string StatementLineName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    public string? AccountTitleName { get; set; } = string.Empty;

    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    public int LineCategory { get; set; } = 0;

    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsTotalLine { get; set; } = 0;

    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    public decimal PeriodAmount { get; set; }

    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    public decimal PriorPeriodAmount { get; set; }

    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    public decimal YearToDateAmount { get; set; }

    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    public int IsExpense { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int ProfitLossStatus { get; set; } = 0;

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
