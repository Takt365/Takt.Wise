// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models
// 文件名称：TaktFileUploadModels.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：通用文件上传/下载模型（与 frontend upload.ts 协议对齐，供各业务模块复用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Models;

/// <summary>
/// 文件存储隔离范围（租户/公司/可选业务子目录）
/// </summary>
public class TaktFileUploadScope
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 业务存储子目录（由字典/业务传入，如 avatar、images）
    /// </summary>
    public string? CategoryPath { get; set; }

    /// <summary>
    /// 上传类型（引擎路由：普通/分片/直传 OSS）
    /// </summary>
    public TaktFileUploadType FileUploadType { get; set; } = TaktFileUploadType.Normal;

    /// <summary>
    /// 目标文件名（storageNaming=2 自定义时使用；未指定时引擎自动生成 GUID 存储名）
    /// </summary>
    public string? TargetFileName { get; set; }

    /// <summary>
    /// 存储命名规则（字典 sys_storage_naming：0=原文件+哈希，1=自动生成，2=自定义）
    /// </summary>
    public int StorageNaming { get; set; } = 0;

    /// <summary>
    /// 存储方式（字典 sys_storage_type：0=本地，1=OSS，2=FTP）
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置 JSON（含 ossProvider/ftpProvider 等）
    /// </summary>
    public string? StorageConfig { get; set; }
}

/// <summary>
/// 分片存在性检查请求
/// </summary>
public class TaktFileChunkCheckRequest
{
    /// <summary>
    /// 文件唯一标识（通常为 MD5）
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 分片序号（从 1 开始）
    /// </summary>
    public int ChunkNumber { get; set; }

    /// <summary>
    /// 当前分片大小（字节）
    /// </summary>
    public long ChunkSize { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// 总分片数（可选；大于 0 时校验与服务器分片计划一致）
    /// </summary>
    public int TotalChunks { get; set; }
}

/// <summary>
/// 分片存在性检查结果
/// </summary>
public class TaktFileChunkCheckResult
{
    /// <summary>
    /// 分片是否已存在
    /// </summary>
    public bool Exists { get; set; }
}

/// <summary>
/// 已上传分片列表查询请求（断点续传批量恢复）
/// </summary>
public class TaktFileChunkListRequest
{
    /// <summary>
    /// 文件唯一标识（通常为 MD5）
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 总分片数（可选；大于 0 时过滤超出范围的分片序号）
    /// </summary>
    public int TotalChunks { get; set; }

    /// <summary>
    /// 文件总大小（字节；大于 0 时用于校验分片计划）
    /// </summary>
    public long TotalSize { get; set; }
}

/// <summary>
/// 已上传分片列表查询结果
/// </summary>
public class TaktFileChunkListResult
{
    /// <summary>
    /// 已上传的分片序号列表（从 1 开始，升序）
    /// </summary>
    public List<int> UploadedChunkNumbers { get; set; } = [];
}

/// <summary>
/// 分片上传元数据（multipart 表单字段，不含 file 流）
/// </summary>
public class TaktFileChunkUploadRequest
{
    /// <summary>
    /// 文件唯一标识
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 分片序号（从 1 开始）
    /// </summary>
    public int ChunkNumber { get; set; }

    /// <summary>
    /// 总分片数
    /// </summary>
    public int TotalChunks { get; set; }

    /// <summary>
    /// 当前分片大小（字节）
    /// </summary>
    public long ChunkSize { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;
}

/// <summary>
/// 分片合并请求（纯存储层，不含业务元数据）
/// </summary>
public class TaktFileChunkMergeRequest
{
    /// <summary>
    /// 文件唯一标识
    /// </summary>
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 总分片数
    /// </summary>
    public int TotalChunks { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSize { get; set; }
}

/// <summary>
/// 存储完成后的文件描述（字段名与 <c>TaktFile</c> 实体存储列 1:1 对齐）
/// </summary>
public class TaktStoredFileResult
{
    /// <summary>
    /// 文件编码（业务编码 FILE+时间戳+随机段）→ <c>TaktFile.FileCode</c>
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（存储文件名）→ <c>TaktFile.FileName</c>
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称 → <c>TaktFile.FileOriginalName</c>
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（相对 wwwroot）→ <c>TaktFile.FilePath</c>
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）→ <c>TaktFile.FileSize</c>
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型 → <c>TaktFile.FileType</c>
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// 文件扩展名（不含点）→ <c>TaktFile.FileExtension</c>
    /// </summary>
    public string? FileExtension { get; set; }

