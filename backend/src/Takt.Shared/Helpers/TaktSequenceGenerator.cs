// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSequenceGenerator.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：行号与排序号生成（配合仓储 GetMaxIntAsync 查询当前最大值）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// 行号、排序号序列生成（无状态；currentMax 由 <c>ITaktTenantRepository</c> / <c>ITaktCompanyRepository</c> / <c>ITaktApprovalRepository</c> 的 <c>GetMaxIntAsync</c> 提供）
/// </summary>
/// <remarks>纯函数；最大值由调用方查询仓储后传入，本类不存储业务状态。</remarks>
public static class TaktSequenceGenerator
{
    /// <summary>
    /// 生成下一个行号（扁平结构 - 基于业务编码）
    /// </summary>
    /// <param name="businessCode">业务编码（如采购订单号、入库单号等）</param>
    /// <param name="currentMaxLineNumber">当前最大行号（0 表示第一行，从 10 开始）</param>
    /// <returns>下一个行号</returns>
    public static int GenerateNextLineNumber(string businessCode, int currentMaxLineNumber = 0)
    {
        EnsureBusinessCode(businessCode);
        return ComputeNext(
            currentMaxLineNumber,
            TaktSequenceDefaults.LineNumberDefaultStart,
            TaktSequenceDefaults.LineNumberStep);
    }

    /// <summary>
    /// 生成下一个行号（多维度分组结构 - 基于主表业务编码 + 分组代码）
    /// </summary>
    /// <param name="masterBusinessCode">主表业务编码</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="currentMaxLineNumber">当前分组下的最大行号（0 表示第一行）</param>
    /// <returns>下一个行号</returns>
    public static int GenerateNextLineNumberForGroup(
        string masterBusinessCode,
        string groupCode,
        int currentMaxLineNumber = 0)
    {
        EnsureBusinessCode(masterBusinessCode);
        EnsureGroupCode(groupCode);
        return ComputeNext(
            currentMaxLineNumber,
            TaktSequenceDefaults.LineNumberDefaultStart,
            TaktSequenceDefaults.LineNumberStep);
    }

    /// <summary>
    /// 批量生成行号序列（扁平结构）
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <param name="count">数量</param>
    /// <param name="startFrom">起始行号（0 表示从 10 开始）</param>
    /// <returns>行号序列</returns>
    public static IEnumerable<int> GenerateLineNumberSequence(string businessCode, int count, int startFrom = 0)
    {
        EnsureBusinessCode(businessCode);
        EnsurePositiveCount(count);
        return ComputeSequence(
            count,
            startFrom,
            TaktSequenceDefaults.LineNumberDefaultStart,
            TaktSequenceDefaults.LineNumberStep);
    }

    /// <summary>
    /// 批量生成行号序列（分组结构）
    /// </summary>
    /// <param name="masterBusinessCode">主表业务编码</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="count">数量</param>
    /// <param name="startFrom">起始行号（0 表示从 10 开始）</param>
    /// <returns>行号序列</returns>
    public static IEnumerable<int> GenerateLineNumberSequenceForGroup(
        string masterBusinessCode,
        string groupCode,
        int count,
        int startFrom = 0)
    {
        EnsureBusinessCode(masterBusinessCode);
        EnsureGroupCode(groupCode);
        EnsurePositiveCount(count);
        return ComputeSequence(
            count,
            startFrom,
            TaktSequenceDefaults.LineNumberDefaultStart,
            TaktSequenceDefaults.LineNumberStep);
    }

