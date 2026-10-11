// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Controlling
// 文件名称：TaktStandardWageRateDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：StandardWageRate 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktStandardWageRate 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Controlling;

// ========================================
// StandardWageRate 响应 DTO
// ========================================

/// <summary>
/// 标准工资率实体
/// 对应前端 TaktStandardWageRateDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktStandardWageRateDto : TaktCompanyDtoBase
{
    /// <summary>
    /// StandardWageRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardWageRateId { get; set; }


}

// ========================================
// StandardWageRate 查询 DTO
// ========================================

/// <summary>
/// StandardWageRate 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktStandardWageRateQueryDto : TaktPagedQuery
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
    /// 年月（yyyyMM）
    /// </summary>
    public string? YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 工作天数
    /// </summary>
    public decimal? WorkingDays { get; set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal? SalesAmount { get; set; }

    /// <summary>
    /// 直接人数
    /// </summary>
    public int? DirectLaborCount { get; set; }

    /// <summary>
    /// 直接工资
    /// </summary>
    public decimal? DirectLaborWage { get; set; }

    /// <summary>
    /// 直接加班小时
    /// </summary>
    public decimal? DirectOvertimeHours { get; set; }

    /// <summary>
    /// 直接加班总额
    /// </summary>
    public decimal? DirectOvertimeTotal { get; set; }

    /// <summary>
    /// 直接工资率
    /// </summary>
    public decimal? DirectWageRate { get; set; }

    /// <summary>
    /// 间接人数
    /// </summary>
    public int? IndirectLaborCount { get; set; }

    /// <summary>
    /// 间接工资
    /// </summary>
    public decimal? IndirectLaborWage { get; set; }

    /// <summary>
    /// 间接加班小时
    /// </summary>
    public decimal? IndirectOvertimeHours { get; set; }

    /// <summary>
    /// 间接加班总额
    /// </summary>
    public decimal? IndirectOvertimeTotal { get; set; }

    /// <summary>
    /// 间接工资率
    /// </summary>
    public decimal? IndirectWageRate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

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
// 创建StandardWageRate DTO
// ========================================

/// <summary>
/// 创建StandardWageRate DTO
/// </summary>
public class TaktStandardWageRateCreateDto
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
    /// 年月（yyyyMM）
    /// </summary>
    [Required(ErrorMessage = "年月（yyyyMM）不能为空")]
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 工作天数
    /// </summary>
    public decimal WorkingDays { get; set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal SalesAmount { get; set; }

    /// <summary>
    /// 直接人数
    /// </summary>
    public int DirectLaborCount { get; set; } = 0;

    /// <summary>
    /// 直接工资
    /// </summary>
    public decimal DirectLaborWage { get; set; }

    /// <summary>
    /// 直接加班小时
    /// </summary>
    public decimal DirectOvertimeHours { get; set; }

    /// <summary>
    /// 直接加班总额
    /// </summary>
    public decimal DirectOvertimeTotal { get; set; }

    /// <summary>
    /// 直接工资率
    /// </summary>
    public decimal DirectWageRate { get; set; }

    /// <summary>
    /// 间接人数
    /// </summary>
    public int IndirectLaborCount { get; set; } = 0;

    /// <summary>
    /// 间接工资
    /// </summary>
    public decimal IndirectLaborWage { get; set; }

    /// <summary>
    /// 间接加班小时
    /// </summary>
    public decimal IndirectOvertimeHours { get; set; }

    /// <summary>
    /// 间接加班总额
    /// </summary>
    public decimal IndirectOvertimeTotal { get; set; }

    /// <summary>
    /// 间接工资率
    /// </summary>
    public decimal IndirectWageRate { get; set; }

    /// <summary>
    /// 关联工厂
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
// 更新StandardWageRate DTO
// ========================================

/// <summary>
/// 更新StandardWageRate DTO
/// 继承 TaktStandardWageRateCreateDto，添加 StandardWageRateId 字段
/// </summary>
public class TaktStandardWageRateUpdateDto : TaktStandardWageRateCreateDto
{
    /// <summary>
    /// StandardWageRateID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardWageRateId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// StandardWageRate 导入模板行 DTO
/// </summary>
public class TaktStandardWageRateTemplateDto
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
    /// 年月（yyyyMM）
    /// </summary>
    public string? YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 工作天数
    /// </summary>
    public decimal? WorkingDays { get; set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal? SalesAmount { get; set; }

    /// <summary>
    /// 直接人数
    /// </summary>
    public int? DirectLaborCount { get; set; }

    /// <summary>
    /// 直接工资
    /// </summary>
    public decimal? DirectLaborWage { get; set; }

    /// <summary>
    /// 直接加班小时
    /// </summary>
    public decimal? DirectOvertimeHours { get; set; }

    /// <summary>
    /// 直接加班总额
    /// </summary>
    public decimal? DirectOvertimeTotal { get; set; }

    /// <summary>
    /// 直接工资率
    /// </summary>
    public decimal? DirectWageRate { get; set; }

    /// <summary>
    /// 间接人数
    /// </summary>
    public int? IndirectLaborCount { get; set; }

    /// <summary>
    /// 间接工资
    /// </summary>
    public decimal? IndirectLaborWage { get; set; }

    /// <summary>
    /// 间接加班小时
    /// </summary>
    public decimal? IndirectOvertimeHours { get; set; }

    /// <summary>
    /// 间接加班总额
    /// </summary>
    public decimal? IndirectOvertimeTotal { get; set; }

    /// <summary>
    /// 间接工资率
    /// </summary>
    public decimal? IndirectWageRate { get; set; }

    /// <summary>
    /// 关联工厂
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
/// StandardWageRate 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktStandardWageRateImportDto
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
    /// 年月（yyyyMM）
    /// </summary>
    public string? YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 工作天数
    /// </summary>
    public decimal? WorkingDays { get; set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal? SalesAmount { get; set; }

    /// <summary>
    /// 直接人数
    /// </summary>
    public int? DirectLaborCount { get; set; }

    /// <summary>
    /// 直接工资
    /// </summary>
    public decimal? DirectLaborWage { get; set; }

    /// <summary>
    /// 直接加班小时
    /// </summary>
    public decimal? DirectOvertimeHours { get; set; }

    /// <summary>
    /// 直接加班总额
    /// </summary>
    public decimal? DirectOvertimeTotal { get; set; }

    /// <summary>
    /// 直接工资率
    /// </summary>
    public decimal? DirectWageRate { get; set; }

    /// <summary>
    /// 间接人数
    /// </summary>
    public int? IndirectLaborCount { get; set; }

    /// <summary>
    /// 间接工资
    /// </summary>
    public decimal? IndirectLaborWage { get; set; }

    /// <summary>
    /// 间接加班小时
    /// </summary>
    public decimal? IndirectOvertimeHours { get; set; }

    /// <summary>
    /// 间接加班总额
    /// </summary>
    public decimal? IndirectOvertimeTotal { get; set; }

    /// <summary>
    /// 间接工资率
    /// </summary>
    public decimal? IndirectWageRate { get; set; }

    /// <summary>
    /// 关联工厂
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
/// StandardWageRate 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktStandardWageRateExportDto
{
    /// <summary>
    /// StandardWageRateID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardWageRateId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 年月（yyyyMM）
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 工作天数
    /// </summary>
    public decimal WorkingDays { get; set; }

    /// <summary>
    /// 销售额
    /// </summary>
    public decimal SalesAmount { get; set; }

    /// <summary>
    /// 直接人数
    /// </summary>
    public int DirectLaborCount { get; set; } = 0;

    /// <summary>
    /// 直接工资
    /// </summary>
    public decimal DirectLaborWage { get; set; }

    /// <summary>
    /// 直接加班小时
    /// </summary>
    public decimal DirectOvertimeHours { get; set; }

    /// <summary>
    /// 直接加班总额
    /// </summary>
    public decimal DirectOvertimeTotal { get; set; }

    /// <summary>
    /// 直接工资率
    /// </summary>
    public decimal DirectWageRate { get; set; }

    /// <summary>
    /// 间接人数
    /// </summary>
    public int IndirectLaborCount { get; set; } = 0;

    /// <summary>
    /// 间接工资
    /// </summary>
    public decimal IndirectLaborWage { get; set; }

    /// <summary>
    /// 间接加班小时
    /// </summary>
    public decimal IndirectOvertimeHours { get; set; }

    /// <summary>
    /// 间接加班总额
    /// </summary>
    public decimal IndirectOvertimeTotal { get; set; }

    /// <summary>
    /// 间接工资率
    /// </summary>
    public decimal IndirectWageRate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

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
