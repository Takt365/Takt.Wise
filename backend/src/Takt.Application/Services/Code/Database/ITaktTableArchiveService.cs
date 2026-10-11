// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：ITaktTableArchiveService.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据表归档应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Database;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 数据表归档应用服务接口
/// </summary>
public interface ITaktTableArchiveService
{
    /// <summary>
    /// 获取数据表归档列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTableArchiveDto>> GetTableArchiveListAsync(TaktTableArchiveQueryDto queryDto);

    /// <summary>
    /// 根据ID获取数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <returns>DTO</returns>
    Task<TaktTableArchiveDto?> GetTableArchiveByIdAsync(long id);

    /// <summary>
    /// 获取数据表归档选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetTableArchiveOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建数据表归档
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTableArchiveDto> CreateTableArchiveAsync(TaktTableArchiveCreateDto dto);

    /// <summary>
    /// 更新数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTableArchiveDto> UpdateTableArchiveAsync(long id, TaktTableArchiveUpdateDto dto);

    /// <summary>
    /// 删除数据表归档
    /// </summary>
    /// <param name="id">数据表归档ID</param>
    /// <returns>任务</returns>
    Task DeleteTableArchiveByIdAsync(long id);

    /// <summary>
    /// 批量删除数据表归档
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTableArchiveBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新数据表归档状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTableArchiveDto> UpdateTableArchiveStatusAsync(TaktTableArchiveStatusDto dto);

    /// <summary>
    /// 更新数据表归档排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTableArchiveDto> UpdateTableArchiveSortAsync(TaktTableArchiveSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetTableArchiveTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入数据表归档
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportTableArchiveAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出数据表归档
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTableArchiveAsync(TaktTableArchiveQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 预览按年归档行数
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>预览结果</returns>
    Task<TaktTableArchivePreviewResultDto> PreviewTableArchiveAsync(TaktTableArchiveExecuteDto dto);

    /// <summary>
    /// 执行按年归档并写入审计日志
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>执行结果</returns>
    Task<TaktTableArchiveExecuteResultDto> ExecuteTableArchiveAsync(TaktTableArchiveExecuteDto dto);

    /// <summary>
    /// 立即归档：创建一次性 Quartz 任务（尽快触发）
    /// </summary>
    /// <param name="dto">归档请求</param>
    /// <returns>调度结果</returns>
    Task<TaktTableArchiveScheduleResultDto> RunTableArchiveNowAsync(TaktTableArchiveScheduleDto dto);

    /// <summary>
    /// 后台归档：创建一次性 Quartz 任务（按 ScheduledAt 触发）
    /// </summary>
    /// <param name="dto">归档请求（须含 ScheduledAt）</param>
    /// <returns>调度结果</returns>
    Task<TaktTableArchiveScheduleResultDto> ScheduleTableArchiveAsync(TaktTableArchiveScheduleDto dto);

    /// <summary>
    /// 预建年分表
    /// </summary>
    /// <param name="dto">建表请求</param>
    /// <returns>建表结果</returns>
    Task<TaktTableEnsureYearTablesResultDto> EnsureYearTablesAsync(TaktTableEnsureYearTablesDto dto);
}
