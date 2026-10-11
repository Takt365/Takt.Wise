// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktOperLogService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：操作日志应用服务实现
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
/// 操作日志应用服务
/// </summary>
public class TaktOperLogService : TaktServiceBase, ITaktOperLogService
{
    private readonly ITaktCompanyRepository<TaktOperLog> _operLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="operLogRepository">操作日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOperLogService(
        ITaktCompanyRepository<TaktOperLog> operLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _operLogRepository = operLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取操作日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOperLogDto>> GetOperLogListAsync(TaktOperLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _operLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        var dtos = data.Adapt<List<TaktOperLogDto>>();
        foreach (var dto in dtos)
        {
            EnrichOperLogDto(dto);
        }
        return TaktPagedResult<TaktOperLogDto>.Create(
            dtos,
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 获取当前登录用户的操作日志列表（分页；强制精确匹配当前 UserName）
    /// </summary>
    /// <param name="queryDto">查询DTO（高级条件可用；UserName 由服务端清空后强制精确匹配当前用户）</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOperLogDto>> GetMyOperLogListAsync(TaktOperLogQueryDto queryDto)
    {
        var userName = CurrentUserName?.Trim();
        if (string.IsNullOrEmpty(userName))
        {
            return TaktPagedResult<TaktOperLogDto>.Create(
                new List<TaktOperLogDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }

        // 禁止客户端通过高级查询 UserName 越权查看他人日志
        queryDto.UserName = null;

        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var basePredicate = QueryExpression(queryDto);
        var userNameLower = userName.ToLowerInvariant();
        var exp = Expressionable.Create<TaktOperLog>();
        exp = exp.And(basePredicate);
        exp = exp.And(x => x.UserName != null && x.UserName.ToLower() == userNameLower);
        var predicate = exp.ToExpression();
        var (data, total) = await _operLogRepository.GetPagedAsync(pageIndex, pageSize, predicate);
        var dtos = data.Adapt<List<TaktOperLogDto>>();
        foreach (var dto in dtos)
        {
            EnrichOperLogDto(dto);
        }

        return TaktPagedResult<TaktOperLogDto>.Create(dtos, total, pageIndex, pageSize);
    }

    /// <summary>
    /// 根据ID获取操作日志
    /// </summary>
    /// <param name="id">操作日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktOperLogDto?> GetOperLogByIdAsync(long id)
    {
        var entity = await _operLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktOperLogDto>();
        EnrichOperLogDto(dto);
        return dto;
    }

    /// <summary>
    /// 获取操作日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOperLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _operLogRepository.GetListAsync(
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
    /// 创建操作日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOperLogDto> CreateOperLogAsync(TaktOperLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktOperLog>();
        ApplyClientUserAgentProfile(entity);
        entity = await _operLogRepository.CreateAsync(entity);
        return await GetOperLogByIdAsync(entity.Id) ?? entity.Adapt<TaktOperLogDto>();
    }

    /// <summary>
    /// 更新操作日志
    /// </summary>
    /// <param name="id">操作日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOperLogDto> UpdateOperLogAsync(long id, TaktOperLogUpdateDto dto)
    {
        var entity = await _operLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("操作日志不存在");
        }
        dto.Adapt(entity);
        ApplyClientUserAgentProfile(entity);
        await _operLogRepository.UpdateAsync(entity);
        return await GetOperLogByIdAsync(id) ?? throw new TaktBusinessException("操作日志不存在");
    }

    /// <summary>
    /// 删除操作日志
    /// </summary>
    /// <param name="id">操作日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteOperLogByIdAsync(long id)
    {
        var deleted = await _operLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("操作日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除操作日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteOperLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteOperLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新操作日志状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOperLogDto> UpdateOperLogStatusAsync(TaktOperLogStatusDto dto)
    {
        var entity = await _operLogRepository.GetByIdAsync(dto.OperLogId);
        if (entity == null)
        {
            throw new TaktBusinessException("操作日志不存在");
        }
        entity.OperStatus = (int)dto.OperStatus;
        await _operLogRepository.UpdateAsync(entity);
        return await GetOperLogByIdAsync(dto.OperLogId) ?? throw new TaktBusinessException("操作日志不存在");
    }

    /// <summary>
    /// 导出操作日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOperLogAsync(TaktOperLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktOperLogQueryDto());
        var list = await _operLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOperLogExportDto>(),
                sheetName ?? "操作日志数据",
                fileName ?? "操作日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktOperLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "操作日志数据",
            fileName ?? "操作日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建操作日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktOperLog, bool>> QueryExpression(TaktOperLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktOperLog>();

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
                || (x.OperModule != null && x.OperModule.Contains(keywords))
                || (x.OperType != null && x.OperType.Contains(keywords))
                || (x.OperMethod != null && x.OperMethod.Contains(keywords))
                || (x.RequestMethod != null && x.RequestMethod.Contains(keywords))
                || (x.OperUrl != null && x.OperUrl.Contains(keywords))
                || (x.RequestParam != null && x.RequestParam.Contains(keywords))
                || (x.JsonResult != null && x.JsonResult.Contains(keywords))
                || (x.ErrorMsg != null && x.ErrorMsg.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.OperModule))
        {
            exp = exp.And(x => x.OperModule != null && x.OperModule.Contains(queryDto.OperModule));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OperType))
        {
            var operType = queryDto.OperType.Trim();
            exp = exp.And(x => x.OperType == operType);
        }

        if (!string.IsNullOrEmpty(queryDto?.OperMethod))
        {
            exp = exp.And(x => x.OperMethod != null && x.OperMethod.Contains(queryDto.OperMethod));
        }

        if (!string.IsNullOrEmpty(queryDto?.RequestMethod))
        {
            exp = exp.And(x => x.RequestMethod != null && x.RequestMethod.Contains(queryDto.RequestMethod));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperUrl))
        {
            exp = exp.And(x => x.OperUrl != null && x.OperUrl.Contains(queryDto.OperUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.RequestParam))
        {
            exp = exp.And(x => x.RequestParam != null && x.RequestParam.Contains(queryDto.RequestParam));
        }

        if (!string.IsNullOrEmpty(queryDto?.JsonResult))
        {
            exp = exp.And(x => x.JsonResult != null && x.JsonResult.Contains(queryDto.JsonResult));
        }

        if (queryDto?.OperStatus.HasValue == true)
        {
            exp = exp.And(x => x.OperStatus == (int)queryDto.OperStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ErrorMsg))
        {
            exp = exp.And(x => x.ErrorMsg != null && x.ErrorMsg.Contains(queryDto.ErrorMsg));
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
    /// <param name="dto">操作日志 DTO</param>
    private static void EnrichOperLogDto(TaktOperLogDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.OperType)
            || string.Equals(dto.OperType, TaktConstants.OperType.Unknown, StringComparison.OrdinalIgnoreCase))
        {
            dto.OperType = TaktHttpAuditHelper.ResolveOperType(dto.RequestMethod, dto.OperUrl);
        }

        if (string.IsNullOrWhiteSpace(dto.OperModule))
        {
            dto.OperModule = TaktHttpAuditHelper.ResolveOperModule(dto.OperUrl);
        }

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
    /// <param name="entity">操作日志实体</param>
    private static void ApplyClientUserAgentProfile(TaktOperLog entity)
    {
        (entity.Browser, entity.Os, entity.DeviceType) = TaktUserAgentHelper.FillMissingFromUserAgent(
            entity.UserAgent,
            entity.Browser,
            entity.Os,
            entity.DeviceType);
    }
}
