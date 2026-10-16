// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcScopeRowGrain.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：按实施范围决定各部门执行行去重键，并汇总同键根物料编码组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 部门执行行去重粒度（随实施范围变化）
/// </summary>
internal enum TaktEcScopeDedupKind
{
    /// <summary>
    /// 设变单号 + 机种 + 根物料编码
    /// </summary>
    ModelRoot = 1,

    /// <summary>
    /// 设变单号 + 机种，根物料写入编码组
    /// </summary>
    ModelGroup = 2,

    /// <summary>
    /// 设变单号 + 机种（自动文案行）
    /// </summary>
    ModelOnly = 3,

    /// <summary>
    /// 设变单号 + 机种 + 根物料 + 新物料
    /// </summary>
    ModelRootNew = 4,

    /// <summary>
    /// 设变单号 + 机种 + 新物料，根物料写入编码组
    /// </summary>
    ModelNewGroup = 5,

    /// <summary>
    /// 设变单号 + 新物料，根物料写入编码组（不含机种）
    /// </summary>
    EcNewGroup = 6,

    /// <summary>
    /// 采购：任何范围均为设变单号 + 新采购类型 + 新物料；机种与根物料写入编码组
    /// </summary>
    PurchaseNewGroup = 7,

    /// <summary>
    /// 受检：任何范围均为设变单号 + 新品检验 + 新物料；机种与根物料写入编码组
    /// </summary>
    PurchaseInspectNewGroup = 8,

    /// <summary>
    /// 按设变：仅设变单号；机种与根物料写入编码组。制二另汇总新物料与上阶物料编码组
    /// </summary>
    EcOnlyGroup = 9,

    /// <summary>
    /// 设变单号 + 根物料（按成品）；机种写入编码组
    /// </summary>
    EcRootGroup = 10,
}

