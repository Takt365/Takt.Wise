// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktHttpAuditHelper.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：HTTP 请求审计字段解析（操作模块、操作类型，对齐 TaktConstants.OperType）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// HTTP 请求审计辅助类（纯函数，无 I/O）
/// </summary>
public static class TaktHttpAuditHelper
{
    /// <summary>
    /// 从 HTTP 上下文解析操作类型
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>操作类型（TaktConstants.OperType）</returns>
    public static string ResolveOperType(HttpContext? context)
    {
        if (context == null)
        {
            return TaktConstants.OperType.Unknown;
        }

        return ResolveOperType(context.Request.Method, context.Request.Path.Value);
    }

    /// <summary>
    /// 根据请求方式与路径解析操作类型
    /// </summary>
    /// <param name="requestMethod">HTTP 方法</param>
    /// <param name="path">请求路径（可含查询串）</param>
    /// <returns>操作类型（TaktConstants.OperType）</returns>
    public static string ResolveOperType(string? requestMethod, string? path)
    {
        var normalizedPath = (path ?? string.Empty).Trim();
        if (ContainsPathSegment(normalizedPath, "export"))
        {
            return TaktConstants.OperType.Export;
        }

        if (ContainsPathSegment(normalizedPath, "import"))
        {
            return TaktConstants.OperType.Import;
        }

        if (ContainsPathSegment(normalizedPath, "template"))
        {
            return TaktConstants.OperType.Import;
        }

        if (ContainsPathSegment(normalizedPath, "batch"))
        {
            return TaktConstants.OperType.Delete;
        }

        if (ContainsPathSegment(normalizedPath, "status"))
        {
            return TaktConstants.OperType.Update;
        }

        if (ContainsPathSegment(normalizedPath, "tree-options")
            || ContainsPathSegment(normalizedPath, "options")
            || ContainsPathSegment(normalizedPath, "/list"))
        {
            return TaktConstants.OperType.Query;
        }

        var method = (requestMethod ?? string.Empty).Trim().ToUpperInvariant();
        return method switch
        {
            "GET" or "HEAD" => TaktConstants.OperType.Query,
            "POST" => TaktConstants.OperType.Create,
            "PUT" or "PATCH" => TaktConstants.OperType.Update,
            "DELETE" => TaktConstants.OperType.Delete,
            _ => TaktConstants.OperType.Unknown,
        };
    }

    /// <summary>
    /// 从请求路径解析操作模块（通常为 API 控制器段，如 TaktUsers）
    /// </summary>
    /// <param name="path">请求路径</param>
    /// <returns>操作模块名；无法解析时返回空串</returns>
    public static string ResolveOperModule(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length >= 2
            && string.Equals(segments[0], "api", StringComparison.OrdinalIgnoreCase))
        {
            return segments[1];
        }

