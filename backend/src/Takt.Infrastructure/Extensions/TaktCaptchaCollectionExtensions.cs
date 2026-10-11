// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktCaptchaCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：验证码服务注册扩展方法
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services.Captcha;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 验证码服务注册扩展方法
/// </summary>
public static class TaktCaptchaCollectionExtensions
{
    /// <summary>
    /// 添加验证码服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktCaptcha(this IServiceCollection services, IConfiguration configuration)
    {
        var captchaOptions = configuration.RequireOptions<TaktCaptchaOptions>(TaktCaptchaOptions.SectionName);
        captchaOptions.Validate();

        services.Configure<TaktCaptchaOptions>(configuration.GetSection(TaktCaptchaOptions.SectionName));
        services.AddSingleton(captchaOptions);
        services.AddHttpClient(nameof(TaktCaptchaInitializer), client =>
        {
            client.Timeout = TimeSpan.FromSeconds(60);
        });
        services.AddHostedService<TaktCaptchaInitializer>();
        services.AddScoped<ITaktCaptchaService, TaktCaptchaService>();

        return services;
    }
}
