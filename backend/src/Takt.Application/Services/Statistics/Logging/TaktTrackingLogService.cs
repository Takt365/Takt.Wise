// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktTrackingLogService.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：交互日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using FluentValidation;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 交互日志应用服务
/// </summary>
public class TaktTrackingLogService : TaktServiceBase, ITaktTrackingLogService
{
    /// <summary>
    /// 客户端单次批量上报上限
    /// </summary>
    private const int MaxTrackBatchSize = 50;

    private readonly ITaktCompanyRepository<TaktTrackingLog> _trackingLogRepository;
    private readonly IValidator<TaktTrackingLogBatchTrackDto> _batchTrackValidator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trackingLogRepository">交互日志仓储</param>
    /// <param name="batchTrackValidator">批量上报验证器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTrackingLogService(
        ITaktCompanyRepository<TaktTrackingLog> trackingLogRepository,
        IValidator<TaktTrackingLogBatchTrackDto> batchTrackValidator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _trackingLogRepository = trackingLogRepository;
        _batchTrackValidator = batchTrackValidator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取交互日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTrackingLogDto>> GetTrackingLogListAsync(TaktTrackingLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _trackingLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTrackingLogDto>.Create(
            data.Adapt<List<TaktTrackingLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrackingLogDto?> GetTrackingLogByIdAsync(long id)
    {
        var entity = await _trackingLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTrackingLogDto>();
    }

    /// <summary>
    /// 获取交互日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTrackingLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _trackingLogRepository.GetListAsync(
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
    /// 创建交互日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrackingLogDto> CreateTrackingLogAsync(TaktTrackingLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktTrackingLog>();
        entity = await _trackingLogRepository.CreateAsync(entity);
        return await GetTrackingLogByIdAsync(entity.Id) ?? entity.Adapt<TaktTrackingLogDto>();
    }

    /// <summary>
    /// 更新交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrackingLogDto> UpdateTrackingLogAsync(long id, TaktTrackingLogUpdateDto dto)
    {
        var entity = await _trackingLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("交互日志不存在");
        }
        dto.Adapt(entity);
        await _trackingLogRepository.UpdateAsync(entity);
        return await GetTrackingLogByIdAsync(id) ?? throw new TaktBusinessException("交互日志不存在");
    }

    /// <summary>
    /// 删除交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTrackingLogByIdAsync(long id)
    {
        var deleted = await _trackingLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("交互日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除交互日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTrackingLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTrackingLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出交互日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTrackingLogAsync(TaktTrackingLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTrackingLogQueryDto());
        var list = await _trackingLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTrackingLogExportDto>(),
                sheetName ?? "交互日志数据",
                fileName ?? "交互日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTrackingLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "交互日志数据",
            fileName ?? "交互日志导出.xlsx");
    }

    /// <summary>
    /// 批量上报 Long Task 等客户端性能事件
    /// </summary>
    /// <param name="dto">批量上报 DTO</param>
    /// <param name="clientIp">客户端 IP</param>
    /// <returns>成功写入条数</returns>
    public async Task<int> TrackTrackingLogBatchAsync(TaktTrackingLogBatchTrackDto dto, string? clientIp)
    {
        EnsureThreeLayerContext();
        if (!IsAuthenticated || CurrentUserId is not > 0 || string.IsNullOrWhiteSpace(CurrentUserName))
        {
            throw new TaktBusinessException("未登录，无法上报交互日志");
        }

        var validation = await _batchTrackValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            throw new TaktBusinessException(validation.Errors[0].ErrorMessage);
        }

        var items = dto.Items.Take(MaxTrackBatchSize).ToList();
        if (items.Count == 0)
        {
            return 0;
        }

        var normalizedIp = string.IsNullOrWhiteSpace(clientIp) ? string.Empty : clientIp.Trim();
        if (normalizedIp.Length > 50)
        {
            normalizedIp = normalizedIp[..50];
        }

