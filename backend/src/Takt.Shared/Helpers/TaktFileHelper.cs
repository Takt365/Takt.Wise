// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFileHelper.cs
// 创建时间：2025-01-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 文件帮助类（I/O、分类、状态、访问判定、分片计划、存储配置解析）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Cryptography;
using System.Text;
using MimeKit;
using Newtonsoft.Json;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// Takt 文件帮助类（I/O、状态、访问、分片计划、存储配置）
/// </summary>
/// <remarks>含文件系统 I/O 网关；读写/删除方法名与 XML 均明示副作用。</remarks>
public static class TaktFileHelper
{
    /// <summary>
    /// 文件分类枚举
    /// </summary>
    public enum FileCategory
    {
        /// <summary>
        /// 文档
        /// </summary>
        Document = 0,

        /// <summary>
        /// 图片
        /// </summary>
        Image = 1,

        /// <summary>
        /// 视频
        /// </summary>
        Video = 2,

        /// <summary>
        /// 音频
        /// </summary>
        Audio = 3,

        /// <summary>
        /// 压缩包
        /// </summary>
        Archive = 4,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 5
    }

    #region 文件状态

    /// <summary>
    /// 禁用（字典 sys_normal_disable=0）
    /// </summary>
    public const int FileStatusDisabled = 0;

    /// <summary>
    /// 启用（字典 sys_normal_disable=1）
    /// </summary>
    public const int FileStatusEnabled = 1;

    /// <summary>
    /// 锁定（字典 sys_normal_disable=2）
    /// </summary>
    public const int FileStatusLocked = 2;

    /// <summary>
    /// 文件是否为启用态（仅 1 可下载）
    /// </summary>
    /// <param name="fileStatus">文件状态</param>
    /// <returns>启用返回 true</returns>
    public static bool IsFileStatusEnabled(int fileStatus) => fileStatus == FileStatusEnabled;

    /// <summary>
    /// 是否为合法文件状态字典值（0/1/2）
    /// </summary>
    /// <param name="fileStatus">文件状态</param>
    /// <returns>合法返回 true</returns>
    public static bool IsValidFileStatus(int fileStatus) =>
        fileStatus is FileStatusDisabled or FileStatusEnabled or FileStatusLocked;

    /// <summary>
    /// 解析上传/表单传入的文件状态；非法或未传时默认启用
    /// </summary>
    /// <param name="fileStatus">可选状态</param>
    /// <returns>有效状态值</returns>
    public static int NormalizeFileStatusOrDefault(int? fileStatus)
    {
        if (!fileStatus.HasValue || !IsValidFileStatus(fileStatus.Value))
        {
            return FileStatusEnabled;
        }

        return fileStatus.Value;
    }

    #endregion

    #region 文件访问

    /// <summary>
    /// 当前用户是否可访问该文件（IsPublic 字典 sys_public_type：0=公开，1=私有；不含 RBAC）
    /// </summary>
    /// <param name="isPublic">公开（0=公开，1=私有）</param>
    /// <param name="createdBy">文件创建人用户 ID</param>
    /// <param name="currentUserId">当前登录用户 ID</param>
    /// <returns>公开文件为 true；私有文件仅创建人为 true</returns>
    public static bool CanAccessFile(int isPublic, long createdBy, long? currentUserId)
    {
        if (isPublic == 0)
        {
            return true;
        }

        return currentUserId is > 0 && createdBy == currentUserId.Value;
    }

    #endregion

    #region 存储配置

    /// <summary>
    /// 本地存储（字典 sys_storage_type=0）
    /// </summary>
    public const int StorageTypeLocal = 0;

    /// <summary>
    /// OSS 对象存储（字典 sys_storage_type=1）
    /// </summary>
    public const int StorageTypeOss = 1;

    /// <summary>
    /// FTP 存储（字典 sys_storage_type=2）
    /// </summary>
    public const int StorageTypeFtp = 2;

    /// <summary>
    /// 默认 OSS 提供商标识（字典 sys_oss_provider=aliyun）
    /// </summary>
    public const string DefaultOssProvider = "aliyun";

    /// <summary>
    /// 默认 FTP 提供商标识（字典 sys_ftp_provider=teac_cn）
    /// </summary>
    public const string DefaultFtpProvider = "teac_cn";

