// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktClaimNames.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：JWT / Cookie Claims 键名（租户、公司与 TaktUserContext 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 平台 JWT / Cookie 自定义 Claim 名称
/// </summary>
public static class TaktClaimNames
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public const string TenantCode = "tenant_code";

    /// <summary>
    /// 公司编码
    /// </summary>
    public const string CompanyCode = "company_code";

    /// <summary>
    /// 登录账号（OpenID Connect preferred_username / OpenIddict Claims.PreferredUsername）
    /// </summary>
    public const string PreferredUsername = "preferred_username";
}
