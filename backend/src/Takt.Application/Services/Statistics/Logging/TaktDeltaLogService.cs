// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktDeltaLogService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：差异日志应用服务实现
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
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Statistics.Logging;

/// <summary>
/// 差异日志应用服务
/// </summary>
public class TaktDeltaLogService : TaktServiceBase, ITaktDeltaLogService
{
    private readonly ITaktCompanyRepository<TaktDeltaLog> _deltaLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deltaLogRepository">差异日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDeltaLogService(
        ITaktCompanyRepository<TaktDeltaLog> deltaLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _deltaLogRepository = deltaLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取差异日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDeltaLogDto>> GetDeltaLogListAsync(TaktDeltaLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _deltaLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        var dtos = data.Adapt<List<TaktDeltaLogDto>>();
        foreach (var dto in dtos)
        {
            EnrichDeltaLogDto(dto);
        }
        return TaktPagedResult<TaktDeltaLogDto>.Create(
            dtos,
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeltaLogDto?> GetDeltaLogByIdAsync(long id)
    {
        var entity = await _deltaLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDeltaLogDto>();
        EnrichDeltaLogDto(dto);
        return dto;
    }

    /// <summary>
    /// 获取差异日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDeltaLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _deltaLogRepository.GetListAsync(
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
    /// 创建差异日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeltaLogDto> CreateDeltaLogAsync(TaktDeltaLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktDeltaLog>();
        ApplyClientUserAgentProfile(entity);
        entity = await _deltaLogRepository.CreateAsync(entity);
        return await GetDeltaLogByIdAsync(entity.Id) ?? entity.Adapt<TaktDeltaLogDto>();
    }

    /// <summary>
    /// 更新差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDeltaLogDto> UpdateDeltaLogAsync(long id, TaktDeltaLogUpdateDto dto)
    {
        var entity = await _deltaLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("差异日志不存在");
        }
        dto.Adapt(entity);
        ApplyClientUserAgentProfile(entity);
        await _deltaLogRepository.UpdateAsync(entity);
        return await GetDeltaLogByIdAsync(id) ?? throw new TaktBusinessException("差异日志不存在");
    }

    /// <summary>
    /// 删除差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDeltaLogByIdAsync(long id)
    {
        var deleted = await _deltaLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("差异日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除差异日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDeltaLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDeltaLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出差异日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDeltaLogAsync(TaktDeltaLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDeltaLogQueryDto());
        var list = await _deltaLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDeltaLogExportDto>(),
                sheetName ?? "差异日志数据",
                fileName ?? "差异日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDeltaLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "差异日志数据",
            fileName ?? "差异日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建差异日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDeltaLog, bool>> QueryExpression(TaktDeltaLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDeltaLog>();

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
                || (x.OperType != null && x.OperType.Contains(keywords))
                || (x.TableName != null && x.TableName.Contains(keywords))
                || (x.BeforeData != null && x.BeforeData.Contains(keywords))
                || (x.AfterData != null && x.AfterData.Contains(keywords))
                || (x.DiffData != null && x.DiffData.Contains(keywords))
                || (x.SqlStatement != null && x.SqlStatement.Contains(keywords))
                || (x.OperIp != null && x.OperIp.Contains(keywords))
                || (x.OperLocation != null && x.OperLocation.Contains(keywords))
                || (x.UserAgent != null && x.UserAgent.Contains(keywords))
                || (x.Browser != null && x.Browser.Contains(keywords))
                || (x.Os != null && x.Os.Contains(keywords))
                || (x.DeviceType != null && x.DeviceType.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OperType))
        {
            var operType = queryDto.OperType.Trim();
            exp = exp.And(x => x.OperType == operType);
        }

        if (!string.IsNullOrEmpty(queryDto?.TableName))
        {
            exp = exp.And(x => x.TableName != null && x.TableName.Contains(queryDto.TableName));
        }

        if (queryDto?.PrimaryKeyId.HasValue == true)
        {
            exp = exp.And(x => x.PrimaryKeyId == queryDto.PrimaryKeyId);
        }

        if (!string.IsNullOrEmpty(queryDto?.BeforeData))
        {
            exp = exp.And(x => x.BeforeData != null && x.BeforeData.Contains(queryDto.BeforeData));
        }

        if (!string.IsNullOrEmpty(queryDto?.AfterData))
        {
            exp = exp.And(x => x.AfterData != null && x.AfterData.Contains(queryDto.AfterData));
        }

        if (!string.IsNullOrEmpty(queryDto?.DiffData))
        {
            exp = exp.And(x => x.DiffData != null && x.DiffData.Contains(queryDto.DiffData));
        }

        if (!string.IsNullOrEmpty(queryDto?.SqlStatement))
        {
            exp = exp.And(x => x.SqlStatement != null && x.SqlStatement.Contains(queryDto.SqlStatement));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperIp))
        {
            exp = exp.And(x => x.OperIp != null && x.OperIp.Contains(queryDto.OperIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperLocation))
        {
            exp = exp.And(x => x.OperLocation != null && x.OperLocation.Contains(queryDto.OperLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAgent))
        {
            exp = exp.And(x => x.UserAgent != null && x.UserAgent.Contains(queryDto.UserAgent));
        }

        if (!string.IsNullOrEmpty(queryDto?.Browser))
        {
            exp = exp.And(x => x.Browser != null && x.Browser.Contains(queryDto.Browser));
        }

        if (!string.IsNullOrEmpty(queryDto?.Os))
        {
            exp = exp.And(x => x.Os != null && x.Os.Contains(queryDto.Os));
        }

        if (!string.IsNullOrEmpty(queryDto?.DeviceType))
        {
            exp = exp.And(x => x.DeviceType != null && x.DeviceType.Contains(queryDto.DeviceType));
        }

        if (queryDto?.ElapsedTime.HasValue == true)
        {
            exp = exp.And(x => x.ElapsedTime == queryDto.ElapsedTime);
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

        if (queryDto?.OperTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.OperTime >= queryDto.OperTimeStart);
        }

        if (queryDto?.OperTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.OperTime <= queryDto.OperTimeEnd);
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

    /// <summary>
    /// 列表/详情展示时根据 User-Agent 回填 Browser/Os/DeviceType
    /// </summary>
    /// <param name="dto">差异日志 DTO</param>
    private static void EnrichDeltaLogDto(TaktDeltaLogDto dto)
    {
        dto.OperLocation = TaktHttpAuditHelper.ResolveLocationFromIp(dto.OperIp, dto.OperLocation);
        (dto.Browser, dto.Os, dto.DeviceType) = TaktUserAgentHelper.FillMissingFromUserAgent(
            dto.UserAgent,
            dto.Browser ?? TaktConstants.BrowserType.Unknown,
            dto.Os ?? TaktConstants.OperatingSystem.Unknown,
            dto.DeviceType ?? TaktConstants.DeviceType.Unknown);
    }

    /// <summary>
    /// 根据 User-Agent 回填 Browser/Os/DeviceType（显式 unknown 时解析）
    /// </summary>
    /// <param name="entity">差异日志实体</param>
    private static void ApplyClientUserAgentProfile(TaktDeltaLog entity)
    {
        (entity.Browser, entity.Os, entity.DeviceType) = TaktUserAgentHelper.FillMissingFromUserAgent(
            entity.UserAgent,
            entity.Browser,
            entity.Os,
            entity.DeviceType);
    }
}
