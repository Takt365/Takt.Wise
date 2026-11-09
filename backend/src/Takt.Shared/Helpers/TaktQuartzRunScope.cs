// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktQuartzRunScope.cs
// 创建时间：2026-11-03
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 本次执行的源库、目标库、核算月（日志与所选执行参数一致）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 当前 Quartz 执行的源库、目标库与核算月。仅在任务执行异步流内有效。
/// </summary>
public static class TaktQuartzRunScope
{
    private static readonly AsyncLocal<State?> Ambient = new();

    /// <summary>
    /// 源库名；未选则为 null
    /// </summary>
    public static string? SourceDatabase => Ambient.Value?.SourceDatabase;

    /// <summary>
    /// 目标库名；未选则为 null
    /// </summary>
    public static string? TargetDatabase => Ambient.Value?.TargetDatabase;

    /// <summary>
    /// 核算月 yyyy-MM；未选则为 null
    /// </summary>
    public static string? CostingPeriod => Ambient.Value?.CostingPeriod;

    /// <summary>
    /// 进入本次执行范围。Dispose 后恢复外层范围。
    /// </summary>
    /// <param name="sourceDatabase">源库</param>
    /// <param name="targetDatabase">目标库</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <returns>范围句柄</returns>
    public static IDisposable Begin(string? sourceDatabase, string? targetDatabase, string? costingPeriod)
    {
        var previous = Ambient.Value;
        Ambient.Value = new State(
            Normalize(sourceDatabase),
            Normalize(targetDatabase),
            Normalize(costingPeriod));
        return new PopScope(previous);
    }

    /// <summary>
    /// 日志模板尚未写明源库/目标库时，在前面补上本次选中的库和核算月
    /// </summary>
    /// <param name="messageTemplate">原消息模板</param>
    /// <returns>补全后的模板</returns>
    public static string PrefixMessage(string messageTemplate)
    {
        var state = Ambient.Value;
        if (state == null || string.IsNullOrWhiteSpace(messageTemplate))
        {
            return messageTemplate;
        }
        if (messageTemplate.Contains("Target=", StringComparison.Ordinal)
            || messageTemplate.Contains("Source=", StringComparison.Ordinal))
        {
            return messageTemplate;
        }
        var parts = new List<string>(3);
        if (!string.IsNullOrEmpty(state.SourceDatabase))
        {
            parts.Add("Source=" + state.SourceDatabase);
        }
        if (!string.IsNullOrEmpty(state.TargetDatabase))
        {
            parts.Add("Target=" + state.TargetDatabase);
        }
        var hasPeriod = messageTemplate.Contains("Period=", StringComparison.Ordinal)
            || messageTemplate.Contains("Month=", StringComparison.Ordinal);
        if (!hasPeriod && !string.IsNullOrEmpty(state.CostingPeriod))
        {
            parts.Add("Period=" + state.CostingPeriod);
        }
        if (parts.Count == 0)
        {
            return messageTemplate;
        }
        return string.Join(' ', parts) + " " + messageTemplate;
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private sealed class State
    {
        public State(string? sourceDatabase, string? targetDatabase, string? costingPeriod)
        {
            SourceDatabase = sourceDatabase;
            TargetDatabase = targetDatabase;
            CostingPeriod = costingPeriod;
        }

        public string? SourceDatabase { get; }

        public string? TargetDatabase { get; }

        public string? CostingPeriod { get; }
    }

    private sealed class PopScope : IDisposable
    {
        private readonly State? _previous;
        private bool _disposed;

        public PopScope(State? previous)
        {
            _previous = previous;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            Ambient.Value = _previous;
        }
    }
}
