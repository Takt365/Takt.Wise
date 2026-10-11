// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Exceptions
// 文件名称：TaktBusinessException.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt业务异常，用于业务逻辑错误处理
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Exceptions;

/// <summary>
/// Takt业务异常
/// </summary>
/// <remarks>
/// 业务异常用于表示业务逻辑错误，是预期的异常情况。
/// 此类异常由全局异常处理中间件统一捕获并记录日志（警告级别）。
/// 
/// 符合 .NET 异常设计规范：
/// - 提供所有标准构造函数
/// - 支持错误代码属性
/// - 支持内部异常链
/// </remarks>
public class TaktBusinessException : Exception
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public TaktBusinessException()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常消息</param>
    public TaktBusinessException(string message) : base(message)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常消息</param>
    /// <param name="errorCode">错误代码</param>
    public TaktBusinessException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常消息</param>
    /// <param name="innerException">内部异常</param>
    public TaktBusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">异常消息</param>
    /// <param name="errorCode">错误代码</param>
    /// <param name="innerException">内部异常</param>
    public TaktBusinessException(string message, string errorCode, Exception innerException) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}