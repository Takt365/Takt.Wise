// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Database
// 文件名称：TaktTableArchiveDtos.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：TableArchive 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTableArchive 生成，请按需审阅）
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
// TableArchive 响应 DTO
// ========================================

/// <summary>
/// 数据表归档（按表登记归档键与热库保留年数）
/// 对应前端 TaktTableArchiveDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTableArchiveDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TableArchiveID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TableArchiveId { get; set; }

    /// <summary>
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    public string ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    public int ArchiveKeyKind { get; set; } = 3;

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    public int RetainHotYears { get; set; } = 1;

    /// <summary>
    /// 归档名称（物理表名_归档键类型码，服务端生成）
    /// </summary>
    public string ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int ArchiveStatus { get; set; } = 0;

}

// ========================================
// TableArchive 查询 DTO
// ========================================

/// <summary>
/// TableArchive 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTableArchiveQueryDto : TaktPagedQuery
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
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    public string? TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    public string? TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    public string? ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    public int? ArchiveKeyKind { get; set; }

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    public int? RetainHotYears { get; set; }

    /// <summary>
    /// 归档名称（物理表名_归档键类型码，服务端生成）
    /// </summary>
    public string? ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? ArchiveStatus { get; set; }

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
// 创建TableArchive DTO
// ========================================

/// <summary>
/// 创建TableArchive DTO
/// </summary>
public class TaktTableArchiveCreateDto
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
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    [Required(ErrorMessage = "目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）不能为空")]
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    [Required(ErrorMessage = "目标数据库展示名（与 DatabaseInfos DisplayName 一致）不能为空")]
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    [Required(ErrorMessage = "物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）不能为空")]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    [Required(ErrorMessage = "归档键列名（如 costing_date；小写蛇形，与物理列一致）不能为空")]
    public string ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    public int ArchiveKeyKind { get; set; } = 3;

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    public int RetainHotYears { get; set; } = 1;

    /// <summary>
    /// 归档名称（服务端按物理表名_归档键类型码生成；客户端可忽略）
    /// </summary>
    public string ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int ArchiveStatus { get; set; } = 0;

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
// 更新TableArchive DTO
// ========================================

/// <summary>
/// 更新TableArchive DTO
/// 继承 TaktTableArchiveCreateDto，添加 TableArchiveId 字段
/// </summary>
public class TaktTableArchiveUpdateDto : TaktTableArchiveCreateDto
{
    /// <summary>
    /// TableArchiveID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TableArchiveId { get; set; }

}

// ========================================
// TableArchive 状态 DTO
// ========================================

/// <summary>
/// TableArchive 状态更新 DTO
/// </summary>
public class TaktTableArchiveStatusDto
{
    /// <summary>
    /// TableArchiveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TableArchiveId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int ArchiveStatus { get; set; } = 0;
}

// ========================================
// TableArchive 排序 DTO
// ========================================

/// <summary>
/// TableArchive 排序更新 DTO
/// </summary>
public class TaktTableArchiveSortDto
{
    /// <summary>
    /// TableArchiveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TableArchiveId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TableArchive 导入模板行 DTO
/// </summary>
public class TaktTableArchiveTemplateDto
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
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    public string? TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    public string? TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    public string? ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    public int? ArchiveKeyKind { get; set; }

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    public int? RetainHotYears { get; set; }

    /// <summary>
    /// 归档名称（物理表名_归档键类型码，服务端生成）
    /// </summary>
    public string? ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? ArchiveStatus { get; set; }

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
/// TableArchive 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTableArchiveImportDto
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
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    public string? TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    public string? TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    public string? ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    public int? ArchiveKeyKind { get; set; }

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    public int? RetainHotYears { get; set; }

