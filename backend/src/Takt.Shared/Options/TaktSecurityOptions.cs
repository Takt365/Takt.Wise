// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktSecurityOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：安全配置选项；appsettings 覆盖本类默认值
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 安全配置选项
/// </summary>
public class TaktSecurityOptions
{
    public const string SectionName = "Security";

    /// <summary>
    /// 频率限制配置
    /// </summary>
    public TaktRateLimitOptions RateLimit { get; set; } = new();

    /// <summary>
    /// 登录端点专用限流（verify-password、signin）
    /// </summary>
    public TaktRateLimitOptions LoginRateLimit { get; set; } = new()
    {
        Enabled = true,
        MaxRequests = 10,
        TimeWindowSeconds = 60,
    };

    /// <summary>
    /// CSRF 防护配置
    /// </summary>
    public TaktCsrfProtectionOptions CsrfProtection { get; set; } = new();

    /// <summary>
    /// XSS 防护配置
    /// </summary>
    public TaktXssProtectionOptions XssProtection { get; set; } = new();

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        RateLimit.Validate();
        LoginRateLimit.Validate();
        XssProtection.Validate();
    }
}

/// <summary>
/// 频率限制配置
/// </summary>
public class TaktRateLimitOptions
{
    /// <summary>
    /// 是否启用全局限流
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 最大请求数
    /// </summary>
    public int MaxRequests { get; set; } = 1000;

    /// <summary>
    /// 时间窗口（秒）
    /// </summary>
    public int TimeWindowSeconds { get; set; } = 60;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (MaxRequests <= 0)
        {
            throw new InvalidOperationException($"{TaktSecurityOptions.SectionName}:RateLimit:MaxRequests 必须大于 0");
        }

        if (TimeWindowSeconds <= 0)
        {
            throw new InvalidOperationException($"{TaktSecurityOptions.SectionName}:RateLimit:TimeWindowSeconds 必须大于 0");
        }
    }
}

/// <summary>
/// CSRF 防护配置
/// </summary>
public class TaktCsrfProtectionOptions
{
    /// <summary>
    /// 是否启用 CSRF 防护
    /// </summary>
    public bool Enabled { get; set; } = true;
}

/// <summary>
/// XSS 防护配置
/// </summary>
public class TaktXssProtectionOptions
{
    /// <summary>
    /// 是否启用 XSS 防护
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 允许的文件扩展名
    /// </summary>
    public List<string> AllowedFileExtensions { get; set; } =
    [
        "zip", "rar", "7z", "tar", "gz", "bz2", "xz",
        "jpg", "jpeg", "png", "gif", "tif", "tiff", "svg",
        "mp4", "avi", "mov", "wmv", "flv", "mkv", "webm",
        "mp3", "wav", "flac", "aac", "ogg", "wma",
        "pdf", "docx", "xlsx", "pptx", "txt", "xml", "json"
    ];

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (Enabled && AllowedFileExtensions.Count == 0)
        {
            throw new InvalidOperationException(
                $"{TaktSecurityOptions.SectionName}:XssProtection:AllowedFileExtensions 在启用时不能为空");
        }
    }
}
