// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：ITaktCustomerServiceStatService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：客户服务看板统计服务接口（与 Request/Order/Ticket/Contract CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.CustomerService;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 客户服务看板统计服务（读请求/订单/工单/合同；与各 CRUD 服务分离）
/// </summary>
public interface ITaktCustomerServiceStatService
{
    /// <summary>
    /// 服务请求统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>请求统计</returns>
    Task<TaktServiceRequestStatDto> GetServiceRequestStatAsync(TaktServiceRequestStatQueryDto queryDto);

    /// <summary>
    /// 服务订单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>订单统计</returns>
    Task<TaktServiceOrderStatDto> GetServiceOrderStatAsync(TaktServiceOrderStatQueryDto queryDto);

    /// <summary>
    /// 服务工单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>工单统计</returns>
    Task<TaktServiceTicketStatDto> GetServiceTicketStatAsync(TaktServiceTicketStatQueryDto queryDto);

    /// <summary>
    /// 服务合同统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>合同统计</returns>
    Task<TaktServiceContractStatDto> GetServiceContractStatAsync(TaktServiceContractStatQueryDto queryDto);
}
