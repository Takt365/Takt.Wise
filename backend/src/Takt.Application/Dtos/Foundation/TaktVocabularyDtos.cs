// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktVocabularyDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Vocabulary 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktVocabulary 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Vocabulary 响应 DTO
// ========================================

/// <summary>
/// 敏感词实体（租户内共享，供新闻、公告评论等模块引用）
/// 对应前端 TaktVocabularyDto
/// 继承 TaktTenantCoreDtoBase（组合 4）
/// </summary>
public class TaktVocabularyDto : TaktTenantCoreDtoBase
{

    /// <summary>
    /// VocabularyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VocabularyId { get; set; }

    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    public string WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    public int WordCategory { get; set; } = 0;

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    public int FilterLevel { get; set; } = 0;

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    public string ReplaceText { get; set; } = "*Takt*";

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int VocabularyStatus { get; set; } = 0;

}

// ========================================
// Vocabulary 查询 DTO
// ========================================

/// <summary>
/// Vocabulary 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktVocabularyQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    public string? WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    public int? WordCategory { get; set; }

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    public int? FilterLevel { get; set; }

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    public string? ReplaceText { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? VocabularyStatus { get; set; }

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
// 创建Vocabulary DTO
// ========================================

/// <summary>
/// 创建Vocabulary DTO
/// </summary>
public class TaktVocabularyCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    [Required(ErrorMessage = "敏感词文本（租户内唯一）不能为空")]
    public string WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    public int WordCategory { get; set; } = 0;

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    public int FilterLevel { get; set; } = 0;

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    public string ReplaceText { get; set; } = "*Takt*";

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int VocabularyStatus { get; set; } = 0;

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
// 更新Vocabulary DTO
// ========================================

/// <summary>
/// 更新Vocabulary DTO
/// 继承 TaktVocabularyCreateDto，添加 VocabularyId 字段
/// </summary>
public class TaktVocabularyUpdateDto : TaktVocabularyCreateDto
{
    /// <summary>
    /// VocabularyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VocabularyId { get; set; }

}

// ========================================
// Vocabulary 状态 DTO
// ========================================

/// <summary>
/// Vocabulary 状态更新 DTO
/// </summary>
public class TaktVocabularyStatusDto
{
    /// <summary>
    /// VocabularyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VocabularyId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public int VocabularyStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Vocabulary 导入模板行 DTO
/// </summary>
public class TaktVocabularyTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    public string? WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    public int? WordCategory { get; set; }

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    public int? FilterLevel { get; set; }

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    public string? ReplaceText { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? VocabularyStatus { get; set; }

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
/// Vocabulary 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktVocabularyImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    public string? WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    public int? WordCategory { get; set; }

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    public int? FilterLevel { get; set; }

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    public string? ReplaceText { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? VocabularyStatus { get; set; }

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
/// Vocabulary 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktVocabularyExportDto
{
    /// <summary>
    /// VocabularyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VocabularyId { get; set; }

    /// <summary>
    /// 敏感词文本（租户内唯一）
    /// </summary>
    public string WordText { get; set; } = string.Empty;

    /// <summary>
    /// 词性类别（字典 sys_word_category：1=政治敏感，2=暴力恐怖，3=色情低俗，4=广告营销，5=辱骂歧视）
    /// </summary>
    public int WordCategory { get; set; } = 0;

    /// <summary>
    /// 过滤等级（字典 sys_word_filter_level：1=低，2=中，3=高）
    /// </summary>
    public int FilterLevel { get; set; } = 0;

    /// <summary>
    /// 替换文本（默认 *Takt*）
    /// </summary>
    public string ReplaceText { get; set; } = "*Takt*";

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int VocabularyStatus { get; set; } = 0;

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
