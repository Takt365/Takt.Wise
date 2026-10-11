// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Middleware
// 文件名称：TaktXssProtectionMiddleware.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：XSS 防护中间件（上传扩展名校验、JSON/查询字符串可疑内容检测）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Takt.Infrastructure.Extensions;
using Takt.Shared.Enums;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Middleware;

/// <summary>
/// XSS 防护中间件
/// </summary>
public partial class TaktXssProtectionMiddleware
{
    /// <summary>
    /// 请求体 XSS 扫描最大字节数（超出部分不扫描）
    /// </summary>
    private const int MaxBodyScanBytes = 1024 * 1024;

    /// <summary>
    /// 可疑 XSS 片段关键字列表（大小写不敏感子串匹配）
    /// </summary>
    private static readonly string[] SuspiciousPatterns =
    [
        "<script",
        "javascript:",
        "onerror=",
        "onload=",
        "eval(",
        "expression(",
    ];

    /// <summary>
    /// 管道中的下一个中间件委托
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">下一个中间件</param>
    public TaktXssProtectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 执行中间件
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="securityOptions">安全配置</param>
    /// <returns>异步任务</returns>
    public async Task InvokeAsync(HttpContext context, IOptions<TaktSecurityOptions> securityOptions)
    {
        var options = securityOptions.Value;

        if (!options.XssProtection.Enabled || ShouldSkipXssScan(context))
        {
            await _next(context);
            return;
        }

        if (!ValidateUploadedFileExtensions(context, options.XssProtection.AllowedFileExtensions))
        {
            await WriteJsonErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                "XSS_FILE_EXTENSION_DENIED",
                "不允许上传该类型的文件",
                TaktResultCode.BadRequest);
            return;
        }

        if (!await ValidateRequestPayloadAsync(context))
        {
            await WriteJsonErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                "XSS_SUSPICIOUS_CONTENT",
                "请求内容包含可疑脚本片段",
                TaktResultCode.BadRequest);
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// 是否跳过 XSS 扫描（OAuth、SignalR、健康检查等路径）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>应跳过时为 true</returns>
    private static bool ShouldSkipXssScan(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (path.StartsWith("/connect", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/hubs", StringComparison.OrdinalIgnoreCase)
            || TaktObservabilityCollectionExtensions.IsObservabilityProbePath(context.Request.Path))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 校验 multipart 表单上传文件扩展名是否在白名单内
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="allowedExtensions">允许的扩展名列表（不含点）</param>
    /// <returns>全部合法或未上传文件时为 true</returns>
    private static bool ValidateUploadedFileExtensions(HttpContext context, IReadOnlyList<string> allowedExtensions)
    {
        if (!context.Request.HasFormContentType)
        {
            return true;
        }

        if (!context.Request.Form.Files.Any())
        {
            return true;
        }

        if (allowedExtensions.Count == 0)
        {
            return true;
        }

        var allowed = new HashSet<string>(
            allowedExtensions.Select(NormalizeExtension),
            StringComparer.OrdinalIgnoreCase);

        foreach (var file in context.Request.Form.Files)
        {
            var ext = NormalizeExtension(Path.GetExtension(file.FileName));
            if (string.IsNullOrEmpty(ext) || !allowed.Contains(ext))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 校验查询字符串与请求体是否包含可疑 XSS 片段
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>通过校验为 true</returns>
    private static async Task<bool> ValidateRequestPayloadAsync(HttpContext context)
    {
        if (!ValidateQueryString(context.Request.QueryString))
        {
            return false;
        }

        if (!HasRequestBody(context))
        {
            return true;
        }

        var contentType = context.Request.ContentType ?? string.Empty;
        if (!contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
            && !contentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)
            && !contentType.Contains("text/", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        context.Request.EnableBuffering();

        using var reader = new StreamReader(
            context.Request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 4096,
            leaveOpen: true);

        var buffer = new char[MaxBodyScanBytes];
        var readCount = await reader.ReadBlockAsync(buffer, 0, MaxBodyScanBytes);
        context.Request.Body.Position = 0;

        if (readCount <= 0)
        {
            return true;
        }

        var bodySample = new string(buffer, 0, readCount);
        return !ContainsSuspiciousContent(bodySample);
    }

    /// <summary>
    /// 校验查询字符串：参数名仅检测 script 等关键字；参数值做完整 XSS 检测（避免 onlyX= 等键名误触 on\w+= 规则）
    /// </summary>
    /// <param name="queryString">查询字符串</param>
    /// <returns>通过校验为 true</returns>
    private static bool ValidateQueryString(QueryString queryString)
    {
        if (!queryString.HasValue || string.IsNullOrWhiteSpace(queryString.Value))
        {
            return true;
        }
        var raw = queryString.Value!.TrimStart('?');
        if (string.IsNullOrWhiteSpace(raw))
        {
            return true;
        }
        foreach (var segment in raw.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var eqIndex = segment.IndexOf('=');
            var name = eqIndex >= 0 ? segment[..eqIndex] : segment;
            var value = eqIndex >= 0 ? segment[(eqIndex + 1)..] : string.Empty;
            try
            {
                name = Uri.UnescapeDataString(name.Replace('+', ' '));
                value = Uri.UnescapeDataString(value.Replace('+', ' '));
            }
            catch (UriFormatException)
            {
                return false;
            }
            if (ContainsSuspiciousKeyword(name) || ContainsSuspiciousContent(value))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 是否包含可疑 XSS 关键字（不含 HTML 事件 on*= 正则，供查询参数名等短标识检测）
    /// </summary>
    /// <param name="value">待检测文本</param>
    /// <returns>包含可疑内容时为 true</returns>
    private static bool ContainsSuspiciousKeyword(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        foreach (var pattern in SuspiciousPatterns)
        {
            if (value.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否包含可疑 XSS 内容（关键字子串或 HTML 事件处理器属性）
    /// </summary>
    /// <param name="value">待检测文本</param>
    /// <returns>包含可疑内容时为 true</returns>
    private static bool ContainsSuspiciousContent(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        foreach (var pattern in SuspiciousPatterns)
        {
            if (value.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return HtmlEventHandlerRegex().IsMatch(value);
    }

    /// <summary>
    /// 规范化文件扩展名（不含点、小写）
    /// </summary>
    /// <param name="extension">原始扩展名（可含前导点）</param>
    /// <returns>规范化后的扩展名；空输入返回空字符串</returns>
    private static string NormalizeExtension(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return string.Empty;
        }

        return extension.Trim().TrimStart('.').ToLowerInvariant();
    }

    /// <summary>
    /// 当前请求方法是否可能携带请求体
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>POST/PUT/PATCH 时为 true</returns>
    private static bool HasRequestBody(HttpContext context)
    {
        return HttpMethods.IsPost(context.Request.Method)
            || HttpMethods.IsPut(context.Request.Method)
            || HttpMethods.IsPatch(context.Request.Method);
    }

    /// <summary>
    /// 写入统一 JSON 错误响应
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="statusCode">HTTP 状态码</param>
    /// <param name="errorCode">业务错误码</param>
    /// <param name="message">错误消息</param>
    /// <param name="resultCode">结果枚举码</param>
    /// <returns>异步任务</returns>
    private static async Task WriteJsonErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string errorCode,
        string message,
        TaktResultCode resultCode)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var result = TaktApiResult<object>.Fail(message, resultCode);
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }

    /// <summary>
    /// HTML 内联事件处理器属性正则（如 onclick=、onerror=）
    /// </summary>
    /// <returns>编译后的正则实例</returns>
    [GeneratedRegex(@"on\w+\s*=", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex HtmlEventHandlerRegex();
}
