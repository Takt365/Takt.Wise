// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：TaktDatabaseBackupService.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份应用服务（立即/调度执行，Quartz 一次性任务）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Options;
using SqlSugar;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Code.Database;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 数据库备份应用服务
/// </summary>
public class TaktDatabaseBackupService : TaktServiceBase, ITaktDatabaseBackupService
{
    /// <summary>
    /// Quartz Handler 类名（与 TaktDatabaseBackupJobHandler.HandlerKey 一致）
    /// </summary>
    public const string QuartzHandlerClassName = "TaktDatabaseBackupJobHandler";

    private const int BackupStatusPending = 0;
    private const int BackupStatusRunning = 1;
    private const int BackupStatusSuccess = 2;
    private const int BackupStatusFailed = 3;
    private const int BackupStatusScheduled = 4;
    private const int BackupTypeFull = 1;
    private const int BackupTypeDelta = 2;
    private const int ExecuteModeImmediate = 1;
    private const int ExecuteModeBackground = 2;
    private const int PathTypeLocal = 1;
    private const int PathTypeNetwork = 2;
    private const int PathTypeFtp = 3;
    private const int PathTypeClient = 4;
    private const int QuartzTaskStatusPaused = 1;
    private const string QuartzAssemblyName = "Takt.Infrastructure";
    private const string QuartzJobGroup = "default";
    private const string QuartzTaskTypeAssembly = "assembly";

