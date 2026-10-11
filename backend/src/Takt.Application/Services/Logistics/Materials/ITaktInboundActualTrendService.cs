// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktInboundActualTrendService.cs
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：入库实绩推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 入库实绩推移分析服务（读物料凭证收货行；与物料凭证 CRUD 分离）
/// </summary>
public interface ITaktInboundActualTrendService
{
    /// <summary>
    /// 推移查询栏工厂选项：仅当前公司 RelatedPlant，且须存在于入库收货物料凭证行 PlantCode
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetInboundActualTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂去重供应商
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetInboundActualTrendSupplierOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂去重物料（供应商可选）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <param name="supplierCode">供应商编码（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetInboundActualTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? supplierCode = null);

    /// <summary>
    /// 入库实绩推移转置分析（工厂×物料；列=期间各月；金额浮动=单价差×当月数量）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktPurchasePriceTrendResultDto> GetInboundActualTrendAnalysisAsync(TaktPurchasePriceTrendQueryDto queryDto);

    /// <summary>
    /// 导出入库实绩推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportInboundActualTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
