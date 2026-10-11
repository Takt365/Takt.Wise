// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Database
// 文件名称：TaktTableCloneDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表数据克隆 DTO（一次最多 5 张表；克隆前先备份再清空目标表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Application.Dtos.Code.Database;

/// <summary>
/// 跨租户整表数据克隆请求 DTO（同租户内禁止；一次 1~5 张表）
/// </summary>
public class TaktTableCloneDto
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
    /// 待克隆表清单（1~5 张）
    /// </summary>
    [Required(ErrorMessage = "表清单不能为空")]
    public List<TaktTableCloneItemDto> Tables { get; set; } = new();

    /// <summary>
    /// 是否保留自增列原值（IDENTITY_INSERT）
    /// </summary>
    public bool PreserveIdentityValues { get; set; } = true;

    /// <summary>
    /// 已在备份窗口确认：目标表将先全量备份再 TRUNCATE 清空（执行克隆时必须为 true）
    /// </summary>
    public bool ConfirmTargetBackupAndClear { get; set; }
}

/// <summary>
/// 单张表的跨租户克隆项
/// </summary>
public class TaktTableCloneItemDto
{
    /// <summary>
    /// 源物理表名
    /// </summary>
    [Required(ErrorMessage = "源数据表不能为空")]
    public string SourceTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标物理表名
    /// </summary>
    [Required(ErrorMessage = "目标数据表不能为空")]
    public string TargetTableName { get; set; } = string.Empty;
}

/// <summary>
/// 跨租户整表克隆备份预览 DTO（备份窗口）
/// </summary>
public class TaktTableClonePreviewDto
{
    /// <summary>
    /// 总体提示
    /// </summary>
    public string SummaryMessage { get; set; } = string.Empty;

    /// <summary>
    /// 确认提示（执行克隆前须阅读并勾选 ConfirmTargetBackupAndClear）
    /// </summary>
    public string ConfirmHint { get; set; } = string.Empty;

    /// <summary>
    /// 各目标表备份/清空预览
    /// </summary>
    public List<TaktTableCloneTargetPreviewItemDto> Targets { get; set; } = new();
}

/// <summary>
/// 单张目标表备份预览项
/// </summary>
public class TaktTableCloneTargetPreviewItemDto
{
    /// <summary>
    /// 目标物理表名
    /// </summary>
    public string TargetTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标表现有行数
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
}

/// <summary>
/// 跨租户整表克隆批量结果 DTO
/// </summary>
public class TaktTableCloneResultDto
{
    /// <summary>
    /// 本次克隆表数量
    /// </summary>
    public int TableCount { get; set; }

    /// <summary>
    /// 源表行数合计
    /// </summary>
    public int TotalSourceRowCount { get; set; }

    /// <summary>
    /// 写入目标表行数合计
    /// </summary>
    public int TotalClonedRowCount { get; set; }

    /// <summary>
    /// 各表克隆明细
    /// </summary>
    public List<TaktTableCloneTableResultDto> Tables { get; set; } = new();
}

/// <summary>
/// 单张表克隆结果 DTO
/// </summary>
public class TaktTableCloneTableResultDto
{
    /// <summary>
    /// 源物理表名
    /// </summary>
    public string SourceTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标物理表名
    /// </summary>
    public string TargetTableName { get; set; } = string.Empty;

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
    /// 源表行数
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
