// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktClientService.cs
// 创建时间：2026-10-14
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
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clientRepository">客户端信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktClientService(
        ITaktCompanyRepository<TaktClient> clientRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _clientRepository = clientRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
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
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.ClientCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.ClientCode))
        {
            throw new TaktBusinessException("客户端编码不能为空");
        }
        var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
            _clientRepository,
            x => x.SalesOrganization == entity.SalesOrganization
                && x.ClientCode == entity.ClientCode);
        if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
        {
            throw new TaktBusinessException("客户端信息的SalesOrganization、ClientCode已存在");
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
            x => x.SalesOrganization == entity.SalesOrganization
                && x.ClientCode == entity.ClientCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
        {
            throw new TaktBusinessException("客户端信息的SalesOrganization、ClientCode已存在");
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
                var importKey = $"{entity.SalesOrganization}|{entity.ClientCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesOrganization、ClientCode）");
                }
                var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _clientRepository,
                    x => x.SalesOrganization == entity.SalesOrganization
                        && x.ClientCode == entity.ClientCode);
                if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
                {
                    throw new TaktBusinessException("客户端信息的SalesOrganization、ClientCode已存在");
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
                || (x.AccountGroup != null && x.AccountGroup.Contains(keywords))
                || (x.SalesOrganization != null && x.SalesOrganization.Contains(keywords))
                || (x.DistributionChannel != null && x.DistributionChannel.Contains(keywords))
                || (x.ProductGroup != null && x.ProductGroup.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
                || (x.ClientName2 != null && x.ClientName2.Contains(keywords))
                || (x.ClientName3 != null && x.ClientName3.Contains(keywords))
                || (x.ClientName4 != null && x.ClientName4.Contains(keywords))
                || (x.ClientShortName != null && x.ClientShortName.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationRegion != null && x.RegistrationRegion.Contains(keywords))
                || (x.RegistrationCity != null && x.RegistrationCity.Contains(keywords))
                || (x.Addr1 != null && x.Addr1.Contains(keywords))
                || (x.Addr2 != null && x.Addr2.Contains(keywords))
                || (x.Addr3 != null && x.Addr3.Contains(keywords))
                || (x.Addr4 != null && x.Addr4.Contains(keywords))
                || (x.Addr5 != null && x.Addr5.Contains(keywords))
                || (x.PostalCode != null && x.PostalCode.Contains(keywords))
                || (x.Phone1 != null && x.Phone1.Contains(keywords))
                || (x.Phone2 != null && x.Phone2.Contains(keywords))
                || (x.FaxNumber != null && x.FaxNumber.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.TradingPartner != null && x.TradingPartner.Contains(keywords))
                || (x.TaxNumber1 != null && x.TaxNumber1.Contains(keywords))
                || (x.TaxNumber2 != null && x.TaxNumber2.Contains(keywords))
                || (x.TaxNumber3 != null && x.TaxNumber3.Contains(keywords))
                || (x.BankCountryCode != null && x.BankCountryCode.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankAccount != null && x.BankAccount.Contains(keywords))
                || (x.BankAccountHolder != null && x.BankAccountHolder.Contains(keywords))
                || (x.EnterpriseNature != null && x.EnterpriseNature.Contains(keywords))
                || (x.IndustryAttribute != null && x.IndustryAttribute.Contains(keywords))
                || (x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(keywords))
                || (x.PaymentTerms != null && x.PaymentTerms.Contains(keywords))
                || (x.SalesDistrict != null && x.SalesDistrict.Contains(keywords))
                || (x.SalesDepartment != null && x.SalesDepartment.Contains(keywords))
                || (x.SalesGroup != null && x.SalesGroup.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.CustomerPricingProcedure != null && x.CustomerPricingProcedure.Contains(keywords))
                || (x.ShippingConditions != null && x.ShippingConditions.Contains(keywords))
                || (x.DeliveringPlant != null && x.DeliveringPlant.Contains(keywords))
                || (x.Incoterms1 != null && x.Incoterms1.Contains(keywords))
                || (x.Incoterms2 != null && x.Incoterms2.Contains(keywords))
                || (x.AccountAssignmentGroup != null && x.AccountAssignmentGroup.Contains(keywords))
                || (x.TaxClassification != null && x.TaxClassification.Contains(keywords))
                || (x.ClientContact != null && x.ClientContact.Contains(keywords))
                || (x.ClientPhone != null && x.ClientPhone.Contains(keywords))
                || (x.ClientEmail != null && x.ClientEmail.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountGroup))
        {
            var accountGroup = queryDto.AccountGroup;
            exp = exp.And(x => x.AccountGroup != null && x.AccountGroup.Contains(accountGroup));
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

        if (queryDto?.ClientTitle.HasValue == true)
        {
            var clientTitle = queryDto.ClientTitle.Value;
            exp = exp.And(x => x.ClientTitle == clientTitle);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientName3))
        {
            var clientName3 = queryDto.ClientName3;
            exp = exp.And(x => x.ClientName3 != null && x.ClientName3.Contains(clientName3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientName4))
        {
            var clientName4 = queryDto.ClientName4;
            exp = exp.And(x => x.ClientName4 != null && x.ClientName4.Contains(clientName4));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientShortName))
        {
            var clientShortName = queryDto.ClientShortName;
            exp = exp.And(x => x.ClientShortName != null && x.ClientShortName.Contains(clientShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationCountry))
        {
            var registrationCountry = queryDto.RegistrationCountry;
            exp = exp.And(x => x.RegistrationCountry != null && x.RegistrationCountry.Contains(registrationCountry));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationRegion))
        {
            var registrationRegion = queryDto.RegistrationRegion;
            exp = exp.And(x => x.RegistrationRegion != null && x.RegistrationRegion.Contains(registrationRegion));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationCity))
        {
            var registrationCity = queryDto.RegistrationCity;
            exp = exp.And(x => x.RegistrationCity != null && x.RegistrationCity.Contains(registrationCity));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Addr1))
        {
            var addr1 = queryDto.Addr1;
            exp = exp.And(x => x.Addr1 != null && x.Addr1.Contains(addr1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Addr2))
        {
            var addr2 = queryDto.Addr2;
            exp = exp.And(x => x.Addr2 != null && x.Addr2.Contains(addr2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Addr3))
        {
            var addr3 = queryDto.Addr3;
            exp = exp.And(x => x.Addr3 != null && x.Addr3.Contains(addr3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Addr4))
        {
            var addr4 = queryDto.Addr4;
            exp = exp.And(x => x.Addr4 != null && x.Addr4.Contains(addr4));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Addr5))
        {
            var addr5 = queryDto.Addr5;
            exp = exp.And(x => x.Addr5 != null && x.Addr5.Contains(addr5));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostalCode))
        {
            var postalCode = queryDto.PostalCode;
            exp = exp.And(x => x.PostalCode != null && x.PostalCode.Contains(postalCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Phone1))
        {
            var phone1 = queryDto.Phone1;
            exp = exp.And(x => x.Phone1 != null && x.Phone1.Contains(phone1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Phone2))
        {
            var phone2 = queryDto.Phone2;
            exp = exp.And(x => x.Phone2 != null && x.Phone2.Contains(phone2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FaxNumber))
        {
            var faxNumber = queryDto.FaxNumber;
            exp = exp.And(x => x.FaxNumber != null && x.FaxNumber.Contains(faxNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TradingPartner))
        {
            var tradingPartner = queryDto.TradingPartner;
            exp = exp.And(x => x.TradingPartner != null && x.TradingPartner.Contains(tradingPartner));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxNumber1))
        {
            var taxNumber1 = queryDto.TaxNumber1;
            exp = exp.And(x => x.TaxNumber1 != null && x.TaxNumber1.Contains(taxNumber1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxNumber2))
        {
            var taxNumber2 = queryDto.TaxNumber2;
            exp = exp.And(x => x.TaxNumber2 != null && x.TaxNumber2.Contains(taxNumber2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxNumber3))
        {
            var taxNumber3 = queryDto.TaxNumber3;
            exp = exp.And(x => x.TaxNumber3 != null && x.TaxNumber3.Contains(taxNumber3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankCountryCode))
        {
            var bankCountryCode = queryDto.BankCountryCode;
            exp = exp.And(x => x.BankCountryCode != null && x.BankCountryCode.Contains(bankCountryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankCode))
        {
            var bankCode = queryDto.BankCode;
            exp = exp.And(x => x.BankCode != null && x.BankCode.Contains(bankCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankAccount))
        {
            var bankAccount = queryDto.BankAccount;
            exp = exp.And(x => x.BankAccount != null && x.BankAccount.Contains(bankAccount));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankAccountHolder))
        {
            var bankAccountHolder = queryDto.BankAccountHolder;
            exp = exp.And(x => x.BankAccountHolder != null && x.BankAccountHolder.Contains(bankAccountHolder));
        }

        if (queryDto?.ClientCategory.HasValue == true)
        {
            var clientCategory = queryDto.ClientCategory.Value;
            exp = exp.And(x => x.ClientCategory == clientCategory);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ReconciliationAccount))
        {
            var reconciliationAccount = queryDto.ReconciliationAccount;
            exp = exp.And(x => x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(reconciliationAccount));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesDistrict))
        {
            var salesDistrict = queryDto.SalesDistrict;
            exp = exp.And(x => x.SalesDistrict != null && x.SalesDistrict.Contains(salesDistrict));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesDepartment))
        {
            var salesDepartment = queryDto.SalesDepartment;
            exp = exp.And(x => x.SalesDepartment != null && x.SalesDepartment.Contains(salesDepartment));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroup))
        {
            var salesGroup = queryDto.SalesGroup;
            exp = exp.And(x => x.SalesGroup != null && x.SalesGroup.Contains(salesGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerPricingProcedure))
        {
            var customerPricingProcedure = queryDto.CustomerPricingProcedure;
            exp = exp.And(x => x.CustomerPricingProcedure != null && x.CustomerPricingProcedure.Contains(customerPricingProcedure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShippingConditions))
        {
            var shippingConditions = queryDto.ShippingConditions;
            exp = exp.And(x => x.ShippingConditions != null && x.ShippingConditions.Contains(shippingConditions));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountAssignmentGroup))
        {
            var accountAssignmentGroup = queryDto.AccountAssignmentGroup;
            exp = exp.And(x => x.AccountAssignmentGroup != null && x.AccountAssignmentGroup.Contains(accountAssignmentGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxClassification))
        {
            var taxClassification = queryDto.TaxClassification;
            exp = exp.And(x => x.TaxClassification != null && x.TaxClassification.Contains(taxClassification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientContact))
        {
            var clientContact = queryDto.ClientContact;
            exp = exp.And(x => x.ClientContact != null && x.ClientContact.Contains(clientContact));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientPhone))
        {
            var clientPhone = queryDto.ClientPhone;
            exp = exp.And(x => x.ClientPhone != null && x.ClientPhone.Contains(clientPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientEmail))
        {
            var clientEmail = queryDto.ClientEmail;
            exp = exp.And(x => x.ClientEmail != null && x.ClientEmail.Contains(clientEmail));
        }

        if (queryDto?.CreditLevel.HasValue == true)
        {
            var creditLevel = queryDto.CreditLevel.Value;
            exp = exp.And(x => x.CreditLevel == creditLevel);
        }

        if (queryDto?.ClientLevel.HasValue == true)
        {
            var clientLevel = queryDto.ClientLevel.Value;
            exp = exp.And(x => x.ClientLevel == clientLevel);
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
        if (!string.IsNullOrWhiteSpace(queryDto.AccountGroup))
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
        if (queryDto.ClientTitle.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ClientName3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientName4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationCountry))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationRegion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationCity))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Addr1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Addr2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Addr3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Addr4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Addr5))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostalCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Phone1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Phone2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FaxNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TradingPartner))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxNumber1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxNumber2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxNumber3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankCountryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankAccount))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankAccountHolder))
        {
            return true;
        }
        if (queryDto.ClientCategory.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ReconciliationAccount))
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesDistrict))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesDepartment))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerPricingProcedure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShippingConditions))
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
        if (!string.IsNullOrWhiteSpace(queryDto.AccountAssignmentGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxClassification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientContact))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientEmail))
        {
            return true;
        }
        if (queryDto.CreditLevel.HasValue)
        {
            return true;
        }
        if (queryDto.ClientLevel.HasValue)
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
