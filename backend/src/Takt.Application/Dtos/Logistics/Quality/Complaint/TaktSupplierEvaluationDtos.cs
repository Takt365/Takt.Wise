// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SupplierEvaluation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSupplierEvaluation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Complaint;

// ========================================
// SupplierEvaluation 响应 DTO
// ========================================

/// <summary>
/// 供应商评价考核主表实体
/// 对应前端 TaktSupplierEvaluationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSupplierEvaluationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SupplierEvaluationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（组合唯一索引）
    /// </summary>
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 供应商 名称（填充字段）
    /// </summary>
    public string? SupplierName { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 评价日期
    /// </summary>
    public DateTime EvaluationDate { get; set; }

    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    public int EvaluationPeriod { get; set; } = 0;

    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    public int EvaluationType { get; set; } = 0;

    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? EvaluatorByGroupCode { get; set; }

    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? EvaluatorByGroupName { get; set; }

    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    public string? EvaluationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int OverallRating { get; set; } = 0;

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 主要优点
    /// </summary>
    public string? MainStrengths { get; set; } = string.Empty;

    /// <summary>
    /// 主要问题/不足
    /// </summary>
    public string? MainIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求/建议
    /// </summary>
    public string? ImprovementRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    public int EvaluationConclusion { get; set; } = 0;

    /// <summary>
    /// 整改期限（要求完成日期）
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    public int EvaluationStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 评价项目明细列表（主子表关系）
    /// （子表：TaktSupplierEvaluationItem）
    /// </summary>
    public List<TaktSupplierEvaluationItemDto>? Items { get; set; }

}

// ========================================
// SupplierEvaluation 查询 DTO
// ========================================

