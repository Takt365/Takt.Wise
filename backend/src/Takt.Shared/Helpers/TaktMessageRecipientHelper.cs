// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktMessageRecipientHelper.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息接收者推送目标解析（单条记录一位接收者；IsCc 自审计抄送发送者）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 在线消息接收者字段工具（无状态纯函数）
/// </summary>
public static class TaktMessageRecipientHelper
{
    /// <summary>
    /// 是否启用抄送发送者本人（自审计）
    /// </summary>
    /// <param name="isCc">是否抄送（sys_yes_no，1=是）</param>
    /// <returns>是否为「是」</returns>
    public static bool IsSelfAuditCcEnabled(int isCc) => isCc == 1;

    /// <summary>
    /// 合并主送与自审计抄送（IsCc 为是时将 FromUser 加入推送目标），按登录名去重
    /// </summary>
    /// <param name="toUserName">接收者用户名</param>
    /// <param name="toUserId">接收者用户 ID</param>
    /// <param name="isCc">是否抄送发送者本人</param>
    /// <param name="fromUserName">发送者用户名</param>
    /// <param name="fromUserId">发送者用户 ID</param>
    /// <returns>去重后的推送目标列表</returns>
    public static IReadOnlyList<(string Name, string? IdToken)> CollectPushTargets(
        string? toUserName,
        long toUserId,
        int isCc,
        string? fromUserName,
        long fromUserId)
    {
        var results = new List<(string Name, string? IdToken)>();
        var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void AppendTarget(string? name, long userId)
        {
            var trimmedName = name?.Trim();
            if (string.IsNullOrEmpty(trimmedName) || seenNames.Contains(trimmedName))
            {
                return;
            }
            seenNames.Add(trimmedName);
            results.Add((trimmedName, userId > 0 ? userId.ToString() : null));
        }

        AppendTarget(toUserName, toUserId);
        if (IsSelfAuditCcEnabled(isCc))
        {
            AppendTarget(fromUserName, fromUserId);
        }

        return results;
    }
}
