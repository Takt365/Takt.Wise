// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktOnlineService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 在线用户应用服务接口
/// </summary>
public interface ITaktOnlineService
{
    /// <summary>
    /// 获取在线用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktOnlineDto>> GetOnlineListAsync(TaktOnlineQueryDto queryDto);

    /// <summary>
    /// 根据ID获取在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>DTO</returns>
    Task<TaktOnlineDto?> GetOnlineByIdAsync(long id);

    /// <summary>
    /// 获取在线用户选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetOnlineOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 注册 SignalR 在线会话（租户+公司+UserId 唯一一行：存在则更新，不存在则插入）
    /// </summary>
    /// <param name="dto">连接信息</param>
    /// <returns>在线用户 DTO</returns>
    Task<TaktOnlineDto> RegisterOnlineSessionAsync(TaktOnlineCreateDto dto);

    /// <summary>
    /// SignalR Heartbeat 累计 ConnectionDuration（写入逻辑见 TaktOnlineService 私有方法）
    /// </summary>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <param name="activeAt">活跃时刻</param>
    /// <returns>是否成功累计</returns>
    Task<bool> RefreshOnlineConnectionDurationAsync(string connectionId, DateTime activeAt);

    /// <summary>
    /// 按 ConnectionId 关闭在线会话（仅写 DisconnectTime/OnlineStatus，保留已累计 ConnectionDuration）
    /// </summary>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <param name="disconnectTime">断开时间</param>
    /// <param name="onlineStatus">离线状态（默认 1=离线；强退可传 2=离开）</param>
    /// <returns>是否更新到记录</returns>
    Task<bool> CloseOnlineSessionByConnectionIdAsync(
        string connectionId,
        DateTime disconnectTime,
        int onlineStatus = 1);

    /// <summary>
    /// 按用户 ID 关闭当前租户+公司下所有在线会话（HTTP 登出时 SignalR 可能尚未断开）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="disconnectTime">断开/登出时刻</param>
    /// <param name="onlineStatus">离线状态（默认 1=离线）</param>
    /// <returns>关闭的会话数</returns>
    Task<int> CloseOnlineSessionsByUserIdAsync(
        long userId,
        DateTime disconnectTime,
        int onlineStatus = 1);

    /// <summary>
    /// 删除在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>任务</returns>
    Task DeleteOnlineByIdAsync(long id);

    /// <summary>
    /// 批量删除在线用户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteOnlineBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新在线用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktOnlineDto> UpdateOnlineStatusAsync(TaktOnlineStatusDto dto);

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportOnlineAsync(TaktOnlineQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取在线时长统计（唯一入口：当前/当天/本周日均/本月日均；可选 UserName，为空取当前登录用户）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>统计 DTO</returns>
    Task<TaktOnlineStatisticsDto> GetOnlineStatisticsAsync(TaktOnlineStatisticsQueryDto? queryDto = null);

    /// <summary>
    /// 获取在线看板统计（公司维度：在线人数、当日总访问量、当前会话）
    /// </summary>
    /// <returns>看板统计 DTO</returns>
    Task<TaktOnlineDashboardStatisticsDto> GetOnlineDashboardStatisticsAsync();

}