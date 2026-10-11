// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Logistics.Manufacturing
// 文件名称：TaktSignalRBomMaterialCostItemRecalculatePush.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种月平均重算完成 SignalR 推送模型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Logistics.Manufacturing;

/// <summary>
/// BOM 物料成本机种月平均重算完成推送模型
/// </summary>
public class TaktSignalRBomMaterialCostItemRecalculatePush
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 触发用户名
    /// </summary>
    public string TriggerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 核算月份（YYYY-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;

    /// <summary>
    /// 是否为重置并重算
    /// </summary>
    public bool ForceRecalculate { get; set; }

    /// <summary>
    /// 执行状态（对齐 TaktExecuteStatus：1 成功 / 2 失败）
    /// </summary>
    public int ExecuteStatus { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 失败时的错误摘要
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 扫描 BOM 行数
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// 重算维度组数
    /// </summary>
    public int RefreshedGroupCount { get; set; }

    /// <summary>
    /// 跳过维度组数
    /// </summary>
    public int SkippedGroupCount { get; set; }

    /// <summary>
    /// 重置维度组数
    /// </summary>
    public int ResetGroupCount { get; set; }

    /// <summary>
    /// 涉及核算月份数
    /// </summary>
    public int ProcessedMonthCount { get; set; }

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime CompletedAt { get; set; }
}
