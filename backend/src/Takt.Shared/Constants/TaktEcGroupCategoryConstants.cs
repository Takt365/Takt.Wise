// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcGroupCategoryConstants.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变组类别 logistics_manufacturing_ec_group_category（实施顺序通知收件人）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变组类别（字典 logistics_manufacturing_ec_group_category）。按类别取 TaktEcGroup.ContactEmail 派发实施顺序邮件。
/// </summary>
public static class TaktEcGroupCategoryConstants
{
    /// <summary>
    /// EC技术担当
    /// </summary>
    public const int EcOwner = 1;

    /// <summary>
    /// SOP制技担当
    /// </summary>
    public const int Sop = 2;

    /// <summary>
    /// EC生管担当
    /// </summary>
    public const int Pmc = 3;

    /// <summary>
    /// EC采购担当
    /// </summary>
    public const int Mp = 4;

    /// <summary>
    /// EC部管担当
    /// </summary>
    public const int Mc = 5;

    /// <summary>
    /// EC受检担当
    /// </summary>
    public const int Iqc = 6;

    /// <summary>
    /// EC SMT担当
    /// </summary>
    public const int Smt = 7;

    /// <summary>
    /// EC制二担当
    /// </summary>
    public const int Pcba = 8;

    /// <summary>
    /// EC制一担当
    /// </summary>
    public const int Assy = 9;

    /// <summary>
    /// EC品管担当
    /// </summary>
    public const int Qa = 10;

    /// <summary>
    /// 类别对应的执行部门编码
    /// </summary>
    /// <param name="category">设变组类别</param>
    /// <returns>部门编码；未知类别为空</returns>
    public static string? ToDeptCode(int category) => category switch
    {
        EcOwner => TaktEcDeptCodes.Eng,
        Sop => TaktEcDeptCodes.Te,
        Pmc => TaktEcDeptCodes.Pmc,
        Mp => TaktEcDeptCodes.Mp,
        Mc => TaktEcDeptCodes.Mc,
        Iqc => TaktEcDeptCodes.Iqc,
        Smt => TaktEcDeptCodes.Smt,
        Pcba => TaktEcDeptCodes.Pcba,
        Assy => TaktEcDeptCodes.Assy,
        Qa => TaktEcDeptCodes.Qa,
        _ => null,
    };
}
