// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Database
// 文件名称：TaktDatabaseBackupDtos.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：DatabaseBackup 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDatabaseBackup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Code.Database;

// ========================================
// DatabaseBackup 响应 DTO
// ========================================

/// <summary>
/// 数据库备份记录
/// 对应前端 TaktDatabaseBackupDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDatabaseBackupDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DatabaseBackupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DatabaseBackupId { get; set; }

    /// <summary>
    /// 备份编码（租户+公司内唯一）
    /// </summary>
    public string BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    public int BackupType { get; set; } = 0;

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; } = 0;

    /// <summary>
    /// 备份路径类型（1=本地服务器端 2=文件服务器 3=FTP 4=客户端）
    /// </summary>
    public int BackupPathType { get; set; } = 4;

    /// <summary>
    /// 目标备份目录
    /// </summary>
    public string BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 网络主机或 FTP 服务器名称
    /// </summary>
    public string? BackupHost { get; set; }

    /// <summary>
    /// FTP 端口
    /// </summary>
    public int? BackupPort { get; set; }

    /// <summary>
    /// 网络/FTP 用户名
    /// </summary>
    public string? BackupUserName { get; set; }

    /// <summary>
    /// 是否已保存密码（详情不回显明文）
    /// </summary>
    public bool HasBackupPassword { get; set; }

    /// <summary>
    /// 备份文件名（含 .bak）
    /// </summary>
    public string BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// 最近一次执行时间（摘要；明细见备份日志）
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    public string? QuartzTaskName { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    public int BackupStatus { get; set; } = 0;

}

// ========================================
// DatabaseBackup 查询 DTO
// ========================================

