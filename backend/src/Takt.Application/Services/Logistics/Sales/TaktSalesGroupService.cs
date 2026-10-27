// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesGroupService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售组主数据应用服务实现
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
/// 销售组主数据应用服务
/// </summary>
public class TaktSalesGroupService : TaktServiceBase, ITaktSalesGroupService
{
    private readonly ITaktCompanyRepository<TaktSalesGroup> _salesGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesGroupRepository">销售组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesGroupService(
        ITaktCompanyRepository<TaktSalesGroup> salesGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesGroupRepository = salesGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取销售组主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesGroupDto>> GetSalesGroupListAsync(TaktSalesGroupQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesGroupDto>.Create(
                new List<TaktSalesGroupDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesGroupDto>.Create(
            data.Adapt<List<TaktSalesGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto?> GetSalesGroupByIdAsync(long id)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesGroupDto>();
    }

    /// <summary>
    /// 获取销售组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesGroupOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesGroupRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1,
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesGroupCode,
            DictLabel = string.IsNullOrWhiteSpace(e.SalesGroupName) ? e.SalesGroupCode : e.SalesGroupName,
            ExtLabel = e.SalesGroupCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建销售组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> CreateSalesGroupAsync(TaktSalesGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesGroup>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var existingRows = await _salesGroupRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
            var generated = await _numberingGenerator.GenerateNextAsync(
                dto.NumberingRuleCode.Trim(),
                existingRows.Select(x => (string?)x.SalesGroupCode).ToList());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.SalesGroupCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.SalesGroupCode))
        {
            throw new TaktBusinessException("销售组编码不能为空");
        }
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_sales_sales_group_unique = await _uniqueValidator.IsUniqueAsync(
            _salesGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesGroupCode == entity.SalesGroupCode);
        if (!isUnique_ix_takt_logistics_sales_sales_group_unique)
        {
            throw new TaktBusinessException("销售组主数据的PlantCode、SalesGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _salesGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _salesGroupRepository.CreateAsync(entity);
        return await GetSalesGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesGroupDto>();
    }

    /// <summary>
    /// 更新销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupAsync(long id, TaktSalesGroupUpdateDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_sales_sales_group_unique = await _uniqueValidator.IsUniqueAsync(
            _salesGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesGroupCode == entity.SalesGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_sales_group_unique)
        {
            throw new TaktBusinessException("销售组主数据的PlantCode、SalesGroupCode已存在");
        }
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(id) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 删除销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesGroupByIdAsync(long id)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置销售组主数据不允许删除");
        }
        var deleted = await _salesGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _salesGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置销售组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteSalesGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupStatusAsync(TaktSalesGroupStatusDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(dto.SalesGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置销售组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(dto.SalesGroupId) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 更新销售组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupSortAsync(TaktSalesGroupSortDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(dto.SalesGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(dto.SalesGroupId) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 更新销售组主数据内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesGroupDto> UpdateSalesGroupBuiltInAsync(TaktSalesGroupBuiltInDto dto)
    {
        var entity = await _salesGroupRepository.GetByIdAsync(dto.SalesGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售组主数据不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置销售组主数据标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _salesGroupRepository.UpdateAsync(entity);
        return await GetSalesGroupByIdAsync(dto.SalesGroupId) ?? throw new TaktBusinessException("销售组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesGroupTemplateDto>(
            sheetName ?? "销售组主数据导入模板",
            fileName ?? "销售组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesGroupImportDto>(fileStream, sheetName ?? "销售组主数据导入模板");
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
                var entity = rows[i].Adapt<TaktSalesGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.SalesGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesGroupCode）");
                }
                var isUnique_ix_takt_logistics_sales_sales_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesGroupCode == entity.SalesGroupCode);
                if (!isUnique_ix_takt_logistics_sales_sales_group_unique)
                {
                    throw new TaktBusinessException("销售组主数据的PlantCode、SalesGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _salesGroupRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
                }
                await _salesGroupRepository.CreateAsync(entity);
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
    /// 导出销售组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesGroupAsync(TaktSalesGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesGroupQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesGroupExportDto>(),
                sheetName ?? "销售组主数据数据",
                fileName ?? "销售组主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesGroupExportDto>(),
                sheetName ?? "销售组主数据数据",
                fileName ?? "销售组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售组主数据数据",
            fileName ?? "销售组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesGroup, bool>> QueryExpression(TaktSalesGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesGroup>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesGroupCode != null && x.SalesGroupCode.Contains(keywords))
                || (x.SalesGroupName != null && x.SalesGroupName.Contains(keywords))
                || (x.SalesGroupDescription != null && x.SalesGroupDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroupCode))
        {
            var salesGroupCode = queryDto.SalesGroupCode;
            exp = exp.And(x => x.SalesGroupCode != null && x.SalesGroupCode.Contains(salesGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroupName))
        {
            var salesGroupName = queryDto.SalesGroupName;
            exp = exp.And(x => x.SalesGroupName != null && x.SalesGroupName.Contains(salesGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroupDescription))
        {
            var salesGroupDescription = queryDto.SalesGroupDescription;
            exp = exp.And(x => x.SalesGroupDescription != null && x.SalesGroupDescription.Contains(salesGroupDescription));
        }
        if (queryDto?.SalesGroupCategory.HasValue == true)
        {
            var salesGroupCategory = queryDto.SalesGroupCategory.Value;
            exp = exp.And(x => x.SalesGroupCategory == salesGroupCategory);
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
    private static bool HasAnyListQueryFilter(TaktSalesGroupQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroupDescription))
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
