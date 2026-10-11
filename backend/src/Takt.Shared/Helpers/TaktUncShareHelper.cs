// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktUncShareHelper.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：Windows UNC 共享临时挂载（WNetAddConnection2）；有凭据时供浏览/IO，Dispose 时断开
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Takt.Shared.Helpers;

/// <summary>
/// UNC 共享连接辅助（Windows；副作用 I/O：挂载/断开网络驱动器会话）
/// </summary>
/// <remarks>
/// 非纯工具：调用 WNet API。无凭据时返回空连接（依赖进程账号）。仅 Windows 有效。
/// </remarks>
public static class TaktUncShareHelper
{
    private const int ResourceTypeDisk = 0x1;
    private const int ConnectTemporary = 0x00000004;
    private const int ErrorAlreadyAssigned = 85;
    private const int ErrorSessionCredentialConflict = 1219;
    private const int NoError = 0;

    /// <summary>
    /// 解析 UNC 路径的共享根（\\server\share）
    /// </summary>
    /// <param name="uncPath">完整 UNC 路径</param>
    /// <returns>共享根路径</returns>
    /// <exception cref="ArgumentException">非 UNC 或缺少共享名</exception>
    public static string GetShareRoot(string uncPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uncPath);
        var trimmed = uncPath.Trim().Replace('/', '\\').TrimEnd('\\');
        if (!trimmed.StartsWith(@"\\", StringComparison.Ordinal))
        {
            throw new ArgumentException("路径须为 UNC（以 \\\\ 开头）", nameof(uncPath));
        }
        var body = trimmed[2..];
        var parts = body.Split('\\', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            throw new ArgumentException("UNC 须包含服务器与共享名", nameof(uncPath));
        }
        return $@"\\{parts[0]}\{parts[1]}";
    }

    /// <summary>
    /// 规范化 UNC 路径（统一反斜杠、去掉末尾分隔符）
    /// </summary>
    /// <param name="uncPath">原始路径</param>
    /// <returns>规范化路径</returns>
    public static string NormalizeUncPath(string uncPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uncPath);
        return uncPath.Trim().Replace('/', '\\').TrimEnd('\\');
    }

    /// <summary>
    /// 使用可选凭据临时连接 UNC 共享；Dispose 时取消连接
    /// </summary>
    /// <param name="uncPath">任意位于共享下的 UNC 路径</param>
    /// <param name="UserName">用户名（可含 DOMAIN\user）；空则不挂载</param>
    /// <param name="password">密码；与用户名同时为空则不挂载</param>
    /// <returns>连接作用域；须 using</returns>
    /// <exception cref="ArgumentException">路径非法</exception>
    /// <exception cref="InvalidOperationException">挂载失败（非 Windows 或 WNet 错误）</exception>
    [SupportedOSPlatform("windows")]
    public static IDisposable Connect(string uncPath, string? UserName, string? password)
    {
        var normalized = NormalizeUncPath(uncPath);
        var shareRoot = GetShareRoot(normalized);
        if (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(password))
        {
            return NullScope.Instance;
        }
        if (!OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException("带凭据的 UNC 浏览仅支持 Windows 宿主");
        }
        ConnectShare(shareRoot, UserName?.Trim(), password ?? string.Empty);
        return new UncConnectionScope(shareRoot);
    }

    [SupportedOSPlatform("windows")]
    private static void ConnectShare(string shareRoot, string? UserName, string password)
    {
        var resource = new NetResource
        {
            dwType = ResourceTypeDisk,
            lpRemoteName = shareRoot,
        };
        var result = WNetAddConnection2(ref resource, password, UserName, ConnectTemporary);
        if (result == NoError || result == ErrorAlreadyAssigned)
        {
            return;
        }
        if (result == ErrorSessionCredentialConflict)
        {
            WNetCancelConnection2(shareRoot, 0, true);
            result = WNetAddConnection2(ref resource, password, UserName, ConnectTemporary);
            if (result == NoError || result == ErrorAlreadyAssigned)
            {
                return;
            }
        }
        throw new InvalidOperationException($"无法连接网络共享 {shareRoot}（WNet 错误码 {result}）");
    }

    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    private static extern int WNetAddConnection2(
        ref NetResource lpNetResource,
        string lpPassword,
        string? lpUserName,
        int dwFlags);

    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct NetResource
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        public string? lpLocalName;
        public string? lpRemoteName;
        public string? lpComment;
        public string? lpProvider;
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose()
        {
        }
    }

    private sealed class UncConnectionScope : IDisposable
    {
        private readonly string _shareRoot;
        private bool _disposed;

        public UncConnectionScope(string shareRoot)
        {
            _shareRoot = shareRoot;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            if (OperatingSystem.IsWindows())
            {
                WNetCancelConnection2(_shareRoot, 0, true);
            }
        }
    }
}
