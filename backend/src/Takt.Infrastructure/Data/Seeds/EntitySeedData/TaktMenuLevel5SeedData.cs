// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuLevel5SeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 五级菜单种子数据协调占位（产出/不良等已扁平至四级，无五级菜单种子项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 五级菜单种子数据。
/// <para>由 TaktMenuSeedData 统一协调调用；当前无五级菜单种子项。</para>
/// </summary>
public class TaktMenuLevel5SeedData
{
    /// <summary>
    /// 初始化五级菜单种子数据（当前无条目，保留层级扩展点）。
    /// </summary>
    /// <param name="serviceProvider">服务提供者。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)。</returns>
    public Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过五级菜单种子数据初始化");
            return Task.FromResult((0, 0));
        }
        return Task.FromResult((0, 0));
    }
}
