// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptMatrixService.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行矩阵视图服务（8 张部门表内存投影 + 转置/统计；供看板/部管/批次页）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
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
/// 设变部门执行矩阵视图服务（跨 8 张 TaktEc* 部门表，无统一 EcExec 实体）
/// </summary>
public class TaktEcDeptMatrixService : TaktServiceBase, ITaktEcDeptMatrixService
{
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly TaktEcExecDeptAccess _ecExecDeptAccess;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="ecExecDeptAccess">设变部门执行跨表访问</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecTeRepository">设变技术课主表仓储</param>
    /// <param name="ecAttachmentRepository">设变附件仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcDeptMatrixService(
        TaktEcExecPersistence ecExecPersistence,
        TaktEcExecDeptAccess ecExecDeptAccess,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcTe> ecTeRepository,
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecExecPersistence = ecExecPersistence;
        _ecExecDeptAccess = ecExecDeptAccess;
        _ecDetailRepository = ecDetailRepository;
        _ecTeRepository = ecTeRepository;
        _ecAttachmentRepository = ecAttachmentRepository;
    }

    /// <summary>
    /// 统计部门执行行数（8 张部门表聚合）
    /// </summary>
    /// <param name="isImplemented">是否实施（0=否 1=是；空=全部）</param>
    /// <returns>部门执行行数量</returns>
    public async Task<int> CountDeptExecutionRowsAsync(int? isImplemented = null)
    {
        EnsureThreeLayerContext();
        return await _ecExecPersistence.CountAllDeptRowsForScopeAsync(
            CurrentTenantCode,
            CurrentCompanyCode,
            isImplemented);
    }

    /// <summary>
    /// 获取设变部门执行统计（设变单数 + 明细数 + 部门行数）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设变部门执行统计</returns>
    public async Task<TaktEcExecStatDto> GetEcDeptExecutionStatAsync(TaktEcExecStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.EcEntryDateStart,
            queryDto.EcEntryDateEnd);
        var ecIdsInRange = (await _ecTeRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EcEntryDate >= start
            && x.EcEntryDate <= end))
            .Select(x => x.Id)
            .ToHashSet();
        var ecDetails = ecIdsInRange.Count == 0
            ? []
            : await _ecDetailRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && ecIdsInRange.Contains(x.EcTeId));
        var detailIds = ecDetails.Select(x => x.Id).ToList();
        var ecDeptCount = 0;
        if (detailIds.Count > 0)
        {
            var execRows = await _ecExecDeptAccess.ListBaseByEcDetailIdsAsync(detailIds);
            ecDeptCount = execRows.Count(x =>
                (string.IsNullOrEmpty(queryDto.DeptCode) || x.DeptCode == queryDto.DeptCode)
                && (!queryDto.IsImplemented.HasValue || x.IsImplemented == queryDto.IsImplemented.Value));
        }
        return new TaktEcExecStatDto
        {
            StatMonth = statMonth,
            EcCount = ecDetails.Select(x => x.EcTeId).Distinct().Count(),
            EcDetailCount = ecDetails.Count,
            EcExecCount = ecDeptCount,
            DeptCode = queryDto.DeptCode,
        };
    }

    // ========================================
    // 设变部门执行转置
    // ========================================

    /// <summary>
    /// 获取设变部门执行转置列表（分页；行=设变明细，列=各部门实施状态）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分页结果</returns>
    public async Task<TaktEcExecTransposedResultDto> GetEcDeptTransposedListAsync(TaktEcExecTransposedQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = TransposedQueryExpression(queryDto);
        var (details, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        var detailIds = details.Select(x => x.Id).ToList();
        var ecIds = details.Select(x => x.EcTeId).Distinct().ToList();
        var deptGroups = await LoadTransposedDeptGroupsAsync(detailIds);
        var ecMap = await LoadEcMapAsync(ecIds);
        var rows = details.Select(detail => BuildTransposedRow(detail, ecMap, deptGroups)).ToList();
        return new TaktEcExecTransposedResultDto
        {
            Paged = TaktPagedResult<TaktEcExecTransposedDto>.Create(
                rows,
                total,
                queryDto.PageIndex,
                queryDto.PageSize),
            DeptCodeOrder = TaktEcDeptCodes.TransposedOrder,
        };
    }

    /// <summary>
    /// 批量加载设变主表映射
    /// </summary>
    /// <param name="ecIds">设变主表 ID 列表</param>
    /// <returns>主表 ID 映射</returns>
    private async Task<Dictionary<long, TaktEcTe>> LoadEcMapAsync(IReadOnlyList<long> ecIds)
    {
        if (ecIds.Count == 0)
        {
            return new Dictionary<long, TaktEcTe>();
        }
        var ecs = await _ecTeRepository.GetListAsync(x => ecIds.Contains(x.Id));
        return ecs.ToDictionary(x => x.Id);
    }

    /// <summary>
    /// 按明细 ID 加载全部部门记录分组
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>明细 ID → 部门记录列表</returns>
    private async Task<Dictionary<long, List<object>>> LoadTransposedDeptGroupsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return new Dictionary<long, List<object>>();
        }
        return await _ecExecPersistence.LoadGroupsAllDeptsAsync(detailIds);
    }

    /// <summary>
    /// 合并明细、主表与部门记录为转置行
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="ecMap">主表映射</param>
    /// <param name="deptGroups">部门分组</param>
    /// <returns>转置行 DTO</returns>
    private static TaktEcExecTransposedDto BuildTransposedRow(
        TaktEcDetail detail,
        IReadOnlyDictionary<long, TaktEcTe> ecMap,
        IReadOnlyDictionary<long, List<object>> deptGroups)
    {
        ecMap.TryGetValue(detail.EcTeId, out var ec);
        deptGroups.TryGetValue(detail.Id, out var deptList);
        deptList ??= [];
        var deptByCode = deptList.ToDictionary(TaktEcDeptEntityHelper.GetDeptCode, StringComparer.Ordinal);
        var cells = new Dictionary<string, TaktEcExecTransposedCellDto>(StringComparer.Ordinal);
        foreach (var code in TaktEcDeptCodes.TransposedOrder)
        {
            deptByCode.TryGetValue(code, out var dept);
            cells[code] = MapTransposedCell(code, dept);
        }
        return new TaktEcExecTransposedDto
        {
            EcDetailId = detail.Id,
            EcTeId = detail.EcTeId,
            LineNumber = detail.LineNumber,
            EcIssueDate = ec?.EcIssueDate ?? default,
            EcLeader = ec?.EcLeader ?? string.Empty,
            EcCode = detail.EcCode,
            EcModelCode = detail.EcModelCode,
            EcNewMaterialCode = detail.EcNewMaterialCode,
            DeptCells = cells,
        };
    }

    /// <summary>
    /// 映射部门记录为转置单元格 DTO
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="dept">部门记录（可为空）</param>
    /// <returns>单元格 DTO</returns>
    private static TaktEcExecTransposedCellDto MapTransposedCell(string deptCode, object? dept)
    {
        if (dept == null)
        {
            var empty = TaktEcExecTransposedHelper.BuildCell(deptCode, 0, null);
            return new TaktEcExecTransposedCellDto
            {
                DeptCode = empty.DeptCode,
                IsImplemented = empty.IsImplemented,
                CompletedDate = empty.CompletedDate,
                DisplayText = empty.DisplayText,
            };
        }
        var completedDate = TaktEcDeptEntityHelper.ResolveTransposedCompletedDate(dept);
        var cell = TaktEcExecTransposedHelper.BuildCell(
            deptCode,
            TaktEcDeptEntityHelper.GetIsImplemented(dept),
            completedDate);
        return new TaktEcExecTransposedCellDto
        {
            DeptCode = cell.DeptCode,
            IsImplemented = cell.IsImplemented,
            CompletedDate = cell.CompletedDate,
            DisplayText = cell.DisplayText,
        };
    }

    /// <summary>
    /// 构建转置列表查询表达式（基于设变明细 + 主表发行日期/负责人）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> TransposedQueryExpression(TaktEcExecTransposedQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        if (!string.IsNullOrEmpty(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcModelCode != null && x.EcModelCode.Contains(keywords))
                || (x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(keywords))
                || (x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcModelCode))
        {
            exp = exp.And(x => x.EcModelCode != null && x.EcModelCode.Contains(queryDto.EcModelCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcNewMaterialCode))
        {
            exp = exp.And(x => x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(queryDto.EcNewMaterialCode));
        }
        if (queryDto.EcIssueDateStart.HasValue)
        {
            var start = queryDto.EcIssueDateStart.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcIssueDate >= start)
                .Any());
        }
        if (queryDto.EcIssueDateEnd.HasValue)
        {
            var end = queryDto.EcIssueDateEnd.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcIssueDate <= end)
                .Any());
        }
        if (!string.IsNullOrEmpty(queryDto.EcLeader))
        {
            var leader = queryDto.EcLeader;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcLeader != null && ec.EcLeader.Contains(leader))
                .Any());
        }
        if (!string.IsNullOrEmpty(queryDto.DeptCode) && queryDto.IsImplemented.HasValue)
        {
            var deptCode = queryDto.DeptCode;
            var flag = queryDto.IsImplemented.Value;
            exp = deptCode switch
            {
                TaktEcDeptCodes.Pc => exp.And(x => SqlFunc.Subqueryable<TaktEcPc>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Pur => exp.And(x => SqlFunc.Subqueryable<TaktEcPur>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Iqc => exp.And(x => SqlFunc.Subqueryable<TaktEcIqc>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mc => exp.And(x => SqlFunc.Subqueryable<TaktEcMc>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mfg2 => exp.And(x =>
                    SqlFunc.Subqueryable<TaktEcMtl>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()
                    || SqlFunc.Subqueryable<TaktEcMfg2>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mfg1 => exp.And(x => SqlFunc.Subqueryable<TaktEcMfg1>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Qc => exp.And(x => SqlFunc.Subqueryable<TaktEcQc>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Me => exp.And(x => SqlFunc.Subqueryable<TaktEcMe>().Where(d => d.EcCode == x.EcCode && d.IsImplemented == flag).Any()),
                _ => exp
            };
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        return exp.ToExpression();
    }

    // ========================================
    // 设变批次转置
    // ========================================

    /// <summary>
    /// 获取设变批次转置列表（分页）。一行一个设变号，各部门列出自己的全部执行行
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>批次转置分页结果</returns>
    public async Task<TaktEcExecBatchTransposedResultDto> GetEcBatchTransposedListAsync(TaktEcExecBatchTransposedQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var matched = await _ecDetailRepository.GetListAsync(BatchTransposedQueryExpression(queryDto));
        var ecIds = matched.Select(x => x.EcTeId).Distinct().ToList();
        var codes = matched
            .Select(x => x.EcCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var pageIndex = Math.Max(1, queryDto.PageIndex);
        var pageSize = Math.Max(1, queryDto.PageSize);
        if (codes.Count == 0)
        {
            return new TaktEcExecBatchTransposedResultDto
            {
                Paged = TaktPagedResult<TaktEcExecBatchTransposedDto>.Create([], 0, pageIndex, pageSize),
                StageCodeOrder = TaktEcBatchStageCodes.TransposedOrder,
            };
        }
        var ecMap = await LoadEcMapAsync(ecIds);
        var details = await ListBatchDetailsAsync(ecIds);
        var attachmentGroups = await LoadAttachmentGroupsAsync(ecIds);
        var stageLines = await _ecExecDeptAccess.ListBatchStageLinesByEcCodesAsync(codes);
        var linesByCode = stageLines
            .Where(x => !string.IsNullOrWhiteSpace(x.EcCode))
            .GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.ToList(), StringComparer.OrdinalIgnoreCase);
        var rows = new List<TaktEcExecBatchTransposedDto>();
        foreach (var group in details
            .Where(x => x.IsObsolete == 0 && !string.IsNullOrWhiteSpace(x.EcCode))
            .GroupBy(x => x.EcCode, StringComparer.OrdinalIgnoreCase))
        {
            var sample = group.ToList();
            var master = sample
                .Select(x => ecMap.TryGetValue(x.EcTeId, out var ec) ? ec : null)
                .Where(x => x != null)
                .OrderByDescending(x => x!.EcIssueDate)
                .FirstOrDefault();
            var attachments = sample
                .Select(x => x.EcTeId)
                .Distinct()
                .SelectMany(id => attachmentGroups.TryGetValue(id, out var list) ? list : [])
                .ToList();
            linesByCode.TryGetValue(group.Key, out var lines);
            rows.Add(BuildBatchTransposedRow(master, sample, attachments, lines ?? []));
        }
        var ordered = rows
            .OrderByDescending(x => x.EcIssueDate)
            .ThenBy(x => x.EcCode, StringComparer.Ordinal)
            .ToList();
        var skip = checked((pageIndex - 1) * pageSize);
        return new TaktEcExecBatchTransposedResultDto
        {
            Paged = TaktPagedResult<TaktEcExecBatchTransposedDto>.Create(
                ordered.Skip(skip).Take(pageSize).ToList(),
                ordered.Count,
                pageIndex,
                pageSize),
            StageCodeOrder = TaktEcBatchStageCodes.TransposedOrder,
        };
    }

    /// <summary>
    /// 分批读取未作废明细，供同一设变号汇总机种和成品
    /// </summary>
    /// <param name="ecTeIds">技术主表 ID</param>
    /// <returns>明细</returns>
    private async Task<List<TaktEcDetail>> ListBatchDetailsAsync(IReadOnlyList<long> ecTeIds)
    {
        var details = new List<TaktEcDetail>();
        const int batchSize = 200;
        for (var offset = 0; offset < ecTeIds.Count; offset += batchSize)
        {
            var chunk = ecTeIds.Skip(offset).Take(batchSize).ToList();
            details.AddRange(await _ecDetailRepository.GetListAsync(x =>
                chunk.Contains(x.EcTeId) && x.IsDeleted == 0 && x.IsObsolete == 0));
        }
        return details;
    }

    /// <summary>
    /// 按设变主表 ID 加载附件分组
    /// </summary>
    /// <param name="ecIds">设变主表 ID 列表</param>
    /// <returns>主表 ID → 附件列表</returns>
    private async Task<Dictionary<long, List<TaktEcAttachment>>> LoadAttachmentGroupsAsync(IReadOnlyList<long> ecIds)
    {
        if (ecIds.Count == 0)
        {
            return new Dictionary<long, List<TaktEcAttachment>>();
        }
        var attachments = await _ecAttachmentRepository.GetListAsync(x => ecIds.Contains(x.EcTeId));
        return attachments
            .GroupBy(x => x.EcTeId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>
    /// 合并一个设变号的主表、明细、附件和各部门执行行
    /// </summary>
    /// <param name="master">技术主表</param>
    /// <param name="details">该设变号的明细</param>
    /// <param name="attachments">附件</param>
    /// <param name="lines">各部门执行行</param>
    /// <returns>批次转置行</returns>
    private static TaktEcExecBatchTransposedDto BuildBatchTransposedRow(
        TaktEcTe? master,
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<TaktEcAttachment> attachments,
        IReadOnlyList<TaktEcBatchStageLine> lines)
    {
        var first = details[0];
        var stageCells = new Dictionary<string, TaktEcExecBatchTransposedStageDto>(StringComparer.Ordinal);
        foreach (var stageCode in TaktEcBatchStageCodes.TransposedOrder)
        {
            var stageLines = BuildDeptDetailLines(stageCode, master, details, lines);
            var head = stageLines.FirstOrDefault();
            stageCells[stageCode] = new TaktEcExecBatchTransposedStageDto
            {
                StageCode = stageCode,
                Lines = stageLines,
                StageDate = head?.StageDate,
                BatchNo = head?.BatchCode,
                BatchCode = head?.BatchCode ?? string.Empty,
                DateDisplayText = head?.DateDisplayText,
            };
        }
        return new TaktEcExecBatchTransposedDto
        {
            EcDetailId = first.Id,
            EcTeId = master?.Id ?? first.EcTeId,
            LineNumber = first.LineNumber,
            EcCode = first.EcCode,
            TechnicalLiaisonNo = JoinAttachmentDocCodes(attachments, TaktEcAttachmentTypeConstants.Liaison),
            PNo = JoinAttachmentDocCodes(attachments, TaktEcAttachmentTypeConstants.Fpp),
            TcjLiaisonNo = JoinAttachmentDocCodes(attachments, TaktEcAttachmentTypeConstants.Tcj),
            EcIssueDate = master?.EcIssueDate ?? master?.EcEntryDate ?? default,
            EcModelCode = JoinDistinct(details.Select(x => x.EcModelCode)),
            EcNewMaterialCode = JoinDistinct(details.Select(x => x.EcRootMaterialCode)),
            EcEntryDate = master?.EcEntryDate ?? default,
            StageCells = stageCells,
        };
    }

    /// <summary>
    /// 一个部门按可见明细逐行展开。同一设变加机种的多条明细各自占一行，日期和批次取对应执行行
    /// </summary>
    /// <param name="stageCode">阶段编码</param>
    /// <param name="master">技术主表</param>
    /// <param name="details">该设变号的明细</param>
    /// <param name="lines">该设变号的执行行</param>
    /// <returns>阶段行</returns>
    private static List<TaktEcExecBatchTransposedStageLineDto> BuildDeptDetailLines(
        string stageCode,
        TaktEcTe? master,
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<TaktEcBatchStageLine> lines)
    {
        var deptCode = DeptCodeOfStage(stageCode);
        var controlScope = master?.EcControlScope ?? TaktEcControlScopeConstants.ByModel;
        var implScope = master?.EcImplScope ?? 0;
        var execByGrain = new Dictionary<string, TaktEcBatchStageLine>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in lines.Where(x => string.Equals(x.StageCode, stageCode, StringComparison.Ordinal)))
        {
            var grain = line.EcGrainKey ?? string.Empty;
            if (!execByGrain.ContainsKey(grain))
            {
                execByGrain[grain] = line;
            }
        }
        var result = new List<TaktEcExecBatchTransposedStageLineDto>();
        foreach (var detail in details.OrderBy(x => x.LineNumber).ThenBy(x => x.Id))
        {
            if (!TaktEcScopeRowGrain.IsEligible(detail, deptCode, implScope, null))
            {
                continue;
            }
            execByGrain.TryGetValue(TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, controlScope), out var exec);
            var cell = TaktEcExecBatchTransposedHelper.BuildStageCell(stageCode, exec?.StageDate, exec?.BatchCode);
            result.Add(new TaktEcExecBatchTransposedStageLineDto
            {
                StageDate = cell.StageDate,
                DateDisplayText = cell.DateDisplayText,
                BatchCode = cell.BatchCode ?? string.Empty,
            });
        }
        return result;
    }

    /// <summary>
    /// 批次阶段对应的执行部门
    /// </summary>
    /// <param name="stageCode">阶段编码</param>
    /// <returns>部门编码</returns>
    private static string DeptCodeOfStage(string stageCode)
    {
        if (stageCode == TaktEcBatchStageCodes.Scheduled)
        {
            return TaktEcDeptCodes.Pc;
        }
        if (stageCode == TaktEcBatchStageCodes.PurchaseOrder)
        {
            return TaktEcDeptCodes.Pur;
        }
        if (stageCode == TaktEcBatchStageCodes.IncomingInspection)
        {
            return TaktEcDeptCodes.Iqc;
        }
        if (stageCode == TaktEcBatchStageCodes.Outbound)
        {
            return TaktEcDeptCodes.Mc;
        }
        if (stageCode == TaktEcBatchStageCodes.ElectronicOutbound)
        {
            return TaktEcDeptCodes.Mtl;
        }
        if (stageCode == TaktEcBatchStageCodes.Mfg2Production)
        {
            return TaktEcDeptCodes.Mfg2;
        }
        if (stageCode == TaktEcBatchStageCodes.Mfg1Production)
        {
            return TaktEcDeptCodes.Mfg1;
        }
        if (stageCode == TaktEcBatchStageCodes.SampleInspection)
        {
            return TaktEcDeptCodes.Qc;
        }
        return TaktEcDeptCodes.Me;
    }

    /// <summary>
    /// 同一类别的文件编码去重后用顿号连接
    /// </summary>
    /// <param name="attachments">附件</param>
    /// <param name="attachmentType">文件类别</param>
    /// <returns>文件编码</returns>
    private static string JoinAttachmentDocCodes(IReadOnlyList<TaktEcAttachment> attachments, string attachmentType)
    {
        return JoinDistinct(attachments
            .Where(x => x.IsObsolete == 0 && string.Equals(x.AttachmentType, attachmentType, StringComparison.OrdinalIgnoreCase))
            .Select(x => x.DocCode));
    }

    /// <summary>
    /// 去重后用顿号连接
    /// </summary>
    /// <param name="values">文本</param>
    /// <returns>展示文本</returns>
    private static string JoinDistinct(IEnumerable<string?> values)
    {
        return string.Join(
            "、",
            values.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.Ordinal));
    }

    /// <summary>
    /// 构建批次转置列表查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> BatchTransposedQueryExpression(TaktEcExecBatchTransposedQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
            .Where(ec => ec.Id == x.EcTeId
                && ec.IsDeleted == 0
                && (ec.EcImplScope == TaktEcScopeConstants.AllDestination
                    || ec.EcImplScope == TaktEcScopeConstants.MaterialControl))
            .Any());
        exp = exp.And(x =>
            SqlFunc.Subqueryable<TaktEcPur>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcPc>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcIqc>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcMc>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcMtl>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcMfg2>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcMfg1>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcQc>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcMe>().Where(d => d.EcCode == x.EcCode && d.IsDeleted == 0 && d.IsObsolete == 0).Any());
        if (!string.IsNullOrEmpty(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcModelCode != null && x.EcModelCode.Contains(keywords))
                || (x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(keywords))
                || (x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcModelCode))
        {
            exp = exp.And(x => x.EcModelCode != null && x.EcModelCode.Contains(queryDto.EcModelCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcNewMaterialCode))
        {
            exp = exp.And(x => x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(queryDto.EcNewMaterialCode));
        }
        if (queryDto.EcIssueDateStart.HasValue)
        {
            var start = queryDto.EcIssueDateStart.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcIssueDate >= start)
                .Any());
        }
        if (queryDto.EcIssueDateEnd.HasValue)
        {
            var end = queryDto.EcIssueDateEnd.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcIssueDate <= end)
                .Any());
        }
        if (queryDto.EcEntryDateStart.HasValue)
        {
            var entryStart = queryDto.EcEntryDateStart.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcEntryDate >= entryStart)
                .Any());
        }
        if (queryDto.EcEntryDateEnd.HasValue)
        {
            var entryEnd = queryDto.EcEntryDateEnd.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcTe>()
                .Where(ec => ec.Id == x.EcTeId && ec.EcEntryDate <= entryEnd)
                .Any());
        }
        if (!string.IsNullOrEmpty(queryDto.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x =>
                SqlFunc.Subqueryable<TaktEcPur>()
                    .Where(d => d.EcCode == x.EcCode && d.PurchaseOrderCode != null && d.PurchaseOrderCode.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcPc>()
                    .Where(d => d.EcCode == x.EcCode && d.ScheduledBatch != null && d.ScheduledBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcIqc>()
                    .Where(d => d.EcCode == x.EcCode && d.IqcOrderCode != null && d.IqcOrderCode.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcMc>()
                    .Where(d => d.EcCode == x.EcCode && d.OutboundBatch != null && d.OutboundBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcMtl>()
                    .Where(d => d.EcCode == x.EcCode && d.OutboundBatch != null && d.OutboundBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcMfg2>()
                    .Where(d => d.EcCode == x.EcCode && d.ImplementationBatch != null && d.ImplementationBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcMfg1>()
                    .Where(d => d.EcCode == x.EcCode && d.ImplementationBatch != null && d.ImplementationBatch.Contains(batchCode))
                    .Any());
        }
        return exp.ToExpression();
    }
}
