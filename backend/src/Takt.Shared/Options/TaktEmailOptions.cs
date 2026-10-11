// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktEmailOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：邮件配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 邮件配置选项
/// </summary>
public class TaktEmailOptions
{
    public const string SectionName = "Email";

    /// <summary>
    /// SMTP 服务器地址
    /// </summary>
    public string SmtpHost { get; set; } = string.Empty;

    /// <summary>
    /// SMTP 端口
    /// </summary>
    public int SmtpPort { get; set; } = 587;

    /// <summary>
    /// SMTP 用户名
    /// </summary>
    public string SmtpUserName { get; set; } = string.Empty;

    /// <summary>
    /// SMTP 密码
    /// </summary>
    public string SmtpPassword { get; set; } = string.Empty;

    /// <summary>
    /// 发件人邮箱
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

    /// <summary>
    /// 发件人名称
    /// </summary>
    public string FromName { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用 SSL
    /// </summary>
    public bool EnableSsl { get; set; } = true;

    /// <summary>
    /// 是否跳过 SSL 证书验证
    /// </summary>
    public bool SkipSslCertificateValidation { get; set; }

    /// <summary>
    /// 是否启用附件
    /// </summary>
    public bool EnableAttachments { get; set; } = true;

    /// <summary>
    /// 最大附件大小（MB）
    /// </summary>
    public int MaxAttachmentSizeMB { get; set; } = 25;

    /// <summary>
    /// 最大邮件大小（MB）
    /// </summary>
    public int MaxEmailSizeMB { get; set; } = 50;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (SmtpPort <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:SmtpPort 必须大于 0");
        }

        if (MaxAttachmentSizeMB <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:MaxAttachmentSizeMB 必须大于 0");
        }

        if (MaxEmailSizeMB <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:MaxEmailSizeMB 必须大于 0");
        }
    }
}
