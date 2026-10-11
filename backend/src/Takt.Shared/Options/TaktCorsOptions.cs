// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktCorsOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：CORS 跨域配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// CORS 跨域配置选项
/// </summary>
public class CorsSettings
{
    public const string SectionName = "Cors";

    /// <summary>
    /// 允许的来源列表
    /// </summary>
    public string[] AllowedOrigins { get; set; } = null!;

    /// <summary>
    /// 允许的 HTTP 方法
    /// </summary>
    public string[] AllowedMethods { get; set; } = null!;

    /// <summary>
    /// 允许的请求头
    /// </summary>
    public string[] AllowedHeaders { get; set; } = null!;

    /// <summary>
    /// 是否允许携带凭据（Cookie、Authorization 头等）
    /// </summary>
    public bool AllowCredentials { get; set; }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (AllowedOrigins == null || AllowedOrigins.Length == 0)
        {
            throw new InvalidOperationException($"{SectionName}:AllowedOrigins 不能为空");
        }

        if (AllowedMethods == null || AllowedMethods.Length == 0)
        {
            throw new InvalidOperationException($"{SectionName}:AllowedMethods 不能为空");
        }

        if (AllowedHeaders == null || AllowedHeaders.Length == 0)
        {
            throw new InvalidOperationException($"{SectionName}:AllowedHeaders 不能为空");
        }
    }
}
