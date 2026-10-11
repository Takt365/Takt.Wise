// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Database
// 文件名称：TaktDatabaseBackupValidators.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：DatabaseBackup 模块 FluentValidation（配置字段；编码可空由服务生成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Validators.Code.Database;

/// <summary>
/// 创建 DatabaseBackup DTO 验证器
/// </summary>
public class TaktDatabaseBackupCreateValidator : AbstractValidator<TaktDatabaseBackupCreateDto>
{
    /// <summary>
    /// 初始化创建校验规则
    /// </summary>
    public TaktDatabaseBackupCreateValidator()
    {
        RuleFor(x => x.BackupCode)
            .MaximumLength(40).WithMessage("备份编码长度不能超过40个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.BackupCode));
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .MaximumLength(3).WithMessage("目标租户长度不能超过3个字符");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库展示名不能为空")
            .MaximumLength(40).WithMessage("目标数据库展示名长度不能超过40个字符");
        RuleFor(x => x.BackupType)
            .Must(v => v is 0 or 1 or 2).WithMessage("备份类型须为 1(Full) 或 2(Delta)");
        RuleFor(x => x.BackupPathType)
            .Must(v => v is 0 or 1 or 2 or 3 or 4)
            .WithMessage("路径类型须为 1(本地服务器) 2(文件服务器) 3(FTP) 4(客户端)");
        RuleFor(x => x.BackupPath)
            .NotEmpty().WithMessage("备份目录不能为空")
            .MaximumLength(500).WithMessage("备份目录长度不能超过500个字符");
        RuleFor(x => x.BackupHost)
            .MaximumLength(200).WithMessage("主机长度不能超过200个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.BackupHost));
        RuleFor(x => x.BackupUserName)
            .MaximumLength(100).WithMessage("用户名长度不能超过100个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.BackupUserName));
        RuleFor(x => x.BackupFileName)
            .MaximumLength(200).WithMessage("备份文件名长度不能超过200个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.BackupFileName));
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");

        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}

/// <summary>
/// 更新 DatabaseBackup DTO 验证器
/// </summary>
public class TaktDatabaseBackupUpdateValidator : AbstractValidator<TaktDatabaseBackupUpdateDto>
{
    /// <summary>
    /// 初始化更新校验规则
    /// </summary>
    public TaktDatabaseBackupUpdateValidator()
    {
        RuleFor(x => x.DatabaseBackupId)
            .GreaterThan(0).WithMessage("DatabaseBackupID无效");
        Include(new TaktDatabaseBackupCreateValidator());
    }
}

/// <summary>
/// 按 Id 后台调度验证器
/// </summary>
public class TaktDatabaseBackupScheduleByIdValidator : AbstractValidator<TaktDatabaseBackupScheduleByIdDto>
{
    /// <summary>
    /// 初始化调度校验规则
    /// </summary>
    public TaktDatabaseBackupScheduleByIdValidator()
    {
        RuleFor(x => x.ScheduledAt)
            .Must(v => v > DateTime.Now).WithMessage("计划执行时间须晚于当前时间");
    }
}
