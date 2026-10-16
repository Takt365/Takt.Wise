// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktDeltaLogValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：DeltaLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDeltaLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Validators.Statistics.Logging;

// ========================================
// 创建DeltaLog 验证器
// ========================================

/// <summary>
/// 创建DeltaLog DTO 验证器
/// </summary>
public class TaktDeltaLogCreateValidator : AbstractValidator<TaktDeltaLogCreateDto>
{
    /// <summary>
    /// 初始化 创建DeltaLog 校验规则
    /// </summary>
    public TaktDeltaLogCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.OperType)
            .NotEmpty().WithMessage("操作类型不能为空")
            .MaximumLength(40).WithMessage("操作类型长度不能超过40个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("数据库表名不能为空")
            .MaximumLength(200).WithMessage("数据库表名长度不能超过200个字符");
        RuleFor(x => x.PrimaryKeyId)
            .GreaterThanOrEqualTo(0).WithMessage("业务主键 ID不能为负数");
        RuleFor(x => x.BeforeData)
            .NotEmpty().WithMessage("修改前数据 JSON不能为空");
        RuleFor(x => x.AfterData)
            .NotEmpty().WithMessage("修改后数据 JSON不能为空");
        RuleFor(x => x.DiffData)
            .NotEmpty().WithMessage("差异内容 JSON不能为空");
        RuleFor(x => x.SqlStatement)
            .NotEmpty().WithMessage("执行的 SQL 语句不能为空");
        RuleFor(x => x.OperIp)
            .NotEmpty().WithMessage("操作 IP不能为空")
            .MaximumLength(50).WithMessage("操作 IP长度不能超过50个字符");
        RuleFor(x => x.OperLocation)
            .NotEmpty().WithMessage("操作地点不能为空")
            .MaximumLength(200).WithMessage("操作地点长度不能超过200个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.Browser)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.Os)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("登录设备不能为空")
            .MaximumLength(40).WithMessage("登录设备长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新DeltaLog 验证器
// ========================================

/// <summary>
/// 更新DeltaLog DTO 验证器
/// </summary>
public class TaktDeltaLogUpdateValidator : AbstractValidator<TaktDeltaLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新DeltaLog 校验规则
    /// </summary>
    public TaktDeltaLogUpdateValidator()
    {
        RuleFor(x => x.DeltaLogId)
            .GreaterThan(0).WithMessage("DeltaLogID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.OperType)
            .NotEmpty().WithMessage("操作类型不能为空")
            .MaximumLength(40).WithMessage("操作类型长度不能超过40个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("数据库表名不能为空")
            .MaximumLength(200).WithMessage("数据库表名长度不能超过200个字符");
        RuleFor(x => x.PrimaryKeyId)
            .GreaterThanOrEqualTo(0).WithMessage("业务主键 ID不能为负数");
        RuleFor(x => x.BeforeData)
            .NotEmpty().WithMessage("修改前数据 JSON不能为空");
        RuleFor(x => x.AfterData)
            .NotEmpty().WithMessage("修改后数据 JSON不能为空");
        RuleFor(x => x.DiffData)
            .NotEmpty().WithMessage("差异内容 JSON不能为空");
        RuleFor(x => x.SqlStatement)
            .NotEmpty().WithMessage("执行的 SQL 语句不能为空");
        RuleFor(x => x.OperIp)
            .NotEmpty().WithMessage("操作 IP不能为空")
            .MaximumLength(50).WithMessage("操作 IP长度不能超过50个字符");
        RuleFor(x => x.OperLocation)
            .NotEmpty().WithMessage("操作地点不能为空")
            .MaximumLength(200).WithMessage("操作地点长度不能超过200个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.Browser)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.Os)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("登录设备不能为空")
            .MaximumLength(40).WithMessage("登录设备长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
