// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktMailHelper.cs
// 创建时间：2025-01-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt邮件帮助类，使用 MailKit 发送邮件
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Takt.Shared.Options;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Takt.Shared.Helpers;

/// <summary>
/// Takt邮件帮助类
/// </summary>
/// <remarks>网络 I/O 网关；所有发送方法须显式传入 IConfiguration 与收件人参数。</remarks>
public static class TaktMailHelper
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="to">收件人邮箱</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="isHtml">是否为HTML格式，默认为true</param>
    /// <returns>任务</returns>
    public static async Task SendEmailAsync(
        IConfiguration configuration,
        string to,
        string subject,
        string body,
        bool isHtml = true)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(to);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentNullException.ThrowIfNull(body);
        await SendEmailAsync(configuration, new List<string> { to }, subject, body, null, null, isHtml);
    }

    /// <summary>
    /// 发送邮件（支持多个收件人）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="to">收件人邮箱列表</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="isHtml">是否为HTML格式，默认为true</param>
    /// <returns>任务</returns>
    public static async Task SendEmailAsync(
        IConfiguration configuration,
        List<string> to,
        string subject,
        string body,
        bool isHtml = true)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(to);
        if (to.Count == 0)
        {
            throw new ArgumentException("收件人列表不能为空", nameof(to));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentNullException.ThrowIfNull(body);
        await SendEmailAsync(configuration, to, subject, body, null, null, isHtml);
    }

    /// <summary>
    /// 发送邮件（支持抄送和密送）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="to">收件人邮箱列表</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="cc">抄送人邮箱列表（可选）</param>
    /// <param name="bcc">密送人邮箱列表（可选）</param>
    /// <param name="isHtml">是否为HTML格式，默认为true</param>
    /// <returns>任务</returns>
    public static async Task SendEmailAsync(
        IConfiguration configuration,
        List<string> to,
        string subject,
        string body,
        List<string>? cc,
        List<string>? bcc,
        bool isHtml = true)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(to);
        ArgumentException.ThrowIfNullOrEmpty(subject);
        ArgumentException.ThrowIfNullOrEmpty(body);

        if (to.Count == 0)
        {
            throw new ArgumentException("收件人列表不能为空", nameof(to));
        }

        try
        {
            var email = BindEmailOptions(configuration);
            EnsureEmailReadyForSend(email);

            // 创建邮件消息
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.FromName, email.FromEmail));
            
            // 添加收件人
            foreach (var recipient in to)
            {
                if (!string.IsNullOrWhiteSpace(recipient))
                {
                    message.To.Add(MailboxAddress.Parse(recipient));
                }
            }

            // 添加抄送
            if (cc != null && cc.Any())
            {
                foreach (var recipient in cc)
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        message.Cc.Add(MailboxAddress.Parse(recipient));
                    }
                }
            }

            // 添加密送
            if (bcc != null && bcc.Any())
            {
                foreach (var recipient in bcc)
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        message.Bcc.Add(MailboxAddress.Parse(recipient));
                    }
                }
            }

            message.Subject = subject;

            // 设置邮件正文
            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }
            message.Body = bodyBuilder.ToMessageBody();

            // 发送邮件
            using var client = new SmtpClient();
            if (email.SkipSslCertificateValidation)
                client.ServerCertificateValidationCallback = (_, _2, _3, _4) => true;
            await client.ConnectAsync(email.SmtpHost, email.SmtpPort, email.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            await client.AuthenticateAsync(email.SmtpUserName, email.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            TaktLogger.Information("[TaktMailHelper] 邮件发送成功，收件人: {Recipients}, 主题: {Subject}", string.Join(", ", to), subject);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktMailHelper] 邮件发送失败，收件人: {Recipients}, 主题: {Subject}", string.Join(", ", to), subject);
            throw;
        }
    }

    /// <summary>
    /// 发送邮件（支持附件）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="to">收件人邮箱</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="attachments">附件路径列表（可选）</param>
    /// <param name="isHtml">是否为HTML格式，默认为true</param>
    /// <returns>任务</returns>
    public static async Task SendEmailAsync(
        IConfiguration configuration,
        string to,
        string subject,
        string body,
        List<string>? attachments,
        bool isHtml = true)
    {
        await SendEmailAsync(configuration, new List<string> { to }, subject, body, null, null, attachments, null, null, MessagePriority.Normal, isHtml);
    }

    /// <summary>
    /// 发送邮件（完整功能，支持附件、回复地址、优先级等）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="to">收件人邮箱列表</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="cc">抄送人邮箱列表（可选）</param>
    /// <param name="bcc">密送人邮箱列表（可选）</param>
    /// <param name="attachments">附件路径列表（可选）</param>
    /// <param name="replyTo">回复地址（可选）</param>
    /// <param name="fromEmail">自定义发件人邮箱（可选，默认从配置读取）</param>
    /// <param name="priority">邮件优先级，默认为Normal</param>
    /// <param name="isHtml">是否为HTML格式，默认为true</param>
    /// <returns>任务</returns>
    public static async Task SendEmailAsync(
        IConfiguration configuration,
        List<string> to,
        string subject,
        string body,
        List<string>? cc,
        List<string>? bcc,
        List<string>? attachments,
        string? replyTo,
        string? fromEmail,
        MessagePriority priority = MessagePriority.Normal,
        bool isHtml = true)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(to);
        ArgumentException.ThrowIfNullOrEmpty(subject);
        ArgumentException.ThrowIfNullOrEmpty(body);

        if (to.Count == 0)
        {
            throw new ArgumentException("收件人列表不能为空", nameof(to));
        }

        try
        {
            var email = BindEmailOptions(configuration);
            EnsureEmailReadyForSend(email);

            // 使用自定义发件人邮箱或配置中的邮箱
            var actualFromEmail = !string.IsNullOrWhiteSpace(fromEmail) ? fromEmail : email.FromEmail;

            // 创建邮件消息
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.FromName, actualFromEmail));
            
            // 添加回复地址
            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                message.ReplyTo.Add(MailboxAddress.Parse(replyTo));
            }
            
            // 设置邮件优先级
            message.Priority = priority;
            
            // 添加收件人
            foreach (var recipient in to)
            {
                if (!string.IsNullOrWhiteSpace(recipient))
                {
                    message.To.Add(MailboxAddress.Parse(recipient));
                }
            }

            // 添加抄送
            if (cc != null && cc.Any())
            {
                foreach (var recipient in cc)
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        message.Cc.Add(MailboxAddress.Parse(recipient));
                    }
                }
            }

            // 添加密送
            if (bcc != null && bcc.Any())
            {
                foreach (var recipient in bcc)
                {
                    if (!string.IsNullOrWhiteSpace(recipient))
                    {
                        message.Bcc.Add(MailboxAddress.Parse(recipient));
                    }
                }
            }

            message.Subject = subject;

            // 设置邮件正文
            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }

            var maxAttachmentSizeBytes = (long)email.MaxAttachmentSizeMB * 1024 * 1024;
            var maxEmailSizeBytes = (long)email.MaxEmailSizeMB * 1024 * 1024;

            // 计算邮件正文大小（字节）
            var bodySizeBytes = Encoding.UTF8.GetByteCount(body);
            var totalSizeBytes = (long)bodySizeBytes;

            // 添加附件
            if (attachments != null && attachments.Any())
            {
                if (!email.EnableAttachments)
                {
                    TaktLogger.Warning("[TaktMailHelper] 附件功能已禁用，跳过所有附件");
                    throw new InvalidOperationException("附件功能已禁用，请检查 appsettings.json 中的 Email:EnableAttachments 配置");
                }

                foreach (var attachmentPath in attachments)
                {
                    if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath))
                    {
                        var fileName = Path.GetFileName(attachmentPath);
                        var fileInfo = new FileInfo(attachmentPath);
                        var fileSizeBytes = fileInfo.Length;

                        // 检查单个附件大小
                        if (fileSizeBytes > maxAttachmentSizeBytes)
                        {
                            var fileSizeMB = fileSizeBytes / (1024.0 * 1024.0);
                            TaktLogger.Warning("[TaktMailHelper] 附件大小超过限制: {FileName}, 大小: {SizeMB:F2} MB, 限制: {MaxSizeMB:F2} MB", 
                                fileName, fileSizeMB, email.MaxAttachmentSizeMB);
                            throw new InvalidOperationException($"附件大小超过限制: {fileName} ({fileSizeMB:F2} MB) > {email.MaxAttachmentSizeMB:F2} MB");
                        }

                        // 检查总邮件大小
                        if (totalSizeBytes + fileSizeBytes > maxEmailSizeBytes)
                        {
                            var totalSizeMB = (totalSizeBytes + fileSizeBytes) / (1024.0 * 1024.0);
                            TaktLogger.Warning("[TaktMailHelper] 邮件总大小超过限制: 当前大小: {TotalSizeMB:F2} MB, 限制: {MaxSizeMB:F2} MB", 
                                totalSizeMB, email.MaxEmailSizeMB);
                            throw new InvalidOperationException($"邮件总大小超过限制: {totalSizeMB:F2} MB > {email.MaxEmailSizeMB:F2} MB");
                        }

                        bodyBuilder.Attachments.Add(attachmentPath);
                        totalSizeBytes += fileSizeBytes;
                        TaktLogger.Information("[TaktMailHelper] 添加附件: {FileName}, 大小: {SizeMB:F2} MB, 路径: {FilePath}", 
                            fileName, fileSizeBytes / (1024.0 * 1024.0), attachmentPath);
                    }
                    else
                    {
                        TaktLogger.Warning("[TaktMailHelper] 附件不存在或路径为空，跳过: {FilePath}", attachmentPath ?? "null");
                    }
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            // 发送邮件
            using var client = new SmtpClient();
            if (email.SkipSslCertificateValidation)
                client.ServerCertificateValidationCallback = (_, _2, _3, _4) => true;
            await client.ConnectAsync(email.SmtpHost, email.SmtpPort, email.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            await client.AuthenticateAsync(email.SmtpUserName, email.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            var attachmentInfo = attachments != null && attachments.Any() 
                ? $", 附件数: {attachments.Count(f => !string.IsNullOrWhiteSpace(f) && File.Exists(f))}, 总大小: {totalSizeBytes / (1024.0 * 1024.0):F2} MB" 
                : "";
            TaktLogger.Information("[TaktMailHelper] 邮件发送成功，收件人: {Recipients}, 主题: {Subject}{AttachmentInfo}", 
                string.Join(", ", to), subject, attachmentInfo);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktMailHelper] 邮件发送失败，收件人: {Recipients}, 主题: {Subject}", string.Join(", ", to), subject);
            throw;
        }
    }

    /// <summary>
    /// 发送邮件（支持内嵌图片）
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="to">收件人邮箱</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="htmlBody">HTML邮件内容（可以使用 &lt;img src="cid:图片ID"&gt; 引用内嵌图片）</param>
    /// <param name="embeddedImages">内嵌图片字典（Key: 图片ID, Value: 图片路径）</param>
    /// <param name="attachments">附件路径列表（可选）</param>
    /// <returns>任务</returns>
    public static async Task SendEmailWithEmbeddedImagesAsync(
        IConfiguration configuration,
        string to,
        string subject,
        string htmlBody,
        Dictionary<string, string>? embeddedImages,
        List<string>? attachments = null)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(to);
        ArgumentException.ThrowIfNullOrEmpty(subject);
        ArgumentException.ThrowIfNullOrEmpty(htmlBody);

        try
        {
            var email = BindEmailOptions(configuration);
            EnsureEmailReadyForSend(email);

            // 创建邮件消息
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.FromName, email.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var maxAttachmentSizeBytes = (long)email.MaxAttachmentSizeMB * 1024 * 1024;
            var maxEmailSizeBytes = (long)email.MaxEmailSizeMB * 1024 * 1024;

            // 计算邮件正文大小（字节）
            var bodySizeBytes = Encoding.UTF8.GetByteCount(htmlBody);
            var totalSizeBytes = (long)bodySizeBytes;

            // 设置邮件正文和内嵌图片
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlBody;

            // 添加内嵌图片
            if (embeddedImages != null && embeddedImages.Any())
            {
                foreach (var image in embeddedImages)
                {
                    if (!string.IsNullOrWhiteSpace(image.Value) && File.Exists(image.Value))
                    {
                        var imageContentId = image.Key;
                        var fileInfo = new FileInfo(image.Value);
                        var fileSizeBytes = fileInfo.Length;

                        // 检查总邮件大小（内嵌图片也计入）
                        if (totalSizeBytes + fileSizeBytes > maxEmailSizeBytes)
                        {
                            var totalSizeMB = (totalSizeBytes + fileSizeBytes) / (1024.0 * 1024.0);
                            TaktLogger.Warning("[TaktMailHelper] 邮件总大小超过限制（含内嵌图片）: 当前大小: {TotalSizeMB:F2} MB, 限制: {MaxSizeMB:F2} MB", 
                                totalSizeMB, email.MaxEmailSizeMB);
                            throw new InvalidOperationException($"邮件总大小超过限制: {totalSizeMB:F2} MB > {email.MaxEmailSizeMB:F2} MB");
                        }

                        var linkedResource = bodyBuilder.LinkedResources.Add(image.Value);
                        linkedResource.ContentId = imageContentId;
                        totalSizeBytes += fileSizeBytes;
                        TaktLogger.Information("[TaktMailHelper] 添加内嵌图片: ID={ImageId}, 大小: {SizeMB:F2} MB, 路径: {FilePath}", 
                            imageContentId, fileSizeBytes / (1024.0 * 1024.0), image.Value);
                    }
                    else
                    {
                        TaktLogger.Warning("[TaktMailHelper] 内嵌图片不存在或路径为空，跳过: ID={ImageId}, 路径: {FilePath}", 
                            image.Key, image.Value ?? "null");
                    }
                }
            }

            // 添加附件
            if (attachments != null && attachments.Any())
            {
                if (!email.EnableAttachments)
                {
                    TaktLogger.Warning("[TaktMailHelper] 附件功能已禁用，跳过所有附件");
                    throw new InvalidOperationException("附件功能已禁用，请检查 appsettings.json 中的 Email:EnableAttachments 配置");
                }

                foreach (var attachmentPath in attachments)
                {
                    if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath))
                    {
                        var fileName = Path.GetFileName(attachmentPath);
                        var fileInfo = new FileInfo(attachmentPath);
                        var fileSizeBytes = fileInfo.Length;

                        // 检查单个附件大小
                        if (fileSizeBytes > maxAttachmentSizeBytes)
                        {
                            var fileSizeMB = fileSizeBytes / (1024.0 * 1024.0);
                            TaktLogger.Warning("[TaktMailHelper] 附件大小超过限制: {FileName}, 大小: {SizeMB:F2} MB, 限制: {MaxSizeMB:F2} MB", 
                                fileName, fileSizeMB, email.MaxAttachmentSizeMB);
                            throw new InvalidOperationException($"附件大小超过限制: {fileName} ({fileSizeMB:F2} MB) > {email.MaxAttachmentSizeMB:F2} MB");
                        }

                        // 检查总邮件大小
                        if (totalSizeBytes + fileSizeBytes > maxEmailSizeBytes)
                        {
                            var totalSizeMB = (totalSizeBytes + fileSizeBytes) / (1024.0 * 1024.0);
                            TaktLogger.Warning("[TaktMailHelper] 邮件总大小超过限制: 当前大小: {TotalSizeMB:F2} MB, 限制: {MaxSizeMB:F2} MB", 
                                totalSizeMB, email.MaxEmailSizeMB);
                            throw new InvalidOperationException($"邮件总大小超过限制: {totalSizeMB:F2} MB > {email.MaxEmailSizeMB:F2} MB");
                        }

                        bodyBuilder.Attachments.Add(attachmentPath);
                        totalSizeBytes += fileSizeBytes;
                        TaktLogger.Information("[TaktMailHelper] 添加附件: {FileName}, 大小: {SizeMB:F2} MB, 路径: {FilePath}", 
                            fileName, fileSizeBytes / (1024.0 * 1024.0), attachmentPath);
                    }
                    else
                    {
                        TaktLogger.Warning("[TaktMailHelper] 附件不存在或路径为空，跳过: {FilePath}", attachmentPath ?? "null");
                    }
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            // 发送邮件
            using var client = new SmtpClient();
            if (email.SkipSslCertificateValidation)
                client.ServerCertificateValidationCallback = (_, _2, _3, _4) => true;
            await client.ConnectAsync(email.SmtpHost, email.SmtpPort, email.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            await client.AuthenticateAsync(email.SmtpUserName, email.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            var embeddedInfo = embeddedImages != null && embeddedImages.Any() 
                ? $", 内嵌图片数: {embeddedImages.Count(i => !string.IsNullOrWhiteSpace(i.Value) && File.Exists(i.Value))}" 
                : "";
            var attachmentInfo = attachments != null && attachments.Any() 
                ? $", 附件数: {attachments.Count(f => !string.IsNullOrWhiteSpace(f) && File.Exists(f))}" 
                : "";
            var sizeInfo = $", 总大小: {totalSizeBytes / (1024.0 * 1024.0):F2} MB";
            TaktLogger.Information("[TaktMailHelper] 邮件发送成功（含内嵌图片），收件人: {Recipient}, 主题: {Subject}{EmbeddedInfo}{AttachmentInfo}{SizeInfo}", 
                to, subject, embeddedInfo, attachmentInfo, sizeInfo);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktMailHelper] 邮件发送失败（含内嵌图片），收件人: {Recipient}, 主题: {Subject}", to, subject);
            throw;
        }
    }

    /// <summary>
    /// 从配置绑定 Email 节（启动期已校验；缺失或绑定失败时抛异常）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>邮件配置</returns>
    private static TaktEmailOptions BindEmailOptions(IConfiguration configuration) =>
        configuration.BindOptions<TaktEmailOptions>(TaktEmailOptions.SectionName);

    /// <summary>
    /// 发信前校验 SMTP 凭据（启动期仅校验数值范围，凭据由运维在 appsettings 覆盖）
    /// </summary>
    /// <param name="email">邮件配置</param>
    private static void EnsureEmailReadyForSend(TaktEmailOptions email)
    {
        ArgumentNullException.ThrowIfNull(email);
        if (string.IsNullOrWhiteSpace(email.SmtpHost)
            || string.IsNullOrWhiteSpace(email.SmtpUserName)
            || string.IsNullOrWhiteSpace(email.FromEmail)
            || string.IsNullOrWhiteSpace(email.FromName))
        {
            throw new InvalidOperationException(
                "邮件 SMTP 凭据不完整，请在 appsettings Email 节配置 SmtpHost/SmtpUserName/FromEmail/FromName");
        }
    }
}
