// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGroupService.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变组主数据应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变组主数据应用服务
/// </summary>
public class TaktEcGroupService : TaktServiceBase, ITaktEcGroupService
{
    private readonly ITaktCompanyRepository<TaktEcGroup> _ecGroupRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecGroupRepository">设变组主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcGroupService(
        ITaktCompanyRepository<TaktEcGroup> ecGroupRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecGroupRepository = ecGroupRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcGroupDto>> GetEcGroupListAsync(TaktEcGroupQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecGroupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcGroupDto>.Create(
            data.Adapt<List<TaktEcGroupDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGroupDto?> GetEcGroupByIdAsync(long id)
    {
        var entity = await _ecGroupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcGroupDto>();
    }

    /// <summary>
    /// 获取设变组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="ecGroupCategory">设变组类别（可选；部门编码，多个逗号分隔）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcGroupOptionsAsync(string? plantCode = null, string? keyword = null, string? ecGroupCategory = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var categories = (ecGroupCategory ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var predicate = Expressionable.Create<TaktEcGroup>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.GroupStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(categories.Count > 0, x => categories.Contains(x.EcGroupCategory))
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.EcGroupCode != null && x.EcGroupCode.Contains(normalizedKeyword!))
                || (x.EcGroupName != null && x.EcGroupName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _ecGroupRepository.GetListAsync(predicate, x => x.SortOrder, false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EcGroupCode,
            DictLabel = string.IsNullOrWhiteSpace(e.EcGroupName) ? e.EcGroupCode : e.EcGroupName,
            ExtLabel = e.EcGroupCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建设变组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGroupDto> CreateEcGroupAsync(TaktEcGroupCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcGroup>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_manufacturing_ec_group_unique = await _uniqueValidator.IsUniqueAsync(
            _ecGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EcGroupCode == entity.EcGroupCode);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_group_unique)
        {
            throw new TaktBusinessException("设变组主数据的PlantCode、EcGroupCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _ecGroupRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _ecGroupRepository.CreateAsync(entity);
        return await GetEcGroupByIdAsync(entity.Id) ?? entity.Adapt<TaktEcGroupDto>();
    }

    /// <summary>
    /// 更新设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGroupDto> UpdateEcGroupAsync(long id, TaktEcGroupUpdateDto dto)
    {
        var entity = await _ecGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变组主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_manufacturing_ec_group_unique = await _uniqueValidator.IsUniqueAsync(
            _ecGroupRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EcGroupCode == entity.EcGroupCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_group_unique)
        {
            throw new TaktBusinessException("设变组主数据的PlantCode、EcGroupCode已存在");
        }
        await _ecGroupRepository.UpdateAsync(entity);
        return await GetEcGroupByIdAsync(id) ?? throw new TaktBusinessException("设变组主数据不存在");
    }

    /// <summary>
    /// 删除设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcGroupByIdAsync(long id)
    {
        var entity = await _ecGroupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变组主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置设变组主数据不允许删除");
        }
        var deleted = await _ecGroupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变组主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcGroupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _ecGroupRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置设变组主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteEcGroupByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变组主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGroupDto> UpdateEcGroupStatusAsync(TaktEcGroupStatusDto dto)
    {
        var entity = await _ecGroupRepository.GetByIdAsync(dto.EcGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变组主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.GroupStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置设变组主数据");
        }
        entity.GroupStatus = dto.GroupStatus;
        await _ecGroupRepository.UpdateAsync(entity);
        return await GetEcGroupByIdAsync(dto.EcGroupId) ?? throw new TaktBusinessException("设变组主数据不存在");
    }

    /// <summary>
    /// 更新设变组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGroupDto> UpdateEcGroupSortAsync(TaktEcGroupSortDto dto)
    {
        var entity = await _ecGroupRepository.GetByIdAsync(dto.EcGroupId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变组主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _ecGroupRepository.UpdateAsync(entity);
        return await GetEcGroupByIdAsync(dto.EcGroupId) ?? throw new TaktBusinessException("设变组主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcGroupTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcGroupTemplateDto>(
            sheetName ?? "设变组主数据导入模板",
            fileName ?? "设变组主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变组主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcGroupAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcGroupImportDto>(fileStream, sheetName ?? "设变组主数据导入模板");
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
                var entity = rows[i].Adapt<TaktEcGroup>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.EcGroupCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EcGroupCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_group_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecGroupRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EcGroupCode == entity.EcGroupCode);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_group_unique)
                {
                    throw new TaktBusinessException("设变组主数据的PlantCode、EcGroupCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _ecGroupRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
                }
                await _ecGroupRepository.CreateAsync(entity);
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
    /// 导出设变组主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcGroupAsync(TaktEcGroupQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcGroupQueryDto());
        var list = await _ecGroupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcGroupExportDto>(),
                sheetName ?? "设变组主数据数据",
                fileName ?? "设变组主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcGroupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变组主数据数据",
            fileName ?? "设变组主数据导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变组主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcGroup, bool>> QueryExpression(TaktEcGroupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcGroup>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EcGroupCode != null && x.EcGroupCode.Contains(keywords))
                || (x.EcGroupName != null && x.EcGroupName.Contains(keywords))
                || (x.EcGroupDescription != null && x.EcGroupDescription.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.EcGroupCode))
        {
            exp = exp.And(x => x.EcGroupCode != null && x.EcGroupCode.Contains(queryDto.EcGroupCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcGroupName))
        {
            exp = exp.And(x => x.EcGroupName != null && x.EcGroupName.Contains(queryDto.EcGroupName));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcGroupDescription))
        {
            exp = exp.And(x => x.EcGroupDescription != null && x.EcGroupDescription.Contains(queryDto.EcGroupDescription));
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
