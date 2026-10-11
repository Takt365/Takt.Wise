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
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Interfaces;
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
    /// 构造函数
    /// </summary>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIpGeolocationService(
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
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
            FullAddress = result.FullAddress,
            FormattedAddress = result.FormattedAddress,
        };
    }
}
