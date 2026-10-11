// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：ITaktManufacturingPlanningOrchestrator.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：制造计划全链路编排接口（MDS→MPS→MRP→APS→工单 / 采购计划→PR）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

namespace Takt.Application.Services.Logistics.Manufacturing.Mrp;

/// <summary>
/// 制造计划全链路编排：MPS/MRP 运算、发布、APS 下推、采购计划转 PR
/// </summary>
public interface ITaktManufacturingPlanningOrchestrator
{
    /// <summary>
    /// 从 MDS 生成或刷新 MPS 明细
    /// </summary>
    /// <param name="dto">MDS 下推参数</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> RunMpsFromMdsAsync(TaktMpsRunFromMdsDto dto);

    /// <summary>
    /// 从 MPS 执行完整 MRP 运算（BOM 展开 + 库存/在途抵扣）
    /// </summary>
    /// <param name="dto">MRP 运算参数</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> RunMrpFromMpsAsync(TaktMrpRunDto dto);

    /// <summary>
    /// 发布 MRP 运算结果（自制→计划订单+生产计划，外购→采购计划）
    /// </summary>
    /// <param name="materialRequirementsPlanningId">MRP 头表 ID</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> PublishMrpAsync(long materialRequirementsPlanningId);

    /// <summary>
    /// 计划订单释放到 APS 订单
    /// </summary>
    /// <param name="dto">计划订单 ID 列表</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> ReleasePlannedOrdersToApsAsync(TaktReleasePlannedOrdersToApsDto dto);

    /// <summary>
    /// APS 订单排程（无限产能按日期排序）
    /// </summary>
    /// <param name="dto">APS 排程参数</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> RunApsSchedulingAsync(TaktApsScheduleRunDto dto);

    /// <summary>
    /// APS 订单释放为生产工单
    /// </summary>
    /// <param name="dto">APS 订单 ID 列表</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> ReleaseApsToProductionOrdersAsync(TaktReleaseApsToProductionDto dto);

    /// <summary>
    /// 采购计划转采购申请
    /// </summary>
    /// <param name="purchasePlanId">采购计划 ID</param>
    /// <param name="dto">转 PR 选项</param>
    /// <returns>编排结果</returns>
    Task<TaktManufacturingPlanningFlowResultDto> ConvertPurchasePlanToPurchaseRequestAsync(long purchasePlanId, TaktConvertPurchasePlanToPrDto? dto = null);
}
