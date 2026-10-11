// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerSatisfactionSurveyItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomerSatisfactionSurveyItem 生成，请按需审阅）
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
// CustomerSatisfactionSurveyItem 响应 DTO
// ========================================

/// <summary>
/// 客户满意度调查项目明细实体
/// 对应前端 TaktCustomerSatisfactionSurveyItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerSatisfactionSurveyItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerSatisfactionSurveyItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyItemId { get; set; }

    /// <summary>
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SurveyId { get; set; }

    /// <summary>
    /// 调查表 名称（填充字段）
    /// </summary>
    public string? SurveyName { get; set; }

    /// <summary>
    /// 调查表编码（冗余字段，便于查询）
    /// </summary>
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 调查项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestion { get; set; } = string.Empty;

    /// <summary>
    /// 跟进措施
    /// </summary>
    public string? FollowUpAction { get; set; } = string.Empty;

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    public int FollowUpStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 调查表主表
    /// （主表：TaktCustomerSatisfactionSurvey）
    /// </summary>
    public TaktCustomerSatisfactionSurveyDto? Survey { get; set; }

}

// ========================================
// CustomerSatisfactionSurveyItem 查询 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurveyItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerSatisfactionSurveyItemQueryDto : TaktPagedQuery
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
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SurveyId { get; set; }

    /// <summary>
    /// 调查表编码（冗余字段，便于查询）
    /// </summary>
    public string? CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    public int? CategoryType { get; set; }

    /// <summary>
    /// 调查项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestion { get; set; } = string.Empty;

    /// <summary>
    /// 跟进措施
    /// </summary>
    public string? FollowUpAction { get; set; } = string.Empty;

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    public int? FollowUpStatus { get; set; }

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
// 创建CustomerSatisfactionSurveyItem DTO
// ========================================

/// <summary>
/// 创建CustomerSatisfactionSurveyItem DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyItemCreateDto
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
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SurveyId { get; set; }

    /// <summary>
    /// 调查表编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "调查表编码（冗余字段，便于查询）不能为空")]
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 调查项目名称
    /// </summary>
    [Required(ErrorMessage = "调查项目名称不能为空")]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestion { get; set; } = string.Empty;

    /// <summary>
    /// 跟进措施
    /// </summary>
    public string? FollowUpAction { get; set; } = string.Empty;

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    public int FollowUpStatus { get; set; } = 0;

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
// 更新CustomerSatisfactionSurveyItem DTO
// ========================================

/// <summary>
/// 更新CustomerSatisfactionSurveyItem DTO
/// 继承 TaktCustomerSatisfactionSurveyItemCreateDto，添加 CustomerSatisfactionSurveyItemId 字段
/// </summary>
public class TaktCustomerSatisfactionSurveyItemUpdateDto : TaktCustomerSatisfactionSurveyItemCreateDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyItemId { get; set; }

}

// ========================================
// CustomerSatisfactionSurveyItem 状态 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurveyItem 状态更新 DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyItemStatusDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyItemId { get; set; }

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    [Required(ErrorMessage = "跟进状态（字典 logistics_quality_follow_up_status）不能为空")]
    public int FollowUpStatus { get; set; } = 0;
}

// ========================================
// CustomerSatisfactionSurveyItem 作废 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurveyItem 作废/撤销作废 DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyItemObsoleteDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CustomerSatisfactionSurveyItem 导入模板行 DTO
/// </summary>
public class TaktCustomerSatisfactionSurveyItemTemplateDto
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
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SurveyId { get; set; }

    /// <summary>
    /// 调查表编码（冗余字段，便于查询）
    /// </summary>
    public string? CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    public int? CategoryType { get; set; }

    /// <summary>
    /// 调查项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestion { get; set; } = string.Empty;

    /// <summary>
    /// 跟进措施
    /// </summary>
    public string? FollowUpAction { get; set; } = string.Empty;

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    public int? FollowUpStatus { get; set; }

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
/// CustomerSatisfactionSurveyItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerSatisfactionSurveyItemImportDto
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
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SurveyId { get; set; }

    /// <summary>
    /// 调查表编码（冗余字段，便于查询）
    /// </summary>
    public string? CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    public int? CategoryType { get; set; }

    /// <summary>
    /// 调查项目名称
    /// </summary>
    public string? ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestion { get; set; } = string.Empty;

    /// <summary>
    /// 跟进措施
    /// </summary>
    public string? FollowUpAction { get; set; } = string.Empty;

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    public int? FollowUpStatus { get; set; }

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
/// CustomerSatisfactionSurveyItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerSatisfactionSurveyItemExportDto
{
    /// <summary>
    /// CustomerSatisfactionSurveyItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerSatisfactionSurveyItemId { get; set; }

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
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SurveyId { get; set; }

    /// <summary>
    /// 调查表编码（冗余字段，便于查询）
    /// </summary>
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 调查项目名称
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    public string? ItemDescription { get; set; } = string.Empty;

    /// <summary>
    /// 权重（%）
    /// </summary>
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    public string? CustomerFeedback { get; set; } = string.Empty;

    /// <summary>
    /// 改进建议
    /// </summary>
    public string? ImprovementSuggestion { get; set; } = string.Empty;

    /// <summary>
    /// 跟进措施
    /// </summary>
    public string? FollowUpAction { get; set; } = string.Empty;

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    public int FollowUpStatus { get; set; } = 0;

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
