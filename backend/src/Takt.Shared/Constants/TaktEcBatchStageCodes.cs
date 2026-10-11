// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcBatchStageCodes.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变批次转置阶段编码（预定投入/出库/制二生产/制一生产/检样）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 设变批次转置阶段（列顺序与业务截图一致）
/// </summary>
public static class TaktEcBatchStageCodes
{
    /// <summary>
    /// 预定投入/预定批次（生管 Pmc）
    /// </summary>
    public const string Scheduled = "Scheduled";
    /// <summary>
    /// 出库日期/出库批次（部管 Mc）
    /// </summary>
    public const string Outbound = "Outbound";
    /// <summary>
    /// 生产日期/生产批次（制二 Pcba）
    /// </summary>
    public const string PcbaProduction = "PcbaProduction";
    /// <summary>
    /// 生产日期/生产批次（制一 Assy）
    /// </summary>
    public const string AssyProduction = "AssyProduction";
    /// <summary>
    /// 检样日期（品管 Qa，仅日期）
    /// </summary>
    public const string SampleInspection = "SampleInspection";
    /// <summary>
    /// 转置表列顺序
    /// </summary>
    public static readonly string[] TransposedOrder =
    [
        Scheduled, Outbound, PcbaProduction, AssyProduction, SampleInspection
    ];
}
