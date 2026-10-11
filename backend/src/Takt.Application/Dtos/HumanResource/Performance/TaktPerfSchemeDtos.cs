// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Performance
// 文件名称：TaktPerfSchemeDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfScheme 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPerfScheme 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Performance;

// ========================================
// PerfScheme 响应 DTO
// ========================================

/// <summary>
/// 绩效方案指标（方案维度 + 指标维度合一，每行表示某方案下的一条指标）
/// 对应前端 TaktPerfSchemeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPerfSchemeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PerfSchemeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfSchemeId { get; set; }


    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int SchemeMetricStatus { get; set; } = 0;

}

// ========================================
// PerfScheme 查询 DTO
// ========================================

/// <summary>
/// PerfScheme 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPerfSchemeQueryDto : TaktPagedQuery
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
    /// 方案编码
    /// </summary>
    public string? SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal? SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal? SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string? MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string? Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string? MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string? ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal? StandardWeight { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? SchemeMetricStatus { get; set; }

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
// 创建PerfScheme DTO
// ========================================

/// <summary>
/// 创建PerfScheme DTO
/// </summary>
public class TaktPerfSchemeCreateDto
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
    /// 方案编码
    /// </summary>
    [Required(ErrorMessage = "方案编码不能为空")]
    public string SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    [Required(ErrorMessage = "方案名称不能为空")]
    public string SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    [Required(ErrorMessage = "适用部门不能为空")]
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    [Required(ErrorMessage = "考核周期类型（月度/季度/半年度/年度）不能为空")]
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    [Required(ErrorMessage = "评分标准（百分制/五分制/等级制）不能为空")]
    public string ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    [Required(ErrorMessage = "指标编码不能为空")]
    public string MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    [Required(ErrorMessage = "指标名称不能为空")]
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    [Required(ErrorMessage = "指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）不能为空")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    [Required(ErrorMessage = "指标类型（定量/定性）不能为空")]
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    [Required(ErrorMessage = "评分标准说明不能为空")]
    public string ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal StandardWeight { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int SchemeMetricStatus { get; set; } = 0;

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
// 更新PerfScheme DTO
// ========================================

/// <summary>
/// 更新PerfScheme DTO
/// 继承 TaktPerfSchemeCreateDto，添加 PerfSchemeId 字段
/// </summary>
public class TaktPerfSchemeUpdateDto : TaktPerfSchemeCreateDto
{
    /// <summary>
    /// PerfSchemeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfSchemeId { get; set; }

}

// ========================================
// PerfScheme 状态 DTO
// ========================================

/// <summary>
/// PerfScheme 状态更新 DTO
/// </summary>
public class TaktPerfSchemeStatusDto
{
    /// <summary>
    /// PerfSchemeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfSchemeId { get; set; }

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（0=启用 1=停用）不能为空")]
    public int SchemeMetricStatus { get; set; } = 0;
}

// ========================================
// PerfScheme 排序 DTO
// ========================================

/// <summary>
/// PerfScheme 排序更新 DTO
/// </summary>
public class TaktPerfSchemeSortDto
{
    /// <summary>
    /// PerfSchemeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfSchemeId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PerfScheme 导入模板行 DTO
/// </summary>
public class TaktPerfSchemeTemplateDto
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
    /// 方案编码
    /// </summary>
    public string? SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal? SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal? SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string? MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string? Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string? MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string? ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal? StandardWeight { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? SchemeMetricStatus { get; set; }

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
/// PerfScheme 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPerfSchemeImportDto
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
    /// 方案编码
    /// </summary>
    public string? SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string? ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string? CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string? ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal? SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal? SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string? MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string? MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string? Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string? MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string? ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal? StandardWeight { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int? SchemeMetricStatus { get; set; }

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
/// PerfScheme 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPerfSchemeExportDto
{
    /// <summary>
    /// PerfSchemeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PerfSchemeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案编码
    /// </summary>
    public string SchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string SchemeName { get; set; } = string.Empty;

    /// <summary>
    /// 适用部门
    /// </summary>
    public string ApplicableDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 考核周期类型（月度/季度/半年度/年度）
    /// </summary>
    public string CycleType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准（百分制/五分制/等级制）
    /// </summary>
    public string ScoringStandard { get; set; } = string.Empty;

    /// <summary>
    /// 自评权重（%）
    /// </summary>
    public decimal SelfEvaluationWeight { get; set; }

    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    public decimal SupervisorWeight { get; set; }

    /// <summary>
    /// 指标编码
    /// </summary>
    public string MetricCode { get; set; } = string.Empty;

    /// <summary>
    /// 指标名称
    /// </summary>
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 指标类型（定量/定性）
    /// </summary>
    public string MetricType { get; set; } = string.Empty;

    /// <summary>
    /// 评分标准说明
    /// </summary>
    public string ScoringCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 标准权重（%）
    /// </summary>
    public decimal StandardWeight { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（0=启用 1=停用）
    /// </summary>
    public int SchemeMetricStatus { get; set; } = 0;

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
