// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Database
// 文件名称：TaktDataCloneDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆 DTO（一次一公司一表；克隆前先备份再清空目标公司数据）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Application.Dtos.Code.Database;

/// <summary>
/// 公司级数据克隆请求 DTO（一次仅一个源公司、一张源表 → 一个目标公司、一张目标表）
/// </summary>
public class TaktDataCloneDto
{
    /// <summary>
    /// 源租户编码（3 位）
    /// </summary>
    [Required(ErrorMessage = "源租户编码不能为空")]
    public string SourceTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源数据库展示名
    /// </summary>
    [Required(ErrorMessage = "源数据库不能为空")]
    public string SourceDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 源物理表名
    /// </summary>
    [Required(ErrorMessage = "源数据表不能为空")]
    public string SourceTableName { get; set; } = string.Empty;

    /// <summary>
    /// 源公司编码（4 位）
    /// </summary>
    [Required(ErrorMessage = "源公司编码不能为空")]
    public string SourceCompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位）
    /// </summary>
    [Required(ErrorMessage = "目标租户不能为空")]
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    [Required(ErrorMessage = "目标数据库不能为空")]
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 目标物理表名
    /// </summary>
    [Required(ErrorMessage = "目标数据表不能为空")]
    public string TargetTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标公司编码（4 位）
    /// </summary>
    [Required(ErrorMessage = "目标公司编码不能为空")]
    public string TargetCompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否保留自增列原值（IDENTITY_INSERT）
    /// </summary>
    public bool PreserveIdentityValues { get; set; } = true;

    /// <summary>
    /// 已在备份窗口确认：目标公司将先备份再清空（执行克隆时必须为 true）
    /// </summary>
    public bool ConfirmTargetBackupAndClear { get; set; }
}

/// <summary>
/// 公司级数据克隆备份预览 DTO（备份窗口）
/// </summary>
public class TaktDataClonePreviewDto
{
    /// <summary>
    /// 目标物理表名
    /// </summary>
    public string TargetTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标公司编码
    /// </summary>
    public string TargetCompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标公司现有行数
    /// </summary>
    public int TargetRowCount { get; set; }

    /// <summary>
    /// 计划备份表名
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

    /// <summary>
    /// 确认提示
    /// </summary>
    public string ConfirmHint { get; set; } = string.Empty;
}

/// <summary>
/// 公司级数据克隆结果 DTO
/// </summary>
public class TaktDataCloneResultDto
{
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
    public List<string> CommonColumns { get; set; } = new();

    /// <summary>
    /// 源表存在但目标表未映射的列
    /// </summary>
    public List<string> SkippedSourceColumns { get; set; } = new();

    /// <summary>
    /// 目标表存在但源表未映射的列
    /// </summary>
    public List<string> SkippedTargetColumns { get; set; } = new();
}
