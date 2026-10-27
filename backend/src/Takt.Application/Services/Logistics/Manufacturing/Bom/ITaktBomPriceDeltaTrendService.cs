// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomPriceDeltaTrendService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移服务接口（独立模块）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本差异推移（产品月成本 + 0价格组 + PriceDeltaTrend）
/// 查询栏工厂/机种/产品选项统一走 ITaktBomCostOptionService。
/// </summary>
public interface ITaktBomPriceDeltaTrendService
{
    /// <summary>
    /// 成本差异推移列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页结果</returns>
    Task<TaktBomPriceDeltaTrendResultDto> GetBomPriceDeltaTrendListAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto);

    /// <summary>
    /// 导出成本差异推移
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomPriceDeltaTrendAsync(
        TaktBomPriceDeltaTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
