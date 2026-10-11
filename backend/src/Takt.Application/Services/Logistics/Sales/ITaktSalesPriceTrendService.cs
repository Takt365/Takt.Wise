// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesPriceTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格推移分析服务（读销售价格本表；与 CRUD 服务分离）
/// </summary>
public interface ITaktSalesPriceTrendService
{
    /// <summary>
    /// 推移查询栏：销售价格本表工厂去重选项
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesPriceTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesPriceTrendPriceTypeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重客户（级联第 3 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesPriceTrendCustomerOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重物料（客户可选，传入则再过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="customerCode">客户编码（可选）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesPriceTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null, string? customerCode = null);

    /// <summary>
    /// 销售价格推移转置分析（工厂×物料×客户×ValidFrom）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktSalesPriceTrendResultDto> GetSalesPriceTrendAnalysisAsync(
        TaktSalesPriceTrendQueryDto queryDto);

    /// <summary>
    /// 导出销售价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesPriceTrendAnalysisAsync(
        TaktSalesPriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
