// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Models
// 文件名称：TaktPagedQuery.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：分页查询基类与全局配置 DTO（Query 入参 / Config 由 Platform API 下发）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models;

using Takt.Shared.Helpers;

/// <summary>
/// Takt通用分页查询基类
/// </summary>
public class TaktPagedQuery
{
    /// <summary>
    /// 当前页码（从1开始，默认见 appsettings Paged:DefaultPageIndex）
    /// </summary>
    public int PageIndex { get; set; } = TaktPagedClamp.DefaultPageIndex;

    /// <summary>
    /// 每页大小（默认见 appsettings Paged:DefaultPageSize）
    /// </summary>
    public int PageSize { get; set; } = TaktPagedClamp.DefaultPageSize;

    /// <summary>
    /// 关键词（用于模糊查询，在多个字段中搜索）
    /// </summary>
    public string? KeyWords { get; set; }
}

/// <summary>
/// 分页全局配置（公开只读，来源 appsettings Paged，供前端 bootstrap）
/// </summary>
public class TaktPagedConfigDto
{
    /// <summary>
    /// 默认页码（从 1 开始）
    /// </summary>
    public int DefaultPageIndex { get; set; }

    /// <summary>
    /// 默认每页条数
    /// </summary>
    public int DefaultPageSize { get; set; }

    /// <summary>
    /// 列表接口 pageSize 上限
    /// </summary>
    public int MaxPageSize { get; set; }

    /// <summary>
    /// 前端可选每页条数
    /// </summary>
    public int[] PageSizeOptions { get; set; } = [];
}