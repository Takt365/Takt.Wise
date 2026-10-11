// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktLogReporter.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一日志远端上报（内存队列、批量 flush）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using Takt.Shared.Enums;
using Takt.Shared.Models.Logging;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 统一日志远端上报器
/// </summary>
/// <remarks>
/// 非纯工具网关：内存队列、定时器与 HttpClient 批量 HTTP 上报；由 TaktLogger.Configure 触发配置。
/// </remarks>
public static class TaktLogReporter
{
    private static readonly ConcurrentQueue<TaktLogEntry> Queue = new();
    private static readonly HttpClient HttpClient = new();
    private static readonly object SyncRoot = new();

    private static TaktLoggingOptions _options = new();
    private static Timer? _flushTimer;
    private static int _isFlushing;

    /// <summary>
    /// 远端上报队列最大条数（超出时丢弃最旧条目，防止上报失败导致内存无限增长）
    /// </summary>
    private const int MaxQueueSize = 10_000;

    /// <summary>
    /// 配置上报器
    /// </summary>
    /// <param name="options">日志配置</param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null</exception>
    public static void Configure(TaktLoggingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        lock (SyncRoot)
        {
            _options = options;
            RestartFlushTimer();
        }
    }

    /// <summary>
    /// 入队待上报日志（Warn 及以上）
    /// </summary>
    /// <param name="entry">日志条目</param>
    /// <exception cref="ArgumentNullException"><paramref name="entry"/> 为 null</exception>
    public static void Enqueue(TaktLogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        if (!_options.EnableRemoteReport || string.IsNullOrWhiteSpace(_options.RemoteReportUrl))
        {
            return;
        }

        if (!TaktLogFormatter.ShouldLogLevel(entry.Level, TaktLogLevel.Warn))
        {
            return;
        }

        Queue.Enqueue(entry);
        while (Queue.Count > MaxQueueSize && Queue.TryDequeue(out _))
        {
        }

        if (Queue.Count >= _options.BatchSize)
        {
            _ = FlushAsync();
        }
    }

    /// <summary>
    /// 立即 flush 队列
    /// </summary>
    /// <returns>异步任务</returns>
    public static async Task FlushAsync()
    {
        if (!_options.EnableRemoteReport || string.IsNullOrWhiteSpace(_options.RemoteReportUrl))
        {
            return;
        }

        if (Interlocked.CompareExchange(ref _isFlushing, 1, 0) != 0)
        {
            return;
        }

        var batch = new List<TaktLogEntry>();
        while (Queue.TryDequeue(out var entry))
        {
            batch.Add(entry);
        }

        if (batch.Count == 0)
        {
            Interlocked.Exchange(ref _isFlushing, 0);
            return;
        }

        try
        {
            var body = TaktLogFormatter.FormatReportPayload(batch);
            using var content = new StringContent(body, Encoding.UTF8, "application/json");
            using var response = await HttpClient.PostAsync(_options.RemoteReportUrl, content).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                RequeueBatch(batch);
            }
        }
        catch
        {
            RequeueBatch(batch);
        }
        finally
        {
            Interlocked.Exchange(ref _isFlushing, 0);
        }
    }

    /// <summary>
    /// 停止定时 flush
    /// </summary>
    public static void Stop()
    {
        lock (SyncRoot)
        {
            _flushTimer?.Dispose();
            _flushTimer = null;
        }
    }

    /// <summary>
    /// 上报失败时将批次重新入队（超队列上限时丢弃最旧条目）
    /// </summary>
    /// <param name="batch">待重入队的日志批次</param>
    private static void RequeueBatch(List<TaktLogEntry> batch)
    {
        foreach (var item in batch)
        {
            Queue.Enqueue(item);
        }

        while (Queue.Count > MaxQueueSize && Queue.TryDequeue(out _))
        {
        }
    }

    /// <summary>
    /// 重启定时 flush 计时器（远端上报启用且 URL 有效时）
    /// </summary>
    private static void RestartFlushTimer()
    {
        _flushTimer?.Dispose();
        _flushTimer = null;

        if (!_options.EnableRemoteReport || string.IsNullOrWhiteSpace(_options.RemoteReportUrl))
        {
            return;
        }

        _flushTimer = new Timer(
            _ => _ = FlushAsync(),
            null,
            TimeSpan.FromMilliseconds(_options.FlushIntervalMs),
            TimeSpan.FromMilliseconds(_options.FlushIntervalMs));
    }
}
