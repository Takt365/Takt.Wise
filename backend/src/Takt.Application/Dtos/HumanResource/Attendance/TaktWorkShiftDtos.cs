// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Attendance
// 文件名称：TaktWorkShiftDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：WorkShift 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktWorkShift 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Attendance;

// ========================================
// WorkShift 响应 DTO
// ========================================

/// <summary>
/// 班次定义（如早班、中班、夜班）
/// 对应前端 TaktWorkShiftDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktWorkShiftDto : TaktCompanyDtoBase
{
    /// <summary>
    /// WorkShiftID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkShiftId { get; set; }


    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// WorkShift 查询 DTO
// ========================================

/// <summary>
/// WorkShift 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktWorkShiftQueryDto : TaktPagedQuery
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
    /// 班次编码（租户+公司内唯一）
    /// </summary>
    public string? ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次名称
    /// </summary>
    public string? ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// 当班开始时间（HH:mm）
    /// </summary>
    public string? StartTime { get; set; } = string.Empty;

    /// <summary>
    /// 当班结束时间（HH:mm）
    /// </summary>
    public string? EndTime { get; set; } = string.Empty;

    /// <summary>
    /// 是否跨自然日（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? CrossMidnight { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建WorkShift DTO
// ========================================

/// <summary>
/// 创建WorkShift DTO
/// </summary>
public class TaktWorkShiftCreateDto
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
    /// 班次编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "班次编码（租户+公司内唯一）不能为空")]
    public string ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次名称
    /// </summary>
    [Required(ErrorMessage = "班次名称不能为空")]
    public string ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// 当班开始时间（HH:mm）
    /// </summary>
    [Required(ErrorMessage = "当班开始时间（HH:mm）不能为空")]
    public string StartTime { get; set; } = string.Empty;

    /// <summary>
    /// 当班结束时间（HH:mm）
    /// </summary>
    [Required(ErrorMessage = "当班结束时间（HH:mm）不能为空")]
    public string EndTime { get; set; } = string.Empty;

    /// <summary>
    /// 是否跨自然日（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int CrossMidnight { get; set; } = 0;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

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
// 更新WorkShift DTO
// ========================================

/// <summary>
/// 更新WorkShift DTO
/// 继承 TaktWorkShiftCreateDto，添加 WorkShiftId 字段
/// </summary>
public class TaktWorkShiftUpdateDto : TaktWorkShiftCreateDto
{
    /// <summary>
    /// WorkShiftID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkShiftId { get; set; }

}

// ========================================
// WorkShift 排序 DTO
// ========================================

/// <summary>
/// WorkShift 排序更新 DTO
/// </summary>
public class TaktWorkShiftSortDto
{
    /// <summary>
    /// WorkShiftID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkShiftId { get; set; }

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
/// WorkShift 导入模板行 DTO
/// </summary>
public class TaktWorkShiftTemplateDto
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
    /// 班次编码（租户+公司内唯一）
    /// </summary>
    public string? ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次名称
    /// </summary>
    public string? ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// 当班开始时间（HH:mm）
    /// </summary>
    public string? StartTime { get; set; } = string.Empty;

    /// <summary>
    /// 当班结束时间（HH:mm）
    /// </summary>
    public string? EndTime { get; set; } = string.Empty;

    /// <summary>
    /// 是否跨自然日（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? CrossMidnight { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

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
/// WorkShift 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktWorkShiftImportDto
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
    /// 班次编码（租户+公司内唯一）
    /// </summary>
    public string? ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次名称
    /// </summary>
    public string? ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// 当班开始时间（HH:mm）
    /// </summary>
    public string? StartTime { get; set; } = string.Empty;

    /// <summary>
    /// 当班结束时间（HH:mm）
    /// </summary>
    public string? EndTime { get; set; } = string.Empty;

    /// <summary>
    /// 是否跨自然日（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? CrossMidnight { get; set; }

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

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
/// WorkShift 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktWorkShiftExportDto
{
    /// <summary>
    /// WorkShiftID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkShiftId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次编码（租户+公司内唯一）
    /// </summary>
    public string ShiftCode { get; set; } = string.Empty;

    /// <summary>
    /// 班次名称
    /// </summary>
    public string ShiftName { get; set; } = string.Empty;

    /// <summary>
    /// 当班开始时间（HH:mm）
    /// </summary>
    public string StartTime { get; set; } = string.Empty;

    /// <summary>
    /// 当班结束时间（HH:mm）
    /// </summary>
    public string EndTime { get; set; } = string.Empty;

    /// <summary>
    /// 是否跨自然日（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int CrossMidnight { get; set; } = 0;

    /// <summary>
    /// 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
