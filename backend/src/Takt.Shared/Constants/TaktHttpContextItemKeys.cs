// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktHttpContextItemKeys.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：HttpContext.Items 键名（审计操作人暂存，供登出等清会话后 SqlSugar 差异日志使用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// HttpContext.Items 键名
/// </summary>
public static class TaktHttpContextItemKeys
{
    /// <summary>
    /// 当前请求显式暂存的操作人用户 ID（long）
    /// </summary>
    public const string AuditOperatorUserId = "Takt.Audit.OperatorUserId";

    /// <summary>
    /// 当前请求显式暂存的操作人登录名
    /// </summary>
    public const string AuditOperatorUserName = "Takt.Audit.OperatorUserName";

    /// <summary>
    /// 当前请求审计 Remark（如登出时用户名未知的原因说明）
    /// </summary>
    public const string AuditContextRemark = "Takt.Audit.ContextRemark";
}
