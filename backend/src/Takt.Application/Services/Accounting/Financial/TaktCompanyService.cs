// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktCompanyService.cs
// 创建时间：2026-08-15
// 创建人：Takt365(Cursor AI)
// 功能描述：公司应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Application.Services.Identity;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 公司应用服务
/// </summary>
public class TaktCompanyService : TaktServiceBase, ITaktCompanyService
{
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCompanyService(
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktRbacService rbacService,

        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _companyRepository = companyRepository;
        _rbacService = rbacService;

        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取公司列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCompanyDto>> GetCompanyListAsync(TaktCompanyQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCompanyDto>.Create(
                new List<TaktCompanyDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _companyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCompanyDto>.Create(
            data.Adapt<List<TaktCompanyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto?> GetCompanyByIdAsync(long id)
    {
        var entity = await _companyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCompanyDto>();
        return dto;    }

    /// <summary>
    /// 获取公司选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCompanyOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyStatus == 1,
            x => x.CompanyShortName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CompanyCode,
            DictLabel = e.CompanyShortName ?? e.CompanyCode,
        }).ToList();
    }

    /// <summary>
    /// 创建公司
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> CreateCompanyAsync(TaktCompanyCreateDto dto)
    {
        var entity = dto.Adapt<TaktCompany>();
        var isUnique_ix_company_code_unique = await _uniqueValidator.IsUniqueAsync(
            _companyRepository,
            x => x.CompanyCode == entity.CompanyCode);
        if (!isUnique_ix_company_code_unique)
        {
            throw new TaktBusinessException("公司的CompanyCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _companyRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _companyRepository.CreateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleCompanyIdsAsync(roleId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignRoleCompaniesAsync(roleId, codes.ToArray());
            }
        }
        if (dto.UserIds != null)
        {
            foreach (var userId in dto.UserIds.Distinct())
            {
                var links = await _rbacService.GetUserCompanyIdsAsync(userId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignUserCompaniesAsync(userId, codes.ToArray());
            }
        }
        return await GetCompanyByIdAsync(entity.Id) ?? entity.Adapt<TaktCompanyDto>();
    }

    /// <summary>
    /// 更新公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> UpdateCompanyAsync(long id, TaktCompanyUpdateDto dto)
    {
        var entity = await _companyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("公司不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_company_code_unique = await _uniqueValidator.IsUniqueAsync(
            _companyRepository,
            x => x.CompanyCode == entity.CompanyCode,
            id);
        if (!isUnique_ix_company_code_unique)
        {
            throw new TaktBusinessException("公司的CompanyCode已存在");
        }
        await _companyRepository.UpdateAsync(entity);
        if (dto.RoleIds != null)
        {
            foreach (var roleId in dto.RoleIds.Distinct())
            {
                var links = await _rbacService.GetRoleCompanyIdsAsync(roleId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignRoleCompaniesAsync(roleId, codes.ToArray());
            }
        }
        if (dto.UserIds != null)
        {
            foreach (var userId in dto.UserIds.Distinct())
            {
                var links = await _rbacService.GetUserCompanyIdsAsync(userId);
                var codes = links.Select(x => x.CompanyCode).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
                if (!codes.Contains(entity.CompanyCode))
                {
                    codes.Add(entity.CompanyCode);
                }
                await _rbacService.AssignUserCompaniesAsync(userId, codes.ToArray());
            }
        }
        return await GetCompanyByIdAsync(id) ?? throw new TaktBusinessException("公司不存在");
    }

    /// <summary>
    /// 删除公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCompanyByIdAsync(long id)
    {
        var deleted = await _companyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("公司不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除公司
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCompanyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCompanyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新公司状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> UpdateCompanyStatusAsync(TaktCompanyStatusDto dto)
    {
        var entity = await _companyRepository.GetByIdAsync(dto.CompanyId);
        if (entity == null)
        {
            throw new TaktBusinessException("公司不存在");
        }
        entity.CompanyStatus = dto.CompanyStatus;
        await _companyRepository.UpdateAsync(entity);
        return await GetCompanyByIdAsync(dto.CompanyId) ?? throw new TaktBusinessException("公司不存在");
    }

    /// <summary>
    /// 更新公司排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCompanyDto> UpdateCompanySortAsync(TaktCompanySortDto dto)
    {
        var entity = await _companyRepository.GetByIdAsync(dto.CompanyId);
        if (entity == null)
        {
            throw new TaktBusinessException("公司不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _companyRepository.UpdateAsync(entity);
        return await GetCompanyByIdAsync(dto.CompanyId) ?? throw new TaktBusinessException("公司不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCompanyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCompanyTemplateDto>(
            sheetName ?? "公司导入模板",
            fileName ?? "公司导入模板.xlsx");
    }

    /// <summary>
    /// 导入公司
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCompanyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCompanyImportDto>(fileStream, sheetName ?? "公司导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _companyRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCompany>();
                var importKey = $"{entity.CompanyCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CompanyCode）");
                }
                var isUnique_ix_company_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _companyRepository,
                    x => x.CompanyCode == entity.CompanyCode);
                if (!isUnique_ix_company_code_unique)
                {
                    throw new TaktBusinessException("公司的CompanyCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _companyRepository.CreateAsync(entity);
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
    /// 导出公司
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCompanyAsync(TaktCompanyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCompanyQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCompanyExportDto>(),
                sheetName ?? "公司数据",
                fileName ?? "公司导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _companyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCompanyExportDto>(),
                sheetName ?? "公司数据",
                fileName ?? "公司导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCompanyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "公司数据",
            fileName ?? "公司导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建公司查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCompany, bool>> QueryExpression(TaktCompanyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCompany>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.CompanyName1 != null && x.CompanyName1.Contains(keywords))
                || (x.CompanyName2 != null && x.CompanyName2.Contains(keywords))
                || (x.CompanyShortName != null && x.CompanyShortName.Contains(keywords))
                || (x.EnterpriseNature != null && x.EnterpriseNature.Contains(keywords))
                || (x.IndustryAttribute != null && x.IndustryAttribute.Contains(keywords))
                || (x.EnterpriseScale != null && x.EnterpriseScale.Contains(keywords))
                || (x.BusinessScope != null && x.BusinessScope.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationRegion != null && x.RegistrationRegion.Contains(keywords))
                || (x.RegistrationProvince != null && x.RegistrationProvince.Contains(keywords))
                || (x.RegistrationCity != null && x.RegistrationCity.Contains(keywords))
                || (x.BusinessRegion != null && x.BusinessRegion.Contains(keywords))
                || (x.BusinessProvince != null && x.BusinessProvince.Contains(keywords))
                || (x.BusinessCity != null && x.BusinessCity.Contains(keywords))
                || (x.BusinessAddress1 != null && x.BusinessAddress1.Contains(keywords))
                || (x.BusinessAddress2 != null && x.BusinessAddress2.Contains(keywords))
                || (x.CompanyPhone != null && x.CompanyPhone.Contains(keywords))
                || (x.CompanyEmail != null && x.CompanyEmail.Contains(keywords))
                || (x.CompanyFax != null && x.CompanyFax.Contains(keywords))
                || (x.CompanyWebsite != null && x.CompanyWebsite.Contains(keywords))
                || (x.UnifiedSocialCreditCode != null && x.UnifiedSocialCreditCode.Contains(keywords))
                || (x.TaxRegistrationNumber != null && x.TaxRegistrationNumber.Contains(keywords))
                || (x.LegalRepresentative != null && x.LegalRepresentative.Contains(keywords))
                || (x.CompanyManagerUserName != null && x.CompanyManagerUserName.Contains(keywords))
                || (x.CodeAlias != null && x.CodeAlias.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankAccount != null && x.BankAccount.Contains(keywords))
                || (x.AccountHolder != null && x.AccountHolder.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.ChartOfAccounts != null && x.ChartOfAccounts.Contains(keywords))
                || (x.InputTaxCode != null && x.InputTaxCode.Contains(keywords))
                || (x.OutputTaxCode != null && x.OutputTaxCode.Contains(keywords))
                || (x.BusinessPlace != null && x.BusinessPlace.Contains(keywords))
                || (x.PostingPeriodVariant != null && x.PostingPeriodVariant.Contains(keywords))
                || (x.FiscalYearVariant != null && x.FiscalYearVariant.Contains(keywords))
                || (x.CreditControlArea != null && x.CreditControlArea.Contains(keywords))
                || (x.FinancialManagementArea != null && x.FinancialManagementArea.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RelatedPlant))
        {
            var relatedPlant = queryDto.RelatedPlant;
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(relatedPlant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyName1))
        {
            var companyName1 = queryDto.CompanyName1;
            exp = exp.And(x => x.CompanyName1 != null && x.CompanyName1.Contains(companyName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyName2))
        {
            var companyName2 = queryDto.CompanyName2;
            exp = exp.And(x => x.CompanyName2 != null && x.CompanyName2.Contains(companyName2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyShortName))
        {
            var companyShortName = queryDto.CompanyShortName;
            exp = exp.And(x => x.CompanyShortName != null && x.CompanyShortName.Contains(companyShortName));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.EnterpriseScale))
        {
            var enterpriseScale = queryDto.EnterpriseScale;
            exp = exp.And(x => x.EnterpriseScale != null && x.EnterpriseScale.Contains(enterpriseScale));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessScope))
        {
            var businessScope = queryDto.BusinessScope;
            exp = exp.And(x => x.BusinessScope != null && x.BusinessScope.Contains(businessScope));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.RegistrationRegion))
        {
            var registrationRegion = queryDto.RegistrationRegion;
            exp = exp.And(x => x.RegistrationRegion != null && x.RegistrationRegion.Contains(registrationRegion));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessRegion))
        {
            var businessRegion = queryDto.BusinessRegion;
            exp = exp.And(x => x.BusinessRegion != null && x.BusinessRegion.Contains(businessRegion));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessProvince))
        {
            var businessProvince = queryDto.BusinessProvince;
            exp = exp.And(x => x.BusinessProvince != null && x.BusinessProvince.Contains(businessProvince));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessCity))
        {
            var businessCity = queryDto.BusinessCity;
            exp = exp.And(x => x.BusinessCity != null && x.BusinessCity.Contains(businessCity));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessAddress1))
        {
            var businessAddress1 = queryDto.BusinessAddress1;
            exp = exp.And(x => x.BusinessAddress1 != null && x.BusinessAddress1.Contains(businessAddress1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessAddress2))
        {
            var businessAddress2 = queryDto.BusinessAddress2;
            exp = exp.And(x => x.BusinessAddress2 != null && x.BusinessAddress2.Contains(businessAddress2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyPhone))
        {
            var companyPhone = queryDto.CompanyPhone;
            exp = exp.And(x => x.CompanyPhone != null && x.CompanyPhone.Contains(companyPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyEmail))
        {
            var companyEmail = queryDto.CompanyEmail;
            exp = exp.And(x => x.CompanyEmail != null && x.CompanyEmail.Contains(companyEmail));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyFax))
        {
            var companyFax = queryDto.CompanyFax;
            exp = exp.And(x => x.CompanyFax != null && x.CompanyFax.Contains(companyFax));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyWebsite))
        {
            var companyWebsite = queryDto.CompanyWebsite;
            exp = exp.And(x => x.CompanyWebsite != null && x.CompanyWebsite.Contains(companyWebsite));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnifiedSocialCreditCode))
        {
            var unifiedSocialCreditCode = queryDto.UnifiedSocialCreditCode;
            exp = exp.And(x => x.UnifiedSocialCreditCode != null && x.UnifiedSocialCreditCode.Contains(unifiedSocialCreditCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxRegistrationNumber))
        {
            var taxRegistrationNumber = queryDto.TaxRegistrationNumber;
            exp = exp.And(x => x.TaxRegistrationNumber != null && x.TaxRegistrationNumber.Contains(taxRegistrationNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LegalRepresentative))
        {
            var legalRepresentative = queryDto.LegalRepresentative;
            exp = exp.And(x => x.LegalRepresentative != null && x.LegalRepresentative.Contains(legalRepresentative));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompanyManagerUserName))
        {
            var companyManager = queryDto.CompanyManagerUserName;
            exp = exp.And(x => x.CompanyManagerUserName != null && x.CompanyManagerUserName.Contains(companyManager));
        }

        if (queryDto?.RegisteredCapital.HasValue == true)
        {
            var registeredCapital = queryDto.RegisteredCapital.Value;
            exp = exp.And(x => x.RegisteredCapital == registeredCapital);
        }

        if (queryDto?.CompanyExistence.HasValue == true)
        {
            var companyExistence = queryDto.CompanyExistence.Value;
            exp = exp.And(x => x.CompanyExistence == companyExistence);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CodeAlias))
        {
            var codeAlias = queryDto.CodeAlias;
            exp = exp.And(x => x.CodeAlias != null && x.CodeAlias.Contains(codeAlias));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ChartOfAccounts))
        {
            var chartOfAccounts = queryDto.ChartOfAccounts;
            exp = exp.And(x => x.ChartOfAccounts != null && x.ChartOfAccounts.Contains(chartOfAccounts));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InputTaxCode))
        {
            var inputTaxCode = queryDto.InputTaxCode;
            exp = exp.And(x => x.InputTaxCode != null && x.InputTaxCode.Contains(inputTaxCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OutputTaxCode))
        {
            var outputTaxCode = queryDto.OutputTaxCode;
            exp = exp.And(x => x.OutputTaxCode != null && x.OutputTaxCode.Contains(outputTaxCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessPlace))
        {
            var businessPlace = queryDto.BusinessPlace;
            exp = exp.And(x => x.BusinessPlace != null && x.BusinessPlace.Contains(businessPlace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostingPeriodVariant))
        {
            var postingPeriodVariant = queryDto.PostingPeriodVariant;
            exp = exp.And(x => x.PostingPeriodVariant != null && x.PostingPeriodVariant.Contains(postingPeriodVariant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiscalYearVariant))
        {
            var fiscalYearVariant = queryDto.FiscalYearVariant;
            exp = exp.And(x => x.FiscalYearVariant != null && x.FiscalYearVariant.Contains(fiscalYearVariant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CreditControlArea))
        {
            var creditControlArea = queryDto.CreditControlArea;
            exp = exp.And(x => x.CreditControlArea != null && x.CreditControlArea.Contains(creditControlArea));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FinancialManagementArea))
        {
            var financialManagementArea = queryDto.FinancialManagementArea;
            exp = exp.And(x => x.FinancialManagementArea != null && x.FinancialManagementArea.Contains(financialManagementArea));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.CompanyStatus.HasValue == true)
        {
            var companyStatus = queryDto.CompanyStatus.Value;
            exp = exp.And(x => x.CompanyStatus == companyStatus);
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

        if (queryDto?.EstablishmentDateStart.HasValue == true)
        {
            var establishmentDateStart = queryDto.EstablishmentDateStart.Value;
            exp = exp.And(x => x.EstablishmentDate >= establishmentDateStart);
        }

        if (queryDto?.EstablishmentDateEnd.HasValue == true)
        {
            var establishmentDateEnd = queryDto.EstablishmentDateEnd.Value;
            exp = exp.And(x => x.EstablishmentDate <= establishmentDateEnd);
        }

        if (queryDto?.ClosingDateStart.HasValue == true)
        {
            var closingDateStart = queryDto.ClosingDateStart.Value;
            exp = exp.And(x => x.ClosingDate >= closingDateStart);
        }

        if (queryDto?.ClosingDateEnd.HasValue == true)
        {
            var closingDateEnd = queryDto.ClosingDateEnd.Value;
            exp = exp.And(x => x.ClosingDate <= closingDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktCompanyQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RelatedPlant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyName2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyShortName))
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
        if (!string.IsNullOrWhiteSpace(queryDto.EnterpriseScale))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessScope))
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
        if (!string.IsNullOrWhiteSpace(queryDto.RegistrationRegion))
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
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessRegion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessProvince))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessCity))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessAddress1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessAddress2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyEmail))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyFax))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyWebsite))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnifiedSocialCreditCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxRegistrationNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LegalRepresentative))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompanyManagerUserName))
        {
            return true;
        }
        if (queryDto.RegisteredCapital.HasValue)
        {
            return true;
        }
        if (queryDto.CompanyExistence.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CodeAlias))
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
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ChartOfAccounts))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InputTaxCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OutputTaxCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessPlace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostingPeriodVariant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiscalYearVariant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CreditControlArea))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FinancialManagementArea))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.CompanyStatus.HasValue)
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
        if (queryDto.EstablishmentDateStart.HasValue || queryDto.EstablishmentDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ClosingDateStart.HasValue || queryDto.ClosingDateEnd.HasValue)
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
