// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：ITaktMaintenanceNotificationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：维护通知单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 维护通知单应用服务接口
/// </summary>
public interface ITaktMaintenanceNotificationService
{
    /// <summary>
    /// 获取维护通知单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMaintenanceNotificationDto>> GetMaintenanceNotificationListAsync(TaktMaintenanceNotificationQueryDto queryDto);

    /// <summary>
    /// 根据ID获取维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceNotificationDto?> GetMaintenanceNotificationByIdAsync(long id);

    /// <summary>
    /// 获取维护通知单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaintenanceNotificationOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建维护通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceNotificationDto> CreateMaintenanceNotificationAsync(TaktMaintenanceNotificationCreateDto dto);

    /// <summary>
    /// 更新维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceNotificationDto> UpdateMaintenanceNotificationAsync(long id, TaktMaintenanceNotificationUpdateDto dto);

    /// <summary>
    /// 删除维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <returns>任务</returns>
    Task DeleteMaintenanceNotificationByIdAsync(long id);

    /// <summary>
    /// 批量删除维护通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMaintenanceNotificationBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新维护通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaintenanceNotificationDto> UpdateMaintenanceNotificationStatusAsync(TaktMaintenanceNotificationStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMaintenanceNotificationTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入维护通知单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMaintenanceNotificationAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出维护通知单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaintenanceNotificationAsync(TaktMaintenanceNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null);

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取维护通知单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>维护通知单统计</returns>
    Task<TaktMaintenanceNotificationStatDto> GetMaintenanceNotificationStatAsync(TaktMaintenanceNotificationStatQueryDto queryDto);

}
