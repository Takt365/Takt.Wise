// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktGenQueryTypeHelper.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成列 IsQuery 与 QueryType 关联解析（对齐字典 code_generator_query_type dict_value）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 代码生成列 IsQuery 与 QueryType 关联工具
/// </summary>
public static class TaktGenQueryTypeHelper
{
    /// <summary>
    /// 按 IsQuery 解析 QueryType：IsQuery=0 一律空串；IsQuery=1 时保留有效值，否则 string/Guid 默认 like、其他默认 eq
    /// </summary>
    /// <param name="isQuery">是否查询字段（1=是，0=否）</param>
    /// <param name="queryType">提交的查询方式（可为空）</param>
    /// <param name="csharpDataType">C# 类型名</param>
    /// <returns>规范化后的 QueryType</returns>
    public static string Resolve(int isQuery, string? queryType, string? csharpDataType)
    {
        if (isQuery != 1)
        {
            return string.Empty;
        }
        var trimmed = queryType?.Trim();
        if (!string.IsNullOrEmpty(trimmed))
        {
            return trimmed;
        }
        return IsStringLikeCsharpType(csharpDataType) ? "like" : "eq";
    }

    /// <summary>
    /// 判断 C# 类型是否按字符串类查询默认 like
    /// </summary>
    /// <param name="csharpDataType">C# 类型名</param>
    /// <returns>是否为 string/Guid 等等价字符串查询类型</returns>
    public static bool IsStringLikeCsharpType(string? csharpDataType)
    {
        if (string.IsNullOrWhiteSpace(csharpDataType))
        {
            return true;
        }
        var t = csharpDataType.Trim();
        return t.Equals("string", StringComparison.OrdinalIgnoreCase)
            || t.Equals("guid", StringComparison.OrdinalIgnoreCase);
    }
}
