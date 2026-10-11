// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktSnowflakeBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：雪花主键 = 对应租户 Scope 四组合 + Id；公司/审批含 PlantCode
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户组合 4：CoreScope + Id（雪花主键）
/// </summary>
public abstract class TaktTenantCoreEntityBase : TaktTenantCoreEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}

/// <summary>
/// 租户组合 2：CultureScope + Id（雪花主键）
/// </summary>
public abstract class TaktTenantCultureEntityBase : TaktTenantCultureEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}

/// <summary>
/// 租户组合 3：PlantScope + Id（雪花主键）
/// </summary>
public abstract class TaktTenantPlantEntityBase : TaktTenantPlantEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}

/// <summary>
/// 租户组合 1：EntityScope + Id（默认雪花主键）
/// </summary>
public abstract class TaktTenantEntityBase : TaktTenantEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }
}

/// <summary>
/// 公司级实体基类（雪花主键）
/// </summary>
public abstract class TaktCompanyEntityBase : TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 主键ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 审批级实体基类（雪花主键）
/// </summary>
public abstract class TaktApprovalEntityBase : TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 主键ID（雪花）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public virtual long Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}
