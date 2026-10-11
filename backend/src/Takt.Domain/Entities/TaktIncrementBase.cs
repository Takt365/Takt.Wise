// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktIncrementBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：自增主键 = 对应租户 Scope 四组合 + Id；公司/审批含 PlantCode
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户组合 4 自增（CoreScope + Id）
/// </summary>
public abstract class TaktTenantCoreEntityIncrementBase : TaktTenantCoreEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }
}

/// <summary>
/// 租户组合 2 自增（CultureScope + Id）
/// </summary>
public abstract class TaktTenantCultureEntityIncrementBase : TaktTenantCultureEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }
}

/// <summary>
/// 租户组合 3 自增（PlantScope + Id）
/// </summary>
public abstract class TaktTenantPlantEntityIncrementBase : TaktTenantPlantEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }
}

/// <summary>
/// 租户组合 1 自增（EntityScope + Id）
/// </summary>
public abstract class TaktTenantEntityIncrementBase : TaktTenantEntityScopeBase, ITaktTenantEntity
{
    /// <summary>
    /// 主键ID（自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }
}

/// <summary>
/// 公司级自增实体基类
/// </summary>
public abstract class TaktCompanyEntityIncrementBase : TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 主键ID（自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 审批级自增实体基类
/// </summary>
public abstract class TaktApprovalEntityIncrementBase : TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 主键ID（自增）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, CreateTableFieldSort = -2)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}
