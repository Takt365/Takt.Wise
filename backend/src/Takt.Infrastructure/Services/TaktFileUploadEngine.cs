// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktFileUploadEngine.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：通用文件上传下载引擎实现（本地 + OSS aliyun + FTP；分片在本地合并后推送远程）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Globalization;
using Takt.Application.Services;
using Takt.Domain.Interfaces;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using static Takt.Shared.Helpers.TaktFileHelper;
using static Takt.Shared.Helpers.TaktValidationI18nKeys;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktFileUploadEngine 实现：物理文件 I/O，与业务表无关，供各模块应用服务复用。
/// </summary>
/// <remarks>
/// 支持 0=本地 wwwroot、1=OSS（当前 aliyun）、2=FTP。
/// 分片始终在本地合并，再按 StorageType 推送至远程并删除本地副本。
/// 配置来源 TaktFileUploadOptions 与 appsettings Oss/Ftp 节。
/// </remarks>
public sealed class TaktFileUploadEngine : TaktServiceBase, ITaktFileUploadEngine
{
    /// <summary>
    /// 分片临时文件写入锁（按绝对路径串行，防止并发上传同一 .part 导致 IOException）
    /// </summary>
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> ChunkPartWriteLocks = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Web 宿主环境（解析 ContentRoot 与 wwwroot）
    /// </summary>
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// 上传配置（大小上限、扩展名白名单、相对路径等）
    /// </summary>
    private readonly TaktFileUploadOptions _uploadOptions;

    /// <summary>
    /// 应用配置（解析 Oss/Ftp 提供商节点）
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 编码生成器（按 MIME → FD-F* 规则取 FileCode）
    /// </summary>
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 初始化文件上传引擎
    /// </summary>
    /// <param name="webHostEnvironment">Web 宿主环境</param>
    /// <param name="uploadOptions">上传配置</param>
    /// <param name="configuration">应用配置</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFileUploadEngine(
        IWebHostEnvironment webHostEnvironment,
        IOptions<TaktFileUploadOptions> uploadOptions,
        IConfiguration configuration,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        ArgumentNullException.ThrowIfNull(webHostEnvironment);
        ArgumentNullException.ThrowIfNull(uploadOptions);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(numberingGenerator);
        _webHostEnvironment = webHostEnvironment;
        _uploadOptions = uploadOptions.Value;
        _configuration = configuration;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 整文件上传（本地 wwwroot 或经暂存区推送 OSS/FTP）
    /// </summary>
    /// <param name="fileStream">文件流；不可为 null</param>
    /// <param name="fileName">原始文件名（仅取 Path.GetFileName 安全段）</param>
    /// <param name="contentType">MIME 类型；为空时按扩展名推断</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存储结果（相对路径、哈希、访问 URL 等，不含业务表 Id）</returns>
    /// <exception cref="ArgumentNullException">fileStream 为 null</exception>
    /// <exception cref="ArgumentException">fileName 为空或 identifier 类参数非法</exception>
    public async Task<TaktStoredFileResult> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        if (fileStream.CanSeek && fileStream.Length == 0)
        {
            ThrowLocalizedException(FileEmpty);
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var resolvedScope = ResolveScope(scope);
        var safeOriginalName = Path.GetFileName(fileName);
        ValidateFileName(safeOriginalName);
        var fileExtension = GetNormalizedExtension(safeOriginalName);
        if (fileStream.CanSeek && fileStream.Length > _uploadOptions.MaxFileSizeBytes)
        {
            ThrowFileSizeExceededException();
        }

        var fileMimeType = string.IsNullOrWhiteSpace(contentType) ? GetMimeType(safeOriginalName) : contentType.Trim();
        var fileCode = await GenerateFileCodeFromMimeAsync(fileMimeType, cancellationToken);
        var fileHash = await ComputeStreamHashAsync(fileStream);
        if (fileStream.CanSeek)
        {
            fileStream.Position = 0;
        }

        var finalFileName = TaktFileHelper.ResolveStoredFileName(
            resolvedScope.StorageNaming,
            safeOriginalName,
            fileHash,
            resolvedScope.TargetFileName);
        ValidateFileName(finalFileName);
        var relativePath = BuildStoredRelativePath(resolvedScope, finalFileName);
        var absolutePath = GetLocalWriteAbsolutePath(resolvedScope, relativePath);
        EnsureDirectoryExists(Path.GetDirectoryName(absolutePath)!);
        await using (var fileWriteStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await fileStream.CopyToAsync(fileWriteStream, cancellationToken);
        }

        var fileSize = GetFileSize(absolutePath);
        EnsureFileSizeWithinLimit(fileSize);
        var localResult = BuildStoredFileResult(
            fileCode,
            safeOriginalName,
            finalFileName,
            relativePath,
            fileSize,
            fileMimeType,
            fileExtension,
            fileHash,
            MapFileCategory(GetFileCategoryFromMimeType(fileMimeType)),
            resolvedScope.StorageType,
            resolvedScope.StorageConfig);
        return await FinalizeStoredFileAsync(
            localResult,
            resolvedScope,
            absolutePath,
            fileMimeType,
            cancellationToken);
    }

    /// <summary>
    /// 获取上传策略（MaxChunkCount、ChunkRelativePath、分片计划等）
    /// </summary>
    /// <param name="totalSizeBytes">文件总大小；为空时仅返回全局配置</param>
    /// <returns>上传策略 DTO</returns>
    public TaktFileUploadPolicyResult GetUploadPolicy(long? totalSizeBytes = null)
    {
        var policy = BuildBaseUploadPolicy();
        if (!totalSizeBytes.HasValue || totalSizeBytes.Value <= 0)
        {
            return policy;
        }

        var plan = ResolveChunkPlan(totalSizeBytes.Value);
        policy.UseChunkUpload = plan.UseChunkUpload;
        policy.ChunkSizeBytes = plan.ChunkSizeBytes;
        policy.TotalChunks = plan.TotalChunks;
        policy.TotalSizeBytes = plan.TotalSizeBytes;
        return policy;
    }

