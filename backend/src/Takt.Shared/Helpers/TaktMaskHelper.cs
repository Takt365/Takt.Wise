// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktMaskHelper.cs
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：数据脱敏工具类，与前端 mask.ts 对齐
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Takt.Shared.Helpers;

/// <summary>
/// 数据脱敏工具类（与前端 <c>MaskHelper</c> / <c>mask.ts</c> 对齐）。
/// </summary>
/// <remarks>无状态；<c>DefaultSensitiveFields</c> 为编译期不可变常量数组。</remarks>
public static class TaktMaskHelper
{
    /// <summary>
    /// 默认敏感字段列表（与前端 DEFAULT_SENSITIVE_FIELDS 一致）。
    /// </summary>
    private static readonly string[] DefaultSensitiveFields =
    [
        "password", "pwd", "passwd", "token", "authorization", "auth", "csrf", "cookie",
        "secret", "key", "apiKey", "apikey", "accessKey", "secretKey", "privateKey", "publicKey",
        "ticket", "cipher", "loginTicket", "idCard", "idcard", "identityCard",
        "bankCard", "bankcard", "cardNumber", "cardCode", "creditCard",
        "phone", "mobile", "telephone", "tel", "email", "mail", "address", "addr"
    ];

    /// <summary>
    /// 脱敏手机号（默认保留前 3 后 4）。
    /// </summary>
    public static string MaskPhone(string? phone, int start = 3, int end = 4, char maskChar = '*')
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return string.Empty;
        }

        var phoneStr = phone.Trim();
        if (phoneStr.Length <= start + end)
        {
            return phoneStr;
        }

        var visibleStart = phoneStr[..start];
        var visibleEnd = phoneStr[^end..];
        var maskLength = phoneStr.Length - start - end;
        return visibleStart + new string(maskChar, maskLength) + visibleEnd;
    }

    /// <summary>
    /// 脱敏邮箱（保留 @ 前首字符与域名）。
    /// </summary>
    public static string MaskEmail(string? email, char maskChar = '*')
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return string.Empty;
        }

        var emailStr = email.Trim();
        var atIndex = emailStr.IndexOf('@');
        if (atIndex <= 0)
        {
            return emailStr;
        }

        var UserName = emailStr[..atIndex];
        var domain = emailStr[atIndex..];

        if (UserName.Length <= 1)
        {
            return UserName + new string(maskChar, 3) + domain;
        }

        var visibleStart = UserName[..1];
        var mask = new string(maskChar, Math.Max(3, UserName.Length - 1));
        return visibleStart + mask + domain;
    }

    /// <summary>
    /// 脱敏身份证号（默认保留前 3 后 4）。
    /// </summary>
    public static string MaskIdCard(string? idCard, int start = 3, int end = 4, char maskChar = '*')
        => MaskSegment(idCard, start, end, maskChar);

    /// <summary>
    /// 脱敏银行卡号（默认保留前 4 后 4）。
    /// </summary>
    public static string MaskBankCard(string? bankCard, int start = 4, int end = 4, char maskChar = '*')
        => MaskSegment(bankCard, start, end, maskChar);

    /// <summary>
    /// 脱敏姓名。
    /// </summary>
    public static string MaskName(string? name, char maskChar = '*')
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        var nameStr = name.Trim();
        if (nameStr.Length <= 1)
        {
            return nameStr;
        }

        if (nameStr.Length == 2)
        {
            return nameStr[0] + maskChar.ToString();
        }

        return nameStr[..1] + new string(maskChar, nameStr.Length - 1);
    }

    /// <summary>
    /// 脱敏地址（默认保留前 6 位）。
    /// </summary>
    public static string MaskAddress(string? address, int keepLength = 6, char maskChar = '*')
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return string.Empty;
        }

        var addressStr = address.Trim();
        if (addressStr.Length <= keepLength)
        {
            return addressStr;
        }

        var visibleStart = addressStr[..keepLength];
        var mask = new string(maskChar, Math.Min(4, addressStr.Length - keepLength));
        return visibleStart + mask;
    }

    /// <summary>
    /// 通用脱敏（默认保留前 3 后 3）。
    /// </summary>
    public static string Mask(string? text, int start = 3, int end = 3, char maskChar = '*')
        => MaskSegment(text, start, end, maskChar);

    /// <summary>
    /// 完全脱敏。
    /// </summary>
    public static string MaskFull(string? text, char maskChar = '*')
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        return new string(maskChar, text.Length);
    }

    /// <summary>
    /// 脱敏对象中的敏感字段（JSON 对象/数组递归）。
    /// </summary>
    public static JsonNode? MaskObject(JsonNode? node, IReadOnlyList<string>? sensitiveFields = null, char maskChar = '*')
    {
        if (node is null)
        {
            return null;
        }

        var fields = sensitiveFields ?? DefaultSensitiveFields;

        return node switch
        {
            JsonObject obj => MaskJsonObject(obj, fields, maskChar),
            JsonArray arr => MaskJsonArray(arr, fields, maskChar),
            _ => node
        };
    }

    /// <summary>
    /// 脱敏字典对象中的敏感字段。
    /// </summary>
    public static object? MaskObject(object? obj, IReadOnlyList<string>? sensitiveFields = null, char maskChar = '*')
    {
        if (obj is null)
        {
            return null;
        }

        if (obj is not IDictionary dict)
        {
            if (obj is IEnumerable enumerable and not string)
            {
                var list = new List<object?>();
                foreach (var item in enumerable)
                {
                    list.Add(MaskObject(item, sensitiveFields, maskChar));
                }
                return list;
            }
            return obj;
        }

        var fields = sensitiveFields ?? DefaultSensitiveFields;
        var sanitized = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        foreach (DictionaryEntry entry in dict)
        {
            var key = Convert.ToString(entry.Key) ?? string.Empty;
            var value = entry.Value;
            var lowerKey = key.ToLowerInvariant();

            if (IsSensitiveField(key, fields))
            {
                sanitized[key] = MaskSensitiveValue(lowerKey, value, maskChar);
            }
            else if (value is IDictionary or IEnumerable and not string)
            {
                sanitized[key] = MaskObject(value, fields, maskChar);
            }
            else
            {
                sanitized[key] = value;
            }
        }

        return sanitized;
    }

    /// <summary>
    /// 检查字段名是否为敏感字段。
    /// </summary>
    public static bool IsSensitiveField(string? fieldName, IReadOnlyList<string>? sensitiveFields = null)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
        {
            return false;
        }

        var fields = sensitiveFields ?? DefaultSensitiveFields;
        var lowerFieldName = fieldName.ToLowerInvariant();
        return fields.Any(field => lowerFieldName.Contains(field, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 脱敏日志数据
    /// </summary>
    /// <param name="data">待脱敏对象</param>
    /// <param name="sensitiveFields">敏感字段名列表；null 时使用默认规则</param>
    /// <returns>脱敏后的对象</returns>
    public static object? MaskForLogging(object? data, IReadOnlyList<string>? sensitiveFields = null)
        => MaskObject(data, sensitiveFields, '*');

    /// <summary>
    /// 按起止可见位数对字符串分段脱敏
    /// </summary>
    /// <param name="text">原文</param>
    /// <param name="start">首部保留字符数</param>
    /// <param name="end">尾部保留字符数</param>
    /// <param name="maskChar">掩码字符</param>
    /// <returns>脱敏后的字符串</returns>
    private static string MaskSegment(string? text, int start, int end, char maskChar)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var textStr = text.Trim();
        if (textStr.Length <= start + end)
        {
            return textStr;
        }

        var visibleStart = textStr[..start];
        var visibleEnd = textStr[^end..];
        var maskLength = textStr.Length - start - end;
        return visibleStart + new string(maskChar, maskLength) + visibleEnd;
    }

    /// <summary>
    /// 递归脱敏 JSON 对象节点
    /// </summary>
    /// <param name="obj">JSON 对象</param>
    /// <param name="fields">敏感字段名列表</param>
    /// <param name="maskChar">掩码字符</param>
    /// <returns>脱敏后的 JSON 对象</returns>
    private static JsonObject MaskJsonObject(JsonObject obj, IReadOnlyList<string> fields, char maskChar)
    {
        var sanitized = new JsonObject();
        foreach (var property in obj)
        {
            var key = property.Key;
            var lowerKey = key.ToLowerInvariant();
            if (IsSensitiveField(key, fields))
            {
                sanitized[key] = MaskSensitiveJsonValue(lowerKey, property.Value, maskChar);
            }
            else if (property.Value is JsonObject childObj)
            {
                sanitized[key] = MaskJsonObject(childObj, fields, maskChar);
            }
            else if (property.Value is JsonArray childArr)
            {
                sanitized[key] = MaskJsonArray(childArr, fields, maskChar);
            }
            else
            {
                sanitized[key] = property.Value?.DeepClone();
            }
        }
        return sanitized;
    }

    /// <summary>
    /// 递归脱敏 JSON 数组节点
    /// </summary>
    /// <param name="arr">JSON 数组</param>
    /// <param name="fields">敏感字段名列表</param>
    /// <param name="maskChar">掩码字符</param>
    /// <returns>脱敏后的 JSON 数组</returns>
    private static JsonArray MaskJsonArray(JsonArray arr, IReadOnlyList<string> fields, char maskChar)
    {
        var sanitized = new JsonArray();
        foreach (var item in arr)
        {
            if (item is JsonObject childObj)
            {
                sanitized.Add(MaskJsonObject(childObj, fields, maskChar));
            }
            else if (item is JsonArray childArr)
            {
                sanitized.Add(MaskJsonArray(childArr, fields, maskChar));
            }
            else
            {
                sanitized.Add(item?.DeepClone());
            }
        }
        return sanitized;
    }

    /// <summary>
    /// 按字段语义脱敏 CLR 对象值
    /// </summary>
    /// <param name="lowerKey">小写字段名（用于语义匹配）</param>
    /// <param name="value">原始值</param>
    /// <param name="maskChar">掩码字符</param>
    /// <returns>脱敏后的值</returns>
    private static object? MaskSensitiveValue(string lowerKey, object? value, char maskChar)
    {
        if (value is string str && !string.IsNullOrWhiteSpace(str))
        {
            return MaskSensitiveString(lowerKey, str, maskChar);
        }
        return new string(maskChar, 3);
    }

    /// <summary>
    /// 按字段语义脱敏 JSON 节点值
    /// </summary>
    /// <param name="lowerKey">小写字段名（用于语义匹配）</param>
    /// <param name="value">JSON 节点值</param>
    /// <param name="maskChar">掩码字符</param>
    /// <returns>脱敏后的 JSON 节点</returns>
    private static JsonNode? MaskSensitiveJsonValue(string lowerKey, JsonNode? value, char maskChar)
    {
        if (value is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var str) && !string.IsNullOrWhiteSpace(str))
        {
            return MaskSensitiveString(lowerKey, str, maskChar);
        }
        return new string(maskChar, 3);
    }

    /// <summary>
    /// 按字段名语义选择脱敏策略（手机/邮箱/身份证等）
    /// </summary>
    /// <param name="lowerKey">小写字段名</param>
    /// <param name="value">原始字符串</param>
    /// <param name="maskChar">掩码字符</param>
    /// <returns>脱敏后的字符串</returns>
    private static string MaskSensitiveString(string lowerKey, string value, char maskChar)
    {
        if (lowerKey.Contains("phone", StringComparison.Ordinal) || lowerKey.Contains("mobile", StringComparison.Ordinal) || lowerKey.Contains("tel", StringComparison.Ordinal))
        {
            return MaskPhone(value, 3, 4, maskChar);
        }
        if (lowerKey.Contains("email", StringComparison.Ordinal) || lowerKey.Contains("mail", StringComparison.Ordinal))
        {
            return MaskEmail(value, maskChar);
        }
        if (lowerKey.Contains("idcard", StringComparison.Ordinal) || lowerKey.Contains("identitycard", StringComparison.Ordinal))
        {
            return MaskIdCard(value, 3, 4, maskChar);
        }
        if (lowerKey.Contains("bankcard", StringComparison.Ordinal) || lowerKey.Contains("cardnumber", StringComparison.Ordinal) || lowerKey.Contains("cardno", StringComparison.Ordinal))
        {
            return MaskBankCard(value, 4, 4, maskChar);
        }
        if (lowerKey.Contains("name", StringComparison.Ordinal) && !lowerKey.Contains("user_name", StringComparison.Ordinal))
        {
            return MaskName(value, maskChar);
        }
        if (lowerKey.Contains("address", StringComparison.Ordinal) || lowerKey.Contains("addr", StringComparison.Ordinal))
        {
            return MaskAddress(value, 6, maskChar);
        }
        return MaskFull(value, maskChar);
    }
}
