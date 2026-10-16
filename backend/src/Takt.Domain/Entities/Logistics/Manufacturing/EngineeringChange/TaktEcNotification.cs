// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotification.cs
// 创建时间：2026-05-07
// 创建人：Takt365(Qoder AI)
// 功能描述：工程变更通知单（技术阶段一 ④）；技术保存主表/附件/明细后由系统自动生成并派发，通知各部门进入执行阶段
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单（技术阶段一 ④，隶属 TaktEcGijutsu）。技术完成 ①主表 ②附件 ③明细 保存后由 TaktEcGijutsuService 自动生成并派发；
/// 各部门确认后在 TaktEcExec* 执行，技术通过看板/批次监控。FlowInstanceId 由通知审批流程写入（可选）。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_notification", "工程变更通知单表")]
[SugarIndex("ix_ec_notification_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_notification_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcGijutsuId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_notification_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktEcNotification : TaktApprovalEntityBase
{

    /// <summary>
    /// 通知单号（按通知方式经编码规则生成；前端表单选择编码规则后自动通过 TaktNumbering 通知单号规则生成并展示，非手输；单据类型菜单：设变通知）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_code", ColumnDescription = "通知单号", Length = 30, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 技术课主表 ID（TaktEcGijutsu 主键；序列化为 string 避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "ec_gijutsu_id", ColumnDescription = "技术课主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }

    /// <summary>
    /// 设变单号（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", Length = 30, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题（冗余字段）
    /// </summary>
    [SugarColumn(ColumnName = "ec_title", ColumnDescription = "设变标题", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcTitle { get; set; }

    /// <summary>
    /// 通知日期
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_date", ColumnDescription = "通知日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EcNotificationDate { get; set; }

    /// <summary>
    /// 通知部门编码（多选，逗号分隔；选项 TaktDepts/options；DictValue=DeptCode，DictLabel=DeptName1；联动 TaktEcGroups/options?ecGroupCategory=；Length=500 容纳多个 6 位编码）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_dept_codes", ColumnDescription = "通知部门组编码", ColumnDataType = "varchar", Length = 500, IsNullable = true)]
    public string? EcNotificationDeptCodes { get; set; }

    /// <summary>
    /// 通知部门名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationDeptCodes 取 TaktDept.DeptName1 联动；Length=500）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_dept_names", ColumnDescription = "通知部门组名称", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? EcNotificationDeptNames { get; set; }

    /// <summary>
    /// 通知组编码（多选，逗号分隔；选项 TaktEcGroups/options?ecGroupCategory=EcNotificationDeptCodes；DictValue=EcGroupCode，DictLabel=EcGroupName；Length=500 容纳多个 6 位编码）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_notifier_group_code", ColumnDescription = "通知组编码", ColumnDataType = "varchar", Length = 500, IsNullable = true)]
    public string? EcNotificationNotifierGroupCode { get; set; }

    /// <summary>
    /// 通知组名称（多选，逗号分隔，与编码同序；冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动；Length=500）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_notifier_group_name", ColumnDescription = "通知组名称", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? EcNotificationNotifierGroupName { get; set; }

    /// <summary>
    /// 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_method", ColumnDescription = "通知方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int EcNotificationMethod { get; set; } = 2;

    /// <summary>
    /// 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_status", ColumnDescription = "通知状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EcNotificationStatus { get; set; } = 0;

    /// <summary>
    /// 关联的设变主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcGijutsuId))]
    public TaktEcGijutsu? EcGijutsu { get; set; }
}
