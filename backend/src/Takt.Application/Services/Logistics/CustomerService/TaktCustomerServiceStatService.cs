// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceStatService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：客户服务看板统计服务（与 Request/Order/Ticket/Contract CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Domain.Entities.Logistics.CustomerService;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 客户服务看板统计服务（读四张业务表；与各 CRUD 服务分离）
/// </summary>
public class TaktCustomerServiceStatService : TaktServiceBase, ITaktCustomerServiceStatService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceRequest> _customerServiceRequestRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceOrder> _customerServiceOrderRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceTicket> _customerServiceTicketRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceContract> _customerServiceContractRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceRequestRepository">服务请求仓储</param>
    /// <param name="customerServiceOrderRepository">服务订单仓储</param>
    /// <param name="customerServiceTicketRepository">服务工单仓储</param>
    /// <param name="customerServiceContractRepository">服务合同仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceStatService(
        ITaktCompanyRepository<TaktCustomerServiceRequest> customerServiceRequestRepository,
        ITaktCompanyRepository<TaktCustomerServiceOrder> customerServiceOrderRepository,
        ITaktCompanyRepository<TaktCustomerServiceTicket> customerServiceTicketRepository,
        ITaktCompanyRepository<TaktCustomerServiceContract> customerServiceContractRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceRequestRepository = customerServiceRequestRepository;
        _customerServiceOrderRepository = customerServiceOrderRepository;
        _customerServiceTicketRepository = customerServiceTicketRepository;
        _customerServiceContractRepository = customerServiceContractRepository;
    }

    /// <inheritdoc />
    public async Task<TaktServiceRequestStatDto> GetServiceRequestStatAsync(TaktServiceRequestStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.RequestDateStart,
            queryDto.RequestDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktCustomerServiceRequest, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.RequestDate >= start
            && x.RequestDate <= end;
        var monthRequestCount = await _customerServiceRequestRepository.CountAsync(predicate);
        return new TaktServiceRequestStatDto
        {
            StatMonth = statMonth,
            MonthRequestCount = monthRequestCount,
        };
    }

    /// <inheritdoc />
    public async Task<TaktServiceOrderStatDto> GetServiceOrderStatAsync(TaktServiceOrderStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.OrderDateStart,
            queryDto.OrderDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktCustomerServiceOrder, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.OrderDate >= start
            && x.OrderDate <= end;
        var monthOrderCount = await _customerServiceOrderRepository.CountAsync(predicate);
        var monthTotalAmount = await _customerServiceOrderRepository.SumAsync(x => x.TotalAmount, predicate);
        return new TaktServiceOrderStatDto
        {
            StatMonth = statMonth,
            MonthOrderCount = monthOrderCount,
            MonthTotalAmount = monthTotalAmount,
        };
    }

    /// <inheritdoc />
    public async Task<TaktServiceTicketStatDto> GetServiceTicketStatAsync(TaktServiceTicketStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.CreatedAtStart,
            queryDto.CreatedAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktCustomerServiceTicket, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end;
        var monthTicketCount = await _customerServiceTicketRepository.CountAsync(predicate);
        Expression<Func<TaktCustomerServiceTicket, bool>> openPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && x.TicketStatus >= 0
            && x.TicketStatus <= 3;
        var monthOpenTicketCount = await _customerServiceTicketRepository.CountAsync(openPredicate);
        Expression<Func<TaktCustomerServiceTicket, bool>> closedPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && x.TicketStatus >= 4
            && x.TicketStatus <= 5;
        var monthClosedTicketCount = await _customerServiceTicketRepository.CountAsync(closedPredicate);
        return new TaktServiceTicketStatDto
        {
            StatMonth = statMonth,
            MonthTicketCount = monthTicketCount,
            MonthOpenTicketCount = monthOpenTicketCount,
            MonthClosedTicketCount = monthClosedTicketCount,
        };
    }

    /// <inheritdoc />
    public async Task<TaktServiceContractStatDto> GetServiceContractStatAsync(TaktServiceContractStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.EffectiveDateStart,
            queryDto.EffectiveDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktCustomerServiceContract, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.EffectiveDate >= start
            && x.EffectiveDate <= end;
        var monthContractCount = await _customerServiceContractRepository.CountAsync(predicate);
        var monthContractAmount = await _customerServiceContractRepository.SumAsync(x => x.ContractAmount, predicate);
        return new TaktServiceContractStatDto
        {
            StatMonth = statMonth,
            MonthContractCount = monthContractCount,
            MonthContractAmount = monthContractAmount,
        };
    }
}
