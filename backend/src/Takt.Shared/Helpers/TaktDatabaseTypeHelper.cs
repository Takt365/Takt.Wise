// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDatabaseTypeHelper.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：appsettings Database:DbType 与 SqlSugar DbType 的全局唯一映射（与 SqlSugar DbType 枚举整型值对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Shared.Helpers;

/// <summary>
/// 数据库类型映射（<c>Database:DbType</c> 整型配置 → SqlSugar DbType 的全局唯一入口）
/// </summary>
public static class TaktDatabaseTypeHelper
{
    public const int ConfiguredMySql = 0;
    public const int ConfiguredSqlServer = 1;
    public const int ConfiguredSqlite = 2;
    public const int ConfiguredOracle = 3;
    public const int ConfiguredPostgreSql = 4;
    public const int ConfiguredDm = 5;
    public const int ConfiguredKdbndp = 6;
    public const int ConfiguredOscar = 7;
    public const int ConfiguredMySqlConnector = 8;
    public const int ConfiguredAccess = 9;
    public const int ConfiguredOpenGauss = 10;
    public const int ConfiguredQuestDb = 11;
    public const int ConfiguredHg = 12;
    public const int ConfiguredClickHouse = 13;
    public const int ConfiguredGBase = 14;
    public const int ConfiguredOdbc = 15;
    public const int ConfiguredOceanBaseForOracle = 16;
    public const int ConfiguredTDengine = 17;
    public const int ConfiguredGaussDb = 18;
    public const int ConfiguredOceanBase = 19;
    public const int ConfiguredTidb = 20;
    public const int ConfiguredVastbase = 21;
    public const int ConfiguredPolarDb = 22;
    public const int ConfiguredDoris = 23;
    public const int ConfiguredXugu = 24;
    public const int ConfiguredGoldenDb = 25;
    public const int ConfiguredTdsqlForPgOdbc = 26;
    public const int ConfiguredTdsql = 27;
    public const int ConfiguredHana = 28;
    public const int ConfiguredDb2 = 29;
    public const int ConfiguredGaussDbNative = 30;
    public const int ConfiguredDuckDb = 31;
    public const int ConfiguredMongoDb = 32;
    public const int ConfiguredCustom = 900;

    /// <summary>
    /// 将 <c>Database:DbType</c> 配置值解析为 SqlSugar 数据库类型（全工程唯一 switch）
    /// </summary>
    /// <param name="configuredDbType"><c>appsettings Database:DbType</c> 整型值</param>
    /// <returns>SqlSugar 数据库类型</returns>
    /// <exception cref="InvalidOperationException">不支持的数据库类型配置值</exception>
    public static DbType ResolveSugarDbType(int configuredDbType) =>
        configuredDbType switch
        {
            ConfiguredMySql => DbType.MySql,
            ConfiguredSqlServer => DbType.SqlServer,
            ConfiguredSqlite => DbType.Sqlite,
            ConfiguredOracle => DbType.Oracle,
            ConfiguredPostgreSql => DbType.PostgreSQL,
            ConfiguredDm => DbType.Dm,
            ConfiguredKdbndp => DbType.Kdbndp,
            ConfiguredOscar => DbType.Oscar,
            ConfiguredMySqlConnector => DbType.MySqlConnector,
            ConfiguredAccess => DbType.Access,
            ConfiguredOpenGauss => DbType.OpenGauss,
            ConfiguredQuestDb => DbType.QuestDB,
            ConfiguredHg => DbType.HG,
            ConfiguredClickHouse => DbType.ClickHouse,
            ConfiguredGBase => DbType.GBase,
            ConfiguredOdbc => DbType.Odbc,
            ConfiguredOceanBaseForOracle => DbType.OceanBaseForOracle,
            ConfiguredTDengine => DbType.TDengine,
            ConfiguredGaussDb => DbType.GaussDB,
            ConfiguredOceanBase => DbType.OceanBase,
            ConfiguredTidb => DbType.Tidb,
            ConfiguredVastbase => DbType.Vastbase,
            ConfiguredPolarDb => DbType.PolarDB,
            ConfiguredDoris => DbType.Doris,
            ConfiguredXugu => DbType.Xugu,
            ConfiguredGoldenDb => DbType.GoldenDB,
            ConfiguredTdsqlForPgOdbc => DbType.TDSQLForPGODBC,
            ConfiguredTdsql => DbType.TDSQL,
            ConfiguredHana => DbType.HANA,
            ConfiguredDb2 => DbType.DB2,
            ConfiguredGaussDbNative => DbType.GaussDBNative,
            ConfiguredDuckDb => DbType.DuckDB,
            ConfiguredMongoDb => DbType.MongoDb,
            ConfiguredCustom => DbType.Custom,
            _ => throw new InvalidOperationException($"不支持的数据库类型: {configuredDbType}"),
        };
}
