// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Logistics.Manufacturing
// 文件名称：TaktSignalREcGijutsuPersistPush.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主表后台保存完成 SignalR 推送模型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Logistics.Manufacturing;

/// <summary>
/// 设变技术课主表后台保存完成推送模型
/// </summary>
public class TaktSignalREcGijutsuPersistPush
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
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否为更新
    /// </summary>
    public bool IsUpdate { get; set; }

    /// <summary>
    /// 主表主键（失败时可能为 0）
    /// </summary>
    public long EcGijutsuId { get; set; }

    /// <summary>
    /// 明细行数
    /// </summary>
    public int DetailCount { get; set; }

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
    /// 完成时间
    /// </summary>
    public DateTime CompletedAt { get; set; }
}
