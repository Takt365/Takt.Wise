// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcBatchService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变投入批次应用服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变投入批次应用服务接口
/// </summary>
public interface ITaktEcBatchService
{
    /// <summary>
    /// 获取投入批次列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcBatchDto>> GetEcBatchListAsync(TaktEcBatchQueryDto queryDto);

    /// <summary>
    /// 根据设变明细 ID 获取投入批次行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>批次 DTO</returns>
    Task<TaktEcBatchDto?> GetEcBatchByEcDetailIdAsync(long ecDetailId);

    /// <summary>
    /// 更新投入批次
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>批次 DTO</returns>
    Task<TaktEcBatchDto> UpdateEcBatchAsync(long ecDetailId, TaktEcBatchUpdateDto dto);

    /// <summary>
    /// 导出投入批次
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcBatchAsync(
        TaktEcBatchQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null);
}
