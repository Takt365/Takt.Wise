// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPagedClamp.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：分页参数规范化（页码、页大小 clamp，Skip 算术安全；数值来自 appsettings Paged）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 分页参数规范化工具（启动期 Configure 注入 appsettings Paged，再供全栈只读使用）
/// </summary>
/// <remarks>
/// 非纯工具网关：DefaultPageSize 等由 Program 启动时 Configure 写入；运维仅改 appsettings Paged 节。
/// </remarks>
public static class TaktPagedClamp
{
    private static bool _configured;

    /// <summary>
    /// 列表接口允许的最大每页条数
    /// </summary>
    public static int DefaultMaxPageSize { get; private set; } = 100;

    /// <summary>
    /// 默认每页条数
    /// </summary>
    public static int DefaultPageSize { get; private set; } = 20;

    /// <summary>
    /// 默认页码（从 1 开始）
    /// </summary>
    public static int DefaultPageIndex { get; private set; } = 1;

    /// <summary>
    /// 前端可选每页条数
    /// </summary>
    public static int[] PageSizeOptions { get; private set; } = [10, 20, 50, 100];

    /// <summary>
    /// 从 appsettings Paged 节注入运行时默认值（Program 启动时调用一次）
    /// </summary>
    /// <param name="options">已校验的分页配置</param>
    /// <exception cref="ArgumentNullException">options 为空</exception>
    /// <exception cref="InvalidOperationException">重复 Configure</exception>
    public static void Configure(TaktPagedOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (_configured)
        {
            throw new InvalidOperationException("TaktPagedClamp 已配置，禁止重复 Configure");
        }

        options.Validate();
        DefaultPageIndex = options.DefaultPageIndex;
        DefaultPageSize = options.DefaultPageSize;
        DefaultMaxPageSize = options.MaxPageSize;
        PageSizeOptions = options.PageSizeOptions.ToArray();
        _configured = true;
    }

    /// <summary>
    /// 规范化页码（最小为 1）
    /// </summary>
    /// <param name="pageIndex">原始页码</param>
    /// <returns>不小于 1 的页码</returns>
    public static int NormalizePageIndex(int pageIndex) => Math.Max(1, pageIndex);

    /// <summary>
    /// 规范化每页大小（限制在 1～<paramref name="maxPageSize"/>）
    /// </summary>
    /// <param name="pageSize">原始每页大小</param>
    /// <param name="maxPageSize">上限；为 null 时使用 DefaultMaxPageSize</param>
    /// <returns>规范化后的每页大小</returns>
    public static int NormalizePageSize(int pageSize, int? maxPageSize = null)
    {
        var cap = maxPageSize ?? DefaultMaxPageSize;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cap);
        if (pageSize <= 0)
        {
            return DefaultPageSize;
        }

        return Math.Min(cap, pageSize);
    }

    /// <summary>
    /// 计算分页 Skip 偏移（checked 防算术溢出）
    /// </summary>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <returns>Skip 行数</returns>
    /// <exception cref="OverflowException">偏移计算溢出时抛出</exception>
    public static int ComputeSkip(int pageIndex, int pageSize)
    {
        var normalizedIndex = NormalizePageIndex(pageIndex);
        var normalizedSize = NormalizePageSize(pageSize);
        return checked((normalizedIndex - 1) * normalizedSize);
    }
}
