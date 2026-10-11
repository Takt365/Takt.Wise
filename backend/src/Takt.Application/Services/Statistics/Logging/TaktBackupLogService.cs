// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktBackupLogService.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：备份日志应用服务实现
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
/// 备份日志应用服务
/// </summary>
public class TaktBackupLogService : TaktServiceBase, ITaktBackupLogService
{
    private readonly ITaktCompanyRepository<TaktBackupLog> _backupLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="backupLogRepository">备份日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBackupLogService(
        ITaktCompanyRepository<TaktBackupLog> backupLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _backupLogRepository = backupLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取备份日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBackupLogDto>> GetBackupLogListAsync(TaktBackupLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _backupLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBackupLogDto>.Create(
            data.Adapt<List<TaktBackupLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取备份日志
    /// </summary>
    /// <param name="id">备份日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBackupLogDto?> GetBackupLogByIdAsync(long id)
    {
        var entity = await _backupLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBackupLogDto>();
    }

    /// <summary>
    /// 获取备份日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBackupLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _backupLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RunStatus == 1,
            x => x.SourceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrWhiteSpace(e.SourceCode) ? e.TargetName : e.SourceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建备份日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBackupLogDto> CreateBackupLogAsync(TaktBackupLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktBackupLog>();
        entity = await _backupLogRepository.CreateAsync(entity);
        return await GetBackupLogByIdAsync(entity.Id) ?? entity.Adapt<TaktBackupLogDto>();
    }

    /// <summary>
    /// 更新备份日志
    /// </summary>
    /// <param name="id">备份日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBackupLogDto> UpdateBackupLogAsync(long id, TaktBackupLogUpdateDto dto)
    {
        var entity = await _backupLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("备份日志不存在");
        }
        dto.Adapt(entity);
        await _backupLogRepository.UpdateAsync(entity);
        return await GetBackupLogByIdAsync(id) ?? throw new TaktBusinessException("备份日志不存在");
    }

    /// <summary>
    /// 删除备份日志
    /// </summary>
    /// <param name="id">备份日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBackupLogByIdAsync(long id)
    {
        var deleted = await _backupLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("备份日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除备份日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBackupLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBackupLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新备份日志状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBackupLogDto> UpdateBackupLogStatusAsync(TaktBackupLogStatusDto dto)
    {
        var entity = await _backupLogRepository.GetByIdAsync(dto.BackupLogId);
        if (entity == null)
        {
            throw new TaktBusinessException("备份日志不存在");
        }
        entity.RunStatus = dto.RunStatus;
        await _backupLogRepository.UpdateAsync(entity);
        return await GetBackupLogByIdAsync(dto.BackupLogId) ?? throw new TaktBusinessException("备份日志不存在");
    }

    /// <summary>
    /// 导出备份日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBackupLogAsync(TaktBackupLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBackupLogQueryDto());
        var list = await _backupLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBackupLogExportDto>(),
                sheetName ?? "备份日志数据",
                fileName ?? "备份日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBackupLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "备份日志数据",
            fileName ?? "备份日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建备份日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBackupLog, bool>> QueryExpression(TaktBackupLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBackupLog>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            var keywordsLower = keywords.ToLower();
            exp = exp.And(x =>
                (x.TenantCode != null && x.TenantCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.ToLower().Contains(keywordsLower))
                || (x.CompanyCode != null && x.CompanyCode.ToLower().Contains(keywordsLower))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.BackupKind != null && x.BackupKind.Contains(keywords))
                || (x.SourceId != null && x.SourceId.Contains(keywords))
                || (x.SourceCode != null && x.SourceCode.Contains(keywords))
                || (x.TargetName != null && x.TargetName.Contains(keywords))
                || (x.TargetScope != null && x.TargetScope.Contains(keywords))
                || (x.ResultPath != null && x.ResultPath.Contains(keywords))
                || (x.ErrorMessage != null && x.ErrorMessage.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.BackupKind))
        {
            exp = exp.And(x => x.BackupKind != null && x.BackupKind.Contains(queryDto.BackupKind));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceId))
        {
            exp = exp.And(x => x.SourceId != null && x.SourceId.Contains(queryDto.SourceId));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCode))
        {
            exp = exp.And(x => x.SourceCode != null && x.SourceCode.Contains(queryDto.SourceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetName))
        {
            exp = exp.And(x => x.TargetName != null && x.TargetName.Contains(queryDto.TargetName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetScope))
        {
            exp = exp.And(x => x.TargetScope != null && x.TargetScope.Contains(queryDto.TargetScope));
        }

        if (queryDto?.SyncMode.HasValue == true)
        {
            exp = exp.And(x => x.SyncMode == queryDto.SyncMode);
        }

        if (queryDto?.ExecuteMode.HasValue == true)
        {
            exp = exp.And(x => x.ExecuteMode == queryDto.ExecuteMode);
        }

        if (queryDto?.PathType.HasValue == true)
        {
            exp = exp.And(x => x.PathType == queryDto.PathType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ResultPath))
        {
            exp = exp.And(x => x.ResultPath != null && x.ResultPath.Contains(queryDto.ResultPath));
        }

        if (queryDto?.FileSizeBytes.HasValue == true)
        {
            exp = exp.And(x => x.FileSizeBytes == queryDto.FileSizeBytes);
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
