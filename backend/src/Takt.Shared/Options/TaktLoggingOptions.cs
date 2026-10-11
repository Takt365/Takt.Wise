// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktLoggingOptions.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一日志配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Options;

/// <summary>
/// 统一日志配置（与 appsettings.json TaktLogging 节点对应）
/// </summary>
public class TaktLoggingOptions
{
    public const string SectionName = "TaktLogging";

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; } = "TaktWise";

    /// <summary>
    /// 应用版本
    /// </summary>
    public string AppVersion { get; set; } = "1.0.0";

    /// <summary>
    /// 运行环境
    /// </summary>
    public string Environment { get; set; } = "Development";

    /// <summary>
    /// 最低采集级别
    /// </summary>
    public TaktLogLevel MinLevel { get; set; } = TaktLogLevel.Info;

    /// <summary>
    /// 是否启用远端上报
    /// </summary>
    public bool EnableRemoteReport { get; set; }

    /// <summary>
    /// 远端上报地址（POST JSON）
    /// </summary>
    public string RemoteReportUrl { get; set; } = string.Empty;

    /// <summary>
    /// 批量上报条数阈值
    /// </summary>
    public int BatchSize { get; set; } = 20;

    /// <summary>
    /// 定时 flush 间隔（毫秒）
    /// </summary>
    public int FlushIntervalMs { get; set; } = 10000;

    /// <summary>
    /// 控制台输出模板（Serilog outputTemplate）
    /// </summary>
    public string ConsoleOutputTemplate { get; set; } =
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 文件输出模板（Serilog outputTemplate）
    /// </summary>
    public string FileOutputTemplate { get; set; } =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(AppName))
        {
            throw new InvalidOperationException($"{SectionName}:AppName 不能为空");
        }

        if (string.IsNullOrWhiteSpace(AppVersion))
        {
            throw new InvalidOperationException($"{SectionName}:AppVersion 不能为空");
        }

        if (string.IsNullOrWhiteSpace(Environment))
        {
            throw new InvalidOperationException($"{SectionName}:Environment 不能为空");
        }

        if (string.IsNullOrWhiteSpace(ConsoleOutputTemplate))
        {
            throw new InvalidOperationException($"{SectionName}:ConsoleOutputTemplate 不能为空");
        }

        if (string.IsNullOrWhiteSpace(FileOutputTemplate))
        {
            throw new InvalidOperationException($"{SectionName}:FileOutputTemplate 不能为空");
        }

        if (BatchSize <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:BatchSize 必须大于 0");
        }

        if (FlushIntervalMs <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:FlushIntervalMs 必须大于 0");
        }

        if (EnableRemoteReport && string.IsNullOrWhiteSpace(RemoteReportUrl))
        {
            throw new InvalidOperationException($"{SectionName}:RemoteReportUrl 在启用远端上报时不能为空");
        }
    }
}
