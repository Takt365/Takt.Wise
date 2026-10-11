// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Attendance
// 文件名称：TaktHolidayService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：假日信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
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
/// 假日信息应用服务
/// </summary>
public class TaktHolidayService : TaktServiceBase, ITaktHolidayService
{
    private readonly ITaktCompanyRepository<TaktHoliday> _holidayRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="holidayRepository">假日信息仓储</param>
    /// <param name="companyRepository">公司仓储（校验租户下公司存在且启用）</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktHolidayService(
        ITaktCompanyRepository<TaktHoliday> holidayRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _holidayRepository = holidayRepository;
        _companyRepository = companyRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取假日信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktHolidayDto>> GetHolidayListAsync(TaktHolidayQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _holidayRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktHolidayDto>.Create(
            data.Adapt<List<TaktHolidayDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktHolidayDto?> GetHolidayByIdAsync(long id)
    {
        var entity = await _holidayRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktHolidayDto>();
    }

    /// <summary>
    /// 获取假日信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetHolidayOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _holidayRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.HolidayName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.HolidayName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建假日信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktHolidayDto> CreateHolidayAsync(TaktHolidayCreateDto dto)
    {
        var entity = dto.Adapt<TaktHoliday>();
        FillHolidayDaysCount(entity);
        var isUnique_ix_holiday_start_end_type_unique = await _uniqueValidator.IsUniqueAsync(
            _holidayRepository,
            x => x.StartDate == entity.StartDate
                && x.EndDate == entity.EndDate
                && x.HolidayType == entity.HolidayType);
        if (!isUnique_ix_holiday_start_end_type_unique)
        {
            throw new TaktBusinessException("假日信息的StartDate、EndDate、HolidayType已存在");
        }
        entity = await _holidayRepository.CreateAsync(entity);
        return await GetHolidayByIdAsync(entity.Id) ?? entity.Adapt<TaktHolidayDto>();
    }

    /// <summary>
    /// 更新假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktHolidayDto> UpdateHolidayAsync(long id, TaktHolidayUpdateDto dto)
    {
        var entity = await _holidayRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("假日信息不存在");
        }
        dto.Adapt(entity);
        FillHolidayDaysCount(entity);
        var isUnique_ix_holiday_start_end_type_unique = await _uniqueValidator.IsUniqueAsync(
            _holidayRepository,
            x => x.StartDate == entity.StartDate
                && x.EndDate == entity.EndDate
                && x.HolidayType == entity.HolidayType,
            id);
        if (!isUnique_ix_holiday_start_end_type_unique)
        {
            throw new TaktBusinessException("假日信息的StartDate、EndDate、HolidayType已存在");
        }
        await _holidayRepository.UpdateAsync(entity);
        return await GetHolidayByIdAsync(id) ?? throw new TaktBusinessException("假日信息不存在");
    }

    /// <summary>
    /// 删除假日信息
    /// </summary>
    /// <param name="id">假日信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteHolidayByIdAsync(long id)
    {
        var deleted = await _holidayRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("假日信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除假日信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteHolidayBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteHolidayByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetHolidayTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktHolidayTemplateDto>(
            sheetName ?? "假日信息导入模板",
            fileName ?? "假日信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入假日信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportHolidayAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktHolidayImportDto>(fileStream, sheetName ?? "假日信息导入模板");
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
                var entity = rows[i].Adapt<TaktHoliday>();
                FillHolidayDaysCount(entity);
                var importKey = $"{entity.StartDate}|{entity.EndDate}|{entity.HolidayType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（StartDate、EndDate、HolidayType）");
                }
                var isUnique_ix_holiday_start_end_type_unique = await _uniqueValidator.IsUniqueAsync(
                    _holidayRepository,
                    x => x.StartDate == entity.StartDate
                        && x.EndDate == entity.EndDate
                        && x.HolidayType == entity.HolidayType);
                if (!isUnique_ix_holiday_start_end_type_unique)
                {
                    throw new TaktBusinessException("假日信息的StartDate、EndDate、HolidayType已存在");
                }
                await _holidayRepository.CreateAsync(entity);
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
    /// 导出假日信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportHolidayAsync(TaktHolidayQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktHolidayQueryDto());
        var list = await _holidayRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktHolidayExportDto>(),
                sheetName ?? "假日信息数据",
                fileName ?? "假日信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktHolidayExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "假日信息数据",
            fileName ?? "假日信息导出.xlsx");
    }

    /// <summary>
    /// 获取服务器当日、指定租户与公司下的假日主题色与问候信息
    /// </summary>
    /// <param name="tenantCode">租户编码（登录页显式传入；禁止依赖配置默认租户）</param>
    /// <param name="companyCode">公司编码（由登录预览语言接口解析后传入）</param>
    /// <returns>假日主题 DTO</returns>
    public async Task<TaktHolidayThemeDto> GetHolidayThemeAsync(string tenantCode, string companyCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            ThrowBusinessException("租户编码不能为空");
        }
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            ThrowBusinessException("公司编码不能为空");
        }
        var effectiveTenant = tenantCode.Trim();
        var effectiveCompany = companyCode.Trim();
        var empty = new TaktHolidayThemeDto();
        var company = await _companyRepository.FirstAsync(c =>
            c.TenantCode == effectiveTenant
            && c.CompanyCode == effectiveCompany
            && c.CompanyStatus == 1);
        if (company == null)
        {
            return empty;
        }
        var today = DateTime.Now.Date;
        var holidays = await _holidayRepository.GetListAsync(
            h => h.TenantCode == effectiveTenant
                && h.CompanyCode == effectiveCompany
                && h.StartDate.Date <= today
                && h.EndDate.Date >= today,
            h => h.StartDate,
            true);
        var holiday = holidays.FirstOrDefault();
        if (holiday == null)
        {
            return empty;
        }
        var dto = holiday.Adapt<TaktHolidayThemeDto>();
        // 命中 StartDate～EndDate 即为放假日；调休上班日在 CompensatoryWorkDates，不在本区间
        dto.IsHolidayToday = !ContainsCompensatoryWorkDate(holiday.CompensatoryWorkDates, today);
        return dto;
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建假日信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktHoliday, bool>> QueryExpression(TaktHolidayQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktHoliday>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.HolidayName != null && x.HolidayName.Contains(keywords))
                || SqlFunc.ToString(x.HolidayType).Contains(keywords)
                || (x.CompensatoryWorkDates != null && x.CompensatoryWorkDates.Contains(keywords))
                || SqlFunc.ToString(x.IsPaid).Contains(keywords)
                || SqlFunc.ToString(x.DaysCount).Contains(keywords)
                || (x.HolidayGreeting != null && x.HolidayGreeting.Contains(keywords))
                || (x.HolidayQuote != null && x.HolidayQuote.Contains(keywords))
                || (x.HolidayTheme != null && x.HolidayTheme.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.HolidayName))
        {
            exp = exp.And(x => x.HolidayName != null && x.HolidayName.Contains(queryDto.HolidayName));
        }

        if (queryDto?.HolidayType.HasValue == true)
        {
            exp = exp.And(x => x.HolidayType == queryDto.HolidayType);
        }

        if (!string.IsNullOrEmpty(queryDto?.CompensatoryWorkDates))
        {
            exp = exp.And(x => x.CompensatoryWorkDates != null && x.CompensatoryWorkDates.Contains(queryDto.CompensatoryWorkDates));
        }

        if (queryDto?.DaysCount.HasValue == true)
        {
            exp = exp.And(x => x.DaysCount == queryDto.DaysCount);
        }

        if (queryDto?.IsPaid.HasValue == true)
        {
            exp = exp.And(x => x.IsPaid == queryDto.IsPaid);
        }

        if (!string.IsNullOrEmpty(queryDto?.HolidayGreeting))
        {
            exp = exp.And(x => x.HolidayGreeting != null && x.HolidayGreeting.Contains(queryDto.HolidayGreeting));
        }

        if (!string.IsNullOrEmpty(queryDto?.HolidayQuote))
        {
            exp = exp.And(x => x.HolidayQuote != null && x.HolidayQuote.Contains(queryDto.HolidayQuote));
        }

        if (!string.IsNullOrEmpty(queryDto?.HolidayTheme))
        {
            exp = exp.And(x => x.HolidayTheme != null && x.HolidayTheme.Contains(queryDto.HolidayTheme));
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

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.EndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndDate >= queryDto.EndDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndDate <= queryDto.EndDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }

    /// <summary>
    /// 由起止日期回填假期天数（含起止日）
    /// </summary>
    /// <param name="entity">假日实体</param>
    private static void FillHolidayDaysCount(TaktHoliday entity)
    {
        var start = entity.StartDate.Date;
        var end = entity.EndDate.Date;
        entity.StartDate = start;
        entity.EndDate = end;
        if (end < start)
        {
            entity.DaysCount = 0;
            return;
        }
        entity.DaysCount = checked(end.Subtract(start).Days + 1);
    }

    /// <summary>
    /// 判断指定日是否为调休上班日（取「上班日=&gt;放假日」左侧；兼容仅上班日）
    /// </summary>
    /// <param name="raw">CompensatoryWorkDates</param>
    /// <param name="date">待判日期</param>
    /// <returns>是否命中</returns>
    private static bool ContainsCompensatoryWorkDate(string? raw, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }
        var key = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        foreach (var part in raw.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var workRaw = part;
            var arrow = part.IndexOf("=>", StringComparison.Ordinal);
            if (arrow >= 0)
            {
                workRaw = part[..arrow].Trim();
            }
            if (string.Equals(workRaw, key, StringComparison.Ordinal))
            {
                return true;
            }
        }
        return false;
    }
}
