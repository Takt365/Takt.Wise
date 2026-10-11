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
/// 月生产推移转置分析服务（读组立/PCBA 产出本表；与 CRUD 服务分离）
/// </summary>
public interface ITaktProductionMonthlyTrendService
{
    /// <summary>
    /// 推移查询栏：组立/PCBA 产出本表工厂去重选项（并集）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂返回有数据的产出类别（assy / pcba）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendOutputCategoryOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂（及可选产出类别）去重机种
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="outputCategory">产出类别（assy/pcba；空则并集）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductionMonthlyTrendModelOptionsAsync(string? plantCode = null, string? keyword = null, string? outputCategory = null);

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
