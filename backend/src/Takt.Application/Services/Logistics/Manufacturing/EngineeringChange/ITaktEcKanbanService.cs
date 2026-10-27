// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcKanbanService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板应用服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变看板应用服务接口
/// </summary>
public interface ITaktEcKanbanService
{
    /// <summary>
    /// 获取设变看板列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcKanbanDto>> GetEcKanbanListAsync(TaktEcKanbanQueryDto queryDto);

    /// <summary>
    /// 根据设变主表 ID 获取看板行
    /// </summary>
    /// <param name="ecTeId">设变主表 ID</param>
    /// <returns>看板 DTO</returns>
    Task<TaktEcKanbanDto?> GetEcKanbanByEcIdAsync(long ecTeId);

    /// <summary>
    /// 导出设变看板
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcKanbanAsync(
        TaktEcKanbanQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取设变看板统计（实施状态饼图、部门柱图、按月折线）
    /// </summary>
    /// <param name="queryDto">查询条件（录入期间等）</param>
    /// <returns>看板统计</returns>
    Task<TaktEcKanbanBoardStatDto> GetEcKanbanBoardStatAsync(TaktEcKanbanQueryDto queryDto);
}
