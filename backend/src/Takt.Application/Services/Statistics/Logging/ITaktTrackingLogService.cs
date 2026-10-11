// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：ITaktTrackingLogService.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：交互日志应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Logging;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 交互日志应用服务接口
/// </summary>
public interface ITaktTrackingLogService
{
    /// <summary>
    /// 获取交互日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTrackingLogDto>> GetTrackingLogListAsync(TaktTrackingLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>DTO</returns>
    Task<TaktTrackingLogDto?> GetTrackingLogByIdAsync(long id);

    /// <summary>
    /// 获取交互日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetTrackingLogOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建交互日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrackingLogDto> CreateTrackingLogAsync(TaktTrackingLogCreateDto dto);

    /// <summary>
    /// 更新交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrackingLogDto> UpdateTrackingLogAsync(long id, TaktTrackingLogUpdateDto dto);

    /// <summary>
    /// 删除交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>任务</returns>
    Task DeleteTrackingLogByIdAsync(long id);

    /// <summary>
    /// 批量删除交互日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTrackingLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出交互日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTrackingLogAsync(TaktTrackingLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 批量上报 Long Task 等客户端性能事件（登录用户自动写入租户/公司/用户信息）
    /// </summary>
    /// <param name="dto">批量上报 DTO</param>
    /// <param name="clientIp">客户端 IP</param>
    /// <returns>成功写入条数</returns>
    Task<int> TrackTrackingLogBatchAsync(TaktTrackingLogBatchTrackDto dto, string? clientIp);
}
