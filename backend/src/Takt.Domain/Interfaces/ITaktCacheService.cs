// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktCacheService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一缓存服务接口（内存 / 分布式，读取 Cache 配置）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Foundation;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 统一缓存服务
/// </summary>
public interface ITaktCacheService
{
    /// <summary>
    /// 获取缓存项
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>缓存值；不存在时返回 default</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// 写入缓存项（使用配置中的默认过期策略）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="value">缓存值</param>
    /// <param name="absoluteExpiration">绝对过期时间；为空时使用配置默认分钟数</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// 移除缓存项
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取或创建缓存项
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="key">缓存键</param>
    /// <param name="factory">未命中时的工厂方法</param>
    /// <param name="absoluteExpiration">绝对过期时间；为空时使用配置默认分钟数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>缓存值</returns>
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// 检查缓存键是否存在于内存或分布式层
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存在返回 true</returns>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取进程内业务 MemoryCache 运行时统计
    /// </summary>
    /// <returns>统计快照</returns>
    TaktCacheMemoryStatisticsSnapshot GetMemoryStatistics();
}