    /// <summary>
    /// 检查指定分片 part 文件是否已存在（断点续传）
    /// </summary>
    /// <param name="request">检查参数（identifier、chunkNumber、totalSize 等）</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <returns>分片是否存在</returns>
    /// <exception cref="ArgumentNullException">request 为 null</exception>
    /// <exception cref="ArgumentException">identifier 非法或过长</exception>
    public Task<TaktFileChunkCheckResult> CheckChunkAsync(
        TaktFileChunkCheckRequest request,
        TaktFileUploadScope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        EnsureFileSizeWithinLimit(request.TotalSize);
        if (request.TotalSize > 0)
        {
            var plan = ResolveChunkPlan(request.TotalSize);
            if (request.TotalChunks > 0 && request.TotalChunks != plan.TotalChunks)
            {
                ThrowLocalizedException(FileUploadChunkPlanMismatch);
            }

            if (request.ChunkNumber > 0)
            {
                EnsureChunkMetadataMatchesPlan(
                    plan,
                    plan.TotalChunks,
                    request.ChunkNumber,
                    request.ChunkSize,
                    request.ChunkSize);
            }
        }

        var chunkPath = GetChunkPartPath(resolvedScope, request.Identifier, request.ChunkNumber);
        return Task.FromResult(new TaktFileChunkCheckResult
        {
            Exists = FileExists(chunkPath),
        });
    }

    /// <summary>
    /// 列出指定 identifier 下已落盘的分片序号（断点续传批量恢复）
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <returns>已上传分片序号（升序）</returns>
    public Task<TaktFileChunkListResult> ListUploadedChunksAsync(
        TaktFileChunkListRequest request,
        TaktFileUploadScope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        TaktFileChunkPlan? plan = null;
        if (request.TotalSize > 0)
        {
            plan = ResolveChunkPlan(request.TotalSize);
            if (request.TotalChunks > 0 && request.TotalChunks != plan.TotalChunks)
            {
                ThrowLocalizedException(FileUploadChunkPlanMismatch);
            }
        }

        var dir = GetChunkDirectory(resolvedScope, request.Identifier);
        var uploaded = new List<int>();
        if (Directory.Exists(dir))
        {
            foreach (var partPath in Directory.GetFiles(dir, "*.part"))
            {
                var chunkName = Path.GetFileNameWithoutExtension(partPath);
                if (!int.TryParse(chunkName, out var chunkNumber) || chunkNumber < 1)
                {
                    continue;
                }

                var maxChunks = plan?.TotalChunks ?? request.TotalChunks;
                if (maxChunks > 0 && chunkNumber > maxChunks)
                {
                    continue;
                }

                if (plan != null)
                {
                    var expectedSize = TaktFileHelper.GetExpectedChunkSize(plan, chunkNumber);
                    if (GetFileSize(partPath) != expectedSize)
                    {
                        continue;
                    }
                }

                uploaded.Add(chunkNumber);
            }
        }

        uploaded.Sort();
        return Task.FromResult(new TaktFileChunkListResult
        {
            UploadedChunkNumbers = uploaded,
        });
    }

    /// <summary>
    /// 取消分片上传并删除临时目录
    /// </summary>
    /// <param name="identifier">上传会话标识</param>
    /// <param name="scope">租户/公司隔离范围</param>
    public Task CancelUploadedChunksAsync(string identifier, TaktFileUploadScope? scope = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(identifier);
        DeleteChunkDirectory(resolvedScope, identifier);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 上传单个分片至临时目录（{chunkRelativePath}/{tenant}/{company}/{identifier}/{n}.part）
    /// </summary>
    /// <param name="chunkStream">分片二进制流</param>
    /// <param name="request">分片元数据（序号、总数、文件大小等）</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="ArgumentNullException">chunkStream 或 request 为 null</exception>
    /// <exception cref="ArgumentException">identifier 或文件名非法</exception>
    public async Task UploadChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chunkStream);
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        ValidateFileName(request.FileName);
        EnsureFileSizeWithinLimit(request.TotalSize);
        var plan = ResolveChunkPlan(request.TotalSize);
        EnsureChunkMetadataMatchesPlan(
            plan,
            request.TotalChunks,
            request.ChunkNumber,
            request.ChunkSize,
            request.ChunkSize);
        if (request.ChunkNumber < 1 || request.ChunkNumber > request.TotalChunks)
        {
            ThrowLocalizedException(FileUploadChunkIndexInvalid);
        }