    private readonly ITaktCompanyRepository<TaktDatabaseBackup> _databaseBackupRepository;
    private readonly ITaktCompanyRepository<TaktBackupLog> _backupLogRepository;
    private readonly ITaktDatabaseBackupProvider _backupProvider;
    private readonly ITaktQuartzTaskService _quartzTaskService;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly TaktDatabaseBackupOptions _backupOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="databaseBackupRepository">数据库备份仓储</param>
    /// <param name="backupLogRepository">备份执行日志仓储</param>
    /// <param name="backupProvider">SQL Server 备份 Provider</param>
    /// <param name="quartzTaskService">Quartz 任务服务</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="backupOptions">备份配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDatabaseBackupService(
        ITaktCompanyRepository<TaktDatabaseBackup> databaseBackupRepository,
        ITaktCompanyRepository<TaktBackupLog> backupLogRepository,
        ITaktDatabaseBackupProvider backupProvider,
        ITaktQuartzTaskService quartzTaskService,
        ITaktUniqueValidator uniqueValidator,
        IOptions<TaktDatabaseBackupOptions> backupOptions,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _databaseBackupRepository = databaseBackupRepository;
        _backupLogRepository = backupLogRepository;
        _backupProvider = backupProvider;
        _quartzTaskService = quartzTaskService;
        _uniqueValidator = uniqueValidator;
        _backupOptions = backupOptions.Value;
    }

    /// <summary>
    /// 获取数据库备份列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDatabaseBackupDto>> GetDatabaseBackupListAsync(TaktDatabaseBackupQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _databaseBackupRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDatabaseBackupDto>.Create(
            data.Select(MapToDto).ToList(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取数据库备份
    /// </summary>
    /// <param name="id">数据库备份ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDatabaseBackupDto?> GetDatabaseBackupByIdAsync(long id)
    {
        var entity = await _databaseBackupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return MapToDto(entity);
    }

    /// <summary>
    /// 获取备份路径选项（默认根目录与白名单）
    /// </summary>
    /// <returns>路径选项</returns>
    public TaktDatabaseBackupPathOptionsDto GetDatabaseBackupPathOptions()
    {
        // 不再下发固定根；保留字段仅为兼容旧前端
        return new TaktDatabaseBackupPathOptionsDto
        {
            DefaultRoot = string.Empty,
            AllowedRoots = new List<string>(),
        };
    }

    /// <summary>
    /// 浏览本地目录（任意盘符路径；无固定白名单）
    /// </summary>
    /// <param name="dto">请求</param>
    /// <returns>浏览结果</returns>
    public Task<TaktDatabaseBackupBrowseResult> BrowseLocalAsync(TaktDatabaseBackupBrowseLocalDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        string? current = null;
        var requested = (dto.CurrentPath ?? string.Empty).Trim();
        // 前端「此电脑」哨兵 / 空路径 → 盘符列表（勿对哨兵做 Path.GetFullPath）
        if (!string.IsNullOrWhiteSpace(requested)
            && !string.Equals(requested, "__takt_local_root__", StringComparison.Ordinal))
        {
            try
            {
                current = Path.GetFullPath(requested);
            }
            catch (Exception)
            {
                current = null;
            }
        }
        if (current == null)
        {
            return Task.FromResult(new TaktDatabaseBackupBrowseResult
            {
                CurrentPath = string.Empty,
                ParentPath = null,
                Items = ListLocalDriveRoots(),
            });
        }
        if (!Directory.Exists(current))
        {
            throw new TaktBusinessException($"目录不存在或不可访问: {current}");
        }
        var root = Path.GetPathRoot(current);
        var currentNorm = current.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var rootNorm = root?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string? parent;
        if (!string.IsNullOrEmpty(rootNorm)
            && currentNorm.Equals(rootNorm, StringComparison.OrdinalIgnoreCase))
        {
            // 盘符根：上级约定为空串，前端回到驱动器列表
            parent = string.Empty;
        }
        else
        {
            parent = Directory.GetParent(current)?.FullName;
        }
        var items = Directory.GetDirectories(current)
            .OrderBy(d => d, StringComparer.OrdinalIgnoreCase)
            .Take(500)
            .Select(d =>
            {
                var full = Path.GetFullPath(d);
                DateTime? modified = null;
                try
                {
                    modified = Directory.GetLastWriteTime(full);
                }
                catch
                {
                    // 忽略个别目录取时间失败
                }
                return new TaktDatabaseBackupBrowseItem
                {
                    Name = Path.GetFileName(full.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)),
                    FullPath = full,
                    IsDirectory = true,
                    ModifiedTime = modified,
                };
            })
            .ToList();
        return Task.FromResult(new TaktDatabaseBackupBrowseResult
        {
            CurrentPath = Path.GetFullPath(current),
            ParentPath = parent,
            Items = items,
        });
    }

    /// <summary>
    /// 浏览网络 UNC 目录
    /// </summary>
    /// <param name="dto">请求</param>
    /// <returns>浏览结果</returns>
    public async Task<TaktDatabaseBackupBrowseResult> BrowseNetworkAsync(TaktDatabaseBackupBrowseNetworkDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var path = (dto.Path ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(path) || !path.StartsWith(@"\\", StringComparison.Ordinal))
        {
            throw new TaktBusinessException("网络路径须为 UNC（以 \\\\ 开头）");
        }
        path = TaktUncShareHelper.NormalizeUncPath(path);
        var password = await ResolveBrowsePasswordAsync(dto.Password, dto.DatabaseBackupId);
        var needCredential = !string.IsNullOrWhiteSpace(dto.UserName) || !string.IsNullOrWhiteSpace(password);
        if (needCredential && !OperatingSystem.IsWindows())
        {
            throw new TaktBusinessException("带凭据的 UNC 浏览仅支持 Windows 宿主");
        }
        try
        {
            using var _ = OperatingSystem.IsWindows()
                ? TaktUncShareHelper.Connect(path, dto.UserName, password)
                : null;
            if (!Directory.Exists(path))
            {
                throw new TaktBusinessException("网络目录不存在或无权访问（请核对 UNC、账号密码，以及 API 进程对共享的权限）");
            }
            var parent = Directory.GetParent(path)?.FullName;
            var items = Directory.GetDirectories(path)
                .OrderBy(d => d, StringComparer.OrdinalIgnoreCase)
                .Take(500)
                .Select(d =>
                {
                    DateTime? modified = null;
                    try
                    {
                        modified = Directory.GetLastWriteTime(d);
                    }
                    catch
                    {
                        // 忽略个别目录取时间失败
                    }
                    return new TaktDatabaseBackupBrowseItem
                    {
                        Name = Path.GetFileName(d),
                        FullPath = d,
                        IsDirectory = true,
                        ModifiedTime = modified,
                    };
                })
                .ToList();
            return new TaktDatabaseBackupBrowseResult
            {
                CurrentPath = path,
                ParentPath = parent,
                Items = items,
            };
        }
        catch (ArgumentException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
    }

    /// <summary>
    /// 浏览 FTP 目录
    /// </summary>
    /// <param name="dto">请求</param>
    /// <returns>浏览结果</returns>
    public async Task<TaktDatabaseBackupBrowseResult> BrowseFtpAsync(TaktDatabaseBackupBrowseFtpDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(dto.Host))
        {
            throw new TaktBusinessException("FTP 服务器名称不能为空");
        }
        if (string.IsNullOrWhiteSpace(dto.UserName))
        {
            throw new TaktBusinessException("FTP 用户名不能为空");
        }
        var password = await ResolveBrowsePasswordAsync(dto.Password, dto.DatabaseBackupId);
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new TaktBusinessException("FTP 密码不能为空（编辑已有配置时可留空以使用已存密码，但仍须先输入或保存过密码）");
        }
        var remotePath = string.IsNullOrWhiteSpace(dto.Path) ? "/" : dto.Path.Trim().Replace('\\', '/');
        if (!remotePath.StartsWith('/'))
        {
            remotePath = "/" + remotePath;
        }
        var ftp = new TaktFtpOptions
        {
            Host = dto.Host.Trim(),
            Port = dto.Port is > 0 ? dto.Port.Value : 21,
            UserName = dto.UserName.Trim(),
            Password = password,
            Timeout = 60,
        };
        var dirs = await TaktFtpHelper.ListDirectoriesAsync(ftp, remotePath);
        var parent = remotePath.TrimEnd('/');
        var slash = parent.LastIndexOf('/');
        string? parentPath = slash > 0 ? parent[..slash] : (slash == 0 ? "/" : null);
        if (parentPath == string.Empty)
        {
            parentPath = "/";
        }
        var items = dirs
            .OrderBy(d => d.Name, StringComparer.OrdinalIgnoreCase)
            .Take(500)
            .Select(d =>
            {
                var name = d.Name;
                var full = remotePath.TrimEnd('/') + "/" + name;
                return new TaktDatabaseBackupBrowseItem
                {
                    Name = name,
                    FullPath = full,
                    IsDirectory = true,
                    ModifiedTime = d.Modified == default ? null : d.Modified,
                };
            })
            .ToList();
        return new TaktDatabaseBackupBrowseResult
        {
            CurrentPath = remotePath,
            ParentPath = parentPath,
            Items = items,
        };
    }

    /// <summary>
    /// 在 API 宿主上创建本地目录（任意路径，无固定根）
    /// </summary>
    /// <param name="dto">目录路径</param>
    /// <returns>创建后的完整路径</returns>
    public Task<string> CreateLocalDirectoryAsync(TaktDatabaseBackupMkdirLocalDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(dto.Path))
        {
            throw new TaktBusinessException("目录路径不能为空");
        }
        string full;
        try
        {
            full = Path.GetFullPath(dto.Path.Trim());
        }
        catch (Exception ex)
        {
            throw new TaktBusinessException($"无效的目录路径: {ex.Message}");
        }
        try
        {
            Directory.CreateDirectory(full);
        }
        catch (Exception ex)
        {
            throw new TaktBusinessException($"创建目录失败: {ex.Message}");
        }
        return Task.FromResult(full);
    }

    /// <summary>
    /// 创建网络 UNC 目录
    /// </summary>
    /// <param name="dto">UNC 与凭据</param>
    /// <returns>创建后的路径</returns>
    public async Task<string> CreateNetworkDirectoryAsync(TaktDatabaseBackupBrowseNetworkDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var path = (dto.Path ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(path) || !path.StartsWith(@"\\", StringComparison.Ordinal))
        {
            throw new TaktBusinessException("网络路径须为 UNC（以 \\\\ 开头）");
        }
        path = TaktUncShareHelper.NormalizeUncPath(path);
        var password = await ResolveBrowsePasswordAsync(dto.Password, dto.DatabaseBackupId);
        var needCredential = !string.IsNullOrWhiteSpace(dto.UserName) || !string.IsNullOrWhiteSpace(password);
        if (needCredential && !OperatingSystem.IsWindows())
        {
            throw new TaktBusinessException("带凭据的 UNC 创建目录仅支持 Windows 宿主");
        }
        try
        {
            using var _ = OperatingSystem.IsWindows()
                ? TaktUncShareHelper.Connect(path, dto.UserName, password)
                : null;
            Directory.CreateDirectory(path);
            return path.TrimEnd('\\', '/');
        }
        catch (TaktBusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new TaktBusinessException($"创建网络目录失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 创建 FTP 远程目录
    /// </summary>
    /// <param name="dto">FTP 连接与路径</param>
    /// <returns>创建后的远程路径</returns>
    public async Task<string> CreateFtpDirectoryAsync(TaktDatabaseBackupBrowseFtpDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (string.IsNullOrWhiteSpace(dto.Host))
        {
            throw new TaktBusinessException("FTP 服务器名称不能为空");
        }
        if (string.IsNullOrWhiteSpace(dto.UserName))
        {
            throw new TaktBusinessException("FTP 用户名不能为空");
        }
        var password = await ResolveBrowsePasswordAsync(dto.Password, dto.DatabaseBackupId);
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new TaktBusinessException("FTP 密码不能为空");
        }
        var remotePath = string.IsNullOrWhiteSpace(dto.Path) ? "/" : dto.Path.Trim().Replace('\\', '/');
        if (!remotePath.StartsWith('/'))
        {
            remotePath = "/" + remotePath;
        }
        var ftp = new TaktFtpOptions
        {
            Host = dto.Host.Trim(),
            Port = dto.Port is > 0 ? dto.Port.Value : 21,
            UserName = dto.UserName.Trim(),
            Password = password,
            Timeout = 60,
        };
        try
        {
            await TaktFtpHelper.CreateDirectoryAsync(ftp, remotePath);
            return remotePath == "/" ? "/" : remotePath.TrimEnd('/');
        }
        catch (Exception ex)
        {
            throw new TaktBusinessException($"创建 FTP 目录失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 创建备份配置（草稿，状态待执行）
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>备份记录</returns>
    public async Task<TaktDatabaseBackupDto> CreateDatabaseBackupAsync(TaktDatabaseBackupCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        ValidateConfigFields(dto);
        if (NormalizePathType(dto.BackupPathType) == PathTypeFtp && string.IsNullOrWhiteSpace(dto.BackupPassword))
        {
            throw new TaktBusinessException("FTP 密码不能为空");
        }
        var entity = new TaktDatabaseBackup
        {
            BackupCode = string.IsNullOrWhiteSpace(dto.BackupCode)
                ? GenerateBackupCode(dto.TargetDatabaseName)
                : dto.BackupCode.Trim(),
            TargetTenantCode = dto.TargetTenantCode.Trim(),
            TargetDatabaseName = dto.TargetDatabaseName.Trim(),
            BackupType = dto.BackupType <= 0 ? BackupTypeFull : dto.BackupType,
            ExecuteMode = 0,
            BackupPathType = NormalizePathType(dto.BackupPathType),
            BackupPath = ResolveBackupPath(dto.BackupPath, dto.BackupPathType),
            BackupHost = string.IsNullOrWhiteSpace(dto.BackupHost) ? null : dto.BackupHost.Trim(),
            BackupPort = dto.BackupPathType == PathTypeFtp ? (dto.BackupPort is > 0 ? dto.BackupPort : 21) : null,
            BackupUserName = string.IsNullOrWhiteSpace(dto.BackupUserName) ? null : dto.BackupUserName.Trim(),
            BackupPassword = EncryptPasswordIfAny(dto.BackupPassword),
            BackupFileName = ResolveFileName(
                dto.BackupFileName,
                dto.TargetDatabaseName,
                dto.BackupType <= 0 ? BackupTypeFull : dto.BackupType),
            BackupStatus = BackupStatusPending,
            Remark = dto.Remark,
            ExtField = dto.ExtField,
        };
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _databaseBackupRepository,
            x => x.BackupCode == entity.BackupCode);
        if (!isUnique)
        {
            throw new TaktBusinessException("备份编码已存在");
        }
        entity = await _databaseBackupRepository.CreateAsync(entity);
        return MapToDto(entity);
    }

    /// <summary>
    /// 更新备份配置（仅待执行/失败可改）
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>备份记录</returns>
    public async Task<TaktDatabaseBackupDto> UpdateDatabaseBackupAsync(long id, TaktDatabaseBackupUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        var entity = await GetOwnedBackupEntityAsync(id);
        if (entity.BackupStatus is BackupStatusRunning or BackupStatusScheduled)
        {
            throw new TaktBusinessException("执行中或已调度的备份不可修改配置");
        }
        ValidateConfigFields(dto);
        if (NormalizePathType(dto.BackupPathType) == PathTypeFtp
            && string.IsNullOrWhiteSpace(dto.BackupPassword)
            && string.IsNullOrWhiteSpace(entity.BackupPassword))
        {
            throw new TaktBusinessException("FTP 密码不能为空");
        }
        entity.TargetTenantCode = dto.TargetTenantCode.Trim();
        entity.TargetDatabaseName = dto.TargetDatabaseName.Trim();
        entity.BackupType = dto.BackupType <= 0 ? BackupTypeFull : dto.BackupType;
        entity.BackupPathType = NormalizePathType(dto.BackupPathType);
        entity.BackupPath = ResolveBackupPath(dto.BackupPath, dto.BackupPathType);
        entity.BackupHost = string.IsNullOrWhiteSpace(dto.BackupHost) ? null : dto.BackupHost.Trim();
        entity.BackupPort = dto.BackupPathType == PathTypeFtp ? (dto.BackupPort is > 0 ? dto.BackupPort : 21) : null;
        entity.BackupUserName = string.IsNullOrWhiteSpace(dto.BackupUserName) ? null : dto.BackupUserName.Trim();
        if (!string.IsNullOrWhiteSpace(dto.BackupPassword))
        {
            entity.BackupPassword = EncryptPasswordIfAny(dto.BackupPassword);
        }
        entity.BackupFileName = ResolveFileName(dto.BackupFileName, dto.TargetDatabaseName, entity.BackupType);
        entity.Remark = dto.Remark;
        entity.ExtField = dto.ExtField;
        if (!string.IsNullOrWhiteSpace(dto.BackupCode) && !string.Equals(dto.BackupCode.Trim(), entity.BackupCode, StringComparison.Ordinal))
        {
            var code = dto.BackupCode.Trim();
            var isUnique = await _uniqueValidator.IsUniqueAsync(
                _databaseBackupRepository,
                x => x.BackupCode == code,
                id);
            if (!isUnique)
            {
                throw new TaktBusinessException("备份编码已存在");
            }
            entity.BackupCode = code;
        }
        await _databaseBackupRepository.UpdateAsync(entity);
        return MapToDto(entity);
    }

    /// <summary>
    /// 导出数据库备份
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件名与内容</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDatabaseBackupAsync(
        TaktDatabaseBackupQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(query ?? new TaktDatabaseBackupQueryDto());
        var list = await _databaseBackupRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDatabaseBackupExportDto>(),
                sheetName ?? "数据库备份数据",
                fileName ?? "数据库备份导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDatabaseBackupExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "数据库备份数据",
            fileName ?? "数据库备份导出.xlsx");
    }

    /// <summary>
    /// 按记录立即执行（创建一次性 Quartz 任务）
    /// </summary>
    /// <param name="id">备份记录主键</param>
    /// <returns>备份记录</returns>
    public async Task<TaktDatabaseBackupDto> RunDatabaseBackupByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        var entity = await GetOwnedBackupEntityAsync(id);
        EnsureCanSchedule(entity);
        return await AttachQuartzAndPersistAsync(entity, DateTime.Now, ExecuteModeImmediate);
    }

    /// <summary>
    /// 按记录后台调度（创建一次性 Quartz 任务）
    /// </summary>
    /// <param name="id">备份记录主键</param>
    /// <param name="dto">调度参数</param>
    /// <returns>备份记录</returns>
    public async Task<TaktDatabaseBackupDto> ScheduleDatabaseBackupByIdAsync(long id, TaktDatabaseBackupScheduleByIdDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (dto.ScheduledAt <= DateTime.Now)
        {
            throw new TaktBusinessException("计划执行时间须晚于当前时间");
        }
        var entity = await GetOwnedBackupEntityAsync(id);
        EnsureCanSchedule(entity);
        return await AttachQuartzAndPersistAsync(entity, dto.ScheduledAt, ExecuteModeBackground);
    }

    /// <summary>
    /// 立即执行数据库备份（创建记录 + 一次性 Quartz；兼容旧 API）
    /// </summary>
    /// <param name="dto">备份请求</param>
    /// <returns>备份记录（状态为已调度，关联 QuartzTaskId）</returns>
    public async Task<TaktDatabaseBackupDto> RunDatabaseBackupNowAsync(TaktDatabaseBackupRunDto dto)
    {
        EnsureThreeLayerContext();
        ValidateRunDto(dto, requireScheduledAt: false);
        // 立即执行也写入一次性 Quartz 任务，由调度器触发（FirstRunAt<=Now → StartNow）
        dto.ScheduledAt = DateTime.Now;
        return await CreateScheduledBackupAsync(dto, ExecuteModeImmediate);
    }

    /// <summary>
    /// 调度数据库备份（创建 Quartz 一次性任务）
    /// </summary>
    /// <param name="dto">备份请求（含 ScheduledAt）</param>
    /// <returns>备份记录</returns>
    public async Task<TaktDatabaseBackupDto> ScheduleDatabaseBackupAsync(TaktDatabaseBackupRunDto dto)
    {
        EnsureThreeLayerContext();
        ValidateRunDto(dto, requireScheduledAt: true);
        return await CreateScheduledBackupAsync(dto, ExecuteModeBackground);
    }

    /// <summary>
    /// 创建备份记录并挂接一次性 Quartz 任务
    /// </summary>
    /// <param name="dto">备份请求（须含 ScheduledAt）</param>
    /// <param name="executeMode">1=立即 2=后台</param>
    /// <returns>备份记录</returns>
    private async Task<TaktDatabaseBackupDto> CreateScheduledBackupAsync(TaktDatabaseBackupRunDto dto, int executeMode)
    {
        var entity = await CreateBackupRecordAsync(dto, executeMode, BackupStatusScheduled);
        return await AttachQuartzAndPersistAsync(entity, dto.ScheduledAt!.Value, executeMode);
    }

    /// <summary>
    /// 已有记录挂接 Quartz 并更新状态
    /// </summary>
    /// <param name="entity">备份实体</param>
    /// <param name="scheduledAt">计划时间</param>
    /// <param name="executeMode">执行方式</param>
    /// <returns>DTO</returns>
    private async Task<TaktDatabaseBackupDto> AttachQuartzAndPersistAsync(
        TaktDatabaseBackup entity,
        DateTime scheduledAt,
        int executeMode)
    {
        entity.ExecuteMode = executeMode;
        entity.ScheduledAt = scheduledAt;
        entity.BackupStatus = BackupStatusScheduled;
        // 须先落库为「已调度」，再创建/触发 Quartz
        await _databaseBackupRepository.UpdateAsync(entity);
        var isImmediate = executeMode == ExecuteModeImmediate
            || scheduledAt <= DateTime.Now.AddSeconds(1);
        // 已有关联任务时：立即执行直接 Trigger，避免 TaskCode 冲突重复建任务
        if (isImmediate && entity.QuartzTaskId is > 0)
        {
            var existing = await _quartzTaskService.GetQuartzTaskByIdAsync(entity.QuartzTaskId.Value);
            if (existing != null)
            {
                await _quartzTaskService.ExecuteQuartzTaskNowAsync(entity.QuartzTaskId.Value);
                return MapToDto(entity);
            }
        }
        var quartzDto = BuildQuartzTaskCreateDto(entity, scheduledAt);
        var quartzTask = await _quartzTaskService.CreateQuartzTaskAsync(quartzDto);
        entity.QuartzTaskId = quartzTask.QuartzTaskId;
        await _databaseBackupRepository.UpdateAsync(entity);
        return MapToDto(entity);
    }

    /// <summary>
    /// 获取当前租户公司下的备份实体
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>实体</returns>
    private async Task<TaktDatabaseBackup> GetOwnedBackupEntityAsync(long id)
    {
        var entity = await _databaseBackupRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("数据库备份不存在或已删除");
        }
        return entity;
    }

    /// <summary>
    /// 校验记录是否允许再次调度
    /// </summary>
    /// <param name="entity">备份实体</param>
    private static void EnsureCanSchedule(TaktDatabaseBackup entity)
    {
        if (entity.BackupStatus is BackupStatusRunning or BackupStatusScheduled)
        {
            throw new TaktBusinessException("执行中或已调度的备份不可重复触发");
        }
    }

    /// <summary>
    /// 目标租户强制为当前上下文租户（不可跨租户备份）
    /// </summary>
    /// <param name="dto">创建/更新 DTO</param>
    private void EnforceCurrentTargetTenant(TaktDatabaseBackupCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        dto.TargetTenantCode = CurrentTenantCode;
    }

    /// <summary>
    /// 目标租户强制为当前上下文租户（立即/调度请求）
    /// </summary>
    /// <param name="dto">运行请求</param>
    private void EnforceCurrentTargetTenant(TaktDatabaseBackupRunDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        dto.TargetTenantCode = CurrentTenantCode;
    }

    /// <summary>
    /// 校验备份配置字段
    /// </summary>
    /// <param name="dto">创建/更新 DTO</param>
    private void ValidateConfigFields(TaktDatabaseBackupCreateDto dto)
    {
        EnforceCurrentTargetTenant(dto);
        if (string.IsNullOrWhiteSpace(dto.TargetTenantCode) || dto.TargetTenantCode.Trim().Length != 3)
        {
            throw new TaktBusinessException("目标租户须为 3 位");
        }
        if (string.IsNullOrWhiteSpace(dto.TargetDatabaseName))
        {
            throw new TaktBusinessException("目标数据库名不能为空");
        }
        if (dto.BackupType is not (BackupTypeFull or BackupTypeDelta) && dto.BackupType != 0)
        {
            throw new TaktBusinessException("备份类型须为 1(Full Sync) 或 2(Delta Sync)");
        }
        var pathType = NormalizePathType(dto.BackupPathType);
        if (string.IsNullOrWhiteSpace(ResolveBackupPath(dto.BackupPath, pathType)))
        {
            throw new TaktBusinessException("备份目录不能为空");
        }
        if (pathType == PathTypeNetwork)
        {
            var path = dto.BackupPath.Trim();
            if (!path.StartsWith(@"\\", StringComparison.Ordinal))
            {
                throw new TaktBusinessException("网络备份路径须为 UNC（以 \\\\ 开头）");
            }
        }
        if (pathType == PathTypeFtp)
        {
            if (string.IsNullOrWhiteSpace(dto.BackupHost))
            {
                throw new TaktBusinessException("FTP 服务器名称不能为空");
            }
            if (string.IsNullOrWhiteSpace(dto.BackupUserName))
            {
                throw new TaktBusinessException("FTP 用户名不能为空");
            }
        }
        if (pathType is PathTypeNetwork or PathTypeFtp
            && string.IsNullOrWhiteSpace(dto.BackupPassword)
            && string.IsNullOrWhiteSpace(dto.BackupUserName) == false)
        {
            // 创建时密码可空（编辑留空不改）；网络可无密码匿名共享
        }
    }

    /// <summary>
    /// 执行已调度的数据库备份（Quartz Job 回调）
    /// </summary>
    /// <param name="backupId">备份记录主键</param>
    /// <returns>执行摘要（含落盘路径；跳过时说明原因）</returns>
    public async Task<string> ExecuteScheduledDatabaseBackupAsync(long backupId)
    {
        var entity = await _databaseBackupRepository.GetByIdAsync(backupId);
        if (entity == null)
        {
            throw new TaktBusinessException("数据库备份记录不存在");
        }
        if (entity.BackupStatus == BackupStatusRunning)
        {
            throw new TaktBusinessException("备份正在执行中，请勿重复触发");
        }
        // Quartz 调度或任务中心「立即执行」均可触发：不要求状态仍为「已调度」
        // （失败/成功后重跑旧 Job 时状态多为 2/3，此前误跳过）
        string resultPath;
        try
        {
            resultPath = await ExecuteBackupCoreAsync(entity);
        }
        finally
        {
            if (entity.QuartzTaskId.HasValue && entity.QuartzTaskId.Value > 0)
            {
                await _quartzTaskService.UpdateQuartzTaskStatusAsync(new TaktQuartzTaskStatusDto
                {
                    QuartzTaskId = entity.QuartzTaskId.Value,
                    TaskStatus = QuartzTaskStatusPaused,
                });
            }
        }
        return $"数据库备份完成 backupId={backupId}，文件={resultPath}";
    }

    /// <summary>
    /// 删除数据库备份
    /// </summary>
    /// <param name="id">数据库备份ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDatabaseBackupByIdAsync(long id)
    {
        var entity = await _databaseBackupRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("数据库备份不存在或已删除");
        }
        if (entity.BackupStatus == BackupStatusRunning)
        {
            throw new TaktBusinessException("执行中的备份不可删除");
        }
        var deleted = await _databaseBackupRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("数据库备份不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除数据库备份
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDatabaseBackupBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDatabaseBackupByIdAsync(id);
        }
    }

    /// <summary>
    /// 校验立即/调度备份请求
    /// </summary>
    /// <param name="dto">请求 DTO</param>
    /// <param name="requireScheduledAt">是否要求计划时间</param>
    private void ValidateRunDto(TaktDatabaseBackupRunDto dto, bool requireScheduledAt)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnforceCurrentTargetTenant(dto);
        if (string.IsNullOrWhiteSpace(dto.TargetTenantCode))
        {
            throw new TaktBusinessException("目标租户不能为空");
        }
        if (dto.TargetTenantCode.Trim().Length != 3)
        {
            throw new TaktBusinessException("目标租户须为 3 位");
        }
        if (string.IsNullOrWhiteSpace(dto.TargetDatabaseName))
        {
            throw new TaktBusinessException("目标数据库名不能为空");
        }
        if (dto.BackupType is not (BackupTypeFull or BackupTypeDelta))
        {
            throw new TaktBusinessException("备份类型须为 1(Full Sync) 或 2(Delta Sync)");
        }
        var backupPath = ResolveBackupPath(dto.BackupPath, dto.BackupPathType);
        if (string.IsNullOrWhiteSpace(backupPath))
        {
            throw new TaktBusinessException("备份目录不能为空");
        }
        dto.BackupPath = backupPath;
        if (requireScheduledAt)
        {
            if (!dto.ScheduledAt.HasValue)
            {
                throw new TaktBusinessException("计划执行时间不能为空");
            }
            if (dto.ScheduledAt.Value <= DateTime.Now)
            {
                throw new TaktBusinessException("计划执行时间须晚于当前时间");
            }
        }
    }

    /// <summary>
    /// 解析备份目录
    /// </summary>
    /// <param name="backupPath">用户输入路径</param>
    /// <param name="pathType">路径类型</param>
    /// <returns>有效目录</returns>
    private string ResolveBackupPath(string backupPath, int pathType)
    {
        // 本地/网络/FTP 均须由用户选择回填，不再回退到固定 DefaultRoot
        _ = pathType;
        return string.IsNullOrWhiteSpace(backupPath) ? string.Empty : backupPath.Trim();
    }

    /// <summary>
    /// 规范化路径类型
    /// </summary>
    private static int NormalizePathType(int pathType)
    {
        return pathType is PathTypeLocal or PathTypeNetwork or PathTypeFtp or PathTypeClient
            ? pathType
            : PathTypeClient;
    }

    /// <summary>
    /// 解析备份文件名（空则 z{库名}_{Full|Delta}_{时间戳}.bak）
    /// </summary>
    /// <param name="fileName">用户文件名</param>
    /// <param name="databaseName">库名</param>
    /// <param name="backupType">1=Full 2=Delta</param>
    /// <returns>合法文件名</returns>
    private static string ResolveFileName(string? fileName, string databaseName, int backupType)
    {
        return TaktDatabaseBackupPathHelper.ResolveBackupFileName(
            fileName,
            string.IsNullOrWhiteSpace(databaseName) ? "db" : databaseName.Trim(),
            backupType <= 0 ? BackupTypeFull : backupType);
    }

    /// <summary>
    /// 加密密码；空则返回 null
    /// </summary>
    private string? EncryptPasswordIfAny(string? plain)
    {
        if (string.IsNullOrWhiteSpace(plain))
        {
            return null;
        }
        var key = _backupOptions.CredentialProtectionKey;
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new TaktBusinessException("未配置 DatabaseBackup:CredentialProtectionKey");
        }
        return TaktCredentialProtectHelper.Protect(plain.Trim(), key);
    }

    /// <summary>
    /// 解密密码
    /// </summary>
    private string? DecryptPassword(string? cipher)
    {
        if (string.IsNullOrWhiteSpace(cipher))
        {
            return null;
        }
        var key = _backupOptions.CredentialProtectionKey;
        if (string.IsNullOrWhiteSpace(key))
        {
            return cipher;
        }
        return TaktCredentialProtectHelper.Unprotect(cipher, key);
    }

    /// <summary>
    /// 实体映射为 DTO（密码不回显）
    /// </summary>
    private static TaktDatabaseBackupDto MapToDto(TaktDatabaseBackup entity)
    {
        var dto = entity.Adapt<TaktDatabaseBackupDto>();
        dto.HasBackupPassword = !string.IsNullOrWhiteSpace(entity.BackupPassword);
        return dto;
    }

    /// <summary>
    /// 列出本机可用驱动器（或非 Windows 的根）作为本地浏览起点
    /// </summary>
    /// <returns>驱动器/根目录项</returns>
    private static List<TaktDatabaseBackupBrowseItem> ListLocalDriveRoots()
    {
        if (OperatingSystem.IsWindows())
        {
            return System.IO.DriveInfo.GetDrives()
                .Where(d => d.IsReady
                    && d.DriveType is DriveType.Fixed or DriveType.Removable or DriveType.Network)
                .OrderBy(d => d.Name, StringComparer.OrdinalIgnoreCase)
                .Select(d =>
                {
                    var root = d.RootDirectory.FullName;
                    var label = string.IsNullOrWhiteSpace(d.VolumeLabel) ? d.DriveType.ToString() : d.VolumeLabel;
                    return new TaktDatabaseBackupBrowseItem
                    {
                        Name = $"{root.TrimEnd('\\')} ({label})",
                        FullPath = root,
                        IsDirectory = true,
                    };
                })
                .ToList();
        }
        return new List<TaktDatabaseBackupBrowseItem>
        {
            new()
            {
                Name = "/",
                FullPath = "/",
                IsDirectory = true,
            },
        };
    }

    /// <summary>
    /// 解析浏览用密码：优先请求明文，否则按配置 Id 解密已存密码
    /// </summary>
    /// <param name="plainPassword">请求中的明文密码</param>
    /// <param name="databaseBackupId">备份配置主键</param>
    /// <returns>明文密码；无则 null</returns>
    private async Task<string?> ResolveBrowsePasswordAsync(string? plainPassword, long? databaseBackupId)
    {
        if (!string.IsNullOrWhiteSpace(plainPassword))
        {
            return plainPassword;
        }
        if (databaseBackupId is null or <= 0)
        {
            return null;
        }
        var entity = await _databaseBackupRepository.GetByIdAsync(databaseBackupId.Value);
        if (entity == null
            || entity.IsDeleted != 0
            || entity.TenantCode != CurrentTenantCode
            || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return DecryptPassword(entity.BackupPassword);
    }

    /// <summary>
    /// 生成默认备份编码：{库名}_{yyyyMMddHHmmss}（最长 40）
    /// </summary>
    /// <param name="databaseName">目标数据库展示名</param>
    /// <returns>备份编码</returns>
    private static string GenerateBackupCode(string? databaseName)
    {
        var stamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var safe = string.IsNullOrWhiteSpace(databaseName) ? "db" : databaseName.Trim();
        var sb = new System.Text.StringBuilder(safe.Length);
        foreach (var ch in safe)
        {
            if (char.IsAsciiLetterOrDigit(ch) || ch is '_' or '-' or '.')
            {
                sb.Append(ch);
            }
            else
            {
                sb.Append('_');
            }
        }
        safe = sb.Length == 0 ? "db" : sb.ToString();
        var maxDbLen = Math.Max(1, 40 - 1 - stamp.Length);
        if (safe.Length > maxDbLen)
        {
            safe = safe[..maxDbLen];
        }
        return $"{safe}_{stamp}";
    }

    /// <summary>
    /// 创建备份记录实体并持久化
    /// </summary>
    /// <param name="dto">运行请求</param>
    /// <param name="executeMode">执行方式</param>
    /// <param name="backupStatus">初始状态</param>
    /// <returns>已持久化实体</returns>
    private async Task<TaktDatabaseBackup> CreateBackupRecordAsync(
        TaktDatabaseBackupRunDto dto,
        int executeMode,
        int backupStatus)
    {
        var entity = new TaktDatabaseBackup
        {
            BackupCode = GenerateBackupCode(dto.TargetDatabaseName),
            TargetTenantCode = dto.TargetTenantCode.Trim(),
            TargetDatabaseName = dto.TargetDatabaseName.Trim(),
            BackupType = dto.BackupType,
            ExecuteMode = executeMode,
            BackupPathType = NormalizePathType(dto.BackupPathType),
            BackupPath = dto.BackupPath,
            BackupHost = string.IsNullOrWhiteSpace(dto.BackupHost) ? null : dto.BackupHost.Trim(),
            BackupPort = dto.BackupPathType == PathTypeFtp ? (dto.BackupPort is > 0 ? dto.BackupPort : 21) : null,
            BackupUserName = string.IsNullOrWhiteSpace(dto.BackupUserName) ? null : dto.BackupUserName.Trim(),
            BackupPassword = EncryptPasswordIfAny(dto.BackupPassword),
            BackupFileName = ResolveFileName(dto.BackupFileName, dto.TargetDatabaseName, dto.BackupType),
            ScheduledAt = dto.ScheduledAt,
            BackupStatus = backupStatus,
            Remark = dto.Remark,
        };
        return await _databaseBackupRepository.CreateAsync(entity);
    }

    /// <summary>
    /// 调用 Provider 执行 BACKUP DATABASE；结果明细只写 BackupLog，配置表仅更新状态摘要
    /// </summary>
    /// <param name="entity">备份配置</param>
    /// <returns>实际落盘路径</returns>
    private async Task<string> ExecuteBackupCoreAsync(TaktDatabaseBackup entity)
    {
        var startedAt = DateTime.Now;
        entity.BackupStatus = BackupStatusRunning;
        entity.LastRunAt = startedAt;
        await _databaseBackupRepository.UpdateAsync(entity);
        var log = new TaktBackupLog
        {
            BackupKind = "database",
            SourceId = entity.Id.ToString(),
            SourceCode = entity.BackupCode,
            TargetName = entity.TargetDatabaseName,
            TargetScope = entity.TargetTenantCode,
            SyncMode = entity.BackupType,
            ExecuteMode = entity.ExecuteMode,
            PathType = entity.BackupPathType,
            RunStatus = 0,
            StartedAt = startedAt,
        };
        log = await _backupLogRepository.CreateAsync(log);
        var options = new TaktDatabaseBackupOptionsModel
        {
            TargetTenantCode = entity.TargetTenantCode,
            TargetDatabaseName = entity.TargetDatabaseName,
            BackupType = entity.BackupType,
            BackupPathType = NormalizePathType(entity.BackupPathType),
            BackupDirectory = entity.BackupPath,
            BackupFileName = TaktDatabaseBackupPathHelper.ResolveBackupFileNameForRun(
                entity.BackupFileName,
                string.IsNullOrWhiteSpace(entity.TargetDatabaseName) ? "db" : entity.TargetDatabaseName.Trim(),
                entity.BackupType),
            BackupHost = entity.BackupHost,
            BackupPort = entity.BackupPort,
            BackupUserName = entity.BackupUserName,
            BackupPassword = DecryptPassword(entity.BackupPassword),
            FtpTempRoot = _backupOptions.FtpTempRoot,
        };
        TaktDatabaseBackupResult result;
        try
        {
            result = await _backupProvider.BackupAsync(options);
        }
        catch (Exception ex)
        {
            var finishedAt = DateTime.Now;
            entity.BackupStatus = BackupStatusFailed;
            entity.LastRunAt = finishedAt;
            await _databaseBackupRepository.UpdateAsync(entity);
            log.RunStatus = 2;
            log.FinishedAt = finishedAt;
            log.ErrorMessage = ex.Message;
            await _backupLogRepository.UpdateAsync(log);
            throw;
        }
        var doneAt = DateTime.Now;
        if (result.Success)
        {
            entity.BackupStatus = BackupStatusSuccess;
            log.RunStatus = 1;
            log.ResultPath = result.BackupFilePath;
            log.FileSizeBytes = result.FileSizeBytes;
            log.ErrorMessage = null;
        }
        else
        {
            entity.BackupStatus = BackupStatusFailed;
            log.RunStatus = 2;
            log.ErrorMessage = result.ErrorMessage;
        }
        entity.LastRunAt = doneAt;
        log.FinishedAt = doneAt;
        await _databaseBackupRepository.UpdateAsync(entity);
        await _backupLogRepository.UpdateAsync(log);
        if (!result.Success)
        {
            throw new TaktBusinessException(result.ErrorMessage ?? "数据库备份失败");
        }
        return result.BackupFilePath;
    }

    /// <summary>
    /// 构建一次性 Quartz 任务创建 DTO
    /// </summary>
    /// <param name="entity">备份记录</param>
    /// <param name="scheduledAt">计划执行时间</param>
    /// <returns>Quartz 创建 DTO</returns>
    private TaktQuartzTaskCreateDto BuildQuartzTaskCreateDto(TaktDatabaseBackup entity, DateTime scheduledAt)
    {
        var executeParams = System.Text.Json.JsonSerializer.Serialize(new { backupId = entity.Id.ToString() });
        // TaskCode 上限 varchar(50)；勿截断雪花 Id，用短 Guid 保证唯一
        var uniq = Guid.NewGuid().ToString("N")[..8];
        var suffix = $"{entity.Id}_{uniq}";
        return new TaktQuartzTaskCreateDto
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            TaskCode = $"QT_DB_BAK_{suffix}",
            TaskName = $"数据库备份 {entity.TargetDatabaseName}",
            JobName = $"db_backup_{suffix}",
            JobGroup = QuartzJobGroup,
            TaskType = QuartzTaskTypeAssembly,
            AssemblyName = QuartzAssemblyName,
            ClassName = QuartzHandlerClassName,
            TriggerType = 0,
            IntervalSeconds = 0,
            CronExpression = string.Empty,
            FirstRunAt = scheduledAt,
            ExecuteParams = executeParams,
            Concurrent = 0,
            MisfirePolicy = 0,
            TaskStatus = 0,
            TaskDescription = $"数据库备份调度 backupId={entity.Id}",
            Remark = entity.Remark,
        };
    }

    /// <summary>
    /// 构建数据库备份查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDatabaseBackup, bool>> QueryExpression(TaktDatabaseBackupQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDatabaseBackup>();
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.BackupCode != null && x.BackupCode.Contains(keywords))
                || (x.TargetTenantCode != null && x.TargetTenantCode.Contains(keywords))
                || (x.TargetDatabaseName != null && x.TargetDatabaseName.Contains(keywords))
                || SqlFunc.ToString(x.BackupType).Contains(keywords)
                || SqlFunc.ToString(x.ExecuteMode).Contains(keywords)
                || (x.BackupPath != null && x.BackupPath.Contains(keywords))
                || (x.BackupFileName != null && x.BackupFileName.Contains(keywords))
                || SqlFunc.ToString(x.BackupStatus).Contains(keywords)
                || (x.Remark != null && x.Remark.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.BackupCode))
        {
            exp = exp.And(x => x.BackupCode != null && x.BackupCode.Contains(queryDto.BackupCode));
        }
        if (!string.IsNullOrEmpty(queryDto?.TargetTenantCode))
        {
            exp = exp.And(x => x.TargetTenantCode != null && x.TargetTenantCode.Contains(queryDto.TargetTenantCode));
        }
        if (!string.IsNullOrEmpty(queryDto?.TargetDatabaseName))
        {
            exp = exp.And(x => x.TargetDatabaseName != null && x.TargetDatabaseName.Contains(queryDto.TargetDatabaseName));
        }
        if (queryDto?.BackupType.HasValue == true)
        {
            exp = exp.And(x => x.BackupType == queryDto.BackupType);
        }
        if (queryDto?.BackupPathType.HasValue == true)
        {
            exp = exp.And(x => x.BackupPathType == queryDto.BackupPathType);
        }
        if (queryDto?.ExecuteMode.HasValue == true)
        {
            exp = exp.And(x => x.ExecuteMode == queryDto.ExecuteMode);
        }
        if (queryDto?.BackupStatus.HasValue == true)
        {
            exp = exp.And(x => x.BackupStatus == queryDto.BackupStatus);
        }
        if (queryDto?.ScheduledAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledAt >= queryDto.ScheduledAtStart);
        }
        if (queryDto?.ScheduledAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledAt <= queryDto.ScheduledAtEnd);
        }
        if (queryDto?.LastRunAtStart.HasValue == true)
        {
            exp = exp.And(x => x.LastRunAt >= queryDto.LastRunAtStart);
        }
        if (queryDto?.LastRunAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastRunAt <= queryDto.LastRunAtEnd);
        }
        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }
        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
