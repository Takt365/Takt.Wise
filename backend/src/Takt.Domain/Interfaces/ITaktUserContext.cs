// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktUserContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户上下文接口，提供当前登录用户信息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 用户上下文接口
/// 提供当前登录用户的身份信息
/// </summary>
public interface ITaktUserContext
{
    /// <summary>
    /// 当前用户ID
    /// </summary>
    long? UserId { get; }

    /// <summary>
    /// 当前用户名
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// 当前租户编码
    /// </summary>
    string? TenantCode { get; }

    /// <summary>
    /// 当前公司编码
    /// </summary>
    string? CompanyCode { get; }

    /// <summary>
    /// 请求头中的公司编码（未传则为 null，不含默认公司回退）
    /// </summary>
    string? RequestCompanyCode { get; }

    /// <summary>
    /// 是否已认证
    /// </summary>
    bool IsAuthenticated { get; }
}
