// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopEsdCheckDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SopEsdCheck 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopEsdCheck 生成，请按需审阅）
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
// SopEsdCheck 响应 DTO
// ========================================

/// <summary>
/// SOP ESD 检查实体
/// 对应前端 TaktSopEsdCheckDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopEsdCheckDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopEsdCheckID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopEsdCheckId { get; set; }


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
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工 名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    public string? DeviceCode { get; set; } = string.Empty;

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsCompliant { get; set; } = 0;

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int LockScreenTriggered { get; set; } = 0;

    /// <summary>
    /// 检查时间
    /// </summary>
    public DateTime CheckedAt { get; set; }

    /// <summary>
    /// 工位
    /// （主表：TaktSopWorkstation）
    /// </summary>
    public TaktSopWorkstationDto? Workstation { get; set; }

}

// ========================================
// SopEsdCheck 查询 DTO
// ========================================

/// <summary>
/// SopEsdCheck 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopEsdCheckQueryDto : TaktPagedQuery
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
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    public string? DeviceCode { get; set; } = string.Empty;

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsCompliant { get; set; }

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? LockScreenTriggered { get; set; }

    /// <summary>
    /// 检查时间（范围查询-开始）
    /// </summary>
    public DateTime? CheckedAtStart { get; set; }

    /// <summary>
    /// 检查时间（范围查询-结束）
    /// </summary>
    public DateTime? CheckedAtEnd { get; set; }

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
// 创建SopEsdCheck DTO
// ========================================

/// <summary>
/// 创建SopEsdCheck DTO
/// </summary>
public class TaktSopEsdCheckCreateDto
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
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    public string? DeviceCode { get; set; } = string.Empty;

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsCompliant { get; set; } = 0;

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int LockScreenTriggered { get; set; } = 0;

    /// <summary>
    /// 检查时间
    /// </summary>
    public DateTime CheckedAt { get; set; }

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
// 更新SopEsdCheck DTO
// ========================================

/// <summary>
/// 更新SopEsdCheck DTO
/// 继承 TaktSopEsdCheckCreateDto，添加 SopEsdCheckId 字段
/// </summary>
public class TaktSopEsdCheckUpdateDto : TaktSopEsdCheckCreateDto
{
    /// <summary>
    /// SopEsdCheckID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopEsdCheckId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopEsdCheck 导入模板行 DTO
/// </summary>
public class TaktSopEsdCheckTemplateDto
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
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    public string? DeviceCode { get; set; } = string.Empty;

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsCompliant { get; set; }

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? LockScreenTriggered { get; set; }

    /// <summary>
    /// 检查时间
    /// </summary>
    public DateTime? CheckedAt { get; set; }

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
/// SopEsdCheck 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopEsdCheckImportDto
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
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    public string? DeviceCode { get; set; } = string.Empty;

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsCompliant { get; set; }

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? LockScreenTriggered { get; set; }

    /// <summary>
    /// 检查时间
    /// </summary>
    public DateTime? CheckedAt { get; set; }

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
/// SopEsdCheck 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopEsdCheckExportDto
{
    /// <summary>
    /// SopEsdCheckID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopEsdCheckId { get; set; }

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
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    public string? DeviceCode { get; set; } = string.Empty;

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsCompliant { get; set; } = 0;

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int LockScreenTriggered { get; set; } = 0;

    /// <summary>
    /// 检查时间
    /// </summary>
    public DateTime CheckedAt { get; set; }

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