/// <summary>
/// 设变实施范围 → 部门执行去重键。
/// 部门范围 EcDeptScope：1=按成品，2=按机种，3=按设变。管理范围 EcScope（全仕向/部管/内部/技术）不参与去重键。
/// </summary>
internal static class TaktEcScopeRowGrain
{
    /// <summary>
    /// 解析部门在该实施范围下的去重粒度
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <returns>去重粒度</returns>
    internal static TaktEcScopeDedupKind ResolveKind(string deptCode, int ecDeptScope)
    {
        var finished = ecDeptScope == TaktEcDeptScopeConstants.ByFinishedProduct;
        var byModel = ecDeptScope == TaktEcDeptScopeConstants.ByModel;
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => finished
                ? TaktEcScopeDedupKind.ModelRoot
                : byModel
                    ? TaktEcScopeDedupKind.ModelGroup
                    : TaktEcScopeDedupKind.ModelOnly,
            TaktEcDeptCodes.Mc or TaktEcDeptCodes.Smt => TaktEcScopeDedupKind.ModelOnly,
            TaktEcDeptCodes.Te => TaktEcScopeDedupKind.ModelGroup,
            TaktEcDeptCodes.Pcba or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa => finished
                ? TaktEcScopeDedupKind.EcRootGroup
                : byModel
                    ? TaktEcScopeDedupKind.ModelGroup
                    : TaktEcScopeDedupKind.EcOnlyGroup,
            TaktEcDeptCodes.Mp or TaktEcDeptCodes.Iqc => TaktEcScopeDedupKind.EcNewGroup,
            _ => finished || byModel
                ? TaktEcScopeDedupKind.ModelRoot
                : TaktEcScopeDedupKind.ModelOnly,
        };
    }

    /// <summary>
    /// 明细是否参与该部门本范围的执行行（不含组内去重）。
    /// 部管在全仕向/部管下仍要采购类型 F 且仓库非 C003、并有有效新物料；实施范围为内部或技术时不看采购类型，每条未作废明细都落行。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">调用方传入的范围参数（去重键用部门实施范围，与明细上的实施范围数值可能撞车，部管资格以明细 EcScope 为准）</param>
    /// <param name="fertRootKeys">历史参数。制二/制一/品管已改为全量，不再按 FERT 过滤</param>
    /// <returns>是否可生成执行行</returns>
    internal static bool IsEligible(
        TaktEcDetail detail,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (detail.IsObsolete != 0)
        {
            return false;
        }
        if (deptCode is TaktEcDeptCodes.Pcba or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa)
        {
            return true;
        }
        _ = ecScope;
        return deptCode switch
        {
            TaktEcDeptCodes.Mc => IsMaterialControlEligible(detail),
            TaktEcDeptCodes.Smt => TaktEcScopeConstants.IsPcbaC003ExternalGroup(detail.EcNewPurchaseType, detail.EcNewWarehouse)
                && TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode),
            TaktEcDeptCodes.Mp => TaktEcScopeConstants.IsExternalPurchaseType(detail.EcNewPurchaseType)
                && TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode),
            TaktEcDeptCodes.Iqc => detail.EcNewRequiresInspection == 1
                && TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode),
            _ => true,
        };
    }

    /// <summary>
    /// 部管是否落执行行。资格看明细上的实施范围：内部、技术不限采购类型；其余范围要采购类型 F、仓库非 C003，且新物料有效。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>是否可生成部管执行行</returns>
    private static bool IsMaterialControlEligible(TaktEcDetail detail)
    {
        if (detail.EcScope == TaktEcScopeConstants.Internal
            || detail.EcScope == TaktEcScopeConstants.Technical)
        {
            return true;
        }
        return TaktEcScopeConstants.IsBukanVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse)
            && TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode);
    }

    /// <summary>
    /// 去重键（忽略大小写比较）
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="kind">去重粒度</param>
    /// <returns>组合键</returns>
    internal static string BuildKey(TaktEcDetail detail, TaktEcScopeDedupKind kind)
    {
        ArgumentNullException.ThrowIfNull(detail);
        var ecCode = detail.EcCode ?? string.Empty;
        var model = detail.EcModelCode ?? string.Empty;
        var root = detail.EcRootMaterialCode ?? string.Empty;
        var material = detail.EcNewMaterialCode ?? string.Empty;
        var purchaseType = detail.EcNewPurchaseType ?? string.Empty;
        var inspection = detail.EcNewRequiresInspection.ToString();
        return kind switch
        {
            TaktEcScopeDedupKind.ModelRoot => $"{ecCode}\u001f{model}\u001f{root}",
            TaktEcScopeDedupKind.ModelGroup or TaktEcScopeDedupKind.ModelOnly => $"{ecCode}\u001f{model}",
            TaktEcScopeDedupKind.ModelRootNew => $"{ecCode}\u001f{model}\u001f{root}\u001f{material}",
            TaktEcScopeDedupKind.ModelNewGroup => $"{ecCode}\u001f{model}\u001f{material}",
            TaktEcScopeDedupKind.EcNewGroup => $"{ecCode}\u001f{material}",
            TaktEcScopeDedupKind.PurchaseNewGroup => $"{ecCode}\u001f{purchaseType}\u001f{material}",
            TaktEcScopeDedupKind.PurchaseInspectNewGroup => $"{ecCode}\u001f{inspection}\u001f{material}",
            TaktEcScopeDedupKind.EcOnlyGroup => ecCode,
            TaktEcScopeDedupKind.EcRootGroup => $"{ecCode}\u001f{root}",
            _ => $"{ecCode}\u001f{model}\u001f{root}",
        };
    }

    /// <summary>
    /// 部管/SMT 行键：设变单号 + 机种 + 新物料。行号不同但新物料相同的明细并成一行。
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>组合键</returns>
    internal static string BuildLineMaterialKey(TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        return $"{detail.EcCode}\u001f{detail.EcModelCode}\u001f{detail.EcNewMaterialCode}";
    }

    /// <summary>
    /// 部管/SMT：同一行键只保留最大 Id，其余成品副本不落执行行
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="fertRootKeys">可见性过滤用的成品键</param>
    /// <returns>应跳过的明细 Id</returns>
    internal static HashSet<long> BuildLineMaterialSkipIds(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        var groups = new Dictionary<string, List<TaktEcDetail>>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in details)
        {
            if (!IsEligible(detail, deptCode, ecScope, fertRootKeys))
            {
                continue;
            }
            var key = BuildLineMaterialKey(detail);
            if (!groups.TryGetValue(key, out var list))
            {
                list = [];
                groups[key] = list;
            }
            list.Add(detail);
        }
        var skip = new HashSet<long>();
        foreach (var list in groups.Values)
        {
            var maxId = list.Max(x => x.Id);
            foreach (var detail in list)
            {
                if (detail.Id != maxId)
                {
                    skip.Add(detail.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 可见组内非最大 Id（这些明细不落执行行）
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>应跳过的明细 Id</returns>
    internal static HashSet<long> BuildSkipIds(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        if (deptCode is TaktEcDeptCodes.Mc or TaktEcDeptCodes.Smt)
        {
            return BuildLineMaterialSkipIds(details, deptCode, ecScope, fertRootKeys);
        }
        var kind = ResolveKind(deptCode, ecScope);
        var groups = new Dictionary<string, List<TaktEcDetail>>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in details)
        {
            if (!IsEligible(detail, deptCode, ecScope, fertRootKeys))
            {
                continue;
            }
            var key = BuildKey(detail, kind);
            if (!groups.TryGetValue(key, out var list))
            {
                list = [];
                groups[key] = list;
            }
            list.Add(detail);
        }
        var skip = new HashSet<long>();
        foreach (var list in groups.Values)
        {
            var maxId = list.Max(x => x.Id);
            foreach (var detail in list)
            {
                if (detail.Id != maxId)
                {
                    skip.Add(detail.Id);
                }
            }
        }
        return skip;
    }

    /// <summary>
    /// 根物料编码组内是否有未停产明细（空或 Z0）。有则执行内容留空，不按停产自动填写。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>组内存在未停产时为 true</returns>
    internal static bool RootGroupHasInProduction(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(keeper);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var kind = ResolveKind(deptCode, ecDeptScope);
        var key = BuildKey(keeper, kind);
        foreach (var item in details)
        {
            if (!IsEligible(item, deptCode, ecDeptScope, fertRootKeys))
            {
                continue;
            }
            if (!string.Equals(BuildKey(item, kind), key, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            if (!TaktEcScopeConstants.IsEolDiscontinued(item.DiscontinuedStatus))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 根物料编码组是否全部停产。组内至少有一条可派生明细，且没有未停产。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>整组停产时为 true</returns>
    internal static bool RootGroupAllDiscontinued(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(keeper);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var kind = ResolveKind(deptCode, ecDeptScope);
        var key = BuildKey(keeper, kind);
        var matched = false;
        foreach (var item in details)
        {
            if (!IsEligible(item, deptCode, ecDeptScope, fertRootKeys))
            {
                continue;
            }
            if (!string.Equals(BuildKey(item, kind), key, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            matched = true;
            if (!TaktEcScopeConstants.IsEolDiscontinued(item.DiscontinuedStatus))
            {
                return false;
            }
        }
        return matched;
    }

    /// <summary>
    /// 根物料编码组是否全部不是 FERT。组内至少有一条可派生明细，且每条根物料类型都不是 FERT。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>整组非 FERT 时为 true</returns>
    internal static bool RootGroupAllNonFert(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(keeper);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var kind = ResolveKind(deptCode, ecDeptScope);
        var key = BuildKey(keeper, kind);
        var matched = false;
        foreach (var item in details)
        {
            if (!IsEligible(item, deptCode, ecDeptScope, fertRootKeys))
            {
                continue;
            }
            if (!string.Equals(BuildKey(item, kind), key, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            matched = true;
            if (!IsNonFertRootMaterialType(item.EcRootMaterialType))
            {
                return false;
            }
        }
        return matched;
    }

    /// <summary>
    /// 根物料编码组的物料类型。整组都不是 FERT 时，按出现顺序去重，用 / 连接，如 HALB/ROH。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>物料类型后缀；不是整组非 FERT 时为 null</returns>
    internal static string? RootGroupMaterialTypeSuffix(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        if (!RootGroupAllNonFert(details, keeper, deptCode, ecDeptScope, fertRootKeys))
        {
            return null;
        }
        var kind = ResolveKind(deptCode, ecDeptScope);
        var key = BuildKey(keeper, kind);
        var types = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in details)
        {
            if (!IsEligible(item, deptCode, ecDeptScope, fertRootKeys))
            {
                continue;
            }
            if (!string.Equals(BuildKey(item, kind), key, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var materialType = item.EcRootMaterialType?.Trim() ?? string.Empty;
            if (materialType.Length == 0 || !seen.Add(materialType))
            {
                continue;
            }
            types.Add(materialType);
        }
        return types.Count == 0 ? null : string.Join('/', types);
    }

    /// <summary>
    /// 根物料编码组停产状态：组内任一未停产返回 Z0；全部停产时保留当前行状态。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>写入执行行的停产状态</returns>
    internal static string ResolveRootGroupDiscontinuedStatus(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(keeper);
        if (RootGroupHasInProduction(details, keeper, deptCode, ecDeptScope, fertRootKeys))
        {
            return TaktEcScopeConstants.PlannedMaterialStatus;
        }
        return keeper.DiscontinuedStatus?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 采购/受检停产状态：同去重键下任意未停产时返回 Z0；全部停产时保留当前行状态。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码（仅采购、受检）</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <returns>写入执行行的停产状态</returns>
    internal static string ResolvePurchaseInspectDiscontinuedStatus(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(keeper);
        var fallback = keeper.DiscontinuedStatus?.Trim() ?? string.Empty;
        if (deptCode is not (TaktEcDeptCodes.Mp or TaktEcDeptCodes.Iqc))
        {
            return fallback;
        }
        return ResolveRootGroupDiscontinuedStatus(details, keeper, deptCode, ecDeptScope, null);
    }

    /// <summary>
    /// 采购/受检同去重键下是否存在成品且未停产的根物料。有则执行内容留空，不按无关自动完成。
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="keeper">本次保留的明细</param>
    /// <param name="deptCode">部门编码（采购或受检）</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="knownRootKeys">工厂物料已建档的工厂+根物料键</param>
    /// <param name="nonFertRootKeys">类型不是 FERT 的工厂+根物料键</param>
    /// <returns>组内存在在产成品时为 true</returns>
    internal static bool HasLiveFertInPurchaseInspectGroup(
        IReadOnlyList<TaktEcDetail> details,
        TaktEcDetail keeper,
        string deptCode,
        int ecDeptScope,
        IReadOnlySet<string> knownRootKeys,
        IReadOnlySet<string> nonFertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(keeper);
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        ArgumentNullException.ThrowIfNull(knownRootKeys);
        ArgumentNullException.ThrowIfNull(nonFertRootKeys);
        if (deptCode is not (TaktEcDeptCodes.Mp or TaktEcDeptCodes.Iqc))
        {
            return false;
        }
        var kind = ResolveKind(deptCode, ecDeptScope);
        var key = BuildKey(keeper, kind);
        foreach (var item in details)
        {
            if (!IsEligible(item, deptCode, ecDeptScope, null))
            {
                continue;
            }
            if (!string.Equals(BuildKey(item, kind), key, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var rootKey = (item.PlantCode?.Trim() ?? string.Empty)
                + "\u001f"
                + (item.EcRootMaterialCode?.Trim() ?? string.Empty);
            if (!knownRootKeys.Contains(rootKey) || nonFertRootKeys.Contains(rootKey))
            {
                continue;
            }
            if (!TaktEcScopeConstants.IsEolDiscontinued(item.DiscontinuedStatus))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 每个去重键下的根物料编码组（分号分隔，不截断）
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>键为去重键，值为编码组</returns>
    internal static Dictionary<string, string> BuildRootGroups(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(details);
        var kind = ResolveKind(deptCode, ecScope);
        var buckets = new Dictionary<string, List<string?>>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in details)
        {
            if (!IsEligible(detail, deptCode, ecScope, fertRootKeys))
            {
                continue;
            }
            var key = BuildKey(detail, kind);
            if (!buckets.TryGetValue(key, out var codes))
            {
                codes = [];
                buckets[key] = codes;
            }
            codes.Add(detail.EcRootMaterialCode);
        }
        return buckets.ToDictionary(
            pair => pair.Key,
            pair => TaktEcRootMaterialCodeGroup.Join(pair.Value),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 每个去重键下的机种编码组（分号分隔，不截断）
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>键为去重键，值为机种编码组</returns>
    internal static Dictionary<string, string> BuildModelGroups(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        return BuildCodeGroups(details, deptCode, ecScope, fertRootKeys, detail => detail.EcModelCode);
    }

    /// <summary>
    /// 每个去重键下指定字段的编码组（分号分隔，不截断）
    /// </summary>
    /// <param name="details">同一设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <param name="pick">从明细取编码</param>
    /// <returns>键为去重键，值为编码组</returns>
    internal static Dictionary<string, string> BuildCodeGroups(
        IReadOnlyList<TaktEcDetail> details,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys,
        Func<TaktEcDetail, string?> pick)
    {
        ArgumentNullException.ThrowIfNull(details);
        ArgumentNullException.ThrowIfNull(pick);
        var kind = ResolveKind(deptCode, ecScope);
        var buckets = new Dictionary<string, List<string?>>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in details)
        {
            if (!IsEligible(detail, deptCode, ecScope, fertRootKeys))
            {
                continue;
            }
            var key = BuildKey(detail, kind);
            if (!buckets.TryGetValue(key, out var codes))
            {
                codes = [];
                buckets[key] = codes;
            }
            codes.Add(pick(detail));
        }
        return buckets.ToDictionary(
            pair => pair.Key,
            pair => TaktEcRootMaterialCodeGroup.Join(pair.Value),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 同键是否存在更大 Id 的可见明细
    /// </summary>
    /// <param name="detail">当前明细</param>
    /// <param name="siblings">同一设变未作废明细（含当前行）</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="fertRootKeys">制一按成品的 FERT 键</param>
    /// <returns>存在更大 Id 则 true</returns>
    internal static bool HasNewer(
        TaktEcDetail detail,
        IEnumerable<TaktEcDetail> siblings,
        string deptCode,
        int ecScope,
        IReadOnlySet<string>? fertRootKeys)
    {
        ArgumentNullException.ThrowIfNull(detail);
        ArgumentNullException.ThrowIfNull(siblings);
        if (deptCode is TaktEcDeptCodes.Mc or TaktEcDeptCodes.Smt)
        {
            if (!IsEligible(detail, deptCode, ecScope, fertRootKeys))
            {
                return false;
            }
            var lineKey = BuildLineMaterialKey(detail);
            foreach (var sibling in siblings)
            {
                if (sibling.Id <= detail.Id || !IsEligible(sibling, deptCode, ecScope, fertRootKeys))
                {
                    continue;
                }
                if (string.Equals(BuildLineMaterialKey(sibling), lineKey, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        if (!IsEligible(detail, deptCode, ecScope, fertRootKeys))
        {
            return false;
        }
        var kind = ResolveKind(deptCode, ecScope);
        var key = BuildKey(detail, kind);
        foreach (var sibling in siblings)
        {
            if (sibling.Id <= detail.Id || !IsEligible(sibling, deptCode, ecScope, fertRootKeys))
            {
                continue;
            }
            if (string.Equals(BuildKey(sibling, kind), key, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 工厂 + 根物料编码键
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>组合键</returns>
    internal static string RootKey(TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        return (detail.PlantCode?.Trim() ?? string.Empty) + "\u001f" + (detail.EcRootMaterialCode?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// 按成品只看当前明细：停产写 EOL；否则根物料类型为非 FERT 时写该类型。不看同组其它行。
    /// </summary>
    /// <param name="detail">本次保留的明细</param>
    /// <returns>EOL 或物料类型；不自动填写时为 null</returns>
    internal static string? FinishedProductExecSuffix(TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (TaktEcScopeConstants.IsEolDiscontinued(detail.DiscontinuedStatus))
        {
            return TaktEcScopeConstants.EolExecContent;
        }
        var materialType = detail.EcRootMaterialType?.Trim() ?? string.Empty;
        return IsNonFertRootMaterialType(materialType) ? materialType : null;
    }

    /// <summary>
    /// 根物料类型是否为非 FERT。空类型不算非 FERT。
    /// </summary>
    /// <param name="materialType">设变明细根物料类型</param>
    /// <returns>非 FERT 时为 true</returns>
    private static bool IsNonFertRootMaterialType(string? materialType)
    {
        var type = materialType?.Trim() ?? string.Empty;
        return type.Length > 0
            && !string.Equals(type, TaktEcSeizouikkaConstants.FertMaterialType, StringComparison.OrdinalIgnoreCase);
    }
}
