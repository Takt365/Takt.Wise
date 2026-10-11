// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktDurationLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：DurationLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDurationLog 生成，请按需审阅）
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
// DurationLog 响应 DTO
// ========================================

/// <summary>
/// 在线时长日志实体（日汇总）
/// 对应前端 TaktDurationLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDurationLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DurationLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DurationLogId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 统计日期（自然日，不含时分秒）
    /// </summary>
    public DateTime StatDate { get; set; }

    /// <summary>
    /// 当日累计在线时长（秒）
    /// </summary>
    public int DurationSeconds { get; set; } = 0;

}

// ========================================
// DurationLog 查询 DTO
// ========================================

/// <summary>
/// DurationLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDurationLogQueryDto : TaktPagedQuery
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
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 统计日期（自然日，不含时分秒）（范围查询-开始）
    /// </summary>
    public DateTime? StatDateStart { get; set; }

    /// <summary>
    /// 统计日期（自然日，不含时分秒）（范围查询-结束）
    /// </summary>
    public DateTime? StatDateEnd { get; set; }

    /// <summary>
    /// 当日累计在线时长（秒）
    /// </summary>
    public int? DurationSeconds { get; set; }

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
// 创建DurationLog DTO
// ========================================

/// <summary>
/// 创建DurationLog DTO
/// </summary>
public class TaktDurationLogCreateDto
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
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    [Required(ErrorMessage = "用户名（登录账号）不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 统计日期（自然日，不含时分秒）
    /// </summary>
    public DateTime StatDate { get; set; }

    /// <summary>
    /// 当日累计在线时长（秒）
    /// </summary>
    public int DurationSeconds { get; set; } = 0;

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
// 更新DurationLog DTO
// ========================================

/// <summary>
/// 更新DurationLog DTO
/// 继承 TaktDurationLogCreateDto，添加 DurationLogId 字段
/// </summary>
public class TaktDurationLogUpdateDto : TaktDurationLogCreateDto
{
    /// <summary>
    /// DurationLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DurationLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// DurationLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDurationLogExportDto
{
    /// <summary>
    /// DurationLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DurationLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 统计日期（自然日，不含时分秒）
    /// </summary>
    public DateTime StatDate { get; set; }

    /// <summary>
    /// 当日累计在线时长（秒）
    /// </summary>
    public int DurationSeconds { get; set; } = 0;

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