/// <summary>
/// DatabaseBackup 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDatabaseBackupQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 备份编码（租户+公司内唯一）
    /// </summary>
    public string? BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    public string? TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string? TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    public int? BackupType { get; set; }

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int? ExecuteMode { get; set; }

    /// <summary>
    /// 备份路径类型（1=本地服务器端 2=文件服务器 3=FTP 4=客户端）
    /// </summary>
    public int? BackupPathType { get; set; }

    /// <summary>
    /// 目标备份目录
    /// </summary>
    public string? BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 备份文件名
    /// </summary>
    public string? BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）（范围查询-开始）
    /// </summary>
    public DateTime? ScheduledAtStart { get; set; }

    /// <summary>
    /// 计划执行时间（后台调度）（范围查询-结束）
    /// </summary>
    public DateTime? ScheduledAtEnd { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-开始）
    /// </summary>
    public DateTime? LastRunAtStart { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-结束）
    /// </summary>
    public DateTime? LastRunAtEnd { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    public int? BackupStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建DatabaseBackup DTO
// ========================================

/// <summary>
/// 创建DatabaseBackup DTO
/// </summary>
public class TaktDatabaseBackupCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 备份编码（租户+公司内唯一；空则服务端自动生成）
    /// </summary>
    public string BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    [Required(ErrorMessage = "目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）不能为空")]
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    [Required(ErrorMessage = "目标数据库展示名不能为空")]
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    public int BackupType { get; set; } = 0;

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; } = 0;

    /// <summary>
    /// 备份路径类型（1=本地服务器端 2=文件服务器 3=FTP 4=客户端）
    /// </summary>
    public int BackupPathType { get; set; } = 4;

    /// <summary>
    /// 目标备份目录
    /// </summary>
    [Required(ErrorMessage = "备份目录不能为空")]
    public string BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 网络主机或 FTP 服务器名称
    /// </summary>
    public string? BackupHost { get; set; }

    /// <summary>
    /// FTP 端口
    /// </summary>
    public int? BackupPort { get; set; }

    /// <summary>
    /// 网络/FTP 用户名
    /// </summary>
    public string? BackupUserName { get; set; }

    /// <summary>
    /// 网络/FTP 密码（明文提交；空表示不修改）
    /// </summary>
    public string? BackupPassword { get; set; }

    /// <summary>
    /// 备份文件名（含 .bak；空则服务端生成）
    /// </summary>
    public string BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    public int BackupStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新DatabaseBackup DTO
// ========================================

/// <summary>
/// 更新DatabaseBackup DTO
/// 继承 TaktDatabaseBackupCreateDto，添加 DatabaseBackupId 字段
/// </summary>
public class TaktDatabaseBackupUpdateDto : TaktDatabaseBackupCreateDto
{
    /// <summary>
    /// DatabaseBackupID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DatabaseBackupId { get; set; }

}

// ========================================
// DatabaseBackup 状态 DTO
// ========================================

/// <summary>
/// DatabaseBackup 状态更新 DTO
/// </summary>
public class TaktDatabaseBackupStatusDto
{
    /// <summary>
    /// DatabaseBackupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DatabaseBackupId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    [Required(ErrorMessage = "备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）不能为空")]
    public int BackupStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// DatabaseBackup 导入模板行 DTO
/// </summary>
public class TaktDatabaseBackupTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 备份编码（租户+公司内唯一）
    /// </summary>
    public string? BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    public string? TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string? TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    public int? BackupType { get; set; }

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int? ExecuteMode { get; set; }

    /// <summary>
    /// 用户选择的备份目录（SQL Server 服务端可写路径）
    /// </summary>
    public string? BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 实际备份文件完整路径（.bak）
    /// </summary>
    public string? BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    public int? BackupStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// DatabaseBackup 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDatabaseBackupImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 备份编码（租户+公司内唯一）
    /// </summary>
    public string? BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    public string? TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string? TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    public int? BackupType { get; set; }

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int? ExecuteMode { get; set; }

    /// <summary>
    /// 用户选择的备份目录（SQL Server 服务端可写路径）
    /// </summary>
    public string? BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 实际备份文件完整路径（.bak）
    /// </summary>
    public string? BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    public int? BackupStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// DatabaseBackup 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDatabaseBackupExportDto
{
    /// <summary>
    /// DatabaseBackupID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DatabaseBackupId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 备份编码（租户+公司内唯一）
    /// </summary>
    public string BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    public int BackupType { get; set; } = 0;

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; } = 0;

    /// <summary>
    /// 备份路径类型（1=本地服务器端 2=文件服务器 3=FTP 4=客户端）
    /// </summary>
    public int BackupPathType { get; set; } = 4;

    /// <summary>
    /// 目标备份目录
    /// </summary>
    public string BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 网络主机或 FTP 服务器名称
    /// </summary>
    public string? BackupHost { get; set; }

    /// <summary>
    /// 备份文件名（含 .bak）
    /// </summary>
    public string BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// 最近一次执行时间（摘要）
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度）
    /// </summary>
    public int BackupStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 立即 / 调度备份请求（兼容：直接提交参数创建并调度）
/// </summary>
public class TaktDatabaseBackupRunDto
{
    public string TargetTenantCode { get; set; } = string.Empty;
    public string TargetDatabaseName { get; set; } = string.Empty;
    /// <summary>
    /// 1=Full Sync 2=Delta Sync
    /// </summary>
    public int BackupType { get; set; } = 1;
    /// <summary>
    /// 1=本地服务器端 2=文件服务器 3=FTP 4=客户端
    /// </summary>
    public int BackupPathType { get; set; } = 4;
    public string BackupPath { get; set; } = string.Empty;
    public string? BackupHost { get; set; }
    public int? BackupPort { get; set; }
    public string? BackupUserName { get; set; }
    public string? BackupPassword { get; set; }
    public string? BackupFileName { get; set; }
    public string? Remark { get; set; }
    public DateTime? ScheduledAt { get; set; }
}

/// <summary>
/// 按已有备份记录后台调度
/// </summary>
public class TaktDatabaseBackupScheduleByIdDto
{
    /// <summary>
    /// 计划执行时间（须晚于当前）
    /// </summary>
    public DateTime ScheduledAt { get; set; }
}

/// <summary>
/// 备份路径选项（兼容旧前端；不再强制固定根目录）
/// </summary>
public class TaktDatabaseBackupPathOptionsDto
{
    /// <summary>
    /// 已废弃：不再作为默认回填路径
    /// </summary>
    public string DefaultRoot { get; set; } = string.Empty;

    /// <summary>
    /// 已废弃：不再限制浏览/写入
    /// </summary>
    public List<string> AllowedRoots { get; set; } = new();
}

/// <summary>
/// 本地目录浏览请求
/// </summary>
public class TaktDatabaseBackupBrowseLocalDto
{
    /// <summary>
    /// 当前目录；空则返回本机驱动器列表
    /// </summary>
    public string? CurrentPath { get; set; }
}

/// <summary>
/// 本地创建目录请求（任意绝对路径，无白名单）
/// </summary>
public class TaktDatabaseBackupMkdirLocalDto
{
    /// <summary>
    /// 要创建的完整路径，例如 D:\Backup\2026
    /// </summary>
    public string Path { get; set; } = string.Empty;
}

/// <summary>
/// 网络目录浏览请求
/// </summary>
public class TaktDatabaseBackupBrowseNetworkDto
{
    /// <summary>
    /// UNC 路径
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（可选；空则用进程账号）
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// 密码（可选；空且有 DatabaseBackupId 时用库内已存密码）
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// 已有备份配置主键；浏览时可解密已存密码
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DatabaseBackupId { get; set; }
}

/// <summary>
/// FTP 目录浏览请求
/// </summary>
public class TaktDatabaseBackupBrowseFtpDto
{
    /// <summary>
    /// 服务器名称
    /// </summary>
    public string Host { get; set; } = string.Empty;
    /// <summary>
    /// 端口，默认 21
    /// </summary>
    public int? Port { get; set; }
    /// <summary>
    /// 远程目录
    /// </summary>
    public string? Path { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 密码（可空；空且有 DatabaseBackupId 时用库内已存密码）
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// 已有备份配置主键；浏览时可解密已存密码
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DatabaseBackupId { get; set; }
}
