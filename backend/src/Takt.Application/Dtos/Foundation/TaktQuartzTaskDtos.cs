// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktQuartzTaskDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：QuartzTask 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQuartzTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// QuartzTask 响应 DTO
// ========================================

/// <summary>
/// Quartz 定时任务实体
/// 对应前端 TaktQuartzTaskDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQuartzTaskDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QuartzTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    public int TriggerType { get; set; } = 0;

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int IntervalSeconds { get; set; } = 0;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int Concurrent { get; set; } = 0;

    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public int MisfirePolicy { get; set; } = 0;

    /// <summary>
    /// 首次执行（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecuteCount { get; set; } = 0;

    /// <summary>
    /// 上次执行
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? TaskDescription { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

}

// ========================================
// QuartzTask 查询 DTO
// ========================================

/// <summary>
/// QuartzTask 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQuartzTaskQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string? TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string? JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    public string? TaskType { get; set; } = string.Empty;

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string? ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    public int? TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int? IntervalSeconds { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public int? MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行（调度生效开始时间）（范围查询-开始）
    /// </summary>
    public DateTime? FirstRunAtStart { get; set; }

    /// <summary>
    /// 首次执行（调度生效开始时间）（范围查询-结束）
    /// </summary>
    public DateTime? FirstRunAtEnd { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int? ExecuteCount { get; set; }

    /// <summary>
    /// 上次执行（范围查询-开始）
    /// </summary>
    public DateTime? LastRunAtStart { get; set; }

    /// <summary>
    /// 上次执行（范围查询-结束）
    /// </summary>
    public DateTime? LastRunAtEnd { get; set; }

    /// <summary>
    /// 下次执行（范围查询-开始）
    /// </summary>
    public DateTime? NextRunAtStart { get; set; }

    /// <summary>
    /// 下次执行（范围查询-结束）
    /// </summary>
    public DateTime? NextRunAtEnd { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? TaskDescription { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    public int? TaskStatus { get; set; }

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
// 创建QuartzTask DTO
// ========================================

/// <summary>
/// 创建QuartzTask DTO
/// </summary>
public class TaktQuartzTaskCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "任务编码（租户+公司内唯一）不能为空")]
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    [Required(ErrorMessage = "任务名称不能为空")]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    [Required(ErrorMessage = "Quartz Job 名称不能为空")]
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    [Required(ErrorMessage = "Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）不能为空")]
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    [Required(ErrorMessage = "任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）不能为空")]
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    [Required(ErrorMessage = "程序集名称（任务类型为程序集时使用）不能为空")]
    public string AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    [Required(ErrorMessage = "任务类名（任务类型为程序集时使用）不能为空")]
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    public int TriggerType { get; set; } = 0;

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    [Required(ErrorMessage = "Cron 表达式（触发器类型为 Cron 时使用）不能为空")]
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int IntervalSeconds { get; set; } = 0;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int Concurrent { get; set; } = 0;

    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public int MisfirePolicy { get; set; } = 0;

    /// <summary>
    /// 首次执行（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecuteCount { get; set; } = 0;

    /// <summary>
    /// 上次执行
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? TaskDescription { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

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
// 更新QuartzTask DTO
// ========================================

/// <summary>
/// 更新QuartzTask DTO
/// 继承 TaktQuartzTaskCreateDto，添加 QuartzTaskId 字段
/// </summary>
public class TaktQuartzTaskUpdateDto : TaktQuartzTaskCreateDto
{
    /// <summary>
    /// QuartzTaskID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

}

// ========================================
// QuartzTask 状态 DTO
// ========================================

/// <summary>
/// QuartzTask 状态更新 DTO
/// </summary>
public class TaktQuartzTaskStatusDto
{
    /// <summary>
    /// QuartzTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    [Required(ErrorMessage = "任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）不能为空")]
    public int TaskStatus { get; set; } = 0;
}

// ========================================
// QuartzTask 立即执行 DTO
// ========================================

/// <summary>
/// QuartzTask 立即执行请求 DTO（可选覆盖执行参数）
/// </summary>
public class TaktQuartzTaskExecuteDto
{
    /// <summary>
    /// 本次触发执行参数（非空则覆盖任务配置 ExecuteParams）
    /// </summary>
    public string? ExecuteParams { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QuartzTask 导入模板行 DTO
/// </summary>
public class TaktQuartzTaskTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string? TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string? JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    public string? TaskType { get; set; } = string.Empty;

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string? ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    public int? TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int? IntervalSeconds { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public int? MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int? ExecuteCount { get; set; }

    /// <summary>
    /// 上次执行
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? TaskDescription { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    public int? TaskStatus { get; set; }

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
/// QuartzTask 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQuartzTaskImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string? TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string? JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    public string? JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    public string? TaskType { get; set; } = string.Empty;

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string? AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string? ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    public int? TriggerType { get; set; }

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string? CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int? IntervalSeconds { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? Concurrent { get; set; }

    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public int? MisfirePolicy { get; set; }

    /// <summary>
    /// 首次执行（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int? ExecuteCount { get; set; }

    /// <summary>
    /// 上次执行
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? TaskDescription { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    public int? TaskStatus { get; set; }

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
/// QuartzTask 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQuartzTaskExportDto
{
    /// <summary>
    /// QuartzTaskID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    public string JobName { get; set; } = string.Empty;

    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    public string JobGroup { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    public string AssemblyName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    public string? ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    public string? RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    public string? SqlScript { get; set; } = string.Empty;

    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    public int TriggerType { get; set; } = 0;

    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    public int IntervalSeconds { get; set; } = 0;

    /// <summary>
    /// 执行参数
    /// </summary>
    public string? ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int Concurrent { get; set; } = 0;

    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    public int MisfirePolicy { get; set; } = 0;

    /// <summary>
    /// 首次执行（调度生效开始时间）
    /// </summary>
    public DateTime? FirstRunAt { get; set; }

    /// <summary>
    /// 执行次数
    /// </summary>
    public int ExecuteCount { get; set; } = 0;

    /// <summary>
    /// 上次执行
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 任务描述
    /// </summary>
    public string? TaskDescription { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

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
