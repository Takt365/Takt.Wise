// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktIpLocationCollectionExtensions.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：IP2Region 离线库启动初始化（供 LoginLocation / OperLocation / IP归属 解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Takt.Application.Services.Foundation;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// IP 定位数据库启动扩展
/// </summary>
public static class TaktIpLocationCollectionExtensions
{
    private const string Ipv4FileName = "ip2region_v4.xdb";
    private const string Ipv6FileName = "ip2region_v6.xdb";

    /// <summary>
    /// 注册 IP 归属相关 HttpClient（公网 echo 回退）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktIpGeolocationHttp(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(TaktIpGeolocationService), client =>
        {
            client.Timeout = TimeSpan.FromSeconds(8);
        });
        return services;
    }

    /// <summary>
    /// 初始化 TaktLocationHelper（在 WebRoot / ContentRoot / BaseDirectory 下查找 Region/ip2region_v4.xdb）
    /// IPv4 库缺失时记录警告并跳过（此时仅内网占位可查，公网查询将提示缺库）；IPv6 库可选
    /// </summary>
    /// <param name="app">Web 应用</param>
    public static void InitializeTaktIpLocationDatabase(this WebApplication app)
    {
        var regionDir = ResolveRegionDirectory(app);
        if (string.IsNullOrWhiteSpace(regionDir))
        {
            TaktLogger.Warning(
                "[TaktLocationHelper] 未找到 {Ipv4FileName}（已试 WebRoot/Region、ContentRoot/wwwroot/Region、BaseDirectory）。公网 IP 归属不可用，仅内网占位可查。",
                Ipv4FileName);
            return;
        }

        var ipv4Path = Path.Combine(regionDir, Ipv4FileName);
        var ipv6Path = Path.Combine(regionDir, Ipv6FileName);
        var ipv6 = File.Exists(ipv6Path) ? ipv6Path : null;
        TaktLocationHelper.Initialize(ipv4Path, ipv6);
        TaktLogger.Information(
            "[TaktLocationHelper] 已从 {RegionDir} 加载离线库（IPv6={HasIpv6}）",
            regionDir,
            ipv6 != null);
    }

    /// <summary>
    /// 解析含 ip2region_v4.xdb 的 Region 目录
    /// </summary>
    /// <param name="app">Web 应用</param>
    /// <returns>目录绝对路径；未找到则为 null</returns>
    private static string? ResolveRegionDirectory(WebApplication app)
    {
        var candidates = new List<string>();
        if (!string.IsNullOrWhiteSpace(app.Environment.WebRootPath))
        {
            candidates.Add(Path.Combine(app.Environment.WebRootPath, "Region"));
        }

        if (!string.IsNullOrWhiteSpace(app.Environment.ContentRootPath))
        {
            candidates.Add(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Region"));
            candidates.Add(Path.Combine(app.Environment.ContentRootPath, "Region"));
        }

        var baseDir = AppContext.BaseDirectory;
        if (!string.IsNullOrWhiteSpace(baseDir))
        {
            candidates.Add(Path.Combine(baseDir, "wwwroot", "Region"));
            candidates.Add(Path.Combine(baseDir, "Region"));
        }

        foreach (var dir in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var ipv4Path = Path.Combine(dir, Ipv4FileName);
            if (File.Exists(ipv4Path))
            {
                return dir;
            }
        }

        return null;
    }
}
