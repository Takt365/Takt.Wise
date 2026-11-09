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
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
    /// ISO 3166-1 alpha-2 国家码（xdb 第五段，如 CN / US；可空）
    /// </summary>
    public string IsoCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整地址原文（官方：国家|省份|城市|ISP|ISO）
    /// </summary>
    public string FullAddress { get; set; } = string.Empty;

    /// <summary>
    /// 格式化地址（国家 省份 城市 ISP；不含 ISO）
    /// </summary>
    public string FormattedAddress
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Country)) parts.Add(Country);
            var provinceOrRegion = !string.IsNullOrWhiteSpace(Province) ? Province : Region;
            if (!string.IsNullOrWhiteSpace(provinceOrRegion)) parts.Add(provinceOrRegion);
            if (!string.IsNullOrWhiteSpace(City)) parts.Add(City);
            if (!string.IsNullOrWhiteSpace(Isp)) parts.Add(Isp);
            return parts.Count > 0 ? string.Join(" ", parts) : "未知";
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
/// 非纯工具网关：启动时 Initialize 加载 xdb 并缓存 ISearcher。
/// NuGet：IP2Region.Net；用法对齐官方 <c>new Searcher(CachePolicy.Content, xdbPath)</c>（整库内存、线程安全）。
/// 查询算法（IP→字节、Vector、二分、读地域）全部委托官方 Searcher，与「XDB 搜索过程」四步一致；
/// 本类只做双库（v4/v6）初始化、回环/内网回退与地域串字段切分（不调用 AddIP2RegionService）。
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

                // 官方 NuGet：ISearcher searcher = new Searcher(CachePolicy.Content, path);
                _ipv4Searcher = new Searcher(CachePolicy.Content, ipv4DbPath);
                _ipv4DbPath = ipv4DbPath;
                TaktLogger.Information("[TaktLocationHelper] IPv4数据库初始化成功: {FilePath}（CachePolicy.Content）", ipv4DbPath);

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
            var trimmedIp = ip.Trim();
            // 验证IP地址格式；IPv4 映射的 IPv6（::ffff:x.x.x.x）归一为 IPv4，走 v4 库
            if (!IPAddress.TryParse(trimmedIp, out var ipAddress))
            {
                TaktLogger.Warning("[TaktLocationHelper] 无效的IP地址格式: {Ip}", trimmedIp);
                return null;
            }

            if (ipAddress.IsIPv4MappedToIPv6)
            {
                ipAddress = ipAddress.MapToIPv4();
                trimmedIp = ipAddress.ToString();
            }

            // 回环/RFC1918 等无公网归属：直接返回内网占位（不依赖 xdb）
            if (IsLocalOrPrivateIp(ipAddress))
            {
                return CreateLocalIpResult(trimmedIp);
            }

            // 公网 IP 必须已加载离线库；未初始化时抛出，由上层转业务提示（禁止静默 Found=false）
            EnsureInitialized();

            ISearcher? searcher = null;
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                searcher = _ipv4Searcher;
                if (searcher == null)
                {
                    throw new InvalidOperationException("IPv4 归属离线库未加载，无法查询公网 IP。请将 ip2region_v4.xdb 置于 wwwroot/Region 后重启。");
                }
            }
            else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                searcher = _ipv6Searcher;
                if (searcher == null)
                {
                    throw new InvalidOperationException("IPv6 归属离线库未加载，无法查询公网 IPv6。请将 ip2region_v6.xdb 置于 wwwroot/Region 后重启。");
                }
            }
            else
            {
                TaktLogger.Warning("[TaktLocationHelper] 不支持的IP地址类型: {Ip}", trimmedIp);
                return null;
            }

            // 官方 Searcher：parseIP → vector → 二分 → 读 utf-8 地域串（见 wwwroot/Region/README.md）
            var result = searcher.Search(trimmedIp);
            if (string.IsNullOrWhiteSpace(result))
            {
                TaktLogger.Debug("[TaktLocationHelper] 未找到IP地址位置信息: {Ip}", trimmedIp);
                return null;
            }

            return ParseLocationResult(trimmedIp, result);
        }
        catch (InvalidOperationException)
        {
            throw;
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
    /// 解析客户端 IP（登录/操作/访问等日志唯一推荐入口）
    /// </summary>
    /// <remarks>
    /// 优先 X-Forwarded-For，其次 RemoteIpAddress；若仅为回环（::1/127.*）则回退本机「已激活」网卡 IPv4（如 192.168.x.x）。
    /// <c>绝不</c>把 ::1 当作业务/日志客户端 IP。
    /// </remarks>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>可用客户端 IP；无法解析时为 null</returns>
    public static string? ResolveClientIp(HttpContext? context)
    {
        if (context == null)
        {
            return null;
        }

        string? candidate = null;
        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwarded))
        {
            candidate = forwarded.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
        }

        if (string.IsNullOrWhiteSpace(candidate))
        {
            var remote = context.Connection.RemoteIpAddress;
            if (remote != null)
            {
                if (remote.IsIPv4MappedToIPv6)
                {
                    remote = remote.MapToIPv4();
                }

                candidate = remote.ToString();
            }
        }

        candidate = NormalizeIpText(candidate ?? string.Empty);
        if (!string.IsNullOrWhiteSpace(candidate) && !IsLoopbackIpText(candidate))
        {
            return candidate;
        }

        // Vite/localhost 代理常见 ::1 —— 回退真实激活网卡，供 LoginIp/OperIp 等落库
        return ResolvePreferredActiveHostIp();
    }

    /// <summary>
    /// 枚举本机已激活网卡单播 IP：Up + 非回环/隧道/伪网卡/链路本地；有默认网关的 IPv4 优先
    /// </summary>
    /// <returns>去重排序后的 IP（绝不含 ::1 / 127.* / 169.254.*）</returns>
    public static IReadOnlyList<string> GetActiveHostIpAddresses()
    {
        var scored = new List<(int Score, string Ip)>();

        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus != OperationalStatus.Up)
            {
                continue;
            }

            if (nic.NetworkInterfaceType is NetworkInterfaceType.Loopback or NetworkInterfaceType.Tunnel)
            {
                continue;
            }

            if (IsFilterOrPseudoAdapterName(nic.Name) || IsFilterOrPseudoAdapterName(nic.Description))
            {
                continue;
            }

            IPInterfaceProperties props;
            try
            {
                props = nic.GetIPProperties();
            }
            catch (NetworkInformationException ex)
            {
                TaktLogger.Debug(ex, "[TaktLocationHelper] 读取网卡属性失败: {Nic}", nic.Name);
                continue;
            }

            var hasGateway = props.GatewayAddresses.Any(static g =>
                g.Address != null
                && !IPAddress.IsLoopback(g.Address)
                && !g.Address.Equals(IPAddress.Any)
                && !g.Address.Equals(IPAddress.IPv6Any));

            foreach (var uni in props.UnicastAddresses)
            {
                var address = uni.Address;
                if (address.IsIPv4MappedToIPv6)
                {
                    address = address.MapToIPv4();
                }

                if (!IsUsableHostUnicastAddress(address))
                {
                    continue;
                }

                var text = NormalizeIpText(address.ToString());
                if (string.IsNullOrWhiteSpace(text) || IsLoopbackIpText(text))
                {
                    continue;
                }

                var score = 0;
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    score += 100;
                }

                if (hasGateway)
                {
                    score += 50;
                }

                scored.Add((score, text));
            }
        }

        return scored
            .OrderByDescending(static x => x.Score)
            .Select(static x => x.Ip)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 本机优先激活 IP（有网关的 IPv4 优先）；无激活网卡时为 null
    /// </summary>
    /// <returns>优先 IP 或 null</returns>
    public static string? ResolvePreferredActiveHostIp()
    {
        var ips = GetActiveHostIpAddresses();
        return ips.Count > 0 ? ips[0] : null;
    }

    /// <summary>
    /// 是否为回环 IP 文本（::1 / 127.* / localhost）
    /// </summary>
    /// <param name="ip">IP 文本</param>
    /// <returns>回环则为 true</returns>
    public static bool IsLoopbackIpText(string? ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return false;
        }

        var text = NormalizeIpText(ip);
        if (string.Equals(text, "::1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(text, "localhost", StringComparison.OrdinalIgnoreCase)
            || text.StartsWith("127.", StringComparison.Ordinal))
        {
            return true;
        }

        return IPAddress.TryParse(text, out var address) && IPAddress.IsLoopback(address);
    }

    /// <summary>
    /// 可用主机单播地址（非回环、非链路本地）
    /// </summary>
    /// <param name="address">IP</param>
    /// <returns>可用则为 true</returns>
    private static bool IsUsableHostUnicastAddress(IPAddress address)
    {
        return !IPAddress.IsLoopback(address) && !IsLinkLocalAddress(address);
    }

    /// <summary>
    /// Windows 筛选器/伪适配器名称（无真实业务 IP）
    /// </summary>
    /// <param name="name">网卡名或描述</param>
    /// <returns>伪/过滤网卡则为 true</returns>
    private static bool IsFilterOrPseudoAdapterName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return name.Contains("LightWeight Filter", StringComparison.OrdinalIgnoreCase)
            || name.Contains("QoS Packet Scheduler", StringComparison.OrdinalIgnoreCase)
            || name.Contains("WFP ", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Virtual Box", StringComparison.OrdinalIgnoreCase)
            || name.Contains("VMware", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Hyper-V", StringComparison.OrdinalIgnoreCase)
            || name.Contains("vEthernet", StringComparison.OrdinalIgnoreCase)
            || name.Contains("Docker", StringComparison.OrdinalIgnoreCase)
            || name.Contains("WSL", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否为可路由公网 IP（非回环/RFC1918/链路本地等）
    /// </summary>
    /// <param name="ip">IP 文本</param>
    /// <returns>公网可路由则为 true</returns>
    public static bool IsPublicRoutableIp(string? ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return false;
        }

        if (!IPAddress.TryParse(NormalizeIpText(ip), out var address))
        {
            return false;
        }

        if (address.IsIPv4MappedToIPv6)
        {
            address = address.MapToIPv4();
        }

        return !IPAddress.IsLoopback(address)
            && !IsLocalOrPrivateIp(address)
            && !IsLinkLocalAddress(address);
    }

    /// <summary>
    /// 去掉 IPv6 zone id（如 %eth0），并去除首尾空白
    /// </summary>
    /// <param name="ip">原始 IP 文本</param>
    /// <returns>规范化文本</returns>
    private static string NormalizeIpText(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return string.Empty;
        }

        var text = ip.Trim();
        var zoneIndex = text.IndexOf('%');
        if (zoneIndex >= 0)
        {
            text = text[..zoneIndex];
        }

        return text;
    }

    /// <summary>
    /// 是否为链路本地地址（IPv4 169.254/16、IPv6 fe80::/10）
    /// </summary>
    /// <param name="address">IP</param>
    /// <returns>链路本地则为 true</returns>
    private static bool IsLinkLocalAddress(IPAddress address)
    {
        if (address.AddressFamily == AddressFamily.InterNetworkV6)
        {
            return address.IsIPv6LinkLocal;
        }

        if (address.AddressFamily == AddressFamily.InterNetwork)
        {
            var bytes = address.GetAddressBytes();
            return bytes.Length == 4 && bytes[0] == 169 && bytes[1] == 254;
        }

        return false;
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
    /// 解析地域信息（对应 xdb 第三层原文；官方：国家|省份|城市|ISP，新版可多 ISO）
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="result"><c>Searcher.Search</c> 返回的 utf-8 地域串</param>
    /// <returns>IP定位结果对象</returns>
    /// <remarks>
    /// 与官方文档一致：原始「开始IP|结束IP|地域信息」写入 xdb 的仅为地域段。
    /// 示例：<c>中国|福建省|福州市|电信</c>、<c>0|0|内网IP|内网IP</c>。
    /// 社区新数据偶发第五段 ISO（如 <c>…|CN</c>），按可选字段解析。
    /// </remarks>
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

        // 官方稳定格式：国家|省份|城市|ISP
        // 可选扩展：国家|省份|城市|ISP|ISO
        // ❌ 禁止「国家|区域|省份|城市|ISP」错位（省份会落到 Region，ISO 会当成 ISP）
        var parts = result.Split('|');
        if (parts.Length >= 1) location.Country = parts[0].Trim();
        if (parts.Length >= 2)
        {
            location.Province = parts[1].Trim();
            location.Region = location.Province;
        }
        if (parts.Length >= 3) location.City = parts[2].Trim();
        if (parts.Length >= 4) location.Isp = parts[3].Trim();
        if (parts.Length >= 5) location.IsoCountryCode = parts[4].Trim();

        location.Country = NormalizeRegionToken(location.Country);
        location.Region = NormalizeRegionToken(location.Region);
        location.Province = NormalizeRegionToken(location.Province);
        location.City = NormalizeRegionToken(location.City);
        location.Isp = NormalizeRegionToken(location.Isp);
        location.IsoCountryCode = NormalizeRegionToken(location.IsoCountryCode);

        // 仅四段且末段为两字母国家码时，视为 ISO 而非 ISP（兼容异常数据）
        if (string.IsNullOrWhiteSpace(location.IsoCountryCode)
            && location.Isp.Length == 2
            && location.Isp.All(static c => char.IsLetter(c)))
        {
            location.IsoCountryCode = location.Isp.ToUpperInvariant();
            location.Isp = string.Empty;
        }

        return location;
    }

    /// <summary>
    /// 将 xdb 占位符「0」规范为空串
    /// </summary>
    /// <param name="token">字段原值</param>
    /// <returns>规范化后的字段</returns>
    private static string NormalizeRegionToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token) || token == "0")
        {
            return string.Empty;
        }

        return token.Trim();
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
    /// 确保 IPv4 离线库已初始化（公网查询前置）
    /// </summary>
    /// <exception cref="InvalidOperationException">未加载 ip2region_v4.xdb</exception>
    private static void EnsureInitialized()
    {
        if (!_isInitialized || _ipv4Searcher == null)
        {
            throw new InvalidOperationException(
                "IP 归属离线库未加载：请将 ip2region_v4.xdb（及可选 ip2region_v6.xdb）放到 WebApi 的 wwwroot/Region 目录后重启应用。内网 IP 仍可查询，公网 IP 依赖该库。");
        }
    }
}
