// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktAuditContextRemarks.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：审计/统计日志 Remark 文案（用户名未知、登出原因等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 审计上下文 Remark 文案（差异日志、操作日志、登录日志共用）
/// </summary>
public static class TaktAuditContextRemarks
{
    /// <summary>
    /// 非登出场景下操作人用户名无法解析时的默认 Remark
    /// </summary>
    public const string DefaultUnknownOperator = "操作人用户名未解析";

    /// <summary>
    /// 登出成功但无法解析用户名时的 Remark（含认证阶段与说明）
    /// </summary>
    /// <param name="phase">认证阶段（TaktAuthLoginPhases）</param>
    /// <param name="detail">登出说明（如 Cookie 会话已注销）</param>
    /// <returns>Remark 文本</returns>
    public static string BuildSignOutUnknownUser(string phase, string? detail = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(phase);
        var phaseText = phase.Trim();
        if (string.IsNullOrWhiteSpace(detail))
        {
            return $"登出：用户名未解析，阶段={phaseText}";
        }

        return $"登出：用户名未解析，阶段={phaseText}，{detail.Trim()}";
    }
}
