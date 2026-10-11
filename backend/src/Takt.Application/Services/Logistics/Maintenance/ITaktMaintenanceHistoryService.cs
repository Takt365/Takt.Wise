// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：ITaktMaintenanceHistoryService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护履历应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 设备维护履历应用服务接口
/// </summary>
public interface ITaktMaintenanceHistoryService
{
    /// <summary>
    /// 获取设备维护履历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMaintenanceHistoryDto>> GetMaintenanceHistoryListAsync(TaktMaintenanceHistoryQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceHistoryDto?> GetMaintenanceHistoryByIdAsync(long id);

    /// <summary>
    /// 获取设备维护履历选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaintenanceHistoryOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建设备维护履历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceHistoryDto> CreateMaintenanceHistoryAsync(TaktMaintenanceHistoryCreateDto dto);

    /// <summary>
    /// 更新设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceHistoryDto> UpdateMaintenanceHistoryAsync(long id, TaktMaintenanceHistoryUpdateDto dto);

    /// <summary>
    /// 删除设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>任务</returns>
    Task DeleteMaintenanceHistoryByIdAsync(long id);

    /// <summary>
    /// 批量删除设备维护履历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMaintenanceHistoryBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设备维护履历状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceHistoryDto> UpdateMaintenanceHistoryStatusAsync(TaktMaintenanceHistoryStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMaintenanceHistoryTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设备维护履历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMaintenanceHistoryAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设备维护履历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaintenanceHistoryAsync(TaktMaintenanceHistoryQueryDto? query = null, string? sheetName = null, string? fileName = null);

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取维护履历统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>维护履历统计</returns>
    Task<TaktMaintenanceHistoryStatDto> GetMaintenanceHistoryStatAsync(TaktMaintenanceHistoryStatQueryDto queryDto);

}