        var entities = items.Select(item => new TaktTrackingLog
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            UserName = CurrentUserName.Trim(),
            UserId = CurrentUserId.Value,
            EventTrackingType = string.IsNullOrWhiteSpace(item.EventTrackingType) ? "longtask" : item.EventTrackingType.Trim(),
            EventTrackingCategory = string.IsNullOrWhiteSpace(item.EventTrackingCategory) ? "performance" : item.EventTrackingCategory.Trim(),
            EventTime = NormalizeClientEventTime(item.EventTime),
            DurationMs = item.DurationMs,
            PerformanceStartMs = item.PerformanceStartMs,
            EntryName = item.EntryName ?? string.Empty,
            TrackingLevel = item.TrackingLevel,
            RoutePath = item.RoutePath ?? string.Empty,
            PageUrl = item.PageUrl ?? string.Empty,
            ContainerType = item.ContainerType ?? string.Empty,
            ContainerName = item.ContainerName ?? string.Empty,
            ContainerSrc = item.ContainerSrc ?? string.Empty,
            ContainerId = item.ContainerId ?? string.Empty,
            AttributionJson = string.IsNullOrWhiteSpace(item.AttributionJson) ? "[]" : item.AttributionJson,
            UserAgent = item.UserAgent ?? string.Empty,
            ClientIp = normalizedIp,
        }).ToList();

        await _trackingLogRepository.CreateRangeAsync(entities);
        return entities.Count;
    }

    /// <summary>
    /// 规范化客户端上报时间为 UTC
    /// </summary>
    /// <param name="eventTime">客户端时间</param>
    /// <returns>UTC 时间</returns>
    private static DateTime NormalizeClientEventTime(DateTime eventTime)
    {
        return eventTime.Kind switch
        {
            DateTimeKind.Utc => eventTime,
            DateTimeKind.Local => eventTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(eventTime, DateTimeKind.Utc),
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建交互日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTrackingLog, bool>> QueryExpression(TaktTrackingLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTrackingLog>();

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
                || (x.EventTrackingType != null && x.EventTrackingType.Contains(keywords))
                || (x.EventTrackingCategory != null && x.EventTrackingCategory.Contains(keywords))
                || (x.EntryName != null && x.EntryName.Contains(keywords))
                || (x.RoutePath != null && x.RoutePath.Contains(keywords))
                || (x.PageUrl != null && x.PageUrl.Contains(keywords))
                || (x.ContainerType != null && x.ContainerType.Contains(keywords))
                || (x.ContainerName != null && x.ContainerName.Contains(keywords))
                || (x.ContainerSrc != null && x.ContainerSrc.Contains(keywords))
                || (x.ContainerId != null && x.ContainerId.Contains(keywords))
                || (x.AttributionJson != null && x.AttributionJson.Contains(keywords))
                || (x.UserAgent != null && x.UserAgent.Contains(keywords))
                || (x.ClientIp != null && x.ClientIp.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.EventTrackingType))
        {
            exp = exp.And(x => x.EventTrackingType != null && x.EventTrackingType.Contains(queryDto.EventTrackingType));
        }

        if (!string.IsNullOrEmpty(queryDto?.EventTrackingCategory))
        {
            exp = exp.And(x => x.EventTrackingCategory != null && x.EventTrackingCategory.Contains(queryDto.EventTrackingCategory));
        }

        if (queryDto?.DurationMs.HasValue == true)
        {
            exp = exp.And(x => x.DurationMs == queryDto.DurationMs);
        }

        if (queryDto?.PerformanceStartMs.HasValue == true)
        {
            exp = exp.And(x => x.PerformanceStartMs == queryDto.PerformanceStartMs);
        }

        if (!string.IsNullOrEmpty(queryDto?.EntryName))
        {
            exp = exp.And(x => x.EntryName != null && x.EntryName.Contains(queryDto.EntryName));
        }

        if (queryDto?.TrackingLevel.HasValue == true)
        {
            exp = exp.And(x => x.TrackingLevel == queryDto.TrackingLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutePath))
        {
            exp = exp.And(x => x.RoutePath != null && x.RoutePath.Contains(queryDto.RoutePath));
        }

        if (!string.IsNullOrEmpty(queryDto?.PageUrl))
        {
            exp = exp.And(x => x.PageUrl != null && x.PageUrl.Contains(queryDto.PageUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContainerType))
        {
            exp = exp.And(x => x.ContainerType != null && x.ContainerType.Contains(queryDto.ContainerType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContainerName))
        {
            exp = exp.And(x => x.ContainerName != null && x.ContainerName.Contains(queryDto.ContainerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContainerSrc))
        {
            exp = exp.And(x => x.ContainerSrc != null && x.ContainerSrc.Contains(queryDto.ContainerSrc));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContainerId))
        {
            exp = exp.And(x => x.ContainerId != null && x.ContainerId.Contains(queryDto.ContainerId));
        }

        if (!string.IsNullOrEmpty(queryDto?.AttributionJson))
        {
            exp = exp.And(x => x.AttributionJson != null && x.AttributionJson.Contains(queryDto.AttributionJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAgent))
        {
            exp = exp.And(x => x.UserAgent != null && x.UserAgent.Contains(queryDto.UserAgent));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientIp))
        {
            exp = exp.And(x => x.ClientIp != null && x.ClientIp.Contains(queryDto.ClientIp));
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

        if (queryDto?.EventTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.EventTime >= queryDto.EventTimeStart);
        }

        if (queryDto?.EventTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.EventTime <= queryDto.EventTimeEnd);
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
