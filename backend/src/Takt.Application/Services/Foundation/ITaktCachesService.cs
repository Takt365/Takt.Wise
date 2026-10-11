// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktCachesService.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存管理应用服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 缓存管理应用服务
/// </summary>
public interface ITaktCachesService
{
    /// <summary>
    /// 获取缓存配置信息
    /// </summary>
    /// <returns>配置 DTO</returns>
    Task<TaktCacheInfoDto> GetCacheInfoAsync();

    /// <summary>
    /// 获取缓存统计信息
    /// </summary>
    /// <returns>统计 DTO</returns>
    Task<TaktCacheStatisticsDto> GetCacheStatisticsAsync();

    /// <summary>
    /// 检查缓存键是否存在
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>存在性 DTO</returns>
    Task<TaktCacheKeyExistsDto> ExistsCacheKeyAsync(string key);

    /// <summary>
    /// 移除指定缓存键
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>任务</returns>
    Task RemoveCacheKeyAsync(string key);
}
