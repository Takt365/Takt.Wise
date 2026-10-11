// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceRequestService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务请求应用服务实现
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
/// 服务请求应用服务
/// </summary>
public class TaktCustomerServiceRequestService : TaktServiceBase, ITaktCustomerServiceRequestService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceRequest> _customerServiceRequestRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceContract> _customerServiceContractRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceRequestRepository">服务请求仓储</param>
    /// <param name="customerServiceContractRepository">服务合同仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceRequestService(
        ITaktCompanyRepository<TaktCustomerServiceRequest> customerServiceRequestRepository,
        ITaktCompanyRepository<TaktCustomerServiceContract> customerServiceContractRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceRequestRepository = customerServiceRequestRepository;
        _customerServiceContractRepository = customerServiceContractRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务请求列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerServiceRequestDto>> GetCustomerServiceRequestListAsync(TaktCustomerServiceRequestQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerServiceRequestDto>.Create(
                new List<TaktCustomerServiceRequestDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerServiceRequestRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerServiceRequestDto>.Create(
            data.Adapt<List<TaktCustomerServiceRequestDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceRequestDto?> GetCustomerServiceRequestByIdAsync(long id)
    {
        var entity = await _customerServiceRequestRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerServiceRequestDto>();
    }

    /// <summary>
    /// 获取服务请求选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerServiceRequestOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerServiceRequestRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RequestStatus == 1,
            x => x.AssignedEmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ServiceRequestCode,
            DictLabel = e.AssignedEmployeeName ?? e.ServiceRequestCode,
        }).ToList();
    }

    /// <summary>
    /// 创建服务请求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceRequestDto> CreateCustomerServiceRequestAsync(TaktCustomerServiceRequestCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerServiceRequest>();
        await StampCustomerServiceRequestCustomerServiceContractAsync(entity, dto);
        var isUnique_ix_takt_logistics_customer_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceRequestCode == entity.ServiceRequestCode);
        if (!isUnique_ix_takt_logistics_customer_service_request_code_unique)
        {
            throw new TaktBusinessException("服务请求的PlantCode、ServiceRequestCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerServiceRequestRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _customerServiceRequestRepository.CreateAsync(entity);
        return await GetCustomerServiceRequestByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerServiceRequestDto>();
    }

    /// <summary>
    /// 更新服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceRequestDto> UpdateCustomerServiceRequestAsync(long id, TaktCustomerServiceRequestUpdateDto dto)
    {
        var entity = await _customerServiceRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        dto.Adapt(entity);
        await StampCustomerServiceRequestCustomerServiceContractAsync(entity, dto);
        var isUnique_ix_takt_logistics_customer_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceRequestCode == entity.ServiceRequestCode,
            id);
        if (!isUnique_ix_takt_logistics_customer_service_request_code_unique)
        {
            throw new TaktBusinessException("服务请求的PlantCode、ServiceRequestCode已存在");
        }
        await _customerServiceRequestRepository.UpdateAsync(entity);
        return await GetCustomerServiceRequestByIdAsync(id) ?? throw new TaktBusinessException("服务请求不存在");
    }

    /// <summary>
    /// 删除服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceRequestByIdAsync(long id)
    {
        var deleted = await _customerServiceRequestRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务请求不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务请求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceRequestBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerServiceRequestByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务请求状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceRequestDto> UpdateCustomerServiceRequestStatusAsync(TaktCustomerServiceRequestStatusDto dto)
    {
        var entity = await _customerServiceRequestRepository.GetByIdAsync(dto.CustomerServiceRequestId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        entity.RequestStatus = dto.RequestStatus;
        await _customerServiceRequestRepository.UpdateAsync(entity);
        return await GetCustomerServiceRequestByIdAsync(dto.CustomerServiceRequestId) ?? throw new TaktBusinessException("服务请求不存在");
    }

    /// <summary>
    /// 更新服务请求排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceRequestDto> UpdateCustomerServiceRequestSortAsync(TaktCustomerServiceRequestSortDto dto)
    {
        var entity = await _customerServiceRequestRepository.GetByIdAsync(dto.CustomerServiceRequestId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerServiceRequestRepository.UpdateAsync(entity);
        return await GetCustomerServiceRequestByIdAsync(dto.CustomerServiceRequestId) ?? throw new TaktBusinessException("服务请求不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerServiceRequestTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerServiceRequestTemplateDto>(
            sheetName ?? "服务请求导入模板",
            fileName ?? "服务请求导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务请求
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerServiceRequestAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerServiceRequestImportDto>(fileStream, sheetName ?? "服务请求导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerServiceRequest>();
                var importDto = rows[i].Adapt<TaktCustomerServiceRequestCreateDto>();
                await StampCustomerServiceRequestCustomerServiceContractAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.ServiceRequestCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceRequestCode）");
                }
                var isUnique_ix_takt_logistics_customer_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerServiceRequestRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceRequestCode == entity.ServiceRequestCode);
                if (!isUnique_ix_takt_logistics_customer_service_request_code_unique)
                {
                    throw new TaktBusinessException("服务请求的PlantCode、ServiceRequestCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerServiceRequestRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _customerServiceRequestRepository.CreateAsync(entity);
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
    /// 导出服务请求
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerServiceRequestAsync(TaktCustomerServiceRequestQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerServiceRequestQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceRequestExportDto>(),
                sheetName ?? "服务请求数据",
                fileName ?? "服务请求导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerServiceRequestRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceRequestExportDto>(),
                sheetName ?? "服务请求数据",
                fileName ?? "服务请求导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerServiceRequestExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务请求数据",
            fileName ?? "服务请求导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步服务请求主表外键（ManyToOne → 服务合同）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerServiceRequestCustomerServiceContractAsync(TaktCustomerServiceRequest entity, TaktCustomerServiceRequestCreateDto dto)
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
    /// 构建服务请求查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerServiceRequest, bool>> QueryExpression(TaktCustomerServiceRequestQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerServiceRequest>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || (x.RequestSubject != null && x.RequestSubject.Contains(keywords))
                || (x.RequestDescription != null && x.RequestDescription.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.ServiceAddress != null && x.ServiceAddress.Contains(keywords))
                || (x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceRequestCode))
        {
            var serviceRequestCode = queryDto.ServiceRequestCode;
            exp = exp.And(x => x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(serviceRequestCode));
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

        if (queryDto?.RequestType.HasValue == true)
        {
            var requestType = queryDto.RequestType.Value;
            exp = exp.And(x => x.RequestType == requestType);
        }

        if (queryDto?.SourceChannel.HasValue == true)
        {
            var sourceChannel = queryDto.SourceChannel.Value;
            exp = exp.And(x => x.SourceChannel == sourceChannel);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            var priority = queryDto.Priority.Value;
            exp = exp.And(x => x.Priority == priority);
        }

        if (queryDto?.RequestStatus.HasValue == true)
        {
            var requestStatus = queryDto.RequestStatus.Value;
            exp = exp.And(x => x.RequestStatus == requestStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestSubject))
        {
            var requestSubject = queryDto.RequestSubject;
            exp = exp.And(x => x.RequestSubject != null && x.RequestSubject.Contains(requestSubject));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestDescription))
        {
            var requestDescription = queryDto.RequestDescription;
            exp = exp.And(x => x.RequestDescription != null && x.RequestDescription.Contains(requestDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactPerson))
        {
            var contactPerson = queryDto.ContactPerson;
            exp = exp.And(x => x.ContactPerson != null && x.ContactPerson.Contains(contactPerson));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactPhone))
        {
            var contactPhone = queryDto.ContactPhone;
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(contactPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactEmail))
        {
            var contactEmail = queryDto.ContactEmail;
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(contactEmail));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceAddress))
        {
            var serviceAddress = queryDto.ServiceAddress;
            exp = exp.And(x => x.ServiceAddress != null && x.ServiceAddress.Contains(serviceAddress));
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

        if (queryDto?.RequestDateStart.HasValue == true)
        {
            var requestDateStart = queryDto.RequestDateStart.Value;
            exp = exp.And(x => x.RequestDate >= requestDateStart);
        }

        if (queryDto?.RequestDateEnd.HasValue == true)
        {
            var requestDateEnd = queryDto.RequestDateEnd.Value;
            exp = exp.And(x => x.RequestDate <= requestDateEnd);
        }

        if (queryDto?.ExpectedServiceDateStart.HasValue == true)
        {
            var expectedServiceDateStart = queryDto.ExpectedServiceDateStart.Value;
            exp = exp.And(x => x.ExpectedServiceDate >= expectedServiceDateStart);
        }

        if (queryDto?.ExpectedServiceDateEnd.HasValue == true)
        {
            var expectedServiceDateEnd = queryDto.ExpectedServiceDateEnd.Value;
            exp = exp.And(x => x.ExpectedServiceDate <= expectedServiceDateEnd);
        }

        if (queryDto?.AssignedAtStart.HasValue == true)
        {
            var assignedAtStart = queryDto.AssignedAtStart.Value;
            exp = exp.And(x => x.AssignedAt >= assignedAtStart);
        }

        if (queryDto?.AssignedAtEnd.HasValue == true)
        {
            var assignedAtEnd = queryDto.AssignedAtEnd.Value;
            exp = exp.And(x => x.AssignedAt <= assignedAtEnd);
        }

        if (queryDto?.ClosedAtStart.HasValue == true)
        {
            var closedAtStart = queryDto.ClosedAtStart.Value;
            exp = exp.And(x => x.ClosedAt >= closedAtStart);
        }

        if (queryDto?.ClosedAtEnd.HasValue == true)
        {
            var closedAtEnd = queryDto.ClosedAtEnd.Value;
            exp = exp.And(x => x.ClosedAt <= closedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktCustomerServiceRequestQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceRequestCode))
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
        if (queryDto.RequestType.HasValue)
        {
            return true;
        }
        if (queryDto.SourceChannel.HasValue)
        {
            return true;
        }
        if (queryDto.Priority.HasValue)
        {
            return true;
        }
        if (queryDto.RequestStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RequestSubject))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RequestDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactPerson))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactEmail))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceAddress))
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
        if (queryDto.RequestDateStart.HasValue || queryDto.RequestDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExpectedServiceDateStart.HasValue || queryDto.ExpectedServiceDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.AssignedAtStart.HasValue || queryDto.AssignedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ClosedAtStart.HasValue || queryDto.ClosedAtEnd.HasValue)
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
