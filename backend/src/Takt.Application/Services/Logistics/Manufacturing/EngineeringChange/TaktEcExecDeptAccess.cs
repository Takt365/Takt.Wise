// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecDeptAccess.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行跨表访问（8 张 TaktEcExec* 部门表，按 DeptCode 路由）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行公共字段快照（跨 8 张子表聚合用）
/// </summary>
public sealed class TaktEcExecBaseRow
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; init; }
    /// <summary>
    /// 设变明细 ID（列表按明细展开时回填；执行表本身不保存该外键）
    /// </summary>
    public long EcDetailId { get; init; }
    /// <summary>
    /// 执行行去重键
    /// </summary>
    public string EcGrainKey { get; init; } = string.Empty;
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; init; } = string.Empty;
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; init; } = string.Empty;
    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; init; }
    /// <summary>
    /// 是否实施
    /// </summary>
    public int IsImplemented { get; init; }
    /// <summary>
    /// 执行内容
    /// </summary>
    public string? ExecContent { get; init; }
    /// <summary>
    /// 是否作废
    /// </summary>
    public int IsObsolete { get; init; }
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; init; } = string.Empty;
    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; init; } = string.Empty;
}

/// <summary>
/// 投入批次某一部门的一行日期和批次
/// </summary>
public sealed class TaktEcBatchStageLine
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; init; } = string.Empty;
    /// <summary>
    /// 阶段编码（TaktEcBatchStageCodes）
    /// </summary>
    public string StageCode { get; init; } = string.Empty;
    /// <summary>
    /// 执行行去重键，用于同一部门内排序
    /// </summary>
    public string EcGrainKey { get; init; } = string.Empty;
    /// <summary>
    /// 阶段日期
    /// </summary>
    public DateTime? StageDate { get; init; }
    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; init; }
}

/// <summary>
/// 设变部门执行跨表访问
/// </summary>
public class TaktEcExecDeptAccess
{
    private readonly ITaktCompanyRepository<TaktEcPc> _pcRepository;
    private readonly ITaktCompanyRepository<TaktEcPur> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcIqc> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcMc> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcMfg2> _pcbaRepository;
    private readonly ITaktCompanyRepository<TaktEcMtl> _smtRepository;
    private readonly ITaktCompanyRepository<TaktEcMfg1> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcQc> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcMe> _teRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcExecDeptAccess(
        ITaktCompanyRepository<TaktEcPc> pcRepository,
        ITaktCompanyRepository<TaktEcPur> mpRepository,
        ITaktCompanyRepository<TaktEcIqc> iqcRepository,
        ITaktCompanyRepository<TaktEcMc> mcRepository,
        ITaktCompanyRepository<TaktEcMfg2> pcbaRepository,
        ITaktCompanyRepository<TaktEcMtl> smtRepository,
        ITaktCompanyRepository<TaktEcMfg1> assyRepository,
        ITaktCompanyRepository<TaktEcQc> qaRepository,
        ITaktCompanyRepository<TaktEcMe> teRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcTe> ecTeRepository)
    {
        _pcRepository = pcRepository;
        _mpRepository = mpRepository;
        _iqcRepository = iqcRepository;
        _mcRepository = mcRepository;
        _pcbaRepository = pcbaRepository;
        _smtRepository = smtRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
        _teRepository = teRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecTeRepository = ecTeRepository;
    }

    /// <summary>
    /// 生管课仓储
    /// </summary>
    public ITaktCompanyRepository<TaktEcPc> PcRepository => _pcRepository;

    /// <summary>
    /// 制造2课（非 F）仓储
    /// </summary>
    public ITaktCompanyRepository<TaktEcMfg2> Mfg2Repository => _pcbaRepository;

    /// <summary>
    /// 物料（F+C003，DeptCode=D0625）仓储
    /// </summary>
    public ITaktCompanyRepository<TaktEcMtl> MtlRepository => _smtRepository;

