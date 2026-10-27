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
    /// 机种、成品列最多展示的去重个数。超出后以省略号结尾
    /// </summary>
    private const int IdentityLabelCap = 8;

    /// <summary>
    /// 机种、成品只抽样这么多明细行。禁止对整张明细做聚合
    /// </summary>
    private const int IdentitySampleRows = 80;

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
        await FillIdentityLabelsAsync(page);
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
            await FillIdentityLabelsAsync([row]);
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
        await FillIdentityLabelsAsync(rows);
        return await TaktExcelHelper.ExportAsync(
            rows.SelectMany(ExpandExportLines).ToList(),
            sheetName ?? "设变看板",
            fileName ?? "设变看板导出.xlsx");
    }

    /// <summary>
    /// 获取设变看板统计。主表按实施状态计数；部门表按已实施/未实施计数；录入期间按月计数
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
        var depts = new List<TaktEcKanbanDeptStatDto>
        {
            await CountDeptBoardAsync(TaktEcDeptCodes.Pc, () => CountDeptImplPairAsync<TaktEcPc>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Pur, () => CountDeptImplPairAsync<TaktEcPur>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Iqc, () => CountDeptImplPairAsync<TaktEcIqc>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Mc, () => CountDeptImplPairAsync<TaktEcMc>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Mtl, () => CountDeptImplPairAsync<TaktEcMtl>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Mfg2, () => CountDeptImplPairAsync<TaktEcMfg2>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Mfg1, () => CountDeptImplPairAsync<TaktEcMfg1>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Qc, () => CountDeptImplPairAsync<TaktEcQc>()),
            await CountDeptBoardAsync(TaktEcDeptCodes.Me, CountMeImplPairAsync),
        };
        return new TaktEcKanbanBoardStatDto
        {
            EcTotal = notStarted + inProgress + officially + fully,
            NotStarted = notStarted,
            InProgress = inProgress,
            OfficiallyCompleted = officially,
            FullyCompleted = fully,
            Depts = depts,
            Months = await CountMonthsAsync(baseQuery),
        };
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
    /// 组装部门统计行
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="pairFactory">已实施/未实施计数</param>
    /// <returns>部门统计</returns>
    private static async Task<TaktEcKanbanDeptStatDto> CountDeptBoardAsync(
        string deptCode,
        Func<Task<(int Implemented, int NotImplemented)>> pairFactory)
    {
        var (implemented, notImplemented) = await pairFactory();
        return new TaktEcKanbanDeptStatDto
        {
            DeptCode = deptCode,
            Implemented = implemented,
            NotImplemented = notImplemented,
        };
    }

    /// <summary>
    /// 一个部门执行表：全仕向/部管、未作废行的已实施与未实施计数
    /// </summary>
    /// <typeparam name="TEntity">部门执行实体</typeparam>
    /// <returns>已实施、未实施</returns>
    private async Task<(int Implemented, int NotImplemented)> CountDeptImplPairAsync<TEntity>()
        where TEntity : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var allDestination = TaktEcScopeConstants.AllDestination;
        var materialControl = TaktEcScopeConstants.MaterialControl;
        var implemented = await _db.Queryable<TEntity>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && x.IsDeleted == 0
                && x.IsObsolete == 0
                && (x.EcImplScope == allDestination || x.EcImplScope == materialControl)
                && x.IsImplemented == 1)
            .CountAsync();
        var notImplemented = await _db.Queryable<TEntity>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && x.IsDeleted == 0
                && x.IsObsolete == 0
                && (x.EcImplScope == allDestination || x.EcImplScope == materialControl)
                && x.IsImplemented != 1)
            .CountAsync();
        return (implemented, notImplemented);
    }

    /// <summary>
    /// 制技执行表：IsImplemented 不落库，按 IsSopUpdated 统计已更新/未更新
    /// </summary>
    /// <returns>已实施（SOP 已更新）、未实施</returns>
    private async Task<(int Implemented, int NotImplemented)> CountMeImplPairAsync()
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var allDestination = TaktEcScopeConstants.AllDestination;
        var materialControl = TaktEcScopeConstants.MaterialControl;
        var implemented = await _db.Queryable<TaktEcMe>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && x.IsDeleted == 0
                && x.IsObsolete == 0
                && (x.EcImplScope == allDestination || x.EcImplScope == materialControl)
                && x.IsSopUpdated == 1)
            .CountAsync();
        var notImplemented = await _db.Queryable<TaktEcMe>()
            .Where(x => x.TenantCode == tenant
                && x.CompanyCode == company
                && x.IsDeleted == 0
                && x.IsObsolete == 0
                && (x.EcImplScope == allDestination || x.EcImplScope == materialControl)
                && x.IsSopUpdated != 1)
            .CountAsync();
        return (implemented, notImplemented);
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
    /// 为一页设变主表组装部门实施标记。只读这些设变号的执行行，不读明细表
    /// </summary>
    /// <param name="masters">当前页或导出范围内的主表</param>
    /// <returns>转置行，顺序与主表一致</returns>
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
        var execByDept = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in codes)
        {
            PutFlags(execByDept, code, TaktEcDeptCodes.Pc, await QueryFlagsAsync<TaktEcPc>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Pur, await QueryFlagsAsync<TaktEcPur>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Iqc, await QueryFlagsAsync<TaktEcIqc>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Mc, await QueryFlagsAsync<TaktEcMc>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Mtl, await QueryFlagsAsync<TaktEcMtl>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Mfg2, await QueryFlagsAsync<TaktEcMfg2>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Mfg1, await QueryFlagsAsync<TaktEcMfg1>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Qc, await QueryFlagsAsync<TaktEcQc>(code));
            PutFlags(execByDept, code, TaktEcDeptCodes.Me, await QueryMeFlagsAsync(code));
        }
        var rows = new List<TaktEcKanbanDto>();
        foreach (var master in masters)
        {
            rows.Add(new TaktEcKanbanDto
            {
                EcTeId = master.Id,
                EcIssueDate = master.EcIssueDate,
                EcLeader = master.EcLeader ?? string.Empty,
                EcCode = master.EcCode,
                EcEntryDate = master.EcEntryDate,
                PurImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Pur, execByDept),
                PcImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Pc, execByDept),
                IqcImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Iqc, execByDept),
                McImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Mc, execByDept),
                MtlImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Mtl, execByDept),
                Mfg2Implemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Mfg2, execByDept),
                Mfg1Implemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Mfg1, execByDept),
                QcImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Qc, execByDept),
                MeImplemented = ExecFlags(master.EcCode, TaktEcDeptCodes.Me, execByDept),
            });
        }
        return rows;
    }

    /// <summary>
    /// 当前页补机种和成品。每个设变只取固定行数，不聚合整张明细
    /// </summary>
    /// <param name="rows">当前页或导出的看板行</param>
    private async Task FillIdentityLabelsAsync(IReadOnlyList<TaktEcKanbanDto> rows)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        foreach (var row in rows)
        {
            var sample = await _db.Queryable<TaktEcDetail>()
                .Where(x => x.EcTeId == row.EcTeId && x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0 && x.IsObsolete == 0)
                .Select(x => new { x.EcModelCode, x.EcRootMaterialCode })
                .Take(IdentitySampleRows)
                .ToListAsync();
            var more = sample.Count >= IdentitySampleRows;
            row.EcModelCode = JoinSample(sample.Select(x => x.EcModelCode), more);
            row.EcRootMaterialCode = JoinSample(sample.Select(x => x.EcRootMaterialCode), more);
        }
    }

    /// <summary>
    /// 抽样值去重后连接。样本已满或个数超过上限时以省略号结尾
    /// </summary>
    /// <param name="values">抽样值</param>
    /// <param name="sampleFull">样本是否已取满</param>
    /// <returns>展示文本</returns>
    private static string JoinSample(IEnumerable<string?> values, bool sampleFull)
    {
        var trimmed = values
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToList();
        if (!sampleFull && trimmed.Count <= IdentityLabelCap)
        {
            return string.Join("、", trimmed);
        }
        return string.Join("、", trimmed.Take(IdentityLabelCap)) + "…";
    }

    /// <summary>
    /// 写入一个部门的实施标记
    /// </summary>
    /// <param name="execByDept">部门加设变单号到标记</param>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="flags">实施标记</param>
    private static void PutFlags(
        Dictionary<string, List<int>> execByDept,
        string ecCode,
        string deptCode,
        List<int> flags)
    {
        if (flags.Count > 0)
        {
            execByDept[DeptKey(deptCode, ecCode)] = flags;
        }
    }

    /// <summary>
    /// 一个设变在一个部门表中的实施标记。数据库只取上限行，不扫描明细
    /// </summary>
    /// <typeparam name="TEntity">部门执行实体</typeparam>
    /// <param name="ecCode">设变单号</param>
    /// <returns>0 或 1 的列表</returns>
    private async Task<List<int>> QueryFlagsAsync<TEntity>(string ecCode)
        where TEntity : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TEntity>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.EcCode == ecCode && x.IsDeleted == 0 && x.IsObsolete == 0)
            .Take(KanbanDeptLineCap)
            .ToListAsync();
        return rows.Select(x => x.IsImplemented == 1 ? 1 : 0).ToList();
    }

    /// <summary>
    /// 制技：按 IsSopUpdated 生成实施标记（IsImplemented 不落库）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>0 或 1 的列表</returns>
    private async Task<List<int>> QueryMeFlagsAsync(string ecCode)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var rows = await _db.Queryable<TaktEcMe>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.EcCode == ecCode && x.IsDeleted == 0 && x.IsObsolete == 0)
            .Take(KanbanDeptLineCap)
            .ToListAsync();
        return rows.Select(x => x.IsSopUpdated == 1 ? 1 : 0).ToList();
    }

    /// <summary>
    /// 该部门已取回的实施标记
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="execByDept">部门加设变单号到标记</param>
    /// <returns>实施标记列表</returns>
    private static List<int> ExecFlags(
        string ecCode,
        string deptCode,
        IReadOnlyDictionary<string, List<int>> execByDept)
    {
        return execByDept.TryGetValue(DeptKey(deptCode, ecCode), out var list) ? list : [];
    }

    /// <summary>
    /// 部门与设变单号索引
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecCode">设变单号</param>
    /// <returns>索引键</returns>
    private static string DeptKey(string deptCode, string ecCode)
    {
        return string.Concat(deptCode, "\u001f", ecCode);
    }

    /// <summary>
    /// 按各部门执行行展开导出。某一行已实施时该格为勾
    /// </summary>
    /// <param name="row">转置行</param>
    /// <returns>导出行</returns>
    private static IEnumerable<TaktEcKanbanExportDto> ExpandExportLines(TaktEcKanbanDto row)
    {
        IReadOnlyList<int>[] groups =
        [
            row.PcImplemented,
            row.PurImplemented,
            row.IqcImplemented,
            row.McImplemented,
            row.MtlImplemented,
            row.Mfg2Implemented,
            row.Mfg1Implemented,
            row.QcImplemented,
            row.MeImplemented,
        ];
        var lineCount = 1;
        foreach (var group in groups)
        {
            if (group != null && group.Count > lineCount)
            {
                lineCount = group.Count;
            }
        }
        for (var index = 0; index < lineCount; index++)
        {
            yield return new TaktEcKanbanExportDto
            {
                EcIssueDate = row.EcIssueDate.ToString("yyyy-MM-dd"),
                EcLeader = row.EcLeader,
                EcCode = row.EcCode,
                EcModelCode = row.EcModelCode,
                EcRootMaterialCode = row.EcRootMaterialCode,
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
