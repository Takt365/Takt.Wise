// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktTenantContextOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：租户上下文配置（请求头名称；租户/公司须由前端显式传入或 JWT/用户关联表解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Shared.Options;

/// <summary>
/// 租户上下文配置
/// </summary>
public class TaktTenantContextOptions
{
    public const string SectionName = "TenantContext";

    /// <summary>
    /// 租户请求头名称
    /// </summary>
    public string TenantHeaderName { get; set; } = TaktHttpHeaderNames.TenantCode;

    /// <summary>
    /// 公司请求头名称
    /// </summary>
    public string CompanyHeaderName { get; set; } = TaktHttpHeaderNames.CompanyCode;

    /// <summary>
    /// 验证配置是否完整
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(TenantHeaderName))
        {
            throw new InvalidOperationException($"{SectionName}:TenantHeaderName 未配置");
        }

        if (string.IsNullOrWhiteSpace(CompanyHeaderName))
        {
            throw new InvalidOperationException($"{SectionName}:CompanyHeaderName 未配置");
        }
    }
}
