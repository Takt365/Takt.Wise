// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktArchiveLogService.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：归档日志应用服务实现
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
/// 归档日志应用服务
/// </summary>
public class TaktArchiveLogService : TaktServiceBase, ITaktArchiveLogService
{
    private readonly ITaktCompanyRepository<TaktArchiveLog> _archiveLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="archiveLogRepository">归档日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktArchiveLogService(
        ITaktCompanyRepository<TaktArchiveLog> archiveLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _archiveLogRepository = archiveLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取归档日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktArchiveLogDto>> GetArchiveLogListAsync(TaktArchiveLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _archiveLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktArchiveLogDto>.Create(
            data.Adapt<List<TaktArchiveLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取归档日志
    /// </summary>
    /// <param name="id">归档日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktArchiveLogDto?> GetArchiveLogByIdAsync(long id)
    {
        var entity = await _archiveLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktArchiveLogDto>();
    }

    /// <summary>
    /// 获取归档日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetArchiveLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _archiveLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RunStatus == 1,
            x => x.SourceName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrWhiteSpace(e.SourceName) ? e.SourceId : e.SourceName,
        }).ToList();
    }

    /// <summary>
    /// 创建归档日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktArchiveLogDto> CreateArchiveLogAsync(TaktArchiveLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktArchiveLog>();
        entity = await _archiveLogRepository.CreateAsync(entity);
        return await GetArchiveLogByIdAsync(entity.Id) ?? entity.Adapt<TaktArchiveLogDto>();
    }

    /// <summary>
    /// 更新归档日志
    /// </summary>
    /// <param name="id">归档日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktArchiveLogDto> UpdateArchiveLogAsync(long id, TaktArchiveLogUpdateDto dto)
    {
        var entity = await _archiveLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("归档日志不存在");
        }
        dto.Adapt(entity);
        await _archiveLogRepository.UpdateAsync(entity);
        return await GetArchiveLogByIdAsync(id) ?? throw new TaktBusinessException("归档日志不存在");
    }

    /// <summary>
    /// 删除归档日志
    /// </summary>
    /// <param name="id">归档日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteArchiveLogByIdAsync(long id)
    {
        var deleted = await _archiveLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("归档日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除归档日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteArchiveLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteArchiveLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新归档日志状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktArchiveLogDto> UpdateArchiveLogStatusAsync(TaktArchiveLogStatusDto dto)
    {
        var entity = await _archiveLogRepository.GetByIdAsync(dto.ArchiveLogId);
        if (entity == null)
        {
            throw new TaktBusinessException("归档日志不存在");
        }
        entity.RunStatus = dto.RunStatus;
        await _archiveLogRepository.UpdateAsync(entity);
        return await GetArchiveLogByIdAsync(dto.ArchiveLogId) ?? throw new TaktBusinessException("归档日志不存在");
    }

    /// <summary>
    /// 导出归档日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportArchiveLogAsync(TaktArchiveLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktArchiveLogQueryDto());
        var list = await _archiveLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktArchiveLogExportDto>(),
                sheetName ?? "归档日志数据",
                fileName ?? "归档日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktArchiveLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "归档日志数据",
            fileName ?? "归档日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建归档日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktArchiveLog, bool>> QueryExpression(TaktArchiveLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktArchiveLog>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            var keywordsLower = keywords.ToLower();
            exp = exp.And(x =>
                (x.TenantCode != null && x.TenantCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.ToLower().Contains(keywordsLower))
                || (x.CompanyCode != null && x.CompanyCode.ToLower().Contains(keywordsLower))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ArchiveKind != null && x.ArchiveKind.Contains(keywords))
                || (x.SourceId != null && x.SourceId.Contains(keywords))
                || (x.SourceName != null && x.SourceName.Contains(keywords))
                || (x.TargetName != null && x.TargetName.Contains(keywords))
                || (x.ErrorMessage != null && x.ErrorMessage.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ArchiveKind))
        {
            exp = exp.And(x => x.ArchiveKind != null && x.ArchiveKind.Contains(queryDto.ArchiveKind));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceId))
        {
            exp = exp.And(x => x.SourceId != null && x.SourceId.Contains(queryDto.SourceId));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceName))
        {
            exp = exp.And(x => x.SourceName != null && x.SourceName.Contains(queryDto.SourceName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetName))
        {
            exp = exp.And(x => x.TargetName != null && x.TargetName.Contains(queryDto.TargetName));
        }

        if (queryDto?.ArchiveYear.HasValue == true)
        {
            exp = exp.And(x => x.ArchiveYear == queryDto.ArchiveYear);
        }

        if (queryDto?.SourceCount.HasValue == true)
        {
            exp = exp.And(x => x.SourceCount == queryDto.SourceCount);
        }

        if (queryDto?.ArchivedCount.HasValue == true)
        {
            exp = exp.And(x => x.ArchivedCount == queryDto.ArchivedCount);
        }

        if (queryDto?.DeletedCount.HasValue == true)
        {
            exp = exp.And(x => x.DeletedCount == queryDto.DeletedCount);
        }

        if (queryDto?.RunStatus.HasValue == true)
        {
            exp = exp.And(x => x.RunStatus == queryDto.RunStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ErrorMessage))
        {
            exp = exp.And(x => x.ErrorMessage != null && x.ErrorMessage.Contains(queryDto.ErrorMessage));
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

        if (queryDto?.StartedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.StartedAt >= queryDto.StartedAtStart);
        }

        if (queryDto?.StartedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartedAt <= queryDto.StartedAtEnd);
        }

        if (queryDto?.FinishedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.FinishedAt >= queryDto.FinishedAtStart);
        }

        if (queryDto?.FinishedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.FinishedAt <= queryDto.FinishedAtEnd);
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