        return segments.Length > 0 ? segments[0] : string.Empty;
    }

    /// <summary>
    /// 从 HTTP 上下文解析客户端 IP 与规范化地点（TaktLocationHelper + 内网回退，日志域唯一入口）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="maxLength">地点最大长度</param>
    /// <returns>IP 与地点（均非 null）</returns>
    public static (string Ip, string Location) ResolveClientIpAndLocation(HttpContext? context, int maxLength = 200)
    {
        var (ip, rawLocation) = TaktLocationHelper.ResolveClientIpAndLocationForLog(context, null, maxLength);
        var location = ResolveLocationForLog(ip, rawLocation, maxLength);
        return (ip ?? string.Empty, location);
    }

    /// <summary>
    /// 根据 IP 与可选已有地点解析规范化地点（无 HttpContext 时的唯一入口）
    /// </summary>
    /// <param name="ip">IP 地址</param>
    /// <param name="existingLocation">已有地点</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>地点文案（非 null）</returns>
    public static string ResolveLocationFromIp(string? ip, string? existingLocation = null, int maxLength = 200)
    {
        return ResolveLocationForLog(ip, existingLocation, maxLength);
    }

    /// <summary>
    /// HTTP 客户端审计上下文（IP、地点、User-Agent、Browser、Os、DeviceType）
    /// </summary>
    /// <param name="Ip">客户端 IP</param>
    /// <param name="Location">规范化地点</param>
    /// <param name="UserAgent">User-Agent 原文（最长 500）</param>
    /// <param name="Browser">浏览器</param>
    /// <param name="OperatingSystem">操作系统</param>
    /// <param name="DeviceType">登录设备</param>
    public readonly record struct ClientLogContext(
        string Ip,
        string Location,
        string UserAgent,
        string Browser,
        string OperatingSystem,
        string DeviceType);

    /// <summary>
    /// 从 HTTP 上下文解析客户端审计字段（日志/在线域写入唯一入口）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>客户端审计上下文</returns>
    public static ClientLogContext ResolveClientLogContext(HttpContext? context)
    {
        var (ip, location) = ResolveClientIpAndLocation(context);
        var userAgentRaw = TaktUserAgentHelper.ResolveUserAgent(context);
        var userAgent = string.IsNullOrWhiteSpace(userAgentRaw)
            ? string.Empty
            : userAgentRaw.Length > 500 ? userAgentRaw[..500] : userAgentRaw;
        var profile = TaktUserAgentHelper.ResolveFromHttpContext(context);
        return new ClientLogContext(
            ip,
            location,
            userAgent,
            profile.Browser,
            profile.OperatingSystem,
            profile.DeviceType);
    }

    /// <summary>
    /// 持久化层数据变更的操作类型（insert/delete/其他→update）
    /// </summary>
    /// <param name="isInsert">是否为插入</param>
    /// <param name="isDelete">是否为删除</param>
    /// <returns>操作类型（TaktConstants.OperType）</returns>
    public static string ResolveOperTypeFromEntityChange(bool isInsert, bool isDelete)
    {
        if (isDelete)
        {
            return TaktConstants.OperType.Delete;
        }

        if (isInsert)
        {
            return TaktConstants.OperType.Create;
        }

        return TaktConstants.OperType.Update;
    }

    /// <summary>
    /// 规范化日志地点：优先已有值，否则根据 IP 解析；内网/本机 IP 回退为可读文案
    /// </summary>
    /// <param name="ip">IP 地址</param>
    /// <param name="existingLocation">已有地点</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>地点文案（非 null）</returns>
    public static string ResolveLocationForLog(string? ip, string? existingLocation, int maxLength = 200)
    {
        var location = TaktLocationHelper.ResolveIpLocationForLogOrKeep(ip, existingLocation, maxLength);
        if (!string.IsNullOrWhiteSpace(location))
        {
            return location;
        }

        var trimmedIp = ip?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedIp))
        {
            return string.Empty;
        }

        if (IsLoopbackOrPrivateIp(trimmedIp))
        {
            return "内网";
        }

        return trimmedIp.Length <= maxLength ? trimmedIp : trimmedIp[..maxLength];
    }

    /// <summary>
    /// 路径是否包含指定段（忽略大小写，按 / 分段匹配）
    /// </summary>
    private static bool ContainsPathSegment(string path, string segment)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(segment);
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var queryIndex = path.IndexOf('?', StringComparison.Ordinal);
        var pathOnly = queryIndex >= 0 ? path[..queryIndex] : path;
        var segments = pathOnly.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments.Any(x => string.Equals(x, segment, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 是否为回环或私有 IPv4 地址
    /// </summary>
    private static bool IsLoopbackOrPrivateIp(string ip)
    {
        if (ip == "::1"
            || string.Equals(ip, "localhost", StringComparison.OrdinalIgnoreCase)
            || ip.StartsWith("127.", StringComparison.Ordinal))
        {
            return true;
        }

        if (!System.Net.IPAddress.TryParse(ip, out var address))
        {
            return false;
        }

        var bytes = address.GetAddressBytes();
        if (bytes.Length != 4)
        {
            return false;
        }

        if (bytes[0] == 10)
        {
            return true;
        }

        if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
        {
            return true;
        }

        if (bytes[0] == 192 && bytes[1] == 168)
        {
            return true;
        }

        return false;
    }
}
