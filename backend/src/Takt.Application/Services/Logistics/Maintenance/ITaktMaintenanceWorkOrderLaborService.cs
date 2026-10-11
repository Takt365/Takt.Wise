// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：ITaktMaintenanceWorkOrderLaborService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单报工应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 维护工单报工应用服务接口
/// </summary>
public interface ITaktMaintenanceWorkOrderLaborService
{
    /// <summary>
    /// 获取维护工单报工列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMaintenanceWorkOrderLaborDto>> GetMaintenanceWorkOrderLaborListAsync(TaktMaintenanceWorkOrderLaborQueryDto queryDto);

    /// <summary>
    /// 根据ID获取维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceWorkOrderLaborDto?> GetMaintenanceWorkOrderLaborByIdAsync(long id);

    /// <summary>
    /// 获取维护工单报工选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaintenanceWorkOrderLaborOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建维护工单报工
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceWorkOrderLaborDto> CreateMaintenanceWorkOrderLaborAsync(TaktMaintenanceWorkOrderLaborCreateDto dto);

    /// <summary>
    /// 更新维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceWorkOrderLaborDto> UpdateMaintenanceWorkOrderLaborAsync(long id, TaktMaintenanceWorkOrderLaborUpdateDto dto);

    /// <summary>
    /// 删除维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <returns>任务</returns>
    Task DeleteMaintenanceWorkOrderLaborByIdAsync(long id);

    /// <summary>
    /// 批量删除维护工单报工
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMaintenanceWorkOrderLaborBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新维护工单报工状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceWorkOrderLaborDto> UpdateMaintenanceWorkOrderLaborStatusAsync(TaktMaintenanceWorkOrderLaborStatusDto dto);

    /// <summary>
    /// 更新维护工单报工作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceWorkOrderLaborDto> UpdateMaintenanceWorkOrderLaborObsoleteAsync(TaktMaintenanceWorkOrderLaborObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMaintenanceWorkOrderLaborTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入维护工单报工
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMaintenanceWorkOrderLaborAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出维护工单报工
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaintenanceWorkOrderLaborAsync(TaktMaintenanceWorkOrderLaborQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
