// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputProdActualQtyLimitHelper.cs
// 功能描述：组立日报明细生产实际累计不得超过工单数量（跨生产日期、同工单号汇总）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报工单生产实际上限校验（同工单号全量明细 ProdActualQty 汇总）
/// </summary>
internal static class TaktAssyOutputProdActualQtyLimitHelper
{
    /// <summary>
    /// 解析工单数量上限（优先生产工单主数据，否则取组立日报主表快照）
    /// </summary>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="master">组立日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>工单数量上限</returns>
    public static async Task<decimal> ResolveProdOrderQtyLimitAsync(
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        TaktAssyOutput master,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(productionOrderRepository);
        ArgumentNullException.ThrowIfNull(master);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var prodOrderCode = master.ProdOrderCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return master.ProdOrderQty;
        }
        var orders = await productionOrderRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.ProdOrderCode == prodOrderCode);
        if (!string.IsNullOrWhiteSpace(master.PlantCode))
        {
            var plant = master.PlantCode.Trim();
            orders = orders.Where(x => x.PlantCode == plant).ToList();
        }
        if (!string.IsNullOrWhiteSpace(master.ProdOrderType))
        {
            var orderType = master.ProdOrderType.Trim();
            orders = orders.Where(x => x.ProdOrderType == orderType).ToList();
        }
        if (!string.IsNullOrWhiteSpace(master.MaterialCode))
        {
            var materialCode = master.MaterialCode.Trim();
            orders = orders.Where(x => x.MaterialCode == materialCode).ToList();
        }
        if (orders.Count > 0)
        {
            return orders.Max(x => x.ProdOrderQty);
        }
        return master.ProdOrderQty;
    }

    /// <summary>
    /// 保存主表下全部明细前校验：同工单累计生产实际不得超过工单数量
    /// </summary>
    /// <param name="detailRepository">组立日报明细仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="master">组立日报主表</param>
    /// <param name="proposedMasterProdActualTotal">本次保存后主表下明细生产实际合计</param>
    /// <param name="replaceAssyOutputId">被替换明细所属主表 Id（更新/先删后插时传入）</param>
    /// <returns>任务</returns>
    /// <exception cref="TaktBusinessException">累计生产实际超过工单数量时抛出</exception>
    public static async Task EnsureProdActualQtyNotExceedForMasterAsync(
        ITaktCompanyRepository<TaktAssyOutputDetail> detailRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        string tenantCode,
        string companyCode,
        TaktAssyOutput master,
        decimal proposedMasterProdActualTotal,
        long replaceAssyOutputId)
    {
        ArgumentNullException.ThrowIfNull(detailRepository);
        ArgumentNullException.ThrowIfNull(productionOrderRepository);
        ArgumentNullException.ThrowIfNull(master);
        var prodOrderCode = master.ProdOrderCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return;
        }
        var limit = await ResolveProdOrderQtyLimitAsync(productionOrderRepository, master, tenantCode, companyCode);
        if (limit <= 0)
        {
            return;
        }
        var existingTotal = await SumProdActualQtyByProdOrderCodeAsync(detailRepository, tenantCode, companyCode, prodOrderCode);
        var replacedMasterTotal = replaceAssyOutputId > 0
            ? await detailRepository.SumAsync(
                x => x.ProdActualQty,
                x => x.TenantCode == tenantCode
                    && x.CompanyCode == companyCode
                    && x.AssyOutputId == replaceAssyOutputId)
            : 0m;
        var newTotal = existingTotal - replacedMasterTotal + proposedMasterProdActualTotal;
        ThrowIfExceedsLimit(prodOrderCode, newTotal, limit);
    }

    /// <summary>
    /// 保存单条明细前校验：同工单累计生产实际不得超过工单数量
    /// </summary>
    /// <param name="detailRepository">组立日报明细仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="master">组立日报主表</param>
    /// <param name="proposedDetailProdActualQty">本次保存的明细生产实际</param>
    /// <param name="replaceDetailId">被替换的明细 Id（更新时传入）</param>
    /// <returns>任务</returns>
    /// <exception cref="TaktBusinessException">累计生产实际超过工单数量时抛出</exception>
    public static async Task EnsureProdActualQtyNotExceedForDetailAsync(
        ITaktCompanyRepository<TaktAssyOutputDetail> detailRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        string tenantCode,
        string companyCode,
        TaktAssyOutput master,
        decimal proposedDetailProdActualQty,
        long? replaceDetailId = null)
    {
        ArgumentNullException.ThrowIfNull(detailRepository);
        ArgumentNullException.ThrowIfNull(productionOrderRepository);
        ArgumentNullException.ThrowIfNull(master);
        var prodOrderCode = master.ProdOrderCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return;
        }
        var limit = await ResolveProdOrderQtyLimitAsync(productionOrderRepository, master, tenantCode, companyCode);
        if (limit <= 0)
        {
            return;
        }
        var existingTotal = await SumProdActualQtyByProdOrderCodeAsync(detailRepository, tenantCode, companyCode, prodOrderCode);
        var replacedDetailQty = 0m;
        if (replaceDetailId is > 0)
        {
            var oldDetail = await detailRepository.GetByIdAsync(replaceDetailId.Value);
            if (oldDetail != null
                && oldDetail.TenantCode == tenantCode
                && oldDetail.CompanyCode == companyCode
                && string.Equals(oldDetail.ProdOrderCode?.Trim(), prodOrderCode, StringComparison.Ordinal))
            {
                replacedDetailQty = oldDetail.ProdActualQty;
            }
        }
        var newTotal = existingTotal - replacedDetailQty + proposedDetailProdActualQty;
        ThrowIfExceedsLimit(prodOrderCode, newTotal, limit);
    }

    /// <summary>
    /// 按生产时段合并更新 DTO 后，计算主表下明细生产实际合计
    /// </summary>
    /// <param name="existingDetails">当前主表已有明细</param>
    /// <param name="updateDtos">本次提交的明细更新项</param>
    /// <returns>合并后的生产实际合计</returns>
    public static decimal CalculateMasterProdActualTotalAfterDetailUpdates(
        IReadOnlyList<TaktAssyOutputDetail> existingDetails,
        IReadOnlyList<TaktAssyOutputDetailCreateDto> updateDtos)
    {
        ArgumentNullException.ThrowIfNull(existingDetails);
        ArgumentNullException.ThrowIfNull(updateDtos);
        if (existingDetails.Count == 0)
        {
            return 0m;
        }
        var updatesByPeriod = updateDtos
            .Where(d => !string.IsNullOrWhiteSpace(d.TimePeriod))
            .GroupBy(d => d.TimePeriod.Trim(), StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First().ProdActualQty, StringComparer.Ordinal);
        decimal total = 0m;
        foreach (var detail in existingDetails)
        {
            var periodKey = detail.TimePeriod?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(periodKey) && updatesByPeriod.TryGetValue(periodKey, out var qty))
            {
                total += qty;
            }
            else
            {
                total += detail.ProdActualQty;
            }
        }
        return total;
    }

    /// <summary>
    /// 按工单号汇总已持久化明细生产实际
    /// </summary>
    private static async Task<decimal> SumProdActualQtyByProdOrderCodeAsync(
        ITaktCompanyRepository<TaktAssyOutputDetail> detailRepository,
        string tenantCode,
        string companyCode,
        string prodOrderCode)
    {
        return await detailRepository.SumAsync(
            x => x.ProdActualQty,
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.ProdOrderCode == prodOrderCode);
    }

    /// <summary>
    /// 累计生产实际超过工单数量时抛出业务异常
    /// </summary>
    private static void ThrowIfExceedsLimit(string prodOrderCode, decimal newTotal, decimal limit)
    {
        if (newTotal > limit)
        {
            throw new TaktBusinessException(
                $"工单 {prodOrderCode} 累计生产实际 {FormatQty(newTotal)} 已超过工单数量 {FormatQty(limit)}，无法保存");
        }
    }

    /// <summary>
    /// 格式化数量展示（去除多余小数位）
    /// </summary>
    private static string FormatQty(decimal value) => value.ToString("0.###");
}
