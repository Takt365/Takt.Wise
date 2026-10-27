// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecPersistence.Batch.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行批量派生（预加载 + CreateRange/UpdateRange，避免逐行 N+1）
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
/// 设变部门执行聚合持久化（批量派生扩展）
/// </summary>
public partial class TaktEcExecPersistence
{
    /// <summary>
    /// 批量派生单部门执行行结果
    /// </summary>
    public readonly record struct TaktEcDeptExecBatchResult(int SavedCount, int SkippedCount);

    /// <summary>
    /// 按明细列表批量 Upsert 单部门执行行（编排层专用；禁止再逐行查库/回写）
    /// </summary>
    /// <param name="details">同一设变下未作废明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="shouldAutoComplete">是否自动填完（按明细）</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>写入/跳过条数</returns>
    public async Task<TaktEcDeptExecBatchResult> UpsertDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecImplScope)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        ArgumentNullException.ThrowIfNull(shouldAutoComplete);
        if (details.Count == 0)
        {
            return new TaktEcDeptExecBatchResult(0, 0);
        }
        if (deptCode == TaktEcDeptCodes.Mfg2 || deptCode == TaktEcDeptCodes.Mtl)
        {
            return await UpsertPcbaDeptExecBatchWithFillModeAsync(details, shouldAutoComplete, ecImplScope);
        }
        return await UpsertTypedDeptExecBatchWithFillModeAsync(details, deptCode, shouldAutoComplete, ecImplScope);
    }

    /// <summary>
    /// 非制造二课：单表批量 Upsert
    /// </summary>
    private async Task<TaktEcDeptExecBatchResult> UpsertTypedDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecImplScope)
    {
        var ecCode = details
            .Select(x => x.EcCode?.Trim())
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? string.Empty;
        var existingList = !string.IsNullOrWhiteSpace(ecCode)
            ? await ListEntitiesByEcCodeAndDeptAsync(ecCode, deptCode)
            : await ListEntitiesByDetailIdsAndDeptAsync(details.Select(x => x.Id).ToList(), deptCode);
        var masterControlScope = await GetMasterControlScopeAsync(details[0]);
        var existingByGrain = existingList
            .OfType<ITaktEcDeptExecEntity>()
            .GroupBy(x => x.EcGrainKey)
            .ToDictionary(g => g.Key, g => (object)g.First());
        var rootPresence = await LoadRootMaterialPresenceAsync(details);
        var derivable = SelectKnownRootDetails(details, rootPresence);
        var ecControlScope = TaktEcControlScopeConstants.ResolveForDept(deptCode, ecImplScope, masterControlScope);
        var dedupKind = TaktEcScopeRowGrain.ResolveKind(deptCode, ecControlScope);
        var fertRootKeys = deptCode == TaktEcDeptCodes.Mfg1
            && ecControlScope == TaktEcControlScopeConstants.ByFinishedProduct
            ? CollectFertRootMaterialKeys(derivable)
            : null;
        var skipByDedup = TaktEcScopeRowGrain.BuildSkipIds(derivable, deptCode, ecControlScope, fertRootKeys);
        var rootGroups = TaktEcScopeRowGrain.BuildRootGroups(derivable, deptCode, ecControlScope, fertRootKeys);
        var modelGroups = TaktEcScopeRowGrain.BuildModelGroups(derivable, deptCode, ecControlScope, fertRootKeys);
        var newMaterialGroups = deptCode == TaktEcDeptCodes.Mfg2
            ? TaktEcScopeRowGrain.BuildCodeGroups(derivable, deptCode, ecControlScope, fertRootKeys, detail => detail.EcNewMaterialCode)
            : null;
        var parentMaterialGroups = deptCode == TaktEcDeptCodes.Mfg2
            ? TaktEcScopeRowGrain.BuildCodeGroups(derivable, deptCode, ecControlScope, fertRootKeys, detail => detail.EcParentMaterialCode)
            : null;
        if (skipByDedup.Count > 0)
        {
            TaktLogger.Information(
                "[EcTePersist] 部门执行去重 DeptCode={DeptCode} DetailCount={DetailCount} SkipByDedup={SkipCount}",
                deptCode,
                details.Count,
                skipByDedup.Count);
        }
        var applyNotRelatedAuto = TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope);
        var deptName = await ResolveDeptNameAsync(deptCode);
        var toCreate = new List<object>();
        var toUpdate = new List<object>();
        var savedCount = 0;
        var skippedCount = 0;
        foreach (var detail in details)
        {
            var grainKey = TaktEcScopeRowGrain.BuildExecGrainKey(detail, deptCode, masterControlScope);
            existingByGrain.TryGetValue(grainKey, out var existing);
            if (!rootPresence.KnownKeys.Contains(BuildRootMaterialKey(detail)))
            {
                if (TryMarkObsolete(existing))
                {
                    toUpdate.Add(existing!);
                }
                skippedCount++;
                continue;
            }
            if (!TaktEcScopeRowGrain.IsEligible(detail, deptCode, ecImplScope, fertRootKeys))
            {
                if (TryMarkObsolete(existing))
                {
                    toUpdate.Add(existing!);
                }
                skippedCount++;
                continue;
            }
            if (skipByDedup.Contains(detail.Id))
            {
                if (TryMarkObsolete(existing))
                {
                    toUpdate.Add(existing!);
                }
                skippedCount++;
                continue;
            }
            var isNew = existing == null;
            var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
            var exec = await CreateConcreteExec(detail, deptCode, existing, lineNumber, applyNotRelatedAuto: false, masterControlScope);
            var groupKey = TaktEcScopeRowGrain.BuildKey(detail, dedupKind, deptCode);
            if (rootGroups.TryGetValue(groupKey, out var rootGroup))
            {
                AssignRootMaterialCodeGroup(exec, rootGroup);
            }
            if (modelGroups.TryGetValue(groupKey, out var modelGroup))
            {
                AssignModelCodeGroup(exec, modelGroup);
            }
            if (newMaterialGroups != null
                && parentMaterialGroups != null
                && newMaterialGroups.TryGetValue(groupKey, out var newMaterialGroup)
                && parentMaterialGroups.TryGetValue(groupKey, out var parentMaterialGroup))
            {
                AssignPcbaMaterialGroups(exec, newMaterialGroup, parentMaterialGroup);
            }
            EnsureDeptName(exec, deptName);
            if (exec is ITaktEcDeptExecEntity deptExec && deptExec.IsObsolete == 1)
            {
                deptExec.IsObsolete = 0;
            }
            if (exec is TaktCompanyEntityBase persisted && persisted.IsDeleted != 0)
            {
                persisted.IsDeleted = 0;
                persisted.DeletedBy = null;
                persisted.DeletedAt = null;
            }
            SetDiscontinuedStatusIfPresent(
                exec,
                TaktEcScopeRowGrain.ResolveRootGroupDiscontinuedStatus(
                    derivable,
                    detail,
                    deptCode,
                    ecControlScope,
                    fertRootKeys));
            ApplyScopeFillMode(exec, isNew, shouldAutoComplete(detail), ecImplScope);
            var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
            var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
            if (!isEolFilled && applyNotRelatedAuto)
            {
                TaktEcExecNotRelated.TryAfterFill(exec, detail);
            }
            var autoCloseContent = ResolveAutoCloseExecContent(
                derivable,
                detail,
                deptCode,
                ecImplScope,
                ecControlScope,
                fertRootKeys);
            if (autoCloseContent != null)
            {
                ApplyAutoCloseDeptExec(exec, isNew, autoCloseContent);
            }
            ApplyMaterialTypeDeptControlled(exec, derivable, detail, deptCode, ecControlScope, fertRootKeys);
            NormalizeExecContent(exec);
            if (isNew)
            {
                toCreate.Add(exec);
            }
            else
            {
                toUpdate.Add(exec);
            }
            savedCount++;
        }
        await DeleteNonKeeperRowsForDeptAsync(deptCode, existingList, toCreate, toUpdate);
        await SaveTypedDeptRangeAsync(deptCode, toCreate, toUpdate);
        TaktLogger.Information(
            "[EcTePersist] 部门执行已落库 DeptCode={DeptCode} Saved={Saved} Skipped={Skipped}",
            deptCode,
            savedCount,
            skippedCount);
        return new TaktEcDeptExecBatchResult(savedCount, skippedCount);
    }

    /// <summary>
    /// 制造二课双表批量 Upsert
    /// </summary>
    private async Task<TaktEcDeptExecBatchResult> UpsertPcbaDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecImplScope)
    {
        var ecCode = details
            .Select(x => x.EcCode?.Trim())
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? string.Empty;
        Dictionary<string, TaktEcMtl> smtExisting;
        Dictionary<string, TaktEcMfg2> pcbaExisting;
        if (!string.IsNullOrWhiteSpace(ecCode))
        {
            smtExisting = (await _smtRepository.GetListAsync(x => x.EcCode == ecCode, null, true))
                .GroupBy(x => x.EcGrainKey)
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.IsDeleted).First());
            pcbaExisting = (await _pcbaRepository.GetListAsync(x => x.EcCode == ecCode, null, true))
                .GroupBy(x => x.EcGrainKey)
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.IsDeleted).First());
        }
        else
        {
            smtExisting = new Dictionary<string, TaktEcMtl>();
            pcbaExisting = new Dictionary<string, TaktEcMfg2>();
        }
        var rootPresence = await LoadRootMaterialPresenceAsync(details);
        var derivable = SelectKnownRootDetails(details, rootPresence);
        var masterDeptScope = await GetMasterControlScopeAsync(details[0]);
        var smtDeptScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Mtl, ecImplScope, masterDeptScope);
        var nikaDeptScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Mfg2, ecImplScope, masterDeptScope);
        var smtKind = TaktEcScopeRowGrain.ResolveKind(TaktEcDeptCodes.Mtl, smtDeptScope);
        var nikaKind = TaktEcScopeRowGrain.ResolveKind(TaktEcDeptCodes.Mfg2, nikaDeptScope);
        var skipSmt = TaktEcScopeRowGrain.BuildSkipIds(derivable, TaktEcDeptCodes.Mtl, smtDeptScope, null);
        var skipPcba = TaktEcScopeRowGrain.BuildSkipIds(derivable, TaktEcDeptCodes.Mfg2, nikaDeptScope, null);
        var smtRootGroups = TaktEcScopeRowGrain.BuildRootGroups(derivable, TaktEcDeptCodes.Mtl, smtDeptScope, null);
        var nikaRootGroups = TaktEcScopeRowGrain.BuildRootGroups(derivable, TaktEcDeptCodes.Mfg2, nikaDeptScope, null);
        var nikaModelGroups = TaktEcScopeRowGrain.BuildModelGroups(derivable, TaktEcDeptCodes.Mfg2, nikaDeptScope, null);
        _ = shouldAutoComplete;
        var applyNotRelatedAuto = TaktEcScopeConstants.AllowsNotRelatedFill(ecImplScope);
        var smtDeptName = await ResolveDeptNameAsync(TaktEcDeptCodes.Mtl);
        var pcbaDeptName = await ResolveDeptNameAsync(TaktEcDeptCodes.Mfg2);
        var smtCreate = new List<TaktEcMtl>();
        var smtUpdate = new List<TaktEcMtl>();
        var pcbaCreate = new List<TaktEcMfg2>();
        var pcbaUpdate = new List<TaktEcMfg2>();
        var savedCount = 0;
        var skippedCount = 0;
        foreach (var detail in details)
        {
            var smtGrain = TaktEcScopeRowGrain.BuildExecGrainKey(detail, TaktEcDeptCodes.Mtl, masterDeptScope);
            var nikaGrain = TaktEcScopeRowGrain.BuildExecGrainKey(detail, TaktEcDeptCodes.Mfg2, masterDeptScope);
            smtExisting.TryGetValue(smtGrain, out var smtRow);
            pcbaExisting.TryGetValue(nikaGrain, out var pcbaRow);
            if (!rootPresence.KnownKeys.Contains(BuildRootMaterialKey(detail)))
            {
                if (TryMarkObsolete(smtRow))
                {
                    smtUpdate.Add(smtRow!);
                }
                if (TryMarkObsolete(pcbaRow))
                {
                    pcbaUpdate.Add(pcbaRow!);
                }
                skippedCount++;
                continue;
            }
            var smtKeep = TaktEcScopeRowGrain.IsEligible(detail, TaktEcDeptCodes.Mtl, ecImplScope, null)
                && !skipSmt.Contains(detail.Id);
            var nikaKeep = TaktEcScopeRowGrain.IsEligible(detail, TaktEcDeptCodes.Mfg2, ecImplScope, null)
                && !skipPcba.Contains(detail.Id);
            if (!smtKeep)
            {
                if (TryMarkObsolete(smtRow))
                {
                    smtUpdate.Add(smtRow!);
                }
            }
            else
            {
                var isNew = smtRow == null;
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = (TaktEcMtl)await CreatePcbaConcreteExec(
                    detail,
                    TaktEcMtlRouteTarget.Mtl,
                    smtRow,
                    lineNumber,
                    applyNotRelatedAuto: false,
                    masterDeptScope);
                var smtGroupKey = TaktEcScopeRowGrain.BuildKey(detail, smtKind);
                if (smtRootGroups.TryGetValue(smtGroupKey, out var smtRootGroup))
                {
                    exec.EcRootMaterialCodeGroup = smtRootGroup;
                }
                EnsureDeptName(exec, smtDeptName);
                if (exec.IsObsolete == 1)
                {
                    exec.IsObsolete = 0;
                }
                if (exec.IsDeleted != 0)
                {
                    exec.IsDeleted = 0;
                    exec.DeletedBy = null;
                    exec.DeletedAt = null;
                }
                var smtGroupLive = TaktEcScopeRowGrain.RootGroupHasInProduction(
                    derivable,
                    detail,
                    TaktEcDeptCodes.Mtl,
                    smtDeptScope,
                    null);
                SetDiscontinuedStatusIfPresent(
                    exec,
                    smtGroupLive
                        ? TaktEcScopeConstants.PlannedMaterialStatus
                        : detail.DiscontinuedStatus?.Trim() ?? string.Empty);
                ApplyScopeFillMode(
                    exec,
                    isNew,
                    TaktEcScopeConstants.ShouldAutoCompleteExec(ecImplScope, TaktEcDeptCodes.Mtl, detail.EcNewPurchaseType, detail.EcNewWarehouse),
                    ecImplScope);
                ApplyPostFillNotRelated(exec, detail, applyNotRelatedAuto);
                var smtAutoCloseContent = ResolveAutoCloseExecContent(
                    derivable,
                    detail,
                    TaktEcDeptCodes.Mtl,
                    ecImplScope,
                    smtDeptScope,
                    null);
                if (smtAutoCloseContent != null)
                {
                    ApplyAutoCloseDeptExec(exec, isNew, smtAutoCloseContent);
                }
                NormalizeExecContent(exec);
                if (isNew)
                {
                    smtCreate.Add(exec);
                }
                else
                {
                    smtUpdate.Add(exec);
                }
                savedCount++;
            }
            if (!nikaKeep)
            {
                if (TryMarkObsolete(pcbaRow))
                {
                    pcbaUpdate.Add(pcbaRow!);
                }
            }
            else
            {
                var isNew = pcbaRow == null;
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = (TaktEcMfg2)await CreatePcbaConcreteExec(
                    detail,
                    TaktEcMtlRouteTarget.Mfg2,
                    pcbaRow,
                    lineNumber,
                    applyNotRelatedAuto: false,
                    masterDeptScope);
                var nikaGroupKey = TaktEcScopeRowGrain.BuildKey(detail, nikaKind, TaktEcDeptCodes.Mfg2);
                if (nikaRootGroups.TryGetValue(nikaGroupKey, out var nikaRootGroup))
                {
                    exec.EcRootMaterialCodeGroup = nikaRootGroup;
                }
                if (nikaModelGroups.TryGetValue(nikaGroupKey, out var nikaModelGroup))
                {
                    exec.EcModelCodeGroup = nikaModelGroup;
                }
                EnsureDeptName(exec, pcbaDeptName);
                if (exec.IsObsolete == 1)
                {
                    exec.IsObsolete = 0;
                }
                if (exec.IsDeleted != 0)
                {
                    exec.IsDeleted = 0;
                    exec.DeletedBy = null;
                    exec.DeletedAt = null;
                }
                var nikaGroupLive = TaktEcScopeRowGrain.RootGroupHasInProduction(
                    derivable,
                    detail,
                    TaktEcDeptCodes.Mfg2,
                    nikaDeptScope,
                    null);
                SetDiscontinuedStatusIfPresent(
                    exec,
                    nikaGroupLive
                        ? TaktEcScopeConstants.PlannedMaterialStatus
                        : detail.DiscontinuedStatus?.Trim() ?? string.Empty);
                ApplyScopeFillMode(
                    exec,
                    isNew,
                    TaktEcScopeConstants.ShouldAutoCompleteExec(ecImplScope, TaktEcDeptCodes.Mfg2, detail.EcNewPurchaseType, detail.EcNewWarehouse),
                    ecImplScope);
                ApplyPostFillNotRelated(exec, detail, applyNotRelatedAuto);
                var nikaAutoCloseContent = ResolveAutoCloseExecContent(
                    derivable,
                    detail,
                    TaktEcDeptCodes.Mfg2,
                    ecImplScope,
                    nikaDeptScope,
                    null);
                if (nikaAutoCloseContent != null)
                {
                    ApplyAutoCloseDeptExec(exec, isNew, nikaAutoCloseContent);
                }
                ApplyMaterialTypeDeptControlled(exec, derivable, detail, TaktEcDeptCodes.Mfg2, nikaDeptScope, null);
                NormalizeExecContent(exec);
                if (isNew)
                {
                    pcbaCreate.Add(exec);
                }
                else
                {
                    pcbaUpdate.Add(exec);
                }
                savedCount++;
            }
            if (!smtKeep && !nikaKeep)
            {
                skippedCount++;
            }
        }
        var smtKeeperDetailIds = smtCreate
            .Concat(smtUpdate)
            .Where(x => x.IsObsolete == 0)
            .Select(x => x.EcGrainKey)
            .ToHashSet();
        var smtDeleteIds = smtExisting.Values
            .Where(x => x.Id > 0 && !smtKeeperDetailIds.Contains(x.EcGrainKey))
            .Select(x => x.Id)
            .Distinct()
            .ToList();
        if (smtDeleteIds.Count > 0)
        {
            await _smtRepository.DeleteAsync(x => smtDeleteIds.Contains(x.Id));
            smtUpdate.RemoveAll(x => smtDeleteIds.Contains(x.Id));
        }
        var nikaKeeperDetailIds = pcbaCreate
            .Concat(pcbaUpdate)
            .Where(x => x.IsObsolete == 0)
            .Select(x => x.EcGrainKey)
            .ToHashSet();
        var nikaDeleteIds = pcbaExisting.Values
            .Where(x => x.Id > 0 && !nikaKeeperDetailIds.Contains(x.EcGrainKey))
            .Select(x => x.Id)
            .Distinct()
            .ToList();
        if (nikaDeleteIds.Count > 0)
        {
            await _pcbaRepository.DeleteAsync(x => nikaDeleteIds.Contains(x.Id));
            pcbaUpdate.RemoveAll(x => nikaDeleteIds.Contains(x.Id));
        }
        if (smtCreate.Count > 0)
        {
            await _smtRepository.CreateRangeAsync(smtCreate);
        }
        if (smtUpdate.Count > 0)
        {
            await _smtRepository.UpdateRangeAsync(smtUpdate);
        }
        if (pcbaCreate.Count > 0)
        {
            await _pcbaRepository.CreateRangeAsync(pcbaCreate);
        }
        if (pcbaUpdate.Count > 0)
        {
            await _pcbaRepository.UpdateRangeAsync(pcbaUpdate);
        }
        return new TaktEcDeptExecBatchResult(savedCount, skippedCount);
    }

    /// <summary>
    /// 填充后「无关」补齐（与单行 Upsert 一致）
    /// </summary>
    private static void ApplyPostFillNotRelated(
        object exec,
        TaktEcDetail detail,
        bool applyNotRelatedAuto)
    {
        if (!applyNotRelatedAuto)
        {
            return;
        }
        var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
        var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
        if (!isEolFilled)
        {
            TaktEcExecNotRelated.TryAfterFill(exec, detail);
        }
    }

    /// <summary>
    /// 规范化执行内容字段
    /// </summary>
    private static void NormalizeExecContent(object entity)
    {
        TaktEcDeptEntityHelper.SetExecContent(
            entity,
            TaktEcDeptEntityHelper.GetExecContent(entity),
            overwrite: true);
    }

    /// <summary>
    /// 停产与非 FERT 的冒号文案只写全仕向、部管里仍须人工实施的部门。内部、技术，以及部管下制二/制一/品管，保留实施范围自动文案。
    /// 生管/制一/制二/品管按机种、按设变：停产行写 EOL，在产非 FERT 行写 HALB、ROH 或 HALB/ROH，二者都不并入在产成品的机种组与根物料组。
    /// 按成品：当前成品停产写 EOL，否则非 FERT 写该类型。采购/受检/部管/物料始终按设变。
    /// </summary>
    /// <param name="details">同一设变可派生明细</param>
    /// <param name="detail">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <param name="ecControlScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>自动执行内容；不自动填写时为 null</returns>
    private static string? ResolveAutoCloseExecContent(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail detail,
        string deptCode,
        int ecImplScope,
        int ecControlScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        if (ecImplScope != TaktEcScopeConstants.AllDestination
            && ecImplScope != TaktEcScopeConstants.MaterialControl)
        {
            return null;
        }
        if (TaktEcScopeConstants.ShouldAutoCompleteExec(ecImplScope, deptCode))
        {
            return null;
        }
        if (ecControlScope == TaktEcControlScopeConstants.ByFinishedProduct
            && deptCode is TaktEcDeptCodes.Pc or TaktEcDeptCodes.Mfg2 or TaktEcDeptCodes.Mfg1 or TaktEcDeptCodes.Qc)
        {
            var finishedSuffix = TaktEcScopeRowGrain.FinishedProductExecSuffix(detail);
            return string.IsNullOrEmpty(finishedSuffix)
                ? null
                : TaktEcScopeConstants.FormatScopeDeptExecContent(ecImplScope, ecControlScope, finishedSuffix);
        }
        if (TaktEcScopeRowGrain.RootGroupAllDiscontinued(details, detail, deptCode, ecControlScope, fertRootKeys))
        {
            return TaktEcScopeConstants.FormatScopeDeptExecContent(ecImplScope, ecControlScope, TaktEcScopeConstants.EolExecContent);
        }
        var materialTypeSuffix = TaktEcScopeRowGrain.RootGroupMaterialTypeSuffix(
            details,
            detail,
            deptCode,
            ecControlScope,
            fertRootKeys);
        return string.IsNullOrEmpty(materialTypeSuffix)
            ? null
            : TaktEcScopeConstants.FormatScopeDeptExecContent(ecImplScope, ecControlScope, materialTypeSuffix);
    }

    /// <summary>
    /// 生管、制一、制二、品管：根物料类型不是 FERT 时，控制范围写成不管理。FERT 保留已解析的按成品、按机种或按设变。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="details">同一设变可派生明细</param>
    /// <param name="detail">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecControlScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    private static void ApplyMaterialTypeDeptControlled(
        object exec,
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail detail,
        string deptCode,
        int ecControlScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        if (exec is not ITaktEcDeptExecEntity row)
        {
            return;
        }
        if (deptCode is not (TaktEcDeptCodes.Pc or TaktEcDeptCodes.Mfg2 or TaktEcDeptCodes.Mfg1 or TaktEcDeptCodes.Qc))
        {
            return;
        }
        if (!IsNonFertDeptRow(details, detail, deptCode, ecControlScope, fertRootKeys))
        {
            return;
        }
        row.EcControlScope = TaktEcControlScopeConstants.NotManaged;
    }

    /// <summary>
    /// 当前执行行是否含有 FERT 以外的根物料类型。按成品看本行；按机种、按设变看整组。
    /// </summary>
    /// <param name="details">同一设变可派生明细</param>
    /// <param name="detail">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecControlScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>非 FERT 时为 true</returns>
    private static bool IsNonFertDeptRow(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail detail,
        string deptCode,
        int ecControlScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        if (ecControlScope == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            var materialType = detail.EcRootMaterialType?.Trim() ?? string.Empty;
            return materialType.Length > 0
                && !string.Equals(materialType, TaktEcMfg1Constants.FertMaterialType, StringComparison.OrdinalIgnoreCase);
        }
        return TaktEcScopeRowGrain.RootGroupHasNonFert(details, detail, deptCode, ecControlScope, fertRootKeys);
    }

    /// <summary>
    /// 写入自动完成：实施、当天日期、预定批次与执行内容。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isNew">是否新建</param>
    /// <param name="marker">执行内容（EOL 或无关）</param>
    private static void ApplyAutoCloseDeptExec(object exec, bool isNew, string marker)
    {
        if (exec is TaktEcMe)
        {
            return;
        }
        var current = TaktEcDeptEntityHelper.GetExecContent(exec);
        var canOverwrite = isNew
            || TaktEcScopeConstants.IsScopeGeneratedExecContent(current);
        if (!canOverwrite)
        {
            return;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(marker);
        var today = DateTime.Today;
        TaktEcDeptEntityHelper.SetIsImplemented(exec, 1);
        TaktEcDeptEntityHelper.SetExecContent(exec, marker, overwrite: true);
        switch (exec)
        {
            case TaktEcPc row:
                row.ScheduledDate = today;
                row.ScheduledBatch = marker;
                break;
            case TaktEcMc row:
                row.ScheduledDate = today;
                row.ScheduledBatch = marker;
                break;
            case TaktEcPur row:
                row.PurchaseOrderIssueDate = today;
                break;
            case TaktEcIqc row:
                row.InspectionDate = today;
                break;
            case TaktEcMtl row:
                row.ScheduledDate = today;
                row.ScheduledBatch = marker;
                break;
            case TaktEcMfg2 row:
                row.ScheduledDate = today;
                row.ScheduledBatch = marker;
                row.ImplementationBatch = marker;
                break;
            case TaktEcMfg1 row:
                row.ScheduledDate = today;
                row.ScheduledBatch = marker;
                row.ImplementationBatch = marker;
                break;
            case TaktEcQc row:
                row.ScheduledDate = today;
                row.ScheduledBatch = marker;
                break;
        }
    }

    /// <summary>
    /// 制一课：根物料类型不是 FERT 时默认已实施，预定日期为当天，预定批次与实施批次为制一课无关。
    /// 已手填过其它批次的旧行不覆盖；已是无关文案时保留原预定日期，只补回已实施。
    /// </summary>
    /// <param name="row">制一执行行</param>
    /// <param name="isNew">是否新建</param>
    private static void ApplyAssyNonFertDefault(TaktEcMfg1 row, bool isNew)
    {
        var marker = TaktEcMfg1Constants.NotRelatedBatch;
        var scheduled = row.ScheduledBatch?.Trim() ?? string.Empty;
        var implementation = row.ImplementationBatch?.Trim() ?? string.Empty;
        var marked = string.Equals(scheduled, marker, StringComparison.Ordinal)
            && string.Equals(implementation, marker, StringComparison.Ordinal);
        var empty = scheduled.Length == 0 && implementation.Length == 0;
        if (!isNew && !empty && !marked)
        {
            return;
        }
        row.IsImplemented = 1;
        row.ScheduledBatch = marker;
        row.ImplementationBatch = marker;
        if (row.ScheduledDate == null)
        {
            row.ScheduledDate = DateTime.Today;
        }
    }

    /// <summary>
    /// 制一行是否已按非 FERT 写成制一课无关（生管冗余同步时保留）
    /// </summary>
    /// <param name="row">制一执行行</param>
    /// <returns>是否无关默认</returns>
    private static bool IsAssyNotRelatedDefault(TaktEcMfg1 row)
    {
        return TaktEcMfg1Constants.IsNotRelatedBatch(row.ScheduledBatch)
            || TaktEcMfg1Constants.IsNotRelatedBatch(row.ImplementationBatch)
            || TaktEcScopeConstants.IsDeptMaterialTypeExecContent(row.ScheduledBatch)
            || TaktEcScopeConstants.IsDeptMaterialTypeExecContent(row.ImplementationBatch)
            || TaktEcScopeConstants.IsDeptMaterialTypeExecContent(row.ExecContent);
    }

    /// <summary>
    /// 工厂 + 根物料编码键（忽略大小写比较）
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>组合键</returns>
    private static string BuildRootMaterialKey(TaktEcDetail detail)
    {
        return BuildRootMaterialKey(detail.PlantCode, detail.EcRootMaterialCode);
    }

    /// <summary>
    /// 工厂 + 物料编码键
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="materialCode">物料编码</param>
    /// <returns>组合键</returns>
    private static string BuildRootMaterialKey(string? plantCode, string? materialCode)
    {
        return (plantCode?.Trim() ?? string.Empty) + "\u001f" + (materialCode?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// 只保留工厂物料已建档的根物料明细。未建档的根物料不进入去重，避免把同组其它根物料一起跳过。
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <param name="presence">工厂物料建档情况</param>
    /// <returns>可派生明细</returns>
    private static List<TaktEcDetail> SelectKnownRootDetails(
        IReadOnlyList<TaktEcDetail> details,
        RootMaterialPresence presence)
    {
        return details
            .Where(detail => presence.KnownKeys.Contains(BuildRootMaterialKey(detail)))
            .ToList();
    }

    /// <summary>
    /// 工厂物料中已建档的根物料，以及其中类型不是 FERT 的键
    /// </summary>
    /// <param name="KnownKeys">已建档的工厂+物料键</param>
    private readonly record struct RootMaterialPresence(HashSet<string> KnownKeys);

    /// <summary>
    /// 批量解析根物料在工厂物料中的建档情况（未建档不进入已知集合）
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>已知键与非 FERT 键</returns>
    private async Task<RootMaterialPresence> LoadRootMaterialPresenceAsync(IReadOnlyList<TaktEcDetail> details)
    {
        var codesByPlant = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in details)
        {
            var materialCode = detail.EcRootMaterialCode?.Trim();
            var plantCode = detail.PlantCode?.Trim();
            if (string.IsNullOrEmpty(materialCode) || string.IsNullOrEmpty(plantCode))
            {
                continue;
            }
            if (!codesByPlant.TryGetValue(plantCode, out var codes))
            {
                codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                codesByPlant[plantCode] = codes;
            }
            codes.Add(materialCode);
        }
        var known = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        const int chunkSize = 500;
        foreach (var pair in codesByPlant)
        {
            var plantCode = pair.Key;
            var codeList = pair.Value.ToList();
            for (var offset = 0; offset < codeList.Count; offset += chunkSize)
            {
                var count = Math.Min(chunkSize, codeList.Count - offset);
                var chunk = codeList.GetRange(offset, count);
                var materials = await _materialPlantRepository.GetListAsync(x =>
                    x.PlantCode == plantCode && chunk.Contains(x.MaterialCode));
                foreach (var material in materials)
                {
                    var key = BuildRootMaterialKey(material.PlantCode, material.MaterialCode);
                    known.Add(key);
                }
            }
        }
        return new RootMaterialPresence(known);
    }

    /// <summary>
    /// 明细根物料是否已在工厂物料建档
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>已建档为 true</returns>
    private async Task<bool> IsKnownRootMaterialAsync(TaktEcDetail detail)
    {
        var presence = await LoadRootMaterialPresenceAsync([detail]);
        return presence.KnownKeys.Contains(BuildRootMaterialKey(detail));
    }

    /// <summary>
    /// 单行自动执行内容。全仕向、部管按部门实施范围写 EOL 或物料类型；内部、技术不在这里改写。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>自动执行内容；不自动填写时为 null</returns>
    private async Task<string?> ResolveAutoCloseExecContentAsync(TaktEcDetail detail, string deptCode, int ecImplScope)
    {
        var siblings = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode && x.IsObsolete == 0);
        var deptScope = TaktEcControlScopeConstants.ResolveForDept(deptCode, ecImplScope, await GetMasterControlScopeAsync(detail));
        IReadOnlySet<string>? fertRootKeys = null;
        if (deptCode == TaktEcDeptCodes.Mfg1 && deptScope == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            fertRootKeys = CollectFertRootMaterialKeys(siblings);
        }
        var presence = await LoadRootMaterialPresenceAsync(siblings);
        var derivable = SelectKnownRootDetails(siblings, presence);
        return ResolveAutoCloseExecContent(
            derivable,
            detail,
            deptCode,
            ecImplScope,
            deptScope,
            fertRootKeys);
    }

    /// <summary>
    /// 单行落库时，生管、制一、制二、品管的非 FERT 行把控制范围写成不管理。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>任务</returns>
    private async Task ApplyNonFertDeptControlledAsync(object exec, TaktEcDetail detail, string deptCode, int ecImplScope)
    {
        if (exec is not ITaktEcDeptExecEntity)
        {
            return;
        }
        var siblings = await _ecDetailRepository.GetListAsync(x =>
            x.EcCode == detail.EcCode && x.IsObsolete == 0);
        var deptScope = TaktEcControlScopeConstants.ResolveForDept(deptCode, ecImplScope, await GetMasterControlScopeAsync(detail));
        IReadOnlySet<string>? fertRootKeys = null;
        if (deptCode == TaktEcDeptCodes.Mfg1 && deptScope == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            fertRootKeys = CollectFertRootMaterialKeys(siblings);
        }
        var presence = await LoadRootMaterialPresenceAsync(siblings);
        var derivable = SelectKnownRootDetails(siblings, presence);
        ApplyMaterialTypeDeptControlled(exec, derivable, detail, deptCode, deptScope, fertRootKeys);
    }

    /// <summary>
    /// 按明细根物料类型收集 FERT 键
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>FERT 的工厂+物料键</returns>
    private static HashSet<string> CollectFertRootMaterialKeys(IReadOnlyList<TaktEcDetail> details)
    {
        var fert = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in details)
        {
            if (!string.Equals(
                detail.EcRootMaterialType?.Trim(),
                TaktEcMfg1Constants.FertMaterialType,
                StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            fert.Add(BuildRootMaterialKey(detail));
        }
        return fert;
    }

    /// <summary>
    /// 写入根物料编码组（无该字段的部门忽略）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="rootGroup">分号分隔的根物料编码</param>
    private static void AssignRootMaterialCodeGroup(object exec, string rootGroup)
    {
        switch (exec)
        {
            case TaktEcPc row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcMc row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcMtl row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcMfg1 row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcMfg2 row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcQc row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcPur row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcIqc row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcMe row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
        }
    }

    /// <summary>
    /// 写入机种编码组（生管/采购/受检/部管/SMT/制一/制二/品管/制技）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="modelGroup">分号分隔的机种编码</param>
    private static void AssignModelCodeGroup(object exec, string modelGroup)
    {
        switch (exec)
        {
            case TaktEcPur row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcIqc row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcMfg2 row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcMfg1 row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcQc row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcPc row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcMe row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcMc row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcMtl row:
                row.EcModelCodeGroup = modelGroup;
                break;
        }
    }

    /// <summary>
    /// 按设变时写入制二的新物料编码组与上阶物料编码组
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="newMaterialGroup">新物料编码组</param>
    /// <param name="parentMaterialGroup">上阶物料编码组</param>
    private static void AssignPcbaMaterialGroups(object exec, string newMaterialGroup, string parentMaterialGroup)
    {
        if (exec is TaktEcMfg2 row)
        {
            row.EcNewMaterialCodeGroup = newMaterialGroup;
            row.EcParentMaterialCodeGroup = parentMaterialGroup;
        }
    }

    /// <summary>
    /// 标记作废（已作废则 false）
    /// </summary>
    private static bool TryMarkObsolete(object? existing)
    {
        if (existing == null || TaktEcDeptEntityHelper.GetIsObsolete(existing) == 1)
        {
            return false;
        }
        if (existing is ITaktEcDeptExecEntity deptExec)
        {
            deptExec.IsObsolete = 1;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 按部门批量落库
    /// </summary>
    private async Task SaveTypedDeptRangeAsync(string deptCode, List<object> toCreate, List<object> toUpdate)
    {
        switch (deptCode)
        {
            case TaktEcDeptCodes.Pc:
                await SaveTypedRangeAsync(_pcRepository, toCreate.Cast<TaktEcPc>().ToList(), toUpdate.Cast<TaktEcPc>().ToList());
                break;
            case TaktEcDeptCodes.Pur:
                await SaveTypedRangeAsync(_mpRepository, toCreate.Cast<TaktEcPur>().ToList(), toUpdate.Cast<TaktEcPur>().ToList());
                break;
            case TaktEcDeptCodes.Iqc:
                await SaveTypedRangeAsync(_iqcRepository, toCreate.Cast<TaktEcIqc>().ToList(), toUpdate.Cast<TaktEcIqc>().ToList());
                break;
            case TaktEcDeptCodes.Mc:
                await SaveTypedRangeAsync(_mcRepository, toCreate.Cast<TaktEcMc>().ToList(), toUpdate.Cast<TaktEcMc>().ToList());
                break;
            case TaktEcDeptCodes.Mfg1:
                await SaveTypedRangeAsync(_assyRepository, toCreate.Cast<TaktEcMfg1>().ToList(), toUpdate.Cast<TaktEcMfg1>().ToList());
                break;
            case TaktEcDeptCodes.Qc:
                await SaveTypedRangeAsync(_qaRepository, toCreate.Cast<TaktEcQc>().ToList(), toUpdate.Cast<TaktEcQc>().ToList());
                break;
            case TaktEcDeptCodes.Me:
                await SaveTypedRangeAsync(_teRepository, toCreate.Cast<TaktEcMe>().ToList(), toUpdate.Cast<TaktEcMe>().ToList());
                break;
            default:
                throw new InvalidOperationException($"不支持的部门编码：{deptCode}");
        }
    }

    /// <summary>
    /// 泛型批量 Create/Update
    /// </summary>
    private static async Task SaveTypedRangeAsync<TEntity>(
        ITaktCompanyRepository<TEntity> repository,
        List<TEntity> toCreate,
        List<TEntity> toUpdate)
        where TEntity : TaktCompanyEntityBase, new()
    {
        if (toCreate.Count > 0)
        {
            await repository.CreateRangeAsync(toCreate);
        }
        if (toUpdate.Count > 0)
        {
            await repository.UpdateRangeAsync(toUpdate);
        }
    }

    /// <summary>
    /// 删掉同设变单号+机种下非代表明细的执行行，避免唯一键（设变单号+机种编码）冲突
    /// </summary>
    /// <typeparam name="TEntity">部管或 SMT 执行实体</typeparam>
    /// <param name="existing">本设变已有执行行</param>
    /// <param name="keepers">本次保留的执行行</param>
    /// <param name="repository">对应仓储</param>
    /// <param name="toUpdate">待更新列表（去掉已删除行）</param>
    private static async Task DeleteNonKeeperEcModelRowsAsync<TEntity>(
        IEnumerable<TEntity> existing,
        IEnumerable<TEntity> keepers,
        ITaktCompanyRepository<TEntity> repository,
        List<object> toUpdate)
        where TEntity : TaktCompanyEntityBase, ITaktEcDeptExecEntity, new()
    {
        var keeperDetailIds = keepers
            .Where(x => x.IsObsolete == 0)
            .Select(x => x.EcGrainKey)
            .ToHashSet();
        var deleteIds = existing
            .Where(x => x.Id > 0 && !keeperDetailIds.Contains(x.EcGrainKey))
            .Select(x => x.Id)
            .Distinct()
            .ToList();
        if (deleteIds.Count == 0)
        {
            return;
        }
        await repository.DeleteAsync(x => deleteIds.Contains(x.Id));
        toUpdate.RemoveAll(x => x is TEntity row && deleteIds.Contains(row.Id));
    }

    /// <summary>
    /// 删掉本设变中本次未保留的执行行，使落库行数与实施范围去重键一致
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="existing">本设变已有执行行</param>
    /// <param name="toCreate">待新增</param>
    /// <param name="toUpdate">待更新</param>
    /// <returns>任务</returns>
    private async Task DeleteNonKeeperRowsForDeptAsync(
        string deptCode,
        IEnumerable<object> existing,
        IEnumerable<object> toCreate,
        List<object> toUpdate)
    {
        switch (deptCode)
        {
            case TaktEcDeptCodes.Pc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcPc>(), toCreate.OfType<TaktEcPc>().Concat(toUpdate.OfType<TaktEcPc>()), _pcRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Pur:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcPur>(), toCreate.OfType<TaktEcPur>().Concat(toUpdate.OfType<TaktEcPur>()), _mpRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Iqc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcIqc>(), toCreate.OfType<TaktEcIqc>().Concat(toUpdate.OfType<TaktEcIqc>()), _iqcRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Mc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcMc>(), toCreate.OfType<TaktEcMc>().Concat(toUpdate.OfType<TaktEcMc>()), _mcRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Mfg1:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcMfg1>(), toCreate.OfType<TaktEcMfg1>().Concat(toUpdate.OfType<TaktEcMfg1>()), _assyRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Qc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcQc>(), toCreate.OfType<TaktEcQc>().Concat(toUpdate.OfType<TaktEcQc>()), _qaRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Me:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcMe>(), toCreate.OfType<TaktEcMe>().Concat(toUpdate.OfType<TaktEcMe>()), _teRepository, toUpdate);
                break;
        }
    }

    /// <summary>
    /// 按设变单号+机种编码汇总可见明细的根物料编码组
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="visible">是否计入编码组</param>
    /// <returns>键为设变单号与机种编码</returns>
    private static Dictionary<(string EcCode, string Model), string> BuildEcModelRootMaterialCodeGroups(
        IReadOnlyList<TaktEcDetail> details,
        Func<TaktEcDetail, bool> visible)
    {
        return details
            .Where(d => d.IsObsolete == 0 && visible(d))
            .GroupBy(d => (d.EcCode ?? string.Empty, d.EcModelCode ?? string.Empty))
            .ToDictionary(
                g => g.Key,
                g => TaktEcRootMaterialCodeGroup.Join(g.Select(x => x.EcRootMaterialCode)));
    }

    /// <summary>
    /// 采购/受检/部管是否属于列表可见条件（不含组内去重）
    /// </summary>
    private static bool IsNewMaterialDeptListVisible(TaktEcDetail detail, string deptCode)
    {
        return deptCode switch
        {
            TaktEcDeptCodes.Pur => TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType),
            TaktEcDeptCodes.Iqc => detail.EcNewRequiresInspection == 1,
            TaktEcDeptCodes.Mc => TaktEcScopeConstants.IsMcVisible(
                detail.EcNewPurchaseType,
                detail.EcNewWarehouse),
            _ => true
        };
    }

    /// <summary>
    /// 各部门列表去重：可见组内非最大 Id 跳过（与各课 QueryHelper 同键）
    /// </summary>
    private static HashSet<long> BuildListDedupSkipDetailIds(IReadOnlyList<TaktEcDetail> details, string deptCode)
    {
        return deptCode switch
        {
            TaktEcDeptCodes.Pc or TaktEcDeptCodes.Mfg1 or TaktEcDeptCodes.Qc
                => BuildModelRootMaterialSkipDetailIds(details),
            TaktEcDeptCodes.Me => BuildTeEcModelSkipDetailIds(details),
            TaktEcDeptCodes.Pur => BuildMpSkipDetailIds(details),
            TaktEcDeptCodes.Iqc => BuildIqcSkipDetailIds(details),
            TaktEcDeptCodes.Mc => BuildMcSkipDetailIds(details),
            _ => []
        };
    }

    /// <summary>
    /// 生管/制一/品管：设变+机种+根物料编码去重，非最大 Id 跳过
    /// </summary>
    private static HashSet<long> BuildModelRootMaterialSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d => d.IsObsolete == 0);
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty,
            Finished: d.EcRootMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 制技：始终按设变单号+机种去重，非最大 Id 跳过
    /// </summary>
    private static HashSet<long> BuildTeEcModelSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d => d.IsObsolete == 0);
        foreach (var group in visible.GroupBy(
            d => $"{d.EcCode}\u001f{d.EcModelCode}",
            StringComparer.OrdinalIgnoreCase))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 采购课：采购类型 F 按设变去重（设变单号+新物料），非最大 Id 跳过
    /// </summary>
    private static HashSet<long> BuildMpSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsExternalPurchaseType(d.EcNewPurchaseType)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(
            d => $"{d.EcCode}\u001f{d.EcNewMaterialCode}",
            StringComparer.OrdinalIgnoreCase))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 受检课：新品检验为 1 按设变去重（设变单号+新物料），非最大 Id 跳过
    /// </summary>
    private static HashSet<long> BuildIqcSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            d.EcNewRequiresInspection == 1
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(
            d => $"{d.EcCode}\u001f{d.EcNewMaterialCode}",
            StringComparer.OrdinalIgnoreCase))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 部管课：同一设变、机种、行号、新物料只留一条，成品展开不重复落行
    /// </summary>
    private static HashSet<long> BuildMcSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        return TaktEcScopeRowGrain.BuildLineMaterialSkipIds(details, TaktEcDeptCodes.Mc, 0, null);
    }

    /// <summary>
    /// SMT：同一设变、机种、行号、新物料只留一条，成品展开不重复落行
    /// </summary>
    private static HashSet<long> BuildPcbaSmtSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        return TaktEcScopeRowGrain.BuildLineMaterialSkipIds(details, TaktEcDeptCodes.Mtl, 0, null);
    }

    /// <summary>
    /// 制二非 F 可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildPcbaPcbaSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsPcbaOtherPurchaseGroup(d.EcNewPurchaseType));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty,
            Finished: d.EcRootMaterialCode ?? string.Empty)))
        {
            var maxId = group.Max(x => x.Id);
            foreach (var d in group)
            {
                if (d.Id != maxId)
                {
                    skip.Add(d.Id);
                }
            }
        }
        return skip;
    }
}
