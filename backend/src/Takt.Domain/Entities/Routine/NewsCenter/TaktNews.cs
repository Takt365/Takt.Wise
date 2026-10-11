// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.NewsCenter
// 文件名称：TaktNews.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心主实体，支持分类、置顶、推荐、社交统计；正文为富文本；需审批通过后发布
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.NewsCenter;

/// <summary>
/// 新闻中心主实体
/// 支持分类、置顶、推荐、社交统计；正文为富文本 HTML；需审批通过后发布（草稿→审批→发布）
/// 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// </summary>
[SugarTable("takt_routine_news_center", "新闻中心表")]
[SugarIndex("ix_news_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_news_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_news_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsCode), OrderByType.Asc, true)]
[SugarIndex("ix_news_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsCategory), OrderByType.Asc, false)]
[SugarIndex("ix_news_publish_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsPublishTime), OrderByType.Desc, false)]
[SugarIndex("ix_news_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsStatus), OrderByType.Asc, false)]
[SugarIndex("ix_news_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktNews : TaktApprovalEntityBase
{
    /// <summary>
    /// 新闻编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 新闻编码规则生成并展示，非手输；单据类型菜单：新闻中心）
    /// </summary>
    [SugarColumn(ColumnName = "news_code", ColumnDescription = "新闻编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string NewsCode { get; set; } = string.Empty;
    /// <summary>
    /// 新闻分类（字典 sys_news_type；0=公司新闻 1=行业动态 2=技术分享 3=产品发布 4=活动资讯 5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "news_category", ColumnDescription = "新闻分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsCategory { get; set; } = 0;
    /// <summary>
    /// 新闻标题
    /// </summary>
    [SugarColumn(ColumnName = "news_title", ColumnDescription = "新闻标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string NewsTitle { get; set; } = string.Empty;
    /// <summary>
    /// 新闻摘要（用于列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "news_summary", ColumnDescription = "新闻摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? NewsSummary { get; set; }
    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "news_tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? NewsTags { get; set; }
    /// <summary>
    /// 新闻内容（富文本 HTML；插图随正文存储，无独立附件）
    /// </summary>
    [SugarColumn(ColumnName = "news_content", ColumnDescription = "新闻内容", ColumnDataType = "ntext", IsNullable = false)]
    public string NewsContent { get; set; } = string.Empty;
    /// <summary>
    /// 新闻封面图片 URL
    /// </summary>
    [SugarColumn(ColumnName = "news_cover_image", ColumnDescription = "新闻封面图片URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? NewsCoverImage { get; set; }
    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "news_is_top", ColumnDescription = "置顶", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsIsTop { get; set; } = 0;
    /// <summary>
    /// 推荐（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "news_is_recommended", ColumnDescription = "推荐", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsIsRecommended { get; set; } = 0;
    /// <summary>
    /// 生效时间
    /// </summary>
    [SugarColumn(ColumnName = "news_effective_time", ColumnDescription = "生效时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NewsEffectiveTime { get; set; }
    /// <summary>
    /// 失效时间
    /// </summary>
    [SugarColumn(ColumnName = "news_expire_time", ColumnDescription = "失效时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NewsExpireTime { get; set; }
    /// <summary>
    /// 阅读次数
    /// </summary>
    [SugarColumn(ColumnName = "news_read_count", ColumnDescription = "阅读次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsReadCount { get; set; } = 0;
    /// <summary>
    /// 点赞次数
    /// </summary>
    [SugarColumn(ColumnName = "news_like_count", ColumnDescription = "点赞次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsLikeCount { get; set; } = 0;
    /// <summary>
    /// 评论次数
    /// </summary>
    [SugarColumn(ColumnName = "news_comment_count", ColumnDescription = "评论次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsCommentCount { get; set; } = 0;
    /// <summary>
    /// 收藏次数
    /// </summary>
    [SugarColumn(ColumnName = "news_favorite_count", ColumnDescription = "收藏次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsFavoriteCount { get; set; } = 0;
    /// <summary>
    /// 分享次数
    /// </summary>
    [SugarColumn(ColumnName = "news_share_count", ColumnDescription = "分享次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsShareCount { get; set; } = 0;
    /// <summary>
    /// 发布部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "发布部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }
    /// <summary>
    /// 发布部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "发布部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "publisher_id", ColumnDescription = "发布人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }
    /// <summary>
    /// 发布人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "publisher_name", ColumnDescription = "发布人姓名", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PublisherName { get; set; } = string.Empty;
    /// <summary>
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnName = "news_publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NewsPublishTime { get; set; }
    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    [SugarColumn(ColumnName = "target_scope", ColumnDescription = "目标范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TargetScope { get; set; } = 0;
    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    [SugarColumn(ColumnName = "target_departments", ColumnDescription = "目标部门编码", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? TargetDepartments { get; set; }
    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    [SugarColumn(ColumnName = "target_users", ColumnDescription = "目标用户名", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? TargetUsers { get; set; }
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 新闻状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    [SugarColumn(ColumnName = "news_status", ColumnDescription = "新闻状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 新闻评论列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsComment.NewsId))]
    public List<TaktNewsComment>? Comments { get; set; }
    /// <summary>
    /// 新闻点赞记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsLike.NewsId))]
    public List<TaktNewsLike>? Likes { get; set; }
    /// <summary>
    /// 新闻阅读记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsRead.NewsId))]
    public List<TaktNewsRead>? Reads { get; set; }
    /// <summary>
    /// 新闻收藏记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsFavorite.NewsId))]
    public List<TaktNewsFavorite>? Favorites { get; set; }
    /// <summary>
    /// 新闻分享记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsShare.NewsId))]
    public List<TaktNewsShare>? Shares { get; set; }
}
