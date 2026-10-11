// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktKnowledgeDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Knowledge 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktKnowledge 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

// ========================================
// Knowledge 响应 DTO
// ========================================

/// <summary>
/// 服务台知识库实体
/// 对应前端 TaktKnowledgeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktKnowledgeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// KnowledgeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 知识标题
    /// </summary>
    public string KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    public string? KnowledgeContent { get; set; } = string.Empty;

    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    public string? KnowledgeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（如 faq/guide 等）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? KnowledgeTags { get; set; } = string.Empty;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int KnowledgeViewCount { get; set; } = 0;

    /// <summary>
    /// 有用评价数
    /// </summary>
    public int HelpfulCount { get; set; } = 0;

    /// <summary>
    /// 无帮助评价数
    /// </summary>
    public int UnhelpfulCount { get; set; } = 0;

    /// <summary>
    /// 是否已发布（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int KnowledgeIsPublished { get; set; } = 0;

    /// <summary>
    /// 版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// 最后修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    public int KnowledgeStatus { get; set; } = 0;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

}

// ========================================
// Knowledge 查询 DTO
// ========================================

/// <summary>
/// Knowledge 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktKnowledgeQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 知识标题
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    public string? KnowledgeContent { get; set; } = string.Empty;

    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    public string? KnowledgeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（如 faq/guide 等）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? KnowledgeTags { get; set; } = string.Empty;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? KnowledgeViewCount { get; set; }

    /// <summary>
    /// 有用评价数
    /// </summary>
    public int? HelpfulCount { get; set; }

    /// <summary>
    /// 无帮助评价数
    /// </summary>
    public int? UnhelpfulCount { get; set; }

    /// <summary>
    /// 是否已发布（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int? KnowledgeIsPublished { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 发布时间（范围查询-开始）
    /// </summary>
    public DateTime? PublishedAtStart { get; set; }

    /// <summary>
    /// 发布时间（范围查询-结束）
    /// </summary>
    public DateTime? PublishedAtEnd { get; set; }

    /// <summary>
    /// 最后修订时间（范围查询-开始）
    /// </summary>
    public DateTime? RevisedAtStart { get; set; }

    /// <summary>
    /// 最后修订时间（范围查询-结束）
    /// </summary>
    public DateTime? RevisedAtEnd { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    public int? KnowledgeStatus { get; set; }

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
// 创建Knowledge DTO
// ========================================

/// <summary>
/// 创建Knowledge DTO
/// </summary>
public class TaktKnowledgeCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 知识标题
    /// </summary>
    [Required(ErrorMessage = "知识标题不能为空")]
    public string KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    public string? KnowledgeContent { get; set; } = string.Empty;

    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    public string? KnowledgeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（如 faq/guide 等）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? KnowledgeTags { get; set; } = string.Empty;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int KnowledgeViewCount { get; set; } = 0;

    /// <summary>
    /// 有用评价数
    /// </summary>
    public int HelpfulCount { get; set; } = 0;

    /// <summary>
    /// 无帮助评价数
    /// </summary>
    public int UnhelpfulCount { get; set; } = 0;

    /// <summary>
    /// 是否已发布（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int KnowledgeIsPublished { get; set; } = 0;

    /// <summary>
    /// 版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// 最后修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    public int KnowledgeStatus { get; set; } = 0;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
// 更新Knowledge DTO
// ========================================

/// <summary>
/// 更新Knowledge DTO
/// 继承 TaktKnowledgeCreateDto，添加 KnowledgeId 字段
/// </summary>
public class TaktKnowledgeUpdateDto : TaktKnowledgeCreateDto
{
    /// <summary>
    /// KnowledgeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

}

// ========================================
// Knowledge 状态 DTO
// ========================================

/// <summary>
/// Knowledge 状态更新 DTO
/// </summary>
public class TaktKnowledgeStatusDto
{
    /// <summary>
    /// KnowledgeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    [Required(ErrorMessage = "知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）不能为空")]
    public int KnowledgeStatus { get; set; } = 0;
}

// ========================================
// Knowledge 排序 DTO
// ========================================

/// <summary>
/// Knowledge 排序更新 DTO
/// </summary>
public class TaktKnowledgeSortDto
{
    /// <summary>
    /// KnowledgeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

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
/// Knowledge 导入模板行 DTO
/// </summary>
public class TaktKnowledgeTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 知识标题
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    public string? KnowledgeContent { get; set; } = string.Empty;

    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    public string? KnowledgeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（如 faq/guide 等）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? KnowledgeTags { get; set; } = string.Empty;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? KnowledgeViewCount { get; set; }

    /// <summary>
    /// 有用评价数
    /// </summary>
    public int? HelpfulCount { get; set; }

    /// <summary>
    /// 无帮助评价数
    /// </summary>
    public int? UnhelpfulCount { get; set; }

    /// <summary>
    /// 是否已发布（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int? KnowledgeIsPublished { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// 最后修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    public int? KnowledgeStatus { get; set; }

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
/// Knowledge 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktKnowledgeImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 知识标题
    /// </summary>
    public string? KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    public string? KnowledgeContent { get; set; } = string.Empty;

    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    public string? KnowledgeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（如 faq/guide 等）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? KnowledgeTags { get; set; } = string.Empty;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? KnowledgeViewCount { get; set; }

    /// <summary>
    /// 有用评价数
    /// </summary>
    public int? HelpfulCount { get; set; }

    /// <summary>
    /// 无帮助评价数
    /// </summary>
    public int? UnhelpfulCount { get; set; }

    /// <summary>
    /// 是否已发布（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int? KnowledgeIsPublished { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// 最后修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    public int? KnowledgeStatus { get; set; }

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
/// Knowledge 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktKnowledgeExportDto
{
    /// <summary>
    /// KnowledgeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long KnowledgeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 知识标题
    /// </summary>
    public string KnowledgeTitle { get; set; } = string.Empty;

    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    public string? KnowledgeContent { get; set; } = string.Empty;

    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    public string? KnowledgeSummary { get; set; } = string.Empty;

    /// <summary>
    /// 分类编码（如 faq/guide 等）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? KnowledgeTags { get; set; } = string.Empty;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int KnowledgeViewCount { get; set; } = 0;

    /// <summary>
    /// 有用评价数
    /// </summary>
    public int HelpfulCount { get; set; } = 0;

    /// <summary>
    /// 无帮助评价数
    /// </summary>
    public int UnhelpfulCount { get; set; } = 0;

    /// <summary>
    /// 是否已发布（字典 sys_yes_no；1=是 0=否）
    /// </summary>
    public int KnowledgeIsPublished { get; set; } = 0;

    /// <summary>
    /// 版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// 最后修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    public int KnowledgeStatus { get; set; } = 0;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

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
