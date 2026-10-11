// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.CustomerService
// 文件名称：TaktCustomerServiceStatsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：客户服务看板统计控制器（与 Request/Order/Ticket/Contract CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Application.Services.Logistics.CustomerService;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.CustomerService;

/// <summary>
/// 客户服务看板统计控制器（与各 CRUD 控制器分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客户服务统计")]
public class TaktCustomerServiceStatsController : TaktControllerBase
{
    private readonly ITaktCustomerServiceStatService _customerServiceStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceStatService">客户服务统计服务</param>
    public TaktCustomerServiceStatsController(ITaktCustomerServiceStatService customerServiceStatService)
    {
        _customerServiceStatService = customerServiceStatService;
    }

    /// <summary>
    /// 服务请求统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>请求统计</returns>
    [TaktPermission("logistics:customer:service:request:query", "服务请求统计")]
    [HttpGet("request-stat")]
    public async Task<IActionResult> GetServiceRequestStatAsync([FromQuery] TaktServiceRequestStatQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceStatService.GetServiceRequestStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 服务订单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>订单统计</returns>
    [TaktPermission("logistics:customer:service:order:query", "服务订单统计")]
    [HttpGet("order-stat")]
    public async Task<IActionResult> GetServiceOrderStatAsync([FromQuery] TaktServiceOrderStatQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceStatService.GetServiceOrderStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 服务工单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>工单统计</returns>
    [TaktPermission("logistics:customer:service:ticket:query", "服务工单统计")]
    [HttpGet("ticket-stat")]
    public async Task<IActionResult> GetServiceTicketStatAsync([FromQuery] TaktServiceTicketStatQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceStatService.GetServiceTicketStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 服务合同统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>合同统计</returns>
    [TaktPermission("logistics:customer:service:contract:query", "服务合同统计")]
    [HttpGet("contract-stat")]
    public async Task<IActionResult> GetServiceContractStatAsync([FromQuery] TaktServiceContractStatQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceStatService.GetServiceContractStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
