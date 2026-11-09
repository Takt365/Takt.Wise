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
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities;
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
    /// <summary>
    /// 每个部门最多带回的执行行。查询停在这个上限，不随明细行数增长
    /// </summary>
    private const int KanbanDeptLineCap = 300;

    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;
    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecTeRepository">设变技术课主表仓储</param>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcKanbanService(
        ITaktCompanyRepository<TaktEcTe> ecTeRepository,
        ISqlSugarClient db,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecTeRepository = ecTeRepository;
        _db = db;
    }

    /// <summary>
    /// 获取设变看板列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcKanbanDto>> GetEcKanbanListAsync(TaktEcKanbanQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var (masters, total) = await _ecTeRepository.GetPagedAsync(
            ListExpression(queryDto),
            pageIndex,
            pageSize,
            x => x.EcIssueDate,
            true,
            thenBy: x => x.EcCode,
            thenByDesc: false);
        var page = await BuildRowsAsync(masters);
        FillIdentityFromPcLines(page);
        return TaktPagedResult<TaktEcKanbanDto>.Create(page, total, pageIndex, pageSize);
    }

    /// <summary>
    /// 根据设变主表 ID 获取看板行
    /// </summary>
    /// <param name="ecTeId">设变主表 ID</param>
    /// <returns>看板 DTO</returns>
    public async Task<TaktEcKanbanDto?> GetEcKanbanByEcIdAsync(long ecTeId)
    {
        EnsureThreeLayerContext();
        var ec = await _ecTeRepository.GetByIdAsync(ecTeId);
        if (ec == null || ec.TenantCode != CurrentTenantCode || ec.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var rows = await BuildRowsAsync([ec]);
        var row = rows.FirstOrDefault();
        if (row != null)
        {
            FillIdentityFromPcLines([row]);
        }
        return row;
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
        var masters = await _ecTeRepository.GetListAsync(ListExpression(query ?? new TaktEcKanbanQueryDto()));
        var rows = await BuildRowsAsync(masters);
        rows = rows
            .OrderByDescending(x => x.EcIssueDate)
            .ThenBy(x => x.EcCode, StringComparer.Ordinal)
            .ToList();
        FillIdentityFromPcLines(rows);
        return await TaktExcelHelper.ExportAsync(
            rows.SelectMany(ExpandExportLines).ToList(),
            sheetName ?? "设变看板",
            fileName ?? "设变看板导出.xlsx");
    }

    /// <summary>
    /// 获取设变看板统计。主表按实施状态计数；明细/部门执行仅计 FERT 根物料且停产状态=计划物料（Z0）；录入期间按月计数
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>看板统计</returns>
    public async Task<TaktEcKanbanBoardStatDto> GetEcKanbanBoardStatAsync(TaktEcKanbanQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var baseQuery = queryDto ?? new TaktEcKanbanQueryDto();
        var notStarted = await CountByStatusAsync(baseQuery, TaktEcImplementationStatusConstants.NotStarted);
        var inProgress = await CountByStatusAsync(baseQuery, TaktEcImplementationStatusConstants.InProgress);
        var officially = await CountByStatusAsync(baseQuery, TaktEcImplementationStatusConstants.OfficiallyCompleted);
        var fully = await CountByStatusAsync(baseQuery, TaktEcImplementationStatusConstants.FullyCompleted);
        // 旭日图：内围主表、中围明细、外围各部门执行行 —— 均限定在当前查询主表设变号内
        var masterCodes = await LoadBoardMasterEcCodesAsync(baseQuery);
        var fertPlannedRootsByEc = await LoadFertPlannedRootCodesByEcAsync(masterCodes);
        var detailTotal = await CountFertPlannedDetailRowsByEcCodesAsync(masterCodes);
        var depts = new List<TaktEcKanbanDeptStatDto>
        {
            await CountDeptBoardByCodesAsync<TaktEcPc>(TaktEcDeptCodes.Pc, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcMfg2>(TaktEcDeptCodes.Mfg2, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcMfg1>(TaktEcDeptCodes.Mfg1, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcQc>(TaktEcDeptCodes.Qc, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcPur>(TaktEcDeptCodes.Pur, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcIqc>(TaktEcDeptCodes.Iqc, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcMc>(TaktEcDeptCodes.Mc, masterCodes, fertPlannedRootsByEc),
            await CountDeptBoardByCodesAsync<TaktEcMtl>(TaktEcDeptCodes.Mtl, masterCodes, fertPlannedRootsByEc),
        };
        return new TaktEcKanbanBoardStatDto
        {
            EcTotal = notStarted + inProgress + officially + fully,
            DetailTotal = detailTotal,
            NotStarted = notStarted,
            InProgress = inProgress,
            OfficiallyCompleted = officially,
            FullyCompleted = fully,
            Depts = depts,
            Months = await CountMonthsAsync(baseQuery),
        };
    }

    /// <summary>
    /// 当前查询条件下的主表设变单号（不含实施状态过滤，与 EcTotal 同源）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>去重后的设变单号</returns>
    private async Task<List<string>> LoadBoardMasterEcCodesAsync(TaktEcKanbanQueryDto queryDto)
    {
        var dto = new TaktEcKanbanQueryDto
        {
            KeyWords = queryDto.KeyWords,
            EcCode = queryDto.EcCode,
            ChangeStatus = queryDto.ChangeStatus,
            EcStatus = queryDto.EcStatus,
            CultureCode = queryDto.CultureCode,
            EcEntryDateStart = queryDto.EcEntryDateStart,
            EcEntryDateEnd = queryDto.EcEntryDateEnd,
        };
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var codes = await _db.Queryable<TaktEcTe>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0)
            .Where(ListExpression(dto))
            .Select(x => x.EcCode)
            .ToListAsync();
        return codes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>
    /// 明细行数：仅 FERT 根物料且停产状态为计划物料（空或 Z0）
    /// </summary>
    /// <param name="ecCodes">设变单号</param>
    /// <returns>行数</returns>
    private async Task<int> CountFertPlannedDetailRowsByEcCodesAsync(IReadOnlyList<string> ecCodes)
    {
        if (ecCodes.Count == 0)
        {
            return 0;
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var fert = TaktEcMfg1Constants.FertMaterialType;
        var planned = TaktEcScopeConstants.PlannedMaterialStatus;
        return await _db.Queryable<TaktEcDetail>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && ecCodes.Contains(x.EcCode)
                && x.IsDeleted == 0
                && x.IsObsolete == 0
                && x.EcRootMaterialType == fert
                && (x.DiscontinuedStatus == null
                    || x.DiscontinuedStatus == ""
                    || x.DiscontinuedStatus == planned))
            .CountAsync();
    }

    /// <summary>
    /// 按设变号加载 FERT 且计划物料（Z0）的根物料编码集合（看板统计/过滤共用）
    /// </summary>
    /// <param name="ecCodes">设变单号</param>
    /// <returns>EcCode → 根物料编码集合</returns>
    private async Task<Dictionary<string, HashSet<string>>> LoadFertPlannedRootCodesByEcAsync(
        IReadOnlyList<string> ecCodes)
    {
        var result = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        if (ecCodes.Count == 0)
        {
            return result;
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var fert = TaktEcMfg1Constants.FertMaterialType;
        var planned = TaktEcScopeConstants.PlannedMaterialStatus;
        var rows = await _db.Queryable<TaktEcDetail>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && ecCodes.Contains(x.EcCode)
                && x.IsDeleted == 0
                && x.IsObsolete == 0
                && x.EcRootMaterialType == fert
                && (x.DiscontinuedStatus == null
                    || x.DiscontinuedStatus == ""
                    || x.DiscontinuedStatus == planned))
            .Select(x => new { x.EcCode, x.EcRootMaterialCode })
            .ToListAsync();
        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.EcCode) || string.IsNullOrWhiteSpace(row.EcRootMaterialCode))
            {
                continue;
            }
            if (!result.TryGetValue(row.EcCode, out var set))
            {
                set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                result[row.EcCode] = set;
            }
            set.Add(row.EcRootMaterialCode.Trim());
        }
        return result;
    }

    /// <summary>
    /// 部门统计：仅 FERT+计划物料执行行；行级已实施/未实施 + 设变单级未开始/实施中/完成
    /// （完成 = 该部门在该设变下有执行行且明细全部已实施）
    /// </summary>
    /// <typeparam name="TEntity">部门执行实体</typeparam>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecCodes">当前查询主表设变号</param>
    /// <param name="fertPlannedRootsByEc">FERT 计划物料根编码</param>
    /// <returns>部门统计</returns>
    private async Task<TaktEcKanbanDeptStatDto> CountDeptBoardByCodesAsync<TEntity>(
        string deptCode,
        IReadOnlyList<string> ecCodes,
        IReadOnlyDictionary<string, HashSet<string>> fertPlannedRootsByEc)
        where TEntity : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        if (ecCodes.Count == 0)
        {
            return new TaktEcKanbanDeptStatDto { DeptCode = deptCode };
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var entities = await _db.Queryable<TEntity>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && ecCodes.Contains(x.EcCode)
                && x.IsDeleted == 0
                && x.IsObsolete == 0)
            .ToListAsync();
        var rows = entities
            .Select(ProjectDeptExecForKanbanStat)
            .Where(x => IsKanbanPlannedMaterial(x.DiscontinuedStatus)
                && IsRootGroupAllInFertPlannedSet(x.EcCode, x.RootGroup, fertPlannedRootsByEc))
            .ToList();
        var implemented = rows.Count(x => x.IsImplemented == 1);
        var notImplemented = rows.Count - implemented;
        var byEc = rows
            .Where(x => !string.IsNullOrWhiteSpace(x.EcCode))
            .GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        var ecNotStarted = 0;
        var ecInProgress = 0;
        var ecCompleted = 0;
        foreach (var code in ecCodes)
        {
            if (!byEc.TryGetValue(code, out var list) || list.Count == 0)
            {
                ecNotStarted++;
                continue;
            }
            // 本部门明细全部已实施才算完成
            if (list.Any(x => x.IsImplemented != 1))
            {
                ecInProgress++;
            }
            else
            {
                ecCompleted++;
            }
        }
        return new TaktEcKanbanDeptStatDto
        {
            DeptCode = deptCode,
            Implemented = implemented,
            NotImplemented = notImplemented,
            EcNotStarted = ecNotStarted,
            EcInProgress = ecInProgress,
            EcCompleted = ecCompleted,
        };
    }

    /// <summary>
    /// 部门执行实体投影为看板统计行字段
    /// </summary>
    /// <param name="entity">执行实体</param>
    /// <returns>统计用字段</returns>
    private static (string EcCode, int IsImplemented, string DiscontinuedStatus, string RootGroup)
        ProjectDeptExecForKanbanStat(ITaktEcDeptExecEntity entity)
    {
        return entity switch
        {
            TaktEcPc e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcMfg2 e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcMfg1 e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcQc e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcPur e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcIqc e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcMc e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            TaktEcMtl e => (e.EcCode, e.IsImplemented, e.DiscontinuedStatus ?? string.Empty, e.EcRootMaterialCodeGroup ?? string.Empty),
            _ => (entity.EcCode, entity.IsImplemented, string.Empty, string.Empty),
        };
    }

    /// <summary>
    /// 根物料编码组是否全部落在该设变的 FERT 计划物料集合内
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="rootGroup">根物料编码组</param>
    /// <param name="fertPlannedRootsByEc">FERT 计划物料根编码</param>
    /// <returns>全部命中则为 true</returns>
    private static bool IsRootGroupAllInFertPlannedSet(
        string? ecCode,
        string? rootGroup,
        IReadOnlyDictionary<string, HashSet<string>> fertPlannedRootsByEc)
    {
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            return false;
        }
        var roots = SplitGroupCodes(rootGroup);
        if (roots.Count == 0)
        {
            return false;
        }
        if (!fertPlannedRootsByEc.TryGetValue(ecCode, out var set) || set.Count == 0)
        {
            return false;
        }
        return roots.All(r => set.Contains(r));
    }

    /// <summary>
    /// 按实施状态统计主表行数
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="status">实施路径状态</param>
    /// <returns>行数</returns>
    private async Task<int> CountByStatusAsync(TaktEcKanbanQueryDto queryDto, int status)
    {
        var dto = new TaktEcKanbanQueryDto
        {
            KeyWords = queryDto.KeyWords,
            EcCode = queryDto.EcCode,
            ChangeStatus = queryDto.ChangeStatus,
            EcStatus = queryDto.EcStatus,
            CultureCode = queryDto.CultureCode,
            EcEntryDateStart = queryDto.EcEntryDateStart,
            EcEntryDateEnd = queryDto.EcEntryDateEnd,
            ImplementationStatus = status,
        };
        return await _ecTeRepository.CountAsync(ListExpression(dto));
    }

    /// <summary>
    /// 录入期间内按月统计设变单数（最多 12 个月）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>按月序列</returns>
    private async Task<List<TaktEcKanbanMonthStatDto>> CountMonthsAsync(TaktEcKanbanQueryDto queryDto)
    {
        var months = new List<TaktEcKanbanMonthStatDto>();
        if (!queryDto.EcEntryDateStart.HasValue || !queryDto.EcEntryDateEnd.HasValue)
        {
            return months;
        }
        var cursor = new DateTime(queryDto.EcEntryDateStart.Value.Year, queryDto.EcEntryDateStart.Value.Month, 1);
        var endMonth = new DateTime(queryDto.EcEntryDateEnd.Value.Year, queryDto.EcEntryDateEnd.Value.Month, 1);
        var guard = 0;
        while (cursor <= endMonth && guard < 12)
        {
            var monthStart = cursor;
            var monthEnd = cursor.AddMonths(1).AddDays(-1);
            if (monthEnd > queryDto.EcEntryDateEnd.Value.Date)
            {
                monthEnd = queryDto.EcEntryDateEnd.Value.Date;
            }
            var dto = new TaktEcKanbanQueryDto
            {
                KeyWords = queryDto.KeyWords,
                EcCode = queryDto.EcCode,
                ChangeStatus = queryDto.ChangeStatus,
                EcStatus = queryDto.EcStatus,
                CultureCode = queryDto.CultureCode,
                EcEntryDateStart = monthStart,
                EcEntryDateEnd = monthEnd,
            };
            months.Add(new TaktEcKanbanMonthStatDto
            {
                YearMonth = cursor.ToString("yyyy-MM"),
                EcCount = await _ecTeRepository.CountAsync(ListExpression(dto)),
            });
            cursor = cursor.AddMonths(1);
            guard = checked(guard + 1);
        }
        return months;
    }

    /// <summary>
    /// 为一页设变主表组装看板行。以生管执行行为标准行序；制一/制二/品管按去重键直接对齐；
    /// 采购/受检/部管/物料/制技按成品组反向∪明细去重键聚合到对应生管行；多物料须全部实施才为是
    /// </summary>
    /// <param name="masters">当前页或导出范围内的主表</param>
    /// <returns>转置行，顺序与主表一致；各部门 *Lines 长度均等于生管行数</returns>
    private async Task<List<TaktEcKanbanDto>> BuildRowsAsync(IReadOnlyList<TaktEcTe> masters)
    {
        if (masters.Count == 0)
        {
            return [];
        }
        var codes = masters
            .Select(x => x.EcCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var details = await _db.Queryable<TaktEcDetail>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .Select(x => new TaktEcDetail
            {
                EcCode = x.EcCode,
                EcModelCode = x.EcModelCode,
                EcRootMaterialCode = x.EcRootMaterialCode,
                EcNewMaterialCode = x.EcNewMaterialCode,
                EcNewPurchaseType = x.EcNewPurchaseType,
                EcNewRequiresInspection = x.EcNewRequiresInspection,
                EcRootMaterialType = x.EcRootMaterialType,
                DiscontinuedStatus = x.DiscontinuedStatus,
                EcImplScope = x.EcImplScope,
            })
            .ToListAsync();
        var detailsByCode = details
            .GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<TaktEcDetail>)g.ToList(), StringComparer.OrdinalIgnoreCase);
        var pcByCode = GroupExec(await QueryPcRawsAsync(codes));
        var purFullByCode = GroupPurFull(await QueryPurFullAsync(codes));
        var purByCode = GroupExec(FlattenPurFullToExec(purFullByCode));
        var iqcFullByCode = GroupIqcFull(await QueryIqcFullAsync(codes));
        var iqcByCode = GroupExec(FlattenIqcFullToExec(iqcFullByCode));
        var mcFullByCode = GroupMcFull(await QueryMcFullAsync(codes));
        var mcByCode = GroupExec(FlattenMcFullToExec(mcFullByCode));
        var mtlFullByCode = GroupMtlFull(await QueryMtlFullAsync(codes));
        var mtlByCode = GroupExec(FlattenMtlFullToExec(mtlFullByCode));
        var mfg2ByCode = GroupExec(await QueryMfg2RawsAsync(codes));
        var mfg1ByCode = GroupExec(await QueryMfg1RawsAsync(codes));
        var qcByCode = GroupExec(await QueryQcRawsAsync(codes));
        var meByCode = GroupExec(await QueryMeRawsAsync(codes));
        var rows = new List<TaktEcKanbanDto>(masters.Count);
        foreach (var master in masters)
        {
            detailsByCode.TryGetValue(master.EcCode ?? string.Empty, out var codeDetails);
            codeDetails ??= [];
            pcByCode.TryGetValue(master.EcCode ?? string.Empty, out var pcs);
            pcs ??= [];
            var purFull = LookupPurFull(purFullByCode, master.EcCode);
            var iqcFull = LookupIqcFull(iqcFullByCode, master.EcCode);
            var mcFull = LookupMcFull(mcFullByCode, master.EcCode);
            var mtlFull = LookupMtlFull(mtlFullByCode, master.EcCode);
            var aligned = AlignDeptsToPc(
                master.EcControlScope,
                codeDetails,
                pcs,
                Lookup(purByCode, master.EcCode),
                Lookup(iqcByCode, master.EcCode),
                Lookup(mcByCode, master.EcCode),
                Lookup(mtlByCode, master.EcCode),
                Lookup(mfg2ByCode, master.EcCode),
                Lookup(mfg1ByCode, master.EcCode),
                Lookup(qcByCode, master.EcCode),
                Lookup(meByCode, master.EcCode));
            rows.Add(new TaktEcKanbanDto
            {
                EcTeId = master.Id,
                EcIssueDate = master.EcIssueDate,
                EcLeader = master.EcLeader ?? string.Empty,
                EcCode = master.EcCode ?? string.Empty,
                EcEntryDate = master.EcEntryDate,
                PcLines = aligned.Pc,
                PurLines = aligned.Pur,
                PurDetailLines = ToPurDetailLines(purFull, codeDetails),
                IqcDetailLines = ToIqcDetailLines(iqcFull, codeDetails),
                McDetailLines = ToMcDetailLines(mcFull, codeDetails),
                MtlDetailLines = ToMtlDetailLines(mtlFull, codeDetails),
                IqcLines = aligned.Iqc,
                McLines = aligned.Mc,
                MtlLines = aligned.Mtl,
                Mfg2Lines = aligned.Mfg2,
                Mfg1Lines = aligned.Mfg1,
                QcLines = aligned.Qc,
                MeLines = aligned.Me,
                PcImplemented = ToFlags(aligned.Pc),
                PurImplemented = ToFlags(aligned.Pur),
                IqcImplemented = ToFlags(aligned.Iqc),
                McImplemented = ToFlags(aligned.Mc),
                MtlImplemented = ToFlags(aligned.Mtl),
                Mfg2Implemented = ToFlags(aligned.Mfg2),
                Mfg1Implemented = ToFlags(aligned.Mfg1),
                QcImplemented = ToFlags(aligned.Qc),
                MeImplemented = ToFlags(aligned.Me),
            });
        }
        return rows;
    }

    /// <summary>
    /// 主表兼容字段机种/成品：取第一行生管明细原样（展开行以 PcLines 各行字段为准，禁止整单明细拼组）
    /// </summary>
    /// <param name="rows">看板行</param>
    private static void FillIdentityFromPcLines(IReadOnlyList<TaktEcKanbanDto> rows)
    {
        foreach (var row in rows)
        {
            var first = row.PcLines.FirstOrDefault();
            row.EcModelCode = first?.EcModelCode ?? string.Empty;
            row.EcRootMaterialCode = first?.EcRootMaterialCode ?? string.Empty;
        }
    }

    /// <summary>
    /// 看板执行行原始数据（含去重键，供按生管对齐）
    /// </summary>
    private sealed class KanbanExecRaw
    {
        /// <summary>主键</summary>
        public long Id { get; init; }
        /// <summary>设变单号</summary>
        public string EcCode { get; init; } = string.Empty;
        /// <summary>执行去重键（ntext，查询须 SqlFunc.ToString）</summary>
        public string EcGrainKey { get; init; } = string.Empty;
        /// <summary>机种编码组（生管；ntext）</summary>
        public string EcModelCodeGroup { get; init; } = string.Empty;
        /// <summary>根物料编码组（生管；ntext；按成品时常为单码）</summary>
        public string EcRootMaterialCodeGroup { get; init; } = string.Empty;
        /// <summary>行号</summary>
        public int LineNumber { get; init; }
        /// <summary>是否实施（制技为是否更新 SOP）</summary>
        public int IsImplemented { get; init; }
        /// <summary>阶段日期</summary>
        public DateTime? StageDate { get; init; }
        /// <summary>批次/单号</summary>
        public string? ProdBatch { get; init; }
        /// <summary>停产状态（空或 Z0=计划物料）</summary>
        public string DiscontinuedStatus { get; init; } = string.Empty;
    }

    /// <summary>
    /// 对齐后的各部门执行行：生管/制二/制一/品管/制技长度=生管行数；采购/受检/部管/物料为本部门库表行
    /// </summary>
    private sealed class KanbanAlignedDepts
    {
        public List<TaktEcKanbanDeptLineDto> Pc { get; init; } = [];
        public List<TaktEcKanbanDeptLineDto> Pur { get; set; } = [];
        public List<TaktEcKanbanDeptLineDto> Iqc { get; set; } = [];
        public List<TaktEcKanbanDeptLineDto> Mc { get; set; } = [];
        public List<TaktEcKanbanDeptLineDto> Mtl { get; set; } = [];
        public List<TaktEcKanbanDeptLineDto> Mfg2 { get; init; } = [];
        public List<TaktEcKanbanDeptLineDto> Mfg1 { get; init; } = [];
        public List<TaktEcKanbanDeptLineDto> Qc { get; init; } = [];
        public List<TaktEcKanbanDeptLineDto> Me { get; init; } = [];
    }

    /// <summary>
    /// 按设变单号分组执行行
    /// </summary>
    /// <param name="rows">原始行</param>
    /// <returns>设变单号 → 行列表</returns>
    private static Dictionary<string, List<KanbanExecRaw>> GroupExec(IReadOnlyList<KanbanExecRaw> rows)
    {
        return rows
            .GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.LineNumber).ThenBy(x => x.EcGrainKey, StringComparer.Ordinal).ToList(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 取设变号下的执行行
    /// </summary>
    /// <param name="map">分组</param>
    /// <param name="ecCode">设变单号</param>
    /// <returns>行列表</returns>
    private static IReadOnlyList<KanbanExecRaw> Lookup(IReadOnlyDictionary<string, List<KanbanExecRaw>> map, string? ecCode)
    {
        if (string.IsNullOrWhiteSpace(ecCode) || !map.TryGetValue(ecCode, out var list))
        {
            return [];
        }
        return list;
    }

    /// <summary>
    /// 以生管行为脊柱对齐制一/制二/品管/制技（脊柱仅 FERT+计划物料）；采购/受检/部管/物料按本部门库表行过滤后回填（独立页签）
    /// </summary>
    /// <param name="masterControlScope">技术主表控制范围</param>
    /// <param name="details">该设变明细</param>
    /// <param name="pcs">生管行（已按行号排序）</param>
    /// <param name="purs">采购行</param>
    /// <param name="iqcs">受检行</param>
    /// <param name="mcs">部管行</param>
    /// <param name="mtls">物料行</param>
    /// <param name="mfg2s">制二行</param>
    /// <param name="mfg1s">制一行</param>
    /// <param name="qcs">品管行</param>
    /// <param name="mes">制技行</param>
    /// <returns>对齐后的各部门行</returns>
    private static KanbanAlignedDepts AlignDeptsToPc(
        int masterControlScope,
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<KanbanExecRaw> pcs,
        IReadOnlyList<KanbanExecRaw> purs,
        IReadOnlyList<KanbanExecRaw> iqcs,
        IReadOnlyList<KanbanExecRaw> mcs,
        IReadOnlyList<KanbanExecRaw> mtls,
        IReadOnlyList<KanbanExecRaw> mfg2s,
        IReadOnlyList<KanbanExecRaw> mfg1s,
        IReadOnlyList<KanbanExecRaw> qcs,
        IReadOnlyList<KanbanExecRaw> mes)
    {
        // 采购等与生管粒度不同：仅 FERT+计划物料的本部门库表行
        var purLines = ToAsStoredDeptLines(purs, details);
        var iqcLines = ToAsStoredDeptLines(iqcs, details);
        var mcLines = ToAsStoredDeptLines(mcs, details);
        var mtlLines = ToAsStoredDeptLines(mtls, details);
        // 批次标签脊柱：FERT 且计划物料（空/Z0）；禁止按空 EcGrainKey 去重
        var spine = pcs
            .Where(pc => IsKanbanPcSpineRow(details, pc, masterControlScope))
            .Take(KanbanDeptLineCap)
            .ToList();
        if (spine.Count == 0)
        {
            return new KanbanAlignedDepts
            {
                Pc = [EmptyDeptLine()],
                Pur = purLines.Count > 0 ? purLines : [EmptyDeptLine()],
                Iqc = iqcLines.Count > 0 ? iqcLines : [EmptyDeptLine()],
                Mc = mcLines.Count > 0 ? mcLines : [EmptyDeptLine()],
                Mtl = mtlLines.Count > 0 ? mtlLines : [EmptyDeptLine()],
                Mfg2 = [EmptyDeptLine()],
                Mfg1 = [EmptyDeptLine()],
                Qc = [EmptyDeptLine()],
                Me = [EmptyDeptLine()],
            };
        }
        var mfg2Index = IndexByGrain(mfg2s);
        var mfg1Index = IndexByGrain(mfg1s);
        var qcIndex = IndexByGrain(qcs);
        var meIndex = IndexByGrain(mes);
        var result = new KanbanAlignedDepts
        {
            Pc = new List<TaktEcKanbanDeptLineDto>(spine.Count),
            Pur = purLines,
            Iqc = iqcLines,
            Mc = mcLines,
            Mtl = mtlLines,
            Mfg2 = new List<TaktEcKanbanDeptLineDto>(spine.Count),
            Mfg1 = new List<TaktEcKanbanDeptLineDto>(spine.Count),
            Qc = new List<TaktEcKanbanDeptLineDto>(spine.Count),
            Me = new List<TaktEcKanbanDeptLineDto>(spine.Count),
        };
        for (var i = 0; i < spine.Count; i++)
        {
            var pc = spine[i];
            // 设变信息机种/成品：生管该行是什么就展示什么（全仕向+按成品=单机种+单成品）
            result.Pc.Add(ToPcDeptLine(pc));
            // 制一/制二/品管与生管同粒，走明细去重键或同行号
            var pcDetails = DetailsForPcRow(details, pc, masterControlScope);
            result.Mfg2.Add(PickDirect(pcDetails, TaktEcDeptCodes.Mfg2, masterControlScope, mfg2Index, mfg2s, i));
            result.Mfg1.Add(PickDirect(pcDetails, TaktEcDeptCodes.Mfg1, masterControlScope, mfg1Index, mfg1s, i));
            result.Qc.Add(PickDirect(pcDetails, TaktEcDeptCodes.Qc, masterControlScope, qcIndex, qcs, i));
            // 制技仍按命中行聚合勾叉
            result.Me.Add(AggregateDeptForPc(pc, pcDetails, TaktEcDeptCodes.Me, masterControlScope, mes, meIndex));
        }
        return result;
    }

    /// <summary>
    /// 部门执行行按库表顺序映射（采购/受检/部管/物料：仅 FERT+计划物料）
    /// </summary>
    /// <param name="rows">同设变部门执行行</param>
    /// <param name="details">该设变明细</param>
    /// <returns>看板行</returns>
    private static List<TaktEcKanbanDeptLineDto> ToAsStoredDeptLines(
        IReadOnlyList<KanbanExecRaw> rows,
        IReadOnlyList<TaktEcDetail> details)
    {
        return rows
            .Where(x => IsKanbanPlannedMaterial(x.DiscontinuedStatus)
                && IsKanbanFertPlannedRootGroup(details, x.EcRootMaterialCodeGroup))
            .OrderBy(x => x.LineNumber)
            .ThenBy(x => x.Id)
            .Take(KanbanDeptLineCap)
            .Select(ToAsStoredDeptLine)
            .ToList();
    }

    /// <summary>
    /// 单条部门执行行 → 看板格（含机种/成品组展示）
    /// </summary>
    /// <param name="raw">原始行</param>
    /// <returns>DTO</returns>
    private static TaktEcKanbanDeptLineDto ToAsStoredDeptLine(KanbanExecRaw raw)
    {
        var line = ToDeptLine(raw);
        line.EcModelCode = DisplayPcGroupAsStored(raw.EcModelCodeGroup);
        line.EcRootMaterialCode = DisplayPcGroupAsStored(raw.EcRootMaterialCodeGroup);
        line.LineCount = 1;
        line.ImplementedCount = raw.IsImplemented == 1 ? 1 : 0;
        line.NotImplementedCount = raw.IsImplemented == 1 ? 0 : 1;
        return line;
    }

    /// <summary>
    /// 归属某一生管执行行的明细：仅按该行 EcGrainKey（与生管落行去重键一致）；不对机种/成品组做拆解
    /// </summary>
    /// <param name="details">该设变全部明细</param>
    /// <param name="pc">生管执行行</param>
    /// <param name="masterControlScope">主表控制范围</param>
    /// <returns>该生管行覆盖的明细</returns>
    private static List<TaktEcDetail> DetailsForPcRow(
        IReadOnlyList<TaktEcDetail> details,
        KanbanExecRaw pc,
        int masterControlScope)
    {
        if (string.IsNullOrWhiteSpace(pc.EcGrainKey))
        {
            return [];
        }
        return details
            .Where(d => string.Equals(
                TaktEcScopeRowGrain.BuildExecGrainKey(d, TaktEcDeptCodes.Pc, masterControlScope),
                pc.EcGrainKey,
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// 根物料类型是否为成品 FERT
    /// </summary>
    /// <param name="materialType">明细 EcRootMaterialType</param>
    /// <returns>是 FERT 则为 true</returns>
    private static bool IsFertMaterialType(string? materialType)
    {
        return string.Equals(
            materialType?.Trim(),
            TaktEcMfg1Constants.FertMaterialType,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否计划物料停产状态（空或 Z0；非 EOL）
    /// </summary>
    /// <param name="discontinuedStatus">停产状态</param>
    /// <returns>计划物料则为 true</returns>
    private static bool IsKanbanPlannedMaterial(string? discontinuedStatus)
    {
        return !TaktEcScopeConstants.IsEolDiscontinued(discontinuedStatus);
    }

    /// <summary>
    /// 明细是否为看板纳入范围：FERT 根物料且计划物料
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>纳入则为 true</returns>
    private static bool IsKanbanFertPlannedDetail(TaktEcDetail detail)
    {
        return IsFertMaterialType(detail.EcRootMaterialType)
            && IsKanbanPlannedMaterial(detail.DiscontinuedStatus);
    }

    /// <summary>
    /// 根物料编码组是否全部对应 FERT+计划物料明细
    /// </summary>
    /// <param name="details">该设变明细</param>
    /// <param name="rootGroup">根物料编码组</param>
    /// <returns>全部命中则为 true</returns>
    private static bool IsKanbanFertPlannedRootGroup(
        IReadOnlyList<TaktEcDetail> details,
        string? rootGroup)
    {
        var roots = SplitGroupCodes(rootGroup);
        if (roots.Count == 0)
        {
            return false;
        }
        foreach (var root in roots)
        {
            var related = details
                .Where(d => string.Equals(d.EcRootMaterialCode, root, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (related.Count == 0 || !related.All(IsKanbanFertPlannedDetail))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 批次/脊柱：该生管行关联明细须均为 FERT+计划物料（无明细时按根物料编码组回查）
    /// </summary>
    /// <param name="details">该设变明细</param>
    /// <param name="pc">生管执行行</param>
    /// <param name="masterControlScope">主表控制范围</param>
    /// <returns>可进入脊柱则为 true</returns>
    private static bool IsKanbanPcSpineRow(
        IReadOnlyList<TaktEcDetail> details,
        KanbanExecRaw pc,
        int masterControlScope)
    {
        if (!IsKanbanPlannedMaterial(pc.DiscontinuedStatus))
        {
            return false;
        }
        var pcDetails = DetailsForPcRow(details, pc, masterControlScope);
        IReadOnlyList<TaktEcDetail> typed = pcDetails;
        if (typed.Count == 0)
        {
            var roots = SplitGroupCodes(pc.EcRootMaterialCodeGroup);
            if (roots.Count == 0)
            {
                return false;
            }
            typed = details
                .Where(d => roots.Any(r =>
                    string.Equals(d.EcRootMaterialCode, r, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
        if (typed.Count == 0)
        {
            return false;
        }
        return typed.All(IsKanbanFertPlannedDetail);
    }

    /// <summary>
    /// 生管表字段原样展示：分号仅换成顿号，不删码、不取首码、不从 GrainKey 拆
    /// </summary>
    /// <param name="group">生管 EcModelCodeGroup / EcRootMaterialCodeGroup</param>
    /// <returns>展示文本</returns>
    private static string DisplayPcGroupAsStored(string? group)
    {
        if (string.IsNullOrWhiteSpace(group))
        {
            return string.Empty;
        }
        return string.Join(
            "、",
            group.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => x.Length > 0));
    }

    /// <summary>
    /// 去重键 → 执行行列表
    /// </summary>
    /// <param name="rows">执行行</param>
    /// <returns>索引</returns>
    private static Dictionary<string, List<KanbanExecRaw>> IndexByGrain(IReadOnlyList<KanbanExecRaw> rows)
    {
        var map = new Dictionary<string, List<KanbanExecRaw>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var key = row.EcGrainKey ?? string.Empty;
            if (!map.TryGetValue(key, out var list))
            {
                list = [];
                map[key] = list;
            }
            list.Add(row);
        }
        return map;
    }

    /// <summary>
    /// 制一/制二/品管：与生管同行数直接关联；优先明细去重键，否则同行号下标
    /// </summary>
    /// <param name="pcDetails">归属该生管行的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="masterControlScope">主表控制范围</param>
    /// <param name="byGrain">去重键索引</param>
    /// <param name="ordered">按行号排序的全量行</param>
    /// <param name="pcIndex">生管行下标</param>
    /// <returns>对齐后的一格</returns>
    private static TaktEcKanbanDeptLineDto PickDirect(
        IReadOnlyList<TaktEcDetail> pcDetails,
        string deptCode,
        int masterControlScope,
        IReadOnlyDictionary<string, List<KanbanExecRaw>> byGrain,
        IReadOnlyList<KanbanExecRaw> ordered,
        int pcIndex)
    {
        var matched = MatchByDetailGrains(pcDetails, deptCode, masterControlScope, byGrain);
        if (matched.Count > 0)
        {
            return AggregateLines(matched);
        }
        if (pcIndex >= 0 && pcIndex < ordered.Count)
        {
            return ToDeptLine(ordered[pcIndex]);
        }
        return EmptyDeptLine();
    }

    /// <summary>
    /// 制技等：反向 ∪ 明细键命中后按全部实施才为是聚合勾叉
    /// </summary>
    /// <param name="pc">生管脊柱行</param>
    /// <param name="pcDetails">归属该生管行的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="masterControlScope">主表控制范围</param>
    /// <param name="deptRows">同设变部门执行行</param>
    /// <param name="byGrain">去重键索引</param>
    /// <returns>聚合格</returns>
    private static TaktEcKanbanDeptLineDto AggregateDeptForPc(
        KanbanExecRaw pc,
        IReadOnlyList<TaktEcDetail> pcDetails,
        string deptCode,
        int masterControlScope,
        IReadOnlyList<KanbanExecRaw> deptRows,
        IReadOnlyDictionary<string, List<KanbanExecRaw>> byGrain)
    {
        return AggregateLines(CollectDeptRowsForPc(pc, pcDetails, deptCode, masterControlScope, deptRows, byGrain));
    }

    /// <summary>
    /// 生管行下部门执行行：成品组反向命中 ∪ 明细去重键命中（同 Id 去重）
    /// </summary>
    /// <param name="pc">生管行</param>
    /// <param name="pcDetails">生管行明细</param>
    /// <param name="deptCode">部门</param>
    /// <param name="masterControlScope">主表控制范围</param>
    /// <param name="deptRows">部门全量行</param>
    /// <param name="byGrain">去重键索引</param>
    /// <returns>命中行</returns>
    private static List<KanbanExecRaw> CollectDeptRowsForPc(
        KanbanExecRaw pc,
        IReadOnlyList<TaktEcDetail> pcDetails,
        string deptCode,
        int masterControlScope,
        IReadOnlyList<KanbanExecRaw> deptRows,
        IReadOnlyDictionary<string, List<KanbanExecRaw>> byGrain)
    {
        var matched = new List<KanbanExecRaw>();
        var seen = new HashSet<long>();
        void AddRange(IEnumerable<KanbanExecRaw> rows)
        {
            foreach (var row in rows)
            {
                if (seen.Add(row.Id))
                {
                    matched.Add(row);
                }
            }
        }
        AddRange(MatchByPcGroups(pc, deptRows));
        AddRange(MatchByDetailGrains(pcDetails, deptCode, masterControlScope, byGrain));
        return matched;
    }

    /// <summary>
    /// 生管成品/机种反向命中部门执行行（同设变；成品组含生管成品，或机种组含生管机种）
    /// </summary>
    /// <param name="pc">生管行</param>
    /// <param name="deptRows">部门行</param>
    /// <returns>命中行</returns>
    private static List<KanbanExecRaw> MatchByPcGroups(
        KanbanExecRaw pc,
        IReadOnlyList<KanbanExecRaw> deptRows)
    {
        var pcRoots = SplitGroupCodes(pc.EcRootMaterialCodeGroup);
        var pcModels = SplitGroupCodes(pc.EcModelCodeGroup);
        if (pcRoots.Count == 0 && pcModels.Count == 0)
        {
            return [];
        }
        var matched = new List<KanbanExecRaw>();
        foreach (var row in deptRows)
        {
            if (!string.Equals(row.EcCode, pc.EcCode, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            if (DeptRowCoversPc(row, pcModels, pcRoots))
            {
                matched.Add(row);
            }
        }
        return matched;
    }

    /// <summary>
    /// 部门行是否覆盖该生管行：优先成品组含生管成品；否则机种组含生管机种（部管/物料按机种）
    /// </summary>
    /// <param name="dept">部门执行行</param>
    /// <param name="pcModels">生管机种码</param>
    /// <param name="pcRoots">生管成品码</param>
    /// <returns>覆盖则为 true</returns>
    private static bool DeptRowCoversPc(
        KanbanExecRaw dept,
        IReadOnlyList<string> pcModels,
        IReadOnlyList<string> pcRoots)
    {
        if (pcRoots.Count > 0)
        {
            var rootHit = pcRoots.Any(root =>
                TaktEcRootMaterialCodeGroup.ContainsCode(dept.EcRootMaterialCodeGroup, root));
            if (rootHit)
            {
                if (pcModels.Count > 0 && !string.IsNullOrWhiteSpace(dept.EcModelCodeGroup))
                {
                    return pcModels.Any(model =>
                        TaktEcRootMaterialCodeGroup.ContainsCode(dept.EcModelCodeGroup, model));
                }
                return true;
            }
        }
        if (pcModels.Count == 0)
        {
            return false;
        }
        return pcModels.Any(model =>
            TaktEcRootMaterialCodeGroup.ContainsCode(dept.EcModelCodeGroup, model));
    }

    /// <summary>
    /// 分号编码组拆成码列表
    /// </summary>
    /// <param name="group">分号分隔组</param>
    /// <returns>码列表</returns>
    private static List<string> SplitGroupCodes(string? group)
    {
        if (string.IsNullOrWhiteSpace(group))
        {
            return [];
        }
        return group
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => x.Length > 0)
            .ToList();
    }

    /// <summary>
    /// 按生管行覆盖的明细，解析目标部门去重键并收集执行行（同键去重）
    /// </summary>
    /// <param name="pcDetails">生管行明细</param>
    /// <param name="deptCode">目标部门</param>
    /// <param name="masterControlScope">主表控制范围</param>
    /// <param name="byGrain">去重键索引</param>
    /// <returns>匹配到的执行行</returns>
    private static List<KanbanExecRaw> MatchByDetailGrains(
        IReadOnlyList<TaktEcDetail> pcDetails,
        string deptCode,
        int masterControlScope,
        IReadOnlyDictionary<string, List<KanbanExecRaw>> byGrain)
    {
        var matched = new List<KanbanExecRaw>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in pcDetails)
        {
            if (!TaktEcScopeRowGrain.IsEligible(detail, deptCode, detail.EcImplScope, null))
            {
                continue;
            }
            var grain = TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, masterControlScope);
            if (!byGrain.TryGetValue(grain, out var list))
            {
                continue;
            }
            foreach (var row in list)
            {
                if (seen.Add(row.Id.ToString()))
                {
                    matched.Add(row);
                }
            }
        }
        return matched;
    }

    /// <summary>
    /// 多执行行合成一格（勾叉）：全部实施才为是；未全部实施不回填日期/批次
    /// </summary>
    /// <param name="matched">匹配行</param>
    /// <returns>聚合 DTO</returns>
    private static TaktEcKanbanDeptLineDto AggregateLines(IReadOnlyList<KanbanExecRaw> matched)
    {
        if (matched.Count == 0)
        {
            return EmptyDeptLine();
        }
        if (!matched.All(x => x.IsImplemented == 1))
        {
            return new TaktEcKanbanDeptLineDto
            {
                IsImplemented = 0,
                StageDate = null,
                DateDisplayText = string.Empty,
                ProdBatch = string.Empty,
            };
        }
        var dates = matched
            .Where(x => x.StageDate.HasValue)
            .Select(x => x.StageDate!.Value.Date)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
        var batches = matched
            .Select(x => x.ProdBatch?.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .Cast<string>()
            .ToList();
        return new TaktEcKanbanDeptLineDto
        {
            IsImplemented = 1,
            StageDate = dates.Count == 1 ? dates[0] : null,
            DateDisplayText = string.Join("、", dates.Select(x => x.ToString("yyyyMMdd"))),
            ProdBatch = string.Join("、", batches),
        };
    }

    /// <summary>
    /// 生管行 → 看板格：机种/成品 = 生管明细表 EcModelCodeGroup / EcRootMaterialCodeGroup 原样
    /// </summary>
    /// <param name="pc">生管原始行</param>
    /// <returns>DTO</returns>
    private static TaktEcKanbanDeptLineDto ToPcDeptLine(KanbanExecRaw pc)
    {
        var line = ToDeptLine(pc);
        line.EcModelCode = DisplayPcGroupAsStored(pc.EcModelCodeGroup);
        line.EcRootMaterialCode = DisplayPcGroupAsStored(pc.EcRootMaterialCodeGroup);
        return line;
    }

    /// <summary>
    /// 原始行 → 看板部门格（非生管不填机种/成品）
    /// </summary>
    /// <param name="raw">原始行</param>
    /// <returns>DTO</returns>
    private static TaktEcKanbanDeptLineDto ToDeptLine(KanbanExecRaw raw)
    {
        var date = raw.StageDate?.Date;
        return new TaktEcKanbanDeptLineDto
        {
            IsImplemented = raw.IsImplemented == 1 ? 1 : 0,
            StageDate = date,
            DateDisplayText = date?.ToString("yyyyMMdd") ?? string.Empty,
            ProdBatch = string.IsNullOrWhiteSpace(raw.ProdBatch) ? string.Empty : raw.ProdBatch.Trim(),
        };
    }

    /// <summary>
    /// 无对应执行行
    /// </summary>
    /// <returns>空格</returns>
    private static TaktEcKanbanDeptLineDto EmptyDeptLine()
    {
        return new TaktEcKanbanDeptLineDto
        {
            IsImplemented = -1,
            StageDate = null,
            DateDisplayText = string.Empty,
            ProdBatch = string.Empty,
        };
    }

    /// <summary>
    /// 从执行行提取实施标记（含 -1）
    /// </summary>
    /// <param name="lines">执行行</param>
    /// <returns>标记列表</returns>
    private static List<int> ToFlags(IReadOnlyList<TaktEcKanbanDeptLineDto> lines)
    {
        return lines.Select(x => x.IsImplemented).ToList();
    }

    /// <summary>
    /// 生管原始行
    /// </summary>
    /// <param name="codes">设变单号</param>
    /// <returns>原始行</returns>
    private async Task<List<KanbanExecRaw>> QueryPcRawsAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcPc>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.EcCode,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                EcModelCodeGroup = SqlFunc.ToString(x.EcModelCodeGroup),
                EcRootMaterialCodeGroup = SqlFunc.ToString(x.EcRootMaterialCodeGroup),
                x.LineNumber,
                x.IsImplemented,
                x.ScheduledDate,
                x.ScheduledBatch,
                x.DiscontinuedStatus,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            EcModelCodeGroup = x.EcModelCodeGroup ?? string.Empty,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup ?? string.Empty,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.ScheduledDate,
            ProdBatch = x.ScheduledBatch,
            DiscontinuedStatus = x.DiscontinuedStatus ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 采购完整业务行（看板采购页签 + 精简 PurLines）
    /// </summary>
    private sealed class KanbanPurFullRaw
    {
        public long Id { get; init; }
        public int LineNumber { get; init; }
        public string DiscontinuedStatus { get; init; } = string.Empty;
        public string EcCode { get; init; } = string.Empty;
        public string EcModelCodeGroup { get; init; } = string.Empty;
        public string EcRootMaterialCodeGroup { get; init; } = string.Empty;
        public string EcNewMaterialCodeGroup { get; init; } = string.Empty;
        public string EcNewPurchaseType { get; init; } = string.Empty;
        public int IsImplemented { get; init; }
        public string? ExecContent { get; init; }
        public string? EcOldPartBalance { get; init; }
        public DateTime? InboundDate { get; init; }
        public string DeptCode { get; init; } = string.Empty;
        public string DeptName { get; init; } = string.Empty;
        public int EcImplScope { get; init; }
        public int EcControlScope { get; init; }
        public string EcGrainKey { get; init; } = string.Empty;
        public int IsObsolete { get; init; }
    }

    /// <summary>
    /// 采购完整行按设变单号分组
    /// </summary>
    /// <param name="rows">完整行</param>
    /// <returns>分组</returns>
    private static Dictionary<string, List<KanbanPurFullRaw>> GroupPurFull(IReadOnlyList<KanbanPurFullRaw> rows)
    {
        return rows
            .GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.LineNumber).ThenBy(x => x.Id).ToList(),
                StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 取设变号下采购完整行
    /// </summary>
    /// <param name="map">分组</param>
    /// <param name="ecCode">设变单号</param>
    /// <returns>行列表</returns>
    private static IReadOnlyList<KanbanPurFullRaw> LookupPurFull(
        IReadOnlyDictionary<string, List<KanbanPurFullRaw>> map,
        string? ecCode)
    {
        if (string.IsNullOrWhiteSpace(ecCode) || !map.TryGetValue(ecCode, out var list))
        {
            return [];
        }
        return list;
    }

    /// <summary>
    /// 采购完整行 → 对齐用精简执行行
    /// </summary>
    /// <param name="map">分组</param>
    /// <returns>精简行</returns>
    private static List<KanbanExecRaw> FlattenPurFullToExec(IReadOnlyDictionary<string, List<KanbanPurFullRaw>> map)
    {
        return map.Values
            .SelectMany(list => list)
            .Select(x => new KanbanExecRaw
            {
                Id = x.Id,
                EcCode = x.EcCode,
                EcGrainKey = x.EcGrainKey,
                EcModelCodeGroup = x.EcModelCodeGroup,
                EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
                LineNumber = x.LineNumber,
                IsImplemented = x.IsImplemented,
                StageDate = x.InboundDate,
                ProdBatch = string.Empty,
                DiscontinuedStatus = x.DiscontinuedStatus,
            })
            .ToList();
    }

    /// <summary>
    /// 采购完整行 → 看板采购页签 DTO（仅 FERT+计划物料）
    /// </summary>
    /// <param name="rows">完整行</param>
    /// <param name="details">该设变明细</param>
    /// <returns>DTO 列表</returns>
    private static List<TaktEcKanbanPurLineDto> ToPurDetailLines(
        IReadOnlyList<KanbanPurFullRaw> rows,
        IReadOnlyList<TaktEcDetail> details)
    {
        return rows
            .Where(x => IsKanbanPlannedMaterial(x.DiscontinuedStatus)
                && IsKanbanFertPlannedRootGroup(details, x.EcRootMaterialCodeGroup))
            .Take(KanbanDeptLineCap)
            .Select(x => new TaktEcKanbanPurLineDto
            {
                EcPurId = x.Id,
                LineNumber = x.LineNumber,
                DiscontinuedStatus = x.DiscontinuedStatus,
                EcCode = x.EcCode,
                EcModelCodeGroup = x.EcModelCodeGroup,
                EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
                EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup,
                EcNewPurchaseType = x.EcNewPurchaseType,
                IsImplemented = x.IsImplemented,
                ExecContent = x.ExecContent,
                EcOldPartBalance = x.EcOldPartBalance,
                InboundDate = x.InboundDate,
                DeptCode = x.DeptCode,
                DeptName = x.DeptName,
                EcImplScope = x.EcImplScope,
                EcControlScope = x.EcControlScope,
                EcGrainKey = x.EcGrainKey,
                IsObsolete = x.IsObsolete,
            })
            .ToList();
    }

    /// <summary>
    /// 采购完整业务行查询（TaktEcPur 原样）
    /// </summary>
    /// <param name="codes">设变单号</param>
    /// <returns>完整行</returns>
    private async Task<List<KanbanPurFullRaw>> QueryPurFullAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcPur>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.LineNumber,
                x.DiscontinuedStatus,
                x.EcCode,
                EcModelCodeGroup = SqlFunc.ToString(x.EcModelCodeGroup),
                EcRootMaterialCodeGroup = SqlFunc.ToString(x.EcRootMaterialCodeGroup),
                EcNewMaterialCodeGroup = SqlFunc.ToString(x.EcNewMaterialCodeGroup),
                x.EcNewPurchaseType,
                x.IsImplemented,
                x.ExecContent,
                x.EcOldPartBalance,
                x.InboundDate,
                x.DeptCode,
                x.DeptName,
                x.EcImplScope,
                x.EcControlScope,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.IsObsolete,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanPurFullRaw
        {
            Id = x.Id,
            LineNumber = x.LineNumber,
            DiscontinuedStatus = x.DiscontinuedStatus ?? string.Empty,
            EcCode = x.EcCode ?? string.Empty,
            EcModelCodeGroup = x.EcModelCodeGroup ?? string.Empty,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup ?? string.Empty,
            EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup ?? string.Empty,
            EcNewPurchaseType = x.EcNewPurchaseType ?? string.Empty,
            IsImplemented = x.IsImplemented,
            ExecContent = x.ExecContent,
            EcOldPartBalance = x.EcOldPartBalance,
            InboundDate = x.InboundDate,
            DeptCode = x.DeptCode ?? string.Empty,
            DeptName = x.DeptName ?? string.Empty,
            EcImplScope = x.EcImplScope,
            EcControlScope = x.EcControlScope,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            IsObsolete = x.IsObsolete,
        }).ToList();
    }

    /// <summary>受检完整业务行</summary>
    private sealed class KanbanIqcFullRaw
    {
        public long Id { get; init; }
        public int LineNumber { get; init; }
        public string DiscontinuedStatus { get; init; } = string.Empty;
        public string EcCode { get; init; } = string.Empty;
        public string EcModelCodeGroup { get; init; } = string.Empty;
        public string EcNewMaterialCodeGroup { get; init; } = string.Empty;
        public string EcRootMaterialCodeGroup { get; init; } = string.Empty;
        public int EcNewRequiresInspection { get; init; }
        public int IsImplemented { get; init; }
        public string? ExecContent { get; init; }
        public DateTime? InspectionDate { get; init; }
        public string DeptCode { get; init; } = string.Empty;
        public string DeptName { get; init; } = string.Empty;
        public int EcImplScope { get; init; }
        public int EcControlScope { get; init; }
        public string EcGrainKey { get; init; } = string.Empty;
        public int IsObsolete { get; init; }
    }

    /// <summary>部管完整业务行</summary>
    private sealed class KanbanMcFullRaw
    {
        public long Id { get; init; }
        public int LineNumber { get; init; }
        public string EcCode { get; init; } = string.Empty;
        public string DiscontinuedStatus { get; init; } = string.Empty;
        public string EcModelCodeGroup { get; init; } = string.Empty;
        public string EcRootMaterialCodeGroup { get; init; } = string.Empty;
        public string EcNewMaterialCodeGroup { get; init; } = string.Empty;
                public string EcNewPurchaseType { get; init; } = string.Empty;
        public string EcNewWarehouse { get; init; } = string.Empty;
        public int IsImplemented { get; init; }
        public DateTime? ScheduledDate { get; init; }
        public string? ScheduledBatch { get; init; }
        public string? ExecContent { get; init; }
        public string? OutboundBatch { get; init; }
        public DateTime? OutboundDate { get; init; }
        public string DeptCode { get; init; } = string.Empty;
        public string DeptName { get; init; } = string.Empty;
        public int EcImplScope { get; init; }
        public int EcControlScope { get; init; }
        public string EcGrainKey { get; init; } = string.Empty;
        public int IsObsolete { get; init; }
    }

    /// <summary>物料完整业务行</summary>
    private sealed class KanbanMtlFullRaw
    {
        public long Id { get; init; }
        public int LineNumber { get; init; }
        public string EcCode { get; init; } = string.Empty;
        public string DiscontinuedStatus { get; init; } = string.Empty;
        public string EcModelCodeGroup { get; init; } = string.Empty;
        public string EcRootMaterialCodeGroup { get; init; } = string.Empty;
        public string EcParentMaterialCode { get; init; } = string.Empty;
        public string EcParentMaterialDescription { get; init; } = string.Empty;
        public string EcNewMaterialCodeGroup { get; init; } = string.Empty;
                public string EcNewPurchaseType { get; init; } = string.Empty;
        public string EcNewWarehouse { get; init; } = string.Empty;
        public int IsImplemented { get; init; }
        public DateTime? ScheduledDate { get; init; }
        public string? ScheduledBatch { get; init; }
        public string? ExecContent { get; init; }
        public string? OutboundBatch { get; init; }
        public DateTime? OutboundDate { get; init; }
        public string DeptCode { get; init; } = string.Empty;
        public string DeptName { get; init; } = string.Empty;
        public int EcImplScope { get; init; }
        public int EcControlScope { get; init; }
        public string EcGrainKey { get; init; } = string.Empty;
        public int IsObsolete { get; init; }
    }

    private static Dictionary<string, List<KanbanIqcFullRaw>> GroupIqcFull(IReadOnlyList<KanbanIqcFullRaw> rows) =>
        rows.GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.LineNumber).ThenBy(x => x.Id).ToList(), StringComparer.OrdinalIgnoreCase);

    private static Dictionary<string, List<KanbanMcFullRaw>> GroupMcFull(IReadOnlyList<KanbanMcFullRaw> rows) =>
        rows.GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.LineNumber).ThenBy(x => x.Id).ToList(), StringComparer.OrdinalIgnoreCase);

    private static Dictionary<string, List<KanbanMtlFullRaw>> GroupMtlFull(IReadOnlyList<KanbanMtlFullRaw> rows) =>
        rows.GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.LineNumber).ThenBy(x => x.Id).ToList(), StringComparer.OrdinalIgnoreCase);

    private static IReadOnlyList<KanbanIqcFullRaw> LookupIqcFull(IReadOnlyDictionary<string, List<KanbanIqcFullRaw>> map, string? ecCode) =>
        string.IsNullOrWhiteSpace(ecCode) || !map.TryGetValue(ecCode, out var list) ? [] : list;

    private static IReadOnlyList<KanbanMcFullRaw> LookupMcFull(IReadOnlyDictionary<string, List<KanbanMcFullRaw>> map, string? ecCode) =>
        string.IsNullOrWhiteSpace(ecCode) || !map.TryGetValue(ecCode, out var list) ? [] : list;

    private static IReadOnlyList<KanbanMtlFullRaw> LookupMtlFull(IReadOnlyDictionary<string, List<KanbanMtlFullRaw>> map, string? ecCode) =>
        string.IsNullOrWhiteSpace(ecCode) || !map.TryGetValue(ecCode, out var list) ? [] : list;

    private static List<KanbanExecRaw> FlattenIqcFullToExec(IReadOnlyDictionary<string, List<KanbanIqcFullRaw>> map) =>
        map.Values.SelectMany(list => list).Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.InspectionDate,
            ProdBatch = null,
            DiscontinuedStatus = x.DiscontinuedStatus,
        }).ToList();

    private static List<KanbanExecRaw> FlattenMcFullToExec(IReadOnlyDictionary<string, List<KanbanMcFullRaw>> map) =>
        map.Values.SelectMany(list => list).Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.OutboundDate,
            ProdBatch = x.OutboundBatch,
            DiscontinuedStatus = x.DiscontinuedStatus,
        }).ToList();

    private static List<KanbanExecRaw> FlattenMtlFullToExec(IReadOnlyDictionary<string, List<KanbanMtlFullRaw>> map) =>
        map.Values.SelectMany(list => list).Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey,
            EcModelCodeGroup = x.EcModelCodeGroup,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.OutboundDate,
            ProdBatch = x.OutboundBatch,
            DiscontinuedStatus = x.DiscontinuedStatus,
        }).ToList();

    /// <summary>
    /// 受检完整行 → 看板受检页签 DTO（仅 FERT+计划物料）
    /// </summary>
    private static List<TaktEcKanbanIqcLineDto> ToIqcDetailLines(
        IReadOnlyList<KanbanIqcFullRaw> rows,
        IReadOnlyList<TaktEcDetail> details) =>
        rows
            .Where(x => IsKanbanPlannedMaterial(x.DiscontinuedStatus)
                && IsKanbanFertPlannedRootGroup(details, x.EcRootMaterialCodeGroup))
            .Take(KanbanDeptLineCap)
            .Select(x => new TaktEcKanbanIqcLineDto
            {
                EcIqcId = x.Id,
                LineNumber = x.LineNumber,
                DiscontinuedStatus = x.DiscontinuedStatus,
                EcCode = x.EcCode,
                EcModelCodeGroup = x.EcModelCodeGroup,
                EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup,
                EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
                EcNewRequiresInspection = x.EcNewRequiresInspection,
                IsImplemented = x.IsImplemented,
                ExecContent = x.ExecContent,
                InspectionDate = x.InspectionDate,
                DeptCode = x.DeptCode,
                DeptName = x.DeptName,
                EcImplScope = x.EcImplScope,
                EcControlScope = x.EcControlScope,
                EcGrainKey = x.EcGrainKey,
                IsObsolete = x.IsObsolete,
            }).ToList();

    /// <summary>
    /// 部管完整行 → 看板部管页签 DTO（仅 FERT+计划物料）
    /// </summary>
    private static List<TaktEcKanbanMcLineDto> ToMcDetailLines(
        IReadOnlyList<KanbanMcFullRaw> rows,
        IReadOnlyList<TaktEcDetail> details) =>
        rows
            .Where(x => IsKanbanPlannedMaterial(x.DiscontinuedStatus)
                && IsKanbanFertPlannedRootGroup(details, x.EcRootMaterialCodeGroup))
            .Take(KanbanDeptLineCap)
            .Select(x => new TaktEcKanbanMcLineDto
            {
                EcMcId = x.Id,
                LineNumber = x.LineNumber,
                EcCode = x.EcCode,
                DiscontinuedStatus = x.DiscontinuedStatus,
                EcModelCodeGroup = x.EcModelCodeGroup,
                EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
                EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup,
                EcNewPurchaseType = x.EcNewPurchaseType,
                EcNewWarehouse = x.EcNewWarehouse,
                IsImplemented = x.IsImplemented,
                ScheduledDate = x.ScheduledDate,
                ScheduledBatch = x.ScheduledBatch,
                ExecContent = x.ExecContent,
                OutboundBatch = x.OutboundBatch,
                OutboundDate = x.OutboundDate,
                DeptCode = x.DeptCode,
                DeptName = x.DeptName,
                EcImplScope = x.EcImplScope,
                EcControlScope = x.EcControlScope,
                EcGrainKey = x.EcGrainKey,
                IsObsolete = x.IsObsolete,
            }).ToList();

    /// <summary>
    /// 物料完整行 → 看板物料页签 DTO（仅 FERT+计划物料）
    /// </summary>
    private static List<TaktEcKanbanMtlLineDto> ToMtlDetailLines(
        IReadOnlyList<KanbanMtlFullRaw> rows,
        IReadOnlyList<TaktEcDetail> details) =>
        rows
            .Where(x => IsKanbanPlannedMaterial(x.DiscontinuedStatus)
                && IsKanbanFertPlannedRootGroup(details, x.EcRootMaterialCodeGroup))
            .Take(KanbanDeptLineCap)
            .Select(x => new TaktEcKanbanMtlLineDto
            {
                EcMtlId = x.Id,
                LineNumber = x.LineNumber,
                EcCode = x.EcCode,
                DiscontinuedStatus = x.DiscontinuedStatus,
                EcModelCodeGroup = x.EcModelCodeGroup,
                EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup,
                EcParentMaterialCode = x.EcParentMaterialCode,
                EcParentMaterialDescription = x.EcParentMaterialDescription,
                EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup,
                EcNewPurchaseType = x.EcNewPurchaseType,
                EcNewWarehouse = x.EcNewWarehouse,
                IsImplemented = x.IsImplemented,
                ScheduledDate = x.ScheduledDate,
                ScheduledBatch = x.ScheduledBatch,
                ExecContent = x.ExecContent,
                OutboundBatch = x.OutboundBatch,
                OutboundDate = x.OutboundDate,
                DeptCode = x.DeptCode,
                DeptName = x.DeptName,
                EcImplScope = x.EcImplScope,
                EcControlScope = x.EcControlScope,
                EcGrainKey = x.EcGrainKey,
                IsObsolete = x.IsObsolete,
            }).ToList();

    /// <summary>受检完整业务行查询</summary>
    private async Task<List<KanbanIqcFullRaw>> QueryIqcFullAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcIqc>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.LineNumber,
                x.DiscontinuedStatus,
                x.EcCode,
                EcModelCodeGroup = SqlFunc.ToString(x.EcModelCodeGroup),
                EcNewMaterialCodeGroup = SqlFunc.ToString(x.EcNewMaterialCodeGroup),
                EcRootMaterialCodeGroup = SqlFunc.ToString(x.EcRootMaterialCodeGroup),
                x.EcNewRequiresInspection,
                x.IsImplemented,
                x.ExecContent,
                x.InspectionDate,
                x.DeptCode,
                x.DeptName,
                x.EcImplScope,
                x.EcControlScope,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.IsObsolete,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanIqcFullRaw
        {
            Id = x.Id,
            LineNumber = x.LineNumber,
            DiscontinuedStatus = x.DiscontinuedStatus ?? string.Empty,
            EcCode = x.EcCode ?? string.Empty,
            EcModelCodeGroup = x.EcModelCodeGroup ?? string.Empty,
            EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup ?? string.Empty,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup ?? string.Empty,
            EcNewRequiresInspection = x.EcNewRequiresInspection,
            IsImplemented = x.IsImplemented,
            ExecContent = x.ExecContent,
            InspectionDate = x.InspectionDate,
            DeptCode = x.DeptCode ?? string.Empty,
            DeptName = x.DeptName ?? string.Empty,
            EcImplScope = x.EcImplScope,
            EcControlScope = x.EcControlScope,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            IsObsolete = x.IsObsolete,
        }).ToList();
    }

    /// <summary>部管完整业务行查询</summary>
    private async Task<List<KanbanMcFullRaw>> QueryMcFullAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcMc>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.LineNumber,
                x.EcCode,
                x.DiscontinuedStatus,
                EcModelCodeGroup = SqlFunc.ToString(x.EcModelCodeGroup),
                EcRootMaterialCodeGroup = SqlFunc.ToString(x.EcRootMaterialCodeGroup),
                EcNewMaterialCodeGroup = SqlFunc.ToString(x.EcNewMaterialCodeGroup),
                                x.EcNewPurchaseType,
                x.EcNewWarehouse,
                x.IsImplemented,
                x.ScheduledDate,
                x.ScheduledBatch,
                x.ExecContent,
                x.OutboundBatch,
                x.OutboundDate,
                x.DeptCode,
                x.DeptName,
                x.EcImplScope,
                x.EcControlScope,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.IsObsolete,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanMcFullRaw
        {
            Id = x.Id,
            LineNumber = x.LineNumber,
            EcCode = x.EcCode ?? string.Empty,
            DiscontinuedStatus = x.DiscontinuedStatus ?? string.Empty,
            EcModelCodeGroup = x.EcModelCodeGroup ?? string.Empty,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup ?? string.Empty,
            EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup ?? string.Empty,
                        EcNewPurchaseType = x.EcNewPurchaseType ?? string.Empty,
            EcNewWarehouse = x.EcNewWarehouse ?? string.Empty,
            IsImplemented = x.IsImplemented,
            ScheduledDate = x.ScheduledDate,
            ScheduledBatch = x.ScheduledBatch,
            ExecContent = x.ExecContent,
            OutboundBatch = x.OutboundBatch,
            OutboundDate = x.OutboundDate,
            DeptCode = x.DeptCode ?? string.Empty,
            DeptName = x.DeptName ?? string.Empty,
            EcImplScope = x.EcImplScope,
            EcControlScope = x.EcControlScope,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            IsObsolete = x.IsObsolete,
        }).ToList();
    }

    /// <summary>物料完整业务行查询</summary>
    private async Task<List<KanbanMtlFullRaw>> QueryMtlFullAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcMtl>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.LineNumber,
                x.EcCode,
                x.DiscontinuedStatus,
                EcModelCodeGroup = SqlFunc.ToString(x.EcModelCodeGroup),
                EcRootMaterialCodeGroup = SqlFunc.ToString(x.EcRootMaterialCodeGroup),
                x.EcParentMaterialCode,
                x.EcParentMaterialDescription,
                EcNewMaterialCodeGroup = SqlFunc.ToString(x.EcNewMaterialCodeGroup),
                                x.EcNewPurchaseType,
                x.EcNewWarehouse,
                x.IsImplemented,
                x.ScheduledDate,
                x.ScheduledBatch,
                x.ExecContent,
                x.OutboundBatch,
                x.OutboundDate,
                x.DeptCode,
                x.DeptName,
                x.EcImplScope,
                x.EcControlScope,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.IsObsolete,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanMtlFullRaw
        {
            Id = x.Id,
            LineNumber = x.LineNumber,
            EcCode = x.EcCode ?? string.Empty,
            DiscontinuedStatus = x.DiscontinuedStatus ?? string.Empty,
            EcModelCodeGroup = x.EcModelCodeGroup ?? string.Empty,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup ?? string.Empty,
            EcParentMaterialCode = x.EcParentMaterialCode ?? string.Empty,
            EcParentMaterialDescription = x.EcParentMaterialDescription ?? string.Empty,
            EcNewMaterialCodeGroup = x.EcNewMaterialCodeGroup ?? string.Empty,
                        EcNewPurchaseType = x.EcNewPurchaseType ?? string.Empty,
            EcNewWarehouse = x.EcNewWarehouse ?? string.Empty,
            IsImplemented = x.IsImplemented,
            ScheduledDate = x.ScheduledDate,
            ScheduledBatch = x.ScheduledBatch,
            ExecContent = x.ExecContent,
            OutboundBatch = x.OutboundBatch,
            OutboundDate = x.OutboundDate,
            DeptCode = x.DeptCode ?? string.Empty,
            DeptName = x.DeptName ?? string.Empty,
            EcImplScope = x.EcImplScope,
            EcControlScope = x.EcControlScope,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            IsObsolete = x.IsObsolete,
        }).ToList();
    }

    /// <summary>
    /// 制二原始行
    /// </summary>
    /// <param name="codes">设变单号</param>
    /// <returns>原始行</returns>
    private async Task<List<KanbanExecRaw>> QueryMfg2RawsAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcMfg2>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.EcCode,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.LineNumber,
                x.IsImplemented,
                x.ProductionDate,
                x.ProductionBatch,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.ProductionDate,
            ProdBatch = x.ProductionBatch,
        }).ToList();
    }

    /// <summary>
    /// 制一原始行
    /// </summary>
    /// <param name="codes">设变单号</param>
    /// <returns>原始行</returns>
    private async Task<List<KanbanExecRaw>> QueryMfg1RawsAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcMfg1>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.EcCode,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.LineNumber,
                x.IsImplemented,
                x.ProductionDate,
                x.ProductionBatch,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.ProductionDate,
            ProdBatch = x.ProductionBatch,
        }).ToList();
    }

    /// <summary>
    /// 品管原始行
    /// </summary>
    /// <param name="codes">设变单号</param>
    /// <returns>原始行</returns>
    private async Task<List<KanbanExecRaw>> QueryQcRawsAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcQc>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.EcCode,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                x.LineNumber,
                x.IsImplemented,
                x.InspectionDate,
                x.InspectionBatch,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsImplemented,
            StageDate = x.InspectionDate,
            ProdBatch = x.InspectionBatch,
        }).ToList();
    }

    /// <summary>
    /// 制技原始行
    /// </summary>
    /// <param name="codes">设变单号</param>
    /// <returns>原始行</returns>
    private async Task<List<KanbanExecRaw>> QueryMeRawsAsync(IReadOnlyList<string> codes)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcMe>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && codes.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0)
            .OrderBy(x => x.LineNumber)
            .Select(x => new
            {
                x.Id,
                x.EcCode,
                EcGrainKey = SqlFunc.ToString(x.EcGrainKey),
                EcModelCodeGroup = SqlFunc.ToString(x.EcModelCodeGroup),
                EcRootMaterialCodeGroup = SqlFunc.ToString(x.EcRootMaterialCodeGroup),
                x.LineNumber,
                x.IsSopUpdated,
                x.SopDate,
            })
            .ToListAsync();
        return rows.Select(x => new KanbanExecRaw
        {
            Id = x.Id,
            EcCode = x.EcCode,
            EcGrainKey = x.EcGrainKey ?? string.Empty,
            EcModelCodeGroup = x.EcModelCodeGroup ?? string.Empty,
            EcRootMaterialCodeGroup = x.EcRootMaterialCodeGroup ?? string.Empty,
            LineNumber = x.LineNumber,
            IsImplemented = x.IsSopUpdated,
            StageDate = x.SopDate,
            ProdBatch = null,
        }).ToList();
    }

    /// <summary>
    /// 按生管行数展开导出。某一行已实施时该格为勾
    /// </summary>
    /// <param name="row">转置行</param>
    /// <returns>导出行</returns>
    private static IEnumerable<TaktEcKanbanExportDto> ExpandExportLines(TaktEcKanbanDto row)
    {
        var lineCount = Math.Max(1, row.PcLines?.Count ?? row.PcImplemented?.Count ?? 0);
        for (var index = 0; index < lineCount; index++)
        {
            var pc = index < (row.PcLines?.Count ?? 0) ? row.PcLines![index] : null;
            yield return new TaktEcKanbanExportDto
            {
                EcIssueDate = row.EcIssueDate.ToString("yyyy-MM-dd"),
                EcLeader = row.EcLeader,
                EcCode = row.EcCode,
                EcModelCode = pc?.EcModelCode ?? row.EcModelCode,
                EcRootMaterialCode = pc?.EcRootMaterialCode ?? row.EcRootMaterialCode,
                EcEntryDate = row.EcEntryDate.ToString("yyyy-MM-dd"),
                PcImplemented = FlagText(row.PcImplemented, index),
                PurImplemented = FlagText(row.PurImplemented, index),
                IqcImplemented = FlagText(row.IqcImplemented, index),
                McImplemented = FlagText(row.McImplemented, index),
                MtlImplemented = FlagText(row.MtlImplemented, index),
                Mfg2Implemented = FlagText(row.Mfg2Implemented, index),
                Mfg1Implemented = FlagText(row.Mfg1Implemented, index),
                QcImplemented = FlagText(row.QcImplemented, index),
                MeImplemented = FlagText(row.MeImplemented, index),
            };
        }
    }

    /// <summary>
    /// 指定明细行已实施时返回勾
    /// </summary>
    /// <param name="flags">该部门的实施标记</param>
    /// <param name="index">行序号</param>
    /// <returns>勾或空</returns>
    private static string FlagText(IReadOnlyList<int>? flags, int index)
    {
        if (flags == null || index < 0 || index >= flags.Count || flags[index] != 1)
        {
            return string.Empty;
        }
        return "√";
    }

    /// <summary>
    /// 列表与导出的主表条件。实施状态在主表分页时用执行表是否存在来过滤，不把明细装进内存
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcTe, bool>> ListExpression(TaktEcKanbanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcTe>();
        exp.And(QueryExpression(queryDto));
        AppendImplementationStatus(exp, queryDto?.ImplementationStatus);
        return exp.ToExpression();
    }

    /// <summary>
    /// 按实施状态追加执行表条件。有未实施执行行视为进行中；全部已实施视为完成；没有任何执行行视为未开始
    /// </summary>
    /// <param name="exp">主表条件</param>
    /// <param name="status">实施状态</param>
    private static void AppendImplementationStatus(Expressionable<TaktEcTe> exp, int? status)
    {
        if (status == TaktEcImplementationStatusConstants.InProgress)
        {
            exp.And(x =>
                SqlFunc.Subqueryable<TaktEcPc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcPur>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcIqc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcMc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcMtl>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcMfg2>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcMfg1>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcQc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                || SqlFunc.Subqueryable<TaktEcMe>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsSopUpdated != 1).Any());
            return;
        }
        if (status == TaktEcImplementationStatusConstants.FullyCompleted
            || status == TaktEcImplementationStatusConstants.OfficiallyCompleted)
        {
            exp.And(x =>
                (SqlFunc.Subqueryable<TaktEcPc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcPur>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcIqc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcMc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcMtl>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcMfg2>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcMfg1>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcQc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                    || SqlFunc.Subqueryable<TaktEcMe>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any())
                && !SqlFunc.Subqueryable<TaktEcPc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcPur>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcIqc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcMc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcMtl>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcMfg2>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcMfg1>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcQc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsImplemented != 1).Any()
                && !SqlFunc.Subqueryable<TaktEcMe>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0 && s.IsSopUpdated != 1).Any());
            return;
        }
        if (status == TaktEcImplementationStatusConstants.NotStarted)
        {
            exp.And(x =>
                !SqlFunc.Subqueryable<TaktEcPc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcPur>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcIqc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcMc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcMtl>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcMfg2>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcMfg1>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcQc>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any()
                && !SqlFunc.Subqueryable<TaktEcMe>().Where(s => s.TenantCode == x.TenantCode && s.CompanyCode == x.CompanyCode && s.PlantCode == x.PlantCode && s.EcCode == x.EcCode && s.IsDeleted == 0 && s.IsObsolete == 0).Any());
        }
    }

    /// <summary>
    /// 构建看板查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcTe, bool>> QueryExpression(TaktEcKanbanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcTe>();
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

        if (queryDto?.EcEntryDateStart.HasValue == true)
        {
            var start = queryDto.EcEntryDateStart.Value.Date;
            exp = exp.And(x => x.EcEntryDate >= start);
        }

        // 结束日含当天全日（避免仅比较到 00:00:00 漏掉同日录入）
        if (queryDto?.EcEntryDateEnd.HasValue == true)
        {
            var endExclusive = queryDto.EcEntryDateEnd.Value.Date.AddDays(1);
            exp = exp.And(x => x.EcEntryDate < endExclusive);
        }

        // 看板仅展示全仕向 / 部管；内部、技术不进入看板
        exp = exp.And(x =>
            x.EcImplScope == TaktEcScopeConstants.AllDestination
            || x.EcImplScope == TaktEcScopeConstants.MaterialControl);
        return exp.ToExpression();
    }
}
