// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcTeStatusConstants.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主表 EcStatus 字典 logistics_manufacturing_ec_te_status 数值常量（自动回写用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变技术课主表 EcStatus（字典 logistics_manufacturing_ec_te_status）
/// </summary>
public static class TaktEcTeStatusConstants
{
    /// <summary>
    /// 发行
    /// </summary>
    public const int Issued = 1;

    /// <summary>
    /// 执行中（任一责任部门执行表有输入）
    /// </summary>
    public const int InProgress = 2;

    /// <summary>
    /// 完成（全部责任部门执行表均已填写）
    /// </summary>
    public const int Completed = 3;
}
