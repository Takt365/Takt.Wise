// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktLogger.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 统一日志入口（统一采集、统一格式化、统一上报）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;
using Takt.Shared.Models.Logging;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// Takt 统一日志帮助类
/// </summary>
/// <remarks>
/// 非纯工具网关：启动时 Configure 注入 TaktLoggingOptions 并委托远端上报；
/// 写日志为 I/O 副作用，级别过滤依赖已配置选项。
/// </remarks>
public static class TaktLogger
{
    private static TaktLoggingOptions _options = new();

    /// <summary>
    /// 配置统一日志（应用启动时调用）
    /// </summary>
    /// <param name="options">日志配置</param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null</exception>
    public static void Configure(TaktLoggingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
        TaktLogReporter.Configure(options);
    }

    /// <summary>
    /// 创建带模块前缀的子日志上下文
    /// </summary>
    /// <param name="moduleName">模块名</param>
    /// <param name="context">附加上下文</param>
    /// <returns>合并后的上下文</returns>
    public static TaktLogContext CreateModuleContext(string moduleName, TaktLogContext? context = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(moduleName);
        return new TaktLogContext
        {
            Module = moduleName,
            Action = context?.Action,
            UserId = context?.UserId,
            UserName = context?.UserName,
            TenantCode = context?.TenantCode,
            CompanyCode = context?.CompanyCode,
            Route = context?.Route,
            RequestId = context?.RequestId,
            TraceId = context?.TraceId,
            ClientIp = context?.ClientIp,
            Extra = context?.Extra
        };
    }

    /// <summary>
    /// 开启日志作用域（自动写入 Serilog LogContext）
    /// </summary>
    /// <param name="context">业务上下文</param>
    /// <returns>可释放的作用域</returns>
    public static IDisposable BeginScope(TaktLogContext? context)
    {
        return PushProperties(TaktLogFormatter.ToPropertyDictionary(context));
    }

    /// <summary>
    /// 记录详细调试信息（Verbose 级别）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Verbose(string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Verbose, null, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录详细调试信息（Verbose 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Verbose(Exception exception, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Verbose, exception, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录调试信息（Debug 级别）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Debug(string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Debug, null, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录调试信息（Debug 级别）
    /// </summary>
    /// <param name="context">业务上下文</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Debug(TaktLogContext context, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Debug, null, messageTemplate, context, propertyValues);
    }

    /// <summary>
    /// 记录调试信息（Debug 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Debug(Exception exception, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Debug, exception, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录信息（Information 级别）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Information(string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Information, null, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录信息（Information 级别）
    /// </summary>
    /// <param name="context">业务上下文</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Information(TaktLogContext context, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Information, null, messageTemplate, context, propertyValues);
    }

    /// <summary>
    /// 记录信息（Information 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Information(Exception exception, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Information, exception, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录警告（Warning 级别）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Warning(string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Warning, null, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录警告（Warning 级别）
    /// </summary>
    /// <param name="context">业务上下文</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Warning(TaktLogContext context, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Warning, null, messageTemplate, context, propertyValues);
    }

    /// <summary>
    /// 记录警告（Warning 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Warning(Exception exception, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Warning, exception, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录错误（Error 级别）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Error(string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Error, null, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录错误（Error 级别）
    /// </summary>
    /// <param name="context">业务上下文</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Error(TaktLogContext context, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Error, null, messageTemplate, context, propertyValues);
    }

    /// <summary>
    /// 记录错误（Error 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Error(Exception exception, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Error, exception, messageTemplate, null, propertyValues);
    }

    /// <summary>
    /// 记录错误（Error 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="context">业务上下文</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Error(Exception exception, TaktLogContext context, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Error, exception, messageTemplate, context, propertyValues);
    }

    /// <summary>
    /// 记录致命错误（Fatal 级别）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Fatal(string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Fatal, null, messageTemplate, null, propertyValues, flushReport: true);
    }

    /// <summary>
    /// 记录致命错误（Fatal 级别）
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    public static void Fatal(Exception exception, string messageTemplate, params object?[]? propertyValues)
    {
        Write(LogEventLevel.Fatal, exception, messageTemplate, null, propertyValues, flushReport: true);
    }

    /// <summary>
    /// 使用属性记录日志
    /// </summary>
    /// <param name="propertyName">属性名</param>
    /// <param name="value">属性值</param>
    /// <returns>可释放的上下文</returns>
    public static IDisposable PushProperty(string propertyName, object? value)
    {
        return LogContext.PushProperty(propertyName, value);
    }

    /// <summary>
    /// 使用多个属性记录日志
    /// </summary>
    /// <param name="properties">属性字典</param>
    /// <returns>可释放的上下文</returns>
    public static IDisposable PushProperties(Dictionary<string, object?> properties)
    {
        if (properties == null || properties.Count == 0)
        {
            return new EmptyDisposable();
        }

        IDisposable? result = null;
        foreach (var property in properties)
        {
            result = result == null
                ? LogContext.PushProperty(property.Key, property.Value)
                : new CompositeDisposable(result, LogContext.PushProperty(property.Key, property.Value));
        }

        return result ?? new EmptyDisposable();
    }

    /// <summary>
    /// 检查是否启用指定级别
    /// </summary>
    /// <param name="level">日志级别</param>
    /// <returns>是否启用</returns>
    public static bool IsEnabled(LogEventLevel level)
    {
        return Log.IsEnabled(level);
    }

    /// <summary>
    /// 立即 flush 远端上报队列
    /// </summary>
    public static Task FlushReportAsync()
    {
        return TaktLogReporter.FlushAsync();
    }

    /// <summary>
    /// 统一写日志：Serilog 输出 + 远端上报队列入队
    /// </summary>
    /// <param name="level">Serilog 日志级别</param>
    /// <param name="exception">关联异常；无则 null</param>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="context">业务上下文</param>
    /// <param name="propertyValues">模板占位参数</param>
    /// <param name="flushReport">是否立即 flush 上报队列</param>
    private static void Write(
        LogEventLevel level,
        Exception? exception,
        string messageTemplate,
        TaktLogContext? context,
        object?[]? propertyValues,
        bool flushReport = false)
    {
        var taktLevel = TaktLogFormatter.FromSerilogLevel(level);
        if (!TaktLogFormatter.ShouldLogLevel(taktLevel, _options.MinLevel))
        {
            return;
        }

        using var scope = BeginScope(context);
        var values = propertyValues ?? Array.Empty<object?>();

        if (exception == null)
        {
            Log.Write(level, messageTemplate, values);
        }
        else
        {
            Log.Write(level, exception, messageTemplate, values);
        }

        var entry = TaktLogFormatter.BuildLogEntry(
            taktLevel,
            messageTemplate,
            _options,
            context,
            exception);

        TaktLogReporter.Enqueue(entry);

        if (flushReport)
        {
            _ = TaktLogReporter.FlushAsync();
        }
    }

    /// <summary>
    /// 复合释放器
    /// </summary>
    private sealed class CompositeDisposable : IDisposable
    {
        private readonly IDisposable _first;
        private readonly IDisposable _second;

        public CompositeDisposable(IDisposable first, IDisposable second)
        {
            _first = first;
            _second = second;
        }

        /// <summary>
        /// 依次释放两个 IDisposable 资源
        /// </summary>
        public void Dispose()
        {
            _second.Dispose();
            _first.Dispose();
        }
    }

    /// <summary>
    /// 空释放器
    /// </summary>
    private sealed class EmptyDisposable : IDisposable
    {
        /// <summary>
        /// 空释放（占位，无资源需释放）
        /// </summary>
        public void Dispose()
        {
        }
    }
}