    /// <summary>
    /// 归档名称（物理表名_归档键类型码，服务端生成）
    /// </summary>
    public string? ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? ArchiveStatus { get; set; }

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
/// TableArchive 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTableArchiveExportDto
{
    /// <summary>
    /// TableArchiveID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TableArchiveId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    public string ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    public int ArchiveKeyKind { get; set; } = 3;

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    public int RetainHotYears { get; set; } = 1;

    /// <summary>
    /// 归档名称（物理表名_归档键类型码，服务端生成）
    /// </summary>
    public string ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int ArchiveStatus { get; set; } = 0;

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

// ========================================
// 数据归档编排 DTO
// ========================================

/// <summary>
/// 按年归档预览/执行请求 DTO
/// </summary>
public class TaktTableArchiveExecuteDto
{
    /// <summary>
    /// 策略主键列表（string，与前端雪花 ID 对齐）
    /// </summary>
    public List<string> PolicyIds { get; set; } = new();

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }
}

/// <summary>
/// 单策略归档预览项
/// </summary>
public class TaktTableArchivePreviewItemDto
{
    /// <summary>
    /// 策略主键
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PolicyId { get; set; }

    /// <summary>
    /// 归档名称（物理表名_归档键类型码，服务端生成）
    /// </summary>
    public string ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 源物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标表名（{table}_{yyyy}）
    /// </summary>
    public string ArchiveTableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 将迁移行数
    /// </summary>
    public int SourceRowCount { get; set; }
}

/// <summary>
/// 归档预览聚合结果
/// </summary>
public class TaktTableArchivePreviewResultDto
{
    /// <summary>
    /// 各策略预览项
    /// </summary>
    public List<TaktTableArchivePreviewItemDto> Items { get; set; } = new();

    /// <summary>
    /// 合计将迁移行数
    /// </summary>
    public int TotalRowCount { get; set; }
}

/// <summary>
/// 单策略归档执行项
/// </summary>
public class TaktTableArchiveExecuteItemDto
{
    /// <summary>
    /// 策略主键
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PolicyId { get; set; }

    /// <summary>
    /// 源物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标表名
    /// </summary>
    public string ArchiveTableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配行数
    /// </summary>
    public int SourceRowCount { get; set; }

    /// <summary>
    /// 实际归档行数
    /// </summary>
    public int ArchivedRowCount { get; set; }

    /// <summary>
    /// 从热表删除行数
    /// </summary>
    public int DeletedRowCount { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 失败错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 归档执行聚合结果
/// </summary>
public class TaktTableArchiveExecuteResultDto
{
    /// <summary>
    /// 各策略执行项
    /// </summary>
    public List<TaktTableArchiveExecuteItemDto> Items { get; set; } = new();
}

/// <summary>
/// 预建年分表请求 DTO
/// </summary>
public class TaktTableEnsureYearTablesDto
{
    /// <summary>
    /// 策略主键（string）
    /// </summary>
    public string PolicyId { get; set; } = string.Empty;

    /// <summary>
    /// 年份列表
    /// </summary>
    public List<int> Years { get; set; } = new();
}

/// <summary>
/// 预建年分表结果 DTO
/// </summary>
public class TaktTableEnsureYearTablesResultDto
{
    /// <summary>
    /// 策略主键
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PolicyId { get; set; }

    /// <summary>
    /// 源物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 已就绪年分表名列表
    /// </summary>
    public List<string> YearTableNames { get; set; } = new();
}

/// <summary>
/// 按年归档调度请求（立即/后台均创建 Quartz 一次性任务）
/// </summary>
public class TaktTableArchiveScheduleDto
{
    /// <summary>
    /// 策略主键列表（string）
    /// </summary>
    public List<string> PolicyIds { get; set; } = new();

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 计划执行时间（后台执行必填且须晚于当前时间；立即执行可空）
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
}

/// <summary>
/// 按年归档调度结果（已创建 Quartz 任务）
/// </summary>
public class TaktTableArchiveScheduleResultDto
{
    /// <summary>
    /// Quartz 任务主键
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务编码
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; }

    /// <summary>
    /// 计划/触发时间
    /// </summary>
    public DateTime ScheduledAt { get; set; }

    /// <summary>
    /// 归档年份
    /// </summary>
    public int ArchiveYear { get; set; }

    /// <summary>
    /// 策略主键列表
    /// </summary>
    public List<string> PolicyIds { get; set; } = new();
}
