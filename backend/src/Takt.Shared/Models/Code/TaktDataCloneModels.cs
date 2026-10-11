// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Code
// 文件名称：TaktDataCloneModels.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆选项与结果模型（供 ITaktDataCloneProvider 使用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Code;

/// <summary>
/// 公司级数据克隆选项（源/目标租户、数据库、表、公司）
/// </summary>
public class TaktDataCloneOptions
{
    /// <summary>
    /// 源租户编码（3 位）
    /// </summary>
    public string SourceTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源数据库展示名
    /// </summary>
    public string SourceDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 源物理表名
    /// </summary>
    public string SourceTableName { get; set; } = string.Empty;

    /// <summary>
    /// 源公司编码（4 位）
    /// </summary>
    public string SourceCompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位）
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 目标物理表名
    /// </summary>
    public string TargetTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标公司编码（4 位）
    /// </summary>
    public string TargetCompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否保留自增列原值（IDENTITY_INSERT）
    /// </summary>
    public bool PreserveIdentityValues { get; set; } = true;
}

/// <summary>
/// 公司级数据克隆执行结果
/// </summary>
public class TaktDataCloneResult
{
    /// <summary>
    /// 源公司匹配行数
    /// </summary>
    public int SourceRowCount { get; set; }

    /// <summary>
    /// 实际写入目标表行数
    /// </summary>
    public int ClonedRowCount { get; set; }

    /// <summary>
    /// 参与映射的同名列数量
    /// </summary>
    public int CommonColumnCount { get; set; }

    /// <summary>
    /// 参与 INSERT 的同名列
    /// </summary>
    public IReadOnlyList<string> CommonColumns { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 源表存在但目标表未映射的列
    /// </summary>
    public IReadOnlyList<string> SkippedSourceColumns { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 目标表存在但源表未映射的列
    /// </summary>
    public IReadOnlyList<string> SkippedTargetColumns { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 备份表名
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
    /// 备份与清空摘要
    /// </summary>
    public string BackupSummaryMessage { get; set; } = string.Empty;
}
