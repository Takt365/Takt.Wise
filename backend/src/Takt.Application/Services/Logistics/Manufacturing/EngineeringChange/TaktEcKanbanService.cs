// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKanbanService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板应用服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变看板应用服务
/// </summary>
public class TaktEcKanbanService : TaktServiceBase, ITaktEcKanbanService
{
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecEngRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly TaktEcExecDeptAccess _ecExecDeptAccess;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecEngRepository">设变技术课主表仓储</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecExecDeptAccess">设变部门执行跨表访问</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcKanbanService(
        ITaktCompanyRepository<TaktEcGijutsu> ecEngRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        TaktEcExecDeptAccess ecExecDeptAccess,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecEngRepository = ecEngRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecExecDeptAccess = ecExecDeptAccess;
    }

    /// <summary>
    /// 获取设变看板列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcKanbanDto>> GetEcKanbanListAsync(TaktEcKanbanQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(queryDto);
        var hasComputedFilter = HasComputedFilter(queryDto);
        if (hasComputedFilter)
        {
            var list = await _ecEngRepository.GetListForExportAsync(predicate);
            var rows = new List<TaktEcKanbanDto>();
            foreach (var ec in list)
            {
                rows.Add(await BuildKanbanRowAsync(ec));
            }
            rows = ApplyComputedFilters(rows, queryDto);
            var total = rows.Count;
            var skip = checked((queryDto.PageIndex - 1) * queryDto.PageSize);
            var page = rows.Skip(skip).Take(queryDto.PageSize).ToList();
            return TaktPagedResult<TaktEcKanbanDto>.Create(page, total, queryDto.PageIndex, queryDto.PageSize);
        }
        var (ecs, totalCount) = await _ecEngRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        var pageRows = new List<TaktEcKanbanDto>();
        foreach (var ec in ecs)
        {
            pageRows.Add(await BuildKanbanRowAsync(ec));
        }
        return TaktPagedResult<TaktEcKanbanDto>.Create(pageRows, totalCount, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变主表 ID 获取看板行
    /// </summary>
    /// <param name="ecGijutsuId">设变主表 ID</param>
    /// <returns>看板 DTO</returns>
    public async Task<TaktEcKanbanDto?> GetEcKanbanByEcIdAsync(long ecGijutsuId)
    {
        EnsureThreeLayerContext();
        var ec = await _ecEngRepository.GetByIdAsync(ecGijutsuId);
        if (ec == null || ec.TenantCode != CurrentTenantCode || ec.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return await BuildKanbanRowAsync(ec);
    }

    /// <summary>
    /// 导出设变看板
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcKanbanAsync(
        TaktEcKanbanQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(query ?? new TaktEcKanbanQueryDto());
        var list = await _ecEngRepository.GetListForExportAsync(predicate);
        var rows = new List<TaktEcKanbanDto>();
        foreach (var ec in list)
        {
            rows.Add(await BuildKanbanRowAsync(ec));
        }
        rows = ApplyComputedFilters(rows, query ?? new TaktEcKanbanQueryDto());
        return await TaktExcelHelper.ExportAsync(
            rows,
            sheetName ?? "设变看板",
            fileName ?? "设变看板导出.xlsx");
    }

    /// <summary>
    /// 构建设变看板行
    /// </summary>
    /// <param name="ecEng">设变技术课主表</param>
    /// <returns>看板 DTO</returns>
    private async Task<TaktEcKanbanDto> BuildKanbanRowAsync(TaktEcGijutsu ecEng)
    {
        var dto = ecEng.Adapt<TaktEcKanbanDto>();
        var details = await _ecDetailRepository.GetListAsync(x => x.EcGijutsuId == ecEng.Id);
        dto.DetailCount = details.Count;
        var detailIds = details.Select(x => x.Id).ToList();
        var depts = detailIds.Count == 0
            ? []
            : await _ecExecDeptAccess.ListBaseByEcDetailIdsAsync(detailIds);
        dto.DeptStages = TaktEcDeptCodes.KanbanOrder.Select(code =>
        {
            var matched = depts.Where(d => d.DeptCode == code).ToList();
            return new TaktEcKanbanDeptStageDto
            {
                DeptCode = code,
                TotalCount = details.Count,
                ImplementedCount = matched.Count(d => d.IsImplemented == 1),
            };
        }).ToList();
        var path = TaktEcImplementationPathHelper.Resolve(
            dto.DetailCount,
            dto.DeptStages.Select(s => new TaktEcImplementationStageSnapshot(
                s.DeptCode,
                s.ImplementedCount,
                s.TotalCount)).ToList());
        dto.CurrentDeptCode = path.CurrentDeptCode;
        dto.PendingAtCurrentDeptCount = path.PendingAtCurrentDeptCount;
        dto.ImplementationStatus = path.ImplementationStatus;
        dto.IsOfficiallyCompleted = path.IsOfficiallyCompleted ? 1 : 0;
        return dto;
    }

    /// <summary>
    /// 是否存在实施路径计算型筛选条件
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>是否需要内存过滤</returns>
    private static bool HasComputedFilter(TaktEcKanbanQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        return !string.IsNullOrEmpty(queryDto.CurrentDeptCode)
            || queryDto.ImplementationStatus.HasValue;
    }

    /// <summary>
    /// 按实施路径字段过滤看板行
    /// </summary>
    /// <param name="rows">看板行</param>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>过滤后的行</returns>
    private static List<TaktEcKanbanDto> ApplyComputedFilters(
        List<TaktEcKanbanDto> rows,
        TaktEcKanbanQueryDto queryDto)
    {
        IEnumerable<TaktEcKanbanDto> filtered = rows;
        if (!string.IsNullOrEmpty(queryDto.CurrentDeptCode))
        {
            filtered = filtered.Where(x =>
                string.Equals(x.CurrentDeptCode, queryDto.CurrentDeptCode, StringComparison.OrdinalIgnoreCase));
        }
        if (queryDto.ImplementationStatus.HasValue)
        {
            filtered = filtered.Where(x => x.ImplementationStatus == queryDto.ImplementationStatus.Value);
        }
        return filtered.ToList();
    }

    /// <summary>
    /// 构建看板查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcGijutsu, bool>> QueryExpression(TaktEcKanbanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcGijutsu>();
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcLeader != null && x.EcLeader.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (queryDto?.ChangeStatus.HasValue == true)
        {
            exp = exp.And(x => x.ChangeStatus == queryDto.ChangeStatus);
        }
        if (queryDto?.EcStatus.HasValue == true)
        {
            exp = exp.And(x => x.EcStatus == queryDto.EcStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        return exp.ToExpression();
    }
}
