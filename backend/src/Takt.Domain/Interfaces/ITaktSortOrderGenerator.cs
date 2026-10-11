// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktSortOrderGenerator.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：排序号生成器接口（currentMax 由现有仓储 GetMaxIntAsync 提供）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 排序号生成器（Infrastructure 由 TaktSortOrderGenerator 实现）
/// </summary>
public interface ITaktSortOrderGenerator
{
    /// <summary>
    /// 生成下一个排序号（扁平结构 - 无父 ID）
    /// </summary>
    /// <param name="currentMaxSortOrder">当前最大排序号（0 表示第一个，从 1 开始；由仓储查询）</param>
    /// <returns>下一个排序号</returns>
    int GenerateNext(int currentMaxSortOrder = 0);

    /// <summary>
    /// 生成下一个排序号（树形结构 - 有父 ID）
    /// </summary>
    /// <param name="parentId">父节点 ID</param>
    /// <param name="currentMaxSortOrder">当前父节点下的最大排序号（0 表示第一个子节点）</param>
    /// <returns>下一个排序号</returns>
    int GenerateNext(long parentId, int currentMaxSortOrder = 0);

    /// <summary>
    /// 生成下一个排序号（主子表结构 - 有主表 ID）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="currentMaxSortOrder">当前主表下的最大排序号（0 表示第一个明细）</param>
    /// <returns>下一个排序号</returns>
    int GenerateNextForMaster(long masterId, int currentMaxSortOrder = 0);

    /// <summary>
    /// 生成下一个排序号（多维度分组结构 - 有主表 ID + 分组代码）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="groupCode">分组代码（如部门代码、类别代码等）</param>
    /// <param name="currentMaxSortOrder">当前分组下的最大排序号（0 表示第一个）</param>
    /// <returns>下一个排序号</returns>
    int GenerateNextForGroup(long masterId, string groupCode, int currentMaxSortOrder = 0);

    /// <summary>
    /// 批量生成排序号序列（扁平结构）
    /// </summary>
    /// <param name="count">需要生成的排序号数量</param>
    /// <param name="startFrom">起始排序号（0 表示从 1 开始；非 0 表示当前最大排序号）</param>
    /// <returns>排序号序列</returns>
    IEnumerable<int> GenerateSequence(int count, int startFrom = 0);

    /// <summary>
    /// 批量生成排序号序列（树形结构 - 同一父节点下）
    /// </summary>
    /// <param name="parentId">父节点 ID</param>
    /// <param name="count">需要生成的排序号数量</param>
    /// <param name="startFrom">起始排序号（0 表示从 1 开始）</param>
    /// <returns>排序号序列</returns>
    IEnumerable<int> GenerateSequence(long parentId, int count, int startFrom = 0);

    /// <summary>
    /// 批量生成排序号序列（主子表结构 - 同一主表下）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="count">需要生成的排序号数量</param>
    /// <param name="startFrom">起始排序号（0 表示从 1 开始）</param>
    /// <returns>排序号序列</returns>
    IEnumerable<int> GenerateSequenceForMaster(long masterId, int count, int startFrom = 0);

    /// <summary>
    /// 批量生成排序号序列（多维度分组结构 - 同一主表 + 同一分组下）
    /// </summary>
    /// <param name="masterId">主表 ID</param>
    /// <param name="groupCode">分组代码</param>
    /// <param name="count">需要生成的排序号数量</param>
    /// <param name="startFrom">起始排序号（0 表示从 1 开始）</param>
    /// <returns>排序号序列</returns>
    IEnumerable<int> GenerateSequenceForGroup(long masterId, string groupCode, int count, int startFrom = 0);
}
