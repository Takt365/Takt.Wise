// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.CustomerService
// 文件名称：TaktCustomerServiceTicketValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerServiceTicket 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCustomerServiceTicket 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.CustomerService;

namespace Takt.Application.Validators.Logistics.CustomerService;

// ========================================
// 创建CustomerServiceTicket 验证器
// ========================================

/// <summary>
/// 创建CustomerServiceTicket DTO 验证器
/// </summary>
public class TaktCustomerServiceTicketCreateValidator : AbstractValidator<TaktCustomerServiceTicketCreateDto>
{
    /// <summary>
    /// 初始化 创建CustomerServiceTicket 校验规则
    /// </summary>
    public TaktCustomerServiceTicketCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ClientId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ClientId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceTicketCode)
            .NotEmpty().WithMessage("服务工单编码不能为空")
            .MaximumLength(50).WithMessage("服务工单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空").When(x => x.ClientId <= 0)
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName1)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(140).WithMessage("客户端名称长度不能超过140个字符");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.ServiceOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务订单ID不能为负数");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.TicketSubject)
            .NotEmpty().WithMessage("工单主题不能为空")
            .MaximumLength(200).WithMessage("工单主题长度不能超过200个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("指派服务人员工ID不能为负数");
        RuleFor(x => x.AcceptedByEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("验收人不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CustomerServiceTicket 验证器
// ========================================

/// <summary>
/// 更新CustomerServiceTicket DTO 验证器
/// </summary>
public class TaktCustomerServiceTicketUpdateValidator : AbstractValidator<TaktCustomerServiceTicketUpdateDto>
{
    /// <summary>
    /// 初始化 更新CustomerServiceTicket 校验规则
    /// </summary>
    public TaktCustomerServiceTicketUpdateValidator()
    {
        RuleFor(x => x.CustomerServiceTicketId)
            .GreaterThan(0).WithMessage("CustomerServiceTicketID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ClientId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ClientId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ServiceTicketCode)
            .NotEmpty().WithMessage("服务工单编码不能为空")
            .MaximumLength(50).WithMessage("服务工单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空").When(x => x.ClientId <= 0)
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName1)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(140).WithMessage("客户端名称长度不能超过140个字符");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.ServiceOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务订单ID不能为负数");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.TicketSubject)
            .NotEmpty().WithMessage("工单主题不能为空")
            .MaximumLength(200).WithMessage("工单主题长度不能超过200个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("指派服务人员工ID不能为负数");
        RuleFor(x => x.AcceptedByEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("验收人不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入CustomerServiceTicket 验证器
// ========================================

/// <summary>
/// 导入CustomerServiceTicket DTO 验证器
/// </summary>
public class TaktCustomerServiceTicketImportValidator : AbstractValidator<TaktCustomerServiceTicketImportDto>
{
    /// <summary>
    /// 初始化 导入CustomerServiceTicket 校验规则
    /// </summary>
    public TaktCustomerServiceTicketImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ServiceTicketCode)
            .NotEmpty().WithMessage("服务工单编码不能为空")
            .MaximumLength(50).WithMessage("服务工单编码长度不能超过50个字符");
        RuleFor(x => x.ClientId)
            .GreaterThanOrEqualTo(0).WithMessage("客户端ID不能为负数");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(20).WithMessage("客户端编码长度不能超过20个字符");
        RuleFor(x => x.ClientName1)
            .NotEmpty().WithMessage("客户端名称不能为空")
            .MaximumLength(140).WithMessage("客户端名称长度不能超过140个字符");
        RuleFor(x => x.ServiceRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务请求ID不能为负数");
        RuleFor(x => x.ServiceOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务订单ID不能为负数");
        RuleFor(x => x.ServiceContractId)
            .GreaterThanOrEqualTo(0).WithMessage("关联服务合同ID不能为负数");
        RuleFor(x => x.TicketSubject)
            .NotEmpty().WithMessage("工单主题不能为空")
            .MaximumLength(200).WithMessage("工单主题长度不能超过200个字符");
        RuleFor(x => x.AssignedEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("指派服务人员工ID不能为负数");
        RuleFor(x => x.AcceptedByEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("验收人不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
