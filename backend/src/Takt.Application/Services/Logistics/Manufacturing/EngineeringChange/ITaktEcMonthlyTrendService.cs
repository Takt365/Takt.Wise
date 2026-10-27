// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：设变推移转置分析服务接口（设变号×部门；实施推移按部门）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变推移转置分析服务
/// </summary>
public interface ITaktEcMonthlyTrendService
{
    /// <summary>
    /// 推移查询栏：工厂去重选项（设变主表 PlantCode；执行任务无工厂列）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcMonthlyTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂去重部门（级联第 2 级；来自执行任务）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcMonthlyTrendDeptOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂+部门去重设变单号（级联第 3 级；部门可空；来自执行任务）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="deptCode">部门编码（可空）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcMonthlyTrendEcCodeOptionsAsync(string? plantCode = null, string? keyword = null, string? deptCode = null);

    /// <summary>
    /// 获取设变推移转置分析（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    Task<TaktEcMonthlyTrendResultDto> GetEcMonthlyTrendAnalysisAsync(
        TaktEcMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出设变推移转置分析 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">导出文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcMonthlyTrendAnalysisAsync(
        TaktEcMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取月实施推移转置分析（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    Task<TaktEcImplementationMonthlyTrendResultDto> GetEcImplementationMonthlyTrendAnalysisAsync(
        TaktEcImplementationMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出月实施推移转置分析 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">导出文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcImplementationMonthlyTrendAnalysisAsync(
        TaktEcImplementationMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
