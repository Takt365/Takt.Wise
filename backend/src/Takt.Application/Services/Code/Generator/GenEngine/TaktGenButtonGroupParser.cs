// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：TaktGenButtonGroupParser.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成菜单按钮组 JSON/逗号分隔配置解析
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json.Linq;

namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 按钮组配置解析
/// </summary>
internal static class TaktGenButtonGroupParser
{
    /// <summary>
    /// 解析按钮组为权限后缀列表
    /// </summary>
    /// <param name="buttonGroup">按钮组原始配置</param>
    /// <returns>权限后缀列表（小写）</returns>
    public static IReadOnlyList<string> ParseSelectionSuffixes(string? buttonGroup)
    {
        if (string.IsNullOrWhiteSpace(buttonGroup))
            return Array.Empty<string>();

        var trimmed = buttonGroup.Trim();

        if (trimmed.StartsWith('{'))
        {
            try
            {
                var jobj = JObject.Parse(trimmed);
                var list = new List<string>();
                foreach (var prop in jobj.Properties())
                {
                    var sfxSrc = prop.Value?.Type == JTokenType.String ? prop.Value.ToString() : prop.Value?.ToString();
                    var sfx = (sfxSrc ?? string.Empty).Trim().ToLowerInvariant();
                    if (sfx.Length > 0)
                        list.Add(sfx);
                }
                return list;
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        if (trimmed.StartsWith('['))
        {
            try
            {
                var arr = JArray.Parse(trimmed);
                return arr
                    .Where(el => el?.Type == JTokenType.String)
                    .Select(el => el!.ToString().Trim().ToLowerInvariant())
                    .Where(s => s.Length > 0)
                    .ToList();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        return trimmed
            .Split(new[] { ',', '，', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToLowerInvariant())
            .Where(s => s.Length > 0)
            .ToList();
    }
}
