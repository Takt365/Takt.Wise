// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktLocalizationExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：本地化扩展方法
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 本地化扩展方法
/// </summary>
public static class TaktLocalizationExtensions
{
    /// <summary>
    /// 使用请求本地化（Accept-Language；兜底见 appsettings Localization:DefaultCulture）
    /// </summary>
    /// <param name="app">应用构建器</param>
    /// <returns>应用构建器</returns>
    public static IApplicationBuilder UseTaktLocalization(this IApplicationBuilder app)
    {
        var locOptions = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(locOptions.Value);
        return app;
    }
}
