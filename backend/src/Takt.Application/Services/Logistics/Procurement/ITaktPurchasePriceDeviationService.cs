// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchasePriceDeviationService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价偏离 分析服务接口（PO/MIRO）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价偏离（采购价格）分析服务
/// </summary>
public interface ITaktPurchasePriceDeviationService
{
    /// <summary>
    /// 查询栏工厂选项（采购订单本表 PlantCode 去重）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceDeviationPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 查询栏供应商选项（工厂 + 订单期间，订单本表去重）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <param name="periodDateStart">订单期间起（当月首日）</param>
    /// <param name="periodDateEnd">订单期间止（当月首日）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceDeviationSupplierOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null);

    /// <summary>
    /// 查询栏物料选项（工厂 + 订单期间 + 可选供应商；订单明细去重；远程搜索上限）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <param name="supplierCode">供应商（可选）</param>
    /// <param name="periodDateStart">订单期间起（当月首日）</param>
    /// <param name="periodDateEnd">订单期间止（当月首日）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceDeviationMaterialOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        string? supplierCode = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null);

    /// <summary>
    /// PO 价偏离 PCR 分析（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    Task<TaktPurchasePriceDeviationResultDto> GetPurchasePriceDeviationAnalysisAsync(TaktPurchasePriceDeviationQueryDto queryDto);

    /// <summary>
    /// 导出 PO 价偏离 PCR 清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceDeviationAnalysisAsync(
        TaktPurchasePriceDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 查询栏工厂选项（物料凭证行 PlantCode 去重）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMiroPricePcrDeviationPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 查询栏供应商选项（工厂 + 过账期间，凭证行去重）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <param name="periodDateStart">过账期间起（当月首日）</param>
    /// <param name="periodDateEnd">过账期间止（当月首日）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMiroPricePcrDeviationSupplierOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null);

    /// <summary>
    /// 查询栏物料选项（工厂 + 过账期间 + 可选供应商；凭证行去重）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <param name="supplierCode">供应商（可选）</param>
    /// <param name="periodDateStart">过账期间起（当月首日）</param>
    /// <param name="periodDateEnd">过账期间止（当月首日）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMiroPricePcrDeviationMaterialOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        string? supplierCode = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null);

    /// <summary>
    /// MIRO 价偏离 PCR 分析（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    Task<TaktMiroPricePcrDeviationResultDto> GetMiroPricePcrDeviationAnalysisAsync(TaktMiroPricePcrDeviationQueryDto queryDto);

    /// <summary>
    /// 导出 MIRO 价偏离 PCR 清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMiroPricePcrDeviationAnalysisAsync(
        TaktMiroPricePcrDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
