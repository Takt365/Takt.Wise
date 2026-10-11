#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepMedia.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工步多媒体（JPG/PNG/MP4/PDF/3D）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 工步多媒体实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_step_media", "SOP工步多媒体表")]
[SugarIndex("ix_sop_step_media_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_step_media_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_step_media_step", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StepId), OrderByType.Asc, false)]
public class TaktSopStepMedia : TaktCompanyEntityBase
{
    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "step_id", ColumnDescription = "工步ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    [SugarColumn(ColumnName = "media_type", ColumnDescription = "媒体类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MediaType { get; set; } = 1;

    /// <summary>
    /// 文件 URL
    /// </summary>
    [SugarColumn(ColumnName = "file_url", ColumnDescription = "文件URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    [SugarColumn(ColumnName = "file_ext", ColumnDescription = "文件扩展名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? FileExt { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工步
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(StepId))]
    public TaktSopStep? Step { get; set; }
}
