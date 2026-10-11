// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：ITaktDatabaseBackupService.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份应用服务接口（立即/调度备份，非 CRUD 创建更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Database;
using Takt.Shared.Models;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 数据库备份应用服务接口
/// </summary>
public interface ITaktDatabaseBackupService
{
    /// <summary>
    /// 获取数据库备份列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktDatabaseBackupDto>> GetDatabaseBackupListAsync(TaktDatabaseBackupQueryDto queryDto);

    /// <summary>
    /// 根据ID获取数据库备份
    /// </summary>
    /// <param name="id">数据库备份ID</param>
    /// <returns>DTO</returns>
    Task<TaktDatabaseBackupDto?> GetDatabaseBackupByIdAsync(long id);

    /// <summary>
    /// 获取备份路径选项（默认根目录与白名单）
    /// </summary>
    /// <returns>路径选项</returns>
    TaktDatabaseBackupPathOptionsDto GetDatabaseBackupPathOptions();

    /// <summary>
    /// 浏览本地目录（任意盘符路径；无固定白名单）
    /// </summary>
    /// <param name="dto">请求</param>
    /// <returns>浏览结果</returns>
    Task<Takt.Shared.Models.Code.TaktDatabaseBackupBrowseResult> BrowseLocalAsync(TaktDatabaseBackupBrowseLocalDto dto);

    /// <summary>
    /// 浏览网络 UNC 目录
    /// </summary>
    /// <param name="dto">请求</param>
    /// <returns>浏览结果</returns>
    Task<Takt.Shared.Models.Code.TaktDatabaseBackupBrowseResult> BrowseNetworkAsync(TaktDatabaseBackupBrowseNetworkDto dto);

    /// <summary>
    /// 浏览 FTP 目录
    /// </summary>
    /// <param name="dto">请求</param>
    /// <returns>浏览结果</returns>
    Task<Takt.Shared.Models.Code.TaktDatabaseBackupBrowseResult> BrowseFtpAsync(TaktDatabaseBackupBrowseFtpDto dto);

    /// <summary>
    /// 在 API 宿主上创建本地目录（任意路径，无固定根）
    /// </summary>
    /// <param name="dto">目录路径</param>
    /// <returns>创建后的完整路径</returns>
    Task<string> CreateLocalDirectoryAsync(TaktDatabaseBackupMkdirLocalDto dto);

    /// <summary>
    /// 创建网络 UNC 目录
    /// </summary>
    /// <param name="dto">UNC 与凭据</param>
    /// <returns>创建后的路径</returns>
    Task<string> CreateNetworkDirectoryAsync(TaktDatabaseBackupBrowseNetworkDto dto);

    /// <summary>
    /// 创建 FTP 远程目录
    /// </summary>
    /// <param name="dto">FTP 连接与路径</param>
    /// <returns>创建后的远程路径</returns>
    Task<string> CreateFtpDirectoryAsync(TaktDatabaseBackupBrowseFtpDto dto);

    /// <summary>
    /// 创建备份配置（草稿，状态待执行）
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>备份记录</returns>
    Task<TaktDatabaseBackupDto> CreateDatabaseBackupAsync(TaktDatabaseBackupCreateDto dto);

    /// <summary>
    /// 更新备份配置（仅待执行/失败可改）
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>备份记录</returns>
    Task<TaktDatabaseBackupDto> UpdateDatabaseBackupAsync(long id, TaktDatabaseBackupUpdateDto dto);

    /// <summary>
    /// 导出数据库备份
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    Task<(string fileName, byte[] fileContent)> ExportDatabaseBackupAsync(
        TaktDatabaseBackupQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 按记录立即执行（创建一次性 Quartz 任务）
    /// </summary>
    /// <param name="id">备份记录主键</param>
    /// <returns>备份记录</returns>
    Task<TaktDatabaseBackupDto> RunDatabaseBackupByIdAsync(long id);

    /// <summary>
    /// 按记录后台调度（创建一次性 Quartz 任务）
    /// </summary>
    /// <param name="id">备份记录主键</param>
    /// <param name="dto">调度参数</param>
    /// <returns>备份记录</returns>
    Task<TaktDatabaseBackupDto> ScheduleDatabaseBackupByIdAsync(long id, TaktDatabaseBackupScheduleByIdDto dto);

    /// <summary>
    /// 立即执行数据库备份（创建记录 + 一次性 Quartz；兼容旧 API）
    /// </summary>
    /// <param name="dto">备份请求</param>
    /// <returns>备份记录（状态为已调度，关联 QuartzTaskId）</returns>
    Task<TaktDatabaseBackupDto> RunDatabaseBackupNowAsync(TaktDatabaseBackupRunDto dto);

    /// <summary>
    /// 调度数据库备份（创建 Quartz 一次性任务）
    /// </summary>
    /// <param name="dto">备份请求（含 ScheduledAt）</param>
    /// <returns>备份记录</returns>
    Task<TaktDatabaseBackupDto> ScheduleDatabaseBackupAsync(TaktDatabaseBackupRunDto dto);

    /// <summary>
    /// 执行已调度的数据库备份（Quartz Job 回调）
    /// </summary>
    /// <param name="backupId">备份记录主键</param>
    /// <returns>执行摘要（含落盘路径；跳过时说明原因）</returns>
    Task<string> ExecuteScheduledDatabaseBackupAsync(long backupId);

    /// <summary>
    /// 删除数据库备份
    /// </summary>
    /// <param name="id">数据库备份ID</param>
    /// <returns>任务</returns>
    Task DeleteDatabaseBackupByIdAsync(long id);

    /// <summary>
    /// 批量删除数据库备份
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteDatabaseBackupBatchAsync(IEnumerable<long> ids);
}
