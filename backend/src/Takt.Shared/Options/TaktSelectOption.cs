// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Options
// 文件名称：TaktSelectOption.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt通用下拉选择框选项，用于前端下拉框、选择器等组件
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;

namespace Takt.Shared.Options;

/// <summary>
/// 下拉选择框选项
/// </summary>
public class TaktSelectOption
{
    /// <summary>
    /// 字典标签
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典键值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public object DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签
    /// </summary>
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    public object? ExtValue { get; set; }

    /// <summary>
    /// 国际化键（用于多语言翻译，与 TaktDictData.I18nKey 一致）
    /// </summary>
    public string? I18nKey { get; set; }

    /// <summary>
    /// 字典类型编码（用于批量加载时前端分组，单个查询时通常为空）
    /// </summary>
    public string? DictTypeCode { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// CSS类名
    /// </summary>
    public int? CssClass { get; set; }

    /// <summary>
    /// 列表类名
    /// </summary>
    public int? ListClass { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否默认项（1=是，0=否；与 TaktDictData.IsDefault / sys_yes_no 一致，供前端默认选中）
    /// </summary>
    public int IsDefault { get; set; } = 0;
}

/// <summary>
/// 树形下拉选择框选项（通用树形结构，适用于部门、会计科目、菜单等）
/// </summary>
public class TaktTreeSelectOption : TaktSelectOption
{
    /// <summary>
    /// 子节点列表（懒加载树：非叶子勿赋空数组，保持 null 以便前端展示展开图标并由 loadData 拉取）
    /// </summary>
    public List<TaktTreeSelectOption>? Children { get; set; }

    /// <summary>
    /// 是否叶子节点（懒加载树：true=不可展开；false/null 且 Children 为空时由前端 loadData）
    /// </summary>
    public bool? IsLeaf { get; set; }
}