// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktQuartzLogService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：任务执行日志应用服务实现
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
/// 任务执行日志应用服务
/// </summary>
public class TaktQuartzLogService : TaktServiceBase, ITaktQuartzLogService
{
    private readonly ITaktCompanyRepository<TaktQuartzLog> _quartzLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="quartzLogRepository">任务执行日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQuartzLogService(
        ITaktCompanyRepository<TaktQuartzLog> quartzLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _quartzLogRepository = quartzLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取任务执行日志列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQuartzLogDto>> GetQuartzLogListAsync(TaktQuartzLogQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQuartzLogDto>.Create(
                new List<TaktQuartzLogDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _quartzLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQuartzLogDto>.Create(
            data.Adapt<List<TaktQuartzLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto?> GetQuartzLogByIdAsync(long id)
    {
        var entity = await _quartzLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQuartzLogDto>();
    }

    /// <summary>
    /// 获取任务执行日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQuartzLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _quartzLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExecuteStatus == 1,
            x => x.TaskName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TaskName,
            DictLabel = e.TaskName,
        }).ToList();
    }

    /// <summary>
    /// 创建任务执行日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto> CreateQuartzLogAsync(TaktQuartzLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktQuartzLog>();
        entity = await _quartzLogRepository.CreateAsync(entity);
        return await GetQuartzLogByIdAsync(entity.Id) ?? entity.Adapt<TaktQuartzLogDto>();
    }

    /// <summary>
    /// 更新任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto> UpdateQuartzLogAsync(long id, TaktQuartzLogUpdateDto dto)
    {
        var entity = await _quartzLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("任务执行日志不存在");
        }
        dto.Adapt(entity);
        await _quartzLogRepository.UpdateAsync(entity);
        return await GetQuartzLogByIdAsync(id) ?? throw new TaktBusinessException("任务执行日志不存在");
    }

    /// <summary>
    /// 删除任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzLogByIdAsync(long id)
    {
        var deleted = await _quartzLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("任务执行日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除任务执行日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQuartzLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新任务执行日志状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzLogDto> UpdateQuartzLogStatusAsync(TaktQuartzLogStatusDto dto)
    {
        var entity = await _quartzLogRepository.GetByIdAsync(dto.QuartzLogId);
        if (entity == null)
        {
            throw new TaktBusinessException("任务执行日志不存在");
        }
        entity.ExecuteStatus = (int)dto.ExecuteStatus;
        await _quartzLogRepository.UpdateAsync(entity);
        return await GetQuartzLogByIdAsync(dto.QuartzLogId) ?? throw new TaktBusinessException("任务执行日志不存在");
    }

    /// <summary>
    /// 导出任务执行日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQuartzLogAsync(TaktQuartzLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQuartzLogQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQuartzLogExportDto>(),
                sheetName ?? "任务执行日志数据",
                fileName ?? "任务执行日志导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _quartzLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQuartzLogExportDto>(),
                sheetName ?? "任务执行日志数据",
                fileName ?? "任务执行日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQuartzLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "任务执行日志数据",
            fileName ?? "任务执行日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建任务执行日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQuartzLog, bool>> QueryExpression(TaktQuartzLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQuartzLog>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            var keywordsLower = keywords.ToLower();
            exp = exp.And(x =>
                (x.TenantCode != null && x.TenantCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.ToLower().Contains(keywordsLower))
                || (x.CompanyCode != null && x.CompanyCode.ToLower().Contains(keywordsLower))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.TaskName != null && x.TaskName.Contains(keywords))
                || (x.JobGroup != null && x.JobGroup.Contains(keywords))
                || (x.TaskType != null && x.TaskType.Contains(keywords))
                || (x.ExecuteParams != null && x.ExecuteParams.Contains(keywords))
                || (x.ExecuteMessage != null && x.ExecuteMessage.Contains(keywords))
                || (x.ErrorInfo != null && x.ErrorInfo.Contains(keywords))
                || (x.ExecuteIp != null && x.ExecuteIp.Contains(keywords))
                || (x.ExecuteHost != null && x.ExecuteHost.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode.Trim();
            exp = exp.And(x =>
                x.PlantCode != null
                && x.PlantCode.ToLower().Contains(plantCode.ToLower()));
        }

        if (queryDto?.QuartzTaskId.HasValue == true)
        {
            var quartzTaskId = queryDto.QuartzTaskId.Value;
            exp = exp.And(x => x.QuartzTaskId == quartzTaskId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskName))
        {
            var taskName = queryDto.TaskName;
            exp = exp.And(x => x.TaskName != null && x.TaskName.Contains(taskName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.JobGroup))
        {
            var jobGroup = queryDto.JobGroup;
            exp = exp.And(x => x.JobGroup != null && x.JobGroup.Contains(jobGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskType))
        {
            var taskType = queryDto.TaskType;
            exp = exp.And(x => x.TaskType != null && x.TaskType.Contains(taskType));
        }

        if (queryDto?.ExecuteDuration.HasValue == true)
        {
            var executeDuration = queryDto.ExecuteDuration.Value;
            exp = exp.And(x => x.ExecuteDuration == executeDuration);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExecuteParams))
        {
            var executeParams = queryDto.ExecuteParams;
            exp = exp.And(x => x.ExecuteParams != null && x.ExecuteParams.Contains(executeParams));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExecuteMessage))
        {
            var executeMessage = queryDto.ExecuteMessage;
            exp = exp.And(x => x.ExecuteMessage != null && x.ExecuteMessage.Contains(executeMessage));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ErrorInfo))
        {
            var errorInfo = queryDto.ErrorInfo;
            exp = exp.And(x => x.ErrorInfo != null && x.ErrorInfo.Contains(errorInfo));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExecuteIp))
        {
            var executeIp = queryDto.ExecuteIp;
            exp = exp.And(x => x.ExecuteIp != null && x.ExecuteIp.Contains(executeIp));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExecuteHost))
        {
            var executeHost = queryDto.ExecuteHost;
            exp = exp.And(x => x.ExecuteHost != null && x.ExecuteHost.Contains(executeHost));
        }

        if (queryDto?.ExecuteStatus.HasValue == true)
        {
            var executeStatus = queryDto.ExecuteStatus.Value;
            exp = exp.And(x => x.ExecuteStatus == (int)executeStatus);
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

        if (queryDto?.ExecuteTimeStart.HasValue == true)
        {
            var executeTimeStart = queryDto.ExecuteTimeStart.Value;
            exp = exp.And(x => x.ExecuteTime >= executeTimeStart);
        }

        if (queryDto?.ExecuteTimeEnd.HasValue == true)
        {
            var executeTimeEnd = queryDto.ExecuteTimeEnd.Value;
            exp = exp.And(x => x.ExecuteTime <= executeTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktQuartzLogQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (queryDto.QuartzTaskId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaskName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.JobGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaskType))
        {
            return true;
        }
        if (queryDto.ExecuteDuration.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExecuteParams))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExecuteMessage))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ErrorInfo))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExecuteIp))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExecuteHost))
        {
            return true;
        }
        if (queryDto.ExecuteStatus.HasValue)
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
        if (queryDto.ExecuteTimeStart.HasValue || queryDto.ExecuteTimeEnd.HasValue)
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
