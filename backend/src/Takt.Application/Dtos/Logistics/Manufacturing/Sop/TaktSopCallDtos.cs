// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopCallDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SopCall 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopCall 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopCall 响应 DTO
// ========================================

/// <summary>
/// SOP 安灯呼叫实体
/// 对应前端 TaktSopCallDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopCallDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopCallID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopCallId { get; set; }


    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 工位 名称（填充字段）
    /// </summary>
    public string? WorkstationName { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 执行追溯 名称（填充字段）
    /// </summary>
    public string? ExecName { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    public int CallType { get; set; } = 0;

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CallerId { get; set; }

    /// <summary>
    /// 呼叫人 名称（填充字段）
    /// </summary>
    public string? CallerName { get; set; }

    /// <summary>
    /// 呼叫时间
    /// </summary>
    public DateTime CalledAt { get; set; }

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    public int CallStatus { get; set; } = 0;

    /// <summary>
    /// 工位
    /// （主表：TaktSopWorkstation）
    /// </summary>
    public TaktSopWorkstationDto? Workstation { get; set; }

}

// ========================================
// SopCall 查询 DTO
// ========================================

/// <summary>
/// SopCall 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopCallQueryDto : TaktPagedQuery
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
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    public int? CallType { get; set; }

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CallerId { get; set; }

    /// <summary>
    /// 呼叫时间（范围查询-开始）
    /// </summary>
    public DateTime? CalledAtStart { get; set; }

    /// <summary>
    /// 呼叫时间（范围查询-结束）
    /// </summary>
    public DateTime? CalledAtEnd { get; set; }

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }

    /// <summary>
    /// 响应时间（范围查询-开始）
    /// </summary>
    public DateTime? RespondedAtStart { get; set; }

    /// <summary>
    /// 响应时间（范围查询-结束）
    /// </summary>
    public DateTime? RespondedAtEnd { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    public int? CallStatus { get; set; }

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
// 创建SopCall DTO
// ========================================

/// <summary>
/// 创建SopCall DTO
/// </summary>
public class TaktSopCallCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    public int CallType { get; set; } = 0;

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CallerId { get; set; }

    /// <summary>
    /// 呼叫时间
    /// </summary>
    public DateTime CalledAt { get; set; }

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    public int CallStatus { get; set; } = 0;

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
// 更新SopCall DTO
// ========================================

/// <summary>
/// 更新SopCall DTO
/// 继承 TaktSopCallCreateDto，添加 SopCallId 字段
/// </summary>
public class TaktSopCallUpdateDto : TaktSopCallCreateDto
{
    /// <summary>
    /// SopCallID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopCallId { get; set; }

}

// ========================================
// SopCall 状态 DTO
// ========================================

/// <summary>
/// SopCall 状态更新 DTO
/// </summary>
public class TaktSopCallStatusDto
{
    /// <summary>
    /// SopCallID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopCallId { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    [Required(ErrorMessage = "呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）不能为空")]
    public int CallStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopCall 导入模板行 DTO
/// </summary>
public class TaktSopCallTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    public int? CallType { get; set; }

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CallerId { get; set; }

    /// <summary>
    /// 呼叫时间
    /// </summary>
    public DateTime? CalledAt { get; set; }

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    public int? CallStatus { get; set; }

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
/// SopCall 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopCallImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    public int? CallType { get; set; }

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CallerId { get; set; }

    /// <summary>
    /// 呼叫时间
    /// </summary>
    public DateTime? CalledAt { get; set; }

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    public int? CallStatus { get; set; }

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
/// SopCall 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopCallExportDto
{
    /// <summary>
    /// SopCallID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopCallId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
    /// </summary>
    public int CallType { get; set; } = 0;

    /// <summary>
    /// 呼叫人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CallerId { get; set; }

    /// <summary>
    /// 呼叫时间
    /// </summary>
    public DateTime CalledAt { get; set; }

    /// <summary>
    /// 响应人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RespondedBy { get; set; }

    /// <summary>
    /// 响应时间
    /// </summary>
    public DateTime? RespondedAt { get; set; }

    /// <summary>
    /// 响应时长（秒）
    /// </summary>
    public int? ResponseSeconds { get; set; }

    /// <summary>
    /// 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
    /// </summary>
    public int CallStatus { get; set; } = 0;

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
