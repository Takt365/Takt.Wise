// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：ITaktDefectMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移转置分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 月生产不良推移转置分析服务
/// </summary>
public interface ITaktDefectMonthlyTrendService
{
    /// <summary>
    /// 推移查询栏：组立不良 ∪ PCBA 检查工厂去重选项
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="statTab">页签 assy 或 pcba；空则两类工厂都返回</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDefectMonthlyTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null, string? statTab = null);

    /// <summary>
    /// 推移查询栏：按工厂可用不良类别（assy / pcba；级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDefectMonthlyTrendDefectCategoryOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂（及可选不良类别）去重机种（级联第 3 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="defectCategory">不良类别（assy / pcba；空则两表并集）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetDefectMonthlyTrendModelOptionsAsync(string? plantCode = null, string? keyword = null, string? defectCategory = null);

    /// <summary>
    /// 获取月生产不良推移转置分析（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    Task<TaktDefectMonthlyTrendResultDto> GetDefectMonthlyTrendAnalysisAsync(
        TaktDefectMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出月生产不良推移转置分析 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">导出文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportDefectMonthlyTrendAnalysisAsync(
        TaktDefectMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
