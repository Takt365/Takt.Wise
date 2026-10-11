// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Generator
// 文件名称：TaktGenTableColumnDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：GenTableColumn 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktGenTableColumn 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Code.Generator;

// ========================================
// GenTableColumn 响应 DTO
// ========================================

/// <summary>
/// Takt代码生成字段配置实体 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// 对应前端 TaktGenTableColumnDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktGenTableColumnDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// GenTableColumnID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableColumnId { get; set; }

    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 生成表名称（填充字段）
    /// </summary>
    public string? GenTableName { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; } = string.Empty;

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    public string DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    public string CsharpDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    public string CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    public int Length { get; set; } = 0;

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    public int DecimalDigits { get; set; } = 0;

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPk { get; set; } = 0;

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsIncrement { get; set; } = 0;

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsCreate { get; set; } = 0;

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUpdate { get; set; } = 0;

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUnique { get; set; } = 0;

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsList { get; set; } = 0;

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsExport { get; set; } = 0;

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    public int IsSort { get; set; } = 0;

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsQuery { get; set; } = 0;

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    public string QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string HtmlType { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictType { get; set; } = string.Empty;

    /// <summary>
    /// 所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id）
    /// （主表：TaktGenTable）
    /// </summary>
    public TaktGenTableDto? Table { get; set; }

}

// ========================================
// GenTableColumn 查询 DTO
// ========================================

/// <summary>
/// GenTableColumn 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktGenTableColumnQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? GenTableId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    public string? DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; } = string.Empty;

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    public string? DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    public string? CsharpDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    public string? CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    public int? DecimalDigits { get; set; }

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsPk { get; set; }

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsIncrement { get; set; }

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsRequired { get; set; }

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsCreate { get; set; }

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUpdate { get; set; }

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUnique { get; set; }

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsList { get; set; }

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsExport { get; set; }

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    public int? IsSort { get; set; }

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsQuery { get; set; }

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    public string? QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? HtmlType { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictType { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建GenTableColumn DTO
// ========================================

/// <summary>
/// 创建GenTableColumn DTO
/// </summary>
public class TaktGenTableColumnCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    [Required(ErrorMessage = "数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）不能为空")]
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; } = string.Empty;

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    [Required(ErrorMessage = "数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）不能为空")]
    public string DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    [Required(ErrorMessage = "C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）不能为空")]
    public string CsharpDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    [Required(ErrorMessage = "C#列名（C#属性名，首字母大写，帕斯卡命名法）不能为空")]
    public string CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    public int Length { get; set; } = 0;

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    public int DecimalDigits { get; set; } = 0;

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPk { get; set; } = 0;

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsIncrement { get; set; } = 0;

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsCreate { get; set; } = 0;

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUpdate { get; set; } = 0;

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUnique { get; set; } = 0;

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsList { get; set; } = 0;

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsExport { get; set; } = 0;

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    public int IsSort { get; set; } = 0;

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsQuery { get; set; } = 0;

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    [Required(ErrorMessage = "查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq不能为空")]
    public string QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    [Required(ErrorMessage = "显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）不能为空")]
    public string HtmlType { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictType { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新GenTableColumn DTO
// ========================================

/// <summary>
/// 更新GenTableColumn DTO
/// 继承 TaktGenTableColumnCreateDto，添加 GenTableColumnId 字段
/// </summary>
public class TaktGenTableColumnUpdateDto : TaktGenTableColumnCreateDto
{
    /// <summary>
    /// GenTableColumnID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableColumnId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// GenTableColumn 导入模板行 DTO
/// </summary>
public class TaktGenTableColumnTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? GenTableId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    public string? DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; } = string.Empty;

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    public string? DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    public string? CsharpDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    public string? CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    public int? DecimalDigits { get; set; }

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsPk { get; set; }

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsIncrement { get; set; }

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsRequired { get; set; }

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsCreate { get; set; }

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUpdate { get; set; }

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUnique { get; set; }

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsList { get; set; }

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsExport { get; set; }

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    public int? IsSort { get; set; }

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsQuery { get; set; }

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    public string? QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? HtmlType { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictType { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// GenTableColumn 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktGenTableColumnImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? GenTableId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    public string? DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; } = string.Empty;

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    public string? DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    public string? CsharpDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    public string? CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    public int? DecimalDigits { get; set; }

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsPk { get; set; }

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsIncrement { get; set; }

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsRequired { get; set; }

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsCreate { get; set; }

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUpdate { get; set; }

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUnique { get; set; }

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsList { get; set; }

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsExport { get; set; }

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    public int? IsSort { get; set; }

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsQuery { get; set; }

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    public string? QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? HtmlType { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictType { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// GenTableColumn 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktGenTableColumnExportDto
{
    /// <summary>
    /// GenTableColumnID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableColumnId { get; set; }

    /// <summary>
    /// 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
    /// </summary>
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; } = string.Empty;

    /// <summary>
    /// 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
    /// </summary>
    public string DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
    /// </summary>
    public string CsharpDataType { get; set; } = string.Empty;

    /// <summary>
    /// C#列名（C#属性名，首字母大写，帕斯卡命名法）
    /// </summary>
    public string CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// C#长度（字符串长度、数值类型的整数位数）
    /// </summary>
    public int Length { get; set; } = 0;

    /// <summary>
    /// C#小数位数（decimal等数值类型的小数位数）
    /// </summary>
    public int DecimalDigits { get; set; } = 0;

    /// <summary>
    /// 主键（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsPk { get; set; } = 0;

    /// <summary>
    /// 自增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsIncrement { get; set; } = 0;

    /// <summary>
    /// 必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 新增（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsCreate { get; set; } = 0;

    /// <summary>
    /// 更新（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUpdate { get; set; } = 0;

    /// <summary>
    /// 查重（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUnique { get; set; } = 0;

    /// <summary>
    /// 列表（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsList { get; set; } = 0;

    /// <summary>
    /// 导出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsExport { get; set; } = 0;

    /// <summary>
    /// 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
    /// </summary>
    public int IsSort { get; set; } = 0;

    /// <summary>
    /// 查询（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsQuery { get; set; } = 0;

    /// <summary>
    /// 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
    /// </summary>
    public string QueryType { get; set; } = string.Empty;

    /// <summary>
    /// 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string HtmlType { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型（选项 TaktDictTypes/options；DictValue=Id）
    /// </summary>
    public string? DictType { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
