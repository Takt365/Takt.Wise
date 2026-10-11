// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Code
// 文件名称：TaktTableArchiveModels.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：同库按年数据归档选项与结果模型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Code;

/// <summary>
/// 单表按年归档执行选项
/// </summary>
public class TaktTableArchiveOptions
{
    /// <summary>
    /// 目标租户（3 位）
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 源物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名
    /// </summary>
    public string ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等；默认 3）
    /// </summary>
    public int ArchiveKeyKind { get; set; } = 3;

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 公司编码过滤（4 位；热表含 company_code 时生效）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;
}

/// <summary>
/// 归档预览结果
/// </summary>
public class TaktTableArchivePreview
{
    /// <summary>
    /// 源物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标表名
    /// </summary>
    public string ArchiveTableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 将迁移行数
    /// </summary>
    public int SourceRowCount { get; set; }
}

/// <summary>
/// 归档执行结果
/// </summary>
public class TaktTableArchiveResult
{
    /// <summary>
    /// 源物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标表名
    /// </summary>
    public string ArchiveTableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配行数
    /// </summary>
    public int SourceRowCount { get; set; }

    /// <summary>
    /// 迁移行数（DELETE OUTPUT 影响行）
    /// </summary>
    public int ArchivedRowCount { get; set; }

    /// <summary>
    /// 从热表删除行数（与 ArchivedRowCount 相同）
    /// </summary>
    public int DeletedRowCount { get; set; }
}
