// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Foundation
// 文件名称：TaktDictStorageContext.cs
// 功能描述：字典落库上下文：双向快照 + 多选排序映射（由 ITaktDictDataService 预加载）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Foundation;

/// <summary>
/// 字典落库上下文（快照 + DictValue→SortOrder 映射，供多选字段排序）
/// </summary>
public sealed class TaktDictStorageContext
{
    /// <summary>
    /// 字典双向快照
    /// </summary>
    public TaktDictSnapshot Snapshot { get; init; } = TaktDictSnapshot.CreateFromRows(Array.Empty<(string, string, string)>());

    /// <summary>
    /// 按 dict_type_code 分组的 DictValue→SortOrder
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> SortMapsByTypeCode { get; init; } =
        new Dictionary<string, IReadOnlyDictionary<string, int>>(StringComparer.Ordinal);
}
