// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktLogFormatter.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一日志格式化（Serilog 模板、标准条目、上下文属性）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json.Serialization;
using Takt.Shared.Enums;
using Takt.Shared.Models.Logging;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 统一日志格式化器
/// </summary>
/// <remarks>无状态；<c>JsonSettings</c> 为只读序列化配置。</remarks>
public static class TaktLogFormatter
{
    /// <summary>
    /// 控制台统一输出模板（不含 LogContext 占位括号，避免无上下文时输出 [/] []）
    /// </summary>
    public const string DefaultConsoleOutputTemplate =
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 文件统一输出模板（结构化上下文由 TaktLogReporter 上报，不写入固定括号占位）
    /// </summary>
    public const string DefaultFileOutputTemplate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 批量上报 JSON 序列化配置（Newtonsoft.Json；camelCase、无缩进）
    /// </summary>
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.None
    };

    /// <summary>
    /// HTTP 请求日志正文前缀（仅输出非空 rid/tenant/company/user，避免 Serilog 模板空括号）
    /// </summary>
    /// <param name="context">请求级日志上下文</param>
    /// <returns>如 [rid=abc12345 tenant=000 company=1000 user=admin] ；无字段时返回空字符串</returns>
    public static string FormatHttpRequestPrefix(TaktLogContext? context)
    {
        if (context == null)
        {
            return string.Empty;
        }

        var parts = new List<string>(4);
        if (!string.IsNullOrWhiteSpace(context.RequestId))
        {
            parts.Add($"rid={context.RequestId}");
        }
        if (!string.IsNullOrWhiteSpace(context.TraceId))
        {
            parts.Add($"tid={context.TraceId}");
        }
        if (!string.IsNullOrWhiteSpace(context.TenantCode))
        {
            parts.Add($"tenant={context.TenantCode}");
        }
        if (!string.IsNullOrWhiteSpace(context.CompanyCode))
        {
            parts.Add($"company={context.CompanyCode}");
        }
        if (!string.IsNullOrWhiteSpace(context.UserName))
        {
            parts.Add($"user={context.UserName}");
        }
        else if (!string.IsNullOrWhiteSpace(context.UserId))
        {
            parts.Add($"uid={context.UserId}");
        }

        return parts.Count == 0 ? string.Empty : $"[{string.Join(' ', parts)}] ";
    }

    /// <summary>
    /// 将 TaktLogLevel 映射为 Serilog LogEventLevel
    /// </summary>
    /// <param name="level">Takt 日志级别</param>
    /// <returns>Serilog 日志级别</returns>
    public static LogEventLevel ToSerilogLevel(TaktLogLevel level)
    {
        return level switch
        {
            TaktLogLevel.Debug => LogEventLevel.Debug,
            TaktLogLevel.Info => LogEventLevel.Information,
            TaktLogLevel.Warn => LogEventLevel.Warning,
            TaktLogLevel.Error => LogEventLevel.Error,
            TaktLogLevel.Fatal => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }

    /// <summary>
    /// 将 Serilog LogEventLevel 映射为 TaktLogLevel
    /// </summary>
    /// <param name="level">Serilog 日志级别</param>
    /// <returns>Takt 日志级别</returns>
    public static TaktLogLevel FromSerilogLevel(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => TaktLogLevel.Debug,
            LogEventLevel.Debug => TaktLogLevel.Debug,
            LogEventLevel.Information => TaktLogLevel.Info,
            LogEventLevel.Warning => TaktLogLevel.Warn,
            LogEventLevel.Error => TaktLogLevel.Error,
            LogEventLevel.Fatal => TaktLogLevel.Fatal,
            _ => TaktLogLevel.Info
        };
    }

    /// <summary>
    /// 是否应输出指定级别
    /// </summary>
    /// <param name="level">当前级别</param>
    /// <param name="minLevel">最低级别</param>
    /// <returns>是否输出</returns>
    public static bool ShouldLogLevel(TaktLogLevel level, TaktLogLevel minLevel)
    {
        return level >= minLevel;
    }

    /// <summary>
    /// 序列化异常
    /// </summary>
    /// <param name="exception">异常</param>
    /// <returns>错误信息</returns>
    public static TaktLogErrorInfo? SerializeException(Exception? exception)
    {
        if (exception == null)
        {
            return null;
        }

        return new TaktLogErrorInfo
        {
            Name = exception.GetType().Name,
            Message = exception.Message,
            Stack = exception.StackTrace
        };
    }

    /// <summary>
    /// 构建标准日志条目
    /// </summary>
    /// <param name="level">级别</param>
    /// <param name="message">消息</param>
    /// <param name="options">日志配置</param>
    /// <param name="context">业务上下文</param>
    /// <param name="exception">异常</param>
    /// <param name="tags">标签</param>
    /// <returns>标准日志条目</returns>
    public static TaktLogEntry BuildLogEntry(
        TaktLogLevel level,
        string message,
        TaktLoggingOptions options,
        TaktLogContext? context = null,
        Exception? exception = null,
        IEnumerable<string>? tags = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentNullException.ThrowIfNull(options);
        return new TaktLogEntry
        {
            Level = level,
            Message = message,
            Timestamp = DateTime.UtcNow.ToString("O"),
            AppName = options.AppName,
            AppVersion = options.AppVersion,
            Environment = options.Environment,
            MachineName = Environment.MachineName,
            Context = context,
            Error = SerializeException(exception),
            Tags = tags?.ToList()
        };
    }

    /// <summary>
    /// 将业务上下文转为 Serilog LogContext 属性字典
    /// </summary>
    /// <param name="context">业务上下文</param>
    /// <returns>属性字典</returns>
    public static Dictionary<string, object?> ToPropertyDictionary(TaktLogContext? context)
    {
        var properties = new Dictionary<string, object?>();
        if (context == null)
        {
            return properties;
        }

        AddIfNotEmpty(properties, "Module", context.Module);
        AddIfNotEmpty(properties, "Action", context.Action);
        AddIfNotEmpty(properties, "UserId", context.UserId);
        AddIfNotEmpty(properties, "user_name", context.UserName);
        AddIfNotEmpty(properties, "TenantCode", context.TenantCode);
        AddIfNotEmpty(properties, "CompanyCode", context.CompanyCode);
        AddIfNotEmpty(properties, "Route", context.Route);
        AddIfNotEmpty(properties, "RequestId", context.RequestId);
        AddIfNotEmpty(properties, "TraceId", context.TraceId);
        AddIfNotEmpty(properties, "ClientIp", context.ClientIp);

        if (context.Extra == null)
        {
            return properties;
        }

        foreach (var pair in context.Extra)
        {
            properties[pair.Key] = pair.Value;
        }

        return properties;
    }

    /// <summary>
    /// 格式化批量上报 JSON
    /// </summary>
    /// <param name="entries">日志条目</param>
    /// <returns>JSON 字符串</returns>
    public static string FormatReportPayload(IReadOnlyList<TaktLogEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);
        var payload = new TaktLogReportPayload
        {
            BatchId = Guid.NewGuid().ToString("N"),
            ReportedAt = DateTime.UtcNow.ToString("O"),
            Entries = entries.ToList()
        };

        return JsonConvert.SerializeObject(payload, JsonSettings);
    }

    /// <summary>
    /// 采样列表（用于结构化日志 Detail，避免输出过长）
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="items">原始列表</param>
    /// <param name="maxSample">最大条数</param>
    /// <returns>采样结果与总数</returns>
    public static (IReadOnlyList<T> Sample, int Total) SampleForLog<T>(
        IReadOnlyList<T> items,
        int maxSample = 40) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(items);
        if (maxSample <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxSample), maxSample, "maxSample 必须大于 0");
        }
        if (items.Count == 0)
        {
            return (Array.Empty<T>(), 0);
        }

        return (items.Take(maxSample).ToList(), items.Count);
    }

    /// <summary>
    /// 将非空字符串写入 Serilog 属性字典（跳过 null/空白，避免输出空占位）
    /// </summary>
    /// <param name="properties">目标属性字典</param>
    /// <param name="key">属性键</param>
    /// <param name="value">属性值；为空或仅空白时不写入</param>
    private static void AddIfNotEmpty(Dictionary<string, object?> properties, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            properties[key] = value;
        }
    }
}
