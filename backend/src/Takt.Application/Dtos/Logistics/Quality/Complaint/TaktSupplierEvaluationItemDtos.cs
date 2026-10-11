// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SupplierEvaluationItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSupplierEvaluationItem 生成，请按需审阅）
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
// SupplierEvaluationItem 响应 DTO
// ========================================

/// <summary>
/// 供应商评价考核项目明细实体
/// 对应前端 TaktSupplierEvaluationItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSupplierEvaluationItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SupplierEvaluationItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationItemId { get; set; }

    /// <summary>
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluationId { get; set; }

    /// <summary>
    /// 评价表 名称（填充字段）
    /// </summary>
    public string? EvaluationName { get; set; }

    /// <summary>
    /// 评价表编码（冗余字段，便于查询）
    /// </summary>
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 评价项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分标准
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    public string? EvaluationComment { get; set; } = string.Empty;

    /// <summary>
    /// 存在问题
    /// </summary>
    public string? ExistingIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求
    /// </summary>
    public string? ImprovementRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    public int RectificationRequired { get; set; } = 0;

    /// <summary>
    /// 整改期限
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 评价表主表
    /// （主表：TaktSupplierEvaluation）
    /// </summary>
    public TaktSupplierEvaluationDto? Evaluation { get; set; }

}

// ========================================
// SupplierEvaluationItem 查询 DTO
// ========================================

/// <summary>
/// SupplierEvaluationItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSupplierEvaluationItemQueryDto : TaktPagedQuery
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
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（冗余字段，便于查询）
    /// </summary>
    public string? SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    public int? CategoryType { get; set; }

    /// <summary>
    /// 评价项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// 评分标准
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    public string? EvaluationComment { get; set; } = string.Empty;

    /// <summary>
    /// 存在问题
    /// </summary>
    public string? ExistingIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求
    /// </summary>
    public string? ImprovementRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    public int? RectificationRequired { get; set; }

    /// <summary>
    /// 整改期限（范围查询-开始）
    /// </summary>
    public DateTime? RectificationDeadlineStart { get; set; }

    /// <summary>
    /// 整改期限（范围查询-结束）
    /// </summary>
    public DateTime? RectificationDeadlineEnd { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int? RectificationStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建SupplierEvaluationItem DTO
// ========================================

/// <summary>
/// 创建SupplierEvaluationItem DTO
/// </summary>
public class TaktSupplierEvaluationItemCreateDto
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
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "评价表编码（冗余字段，便于查询）不能为空")]
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 评价项目名称
    /// </summary>
    [Required(ErrorMessage = "评价项目名称不能为空")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分标准
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    public string? EvaluationComment { get; set; } = string.Empty;

    /// <summary>
    /// 存在问题
    /// </summary>
    public string? ExistingIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求
    /// </summary>
    public string? ImprovementRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    public int RectificationRequired { get; set; } = 0;

    /// <summary>
    /// 整改期限
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新SupplierEvaluationItem DTO
// ========================================

/// <summary>
/// 更新SupplierEvaluationItem DTO
/// 继承 TaktSupplierEvaluationItemCreateDto，添加 SupplierEvaluationItemId 字段
/// </summary>
public class TaktSupplierEvaluationItemUpdateDto : TaktSupplierEvaluationItemCreateDto
{
    /// <summary>
    /// SupplierEvaluationItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationItemId { get; set; }

}

// ========================================
// SupplierEvaluationItem 状态 DTO
// ========================================

/// <summary>
/// SupplierEvaluationItem 状态更新 DTO
/// </summary>
public class TaktSupplierEvaluationItemStatusDto
{
    /// <summary>
    /// SupplierEvaluationItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationItemId { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    [Required(ErrorMessage = "整改状态（字典 logistics_quality_rectification_status）不能为空")]
    public int RectificationStatus { get; set; } = 0;
}

// ========================================
// SupplierEvaluationItem 作废 DTO
// ========================================

/// <summary>
/// SupplierEvaluationItem 作废/撤销作废 DTO
/// </summary>
public class TaktSupplierEvaluationItemObsoleteDto
{
    /// <summary>
    /// SupplierEvaluationItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SupplierEvaluationItem 导入模板行 DTO
/// </summary>
public class TaktSupplierEvaluationItemTemplateDto
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
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（冗余字段，便于查询）
    /// </summary>
    public string? SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    public int? CategoryType { get; set; }

    /// <summary>
    /// 评价项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// 评分标准
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    public string? EvaluationComment { get; set; } = string.Empty;

    /// <summary>
    /// 存在问题
    /// </summary>
    public string? ExistingIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求
    /// </summary>
    public string? ImprovementRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    public int? RectificationRequired { get; set; }

    /// <summary>
    /// 整改期限
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int? RectificationStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SupplierEvaluationItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSupplierEvaluationItemImportDto
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
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（冗余字段，便于查询）
    /// </summary>
    public string? SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    public int? CategoryType { get; set; }

    /// <summary>
    /// 评价项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// 评分标准
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    public string? EvaluationComment { get; set; } = string.Empty;

    /// <summary>
    /// 存在问题
    /// </summary>
    public string? ExistingIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求
    /// </summary>
    public string? ImprovementRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    public int? RectificationRequired { get; set; }

    /// <summary>
    /// 整改期限
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int? RectificationStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SupplierEvaluationItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSupplierEvaluationItemExportDto
{
    /// <summary>
    /// SupplierEvaluationItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierEvaluationItemId { get; set; }

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
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（冗余字段，便于查询）
    /// </summary>
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 评价项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分标准
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    public string? EvaluationComment { get; set; } = string.Empty;

    /// <summary>
    /// 存在问题
    /// </summary>
    public string? ExistingIssues { get; set; } = string.Empty;

    /// <summary>
    /// 改进要求
    /// </summary>
    public string? ImprovementRequirement { get; set; } = string.Empty;

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    public int RectificationRequired { get; set; } = 0;

    /// <summary>
    /// 整改期限
    /// </summary>
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
