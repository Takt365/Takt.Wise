// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktVisitLogService.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：用户日访问量应用服务实现
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
/// 用户日访问量应用服务
/// </summary>
public class TaktVisitLogService : TaktServiceBase, ITaktVisitLogService
{
    private readonly ITaktCompanyRepository<TaktVisitLog> _visitLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="visitLogRepository">用户日访问量仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktVisitLogService(
        ITaktCompanyRepository<TaktVisitLog> visitLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _visitLogRepository = visitLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取用户日访问量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktVisitLogDto>> GetVisitLogListAsync(TaktVisitLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _visitLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktVisitLogDto>.Create(
            data.Adapt<List<TaktVisitLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitLogDto?> GetVisitLogByIdAsync(long id)
    {
        var entity = await _visitLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktVisitLogDto>();
    }

    /// <summary>
    /// 获取用户日访问量选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetVisitLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _visitLogRepository.GetListAsync(
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
    /// 创建用户日访问量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitLogDto> CreateVisitLogAsync(TaktVisitLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktVisitLog>();
        var isUnique_ix_visit_log_user_stat_date_unique = await _uniqueValidator.IsUniqueAsync(
            _visitLogRepository,
            x => x.UserId == entity.UserId
                && x.StatDate == entity.StatDate);
        if (!isUnique_ix_visit_log_user_stat_date_unique)
        {
            throw new TaktBusinessException("用户日访问量的UserId、StatDate已存在");
        }
        entity = await _visitLogRepository.CreateAsync(entity);
        return await GetVisitLogByIdAsync(entity.Id) ?? entity.Adapt<TaktVisitLogDto>();
    }

    /// <summary>
    /// 更新用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitLogDto> UpdateVisitLogAsync(long id, TaktVisitLogUpdateDto dto)
    {
        var entity = await _visitLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("用户日访问量不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_visit_log_user_stat_date_unique = await _uniqueValidator.IsUniqueAsync(
            _visitLogRepository,
            x => x.UserId == entity.UserId
                && x.StatDate == entity.StatDate,
            id);
        if (!isUnique_ix_visit_log_user_stat_date_unique)
        {
            throw new TaktBusinessException("用户日访问量的UserId、StatDate已存在");
        }
        await _visitLogRepository.UpdateAsync(entity);
        return await GetVisitLogByIdAsync(id) ?? throw new TaktBusinessException("用户日访问量不存在");
    }

    /// <summary>
    /// 删除用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <returns>任务</returns>
    public async Task DeleteVisitLogByIdAsync(long id)
    {
        var deleted = await _visitLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("用户日访问量不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除用户日访问量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteVisitLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteVisitLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出用户日访问量
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportVisitLogAsync(TaktVisitLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktVisitLogQueryDto());
        var list = await _visitLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktVisitLogExportDto>(),
                sheetName ?? "用户日访问量数据",
                fileName ?? "用户日访问量导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktVisitLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "用户日访问量数据",
            fileName ?? "用户日访问量导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建用户日访问量查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktVisitLog, bool>> QueryExpression(TaktVisitLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktVisitLog>();

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

        if (queryDto?.VisitCount.HasValue == true)
        {
            exp = exp.And(x => x.VisitCount == queryDto.VisitCount);
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
