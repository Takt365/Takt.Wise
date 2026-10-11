// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktCacheDtos.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存管理 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// 缓存配置信息 DTO
/// </summary>
public class TaktCacheInfoDto
{
    /// <summary>
    /// 缓存提供者（Memory / Redis）
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// 默认过期时间（分钟）
    /// </summary>
    public int DefaultExpirationMinutes { get; set; }

    /// <summary>
    /// 是否启用滑动过期
    /// </summary>
    public bool EnableSlidingExpiration { get; set; }

    /// <summary>
    /// 是否启用多级缓存（Memory + Redis）
    /// </summary>
    public bool EnableMultiLevelCache { get; set; }

    /// <summary>
    /// Redis 实例名前缀
    /// </summary>
    public string? RedisInstanceName { get; set; }
}

/// <summary>
/// 缓存统计信息 DTO
/// </summary>
public class TaktCacheStatisticsDto
{
    /// <summary>
    /// 是否支持统计
    /// </summary>
    public bool Supported { get; set; }

    /// <summary>
    /// 说明文案
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 当前条目数
    /// </summary>
    public long? CurrentEntryCount { get; set; }

    /// <summary>
    /// 总命中次数
    /// </summary>
    public long? TotalHits { get; set; }

    /// <summary>
    /// 总未命中次数
    /// </summary>
    public long? TotalMisses { get; set; }

    /// <summary>
    /// 命中率（0~1）
    /// </summary>
    public double? HitRate { get; set; }

    /// <summary>
    /// 估算占用字节数
    /// </summary>
    public long? CurrentEstimatedSizeBytes { get; set; }
}

/// <summary>
/// 缓存键存在性 DTO
/// </summary>
public class TaktCacheKeyExistsDto
{
    /// <summary>
    /// 键是否存在
    /// </summary>
    public bool Exists { get; set; }
}
