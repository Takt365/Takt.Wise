// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktBudgetActualDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：BudgetActual 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBudgetActual 生成，请按需审阅）
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
// BudgetActual 响应 DTO
// ========================================

/// <summary>
/// 预算实绩实体（管理会计 Budget vs Actual；CAS 全面预算管理实务 / 国际通用管理会计） 差异约定：差异金额 = 实绩 − 预算；差异率 = 差异 / |预算|（预算为 0 时为 0）。 唯一键：租户 + 公司 + 工厂 + 期间 + 成本中心 + 预算项
/// 对应前端 TaktBudgetActualDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBudgetActualDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BudgetActualID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BudgetActualId { get; set; }


    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 预算项编码
    /// </summary>
    public string BudgetItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算项名称
    /// </summary>
    public string BudgetItemName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    public int BudgetType { get; set; } = 0;

    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    public int MeasureType { get; set; } = 0;

    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    public decimal VarianceAmount { get; set; }

    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    public decimal VariancePercent { get; set; }

    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    public decimal PriorPeriodActual { get; set; }

    /// <summary>
    /// 本年累计预算
    /// </summary>
    public decimal YtdBudgetAmount { get; set; }

    /// <summary>
    /// 本年累计实绩
    /// </summary>
    public decimal YtdActualAmount { get; set; }

    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    public decimal YtdVarianceAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int BudgetActualStatus { get; set; } = 0;

}

// ========================================
// BudgetActual 查询 DTO
// ========================================

/// <summary>
/// BudgetActual 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBudgetActualQueryDto : TaktPagedQuery
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
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 预算项编码
    /// </summary>
    public string? BudgetItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算项名称
    /// </summary>
    public string? BudgetItemName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    public int? BudgetType { get; set; }

    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    public int? MeasureType { get; set; }

    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    public decimal? PriorPeriodActual { get; set; }

    /// <summary>
    /// 本年累计预算
    /// </summary>
    public decimal? YtdBudgetAmount { get; set; }

    /// <summary>
    /// 本年累计实绩
    /// </summary>
    public decimal? YtdActualAmount { get; set; }

    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    public decimal? YtdVarianceAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? BudgetActualStatus { get; set; }

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
// 创建BudgetActual DTO
// ========================================

/// <summary>
/// 创建BudgetActual DTO
/// </summary>
public class TaktBudgetActualCreateDto
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
    /// 会计期间编码（YYYYMM）
    /// </summary>
    [Required(ErrorMessage = "会计期间编码（YYYYMM）不能为空")]
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    [Required(ErrorMessage = "成本中心编码（选项 TaktCostCenters/options；空串表示公司级）不能为空")]
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 预算项编码
    /// </summary>
    [Required(ErrorMessage = "预算项编码不能为空")]
    public string BudgetItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算项名称
    /// </summary>
    [Required(ErrorMessage = "预算项名称不能为空")]
    public string BudgetItemName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    public int BudgetType { get; set; } = 0;

    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    public int MeasureType { get; set; } = 0;

    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    public decimal VarianceAmount { get; set; }

    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    public decimal VariancePercent { get; set; }

    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    public decimal PriorPeriodActual { get; set; }

    /// <summary>
    /// 本年累计预算
    /// </summary>
    public decimal YtdBudgetAmount { get; set; }

    /// <summary>
    /// 本年累计实绩
    /// </summary>
    public decimal YtdActualAmount { get; set; }

    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    public decimal YtdVarianceAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int BudgetActualStatus { get; set; } = 0;

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
// 更新BudgetActual DTO
// ========================================

/// <summary>
/// 更新BudgetActual DTO
/// 继承 TaktBudgetActualCreateDto，添加 BudgetActualId 字段
/// </summary>
public class TaktBudgetActualUpdateDto : TaktBudgetActualCreateDto
{
    /// <summary>
    /// BudgetActualID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BudgetActualId { get; set; }

}

// ========================================
// BudgetActual 状态 DTO
// ========================================

/// <summary>
/// BudgetActual 状态更新 DTO
/// </summary>
public class TaktBudgetActualStatusDto
{
    /// <summary>
    /// BudgetActualID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BudgetActualId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=停用）不能为空")]
    public int BudgetActualStatus { get; set; } = 0;
}

// ========================================
// BudgetActual 排序 DTO
// ========================================

/// <summary>
/// BudgetActual 排序更新 DTO
/// </summary>
public class TaktBudgetActualSortDto
{
    /// <summary>
    /// BudgetActualID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BudgetActualId { get; set; }

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
/// BudgetActual 导入模板行 DTO
/// </summary>
public class TaktBudgetActualTemplateDto
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
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 预算项编码
    /// </summary>
    public string? BudgetItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算项名称
    /// </summary>
    public string? BudgetItemName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    public int? BudgetType { get; set; }

    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    public int? MeasureType { get; set; }

    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    public decimal? PriorPeriodActual { get; set; }

    /// <summary>
    /// 本年累计预算
    /// </summary>
    public decimal? YtdBudgetAmount { get; set; }

    /// <summary>
    /// 本年累计实绩
    /// </summary>
    public decimal? YtdActualAmount { get; set; }

    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    public decimal? YtdVarianceAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? BudgetActualStatus { get; set; }

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
/// BudgetActual 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBudgetActualImportDto
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
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 预算项编码
    /// </summary>
    public string? BudgetItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算项名称
    /// </summary>
    public string? BudgetItemName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    public int? BudgetType { get; set; }

    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    public int? MeasureType { get; set; }

    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    public decimal? PriorPeriodActual { get; set; }

    /// <summary>
    /// 本年累计预算
    /// </summary>
    public decimal? YtdBudgetAmount { get; set; }

    /// <summary>
    /// 本年累计实绩
    /// </summary>
    public decimal? YtdActualAmount { get; set; }

    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    public decimal? YtdVarianceAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? BudgetActualStatus { get; set; }

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
/// BudgetActual 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBudgetActualExportDto
{
    /// <summary>
    /// BudgetActualID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BudgetActualId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    public string? CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// 预算项编码
    /// </summary>
    public string BudgetItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算项名称
    /// </summary>
    public string BudgetItemName { get; set; } = string.Empty;

    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    public string? AccountTitleCode { get; set; } = string.Empty;

    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    public int BudgetType { get; set; } = 0;

    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    public int MeasureType { get; set; } = 0;

    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    public decimal VarianceAmount { get; set; }

    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    public decimal VariancePercent { get; set; }

    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    public decimal PriorPeriodActual { get; set; }

    /// <summary>
    /// 本年累计预算
    /// </summary>
    public decimal YtdBudgetAmount { get; set; }

    /// <summary>
    /// 本年累计实绩
    /// </summary>
    public decimal YtdActualAmount { get; set; }

    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    public decimal YtdVarianceAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int BudgetActualStatus { get; set; } = 0;

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
