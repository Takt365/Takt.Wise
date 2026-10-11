// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktWorkShiftService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：班次信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Attendance;

/// <summary>
/// 班次信息应用服务
/// </summary>
public class TaktWorkShiftService : TaktServiceBase, ITaktWorkShiftService
{
    private readonly ITaktCompanyRepository<TaktWorkShift> _workShiftRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workShiftRepository">班次信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktWorkShiftService(
        ITaktCompanyRepository<TaktWorkShift> workShiftRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _workShiftRepository = workShiftRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取班次信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktWorkShiftDto>> GetWorkShiftListAsync(TaktWorkShiftQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _workShiftRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktWorkShiftDto>.Create(
            data.Adapt<List<TaktWorkShiftDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取班次信息
    /// </summary>
    /// <param name="id">班次信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkShiftDto?> GetWorkShiftByIdAsync(long id)
    {
        var entity = await _workShiftRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktWorkShiftDto>();
    }

    /// <summary>
    /// 获取班次信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetWorkShiftOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _workShiftRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ShiftName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ShiftName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建班次信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkShiftDto> CreateWorkShiftAsync(TaktWorkShiftCreateDto dto)
    {
        var entity = dto.Adapt<TaktWorkShift>();
        var isUnique_ix_work_shift_code_unique = await _uniqueValidator.IsUniqueAsync(
            _workShiftRepository,
            x => x.ShiftCode == entity.ShiftCode);
        if (!isUnique_ix_work_shift_code_unique)
        {
            throw new TaktBusinessException("班次信息的ShiftCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _workShiftRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _workShiftRepository.CreateAsync(entity);
        return await GetWorkShiftByIdAsync(entity.Id) ?? entity.Adapt<TaktWorkShiftDto>();
    }

    /// <summary>
    /// 更新班次信息
    /// </summary>
    /// <param name="id">班次信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkShiftDto> UpdateWorkShiftAsync(long id, TaktWorkShiftUpdateDto dto)
    {
        var entity = await _workShiftRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("班次信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_work_shift_code_unique = await _uniqueValidator.IsUniqueAsync(
            _workShiftRepository,
            x => x.ShiftCode == entity.ShiftCode,
            id);
        if (!isUnique_ix_work_shift_code_unique)
        {
            throw new TaktBusinessException("班次信息的ShiftCode已存在");
        }
        await _workShiftRepository.UpdateAsync(entity);
        return await GetWorkShiftByIdAsync(id) ?? throw new TaktBusinessException("班次信息不存在");
    }

    /// <summary>
    /// 删除班次信息
    /// </summary>
    /// <param name="id">班次信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkShiftByIdAsync(long id)
    {
        var deleted = await _workShiftRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("班次信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除班次信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteWorkShiftBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteWorkShiftByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新班次信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWorkShiftDto> UpdateWorkShiftSortAsync(TaktWorkShiftSortDto dto)
    {
        var entity = await _workShiftRepository.GetByIdAsync(dto.WorkShiftId);
        if (entity == null)
        {
            throw new TaktBusinessException("班次信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _workShiftRepository.UpdateAsync(entity);
        return await GetWorkShiftByIdAsync(dto.WorkShiftId) ?? throw new TaktBusinessException("班次信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetWorkShiftTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktWorkShiftTemplateDto>(
            sheetName ?? "班次信息导入模板",
            fileName ?? "班次信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入班次信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportWorkShiftAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktWorkShiftImportDto>(fileStream, sheetName ?? "班次信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _workShiftRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktWorkShift>();
                var importKey = $"{entity.ShiftCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ShiftCode）");
                }
                var isUnique_ix_work_shift_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _workShiftRepository,
                    x => x.ShiftCode == entity.ShiftCode);
                if (!isUnique_ix_work_shift_code_unique)
                {
                    throw new TaktBusinessException("班次信息的ShiftCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _workShiftRepository.CreateAsync(entity);
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
    /// 导出班次信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportWorkShiftAsync(TaktWorkShiftQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktWorkShiftQueryDto());
        var list = await _workShiftRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWorkShiftExportDto>(),
                sheetName ?? "班次信息数据",
                fileName ?? "班次信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktWorkShiftExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "班次信息数据",
            fileName ?? "班次信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建班次信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktWorkShift, bool>> QueryExpression(TaktWorkShiftQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktWorkShift>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ShiftCode != null && x.ShiftCode.Contains(keywords))
                || (x.ShiftName != null && x.ShiftName.Contains(keywords))
                || (x.StartTime != null && x.StartTime.Contains(keywords))
                || (x.EndTime != null && x.EndTime.Contains(keywords))
                || SqlFunc.ToString(x.CrossMidnight).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ShiftCode))
        {
            exp = exp.And(x => x.ShiftCode != null && x.ShiftCode.Contains(queryDto.ShiftCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShiftName))
        {
            exp = exp.And(x => x.ShiftName != null && x.ShiftName.Contains(queryDto.ShiftName));
        }

        if (!string.IsNullOrEmpty(queryDto?.StartTime))
        {
            exp = exp.And(x => x.StartTime != null && x.StartTime.Contains(queryDto.StartTime));
        }

        if (!string.IsNullOrEmpty(queryDto?.EndTime))
        {
            exp = exp.And(x => x.EndTime != null && x.EndTime.Contains(queryDto.EndTime));
        }

        if (queryDto?.CrossMidnight.HasValue == true)
        {
            exp = exp.And(x => x.CrossMidnight == queryDto.CrossMidnight);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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
