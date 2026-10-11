// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderBackfillHelper.cs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单关联字段回填（PlantCode 等随工单解析）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Repositories;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 生产工单关联字段回填辅助
/// </summary>
internal static class TaktProductionOrderBackfillHelper
{
    /// <summary>
    /// 按工单号解析工厂代码
    /// </summary>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>工厂代码；未匹配时返回 null</returns>
    public static async Task<string?> ResolvePlantCodeAsync(
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        string tenantCode,
        string companyCode,
        string prodOrderCode)
    {
        ArgumentNullException.ThrowIfNull(productionOrderRepository);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return null;
        }
        var order = await productionOrderRepository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdOrderCode == prodOrderCode);
        return string.IsNullOrWhiteSpace(order?.PlantCode) ? null : order.PlantCode;
    }

    /// <summary>
    /// 将解析到的工厂代码写入目标实体
    /// </summary>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="setPlantCode">写入 PlantCode 的委托</param>
    /// <returns>任务</returns>
    public static async Task ApplyPlantCodeAsync(
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        string tenantCode,
        string companyCode,
        string prodOrderCode,
        Action<string> setPlantCode)
    {
        ArgumentNullException.ThrowIfNull(setPlantCode);
        var plantCode = await ResolvePlantCodeAsync(
            productionOrderRepository,
            tenantCode,
            companyCode,
            prodOrderCode);
        if (!string.IsNullOrWhiteSpace(plantCode))
        {
            setPlantCode(plantCode);
        }
    }
}