/// <summary>
/// SupplierEvaluation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSupplierEvaluationQueryDto : TaktPagedQuery
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
    /// 评价表编码（组合唯一索引）
    /// </summary>
    public string? SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 评价日期（范围查询-开始）
    /// </summary>
    public DateTime? EvaluationDateStart { get; set; }

    /// <summary>
    /// 评价日期（范围查询-结束）
    /// </summary>
    public DateTime? EvaluationDateEnd { get; set; }

    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    public int? EvaluationPeriod { get; set; }

    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    public int? EvaluationType { get; set; }

    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? EvaluatorByGroupCode { get; set; }

    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? EvaluatorByGroupName { get; set; }

    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    public string? EvaluationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? OverallRating { get; set; }

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 主要优点
    /// </summary>
    public string? MainStrengths { get; set; } = string.Empty;

    /// <summary>
    /// 主要问题/不足
    /// </summary>
    public string? MainIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求/建议
    /// </summary>
    public string? ImprovementRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    public int? EvaluationConclusion { get; set; }

    /// <summary>
    /// 整改期限（要求完成日期）（范围查询-开始）
    /// </summary>
    public DateTime? RectificationDeadlineStart { get; set; }

    /// <summary>
    /// 整改期限（要求完成日期）（范围查询-结束）
    /// </summary>
    public DateTime? RectificationDeadlineEnd { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    public int? EvaluationStatus { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int? RectificationStatus { get; set; }

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
// 创建SupplierEvaluation DTO
// ========================================

/// <summary>
/// 创建SupplierEvaluation DTO
/// </summary>
public class TaktSupplierEvaluationCreateDto
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
    /// 评价表编码（组合唯一索引）
    /// </summary>
    [Required(ErrorMessage = "评价表编码（组合唯一索引）不能为空")]
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    [Required(ErrorMessage = "供应商名称不能为空")]
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 评价日期
    /// </summary>
    public DateTime EvaluationDate { get; set; }

    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    public int EvaluationPeriod { get; set; } = 0;

    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    public int EvaluationType { get; set; } = 0;

    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? EvaluatorByGroupCode { get; set; }

    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? EvaluatorByGroupName { get; set; }

    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    public string? EvaluationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int OverallRating { get; set; } = 0;

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 主要优点
    /// </summary>
    public string? MainStrengths { get; set; } = string.Empty;

    /// <summary>
    /// 主要问题/不足
    /// </summary>
    public string? MainIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求/建议
    /// </summary>
    public string? ImprovementRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    public int EvaluationConclusion { get; set; } = 0;

    /// <summary>
    /// 整改期限（要求完成日期）
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    public int EvaluationStatus { get; set; } = 0;

    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 评价项目明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSupplierEvaluationItemCreateDto>? Items { get; set; }

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
// 更新SupplierEvaluation DTO
// ========================================

/// <summary>
/// 更新SupplierEvaluation DTO
/// 继承 TaktSupplierEvaluationCreateDto，添加 SupplierEvaluationId 字段
/// </summary>
public class TaktSupplierEvaluationUpdateDto : TaktSupplierEvaluationCreateDto
{
    /// <summary>
    /// SupplierEvaluationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationId { get; set; }

    /// <summary>
    /// 评价项目明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSupplierEvaluationItemUpdateDto>? Items { get; set; }

}

// ========================================
// SupplierEvaluation 状态 DTO
// ========================================

/// <summary>
/// SupplierEvaluation 状态更新 DTO
/// </summary>
public class TaktSupplierEvaluationStatusDto
{
    /// <summary>
    /// SupplierEvaluationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationId { get; set; }

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    [Required(ErrorMessage = "评价状态（字典 logistics_quality_evaluation_status）不能为空")]
    public int EvaluationStatus { get; set; } = 0;
}

// ========================================
// SupplierEvaluation 排序 DTO
// ========================================

/// <summary>
/// SupplierEvaluation 排序更新 DTO
/// </summary>
public class TaktSupplierEvaluationSortDto
{
    /// <summary>
    /// SupplierEvaluationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SupplierEvaluation 导入模板行 DTO
/// </summary>
public class TaktSupplierEvaluationTemplateDto
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
    /// 评价表编码（组合唯一索引）
    /// </summary>
    public string? SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 评价日期
    /// </summary>
    public DateTime? EvaluationDate { get; set; }

    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    public int? EvaluationPeriod { get; set; }

    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    public int? EvaluationType { get; set; }

    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? EvaluatorByGroupCode { get; set; }

    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? EvaluatorByGroupName { get; set; }

    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    public string? EvaluationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? OverallRating { get; set; }

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 主要优点
    /// </summary>
    public string? MainStrengths { get; set; } = string.Empty;

    /// <summary>
    /// 主要问题/不足
    /// </summary>
    public string? MainIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求/建议
    /// </summary>
    public string? ImprovementRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    public int? EvaluationConclusion { get; set; }

    /// <summary>
    /// 整改期限（要求完成日期）
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    public int? EvaluationStatus { get; set; }

    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int? RectificationStatus { get; set; }

    /// <summary>
    /// 评价项目明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSupplierEvaluationItemCreateDto>? Items { get; set; }

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
/// SupplierEvaluation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSupplierEvaluationImportDto
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
    /// 评价表编码（组合唯一索引）
    /// </summary>
    public string? SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 评价日期
    /// </summary>
    public DateTime? EvaluationDate { get; set; }

    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    public int? EvaluationPeriod { get; set; }

    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    public int? EvaluationType { get; set; }

    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? EvaluatorByGroupCode { get; set; }

    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? EvaluatorByGroupName { get; set; }

    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    public string? EvaluationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? OverallRating { get; set; }

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 主要优点
    /// </summary>
    public string? MainStrengths { get; set; } = string.Empty;

    /// <summary>
    /// 主要问题/不足
    /// </summary>
    public string? MainIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求/建议
    /// </summary>
    public string? ImprovementRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    public int? EvaluationConclusion { get; set; }

    /// <summary>
    /// 整改期限（要求完成日期）
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    public int? EvaluationStatus { get; set; }

    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int? RectificationStatus { get; set; }

    /// <summary>
    /// 评价项目明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSupplierEvaluationItemCreateDto>? Items { get; set; }

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
/// SupplierEvaluation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSupplierEvaluationExportDto
{
    /// <summary>
    /// SupplierEvaluationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationId { get; set; }

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
    /// 评价表编码（组合唯一索引）
    /// </summary>
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 评价日期
    /// </summary>
    public DateTime EvaluationDate { get; set; }

    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    public int EvaluationPeriod { get; set; } = 0;

    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    public int EvaluationType { get; set; } = 0;

    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    public string? EvaluatorByGroupCode { get; set; }

    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    public string? EvaluatorByGroupName { get; set; }

    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
    /// </summary>
    public string? EvaluationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int OverallRating { get; set; } = 0;

    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    public int? TotalScore { get; set; }

    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    public int? DeliveryScore { get; set; }

    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    public int? PriceScore { get; set; }

    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    public int? ServiceScore { get; set; }

    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    public int? TechnicalScore { get; set; }

    /// <summary>
    /// 主要优点
    /// </summary>
    public string? MainStrengths { get; set; } = string.Empty;

    /// <summary>
    /// 主要问题/不足
    /// </summary>
    public string? MainIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求/建议
    /// </summary>
    public string? ImprovementRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    public int EvaluationConclusion { get; set; } = 0;

    /// <summary>
    /// 整改期限（要求完成日期）
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    public int EvaluationStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int RectificationStatus { get; set; } = 0;

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
