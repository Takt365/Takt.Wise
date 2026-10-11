// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktEmpSalaryDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：EmpSalary 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmpSalary 生成，请按需审阅）
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
// EmpSalary 响应 DTO
// ========================================

/// <summary>
/// 员工薪酬档案（现金报酬定薪记录）
/// 对应前端 TaktEmpSalaryDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmpSalaryDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmpSalaryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpSalaryId { get; set; }


    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int EmpSalaryStatus { get; set; } = 0;

}

// ========================================
// EmpSalary 查询 DTO
// ========================================

/// <summary>
/// EmpSalary 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmpSalaryQueryDto : TaktPagedQuery
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 关联薪级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal? PositionSalary { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal? AllowanceTotal { get; set; }

    /// <summary>
    /// 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryItemId { get; set; }

    /// <summary>
    /// 授予股数/份数（股权激励定薪时使用）
    /// </summary>
    public decimal? EmpSalaryShareCount { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? EmpSalaryStatus { get; set; }

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
// 创建EmpSalary DTO
// ========================================

/// <summary>
/// 创建EmpSalary DTO
/// </summary>
public class TaktEmpSalaryCreateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    [Required(ErrorMessage = "员工姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 关联薪级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal PositionSalary { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal AllowanceTotal { get; set; }

    /// <summary>
    /// 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryItemId { get; set; }

    /// <summary>
    /// 授予股数/份数（股权激励定薪时使用）
    /// </summary>
    public decimal EmpSalaryShareCount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int EmpSalaryStatus { get; set; } = 0;

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
// 更新EmpSalary DTO
// ========================================

/// <summary>
/// 更新EmpSalary DTO
/// 继承 TaktEmpSalaryCreateDto，添加 EmpSalaryId 字段
/// </summary>
public class TaktEmpSalaryUpdateDto : TaktEmpSalaryCreateDto
{
    /// <summary>
    /// EmpSalaryID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpSalaryId { get; set; }

}

// ========================================
// EmpSalary 状态 DTO
// ========================================

/// <summary>
/// EmpSalary 状态更新 DTO
/// </summary>
public class TaktEmpSalaryStatusDto
{
    /// <summary>
    /// EmpSalaryID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpSalaryId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int EmpSalaryStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmpSalary 导入模板行 DTO
/// </summary>
public class TaktEmpSalaryTemplateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 关联薪级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal? PositionSalary { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal? AllowanceTotal { get; set; }

    /// <summary>
    /// 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryItemId { get; set; }

    /// <summary>
    /// 授予股数/份数（股权激励定薪时使用）
    /// </summary>
    public decimal? EmpSalaryShareCount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? EmpSalaryStatus { get; set; }

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
/// EmpSalary 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmpSalaryImportDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 关联薪级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal? PositionSalary { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal? AllowanceTotal { get; set; }

    /// <summary>
    /// 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryItemId { get; set; }

    /// <summary>
    /// 授予股数/份数（股权激励定薪时使用）
    /// </summary>
    public decimal? EmpSalaryShareCount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? EmpSalaryStatus { get; set; }

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
/// EmpSalary 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmpSalaryExportDto
{
    /// <summary>
    /// EmpSalaryID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpSalaryId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪酬体系 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }

    /// <summary>
    /// 关联薪级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal PositionSalary { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal AllowanceTotal { get; set; }

    /// <summary>
    /// 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryItemId { get; set; }

    /// <summary>
    /// 授予股数/份数（股权激励定薪时使用）
    /// </summary>
    public decimal EmpSalaryShareCount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int EmpSalaryStatus { get; set; } = 0;

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
