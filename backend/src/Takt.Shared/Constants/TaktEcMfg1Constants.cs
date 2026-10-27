// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcMfg1Constants.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制一课执行常量（根物料非 FERT 时的默认批次文案）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变制一课（TaktEcMfg1）执行常量
/// </summary>
public static class TaktEcMfg1Constants
{
    /// <summary>
    /// 成品物料类型（字典 logistics_materials_material_type）
    /// </summary>
    public const string FertMaterialType = "FERT";

    /// <summary>
    /// 根物料类型不是 FERT 时，预定批次与实施批次的默认文案
    /// </summary>
    public const string NotRelatedBatch = "<制一课无关>";

    /// <summary>
    /// 批次文案是否为制一课无关
    /// </summary>
    /// <param name="batch">预定批次或实施批次</param>
    /// <returns>是否无关文案</returns>
    public static bool IsNotRelatedBatch(string? batch)
    {
        return string.Equals(batch?.Trim(), NotRelatedBatch, StringComparison.Ordinal);
    }
}
