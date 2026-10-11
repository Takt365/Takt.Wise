// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models
// 文件名称：TaktFileChunkPlan.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：分片上传计划（由文件总大小与 FileUpload 配置计算）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models;

/// <summary>
/// 分片上传计划（总大小 + 配置 → 是否分片、分片大小、分片数）
/// </summary>
public sealed class TaktFileChunkPlan
{
    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSizeBytes { get; init; }

    /// <summary>
    /// 是否应走分片上传（超过 ChunkThresholdBytes）
    /// </summary>
    public bool UseChunkUpload { get; init; }

    /// <summary>
    /// 标准分片大小（字节）；最后一片可能更小
    /// </summary>
    public long ChunkSizeBytes { get; init; }

    /// <summary>
    /// 总分片数（≥1，且 ≤ MaxChunkCount）
    /// </summary>
    public int TotalChunks { get; init; }

    /// <summary>
    /// 单文件最大字节数（来自配置快照）
    /// </summary>
    public long MaxFileSizeBytes { get; init; }

    /// <summary>
    /// 最大分片数（来自配置快照）
    /// </summary>
    public int MaxChunkCount { get; init; }

    /// <summary>
    /// 分片阈值（字节；不超过此值可整文件上传）
    /// </summary>
    public long ChunkThresholdBytes { get; init; }

    /// <summary>
    /// 默认分片大小（字节）
    /// </summary>
    public long DefaultChunkSizeBytes { get; init; }
}
