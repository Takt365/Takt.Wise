// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktFinancialPeriodDtos.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：FinancialPeriod 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFinancialPeriod 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// FinancialPeriod 响应 DTO
// ========================================

/// <summary>
/// 财务期间（租户级主数据；按 CountryCode / sys_country_code 区分各国财年规则）
/// 对应前端 TaktFinancialPeriodDto
/// 继承 TaktTenantCoreDtoBase（组合 4）
/// </summary>
public class TaktFinancialPeriodDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// FinancialPeriodID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FinancialPeriodId { get; set; }

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    public string FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    public int CalendarYear { get; set; } = 0;

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    public int CalendarMonth { get; set; } = 0;

    /// <summary>
    /// 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    public string FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

}

// ========================================
// FinancialPeriod 查询 DTO
// ========================================

/// <summary>
/// FinancialPeriod 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFinancialPeriodQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    public string? FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    public int? CalendarYear { get; set; }

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    public int? CalendarMonth { get; set; }

    /// <summary>
    /// 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    public string? FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsBuiltIn { get; set; }

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
// 创建FinancialPeriod DTO
// ========================================

/// <summary>
/// 创建FinancialPeriod DTO
/// </summary>
public class TaktFinancialPeriodCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "国家代码不能为空")]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    [Required(ErrorMessage = "财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）不能为空")]
    public string FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    [Required(ErrorMessage = "会计期间编码（YYYYMM，如 201101、202704）不能为空")]
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    public int CalendarYear { get; set; } = 0;

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    public int CalendarMonth { get; set; } = 0;

    /// <summary>
    /// 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    [Required(ErrorMessage = "财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）不能为空")]
    public string FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

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
// 更新FinancialPeriod DTO
// ========================================

/// <summary>
/// 更新FinancialPeriod DTO
/// 继承 TaktFinancialPeriodCreateDto，添加 FinancialPeriodId 字段
/// </summary>
public class TaktFinancialPeriodUpdateDto : TaktFinancialPeriodCreateDto
{
    /// <summary>
    /// FinancialPeriodID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FinancialPeriodId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FinancialPeriod 导入模板行 DTO
/// </summary>
public class TaktFinancialPeriodTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    public string? FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    public int? CalendarYear { get; set; }

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    public int? CalendarMonth { get; set; }

    /// <summary>
    /// 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    public string? FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsBuiltIn { get; set; }

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
/// FinancialPeriod 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFinancialPeriodImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    public string? FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    public string? PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    public int? CalendarYear { get; set; }

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    public int? CalendarMonth { get; set; }

    /// <summary>
    /// 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    public string? FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int? IsBuiltIn { get; set; }

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
/// FinancialPeriod 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFinancialPeriodExportDto
{
    /// <summary>
    /// FinancialPeriodID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FinancialPeriodId { get; set; }

    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    public string FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    public int CalendarYear { get; set; } = 0;

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    public int CalendarMonth { get; set; } = 0;

    /// <summary>
    /// 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    public string FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

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
