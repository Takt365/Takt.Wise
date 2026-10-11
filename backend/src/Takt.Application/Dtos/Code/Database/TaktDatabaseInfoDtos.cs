// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Database
// 文件名称：TaktDatabaseInfoDtos.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库摘要、数据库表摘要、数据库表列摘要 introspect DTO（代码生成、Schema 工具等共用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Code.Database;

using Takt.Shared.Models;

/// <summary>
/// 数据库摘要（可连接租户业务库，与 appsettings Database:TenantCodes / ConnectionStrings:Tenant_* 对齐）
/// </summary>
public class TaktDatabaseInfoDto
{
    /// <summary>
    /// 租户编码（3 位，如 000、100）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据库展示名称（连接串 Database= 段，如 Takt_000_Dev）
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}

/// <summary>
/// 数据库表摘要（指定租户库下物理表 introspect 结果，用于选表导入）
/// </summary>
public class TaktDatabaseTableInfoDto
{
    /// <summary>
    /// 数据表名称
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; }
}

/// <summary>
/// 数据库表摘要分页查询（租户取自当前登录上下文，页内不可切换）
/// </summary>
public class TaktDatabaseTableInfoQueryDto : TaktPagedQuery
{
}

/// <summary>
/// 数据库表列摘要（指定物理表列 introspect 结果）
/// </summary>
public class TaktDatabaseTableColumnInfoDto
{
    /// <summary>
    /// 数据库列名称（snake_case，如 user_name）
    /// </summary>
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（列注释）
    /// </summary>
    public string? ColumnComment { get; set; }

    /// <summary>
    /// 数据库数据类型（如 nvarchar、int、datetime）
    /// </summary>
    public string DatabaseDataType { get; set; } = string.Empty;

    /// <summary>
    /// 长度（字符串长度或数值整数位）
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
