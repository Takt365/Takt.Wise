// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktSystemOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：系统配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 系统配置选项
/// </summary>
public class TaktSystemOptions
{
    public const string SectionName = "System";

    /// <summary>
    /// 是否启用单点登录
    /// </summary>
    public bool SingleLogin { get; set; }

    /// <summary>
    /// 是否显示数据库日志
    /// </summary>
    public bool ShowDbLog { get; set; }

    /// <summary>
    /// 是否演示模式
    /// </summary>
    public bool DemoMode { get; set; }

    /// <summary>
    /// 运行环境（Development/Staging/Production）
    /// </summary>
    public string Environment { get; set; } = "Development";

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Environment))
        {
            throw new InvalidOperationException($"{SectionName}:Environment 不能为空");
        }
    }
}
