// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.Announcement
// 文件名称：TaktAnnouncement.cs
// 创建时间：2026-05-18
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知实体，用于发布系统公告、通知、新闻等
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.Announcement;

/// <summary>
/// 公告通知实体
/// 用于发布系统公告、通知等信息；支持富文本、附件、置顶、定时发布；需审批通过后发布
/// 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// </summary>
[SugarTable("takt_routine_announcement", "公告通知表")]
[SugarIndex("ix_announcement_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_announcement_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_announcement_publish_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PublishTime), OrderByType.Desc, false)]
[SugarIndex("ix_announcement_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AnnouncementCode), OrderByType.Asc, true)]
public class TaktAnnouncement : TaktApprovalEntityBase
{
    /// <summary>
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    [SugarColumn(ColumnName = "announcement_code", ColumnDescription = "公告编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string AnnouncementCode { get; set; } = string.Empty;
    /// <summary>
    /// 公告标题
    /// </summary>
    [SugarColumn(ColumnName = "announcement_title", ColumnDescription = "公告标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string AnnouncementTitle { get; set; } = string.Empty;
    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    [SugarColumn(ColumnName = "announcement_type", ColumnDescription = "公告类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int AnnouncementType { get; set; } = 2;
    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "content", ColumnDescription = "公告内容", ColumnDataType = "ntext", IsNullable = false)]
    public string Content { get; set; } = string.Empty;
    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    [SugarColumn(ColumnName = "summary", ColumnDescription = "公告摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Summary { get; set; }
    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Tags { get; set; }
    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FileName { get; set; }
    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? AccessUrl { get; set; }
    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PublishTime { get; set; }
    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_scheduled", ColumnDescription = "定时发布", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsScheduled { get; set; } = 0;
    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_top", ColumnDescription = "置顶", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsTop { get; set; } = 0;
    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "top_priority", ColumnDescription = "置顶优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TopPriority { get; set; } = 0;
    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    [SugarColumn(ColumnName = "expire_time", ColumnDescription = "过期时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpireTime { get; set; }
    /// <summary>
    /// 查看次数
    /// </summary>
    [SugarColumn(ColumnName = "view_count", ColumnDescription = "查看次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ViewCount { get; set; } = 0;
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
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    [SugarColumn(ColumnName = "announcement_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AnnouncementStatus { get; set; } = 0;
}
