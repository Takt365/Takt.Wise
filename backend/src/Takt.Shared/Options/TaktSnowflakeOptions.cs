// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktSnowflakeOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：雪花算法配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 雪花算法配置选项
/// </summary>
public class TaktSnowflakeOptions
{
    public const string SectionName = "Snowflake";

    /// <summary>
    /// 是否启用雪花算法
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 工作机器ID（0-63）
    /// </summary>
    public long WorkId { get; set; }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (Enabled && (WorkId < 0 || WorkId > 63))
        {
            throw new InvalidOperationException($"{SectionName}:WorkId 必须在 0-63 之间");
        }
    }
}
