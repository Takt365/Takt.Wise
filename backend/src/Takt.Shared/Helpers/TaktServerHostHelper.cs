// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktServerHostHelper.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 执行日志 ExecuteIp/ExecuteHost（本机固定值；无 DNS；与客户端 IP 无关）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// Quartz 执行机标识。任务始终在本机 WebApi/Host 进程内执行，IP/主机名写死，不做网络解析。
/// 客户端 IP 见 TaktLocationHelper / HttpContext，勿混用本类。
/// </summary>
public static class TaktServerHostHelper
{
    /// <summary>本机回环 IPv4（Quartz ExecuteIp 固定值）</summary>
    public const string LocalLoopbackIPv4 = "127.0.0.1";

    /// <summary>
    /// 本机主机名（Environment.MachineName）
    /// </summary>
    /// <returns>主机名；为空时返回空串</returns>
    public static string ResolveLocalMachineName()
    {
        return Environment.MachineName ?? string.Empty;
    }

    /// <summary>
    /// 本机 IPv4（固定 127.0.0.1）
    /// </summary>
    /// <returns>127.0.0.1</returns>
    public static string ResolveLocalIPv4()
    {
        return LocalLoopbackIPv4;
    }

    /// <summary>
    /// Quartz 任务执行日志：执行机 IP + 主机名
    /// </summary>
    /// <returns>ExecuteIp=127.0.0.1，ExecuteHost=本机名</returns>
    public static (string ExecuteIp, string ExecuteHost) ResolveQuartzExecuteEndpoint()
    {
        return (LocalLoopbackIPv4, ResolveLocalMachineName());
    }
}
