// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.NewsCenter
// 文件名称：TaktNewsLike.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心点赞记录实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.NewsCenter;

/// <summary>
/// 新闻中心点赞记录实体
/// </summary>
[SugarTable("takt_routine_news_center_like", "新闻中心点赞记录表")]
[SugarIndex("ix_news_like_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_news_like_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_news_like_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsId), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("ix_news_like_news_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NewsId), OrderByType.Asc, false)]
[SugarIndex("ix_news_like_user_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
[SugarIndex("ix_news_like_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LikeTime), OrderByType.Desc, false)]
public class TaktNewsLike : TaktCompanyEntityBase
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
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }
    /// <summary>
    /// 用户姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户姓名", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 点赞时间
    /// </summary>
    [SugarColumn(ColumnName = "like_time", ColumnDescription = "点赞时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime LikeTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
    /// <summary>
    /// 新闻（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(NewsId))]
    public TaktNews? News { get; set; }
}
