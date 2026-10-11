// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Context
// 文件名称：TaktOpenIddictContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenIddict Entity Framework Core 数据库上下文
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Takt.Infrastructure.Data.Seeds.EntitySeedData;

namespace Takt.Infrastructure.Data.Context;

/// <summary>
/// OpenIddict数据库上下文
/// 用于存储OpenIddict的应用、授权、令牌等数据
/// 注意：这个Context仅用于OpenIddict内部存储，与主数据库（SqlSugar）分离
/// </summary>
public class TaktOpenIddictContext : DbContext
{
    /// <summary>
    /// 初始化 OpenIddict 数据库上下文
    /// </summary>
    /// <param name="options">EF Core 上下文选项（由 DI 注入）</param>
    public TaktOpenIddictContext(DbContextOptions<TaktOpenIddictContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 配置 OpenIddict 实体模型映射
    /// </summary>
    /// <param name="modelBuilder">EF Core 模型构建器</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseOpenIddict();
    }

    /// <summary>
    /// 初始化 OpenIddict 认证库（步骤 1：建库/建表 + OpenIddict 应用与范围种子）
    /// </summary>
    /// <param name="serviceProvider">服务提供者（用于 OpenIddict 种子）</param>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task InitializeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        await Database.EnsureCreatedAsync(cancellationToken);
        await TaktOpenIddictSeedData.InitializeAsync(serviceProvider);
    }
}
