// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktSelfService.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台自助服务项实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 服务台自助服务项实体
/// </summary>
[SugarTable("takt_routine_help_desk_self_service", "自助服务表")]
[SugarIndex("ix_self_service_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_self_service_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_self_service_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ServiceType), OrderByType.Asc, false)]
[SugarIndex("ix_self_service_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SelfServiceStatus), OrderByType.Asc, false)]
public class TaktSelfService : TaktCompanyEntityBase
{
    /// <summary>
    /// 自助服务名称
    /// </summary>
    [SugarColumn(ColumnName = "service_name", ColumnDescription = "服务名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ServiceName { get; set; } = string.Empty;
    /// <summary>
    /// 服务类型（字典 routine_self_service_type；0=链接 1=表单 2=知识引导）
    /// </summary>
    [SugarColumn(ColumnName = "service_type", ColumnDescription = "服务类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ServiceType { get; set; } = 0;
    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(ColumnName = "self_service_description", ColumnDescription = "描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? SelfServiceDescription { get; set; }
    /// <summary>
    /// 链接地址或表单编码
    /// </summary>
    [SugarColumn(ColumnName = "link_or_code", ColumnDescription = "链接或表单编码", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? LinkOrCode { get; set; }
    /// <summary>
    /// 图标或图片 URL
    /// </summary>
    [SugarColumn(ColumnName = "icon_url", ColumnDescription = "图标URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? IconUrl { get; set; }
    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 自助服务状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "self_service_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SelfServiceStatus { get; set; } = 1;
}
