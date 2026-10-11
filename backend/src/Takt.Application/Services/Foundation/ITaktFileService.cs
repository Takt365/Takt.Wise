// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktFileService.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：文件应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 文件应用服务接口
/// </summary>
public interface ITaktFileService
{
    /// <summary>
    /// 获取文件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktFileDto>> GetFileListAsync(TaktFileQueryDto queryDto);

    /// <summary>
    /// 根据ID获取文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>DTO</returns>
    Task<TaktFileDto?> GetFileByIdAsync(long id);

    /// <summary>
    /// 按文件ID下载物理文件流（校验租户/公司、状态与公开范围，并更新下载统计）
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流与下载文件名</returns>
    Task<TaktFileDownloadStreamResult> DownloadFileByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 按访问地址打开物理文件流（租户/公司内匹配 AccessUrl，再走按 ID 下载）
    /// </summary>
    /// <param name="accessUrl">TaktFile.AccessUrl</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流与下载文件名</returns>
    Task<TaktFileDownloadStreamResult> DownloadFileByAccessUrlAsync(string accessUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取文件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetFileOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFileDto> CreateFileAsync(TaktFileCreateDto dto);

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFileDto> UpdateFileAsync(long id, TaktFileUpdateDto dto);

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>任务</returns>
    Task DeleteFileByIdAsync(long id);

    /// <summary>
    /// 批量删除文件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteFileBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新文件状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFileDto> UpdateFileStatusAsync(TaktFileStatusDto dto);

    /// <summary>
    /// 更新文件公开
    /// </summary>
    /// <param name="dto">公开范围 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktFileDto> UpdateFilePublicAsync(TaktFilePublicDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetFileTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入文件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportFileAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出文件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportFileAsync(TaktFileQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取上传策略（可选 totalSizeBytes 计算分片计划）
    /// </summary>
    /// <param name="totalSizeBytes">文件总大小（字节）</param>
    /// <returns>上传策略</returns>
    TaktFileUploadPolicyResult GetFileUploadPolicy(long? totalSizeBytes = null);

    /// <summary>
    /// 整文件上传并落库
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="meta">业务元数据</param>
    /// <param name="clientIp">上传来源客户端 IP（由 WebApi 经 TaktLocationHelper.ResolveClientIp 解析）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>上传结果</returns>
    Task<TaktFileUploadResultDto> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadMetaDto? meta = null,
        string? clientIp = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查分片是否已上传
    /// </summary>
    /// <param name="request">检查参数</param>
    /// <returns>是否存在</returns>
    Task<TaktFileChunkCheckResult> CheckFileChunkAsync(TaktFileChunkCheckRequest request);

    /// <summary>
    /// 列出已上传分片序号
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <returns>已上传分片序号</returns>
    Task<TaktFileChunkListResult> ListFileChunksAsync(TaktFileChunkListRequest request);

    /// <summary>
    /// 上传单个分片
    /// </summary>
    /// <param name="chunkStream">分片流</param>
    /// <param name="request">分片元数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task UploadFileChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 合并分片并落库
    /// </summary>
    /// <param name="dto">合并参数</param>
    /// <param name="clientIp">上传来源客户端 IP（由 WebApi 经 TaktLocationHelper.ResolveClientIp 解析）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>上传结果</returns>
    Task<TaktFileUploadResultDto> MergeFileChunksAsync(
        TaktFileChunkMergeDto dto,
        string? clientIp = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 取消分片上传并清理临时分片
    /// </summary>
    /// <param name="identifier">上传会话标识</param>
    Task CancelFileChunksAsync(string identifier);
}