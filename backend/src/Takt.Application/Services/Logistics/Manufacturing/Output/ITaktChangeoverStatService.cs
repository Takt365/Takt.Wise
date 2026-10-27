// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktChangeoverStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：切换统计服务
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 切换统计：按期间列出生产切换记录并导出
/// </summary>
public interface ITaktChangeoverStatService
{
    /// <summary>
    /// 工厂选项
    /// </summary>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetChangeoverStatPlantOptionsAsync(string? keyword = null);

    /// <summary>
    /// 班组选项
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetChangeoverStatTeamOptionsAsync(string? plantCode, string? keyword = null);

    /// <summary>
    /// 分页清单
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页结果</returns>
    Task<TaktChangeoverStatResultDto> GetChangeoverStatListAsync(TaktChangeoverStatQueryDto queryDto);

    /// <summary>
    /// 导出清单
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportChangeoverStatAsync(TaktChangeoverStatQueryDto query, string? sheetName = null, string? fileName = null);
}
