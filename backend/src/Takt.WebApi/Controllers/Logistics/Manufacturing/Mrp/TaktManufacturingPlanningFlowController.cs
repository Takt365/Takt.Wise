// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp
// 文件名称：TaktManufacturingPlanningFlowController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：制造计划全链路流程 API（MDS→MPS→MRP→APS→工单 / 采购计划→PR）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Application.Services.Logistics.Manufacturing.Mrp;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp;

/// <summary>
/// 制造计划全链路流程控制器（非标准 CRUD，编排下推专用）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "制造计划全链路")]
public class TaktManufacturingPlanningFlowController : TaktControllerBase
{
    private readonly ITaktManufacturingPlanningOrchestrator _orchestrator;

    /// <summary>
    /// 初始化制造计划流程控制器
    /// </summary>
    /// <param name="orchestrator">制造计划编排服务</param>
    public TaktManufacturingPlanningFlowController(ITaktManufacturingPlanningOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    /// <summary>
    /// 从 MDS 生成或刷新 MPS
    /// </summary>
    /// <param name="dto">下推参数</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:generate", "从MDS生成MPS")]
    [HttpPost("mps/run-from-mds")]
    public async Task<IActionResult> RunMpsFromMdsAsync([FromBody] TaktMpsRunFromMdsDto dto)
    {
        try
        {
            var result = await _orchestrator.RunMpsFromMdsAsync(dto);
            return Success(result, "MPS 生成成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 执行 MRP 运算
    /// </summary>
    /// <param name="dto">运算参数</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:run", "MRP运算")]
    [HttpPost("mrp/run")]
    public async Task<IActionResult> RunMrpAsync([FromBody] TaktMrpRunDto dto)
    {
        try
        {
            var result = await _orchestrator.RunMrpFromMpsAsync(dto);
            return Success(result, "MRP 运算成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 发布 MRP 运算结果
    /// </summary>
    /// <param name="id">MRP 头表 ID</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:publish", "发布MRP")]
    [HttpPost("mrp/{id}/publish")]
    public async Task<IActionResult> PublishMrpAsync(long id)
    {
        try
        {
            var result = await _orchestrator.PublishMrpAsync(id);
            return Success(result, "MRP 发布成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 计划订单释放到 APS
    /// </summary>
    /// <param name="dto">计划订单 ID 列表</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:release", "释放计划订单到APS")]
    [HttpPost("planned-orders/release-to-aps")]
    public async Task<IActionResult> ReleasePlannedOrdersToApsAsync([FromBody] TaktReleasePlannedOrdersToApsDto dto)
    {
        try
        {
            var result = await _orchestrator.ReleasePlannedOrdersToApsAsync(dto);
            return Success(result, "释放到 APS 成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// APS 排程
    /// </summary>
    /// <param name="dto">排程参数</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:schedule", "APS排程")]
    [HttpPost("aps/schedule")]
    public async Task<IActionResult> RunApsSchedulingAsync([FromBody] TaktApsScheduleRunDto dto)
    {
        try
        {
            var result = await _orchestrator.RunApsSchedulingAsync(dto);
            return Success(result, "APS 排程成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// APS 释放为生产工单
    /// </summary>
    /// <param name="dto">APS 订单 ID 列表</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:publish", "APS发布生产工单")]
    [HttpPost("aps/release-to-production")]
    public async Task<IActionResult> ReleaseApsToProductionOrdersAsync([FromBody] TaktReleaseApsToProductionDto dto)
    {
        try
        {
            var result = await _orchestrator.ReleaseApsToProductionOrdersAsync(dto);
            return Success(result, "生产工单发布成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 采购计划转采购申请
    /// </summary>
    /// <param name="id">采购计划 ID</param>
    /// <param name="dto">转 PR 选项</param>
    /// <returns>编排结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:convertto", "采购计划转采购申请")]
    [HttpPost("purchase-plans/{id}/convert-to-pr")]
    public async Task<IActionResult> ConvertPurchasePlanToPurchaseRequestAsync(long id, [FromBody] TaktConvertPurchasePlanToPrDto? dto)
    {
        try
        {
            var result = await _orchestrator.ConvertPurchasePlanToPurchaseRequestAsync(id, dto);
            return Success(result, "转采购申请成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
