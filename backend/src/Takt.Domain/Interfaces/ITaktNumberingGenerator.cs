// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktNumberingGenerator.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：业务编码生成器接口（按编码规则递增流水并写库；Infrastructure 由 TaktNumberingGenerator 实现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 业务编码生成器（运行时取号，供 HelpDesk、文管、通知等各业务创建单据时调用）
/// </summary>
public interface ITaktNumberingGenerator
{
    /// <summary>
    /// 生成下一个业务编码（递增流水并持久化）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果</returns>
    Task<TaktNumberingModel> GenerateNextAsync(string ruleCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// 预览下一个业务编码（不占用流水号、不写库）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
    Task<TaktNumberingModel> PreviewNextAsync(string ruleCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// 尝试生成下一个业务编码；规则不存在或已禁用时返回 null（不抛异常）
    /// </summary>
    /// <param name="ruleCode">规则编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>生成结果；不可用时为 null</returns>
    Task<TaktNumberingModel?> TryGenerateNextAsync(string ruleCode, CancellationToken cancellationToken = default);
}
