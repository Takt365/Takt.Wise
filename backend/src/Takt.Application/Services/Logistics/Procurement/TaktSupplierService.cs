// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktSupplierService.cs
// 创建时间：2026-08-23
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

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierRepository">供货商信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSupplierService(
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _supplierRepository = supplierRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
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
        var list = await _supplierRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SupplierStatus == 1,
            x => x.SupplierShortName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SupplierCode,
            DictLabel = e.SupplierShortName ?? e.SupplierCode,
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
        var isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierRepository,
            x => x.SupplierCode == entity.SupplierCode);
        if (!isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique)
        {
            throw new TaktBusinessException("供货商信息的SupplierCode已存在");
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
            x => x.SupplierCode == entity.SupplierCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique)
        {
            throw new TaktBusinessException("供货商信息的SupplierCode已存在");
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
                var importKey = $"{entity.SupplierCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SupplierCode）");
                }
                var isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _supplierRepository,
                    x => x.SupplierCode == entity.SupplierCode);
                if (!isUnique_ix_takt_logistics_procurement_supplier_supplier_code_unique)
                {
                    throw new TaktBusinessException("供货商信息的SupplierCode已存在");
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
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.SupplierName2 != null && x.SupplierName2.Contains(keywords))
                || (x.SupplierShortName != null && x.SupplierShortName.Contains(keywords))
                || (x.EnterpriseNature != null && x.EnterpriseNature.Contains(keywords))
                || (x.IndustryAttribute != null && x.IndustryAttribute.Contains(keywords))
                || (x.SupplierTaxNumber != null && x.SupplierTaxNumber.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationProvince != null && x.RegistrationProvince.Contains(keywords))
                || (x.RegistrationCity != null && x.RegistrationCity.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.SupplierPhone != null && x.SupplierPhone.Contains(keywords))
                || (x.SupplierFax != null && x.SupplierFax.Contains(keywords))
                || (x.SupplierEmail != null && x.SupplierEmail.Contains(keywords))
                || (x.SupplierWebsite != null && x.SupplierWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.PaymentTerms != null && x.PaymentTerms.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankAccount != null && x.BankAccount.Contains(keywords))
                || (x.AccountHolder != null && x.AccountHolder.Contains(keywords))
                || (x.Incoterms1 != null && x.Incoterms1.Contains(keywords))
                || (x.Incoterms2 != null && x.Incoterms2.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierShortName))
        {
            var supplierShortName = queryDto.SupplierShortName;
            exp = exp.And(x => x.SupplierShortName != null && x.SupplierShortName.Contains(supplierShortName));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierTaxNumber))
        {
            var supplierTaxNumber = queryDto.SupplierTaxNumber;
            exp = exp.And(x => x.SupplierTaxNumber != null && x.SupplierTaxNumber.Contains(supplierTaxNumber));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierPhone))
        {
            var supplierPhone = queryDto.SupplierPhone;
            exp = exp.And(x => x.SupplierPhone != null && x.SupplierPhone.Contains(supplierPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierFax))
        {
            var supplierFax = queryDto.SupplierFax;
            exp = exp.And(x => x.SupplierFax != null && x.SupplierFax.Contains(supplierFax));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierEmail))
        {
            var supplierEmail = queryDto.SupplierEmail;
            exp = exp.And(x => x.SupplierEmail != null && x.SupplierEmail.Contains(supplierEmail));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierWebsite))
        {
            var supplierWebsite = queryDto.SupplierWebsite;
            exp = exp.And(x => x.SupplierWebsite != null && x.SupplierWebsite.Contains(supplierWebsite));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ReconciliationAccount))
        {
            var reconciliationAccount = queryDto.ReconciliationAccount;
            exp = exp.And(x => x.ReconciliationAccount != null && x.ReconciliationAccount.Contains(reconciliationAccount));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (queryDto?.ClearingWithCustomer.HasValue == true)
        {
            var clearingWithCustomer = queryDto.ClearingWithCustomer.Value;
            exp = exp.And(x => x.ClearingWithCustomer == clearingWithCustomer);
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            var paymentMethod = queryDto.PaymentMethod.Value;
            exp = exp.And(x => x.PaymentMethod == paymentMethod);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentTerms))
        {
            var paymentTerms = queryDto.PaymentTerms;
            exp = exp.And(x => x.PaymentTerms != null && x.PaymentTerms.Contains(paymentTerms));
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

        if (queryDto?.GrBasedInvoiceInspection.HasValue == true)
        {
            var grBasedInvoiceInspection = queryDto.GrBasedInvoiceInspection.Value;
            exp = exp.And(x => x.GrBasedInvoiceInspection == grBasedInvoiceInspection);
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

        if (queryDto?.AutomaticPurchaseOrder.HasValue == true)
        {
            var automaticPurchaseOrder = queryDto.AutomaticPurchaseOrder.Value;
            exp = exp.And(x => x.AutomaticPurchaseOrder == automaticPurchaseOrder);
        }

        if (queryDto?.PricingDateControl.HasValue == true)
        {
            var pricingDateControl = queryDto.PricingDateControl.Value;
            exp = exp.And(x => x.PricingDateControl == pricingDateControl);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroup))
        {
            var purchaseGroup = queryDto.PurchaseGroup;
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(purchaseGroup));
        }

        if (queryDto?.PlannedDeliveryTimeDays.HasValue == true)
        {
            var plannedDeliveryTimeDays = queryDto.PlannedDeliveryTimeDays.Value;
            exp = exp.And(x => x.PlannedDeliveryTimeDays == plannedDeliveryTimeDays);
        }

        if (queryDto?.EvaluatedReceiptSettlement.HasValue == true)
        {
            var evaluatedReceiptSettlement = queryDto.EvaluatedReceiptSettlement.Value;
            exp = exp.And(x => x.EvaluatedReceiptSettlement == evaluatedReceiptSettlement);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasingOrganization))
        {
            var purchasingOrganization = queryDto.PurchasingOrganization;
            exp = exp.And(x => x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(purchasingOrganization));
        }

        if (queryDto?.SupplierLevel.HasValue == true)
        {
            var supplierLevel = queryDto.SupplierLevel.Value;
            exp = exp.And(x => x.SupplierLevel == supplierLevel);
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
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierShortName))
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
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierTaxNumber))
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
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierFax))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierEmail))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierWebsite))
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
        if (!string.IsNullOrWhiteSpace(queryDto.ReconciliationAccount))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (queryDto.ClearingWithCustomer.HasValue)
        {
            return true;
        }
        if (queryDto.PaymentMethod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentTerms))
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
        if (queryDto.GrBasedInvoiceInspection.HasValue)
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
        if (queryDto.AutomaticPurchaseOrder.HasValue)
        {
            return true;
        }
        if (queryDto.PricingDateControl.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
        {
            return true;
        }
        if (queryDto.PlannedDeliveryTimeDays.HasValue)
        {
            return true;
        }
        if (queryDto.EvaluatedReceiptSettlement.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasingOrganization))
        {
            return true;
        }
        if (queryDto.SupplierLevel.HasValue)
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
