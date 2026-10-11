// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：ITaktServerMonitorService.cs
// 创建时间：2026-05-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务器监控服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 服务器监控服务接口
/// </summary>
public interface ITaktServerMonitorService
{
    /// <summary>
    /// 获取服务器硬件信息
    /// </summary>
    /// <returns>服务器硬件信息 DTO</returns>
    Task<TaktServerHardwareDto> GetServerHardwareAsync();

    /// <summary>
    /// 获取应用运行状态
    /// </summary>
    /// <returns>应用运行状态 DTO</returns>
    Task<TaktAppStatusDto> GetAppStatusAsync();

    /// <summary>
    /// 刷新硬件信息缓存
    /// </summary>
    void RefreshHardwareCache();
}
