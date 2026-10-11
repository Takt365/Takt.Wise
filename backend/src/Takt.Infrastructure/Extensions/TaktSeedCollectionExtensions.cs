// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktSeedCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据服务注册扩展方法
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Data.Seeds;
using Takt.Infrastructure.Data.Seeds.EntitySeedData;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 种子数据服务扩展方法
/// 注意：种子数据类由 Autofac 自动扫描注册（TaktAutofacModule），无需在此重复注册
/// </summary>
public static class TaktSeedCollectionExtensions
{
    /// <summary>
    /// 初始化业务种子数据（步骤 3，不含 OpenIddict）
    /// </summary>
    /// <param name="app">应用程序</param>
    /// <returns>异步任务</returns>
    public static Task InitializeTaktSeedDataAsync(this WebApplication app)
    {
        return TaktSeedContext.InitializeSeedDataAsync(app.Services);
    }
}
