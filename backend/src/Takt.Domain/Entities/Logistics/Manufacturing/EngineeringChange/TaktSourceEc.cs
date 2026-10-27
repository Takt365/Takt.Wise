// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEc.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主表实体，存储来源系统设变主数据及主子表关联。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源明细列表
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_source", "设变来源主表")]
[SugarIndex("ix_source_ec_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_source_ec_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_source_ec_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SourceEcCode), OrderByType.Asc, true)]
[SugarIndex("ix_source_ec_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_source_ec_issue_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, nameof(SourceIssueDate), OrderByType.Desc, false)]
public class TaktSourceEc : TaktCompanyEntityBase
{

    /// <summary>
    /// 设变号码（前端表单选择编码规则后自动通过 TaktNumbering 设变号码规则生成并展示，非手输；单据类型菜单：设变来源）
    /// </summary>
    [SugarColumn(ColumnName = "source_ec_code", ColumnDescription = "设变号码", Length = 6, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceEcCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    [SugarColumn(ColumnName = "source_model", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false, DefaultValue = "ALL")]
    public string SourceModel { get; set; } = "ALL";

    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnName = "source_title", ColumnDescription = "标题", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceTitle { get; set; } = string.Empty;

    /// <summary>
    /// 状态（来源 PLM 英文；包含关键字映射 ChangeStatus：Work→1、Cancel→2、Issued→3、Change→4、Fixed→5、Pending→6、Rejected→7）
    /// </summary>
    [SugarColumn(ColumnName = "source_status", ColumnDescription = "状态", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    [SugarColumn(ColumnName = "source_issue_date", ColumnDescription = "发行日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime SourceIssueDate { get; set; }

    /// <summary>
    /// TCJ担当（来源 PLM 字段；与设变主 EcLeader 无对应关系，导入时不映射）
    /// </summary>
    [SugarColumn(ColumnName = "source_tcj_owner", ColumnDescription = "TCJ担当", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceTcjOwner { get; set; }

    /// <summary>
    /// TCJ依赖
    /// </summary>
    [SugarColumn(ColumnName = "source_tcj_dependency", ColumnDescription = "TCJ依赖", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceTcjDependency { get; set; }

    /// <summary>
    /// 设变会议
    /// </summary>
    [SugarColumn(ColumnName = "source_ec_meeting", ColumnDescription = "设变会议", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceEcMeeting { get; set; }

    /// <summary>
    /// PP番号
    /// </summary>
    [SugarColumn(ColumnName = "source_pp_code", ColumnDescription = "PP番号", Length = 10, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourcePpCode { get; set; }

    /// <summary>
    /// 技联书
    /// </summary>
    [SugarColumn(ColumnName = "source_technical_notice_code", ColumnDescription = "技联书", Length = 10, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceTechnicalNoticeCode { get; set; }

    /// <summary>
    /// 实施
    /// </summary>
    [SugarColumn(ColumnName = "source_implementation", ColumnDescription = "实施", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceImplementation { get; set; }

    /// <summary>
    /// 主变更理由
    /// </summary>
    [SugarColumn(ColumnName = "source_main_change_reason", ColumnDescription = "主变更理由", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceMainChangeReason { get; set; }

    /// <summary>
    /// 次变更理由
    /// </summary>
    [SugarColumn(ColumnName = "source_secondary_change_reason", ColumnDescription = "次变更理由", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceSecondaryChangeReason { get; set; }

    /// <summary>
    /// 安规
    /// </summary>
    [SugarColumn(ColumnName = "source_safety_regulation", ColumnDescription = "安规", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceSafetyRegulation { get; set; }

    /// <summary>
    /// 进行状况
    /// </summary>
    [SugarColumn(ColumnName = "source_progress_status", ColumnDescription = "进行状况", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceProgressStatus { get; set; }

    /// <summary>
    /// 机番管理
    /// </summary>
    [SugarColumn(ColumnName = "source_serial_number_control", ColumnDescription = "机番管理", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceSerialNumberControl { get; set; }

    /// <summary>
    /// 客户承认
    /// </summary>
    [SugarColumn(ColumnName = "source_customer_approval", ColumnDescription = "客户承认", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceCustomerApproval { get; set; }

    /// <summary>
    /// 服务手册订正
    /// </summary>
    [SugarColumn(ColumnName = "source_service_manual_revision", ColumnDescription = "服务手册订正", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceServiceManualRevision { get; set; }

    /// <summary>
    /// 用户手册订正
    /// </summary>
    [SugarColumn(ColumnName = "source_user_manual_revision", ColumnDescription = "用户手册订正", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceUserManualRevision { get; set; }

    /// <summary>
    /// 宣传手册订正
    /// </summary>
    [SugarColumn(ColumnName = "source_promotion_manual_revision", ColumnDescription = "宣传手册订正", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourcePromotionManualRevision { get; set; }

    /// <summary>
    /// 标准书订正
    /// </summary>
    [SugarColumn(ColumnName = "source_standard_document_revision", ColumnDescription = "标准书订正", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceStandardDocumentRevision { get; set; }

    /// <summary>
    /// 情报发行
    /// </summary>
    [SugarColumn(ColumnName = "source_information_release", ColumnDescription = "情报发行", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceInformationRelease { get; set; }

    /// <summary>
    /// 成本变动
    /// </summary>
    [SugarColumn(ColumnName = "source_cost_change", ColumnDescription = "成本变动", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceCostChange { get; set; }

    /// <summary>
    /// 单位成本
    /// </summary>
    [SugarColumn(ColumnName = "source_unit_cost", ColumnDescription = "单位成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal SourceUnitCost { get; set; } = 0;

    /// <summary>
    /// 模具改修费
    /// </summary>
    [SugarColumn(ColumnName = "source_mold_modification_cost", ColumnDescription = "模具改修费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal SourceMoldModificationCost { get; set; } = 0;

    /// <summary>
    /// 相关图纸
    /// </summary>
    [SugarColumn(ColumnName = "source_related_drawing", ColumnDescription = "相关图纸", Length = 210, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceRelatedDrawing { get; set; }

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "source_ec_content", ColumnDescription = "设变内容", ColumnDataType = "ntext", IsNullable = false)]
    public string SourceEcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变来源明细列表
    /// </summary>
    // ========================================
    // 导航属性区域
    // ========================================
    [Navigate(NavigateType.OneToMany, nameof(TaktSourceEcDetail.SourceEcId))]
    public List<TaktSourceEcDetail>? SourceEcDetails { get; set; }
}
