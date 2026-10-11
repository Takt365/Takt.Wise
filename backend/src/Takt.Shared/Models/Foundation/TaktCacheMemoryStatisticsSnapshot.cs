// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Foundation
// 文件名称：TaktCacheMemoryStatisticsSnapshot.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：业务 MemoryCache 运行时统计快照
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Foundation;

/// <summary>
/// 业务 MemoryCache 运行时统计快照
/// </summary>
public class TaktCacheMemoryStatisticsSnapshot
{
    /// <summary>
    /// 是否支持统计
    /// </summary>
    public bool Supported { get; set; }

    /// <summary>
    /// 说明文案（不支持或补充说明）
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 当前内存层条目数
    /// </summary>
    public long CurrentEntryCount { get; set; }

    /// <summary>
    /// 总命中次数
    /// </summary>
    public long TotalHits { get; set; }

    /// <summary>
    /// 总未命中次数
    /// </summary>
    public long TotalMisses { get; set; }

    /// <summary>
    /// 命中率（0~1）
    /// </summary>
    public double HitRate { get; set; }

    /// <summary>
    /// 估算占用字节数
    /// </summary>
    public long CurrentEstimatedSizeBytes { get; set; }
}
