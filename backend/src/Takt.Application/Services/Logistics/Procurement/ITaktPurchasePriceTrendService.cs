// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchasePriceTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格推移分析服务（读采购价格本表；与 CRUD 服务分离）
/// </summary>
public interface ITaktPurchasePriceTrendService
{
    /// <summary>
    /// 推移查询栏工厂选项（级联第 1 级）：仅当前公司 RelatedPlant，且须存在于采购价格本表 PlantCode
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项（通常 0～1 项；DictValue=PlantCode）</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceTrendPriceTypeOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重供应商（级联第 3 级；优先与同厂供应商主数据交叉）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceTrendSupplierOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重物料（供应商可选，传入则再过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="supplierCode">供应商编码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null, string? supplierCode = null);

    /// <summary>
    /// 采购价格推移转置分析（工厂×物料×供应商×月份）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktPurchasePriceTrendResultDto> GetPurchasePriceTrendAnalysisAsync(TaktPurchasePriceTrendQueryDto queryDto);

    /// <summary>
    /// 导出采购价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
