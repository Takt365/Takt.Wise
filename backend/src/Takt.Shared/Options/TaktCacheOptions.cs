// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktCacheOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;

namespace Takt.Shared.Options;

/// <summary>
/// 缓存配置选项
/// </summary>
public class TaktCacheOptions
{
    public const string SectionName = "Cache";

    /// <summary>
    /// 缓存提供者：Memory（内存缓存）或 Redis（Redis缓存）
    /// </summary>
    public string Provider { get; set; } = "Memory";

    /// <summary>
    /// 默认过期时间（分钟）
    /// </summary>
    public int DefaultExpirationMinutes { get; set; } = 30;

    /// <summary>
    /// 是否启用滑动过期（每次访问时重置过期时间）
    /// </summary>
    public bool EnableSlidingExpiration { get; set; } = true;

    /// <summary>
    /// 内存缓存配置
    /// </summary>
    public TaktCacheMemoryOptions Memory { get; set; } = new();

    /// <summary>
    /// Redis缓存配置
    /// </summary>
    public TaktCacheRedisOptions Redis { get; set; } = new();

    /// <summary>
    /// 菜单缓存配置（树/全量列表自动缓存开关）
    /// </summary>
    public TaktMenuCacheOptions Menu { get; set; } = new();

    /// <summary>
    /// 是否为内存缓存提供者
    /// </summary>
    public bool IsMemoryProvider =>
        Provider.Equals("Memory", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// 是否为 Redis 缓存提供者
    /// </summary>
    public bool IsRedisProvider =>
        Provider.Equals("Redis", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// 解析 Redis 连接字符串（优先 Cache:Redis:ConnectionString，其次 ConnectionStrings:Redis）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>连接字符串</returns>
    public string ResolveRedisConnectionString(IConfiguration configuration)
    {
        if (!string.IsNullOrWhiteSpace(Redis.ConnectionString))
        {
            return Redis.ConnectionString.Trim();
        }

        return configuration.GetConnectionString("Redis")?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 验证配置（Provider 与 Redis.Enabled 必须成对匹配）
    /// </summary>
    /// <param name="configuration">应用配置（启用 Redis 时校验连接字符串）</param>
    public void Validate(IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(Provider))
        {
            throw new InvalidOperationException("Cache:Provider 不能为空");
        }

        if (!IsMemoryProvider && !IsRedisProvider)
        {
            throw new InvalidOperationException("Cache:Provider 必须是 Memory 或 Redis");
        }

        if (DefaultExpirationMinutes <= 0)
        {
            throw new InvalidOperationException("Cache:DefaultExpirationMinutes 必须大于 0");
        }

        Memory.Validate();

        if (IsMemoryProvider)
        {
            if (Redis.Enabled)
            {
                throw new InvalidOperationException(
                    "Cache 配置无效：Provider 为 Memory 时，Redis:Enabled 必须为 false");
            }

            return;
        }

        Redis.Validate(configuration, ResolveRedisConnectionString(configuration));

        if (!Redis.Enabled)
        {
            throw new InvalidOperationException(
                "Cache 配置无效：Provider 为 Redis 时，Redis:Enabled 必须为 true");
        }
    }
}

/// <summary>
/// 内存缓存配置选项
/// </summary>
public class TaktCacheMemoryOptions
{
    /// <summary>
    /// 缓存容量上限（抽象 Size 单位总和；须与写入项的 <c>MemoryCacheEntryOptions.Size</c> 配合）
    /// </summary>
    public int SizeLimit { get; set; } = 4096;

    /// <summary>
    /// 压缩百分比（0.0-1.0），映射到 MemoryCacheOptions.CompactionPercentage
    /// </summary>
    public double CompactionPercentage { get; set; } = 0.25;

    /// <summary>
    /// 过期扫描频率（秒），映射到 MemoryCacheOptions.ExpirationScanFrequency
    /// </summary>
    public int ExpirationScanFrequency { get; set; } = 60;

    /// <summary>
    /// 验证内存缓存配置（仅校验实际绑定到 MemoryCache 的项）
    /// </summary>
    public void Validate()
    {
        if (SizeLimit <= 0)
        {
            throw new InvalidOperationException("Cache:Memory:SizeLimit 必须大于 0");
        }

        if (CompactionPercentage < 0 || CompactionPercentage > 1)
        {
            throw new InvalidOperationException("Cache:Memory:CompactionPercentage 必须在 0 到 1 之间");
        }

        if (ExpirationScanFrequency <= 0)
        {
            throw new InvalidOperationException("Cache:Memory:ExpirationScanFrequency 必须大于 0");
        }
    }
}

/// <summary>
/// Redis缓存配置选项
/// </summary>
public class TaktCacheRedisOptions
{
    /// <summary>
    /// 是否启用 Redis（须与 Cache:Provider 匹配：Memory 对应 false，Redis 对应 true）
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Redis连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 实例名称前缀，用于区分不同应用的缓存键
    /// </summary>
    public string InstanceName { get; set; } = "Takt.Net";

    /// <summary>
    /// 默认数据库编码
    /// </summary>
    public int DefaultDatabase { get; set; }

    /// <summary>
    /// 连接超时时间（毫秒）
    /// </summary>
    public int ConnectTimeout { get; set; } = 5000;

    /// <summary>
    /// 同步超时时间（毫秒）
    /// </summary>
    public int SyncTimeout { get; set; } = 5000;

    /// <summary>
    /// 是否允许管理员操作
    /// </summary>
    public bool AllowAdmin { get; set; } = true;

    /// <summary>
    /// 是否启用SSL
    /// </summary>
    public bool Ssl { get; set; }

    /// <summary>
    /// Redis密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用压缩
    /// </summary>
    public bool EnableCompression { get; set; } = true;

    /// <summary>
    /// 压缩阈值（字节），超过此大小的值才会压缩
    /// </summary>
    public int CompressionThreshold { get; set; } = 1024;

    /// <summary>
    /// 验证 Redis 缓存配置（Provider 为 Redis 时调用）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="resolvedConnectionString">已解析的连接字符串</param>
    public void Validate(IConfiguration configuration, string resolvedConnectionString)
    {
        if (string.IsNullOrWhiteSpace(InstanceName))
        {
            throw new InvalidOperationException("Cache:Redis:InstanceName 不能为空");
        }

        if (ConnectTimeout <= 0)
        {
            throw new InvalidOperationException("Cache:Redis:ConnectTimeout 必须大于 0");
        }

        if (SyncTimeout <= 0)
        {
            throw new InvalidOperationException("Cache:Redis:SyncTimeout 必须大于 0");
        }

        if (CompressionThreshold <= 0)
        {
            throw new InvalidOperationException("Cache:Redis:CompressionThreshold 必须大于 0");
        }

        if (string.IsNullOrWhiteSpace(resolvedConnectionString))
        {
            throw new InvalidOperationException(
                "Cache 配置无效：Provider 为 Redis 时，Redis:ConnectionString 不可为空（或配置 ConnectionStrings:Redis）");
        }
    }
}

/// <summary>
/// 菜单缓存配置（与菜单实体 <c>IsCached</c> 配合：全局开关 + 单条是否缓存）
/// </summary>
public class TaktMenuCacheOptions
{
    /// <summary>
    /// 是否启用租户菜单全量/树形读取自动缓存（写操作按租户失效）
    /// </summary>
    public bool Enabled { get; set; } = true;
}
