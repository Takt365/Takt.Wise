// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktPayScaleDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：PayScale 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPayScale 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Compensation;

// ========================================
// PayScale 响应 DTO
// ========================================

/// <summary>
/// 薪级薪等（现金报酬等级带宽）
/// 对应前端 TaktPayScaleDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPayScaleDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PayScaleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayScaleId { get; set; }


    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ScaleStatus { get; set; } = 0;

}

// ========================================
// PayScale 查询 DTO
// ========================================

/// <summary>
/// PayScale 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPayScaleQueryDto : TaktPagedQuery
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
    /// 薪级编码（租户+公司内唯一）
    /// </summary>
    public string? ScaleCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪级名称
    /// </summary>
    public string? ScaleName { get; set; } = string.Empty;

    /// <summary>
    /// 等级（数字越大等级越高）
    /// </summary>
    public int? GradeLevel { get; set; }

    /// <summary>
    /// 下限金额（元）
    /// </summary>
    public decimal? MinSalary { get; set; }

    /// <summary>
    /// 中位金额（元）
    /// </summary>
    public decimal? MidSalary { get; set; }

    /// <summary>
    /// 上限金额（元）
    /// </summary>
    public decimal? MaxSalary { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ScaleStatus { get; set; }

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
// 创建PayScale DTO
// ========================================

/// <summary>
/// 创建PayScale DTO
/// </summary>
public class TaktPayScaleCreateDto
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
    /// 薪级编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "薪级编码（租户+公司内唯一）不能为空")]
    public string ScaleCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪级名称
    /// </summary>
    [Required(ErrorMessage = "薪级名称不能为空")]
    public string ScaleName { get; set; } = string.Empty;

    /// <summary>
    /// 等级（数字越大等级越高）
    /// </summary>
    public int GradeLevel { get; set; } = 0;

    /// <summary>
    /// 下限金额（元）
    /// </summary>
    public decimal MinSalary { get; set; }

    /// <summary>
    /// 中位金额（元）
    /// </summary>
    public decimal MidSalary { get; set; }

    /// <summary>
    /// 上限金额（元）
    /// </summary>
    public decimal MaxSalary { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ScaleStatus { get; set; } = 0;

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
// 更新PayScale DTO
// ========================================

/// <summary>
/// 更新PayScale DTO
/// 继承 TaktPayScaleCreateDto，添加 PayScaleId 字段
/// </summary>
public class TaktPayScaleUpdateDto : TaktPayScaleCreateDto
{
    /// <summary>
    /// PayScaleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayScaleId { get; set; }

}

// ========================================
// PayScale 状态 DTO
// ========================================

/// <summary>
/// PayScale 状态更新 DTO
/// </summary>
public class TaktPayScaleStatusDto
{
    /// <summary>
    /// PayScaleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayScaleId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int ScaleStatus { get; set; } = 0;
}

// ========================================
// PayScale 排序 DTO
// ========================================

/// <summary>
/// PayScale 排序更新 DTO
/// </summary>
public class TaktPayScaleSortDto
{
    /// <summary>
    /// PayScaleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayScaleId { get; set; }

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
/// PayScale 导入模板行 DTO
/// </summary>
public class TaktPayScaleTemplateDto
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
    /// 薪级编码（租户+公司内唯一）
    /// </summary>
    public string? ScaleCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪级名称
    /// </summary>
    public string? ScaleName { get; set; } = string.Empty;

    /// <summary>
    /// 等级（数字越大等级越高）
    /// </summary>
    public int? GradeLevel { get; set; }

    /// <summary>
    /// 下限金额（元）
    /// </summary>
    public decimal? MinSalary { get; set; }

    /// <summary>
    /// 中位金额（元）
    /// </summary>
    public decimal? MidSalary { get; set; }

    /// <summary>
    /// 上限金额（元）
    /// </summary>
    public decimal? MaxSalary { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ScaleStatus { get; set; }

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
/// PayScale 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPayScaleImportDto
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
    /// 薪级编码（租户+公司内唯一）
    /// </summary>
    public string? ScaleCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪级名称
    /// </summary>
    public string? ScaleName { get; set; } = string.Empty;

    /// <summary>
    /// 等级（数字越大等级越高）
    /// </summary>
    public int? GradeLevel { get; set; }

    /// <summary>
    /// 下限金额（元）
    /// </summary>
    public decimal? MinSalary { get; set; }

    /// <summary>
    /// 中位金额（元）
    /// </summary>
    public decimal? MidSalary { get; set; }

    /// <summary>
    /// 上限金额（元）
    /// </summary>
    public decimal? MaxSalary { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? ScaleStatus { get; set; }

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
/// PayScale 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPayScaleExportDto
{
    /// <summary>
    /// PayScaleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayScaleId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪级编码（租户+公司内唯一）
    /// </summary>
    public string ScaleCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪级名称
    /// </summary>
    public string ScaleName { get; set; } = string.Empty;

    /// <summary>
    /// 等级（数字越大等级越高）
    /// </summary>
    public int GradeLevel { get; set; } = 0;

    /// <summary>
    /// 下限金额（元）
    /// </summary>
    public decimal MinSalary { get; set; }

    /// <summary>
    /// 中位金额（元）
    /// </summary>
    public decimal MidSalary { get; set; }

    /// <summary>
    /// 上限金额（元）
    /// </summary>
    public decimal MaxSalary { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int ScaleStatus { get; set; } = 0;

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
