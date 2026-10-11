// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktIpLocationCollectionExtensions.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：IP2Region 离线库启动初始化（供 LoginLocation / OperLocation 解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Builder;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// IP 定位数据库启动扩展
/// </summary>
public static class TaktIpLocationCollectionExtensions
{
    /// <summary>
    /// 初始化 TaktLocationHelper（读取 wwwroot/Region 下 ip2region xdb）
    /// IPv4 库缺失时记录警告并跳过；IPv6 库可选
    /// </summary>
    /// <param name="app">Web 应用（WebRootPath 下须有 Region/ip2region_v4.xdb）</param>
    public static void InitializeTaktIpLocationDatabase(this WebApplication app)
    {
        var regionDir = Path.Combine(app.Environment.WebRootPath, "Region");
        var ipv4Path = Path.Combine(regionDir, "ip2region_v4.xdb");
        var ipv6Path = Path.Combine(regionDir, "ip2region_v6.xdb");

        if (!File.Exists(ipv4Path))
        {
            TaktLogger.Warning(
                "[TaktLocationHelper] 未找到 IPv4 数据库 {Ipv4Path}，LoginLocation/OperLocation 将仅在调用方已传入时落库",
                ipv4Path);
            return;
        }

        var ipv6 = File.Exists(ipv6Path) ? ipv6Path : null;
        TaktLocationHelper.Initialize(ipv4Path, ipv6);
    }
}
