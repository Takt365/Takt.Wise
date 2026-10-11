// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktGuidBase.cs
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：GUID 主键 = 对应租户 Scope 四组合 + Id；公司/审批含 PlantCode
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities;

/// <summary>
/// 租户组合 4 GUID（CoreScope + Id）
/// </summary>
public abstract class TaktTenantCoreEntityGuidBase : TaktTenantCoreEntityScopeBase
{
    /// <summary>
    /// 主键ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    public Guid Id { get; set; }
}

/// <summary>
/// 租户组合 2 GUID（CultureScope + Id）
/// </summary>
public abstract class TaktTenantCultureEntityGuidBase : TaktTenantCultureEntityScopeBase
{
    /// <summary>
    /// 主键ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    public Guid Id { get; set; }
}

/// <summary>
/// 租户组合 3 GUID（PlantScope + Id）
/// </summary>
public abstract class TaktTenantPlantEntityGuidBase : TaktTenantPlantEntityScopeBase
{
    /// <summary>
    /// 主键ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    public Guid Id { get; set; }
}

/// <summary>
/// 租户组合 1 GUID（EntityScope + Id）
/// </summary>
public abstract class TaktTenantEntityGuidBase : TaktTenantEntityScopeBase
{
    /// <summary>
    /// 主键ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    public Guid Id { get; set; }
}

/// <summary>
/// 公司级 GUID 实体基类
/// </summary>
public abstract class TaktCompanyEntityGuidBase : TaktCompanyEntityScopeBase
{
    /// <summary>
    /// 主键ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    public Guid Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}

/// <summary>
/// 审批级 GUID 实体基类
/// </summary>
public abstract class TaktApprovalEntityGuidBase : TaktApprovalEntityScopeBase
{
    /// <summary>
    /// 主键ID（GUID）
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "uniqueidentifier", IsPrimaryKey = true, IsNullable = false, CreateTableFieldSort = -2)]
    public Guid Id { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, CreateTableFieldSort = -1)]
    public string PlantCode { get; set; } = string.Empty;
}
