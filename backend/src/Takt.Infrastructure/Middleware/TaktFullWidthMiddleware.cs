// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Middleware
// 文件名称：TaktFullWidthMiddleware.cs
// 创建时间：2026-10-12
// 创建人：Takt365(Cursor AI)
// 功能描述：创建与更新请求过滤全角字符（POST 创建、POST 导入、PUT 更新、PUT 状态/排序）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Buffers;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Takt.Domain.Interfaces;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Infrastructure.Middleware;

/// <summary>
/// 创建与更新请求禁止全角字符，含导入与改状态。
/// </summary>
public sealed class TaktFullWidthMiddleware
{
    private const int MaxJsonScanBytes = 32 * 1024 * 1024;
    private readonly RequestDelegate _next;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">下一个中间件</param>
    public TaktFullWidthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 写请求体含全角字符时直接返回 400
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="localizationService">校验文案</param>
    /// <returns>任务</returns>
    public async Task InvokeAsync(HttpContext context, ITaktLocalizationService localizationService)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(localizationService);
        var readonlyFields = ReadReadonlyFields(context);
        if (await ContainsFullWidthAsync(context, readonlyFields))
        {
            var template = localizationService.TranslateValidation(TaktValidationI18nKeys.FullWidthForbidden);
            var message = string.Equals(template, TaktValidationI18nKeys.FullWidthForbidden, StringComparison.Ordinal)
                ? "不能包含全角字符"
                : template.Replace("{field}", string.Empty, StringComparison.Ordinal).Trim();
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(JsonSerializer.Serialize(
                TaktApiResult<object>.Fail(message, TaktResultCode.BadRequest)));
            return;
        }
        await _next(context);
    }

    /// <summary>
    /// 创建或更新正文是否含全角字符
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="readonlyFields">表单禁用项，不参与全角检查</param>
    /// <returns>含全角时为 true</returns>
    private static async Task<bool> ContainsFullWidthAsync(HttpContext context, HashSet<string> readonlyFields)
    {
        if (!IsCreateOrUpdate(context))
        {
            return false;
        }
        var contentType = context.Request.ContentType ?? string.Empty;
        if (context.Request.HasFormContentType && contentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
        {
            var form = await context.Request.ReadFormAsync();
            foreach (var pair in form)
            {
                if (IsRichContentField(pair.Key) || IsReadonlySaveField(pair.Key, readonlyFields))
                {
                    continue;
                }
                if (pair.Value.Any(TaktRegexHelper.ContainsFullWidth))
                {
                    return true;
                }
            }
            return false;
        }
        if (!contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
            && !contentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)
            && !contentType.Contains("text/", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        context.Request.EnableBuffering();
        if (context.Request.ContentLength is > MaxJsonScanBytes)
        {
            return false;
        }
        var text = await ReadBodyTextAsync(context.Request.Body, MaxJsonScanBytes);
        context.Request.Body.Position = 0;
        if (text == null)
        {
            return false;
        }
        if (text.Length == 0)
        {
            return false;
        }
        if (contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
        {
            return JsonContainsFullWidth(text, readonlyFields);
        }
        if (contentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
        {
            return FormContainsFullWidth(text, readonlyFields);
        }
        return TaktRegexHelper.ContainsFullWidth(text);
    }

    /// <summary>
    /// 读取正文。超过上限返回 null，避免截断后把设变内容当纯文本误判。
    /// </summary>
    /// <param name="body">请求体</param>
    /// <param name="maxBytes">最大字节数</param>
    /// <returns>正文；超限时为 null</returns>
    private static async Task<string?> ReadBodyTextAsync(Stream body, int maxBytes)
    {
        var rented = ArrayPool<byte>.Shared.Rent(81920);
        try
        {
            using var buffer = new MemoryStream();
            while (true)
            {
                var read = await body.ReadAsync(rented.AsMemory(0, rented.Length));
                if (read <= 0)
                {
                    break;
                }
                if (buffer.Length > maxBytes - read)
                {
                    return null;
                }
                buffer.Write(rented, 0, read);
            }
            return Encoding.UTF8.GetString(buffer.GetBuffer(), 0, (int)buffer.Length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented);
        }
    }

    /// <summary>
    /// JSON 正文是否含全角字符。content、*Content、*Agenda、*Minutes 不检查。无法解析时不把原文当纯文本再扫。
    /// </summary>
    /// <param name="json">请求正文</param>
    /// <param name="readonlyFields">表单禁用项</param>
    /// <returns>含全角时为 true</returns>
    private static bool JsonContainsFullWidth(string json, HashSet<string> readonlyFields)
    {
        try
        {
            using var document = JsonDocument.Parse(json);
            return ElementContainsFullWidth(document.RootElement, readonlyFields);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// 表单键值是否含全角字符。正文类字段不检查。
    /// </summary>
    /// <param name="form">application/x-www-form-urlencoded 正文</param>
    /// <param name="readonlyFields">表单禁用项</param>
    /// <returns>含全角时为 true</returns>
    private static bool FormContainsFullWidth(string form, HashSet<string> readonlyFields)
    {
        foreach (var pair in form.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var separator = pair.IndexOf('=');
            var name = separator < 0 ? pair : pair[..separator];
            var rawValue = separator < 0 ? string.Empty : pair[(separator + 1)..];
            var fieldName = Uri.UnescapeDataString(name.Replace('+', ' '));
            if (IsRichContentField(fieldName) || IsReadonlySaveField(fieldName, readonlyFields))
            {
                continue;
            }
            if (TaktRegexHelper.ContainsFullWidth(Uri.UnescapeDataString(rawValue.Replace('+', ' '))))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// JSON 节点是否含全角字符
    /// </summary>
    /// <param name="element">JSON 节点</param>
    /// <param name="readonlyFields">表单禁用项</param>
    /// <returns>含全角时为 true</returns>
    private static bool ElementContainsFullWidth(JsonElement element, HashSet<string> readonlyFields)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                return TaktRegexHelper.ContainsFullWidth(element.GetString());
            case JsonValueKind.Array:
                foreach (var item in element.EnumerateArray())
                {
                    if (ElementContainsFullWidth(item, readonlyFields))
                    {
                        return true;
                    }
                }
                return false;
            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                {
                    if (IsRichContentField(property.Name) || IsReadonlySaveField(property.Name, readonlyFields))
                    {
                        continue;
                    }
                    if (ElementContainsFullWidth(property.Value, readonlyFields))
                    {
                        return true;
                    }
                }
                return false;
            default:
                return false;
        }
    }

    /// <summary>
    /// 读取本次保存的表单禁用项。名称仅保留字母、数字、下划线与点
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>字段名集合</returns>
    private static HashSet<string> ReadReadonlyFields(HttpContext context)
    {
        var fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (!context.Request.Headers.TryGetValue("X-Takt-Readonly-Fields", out var values))
        {
            return fields;
        }
        var raw = values.ToString();
        if (raw.Length == 0 || raw.Length > 4000)
        {
            return fields;
        }
        foreach (var part in raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (part.Length > 80 || fields.Count >= 200)
            {
                continue;
            }
            if (part.All(ch => char.IsAsciiLetterOrDigit(ch) || ch is '_' or '.'))
            {
                fields.Add(part);
            }
        }
        return fields;
    }

    /// <summary>
    /// 是否为本次请求声明的表单只读项
    /// </summary>
    /// <param name="name">属性名</param>
    /// <param name="readonlyFields">表单禁用项</param>
    /// <returns>只读项时为 true</returns>
    private static bool IsReadonlySaveField(string name, HashSet<string> readonlyFields)
    {
        return readonlyFields.Count > 0 && readonlyFields.Contains(name);
    }

    /// <summary>
    /// 是否为正文类字段（设变内容、邮件内容、会议议程/纪要等）
    /// </summary>
    /// <param name="name">属性名</param>
    /// <returns>正文类字段时为 true</returns>
    private static bool IsRichContentField(string name)
    {
        return name.Equals("content", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("Content", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("Agenda", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("Minutes", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否为创建或更新：POST 集合、POST import、PUT {id}、PUT status/sort/built-in/public
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>是创建或更新时为 true</returns>
    private static bool IsCreateOrUpdate(HttpContext context)
    {
        var parts = (context.Request.Path.Value ?? string.Empty)
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Where(segment => !segment.Equals("api", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (parts.Length == 0)
        {
            return false;
        }
        var action = parts[^1];
        if (HttpMethods.IsPost(context.Request.Method))
        {
            return parts.Length == 1
                || (parts.Length == 2 && action.Equals("import", StringComparison.OrdinalIgnoreCase));
        }
        if (!HttpMethods.IsPut(context.Request.Method) || parts.Length != 2)
        {
            return false;
        }
        return action.All(char.IsDigit)
            || action.Equals("status", StringComparison.OrdinalIgnoreCase)
            || action.Equals("sort", StringComparison.OrdinalIgnoreCase)
            || action.Equals("built-in", StringComparison.OrdinalIgnoreCase)
            || action.Equals("public", StringComparison.OrdinalIgnoreCase);
    }
}