    /// <summary>
    /// 从 StorageConfig JSON 解析存储配置载荷
    /// </summary>
    /// <param name="jsonConfig">StorageConfig 原始 JSON</param>
    /// <returns>解析结果；非法 JSON 返回空载荷</returns>
    public static TaktFileStorageConfigPayload ParseStorageConfig(string? jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig))
        {
            return new TaktFileStorageConfigPayload();
        }

        try
        {
            var parsed = JsonConvert.DeserializeObject<TaktFileStorageConfigPayload>(jsonConfig);
            return parsed ?? new TaktFileStorageConfigPayload();
        }
        catch (JsonException ex)
        {
            TaktLogger.Warning(ex, "[TaktFileHelper] StorageConfig JSON 解析失败");
            return new TaktFileStorageConfigPayload();
        }
    }

    /// <summary>
    /// 解析 OSS 提供商标识
    /// </summary>
    /// <param name="jsonConfig">StorageConfig JSON</param>
    /// <returns>提供商标识（小写）</returns>
    public static string ResolveOssProvider(string? jsonConfig)
    {
        var payload = ParseStorageConfig(jsonConfig);
        var provider = string.IsNullOrWhiteSpace(payload.OssProvider)
            ? DefaultOssProvider
            : payload.OssProvider.Trim();
        return provider.ToLowerInvariant();
    }

    /// <summary>
    /// 解析 FTP 提供商标识
    /// </summary>
    /// <param name="jsonConfig">StorageConfig JSON</param>
    /// <returns>提供商标识（小写）</returns>
    public static string ResolveFtpProvider(string? jsonConfig)
    {
        var payload = ParseStorageConfig(jsonConfig);
        var provider = string.IsNullOrWhiteSpace(payload.FtpProvider)
            ? DefaultFtpProvider
            : payload.FtpProvider.Trim();
        return provider.ToLowerInvariant();
    }

    /// <summary>
    /// 规范化远程对象/文件键（统一斜杠、去首尾 /、禁止 ..）
    /// </summary>
    /// <param name="relativePath">相对存储路径</param>
    /// <returns>安全远程键</returns>
    /// <exception cref="ArgumentException">路径非法</exception>
    public static string NormalizeRemoteObjectKey(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        var normalized = relativePath.Replace('\\', '/').TrimStart('/');
        if (normalized.Contains("..", StringComparison.Ordinal))
        {
            throw new ArgumentException("远程存储路径非法");
        }

        return normalized;
    }

    #endregion

    #region 分片上传计划

    /// <summary>
    /// 根据文件总大小与上传配置生成分片计划
    /// </summary>
    /// <param name="options">FileUpload 配置</param>
    /// <param name="totalSizeBytes">文件总大小（字节）</param>
    /// <returns>分片计划</returns>
    /// <exception cref="ArgumentNullException">options 为 null</exception>
    /// <exception cref="ArgumentOutOfRangeException">totalSizeBytes 非法或超过 MaxFileSizeBytes</exception>
    public static TaktFileChunkPlan ResolveChunkPlan(TaktFileUploadOptions options, long totalSizeBytes)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (totalSizeBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSizeBytes), "文件总大小必须大于 0");
        }

        if (totalSizeBytes > options.MaxFileSizeBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSizeBytes), "文件总大小超过 MaxFileSizeBytes");
        }

        var maxChunkCount = Math.Max(1, options.MaxChunkCount);
        var defaultChunkSize = Math.Max(1L, options.DefaultChunkSizeBytes);
        var threshold = Math.Max(1L, options.ChunkThresholdBytes);

        if (totalSizeBytes <= threshold)
        {
            return BuildChunkPlan(
                totalSizeBytes,
                useChunkUpload: false,
                chunkSizeBytes: totalSizeBytes,
                totalChunks: 1,
                options,
                maxChunkCount,
                defaultChunkSize,
                threshold);
        }

        var chunkSize = defaultChunkSize;
        var totalChunks = checked((int)Math.Ceiling((double)totalSizeBytes / chunkSize));
        if (totalChunks > maxChunkCount)
        {
            chunkSize = (long)Math.Ceiling((double)totalSizeBytes / maxChunkCount);
            if (chunkSize < 1)
            {
                chunkSize = 1;
            }

            totalChunks = checked((int)Math.Ceiling((double)totalSizeBytes / chunkSize));
        }

        if (totalChunks > maxChunkCount)
        {
            throw new InvalidOperationException("无法在 MaxChunkCount 限制内切分分片");
        }

        return BuildChunkPlan(
            totalSizeBytes,
            useChunkUpload: true,
            chunkSizeBytes: chunkSize,
            totalChunks: totalChunks,
            options,
            maxChunkCount,
            defaultChunkSize,
            threshold);
    }

    /// <summary>
    /// 获取指定序号分片的期望大小（最后一片为余数）
    /// </summary>
    /// <param name="plan">分片计划</param>
    /// <param name="chunkNumber">分片序号（从 1 开始）</param>
    /// <returns>期望字节数</returns>
    /// <exception cref="ArgumentNullException">plan 为 null</exception>
    /// <exception cref="ArgumentOutOfRangeException">chunkNumber 超出范围</exception>
    public static long GetExpectedChunkSize(TaktFileChunkPlan plan, int chunkNumber)
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (chunkNumber < 1 || chunkNumber > plan.TotalChunks)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkNumber));
        }

        if (chunkNumber < plan.TotalChunks)
        {
            return plan.ChunkSizeBytes;
        }

        var remainder = checked(plan.TotalSizeBytes - ((long)(plan.TotalChunks - 1) * plan.ChunkSizeBytes));
        return remainder > 0 ? remainder : plan.ChunkSizeBytes;
    }

    /// <summary>
    /// 校验客户端声明的分片元数据是否与计划一致
    /// </summary>
    /// <param name="plan">分片计划</param>
    /// <param name="totalChunks">客户端声明总分片数</param>
    /// <param name="chunkNumber">分片序号</param>
    /// <param name="declaredChunkSize">客户端声明分片大小</param>
    /// <param name="actualChunkSize">实际上传字节数</param>
    /// <returns>元数据是否与计划一致</returns>
    public static bool IsChunkMetadataValid(
        TaktFileChunkPlan plan,
        int totalChunks,
        int chunkNumber,
        long declaredChunkSize,
        long actualChunkSize)
    {
        ArgumentNullException.ThrowIfNull(plan);
        if (totalChunks != plan.TotalChunks)
        {
            return false;
        }

        if (chunkNumber < 1 || chunkNumber > plan.TotalChunks)
        {
            return false;
        }

        var expected = GetExpectedChunkSize(plan, chunkNumber);
        return declaredChunkSize == expected && actualChunkSize == expected;
    }

    private static TaktFileChunkPlan BuildChunkPlan(
        long totalSizeBytes,
        bool useChunkUpload,
        long chunkSizeBytes,
        int totalChunks,
        TaktFileUploadOptions options,
        int maxChunkCount,
        long defaultChunkSize,
        long threshold)
    {
        return new TaktFileChunkPlan
        {
            TotalSizeBytes = totalSizeBytes,
            UseChunkUpload = useChunkUpload,
            ChunkSizeBytes = chunkSizeBytes,
            TotalChunks = totalChunks,
            MaxFileSizeBytes = options.MaxFileSizeBytes,
            MaxChunkCount = maxChunkCount,
            ChunkThresholdBytes = threshold,
            DefaultChunkSizeBytes = defaultChunkSize,
        };
    }

    #endregion

    #region 文件读取

    /// <summary>
    /// 读取文件内容为字节数组
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>文件字节数组</returns>
    public static async Task<byte[]> ReadFileAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            TaktLogger.Warning("[TaktFileHelper] 读取文件失败，文件不存在: {FilePath}", filePath);
            throw new FileNotFoundException($"文件不存在: {filePath}");
        }

        try
        {
            var data = await File.ReadAllBytesAsync(filePath);
            TaktLogger.Information("[TaktFileHelper] 读取文件成功: {FilePath}, 大小: {Size} 字节", filePath, data.Length);
            return data;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 读取文件失败: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// 读取文件内容为字符串
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="encoding">编码，默认为UTF-8</param>
    /// <returns>文件内容字符串</returns>
    public static async Task<string> ReadFileTextAsync(string filePath, Encoding? encoding = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            TaktLogger.Warning("[TaktFileHelper] 读取文件文本失败，文件不存在: {FilePath}", filePath);
            throw new FileNotFoundException($"文件不存在: {filePath}");
        }

        encoding ??= Encoding.UTF8;
        try
        {
            var content = await File.ReadAllTextAsync(filePath, encoding);
            TaktLogger.Information("[TaktFileHelper] 读取文件文本成功: {FilePath}, 编码: {Encoding}", filePath, encoding.EncodingName);
            return content;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 读取文件文本失败: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// 读取文件为流
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>文件流</returns>
    public static FileStream ReadFileStream(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"文件不存在: {filePath}");
        }

        return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>是否存在</returns>
    public static bool FileExists(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        return File.Exists(filePath);
    }

    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>文件信息</returns>
    public static FileInfo? GetFileInfo(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;

        return new FileInfo(filePath);
    }

    #endregion

    #region 文件写入/创建

    /// <summary>
    /// 写入文件（从字节数组）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="data">文件数据</param>
    /// <param name="createDirectory">是否自动创建目录，默认为true</param>
    /// <returns>任务</returns>
    public static async Task WriteFileAsync(string filePath, byte[] data, bool createDirectory = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(data);

        if (createDirectory)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        await File.WriteAllBytesAsync(filePath, data);
    }

    /// <summary>
    /// 写入文件（从字符串）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="content">文件内容</param>
    /// <param name="encoding">编码，默认为UTF-8</param>
    /// <param name="createDirectory">是否自动创建目录，默认为true</param>
    /// <returns>任务</returns>
    public static async Task WriteFileTextAsync(string filePath, string content, Encoding? encoding = null, bool createDirectory = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(content);

        try
        {
            if (createDirectory)
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            encoding ??= Encoding.UTF8;
            await File.WriteAllTextAsync(filePath, content, encoding);
            TaktLogger.Information("[TaktFileHelper] 写入文件文本成功: {FilePath}, 编码: {Encoding}, 长度: {Length} 字符", filePath, encoding.EncodingName, content.Length);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 写入文件文本失败: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// 写入文件（从流）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="stream">文件流</param>
    /// <param name="createDirectory">是否自动创建目录，默认为true</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public static async Task WriteFileFromStreamAsync(string filePath, Stream stream, bool createDirectory = true, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(stream);

        if (createDirectory)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }

    /// <summary>
    /// 追加内容到文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="content">要追加的内容</param>
    /// <param name="encoding">编码，默认为UTF-8</param>
    /// <param name="createDirectory">是否自动创建目录，默认为true</param>
    /// <returns>任务</returns>
    public static async Task AppendFileTextAsync(string filePath, string content, Encoding? encoding = null, bool createDirectory = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(content);

        try
        {
            if (createDirectory)
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            encoding ??= Encoding.UTF8;
            await File.AppendAllTextAsync(filePath, content, encoding);
            TaktLogger.Information("[TaktFileHelper] 追加文件文本成功: {FilePath}, 追加长度: {Length} 字符", filePath, content.Length);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 追加文件文本失败: {FilePath}", filePath);
            throw;
        }
    }

    #endregion

    #region 文件更新/替换

    /// <summary>
    /// 替换文件（先删除旧文件，再写入新文件）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="newData">新文件数据</param>
    /// <param name="backup">是否备份原文件，默认为false</param>
    /// <returns>任务</returns>
    public static async Task ReplaceFileAsync(string filePath, byte[] newData, bool backup = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(newData);

        if (File.Exists(filePath) && backup)
        {
            var backupPath = $"{filePath}.backup.{DateTime.Now:yyyyMMddHHmmss}";
            await CopyFileAsync(filePath, backupPath, true);
        }

        await WriteFileAsync(filePath, newData);
    }

    /// <summary>
    /// 替换文件（从流）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="newStream">新文件流</param>
    /// <param name="backup">是否备份原文件，默认为false</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public static async Task ReplaceFileFromStreamAsync(string filePath, Stream newStream, bool backup = false, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(newStream);

        try
        {
            if (File.Exists(filePath) && backup)
            {
                var backupPath = $"{filePath}.backup.{DateTime.Now:yyyyMMddHHmmss}";
                await CopyFileAsync(filePath, backupPath, true);
                TaktLogger.Information("[TaktFileHelper] 文件备份成功: {FilePath} -> {BackupPath}", filePath, backupPath);
            }

            await WriteFileFromStreamAsync(filePath, newStream, cancellationToken: cancellationToken);
            TaktLogger.Information("[TaktFileHelper] 从流替换文件成功: {FilePath}, 备份: {Backup}", filePath, backup);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 从流替换文件失败: {FilePath}", filePath);
            throw;
        }
    }

    #endregion

    #region 文件删除

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="throwIfNotExists">如果文件不存在是否抛出异常，默认为false</param>
    /// <returns>是否删除成功</returns>
    public static bool DeleteFile(string filePath, bool throwIfNotExists = false)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            if (throwIfNotExists)
                throw new ArgumentException("文件路径不能为空", nameof(filePath));
            return false;
        }

        if (!File.Exists(filePath))
        {
            if (throwIfNotExists)
            {
                TaktLogger.Warning("[TaktFileHelper] 删除文件失败，文件不存在: {FilePath}", filePath);
                throw new FileNotFoundException($"文件不存在: {filePath}");
            }
            return false;
        }

        try
        {
            File.Delete(filePath);
            TaktLogger.Information("[TaktFileHelper] 删除文件成功: {FilePath}", filePath);
            return true;
        }
        catch (Exception ex)
        {
            if (throwIfNotExists)
            {
                TaktLogger.Error(ex, "[TaktFileHelper] 删除文件失败: {FilePath}", filePath);
                throw;
            }
            TaktLogger.Warning(ex, "[TaktFileHelper] 删除文件失败: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// 删除文件（异步）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="throwIfNotExists">如果文件不存在是否抛出异常，默认为false</param>
    /// <returns>是否删除成功</returns>
    public static async Task<bool> DeleteFileAsync(string filePath, bool throwIfNotExists = false)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            if (throwIfNotExists)
                throw new ArgumentException("文件路径不能为空", nameof(filePath));
            return false;
        }

        if (!File.Exists(filePath))
        {
            if (throwIfNotExists)
            {
                TaktLogger.Warning("[TaktFileHelper] 删除文件失败，文件不存在: {FilePath}", filePath);
                throw new FileNotFoundException($"文件不存在: {filePath}");
            }
            return false;
        }

        try
        {
            await Task.Run(() => File.Delete(filePath));
            TaktLogger.Information("[TaktFileHelper] 删除文件成功: {FilePath}", filePath);
            return true;
        }
        catch (Exception ex)
        {
            if (throwIfNotExists)
            {
                TaktLogger.Error(ex, "[TaktFileHelper] 删除文件失败: {FilePath}", filePath);
                throw;
            }
            TaktLogger.Warning(ex, "[TaktFileHelper] 删除文件失败: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// 批量删除文件
    /// </summary>
    /// <param name="filePaths">文件路径列表</param>
    /// <param name="throwOnError">遇到错误是否抛出异常，默认为false</param>
    /// <returns>成功删除的文件数量</returns>
    public static int DeleteFiles(IEnumerable<string> filePaths, bool throwOnError = false)
    {
        ArgumentNullException.ThrowIfNull(filePaths);

        var successCount = 0;
        var totalCount = 0;
        foreach (var filePath in filePaths)
        {
            totalCount++;
            try
            {
                if (DeleteFile(filePath, false))
                {
                    successCount++;
                }
            }
            catch (Exception ex)
            {
                if (throwOnError)
                {
                    TaktLogger.Error(ex, "[TaktFileHelper] 批量删除文件失败: {FilePath}", filePath);
                    throw;
                }
                TaktLogger.Warning(ex, "[TaktFileHelper] 批量删除文件失败: {FilePath}", filePath);
            }
        }

        TaktLogger.Information("[TaktFileHelper] 批量删除文件完成: 总数: {TotalCount}, 成功: {SuccessCount}", totalCount, successCount);
        return successCount;
    }

    /// <summary>
    /// 删除目录及其所有子文件和子目录（递归删除）
    /// </summary>
    /// <param name="directoryPath">目录路径</param>
    /// <param name="throwIfNotExists">如果目录不存在是否抛出异常，默认为false</param>
    /// <returns>是否删除成功</returns>
    public static bool DeleteDirectory(string directoryPath, bool throwIfNotExists = false)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            if (throwIfNotExists)
                throw new ArgumentException("目录路径不能为空", nameof(directoryPath));
            return false;
        }

        if (!Directory.Exists(directoryPath))
        {
            if (throwIfNotExists)
            {
                TaktLogger.Warning("[TaktFileHelper] 删除目录失败，目录不存在: {DirectoryPath}", directoryPath);
                throw new DirectoryNotFoundException($"目录不存在: {directoryPath}");
            }
            return false;
        }

        try
        {
            Directory.Delete(directoryPath, true);
            TaktLogger.Information("[TaktFileHelper] 删除目录成功: {DirectoryPath}", directoryPath);
            return true;
        }
        catch (Exception ex)
        {
            if (throwIfNotExists)
            {
                TaktLogger.Error(ex, "[TaktFileHelper] 删除目录失败: {DirectoryPath}", directoryPath);
                throw;
            }
            TaktLogger.Warning(ex, "[TaktFileHelper] 删除目录失败: {DirectoryPath}", directoryPath);
            return false;
        }
    }

    /// <summary>
    /// 删除目录及其所有子文件和子目录（异步）
    /// </summary>
    /// <param name="directoryPath">目录路径</param>
    /// <param name="throwIfNotExists">如果目录不存在是否抛出异常，默认为false</param>
    /// <returns>是否删除成功</returns>
    public static Task<bool> DeleteDirectoryAsync(string directoryPath, bool throwIfNotExists = false)
    {
        return Task.FromResult(DeleteDirectory(directoryPath, throwIfNotExists));
    }

    /// <summary>
    /// 按名称模式删除文件或目录（支持递归搜索）
    /// </summary>
    /// <param name="searchPath">搜索路径（目录路径）</param>
    /// <param name="namePattern">名称模式（包含的字符串，区分大小写）</param>
    /// <param name="recursive">是否递归搜索子目录，默认为true</param>
    /// <param name="includeDirectories">是否删除匹配的目录，默认为true</param>
    /// <param name="includeFiles">是否删除匹配的文件，默认为true</param>
    /// <param name="throwOnError">遇到错误是否抛出异常，默认为false</param>
    /// <returns>删除统计信息（文件数、目录数）</returns>
    public static (int fileCount, int directoryCount) DeleteByPattern(
        string searchPath,
        string namePattern,
        bool recursive = true,
        bool includeDirectories = true,
        bool includeFiles = true,
        bool throwOnError = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(searchPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(namePattern);

        if (!Directory.Exists(searchPath))
        {
            TaktLogger.Warning("[TaktFileHelper] 按模式删除失败，搜索路径不存在: {SearchPath}", searchPath);
            if (throwOnError)
                throw new DirectoryNotFoundException($"搜索路径不存在: {searchPath}");
            return (0, 0);
        }

        var fileCount = 0;
        var directoryCount = 0;
        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        try
        {
            // 删除匹配的文件
            if (includeFiles)
            {
                var files = Directory.GetFiles(searchPath, "*", searchOption)
                    .Where(f => Path.GetFileName(f).Contains(namePattern, StringComparison.Ordinal))
                    .ToList();

                foreach (var file in files)
                {
                    try
                    {
                        if (DeleteFile(file, false))
                        {
                            fileCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (throwOnError)
                        {
                            TaktLogger.Error(ex, "[TaktFileHelper] 按模式删除文件失败: {FilePath}", file);
                            throw;
                        }
                        TaktLogger.Warning(ex, "[TaktFileHelper] 按模式删除文件失败: {FilePath}", file);
                    }
                }
            }

            // 删除匹配的目录（需要从最深层的目录开始删除）
            if (includeDirectories)
            {
                var directories = Directory.GetDirectories(searchPath, "*", searchOption)
                    .Where(d => Path.GetFileName(d).Contains(namePattern, StringComparison.Ordinal))
                    .OrderByDescending(d => d.Length) // 先删除深层目录
                    .ToList();

                foreach (var directory in directories)
                {
                    try
                    {
                        if (DeleteDirectory(directory, false))
                        {
                            directoryCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (throwOnError)
                        {
                            TaktLogger.Error(ex, "[TaktFileHelper] 按模式删除目录失败: {DirectoryPath}", directory);
                            throw;
                        }
                        TaktLogger.Warning(ex, "[TaktFileHelper] 按模式删除目录失败: {DirectoryPath}", directory);
                    }
                }
            }

            TaktLogger.Information("[TaktFileHelper] 按模式删除完成: 搜索路径: {SearchPath}, 模式: {Pattern}, 删除文件数: {FileCount}, 删除目录数: {DirectoryCount}", 
                searchPath, namePattern, fileCount, directoryCount);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 按模式删除失败: 搜索路径: {SearchPath}, 模式: {Pattern}", searchPath, namePattern);
            if (throwOnError)
                throw;
        }

        return (fileCount, directoryCount);
    }

    /// <summary>
    /// 按名称模式删除文件或目录（异步，支持递归搜索）
    /// </summary>
    /// <param name="searchPath">搜索路径（目录路径）</param>
    /// <param name="namePattern">名称模式（包含的字符串，区分大小写）</param>
    /// <param name="recursive">是否递归搜索子目录，默认为true</param>
    /// <param name="includeDirectories">是否删除匹配的目录，默认为true</param>
    /// <param name="includeFiles">是否删除匹配的文件，默认为true</param>
    /// <param name="throwOnError">遇到错误是否抛出异常，默认为false</param>
    /// <returns>删除统计信息（文件数、目录数）</returns>
    public static Task<(int fileCount, int directoryCount)> DeleteByPatternAsync(
        string searchPath,
        string namePattern,
        bool recursive = true,
        bool includeDirectories = true,
        bool includeFiles = true,
        bool throwOnError = false)
    {
        return Task.FromResult(DeleteByPattern(searchPath, namePattern, recursive, includeDirectories, includeFiles, throwOnError));
    }

    #endregion

    #region 文件工具方法

    /// <summary>
    /// 获取文件的MIME类型（使用MimeKit库）
    /// </summary>
    /// <param name="fileName">文件名或文件路径</param>
    /// <returns>MIME类型，如果无法识别则返回 "application/octet-stream"</returns>
    public static string GetMimeType(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "application/octet-stream";

        var extension = Path.GetExtension(fileName)?.TrimStart('.')?.ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension))
            return "application/octet-stream";

        // 使用 MimeKit 的 MimeTypes 类获取 MIME 类型
        try
        {
            var mimeType = MimeTypes.GetMimeType(extension);
            return mimeType ?? "application/octet-stream";
        }
        catch
        {
            return "application/octet-stream";
        }
    }

    /// <summary>
    /// 根据 MIME 类型（FileType）推断文件分类
    /// </summary>
    /// <param name="fileType">MIME 类型，如 image/png、application/pdf</param>
    /// <returns>文件分类（0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）</returns>
    public static FileCategory GetFileCategoryFromMimeType(string fileType)
    {
        if (string.IsNullOrWhiteSpace(fileType))
        {
            return FileCategory.Other;
        }

        var mime = fileType.Trim().Split(';')[0].Trim().ToLowerInvariant();
        if (mime.StartsWith("image/", StringComparison.Ordinal))
        {
            return FileCategory.Image;
        }

        if (mime.StartsWith("video/", StringComparison.Ordinal))
        {
            return FileCategory.Video;
        }

        if (mime.StartsWith("audio/", StringComparison.Ordinal))
        {
            return FileCategory.Audio;
        }

        if (mime is "application/zip" or "application/x-zip-compressed"
            or "application/x-rar-compressed" or "application/vnd.rar"
            or "application/x-7z-compressed" or "application/gzip"
            or "application/x-gzip" or "application/x-tar"
            or "application/x-bzip2" or "application/x-xz"
            or "application/vnd.ms-cab-compressed")
        {
            return FileCategory.Archive;
        }

        if (mime.StartsWith("text/", StringComparison.Ordinal)
            || mime is "application/pdf"
            || mime.StartsWith("application/vnd.", StringComparison.Ordinal)
            || mime is "application/msword"
            || mime is "application/vnd.ms-excel"
            || mime is "application/vnd.ms-powerpoint"
            || mime is "application/rtf")
        {
            return FileCategory.Document;
        }

        return FileCategory.Other;
    }

    /// <summary>
    /// 根据文件名推断文件分类（内部先解析 MIME 再分类）
    /// </summary>
    /// <param name="fileName">文件名或文件路径</param>
    /// <returns>文件分类</returns>
    public static FileCategory GetFileCategory(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return FileCategory.Other;
        }

        return GetFileCategoryFromMimeType(GetMimeType(fileName));
    }

    /// <summary>
    /// 按文件分类返回存储子目录段（与历史字典 sys_storage_directory 目录名对齐）
    /// </summary>
    /// <param name="category">文件分类</param>
    /// <returns>目录段，如 images、documents；Other 为 default</returns>
    public static string GetStorageDirectorySegment(FileCategory category)
    {
        switch (category)
        {
            case FileCategory.Document:
                return "documents";
            case FileCategory.Image:
                return "images";
            case FileCategory.Video:
                return "videos";
            case FileCategory.Audio:
                return "audios";
            case FileCategory.Archive:
                return "archives";
            default:
                return "default";
        }
    }

    /// <summary>
    /// 按文件分类整型返回存储子目录段
    /// </summary>
    /// <param name="fileCategory">文件分类 0~5</param>
    /// <returns>目录段</returns>
    public static string GetStorageDirectorySegment(int fileCategory)
    {
        if (fileCategory < 0 || fileCategory > 5)
        {
            return "default";
        }

        return GetStorageDirectorySegment((FileCategory)fileCategory);
    }

    /// <summary>
    /// 生成文件业务编码（租户+公司内唯一索引；与磁盘存储名 FileName 无关）
    /// </summary>
    /// <returns>业务编码，长度不超过 50</returns>
    public static string GenerateFileCode()
    {
        var stamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var suffix = Guid.NewGuid().ToString("N")[..12];
        return $"FILE{stamp}{suffix}";
    }

    /// <summary>
    /// 规范化前端传入的 storageNaming（字典 sys_storage_naming：0/1/2）
    /// </summary>
    /// <param name="storageNaming">表单传入值；为空时使用 defaultValue</param>
    /// <param name="defaultValue">缺省或非法时返回值，默认 0=原文件+哈希</param>
    /// <returns>0、1 或 2</returns>
    public static int NormalizeStorageNamingValue(int? storageNaming, int defaultValue = 0)
    {
        if (!storageNaming.HasValue)
        {
            return defaultValue;
        }

        return NormalizeStorageNaming(storageNaming.Value, defaultValue);
    }

    /// <summary>
    /// 按字典 sys_storage_naming 解析磁盘存储文件名（与 FileCode 业务编码无关）
    /// </summary>
    /// <param name="storageNaming">0=原文件+哈希，1=自动生成 GUID 文件名，2=自定义 targetFileName</param>
    /// <param name="originalFileName">原始文件名（含扩展名）</param>
    /// <param name="fileHash">文件 MD5；规则 0 时使用</param>
    /// <param name="targetFileName">自定义目标名；规则 2 时使用</param>
    /// <returns>磁盘存储文件名</returns>
    /// <exception cref="ArgumentException">originalFileName 为空</exception>
    public static string ResolveStoredFileName(
        int storageNaming,
        string originalFileName,
        string? fileHash,
        string? targetFileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);
        var fileExtension = Path.GetExtension(originalFileName)?.TrimStart('.')?.ToLowerInvariant() ?? string.Empty;
        var naming = NormalizeStorageNaming(storageNaming, 0);
        if (naming == 2)
        {
            return ResolveTargetOrAutoStoredFileName(fileExtension, targetFileName);
        }

        if (naming == 1)
        {
            return GenerateAutoStoredFileName(fileExtension);
        }

        var baseName = SanitizeStoredFileBaseName(originalFileName);
        var hashSegment = string.IsNullOrWhiteSpace(fileHash)
            ? Guid.NewGuid().ToString("N")
            : fileHash.Trim().ToLowerInvariant();
        return string.IsNullOrEmpty(fileExtension)
            ? $"{baseName}_{hashSegment}"
            : $"{baseName}_{hashSegment}.{fileExtension}";
    }

    /// <summary>
    /// 自动生成磁盘存储文件名（GUID + 扩展名）
    /// </summary>
    /// <param name="fileExtension">扩展名（不含点）</param>
    /// <returns>磁盘文件名</returns>
    public static string GenerateAutoStoredFileName(string fileExtension)
    {
        var token = Guid.NewGuid().ToString("N");
        return string.IsNullOrEmpty(fileExtension)
            ? token
            : $"{token}.{fileExtension}";
    }

    /// <summary>
    /// 清洗原文件名基名（去除非法字符并限制长度）
    /// </summary>
    /// <param name="originalFileName">原始文件名（含扩展名）</param>
    /// <returns>可用于存储文件名的基名</returns>
    /// <exception cref="ArgumentException">originalFileName 为空</exception>
    public static string SanitizeStoredFileBaseName(string originalFileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(baseName))
        {
            baseName = "file";
        }

        var invalidChars = Path.GetInvalidFileNameChars();
        var builder = new StringBuilder(baseName.Length);
        foreach (var character in baseName)
        {
            builder.Append(Array.IndexOf(invalidChars, character) >= 0 ? '_' : character);
        }

        var sanitized = builder.ToString().Trim().Trim('_');
        if (string.IsNullOrEmpty(sanitized))
        {
            sanitized = "file";
        }

        const int maxBaseLength = 100;
        if (sanitized.Length > maxBaseLength)
        {
            sanitized = sanitized[..maxBaseLength];
        }

        return sanitized;
    }

    /// <summary>
    /// 由文件编码与存储文件名生成 FileDescription 默认摘要
    /// </summary>
    /// <param name="fileCode">文件业务编码</param>
    /// <param name="fileName">存储文件名</param>
    /// <returns>编码与名称组合摘要</returns>
    /// <exception cref="ArgumentException">fileCode 或 fileName 为空</exception>
    public static string BuildFileCodeNameDescription(string fileCode, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return $"编码: {fileCode.Trim()} | 名称: {fileName.Trim()}";
    }

    /// <summary>
    /// 由上传原文件信息生成 FileDescription 默认摘要
    /// </summary>
    /// <param name="originalFileName">原始文件名（含扩展名）</param>
    /// <param name="fileSize">文件大小（字节）</param>
    /// <param name="fileType">MIME 类型</param>
    /// <param name="fileExtension">扩展名（不含点）</param>
    /// <returns>原文件信息摘要</returns>
    /// <exception cref="ArgumentException">originalFileName 为空</exception>
    public static string BuildOriginalFileInfoDescription(
        string originalFileName,
        long fileSize,
        string? fileType,
        string? fileExtension)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);
        if (fileSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fileSize), fileSize, "文件大小不能为负数");
        }

        var safeOriginalName = Path.GetFileName(originalFileName.Trim());
        var extension = string.IsNullOrWhiteSpace(fileExtension)
            ? Path.GetExtension(safeOriginalName)?.TrimStart('.') ?? string.Empty
            : fileExtension.Trim().TrimStart('.');
        var mime = string.IsNullOrWhiteSpace(fileType) ? string.Empty : fileType.Trim();
        var parts = new List<string> { $"原文件: {safeOriginalName}", $"大小: {fileSize} 字节" };
        if (!string.IsNullOrEmpty(mime))
        {
            parts.Add($"类型: {mime}");
        }

        if (!string.IsNullOrEmpty(extension))
        {
            parts.Add($"扩展名: {extension}");
        }

        return string.Join(" | ", parts);
    }

    /// <summary>
    /// 计算文件哈希值（MD5）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>MD5哈希值（小写）</returns>
    public static async Task<string> ComputeFileHashAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            TaktLogger.Warning("[TaktFileHelper] 计算文件哈希失败，文件不存在: {FilePath}", filePath);
            throw new FileNotFoundException($"文件不存在: {filePath}");
        }

        try
        {
            await using var stream = ReadFileStream(filePath);
            var hash = await ComputeStreamHashAsync(stream);
            TaktLogger.Information("[TaktFileHelper] 计算文件哈希成功: {FilePath}, 哈希值: {Hash}", filePath, hash);
            return hash;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 计算文件哈希失败: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// 计算流哈希值（MD5）
    /// </summary>
    /// <param name="stream">文件流</param>
    /// <returns>MD5哈希值（小写）</returns>
    public static async Task<string> ComputeStreamHashAsync(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var md5 = MD5.Create();
        var hashBytes = await md5.ComputeHashAsync(stream);
        var hashString = new StringBuilder();
        foreach (var b in hashBytes)
        {
            hashString.Append(b.ToString("x2"));
        }
        return hashString.ToString();
    }

    /// <summary>
    /// 计算字节数组哈希值（MD5）
    /// </summary>
    /// <param name="data">字节数组</param>
    /// <returns>MD5哈希值（小写）</returns>
    public static string ComputeHash(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(data);
        var hashString = new StringBuilder();
        foreach (var b in hashBytes)
        {
            hashString.Append(b.ToString("x2"));
        }
        return hashString.ToString();
    }

    /// <summary>
    /// 获取文件大小（字节）
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>文件大小，如果文件不存在返回-1</returns>
    public static long GetFileSize(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return -1;

        return new FileInfo(filePath).Length;
    }

    /// <summary>
    /// 格式化文件大小
    /// </summary>
    /// <param name="bytes">字节数</param>
    /// <returns>格式化后的文件大小字符串</returns>
    public static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    /// <summary>
    /// 生成唯一文件名（使用GUID）
    /// </summary>
    /// <param name="originalFileName">原始文件名</param>
    /// <returns>唯一文件名</returns>
    public static string GenerateUniqueFileName(string originalFileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);
        var extension = Path.GetExtension(originalFileName)?.TrimStart('.')?.ToLowerInvariant() ?? string.Empty;
        return GenerateAutoStoredFileName(extension);
    }

    /// <summary>
    /// 生成日期路径（年/月/日）
    /// </summary>
    /// <param name="date">日期，默认为当前日期</param>
    /// <returns>日期路径</returns>
    public static string GenerateDatePath(DateTime? date = null)
    {
        var targetDate = date ?? DateTime.Now;
        return Path.Combine(targetDate.Year.ToString(), targetDate.Month.ToString("D2"), targetDate.Day.ToString("D2"));
    }

    /// <summary>
    /// 确保目录存在
    /// </summary>
    /// <param name="directoryPath">目录路径</param>
    public static void EnsureDirectoryExists(string directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            return;

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    /// <summary>
    /// 解析可选配置路径（绝对路径或相对 ContentRoot）
    /// </summary>
    /// <param name="contentRootPath">Web ContentRoot 绝对路径</param>
    /// <param name="configuredRootPath">配置路径（可空）</param>
    /// <returns>规范化绝对路径</returns>
    /// <exception cref="ArgumentException">contentRootPath 为空</exception>
    public static string ResolveConfiguredStorageRootPath(string contentRootPath, string configuredRootPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentRootPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuredRootPath);
        var configured = configuredRootPath.Trim();
        return Path.IsPathRooted(configured)
            ? Path.GetFullPath(configured)
            : Path.GetFullPath(Path.Combine(contentRootPath, configured));
    }

    /// <summary>
    /// 解析分片临时根目录（默认 wwwroot；仅配置 ChunkStorageRootPath 时覆盖）
    /// </summary>
    /// <param name="contentRootPath">Web ContentRoot 绝对路径</param>
    /// <param name="configuredRootPath">配置项 ChunkStorageRootPath（可空）</param>
    /// <returns>分片根目录绝对路径（其下再拼 ChunkRelativePath）</returns>
    /// <exception cref="ArgumentException">contentRootPath 为空</exception>
    public static string ResolveChunkStorageRootPath(string contentRootPath, string? configuredRootPath = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentRootPath);
        if (!string.IsNullOrWhiteSpace(configuredRootPath))
        {
            return ResolveConfiguredStorageRootPath(contentRootPath, configuredRootPath);
        }

        return GetWwwRootPath(contentRootPath);
    }

    /// <summary>
    /// 解析本地正式文件存储根目录（默认 wwwroot；仅配置 UploadStorageRootPath 时覆盖）
    /// </summary>
    /// <param name="contentRootPath">Web ContentRoot 绝对路径</param>
    /// <param name="configuredUploadStorageRootPath">配置项 UploadStorageRootPath（可空）</param>
    /// <returns>本地正式文件存储根目录绝对路径</returns>
    /// <exception cref="ArgumentException">contentRootPath 为空</exception>
    public static string ResolveLocalUploadStorageRootPath(
        string contentRootPath,
        string? configuredUploadStorageRootPath = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentRootPath);
        if (!string.IsNullOrWhiteSpace(configuredUploadStorageRootPath))
        {
            return ResolveConfiguredStorageRootPath(contentRootPath, configuredUploadStorageRootPath);
        }

        return GetWwwRootPath(contentRootPath);
    }

    /// <summary>
    /// 获取wwwroot路径（支持多种路径解析策略）
    /// </summary>
    /// <param name="contentRootPath">内容根路径（可选，如果提供则优先使用）</param>
    /// <param name="baseDirectory">基础目录（可选，如果提供则使用，否则使用 AppContext.BaseDirectory）</param>
    /// <returns>wwwroot路径</returns>
    public static string GetWwwRootPath(string? contentRootPath = null, string? baseDirectory = null)
    {
        // 优先使用提供的 contentRootPath
        if (!string.IsNullOrEmpty(contentRootPath))
        {
            return Path.Combine(contentRootPath, "wwwroot");
        }
        
        // 使用提供的 baseDirectory 或默认的 AppContext.BaseDirectory，向上逐级查找 wwwroot（兼容 bin/Debug/net9.0 启动）
        var dir = string.IsNullOrEmpty(baseDirectory) ? AppContext.BaseDirectory : baseDirectory;
        var current = new DirectoryInfo(dir);
        while (current != null)
        {
            var candidate = Path.Combine(current.FullName, "wwwroot");
            if (Directory.Exists(candidate))
                return candidate;
            current = current.Parent;
        }
        return Path.Combine(dir, "wwwroot");
    }

    /// <summary>
    /// 从 Web 内容根或基础目录向上查找仓库根路径（同时存在 backend 与 frontend 子目录）。
    /// </summary>
    /// <param name="contentRootPath">Web 内容根路径（可空）</param>
    /// <param name="baseDirectory">基础目录（可空，默认 AppContext.BaseDirectory）</param>
    /// <returns>仓库根目录绝对路径</returns>
    /// <exception cref="InvalidOperationException">未找到符合条件的仓库根目录</exception>
    public static string GetSolutionRootPath(string? contentRootPath = null, string? baseDirectory = null)
    {
        var startDir = !string.IsNullOrWhiteSpace(contentRootPath)
            ? contentRootPath
            : (string.IsNullOrWhiteSpace(baseDirectory) ? AppContext.BaseDirectory : baseDirectory);
        var current = new DirectoryInfo(startDir);
        while (current != null)
        {
            var backendDir = Path.Combine(current.FullName, "backend");
            var frontendDir = Path.Combine(current.FullName, "frontend");
            if (Directory.Exists(backendDir) && Directory.Exists(frontendDir))
                return current.FullName;
            current = current.Parent;
        }
        throw new InvalidOperationException(
            $"未找到项目根目录（需同时存在 backend 与 frontend 子目录），起始路径：{startDir}");
    }

    /// <summary>
    /// 复制文件（同步）
    /// </summary>
    /// <param name="sourcePath">源文件路径</param>
    /// <param name="destinationPath">目标文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为false</param>
    public static void CopyFile(string sourcePath, string destinationPath, bool overwrite = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        if (!File.Exists(sourcePath))
        {
            TaktLogger.Warning("[TaktFileHelper] 复制文件失败，源文件不存在: {SourcePath}", sourcePath);
            throw new FileNotFoundException($"源文件不存在: {sourcePath}");
        }

        try
        {
            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                EnsureDirectoryExists(directory);
            }

            File.Copy(sourcePath, destinationPath, overwrite);
            var fileSize = GetFileSize(destinationPath);
            TaktLogger.Information("[TaktFileHelper] 复制文件成功: {SourcePath} -> {DestinationPath}, 大小: {Size} 字节, 覆盖: {Overwrite}", sourcePath, destinationPath, fileSize, overwrite);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 复制文件失败: {SourcePath} -> {DestinationPath}", sourcePath, destinationPath);
            throw;
        }
    }

    /// <summary>
    /// 复制文件（异步）
    /// </summary>
    /// <param name="sourcePath">源文件路径</param>
    /// <param name="destinationPath">目标文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为false</param>
    /// <returns>任务</returns>
    public static async Task CopyFileAsync(string sourcePath, string destinationPath, bool overwrite = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        if (!File.Exists(sourcePath))
        {
            TaktLogger.Warning("[TaktFileHelper] 复制文件失败，源文件不存在: {SourcePath}", sourcePath);
            throw new FileNotFoundException($"源文件不存在: {sourcePath}");
        }

        var directory = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrWhiteSpace(directory))
            EnsureDirectoryExists(directory);

        if (File.Exists(destinationPath) && !overwrite)
            throw new IOException($"目标文件已存在: {destinationPath}");

        await using var source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, FileOptions.Asynchronous);
        await using var destination = new FileStream(destinationPath, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, FileOptions.Asynchronous);
        await source.CopyToAsync(destination);
        await destination.FlushAsync();
        var fileSize = GetFileSize(destinationPath);
        TaktLogger.Information("[TaktFileHelper] 复制文件成功: {SourcePath} -> {DestinationPath}, 大小: {Size} 字节, 覆盖: {Overwrite}", sourcePath, destinationPath, fileSize, overwrite);
    }

    /// <summary>
    /// 移动文件（同步）
    /// </summary>
    /// <param name="sourcePath">源文件路径</param>
    /// <param name="destinationPath">目标文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为false</param>
    public static void MoveFile(string sourcePath, string destinationPath, bool overwrite = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourcePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationPath);

        if (!File.Exists(sourcePath))
        {
            TaktLogger.Warning("[TaktFileHelper] 移动文件失败，源文件不存在: {SourcePath}", sourcePath);
            throw new FileNotFoundException($"源文件不存在: {sourcePath}");
        }

        if (File.Exists(destinationPath) && !overwrite)
        {
            TaktLogger.Warning("[TaktFileHelper] 移动文件失败，目标文件已存在: {DestinationPath}", destinationPath);
            throw new IOException($"目标文件已存在: {destinationPath}");
        }

        try
        {
            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                EnsureDirectoryExists(directory);
            }

            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }

            File.Move(sourcePath, destinationPath);
            var fileSize = GetFileSize(destinationPath);
            TaktLogger.Information("[TaktFileHelper] 移动文件成功: {SourcePath} -> {DestinationPath}, 大小: {Size} 字节, 覆盖: {Overwrite}", sourcePath, destinationPath, fileSize, overwrite);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFileHelper] 移动文件失败: {SourcePath} -> {DestinationPath}", sourcePath, destinationPath);
            throw;
        }
    }

    /// <summary>
    /// 移动文件（异步）
    /// </summary>
    /// <param name="sourcePath">源文件路径</param>
    /// <param name="destinationPath">目标文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为false</param>
    /// <returns>任务</returns>
    public static async Task MoveFileAsync(string sourcePath, string destinationPath, bool overwrite = false)
    {
        await CopyFileAsync(sourcePath, destinationPath, overwrite);
        await DeleteFileAsync(sourcePath, throwIfNotExists: true);
    }

    /// <summary>
    /// 物理删除标记（插入扩展名前，如 report.xlsx → report.del.xlsx）
    /// </summary>
    public const string DeletedPhysicalFileMarker = ".del";

    /// <summary>
    /// 构建带删除标记的物理文件名（xxx.ext → xxx.del.ext；无扩展名时 xxx → xxx.del）
    /// </summary>
    /// <param name="fileName">原物理文件名（不含目录）</param>
    /// <returns>带删除标记的文件名</returns>
    /// <exception cref="ArgumentException">fileName 为空</exception>
    public static string BuildDeletedPhysicalFileName(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extension))
        {
            return $"{fileName}{DeletedPhysicalFileMarker}";
        }

        var baseName = Path.GetFileNameWithoutExtension(fileName);
        return $"{baseName}{DeletedPhysicalFileMarker}{extension}";
    }

    /// <summary>
    /// 判断物理文件名是否已带删除标记
    /// </summary>
    /// <param name="fileName">物理文件名（不含目录）</param>
    /// <returns>已带 .del 标记时为 true</returns>
    /// <exception cref="ArgumentException">fileName 为空</exception>
    public static bool IsDeletedPhysicalFileName(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(extension))
        {
            return fileName.EndsWith(DeletedPhysicalFileMarker, StringComparison.OrdinalIgnoreCase);
        }

        var baseName = Path.GetFileNameWithoutExtension(fileName);
        return baseName.EndsWith(DeletedPhysicalFileMarker, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 将相对路径末段文件名替换为带删除标记的文件名
    /// </summary>
    /// <param name="relativePath">存储相对路径</param>
    /// <returns>替换文件名后的相对路径</returns>
    /// <exception cref="ArgumentException">relativePath 为空</exception>
    public static string BuildDeletedPhysicalRelativePath(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        var normalized = relativePath.Replace('\\', '/').Trim('/');
        var fileName = Path.GetFileName(normalized);
        var deletedFileName = BuildDeletedPhysicalFileName(fileName);
        var directory = Path.GetDirectoryName(normalized.Replace('/', Path.DirectorySeparatorChar));
        if (string.IsNullOrWhiteSpace(directory))
        {
            return deletedFileName;
        }

        return $"{directory.Replace('\\', '/')}/{deletedFileName}";
    }

    /// <summary>
    /// 将 storageNaming 约束到 0~2
    /// </summary>
    /// <param name="storageNaming">原始值</param>
    /// <param name="fallback">越界时的回退值</param>
    /// <returns>0、1 或 2</returns>
    private static int NormalizeStorageNaming(int storageNaming, int fallback)
    {
        return storageNaming switch
        {
            0 or 1 or 2 => storageNaming,
            _ => fallback is 0 or 1 or 2 ? fallback : 0,
        };
    }

    /// <summary>
    /// 解析 targetFileName 或回退为独立 GUID 存储名
    /// </summary>
    /// <param name="fileExtension">扩展名（不含点）</param>
    /// <param name="targetFileName">目标文件名（可选）</param>
    /// <returns>磁盘文件名</returns>
    private static string ResolveTargetOrAutoStoredFileName(string fileExtension, string? targetFileName)
    {
        if (!string.IsNullOrWhiteSpace(targetFileName))
        {
            var trimmed = Path.GetFileName(targetFileName.Trim());
            if (string.IsNullOrEmpty(Path.GetExtension(trimmed)) && !string.IsNullOrEmpty(fileExtension))
            {
                return $"{trimmed}.{fileExtension}";
            }

            return trimmed;
        }

        return GenerateAutoStoredFileName(fileExtension);
    }

    #endregion
}
