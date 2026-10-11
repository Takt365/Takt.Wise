// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcNotificationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知单应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单应用服务接口
/// </summary>
public interface ITaktEcNotificationService
{
    /// <summary>
    /// 获取工程变更通知单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcNotificationDto>> GetEcNotificationListAsync(TaktEcNotificationQueryDto queryDto);

    /// <summary>
    /// 根据ID获取工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcNotificationDto?> GetEcNotificationByIdAsync(long id);

    /// <summary>
    /// 获取工程变更通知单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcNotificationOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建工程变更通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcNotificationDto> CreateEcNotificationAsync(TaktEcNotificationCreateDto dto);

    /// <summary>
    /// 更新工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcNotificationDto> UpdateEcNotificationAsync(long id, TaktEcNotificationUpdateDto dto);

    /// <summary>
    /// 删除工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>任务</returns>
    Task DeleteEcNotificationByIdAsync(long id);

    /// <summary>
    /// 批量删除工程变更通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcNotificationBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新工程变更通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcNotificationDto> UpdateEcNotificationStatusAsync(TaktEcNotificationStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcNotificationTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入工程变更通知单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcNotificationAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出工程变更通知单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcNotificationAsync(TaktEcNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
