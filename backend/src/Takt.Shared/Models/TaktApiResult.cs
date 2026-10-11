// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Models
// 文件名称：TaktApiResult.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt API统一返回结果，提供标准化的API响应格式
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Models;

/// <summary>
/// Takt API统一返回结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class TaktApiResult<T>
{
    /// <summary>
    /// 结果代码
    /// </summary>
    public TaktResultCode Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success => Code == TaktResultCode.Success;

    /// <summary>
    /// 成功结果
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="message">消息</param>
    /// <returns>API结果</returns>
    public static TaktApiResult<T> Ok(T? data = default, string message = "操作成功")
    {
        return new TaktApiResult<T>
        {
            Code = TaktResultCode.Success,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 失败结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="code">结果代码</param>
    /// <returns>API结果</returns>
    public static TaktApiResult<T> Fail(string message, TaktResultCode code = TaktResultCode.BadRequest)
    {
        return new TaktApiResult<T>
        {
            Code = code,
            Message = message,
            Data = default
        };
    }
}

/// <summary>
/// Takt API统一返回结果（无数据）
/// </summary>
public class TaktApiResult : TaktApiResult<object>
{
    /// <summary>
    /// 成功结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>API结果</returns>
    public static TaktApiResult Ok(string message = "操作成功")
    {
        return new TaktApiResult
        {
            Code = TaktResultCode.Success,
            Message = message,
            Data = null
        };
    }

    /// <summary>
    /// 失败结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="code">结果代码</param>
    /// <returns>API结果</returns>
    public static new TaktApiResult Fail(string message, TaktResultCode code = TaktResultCode.BadRequest)
    {
        return new TaktApiResult
        {
            Code = code,
            Message = message,
            Data = null
        };
    }
}