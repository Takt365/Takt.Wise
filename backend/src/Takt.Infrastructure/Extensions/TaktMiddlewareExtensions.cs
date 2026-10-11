// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaktMiddlewareExtensions.cs" company="TaktWise">
//   Copyright (c) TaktWise. All rights reserved.
// </copyright>
// <summary>
//   中间件扩展方法
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Takt.Infrastructure.Middleware;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 中间件扩展方法
/// </summary>
public static class TaktMiddlewareExtensions
{
    /// <summary>
    /// 使用统一异常处理中间件
    /// </summary>
    /// <param name="app">应用程序构建器</param>
    /// <returns>应用程序构建器</returns>
    public static IApplicationBuilder UseTaktExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TaktExceptionMiddleware>();
    }

    /// <summary>
    /// 使用统一请求日志中间件
    /// </summary>
    /// <param name="app">应用程序构建器</param>
    /// <returns>应用程序构建器</returns>
    public static IApplicationBuilder UseTaktRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TaktLoggingMiddleware>();
    }
}
