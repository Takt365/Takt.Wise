// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktProcurementHelper.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：采购链路 BusinessKey 编解码与派生业务编码（短码/会签/费用单）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 采购链路辅助（会签 BusinessKey、派生业务编码）
/// </summary>
public static class TaktProcurementHelper
{
    /// <summary>
    /// 构建会签 BusinessKey（inquiry:{id} / pr:{id} / expense:{id}）
    /// </summary>
    /// <param name="businessType">业务类型段（inquiry/pr/expense）</param>
    /// <param name="entityId">业务主键</param>
    /// <returns>businessKey</returns>
    /// <exception cref="ArgumentException">businessType 为空</exception>
    public static string BuildBusinessKey(string businessType, long entityId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(businessType);
        return $"{businessType.Trim().ToLowerInvariant()}:{entityId}";
    }

    /// <summary>
    /// 解析会签 BusinessKey
    /// </summary>
    /// <param name="businessKey">业务键</param>
    /// <param name="businessType">业务类型段</param>
    /// <param name="entityId">业务主键</param>
    /// <returns>合法格式为 true</returns>
    public static bool TryParseBusinessKey(string? businessKey, out string businessType, out long entityId)
    {
        businessType = string.Empty;
        entityId = 0;
        if (string.IsNullOrWhiteSpace(businessKey))
        {
            return false;
        }

        var index = businessKey.IndexOf(':');
        if (index <= 0 || index >= businessKey.Length - 1)
        {
            return false;
        }

        businessType = businessKey[..index].Trim().ToLowerInvariant();
        return long.TryParse(businessKey[(index + 1)..], out entityId);
    }

    /// <summary>
    /// 由来源主键派生短业务编码
    /// </summary>
    /// <param name="prefix">前缀（1～2 字符）</param>
    /// <param name="sourceId">来源主键</param>
    /// <param name="maxLength">最大长度（默认 10）</param>
    /// <returns>派生编码</returns>
    /// <exception cref="ArgumentException">prefix 为空或 maxLength 过小</exception>
    public static string DeriveShortCode(string prefix, long sourceId, int maxLength = 10)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);
        if (maxLength < 4)
        {
            throw new ArgumentException("maxLength 过小", nameof(maxLength));
        }

        var suffix = (sourceId % 100_000_000).ToString("D8");
        var code = prefix + suffix;
        return code.Length <= maxLength ? code : code[^maxLength..];
    }

    /// <summary>
    /// 会签编码（最长 50）
    /// </summary>
    /// <param name="inquiryCode">询价编码</param>
    /// <param name="inquiryId">询价主键</param>
    /// <returns>会签编码</returns>
    /// <exception cref="ArgumentException">inquiryCode 为空</exception>
    public static string DeriveCountersignCode(string inquiryCode, long inquiryId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inquiryCode);
        var candidate = $"CS-{inquiryCode}";
        return candidate.Length <= 50 ? candidate : $"CS-{inquiryId}";
    }

    /// <summary>
    /// 费用单编码（最长 40）
    /// </summary>
    /// <param name="orderCode">采购订单或来源编码</param>
    /// <param name="orderId">来源主键</param>
    /// <returns>费用单编码</returns>
    /// <exception cref="ArgumentException">orderCode 为空</exception>
    public static string DeriveExpenseCode(string orderCode, long orderId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderCode);
        var candidate = $"EX-{orderCode}";
        return candidate.Length <= 40 ? candidate : $"EX-{orderId}";
    }
}
