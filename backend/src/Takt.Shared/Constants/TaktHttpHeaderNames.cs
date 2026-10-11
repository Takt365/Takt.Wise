// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktHttpHeaderNames.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：HTTP 请求头与 SignalR 查询参数名（客户端画像传递，与 TaktUserAgentHelper 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// HTTP 客户端画像请求头名称
/// </summary>
public static class TaktHttpHeaderNames
{
    /// <summary>
    /// 租户编码（与 TenantContext:TenantHeaderName 默认一致）
    /// </summary>
    public const string TenantCode = "X-Tenant-Code";

    /// <summary>
    /// 公司编码（与 TenantContext:CompanyHeaderName 默认一致）
    /// </summary>
    public const string CompanyCode = "X-Company-Code";

    /// <summary>
    /// 显式 User-Agent（代理/WebSocket 场景下 User-Agent 为空时的回退）
    /// </summary>
    public const string ClientUserAgent = "X-Takt-User-Agent";

    /// <summary>
    /// 显式浏览器（TaktConstants.BrowserType）
    /// </summary>
    public const string ClientBrowser = "X-Takt-Client-Browser";

    /// <summary>
    /// 显式操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public const string ClientOperatingSystem = "X-Takt-Client-Os";

    /// <summary>
    /// 显式登录设备（TaktConstants.DeviceType）
    /// </summary>
    public const string ClientDeviceType = "X-Takt-Client-Device";
}

/// <summary>
/// SignalR WebSocket 无法携带自定义 Header 时使用的查询参数名（由 TaktSignalRTokenMiddleware 提升为请求头）
/// </summary>
public static class TaktHttpQueryNames
{
    /// <summary>
    /// 登录预览等场景的租户编码（Header 缺失时的回退）
    /// </summary>
    public const string TenantCode = "tenantCode";

    /// <summary>
    /// 对应 X-Takt-User-Agent
    /// </summary>
    public const string ClientUserAgent = "takt_user_agent";

    /// <summary>
    /// 对应 X-Takt-Client-Browser
    /// </summary>
    public const string ClientBrowser = "takt_client_browser";

    /// <summary>
    /// 对应 X-Takt-Client-Os
    /// </summary>
    public const string ClientOperatingSystem = "takt_client_os";

    /// <summary>
    /// 对应 X-Takt-Client-Device
    /// </summary>
    public const string ClientDeviceType = "takt_client_device";
}
