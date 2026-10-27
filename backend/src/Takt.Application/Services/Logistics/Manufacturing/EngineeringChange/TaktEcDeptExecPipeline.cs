// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptExecPipeline.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：各部门执行行查询→去重→保存单链路（按 DeptCode 分派）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 单一执行部门的查询 / 去重 / 保存
/// </summary>
internal interface ITaktEcDeptExecPipeline
{
    /// <summary>
    /// 部门编码
    /// </summary>
    string DeptCode { get; }
    /// <summary>
    /// 按设变单号与执行去重键查询
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="grainKey">执行去重键</param>
    /// <returns>执行行；没有时 null</returns>
    Task<object?> QueryByGrainAsync(string ecCode, string grainKey);
    /// <summary>
    /// 按设变单号查询
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>执行行</returns>
    Task<List<object>> QueryByEcCodeAsync(string ecCode);
    /// <summary>
    /// 新建或复用后同步明细冗余字段。控制范围只在新建时写入。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="existing">已有执行行；空则新建</param>
    /// <param name="lineNumber">新建行号</param>
    /// <param name="applyNotRelatedAuto">是否写入无关自动完成</param>
    /// <param name="masterControlScope">技术课主表控制范围</param>
    /// <returns>绑定后的执行实体</returns>
    object Bind(TaktEcDetail detail, object? existing, int lineNumber, bool applyNotRelatedAuto, int masterControlScope);
    /// <summary>
    /// 保存
    /// </summary>
    Task<object> SaveAsync(object exec, bool isNew);
    /// <summary>
    /// 按设变单号删除
    /// </summary>
    Task DeleteByEcCodeAsync(string ecCode);
}

/// <summary>
/// 部门执行行查询→去重→保存（每课一条泛型链路）
/// </summary>
/// <typeparam name="TEntity">部门执行实体</typeparam>
internal sealed class TaktEcDeptExecPipeline<TEntity> : ITaktEcDeptExecPipeline
    where TEntity : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
{
    private readonly ITaktCompanyRepository<TEntity> _repository;
    private readonly Action<TEntity, TaktEcDetail, bool>? _afterBind;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">部门执行仓储</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="afterBind">绑定后部门专有处理（采购/受检/部管无关文案）</param>
    public TaktEcDeptExecPipeline(
        ITaktCompanyRepository<TEntity> repository,
        string deptCode,
        Action<TEntity, TaktEcDetail, bool>? afterBind = null)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        _repository = repository;
        DeptCode = deptCode;
        _afterBind = afterBind;
    }

    /// <inheritdoc />
    public string DeptCode { get; }

    /// <inheritdoc />
    public async Task<object?> QueryByGrainAsync(string ecCode, string grainKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(grainKey);
        return await _repository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey);
    }

    /// <inheritdoc />
    public async Task<List<object>> QueryByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var list = await _repository.GetListAsync(x => x.EcCode == ecCode);
        return list.Cast<object>().ToList();
    }

    /// <inheritdoc />
    public object Bind(TaktEcDetail detail, object? existing, int lineNumber, bool applyNotRelatedAuto, int masterControlScope)
    {
        ArgumentNullException.ThrowIfNull(detail);
        var isNew = existing == null;
        var exec = existing as TEntity ?? new TEntity
        {
            DeptCode = DeptCode
        };
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, isNew ? lineNumber : null, masterControlScope);
        _afterBind?.Invoke(exec, detail, applyNotRelatedAuto);
        return exec;
    }

    /// <inheritdoc />
    public async Task<object> SaveAsync(object exec, bool isNew)
    {
        var entity = exec as TEntity ?? throw new ArgumentException("执行实体类型与部门链路不匹配", nameof(exec));
        if (isNew)
        {
            return await _repository.CreateAsync(entity);
        }
        await _repository.UpdateAsync(entity);
        return entity;
    }

    /// <inheritdoc />
    public Task DeleteByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        return _repository.DeleteAsync(x => x.EcCode == code);
    }
}

