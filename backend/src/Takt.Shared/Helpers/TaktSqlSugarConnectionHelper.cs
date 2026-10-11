// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSqlSugarConnectionHelper.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SqlSugar 租户连接配置统一工厂；全局开启 CodeFirst IsCreateTableFiledSort，使基类 CreateTableFieldSort（Id=-2、PlantCode/RelatedPlant=-1）在建表时生效
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Shared.Helpers;

/// <summary>
/// SqlSugar 连接配置工厂（避免各处重复拼装 ConnectionConfig）
/// </summary>
public static class TaktSqlSugarConnectionHelper
{
    /// <summary>
    /// 创建租户 SqlSugar 连接配置
    /// </summary>
    /// <param name="sugarDbType">已解析的 SqlSugar 数据库类型（来自 TaktDatabaseOptions.SugarDbType）</param>
    /// <param name="tenantCode">租户编码（默认 ConfigId）</param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="registerRepositorySqlFunc">是否注册仓储聚合 SqlFunc 扩展</param>
    /// <param name="configId">连接 ConfigId；空则用租户编码。Quartz 切库须用独立 Id，避免与请求租户客户端共用静态连接</param>
    /// <returns>ConnectionConfig</returns>
    public static ConnectionConfig CreateConnectionConfig(
        DbType sugarDbType,
        string tenantCode,
        string connectionString,
        bool registerRepositorySqlFunc = true,
        string? configId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        var resolvedConfigId = string.IsNullOrWhiteSpace(configId) ? tenantCode.Trim() : configId.Trim();
        var config = new ConnectionConfig
        {
            ConfigId = resolvedConfigId,
            DbType = sugarDbType,
            ConnectionString = connectionString,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings
            {
                SqlServerCodeFirstNvarchar = true,
            },
        };
        ApplyCreateTableFieldSort(config);
        if (registerRepositorySqlFunc)
        {
            TaktSqlFuncRegistrationHelper.ApplyRepositorySqlFuncExtensions(config);
        }

        return config;
    }

    /// <summary>
    /// 全局开启建表字段排序（SqlSugar 默认关闭；未开启时 CreateTableFieldSort 无效）
    /// </summary>
    /// <param name="config">SqlSugar 连接配置</param>
    private static void ApplyCreateTableFieldSort(ConnectionConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        config.ConfigureExternalServices ??= new ConfigureExternalServices();
        var previous = config.ConfigureExternalServices.EntityNameService;
        config.ConfigureExternalServices.EntityNameService = (type, entity) =>
        {
            previous?.Invoke(type, entity);
            // SqlSugar 属性名拼写为 Filed（非 Field）
            entity.IsCreateTableFiledSort = true;
        };
    }

    /// <summary>
    /// 创建租户 SqlSugar 客户端
    /// </summary>
    /// <param name="sugarDbType">已解析的 SqlSugar 数据库类型</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="registerRepositorySqlFunc">是否注册仓储聚合 SqlFunc 扩展</param>
    /// <param name="configId">连接 ConfigId；空则用租户编码</param>
    /// <returns>SqlSugar 客户端</returns>
    public static SqlSugarClient CreateClient(
        DbType sugarDbType,
        string tenantCode,
        string connectionString,
        bool registerRepositorySqlFunc = true,
        string? configId = null) =>
        new(CreateConnectionConfig(
            sugarDbType,
            tenantCode,
            connectionString,
            registerRepositorySqlFunc,
            configId));
}
