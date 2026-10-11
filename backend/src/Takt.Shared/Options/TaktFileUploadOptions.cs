// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktFileUploadOptions.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传引擎配置（本地存储路径、大小上限、分片上限）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 文件上传配置（<c>appsettings:FileUpload</c> 覆盖本类默认值）
/// </summary>
public class TaktFileUploadOptions
{
    /// <summary>
    /// appsettings 配置节名称
    /// </summary>
    public const string SectionName = "FileUpload";

    /// <summary>
    /// 相对 wwwroot 的正式文件目录
    /// </summary>
    public string UploadRelativePath { get; set; } = "uploads";

    /// <summary>
    /// 本地正式文件存储根目录（绝对路径或相对 ContentRoot；空则默认 wwwroot）
    /// </summary>
    public string? UploadStorageRootPath { get; set; }

    /// <summary>
    /// 分片临时根目录（绝对路径或相对 ContentRoot；空则默认 wwwroot）
    /// </summary>
    public string? ChunkStorageRootPath { get; set; }

    /// <summary>
    /// 相对 wwwroot 的分片临时目录
    /// </summary>
    public string ChunkRelativePath { get; set; } = "uploads/_chunks";

    /// <summary>
    /// 单文件最大字节数（默认 500MB）
    /// </summary>
    public long MaxFileSizeBytes { get; set; } = 524_288_000;

    /// <summary>
    /// 单文件最大分片数（防滥用；超大文件时自动放大分片大小以满足此上限）
    /// </summary>
    public int MaxChunkCount { get; set; } = 10_000;

    /// <summary>
    /// 默认分片大小（字节，默认 2MB）
    /// </summary>
    public long DefaultChunkSizeBytes { get; set; } = 2_097_152;

    /// <summary>
    /// 分片上传阈值（字节，默认 5MB；不超过此值可整文件上传）
    /// </summary>
    public long ChunkThresholdBytes { get; set; } = 5_242_880;

    /// <summary>
    /// 允许上传的扩展名（小写、不含点；空数组表示不限制）
    /// </summary>
    public string[] AllowedExtensions { get; set; } = [];

    /// <summary>
    /// 禁止上传的扩展名（小写、不含点；在 AllowedExtensions 校验之前执行）
    /// </summary>
    public string[] DeniedExtensions { get; set; } = ["doc", "xls", "ppt"];
}
