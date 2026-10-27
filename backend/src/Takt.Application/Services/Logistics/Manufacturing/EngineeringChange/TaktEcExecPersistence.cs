// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecPersistence.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行聚合持久化（8 张 TaktEcExec* 部门表读写，由 TaktEcDetail 关联）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行聚合持久化
/// </summary>
public partial class TaktEcExecPersistence
{
    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcPc> _pcRepository;
    private readonly ITaktCompanyRepository<TaktEcPur> _mpRepository;
    private readonly ITaktCompanyRepository<TaktEcIqc> _iqcRepository;
    private readonly ITaktCompanyRepository<TaktEcMc> _mcRepository;
    private readonly ITaktCompanyRepository<TaktEcMfg2> _pcbaRepository;
    private readonly ITaktCompanyRepository<TaktEcMtl> _smtRepository;
    private readonly ITaktCompanyRepository<TaktEcMfg1> _assyRepository;
    private readonly ITaktCompanyRepository<TaktEcQc> _qaRepository;
    private readonly ITaktCompanyRepository<TaktEcMe> _teRepository;
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;

    private readonly TaktEcExecDeptAccess _deptAccess;
    private readonly TaktEcTeStatusSynchronizer _ecTeStatusSynchronizer;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcExecPersistence(
        TaktEcExecDeptAccess deptAccess,
        TaktEcTeStatusSynchronizer ecTeStatusSynchronizer,
        ITaktCompanyRepository<TaktEcTe> ecTeRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcPc> pcRepository,
        ITaktCompanyRepository<TaktEcPur> mpRepository,
        ITaktCompanyRepository<TaktEcIqc> iqcRepository,
        ITaktCompanyRepository<TaktEcMc> mcRepository,
        ITaktCompanyRepository<TaktEcMfg2> pcbaRepository,
        ITaktCompanyRepository<TaktEcMtl> smtRepository,
        ITaktCompanyRepository<TaktEcMfg1> assyRepository,
        ITaktCompanyRepository<TaktEcQc> qaRepository,
        ITaktCompanyRepository<TaktEcMe> teRepository,
        ITaktCompanyRepository<TaktDept> deptRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository)
    {
        _deptAccess = deptAccess;
        _ecTeStatusSynchronizer = ecTeStatusSynchronizer;
        _ecTeRepository = ecTeRepository;
        _ecDetailRepository = ecDetailRepository;
        _pcRepository = pcRepository;
        _mpRepository = mpRepository;
        _iqcRepository = iqcRepository;
        _mcRepository = mcRepository;
        _pcbaRepository = pcbaRepository;
        _smtRepository = smtRepository;
        _assyRepository = assyRepository;
        _qaRepository = qaRepository;
        _teRepository = teRepository;
        _deptRepository = deptRepository;
        _materialPlantRepository = materialPlantRepository;
    }

    /// <summary>
    /// 取明细在指定部门的最大行号
    /// </summary>
    /// <param name="ecDetailId">明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <returns>最大行号</returns>
    public Task<int> GetMaxLineNumberForDetailDeptAsync(
        long ecDetailId,
        string deptCode,
        string tenantCode,
        string companyCode) =>
        _deptAccess.GetMaxLineNumberForDetailDeptAsync(ecDetailId, deptCode, tenantCode, companyCode);

    /// <summary>
    /// 根据明细与部门加载执行实体
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>部门执行实体</returns>
    public Task<object?> LoadByDetailAndDeptAsync(long ecDetailId, string deptCode) =>
        FirstEntityByDetailAndDeptAsync(ecDetailId, deptCode);

