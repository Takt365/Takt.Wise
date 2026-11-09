// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktIpGeolocationService.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：IP 归属查询应用服务（封装 TaktLocationHelper）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Interfaces;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// IP 归属查询应用服务
/// </summary>
public class TaktIpGeolocationService : TaktServiceBase, ITaktIpGeolocationService
{
    /// <summary>
    /// IPv6 文本最大长度（含压缩与 zone 常见上限）
    /// </summary>
    private const int MaxIpLength = 45;

    /// <summary>
    /// 出口公网 echo（本进程 NAT 对外地址；浏览器侧另有前端直连探测）
    /// </summary>
    private static readonly string[] PublicIpEchoUrls =
    [
        "https://api.ipify.org?format=json",
        "https://api64.ipify.org?format=json",
    ];

    private readonly IHttpClientFactory? _httpClientFactory;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    /// <param name="httpClientFactory">HTTP 客户端工厂（公网 echo 回退）</param>
    public TaktIpGeolocationService(
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null,
        IHttpClientFactory? httpClientFactory = null)
        : base(userContext, localizationService)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 按 IP 查询归属地（ip2region 离线库）
    /// </summary>
    /// <param name="ip">IPv4 或 IPv6</param>
    /// <returns>归属结果；格式非法时抛业务异常；未命中时 Found=false</returns>
    public async Task<TaktIpGeolocationDto> SearchIpGeolocationAsync(string ip)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ip);
        var trimmed = ip.Trim();
        if (trimmed.Length > MaxIpLength)
        {
            ThrowBusinessException("IP 地址长度不能超过 45 个字符");
        }

        if (!IPAddress.TryParse(trimmed, out _))
        {
            ThrowBusinessException("IP 地址格式无效");
        }

        try
        {
            var result = await TaktLocationHelper.SearchAsync(trimmed);
            if (result == null)
            {
                return new TaktIpGeolocationDto
                {
                    Ip = trimmed,
                    Found = false,
                };
            }

            return new TaktIpGeolocationDto
            {
                Ip = result.Ip,
                Found = true,
                Country = result.Country,
                Region = result.Region,
                Province = result.Province,
                City = result.City,
                Isp = result.Isp,
                IsoCountryCode = result.IsoCountryCode,
                FullAddress = result.FullAddress,
                FormattedAddress = result.FormattedAddress,
            };
        }
        catch (InvalidOperationException ex)
        {
            // 公网依赖离线库；未加载时勿静默 Found=false（否则表现为「只能查内网」）
            throw new TaktBusinessException(ex.Message);
        }
    }

    /// <summary>
    /// 按本机已激活网卡 IP 查询归属（排除回环/隧道/链路本地；优先 IPv4）
    /// </summary>
    /// <returns>优先网卡 IP 的归属结果，并在 ActiveHostIps 中列出全部激活地址</returns>
    public async Task<TaktIpGeolocationDto> SearchActiveHostIpGeolocationAsync()
    {
        var activeIps = TaktLocationHelper.GetActiveHostIpAddresses();
        if (activeIps.Count == 0)
        {
            ThrowBusinessException("未检测到已激活网卡上的有效 IP（已排除回环、隧道与链路本地地址）");
        }

        var preferred = activeIps[0];
        var dto = await SearchIpGeolocationAsync(preferred);
        dto.ActiveHostIps = activeIps.ToList();
        return dto;
    }

    /// <summary>
    /// 解析访问者公网 IP 并查归属：优先请求侧公网地址，否则探测 API 进程出口公网 IP
    /// </summary>
    /// <param name="requestClientIp">当前请求客户端 IP</param>
    /// <returns>公网 IP 归属结果</returns>
    public async Task<TaktIpGeolocationDto> SearchVisitorPublicIpGeolocationAsync(string? requestClientIp)
    {
        string? publicIp = null;
        if (TaktLocationHelper.IsPublicRoutableIp(requestClientIp))
        {
            publicIp = requestClientIp!.Trim();
        }
        else
        {
            publicIp = await FetchEgressPublicIpAsync();
        }

        if (string.IsNullOrWhiteSpace(publicIp))
        {
            ThrowBusinessException("无法获取公网 IP（请求非公网且出口探测失败）");
        }

        return await SearchIpGeolocationAsync(publicIp);
    }

    /// <summary>
    /// 经公网 echo 探测本进程出口 IP
    /// </summary>
    /// <returns>公网 IP；失败为 null</returns>
    private async Task<string?> FetchEgressPublicIpAsync()
    {
        if (_httpClientFactory == null)
        {
            return null;
        }

        var client = _httpClientFactory.CreateClient(nameof(TaktIpGeolocationService));
        foreach (var url in PublicIpEchoUrls)
        {
            try
            {
                using var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var payload = await response.Content.ReadFromJsonAsync<IpifyEchoDto>();
                var ip = payload?.Ip?.Trim();
                if (TaktLocationHelper.IsPublicRoutableIp(ip))
                {
                    return ip;
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Debug(ex, "[TaktIpGeolocation] 公网 echo 失败: {Url}", url);
            }
        }

        return null;
    }

    /// <summary>
    /// ipify JSON 响应
    /// </summary>
    private sealed class IpifyEchoDto
    {
        /// <summary>
        /// 公网 IP
        /// </summary>
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }
    }
}
