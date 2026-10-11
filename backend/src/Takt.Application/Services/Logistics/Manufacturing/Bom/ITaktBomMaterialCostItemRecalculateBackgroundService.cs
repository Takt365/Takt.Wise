// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostItemRecalculateBackgroundService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种月平均重算后台任务调度接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本机种月平均重算后台任务调度接口
/// </summary>
public interface ITaktBomMaterialCostItemRecalculateBackgroundService
{
    /// <summary>
    /// 提交后台重算任务（立即返回；完成后通过 SignalR 通知触发用户）
    /// </summary>
    /// <param name="queryDto">已校验的计算查询（含 ProcessRecordCount）</param>
    /// <param name="forceRecalculate">true=重算（归档旧成本到 ExtField 后重写）；false=计算成本</param>
    /// <returns>任务</returns>
    Task EnqueueRecalculateAsync(
        TaktBomCalculateQueryDto queryDto,
        bool forceRecalculate = false);
}
