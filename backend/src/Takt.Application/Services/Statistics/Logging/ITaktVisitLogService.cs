// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：ITaktVisitLogService.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：用户日访问量应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Logging;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 用户日访问量应用服务接口
/// </summary>
public interface ITaktVisitLogService
{
    /// <summary>
    /// 获取用户日访问量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktVisitLogDto>> GetVisitLogListAsync(TaktVisitLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <returns>DTO</returns>
    Task<TaktVisitLogDto?> GetVisitLogByIdAsync(long id);

    /// <summary>
    /// 获取用户日访问量选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetVisitLogOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建用户日访问量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktVisitLogDto> CreateVisitLogAsync(TaktVisitLogCreateDto dto);

    /// <summary>
    /// 更新用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktVisitLogDto> UpdateVisitLogAsync(long id, TaktVisitLogUpdateDto dto);

    /// <summary>
    /// 删除用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <returns>任务</returns>
    Task DeleteVisitLogByIdAsync(long id);

    /// <summary>
    /// 批量删除用户日访问量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteVisitLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出用户日访问量
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportVisitLogAsync(TaktVisitLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
