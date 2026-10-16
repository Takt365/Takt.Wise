// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktUserCompanyValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：UserCompany 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktUserCompany 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// UserCompany 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 UserCompany DTO 验证器
/// </summary>
public class TaktUserCompanyDtoValidator : AbstractValidator<TaktUserCompanyDto>
{
    /// <summary>
    /// 初始化 关联 UserCompany 校验规则
    /// </summary>
    public TaktUserCompanyDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("用户ID无效");
        RuleFor(x => x.UserName)
            .MaximumLength(200).WithMessage("UserName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.UserName));
    }
}
