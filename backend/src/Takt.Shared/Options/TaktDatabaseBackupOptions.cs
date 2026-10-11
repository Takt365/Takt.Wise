// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktDatabaseBackupOptions.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份配置（凭据密钥、暂存根默认 wwwroot/Backup）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 数据库备份选项（appsettings DatabaseBackup 节）
/// </summary>
public class TaktDatabaseBackupOptions
{
    /// <summary>
    /// 配置节名
    /// </summary>
    public const string SectionName = "DatabaseBackup";

    /// <summary>
    /// 已废弃：本地路径由用户选择/创建，不再作为默认回填
    /// </summary>
    public string DefaultRoot { get; set; } = string.Empty;

    /// <summary>
    /// 已废弃：不再限制本地浏览/写入
    /// </summary>
    public List<string> AllowedRoots { get; set; } = new();

    /// <summary>
    /// SQL BACKUP 暂存根（FTP/客户端 staging）。空或 wwwroot/Backup → WebApi/wwwroot/Backup；远程 SQL 须 UNC
    /// </summary>
    public string FtpTempRoot { get; set; } = "wwwroot/Backup";

    /// <summary>
    /// 凭据可逆加密密钥（任意长度；勿提交真实生产密钥到公开仓库）
    /// </summary>
    public string CredentialProtectionKey { get; set; } = "Takt.DatabaseBackup.Dev.ChangeMe";
}
