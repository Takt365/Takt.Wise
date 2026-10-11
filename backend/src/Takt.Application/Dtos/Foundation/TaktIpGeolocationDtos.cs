// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktIpGeolocationDtos.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：IP 归属查询 DTO（基于 TaktLocationHelper / ip2region）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// IP 归属查询结果 DTO
/// </summary>
public class TaktIpGeolocationDto
{
    /// <summary>
    /// 查询的 IP 地址
    /// </summary>
    public string Ip { get; set; } = string.Empty;

    /// <summary>
    /// 是否命中定位结果（含内网占位结果）
    /// </summary>
    public bool Found { get; set; }

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
    /// 完整地址信息（国家|区域|省份|城市|ISP）
    /// </summary>
    public string FullAddress { get; set; } = string.Empty;

    /// <summary>
    /// 格式化地址（用于显示）
    /// </summary>
    public string FormattedAddress { get; set; } = string.Empty;
}
