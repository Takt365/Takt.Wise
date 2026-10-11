// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：TaktFlowConditionEvaluator.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程条件分支求值（与前端 conditionList 语义一致）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 流程条件求值
/// </summary>
public static class TaktFlowConditionEvaluator
{
    /// <summary>
    /// 判断条件节点是否匹配
    /// </summary>
    /// <param name="node">条件节点</param>
    /// <param name="frmDataJson">表单 JSON</param>
    /// <returns>是否匹配</returns>
    public static bool MatchesConditionNode(TaktFlowTreeNode node, string? frmDataJson)
    {
        if (node.IsDefault == 1)
        {
            return true;
        }
        var list = node.ConditionList;
        if (list == null || list.Count == 0)
        {
            return node.IsDefault == 1;
        }
        var formValues = ParseFormValues(frmDataJson);
        foreach (var item in list)
        {
            if (string.IsNullOrWhiteSpace(item.Zdy1))
            {
                continue;
            }
            var fieldKey = ResolveFieldKey(item);
            if (!formValues.TryGetValue(fieldKey, out var actual))
            {
                return false;
            }
            if (!CompareValues(actual, item.Zdy1, item.OptType))
            {
                return false;
            }
        }
        return list.Any(x => !string.IsNullOrWhiteSpace(x.Zdy1));
    }

    /// <summary>
    /// 从网关选择匹配的条件分支
    /// </summary>
    /// <param name="gateway">网关节点</param>
    /// <param name="frmDataJson">表单 JSON</param>
    /// <returns>匹配分支</returns>
    public static TaktFlowTreeNode? SelectConditionBranch(TaktFlowTreeNode gateway, string? frmDataJson)
    {
        var branches = gateway.ConditionNodes?
            .OrderBy(x => x.PriorityLevel ?? int.MaxValue)
            .ToList();
        if (branches == null || branches.Count == 0)
        {
            return null;
        }
        TaktFlowTreeNode? defaultBranch = null;
        foreach (var branch in branches)
        {
            if (branch.IsDefault == 1)
            {
                defaultBranch = branch;
                continue;
            }
            if (MatchesConditionNode(branch, frmDataJson))
            {
                return branch;
            }
        }
        return defaultBranch ?? branches[^1];
    }

    /// <summary>
    /// 解析表单字段字典
    /// </summary>
    /// <param name="frmDataJson">表单 JSON</param>
    /// <returns>字段键值</returns>
    private static Dictionary<string, string> ParseFormValues(string? frmDataJson)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(frmDataJson))
        {
            return result;
        }
        try
        {
            if (JObject.Parse(frmDataJson) is not JObject root)
            {
                return result;
            }
            foreach (var prop in root.Properties())
            {
                result[prop.Name] = prop.Value.Type switch
                {
                    JTokenType.String => prop.Value.Value<string>() ?? string.Empty,
                    JTokenType.Integer or JTokenType.Float => prop.Value.ToString(Formatting.None),
                    JTokenType.Boolean => prop.Value.Value<bool>() ? "true" : "false",
                    JTokenType.Null => string.Empty,
                    _ => prop.Value.ToString(Formatting.None) ?? string.Empty
                };
            }
        }
        catch
        {
            // 表单非 JSON 时条件不匹配
        }
        return result;
    }

    /// <summary>
    /// 解析条件字段键
    /// </summary>
    /// <param name="item">条件项</param>
    /// <returns>字段键</returns>
    private static string ResolveFieldKey(TaktFlowConditionItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.FormId))
        {
            return item.FormId.Trim();
        }
        if (!string.IsNullOrWhiteSpace(item.ShowName))
        {
            return item.ShowName.Trim().Replace(" ", "_");
        }
        return string.Empty;
    }

    /// <summary>
    /// 比较字段值
    /// </summary>
    /// <param name="actual">实际值</param>
    /// <param name="expected">期望值</param>
    /// <param name="optType">比较符</param>
    /// <returns>是否满足</returns>
    private static bool CompareValues(string actual, string expected, string? optType)
    {
        if (decimal.TryParse(actual, NumberStyles.Any, CultureInfo.InvariantCulture, out var actualNum)
            && decimal.TryParse(expected, NumberStyles.Any, CultureInfo.InvariantCulture, out var expectedNum))
        {
            return optType switch
            {
                "1" => actualNum < expectedNum,
                "2" => actualNum > expectedNum,
                "4" => actualNum >= expectedNum,
                "5" => actualNum <= expectedNum,
                _ => actualNum == expectedNum
            };
        }
        var cmp = string.Compare(actual, expected, StringComparison.OrdinalIgnoreCase);
        return optType switch
        {
            "1" => cmp < 0,
            "2" => cmp > 0,
            "4" => cmp >= 0,
            "5" => cmp <= 0,
            _ => cmp == 0
        };
    }
}
