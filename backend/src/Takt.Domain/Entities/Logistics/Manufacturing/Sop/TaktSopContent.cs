#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopContent.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 多语言正文（zh-CN/en-US/ja-JP/zh-HK），含工步树
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 多语言正文实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_content", "SOP多语言正文表")]
[SugarIndex("ix_sop_content_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_content_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_content_culture_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RevisionId), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, true)]
public class TaktSopContent : TaktCompanyEntityBase
{
    /// <summary>
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "revision_id", ColumnDescription = "版本ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sop_id", ColumnDescription = "SOP主档ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    [SugarColumn(ColumnName = "content_title", ColumnDescription = "正文标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ContentTitle { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RevisionId))]
    public TaktSopRevision? Revision { get; set; }

    /// <summary>
    /// 工步列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopStep.ContentId))]
    public List<TaktSopStep>? Steps { get; set; }
}
