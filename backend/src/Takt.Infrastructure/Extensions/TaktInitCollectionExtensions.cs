// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktInitCollectionExtensions.cs
// 创建时间：2026-06-01
// 创建人：Takt365(Cursor AI)
// 功能描述：应用启动三步初始化（OpenIddict → SqlSugar 建表 → 业务种子）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Data.Seeds;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 应用启动初始化扩展（OpenIddict → 建表 → 种子）
/// </summary>
public static class TaktInitCollectionExtensions
{
    /// <summary>
    /// 执行 Takt 启动初始化：
    /// 1. TaktOpenIddictContext — 认证库
    /// 2. TaktSqlSugarContext — 租户业务库与数据表（<c>InitDb</c>）
    /// 3. TaktSeedContext — 业务种子（<c>SeedData</c>）
    /// </summary>
    /// <param name="app">Web 应用</param>
    /// <param name="initOptions">Init 配置</param>
    public static async Task RunTaktInitializationAsync(
        this WebApplication app,
        TaktInitOptions initOptions)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        TaktLogger.Information("[Init 1/3] TaktOpenIddictContext — 认证数据库");
        var openIddictContext = serviceProvider.GetRequiredService<TaktOpenIddictContext>();
        await openIddictContext.InitializeAsync(serviceProvider);
        TaktLogger.Information("  ✓ OpenIddict 认证库与客户端种子已就绪");

        if (initOptions.InitDb)
        {
            TaktLogger.Information("[Init 2/3] TaktSqlSugarContext — 租户业务库与数据表");
            TaktSqlSugarContext.InitializeAllTenantDatabases(configuration);
            TaktLogger.Information("  ✓ 租户业务库与数据表初始化完成");
        }
        else
        {
            TaktLogger.Information("[Init 2/3] 跳过 TaktSqlSugarContext（InitDb=false）");
        }

        if (initOptions.SeedData)
        {
            if (!initOptions.InitDb)
            {
                TaktLogger.Information("[Init 3/3] InitDb=false，种子写入前校验租户库与业务表...");
            }

            TaktSqlSugarContext.EnsureAllTenantDatabasesReady(configuration);
            TaktLogger.Information("[Init 3/3] TaktSeedContext — 业务种子数据");
            var (insertCount, updateCount) = await TaktSeedContext.InitializeSeedDataAsync(serviceProvider);
            TaktLogger.Information("  ✓ 业务种子完成：插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        }
        else
        {
            TaktLogger.Information("[Init 3/3] 跳过 TaktSeedContext（SeedData=false）");
        }
    }
}
