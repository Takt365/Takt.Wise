// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityAssurance 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityAssurance 生成，请按需审阅）
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
// QualityAssurance 响应 DTO
// ========================================

/// <summary>
/// 品质业务主表,用于记录品质业务的基础信息(年月、顾客)及汇总数据
/// 对应前端 TaktQualityAssuranceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityAssuranceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityAssuranceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? RecordGroupName { get; set; }

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 来料检验费用明细列表
    /// （子表：TaktQualityAssuranceIncoming）
    /// </summary>
    public List<TaktQualityAssuranceIncomingDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表
    /// （子表：TaktQualityAssuranceFirstArticle）
    /// </summary>
    public List<TaktQualityAssuranceFirstArticleDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表
    /// （子表：TaktQualityAssuranceCalibration）
    /// </summary>
    public List<TaktQualityAssuranceCalibrationDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表
    /// （子表：TaktQualityAssuranceOther）
    /// </summary>
    public List<TaktQualityAssuranceOtherDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表
    /// （子表：TaktQualityAssuranceOutgoing）
    /// </summary>
    public List<TaktQualityAssuranceOutgoingDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表
    /// （子表：TaktQualityAssuranceReliability）
    /// </summary>
    public List<TaktQualityAssuranceReliabilityDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表
    /// （子表：TaktQualityAssuranceCustomerResponse）
    /// </summary>
    public List<TaktQualityAssuranceCustomerResponseDto>? CustomerResponseItems { get; set; }

}

// ========================================
// QualityAssurance 查询 DTO
// ========================================

/// <summary>
/// QualityAssurance 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityAssuranceQueryDto : TaktPagedQuery
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string? AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? RecordGroupName { get; set; }

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
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
// 创建QualityAssurance DTO
// ========================================

/// <summary>
/// 创建QualityAssurance DTO
/// </summary>
public class TaktQualityAssuranceCreateDto
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    [Required(ErrorMessage = "品质业务编码(唯一,如:QO-2026-0001)不能为空")]
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    [Required(ErrorMessage = "业务年月(格式:2026-05)不能为空")]
    public string AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? RecordGroupName { get; set; }

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    [Required(ErrorMessage = "成本币种(CNY/USD/JPY等)不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 来料检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceIncomingCreateDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceFirstArticleCreateDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceCalibrationCreateDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceOtherCreateDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceOutgoingCreateDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceReliabilityCreateDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceCustomerResponseCreateDto>? CustomerResponseItems { get; set; }

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
// 更新QualityAssurance DTO
// ========================================

/// <summary>
/// 更新QualityAssurance DTO
/// 继承 TaktQualityAssuranceCreateDto，添加 QualityAssuranceId 字段
/// </summary>
public class TaktQualityAssuranceUpdateDto : TaktQualityAssuranceCreateDto
{
    /// <summary>
    /// QualityAssuranceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 来料检验费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceIncomingUpdateDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceFirstArticleUpdateDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceCalibrationUpdateDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceOtherUpdateDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceOutgoingUpdateDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceReliabilityUpdateDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表（子表，级联保存）
    /// </summary>
    public new List<TaktQualityAssuranceCustomerResponseUpdateDto>? CustomerResponseItems { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityAssurance 导入模板行 DTO
/// </summary>
public class TaktQualityAssuranceTemplateDto
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string? AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? RecordGroupName { get; set; }

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 来料检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceIncomingCreateDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceFirstArticleCreateDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceCalibrationCreateDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceOtherCreateDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceOutgoingCreateDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceReliabilityCreateDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceCustomerResponseCreateDto>? CustomerResponseItems { get; set; }

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
/// QualityAssurance 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityAssuranceImportDto
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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string? QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string? AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? RecordGroupName { get; set; }

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal? TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 来料检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceIncomingCreateDto>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceFirstArticleCreateDto>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceCalibrationCreateDto>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceOtherCreateDto>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceOutgoingCreateDto>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceReliabilityCreateDto>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表（子表，级联保存）
    /// </summary>
    public List<TaktQualityAssuranceCustomerResponseCreateDto>? CustomerResponseItems { get; set; }

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
/// QualityAssurance 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityAssuranceExportDto
{
    /// <summary>
    /// QualityAssuranceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

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
    /// 品质业务编码(唯一,如:QO-2026-0001)
    /// </summary>
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    public string AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// Debit Note No
    /// </summary>
    public string? DebitNoteCode { get; set; } = string.Empty;

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    public string? RecordGroupName { get; set; }

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    public decimal TotalQualityCost { get; set; }

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
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