        var chunkPath = GetChunkPartPath(resolvedScope, request.Identifier, request.ChunkNumber);
        var expectedSize = TaktFileHelper.GetExpectedChunkSize(plan, request.ChunkNumber);
        var chunkLock = ChunkPartWriteLocks.GetOrAdd(chunkPath, _ => new SemaphoreSlim(1, 1));
        await chunkLock.WaitAsync(cancellationToken);
        try
        {
            if (FileExists(chunkPath) && GetFileSize(chunkPath) == expectedSize)
            {
                return;
            }

            long actualLength = 0;
            if (chunkStream.CanSeek)
            {
                actualLength = chunkStream.Length;
            }
            else
            {
                await WriteFileFromStreamAsync(chunkPath, chunkStream, createDirectory: true, cancellationToken);
                actualLength = GetFileSize(chunkPath);
            }

            if (chunkStream.CanSeek)
            {
                if (actualLength != expectedSize)
                {
                    ThrowLocalizedException(FileUploadChunkSizeMismatch);
                }

                await WriteFileFromStreamAsync(chunkPath, chunkStream, createDirectory: true, cancellationToken);
                return;
            }

            if (actualLength != expectedSize)
            {
                DeleteFile(chunkPath, throwIfNotExists: false);
                ThrowLocalizedException(FileUploadChunkSizeMismatch);
            }
        }
        finally
        {
            chunkLock.Release();
        }
    }

    /// <summary>
    /// 按序合并分片为完整文件，校验总大小后写入正式存储路径并清理分片临时目录
    /// </summary>
    /// <param name="request">合并参数（identifier、fileName、totalChunks、totalSize）</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>与整文件上传一致的存储结果</returns>
    /// <exception cref="ArgumentNullException">request 为 null</exception>
    /// <exception cref="ArgumentException">identifier 或文件名非法</exception>
    public async Task<TaktStoredFileResult> MergeChunksAsync(
        TaktFileChunkMergeRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        ValidateFileName(request.FileName);
        EnsureFileSizeWithinLimit(request.TotalSize);
        var plan = ResolveChunkPlan(request.TotalSize);
        if (request.TotalChunks != plan.TotalChunks)
        {
            ThrowLocalizedException(FileUploadChunkPlanMismatch);
        }

        var safeOriginalName = Path.GetFileName(request.FileName);
        ValidateFileName(safeOriginalName);
        var fileMimeType = GetMimeType(safeOriginalName);
        var fileCode = await GenerateFileCodeFromMimeAsync(fileMimeType, cancellationToken);
        var finalFileName = TaktFileHelper.ResolveStoredFileName(
            resolvedScope.StorageNaming,
            safeOriginalName,
            request.Identifier,
            resolvedScope.TargetFileName);
        ValidateFileName(finalFileName);
        var relativePath = BuildStoredRelativePath(resolvedScope, finalFileName);
        var absolutePath = GetLocalWriteAbsolutePath(resolvedScope, relativePath);
        EnsureDirectoryExists(Path.GetDirectoryName(absolutePath)!);

        await using (var output = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            long written = 0;
            for (var i = 1; i <= request.TotalChunks; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var chunkPath = GetChunkPartPath(resolvedScope, request.Identifier, i);
                if (!FileExists(chunkPath))
                {
                    ThrowLocalizedException(
                        FileUploadChunkMissing,
                        extraTokens: new Dictionary<string, string>
                        {
                            ["index"] = i.ToString(CultureInfo.InvariantCulture),
                        });
                }

                await using var chunkStream = ReadFileStream(chunkPath);
                await chunkStream.CopyToAsync(output, cancellationToken);
                written = checked(written + chunkStream.Length);
            }

            if (request.TotalSize > 0 && written != request.TotalSize)
            {
                DeleteFile(absolutePath, throwIfNotExists: false);
                ThrowLocalizedException(FileUploadMergeSizeMismatch);
            }
        }

        EnsureFileSizeWithinLimit(GetFileSize(absolutePath));
        DeleteChunkDirectory(resolvedScope, request.Identifier);
        var localResult = await BuildStoredFileResultAsync(
            safeOriginalName,
            finalFileName,
            relativePath,
            null,
            fileCode,
            request.Identifier);
        return await FinalizeStoredFileAsync(
            localResult,
            resolvedScope,
            absolutePath,
            null,
            cancellationToken);
    }

    /// <summary>
    /// 按存储描述符打开本地只读文件流（调用方负责释放 Stream）
    /// </summary>
    /// <param name="descriptor">存储定位（FilePath + StorageType）</param>
    /// <param name="downloadFileName">下载展示文件名；为空时使用路径末段</param>
    /// <param name="contentType">MIME；为空时按扩展名推断</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流、文件名与 ContentType</returns>
    /// <exception cref="ArgumentNullException">descriptor 为 null</exception>
    /// <exception cref="ArgumentException">FilePath 为空</exception>
    public Task<TaktFileDownloadStreamResult> OpenReadAsync(
        TaktFileStorageDescriptor descriptor,
        string? downloadFileName = null,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.FilePath);
        var name = string.IsNullOrWhiteSpace(downloadFileName)
            ? Path.GetFileName(descriptor.FilePath)
            : downloadFileName;
        var mime = string.IsNullOrWhiteSpace(contentType) ? GetMimeType(name) : contentType;
        cancellationToken.ThrowIfCancellationRequested();
        if (descriptor.StorageType == TaktFileHelper.StorageTypeLocal)
        {
            var absolutePath = GetAbsoluteUploadPath(descriptor.FilePath);
            if (!FileExists(absolutePath))
            {
                ThrowLocalizedException(FilePhysicalNotFound);
            }

            return Task.FromResult(new TaktFileDownloadStreamResult
            {
                Stream = ReadFileStream(absolutePath),
                FileName = name,
                ContentType = mime,
            });
        }

        if (descriptor.StorageType == TaktFileHelper.StorageTypeOss)
        {
            return OpenReadFromOssAsync(descriptor, name, mime, cancellationToken);
        }

        if (descriptor.StorageType == TaktFileHelper.StorageTypeFtp)
        {
            return OpenReadFromFtpAsync(descriptor, name, mime, cancellationToken);
        }

        ThrowLocalizedException(FileStorageReadUnsupported);
        return null!;
    }

    /// <summary>
    /// 删除本地物理文件（文件不存在时不抛异常）
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="ArgumentNullException">descriptor 为 null</exception>
    public async Task DeleteStoredFileAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        if (descriptor.StorageType == TaktFileHelper.StorageTypeLocal)
        {
            var absolutePath = GetAbsoluteUploadPath(descriptor.FilePath);
            cancellationToken.ThrowIfCancellationRequested();
            DeleteFile(absolutePath, throwIfNotExists: false);
            return;
        }

        if (descriptor.StorageType == TaktFileHelper.StorageTypeOss)
        {
            await DeleteOssObjectAsync(descriptor, cancellationToken);
            return;
        }

        if (descriptor.StorageType == TaktFileHelper.StorageTypeFtp)
        {
            await DeleteFtpObjectAsync(descriptor, cancellationToken);
            return;
        }

        ThrowLocalizedException(FileStorageDeleteUnsupported);
    }

    /// <summary>
    /// 将本地物理文件重命名为带删除标记的文件名（xxx.ext → xxx.del.ext）
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>重命名后的相对路径</returns>
    /// <exception cref="ArgumentNullException">descriptor 为 null</exception>
    /// <exception cref="ArgumentException">FilePath 为空</exception>
    public async Task<string> MarkStoredFileDeletedAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.FilePath);
        if (descriptor.StorageType == TaktFileHelper.StorageTypeLocal)
        {
            return await MarkLocalStoredFileDeletedAsync(descriptor, cancellationToken);
        }

        if (descriptor.StorageType == TaktFileHelper.StorageTypeOss)
        {
            return await MarkOssStoredFileDeletedAsync(descriptor, cancellationToken);
        }

        if (descriptor.StorageType == TaktFileHelper.StorageTypeFtp)
        {
            return await MarkFtpStoredFileDeletedAsync(descriptor, cancellationToken);
        }

        ThrowLocalizedException(FileStorageMarkDeleteUnsupported);
        return descriptor.FilePath;
    }

    /// <summary>
    /// 本地物理文件标记删除（xxx.ext → xxx.del.ext）
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>重命名后的相对路径</returns>
    private Task<string> MarkLocalStoredFileDeletedAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken)
    {
        var relativePath = descriptor.FilePath.Replace('\\', '/').TrimStart('/');
        var fileName = Path.GetFileName(relativePath);
        if (IsDeletedPhysicalFileName(fileName))
        {
            return Task.FromResult(relativePath);
        }

        var absolutePath = GetAbsoluteUploadPath(relativePath);
        cancellationToken.ThrowIfCancellationRequested();
        if (!FileExists(absolutePath))
        {
            TaktLogger.Warning("[TaktFileUploadEngine] 标记删除时物理文件不存在: {FilePath}", relativePath);
            return Task.FromResult(BuildDeletedPhysicalRelativePath(relativePath));
        }

        var deletedRelativePath = BuildDeletedPhysicalRelativePath(relativePath);
        var deletedAbsolutePath = GetAbsoluteUploadPath(deletedRelativePath);
        var attempt = 0;
        while (FileExists(deletedAbsolutePath) && attempt < 100)
        {
            attempt = checked(attempt + 1);
            var extension = Path.GetExtension(fileName);
            var baseName = Path.GetFileNameWithoutExtension(fileName);
            var suffixedFileName = string.IsNullOrEmpty(extension)
                ? $"{baseName}{DeletedPhysicalFileMarker}.{attempt}"
                : $"{baseName}{DeletedPhysicalFileMarker}.{attempt}{extension}";
            var directory = Path.GetDirectoryName(relativePath.Replace('/', Path.DirectorySeparatorChar));
            deletedRelativePath = string.IsNullOrWhiteSpace(directory)
                ? suffixedFileName
                : $"{directory.Replace('\\', '/')}/{suffixedFileName}";
            deletedAbsolutePath = GetAbsoluteUploadPath(deletedRelativePath);
        }

        if (FileExists(deletedAbsolutePath))
        {
            ThrowLocalizedException(FilePhysicalDeleteTargetExists);
        }

        MoveFile(absolutePath, deletedAbsolutePath);
        TaktLogger.Information(
            "[TaktFileUploadEngine] 物理文件已标记删除: {SourcePath} -> {TargetPath}",
            relativePath,
            deletedRelativePath);
        return Task.FromResult(deletedRelativePath);
    }

    /// <summary>
    /// 解析租户/公司隔离范围；显式 scope 不完整时回退到当前三层上下文
    /// </summary>
    /// <param name="scope">显式范围；TenantCode/CompanyCode 均有效时直接使用</param>
    /// <returns>含 TenantCode、CompanyCode 与可选 CategoryPath 的有效范围</returns>
    private TaktFileUploadScope ResolveScope(TaktFileUploadScope? scope)
    {
        if (scope != null
            && !string.IsNullOrWhiteSpace(scope.TenantCode)
            && !string.IsNullOrWhiteSpace(scope.CompanyCode))
        {
            return scope;
        }

        EnsureThreeLayerContext();
        return new TaktFileUploadScope
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            CategoryPath = scope?.CategoryPath,
            FileUploadType = scope?.FileUploadType ?? TaktFileUploadType.Normal,
            TargetFileName = scope?.TargetFileName,
            StorageNaming = scope?.StorageNaming ?? 0,
            StorageType = scope?.StorageType ?? TaktFileHelper.StorageTypeLocal,
            StorageConfig = scope?.StorageConfig,
        };
    }

    /// <summary>
    /// 按 MIME 映射 TaktNumbering 规则码（FD-FDOC/IMG/VID/AUD/ARC/OTH）并生成 FileCode
    /// </summary>
    /// <param name="fileMimeType">MIME 类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>业务编码</returns>
    private async Task<string> GenerateFileCodeFromMimeAsync(
        string fileMimeType,
        CancellationToken cancellationToken = default)
    {
        var ruleCode = ResolveFileNumberingRuleCode(fileMimeType);
        var generated = await _numberingGenerator.GenerateNextAsync(ruleCode, cancellationToken);
        if (string.IsNullOrWhiteSpace(generated.BusinessCode))
        {
            throw new TaktBusinessException("文件编码生成失败");
        }

        return generated.BusinessCode;
    }

    /// <summary>
    /// MIME → 文件编码规则码（与 TaktNumberingSeedData FD-F* 对齐）
    /// </summary>
    /// <param name="fileMimeType">MIME 类型</param>
    /// <returns>规则码</returns>
    private static string ResolveFileNumberingRuleCode(string fileMimeType)
    {
        return GetFileCategoryFromMimeType(fileMimeType) switch
        {
            FileCategory.Document => "FD-FDOC",
            FileCategory.Image => "FD-FIMG",
            FileCategory.Video => "FD-FVID",
            FileCategory.Audio => "FD-FAUD",
            FileCategory.Archive => "FD-FARC",
            _ => "FD-FOTH",
        };
    }

    /// <summary>
    /// 构建存储结果（含大小、哈希、MIME、分类与访问 URL）
    /// </summary>
    /// <param name="originalName">原始文件名</param>
    /// <param name="storedFileName">磁盘存储文件名</param>
    /// <param name="relativePath">相对 wwwroot 的存储路径</param>
    /// <param name="contentType">MIME；为空时按扩展名推断</param>
    /// <param name="fileCode">文件编码；为空时不填充</param>
    /// <param name="precomputedHash">预计算 MD5；为空时从磁盘文件计算</param>
    /// <returns>与 TaktFile 存储列对齐的结果 DTO</returns>
    private async Task<TaktStoredFileResult> BuildStoredFileResultAsync(
        string originalName,
        string storedFileName,
        string relativePath,
        string? contentType,
        string? fileCode = null,
        string? precomputedHash = null)
    {
        var absolutePath = GetAbsoluteUploadPath(relativePath);
        var fileSize = GetFileSize(absolutePath);
        EnsureFileSizeWithinLimit(fileSize);
        var extension = GetNormalizedExtension(originalName);
        var fileHash = precomputedHash ?? await ComputeFileHashAsync(absolutePath);
        return BuildStoredFileResult(
            fileCode ?? string.Empty,
            originalName,
            storedFileName,
            relativePath,
            fileSize,
            string.IsNullOrWhiteSpace(contentType) ? GetMimeType(originalName) : contentType.Trim(),
            extension,
            fileHash,
            MapFileCategory(GetFileCategoryFromMimeType(
                string.IsNullOrWhiteSpace(contentType) ? GetMimeType(originalName) : contentType.Trim())));
    }

    /// <summary>
    /// 组装 TaktStoredFileResult（字段与 Routine.Tasks <c>FileUploadResultDto</c> 对齐）
    /// </summary>
    /// <param name="fileCode">文件编码</param>
    /// <param name="originalName">原始文件名</param>
    /// <param name="storedFileName">存储文件名</param>
    /// <param name="relativePath">相对路径</param>
    /// <param name="fileSize">文件大小</param>
    /// <param name="fileMimeType">MIME 类型</param>
    /// <param name="fileExtension">扩展名（不含点）</param>
    /// <param name="fileHash">MD5 哈希</param>
    /// <param name="fileCategory">文件分类</param>
    /// <returns>存储结果</returns>
    private static TaktStoredFileResult BuildStoredFileResult(
        string fileCode,
        string originalName,
        string storedFileName,
        string relativePath,
        long fileSize,
        string fileMimeType,
        string fileExtension,
        string fileHash,
        int fileCategory,
        int storageType = TaktFileHelper.StorageTypeLocal,
        string? storageConfig = null) =>
        new()
        {
            FileCode = fileCode,
            FilePath = relativePath,
            FileName = storedFileName,
            FileOriginalName = originalName,
            FileSize = fileSize,
            FileType = fileMimeType,
            FileExtension = fileExtension,
            FileHash = fileHash,
            AccessUrl = BuildAccessUrl(relativePath, storageType, storageConfig),
            StorageType = storageType,
            StorageConfig = storageConfig,
            FileCategory = fileCategory,
        };

    /// <summary>
    /// 将 TaktFileHelper.FileCategory 映射为 TaktFileCategory 枚举
    /// </summary>
    /// <param name="category">Helper 侧文件分类</param>
    /// <returns>实体 FileCategory 值</returns>
    private static int MapFileCategory(FileCategory category) =>
        (int)category;

    /// <summary>
    /// 构建正式存储相对路径：{UploadRelativePath}/[{CategoryPath}/]{tenant}/{company}/{yyyy/MM/dd}/{storedFileName}
    /// </summary>
    /// <param name="scope">租户/公司与业务子路径（CategoryPath 含模块与文件类型目录，如 human-resource/images）</param>
    /// <param name="storedFileName">唯一存储文件名</param>
    /// <returns>以 / 分隔的相对路径</returns>
    private string BuildStoredRelativePath(TaktFileUploadScope scope, string storedFileName)
    {
        var dateSegment = GenerateDatePath(DateTime.Now).Replace('\\', '/');
        var segments = new List<string>
        {
            _uploadOptions.UploadRelativePath,
        };
        if (!string.IsNullOrWhiteSpace(scope.CategoryPath))
        {
            segments.Add(NormalizeCategoryPath(scope.CategoryPath));
        }

        segments.Add(scope.TenantCode);
        segments.Add(scope.CompanyCode);
        segments.Add(dateSegment);
        segments.Add(storedFileName);
        return string.Join('/', segments);
    }

    /// <summary>
    /// 获取规范化扩展名（小写、不含点）
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>扩展名；无扩展名时返回空串</returns>
    private static string GetNormalizedExtension(string fileName) =>
        Path.GetExtension(fileName)?.TrimStart('.')?.ToLowerInvariant() ?? string.Empty;

    /// <summary>
    /// 拒绝配置中的禁止扩展名（<see cref="TaktFileUploadOptions.DeniedExtensions"/>）
    /// </summary>
    /// <param name="fileExtension">扩展名（不含点）</param>
    private void ValidateDeniedExtension(string fileExtension)
    {
        if (string.IsNullOrWhiteSpace(fileExtension) || _uploadOptions.DeniedExtensions.Length == 0)
        {
            return;
        }

        if (_uploadOptions.DeniedExtensions.Any(x => string.Equals(x, fileExtension, StringComparison.OrdinalIgnoreCase)))
        {
            ThrowUnsupportedFileTypeException();
        }
    }

    /// <summary>
    /// 规范化业务子路径（统一斜杠、去首尾 /、禁止 .. 穿越）
    /// </summary>
    /// <param name="categoryPath">业务传入的子目录</param>
    /// <returns>安全相对路径段</returns>
    /// <exception cref="ArgumentException">含 .. 等非法段</exception>
    private static string NormalizeCategoryPath(string categoryPath)
    {
        var normalized = categoryPath.Replace('\\', '/').Trim('/');
        if (normalized.Contains("..", StringComparison.Ordinal))
        {
            throw new ArgumentException("CategoryPath 非法");
        }

        return normalized;
    }

    /// <summary>
    /// 构建对外 HTTP 访问 URL（本地为根相对路径；OSS 在 Finalize 阶段覆盖）
    /// </summary>
    /// <param name="relativePath">存储相对路径</param>
    /// <param name="storageType">存储方式</param>
    /// <param name="storageConfig">存储配置 JSON</param>
    /// <returns>访问 URL</returns>
    private static string BuildAccessUrl(string relativePath, int storageType, string? storageConfig)
    {
        if (storageType == TaktFileHelper.StorageTypeFtp)
        {
            return $"/{relativePath.Replace('\\', '/').TrimStart('/')}";
        }

        return $"/{relativePath.Replace('\\', '/').TrimStart('/')}";
    }

    /// <summary>
    /// 构建对外 HTTP 访问 URL（根相对路径，本地默认）
    /// </summary>
    /// <param name="relativePath">存储相对路径</param>
    /// <returns>以 / 开头的 URL 路径</returns>
    private static string BuildAccessUrl(string relativePath) =>
        BuildAccessUrl(relativePath, TaktFileHelper.StorageTypeLocal, null);

    /// <summary>
    /// 获取本地正式文件存储根目录（默认 wwwroot）
    /// </summary>
    /// <returns>存储根目录绝对路径</returns>
    private string GetLocalUploadStorageRootPath() =>
        TaktFileHelper.ResolveLocalUploadStorageRootPath(
            _webHostEnvironment.ContentRootPath,
            _uploadOptions.UploadStorageRootPath);

    /// <summary>
    /// 将相对路径解析为本地存储根目录下绝对路径（Path.GetFullPath 规范化）
    /// </summary>
    /// <param name="relativePath">相对存储根的路径（如 uploads/…）</param>
    /// <returns>本地绝对文件路径</returns>
    private string GetAbsoluteUploadPath(string relativePath)
    {
        var normalized = relativePath.Replace('/', Path.DirectorySeparatorChar);
        return Path.GetFullPath(Path.Combine(GetLocalUploadStorageRootPath(), normalized));
    }

    /// <summary>
    /// 获取分片临时根目录（默认 wwwroot）
    /// </summary>
    /// <returns>分片临时根目录绝对路径</returns>
    private string GetChunkStorageRootPath() =>
        TaktFileHelper.ResolveChunkStorageRootPath(
            _webHostEnvironment.ContentRootPath,
            _uploadOptions.ChunkStorageRootPath);

    /// <summary>
    /// 获取分片临时目录绝对路径
    /// </summary>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <param name="identifier">上传会话标识</param>
    /// <returns>分片目录绝对路径</returns>
    private string GetChunkDirectory(TaktFileUploadScope scope, string identifier)
    {
        return Path.Combine(
            GetChunkStorageRootPath(),
            GetNormalizedChunkRelativePath(),
            scope.TenantCode,
            scope.CompanyCode,
            SanitizeIdentifier(identifier));
    }

    /// <summary>
    /// 获取分片 part 文件的绝对路径
    /// </summary>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <param name="identifier">上传会话标识（通常为文件 MD5）</param>
    /// <param name="chunkNumber">分片序号（从 1 开始）</param>
    /// <returns>{chunkDir}/{chunkNumber}.part 绝对路径</returns>
    private string GetChunkPartPath(TaktFileUploadScope scope, string identifier, int chunkNumber)
    {
        return Path.Combine(GetChunkDirectory(scope, identifier), $"{chunkNumber}.part");
    }

    /// <summary>
    /// 递归删除指定 identifier 的分片临时目录
    /// </summary>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <param name="identifier">上传会话标识</param>
    private void DeleteChunkDirectory(TaktFileUploadScope scope, string identifier)
    {
        var dir = GetChunkDirectory(scope, identifier);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    /// <summary>
    /// 校验上传会话 identifier（非空、长度 ≤128、仅字母数字与 - _）
    /// </summary>
    /// <param name="identifier">客户端生成的文件唯一标识</param>
    /// <exception cref="ArgumentException">为空、过长或含非法字符</exception>
    private static void ValidateIdentifier(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        if (identifier.Length > 128)
        {
            throw new ArgumentException("identifier 过长");
        }

        foreach (var ch in identifier)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '-' && ch != '_')
            {
                throw new ArgumentException("identifier 含非法字符");
            }
        }
    }

    /// <summary>
    /// 规范化 identifier（去除首尾空白）
    /// </summary>
    /// <param name="identifier">原始 identifier</param>
    /// <returns>Trim 后的 identifier</returns>
    private static string SanitizeIdentifier(string identifier) => identifier.Trim();

    /// <summary>
    /// 校验分片元数据与服务器分片计划一致
    /// </summary>
    private void EnsureChunkMetadataMatchesPlan(
        TaktFileChunkPlan plan,
        int totalChunks,
        int chunkNumber,
        long declaredChunkSize,
        long actualChunkSize)
    {
        if (!TaktFileHelper.IsChunkMetadataValid(
                plan,
                totalChunks,
                chunkNumber,
                declaredChunkSize,
                actualChunkSize))
        {
            if (totalChunks != plan.TotalChunks)
            {
                ThrowLocalizedException(FileUploadChunkPlanMismatch);
            }

            ThrowLocalizedException(FileUploadChunkSizeMismatch);
        }
    }

    /// <summary>
    /// 解析分片计划；超过 MaxFileSizeBytes 时抛出本地化异常
    /// </summary>
    /// <param name="totalSizeBytes">文件总大小</param>
    /// <returns>分片计划</returns>
    private TaktFileChunkPlan ResolveChunkPlan(long totalSizeBytes)
    {
        try
        {
            return TaktFileHelper.ResolveChunkPlan(_uploadOptions, totalSizeBytes);
        }
        catch (ArgumentOutOfRangeException)
        {
            ThrowFileSizeExceededException();
            return null!;
        }
        catch (InvalidOperationException)
        {
            ThrowLocalizedException(FileUploadChunkCountExceeded, max: _uploadOptions.MaxChunkCount);
            return null!;
        }
    }

    /// <summary>
    /// 组装全局上传策略（不含按文件大小计算的分片计划）
    /// </summary>
    private TaktFileUploadPolicyResult BuildBaseUploadPolicy()
    {
        return new TaktFileUploadPolicyResult
        {
            MaxFileSizeBytes = _uploadOptions.MaxFileSizeBytes,
            MaxChunkCount = _uploadOptions.MaxChunkCount,
            DefaultChunkSizeBytes = _uploadOptions.DefaultChunkSizeBytes,
            ChunkThresholdBytes = _uploadOptions.ChunkThresholdBytes,
            ChunkRelativePath = GetNormalizedChunkRelativePath().Replace('\\', '/'),
            AllowedExtensions = _uploadOptions.AllowedExtensions.ToArray(),
            DeniedExtensions = _uploadOptions.DeniedExtensions.ToArray(),
        };
    }

    /// <summary>
    /// 规范化分片临时目录相对路径（禁止 .. 穿越）
    /// </summary>
    private string GetNormalizedChunkRelativePath()
    {
        var normalized = _uploadOptions.ChunkRelativePath.Replace('\\', '/').Trim('/');
        if (string.IsNullOrWhiteSpace(normalized) || normalized.Contains("..", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("ChunkRelativePath 配置非法");
        }

        return normalized.Replace('/', Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// 校验文件名有效性与扩展名白名单（TaktFileUploadOptions.AllowedExtensions）
    /// </summary>
    /// <param name="fileName">原始或客户端文件名</param>
    private void ValidateFileName(string fileName)
    {
        var safeName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeName))
        {
            ThrowLocalizedException(FileUploadNameInvalid);
        }

        var ext = GetNormalizedExtension(safeName);
        ValidateDeniedExtension(ext);
        if (_uploadOptions.AllowedExtensions.Length == 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ext)
            || !_uploadOptions.AllowedExtensions.Any(x => string.Equals(x, ext, StringComparison.OrdinalIgnoreCase)))
        {
            ThrowUnsupportedFileTypeException();
        }
    }

    /// <summary>
    /// 抛出文件大小超限友好提示（validation.file.upload.size.exceeded）
    /// </summary>
    private void ThrowFileSizeExceededException()
    {
        var maxMb = Math.Max(1, _uploadOptions.MaxFileSizeBytes / 1024 / 1024);
        ThrowLocalizedException(FileUploadSizeExceeded, max: (int)maxMb);
    }

    /// <summary>
    /// 抛出不支持文件类型友好提示（validation.file.upload.type.unsupported）
    /// </summary>
    private void ThrowUnsupportedFileTypeException()
    {
        ThrowLocalizedException(FileUploadTypeUnsupported);
    }

    /// <summary>
    /// 校验字节数不超过 TaktFileUploadOptions.MaxFileSizeBytes
    /// </summary>
    /// <param name="sizeBytes">待校验大小</param>
    private void EnsureFileSizeWithinLimit(long sizeBytes)
    {
        if (sizeBytes < 0 || sizeBytes > _uploadOptions.MaxFileSizeBytes)
        {
            ThrowFileSizeExceededException();
        }
    }

    /// <summary>
    /// 本地落盘完成后按 StorageType 推送至 OSS/FTP，并删除本地副本
    /// </summary>
    /// <param name="localResult">本地存储结果</param>
    /// <param name="scope">隔离与存储配置</param>
    /// <param name="absoluteLocalPath">本地绝对路径</param>
    /// <param name="contentType">MIME</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>最终存储结果</returns>
    private async Task<TaktStoredFileResult> FinalizeStoredFileAsync(
        TaktStoredFileResult localResult,
        TaktFileUploadScope scope,
        string absoluteLocalPath,
        string? contentType,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(localResult);
        if (scope.StorageType == TaktFileHelper.StorageTypeLocal)
        {
            return localResult;
        }

        var objectKey = TaktFileHelper.NormalizeRemoteObjectKey(localResult.FilePath);
        try
        {
            if (scope.StorageType == TaktFileHelper.StorageTypeOss)
            {
                var provider = TaktFileHelper.ResolveOssProvider(scope.StorageConfig);
                if (!TaktOssHelper.IsSupportedProvider(provider))
                {
                    ThrowLocalizedException(FileStorageProviderUnsupported);
                }

                var ossOptions = TaktOssHelper.GetOssOptionsFromConfiguration(_configuration, provider);
                await TaktOssHelper.UploadLocalFileAsync(
                    _configuration,
                    provider,
                    absoluteLocalPath,
                    objectKey,
                    contentType,
                    cancellationToken);
                DeleteFile(absoluteLocalPath, throwIfNotExists: false);
                localResult.StorageType = TaktFileHelper.StorageTypeOss;
                localResult.StorageConfig = scope.StorageConfig;
                localResult.AccessUrl = TaktOssHelper.BuildPublicObjectUrl(ossOptions, objectKey);
                return localResult;
            }

            if (scope.StorageType == TaktFileHelper.StorageTypeFtp)
            {
                await PublishLocalFileToFtpAsync(scope.StorageConfig, absoluteLocalPath, objectKey, cancellationToken);
                DeleteFile(absoluteLocalPath, throwIfNotExists: false);
                localResult.StorageType = TaktFileHelper.StorageTypeFtp;
                localResult.StorageConfig = scope.StorageConfig;
                localResult.AccessUrl = BuildAccessUrl(objectKey, TaktFileHelper.StorageTypeFtp, scope.StorageConfig);
                return localResult;
            }

            return localResult;
        }
        catch (Exception ex) when (ex is not TaktLocalizedException and not TaktBusinessException)
        {
            TaktLogger.Error(ex, "[TaktFileUploadEngine] 远程存储上传失败: StorageType={StorageType}, Path={Path}", scope.StorageType, objectKey);
            ThrowLocalizedException(FileStorageUploadFailed);
            return localResult;
        }
    }

    /// <summary>
    /// 从 OSS 打开只读流
    /// </summary>
    private async Task<TaktFileDownloadStreamResult> OpenReadFromOssAsync(
        TaktFileStorageDescriptor descriptor,
        string downloadFileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        var provider = TaktFileHelper.ResolveOssProvider(descriptor.StorageConfig);
        if (!TaktOssHelper.IsSupportedProvider(provider))
        {
            ThrowLocalizedException(FileStorageProviderUnsupported);
        }

        var ossOptions = TaktOssHelper.GetOssOptionsFromConfiguration(_configuration, provider);
        var objectKey = TaktFileHelper.NormalizeRemoteObjectKey(descriptor.FilePath);
        if (!await TaktOssHelper.ObjectExistsAsync(ossOptions, objectKey, cancellationToken))
        {
            ThrowLocalizedException(FilePhysicalNotFound);
        }

        var stream = await TaktOssHelper.GetObjectStreamAsync(ossOptions, objectKey, cancellationToken);
        return new TaktFileDownloadStreamResult
        {
            Stream = stream,
            FileName = downloadFileName,
            ContentType = contentType,
        };
    }

    /// <summary>
    /// 从 FTP 打开只读流（FluentFTP DownloadStream → MemoryStream）
    /// </summary>
    private async Task<TaktFileDownloadStreamResult> OpenReadFromFtpAsync(
        TaktFileStorageDescriptor descriptor,
        string downloadFileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        var ftpOptions = ResolveFtpOptions(descriptor.StorageConfig);
        var remotePath = TaktFileHelper.NormalizeRemoteObjectKey(descriptor.FilePath);
        if (!await TaktFtpHelper.FileExistsAsync(ftpOptions, remotePath))
        {
            ThrowLocalizedException(FilePhysicalNotFound);
        }

        var memoryStream = new MemoryStream();
        await TaktFtpHelper.DownloadStreamAsync(ftpOptions, remotePath, memoryStream);
        memoryStream.Position = 0;
        cancellationToken.ThrowIfCancellationRequested();
        return new TaktFileDownloadStreamResult
        {
            Stream = memoryStream,
            FileName = downloadFileName,
            ContentType = contentType,
        };
    }

    /// <summary>
    /// 解析 FTP 配置（StorageConfig.ftpProvider → appsettings Ftp 节）
    /// </summary>
    /// <param name="storageConfig">StorageConfig JSON</param>
    /// <returns>FTP 连接配置</returns>
    private TaktFtpOptions ResolveFtpOptions(string? storageConfig)
    {
        var provider = TaktFileHelper.ResolveFtpProvider(storageConfig);
        return TaktFtpHelper.GetFtpOptionsFromConfiguration(_configuration, provider);
    }

    /// <summary>
    /// 将本地暂存文件经 TaktFtpHelper（FluentFTP）推送到 FTP
    /// </summary>
    /// <param name="storageConfig">StorageConfig JSON</param>
    /// <param name="absoluteLocalPath">本地暂存绝对路径</param>
    /// <param name="remoteFilePath">远程相对路径</param>
    /// <param name="cancellationToken">取消令牌</param>
    private async Task PublishLocalFileToFtpAsync(
        string? storageConfig,
        string absoluteLocalPath,
        string remoteFilePath,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(absoluteLocalPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);
        cancellationToken.ThrowIfCancellationRequested();
        var ftpOptions = ResolveFtpOptions(storageConfig);
        await TaktFtpHelper.UploadLocalFileViaFluentFtpAsync(ftpOptions, absoluteLocalPath, remoteFilePath);
    }

    /// <summary>
    /// 获取写入绝对路径：本地存储写 wwwroot；OSS/FTP 写 _staging 暂存区（合并/推送后删除）
    /// </summary>
    /// <param name="scope">隔离与存储类型</param>
    /// <param name="relativePath">业务相对路径（与落库 FilePath 一致）</param>
    /// <returns>本地绝对路径</returns>
    private string GetLocalWriteAbsolutePath(TaktFileUploadScope scope, string relativePath)
    {
        if (scope.StorageType == TaktFileHelper.StorageTypeLocal)
        {
            return GetAbsoluteUploadPath(relativePath);
        }

        var stagingRoot = Path.Combine(
            GetChunkStorageRootPath(),
            GetNormalizedChunkRelativePath(),
            "_staging");
        var normalized = relativePath.Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.GetFullPath(Path.Combine(stagingRoot, normalized));
        var fullStagingRoot = Path.GetFullPath(stagingRoot);
        if (!absolutePath.StartsWith(fullStagingRoot, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("暂存路径非法");
        }

        return absolutePath;
    }

    /// <summary>
    /// 删除 OSS 对象
    /// </summary>
    private async Task DeleteOssObjectAsync(TaktFileStorageDescriptor descriptor, CancellationToken cancellationToken)
    {
        var provider = TaktFileHelper.ResolveOssProvider(descriptor.StorageConfig);
        if (!TaktOssHelper.IsSupportedProvider(provider))
        {
            ThrowLocalizedException(FileStorageProviderUnsupported);
        }

        var ossOptions = TaktOssHelper.GetOssOptionsFromConfiguration(_configuration, provider);
        var objectKey = TaktFileHelper.NormalizeRemoteObjectKey(descriptor.FilePath);
        await TaktOssHelper.DeleteObjectAsync(ossOptions, objectKey, cancellationToken);
    }

    /// <summary>
    /// 删除 FTP 远程文件
    /// </summary>
    private async Task DeleteFtpObjectAsync(TaktFileStorageDescriptor descriptor, CancellationToken cancellationToken)
    {
        var ftpOptions = ResolveFtpOptions(descriptor.StorageConfig);
        var remotePath = TaktFileHelper.NormalizeRemoteObjectKey(descriptor.FilePath);
        cancellationToken.ThrowIfCancellationRequested();
        await TaktFtpHelper.DeleteFileAsync(ftpOptions, remotePath);
    }

    /// <summary>
    /// OSS 对象标记删除（复制至 .del 键后删除源对象）
    /// </summary>
    private async Task<string> MarkOssStoredFileDeletedAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken)
    {
        var relativePath = descriptor.FilePath.Replace('\\', '/').TrimStart('/');
        var fileName = Path.GetFileName(relativePath);
        if (IsDeletedPhysicalFileName(fileName))
        {
            return relativePath;
        }

        var provider = TaktFileHelper.ResolveOssProvider(descriptor.StorageConfig);
        if (!TaktOssHelper.IsSupportedProvider(provider))
        {
            ThrowLocalizedException(FileStorageProviderUnsupported);
        }

        var ossOptions = TaktOssHelper.GetOssOptionsFromConfiguration(_configuration, provider);
        var sourceKey = TaktFileHelper.NormalizeRemoteObjectKey(relativePath);
        if (!await TaktOssHelper.ObjectExistsAsync(ossOptions, sourceKey, cancellationToken))
        {
            return BuildDeletedPhysicalRelativePath(relativePath);
        }

        var deletedRelativePath = BuildDeletedPhysicalRelativePath(relativePath);
        var deletedKey = TaktFileHelper.NormalizeRemoteObjectKey(deletedRelativePath);
        var attempt = 0;
        while (await TaktOssHelper.ObjectExistsAsync(ossOptions, deletedKey, cancellationToken) && attempt < 100)
        {
            attempt = checked(attempt + 1);
            var extension = Path.GetExtension(fileName);
            var baseName = Path.GetFileNameWithoutExtension(fileName);
            var suffixedFileName = string.IsNullOrEmpty(extension)
                ? $"{baseName}{DeletedPhysicalFileMarker}.{attempt}"
                : $"{baseName}{DeletedPhysicalFileMarker}.{attempt}{extension}";
            var directory = Path.GetDirectoryName(relativePath.Replace('/', Path.DirectorySeparatorChar));
            deletedRelativePath = string.IsNullOrWhiteSpace(directory)
                ? suffixedFileName
                : $"{directory.Replace('\\', '/')}/{suffixedFileName}";
            deletedKey = TaktFileHelper.NormalizeRemoteObjectKey(deletedRelativePath);
        }

        if (await TaktOssHelper.ObjectExistsAsync(ossOptions, deletedKey, cancellationToken))
        {
            ThrowLocalizedException(FilePhysicalDeleteTargetExists);
        }

        await TaktOssHelper.CopyObjectAsync(ossOptions, sourceKey, deletedKey, cancellationToken);
        await TaktOssHelper.DeleteObjectAsync(ossOptions, sourceKey, cancellationToken);
        return deletedRelativePath;
    }

    /// <summary>
    /// FTP 远程文件标记删除（重命名为 .del 后缀）
    /// </summary>
    private async Task<string> MarkFtpStoredFileDeletedAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken)
    {
        var relativePath = descriptor.FilePath.Replace('\\', '/').TrimStart('/');
        var fileName = Path.GetFileName(relativePath);
        if (IsDeletedPhysicalFileName(fileName))
        {
            return relativePath;
        }

        var ftpOptions = ResolveFtpOptions(descriptor.StorageConfig);
        var sourcePath = TaktFileHelper.NormalizeRemoteObjectKey(relativePath);
        cancellationToken.ThrowIfCancellationRequested();
        if (!await TaktFtpHelper.FileExistsAsync(ftpOptions, sourcePath))
        {
            return BuildDeletedPhysicalRelativePath(relativePath);
        }

        var deletedRelativePath = BuildDeletedPhysicalRelativePath(relativePath);
        var deletedPath = TaktFileHelper.NormalizeRemoteObjectKey(deletedRelativePath);
        var attempt = 0;
        while (await TaktFtpHelper.FileExistsAsync(ftpOptions, deletedPath) && attempt < 100)
        {
            attempt = checked(attempt + 1);
            var extension = Path.GetExtension(fileName);
            var baseName = Path.GetFileNameWithoutExtension(fileName);
            var suffixedFileName = string.IsNullOrEmpty(extension)
                ? $"{baseName}{DeletedPhysicalFileMarker}.{attempt}"
                : $"{baseName}{DeletedPhysicalFileMarker}.{attempt}{extension}";
            var directory = Path.GetDirectoryName(relativePath.Replace('/', Path.DirectorySeparatorChar));
            deletedRelativePath = string.IsNullOrWhiteSpace(directory)
                ? suffixedFileName
                : $"{directory.Replace('\\', '/')}/{suffixedFileName}";
            deletedPath = TaktFileHelper.NormalizeRemoteObjectKey(deletedRelativePath);
        }

        if (await TaktFtpHelper.FileExistsAsync(ftpOptions, deletedPath))
        {
            ThrowLocalizedException(FilePhysicalDeleteTargetExists);
        }

        await TaktFtpHelper.RenameAsync(ftpOptions, sourcePath, deletedPath);
        return deletedRelativePath;
    }
}