    /// <summary>
    /// 按设变单号与部门取首条执行记录（公共字段）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>公共字段快照；不存在时 null</returns>
    public async Task<TaktEcExecBaseRow?> FirstBaseByEcCodeAndDeptAsync(string ecCode, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        return deptCode switch
        {
            TaktEcDeptCodes.Pc => ToBaseRow(await _pcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Pur => ToBaseRow(await _mpRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Iqc => ToBaseRow(await _iqcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mc => ToBaseRow(await _mcRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mtl => ToBaseRow(await _smtRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Mfg2 => await FirstSmtBaseByEcCodeAsync(ecCode),
            TaktEcDeptCodes.Mfg1 => ToBaseRow(await _assyRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Qc => ToBaseRow(await _qaRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            TaktEcDeptCodes.Me => ToBaseRow(await _teRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0)),
            _ => null
        };
    }

    /// <summary>
    /// 按明细 ID 与部门取执行记录（公共字段）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>公共字段快照；不存在时 null</returns>
    public async Task<TaktEcExecBaseRow?> FirstBaseByDetailAndDeptAsync(long ecDetailId, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return null;
        }
        var lookupDept = deptCode == TaktEcDeptCodes.Mfg2 ? TaktEcDeptCodes.Mfg2 : deptCode;
        var grain = await BuildGrainAsync(detail, lookupDept);
        var ecCode = detail.EcCode ?? string.Empty;
        return deptCode switch
        {
            TaktEcDeptCodes.Pc => ToBaseRow(await _pcRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Pur => ToBaseRow(await _mpRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Iqc => ToBaseRow(await _iqcRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Mc => ToBaseRow(await _mcRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Mtl => ToBaseRow(await _smtRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Mfg2 => await FirstSmtBaseByDetailIdAsync(detail, ecDetailId),
            TaktEcDeptCodes.Mfg1 => ToBaseRow(await _assyRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Qc => ToBaseRow(await _qaRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            TaktEcDeptCodes.Me => ToBaseRow(await _teRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain && x.IsDeleted == 0), ecDetailId),
            _ => null
        };
    }

    /// <summary>
    /// IN 分批上限（避免 SqlSugar 展开超大 Contains 导致 SQL Server「查询处理器用尽内部资源」）
    /// </summary>
    private const int EcDetailIdInBatchSize = 500;

    /// <summary>
    /// 按设变单号聚合全部部门执行行（公共字段；等式条件，适合十万级明细）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>执行行列表</returns>
    public async Task<List<TaktEcExecBaseRow>> ListBaseByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        var rows = new List<TaktEcExecBaseRow>();
        rows.AddRange((await _pcRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _mpRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _iqcRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _mcRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _smtRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _pcbaRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _assyRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _qaRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        rows.AddRange((await _teRepository.GetListAsync(x => x.EcCode == code && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        return rows;
    }

    /// <summary>
    /// 按多个设变单号聚合全部部门执行行。分批 IN，避免查询计划资源耗尽
    /// </summary>
    /// <param name="ecCodes">设变单号</param>
    /// <returns>执行行列表</returns>
    public async Task<List<TaktEcExecBaseRow>> ListBaseByEcCodesAsync(IReadOnlyCollection<string> ecCodes)
    {
        ArgumentNullException.ThrowIfNull(ecCodes);
        var codes = ecCodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var rows = new List<TaktEcExecBaseRow>();
        for (var offset = 0; offset < codes.Count; offset += EcDetailIdInBatchSize)
        {
            var chunk = codes.Skip(offset).Take(EcDetailIdInBatchSize).ToList();
            rows.AddRange((await _pcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _mpRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _iqcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _mcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _smtRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _pcbaRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _assyRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _qaRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
            rows.AddRange((await _teRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0)).Select(x => ToBaseRow(x)).Where(x => x != null)!);
        }
        return rows;
    }

    /// <summary>
    /// 按多个设变单号读取投入批次要展示的执行行。每个部门保留自己的全部未作废行
    /// </summary>
    /// <param name="ecCodes">设变单号</param>
    /// <returns>阶段行</returns>
    public async Task<List<TaktEcBatchStageLine>> ListBatchStageLinesByEcCodesAsync(IReadOnlyCollection<string> ecCodes)
    {
        ArgumentNullException.ThrowIfNull(ecCodes);
        var codes = ecCodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var rows = new List<TaktEcBatchStageLine>();
        for (var offset = 0; offset < codes.Count; offset += EcDetailIdInBatchSize)
        {
            var chunk = codes.Skip(offset).Take(EcDetailIdInBatchSize).ToList();
            rows.AddRange((await _mpRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.PurchaseOrder,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.PurchaseOrderIssueDate,
                    BatchCode = x.PurchaseOrderCode,
                }));
            rows.AddRange((await _pcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.Scheduled,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.ScheduledDate,
                    BatchCode = x.ScheduledBatch,
                }));
            rows.AddRange((await _iqcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.IncomingInspection,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.InspectionDate,
                    BatchCode = x.IqcOrderCode,
                }));
            rows.AddRange((await _mcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.Outbound,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.OutboundDate,
                    BatchCode = x.OutboundBatch,
                }));
            rows.AddRange((await _smtRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.ElectronicOutbound,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.OutboundDate,
                    BatchCode = x.OutboundBatch,
                }));
            rows.AddRange((await _pcbaRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.Mfg2Production,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.ProductionDate,
                    BatchCode = x.ImplementationBatch,
                }));
            rows.AddRange((await _assyRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.Mfg1Production,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.ProductionDate,
                    BatchCode = x.ImplementationBatch,
                }));
            rows.AddRange((await _qaRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.SampleInspection,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.InspectionDate,
                }));
            rows.AddRange((await _teRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0))
                .Select(x => new TaktEcBatchStageLine
                {
                    EcCode = x.EcCode,
                    StageCode = TaktEcBatchStageCodes.TechSop,
                    EcGrainKey = x.EcGrainKey ?? string.Empty,
                    StageDate = x.SopDate,
                }));
        }
        return rows;
    }

    /// <summary>
    /// 设变单号下是否存在任一部门已输入（实施=是，或执行内容为人工填写；系统按实施范围写入的文案不算输入）。仅查首行，不拉全表
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>存在已输入行时 true</returns>
    public async Task<bool> ExistsAnyDeptInputByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        if (await _pcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _mpRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _iqcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _mcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _smtRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _pcbaRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _assyRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        if (await _qaRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.IsImplemented == 1 || (x.ExecContent != null && x.ExecContent != ""
                    && x.ExecContent != TaktEcScopeConstants.AutoCompletedExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByFinishedProductExecContent
                    && x.ExecContent != TaktEcScopeConstants.ByModelExecContent
                    && x.ExecContent != "实施范围-全仕向"
                    && x.ExecContent != TaktEcScopeConstants.MaterialControlExecContent
                    && x.ExecContent != TaktEcScopeConstants.InternalExecContent
                    && x.ExecContent != TaktEcScopeConstants.TechnicalExecContent
                    && x.ExecContent != TaktEcPurConstants.NotPurchasingRelatedExecContent
                    && x.ExecContent != TaktEcIqcConstants.NotRelatedToIqcExecContent
                    && x.ExecContent != TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    && !x.ExecContent.EndsWith("-EOL")))) != null)
        {
            return true;
        }
        return await _teRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && (x.SopDate != null
                    || x.IsSopUpdated == 1
                    || (x.TechLeader != null && x.TechLeader != "")
                    || (x.SopLeader != null && x.SopLeader != ""))) != null;
    }

    /// <summary>
    /// 设变单号下是否仍有未完成的执行部门行（无人工输入）。没有未完成行且已有输入时主表为完成
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>存在未完成行时 true</returns>
    public async Task<bool> ExistsAnyUnfilledDeptByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        if (await _pcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _mpRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _iqcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _mcRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _smtRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _pcbaRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _assyRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        if (await _qaRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0 && x.IsImplemented != 1
                && (x.ExecContent == null || x.ExecContent == ""
                    || x.ExecContent == TaktEcScopeConstants.AutoCompletedExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByFinishedProductExecContent
                    || x.ExecContent == TaktEcScopeConstants.ByModelExecContent
                    || x.ExecContent == "实施范围-全仕向"
                    || x.ExecContent == TaktEcScopeConstants.MaterialControlExecContent
                    || x.ExecContent == TaktEcScopeConstants.InternalExecContent
                    || x.ExecContent == TaktEcScopeConstants.TechnicalExecContent
                    || x.ExecContent == TaktEcPurConstants.NotPurchasingRelatedExecContent
                    || x.ExecContent == TaktEcIqcConstants.NotRelatedToIqcExecContent
                    || x.ExecContent == TaktEcMcConstants.NotRelatedToMaterialControlExecContent
                    || x.ExecContent.EndsWith("-EOL"))) != null)
        {
            return true;
        }
        return await _teRepository.FirstAsync(x =>
                x.EcCode == code && x.IsDeleted == 0 && x.IsObsolete == 0
                && x.SopDate == null
                && x.IsSopUpdated != 1
                && (x.TechLeader == null || x.TechLeader == "")
                && (x.SopLeader == null || x.SopLeader == "")) != null;
    }

    /// <summary>
    /// 按明细 ID 列表聚合全部部门执行行（公共字段；超大批次分片 IN，避免查询计划资源耗尽）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>执行行列表</returns>
    public async Task<List<TaktEcExecBaseRow>> ListBaseByEcDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return [];
        }
        if (detailIds.Count <= EcDetailIdInBatchSize)
        {
            return await ListBaseByEcDetailIdsChunkAsync(detailIds);
        }
        var rows = new List<TaktEcExecBaseRow>();
        for (var offset = 0; offset < detailIds.Count; offset = checked(offset + EcDetailIdInBatchSize))
        {
            var take = Math.Min(EcDetailIdInBatchSize, detailIds.Count - offset);
            var chunk = detailIds.Skip(offset).Take(take).ToList();
            rows.AddRange(await ListBaseByEcDetailIdsChunkAsync(chunk));
        }
        return rows;
    }

    /// <summary>
    /// 单批明细 ID 聚合部门执行行
    /// </summary>
    /// <param name="detailIds">单批明细 ID（建议 ≤ EcDetailIdInBatchSize）</param>
    /// <returns>执行行列表</returns>
    private async Task<List<TaktEcExecBaseRow>> ListBaseByEcDetailIdsChunkAsync(IReadOnlyList<long> detailIds)
    {
        var details = await _ecDetailRepository.GetListAsync(x => detailIds.Contains(x.Id) && x.IsDeleted == 0);
        var rows = new List<TaktEcExecBaseRow>();
        foreach (var group in details.GroupBy(x => x.EcCode ?? string.Empty))
        {
            if (string.IsNullOrWhiteSpace(group.Key))
            {
                continue;
            }
            var execRows = await ListBaseByEcCodeAsync(group.Key);
            var master = await MasterScopeAsync(group.First());
            foreach (var detail in group)
            {
                foreach (var exec in execRows)
                {
                    var grain = TaktEcScopeRowGrain.BuildExecGrainKey(detail, exec.DeptCode, master);
                    if (!string.Equals(exec.EcGrainKey, grain, StringComparison.Ordinal))
                    {
                        continue;
                    }
                    rows.Add(new TaktEcExecBaseRow
                    {
                        Id = exec.Id,
                        EcDetailId = detail.Id,
                        EcGrainKey = exec.EcGrainKey,
                        EcCode = exec.EcCode,
                        DeptCode = exec.DeptCode,
                        LineNumber = exec.LineNumber,
                        IsImplemented = exec.IsImplemented,
                        ExecContent = exec.ExecContent,
                        IsObsolete = exec.IsObsolete,
                        TenantCode = exec.TenantCode,
                        CompanyCode = exec.CompanyCode,
                    });
                }
            }
        }
        return rows;
    }

    /// <summary>
    /// 按明细 ID 聚合全部部门执行行（公共字段）
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <returns>执行行列表</returns>
    public Task<List<TaktEcExecBaseRow>> ListBaseByEcDetailIdAsync(long ecDetailId) =>
        ListBaseByEcDetailIdsAsync([ecDetailId]);

    /// <summary>
    /// 取明细下全部部门执行行的最大行号
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <returns>最大行号；无记录时为 0</returns>
    public async Task<int> GetMaxLineNumberForDetailAsync(long ecDetailId)
    {
        var rows = await ListBaseByEcDetailIdAsync(ecDetailId);
        return rows.Count == 0 ? 0 : rows.Max(x => x.LineNumber);
    }

    /// <summary>
    /// 按明细 ID 删除全部部门执行行
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAllByEcDetailIdAsync(long ecDetailId)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || string.IsNullOrWhiteSpace(detail.EcCode))
        {
            return;
        }
        var master = await MasterScopeAsync(detail);
        var siblings = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode && x.Id != detail.Id && x.IsObsolete == 0 && x.IsDeleted == 0);
        await DeleteSoleGrainAsync(_pcRepository, detail, siblings, TaktEcDeptCodes.Pc, master);
        await DeleteSoleGrainAsync(_mpRepository, detail, siblings, TaktEcDeptCodes.Pur, master);
        await DeleteSoleGrainAsync(_iqcRepository, detail, siblings, TaktEcDeptCodes.Iqc, master);
        await DeleteSoleGrainAsync(_mcRepository, detail, siblings, TaktEcDeptCodes.Mc, master);
        await DeleteSoleGrainAsync(_smtRepository, detail, siblings, TaktEcDeptCodes.Mtl, master);
        await DeleteSoleGrainAsync(_pcbaRepository, detail, siblings, TaktEcDeptCodes.Mfg2, master);
        await DeleteSoleGrainAsync(_assyRepository, detail, siblings, TaktEcDeptCodes.Mfg1, master);
        await DeleteSoleGrainAsync(_qaRepository, detail, siblings, TaktEcDeptCodes.Qc, master);
        await DeleteSoleGrainAsync(_teRepository, detail, siblings, TaktEcDeptCodes.Me, master);
    }

    /// <summary>
    /// 按 ID 与部门编码取最大行号（单表）
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <returns>最大行号</returns>
    public async Task<int> GetMaxLineNumberForDetailDeptAsync(
        long ecDetailId,
        string deptCode,
        string tenantCode,
        string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return 0;
        }
        var grainDept = deptCode == TaktEcDeptCodes.Mfg2 ? TaktEcDeptCodes.Mfg2 : deptCode;
        var grain = await BuildGrainAsync(detail, grainDept);
        var ecCode = detail.EcCode ?? string.Empty;
        return deptCode switch
        {
            TaktEcDeptCodes.Pc => await _pcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Pur => await _mpRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Iqc => await _iqcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Mc => await _mcRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Mtl => await _smtRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Mfg2 => await GetMaxLineNumberForPcbaAsync(detail, tenantCode, companyCode),
            TaktEcDeptCodes.Mfg1 => await _assyRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Qc => await _qaRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            TaktEcDeptCodes.Me => await _teRepository.GetMaxIntAsync(
                x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain,
                x => x.LineNumber),
            _ => 0
        };
    }

    /// <summary>
    /// 按设变单号取制造二课公共字段（优先未作废电子料件，再制二非 F）
    /// </summary>
    private async Task<TaktEcExecBaseRow?> FirstSmtBaseByEcCodeAsync(string ecCode)
    {
        var electronic = await _smtRepository.FirstAsync(x =>
            x.EcCode == ecCode && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (electronic != null)
        {
            return ToBaseRow(electronic);
        }
        var pcba = await _pcbaRepository.FirstAsync(x =>
            x.EcCode == ecCode && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (pcba != null)
        {
            return ToBaseRow(pcba);
        }
        return ToBaseRow(await _smtRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0))
            ?? ToBaseRow(await _pcbaRepository.FirstAsync(x => x.EcCode == ecCode && x.IsDeleted == 0));
    }

    /// <summary>
    /// 按明细 ID 取制造二课公共字段（优先未作废电子料件，再制二非 F）
    /// </summary>
    private async Task<TaktEcExecBaseRow?> FirstSmtBaseByDetailIdAsync(TaktEcDetail detail, long ecDetailId)
    {
        var ecCode = detail.EcCode ?? string.Empty;
        var smtGrain = await BuildGrainAsync(detail, TaktEcDeptCodes.Mtl);
        var electronic = await _smtRepository.FirstAsync(x =>
            x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == smtGrain && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (electronic != null)
        {
            return ToBaseRow(electronic, ecDetailId);
        }
        var nikaGrain = await BuildGrainAsync(detail, TaktEcDeptCodes.Mfg2);
        var pcba = await _pcbaRepository.FirstAsync(x =>
            x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == nikaGrain && x.IsDeleted == 0 && x.IsObsolete == 0);
        if (pcba != null)
        {
            return ToBaseRow(pcba, ecDetailId);
        }
        return ToBaseRow(await _smtRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == smtGrain && x.IsDeleted == 0), ecDetailId)
            ?? ToBaseRow(await _pcbaRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == nikaGrain && x.IsDeleted == 0), ecDetailId);
    }

    /// <summary>
    /// 制造二课双表最大行号
    /// </summary>
    private async Task<int> GetMaxLineNumberForPcbaAsync(TaktEcDetail detail, string tenantCode, string companyCode)
    {
        var ecCode = detail.EcCode ?? string.Empty;
        var smtGrain = await BuildGrainAsync(detail, TaktEcDeptCodes.Mtl);
        var nikaGrain = await BuildGrainAsync(detail, TaktEcDeptCodes.Mfg2);
        var electronicMax = await _smtRepository.GetMaxIntAsync(
            x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == smtGrain,
            x => x.LineNumber);
        var pcbaMax = await _pcbaRepository.GetMaxIntAsync(
            x => x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == nikaGrain,
            x => x.LineNumber);
        return Math.Max(electronicMax, pcbaMax);
    }
    /// <summary>
    /// 按明细关联生管执行行，读取旧品处理。
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>明细 ID → 生管 EcOldPartDisposition</returns>
    public async Task<Dictionary<long, string?>> ListPcOldPartDispositionByDetailsAsync(
        IReadOnlyList<TaktEcDetail> details)
    {
        ArgumentNullException.ThrowIfNull(details);
        var result = new Dictionary<long, string?>();
        if (details.Count == 0)
        {
            return result;
        }
        var mastersById = await LoadMastersByIdAsync(details);
        var mastersByCode = await LoadMastersByCodeAsync(details, mastersById);
        var pcByGrain = await LoadPcDispositionByGrainAsync(details);
        foreach (var detail in details)
        {
            var scope = NormalizeControlScope(ResolveMaster(detail, mastersById, mastersByCode)?.EcControlScope);
            var ecCode = detail.EcCode ?? string.Empty;
            var pcGrain = TaktEcScopeRowGrain.BuildExecGrainKey(detail, TaktEcDeptCodes.Pc, scope);
            pcByGrain.TryGetValue(DispositionKey(ecCode, pcGrain), out var pc);
            result[detail.Id] = pc;
        }
        return result;
    }
    /// <summary>
    /// 按技术课主表解析控制范围，供去重键计算。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>控制范围</returns>
    private async Task<int> MasterScopeAsync(TaktEcDetail detail)
    {
        TaktEcTe? te = null;
        if (detail.EcTeId > 0)
        {
            te = await _ecTeRepository.GetByIdAsync(detail.EcTeId);
        }
        if (te == null && !string.IsNullOrWhiteSpace(detail.EcCode))
        {
            te = await _ecTeRepository.FirstAsync(x => x.EcCode == detail.EcCode);
        }
        return NormalizeControlScope(te?.EcControlScope);
    }
    /// <summary>
    /// 控制范围非法时回落到按机种。
    /// </summary>
    /// <param name="raw">技术课主表控制范围</param>
    /// <returns>可用于去重键的控制范围</returns>
    private static int NormalizeControlScope(int? raw)
    {
        var value = raw ?? TaktEcControlScopeConstants.ByModel;
        return value is TaktEcControlScopeConstants.ByFinishedProduct
            or TaktEcControlScopeConstants.ByModel
            or TaktEcControlScopeConstants.ByEngineeringChange
            or TaktEcControlScopeConstants.NotManaged
            ? value
            : TaktEcControlScopeConstants.ByModel;
    }
    /// <summary>
    /// 按明细上的技术课主键批量加载主表。
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>主表 ID → 技术课</returns>
    private async Task<Dictionary<long, TaktEcTe>> LoadMastersByIdAsync(IReadOnlyList<TaktEcDetail> details)
    {
        var masters = new Dictionary<long, TaktEcTe>();
        var ids = details.Where(x => x.EcTeId > 0).Select(x => x.EcTeId).Distinct().ToList();
        for (var offset = 0; offset < ids.Count; offset += EcDetailIdInBatchSize)
        {
            var chunk = ids.Skip(offset).Take(EcDetailIdInBatchSize).ToList();
            var rows = await _ecTeRepository.GetListAsync(x => chunk.Contains(x.Id) && x.IsDeleted == 0);
            foreach (var row in rows)
            {
                masters[row.Id] = row;
            }
        }
        return masters;
    }
    /// <summary>
    /// 主键未命中时，按设变单号补技术课主表。
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <param name="mastersById">已按主键命中的主表</param>
    /// <returns>设变单号 → 技术课</returns>
    private async Task<Dictionary<string, TaktEcTe>> LoadMastersByCodeAsync(
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyDictionary<long, TaktEcTe> mastersById)
    {
        var masters = new Dictionary<string, TaktEcTe>(StringComparer.OrdinalIgnoreCase);
        var codes = details
            .Where(x => x.EcTeId <= 0 || !mastersById.ContainsKey(x.EcTeId))
            .Select(x => x.EcCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        for (var offset = 0; offset < codes.Count; offset += EcDetailIdInBatchSize)
        {
            var chunk = codes.Skip(offset).Take(EcDetailIdInBatchSize).ToList();
            var rows = await _ecTeRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0);
            foreach (var row in rows.OrderBy(x => x.Id))
            {
                if (!string.IsNullOrWhiteSpace(row.EcCode))
                {
                    masters.TryAdd(row.EcCode, row);
                }
            }
        }
        return masters;
    }
    /// <summary>
    /// 按设变单号加去重键索引未作废生管行的旧品处理。
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>单号与去重键 → EcOldPartDisposition</returns>
    private async Task<Dictionary<string, string?>> LoadPcDispositionByGrainAsync(IReadOnlyList<TaktEcDetail> details)
    {
        var map = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        var codes = details
            .Select(x => x.EcCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        for (var offset = 0; offset < codes.Count; offset += EcDetailIdInBatchSize)
        {
            var chunk = codes.Skip(offset).Take(EcDetailIdInBatchSize).ToList();
            var rows = await _pcRepository.GetListAsync(x => chunk.Contains(x.EcCode) && x.IsDeleted == 0 && x.IsObsolete == 0);
            foreach (var row in rows.OrderBy(x => x.Id))
            {
                map.TryAdd(DispositionKey(row.EcCode, row.EcGrainKey), row.EcOldPartDisposition);
            }
        }
        return map;
    }
    /// <summary>
    /// 明细优先用技术课主键，否则用设变单号匹配主表。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="mastersById">主键索引</param>
    /// <param name="mastersByCode">单号索引</param>
    /// <returns>技术课主表；不存在时 null</returns>
    private static TaktEcTe? ResolveMaster(
        TaktEcDetail detail,
        IReadOnlyDictionary<long, TaktEcTe> mastersById,
        IReadOnlyDictionary<string, TaktEcTe> mastersByCode)
    {
        if (detail.EcTeId > 0 && mastersById.TryGetValue(detail.EcTeId, out var byId))
        {
            return byId;
        }
        if (!string.IsNullOrWhiteSpace(detail.EcCode) && mastersByCode.TryGetValue(detail.EcCode, out var byCode))
        {
            return byCode;
        }
        return null;
    }
    /// <summary>
    /// 设变单号与去重键组成忽略大小写的查找键。
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="grainKey">执行去重键</param>
    /// <returns>索引键</returns>
    private static string DispositionKey(string? ecCode, string? grainKey)
    {
        return $"{ecCode?.Trim()}\u001f{grainKey?.Trim()}";
    }
    /// <summary>
    /// 计算明细在指定部门的执行去重键。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>去重键</returns>
    private async Task<string> BuildGrainAsync(TaktEcDetail detail, string deptCode)
    {
        var master = await MasterScopeAsync(detail);
        return TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, master);
    }
    /// <summary>
    /// 同一去重键没有其它未作废明细时，删除该执行行。
    /// </summary>
    /// <typeparam name="TEntity">部门执行实体</typeparam>
    /// <param name="repository">部门执行仓储</param>
    /// <param name="detail">当前明细</param>
    /// <param name="siblings">同设变单号的其它明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="master">技术课主表控制范围</param>
    /// <returns>任务</returns>
    private static async Task DeleteSoleGrainAsync<TEntity>(
        ITaktCompanyRepository<TEntity> repository,
        TaktEcDetail detail,
        IReadOnlyList<TaktEcDetail> siblings,
        string deptCode,
        int master)
        where TEntity : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var grain = TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, master);
        if (siblings.Any(x => TaktEcScopeRowGrain.BuildExecGrainKey(x, deptCode, master) == grain))
        {
            return;
        }
        var ecCode = detail.EcCode ?? string.Empty;
        await repository.DeleteAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grain);
    }

    /// <summary>
    /// 映射公共字段
    /// </summary>
    /// <param name="entity">执行实体</param>
    /// <param name="detailId">按明细展开时回填的明细 ID；执行表不保存该外键</param>
    /// <returns>公共字段快照</returns>
    private static TaktEcExecBaseRow? ToBaseRow(object? entity, long detailId = 0) => entity switch
    {
        TaktEcPc e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcPur e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcIqc e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcMc e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcMtl e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcMfg2 e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcMfg1 e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcQc e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsImplemented,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        TaktEcMe e => new TaktEcExecBaseRow
        {
            Id = e.Id,
            EcDetailId = detailId,
            EcGrainKey = e.EcGrainKey,
            EcCode = e.EcCode,
            DeptCode = e.DeptCode,
            LineNumber = e.LineNumber,
            IsImplemented = e.IsSopUpdated,
            ExecContent = e.ExecContent,
            IsObsolete = e.IsObsolete,
            TenantCode = e.TenantCode,
            CompanyCode = e.CompanyCode,
        },
        _ => null
    };
}
