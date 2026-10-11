// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktLineNumberGenerator.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：明细行号生成器接口（currentMax 由现有仓储 GetMaxIntAsync 提供）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 明细行号生成器（Infrastructure 由 TaktLineNumberGenerator 实现）
/// </summary>
public interface ITaktLineNumberGenerator
{
    /// <summary>
    /// 生成下一个行号（扁平结构 - 基于业务编码）
    /// </summary>
    /// <param name="businessCode">业务编码（如采购订单号、入库单号等）</param>
    /// <param name="currentMaxLineNumber">当前最大行号（0 表示第一行，从 10 开始；由仓储查询）</param>
    /// <returns>下一个行号</returns>
    int GenerateNext(string businessCode, int currentMaxLineNumber = 0);

    /// <summary>
    /// 生成下一个行号（多维度分组结构 - 基于主表业务编码 + 分组代码）
    /// </summary>
    /// <param name="masterBusinessCode">主表业务编码（如设变单号、采购订单号等）</param>
    /// <param name="groupCode">分组代码（如部门代码、类别代码等）</param>
    /// <param name="currentMaxLineNumber">当前分组下的最大行号（0 表示第一行）</param>
    /// <returns>下一个行号</returns>
    int GenerateNextForGroup(string masterBusinessCode, string groupCode, int currentMaxLineNumber = 0);

    /// <summary>
    /// 批量生成行号序列（扁平结构）
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <param name="count">需要生成的行号数量</param>
    /// <param name="startFrom">起始行号（0 表示从 10 开始；非 0 表示当前最大行号）</param>
    /// <returns>行号序列</returns>
    IEnumerable<int> GenerateSequence(string businessCode, int count, int startFrom = 0);

    /// <summary>
    /// 批量生成行号序列（多维度分组结构）
    /// </summary>
    /// <param name="masterBusinessCode">主表业务编码</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="count">需要生成的行号数量</param>
    /// <param name="startFrom">起始行号（0 表示从 10 开始）</param>
    /// <returns>行号序列</returns>
    IEnumerable<int> GenerateSequenceForGroup(string masterBusinessCode, string groupCode, int count, int startFrom = 0);

    /// <summary>
    /// 格式化行号为完整业务编码（如：PO20250001-10）
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <param name="lineNumber">行号</param>
    /// <returns>完整业务编码</returns>
    string FormatBusinessCode(string businessCode, int lineNumber);
}
