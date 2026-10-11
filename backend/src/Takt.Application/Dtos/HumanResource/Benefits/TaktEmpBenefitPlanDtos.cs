// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Benefits
// 文件名称：TaktEmpBenefitPlanDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：EmpBenefitPlan 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmpBenefitPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Benefits;

// ========================================
// EmpBenefitPlan 响应 DTO
// ========================================

/// <summary>
/// 员工福利方案（非现金福利参与配置）
/// 对应前端 TaktEmpBenefitPlanDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmpBenefitPlanDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmpBenefitPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpBenefitPlanId { get; set; }

    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    public string? BenefitItemName { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 参保/参与日期
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    public int EmpBenefitStatus { get; set; } = 0;

}

// ========================================
// EmpBenefitPlan 查询 DTO
// ========================================

/// <summary>
/// EmpBenefitPlan 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmpBenefitPlanQueryDto : TaktPagedQuery
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 参保/参与日期（范围查询-开始）
    /// </summary>
    public DateTime? EnrollmentDateStart { get; set; }

    /// <summary>
    /// 参保/参与日期（范围查询-结束）
    /// </summary>
    public DateTime? EnrollmentDateEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    public int? EmpBenefitStatus { get; set; }

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
// 创建EmpBenefitPlan DTO
// ========================================

/// <summary>
/// 创建EmpBenefitPlan DTO
/// </summary>
public class TaktEmpBenefitPlanCreateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    [Required(ErrorMessage = "方案编码不能为空")]
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 参保/参与日期
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    public int EmpBenefitStatus { get; set; } = 0;

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
// 更新EmpBenefitPlan DTO
// ========================================

/// <summary>
/// 更新EmpBenefitPlan DTO
/// 继承 TaktEmpBenefitPlanCreateDto，添加 EmpBenefitPlanId 字段
/// </summary>
public class TaktEmpBenefitPlanUpdateDto : TaktEmpBenefitPlanCreateDto
{
    /// <summary>
    /// EmpBenefitPlanID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpBenefitPlanId { get; set; }

}

// ========================================
// EmpBenefitPlan 状态 DTO
// ========================================

/// <summary>
/// EmpBenefitPlan 状态更新 DTO
/// </summary>
public class TaktEmpBenefitPlanStatusDto
{
    /// <summary>
    /// EmpBenefitPlanID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpBenefitPlanId { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）不能为空")]
    public int EmpBenefitStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EmpBenefitPlan 导入模板行 DTO
/// </summary>
public class TaktEmpBenefitPlanTemplateDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 参保/参与日期
    /// </summary>
    public DateTime? EnrollmentDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    public int? EmpBenefitStatus { get; set; }

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
/// EmpBenefitPlan 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmpBenefitPlanImportDto
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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 参保/参与日期
    /// </summary>
    public DateTime? EnrollmentDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    public int? EmpBenefitStatus { get; set; }

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
/// EmpBenefitPlan 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmpBenefitPlanExportDto
{
    /// <summary>
    /// EmpBenefitPlanID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmpBenefitPlanId { get; set; }

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
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }

    /// <summary>
    /// 方案编码
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 参保/参与日期
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    public int EmpBenefitStatus { get; set; } = 0;

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
