// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktUserAgentHelper.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：从 User-Agent 解析浏览器、操作系统、登录设备（对齐 TaktConstants）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// User-Agent 解析辅助类（纯函数，无 I/O）
/// </summary>
public static class TaktUserAgentHelper
{
    /// <summary>
    /// User-Agent 解析结果
    /// </summary>
    /// <param name="Browser">浏览器（TaktConstants.BrowserType）</param>
    /// <param name="OperatingSystem">操作系统（TaktConstants.OperatingSystem）</param>
    /// <param name="DeviceType">登录设备（TaktConstants.DeviceType）</param>
    public readonly record struct UserAgentProfile(string Browser, string OperatingSystem, string DeviceType);

    /// <summary>
    /// 从 HTTP 请求头解析 User-Agent 原文（优先 X-Takt-User-Agent，其次标准 User-Agent）
    /// </summary>
    /// <param name="headers">请求头集合</param>
    /// <returns>User-Agent 原文；缺失时返回空串</returns>
    public static string ResolveUserAgent(IHeaderDictionary? headers)
    {
        if (headers == null)
        {
            return string.Empty;
        }

        var custom = headers[TaktHttpHeaderNames.ClientUserAgent].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(custom))
        {
            return custom.Trim();
        }

        return headers.UserAgent.ToString();
    }

    /// <summary>
    /// 从 HTTP 请求头合并显式客户端字段与 User-Agent 解析结果
    /// </summary>
    /// <param name="headers">请求头集合</param>
    /// <returns>合并后的客户端画像</returns>
    public static UserAgentProfile ResolveFromHttpHeaders(IHeaderDictionary? headers)
    {
        if (headers == null)
        {
            return UnknownProfile();
        }

        var userAgent = ResolveUserAgent(headers);
        var browser = headers[TaktHttpHeaderNames.ClientBrowser].FirstOrDefault();
        var operatingSystem = headers[TaktHttpHeaderNames.ClientOperatingSystem].FirstOrDefault();
        var deviceType = headers[TaktHttpHeaderNames.ClientDeviceType].FirstOrDefault();
        return Resolve(userAgent, browser, operatingSystem, deviceType);
    }

    /// <summary>
    /// 从 HTTP 上下文解析客户端画像
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>合并后的客户端画像</returns>
    public static UserAgentProfile ResolveFromHttpContext(HttpContext? context)
    {
        return ResolveFromHttpHeaders(context?.Request.Headers);
    }

    /// <summary>
    /// 从 HTTP 上下文解析 User-Agent 原文
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>User-Agent 原文</returns>
    public static string ResolveUserAgent(HttpContext? context)
    {
        return ResolveUserAgent(context?.Request.Headers);
    }

    /// <summary>
    /// 从 User-Agent 字符串解析浏览器、操作系统与登录设备
    /// </summary>
    /// <param name="userAgent">HTTP User-Agent；为空时返回 unknown</param>
    /// <returns>解析结果，值均对齐 TaktConstants</returns>
    public static UserAgentProfile Parse(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return UnknownProfile();
        }

        var ua = userAgent;
        var operatingSystem = ResolveOperatingSystem(ua);
        var browser = ResolveBrowser(ua);
        var deviceType = ResolveDeviceType(ua);
        return new UserAgentProfile(browser, operatingSystem, deviceType);
    }

    /// <summary>
    /// 合并显式客户端字段与 User-Agent 解析结果（显式 unknown/空时回退解析值）
    /// </summary>
    /// <param name="userAgent">HTTP User-Agent</param>
    /// <param name="browser">显式浏览器（可选）</param>
    /// <param name="operatingSystem">显式操作系统（可选）</param>
    /// <param name="deviceType">显式登录设备（可选）</param>
    /// <returns>合并后的客户端画像</returns>
    public static UserAgentProfile Resolve(
        string? userAgent,
        string? browser = null,
        string? operatingSystem = null,
        string? deviceType = null)
    {
        var parsed = Parse(userAgent);
        return new UserAgentProfile(
            CoalesceClientHint(browser, parsed.Browser, TaktConstants.BrowserType.Unknown, TaktConstants.BrowserType.IsValid),
            CoalesceClientHint(operatingSystem, parsed.OperatingSystem, TaktConstants.OperatingSystem.Unknown, TaktConstants.OperatingSystem.IsValid),
            CoalesceClientHint(deviceType, parsed.DeviceType, TaktConstants.DeviceType.Unknown, TaktConstants.DeviceType.IsValid));
    }

    /// <summary>
    /// 当 Browser/Os/DeviceType 为 unknown 或空时，根据 UserAgent 回填
    /// </summary>
    /// <param name="userAgent">User-Agent 原文</param>
    /// <param name="browser">当前浏览器</param>
    /// <param name="operatingSystem">当前操作系统</param>
    /// <param name="deviceType">当前登录设备</param>
    /// <returns>回填后的 (Browser, OperatingSystem, DeviceType)</returns>
    public static (string Browser, string OperatingSystem, string DeviceType) FillMissingFromUserAgent(
        string? userAgent,
        string browser,
        string operatingSystem,
        string deviceType)
    {
        var profile = Resolve(userAgent, browser, operatingSystem, deviceType);
        return (profile.Browser, profile.OperatingSystem, profile.DeviceType);
    }

    /// <summary>
    /// 当 Browser/Os 为 unknown 或空时，根据 UserAgent 回填（无 DeviceType 字段的实体）
    /// </summary>
    /// <param name="userAgent">User-Agent 原文</param>
    /// <param name="browser">当前浏览器</param>
    /// <param name="operatingSystem">当前操作系统</param>
    /// <returns>回填后的 (Browser, OperatingSystem)</returns>
    public static (string Browser, string OperatingSystem) FillBrowserOsFromUserAgent(
        string? userAgent,
        string browser,
        string operatingSystem)
    {
        var profile = Resolve(userAgent, browser, operatingSystem);
        return (profile.Browser, profile.OperatingSystem);
    }

    /// <summary>
    /// 优先使用显式客户端字段，否则使用 User-Agent 解析值
    /// </summary>
    private static string CoalesceClientHint(
        string? explicitValue,
        string parsedValue,
        string unknownValue,
        Func<string?, bool> isValid)
    {
        var trimmed = explicitValue?.Trim();
        if (!string.IsNullOrWhiteSpace(trimmed)
            && isValid(trimmed)
            && !string.Equals(trimmed, unknownValue, StringComparison.OrdinalIgnoreCase))
        {
            return trimmed;
        }

        return parsedValue;
    }

    /// <summary>
    /// 解析浏览器
    /// </summary>
    /// <param name="userAgent">User-Agent 原文</param>
    /// <returns>TaktConstants.BrowserType 常量值</returns>
    private static string ResolveBrowser(string userAgent)
    {
        if (userAgent.Contains("Edg/", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("Edge/", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.BrowserType.Edge;
        }

        if (userAgent.Contains("Firefox/", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.BrowserType.Firefox;
        }

        if (userAgent.Contains("Chrome/", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("CriOS/", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.BrowserType.Chrome;
        }

        if (userAgent.Contains("Safari/", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.BrowserType.Safari;
        }

        return TaktConstants.BrowserType.Unknown;
    }

    /// <summary>
    /// 解析操作系统
    /// </summary>
    /// <param name="userAgent">User-Agent 原文</param>
    /// <returns>TaktConstants.OperatingSystem 常量值</returns>
    private static string ResolveOperatingSystem(string userAgent)
    {
        if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("iPod", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.OperatingSystem.Ios;
        }

        if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.OperatingSystem.Android;
        }

        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.OperatingSystem.Windows;
        }

        if (userAgent.Contains("Mac OS X", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("Macintosh", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.OperatingSystem.MacOs;
        }

        if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.OperatingSystem.Linux;
        }

        return TaktConstants.OperatingSystem.Unknown;
    }

    /// <summary>
    /// 解析登录设备
    /// </summary>
    /// <param name="userAgent">User-Agent 原文</param>
    /// <returns>TaktConstants.DeviceType 常量值</returns>
    private static string ResolveDeviceType(string userAgent)
    {
        if (userAgent.Contains("iPad", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.DeviceType.Tablet;
        }

        if (userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("iPod", StringComparison.OrdinalIgnoreCase)
            || userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            return TaktConstants.DeviceType.Mobile;
        }

        return TaktConstants.DeviceType.Pc;
    }

    /// <summary>
    /// 未知 User-Agent 解析结果
    /// </summary>
    /// <returns>三项均为 unknown 的配置</returns>
    private static UserAgentProfile UnknownProfile()
    {
        return new UserAgentProfile(
            TaktConstants.BrowserType.Unknown,
            TaktConstants.OperatingSystem.Unknown,
            TaktConstants.DeviceType.Unknown);
    }
}
