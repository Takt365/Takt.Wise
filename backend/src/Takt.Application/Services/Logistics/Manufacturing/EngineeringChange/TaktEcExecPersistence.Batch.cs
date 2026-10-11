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
    /// <param name="ecScope">设变实施范围</param>
    /// <returns>写入/跳过条数</returns>
    public async Task<TaktEcDeptExecBatchResult> UpsertDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecScope)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        ArgumentNullException.ThrowIfNull(shouldAutoComplete);
        if (details.Count == 0)
        {
            return new TaktEcDeptExecBatchResult(0, 0);
        }
        if (deptCode == TaktEcDeptCodes.Pcba)
        {
            return await UpsertPcbaDeptExecBatchWithFillModeAsync(details, shouldAutoComplete, ecScope);
        }
        return await UpsertTypedDeptExecBatchWithFillModeAsync(details, deptCode, shouldAutoComplete, ecScope);
    }

    /// <summary>
    /// 非制造二课：单表批量 Upsert
    /// </summary>
    private async Task<TaktEcDeptExecBatchResult> UpsertTypedDeptExecBatchWithFillModeAsync(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        Func<TaktEcDetail, bool> shouldAutoComplete,
        int ecScope)
    {
        var ecCode = details
            .Select(x => x.EcCode?.Trim())
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? string.Empty;
        var existingList = !string.IsNullOrWhiteSpace(ecCode)
            ? await ListEntitiesByEcCodeAndDeptAsync(ecCode, deptCode)
            : await ListEntitiesByDetailIdsAndDeptAsync(details.Select(x => x.Id).ToList(), deptCode);
        var existingByDetailId = existingList
            .OfType<ITaktEcDeptExecEntity>()
            .GroupBy(x => x.EcDetailId)
            .ToDictionary(g => g.Key, g => (object)g.First());
        var ecDeptScope = TaktEcDeptScopeConstants.ResolveForDept(deptCode, ecScope, details[0].EcDeptScope);
        var dedupKind = TaktEcScopeRowGrain.ResolveKind(deptCode, ecDeptScope);
        var fertRootKeys = deptCode == TaktEcDeptCodes.Assy
            && ecDeptScope == TaktEcDeptScopeConstants.ByFinishedProduct
            ? await LoadFertRootMaterialKeysAsync(details)
            : null;
        var skipByDedup = TaktEcScopeRowGrain.BuildSkipIds(details, deptCode, ecDeptScope, fertRootKeys);
        var rootGroups = TaktEcScopeRowGrain.BuildRootGroups(details, deptCode, ecDeptScope, fertRootKeys);
        var modelGroups = TaktEcScopeRowGrain.BuildModelGroups(details, deptCode, ecDeptScope, fertRootKeys);
        var newMaterialGroups = deptCode == TaktEcDeptCodes.Pcba
            ? TaktEcScopeRowGrain.BuildCodeGroups(details, deptCode, ecDeptScope, fertRootKeys, detail => detail.EcNewMaterialCode)
            : null;
        var parentMaterialGroups = deptCode == TaktEcDeptCodes.Pcba
            ? TaktEcScopeRowGrain.BuildCodeGroups(details, deptCode, ecDeptScope, fertRootKeys, detail => detail.EcParentMaterialCode)
            : null;
        if (skipByDedup.Count > 0)
        {
            TaktLogger.Information(
                "[EcGijutsuPersist] 部门执行去重 DeptCode={DeptCode} DetailCount={DetailCount} SkipByDedup={SkipCount}",
                deptCode,
                details.Count,
                skipByDedup.Count);
        }
        var applyNotRelatedAuto = TaktEcScopeConstants.AllowsNotRelatedFill(ecScope);
        var nonFertRootKeys = deptCode == TaktEcDeptCodes.Assy
            && dedupKind == TaktEcScopeDedupKind.ModelRoot
            ? await LoadNonFertRootMaterialKeysAsync(details)
            : null;
        var deptName = await ResolveDeptNameAsync(deptCode);
        var toCreate = new List<object>();
        var toUpdate = new List<object>();
        var savedCount = 0;
        var skippedCount = 0;
        foreach (var detail in details)
        {
            existingByDetailId.TryGetValue(detail.Id, out var existing);
            if (!TaktEcScopeRowGrain.IsEligible(detail, deptCode, ecScope, fertRootKeys))
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
            var exec = CreateConcreteExec(detail, deptCode, existing, lineNumber, applyNotRelatedAuto: false);
            var groupKey = TaktEcScopeRowGrain.BuildKey(detail, dedupKind);
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
            ApplyScopeFillMode(exec, isNew, shouldAutoComplete(detail), ecScope, detail);
            var filledContent = TaktEcDeptEntityHelper.GetExecContent(exec);
            var isEolFilled = TaktEcScopeConstants.IsEolExecContent(filledContent);
            if (!isEolFilled && applyNotRelatedAuto)
            {
                TaktEcExecNotRelated.TryAfterFill(exec, detail);
            }
            NormalizeExecContent(exec);
            if (nonFertRootKeys != null
                && exec is TaktEcSeizouikka assy
                && nonFertRootKeys.Contains(BuildRootMaterialKey(detail)))
            {
                ApplySeizouikkaNonFertDefault(assy, isNew);
            }
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
            "[EcGijutsuPersist] 部门执行已落库 DeptCode={DeptCode} Saved={Saved} Skipped={Skipped}",
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
        int ecScope)
    {
        var ecCode = details
            .Select(x => x.EcCode?.Trim())
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
            ?? string.Empty;
        Dictionary<long, TaktEcSmt> smtExisting;
        Dictionary<long, TaktEcSeizounika> seizounikaExisting;
        if (!string.IsNullOrWhiteSpace(ecCode))
        {
            smtExisting = (await _smtRepository.GetListAsync(x => x.EcCode == ecCode))
                .GroupBy(x => x.EcDetailId)
                .ToDictionary(g => g.Key, g => g.First());
            seizounikaExisting = (await _seizounikaRepository.GetListAsync(x => x.EcCode == ecCode))
                .GroupBy(x => x.EcDetailId)
                .ToDictionary(g => g.Key, g => g.First());
        }
        else
        {
            var detailIds = details.Select(x => x.Id).ToList();
            smtExisting = new Dictionary<long, TaktEcSmt>();
            seizounikaExisting = new Dictionary<long, TaktEcSeizounika>();
            const int idChunk = 1000;
            for (var offset = 0; offset < detailIds.Count; offset += idChunk)
            {
                var chunk = detailIds.Skip(offset).Take(idChunk).ToList();
                foreach (var row in await _smtRepository.GetListAsync(x => chunk.Contains(x.EcDetailId)))
                {
                    smtExisting.TryAdd(row.EcDetailId, row);
                }
                foreach (var row in await _seizounikaRepository.GetListAsync(x => chunk.Contains(x.EcDetailId)))
                {
                    seizounikaExisting.TryAdd(row.EcDetailId, row);
                }
            }
        }
        var ecDeptScope = details[0].EcDeptScope > 0 ? details[0].EcDeptScope : TaktEcDeptScopeConstants.ByModel;
        var smtKind = TaktEcScopeRowGrain.ResolveKind(TaktEcDeptCodes.Smt, ecDeptScope);
        var nikaKind = TaktEcScopeRowGrain.ResolveKind(TaktEcDeptCodes.Pcba, ecDeptScope);
        var skipSmt = TaktEcScopeRowGrain.BuildSkipIds(details, TaktEcDeptCodes.Smt, ecDeptScope, null);
        var skipSeizounika = TaktEcScopeRowGrain.BuildSkipIds(details, TaktEcDeptCodes.Pcba, ecDeptScope, null);
        var smtRootGroups = TaktEcScopeRowGrain.BuildRootGroups(details, TaktEcDeptCodes.Smt, ecDeptScope, null);
        var nikaRootGroups = TaktEcScopeRowGrain.BuildRootGroups(details, TaktEcDeptCodes.Pcba, ecDeptScope, null);
        _ = shouldAutoComplete;
        var applyNotRelatedAuto = TaktEcScopeConstants.AllowsNotRelatedFill(ecScope);
        var smtDeptName = await ResolveDeptNameAsync(TaktEcDeptCodes.Smt);
        var seizounikaDeptName = await ResolveDeptNameAsync(TaktEcDeptCodes.Pcba);
        var smtCreate = new List<TaktEcSmt>();
        var smtUpdate = new List<TaktEcSmt>();
        var seizounikaCreate = new List<TaktEcSeizounika>();
        var seizounikaUpdate = new List<TaktEcSeizounika>();
        var savedCount = 0;
        var skippedCount = 0;
        foreach (var detail in details)
        {
            smtExisting.TryGetValue(detail.Id, out var smtRow);
            seizounikaExisting.TryGetValue(detail.Id, out var seizounikaRow);
            var smtKeep = TaktEcScopeRowGrain.IsEligible(detail, TaktEcDeptCodes.Smt, ecScope, null)
                && !skipSmt.Contains(detail.Id);
            var nikaKeep = TaktEcScopeRowGrain.IsEligible(detail, TaktEcDeptCodes.Pcba, ecScope, null)
                && !skipSeizounika.Contains(detail.Id);
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
                var exec = (TaktEcSmt)CreatePcbaConcreteExec(
                    detail,
                    TaktEcSmtRouteTarget.Smt,
                    smtRow,
                    lineNumber,
                    applyNotRelatedAuto: false);
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
                ApplyScopeFillMode(
                    exec,
                    isNew,
                    TaktEcScopeConstants.ShouldAutoCompleteExec(ecScope, TaktEcDeptCodes.Smt, detail.EcNewPurchaseType, detail.EcNewWarehouse),
                    ecScope,
                    detail);
                ApplyPostFillNotRelated(exec, detail, applyNotRelatedAuto);
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
                if (TryMarkObsolete(seizounikaRow))
                {
                    seizounikaUpdate.Add(seizounikaRow!);
                }
            }
            else
            {
                var isNew = seizounikaRow == null;
                var lineNumber = detail.LineNumber > 0 ? detail.LineNumber : 10;
                var exec = (TaktEcSeizounika)CreatePcbaConcreteExec(
                    detail,
                    TaktEcSmtRouteTarget.Seizounika,
                    seizounikaRow,
                    lineNumber,
                    applyNotRelatedAuto: false);
                var nikaGroupKey = TaktEcScopeRowGrain.BuildKey(detail, nikaKind);
                if (nikaRootGroups.TryGetValue(nikaGroupKey, out var nikaRootGroup))
                {
                    exec.EcRootMaterialCodeGroup = nikaRootGroup;
                }
                EnsureDeptName(exec, seizounikaDeptName);
                if (exec.IsObsolete == 1)
                {
                    exec.IsObsolete = 0;
                }
                ApplyScopeFillMode(
                    exec,
                    isNew,
                    TaktEcScopeConstants.ShouldAutoCompleteExec(ecScope, TaktEcDeptCodes.Pcba, detail.EcNewPurchaseType, detail.EcNewWarehouse),
                    ecScope,
                    detail);
                ApplyPostFillNotRelated(exec, detail, applyNotRelatedAuto);
                NormalizeExecContent(exec);
                if (isNew)
                {
                    seizounikaCreate.Add(exec);
                }
                else
                {
                    seizounikaUpdate.Add(exec);
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
            .Select(x => x.EcDetailId)
            .ToHashSet();
        var smtDeleteIds = smtExisting.Values
            .Where(x => x.Id > 0 && !smtKeeperDetailIds.Contains(x.EcDetailId))
            .Select(x => x.Id)
            .Distinct()
            .ToList();
        if (smtDeleteIds.Count > 0)
        {
            await _smtRepository.DeleteAsync(x => smtDeleteIds.Contains(x.Id));
            smtUpdate.RemoveAll(x => smtDeleteIds.Contains(x.Id));
        }
        var nikaKeeperDetailIds = seizounikaCreate
            .Concat(seizounikaUpdate)
            .Where(x => x.IsObsolete == 0)
            .Select(x => x.EcDetailId)
            .ToHashSet();
        var nikaDeleteIds = seizounikaExisting.Values
            .Where(x => x.Id > 0 && !nikaKeeperDetailIds.Contains(x.EcDetailId))
            .Select(x => x.Id)
            .Distinct()
            .ToList();
        if (nikaDeleteIds.Count > 0)
        {
            await _seizounikaRepository.DeleteAsync(x => nikaDeleteIds.Contains(x.Id));
            seizounikaUpdate.RemoveAll(x => nikaDeleteIds.Contains(x.Id));
        }
        if (smtCreate.Count > 0)
        {
            await _smtRepository.CreateRangeAsync(smtCreate);
        }
        if (smtUpdate.Count > 0)
        {
            await _smtRepository.UpdateRangeAsync(smtUpdate);
        }
        if (seizounikaCreate.Count > 0)
        {
            await _seizounikaRepository.CreateRangeAsync(seizounikaCreate);
        }
        if (seizounikaUpdate.Count > 0)
        {
            await _seizounikaRepository.UpdateRangeAsync(seizounikaUpdate);
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
    /// 制一课：根物料类型不是 FERT 时默认已实施，预定日期为当天，预定批次与实施批次为制一课无关。
    /// 已手填过其它批次的旧行不覆盖；已是无关文案时保留原预定日期，只补回已实施。
    /// </summary>
    /// <param name="row">制一执行行</param>
    /// <param name="isNew">是否新建</param>
    private static void ApplySeizouikkaNonFertDefault(TaktEcSeizouikka row, bool isNew)
    {
        var marker = TaktEcSeizouikkaConstants.NotRelatedBatch;
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
    private static bool IsSeizouikkaNotRelatedDefault(TaktEcSeizouikka row)
    {
        return TaktEcSeizouikkaConstants.IsNotRelatedBatch(row.ScheduledBatch)
            || TaktEcSeizouikkaConstants.IsNotRelatedBatch(row.ImplementationBatch);
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
    /// 批量解析根物料类型不是 FERT 的工厂物料键（未建档的编码不计入）
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>非 FERT 的工厂+物料键</returns>
    private async Task<HashSet<string>> LoadNonFertRootMaterialKeysAsync(IReadOnlyList<TaktEcDetail> details)
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
        var nonFert = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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
                    if (string.Equals(
                        material.MaterialType?.Trim(),
                        TaktEcSeizouikkaConstants.FertMaterialType,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    nonFert.Add(BuildRootMaterialKey(material.PlantCode, material.MaterialCode));
                }
            }
        }
        return nonFert;
    }

    /// <summary>
    /// 批量解析根物料类型为 FERT 的工厂物料键（未建档的编码不计入）
    /// </summary>
    /// <param name="details">设变明细</param>
    /// <returns>FERT 的工厂+物料键</returns>
    private async Task<HashSet<string>> LoadFertRootMaterialKeysAsync(IReadOnlyList<TaktEcDetail> details)
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
        var fert = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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
                    if (!string.Equals(
                        material.MaterialType?.Trim(),
                        TaktEcSeizouikkaConstants.FertMaterialType,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    fert.Add(BuildRootMaterialKey(material.PlantCode, material.MaterialCode));
                }
            }
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
            case TaktEcSeikan row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcBukan row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcSmt row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcSeizouikka row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcSeizounika row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcHinkan row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcKoubai row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcUkeken row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
            case TaktEcSeizougijutsu row:
                row.EcRootMaterialCodeGroup = rootGroup;
                break;
        }
    }

    /// <summary>
    /// 写入机种编码组（采购/受检）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="modelGroup">分号分隔的机种编码</param>
    private static void AssignModelCodeGroup(object exec, string modelGroup)
    {
        switch (exec)
        {
            case TaktEcKoubai row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcUkeken row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcSeizounika row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcSeizouikka row:
                row.EcModelCodeGroup = modelGroup;
                break;
            case TaktEcHinkan row:
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
        if (exec is TaktEcSeizounika row)
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
            case TaktEcDeptCodes.Pmc:
                await SaveTypedRangeAsync(_pmcRepository, toCreate.Cast<TaktEcSeikan>().ToList(), toUpdate.Cast<TaktEcSeikan>().ToList());
                break;
            case TaktEcDeptCodes.Mp:
                await SaveTypedRangeAsync(_mpRepository, toCreate.Cast<TaktEcKoubai>().ToList(), toUpdate.Cast<TaktEcKoubai>().ToList());
                break;
            case TaktEcDeptCodes.Iqc:
                await SaveTypedRangeAsync(_iqcRepository, toCreate.Cast<TaktEcUkeken>().ToList(), toUpdate.Cast<TaktEcUkeken>().ToList());
                break;
            case TaktEcDeptCodes.Mc:
                await SaveTypedRangeAsync(_mcRepository, toCreate.Cast<TaktEcBukan>().ToList(), toUpdate.Cast<TaktEcBukan>().ToList());
                break;
            case TaktEcDeptCodes.Assy:
                await SaveTypedRangeAsync(_assyRepository, toCreate.Cast<TaktEcSeizouikka>().ToList(), toUpdate.Cast<TaktEcSeizouikka>().ToList());
                break;
            case TaktEcDeptCodes.Qa:
                await SaveTypedRangeAsync(_qaRepository, toCreate.Cast<TaktEcHinkan>().ToList(), toUpdate.Cast<TaktEcHinkan>().ToList());
                break;
            case TaktEcDeptCodes.Te:
                await SaveTypedRangeAsync(_teRepository, toCreate.Cast<TaktEcSeizougijutsu>().ToList(), toUpdate.Cast<TaktEcSeizougijutsu>().ToList());
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
            .Select(x => x.EcDetailId)
            .ToHashSet();
        var deleteIds = existing
            .Where(x => x.Id > 0 && !keeperDetailIds.Contains(x.EcDetailId))
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
            case TaktEcDeptCodes.Pmc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcSeikan>(), toCreate.OfType<TaktEcSeikan>().Concat(toUpdate.OfType<TaktEcSeikan>()), _pmcRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Mp:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcKoubai>(), toCreate.OfType<TaktEcKoubai>().Concat(toUpdate.OfType<TaktEcKoubai>()), _mpRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Iqc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcUkeken>(), toCreate.OfType<TaktEcUkeken>().Concat(toUpdate.OfType<TaktEcUkeken>()), _iqcRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Mc:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcBukan>(), toCreate.OfType<TaktEcBukan>().Concat(toUpdate.OfType<TaktEcBukan>()), _mcRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Assy:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcSeizouikka>(), toCreate.OfType<TaktEcSeizouikka>().Concat(toUpdate.OfType<TaktEcSeizouikka>()), _assyRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Qa:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcHinkan>(), toCreate.OfType<TaktEcHinkan>().Concat(toUpdate.OfType<TaktEcHinkan>()), _qaRepository, toUpdate);
                break;
            case TaktEcDeptCodes.Te:
                await DeleteNonKeeperEcModelRowsAsync(existing.OfType<TaktEcSeizougijutsu>(), toCreate.OfType<TaktEcSeizougijutsu>().Concat(toUpdate.OfType<TaktEcSeizougijutsu>()), _teRepository, toUpdate);
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
            TaktEcDeptCodes.Mp => TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType),
            TaktEcDeptCodes.Iqc => detail.EcNewRequiresInspection == 1,
            TaktEcDeptCodes.Mc => TaktEcScopeConstants.IsBukanVisible(
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
            TaktEcDeptCodes.Pmc or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa or TaktEcDeptCodes.Te
                => BuildModelRootMaterialSkipDetailIds(details),
            TaktEcDeptCodes.Mp => BuildKoubaiSkipDetailIds(details),
            TaktEcDeptCodes.Iqc => BuildUkekenSkipDetailIds(details),
            TaktEcDeptCodes.Mc => BuildBukanSkipDetailIds(details),
            _ => []
        };
    }

    /// <summary>
    /// 生管/制一/品管/制技：设变+机种+根物料编码去重，非最大 Id 跳过
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
    /// 采购课可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildKoubaiSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsExternalPurchaseType(d.EcNewPurchaseType)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            PurchaseType: d.EcNewPurchaseType ?? string.Empty,
            Material: d.EcNewMaterialCode ?? string.Empty)))
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
    /// 受检课可见组去重跳过 Id（设变单号+新品检验+新物料编码）
    /// </summary>
    private static HashSet<long> BuildUkekenSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            d.EcNewRequiresInspection == 1
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Inspection: d.EcNewRequiresInspection,
            Material: d.EcNewMaterialCode ?? string.Empty)))
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
    /// 部管课可见组去重跳过 Id（设变单号+机种编码）
    /// </summary>
    private static HashSet<long> BuildBukanSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsBukanVisible(d.EcNewPurchaseType, d.EcNewWarehouse)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty)))
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
    /// SMT 可见组去重跳过 Id（设变单号+机种编码）
    /// </summary>
    private static HashSet<long> BuildPcbaSmtSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
    {
        var skip = new HashSet<long>();
        var visible = details.Where(d =>
            TaktEcScopeConstants.IsPcbaC003ExternalGroup(d.EcNewPurchaseType, d.EcNewWarehouse)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(d.EcNewMaterialCode));
        foreach (var group in visible.GroupBy(d => (
            EcCode: d.EcCode ?? string.Empty,
            Model: d.EcModelCode ?? string.Empty)))
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
    /// 制二非 F 可见组去重跳过 Id
    /// </summary>
    private static HashSet<long> BuildPcbaSeizounikaSkipDetailIds(IReadOnlyList<TaktEcDetail> details)
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
