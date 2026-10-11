// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowForm.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程表单定义实体，存储表单编码、设计器 JSON 及数据源关联
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程表单定义实体
/// </summary>
[SugarTable("takt_workflow_form", "流程表单表")]
[SugarIndex("ix_flow_form_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_form_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(FormCode), OrderByType.Asc, true)]
[SugarIndex("ix_flow_form_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktFlowForm : TaktCompanyEntityBase
{

    /// <summary>
    /// 表单编码（公司内唯一；前端表单按表单分类选择编码规则后自动通过 TaktNumbering 表单编码规则生成并展示，非手输；单据类型菜单：表单管理）
    /// </summary>
    [SugarColumn(ColumnName = "form_code", ColumnDescription = "表单编码", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string FormCode { get; set; } = string.Empty;
    /// <summary>
    /// 表单名称
    /// </summary>
    [SugarColumn(ColumnName = "form_name", ColumnDescription = "表单名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string FormName { get; set; } = string.Empty;
    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    [SugarColumn(ColumnName = "form_category", ColumnDescription = "表单分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FormCategory { get; set; }
    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    [SugarColumn(ColumnName = "form_type", ColumnDescription = "表单类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FormType { get; set; }
    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    [SugarColumn(ColumnName = "form_config", ColumnDescription = "表单配置", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? FormConfig { get; set; }
    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    [SugarColumn(ColumnName = "form_template", ColumnDescription = "表单模板", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? FormTemplate { get; set; }
    /// <summary>
    /// 表单版本标签
    /// </summary>
    [SugarColumn(ColumnName = "form_version", ColumnDescription = "表单版本", ColumnDataType = "varchar", Length = 32, IsNullable = false, DefaultValue = "v1.0.0")]
    public string FormVersion { get; set; } = "v1.0.0";
    /// <summary>
    /// 是否绑定数据源（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_datasource", ColumnDescription = "数据源表单", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDatasource { get; set; }
    /// <summary>
    /// 关联库名（选项 TaktDatabaseInfos/list；DictValue=TenantCode）
    /// </summary>
    [SugarColumn(ColumnName = "related_database_name", ColumnDescription = "关联库名", ColumnDataType = "varchar", Length = 128, IsNullable = true)]
    public string? RelatedDataBaseName { get; set; }
    /// <summary>
    /// 关联表名（选项 TaktDatabaseInfos/tables；DictValue=TableName）
    /// </summary>
    [SugarColumn(ColumnName = "related_table_name", ColumnDescription = "关联表名", ColumnDataType = "varchar", Length = 128, IsNullable = true)]
    public string? RelatedTableName { get; set; }
    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    [SugarColumn(ColumnName = "related_form_field", ColumnDescription = "关联映射", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? RelatedFormField { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 表单状态（字典 sys_scheme_status；0=草稿 1=已发布 2=已禁用）
    /// </summary>
    [SugarColumn(ColumnName = "form_status", ColumnDescription = "表单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FormStatus { get; set; }
}
