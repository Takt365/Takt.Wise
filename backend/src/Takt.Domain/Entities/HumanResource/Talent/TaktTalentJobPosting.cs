// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Talent
// 文件名称：TaktTalentJobPosting.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：职位发布（人才链路：用人需求→职位发布→录用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Talent;

/// <summary>
/// 职位发布（业务发布单，非审批单；状态见 posting_status）
/// </summary>
[SugarTable("takt_human_resource_talent_job_posting", "职位发布表")]
[SugarIndex("ix_talent_job_posting_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_talent_job_posting_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PostingCode), OrderByType.Asc, true)]
[SugarIndex("ix_talent_job_posting_staffing", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StaffingRequirementId), OrderByType.Asc, false)]
public class TaktTalentJobPosting : TaktCompanyEntityBase
{
    /// <summary>
    /// 用人需求（选项 TaktTalentStaffingRequirements/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "staffing_requirement_id", ColumnDescription = "用人需求ID", ColumnDataType = "bigint", IsNullable = false)]
    public long StaffingRequirementId { get; set; }
    /// <summary>
    /// 发布编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "posting_code", ColumnDescription = "发布编码", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string PostingCode { get; set; } = string.Empty;
    /// <summary>
    /// 职位标题
    /// </summary>
    [SugarColumn(ColumnName = "talent_job_posting_title", ColumnDescription = "职位标题", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string TalentJobPostingTitle { get; set; } = string.Empty;
    /// <summary>
    /// 职位发布日期
    /// </summary>
    [SugarColumn(ColumnName = "publish_date", ColumnDescription = "职位发布日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PublishDate { get; set; }
    /// <summary>
    /// 招聘开放日期
    /// </summary>
    [SugarColumn(ColumnName = "open_date", ColumnDescription = "招聘开放日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OpenDate { get; set; }
    /// <summary>
    /// 招聘关闭日期
    /// </summary>
    [SugarColumn(ColumnName = "close_date", ColumnDescription = "招聘关闭日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CloseDate { get; set; }
    /// <summary>
    /// 发布渠道（字典 humanresource_talent_publish_channel；0=官网 1=招聘网站 2=内推 3=校园 9=其他）
    /// </summary>
    [SugarColumn(ColumnName = "publish_channel", ColumnDescription = "发布渠道", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PublishChannel { get; set; }
    /// <summary>
    /// 发布说明
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "发布说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }
    /// <summary>
    /// 发布状态（字典 humanresource_talent_job_posting_status；0=草稿 1=招聘中 2=已暂停 3=已关闭）
    /// </summary>
    [SugarColumn(ColumnName = "posting_status", ColumnDescription = "发布状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PostingStatus { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 用人需求
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(StaffingRequirementId))]
    public TaktTalentStaffingRequirement? StaffingRequirement { get; set; }

    /// <summary>
    /// 录用信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktTalentOffer.JobPostingId))]
    public List<TaktTalentOffer>? TalentOffers { get; set; }
}
