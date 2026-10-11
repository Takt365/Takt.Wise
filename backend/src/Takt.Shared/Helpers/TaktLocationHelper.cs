// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktLocationHelper.cs
// 创建时间：2025-01-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt IP定位帮助类，使用 IP2Region.Net 进行离线IP地址定位
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Takt.Shared.Helpers;

/// <summary>
/// IP定位结果
/// </summary>
public class IpLocationResult
{
    /// <summary>
    /// IP地址
    /// </summary>
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 区域（省/州）
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// 省份
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// ISP（互联网服务提供商）
    /// </summary>
    public string Isp { get; set; } = string.Empty;

    /// <summary>
    /// 完整地址信息（格式：国家|区域|省份|城市|ISP）
    /// </summary>
    public string FullAddress { get; set; } = string.Empty;

    /// <summary>
    /// 格式化地址（用于显示）
    /// </summary>
    public string FormattedAddress
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Country)) parts.Add(Country);
            if (!string.IsNullOrWhiteSpace(Province)) parts.Add(Province);
            if (!string.IsNullOrWhiteSpace(City)) parts.Add(City);
            if (!string.IsNullOrWhiteSpace(Isp)) parts.Add(Isp);
            return parts.Any() ? string.Join(" ", parts) : "未知";
        }
    }

    /// <summary>
    /// 日志表用地点文案（国家-省份-城市，与实体字段说明一致）
    /// </summary>
    /// <returns>地点字符串；无法拼出有效段时返回 FormattedAddress</returns>
    public string ToLogLocationText()
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(Country))
        {
            parts.Add(Country);
        }

        if (!string.IsNullOrWhiteSpace(Province))
        {
            parts.Add(Province);
        }

        if (!string.IsNullOrWhiteSpace(City))
        {
            parts.Add(City);
        }

        if (parts.Count > 0)
        {
            return string.Join("-", parts);
        }

        var formatted = FormattedAddress;
        return string.IsNullOrWhiteSpace(formatted) || formatted == "未知" ? string.Empty : formatted;
    }
}

/// <summary>
/// Takt IP 定位帮助类（IP2Region 离线库）。
/// </summary>
/// <remarks>
/// 非纯工具网关：启动时 Initialize 加载 xdb 并缓存 ISearcher 实例；查询方法为 I/O 只读，失败返回 null 而非抛业务异常。
/// </remarks>
public static class TaktLocationHelper
{
    private static readonly object _lockObject = new object();
    private static ISearcher? _ipv4Searcher;
    private static ISearcher? _ipv6Searcher;
    private static string? _ipv4DbPath;
    private static string? _ipv6DbPath;
    private static bool _isInitialized = false;

    /// <summary>
    /// 初始化IP定位数据库
    /// </summary>
    /// <param name="ipv4DbPath">IPv4数据库文件路径（.xdb文件）</param>
    /// <param name="ipv6DbPath">IPv6数据库文件路径（.xdb文件，可选）</param>
    public static void Initialize(string ipv4DbPath, string? ipv6DbPath = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ipv4DbPath);

