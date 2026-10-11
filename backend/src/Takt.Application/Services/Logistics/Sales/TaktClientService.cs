// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktClientService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 客户端信息应用服务
/// </summary>
public class TaktClientService : TaktServiceBase, ITaktClientService
{
    private readonly ITaktCompanyRepository<TaktClient> _clientRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clientRepository">客户端信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktClientService(
        ITaktCompanyRepository<TaktClient> clientRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _clientRepository = clientRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户端信息列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktClientDto>> GetClientListAsync(TaktClientQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktClientDto>.Create(
                new List<TaktClientDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _clientRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktClientDto>.Create(
            data.Adapt<List<TaktClientDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto?> GetClientByIdAsync(long id)
    {
        var entity = await _clientRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktClientDto>();
    }

    /// <summary>
    /// 获取客户端信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetClientOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _clientRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientStatus == 1,
            x => x.ClientShortName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ClientCode,
            DictLabel = e.ClientShortName ?? e.ClientCode,
        }).ToList();
    }

    /// <summary>
    /// 创建客户端信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> CreateClientAsync(TaktClientCreateDto dto)
    {
        var entity = dto.Adapt<TaktClient>();
        var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
            _clientRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ClientCode == entity.ClientCode);
        if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
        {
            throw new TaktBusinessException("客户端信息的PlantCode、ClientCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _clientRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _clientRepository.CreateAsync(entity);
        return await GetClientByIdAsync(entity.Id) ?? entity.Adapt<TaktClientDto>();
    }

    /// <summary>
    /// 更新客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> UpdateClientAsync(long id, TaktClientUpdateDto dto)
    {
        var entity = await _clientRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户端信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
            _clientRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ClientCode == entity.ClientCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
        {
            throw new TaktBusinessException("客户端信息的PlantCode、ClientCode已存在");
        }
        await _clientRepository.UpdateAsync(entity);
        return await GetClientByIdAsync(id) ?? throw new TaktBusinessException("客户端信息不存在");
    }

    /// <summary>
    /// 删除客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteClientByIdAsync(long id)
    {
        var deleted = await _clientRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客户端信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客户端信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteClientBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteClientByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户端信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> UpdateClientStatusAsync(TaktClientStatusDto dto)
    {
        var entity = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户端信息不存在");
        }
        entity.ClientStatus = dto.ClientStatus;
        await _clientRepository.UpdateAsync(entity);
        return await GetClientByIdAsync(dto.ClientId) ?? throw new TaktBusinessException("客户端信息不存在");
    }

    /// <summary>
    /// 更新客户端信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> UpdateClientSortAsync(TaktClientSortDto dto)
    {
        var entity = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户端信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _clientRepository.UpdateAsync(entity);
        return await GetClientByIdAsync(dto.ClientId) ?? throw new TaktBusinessException("客户端信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetClientTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktClientTemplateDto>(
            sheetName ?? "客户端信息导入模板",
            fileName ?? "客户端信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户端信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportClientAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktClientImportDto>(fileStream, sheetName ?? "客户端信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _clientRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktClient>();
                var importKey = $"{entity.PlantCode}|{entity.ClientCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ClientCode）");
                }
                var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _clientRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ClientCode == entity.ClientCode);
                if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
                {
                    throw new TaktBusinessException("客户端信息的PlantCode、ClientCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _clientRepository.CreateAsync(entity);
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
    /// 导出客户端信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportClientAsync(TaktClientQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktClientQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktClientExportDto>(),
                sheetName ?? "客户端信息数据",
                fileName ?? "客户端信息导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _clientRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktClientExportDto>(),
                sheetName ?? "客户端信息数据",
                fileName ?? "客户端信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktClientExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户端信息数据",
            fileName ?? "客户端信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户端信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktClient, bool>> QueryExpression(TaktClientQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktClient>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || (x.ClientName2 != null && x.ClientName2.Contains(keywords))
                || (x.ClientShortName != null && x.ClientShortName.Contains(keywords))
                || (x.EnterpriseNature != null && x.EnterpriseNature.Contains(keywords))
                || (x.IndustryAttribute != null && x.IndustryAttribute.Contains(keywords))
                || (x.ClientTaxNumber != null && x.ClientTaxNumber.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationProvince != null && x.RegistrationProvince.Contains(keywords))
                || (x.RegistrationCity != null && x.RegistrationCity.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.ClientPhone != null && x.ClientPhone.Contains(keywords))
                || (x.ClientFax != null && x.ClientFax.Contains(keywords))
                || (x.ClientEmail != null && x.ClientEmail.Contains(keywords))
                || (x.ClientWebsite != null && x.ClientWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.SalesOrganization != null && x.SalesOrganization.Contains(keywords))
                || (x.DistributionChannel != null && x.DistributionChannel.Contains(keywords))
                || (x.ProductGroup != null && x.ProductGroup.Contains(keywords))
                || (x.CustomerGroup != null && x.CustomerGroup.Contains(keywords))
                || (x.TradingPartner != null && x.TradingPartner.Contains(keywords))
                || (x.AccountAssignmentGroup != null && x.AccountAssignmentGroup.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.NielsenIndicator != null && x.NielsenIndicator.Contains(keywords))
                || (x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(keywords))
                || (x.Headquarters != null && x.Headquarters.Contains(keywords))
                || (x.PaymentTerms != null && x.PaymentTerms.Contains(keywords))
                || (x.DeliveringPlant != null && x.DeliveringPlant.Contains(keywords))
                || (x.Incoterms1 != null && x.Incoterms1.Contains(keywords))
                || (x.Incoterms2 != null && x.Incoterms2.Contains(keywords))
                || (x.ShippingConditions != null && x.ShippingConditions.Contains(keywords))
                || (x.CustomerPricingProcedure != null && x.CustomerPricingProcedure.Contains(keywords))
                || (x.PlatformName != null && x.PlatformName.Contains(keywords))
                || (x.StoreName != null && x.StoreName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientName2))
        {
            var clientName2 = queryDto.ClientName2;
            exp = exp.And(x => x.ClientName2 != null && x.ClientName2.Contains(clientName2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientShortName))
        {
            var clientShortName = queryDto.ClientShortName;
            exp = exp.And(x => x.ClientShortName != null && x.ClientShortName.Contains(clientShortName));
        }

        if (queryDto?.ClientType.HasValue == true)
        {
            var clientType = queryDto.ClientType.Value;
            exp = exp.And(x => x.ClientType == clientType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EnterpriseNature))
        {
            var enterpriseNature = queryDto.EnterpriseNature;
            exp = exp.And(x => x.EnterpriseNature != null && x.EnterpriseNature.Contains(enterpriseNature));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IndustryAttribute))
        {
            var industryAttribute = queryDto.IndustryAttribute;
            exp = exp.And(x => x.IndustryAttribute != null && x.IndustryAttribute.Contains(industryAttribute));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientTaxNumber))
        {
            var clientTaxNumber = queryDto.ClientTaxNumber;
            exp = exp.And(x => x.ClientTaxNumber != null && x.ClientTaxNumber.Contains(clientTaxNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (queryDto?.TaxRate.HasValue == true)
        {
            var taxRate = queryDto.TaxRate.Value;
            exp = exp.And(x => x.TaxRate == taxRate);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationCountry))
        {
            var registrationCountry = queryDto.RegistrationCountry;
            exp = exp.And(x => x.RegistrationCountry != null && x.RegistrationCountry.Contains(registrationCountry));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationProvince))
        {
            var registrationProvince = queryDto.RegistrationProvince;
            exp = exp.And(x => x.RegistrationProvince != null && x.RegistrationProvince.Contains(registrationProvince));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationCity))
        {
            var registrationCity = queryDto.RegistrationCity;
            exp = exp.And(x => x.RegistrationCity != null && x.RegistrationCity.Contains(registrationCity));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationAddress1))
        {
            var registrationAddress1 = queryDto.RegistrationAddress1;
            exp = exp.And(x => x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(registrationAddress1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationAddress2))
        {
            var registrationAddress2 = queryDto.RegistrationAddress2;
            exp = exp.And(x => x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(registrationAddress2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientPhone))
        {
            var clientPhone = queryDto.ClientPhone;
            exp = exp.And(x => x.ClientPhone != null && x.ClientPhone.Contains(clientPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientFax))
        {
            var clientFax = queryDto.ClientFax;
            exp = exp.And(x => x.ClientFax != null && x.ClientFax.Contains(clientFax));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientEmail))
        {
            var clientEmail = queryDto.ClientEmail;
            exp = exp.And(x => x.ClientEmail != null && x.ClientEmail.Contains(clientEmail));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientWebsite))
        {
            var clientWebsite = queryDto.ClientWebsite;
            exp = exp.And(x => x.ClientWebsite != null && x.ClientWebsite.Contains(clientWebsite));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrganization))
        {
            var salesOrganization = queryDto.SalesOrganization;
            exp = exp.And(x => x.SalesOrganization != null && x.SalesOrganization.Contains(salesOrganization));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DistributionChannel))
        {
            var distributionChannel = queryDto.DistributionChannel;
            exp = exp.And(x => x.DistributionChannel != null && x.DistributionChannel.Contains(distributionChannel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductGroup))
        {
            var productGroup = queryDto.ProductGroup;
            exp = exp.And(x => x.ProductGroup != null && x.ProductGroup.Contains(productGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerGroup))
        {
            var customerGroup = queryDto.CustomerGroup;
            exp = exp.And(x => x.CustomerGroup != null && x.CustomerGroup.Contains(customerGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TradingPartner))
        {
            var tradingPartner = queryDto.TradingPartner;
            exp = exp.And(x => x.TradingPartner != null && x.TradingPartner.Contains(tradingPartner));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountAssignmentGroup))
        {
            var accountAssignmentGroup = queryDto.AccountAssignmentGroup;
            exp = exp.And(x => x.AccountAssignmentGroup != null && x.AccountAssignmentGroup.Contains(accountAssignmentGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NielsenIndicator))
        {
            var nielsenIndicator = queryDto.NielsenIndicator;
            exp = exp.And(x => x.NielsenIndicator != null && x.NielsenIndicator.Contains(nielsenIndicator));
        }

        if (queryDto?.CentralPostingBlock.HasValue == true)
        {
            var centralPostingBlock = queryDto.CentralPostingBlock.Value;
            exp = exp.And(x => x.CentralPostingBlock == centralPostingBlock);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReconciliationAccount))
        {
            var reconciliationAccount = queryDto.ReconciliationAccount;
            exp = exp.And(x => x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(reconciliationAccount));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Headquarters))
        {
            var headquarters = queryDto.Headquarters;
            exp = exp.And(x => x.Headquarters != null && x.Headquarters.Contains(headquarters));
        }

        if (queryDto?.ClearingWithVendor.HasValue == true)
        {
            var clearingWithVendor = queryDto.ClearingWithVendor.Value;
            exp = exp.And(x => x.ClearingWithVendor == clearingWithVendor);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentTerms))
        {
            var paymentTerms = queryDto.PaymentTerms;
            exp = exp.And(x => x.PaymentTerms != null && x.PaymentTerms.Contains(paymentTerms));
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            var paymentMethod = queryDto.PaymentMethod.Value;
            exp = exp.And(x => x.PaymentMethod == paymentMethod);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeliveringPlant))
        {
            var deliveringPlant = queryDto.DeliveringPlant;
            exp = exp.And(x => x.DeliveringPlant != null && x.DeliveringPlant.Contains(deliveringPlant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Incoterms1))
        {
            var incoterms1 = queryDto.Incoterms1;
            exp = exp.And(x => x.Incoterms1 != null && x.Incoterms1.Contains(incoterms1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Incoterms2))
        {
            var incoterms2 = queryDto.Incoterms2;
            exp = exp.And(x => x.Incoterms2 != null && x.Incoterms2.Contains(incoterms2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShippingConditions))
        {
            var shippingConditions = queryDto.ShippingConditions;
            exp = exp.And(x => x.ShippingConditions != null && x.ShippingConditions.Contains(shippingConditions));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerPricingProcedure))
        {
            var customerPricingProcedure = queryDto.CustomerPricingProcedure;
            exp = exp.And(x => x.CustomerPricingProcedure != null && x.CustomerPricingProcedure.Contains(customerPricingProcedure));
        }

        if (queryDto?.SalesChannel.HasValue == true)
        {
            var salesChannel = queryDto.SalesChannel.Value;
            exp = exp.And(x => x.SalesChannel == salesChannel);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlatformName))
        {
            var platformName = queryDto.PlatformName;
            exp = exp.And(x => x.PlatformName != null && x.PlatformName.Contains(platformName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StoreName))
        {
            var storeName = queryDto.StoreName;
            exp = exp.And(x => x.StoreName != null && x.StoreName.Contains(storeName));
        }

        if (queryDto?.ClientLevel.HasValue == true)
        {
            var clientLevel = queryDto.ClientLevel.Value;
            exp = exp.And(x => x.ClientLevel == clientLevel);
        }

        if (queryDto?.EvaluationScore.HasValue == true)
        {
            var evaluationScore = queryDto.EvaluationScore.Value;
            exp = exp.And(x => x.EvaluationScore == evaluationScore);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.ClientStatus.HasValue == true)
        {
            var clientStatus = queryDto.ClientStatus.Value;
            exp = exp.And(x => x.ClientStatus == clientStatus);
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
    private static bool HasAnyListQueryFilter(TaktClientQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ClientCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientName2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientShortName))
        {
            return true;
        }
        if (queryDto.ClientType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EnterpriseNature))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IndustryAttribute))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientTaxNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (queryDto.TaxRate.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationCountry))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationProvince))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationCity))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationAddress1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationAddress2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientFax))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientEmail))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientWebsite))
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
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrganization))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DistributionChannel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TradingPartner))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountAssignmentGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NielsenIndicator))
        {
            return true;
        }
        if (queryDto.CentralPostingBlock.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReconciliationAccount))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Headquarters))
        {
            return true;
        }
        if (queryDto.ClearingWithVendor.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentTerms))
        {
            return true;
        }
        if (queryDto.PaymentMethod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeliveringPlant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Incoterms1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Incoterms2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShippingConditions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerPricingProcedure))
        {
            return true;
        }
        if (queryDto.SalesChannel.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlatformName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StoreName))
        {
            return true;
        }
        if (queryDto.ClientLevel.HasValue)
        {
            return true;
        }
        if (queryDto.EvaluationScore.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.ClientStatus.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
