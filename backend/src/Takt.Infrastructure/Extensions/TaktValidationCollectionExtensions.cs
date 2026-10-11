// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktValidationCollectionExtensions.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：FluentValidation 验证器 DI 注册（扫描 Takt.Application；入参校验由 WebApi TaktValidationFilter 执行）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Takt.Application.Validators.Identity;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// FluentValidation 验证器注册扩展
/// </summary>
public static class TaktValidationCollectionExtensions
{
    /// <summary>
    /// 注册 Takt.Application 程序集中的全部 FluentValidation 验证器
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktValidators(this IServiceCollection services)
    {
        // TaktUserCreateValidator 仅作程序集锚点；会扫描 Takt.Application 内所有 AbstractValidator<T>
        services.AddValidatorsFromAssemblyContaining<TaktUserCreateValidator>();
        return services;
    }
}
