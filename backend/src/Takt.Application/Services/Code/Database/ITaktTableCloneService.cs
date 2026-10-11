// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：ITaktTableCloneService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表数据克隆应用服务接口（备份预览 + 执行克隆；库表元数据见 ITaktDatabaseInfoService）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 跨租户整表数据克隆应用服务
/// </summary>
public interface ITaktTableCloneService
{
    /// <summary>
    /// 单次请求允许克隆的最大表数量
    /// </summary>
    const int MaxTableCountPerRequest = 5;

    /// <summary>
    /// 获取跨租户整表克隆备份预览（备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求（源/目标租户、数据库、表清单）</param>
    /// <returns>各目标表备份与清空预览</returns>
    Task<TaktTableClonePreviewDto> GetTableClonePreviewAsync(TaktTableCloneDto dto);

    /// <summary>
    /// 将源表数据克隆到目标表（跨租户；一次 1~MaxTableCountPerRequest 张表；须先确认备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求（源/目标租户、数据库、表清单）</param>
    /// <returns>批量克隆结果</returns>
    Task<TaktTableCloneResultDto> CloneTableAsync(TaktTableCloneDto dto);
}
