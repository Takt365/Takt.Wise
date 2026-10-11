// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.NewsCenter
// 文件名称：TaktNewsDtos.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：News 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktNews 生成，请按需审阅）
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
// News 响应 DTO
// ========================================

/// <summary>
/// 新闻中心主实体 支持分类、置顶、推荐、社交统计；正文为富文本 HTML；需审批通过后发布（草稿→审批→发布） 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// 对应前端 TaktNewsDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktNewsDto : TaktApprovalDtoBase
{
    /// <summary>
    /// NewsID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
    /// </summary>
    public string NewsCode { get; set; } = string.Empty;

    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    public int NewsCategory { get; set; } = 0;

    /// <summary>
    /// 新闻标题
    /// </summary>
    public string NewsTitle { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    public string? NewsSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? NewsTags { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    public string NewsContent { get; set; } = string.Empty;

    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    public string? NewsCoverImage { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int NewsIsTop { get; set; } = 0;

    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int NewsIsRecommended { get; set; } = 0;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? NewsEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? NewsExpireTime { get; set; }

    /// <summary>
    /// 阅读次数
    /// </summary>
    public int NewsReadCount { get; set; } = 0;

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int NewsLikeCount { get; set; } = 0;

    /// <summary>
    /// 评论次数
    /// </summary>
    public int NewsCommentCount { get; set; } = 0;

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int NewsFavoriteCount { get; set; } = 0;

    /// <summary>
    /// 分享次数
    /// </summary>
    public int NewsShareCount { get; set; } = 0;

    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 发布部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? NewsPublishTime { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int TargetScope { get; set; } = 0;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int NewsStatus { get; set; } = 0;

}

// ========================================
// News 查询 DTO
// ========================================

/// <summary>
/// News 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktNewsQueryDto : TaktPagedQuery
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
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
    /// </summary>
    public string? NewsCode { get; set; } = string.Empty;

    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    public int? NewsCategory { get; set; }

    /// <summary>
    /// 新闻标题
    /// </summary>
    public string? NewsTitle { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    public string? NewsSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? NewsTags { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    public string? NewsContent { get; set; } = string.Empty;

    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    public string? NewsCoverImage { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? NewsIsTop { get; set; }

    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? NewsIsRecommended { get; set; }

    /// <summary>
    /// 生效时间（范围查询-开始）
    /// </summary>
    public DateTime? NewsEffectiveTimeStart { get; set; }

    /// <summary>
    /// 生效时间（范围查询-结束）
    /// </summary>
    public DateTime? NewsEffectiveTimeEnd { get; set; }

    /// <summary>
    /// 失效时间（范围查询-开始）
    /// </summary>
    public DateTime? NewsExpireTimeStart { get; set; }

    /// <summary>
    /// 失效时间（范围查询-结束）
    /// </summary>
    public DateTime? NewsExpireTimeEnd { get; set; }

    /// <summary>
    /// 阅读次数
    /// </summary>
    public int? NewsReadCount { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int? NewsLikeCount { get; set; }

    /// <summary>
    /// 评论次数
    /// </summary>
    public int? NewsCommentCount { get; set; }

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int? NewsFavoriteCount { get; set; }

    /// <summary>
    /// 分享次数
    /// </summary>
    public int? NewsShareCount { get; set; }

    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 发布部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（范围查询-开始）
    /// </summary>
    public DateTime? NewsPublishTimeStart { get; set; }

    /// <summary>
    /// 发布时间（范围查询-结束）
    /// </summary>
    public DateTime? NewsPublishTimeEnd { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int? TargetScope { get; set; }

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? NewsStatus { get; set; }

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
// 创建News DTO
// ========================================

/// <summary>
/// 创建News DTO
/// </summary>
public class TaktNewsCreateDto
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
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
    /// </summary>
    [Required(ErrorMessage = "新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）不能为空")]
    public string NewsCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    public int NewsCategory { get; set; } = 0;

    /// <summary>
    /// 新闻标题
    /// </summary>
    [Required(ErrorMessage = "新闻标题不能为空")]
    public string NewsTitle { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    public string? NewsSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? NewsTags { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    [Required(ErrorMessage = "新闻内容（富文本 HTML；插图随正文存储，无独立附件）不能为空")]
    public string NewsContent { get; set; } = string.Empty;

    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    public string? NewsCoverImage { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int NewsIsTop { get; set; } = 0;

    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int NewsIsRecommended { get; set; } = 0;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? NewsEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? NewsExpireTime { get; set; }

    /// <summary>
    /// 阅读次数
    /// </summary>
    public int NewsReadCount { get; set; } = 0;

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int NewsLikeCount { get; set; } = 0;

    /// <summary>
    /// 评论次数
    /// </summary>
    public int NewsCommentCount { get; set; } = 0;

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int NewsFavoriteCount { get; set; } = 0;

    /// <summary>
    /// 分享次数
    /// </summary>
    public int NewsShareCount { get; set; } = 0;

    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 发布部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? NewsPublishTime { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int TargetScope { get; set; } = 0;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int NewsStatus { get; set; } = 0;

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
// 更新News DTO
// ========================================

/// <summary>
/// 更新News DTO
/// 继承 TaktNewsCreateDto，添加 NewsId 字段
/// </summary>
public class TaktNewsUpdateDto : TaktNewsCreateDto
{
    /// <summary>
    /// NewsID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

}

// ========================================
// News 状态 DTO
// ========================================

/// <summary>
/// News 状态更新 DTO
/// </summary>
public class TaktNewsStatusDto
{
    /// <summary>
    /// NewsID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    [Required(ErrorMessage = "新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）不能为空")]
    public int NewsStatus { get; set; } = 0;
}

// ========================================
// News 排序 DTO
// ========================================

/// <summary>
/// News 排序更新 DTO
/// </summary>
public class TaktNewsSortDto
{
    /// <summary>
    /// NewsID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

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
/// News 导入模板行 DTO
/// </summary>
public class TaktNewsTemplateDto
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
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
    /// </summary>
    public string? NewsCode { get; set; } = string.Empty;

    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    public int? NewsCategory { get; set; }

    /// <summary>
    /// 新闻标题
    /// </summary>
    public string? NewsTitle { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    public string? NewsSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? NewsTags { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    public string? NewsContent { get; set; } = string.Empty;

    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    public string? NewsCoverImage { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? NewsIsTop { get; set; }

    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? NewsIsRecommended { get; set; }

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? NewsEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? NewsExpireTime { get; set; }

    /// <summary>
    /// 阅读次数
    /// </summary>
    public int? NewsReadCount { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int? NewsLikeCount { get; set; }

    /// <summary>
    /// 评论次数
    /// </summary>
    public int? NewsCommentCount { get; set; }

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int? NewsFavoriteCount { get; set; }

    /// <summary>
    /// 分享次数
    /// </summary>
    public int? NewsShareCount { get; set; }

    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 发布部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? NewsPublishTime { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int? TargetScope { get; set; }

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? NewsStatus { get; set; }

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
/// News 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktNewsImportDto
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
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
    /// </summary>
    public string? NewsCode { get; set; } = string.Empty;

    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    public int? NewsCategory { get; set; }

    /// <summary>
    /// 新闻标题
    /// </summary>
    public string? NewsTitle { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    public string? NewsSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? NewsTags { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    public string? NewsContent { get; set; } = string.Empty;

    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    public string? NewsCoverImage { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? NewsIsTop { get; set; }

    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? NewsIsRecommended { get; set; }

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? NewsEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? NewsExpireTime { get; set; }

    /// <summary>
    /// 阅读次数
    /// </summary>
    public int? NewsReadCount { get; set; }

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int? NewsLikeCount { get; set; }

    /// <summary>
    /// 评论次数
    /// </summary>
    public int? NewsCommentCount { get; set; }

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int? NewsFavoriteCount { get; set; }

    /// <summary>
    /// 分享次数
    /// </summary>
    public int? NewsShareCount { get; set; }

    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 发布部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? NewsPublishTime { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int? TargetScope { get; set; }

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? NewsStatus { get; set; }

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
/// News 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktNewsExportDto
{
    /// <summary>
    /// NewsID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }

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
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻）
    /// </summary>
    public string NewsCode { get; set; } = string.Empty;

    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    public int NewsCategory { get; set; } = 0;

    /// <summary>
    /// 新闻标题
    /// </summary>
    public string NewsTitle { get; set; } = string.Empty;

    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    public string? NewsSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? NewsTags { get; set; } = string.Empty;

    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    public string NewsContent { get; set; } = string.Empty;

    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    public string? NewsCoverImage { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int NewsIsTop { get; set; } = 0;

    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int NewsIsRecommended { get; set; } = 0;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? NewsEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? NewsExpireTime { get; set; }

    /// <summary>
    /// 阅读次数
    /// </summary>
    public int NewsReadCount { get; set; } = 0;

    /// <summary>
    /// 点赞次数
    /// </summary>
    public int NewsLikeCount { get; set; } = 0;

    /// <summary>
    /// 评论次数
    /// </summary>
    public int NewsCommentCount { get; set; } = 0;

    /// <summary>
    /// 收藏次数
    /// </summary>
    public int NewsFavoriteCount { get; set; } = 0;

    /// <summary>
    /// 分享次数
    /// </summary>
    public int NewsShareCount { get; set; } = 0;

    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 发布部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? NewsPublishTime { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int TargetScope { get; set; } = 0;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int NewsStatus { get; set; } = 0;

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
