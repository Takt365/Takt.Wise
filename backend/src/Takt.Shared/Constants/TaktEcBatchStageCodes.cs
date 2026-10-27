// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcBatchStageCodes.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变批次转置阶段编码（生管/采购/受检/部管/物料/制二/制一/品管/制技）
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
    /// 采购订单发行日期/采购订单号码（采购 Mp）
    /// </summary>
    public const string PurchaseOrder = "PurchaseOrder";
    /// <summary>
    /// 预定投入/预定批次（生管 Pc）
    /// </summary>
    public const string Scheduled = "Scheduled";
    /// <summary>
    /// 检验日期/受检单号（受检 Iqc）
    /// </summary>
    public const string IncomingInspection = "IncomingInspection";
    /// <summary>
    /// 出库日期/出库批次（部管 Mc）
    /// </summary>
    public const string Outbound = "Outbound";
    /// <summary>
    /// 出库日期/出库批次（物料）
    /// </summary>
    public const string ElectronicOutbound = "ElectronicOutbound";
    /// <summary>
    /// 生产日期/生产批次（制二 Pcba）
    /// </summary>
    public const string Mfg2Production = "PcbaProduction";
    /// <summary>
    /// 生产日期/生产批次（制一 Assy）
    /// </summary>
    public const string Mfg1Production = "AssyProduction";
    /// <summary>
    /// 检样日期（品管 Qa，仅日期）
    /// </summary>
    public const string SampleInspection = "SampleInspection";
    /// <summary>
    /// SOP 日期（制技 Te，仅日期）
    /// </summary>
    public const string TechSop = "TechSop";
    /// <summary>
    /// 转置表列顺序：生管、采购、受检、部管、物料、制二、制一、品管、制技
    /// </summary>
    public static readonly string[] TransposedOrder =
    [
        Scheduled, PurchaseOrder, IncomingInspection, Outbound, ElectronicOutbound, Mfg2Production, Mfg1Production, SampleInspection, TechSop
    ];
}
