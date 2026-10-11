// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktLoginValidators.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：登录相关 DTO FluentValidation 验证器（抽象 common.validation.* + 字段标签）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;

namespace Takt.Application.Validators.Identity;

/// <summary>
/// RSA PKCS#1 传输密文（Base64）长度范围（2048 位密钥密文约 344 字符，预留更大密钥余量）
/// </summary>
internal static class TaktLoginPasswordTransportRules
{
    internal const int CipherMinLength = 64;

    internal const int CipherMaxLength = 4096;
}

/// <summary>
/// 用户登录请求 DTO 验证器
/// </summary>
public class TaktLoginRequestValidator : AbstractValidator<TaktLoginRequestDto>
{
    /// <summary>
    /// 初始化登录请求校验规则
    /// </summary>
    /// <param name="localizationService">本地化服务</param>
    public TaktLoginRequestValidator(ITaktLocalizationService localizationService)
    {
        string T(string key) => localizationService.Translate(key);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.Required, TaktValidationI18nKeys.EntityUserName))
            .MinimumLength(4)
            .MaximumLength(20)
            .WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.LengthBetween, TaktValidationI18nKeys.EntityUserName, min: 4, max: 20));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.Required, TaktValidationI18nKeys.EntityUserPassword))
            .Length(TaktLoginPasswordTransportRules.CipherMinLength, TaktLoginPasswordTransportRules.CipherMaxLength)
            .WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.InvalidFormat, TaktValidationI18nKeys.FieldPasswordCipher))
            .When(x => string.IsNullOrWhiteSpace(x.LoginTicket));

        RuleFor(x => x.LoginTicket)
            .MaximumLength(64).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldLoginTicket, max: 64))
            .When(x => !string.IsNullOrWhiteSpace(x.LoginTicket));

        RuleFor(x => x.TenantCode)
            .MaximumLength(4).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldTenantCode, max: 4))
            .When(x => !string.IsNullOrEmpty(x.TenantCode));

        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldCompanyCode, max: 4))
            .When(x => !string.IsNullOrEmpty(x.CompanyCode));

        RuleFor(x => x.CultureCode)
            .MaximumLength(10).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldCultureCode, max: 10))
            .When(x => !string.IsNullOrEmpty(x.CultureCode));

        RuleFor(x => x.CaptchaCode)
            .MaximumLength(8192).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldCaptchaPayload, max: 8192))
            .When(x => !string.IsNullOrEmpty(x.CaptchaCode));
    }
}

/// <summary>
/// 登录会话密码校验请求 DTO 验证器
/// </summary>
public class TaktSessionVerifyPasswordRequestValidator : AbstractValidator<TaktSessionVerifyPasswordRequestDto>
{
    /// <summary>
    /// 初始化密码校验规则
    /// </summary>
    /// <param name="localizationService">本地化服务</param>
    public TaktSessionVerifyPasswordRequestValidator(ITaktLocalizationService localizationService)
    {
        string T(string key) => localizationService.Translate(key);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.Required, TaktValidationI18nKeys.EntityUserName))
            .MinimumLength(4)
            .MaximumLength(20)
            .WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.LengthBetween, TaktValidationI18nKeys.EntityUserName, min: 4, max: 20));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.Required, TaktValidationI18nKeys.EntityUserPassword))
            .Length(TaktLoginPasswordTransportRules.CipherMinLength, TaktLoginPasswordTransportRules.CipherMaxLength)
            .WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.InvalidFormat, TaktValidationI18nKeys.FieldPasswordCipher));

        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldTenantCode))
            .MaximumLength(4).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldTenantCode, max: 4));

        RuleFor(x => x.CaptchaCode)
            .MaximumLength(8192).WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.TooLong, TaktValidationI18nKeys.FieldCaptchaPayload, max: 8192))
            .When(x => !string.IsNullOrEmpty(x.CaptchaCode));
    }
}

/// <summary>
/// 刷新令牌请求 DTO 验证器
/// </summary>
public class TaktRefreshTokenRequestValidator : AbstractValidator<TaktRefreshTokenRequestDto>
{
    /// <summary>
    /// 初始化刷新令牌校验规则
    /// </summary>
    /// <param name="localizationService">本地化服务</param>
    public TaktRefreshTokenRequestValidator(ITaktLocalizationService localizationService)
    {
        string T(string key) => localizationService.Translate(key);

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(TaktValidationMessageHelper.Build(T, TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldRefreshToken));
    }
}
