// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktApsGroupService.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：APS组主数据应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// APS组主数据应用服务
/// </summary>
public class TaktApsGroupService : TaktServiceBase, ITaktApsGroupService
{
    private readonly ITaktCompanyRepository<TaktApsGroup> _apsGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsGroupRepository">APS组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsGroupService(
        ITaktCompanyRepository<TaktApsGroup> apsGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsGroupRepository = apsGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS组主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsGroupDto>> GetApsGroupListAsync(TaktApsGroupQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktApsGroupDto>.Create(
                new List<TaktApsGroupDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsGroupDto>.Create(
            data.Adapt<List<TaktApsGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS组主数据
    /// </summary>
    /// <param name="id">APS组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsGroupDto?> GetApsGroupByIdAsync(long id)
    {
        var entity = await _apsGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktApsGroupDto>();
    }

    /// <summary>
    /// 获取APS组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="apsGroupCategory">APS组类别（可选；1=计划员 2=发布员 3=设备操作员 4=设备维护员）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsGroupOptionsAsync(string? plantCode = null, string? keyword = null, int? apsGroupCategory = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktApsGroup>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(apsGroupCategory.HasValue, x => x.ApsGroupCategory == apsGroupCategory!.Value)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.ApsGroupCode != null && x.ApsGroupCode.Contains(normalizedKeyword!))
                || (x.ApsGroupName != null && x.ApsGroupName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _apsGroupRepository.GetListAsync(
            predicate,
            x => x.ApsGroupName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ApsGroupCode,
            DictLabel = e.ApsGroupName ?? e.ApsGroupCode,
            ExtLabel = e.ApsGroupName ?? e.ApsGroupCode,
            ExtValue = e.Id,
        }).ToList();
    }

    /// <summary>
    /// 创建APS组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsGroupDto> CreateApsGroupAsync(TaktApsGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_manufacturing_aps_group_unique = await _uniqueValidator.IsUniqueAsync(
            _apsGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ApsGroupCategory == entity.ApsGroupCategory
                && x.ApsGroupCode == entity.ApsGroupCode);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_group_unique)
        {
            throw new TaktBusinessException("APS组主数据的PlantCode、ApsGroupCategory、ApsGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _apsGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _apsGroupRepository.CreateAsync(entity);
        return await GetApsGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktApsGroupDto>();
    }

    /// <summary>
    /// 更新APS组主数据
    /// </summary>
    /// <param name="id">APS组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsGroupDto> UpdateApsGroupAsync(long id, TaktApsGroupUpdateDto dto)
    {
        var entity = await _apsGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_manufacturing_aps_group_unique = await _uniqueValidator.IsUniqueAsync(
            _apsGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ApsGroupCategory == entity.ApsGroupCategory
                && x.ApsGroupCode == entity.ApsGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_group_unique)
        {
            throw new TaktBusinessException("APS组主数据的PlantCode、ApsGroupCategory、ApsGroupCode已存在");
        }
        await _apsGroupRepository.UpdateAsync(entity);
        return await GetApsGroupByIdAsync(id) ?? throw new TaktBusinessException("APS组主数据不存在");
    }

    /// <summary>
    /// 删除APS组主数据
    /// </summary>
    /// <param name="id">APS组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsGroupByIdAsync(long id)
    {
        var entity = await _apsGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置APS组主数据不允许删除");
        }
        var deleted = await _apsGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _apsGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置APS组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteApsGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsGroupDto> UpdateApsGroupStatusAsync(TaktApsGroupStatusDto dto)
    {
        var entity = await _apsGroupRepository.GetByIdAsync(dto.ApsGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置APS组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _apsGroupRepository.UpdateAsync(entity);
        return await GetApsGroupByIdAsync(dto.ApsGroupId) ?? throw new TaktBusinessException("APS组主数据不存在");
    }

    /// <summary>
    /// 更新APS组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsGroupDto> UpdateApsGroupSortAsync(TaktApsGroupSortDto dto)
    {
        var entity = await _apsGroupRepository.GetByIdAsync(dto.ApsGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _apsGroupRepository.UpdateAsync(entity);
        return await GetApsGroupByIdAsync(dto.ApsGroupId) ?? throw new TaktBusinessException("APS组主数据不存在");
    }

    /// <summary>
    /// 更新APS组主数据内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsGroupDto> UpdateApsGroupBuiltInAsync(TaktApsGroupBuiltInDto dto)
    {
        var entity = await _apsGroupRepository.GetByIdAsync(dto.ApsGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS组主数据不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置APS组主数据标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _apsGroupRepository.UpdateAsync(entity);
        return await GetApsGroupByIdAsync(dto.ApsGroupId) ?? throw new TaktBusinessException("APS组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsGroupTemplateDto>(
            sheetName ?? "APS组主数据导入模板",
            fileName ?? "APS组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsGroupImportDto>(fileStream, sheetName ?? "APS组主数据导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _apsGroupRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktApsGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.ApsGroupCategory}|{entity.ApsGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ApsGroupCategory、ApsGroupCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_aps_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ApsGroupCategory == entity.ApsGroupCategory
                        && x.ApsGroupCode == entity.ApsGroupCode);
                if (!isUnique_ix_takt_logistics_manufacturing_aps_group_unique)
                {
                    throw new TaktBusinessException("APS组主数据的PlantCode、ApsGroupCategory、ApsGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _apsGroupRepository.CreateAsync(entity);
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
    /// 导出APS组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsGroupAsync(TaktApsGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktApsGroupQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsGroupExportDto>(),
                sheetName ?? "APS组主数据数据",
                fileName ?? "APS组主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _apsGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsGroupExportDto>(),
                sheetName ?? "APS组主数据数据",
                fileName ?? "APS组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS组主数据数据",
            fileName ?? "APS组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsGroup, bool>> QueryExpression(TaktApsGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsGroup>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ApsGroupCode != null && x.ApsGroupCode.Contains(keywords))
                || (x.ApsGroupName != null && x.ApsGroupName.Contains(keywords))
                || (x.ApsGroupDescription != null && x.ApsGroupDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ApsGroupCode))
        {
            var apsGroupCode = queryDto.ApsGroupCode;
            exp = exp.And(x => x.ApsGroupCode != null && x.ApsGroupCode.Contains(apsGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApsGroupName))
        {
            var apsGroupName = queryDto.ApsGroupName;
            exp = exp.And(x => x.ApsGroupName != null && x.ApsGroupName.Contains(apsGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApsGroupDescription))
        {
            var apsGroupDescription = queryDto.ApsGroupDescription;
            exp = exp.And(x => x.ApsGroupDescription != null && x.ApsGroupDescription.Contains(apsGroupDescription));
        }

        if (queryDto?.ApsGroupCategory.HasValue == true)
        {
            var apsGroupCategory = queryDto.ApsGroupCategory.Value;
            exp = exp.And(x => x.ApsGroupCategory == apsGroupCategory);
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
    private static bool HasAnyListQueryFilter(TaktApsGroupQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ApsGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApsGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApsGroupDescription))
        {
            return true;
        }
        if (queryDto.ApsGroupCategory.HasValue)
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
