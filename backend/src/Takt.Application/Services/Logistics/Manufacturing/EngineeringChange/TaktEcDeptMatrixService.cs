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
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecEngRepository;
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="ecExecDeptAccess">设变部门执行跨表访问</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecEngRepository">设变技术课主表仓储</param>
    /// <param name="ecAttachmentRepository">设变附件仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcDeptMatrixService(
        TaktEcExecPersistence ecExecPersistence,
        TaktEcExecDeptAccess ecExecDeptAccess,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcGijutsu> ecEngRepository,
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecExecPersistence = ecExecPersistence;
        _ecExecDeptAccess = ecExecDeptAccess;
        _ecDetailRepository = ecDetailRepository;
        _ecEngRepository = ecEngRepository;
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
        var ecIdsInRange = (await _ecEngRepository.GetListAsync(x =>
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
                && ecIdsInRange.Contains(x.EcGijutsuId));
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
            EcCount = ecDetails.Select(x => x.EcGijutsuId).Distinct().Count(),
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
        var ecIds = details.Select(x => x.EcGijutsuId).Distinct().ToList();
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
    private async Task<Dictionary<long, TaktEcGijutsu>> LoadEcMapAsync(IReadOnlyList<long> ecIds)
    {
        if (ecIds.Count == 0)
        {
            return new Dictionary<long, TaktEcGijutsu>();
        }
        var ecs = await _ecEngRepository.GetListAsync(x => ecIds.Contains(x.Id));
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
        IReadOnlyDictionary<long, TaktEcGijutsu> ecMap,
        IReadOnlyDictionary<long, List<object>> deptGroups)
    {
        ecMap.TryGetValue(detail.EcGijutsuId, out var ec);
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
            EcGijutsuId = detail.EcGijutsuId,
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
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcGijutsu>()
                .Where(ec => ec.Id == x.EcGijutsuId && ec.EcIssueDate >= start)
                .Any());
        }
        if (queryDto.EcIssueDateEnd.HasValue)
        {
            var end = queryDto.EcIssueDateEnd.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcGijutsu>()
                .Where(ec => ec.Id == x.EcGijutsuId && ec.EcIssueDate <= end)
                .Any());
        }
        if (!string.IsNullOrEmpty(queryDto.EcLeader))
        {
            var leader = queryDto.EcLeader;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcGijutsu>()
                .Where(ec => ec.Id == x.EcGijutsuId && ec.EcLeader != null && ec.EcLeader.Contains(leader))
                .Any());
        }
        if (!string.IsNullOrEmpty(queryDto.DeptCode) && queryDto.IsImplemented.HasValue)
        {
            var deptCode = queryDto.DeptCode;
            var flag = queryDto.IsImplemented.Value;
            exp = deptCode switch
            {
                TaktEcDeptCodes.Pmc => exp.And(x => SqlFunc.Subqueryable<TaktEcSeikan>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mp => exp.And(x => SqlFunc.Subqueryable<TaktEcKoubai>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Iqc => exp.And(x => SqlFunc.Subqueryable<TaktEcUkeken>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mc => exp.And(x => SqlFunc.Subqueryable<TaktEcBukan>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Pcba => exp.And(x =>
                    SqlFunc.Subqueryable<TaktEcEmtl>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()
                    || SqlFunc.Subqueryable<TaktEcSeizounika>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Assy => exp.And(x => SqlFunc.Subqueryable<TaktEcSeizouikka>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Qa => exp.And(x => SqlFunc.Subqueryable<TaktEcHinkan>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Te => exp.And(x => SqlFunc.Subqueryable<TaktEcSeizougijutsu>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
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
    /// 获取设变批次转置列表（分页）。先取生管/部管/SMT/制二/制一/品管执行记录指向的明细，再反查明细表展示
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>批次转置分页结果</returns>
    public async Task<TaktEcExecBatchTransposedResultDto> GetEcBatchTransposedListAsync(TaktEcExecBatchTransposedQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = BatchTransposedQueryExpression(queryDto);
        var (details, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        var detailIds = details.Select(x => x.Id).ToList();
        var ecIds = details.Select(x => x.EcGijutsuId).Distinct().ToList();
        var deptGroups = await LoadTransposedDeptGroupsAsync(detailIds);
        var ecMap = await LoadEcMapAsync(ecIds);
        var attachmentGroups = await LoadAttachmentGroupsAsync(ecIds);
        var rows = details.Select(detail =>
            BuildBatchTransposedRow(detail, ecMap, deptGroups, attachmentGroups)).ToList();
        return new TaktEcExecBatchTransposedResultDto
        {
            Paged = TaktPagedResult<TaktEcExecBatchTransposedDto>.Create(
                rows,
                total,
                queryDto.PageIndex,
                queryDto.PageSize),
            StageCodeOrder = TaktEcBatchStageCodes.TransposedOrder,
        };
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
        var attachments = await _ecAttachmentRepository.GetListAsync(x => ecIds.Contains(x.EcGijutsuId));
        return attachments
            .GroupBy(x => x.EcGijutsuId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>
    /// 合并明细、主表、附件与部门记录为批次转置行
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="ecMap">主表映射</param>
    /// <param name="deptGroups">部门分组</param>
    /// <param name="attachmentGroups">附件分组</param>
    /// <returns>批次转置行 DTO</returns>
    private static TaktEcExecBatchTransposedDto BuildBatchTransposedRow(
        TaktEcDetail detail,
        IReadOnlyDictionary<long, TaktEcGijutsu> ecMap,
        IReadOnlyDictionary<long, List<object>> deptGroups,
        IReadOnlyDictionary<long, List<TaktEcAttachment>> attachmentGroups)
    {
        ecMap.TryGetValue(detail.EcGijutsuId, out var ec);
        deptGroups.TryGetValue(detail.Id, out var deptList);
        deptList ??= [];
        attachmentGroups.TryGetValue(detail.EcGijutsuId, out var attachments);
        attachments ??= [];
        var pmc = TaktEcDeptEntityHelper.FindByDeptCode(deptList, TaktEcDeptCodes.Pmc) as TaktEcSeikan;
        var mc = TaktEcDeptEntityHelper.FindByDeptCode(deptList, TaktEcDeptCodes.Mc) as TaktEcBukan;
        var smt = deptList.OfType<TaktEcEmtl>().FirstOrDefault();
        var pcbaSeizounika = deptList.OfType<TaktEcSeizounika>().FirstOrDefault();
        var assy = TaktEcDeptEntityHelper.FindByDeptCode(deptList, TaktEcDeptCodes.Assy) as TaktEcSeizouikka;
        var qa = TaktEcDeptEntityHelper.FindByDeptCode(deptList, TaktEcDeptCodes.Qa);
        var stageCells = new Dictionary<string, TaktEcExecBatchTransposedStageDto>(StringComparer.Ordinal)
        {
            [TaktEcBatchStageCodes.Scheduled] = MapBatchStageCell(
                TaktEcBatchStageCodes.Scheduled,
                pmc?.ScheduledDate,
                pmc?.ScheduledBatch),
            [TaktEcBatchStageCodes.Outbound] = MapBatchStageCell(
                TaktEcBatchStageCodes.Outbound,
                mc?.OutboundDate,
                mc?.OutboundBatch),
            [TaktEcBatchStageCodes.PcbaProduction] = MapBatchStageCell(
                TaktEcBatchStageCodes.PcbaProduction,
                smt?.OutboundDate ?? pcbaSeizounika?.ProductionDate,
                smt?.OutboundBatch ?? pcbaSeizounika?.ImplementationBatch),
            [TaktEcBatchStageCodes.AssyProduction] = MapBatchStageCell(
                TaktEcBatchStageCodes.AssyProduction,
                assy?.ProductionDate,
                assy?.ProductionTeam),
            [TaktEcBatchStageCodes.SampleInspection] = MapBatchStageCell(
                TaktEcBatchStageCodes.SampleInspection,
                TaktEcDeptEntityHelper.ResolveSampleInspectionDate(qa),
                null),
        };
        return new TaktEcExecBatchTransposedDto
        {
            EcDetailId = detail.Id,
            EcGijutsuId = detail.EcGijutsuId,
            LineNumber = detail.LineNumber,
            EcCode = detail.EcCode,
            TechnicalLiaisonNo = FindAttachmentDocCode(attachments, TaktEcAttachmentTypeConstants.Liaison),
            PNo = FindAttachmentDocCode(attachments, TaktEcAttachmentTypeConstants.Fpp),
            TcjLiaisonNo = FindAttachmentDocCode(attachments, TaktEcAttachmentTypeConstants.Tcj),
            EcIssueDate = ec?.EcIssueDate ?? ec?.EcEntryDate ?? default,
            EcModelCode = detail.EcModelCode,
            EcNewMaterialCode = detail.EcNewMaterialCode,
            EcEntryDate = ec?.EcEntryDate ?? default,
            StageCells = stageCells,
        };
    }

    /// <summary>
    /// 按附件类别取文件编码
    /// </summary>
    /// <param name="attachments">附件列表</param>
    /// <param name="attachmentType">文件类别</param>
    /// <returns>文件编码</returns>
    private static string? FindAttachmentDocCode(IReadOnlyList<TaktEcAttachment> attachments, string attachmentType)
    {
        return attachments
            .FirstOrDefault(x => string.Equals(x.AttachmentType, attachmentType, StringComparison.OrdinalIgnoreCase))
            ?.DocCode;
    }

    /// <summary>
    /// 映射批次阶段单元格 DTO
    /// </summary>
    /// <param name="stageCode">阶段编码</param>
    /// <param name="stageDate">阶段日期</param>
    /// <param name="batchCode">批次号</param>
    /// <returns>阶段单元格 DTO</returns>
    private static TaktEcExecBatchTransposedStageDto MapBatchStageCell(
        string stageCode,
        DateTime? stageDate,
        string? batchCode)
    {
        var cell = TaktEcExecBatchTransposedHelper.BuildStageCell(stageCode, stageDate, batchCode);
        return new TaktEcExecBatchTransposedStageDto
        {
            StageCode = cell.StageCode,
            StageDate = cell.StageDate,
            BatchNo = cell.BatchCode,
            BatchCode = cell.BatchCode ?? string.Empty,
            DateDisplayText = cell.DateDisplayText,
        };
    }

    /// <summary>
    /// 构建批次转置列表查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> BatchTransposedQueryExpression(TaktEcExecBatchTransposedQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x =>
            SqlFunc.Subqueryable<TaktEcSeikan>().Where(d => d.EcDetailId == x.Id && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcBukan>().Where(d => d.EcDetailId == x.Id && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcEmtl>().Where(d => d.EcDetailId == x.Id && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcSeizounika>().Where(d => d.EcDetailId == x.Id && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcSeizouikka>().Where(d => d.EcDetailId == x.Id && d.IsDeleted == 0 && d.IsObsolete == 0).Any()
            || SqlFunc.Subqueryable<TaktEcHinkan>().Where(d => d.EcDetailId == x.Id && d.IsDeleted == 0 && d.IsObsolete == 0).Any());
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
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcGijutsu>()
                .Where(ec => ec.Id == x.EcGijutsuId && ec.EcIssueDate >= start)
                .Any());
        }
        if (queryDto.EcIssueDateEnd.HasValue)
        {
            var end = queryDto.EcIssueDateEnd.Value;
            exp = exp.And(x => SqlFunc.Subqueryable<TaktEcGijutsu>()
                .Where(ec => ec.Id == x.EcGijutsuId && ec.EcIssueDate <= end)
                .Any());
        }
        if (!string.IsNullOrEmpty(queryDto.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x =>
                SqlFunc.Subqueryable<TaktEcSeikan>()
                    .Where(d => d.EcDetailId == x.Id && d.ScheduledBatch != null && d.ScheduledBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcBukan>()
                    .Where(d => d.EcDetailId == x.Id && d.OutboundBatch != null && d.OutboundBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcEmtl>()
                    .Where(d => d.EcDetailId == x.Id && d.OutboundBatch != null && d.OutboundBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcSeizounika>()
                    .Where(d => d.EcDetailId == x.Id && d.ImplementationBatch != null && d.ImplementationBatch.Contains(batchCode))
                    .Any()
                || SqlFunc.Subqueryable<TaktEcSeizouikka>()
                    .Where(d => d.EcDetailId == x.Id && d.ProductionTeam != null && d.ProductionTeam.Contains(batchCode))
                    .Any());
        }
        return exp.ToExpression();
    }
}