    /// <summary>
    /// 批量加载指定部门的执行映射（明细 ID → 实体）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>映射</returns>
    public async Task<Dictionary<long, object>> LoadMapByDetailIdsAsync(
        IReadOnlyList<long> detailIds,
        string deptCode)
    {
        if (detailIds.Count == 0)
        {
            return new Dictionary<long, object>();
        }
        var details = await _ecDetailRepository.GetListAsync(x => detailIds.Contains(x.Id));
        var result = new Dictionary<long, object>(details.Count);
        foreach (var group in details.GroupBy(x => x.EcCode ?? string.Empty))
        {
            if (group.Key.Length == 0 || group.First() is not TaktEcDetail first)
            {
                continue;
            }
            var master = await GetMasterControlScopeAsync(first);
            var execs = await ListEntitiesByEcCodeAndDeptAsync(group.Key, deptCode);
            var byGrain = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (var exec in execs)
            {
                if (exec is ITaktEcDeptExecEntity row && !string.IsNullOrWhiteSpace(row.EcGrainKey))
                {
                    byGrain.TryAdd(row.EcGrainKey, exec);
                }
            }
            foreach (var detail in group)
            {
                var grainKey = TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, master);
                if (byGrain.TryGetValue(grainKey, out var exec))
                {
                    result[detail.Id] = exec;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 批量加载全部部门的执行分组（明细 ID → 实体列表）
    /// </summary>
    /// <param name="detailIds">明细 ID 列表</param>
    /// <returns>分组映射</returns>
    public async Task<Dictionary<long, List<object>>> LoadGroupsAllDeptsAsync(IReadOnlyList<long> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return new Dictionary<long, List<object>>();
        }
        var details = await _ecDetailRepository.GetListAsync(x => detailIds.Contains(x.Id));
        var result = new Dictionary<long, List<object>>();
        foreach (var detail in details)
        {
            var rows = new List<object>();
            foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
            {
                var exec = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
                if (exec != null)
                {
                    rows.Add(exec);
                }
            }
            result[detail.Id] = rows;
        }
        return result;
    }

    /// <summary>
    /// 统计租户+公司范围内全部部门执行行数
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司代码</param>
    /// <param name="isImplemented">是否实施（空=全部）</param>
    /// <returns>行数</returns>
    public async Task<int> CountAllDeptRowsForScopeAsync(string tenantCode, string companyCode, int? isImplemented = null)
    {
        var execs = new List<object>();
        execs.AddRange(await _pcRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _mpRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _iqcRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _mcRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _pcbaRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _smtRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _assyRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _qaRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        execs.AddRange(await _teRepository.GetListAsync(x => x.TenantCode == tenantCode && x.CompanyCode == companyCode));
        if (!isImplemented.HasValue)
        {
            return execs.Count;
        }
        return execs.Count(x => TaktEcDeptEntityHelper.MatchesIsImplemented(x, isImplemented.Value));
    }

    /// <summary>
    /// 按明细 ID 批量删除执行记录（含子表）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>任务</returns>
    public async Task DeleteByDetailIdCascadeAsync(long ecDetailId)
    {
        var existingRows = await _deptAccess.ListBaseByEcDetailIdAsync(ecDetailId);
        var ecCode = existingRows.FirstOrDefault()?.EcCode;
        await _deptAccess.DeleteAllByEcDetailIdAsync(ecDetailId);
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(ecCode);
    }

    /// <summary>
    /// 按设变单号删除全部部门执行行（含子表；用于明细先删后插后的孤儿清理）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>任务</returns>
    public async Task DeleteAllByEcCodeAsync(string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        await _pcRepository.DeleteAsync(x => x.EcCode == code);
        await _mpRepository.DeleteAsync(x => x.EcCode == code);
        await _iqcRepository.DeleteAsync(x => x.EcCode == code);
        await _mcRepository.DeleteAsync(x => x.EcCode == code);
        await _pcbaRepository.DeleteAsync(x => x.EcCode == code);
        await _smtRepository.DeleteAsync(x => x.EcCode == code);
        await _assyRepository.DeleteAsync(x => x.EcCode == code);
        await _qaRepository.DeleteAsync(x => x.EcCode == code);
        await _teRepository.DeleteAsync(x => x.EcCode == code);
    }

    /// <summary>
    /// 实施范围=内部/技术时不做采购类型等条件扇出（执行内容已按「实施范围-内部」「实施范围-技术」填好）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <returns>是否跳过条件扇出</returns>
    private async Task<bool> ShouldSkipConditionFanOutAsync(string? ecCode)
    {
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            return false;
        }
        var te = await _ecTeRepository.FirstAsync(x => x.EcCode == ecCode);
        return te != null
            && (te.EcImplScope == TaktEcScopeConstants.Internal
                || te.EcImplScope == TaktEcScopeConstants.Technical);
    }

    /// <summary>
    /// 读取技术课主表控制范围。明细不保存该字段，新建执行行时按部门解析。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>1=按成品，2=按机种，3=按设变，4=不管理；缺主表时按机种</returns>
    private async Task<int> GetMasterControlScopeAsync(TaktEcDetail detail)
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
        var raw = te?.EcControlScope ?? TaktEcControlScopeConstants.ByModel;
        return raw is TaktEcControlScopeConstants.ByFinishedProduct
            or TaktEcControlScopeConstants.ByModel
            or TaktEcControlScopeConstants.ByEngineeringChange
            or TaktEcControlScopeConstants.NotManaged
            ? raw
            : TaktEcControlScopeConstants.ByModel;
    }
    /// <summary>
    /// 取与执行行去重键相同的一条未作废明细。
    /// </summary>
    /// <param name="source">部门执行行</param>
    /// <returns>明细；没有时 null</returns>
    private async Task<TaktEcDetail?> FirstDetailForExecAsync(ITaktEcDeptExecEntity source)
    {
        if (string.IsNullOrWhiteSpace(source.EcCode) || string.IsNullOrWhiteSpace(source.EcGrainKey))
        {
            return null;
        }
        var details = await _ecDetailRepository.GetListAsync(x => x.EcCode == source.EcCode && x.IsObsolete == 0);
        if (details.Count == 0)
        {
            return null;
        }
        var master = await GetMasterControlScopeAsync(details[0]);
        return details.FirstOrDefault(x =>
            TaktEcScopeRowGrain.BuildExecGrainKey(x, source.DeptCode, master) == source.EcGrainKey);
    }
    /// <summary>
    /// 按执行行去重键更新停产状态。
    /// </summary>
    /// <param name="source">部门执行行</param>
    /// <param name="discontinuedStatus">停产状态</param>
    /// <returns>任务</returns>
    public async Task ApplyDiscontinuedStatusForExecAsync(ITaktEcDeptExecEntity source, string discontinuedStatus)
    {
        ArgumentNullException.ThrowIfNull(source);
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        await ApplyDiscontinuedStatusForDetailAsync(detail.Id, discontinuedStatus);
    }

    /// <summary>
    /// 从部门视图更新 DTO 写入或更新执行记录
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="dto">更新 DTO</param>
    /// <param name="lineNumberGenerator">行号生成回调</param>
    /// <returns>部门执行实体</returns>
    public async Task<object> UpsertFromViewUpdateAsync(
        TaktEcDetail detail,
        string deptCode,
        TaktEcDeptViewUpdateDto dto,
        Func<Task<int>> lineNumberGenerator)
    {
        object? exec;
        bool isNew;
        if (deptCode == TaktEcDeptCodes.Mfg2)
        {
            var route = TaktEcMtlRouteHelper.Resolve(detail);
            if (route == TaktEcMtlRouteTarget.None)
            {
                await ObsoletePcbaBothTablesAsync(detail.Id);
                throw new InvalidOperationException("制造二课当前明细无有效执行表路由（F 且非 C003）");
            }
            if (route == TaktEcMtlRouteTarget.Mtl
                && !TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
            {
                await ObsoletePcbaBothTablesAsync(detail.Id);
                throw new InvalidOperationException("制造二课 SMT需要有效新物料编码");
            }
            await ObsoleteTypedDeptExecIfExistsAsync(
                detail.Id,
                route == TaktEcMtlRouteTarget.Mtl
                    ? TaktEcMtlRouteTarget.Mfg2
                    : TaktEcMtlRouteTarget.Mtl);
            exec = await FirstSmtEntityByDetailAndTargetAsync(detail.Id, route);
            isNew = exec == null;
            exec = await CreatePcbaConcreteExec(detail, route, exec, await lineNumberGenerator(), applyNotRelatedAuto: false);
        }
        else
        {
            exec = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
            isNew = exec == null;
            exec = await CreateConcreteExec(detail, deptCode, exec, await lineNumberGenerator(), applyNotRelatedAuto: false);
        }
        ApplyViewUpdateToExec(exec, dto);
        ApplyViewUpdateDeptFields(exec, dto);
        var saved = await SaveEntityAsync(exec, deptCode, isNew);
        if (saved is TaktEcPc pc)
        {
            await FanOutPcFillableByEcModelAndRootMaterialAsync(pc);
        }
        else if (saved is TaktEcPur mp)
        {
            await FanOutMpFillableByEcAndNewMaterialAsync(mp);
        }
        else if (saved is TaktEcIqc iqc)
        {
            await FanOutIqcFillableByEcAndNewMaterialAsync(iqc);
        }
        else if (saved is TaktEcMc mc)
        {
            await FanOutMcFillableByEcModelAndNewMaterialAsync(mc);
        }
        else if (saved is TaktEcMfg2 pcba)
        {
            await FanOutPcbaFillableByEcModelAndRootMaterialAsync(pcba);
        }
        else if (saved is TaktEcMtl smt)
        {
            await FanOutSmtFillableByEcAndParentMaterialAsync(smt);
        }
        else if (saved is TaktEcMfg1 assy)
        {
            await FanOutAssyFillableByEcModelAndRootMaterialAsync(assy);
        }
        else if (saved is TaktEcQc qa)
        {
            await FanOutQaFillableByEcModelAndRootMaterialAsync(qa);
        }
        else if (saved is TaktEcMe te)
        {
            await FanOutTeFillableByEcModelAndRootMaterialAsync(te);
        }
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
        await TryCascadeAfterGateDeptCompletedAsync(detail, deptCode, saved);
        return saved;
    }

    /// <summary>
    /// 采购课：将可填字段同步到同设变单号+新物料且新采购类型为 F 的全部未作废执行行（不含当前行）。
    /// 源明细非 F 时不扇出，避免覆盖「采购无关」自动完成行。
    /// </summary>
    /// <param name="source">已写入当前行的采购执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutMpFillableByEcAndNewMaterialAsync(TaktEcPur source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null || !TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcNewMaterialCode == detail.EcNewMaterialCode
            && x.EcNewPurchaseType == purchaseTypeF
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _mpRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.PurchaseOrderIssueDate = source.PurchaseOrderIssueDate;
            row.Supplier = source.Supplier;
            row.PurchaseOrderCode = source.PurchaseOrderCode;
            row.EcOldPartDisposition = source.EcOldPartDisposition;
        }
        await _mpRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 受检课：将可填字段同步到同设变单号+新物料且新品需检验=1 的全部未作废执行行（不含当前行）。
    /// 源明细无需检验时不扇出，避免覆盖「跟 IQC 无关」自动完成行。
    /// </summary>
    /// <param name="source">已写入当前行的受检执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutIqcFillableByEcAndNewMaterialAsync(TaktEcIqc source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null || detail.EcNewRequiresInspection != 1)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcNewMaterialCode == detail.EcNewMaterialCode
            && x.EcNewRequiresInspection == 1
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _iqcRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.IqcOrderCode = source.IqcOrderCode;
            row.InspectionDate = source.InspectionDate;
        }
        await _iqcRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 部管课：将可填字段同步到同设变单号+机种且采购类型 F、仓库非 C003 的全部未作废执行行（不含当前行）。
    /// 源明细不可见时不扇出，避免覆盖「跟部管无关」自动完成行。
    /// </summary>
    /// <param name="source">已写入当前行的部管执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutMcFillableByEcModelAndNewMaterialAsync(TaktEcMc source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null || !TaktEcScopeConstants.IsMcVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var model = detail.EcModelCode ?? string.Empty;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == model
            && x.EcNewPurchaseType == purchaseTypeF
            && (x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003)
            && x.IsObsolete == 0);
        var mcKind = TaktEcScopeRowGrain.ResolveKind(TaktEcDeptCodes.Mc, source.EcControlScope);
        var mcKey = TaktEcScopeRowGrain.BuildKey(detail, mcKind);
        siblingDetails = siblingDetails
            .Where(x => string.Equals(TaktEcScopeRowGrain.BuildKey(x, mcKind), mcKey, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _mcRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.OutboundBatch = source.OutboundBatch;
            row.OutboundDate = source.OutboundDate;
        }
        await _mcRepository.UpdateRangeAsync(rows);
        if (source.IsImplemented != 1)
        {
            return;
        }
        foreach (var row in rows)
        {
            var coveredMc = await FirstDetailForExecAsync(row); if (coveredMc != null) { await TryCascadeAfterGateDeptCompletedByDetailIdAsync(coveredMc.Id, TaktEcDeptCodes.Mc, row); }
        }
    }

    /// <summary>
    /// SMT：将可填字段同步到同设变单号+机种且 F+C003 的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的 SMT执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutSmtFillableByEcAndParentMaterialAsync(TaktEcMtl source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null
            || !TaktEcScopeConstants.IsPcbaC003ExternalGroup(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var model = detail.EcModelCode ?? string.Empty;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == model
            && x.EcNewPurchaseType == purchaseTypeF
            && x.EcNewWarehouse != null
            && x.EcNewWarehouse == warehouseC003
            && x.IsObsolete == 0);
        var smtKind = TaktEcScopeRowGrain.ResolveKind(TaktEcDeptCodes.Mtl, source.EcControlScope);
        var smtKey = TaktEcScopeRowGrain.BuildKey(detail, smtKind);
        siblingDetails = siblingDetails
            .Where(x => string.Equals(TaktEcScopeRowGrain.BuildKey(x, smtKind), smtKey, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _smtRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.OutboundBatch = source.OutboundBatch;
            row.OutboundDate = source.OutboundDate;
        }
        await _smtRepository.UpdateRangeAsync(rows);
        if (source.IsImplemented != 1)
        {
            return;
        }
        foreach (var row in rows)
        {
            var coveredSmt = await FirstDetailForExecAsync(row); if (coveredSmt != null) { await TryCascadeAfterGateDeptCompletedByDetailIdAsync(coveredSmt.Id, TaktEcDeptCodes.Mtl, row); }
        }
    }

    /// <summary>
    /// 制二课：将可填字段同步到同设变单号+机种+根物料编码且采购类型非 F 的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制二执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutPcbaFillableByEcModelAndRootMaterialAsync(TaktEcMfg2 source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null || !TaktEcScopeConstants.IsPcbaOtherPurchaseGroup(detail.EcNewPurchaseType))
        {
            return;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcRootMaterialCode == detail.EcRootMaterialCode
            && x.EcNewPurchaseType != purchaseTypeF
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _pcbaRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionTeam = source.ProductionTeam;
            row.ProductionDate = source.ProductionDate;
            row.ImplementationBatch = source.ImplementationBatch;
        }
        await _pcbaRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 生管课：将可填字段同步到同设变单号+机种+根物料编码的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的生管执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutPcFillableByEcModelAndRootMaterialAsync(TaktEcPc source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcRootMaterialCode == detail.EcRootMaterialCode
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _pcRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count > 0)
        {
            foreach (var row in rows)
            {
                row.IsImplemented = source.IsImplemented;
                row.ExecContent = source.ExecContent;
                row.ScheduledDate = source.ScheduledDate;
                row.ScheduledBatch = source.ScheduledBatch;
                row.PoRemainder = source.PoRemainder;
                row.BalanceQuantity = source.BalanceQuantity;
                row.EcOldPartDisposition = source.EcOldPartDisposition;
            }
            await _pcRepository.UpdateRangeAsync(rows);
        }
        // 去重后常仅一行生管：预定日期/批次冗余到除采购/受检外的执行表
        await SyncPcScheduledRedundantToSiblingDeptsAsync(source.EcCode, source.ScheduledDate, source.ScheduledBatch);
    }

    /// <summary>
    /// 生管预定日期/预定批次 → 部管/制一/制二/SMT/品管（不含采购、受检、制技）。
    /// </summary>
    private async Task SyncPcScheduledRedundantToSiblingDeptsAsync(
        string ecCode,
        DateTime? scheduledDate,
        string? scheduledBatch)
    {
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            return;
        }
        async Task SyncAsync<T>(ITaktCompanyRepository<T> repo, Action<T, DateTime?, string?> apply)
            where T : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
        {
            var rows = await repo.GetListAsync(x => x.EcCode == ecCode && x.IsObsolete == 0);
            if (rows.Count == 0)
            {
                return;
            }
            foreach (var row in rows)
            {
                apply(row, scheduledDate, scheduledBatch);
            }
            await repo.UpdateRangeAsync(rows);
        }
        await SyncAsync(_mcRepository, (TaktEcMc r, DateTime? d, string? b) => { r.ScheduledDate = d; r.ScheduledBatch = b; });
        await SyncAsync(_assyRepository, (TaktEcMfg1 r, DateTime? d, string? b) =>
        {
            if (IsAssyNotRelatedDefault(r))
            {
                return;
            }
            r.ScheduledDate = d;
            r.ScheduledBatch = b;
        });
        await SyncAsync(_pcbaRepository, (TaktEcMfg2 r, DateTime? d, string? b) => { r.ScheduledDate = d; r.ScheduledBatch = b; });
        await SyncAsync(_smtRepository, (TaktEcMtl r, DateTime? d, string? b) => { r.ScheduledDate = d; r.ScheduledBatch = b; });
        await SyncAsync(_qaRepository, (TaktEcQc r, DateTime? d, string? b) => { r.ScheduledDate = d; r.ScheduledBatch = b; });
    }

    /// <summary>
    /// 制一课：将可填字段同步到同设变单号+机种+根物料编码的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制一执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutAssyFillableByEcModelAndRootMaterialAsync(TaktEcMfg1 source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcRootMaterialCode == detail.EcRootMaterialCode
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _assyRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionTeam = source.ProductionTeam;
            row.ProductionDate = source.ProductionDate;
            row.ImplementationBatch = source.ImplementationBatch;
        }
        await _assyRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 品管课：将可填字段同步到同设变单号+机种+根物料编码的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的品管执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutQaFillableByEcModelAndRootMaterialAsync(TaktEcQc source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.EcRootMaterialCode == detail.EcRootMaterialCode
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _qaRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.ProductionTeam = source.ProductionTeam;
            row.InspectionDate = source.InspectionDate;
            row.InspectionBatch = source.InspectionBatch;
            row.SamplingCode = source.SamplingCode;
        }
        await _qaRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 制技课：将可填字段同步到同设变单号+机种的全部未作废执行行（不含当前行）。
    /// </summary>
    /// <param name="source">已写入当前行的制技执行实体</param>
    /// <returns>任务</returns>
    public async Task FanOutTeFillableByEcModelAndRootMaterialAsync(TaktEcMe source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (await ShouldSkipConditionFanOutAsync(source.EcCode))
        {
            return;
        }
        var detail = await FirstDetailForExecAsync(source);
        if (detail == null)
        {
            return;
        }
        var siblingDetails = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode
            && x.EcModelCode == detail.EcModelCode
            && x.IsObsolete == 0);
        if (siblingDetails.Count == 0)
        {
            return;
        }
        var detailIds = siblingDetails.Select(x => x.Id).ToList();
        var rows = await _teRepository.GetListAsync(x =>
            x.EcCode == source.EcCode && SqlFunc.ToString(x.EcGrainKey) == source.EcGrainKey && x.IsObsolete == 0 && x.Id != source.Id);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsImplemented = source.IsImplemented;
            row.ExecContent = source.ExecContent;
            row.SopDate = source.SopDate;
            row.IsSopUpdated = source.IsSopUpdated;
        }
        await _teRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 按明细确保部门执行行，并按实施范围写入实施状态与执行内容。
    /// 内部：各部门强制「实施范围-内部」。技术：各部门强制「实施范围-技术」。部管：制一/制二/品管强制「实施范围-部管」。停产≠Z0 一律先写 EOL，判断不变。
    /// 全仕向/部管：新建或系统文案可覆盖；EOL 优先；条件无关行再按采购/受检/部管规则写入。
    /// 采购/受检/部管：新物料编码为空或「0」时不新建、已有行作废（内部/技术同样遵守）。
    /// 采购/受检/部管：列表可见组内与 QueryHelper 同键去重，仅最大 Id 明细生成执行行（其余作废）。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="autoComplete">true=按实施范围自动填写；false=待人工填写</param>
    /// <param name="ecImplScope">实施范围（字典 logistics_manufacturing_ec_impl_scope_category）</param>
    /// <returns>部门执行实体；跳过生成时为 null</returns>
    public async Task<object?> UpsertDeptExecWithFillModeAsync(
        TaktEcDetail detail,
        string deptCode,
        bool autoComplete,
        int ecImplScope)
    {
        ArgumentNullException.ThrowIfNull(detail);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        if (deptCode == TaktEcDeptCodes.Mfg2 || deptCode == TaktEcDeptCodes.Mtl)
        {
            return await UpsertPcbaDeptExecWithFillModeAsync(detail, autoComplete, ecImplScope);
        }
        if (!await IsKnownRootMaterialAsync(detail))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        if (ecImplScope != TaktEcScopeConstants.Internal
            && ecImplScope != TaktEcScopeConstants.Technical
            && TaktEcScopeConstants.IsNewMaterialDependentDept(deptCode)
            && !TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        if (await ShouldSkipDeptExecByScopeGrainAsync(detail, deptCode, ecImplScope))
        {
            await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
            return null;
        }
        var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
        var isNew = existing == null;
        var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var applyNotRelatedAuto = TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope);
        var exec = await CreateConcreteExec(detail, deptCode, existing, lineNumber, applyNotRelatedAuto);
        await AssignScopeRootGroupAsync(exec, detail, deptCode, ecImplScope);
        EnsureDeptName(exec, await ResolveDeptNameAsync(deptCode));
        // 新物料有效时恢复此前因「无新物料」作废的行
        if (exec is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
        {
            deptExec.IsObsolete = 0;
        }
        ApplyScopeFillMode(exec, isNew, autoComplete, ecImplScope);
        var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
        if (!isEolFilled
            && TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope))
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        var autoCloseContent = await ResolveAutoCloseExecContentAsync(detail, deptCode, ecImplScope);
        if (autoCloseContent != null)
        {
            ApplyAutoCloseDeptExec(exec, isNew, autoCloseContent);
        }
        await ApplyNonFertDeptControlledAsync(exec, detail, deptCode, ecImplScope);
        // 批量派生时不在此刷新主表状态（技术课导入路径不按子表重算；部门填报 CRUD 自行 Refresh）
        return await SaveEntityAsync(exec, deptCode, isNew);
    }

    /// <summary>
    /// 制造二课：按 F+C003 / 非 F 路由到双表；其余组别两侧作废
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="autoComplete">是否自动填写</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>部门执行实体；跳过时 null</returns>
    private async Task<object?> UpsertPcbaDeptExecWithFillModeAsync(
        TaktEcDetail detail,
        bool autoComplete,
        int ecImplScope)
    {
        _ = autoComplete;
        if (!await IsKnownRootMaterialAsync(detail))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return null;
        }
        object? saved = null;
        if (await ShouldSkipDeptExecByScopeGrainAsync(detail, TaktEcDeptCodes.Mtl, ecImplScope))
        {
            await ObsoleteTypedDeptExecIfExistsAsync(detail.Id, TaktEcMtlRouteTarget.Mtl);
        }
        else
        {
            saved = await UpsertSmtTypedExecAsync(detail, TaktEcMtlRouteTarget.Mtl, ecImplScope);
        }
        if (await ShouldSkipDeptExecByScopeGrainAsync(detail, TaktEcDeptCodes.Mfg2, ecImplScope))
        {
            await ObsoleteTypedDeptExecIfExistsAsync(detail.Id, TaktEcMtlRouteTarget.Mfg2);
        }
        else
        {
            saved = await UpsertSmtTypedExecAsync(detail, TaktEcMtlRouteTarget.Mfg2, ecImplScope);
        }
        return saved;
    }

    /// <summary>
    /// 制造二课指定目标表 Upsert
    /// </summary>
    private async Task<object> UpsertSmtTypedExecAsync(
        TaktEcDetail detail,
        TaktEcMtlRouteTarget target,
        int ecImplScope)
    {
        var existing = await FirstSmtEntityByDetailAndTargetAsync(detail.Id, target);
        var isNew = existing == null;
        var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var applyNotRelatedAuto = TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope);
        var exec = await CreatePcbaConcreteExec(detail, target, existing, lineNumber, applyNotRelatedAuto);
        var targetDeptCode = target == TaktEcMtlRouteTarget.Mtl
            ? TaktEcDeptCodes.Mtl
            : TaktEcDeptCodes.Mfg2;
        await AssignScopeRootGroupAsync(exec, detail, targetDeptCode, ecImplScope);
        EnsureDeptName(exec, await ResolveDeptNameAsync(targetDeptCode));
        if (exec is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
        {
            deptExec.IsObsolete = 0;
        }
        var sideAuto = TaktEcScopeConstants.ShouldAutoCompleteExec(
            ecImplScope,
            targetDeptCode,
            detail.EcNewPurchaseType,
            detail.EcNewWarehouse);
        ApplyScopeFillMode(exec, isNew, sideAuto, ecImplScope);
        var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
        if (!isEolFilled
            && TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope))
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        var autoCloseContent = await ResolveAutoCloseExecContentAsync(detail, targetDeptCode, ecImplScope);
        if (autoCloseContent != null)
        {
            ApplyAutoCloseDeptExec(exec, isNew, autoCloseContent);
        }
        await ApplyNonFertDeptControlledAsync(exec, detail, targetDeptCode, ecImplScope);
        return await SavePcbaEntityAsync(exec, isNew);
    }

    /// <summary>
    /// 新物料无效时：将已有采购/受检/部管执行行标记作废
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    private async Task ObsoleteDeptExecIfExistsAsync(long ecDetailId, string deptCode)
    {
        if (deptCode == TaktEcDeptCodes.Mfg2)
        {
            await ObsoletePcbaBothTablesAsync(ecDetailId);
            return;
        }
        var existing = await FirstEntityByDetailAndDeptAsync(ecDetailId, deptCode);
        if (existing == null || TaktEcDeptEntityHelper.GetIsObsolete(existing) == 1)
        {
            return;
        }
        if (existing is ITaktEcDeptExecEntity deptExec)
        {
            deptExec.IsObsolete = 1;
        }
        await SaveEntityAsync(existing, deptCode, isNew: false);
    }

    /// <summary>
    /// 作废制造二课两侧表中该明细行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    private async Task ObsoletePcbaBothTablesAsync(long ecDetailId)
    {
        await ObsoleteTypedDeptExecIfExistsAsync(ecDetailId, TaktEcMtlRouteTarget.Mtl);
        await ObsoleteTypedDeptExecIfExistsAsync(ecDetailId, TaktEcMtlRouteTarget.Mfg2);
    }

    /// <summary>
    /// 作废制造二课指定目标表行
    /// </summary>
    private async Task ObsoleteTypedDeptExecIfExistsAsync(long ecDetailId, TaktEcMtlRouteTarget target)
    {
        var existing = await FirstSmtEntityByDetailAndTargetAsync(ecDetailId, target);
        if (existing == null || TaktEcDeptEntityHelper.GetIsObsolete(existing) == 1)
        {
            return;
        }
        if (existing is ITaktEcDeptExecEntity deptExec)
        {
            deptExec.IsObsolete = 1;
        }
        await SavePcbaEntityAsync(existing, isNew: false);
    }

    /// <summary>
    /// 按实施范围去重：不合格或同键存在更大 Id 时跳过
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecImplScope">实施范围；空则用明细上的冗余值</param>
    /// <returns>true=跳过并应作废已有行</returns>
    private async Task<bool> ShouldSkipDeptExecByScopeGrainAsync(TaktEcDetail detail, string deptCode, int? ecImplScope = null)
    {
        var scope = ecImplScope ?? detail.EcImplScope;
        var masterDeptScope = await GetMasterControlScopeAsync(detail);
        var deptScope = TaktEcControlScopeConstants.ResolveForDept(deptCode, scope, masterDeptScope);
        var siblings = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode && x.IsObsolete == 0);
        IReadOnlySet<string>? fertRootKeys = null;
        if (deptCode == TaktEcDeptCodes.Mfg1 && deptScope == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            fertRootKeys = CollectFertRootMaterialKeys(siblings);
        }
        if (!TaktEcScopeRowGrain.IsEligible(detail, deptCode, deptScope, fertRootKeys))
        {
            return true;
        }
        return TaktEcScopeRowGrain.HasNewer(detail, siblings, deptCode, deptScope, fertRootKeys);
    }

    /// <summary>
    /// 把同去重键的根物料编码组写到执行行
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>任务</returns>
    private async Task AssignScopeRootGroupAsync(
        object exec,
        TaktEcDetail detail,
        string deptCode,
        int ecImplScope)
    {
        var siblings = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode && x.IsObsolete == 0);
        var masterDeptScope = await GetMasterControlScopeAsync(detail);
        var deptScope = TaktEcControlScopeConstants.ResolveForDept(deptCode, ecImplScope, masterDeptScope);
        IReadOnlySet<string>? fertRootKeys = null;
        if (deptCode == TaktEcDeptCodes.Mfg1 && deptScope == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            fertRootKeys = CollectFertRootMaterialKeys(siblings);
        }
        var groups = TaktEcScopeRowGrain.BuildRootGroups(siblings, deptCode, deptScope, fertRootKeys);
        var modelGroups = TaktEcScopeRowGrain.BuildModelGroups(siblings, deptCode, deptScope, fertRootKeys);
        var kind = TaktEcScopeRowGrain.ResolveKind(deptCode, deptScope);
        var groupKey = TaktEcScopeRowGrain.BuildKey(detail, kind, deptCode);
        if (groups.TryGetValue(groupKey, out var rootGroup))
        {
            AssignRootMaterialCodeGroup(exec, rootGroup);
        }
        if (modelGroups.TryGetValue(groupKey, out var modelGroup))
        {
            AssignModelCodeGroup(exec, modelGroup);
        }
        if (deptCode is TaktEcDeptCodes.Pur or TaktEcDeptCodes.Iqc)
        {
            SetDiscontinuedStatusIfPresent(
                exec,
                TaktEcScopeRowGrain.ResolvePurchaseInspectDiscontinuedStatus(siblings, detail, deptCode, deptScope));
        }
        else
        {
            SetDiscontinuedStatusIfPresent(
                exec,
                TaktEcScopeRowGrain.ResolveRootGroupDiscontinuedStatus(siblings, detail, deptCode, deptScope, fertRootKeys));
        }
        if (deptCode == TaktEcDeptCodes.Mfg2)
        {
            var newMaterialGroups = TaktEcScopeRowGrain.BuildCodeGroups(siblings, deptCode, deptScope, fertRootKeys, item => item.EcNewMaterialCode);
            var parentMaterialGroups = TaktEcScopeRowGrain.BuildCodeGroups(siblings, deptCode, deptScope, fertRootKeys, item => item.EcParentMaterialCode);
            if (newMaterialGroups.TryGetValue(groupKey, out var newMaterialGroup)
                && parentMaterialGroups.TryGetValue(groupKey, out var parentMaterialGroup))
            {
                AssignPcbaMaterialGroups(exec, newMaterialGroup, parentMaterialGroup);
            }
        }
    }

    /// <summary>
    /// 采购/受检/部管是否因与列表 QueryHelper 同键去重而跳过生成（组内仅最大 Id 保留）。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>true=跳过并应作废已有行</returns>
    private async Task<bool> ShouldSkipNewMaterialDeptByListDedupAsync(TaktEcDetail detail, string deptCode)
    {
        if (!TaktEcScopeConstants.IsNewMaterialDependentDept(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pur => await HasNewerMpVisibleSiblingAsync(detail),
            TaktEcDeptCodes.Iqc => await HasNewerIqcVisibleSiblingAsync(detail),
            _ => false
        };
    }

    /// <summary>
    /// 生管/制一/品管：设变+机种+根物料编码组内是否存在更大 Id。制技：设变+机种组内是否存在更大 Id。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>true=跳过并应作废已有行</returns>
    private async Task<bool> ShouldSkipModelRootMaterialDeptByListDedupAsync(TaktEcDetail detail, string deptCode)
    {
        if (deptCode is not (TaktEcDeptCodes.Pc or TaktEcDeptCodes.Mfg1 or TaktEcDeptCodes.Qc or TaktEcDeptCodes.Me))
        {
            return false;
        }
        if (detail.IsObsolete != 0)
        {
            return true;
        }
        if (deptCode == TaktEcDeptCodes.Me)
        {
            var newerByModel = await _ecDetailRepository.FirstAsync(s =>
                s.IsObsolete == 0
                && s.EcCode == detail.EcCode
                && s.EcModelCode == detail.EcModelCode
                && s.Id > detail.Id);
            return newerByModel != null;
        }
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcModelCode == detail.EcModelCode
            && s.EcRootMaterialCode == detail.EcRootMaterialCode
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 制二非 F 可见组内是否存在更大 Id 的同设变+机种+根物料编码明细
    /// </summary>
    private async Task<bool> HasNewerPcbaVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (!TaktEcScopeConstants.IsPcbaOtherPurchaseGroup(detail.EcNewPurchaseType))
        {
            return false;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var model = detail.EcModelCode ?? string.Empty;
        var rootMaterialCode = detail.EcRootMaterialCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcModelCode == model
            && s.EcRootMaterialCode == rootMaterialCode
            && s.EcNewPurchaseType != purchaseTypeF
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 采购课可见组（采购类型 F）内是否存在更大 Id 的同设变+新物料明细
    /// </summary>
    /// <param name="detail">当前明细</param>
    /// <returns>存在更大 Id 兄弟则 true</returns>
    private async Task<bool> HasNewerMpVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (!TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType))
        {
            return false;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var material = detail.EcNewMaterialCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcNewMaterialCode == material
            && s.EcNewPurchaseType == purchaseTypeF
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 受检课可见组（需检验=1）内是否存在更大 Id 的同设变+新物料明细
    /// </summary>
    /// <param name="detail">当前明细</param>
    /// <returns>存在更大 Id 兄弟则 true</returns>
    private async Task<bool> HasNewerIqcVisibleSiblingAsync(TaktEcDetail detail)
    {
        if (detail.EcNewRequiresInspection != 1)
        {
            return false;
        }
        var material = detail.EcNewMaterialCode ?? string.Empty;
        var newer = await _ecDetailRepository.FirstAsync(s =>
            s.IsObsolete == 0
            && s.EcCode == detail.EcCode
            && s.EcNewMaterialCode == material
            && s.EcNewRequiresInspection == 1
            && s.Id > detail.Id);
        return newer != null;
    }

    /// <summary>
    /// 按明细更新停产状态，并同步各课执行行冗余字段与自动填充/清除
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="discontinuedStatus">根物料停产状态（Z0=在产；非 Z0 视为停产，按钮默认写 ZQ）</param>
    public async Task ApplyDiscontinuedStatusForDetailAsync(long ecDetailId, string discontinuedStatus)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        var status = string.IsNullOrWhiteSpace(discontinuedStatus)
            ? TaktEcScopeConstants.PlannedMaterialStatus
            : discontinuedStatus.Trim();
        detail.DiscontinuedStatus = status;
        await _ecDetailRepository.UpdateAsync(detail);
        var te = await _ecTeRepository.FirstAsync(x => x.EcCode == detail.EcCode);
        var ecImplScope = te?.EcImplScope ?? TaktEcScopeConstants.Technical;
        var rows = await ListAllEntitiesByDetailIdsAsync(new[] { detail.Id });
        foreach (var exec in rows)
        {
            if (TaktEcDeptEntityHelper.GetIsObsolete(exec) == 1)
            {
                continue;
            }
            SetDiscontinuedStatusIfPresent(exec, status);
            var deptCode = TaktEcDeptEntityHelper.GetDeptCode(exec);
            var deptScope = TaktEcControlScopeConstants.ResolveForDept(deptCode, ecImplScope, te?.EcControlScope ?? TaktEcControlScopeConstants.ByModel);
            ApplyManualDiscontinuedFillMode(exec, ecImplScope, deptScope, detail);
            if (!TaktEcScopeConstants.IsEolDiscontinued(status)
                && TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope))
            {
                var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
                var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
                if (!isEolFilled)
                {
                    TaktEcExecNotRelated.TryAfterFill(exec, detail);
                }
            }
            await SaveEntityAsync(exec, deptCode == TaktEcDeptCodes.Mfg2 ? TaktEcDeptCodes.Mfg2 : deptCode, isNew: false);
        }
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
    }

    /// <summary>
    /// 写入执行行冗余停产状态（各部门执行表 DiscontinuedStatus）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="status">停产状态</param>
    private static void SetDiscontinuedStatusIfPresent(object exec, string status)
    {
        switch (exec)
        {
            case TaktEcPur e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcIqc e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcPc e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcMc e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcMtl e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcMfg2 e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcMfg1 e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcQc e:
                e.DiscontinuedStatus = status;
                break;
            case TaktEcMe e:
                e.DiscontinuedStatus = status;
                break;
        }
    }

    /// <summary>
    /// 人工停产/在产：停产写「全仕向:按机种:EOL」；在产按实施范围回填或清空
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <param name="ecControlScope">部门实施范围</param>
    /// <param name="detail">设变明细</param>
    private static void ApplyManualDiscontinuedFillMode(object exec, int ecImplScope, int ecControlScope, TaktEcDetail detail)
    {
        if (TaktEcScopeConstants.IsEolDiscontinued(detail.DiscontinuedStatus))
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcScopeConstants.FormatScopeDeptExecContent(ecImplScope, ecControlScope, TaktEcScopeConstants.EolExecContent),
                overwrite: true);
            return;
        }
        if (ecImplScope == TaktEcScopeConstants.Internal
            || ecImplScope == TaktEcScopeConstants.Technical)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcScopeConstants.ResolveAutoExecContent(ecImplScope),
                overwrite: true);
            return;
        }
        var deptCode = TaktEcDeptEntityHelper.GetDeptCode(exec);
        var autoComplete = ResolveAutoCompleteForManualClear(ecImplScope, deptCode, detail);
        if (autoComplete)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcScopeConstants.ResolveAutoExecContent(ecImplScope),
                overwrite: true);
            return;
        }
        TaktEcDeptEntityHelper.SetIsImplemented(exec, 0);
        TaktEcDeptEntityHelper.SetExecContent(exec, string.Empty, overwrite: true);
    }

    /// <summary>
    /// 在产清除后是否自动完成（与编排规则一致：全仕向不自动；部管按待填部门名单）
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="detail">设变明细</param>
    /// <returns>是否自动完成</returns>
    private static bool ResolveAutoCompleteForManualClear(int ecImplScope, string deptCode, TaktEcDetail detail)
    {
        return TaktEcScopeConstants.ShouldAutoCompleteExec(
            ecImplScope,
            deptCode,
            detail.EcNewPurchaseType,
            detail.EcNewWarehouse);
    }

    /// <summary>
    /// 按实施范围写入执行内容。内部、技术、部管自动部门写入场景文案，并把实施标记写成已实施。全仕向等待人工填写。停产与非 FERT 由根物料编码组另写「全仕向:按机种:EOL」。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isNew">是否新建</param>
    /// <param name="autoComplete">是否自动填写（部管非待填部门 / 其它范围由调用方判定）</param>
    /// <param name="ecImplScope">实施范围</param>
    private static void ApplyScopeFillMode(
        object exec,
        bool isNew,
        bool autoComplete,
        int ecImplScope)
    {
        if (ecImplScope == TaktEcScopeConstants.Internal
            || ecImplScope == TaktEcScopeConstants.Technical
            || autoComplete)
        {
            TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
            TaktEcDeptEntityHelper.SetExecContent(
                exec,
                TaktEcScopeConstants.ResolveAutoExecContent(ecImplScope),
                overwrite: true);
            return;
        }
        var currentContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var canOverwrite = isNew
            || TaktEcScopeConstants.IsScopeGeneratedExecContent(currentContent);
        if (!canOverwrite)
        {
            return;
        }
        TaktEcDeptEntityHelper.SetIsImplemented(exec, 0);
        TaktEcDeptEntityHelper.SetExecContent(exec, string.Empty, overwrite: true);
    }

    /// <summary>
    /// 实施范围=部管：门禁部门完成后，按名单补齐生管/采购/受检/部管/制二待填行，其余部门填「实施范围-部管」
    /// </summary>
    public async Task TryCascadeAfterGateDeptCompletedAsync(
        TaktEcDetail detail,
        string deptCode,
        object savedExec)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (TaktEcDeptEntityHelper.GetIsImplemented(savedExec) != 1)
        {
            return;
        }
        var isMcGate = deptCode == TaktEcDeptCodes.Mc;
        var isPcbaGate = deptCode == TaktEcDeptCodes.Mfg2 || deptCode == TaktEcDeptCodes.Mtl;
        if (!isMcGate && !isPcbaGate)
        {
            return;
        }
        var te = await _ecTeRepository.FirstAsync(x => x.EcCode == detail.EcCode);
        if (te == null || te.EcImplScope != TaktEcScopeConstants.MaterialControl)
        {
            return;
        }
        var isC003 = TaktEcScopeConstants.IsPcbaGateWarehouse(detail.EcNewWarehouse);
        if (isMcGate && isC003)
        {
            return;
        }
        if (isPcbaGate && !isC003)
        {
            return;
        }
        foreach (var otherDept in TaktEcDeptCodes.KanbanOrder)
        {
            if (otherDept == deptCode
                || (isPcbaGate && otherDept == TaktEcDeptCodes.Mfg2 && deptCode == TaktEcDeptCodes.Mtl))
            {
                continue;
            }
            var autoCompleteOther = !TaktEcScopeConstants.IsMaterialControlNeedFillDept(
                otherDept,
                detail.EcNewPurchaseType,
                detail.EcNewWarehouse);
            _ = await UpsertDeptExecWithFillModeAsync(
                detail,
                otherDept,
                autoComplete: autoCompleteOther,
                TaktEcScopeConstants.MaterialControl);
        }
    }

    /// <summary>
    /// 按执行行去重键触发门禁部门完成后的级联。
    /// </summary>
    /// <param name="savedExec">已保存的部门执行实体</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>任务</returns>
    public async Task TryCascadeAfterGateDeptCompletedForExecAsync(ITaktEcDeptExecEntity savedExec, string deptCode)
    {
        ArgumentNullException.ThrowIfNull(savedExec);
        var detail = await FirstDetailForExecAsync(savedExec);
        if (detail == null)
        {
            return;
        }
        await TryCascadeAfterGateDeptCompletedAsync(detail, deptCode, savedExec);
    }

    /// <summary>
    /// 按明细 ID 触发门禁部门完成后的级联（CRUD 写路径用）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="savedExec">已保存的部门执行实体</param>
    /// <returns>任务</returns>
    public async Task TryCascadeAfterGateDeptCompletedByDetailIdAsync(
        long ecDetailId,
        string deptCode,
        object savedExec)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return;
        }
        await TryCascadeAfterGateDeptCompletedAsync(detail, deptCode, savedExec);
    }

    /// <summary>
    /// 将视图更新 DTO 写入部门执行实体
    /// </summary>
    private static void ApplyViewUpdateToExec(object exec, TaktEcDeptViewUpdateDto dto)
    {
        switch (exec)
        {
            case TaktEcPc e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcPur e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcIqc e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcMc e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcMtl e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcMfg2 e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcMfg1 e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcQc e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
            case TaktEcMe e:
                e.IsImplemented = dto.IsImplemented;
                e.ExecContent = dto.Content;
                e.Remark = dto.Remark;
                break;
        }
    }

    /// <summary>
    /// 从视图 DTO 写入部门专有字段（实体已含同表字段）
    /// </summary>
    private static void ApplyViewUpdateDeptFields(object exec, TaktEcDeptViewUpdateDto dto)
    {
        switch (exec)
        {
            case TaktEcPc pc:
                pc.ScheduledDate = dto.ScheduledDate;
                pc.ScheduledBatch = dto.ScheduledBatch;
                var poRemainder = string.IsNullOrWhiteSpace(dto.PoRemainder) ? null : dto.PoRemainder.Trim();
                if (!TaktRegexHelper.IsValidEcPoRemainder(poRemainder))
                {
                    throw new TaktBusinessException("Po残须为空或「采购单10位-序号4位:数量」，如 4500123456-0010:18.5");
                }
                pc.PoRemainder = poRemainder;
                pc.BalanceQuantity = dto.BalanceQuantity;
                pc.EcOldPartDisposition = dto.OldProductHandling;
                break;
            case TaktEcPur pur:
                pur.PurchaseOrderIssueDate = dto.PurchaseOrderIssueDate;
                pur.Supplier = dto.Supplier;
                pur.PurchaseOrderCode = dto.PurchaseOrderCode;
                pur.EcOldPartDisposition = dto.OldProductHandling;
                break;
            case TaktEcIqc iqc:
                iqc.IqcOrderCode = dto.IqcOrderCode;
                iqc.InspectionDate = dto.InspectionDate;
                break;
            case TaktEcMc mc:
                mc.OutboundBatch = dto.OutboundBatch;
                mc.OutboundDate = dto.OutboundDate;
                break;
            case TaktEcMtl electronic:
                electronic.OutboundBatch = dto.OutboundBatch;
                electronic.OutboundDate = dto.OutboundDate;
                break;
            case TaktEcMfg2 mfg2:
                mfg2.ProductionDate = dto.ProductionDate;
                mfg2.ProductionTeam = dto.ProductionTeam;
                mfg2.ImplementationBatch = dto.ImplementationBatch ?? dto.ProductionBatch;
                break;
            case TaktEcMfg1 mfg1:
                mfg1.ProductionTeam = dto.ProductionTeam;
                mfg1.ProductionDate = dto.ProductionDate;
                mfg1.ImplementationBatch = dto.ImplementationBatch;
                break;
            case TaktEcQc qc:
                qc.ProductionTeam = dto.ProductionTeam;
                qc.InspectionDate = dto.InspectionDate;
                qc.InspectionBatch = dto.InspectionBatch;
                qc.SamplingCode = dto.SamplingCode;
                break;
            case TaktEcMe me:
                me.SopDate = dto.SopDate;
                me.IsSopUpdated = dto.IsSopUpdated;
                break;
        }
    }

    /// <summary>
    /// 按明细与部门查找执行实体
    /// </summary>
    private async Task<object?> FirstEntityByDetailAndDeptAsync(long ecDetailId, string deptCode)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return null;
        }
        var master = await GetMasterControlScopeAsync(detail);
        return await FirstEntityByGrainAsync(detail, deptCode, master);
    }

    /// <summary>
    /// 按执行去重键与部门查找执行实体。同一键的多条明细命中同一行。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="masterControlScope">技术课主表控制范围</param>
    /// <returns>部门执行实体</returns>
    private async Task<object?> FirstEntityByGrainAsync(TaktEcDetail detail, string deptCode, int masterControlScope)
    {
        var grainKey = TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, masterControlScope);
        var ecCode = detail.EcCode ?? string.Empty;
        if (deptCode == TaktEcDeptCodes.Mfg2 || deptCode == TaktEcDeptCodes.Mtl)
        {
            return await FirstSmtEntityByDetailAndTargetAsync(
                detail,
                deptCode == TaktEcDeptCodes.Mtl ? TaktEcMtlRouteTarget.Mtl : TaktEcMtlRouteTarget.Mfg2,
                masterControlScope);
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pc => await _pcRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcDeptCodes.Pur => await _mpRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcDeptCodes.Iqc => await _iqcRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcDeptCodes.Mc => await _mcRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcDeptCodes.Mfg1 => await _assyRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcDeptCodes.Qc => await _qaRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcDeptCodes.Me => await _teRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            _ => null
        };
    }

    /// <summary>
    /// 按明细解析制造二课执行行（优先按路由目标表）
    /// </summary>
    private async Task<object?> FirstPcbaEntityByDetailIdAsync(long ecDetailId)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return null;
        }
        var route = TaktEcMtlRouteHelper.Resolve(detail);
        if (route == TaktEcMtlRouteTarget.None)
        {
            return null;
        }
        var master = await GetMasterControlScopeAsync(detail);
        return await FirstSmtEntityByDetailAndTargetAsync(detail, route, master);
    }

    /// <summary>
    /// 按去重键与目标表取制造二课执行行
    /// </summary>
    private async Task<object?> FirstSmtEntityByDetailAndTargetAsync(long ecDetailId, TaktEcMtlRouteTarget target)
    {
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null)
        {
            return null;
        }
        var master = await GetMasterControlScopeAsync(detail);
        return await FirstSmtEntityByDetailAndTargetAsync(detail, target, master);
    }

    /// <summary>
    /// 按去重键与目标表取制造二课执行行
    /// </summary>
    private async Task<object?> FirstSmtEntityByDetailAndTargetAsync(
        TaktEcDetail detail,
        TaktEcMtlRouteTarget target,
        int masterControlScope)
    {
        var deptCode = target == TaktEcMtlRouteTarget.Mtl ? TaktEcDeptCodes.Mtl : TaktEcDeptCodes.Mfg2;
        var grainKey = TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, masterControlScope);
        var ecCode = detail.EcCode ?? string.Empty;
        return target switch
        {
            TaktEcMtlRouteTarget.Mtl =>
                await _smtRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            TaktEcMtlRouteTarget.Mfg2 =>
                await _pcbaRepository.FirstAsync(x => x.EcCode == ecCode && SqlFunc.ToString(x.EcGrainKey) == grainKey),
            _ => null
        };
    }

    /// <summary>
    /// 按设变单号与部门加载执行实体（含已软删行：唯一索引不含 is_deleted，再插入同一明细会撞键；禁止对十万 DetailId 做 Contains/IN）
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>执行实体列表</returns>
    private async Task<List<object>> ListEntitiesByEcCodeAndDeptAsync(string ecCode, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        var code = ecCode.Trim();
        if (deptCode == TaktEcDeptCodes.Mfg2)
        {
            var rows = new List<object>();
            rows.AddRange(await _smtRepository.GetListAsync(x => x.EcCode == code, null, true));
            rows.AddRange(await _pcbaRepository.GetListAsync(x => x.EcCode == code, null, true));
            return rows;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pc => (await _pcRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            TaktEcDeptCodes.Pur => (await _mpRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            TaktEcDeptCodes.Iqc => (await _iqcRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            TaktEcDeptCodes.Mc => (await _mcRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            TaktEcDeptCodes.Mfg1 => (await _assyRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            TaktEcDeptCodes.Qc => (await _qaRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            TaktEcDeptCodes.Me => (await _teRepository.GetListAsync(x => x.EcCode == code, null, true)).Cast<object>().ToList(),
            _ => []
        };
    }

    /// <summary>
    /// 按明细 ID 列表与部门加载执行实体（仅小批量；大批量请用 ListEntitiesByEcCodeAndDeptAsync）
    /// </summary>
    private async Task<List<object>> ListEntitiesByDetailIdsAndDeptAsync(IReadOnlyList<long> detailIds, string deptCode)
    {
        if (detailIds == null || detailIds.Count == 0)
        {
            return [];
        }
        var map = await LoadMapByDetailIdsAsync(detailIds, deptCode);
        return map.Values.Distinct().ToList();
    }

    /// <summary>
    /// 按明细 ID 列表加载这些明细所落入的全部部门执行行
    /// </summary>
    private async Task<List<object>> ListAllEntitiesByDetailIdsAsync(IReadOnlyList<long> detailIds)
    {
        var groups = await LoadGroupsAllDeptsAsync(detailIds);
        return groups.Values.SelectMany(x => x).Distinct().ToList();
    }

    /// <summary>
    /// 创建或复用部门执行实体，并从明细同步冗余字段
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="existing">已有执行实体；为空则新建</param>
    /// <param name="lineNumber">行号</param>
    /// <param name="applyNotRelatedAuto">是否按采购类型/检验写入「无关」自动完成（默认否；仅全仕向/部管由 Upsert 显式传入 true）</param>
    /// <param name="masterControlScope">技术课主表控制范围；空则按明细所属主表读取。仅新建执行行写入</param>
    private async Task<object> CreateConcreteExec(
        TaktEcDetail detail,
        string deptCode,
        object? existing,
        int lineNumber,
        bool applyNotRelatedAuto = false,
        int? masterControlScope = null)
    {
        if (deptCode == TaktEcDeptCodes.Mfg2)
        {
            var route = TaktEcMtlRouteHelper.Resolve(detail);
            if (route == TaktEcMtlRouteTarget.None)
            {
                throw new InvalidOperationException("制造二课当前明细无有效执行表路由（F 且非 C003）");
            }
            return await CreatePcbaConcreteExec(detail, route, existing, lineNumber, applyNotRelatedAuto, masterControlScope);
        }
        var isNew = existing == null;
        var exec = existing ?? deptCode switch
        {
            TaktEcDeptCodes.Pc => new TaktEcPc { DeptCode = deptCode },
            TaktEcDeptCodes.Pur => new TaktEcPur { DeptCode = deptCode },
            TaktEcDeptCodes.Iqc => new TaktEcIqc { DeptCode = deptCode },
            TaktEcDeptCodes.Mc => new TaktEcMc { DeptCode = deptCode },
            TaktEcDeptCodes.Mfg1 => new TaktEcMfg1 { DeptCode = deptCode },
            TaktEcDeptCodes.Qc => new TaktEcQc { DeptCode = deptCode },
            TaktEcDeptCodes.Me => new TaktEcMe { DeptCode = deptCode },
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };
        var controlScope = masterControlScope ?? await GetMasterControlScopeAsync(detail);
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, isNew ? lineNumber : null, controlScope);
        EnsureDeptName(exec);
        if (applyNotRelatedAuto)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        return exec;
    }

    /// <summary>
    /// 创建或复用制造二课目标表实体
    /// </summary>
    private async Task<object> CreatePcbaConcreteExec(
        TaktEcDetail detail,
        TaktEcMtlRouteTarget target,
        object? existing,
        int lineNumber,
        bool applyNotRelatedAuto,
        int? masterControlScope = null)
    {
        object exec;
        var isNew = false;
        if (target == TaktEcMtlRouteTarget.Mtl)
        {
            if (existing is TaktEcMtl electronic)
            {
                exec = electronic;
                electronic.DeptCode = TaktEcDeptCodes.Mtl;
            }
            else
            {
                isNew = true;
                exec = new TaktEcMtl { DeptCode = TaktEcDeptCodes.Mtl };
            }
        }
        else if (target == TaktEcMtlRouteTarget.Mfg2)
        {
            if (existing is TaktEcMfg2 pcba)
            {
                exec = pcba;
                pcba.DeptCode = TaktEcDeptCodes.Mfg2;
            }
            else
            {
                isNew = true;
                exec = new TaktEcMfg2 { DeptCode = TaktEcDeptCodes.Mfg2 };
            }
        }
        else
        {
            throw new InvalidOperationException("制造二课路由目标无效");
        }
        var controlScope = masterControlScope ?? await GetMasterControlScopeAsync(detail);
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, isNew ? lineNumber : null, controlScope);
        EnsureDeptName(exec);
        if (applyNotRelatedAuto)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
        return exec;
    }

    /// <summary>
    /// 从设变明细同步冗余字段到部门执行行（新建时写入行号）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="lineNumberForCreate">新建行号</param>
    /// <param name="applyNotRelatedAuto">是否按采购类型/检验写入「无关」自动完成（默认否）</param>
    private static void ApplyDetailRedundantFields(
        object exec,
        TaktEcDetail detail,
        int? lineNumberForCreate,
        bool applyNotRelatedAuto = false)
    {
        TaktEcDeptExecRedundantBinder.Apply(exec, detail, lineNumberForCreate, TaktEcControlScopeConstants.ByModel);
        EnsureDeptName(exec);
        if (applyNotRelatedAuto)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
    }

    /// <summary>
    /// 按 DeptCode 解析设变侧部门名称（优先课别显示名 GetDisplayName；未知码再取 TaktDept.DeptName1）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>部门名称</returns>
    private async Task<string> ResolveDeptNameAsync(string deptCode)
    {
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return string.Empty;
        }
        var code = deptCode.Trim();
        var ecDisplayName = TaktEcDeptCodes.GetDisplayName(code);
        // 设变映射名可与组织短名不同（如 D0625：组织=物料，设变=制造2课-物料）
        if (!string.IsNullOrWhiteSpace(ecDisplayName)
            && !string.Equals(ecDisplayName, code, StringComparison.Ordinal))
        {
            return ecDisplayName;
        }
        var dept = await _deptRepository.FirstAsync(x => x.DeptCode == code);
        if (dept != null && !string.IsNullOrWhiteSpace(dept.DeptName1))
        {
            return dept.DeptName1.Trim();
        }
        return ecDisplayName;
    }

    /// <summary>
    /// 回填部门名称；preferred 非空则强制写入，否则空值时用课别显示名兜底
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="preferredName">已解析的部门名称（批量路径一次解析后传入）</param>
    private static void EnsureDeptName(object exec, string? preferredName = null)
    {
        if (exec is not ITaktEcDeptExecEntity entity)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(preferredName))
        {
            entity.DeptName = preferredName.Trim();
            return;
        }
        if (string.IsNullOrWhiteSpace(entity.DeptName))
        {
            entity.DeptName = TaktEcDeptCodes.GetDisplayName(entity.DeptCode);
        }
    }

    /// <summary>
    /// 按设变单号+机种编码汇总部管或 SMT 可见明细的根物料编码组
    /// </summary>
    /// <param name="ecCode">设变单号</param>
    /// <param name="ecModelCode">机种编码</param>
    /// <param name="forSmt">true=SMT（F+C003）；false=部管（F 且非 C003）</param>
    /// <returns>分号分隔的根物料编码组</returns>
    private async Task<string> LoadEcModelRootMaterialCodeGroupAsync(string? ecCode, string? ecModelCode, bool forSmt)
    {
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            return string.Empty;
        }
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var model = ecModelCode ?? string.Empty;
        var siblings = forSmt
            ? await _ecDetailRepository.GetListAsync(x =>
                x.EcCode == ecCode
                && x.EcModelCode == model
                && x.EcNewPurchaseType == purchaseTypeF
                && x.EcNewWarehouse != null
                && x.EcNewWarehouse == warehouseC003
                && x.IsObsolete == 0)
            : await _ecDetailRepository.GetListAsync(x =>
                x.EcCode == ecCode
                && x.EcModelCode == model
                && x.EcNewPurchaseType == purchaseTypeF
                && (x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003)
                && x.IsObsolete == 0);
        return TaktEcRootMaterialCodeGroup.Join(siblings.Select(x => x.EcRootMaterialCode));
    }

    /// <summary>
    /// 保存执行实体到对应部门表，并回写明细子表外键 EcXxxId（一对多）
    /// </summary>
    private async Task<object> SaveEntityAsync(object exec, string deptCode, bool isNew)
    {
        if (deptCode == TaktEcDeptCodes.Mfg2 || deptCode == TaktEcDeptCodes.Mtl)
        {
            return await SavePcbaEntityAsync(exec, isNew);
        }
        var saved = deptCode switch
        {
            TaktEcDeptCodes.Pc => (object)await SaveTypedAsync(_pcRepository, (TaktEcPc)exec, isNew),
            TaktEcDeptCodes.Pur => await SaveTypedAsync(_mpRepository, (TaktEcPur)exec, isNew),
            TaktEcDeptCodes.Iqc => await SaveTypedAsync(_iqcRepository, (TaktEcIqc)exec, isNew),
            TaktEcDeptCodes.Mc => await SaveTypedAsync(_mcRepository, (TaktEcMc)exec, isNew),
            TaktEcDeptCodes.Mfg1 => await SaveTypedAsync(_assyRepository, (TaktEcMfg1)exec, isNew),
            TaktEcDeptCodes.Qc => await SaveTypedAsync(_qaRepository, (TaktEcQc)exec, isNew),
            TaktEcDeptCodes.Me => await SaveTypedAsync(_teRepository, (TaktEcMe)exec, isNew),
            _ => throw new InvalidOperationException($"不支持的部门编码：{deptCode}")
        };
        return saved;
    }

    /// <summary>
    /// 保存制造二课执行实体
    /// </summary>
    private async Task<object> SavePcbaEntityAsync(object exec, bool isNew)
    {
        var saved = exec switch
        {
            TaktEcMtl electronic => (object)await SaveTypedAsync(_smtRepository, electronic, isNew),
            TaktEcMfg2 pcba => await SaveTypedAsync(_pcbaRepository, pcba, isNew),
            _ => throw new InvalidOperationException("制造二课执行实体类型不匹配")
        };
        return saved;
    }

    /// <summary>
    /// 填充执行 DTO 视图子表明细（按业务键 + IsObsolete==0；含各部门可见性过滤）
    /// </summary>
    /// <param name="savedExec">执行实体</param>
    /// <param name="dto">对应响应 DTO</param>
    public async Task FillExecViewDetailsAsync(object savedExec, object dto)
    {
        ArgumentNullException.ThrowIfNull(savedExec);
        ArgumentNullException.ThrowIfNull(dto);
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        switch (savedExec)
        {
            case TaktEcPc e when dto is TaktEcPcDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode)
                        && TaktEcRootMaterialCodeGroup.ContainsCode(e.EcRootMaterialCodeGroup, x.EcRootMaterialCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcPur e when dto is TaktEcPurDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewMaterialCode == e.EcNewMaterialCode
                        && x.EcNewPurchaseType == purchaseTypeF
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcIqc e when dto is TaktEcIqcDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewMaterialCode == e.EcNewMaterialCode
                        && x.EcNewRequiresInspection == 1
                        && x.IsObsolete == 0))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcMc e when dto is TaktEcMcDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewPurchaseType == purchaseTypeF
                        && (x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003)
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcMtl e when dto is TaktEcMtlDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewPurchaseType == purchaseTypeF
                        && x.EcNewWarehouse != null
                        && x.EcNewWarehouse == warehouseC003
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcMfg2 e when dto is TaktEcMfg2Dto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.EcNewPurchaseType != purchaseTypeF
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode)
                        && TaktEcRootMaterialCodeGroup.ContainsCode(e.EcRootMaterialCodeGroup, x.EcRootMaterialCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcMfg1 e when dto is TaktEcMfg1Dto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode)
                        && TaktEcRootMaterialCodeGroup.ContainsCode(e.EcRootMaterialCodeGroup, x.EcRootMaterialCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcQc e when dto is TaktEcQcDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode)
                        && TaktEcRootMaterialCodeGroup.ContainsCode(e.EcRootMaterialCodeGroup, x.EcRootMaterialCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
            case TaktEcMe e when dto is TaktEcMeDto d:
                d.EcDetails = (await _ecDetailRepository.GetListAsync(x =>
                        x.EcCode == e.EcCode
                        && x.IsObsolete == 0))
                    .Where(x => TaktEcRootMaterialCodeGroup.ContainsCode(e.EcModelCodeGroup, x.EcModelCode)
                        && TaktEcRootMaterialCodeGroup.ContainsCode(e.EcRootMaterialCodeGroup, x.EcRootMaterialCode))
                    .Adapt<List<TaktEcDetailDto>>();
                break;
        }
    }

    /// <summary>
    /// 为设变明细初始化全部责任部门执行行（KanbanOrder 共 8 课，每明细×部门一行；已存在则跳过）
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>新建部门行数</returns>
    public async Task<int> EnsureAllDeptExecRowsForDetailAsync(TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (detail.Id <= 0)
        {
            throw new ArgumentException("设变明细 ID 无效", nameof(detail));
        }
        var created = 0;
        foreach (var deptCode in TaktEcDeptCodes.KanbanOrder)
        {
            if (deptCode == TaktEcDeptCodes.Mfg2)
            {
                created += await EnsurePcbaDeptExecRowAsync(detail);
                continue;
            }
            if (deptCode == TaktEcDeptCodes.Mtl)
            {
                continue;
            }
            if (await ShouldSkipDeptExecByScopeGrainAsync(detail, deptCode))
            {
                await ObsoleteDeptExecIfExistsAsync(detail.Id, deptCode);
                continue;
            }
            var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, deptCode);
            if (existing != null)
            {
                ApplyDetailRedundantFields(existing, detail, null, applyNotRelatedAuto: false);
                if (existing is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
                {
                    deptExec.IsObsolete = 0;
                }
                await SaveEntityAsync(existing, deptCode, false);
                continue;
            }
            var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
            var exec = await CreateConcreteExec(detail, deptCode, null, lineNumber, applyNotRelatedAuto: false);
            await SaveEntityAsync(exec, deptCode, true);
            created += 1;
        }
        await _ecTeStatusSynchronizer.RefreshByEcCodeAsync(detail.EcCode);
        return created;
    }

    /// <summary>
    /// 制造二课 Ensure：按路由作废对侧并创建/同步目标表
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>新建行数（0 或 1）</returns>
    private async Task<int> EnsurePcbaDeptExecRowAsync(TaktEcDetail detail)
    {
        var route = TaktEcMtlRouteHelper.Resolve(detail);
        if (route == TaktEcMtlRouteTarget.None)
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        if (route == TaktEcMtlRouteTarget.Mtl
            && !TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        if (route == TaktEcMtlRouteTarget.Mfg2
            && await HasNewerPcbaVisibleSiblingAsync(detail))
        {
            await ObsoletePcbaBothTablesAsync(detail.Id);
            return 0;
        }
        await ObsoleteTypedDeptExecIfExistsAsync(
            detail.Id,
            route == TaktEcMtlRouteTarget.Mtl
                ? TaktEcMtlRouteTarget.Mfg2
                : TaktEcMtlRouteTarget.Mtl);
        var existingPcba = await FirstSmtEntityByDetailAndTargetAsync(detail.Id, route);
        if (existingPcba != null)
        {
            ApplyDetailRedundantFields(existingPcba, detail, null, applyNotRelatedAuto: false);
            if (existingPcba is ITaktEcDeptExecEntity existingDept && existingDept.IsObsolete == 1)
            {
                existingDept.IsObsolete = 0;
            }
            await SavePcbaEntityAsync(existingPcba, false);
            return 0;
        }
        var pcbaLine = detail.LineNumber > 0 ? detail.LineNumber : 10;
        var pcbaExec = await CreatePcbaConcreteExec(detail, route, null, pcbaLine, applyNotRelatedAuto: false);
        await SavePcbaEntityAsync(pcbaExec, true);
        return 1;
    }

    /// <summary>
    /// 批量为设变明细初始化全部责任部门执行行
    /// </summary>
    /// <param name="details">设变明细列表</param>
    /// <returns>任务</returns>
    public async Task EnsureAllDeptExecRowsForDetailsAsync(IReadOnlyList<TaktEcDetail> details)
    {
        if (details == null || details.Count == 0)
        {
            return;
        }
        foreach (var detail in details)
        {
            await EnsureAllDeptExecRowsForDetailAsync(detail);
        }
    }

    /// <summary>
    /// 按指定部门编码顺序为每条设变明细初始化部门执行行（已存在则跳过）
    /// </summary>
    /// <param name="details">设变明细列表</param>
    /// <param name="deptCodesInOrder">部门编码顺序</param>
    /// <returns>任务</returns>
    public async Task EnsureDeptExecRowsForDetailsInOrderAsync(
        IReadOnlyList<TaktEcDetail> details,
        IReadOnlyList<string> deptCodesInOrder)
    {
        if (details == null || details.Count == 0 || deptCodesInOrder == null || deptCodesInOrder.Count == 0)
        {
            return;
        }
        foreach (var detail in details)
        {
            ArgumentNullException.ThrowIfNull(detail);
            if (detail.Id <= 0)
            {
                throw new ArgumentException("设变明细 ID 无效", nameof(details));
            }
            foreach (var deptCode in deptCodesInOrder)
            {
                if (string.IsNullOrWhiteSpace(deptCode))
                {
                    continue;
                }
                var normalizedDeptCode = deptCode.Trim();
                if (normalizedDeptCode == TaktEcDeptCodes.Mfg2)
                {
                    await EnsurePcbaDeptExecRowAsync(detail);
                    continue;
                }
                if (normalizedDeptCode == TaktEcDeptCodes.Mtl)
                {
                    continue;
                }
                if (await ShouldSkipDeptExecByScopeGrainAsync(detail, normalizedDeptCode))
                {
                    await ObsoleteDeptExecIfExistsAsync(detail.Id, normalizedDeptCode);
                    continue;
                }
                var existing = await FirstEntityByDetailAndDeptAsync(detail.Id, normalizedDeptCode);
                if (existing != null)
                {
                    ApplyDetailRedundantFields(existing, detail, null, applyNotRelatedAuto: false);
                    if (existing is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
                    {
                        deptExec.IsObsolete = 0;
                    }
                    await SaveEntityAsync(existing, normalizedDeptCode, false);
                    continue;
                }
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = await CreateConcreteExec(detail, normalizedDeptCode, null, lineNumber, applyNotRelatedAuto: false);
                await SaveEntityAsync(exec, normalizedDeptCode, true);
            }
        }
        await _ecTeStatusSynchronizer.RefreshByEcCodesAsync(details.Select(x => x.EcCode));
    }

    /// <summary>
    /// 泛型保存（落库前把历史短文案规范为「实施范围-…」）
    /// </summary>
    private static async Task<TEntity> SaveTypedAsync<TEntity>(ITaktCompanyRepository<TEntity> repository, TEntity entity, bool isNew)
        where TEntity : TaktCompanyEntityBase, new()
    {
        TaktEcDeptEntityHelper.SetExecContent(entity, TaktEcDeptEntityHelper.GetExecContent(entity), overwrite: true);
        if (isNew)
        {
            return await repository.CreateAsync(entity);
        }
        await repository.UpdateAsync(entity);
        return entity;
    }
}