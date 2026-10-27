// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktProductionMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移转置分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 生产统计：组立/PCBA 按班组或按机种分别统计，选项取自各自产出表
/// </summary>
public interface ITaktProductionMonthlyTrendService
{
    /// <summary>
    /// 当前页签产出表的工厂去重
    /// </summary>
    /// <param name="statTab">assy-team / assy-model / pcba-team / pcba-model</param>
    /// <param name="keyword">关键字</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendPlantOptionsAsync(string? statTab, string? keyword = null);

    /// <summary>
    /// 当前页签产出表的班组去重（组立主表；PCBA 明细）
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendTeamOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 当前页签产出表的机种去重
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendModelOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 当前页签产出表的物料去重
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendMaterialOptionsAsync(string? statTab, string? plantCode, string? keyword = null);

    /// <summary>
    /// 生产统计明细（组立按日报，PCBA 按明细）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    Task<TaktProductionStatLineResultDto> GetProductionStatLineListAsync(TaktProductionMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出生产统计明细
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="fileName">文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportProductionStatLineAsync(
        TaktProductionMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取月生产推移转置分析（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    Task<TaktProductionMonthlyTrendResultDto> GetProductionMonthlyTrendAnalysisAsync(
        TaktProductionMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出月生产推移转置分析 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">导出文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportProductionMonthlyTrendAnalysisAsync(
        TaktProductionMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
