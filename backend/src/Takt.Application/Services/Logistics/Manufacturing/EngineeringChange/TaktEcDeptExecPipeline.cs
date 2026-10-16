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
    /// 按明细 ID 查询（去重键：EcDetailId 唯一）
    /// </summary>
    Task<object?> QueryByDetailIdAsync(long ecDetailId);
    /// <summary>
    /// 按明细 ID 列表查询
    /// </summary>
    Task<List<object>> QueryByDetailIdsAsync(IReadOnlyList<long> detailIds);
    /// <summary>
    /// 新建或复用后同步明细冗余字段
    /// </summary>
    object Bind(TaktEcDetail detail, object? existing, int lineNumber, bool applyNotRelatedAuto);
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
    public async Task<object?> QueryByDetailIdAsync(long ecDetailId)
    {
        return await _repository.FirstAsync(x => x.EcDetailId == ecDetailId);
    }

    /// <inheritdoc />
    public async Task<List<object>> QueryByDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return [];
        }
        var list = await _repository.GetListAsync(x => detailIds.Contains(x.EcDetailId));
        return list.Cast<object>().ToList();
    }

    /// <inheritdoc />
    public object Bind(TaktEcDetail detail, object? existing, int lineNumber, bool applyNotRelatedAuto)
    {
        ArgumentNullException.ThrowIfNull(detail);
        var isNew = existing == null;
        var exec = existing as TEntity ?? new TEntity
        {
            EcDetailId = detail.Id,
            DeptCode = DeptCode
        };
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, isNew ? lineNumber : null);
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
        ITaktCompanyRepository<TaktEcSeikan> pmcRepository,
        ITaktCompanyRepository<TaktEcKoubai> mpRepository,
        ITaktCompanyRepository<TaktEcUkeken> iqcRepository,
        ITaktCompanyRepository<TaktEcBukan> mcRepository,
        ITaktCompanyRepository<TaktEcSeizounika> seizounikaRepository,
        ITaktCompanyRepository<TaktEcEmtl> smtRepository,
        ITaktCompanyRepository<TaktEcSeizouikka> assyRepository,
        ITaktCompanyRepository<TaktEcHinkan> qaRepository,
        ITaktCompanyRepository<TaktEcSeizougijutsu> teRepository)
    {
        // Pcba Hub 仍挂 Seizounika；PCBA 表运行时 Upsert 由 Persistence 双表路由，Hub 保留 pcba 注入供后续扩展
        _ = smtRepository;
        _pipelines = new Dictionary<string, ITaktEcDeptExecPipeline>(StringComparer.Ordinal)
        {
            [TaktEcDeptCodes.Pmc] = new TaktEcDeptExecPipeline<TaktEcSeikan>(pmcRepository, TaktEcDeptCodes.Pmc),
            [TaktEcDeptCodes.Mp] = new TaktEcDeptExecPipeline<TaktEcKoubai>(
                mpRepository,
                TaktEcDeptCodes.Mp,
                (e, d, auto) => { if (auto) { TaktEcExecNotRelated.TryKoubai(e, d); } }),
            [TaktEcDeptCodes.Iqc] = new TaktEcDeptExecPipeline<TaktEcUkeken>(
                iqcRepository,
                TaktEcDeptCodes.Iqc,
                (e, d, auto) => { if (auto) { TaktEcExecNotRelated.TryUkeken(e, d); } }),
            [TaktEcDeptCodes.Mc] = new TaktEcDeptExecPipeline<TaktEcBukan>(
                mcRepository,
                TaktEcDeptCodes.Mc,
                (e, d, auto) => { if (auto) { TaktEcExecNotRelated.TryBukan(e, d); } }),
            [TaktEcDeptCodes.Pcba] = new TaktEcDeptExecPipeline<TaktEcSeizounika>(seizounikaRepository, TaktEcDeptCodes.Pcba),
            [TaktEcDeptCodes.Assy] = new TaktEcDeptExecPipeline<TaktEcSeizouikka>(assyRepository, TaktEcDeptCodes.Assy),
            [TaktEcDeptCodes.Qa] = new TaktEcDeptExecPipeline<TaktEcHinkan>(qaRepository, TaktEcDeptCodes.Qa),
            [TaktEcDeptCodes.Te] = new TaktEcDeptExecPipeline<TaktEcSeizougijutsu>(teRepository, TaktEcDeptCodes.Te)
        };
    }

    /// <summary>
    /// 全部部门链路（看板顺序）
    /// </summary>
    public IEnumerable<ITaktEcDeptExecPipeline> All =>
        TaktEcDeptCodes.KanbanOrder.Where(code => code != TaktEcDeptCodes.Smt).Select(Get);

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
    public static void TryKoubai(TaktEcKoubai koubai, TaktEcDetail detail)
    {
        if (TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return;
        }
        koubai.ExecContent = TaktEcKoubaiConstants.NotPurchasingRelatedExecContent;
        koubai.PurchaseOrderIssueDate = null;
        koubai.Supplier = null;
        koubai.PurchaseOrderCode = null;
        koubai.IsImplemented = 1;
    }

    /// <summary>
    /// 无需检验：跟 IQC 无关
    /// </summary>
    public static void TryUkeken(TaktEcUkeken ukeken, TaktEcDetail detail)
    {
        if (detail.EcNewRequiresInspection == 1)
        {
            return;
        }
        ukeken.ExecContent = TaktEcUkekenConstants.NotRelatedToIqcExecContent;
        ukeken.IqcOrderCode = null;
        ukeken.InspectionDate = null;
        ukeken.IsImplemented = 1;
    }

    /// <summary>
    /// 部管不可见：跟部管无关
    /// </summary>
    public static void TryBukan(TaktEcBukan bukan, TaktEcDetail detail)
    {
        if (TaktEcScopeConstants.IsBukanVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        bukan.ExecContent = TaktEcBukanConstants.NotRelatedToMaterialControlExecContent;
        bukan.OutboundBatch = null;
        bukan.OutboundDate = null;
        bukan.IsImplemented = 1;
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
            case TaktEcKoubai koubai:
                TryKoubai(koubai, detail);
                break;
            case TaktEcUkeken ukeken:
                TryUkeken(ukeken, detail);
                break;
            case TaktEcBukan bukan:
                TryBukan(bukan, detail);
                break;
        }
    }
}
