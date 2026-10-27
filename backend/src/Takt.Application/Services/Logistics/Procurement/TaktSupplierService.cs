// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktSupplierService.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：供货商信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 供货商信息应用服务
/// </summary>
public class TaktSupplierService : TaktServiceBase, ITaktSupplierService
{
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierRepository">供货商信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSupplierService(
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _supplierRepository = supplierRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取供货商信息列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSupplierDto>> GetSupplierListAsync(TaktSupplierQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSupplierDto>.Create(
                new List<TaktSupplierDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _supplierRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSupplierDto>.Create(
            data.Adapt<List<TaktSupplierDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto?> GetSupplierByIdAsync(long id)
    {
        var entity = await _supplierRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSupplierDto>();
    }

    /// <summary>
    /// 获取供货商信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSupplierOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktSupplier>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SupplierStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.SupplierCode != null && x.SupplierCode.Contains(normalizedKeyword!))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _supplierRepository.GetListAsync(
            predicate,
            x => x.SupplierName1 ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SupplierCode,
            DictLabel = string.IsNullOrWhiteSpace(e.SupplierName1) ? e.SupplierCode : e.SupplierName1,
        }).ToList();
    }

    /// <summary>
    /// 创建供货商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> CreateSupplierAsync(TaktSupplierCreateDto dto)
    {
        var entity = dto.Adapt<TaktSupplier>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var existingRows = await _supplierRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
            var generated = await _numberingGenerator.GenerateNextAsync(
                dto.NumberingRuleCode.Trim(),
                existingRows.Select(x => (string?)x.SupplierCode).ToList());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.SupplierCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.SupplierCode))
        {
            throw new TaktBusinessException("供应商不能为空");
        }
        var isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierRepository,
            x => x.PurchasingOrganization == entity.PurchasingOrganization
                && x.SupplierCode == entity.SupplierCode);
        if (!isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique)
        {
            throw new TaktBusinessException("供货商信息的PurchasingOrganization、SupplierCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _supplierRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _supplierRepository.CreateAsync(entity);
        return await GetSupplierByIdAsync(entity.Id) ?? entity.Adapt<TaktSupplierDto>();
    }

    /// <summary>
    /// 更新供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> UpdateSupplierAsync(long id, TaktSupplierUpdateDto dto)
    {
        var entity = await _supplierRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供货商信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierRepository,
            x => x.PurchasingOrganization == entity.PurchasingOrganization
                && x.SupplierCode == entity.SupplierCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique)
        {
            throw new TaktBusinessException("供货商信息的PurchasingOrganization、SupplierCode已存在");
        }
        await _supplierRepository.UpdateAsync(entity);
        return await GetSupplierByIdAsync(id) ?? throw new TaktBusinessException("供货商信息不存在");
    }

    /// <summary>
    /// 删除供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierByIdAsync(long id)
    {
        var deleted = await _supplierRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("供货商信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除供货商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSupplierByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新供货商信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> UpdateSupplierStatusAsync(TaktSupplierStatusDto dto)
    {
        var entity = await _supplierRepository.GetByIdAsync(dto.SupplierId);
        if (entity == null)
        {
            throw new TaktBusinessException("供货商信息不存在");
        }
        entity.SupplierStatus = dto.SupplierStatus;
        await _supplierRepository.UpdateAsync(entity);
        return await GetSupplierByIdAsync(dto.SupplierId) ?? throw new TaktBusinessException("供货商信息不存在");
    }

    /// <summary>
    /// 更新供货商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierDto> UpdateSupplierSortAsync(TaktSupplierSortDto dto)
    {
        var entity = await _supplierRepository.GetByIdAsync(dto.SupplierId);
        if (entity == null)
        {
            throw new TaktBusinessException("供货商信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _supplierRepository.UpdateAsync(entity);
        return await GetSupplierByIdAsync(dto.SupplierId) ?? throw new TaktBusinessException("供货商信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSupplierTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSupplierTemplateDto>(
            sheetName ?? "供货商信息导入模板",
            fileName ?? "供货商信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入供货商信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSupplierAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSupplierImportDto>(fileStream, sheetName ?? "供货商信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _supplierRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSupplier>();
                var importKey = $"{entity.PurchasingOrganization}|{entity.SupplierCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchasingOrganization、SupplierCode）");
                }
                var isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _supplierRepository,
                    x => x.PurchasingOrganization == entity.PurchasingOrganization
                        && x.SupplierCode == entity.SupplierCode);
                if (!isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique)
                {
                    throw new TaktBusinessException("供货商信息的PurchasingOrganization、SupplierCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _supplierRepository.CreateAsync(entity);
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
    /// 导出供货商信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSupplierAsync(TaktSupplierQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSupplierQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierExportDto>(),
                sheetName ?? "供货商信息数据",
                fileName ?? "供货商信息导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _supplierRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierExportDto>(),
                sheetName ?? "供货商信息数据",
                fileName ?? "供货商信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSupplierExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "供货商信息数据",
            fileName ?? "供货商信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建供货商信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSupplier, bool>> QueryExpression(TaktSupplierQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSupplier>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.AccountGroup != null && x.AccountGroup.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.SupplierName2 != null && x.SupplierName2.Contains(keywords))
                || (x.SupplierName3 != null && x.SupplierName3.Contains(keywords))
                || (x.SupplierName4 != null && x.SupplierName4.Contains(keywords))
                || (x.SupplierShortName != null && x.SupplierShortName.Contains(keywords))
                || (x.CountryCode != null && x.CountryCode.Contains(keywords))
                || (x.RegionCode != null && x.RegionCode.Contains(keywords))
                || (x.CityCode != null && x.CityCode.Contains(keywords))
                || (x.Addr1 != null && x.Addr1.Contains(keywords))
                || (x.Addr2 != null && x.Addr2.Contains(keywords))
                || (x.PostalCode != null && x.PostalCode.Contains(keywords))
                || (x.Phone1 != null && x.Phone1.Contains(keywords))
                || (x.Phone2 != null && x.Phone2.Contains(keywords))
                || (x.FaxNumber != null && x.FaxNumber.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.TradingPartner != null && x.TradingPartner.Contains(keywords))
                || (x.AuthorizationGroup != null && x.AuthorizationGroup.Contains(keywords))
                || (x.GroupKey != null && x.GroupKey.Contains(keywords))
                || (x.EnterpriseNature != null && x.EnterpriseNature.Contains(keywords))
                || (x.IndustryAttribute != null && x.IndustryAttribute.Contains(keywords))
                || (x.CreditInfoNumber != null && x.CreditInfoNumber.Contains(keywords))
                || (x.BankCountryCode != null && x.BankCountryCode.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankAccount != null && x.BankAccount.Contains(keywords))
                || (x.AccountHolder != null && x.AccountHolder.Contains(keywords))
                || (x.BankChargeBearerCode != null && x.BankChargeBearerCode.Contains(keywords))
                || (x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(keywords))
                || (x.HeadOfficeAccountCode != null && x.HeadOfficeAccountCode.Contains(keywords))
                || (x.PlanningGroup != null && x.PlanningGroup.Contains(keywords))
                || (x.PaymentTerms != null && x.PaymentTerms.Contains(keywords))
                || (x.HouseBank != null && x.HouseBank.Contains(keywords))
                || (x.AccountingClerk != null && x.AccountingClerk.Contains(keywords))
                || (x.OurAccountAtSupplier != null && x.OurAccountAtSupplier.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.WithholdingTaxCountry != null && x.WithholdingTaxCountry.Contains(keywords))
                || (x.WithholdingTaxType != null && x.WithholdingTaxType.Contains(keywords))
                || (x.WithholdingTaxCode != null && x.WithholdingTaxCode.Contains(keywords))
                || (x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(keywords))
                || (x.OrderCurrencyCode != null && x.OrderCurrencyCode.Contains(keywords))
                || (x.Incoterms1 != null && x.Incoterms1.Contains(keywords))
                || (x.Incoterms2 != null && x.Incoterms2.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.TaxNumber1 != null && x.TaxNumber1.Contains(keywords))
                || (x.TaxNumber2 != null && x.TaxNumber2.Contains(keywords))
                || (x.TaxNumber3 != null && x.TaxNumber3.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountGroup))
        {
            var accountGroup = queryDto.AccountGroup;
            exp = exp.And(x => x.AccountGroup != null && x.AccountGroup.Contains(accountGroup));
        }

        if (queryDto?.SupplierTitle.HasValue == true)
        {
            var supplierTitle = queryDto.SupplierTitle.Value;
            exp = exp.And(x => x.SupplierTitle == supplierTitle);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName1))
        {
            var supplierName1 = queryDto.SupplierName1;
            exp = exp.And(x => x.SupplierName1 != null && x.SupplierName1.Contains(supplierName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName2))
        {
            var supplierName2 = queryDto.SupplierName2;
            exp = exp.And(x => x.SupplierName2 != null && x.SupplierName2.Contains(supplierName2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName3))
        {
            var supplierName3 = queryDto.SupplierName3;
            exp = exp.And(x => x.SupplierName3 != null && x.SupplierName3.Contains(supplierName3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName4))
        {
            var supplierName4 = queryDto.SupplierName4;
            exp = exp.And(x => x.SupplierName4 != null && x.SupplierName4.Contains(supplierName4));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierShortName))
        {
            var supplierShortName = queryDto.SupplierShortName;
            exp = exp.And(x => x.SupplierShortName != null && x.SupplierShortName.Contains(supplierShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountryCode))
        {
            var countryCode = queryDto.CountryCode;
            exp = exp.And(x => x.CountryCode != null && x.CountryCode.Contains(countryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegionCode))
        {
            var regionCode = queryDto.RegionCode;
            exp = exp.And(x => x.RegionCode != null && x.RegionCode.Contains(regionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CityCode))
        {
            var cityCode = queryDto.CityCode;
            exp = exp.And(x => x.CityCode != null && x.CityCode.Contains(cityCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TradingPartner))
        {
            var tradingPartner = queryDto.TradingPartner;
            exp = exp.And(x => x.TradingPartner != null && x.TradingPartner.Contains(tradingPartner));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AuthorizationGroup))
        {
            var authorizationGroup = queryDto.AuthorizationGroup;
            exp = exp.And(x => x.AuthorizationGroup != null && x.AuthorizationGroup.Contains(authorizationGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GroupKey))
        {
            var groupKey = queryDto.GroupKey;
            exp = exp.And(x => x.GroupKey != null && x.GroupKey.Contains(groupKey));
        }

        if (queryDto?.SupplierType.HasValue == true)
        {
            var supplierType = queryDto.SupplierType.Value;
            exp = exp.And(x => x.SupplierType == supplierType);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CreditInfoNumber))
        {
            var creditInfoNumber = queryDto.CreditInfoNumber;
            exp = exp.And(x => x.CreditInfoNumber != null && x.CreditInfoNumber.Contains(creditInfoNumber));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountHolder))
        {
            var accountHolder = queryDto.AccountHolder;
            exp = exp.And(x => x.AccountHolder != null && x.AccountHolder.Contains(accountHolder));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BankChargeBearerCode))
        {
            var bankChargeBearerCode = queryDto.BankChargeBearerCode;
            exp = exp.And(x => x.BankChargeBearerCode != null && x.BankChargeBearerCode.Contains(bankChargeBearerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReconciliationAccount))
        {
            var reconciliationAccount = queryDto.ReconciliationAccount;
            exp = exp.And(x => x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(reconciliationAccount));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HeadOfficeAccountCode))
        {
            var headOfficeAccountCode = queryDto.HeadOfficeAccountCode;
            exp = exp.And(x => x.HeadOfficeAccountCode != null && x.HeadOfficeAccountCode.Contains(headOfficeAccountCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanningGroup))
        {
            var planningGroup = queryDto.PlanningGroup;
            exp = exp.And(x => x.PlanningGroup != null && x.PlanningGroup.Contains(planningGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentTerms))
        {
            var paymentTerms = queryDto.PaymentTerms;
            exp = exp.And(x => x.PaymentTerms != null && x.PaymentTerms.Contains(paymentTerms));
        }

        if (queryDto?.DoubleInvoiceCheck.HasValue == true)
        {
            var doubleInvoiceCheck = queryDto.DoubleInvoiceCheck.Value;
            exp = exp.And(x => x.DoubleInvoiceCheck == doubleInvoiceCheck);
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            var paymentMethod = queryDto.PaymentMethod.Value;
            exp = exp.And(x => x.PaymentMethod == paymentMethod);
        }

        if (queryDto?.PaymentBlock.HasValue == true)
        {
            var paymentBlock = queryDto.PaymentBlock.Value;
            exp = exp.And(x => x.PaymentBlock == paymentBlock);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HouseBank))
        {
            var houseBank = queryDto.HouseBank;
            exp = exp.And(x => x.HouseBank != null && x.HouseBank.Contains(houseBank));
        }

        if (queryDto?.ClearingWithCustomer.HasValue == true)
        {
            var clearingWithCustomer = queryDto.ClearingWithCustomer.Value;
            exp = exp.And(x => x.ClearingWithCustomer == clearingWithCustomer);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountingClerk))
        {
            var accountingClerk = queryDto.AccountingClerk;
            exp = exp.And(x => x.AccountingClerk != null && x.AccountingClerk.Contains(accountingClerk));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OurAccountAtSupplier))
        {
            var ourAccountAtSupplier = queryDto.OurAccountAtSupplier;
            exp = exp.And(x => x.OurAccountAtSupplier != null && x.OurAccountAtSupplier.Contains(ourAccountAtSupplier));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.WithholdingTaxCountry))
        {
            var withholdingTaxCountry = queryDto.WithholdingTaxCountry;
            exp = exp.And(x => x.WithholdingTaxCountry != null && x.WithholdingTaxCountry.Contains(withholdingTaxCountry));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WithholdingTaxType))
        {
            var withholdingTaxType = queryDto.WithholdingTaxType;
            exp = exp.And(x => x.WithholdingTaxType != null && x.WithholdingTaxType.Contains(withholdingTaxType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WithholdingTaxCode))
        {
            var withholdingTaxCode = queryDto.WithholdingTaxCode;
            exp = exp.And(x => x.WithholdingTaxCode != null && x.WithholdingTaxCode.Contains(withholdingTaxCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasingOrganization))
        {
            var purchasingOrganization = queryDto.PurchasingOrganization;
            exp = exp.And(x => x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(purchasingOrganization));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrderCurrencyCode))
        {
            var orderCurrencyCode = queryDto.OrderCurrencyCode;
            exp = exp.And(x => x.OrderCurrencyCode != null && x.OrderCurrencyCode.Contains(orderCurrencyCode));
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

        if (queryDto?.PricingDateControl.HasValue == true)
        {
            var pricingDateControl = queryDto.PricingDateControl.Value;
            exp = exp.And(x => x.PricingDateControl == pricingDateControl);
        }

        if (queryDto?.GrBasedInvoiceInspection.HasValue == true)
        {
            var grBasedInvoiceInspection = queryDto.GrBasedInvoiceInspection.Value;
            exp = exp.And(x => x.GrBasedInvoiceInspection == grBasedInvoiceInspection);
        }

        if (queryDto?.EvaluatedReceiptSettlement.HasValue == true)
        {
            var evaluatedReceiptSettlement = queryDto.EvaluatedReceiptSettlement.Value;
            exp = exp.And(x => x.EvaluatedReceiptSettlement == evaluatedReceiptSettlement);
        }

        if (queryDto?.ReturnErsFlag.HasValue == true)
        {
            var returnErsFlag = queryDto.ReturnErsFlag.Value;
            exp = exp.And(x => x.ReturnErsFlag == returnErsFlag);
        }

        if (queryDto?.AutomaticPurchaseOrder.HasValue == true)
        {
            var automaticPurchaseOrder = queryDto.AutomaticPurchaseOrder.Value;
            exp = exp.And(x => x.AutomaticPurchaseOrder == automaticPurchaseOrder);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroup))
        {
            var purchaseGroup = queryDto.PurchaseGroup;
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(purchaseGroup));
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

        if (queryDto?.CreditLevel.HasValue == true)
        {
            var creditLevel = queryDto.CreditLevel.Value;
            exp = exp.And(x => x.CreditLevel == creditLevel);
        }

        if (queryDto?.SupplierLevel.HasValue == true)
        {
            var supplierLevel = queryDto.SupplierLevel.Value;
            exp = exp.And(x => x.SupplierLevel == supplierLevel);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.SupplierStatus.HasValue == true)
        {
            var supplierStatus = queryDto.SupplierStatus.Value;
            exp = exp.And(x => x.SupplierStatus == supplierStatus);
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
    private static bool HasAnyListQueryFilter(TaktSupplierQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountGroup))
        {
            return true;
        }
        if (queryDto.SupplierTitle.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CityCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TradingPartner))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AuthorizationGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GroupKey))
        {
            return true;
        }
        if (queryDto.SupplierType.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CreditInfoNumber))
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
        if (!string.IsNullOrWhiteSpace(queryDto.AccountHolder))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BankChargeBearerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReconciliationAccount))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HeadOfficeAccountCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanningGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentTerms))
        {
            return true;
        }
        if (queryDto.DoubleInvoiceCheck.HasValue)
        {
            return true;
        }
        if (queryDto.PaymentMethod.HasValue)
        {
            return true;
        }
        if (queryDto.PaymentBlock.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HouseBank))
        {
            return true;
        }
        if (queryDto.ClearingWithCustomer.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountingClerk))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OurAccountAtSupplier))
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
        if (!string.IsNullOrWhiteSpace(queryDto.WithholdingTaxCountry))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WithholdingTaxType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WithholdingTaxCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasingOrganization))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrderCurrencyCode))
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
        if (queryDto.PricingDateControl.HasValue)
        {
            return true;
        }
        if (queryDto.GrBasedInvoiceInspection.HasValue)
        {
            return true;
        }
        if (queryDto.EvaluatedReceiptSettlement.HasValue)
        {
            return true;
        }
        if (queryDto.ReturnErsFlag.HasValue)
        {
            return true;
        }
        if (queryDto.AutomaticPurchaseOrder.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
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
        if (queryDto.CreditLevel.HasValue)
        {
            return true;
        }
        if (queryDto.SupplierLevel.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.SupplierStatus.HasValue)
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
