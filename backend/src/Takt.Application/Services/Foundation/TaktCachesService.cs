// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktCachesService.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存管理应用服务（配置读取、统计、键检查与删除）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Options;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Interfaces;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 缓存管理应用服务
/// </summary>
public class TaktCachesService : TaktServiceBase, ITaktCachesService
{
    private readonly ITaktCacheService _cacheService;
    private readonly TaktCacheOptions _cacheOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cacheService">统一缓存服务</param>
    /// <param name="cacheOptions">缓存配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCachesService(
        ITaktCacheService cacheService,
        IOptions<TaktCacheOptions> cacheOptions,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _cacheService = cacheService;
        _cacheOptions = cacheOptions.Value;
    }

    /// <summary>
    /// 获取缓存配置信息
    /// </summary>
    /// <returns>配置 DTO</returns>
    public Task<TaktCacheInfoDto> GetCacheInfoAsync()
    {
        var dto = new TaktCacheInfoDto
        {
            Provider = _cacheOptions.Provider,
            DefaultExpirationMinutes = _cacheOptions.DefaultExpirationMinutes,
            EnableSlidingExpiration = _cacheOptions.EnableSlidingExpiration,
            EnableMultiLevelCache = _cacheOptions.IsRedisProvider,
            RedisInstanceName = _cacheOptions.IsRedisProvider ? _cacheOptions.Redis.InstanceName : null,
        };
        return Task.FromResult(dto);
    }

    /// <summary>
    /// 获取缓存统计信息
    /// </summary>
    /// <returns>统计 DTO</returns>
    public Task<TaktCacheStatisticsDto> GetCacheStatisticsAsync()
    {
        var snapshot = _cacheService.GetMemoryStatistics();
        var dto = new TaktCacheStatisticsDto
        {
            Supported = snapshot.Supported,
            Message = _cacheOptions.IsRedisProvider
                ? "统计反映进程内 Memory 层；Redis 分布式键请使用 Redis 监控工具查看"
                : snapshot.Message,
            CurrentEntryCount = snapshot.CurrentEntryCount,
            TotalHits = snapshot.TotalHits,
            TotalMisses = snapshot.TotalMisses,
            HitRate = snapshot.HitRate,
            CurrentEstimatedSizeBytes = snapshot.CurrentEstimatedSizeBytes,
        };
        return Task.FromResult(dto);
    }

    /// <summary>
    /// 检查缓存键是否存在
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>存在性 DTO</returns>
    public async Task<TaktCacheKeyExistsDto> ExistsCacheKeyAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var exists = await _cacheService.ExistsAsync(key.Trim());
        return new TaktCacheKeyExistsDto { Exists = exists };
    }

    /// <summary>
    /// 移除指定缓存键
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>任务</returns>
    public async Task RemoveCacheKeyAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        await _cacheService.RemoveAsync(key.Trim());
    }
}
