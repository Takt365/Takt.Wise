// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktQualityGroupService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：质量组主数据应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 质量组主数据应用服务
/// </summary>
public class TaktQualityGroupService : TaktServiceBase, ITaktQualityGroupService
{
    private readonly ITaktCompanyRepository<TaktQualityGroup> _qualityGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityGroupRepository">质量组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityGroupService(
        ITaktCompanyRepository<TaktQualityGroup> qualityGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityGroupRepository = qualityGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取质量组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityGroupDto>> GetQualityGroupListAsync(TaktQualityGroupQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityGroupDto>.Create(
            data.Adapt<List<TaktQualityGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取质量组主数据
    /// </summary>
    /// <param name="id">质量组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityGroupDto?> GetQualityGroupByIdAsync(long id)
    {
        var entity = await _qualityGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityGroupDto>();
    }

    /// <summary>
    /// 获取质量组主数据选项列表
    /// </summary>
    /// <param name="qualityGroupCategory">质量组类别（字典 logistics_quality_group_category；为空则返回全部启用组）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityGroupOptionsAsync(string? plantCode = null, string? keyword = null, int? qualityGroupCategory = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityGroupRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.GroupStatus == 1
                && (!qualityGroupCategory.HasValue || x.QualityGroupCategory == qualityGroupCategory.Value),
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityGroupCode,
            DictLabel = string.IsNullOrWhiteSpace(e.QualityGroupName) ? e.QualityGroupCode : e.QualityGroupName,
            ExtLabel = e.QualityGroupCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建质量组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityGroupDto> CreateQualityGroupAsync(TaktQualityGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_quality_operation_quality_group_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityGroupCategory == entity.QualityGroupCategory
                && x.QualityGroupCode == entity.QualityGroupCode);
        if (!isUnique_ix_takt_logistics_quality_operation_quality_group_unique)
        {
            throw new TaktBusinessException("质量组主数据的PlantCode、QualityGroupCategory、QualityGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _qualityGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _qualityGroupRepository.CreateAsync(entity);
        return await GetQualityGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityGroupDto>();
    }

    /// <summary>
    /// 更新质量组主数据
    /// </summary>
    /// <param name="id">质量组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityGroupDto> UpdateQualityGroupAsync(long id, TaktQualityGroupUpdateDto dto)
    {
        var entity = await _qualityGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_quality_operation_quality_group_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityGroupCategory == entity.QualityGroupCategory
                && x.QualityGroupCode == entity.QualityGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_operation_quality_group_unique)
        {
            throw new TaktBusinessException("质量组主数据的PlantCode、QualityGroupCategory、QualityGroupCode已存在");
        }
        await _qualityGroupRepository.UpdateAsync(entity);
        return await GetQualityGroupByIdAsync(id) ?? throw new TaktBusinessException("质量组主数据不存在");
    }

    /// <summary>
    /// 删除质量组主数据
    /// </summary>
    /// <param name="id">质量组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityGroupByIdAsync(long id)
    {
        var entity = await _qualityGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置质量组主数据不允许删除");
        }
        var deleted = await _qualityGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("质量组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除质量组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _qualityGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置质量组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteQualityGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新质量组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityGroupDto> UpdateQualityGroupStatusAsync(TaktQualityGroupStatusDto dto)
    {
        var entity = await _qualityGroupRepository.GetByIdAsync(dto.QualityGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("质量组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置质量组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _qualityGroupRepository.UpdateAsync(entity);
        return await GetQualityGroupByIdAsync(dto.QualityGroupId) ?? throw new TaktBusinessException("质量组主数据不存在");
    }

    /// <summary>
    /// 更新质量组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityGroupDto> UpdateQualityGroupSortAsync(TaktQualityGroupSortDto dto)
    {
        var entity = await _qualityGroupRepository.GetByIdAsync(dto.QualityGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("质量组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _qualityGroupRepository.UpdateAsync(entity);
        return await GetQualityGroupByIdAsync(dto.QualityGroupId) ?? throw new TaktBusinessException("质量组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityGroupTemplateDto>(
            sheetName ?? "质量组主数据导入模板",
            fileName ?? "质量组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入质量组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityGroupImportDto>(fileStream, sheetName ?? "质量组主数据导入模板");
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
                var entity = rows[i].Adapt<TaktQualityGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.QualityGroupCategory}|{entity.QualityGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityGroupCategory、QualityGroupCode）");
                }
                var isUnique_ix_takt_logistics_quality_operation_quality_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityGroupCategory == entity.QualityGroupCategory
                        && x.QualityGroupCode == entity.QualityGroupCode);
                if (!isUnique_ix_takt_logistics_quality_operation_quality_group_unique)
                {
                    throw new TaktBusinessException("质量组主数据的PlantCode、QualityGroupCategory、QualityGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _qualityGroupRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
                }
                await _qualityGroupRepository.CreateAsync(entity);
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
    /// 导出质量组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityGroupAsync(TaktQualityGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktQualityGroupQueryDto());
        var list = await _qualityGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityGroupExportDto>(),
                sheetName ?? "质量组主数据数据",
                fileName ?? "质量组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "质量组主数据数据",
            fileName ?? "质量组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建质量组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityGroup, bool>> QueryExpression(TaktQualityGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityGroup>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.QualityGroupCategory).Contains(keywords)
                || (x.QualityGroupCode != null && x.QualityGroupCode.Contains(keywords))
                || (x.QualityGroupName != null && x.QualityGroupName.Contains(keywords))
                || (x.QualityGroupDescription != null && x.QualityGroupDescription.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.GroupStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.QualityGroupCategory.HasValue == true)
        {
            exp = exp.And(x => x.QualityGroupCategory == queryDto.QualityGroupCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityGroupCode))
        {
            exp = exp.And(x => x.QualityGroupCode != null && x.QualityGroupCode.Contains(queryDto.QualityGroupCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityGroupName))
        {
            exp = exp.And(x => x.QualityGroupName != null && x.QualityGroupName.Contains(queryDto.QualityGroupName));
        }

        if (!string.IsNullOrEmpty(queryDto?.QualityGroupDescription))
        {
            exp = exp.And(x => x.QualityGroupDescription != null && x.QualityGroupDescription.Contains(queryDto.QualityGroupDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPhone))
        {
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(queryDto.ContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactEmail))
        {
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(queryDto.ContactEmail));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.GroupStatus.HasValue == true)
        {
            exp = exp.And(x => x.GroupStatus == queryDto.GroupStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
