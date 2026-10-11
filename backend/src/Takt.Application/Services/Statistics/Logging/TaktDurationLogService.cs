// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktDurationLogService.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线时长日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 在线时长日志应用服务
/// </summary>
public class TaktDurationLogService : TaktServiceBase, ITaktDurationLogService
{
    private readonly ITaktCompanyRepository<TaktDurationLog> _durationLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="durationLogRepository">在线时长日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDurationLogService(
        ITaktCompanyRepository<TaktDurationLog> durationLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _durationLogRepository = durationLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取在线时长日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDurationLogDto>> GetDurationLogListAsync(TaktDurationLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _durationLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDurationLogDto>.Create(
            data.Adapt<List<TaktDurationLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取在线时长日志
    /// </summary>
    /// <param name="id">在线时长日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDurationLogDto?> GetDurationLogByIdAsync(long id)
    {
        var entity = await _durationLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktDurationLogDto>();
    }

    /// <summary>
    /// 获取在线时长日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDurationLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _durationLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.UserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建在线时长日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDurationLogDto> CreateDurationLogAsync(TaktDurationLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktDurationLog>();
        var isUnique_ix_duration_log_user_stat_date_unique = await _uniqueValidator.IsUniqueAsync(
            _durationLogRepository,
            x => x.UserId == entity.UserId
                && x.StatDate == entity.StatDate);
        if (!isUnique_ix_duration_log_user_stat_date_unique)
        {
            throw new TaktBusinessException("在线时长日志的UserId、StatDate已存在");
        }
        entity = await _durationLogRepository.CreateAsync(entity);
        return await GetDurationLogByIdAsync(entity.Id) ?? entity.Adapt<TaktDurationLogDto>();
    }

    /// <summary>
    /// 更新在线时长日志
    /// </summary>
    /// <param name="id">在线时长日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDurationLogDto> UpdateDurationLogAsync(long id, TaktDurationLogUpdateDto dto)
    {
        var entity = await _durationLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("在线时长日志不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_duration_log_user_stat_date_unique = await _uniqueValidator.IsUniqueAsync(
            _durationLogRepository,
            x => x.UserId == entity.UserId
                && x.StatDate == entity.StatDate,
            id);
        if (!isUnique_ix_duration_log_user_stat_date_unique)
        {
            throw new TaktBusinessException("在线时长日志的UserId、StatDate已存在");
        }
        await _durationLogRepository.UpdateAsync(entity);
        return await GetDurationLogByIdAsync(id) ?? throw new TaktBusinessException("在线时长日志不存在");
    }

    /// <summary>
    /// 删除在线时长日志
    /// </summary>
    /// <param name="id">在线时长日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDurationLogByIdAsync(long id)
    {
        var deleted = await _durationLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("在线时长日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除在线时长日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDurationLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDurationLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出在线时长日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDurationLogAsync(TaktDurationLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDurationLogQueryDto());
        var list = await _durationLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDurationLogExportDto>(),
                sheetName ?? "在线时长日志数据",
                fileName ?? "在线时长日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDurationLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "在线时长日志数据",
            fileName ?? "在线时长日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建在线时长日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDurationLog, bool>> QueryExpression(TaktDurationLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDurationLog>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            var keywordsLower = keywords.ToLower();
            exp = exp.And(x =>
                (x.TenantCode != null && x.TenantCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.ToLower().Contains(keywordsLower))
                || (x.CompanyCode != null && x.CompanyCode.ToLower().Contains(keywordsLower))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (queryDto?.UserId.HasValue == true)
        {
            exp = exp.And(x => x.UserId == queryDto.UserId);
        }

        if (queryDto?.DurationSeconds.HasValue == true)
        {
            exp = exp.And(x => x.DurationSeconds == queryDto.DurationSeconds);
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

        if (queryDto?.StatDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StatDate >= queryDto.StatDateStart);
        }

        if (queryDto?.StatDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StatDate <= queryDto.StatDateEnd);
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
            var plantCode = queryDto.PlantCode.Trim();
            exp = exp.And(x =>
                x.PlantCode != null
                && x.PlantCode.ToLower().Contains(plantCode.ToLower()));
        }

        return exp.ToExpression();
    }
}
