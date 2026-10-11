// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Models
// 文件名称：TaktPagedResult.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt通用分页结果，提供标准化的分页数据返回格式
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models;

using Takt.Shared.Helpers;

/// <summary>
/// Takt通用分页结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class TaktPagedResult<T>
{
    /// <summary>
    /// 数据列表
    /// </summary>
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// 总记录数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 当前页码（从1开始）
    /// </summary>
    public int PageIndex { get; set; } = TaktPagedClamp.DefaultPageIndex;

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; } = TaktPagedClamp.DefaultPageSize;

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(Total / (double)PageSize) : 0;

    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// 创建分页结果
    /// </summary>
    /// <param name="data">数据列表</param>
    /// <param name="total">总记录数</param>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <returns>分页结果</returns>
    public static TaktPagedResult<T> Create(List<T> data, int total, int pageIndex, int pageSize)
    {
        return new TaktPagedResult<T>
        {
            Data = data,
            Total = total,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}