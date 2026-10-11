// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceTicketService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务工单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Domain.Entities.Logistics.CustomerService;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 服务工单应用服务
/// </summary>
public class TaktCustomerServiceTicketService : TaktServiceBase, ITaktCustomerServiceTicketService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceTicket> _customerServiceTicketRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceRequest> _customerServiceRequestRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceOrder> _customerServiceOrderRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceContract> _customerServiceContractRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceTicketRepository">服务工单仓储</param>
    /// <param name="customerServiceRequestRepository">服务请求仓储</param>
    /// <param name="customerServiceOrderRepository">服务订单仓储</param>
    /// <param name="customerServiceContractRepository">服务合同仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceTicketService(
        ITaktCompanyRepository<TaktCustomerServiceTicket> customerServiceTicketRepository,
        ITaktCompanyRepository<TaktCustomerServiceRequest> customerServiceRequestRepository,
        ITaktCompanyRepository<TaktCustomerServiceOrder> customerServiceOrderRepository,
        ITaktCompanyRepository<TaktCustomerServiceContract> customerServiceContractRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceTicketRepository = customerServiceTicketRepository;
        _customerServiceRequestRepository = customerServiceRequestRepository;
        _customerServiceOrderRepository = customerServiceOrderRepository;
        _customerServiceContractRepository = customerServiceContractRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务工单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerServiceTicketDto>> GetCustomerServiceTicketListAsync(TaktCustomerServiceTicketQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerServiceTicketDto>.Create(
                new List<TaktCustomerServiceTicketDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerServiceTicketRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerServiceTicketDto>.Create(
            data.Adapt<List<TaktCustomerServiceTicketDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceTicketDto?> GetCustomerServiceTicketByIdAsync(long id)
    {
        var entity = await _customerServiceTicketRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerServiceTicketDto>();
    }

    /// <summary>
    /// 获取服务工单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerServiceTicketOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerServiceTicketRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TicketStatus == 1,
            x => x.AssignedEmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ServiceTicketCode,
            DictLabel = e.AssignedEmployeeName ?? e.ServiceTicketCode,
        }).ToList();
    }

    /// <summary>
    /// 创建服务工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceTicketDto> CreateCustomerServiceTicketAsync(TaktCustomerServiceTicketCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerServiceTicket>();
        await StampCustomerServiceTicketCustomerServiceRequestAsync(entity, dto);
        await StampCustomerServiceTicketCustomerServiceOrderAsync(entity, dto);
        await StampCustomerServiceTicketCustomerServiceContractAsync(entity, dto);
        var isUnique_ix_takt_logistics_customer_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceTicketRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceTicketCode == entity.ServiceTicketCode);
        if (!isUnique_ix_takt_logistics_customer_service_ticket_code_unique)
        {
            throw new TaktBusinessException("服务工单的PlantCode、ServiceTicketCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerServiceTicketRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _customerServiceTicketRepository.CreateAsync(entity);
        return await GetCustomerServiceTicketByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerServiceTicketDto>();
    }

    /// <summary>
    /// 更新服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceTicketDto> UpdateCustomerServiceTicketAsync(long id, TaktCustomerServiceTicketUpdateDto dto)
    {
        var entity = await _customerServiceTicketRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务工单不存在");
        }
        dto.Adapt(entity);
        await StampCustomerServiceTicketCustomerServiceRequestAsync(entity, dto);
        await StampCustomerServiceTicketCustomerServiceOrderAsync(entity, dto);
        await StampCustomerServiceTicketCustomerServiceContractAsync(entity, dto);
        var isUnique_ix_takt_logistics_customer_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceTicketRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceTicketCode == entity.ServiceTicketCode,
            id);
        if (!isUnique_ix_takt_logistics_customer_service_ticket_code_unique)
        {
            throw new TaktBusinessException("服务工单的PlantCode、ServiceTicketCode已存在");
        }
        await _customerServiceTicketRepository.UpdateAsync(entity);
        return await GetCustomerServiceTicketByIdAsync(id) ?? throw new TaktBusinessException("服务工单不存在");
    }

    /// <summary>
    /// 删除服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceTicketByIdAsync(long id)
    {
        var deleted = await _customerServiceTicketRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceTicketBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerServiceTicketByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceTicketDto> UpdateCustomerServiceTicketStatusAsync(TaktCustomerServiceTicketStatusDto dto)
    {
        var entity = await _customerServiceTicketRepository.GetByIdAsync(dto.CustomerServiceTicketId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务工单不存在");
        }
        entity.TicketStatus = dto.TicketStatus;
        await _customerServiceTicketRepository.UpdateAsync(entity);
        return await GetCustomerServiceTicketByIdAsync(dto.CustomerServiceTicketId) ?? throw new TaktBusinessException("服务工单不存在");
    }

    /// <summary>
    /// 更新服务工单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceTicketDto> UpdateCustomerServiceTicketSortAsync(TaktCustomerServiceTicketSortDto dto)
    {
        var entity = await _customerServiceTicketRepository.GetByIdAsync(dto.CustomerServiceTicketId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务工单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerServiceTicketRepository.UpdateAsync(entity);
        return await GetCustomerServiceTicketByIdAsync(dto.CustomerServiceTicketId) ?? throw new TaktBusinessException("服务工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerServiceTicketTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerServiceTicketTemplateDto>(
            sheetName ?? "服务工单导入模板",
            fileName ?? "服务工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerServiceTicketAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerServiceTicketImportDto>(fileStream, sheetName ?? "服务工单导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCustomerServiceTicket>();
                var importDto = rows[i].Adapt<TaktCustomerServiceTicketCreateDto>();
                await StampCustomerServiceTicketCustomerServiceRequestAsync(entity, importDto);
                await StampCustomerServiceTicketCustomerServiceOrderAsync(entity, importDto);
                await StampCustomerServiceTicketCustomerServiceContractAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.ServiceTicketCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceTicketCode）");
                }
                var isUnique_ix_takt_logistics_customer_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerServiceTicketRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceTicketCode == entity.ServiceTicketCode);
                if (!isUnique_ix_takt_logistics_customer_service_ticket_code_unique)
                {
                    throw new TaktBusinessException("服务工单的PlantCode、ServiceTicketCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerServiceTicketRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _customerServiceTicketRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出服务工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerServiceTicketAsync(TaktCustomerServiceTicketQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerServiceTicketQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceTicketExportDto>(),
                sheetName ?? "服务工单数据",
                fileName ?? "服务工单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerServiceTicketRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceTicketExportDto>(),
                sheetName ?? "服务工单数据",
                fileName ?? "服务工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerServiceTicketExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务工单数据",
            fileName ?? "服务工单导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步服务工单主表外键（ManyToOne → 服务请求）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerServiceTicketCustomerServiceRequestAsync(TaktCustomerServiceTicket entity, TaktCustomerServiceTicketCreateDto dto)
    {
        if (dto.ServiceRequestId is not > 0)
        {
            return;
        }
        var master = await _customerServiceRequestRepository.GetByIdAsync(dto.ServiceRequestId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        entity.ServiceRequestId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.ClientCode))
        {
            entity.ClientCode = master.ClientCode;
        }
        if (string.IsNullOrEmpty(entity.ClientName1))
        {
            entity.ClientName1 = master.ClientName1;
        }
        if (string.IsNullOrEmpty(entity.ServiceRequestCode))
        {
            entity.ServiceRequestCode = master.ServiceRequestCode;
        }
        if (string.IsNullOrEmpty(entity.ServiceContractCode))
        {
            entity.ServiceContractCode = master.ServiceContractCode ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.AssignedEmployeeName))
        {
            entity.AssignedEmployeeName = master.AssignedEmployeeName ?? string.Empty;
        }
    }

    /// <summary>
    /// 同步服务工单主表外键（ManyToOne → 服务订单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerServiceTicketCustomerServiceOrderAsync(TaktCustomerServiceTicket entity, TaktCustomerServiceTicketCreateDto dto)
    {
        if (dto.ServiceOrderId is not > 0)
        {
            return;
        }
        var master = await _customerServiceOrderRepository.GetByIdAsync(dto.ServiceOrderId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        entity.ServiceOrderId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.ClientCode))
        {
            entity.ClientCode = master.ClientCode;
        }
        if (string.IsNullOrEmpty(entity.ClientName1))
        {
            entity.ClientName1 = master.ClientName1;
        }
        if (string.IsNullOrEmpty(entity.ServiceRequestCode))
        {
            entity.ServiceRequestCode = master.ServiceRequestCode ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.ServiceOrderCode))
        {
            entity.ServiceOrderCode = master.ServiceOrderCode;
        }
        if (string.IsNullOrEmpty(entity.ServiceContractCode))
        {
            entity.ServiceContractCode = master.ServiceContractCode ?? string.Empty;
        }
    }

    /// <summary>
    /// 同步服务工单主表外键（ManyToOne → 服务合同）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerServiceTicketCustomerServiceContractAsync(TaktCustomerServiceTicket entity, TaktCustomerServiceTicketCreateDto dto)
    {
        if (dto.ServiceContractId is not > 0)
        {
            return;
        }
        var master = await _customerServiceContractRepository.GetByIdAsync(dto.ServiceContractId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.ServiceContractId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.ClientCode))
        {
            entity.ClientCode = master.ClientCode;
        }
        if (string.IsNullOrEmpty(entity.ClientName1))
        {
            entity.ClientName1 = master.ClientName1;
        }
        if (string.IsNullOrEmpty(entity.ServiceContractCode))
        {
            entity.ServiceContractCode = master.ServiceContractCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerServiceTicket, bool>> QueryExpression(TaktCustomerServiceTicketQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerServiceTicket>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceTicketCode != null && x.ServiceTicketCode.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || (x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(keywords))
                || (x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(keywords))
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || (x.TicketSubject != null && x.TicketSubject.Contains(keywords))
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.SolutionDescription != null && x.SolutionDescription.Contains(keywords))
                || (x.ServiceLocation != null && x.ServiceLocation.Contains(keywords))
                || (x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(keywords))
                || (x.AcceptedByEmployeeName != null && x.AcceptedByEmployeeName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceTicketCode))
        {
            var serviceTicketCode = queryDto.ServiceTicketCode;
            exp = exp.And(x => x.ServiceTicketCode != null && x.ServiceTicketCode.Contains(serviceTicketCode));
        }

        if (queryDto?.ClientId.HasValue == true)
        {
            var clientId = queryDto.ClientId.Value;
            exp = exp.And(x => x.ClientId == clientId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientCode))
        {
            var clientCode = queryDto.ClientCode;
            exp = exp.And(x => x.ClientCode != null && x.ClientCode.Contains(clientCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientName1))
        {
            var clientName1 = queryDto.ClientName1;
            exp = exp.And(x => x.ClientName1 != null && x.ClientName1.Contains(clientName1));
        }

        if (queryDto?.ServiceRequestId.HasValue == true)
        {
            var serviceRequestId = queryDto.ServiceRequestId.Value;
            exp = exp.And(x => x.ServiceRequestId == serviceRequestId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceRequestCode))
        {
            var serviceRequestCode = queryDto.ServiceRequestCode;
            exp = exp.And(x => x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(serviceRequestCode));
        }

        if (queryDto?.ServiceOrderId.HasValue == true)
        {
            var serviceOrderId = queryDto.ServiceOrderId.Value;
            exp = exp.And(x => x.ServiceOrderId == serviceOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceOrderCode))
        {
            var serviceOrderCode = queryDto.ServiceOrderCode;
            exp = exp.And(x => x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(serviceOrderCode));
        }

        if (queryDto?.ServiceContractId.HasValue == true)
        {
            var serviceContractId = queryDto.ServiceContractId.Value;
            exp = exp.And(x => x.ServiceContractId == serviceContractId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceContractCode))
        {
            var serviceContractCode = queryDto.ServiceContractCode;
            exp = exp.And(x => x.ServiceContractCode != null && x.ServiceContractCode.Contains(serviceContractCode));
        }

        if (queryDto?.TicketType.HasValue == true)
        {
            var ticketType = queryDto.TicketType.Value;
            exp = exp.And(x => x.TicketType == ticketType);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            var priority = queryDto.Priority.Value;
            exp = exp.And(x => x.Priority == priority);
        }

        if (queryDto?.TicketStatus.HasValue == true)
        {
            var ticketStatus = queryDto.TicketStatus.Value;
            exp = exp.And(x => x.TicketStatus == ticketStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TicketSubject))
        {
            var ticketSubject = queryDto.TicketSubject;
            exp = exp.And(x => x.TicketSubject != null && x.TicketSubject.Contains(ticketSubject));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FaultDescription))
        {
            var faultDescription = queryDto.FaultDescription;
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(faultDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SolutionDescription))
        {
            var solutionDescription = queryDto.SolutionDescription;
            exp = exp.And(x => x.SolutionDescription != null && x.SolutionDescription.Contains(solutionDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceLocation))
        {
            var serviceLocation = queryDto.ServiceLocation;
            exp = exp.And(x => x.ServiceLocation != null && x.ServiceLocation.Contains(serviceLocation));
        }

        if (queryDto?.AssignedEmployeeId.HasValue == true)
        {
            var assignedEmployeeId = queryDto.AssignedEmployeeId.Value;
            exp = exp.And(x => x.AssignedEmployeeId == assignedEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssignedEmployeeName))
        {
            var assignedEmployeeName = queryDto.AssignedEmployeeName;
            exp = exp.And(x => x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(assignedEmployeeName));
        }

        if (queryDto?.AcceptanceResult.HasValue == true)
        {
            var acceptanceResult = queryDto.AcceptanceResult.Value;
            exp = exp.And(x => x.AcceptanceResult == acceptanceResult);
        }

        if (queryDto?.AcceptedByEmployeeId.HasValue == true)
        {
            var acceptedByEmployeeId = queryDto.AcceptedByEmployeeId.Value;
            exp = exp.And(x => x.AcceptedByEmployeeId == acceptedByEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AcceptedByEmployeeName))
        {
            var acceptedByEmployeeName = queryDto.AcceptedByEmployeeName;
            exp = exp.And(x => x.AcceptedByEmployeeName != null && x.AcceptedByEmployeeName.Contains(acceptedByEmployeeName));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.ScheduledStartTimeStart.HasValue == true)
        {
            var scheduledStartTimeStart = queryDto.ScheduledStartTimeStart.Value;
            exp = exp.And(x => x.ScheduledStartTime >= scheduledStartTimeStart);
        }

        if (queryDto?.ScheduledStartTimeEnd.HasValue == true)
        {
            var scheduledStartTimeEnd = queryDto.ScheduledStartTimeEnd.Value;
            exp = exp.And(x => x.ScheduledStartTime <= scheduledStartTimeEnd);
        }

        if (queryDto?.ScheduledEndTimeStart.HasValue == true)
        {
            var scheduledEndTimeStart = queryDto.ScheduledEndTimeStart.Value;
            exp = exp.And(x => x.ScheduledEndTime >= scheduledEndTimeStart);
        }

        if (queryDto?.ScheduledEndTimeEnd.HasValue == true)
        {
            var scheduledEndTimeEnd = queryDto.ScheduledEndTimeEnd.Value;
            exp = exp.And(x => x.ScheduledEndTime <= scheduledEndTimeEnd);
        }

        if (queryDto?.ActualStartTimeStart.HasValue == true)
        {
            var actualStartTimeStart = queryDto.ActualStartTimeStart.Value;
            exp = exp.And(x => x.ActualStartTime >= actualStartTimeStart);
        }

        if (queryDto?.ActualStartTimeEnd.HasValue == true)
        {
            var actualStartTimeEnd = queryDto.ActualStartTimeEnd.Value;
            exp = exp.And(x => x.ActualStartTime <= actualStartTimeEnd);
        }

        if (queryDto?.ActualEndTimeStart.HasValue == true)
        {
            var actualEndTimeStart = queryDto.ActualEndTimeStart.Value;
            exp = exp.And(x => x.ActualEndTime >= actualEndTimeStart);
        }

        if (queryDto?.ActualEndTimeEnd.HasValue == true)
        {
            var actualEndTimeEnd = queryDto.ActualEndTimeEnd.Value;
            exp = exp.And(x => x.ActualEndTime <= actualEndTimeEnd);
        }

        if (queryDto?.AcceptedAtStart.HasValue == true)
        {
            var acceptedAtStart = queryDto.AcceptedAtStart.Value;
            exp = exp.And(x => x.AcceptedAt >= acceptedAtStart);
        }

        if (queryDto?.AcceptedAtEnd.HasValue == true)
        {
            var acceptedAtEnd = queryDto.AcceptedAtEnd.Value;
            exp = exp.And(x => x.AcceptedAt <= acceptedAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktCustomerServiceTicketQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceTicketCode))
        {
            return true;
        }
        if (queryDto.ClientId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientName1))
        {
            return true;
        }
        if (queryDto.ServiceRequestId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceRequestCode))
        {
            return true;
        }
        if (queryDto.ServiceOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceOrderCode))
        {
            return true;
        }
        if (queryDto.ServiceContractId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceContractCode))
        {
            return true;
        }
        if (queryDto.TicketType.HasValue)
        {
            return true;
        }
        if (queryDto.Priority.HasValue)
        {
            return true;
        }
        if (queryDto.TicketStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TicketSubject))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FaultDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SolutionDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceLocation))
        {
            return true;
        }
        if (queryDto.AssignedEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssignedEmployeeName))
        {
            return true;
        }
        if (queryDto.AcceptanceResult.HasValue)
        {
            return true;
        }
        if (queryDto.AcceptedByEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AcceptedByEmployeeName))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.ScheduledStartTimeStart.HasValue || queryDto.ScheduledStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ScheduledEndTimeStart.HasValue || queryDto.ScheduledEndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualStartTimeStart.HasValue || queryDto.ActualStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualEndTimeStart.HasValue || queryDto.ActualEndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.AcceptedAtStart.HasValue || queryDto.AcceptedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
