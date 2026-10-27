// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变统计：各执行部门按设变单号汇总行数与实施情况。制技按是否更新 SOP
/// </summary>
public interface ITaktEcStatService
{
    /// <summary>
    /// 当前部门页签的工厂
    /// </summary>
    /// <param name="statTab">部门页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetEcStatPlantOptionsAsync(string? statTab, string? keyword = null);

    /// <summary>
    /// 当前部门页签的设变单号
    /// </summary>
    /// <param name="statTab">部门页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetEcStatEcOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 设变统计列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    Task<TaktEcDeptReportResultDto> GetEcStatListAsync(TaktEcDeptReportQueryDto queryDto);

    /// <summary>
    /// 导出设变统计
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="fileName">文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcStatAsync(TaktEcDeptReportQueryDto query, string? sheetName = null, string? fileName = null);
}
