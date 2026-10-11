// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktCostElementKatypConstants.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素类别与初级/次级类型推导常量
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 成本要素类别与类型（字典 accounting_controlling_cost_element_category / accounting_controlling_cost_element_type）
/// </summary>
public static class TaktCostElementKatypConstants
{
    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type）
    /// </summary>
    public static class Type
    {
        /// <summary>
        /// 初级成本要素（对应 FI 总账科目）
        /// </summary>
        public const int Primary = 0;
        /// <summary>
        /// 次级成本要素（仅 CO 内部）
        /// </summary>
        public const int Secondary = 1;
    }

    /// <summary>
    /// 初级 KATYP 整型值（DictValue 与实体 CostElementCategory 对齐）
    /// </summary>
    private static readonly HashSet<int> PrimaryKatypValues = new()
    {
        1, 3, 4, 11, 12, 22, 90
    };

    /// <summary>
    /// 全部有效 KATYP 整型值
    /// </summary>
    private static readonly HashSet<int> AllKatypValues = new()
    {
        1, 3, 4, 11, 12, 22, 90,
        21, 31, 41, 42, 43, 50, 51, 52, 61, 66
    };

    /// <summary>
    /// 由 KATYP 类别推导成本要素类型
    /// </summary>
    /// <param name="costElementCategory">成本要素类别（KATYP 整型）</param>
    /// <returns>0=初级，1=次级</returns>
    /// <exception cref="ArgumentOutOfRangeException">类别不在有效 KATYP 集合内</exception>
    public static int ResolveTypeFromCategory(int costElementCategory)
    {
        if (!AllKatypValues.Contains(costElementCategory))
        {
            throw new ArgumentOutOfRangeException(nameof(costElementCategory), costElementCategory, "无效的成本要素类别 KATYP");
        }
        return PrimaryKatypValues.Contains(costElementCategory) ? Type.Primary : Type.Secondary;
    }

    /// <summary>
    /// 判断是否为有效 KATYP 类别
    /// </summary>
    /// <param name="costElementCategory">成本要素类别</param>
    /// <returns>是否在种子字典定义范围内</returns>
    public static bool IsValidCategory(int costElementCategory) => AllKatypValues.Contains(costElementCategory);
}
