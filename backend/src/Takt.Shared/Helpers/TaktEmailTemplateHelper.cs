// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktEmailTemplateHelper.cs
// 功能描述：邮件模板加载与占位符替换，供发送邮件时套用。优先从标准位置 wwwroot/Email/Templates 加载。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;

namespace Takt.Shared.Helpers;

/// <summary>
/// 邮件模板名称常量，引用时使用
/// </summary>
public static class TaktEmailTemplateNames
{
    /// <summary>
    /// 用户注册成功通知（含初始密码）
    /// </summary>
    public const string InitialPassword = "InitialPassword";

    /// <summary>
    /// 密码重置通知
    /// </summary>
    public const string ForgotPassword = "ForgotPassword";

    /// <summary>
    /// 通用通知（系统通知等），占位符：Title、Greeting、Content
    /// </summary>
    public const string Generic = "Generic";

    /// <summary>
    /// 流程处理通用模板，占位符：Title、Greeting、ProcessName、Applicant、ApplyTime、CurrentNode、Content、ActionHint
    /// </summary>
    public const string ProcessNotify = "ProcessNotify";
}

/// <summary>
/// 邮件模板帮助类：纯文本 .txt 模板，从标准位置 wwwroot/Email/Templates 按变量填充后返回正文。
/// </summary>
/// <remarks>文件 I/O 网关；模板路径须由调用方显式传入 <paramref name="templatesBasePath"/>。</remarks>
public static class TaktEmailTemplateHelper
{
    private const string TemplateExtension = ".txt";

    /// <summary>
    /// 加载模板并替换占位符。仅从 <paramref name="templatesBasePath"/>（wwwroot/Email/Templates）读取 .txt。
    /// </summary>
    /// <param name="templateName">模板名（不含扩展名），使用 TaktEmailTemplateNames 常量</param>
    /// <param name="variables">变量字典，键为占位符名</param>
    /// <param name="templatesBasePath">模板目录绝对路径（必填，由 Program 设置 Email:TemplatesPath）</param>
    /// <returns>填充后的纯文本字符串，发送时请使用 isHtml: false</returns>
    /// <exception cref="ArgumentException"><paramref name="templateName"/> 为空</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variables"/> 为 null</exception>
    public static async Task<string> GetFilledBodyAsync(string templateName, IReadOnlyDictionary<string, string?> variables, string? templatesBasePath = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);
        ArgumentNullException.ThrowIfNull(variables);
        var content = await LoadTemplateAsync(templateName, templatesBasePath);
        return FillTemplate(content, variables);
    }

    /// <summary>
    /// 加载模板原始内容。仅从标准位置读取 .txt 文件。
    /// </summary>
    /// <param name="templateName">模板名（不含扩展名）</param>
    /// <param name="templatesBasePath">模板目录绝对路径（WebApi wwwroot/Email/Templates）</param>
    public static async Task<string> LoadTemplateAsync(string templateName, string? templatesBasePath = null)
    {
        if (string.IsNullOrWhiteSpace(templateName))
            throw new ArgumentException("模板名不能为空", nameof(templateName));
        if (string.IsNullOrWhiteSpace(templatesBasePath))
            throw new InvalidOperationException("邮件模板路径未配置，请设置 Email:TemplatesPath（标准位置：wwwroot/Email/Templates）。");

        var name = templateName.Trim();
        var fileName = name + TemplateExtension;
        var filePath = Path.Combine(templatesBasePath, fileName);

        if (!File.Exists(filePath))
        {
            TaktLogger.Warning("[TaktEmailTemplateHelper] 未找到邮件模板文件: {FilePath}", filePath);
            throw new InvalidOperationException($"未找到邮件模板: {templateName}。请确认 {filePath} 存在。");
        }

        try
        {
            return await File.ReadAllTextAsync(filePath);
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(ex, "[TaktEmailTemplateHelper] 读取模板失败: {FilePath}", filePath);
            throw;
        }
    }

    /// <summary>
    /// 将模板内容中的 {{Key}} 替换为 variables 中对应键的值
    /// </summary>
    /// <param name="content">模板正文</param>
    /// <param name="variables">占位符变量</param>
    /// <returns>替换后的正文</returns>
    /// <exception cref="ArgumentNullException"><paramref name="variables"/> 为 null</exception>
    public static string FillTemplate(string content, IReadOnlyDictionary<string, string?> variables)
    {
        ArgumentNullException.ThrowIfNull(variables);
        if (string.IsNullOrEmpty(content))
            return content;
        if (variables.Count == 0)
            return content;

        var result = content;
        foreach (var kv in variables)
        {
            var placeholder = "{{" + kv.Key + "}}";
            var value = kv.Value ?? string.Empty;
            result = result.Replace(placeholder, value);
        }

        // 未提供变量的占位符保留或置空（可选：替换为空白）
        result = Regex.Replace(result, @"\{\{(\w+)\}\}", string.Empty);
        return result;
    }

    /// <summary>
    /// 生成邮件称呼：尊敬的 邮箱(显示名)：
    /// </summary>
    /// <param name="userEmail">用户邮箱</param>
    /// <param name="displayName">显示名</param>
    /// <returns>称呼行</returns>
    /// <exception cref="ArgumentException"><paramref name="userEmail"/> 为空</exception>
    public static string BuildGreeting(string userEmail, string? displayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);
        var name = string.IsNullOrWhiteSpace(displayName) ? "用户" : displayName;
        return $"尊敬的 {userEmail}({name})：";
    }
}
