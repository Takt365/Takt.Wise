// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Filters
// 文件名称：TaktValidationFilter.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：FluentValidation 手动入参校验过滤器（替代已弃用的 FluentValidation.AspNetCore 自动验证）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Takt.Shared.Enums;
using Takt.Shared.Models;

namespace Takt.WebApi.Filters;

/// <summary>
/// FluentValidation 手动入参校验过滤器（按 Action 参数类型解析 IValidator 泛型接口）
/// </summary>
public sealed class TaktValidationFilter : IAsyncActionFilter
{
    /// <summary>
    /// Action 执行前校验复杂类型入参
    /// </summary>
    /// <param name="context">Action 执行上下文</param>
    /// <param name="next">后续管道委托</param>
    /// <returns>任务</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);
        var failures = new List<FluentValidation.Results.ValidationFailure>();
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null || !ShouldValidate(argument.GetType()))
            {
                continue;
            }
            var validator = ResolveValidator(context.HttpContext.RequestServices, argument.GetType());
            if (validator == null)
            {
                continue;
            }
            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
            if (!result.IsValid)
            {
                failures.AddRange(result.Errors);
            }
        }
        if (failures.Count > 0)
        {
            var message = string.Join("; ", failures
                .Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage))
                .Select(x => x.ErrorMessage)
                .Distinct());
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "请求参数校验失败";
            }
            var path = context.HttpContext.Request.Path.Value ?? string.Empty;
            var method = context.HttpContext.Request.Method;
            Serilog.Log.Warning(
                "FluentValidation 参数校验失败: {Method} {Path}, Message={Message}",
                method,
                path,
                message);
            context.Result = new ObjectResult(TaktApiResult.Fail(message, TaktResultCode.BadRequest))
            {
                StatusCode = StatusCodes.Status400BadRequest,
            };
            return;
        }
        await next();
    }

    /// <summary>
    /// 从 DI 解析指定类型的 FluentValidation 验证器
    /// </summary>
    /// <param name="serviceProvider">请求服务提供者</param>
    /// <param name="modelType">入参类型</param>
    /// <returns>验证器实例；未注册时返回 null</returns>
    private static IValidator? ResolveValidator(IServiceProvider serviceProvider, Type modelType)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(modelType);
        var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
        return serviceProvider.GetService(validatorType) as IValidator;
    }

    /// <summary>
    /// 是否应对该入参类型执行 FluentValidation
    /// </summary>
    /// <param name="modelType">入参类型</param>
    /// <returns>复杂引用类型返回 true</returns>
    private static bool ShouldValidate(Type modelType)
    {
        return modelType.IsClass && modelType != typeof(string);
    }
}
