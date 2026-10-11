// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Code
// 文件名称：TaktCloneBackupModels.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：数据克隆备份/清空步骤模型（预览与执行结果）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Code;

/// <summary>
/// 目标表备份预览（克隆前备份窗口数据）
/// </summary>
public class TaktCloneTargetBackupPreview
{
    /// <summary>
    /// 目标物理表名
    /// </summary>
    public string TargetTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标公司编码（公司级克隆时有值）
    /// </summary>
    public string? TargetCompanyCode { get; set; }

    /// <summary>
    /// 即将备份的行数
    /// </summary>
    public int TargetRowCount { get; set; }

    /// <summary>
    /// 计划生成的备份表名（执行时按 UTC 时间戳生成）
    /// </summary>
    public string PlannedBackupTableName { get; set; } = string.Empty;

    /// <summary>
    /// 备份步骤说明
    /// </summary>
    public string BackupDescription { get; set; } = string.Empty;

    /// <summary>
    /// 清空步骤说明
    /// </summary>
    public string ClearDescription { get; set; } = string.Empty;

    /// <summary>
    /// 风险提示
    /// </summary>
    public string WarningMessage { get; set; } = string.Empty;
}

/// <summary>
/// 目标表备份与清空执行结果
/// </summary>
public class TaktCloneTargetBackupStepResult
{
    /// <summary>
    /// 实际备份表名
    /// </summary>
    public string BackupTableName { get; set; } = string.Empty;

    /// <summary>
    /// 备份行数
    /// </summary>
    public int BackedUpRowCount { get; set; }

    /// <summary>
    /// 清空行数
    /// </summary>
    public int ClearedRowCount { get; set; }

    /// <summary>
    /// 执行摘要
    /// </summary>
    public string SummaryMessage { get; set; } = string.Empty;
}
