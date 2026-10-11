// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 产品成本推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 产品成本推移分析服务（读 BOM 成本本表；仅产品推移）。
/// 查询栏工厂 / 机种 / 物料选项：统一引用 ITaktBomMaterialCostAnalysisService，本服务不重复提供。
/// </summary>
public interface ITaktBomMaterialCostTrendService
{
    /// <summary>
    /// 产品成本推移：单个产品下明细组件×月材料成本并算环比
    /// </summary>
    /// <param name="queryDto">查询 DTO（PlantCode + ProductCode 必填；ModelCode 可选）</param>
    /// <returns>明细组件×月材料成本结果</returns>
    Task<TaktBomMaterialCostTrendComponentMovingPriceResultDto> GetBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostTrendComponentMovingPriceQueryDto queryDto);

    /// <summary>
    /// 导出产品成本推移（单个产品明细组件×月材料成本）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostTrendComponentMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
