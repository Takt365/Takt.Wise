// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceOrderService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务订单应用服务实现
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
/// 服务订单应用服务
/// </summary>
public class TaktCustomerServiceOrderService : TaktServiceBase, ITaktCustomerServiceOrderService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceOrder> _customerServiceOrderRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceContract> _customerServiceContractRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceRequest> _customerServiceRequestRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceOrderRepository">服务订单仓储</param>
    /// <param name="customerServiceContractRepository">服务合同仓储</param>
    /// <param name="customerServiceRequestRepository">服务请求仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceOrderService(
        ITaktCompanyRepository<TaktCustomerServiceOrder> customerServiceOrderRepository,
        ITaktCompanyRepository<TaktCustomerServiceContract> customerServiceContractRepository,
        ITaktCompanyRepository<TaktCustomerServiceRequest> customerServiceRequestRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceOrderRepository = customerServiceOrderRepository;
        _customerServiceContractRepository = customerServiceContractRepository;
        _customerServiceRequestRepository = customerServiceRequestRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务订单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerServiceOrderDto>> GetCustomerServiceOrderListAsync(TaktCustomerServiceOrderQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerServiceOrderDto>.Create(
                new List<TaktCustomerServiceOrderDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerServiceOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerServiceOrderDto>.Create(
            data.Adapt<List<TaktCustomerServiceOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceOrderDto?> GetCustomerServiceOrderByIdAsync(long id)
    {
        var entity = await _customerServiceOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerServiceOrderDto>();
    }

    /// <summary>
    /// 获取服务订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerServiceOrderOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerServiceOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.ServiceOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ServiceOrderCode,
            DictLabel = e.ServiceOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建服务订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceOrderDto> CreateCustomerServiceOrderAsync(TaktCustomerServiceOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerServiceOrder>();
        await StampCustomerServiceOrderCustomerServiceContractAsync(entity, dto);
        await StampCustomerServiceOrderCustomerServiceRequestAsync(entity, dto);
        var isUnique_ix_takt_logistics_customer_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceOrderCode == entity.ServiceOrderCode);
        if (!isUnique_ix_takt_logistics_customer_service_order_code_unique)
        {
            throw new TaktBusinessException("服务订单的PlantCode、ServiceOrderCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerServiceOrderRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _customerServiceOrderRepository.CreateAsync(entity);
        return await GetCustomerServiceOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerServiceOrderDto>();
    }

    /// <summary>
    /// 更新服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceOrderDto> UpdateCustomerServiceOrderAsync(long id, TaktCustomerServiceOrderUpdateDto dto)
    {
        var entity = await _customerServiceOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        dto.Adapt(entity);
        await StampCustomerServiceOrderCustomerServiceContractAsync(entity, dto);
        await StampCustomerServiceOrderCustomerServiceRequestAsync(entity, dto);
        var isUnique_ix_takt_logistics_customer_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceOrderCode == entity.ServiceOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_customer_service_order_code_unique)
        {
            throw new TaktBusinessException("服务订单的PlantCode、ServiceOrderCode已存在");
        }
        await _customerServiceOrderRepository.UpdateAsync(entity);
        return await GetCustomerServiceOrderByIdAsync(id) ?? throw new TaktBusinessException("服务订单不存在");
    }

    /// <summary>
    /// 删除服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceOrderByIdAsync(long id)
    {
        var deleted = await _customerServiceOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerServiceOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceOrderDto> UpdateCustomerServiceOrderStatusAsync(TaktCustomerServiceOrderStatusDto dto)
    {
        var entity = await _customerServiceOrderRepository.GetByIdAsync(dto.CustomerServiceOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _customerServiceOrderRepository.UpdateAsync(entity);
        return await GetCustomerServiceOrderByIdAsync(dto.CustomerServiceOrderId) ?? throw new TaktBusinessException("服务订单不存在");
    }

    /// <summary>
    /// 更新服务订单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceOrderDto> UpdateCustomerServiceOrderSortAsync(TaktCustomerServiceOrderSortDto dto)
    {
        var entity = await _customerServiceOrderRepository.GetByIdAsync(dto.CustomerServiceOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerServiceOrderRepository.UpdateAsync(entity);
        return await GetCustomerServiceOrderByIdAsync(dto.CustomerServiceOrderId) ?? throw new TaktBusinessException("服务订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerServiceOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerServiceOrderTemplateDto>(
            sheetName ?? "服务订单导入模板",
            fileName ?? "服务订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerServiceOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerServiceOrderImportDto>(fileStream, sheetName ?? "服务订单导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerServiceOrder>();
                var importDto = rows[i].Adapt<TaktCustomerServiceOrderCreateDto>();
                await StampCustomerServiceOrderCustomerServiceContractAsync(entity, importDto);
                await StampCustomerServiceOrderCustomerServiceRequestAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.ServiceOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceOrderCode）");
                }
                var isUnique_ix_takt_logistics_customer_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerServiceOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceOrderCode == entity.ServiceOrderCode);
                if (!isUnique_ix_takt_logistics_customer_service_order_code_unique)
                {
                    throw new TaktBusinessException("服务订单的PlantCode、ServiceOrderCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerServiceOrderRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _customerServiceOrderRepository.CreateAsync(entity);
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
    /// 导出服务订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerServiceOrderAsync(TaktCustomerServiceOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerServiceOrderQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceOrderExportDto>(),
                sheetName ?? "服务订单数据",
                fileName ?? "服务订单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerServiceOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceOrderExportDto>(),
                sheetName ?? "服务订单数据",
                fileName ?? "服务订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerServiceOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务订单数据",
            fileName ?? "服务订单导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步服务订单主表外键（ManyToOne → 服务合同）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerServiceOrderCustomerServiceContractAsync(TaktCustomerServiceOrder entity, TaktCustomerServiceOrderCreateDto dto)
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
        if (string.IsNullOrEmpty(entity.CurrencyCode))
        {
            entity.CurrencyCode = master.CurrencyCode;
        }
    }

    /// <summary>
    /// 同步服务订单主表外键（ManyToOne → 服务请求）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerServiceOrderCustomerServiceRequestAsync(TaktCustomerServiceOrder entity, TaktCustomerServiceOrderCreateDto dto)
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
        if (string.IsNullOrEmpty(entity.ServiceContractCode))
        {
            entity.ServiceContractCode = master.ServiceContractCode ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.ServiceRequestCode))
        {
            entity.ServiceRequestCode = master.ServiceRequestCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerServiceOrder, bool>> QueryExpression(TaktCustomerServiceOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerServiceOrder>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || (x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.ServiceEmployeeName != null && x.ServiceEmployeeName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceOrderCode))
        {
            var serviceOrderCode = queryDto.ServiceOrderCode;
            exp = exp.And(x => x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(serviceOrderCode));
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

        if (queryDto?.OrderType.HasValue == true)
        {
            var orderType = queryDto.OrderType.Value;
            exp = exp.And(x => x.OrderType == orderType);
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            var orderStatus = queryDto.OrderStatus.Value;
            exp = exp.And(x => x.OrderStatus == orderStatus);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            var totalAmount = queryDto.TotalAmount.Value;
            exp = exp.And(x => x.TotalAmount == totalAmount);
        }

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            var discountAmount = queryDto.DiscountAmount.Value;
            exp = exp.And(x => x.DiscountAmount == discountAmount);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (queryDto?.ActualAmount.HasValue == true)
        {
            var actualAmount = queryDto.ActualAmount.Value;
            exp = exp.And(x => x.ActualAmount == actualAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (queryDto?.ServiceEmployeeId.HasValue == true)
        {
            var serviceEmployeeId = queryDto.ServiceEmployeeId.Value;
            exp = exp.And(x => x.ServiceEmployeeId == serviceEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceEmployeeName))
        {
            var serviceEmployeeName = queryDto.ServiceEmployeeName;
            exp = exp.And(x => x.ServiceEmployeeName != null && x.ServiceEmployeeName.Contains(serviceEmployeeName));
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

        if (queryDto?.OrderDateStart.HasValue == true)
        {
            var orderDateStart = queryDto.OrderDateStart.Value;
            exp = exp.And(x => x.OrderDate >= orderDateStart);
        }

        if (queryDto?.OrderDateEnd.HasValue == true)
        {
            var orderDateEnd = queryDto.OrderDateEnd.Value;
            exp = exp.And(x => x.OrderDate <= orderDateEnd);
        }

        if (queryDto?.PlannedStartDateStart.HasValue == true)
        {
            var plannedStartDateStart = queryDto.PlannedStartDateStart.Value;
            exp = exp.And(x => x.PlannedStartDate >= plannedStartDateStart);
        }

        if (queryDto?.PlannedStartDateEnd.HasValue == true)
        {
            var plannedStartDateEnd = queryDto.PlannedStartDateEnd.Value;
            exp = exp.And(x => x.PlannedStartDate <= plannedStartDateEnd);
        }

        if (queryDto?.PlannedEndDateStart.HasValue == true)
        {
            var plannedEndDateStart = queryDto.PlannedEndDateStart.Value;
            exp = exp.And(x => x.PlannedEndDate >= plannedEndDateStart);
        }

        if (queryDto?.PlannedEndDateEnd.HasValue == true)
        {
            var plannedEndDateEnd = queryDto.PlannedEndDateEnd.Value;
            exp = exp.And(x => x.PlannedEndDate <= plannedEndDateEnd);
        }

        if (queryDto?.ActualStartDateStart.HasValue == true)
        {
            var actualStartDateStart = queryDto.ActualStartDateStart.Value;
            exp = exp.And(x => x.ActualStartDate >= actualStartDateStart);
        }

        if (queryDto?.ActualStartDateEnd.HasValue == true)
        {
            var actualStartDateEnd = queryDto.ActualStartDateEnd.Value;
            exp = exp.And(x => x.ActualStartDate <= actualStartDateEnd);
        }

        if (queryDto?.ActualEndDateStart.HasValue == true)
        {
            var actualEndDateStart = queryDto.ActualEndDateStart.Value;
            exp = exp.And(x => x.ActualEndDate >= actualEndDateStart);
        }

        if (queryDto?.ActualEndDateEnd.HasValue == true)
        {
            var actualEndDateEnd = queryDto.ActualEndDateEnd.Value;
            exp = exp.And(x => x.ActualEndDate <= actualEndDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktCustomerServiceOrderQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceOrderCode))
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
        if (queryDto.ServiceContractId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceContractCode))
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
        if (queryDto.OrderType.HasValue)
        {
            return true;
        }
        if (queryDto.OrderStatus.HasValue)
        {
            return true;
        }
        if (queryDto.TotalAmount.HasValue)
        {
            return true;
        }
        if (queryDto.DiscountAmount.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ActualAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (queryDto.ServiceEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceEmployeeName))
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
        if (queryDto.OrderDateStart.HasValue || queryDto.OrderDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedStartDateStart.HasValue || queryDto.PlannedStartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedEndDateStart.HasValue || queryDto.PlannedEndDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualStartDateStart.HasValue || queryDto.ActualStartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualEndDateStart.HasValue || queryDto.ActualEndDateEnd.HasValue)
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
