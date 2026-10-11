// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktFtpOptions.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：FTP 配置选项，绑定 appsettings <c>Ftp:{provider}</c>（与字典 sys_ftp_provider 一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 单个 FTP 提供商配置（如 <c>Ftp:teac_cn</c>、<c>Ftp:teac_jp</c>）
/// </summary>
public class TaktFtpOptions
{
    /// <summary>
    /// 配置根节名称
    /// </summary>
    public const string SectionName = "Ftp";

    /// <summary>
    /// FTP 服务器地址
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// FTP 端口
    /// </summary>
    public int Port { get; set; } = 21;

    /// <summary>
    /// FTP 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// FTP 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 是否使用 SSL/TLS
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// 连接超时时间（秒）
    /// </summary>
    public int Timeout { get; set; } = 30;

    /// <summary>
    /// 基础路径（FTP 服务器上的基础目录，可选）
    /// </summary>
    public string? BasePath { get; set; }

    /// <summary>
    /// 校验 FTP 提供商配置
    /// </summary>
    /// <param name="provider">提供商标识（用于异常信息）</param>
    public void Validate(string provider)
    {
        if (string.IsNullOrWhiteSpace(Host))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:Host 不能为空");
        }

        if (Port <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:Port 必须大于 0");
        }

        if (string.IsNullOrWhiteSpace(UserName))
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:UserName 不能为空");
        }

        if (Timeout <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:{provider}:Timeout 必须大于 0");
        }
    }
}