/// <summary>
/// 按部门编码分派查询→去重→保存链路
/// </summary>
internal sealed class TaktEcDeptExecPipelineHub
{
    private readonly IReadOnlyDictionary<string, ITaktEcDeptExecPipeline> _pipelines;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcDeptExecPipelineHub(
        ITaktCompanyRepository<TaktEcPc> pcRepository,
        ITaktCompanyRepository<TaktEcPur> mpRepository,
        ITaktCompanyRepository<TaktEcIqc> iqcRepository,
        ITaktCompanyRepository<TaktEcMc> mcRepository,
        ITaktCompanyRepository<TaktEcMfg2> pcbaRepository,
        ITaktCompanyRepository<TaktEcMtl> smtRepository,
        ITaktCompanyRepository<TaktEcMfg1> assyRepository,
        ITaktCompanyRepository<TaktEcQc> qaRepository,
        ITaktCompanyRepository<TaktEcMe> teRepository)
    {
        // Pcba Hub 仍挂 Pcba；PCBA 表运行时 Upsert 由 Persistence 双表路由，Hub 保留 pcba 注入供后续扩展
        _ = smtRepository;
        _pipelines = new Dictionary<string, ITaktEcDeptExecPipeline>(StringComparer.Ordinal)
        {
            [TaktEcDeptCodes.Pc] = new TaktEcDeptExecPipeline<TaktEcPc>(pcRepository, TaktEcDeptCodes.Pc),
            [TaktEcDeptCodes.Pur] = new TaktEcDeptExecPipeline<TaktEcPur>(
                mpRepository,
                TaktEcDeptCodes.Pur,
                (e, d, auto) => { if (auto) { TaktEcExecNotRelated.TryMp(e, d); } }),
            [TaktEcDeptCodes.Iqc] = new TaktEcDeptExecPipeline<TaktEcIqc>(
                iqcRepository,
                TaktEcDeptCodes.Iqc,
                (e, d, auto) => { if (auto) { TaktEcExecNotRelated.TryIqc(e, d); } }),
            [TaktEcDeptCodes.Mc] = new TaktEcDeptExecPipeline<TaktEcMc>(
                mcRepository,
                TaktEcDeptCodes.Mc,
                (e, d, auto) => { if (auto) { TaktEcExecNotRelated.TryMc(e, d); } }),
            [TaktEcDeptCodes.Mfg2] = new TaktEcDeptExecPipeline<TaktEcMfg2>(pcbaRepository, TaktEcDeptCodes.Mfg2),
            [TaktEcDeptCodes.Mfg1] = new TaktEcDeptExecPipeline<TaktEcMfg1>(assyRepository, TaktEcDeptCodes.Mfg1),
            [TaktEcDeptCodes.Qc] = new TaktEcDeptExecPipeline<TaktEcQc>(qaRepository, TaktEcDeptCodes.Qc),
            [TaktEcDeptCodes.Me] = new TaktEcDeptExecPipeline<TaktEcMe>(teRepository, TaktEcDeptCodes.Me)
        };
    }

    /// <summary>
    /// 全部部门链路（看板顺序）
    /// </summary>
    public IEnumerable<ITaktEcDeptExecPipeline> All =>
        TaktEcDeptCodes.KanbanOrder.Where(code => code != TaktEcDeptCodes.Mtl).Select(Get);

    /// <summary>
    /// 取指定部门链路
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>该课查询/去重/保存链路</returns>
    public ITaktEcDeptExecPipeline Get(string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        if (!_pipelines.TryGetValue(deptCode.Trim(), out var pipeline))
        {
            throw new InvalidOperationException($"不支持的部门编码：{deptCode}");
        }
        return pipeline;
    }
}

/// <summary>
/// 全仕向/部管下采购、受检、部管「无关」自动完成
/// </summary>
internal static class TaktEcExecNotRelated
{
    /// <summary>
    /// 采购类型非 F：采购无关
    /// </summary>
    public static void TryMp(TaktEcPur mp, TaktEcDetail detail)
    {
        if (TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return;
        }
        mp.ExecContent = TaktEcPurConstants.NotPurchasingRelatedExecContent;
        mp.PurchaseOrderIssueDate = null;
        mp.Supplier = null;
        mp.PurchaseOrderCode = null;
        mp.IsImplemented = 1;
    }

    /// <summary>
    /// 无需检验：跟 IQC 无关
    /// </summary>
    public static void TryIqc(TaktEcIqc iqc, TaktEcDetail detail)
    {
        if (detail.EcNewRequiresInspection == 1)
        {
            return;
        }
        iqc.ExecContent = TaktEcIqcConstants.NotRelatedToIqcExecContent;
        iqc.IqcOrderCode = null;
        iqc.InspectionDate = null;
        iqc.IsImplemented = 1;
    }

    /// <summary>
    /// 部管不可见：跟部管无关
    /// </summary>
    public static void TryMc(TaktEcMc mc, TaktEcDetail detail)
    {
        if (TaktEcScopeConstants.IsMcVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        mc.ExecContent = TaktEcMcConstants.NotRelatedToMaterialControlExecContent;
        mc.OutboundBatch = null;
        mc.OutboundDate = null;
        mc.IsImplemented = 1;
    }

    /// <summary>
    /// 全仕向/部管填写后：按实体类型写入采购/受检/部管「无关」
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    public static void TryAfterFill(object exec, TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(exec);
        ArgumentNullException.ThrowIfNull(detail);
        switch (exec)
        {
            case TaktEcPur pur:
                TryMp(pur, detail);
                break;
            case TaktEcIqc iqc:
                TryIqc(iqc, detail);
                break;
            case TaktEcMc mc:
                TryMc(mc, detail);
                break;
        }
    }
}
