// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktDatabaseBackupProvider.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：SQL Server 数据库备份 Provider 契约（Full / Differential）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Code;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 数据库备份 Provider
/// </summary>
public interface ITaktDatabaseBackupProvider
{
    /// <summary>
    /// 执行 BACKUP DATABASE（Full 或 Differential）
    /// </summary>
    /// <param name="options">备份选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>备份结果</returns>
    Task<TaktDatabaseBackupResult> BackupAsync(
        TaktDatabaseBackupOptionsModel options,
        CancellationToken cancellationToken = default);
}
