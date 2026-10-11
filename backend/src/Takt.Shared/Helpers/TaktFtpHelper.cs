// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFtpHelper.cs
// 创建时间：2025-01-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt FTP帮助类，使用 FluentFTP 进行文件传输
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentFTP;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// Takt FTP帮助类
/// </summary>
/// <remarks>网络/文件 I/O 网关；连接配置由调用方通过 TaktFtpOptions 或 IConfiguration 显式传入。</remarks>
public static class TaktFtpHelper
{
    /// <summary>
    /// 从配置中读取 FTP 设置（键与字典 <c>sys_ftp_provider</c> 一致，如 <c>teac_cn</c>、<c>teac_jp</c>）。
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识，对应 <c>Ftp:{provider}</c> 节点</param>
    /// <returns>FTP 配置</returns>
    public static TaktFtpOptions GetFtpOptionsFromConfiguration(IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        return configuration.RequireFtpProvider(provider);
    }

    /// <summary>
    /// 从 JSON 字符串解析 FTP 配置
    /// </summary>
    /// <param name="jsonConfig">JSON 配置字符串</param>
    /// <returns>FTP 配置；解析失败返回 null</returns>
    public static TaktFtpOptions? GetFtpOptionsFromJson(string? jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig))
            return null;

        try
        {
            return JsonConvert.DeserializeObject<TaktFtpOptions>(jsonConfig);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] 解析 FTP 配置 JSON 失败: {JsonConfig}", jsonConfig);
            return null;
        }
    }

    /// <summary>
    /// 创建FTP客户端
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <returns>FTP客户端</returns>
    private static AsyncFtpClient CreateFtpClient(TaktFtpOptions config)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Host);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.UserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Password);

        var client = new AsyncFtpClient(config.Host, config.UserName, config.Password, config.Port);
        
        if (config.EnableSsl)
        {
            client.Config.EncryptionMode = FtpEncryptionMode.Explicit;
            client.Config.ValidateAnyCertificate = true; // 生产环境应设置为false并配置证书
        }

        client.Config.ConnectTimeout = config.Timeout * 1000; // 转换为毫秒
        client.Config.ReadTimeout = config.Timeout * 1000;
        client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive;

        return client;
    }

    /// <summary>
    /// 文件上传引擎（StorageType=FTP）专用：经 FluentFTP 将本地暂存文件推送到 FTP
    /// </summary>
    /// <param name="config">FTP 配置（appsettings Ftp:{provider}）</param>
    /// <param name="localFilePath">本地暂存绝对路径</param>
    /// <param name="remoteFilePath">远程相对路径（由 BasePath 组合）</param>
    /// <param name="overwrite">是否覆盖远程已存在文件</param>
    /// <returns>任务</returns>
    /// <exception cref="ArgumentNullException">config 为 null</exception>
    /// <exception cref="ArgumentException">路径为空</exception>
    /// <exception cref="FileNotFoundException">本地文件不存在</exception>
    public static Task UploadLocalFileViaFluentFtpAsync(
        TaktFtpOptions config,
        string localFilePath,
        string remoteFilePath,
        bool overwrite = true) =>
        UploadFileAsync(config, localFilePath, remoteFilePath, overwrite);

    /// <summary>
    /// 上传文件到FTP服务器（FluentFTP AsyncFtpClient.UploadFile）
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="localFilePath">本地文件路径</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为true</param>
    /// <returns>任务</returns>
    public static async Task UploadFileAsync(TaktFtpOptions config, string localFilePath, string remoteFilePath, bool overwrite = true)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(localFilePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);

        if (!File.Exists(localFilePath))
        {
            TaktLogger.Warning("[TaktFtpHelper] 本地文件不存在: {LocalFilePath}", localFilePath);
            throw new FileNotFoundException($"本地文件不存在: {localFilePath}");
        }

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            // 确保远程目录存在
            var remoteDir = Path.GetDirectoryName(fullRemotePath)?.Replace('\\', '/');
            if (!string.IsNullOrWhiteSpace(remoteDir))
            {
                await client.CreateDirectory(remoteDir);
            }

            // 上传文件
            var uploadStatus = await client.UploadFile(localFilePath, fullRemotePath, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
            
            if (uploadStatus == FtpStatus.Success)
            {
                var fileSize = new FileInfo(localFilePath).Length;
                TaktLogger.Information("[TaktFtpHelper] FTP文件上传成功: {LocalFilePath} -> {RemoteFilePath}, 大小: {Size} 字节", 
                    localFilePath, fullRemotePath, fileSize);
            }
            else
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP文件上传状态: {Status}, 本地文件: {LocalFilePath}, 远程文件: {RemoteFilePath}", 
                    uploadStatus, localFilePath, fullRemotePath);
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP文件上传失败: {LocalFilePath} -> {RemoteFilePath}", localFilePath, remoteFilePath);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 从FTP服务器下载文件
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <param name="localFilePath">本地文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为true</param>
    /// <returns>任务</returns>
    public static async Task DownloadFileAsync(TaktFtpOptions config, string remoteFilePath, string localFilePath, bool overwrite = true)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(localFilePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            // 检查远程文件是否存在
            if (!await client.FileExists(fullRemotePath))
            {
                TaktLogger.Warning("[TaktFtpHelper] 远程文件不存在: {RemoteFilePath}", fullRemotePath);
                throw new FileNotFoundException($"远程文件不存在: {fullRemotePath}");
            }

            // 确保本地目录存在
            var localDir = Path.GetDirectoryName(localFilePath);
            if (!string.IsNullOrWhiteSpace(localDir))
            {
                Directory.CreateDirectory(localDir);
            }

            // 下载文件
            var downloadStatus = await client.DownloadFile(localFilePath, fullRemotePath, overwrite ? FtpLocalExists.Overwrite : FtpLocalExists.Skip);
            
            if (downloadStatus == FtpStatus.Success)
            {
                var fileSize = new FileInfo(localFilePath).Length;
                TaktLogger.Information("[TaktFtpHelper] FTP文件下载成功: {RemoteFilePath} -> {LocalFilePath}, 大小: {Size} 字节", 
                    fullRemotePath, localFilePath, fileSize);
            }
            else
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP文件下载状态: {Status}, 远程文件: {RemoteFilePath}, 本地文件: {LocalFilePath}", 
                    downloadStatus, fullRemotePath, localFilePath);
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP文件下载失败: {RemoteFilePath} -> {LocalFilePath}", remoteFilePath, localFilePath);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 删除FTP服务器上的文件
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <returns>任务</returns>
    public static async Task DeleteFileAsync(TaktFtpOptions config, string remoteFilePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            // 检查文件是否存在
            if (!await client.FileExists(fullRemotePath))
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP文件不存在，跳过删除: {RemoteFilePath}", fullRemotePath);
                return;
            }

            // 删除文件
            await client.DeleteFile(fullRemotePath);
            TaktLogger.Information("[TaktFtpHelper] FTP文件删除成功: {RemoteFilePath}", fullRemotePath);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP文件删除失败: {RemoteFilePath}", remoteFilePath);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 检查FTP服务器上的文件是否存在
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <returns>文件是否存在</returns>
    public static async Task<bool> FileExistsAsync(TaktFtpOptions config, string remoteFilePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            return await client.FileExists(fullRemotePath);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] 检查FTP文件是否存在失败: {RemoteFilePath}", remoteFilePath);
            return false;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 列出FTP服务器上的文件
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteDirectory">远程目录路径（相对于BasePath或FTP根目录，默认为空表示根目录）</param>
    /// <returns>文件列表</returns>
    public static async Task<List<FtpListItem>> ListFilesAsync(TaktFtpOptions config, string? remoteDirectory = null)
    {
        ArgumentNullException.ThrowIfNull(config);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteDirectory ?? string.Empty);

            var items = await client.GetListing(fullRemotePath);
            var files = items.Where(item => item.Type == FtpObjectType.File).ToList();

            TaktLogger.Information("[TaktFtpHelper] FTP列出文件成功: 目录: {RemoteDirectory}, 文件数: {FileCount}", 
                fullRemotePath, files.Count);

            return files;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP列出文件失败: 目录: {RemoteDirectory}", remoteDirectory ?? "根目录");
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 列出FTP服务器上的目录
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteDirectory">远程目录路径（相对于BasePath或FTP根目录，默认为空表示根目录）</param>
    /// <returns>目录列表</returns>
    public static async Task<List<FtpListItem>> ListDirectoriesAsync(TaktFtpOptions config, string? remoteDirectory = null)
    {
        ArgumentNullException.ThrowIfNull(config);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteDirectory ?? string.Empty);

            var items = await client.GetListing(fullRemotePath);
            var directories = items.Where(item => item.Type == FtpObjectType.Directory).ToList();

            TaktLogger.Information("[TaktFtpHelper] FTP列出目录成功: 目录: {RemoteDirectory}, 目录数: {DirectoryCount}", 
                fullRemotePath, directories.Count);

            return directories;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP列出目录失败: 目录: {RemoteDirectory}", remoteDirectory ?? "根目录");
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 列出FTP服务器上的所有项目（文件和目录）
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteDirectory">远程目录路径（相对于BasePath或FTP根目录，默认为空表示根目录）</param>
    /// <returns>所有项目列表</returns>
    public static async Task<List<FtpListItem>> ListAllAsync(TaktFtpOptions config, string? remoteDirectory = null)
    {
        ArgumentNullException.ThrowIfNull(config);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteDirectory ?? string.Empty);

            var items = await client.GetListing(fullRemotePath);
            var allItems = items.ToList();

            TaktLogger.Information("[TaktFtpHelper] FTP列出所有项目成功: 目录: {RemoteDirectory}, 项目数: {ItemCount}", 
                fullRemotePath, allItems.Count);

            return allItems;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP列出所有项目失败: 目录: {RemoteDirectory}", remoteDirectory ?? "根目录");
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 重命名或移动FTP服务器上的文件或目录
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remotePath">远程文件或目录路径（相对于BasePath或FTP根目录）</param>
    /// <param name="newRemotePath">新的远程文件或目录路径（相对于BasePath或FTP根目录）</param>
    /// <returns>任务</returns>
    public static async Task RenameAsync(TaktFtpOptions config, string remotePath, string newRemotePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remotePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(newRemotePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remotePath);
            var fullNewRemotePath = BuildRemotePath(config, newRemotePath);

            // 重命名或移动
            await client.Rename(fullRemotePath, fullNewRemotePath);
            TaktLogger.Information("[TaktFtpHelper] FTP重命名/移动成功: {RemotePath} -> {NewRemotePath}", 
                fullRemotePath, fullNewRemotePath);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP重命名/移动失败: {RemotePath} -> {NewRemotePath}", 
                remotePath, newRemotePath);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 获取FTP服务器上文件或目录的修改时间
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remotePath">远程文件或目录路径（相对于BasePath或FTP根目录）</param>
    /// <returns>修改时间，如果文件或目录不存在返回null</returns>
    public static async Task<DateTime?> GetModifiedTimeAsync(TaktFtpOptions config, string remotePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remotePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remotePath);

            if (!await client.FileExists(fullRemotePath) && !await client.DirectoryExists(fullRemotePath))
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP文件或目录不存在: {RemotePath}", fullRemotePath);
                return null;
            }

            var modifiedTime = await client.GetModifiedTime(fullRemotePath);
            TaktLogger.Information("[TaktFtpHelper] FTP获取修改时间成功: {RemotePath}, 修改时间: {ModifiedTime}", 
                fullRemotePath, modifiedTime);
            return modifiedTime;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP获取修改时间失败: {RemotePath}", remotePath);
            return null;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 使用流上传文件到FTP服务器
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="stream">文件流</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为true</param>
    /// <returns>任务</returns>
    public static async Task UploadStreamAsync(TaktFtpOptions config, Stream stream, string remoteFilePath, bool overwrite = true)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            // 确保远程目录存在
            var remoteDir = Path.GetDirectoryName(fullRemotePath)?.Replace('\\', '/');
            if (!string.IsNullOrWhiteSpace(remoteDir))
            {
                await client.CreateDirectory(remoteDir);
            }

            // 上传流
            var uploadStatus = await client.UploadStream(stream, fullRemotePath, overwrite ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip);
            
            if (uploadStatus == FtpStatus.Success)
            {
                TaktLogger.Information("[TaktFtpHelper] FTP流上传成功: {RemoteFilePath}, 大小: {Size} 字节", 
                    fullRemotePath, stream.Length);
            }
            else
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP流上传状态: {Status}, 远程文件: {RemoteFilePath}", 
                    uploadStatus, fullRemotePath);
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP流上传失败: {RemoteFilePath}", remoteFilePath);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 使用流从FTP服务器下载文件
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <param name="stream">目标流</param>
    /// <returns>任务</returns>
    public static async Task DownloadStreamAsync(TaktFtpOptions config, string remoteFilePath, Stream stream)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);
        ArgumentNullException.ThrowIfNull(stream);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            // 检查远程文件是否存在
            if (!await client.FileExists(fullRemotePath))
            {
                TaktLogger.Warning("[TaktFtpHelper] 远程文件不存在: {RemoteFilePath}", fullRemotePath);
                throw new FileNotFoundException($"远程文件不存在: {fullRemotePath}");
            }

            // 下载到流
            var success = await client.DownloadStream(stream, fullRemotePath);
            
            if (success)
            {
                TaktLogger.Information("[TaktFtpHelper] FTP流下载成功: {RemoteFilePath}, 大小: {Size} 字节", 
                    fullRemotePath, stream.Length);
            }
            else
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP流下载失败: {RemoteFilePath}", fullRemotePath);
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP流下载失败: {RemoteFilePath}", remoteFilePath);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 创建FTP目录
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteDirectory">远程目录路径（相对于BasePath或FTP根目录）</param>
    /// <returns>任务</returns>
    public static async Task CreateDirectoryAsync(TaktFtpOptions config, string remoteDirectory)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteDirectory);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteDirectory);

            // 创建目录（递归创建父目录）
            await client.CreateDirectory(fullRemotePath);
            TaktLogger.Information("[TaktFtpHelper] FTP目录创建成功: {RemoteDirectory}", fullRemotePath);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP目录创建失败: {RemoteDirectory}", remoteDirectory);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 删除FTP目录（递归删除）
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteDirectory">远程目录路径（相对于BasePath或FTP根目录）</param>
    /// <returns>任务</returns>
    public static async Task DeleteDirectoryAsync(TaktFtpOptions config, string remoteDirectory)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteDirectory);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteDirectory);

            // 删除目录（递归删除）
            await client.DeleteDirectory(fullRemotePath);
            TaktLogger.Information("[TaktFtpHelper] FTP目录删除成功: {RemoteDirectory}", fullRemotePath);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP目录删除失败: {RemoteDirectory}", remoteDirectory);
            throw;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 获取FTP文件大小
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remoteFilePath">远程文件路径（相对于BasePath或FTP根目录）</param>
    /// <returns>文件大小（字节），如果文件不存在返回-1</returns>
    public static async Task<long> GetFileSizeAsync(TaktFtpOptions config, string remoteFilePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(remoteFilePath);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            // 构建完整远程路径
            var fullRemotePath = BuildRemotePath(config, remoteFilePath);

            if (!await client.FileExists(fullRemotePath))
            {
                TaktLogger.Warning("[TaktFtpHelper] FTP文件不存在: {RemoteFilePath}", fullRemotePath);
                return -1;
            }

            var fileSize = await client.GetFileSize(fullRemotePath);
            TaktLogger.Information("[TaktFtpHelper] FTP文件大小: {RemoteFilePath}, 大小: {Size} 字节", fullRemotePath, fileSize);
            return fileSize;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] 获取FTP文件大小失败: {RemoteFilePath}", remoteFilePath);
            return -1;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    /// <summary>
    /// 测试FTP连接
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <returns>连接是否成功</returns>
    public static async Task<bool> TestConnectionAsync(TaktFtpOptions config)
    {
        ArgumentNullException.ThrowIfNull(config);

        AsyncFtpClient? client = null;
        try
        {
            client = CreateFtpClient(config);
            await client.AutoConnect();

            TaktLogger.Information("[TaktFtpHelper] FTP连接测试成功: {Host}:{Port}", config.Host, config.Port);
            return true;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktFtpHelper] FTP连接测试失败: {Host}:{Port}", config.Host, config.Port);
            return false;
        }
        finally
        {
            if (client != null && client.IsConnected)
            {
                await client.Disconnect();
            }
            client?.Dispose();
        }
    }

    // ========== 直接使用 IConfiguration 的重载方法（provider 与 sys_ftp_provider 字典值一致） ==========

    /// <summary>
    /// 上传文件到 FTP 服务器（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="localFilePath">本地文件路径</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为 true</param>
    /// <returns>任务</returns>
    public static async Task UploadFileAsync(IConfiguration configuration, string provider, string localFilePath, string remoteFilePath, bool overwrite = true)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await UploadFileAsync(config, localFilePath, remoteFilePath, overwrite);
    }

    /// <summary>
    /// 从 FTP 服务器下载文件（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <param name="localFilePath">本地文件路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为 true</param>
    /// <returns>任务</returns>
    public static async Task DownloadFileAsync(IConfiguration configuration, string provider, string remoteFilePath, string localFilePath, bool overwrite = true)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await DownloadFileAsync(config, remoteFilePath, localFilePath, overwrite);
    }

    /// <summary>
    /// 删除 FTP 服务器上的文件（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>任务</returns>
    public static async Task DeleteFileAsync(IConfiguration configuration, string provider, string remoteFilePath)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await DeleteFileAsync(config, remoteFilePath);
    }

    /// <summary>
    /// 检查 FTP 服务器上的文件是否存在（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>文件是否存在</returns>
    public static async Task<bool> FileExistsAsync(IConfiguration configuration, string provider, string remoteFilePath)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await FileExistsAsync(config, remoteFilePath);
    }

    /// <summary>
    /// 列出 FTP 服务器上的文件（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteDirectory">远程目录路径（相对于 BasePath 或 FTP 根目录，默认为空表示根目录）</param>
    /// <returns>文件列表</returns>
    public static async Task<List<FtpListItem>> ListFilesAsync(IConfiguration configuration, string provider, string? remoteDirectory = null)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await ListFilesAsync(config, remoteDirectory);
    }

    /// <summary>
    /// 列出 FTP 服务器上的目录（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteDirectory">远程目录路径（相对于 BasePath 或 FTP 根目录，默认为空表示根目录）</param>
    /// <returns>目录列表</returns>
    public static async Task<List<FtpListItem>> ListDirectoriesAsync(IConfiguration configuration, string provider, string? remoteDirectory = null)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await ListDirectoriesAsync(config, remoteDirectory);
    }

    /// <summary>
    /// 列出 FTP 服务器上的所有项目（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteDirectory">远程目录路径（相对于 BasePath 或 FTP 根目录，默认为空表示根目录）</param>
    /// <returns>所有项目列表</returns>
    public static async Task<List<FtpListItem>> ListAllAsync(IConfiguration configuration, string provider, string? remoteDirectory = null)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await ListAllAsync(config, remoteDirectory);
    }

    /// <summary>
    /// 创建 FTP 目录（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteDirectory">远程目录路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>任务</returns>
    public static async Task CreateDirectoryAsync(IConfiguration configuration, string provider, string remoteDirectory)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await CreateDirectoryAsync(config, remoteDirectory);
    }

    /// <summary>
    /// 删除 FTP 目录（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteDirectory">远程目录路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>任务</returns>
    public static async Task DeleteDirectoryAsync(IConfiguration configuration, string provider, string remoteDirectory)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await DeleteDirectoryAsync(config, remoteDirectory);
    }

    /// <summary>
    /// 获取 FTP 文件大小（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>文件大小（字节），如果文件不存在返回 -1</returns>
    public static async Task<long> GetFileSizeAsync(IConfiguration configuration, string provider, string remoteFilePath)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await GetFileSizeAsync(config, remoteFilePath);
    }

    /// <summary>
    /// 测试 FTP 连接（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <returns>连接是否成功</returns>
    public static async Task<bool> TestConnectionAsync(IConfiguration configuration, string provider)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await TestConnectionAsync(config);
    }

    /// <summary>
    /// 重命名或移动 FTP 服务器上的文件或目录（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remotePath">远程文件或目录路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <param name="newRemotePath">新的远程文件或目录路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>任务</returns>
    public static async Task RenameAsync(IConfiguration configuration, string provider, string remotePath, string newRemotePath)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await RenameAsync(config, remotePath, newRemotePath);
    }

    /// <summary>
    /// 获取 FTP 服务器上文件或目录的修改时间（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remotePath">远程文件或目录路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <returns>修改时间，如果文件或目录不存在返回 null</returns>
    public static async Task<DateTime?> GetModifiedTimeAsync(IConfiguration configuration, string provider, string remotePath)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        return await GetModifiedTimeAsync(config, remotePath);
    }

    /// <summary>
    /// 使用流上传文件到 FTP 服务器（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="stream">文件流</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <param name="overwrite">是否覆盖已存在的文件，默认为 true</param>
    /// <returns>任务</returns>
    public static async Task UploadStreamAsync(IConfiguration configuration, string provider, Stream stream, string remoteFilePath, bool overwrite = true)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await UploadStreamAsync(config, stream, remoteFilePath, overwrite);
    }

    /// <summary>
    /// 使用流从 FTP 服务器下载文件（使用配置）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">FTP 提供商标识（如 <c>teac_cn</c>）</param>
    /// <param name="remoteFilePath">远程文件路径（相对于 BasePath 或 FTP 根目录）</param>
    /// <param name="stream">目标流</param>
    /// <returns>任务</returns>
    public static async Task DownloadStreamAsync(IConfiguration configuration, string provider, string remoteFilePath, Stream stream)
    {
        var config = GetFtpOptionsFromConfiguration(configuration, provider);
        await DownloadStreamAsync(config, remoteFilePath, stream);
    }

    /// <summary>
    /// 构建完整远程路径（包含BasePath）
    /// </summary>
    /// <param name="config">FTP配置</param>
    /// <param name="remotePath">远程路径</param>
    /// <returns>完整远程路径</returns>
    private static string BuildRemotePath(TaktFtpOptions config, string remotePath)
    {
        if (string.IsNullOrWhiteSpace(remotePath))
            return config.BasePath ?? "/";

        // 标准化路径分隔符
        remotePath = remotePath.Replace('\\', '/').TrimStart('/');

        // 如果有BasePath，则组合路径
        if (!string.IsNullOrWhiteSpace(config.BasePath))
        {
            var basePath = config.BasePath.TrimEnd('/');
            return $"{basePath}/{remotePath}";
        }

        return remotePath;
    }
}
