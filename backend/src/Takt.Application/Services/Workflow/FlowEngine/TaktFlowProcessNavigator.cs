// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：TaktFlowProcessNavigator.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程树解析与节点遍历
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 流程树导航
/// </summary>
public static class TaktFlowProcessNavigator
{
    private static readonly JsonSerializerSettings DesignJsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    /// <summary>
    /// 解析流程设计 JSON 为树根
    /// </summary>
    /// <param name="processContent">设计 JSON</param>
    /// <returns>根节点</returns>
    public static TaktFlowTreeNode? ParseRoot(string? processContent)
    {
        if (string.IsNullOrWhiteSpace(processContent))
        {
            return null;
        }
        try
        {
            var token = JToken.Parse(processContent.Trim());
            if (token.Type == JTokenType.String)
            {
                var inner = token.ToString();
                if (!string.IsNullOrWhiteSpace(inner))
                {
                    token = JToken.Parse(inner);
                }
            }
            if (token is not JObject obj)
            {
                return null;
            }
            var treeToken = obj["flowTree"] ?? obj["FlowTree"];
            if (treeToken is JObject treeObj)
            {
                return treeObj.ToObject<TaktFlowTreeNode>(JsonSerializer.Create(DesignJsonSettings));
            }
            if (obj["nodeType"] != null || obj["NodeType"] != null)
            {
                return obj.ToObject<TaktFlowTreeNode>(JsonSerializer.Create(DesignJsonSettings));
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 按 nodeId 查找节点
    /// </summary>
    /// <param name="root">根节点</param>
    /// <param name="nodeId">节点 ID</param>
    /// <returns>节点</returns>
    public static TaktFlowTreeNode? FindNode(TaktFlowTreeNode? root, string? nodeId)
    {
        if (root == null || string.IsNullOrWhiteSpace(nodeId))
        {
            return null;
        }
        if (string.Equals(root.NodeId, nodeId, StringComparison.Ordinal))
        {
            return root;
        }
        if (root.ChildNode != null)
        {
            var found = FindNode(root.ChildNode, nodeId);
            if (found != null)
            {
                return found;
            }
        }
        if (root.ConditionNodes != null)
        {
            foreach (var branch in root.ConditionNodes)
            {
                var found = FindNode(branch, nodeId);
                if (found != null)
                {
                    return found;
                }
                if (branch.ChildNode != null)
                {
                    found = FindNode(branch.ChildNode, nodeId);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
        }
        if (root.ParallelNodes != null)
        {
            foreach (var branch in root.ParallelNodes)
            {
                var found = FindNode(branch, nodeId);
                if (found != null)
                {
                    return found;
                }
                if (branch.ChildNode != null)
                {
                    found = FindNode(branch.ChildNode, nodeId);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 获取节点显示名称
    /// </summary>
    /// <param name="node">节点</param>
    /// <returns>名称</returns>
    public static string GetNodeDisplayName(TaktFlowTreeNode node)
    {
        if (!string.IsNullOrWhiteSpace(node.NodeDisplayName))
        {
            return node.NodeDisplayName;
        }
        return node.NodeName ?? node.NodeId;
    }

    /// <summary>
    /// 在子树中查找首个审批节点（nodeType=4）
    /// </summary>
    /// <param name="node">起点</param>
    /// <returns>审批节点</returns>
    public static TaktFlowTreeNode? FindFirstApproverNode(TaktFlowTreeNode? node)
    {
        if (node == null)
        {
            return null;
        }
        if (node.NodeType == 4)
        {
            return node;
        }
        if (node.NodeType == 6)
        {
            return FindFirstApproverNode(node.ChildNode);
        }
        if (node.NodeType == 2)
        {
            var branch = node.ConditionNodes?.OrderBy(x => x.PriorityLevel ?? int.MaxValue).FirstOrDefault();
            return branch == null ? FindFirstApproverNode(node.ChildNode) : FindFirstApproverNode(branch.ChildNode ?? branch);
        }
        if (node.NodeType == 7 && node.ParallelNodes != null)
        {
            return node.ParallelNodes.Select(FindFirstApproverNode).FirstOrDefault(x => x != null);
        }
        return FindFirstApproverNode(node.ChildNode);
    }

    /// <summary>
    /// 收集并行网关下各分支的首个审批节点
    /// </summary>
    /// <param name="gateway">并行网关</param>
    /// <returns>审批节点列表</returns>
    public static List<TaktFlowTreeNode> CollectParallelApproverNodes(TaktFlowTreeNode gateway)
    {
        var list = new List<TaktFlowTreeNode>();
        if (gateway.ParallelNodes == null)
        {
            return list;
        }
        foreach (var branch in gateway.ParallelNodes)
        {
            var approver = FindFirstApproverNode(branch);
            if (approver != null)
            {
                list.Add(approver);
            }
        }
        return list;
    }

    /// <summary>
    /// 从当前节点推进到下一执行点（返回需停留的节点列表；空表示流程结束）
    /// </summary>
    /// <param name="root">根</param>
    /// <param name="currentNodeId">当前节点 ID</param>
    /// <param name="frmDataJson">表单数据</param>
    /// <returns>下一批执行节点</returns>
    public static List<TaktFlowTreeNode> ResolveNextExecutionNodes(TaktFlowTreeNode root, string? currentNodeId, string? frmDataJson)
    {
        var result = new List<TaktFlowTreeNode>();
        var start = string.IsNullOrWhiteSpace(currentNodeId)
            ? root
            : FindNode(root, currentNodeId) ?? root;
        var cursor = AdvanceFrom(start, frmDataJson, afterCurrent: !string.IsNullOrWhiteSpace(currentNodeId));
        while (cursor != null)
        {
            if (cursor.NodeType == 4)
            {
                result.Add(cursor);
                return result;
            }
            if (cursor.NodeType == 7)
            {
                result.AddRange(CollectParallelApproverNodes(cursor));
                return result;
            }
            if (cursor.NodeType == 6)
            {
                cursor = cursor.ChildNode;
                continue;
            }
            if (cursor.NodeType == 2)
            {
                var branch = TaktFlowConditionEvaluator.SelectConditionBranch(cursor, frmDataJson);
                cursor = branch?.ChildNode ?? cursor.ChildNode;
                continue;
            }
            if (cursor.NodeType == 1)
            {
                cursor = cursor.ChildNode;
                continue;
            }
            cursor = cursor.ChildNode;
        }
        return result;
    }

    /// <summary>
    /// 从节点继续向后（审批完成后）
    /// </summary>
    /// <param name="root">根</param>
    /// <param name="completedNode">已完成节点</param>
    /// <param name="frmDataJson">表单</param>
    /// <returns>下一执行节点</returns>
    public static List<TaktFlowTreeNode> ResolveAfterNodeCompleted(TaktFlowTreeNode root, TaktFlowTreeNode completedNode, string? frmDataJson)
    {
        var parentParallel = FindParentParallelGateway(root, completedNode.NodeId);
        if (parentParallel != null)
        {
            var branchApprovers = CollectParallelApproverNodes(parentParallel);
            var pendingBranchIds = branchApprovers.Select(x => x.NodeId).ToHashSet(StringComparer.Ordinal);
            pendingBranchIds.Remove(completedNode.NodeId);
            if (pendingBranchIds.Count > 0)
            {
                return new List<TaktFlowTreeNode>();
            }
            var mergeNode = parentParallel.ChildNode;
            if (mergeNode == null)
            {
                return new List<TaktFlowTreeNode>();
            }
            return ResolveNextExecutionNodes(root, mergeNode.NodeId, frmDataJson);
        }
        return ResolveNextExecutionNodes(root, completedNode.NodeId, frmDataJson);
    }

    /// <summary>
    /// 查找包含指定审批节点的并行网关
    /// </summary>
    /// <param name="root">根</param>
    /// <param name="approverNodeId">审批节点 ID</param>
    /// <returns>并行网关</returns>
    public static TaktFlowTreeNode? FindParentParallelGateway(TaktFlowTreeNode root, string approverNodeId)
    {
        return FindParentParallelGatewayCore(root, approverNodeId, null);
    }

    /// <summary>
    /// 从指定节点沿 child 链前进
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="frmDataJson">表单</param>
    /// <param name="afterCurrent">是否跳过当前节点</param>
    /// <returns>游标节点</returns>
    private static TaktFlowTreeNode? AdvanceFrom(TaktFlowTreeNode node, string? frmDataJson, bool afterCurrent)
    {
        var cursor = afterCurrent ? node.ChildNode : node;
        while (cursor != null)
        {
            if (cursor.NodeType == 4 || cursor.NodeType == 7)
            {
                return cursor;
            }
            if (cursor.NodeType == 6)
            {
                cursor = cursor.ChildNode;
                continue;
            }
            if (cursor.NodeType == 2)
            {
                var branch = TaktFlowConditionEvaluator.SelectConditionBranch(cursor, frmDataJson);
                cursor = branch?.ChildNode ?? cursor.ChildNode;
                continue;
            }
            if (cursor.NodeType == 1)
            {
                cursor = cursor.ChildNode;
                continue;
            }
            cursor = cursor.ChildNode;
        }
        return null;
    }

    /// <summary>
    /// 递归查找并行网关父节点
    /// </summary>
    /// <param name="node">当前节点</param>
    /// <param name="approverNodeId">目标 ID</param>
    /// <param name="parallelAncestor">并行祖先</param>
    /// <returns>网关</returns>
    private static TaktFlowTreeNode? FindParentParallelGatewayCore(
        TaktFlowTreeNode node,
        string approverNodeId,
        TaktFlowTreeNode? parallelAncestor)
    {
        if (node.NodeType == 7)
        {
            parallelAncestor = node;
        }
        if (node.NodeType == 4 && string.Equals(node.NodeId, approverNodeId, StringComparison.Ordinal))
        {
            return parallelAncestor;
        }
        if (node.ChildNode != null)
        {
            var found = FindParentParallelGatewayCore(node.ChildNode, approverNodeId, parallelAncestor);
            if (found != null)
            {
                return found;
            }
        }
        if (node.ConditionNodes != null)
        {
            foreach (var branch in node.ConditionNodes)
            {
                var found = FindParentParallelGatewayCore(branch, approverNodeId, parallelAncestor);
                if (found != null)
                {
                    return found;
                }
                if (branch.ChildNode != null)
                {
                    found = FindParentParallelGatewayCore(branch.ChildNode, approverNodeId, parallelAncestor);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
        }
        if (node.ParallelNodes != null)
        {
            foreach (var branch in node.ParallelNodes)
            {
                var found = FindParentParallelGatewayCore(branch, approverNodeId, parallelAncestor);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }
}
