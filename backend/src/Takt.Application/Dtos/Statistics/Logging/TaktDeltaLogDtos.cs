// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktDeltaLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：DeltaLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDeltaLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Statistics.Logging;

// ========================================
// DeltaLog 响应 DTO
// ========================================

/// <summary>
/// 差异日志实体（AOP 审计）
/// 对应前端 TaktDeltaLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDeltaLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DeltaLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeltaLogId { get; set; }

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，如 create=新增、update=修改、delete=删除、query=查询）
    /// </summary>
    public string OperType { get; set; } = string.Empty;

    /// <summary>
    /// 数据库表名（SugarTable 物理表名）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 业务主键 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryKeyId { get; set; }

    /// <summary>
    /// 业务主键 名称（填充字段）
    /// </summary>
    public string? PrimaryKeyName { get; set; }

    /// <summary>
    /// 修改前数据 JSON（旧值快照；无数据为空串）
    /// </summary>
    public string BeforeData { get; set; } = string.Empty;

    /// <summary>
    /// 修改后数据 JSON（新值快照；无数据为空串）
    /// </summary>
    public string AfterData { get; set; } = string.Empty;

    /// <summary>
    /// 差异内容 JSON（变更字段及旧/新值明细；无差异为空串）
    /// </summary>
    public string DiffData { get; set; } = string.Empty;

    /// <summary>
    /// 执行的 SQL 语句（AOP 捕获；无 SQL 为空串）
    /// </summary>
    public string SqlStatement { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（数据变更发生时刻）
    /// </summary>
    public DateTime OperTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ElapsedTime { get; set; } = 0;

}

// ========================================
// DeltaLog 查询 DTO
// ========================================

/// <summary>
/// DeltaLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDeltaLogQueryDto : TaktPagedQuery
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
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，如 create=新增、update=修改、delete=删除、query=查询）
    /// </summary>
    public string? OperType { get; set; } = string.Empty;

    /// <summary>
    /// 数据库表名（SugarTable 物理表名）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 业务主键 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryKeyId { get; set; }

    /// <summary>
    /// 修改前数据 JSON（旧值快照；无数据为空串）
    /// </summary>
    public string? BeforeData { get; set; } = string.Empty;

    /// <summary>
    /// 修改后数据 JSON（新值快照；无数据为空串）
    /// </summary>
    public string? AfterData { get; set; } = string.Empty;

    /// <summary>
    /// 差异内容 JSON（变更字段及旧/新值明细；无差异为空串）
    /// </summary>
    public string? DiffData { get; set; } = string.Empty;

    /// <summary>
    /// 执行的 SQL 语句（AOP 捕获；无 SQL 为空串）
    /// </summary>
    public string? SqlStatement { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string? OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    public string? OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string? Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string? Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    public string? DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（数据变更发生时刻）（范围查询-开始）
    /// </summary>
    public DateTime? OperTimeStart { get; set; }

    /// <summary>
    /// 操作时间（数据变更发生时刻）（范围查询-结束）
    /// </summary>
    public DateTime? OperTimeEnd { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int? ElapsedTime { get; set; }

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
// 创建DeltaLog DTO
// ========================================

/// <summary>
/// 创建DeltaLog DTO
/// </summary>
public class TaktDeltaLogCreateDto
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
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    [Required(ErrorMessage = "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，如 create=新增、update=修改、delete=删除、query=查询）
    /// </summary>
    [Required(ErrorMessage = "操作类型（TaktConstants.OperType，如 create=新增、update=修改、delete=删除、query=查询）不能为空")]
    public string OperType { get; set; } = string.Empty;

    /// <summary>
    /// 数据库表名（SugarTable 物理表名）
    /// </summary>
    [Required(ErrorMessage = "数据库表名（SugarTable 物理表名）不能为空")]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 业务主键 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryKeyId { get; set; }

    /// <summary>
    /// 修改前数据 JSON（旧值快照；无数据为空串）
    /// </summary>
    [Required(ErrorMessage = "修改前数据 JSON（旧值快照；无数据为空串）不能为空")]
    public string BeforeData { get; set; } = string.Empty;

    /// <summary>
    /// 修改后数据 JSON（新值快照；无数据为空串）
    /// </summary>
    [Required(ErrorMessage = "修改后数据 JSON（新值快照；无数据为空串）不能为空")]
    public string AfterData { get; set; } = string.Empty;

    /// <summary>
    /// 差异内容 JSON（变更字段及旧/新值明细；无差异为空串）
    /// </summary>
    [Required(ErrorMessage = "差异内容 JSON（变更字段及旧/新值明细；无差异为空串）不能为空")]
    public string DiffData { get; set; } = string.Empty;

    /// <summary>
    /// 执行的 SQL 语句（AOP 捕获；无 SQL 为空串）
    /// </summary>
    [Required(ErrorMessage = "执行的 SQL 语句（AOP 捕获；无 SQL 为空串）不能为空")]
    public string SqlStatement { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    [Required(ErrorMessage = "操作 IP不能为空")]
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    [Required(ErrorMessage = "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）不能为空")]
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [Required(ErrorMessage = "用户代理（User-Agent）不能为空")]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "浏览器（TaktConstants.BrowserType，默认 unknown）不能为空")]
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "操作系统（TaktConstants.OperatingSystem，默认 unknown）不能为空")]
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    [Required(ErrorMessage = "登录设备（TaktConstants.DeviceType，默认 unknown）不能为空")]
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（数据变更发生时刻）
    /// </summary>
    public DateTime OperTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ElapsedTime { get; set; } = 0;

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
// 更新DeltaLog DTO
// ========================================

/// <summary>
/// 更新DeltaLog DTO
/// 继承 TaktDeltaLogCreateDto，添加 DeltaLogId 字段
/// </summary>
public class TaktDeltaLogUpdateDto : TaktDeltaLogCreateDto
{
    /// <summary>
    /// DeltaLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeltaLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// DeltaLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDeltaLogExportDto
{
    /// <summary>
    /// DeltaLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeltaLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType，如 create=新增、update=修改、delete=删除、query=查询）
    /// </summary>
    public string OperType { get; set; } = string.Empty;

    /// <summary>
    /// 数据库表名（SugarTable 物理表名）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 业务主键 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryKeyId { get; set; }

    /// <summary>
    /// 修改前数据 JSON（旧值快照；无数据为空串）
    /// </summary>
    public string BeforeData { get; set; } = string.Empty;

    /// <summary>
    /// 修改后数据 JSON（新值快照；无数据为空串）
    /// </summary>
    public string AfterData { get; set; } = string.Empty;

    /// <summary>
    /// 差异内容 JSON（变更字段及旧/新值明细；无差异为空串）
    /// </summary>
    public string DiffData { get; set; } = string.Empty;

    /// <summary>
    /// 执行的 SQL 语句（AOP 捕获；无 SQL 为空串）
    /// </summary>
    public string SqlStatement { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，默认 unknown）
    /// </summary>
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，默认 unknown）
    /// </summary>
    public string Os { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，默认 unknown）
    /// </summary>
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 操作时间（数据变更发生时刻）
    /// </summary>
    public DateTime OperTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ElapsedTime { get; set; } = 0;

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