        lock (_lockObject)
        {
            try
            {
                // 初始化IPv4数据库
                if (!File.Exists(ipv4DbPath))
                {
                    TaktLogger.Warning("[TaktLocationHelper] IPv4数据库文件不存在: {FilePath}", ipv4DbPath);
                    throw new FileNotFoundException($"IPv4数据库文件不存在: {ipv4DbPath}");
                }

                _ipv4Searcher = new Searcher(CachePolicy.Content, ipv4DbPath);
                _ipv4DbPath = ipv4DbPath;
                TaktLogger.Information("[TaktLocationHelper] IPv4数据库初始化成功: {FilePath}", ipv4DbPath);

                // 初始化IPv6数据库（如果提供）
                if (!string.IsNullOrWhiteSpace(ipv6DbPath))
                {
                    if (!File.Exists(ipv6DbPath))
                    {
                        TaktLogger.Warning("[TaktLocationHelper] IPv6数据库文件不存在: {FilePath}，将仅使用IPv4数据库", ipv6DbPath);
                    }
                    else
                    {
                        _ipv6Searcher = new Searcher(CachePolicy.Content, ipv6DbPath);
                        _ipv6DbPath = ipv6DbPath;
                        TaktLogger.Information("[TaktLocationHelper] IPv6数据库初始化成功: {FilePath}", ipv6DbPath);
                    }
                }

                _isInitialized = true;
                TaktLogger.Information("[TaktLocationHelper] IP定位数据库初始化完成");
            }
            catch (Exception ex)
            {
                TaktLogger.Error(ex, "[TaktLocationHelper] IP定位数据库初始化失败");
                throw;
            }
        }
    }

    /// <summary>
    /// 查询IP地址位置信息
    /// </summary>
    /// <param name="ip">IP地址（IPv4或IPv6）</param>
    /// <returns>IP定位结果，如果查询失败返回null</returns>
    public static IpLocationResult? Search(string ip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ip);

        try
        {
            // 验证IP地址格式
            if (!IPAddress.TryParse(ip, out var ipAddress))
            {
                TaktLogger.Warning("[TaktLocationHelper] 无效的IP地址格式: {Ip}", ip);
                return null;
            }

            // 检查是否为本地/内网IP
            if (IsLocalOrPrivateIp(ipAddress))
            {
                return CreateLocalIpResult(ip);
            }

            // 确保已初始化
            EnsureInitialized();

            // 根据IP版本选择对应的搜索器
            ISearcher? searcher = null;
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                // IPv4
                searcher = _ipv4Searcher;
                if (searcher == null)
                {
                    TaktLogger.Warning("[TaktLocationHelper] IPv4数据库未初始化，无法查询: {Ip}", ip);
                    return null;
                }
            }
            else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                // IPv6
                searcher = _ipv6Searcher;
                if (searcher == null)
                {
                    TaktLogger.Warning("[TaktLocationHelper] IPv6数据库未初始化，无法查询IPv6地址: {Ip}", ip);
                    return null;
                }
            }
            else
            {
                TaktLogger.Warning("[TaktLocationHelper] 不支持的IP地址类型: {Ip}", ip);
                return null;
            }

            // 执行查询
            var result = searcher.Search(ip);
            if (string.IsNullOrWhiteSpace(result))
            {
                TaktLogger.Debug("[TaktLocationHelper] 未找到IP地址位置信息: {Ip}", ip);
                return null;
            }

            // 解析结果（格式：国家|区域|省份|城市|ISP）
            return ParseLocationResult(ip, result);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktLocationHelper] 查询IP地址位置信息失败: {Ip}", ip);
            return null;
        }
    }

    /// <summary>
    /// 查询IP地址位置信息（异步）
    /// </summary>
    /// <param name="ip">IP地址（IPv4或IPv6）</param>
    /// <returns>IP定位结果，如果查询失败返回null</returns>
    public static async Task<IpLocationResult?> SearchAsync(string ip)
    {
        return await Task.Run(() => Search(ip));
    }

    /// <summary>
    /// 批量查询IP地址位置信息
    /// </summary>
    /// <param name="ips">IP地址列表</param>
    /// <returns>IP定位结果字典（IP地址 -> 定位结果）</returns>
    public static Dictionary<string, IpLocationResult?> BatchSearch(IEnumerable<string> ips)
    {
        ArgumentNullException.ThrowIfNull(ips);

        var results = new Dictionary<string, IpLocationResult?>();
        foreach (var ip in ips)
        {
            if (!string.IsNullOrWhiteSpace(ip))
            {
                results[ip] = Search(ip);
            }
        }
        return results;
    }

    /// <summary>
    /// 批量查询IP地址位置信息（异步）
    /// </summary>
    /// <param name="ips">IP地址列表</param>
    /// <returns>IP定位结果字典（IP地址 -> 定位结果）</returns>
    public static async Task<Dictionary<string, IpLocationResult?>> BatchSearchAsync(IEnumerable<string> ips)
    {
        return await Task.Run(() => BatchSearch(ips));
    }

    /// <summary>
    /// 解析客户端 IP（优先 X-Forwarded-For 首段，否则 RemoteIpAddress；与操作/差异日志一致）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>客户端 IP；无法解析时为 null</returns>
    public static string? ResolveClientIp(HttpContext? context)
    {
        if (context == null)
        {
            return null;
        }

        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwarded))
        {
            var ip = forwarded.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip.Trim();
            }
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    /// <summary>
    /// 由 IP 解析日志/业务落库用的 IP 与地理位置（已有地点则保留）
    /// </summary>
    /// <param name="ip">IP 地址</param>
    /// <param name="existingLocation">已有地点</param>
    /// <param name="maxLength">地点字段最大长度</param>
    /// <returns>IP 与地点；地点无法解析时为 null</returns>
    public static (string? Ip, string? Location) ResolveIpAndLocationForLog(
        string? ip,
        string? existingLocation = null,
        int maxLength = 200)
    {
        var location = ResolveIpLocationForLogOrKeep(ip, existingLocation, maxLength);
        return (ip, location);
    }

    /// <summary>
    /// 由 HTTP 上下文解析客户端 IP 与日志/业务落库用的地理位置
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="existingLocation">已有地点</param>
    /// <param name="maxLength">地点字段最大长度</param>
    /// <returns>客户端 IP 与地点；无法解析时对应项为 null</returns>
    public static (string? Ip, string? Location) ResolveClientIpAndLocationForLog(
        HttpContext? context,
        string? existingLocation = null,
        int maxLength = 200)
    {
        var ip = ResolveClientIp(context);
        return ResolveIpAndLocationForLog(ip, existingLocation, maxLength);
    }

    /// <summary>
    /// 根据 IP 解析日志用登录/操作地点（数据库未初始化或解析失败时返回 null，不抛异常）
    /// </summary>
    /// <param name="ip">IP 地址</param>
    /// <param name="maxLength">写入日志字段的最大长度（默认 200）</param>
    /// <returns>地点文案；无法解析时为 null</returns>
    public static string? ResolveIpLocationForLog(string? ip, int maxLength = 200)
    {
        if (maxLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "maxLength 必须大于 0");
        }

        if (string.IsNullOrWhiteSpace(ip))
        {
            return null;
        }

        var trimmed = ip.Trim();
        if (string.Equals(trimmed, "unknown", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!IsInitialized())
        {
            return null;
        }

        try
        {
            var result = Search(trimmed);
            if (result == null)
            {
                return null;
            }

            var text = result.ToLogLocationText();
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (maxLength > 0 && text.Length > maxLength)
            {
                return text[..maxLength];
            }

            return text;
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "[TaktLocationHelper] 日志地点解析失败: {Ip}", trimmed);
            return null;
        }
    }

    /// <summary>
    /// 在地点为空时根据 IP 补全（已有地点则保留原值）
    /// </summary>
    /// <param name="ip">IP 地址</param>
    /// <param name="existingLocation">已有地点</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>补全后的地点</returns>
    public static string? ResolveIpLocationForLogOrKeep(string? ip, string? existingLocation, int maxLength = 200)
    {
        if (!string.IsNullOrWhiteSpace(existingLocation))
        {
            var kept = existingLocation.Trim();
            return maxLength > 0 && kept.Length > maxLength ? kept[..maxLength] : kept;
        }

        return ResolveIpLocationForLog(ip, maxLength);
    }

    /// <summary>
    /// 检查是否已初始化
    /// </summary>
    /// <returns>如果已初始化返回true</returns>
    public static bool IsInitialized()
    {
        lock (_lockObject)
        {
            return _isInitialized && _ipv4Searcher != null;
        }
    }

    /// <summary>
    /// 获取IPv4数据库路径
    /// </summary>
    /// <returns>IPv4数据库路径</returns>
    public static string? GetIpv4DbPath()
    {
        lock (_lockObject)
        {
            return _ipv4DbPath;
        }
    }

    /// <summary>
    /// 获取IPv6数据库路径
    /// </summary>
    /// <returns>IPv6数据库路径</returns>
    public static string? GetIpv6DbPath()
    {
        lock (_lockObject)
        {
            return _ipv6DbPath;
        }
    }

    /// <summary>
    /// 解析定位结果字符串
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="result">定位结果字符串（格式：国家|区域|省份|城市|ISP）</param>
    /// <returns>IP定位结果对象</returns>
    private static IpLocationResult ParseLocationResult(string ip, string result)
    {
        var location = new IpLocationResult
        {
            Ip = ip,
            FullAddress = result
        };

        if (string.IsNullOrWhiteSpace(result))
        {
            return location;
        }

        // 分割结果（格式：国家|区域|省份|城市|ISP）
        var parts = result.Split('|');
        if (parts.Length >= 1) location.Country = parts[0].Trim();
        if (parts.Length >= 2) location.Region = parts[1].Trim();
        if (parts.Length >= 3) location.Province = parts[2].Trim();
        if (parts.Length >= 4) location.City = parts[3].Trim();
        if (parts.Length >= 5) location.Isp = parts[4].Trim();

        // 处理"0"或空值（表示未知）
        if (location.Country == "0") location.Country = string.Empty;
        if (location.Region == "0") location.Region = string.Empty;
        if (location.Province == "0") location.Province = string.Empty;
        if (location.City == "0") location.City = string.Empty;
        if (location.Isp == "0") location.Isp = string.Empty;

        return location;
    }

    /// <summary>
    /// 检查是否为本地或私有IP地址
    /// </summary>
    /// <param name="ipAddress">IP地址</param>
    /// <returns>如果是本地或私有IP返回true</returns>
    private static bool IsLocalOrPrivateIp(IPAddress ipAddress)
    {
        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            // IPv4
            var bytes = ipAddress.GetAddressBytes();
            
            // 127.0.0.0/8 - 本地回环地址
            if (bytes[0] == 127)
            {
                return true;
            }
            
            // 10.0.0.0/8 - 私有网络
            if (bytes[0] == 10)
            {
                return true;
            }
            
            // 172.16.0.0/12 - 私有网络 (172.16.0.0 - 172.31.255.255)
            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
            {
                return true;
            }
            
            // 192.168.0.0/16 - 私有网络
            if (bytes[0] == 192 && bytes[1] == 168)
            {
                return true;
            }
            
            // 169.254.0.0/16 - 链路本地地址
            if (bytes[0] == 169 && bytes[1] == 254)
            {
                return true;
            }
            
            // 0.0.0.0 - 无效地址
            if (bytes[0] == 0 && bytes[1] == 0 && bytes[2] == 0 && bytes[3] == 0)
            {
                return true;
            }
        }
        else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            // IPv6
            var bytes = ipAddress.GetAddressBytes();
            
            // ::1 - IPv6本地回环地址
            if (bytes.Length == 16)
            {
                bool isLocalhost = true;
                for (int i = 0; i < 15; i++)
                {
                    if (bytes[i] != 0)
                    {
                        isLocalhost = false;
                        break;
                    }
                }
                if (isLocalhost && bytes[15] == 1)
                {
                    return true;
                }
            }
            
            // fe80::/10 - IPv6链路本地地址
            if (bytes.Length >= 1 && bytes[0] == 0xfe && (bytes[1] & 0xc0) == 0x80)
            {
                return true;
            }
            
            // fc00::/7 - IPv6私有地址
            if (bytes.Length >= 1 && (bytes[0] & 0xfe) == 0xfc)
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// 创建本地IP定位结果
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <returns>本地IP定位结果</returns>
    private static IpLocationResult CreateLocalIpResult(string ip)
    {
        return new IpLocationResult
        {
            Ip = ip,
            Country = "本地",
            Region = string.Empty,
            Province = string.Empty,
            City = string.Empty,
            Isp = "内网",
            FullAddress = "本地|内网"
            // FormattedAddress 是只读属性，会根据 Country、Province、City、Isp 自动计算
        };
    }

    /// <summary>
    /// 确保已初始化
    /// </summary>
    private static void EnsureInitialized()
    {
        if (!_isInitialized || _ipv4Searcher == null)
        {
            throw new InvalidOperationException("IP定位数据库未初始化，请先调用 Initialize 方法");
        }
    }
}
