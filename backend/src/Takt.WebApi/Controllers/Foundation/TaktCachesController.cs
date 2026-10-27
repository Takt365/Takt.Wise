// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktCachesController.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存部门管控器，提供配置、统计与键操作 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 缓存部门管控器
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "缓存管理")]
public class TaktCachesController : TaktControllerBase
{
    private readonly ITaktCachesService _cachesService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cachesService">缓存管理服务</param>
    public TaktCachesController(ITaktCachesService cachesService)
    {
        _cachesService = cachesService;
    }

    /// <summary>
    /// 获取缓存配置信息
    /// </summary>
    /// <returns>缓存配置 DTO</returns>
    [TaktPermission("foundation:cache:list", "缓存配置")]
    [HttpGet("info")]
    public async Task<IActionResult> GetCacheInfoAsync()
    {
        try
        {
            var result = await _cachesService.GetCacheInfoAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取缓存统计信息
    /// </summary>
    /// <returns>缓存统计 DTO</returns>
    [TaktPermission("foundation:cache:list", "缓存统计")]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetCacheStatisticsAsync()
    {
        try
        {
            var result = await _cachesService.GetCacheStatisticsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 检查缓存键是否存在
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>存在性 DTO</returns>
    [TaktPermission("foundation:cache:list", "检查缓存键")]
    [HttpGet("exists")]
    public async Task<IActionResult> ExistsCacheKeyAsync([FromQuery] string key)
    {
        try
        {
            var result = await _cachesService.ExistsCacheKeyAsync(key);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 移除指定缓存键
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:cache:list", "删除缓存键")]
    [HttpDelete("key")]
    public async Task<IActionResult> RemoveCacheKeyAsync([FromQuery] string key)
    {
        try
        {
            await _cachesService.RemoveCacheKeyAsync(key);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
