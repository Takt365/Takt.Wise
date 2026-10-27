// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcScopeConstants.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：实施范围 logistics_manufacturing_ec_impl_scope_category 与执行生成判定常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 实施范围（字典 logistics_manufacturing_ec_impl_scope_category）及部门执行生成判定常量。
/// 执行内容：全仕向空白待填；内部自动「实施范围-内部」；技术自动「实施范围-技术」；部管时制一/制二/品管自动「实施范围-部管」。根物料编码组全部停产写「全仕向:按机种:EOL」这类文案；整组都不是 FERT 时后缀改为工厂物料类型（如 HALB、ROH）。实施范围与部门实施取实际值，段间用英文冒号。
/// 部门去重粒度见 TaktEcControlScopeConstants（1=按成品，2=按机种，3=按设变）。
/// </summary>
public static class TaktEcScopeConstants
{
    /// <summary>
    /// 全仕向
    /// </summary>
    public const int AllDestination = 1;

    /// <summary>
    /// 部管
    /// </summary>
    public const int MaterialControl = 2;

    /// <summary>
    /// 内部
    /// </summary>
    public const int Internal = 3;

    /// <summary>
    /// 技术
    /// </summary>
    public const int Technical = 4;

    /// <summary>
    /// 新采购类型：外部采购（需采购/受检填写）
    /// </summary>
    public const string PurchaseTypeExternal = "F";

    /// <summary>
    /// 新品仓库：原料电子保税仓等（实施范围=部管时走制造二课门禁）
    /// </summary>
    public const string NewWarehousePcbaGate = "C003";

    /// <summary>
    /// 计划物料停产状态（字典 logistics_materials_material_discontinued_status；非此值视为 EOL）
    /// </summary>
    public const string PlannedMaterialStatus = "Z0";

    /// <summary>
    /// 停产操作默认写入的根物料停产状态（字典 logistics_materials_material_discontinued_status；生产结束）
    /// </summary>
    public const string EolMaterialStatus = "ZQ";

    /// <summary>
    /// 执行内容标准前缀
    /// </summary>
    public const string ExecContentPrefix = "实施范围-";

    /// <summary>
    /// 按成品停产文案前缀（按成品须空白由各部门填写；仅用于 EOL 与识别旧值）
    /// </summary>
    public const string ByFinishedProductExecContent = ExecContentPrefix + "按成品";

    /// <summary>
    /// 按机种停产文案前缀
    /// </summary>
    public const string ByModelExecContent = ExecContentPrefix + "按机种";

    /// <summary>
    /// 实施范围=部管时，制一/制二/品管的自动执行内容。停产写「部管:按机种:EOL」
    /// </summary>
    public const string MaterialControlExecContent = ExecContentPrefix + "部管";

    /// <summary>
    /// 实施范围=内部时各部门自动执行内容。停产写「内部:按设变:EOL」
    /// </summary>
    public const string InternalExecContent = ExecContentPrefix + "内部";

    /// <summary>
    /// 实施范围=技术时各部门自动执行内容。停产写「技术:按设变:EOL」
    /// </summary>
    public const string TechnicalExecContent = ExecContentPrefix + "技术";

    /// <summary>
    /// 历史自动完成文案；再生成时替换为实施范围文案
    /// </summary>
    public const string AutoCompletedExecContent = "系统自动完成";

    /// <summary>
    /// 实施范围显示名（全仕向 / 部管 / 内部 / 技术）
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>显示名</returns>
    public static string ResolveScopeLabel(int ecImplScope)
    {
        return ecImplScope switch
        {
            AllDestination => "全仕向",
            MaterialControl => "部管",
            Internal => "内部",
            Technical => "技术",
            _ => "技术",
        };
    }

    /// <summary>
    /// 执行内容：实施范围:部门实施:后缀。段间为英文冒号。后缀为 EOL、物料类型（HALB、ROH）或 N/A。
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <param name="ecControlScope">部门实施范围</param>
    /// <param name="suffix">EOL 或物料类型</param>
    /// <returns>执行内容</returns>
    public static string FormatScopeDeptExecContent(int ecImplScope, int ecControlScope, string suffix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(suffix);
        return $"{ResolveScopeLabel(ecImplScope)}:{TaktEcControlScopeConstants.ResolveLabel(ecControlScope)}:{suffix.Trim()}";
    }

    /// <summary>
    /// 拆出「实施范围:部门实施:后缀」。
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <param name="suffix">EOL 或物料类型</param>
    /// <returns>是否为该格式</returns>
    public static bool TryReadScopeDeptSuffix(string? content, out string suffix)
    {
        suffix = string.Empty;
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        var value = content.Trim();
        var parts = value.Split(':');
        if (parts.Length != 3)
        {
            return false;
        }
        if (!IsScopeLabel(parts[0]) || !IsDeptScopeLabel(parts[1]))
        {
            return false;
        }
        suffix = parts[2].Trim();
        return suffix.Length > 0;
    }

