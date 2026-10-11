// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcRootMaterialCodeGroup.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：部管/SMT 根物料编码组拼接（同设变单号+机种，分号分隔，列类型 ntext）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 部管课与 SMT 执行行的根物料编码组
/// </summary>
internal static class TaktEcRootMaterialCodeGroup
{
    /// <summary>
    /// 去重、排序后用分号拼接（列类型 ntext，不截断）
    /// </summary>
    /// <param name="codes">根物料编码</param>
    /// <returns>编码组；无有效编码时为空串</returns>
    internal static string Join(IEnumerable<string?> codes)
    {
        ArgumentNullException.ThrowIfNull(codes);
        var parts = codes
            .Select(code => code?.Trim())
            .Where(code => !string.IsNullOrEmpty(code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(code => code, StringComparer.OrdinalIgnoreCase)
            .Cast<string>();
        return string.Join(';', parts);
    }

    /// <summary>
    /// 编码组是否包含指定根物料编码（分号分段，忽略大小写）
    /// </summary>
    /// <param name="group">根物料编码组</param>
    /// <param name="code">根物料编码</param>
    /// <returns>包含则为 true</returns>
    internal static bool ContainsCode(string? group, string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }
        var target = code.Trim();
        foreach (var part in (group ?? string.Empty).Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (string.Equals(part, target, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
