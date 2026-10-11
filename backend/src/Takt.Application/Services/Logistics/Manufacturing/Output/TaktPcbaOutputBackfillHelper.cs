// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputBackfillHelper.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报主表按生产工单回填 PlantCode/物料/批次等字段
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Repositories;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报主表工单回填辅助
/// </summary>
internal static class TaktPcbaOutputBackfillHelper
{
    /// <summary>
    /// 按工单号回填 PCBA 日报主表（PlantCode、工单类别、机种、物料、批次、数量、序列号）
    /// </summary>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="entity">PCBA 日报主表</param>
    /// <returns>任务</returns>
    public static async Task ApplyMasterFromProductionOrderAsync(
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        string tenantCode,
        string companyCode,
        string prodOrderCode,
        TaktPcbaOutput entity)
    {
        ArgumentNullException.ThrowIfNull(productionOrderRepository);
        ArgumentNullException.ThrowIfNull(modelDestinationRepository);
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return;
        }
        var order = await productionOrderRepository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdOrderCode == prodOrderCode.Trim());
        if (order == null)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(order.PlantCode))
        {
            entity.PlantCode = order.PlantCode;
        }
        entity.ProdOrderType = order.ProdOrderType;
        if (!string.IsNullOrWhiteSpace(order.MaterialCode))
        {
            entity.MaterialCode = order.MaterialCode;
        }
        entity.ProdOrderQty = order.ProdOrderQty;
        entity.BatchCode = order.ProdBatch;
        entity.SerialCode = order.SerialCode;
        if (!string.IsNullOrWhiteSpace(entity.MaterialCode))
        {
            var model = await modelDestinationRepository.FirstAsync(x =>
                x.TenantCode == tenantCode
                && x.MaterialCode == entity.MaterialCode);
            if (!string.IsNullOrWhiteSpace(model?.ModelCode))
            {
                entity.ModelCode = model.ModelCode;
            }
        }
    }
}
