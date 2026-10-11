// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableRuntimeDtos.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表 运行时 DTO（筛选条件 / 查询 / 导出）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Statistics.QuickQuery;

/// <summary>
/// 运行时结果列定义
/// </summary>
public class TaktConfigurableRuntimeColumnDto
{
    /// <summary>
    /// 列键（与查询结果行字典键一致）
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 列显示名
    /// </summary>
    public string Label { get; set; } = string.Empty;
}

/// <summary>
/// 运行时筛选项定义
/// </summary>
public class TaktConfigurableRuntimeSelectionDto
{
    /// <summary>
    /// 筛选项主键（运行时表单绑定键，与 SortOrder 解耦）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSelectionId { get; set; }

    /// <summary>
    /// 排序号（回填）（运行时取值键）
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符（code_generator_query_type：1=eq…7=like…8=between）
    /// </summary>
    public int FilterOperator { get; set; }

    /// <summary>
    /// 是否必填（sys_yes_no，1=是）
    /// </summary>
    public int IsRequired { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// 区间结束默认值
    /// </summary>
    public string? DefaultValueTo { get; set; }
}

/// <summary>
/// 运行时筛选条件响应
/// </summary>
public class TaktConfigurableRuntimeScreenDto
{
    /// <summary>
    /// 定制报表主键
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 定制报表编码
    /// </summary>
    public string ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表名称
    /// </summary>
    public string ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 单次查询最大行数
    /// </summary>
    public int MaxQueryRows { get; set; }

    /// <summary>
    /// 单次导出最大行数
    /// </summary>
    public int MaxExportRows { get; set; }

    /// <summary>
    /// 输出列定义
    /// </summary>
    public List<TaktConfigurableRuntimeColumnDto> Columns { get; set; } = new();

    /// <summary>
    /// 筛选项列表
    /// </summary>
    public List<TaktConfigurableRuntimeSelectionDto> Selections { get; set; } = new();
}

/// <summary>
/// 运行时筛选值
/// </summary>
public class TaktConfigurableRuntimeSelectionValueDto
{
    /// <summary>
    /// 筛选项主键（与 SortOrder 二选一或同时传；优先按主键匹配）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSelectionId { get; set; }

    /// <summary>
    /// 排序号（回填）（与筛选项 SortOrder 一致）
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// 筛选值
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// 区间结束值
    /// </summary>
    public string? ValueTo { get; set; }

    /// <summary>
    /// 运行时比较运算符（1～8，与 code_generator_query_type SortOrder 一致；未传则用筛选项定义，默认 7=like）
    /// </summary>
    public int? FilterOperator { get; set; }
}

/// <summary>
/// 执行定制报表查询请求
/// </summary>
public class TaktConfigurableExecuteQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 运行时筛选值列表
    /// </summary>
    public List<TaktConfigurableRuntimeSelectionValueDto> SelectionValues { get; set; } = new();

    /// <summary>
    /// 本次查询行数上限（0 或未传则用默认 500，最大 50000，且不超过报表配置）
    /// </summary>
    public int RowLimit { get; set; }
}

/// <summary>
/// 执行定制报表查询结果
/// </summary>
public class TaktConfigurableQueryResultDto
{
    /// <summary>
    /// 输出列
    /// </summary>
    public List<TaktConfigurableRuntimeColumnDto> Columns { get; set; } = new();

    /// <summary>
    /// 数据行（列键 → 值）
    /// </summary>
    public List<Dictionary<string, object?>> Rows { get; set; } = new();

    /// <summary>
    /// 总记录数（受 MaxQueryRows 上限约束）
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 当前页码
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }
}

/// <summary>
/// 导出定制报表数据请求
/// </summary>
public class TaktConfigurableExportDataDto
{
    /// <summary>
    /// 运行时筛选值列表
    /// </summary>
    public List<TaktConfigurableRuntimeSelectionValueDto> SelectionValues { get; set; } = new();

    /// <summary>
    /// 本次导出行数上限（0 或未传则用默认 500，最大 50000，且不超过报表配置）
    /// </summary>
    public int RowLimit { get; set; }
}

/// <summary>
/// 设计态预览查询（未持久化定制报表定义 + 分页）
/// </summary>
public class TaktConfigurablePreviewQueryDto : TaktConfigurableExecuteQueryDto
{
    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; }

    /// <summary>
    /// 单次查询最大行数
    /// </summary>
    public int MaxQueryRows { get; set; }

    /// <summary>
    /// 数据源表列表
    /// </summary>
    public List<TaktConfigurableSourceCreateDto> Sources { get; set; } = new();

    /// <summary>
    /// 多表关联列表
    /// </summary>
    public List<TaktConfigurableJoinCreateDto> Joins { get; set; } = new();

    /// <summary>
    /// 输出字段列表
    /// </summary>
    public List<TaktConfigurableFieldCreateDto> Fields { get; set; } = new();

    /// <summary>
    /// 筛选条件列表
    /// </summary>
    public List<TaktConfigurableSelectionCreateDto> Selections { get; set; } = new();

    /// <summary>
    /// 分组字段列表
    /// </summary>
    public List<TaktConfigurableGroupByCreateDto> GroupBys { get; set; } = new();

    /// <summary>
    /// 排序字段列表
    /// </summary>
    public List<TaktConfigurableOrderByCreateDto> OrderBys { get; set; } = new();
}
