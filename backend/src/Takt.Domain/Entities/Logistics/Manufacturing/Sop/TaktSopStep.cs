#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStep.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工步（作业说明、安全弹窗）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 工步实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_step", "SOP工步表")]
[SugarIndex("ix_sop_step_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_step_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_step_content", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ContentId), OrderByType.Asc, false)]
public class TaktSopStep : TaktCompanyEntityBase
{
    /// <summary>
    /// 正文 ID（选项 TaktSopContents/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "content_id", ColumnDescription = "正文ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ContentId { get; set; }

    /// <summary>
    /// 工步序号
    /// </summary>
    [SugarColumn(ColumnName = "step_no", ColumnDescription = "工步序号", ColumnDataType = "int", IsNullable = false)]
    public int StepNo { get; set; }

    /// <summary>
    /// 工步标题
    /// </summary>
    [SugarColumn(ColumnName = "step_title", ColumnDescription = "工步标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string StepTitle { get; set; } = string.Empty;

    /// <summary>
    /// 作业说明
    /// </summary>
    [SugarColumn(ColumnName = "step_description", ColumnDescription = "作业说明", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? StepDescription { get; set; }

    /// <summary>
    /// 安全警示
    /// </summary>
    [SugarColumn(ColumnName = "safety_alert", ColumnDescription = "安全警示", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? SafetyAlert { get; set; }

    /// <summary>
    /// 弹窗（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "safety_popup_required", ColumnDescription = "弹窗", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SafetyPopupRequired { get; set; } = 0;

    /// <summary>
    /// 正文
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ContentId))]
    public TaktSopContent? Content { get; set; }

    /// <summary>
    /// 多媒体
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopStepMedia.StepId))]
    public List<TaktSopStepMedia>? MediaList { get; set; }

    /// <summary>
    /// 检验项目
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopStepCheckItem.StepId))]
    public List<TaktSopStepCheckItem>? CheckItems { get; set; }
}
