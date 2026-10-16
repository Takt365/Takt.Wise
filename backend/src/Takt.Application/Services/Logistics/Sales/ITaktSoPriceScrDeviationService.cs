// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSoPriceScrDeviationService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO 价偏离 SCR 分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// SO 价偏离 SCR（销售条件）分析服务
/// </summary>
public interface ITaktSoPriceScrDeviationService
{
    /// <summary>
    /// 查询栏工厂选项（销售订单本表 PlantCode 去重）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSoPriceScrDeviationPlantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 查询栏客户选项（工厂 + 订单期间，订单本表去重）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <param name="periodDateStart">订单期间起（当月首日）</param>
    /// <param name="periodDateEnd">订单期间止（当月首日）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSoPriceScrDeviationCustomerOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null);

    /// <summary>
    /// 查询栏物料选项（工厂 + 订单期间 + 可选客户；订单明细去重；远程搜索上限）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">关键字（可选）</param>
    /// <param name="customerCode">客户（可选）</param>
    /// <param name="periodDateStart">订单期间起（当月首日）</param>
    /// <param name="periodDateEnd">订单期间止（当月首日）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSoPriceScrDeviationMaterialOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        string? customerCode = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null);

    /// <summary>
    /// SO 价偏离 SCR 分析（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    Task<TaktSoPriceScrDeviationResultDto> GetSoPriceScrDeviationAnalysisAsync(TaktSoPriceScrDeviationQueryDto queryDto);

    /// <summary>
    /// 导出 SO 价偏离 SCR 清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSoPriceScrDeviationAnalysisAsync(
        TaktSoPriceScrDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
