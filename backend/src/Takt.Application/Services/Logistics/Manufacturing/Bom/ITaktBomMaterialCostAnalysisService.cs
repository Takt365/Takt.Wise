// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostAnalysisService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析服务接口（转置 / 差异 / 月度涨跌）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析服务（转置 / 差异；月度涨跌见 ITaktBomMaterialCostAnalysisTrendService）
/// </summary>
public interface ITaktBomMaterialCostAnalysisService
{
    /// <summary>
    /// 获取成本分析转置列表（产品 × 核算月成本矩阵 + 环比涨跌）
    /// </summary>
    /// <param name="queryDto">转置查询 DTO</param>
    /// <returns>分页转置行、期间列、可选机种汇总与合计</returns>
    Task<TaktBomMaterialCostAnalysisTransposedResultDto> GetBomMaterialCostAnalysisTransposedListAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto);

    /// <summary>
    /// 导出成本分析转置 Excel（筛选命中的全部行，不截断）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisTransposedAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 获取成本分析差异（单产品两核算月组件级对比）
    /// </summary>
    /// <param name="queryDto">差异查询 DTO</param>
    /// <returns>汇总与组件差异明细</returns>
    Task<TaktBomMaterialCostAnalysisVarianceResultDto> GetBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto queryDto);

    /// <summary>
    /// 导出成本分析差异 Excel（汇总 + 明细双表）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">明细工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 加载成本分析汇总头（全量，非分页；供月度涨跌独立服务复用）
    /// </summary>
    /// <param name="queryDto">与转置相同筛选语义</param>
    /// <returns>成本汇总行</returns>
    Task<List<TaktBomMaterialCost>> LoadBomMaterialCostAnalysisHeadersAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto);
}
