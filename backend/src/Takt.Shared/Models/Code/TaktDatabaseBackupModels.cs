// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Code
// 文件名称：TaktDatabaseBackupModels.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份 Provider 选项、结果与目录浏览模型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Code;

/// <summary>
/// 数据库备份执行选项
/// </summary>
public class TaktDatabaseBackupOptionsModel
{
    /// <summary>
    /// 目标租户
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库名
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型：1=Full 2=Delta
    /// </summary>
    public int BackupType { get; set; } = 1;

    /// <summary>
    /// 路径类型：1=本地(服务器) 2=文件服务器 3=FTP 4=客户端
    /// </summary>
    public int BackupPathType { get; set; } = 4;

    /// <summary>
    /// 备份目录（服务器本地/UNC/FTP 远程目录；客户端为目录标识，实际落盘见临时目录）
    /// </summary>
    public string BackupDirectory { get; set; } = string.Empty;

    /// <summary>
    /// 备份文件名（含 .bak）
    /// </summary>
    public string BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 主机（FTP/网络）
    /// </summary>
    public string? BackupHost { get; set; }

    /// <summary>
    /// 端口（FTP）
    /// </summary>
    public int? BackupPort { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? BackupUserName { get; set; }

    /// <summary>
    /// 明文密码（仅执行时）
    /// </summary>
    public string? BackupPassword { get; set; }

    /// <summary>
    /// 暂存根（默认 wwwroot/Backup；远程 SQL 须 UNC）
    /// </summary>
    public string? FtpTempRoot { get; set; }
}

/// <summary>
/// 数据库备份执行结果
/// </summary>
public class TaktDatabaseBackupResult
{
    /// <summary>
    /// 目标数据库
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 实际备份文件路径（本地/UNC/远程）
    /// </summary>
    public string BackupFilePath { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型
    /// </summary>
    public int BackupType { get; set; }

    /// <summary>
    /// 文件大小字节
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 目录浏览条目
/// </summary>
public class TaktDatabaseBackupBrowseItem
{
    /// <summary>
    /// 显示名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 完整路径
    /// </summary>
    public string FullPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否目录
    /// </summary>
    public bool IsDirectory { get; set; } = true;

    /// <summary>
    /// 最后修改时间（可空）
    /// </summary>
    public DateTime? ModifiedTime { get; set; }
}

/// <summary>
/// 目录浏览结果
/// </summary>
public class TaktDatabaseBackupBrowseResult
{
    /// <summary>
    /// 当前路径
    /// </summary>
    public string CurrentPath { get; set; } = string.Empty;

    /// <summary>
    /// 父路径（可空表示已到根）
    /// </summary>
    public string? ParentPath { get; set; }

    /// <summary>
    /// 子目录列表
    /// </summary>
    public List<TaktDatabaseBackupBrowseItem> Items { get; set; } = new();
}
