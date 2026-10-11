// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktCalendarService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂日历应用服务实现
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
/// 工厂日历应用服务
/// </summary>
public class TaktCalendarService : TaktServiceBase, ITaktCalendarService
{
    private readonly ITaktCompanyRepository<TaktCalendar> _calendarRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="calendarRepository">工厂日历仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCalendarService(
        ITaktCompanyRepository<TaktCalendar> calendarRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _calendarRepository = calendarRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工厂日历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCalendarDto>> GetCalendarListAsync(TaktCalendarQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _calendarRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCalendarDto>.Create(
            data.Adapt<List<TaktCalendarDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工厂日历
    /// </summary>
    /// <param name="id">工厂日历ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCalendarDto?> GetCalendarByIdAsync(long id)
    {
        var entity = await _calendarRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCalendarDto>();
    }

    /// <summary>
    /// 获取工厂日历选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCalendarOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _calendarRepository.GetListAsync(
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
    /// 创建工厂日历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCalendarDto> CreateCalendarAsync(TaktCalendarCreateDto dto)
    {
        var entity = dto.Adapt<TaktCalendar>();
        FillCalendarDateDerivedFields(entity);
        var isUnique_ix_calendar_plant_date_unique = await _uniqueValidator.IsUniqueAsync(
            _calendarRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CalendarDate == entity.CalendarDate);
        if (!isUnique_ix_calendar_plant_date_unique)
        {
            throw new TaktBusinessException("工厂日历的PlantCode、CalendarDate已存在");
        }
        entity = await _calendarRepository.CreateAsync(entity);
        return await GetCalendarByIdAsync(entity.Id) ?? entity.Adapt<TaktCalendarDto>();
    }

    /// <summary>
    /// 更新工厂日历
    /// </summary>
    /// <param name="id">工厂日历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCalendarDto> UpdateCalendarAsync(long id, TaktCalendarUpdateDto dto)
    {
        var entity = await _calendarRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂日历不存在");
        }
        dto.Adapt(entity);
        FillCalendarDateDerivedFields(entity);
        var isUnique_ix_calendar_plant_date_unique = await _uniqueValidator.IsUniqueAsync(
            _calendarRepository,
            x => x.PlantCode == entity.PlantCode
                && x.CalendarDate == entity.CalendarDate,
            id);
        if (!isUnique_ix_calendar_plant_date_unique)
        {
            throw new TaktBusinessException("工厂日历的PlantCode、CalendarDate已存在");
        }
        await _calendarRepository.UpdateAsync(entity);
        return await GetCalendarByIdAsync(id) ?? throw new TaktBusinessException("工厂日历不存在");
    }

    /// <summary>
    /// 删除工厂日历
    /// </summary>
    /// <param name="id">工厂日历ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCalendarByIdAsync(long id)
    {
        var deleted = await _calendarRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工厂日历不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工厂日历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCalendarBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCalendarByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCalendarTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCalendarTemplateDto>(
            sheetName ?? "工厂日历导入模板",
            fileName ?? "工厂日历导入模板.xlsx");
    }

    /// <summary>
    /// 导入工厂日历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCalendarAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCalendarImportDto>(fileStream, sheetName ?? "工厂日历导入模板");
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
                var entity = rows[i].Adapt<TaktCalendar>();
                FillCalendarDateDerivedFields(entity);
                var importKey = $"{entity.PlantCode}|{entity.CalendarDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、CalendarDate）");
                }
                var isUnique_ix_calendar_plant_date_unique = await _uniqueValidator.IsUniqueAsync(
                    _calendarRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.CalendarDate == entity.CalendarDate);
                if (!isUnique_ix_calendar_plant_date_unique)
                {
                    throw new TaktBusinessException("工厂日历的PlantCode、CalendarDate已存在");
                }
                await _calendarRepository.CreateAsync(entity);
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
    /// 导出工厂日历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCalendarAsync(TaktCalendarQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCalendarQueryDto());
        var list = await _calendarRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCalendarExportDto>(),
                sheetName ?? "工厂日历数据",
                fileName ?? "工厂日历导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCalendarExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工厂日历数据",
            fileName ?? "工厂日历导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工厂日历查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCalendar, bool>> QueryExpression(TaktCalendarQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCalendar>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.IsWorkingDay).Contains(keywords)
                || SqlFunc.ToString(x.HolidayId).Contains(keywords)
                || SqlFunc.ToString(x.ShiftId).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CalendarDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.IsWorkingDay.HasValue == true)
        {
            exp = exp.And(x => x.IsWorkingDay == queryDto.IsWorkingDay);
        }

        if (queryDto?.HolidayId.HasValue == true)
        {
            exp = exp.And(x => x.HolidayId == queryDto.HolidayId);
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

        if (queryDto?.CalendarDateStart.HasValue == true)
        {
            exp = exp.And(x => x.CalendarDate >= queryDto.CalendarDateStart);
        }

        if (queryDto?.CalendarDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.CalendarDate <= queryDto.CalendarDateEnd);
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

    /// <summary>
    /// 由 CalendarDate 回填月内日/星期/周次/季度/季内日/年内日（与种子口径一致）
    /// </summary>
    /// <param name="entity">日历实体</param>
    private static void FillCalendarDateDerivedFields(TaktCalendar entity)
    {
        var date = entity.CalendarDate.Date;
        entity.CalendarDate = date;
        entity.DayOfMonth = date.Day;
        var dow = (int)date.DayOfWeek;
        entity.Weekday = dow == 0 ? 7 : dow;
        entity.WeekOfYear = System.Globalization.ISOWeek.GetWeekOfYear(date);
        entity.Quarter = (date.Month - 1) / 3 + 1;
        var quarterStart = new DateTime(date.Year, (entity.Quarter - 1) * 3 + 1, 1);
        entity.DayOfQuarter = (date - quarterStart).Days + 1;
        entity.DayOfYear = date.DayOfYear;
    }
}
