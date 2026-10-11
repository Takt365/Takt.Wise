// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Schema
// 文件名称：TaktDatabaseBackupProvider.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：SQL Server BACKUP；FTP/客户端暂存默认 wwwroot/Backup；远程 SQL 须 UNC
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SqlSugar;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Schema;

/// <summary>
/// SQL Server 数据库备份 Provider
/// </summary>
public sealed class TaktDatabaseBackupProvider : ITaktDatabaseBackupProvider
{
    private const int BackupTypeFull = 1;
    private const int BackupTypeDelta = 2;
    private const int PathTypeLocal = 1;
    private const int PathTypeNetwork = 2;
    private const int PathTypeFtp = 3;
    private const int PathTypeClient = 4;

    private readonly IConfiguration _configuration;
    private readonly TaktDatabaseBackupOptions _backupOptions;
    private readonly DbType _sugarDbType;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="backupOptions">备份选项</param>
    public TaktDatabaseBackupProvider(
        IConfiguration configuration,
        IOptions<TaktDatabaseBackupOptions> backupOptions)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(backupOptions);
        _configuration = configuration;
        _backupOptions = backupOptions.Value;
        _sugarDbType = configuration.GetSugarDbType();
    }

    /// <summary>
    /// 执行 BACKUP DATABASE（Full 或 Differential）
    /// </summary>
    /// <param name="options">备份选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>备份结果</returns>
    public async Task<TaktDatabaseBackupResult> BackupAsync(
        TaktDatabaseBackupOptionsModel options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();
        if (options.BackupType is not (BackupTypeFull or BackupTypeDelta))
        {
            throw new ArgumentOutOfRangeException(nameof(options.BackupType), "备份类型须为 1(Full) 或 2(Delta)");
        }
        if (options.BackupPathType is not (PathTypeLocal or PathTypeNetwork or PathTypeFtp or PathTypeClient))
        {
            throw new ArgumentOutOfRangeException(nameof(options.BackupPathType), "路径类型须为 1(本地服务器) 2(文件服务器) 3(FTP) 4(客户端)");
        }
        var resolved = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            options.TargetTenantCode,
            options.TargetDatabaseName);
        var fileName = TaktDatabaseBackupPathHelper.ResolveBackupFileNameForRun(
            options.BackupFileName,
            resolved.DatabaseName,
            options.BackupType);
        if (options.BackupPathType == PathTypeFtp)
        {
            return await BackupViaFtpAsync(options, resolved.DatabaseName, resolved.ConnectionString, resolved.TenantCode, fileName, cancellationToken)
                .ConfigureAwait(false);
        }
        if (options.BackupPathType == PathTypeClient)
        {
            return await BackupViaClientStagingAsync(options, resolved.DatabaseName, resolved.ConnectionString, resolved.TenantCode, fileName, cancellationToken)
                .ConfigureAwait(false);
        }
        var directory = options.BackupPathType == PathTypeLocal
            ? NormalizeAndValidateLocalDirectory(options.BackupDirectory)
            : NormalizeNetworkDirectory(options.BackupDirectory);
        var filePath = Path.Combine(directory, fileName);
        return await ExecuteSqlBackupAsync(
                resolved.DatabaseName,
                resolved.ConnectionString,
                resolved.TenantCode,
                options.BackupType,
                directory,
                filePath)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// FTP：先备份到临时本地，再上传并清理
    /// </summary>
    private async Task<TaktDatabaseBackupResult> BackupViaFtpAsync(
        TaktDatabaseBackupOptionsModel options,
        string databaseName,
        string connectionString,
        string tenantCode,
        string fileName,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(options.BackupHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.BackupUserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.BackupPassword);
        var tempRoot = ResolveSqlStagingRoot(options, connectionString);
        var tempDir = Path.Combine(tempRoot, "ftp", Guid.NewGuid().ToString("N"));
        var tempFile = Path.Combine(tempDir, fileName);
        try
        {
            EnsureSqlStagingDirectory(tempDir);
            var localResult = await ExecuteSqlBackupAsync(
                    databaseName,
                    connectionString,
                    tenantCode,
                    options.BackupType,
                    tempDir,
                    tempFile)
                .ConfigureAwait(false);
            if (!localResult.Success)
            {
                return localResult;
            }
            cancellationToken.ThrowIfCancellationRequested();
            var remoteDir = (options.BackupDirectory ?? "/").Trim().Replace('\\', '/');
            if (string.IsNullOrWhiteSpace(remoteDir))
            {
                remoteDir = "/";
            }
            if (!remoteDir.StartsWith('/'))
            {
                remoteDir = "/" + remoteDir;
            }
            remoteDir = remoteDir.TrimEnd('/');
            var remoteFile = remoteDir == "/" ? $"/{fileName}" : $"{remoteDir}/{fileName}";
            var ftp = new TaktFtpOptions
            {
                Host = options.BackupHost.Trim(),
                Port = options.BackupPort is > 0 ? options.BackupPort.Value : 21,
                UserName = options.BackupUserName.Trim(),
                Password = options.BackupPassword,
                Timeout = 120,
            };
            if (remoteDir != "/")
            {
                await TaktFtpHelper.CreateDirectoryAsync(ftp, remoteDir).ConfigureAwait(false);
            }
            await TaktFtpHelper.UploadLocalFileViaFluentFtpAsync(ftp, tempFile, remoteFile, overwrite: true)
                .ConfigureAwait(false);
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = remoteFile,
                BackupType = options.BackupType,
                FileSizeBytes = localResult.FileSizeBytes,
                Success = true,
            };
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[DatabaseBackup] FTP backup failed Db={Database}", databaseName);
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = string.Empty,
                BackupType = options.BackupType,
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
        finally
        {
            try
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, recursive: true);
                }
            }
            catch (Exception cleanupEx)
            {
                TaktLogger.Warning(cleanupEx, "[DatabaseBackup] cleanup temp dir failed: {Dir}", tempDir);
            }
        }
    }

    /// <summary>
    /// 客户端：SQL 无法直写浏览器本机，先落到 SQL Server 可访问的暂存目录（配置路径仅作标识）
    /// </summary>
    private async Task<TaktDatabaseBackupResult> BackupViaClientStagingAsync(
        TaktDatabaseBackupOptionsModel options,
        string databaseName,
        string connectionString,
        string tenantCode,
        string fileName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        string stagingDir = string.Empty;
        try
        {
            var tempRoot = ResolveSqlStagingRoot(options, connectionString);
            stagingDir = Path.Combine(tempRoot, "client", Guid.NewGuid().ToString("N"));
            var stagingFile = Path.Combine(stagingDir, fileName);
            EnsureSqlStagingDirectory(stagingDir);
            var localResult = await ExecuteSqlBackupAsync(
                    databaseName,
                    connectionString,
                    tenantCode,
                    options.BackupType,
                    stagingDir,
                    stagingFile)
                .ConfigureAwait(false);
            if (!localResult.Success)
            {
                return localResult;
            }
            // 保留临时文件供后续下载；BackupFilePath 为服务器实际落盘路径
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = stagingFile,
                BackupType = options.BackupType,
                FileSizeBytes = localResult.FileSizeBytes,
                Success = true,
            };
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[DatabaseBackup] client staging backup failed Db={Database}", databaseName);
            try
            {
                if (!string.IsNullOrEmpty(stagingDir) && Directory.Exists(stagingDir))
                {
                    Directory.Delete(stagingDir, recursive: true);
                }
            }
            catch (Exception cleanupEx)
            {
                TaktLogger.Warning(cleanupEx, "[DatabaseBackup] cleanup client staging failed: {Dir}", stagingDir);
            }
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = string.Empty,
                BackupType = options.BackupType,
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }

    /// <summary>
    /// 解析 SQL BACKUP 暂存根目录（默认 WebApi/wwwroot/Backup；远程 SQL 须 UNC）
    /// </summary>
    /// <param name="options">备份选项</param>
    /// <param name="connectionString">目标库连接串</param>
    /// <returns>绝对路径根</returns>
    private string ResolveSqlStagingRoot(TaktDatabaseBackupOptionsModel options, string connectionString)
    {
        var configured = !string.IsNullOrWhiteSpace(options.FtpTempRoot)
            ? options.FtpTempRoot.Trim()
            : (_backupOptions.FtpTempRoot ?? string.Empty).Trim();
        var root = string.IsNullOrWhiteSpace(configured)
            ? Path.Combine(TaktFileHelper.GetWwwRootPath(), "Backup")
            : ResolveConfiguredStagingPath(configured);
        if (IsLikelyLocalSqlServer(connectionString) || root.StartsWith(@"\\", StringComparison.Ordinal))
        {
            return root;
        }
        // 远程 SQL 无法写本机盘符：提升为 \\本机\盘符$\... 管理共享（须 SQL 服务账号可写）
        var unc = TryPromoteLocalPathToAdminShareUnc(root);
        if (string.IsNullOrEmpty(unc))
        {
            throw new InvalidOperationException(
                $"远程 SQL Server 暂存目录须为 UNC，当前为「{root}」。请配置 DatabaseBackup:FtpTempRoot=\\\\<SqlHost>\\<Share>，"
                + "或将 wwwroot/Backup 做成共享后填 UNC。");
        }
        Directory.CreateDirectory(root);
        TaktLogger.Information(
            "[DatabaseBackup] 远程 SQL：本机暂存提升为管理共享 Local={Local} Unc={Unc}",
            root,
            unc);
        return unc;
    }

    /// <summary>
    /// 本机绝对路径 → \\Machine\X$\relative（供远程 SQL BACKUP 经管理共享写入）
    /// </summary>
    /// <param name="localAbsolutePath">本机绝对路径</param>
    /// <returns>UNC 或 null</returns>
    private static string? TryPromoteLocalPathToAdminShareUnc(string localAbsolutePath)
    {
        if (string.IsNullOrWhiteSpace(localAbsolutePath)
            || localAbsolutePath.StartsWith(@"\\", StringComparison.Ordinal))
        {
            return null;
        }
        var full = Path.GetFullPath(localAbsolutePath);
        var root = Path.GetPathRoot(full);
        if (string.IsNullOrEmpty(root) || root.Length < 2 || root[1] != ':')
        {
            return null;
        }
        var driveLetter = char.ToUpperInvariant(root[0]);
        var relative = full.Length > root.Length
            ? full[root.Length..].TrimStart('\\', '/').Replace('/', '\\')
            : string.Empty;
        var machine = Environment.MachineName;
        return string.IsNullOrEmpty(relative)
            ? $@"\\{machine}\{driveLetter}$"
            : $@"\\{machine}\{driveLetter}$\{relative}";
    }

    /// <summary>
    /// 将 FtpTempRoot 配置解析为绝对路径（支持 UNC、绝对路径、相对 ContentRoot 如 wwwroot/Backup）
    /// </summary>
    /// <param name="configured">配置值</param>
    /// <returns>绝对路径</returns>
    private static string ResolveConfiguredStagingPath(string configured)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configured);
        if (configured.StartsWith(@"\\", StringComparison.Ordinal))
        {
            return configured.TrimEnd('\\', '/');
        }
        var normalized = configured.Replace('/', Path.DirectorySeparatorChar).Trim();
        if (Path.IsPathRooted(normalized))
        {
            return Path.GetFullPath(normalized);
        }
        var trimmed = normalized.TrimStart(Path.DirectorySeparatorChar);
        if (trimmed.Equals("wwwroot", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("wwwroot" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
        {
            var wwwroot = TaktFileHelper.GetWwwRootPath();
            var relative = trimmed.Equals("wwwroot", StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : trimmed["wwwroot".Length..].TrimStart(Path.DirectorySeparatorChar);
            return string.IsNullOrEmpty(relative)
                ? Path.GetFullPath(wwwroot)
                : Path.GetFullPath(Path.Combine(wwwroot, relative));
        }
        var contentRoot = Directory.GetParent(TaktFileHelper.GetWwwRootPath())?.FullName
            ?? AppContext.BaseDirectory;
        return Path.GetFullPath(Path.Combine(contentRoot, trimmed));
    }

    /// <summary>
    /// 连接串是否指向本机 SQL（含 LocalDB / . / localhost）
    /// </summary>
    private static bool IsLikelyLocalSqlServer(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return true;
        }
        var server = ExtractSqlServerDataSource(connectionString);
        if (string.IsNullOrWhiteSpace(server))
        {
            return true;
        }
        var s = server.Trim().TrimStart('[').TrimEnd(']');
        if (s.Equals(".", StringComparison.OrdinalIgnoreCase)
            || s.Equals("(local)", StringComparison.OrdinalIgnoreCase)
            || s.Equals("localhost", StringComparison.OrdinalIgnoreCase)
            || s.StartsWith(".\\", StringComparison.Ordinal)
            || s.StartsWith("(localdb)", StringComparison.OrdinalIgnoreCase)
            || s.StartsWith("localdb\\", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        var host = s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0]
            .Split('\\', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0];
        return host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
            || host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase)
            || host.Equals("::1", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 从连接串提取 Server / Data Source
    /// </summary>
    private static string ExtractSqlServerDataSource(string connectionString)
    {
        foreach (var part in connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var idx = part.IndexOf('=');
            if (idx <= 0)
            {
                continue;
            }
            var key = part[..idx].Trim();
            var value = part[(idx + 1)..].Trim();
            if (key.Equals("Server", StringComparison.OrdinalIgnoreCase)
                || key.Equals("Data Source", StringComparison.OrdinalIgnoreCase)
                || key.Equals("Addr", StringComparison.OrdinalIgnoreCase)
                || key.Equals("Address", StringComparison.OrdinalIgnoreCase)
                || key.Equals("Network Address", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// 创建暂存目录并尽量赋予本机 Users 修改权（便于 SQL Server 服务写入 .bak）
    /// </summary>
    /// <param name="directory">目录绝对路径</param>
    private static void EnsureSqlStagingDirectory(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        Directory.CreateDirectory(directory);
        if (!OperatingSystem.IsWindows() || directory.StartsWith(@"\\", StringComparison.Ordinal))
        {
            // UNC 权限由共享 ACL 决定，跳过本机 icacls
            return;
        }
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "icacls",
                Arguments = $"\"{directory}\" /grant *S-1-5-32-545:(OI)(CI)M /T /C /Q",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            using var proc = System.Diagnostics.Process.Start(psi);
            proc?.WaitForExit(8000);
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(ex, "[DatabaseBackup] grant Users ACL on staging failed: {Dir}", directory);
        }
    }

    /// <summary>
    /// 执行 BACKUP DATABASE 到指定本地/UNC 文件
    /// </summary>
    private async Task<TaktDatabaseBackupResult> ExecuteSqlBackupAsync(
        string databaseName,
        string connectionString,
        string tenantCode,
        int backupType,
        string directory,
        string filePath)
    {
        try
        {
            EnsureSqlStagingDirectory(directory);
        }
        catch (Exception ex)
        {
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = filePath,
                BackupType = backupType,
                Success = false,
                ErrorMessage = $"无法创建备份目录: {ex.Message}",
            };
        }
        if (!Directory.Exists(directory))
        {
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = filePath,
                BackupType = backupType,
                Success = false,
                ErrorMessage = $"备份目录不存在或 SQL Server 不可见: {directory}。请配置 DatabaseBackup:FtpTempRoot（默认 wwwroot/Backup；远程 SQL 须 UNC）。",
            };
        }
        using var db = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, connectionString, tenantCode);
        var quotedDb = QuoteIdentifier(databaseName);
        var escapedPath = filePath.Replace("'", "''", StringComparison.Ordinal);
        var withClause = backupType == BackupTypeDelta
            ? "DIFFERENTIAL, FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10"
            : "FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, COMPRESSION, STATS = 10";
        var sql = $"BACKUP DATABASE {quotedDb} TO DISK = N'{escapedPath}' WITH {withClause}";
        try
        {
            await db.Ado.ExecuteCommandAsync(sql).ConfigureAwait(false);
            long size = 0;
            if (File.Exists(filePath))
            {
                size = new FileInfo(filePath).Length;
            }
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = filePath,
                BackupType = backupType,
                FileSizeBytes = size,
                Success = true,
            };
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[DatabaseBackup] BACKUP failed Db={Database} Path={Path}", databaseName, filePath);
            var hint = string.Empty;
            if (ex.Message.Contains("操作系统错误 3", StringComparison.Ordinal)
                || ex.Message.Contains("error 3", StringComparison.OrdinalIgnoreCase)
                || ex.Message.Contains("找不到指定的路径", StringComparison.Ordinal))
            {
                hint = " 提示：BACKUP 路径须在 SQL Server 宿主上存在且服务账号可写；默认 wwwroot/Backup（本机 SQL）；远程 SQL 请设 DatabaseBackup:FtpTempRoot 为 UNC。";
            }
            return new TaktDatabaseBackupResult
            {
                DatabaseName = databaseName,
                BackupFilePath = filePath,
                BackupType = backupType,
                Success = false,
                ErrorMessage = ex.Message + hint,
            };
        }
    }

    /// <summary>
    /// 规范化本地备份目录（由用户选择；执行前确保目录可写）
    /// </summary>
    private static string NormalizeAndValidateLocalDirectory(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        var full = Path.GetFullPath(directory.Trim())
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (!Directory.Exists(full))
        {
            Directory.CreateDirectory(full);
        }
        return full;
    }

    /// <summary>
    /// 规范化网络 UNC 目录（执行前确保目录存在）
    /// </summary>
    private static string NormalizeNetworkDirectory(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        var trimmed = directory.Trim();
        if (!trimmed.StartsWith(@"\\", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("网络备份路径须为 UNC（以 \\\\ 开头）");
        }
        var full = trimmed.TrimEnd('\\', '/');
        if (!Directory.Exists(full))
        {
            Directory.CreateDirectory(full);
        }
        return full;
    }

    /// <summary>
    /// 安全引用 SQL 标识符
    /// </summary>
    private static string QuoteIdentifier(string name)
    {
        return "[" + name.Replace("]", "]]", StringComparison.Ordinal) + "]";
    }
}
