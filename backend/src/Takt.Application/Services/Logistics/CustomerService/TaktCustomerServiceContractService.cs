// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceContractService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：服务合同应用服务实现
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
/// 服务合同应用服务
/// </summary>
public class TaktCustomerServiceContractService : TaktServiceBase, ITaktCustomerServiceContractService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceContract> _customerServiceContractRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceContractRepository">服务合同仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceContractService(
        ITaktCompanyRepository<TaktCustomerServiceContract> customerServiceContractRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceContractRepository = customerServiceContractRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务合同列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerServiceContractDto>> GetCustomerServiceContractListAsync(TaktCustomerServiceContractQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerServiceContractDto>.Create(
                new List<TaktCustomerServiceContractDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerServiceContractRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerServiceContractDto>.Create(
            data.Adapt<List<TaktCustomerServiceContractDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto?> GetCustomerServiceContractByIdAsync(long id)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerServiceContractDto>();
    }

    /// <summary>
    /// 获取服务合同选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerServiceContractOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _customerServiceContractRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ContractStatus == 1,
            x => x.ContractName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ServiceContractCode,
            DictLabel = e.ContractName ?? e.ServiceContractCode,
        }).ToList();
    }

    /// <summary>
    /// 创建服务合同
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> CreateCustomerServiceContractAsync(TaktCustomerServiceContractCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerServiceContract>();
        var isUnique_ix_takt_logistics_customer_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceContractRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceContractCode == entity.ServiceContractCode);
        if (!isUnique_ix_takt_logistics_customer_service_contract_code_unique)
        {
            throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerServiceContractRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _customerServiceContractRepository.CreateAsync(entity);
        return await GetCustomerServiceContractByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerServiceContractDto>();
    }

    /// <summary>
    /// 更新服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> UpdateCustomerServiceContractAsync(long id, TaktCustomerServiceContractUpdateDto dto)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_customer_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceContractRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceContractCode == entity.ServiceContractCode,
            id);
        if (!isUnique_ix_takt_logistics_customer_service_contract_code_unique)
        {
            throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
        }
        await _customerServiceContractRepository.UpdateAsync(entity);
        return await GetCustomerServiceContractByIdAsync(id) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 删除服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceContractByIdAsync(long id)
    {
        var deleted = await _customerServiceContractRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务合同不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务合同
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceContractBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerServiceContractByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务合同状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> UpdateCustomerServiceContractStatusAsync(TaktCustomerServiceContractStatusDto dto)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(dto.CustomerServiceContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.ContractStatus = dto.ContractStatus;
        await _customerServiceContractRepository.UpdateAsync(entity);
        return await GetCustomerServiceContractByIdAsync(dto.CustomerServiceContractId) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 更新服务合同排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> UpdateCustomerServiceContractSortAsync(TaktCustomerServiceContractSortDto dto)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(dto.CustomerServiceContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerServiceContractRepository.UpdateAsync(entity);
        return await GetCustomerServiceContractByIdAsync(dto.CustomerServiceContractId) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerServiceContractTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerServiceContractTemplateDto>(
            sheetName ?? "服务合同导入模板",
            fileName ?? "服务合同导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务合同
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerServiceContractAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerServiceContractImportDto>(fileStream, sheetName ?? "服务合同导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerServiceContract>();
                var importKey = $"{entity.PlantCode}|{entity.ServiceContractCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceContractCode）");
                }
                var isUnique_ix_takt_logistics_customer_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerServiceContractRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceContractCode == entity.ServiceContractCode);
                if (!isUnique_ix_takt_logistics_customer_service_contract_code_unique)
                {
                    throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerServiceContractRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _customerServiceContractRepository.CreateAsync(entity);
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
    /// 导出服务合同
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerServiceContractAsync(TaktCustomerServiceContractQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerServiceContractQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceContractExportDto>(),
                sheetName ?? "服务合同数据",
                fileName ?? "服务合同导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerServiceContractRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceContractExportDto>(),
                sheetName ?? "服务合同数据",
                fileName ?? "服务合同导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerServiceContractExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务合同数据",
            fileName ?? "服务合同导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务合同查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerServiceContract, bool>> QueryExpression(TaktCustomerServiceContractQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerServiceContract>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || (x.ContractName != null && x.ContractName.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.ServiceScope != null && x.ServiceScope.Contains(keywords))
                || (x.AccountManagerEmployeeName != null && x.AccountManagerEmployeeName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceContractCode))
        {
            var serviceContractCode = queryDto.ServiceContractCode;
            exp = exp.And(x => x.ServiceContractCode != null && x.ServiceContractCode.Contains(serviceContractCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContractName))
        {
            var contractName = queryDto.ContractName;
            exp = exp.And(x => x.ContractName != null && x.ContractName.Contains(contractName));
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

        if (queryDto?.ContractType.HasValue == true)
        {
            var contractType = queryDto.ContractType.Value;
            exp = exp.And(x => x.ContractType == contractType);
        }

        if (queryDto?.ContractStatus.HasValue == true)
        {
            var contractStatus = queryDto.ContractStatus.Value;
            exp = exp.And(x => x.ContractStatus == contractStatus);
        }

        if (queryDto?.ContractAmount.HasValue == true)
        {
            var contractAmount = queryDto.ContractAmount.Value;
            exp = exp.And(x => x.ContractAmount == contractAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (queryDto?.PaymentTerms.HasValue == true)
        {
            var paymentTerms = queryDto.PaymentTerms.Value;
            exp = exp.And(x => x.PaymentTerms == paymentTerms);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceScope))
        {
            var serviceScope = queryDto.ServiceScope;
            exp = exp.And(x => x.ServiceScope != null && x.ServiceScope.Contains(serviceScope));
        }

        if (queryDto?.SlaResponseHours.HasValue == true)
        {
            var slaResponseHours = queryDto.SlaResponseHours.Value;
            exp = exp.And(x => x.SlaResponseHours == slaResponseHours);
        }

        if (queryDto?.SlaResolveHours.HasValue == true)
        {
            var slaResolveHours = queryDto.SlaResolveHours.Value;
            exp = exp.And(x => x.SlaResolveHours == slaResolveHours);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountManagerEmployeeName))
        {
            var accountManager = queryDto.AccountManagerEmployeeName;
            exp = exp.And(x => x.AccountManagerEmployeeName != null && x.AccountManagerEmployeeName.Contains(accountManager));
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

        if (queryDto?.SignDateStart.HasValue == true)
        {
            var signDateStart = queryDto.SignDateStart.Value;
            exp = exp.And(x => x.SignDate >= signDateStart);
        }

        if (queryDto?.SignDateEnd.HasValue == true)
        {
            var signDateEnd = queryDto.SignDateEnd.Value;
            exp = exp.And(x => x.SignDate <= signDateEnd);
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            var effectiveDateStart = queryDto.EffectiveDateStart.Value;
            exp = exp.And(x => x.EffectiveDate >= effectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            var effectiveDateEnd = queryDto.EffectiveDateEnd.Value;
            exp = exp.And(x => x.EffectiveDate <= effectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            var expiryDateStart = queryDto.ExpiryDateStart.Value;
            exp = exp.And(x => x.ExpiryDate >= expiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            var expiryDateEnd = queryDto.ExpiryDateEnd.Value;
            exp = exp.And(x => x.ExpiryDate <= expiryDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktCustomerServiceContractQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceContractCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContractName))
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
        if (queryDto.ContractType.HasValue)
        {
            return true;
        }
        if (queryDto.ContractStatus.HasValue)
        {
            return true;
        }
        if (queryDto.ContractAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (queryDto.PaymentTerms.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceScope))
        {
            return true;
        }
        if (queryDto.SlaResponseHours.HasValue)
        {
            return true;
        }
        if (queryDto.SlaResolveHours.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountManagerEmployeeName))
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
        if (queryDto.SignDateStart.HasValue || queryDto.SignDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EffectiveDateStart.HasValue || queryDto.EffectiveDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExpiryDateStart.HasValue || queryDto.ExpiryDateEnd.HasValue)
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