    /// <summary>
    /// 是否为实施范围显示名
    /// </summary>
    /// <param name="label">显示名</param>
    /// <returns>是否匹配</returns>
    private static bool IsScopeLabel(string label)
    {
        return label is "全仕向" or "部管" or "内部" or "技术";
    }

    /// <summary>
    /// 是否为部门实施显示名
    /// </summary>
    /// <param name="label">显示名</param>
    /// <returns>是否匹配</returns>
    private static bool IsDeptScopeLabel(string label)
    {
        return label is "按成品" or "按机种" or "按设变";
    }

    /// <summary>
    /// 是否为按物料类型生成的执行内容，如「全仕向:按成品:HALB」。
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>是否物料类型文案</returns>
    public static bool IsDeptMaterialTypeExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        var value = content.Trim();
        if (!TryReadScopeDeptSuffix(value, out var materialType))
        {
            return false;
        }
        return materialType.Length > 0
            && !string.Equals(materialType, EolExecContent, StringComparison.OrdinalIgnoreCase)
            && IsMaterialTypeSuffix(materialType);
    }

    /// <summary>
    /// 物料类型后缀：HALB、ROH，或 HALB/ROH
    /// </summary>
    /// <param name="suffix">第三段</param>
    /// <returns>是否为物料类型</returns>
    private static bool IsMaterialTypeSuffix(string suffix)
    {
        var parts = suffix.Split('/');
        if (parts.Length == 0)
        {
            return false;
        }
        foreach (var part in parts)
        {
            if (part.Length is < 1 or > 4)
            {
                return false;
            }
            foreach (var ch in part)
            {
                if (!char.IsAsciiLetterOrDigit(ch))
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 停产后缀。完整文案为「全仕向:按机种:EOL」
    /// </summary>
    public const string EolExecContent = "EOL";

    /// <summary>
    /// 尚未填写时的执行内容后缀
    /// </summary>
    public const string NotApplicableExecSuffix = "N/A";

    /// <summary>
    /// 是否为停产自动文案（「全仕向:按机种:EOL」）
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>是否 EOL 文案</returns>
    public static bool IsEolExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }
        return TryReadScopeDeptSuffix(content.Trim(), out var suffix)
            && string.Equals(suffix, EolExecContent, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 新物料编码是否有效（空或占位「0」视为无新物料；采购/受检/部管不得因此生成执行行）
    /// </summary>
    /// <param name="ecNewMaterialCode">新物料编码</param>
    /// <returns>是否有效新物料</returns>
    public static bool HasEffectiveNewMaterialCode(string? ecNewMaterialCode)
    {
        var code = ecNewMaterialCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(code))
        {
            return false;
        }
        return !string.Equals(code, "0", StringComparison.Ordinal);
    }

    /// <summary>
    /// 是否采购/受检/部管课（依赖有效新物料编码才生成执行行）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>是否为上述三课</returns>
    public static bool IsNewMaterialDependentDept(string? deptCode)
    {
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pur => true,
            TaktEcDeptCodes.Iqc => true,
            TaktEcDeptCodes.Mc => true,
            _ => false
        };
    }

    /// <summary>
    /// 是否按停产状态视为 EOL（空或 Z0 为否）
    /// </summary>
    /// <param name="discontinuedStatus">停产状态</param>
    /// <returns>是否 EOL</returns>
    public static bool IsEolDiscontinued(string? discontinuedStatus)
    {
        var status = discontinuedStatus?.Trim();
        if (string.IsNullOrEmpty(status))
        {
            return false;
        }
        return !string.Equals(status, PlannedMaterialStatus, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否外部采购类型 F（采购课列表可见、采购/受检待填）
    /// </summary>
    /// <param name="purchaseType">采购类型（空视为否）</param>
    /// <returns>是否为 F</returns>
    public static bool IsExternalPurchaseType(string? purchaseType)
    {
        return string.Equals(
            purchaseType?.Trim(),
            PurchaseTypeExternal,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 新品仓库是否为制二门禁仓 C003
    /// </summary>
    /// <param name="warehouse">新品仓库编码</param>
    /// <returns>是否为 C003</returns>
    public static bool IsPcbaGateWarehouse(string? warehouse)
    {
        return string.Equals(
            warehouse?.Trim(),
            NewWarehousePcbaGate,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 部管课是否需人工填写（新采购类型 F 且新品仓库非 C003）
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <param name="newWarehouse">新品仓库</param>
    /// <returns>是否部管课可见待填</returns>
    public static bool IsMcVisible(string? purchaseType, string? newWarehouse)
    {
        return IsExternalPurchaseType(purchaseType) && !IsPcbaGateWarehouse(newWarehouse);
    }

    /// <summary>
    /// 制二课 C003 页签：新采购类型 F 且新品仓库为 C003
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <param name="newWarehouse">新品仓库</param>
    /// <returns>是否属于制二 C003 页签</returns>
    public static bool IsPcbaC003ExternalGroup(string? purchaseType, string? newWarehouse)
    {
        return IsExternalPurchaseType(purchaseType) && IsPcbaGateWarehouse(newWarehouse);
    }

    /// <summary>
    /// 制二课其它页签：新采购类型不是 F
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <returns>是否属于制二 F以外页签</returns>
    public static bool IsPcbaOtherPurchaseGroup(string? purchaseType)
    {
        return !IsExternalPurchaseType(purchaseType);
    }

    /// <summary>
    /// 该部门是否整行自动填写。停产判断不在此处：调用方须先按 DiscontinuedStatus（空或 Z0 为在产，其余为停产）写入 EOL。
    /// 全仕向不自动；制技始终不自动（按机种）；内部/技术除制技外自动；部管时仅非待填部门（制一/制二/品管）自动。自动填写时实施标记写已实施。
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <param name="deptCode">部门编码</param>
    /// <param name="purchaseType">新采购类型（部管待填名单不使用）</param>
    /// <param name="newWarehouse">新品仓库（部管待填名单不使用）</param>
    /// <returns>该部门在此实施范围下由系统填写执行内容，并写成已实施</returns>
    public static bool ShouldAutoCompleteExec(
        int ecImplScope,
        string? deptCode,
        string? purchaseType = null,
        string? newWarehouse = null)
    {
        if (ecImplScope == AllDestination || deptCode == TaktEcDeptCodes.Me)
        {
            return false;
        }
        if (ecImplScope == Internal || ecImplScope == Technical)
        {
            return true;
        }
        if (ecImplScope == MaterialControl)
        {
            return !IsMaterialControlNeedFillDept(deptCode, purchaseType, newWarehouse);
        }
        return false;
    }

    /// <summary>
    /// 按实施范围解析自动执行内容：内部→「实施范围-内部」；技术→「实施范围-技术」；部管→「实施范围-部管」（仅自动部门使用）；全仕向→空
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>自动填写文案；按成品/按机种返回空串</returns>
    public static string ResolveAutoExecContent(int ecImplScope)
    {
        return ecImplScope switch
        {
            AllDestination => string.Empty,
            MaterialControl => MaterialControlExecContent,
            Internal => InternalExecContent,
            Technical => TechnicalExecContent,
            _ => string.Empty
        };
    }

    /// <summary>
    /// 全仕向，或部管（部分部门仍人工填写）时，允许写入「无关」类执行内容
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <returns>是否允许无关补齐</returns>
    public static bool AllowsNotRelatedFill(int ecImplScope)
    {
        return ecImplScope == AllDestination
            || ecImplScope == MaterialControl;
    }

    /// <summary>
    /// 实施范围=部管时，须人工填写执行内容的部门（生管/采购/受检/部管/SMT）；制一/制二/品管自动写「实施范围-部管」
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="purchaseType">新采购类型（保留参数；部管待填名单不再按此过滤）</param>
    /// <param name="newWarehouse">新品仓库（保留参数；部管待填名单不再按此过滤）</param>
    /// <returns>是否待填</returns>
    public static bool IsMaterialControlNeedFillDept(
        string? deptCode,
        string? purchaseType = null,
        string? newWarehouse = null)
    {
        _ = purchaseType;
        _ = newWarehouse;
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return false;
        }
        return deptCode switch
        {
            TaktEcDeptCodes.Pc => true,
            TaktEcDeptCodes.Pur => true,
            TaktEcDeptCodes.Iqc => true,
            TaktEcDeptCodes.Mc => true,
            TaktEcDeptCodes.Mtl => true,
            _ => false
        };
    }

    /// <summary>
    /// 是否为系统按当前实施范围写入的执行内容（可被再生成覆盖）。
    /// </summary>
    /// <param name="content">执行内容</param>
    /// <returns>是否为自动生成文案</returns>
    public static bool IsScopeGeneratedExecContent(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return true;
        }
        var value = content.Trim();
        return string.Equals(value, AutoCompletedExecContent, StringComparison.Ordinal)
            || string.Equals(value, ByFinishedProductExecContent, StringComparison.Ordinal)
            || string.Equals(value, ByModelExecContent, StringComparison.Ordinal)
            || string.Equals(value, ExecContentPrefix + "全仕向", StringComparison.Ordinal)
            || string.Equals(value, MaterialControlExecContent, StringComparison.Ordinal)
            || string.Equals(value, InternalExecContent, StringComparison.Ordinal)
            || string.Equals(value, TechnicalExecContent, StringComparison.Ordinal)
            || string.Equals(value, TaktEcPurConstants.NotPurchasingRelatedExecContent, StringComparison.Ordinal)
            || string.Equals(value, TaktEcIqcConstants.NotRelatedToIqcExecContent, StringComparison.Ordinal)
            || string.Equals(value, TaktEcMcConstants.NotRelatedToMaterialControlExecContent, StringComparison.Ordinal)
            || IsDeptMaterialTypeExecContent(value)
            || IsEolExecContent(value);
    }
}
