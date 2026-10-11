// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.Logging
// 文件名称：TaktLoginLogService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：登录日志应用服务实现
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
/// 登录日志应用服务
/// </summary>
public class TaktLoginLogService : TaktServiceBase, ITaktLoginLogService
{
    private readonly ITaktCompanyRepository<TaktLoginLog> _loginLogRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="loginLogRepository">登录日志仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktLoginLogService(
        ITaktCompanyRepository<TaktLoginLog> loginLogRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _loginLogRepository = loginLogRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取登录日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktLoginLogDto>> GetLoginLogListAsync(TaktLoginLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _loginLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        var dtos = data.Adapt<List<TaktLoginLogDto>>();
        foreach (var dto in dtos)
        {
            EnrichLoginLogDto(dto);
        }
        return TaktPagedResult<TaktLoginLogDto>.Create(
            dtos,
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 获取当前登录用户的登录日志列表（分页；强制精确匹配当前 UserName）
    /// </summary>
    /// <param name="queryDto">查询DTO（高级条件可用；UserName 由服务端清空后强制精确匹配当前用户）</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktLoginLogDto>> GetMyLoginLogListAsync(TaktLoginLogQueryDto queryDto)
    {
        var userName = CurrentUserName?.Trim();
        if (string.IsNullOrEmpty(userName))
        {
            return TaktPagedResult<TaktLoginLogDto>.Create(
                new List<TaktLoginLogDto>(),
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
        var exp = Expressionable.Create<TaktLoginLog>();
        exp = exp.And(basePredicate);
        exp = exp.And(x => x.UserName != null && x.UserName.ToLower() == userNameLower);
        var predicate = exp.ToExpression();
        var (data, total) = await _loginLogRepository.GetPagedAsync(pageIndex, pageSize, predicate);
        var dtos = data.Adapt<List<TaktLoginLogDto>>();
        foreach (var dto in dtos)
        {
            EnrichLoginLogDto(dto);
        }

        return TaktPagedResult<TaktLoginLogDto>.Create(dtos, total, pageIndex, pageSize);
    }

    /// <summary>
    /// 根据ID获取登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktLoginLogDto?> GetLoginLogByIdAsync(long id)
    {
        var entity = await _loginLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktLoginLogDto>();
        EnrichLoginLogDto(dto);
        return dto;
    }

    /// <summary>
    /// 获取登录日志选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetLoginLogOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _loginLogRepository.GetListAsync(
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
    /// 创建登录日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLoginLogDto> CreateLoginLogAsync(TaktLoginLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktLoginLog>();
        ApplyClientUserAgentProfile(entity);
        entity = await _loginLogRepository.CreateAsync(entity);
        return await GetLoginLogByIdAsync(entity.Id) ?? entity.Adapt<TaktLoginLogDto>();
    }

    /// <summary>
    /// 更新登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktLoginLogDto> UpdateLoginLogAsync(long id, TaktLoginLogUpdateDto dto)
    {
        var entity = await _loginLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("登录日志不存在");
        }
        dto.Adapt(entity);
        ApplyClientUserAgentProfile(entity);
        await _loginLogRepository.UpdateAsync(entity);
        return await GetLoginLogByIdAsync(id) ?? throw new TaktBusinessException("登录日志不存在");
    }

    /// <summary>
    /// 删除登录日志
    /// </summary>
    /// <param name="id">登录日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteLoginLogByIdAsync(long id)
    {
        var deleted = await _loginLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("登录日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除登录日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteLoginLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteLoginLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出登录日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportLoginLogAsync(TaktLoginLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktLoginLogQueryDto());
        var list = await _loginLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktLoginLogExportDto>(),
                sheetName ?? "登录日志数据",
                fileName ?? "登录日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktLoginLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "登录日志数据",
            fileName ?? "登录日志导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建登录日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktLoginLog, bool>> QueryExpression(TaktLoginLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktLoginLog>();

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
                || (x.LoginType != null && x.LoginType.Contains(keywords))
                || (x.Browser != null && x.Browser.Contains(keywords))
                || (x.Os != null && x.Os.Contains(keywords))
                || (x.UserAgent != null && x.UserAgent.Contains(keywords))
                || (x.LoginResult != null && x.LoginResult.Contains(keywords))
                || (x.LoginMessage != null && x.LoginMessage.Contains(keywords))
                || (x.LoginIp != null && x.LoginIp.Contains(keywords))
                || (x.LoginLocation != null && x.LoginLocation.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LoginType))
        {
            var loginType = queryDto.LoginType.Trim();
            exp = exp.And(x => x.LoginType == loginType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Browser))
        {
            var browser = queryDto.Browser.Trim();
            exp = exp.And(x => x.Browser == browser);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Os))
        {
            var os = queryDto.Os.Trim();
            exp = exp.And(x => x.Os == os);
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAgent))
        {
            exp = exp.And(x => x.UserAgent != null && x.UserAgent.Contains(queryDto.UserAgent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LoginResult))
        {
            var loginResult = queryDto.LoginResult.Trim();
            exp = exp.And(x => x.LoginResult == loginResult);
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginMessage))
        {
            exp = exp.And(x => x.LoginMessage != null && x.LoginMessage.Contains(queryDto.LoginMessage));
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginIp))
        {
            exp = exp.And(x => x.LoginIp != null && x.LoginIp.Contains(queryDto.LoginIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.LoginLocation))
        {
            exp = exp.And(x => x.LoginLocation != null && x.LoginLocation.Contains(queryDto.LoginLocation));
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

        if (queryDto?.LogoutAtStart.HasValue == true)
        {
            exp = exp.And(x => x.LogoutAt >= queryDto.LogoutAtStart);
        }

        if (queryDto?.LogoutAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.LogoutAt <= queryDto.LogoutAtEnd);
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
    /// 列表/详情展示时根据 User-Agent 回填 Browser/Os
    /// </summary>
    /// <param name="dto">登录日志 DTO</param>
    private static void EnrichLoginLogDto(TaktLoginLogDto dto)
    {
        dto.LoginLocation = TaktHttpAuditHelper.ResolveLocationFromIp(dto.LoginIp, dto.LoginLocation);
        (dto.Browser, dto.Os) = TaktUserAgentHelper.FillBrowserOsFromUserAgent(
            dto.UserAgent,
            dto.Browser ?? TaktConstants.BrowserType.Unknown,
            dto.Os ?? TaktConstants.OperatingSystem.Unknown);
    }

    /// <summary>
    /// 根据 User-Agent 回填 Browser/Os（显式 unknown 时解析）
    /// </summary>
    /// <param name="entity">登录日志实体</param>
    private static void ApplyClientUserAgentProfile(TaktLoginLog entity)
    {
        (entity.Browser, entity.Os) = TaktUserAgentHelper.FillBrowserOsFromUserAgent(
            entity.UserAgent,
            entity.Browser,
            entity.Os);
    }
}
