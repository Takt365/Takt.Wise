// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktStandardOperationRateResolver.cs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：按生产日期解析有效标准生产稼动率（与组立日报派生计算共用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 标准生产稼动率解析辅助（按工厂、生产日期、稼动率类型匹配启用记录）
/// </summary>
internal static class TaktStandardOperationRateResolver
{
    /// <summary>
    /// 按生产日期解析有效标准生产稼动率（%）；未匹配到启用记录时返回 0
    /// </summary>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="operationType">稼动率类型</param>
    /// <returns>稼动率（比例）</returns>
    public static async Task<decimal> ResolveEffectiveOperationRatePercentAsync(
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        DateTime prodDate,
        int operationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(plantCode))
        {
            return 0;
        }
        var prodDateOnly = prodDate.Date;
        var candidates = await standardOperationRateRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PlantCode == plantCode
            && x.OperationType == operationType
            && x.RateStatus == TaktProductionStatHelper.StandardOperationRateStatusEnabled
            && x.EffectiveDate <= prodDateOnly
            && (x.ExpiryDate == null || x.ExpiryDate >= prodDateOnly));
        if (candidates.Count == 0)
        {
            return 0;
        }
        return TaktProductionStatHelper.NormalizeStandardOperationRate(candidates
            .OrderByDescending(x => x.EffectiveDate)
            .First()
            .OperationRate);
    }
}
