// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDerivedFieldsHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报主表派生字段计算（标准工时回填、标准产能计算）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报主表派生字段计算辅助
/// </summary>
internal static class TaktAssyOutputDerivedFieldsHelper
{
    private const int MaxStandardOperationTimeRows = 50;

    /// <summary>
    /// 按物料编码解析生产日期有效的标准工时列表
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工时仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <returns>标准工时列表</returns>
    public static async Task<List<TaktStandardOperationTime>> ResolveStandardOperationTimesByMaterialAsync(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        string tenantCode,
        string companyCode,
        string materialCode,
        string plantCode,
        DateTime prodDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var normalizedMaterial = materialCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedMaterial))
        {
            return [];
        }
        var normalizedPlant = string.IsNullOrWhiteSpace(plantCode) ? null : plantCode.Trim();
        var prodDateOnly = prodDate.Date;
        var list = await GetEffectiveStandardOperationTimesCoreAsync(
            standardOperationTimeRepository,
            tenantCode,
            companyCode,
            normalizedMaterial,
            normalizedPlant,
            prodDateOnly);
        if (list.Count == 0 && normalizedPlant != null)
        {
            list = await GetEffectiveStandardOperationTimesCoreAsync(
                standardOperationTimeRepository,
                tenantCode,
                companyCode,
                normalizedMaterial,
                null,
                prodDateOnly);
        }
        return list.Take(MaxStandardOperationTimeRows).ToList();
    }

    /// <summary>
    /// 按工作中心取最新有效版本后汇总标准工时
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工时仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="materialCode">物料编码（已 trim）</param>
    /// <param name="plantCode">工厂代码（可选，已 trim）</param>
    /// <param name="prodDateOnly">生产日期（仅日期部分）</param>
    /// <returns>标准工时列表</returns>
    private static async Task<List<TaktStandardOperationTime>> GetEffectiveStandardOperationTimesCoreAsync(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        string tenantCode,
        string companyCode,
        string materialCode,
        string? plantCode,
        DateTime prodDateOnly)
    {
        return await standardOperationTimeRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.MaterialCode == materialCode
                && x.ApprovalStatus == TaktProductionStatHelper.StandardOperationTimeApprovalCompleted
                && x.EffectiveDate <= prodDateOnly
                && (x.ExpiryDate == null || x.ExpiryDate >= prodDateOnly)
                && (string.IsNullOrWhiteSpace(plantCode) || x.PlantCode == plantCode),
            x => x.EffectiveDate,
            true);
    }

    /// <summary>
    /// 按工作中心取最新有效版本后汇总标准工时
    /// </summary>
    /// <param name="operationTimes">候选标准工时</param>
    /// <returns>标准工时(分钟)</returns>
    public static decimal CalculateStdMinutesFromOperationTimes(IReadOnlyList<TaktStandardOperationTime> operationTimes)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        var effectiveRows = TaktStandardOperationTimeEffectiveResolver.SelectLatestPerWorkCenter(operationTimes);
        return TaktStandardOperationTimeEffectiveResolver.CalculateStdMinutes(effectiveRows);
    }

    /// <summary>
    /// 按物料编码回填组立日报主表标准工时(分钟)
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工时仓储</param>
    /// <param name="entity">组立日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task ApplyStdMinutesAsync(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        TaktAssyOutput entity,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var operationTimes = await ResolveStandardOperationTimesByMaterialAsync(
            standardOperationTimeRepository,
            tenantCode,
            companyCode,
            entity.MaterialCode,
            entity.PlantCode,
            entity.ProdDate);
        entity.StdMinutes = CalculateStdMinutesFromOperationTimes(operationTimes);
    }

    /// <summary>
    /// 按生产日期解析人员标准生产稼动率（%）；未匹配到启用记录时返回 0
    /// </summary>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <returns>稼动率(%)</returns>
    public static async Task<decimal> ResolvePersonnelOperationRatePercentAsync(
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        DateTime prodDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(plantCode))
        {
            return 0;
        }
        var prodDateOnly = prodDate.Date;
        return await TaktStandardOperationRateResolver.ResolveEffectiveOperationRatePercentAsync(
            standardOperationRateRepository,
            tenantCode,
            companyCode,
            plantCode,
            prodDateOnly,
            TaktProductionStatHelper.AssyStandardOperationRateTypePersonnel);
    }

    /// <summary>
    /// 写入组立日报主表标准产能（小时产能）
    /// </summary>
    /// <param name="entity">组立日报主表</param>
    /// <param name="operationRatePercent">标准生产稼动率(%)</param>
    public static void ApplyStdCapacity(TaktAssyOutput entity, decimal operationRatePercent)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.StdCapacity = TaktProductionStatHelper.CalculateAssyStdCapacity(
            entity.DirectLabor,
            entity.StdMinutes,
            operationRatePercent);
    }

    /// <summary>
    /// 回填标准工时并计算标准产能
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工时仓储</param>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="entity">组立日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task ApplyDerivedFieldsAsync(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        TaktAssyOutput entity,
        string tenantCode,
        string companyCode)
    {
        await ApplyStdMinutesAsync(standardOperationTimeRepository, entity, tenantCode, companyCode);
        var operationRatePercent = await ResolvePersonnelOperationRatePercentAsync(
            standardOperationRateRepository,
            tenantCode,
            companyCode,
            entity.PlantCode,
            entity.ProdDate);
        ApplyStdCapacity(entity, operationRatePercent);
    }
}
