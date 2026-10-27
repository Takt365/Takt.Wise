// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：ITaktQualityCostReportService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质成本报告：保证按记录组、问题按机种、事故按废弃组
/// </summary>
public interface ITaktQualityCostReportService
{
    /// <summary>
    /// 当前页签工厂
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetQualityCostReportPlantOptionsAsync(string? statTab, string? keyword = null);

    /// <summary>
    /// 当前页签维度
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetQualityCostReportDimensionOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 当前页签币种
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    Task<List<TaktSelectOption>> GetQualityCostReportCurrencyOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>结果</returns>
    Task<TaktQualityCostReportResultDto> GetQualityCostReportListAsync(TaktQualityCostReportQueryDto queryDto);

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="fileName">文件名</param>
    /// <returns>文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportQualityCostReportAsync(TaktQualityCostReportQueryDto query, string? sheetName = null, string? fileName = null);
}
