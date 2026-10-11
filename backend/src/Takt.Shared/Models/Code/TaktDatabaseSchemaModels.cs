// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Code
// 文件名称：TaktDatabaseSchemaModels.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：租户库 Schema introspect 模型（供 ITaktDatabaseSchemaProvider 返回）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Models.Code;

/// <summary>
/// 可 introspect 的租户业务库摘要
/// </summary>
public class TaktDatabaseInfo
{
    /// <summary>
    /// 租户编码（3 位）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据库展示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}

/// <summary>
/// 物理表列 Schema 摘要
/// </summary>
public class TaktDatabaseTableColumnInfo
{
    /// <summary>
    /// 数据库列名
    /// </summary>
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列注释
    /// </summary>
    public string? ColumnComment { get; set; }

    /// <summary>
    /// 数据库数据类型
    /// </summary>
    public string DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// 小数位数
    /// </summary>
    public int DecimalDigits { get; set; }

    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPrimaryKey { get; set; }

    /// <summary>
    /// 是否自增
    /// </summary>
    public bool IsIdentity { get; set; }

    /// <summary>
    /// 是否可空
    /// </summary>
    public bool IsNullable { get; set; }
}
