// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcDeptCodes.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变责任部门编码常量，与 TaktDeptSeedData.DeptCode 及各部门执行表 DeptCode 对齐
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变责任部门编码（TaktEcExec* 表 DeptCode，5 位；来源 TaktDeptSeedData）。
/// 常量名保留业务简称；值为组织部门 DeptCode。
/// </summary>
public static class TaktEcDeptCodes
{
    /// <summary>
    /// 技术课（D0710）
    /// </summary>
    public const string Eng = "D0710";
    /// <summary>
    /// 生管课（D0420）
    /// </summary>
    public const string Pmc = "D0420";
    /// <summary>
    /// 采购课（D0510）
    /// </summary>
    public const string Mp = "D0510";
    /// <summary>
    /// 受检课（D0810）
    /// </summary>
    public const string Iqc = "D0810";
    /// <summary>
    /// 部管课（D0430）
    /// </summary>
    public const string Mc = "D0430";
    /// <summary>
    /// 制造2课-物料 / SMT（D0625；TaktEcSmt，F+C003）
    /// </summary>
    public const string Smt = "D0625";
    /// <summary>
    /// 制造2课（D0620；TaktEcSeizounika 非 F；看板/派生「制二」桶）
    /// </summary>
    public const string Pcba = "D0620";
    /// <summary>
    /// 制二课非F（TaktEcSeizounika）左栏主表过滤键；非组织 DeptCode，仅 GetEcDetailMasterListAsync
    /// </summary>
    public const string SeizounikaMaster = "SEIZOUNIKA";
    /// <summary>
    /// 制造1课（D0610）
    /// </summary>
    public const string Assy = "D0610";
    /// <summary>
    /// 品管课（D0820）
    /// </summary>
    public const string Qa = "D0820";
    /// <summary>
    /// 制造技术课（D0630）
    /// </summary>
    public const string Te = "D0630";
    /// <summary>
    /// 正式完成判定部门（品管课 D0820；全部明细已实施后设变视为正式完成）
    /// </summary>
    public const string OfficialCompletionDeptCode = Qa;
    /// <summary>
    /// 阶段二执行部门看板列顺序（不含技术课；技术工作在阶段一 TaktEcGijutsu 完成）
    /// </summary>
    public static readonly string[] KanbanOrder =
    [
        Pmc, Mp, Iqc, Mc, Pcba, Assy, Qa, Te
    ];
    /// <summary>
    /// 转置表格列顺序（采购→生管→受检→部管→制二→制一→品管；不含技术课与制技）
    /// </summary>
    public static readonly string[] TransposedOrder =
    [
        Mp, Pmc, Iqc, Mc, Pcba, Assy, Qa
    ];
    /// <summary>
    /// 来源设变导入后初始化部门执行行顺序（品管→采购→生管→制技→制一→制二→受检；不含部管课）
    /// </summary>
    public static readonly string[] SourceImportDeptOrder =
    [
        Qa, Mp, Pmc, Te, Assy, Pcba, Iqc
    ];

    /// <summary>
    /// 部门编码对应的中文显示名（日志/站内消息）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>显示名；未知编码时回退为编码本身</returns>
    public static string GetDisplayName(string? deptCode)
    {
        if (string.IsNullOrWhiteSpace(deptCode))
        {
            return string.Empty;
        }
        return deptCode.Trim() switch
        {
            Eng => "技术课",
            Pmc => "生管课",
            Mp => "采购课",
            Iqc => "受检课",
            Mc => "部管课",
            Smt => "制造2课-物料",
            Pcba => "制造2课",
            Assy => "制造一课",
            Qa => "品管课",
            Te => "制造技术课",
            _ => deptCode.Trim(),
        };
    }
}
