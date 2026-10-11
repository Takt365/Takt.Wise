// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktShiftScheduleService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：排班信息应用服务实现
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
/// 排班信息应用服务
/// </summary>
public class TaktShiftScheduleService : TaktServiceBase, ITaktShiftScheduleService
{
    private readonly ITaktCompanyRepository<TaktShiftSchedule> _shiftScheduleRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="shiftScheduleRepository">排班信息仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktShiftScheduleService(
        ITaktCompanyRepository<TaktShiftSchedule> shiftScheduleRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _shiftScheduleRepository = shiftScheduleRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取排班信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktShiftScheduleDto>> GetShiftScheduleListAsync(TaktShiftScheduleQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _shiftScheduleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktShiftScheduleDto>.Create(
            data.Adapt<List<TaktShiftScheduleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取排班信息
    /// </summary>
    /// <param name="id">排班信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktShiftScheduleDto?> GetShiftScheduleByIdAsync(long id)
    {
        var entity = await _shiftScheduleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktShiftScheduleDto>();
    }

    /// <summary>
    /// 获取排班信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetShiftScheduleOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _shiftScheduleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建排班信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktShiftScheduleDto> CreateShiftScheduleAsync(TaktShiftScheduleCreateDto dto)
    {
        var entity = dto.Adapt<TaktShiftSchedule>();
        entity = await _shiftScheduleRepository.CreateAsync(entity);
        return await GetShiftScheduleByIdAsync(entity.Id) ?? entity.Adapt<TaktShiftScheduleDto>();
    }

    /// <summary>
    /// 更新排班信息
    /// </summary>
    /// <param name="id">排班信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktShiftScheduleDto> UpdateShiftScheduleAsync(long id, TaktShiftScheduleUpdateDto dto)
    {
        var entity = await _shiftScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("排班信息不存在");
        }
        dto.Adapt(entity);
        await _shiftScheduleRepository.UpdateAsync(entity);
        return await GetShiftScheduleByIdAsync(id) ?? throw new TaktBusinessException("排班信息不存在");
    }

    /// <summary>
    /// 删除排班信息
    /// </summary>
    /// <param name="id">排班信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteShiftScheduleByIdAsync(long id)
    {
        var deleted = await _shiftScheduleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("排班信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除排班信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteShiftScheduleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteShiftScheduleByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetShiftScheduleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktShiftScheduleTemplateDto>(
            sheetName ?? "排班信息导入模板",
            fileName ?? "排班信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入排班信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportShiftScheduleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktShiftScheduleImportDto>(fileStream, sheetName ?? "排班信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktShiftSchedule>();
                await _shiftScheduleRepository.CreateAsync(entity);
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
    /// 导出排班信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportShiftScheduleAsync(TaktShiftScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktShiftScheduleQueryDto());
        var list = await _shiftScheduleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktShiftScheduleExportDto>(),
                sheetName ?? "排班信息数据",
                fileName ?? "排班信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktShiftScheduleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "排班信息数据",
            fileName ?? "排班信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建排班信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktShiftSchedule, bool>> QueryExpression(TaktShiftScheduleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktShiftSchedule>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ScheduleType).Contains(keywords)
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.ShiftId).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScheduleDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ScheduleType.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleType == queryDto.ScheduleType);
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.ShiftId.HasValue == true)
        {
            exp = exp.And(x => x.ShiftId == queryDto.ShiftId);
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

        if (queryDto?.ScheduleDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleDate >= queryDto.ScheduleDateStart);
        }

        if (queryDto?.ScheduleDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScheduleDate <= queryDto.ScheduleDateEnd);
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
