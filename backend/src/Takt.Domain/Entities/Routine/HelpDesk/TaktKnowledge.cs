// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktKnowledge.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台知识库实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 服务台知识库实体
/// </summary>
[SugarTable("takt_routine_help_desk_knowledge", "知识库表")]
[SugarIndex("ix_knowledge_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_category_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CategoryCode), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(KnowledgeStatus), OrderByType.Asc, false)]
[SugarIndex("ix_knowledge_is_published", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(KnowledgeIsPublished), OrderByType.Asc, false)]
public class TaktKnowledge : TaktCompanyEntityBase
{
    /// <summary>
    /// 知识标题
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_title", ColumnDescription = "知识标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string KnowledgeTitle { get; set; } = string.Empty;
    /// <summary>
    /// 知识内容（富文本/HTML）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_content", ColumnDescription = "知识内容", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? KnowledgeContent { get; set; }
    /// <summary>
    /// 知识摘要（简短描述，列表/搜索展示）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_summary", ColumnDescription = "知识摘要", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? KnowledgeSummary { get; set; }
    /// <summary>
    /// 分类编码（业务编码；如 faq/guide）
    /// </summary>
    [SugarColumn(ColumnName = "category_code", ColumnDescription = "分类编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CategoryCode { get; set; }
    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_tags", ColumnDescription = "标签", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? KnowledgeTags { get; set; }
    /// <summary>
    /// 浏览次数
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_view_count", ColumnDescription = "浏览次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int KnowledgeViewCount { get; set; } = 0;
    /// <summary>
    /// 有用评价数
    /// </summary>
    [SugarColumn(ColumnName = "helpful_count", ColumnDescription = "有用评价数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HelpfulCount { get; set; } = 0;
    /// <summary>
    /// 无帮助评价数
    /// </summary>
    [SugarColumn(ColumnName = "unhelpful_count", ColumnDescription = "无帮助评价数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int UnhelpfulCount { get; set; } = 0;
    /// <summary>
    /// 是否已发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_is_published", ColumnDescription = "是否已发布", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int KnowledgeIsPublished { get; set; } = 0;
    /// <summary>
    /// 版本号
    /// </summary>
    [SugarColumn(ColumnName = "version", ColumnDescription = "版本号", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Version { get; set; } = 1;
    /// <summary>
    /// 发布时间
    /// </summary>
    [SugarColumn(ColumnName = "published_at", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PublishedAt { get; set; }
    /// <summary>
    /// 最后修订时间
    /// </summary>
    [SugarColumn(ColumnName = "revised_at", ColumnDescription = "最后修订时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RevisedAt { get; set; }
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 知识状态（字典 routine_knowledge_status；0=草稿 1=已发布 2=已下架）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_status", ColumnDescription = "知识状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int KnowledgeStatus { get; set; } = 0;
    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================
}
