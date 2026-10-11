// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFlowFormBindingHelper.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：解析 RelatedFormField、FrmData 与库列双向映射
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Takt.Shared.Enums;
using Takt.Shared.Models.Workflow;

namespace Takt.Shared.Helpers;

/// <summary>
/// 流程表单绑定解析（RelatedFormField JSON ↔ FrmData ↔ 库列）
/// </summary>
public static class TaktFlowFormBindingHelper
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };

    /// <summary>
    /// 解析 RelatedFormField JSON（支持纯数组或 { fields, business }）
    /// </summary>
    /// <param name="relatedFormFieldJson">表单关联字段 JSON</param>
    /// <returns>绑定根对象</returns>
    public static TaktFlowFormBindingRoot ParseBinding(string? relatedFormFieldJson)
    {
        if (string.IsNullOrWhiteSpace(relatedFormFieldJson))
        {
            return new TaktFlowFormBindingRoot();
        }
        var token = JToken.Parse(relatedFormFieldJson);
        if (token.Type == JTokenType.Array)
        {
            var fields = token.ToObject<List<TaktFlowFormFieldMapping>>(JsonSerializer.Create(JsonSettings)) ?? new List<TaktFlowFormFieldMapping>();
            return new TaktFlowFormBindingRoot { Fields = fields };
        }
        return token.ToObject<TaktFlowFormBindingRoot>(JsonSerializer.Create(JsonSettings)) ?? new TaktFlowFormBindingRoot();
    }

    /// <summary>
    /// 由 FrmData 构建库列字典（仅映射字段）
    /// </summary>
    /// <param name="frmData">表单 JSON</param>
    /// <param name="binding">绑定根</param>
    /// <returns>列名 → 值</returns>
    public static Dictionary<string, object?> BuildDbColumnsFromFrmData(string? frmData, TaktFlowFormBindingRoot binding)
    {
        ArgumentNullException.ThrowIfNull(binding);
        var frmDict = ParseFrmDataToStringDictionary(frmData);
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var field in binding.Fields)
        {
            if (string.IsNullOrWhiteSpace(field.DbColumnName) || string.IsNullOrWhiteSpace(field.CsharpColumnName))
            {
                continue;
            }
            if (!frmDict.TryGetValue(field.CsharpColumnName, out var raw) || string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }
            result[field.DbColumnName] = ConvertFieldValue(raw, field.DataType);
        }
        return result;
    }

    /// <summary>
    /// 由库行构建 FrmData JSON
    /// </summary>
    /// <param name="row">列名 → 值（大小写不敏感）</param>
    /// <param name="binding">绑定根</param>
    /// <returns>FrmData JSON</returns>
    public static string BuildFrmDataFromDbRow(IReadOnlyDictionary<string, object?> row, TaktFlowFormBindingRoot binding)
    {
        ArgumentNullException.ThrowIfNull(row);
        ArgumentNullException.ThrowIfNull(binding);
        var fields = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var field in binding.Fields)
        {
            if (string.IsNullOrWhiteSpace(field.CsharpColumnName) || string.IsNullOrWhiteSpace(field.DbColumnName))
            {
                continue;
            }
            var value = GetRowValue(row, field.DbColumnName);
            if (value != null)
            {
                fields[field.CsharpColumnName] = FormatFieldValue(value, field.DataType);
            }
        }
        return SerializeFrmDataFields(fields);
    }

    /// <summary>
    /// 将键值对序列化为 FrmData JSON（camelCase 扁平键）
    /// </summary>
    /// <param name="fields">表单字段</param>
    /// <returns>JSON 字符串</returns>
    /// <exception cref="ArgumentNullException">fields 为 null</exception>
    public static string SerializeFrmDataFields(IDictionary<string, object?> fields)
    {
        ArgumentNullException.ThrowIfNull(fields);
        return JsonConvert.SerializeObject(fields, JsonSettings);
    }

    /// <summary>
    /// 解析 FrmData 为字典（camelCase 键）
    /// </summary>
    /// <param name="frmData">表单 JSON</param>
    /// <returns>字段字典；空或非法 JSON 时返回空字典</returns>
    public static Dictionary<string, string?> ParseFrmDataToStringDictionary(string? frmData)
    {
        if (string.IsNullOrWhiteSpace(frmData))
        {
            return new Dictionary<string, string?>();
        }
        var raw = JsonConvert.DeserializeObject<Dictionary<string, object?>>(frmData, JsonSettings);
        if (raw == null)
        {
            return new Dictionary<string, string?>();
        }
        var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in raw)
        {
            result[pair.Key] = pair.Value?.ToString();
        }
        return result;
    }

    /// <summary>
    /// 按流程终态解析业务状态值
    /// </summary>
    /// <param name="business">业务绑定</param>
    /// <param name="instanceStatus">流程实例终态</param>
    /// <returns>业务状态值；未配置时 null</returns>
    public static int? ResolveBusinessStatusValue(TaktFlowFormBusinessBinding? business, TaktFlowInstanceStatus instanceStatus)
    {
        if (business == null || string.IsNullOrWhiteSpace(business.BusinessStatusColumn))
        {
            return null;
        }
        switch (instanceStatus)
        {
            case TaktFlowInstanceStatus.Completed:
                return business.StatusApproved;
            case TaktFlowInstanceStatus.Rejected:
                return business.StatusRejected;
            case TaktFlowInstanceStatus.Terminated:
                return business.StatusCancelled;
            default:
                return null;
        }
    }

    /// <summary>
    /// 转换 FrmData 字符串为库列类型
    /// </summary>
    private static object? ConvertFieldValue(string raw, string? dataType)
    {
        if (string.IsNullOrWhiteSpace(dataType))
        {
            return raw;
        }
        var dt = dataType.Trim().ToLowerInvariant();
        if (dt is "int" or "tinyint" or "smallint")
        {
            return int.TryParse(raw, out var i) ? i : raw;
        }
        if (dt is "bigint")
        {
            return long.TryParse(raw, out var l) ? l : raw;
        }
        if (dt is "decimal" or "numeric" or "money")
        {
            return decimal.TryParse(raw, out var d) ? d : raw;
        }
        if (dt is "date" or "datetime")
        {
            return DateTime.TryParse(raw, out var dtVal) ? dtVal : raw;
        }
        return raw;
    }

    /// <summary>
    /// 格式化库值为 FrmData 字符串
    /// </summary>
    private static string? FormatFieldValue(object value, string? dataType)
    {
        if (value is DateTime dateTime)
        {
            var dt = dataType?.Trim().ToLowerInvariant();
            return dt == "date" ? dateTime.ToString("yyyy-MM-dd") : dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        return value.ToString();
    }

    /// <summary>
    /// 从行字典读取列值
    /// </summary>
    private static object? GetRowValue(IReadOnlyDictionary<string, object?> row, string columnName)
    {
        foreach (var pair in row)
        {
            if (string.Equals(pair.Key, columnName, StringComparison.OrdinalIgnoreCase))
            {
                return pair.Value;
            }
        }
        return null;
    }
}
