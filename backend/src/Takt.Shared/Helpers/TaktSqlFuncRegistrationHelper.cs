// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSqlFuncRegistrationHelper.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SqlSugar ConnectionConfig 统一注册仓储层 SqlFunc 扩展
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Shared.Helpers;

/// <summary>
/// SqlSugar 自定义 SqlFunc 注册（连接创建时调用）
/// </summary>
public static class TaktSqlFuncRegistrationHelper
{
    /// <summary>
    /// 向 ConnectionConfig 注册仓储聚合 SqlFunc（幂等）
    /// </summary>
    /// <param name="config">SqlSugar 连接配置</param>
    public static void ApplyRepositorySqlFuncExtensions(ConnectionConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        config.ConfigureExternalServices ??= new ConfigureExternalServices();
        var services = config.ConfigureExternalServices.SqlFuncServices ?? [];
        if (services.All(item => !string.Equals(item.UniqueMethodName, TaktSqlFuncMedian.UniqueMethodName, StringComparison.Ordinal)))
        {
            services.Add(TaktSqlFuncMedian.CreateSqlFuncExternal());
        }

        config.ConfigureExternalServices.SqlFuncServices = services;
    }

    /// <summary>
    /// 向 ConnectionConfig 注册仓储聚合 SqlFunc（幂等）
    /// </summary>
    /// <param name="config">SqlSugar 连接配置</param>
    /// <returns>同一 config 实例（便于链式创建客户端）</returns>
    public static ConnectionConfig ApplyAndReturn(ConnectionConfig config)
    {
        ApplyRepositorySqlFuncExtensions(config);
        return config;
    }
}
