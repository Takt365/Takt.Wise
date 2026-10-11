// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktPrimaryKeyTypeOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：主键类型配置选项（仓储根据实体字段类型自动判断）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 主键类型配置选项（与实体 <c>Id</c> 字段类型配对，由 TaktPrimaryKeyInsertHelper 统一执行）
/// </summary>
public class PrimaryKeyTypeOptions
{
    public const string SectionName = "PrimaryKeyType";

    /// <summary>
    /// 数据库自增 ID（Takt*EntityIncrementBase）
    /// </summary>
    public IdentityIdOptions Identity { get; set; } = new() { Enabled = true };

    /// <summary>
    /// GUID 主键（Takt*EntityGuidBase）
    /// </summary>
    public GuidIdOptions Guid { get; set; } = new() { Enabled = true };

    /// <summary>
    /// 雪花 ID（TaktTenantEntityBase / TaktCompanyEntityBase / TaktApprovalEntityBase）
    /// </summary>
    public SnowflakeIdOptions Snowflake { get; set; } = new() { Enabled = true, WorkId = 1 };

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (Snowflake.Enabled && (Snowflake.WorkId < 0 || Snowflake.WorkId > 63))
        {
            throw new InvalidOperationException(
                $"{SectionName}:Snowflake:WorkId 必须在 0-63 范围内，当前值={Snowflake.WorkId}");
        }
    }
}

/// <summary>
/// 数据库自增ID配置
/// </summary>
public class IdentityIdOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}

/// <summary>
/// GUID配置
/// </summary>
public class GuidIdOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}

/// <summary>
/// 雪花ID配置
/// </summary>
public class SnowflakeIdOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 工作机器ID（0-63，必须唯一）
    /// </summary>
    public long WorkId { get; set; }
}