    /// <summary>
    /// 格式化行号为完整业务编码（如：PO20250001-10）
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <param name="lineNumber">行号</param>
    /// <returns>完整业务编码</returns>
    public static string FormatLineBusinessCode(string businessCode, int lineNumber)
    {
        EnsureBusinessCode(businessCode);
        if (lineNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lineNumber), lineNumber, "行号必须大于 0");
        }

        return $"{businessCode.Trim()}-{lineNumber}";
    }

    /// <summary>
    /// 生成下一个排序号（扁平结构 - 无父 ID）
    /// </summary>
    /// <param name="currentMaxSortOrder">当前最大排序号（0 表示第一个，从 1 开始）</param>
    /// <returns>下一个排序号</returns>
    public static int GenerateNextSortOrder(int currentMaxSortOrder = 0) =>
        ComputeNext(
            currentMaxSortOrder,
            TaktSequenceDefaults.SortOrderDefaultStart,
            TaktSequenceDefaults.SortOrderStep);

    /// <summary>
    /// 生成下一个排序号（树形结构 - 有父 ID）
    /// </summary>
    /// <param name="parentId">父节点 ID</param>
    /// <param name="currentMaxSortOrder">当前父节点下的最大排序号</param>
    /// <returns>下一个排序号</returns>
    public static int GenerateNextSortOrder(long parentId, int currentMaxSortOrder = 0)
    {
        EnsurePositiveId(parentId, nameof(parentId));
        return GenerateNextSortOrder(currentMaxSortOrder);
    }

    /// <summary>
    /// 生成下一个排序号（主子表结构 - 有主表 ID）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="currentMaxSortOrder">当前主表下的最大排序号</param>
    /// <returns>下一个排序号</returns>
    public static int GenerateNextSortOrderForMaster(long masterId, int currentMaxSortOrder = 0)
    {
        EnsurePositiveId(masterId, nameof(masterId));
        return GenerateNextSortOrder(currentMaxSortOrder);
    }

    /// <summary>
    /// 生成下一个排序号（多维度分组结构）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="currentMaxSortOrder">当前分组下的最大排序号</param>
    /// <returns>下一个排序号</returns>
    public static int GenerateNextSortOrderForGroup(long masterId, string groupCode, int currentMaxSortOrder = 0)
    {
        EnsurePositiveId(masterId, nameof(masterId));
        EnsureGroupCode(groupCode);
        return GenerateNextSortOrder(currentMaxSortOrder);
    }

    /// <summary>
    /// 批量生成排序号序列（扁平结构）
    /// </summary>
    /// <param name="count">数量</param>
    /// <param name="startFrom">起始排序号（0 表示从 1 开始）</param>
    /// <returns>排序号序列</returns>
    public static IEnumerable<int> GenerateSortOrderSequence(int count, int startFrom = 0)
    {
        EnsurePositiveCount(count);
        return ComputeSequence(
            count,
            startFrom,
            TaktSequenceDefaults.SortOrderDefaultStart,
            TaktSequenceDefaults.SortOrderStep);
    }

    /// <summary>
    /// 批量生成排序号序列（树形结构 - 同一父节点下）
    /// </summary>
    /// <param name="parentId">父节点 ID</param>
    /// <param name="count">数量</param>
    /// <param name="startFrom">起始排序号</param>
    /// <returns>排序号序列</returns>
    public static IEnumerable<int> GenerateSortOrderSequence(long parentId, int count, int startFrom = 0)
    {
        EnsurePositiveId(parentId, nameof(parentId));
        return GenerateSortOrderSequence(count, startFrom);
    }

    /// <summary>
    /// 批量生成排序号序列（主子表结构）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="count">数量</param>
    /// <param name="startFrom">起始排序号</param>
    /// <returns>排序号序列</returns>
    public static IEnumerable<int> GenerateSortOrderSequenceForMaster(long masterId, int count, int startFrom = 0)
    {
        EnsurePositiveId(masterId, nameof(masterId));
        return GenerateSortOrderSequence(count, startFrom);
    }

    /// <summary>
    /// 批量生成排序号序列（分组结构）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="count">数量</param>
    /// <param name="startFrom">起始排序号</param>
    /// <returns>排序号序列</returns>
    public static IEnumerable<int> GenerateSortOrderSequenceForGroup(
        long masterId,
        string groupCode,
        int count,
        int startFrom = 0)
    {
        EnsurePositiveId(masterId, nameof(masterId));
        EnsureGroupCode(groupCode);
        return GenerateSortOrderSequence(count, startFrom);
    }

    /// <summary>
    /// 计算下一个序列值
    /// </summary>
    /// <param name="currentMax">当前最大值</param>
    /// <param name="defaultStart">首条默认值</param>
    /// <param name="step">步长</param>
    /// <returns>下一个值</returns>
    private static int ComputeNext(int currentMax, int defaultStart, int step)
    {
        if (currentMax <= 0)
        {
            return defaultStart;
        }

        return currentMax + step;
    }

    /// <summary>
    /// 批量生成序列
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <param name="startFrom">起始基准值</param>
    /// <param name="defaultStart">首条默认值</param>
    /// <param name="step">步长</param>
    /// <returns>序列迭代器</returns>
    private static IEnumerable<int> ComputeSequence(int count, int startFrom, int defaultStart, int step)
    {
        var current = startFrom;
        for (var index = 0; index < count; index++)
        {
            current = ComputeNext(current, defaultStart, step);
            yield return current;
        }
    }

    /// <summary>
    /// 校验业务编码非空
    /// </summary>
    /// <param name="businessCode">业务编码</param>
    /// <exception cref="ArgumentException"><paramref name="businessCode"/> 为空</exception>
    private static void EnsureBusinessCode(string businessCode)
    {
        if (string.IsNullOrWhiteSpace(businessCode))
        {
            throw new ArgumentException("业务编码不能为空", nameof(businessCode));
        }
    }

    /// <summary>
    /// 校验分组代码非空
    /// </summary>
    /// <param name="groupCode">分组代码</param>
    /// <exception cref="ArgumentException"><paramref name="groupCode"/> 为空</exception>
    private static void EnsureGroupCode(string groupCode)
    {
        if (string.IsNullOrWhiteSpace(groupCode))
        {
            throw new ArgumentException("分组代码不能为空", nameof(groupCode));
        }
    }

    /// <summary>
    /// 校验 Id 为正数
    /// </summary>
    /// <param name="id">待校验 Id</param>
    /// <param name="paramName">参数名（用于异常消息）</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> 不大于 0</exception>
    private static void EnsurePositiveId(long id, string paramName)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, id, $"{paramName} 必须大于 0");
        }
    }

    /// <summary>
    /// 校验生成数量为正数
    /// </summary>
    /// <param name="count">生成数量</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> 不大于 0</exception>
    private static void EnsurePositiveCount(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "生成数量必须大于 0");
        }
    }
}
