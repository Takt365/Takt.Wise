// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Ecn
// 文件名称：TaktEcGijutsu.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主表实体。技术阶段一①：主表→附件→明细→自动生成通知；通知到达各部门后各部门执行，技术看板/批次监控
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主表实体（技术阶段一 ①）。流程：TaktEcGijutsu → TaktEcAttachment → TaktEcDetail → 系统自动生成 TaktEcNotification 并派发；
/// 通知到达各部门后各部门在 TaktEcExec* 填报执行；技术通过看板/批次等监控执行情况。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_gijutsu", "设变技术课主表")]
[SugarIndex("ix_ec_gijutsu_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_gijutsu_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(EcCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_gijutsu_change_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ChangeStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_gijutsu_ec_entry_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcEntryDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_gijutsu_ec_issue_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcIssueDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_gijutsu_ec_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcStatus), OrderByType.Asc, false)]
public class TaktEcGijutsu : TaktCompanyEntityBase
{

    /// <summary>
  /// 设变单号（唯一）
  /// </summary>
  [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", Length = 10, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

  /// <summary>
  /// 发行日期
  /// </summary>
  [SugarColumn(ColumnName = "ec_issue_date", ColumnDescription = "发行日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）
    /// </summary>
    [SugarColumn(ColumnName = "change_status", ColumnDescription = "变更状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "5")]
    public int ChangeStatus { get; set; } = 5;

    /// <summary>
    /// 设变标题
    /// </summary>
    [SugarColumn(ColumnName = "ec_title", ColumnDescription = "设变标题", Length = 500, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "ec_content", ColumnDescription = "设变内容", ColumnDataType = "ntext", IsNullable = false)]
    public string EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    [SugarColumn(ColumnName = "ec_leader", ColumnDescription = "设变担当", Length = 80, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    [SugarColumn(ColumnName = "ec_loss_amount", ColumnDescription = "损失金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal EcLossAmount { get; set; } = 0;

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
    /// </summary>
    [SugarColumn(ColumnName = "ec_scope", ColumnDescription = "实施范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EcScope { get; set; } = 1;

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种
    /// </summary>
    [SugarColumn(ColumnName = "ec_dept_scope", ColumnDescription = "部门范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 录入日期
    /// </summary>
    [SugarColumn(ColumnName = "ec_entry_date", ColumnDescription = "录入日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成；由各部门执行表自动回写：任一有输入→2，全部填写→3，否则→1）
    /// </summary>
    [SugarColumn(ColumnName = "ec_status", ColumnDescription = "设变状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EcStatus { get; set; } = 1;

    /// <summary>
    /// 设变明细列表（技术阶段一：③，BOM/料号变更行）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcDetail.EcGijutsuId))]
    public List<TaktEcDetail>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcAttachment.EcGijutsuId))]
    public List<TaktEcAttachment>? Attachments { get; set; }

    /// <summary>
    /// 设变通知列表（技术阶段一：④，发行通知至各部门）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEcNotification.EcGijutsuId))]
    public List<TaktEcNotification>? Notifications { get; set; }
}
