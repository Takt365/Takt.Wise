// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Services
// 文件名称：ITaktLocalizationService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：本地化服务接口
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 本地化服务接口
/// </summary>
public interface ITaktLocalizationService
{
    /// <summary>
    /// 翻译文本
    /// </summary>
    /// <param name="key">翻译键</param>
    /// <param name="culture">语言代码（可选，默认当前语言）</param>
    /// <param name="args">格式化参数</param>
    /// <returns>翻译文本；未匹配时直接返回 <paramref name="key"/>（无语言或数据源兜底）</returns>
    string Translate(string key, string? culture = null, params object[] args);

    /// <summary>
    /// 翻译异常消息
    /// </summary>
    /// <param name="messageKey">消息键</param>
    /// <param name="resourceType">资源类型</param>
    /// <param name="culture">语言代码</param>
    /// <param name="args">格式化参数</param>
    /// <returns>翻译文本；未匹配时直接返回 <paramref name="messageKey"/></returns>
    string TranslateException(string messageKey, string resourceType = "Backend", string? culture = null, params object[] args);

    /// <summary>
    /// 翻译验证消息
    /// </summary>
    /// <param name="messageKey">消息键</param>
    /// <param name="culture">语言代码</param>
    /// <param name="args">格式化参数</param>
    /// <returns>翻译文本；未匹配时直接返回 <paramref name="messageKey"/></returns>
    string TranslateValidation(string messageKey, string? culture = null, params object[] args);

    /// <summary>
    /// 获取当前请求或线程上下文中的语言代码
    /// </summary>
    /// <returns>语言代码（如 zh-CN、en-US）</returns>
    string GetCurrentCulture();
}
