// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：ITaktQuartzLogService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：任务执行日志应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Logging;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 任务执行日志应用服务接口
/// </summary>
public interface ITaktQuartzLogService
{
    /// <summary>
    /// 获取任务执行日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktQuartzLogDto>> GetQuartzLogListAsync(TaktQuartzLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>DTO</returns>
    Task<TaktQuartzLogDto?> GetQuartzLogByIdAsync(long id);

    /// <summary>
    /// 获取任务执行日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetQuartzLogOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建任务执行日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQuartzLogDto> CreateQuartzLogAsync(TaktQuartzLogCreateDto dto);

    /// <summary>
    /// 更新任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQuartzLogDto> UpdateQuartzLogAsync(long id, TaktQuartzLogUpdateDto dto);

    /// <summary>
    /// 删除任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>任务</returns>
    Task DeleteQuartzLogByIdAsync(long id);

    /// <summary>
    /// 批量删除任务执行日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteQuartzLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新任务执行日志状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQuartzLogDto> UpdateQuartzLogStatusAsync(TaktQuartzLogStatusDto dto);

    /// <summary>
    /// 导出任务执行日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportQuartzLogAsync(TaktQuartzLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
