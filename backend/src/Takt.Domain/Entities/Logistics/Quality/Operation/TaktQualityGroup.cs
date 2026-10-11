// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Operation
// 文件名称：TaktQualityGroup.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：质量组主数据实体，按质量组类别（IQC组、QA组、IPQC组、责任组、处理组、质量负责人组）定义质量业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// 质量组主数据实体（公司级；按质量组类别 IQC组、QA组、IPQC组、责任组、处理组、质量负责人组 区分的质量业务组织分组）
/// </summary>
[SugarTable("takt_logistics_quality_operation_quality_group", "质量组主数据表")]
[SugarIndex("ix_quality_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_operation_quality_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(QualityGroupCategory), OrderByType.Asc, nameof(QualityGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_operation_quality_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_operation_quality_group_quality_group_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QualityGroupCategory), OrderByType.Asc, false)]
public class TaktQualityGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// 质量组编码（与员工编码同长 6）
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_code", ColumnDescription = "质量组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string QualityGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 质量组名称
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_name", ColumnDescription = "质量组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string QualityGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 质量组描述
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_description", ColumnDescription = "质量组描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? QualityGroupDescription { get; set; }

    /// <summary>
    /// 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_category", ColumnDescription = "质量组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int QualityGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "质量组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}
