// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPlantService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 工厂应用服务
/// </summary>
public class TaktPlantService : TaktServiceBase, ITaktPlantService
{
    private readonly ITaktTenantRepository<TaktPlant> _plantRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="plantRepository">工厂仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPlantService(
        ITaktTenantRepository<TaktPlant> plantRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _plantRepository = plantRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工厂列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPlantDto>> GetPlantListAsync(TaktPlantQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPlantDto>.Create(
                new List<TaktPlantDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _plantRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPlantDto>.Create(
            data.Adapt<List<TaktPlantDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto?> GetPlantByIdAsync(long id)
    {
        var entity = await _plantRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktPlantDto>();
    }

    /// <summary>
    /// 获取工厂选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _plantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.PlantStatus == 1,
            x => x.PlantShortName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PlantCode,
            DictLabel = e.PlantShortName ?? e.PlantCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工厂
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> CreatePlantAsync(TaktPlantCreateDto dto)
    {
        var entity = dto.Adapt<TaktPlant>();
        var isUnique_ix_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _plantRepository,
            x => x.PlantCode == entity.PlantCode);
        if (!isUnique_ix_plant_code_unique)
        {
            throw new TaktBusinessException("工厂的PlantCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _plantRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _plantRepository.CreateAsync(entity);
        return await GetPlantByIdAsync(entity.Id) ?? entity.Adapt<TaktPlantDto>();
    }

    /// <summary>
    /// 更新工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> UpdatePlantAsync(long id, TaktPlantUpdateDto dto)
    {
        var entity = await _plantRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _plantRepository,
            x => x.PlantCode == entity.PlantCode,
            id);
        if (!isUnique_ix_plant_code_unique)
        {
            throw new TaktBusinessException("工厂的PlantCode已存在");
        }
        await _plantRepository.UpdateAsync(entity);
        return await GetPlantByIdAsync(id) ?? throw new TaktBusinessException("工厂不存在");
    }

    /// <summary>
    /// 删除工厂
    /// </summary>
    /// <param name="id">工厂ID</param>
    /// <returns>任务</returns>
    public async Task DeletePlantByIdAsync(long id)
    {
        var deleted = await _plantRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工厂不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工厂
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePlantBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePlantByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工厂状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> UpdatePlantStatusAsync(TaktPlantStatusDto dto)
    {
        var entity = await _plantRepository.GetByIdAsync(dto.PlantId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂不存在");
        }
        entity.PlantStatus = dto.PlantStatus;
        await _plantRepository.UpdateAsync(entity);
        return await GetPlantByIdAsync(dto.PlantId) ?? throw new TaktBusinessException("工厂不存在");
    }

    /// <summary>
    /// 更新工厂排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlantDto> UpdatePlantSortAsync(TaktPlantSortDto dto)
    {
        var entity = await _plantRepository.GetByIdAsync(dto.PlantId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _plantRepository.UpdateAsync(entity);
        return await GetPlantByIdAsync(dto.PlantId) ?? throw new TaktBusinessException("工厂不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPlantTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPlantTemplateDto>(
            sheetName ?? "工厂导入模板",
            fileName ?? "工厂导入模板.xlsx");
    }

    /// <summary>
    /// 导入工厂
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPlantAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPlantImportDto>(fileStream, sheetName ?? "工厂导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _plantRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPlant>();
                var importKey = $"{entity.PlantCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode）");
                }
                var isUnique_ix_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _plantRepository,
                    x => x.PlantCode == entity.PlantCode);
                if (!isUnique_ix_plant_code_unique)
                {
                    throw new TaktBusinessException("工厂的PlantCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _plantRepository.CreateAsync(entity);
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
    /// 导出工厂
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPlantAsync(TaktPlantQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPlantQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPlantExportDto>(),
                sheetName ?? "工厂数据",
                fileName ?? "工厂导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _plantRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPlantExportDto>(),
                sheetName ?? "工厂数据",
                fileName ?? "工厂导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPlantExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工厂数据",
            fileName ?? "工厂导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工厂查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPlant, bool>> QueryExpression(TaktPlantQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPlant>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantName1 != null && x.PlantName1.Contains(keywords))
                || (x.PlantName2 != null && x.PlantName2.Contains(keywords))
                || (x.PlantShortName != null && x.PlantShortName.Contains(keywords))
                || (x.CodeAlias != null && x.CodeAlias.Contains(keywords))
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
                || (x.PlantAddress1 != null && x.PlantAddress1.Contains(keywords))
                || (x.PlantAddress2 != null && x.PlantAddress2.Contains(keywords))
                || (x.PlantPhone != null && x.PlantPhone.Contains(keywords))
                || (x.PlantEmail != null && x.PlantEmail.Contains(keywords))
                || (x.PlantFax != null && x.PlantFax.Contains(keywords))
                || (x.PlantWebsite != null && x.PlantWebsite.Contains(keywords))
                || (x.UnifiedSocialCreditCode != null && x.UnifiedSocialCreditCode.Contains(keywords))
                || (x.TaxRegistrationNumber != null && x.TaxRegistrationNumber.Contains(keywords))
                || (x.LegalRepresentative != null && x.LegalRepresentative.Contains(keywords))
                || (x.PlantManagerUserName != null && x.PlantManagerUserName.Contains(keywords))
                || (x.BankCode != null && x.BankCode.Contains(keywords))
                || (x.BankAccount != null && x.BankAccount.Contains(keywords))
                || (x.AccountHolder != null && x.AccountHolder.Contains(keywords))
                || (x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(keywords))
                || (x.SalesOrganization != null && x.SalesOrganization.Contains(keywords))
                || (x.MaterialRequirementsPlanning != null && x.MaterialRequirementsPlanning.Contains(keywords))
                || (x.DistributionChannel != null && x.DistributionChannel.Contains(keywords))
                || (x.IntercompanyBillingProductGroup != null && x.IntercompanyBillingProductGroup.Contains(keywords))
                || (x.TaxIndicator != null && x.TaxIndicator.Contains(keywords))
                || (x.ValuationArea != null && x.ValuationArea.Contains(keywords))
                || (x.PlantVendorNumber != null && x.PlantVendorNumber.Contains(keywords))
                || (x.PlantCustomerNumber != null && x.PlantCustomerNumber.Contains(keywords))
                || (x.FactoryCalendar != null && x.FactoryCalendar.Contains(keywords))
                || (x.RelatedCompany != null && x.RelatedCompany.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantName1))
        {
            var plantName1 = queryDto.PlantName1;
            exp = exp.And(x => x.PlantName1 != null && x.PlantName1.Contains(plantName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantName2))
        {
            var plantName2 = queryDto.PlantName2;
            exp = exp.And(x => x.PlantName2 != null && x.PlantName2.Contains(plantName2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantShortName))
        {
            var plantShortName = queryDto.PlantShortName;
            exp = exp.And(x => x.PlantShortName != null && x.PlantShortName.Contains(plantShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CodeAlias))
        {
            var codeAlias = queryDto.CodeAlias;
            exp = exp.And(x => x.CodeAlias != null && x.CodeAlias.Contains(codeAlias));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantAddress1))
        {
            var plantAddress1 = queryDto.PlantAddress1;
            exp = exp.And(x => x.PlantAddress1 != null && x.PlantAddress1.Contains(plantAddress1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantAddress2))
        {
            var plantAddress2 = queryDto.PlantAddress2;
            exp = exp.And(x => x.PlantAddress2 != null && x.PlantAddress2.Contains(plantAddress2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantPhone))
        {
            var plantPhone = queryDto.PlantPhone;
            exp = exp.And(x => x.PlantPhone != null && x.PlantPhone.Contains(plantPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantEmail))
        {
            var plantEmail = queryDto.PlantEmail;
            exp = exp.And(x => x.PlantEmail != null && x.PlantEmail.Contains(plantEmail));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantFax))
        {
            var plantFax = queryDto.PlantFax;
            exp = exp.And(x => x.PlantFax != null && x.PlantFax.Contains(plantFax));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantWebsite))
        {
            var plantWebsite = queryDto.PlantWebsite;
            exp = exp.And(x => x.PlantWebsite != null && x.PlantWebsite.Contains(plantWebsite));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantManagerUserName))
        {
            var plantManager = queryDto.PlantManagerUserName;
            exp = exp.And(x => x.PlantManagerUserName != null && x.PlantManagerUserName.Contains(plantManager));
        }

        if (queryDto?.RegisteredCapital.HasValue == true)
        {
            var registeredCapital = queryDto.RegisteredCapital.Value;
            exp = exp.And(x => x.RegisteredCapital == registeredCapital);
        }

        if (queryDto?.PlantExistence.HasValue == true)
        {
            var plantExistence = queryDto.PlantExistence.Value;
            exp = exp.And(x => x.PlantExistence == plantExistence);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasingOrganization))
        {
            var purchasingOrganization = queryDto.PurchasingOrganization;
            exp = exp.And(x => x.PurchasingOrganization != null && x.PurchasingOrganization.Contains(purchasingOrganization));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrganization))
        {
            var salesOrganization = queryDto.SalesOrganization;
            exp = exp.And(x => x.SalesOrganization != null && x.SalesOrganization.Contains(salesOrganization));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialRequirementsPlanning))
        {
            var materialRequirementsPlanning = queryDto.MaterialRequirementsPlanning;
            exp = exp.And(x => x.MaterialRequirementsPlanning != null && x.MaterialRequirementsPlanning.Contains(materialRequirementsPlanning));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DistributionChannel))
        {
            var distributionChannel = queryDto.DistributionChannel;
            exp = exp.And(x => x.DistributionChannel != null && x.DistributionChannel.Contains(distributionChannel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IntercompanyBillingProductGroup))
        {
            var intercompanyBillingProductGroup = queryDto.IntercompanyBillingProductGroup;
            exp = exp.And(x => x.IntercompanyBillingProductGroup != null && x.IntercompanyBillingProductGroup.Contains(intercompanyBillingProductGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxIndicator))
        {
            var taxIndicator = queryDto.TaxIndicator;
            exp = exp.And(x => x.TaxIndicator != null && x.TaxIndicator.Contains(taxIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ValuationArea))
        {
            var valuationArea = queryDto.ValuationArea;
            exp = exp.And(x => x.ValuationArea != null && x.ValuationArea.Contains(valuationArea));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantVendorNumber))
        {
            var plantVendorNumber = queryDto.PlantVendorNumber;
            exp = exp.And(x => x.PlantVendorNumber != null && x.PlantVendorNumber.Contains(plantVendorNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCustomerNumber))
        {
            var plantCustomerNumber = queryDto.PlantCustomerNumber;
            exp = exp.And(x => x.PlantCustomerNumber != null && x.PlantCustomerNumber.Contains(plantCustomerNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FactoryCalendar))
        {
            var factoryCalendar = queryDto.FactoryCalendar;
            exp = exp.And(x => x.FactoryCalendar != null && x.FactoryCalendar.Contains(factoryCalendar));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RelatedCompany))
        {
            var relatedCompany = queryDto.RelatedCompany;
            exp = exp.And(x => x.RelatedCompany != null && x.RelatedCompany.Contains(relatedCompany));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.PlantStatus.HasValue == true)
        {
            var plantStatus = queryDto.PlantStatus.Value;
            exp = exp.And(x => x.PlantStatus == plantStatus);
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
    private static bool HasAnyListQueryFilter(TaktPlantQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PlantName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantName2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CodeAlias))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PlantAddress1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantAddress2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantEmail))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantFax))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantWebsite))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PlantManagerUserName))
        {
            return true;
        }
        if (queryDto.RegisteredCapital.HasValue)
        {
            return true;
        }
        if (queryDto.PlantExistence.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasingOrganization))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrganization))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialRequirementsPlanning))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DistributionChannel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IntercompanyBillingProductGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ValuationArea))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantVendorNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCustomerNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FactoryCalendar))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RelatedCompany))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.PlantStatus.HasValue)
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
