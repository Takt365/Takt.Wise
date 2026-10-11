// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktOssHelper.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：OSS 对象存储配置读取帮助类
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Aliyun.OSS;
using Aliyun.OSS.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// OSS 对象存储配置与 I/O 帮助类
/// </summary>
/// <remarks>无状态；从配置解析选项；I/O 方法当前实现阿里云 OSS（sys_oss_provider=aliyun）。</remarks>
public static class TaktOssHelper
{
    /// <summary>
    /// 当前引擎已实现的 OSS 提供商标识
    /// </summary>
    public const string SupportedProviderAliyun = "aliyun";
    /// <summary>
    /// 从配置中读取 OSS 设置（键与字典 <c>sys_oss_provider</c> 一致，如 <c>aliyun</c>）。
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="provider">OSS 提供商标识，对应 <c>Oss:{provider}</c> 节点</param>
    /// <returns>OSS 配置</returns>
    /// <exception cref="ArgumentNullException"><paramref name="configuration"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="provider"/> 为空</exception>
    public static TaktOssOptions GetOssOptionsFromConfiguration(IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        return configuration.RequireOssProvider(provider);
    }

    /// <summary>
    /// 从 JSON 字符串解析 OSS 配置
    /// </summary>
    /// <param name="jsonConfig">JSON 配置字符串</param>
    /// <returns>OSS 配置；解析失败返回 null</returns>
    public static TaktOssOptions? GetOssOptionsFromJson(string? jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig))
            return null;

        try
        {
            return JsonConvert.DeserializeObject<TaktOssOptions>(jsonConfig);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktOssHelper] 解析 OSS 配置 JSON 失败: {JsonConfig}", jsonConfig);
            return null;
        }
    }

    /// <summary>
    /// 校验 OSS 提供商标识是否受引擎支持
    /// </summary>
    /// <param name="provider">提供商标识</param>
    /// <returns>是否支持</returns>
    public static bool IsSupportedProvider(string provider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        return string.Equals(provider.Trim(), SupportedProviderAliyun, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 创建阿里云 OSS 客户端
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <returns>OSS 客户端</returns>
    /// <exception cref="ArgumentNullException">options 为 null</exception>
    public static IOss CreateClient(TaktOssOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Endpoint);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.AccessKeyId);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.AccessKeySecret);
        return new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
    }

    /// <summary>
    /// 上传对象至 OSS
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <param name="objectKey">对象键</param>
    /// <param name="content">可读流</param>
    /// <param name="contentType">MIME；为空时不设置 ContentType</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    /// <exception cref="ArgumentNullException">options 或 content 为 null</exception>
    /// <exception cref="ArgumentException">objectKey 为空</exception>
    public static Task PutObjectAsync(
        TaktOssOptions options,
        string objectKey,
        Stream content,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(objectKey);
        cancellationToken.ThrowIfCancellationRequested();
        var client = CreateClient(options);
        var metadata = new ObjectMetadata();
        if (!string.IsNullOrWhiteSpace(contentType))
        {
            metadata.ContentType = contentType.Trim();
        }

        client.PutObject(options.Bucket, objectKey, content, metadata);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 从 OSS 下载对象为可读流（调用方负责释放）
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <param name="objectKey">对象键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>对象内容流</returns>
    /// <exception cref="ArgumentNullException">options 为 null</exception>
    /// <exception cref="ArgumentException">objectKey 为空</exception>
    public static Task<Stream> GetObjectStreamAsync(
        TaktOssOptions options,
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(objectKey);
        cancellationToken.ThrowIfCancellationRequested();
        var client = CreateClient(options);
        var ossObject = client.GetObject(options.Bucket, objectKey);
        return Task.FromResult<Stream>(ossObject.Content);
    }

    /// <summary>
    /// 删除 OSS 对象
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <param name="objectKey">对象键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public static Task DeleteObjectAsync(
        TaktOssOptions options,
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(objectKey);
        cancellationToken.ThrowIfCancellationRequested();
        var client = CreateClient(options);
        client.DeleteObject(options.Bucket, objectKey);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 复制 OSS 对象（用于标记删除重命名）
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <param name="sourceObjectKey">源对象键</param>
    /// <param name="destObjectKey">目标对象键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public static Task CopyObjectAsync(
        TaktOssOptions options,
        string sourceObjectKey,
        string destObjectKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceObjectKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(destObjectKey);
        cancellationToken.ThrowIfCancellationRequested();
        var client = CreateClient(options);
        var request = new CopyObjectRequest(options.Bucket, sourceObjectKey, options.Bucket, destObjectKey);
        client.CopyObject(request);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 判断 OSS 对象是否存在
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <param name="objectKey">对象键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否存在</returns>
    public static Task<bool> ObjectExistsAsync(
        TaktOssOptions options,
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(objectKey);
        cancellationToken.ThrowIfCancellationRequested();
        var client = CreateClient(options);
        return Task.FromResult(client.DoesObjectExist(options.Bucket, objectKey));
    }

    /// <summary>
    /// 构建 OSS 对象公开访问 URL（虚拟托管风格）
    /// </summary>
    /// <param name="options">OSS 配置</param>
    /// <param name="objectKey">对象键</param>
    /// <returns>HTTPS 访问 URL</returns>
    public static string BuildPublicObjectUrl(TaktOssOptions options, string objectKey)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(objectKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.Bucket);
        var endpoint = options.Endpoint.Trim().Replace("https://", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace("http://", string.Empty, StringComparison.OrdinalIgnoreCase)
            .TrimEnd('/');
        var key = objectKey.Replace('\\', '/').TrimStart('/');
        return $"https://{options.Bucket}.{endpoint}/{key}";
    }

    /// <summary>
    /// 从 IConfiguration 上传本地文件至 OSS
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="provider">OSS 提供商标识</param>
    /// <param name="localFilePath">本地绝对路径</param>
    /// <param name="objectKey">对象键</param>
    /// <param name="contentType">MIME</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public static async Task UploadLocalFileAsync(
        IConfiguration configuration,
        string provider,
        string localFilePath,
        string objectKey,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentException.ThrowIfNullOrWhiteSpace(localFilePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(objectKey);
        if (!File.Exists(localFilePath))
        {
            throw new FileNotFoundException($"本地文件不存在: {localFilePath}");
        }

        var options = GetOssOptionsFromConfiguration(configuration, provider);
        await using var stream = File.OpenRead(localFilePath);
        await PutObjectAsync(options, objectKey, stream, contentType, cancellationToken);
    }
}
