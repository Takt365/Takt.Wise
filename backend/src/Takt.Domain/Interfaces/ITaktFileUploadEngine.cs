// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktFileUploadEngine.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：通用文件上传下载引擎（整文件/分片/合并/读流/删物理文件；与业务元数据解耦）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 通用文件上传下载引擎。
/// 负责物理存储 I/O（本地分片、合并、读流）；业务元数据落库由 TaktFileService 编排。
/// </summary>
public interface ITaktFileUploadEngine
{
    /// <summary>
    /// 整文件上传
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="scope">隔离范围；为空时从 ITaktUserContext 解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存储结果</returns>
    Task<TaktStoredFileResult> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取上传策略（含 MaxChunkCount、ChunkRelativePath；可选按 totalSize 返回分片计划）
    /// </summary>
    /// <param name="totalSizeBytes">文件总大小；为空时仅返回全局配置</param>
    /// <returns>上传策略</returns>
    TaktFileUploadPolicyResult GetUploadPolicy(long? totalSizeBytes = null);

    /// <summary>
    /// 检查分片是否已上传（断点续传）
    /// </summary>
    /// <param name="request">检查参数</param>
    /// <param name="scope">隔离范围</param>
    /// <returns>是否存在</returns>
    Task<TaktFileChunkCheckResult> CheckChunkAsync(
        TaktFileChunkCheckRequest request,
        TaktFileUploadScope? scope = null);

    /// <summary>
    /// 列出指定 identifier 已上传的分片序号（断点续传）
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <param name="scope">隔离范围</param>
    /// <returns>已上传分片序号</returns>
    Task<TaktFileChunkListResult> ListUploadedChunksAsync(
        TaktFileChunkListRequest request,
        TaktFileUploadScope? scope = null);

    /// <summary>
    /// 取消分片上传并清理临时目录
    /// </summary>
    /// <param name="identifier">上传会话标识</param>
    /// <param name="scope">隔离范围</param>
    Task CancelUploadedChunksAsync(
        string identifier,
        TaktFileUploadScope? scope = null);

    /// <summary>
    /// 上传单个分片
    /// </summary>
    /// <param name="chunkStream">分片流</param>
    /// <param name="request">分片元数据</param>
    /// <param name="scope">隔离范围</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UploadChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 合并分片为完整文件
    /// </summary>
    /// <param name="request">合并参数</param>
    /// <param name="scope">隔离范围</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存储结果</returns>
    Task<TaktStoredFileResult> MergeChunksAsync(
        TaktFileChunkMergeRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 按存储描述符打开只读流
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="downloadFileName">下载文件名；为空时使用路径文件名</param>
    /// <param name="contentType">MIME；为空时按扩展名推断</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>流与内容类型</returns>
    Task<TaktFileDownloadStreamResult> OpenReadAsync(
        TaktFileStorageDescriptor descriptor,
        string? downloadFileName = null,
        string? contentType = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除物理文件（本地存储）
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task DeleteStoredFileAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 将本地物理文件重命名为带删除标记的文件名（xxx.ext → xxx.del.ext），并返回新相对路径
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>重命名后的相对路径；已标记或源文件不存在时返回原路径</returns>
    Task<string> MarkStoredFileDeletedAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken = default);
}
