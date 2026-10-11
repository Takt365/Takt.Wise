// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIssue 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityIssue 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

// ========================================
// QualityIssue 响应 DTO
// ========================================

/// <summary>
/// 品质问题应对主表,用于记录质量问题的基础信息(年月日、机种、批次)及汇总数据
/// 对应前端 TaktQualityIssueDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityIssueDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityIssueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议/调查/试验费用明细列表
    /// （子表：TaktQualityIssueMeeting）
    /// </summary>
    public List<TaktQualityIssueMeetingDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表
    /// （子表：TaktQualityIssueAssyRework）
    /// </summary>
    public List<TaktQualityIssueAssyReworkDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表
    /// （子表：TaktQualityIssuePcbaRework）
    /// </summary>
    public List<TaktQualityIssuePcbaReworkDto>? PcbaReworkItems { get; set; }

}

// ========================================
// QualityIssue 查询 DTO
// ========================================

/// <summary>
/// QualityIssue 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityIssueQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期（范围查询-开始）
    /// </summary>
    public DateTime? IssueDateStart { get; set; }

    /// <summary>
    /// 问题日期（范围查询-结束）
    /// </summary>
    public DateTime? IssueDateEnd { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string? Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int? TotalTimeMinutes { get; set; }

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

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
// 创建QualityIssue DTO
// ========================================

/// <summary>
/// 创建QualityIssue DTO
/// </summary>
public class TaktQualityIssueCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    [Required(ErrorMessage = "品质问题编码（唯一，如：QF-2026-0001）不能为空")]
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    [Required(ErrorMessage = "机种/产品型号不能为空")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    [Required(ErrorMessage = "批次号/Lot No不能为空")]
    public string Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    [Required(ErrorMessage = "成本币种（CNY/USD/JPY等）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议/调查/试验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssueMeetingCreateDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssueAssyReworkCreateDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssuePcbaReworkCreateDto>? PcbaReworkItems { get; set; }

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
// 更新QualityIssue DTO
// ========================================

/// <summary>
/// 更新QualityIssue DTO
/// 继承 TaktQualityIssueCreateDto，添加 QualityIssueId 字段
/// </summary>
public class TaktQualityIssueUpdateDto : TaktQualityIssueCreateDto
{
    /// <summary>
    /// QualityIssueID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 会议/调查/试验费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityIssueMeetingUpdateDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityIssueAssyReworkUpdateDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityIssuePcbaReworkUpdateDto>? PcbaReworkItems { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityIssue 导入模板行 DTO
/// </summary>
public class TaktQualityIssueTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string? Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int? TotalTimeMinutes { get; set; }

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议/调查/试验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssueMeetingCreateDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssueAssyReworkCreateDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssuePcbaReworkCreateDto>? PcbaReworkItems { get; set; }

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
/// QualityIssue 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityIssueImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string? QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string? Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string? Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int? TotalTimeMinutes { get; set; }

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议/调查/试验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssueMeetingCreateDto>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssueAssyReworkCreateDto>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityIssuePcbaReworkCreateDto>? PcbaReworkItems { get; set; }

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
/// QualityIssue 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityIssueExportDto
{
    /// <summary>
    /// QualityIssueID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 问题日期
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    public string Lot { get; set; } = string.Empty;

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    public string? QualityProblemsResponse { get; set; } = string.Empty;

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    public string? ReworkDueToDefects { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    public string? NeedRework { get; set; } = string.Empty;

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

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
