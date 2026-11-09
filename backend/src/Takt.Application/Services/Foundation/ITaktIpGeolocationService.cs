// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktIpGeolocationService.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：IP 归属查询应用服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// IP 归属查询应用服务
/// </summary>
public interface ITaktIpGeolocationService
{
    /// <summary>
    /// 按 IP 查询归属地（ip2region 离线库）
    /// </summary>
    /// <param name="ip">IPv4 或 IPv6</param>
    /// <returns>归属结果；格式非法时抛业务异常；未命中时 Found=false</returns>
    Task<TaktIpGeolocationDto> SearchIpGeolocationAsync(string ip);

    /// <summary>
    /// 按本机已激活网卡 IP 查询归属（排除回环/隧道/链路本地；优先 IPv4）
    /// </summary>
    /// <returns>优先网卡 IP 的归属结果，并在 ActiveHostIps 中列出全部激活地址</returns>
    Task<TaktIpGeolocationDto> SearchActiveHostIpGeolocationAsync();

    /// <summary>
    /// 解析访问者公网 IP 并查归属：优先 HTTP 侧公网地址，否则探测本进程出口公网 IP
    /// </summary>
    /// <param name="requestClientIp">当前请求客户端 IP（可为 ::1 / 内网）</param>
    /// <returns>公网 IP 归属结果</returns>
    Task<TaktIpGeolocationDto> SearchVisitorPublicIpGeolocationAsync(string? requestClientIp);
}