    /// <summary>
    /// 文件哈希（MD5）→ <c>TaktFile.FileHash</c>
    /// </summary>
    public string? FileHash { get; set; }

    /// <summary>
    /// 文件分类（上传引擎按 FileType/MIME 自动写入）→ TaktFile.FileCategory
    /// </summary>
    public int FileCategory { get; set; } = 5;

    /// <summary>
    /// 存储方式 → <c>TaktFile.StorageType</c>
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置（JSON）→ <c>TaktFile.StorageConfig</c>
    /// </summary>
    public string? StorageConfig { get; set; }

    /// <summary>
    /// 访问地址 → <c>TaktFile.AccessUrl</c>
    /// </summary>
    public string? AccessUrl { get; set; }
}

/// <summary>
/// 已存储文件的读取定位符（与 <c>TaktFile.FilePath</c> + <c>TaktFile.StorageType</c> 对齐）
/// </summary>
public class TaktFileStorageDescriptor
{
    /// <summary>
    /// 文件路径（相对 wwwroot）→ <c>TaktFile.FilePath</c>
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 存储方式 → <c>TaktFile.StorageType</c>
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置 JSON → <c>TaktFile.StorageConfig</c>
    /// </summary>
    public string? StorageConfig { get; set; }
}

/// <summary>
/// 打开文件流结果（调用方负责释放 Stream）
/// </summary>
public sealed class TaktFileDownloadStreamResult
{
    /// <summary>
    /// 可读流
    /// </summary>
    public required Stream Stream { get; init; }

    /// <summary>
    /// 建议下载文件名
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// MIME 类型
    /// </summary>
    public required string ContentType { get; init; }
}

/// <summary>
/// 文件上传策略（配置 + 可选按 totalSize 计算的分片计划）
/// </summary>
public class TaktFileUploadPolicyResult
{
    /// <summary>
    /// 单文件最大字节数
    /// </summary>
    public long MaxFileSizeBytes { get; set; }

    /// <summary>
    /// 最大分片数
    /// </summary>
    public int MaxChunkCount { get; set; }

    /// <summary>
    /// 默认分片大小（字节）
    /// </summary>
    public long DefaultChunkSizeBytes { get; set; }

    /// <summary>
    /// 分片上传阈值（字节）
    /// </summary>
    public long ChunkThresholdBytes { get; set; }

    /// <summary>
    /// 分片临时目录（相对 wwwroot）
    /// </summary>
    public string ChunkRelativePath { get; set; } = string.Empty;

    /// <summary>
    /// 允许扩展名（小写、不含点）
    /// </summary>
    public string[] AllowedExtensions { get; set; } = [];

    /// <summary>
    /// 禁止扩展名（小写、不含点）
    /// </summary>
    public string[] DeniedExtensions { get; set; } = [];

    /// <summary>
    /// 查询 totalSize 时：是否应分片上传
    /// </summary>
    public bool? UseChunkUpload { get; set; }

    /// <summary>
    /// 查询 totalSize 时：分片大小（字节）
    /// </summary>
    public long? ChunkSizeBytes { get; set; }

    /// <summary>
    /// 查询 totalSize 时：总分片数
    /// </summary>
    public int? TotalChunks { get; set; }

    /// <summary>
    /// 查询 totalSize 时：文件总大小（字节）
    /// </summary>
    public long? TotalSizeBytes { get; set; }
}
