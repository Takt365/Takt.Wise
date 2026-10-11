// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktLazyTreeHelper.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：懒加载树约定辅助（AdminDivision 已采用；Dept/Account/Menu 可对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 懒加载树约定（大规模树表，如行政区划 50k+ 节点）。
/// GetXxxTreeAsync / GetXxxTreeOptionsAsync 仅返回 parentId 的直接子节点一层：
/// 不整表加载、不递归构树；非叶子 Children 保持空/null，靠实体 IsLeaf（0/1）驱动前端展开与 loadData。
/// 后续 Dept、Account、Menu 等可对齐同一模式，勿再一次性拉全量树。
/// </summary>
public static class TaktLazyTreeHelper
{
    /// <summary>
    /// 将实体 IsLeaf（字典 sys_yes_no；0=否 1=是）转为 Ant Design Tree/TreeSelect 的 isLeaf。
    /// </summary>
    /// <param name="isLeaf">实体 IsLeaf（0/1）</param>
    /// <returns>是否叶子</returns>
    public static bool ToAntIsLeaf(int isLeaf) => isLeaf == 1;
}
