// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.NewsCenter
// 文件名称：TaktNewsCommentDtos.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：NewsComment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktNewsComment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.NewsCenter;

// ========================================
// NewsComment 响应 DTO
// ========================================

/// <summary>
/// 新闻中心评论实体 支持多级回复；需审批通过后展示 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// 对应前端 TaktNewsCommentDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktNewsCommentDto : TaktApprovalDtoBase
{
    /// <summary>
    /// NewsCommentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsCommentId { get; set; }

    /// <summary>
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 新闻 名称（填充字段）
    /// </summary>
    public string? NewsName { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 评论人姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论人头像 URL（冗余字段，便于查询）
    /// </summary>
    public string? UserAvatar { get; set; } = string.Empty;

    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }

    /// <summary>
    /// 被回复人姓名（冗余字段，便于查询）
    /// </summary>
    public string? ReplyToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论内容
    /// </summary>
    public string CommentContent { get; set; } = string.Empty;

    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CommentTime { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int NewsCommentLikeCount { get; set; } = 0;

    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    public int ReplyCount { get; set; } = 0;

    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    public int CommentLevel { get; set; } = 0;

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    public int CommentStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 新闻（主表）
    /// （主表：TaktNews）
    /// </summary>
    public TaktNewsDto? News { get; set; }

}

// ========================================
// NewsComment 树形响应 DTO
// ========================================

/// <summary>
/// NewsComment 树形列表/树选择 DTO（含子节点）
/// 对应 GetNewsCommentTreeAsync 等接口
/// </summary>
public class TaktNewsCommentTreeDto : TaktNewsCommentDto
{
    /// <summary>
    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
    /// </summary>
    public List<TaktNewsCommentTreeDto>? Children { get; set; }
}

// ========================================
// NewsComment 查询 DTO
// ========================================

/// <summary>
/// NewsComment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktNewsCommentQueryDto : TaktPagedQuery
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
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? NewsId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 评论人姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论人头像 URL（冗余字段，便于查询）
    /// </summary>
    public string? UserAvatar { get; set; } = string.Empty;

    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }

    /// <summary>
    /// 被回复人姓名（冗余字段，便于查询）
    /// </summary>
    public string? ReplyToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论内容
    /// </summary>
    public string? CommentContent { get; set; } = string.Empty;

    /// <summary>
    /// 评论时间（范围查询-开始）
    /// </summary>
    public DateTime? CommentTimeStart { get; set; }

    /// <summary>
    /// 评论时间（范围查询-结束）
    /// </summary>
    public DateTime? CommentTimeEnd { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int? NewsCommentLikeCount { get; set; }

    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    public int? ReplyCount { get; set; }

    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    public int? CommentLevel { get; set; }

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    public int? CommentStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建NewsComment DTO
// ========================================

/// <summary>
/// 创建NewsComment DTO
/// </summary>
public class TaktNewsCommentCreateDto
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
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 评论人姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论人头像 URL（冗余字段，便于查询）
    /// </summary>
    public string? UserAvatar { get; set; } = string.Empty;

    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }

    /// <summary>
    /// 被回复人姓名（冗余字段，便于查询）
    /// </summary>
    public string? ReplyToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论内容
    /// </summary>
    [Required(ErrorMessage = "评论内容不能为空")]
    public string CommentContent { get; set; } = string.Empty;

    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CommentTime { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int NewsCommentLikeCount { get; set; } = 0;

    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    public int ReplyCount { get; set; } = 0;

    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    public int CommentLevel { get; set; } = 0;

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    public int CommentStatus { get; set; } = 0;

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
// 更新NewsComment DTO
// ========================================

/// <summary>
/// 更新NewsComment DTO
/// 继承 TaktNewsCommentCreateDto，添加 NewsCommentId 字段
/// </summary>
public class TaktNewsCommentUpdateDto : TaktNewsCommentCreateDto
{
    /// <summary>
    /// NewsCommentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsCommentId { get; set; }

}

// ========================================
// NewsComment 状态 DTO
// ========================================

/// <summary>
/// NewsComment 状态更新 DTO
/// </summary>
public class TaktNewsCommentStatusDto
{
    /// <summary>
    /// NewsCommentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsCommentId { get; set; }

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    [Required(ErrorMessage = "评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）不能为空")]
    public int CommentStatus { get; set; } = 0;
}

// ========================================
// NewsComment 作废 DTO
// ========================================

/// <summary>
/// NewsComment 作废/撤销作废 DTO
/// </summary>
public class TaktNewsCommentObsoleteDto
{
    /// <summary>
    /// NewsCommentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsCommentId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// NewsComment 导入模板行 DTO
/// </summary>
public class TaktNewsCommentTemplateDto
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
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? NewsId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 评论人姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论人头像 URL（冗余字段，便于查询）
    /// </summary>
    public string? UserAvatar { get; set; } = string.Empty;

    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }

    /// <summary>
    /// 被回复人姓名（冗余字段，便于查询）
    /// </summary>
    public string? ReplyToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论内容
    /// </summary>
    public string? CommentContent { get; set; } = string.Empty;

    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime? CommentTime { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int? NewsCommentLikeCount { get; set; }

    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    public int? ReplyCount { get; set; }

    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    public int? CommentLevel { get; set; }

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    public int? CommentStatus { get; set; }

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
/// NewsComment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktNewsCommentImportDto
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
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? NewsId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 评论人姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论人头像 URL（冗余字段，便于查询）
    /// </summary>
    public string? UserAvatar { get; set; } = string.Empty;

    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }

    /// <summary>
    /// 被回复人姓名（冗余字段，便于查询）
    /// </summary>
    public string? ReplyToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论内容
    /// </summary>
    public string? CommentContent { get; set; } = string.Empty;

    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime? CommentTime { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int? NewsCommentLikeCount { get; set; }

    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    public int? ReplyCount { get; set; }

    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    public int? CommentLevel { get; set; }

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    public int? CommentStatus { get; set; }

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
/// NewsComment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktNewsCommentExportDto
{
    /// <summary>
    /// NewsCommentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsCommentId { get; set; }

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
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 评论人姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论人头像 URL（冗余字段，便于查询）
    /// </summary>
    public string? UserAvatar { get; set; } = string.Empty;

    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }

    /// <summary>
    /// 被回复人姓名（冗余字段，便于查询）
    /// </summary>
    public string? ReplyToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 评论内容
    /// </summary>
    public string CommentContent { get; set; } = string.Empty;

    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CommentTime { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int NewsCommentLikeCount { get; set; } = 0;

    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    public int ReplyCount { get; set; } = 0;

    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    public int CommentLevel { get; set; } = 0;

    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    public int CommentStatus { get; set; } = 0;

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
