// ========================================
// 项目名称：节拍智控 ·TaktWise (TDF) 
// 命名空间：Takt.Domain.Entities.Code.Generator
// 文件名称：TaktGenTableColumn.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt代码生成字段配置实体，参考主流代码生成器设计（RuoYi、MyBatis-Plus）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Code.Generator;

/// <summary>
/// Takt代码生成字段配置实体
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_code_generator_gen_table_column", "代码生成数据表列配置")]
[SugarIndex("ix_gen_table_column_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_gen_table_column_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_gen_table_column_column_unique", nameof(TenantCode), OrderByType.Asc, nameof(GenTableId), OrderByType.Asc, nameof(DatabaseColumnName), OrderByType.Asc, true)]
[SugarIndex("ix_gen_table_column_database_column_name", nameof(TenantCode), OrderByType.Asc, nameof(GenTableId), OrderByType.Asc, nameof(DatabaseColumnName), OrderByType.Asc, false)]
[SugarIndex("ix_gen_table_column_gen_table_id", nameof(TenantCode), OrderByType.Asc, nameof(GenTableId), OrderByType.Asc, false)]
public class TaktGenTableColumn : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "gen_table_id", ColumnDescription = "生成表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    [SugarColumn(ColumnName = "database_column_name", ColumnDescription = "列名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    [SugarColumn(ColumnName = "column_comment", ColumnDescription = "列描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ColumnComment { get; set; }

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    [SugarColumn(ColumnName = "database_data_type", ColumnDescription = "数据类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "nvarchar")]
    public string DatabaseDataType { get; set; } = "nvarchar";

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    [SugarColumn(ColumnName = "csharp_data_type", ColumnDescription = "C#类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "string")]
    public string CsharpDataType { get; set; } = "string";

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    [SugarColumn(ColumnName = "csharp_column_name", ColumnDescription = "C#列名", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    [SugarColumn(ColumnName = "length", ColumnDescription = "长度", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Length { get; set; } = 0;

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    [SugarColumn(ColumnName = "decimal_digits", ColumnDescription = "精度", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DecimalDigits { get; set; } = 0;

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_pk", ColumnDescription = "主键", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPk { get; set; } = 0;

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_increment", ColumnDescription = "自增", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsIncrement { get; set; } = 0;

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_required", ColumnDescription = "必填", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsRequired { get; set; } = 1;

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_create", ColumnDescription = "新增", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsCreate { get; set; } = 1;

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_update", ColumnDescription = "更新", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsUpdate { get; set; } = 1;

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_unique", ColumnDescription = "查重", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsUnique { get; set; } = 0;

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_list", ColumnDescription = "列表", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsList { get; set; } = 1;

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_export", ColumnDescription = "导出", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsExport { get; set; } = 1;

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    [SugarColumn(ColumnName = "is_sort", ColumnDescription = "排序", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsSort { get; set; } = 0;

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_query", ColumnDescription = "查询", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsQuery { get; set; } = 0;

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    [SugarColumn(ColumnName = "query_type", ColumnDescription = "查询方式", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "")]
    public string QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    [SugarColumn(ColumnName = "html_type", ColumnDescription = "显示类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "input")]
    public string HtmlType { get; set; } = "input";

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type", ColumnDescription = "字典类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DictType { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(TaktGenTableColumn.GenTableId))]
    public TaktGenTable? Table { get; set; }
}
