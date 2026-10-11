// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：WorkCenter 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktWorkCenter 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Aps;

// ========================================
// WorkCenter 响应 DTO
// ========================================

/// <summary>
/// 工作中心（WC；PlantCode 对齐 TaktCalendar.RelatedPlant）
/// 对应前端 TaktWorkCenterDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktWorkCenterDto : TaktCompanyDtoBase
{
    /// <summary>
    /// WorkCenterID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkCenterId { get; set; }

    /// <summary>
    /// 工作中心编码
    /// </summary>
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int WorkCenterStatus { get; set; } = 0;

    /// <summary>
    /// 工作中心资源列表
    /// （子表：TaktWorkCenterResource）
    /// </summary>
    public List<TaktWorkCenterResourceDto>? Resources { get; set; }

}

// ========================================
// WorkCenter 查询 DTO
// ========================================

/// <summary>
/// WorkCenter 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktWorkCenterQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 工作中心编码
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? WorkCenterStatus { get; set; }

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
// 创建WorkCenter DTO
// ========================================

/// <summary>
/// 创建WorkCenter DTO
/// </summary>
public class TaktWorkCenterCreateDto
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
    /// 工作中心编码
    /// </summary>
    [Required(ErrorMessage = "工作中心编码不能为空")]
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    [Required(ErrorMessage = "工作中心描述不能为空")]
    public string WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int WorkCenterStatus { get; set; } = 0;

    /// <summary>
    /// 工作中心资源列表（子表，级联保存）
    /// </summary>
    public List<TaktWorkCenterResourceCreateDto>? Resources { get; set; }

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
// 更新WorkCenter DTO
// ========================================

/// <summary>
/// 更新WorkCenter DTO
/// 继承 TaktWorkCenterCreateDto，添加 WorkCenterId 字段
/// </summary>
public class TaktWorkCenterUpdateDto : TaktWorkCenterCreateDto
{
    /// <summary>
    /// WorkCenterID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkCenterId { get; set; }

    /// <summary>
    /// 工作中心资源列表（子表，级联保存）
    /// </summary>
    public new List<TaktWorkCenterResourceUpdateDto>? Resources { get; set; }

}

// ========================================
// WorkCenter 状态 DTO
// ========================================

/// <summary>
/// WorkCenter 状态更新 DTO
/// </summary>
public class TaktWorkCenterStatusDto
{
    /// <summary>
    /// WorkCenterID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkCenterId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int WorkCenterStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// WorkCenter 导入模板行 DTO
/// </summary>
public class TaktWorkCenterTemplateDto
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
    /// 工作中心编码
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? WorkCenterStatus { get; set; }

    /// <summary>
    /// 工作中心资源列表（子表，级联保存）
    /// </summary>
    public List<TaktWorkCenterResourceCreateDto>? Resources { get; set; }

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
/// WorkCenter 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktWorkCenterImportDto
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
    /// 工作中心编码
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string? WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? WorkCenterStatus { get; set; }

    /// <summary>
    /// 工作中心资源列表（子表，级联保存）
    /// </summary>
    public List<TaktWorkCenterResourceCreateDto>? Resources { get; set; }

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
/// WorkCenter 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktWorkCenterExportDto
{
    /// <summary>
    /// WorkCenterID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkCenterId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码
    /// </summary>
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心描述
    /// </summary>
    public string WorkCenterDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int WorkCenterStatus { get; set; } = 0;

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
