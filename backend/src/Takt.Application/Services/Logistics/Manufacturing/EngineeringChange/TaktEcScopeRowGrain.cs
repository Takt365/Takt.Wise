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
        var byEc = ecDeptScope == TaktEcDeptScopeConstants.ByEngineeringChange;
        return deptCode switch
        {
            TaktEcDeptCodes.Pmc => finished
                ? TaktEcScopeDedupKind.ModelRoot
                : byModel
                    ? TaktEcScopeDedupKind.ModelGroup
                    : TaktEcScopeDedupKind.ModelOnly,
            TaktEcDeptCodes.Mc or TaktEcDeptCodes.Smt => finished
                ? TaktEcScopeDedupKind.ModelRootNew
                : byModel
                    ? TaktEcScopeDedupKind.ModelNewGroup
                    : TaktEcScopeDedupKind.ModelOnly,
            TaktEcDeptCodes.Pcba or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa => finished
                ? TaktEcScopeDedupKind.EcRootGroup
                : byModel
                    ? TaktEcScopeDedupKind.ModelGroup
                    : TaktEcScopeDedupKind.EcOnlyGroup,
            TaktEcDeptCodes.Mp => TaktEcScopeDedupKind.PurchaseNewGroup,
            TaktEcDeptCodes.Iqc => TaktEcScopeDedupKind.PurchaseInspectNewGroup,
            _ => finished || byModel
                ? TaktEcScopeDedupKind.ModelRoot
                : TaktEcScopeDedupKind.ModelOnly,
        };
    }

    /// <summary>
    /// 明细是否参与该部门本范围的执行行（不含组内去重）
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
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
        var kind = ResolveKind(deptCode, ecScope);
        if (kind == TaktEcScopeDedupKind.ModelOnly)
        {
            return true;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Mc => TaktEcScopeConstants.IsBukanVisible(detail.EcNewPurchaseType, detail.EcNewWarehouse)
                && TaktEcScopeConstants.HasEffectiveNewMaterialCode(detail.EcNewMaterialCode),
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
}
