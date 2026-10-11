// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialGroupService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：物料组主数据应用服务实现
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
/// 物料组主数据应用服务
/// </summary>
public class TaktMaterialGroupService : TaktServiceBase, ITaktMaterialGroupService
{
    private readonly ITaktTenantRepository<TaktMaterialGroup> _materialGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialGroupRepository">物料组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialGroupService(
        ITaktTenantRepository<TaktMaterialGroup> materialGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialGroupRepository = materialGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料组主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialGroupDto>> GetMaterialGroupListAsync(TaktMaterialGroupQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaterialGroupDto>.Create(
                new List<TaktMaterialGroupDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialGroupDto>.Create(
            data.Adapt<List<TaktMaterialGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料组主数据
    /// </summary>
    /// <param name="id">物料组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialGroupDto?> GetMaterialGroupByIdAsync(long id)
    {
        var entity = await _materialGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialGroupDto>();
    }

    /// <summary>
    /// 获取物料组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialGroupOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _materialGroupRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.MaterialGroupName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialGroupCode,
            DictLabel = e.MaterialGroupName ?? e.MaterialGroupCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialGroupDto> CreateMaterialGroupAsync(TaktMaterialGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialGroup>();
        var isUnique_ix_takt_logistics_materials_material_group_unique = await _uniqueValidator.IsUniqueAsync(
            _materialGroupRepository,
            x => x.MaterialGroupCode == entity.MaterialGroupCode);
        if (!isUnique_ix_takt_logistics_materials_material_group_unique)
        {
            throw new TaktBusinessException("物料组主数据的MaterialGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _materialGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _materialGroupRepository.CreateAsync(entity);
        return await GetMaterialGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialGroupDto>();
    }

    /// <summary>
    /// 更新物料组主数据
    /// </summary>
    /// <param name="id">物料组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialGroupDto> UpdateMaterialGroupAsync(long id, TaktMaterialGroupUpdateDto dto)
    {
        var entity = await _materialGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料组主数据不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_group_unique = await _uniqueValidator.IsUniqueAsync(
            _materialGroupRepository,
            x => x.MaterialGroupCode == entity.MaterialGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_group_unique)
        {
            throw new TaktBusinessException("物料组主数据的MaterialGroupCode已存在");
        }
        await _materialGroupRepository.UpdateAsync(entity);
        return await GetMaterialGroupByIdAsync(id) ?? throw new TaktBusinessException("物料组主数据不存在");
    }

    /// <summary>
    /// 删除物料组主数据
    /// </summary>
    /// <param name="id">物料组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialGroupByIdAsync(long id)
    {
        var deleted = await _materialGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialGroupDto> UpdateMaterialGroupSortAsync(TaktMaterialGroupSortDto dto)
    {
        var entity = await _materialGroupRepository.GetByIdAsync(dto.MaterialGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _materialGroupRepository.UpdateAsync(entity);
        return await GetMaterialGroupByIdAsync(dto.MaterialGroupId) ?? throw new TaktBusinessException("物料组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialGroupTemplateDto>(
            sheetName ?? "物料组主数据导入模板",
            fileName ?? "物料组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialGroupImportDto>(fileStream, sheetName ?? "物料组主数据导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _materialGroupRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktMaterialGroup>();
                var importKey = $"{entity.MaterialGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialGroupCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialGroupRepository,
                    x => x.MaterialGroupCode == entity.MaterialGroupCode);
                if (!isUnique_ix_takt_logistics_materials_material_group_unique)
                {
                    throw new TaktBusinessException("物料组主数据的MaterialGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _materialGroupRepository.CreateAsync(entity);
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
    /// 导出物料组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialGroupAsync(TaktMaterialGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaterialGroupQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialGroupExportDto>(),
                sheetName ?? "物料组主数据数据",
                fileName ?? "物料组主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _materialGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialGroupExportDto>(),
                sheetName ?? "物料组主数据数据",
                fileName ?? "物料组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料组主数据数据",
            fileName ?? "物料组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialGroup, bool>> QueryExpression(TaktMaterialGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialGroup>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.MaterialGroupCode != null && x.MaterialGroupCode.Contains(keywords))
                || (x.MaterialGroupName != null && x.MaterialGroupName.Contains(keywords))
                || (x.MaterialGroupDescription != null && x.MaterialGroupDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialGroupCode))
        {
            var materialGroupCode = queryDto.MaterialGroupCode;
            exp = exp.And(x => x.MaterialGroupCode != null && x.MaterialGroupCode.Contains(materialGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialGroupName))
        {
            var materialGroupName = queryDto.MaterialGroupName;
            exp = exp.And(x => x.MaterialGroupName != null && x.MaterialGroupName.Contains(materialGroupName));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialGroupDescription))
        {
            var materialGroupDescription = queryDto.MaterialGroupDescription;
            exp = exp.And(x => x.MaterialGroupDescription != null && x.MaterialGroupDescription.Contains(materialGroupDescription));
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
    private static bool HasAnyListQueryFilter(TaktMaterialGroupQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroupName))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroupDescription))
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
