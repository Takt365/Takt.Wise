// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.NewsCenter
// 文件名称：TaktNewsComment.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论实体，支持多级回复与审核
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.NewsCenter;

/// <summary>
/// 新闻中心评论实体
/// 支持多级回复；需审批通过后展示
/// 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// </summary>
[SugarTable("takt_routine_news_center_comment", "新闻中心评论表")]
[SugarIndex("ix_news_comment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_news_comment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_news_comment_news_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsId), OrderByType.Asc, false)]
[SugarIndex("ix_news_comment_parent_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
[SugarIndex("ix_news_comment_user_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
[SugarIndex("ix_news_comment_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CommentTime), OrderByType.Desc, false)]
[SugarIndex("ix_news_comment_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_news_comment_approval_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
public class TaktNewsComment : TaktApprovalEntityBase
{
    /// <summary>
    /// 新闻 ID（选项 TaktNews/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "news_id", ColumnDescription = "新闻ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long NewsId { get; set; }
    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;
    /// <summary>
    /// 父评论 ID（选项 TaktNewsComments/options；0 表示顶级评论，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父评论ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; } = 0;
    /// <summary>
    /// 评论人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "评论人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }
    /// <summary>
    /// 评论人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "评论人姓名", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 评论人头像 URL（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_avatar", ColumnDescription = "评论人头像URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? UserAvatar { get; set; }
    /// <summary>
    /// 被回复人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "reply_to_user_id", ColumnDescription = "被回复人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReplyToUserId { get; set; }
    /// <summary>
    /// 被回复人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "reply_to_user_name", ColumnDescription = "被回复人姓名", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ReplyToUserName { get; set; }
    /// <summary>
    /// 评论内容
    /// </summary>
    [SugarColumn(ColumnName = "comment_content", ColumnDescription = "评论内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
    public string CommentContent { get; set; } = string.Empty;
    /// <summary>
    /// 评论时间
    /// </summary>
    [SugarColumn(ColumnName = "comment_time", ColumnDescription = "评论时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CommentTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 点赞次数
    /// </summary>
    [SugarColumn(ColumnName = "news_comment_like_count", ColumnDescription = "点赞次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NewsCommentLikeCount { get; set; } = 0;
    /// <summary>
    /// 回复次数（子评论数量）
    /// </summary>
    [SugarColumn(ColumnName = "reply_count", ColumnDescription = "回复次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReplyCount { get; set; } = 0;
    /// <summary>
    /// 评论层级（0=顶级，最多 3 级）
    /// </summary>
    [SugarColumn(ColumnName = "comment_level", ColumnDescription = "评论层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CommentLevel { get; set; } = 0;
    /// <summary>
    /// 评论展示状态（字典 routine_news_center_comment_status；0=待展示 1=已展示 2=已隐藏）
    /// </summary>
    [SugarColumn(ColumnName = "comment_status", ColumnDescription = "评论状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CommentStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 新闻（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(NewsId))]
    public TaktNews? News { get; set; }
    /// <summary>
    /// 评论点赞记录列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktNewsCommentLike.CommentId))]
    public List<TaktNewsCommentLike>? Likes { get; set; }
}
