// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceGroupService.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：客户服务组主数据应用服务实现
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
/// 客户服务组主数据应用服务
/// </summary>
public class TaktCustomerServiceGroupService : TaktServiceBase, ITaktCustomerServiceGroupService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceGroup> _customerServiceGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceGroupRepository">客户服务组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceGroupService(
        ITaktCompanyRepository<TaktCustomerServiceGroup> customerServiceGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceGroupRepository = customerServiceGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户服务组主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerServiceGroupDto>> GetCustomerServiceGroupListAsync(TaktCustomerServiceGroupQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCustomerServiceGroupDto>.Create(
                new List<TaktCustomerServiceGroupDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerServiceGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerServiceGroupDto>.Create(
            data.Adapt<List<TaktCustomerServiceGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户服务组主数据
    /// </summary>
    /// <param name="id">客户服务组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceGroupDto?> GetCustomerServiceGroupByIdAsync(long id)
    {
        var entity = await _customerServiceGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerServiceGroupDto>();
    }

    /// <summary>
    /// 获取客户服务组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerServiceGroupOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktCustomerServiceGroup>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.CustomerServiceGroupCode != null && x.CustomerServiceGroupCode.Contains(normalizedKeyword!))
                || (x.CustomerServiceGroupName != null && x.CustomerServiceGroupName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _customerServiceGroupRepository.GetListAsync(
            predicate,
            x => x.CustomerServiceGroupName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CustomerServiceGroupCode,
            DictLabel = e.CustomerServiceGroupName ?? e.CustomerServiceGroupCode,
        }).ToList();
    }

    /// <summary>
    /// 创建客户服务组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceGroupDto> CreateCustomerServiceGroupAsync(TaktCustomerServiceGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerServiceGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_customer_service_group_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CustomerServiceGroupCategory == entity.CustomerServiceGroupCategory
                && x.CustomerServiceGroupCode == entity.CustomerServiceGroupCode);
        if (!isUnique_ix_takt_logistics_customer_service_group_unique)
        {
            throw new TaktBusinessException("客户服务组主数据的PlantCode、CustomerServiceGroupCategory、CustomerServiceGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerServiceGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _customerServiceGroupRepository.CreateAsync(entity);
        return await GetCustomerServiceGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerServiceGroupDto>();
    }

    /// <summary>
    /// 更新客户服务组主数据
    /// </summary>
    /// <param name="id">客户服务组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceGroupDto> UpdateCustomerServiceGroupAsync(long id, TaktCustomerServiceGroupUpdateDto dto)
    {
        var entity = await _customerServiceGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户服务组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_customer_service_group_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CustomerServiceGroupCategory == entity.CustomerServiceGroupCategory
                && x.CustomerServiceGroupCode == entity.CustomerServiceGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_customer_service_group_unique)
        {
            throw new TaktBusinessException("客户服务组主数据的PlantCode、CustomerServiceGroupCategory、CustomerServiceGroupCode已存在");
        }
        await _customerServiceGroupRepository.UpdateAsync(entity);
        return await GetCustomerServiceGroupByIdAsync(id) ?? throw new TaktBusinessException("客户服务组主数据不存在");
    }

    /// <summary>
    /// 删除客户服务组主数据
    /// </summary>
    /// <param name="id">客户服务组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceGroupByIdAsync(long id)
    {
        var entity = await _customerServiceGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户服务组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置客户服务组主数据不允许删除");
        }
        var deleted = await _customerServiceGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客户服务组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客户服务组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _customerServiceGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置客户服务组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteCustomerServiceGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户服务组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceGroupDto> UpdateCustomerServiceGroupStatusAsync(TaktCustomerServiceGroupStatusDto dto)
    {
        var entity = await _customerServiceGroupRepository.GetByIdAsync(dto.CustomerServiceGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户服务组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置客户服务组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _customerServiceGroupRepository.UpdateAsync(entity);
        return await GetCustomerServiceGroupByIdAsync(dto.CustomerServiceGroupId) ?? throw new TaktBusinessException("客户服务组主数据不存在");
    }

    /// <summary>
    /// 更新客户服务组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceGroupDto> UpdateCustomerServiceGroupSortAsync(TaktCustomerServiceGroupSortDto dto)
    {
        var entity = await _customerServiceGroupRepository.GetByIdAsync(dto.CustomerServiceGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户服务组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerServiceGroupRepository.UpdateAsync(entity);
        return await GetCustomerServiceGroupByIdAsync(dto.CustomerServiceGroupId) ?? throw new TaktBusinessException("客户服务组主数据不存在");
    }

    /// <summary>
    /// 更新客户服务组主数据内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceGroupDto> UpdateCustomerServiceGroupBuiltInAsync(TaktCustomerServiceGroupBuiltInDto dto)
    {
        var entity = await _customerServiceGroupRepository.GetByIdAsync(dto.CustomerServiceGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户服务组主数据不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置客户服务组主数据标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _customerServiceGroupRepository.UpdateAsync(entity);
        return await GetCustomerServiceGroupByIdAsync(dto.CustomerServiceGroupId) ?? throw new TaktBusinessException("客户服务组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerServiceGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerServiceGroupTemplateDto>(
            sheetName ?? "客户服务组主数据导入模板",
            fileName ?? "客户服务组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户服务组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerServiceGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerServiceGroupImportDto>(fileStream, sheetName ?? "客户服务组主数据导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _customerServiceGroupRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCustomerServiceGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.CustomerServiceGroupCategory}|{entity.CustomerServiceGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、CustomerServiceGroupCategory、CustomerServiceGroupCode）");
                }
                var isUnique_ix_takt_logistics_customer_service_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerServiceGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.CustomerServiceGroupCategory == entity.CustomerServiceGroupCategory
                        && x.CustomerServiceGroupCode == entity.CustomerServiceGroupCode);
                if (!isUnique_ix_takt_logistics_customer_service_group_unique)
                {
                    throw new TaktBusinessException("客户服务组主数据的PlantCode、CustomerServiceGroupCategory、CustomerServiceGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _customerServiceGroupRepository.CreateAsync(entity);
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
    /// 导出客户服务组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerServiceGroupAsync(TaktCustomerServiceGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCustomerServiceGroupQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceGroupExportDto>(),
                sheetName ?? "客户服务组主数据数据",
                fileName ?? "客户服务组主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _customerServiceGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceGroupExportDto>(),
                sheetName ?? "客户服务组主数据数据",
                fileName ?? "客户服务组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerServiceGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户服务组主数据数据",
            fileName ?? "客户服务组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户服务组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerServiceGroup, bool>> QueryExpression(TaktCustomerServiceGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerServiceGroup>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CustomerServiceGroupCode != null && x.CustomerServiceGroupCode.Contains(keywords))
                || (x.CustomerServiceGroupName != null && x.CustomerServiceGroupName.Contains(keywords))
                || (x.CustomerServiceGroupDescription != null && x.CustomerServiceGroupDescription.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerServiceGroupCode))
        {
            var customerServiceGroupCode = queryDto.CustomerServiceGroupCode;
            exp = exp.And(x => x.CustomerServiceGroupCode != null && x.CustomerServiceGroupCode.Contains(customerServiceGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerServiceGroupName))
        {
            var customerServiceGroupName = queryDto.CustomerServiceGroupName;
            exp = exp.And(x => x.CustomerServiceGroupName != null && x.CustomerServiceGroupName.Contains(customerServiceGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerServiceGroupDescription))
        {
            var customerServiceGroupDescription = queryDto.CustomerServiceGroupDescription;
            exp = exp.And(x => x.CustomerServiceGroupDescription != null && x.CustomerServiceGroupDescription.Contains(customerServiceGroupDescription));
        }

        if (queryDto?.CustomerServiceGroupCategory.HasValue == true)
        {
            var customerServiceGroupCategory = queryDto.CustomerServiceGroupCategory.Value;
            exp = exp.And(x => x.CustomerServiceGroupCategory == customerServiceGroupCategory);
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

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.GroupStatus.HasValue == true)
        {
            var groupStatus = queryDto.GroupStatus.Value;
            exp = exp.And(x => x.GroupStatus == groupStatus);
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
    private static bool HasAnyListQueryFilter(TaktCustomerServiceGroupQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerServiceGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerServiceGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerServiceGroupDescription))
        {
            return true;
        }
        if (queryDto.CustomerServiceGroupCategory.HasValue)
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
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.GroupStatus.HasValue)
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
