// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktLineNumberGenerator.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：明细行号生成器实现（算法委托 Shared；最大值由现有仓储 GetMaxIntAsync 查询）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktLineNumberGenerator 实现
/// 应用服务典型用法：先 <c>repository.GetMaxIntAsync(predicate, x =&gt; x.LineNo)</c>，再调用本生成器
/// </summary>
public sealed class TaktLineNumberGenerator : ITaktLineNumberGenerator
{
    /// <summary>
    /// 生成下一个行号（扁平结构 - 基于业务编码）
    /// </summary>
    /// <param name="businessCode">业务编码（如采购订单号、入库单号等）</param>
    /// <param name="currentMaxLineNumber">当前最大行号（0 表示第一行，从 10 开始）</param>
    /// <returns>下一个行号</returns>
    public int GenerateNext(string businessCode, int currentMaxLineNumber = 0) =>
        TaktSequenceGenerator.GenerateNextLineNumber(businessCode, currentMaxLineNumber);

    /// <summary>
    /// 生成下一个行号（多维度分组结构 - 基于主表业务编码 + 分组代码）
    /// </summary>
    /// <param name="masterBusinessCode">主表业务编码</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="currentMaxLineNumber">当前分组下的最大行号（0 表示第一行）</param>
    /// <returns>下一个行号</returns>
    public int GenerateNextForGroup(string masterBusinessCode, string groupCode, int currentMaxLineNumber = 0) =>
        TaktSequenceGenerator.GenerateNextLineNumberForGroup(masterBusinessCode, groupCode, currentMaxLineNumber);

    /// <summary>
    /// 批量生成行号序列（扁平结构）
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <param name="count">需要生成的行号数量</param>
    /// <param name="startFrom">起始行号（0 表示从 10 开始）</param>
    /// <returns>行号序列</returns>
    public IEnumerable<int> GenerateSequence(string businessCode, int count, int startFrom = 0) =>
        TaktSequenceGenerator.GenerateLineNumberSequence(businessCode, count, startFrom);

    /// <summary>
    /// 批量生成行号序列（多维度分组结构）
    /// </summary>
    /// <param name="masterBusinessCode">主表业务编码</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="count">需要生成的行号数量</param>
    /// <param name="startFrom">起始行号（0 表示从 10 开始）</param>
    /// <returns>行号序列</returns>
    public IEnumerable<int> GenerateSequenceForGroup(
        string masterBusinessCode,
        string groupCode,
        int count,
        int startFrom = 0) =>
        TaktSequenceGenerator.GenerateLineNumberSequenceForGroup(masterBusinessCode, groupCode, count, startFrom);

    /// <summary>
    /// 格式化行号为完整业务编码（如：PO20250001-10）
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <param name="lineNumber">行号</param>
    /// <returns>完整业务编码</returns>
    public string FormatBusinessCode(string businessCode, int lineNumber) =>
        TaktSequenceGenerator.FormatLineBusinessCode(businessCode, lineNumber);
}
