// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktInboundActualTrendAnalysisBuilder.cs
// 创建时间：2026-09-17
// 创建人：Takt365(Cursor AI)
// 功能描述：入库实绩推移转置分析构建器接口（列=过账年月；S 加/H 减；金额浮动=单价差×当月数量）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 入库实绩推移转置分析构建器（读物料凭证收货行；列=过账年月；借/贷 S 加 H 减；因单价造成的入库金额浮动=单价差×当月数量）
/// </summary>
public interface ITaktInboundActualTrendAnalysisBuilder
{
    /// <summary>
    /// 构建入库实绩推移转置分析全量结果（列=过账年月；金额浮动=单价差×当月数量）
    /// </summary>
    /// <param name="queryDto">查询条件（工厂、期间必填；供应商、物料可选；忽略 PriceType）</param>
    /// <returns>内存构建结果</returns>
    Task<TaktPurchasePriceTrendAnalysisBuilt> BuildAsync(TaktPurchasePriceTrendQueryDto queryDto);
}
